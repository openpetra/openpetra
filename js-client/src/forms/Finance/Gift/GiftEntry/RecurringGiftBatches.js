// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2017-2018 by TBits.net
// Copyright 2019-2022 by SolidCharity.com
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
		display_list();
});

function display_list(source) {
	var x = extract_data($('#tabfilter'));
	x['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadARecurringGiftBatch', x).then(function (data) {
		data = JSON.parse(data.data.d);
		// on reload, clear content
		$('#browse_container').html('');
		for (item of data.result.ARecurringGiftBatch) {
			var today = new Date();
			var submit_date = today;
			var test_date = new Date(today.getFullYear(), today.getMonth(), 15);
			if (test_date > today && submit_date == today) {
				submit_date = test_date;
			}
			test_date = new Date(today.getFullYear(), today.getMonth() + 1, 1);
			if (test_date > today && submit_date == today) {
				submit_date = test_date;
			}
			item['submit_date'] = submit_date;
			format_item(item);
		}
		format_currency(item.a_currency_code_c);
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

	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadARecurringGiftBatchSingle', x).then(function (data) {
		data = JSON.parse(data.data.d);
		var item = data.result.ARecurringGiftBatch[0];
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
		format_currency(item.a_currency_code_c);
		format_date();
		open_gift_transactions($('#Batch' + BatchNumber), BatchNumber, true);
	});
}

function format_item(item) {
	let row = format_tpl($("[phantom] .tpl_row").clone(), item);
	$('#browse_container').append(row);
}

function open_gift_transactions(obj, number = -1, reload = false) {
	obj = $(obj);
	if (number == -1) {
		while(!obj[0].hasAttribute('id') || !obj[0].id.includes("Batch")) {
			obj = obj.parent();
		}
		number = obj[0].id.replace("Batch", "");
	}
	if (!reload && obj.find('.collapse').is(':visible') ) {
		$('.tpl_row .collapse').collapse('hide');
		return;
	}

	let x = {"ALedgerNumber":window.localStorage.getItem('current_ledger'), "ABatchNumber":number};
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadRecurringGiftTransactionsForBatch', x).then(function (data) {
		parsed = JSON.parse(data.data.d);

		if (parsed.result == false) {
			display_error(parsed.AVerificationResult);
			return;
		}

		// on open, clear content
		let place_to_put_content = obj.find('.content_col').html('');
		for (var item of parsed.result.ARecurringGift) {
			let transaction_row = $('[phantom] .tpl_gift').clone();
			transaction_row = format_tpl(transaction_row, item);
			place_to_put_content.append(transaction_row);
		}
		format_currency(data.ACurrencyCode ? data.ACurrencyCode : "EUR");
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
		a_gift_transaction_number_i: $("#Batch" + batch_number + " .tpl_gift").length + 1,

	};
	var today = new Date();
	today.setUTCHours(0, 0, 0, 0);
	var strToday = today.toISOString();
	x['a_date_entered_d'] = strToday.replace('T00:00:00.000Z', '');

	let p = format_tpl( $('[phantom] .tpl_edit_trans').clone(), x);
	p = loadbankaccounts(null, -1, -1, p);

	$('#modal_space').html(p);
	p.find('[edit-only]').hide();
	p.find('[action]').val('create');
	p.modal('show');

	install_ContactEdit_and_RefreshAccounts(p, '-1');
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

	p.find('[name=AStartDonations]').val(new Date().toDateInputValue());
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

	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadRecurringGiftTransactionsForBatch', x).then(function (data) {
		parsed = JSON.parse(data.data.d, parseDates);

		let searched = null;
		for (var trans of parsed.result.ARecurringGift) {
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

		for (var detail of parsed.result.ARecurringGiftDetail) {
			if (detail.a_gift_transaction_number_i == trans_id) {

				let tpl_trans_detail = format_tpl( $('[phantom] .tpl_trans_detail').clone(), detail );
				tpl_edit_raw.find('.detail_col').append(tpl_trans_detail);

			}
		}

		install_ContactEdit_and_RefreshAccounts(tpl_edit_raw, searched['p_donor_key_n']);

		tpl_edit_raw = loadbankaccounts(parsed.result.PPartnerBankingDetails, searched['p_donor_key_n'], searched['p_banking_details_key_i'], tpl_edit_raw)
		$('#modal_space').html(tpl_edit_raw);
		tpl_edit_raw.find('[action]').val('edit');
		tpl_edit_raw.modal('show');

	})
}

function install_ContactEdit_and_RefreshAccounts(obj, donorkey) {
	let btnEditContact = obj.find("#btnEditContact");
	btnEditContact.attr('partnerkey', donorkey);
	btnEditContact.click(function() {
		let partnerkey = $(this).attr('partnerkey');
		window.open("/Partner/Partners/MaintainPartners?partnerkey=" + partnerkey, "_blank");
	});

	let btnRefreshBankAccounts = obj.find("#btnRefreshBankAccounts");
	btnRefreshBankAccounts.attr('partnerkey', donorkey);
	btnRefreshBankAccounts.click(function() {
		let partnerkey = $(this).attr('partnerkey');
		onselect_donor($(this), partnerkey, false);
	});
}

function onselect_donor(obj, donor_key, reset_selection = true) {
	// reload the select for bank accounts
	let x = {"APartnerKey":donor_key};
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadBankingDetailsOfPartner', x).then(function (data) {
		parsed = JSON.parse(data.data.d);
		selected = -1;
		if (!reset_selection) {
			selected = obj.closest('.modal').find('#BankingDetailsKey').val();
		}
		loadbankaccounts(parsed.result.PPartnerBankingDetails, donor_key, selected, obj.closest('.modal'));

		let btnEditContact = obj.closest('.modal').find("#btnEditContact");
		btnEditContact.attr('partnerkey', donor_key);
		let btnRefreshBankAccounts = obj.closest('.modal').find("#btnRefreshBankAccounts");
		btnRefreshBankAccounts.attr('partnerkey', donor_key);

		mandateReferenceID = obj.closest('.modal').find('input[name=a_sepa_mandate_reference_c]');
		if (mandateReferenceID.val() == '') {
			var today = new Date();
			var datebackwards = today.toISOString().slice(0,10).replace(/-/g,"");
			mandateReferenceID.val(donor_key + datebackwards);
		}

	})
}

function loadbankaccounts(accounts_of_donor, donor_key, selected_account, obj) {
	obj.find('#BankingDetailsKey').empty();
	var selected = (selected_account == "")?" selected":"";
	let y = $('<option value="-1"' + selected + '>'+ i18next.t('forms.none_selected') +'</option>');
	obj.find('#BankingDetailsKey').append(y);

	if (accounts_of_donor != null) {
		for (var account of accounts_of_donor) {
			selected = (selected_account == account.p_banking_details_key_i)?" selected":"";
			if (account.p_partner_key_n == donor_key) {
				let y = $('<option value="'+account.p_banking_details_key_i+'"' + selected + '>'+ account.Iban + '</option>');
				obj.find('#BankingDetailsKey').append(y);
			}
		}
	}
	return obj;
}

function edit_gift_trans_detail(ledger_id, batch_id, trans_id, detail_id) {
	if (!allow_modal()) {return}

	let x = {"ALedgerNumber":ledger_id, "ABatchNumber":batch_id};
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadRecurringGiftTransactionsForBatch', x).then(function (data) {
		parsed = JSON.parse(data.data.d, parseDates);
		let searched = null;
		for (trans of parsed.result.ARecurringGiftDetail) {
			if (trans.a_gift_transaction_number_i == trans_id && trans.a_detail_number_i == detail_id) {
				searched = trans;
				break;
			}
		}
		if (searched == null) {
			return alert('ERROR');
		}

		let tpl_edit_raw = format_tpl( $('[phantom] .tpl_edit_trans_detail').clone(), searched );

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

	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainRecurringBatches', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			CloseModal(obj);
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
	payload['ASepaMandateGiven'] = payload['ASepaMandateGiven'] ? payload['ASepaMandateGiven'] : "null"; // if no date is given give "null" as a string
 	payload['action'] = mode;

	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainRecurringGifts', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			if (mode=="edit") {
				CloseModal(obj);
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
	payload['AEndDonations'] = payload['AEndDonations'] ? payload['AEndDonations'] : "null"; // if no date is given give "null" as a string
 	payload['action'] = mode;

	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainRecurringGiftDetails', payload).then(function (result) {
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
	payload['ASepaMandateGiven'] = "null";
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainRecurringGifts', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			CloseModal(obj);
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
	payload['AEndDonations'] = "null";
	payload["ARecipientKey"] = -1;
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainRecurringGiftDetails', payload).then(function (result) {
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

function submit_batch(obj, batch_id) {
	var strSubmitDate = $(obj).parent().find('#submit_date').val();

	let x = {
		ALedgerNumber: window.localStorage.getItem('current_ledger'),
		ARecurringBatchNumber: batch_id,
		AEffectiveDate: strSubmitDate,
		AReference: '',
		AExchangeRateToBase: 1.0,
		AExchangeRateIntlToBase: 1.0
	};
	api.post( 'serverMFinance.asmx/TGiftTransactionWebConnector_SubmitRecurringGiftBatch', x).then(function (data) {
		data = JSON.parse(data.data.d);
		if (data.result == true) {
			display_message( i18next.t('RecurringGiftBatches.success_submit'), 'success' );
			display_list('filter');
			window.open("/Finance/Gift/GiftEntry/GiftBatches?batch=" + data["ANewGiftBatchNo"], "_blank");
		} else {
			display_error( data.AVerifications );
		}
	})
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

	// TODO Recurring
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_ExportAllRecurringGiftBatchData', r).then(function (result) {
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

function download_sepa(obj, batch_id) {
	var strSubmitDate = $(obj).parent().find('#submit_date').val();

	var r = {
				ALedgerNumber: window.localStorage.getItem('current_ledger'),
				ARecurringBatchNumber: batch_id,
				ACollectionDate: strSubmitDate,
			};

	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_ExportRecurringGiftBatch', r).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result) {
			sepafile = parsed.ASEPAFileContent;

			var a = document.createElement("a");
			a.style = "display: none";
			a.href = 'data:application/xml;base64,'+sepafile;
			a.download = i18next.t('RecurringGiftBatch') + '_' + batch_id + '.xml';

			// For Mozilla we need to add the link, otherwise the click won't work
			// see https://support.mozilla.org/de/questions/968992
			document.body.appendChild(a);

			a.click();
		}
		else {
			display_error(parsed.AMessages);
		}
	});
}
