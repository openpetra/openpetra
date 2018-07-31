// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Christopher Jäkel <cj@tbits.net>
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

function autocomplete_motivation_group(input_field) {
	let x = {
		ASearch: $(input_field).val(),
		ALedgerNumber: window.localStorage.getItem('current_ledger'),
		ALimit: 5
		};
	api.post('serverMFinance.asmx/TFinanceServerLookupWebConnector_TypeAheadMotivationCode', x).then(function (result) {
		parsed = JSON.parse(result.data.d);

		if (parsed.result == true) {
			list = [];
			for (key in parsed.AResult) {
				value = parsed.AResult[key];
				list.push({
					key: value.a_motivation_group_code_c,
					display: "<b>" + value.a_motivation_group_code_c + "</b>"
				})
					// label: value.p_partner_short_name_c,
			}
			autocomplete( $(input_field), list);
		}
	})

}
function autocomplete_motivation_detail(input_field) {
	let x = {
		ASearch: $(input_field).val(),
		ALedgerNumber: window.localStorage.getItem('current_ledger'),
		ALimit: 5
		};
	api.post('serverMFinance.asmx/TFinanceServerLookupWebConnector_TypeAheadMotivationCode', x).then(function (result) {
		parsed = JSON.parse(result.data.d);
		console.log(parsed);
		if (parsed.result == true) {
			list = [];
			for (key in parsed.AResult) {
				value = parsed.AResult[key];
				list.push(
					{
						key: value.a_motivation_detail_code_c,
						display: "<b>" + value.a_motivation_detail_code_c + "</b><br>" + value.a_motivation_detail_desc_c
						// label: value.p_partner_short_name_c,
					}
				);
			}
			autocomplete( $(input_field), list);
		}
	})

}
