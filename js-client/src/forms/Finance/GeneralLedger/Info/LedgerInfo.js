// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//       Christopher Jäkel
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
import tpl from '../../../../lib/tpl.js'
import api from '../../../../lib/ajax.js'

class LedgerInfo {

	Ready() {
		let self = this;

		let x = {ALedgerNumber: window.localStorage.getItem('current_ledger')};
		api.post('serverMFinance.asmx/TAPTransactionWebConnector_GetLedgerInfo', x).then(function (data) {
			data = JSON.parse(data.data.d);
			let ledger = data.result[0];
			let to_replace = $('#ledger_info').clone();
			to_replace.find('.current_period').find('span').text( i18next.t( 'LedgerInfo.'+ledger.a_current_period_i+'_month' ) );
			$('.frame').html( tpl.format_tpl( to_replace, ledger ) );
		});

		api.post('serverMFinance.asmx/TFinanceServerLookupWebConnector_GetCurrentPostingRangeDates', x).then(function (data) {
			data = JSON.parse(data.data.d);
			$('#ledger_info').find('.fwd_posting').html( tpl.format_tpl( $('[phantom] .fwd_posting'), data ) );
		})

		api.post('serverMFinance.asmx/TFinanceServerLookupWebConnector_GetCurrentPeriodDates', x).then(function (data) {
			data = JSON.parse(data.data.d);
			$('#ledger_info').find('.period').html( tpl.format_tpl( $('[phantom] .period'), data ) );
		})
	}
} // end of class

export default new LedgerInfo();
