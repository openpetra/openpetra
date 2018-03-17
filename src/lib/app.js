//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//	   Timotheus Pokorra <tp@tbits.net>
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

auth.checkAuth(function() {
	$("#loading").hide();
	$("#login").show();
	$("#btnLogin").click(function(e) {
		e.preventDefault();
		user=$("#txtEmail").val();
		pwd=$("#txtPassword").val();
		if (user == "" && pwd == "") {
			user = "demo";
			pwd = "demo";
		}
		auth.login(user, pwd);
	});
}, function () {
	setTimeout(keepConnection, 5000);

	// load the navigation bars now.
	// we don't want the navigation bars displayed if the user is not logged in
	$("#loading").hide();
	$("#main").show();

        setTimeout(loadNavigation, 50);

});
