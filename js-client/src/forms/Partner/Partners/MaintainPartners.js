// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//	   Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//	   Christopher Jäkel
//
// Copyright 2017-2018 by TBits.net
// Copyright 2019-2022 by SolidCharity.com
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
var PBankingDetails_Store = {}

$('document').ready(function () {
	MaintainPartners_Ready();
});

function MaintainPartners_Ready() {
	let obj = new MaintainPartners();

	obj.load_preset();

	if (window.location.href.endsWith('?NewFamily')) {
		obj.open_new_family();
	} else if (window.location.href.endsWith('?NewOrganisation')) {
		obj.open_new_organisation();
	} else {
		obj.display_list();
	}
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

class MaintainPartners {

load_preset() {
	var self = this;

	var x = window.localStorage.getItem('MaintainPartners');
	if (x != null) {
		x = JSON.parse(x);
		format_tpl($('#tabfilter'), x);
	}

	$('#btnSearch').click(function(){ self.display_list(); $('#tabfilter').collapse('toggle'); });
	$('#btnNewFamily').click(function(){ self.open_new_family(); });
	$('#btnNewOrganisation').click(function(){ self.open_new_organisation(); });
}

display_list() {
	var x = extract_data($('#tabfilter'));
	var self = this;
	x['ALedgerNumber'] = window.localStorage.getItem('current_ledger');
	api.post('serverMPartner.asmx/TSimplePartnerFindWebConnector_FindPartners', x).then(function (data) {
		data = JSON.parse(data.data.d);

		if (data.ATotalRecords == -1) {
			display_error( data.AVerificationResult, "MaintainPartners" );
			return;
		}

		// on reload, clear content
		$('#browse_container').html('');
		for (var item of data.result) {
			// format a partner for every entry
			self.format_item(item);

			$('#btnDetailPartner' + item.p_partner_key_n).click(function(){ self.open_detail(this); });
			$('#btnEditPartner' + item.p_partner_key_n).attr('partnerkey', item.p_partner_key_n);
			$('#btnEditPartner' + item.p_partner_key_n).click(function(){ self.open_edit($(this).attr('partnerkey')); });
		}
	})
}

translate_label(label) {
	if (i18next.t('MaintainPartners.'+label) == 'MaintainPartners.'+label) {
		return label;
	} else {
		return i18next.t('MaintainPartners.'+label);
	}
}

format_item(item) {
	let row = format_tpl($("[phantom] .tpl_row").clone(), item);
	let view = format_tpl($("[phantom] .tpl_view").clone(), item);
	$('#browse_container').append(row);
	$('#partner'+item['p_partner_key_n']).find('.collapse_col').append(view);
}

open_detail(obj) {
	obj = $(obj);
	while(!obj[0].hasAttribute('id') || !obj[0].id.includes("partner")) {
		obj = obj.parent();
	}
	if (obj.find('.collapse').is(':visible') ) {
		$('.tpl_row .collapse').collapse('hide');
		return;
	}
	$('.tpl_row .collapse').collapse('hide');
	obj.find('.collapse').collapse('show')
}

open_new_family() {
	var self = this;
	var r = {
			APartnerClass: "FAMILY"
		};
	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_CreateNewPartner', r ).then(function (data) {
		var parsed = JSON.parse(data.data.d);
		self.display_partner(parsed);
	});
}

open_new_organisation() {
	var self = this;
	var r = {
			APartnerClass: "ORGANISATION"
		};
	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_CreateNewPartner', r ).then(function (data) {
		var parsed = JSON.parse(data.data.d);
		self.display_partner(parsed);
	});
}

open_edit(partner_id) {
	var self = this;
	var r = {
				APartnerKey: partner_id,
			};
	// on open of a edit modal, we get new data,
	// so everything is up to date and we don't have to load it, if we only search
	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_GetPartnerDetails', r).then(function (data) {
		var parsed = JSON.parse(data.data.d);
		self.display_partner(parsed);
	})
}

display_partner(parsed) {
	var self = this;
	if (!allow_modal()) {return}
	let partnerkey = parsed.result.PPartner[0].p_partner_key_n

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
	m = this.load_tags(parsed.result.PType, parsed.APartnerTypes, m);
	m = this.load_subs(parsed.result.PPublication, parsed.ASubscriptions, m);
	m = this.load_countries(parsed.result.PCountry, parsed.result.PLocation[0].p_country_code_c, m);

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

	m = this.display_bankaccounts(partnerkey, parsed.result.PBankingDetails, m);

	let myModal = ShowModal('partneredit' + partnerkey, m);

	$('#btnSavePartner' + partnerkey).click(function(){ self.save_entry(this); });
	$('#btnDeletePartner' + partnerkey).click(function(){ self.delete_entry(this); });
	$('#btnAddBankAccount' + partnerkey).attr('partnerkey', partnerkey);
	$('#btnAddBankAccount' + partnerkey).click(function(){
		let partnerkey = $(this).attr('partnerkey');
		let PartnerEditForm = $('#partneredit' + partnerkey);
		self.new_bank_account(partnerkey,
			PartnerEditForm.find("input[name='PFamily_p_first_name_c']").val(),
			PartnerEditForm.find("input[name='PFamily_p_family_name_c']").val());
	});
}

display_bankaccounts(p_partner_key_n, PBankingDetails, m) {
	var self = this;
	PBankingDetails_Store = {}
	m.find('.bank_account_detail_col').html('');
	if (PBankingDetails.length > 0) {
		for (var detail of PBankingDetails) {
			detail['p_partner_key_n'] = p_partner_key_n;
			let tpl_bank_account_detail = format_tpl( $('[phantom] .tpl_account_detail').clone(), detail);
			m.find('.bank_account_detail_col').append(tpl_bank_account_detail);
			PBankingDetails_Store[detail['p_banking_details_key_i']] = detail;
			let btnEditAccount = m.find("#btnEditBankAccount"+detail['p_banking_details_key_i']);
			btnEditAccount.attr('bankingdetailkey', detail['p_banking_details_key_i']);
			btnEditAccount.click(function() {
				let bankingdetailkey = $(this).attr('bankingdetailkey');
				self.edit_account(bankingdetailkey);
			});
		}
	}

	return m;
}

new_bank_account(partnerkey, firstname, lastname) {
	var self = this;
	if (!allow_modal()) {return}
	let x = {
		p_partner_key_n: partnerkey,
		p_account_name_c: (firstname + " " + lastname).trim(),
		p_banking_details_key_i: -1
	};
	let p = format_tpl( $('[phantom] .tpl_edit_bank_account').clone(), x);
	$('#modal_space').append(p);
	p.find('[edit-only]').hide();
	p.find('[action]').val('create');

	p.find('#btnSaveBankAccount'+x.p_banking_details_key_i).click(function(){self.save_edit_bank_account(this)});
	p.find('#btnCloseBankAccount'+x.p_banking_details_key_i).click(function(){CloseModal(this);});

	p.modal('show');
};

edit_account(detail_key) {
	if (!allow_modal()) {return}
	var self = this;

	let data = PBankingDetails_Store[detail_key];
	let tpl_edit_raw = format_tpl( $('[phantom] .tpl_edit_bank_account').clone(), data);

	let p = $('#modal_space').append(tpl_edit_raw);
	p.find('#btnSaveBankAccount'+detail_key).click(function(){self.save_edit_bank_account(this)});
	p.find('#btnDeleteBankAccount'+detail_key).click(function(){self.delete_bank_account(this)});
	p.find('#btnCloseBankAccount'+detail_key).click(function(){CloseModal(this);});
	tpl_edit_raw.find('[action]').val('edit');
	tpl_edit_raw.modal('show');
}

save_entry(obj) {
	var self = this;
	let modal = FindMyModal(obj)
	var partnerkey = modal.find("[name=p_partner_key_n]").val();

	// extract information from a jquery object
	let x = extract_data(modal);

	if (x['p_send_mail_l'] == false) {
		if ((x['p_street_name_c'] != '') && (x['p_postal_code_c'] != '') &&
			(x['p_city_c'] != '') && (x['p_country_code_c'] != '')) {
			if (confirm(i18next.t('MaintainPartners.set_send_mail'))) {
				modal.find("[name=p_send_mail_l]")[0].checked = true;
				x['p_send_mail_l'] = true;
			}
		}
	}

	// replace all new information in the original data
	last_opened_entry_data.p_default_email_address_c = last_opened_entry_data.ADefaultEmailAddress;
	last_opened_entry_data.p_default_phone_landline_c = last_opened_entry_data.ADefaultPhoneLandline;
	last_opened_entry_data.p_default_phone_mobile_c = last_opened_entry_data.ADefaultPhoneMobile;

	let updated_data = replace_data(last_opened_entry_data, x);

	// get all tags for the partner
	let applied_tags = []
	modal.find('#types').find('.tpl_check').each(function (i, o) {
		o = $(o);
		if (o.find('input').is(':checked')) {
			applied_tags.push(o.find('data').attr('value'));
		}
	});

	// get all subscribed options
	let applied_subs = []
	modal.find('#subscriptions').find('.tpl_check').each(function (i, o) {
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
	updated_data.result.PBankingDetails = [];
	updated_data.result.PPartnerBankingDetails = [];

	// to be save we have the right address in logs
	if (data_changes_log["address"] != null) { data_changes_log["address"]["Value"] = this.getUpdatesAddress(partnerkey); }
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
		var parsed = JSON.parse(data.data.d);
		if (parsed.result == true) {
			CloseModal(modal);
			display_message(i18next.t('forms.saved'), "success");
			self.display_list();
		}
		else if (parsed.AVerificationResult[0].code == "consent_error") {
			// probably only the city or postcode was changed, but not the address
			self.insert_consent(obj, 'address');
		}
		else {
			display_error( parsed.AVerificationResult );
		}
	})
}

delete_entry(obj) {
	var self = this;
	let modal = FindMyModal(obj)
	var partnerkey = modal.find("[name=p_partner_key_n]").val();

	let s = confirm( i18next.t('MaintainPartners.ask_delete_contact') );
	if (!s) {return}

	let r = {'APartnerKey': partnerkey}

	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_DeletePartner', r).then(function (data) {
		var parsed = JSON.parse(data.data.d);
		if (parsed.result == true) {
			CloseModal(modal);
			display_message(i18next.t('MaintainPartners.contact_was_deleted'), "success");
			self.display_list();
		}
		else {
			display_error( parsed.AVerificationResult );
		}
	})
}

updateBankAccounts(PartnerKey) {
	var self = this;

	// somehow the original window stays gray when we return from this modal.
	$('.modal-backdrop').remove();
	let m = $('#partneredit' + PartnerKey)

	let payload = {}
	payload['APartnerKey'] = PartnerKey;
	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_GetBankAccounts', payload).then(function (result) {
		let parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			self.display_bankaccounts(PartnerKey, parsed.PBankingDetails, m);
		}
	});
}

save_edit_bank_account(obj_modal) {
	var self = this;
	let obj = $(obj_modal).closest('.modal');
	let mode = obj.find('[action]').val();

	// extract information from a jquery object
	let payload = translate_to_server( extract_data(obj) );
	payload['action'] = mode;
	if (payload['ABankingDetailsKey'] == '') payload['ABankingDetailsKey'] = -1;

	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_MaintainBankAccounts', payload).then(function (result) {
		var parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			CloseModal(obj);
			self.updateBankAccounts(payload['APartnerKey']);
		}
		else if (parsed.result == false) {
			display_error(parsed.AVerificationResult);
		}

	});

}

delete_bank_account(obj_modal) {
	var self = this;

	let s = confirm( i18next.t('MaintainPartners.ask_delete_bank_account') );
	if (!s) {return}

	let obj = $(obj_modal).closest('.modal');
	let payload = translate_to_server( extract_data(obj) );
	payload["action"] = "delete";

	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_MaintainBankAccounts', payload).then(function (result) {
		var parsed = JSON.parse(result.data.d);
		if (parsed.result == true) {
			display_message(i18next.t('forms.saved'), "success");
			CloseModal(obj);
			self.updateBankAccounts(payload['APartnerKey']);
		}
		else if (parsed.result == false) {
			display_error(parsed.AVerificationResult);
		}
	});
}

// used to load all available tags and set checkbox if needed
load_tags(all_tags, selected_tags, obj) {
	let p = $('<div class="container"></div>');
	var empty = true;
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
	var empty = true;
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
		var selected = (selected_country == country.p_country_code_c)?" selected":"";
		let y = $('<option value="'+country.p_country_code_c+'"' + selected + '>'+country.p_country_code_c + " " + country.p_country_name_c + '</option>');
		obj.find('#CountryCode').append(y);
	}
	return obj;
}

// following funtions are for data history view/edit
open_history(HTMLButton) {
	var partnerkey = $(HTMLButton).closest(".modal").find("[name=p_partner_key_n]").val();
	var req = {
		APartnerKey: partnerkey
	};

	api.post('serverMPartner.asmx/TDataHistoryWebConnector_GetUniqueTypes', req).then(function (data) {
		var parsed = JSON.parse(data.data.d);
		let Temp = $('[phantom] .tpl_history').clone();

		var types = [];

		var DataTypeList = Temp.find("[data-types]");
		for (var type of parsed.result) {
			types.push(type);
			let name = i18next.t('MaintainPartners.'+type);
			let partner = partnerkey;
			// TODO???
			DataTypeList.append(`<button class='btn btn-secondary selecttype' onclick='load_history_data(this)' data-partner='${partner}' data-type='${type}' style='width:100%; margin:2px;'>${name}</button>`);
		}

		for (var type of Object.keys(data_changes_log)) {
			if (! types.includes(type)) {
				let name = i18next.t('MaintainPartners.'+type);
				let partner = partnerkey;
				// TODO???
				DataTypeList.append(`<button class='btn btn-secondary selecttype' onclick='load_history_data(this)' data-partner='${partner}' data-type='${type}' style='width:100%; margin:2px;'>${name}</button>`);
			}
		}

		modal = ShowModal('history' + partnerkey, Temp);
		modal.attr('partnerkey', partnerkey);

		// select the first data type by default
		firstPermission = modal.find('button.selecttype:first');
		if (firstPermission.length) {
			firstPermission.click();
		}
	})
}

add_history_entry(purposes, channels, first, EventDateStr, AllowedPurposes, Value, CreatedBy, ChannelCode) {
	var HistPerm = $("[phantom] .history-entry").clone();

	var EventDate = new Date(EventDateStr);

	AllowedPurposes = AllowedPurposes ? AllowedPurposes : "-"; // be sure there is something
	HistPerm.find(".preview [name=Value]").text(Value);
	if (first) {
		HistPerm.attr("style", "background-color: #EEEEEE; border-color: green; border-style: solid;");
	} else {
		HistPerm.attr("style", "background-color: SlateGray");
	}

	HistPerm.find(".preview [name=EventDate]").text( EventDate.toLocaleDateString() );
	HistPerm.find(".preview [name=Permissions]").text( AllowedPurposes );

	HistPerm.find(".detail [name=Editor]").text( CreatedBy );
	HistPerm.find(".detail [name=Channel]").text( i18next.t('MaintainPartners.'+ChannelCode) );
	for (var channel of channels) {
		if (ChannelCode == channel.p_channel_code_c) {
			HistPerm.find(".detail [name=Channel]").text( translate_label(channel.p_name_c) );
		}
	}
	for (var purpose of purposes) {
		if (AllowedPurposes.split(',').indexOf(purpose.p_purpose_code_c) >= 0) {
			HistPerm.find(".detail [name=Consent]").append( "<br><span>" + translate_label(purpose.p_name_c) + "</span>" );
		}
	}

	return HistPerm;
}

load_history_data(HTMLButton) {
	var self = this;
	var partner_key = $(HTMLButton).attr("data-partner");
	var datatype = $(HTMLButton).attr("data-type");
	var req = {
		APartnerKey: partner_key,
		ADataType: datatype
	};
	var Target = FindModal('history'+partner_key);
	Target.attr("selected-data-type", datatype);

	api.post('serverMPartner.asmx/TDataHistoryWebConnector_GetHistory', req).then(function (data) {
		var parsed = JSON.parse(data.data.d);

		var channels = parsed.result.PConsentChannel;
		var purposes = parsed.result.PConsentPurpose;

		let type = datatype;
		type = i18next.t('MaintainPartners.'+type);
		Target.find(".selected-type").text(type);
		var HistoryList = Target.find("[history]").html("");
		let first = true;

		// show local entries that have not been saved yet
		if (data_changes_log[datatype] != null) {
			var entry = data_changes_log[datatype];

			var HistPerm = self.add_history_entry(
				purposes, channels,
				first,
				entry.ConsentDate, entry.Permissions,
				entry.Value, i18next.t('MaintainPartners.consent_not_saved_yet'), entry.ChannelCode);

			HistoryList.append(HistPerm);
			first = false;
		}

		for (var entry of parsed.result.PConsentHistory) {
			var HistPerm = self.add_history_entry(
				purposes, channels,
				first,
				entry.p_consent_date_d, entry.AllowedPurposes,
				entry.p_value_c, entry.s_created_by_c, entry.p_channel_code_c);

			HistoryList.append(HistPerm);
			first = false;
		}

	})

	api.post('serverMPartner.asmx/TDataHistoryWebConnector_LastKnownEntry', req).then(function (data) {
		var parsed = JSON.parse(data.data.d);
		var TargetPurpose = Target.find("[permissions]").html("");

		var purposes = parsed.result.PConsentPurpose;
		var last_known_configuration = parsed.result.PConsentHistory.pop(); // could be empty
		if (last_known_configuration == null || last_known_configuration.AllowedPurposes == null) {
			last_known_configuration = [];
			last_known_configuration["AllowedPurposes"]="";
		}

		// show local entries that have not been saved yet
		if (data_changes_log[datatype] != null) {
			var entry = data_changes_log[datatype];
			last_known_configuration = {AllowedPurposes: entry.Permissions};
		}

		for (var purpose of purposes) {
			let checked = (last_known_configuration.AllowedPurposes.split(',').indexOf(purpose.p_purpose_code_c) >= 0) ? "checked" : null;
			let name = translate_label(purpose.p_name_c);

			var PermTemp = $("[phantom] .permission-option").clone();
			PermTemp.find("[name]").text(name);
			PermTemp.find("[purposecode]").attr("purposecode", purpose.p_purpose_code_c);
			PermTemp.find("[purposecode]").attr("checked", checked);
			TargetPurpose.append(PermTemp);
		}
	});
}

insert_consent(HTMLField, data_name) {

	var Obj = $(HTMLField);
	var value = Obj.val();
	var modal = FindMyModal(Obj);
	var partnerkey = modal.find("[name=p_partner_key_n]").val();

	// special cases
	if (data_name == "address") { value = this.getUpdatesAddress(partnerkey); }

	data_changes_log[data_name] = {
		PartnerKey: partnerkey,
		Type: data_name,
		Value: value,
		ChannelCode: "", // is set later
		ConsentDate: new Date(), // default to Now
		Permissions: "", // is set later
		Valid: false // is set later
	};

	this.open_consent_modal(partnerkey, data_name);

}

open_consent_modal(partner_key, field, mode="partner_edit") {
	var req = {APartnerKey: partner_key, ADataType:field};

	api.post('serverMPartner.asmx/TDataHistoryWebConnector_LastKnownEntry', req).then(function (data) {
		var parsed = JSON.parse(data.data.d);
		var Temp = $('[phantom] .tpl_consent').clone();

		var purposes = parsed.result.PConsentPurpose;
		var channels = parsed.result.PConsentChannel;
		var last_known_configuration = parsed.result.PConsentHistory.pop() || {}; // could be empty

		// because it could be empty
		if (last_known_configuration.AllowedPurposes == null) {
			last_known_configuration["AllowedPurposes"]="";
			last_known_configuration["p_consent_date_d"] = (new Date()).toDateInputValue();
		}

		Temp.find("data[name=field]").val(field);
		Temp.find("[name=changed_value]").text(i18next.t(`MaintainPartners.${field}`));
		Temp.find("[name=consent_date]").val((new Date()).toDateInputValue());

		// place dynamic channel
		var TargetChannel = Temp.find("[name=consent_channel]").html("");
		for (var channel of channels) {
			let selected = (channel.p_channel_code_c == last_known_configuration.p_channel_code_c) ? "selected" : "";
			let name = translate_label(channel.p_name_c);
			TargetChannel.append(`<option ${selected} value='${channel.p_channel_code_c}'>${name}</option>`);
		}

		// place dynamic purposes
		var TargetPurpose = Temp.find(".permissions").html("");
		for (var purpose of purposes) {
			let checked = (last_known_configuration.AllowedPurposes.split(',').indexOf(purpose.p_purpose_code_c) >= 0) ? "checked" : null;
			let name = translate_label(purpose.p_name_c);

			var PermTemp = $("[phantom] .permission-option").clone();
			PermTemp.find("[name]").text(name);
			PermTemp.find("[purposecode]").attr("purposecode", purpose.p_purpose_code_c);
			PermTemp.find("[purposecode]").attr("checked", checked);
			TargetPurpose.append(PermTemp);
			// TargetPurpose.append(`<label><span>${name}</span><input ${checked} type='checkbox' purposecode='${purpose.p_purpose_code_c}'></label><br>`);
		}

		modal = ShowModal('consent' + partner_key, Temp);
		modal.attr('partnerkey', partner_key);

		if (mode == "partner_edit") {
			modal.find("[mode]").hide();
			modal.find("[mode=partner_edit]").show();
		}

		if (mode == "consent_edit") {
			modal.find("[mode]").hide();
			modal.find("[mode=consent_edit]").show();
		}
	})
}

submit_changes_consent(obj) {
	let modal = FindMyModal(obj);

	var current_field = modal.find("data[name=field]").val();
	var channel_code = modal.find("[name=consent_channel]").val();
	var consent_date = modal.find("[name=consent_date]").val();

	// get all permissions
	var perm_list = [];
	var perms = modal.find(".permissions input[purposecode]:checked");
	for (var Perm of perms) {
		perm_list.push( $(Perm).attr("purposecode") );
	}

	data_changes_log[current_field]["Valid"] = true;
	data_changes_log[current_field]["ChannelCode"] = channel_code;
	data_changes_log[current_field]["ConsentDate"] = consent_date;
	data_changes_log[current_field]["Permissions"] = perm_list.join(',');

	CloseModal(modal);
}

getUpdatesAddress(partnerkey) {
	let modal = FindModal('partneredit'+partnerkey);
	let street = modal.find("#addresses").find("[name=p_street_name_c]").val();
	let city = modal.find("#addresses").find("[name=p_city_c]").val();
	let postal = modal.find("#addresses").find("[name=p_postal_code_c]").val();
	let land = modal.find("#addresses").find("[name=p_country_code_c]").val();
	return `${street}, ${postal} ${city}, ${land}`;
}

submit_consent_edit(Obj, from_modal=false) {
	var self = this;
	let modal = FindMyModal(Obj);

	var partnerkey = modal.attr('partnerkey');
	var ty = modal.attr("selected-data-type");

	if (!from_modal) {
		if (data_changes_log[ty] != null) {
			// don't submit consent here, because we have unsaved consents. The order would be messed up.
			display_error( "MaintainPartners.error_consent_unsaved_changes" );
			return;
		}

		// we need to ask for date and consent channel
		this.open_consent_modal(partnerkey, ty, "consent_edit");
		return;
	}

	var perm_list = [];
	historyModal = FindModal('history'+partnerkey);
	var perms = historyModal.find("input[purposecode]:checked");
	for (var Perm of perms) {
		perm_list.push( $(Perm).attr("purposecode") );
	}

	var req = {
		APartnerKey: partnerkey,
		ADataType: modal.find("data[name=field]").val(),
		AChannelCode: modal.find("[name=consent_channel]").val(),
		AConsentDate: modal.find("[name=consent_date]").val(),
		AConsentCodes: perm_list.join(',')
	};

	api.post('serverMPartner.asmx/TDataHistoryWebConnector_EditHistory', req).then(function (data) {
		var parsed = JSON.parse(data.data.d);
		if (parsed.result) {
			var HTMLDataButton = $(historyModal).find(`button[data-type='${req.ADataType}']`);
			self.load_history_data(HTMLDataButton);
			CloseModal(modal);
		} else {
			if (parsed.AVerificationResult[0].message == "no_changes") {
				CloseModal(modal);
			} else {
				display_error( parsed.AVerificationResult );
			}
		}

	})

}

} // end of class
