// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2017-2018 by TBits.net
// Copyright 2019-2024 by SolidCharity.com
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
import tpl from '../../lib/tpl.js'
import api from '../../lib/ajax.js'
import utils from '../../lib/utils.js'
import modal from '../../lib/modal.js'

class MaintainUsers {

	Ready() {
		this.display_list();

		if (window.location.href.endsWith('?NewUser')) {
			this.open_new();
		}
	}

	display_list() {
		let self = this
		api.post('serverMSysMan.asmx/TMaintenanceWebConnector_LoadUsersAndModulePermissions', {}).then(function (data) {
			data = JSON.parse(data.data.d);
			// on reload, clear content
			$('#browse_container').html('');
			for (var item of data.result.SUser) {
				// format a user for every entry
				self.format_item(self, item, data.result);
			}
			tpl.format_chk();

			$('#btnNew').on('click', function() { self.open_new(self) })
		})
	}

	format_item(self, item, api_result) {
		var permissions = "";
		api_result.SUserModuleAccessPermission.forEach(function(permissionsRow) {
			if (item['s_user_id_c'] == permissionsRow['s_user_id_c'] && permissionsRow['s_can_access_l'] == true) {
				permissions += permissionsRow['s_module_id_c'] + " ";
			}
		});

		item['permissions'] = permissions;

		let row = tpl.format_tpl($('[phantom] .tpl_row').clone(), item);
		let view = tpl.format_tpl($('[phantom] .tpl_view').clone(), item);
		row.find('.collapse_col').append(view);
		row.find('.btnEditUser').on('click', function() { self.open_edit(self, $(this).attr("objid")) })
		$('#browse_container').append(row);
	}

	open_detail(obj) {
		obj = $(obj);
		if (obj.find('.collapse').is(':visible') ) {
			$('.tpl_row .collapse').collapse('hide');
			return;
		}
		$('.tpl_row .collapse').collapse('hide');
		obj.find('.collapse').collapse('show')
	}

	initButtons(self, m) {
		m.find('#btnSaveEntry').on('click', function() { self.save_entry(self, $(this)) })
		m.find('#btnClose').on('click', function() { modal.CloseModal($(this)) })
	}

	open_new(self) {
		if (!modal.allow_modal()) {return}
		api.post('serverMSysMan.asmx/TMaintenanceWebConnector_CreateUserWithInitialPermissions', {} ).then(function (data) {

			let parsed = JSON.parse(data.data.d);
			data = parsed.result;

			let s_user_id_c = data['s_user_id_c']
			let m = tpl.format_tpl($('[phantom] .tpl_edit').clone(), data.SUser[0]);
			let g = m[0].outerHTML;
			g = g.replaceAll('name="s_user_id_c" readonly', 'name="s_user_id_c"');
			m = $(g);

			let user_permissions = [];
			for (let p of data.SUserModuleAccessPermission) {
				if (p.s_can_access_l) {
					user_permissions.push(p.s_module_id_c);
				}
			}

			// generated fields
			m = self.load_modules(parsed.result.SModule, user_permissions, m);

			let myModal = modal.ShowModal('edituser' + s_user_id_c, m);
			tpl.update_requireClass(myModal, "adduser");
			self.initButtons(self, myModal)

			// set the language of the current user as default language for the new user
			$('#modal_space .modal #language_code_id').val(i18next.language.toUpperCase());
		});
	}

	open_edit(self, s_user_id_c) {
		if (!modal.allow_modal()) {return}
		let r = {'AUserId': s_user_id_c}
		api.post('serverMSysMan.asmx/TMaintenanceWebConnector_LoadUserAndModulePermissions', r ).then(function (data) {

			let parsed = JSON.parse(data.data.d);
			data = parsed.result;

			let m = tpl.format_tpl($('[phantom] .tpl_edit').clone(), data.SUser[0]);

			let user_permissions = [];
			for (let p of data.SUserModuleAccessPermission) {
				if (p.s_can_access_l) {
					user_permissions.push(p.s_module_id_c);
				}
			}

			// generated fields
			m = self.load_modules(parsed.result.SModule, user_permissions, m);

			let myModal = modal.ShowModal('edituser' + s_user_id_c, m);
			tpl.update_requireClass(myModal, "edituser");
			self.initButtons(self, myModal)
		});
	}

	save_entry(self, obj_modal) {
		let obj = $(obj_modal).closest('.modal');

		// extract information from a jquery object
		let x = tpl.extract_data(obj);

		let applied_perm = [];

		obj.find('.permissions').find('.tpl_check').each(function (i, obj) {
			obj = $(obj);
			if (obj.find('input').is(':checked')) {
				applied_perm.push(obj.find('data').attr('value'));
			}
		})

		x['AModulePermissions'] = applied_perm;
		let args = utils.translate_to_server(x);

		let NewUser = false;
		if (args['AModificationId'] == '') {
			args['AModificationId'] = 0;
			NewUser = true;
		}

		api.post('serverMSysMan.asmx/TMaintenanceWebConnector_SaveUserAndModulePermissions', args).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			if (!parsed.result) {
				// TODO we need an error code from the server, to display a meaningful error message here
				return utils.display_error(parsed.AVerificationResult);
			} else {
				$('.modal').modal('hide');
				utils.display_message(i18next.t('forms.saved'), "success");
				self.display_list();
			}
		})
	}

	// used to load all available modules and set checkbox if needed
	load_modules(all_modules, selected_modules, obj) {
		let p = $('<div class="container">');
		for (var module of all_modules) {
			let pe = $('[phantom] .tpl_check').clone();
			pe.find('data').attr('value', module['s_module_id_c']);
			pe.find('span').text(module['s_module_name_c']);

			if ($.inArray(module['s_module_id_c'], selected_modules) > -1) {
				pe.find('input').attr('checked', true);
			}
			p.append(pe);
		}
		obj.find('.permissions').html(p);
		return obj;
	}
}

export default new MaintainUsers();