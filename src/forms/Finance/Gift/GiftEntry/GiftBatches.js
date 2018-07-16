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
	display_list();
});

function display_list() {
	// x is search
	let x = {
		ALedgerNumber: 43,
		AYear: 0,
		APeriod: 1
	};

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
	let x = {"ALedgerNumber":43, "ABatchNumber":number};
	api.post('serverMFinance.asmx/TGLTransactionWebConnector_LoadABatchAJournalATransaction', x).then(function (data) {
		data = JSON.parse(data.data.d);
		// on open, clear content
		let place_to_put_content = obj.find('.content_col').html('');
		for (item of data.result.ATransaction) {
			let transaction_row = $('[phantom] .tpl_transaction').clone();
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
function new_batch(ledger_number) {
	let x = {ALedgerNumber :ledger_number};
	api.post('serverMFinance.asmx/TGLTransactionWebConnector_CreateABatch', x).then(
		function (data) {
			parsed = JSON.parse(data.data.d);
			new_entry_data = parsed.result;
			let p = format_tpl( $('[phantom] .tpl_edit_batch').clone(), parsed['result']['ABatch'][0] );
			$('#modal_space').html(p);
			p.find('input').attr('readonly', false);
			p.find('[action]').val('create');
			p.modal('show');
		}
	)
};

function new_trans(ledger_number, batch_number) {
	let x = {ALedgerNumber :ledger_number};
	api.post('serverMFinance.asmx/TGLTransactionWebConnector_CreateABatch', x).then(
		function (data) {
			parsed = JSON.parse(data.data.d);
			new_entry_data = parsed.result;
			let p = format_tpl( $('[phantom] .tpl_edit_trans').clone(), parsed['result']['ATransaction'][0] );
			$('#modal_space').html(p);
			p.find('input').attr('readonly', false);
			p.find('[action]').val('create');
			p.modal('show');
		}
	)
};

/////

function edit_batch(batch_id) {
	var r = {
				ALedgerNumber: 43,
				ABatchNumber: batch_id,
			};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we don't have to load it, if we only search
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_LoadAGiftBatchSingle', r).then(function (data) {
		parsed = JSON.parse(data.data.d)
		let batch = parsed.result.AGiftBatch[0];
		console.log(batch);
		let tpl_m = format_tpl( $('[phantom] .tpl_edit_batch').clone(), batch );

		$('#modal_space').html(tpl_m);
		tpl_m.find('[action]').val('edit');
		tpl_m.modal('show');

	})
}

function edit_trans(batch_id, trans_id) {
	let x = {"ALedgerNumber":43, "ABatchNumber":batch_id};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we don't have to load it, if we only search
	api.post('serverMFinance.asmx/TGLTransactionWebConnector_LoadABatchAJournalATransaction', x).then(function (data) {
		parsed = JSON.parse(data.data.d);
		let searched = null;
		new_entry_data = parsed.result;
		for (trans of parsed.result.ATransaction) {
			if (trans.a_transaction_number_i == trans_id) {
				searched = trans;
				break;
			}
		}
		if (searched == null) {
			return alert('ERROR');
		}
		let tpl_m = format_tpl( $('[phantom] .tpl_edit_trans').clone(), searched );
		$('#modal_space').html(tpl_m);
		tpl_m.find('[action]').val('edit');
		tpl_m.modal('show');

	})
}

/////

function save_edit_batch(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let mode = obj.find('[action]').val();

	// extract information from a jquery object
	let payload = translate_to_server( extract_data(obj) );
 	payload['action'] = mode;

	console.log(payload);
	api.post('serverMFinance.asmx/TGLTransactionWebConnector_MaintainBatches', payload);
}

function save_edit_trans(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let mode = obj.find('[action]').val();

	// extract information from a jquery object
	let payload = translate_to_server( extract_data(obj) );
 	payload['action'] = mode;

	console.log(payload);
	api.post('serverMFinance.asmx/TGLTransactionWebConnector_MaintainTransactions', payload);

}
