// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <tp@tbits.net>
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

function month_end() {

	let x = {ALedgerNumber: window.localStorage.getItem('current_ledger')};
	api.post('serverMFinance.asmx/TPeriodIntervalConnector_PeriodMonthEnd', x).then(function (data) {
		let parsed = JSON.parse(data.data.d);
		let s = false;
		if (parsed.result == true) {
			display_message( i18next.t('forms.saved'), 'success' )
		}
		else {
			display_error( parsed.AVerificationResults );
		}
	});
}
