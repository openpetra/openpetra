// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//         Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//         Christopher Jäkel
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.    See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.    If not, see <http://www.gnu.org/licenses/>.
//

import i18next from 'i18next';
import tpl from './tpl.js'

class Utils {
    constructor() {
        this.DEFAULT_TIMEOUT = 5000;
        this.latestTimeout = null;
    }

    toggle_pin_message(evt, m_id) {
        evt.stopPropagation();
        pin = (window.localStorage.getItem('pin_alert_message') === 'OFF');

        if (pin) {
            window.localStorage.setItem('pin_alert_message', "ON");
            $('#msgtack').html('<i class="fas fa-thumbtack">');
            clearTimeout(this.latestTimeout);
        }
        else {
            window.localStorage.setItem('pin_alert_message', "OFF");
            $('#msgtack').html('<i class="fas fa-history">');
            this.latestTimeout = setTimeout(function () {
                $('[message-id='+m_id+']').remove();
            }, this.DEFAULT_TIMEOUT);
        }
    }

    // a global message that generates messages in the upper middle of the screen, duh.
    display_message(content, style_arguments = null, timeout = this.DEFAULT_TIMEOUT) {
        var display_space = $('#global_message_space');
        if (display_space.length == 0) {
            let x = $('<div id ="global_message_space" class="text-center" style="position:absolute;top:10vh;width:50%;left: 50%;transform: translateX(-50%);z-index:5000;">');
            $('body').append(x);
        }

        let fullcontent = "";
        if (content.includes("|MSGSEPARATOR|")) {
            fullcontent = content.replace("|MSGSEPARATOR|", "").replaceAll("<br/>", "\n");
            var blob = new Blob([fullcontent], {type : "text/plain"});
            var url = URL.createObjectURL(blob);
            content = content.substring(0, content.indexOf("|MSGSEPARATOR|")) + "[...] " + i18next.t("forms.further_messages") + "<br/><br><a href='" + url + "' download='messages.txt'>" + i18next.t("forms.download_messages") + "</a>";
        }

        var message = $('<div id="message" class="text-center msg" style="width:100%;margin:5px auto;cursor:pointer;position:relative;" onclick="$(this).closest(\'.msg\').remove()"></div>');
        message.addClass('display_message');

        if (style_arguments == null) {
            style_arguments = {};
        }

        // if 3rd arg is a pre definded option we add a class
        if (typeof style_arguments == "string") {
            if (style_arguments == "success") {
                message.addClass('display_message_success');
            }
            if (style_arguments == "fail") {
                message.addClass('display_message_fail');
                timeout *= 2;
            }
        } else {
            // if 2nd arg is object we add each thing to style
            for (var arg in style_arguments) {
                message.css(arg, style_arguments[arg]);
            }
        }

        // need a random int to delete message
        var m_id = Math.floor(Math.random() * 100000);

        if (window.localStorage.getItem('pin_alert_message') === 'ON') {
            timeout = 0;
        }

        if (timeout) {
            content +=
                '<div id="msgtack" style="position:absolute;top:0px;right:20px;" onclick="toggle_pin_message(event, '+ m_id +');"><i class="fas fa-history"></i></div>';
        }
        else {
            content +=
                '<div id="msgtack" style="position:absolute;top:0px;right:20px;" onclick="toggle_pin_message(event, '+ m_id +');"><i class="fas fa-thumbtack"></i></div>';
        }

        content +=
            '<div id="msgclose" style="position:absolute;top:0px;right:4px;" onclick="$(this).closest(\'.msg\').remove()"><i class="fas fa-times"></i></div>';

        message.html(content);

        message.attr('message-id', m_id);

        $('#global_message_space').append(message);

        if (timeout) {
            this.latestTimeout = setTimeout(function () {
                $('[message-id='+m_id+']').remove();
            }, timeout);
        }
    }

    display_error(VerificationResult, LanguageNamespace = '', generalerror = 'errors.general') {
        if (VerificationResult == null) {
            this.display_message( i18next.t(generalerror), 'fail');
            return;
        }
        if (typeof VerificationResult === 'string') {
            this.display_message( i18next.t(VerificationResult), "fail");
            return;
        }
        let msgCount = 0;
        let errormsg = "";
        for (error of VerificationResult) {
            if (error.code == "" && error.message == "") {
                continue;
            }
            msgCount += 1;
            if (msgCount == 6) {
                errormsg += "|MSGSEPARATOR|";
            }
            if (error.code != "" && i18next.t(LanguageNamespace + "." + error.code) != LanguageNamespace + "." + error.code) {
                errormsg += i18next.t(LanguageNamespace + "." + error.code) + "<br/>";
            } else if (error.code != "" && i18next.t(error.code) != error.code) {
                errormsg += i18next.t(error.code) + "<br/>";
            } else {
                if (error.message.length > "base64".length && error.message.substr(0,"base64".length) == "base64") {
                    error.message = atob(error.message.substr("base64".length)).replace("\n", "<br/>");
                }
                errormsg += error.message + "<br/>";
            }
        }

        if (msgCount == 0) {
            this.display_message( i18next.t(generalerror), 'fail');
        } else {
            this.display_message( errormsg, 'fail');
        }
    }

    // splits words on _ and capitalize first letter each word
    translate_to_server(array) {
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

    // you can maybe guess what this function does
    capitalizeFirstLetter(string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    }

    // this thing is used to replace extracted data and but it into a given on
    // it will just look throgh all (key, value) pairs in given replace_obj
    // and then will look if a same val attribute is present in the update_data
    // if there is, it's updates it, at the end a updated object will be given back
    replace_data(replace_obj, update_data, prev_table) {
        // because there a special cases like:
        //     PPartner_s_comment_c
        //     PUnit_comment_c
        // both will be handled so that s_comment_c is set but only,
        // if a parent object is name like the thing before _
        // does this makes sense? probably not, but its needed, so don't ask
        if (prev_table == null) {
            prev_table = "";
        }

        for (var variable in replace_obj) {
            // update_date has a var with same name as replace_obj, so we replace it
            if (typeof update_data[variable] !== 'undefined') {
                replace_obj[variable] = update_data[variable];
            }
            if (typeof update_data[prev_table+variable] !== 'undefined') {
                replace_obj[variable] = update_data[prev_table+variable];
            }
            // maybe a update name is in a object
            if (replace_obj[variable] instanceof Object) {
                replace_obj[variable] = replace_data(replace_obj[variable], update_data, variable+"_");
            }
            if (replace_obj[variable] instanceof Array) {
                for (list_item of replace_obj[variable]) {
                list_item = replace_data(list_item, update_data, variable+"_");
                }
            }
        }
        return replace_obj;
    }

    // used to save presets in localStorage
    save_preset(field_name, field_values) {

        if (field_values == null) {
            field_values = tpl.extract_data( $('#tabfilter') );
        }

        window.localStorage.setItem(field_name, JSON.stringify(field_values) );

    }

    // window.atob does not work for umlaut
    // see https://stackoverflow.com/a/30106551
    b64DecodeUnicode(str) {
        return decodeURIComponent(Array.prototype.map.call(atob(str), function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
        }).join(''))
    }

    isEmpty(o) {
        // null
        if (o == null) { return true; }
        // string
        if (typeof o == "string") { if (o != "") { return false; } }
        // number
        if (typeof o == "number") { if (o != 0) { return false; } }
        // object
        for (var v in o) {
            if (o.hasOwnProperty(v)) {
                return false
            }
        }
        return true;
    }

    uploadFile(url, args, success_function, fail_function) {
        if (!(url && args)) {return false;}

        var formData = new FormData();
        for (var upl in args) {
            formData.append(upl, args[upl]);
        }
        var request = new XMLHttpRequest();
        request.onload = function () {
            if (200 <= request.status && request.status < 300) {
                success_function( request.responseText );
            } else if (request.status >= 500) {
                fail_function( Array() );
            } else {
                fail_function( request.responseText );
            }
        }
        request.onerror = function () {
            fail_function(JSONparse(request.responseText));
        }
        request.open("POST", url);
        request.send(formData);
    }
} // class Utils

var utils = new Utils();

export default utils;