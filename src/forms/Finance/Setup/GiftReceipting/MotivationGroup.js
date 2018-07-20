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
		ALedgerNumber: window.localStorage.getItem('current_ledger')
	};

	api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', x).then(function (data) {
		data = JSON.parse(data.data.d);
		console.log(data);
		// on reload, clear content
		$('#browse_container').html('');
		for (item of data.result.AMotivationGroup) {
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

function open_motivations(obj, code) {
	obj = $(obj);
	if (obj.find('.collapse').is(':visible') ) {
		return;
	}
	let x = {
		ALedgerNumber:window.localStorage.getItem('current_ledger'),
	};
	api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', x).then(function (data) {
		data = JSON.parse(data.data.d);
		// on open, clear content
		let place_to_put_content = obj.find('.content_col').html('');
		for (item of data.result.AMotivationDetail) {
			let motivation_row = $('[phantom] .tpl_motivation').clone();
			motivation_row = format_tpl(motivation_row, item);
			place_to_put_content.append(motivation_row);
		}
		$('.tpl_row .collapse').collapse('hide');
		obj.find('.collapse').collapse('show')
	})

}

/////

var new_entry_data = {};
function new_batch() {
	let x = {a_ledger_number_i :window.localStorage.getItem('current_ledger')};
	// console.log(parsed);
	let p = format_tpl( $('[phantom] .tpl_edit_batch').clone(), x );
	$('#modal_space').html(p);
	p.find('[edit-only]').hide();
	p.find('[action]').val('create');
	p.modal('show');
};

function new_motivation(group_code) {
	let x = {a_ledger_number_i:window.localStorage.getItem('current_ledger'), a_motivation_group_code_c:group_code};
	let p = format_tpl( $('[phantom] .tpl_edit_motivation').clone(), x);
	$('#modal_space').html(p);
	p.find('input').attr('readonly', false);
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
	p.find('input').attr('readonly', false);
	p.find('[edit-only]').hide();
	p.find('[action]').val('create');
	p.modal('show');
};

/////

function edit_batch(batch_code) {
	var r = {
				ALedgerNumber: window.localStorage.getItem('current_ledger')
			};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we don't have to load it, if we only search
	api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', r).then(function (data) {
		parsed = JSON.parse(data.data.d)
		let f = null;
		for (batch of parsed.result.AMotivationGroup) {
			if (batch.a_motivation_group_code_c == batch_code) {
				f = batch
			}
		}
		let tpl_m = format_tpl( $('[phantom] .tpl_edit_batch').clone(), f );
		$('#modal_space').html(tpl_m);
		tpl_m.find('[action]').val('edit');
		tpl_m.modal('show');

	})
}

function edit_motivation(batch_id, motivation_id, motivation_detail_id) {
	let x = {"ALedgerNumber":window.localStorage.getItem('current_ledger'), "ABatchNumber":batch_id};
	api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', x).then(function (data) {
		parsed = JSON.parse(data.data.d);
		let searched = null;
		for (moti of parsed.result.AMotivationDetail) {
			if (moti.a_motivation_group_code_c == motivation_id && moti.a_motivation_detail_code_c == motivation_detail_id) {
				searched = moti;
				break;
			}
		}
		if (searched == null) {
			return alert('ERROR');
		}

		let tpl_motivation = format_tpl( $('[phantom] .tpl_edit_motivation').clone(), searched );

		$('#modal_space').html(tpl_motivation);
		tpl_motivation.find('[action]').val('edit');
		tpl_motivation.modal('show');

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
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainMotivations', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			$('#modal_space .modal').modal('hide');
			display_list();
		}
		if (parsed.result == "false") {
			for (msg of parsed.AVerificationResult) {
				display_message(i18next.t(msg.code), "fail");
			}
		}
	});
}

function save_edit_moti_detail(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let mode = obj.find('[action]').val();

	// extract information from a jquery object
	let payload = translate_to_server( extract_data(obj) );
 	payload['action'] = mode;

	console.log(payload);
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainMotivationDetails', payload);

}


function delete_batch(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let payload = translate_to_server( extract_data(obj) );
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainMotivations', payload);
}

function delete_motivation(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let payload = translate_to_server( extract_data(obj) );
	api.post('serverMFinance.asmx/TGiftTransactionWebConnector_MaintainMotivationDetails', payload);
}
