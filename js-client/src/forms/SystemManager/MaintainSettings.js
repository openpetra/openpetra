// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2019 by SolidCharity.com
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

$('document').ready(function () {
  let x = {ASystemDefaultName: 'SelfSignUpEnabled'};
  api.post('serverMSysMan.asmx/TSystemDefaultsConnector_GetSystemDefault', x).then(function (data) {
    enabled = data.data.d;
    $('#chkSelfSignUpEnabled').prop('checked', enabled == "True");
  });
});


function submit() {
  let x = {AKey: 'SelfSignUpEnabled', AValue: $('#chkSelfSignUpEnabled').is(':checked')};
  api.post('serverMSysMan.asmx/TSystemDefaultsConnector_SetSystemDefault', x).then(function (data) {
    result = JSON.parse(data.data.d);
    if (result == true) {
      utils.display_message(i18next.t('forms.saved'), "success");
    }
  });
}
