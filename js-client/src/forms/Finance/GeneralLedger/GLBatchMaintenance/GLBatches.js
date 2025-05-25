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
import tpl from '../../../../lib/tpl.js'
import api from '../../../../lib/ajax.js'
import utils from '../../../../lib/utils.js'
import modal from '../../../../lib/modal.js'
import finance from '../../../../lib/finance.js'
import AutocompleteAccCc from '../../../../lib/autocomplete_posting_acc_cc.js'

class GLBatches {
	constructor() {
		this.new_entry_data = {};
	}

	Ready() {
		let self = this;
		self.countComboboxes = 0;

		finance.get_available_years(
			'#tabfilter [name=ABatchYear]',
			'#tabfilter [name=ABatchPeriod]',
			'',
			function() { self.AfterLoadingComboboxes()} );
		$('#btnNewBatch').on('click', function() { self.new_batch() })
	};

	AfterLoadingComboboxes() {
		let self = this;
		self.countComboboxes++;
		if (self.countComboboxes == 1) {
			self.updatePeriods($('select[name="ABatchYear"]').val())
		}
		else if (self.countComboboxes == 2) {
			$('#selectYear').on('change', function () {self.updatePeriods($(this).val())});
			$('#btnSavePreset').on('click', function () {utils.save_preset('GLBatches')});
			$('#btnSearch').on('click', function () {self.display_list('filter');$('#tabfilter').collapse('toggle')});
			self.load_preset();
		}
	}

	updatePeriods(year) {
		let self = this;
		finance.get_available_periods(year, '#tabfilter [name=ABatchPeriod]', function() { self.AfterLoadingComboboxes()}, true);
	}

	load_preset() {
		let self = this;
		var x = window.localStorage.getItem('GLBatches');
		if (x != null) {
			x = JSON.parse(x);
			// first set the period so that it can be used when the year has been selected
			let y = {"ABatchPeriod":x["ABatchPeriod"]};
			tpl.format_tpl($('#tabfilter'), y);
			tpl.format_tpl($('#tabfilter'), x);
			self.display_list();
		} else {
			// set periods: only open periods
			$('select[name="ABatchPeriod"]').val(0);
			// set batch status: only unposted batches
			$('select[name="ABatchStatus"]').val('Unposted');
			self.display_list();
		}
	}

	display_list(source) {
		let self = this;
		if (source == null) {
			source = "filter";
		}
		// x is search
		if (source == "filter") {
			var x = tpl.extract_data( $('#tabfilter') );
		} else if (source == "preset") {
			var x = window.localStorage.getItem('GLBatches');
			x = JSON.parse(x);
			tpl.format_tpl( $('#tabfilter'), x );
		}
		x['ALedgerNumber'] = window.localStorage.getItem('current_ledger');

		api.post('serverMFinance.asmx/TGLTransactionWebConnector_LoadABatch', x).then(function (data) {
			data = JSON.parse(data.data.d);
			// on reload, clear content
			$('#browse_container').html('');
			for (var item of data.result.ABatch) {
				let row = self.format_item(item);
				$('#browse_container').append(row);
			}
			tpl.format_currency(data.ACurrencyCode);
			tpl.format_date();
		})
	}

	updateBatch(BatchNumber) {
		let self = this;
		var x = {
			'ALedgerNumber': window.localStorage.getItem('current_ledger'),
			'ABatchNumber': BatchNumber};

		api.post('serverMFinance.asmx/TGLTransactionWebConnector_LoadABatch2', x).then(function (data) {
			data = JSON.parse(data.data.d);
			let item = data.result.ABatch[0];
			let batchDiv = $('#Batch' + BatchNumber + " div");
			if (batchDiv.length) {
				let row = self.format_item(item);
				batchDiv.first().replaceWith(row.children()[0]);
			} else {
				$('.tpl_row .collapse').collapse('hide');
				let row = self.format_item(item);
				$('#browse_container').append(row);
				batchDiv = $('#Batch' + BatchNumber + " div");
				$('html, body').animate({
							scrollTop: (batchDiv.offset().top - 100)
						}, 500);
			}
			tpl.format_currency(data.ACurrencyCode);
			tpl.format_date();
			self.open_transactions($('#Batch' + BatchNumber), BatchNumber, true);
		});
	}

	format_date() {
		let self = this;
		$('.format_date').each(
			function(x, obj) {
				obj = $(obj);
				let t = /\((.+)\)/g.exec(obj.text());
				if (t == null || t.length <=1) {return}

				let time = new Date(parseInt(t[1])).toLocaleDateString();
				obj.text(time);
			}
		)
	}

	format_item(item) {
		let self = this;
		let row = tpl.format_tpl($("[phantom] .tpl_row").clone(), item);
		// let view = tpl.format_tpl($("[phantom] .tpl_view").clone(), item);
		// row.find('.collapse_col').append(view);
		row.find('#btnOpenTransactions').on('click', function() { self.open_transactions(this) });
		row.find('#btnEditBatch').on('click', function() { self.edit_batch(item['a_batch_number_i']) });
		return row;
	}

	open_transactions(obj, number = -1, reload = false) {
		let self = this;
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
		if (obj.find('[batch-status]').text() == "Posted" ) {
			obj.find('.only_show_when_not_posted').hide();
		}
		else {
			obj.find('.only_show_when_posted').hide();
		}
		let x = {"ALedgerNumber":window.localStorage.getItem('current_ledger'), "ABatchNumber":number};
		api.post('serverMFinance.asmx/TGLTransactionWebConnector_LoadABatchAJournalATransaction', x).then(function (data) {
			data = JSON.parse(data.data.d);
			// on open, clear content

			let place_to_put_content = obj.find('.content_col').html('');
			for (var item of data.result.ATransaction) {
				if (item['a_debit_credit_indicator_l']) {
					item['debitcredit'] = i18next.t('GLBatches.DEBIT');
					item['creditamountbase'] = '';
					item['debitamountbase'] = item['a_amount_in_base_currency_n'];
				} else {
					item['debitcredit'] = i18next.t('GLBatches.CREDIT');
					item['debitamountbase'] = '';
					item['creditamountbase'] = item['a_amount_in_base_currency_n'];
				}
				for (var account of data.result.AAccount) {
					if (account['a_account_code_c'] == item['a_account_code_c']) {
						item['account_name'] = account['a_account_code_long_desc_c'];
					}
				}
				for (var costcentre of data.result.ACostCentre) {
					if (costcentre['a_cost_centre_code_c'] == item['a_cost_centre_code_c']) {
						item['costcentre_name'] = costcentre['a_cost_centre_name_c'];
					}
				}
				// console.log(item);
				let transaction_row = $('[phantom] .tpl_transaction').clone();
				transaction_row = tpl.format_tpl(transaction_row, item);
				place_to_put_content.append(transaction_row);
			}
			tpl.format_currency(data.ACurrencyCode);
			tpl.format_date();
			if (!reload) {
				$('.tpl_row .collapse').collapse('hide');
			}
			obj.find('.collapse').collapse('show')
		})

	}

/////

	new_batch() {
		let self = this;
		if (!modal.allow_modal()) {return}
		let x = {ALedgerNumber :window.localStorage.getItem('current_ledger')};
		api.post('serverMFinance.asmx/TGLTransactionWebConnector_CreateABatch', x).then(
			function (data) {
				let parsed = JSON.parse(data.data.d);
				self.new_entry_data = parsed.result;
				let p = tpl.format_tpl( $('[phantom] .tpl_edit_batch').clone(), parsed['result']['ABatch'][0] );
				$('#modal_space').html(p);
				p.find('input[name=a_batch_credit_total_n]').attr('readonly', false);
				p.find('input[name=a_batch_debit_total_n]').attr('readonly', false);
				p.find('[action]').val('create');
				p.modal('show');
				p.find("#btnClose").on('click', function() { modal.CloseModal(this) });
				p.find("#btnSave").on('click', function() { self.save_edit_batch(this) });
			}
		)
	};

	new_trans(batch_number, batch_date) {
		let self = this;
		if (!modal.allow_modal()) {return}
		let ledger_number = window.localStorage.getItem('current_ledger');
		self.new_entry_data = [];
		self.new_entry_data['a_ledger_number_i'] = ledger_number;
		self.new_entry_data['a_batch_number_i'] = batch_number;
		self.new_entry_data['a_transaction_number_i'] = $("#Batch" + batch_number + " .tpl_transaction").length + 1;
		self.new_entry_data['a_account_code_c'] = "0100";
		self.new_entry_data['a_cost_centre_code_c'] = ledger_number * 100;
		self.new_entry_data['a_transaction_date_d'] = batch_date;
		let p = tpl.format_tpl( $('[phantom] .tpl_edit_trans').clone(), self.new_entry_data );
		$('#modal_space').html(p);
		p.find('[action]').val('create');
		p.modal('show');
	};

	/////

	edit_batch(batch_id) {
		let self = this;
		if (!modal.allow_modal()) {return}
		var x = window.localStorage.getItem('GLBatches');
		if (x == null) {
			x = tpl.extract_data( $('#tabfilter') );
		} else {
			x = JSON.parse(x);
		}
		x['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
		// on open of a edit modal, we get new data,
		// so everything is up to date and we don't have to load it, if we only search
		api.post('serverMFinance.asmx/TGLTransactionWebConnector_LoadABatch', x).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			let searched = null;
			self.new_entry_data = parsed.result;
			for (var batch of parsed.result.ABatch) {
				if (batch.a_batch_number_i == batch_id) {
					searched = batch;
					break;
				}
			}
			if (searched == null) {
				return alert('ERROR');
			}
			let tpl_m = tpl.format_tpl( $('[phantom] .tpl_edit_batch').clone(), searched );
			if (searched['a_batch_status_c'] == "Posted") {
				tpl_m.find('.posted_readonly').attr('readonly', true)
				tpl_m.find('.not_show_when_posted').hide()
			}
			$('#modal_space').html(tpl_m);
			tpl_m.find('[action]').val('edit');
			tpl_m.modal('show');
			tpl_m.find("#btnClose").on('click', function() { modal.CloseModal(this) });
			tpl_m.find("#btnSave").on('click', function() { self.save_edit_batch(this) });
		})
	}

	edit_trans(batch_id, trans_id) {
		let self = this;
		if (!modal.allow_modal()) {return}
		let x = {"ALedgerNumber":window.localStorage.getItem('current_ledger'), "ABatchNumber":batch_id};
		// on open of a edit modal, we get new data,
		// so everything is up to date and we don't have to load it, if we only search
		api.post('serverMFinance.asmx/TGLTransactionWebConnector_LoadABatchAJournalATransaction', x).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			let searched = null;
			self.new_entry_data = parsed.result;
			for (var trans of parsed.result.ATransaction) {
				if (trans.a_transaction_number_i == trans_id) {
					searched = trans;
					break;
				}
			}
			if (searched == null) {
				return alert('ERROR');
			}
			if (searched['a_debit_credit_indicator_l']) {
				searched['a_debit_amount_base_n'] = searched['a_amount_in_base_currency_n'];
				searched['a_credit_amount_base_n'] = 0.0;
			} else {
				searched['a_debit_amount_base_n'] = 0.0;
				searched['a_credit_amount_base_n'] = searched['a_amount_in_base_currency_n'];
			}
			searched['a_account_name_c'] = searched['a_account_code_c'];
			searched['a_cost_center_name_c'] = searched['a_cost_centre_code_c'];
			let tpl_m = tpl.format_tpl( $('[phantom] .tpl_edit_trans').clone(), searched );
			if (searched['a_batch_status_c'] == "Posted") {
				tpl_m.find('.posted_readonly').attr('readonly', true)
			}
			$('#modal_space').html(tpl_m);
			tpl_m.find('[action]').val('edit');
			tpl_m.modal('show');

		})
	}

	/////

	save_edit_batch(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		let mode = obj.find('[action]').val();

		// extract information from a jquery object
		let payload = utils.translate_to_server( tpl.extract_data(obj) );
		if (payload["ADateEffective"] == '') {
			utils.display_message(i18next.t('GLBatches.missing_batch_date'), "fail");
			exit;
		}
		payload['action'] = mode;
		if (payload['ABatchDebitTotal'] == '') {
			payload['ABatchDebitTotal'] = 0;
		}
		if (payload['ABatchCreditTotal'] == '') {
			payload['ABatchCreditTotal'] = 0;
		}

		api.post('serverMFinance.asmx/TGLTransactionWebConnector_MaintainBatches', payload).then(function (result) {
			let parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				utils.display_message(i18next.t('forms.saved'), "success");
				modal.CloseModal(obj);
				self.updateBatch(payload['ABatchNumber']);
			}
			if (parsed.result == false) {
				utils.display_error(parsed.AVerificationResult);
			}
		});
	}

	save_edit_trans(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		let mode = obj.find('[action]').val();

		// extract information from a jquery object
		let payload = utils.translate_to_server( tpl.extract_data(obj) );
		if (payload["ATransactionDate"] == '') {
			utils.display_message(i18next.t('GLBatches.missing_transaction_date'), "fail");
			exit;
		}
		payload['action'] = mode;
		payload['AJournalNumber'] = 1;
		payload['AAmountInIntlCurrency'] = 0.0;
		let amount = payload['ACreditAmountBase'] - payload['ADebitAmountBase'];
		if (amount < 0) {
			payload['AAmountInBaseCurrency'] = -1 * amount;
			payload['ADebitCreditIndicator'] = true;
		} else {
			payload['AAmountInBaseCurrency'] = amount;
			payload['ADebitCreditIndicator'] = false;
		}
		api.post('serverMFinance.asmx/TGLTransactionWebConnector_MaintainTransactions', payload).then(function (result) {
			let parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				utils.display_message(i18next.t('forms.saved'), "success");
				modal.CloseModal(obj);
				self.updateBatch(payload['ABatchNumber']);
			} else {
				for (var msg of parsed.AVerificationResult) {
					utils.display_message(i18next.t(msg.code ? msg.code : msg.message), "fail");
				}
			}

		});

	}

	delete_edit_trans(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');

		// extract information from a jquery object
		let payload = utils.translate_to_server( tpl.extract_data(obj) );
		payload['action'] = "delete";
		payload['AJournalNumber'] = 1;
		payload['AAmountInIntlCurrency'] = 0.0;
		payload['AAmountInBaseCurrency'] = 0.0;
		payload['ADebitCreditIndicator'] = true;
		api.post('serverMFinance.asmx/TGLTransactionWebConnector_MaintainTransactions', payload).then(function (result) {
			let parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				utils.display_message(i18next.t('forms.deleted'), "success");
				modal.CloseModal(obj);
				self.updateBatch(payload['ABatchNumber']);
			}
			if (parsed.result == "false") {
				for (var msg of parsed.AVerificationResult) {
					utils.display_message(i18next.t(msg.code), "fail");
				}
			}

		});
	}

	importTransactions(batch_id, csv_file) {
		let self = this;
		if (csv_file == undefined) {
			$('#Batch'+batch_id).find('.import_space').css('display', 'block');
			return;
		}

		let x = {
			AImportString: csv_file,
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			ABatchNumber: batch_id,
			AJournalNumber: 1,
			ANumberFormat: "European",
			ADateFormatString: "dmy",
			ADelimiter: "\t",
		};

		api.post('serverMFinance.asmx/TGLTransactionWebConnector_ImportGLTransactions', x).then(function (result) {
			let parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				utils.display_message(i18next.t('forms.saved'), "success");
				self.updateBatch(batch_id);
			}
			if (parsed.result == false) {
				utils.display_error(parsed.AVerificationResult);
			}
		})
	}

	exportTransactions(batch_id) {
		let self = this;
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			ABatchNumber: batch_id,
			AJournalNumber: 1
		};

		api.post('serverMFinance.asmx/TGLTransactionWebConnector_ExportGLBatchTransactions', x).then(function (result) {
			let parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {

				var link = document.createElement("a");
				link.style = "display: none";
				link.href = 'data:application/excel;base64,'+parsed.AExportExcel;
				link.download = i18next.t('GLTransactions') + '.xlsx';
				document.body.appendChild(link);
				link.click();
				link.remove();
			}
			if (parsed.result == "false") {
				for (var msg of parsed.AVerificationResult) {
					utils.display_message(i18next.t(msg.code), "fail");
				}
			}
		})
	}


	/////

	test_post(batch_id) {
		let self = this;
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			ABatchNumber: batch_id
		};
		api.post( 'serverMFinance.asmx/TGLTransactionWebConnector_TestPostGLBatch', x).then(function (data) {
			data = JSON.parse(data.data.d);
			if (data.result == true) {
				// 2 minute timeout
				utils.display_message ( data.ResultingBalances, null, 2*60*1000 );
			} else {
				utils.display_error( data.AVerifications );
			}
		})
	}

	batch_post(batch_id) {
		let self = this;
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			ABatchNumber: batch_id
		};
		api.post( 'serverMFinance.asmx/TGLTransactionWebConnector_PostGLBatch', x).then(function (data) {
			data = JSON.parse(data.data.d);
			if (data.result == true) {
				utils.display_message( i18next.t('GLBatches.success_posted'), 'success' );
				self.display_list('filter');
			} else {
				utils.display_error( data.AVerifications );
			}
		})
	}

	batch_cancel(batch_id) {
		let self = this;
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			ABatchNumber: batch_id
		};
		api.post( 'serverMFinance.asmx/TGLTransactionWebConnector_CancelGLBatch', x).then(function (data) {
			data = JSON.parse(data.data.d);
			if (data.result == true) {
				utils.display_message( i18next.t('GLBatches.success_cancelled'), 'success' );
				self.display_list('filter');
			} else {
				utils.display_error( data.AVerificationResult );
			}
		})
	}

	batch_reverse(batch_id) {
		let self = this;
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			ABatchNumberToReverse: batch_id,
			ADateForReversal: '1900-01-01', // use the date of the batch to reverse, if possible
			AAutoPostReverseBatch: false
		};
		api.post( 'serverMFinance.asmx/TGLTransactionWebConnector_ReverseBatch', x).then(function (data) {
			data = JSON.parse(data.data.d);
			if (data.result == true) {
				utils.display_message( i18next.t('GLBatches.success_revert'), 'success' );
				self.load_preset();
			} else {
				utils.display_error( data.AVerificationResult );
			}
		})
	}
}

export default new GLBatches();