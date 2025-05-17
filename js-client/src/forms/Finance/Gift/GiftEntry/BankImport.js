// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//	   Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//	   Christopher Jäkel
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
import AutocompletePartner from '../../../../lib/autocomplete_partner.js'
import AutocompleteMotivation from '../../../../lib/autocomplete_motivation.js'
import AutocompleteAccCc from '../../../../lib/autocomplete_posting_acc_cc.js'

class BankImport {

	Ready() {
		let self = this;

		$('#bank_number_id').on('click', function(){ self.display_list(); });
		$('#btnDisplay').on('click', function() { self.display_list(); })

		self.display_dropdownlist();
		self.load_preset();
		$('#autocomplete_bankaccountcode').on('input', function () {AutocompleteAccCc.autocomplete_a(this)});
	}

	display_dropdownlist(selected_statement = null) {
		let self = this;
		// x is search
		let x = {};
		x['ALedgerNumber'] = window.localStorage.getItem('current_ledger');

		api.post('serverMFinance.asmx/TBankImportWebConnector_GetImportedBankStatements', x).then(function (data) {
			data = JSON.parse(data.data.d);
			// on reload, clear content
			let field = $('#bank_number_id').html('');
			let first = true;
			let selected = "";
			for (var item of data.result) {
				if ((selected_statement == null && first) || (selected_statement == item.a_statement_key_i)) {
					selected = "selected";
					first = false;
				} else {
					selected = "";
				}
				field.append( $('<option value="'+item.a_statement_key_i+'" '+selected+'>'+
					item.a_filename_c + ' ' + tpl.printJSONDate(item.a_date_d) + '</option>') );
			}
			self.display_list();
		})
	}

	load_preset() {
		let x = {};
		api.post('serverMFinance.asmx/TBankImportWebConnector_ReadSettings', x).then(function (data) {
			data = JSON.parse(data.data.d);
			data['a_bank_account_name_c'] = data['ABankAccountCode'];
			tpl.format_tpl($('#tabsettings'), data);
			tpl.format_tpl($('#toolbar'), data);
		});
	}

	save_preset() {
		var x = tpl.extract_data($('#tabsettings'));
		api.post('serverMFinance.asmx/TBankImportWebConnector_SaveSettings', x).then(function (data) {
			data = JSON.parse(data.data.d);
		});
	}

	display_list() {
		let self = this;
		let x = {};
		x['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
		x['AStatementKey'] = $('#bank_number_id').val();
		if (x['AStatementKey'] == null) {
			// only clear the list if there is no statement selected
			let field = $('#browse_container').html('');
			return;
		}
		x['AMatchAction'] = $('#match_status_id').val();
		api.post('serverMFinance.asmx/TBankImportWebConnector_GetTransactions', x).then(function (data) {
			data = JSON.parse(data.data.d);
			// on reload, clear content
			let field = $('#browse_container').html('');
			for (var item of data.result) {
				self.format_item(item);
			}
			tpl.format_currency(data.ACurrencyCode);
			tpl.format_date();
			$('#trans_total_debit').text(tpl.printCurrency(data.ATotalDebit, data.ACurrencyCode));
			$('#trans_total_credit').text(tpl.printCurrency(data.ATotalCredit, data.ACurrencyCode));
		})
	}

	updateTransaction(StatementKey, OrderId) {
		let self = this;
		// somehow the original window stays gray when we return from this modal.
		$('.modal-backdrop').remove();
		self.edit_gift_trans(StatementKey, OrderId);
	}

	format_item(item) {
		let self = this;
		if (item.a_account_name_c != null) {
			item.a_description_c = item.a_account_name_c + "; " + item.a_description_c;
		}
		let row = tpl.format_tpl($("[phantom] .tpl_row").clone(), item);
		// let view = tpl.format_tpl($("[phantom] .tpl_view").clone(), item);
		// row.find('.collapse_col').append(view);
		row.find('#btnEdit').on('click', function () {self.edit_gift_trans(item['a_statement_key_i'],item['a_order_i'])});
		$('#browse_container').append(row);
	}

	/////

	new_trans_detail(trans_order) {
		let self = this;
		if (!modal.allow_modal()) {return}
		let x = {
			"ALedgerNumber":window.localStorage.getItem('current_ledger'),
			"AStatementKey":$('#bank_number_id').val(),
			"AOrderNumber": trans_order
		};
		api.post('serverMFinance.asmx/TBankImportWebConnector_LoadTransactionAndDetails', x).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			let p = parsed.ATransactions[0];
			p['p_donor_key_n'] = tpl.getKeyValue($('.tpl_edit_trans'), 'p_donor_key_n');
			if (p['DonorKey'] == p['p_donor_key_n']) {
				p['p_donor_short_name_c'] = p['DonorName'];
			}
			p['a_detail_i'] = $('#modal_space .tpl_edit_trans .detail_col > *').length;
			let tpl_edit_raw = tpl.format_tpl( $('[phantom] .tpl_edit_trans_detail').clone(), p );
			$('#modal_space').append(tpl_edit_raw);
			let sclass = $('#modal_space > .tpl_edit_trans [name=MatchAction]:checked').val();
			tpl_edit_raw.append( $('<input type=hidden name=AMatchAction value="'+ sclass + '">') );
			tpl_edit_raw.find('[action]').val('create');

			tpl.update_requireClass(tpl_edit_raw, sclass);
			let m = tpl_edit_raw.modal('show');

			m.find('#autocomplete_motivationdetail').on('input', function () {AutocompleteMotivation.autocomplete_motivation_detail(this, self.update_motivation_group)});
			m.find('#autocomplete_glaccountcode').on('input', function () {AutocompleteAccCc.autocomplete_a(this)});
			m.find('#autocomplete_glcostcenter').on('input', function () {AutocompleteAccCc.autocomplete_cc(this)});
			m.find('#autocomplete_member').on('input', function () {AutocompletePartner.autocomplete_member(this)});
			m.find('#btnClearMember').on('click', function() { self.clear_member(this)});
			m.find('#btnSave').on('click', function () {self.save_edit_trans_detail(this)});
			m.find('#btnClose').on('click', function () {modal.CloseModal(this)});
		})
	}

	/////

	edit_gift_trans(statement_key, trans_order) {
		let self = this;
		if (!modal.allow_modal()) {return}
		let x = {
			"ALedgerNumber":window.localStorage.getItem('current_ledger'),
			"AStatementKey":statement_key,
			"AOrderNumber": trans_order
		};
		// on open of a edit modal, we get new data,
		// so everything is up to date and we don't have to load it, if we only search
		api.post('serverMFinance.asmx/TBankImportWebConnector_LoadTransactionAndDetails', x).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			let transaction = parsed.ATransactions[0];
			if (transaction['DonorKey'] != 0) {
				transaction['p_donor_name_c'] = transaction['DonorKey'] + ' ' + transaction['DonorName'];
			} else {
				transaction['p_donor_name_c'] = "";
			}
			transaction['p_donor_key_n'] = transaction['DonorKey'];

			if (transaction['a_iban_c'] != null) {
				if (transaction['a_account_name_c'] == null) {
					transaction['a_account_name_c'] = transaction['a_iban_c'];
				}
				else {
					transaction['a_account_name_c'] += "; " + transaction['a_iban_c'];
				}
			}
			let tpl_edit_raw = tpl.format_tpl( $('[phantom] .tpl_edit_trans').clone(), transaction);

			for (var detail of parsed.ADetails) {
				let item = detail;
				let tpl_trans_detail = tpl.format_tpl( $('[phantom] .tpl_trans_detail_row').clone(), detail );
				tpl_trans_detail.find('#btnEditGiftTransDetail').on('click',
					function(){self.edit_gift_trans_detail(item['a_statement_key_i'], item['a_order_i'], item['a_detail_i'])});
				tpl_edit_raw.find('.detail_col').append(tpl_trans_detail);
			}

			$('#modal_space').html(tpl_edit_raw);

			tpl.update_requireClass(tpl_edit_raw, parsed.ATransactions[0].MatchAction);
			let m = tpl_edit_raw.modal('show');
			m.find('[action]').val('update');
			m.find('#fieldsetMatchAction').on('change', function() { tpl.update_requireClass(tpl_edit_raw, $(this).find('[type=radio]:checked').val());});
			m.find('#inputDonor').on('input', function() { AutocompletePartner.autocomplete_donor(this)});
			m.find('#btnClearDonor').on('click', function() { self.clear_donor(this)});
			m.find('#btnSave').on('click', function() { self.save_edit_trans(this)});
			m.find('#btnClose').on('click', function() { modal.CloseModal(this)});
			m.find('#btnNewDetail').on('click', function() { self.new_trans_detail(transaction['a_order_i'])});
		})
	}

	edit_gift_trans_detail(statement_id, order_id, detail_id) {
		let self = this;
		if (!modal.allow_modal()) {return}
		let x = {
			"ALedgerNumber":window.localStorage.getItem('current_ledger'),
			"AStatementKey":statement_id,
			"AOrder": order_id,
			"ADetail": detail_id
		};
		api.post('serverMFinance.asmx/TBankImportWebConnector_LoadTransactionDetail', x).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			let detail = parsed.TransactionDetail[0];

			if (detail['p_recipient_key_n'] != 0) {
				detail['p_member_name_c'] = detail['p_recipient_key_n'] + ' ' + detail['p_recipient_short_name_c'];
			} else {
				detail['p_member_name_c'] = "";
			}

			detail['p_donor_key_n'] = tpl.getKeyValue($('.tpl_edit_trans'), 'p_donor_key_n');
			let tpl_edit_raw = tpl.format_tpl( $('[phantom] .tpl_edit_trans_detail').clone(), detail);
			let sclass = $('#modal_space > .modal [name=MatchAction]:checked').val();
			tpl_edit_raw.append( $('<input type=hidden name=AMatchAction value="'+ sclass + '">') );
			$('#modal_space').append(tpl_edit_raw);
			tpl_edit_raw.find('[action]').val('update');
			tpl.update_requireClass(tpl_edit_raw, sclass);
			let m = tpl_edit_raw.modal('show');

			if (detail['a_membership_l']) {
				tpl_edit_raw.find('.MEMBERFEE').show();
			}

			m.find('#autocomplete_motivationdetail').on('input', function () {AutocompleteMotivation.autocomplete_motivation_detail(this, self.update_motivation_group)});
			m.find('#autocomplete_glaccountcode').on('input', function () {AutocompleteAccCc.autocomplete_a(this)});
			m.find('#autocomplete_glcostcenter').on('input', function () {AutocompleteAccCc.autocomplete_cc(this)});
			m.find('#autocomplete_member').on('input', function () {AutocompletePartner.autocomplete_member(this)});
			m.find('#btnClearMember').on('click', function() { self.clear_member(this)});
			m.find('#btnSave').on('click', function () {self.save_edit_trans_detail(this)});
			m.find('#btnDelete').on('click', function () {self.delete_trans_detail(this)});
			m.find('#btnClose').on('click', function () {modal.CloseModal(this)});
		})
	}

	update_motivation_group(input_field_object, selected_value) {
		let obj = $(input_field_object).closest('.modal');
		let details = JSON.parse(input_field_object.attr('details'));
		if (details.membership) {
			let recipient = obj.find('input[name=p_member_name_c]');
			if (recipient.val() == '') {
				let donorkey = obj.find('input[name=p_donor_key_n]').val();
				let donorname = obj.find('input[name=p_donor_short_name_c]').val();
				// default to the matched donor
				recipient.val(donorkey + ' ' + donorname);
				recipient.attr('key-value', donorkey);
			}
			obj.find('.MEMBERFEE').show();
		} else {
			obj.find('.MEMBERFEE').hide();
		}
	}

	/////

	save_edit_trans(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		let mode = obj.find('[action]').val();

		// extract information from a jquery object
		let payload = utils.translate_to_server( tpl.extract_data(obj) );
		payload['action'] = mode;
		payload['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
		payload["AStatementKey"] = $('#bank_number_id').val();

		api.post('serverMFinance.asmx/TBankImportWebConnector_MaintainTransaction', payload).then(function (result) {
			let parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				utils.display_message(i18next.t('forms.saved'), "success");
				modal.CloseModal(obj);
				self.display_list();
			}
			else if (parsed.result == false) {
				utils.display_error(parsed.AVerificationResult);
			}
		});
	}

	save_edit_trans_detail(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		let mode = obj.find('[action]').val();

		// extract information from a jquery object
		let payload = utils.translate_to_server( tpl.extract_data(obj) );
		payload['action'] = mode;
		payload['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
		payload['AStatementKey'] = $('#bank_number_id').val();
		if (payload['ARecipientKey'] == '') { payload['ARecipientKey'] = 0; }

		api.post('serverMFinance.asmx/TBankImportWebConnector_MaintainTransactionDetail', payload).then(function (result) {
			let parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				utils.display_message(i18next.t('forms.saved'), "success");
				self.updateTransaction(payload['AStatementKey'], payload['AOrder']);
			}
			else if (parsed.result == false) {
				utils.display_error(parsed.AVerificationResult);
			}

		});

	}

	/////

	delete_trans_detail(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		let payload = utils.translate_to_server( tpl.extract_data(obj) );
		payload['action'] = "delete";
		payload['AStatementKey'] = $('#bank_number_id').val();
		payload['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
		api.post('serverMFinance.asmx/TBankImportWebConnector_MaintainTransactionDetail', payload).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			if (parsed.result) {
				utils.display_message(i18next.t('forms.deleted'), "success");
				self.updateTransaction(payload['AStatementKey'], payload['AOrder']);
			} else {
				utils.display_error(parsed.AVerificationResult);
			}

		});
	}

	/////

	import_csv_file(obj) {

		let self = this;
		obj = $(obj);
		var filename = obj.val();

		// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
		if (window.File && window.FileReader && window.FileList && window.Blob) {
			//alert("Great success! All the File APIs are supported.");
		} else {
			alert('The File APIs are not fully supported in this browser.');
		}

		var settings = tpl.extract_data($('#tabsettings'));
		var settings2 = tpl.extract_data($('#toolbar'));

		var reader = new FileReader();

		reader.onload = function (event) {

			let p = {
				'ALedgerNumber': window.localStorage.getItem('current_ledger'),
				'ABankAccountCode': settings2['ABankAccountCode'],
				'ABankStatementFilename': filename,
				'ACSVContent': event.target.result,
				'ASeparator': settings['ASeparator'],
				'ADateFormat': settings['ADateFormat'],
				"ANumberFormat": settings['ANumberFormat'],
				"AStartAfterLine": settings['AStartAfterLine'],
				"AColumnMeaning": settings['AColumnMeaning']
				};

			api.post('serverMFinance.asmx/TBankImportWebConnector_ImportFromCSVFile', p)
			.then(function (result) {
				let data = JSON.parse(result.data.d);
				result = data.result;
				if (result == true) {
					utils.display_message(i18next.t('BankImport.upload_success'), "success");
					self.display_dropdownlist(data.AStatementKey);
				} else {
					utils.display_message(i18next.t('BankImport.upload_fail'), "fail");
				}
			})
			.catch(error => {
				//console.log(error.response)
				utils.display_message(i18next.t('BankImport.upload_fail'), "fail");
			});

		}
		// Read in the file as a data URL.
		reader.readAsText(obj[0].files[0], settings['AFileEncoding']);

	};

	import_camt_file(obj) {

		let self = this;
		obj = $(obj);
		var filename = obj.val();

		// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
		if (window.File && window.FileReader && window.FileList && window.Blob) {
			//alert("Great success! All the File APIs are supported.");
		} else {
			alert('The File APIs are not fully supported in this browser.');
		}

		if (filename.endsWith(".zip")) {
			self.import_camt_zip_file(obj, filename);
			return;
		}

		var settings = tpl.extract_data($('#toolbar'));

		var reader = new FileReader();

		reader.onload = function (event) {

			let p = {
				'ALedgerNumber': window.localStorage.getItem('current_ledger'),
				'ABankAccountCode': settings['ABankAccountCode'],
				'ABankStatementFilename': filename,
				'ACAMTContent': event.target.result,
				};

			api.post('serverMFinance.asmx/TBankImportWebConnector_ImportFromCAMTFile', p)
			.then(function (result) {
				result = JSON.parse(result.data.d);
				result = result.result;
				if (result == true) {
					utils.display_message(i18next.t('BankImport.upload_success'), "success");
					self.display_dropdownlist();
				} else {
					utils.display_message(i18next.t('BankImport.upload_fail'), "fail");
				}
			})
			.catch(error => {
				//console.log(error.response)
				utils.display_message(i18next.t('BankImport.upload_fail'), "fail");
			});

		}
		// Read in the file as a data URL.
		reader.readAsText(obj[0].files[0]);
	};

	import_camt_zip_file(obj, filename) {

		let self = this;
		var settings = tpl.extract_data($('#toolbar'));

		var reader = new FileReader();

		reader.onload = function (event) {

			p = {
				'ALedgerNumber': window.localStorage.getItem('current_ledger'),
				'ABankAccountCode': settings['ABankAccountCode'],
				'AZipFileContent': event.target.result,
				};

			api.post('serverMFinance.asmx/TBankImportWebConnector_ImportFromCAMTZIPFile', p)
			.then(function (result) {
				result = JSON.parse(result.data.d);
				result = result.result;
				if (result == true) {
					utils.display_message(i18next.t('BankImport.upload_success'), "success");
					self.display_dropdownlist();
				} else {
					utils.display_message(i18next.t('BankImport.upload_fail'), "fail");
				}
			})
			.catch(error => {
				//console.log(error.response)
				utils.display_message(i18next.t('BankImport.upload_fail'), "fail");
			});

		}
		// Read in the file as a data URL.
		reader.readAsDataURL(obj[0].files[0]);
	};

	import_mt940_file(obj) {

		let self = this;
		obj = $(obj);
		var filename = obj.val();

		// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
		if (window.File && window.FileReader && window.FileList && window.Blob) {
			//alert("Great success! All the File APIs are supported.");
		} else {
			alert('The File APIs are not fully supported in this browser.');
		}

		var settings = tpl.extract_data($('#toolbar'));

		var reader = new FileReader();

		reader.onload = function (event) {

			let p = {
				'ALedgerNumber': window.localStorage.getItem('current_ledger'),
				'ABankAccountCode': settings['ABankAccountCode'],
				'ABankStatementFilename': filename,
				'AMT940Content': event.target.result,
				};

			api.post('serverMFinance.asmx/TBankImportWebConnector_ImportFromMT940File', p)
			.then(function (result) {
				result = JSON.parse(result.data.d);
				result = result.result;
				if (result == true) {
					utils.display_message(i18next.t('BankImport.upload_success'), "success");
					self.display_dropdownlist();
				} else {
					utils.display_message(i18next.t('BankImport.upload_fail'), "fail");
				}
			})
			.catch(error => {
				//console.log(error.response)
				utils.display_message(i18next.t('BankImport.upload_fail'), "fail");
			});

		}
		// Read in the file as a data URL.
		reader.readAsText(obj[0].files[0], settings['AFileEncoding']);
	};

	/////

	transform_to_gl() {
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			AStatementKey: $('#bank_number_id').val(),
		};
		api.post('serverMFinance.asmx/TBankImportWebConnector_CreateGLBatch', x).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			if (parsed.result == true) {
				utils.display_message( i18next.t('forms.saved'), 'success' )
			}
			else {
				utils.display_error( parsed.AVerificationResult );
			}
		});


	}

	transform_to_gift() {
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			AStatementKey: $('#bank_number_id').val(),
		};
		api.post('serverMFinance.asmx/TBankImportWebConnector_CreateGiftBatch', x).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			if (parsed.result == true) {
				utils.display_message( i18next.t('forms.saved'), 'success' )
			}
			else {
				utils.display_error( parsed.AVerificationResult );
			}
		});

	}

	check_for_sponsorship() {
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			AStatementKey: $('#bank_number_id').val(),
		};

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_CheckIncomingDonationsForSponsorship', x).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			if (parsed.result == true) {
				utils.display_message( i18next.t('TODO'), 'success' );
			}
			else {
				utils.display_error( parsed.AVerificationResult );
			}
		});
	}

	delete_current_statement() {
		let self = this;
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			AStatementKey: $('#bank_number_id').val(),
		};

		let s = confirm( i18next.t('BankImport.ask_delete_stmt') );
		if (!s) {return}

		api.post('serverMFinance.asmx/TBankImportWebConnector_DropBankStatement', x).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			if (parsed.result == true) {
				utils.display_message( i18next.t('forms.deleted'), 'success' );
				self.display_dropdownlist();
			}
			else {
				utils.display_error( parsed.AVerificationResult );
			}
		});
	}

	delete_old_statements() {
		let self = this;
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			ACriteria: 'OlderThan1Year'
		};

		let s = confirm( i18next.t('BankImport.ask_delete_stmts') );
		if (!s) {return}

		api.post('serverMFinance.asmx/TBankImportWebConnector_DropBankStatements', x).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			if (parsed.result == true) {
				utils.display_message( i18next.t('forms.deleted'), 'success' );
				self.display_dropdownlist();
			}
			else {
				utils.display_error( parsed.AVerificationResult );
			}
		});
	}

	delete_all_statements() {
		let self = this;
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			ACriteria: 'All'
		};

		let s = confirm( i18next.t('BankImport.ask_delete_stmts') );
		if (!s) {return}

		api.post('serverMFinance.asmx/TBankImportWebConnector_DropBankStatements', x).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			if (parsed.result == true) {
				utils.display_message( i18next.t('forms.deleted'), 'success' );
				self.display_dropdownlist();
			}
			else {
				utils.display_error( parsed.AVerificationResult );
			}
		});
	}

	clear_donor(obj) {
		let obj_donorName = $(obj).parent().find('input[name=p_donor_name_c]');
		obj_donorName.val("");
		obj_donorName.attr('key-value', 0);
	}

	clear_member(obj) {
		let obj_memberName = $(obj).parent().find('input[name=p_member_name_c]');
		obj_memberName.val("");
		obj_memberName.attr('key-value', 0);
	}

}

export default new BankImport();