// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//	   Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2017-2018 by TBits.net
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
import autocomplete_motivation from '../../../../lib/autocomplete_motivation.js'
import autocomplete_acc_cc from '../../../../lib/autocomplete_posting_acc_cc.js'

class Motivations {

	constructor() {
		this.new_entry_data = {};
	}

	Ready() {
		let self = this;
		self.display_list();
		$('#btnNewGroup').on('click', function() {self.new_group()});
		$('#txtDefaultDetail').on('input', function() {autocomplete_motivation.autocomplete_motivation_detail(this)});
		$('#btnSaveDefaultDetail').on('click', function() {self.save_default_detail(this)});
	}

	display_list() {
		let self = this;
		// x is search
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			AMotivationGroupCode: ''
		};

		api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', x).then(function (data) {
			data = JSON.parse(data.data.d);
			// on reload, clear content
			$('#browse_container').html('');
			let x = {
					'a_motivation_group_code_c': data.ADefaultMotivationGroup,
					'a_motivation_detail_code_c': data.ADefaultMotivationDetail
					}
			tpl.insertData($("#toolbar"), x);
			for (var item of data.result.AMotivationGroup) {
				self.format_item(item);
			}
		})
	}

	install_group_click_handlers(groupcode) {
		let self = this;
		$('#btnEdit'+groupcode).on('click', function() {self.edit_group(groupcode)});
		$('#btnView'+groupcode).on('click', function() {self.open_motivations(this, groupcode)});
		$('#btnGroupCollapse'+groupcode).on('click', function() {self.open_motivations(this, groupcode)});
		$('#btnNewDetail'+groupcode).on('click', function() {self.new_motivation(groupcode)});
	}

	update_group(GroupCode) {
		let self = this;
		GroupCode = GroupCode.toUpperCase();
		let x = {
			ALedgerNumber: window.localStorage.getItem('current_ledger'),
			AMotivationGroupCode: GroupCode
		};

		// we would need LoadMotivationGroupDetails
		api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', x).then(function (data) {
			data = JSON.parse(data.data.d);
			let item = data.result.AMotivationGroup[0];
			let groupDiv = $('#group' + GroupCode);
			let row = tpl.format_tpl($("[phantom] .tpl_row").clone(), item);
			groupDiv.replaceWith(row);
			self.install_group_click_handlers(item['a_motivation_group_code_c']);
			// we did already download the details, but they are not rendered yet
			setTimeout(function() { self.open_motivations($('#group' + GroupCode), GroupCode, true) }, 400);
		});
	}

	format_item(item) {
		let self = this;
		let row = tpl.format_tpl($("[phantom] .tpl_row").clone(), item);
		$('#browse_container').append(row);
		self.install_group_click_handlers(item['a_motivation_group_code_c']);
	}

	open_motivations(obj, code, reload = false) {
		let self = this;
		obj = $(obj);
		while(!obj[0].hasAttribute('id') || !obj[0].id.includes("group")) {
			obj = obj.parent();
		}
		if (obj.find('.collapse').is(':visible') ) {
			$('.tpl_row .collapse').collapse('hide');
			return;
		}
		let x = {
			ALedgerNumber:window.localStorage.getItem('current_ledger'),
			AMotivationGroupCode: code
		};
		api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', x).then(function (data) {
			data = JSON.parse(data.data.d);

			// on open, clear content
			let place_to_put_content = obj.find('.content_col').html('');
			if (data.result.hasOwnProperty('AMotivationDetail')) {
				for (var item of data.result.AMotivationDetail) {

					for (var account of data.result.AAccount) {
						if (account['a_account_code_c'] == item['a_account_code_c']) {
							item['account_name'] = account['a_account_code_long_desc_c'];
						}
					}
					for (var costcentre of data.result.ACostCentre) {
						if (costcentre['a_cost_centre_code_c'] == item['a_cost_centre_code_c']) {
							item['costcentre_name'] = costcentre['a_cost_centre_name_c'];
						}
					}

					let motivation_row = $('[phantom] .tpl_motivation').clone();
					motivation_row = tpl.format_tpl(motivation_row, item);
					place_to_put_content.append(motivation_row);

					obj.find('#btnEditDetail'+item['a_motivation_group_code_c']+item['a_motivation_detail_code_c']).
						on('click', function () {self.edit_motivation(item['a_motivation_group_code_c'], item['a_motivation_detail_code_c'])});
				}
			}
			if (!reload) {
				$('.tpl_row .collapse').collapse('hide');
			}
			obj.find('.collapse').collapse('show');
		})

	}

	/////

	new_group() {
		let self = this;
		if (!modal.allow_modal()) {return}
		let x = {a_ledger_number_i :window.localStorage.getItem('current_ledger')};
		// console.log(parsed);
		let p = tpl.format_tpl( $('[phantom] .tpl_edit_group').clone(), x );
		$('#modal_space').html(p);
		p.find('[edit-only]').hide();
		p.find('input[name=a_motivation_group_code_c]').attr('readonly', false);
		p.find('[action]').val('create');
		p.modal('show');
		$('#modal_space').find('#btnGroupClose').on('click', function () {modal.CloseModal(this)});
		$('#modal_space').find('#btnGroupSave').on('click', function () {self.save_edit_group(this)});
	};

	new_motivation(group_code) {
		let self = this;
		if (!modal.allow_modal()) {return}
		let x = {a_ledger_number_i:window.localStorage.getItem('current_ledger'), a_motivation_group_code_c:group_code};
		let p = tpl.format_tpl( $('[phantom] .tpl_edit_motivation').clone(), x);
		$('#modal_space').html(p);
		p.find('input[name=a_motivation_detail_code_c]').attr('readonly', false);
		p.find('input[name=a_tax_decuctible_l]').attr('checked', true);
		p.find('input[name=a_motivation_status_l]').attr('checked', true);
		p.find('[edit-only]').hide();
		p.find('[action]').val('create');
		p.modal('show');
		$('#modal_space').find('#btnDetailSave').on('click', function () {self.save_edit_detail(this)});
		$('#modal_space').find('#btnDetailClose').on('click', function () {modal.CloseModal(this)});
		$('#modal_space').find('#txtAccount').on('input', function () {autocomplete_acc_cc.autocomplete_a(this)});
		$('#modal_space').find('#txtCostCentre').on('input', function () {autocomplete_acc_cc.autocomplete_cc(this)});
	};

	/////

	edit_group(group_code) {
		let self = this;
		if (!modal.allow_modal()) {return}
		var r = {
					ALedgerNumber: window.localStorage.getItem('current_ledger'),
					AMotivationGroupCode: group_code
				};
		// on open of a edit modal, we get new data,
		// so everything is up to date and we don't have to load it, if we only search
		api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', r).then(function (data) {
			let parsed = JSON.parse(data.data.d)
			let f = null;
			for (var group of parsed.result.AMotivationGroup) {
				if (group.a_motivation_group_code_c == group_code) {
					f = group
				}
			}
			let tpl_m = tpl.format_tpl( $('[phantom] .tpl_edit_group').clone(), f );
			$('#modal_space').html(tpl_m);
			tpl_m.find('[action]').val('update');
			tpl_m.modal('show');

			$('#modal_space').find('#btnGroupClose').on('click', function () {modal.CloseModal(this)});
			$('#modal_space').find('#btnGroupSave').on('click', function () {self.save_edit_group(this)});
			$('#modal_space').find('#btnGroupDelete').on('click', function () {self.delete_group(this)});
		})
	}

	edit_motivation(group_id, detail_id) {
		let self = this;
		if (!modal.allow_modal()) {return}
		let x = {
				"ALedgerNumber":window.localStorage.getItem('current_ledger'),
				"AMotivationGroupCode":group_id
				};
		api.post('serverMFinance.asmx/TGiftSetupWebConnector_LoadMotivationDetails', x).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			let searched = null;
			for (var moti of parsed.result.AMotivationDetail) {
				if (moti.a_motivation_group_code_c == group_id && moti.a_motivation_detail_code_c == detail_id) {
					searched = moti;
					break;
				}
			}
			if (searched == null) {
				return alert('ERROR');
			}

			searched['a_account_name_c'] = searched['a_account_code_c'];
			searched['a_cost_center_name_c'] = searched['a_cost_centre_code_c'];

			let tpl_motivation = tpl.format_tpl( $('[phantom] .tpl_edit_motivation').clone(), searched );

			$('#modal_space').html(tpl_motivation);
			tpl_motivation.find('[action]').val('update');
			$('#modal_space .modal').modal('show');
			$('#modal_space').find('#btnDetailDelete').on('click', function () {self.delete_motivation(this)});
			$('#modal_space').find('#btnDetailSave').on('click', function () {self.save_edit_detail(this)});
			$('#modal_space').find('#btnDetailClose').on('click', function () {modal.CloseModal(this)});
		})
	}

	save_edit_group(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		let mode = obj.find('[action]').val();

		// extract information from a jquery object
		let payload = utils.translate_to_server( tpl.extract_data(obj) );
		payload['action'] = mode;
		api.post('serverMFinance.asmx/TGiftSetupWebConnector_MaintainMotivationGroups', payload).then(function (result) {
			let parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				utils.display_message(i18next.t('forms.saved'), "success");
				modal.CloseModal(obj);
				self.update_group(payload['AMotivationGroupCode']);
				//self.display_list();
			} else {
				for (var msg of parsed.AVerificationResult) {
					utils.display_message(i18next.t(msg.code), "fail");
				}
			}
		});
	}

	save_edit_detail(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		let mode = obj.find('[action]').val();

		// extract information from a jquery object
		let payload = utils.translate_to_server( tpl.extract_data(obj) );
		payload['action'] = mode;

		api.post('serverMFinance.asmx/TGiftSetupWebConnector_MaintainMotivationDetails', payload).then(function (result) {
			let parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				utils.display_message(i18next.t('forms.saved'), "success");
				modal.CloseModal(obj);
				self.update_group(payload['AMotivationGroupCode']);
				//self.display_list();
			} else {
				for (var msg of parsed.AVerificationResult) {
					utils.display_message(i18next.t(msg.code), "fail");
				}
				utils.display_message(i18next.t('forms.notsaved'), "fail");
			}
		});
	}


	delete_group(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		let payload = utils.translate_to_server( tpl.extract_data(obj) );
		payload['action'] = 'delete';

		let s = confirm( i18next.t('Motivations.ask_delete_group') );
		if (!s) {return}

		api.post('serverMFinance.asmx/TGiftSetupWebConnector_MaintainMotivationGroups', payload).then(function (result) {
			let parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				utils.display_message(i18next.t('forms.deleted'), "success");
				modal.CloseModal(obj);
				self.display_list();
			} else {
				for (var msg of parsed.AVerificationResult) {
					utils.display_message(i18next.t(msg.code), "fail");
				}
				utils.display_message(i18next.t('forms.notdeleted'), "fail");
			}
		});
	}

	delete_motivation(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		let payload = utils.translate_to_server( tpl.extract_data(obj) );
		payload['action'] = 'delete';

		let s = confirm( i18next.t('Motivations.ask_delete_detail') );
		if (!s) {return}

		api.post('serverMFinance.asmx/TGiftSetupWebConnector_MaintainMotivationDetails', payload).then(function (result) {
			let parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				utils.display_message(i18next.t('forms.deleted'), "success");
				modal.CloseModal(obj);
				//self.display_list();
				self.update_group(payload['AMotivationGroupCode']);
			} else {
				for (var msg of parsed.AVerificationResult) {
					utils.display_message(i18next.t(msg.code), "fail");
				}
				utils.display_message(i18next.t('forms.notdeleted'), "fail");
			}
		});
	}

	save_default_detail(obj) {
		let self = this;
		let payload = utils.translate_to_server( tpl.extract_data($(obj).closest('#toolbar')));
		payload['ALedgerNumber'] = window.localStorage.getItem('current_ledger');

		api.post('serverMFinance.asmx/TGiftSetupWebConnector_SetDefaultMotivationDetail', payload).then(function (result) {
			let parsed = JSON.parse(result.data.d);
			if (parsed.result == true) {
				utils.display_message(i18next.t('forms.saved'), "success");
			} else {
				for (var msg of parsed.AVerificationResult) {
					utils.display_message(i18next.t(msg.code), "fail");
				}
				utils.display_message(i18next.t('forms.notsaved'), "fail");
			}
		});
	}

} // end of class

export default new Motivations();