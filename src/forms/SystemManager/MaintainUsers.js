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

function display_users() {
	API_call('serverMSysMan.asmx/TMaintenanceWebConnector_LoadUsersAndModulePermissions', null, format_user);
}

function format_user(api_result) {
	for (user of api_result.result.SUser) {
		var permissions = "";
		api_result.result.SUserModuleAccessPermission.forEach(function(permissionsRow) {
			if (user['s_user_id_c'] == permissionsRow['s_user_id_c'] && permissionsRow['s_can_access_l'] == true) {
				permissions += permissionsRow['s_module_id_c'] + " ";
			}
		});

		user['permissions'] = permissions;

		let user_html = format_tpl($('#tpl_row').clone(), user);
		let user_info = format_tpl($('#tpl_view').clone(), user);
		let r = "<div class='user'>"+user_html.html()+user_info.html()+"</div>"
		$('#browse').append(r);
	}
}

function edit_entry(s_user_id_c, api_data=false) {
	r = {'AUserId': s_user_id_c};
	api.post('serverMSysMan.asmx/TMaintenanceWebConnector_LoadUserAndModulePermissions', r ).then(function (data) {
		data = JSON.parse(data.data.d).result;
		let m = format_tpl($('#tpl_edit').clone(), data.SUser[0]);

		let user_permissions = [];
		for (let p of data.SUserModuleAccessPermission) {
			user_permissions.push(p.s_module_id_c);
		}

		let p = $('<div class="container">');
		for (perm of data.SModule) {
			let pe = $('#tpl_permission').clone();
			pe.find('data').attr('value', perm['s_module_id_c']);
			pe.find('span').text(perm['s_module_name_c']);

			if ($.inArray(perm['s_module_id_c'], user_permissions) > -1) {
				pe.find('input').attr('checked', true);
			}

			p.append(pe);
		}

		m.find('.permissions').html(p)

		$('#modal_space').html(m.html())
		$('#modal_space > .modal').modal('show')
	});
}

function save_entry(s_user_id_c, entry_window) {
	let x = extract_data(entry_window);

	let perm = [];

	entry_window.find('.permissions').find('.perm').each(function (i, obj) {
		obj = $(obj);
		if (obj.find('input').is(':checked')) {
			perm.push(obj.find('data').attr('value'));
		}
	})

	x['permissions'] = perm;
	let arguments = {
		"AUserId": x['s_user_id_c'],
		"AEmailAddress": x['s_email_address_c'],
		"AFirstName": x['s_first_name_c'],
		"ALastName": x['s_last_name_c'],
		"AAccountLocked": x['s_account_locked_l'],
		"ARetired": x['s_retired_l'],
		"AModulePermissions": x['permissions'],
	}

	api.post('serverMSysMan.asmx/TMaintenanceWebConnector_SaveUserAndModulePermissions', arguments).then(function (data) {
		$('.modal').modal('hide');
		display_message(i18next.t('forms.saved'), "success");
	})
}

$('document').ready(function () {
	display_users();
});
