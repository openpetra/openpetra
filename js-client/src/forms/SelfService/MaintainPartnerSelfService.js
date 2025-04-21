// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//       Christopher Jäkel
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
import tpl from '../../lib/tpl.js'
import api from '../../lib/ajax.js'
import utils from '../../lib/utils.js'
import modal from '../../lib/modal.js'

class MaintainPartnerSelfService {

	constructor() {
		this.last_opened_entry_data = {};
	}

	Ready() {
		this.open_edit();
	}

	show_tab(tab_id) {
		// used to control tabs in modal, because there are issues with bootstrap
		let tab = $(tab_id);
		let target = tab.attr('aria-controls');
		tab.closest('.nav-tabs').find('.nav-link').removeClass('active');
		tab.addClass('active');

		let tgr = tab.closest('.container').find('.tab-content');
		tgr.find('.tab-pane').removeClass('active');
		tgr.find('#'+target).addClass('active');
	}

	open_edit() {
		let self = this;
		var r = {};
		// on open of a edit modal, we get new data,
		// so everything is up to date and we don't have to load it, if we only search
		api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_GetPartnerDetailsSelfService', r).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			self.display_partner(parsed);
		})
	}

	display_partner(parsed) {
		let self = this;
		if (!modal.allow_modal()) {return}
		self.partnerkey = parsed.result.PPartner[0].p_partner_key_n

		// make a deep copy of the server data and set it as a global var.
		self.last_opened_entry_data = $.extend(true, {}, parsed);
		let m = $('[phantom] .tpl_edit').clone();
		// normal info input
		m = tpl.format_tpl(m ,parsed.result.PLocation[0], "PLocation_");
		m = tpl.format_tpl(m ,parsed.result.PPartner[0],"PPartner_");
		if (parsed.result.PFamily != undefined) {
			m = tpl.format_tpl(m ,parsed.result.PFamily[0],"PFamily_");
		}
		if (parsed.result.PPerson != undefined) {
			m = tpl.format_tpl(m ,parsed.result.PPerson[0],"PPerson_");
		}
		if (parsed.result.POrganisation != undefined) {
			m = tpl.format_tpl(m ,parsed.result.POrganisation[0],"POrganisation_");
		}
		if (parsed.result.PUnit != undefined) {
			m = tpl.format_tpl(m ,parsed.result.PUnit[0],"PUnit_");
		}
		if (parsed.result.PBank != undefined) {
			m = tpl.format_tpl(m ,parsed.result.PBank[0],"PBank_");
		}

		// generated fields
		m = self.load_tags(parsed.result.PType, parsed.APartnerTypes, m);
		m = self.load_subs(parsed.result.PPublication, parsed.ASubscriptions, m);
		m = self.load_countries(parsed.result.PCountry, parsed.result.PLocation[0].p_country_code_c, m);

		var sendmail = false;
		if (parsed.result.PPartnerLocation.length > 0) {
			sendmail = parsed.result.PPartnerLocation[0].p_send_mail_l;
		}

		m = tpl.format_tpl(m,
			{'p_default_email_address_c': parsed.ADefaultEmailAddress,
			'p_default_phone_landline_c': parsed.ADefaultPhoneLandline,
			'p_default_phone_mobile_c': parsed.ADefaultPhoneMobile,
			'p_send_mail_l': sendmail},
			null);

		m.find('.select_case').hide();
		m.find('.'+parsed.result.PPartner[0].p_partner_class_c).show();

		let myModal = modal.ShowModal('partneredit' + self.partnerkey, m);
		myModal.find('#btnSavePartner').click(function(){ self.save_partner(this); });
		myModal.find('#btnClosePartner').click(function(){ self.close_partner(this); });
		let elements = $('.nav-tabs .nav-item .nav-link');
		elements.each(function(index) { $(this).on("click", function(e) { self.show_tab(this) })});
	}

	save_partner(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');

		// extract information from a jquery object
		let x = tpl.extract_data(obj);

		// replace all new information in the original data
		self.last_opened_entry_data.p_default_email_address_c = self.last_opened_entry_data.ADefaultEmailAddress;
		self.last_opened_entry_data.p_default_phone_landline_c = self.last_opened_entry_data.ADefaultPhoneLandline;
		self.last_opened_entry_data.p_default_phone_mobile_c = self.last_opened_entry_data.ADefaultPhoneMobile;

		let updated_data = utils.replace_data(self.last_opened_entry_data, x);

		// get all tags for the partner
		let applied_tags = []
		obj.find('#types').find('.tpl_check').each(function (i, o) {
			o = $(o);
			if (o.find('input').is(':checked')) {
				applied_tags.push(o.find('data').attr('value'));
			}
		});

		// get all subscribed options
		let applied_subs = []
		obj.find('#subscriptions').find('.tpl_check').each(function (i, o) {
			o = $(o);
			if (o.find('input').is(':checked')) {
				applied_subs.push(o.find('data').attr('value'));
			}
		});

		// tables we don't want to send
		updated_data.result.PType = [];
		updated_data.result.PPartnerStatus = [];
		updated_data.result.PPublication = [];
		updated_data.result.PCountry = [];
		updated_data.result.PPartnerClasses = [];

		// send request
		let r = {'AMainDS': JSON.stringify(updated_data.result),
				'APartnerTypes': applied_tags,
				'ASubscriptions': applied_subs,
				'ADefaultEmailAddress': updated_data.p_default_email_address_c,
				'ADefaultPhoneLandline': updated_data.p_default_phone_landline_c,
				'ADefaultPhoneMobile': updated_data.p_default_phone_mobile_c
				};

		api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_SavePartnerSelfService', r).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			if (parsed.result == true) {
				modal.CloseModal(obj);
				utils.display_message(i18next.t('forms.saved'), "success");
				self.display_list();
			}
			else {
				utils.display_error( parsed.AVerificationResult );
			}
		})
	}

	close_partner(obj) {
		var self = this;
		let m = modal.FindMyModal(obj)
		modal.CloseModal(m);
		self.display_list();
	}

	// used to load all available tags and set checkbox if needed
	load_tags(all_tags, selected_tags, obj) {
		let p = $('<div class="container"></div>');
		let empty = true;
		for (var tag of all_tags) {
			empty = false;
			let pe = $('[phantom] .tpl_check').clone();
			pe.find('data').attr('value', tag['p_type_code_c']);
			pe.find('span').text(tag['p_type_description_c']);

			if ($.inArray(tag['p_type_code_c'], selected_tags) > -1) {
				pe.find('input').attr('checked', true);
			}
			p.append(pe);
		}
		if (empty)
		{
			let pe = $('[phantom] .tpl_empty_types').clone();
			p.append(pe);
		}
		obj.find('#types').html(p);
		return obj;
	}

	// same as: load_tags just with subscriptions
	load_subs(all_subs, selected_subs, obj) {
		let p = $('<div class="container"></div>');
		let empty = true;
		for (var tag of all_subs) {
			empty = false;
			let pe = $('[phantom] .tpl_check').clone();
			pe.find('data').attr('value', tag['p_publication_code_c']);
			pe.find('span').text(tag['p_publication_code_c']);

			if ($.inArray(tag['p_publication_code_c'], selected_subs) > -1) {
				pe.find('input').attr('checked', true);
			}
			p.append(pe);
		}
		if (empty)
		{
			let pe = $('[phantom] .tpl_empty_subscriptions').clone();
			p.append(pe);
		}

		obj.find('#subscriptions').html(p);
		return obj;
	}

	load_countries(all_countries, selected_country, obj) {
		if (selected_country == null) selected_country="99";
		for (var country of all_countries) {
			let selected = (selected_country == country.p_country_code_c)?" selected":"";
			let y = $('<option value="'+country.p_country_code_c+'"' + selected + '>'+country.p_country_code_c + " " + country.p_country_name_c + '</option>');
			obj.find('#CountryCode').append(y);
		}
		return obj;
	}

} // end of class

export default new MaintainPartnerSelfService();