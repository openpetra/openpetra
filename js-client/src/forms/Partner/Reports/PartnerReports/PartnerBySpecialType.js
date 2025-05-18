// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
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
import reports from '../../../../lib/reports.js'
import api from '../../../../lib/ajax.js'

class PartnerBySpecialType {
	constructor() {
	}

	Ready() {
		let self = this;
		self.loadInConsents();
		api.post('serverMPartner.asmx/TPartnerSetupWebConnector_LoadPartnerTypes', {}).then(function (data) {
			let parsed = JSON.parse(data.data.d);
			self.display_report_form(parsed);
		})
	}

	display_report_form(parsed) {
		let self = this;
		// generated fields
		self.load_tags(parsed.result.PType, $('#reportfilter'));
	}

	calculate_report() {
		let obj = $('#reportfilter');
		// extract information from a jquery object
		let params = tpl.extract_data(obj);

		// get all tags for the partner
		applied_tags = []
		obj.find('#types').find('.tpl_check').each(function (i, o) {
			o = $(o);
			if (o.find('input').is(':checked')) {
				applied_tags.push(o.find('data').attr('value'));
			}
		});

		params['param_explicit_specialtypes'] = applied_tags;
		params['param_today'] = new Date();

		reports.calculate_report_common("forms/Partner/Reports/PartnerReports/PartnerBySpecialType.json", params);
	}

	loadInConsents() {
		api.post('serverMPartner.asmx/TDataHistoryWebConnector_GetConsentChannelAndPurpose', {}).then(function (data) {
			var parsed = JSON.parse(data.data.d);
			var Consents = $(`#reportfilter [consents]`);
			for (var purpose of parsed.result.PConsentPurpose) {
				let name = i18next.t('MaintainPartners.'+purpose.p_name_c, purpose.p_name_c);
				var ConsentTemp = $(`[phantom] .consent-option`).clone();
				ConsentTemp.find(".name").text(name);
				ConsentTemp.find("[name=param_consent]").attr("value", purpose.p_purpose_code_c);
				Consents.append(ConsentTemp);
			}
		})
	}

	// used to load all available tags
	load_tags(all_tags, obj) {
		let p = $('<div class="container">');
		for (var tag of all_tags) {
			let pe = $('[phantom] .tpl_check').clone();
			pe.find('data').attr('value', tag['p_type_code_c']);
			pe.find('span').text(tag['p_type_description_c']);
			p.append(pe);
		}
		obj.find('#types').html(p);
		return obj;
	}
}

export default new PartnerBySpecialType();