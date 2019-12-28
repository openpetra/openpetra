//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//       CJ <cj@tbits.net>
//
// Copyright 2017-2019 by TBits.net
// Coypright 2019 by SolidCharity.com
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

class Navigation {
	constructor() {
		this.debug = 0;
		this.develop = 1;
		// will be replaced by the build script for the release
		this.currentrelease = "CURRENTRELEASE";
		this.classesLoaded = [];
		$(window).scrollTop(0);

		this.module = null;
		this.submodule = null;
		this.getCurrentModule();
	}

	OpenForm(name, title, pushState=true, parameter="")
	{
		if (this.debug) {
			console.log("OpenForm: " + name + " title: " + title);
		}

		// fetch screen content from the server
		var navPage = this.IsNavigationPage(name.replace(/_/g, '/'));
		if (navPage == null) {
			var refresh = "";
			self = this;
			if (self.develop) {
				refresh = "?" + Date.now();
			} else {
				refresh = "?" + self.currentrelease;
			}
			$.ajaxSetup({
				// $.getScript should not use its own timestamp
				cache: true
			});

			axios.get("/src/forms/" + name + ".html" + refresh)
				.then(function(response) {
					var content = response.data;
					content = replaceAll(content, ".js", ".js" + refresh);
					content = translate(content, name.substring(name.lastIndexOf('/')+1));
					$("#containerIFrames").html(content);
					$.getScript("/src/forms/" + name + '.js' + refresh);
			});
		}
		else // fetch navigation page
		{
			this.loadNavigationPage(name);
		}

		var stateObj = { name: name, title: title, parameter: parameter };
		var newUrl = name.replace(/_/g, '/');
		if (parameter != '') newUrl += "?" + parameter;
		if (newUrl == "Home") { newUrl = ''; }
		newUrl = window.location.protocol + '//' + window.location.hostname + '/' + newUrl;
		if (window.location.protocol + '//' + window.location.hostname + '/' + window.location.pathname != newUrl && pushState) {
			if (this.debug) {
				console.log("history.pushState " + newUrl);
			}
			history.pushState(stateObj, title, newUrl);
		}
		var windowTitle = "OpenPetra: " + title;
		if (document.title != windowTitle) {
			document.title = windowTitle;
		}
		$(window).scrollTop(0);
	};

	getCurrentModule() {
		var path = window.location.pathname.substring(1);
		path = path.split('/')

		if (path.length > 0 && path[0] != '') {
			this.module = path[0];
		}
		if (path.length == 2) {
			this.module = path[0];
			this.submodule = path[1];
			if (this.module == '') {
				this.module = path[1];
				this.submodule = '';
			}
		}

		return [this.module, this.submodule];
	}

	IsNavigationPage(path) {
		path = path.split('/')

		var currentPage = null;
		var caption = null;

		// highlight the icon in the top navbar
		if (this.module != null) {
			$('li.nav-item a').removeClass('active');
			$('li.nav-item a[id=mnu'+this.module+']').addClass('active');
		}

		if (path.length == 2) {
			var navigation = JSON.parse(window.localStorage.getItem('navigation'));

			if (this.module in navigation) {
				if (this.submodule in navigation[this.module].items) {
					currentPage = window.location.pathname.substring(1);
					caption = navigation[this.module].caption + ": " + navigation[this.module].items[this.submodule].caption;
				}
			}
		}

		if (currentPage == null) {
			return null;
		}

		return [currentPage, caption];
	}

	// this is called on a reload of the page, we want to jump to the right location, depending on the URL
	UpdateLocation() {
		var currentPage = null;
		var caption = null;

		if (this.debug) {
			console.log("called UpdateLocation " + window.location.pathname);
		}

		// check if this is a link to a navigation page
		if (currentPage == null && window.location.pathname.length > 1) {
			var path = window.location.pathname.substring(1);

			currentPage = this.IsNavigationPage(path);
			if (currentPage != null) {
				caption = currentPage[1];
				currentPage = currentPage[0];
			}

			path = path.split('/')
			if (path.length >= 2 && path[0] != "Settings") {
				// open left navigation sidebar at the right position
				$('a[href="#mnuLst' + path[0] + '"]').click();
			}

			if (currentPage != null) {
				this.OpenForm(currentPage, caption);
			}

		}

		// check if this is a form, add frm to path
		if (currentPage == null && window.location.pathname.length > 1) {
			// load specific frames by URL
			var path = window.location.pathname;
			var frmName = path.substring(path.lastIndexOf('/')+1);
			this.OpenForm(path, i18next.t("navigation."+frmName+"_label"));
			currentPage = frmName;
		}

		if (currentPage == null) {
			// load home page or Dashboard
			this.OpenForm("Home", i18next.t("navigation.home"));
		}
	}

	AddModuleToTopBar(name, title, icon, enabled)
	{
		if (enabled) {
			$("#ModuleNavBar").append("<li class='nav-item'><a id='mnu" + name + "' class='nav-link top-icon href='#' title='" + title + "'><i class='fas fa-" + icon + "'></i><span class='topnav-icontext'>" + title + "</span></a></li>");
		}
	}

	AddMenuItemHandler(mnuItem, frmName, title, parameter) {
		var self = this;
		$('#' + mnuItem).click(function(event) {
			event.preventDefault();

			self.OpenForm(frmName, title, true, parameter);

			// hide the menu if we are on mobile screen (< 768 px width)
			if ($(document).width() < 768) {
				$(this).parent().collapse('toggle');
			}

		});
	}

	AddMenuItem(parent, name, title, tabtitle, icon)
	{
		$("#LeftNavigation").append("<a href='#" + name + "' class='list-group-item d-inline-block collapsed' data-toggle='collapse' data-parent='#sidebar' aria-expanded='false'> <i class='fas fa-" + icon + "'></i>  <span class='d-none d-md-inline'> " + title + "</span> </a><div class='collapse' id='mnuLst" + name + "'></div></a>");
		this.AddMenuItemHandler(name, name, tabtitle);
	}

	// eg. SystemManager/Users/MaintainUsers is a link directly to a form, not a navigation page
	AddMenuItemForm(folderid, name, parent, item, title, tabtitle, icon)
	{
		var url = folderid + "/" + parent.name + "/" + item.form;
		if (item.hasOwnProperty('path')) {
			url = item.path + "/" + item.form;
		}
		$("#LeftNavigation").append("<a href='/" + url + "' class='sidebar-item' id='" + name + "'><i class='fas fa-" + icon + " icon-invisible'></i> " + title +"</a>");
		this.AddMenuItemHandler(name, url, tabtitle, item.action);
	}

	displayNavigationSideBarItem(folderid, parent, folder) {
		var items = parent.items;
		for (var itemid in items) {
			var item = items[itemid];
			var title = i18next.t('navigation.' + item.caption);

			if (item.hasOwnProperty('items') && item.items.count > 0) {
				$("#LeftNavigation").append("<h1 class='sidebar'>" + title + "</h2>");
				this.displayNavigationSideBarItem(folderid, item, folder);
			} else {
				if (item.form != null) {
					this.AddMenuItemForm(folderid, folderid + "_" + itemid, parent, item,
						i18next.t('navigation.' + item.caption),
						i18next.t('navigation.'+folder.caption) + ": "+ i18next.t('navigation.'+item.caption),
						folder.icon);
				} else {
					this.AddMenuItem(folderid, folderid + "_" + itemid,
						i18next.t('navigation.' + item.caption),
						i18next.t('navigation.'+folder.caption) + ": "+ i18next.t('navigation.'+item.caption),
						folder.icon);
				}
			}
		}
	}

	displayNavigationSideBar(navigation) {
		$("#LeftNavigation").html('');

		var folder = navigation[this.module];
		if (folder == null) return;

		if (folder.enabled == "false") {
			return;
		}

		var items = folder.items;
		for (var itemid in items) {
			var item = items[itemid];
			item['name'] = itemid;
			var title = i18next.t('navigation.' + item.caption);

			$("#LeftNavigation").append("<h1 class='sidebar'>" + title + "</h2>");

			this.displayNavigationSideBarItem(this.module, item, folder);
		}

	}

	displayNavigationTopBar(navigation) {
		for (var folderid in navigation) {

			var folder = navigation[folderid];
			this.AddModuleToTopBar(folderid, i18next.t('navigation.'+folder.caption), folder.icon, folder.enabled != "false");
		}

		// link the items in the top menu
		this.AddMenuItemHandler('mnuChangePassword', "Settings/ChangePassword", i18next.t("navigation.change_password"));
		this.AddMenuItemHandler('mnuChangeLanguage', "Settings/ChangeLanguage", i18next.t("navigation.change_language"));
		this.AddMenuItemHandler('mnuHome', "Home", i18next.t("navigation.home"));
		this.AddMenuItemHandler('mnuHelpAbout', "About", i18next.t("navigation.about"));
		this.AddMenuItemHandler('mnuHelpReleaseNotes', "ReleaseNotes", i18next.t("navigation.releasenotes"));

		this.AddMenuItemHandler('mnuPartner', "Partner", i18next.t("navigation.Partner_label"));
		this.AddMenuItemHandler('mnuFinance', "Finance", i18next.t("navigation.Finance_label"));
		this.AddMenuItemHandler('mnuSystemManager', "SystemManager", i18next.t("navigation.SystemManager_label"));

		var self = this;

		// select module in top bar: make it active, deactivate other modules
		$('#ModuleNavBar li.nav-item a').click(function(e) {
			$('#ModuleNavBar li.nav-item a').removeClass('active');
			$(this).addClass('active');
			self.getCurrentModule();
			self.displayNavigationSideBar(navigation);
		});

		this.UpdateLocation();
	}

	loadNavigationPage(navpage) {
		// TODO: store pages per user? see window.localStorage?
		self = this;
		navpage = navpage.replace(/\//g, '_')
		// load navigation page from UINavigation.yml
		api.post('serverSessionManager.asmx/LoadNavigationPage', {
				ANavigationPage: navpage
			})
			.then(function(response) {
				var result = JSON.parse(response.data.d);
				if (result.resultcode == "success") {
					result.htmlpage = translate(result.htmlpage, "navigation");
					$("#containerIFrames").html(result.htmlpage);
				} else {
					console.log(response.data.d);
				}
			})
			.catch(function(error) {
				console.log(error);
			});
	}

	loadNavigation() {
		// TODO: caching for this user??? see window.localStorage below
		self = this;
		// load sidebar navigation from UINavigation.yml
		api.post('serverSessionManager.asmx/GetNavigationMenu', null, null)
			.then(function(response) {
				var result = JSON.parse(response.data.d);
				if (result.resultcode == "success") {
					window.localStorage.setItem('navigation', JSON.stringify(result.navigation));
					self.displayNavigationTopBar(result.navigation);
					self.displayNavigationSideBar(result.navigation);
					window.onpopstate = function(e) {
						if (e.state != null) {
							nav.OpenForm(e.state.name, e.state.title, false);
						}
					};
				} else {
					console.log(response.d);
				}
			})
			.catch(function(error) {
				console.log(error);
			});
	}
}

$('document').ready(function () {
	if (window.localStorage.getItem('username') == null || window.localStorage.getItem('username') == "") {
		return; // User is not logged in
	}

	// check for FINANCE-1 permission. else: hide the ledger selection
	permissions = window.localStorage.getItem('ModulePermissions');
	if ((permissions == null) || !(" " + permissions.replace(/\n/g, ' ') + " ").includes(" FINANCE-1 ")) {
		$('#LedgerSelection').hide();
		return;
	}

	api.post('serverMFinance.asmx/TGLSetupWebConnector_GetAvailableLedgers', {}).then(function (data) {
		data = JSON.parse(data.data.d);
		let dump = $('#ledger_select_dropdown').html('');
		current_selected_ledger = null;
		for (ledger of data.result) {
			if ( ledger.a_ledger_number_i == window.localStorage.getItem('current_ledger') || data.result.length == 1) {
				current_selected_ledger = ledger;
				if (data.result.length == 1 && window.localStorage.getItem('current_ledger') == null) {
					change_standard_ledger(current_selected_ledger.a_ledger_number_i);
				}
			}
			let z = $('<a class="dropdown-item"></a>');
			z.text(ledger.a_ledger_name_c);
			z.attr("onclick", "change_standard_ledger("+ledger.a_ledger_number_i+")");
			dump.append(z);
		}
		if (current_selected_ledger == null) {
			$('#current_ledger_field').html('<b style="color:#f88;">'+i18next.t('navigation.ledgerselect_none')+'</b>');
		} else {
			$('#current_ledger_field').text(current_selected_ledger.a_ledger_name_c);
		}
	})
});

function change_standard_ledger(ledger_id) {
	window.localStorage.setItem('current_ledger', ledger_id);
	api.post('serverMFinance.asmx/TGLSetupWebConnector_GetAvailableLedgers', {}).then(function (data) {
		data = JSON.parse(data.data.d);
		for (ledger of data.result) {
			if (ledger.a_ledger_number_i == ledger_id) {
				display_message(i18next.t("navigation.switchledger")+" "+ledger.a_ledger_name_c, "success");
				$('#current_ledger_field').text(ledger.a_ledger_name_c);
			}
		}
	})
}
