// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2020-2025 by SolidCharity.com
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

import tpl from '../../../../lib/tpl.js'
import finance from '../../../../lib/finance.js'
import reports from '../../../../lib/reports.js'
import AutocompleteAccCc from '../../../../lib/autocomplete_posting_acc_cc.js'

class TrialBalance {
	constructor() {
	}

	Ready() {
		let self = this;
		finance.get_available_years(
			'#reportfilter [name=param_year_i]',
			'#reportfilter [name=param_start_period_i]',
			'#reportfilter [name=param_end_period_i]',
			function() {
				// load_preset();
			});
		$('#selectYear').on('change', function() { self.updatePeriods($(this).val()) });
		$('#autocomplete_acc_start').on('input', function () {AutocompleteAccCc.autocomplete_a(this)});
		$('#autocomplete_acc_end').on('input', function () {AutocompleteAccCc.autocomplete_a(this)});
		$('#btnCalculate').on('click', function () {self.calculate_report()});
		$('#btnDownloadExcel').on('click', function () {reports.download_excel()});
		$('#btnDownloadPDF').on('click', function () {reports.download_pdf()});
	}

	updatePeriods(selected_year) {
		finance.get_available_periods(selected_year, '#reportfilter [name=param_start_period_i]');
		finance.get_available_periods(selected_year, '#reportfilter [name=param_end_period_i]');
	}

	calculate_report() {
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

		reports.calculate_report_common("forms/Finance/GeneralLedger/Reports/TrialBalance.json", params);
	}
}

export default new TrialBalance();
