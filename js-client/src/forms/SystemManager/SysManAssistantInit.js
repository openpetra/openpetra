// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2020 by SolidCharity.com
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

var last_opened_entry_data = {};

$('document').ready(function () {
	open_edit();
});

function open_edit() {
	var r = {AClientLanguage: currentLng()};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we don't have to load it, if we only search
	api.post('serverMSysMan.asmx/TSettingsWebConnector_GetDefaultsForFirstSetup', r).then(function (data) {
		parsed = JSON.parse(data.data.d);
		display_screen(parsed);
	})
}

function display_screen(parsed) {
	if (!modal.allow_modal()) {return}
	// make a deep copy of the server data and set it as a global var.
	last_opened_entry_data = $.extend(true, {}, parsed);
	let m = $('[phantom] .tpl_edit').clone();

	insertData(m, parsed);

	$('#modal_space').html(m);
	$('#modal_space .modal').modal('show');
}

function reloadUsersPage() {
	window.location.replace('/SystemManager/MaintainUsers');
}

function save_entry(obj_modal) {
	let obj = $(obj_modal).closest('.modal');

	// extract information from a jquery object
	let param = tpl.extract_data(obj);
	param['AInitialModulePermissions'] = param['AInitialModulePermissions'].split(",");

	api.post('serverMSysMan.asmx/TSettingsWebConnector_RunFirstSetup', param).then(function (data) {
		parsed = JSON.parse(data.data.d);
		if (!parsed.result) {
			return utils.display_error(parsed.AVerificationResult);
		} else {
			modal.CloseModal(obj);
			utils.display_message(i18next.t('forms.saved'), "success");
			setTimeout(reloadUsersPage, 5000);
		}
	})
}
