function autocomplete(input_field_object, auto_list) {

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
        position = position - 1;
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
        input_field_object.attr('key-value', sub.find('input[type=hidden]').val());

        delete_all_guesses();
      }
    })
    input_field_object.attr('init-autocomplete', true);
  }

  let current_input = input_field_object.val();
  if (input_field_object.siblings(".autocomplete-items").length == 0) {
    list_field = $("<div>");
    list_field.addClass("autocomplete-items");
    input_field_object.closest('.autocomplete').append(list_field);
  } else {
    list_field = input_field_object.siblings(".autocomplete-items");
  }

  list_field.html("");
  for (value in auto_list) {
    guess = auto_list[value];
    guess_field = $("<div>");
    let content = "<span>" + guess + "</span>";
    content = content + "<input type='hidden' value='" + value + "'>"
    guess_field.html(content);
    guess_field.click(function () {
      input_field_object.val( $(this).text() );
      input_field_object.attr('key-value', $(this).find('input[type=hidden]').val());
      delete_all_guesses();
    });
    list_field.append(guess_field);
  }
}

function delete_all_guesses() {
  $('.autocomplete-items').remove();
}
