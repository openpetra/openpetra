// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//	Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
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

import api from './ajax.js';
import autocomplete from './autocomplete.js'

class AutocompletePartner {

	constructor() {
		this.timeout_autocomplete = null;
	}

	autocomplete_contact(input_field, onselect = null, memberonly = false) {
		let self = this;
		if (self.timeout_autocomplete) {
			clearTimeout(self.timeout_autocomplete);
		}
		self.timeout_autocomplete = setTimeout(function() {
			let x = {
				ASearch: $(input_field).val(),
				APartnerClass: '',
				AActiveOnly: false,
				AMemberOnly: memberonly,
				ALimit: 5
				};
			api.post('serverMPartner.asmx/TSimplePartnerFindWebConnector_TypeAheadPartnerFind', x).then(function (result) {
				let parsed = JSON.parse(result.data.d);
				if (parsed.result == true) {
					let list = [];
					for (var key in parsed.AResult) {
						let value = parsed.AResult[key];
						list.push({key: value.p_partner_key_n,
							label: value.p_partner_short_name_c,
							display: (value.p_status_code_c != "ACTIVE"? value.p_status_code_c + " ":"") + 
								"<b>" + value.p_partner_short_name_c + "</b> (" + value.p_partner_key_n + ")<br/>" +
								value.p_street_name_c + "<br/>" + value.p_postal_code_c + " " + value.p_city_c});
					}
					autocomplete.autocomplete( $(input_field), list, onselect);
				} else {
					let list = [];
					autocomplete.autocomplete( $(input_field), list, onselect);
				}
			});
		}, 500);
	}

	autocomplete_donor(input_field, onselect = null) {
		let self = this;
		self.autocomplete_contact(input_field, onselect, false);
	}

	autocomplete_member(input_field, onselect = null) {
		let self = this;
		self.autocomplete_contact(input_field, onselect, true);
	}
}

export default new AutocompletePartner();
