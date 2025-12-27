// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2020-2025 by SolidCharity.com
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
import i18n from '../../lib/i18n.js';
import tpl from '../../lib/tpl.js'
import api from '../../lib/ajax.js'
import utils from '../../lib/utils.js'
import modal from '../../lib/modal.js'

class SysManAssistantInit {

	constructor() {
		this.last_opened_entry_data = {};
	}

	Ready() {
		let self = this;
		self.open_edit();
	}

	open_edit() {
		let self = this;
		var r = {AClientLanguage: i18n.currentLng()};
		// on open of a edit modal, we get new data,
		// so everything is up to date and we don't have to load it, if we only search
		api.post('serverMSysMan.asmx/TSettingsWebConnector_GetDefaultsForFirstSetup', r).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			self.display_screen(parsed);
		})
	}

	display_screen(parsed) {
		let self = this;
		if (!modal.allow_modal()) {return}
		// make a deep copy of the server data and set it as a global var.
		self.last_opened_entry_data = $.extend(true, {}, parsed);
		let m = $('[phantom] .tpl_edit').clone();

		tpl.insertData(m, parsed);

		$('#modal_space').html(m);
		$('#modal_space .modal').modal('show');
		m.find('#btnSave').on('click', function() {self.save_entry(this)});
		m.find('#btnClose').on('click', function() {modal.CloseModal(this)});
	}

	reloadUsersPage() {
		window.location.replace('/SystemManager/MaintainUsers');
	}

	save_entry(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');

		// extract information from a jquery object
		let param = tpl.extract_data(obj);
		param['AInitialModulePermissions'] = param['AInitialModulePermissions'].split(",");

		api.post('serverMSysMan.asmx/TSettingsWebConnector_RunFirstSetup', param).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			if (!parsed.result) {
				return utils.display_error(parsed.AVerificationResult);
			} else {
				modal.CloseModal(obj);
				utils.display_message(i18next.t('forms.saved'), "success");
				setTimeout(function() { self.reloadUsersPage() }, 5000);
			}
		})
	}
}

export default new SysManAssistantInit();