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
	display_dropdownlist();
});

function display_dropdownlist() {
	// x is search
	let x = {};
	x['ALedgerNumber'] = window.localStorage.getItem('current_ledger');

	api.post('serverMFinance.asmx/TransactionWebConnector_GetBankStatements', x).catch(function (data) {
		// data = JSON.parse(data.data.d);
		data = [
			{a_statement_key_i: "3", a_statement_name_c: "März"},
			{a_statement_key_i: "4", a_statement_name_c: "April"},
			{a_statement_key_i: "5", a_statement_name_c: "Mai"},
			{a_statement_key_i: "6", a_statement_name_c: "Juni"},
			{a_statement_key_i: "7", a_statement_name_c: "Juli"}
		]
		// on reload, clear content
    let field = $('#bank_number_id');
		for (item of data) {
      field.append( $('<option value="'+item.a_statement_key_i+'">'+item.a_statement_name_c+'</option>') );
		}
	})
}

function display_list() {
	let x = {};
	x['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
	x['AStatementKey'] = $('#bank_number_id').val();

	api.post('serverMFinance.asmx/TransactionWebConnector_LoadStatementTransactions', x).catch(function (data) {
		// data = JSON.parse(data.data.d);
		data = [
						{'a_statement_key_i': 1, 'a_order_i' : 0, 'a_description_c': 'Spende von TP für Projekt Rettet die Pinguine', 'a_transaction_amount_n': 50},
						{'a_statement_key_i': 1, 'a_order_i' : 1, 'a_description_c': 'Spende von CJ für Projekt Rettet die Delfine und Rettet die Heringe', 'a_transaction_amount_n': 50},
						{'a_statement_key_i': 1, 'a_order_i' : 2, 'a_description_c': 'Mieteinnahme für Untermieter', 'a_transaction_amount_n': 500}
					];
		// on reload, clear content
		let field = $('#browse_container').html('');
		for (item of data) {
			format_item(item);
		}
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

/////

function new_trans_detail() {
	let tpl_edit_raw = $('[phantom] .tpl_edit_trans_detail').clone();
	$('#modal_space').append(tpl_edit_raw);
	$('.modal').modal('hide');
	tpl_edit_raw.find('[action]').val('create');
	tpl_edit_raw.modal('show');

}

/////

function edit_gift_trans(trans_order) {
	let x = {
		"ALedgerNumber":window.localStorage.getItem('current_ledger'),
		"AStatementKey":$('#bank_number_id').val(),
		"AOrderNumber": trans_order
	};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we don't have to load it, if we only search

	// serverMFinance.asmx/TGiftTransactionWebConnector_LoadGiftTransactionsDetail
	api.post('serverMFinance.asmx/TBankImportWebConnector_LoadTransactionAndDetails', x).catch(function (data) {
		// parsed = JSON.parse(data.data.d);
		parsed = {
			'transaction': {
				'a_statement_key_i': 1, 'a_order_i' : 0, 'a_description_c': 'Spende von TP für Projekt Rettet die Pinguine', 'a_transaction_amount_n': 50, 'a_action_c':'unmatched', 'p_donor_key_n': null
			},
			'details': [
				{
					'a_statement_key_i': 1, 'a_order_i' : 0, 'a_detail_i': 1, 'a_transaction_amount_n': 50, 'a_motivation_group_code_c': null, 'a_motivation_detail_code_c': null
				},
				{
					'a_statement_key_i': 1, 'a_order_i' : 0, 'a_detail_i': 2, 'a_transaction_amount_n': 500, 'a_motivation_group_code_c': null, 'a_motivation_detail_code_c': null, 'a_narrative_c': 'Mieteinnahme', 'a_account_code_c': '4211', 'a_cost_centre_code_c': '4300'
				}
			]
		}

		let tpl_edit_raw = format_tpl( $('[phantom] .tpl_edit_trans').clone(), parsed.transaction );

		for (detail of parsed.details) {
			let tpl_trans_detail = format_tpl( $('[phantom] .tpl_trans_detail_row').clone(), detail );
			tpl_edit_raw.find('.detail_col').append(tpl_trans_detail);
		}


		$('#modal_space').html(tpl_edit_raw);
		tpl_edit_raw.find('[action]').val('edit');
		tpl_edit_raw.modal('show');

	})
}

function edit_gift_trans_detail(trans_id, order_id) {

	let x = {
		"ALedgerNumber":window.localStorage.getItem('current_ledger'),
		"AStatementKey":trans_id,
		"ADetailKey": order_id
	};
	api.post('serverMFinance.asmx/TBankImportWebConnector_LoadTransactionAndDetails', x).catch(function (data) {
		// parsed = JSON.parse(data.data.d);
		parsed = {
			'transaction': {
				'a_statement_key_i': 1, 'a_order_i' : 0, 'a_description_c': 'Spende von TP für Projekt Rettet die Pinguine', 'a_transaction_amount_n': 50, 'a_action_c':'unmatched', 'p_donor_key_n': null
			},
			'details': [
				{
					'a_statement_key_i': 1, 'a_order_i' : 0, 'a_detail_i': 1, 'a_transaction_amount_n': 50, 'a_motivation_group_code_c': null, 'a_motivation_detail_code_c': null
				},
				{
					'a_statement_key_i': 1, 'a_order_i' : 0, 'a_detail_i': 2, 'a_transaction_amount_n': 500, 'a_motivation_group_code_c': null, 'a_motivation_detail_code_c': null, 'a_narrative_c': 'Mieteinnahme', 'a_account_code_c': '4211', 'a_cost_centre_code_c': '4300'
				}
			]
		}

		let detail = null;
		for (d of parsed.details) {
			if (d.a_detail_i == order_id) {
				detail = d;
				break;
			}
		}

		let tpl_edit_raw = format_tpl( $('[phantom] .tpl_edit_trans_detail').clone(), detail );

		$('#modal_space').append(tpl_edit_raw);
		$('.modal').modal('hide');
		tpl_edit_raw.find('[action]').val('edit');
		tpl_edit_raw.modal('show');

	})
}

/////

function save_edit_trans(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let mode = obj.find('[action]').val();

	// extract information from a jquery object
	let payload = translate_to_server( extract_data(obj) );
 	payload['action'] = mode;
	api.post('serverMFinance.asmx/TBankImportWebConnector_MaintainTransactions', payload).catch(function (result) {
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

function delete_trans_detail(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let payload = translate_to_server( extract_data(obj) );
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainGiftsDetails', payload);
}

/////

function import_file(self) {

	var filename = self.val();

	// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
	if (window.File && window.FileReader && window.FileList && window.Blob) {
		//alert("Great success! All the File APIs are supported.");
	} else {
	  alert('The File APIs are not fully supported in this browser.');
	}

	var reader = new FileReader();

	reader.onload = function (event) {
		p = {
			'ACSVPartnerData': event.target.result,
			'ADateFormat': "dmy",
			"ASeparator": ";"};

		api.post('serverMFinance.asmx/TImportExportWebConnector_ImportFromCSVFile', p)
		.then(function (result) {
			result = JSON.parse(result.data.d);
			result = result.result;
			if (result == true) {
				display_message(i18next.t('BankImport.upload_success'), "success");
			} else {
				display_message(i18next.t('BankImport.upload_fail'), "fail");
			}
		})
		.catch(error => {
			//console.log(error.response)
			display_message(i18next.t('BankImport.upload_fail'), "fail");
		});

	}
	// Read in the file as a data URL.
	reader.readAsText(self[0].files[0]);

};
