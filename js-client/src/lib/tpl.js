// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//       Christopher Jäkel
//
// Copyright 2017-2018 by TBits.net
// Copyright 2019-2024 by SolidCharity.com
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

import i18next from 'i18next'

class Tpl {

    constructor() {
        Date.prototype.toDateInputValue = (function() {
            var local = new Date(this);
            local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
            return local.toJSON().slice(0,10);
        });
    }

    translate_constants(value) {
        if (value == "") {
            return value;
        }
        if (!(typeof value === 'string' || value instanceof String)) {
            return value;
        }
        if (value == value.toUpperCase() || value == "true" || value == "false" ) {
            // translate constant values if available
            value = i18next.t("constants."+value, value);
        }
        return value;
    }

    replace_val_variables_in_attr(attr, data) {
        self = this
        if (attr !== undefined && attr.indexOf('{val_') !== -1) {
            for (var variable in data) {
                if (data[variable] == null) {
                    data[variable] = "";
                }
                let value = data[variable]

                if (typeof value === 'boolean' || value instanceof Boolean) {
                    value = self.translate_constants(value.toString());
                }
                else if (typeof value === 'string' || value instanceof String) {
                    value = self.translate_constants(value);
                }

                if (typeof value === 'string' || value instanceof String) {
                    value = self.parseJSONDate(variable, value);
                }

                attr = attr.replace(new RegExp('{val_'+variable+'}',"g"), value);
            }
        }
        return attr;
    }

    // this will replace all {val_something} in this object, by converting it to a string and back
    // check all attributes of a, name, and replace variables
    // check all attributes of div, id and onclick, and replace variables
    // check all attributes of button, onclick, and replace variables
    replace_val_variables(tpl, data) {
        self = this;
        if (tpl[0] === undefined) {
            console.log("missing tpl for format_tpl");
            return "";
        }
        let id = $(tpl).attr('id');
        if (id !== undefined && id != null) {
            $(tpl).attr('id', self.replace_val_variables_in_attr(id, data));
        }

        let onclick = $(tpl).attr('onclick');
        if (onclick != undefined && onclick != null) {
            $(tpl).attr('onclick', self.replace_val_variables_in_attr(onclick, data));
        }

        let title = $(tpl).attr('title');
        if (title !== undefined && title != null) {
            $(tpl).attr('title', self.replace_val_variables_in_attr(title, data));
        }

        $(tpl).find('button, div, div span, div div, div div span, div span, a').each(function() {
            let id = $(this).attr('id');
            if (id !== undefined && id != null) {
                $(this).attr('id', self.replace_val_variables_in_attr(id, data));
            }
            let onclick = $(this).attr('onclick');
            if (onclick !== undefined && onclick != null) {
                $(this).attr('onclick', self.replace_val_variables_in_attr(onclick, data));
            }
            let name = $(this).attr('name');
            if (name != undefined && name != null) {
                $(this).attr('name', self.replace_val_variables_in_attr(name, data));
            }
            let title = $(this).attr('title');
            if (title !== undefined && title != null) {
                $(this).attr('title', self.replace_val_variables_in_attr(title, data));
            }
            let text = $(this).text();
            if (text !== undefined && text != null && text[0] == "{") {
                $(this).text(self.replace_val_variables_in_attr(text, data));
            }
        });

        return tpl;
    }

    // tpl needs to be a DOM object, not just text.
    format_tpl(tpl, data, limit_to_table) {
        if (tpl[0] === undefined) {
            console.log("missing tpl for format_tpl");
            return "";
        }

        tpl = this.replace_val_variables(tpl, data);
        tpl = this.set_values_of_input_variables(tpl, data, limit_to_table);

        return tpl;
    }

    parseDates(key, value) {
        const dateFormat = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$/;
        if (typeof value === "string" && dateFormat.test(value)) {
            return new Date(value);
        }

        return value;
    }

    // this will fill all possible elements with data, if their name=
    // attribute is the same as the given object key
    set_values_of_input_variables(tpl, data, limit_to_table) {

        if (limit_to_table == null) {
            limit_to_table = "";
        }

        for (var variable in data) {
            let key = false;
            let f = tpl.find("[name="+variable+"]");
            if (f.length == 0) {
                f = tpl.find("[name="+limit_to_table+variable+"]");
            }
            if (f.length == 0) {
                f = tpl.find("[key-name="+variable+"]");
                key = true;
            }

            if (f.length == 0) {
                // console.log("cannot find variable " + variable);
                continue;
            }

            var value = null
            if (key == true) {
                //hidden key case
                value = data[variable];
                f.attr('key-value', value);
            } else {
                if (f.is('select')) {
                    value = data[variable];
                    f.val(value).change();
                }
                else if (f.is('textarea')) {
                    f.text(data[variable]);
                }
                else if (f.attr('type') == "checkbox") {
                    f.attr('checked', data[variable]);
                    f.prop('checked', data[variable]);
                }
                else if (f.attr('type') == "radio") {
                    tpl.find('[name='+variable+'][value='+data[variable]+']').prop('checked', true);
                    tpl.find('[name='+variable+'][value='+data[variable]+']').attr('checked', 'checked');
                }
                else if (f.attr("type") == "date") {
                    var v = data[variable];
                    if (v == "" || v == null) {
                        f.text("");
                        f.val("");
                    }
                    else
                    {
                        f.text( new Date(v).toLocaleDateString() );
                        v = new Date(v);
                        let YYYY = v.getFullYear();
                        let MM = v.getMonth() + 1;
                        let DD = v.getDate();
                        MM = (MM < 10 ? "0" : "") + MM;
                        DD = (DD < 10 ? "0" : "") + DD;
                        f.val( `${YYYY}-${MM}-${DD}` );
                    }
                }
                else {
                    value = data[variable];

                    if (f.attr("type") != "hidden") {
                        value = this.translate_constants(value);
                    }
                    f.attr('value', value);
                    f.val(value);
                }

                // check if this field also has a key-name with the same name as the field (eg. a_motivation_group_code_c)
                let f2 = tpl.find("[key-name="+variable+"]");
                    if (f2.length != 0) {
                        value = data[variable];
                        f2.attr('key-value', value);
                    }
                }
            }

            return tpl;
        }

        parseJSONDate(variable, value) {
        var dateregex = /^(\d{4})-0?(\d+)-0?(\d+)[T ]0?(\d+):0?(\d+):0?(\d+)$/;
        if (!value.match(dateregex)) return value;

        // https://www.newtonsoft.com/json/help/html/DatesInJSON.htm
        var d = new Date(value);
        if (d != "Invalid Date") {

            if (variable == "s_modification_id_t") {
            value = d.getTime();
            } else {
            value = d.toLocaleDateString();
            }

        }
        return value;
    }

    printJSONDate(value) {
    // https://www.newtonsoft.com/json/help/html/DatesInJSON.htm

    var d = new Date(value);
    if (d != "Invalid Date") {

        value = d.toLocaleDateString();

    }
    return value;
    }

    printCurrency(value, currency) {

    if (isNaN(value)) {
        // perhaps the value has been formatted already
        return value;
    }

    var formatter = new Intl.NumberFormat(navigator.language || navigator.userLanguage, {
        style: 'currency',
        currency: currency,
        minimumFractionDigits: 2
    });

    return formatter.format(value);
    }

    format_currency(currencyCode) {
        $(".format_currency:contains('-')").addClass('debit');
        $('.format_currency').each(
            function(x, obj) {
                obj = $(obj);
                let t = obj.text();
                if (t == null || t.length == 0) {return}
                obj.text(this.printCurrency(t, currencyCode));

            }
        )
    };

    format_date(from_string) {
    if (from_string != undefined) {
        let t = /\((.+)\)/g.exec(from_string);
        if (t && t.length > 1) {
        return new Date(parseInt(t[1])).toDateInputValue();
        } else { return ""; }
    }

        $('.format_date').each(
            function(x, obj) {
                obj = $(obj);
                let t = /\((.+)\)/g.exec(obj.text());
                if (t == null || t.length <=1) {return}

                time = new Date(parseInt(t[1])).toLocaleDateString();
                obj.text(time);

            }
        )
    };

    format_chk() {
        $('.format_chk').each(
            function(x, obj) {
                obj = $(obj);
                let t = obj.text();
                if (t == null || t.length <=1) {return}
                if (t[0] == "{") {return}

                if (t == "false") {
                    obj.html("<i class='fas fa-circle-thin'></i>");
                } else {
                    obj.html("<i class='fas fa-check-circle'></i>");
                }

            }
        )
    };

    // this is the oposite the format one, it will extract all name= objects
    // and will return a object in key, values where key is the name and value... well the value
    extract_data(object) {
        self = this
        var r = {};
        object.find('[name]').each(function (i, obj) {
            obj = $(obj);
            self.ValidateStringForInjectedHTML(obj.val());
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

        // for typeahead fields:
        object.find('[key-name]').each(function (i, obj) {
            obj = $(obj);
            if (typeof obj.attr('key-value') !== 'undefined') {
            r[obj.attr('key-name')] = obj.attr('key-value');
            }
            else {
            // set the default value
            r[obj.attr('key-name')] = obj.val();
            }
        });

        return r;
    }

    getKeyValue(object, name) {
        value = undefined;
        object.find('[key-name]').each(function (i, obj) {
            obj = $(obj);
            if (obj.attr('key-name') == name) {
            value = obj.attr('key-value');
            }
        });
        return value;
    }

    update_requireClass(object, class_) {
        object = $(object);
        object.find('[requireClass]').hide();
        object.find('.'+class_).show();
    }

    resetInput(o) {
        // o = JQuery object | str

        // 'o' is the target, in which all [name] elements get looped
        // and filled with a "neutral" state,
        // means empty strings in input fields and unchecked checkboxes

        // checkbox can be overwritten by [checked]
        // input fields can be overwritten by there [value] attribute
        // does not edit <data> elements!
        if (typeof o != "object") { o = $(o); }
        for (var f of o.find('[name]')) {
            f = $(f);

            // ignore data elements
            if ( f.is("data") ) { continue; }

            // is checkbox
            if (f.attr("type") == "checkbox") {
            f.prop( "checked", (f.is("[checked]") ? true : false) );
            }
            // any other type of element
            else {
            var pre_val = f.attr("value");
            f.val( pre_val ? pre_val : "" );
            }
        }
    }

    extractData(o) {
        self = this
        // o = JQuery object | str

        // 'o' is the target, in which all [name] elements get looped
        // and there .val() is stored with there attr('name') as this key
        // multiple same names overwrite each other, last one counts

        // if element is [type=checkbox] then data[name],
        // is 0 or 1 based on checked prop
        if (typeof o != "object") { o = $(o); }
        let data = {};
        for (var f of o.find('[name]')) {
            f = $(f);
            let name = f.attr('name');
            if (f.attr("type") == "checkbox") {
                if (f.is(":checked")) { data[name] = 1; }
                else { data[name] = 0; }
            } else if (f.attr("key-value") != null) {
                let value = f.attr("key-value");
                data[name] = value;
            }
            else {
                let value = f.val();
                self.ValidateStringForInjectedHTML(value);
                data[name] = value;
            }
        }
        o.find('[key-name]').each(function (i, obj) {
            obj = $(obj);
            data[obj.attr('key-name')] = obj.attr('key-value');
        });

        return data;
    }

    insertData(o, d, to_string=false, currencyCode="EUR", limit_to_table='') {
        self = this
        // o = JQuery object | str
        // d = object
        // to_string = bool :: false

        // 'o' is the target, in this target all keys of 'd' get searched
        // every matching [name=d_key] element, it inserts d[key]

        // if matching element is a [type=checkbox]:
        // element gets prop 'checked' set based on boolish interpretion of d[key]

        // to_string ensures content input by all types, except 'null'
        // which will be converted to a empty string

        if (typeof o != "object") { o = $(o); }
        for (var k in d) {
            try {
                var v = d[k];

                if (to_string) {
                    if (typeof v == "boolean") { v = v ? "true" : "false"; }
                    else if (v == null) { v = ""; }
                }
                let key = false;
                let f = o.find("[name="+k+"]");
                if (f.length == 0) {
                    f = o.find("[name="+limit_to_table+k+"]");
                }
                if (f.length == 0) {
                    f = o.find("[key-name="+k+"]");
                    key = true;
                }

                if (key == true) {
                    //hidden key case
                    f.attr('key-value', v);
                } else {
                    f = $(f);
                    if (f.attr("type") == "checkbox") {
                        if ( v ) { f.prop("checked", true) } else { f.prop("checked", false) }
                        } else if (f.attr("type") == "date") {
                        if (v == "" || v == null) {
                            f.text("");
                            f.val("");
                        }
                        else
                        {
                            f.text( new Date(v).toLocaleDateString() );
                            v = new Date(v);
                            let YYYY = v.getFullYear();
                            let MM = v.getMonth() + 1;
                            let DD = v.getDate();
                            MM = (MM < 10 ? "0" : "") + MM;
                            DD = (DD < 10 ? "0" : "") + DD;
                            f.val( `${YYYY}-${MM}-${DD}` );
                        }
                    } else if (f.attr("type") == "currency") {
                        if (v == "" || v == null) {
                            f.text("");
                        }
                        else
                        {
                            f.text( this.printCurrency(v, currencyCode) );
                        }
                    } else if ( ["SPAN","SUB","H1","H2"].indexOf(f.prop("tagName")) > -1 ) {
                        // avoid cross site scripting. still allow newlines as html code
                        html = v.replace(/<br\/>/g, 'NEWLINE').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/NEWLINE/g, '<br/>');

                        while ((pos = html.indexOf('mailto(')) >= 0) {
                            email = html.substring(pos + 'mailto('.length);
                            email = email.substring(0, email.indexOf(')'));
                            html = html.replace('mailto(' + email + ')', '<a href="' + email + '">' + email + '</a>');
                        }

                        f.html( html );
                    } else {
                        f.val( v );
                    }
                }
            }
            catch (e) { continue }
        }
        self.replace_val_variables(o,d);
    }

    ValidateStringForInjectedHTML(value)
    {
        if ((value.indexOf('>') != -1) && (value.indexOf('<') != -1) && (value.indexOf('=') != -1)) {
            alert('We do not allow a combination of certain characters in ' + value);
            throw ('We do not allow a combination of certain characters in ' + value);
        }
    }
}

let tpl = new Tpl()
export default tpl