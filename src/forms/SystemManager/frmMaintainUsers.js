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

$(function() {
	api.post('serverMSysMan.asmx/TMaintenanceWebConnector_LoadUsersAndModulePermissions', {})
	.then(function(response) {
		if (response.data == null) {
			console.log("error: " + response);
			return;
		}
		var result = JSON.parse(response.data.d);
		if (result.result == "false") {
			console.log("problem loading users");
		} else {
			parent = $("#tpl_row").parent();
			result.result.SUser.forEach(function(element) {
				newrow = $( "#tpl_row" ).clone().
					prop('id', 'row' + element.s_user_id_c).
					appendTo( parent );
				html = newrow.html();
				html = html.replace(new RegExp('{val_userid}',"g"), element.s_user_id_c);
				newrow.html(html);	
				newrow.show();
			});
		}
	})
});
