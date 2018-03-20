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

class JSForm {
	// this function is static, because it is executed before the html of the form is loaded
	static initContent(content) {
		content = replaceAll(content, 'table id=', 'table class="table" id=');
		content = replaceAll(content, 'button id="filter"', 'button id="filter" class="btn btn-primary"');
		content = replaceAll(content, 'button id="filter2"', 'button id="filter2" class="btn btn-primary"');
		content = replaceAll(content, 'button id="cancelfilter"', 'button id="cancelfilter" class="btn btn-secondary"');
		content = replaceAll(content, 'button id="new"', 'button id="new" class="btn btn-primary"');
		content = replaceAll(content, 'button id="edit"', 'button id="edit" class="btn btn-primary"');
		content = replaceAll(content, 'button id="view"', 'button id="view" class="btn btn-primary"');
		content = replaceAll(content, 'button id="closeview"', 'button id="closeview" class="btn btn-secondary"');
		content = replaceAll(content, 'tr id="tpl_view"', 'tr class="view" id="tpl_view"');
		content = replaceAll(content, ' id="tpl_', ' style="display:none" id="tpl_');
		content = replaceAll(content, 'id="tabfilter"', ' style="display:none" id="tabfilter"');
		return content;
	}

	constructor(name, apiUrl, parameters) {
		this.name = name;
		this.searchApiUrl = apiUrl;
		this.searchInitialParameters = parameters;
	}

	initEvents() {
		self = this;
		$('#cancelfilter').click(this.filterCancel);
		$('#filter').click(this.filterClick);
		$('#filter2').click(this.filterClick);
		$('#new').click({self: self, key: key}, self.showAddDialog);
		
	}

	filterClick() {
		if ($('#tabfilter').css('display') == "none") {
			$('#tabfilter').show();
			$('#filter').hide();
		} else {
			self.search();
			$('#filter').show();
			$('#tabfilter').hide();
		}
	}

	filterCancel() {
		$('#filter').show();
		$('#tabfilter').hide();
	}

	// the complex html is added here for modal edit/add dialog
	createAddOrEditDialog(dialogname, title) {
		// create a copy of the template
		var tpl_edit = $( "#tpl_edit" );

		var tpl_edit1 = '<div class="modal modal-wide fade" id="' + dialogname + '" ' +
						'tabindex="-1" role="dialog" aria-labelledby="' + i18next.t(title) + '" ' +
						'aria-hidden="true">' +
						'<div class="modal-dialog" role="document">' +
						'<div class="modal-content">' +
						'<div class="modal-header">' +
						'<h5 class="modal-title" id="modalTitle">' + i18next.t(title) + '</h5>' +
						'<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
						'<span aria-hidden="true">&times;</span></button>' +
						'</div>'	+
						'<div class="modal-body">';
		var tpl_edit2 = '</div><div class="modal-footer">' +
						'<button type="button" class="btn btn-secondary" data-dismiss="modal">' + i18next.t('forms.cancel') + '</button>' +
						'<button type="button" class="btn btn-primary">' + i18next.t('forms.save') + '</button>' +
						'</div></div></div></div>';

		return tpl_edit1 + tpl_edit.html() + tpl_edit2;
	}

	reuseDialog(dialogname) {
		if ($('#' + dialogname).length) {
			// reuse existing dialog
			$('#' + dialogname).modal('show');
			return true;
		}

		return false;
	}

	initEventForTab() {
		// workaround because it seems the tab navigation does not work when the code is dynamically generated
		$('#detailTab a').on('click', function (e) {
			e.preventDefault();
			var href=$(this).prop('href');
			href=href.substring(href.indexOf('#')+1);
			$('div.tab-pane').hide();
			$('div.tab-pane[id="'+href+'"]').show();
		});
	}

	showAddDialog(event) {
		self = event.data.self;

		var dialogname = 'newDialog';
		if (self.reuseDialog(dialogname)) {
			return;
		}

		// create a copy of the template
		var tpl_edit = $( "#tpl_edit" );
		var newedit = tpl_edit.clone().prop('id', dialogname).insertAfter('#tpl_edit');
		var html = self.createAddOrEditDialog(dialogname, self.name + ".addtitle");

		// clear all variables
		pos = -1;
		while ((pos = html.indexOf('{', pos+1)) > -1) {
			pos2 = html.indexOf('}', pos);
			key = html.substring(pos+1, pos2);
			if (key.indexOf('val_') !== false) {
				html = replaceAll(html, '{'+key+'}', '');
			}
		}
		newedit.replaceWith(html);

		$('#' + dialogname).modal('show');

		self.initEventForTab();
	}

	showEditDialog(event) {
		self = event.data.self;
		key = event.data.key;
		var dialogname = 'editDialog' + key;
		if (self.reuseDialog(dialogname)) {
			return;
		}

		// create a copy of the template
		var tpl_edit = $( "#tpl_edit" );
		var newedit = tpl_edit.clone().prop('id', dialogname).insertAfter('#tpl_edit');
		var html = self.createAddOrEditDialog(dialogname, self.name + ".edittitle");

		self.getMainTableFromResult(self.data).forEach(function(row) {
			if (key == self.getKeyFromRow(row)) {
				html = self.insertRowValues(html, row);
				return true; // same as break
			}
		});

		newedit.replaceWith(html);

		$('#' + dialogname).modal('show');

		self.initEventForTab();
	}

	viewClose() {
		$(".view").not("#tpl_view").remove();
	}

	viewClick(event) {
		self = event.data.self;
		key = event.data.key;
		if ($('#view' + key).length) {
			$(".view").not("#tpl_view").remove();
			return;
		}
		$(".view").not("#tpl_view").remove();
		var tpl_view = $( "#tpl_view" );
		var newview = tpl_view.clone().prop('id', 'view' + key).insertAfter('#row'+ key);
		var html = newview.html();
		self.getMainTableFromResult(self.data).forEach(function(row) {
			if (key == self.getKeyFromRow(row)) {
				html = self.insertRowValues(html, row);
				return true; // same as break
			}
		});

		newview.html(html);
		$('#view' + key + ' > td > #closeview').click(self.viewClose);
		$('#view' + key + ' > td > #edit').click({self: self, key: key}, self.showEditDialog);
		newview.show();
	}

	getSearchParams() {
		var parameters = {};
		$('#tabfilter input').each(function () {
			parameters[$(this).attr('name')] = $(this).val();
		});
		$('#tabfilter select').each(function () {
			parameters[$(this).attr('name')] = $(this).val();
		});
		return parameters;
	}

	insertRowValues(html, row) {
		for(var propertyName in row) {
			if (row[propertyName] === null) {
				html = html.replace(new RegExp('{val_'+propertyName+'}',"g"), '');
			} else {
				html = html.replace(new RegExp('{val_'+propertyName+'}',"g"), row[propertyName]);
			}
		}
		return html;
	}

	search() {
		self = this;
		var parameters;
		if ($('#tabfilter').css('display') != "none") {
			parameters = self.getSearchParams();
		} else {
			parameters = self.searchInitialParameters;
		}
		api.post(self.searchApiUrl, parameters)
			.then(function(response) {
				if (response.data == null) {
					console.log("error: " + response);
					return;
				}
				var result = JSON.parse(response.data.d);
				if (result.result == "false") {
					console.log("problem loading " + apiUrl);
				} else {
					var tplrow = $( "#tpl_row" );
					parent = tplrow.parent();

					// clear previous result
					$('#browse tr').each(function() {
						if ($(this).attr('id') !== undefined && $(this).attr('id').startsWith('row')) {
							$(this).remove();
						}
					});

					// clear view
					self.viewClose();

					self.data = result;
					self.getMainTableFromResult(result).forEach(function(row) {
						var key = self.getKeyFromRow(row);
						var newrow = tplrow.clone().
								prop('id', 'row' + key).
								appendTo( parent );
						html = newrow.html();
						html = self.insertRowValues(html, row);
						newrow.html(html);
						newrow.show();
						$('#row' + key).click({self: self, key: key}, self.viewClick);
					});

					// TODO evaluate result.ATotalRecords, and tell the user if there are more records
				}
			});
	}
}
