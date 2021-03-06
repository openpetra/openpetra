// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Christopher Jäkel
//       Timotheus Pokorra <timotheus.pokorra@solidcharity.com>
//
// Copyright 2020-2021 by SolidCharity.com
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

MAX_LENGTH_COMMENT_PREVIEW = 64;

$("document").ready(function () {
  MaintainChildren.initRecurringGiftBatch();
  loadPartnerAdmins();
  loadChildrenStatus();
});

countComboboxes = 0;
function AfterLoadingComboboxes() {
  countComboboxes++;
  if (countComboboxes == 2) {
    load_preset();
    MaintainChildren.filterShow();
  }
}

function load_preset() {
  var x = window.localStorage.getItem('MaintainChildren');
  if (x != null) {
    x = JSON.parse(x);
    format_tpl($('#tabfilter'), x);
  }
}

var MaintainChildren = new (class {
  constructor() {

  }

  filterShow() {
      // get infos from the filter and search with them
      this.show();
  }

  initRecurringGiftBatch() {
    var param = {
      "ALedgerNumber": window.localStorage.getItem("current_ledger"),
      "ABankAccountCode": ""
    };

    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_InitRecurringGiftBatchForSponsorship', param);
  }

  getFilter() {

    var filter = extract_data($("#tabfilter"));

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
    var req = this.getFilter();

    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_FindChildren', req).then(
      function (data) {
        var parsed = JSON.parse(data.data.d);
        var List = $("#result").html("");
        for (var entry of parsed.result) {
          if (entry.DonorName) {
            entry.DonorName = entry.DonorName.replace(/;/g, ";<br/>");
          }
          if (entry.DonorContactDetails) {
            entry.DonorContactDetails = entry.DonorContactDetails.replace(/;/g, "<br/>");
          }

          var Copy = $("[phantom] .children").clone();
          let view = $("[phantom] .tpl_view").clone();

          insertData(Copy, entry);
          insertData(view, entry);
          List.append(Copy);
          $('#child'+entry['p_partner_key_n']).find('.collapse_col').append(view);
        }
      }
    );
  }

  // same as show, but return a PDF file
  print() {
    var req = this.getFilter();
    req["AReportLanguage"] = currentLng();

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
        display_message( i18next.t("forms.printed_success"), "success");
      }
    );
  }

  edit(HTMLBottom, overwrite, OpenTab=null) {
    // get details for the child the user clicked on and open modal

    var req = {
      "APartnerKey": overwrite ? overwrite : $(HTMLBottom).closest(".row").find("[name=p_partner_key_n]").val(),
      "AWithPhoto": true,
      "ALedgerNumber": window.localStorage.getItem("current_ledger")
    };

    this.showWindow(null, "details");
    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_GetChildDetails', req).then(
      function (data) {
        var parsed = JSON.parse(data.data.d);

        var ASponsorshipStatus = parsed.ASponsorshipStatus;
        var partner = parsed.result.PPartner[0];
        var family = parsed.result.PFamily[0];
        var comments = parsed.result.PPartnerComment;
        var recurring = parsed.result.ARecurringGift;
        var recurring_detail = parsed.result.ARecurringGiftDetail;
        var reminder = parsed.result.PPartnerReminder;

        insertData("#detail_modal", {"ASponsorshipStatus":ASponsorshipStatus});
        insertData("#detail_modal", partner);
        insertData("#detail_modal", family);
        $("#new_photo").val('');

        MaintainChildSponsorship.build(recurring, recurring_detail);
        MaintainChildComments.build(comments);
        MaintainChildReminders.build(reminder);

        $("#detail_modal [name='p_photo_b']").attr("src", "data:image/jpg;base64,"+family.p_photo_b);

        $("#detail_modal").attr("mode", "edit");
        $("#detail_modal").modal("show");

        if (OpenTab !== null) {
          MaintainChildren.showWindow(null, OpenTab);
        }
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

  showWindow(HTMLAnchor, overwrite) {
    // hide all windows in #multi_window and only show the one related to the link
    // also updates buttons

    var show = null;
    if (HTMLAnchor !== null) { show = $(HTMLAnchor).attr("show"); }
    if (overwrite) { show = overwrite; }

    // nav-bar
    $("[role=tablist] [show]").removeClass("active");
    $(`[role=tablist] [show=${show}]`).addClass("active");

    // window
    $("#multi_window [window]").hide();
    $(`#multi_window [window=${show}]`).show();
    $("#multi_window").attr("active", show);
  }

  delete() {
    let s = confirm( i18next.t('MaintainChildren.ask_delete_child') );
    if (!s) {return}

    var MaintainChildrenO = this;
    var req = translate_to_server(extractData($("#detail_modal")));
    req["ALedgerNumber"] = window.localStorage.getItem("current_ledger");
    req["APartnerKey"] = $("input[name=p_partner_key_n]").val();

    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_DeleteChild', req).then(
      function (data) {
        var parsed = JSON.parse(data.data.d);
        if (!parsed.result) {
          return display_error(parsed.AVerificationResult);
        } else {
          display_message( i18next.t("forms.deleted"), "success");
          $("#detail_modal").modal("hide");
          MaintainChildrenO.filterShow();
        }
      });
  }

  saveEdit() {

    var MaintainChildrenO = this;
    var req = translate_to_server(extractData($("#detail_modal")));
    req["ALedgerNumber"] = window.localStorage.getItem("current_ledger");
    req["APartnerKey"] = $("input[name=p_partner_key_n]").val();
    if (req["ADateOfBirth"] == "") {
      req["ADateOfBirth"] = "null";
    }

    var mode = $("#detail_modal").attr("mode");
    if (mode == "create") { req["APartnerKey"] = -1; }
    req["APhoto"] = "";
    req["AUploadPhoto"] = false;

    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainChild', req).then(
      function (data) {
        var parsed = JSON.parse(data.data.d);
        if (!parsed.result) {
          return display_error(parsed.AVerificationResult);
        } else {
          display_message( i18next.t("forms.saved"), "success");
          if (mode == "create") {
            $("#detail_modal").modal("hide");
            $("input[name='AFirstName']").val(req["AFirstName"]);
            MaintainChildrenO.filterShow();
          } else {
            $("#detail_modal").modal("hide");
            MaintainChildrenO.filterShow();
          }
        }
      }
    );

  }

  photoPreview() {
    var PhotoField = $("#detail_modal [name=new_photo]");
    var Reader = new FileReader();
    Reader.onload = function (event) {
      let file_content = event.target.result;
      file_content = btoa(file_content);
      $("#detail_modal [name='p_photo_b']").attr("src", "data:image/jpg;base64,"+file_content);
    }
    Reader.readAsBinaryString(PhotoField[0].files[0]);
  }

  uploadNewPhoto() {
    var PhotoField = $("#detail_modal [name=new_photo]");
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
        "APartnerKey":$("#detail_modal [name=p_partner_key_n]").val(),
        "ALedgerNumber": window.localStorage.getItem("current_ledger"),
        "APhotoFilename": filename,
        "APhoto":file_content
      };

      api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_UploadPhoto', req)
      .then(function (data) {
          var parsed = JSON.parse(data.data.d);
          if (parsed.result) {
            display_message( i18next.t("forms.upload_success"), "success");
          } else {
            display_error(parsed.AVerificationResult);
          }
      })
      .catch(error => {
        console.log(error)
        display_message(i18next.t('forms.uploaderror'), "fail");
      });

    }

    Reader.readAsDataURL(PhotoField[0].files[0]);

  }

  showCreate() {
    resetInput("#detail_modal");
    $("#detail_modal img").attr("src", "");
    $("#detail_modal").attr("mode", "create");
    $("#detail_modal").find("select[name='ASponsorshipStatus'] option[value='CHILDREN_HOME']").prop('selected',true);
    $("#detail_modal").modal("show");
    this.showWindow(null, "details");
  }

})

var MaintainChildComments = new (class {
  constructor() {
    this.highest_index = 0;
  }

  build(result) {
    // builds the entrys as rows in there location
    // requires a list of PPartnerComment API data

    var CommentsFamily = $("#detail_modal [window=family_situations] .container-list").html("");
    var CommentsSchool = $("#detail_modal [window=school_situations] .container-list").html("");

    this.highest_index = 0;

    for (var comment of result) {
      var Copy = $("[phantom] .comment").clone();

      // save current highest index
      this.highest_index = comment["p_index_i"]

      // short comment in preview
      comment["p_comment_short_c"] = comment["p_comment_c"];
      if (comment["p_comment_c"].length > MAX_LENGTH_COMMENT_PREVIEW) {
        comment["p_comment_short_c"] = comment["p_comment_c"].substring(0, MAX_LENGTH_COMMENT_PREVIEW-2) + "..";
      }

      insertData(Copy, comment);
      switch (comment.p_comment_type_c) {
        case "FAMILY": CommentsFamily.append(Copy); break;
        case "SCHOOL": CommentsSchool.append(Copy); break;
      }
    }
  }

  showCreate(type) {

    var ddd = {
      "p_partner_key_n" : $("#detail_modal [name=p_partner_key_n]").val(),
      "p_index_i" : (this.highest_index + 1),
      "p_comment_c" : "",
      "p_comment_type_c" : type
    };

    insertData("#comment_modal", ddd);
    $("#comment_modal").attr("mode", "create");
    $("#comment_modal").modal("show");
  }

  saveEdit() {

    var req = translate_to_server(extractData($("#comment_modal")));

    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainChildComments', req).then(
      function (data) {
        var parsed = JSON.parse(data.data.d);

        if (!parsed.result) {
          return display_error(parsed.AVerificationResult);
        }

        $("#comment_modal").modal("hide");
        
        var tabname = null;
        switch (req["ACommentType"]) {
          case "FAMILY": tabname = "family_situations"; break;
          case "SCHOOL": tabname = "school_situations"; break;
        }
        
        MaintainChildren.edit(null, req["APartnerKey"], tabname);
      }
    );

  }

  edit(HTMLBottom) {
    HTMLBottom = $(HTMLBottom).closest(".comment");

    var comment_index = HTMLBottom.find("[name=p_index_i]").val();
    var partner_key = HTMLBottom.find("[name=p_partner_key_n]").val();

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

        insertData("#comment_modal", edit_comment);
        $("#comment_modal").attr("mode", "edit");
        $("#comment_modal").modal("show");

      }
    );
  }

})

var MaintainChildSponsorship = new (class {
  constructor() {

  }

  build(gifts, gift_details) {
    // builds the entrys as rows in their location
    // requires a list of ARecurringGiftDetail API data

    var SponsorList = $("#detail_modal [window=sponsorship] .container-list").html("");

    for (var sponsorship of gift_details) {

      if ((sponsorship.DonorAddress !== null) && (sponsorship.DonorAddress != '')) {
        sponsorship.DonorAddress += '<br/>';
      }
      if ((sponsorship.DonorEmailAddress !== null) && (sponsorship.DonorEmailAddress != '')) {
        sponsorship.DonorEmailAddress = '<a href="mailto:' + sponsorship.DonorEmailAddress + '">' + sponsorship.DonorEmailAddress + '</a><br/>';
      }

      var Copy = $("[phantom] .sponsorship").clone();
      insertData(Copy, sponsorship, sponsorship.CurrencyCode);
      SponsorList.append(Copy);
    }
  }

  showCreate() {

    var reset = {
      "a_gift_transaction_number_i":"-1",
      "a_batch_number_i":"-1",
      "a_detail_number_i":"-1",
      "a_motivation_group_code_c": "",
      "a_motivation_detail_code_c": "",
      "p_recipient_key_n":$("#detail_modal [name=p_partner_key_n]").val(),
      "a_ledger_number_i": window.localStorage.getItem("current_ledger")
    };

    resetInput("#recurring_modal");
    insertData("#recurring_modal", reset);
    $("#recurring_modal").modal("show");

  }

  saveEdit() {
    var req = translate_to_server(extractData($("#recurring_modal")));

    if (!req["ADonorKey"] || isNaN(parseInt(req["ADonorKey"]))) {
      return display_error("MaintainChildren.ErrMissingDonor");
    }

    if (!req["AGiftAmount"] || isNaN(parseFloat(req["AGiftAmount"]))) {
      return display_error("MaintainChildren.ErrMissingAmount");
    }

    if (!req["AStartDonations"]) {
      return display_error("MaintainChildren.ErrStartDonationsDate");
    }

    // check for endless date
    if (!req["AEndDonations"]) {
      req["AEndDonations"] = "null";
    }

    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainSponsorshipRecurringGifts', req).then(
      function (data) {
        var parsed = JSON.parse(data.data.d);

        if (!parsed.result) {
          return display_error(parsed.AVerificationResult);
        }

        $("#recurring_modal").modal("hide");
        MaintainChildren.edit(null, req["ARecipientKey"], "sponsorship");
      }
    );
  }

  delete(obj_modal) {
    let obj = $(obj_modal).closest('.modal');
    let payload = translate_to_server( extract_data(obj) );
    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_DeleteSponsorshipRecurringGift', payload).then(
      function (result) {
        var parsed = JSON.parse(result.data.d);

        if (!parsed.result) {
          return display_error(parsed.AVerificationResult);
        }

        $("#recurring_modal").modal("hide");
        MaintainChildren.edit(null, payload["ARecipientKey"], "sponsorship");
      });
  }

  edit(HTMLBottom, overwrite) {

    HTMLBottom = $(HTMLBottom).closest(".sponsorship");
    var req_detail = extractData(HTMLBottom);

    var req = {
      "APartnerKey": overwrite ? overwrite : $("#detail_modal [name=p_partner_key_n]").val(),
      "ALedgerNumber": window.localStorage.getItem("current_ledger")
    };
    req["AWithPhoto"] = false;

    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_GetChildDetails', req).then(
      function (data) {
        var parsed = JSON.parse(data.data.d);

        var recurring = parsed.result.ARecurringGift;
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

        insertData("#recurring_modal", searched);
        $("#recurring_modal").modal("show");

      }
    );

  }

})

var MaintainChildReminders = new (class {
  constructor() {
    this.highest_index = 0;
  }

  build(result) {
    // builds the entrys as rows in there location
    // requires a list of PPartnerReminder API data

    var Reminders = $("#detail_modal [window=dates_reminder] .container-list").html("");

    this.highest_index = 0;

    for (var reminder of result) {
      var Copy = $("[phantom] .reminder").clone();

      // save current highest index
      this.highest_index = reminder["p_reminder_id_i"]

      // short reminder in preview
      reminder["p_comment_short_c"] = reminder["p_comment_c"];
      if (reminder["p_comment_c"].length > MAX_LENGTH_COMMENT_PREVIEW) {
        reminder["p_comment_short_c"] = reminder["p_comment_c"].substring(0, MAX_LENGTH_COMMENT_PREVIEW-2) + "..";
      }

      insertData(Copy, reminder);
      Reminders.append(Copy);
    }
  }

  showCreate(type) {

    var ddd = {
      "p_partner_key_n": $("#detail_modal [name=p_partner_key_n]").val(),
      "p_reminder_id_i": (this.highest_index + 1),
      "p_comment_c" : "",
      "p_event_date_d": "",
      "p_first_reminder_date_d": ""
    };

    insertData("#reminder_modal", ddd);
    $("#reminder_modal").attr("mode", "create");
    $("#reminder_modal").modal("show");
  }

  saveEdit() {

    var req = translate_to_server(extractData($("#reminder_modal")));

    if (!req["AEventDate"]) {
      return display_error("MaintainChildren.ErrReminderEventDate");
    }

    if (!req["AFirstReminderDate"]) {
      return display_error("MaintainChildren.ErrFirstReminderDate");
    }

    if (!req["AComment"]) {
      return display_error("MaintainChildren.ErrEmptyReminderComment");
    }

    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainChildReminders', req).then(
      function (data) {
        var parsed = JSON.parse(data.data.d);

        if (!parsed.result) {
          return display_error(parsed.AVerificationResult);
        }

        $("#reminder_modal").modal("hide");
        MaintainChildren.edit(null, req["APartnerKey"], "dates_reminder");
      }
    );

  }

  edit(HTMLBottom) {
    HTMLBottom = $(HTMLBottom).closest(".reminder");

    var reminder_id = HTMLBottom.find("[name=p_reminder_id_i]").val();
    var partner_key = HTMLBottom.find("[name=p_partner_key_n]").val();

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

        insertData("#reminder_modal", edit_reminder);
        $("#reminder_modal").attr("mode", "edit");
        $("#reminder_modal").modal("show");

      }
    );
  }

})

function loadPartnerAdmins() {
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

      AfterLoadingComboboxes();
    }
  );
}

function loadChildrenStatus() {
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
      var TargetFields = $("#detail_modal [name=ASponsorshipStatus]").html('');
      for (var statustype of parsed.result) {
        let NewOption = $("<option></option>");
        NewOption.attr("value", statustype.p_type_code_c);
        let name = statustype.p_type_description_c;
        NewOption.text(name);
        TargetFields.append(NewOption);
      }

      AfterLoadingComboboxes();
    }
  );
}

// fix for muti modals, maybe move this to a global file?
$(document).ready(function () {

  $(document).on('show.bs.modal', '.modal', function (event) {
    var zIndex = 1040 + (10 * $('.modal:visible').length);
    $(this).css('z-index', zIndex);
    setTimeout(function() {
      $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
    }, 0);
  });

});
