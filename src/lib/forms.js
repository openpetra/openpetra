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
		content = content.replace(new RegExp('button id="new"',"g"), 'button id="new" class="btn btn-primary"');
		content = content.replace(new RegExp('button id="edit"',"g"), 'button id="edit" class="btn btn-primary"');
		content = content.replace(' id="tpl_', ' style="display:none" id="tpl_');
		return content;
	}

	search(apiUrl, parameters, fn_getTable) {
		api.post(apiUrl, parameters)
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
                	        fn_getTable(result).forEach(function(element) {
                        	        var newrow = tplrow.clone().
                                	        prop('id', 'row' + element[0]).
                                        	appendTo( parent );
	                                html = newrow.html();
        	                        for(var propertyName in element) {
                	                        html = html.replace(new RegExp('{val_'+propertyName+'}',"g"), element[propertyName]);
                        	        }
                                	newrow.html(html);
	                                newrow.show();
        	                });

				// TODO evaluate result.ATotalRecords, and tell the user if there are more records
                	}
        	});
	}
}
