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

$("#btnSubmitPassword").click(function(e) {
		e.preventDefault();

		if ($("#newpwd").val() != $("#newpwd2").val()) {
			alert(i18next.t("ChangePassword.dontmatch"));
			return;
		}

		api.post('serverMSysMan.asmx/TMaintenanceWebConnector_SetUserPassword2', {
				AUserID: window.localStorage.getItem('username'),
				ANewPassword: $("#newpwd").val(),
				ACurrentPassword: $("#curpwd").val(),
				APasswordNeedsChanged: true,
				AClientComputerName: "",
				AClientIPAddress: ""
			})
			.then(function(response) {

				var result = JSON.parse(response.data.d);
				if (result.result == false) {
					if (result.AVerification[0].code == "SYS.00002V")
						alert(i18next.t("ChangePassword.criteria"));
					else if (result.AVerification[0].code == "SYS.00003V")
						alert(i18next.t("ChangePassword.mustbedifferent"));
					else if (result.AVerification[0].code == "ChangePassword.CurrentPwdWrong")
						alert(i18next.t("ChangePassword.wrong"));
					else
						alert(i18next.t("ChangePassword.unknown") + " " + result.AVerification[0].code);
				} else {
					alert(i18next.t("ChangePassword.success"));
					window.localStorage.setItem('mustchangepassword', false);
					window.location.replace('/');
				}
		});
	});
