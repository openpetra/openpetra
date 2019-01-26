// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//  Timotheus Pokorra <timotheus@pokorra@solidcharity.com>
//
// Copyright 2019 by SolidCharity.com
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

function UploadHTMLTemplate() {
	$('#fileHTMLTemplate').click();
}

function GenerateAnnualReceipts(self) {

	var filename = self.val();

	// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
	if (window.File && window.FileReader && window.FileList && window.Blob) {
		//alert("Great success! All the File APIs are supported.");
	} else {
	  alert('The File APIs are not fully supported in this browser.');
	}

	var reader = new FileReader();

	reader.onload = (function(theFile) {
		return function(e) {
			base64EncodedFileContent=e.target.result;
			showPleaseWait();

			// extract information from a jquery object
			let payload = extract_data($('#parameters'));

			p = {'AHTMLTemplate': base64EncodedFileContent,
				'ALedgerNumber': window.localStorage.getItem('current_ledger'),
				'AFrequency': 'Annual',
				'AStartDate': payload['AStartDate'],
				'AEndDate': payload['AEndDate'],
				'ALanguage': currentLng(),
				'ADeceasedFirst': true,
				'AExtract': '',
				'ADonorKey': 0};
			api.post('serverMFinance.asmx/TReceiptingWebConnector_CreateAnnualGiftReceipts', p)
			.then(function (result) {
				var parsed = JSON.parse(result.data.d);
				if (parsed.result == true)
				{
					var _file_ = b64DecodeUnicode(parsed.AHTMLReceipt);
					var link = document.createElement("a");
					link.style = "display: none";
					link.href = 'data:text/html;charset=utf-8,'+encodeURIComponent(_file_);
					var year = payload['AEndDate'].substring(0, 4);
					link.download = i18next.t('PrintAnnualReceipts.annual_receipt_file') + year + '.html';
					document.body.appendChild(link);
					link.click();
					link.remove();
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
	})(self[0].files[0]);

	// Read in the file as a data URL.
	reader.readAsDataURL(self[0].files[0]);
};

function showPleaseWait() {
	$('#myModal').modal();
}
function hidePleaseWait() {
	$('#myModal').modal('hide');
}
