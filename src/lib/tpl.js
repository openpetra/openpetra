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
  if (limit_to_table == null) {
    limit_to_table = "";
  }
  for (variable in data) {
    let f = tpl.find("[name="+variable+"]");
    if (f.length == 0) {
      f = tpl.find("[name="+limit_to_table+variable+"]");
    }
    if (f.is('textarea')) {
      f.text(data[variable]);
    }
    if (f.attr('type') == "checkbox") {
      f.attr('checked', data[variable]);
      f.prop('checked', data[variable]);
    } else {
      f.attr('value', data[variable]);
      f.val(data[variable]);
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
  })

  return r;
}
