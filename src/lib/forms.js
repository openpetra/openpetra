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
	initContent(content) {
		content = content.replace(new RegExp('button id="filter"',"g"), 'button id="filter" class="btn btn-primary"');
		content = content.replace(new RegExp('button id="filter2"',"g"), 'button id="filter2" class="btn btn-primary"');
		content = content.replace(new RegExp('button id="cancelfilter"',"g"), 'button id="cancelfilter" class="btn"');
		content = content.replace(new RegExp('button id="new"',"g"), 'button id="new" class="btn btn-primary"');
		content = content.replace(new RegExp('button id="edit"',"g"), 'button id="edit" class="btn btn-primary"');
		content = content.replace(new RegExp('button id="view"',"g"), 'button id="view" class="btn btn-primary"');
		content = content.replace(new RegExp('button id="closeview"',"g"), 'button id="closeview" class="btn"');
		content = content.replace(new RegExp(' id="tpl_', "g"), ' style="display:none" id="tpl_');
		content = content.replace('id="tabfilter"', ' style="display:none" id="tabfilter"');
		content = content.replace('id="tpl_view"', ' style="display:none" id="tpl_view"');
		return content;
	}

	initSearch(apiUrl, parameters) {
		this.searchApiUrl = apiUrl;
		this.searchInitialParameters = parameters;
	}

	initEvents() {
		self = this;
		$('#cancelfilter').click(function(event) {
			$('#tabfilter').hide();
		});
		$('#filter').click(this.filterClick);
		$('#filter2').click(this.filterClick);
	}

	filterClick() {
		if ($('#tabfilter').css('display') == "none") {
			$('#tabfilter').show();
		} else {
			self.search();
			$('#tabfilter').hide();
		}
	}

	viewClose(event) {
		$( "#view" + event.data.key ).hide();
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
		self.getMainTableFromResult(self.data).forEach(function(element) {
			if (key == self.getKeyFromRow(element)) {
				for(var propertyName in element) {
					html = html.replace(new RegExp('{val_'+propertyName+'}',"g"), element[propertyName]);
				}
				return true; // same as break
			}
		});

        	newview.html(html);
		$('#view' + key + ' > td > #closeview').click({key: key}, self.viewClose);
		newview.show();
	}

	getSearchParams() {
		var parameters = {};
		$('#tabfilter input').each(function () {
			parameters[$(this).attr('name')] = $(this).val();
		});
		return parameters;
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
					//console.log($(this).attr('id')); //remove();
				});

				self.data = result;
                	        self.getMainTableFromResult(result).forEach(function(element) {
					var key = self.getKeyFromRow(element);
                        	        var newrow = tplrow.clone().
                                	        prop('id', 'row' + key).
                                        	appendTo( parent );
	                                html = newrow.html();
        	                        for(var propertyName in element) {
                	                        html = html.replace(new RegExp('{val_'+propertyName+'}',"g"), element[propertyName]);
                        	        }
                                	newrow.html(html);
	                                newrow.show();
					$('#row' + key).click({self: self, key: key}, self.viewClick);
        	                });

				// TODO evaluate result.ATotalRecords, and tell the user if there are more records
                	}
        	});
	}
}
