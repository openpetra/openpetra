// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 Timotheus Pokorra <tp@tbits.net>
//
// Copyright 2017-2018 by TBits.net
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
		result = result.data.d;
		if (result != '')
		{
			data = result;
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

function showPleaseWait() {
	$('#myModal').modal();
}
function hidePleaseWait() {
	$('#myModal').modal('hide');
}
