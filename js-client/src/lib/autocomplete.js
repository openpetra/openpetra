// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//			 Christopher Jäkel
//			 Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.	If not, see <http://www.gnu.org/licenses/>.
//

class Autocomplete {

	autocomplete(input_field_object, auto_list, onselect = null) {
		let self = this;
		return self.autocompleteWithGroup(input_field_object, null, auto_list, onselect);
	}

	autocompleteWithGroup(input_field_object, input_group_object, auto_list, onselect = null) {

		let self = this;

		if (input_field_object.attr('init-autocomplete') == null) {
			input_field_object.attr('select-field', '0');
			input_field_object.keydown(function ( event ) {
				let position = parseInt(input_field_object.attr('select-field'));
				let c = input_field_object.siblings('.autocomplete-items').find('div')
				//down
				if (event.which == 40) {
					position = position + 1;
					if (position > c.length) {
						position = c.length;
					}
					input_field_object.attr('select-field', position );
					c.removeClass('autocomplete-active');
					let cc = $(c[position-1]);
					cc.addClass('autocomplete-active');

				}
				//up
				else if (event.which == 38) {
					let position = position - 1;
					if (position <= 0) {
						position = 1;
					}
					input_field_object.attr('select-field', position );
					c.removeClass('autocomplete-active');
					let cc = $(c[position-1]);
					cc.addClass('autocomplete-active');
				}
				//enter
				else if (event.which == 13) {
					let sub = input_field_object.siblings('.autocomplete-items').find('.autocomplete-active');
					input_field_object.val( sub.text() );
					input_field_object.attr('groupkey', sub.find('input[type=hidden]').attr('groupkey'));
					input_field_object.attr('details', sub.find('input[type=hidden]').attr('details'));
					input_field_object.attr('key-value', sub.find('input[type=hidden]').val());
					if (onselect) { onselect(input_field_object, sub.find('input[type=hidden]').val()); }
					delete_all_guesses();
				}
			})
			input_field_object.attr('init-autocomplete', true);
		}

		let current_input = input_field_object.val();
		let list_field = null;
		if (input_field_object.siblings(".autocomplete-items").length == 0) {
			list_field = $("<div>");
			list_field.addClass("autocomplete-items");
			input_field_object.closest('.autocomplete').append(list_field);
		} else {
			list_field = input_field_object.siblings(".autocomplete-items");
		}

		list_field.html("");
		for (var pair in auto_list) {
			let guess = auto_list[pair];
			let guess_field = $("<div>");
			let content = "<span>" + guess.display + "</span>";
			content = content + "<input type='hidden' value='" + guess.key + "' groupkey='" + guess.groupkey + "' details='" + JSON.stringify(guess) + "'>"
			guess_field.html(content);
			guess_field.attr('label', guess.label);
			guess_field.attr('key', guess.key);
			guess_field.click(function () {
				input_field_object.val( $(this).attr('key') + ' ' + $(this).attr('label'));
				input_field_object.attr('groupkey', $(this).find('input[type=hidden]').attr('groupkey'));
				input_field_object.attr('details', $(this).find('input[type=hidden]').attr('details'));
				input_field_object.attr('key-value', $(this).find('input[type=hidden]').val());
				if (input_group_object != null) {
					input_group_object.val($(this).find('input[type=hidden]').attr("groupkey"));
				}
				self.delete_all_guesses();
				if (onselect) { onselect(input_field_object, $(this).find('input[type=hidden]').val()); }
			});
			list_field.append(guess_field);
		}
	}

	delete_all_guesses() {
		$('.autocomplete-items').remove();
	}
}

export default new Autocomplete();