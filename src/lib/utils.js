function API_call(url, args, next_function) {

    var r = api.post(url, args).then(
      function (data) {
        data = data.data.d
        data = JSON.parse(data);

        next_function( data );
      }
    );

  return r;

}

function display_message(content, style_arguments) {
  var display_space = $('#global_message_space');
  if (display_space.length == 0) {
    let x = $('<div id ="global_message_space" class="text-center" style="position:fixed;top:10vh;width:100%;padding:20px;z-index:5000;">');
    $('body').append(x);
  }
  var message = $('<div class="text-center msg" style="width:50%;margin:5px auto;cursor:pointer;" onclick="$(this).closest(\'.msg\').remove()">');
  message.addClass('display_message');

  if (typeof style_arguments == "string") {
    if (style_arguments == "success") {
      message.addClass('display_message_success');
    }
    if (style_arguments == "fail") {
      message.addClass('display_message_fail');
    }
  }

  if (style_arguments == null) {
    style_arguments = {};
  }

  for (var arg in style_arguments) {
    message.css(arg, style_arguments[arg]);
  }

  message.text(content);

  var m_id = Math.floor(Math.random() * 100000);
  message.attr('message-id', m_id);

  $('#global_message_space').append(message);

  setTimeout(function () {
    $('[message-id='+m_id+']').remove();
  }, 30000);

}

function translate_to_server(array) {
  let new_a = {};
  for (var key in array) {
    if (key == "") {
      continue;
    }
    let n_key = key.match(/.+?_(.*)_.+/);
    if (n_key == null) {
      new_a[key] = array[key];
    } else {
      let x = n_key[1].split('_');
      n_key = "A";
      for (word of x) {
        n_key += capitalizeFirstLetter(word);
      }

      new_a[n_key] = array[key];
    }
  }
  return new_a;
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}
