// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//  Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra. If not, see <http://www.gnu.org/licenses/>.
//

import i18next from 'i18next'
import i18n from '../../../../lib/i18n.js';
import tpl from '../../../../lib/tpl.js'
import utils from '../../../../lib/utils.js'
import api from '../../../../lib/ajax.js'
import AutocompletePartner from '../../../../lib/autocomplete_partner.js'

class PrintAnnualReceipts {
	constructor() {
		this.htmltemplate = null;
		this.logoimage = null;
		this.signatureimage = null;
		this.receiptaction = null;
		this.DefaultNames = {};
		this.htmldata = "";
		this.logodata = "";
		this.logoname = "";
		this.signaturedata = "";
		this.signaturename = "";
	}

	Ready() {
		let self = this;
		var year = (new Date()).getYear() + 1900 - 1;
		$("#StartDate").val(year + "-01-01");
		$("#EndDate").val(year + "-12-31");
		self.LoadDefaultTemplateFiles();
	}

	ReadFile(control, fn) {
		if (control[0].files.length == 0) {
			fn("", "");
			return;
		}

		var reader = new FileReader();

		reader.onload = (function(theFile) {
				base64EncodedFileContent=theFile.target.result;
				// somehow, theFile.name on Firefox is undefined
				let filename = theFile.name;
				if (filename == undefined) {
					filename = control[0].value.split("\\").pop();
				}
				if (filename == undefined) {
					filename="undefined.txt";
				}
				fn(filename, base64EncodedFileContent);
			});

		reader.readAsDataURL(control[0].files[0]);
	}

	SetHtmlTemplate(filename, filedata) {
		let self = this;
		self.htmldata = filedata;

		if (self.htmldata.length == 0 && self.DefaultNames["DefaultFileNameHTML"] == "") {
			utils.display_message(i18next.t('PrintAnnualReceipts.emptytemplate'), "fail");
			self.hidePleaseWait();
			return;
		}

		self.ReadFile(self.logoimage, self.SetLogo);
	}

	SetLogo(filename, filedata) {
		let self = this;
		self.logodata = filedata;
		self.logoname = filename;

		self.ReadFile(self.signatureimage, self.SetSignature);
	}

	SetSignature(filename, filedata) {
		self.signaturedata = filedata;
		self.signaturename = filename;

		self.GenerateAnnualReceiptsRemote();
	}

	GenerateAnnualReceipts(action) {
		self.receiptaction = action;
		self.htmltemplate = $('#HTMLTemplate');
		self.logoimage = $('#LogoImage');
		self.signatureimage = $('#SignatureImage');

		// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
		if (window.File && window.FileReader && window.FileList && window.Blob) {
			//alert("Great success! All the File APIs are supported.");
		} else {
			alert('The File APIs are not fully supported in this browser.');
		}

		self.showPleaseWait();

		self.ReadFile(self.htmltemplate, self.SetHtmlTemplate);
	}

	SetTemplateDefault(filename, filedata, purpose) {

		if (filename == "delete") {
			let p = {'AFileContent': "",
				'AFileName': "",
				'APurpose': purpose
				};
		} else {
			if (filedata.length == 0) {
				utils.display_message(i18next.t('PrintAnnualReceipts.emptytemplatefile'), "fail");
				return;
			}
			let p = {'AFileContent': filedata,
				'AFileName': filename,
				'APurpose': purpose
				};
		}

		api.post('serverMFinance.asmx/TReceiptingWebConnector_StoreDefaultFile', p)
		.then(function (result) {
			var parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				utils.display_message(i18next.t('forms.saved'), "success");
			} else {
				utils.display_message(i18next.t('forms.notsaved'), "fail");
			}
		});
	}

	SetHtmlTemplateDefault(filename, filedata) {
		let self = this;
		self.SetTemplateDefault(filename, filedata, "HTML");
		self.DefaultNames["DefaultFileNameHTML"] = filename;
		tpl.insertData("#parameters", self.DefaultNames);
	}

	SetLogoTemplateDefault(filename, filedata) {
		let self = this;
		self.SetTemplateDefault(filename, filedata, "LOGO");
		self.DefaultNames["DefaultFileNameLogo"] = filename;
		tpl.insertData("#parameters", self.DefaultNames);
	}

	SetSignatureTemplateDefault(filename, filedata) {
		let self = this;
		self.SetTemplateDefault(filename, filedata, "SIGN");
		self.DefaultNames["DefaultFileNameSignature"] = filename;
		tpl.insertData("#parameters", self.DefaultNames);
	}

	StoreHtmlTemplateAsDefault() {
		let self = this;
		self.htmltemplate = $('#HTMLTemplate');

		self.ReadFile(self.htmltemplate, self.SetHtmlTemplateDefault);
	}

	StoreLogoTemplateAsDefault() {
		self.logoimage = $('#LogoImage');

		self.ReadFile(self.logoimage, self.SetLogoTemplateDefault);
	}

	StoreSignatureTemplateAsDefault() {
		let self = this;
		self.signatureimage = $('#SignatureImage');

		self.ReadFile(self.signatureimage, self.SetSignatureTemplateDefault);
	}

	ClearHtmlTemplateAsDefault() {
		let self = this;
		self.SetTemplateDefault("delete", "", "HTML");
		self.DefaultNames["DefaultFileNameHTML"] = '';
		tpl.insertData("#parameters", self.DefaultNames);
	}

	ClearLogoTemplateAsDefault() {
		let self = this;
		self.SetTemplateDefault("delete", "", "LOGO");
		self.DefaultNames["DefaultFileNameLogo"] = '';
		tpl.insertData("#parameters", self.DefaultNames);
	}

	ClearSignatureTemplateAsDefault() {
		let self = this;
		self.SetTemplateDefault("delete", "", "SIGN");
		self.DefaultNames["DefaultFileNameSignature"] = '';
		tpl.insertData("#parameters", self.DefaultNames);
	}

	LoadDefaultTemplateFiles() {
		let self = this;
		let p = {};

		api.post('serverMFinance.asmx/TReceiptingWebConnector_LoadReceiptDefaults', p)
		.then(function (result) {
			let data = result.data.d.replaceAll("\n", "<br/>");
			var parsed = JSON.parse(data);
			if (parsed.result == true) {
				self.DefaultNames["DefaultFileNameHTML"] = parsed.AFileNameHTML;
				self.DefaultNames["DefaultFileNameLogo"] = parsed.AFileNameLogo;
				self.DefaultNames["DefaultFileNameSignature"] = parsed.AFileNameSignature;
				self.DefaultNames["AEmailSubject"] = parsed.AEmailSubject;
				self.DefaultNames["AEmailBody"] = parsed.AEmailBody.replaceAll("<br/>", "\n");
				self.DefaultNames["AEmailFrom"] = parsed.AEmailFrom;
				self.DefaultNames["AEmailFromName"] = parsed.AEmailFromName;
				self.DefaultNames["AEmailFilename"] = parsed.AEmailFilename;
				tpl.insertData("#parameters", self.DefaultNames);
			}
		});
	}

	GenerateAnnualReceiptsRemote() {
		let self = this;
		// extract information from a jquery object
		let payload = tpl.extract_data($('#parameters'));
		if (payload['p_donor_key_n'] == '') {
			payload['p_donor_key_n'] = 0;
		}

		if (self.receiptaction == "email") {
			// TODO confirm that emails will actually be sent
		}

		let p = {'AHTMLTemplate': self.htmldata,
			'ALogoImage': self.logodata,
			'ALogoFilename': self.logoname,
			'ASignatureImage': self.signaturedata,
			'ASignatureFilename': self.signaturename,
			'AEmailSubject': payload['AEmailSubject'],
			'AEmailBody': payload['AEmailBody'],
			'AEmailFrom': payload['AEmailFrom'],
			'AEmailFromName': payload['AEmailFromName'],
			'AEmailFilename': payload['AEmailFilename'],
			'ALedgerNumber': window.localStorage.getItem('current_ledger'),
			'AFrequency': 'Annual',
			'AStartDate': payload['AStartDate'],
			'AEndDate': payload['AEndDate'],
			'ALanguage': i18n.currentLng(),
			'ADeceasedFirst': true,
			'AExtract': '',
			'ADonorKey': payload['p_donor_key_n'],
			'AAction': self.receiptaction,
			'AOnlyTest': payload['AOnlyTest']};

		api.post('serverMFinance.asmx/TReceiptingWebConnector_CreateAnnualGiftReceipts', p)
		.then(function (result) {
			var parsed = JSON.parse(result.data.d);
			if (parsed.result == true)
			{
				var byteString = atob(parsed.APDFReceipt);

				// Convert that text into a byte array.
				var ab = new ArrayBuffer(byteString.length);
				var ia = new Uint8Array(ab);
				for (var i = 0; i < byteString.length; i++) {
					ia[i] = byteString.charCodeAt(i);
				}

				var blob = new Blob([ia], {type : "application/pdf"});
				var url = URL.createObjectURL(blob);
				var a = document.createElement("a");
				a.style = "display: none";
				a.href = url;

				var year = payload['AEndDate'].substring(0, 4);
				a.download = i18next.t('PrintAnnualReceipts.annual_receipt_file') + year + '.pdf';

				document.body.appendChild(a);

				a.click();
				URL.revokeObjectURL(url);
				a.remove();
			}
			else
			{
				utils.display_error(parsed.AVerification);
			}
			self.hidePleaseWait();
		})
		.catch(error => {
			console.log(error);
			self.hidePleaseWait();
			utils.display_message(i18next.t('PrintAnnualReceipts.uploaderror'), "fail");
		});
	};

	showPleaseWait() {
		$('#myModal').modal();
	}
	hidePleaseWait() {
		// somehow the operation can finish too soon, so delay it by a little
		setTimeout(function() { $('#myModal').modal('hide'); }, 500);
	}
}

export default new PrintAnnualReceipts();