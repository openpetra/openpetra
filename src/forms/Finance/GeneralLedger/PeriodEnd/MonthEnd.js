function month_end() {

  let x = {ALedgerNumber: window.localStorage.getItem('current_ledger')};
  api.post('serverMFinance.asmx/TPeriodIntervalConnector_PeriodMonthEnd', x).then(function (data) {
    let parsed = JSON.parse(data.data.d);
		let s = false;
		if (parsed.result == true) {
			display_message( i18next.t('forms.saved'), 'success' )
		}
		else {
			for (error of parsed.AVerificationResult) {
				if (error.code == "") {
					continue;
				}
				s = true;
				display_message( i18next.t(error.code), "fail");
			}
			if (!s) {
				display_message( i18next.t('errors.general'), 'fail');
			}
		}
  });

}
