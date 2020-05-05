// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <tp@tbits.net>
//       Christopher Jäkel <cj@tbits.net>
//
// Copyright 2017-2018 by TBits.net
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

var last_opened_entry_data = {};
var data_changes_log = {};

$('document').ready(function () {
	load_preset();

	if (window.location.href.endsWith('?NewFamily')) {
		open_new_family();
	} else if (window.location.href.endsWith('?NewOrganisation')) {
		open_new_organisation();
	} else {
		display_list();
	}
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
	if (!allow_modal()) {return}
	// make a deep copy of the server data and set it as a global var.
	last_opened_entry_data = $.extend(true, {}, parsed);
	// reset the changes object
	data_changes_log = {};
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
	m = load_countries(parsed.result.PCountry, parsed.result.PLocation[0].p_country_code_c, m);

	var sendmail = false;
	if (parsed.result.PPartnerLocation.length > 0) {
		sendmail = parsed.result.PPartnerLocation[0].p_send_mail_l;
	}

	m = format_tpl(m,
		{'p_default_email_address_c': parsed.ADefaultEmailAddress,
		'p_default_phone_landline_c': parsed.ADefaultPhoneLandline,
		'p_default_phone_mobile_c': parsed.ADefaultPhoneMobile,
		'p_send_mail_l': sendmail},
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
	last_opened_entry_data.p_default_email_address_c = last_opened_entry_data.ADefaultEmailAddress;
	last_opened_entry_data.p_default_phone_landline_c = last_opened_entry_data.ADefaultPhoneLandline;
	last_opened_entry_data.p_default_phone_mobile_c = last_opened_entry_data.ADefaultPhoneMobile;

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
	updated_data.result.PCountry = [];

	// to be save we have the right address in logs
	if (data_changes_log["address"] != null) { data_changes_log["address"]["Value"] = getUpdatesAddress(); }
	var applied_perms = [];
	for (var perm of Object.values(data_changes_log) ) {
		if (perm["Valid"] == false) { return display_message( i18next.t("MaintainPartners.consent_error"), 'fail'); }
		applied_perms.push( JSON.stringify(perm) );
	}

	// send request
	let r = {'AMainDS': JSON.stringify(updated_data.result),
			 'APartnerTypes': applied_tags,
			 'ASubscriptions': applied_subs,
			 'AChanges': applied_perms,
			 'ADefaultEmailAddress': updated_data.p_default_email_address_c,
			 'ADefaultPhoneLandline': updated_data.p_default_phone_landline_c,
			 'ADefaultPhoneMobile': updated_data.p_default_phone_mobile_c
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
	tgr.find('.tab-pane').hide();
	tgr.find('#'+target).show();

}

// used to load all available tags and set checkbox if needed
function load_tags(all_tags, selected_tags, obj) {
	let p = $('<div class="container"></div>');
	emtpy = true;
	for (tag of all_tags) {
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
function load_subs(all_subs, selected_subs, obj) {
	let p = $('<div class="container"></div>');
	empty = true;
	for (tag of all_subs) {
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

function load_countries(all_countries, selected_country, obj) {
	if (selected_country == null) selected_country="99";
	for (country of all_countries) {
		selected = (selected_country == country.p_country_code_c)?" selected":"";
		let y = $('<option value="'+country.p_country_code_c+'"' + selected + '>'+country.p_country_code_c + " " + country.p_country_name_c + '</option>');
		obj.find('#CountryCode').append(y);
	}
	return obj;
}

// following funtions are for data history view/edit
function open_history(HTMLButton) {
	var req = {
		APartnerKey: $(HTMLButton).closest(".modal").find("[name=p_partner_key_n]").val()
	};
	data_changes_log = {};
	api.post('serverMPartner.asmx/TDataHistoryWebConnector_GetUniqueTypes', req).then(function (data) {
		var parsed = JSON.parse(data.data.d);
		let Temp = $('[phantom] .tpl_history').clone();

		var DataTypeList = Temp.find("[data-types]");
		for (var type of parsed.result) {
			let name = i18next.t('MaintainPartners.'+type);
			let partner = req["APartnerKey"];
			DataTypeList.append(`<button class='btn btn-secondary' onclick='load_history_data(this)' data-partner='${partner}' data-type='${type}' style='width:100%; margin:2px;'>${name}</button>`);
		}

		$('#modal_space .modal').modal('hide');
		$('#modal_space').append(Temp);
		$('#modal_space .modal').modal('show');

	})
}

function load_history_data(HTMLButton) {
	var req = {
		APartnerKey: $(HTMLButton).attr("data-partner"),
		ADataType: $(HTMLButton).attr("data-type")
	};
	var Target = $("#modal_space .tpl_history");

	// we also store infos about the current edit user in the button thats needs to be pressed when saving changes
	$("button#consent_edit_btn").attr("data-partner", req.APartnerKey);
	$("button#consent_edit_btn").attr("data-type", req.ADataType);

	api.post('serverMPartner.asmx/TDataHistoryWebConnector_GetHistory', req).then(function (data) {
		var parsed = JSON.parse(data.data.d);

		var channels = parsed.result.PConsentChannel;
		var purposes = parsed.result.PPurpose;

		let type = $(HTMLButton).attr("data-type");
		type = i18next.t('MaintainPartners.'+type);
		Target.find(".selected-type").text(type);
		var HistoryList = Target.find("[history]").html("");
		for (var entry of parsed.result.PDataHistory) {
			var HistPerm = $("[phantom] .history-entry").clone();

			var EventDate = new Date(entry.s_date_created_d);
			var event_date_str = `${EventDate.getFullYear()}-${(EventDate.getMonth()+1)}-${EventDate.getDate()}`;

			entry.AllowedPurposes = entry.AllowedPurposes ? entry.AllowedPurposes : "-"; // be sure there is something
			HistPerm.find(".preview [name=Value]").text(entry.p_value_c);
			HistPerm.attr("style", "background-color: #EEEEEE");
			HistPerm.find(".preview [name=EventDate]").text( event_date_str );
			HistPerm.find(".preview [name=Permissions]").text( entry.AllowedPurposes );

			HistPerm.find(".detail [name=Editor]").text( entry.s_created_by_c );
			HistPerm.find(".detail [name=Channel]").text( i18next.t('MaintainPartners.'+entry.p_channel_code_c) );
			for (var channel of channels) {
				if (entry.p_channel_code_c == channel.p_channel_code_c) {
					HistPerm.find(".detail [name=Channel]").text( i18next.t('MaintainPartners.'+channel.p_name_c) );
				}
			}
			for (var purpose of purposes) {
				if (entry.AllowedPurposes.split(',').indexOf(purpose.p_purpose_code_c) >= 0) {
					HistPerm.find(".detail [name=Consent]").append( "<br><span>" + i18next.t('MaintainPartners.'+purpose.p_name_c) + "</span>" );
				}
			}

			HistoryList.append(HistPerm);
		}

	})

	api.post('serverMPartner.asmx/TDataHistoryWebConnector_LastKnownEntry', req).then(function (data) {
		var parsed = JSON.parse(data.data.d);
		var TargetPurpose = Target.find("[permissions]").html("");

		var purposes = parsed.result.PPurpose;
		var last_known_configuration = parsed.result.PDataHistory.pop(); // could be empty
		if (last_known_configuration.AllowedPurposes == null) { last_known_configuration["AllowedPurposes"]=""; }

		for (var purpose of purposes) {
			let checked = (last_known_configuration.AllowedPurposes.split(',').indexOf(purpose.p_purpose_code_c) >= 0) ? "checked" : null;
			let name = i18next.t('MaintainPartners.'+purpose.p_name_c);

			var PermTemp = $("[phantom] .permission-option").clone();
			PermTemp.find("[name]").text(name);
			PermTemp.find("[purposecode]").attr("purposecode", purpose.p_purpose_code_c);
			PermTemp.find("[purposecode]").attr("checked", checked);
			TargetPurpose.append(PermTemp);
		}
	});
}

function insert_consent(HTMLField, data_name) {

	var Obj = $(HTMLField);
	var value = Obj.val();
	var partner_key = Obj.closest(".modal").find("[name=p_partner_key_n]").val();

	// special cases
	if (data_name == "address") { value = getUpdatesAddress(); }

	data_changes_log[data_name] = {
		PartnerKey: partner_key,
		Type: data_name,
		Value: value,
		ChannelCode: "", // is set later
		Permissions: "", // is set later
		Valid: false // is set later
	};

	open_consent_modal(data_name);

}

function open_consent_modal(field, mode="partner_edit") {

	var partner_key = $("#modal_space .tpl_edit [name=p_partner_key_n]").val();
	var req = {APartnerKey: partner_key, ADataType:field};
	$("#modal_space .tpl_edit #history_button").attr("disabled", true);

	api.post('serverMPartner.asmx/TDataHistoryWebConnector_LastKnownEntry', req).then(function (data) {
		parsed = JSON.parse(data.data.d);
		var Temp = $('[phantom] .tpl_consent').clone();

		var purposes = parsed.result.PPurpose;
		var channels = parsed.result.PConsentChannel;
		var last_known_configuration = parsed.result.PDataHistory.pop() || {}; // could be empty

		// because it could be empty
		if (last_known_configuration.AllowedPurposes == null) { last_known_configuration["AllowedPurposes"]=""; }

		Temp.find("data[name=field]").val(field);
		Temp.find("[name=changed_value]").text(i18next.t(`MaintainPartners.${field}`));

		// place dynamic channel
		var TargetChannel = Temp.find("[name=consent_channel]").html("");
		for (var channel of channels) {
			let selected = (channel.p_channel_code_c == last_known_configuration.p_channel_code_c) ? "selected" : "";
			let name = i18next.t('MaintainPartners.'+channel.p_name_c);
			TargetChannel.append(`<option ${selected} value='${channel.p_channel_code_c}'>${name}</option>`);
		}

		// place dynamic purposes
		var TargetPurpose = Temp.find(".permissions").html("");
		for (var purpose of purposes) {
			let checked = (last_known_configuration.AllowedPurposes.split(',').indexOf(purpose.p_purpose_code_c) >= 0) ? "checked" : null;
			let name = i18next.t('MaintainPartners.'+purpose.p_name_c);

			var PermTemp = $("[phantom] .permission-option").clone();
			PermTemp.find("[name]").text(name);
			PermTemp.find("[purposecode]").attr("purposecode", purpose.p_purpose_code_c);
			PermTemp.find("[purposecode]").attr("checked", checked);
			TargetPurpose.append(PermTemp);
			// TargetPurpose.append(`<label><span>${name}</span><input ${checked} type='checkbox' purposecode='${purpose.p_purpose_code_c}'></label><br>`);
		}

		$('#modal_space .modal.tpl_consent').remove();
		$('#modal_space').append(Temp);

		if (mode == "partner_edit") {
			$("#modal_space .modal.tpl_consent [mode]").hide();
			$("#modal_space .modal.tpl_consent [mode=partner_edit]").show();
		}

		if (mode == "consent_edit") {
			$("#modal_space .modal.tpl_consent [mode]").hide();
			$("#modal_space .modal.tpl_consent [mode=consent_edit]").show();
		}

		$('#modal_space .modal.tpl_consent').modal({backdrop:"static", keyboard: false}); // <- so u can't close it normally

	})
}

function submit_changes_consent() {

	var current_field = $("#modal_space .modal.tpl_consent data[name=field]").val();
	var channel_code = $("#modal_space .modal.tpl_consent [name=consent_channel]").val();

	// get all permissions
	var perm_list = [];
	var perms = $("#modal_space .modal.tpl_consent .permissions input[purposecode]:checked");
	for (var Perm of perms) {
		perm_list.push( $(Perm).attr("purposecode") );
	}

	data_changes_log[current_field]["Valid"] = true;
	data_changes_log[current_field]["ChannelCode"] = channel_code;
	data_changes_log[current_field]["Permissions"] = perm_list.join(',');

	$("#modal_space .modal.tpl_consent").modal("hide");

}

function getUpdatesAddress() {
	let street = $("#modal_space #addresses").find("[name=p_street_name_c]").val();
	let city = $("#modal_space #addresses").find("[name=p_city_c]").val();
	let postal = $("#modal_space #addresses").find("[name=p_postal_code_c]").val();
	let land = $("#modal_space #addresses").find("[name=p_country_code_c]").val();
	return `${street}, ${postal} ${city}, ${land}`;
}

var current_edit_partner_number = "-1";
function submit_consent_edit(from_model=false) {
	if (!from_model) {
		var ty = $("#modal_space #consent_edit_btn").attr("data-type");
		current_edit_partner_number = $("#modal_space #consent_edit_btn").attr("data-partner");
		open_consent_modal(ty, "consent_edit");
		return;
	}

	var perm_list = [];
	var perms = $("#modal_space .modal.tpl_history input[purposecode]:checked");
	for (var Perm of perms) {
		perm_list.push( $(Perm).attr("purposecode") );
	}

	var req = {
		APartnerKey: current_edit_partner_number,
		ADataType: $("#modal_space .modal.tpl_consent data[name=field]").val(),
		AChannelCode: $("#modal_space .modal.tpl_consent [name=consent_channel]").val(),
		AConsentCodes: perm_list.join(',')
	};

	api.post('serverMPartner.asmx/TDataHistoryWebConnector_EditHistory', req).then(function (data) {
		parsed = JSON.parse(data.data.d);
		if (parsed.result) {
			$("#modal_space .tpl_consent").modal("hide");
			var HTMLDataButton = $(`#modal_space .modal.tpl_history button[data-type='${req.ADataType}']`);
			$("#modal_space .tpl_consent").modal("hide");
			load_history_data(HTMLDataButton);
		} else {
			if (parsed.AVerificationResult.message == "np_changes") {
				$("#modal_space .tpl_consent").modal("hide");
			} else {
				display_error( parsed.AVerificationResult );
			}
		}

	})


}
