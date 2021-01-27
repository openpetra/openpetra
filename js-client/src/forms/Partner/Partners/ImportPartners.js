// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Christopher Jäkel
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.  If not, see <http://www.gnu.org/licenses/>.
//

function UploadUserCSV(self) {

	var filename = self.val();

	// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
	if (window.File && window.FileReader && window.FileList && window.Blob) {
		//alert("Great success! All the File APIs are supported.");
	} else {
	  alert('The File APIs are not fully supported in this browser.');
	}

	var reader = new FileReader();

	reader.onload = function (event) {
		p = {'ACSVPartnerData': event.target.result, 'ADateFormat': "dmy", "ASeparator": ";"};

		api.post('serverMPartner.asmx/TImportExportWebConnector_ImportFromCSVFile', p)
		.then(function (result) {
			result = JSON.parse(result.data.d);
			if (result.result == true) {
				display_message(i18next.t('ImportPartners.upload_partner_success'), "success");
			} else {
				display_error(result.AVerificationResult, 'ImportPartners', 'ImportPartners.upload_partner_fail');
			}
		})
		.catch(error => {
			//console.log(error.response)
			display_message(i18next.t('ImportPartners.upload_partner_fail'), "fail");
		});

	}
	// Read in the file as a data URL.
	reader.readAsText(self[0].files[0]);

};

function UploadUserODS(self) {

	var filename = self.val();

	// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
	if (window.File && window.FileReader && window.FileList && window.Blob) {
		//alert("Great success! All the File APIs are supported.");
	} else {
	  alert('The File APIs are not fully supported in this browser.');
	}

	var reader = new FileReader();

	reader.onload = function (theFile) {
		p = {'AODSPartnerData': theFile.target.result,};

		api.post('serverMPartner.asmx/TImportExportWebConnector_ImportFromODSFile', p)
		.then(function (result) {
			result = JSON.parse(result.data.d);
			if (result.result == true) {
				display_message(i18next.t('ImportPartners.upload_partner_success'), "success");
			} else {
				display_error(result.AVerificationResult, 'ImportPartners', 'ImportPartners.upload_partner_fail');
			}
		})
		.catch(error => {
			//console.log(error.response)
			display_message(i18next.t('ImportPartners.upload_partner_fail'), "fail");
		});

	}

	// Read in the file as a data URL.
	reader.readAsDataURL(self[0].files[0]);
};

function UploadUserXLSX(self) {

	var filename = self.val();

	// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
	if (window.File && window.FileReader && window.FileList && window.Blob) {
		//alert("Great success! All the File APIs are supported.");
	} else {
	  alert('The File APIs are not fully supported in this browser.');
	}

	var reader = new FileReader();

	reader.onload = function (theFile) {
		p = {'AXLSXPartnerData': theFile.target.result};

		api.post('serverMPartner.asmx/TImportExportWebConnector_ImportFromXLSXFile', p)
		.then(function (result) {
			result = JSON.parse(result.data.d);
			if (result.result == true) {
				display_message(i18next.t('ImportPartners.upload_partner_success'), "success");
			} else {
				display_error(result.AVerificationResult, 'ImportPartners', 'ImportPartners.upload_partner_fail');
			}
		})
		.catch(error => {
			//console.log(error.response)
			display_message(i18next.t('ImportPartners.upload_partner_fail'), "fail");
		});

	}

	// Read in the file as a data URL.
	reader.readAsDataURL(self[0].files[0]);
};
