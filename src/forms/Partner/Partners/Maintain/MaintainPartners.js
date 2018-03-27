// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <tp@tbits.net>
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.  If not, see <http://www.gnu.org/licenses/>.
//

class MaintainPartnersForm extends JSForm {
	constructor() {
		super('MaintainPartners',
			'serverMPartner.asmx/TSimplePartnerFindWebConnector_FindPartners', {
				AFirstName: '',
				AFamilyNameOrOrganisation: '',
				ACity: '',
				APartnerClass: 'FAMILY',
				AMaxRecords: 25
			});
		super.initEvents();

		// TODO: if no search criteria are defined, then show the 10 last viewed or edited partners
		super.search();
	}

	getMainTableFromResult(result) {
		return result.result;
	}

	getKeyFromRow(row) {
		return row['p_partner_key_n'];
	}

	getDataForEdit(dialogname, key, html) {
		api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_GetPartnerDetails',
			{APartnerKey: key,
			AWithAddressDetails: true,
			AWithSubscriptions: true,
			AWithRelationships: true})
			.then(function(response) {
				if (response.data == null) {
					console.log("error: " + response);
					return;
				}
				var result = JSON.parse(response.data.d);
				if (result.result == "false") {
					console.log("problem loading " + apiUrl);
				} else {
					self.editData = result.result;
					html = self.insertEditDataIntoDialog(self, key, html);
					html = self.showSpecificClass(self, self.editData.PPartner[0], html);
					self.displayDialog(self, dialogname, html);
				}
			});
	}

	saveData(self, dialogname) {
		// console.log(self.editData);

		api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_SavePartner',
			{AMainDS: JSON.stringify(self.editData)})
			.then(function(response) {
				if (response.data == null) {
					console.log("error: " + response);
					return;
				}
				var result = JSON.parse(response.data.d);
				if (result.result == "false") {
					// TODO: show error message
					console.log("problem loading " + apiUrl);
				} else {
					// TODO: show success message
					self.closeEditDialog(dialogname);
				}
			});
	}

	showSpecificClass(self, row, html) {
		var partner_classes = ["FAMILY", "PERSON", "ORGANISATION", "BANK", "UNIT"];
		for (var i in partner_classes) {
			var cl = partner_classes[i];
			if (row['p_partner_class_c'] != cl) {
				html = replaceAll(html, ' class="' + cl + '"', ' class="' + cl + ' hidden"');
			}
		}
		return html;
	}

	insertEditDataIntoDialogDerived(self, dialogname, key, html) {
		// work around for Firefox 52 ESR
		if (typeof this === "undefined") {
			//return self.insertEditDataIntoDialogDerived(self, dialogname, key, html);
		}
		self.getDataForEdit(dialogname, key, html);
		// we don't have the html yet
		return null;
	}
}

$(function() {
	var form = new MaintainPartnersForm();
});
