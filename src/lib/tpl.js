// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <tp@tbits.net>
//       Christopher Jäkel <cj@tbits.net>
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


// this will fill all possible elements with data, if there name=
// attribute is the same as the given object key
// it also replaces all {something} in this object, by converting it to a string and back
function format_tpl(tpl, data, limit_to_table) {
  if (tpl[0] === undefined) {
    console.log("missing tpl for format_tpl");
    return "";
  }
  if (limit_to_table == null) {
    limit_to_table = "";
  }
  for (variable in data) {
    let key = false;
    let f = tpl.find("[name="+variable+"]");
    if (f.length == 0) {
      f = tpl.find("[name="+limit_to_table+variable+"]");
    }
    if (f.length == 0) {
      f = tpl.find("[key-name="+variable+"]");
      key = true;
    }

    if (f.is('select') && key==false) {
      value = data[variable];
      $(f).find("option[value='" + value + "']").attr("selected", true);
    }
    else if (f.is('textarea') && key==false) {
      f.text(data[variable]);
    }
    else if (f.attr('type') == "checkbox" && key==false) {
      f.attr('checked', data[variable]);
      f.prop('checked', data[variable]);
    }
    else if (f.attr('type') == "radio" && key==false) {
      tpl.find('[name='+variable+'][value='+data[variable]+']').prop('checked', true);
      tpl.find('[name='+variable+'][value='+data[variable]+']').attr('checked', 'checked');
    }
    else if ( key==false ) {
      value = data[variable];
      if (typeof value === 'string' || value instanceof String) {
        value = parseJSONDate(variable, value);
      }
      f.attr('value', value);
      f.val(value);
    }
    else {
      //hidden key case
      value = data[variable];
      f.attr('key-value', value);
    }

    let g = tpl[0].outerHTML;
    if (data[variable] == null) {
      data[variable] = "";
    }
    g = g.replace(new RegExp('{val_'+variable+'}',"g"), data[variable]);
    tpl = $(g);
  }

  return tpl;
}

function parseJSONDate(variable, value) {
  // https://www.newtonsoft.com/json/help/html/DatesInJSON.htm
  if (value.substring(0, 6) == "/Date(") {
    d = new Date(parseInt(value.substring(6, value.indexOf(')'))));
    if (variable == "s_modification_id_t") {
      value = d.getTime();
    } else {
      value = d.getFullYear() + "-" + ("0"+(d.getMonth()+1)).slice(-2) + "-" + ("0" + d.getDate()).slice(-2);
      if (d.getHours() != 0 || d.getMinutes() != 0 && d.getSeconds() != 0) {
        value += " " + ("0"+d.getHours()).slice(-2) + ":" + ("0"+d.getMinutes()).slice(-2) + ":" + ("0"+d.getSeconds()).slice(-2);
      }
    }
  }
  return value;
}

function printJSONDate(value) {
  // https://www.newtonsoft.com/json/help/html/DatesInJSON.htm
  if (value.substring(0, 6) == "/Date(") {
    let t = /\((.+)\)/g.exec(value);
    if (t == null || t.length <=1) {return}

    time = new Date(parseInt(t[1])).toLocaleDateString();
    return time;
  }
  return value;
}

// this is the oposite the format one, it will extract all name= objects
// and will return a object in key, values where key is the name and value... well the value
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
    if (obj.attr('type') == 'radio') {
      r[obj.attr('name')] = object.find('[name='+obj.attr('name')+']:checked').val();
    }
  });

  object.find('[key-name]').each(function (i, obj) {
    obj = $(obj);
    r[obj.attr('key-name')] = obj.attr('key-value');
  });

  return r;
}
