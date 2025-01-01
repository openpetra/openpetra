//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//	   Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//	   CJ
//
// Copyright 2017-2019 by TBits.net
// Coypright 2019-2025 by SolidCharity.com
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

import axios from 'axios';
import i18next from 'i18next';
import api from './ajax.js';
import i18n from './i18n.js';
import utils from './utils.js';

import releasenotes from '../forms/ReleaseNotes.js';
import about from '../forms/About.js';
import importpartners from '../forms/Partner/Partners/ImportPartners.js';
import maintainpartners from '../forms/Partner/Partners/MaintainPartners.js';
import maintaintypes from '../forms/Partner/Setup/Types/MaintainTypes.js';
import maintainconsentchannels from '../forms/Partner/Setup/MaintainConsentChannels.js';
import maintainconsentpurposes from '../forms/Partner/Setup/MaintainConsentPurposes.js';
import maintainusers from '../forms/SystemManager/MaintainUsers.js';

class Navigation {
	constructor() {
		this.debug = 0;
		this.develop = 1;
		// will be replaced by the build script for the release
		this.currentrelease = "CURRENTRELEASE";
		this.formsLoaded = {
			'ReleaseNotes': releasenotes,
			'About': about,
			'MaintainPartners': maintainpartners,
			'ImportPartners': importpartners,
			'MaintainTypes': maintaintypes,
			'MaintainConsentChannels': maintainconsentchannels,
			'MaintainConsentPurposes': maintainconsentpurposes,
			'MaintainUsers': maintainusers,
		};
		$(window).scrollTop(0);

		this.module = null;
		this.submodule = null;
		this.getCurrentModule(window.location.pathname);
	}

	// some javascript files (eg. MaintainPartners) can only be loaded once, due to global variables. They must have a <formname>_Ready() function.
	async LoadJavascript(name, formname, refresh)
	{
		if (formname in this.formsLoaded) {
			this.formsLoaded[formname].Ready()
		} else {
			console.log("Problem in navigation.js: missing form " + formname)
		}
	}

	OpenForm(name, title = "", pushState=true, parameter="")
	{
		if (title == "") {
			title = i18next.t('navigation.' + name.substring(name.lastIndexOf('/')+1) + "_title");
		}

		if (this.debug) {
			console.log("OpenForm: " + name + " title: " + title);
		}

		this.ActivateModuleInTopbar();
		this.getCurrentModule(name);

		// fetch screen content from the server
		var navPage = this.GetNavigationPage(name);
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
					content = content.replaceAll(".js", ".js" + refresh);
					var formname = name.substring(name.lastIndexOf('/')+1);
					content = i18n.translate(content, formname);
					$("#containerIFrames").html(content);
					self.LoadJavascript(name, formname, refresh);
			});
		}
		else // fetch navigation page / dash board
		{
			this.loadDashboard(name);
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

	transformPath(path) {
		path = path.replace(/_/g, '/');
		if (path[0] == '/') {
			// drop leading slash
			path = path.substring(1);
		}
		if (path == "") {
			path = "Home";
		}
		return path.split('/');
	}

	getCurrentModule(path) {
		var path = this.transformPath(path);

		if (path.length > 0) {
			this.module = path[0];
		}
		if (path.length > 1) {
			this.submodule = path[1];
		}

		if (this.module == "CrossLedgerSetup") {
			this.module = "Finance";
		}

		return [this.module, this.submodule];
	}

	FindNavigationItem(search, node, path) {
		var items = node.items;
		for (var itemid in items) {
			var item = items[itemid];

			if (path + "/" + itemid == search)
			{
				return node.items[itemid];
			}

			var testnode = this.FindNavigationItem(search, item, path + "/" + itemid);
			if (testnode != null) {
				return testnode;
			}
		}

		return null;
	}

	GetNavigationItem(path) {
		path = this.transformPath(path);

		if (this.module == "Settings") {
			return null;
		}

		var navigation = JSON.parse(window.localStorage.getItem('navigation'));

		var node = null;
		if (path.length == 1) {
			node = navigation[this.module];
		} else {
			node = this.FindNavigationItem(path.join("/"), navigation[this.module], this.module);
		}

		if (node != null && !node.hasOwnProperty('htmlexists')) {
			node['htmlexists'] = false;
		}

		if (node != null && this.debug) {
			console.log("GetNavigationItem:", node);
		}

		return node;
	}

	GetNavigationPage(path) {
		var currentPage = null;
		var caption = null;

		var node = this.GetNavigationItem(path);

		if (node != null && node['htmlexists'] != true) {
			currentPage = window.location.pathname.substring(1);
			var navigation = JSON.parse(window.localStorage.getItem('navigation'));
			caption = i18next.t('navigation.' + navigation[this.module].caption);
			if (node.caption != navigation[this.module].caption) {
				caption += ": " + i18next.t('navigation.' + node.caption);
			}
			return [currentPage, caption];
		}

		return null;
	}

	ActivateModuleInTopbar() {
		// highlight the icon in the top navbar
		if (this.module != null) {
			$('li.nav-item a').removeClass('active');
			$('li.nav-item a[id=mnu'+this.module+']').addClass('active');
		}
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

			currentPage = this.GetNavigationPage(path);
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

		// currentPage has not been set above. so go to the default page, which is home.
		if (currentPage == null) {
			// load home page or Dashboard
			this.OpenForm("Home", i18next.t("navigation.home"));
		}
	}

	AddModuleToTopBar(name, title, icon, enabled)
	{
		if (enabled) {
			$("#ModuleNavBar").append("<li class='nav-item'><a id='mnu" + name + "' class='nav-link top-icon' href='#' title='" + title + "'><i class='fas fa-" + icon + "'></i><span class='topnav-icontext'>" + title + "</span></a></li>");
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

	AddMenuItem(folderid, parent, item, title, tabtitle, icon, indent)
	{
		var url = folderid + "/" + parent.name + "/" + item.caption.replace("_label","");
		var name = url.replace(/\//g, '_');
		$("#LeftNavigation").append("<a href='#' class='sidebar-item indent" + indent + "' id='" + name + "' title='" + title + "'><i class='fas fa-" + icon + " icon-invisible'></i> " + title +"</a>");
		this.AddMenuItemHandler(name, name, tabtitle);
	}

	// eg. SystemManager/Users/MaintainUsers is a link directly to a form, not a navigation page
	AddMenuItemForm(folderid, parent, item, title, tabtitle, icon, indent)
	{
		var url = folderid + "/" + parent.name + "/" + item.form;
		if (item.hasOwnProperty('path')) {
			url = item.path + "/" + item.form;
		}
		var name = url.replace(/\//g, '_');
		if (item.action != '') {
			name += "_" + item.action;
		}
		$("#LeftNavigation").append("<a href='/" + url + "' class='sidebar-item indent" + indent + "' id='" + name + "' title='" + title + "'><i class='fas fa-" + icon + " icon-invisible'></i> " + title +"</a>");
		this.AddMenuItemHandler(name, url, tabtitle, item.action);
	}

	displayNavigationSideBarItem(folderid, parent, folder, indent = 0) {
		var items = parent.items;
		for (var itemid in items) {
			var item = items[itemid];
			var title = i18next.t('navigation.' + item.caption);

			if (item.form != null) {
				this.AddMenuItemForm(folderid, parent, item,
					i18next.t('navigation.' + item.caption),
					i18next.t('navigation.'+folder.caption) + ": "+ i18next.t('navigation.'+item.caption),
					folder.icon,
					indent);
			} else {
				this.AddMenuItem(folderid, parent, item,
					i18next.t('navigation.' + item.caption),
					i18next.t('navigation.'+folder.caption) + ": "+ i18next.t('navigation.'+item.caption),
					folder.icon,
					indent);
			}
			if (item.hasOwnProperty('items')) {
				this.displayNavigationSideBarItem(folderid, item, folder, indent + 1);
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

			$("#LeftNavigation").append("<h1 class='sidebar'>" + title + "</h1>");

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
		this.AddMenuItemHandler('mnuHelpAbout', "About", i18next.t("navigation.about"));
		this.AddMenuItemHandler('mnuHelpReleaseNotes', "ReleaseNotes", i18next.t("navigation.releasenotes"));

		this.AddMenuItemHandler('mnuHome', "Home", i18next.t("navigation.Home_label"));
		this.AddMenuItemHandler('mnuPartner', "Partner", i18next.t("navigation.Partner_label"));
		this.AddMenuItemHandler('mnuFinance', "Finance", i18next.t("navigation.Finance_label"));
		this.AddMenuItemHandler('mnuSystemManager', "SystemManager", i18next.t("navigation.SystemManager_label"));

		var self = this;

		// select module in top bar: make it active, deactivate other modules
		$('#ModuleNavBar li.nav-item a').click(function(e) {
			$('#ModuleNavBar li.nav-item a').removeClass('active');
			$(this).addClass('active');
			var id=$(this).prop('id');
			if (id.startsWith("mnu")) {
				self.module = id.substring(3);
			}
			self.displayNavigationSideBar(navigation);
		});

		this.UpdateLocation();
	}

	addCard(item) {
		var html = "";
		html += '<div class="col-lg-6 col-xl-4">';
		html += '<div class="card">';
		html += '<div class="card-header">' + i18next.t('navigation.' + item.caption) + '</div>';
		html += '<div class="card-content">';

		for (var childitemid in item.items) {
			var child = item.items[childitemid];
			var caption = i18next.t('navigation.' + child.caption);

			if (child.action != undefined && child.form != undefined) {
				html += "<a href='" + child.path + "/" + child.form + "' " +
					"op_caption='" + caption + "' op_pushstate='true' op_action='" + child.action + "'>" +
					"<i class='fas fa-" + child.icon + "'></i>" +
					"<span>" + caption + "</span></a>";
			} else {
				var path = child.path + "/" + childitemid;
				if (child.form == undefined) {
					path = child.path;
				}
				html += "<a href='" + path + "' " +
					"op_caption = '" + caption + "'>" +
					"<i class='fas fa-" + child.icon + "'></i>" +
					"<span>" + caption + "</span></a>";
			}
		}

		html += '</div>';
		html += '</div>';
		html += '</div>';

		return html;
	}

	loadDashboard(navpage) {
		var node = this.GetNavigationItem(navpage);
		let self = this;

		if (node != null && node['htmlexists'] != true) {

			var hasGrandChildren = false;
			for (var itemid in node.items) {
				var item = node.items[itemid];
				for (var childitemid in item.items) {
					var child = item.items[childitemid];
					hasGrandChildren = true;
				}
			}

			var html = '<div class="container container-list dashboard">';
			html += '<div id="dashboardRow" class="row">';

			if (!hasGrandChildren) {
				html += this.addCard(node);
			} else {
				var items = node.items;
				for (var itemid in items) {
					var item = items[itemid];
					html += this.addCard(item);
				}
			}

			html += '</div>';
			html += '</div>';

			$("#containerIFrames").html(html);

			let elements = $("#containerIFrames a[op_caption]");
			elements.each(function(index) {$(this).on("click", function(e) {
				e.preventDefault();
				let element = $(this);
				self.OpenForm(element.attr('href'), element.attr('op_caption'), element.attr('op_pushstate'), element.attr('op_action'))});
			});
		}

		return;

		// TODO: implement real dash board, with graphs, and configurable
/*
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
					result.htmlpage = i18n.translate(result.htmlpage, "navigation");
					$("#containerIFrames").html(result.htmlpage);
				} else {
					console.log(response.data.d);
				}
			})
			.catch(function(error) {
				console.log(error);
			});
*/
	}

	loadNavigation() {
		// TODO: caching for this user??? see window.localStorage below
		self = this;
		// load sidebar navigation from UINavigation.yml
		api.post('serverSessionManager.asmx/GetNavigationMenu', {})
			.then(function(response) {
				var result = JSON.parse(response.data.d);
				if (result.resultcode == "success") {
					window.localStorage.setItem('navigation', JSON.stringify(result.navigation));
					self.displayNavigationTopBar(result.navigation);
					self.displayNavigationSideBar(result.navigation);
					window.onpopstate = function(e) {
						if (e.state != null) {
							self.OpenForm(e.state.name, e.state.title, false);
						}
					};
					if (result.assistant != "" && window.location.pathname == "/") {
						if (window.location.pathname != "/" + result.assistant) {
							self.OpenForm(result.assistant);
						}
					}
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

	LoadAvailableLedgerDropDown();
});

function LoadAvailableLedgerDropDown() {
	// check for FINANCE-1 permission. else: hide the ledger selection
	let permissions = window.localStorage.getItem('ModulePermissions');
	if (permissions == null) {
		permissions = "";
	}
	let permissionsFormatted = (" " + permissions.replace(/\n/g, ' ') + " ");
	if (!permissionsFormatted.includes(" FINANCE-1 ")) {

		// if the user still has access to a ledger (e.g. SponsorADMIN), then store that as default ledger
		if (permissionsFormatted.includes(" LEDGER")) {
			let i = permissionsFormatted.indexOf(" LEDGER") + " LEDGER".length;
			let ledgernumber = permissionsFormatted.substring(i, i + 4); 
			window.localStorage.setItem('current_ledger', parseInt(ledgernumber));
		}

		$('#LedgerSelection').hide();
		return;
	}

	api.post('serverMFinance.asmx/TGLSetupWebConnector_GetAvailableLedgers', {}).then(function (data) {
		data = JSON.parse(data.data.d);
		let dump = $('#ledger_select_dropdown').html('');
		let menu = '';
		let current_selected_ledger = null;
		for (var ledger of data.result) {
			if ( ledger.a_ledger_number_i == window.localStorage.getItem('current_ledger') || data.result.length == 1) {
				current_selected_ledger = ledger;
				if (data.result.length == 1 && window.localStorage.getItem('current_ledger') == null) {
					change_standard_ledger(current_selected_ledger.a_ledger_number_i);
				}
			}
			menu += '<li>';
			menu += '<a class="dropdown-item" href="#" '+
				'onclick="change_standard_ledger(' + ledger.a_ledger_number_i + ')">' +
				ledger.a_ledger_name_c + '</a>';
			menu += '</li>';
		}
		menu += '';
		dump.append($(menu));
		if (current_selected_ledger == null) {
			$('#current_ledger_field').html('<b style="color:#f88;">'+i18next.t('navigation.ledgerselect_none')+'</b>');
		} else {
			$('#current_ledger_field').text(current_selected_ledger.a_ledger_name_c);
		}
	});
}

function change_standard_ledger(ledger_id) {
	window.localStorage.setItem('current_ledger', ledger_id);
	api.post('serverMFinance.asmx/TGLSetupWebConnector_GetAvailableLedgers', {}).then(function (data) {
		data = JSON.parse(data.data.d);
		for (ledger of data.result) {
			if (ledger.a_ledger_number_i == ledger_id) {
				utils.display_message(i18next.t("navigation.switchledger")+" "+ledger.a_ledger_name_c, "success");
				$('#current_ledger_field').text(ledger.a_ledger_name_c);
			}
		}
	})
}

let nav = new Navigation();

export default nav;