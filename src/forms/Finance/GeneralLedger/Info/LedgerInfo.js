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

$('document').ready(function () {
  let x = {ALedgerNumber: window.localStorage.getItem('current_ledger')};
  api.post('serverMFinance.asmx/TAPTransactionWebConnector_GetLedgerInfo', x).then(function (data) {
    data = JSON.parse(data.data.d);
    let ledger = data.result[0];
    let to_replace = $('#ledger_info').clone();
    to_replace.find('.current_period').find('span').text( i18next.t( 'LedgerInfo.'+ledger.a_current_period_i+'_month' ) );
    $('.frame').html( format_tpl( to_replace, ledger ) );
  });

  api.post('serverMFinance.asmx/TFinanceServerLookupWebConnector_GetCurrentPostingRangeDates', x).then(function (data) {
    data = JSON.parse(data.data.d);
    let phantom = $('[phantom] .fwd_posting').copy();
    $('#ledger_info').find('.fwd_posting').html( format_tpl( phantom, data ) );
    adjust_date();
  })

  api.post('serverMFinance.asmx/TFinanceServerLookupWebConnector_GetCurrentPeriodDates', x).then(function (data) {
    data = JSON.parse(data.data.d);
    $('#ledger_info').find('.period').html( format_tpl( $('[phantom] .period').clone(), data ) );
    adjust_date();
  })

});

function adjust_date() {
  $('.frame .date').each(function (g, obj) {
    let object = $(obj);
    let time = object.text();

    let new_time = new Date(time).toLocaleDateString();

    object.text(new_time);
    object.removeClass('date');
  });
}
