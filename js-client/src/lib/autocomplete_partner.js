// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//	Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2017-2018 by TBits.net
// Copyright 2019-2022 by SolidCharity.com
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

var timeout_autocomplete = null;

function autocomplete_donor(input_field, onselect = null) {

	if (timeout_autocomplete) {
		clearTimeout(timeout_autocomplete);
	}
	timeout_autocomplete = setTimeout(function() {
		let x = {
			ASearch: $(input_field).val(),
			APartnerClass: '',
			AActiveOnly: false,
			ALimit: 5
			};
		api.post('serverMPartner.asmx/TSimplePartnerFindWebConnector_TypeAheadPartnerFind', x).then(function (result) {
			parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				list = [];
				for (key in parsed.AResult) {
					value = parsed.AResult[key];
					list.push({key: value.p_partner_key_n,
						label: value.p_partner_short_name_c,
						display: (value.p_status_code_c != "ACTIVE"? value.p_status_code_c + " ":"") + 
							"<b>" + value.p_partner_short_name_c + "</b> (" + value.p_partner_key_n + ")<br/>" +
							value.p_street_name_c + "<br/>" + value.p_postal_code_c + " " + value.p_city_c});
				}
				autocomplete( $(input_field), list, onselect);
			} else {
				list = [];
				autocomplete( $(input_field), list, onselect);
			}
		});
	}, 500);
}
