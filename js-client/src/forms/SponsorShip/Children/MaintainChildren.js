$("document").ready(function () {
  MaintainChildren.initRecurringGiftBatch();
  MaintainChildren.show();
});

var MaintainChildren = new (class {
  constructor() {

  }

  filterShow() {
      // get infos from the filter and search with them
      var filter = extract_data($("#filter"));
      this.show(filter);
  }

  initRecurringGiftBatch() {
    var param = {
      "ALedgerNumber": window.localStorage.getItem("current_ledger"),
      "ABankAccountCode": ""
    };

    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_InitRecurringGiftBatchForSponsorship', param);
  }

  show(filter={}) {
    // get data from server and show them

    var req = {
      "AFirstName": filter.AFirstName ? filter.AFirstName : "",
      "AFamilyName": filter.AFamilyName ? filter.AFamilyName : "",
      "APartnerStatus": filter.APartnerStatus ? filter.APartnerStatus : "",
      "ASponsorshipStatus": filter.ASponsorshipStatus ? filter.ASponsorshipStatus : "",
      "ASponsorAdmin": filter.ASponsorAdmin ? filter.ASponsorAdmin : "",
    };

    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_FindChildren', req).then(
      function (data) {
        var parsed = JSON.parse(data.data.d);
        var List = $("#result").html("");
        for (var entry of parsed.result) {
          var Copy = $("[phantom] .children").clone();
          insertData(Copy, entry);
          List.append(Copy);
        }
      }
    );
  }

  detail(HTMLButtom, overwrite) {
    // get details for the child the user clicked on and open modal

    var req = {
      "APartnerKey": overwrite ? overwrite : $(HTMLButtom).closest(".row").find("[name=p_partner_key_n]").val(),
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

        MaintainChildSponsorship.build(recurring, recurring_detail);
        MaintainChildComments.build(comments);
        MaintainChildReminders.build(reminder);

        $("#detail_modal [name='p_photo_b']").attr("src", "data:image/jpg;base64,"+family.p_photo_b);

        $("#detail_modal").attr("mode", "edit");
        $("#detail_modal").modal("show");
      }
    );

  }

  showWindow(HTMLAnchor, overwrite) {
    // hide all windows in #multi_window and only show the one related to the link
    // also updates buttons

    var show = $(HTMLAnchor).attr("show");
    if (overwrite) { show = overwrite; }

    // nav-bar
    $("[role=tablist] [show]").removeClass("active");
    $(`[role=tablist] [show=${show}]`).addClass("active");

    // window
    $("#multi_window [window]").hide();
    $(`#multi_window [window=${show}]`).show();
    $("#multi_window").attr("active", show);
  }

  saveEdit() {

    var MaintainChildrenO = this;
    var req = translate_to_server(extractData($("#detail_modal")));
    req["ALedgerNumber"] = window.localStorage.getItem("current_ledger");
    if (req["ADateOfBirth"] == "") {
      req["ADateOfBirth"] = "null";
    }

    var mode = $("#detail_modal").attr("mode");
    if (mode == "create") { req["APartnerKey"] = -1; }

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

    Reader.onload = function (event) {
      let file_content = event.target.result;
      file_content = btoa(file_content);

      var req = {
        "APartnerKey":$("#detail_modal [name=p_partner_key_n]").val(),
        "AUploadPhoto":true,
        "ADateOfBirth": "null",
        "ALedgerNumber": window.localStorage.getItem("current_ledger"),
        "APhoto":file_content
      };

      api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainChild', req)
      .then(function (data) {
          var parsed = JSON.parse(data.data.d);
          if (parsed.result) {
            display_message( i18next.t("forms.upload_success"), "success");
          } else {
            display_error(parsed.AVerificationResult);
          }
      });
    }

    Reader.readAsBinaryString(PhotoField[0].files[0]);

  }

  showCreate() {
    resetInput("#detail_modal");
    $("#detail_modal img").attr("src", "");
    $("#detail_modal").attr("mode", "create");
    $("#detail_modal select[name='ASponsorshipStatus'] option[value='CHILDREN_HOME']").attr('selected',true);
    $("#detail_modal").modal("show");
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
      if (comment["p_comment_c"].length > 32) {
        comment["p_comment_c"] = comment["p_comment_c"].substring(0, 30) + "..";
      }

      insertData(Copy, comment);
      switch (comment.p_comment_type_c) {
        case "FAMILY": CommentsFamily.append(Copy); break;
        case "SCHOOL": CommentsSchool.append(Copy); break;
        default: break;
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
        MaintainChildren.detail(null, req["APartnerKey"]);
      }
    );

  }

  detail(HTMLButtom) {
    HTMLButtom = $(HTMLButtom).closest(".comment");

    var comment_index = HTMLButtom.find("[name=p_index_i]").val();
    var partner_key = HTMLButtom.find("[name=p_partner_key_n]").val();

    var req = { "APartnerKey": partner_key };

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
    // builds the entrys as rows in there location
    // requires a list of ARecurringGiftDetail API data

    var SponsorList = $("#detail_modal [window=sponsorship] .container-list").html("");

    for (var sponsorship of gift_details) {
      var Copy = $("[phantom] .sponsorship").clone();
      insertData(Copy, sponsorship);
      SponsorList.append(Copy);
    }
  }

  showCreate() {

    var reset = {
      "a_gift_transaction_number_i":"-1",
      "a_batch_number_i":"-1",
      "a_detail_number_i":"-1",
      "p_recipient_key_n":$("#detail_modal [name=p_partner_key_n]").val(),
      "a_ledger_number_i": window.localStorage.getItem("current_ledger")
    };

    resetInput("#recurring_modal");
    insertData("#recurring_modal", reset);
    $("#recurring_modal").modal("show");

  }

  saveEdit() {
    var req = translate_to_server(extractData($("#recurring_modal")));

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
        MaintainChildren.detail(null, req["ARecipientKey"]);
      }
    );
  }

  detail(HTMLButtom, overwrite) {

    HTMLButtom = $(HTMLButtom).closest(".sponsorship");
    var req_detail = extractData(HTMLButtom);

    var req = {
      "APartnerKey": overwrite ? overwrite : $("#detail_modal [name=p_partner_key_n]").val(),
      "ALedgerNumber": window.localStorage.getItem("current_ledger")
    };
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
      if (reminder["p_comment_c"].length > 32) {
        reminder["p_comment_c"] = reminder["p_comment_c"].substring(0, 30) + "..";
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

    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainChildReminders', req).then(
      function (data) {
        var parsed = JSON.parse(data.data.d);
        $("#reminder_modal").modal("hide");
        MaintainChildren.detail(null, req["APartnerKey"]);
      }
    );

  }

  detail(HTMLButtom) {
    HTMLButtom = $(HTMLButtom).closest(".reminder");

    var reminder_id = HTMLButtom.find("[name=p_reminder_id_i]").val();
    var partner_key = HTMLButtom.find("[name=p_partner_key_n]").val();

    var req = { "APartnerKey": partner_key };

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
        NewOption.text(user.s_last_name_c + ", " + user.s_first_name_c);
        TargetFields.append(NewOption);
      }

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

  loadPartnerAdmins()

});
