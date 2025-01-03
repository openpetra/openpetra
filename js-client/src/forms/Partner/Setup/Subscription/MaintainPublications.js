// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//       Christopher Jäkel
//
// Copyright 2017-2019 by TBits.net
// Copyright 2019-2025 by SolidCharity.com
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
import tpl from '../../../../lib/tpl.js'
import api from '../../../../lib/ajax.js'
import utils from '../../../../lib/utils.js'
import modal from '../../../../lib/modal.js'

class MaintainPublications {

	constructor() {
		this.last_requested_data = {};
	}

	Ready() {
		let self = this;
        if (window.location.href.endsWith('?NewPublication')) {
            self.open_new();
        } else {
            self.display_list();
        }
        $('#btnNew').on('click', function() {self.open_new()});
	}

	display_list() {
		let self = this
		let r = {'ACacheableTable':'PublicationList', 'AHashCode':''};
		api.post('serverMPartner.asmx/TPartnerSetupWebConnector_LoadPublications',r).then(function (data) {
			data = JSON.parse(data.data.d);
			// on reload, clear content
			self.last_requested_data = data.result.PPublication;
			$('#browse_container').html('');
			if (!data.result.PPublication) { return }
			for (var item of data.result.PPublication) {
				// format a abo for every entry
				self.format_item(item);
			}
		})
	}

	self.format_item(item) {
		let self = this
		let row = tpl.format_tpl($("[phantom] .tpl_row").clone(), item);
		let view = tpl.format_tpl($("[phantom] .tpl_view").clone(), item);
		$('#browse_container').append(row);
		$('#publication'+item['p_publication_code_c']).find('.collapse_col').append(view);
		$('#btnEdit'+item['p_publication_code_c']).on('click', function() {self.open_edit(item['p_publication_code_c'])});
		$('#btnView'+item['p_publication_code_c']).on('click', function() {self.open_detail($(this))});
	}

	open_detail(obj) {
		let self = this
		obj = $(obj);
		while(!obj[0].hasAttribute('id') || !obj[0].id.includes("publication")) {
			obj = obj.parent();
		}
		if (obj.find('.collapse').is(':visible') ) {
			$('.tpl_row .collapse').collapse('hide');
			return;
		}
		$('.tpl_row .collapse').collapse('hide');
		obj.find('.collapse').collapse('show')
	}

	open_edit(sub_id) {
		let self = this
		if (!modal.allow_modal()) {return}
		let z = null;
		for (var sub of self.last_requested_data) {
			if (sub.p_publication_code_c == sub_id) {
				z = sub;
				break;
			}
		}

		var f = tpl.format_tpl( $('[phantom] .tpl_edit').clone(), z);
		$('#modal_space').html(f);
		$('#modal_space .modal').modal('show');
		$('#modal_space').find('#btnClose').on('click', function () {modal.CloseModal(this)});
		$('#modal_space').find('#btnSave').on('click', function () {self.save_entry(this)});
		$('#modal_space').find('#btnDelete').on('click', function () {self.delete_entry(this)});
	}

	open_new() {
		let self = this
		if (!modal.allow_modal()) {return}
		let n_ = $('[phantom] .tpl_new').clone();
		$('#modal_space').html(n_);
		$('#modal_space .modal').modal('show');
		$('#modal_space').find('#btnCloseNew').on('click', function () {modal.CloseModal(this)});
		$('#modal_space').find('#btnSaveNew').on('click', function () {self.save_new(this)});
	}

	save_new() {
		let self = this
		if (!modal.allow_modal()) {return}
		let se = $('#modal_space .modal').modal('show');
		let request = utils.translate_to_server(tpl.extract_data(se));

		request['action'] = 'create';

		api.post("serverMPartner.asmx/TPartnerSetupWebConnector_MaintainPublications", request).then(
		function () {
			utils.display_message(i18next.t('MaintainPublications.confirm_create'), 'success');
					se.modal('hide');
					self.display_list();
				}
		)

	}

	save_entry(update) {
		let self = this
		let raw = $(update).closest('.modal');
		let request = utils.translate_to_server(tpl.extract_data(raw));

		request['action'] = 'update';

		api.post("serverMPartner.asmx/TPartnerSetupWebConnector_MaintainPublications", request).then(
			function () {
				utils.display_message(i18next.t('MaintainPublications.confirm_edit'), 'success');
				raw.modal('hide');
				self.display_list();
			}
		)
	}

	delete_entry(d) {
		let self = this
		let raw = $(d).closest('.modal');
		let request = utils.translate_to_server(tpl.extract_data(raw));

		request['action'] = 'delete';

		let s = confirm( i18next.t('MaintainPublications.ask_delete') );
		if (!s) {return}

		api.post("serverMPartner.asmx/TPartnerSetupWebConnector_MaintainPublications", request).then(
				function (data) {
					let parsed = JSON.parse(data.data.d);
					if (parsed.result == true) {
						utils.display_message(i18next.t('forms.deleted'), 'success');
						raw.modal('hide');
						self.display_list();
					} else {
						utils.display_error( i18next.t('forms.notdeleted') );
					}
				}
		);
	}


} // end of class

export default new MaintainPublications();