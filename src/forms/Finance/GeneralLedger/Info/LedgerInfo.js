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
    $('#ledger_info').find('.fwd_posting').html( format_tpl( $('[phantom] .fwd_posting'), data ) );
    adjust_date();
  })

  api.post('serverMFinance.asmx/TFinanceServerLookupWebConnector_GetCurrentPeriodDates', x).then(function (data) {
    data = JSON.parse(data.data.d);
    $('#ledger_info').find('.period').html( format_tpl( $('[phantom] .period'), data ) );
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
