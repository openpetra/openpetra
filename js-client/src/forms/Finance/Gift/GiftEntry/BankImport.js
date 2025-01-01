// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//	   Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//	   Christopher Jäkel
//
// Copyright 2017-2018 by TBits.net
// Copyright 2019-2023 by SolidCharity.com
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
	$('#bank_number_id').click(function(){ display_list(); });
	display_dropdownlist();
	load_preset();
});

function display_dropdownlist(selected_statement = null) {
	// x is search
	let x = {};
	x['ALedgerNumber'] = window.localStorage.getItem('current_ledger');

	api.post('serverMFinance.asmx/TBankImportWebConnector_GetImportedBankStatements', x).then(function (data) {
		data = JSON.parse(data.data.d);
		// on reload, clear content
		let field = $('#bank_number_id').html('');
		let first = true;
		for (item of data.result) {
			if ((selected_statement == null && first) || (selected_statement == item.a_statement_key_i)) {
				selected = "selected";
				first = false;
			} else {
				selected = "";
			}
			field.append( $('<option value="'+item.a_statement_key_i+'" '+selected+'>'+
				item.a_filename_c + ' ' + printJSONDate(item.a_date_d) + '</option>') );
		}
		display_list();
	})
}

function load_preset() {
	let x = {};
	api.post('serverMFinance.asmx/TBankImportWebConnector_ReadSettings', x).then(function (data) {
		data = JSON.parse(data.data.d);
		data['a_bank_account_name_c'] = data['ABankAccountCode'];
		format_tpl($('#tabsettings'), data);
		format_tpl($('#toolbar'), data);
	});
}

function save_preset() {
	var x = tpl.extract_data($('#tabsettings'));
	api.post('serverMFinance.asmx/TBankImportWebConnector_SaveSettings', x).then(function (data) {
		data = JSON.parse(data.data.d);
	});
}

function display_list() {
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
		for (item of data.result) {
			format_item(item);
		}
		format_currency(data.ACurrencyCode);
		format_date();
		$('#trans_total_debit').text(printCurrency(data.ATotalDebit, data.ACurrencyCode));
		$('#trans_total_credit').text(printCurrency(data.ATotalCredit, data.ACurrencyCode));
	})
}

function updateTransaction(StatementKey, OrderId) {
	// somehow the original window stays gray when we return from this modal.
	$('.modal-backdrop').remove();
	edit_gift_trans(StatementKey, OrderId);
}

function format_item(item) {
	if (item.a_account_name_c != null) {
		item.a_description_c = item.a_account_name_c + "; " + item.a_description_c;
	}
	let row = format_tpl($("[phantom] .tpl_row").clone(), item);
	// let view = format_tpl($("[phantom] .tpl_view").clone(), item);
	// row.find('.collapse_col').append(view);
	$('#browse_container').append(row);
}

/////

function new_trans_detail(trans_order) {
	if (!modal.allow_modal()) {return}
	let x = {
		"ALedgerNumber":window.localStorage.getItem('current_ledger'),
		"AStatementKey":$('#bank_number_id').val(),
		"AOrderNumber": trans_order
	};
	api.post('serverMFinance.asmx/TBankImportWebConnector_LoadTransactionAndDetails', x).then(function (data) {
		parsed = JSON.parse(data.data.d);
		let p = parsed.ATransactions[0];
		p['p_donor_key_n'] = getKeyValue($('.tpl_edit_trans'), 'p_donor_key_n');
		p['a_detail_i'] = $('#modal_space .tpl_edit_trans .detail_col > *').length;
		let tpl_edit_raw = format_tpl( $('[phantom] .tpl_edit_trans_detail').clone(), p );
		$('#modal_space').append(tpl_edit_raw);
		let sclass = $('#modal_space > .tpl_edit_trans [name=MatchAction]:checked').val();
		tpl_edit_raw.append( $('<input type=hidden name=AMatchAction value="'+ sclass + '">') );
		tpl_edit_raw.find('[action]').val('create');
		tpl_edit_raw.modal('show');
		update_requireClass(tpl_edit_raw, sclass);
	})
}

/////

function edit_gift_trans(statement_key, trans_order) {
	if (!modal.allow_modal()) {return}
	let x = {
		"ALedgerNumber":window.localStorage.getItem('current_ledger'),
		"AStatementKey":statement_key,
		"AOrderNumber": trans_order
	};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we don't have to load it, if we only search
	api.post('serverMFinance.asmx/TBankImportWebConnector_LoadTransactionAndDetails', x).then(function (data) {
		parsed = JSON.parse(data.data.d);
		transaction = parsed.ATransactions[0];
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
		let tpl_edit_raw = format_tpl( $('[phantom] .tpl_edit_trans').clone(), transaction);

		for (detail of parsed.ADetails) {
			let tpl_trans_detail = format_tpl( $('[phantom] .tpl_trans_detail_row').clone(), detail );
			tpl_edit_raw.find('.detail_col').append(tpl_trans_detail);
		}

		$('#modal_space').html(tpl_edit_raw);
		tpl_edit_raw.find('[action]').val('update');
		tpl_edit_raw.modal('show');
		update_requireClass(tpl_edit_raw, parsed.ATransactions[0].MatchAction);
	})
}

function edit_gift_trans_detail(statement_id, order_id, detail_id) {
	if (!modal.allow_modal()) {return}
	let x = {
		"ALedgerNumber":window.localStorage.getItem('current_ledger'),
		"AStatementKey":statement_id,
		"AOrder": order_id,
		"ADetail": detail_id
	};
	api.post('serverMFinance.asmx/TBankImportWebConnector_LoadTransactionDetail', x).then(function (data) {
		parsed = JSON.parse(data.data.d);
		detail = parsed.TransactionDetail[0];

		if (detail['p_recipient_key_n'] != 0) {
			detail['p_member_name_c'] = detail['p_recipient_key_n'] + ' ' + detail['p_recipient_short_name_c'];
		} else {
			detail['p_member_name_c'] = "";
		}

		detail['p_donor_key_n'] = getKeyValue($('.tpl_edit_trans'), 'p_donor_key_n');
		let tpl_edit_raw = format_tpl( $('[phantom] .tpl_edit_trans_detail').clone(), detail);
		let sclass = $('#modal_space > .modal [name=MatchAction]:checked').val();
		tpl_edit_raw.append( $('<input type=hidden name=AMatchAction value="'+ sclass + '">') );
		$('#modal_space').append(tpl_edit_raw);
		tpl_edit_raw.find('[action]').val('update');
		tpl_edit_raw.modal('show');
		update_requireClass(tpl_edit_raw, sclass);

		if (detail['a_membership_l']) {
			tpl_edit_raw.find('.MEMBERFEE').show();
		}

	})
}

function update_motivation_group(input_field_object, selected_value) {
	let obj = $(input_field_object).closest('.modal');
	details = JSON.parse(input_field_object.attr('details'));
	if (details.membership) {
		recipient = obj.find('input[name=p_member_name_c]');
		if (recipient.val() == '') {
			donorkey = obj.find('input[name=p_donor_key_n]').val();
			donorname = obj.find('input[name=p_donor_short_name_c]').val();
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

function save_edit_trans(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let mode = obj.find('[action]').val();

	// extract information from a jquery object
	let payload = utils.translate_to_server( extract_data(obj) );
 	payload['action'] = mode;
	payload['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
	payload["AStatementKey"] = $('#bank_number_id').val();

	api.post('serverMFinance.asmx/TBankImportWebConnector_MaintainTransaction', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			utils.display_message(i18next.t('forms.saved'), "success");
			CloseModal(obj);
			display_list();
		}
		else if (parsed.result == false) {
			utils.display_error(parsed.AVerificationResult);
		}
	});
}

function save_edit_trans_detail(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let mode = obj.find('[action]').val();

	// extract information from a jquery object
	let payload = utils.translate_to_server( extract_data(obj) );
	payload['action'] = mode;
 	payload['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
	payload['AStatementKey'] = $('#bank_number_id').val();
	if (payload['ARecipientKey'] == '') { payload['ARecipientKey'] = 0; }

	api.post('serverMFinance.asmx/TBankImportWebConnector_MaintainTransactionDetail', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			utils.display_message(i18next.t('forms.saved'), "success");
			updateTransaction(payload['AStatementKey'], payload['AOrder']);
		}
		else if (parsed.result == false) {
			utils.display_error(parsed.AVerificationResult);
		}

	});

}

/////

function delete_trans_detail(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let payload = utils.translate_to_server( extract_data(obj) );
	payload['action'] = "delete";
	payload['AStatementKey'] = $('#bank_number_id').val();
	payload['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
	api.post('serverMFinance.asmx/TBankImportWebConnector_MaintainTransactionDetail', payload).then(function (data) {
		parsed = JSON.parse(data.data.d);
		if (parsed.result) {
			utils.display_message(i18next.t('forms.deleted'), "success");
			updateTransaction(payload['AStatementKey'], payload['AOrder']);
		} else {
			utils.display_error(parsed.AVerificationResult);
		}

	});
}

/////

function import_csv_file(self) {

	self = $(self);
	var filename = self.val();

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

		p = {
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
			data = JSON.parse(result.data.d);
			result = data.result;
			if (result == true) {
				utils.display_message(i18next.t('BankImport.upload_success'), "success");
				display_dropdownlist(data.AStatementKey);
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
	reader.readAsText(self[0].files[0], settings['AFileEncoding']);

};

function import_camt_file(self) {

	self = $(self);
	var filename = self.val();

	// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
	if (window.File && window.FileReader && window.FileList && window.Blob) {
		//alert("Great success! All the File APIs are supported.");
	} else {
	  alert('The File APIs are not fully supported in this browser.');
	}

	if (filename.endsWith(".zip")) {
		import_camt_zip_file(self, filename);
		return;
	}

	var settings = tpl.extract_data($('#toolbar'));

	var reader = new FileReader();

	reader.onload = function (event) {

		p = {
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
				display_dropdownlist();
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
	reader.readAsText(self[0].files[0]);
};

function import_camt_zip_file(self, filename) {

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
				display_dropdownlist();
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
	reader.readAsDataURL(self[0].files[0]);
};

function import_mt940_file(self) {

	self = $(self);
	var filename = self.val();

	// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
	if (window.File && window.FileReader && window.FileList && window.Blob) {
		//alert("Great success! All the File APIs are supported.");
	} else {
	  alert('The File APIs are not fully supported in this browser.');
	}

	var settings = tpl.extract_data($('#toolbar'));

	var reader = new FileReader();

	reader.onload = function (event) {

		p = {
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
				display_dropdownlist();
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
	reader.readAsText(self[0].files[0], settings['AFileEncoding']);
};

/////

function transform_to_gl() {
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

function transform_to_gift() {
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

function check_for_sponsorship() {
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

function delete_current_statement() {
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
			display_dropdownlist();
		}
		else {
			utils.display_error( parsed.AVerificationResult );
		}
	});
}

function delete_old_statements() {
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
			display_dropdownlist();
		}
		else {
			utils.display_error( parsed.AVerificationResult );
		}
	});
}

function delete_all_statements() {
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
			display_dropdownlist();
		}
		else {
			utils.display_error( parsed.AVerificationResult );
		}
	});
}

function clear_donor(self) {
    let obj_donorName = $(self).parent().find('input[name=p_donor_name_c]');
    obj_donorName.val("");
    obj_donorName.attr('key-value', 0);
}

function clear_member(self) {
    let obj_memberName = $(self).parent().find('input[name=p_member_name_c]');
    obj_memberName.val("");
    obj_memberName.attr('key-value', 0);
}