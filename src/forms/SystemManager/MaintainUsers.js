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

class MaintainUsersForm extends JSForm {
	constructor() {
		super('MaintainUsers',
			'serverMSysMan.asmx/TMaintenanceWebConnector_LoadUsersAndModulePermissions',
			{});
		super.initEvents();
		super.search();
	}

	getMainTableFromResult(result) {
		return result.result.SUser;
	}
	getKeyFromRow(row) {
		return row['s_user_id_c'];
	}

	insertRowValues(html, tablename, row) {
		html = super.insertRowValues(html, tablename, row);
		if (html.indexOf("{val_permissions}") > -1) {
			// calculate permissions for this user
			var permissions = "";
			self.viewData.result.SUserModuleAccessPermission.forEach(function(permissionsRow) {
				if (row['s_user_id_c'] == permissionsRow['s_user_id_c'] && permissionsRow['s_can_access_l'] == true) {
					permissions += permissionsRow['s_module_id_c'] + " ";
				}
			});
			html = html.replace(new RegExp('{val_permissions}',"g"), permissions);
		}
		return html;
	}

	insertEditDataIntoDialogDerived(self, dialogname, key, html) {
		return self.insertEditDataIntoDialog(self, key, html);
	}
}

$(function() {
	var form = new MaintainUsersForm();
});

