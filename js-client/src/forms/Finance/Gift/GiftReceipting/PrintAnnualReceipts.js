// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//  Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2019-2021 by SolidCharity.com
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

$(function() {
	var year = (new Date()).getYear() + 1900 - 1;
	$("#StartDate").val(year + "-01-01");
	$("#EndDate").val(year + "-12-31");
	LoadDefaultTemplateFiles();
});

function ReadFile(control, fn) {
	if (control[0].files.length == 0) {
		fn("", "");
		return;
	}

	var reader = new FileReader();

	reader.onload = (function(theFile) {
			base64EncodedFileContent=theFile.target.result;
			// somehow, theFile.name on Firefox is undefined
			filename = theFile.name;
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

var htmltemplate = null;
var logoimage = null;
var signatureimage = null;
var DefaultNames = {};

var htmldata = "";
function SetHtmlTemplate(filename, filedata) {
	htmldata = filedata;

	if (htmldata.length == 0 && DefaultNames["DefaultFileNameHTML"] == "") {
		display_message(i18next.t('PrintAnnualReceipts.emptytemplate'), "fail");
		setTimeout(hidePleaseWait, 500);
		return;
	}

	ReadFile(logoimage, SetLogo);
}

var logodata = "";
var logoname = "";
function SetLogo(filename, filedata) {
	logodata = filedata;
	logoname = filename;

	ReadFile(signatureimage, SetSignature);
}

var signaturedata = "";
var signaturename = "";
function SetSignature(filename, filedata) {
	signaturedata = filedata;
	signaturename = filename;

	GenerateAnnualReceiptsRemote();
}

function GenerateAnnualReceipts() {

	htmltemplate = $('#HTMLTemplate');
	logoimage = $('#LogoImage');
	signatureimage = $('#SignatureImage');

	// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
	if (window.File && window.FileReader && window.FileList && window.Blob) {
		//alert("Great success! All the File APIs are supported.");
	} else {
	  alert('The File APIs are not fully supported in this browser.');
	}

	showPleaseWait();

	ReadFile(htmltemplate, SetHtmlTemplate);
}

function SetTemplateDefault(filename, filedata, purpose) {

	if (filename == "delete") {
		p = {'AFileContent': "",
			'AFileName': "",
			'APurpose': purpose
			};
	} else {
		if (filedata.length == 0) {
			display_message(i18next.t('PrintAnnualReceipts.emptytemplatefile'), "fail");
			return;
		}
		p = {'AFileContent': filedata,
			'AFileName': filename,
			'APurpose': purpose
			};
	}

	api.post('serverMFinance.asmx/TReceiptingWebConnector_StoreDefaultFile', p)
	.then(function (result) {
		var parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
		} else {
			display_message(i18next.t('forms.notsaved'), "fail");
		}
	});
}

function SetHtmlTemplateDefault(filename, filedata) {
	SetTemplateDefault(filename, filedata, "HTML");
	DefaultNames["DefaultFileNameHTML"] = filename;
	insertData("#parameters", DefaultNames);
}

function SetLogoTemplateDefault(filename, filedata) {
	SetTemplateDefault(filename, filedata, "LOGO");
	DefaultNames["DefaultFileNameLogo"] = filename;
	insertData("#parameters", DefaultNames);
}

function SetSignatureTemplateDefault(filename, filedata) {
	SetTemplateDefault(filename, filedata, "SIGN");
	DefaultNames["DefaultFileNameSignature"] = filename;
	insertData("#parameters", DefaultNames);
}

function StoreHtmlTemplateAsDefault() {
	htmltemplate = $('#HTMLTemplate');

	ReadFile(htmltemplate, SetHtmlTemplateDefault);
}

function StoreLogoTemplateAsDefault() {
	logoimage = $('#LogoImage');

	ReadFile(logoimage, SetLogoTemplateDefault);
}

function StoreSignatureTemplateAsDefault() {
	signatureimage = $('#SignatureImage');

	ReadFile(signatureimage, SetSignatureTemplateDefault);
}

function ClearHtmlTemplateAsDefault() {
	SetTemplateDefault("delete", "", "HTML");
	DefaultNames["DefaultFileNameHTML"] = '';
	insertData("#parameters", DefaultNames);
}

function ClearLogoTemplateAsDefault() {
	SetTemplateDefault("delete", "", "LOGO");
	DefaultNames["DefaultFileNameLogo"] = '';
	insertData("#parameters", DefaultNames);
}

function ClearSignatureTemplateAsDefault() {
	SetTemplateDefault("delete", "", "SIGN");
	DefaultNames["DefaultFileNameSignature"] = '';
	insertData("#parameters", DefaultNames);
}

function LoadDefaultTemplateFiles() {
	p = {};

	api.post('serverMFinance.asmx/TReceiptingWebConnector_LoadDefaultTemplateFileNames', p)
	.then(function (result) {
		var parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			DefaultNames["DefaultFileNameHTML"] = parsed.AFileNameHTML;
			DefaultNames["DefaultFileNameLogo"] = parsed.AFileNameLogo;
			DefaultNames["DefaultFileNameSignature"] = parsed.AFileNameSignature;
			insertData("#parameters", DefaultNames);
		}
	});
}

function GenerateAnnualReceiptsRemote() {
	// extract information from a jquery object
	let payload = extract_data($('#parameters'));
	if (payload['p_donor_key_n'] == '') {
		payload['p_donor_key_n'] = 0;
	}

	p = {'AHTMLTemplate': htmldata,
		'ALogoImage': logodata,
		'ALogoFilename': logoname,
		'ASignatureImage': signaturedata,
		'ASignatureFilename': signaturename,
		'ALedgerNumber': window.localStorage.getItem('current_ledger'),
		'AFrequency': 'Annual',
		'AStartDate': payload['AStartDate'],
		'AEndDate': payload['AEndDate'],
		'ALanguage': currentLng(),
		'ADeceasedFirst': true,
		'AExtract': '',
		'ADonorKey': payload['p_donor_key_n']};

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
			display_message(i18next.t('PrintAnnualReceipts.errorempty'), "fail");
		}
		// somehow the operation can finish too soon, and hidePleaseWait would not hide the dialog
		setTimeout(hidePleaseWait, 500);
	})
	.catch(error => {
		console.log(error);
		// somehow the error can be too early, and hidePleaseWait would not hide the dialog
		setTimeout(hidePleaseWait, 500);
		display_message(i18next.t('PrintAnnualReceipts.uploaderror'), "fail");
	});
};

function showPleaseWait() {
	$('#myModal').modal();
}
function hidePleaseWait() {
	$('#myModal').modal('hide');
}
