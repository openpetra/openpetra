//
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

class Auth {
	constructor() {
	}

	login(username, password) {
		// login to server, open a session
		api.post('serverSessionManager.asmx/Login', {
				username: username,
				password: password
			})
			.then(function(response) {
				var result = JSON.parse(response.data.d);
				if (result.resultcode == "eLoginSucceeded") {
					window.localStorage.setItem('username', username);
					window.localStorage.setItem('authenticated', Date.now());
					window.localStorage.setItem('mustchangepassword', result.mustchangepassword);

					if (result.mustchangepassword === true) {
						alert(i18next.t("ChangePassword.immediately"));
						window.location.replace('/Settings/ChangePassword');
					} else {
						window.location.reload();
					}
				} else {
					alert(i18next.t("login.failedlogin"));
				}
			})
			.catch(function(error) {
				console.log(error);
			});
	}

	logout() {
		api.post('serverSessionManager.asmx/Logout')
			.then(function(response) {
				if (true) {
					// we assume we logged off, not depending on the result
					window.localStorage.setItem('username', '');
					window.localStorage.setItem('authenticated', 0);
					window.location.reload();
				}
			})
			.catch(function(error) {
				console.log(error);
				// we assume we logged off, not depending on the result
				window.localStorage.setItem('username', '');
				window.localStorage.setItem('authenticated', 0);
				window.location.reload();
			});
	}

	checkAuth(fnNonUser, fnAuthenticatedUser) {
		var lastAuthCheck = window.localStorage.getItem('authenticated');

		// what if the session timed out on the server?
		// how long ago was the last auth check? check every 5 minutes
		if (lastAuthCheck != null && lastAuthCheck != 0 && ((Date.now() - lastAuthCheck) / 1000 / 60 <= 5)) {
			if (window.localStorage.getItem('mustchangepassword') === true) {
				alert(i18next.t("ChangePassword.immediately"));
				window.location.replace('/Settings/ChangePassword');
			}

			fnAuthenticatedUser();
		} else {
			api.post('serverSessionManager.asmx/IsUserLoggedIn', {})
				.then(function(response) {
					var result = JSON.parse(response.data.d);
					if (result.resultcode == "success") {
						window.localStorage.setItem('authenticated', new Date());
						fnAuthenticatedUser();
					} else {
						if (lastAuthCheck != null && lastAuthCheck != 0) {
							console.log("session has expired");
							window.localStorage.setItem('username', '');
							window.localStorage.setItem('authenticated', 0);
							window.location.reload();
						}
						fnNonUser();
					}
				})
				.catch(function(error) {
					console.log(error);
				});
		}
	}
}
