// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Christopher Jäkel <cj@tbits.net>
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2017-2018 by TBits.net
// Copyright 2019-2023 by SolidCharity.com
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
	api.post('serverMFinance.asmx/TFinanceServerLookupWebConnector_TypeAheadMotivationGroup', x).then(function (result) {
		let parsed = JSON.parse(result.data.d);

		if (parsed.result == true) {
			list = [];
			for (key in parsed.AResult) {
				value = parsed.AResult[key];
				list.push({
					key: value.a_motivation_group_code_c,
					label: value.a_motivation_group_description_c,
					display: "<b>" + value.a_motivation_group_code_c + "</b><br/>" + value.a_motivation_group_description_c
				})
			}
			autocomplete( $(input_field), list);
		}
	})

}
function autocomplete_motivation_detail(input_field, onselect=null) {
	let x = {
		ASearch: $(input_field).val(),
		ALedgerNumber: window.localStorage.getItem('current_ledger'),
		ALimit: 5
		};
	api.post('serverMFinance.asmx/TFinanceServerLookupWebConnector_TypeAheadMotivationDetail', x).then(function (result) {
		let parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			list = [];
			for (key in parsed.AResult) {
				value = parsed.AResult[key];
				list.push(
					{
						groupkey: value.a_motivation_group_code_c,
						membership: value.a_membership_l,
						sponsorship: value.a_sponsorship_l,
						workersupport: value.a_worker_support_l,
						key: value.a_motivation_detail_code_c,
						label: value.a_motivation_detail_desc_c,
						display: "<b>" + value.a_motivation_detail_code_c + "</b><br>" + value.a_motivation_detail_desc_c
					}
				);
			}
			autocompleteWithGroup( $(input_field), $("[name=a_motivation_group_code_c]"), list, onselect);
		}
	})

}
