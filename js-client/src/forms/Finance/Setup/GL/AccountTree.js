// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//       Christopher Jäkel <cj@tbits.net>
//
// Copyright 2017-2018 by TBits.net
// Copyright 2020-2025 by SolidCharity.com
//
// This file is part of OpenPetra.
//
// OpenPetra is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.  If not, see <http://www.gnu.org/licenses/>.
//

import i18next from 'i18next'
import tpl from '../../../../lib/tpl.js'
import api from '../../../../lib/ajax.js'
import utils from '../../../../lib/utils.js'
import modal from '../../../../lib/modal.js'

class AccountTree {

	Ready() {
		let self = this;
		self.LoadTree();
		$('#btnExport').on('click', function() {self.export_file()});
		$('#btnImport').on('click', function() {$('#fileUpload').click()});
		$('#fileUpload').on('change', function() { self.import_file($(this)) });
	}

	LoadTree() {
		let self = this;
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			AAccountHierarchyCode: 'STANDARD'
		};
		api.post('serverMFinance.asmx/TGLSetupWebConnector_LoadAccountHierarchyHtmlCode', x).then(function (data) {
			let _html_ = data.data.d;
			$('#browse_container').html(_html_);
		})
	}

	import_file(filectrl) {
		let self = this;

		// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
		if (window.File && window.FileReader && window.FileList && window.Blob) {
			//alert("Great success! All the File APIs are supported.");
		} else {
			alert('The File APIs are not fully supported in this browser.');
		}

		var reader = new FileReader();

		reader.onload = (function(theFile) {
			return function(e) {
				let s = e.target.result;

				// avoid issues with security checks
				s = s.replaceAll('<', '&lt;');
				s = s.replaceAll('>', '&gt;');

				let p = {AYmlAccountHierarchy: s,
					AHierarchyName: 'STANDARD',
					ALedgerNumber: window.localStorage.getItem('current_ledger')};
				api.post('serverMFinance.asmx/TGLSetupWebConnector_ImportAccountHierarchy', p)
				.then(function (result) {
					result = result.data;
					if (result != '') {
						var parsed = JSON.parse(result.d);
						if (parsed.result == true) {
							utils.display_message(i18next.t('AccountTree.uploadsuccess'), "success");
							self.LoadTree();
						} else {
							utils.display_error(parsed.AVerificationResult);
						}
					}
				})
				.catch(error => {
					if (error.response.data.Message != null) {
						utils.display_message(error.response.data.Message, "fail");
					} else {
						utils.display_message(i18next.t('AccountTree.uploaderror'), "fail");
					}
				});
			};
		})(filectrl[0].files[0]);

		// Read in the file as a data URL.
		reader.readAsText(filectrl[0].files[0]);
	};

	export_file() {
		let self = this;
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			AAccountHierarchyName: 'STANDARD'
		};
		api.post('serverMFinance.asmx/TGLSetupWebConnector_ExportAccountHierarchyYml', x).then(function (data) {
			var parsed = JSON.parse(data.data.d);
			var _file_ = utils.b64DecodeUnicode(parsed.AHierarchyYml);
			var link = document.createElement("a");
			link.style = "display: none";
			link.href = 'data:text/plain;charset=utf-8,'+encodeURIComponent(_file_);
			link.download = i18next.t('AccountTree.accounts_file') + '.yml';
			document.body.appendChild(link);
			link.click();
			link.remove();
		})
	}

} // end of class

export default new AccountTree();