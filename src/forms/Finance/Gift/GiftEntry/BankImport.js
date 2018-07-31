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

	api.post('serverMFinance.asmx/TBankImportWebConnector_GetImportedBankStatements', x).then(function (data) {
		data = JSON.parse(data.data.d);
		// on reload, clear content
		let field = $('#bank_number_id').html('');
		for (item of data.result) {
			field.append( $('<option value="'+item.a_statement_key_i+'">'+
				item.a_filename_c + ' ' + printJSONDate(item.a_date_d) + '</option>') );
		}
	})
}

function display_list() {
	let x = {};
	x['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
	x['AStatementKey'] = $('#bank_number_id').val();
	x['AMatchAction'] = $('#match_status_id').val();
	api.post('serverMFinance.asmx/TBankImportWebConnector_GetTransactions', x).then(function (data) {
		data = JSON.parse(data.data.d);
		// on reload, clear content
		let field = $('#browse_container').html('');
		for (item of data.result) {
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

function new_trans_detail(trans_order) {
	let x = {
		"ALedgerNumber":window.localStorage.getItem('current_ledger'),
		"AStatementKey":$('#bank_number_id').val(),
		"AOrderNumber": trans_order
	};
	api.post('serverMFinance.asmx/TBankImportWebConnector_LoadTransactionAndDetails', x).then(function (data) {
		parsed = JSON.parse(data.data.d);
		let p = parsed.ATransactions[0];
		p['a_detail_i'] = $('#modal_space .tpl_edit_trans .detail_col > *').length;
		let tpl_edit_raw = format_tpl( $('[phantom] .tpl_edit_trans_detail').clone(), p );
		$('#modal_space').append(tpl_edit_raw);
		$('.modal').modal('hide');
		let sclass = $('#modal_space > .tpl_edit_trans [name=MatchAction]:checked').val();
		tpl_edit_raw.append( $('<input type=hidden name=AMatchAction value="'+ sclass + '">') );
		tpl_edit_raw.find('[action]').val('create');
		tpl_edit_raw.modal('show');
		update_requireClass(tpl_edit_raw, sclass);
	})
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
	api.post('serverMFinance.asmx/TBankImportWebConnector_LoadTransactionAndDetails', x).then(function (data) {
		parsed = JSON.parse(data.data.d);
		let tpl_edit_raw = format_tpl( $('[phantom] .tpl_edit_trans').clone(), parsed.ATransactions[0] );

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

	let x = {
		"ALedgerNumber":window.localStorage.getItem('current_ledger'),
		"AStatementKey":statement_id,
		"AOrder": order_id,
		"ADetail": detail_id
	};
	api.post('serverMFinance.asmx/TBankImportWebConnector_LoadTransactionDetail', x).then(function (data) {
		parsed = JSON.parse(data.data.d);
		let tpl_edit_raw = format_tpl( $('[phantom] .tpl_edit_trans_detail').clone(), parsed.TransactionDetail[0] );
		let sclass = $('#modal_space > .modal [name=MatchAction]:checked').val();
		tpl_edit_raw.append( $('<input type=hidden name=AMatchAction value="'+ sclass + '">') );
		$('#modal_space').append(tpl_edit_raw);
		$('.modal').modal('hide');
		tpl_edit_raw.find('[action]').val('update');
		tpl_edit_raw.modal('show');
		update_requireClass(tpl_edit_raw, sclass);
	})
}

/////

function save_edit_trans(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let mode = obj.find('[action]').val();

	// extract information from a jquery object
	let payload = translate_to_server( extract_data(obj) );
 	payload['action'] = mode;
	payload['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
	payload["AStatementKey"] = $('#bank_number_id').val();

	api.post('serverMFinance.asmx/TBankImportWebConnector_MaintainTransaction', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			$('#modal_space .modal').modal('hide');
			display_list();
		}
		else if (parsed.result == false) {
			display_message(i18next.t('forms.error'), "success");
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
 	payload['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
	payload['AStatementKey'] = $('#bank_number_id').val();

	api.post('serverMFinance.asmx/TBankImportWebConnector_MaintainTransactionDetail', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			$('#modal_space .modal').modal('hide');
			display_list();
		}
		else if (parsed.result == false) {
			display_message(i18next.t('forms.error'), "fail");
			for (error of parsed.AVerificationResult) {
				display_message(i18next.t(error.msg), "fail");
			}
		}

	});

}

/////

function delete_trans_detail(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let payload = translate_to_server( extract_data(obj) );
	payload['action'] = "delete";
	payload['AStatementKey'] = $('#bank_number_id').val();
	payload['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
	api.post('serverMFinance.asmx/TBankImportWebConnector_MaintainTransactionDetail', payload).then(function (data) {
		parsed = JSON.parse(data.data.d);
		if (parsed.result) {
			$('#modal_space .modal').modal('hide');
			display_message(i18next.t('forms.deleted'), "success");
		} else {
			display_message(i18next.t('forms.error'), "fail");
			for (error of parsed.AVerificationResult) {
				display_message(i18next.t(error.msg), "fail");
			}
		}

	});
}

/////

function import_file(self) {
	self = $(self);
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
			'ALedgerNumber': window.localStorage.getItem('current_ledger'),
			'ABankAccountCode': '6200', // TODO
			'ABankStatementFilename': filename,
			'ACSVContent': event.target.result,
			'ASeparator': ';',
			'ADateFormat': "dmy",
			"ANumberFormat": "European",
			"ACurrencyCode": "EUR",
			"AStartAfterLine": '"Buchungstag";"Wertstellungstag";"Verwendungszweck";"Umsatz";"Währung"',
			"AColumnMeaning": "unused,DateEffective,Description,Amount,Currency" // TODO
			};

		api.post('serverMFinance.asmx/TBankImportWebConnector_ImportFromCSVFile', p)
		.then(function (result) {
			result = JSON.parse(result.data.d);
			result = result.result;
			if (result == true) {
				display_message(i18next.t('BankImport.upload_success'), "success");
				display_dropdownlist();
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
	reader.readAsText(self[0].files[0], 'ISO-8859-1');

};

function transform_to_gl() {
 let x = {
	 ALedgerNumber: window.localStorage.getItem('current_ledger'),
	 AStatementKey: $('#bank_number_id').val(),
 };
 // AVerificationResult
	api.post('serverMFinance.asmx/TBankImportWebConnector_CreateGLBatch', x).then(function (data) {
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
				display_message('forms.error', 'fail');
			}
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
				display_message('forms.error', 'fail');
			}
		}
	});

}
