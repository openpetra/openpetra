// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
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

var last_requested_data = {};

$('document').ready(function () {
	display_list();
});

function display_list() {
	api.post('serverMPartner.asmx/TPartnerSetupWebConnector_LoadMemberships', {}).then(function (data) {
	  data = JSON.parse(data.data.d);
		// on reload, clear content
		$('#browse_container').html('');
		last_requested_data = data.result.PMembership;
		for (item of data.result.PMembership) {
			item['p_membership_code_c_clean'] = item['p_membership_code_c'].replace(/[^a-zA-Z0-9]+/g, '');
			format_item(item);
		}
	})
}

function format_item(item) {
	let row = format_tpl($("[phantom] .tpl_row").clone(), item);
	item['a_frequency_code_c_translated'] = i18next.t('MaintainMemberships.freq_' + item['a_frequency_code_c']);
	let view = format_tpl($("[phantom] .tpl_view").clone(), item);
	$('#browse_container').append(row);
	$('#membership'+item['p_membership_code_c_clean']).find('.collapse_col').append(view);
}

function open_detail(obj) {
	obj = $(obj);
	while(!obj[0].hasAttribute('id') || !obj[0].id.includes("membership")) {
		obj = obj.parent();
	}
	if (obj.find('.collapse').is(':visible') ) {
		$('.tpl_row .collapse').collapse('hide');
		return;
	}
	$('.tpl_row .collapse').collapse('hide');
	obj.find('.collapse').collapse('show')
}

function open_edit(sub_id) {
	if (!allow_modal()) {return}
	let z = null;
	for (sub of last_requested_data) {
		if (sub.p_membership_code_c == sub_id) {
			z = sub;
			break;
		}
	}

	var f = format_tpl( $('[phantom] .tpl_edit').clone(), z);
	$('#modal_space').html(f);
	$('#modal_space .modal').modal('show');
}

function open_new() {
	if (!allow_modal()) {return}
	let n_ = $('[phantom] .tpl_new').clone();
	$('#modal_space').html(n_);
	$('#modal_space .modal').modal('show');
}

function save_new(obj_modal) {
	if (!allow_modal()) {return}
	let se = $('#modal_space .modal').modal('show');
	let request = translate_to_server(extract_data(se));

	request['action'] = 'create';
	if (request['AMembershipHoursService'] == '') {
		request['AMembershipHoursService'] = 0;
	}
	if (request['AMembershipFee'] == '') {
		request['AMembershipFee'] = 0;
	}

	api.post("serverMPartner.asmx/TPartnerSetupWebConnector_MaintainMemberships", request).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			utils.display_message(i18next.t('MaintainMemberships.confirm_create'), "success");
			CloseModal(se);
			display_list();
		}
		else if (parsed.result == false) {
			utils.display_error(parsed.AVerificationResult);
		}
	});

}

function save_entry(update) {
	let modalspace = $(update).closest('.modal');
	let request = translate_to_server(extract_data(modalspace));

	request['action'] = 'update';
	if (request['AMembershipHoursService'] == '') {
		request['AMembershipHoursService'] = 0;
	}
	if (request['AMembershipFee'] == '') {
		request['AMembershipFee'] = 0;
	}

	api.post("serverMPartner.asmx/TPartnerSetupWebConnector_MaintainMemberships", request).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			utils.display_message(i18next.t('MaintainMemberships.confirm_edit'), "success");
			CloseModal(modalspace);
			display_list();
		}
		else if (parsed.result == false) {
			utils.display_error(parsed.AVerificationResult);
		}
	});
}

function delete_entry(d) {
	let raw = $(d).closest('.modal');
	let request = translate_to_server(extract_data(raw));

	request['action'] = 'delete';

	let s = confirm( i18next.t('MaintainMemberships.ask_delete') );
	if (!s) {return}

	api.post("serverMPartner.asmx/TPartnerSetupWebConnector_MaintainMemberships", request).then(function (result) {
		parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			utils.display_message(i18next.t('forms.deleted'), 'success');
			raw.modal('hide');
			display_list();
		} else {
			utils.display_error( i18next.t('forms.notdeleted') );
		}
	});
}
