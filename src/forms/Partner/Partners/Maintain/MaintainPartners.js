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

var last_opened_entry_data = {};

$('document').ready(function () {
	load_preset();
	display_list();
});

function load_preset() {
	var x = window.localStorage.getItem('MaintainPartners');
	if (x != null) {
		x = JSON.parse(x);
		format_tpl($('#tabfilter'), x);
	}
}

function display_list(source) {
	var x = extract_data($('#tabfilter'));
	x['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
	api.post('serverMPartner.asmx/TSimplePartnerFindWebConnector_FindPartners', x).then(function (data) {
		data = JSON.parse(data.data.d);
		// on reload, clear content
		$('#browse_container').html('');
		for (item of data.result) {
			// format a partner for every entry
			format_item(item);
		}
	})
}

function format_item(item) {
	let row = format_tpl($("[phantom] .tpl_row").clone(), item);
	let view = format_tpl($("[phantom] .tpl_view").clone(), item);
	$('#browse_container').append(row);
	$('#partner'+item['p_partner_key_n']).find('.collapse_col').append(view);
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

function open_new_family() {
	r = {
			APartnerClass: "FAMILY"
		};
	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_CreateNewPartner', r ).then(function (data) {
		parsed = JSON.parse(data.data.d);
		display_partner(parsed);
	});
}

function open_new_organisation() {
	r = {
			APartnerClass: "ORGANISATION"
		};
	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_CreateNewPartner', r ).then(function (data) {
		parsed = JSON.parse(data.data.d);
		display_partner(parsed);
	});
}

function open_edit(partner_id) {
	var r = {
				APartnerKey: partner_id,
			};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we don't have to load it, if we only search
	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_GetPartnerDetails', r).then(function (data) {
		parsed = JSON.parse(data.data.d);
		display_partner(parsed);
	})
}

function display_partner(parsed) {
	// make a deep copy of the server data and set it as a global var.
	last_opened_entry_data = $.extend(true, {}, parsed);
	let m = $('[phantom] .tpl_edit').clone();
	// normal info input
	m = format_tpl(m ,parsed.result.PLocation[0], "PLocation_");
	m = format_tpl(m ,parsed.result.PPartner[0],"PPartner_");
	if (parsed.result.PFamily != undefined) {
		m = format_tpl(m ,parsed.result.PFamily[0],"PFamily_");
	}
	if (parsed.result.PPerson != undefined) {
		m = format_tpl(m ,parsed.result.PPerson[0],"PPerson_");
	}
	if (parsed.result.POrganisation != undefined) {
		m = format_tpl(m ,parsed.result.POrganisation[0],"POrganisation_");
	}
	if (parsed.result.PUnit != undefined) {
		m = format_tpl(m ,parsed.result.PUnit[0],"PUnit_");
	}
	if (parsed.result.PBank != undefined) {
		m = format_tpl(m ,parsed.result.PBank[0],"PBank_");
	}

	// generated fields
	m = load_tags(parsed.result.PType, parsed.APartnerTypes, m);
	m = load_subs(parsed.result.PPublication, parsed.ASubscriptions, m);
	m = format_tpl(m,
		{'p_default_email_address_c': parsed.ADefaultEmailAddress,
		'p_default_phone_landline_c': parsed.ADefaultPhoneLandline,
		'p_default_phone_mobile_c': parsed.ADefaultPhoneMobile,
		'p_send_mail_l': parsed.result.PPartnerLocation[0].p_send_mail_l},
		null);

	m.find('.select_case').hide();
	m.find('.'+parsed.result.PPartner[0].p_partner_class_c).show();
	$('#modal_space').html(m);
	$('#modal_space .modal').modal('show');
}

function save_entry(obj_modal) {
	let obj = $(obj_modal).closest('.modal');

	// extract information from a jquery object
	let x = extract_data(obj);

	// replace all new information in the original data
	let updated_data = replace_data(last_opened_entry_data, x);

	// get all tags for the partner
	applied_tags = []
	obj.find('#types').find('.tpl_check').each(function (i, o) {
		o = $(o);
		if (o.find('input').is(':checked')) {
			applied_tags.push(o.find('data').attr('value'));
		}
	});

	// get all subscribed options
	applied_subs = []
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

	// send request
	let r = {'AMainDS': JSON.stringify(updated_data.result),
			 'APartnerTypes': applied_tags,
			 'ASubscriptions': applied_subs,
			 'ADefaultEmailAddress': updated_data.ADefaultEmailAddress,
			 'ADefaultPhoneLandline': updated_data.ADefaultPhoneLandline,
			 'ADefaultPhoneMobile': updated_data.ADefaultPhoneMobile
			 };

	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_SavePartner', r).then(function (data) {
		parsed = JSON.parse(data.data.d);
		if (parsed.result == true) {
			$('#modal_space .modal').modal('hide');
			display_message(i18next.t('forms.saved'), "success");
			display_list();
		}
		else {
			display_error( parsed.AVerificationResult );
		}
	})
}


function show_tab(tab_id) {
	// used to control tabs in modal, because there are issues with bootstrap
	let tab = $(tab_id);
	let target = tab.attr('aria-controls');
	tab.closest('.nav-tabs').find('.nav-link').removeClass('active');
	tab.addClass('active');

	tgr = tab.closest('.container').find('.tab-content');
	tgr.find('.tab-pane').removeClass('active');
	tgr.find('#'+target).addClass('active');

}

// used to load all available tags and set checkbox if needed
function load_tags(all_tags, selected_tags, obj) {
	let p = $('<div class="container">');
	for (tag of all_tags) {
		let pe = $('[phantom] .tpl_check').clone();
		pe.find('data').attr('value', tag['p_type_code_c']);
		pe.find('span').text(tag['p_type_description_c']);

		if ($.inArray(tag['p_type_code_c'], selected_tags) > -1) {
			pe.find('input').attr('checked', true);
		}
		p.append(pe);
	}
	obj.find('#types').html(p);
	return obj;
}

// same as: load_tags just with subscriptions
function load_subs(all_subs, selected_subs, obj) {
	let p = $('<div class="container">');
	for (tag of all_subs) {
		let pe = $('[phantom] .tpl_check').clone();
		pe.find('data').attr('value', tag['p_publication_code_c']);
		pe.find('span').text(tag['p_publication_code_c']);

		if ($.inArray(tag['p_publication_code_c'], selected_subs) > -1) {
			pe.find('input').attr('checked', true);
		}
		p.append(pe);
	}
	obj.find('#subscriptions').html(p);
	return obj;
}
