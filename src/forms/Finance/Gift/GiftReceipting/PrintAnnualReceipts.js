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
				result = result.data.d;
				if (result != '')
				{
					// Convert that text into a byte array.
					var ab = new ArrayBuffer(result.length);
					var ia = new Uint8Array(ab);
					for (var i = 0; i < result.length; i++) {
						ia[i] = result.charCodeAt(i);
					}

					var blob = new Blob([ia], {encoding:"UTF-8", type : "text/html;charset=UTF-8"});
					var url = URL.createObjectURL(blob);
					var a = document.createElement("a");
					a.style = "display: none";
					a.href = url;
					a.download = "AnnualReceipts.html";

					// For Mozilla we need to add the link, otherwise the click won't work
					// see https://support.mozilla.org/de/questions/968992
					document.getElementById('myModal').appendChild(a);

					a.click();
					URL.revokeObjectURL(url);
				}
				else
				{
					console.debug("something went wrong");
				}
				hidePleaseWait();
			})
			.catch(error => {
				console.log(error);
				hidePleaseWait();
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
