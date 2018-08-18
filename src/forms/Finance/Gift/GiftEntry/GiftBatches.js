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

$('document').ready(function () {
	get_available_years(
		function() {
			load_preset();
		});
});

function load_preset() {
	var x = window.localStorage.getItem('GiftBatches');
	if (x != null) {
		x = JSON.parse(x);
		// first set the period so that it can be used when the year has been selected
		let y = {"APeriod":x["APeriod"]};
		format_tpl($('#tabfilter'), y);
		format_tpl($('#tabfilter'), x);
		display_list();
	}
}

function display_list(source) {
	var x = extract_data($('#tabfilter'));
	x['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadAGiftBatchForYearPeriod', x).then(function (data) {
		data = JSON.parse(data.data.d);
		// on reload, clear content
		$('#browse_container').html('');
		for (item of data.result.AGiftBatch) {
			format_item(item);
		}
		format_date();
	})
}

function format_date() {
	$('.format_date').each(
		function(x, obj) {
			obj = $(obj);
			let t = /\((.+)\)/g.exec(obj.text());
			if (t == null || t.length <=1) {return}

			time = new Date(parseInt(t[1])).toLocaleDateString();
			obj.text(time);

		}
	)
};

function format_item(item) {
	let row = format_tpl($("[phantom] .tpl_row").clone(), item);
	// let view = format_tpl($("[phantom] .tpl_view").clone(), item);
	// row.find('.collapse_col').append(view);
	$('#browse_container').append(row);
}

function open_gift_transactions(obj, number) {
	obj = $(obj);
	if (obj.find('.collapse').is(':visible') ) {
		return;
	}
	let x = {"ALedgerNumber":window.localStorage.getItem('current_ledger'), "ABatchNumber":number};
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadGiftTransactionsForBatch', x).then(function (data) {
		data = JSON.parse(data.data.d);
		// on open, clear content
		let place_to_put_content = obj.find('.content_col').html('');
		for (item of data.result.AGift) {
			let transaction_row = $('[phantom] .tpl_gift').clone();
			transaction_row = format_tpl(transaction_row, item);
			place_to_put_content.append(transaction_row);
		}
		format_date();
		$('.tpl_row .collapse').collapse('hide');
		obj.find('.collapse').collapse('show')
	})

}

/////

var new_entry_data = {};
function new_batch() {
	let x = {ALedgerNumber :window.localStorage.getItem('current_ledger')};
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_CreateAGiftBatch', x).then(
		function (data) {
			parsed = JSON.parse(data.data.d);
			batch = parsed['result']['AGiftBatch'][0];
			let p = format_tpl( $('[phantom] .tpl_edit_batch').clone(), batch );
			$('#modal_space').html(p);
			p.find('input[name=a_bank_account_code_c]').attr('readonly', false);
			p.find('input[name=a_bank_cost_centre_c]').attr('readonly', false);
			p.find('[edit-only]').hide();
			p.find('[action]').val('create');
			p.modal('show');
		}
	)
};

function new_trans(ledger_number, batch_number) {
	let x = {
		a_ledger_number_i: ledger_number,
		a_batch_number_i: batch_number,
		a_gift_transaction_number_i: $("#Batch" + batch_number + " .tpl_gift").length + 1
	};
	var today = new Date();
	today.setUTCHours(0, 0, 0, 0);
	var strToday = today.toISOString();
	x['a_date_entered_d'] = strToday.replace('T00:00:00.000Z', '');

	let p = format_tpl( $('[phantom] .tpl_edit_trans').clone(), x);
	$('#modal_space').html(p);
	p.find('[edit-only]').hide();
	p.find('[action]').val('create');
	p.modal('show');
};

function new_trans_detail(ledger_number, batch_number, trans_id) {
	let x = {
		a_ledger_number_i: ledger_number,
		a_batch_number_i: batch_number,
		a_gift_transaction_number_i: trans_id,
		// a_detail_number_i:  $("#Batch" + batch_number + "Gift" + trans_id + " .tpl_trans_detail").length + 1
	};

	let p = format_tpl( $('[phantom] .tpl_edit_trans_detail').clone(), x);
	$('#modal_space').append(p);
	$('.modal').modal('hide');
	p.find('[edit-only]').hide();
	p.find('[action]').val('create');
	p.modal('show');
};

/////

function edit_batch(batch_id) {
	var r = {
				ALedgerNumber: window.localStorage.getItem('current_ledger'),
				ABatchNumber: batch_id,
			};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we don't have to load it, if we only search
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadAGiftBatchSingle', r).then(function (data) {
		parsed = JSON.parse(data.data.d)
		let batch = parsed.result.AGiftBatch[0];

		batch['a_account_name_c'] = batch['a_bank_account_code_c'];
		batch['a_cost_center_name_c'] = batch['a_bank_cost_centre_c'];

		let tpl_m = format_tpl( $('[phantom] .tpl_edit_batch').clone(), batch );

		$('#modal_space').html(tpl_m);
		tpl_m.find('[action]').val('edit');
		tpl_m.modal('show');

	})
}

function edit_gift_trans(ledger_id, batch_id, trans_id) {
	let x = {"ALedgerNumber":ledger_id, "ABatchNumber":batch_id};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we don't have to load it, if we only search

	// serverMFinance.asmx/TGiftTransactionWebConnector_LoadGiftTransactionsDetail
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadGiftTransactionsForBatch', x).then(function (data) {
		parsed = JSON.parse(data.data.d);
		let searched = null;
		for (trans of parsed.result.AGift) {
			if (trans.a_gift_transaction_number_i == trans_id) {
				searched = trans;
				break;
			}
		}
		if (searched == null) {
			return alert('ERROR');
		}

		searched['p_donor_name_c'] = searched['p_donor_key_n'] + ' ' + searched['DonorName'];

		let tpl_edit_raw = format_tpl( $('[phantom] .tpl_edit_trans').clone(), searched );

		for (detail of parsed.result.AGiftDetail) {
			if (detail.a_gift_transaction_number_i == trans_id) {

				let tpl_trans_detail = format_tpl( $('[phantom] .tpl_trans_detail').clone(), detail );
				tpl_edit_raw.find('.detail_col').append(tpl_trans_detail);

			}
		}


		$('#modal_space').html(tpl_edit_raw);
		tpl_edit_raw.find('[action]').val('edit');
		tpl_edit_raw.modal('show');

	})
}

function edit_gift_trans_detail(ledger_id, batch_id, trans_id, detail_id) {

	let x = {"ALedgerNumber":ledger_id, "ABatchNumber":batch_id};
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadGiftTransactionsForBatch', x).then(function (data) {
		parsed = JSON.parse(data.data.d);
		let searched = null;
		for (trans of parsed.result.AGiftDetail) {
			if (trans.a_gift_transaction_number_i == trans_id) {
				searched = trans;
				break;
			}
		}
		if (searched == null) {
			return alert('ERROR');
		}

		let tpl_edit_raw = format_tpl( $('[phantom] .tpl_edit_trans_detail').clone(), searched );

		$('#modal_space').append(tpl_edit_raw);
		$('.modal').modal('hide');
		tpl_edit_raw.find('[action]').val('edit');
		tpl_edit_raw.modal('show');

	})
}


function save_edit_batch(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let mode = obj.find('[action]').val();

	// extract information from a jquery object
	let payload = translate_to_server( extract_data(obj) );
 	payload['action'] = mode;

	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainBatches', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			$('#modal_space .modal').modal('hide');
			display_list();
		}
		else if (parsed.result == "false") {
			for (msg of parsed.AVerificationResult) {
				display_message(i18next.t(msg.code), "fail");
			}
		}
	});
}

function save_edit_trans(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let mode = obj.find('[action]').val();

	// extract information from a jquery object
	let payload = translate_to_server( extract_data(obj) );
 	payload['action'] = mode;

	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainGifts', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			$('#modal_space .modal').modal('hide');
			display_list();
		}
		else if (parsed.result == false) {
			for (msg of parsed.AVerificationResult) {
				display_message(i18next.t(msg.code), "fail");
			}
		}
	});
}

function save_edit_trans_detail(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let mode = obj.find('[action]').val();

	// extract information from a jquery object
	let payload = translate_to_server( extract_data(obj) );
 	payload['action'] = mode;

	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainGiftDetails', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			$('#modal_space .modal').modal('hide');
			display_list();
		}
		else if (parsed.result == false) {
			for (msg of parsed.AVerificationResult) {
				display_message(i18next.t(msg.code), "fail");
			}
		}

	});

}

/////

function delete_trans(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let payload = translate_to_server( extract_data(obj) );
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainGifts', payload);
}

function delete_trans_detail(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let payload = translate_to_server( extract_data(obj) );
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainGiftsDetails', payload);
}


/////

function get_available_years(fn_to_call) {
	let x = {
		ALedgerNumber: window.localStorage.getItem('current_ledger'),
	};
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_GetAvailableGiftYears', x).then(function (data) {
		data = JSON.parse(data.data.d);
		r = data.result;
		let currentYearNumber = -1;

		for (year of r) {
			let y = $('<option value="'+year.YearNumber+'">'+year.YearDate+'</option>');

			if (year.YearNumber > currentYearNumber) {
				currentYearNumber = year.YearNumber;
			}
			$('#tabfilter [name=AYear]').append(y);
		}

		get_available_periods(currentYearNumber, fn_to_call);
	})
}

function get_available_periods(year, fn_to_call) {
	selectedPeriod = $('#tabfilter [name=APeriod]').val();
	if (selectedPeriod == null) {
		selectedPeriod = -1;
	}
	let x = {
		ALedgerNumber: window.localStorage.getItem('current_ledger'),
		AFinancialYear: year,
	};
	api.post('serverMFinance.asmx/TAccountingPeriodsWebConnector_GetAvailablePeriods', x).then(function (data) {
		data = JSON.parse(data.data.d);
		r = data.result;
		$('#tabfilter [name=APeriod]').html('');
		for (period of r) {
			let translated = period.PeriodName;
			transsplit = translated.split(' ');
			if (transsplit[1] != undefined) {
				// separate the year
				translated = i18next.t('SelectPeriods.' + transsplit[0]) + ' ' + transsplit[1];
			} else {
				translated = i18next.t('SelectPeriods.' + translated);
			}
			selected = (period.PeriodNumber == selectedPeriod)?'selected':'';
			let y = $('<option value="'+period.PeriodNumber+'" ' + selected + '>'+translated+'</option>');
			$('#tabfilter [name=APeriod]').append(y);
		}

		if (fn_to_call != undefined) {
			fn_to_call();
		}
	})
	
}

function post_batch(batch_id) {
	let x = {
		ALedgerNumber: window.localStorage.getItem('current_ledger'),
		AGiftBatchNumber: batch_id
	};
	api.post( 'serverMFinance.asmx/TGiftTransactionWebConnector_PostGiftBatch', x).then(function (data) {
		data = JSON.parse(data.data.d);
		console.log(data);
	})
}
