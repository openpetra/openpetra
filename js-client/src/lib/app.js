//
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

// call the server regularly to keep the connection open
function keepConnection() {
	api.post('serverSessionManager.asmx/PollClientTasks')
		.then(function(response) {
			// console.log("keepConnection call succeeded");
		})
		.catch(function(error) {
			console.log(error);
		});

	// call every 30 seconds
	setTimeout(keepConnection, 30000);
}

function loadNavigation() {
	// wait until translations have been loaded
	if (i18next.t('navigation.Partner_label') == 'navigation.Partner_label') {
		setTimeout(loadNavigation, 50);
		return;
	}

	nav = new Navigation();
	nav.loadNavigation();
	$("#logout").click(function(e) {
			var stateObj = { "logout": "done" };
			history.pushState(stateObj, "OpenPetra", "/");
			e.preventDefault();
			auth.logout();
		});
}

auth = new Auth();

function resetPassword() {
	var url = new URL(window.location.href);
	var ResetPasswordToken = url.searchParams.get("ResetPasswordToken");
	var UserId = url.searchParams.get("UserId");
	if (ResetPasswordToken != null) {
		// delete our session to be logged out
		window.localStorage.setItem('username', '');
		window.localStorage.setItem('authenticated', 0);

		$("#setNewPwd").show();
		$("#btnSetNewPwd").click(function(e) {
			e.preventDefault();
			pwd1=$("#txtPassword1").val();
			pwd2=$("#txtPassword2").val();
			if (pwd1 != pwd2) {
				display_message(i18next.t('login.passwords_dont_match'), "fail");
			} else {
				auth.setNewPassword(UserId, ResetPasswordToken, pwd1);
			}
		});
		return true;
	}
	return false;
}

function reloadMainPage() {
	window.location.replace('/');
}

function selfSignUp() {
	var url = new URL(window.location.href);
	var SelfSignupPasswordToken = url.searchParams.get("SelfSignupPasswordToken");
	var UserId = url.searchParams.get("UserId");
	if (SelfSignupPasswordToken != null) {
		api.post('serverSessionManager.asmx/SignUpSelfServiceConfirm',
			{AUserID: UserId,
				AToken: SelfSignupPasswordToken
			})
			.then(function(response) {
				var result = JSON.parse(response.data.d);
				if (result == true) {
					display_message(i18next.t('login.successSignUpConfirmed'), "success");
					setTimeout(reloadMainPage, 5000);
				} else {
					display_message(i18next.t('login.errorSignUpConfirmed'), "fail");
				}
			})
			.catch(function(error) {
				display_message(i18next.t('login.errorSignUpConfirmed'), "fail");
			});
		return true;
	}
	return false;
}

auth.checkAuth(function(selfsignupEnabled) {
	$("#loading").hide();
	$(window).scrollTop(0);
	if (!resetPassword() && !selfSignUp()) {
		$("#login").show();
		if (selfsignupEnabled) $("#btnSignUp").show();
		if (window.location.hostname.indexOf("demo.") !== -1)
		{
			$("#txtEmail").val("demo");
			$("#txtPassword").val("demo");
		}
		$("#btnLogin").click(function(e) {
			e.preventDefault();
			user=$("#txtEmail").val();
			pwd=$("#txtPassword").val();
			auth.login(user, pwd);
		});
	}
}, function () {
	setTimeout(keepConnection, 5000);
	$("#loading").hide();

	if (!resetPassword() && !selfSignUp()) {
		// load the navigation bars now.
		// we don't want the navigation bars displayed if the user is not logged in
		$("#main").show();

		setTimeout(loadNavigation, 50);
	}
});

function requestNewPassword() {
	$("#login").hide();
	$("#reqNewPwd").show();
	$("#btnReqNewPwd").click(function(e) {
		e.preventDefault();
		user=$("#txtEmailRequestPwd").val();
		if (user == "" || user.indexOf('@') == -1) {
			display_message(i18next.t('login.enterValidEmail'), "fail");
		} else {
			auth.requestNewPassword(user);
		}

	});
}

function requestSignUp() {
	$("#login").hide();
	$("#signUp").show();
	$("#btnSignUpSubmit").click(function(e) {
		e.preventDefault();
		userEmail=$("#txtEmailSignUp").val();
		firstName=$("#txtFirstName").val();
		lastName=$("#txtLastName").val();
		pwd1=$("#txtPassword1").val();
		pwd2=$("#txtPassword2").val();
		if ((pwd1 != pwd2) || (pwd1 == '')) {
			display_message(i18next.t('login.passwords_dont_match'), "fail");
		} else if (userEmail == "" || userEmail.indexOf('@') == -1) {
			display_message(i18next.t('login.enterValidEmail'), "fail");
		} else {
			auth.signUp(userEmail, firstName, lastName, pwd1);
		}

	});
}
