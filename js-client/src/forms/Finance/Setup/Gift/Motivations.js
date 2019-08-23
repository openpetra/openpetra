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
	display_list();
});

function display_list() {
	// x is search
	let x = {
		ALedgerNumber: window.localStorage.getItem('current_ledger'),
		AMotivationGroupCode: ''
	};

	api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', x).then(function (data) {
		data = JSON.parse(data.data.d);
		// on reload, clear content
		$('#browse_container').html('');
		for (item of data.result.AMotivationGroup) {
			format_item(item);
		}
	})
}

function updateGroup(GroupCode) {
	let x = {
		ALedgerNumber: window.localStorage.getItem('current_ledger'),
		AMotivationGroupCode: GroupCode
	};

	api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', x).then(function (data) {
		data = JSON.parse(data.data.d);
		item = data.result.AMotivationGroup[0];
		let row = format_tpl($("[phantom] .tpl_row").clone(), item);
		$('#group' + GroupCode + " div").first().replaceWith(row.children()[0]);
		format_currency(data.ACurrencyCode);
		format_date();
		open_motivations($('#group' + GroupCode), GroupCode, true);
	});
}

function format_item(item) {
	let row = format_tpl($("[phantom] .tpl_row").clone(), item);
	$('#browse_container').append(row);
}

function open_motivations(obj, code, reload = false) {
	obj = $(obj);
	if (!reload && obj.find('.collapse').is(':visible') ) {
		return;
	}
	let x = {
		ALedgerNumber:window.localStorage.getItem('current_ledger'),
		AMotivationGroupCode: code
	};
	api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', x).then(function (data) {
		data = JSON.parse(data.data.d);
		// on open, clear content
		let place_to_put_content = obj.find('.content_col').html('');
		if (data.result.hasOwnProperty('AMotivationDetail')) {
			for (item of data.result.AMotivationDetail) {
				let motivation_row = $('[phantom] .tpl_motivation').clone();
				motivation_row = format_tpl(motivation_row, item);
				place_to_put_content.append(motivation_row);
			}
		}
		if (!reload) {
			$('.tpl_row .collapse').collapse('hide');
		}
		obj.find('.collapse').collapse('show');
	})

}

/////

var new_entry_data = {};
function new_group() {
	let x = {a_ledger_number_i :window.localStorage.getItem('current_ledger')};
	// console.log(parsed);
	let p = format_tpl( $('[phantom] .tpl_edit_group').clone(), x );
	$('#modal_space').html(p);
	p.find('[edit-only]').hide();
	p.find('input[name=a_motivation_group_code_c]').attr('readonly', false);
	p.find('[action]').val('create');
	p.modal('show');
};

function new_motivation(group_code) {
	let x = {a_ledger_number_i:window.localStorage.getItem('current_ledger'), a_motivation_group_code_c:group_code};
	let p = format_tpl( $('[phantom] .tpl_edit_motivation').clone(), x);
	$('#modal_space').html(p);
	p.find('input[name=a_motivation_detail_code_c]').attr('readonly', false);
	p.find('[edit-only]').hide();
	p.find('[action]').val('create');
	p.modal('show');
};

/////

function edit_group(group_code) {
	var r = {
				ALedgerNumber: window.localStorage.getItem('current_ledger'),
				AMotivationGroupCode: group_code
			};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we don't have to load it, if we only search
	api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', r).then(function (data) {
		parsed = JSON.parse(data.data.d)
		let f = null;
		for (group of parsed.result.AMotivationGroup) {
			if (group.a_motivation_group_code_c == group_code) {
				f = group
			}
		}
		let tpl_m = format_tpl( $('[phantom] .tpl_edit_group').clone(), f );
		$('#modal_space').html(tpl_m);
		tpl_m.find('[action]').val('update');
		tpl_m.modal('show');

	})
}

function edit_motivation(group_id, detail_id) {
	let x = {
			"ALedgerNumber":window.localStorage.getItem('current_ledger'),
			"AMotivationGroupCode":group_id
			};
	api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', x).then(function (data) {
		parsed = JSON.parse(data.data.d);
		let searched = null;
		for (moti of parsed.result.AMotivationDetail) {
			if (moti.a_motivation_group_code_c == group_id && moti.a_motivation_detail_code_c == detail_id) {
				searched = moti;
				break;
			}
		}
		if (searched == null) {
			return alert('ERROR');
		}

		searched['a_account_name_c'] = searched['a_account_code_c'];
		searched['a_cost_center_name_c'] = searched['a_cost_centre_code_c'];

		let tpl_motivation = format_tpl( $('[phantom] .tpl_edit_motivation').clone(), searched );

		$('#modal_space').html(tpl_motivation);
		tpl_motivation.find('[action]').val('update');
		tpl_motivation.modal('show');

	})
}

function save_edit_group(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let mode = obj.find('[action]').val();

	// extract information from a jquery object
	let payload = translate_to_server( extract_data(obj) );
	payload['action'] = mode;
	api.post('serverMFinance.asmx/TGiftSetupWebConnector_MaintainMotivationGroups', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			$('#modal_space .modal').modal('hide');
			updateGroup(payload['AMotivationGroupCode']);
		} else {
			for (msg of parsed.AVerificationResult) {
				display_message(i18next.t(msg.code), "fail");
			}
		}
	});
}

function save_edit_detail(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let mode = obj.find('[action]').val();

	// extract information from a jquery object
	let payload = translate_to_server( extract_data(obj) );
	payload['action'] = mode;

	api.post('serverMFinance.asmx/TGiftSetupWebConnector_MaintainMotivationDetails', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			$('#modal_space .modal').modal('hide');
			updateGroup(payload['AMotivationGroupCode']);
		} else {
			for (msg of parsed.AVerificationResult) {
				display_message(i18next.t(msg.code), "fail");
			}
			display_message(i18next.t('forms.notsaved'), "fail");
		}
	});
}


function delete_group(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let payload = translate_to_server( extract_data(obj) );
	payload['action'] = 'delete';

	let s = confirm( i18next.t('Motivations.ask_delete_group') );
	if (!s) {return}

	api.post('serverMFinance.asmx/TGiftSetupWebConnector_MaintainMotivationGroups', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.deleted'), "success");
			$('#modal_space .modal').modal('hide');
			display_list();
		} else {
			for (msg of parsed.AVerificationResult) {
				display_message(i18next.t(msg.code), "fail");
			}
			display_message(i18next.t('forms.notdeleted'), "fail");
		}
	});
}

function delete_motivation(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let payload = translate_to_server( extract_data(obj) );
	payload['action'] = 'delete';

	let s = confirm( i18next.t('Motivations.ask_delete_detail') );
	if (!s) {return}

	api.post('serverMFinance.asmx/TGiftSetupWebConnector_MaintainMotivationDetails', payload).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.deleted'), "success");
			$('#modal_space .modal').modal('hide');
			display_list();
		} else {
			for (msg of parsed.AVerificationResult) {
				display_message(i18next.t(msg.code), "fail");
			}
			display_message(i18next.t('forms.notdeleted'), "fail");
		}
	});
}
