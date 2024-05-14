//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//         Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
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

import i18next from 'i18next';
import HttpApi from 'i18next-http-backend';
import LanguageDetector from 'i18next-browser-languagedetector';

class I18n {
	constructor() {
        let develop = 1;

        // will be replaced by the build script for the release
        let currentrelease = "CURRENTRELEASE";
        let refresh = "?" + currentrelease;
        if (develop) {
            refresh = "?" + Date.now();
        }

        i18next
        .use(HttpApi)
        .use(LanguageDetector)
        .init({
          fallbackLng: 'en',
          debug: false,
          ns: ['common'],
          defaultNS: 'common',
          backend: {
            loadPath: '/locales/{{lng}}/{{ns}}.json' + refresh,
          }
        }, function(err, t) {
          // init set content
          this.updateContent();
        });
    }

    translateElement(obj, form) {
    if (form == "MaintainPartnerSelfService") form = "MaintainPartners";
    let placeholder=$(obj).attr('placeholder');
    if (placeholder !== undefined && placeholder[0] == '{') {
        placeholder = placeholder.substring(1, placeholder.length-1);
        $(obj).attr('placeholder', i18next.t(form + '.' + placeholder) );
    }
    let html = $(obj).html();
    if (html !== undefined && html[0] == '{') {
        html = html.substring(1, html.length-1);
        $(obj).html(i18next.t(form + '.' + html));
    }
    // a href must be replaced, eg. for the link to the manual
    html = $(obj).attr('href');
    if (html !== undefined && html[0] == '{') {
        html = html.substring(1, html.length-1);
        $(obj).attr('href', i18next.t(form + '.' + html));
    }
    html = $(obj).attr('title');
    if (html !== undefined && html[0] == '{') {
        html = html.substring(1, html.length-1);
        $(obj).attr('title', i18next.t(form + '.' + html));
    }
    }

    // this function will translate the index.html
    updateContent() {
    self = this;
    // specify the elements that should be translated
    $('#login input, #login button, #login h4, #login p, #login div a, #login span').each(function () { self.translateElement($(this), 'login'); });
    $('#reqNewPwd input, #reqNewPwd button, #setNewPwd input, #setNewPwd button, #signUp input, #signUp button').each(function () { self.translateElement($(this), 'login'); });
    $('#topnavigation a').each(function () { self.translateElement($(this), 'navigation'); });
    $('#sidebar span, #sidebar a').each(function () { self.translateElement($(this), 'navigation'); });
    $('.nav-link span').each(function () { self.translateElement($(this), 'navigation'); });
    $('.dropdown-item span').each(function () { self.translateElement($(this), 'navigation'); });
    }

    translate(html, form) {
        if (form == "MaintainPartnerSelfService") form = "MaintainPartners";
        let pos = -1;
        while ((pos = html.indexOf('{', pos+1)) > -1) {
            let pos2 = html.indexOf('}', pos);
            let key = html.substring(pos+1, pos2);
            if (key.indexOf('val_') == 0 || key.indexOf('chk_') == 0) {
            continue;
            }
            if (key.indexOf('.') > -1) {
            html = html.replaceAll('{'+key+'}', i18next.t(key));
            } else {
            html = html.replaceAll('{'+key+'}', i18next.t(form + "." + key));
            }
        }

        return html;
    }
} // class i18n

var i18n = new I18n();
export default i18n;

function changeLng(lng) {
  i18next.changeLanguage(lng);
}

function currentLng() {
  return i18next.language;
}

i18next.on('languageChanged', () => {
  i18n.updateContent();
  // see https://emojipedia.org/flags/
  var flag = "🇬🇧";
  switch (currentLng()) {
    case "de":
      flag = "🇩🇪";
      break;
    case "nb-NO":
      flag = "🇳🇴";
      break;
  }
  $("#chlang span").text(flag);
});
