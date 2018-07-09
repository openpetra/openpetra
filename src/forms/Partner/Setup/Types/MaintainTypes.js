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

var last_requested_data = {};

$('document').ready(function () {
	display_list();
});

function display_list() {
	api.post('serverMPartner.asmx/TPartnerSetupWebConnector_LoadPartnerTypes', {}).then(function (data) {
	  data = JSON.parse(data.data.d);
		// on reload, clear content
		$('#browse_container').html('');
		last_requested_data = data.result.PType;
		for (item of data.result.PType) {
			// format a abo for every entry
			format_item(item);
		}
	})
}

function format_item(item) {
	let row = format_tpl($("[phantom] .tpl_row").clone(), item);
	let view = format_tpl($("[phantom] .tpl_view").clone(), item);
	row.find('.collapse_col').append(view);
	$('#browse_container').append(row);
}

function open_detail(obj) {
	obj = $(obj);
	if (obj.find('.collapse').is(':visible') ) {
		$('.tpl_row .collapse').collapse('hide');
		return;
	}
	$('.tpl_row .collapse').collapse('hide');
	obj.find('.collapse').collapse('show')
}

function open_edit(sub_id) {
  let z = null;
  for (sub of last_requested_data) {
    if (sub.p_type_code_c == sub_id) {
      z = sub;
      break;
    }
  }

  var f = format_tpl( $('[phantom] .tpl_edit').clone(), z);
  $('#modal_space').html(f);
	$('#modal_space .modal').modal('show');
}

function open_new() {
  let n_ = $('[phantom] .tpl_new').clone();
  $('#modal_space').html(n_);
	$('#modal_space .modal').modal('show');
}

function save_new() {

    let se = $('#modal_space .modal').modal('show');
    let request = translate_to_server(extract_data(raw));
    
    request['action'] = 'create';

    api.post("serverMPartner.asmx/TPartnerSetupWebConnector_MaintainTypes", request).then(
      function () {
        display_message(i18next.t('MaintainTypes.confirm_create'), 'success');
      }
    )

}

function save_entry(update) {
  let raw = $(update).closest('.modal');
  let request = translate_to_server(extract_data(raw));

  request['action'] = 'update';

  api.post("serverMPartner.asmx/TPartnerSetupWebConnector_MaintainTypes", request).then(
    function () {
      display_message(i18next.t('MaintainTypes.confirm_edit'), 'success');
    }
  )
}

function delete_entry(d) {
  let raw = $(d).closest('.modal');
  let request = translate_to_server(extract_data(raw));

  request['action'] = 'delete';

  if (!s) {return}

  api.post("serverMPartner.asmx/TPartnerSetupWebConnector_MaintainTypes", request).then(
    function () {
      display_message(i18next.t('MaintainTypes.confirm_delete'), 'success');
    }
  );

}
