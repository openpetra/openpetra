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

function autocomplete_a(input_field) {
	let x = {
		ALedgerNumber: window.localStorage.getItem('current_ledger'),
		ASearch: $(input_field).val(),
		APostingOnly: true,
		AExcludePosting: false,
		AActiveOnly: false,
		ABankAccountOnly: false,
		ALimit: 5
		};
	api.post('serverMFinance.asmx/TFinanceServerLookupWebConnector_TypeAheadAccountCode', x).then(function (result) {
		let parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			list = [];
			for (key in parsed.AResult) {
				value = parsed.AResult[key];
				list.push({key: value.a_account_code_c,
					label: value.a_account_code_short_desc_c,
					display: "<b>" + value.a_account_code_c + "</b><br/>" +
						value.a_account_code_short_desc_c});
			}
			autocomplete( $(input_field), list);
		}
	})
}

function autocomplete_cc(input_field) {
	let x = {
		ALedgerNumber: window.localStorage.getItem('current_ledger'),
		ASearch: $(input_field).val(),
		APostingOnly: true,
		AExcludePosting: false,
		AActiveOnly: false,
		ALocalOnly: true,
		AIndicateInactive: true,
		ALimit: 5
		};
	api.post('serverMFinance.asmx/TFinanceServerLookupWebConnector_TypeAheadCostCentreCode', x).then(function (result) {
		let parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			list = [];
			for (key in parsed.AResult) {
				value = parsed.AResult[key];
				list.push({key: value.a_cost_centre_code_c,
					label: value.a_cost_centre_name_c,
					display: "<b>" + value.a_cost_centre_code_c + "</b><br/>" +
						value.a_cost_centre_name_c});
			}
			autocomplete( $(input_field), list);
		}
	})
}
