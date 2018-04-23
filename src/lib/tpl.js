function format_tpl(tpl, data) {
  for (variable in data) {
    let f = tpl.find("[name='"+variable+"']")
    if (f.attr('type') == "checkbox") {
      f.attr('checked', data[variable]);
      f.prop('checked', data[variable]);
    } else {
      f.attr('value', data[variable]);
      f.val(data[variable]);
    }
    let g = tpl[0].outerHTML;
    g = g.replace(new RegExp('{val_'+variable+'}',"g"), data[variable]);
    tpl = $(g);
  }

  return tpl;
}

function extract_data(object) {
  var r = {};
  object.find('[name]').each(function (i, obj) {
    obj = $(obj);
    r[obj.attr('name')] = obj.val();
    if (obj.attr('type') == 'checkbox') {

      if (obj.is(':checked')) {
        r[obj.attr('name')] = true;
      } else {
        r[obj.attr('name')] = false;
      }

    }
  })

  return r;
}
