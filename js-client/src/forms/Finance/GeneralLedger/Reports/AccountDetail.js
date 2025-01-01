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

var last_opened_entry_data = {};

$('document').ready(function () {
	get_available_years(
		'#reportfilter [name=param_year_i]',
		'#reportfilter [name=param_start_period_i]',
		'#reportfilter [name=param_end_period_i]',
		function() {
			load_preset();
		});
});

function updatePeriods(selected_year) {
	get_available_periods(selected_year, '#reportfilter [name=param_start_period_i]', false);
	get_available_periods(selected_year, '#reportfilter [name=param_end_period_i]', false);
}

function calculate_report() {
	let obj = $('#reportfilter');
	// extract information from a jquery object
	let params = tpl.extract_data(obj);

	params['param_ledger_number_i'] = window.localStorage.getItem('current_ledger');
	params['param_account_codes'] = '';
	params['param_rgrAccounts'] = '';
	params['param_rgrCostCentres'] = '';
	params['param_reference_start'] = '';
	params['param_start_period'] -= 1;
	params['param_end_period'] -= 1;
	params['param_cost_centre_code_start'] = '*NOTUSED*';
	// TODO: param_start_date, param_end_date

	calculate_report_common("forms/Finance/GeneralLedger/Reports/AccountDetail.json", params);
}
