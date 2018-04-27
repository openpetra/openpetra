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
		$('#browse_container').html('');
		for (user of data.result) {
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
	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_GetPartnerDetails', r).then(function (data) {
		actuall_data_str = data.data.d;

		parsed = JSON.parse(actuall_data_str);
		console.log(parsed);

		last_opend_entry_data = $.extend(true, {}, parsed);
		let m = $('[phantom] .tpl_edit').clone();
		// normal info input
		m = format_tpl(m ,parsed.result.PLocation[0], "PLocation_");
		m = format_tpl(m ,parsed.result.PPartner[0],"PPartner_");
		m = format_tpl(m ,parsed.result.PUnit[0],"PUnit_");
		m = format_tpl(m ,parsed.result.PFamily[0],"PFamily_");

		m.find('.select_case').hide();
		m.find('.'+parsed.result.PPartner[0].p_partner_class_c).show();
		$('#modal_space').html(m);
		$('#modal_space .modal').modal('show');
	})
}

function save_entry(obj_modal) {
	let obj = $(obj_modal).closest('.modal');
	let x = extract_data(obj);
	console.log(x);
	x = replace_data(last_opend_entry_data.result, x);
	console.log(last_opend_entry_data.result);
	let r = {'AMainDS': JSON.stringify(last_opend_entry_data.result)};
	api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_SavePartner', r).then(function (data) {
		$('#modal_space .modal').modal('hide');
		diplay_partners();
	})
}

function show_tab(tab_id) {
	let tab = $(tab_id);
	let target = tab.attr('aria-controls');
	tab.closest('.nav-tabs').find('.nav-link').removeClass('active');
	tab.addClass('active');

	tgr = tab.closest('.container').find('.tab-content');
	tgr.find('.tab-pane').removeClass('active');
	tgr.find('#'+target).addClass('active');

}

/*
class MaintainPartnersForm extends JSForm {
	constructor() {
		super('MaintainPartners',
			'serverMPartner.asmx/TSimplePartnerFindWebConnector_FindPartners', );
		super.initEvents();

		// TODO: if no search criteria are defined, then show the 10 last viewed or edited partners
		super.search();
	}

	getMainTableFromResult(result) {
		return result.result;
	}

	getKeyFromRow(row) {
		return row['p_partner_key_n'];
	}

	getDataForEdit(dialogname, key, html) {
		api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_GetPartnerDetails',
			{APartnerKey: key,
			AWithAddressDetails: true,
			AWithSubscriptions: true,
			AWithRelationships: true})
			.then(function(response) {
				if (response.data == null) {
					console.log("error: " + response);
					return;
				}
				var result = JSON.parse(response.data.d);
				if (result.result == "false") {
					console.log("problem loading " + apiUrl);
				} else {
					self.editData = result.result;
					html = self.insertEditDataIntoDialog(self, key, html);
					html = self.showSpecificClass(self, self.editData.PPartner[0], html);
					self.displayDialog(self, dialogname, html);
				}
			});
	}

	saveData(self, dialogname) {
		// console.log(self.editData);

		api.post('serverMPartner.asmx/TSimplePartnerEditWebConnector_SavePartner',
			{AMainDS: JSON.stringify(self.editData)})
			.then(function(response) {
				if (response.data == null) {
					console.log("error: " + response);
					return;
				}
				var result = JSON.parse(response.data.d);
				if (result.result == "false") {
					// TODO: show error message
					console.log("problem loading " + apiUrl);
				} else {
					// TODO: show success message
					self.closeEditDialog(dialogname);
				}
			});
	}

	showSpecificClass(self, row, html) {
		var partner_classes = ["FAMILY", "PERSON", "ORGANISATION", "BANK", "UNIT"];
		for (var i in partner_classes) {
			var cl = partner_classes[i];
			if (row['p_partner_class_c'] != cl) {
				html = replaceAll(html, ' class="' + cl + '"', ' class="' + cl + ' hidden"');
			}
		}
		return html;
	}

	insertEditDataIntoDialogDerived(self, dialogname, key, html) {
		self.getDataForEdit(dialogname, key, html);
		// we don't have the html yet
		return null;
	}
}
*/
