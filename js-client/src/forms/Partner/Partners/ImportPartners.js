// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Christopher Jäkel
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2017-2018 by TBits.net
// Copyright 2019-2025 by SolidCharity.com
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
import api from '../../../lib/ajax.js'
import utils from '../../../lib/utils.js'

class ImportPartners {

	Ready() {
		let self = this;
		$('#btnImportData').on('click', function() {$('#fileUploadUserCSV').click()});
		$('#btnImportDataODS').on('click', function() {$('#fileUploadUserODS').click()});
		$('#btnImportDataXLSX').on('click', function() {$('#fileUploadUserXLSX').click()});
		$('#fileUploadUserCSV').on('change', function() { self.UploadUserCSV($(this)) });
		$('#fileUploadUserODS').on('change', function() { self.UploadUserODS($(this)) });
		$('#fileUploadUserXLSX').on('change', function() { self.UploadUserXLSX($(this)) });
		$('#btnDeleteAllContacts').on('click', function() {self.DeleteAllContacts()});
	}

	UploadUserCSV(filectrl) {

		let self = this;
		var filename = filectrl.val();

		// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
		if (window.File && window.FileReader && window.FileList && window.Blob) {
			//alert("Great success! All the File APIs are supported.");
		} else {
			alert('The File APIs are not fully supported in this browser.');
		}

		var reader = new FileReader();

		reader.onload = function (event) {
			let p = {'ACSVPartnerData': event.target.result, 'ADateFormat': "dmy", "ASeparator": ";"};

			self.showPleaseWait();

			api.post('serverMPartner.asmx/TImportExportWebConnector_ImportFromCSVFile', p)
			.then(function (result) {
				result = JSON.parse(result.data.d);
				self.hidePleaseWait();
				if (result.result == true) {
					utils.display_message(i18next.t('ImportPartners.upload_partner_success'), "success");
				} else {
					utils.display_error(result.AVerificationResult, 'ImportPartners', 'ImportPartners.upload_partner_fail');
				}
			})
			.catch(error => {
				//console.log(error.response)
				self.hidePleaseWait();
				utils.display_message(i18next.t('ImportPartners.upload_partner_fail'), "fail");
			});

		}
		// Read in the file as a data URL.
		reader.readAsText(filectrl[0].files[0]);

	};

	UploadUserODS(filectrl) {

		let self = this;
		var filename = filectrl.val();

		// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
		if (window.File && window.FileReader && window.FileList && window.Blob) {
			//alert("Great success! All the File APIs are supported.");
		} else {
			alert('The File APIs are not fully supported in this browser.');
		}

		var reader = new FileReader();

		reader.onload = function (theFile) {
			let p = {'AODSPartnerData': theFile.target.result,};

			self.showPleaseWait();

			api.post('serverMPartner.asmx/TImportExportWebConnector_ImportFromODSFile', p)
			.then(function (result) {
				result = JSON.parse(result.data.d);
				self.hidePleaseWait();
				if (result.result == true) {
					utils.display_message(i18next.t('ImportPartners.upload_partner_success'), "success");
				} else {
					utils.display_error(result.AVerificationResult, 'ImportPartners', 'ImportPartners.upload_partner_fail');
				}
			})
			.catch(error => {
				//console.log(error.response)
				self.hidePleaseWait();
				utils.display_message(i18next.t('ImportPartners.upload_partner_fail'), "fail");
			});

		}

		// Read in the file as a data URL.
		reader.readAsDataURL(filectrl[0].files[0]);
	};

	UploadUserXLSX(filectrl) {

		let self = this;
		var filename = filectrl.val();

		// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
		if (window.File && window.FileReader && window.FileList && window.Blob) {
			//alert("Great success! All the File APIs are supported.");
		} else {
			alert('The File APIs are not fully supported in this browser.');
		}

		var reader = new FileReader();

		reader.onload = function (theFile) {
			let p = {'AXLSXPartnerData': theFile.target.result};

			self.showPleaseWait();

			api.post('serverMPartner.asmx/TImportExportWebConnector_ImportFromXLSXFile', p)
			.then(function (result) {
				result = JSON.parse(result.data.d);
				self.hidePleaseWait();
				if (result.result == true) {
					utils.display_message(i18next.t('ImportPartners.upload_partner_success'), "success");
				} else {
					utils.display_error(result.AVerificationResult, 'ImportPartners', 'ImportPartners.upload_partner_fail');
				}
			})
			.catch(error => {
				//console.log(error.response)
				self.hidePleaseWait();
				utils.display_message(i18next.t('ImportPartners.upload_partner_fail'), "fail");
			});

		}

		// Read in the file as a data URL.
		reader.readAsDataURL(filectrl[0].files[0]);
	};

	DeleteAllContacts() {

		let self = this;
		let s = confirm( i18next.t('ImportPartners.ask_delete_all') );
		if (!s) {return}

		self.showPleaseWait();

		let p = {};
		api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_DeleteAllPartners', p)
		.then(function (result) {
			result = JSON.parse(result.data.d);
			self.hidePleaseWait();
			if (result.result == true) {
				utils.display_message(i18next.t('ImportPartners.delete_all_partner_success'), "success");
			} else {
				utils.display_error(result.AVerificationResult, 'ImportPartners', 'ImportPartners.delete_all_partners_fail');
			}
		})
		.catch(error => {
			//console.log(error.response)
			self.hidePleaseWait();
			utils.display_message(i18next.t('ImportPartners.delete_all_partners_fail'), "fail");
		});
	}

	showPleaseWait() {
		$('#myModal').modal();
	}
	hidePleaseWait() {
		// somehow the operation can finish too soon, so delay it by a little
		setTimeout(function() { $('#myModal').modal('hide'); }, 500);
	}

} // end of class

export default new ImportPartners();