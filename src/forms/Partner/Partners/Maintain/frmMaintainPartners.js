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
		super('frmMaintainPartners',
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

	showSpecificClass(self, html) {
		var partner_classes = ["FAMILY", "PERSON", "ORGANISATION", "BANK", "UNIT"];
			console.log(self.row['p_partner_class_c']);
		for (var i in partner_classes) {
			var cl = partner_classes[i];
			console.log (partner_classes[cl]);
			if (self.row['p_partner_class_c'] != cl) {
				html = replaceAll(html, ' class="' + cl + '"', ' class="' + cl + ' hidden"');
			}
		}
		return html;
	}

	insertEditDataIntoDialog(self, html) {
		// work around for Firefox 52 ESR
                if (typeof this === "undefined") {
			html = self.insertEditDataIntoDialog(self, html);
		} else {
			html = super.insertEditDataIntoDialog(self, html);
		}	
		html = self.showSpecificClass(self, html);
		return html;
	}
}

$(function() {
	var form = new MaintainPartnersForm();
});
