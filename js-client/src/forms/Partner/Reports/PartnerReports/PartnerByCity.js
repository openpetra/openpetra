// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <tp@tbits.net>
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

var last_opened_entry_data = {};

function calculate_report() {
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

	params['param_today'] = new Date();

	calculate_report_common("forms/Partner/Reports/PartnerReports/PartnerByCity.json", params);
}

function loadInConsents() {
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

$("document").ready(function () {
	loadInConsents();
})
