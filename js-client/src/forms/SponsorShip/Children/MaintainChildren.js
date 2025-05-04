// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//			 Christopher Jäkel
//			 Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2020-2025 by SolidCharity.com
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
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.	If not, see <http://www.gnu.org/licenses/>.
//

import i18next from 'i18next'
import tpl from '../../../lib/tpl.js'
import api from '../../../lib/ajax.js'
import utils from '../../../lib/utils.js'
import i18n from '../../../lib/i18n.js'
import modal from '../../../lib/modal.js'
import AutocompletePartner from '../../../lib/autocomplete_partner.js'
import AutocompleteMotivation from '../../../lib/autocomplete_motivation.js'

class MaintainChildren {

	constructor() {
	}

	show_tab(tab_id) {
		// used to control tabs in modal, because there are issues with bootstrap
		let tab = $(tab_id);
		let target = tab.attr('aria-controls');
		tab.closest('.nav-tabs').find('.nav-link').removeClass('active');
		tab.addClass('active');

		let tgr = tab.closest('.container').find('.tab-content');
		tgr.find('.tab-pane').hide();
		tgr.find('#'+target).show();
	}

	Ready() {
		let self = this
		self.countComboboxes = 0;

		$(document).on('show.bs.modal', '.modal', function (event) {
			var zIndex = 1040 + (10 * $('.modal:visible').length);
			$(this).css('z-index', zIndex);
			setTimeout(function() {
				$('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
			}, 0);
		});

		self.initRecurringGiftBatch();
		self.loadPartnerAdmins();
		self.loadChildrenStatus();
		$('#btnSearch').click(function() { self.filterShow() });
		$('#tabfilter').keyup(function(event) {
			if (event.key === "Enter") {
				self.filterShow()
			}
		});
		$('#btnNewChild').on('click', function() { self.showCreate() })
		$('#btnSavePreset').on('click', function() { utils.save_preset('MaintainChildren') })
		$('#btnPrintList').on('click', function() { self.print() })
		$('#btnExportList').on('click', function() { self.export() })
	}

	AfterLoadingComboboxes() {
		let self = this;
		self.countComboboxes++;
		if (self.countComboboxes == 2) {
			self.load_preset();
			self.filterShow();
		}
	}

	load_preset() {
		var x = window.localStorage.getItem('MaintainChildren');
		if (x != null) {
			x = JSON.parse(x);
			tpl.format_tpl($('#tabfilter'), x);
		}
	}

	loadPartnerAdmins() {
		let self = this;
		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_GetSponsorAdmins', {}).then(
			function (data) {
				var parsed = JSON.parse(data.data.d);

				var TargetFields = $("[name=ASponsorAdmin], [name=p_user_id_c]").html('<option value="">('+i18next.t("forms.any")+')</option>');
				for (var user of parsed.result) {
					let NewOption = $("<option></option>");
					NewOption.attr("value", user.s_user_id_c);
					let name = user.s_user_id_c;
					if (user.s_last_name_c != null && user.s_first_name_c != null) {
						name = user.s_last_name_c + ", " + user.s_first_name_c;
					}
					NewOption.text(name);
					TargetFields.append(NewOption);
				}

				self.AfterLoadingComboboxes();
			}
		);
	}

	loadChildrenStatus() {
		let self = this;
		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_GetChildrenStatusOptions', {}).then(
			function (data) {
				var parsed = JSON.parse(data.data.d);

				var TargetFields = $("#tabfilter [name=ASponsorshipStatus]").html('<option value="">('+i18next.t("forms.any")+')</option>');
				for (var statustype of parsed.result) {
					let NewOption = $("<option></option>");
					NewOption.attr("value", statustype.p_type_code_c);
					let name = statustype.p_type_description_c;
					NewOption.text(name);
					TargetFields.append(NewOption);
				}

				// without any option
				var TargetFields = $(".tpl_edit_child [name=ASponsorshipStatus]").html('');
				for (var statustype of parsed.result) {
					let NewOption = $("<option></option>");
					NewOption.attr("value", statustype.p_type_code_c);
					let name = statustype.p_type_description_c;
					NewOption.text(name);
					TargetFields.append(NewOption);
				}

				self.AfterLoadingComboboxes();
			}
		);
	}

	filterShow() {
		let self = this;
		// get infos from the filter and search with them
		self.show();
	}

	initRecurringGiftBatch() {
		var param = {
			"ALedgerNumber": window.localStorage.getItem("current_ledger"),
			"ABankAccountCode": ""
		};

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_InitRecurringGiftBatchForSponsorship', param);
	}

	getFilter() {

		var filter = tpl.extract_data($("#tabfilter"));

		var req = {
			"AChildName": filter.ChildName ? filter.ChildName : "",
			"ADonorName": filter.DonorName ? filter.DonorName : "",
			"APartnerStatus": filter.APartnerStatus ? filter.APartnerStatus : "",
			"ASponsorshipStatus": filter.ASponsorshipStatus ? filter.ASponsorshipStatus : "",
			"ASponsorAdmin": filter.ASponsorAdmin ? filter.ASponsorAdmin : "",
			"ASortBy": filter.ASortBy ? filter.ASortBy : "",
			"AChildWithoutDonor": filter.ChildWithoutDonor ? true : false,
		};

		return req;
	}

	// get data from server and show them
	show() {
		let self = this;
		var req = self.getFilter();

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_FindChildren', req).then(
			function (data) {
				var parsed = JSON.parse(data.data.d);
				var List = $("#result").html("");
				for (var entry of parsed.result) {
					if (entry.SponsorName) {
						entry.SponsorName = entry.SponsorName.replace(/;/g, ";<br/>");
					}
					if (entry.SponsorContactDetails) {
						entry.SponsorContactDetails = entry.SponsorContactDetails.replace(/;/g, "<br/>");
					}

					var Copy = $("[phantom] .children").clone();
					let view = $("[phantom] .tpl_view").clone();

					tpl.insertData(Copy, entry);
					tpl.insertData(view, entry);
					List.append(Copy);
					$('#child'+entry['p_partner_key_n']).find('.collapse_col').append(view);
					$('#btnViewChild'+entry['p_partner_key_n']).on('click', function() {self.open_detail($(this))});
					$('#btnEditChild'+entry['p_partner_key_n']).on('click', function() {self.edit_child($(this))});
				}
			}
		);
	}

	// same as show, but return a PDF file
	print() {
		var req = this.getFilter();
		req["AReportLanguage"] = i18n.currentLng();

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_PrintChildren', req).then(
			function (data) {
				var report = data.data.d;
				var link = document.createElement("a");
				link.style = "display: none";
				link.href = 'data:application/pdf;base64,'+report;
				link.download = i18next.t('MaintainChildren.children') + '.pdf';
				document.body.appendChild(link);
				link.click();
				link.remove();
				utils.display_message( i18next.t("forms.printed_success"), "success");
			}
		);
	}

	// same as show, but export file with addresses of sponsors
	export() {
		var req = this.getFilter();
		req["AReportLanguage"] = i18n.currentLng();

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_ExportChildrenAndSponsorAddresses', req).then(
			function (data) {
				var report = data.data.d;
				var link = document.createElement("a");
				link.style = "display: none";
				link.href = 'data:application/excel;base64,'+report;
				link.download = i18next.t('MaintainChildren.sponsorships') + '.xlsx';
				document.body.appendChild(link);
				link.click();
				link.remove();
				utils.display_message( i18next.t("forms.exported_success"), "success");
			}
		);
	}

	showCreate() {
		let self = this;
		let temp = $('[phantom] .tpl_edit_child').clone();
		temp.attr("mode", "create");
		temp.find("select[name='ASponsorshipStatus'] option[value='CHILDREN_HOME']").prop('selected',true);
		let m = modal.ShowModal('child', temp);

		m.find('#btnClose').on('click', function () {modal.CloseModal(this)});
		m.find('#btnSave').on('click', function () {self.save_child(this)});
		m.find('#btnDelete').on('click', function () {self.delete_child(this)});
		let elements = m.find('.nav-tabs .nav-item .nav-link');
		elements.each(function(index) { $(this).on("click", function(e) { self.show_tab(this) })});
	}

	edit_child(obj) {
		// get details for the child the user clicked on and open modal
		let self = this;

		var req = {
			"APartnerKey": $(obj).closest(".row").find("[name=p_partner_key_n]").val(),
			"AWithPhoto": true,
			"ALedgerNumber": window.localStorage.getItem("current_ledger")
		};

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_GetChildDetails', req).then(
			function (data) {
				var parsed = JSON.parse(data.data.d);

				var ASponsorshipStatus = parsed.ASponsorshipStatus;
				var partner = parsed.result.PPartner[0];
				var family = parsed.result.PFamily[0];
				var comments = parsed.result.PPartnerComment;
				var recurring_detail = parsed.result.ARecurringGiftDetail;
				var reminder = parsed.result.PPartnerReminder;

				let temp = $('[phantom] .tpl_edit_child').clone();

				tpl.insertData(temp, {"ASponsorshipStatus":ASponsorshipStatus});
				tpl.insertData(temp, partner);
				tpl.insertData(temp, family);
				temp.find("#new_photo").val('');

				MaintainChildSponsorship.build(temp, recurring_detail);
				MaintainChildComments.build(temp, comments);
				MaintainChildReminders.build(temp, reminder);

				temp.find("[name='p_photo_b']").attr("src", "data:image/jpg;base64,"+family.p_photo_b);

				temp.attr("mode", "edit");
				let m = modal.ShowModal('edit_child', temp)

				m.find('#new_photo').on('change', function() {self.photoPreview(m)});
				m.find('#btnUploadPhoto').on('click', function () { self.uploadNewPhoto(m)});
				m.find('#btnClose').on('click', function () {modal.CloseModal(this)});
				m.find('#btnSave').on('click', function () {self.save_child(this)});
				m.find('#btnDelete').on('click', function () {self.delete_child(this)});
				let elements = m.find('.nav-tabs .nav-item .nav-link');
				elements.each(function(index) { $(this).on("click", function(e) { self.show_tab(this) })});
		}
		);

	}

	open_detail(obj) {
		obj = $(obj);
		while(!obj[0].hasAttribute('id') || !obj[0].id.includes("child")) {
			obj = obj.parent();
		}
		if (obj.find('.collapse').is(':visible') ) {
			$('.tpl_row .collapse').collapse('hide');
			return;
		}
		$('.tpl_row .collapse').collapse('hide');
		obj.find('.collapse').collapse('show')
	}

	delete_child(obj) {
		let self = this;
		let m = modal.FindMyModal(obj)

		let s = confirm( i18next.t('MaintainChildren.ask_delete_child') );
		if (!s) {return}

		var req = utils.translate_to_server(tpl.extractData(m));
		req["ALedgerNumber"] = window.localStorage.getItem("current_ledger");
		req["APartnerKey"] = m.find("input[name=p_partner_key_n]").val();

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_DeleteChild', req).then(
			function (data) {
				var parsed = JSON.parse(data.data.d);
				if (parsed.result != true) {
					return utils.display_error(parsed.AVerificationResult);
				} else {
					utils.display_message( i18next.t("forms.deleted"), "success");
					modal.CloseModal(obj)
					self.filterShow();
				}
			});
	}

	save_child(obj) {
		let self = this;
		let m = modal.FindMyModal(obj)

		var req = utils.translate_to_server(tpl.extractData(m));
		req["ALedgerNumber"] = window.localStorage.getItem("current_ledger");
		req["APartnerKey"] = m.find("input[name=p_partner_key_n]").val();
		if (req["ADateOfBirth"] == "") {
			req["ADateOfBirth"] = "null";
		}

		var mode = m.attr("mode");
		if (mode == "create") { req["APartnerKey"] = -1; }
		req["APhoto"] = "";
		req["AUploadPhoto"] = false;

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainChild', req).then(
			function (data) {
				var parsed = JSON.parse(data.data.d);
				if (!parsed.result) {
					return utils.display_error(parsed.AVerificationResult);
				} else {
					utils.display_message( i18next.t("forms.saved"), "success");
					if (mode == "create") {
						modal.CloseModal(obj)
						m.find("input[name='AFirstName']").val(req["AFirstName"]);
						self.filterShow();
					} else {
						modal.CloseModal(obj)
						self.filterShow();
					}
				}
			}
		);

	}

	photoPreview(obj) {
		var PhotoField = obj.find("[name=new_photo]");
		var Reader = new FileReader();
		Reader.onload = function (event) {
			let file_content = event.target.result;
			file_content = btoa(file_content);
			obj.find("[name='p_photo_b']").attr("src", "data:image/jpg;base64,"+file_content);
		}
		Reader.readAsBinaryString(PhotoField[0].files[0]);
	}

	uploadNewPhoto(obj) {
		var PhotoField = obj.find("[name=new_photo]");
		let name = PhotoField.val();
		if (!name || !PhotoField[0].files[0]) {return;}

		// see http://www.html5rocks.com/en/tutorials/file/dndfiles/
		if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
			alert('The File APIs are not fully supported in this browser.');
		}

		var Reader = new FileReader();

		Reader.onload = function (theFile) {
			let file_content = theFile.target.result;
			//file_content = btoa(file_content);

			// somehow, theFile.name on Firefox is undefined
			let filename = theFile.name;
			if (filename == undefined) {
				filename = PhotoField[0].value.split("\\").pop();
			}
			if (filename == undefined) {
				filename="undefined.txt";
			}

			var req = {
				"APartnerKey": obj.find("[name=p_partner_key_n]").val(),
				"ALedgerNumber": window.localStorage.getItem("current_ledger"),
				"APhotoFilename": filename,
				"APhoto":file_content
			};

			api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_UploadPhoto', req)
			.then(function (data) {
					var parsed = JSON.parse(data.data.d);
					if (parsed.result) {
						utils.display_message( i18next.t("forms.upload_success"), "success");
					} else {
						utils.display_error(parsed.AVerificationResult);
					}
			})
			.catch(error => {
				console.log(error)
				utils.display_message(i18next.t('forms.uploaderror'), "fail");
			});

		}

		Reader.readAsDataURL(PhotoField[0].files[0]);

	}
}

var MaintainChildComments = new (class {
	constructor() {
		this.highest_index = 0;
		this.MAX_LENGTH_COMMENT_PREVIEW = 64;
	}

	build(obj_modal, comments) {
		// builds the entrys as rows in there location
		// requires a list of PPartnerComment API data
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		self.parent_modal = obj;

		var CommentsFamily = obj.find("[id=family_situations] .container-list").html("");
		var CommentsSchool = obj.find("[id=school_situations] .container-list").html("");

		this.highest_index = 0;

		for (var comment of comments) {
			var Copy = $("[phantom] .comment").clone();

			// save current highest index
			this.highest_index = comment["p_index_i"]

			// short comment in preview
			if (comment["p_comment_c"] == null) { comment["p_comment_c"] = ''; }
			comment["p_comment_short_c"] = comment["p_comment_c"];
			if (comment["p_comment_c"].length > self.MAX_LENGTH_COMMENT_PREVIEW) {
				comment["p_comment_short_c"] = comment["p_comment_c"].substring(0, self.MAX_LENGTH_COMMENT_PREVIEW-2) + "..";
			}

			tpl.insertData(Copy, comment);
			switch (comment.p_comment_type_c) {
				case "FAMILY": CommentsFamily.append(Copy); break;
				case "SCHOOL": CommentsSchool.append(Copy); break;
			}
			Copy.find("#btnEdit").on('click', function () {self.edit(this)});

		}

		if (!self.buttons_init) {
			obj.find("#btnNewFamilySituation").on('click', function () {self.showCreate(obj, 'FAMILY')});
			obj.find("#btnNewSchoolSituation").on('click', function () {self.showCreate(obj, 'SCHOOL')});
			self.buttons_init = true;
		}
	}

	showCreate(obj, type) {
		let self = this;

		var data = {
			"p_partner_key_n" : obj.find("[name=p_partner_key_n]").val(),
			"p_index_i" : (this.highest_index + 1),
			"p_comment_c" : "",
			"p_comment_type_c" : type
		};

		let temp = $("#comment_modal").clone();
		tpl.insertData(temp, data);
		temp.attr("mode", "create");
		let m = modal.ShowModal('comment_new_' + type, temp);
		console.log(m)
		m.find('#btnSave').on('click', function () {self.saveEdit(this)});
		m.find('#btnClose').on('click', function () {modal.CloseModal(this)});
	}

	saveEdit(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		var req = utils.translate_to_server(tpl.extractData(obj));
		req["ALedgerNumber"] = window.localStorage.getItem("current_ledger");

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainChildComments', req).then(
			function (data) {
				var parsed = JSON.parse(data.data.d);

				if (!parsed.result) {
					return utils.display_error(parsed.AVerificationResult);
				}

				modal.CloseModal(obj);
				self.build(self.parent_modal, parsed.APartnerComment);
			}
		);

	}

	edit(obj) {
		let self = this;
		obj = $(obj).closest(".comment");

		var comment_index = obj.find("[name=p_index_i]").val();
		var partner_key = obj.find("[name=p_partner_key_n]").val();

		var req = { "APartnerKey": partner_key };
		req["ALedgerNumber"] = window.localStorage.getItem("current_ledger");
		req["AWithPhoto"] = false;

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_GetChildDetails', req).then(
			function (data) {
				var parsed = JSON.parse(data.data.d);
				var edit_comment = null;

				for (var comment of parsed.result.PPartnerComment) {
					if (comment.p_index_i == comment_index) {
						edit_comment = comment;
						break;
					}
				}

				if (!edit_comment) { return; }

				let temp = $("#comment_modal").clone();
				tpl.insertData(temp, edit_comment);
				temp.attr("mode", "edit");
				let m = modal.ShowModal('comment_edit', temp);
				console.log("show modal comment")
				m.find('#btnSave').on('click', function () {self.saveEdit(this)});
				m.find('#btnClose').on('click', function () {modal.CloseModal(this)});
			}
		);
	}

})

var MaintainChildSponsorship = new (class {
	constructor() {
	}

	build(obj_modal, gift_details) {
		// builds the entrys as rows in their location
		// requires a list of ARecurringGiftDetail API data
		let self = this;

		let obj = $(obj_modal).closest('.modal');
		self.parent_modal = obj;
		var SponsorList = obj.find("[id=sponsorship] .container-list").html("");

		for (var sponsorship of gift_details) {

			if ((sponsorship.DonorAddress !== null) && (sponsorship.DonorAddress != '')) {
				sponsorship.DonorAddress += '<br/>';
			}
			if ((sponsorship.DonorEmailAddress !== null) && (sponsorship.DonorEmailAddress != '')) {
				sponsorship.DonorEmailAddress = '<a href="mailto:' + sponsorship.DonorEmailAddress + '">' + sponsorship.DonorEmailAddress + '</a><br/>';
			}
			sponsorship.p_partner_key_n = obj.find("[name=p_partner_key_n]").val();

			var Temp = $("[phantom] .sponsorship").clone();
			tpl.insertData(Temp, sponsorship, sponsorship.CurrencyCode);
			SponsorList.append(Temp);
			Temp.find("#btnEditSponsorship").on('click', function () {self.edit(this)});
		}

		if (!self.buttons_init) {
			obj.find("#btnNewSponsorship").on('click', function () {self.showCreate(obj)});
			self.buttons_init = true;
		}
	}

	showCreate(obj) {
		let self = this;

		var data = {
			"a_gift_transaction_number_i":"-1",
			"a_batch_number_i":"-1",
			"a_detail_number_i":"-1",
			"a_motivation_group_code_c": "",
			"a_motivation_detail_code_c": "",
			"p_recipient_key_n":obj.find("[name=p_partner_key_n]").val(),
			"a_ledger_number_i": window.localStorage.getItem("current_ledger")
		};
		let temp = $("#recurring_modal").clone();
		tpl.insertData(temp, data);
		let m = modal.ShowModal('sponsorship_new', temp);
		m.find('#btnCloseSponsorship').on('click', function () {modal.CloseModal(this)});
		m.find('#btnSaveSponsorship').on('click', function () {self.saveEdit(this)});
		m.find('#btnDeleteSponsorship').on('click', function () {self.delete(this)});
		m.find('#autocomplete_donor').on('input', function () {AutocompletePartner.autocomplete_donor(this)});
		m.find('#autocomplete_motivation_detail').on('input', function() {AutocompleteMotivation.autocomplete_motivation_detail(this)});
	}

	saveEdit(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		var req = utils.translate_to_server(tpl.extractData(obj));

		if (!req["ADonorKey"] || isNaN(parseInt(req["ADonorKey"]))) {
			return utils.display_error("MaintainChildren.ErrMissingDonor");
		}

		if (!req["AGiftAmount"] || isNaN(parseFloat(req["AGiftAmount"]))) {
			return utils.display_error("MaintainChildren.ErrMissingAmount");
		}

		if (!req["AStartDonations"]) {
			return utils.display_error("MaintainChildren.ErrStartDonationsDate");
		}

		// check for endless date
		if (!req["AEndDonations"]) {
			req["AEndDonations"] = "null";
		}

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainSponsorshipRecurringGifts', req).then(
			function (data) {
				var parsed = JSON.parse(data.data.d);

				if (!parsed.result) {
					return utils.display_error(parsed.AVerificationResult);
				}

				modal.CloseModal(obj)

				self.build(self.parent_modal, parsed.ARecurringGiftDetail);
			}
		);
	}

	delete(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		let payload = utils.translate_to_server(tpl.extract_data(obj));
		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_DeleteSponsorshipRecurringGift', payload).then(
			function (result) {
				var parsed = JSON.parse(result.data.d);

				if (!parsed.result) {
					return utils.display_error(parsed.AVerificationResult);
				}

				modal.CloseModal(obj)

				self.build(self.parent_modal, parsed.ARecurringGiftDetail);
			});
	}

	edit(obj, overwrite) {
		let self = this;

		obj = $(obj).closest(".sponsorship");
		var req_detail = tpl.extractData(obj);

		var req = {
			"APartnerKey": overwrite ? overwrite : obj.find("[name=p_partner_key_n]").val(),
			"ALedgerNumber": window.localStorage.getItem("current_ledger")
		};
		req["AWithPhoto"] = false;

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_GetChildDetails', req).then(
			function (data) {
				var parsed = JSON.parse(data.data.d);

				var recurring_detail = parsed.result.ARecurringGiftDetail;

				var searched = null;
				for (var detail of recurring_detail) {
					if (detail.a_gift_transaction_number_i == req_detail.a_gift_transaction_number_i) {
						if (detail.a_detail_number_i == req_detail.a_detail_number_i) {
							searched = detail;
							break;
						}
					}
				}

				if (!searched) { return; }
				searched['p_donor_name_c'] = searched['p_donor_key_n'] + ' ' + searched['DonorName'];

				let temp = $("#recurring_modal").clone();
				tpl.insertData(temp, searched);
				let m = modal.ShowModal('sponsorship_edit', temp);
				m.find('#btnCloseSponsorship').on('click', function () {modal.CloseModal(this)});
				m.find('#btnSaveSponsorship').on('click', function () {self.saveEdit(this)});
				m.find('#btnDeleteSponsorship').on('click', function () {self.delete(this)});
				m.find('#autocomplete_donor').on('input', function () {AutocompletePartner.autocomplete_donor(this)});
				m.find('#autocomplete_motivation_detail').on('input', function() {AutocompleteMotivation.autocomplete_motivation_detail(this)});
			}
		);

	}

})

var MaintainChildReminders = new (class {
	constructor() {
		this.highest_index = 0;
		this.MAX_LENGTH_COMMENT_PREVIEW = 64;
	}

	build(obj_modal, reminders) {
		// builds the entrys as rows in there location
		// requires a list of PPartnerReminder API data
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		self.parent_modal = obj;
		var Reminders = obj.find("[id=dates_reminder] .container-list").html("");

		this.highest_index = 0;

		for (var reminder of reminders) {
			var Copy = $("[phantom] .reminder").clone();

			// save current highest index
			this.highest_index = reminder["p_reminder_id_i"]

			// short reminder in preview
			if (reminder["p_comment_c"] == null) { reminder["p_comment_c"] = ''; }
			reminder["p_comment_short_c"] = reminder["p_comment_c"];
			if (reminder["p_comment_c"].length > self.MAX_LENGTH_COMMENT_PREVIEW) {
				reminder["p_comment_short_c"] = reminder["p_comment_c"].substring(0, self.MAX_LENGTH_COMMENT_PREVIEW-2) + "..";
			}

			tpl.insertData(Copy, reminder);
			Reminders.append(Copy);
			Copy.find("#btnEdit").on('click', function () {self.edit(this)});
		}

		if (!self.buttons_init) {
			obj.find("#btnNewReminder").on('click', function () {self.showCreate(obj)});
			self.buttons_init = true;
		}
	}

	showCreate(obj) {
		let self = this;

		var data = {
			"p_partner_key_n": obj.find("[name=p_partner_key_n]").val(),
			"p_reminder_id_i": (this.highest_index + 1),
			"p_comment_c" : "",
			"p_event_date_d": "",
			"p_first_reminder_date_d": ""
		};

		let temp = $("#reminder_modal").clone();
		tpl.insertData(temp, data);
		temp.attr("mode", "create");
		let m = modal.ShowModal('reminder_new', temp);

		m.find('#btnSave').on('click', function () {self.saveEdit(this)});
		m.find('#btnClose').on('click', function () {modal.CloseModal(this)});
	}

	saveEdit(obj_modal) {
		let self = this;
		let obj = $(obj_modal).closest('.modal');
		var req = utils.translate_to_server(tpl.extractData(obj));
		req["ALedgerNumber"] = window.localStorage.getItem("current_ledger");

		if (!req["AEventDate"]) {
			return utils.display_error("MaintainChildren.ErrReminderEventDate");
		}

		if (!req["AFirstReminderDate"]) {
			return utils.display_error("MaintainChildren.ErrFirstReminderDate");
		}

		if (!req["AComment"]) {
			return utils.display_error("MaintainChildren.ErrEmptyReminderComment");
		}

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainChildReminders', req).then(
			function (data) {
				var parsed = JSON.parse(data.data.d);

				if (!parsed.result) {
					return utils.display_error(parsed.AVerificationResult);
				}

				modal.CloseModal(obj)
				self.build(self.parent_modal, parsed.APartnerReminder);
			}
		);

	}

	edit(obj) {
		let self = this;
		obj = $(obj).closest(".reminder");

		var reminder_id = obj.find("[name=p_reminder_id_i]").val();
		var partner_key = obj.find("[name=p_partner_key_n]").val();

		var req = { "APartnerKey": partner_key };
		req["ALedgerNumber"] = window.localStorage.getItem("current_ledger");
		req["AWithPhoto"] = false;

		api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_GetChildDetails', req).then(
			function (data) {
				var parsed = JSON.parse(data.data.d);
				var edit_reminder = null;

				for (var reminder of parsed.result.PPartnerReminder) {
					if (reminder.p_reminder_id_i == reminder_id) {
						edit_reminder = reminder;
						break;
					}
				}

				if (!edit_reminder) { return; }

				let temp = $("#reminder_modal").clone();
				tpl.insertData(temp, edit_reminder);
				temp.attr("mode", "edit");
				let m = modal.ShowModal('reminder_edit', temp);
				m.find('#btnSave').on('click', function () {self.saveEdit(this)});
				m.find('#btnClose').on('click', function () {modal.CloseModal(this)});
			}
		);
	}

})

export default new MaintainChildren();