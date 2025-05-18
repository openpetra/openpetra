// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
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

import i18next from 'i18next'
import api from './ajax.js'

class Finance {

	get_available_years(destVariableYear, destVariablePeriod1, destVariablePeriod2, fn_to_call) {
		let self = this;
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
		};
		api.post('serverMFinance.asmx/TAccountingPeriodsWebConnector_GetAvailableGLYears', x).then(function (data) {
			data = JSON.parse(data.data.d);
			let r = data.result;
			let currentYearNumber = -1;
			for (var year of r) {
				let y = $('<option value="'+year.YearNumber+'">'+year.YearDate+'</option>');
				if (year.YearNumber > currentYearNumber) {
					currentYearNumber = year.YearNumber;
				}
				$(destVariableYear).append(y);
			}

			if (destVariablePeriod2 != '') {
				self.get_available_periods(currentYearNumber, destVariablePeriod1, fn_to_call, false);
				self.get_available_periods(currentYearNumber, destVariablePeriod2, fn_to_call, false);
			} else {
				self.get_available_periods(currentYearNumber, destVariablePeriod1, fn_to_call, true);
			}
		})
	}

	get_available_periods(year, destVariable, fn_to_call, OfferMultiplePeriods) {
		let selectedPeriod = $(destVariable).val();
		if (selectedPeriod == null) {
			selectedPeriod = -1;
		}
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			AFinancialYear: year,
		};
		api.post('serverMFinance.asmx/TAccountingPeriodsWebConnector_GetAvailablePeriods', x).then(function (data) {
			data = JSON.parse(data.data.d);
			let r = data.result;
			$(destVariable).html('');
			for (var period of r) {
				if (!OfferMultiplePeriods && period.PeriodNumber < 1) continue;
				let translated = period.PeriodName;
				let transsplit = translated.split(' ');
				if (transsplit[1] != undefined) {
					// separate the year
					translated = i18next.t('SelectPeriods.' + transsplit[0]) + ' ' + transsplit[1];
				} else {
					translated = i18next.t('SelectPeriods.' + translated);
				}
				let selected = (period.PeriodNumber == selectedPeriod)?'selected':'';
				let y = $('<option value="'+period.PeriodNumber+'" ' + selected + '>'+translated+'</option>');
				$(destVariable).append(y);
			}

			if (fn_to_call != undefined) {
				fn_to_call();
			}
		})
	}
}

export default new Finance();