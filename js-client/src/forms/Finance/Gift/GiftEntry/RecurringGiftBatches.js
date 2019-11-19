// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2017-2018 by TBits.net
// Copyright 2019 by SolidCharity.com
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
	} else {
		// set periods: only open periods
		$('select[name="APeriod"]').val(0);
		// set batch status: only unposted batches
		$('select[name="ABatchStatus"]').val('Unposted');
		display_list();
	}
}

function display_list(source) {
	var x = extract_data($('#tabfilter'));
	x['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadARecurringGiftBatch', x).then(function (data) {
		data = JSON.parse(data.data.d);
		// on reload, clear content
		$('#browse_container').html('');
		for (item of data.result.ARecurringGiftBatch) {
			format_item(item);
		}
		format_currency(data.ACurrencyCode);
		format_date();
	})
}

function updateGift(BatchNumber, GiftTransactionNumber) {
	// somehow the original window stays gray when we return from this modal.
	$('.modal-backdrop').remove();
	edit_gift_trans(window.localStorage.getItem('current_ledger'), BatchNumber, GiftTransactionNumber);
}

function updateBatch(BatchNumber) {
	var x = {
		'ALedgerNumber': window.localStorage.getItem('current_ledger'),
		'ABatchNumber': BatchNumber};

	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadAGiftBatchSingle', x).then(function (data) {
		data = JSON.parse(data.data.d);
		item = data.result.AGiftBatch[0];
		let batchDiv = $('#Batch' + BatchNumber + " div");
		if (batchDiv.length) {
			let row = format_tpl($("[phantom] .tpl_row").clone(), item);
			batchDiv.first().replaceWith(row.children()[0]);
		} else {
			$('.tpl_row .collapse').collapse('hide');
			format_item(item);
			batchDiv = $('#Batch' + BatchNumber + " div");
			$('html, body').animate({
								scrollTop: (batchDiv.offset().top - 100)
								}, 500);
		}
		format_currency(data.ACurrencyCode);
		format_date();
		open_gift_transactions($('#Batch' + BatchNumber), BatchNumber, true);
	});
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
	$('#browse_container').append(row);
}

function open_gift_transactions(obj, number, reload = false) {
	obj = $(obj);
	if (!reload && obj.find('.collapse').is(':visible') ) {
		return;
	}
	if (obj.find('[batch-status]').text() == "Posted" || obj.find('[batch-status]').text() == "Cancelled") {
		obj.find('.not_show_when_posted').hide();
	}
	if (obj.find('[batch-status]').text() != "Posted") {
		obj.find('.only_show_when_posted').hide();
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
		format_currency(data.ACurrencyCode);
		format_date();
		if (!reload) {
			$('.tpl_row .collapse').collapse('hide');
		}
		obj.find('.collapse').collapse('show')
	})

}

/////

var new_entry_data = {};
function new_batch() {
	if (!allow_modal()) {return}
	let x = {ALedgerNumber :window.localStorage.getItem('current_ledger')};
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_CreateARecurringGiftBatch', x).then(
		function (data) {
			parsed = JSON.parse(data.data.d);
			batch = parsed['result']['ARecurringGiftBatch'][0];
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
	if (!allow_modal()) {return}
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
	if (!allow_modal()) {return}
	let x = {
		a_ledger_number_i: ledger_number,
		a_batch_number_i: batch_number,
		a_gift_transaction_number_i: trans_id,
		a_detail_number_i:  $("#Batch" + batch_number + "Gift" + trans_id + " .tpl_trans_detail").length + 1
	};

	let p = format_tpl( $('[phantom] .tpl_edit_trans_detail').clone(), x);
	$('#modal_space').append(p);
	p.find('[edit-only]').hide();
	p.find('[action]').val('create');
	p.modal('show');
};

/////

function edit_batch(batch_id) {
	if (!allow_modal()) {return}
	var r = {
				ALedgerNumber: window.localStorage.getItem('current_ledger'),
				ABatchNumber: batch_id,
			};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we don't have to load it, if we only search
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadARecurringGiftBatchSingle', r).then(function (data) {
		parsed = JSON.parse(data.data.d)
		let batch = parsed.result.ARecurringGiftBatch[0];

		batch['a_account_name_c'] = batch['a_bank_account_code_c'];
		batch['a_cost_center_name_c'] = batch['a_bank_cost_centre_c'];

		let tpl_m = format_tpl( $('[phantom] .tpl_edit_batch').clone(), batch );

		$('#modal_space').html(tpl_m);
		tpl_m.find('[action]').val('edit');
		tpl_m.modal('show');

	})
}

function edit_gift_trans(ledger_id, batch_id, trans_id) {
	if (!allow_modal()) {return}
	let x = {"ALedgerNumber":ledger_id, "ABatchNumber":batch_id};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we don't have to load it, if we only search

	// TODO: use serverMFinance.asmx/TGiftTransactionWebConnector_LoadGiftTransactionsDetail
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
		if (!parsed.ABatchIsUnposted) {
			tpl_edit_raw.find(".posted_readonly").attr('readonly', true);
			tpl_edit_raw.find('.not_show_when_posted').hide();
		}

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
	if (!allow_modal()) {return}

	let x = {"ALedgerNumber":ledger_id, "ABatchNumber":batch_id};
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadGiftTransactionsForBatch', x).then(function (data) {
		parsed = JSON.parse(data.data.d);
		let searched = null;
		for (trans of parsed.result.AGiftDetail) {
			if (trans.a_gift_transaction_number_i == trans_id && trans.a_detail_number_i == detail_id) {
				searched = trans;
				break;
			}
		}
		if (searched == null) {
			return alert('ERROR');
		}

		let tpl_edit_raw = format_tpl( $('[phantom] .tpl_edit_trans_detail').clone(), searched );
		if (!parsed.ABatchIsUnposted) {
			tpl_edit_raw.find(".posted_readonly").attr('readonly', true);
			tpl_edit_raw.find('.not_show_when_posted').hide();
		}

		$('#modal_space').append(tpl_edit_raw);
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

	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainRecurringGifts', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			$('#modal_space .modal').modal('hide');
			updateBatch(payload['ABatchNumber']);
		}
		else if (parsed.result == false) {
			display_error(parsed.AVerificationResult);
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
			if (mode=="edit") {
				$('#modal_space .modal').modal('hide');
				updateBatch(payload['ABatchNumber']);
			} else if (mode=="create") {
				// switch to edit mode
				updateBatch(payload['ABatchNumber']);
				p = $('#modal_space .modal');
				p.find('[edit-only]').show();
				p.find('[action]').val('edit');
			}
		}
		else if (parsed.result == false) {
			display_error(parsed.AVerificationResult);
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
			updateGift(payload['ABatchNumber'], payload['AGiftTransactionNumber']);
			updateBatch(payload['ABatchNumber']);
		}
		else if (parsed.result == false) {
			display_error(parsed.AVerificationResult);
		}

	});

}

/////

function delete_trans(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let payload = translate_to_server( extract_data(obj) );
	payload["action"] = "delete";
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainGifts', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			$('#modal_space .modal').modal('hide');
			updateBatch(payload['ABatchNumber']);
		}
		else if (parsed.result == false) {
			display_error(parsed.AVerificationResult);
		}
	});
}

function delete_trans_detail(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let payload = translate_to_server( extract_data(obj) );
	payload["action"] = "delete";
	payload["ARecipientKey"] = -1;
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainGiftDetails', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			$('#modal_space .tpl_edit_trans_detail').modal('hide');
			$('#modal_space .tpl_edit_trans').modal('show');
			updateGift(payload['ABatchNumber'], payload['AGiftTransactionNumber']);
			updateBatch(payload['ABatchNumber']);
		}
		else if (parsed.result == false) {
			display_error(parsed.AVerificationResult);
		}
	});
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
		if (data.result == true) {
			display_message( i18next.t('GiftBatches.success_posted'), 'success' );
			display_list('filter');
		} else {
			display_error( data.AVerifications );
		}
	})
}

function cancel_batch(batch_id) {
	var r = {
				ALedgerNumber: window.localStorage.getItem('current_ledger'),
				ABatchNumber: batch_id,
			};

	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_CancelBatch', r).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			$('#modal_space .modal').modal('hide');
			display_list();
		}
		else if (parsed.result == false) {
			display_error(parsed.AVerificationResult);
		}
	});
}

function export_batch(batch_id) {
	var r = {
				ALedgerNumber: window.localStorage.getItem('current_ledger'),
				ABatchNumberStart: batch_id,
				ABatchNumberEnd: batch_id,
				ABatchDateFrom: "null",
				ABatchDateTo: "null",
				ADateFormatString: "dmy",
				ASummary: false,
				AUseBaseCurrency: true,
				ADateForSummary: "null",
				ANumberFormat: "European",
				ATransactionsOnly: true,
				AExtraColumns: false,
				ARecipientNumber: 0,
				AFieldNumber: 0,
				AIncludeUnposted: true
			};

	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_ExportAllGiftBatchData', r).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result > 0) {
			excelfile = parsed.AExportExcel;

			var a = document.createElement("a");
			a.style = "display: none";
			a.href = 'data:application/excel;base64,'+excelfile;
			a.download = i18next.t('GiftBatch') + '_' + batch_id + '.xlsx';

			// For Mozilla we need to add the link, otherwise the click won't work
			// see https://support.mozilla.org/de/questions/968992
			document.body.appendChild(a);

			a.click();
		}
		else if (parsed.result < 0) {
			display_error(parsed.AVerificationMessages);
		}
	});
}

function adjust_batch(batch_id) {
	var r = {
				ALedgerNumber: window.localStorage.getItem('current_ledger'),
				ABatchNumber: batch_id,
				AGiftDetailNumber: -1,
				ABatchSelected: false,
				ANewBatchNumber: -1,
				ANewGLDateEffective: "null",
				AFunction: "AdjustGift",
				ANoReceipt: false,
				ANewPct: 0.0
			};

	api.post('serverMFinance.asmx/TAdjustmentWebConnector_GiftRevertAdjust', r).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result > 0) {
			display_message(i18next.t('GiftBatches.success_adjust'), "success");
			display_list();
		}
		else if (parsed.result < 0) {
			display_error(parsed.AVerificationMessages);
		}
	});
}
