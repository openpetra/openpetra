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

var last_opend_entry_data = {};

$('document').ready(function () {
	diplay_partners();
});

function diplay_partners() {
	let x = extract_data($('#tabfilter'));
	api.post('serverMPartner.asmx/TSimplePartnerFindWebConnector_FindPartners', x).then(function (data) {
		data = JSON.parse(data.data.d);
		// on reload, clear content
		$('#browse_container').html('');
		for (user of data.result) {
			// format a user for every entry
			format_user(user);
		}
	})
}

function format_user(user) {
	let n = format_tpl($("[phantom] .tpl_row").clone(),user);
	let c = format_tpl($("[phantom] .tpl_view").clone(),user);
	n.find('.collapse_col').append(c);
	$('#browse_container').append(n);
}

function open_detail(obj) {
	obj = $(obj);
	if (obj.find('.collapse').is(':visible') ) {return}
	$('.tpl_row .collapse').collapse('hide');
	obj.find('.collapse').collapse('show')
}

function open_edit(partner_id) {
	var r = {
				APartnerKey: partner_id,
				AWithAddressDetails: true,
				AWithSubscriptions: true,
				AWithRelationships: true
			};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we dont have to load it, if we only search
	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_GetPartnerDetails', r).then(function (data) {
		actuall_data_str = data.data.d;
		parsed = JSON.parse(actuall_data_str);

		// make a deep copy of the server data and set it as a global var.
		last_opend_entry_data = $.extend(true, {}, parsed);
		let m = $('[phantom] .tpl_edit').clone();
		// normal info input
		m = format_tpl(m ,parsed.result.PLocation[0], "PLocation_");
		m = format_tpl(m ,parsed.result.PPartner[0],"PPartner_");
		m = format_tpl(m ,parsed.result.PUnit[0],"PUnit_");
		m = format_tpl(m ,parsed.result.PFamily[0],"PFamily_");

		// generated fields
		m = load_tags(parsed.result.PType, parsed.result.PPartnerType, m);
		m = load_subs(parsed.result.PPublication, parsed.result.PSubscription, m);

		m.find('.select_case').hide();
		m.find('.'+parsed.result.PPartner[0].p_partner_class_c).show();
		$('#modal_space').html(m);
		$('#modal_space .modal').modal('show');
	})
}

function save_entry(obj_modal) {
	let obj = $(obj_modal).closest('.modal');

	// extract informations from a jquery object
	let x = extract_data(obj);

	// replace all new information in the original data
	let updated_data = replace_data(last_opend_entry_data, x);

	// get all tags for the partner
	applyed_tags = []
	obj.find('#types').find('.tpl_tag').each(function (i, o) {
		o = $(o);
		if (o.find('input').is(':checked')) {
			applyed_tags.push(o.find('data').attr('value'));
		}
	});

	// get all subbed options
	applyed_subs = []
	obj.find('#subscriptions').find('.tpl_tag').each(function (i, o) {
		o = $(o);
		if (o.find('input').is(':checked')) {
			applyed_subs.push(o.find('data').attr('value'));
		}
	});

	// set tags and subs to the right place
	updated_data.result.PPartnerType = applyed_tags;
	updated_data.result.PSubscription = applyed_subs;

	// send request
	let r = {'AMainDS': JSON.stringify(updated_data.result)};
	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_SavePartner', r).then(function (data) {
		$('#modal_space .modal').modal('hide');
		display_message(i18next.t('forms.saved'), "success");
		diplay_partners();
	})
}


function show_tab(tab_id) {
	// used to controll tabs in modal, bacause bootstrap hates me :c
	let tab = $(tab_id);
	let target = tab.attr('aria-controls');
	tab.closest('.nav-tabs').find('.nav-link').removeClass('active');
	tab.addClass('active');

	tgr = tab.closest('.container').find('.tab-content');
	tgr.find('.tab-pane').removeClass('active');
	tgr.find('#'+target).addClass('active');

}

// used to load all avaliable tags and set checkbox if needed
function load_tags(all_tags, selected_tags, obj) {
	let p = $('<div class="container">');
	for (tag of all_tags) {
		let pe = $('[phantom] .tpl_tag').clone();
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

// same as: load_tags just with subs
function load_subs(all_subs, selected_subs, obj) {
	let p = $('<div class="container">');
	for (tag of all_subs) {
		let pe = $('[phantom] .tpl_tag').clone();
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
