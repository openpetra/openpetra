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

class Navigation {
	constructor() {
		this.debug = 0;
	}

	// TODO: something about parameters
	// TODO: rename to OpenForm
	OpenTab(name, title, pushState=true)
	{
		var tabname = name.replace(/\//g, '_');

		if (this.debug) {
			console.log("OpenTab: " + tabname + " title: " + title);
		}

		// fetch screen content from the server
		if (name.substring(0, "frm".length) === "frm" || name.indexOf('/frm') > 0)
		{
			axios.get("/src/forms/" + name + ".html")
				.then(function(response) {
					response.data = translate(response.data, name.substring(name.indexOf('/frm')+1));
					response.data = response.data.replace(' id="tpl_', ' style="display:none" id="tpl_');
					$("#containerIFrames").html(response.data);
			});
		}
		else // fetch navigation page
		{
			this.loadNavigationPage(tabname);
		}
		var stateObj = { name: name, title: title };
		var newUrl = name.replace(/frm/g, '').replace(/_/g, '/');
		if (newUrl == "Home") { newUrl = ''; }
		newUrl = "/" + newUrl;
		if (window.location.pathname != newUrl && pushState) {
			if (this.debug) {
				console.log("history.pushState " + newUrl);
			}
			history.pushState(stateObj, title, newUrl);
		}
		var windowTitle = "OpenPetra: " + title;
		if (document.title != windowTitle) {
			document.title = windowTitle;
		}
	};

	// this is called on a reload of the page, we want to jump to the right location, depending on the URL
	UpdateLocation() {
		var currentPage = null;

		if (this.debug) {
			console.log("called UpdateLocation " + window.location.pathname);
		}

		// check if this is a link to a navigation page
		if (currentPage == null && window.location.pathname.length > 1) {
			var path = window.location.pathname.substring(1).split('/');
			if (path.length >= 2 && path[0] != "Settings") {
				// open left navigation sidebar at the right position
				$('a[href="#mnuLst' + path[0] + '"]').click();
			}
			if (path.length == 2) {
				var navigation = JSON.parse(localStorage.getItem('navigation'));

				if (path[0] in navigation) {
					if (path[1] in navigation[path[0]].items) {
						var navPage = window.location.pathname.substring(1);
						currentPage = navPage;
						this.OpenTab(navPage, navigation[path[0]].caption + ": " + navigation[path[0]].items[path[1]].caption);
					}
				}
			}
		}

		// check if this is a form, add frm to path
		if (currentPage == null && window.location.pathname.length > 1) {
			// load specific frames by URL
			var path = window.location.pathname;
			// replace last / with /frm
			var last = path.lastIndexOf('/');
			var frmName = (path.substring(0,last) + "/frm" + path.substring(last+1)).substring(1);
			this.OpenTab(frmName, i18next.t("navigation."+path.substring(last+1)+"_label"));
			currentPage = frmName;
		}

		if (currentPage == null) {
			// load home page or Dashboard
			this.OpenTab("frmHome", i18next.t("navigation.home"));
		}
	}

	AddMenuGroup(name, title, icon, enabled)
	{
		if (!enabled) {
			$("#LeftNavigation").append("<a href='#mnuLst" + name + "' class='list-group-item disabled' data-parent='#sidebar'> <i class='fa fa-" + icon + "'></i>  <span class='d-none d-md-inline'> " + title + "</span></a>");
		} else {
			$("#LeftNavigation").append("<a href='#mnuLst" + name + "' class='list-group-item d-inline-block collapsed' data-toggle='collapse' data-parent='#sidebar' aria-expanded='false'> <i class='fa fa-" + icon + "'></i>  <span class='d-none d-md-inline'> " + title + "</span> </a><div class='collapse' id='mnuLst" + name + "'></div></a>");
		}
	}

	AddMenuItemHandler(mnuItem, frmName, title) {
		var self = this;
		$('#' + mnuItem).click(function(event) {self.OpenTab(frmName, title);event.preventDefault();});
	}

	AddMenuItem(parent, name, title, tabtitle, icon)
	{
		$("#mnuLst" + parent).append("<a href='/#" + name + "' class='list-group-item' data-parent='#mnuLst" + parent + "' id='" + name + "'><i class='fa fa-" + icon + " icon-invisible'></i> " + title +"</a>");
		this.AddMenuItemHandler(name, name, tabtitle);
	}

        // eg. SystemManager/Users/MaintainUsers is a link directly to a form, not a navigation page
	AddMenuItemForm(parent, name, form, title, tabtitle, icon)
	{
		$("#mnuLst" + parent).append("<a href='" + form + "' class='list-group-item' data-parent='#mnuLst" + parent + "' id='" + name + "'><i class='fa fa-" + icon + " icon-invisible'></i> " + title +"</a>");
		this.AddMenuItemHandler(name, parent + "/frm" + form, tabtitle);
	}

	displayNavigation(navigation) {
		for (var folderid in navigation) {
			var folder = navigation[folderid];
			this.AddMenuGroup(folderid, i18next.t('navigation.'+folder.caption), folder.icon, folder.enabled != "false");
			if (folder.enabled == "false") {
				continue;
			}
			var items = folder.items;
			for (var itemid in items) {
				var item = items[itemid];
                                
				if (item.form != null) {
					this.AddMenuItemForm(folderid, folderid + "_" + itemid, item.form, 
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

		// link the items in the top menu
		this.AddMenuItemHandler('mnuChangePassword', "Settings/frmChangePassword", i18next.t("navigation.change_password"));
		this.AddMenuItemHandler('mnuChangeLanguage', "Settings/frmChangeLanguage", i18next.t("navigation.change_language"));
		this.AddMenuItemHandler('mnuHome', "frmHome", i18next.t("navigation.home"));

		this.UpdateLocation();
	}

	loadNavigationPage(navpage) {
		// TODO: store pages per user? see localStorage?
		self = this;
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
		// TODO: caching for this user??? see localStorage below
		self = this;
		// load sidebar navigation from UINavigation.yml
		api.post('serverSessionManager.asmx/GetNavigationMenu', null, null)
			.then(function(response) {
				var result = JSON.parse(response.data.d);
				if (result.resultcode == "success") {
					localStorage.setItem('navigation', JSON.stringify(result.navigation));
					self.displayNavigation(result.navigation);
					window.onpopstate = function(e) {
						if (e.state != null) {
							nav.OpenTab(e.state.name, e.state.title, false);
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
