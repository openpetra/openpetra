// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//       Christopher Jäkel <cj@tbits.net>
//
// Copyright 2017-2019 by TBits.net
// Copyright 2019-2020 by SolidCharity.com
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
	if (window.location.href.endsWith('?NewPublication')) {
		open_new();
	} else {
		display_list();
	}
});

function display_list() {
  let r = {'ACacheableTable':'PublicationList', 'AHashCode':''};
	api.post('serverMPartner.asmx/TPartnerSetupWebConnector_LoadPublications',r).then(function (data) {
		data = JSON.parse(data.data.d);
		// on reload, clear content
		last_requested_data = data.result.PPublication;
		$('#browse_container').html('');
		for (item of data.result.PPublication) {
			// format a abo for every entry
			format_item(item);
		}
	})
}

function format_item(item) {
	let row = format_tpl($("[phantom] .tpl_row").clone(), item);
	let view = format_tpl($("[phantom] .tpl_view").clone(), item);
	$('#browse_container').append(row);
	$('#publication'+item['p_publication_code_c']).find('.collapse_col').append(view);
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
  if (!allow_modal()) {return}
  let z = null;
  for (sub of last_requested_data) {
    if (sub.p_publication_code_c == sub_id) {
      z = sub;
      break;
    }
  }

  var f = format_tpl( $('[phantom] .tpl_edit').clone(), z);
  $('#modal_space').html(f);
	$('#modal_space .modal').modal('show');




}

function open_new() {
  if (!allow_modal()) {return}
  let n_ = $('[phantom] .tpl_new').clone();
  $('#modal_space').html(n_);
	$('#modal_space .modal').modal('show');
}

function save_new() {
    if (!allow_modal()) {return}
    let se = $('#modal_space .modal').modal('show');
    let request = translate_to_server(extract_data(se));

    request['action'] = 'create';

    api.post("serverMPartner.asmx/TPartnerSetupWebConnector_MaintainPublications", request).then(
      function () {
        utils.display_message(i18next.t('MaintainPublications.confirm_create'), 'success');
				se.modal('hide');
				display_list();
			}
    )

}

function save_entry(update) {
  let raw = $(update).closest('.modal');
  let request = translate_to_server(extract_data(raw));

  request['action'] = 'update';

  api.post("serverMPartner.asmx/TPartnerSetupWebConnector_MaintainPublications", request).then(
    function () {
      utils.display_message(i18next.t('MaintainPublications.confirm_edit'), 'success');
			raw.modal('hide');
			display_list();
		}
  )
}

function delete_entry(d) {
  let raw = $(d).closest('.modal');
  let request = translate_to_server(extract_data(raw));

  request['action'] = 'delete';

  let s = confirm( i18next.t('MaintainPublications.ask_delete') );
  if (!s) {return}

  api.post("serverMPartner.asmx/TPartnerSetupWebConnector_MaintainPublications", request).then(
		function (data) {
			parsed = JSON.parse(data.data.d);
			if (parsed.result == true) {
				utils.display_message(i18next.t('forms.deleted'), 'success');
				raw.modal('hide');
				display_list();
			} else {
				utils.display_error( i18next.t('forms.notdeleted') );
			}
		}
  );

}
