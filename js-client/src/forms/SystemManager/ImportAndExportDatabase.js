// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2017-2018 by TBits.net
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
// along with OpenPetra.	If not, see <http://www.gnu.org/licenses/>.
//

function ExportAllData() {
	showPleaseWait();
	api.post('serverMSysMan.asmx/TImportExportWebConnector_ExportAllTables', {}).then(function (result) {
		var parsed = JSON.parse(result.data.d);
		if (parsed.result == true)
		{
			data = parsed.ADataYmlGzBase64;
			var byteString = atob(data);

			// Convert that text into a byte array.
			var ab = new ArrayBuffer(byteString.length);
			var ia = new Uint8Array(ab);
			for (var i = 0; i < byteString.length; i++) {
				ia[i] = byteString.charCodeAt(i);
			}

			var blob = new Blob([ia], {type : "application/gzip"});
			var url = URL.createObjectURL(blob);
			var a = document.createElement("a");
			a.style = "display: none";
			a.href = url;
			a.download = "exportedDatabase.yml.gz";

			// For Mozilla we need to add the link, otherwise the click won't work
			// see https://support.mozilla.org/de/questions/968992
			document.body.appendChild(a);

			a.click();
			URL.revokeObjectURL(url);
		}
		else
		{
			console.debug("something went wrong");
		}
		hidePleaseWait();
	});
}

function ResetAllData() {
	let s = confirm( i18next.t('ImportAndExportDatabase.ask_reset') );
	if (!s) {return}

	$('#fileResetDatabase').click();
}

function ResetDatabase(self) {

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
			s=e.target.result;
			showPleaseWait();
			base64EncodedFileContent = s.substring(s.indexOf("base64,") + "base64,".length);

			p = {'AZippedNewDatabaseData': base64EncodedFileContent};
			api.post('serverMSysMan.asmx/TImportExportWebConnector_ResetDatabase', p)
			.then(function (result) {
				result = result.data;
				if (result != '') {
					hidePleaseWait();
					if (result.d == true) {
						utils.display_message(i18next.t('ImportAndExportDatabase.uploadsuccess'), "success");
					} else {
						utils.display_message(i18next.t('ImportAndExportDatabase.uploaderror'), "fail");
					}
				}
			})
			.catch(error => {
				//console.log(error.response)
				hidePleaseWait();
				utils.display_message(i18next.t('ImportAndExportDatabase.uploaderror'), "fail");
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
	// somehow the operation can finish too soon, so delay it by a little
	setTimeout(function() { $('#myModal').modal('hide'); }, 500);
}
