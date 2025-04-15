//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//	Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2017-2018 by TBits.net
// Copyright 2019-2025 by SolidCharity.com
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

import api from './ajax.js';
import utils from './utils.js';
import i18next from 'i18next';
import nav from './navigation.js';

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

	nav.loadNavigation();
	$("#logout").click(function(e) {
			var stateObj = { "logout": "done" };
			history.pushState(stateObj, "OpenPetra", "/");
			e.preventDefault();
			auth.logout();
		});
}

import Auth from './auth.js';
var auth = new Auth();

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
			let pwd1=$("#txtPasswordReset1").val();
			let pwd2=$("#txtPasswordReset2").val();
			if (pwd1 != pwd2) {
				utils.display_message(i18next.t('login.passwords_dont_match'), "fail");
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
					utils.display_message(i18next.t('login.successSignUpConfirmed'), "success");
					setTimeout(reloadMainPage, 5000);
				} else {
					utils.display_message(i18next.t('login.errorSignUpConfirmed'), "fail");
				}
			})
			.catch(function(error) {
				utils.display_message(i18next.t('login.errorSignUpConfirmed'), "fail");
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
			$("#hintdemo").show();
			$("#hintdemo2").show();
		}
		$("#btnLogin").click(function(e) {
			e.preventDefault();
			let user=$("#txtEmail").val();
			let pwd=$("#txtPassword").val();
			auth.login(user, pwd);
		});
		$("#btnRequestNewPassword").click(function(e) {
			e.preventDefault();
			requestNewPassword();
		});
		$("#btnSignUp").click(function(e) {
			e.preventDefault();
			requestSignUp();
		});
	}
	const urlParams = new URLSearchParams(window.location.search);
	if (urlParams.get('action') == 'resetpwd') {
		requestNewPassword(urlParams.get('email'));
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

function requestNewPassword(emailAddress) {
	$("#login").hide();
	$("#txtEmailRequestPwd").val(emailAddress);
	$("#reqNewPwd").show();
	$("#btnReqNewPwd").click(function(e) {
		e.preventDefault();
		let user=$("#txtEmailRequestPwd").val();
		if (user == "" || user.indexOf('@') == -1) {
			utils.display_message(i18next.t('login.enterValidEmail'), "fail");
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
		let userEmail=$("#txtEmailSignUp").val();
		let firstName=$("#txtFirstName").val();
		let lastName=$("#txtLastName").val();
		let pwd1=$("#txtPasswordSignUp1").val();
		let pwd2=$("#txtPasswordSignUp2").val();
		if ((pwd1 != pwd2) || (pwd1 == '')) {
			utils.display_message(i18next.t('login.passwords_dont_match'), "fail");
		} else if (userEmail == "" || userEmail.indexOf('@') == -1) {
			utils.display_message(i18next.t('login.enterValidEmail'), "fail");
		} else {
			auth.signUp(userEmail, firstName, lastName, pwd1);
		}

	});
}
