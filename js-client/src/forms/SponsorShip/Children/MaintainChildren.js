$("document").ready(function () {
  MaintainChildren.show();
});

var MaintainChildren = new (class {
  constructor() {

  }

  filterShow() {
      // get infos from the filter ans search with them
      var filter = extract_data($("#filter"));
      this.show(filter);
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

  detail(HTMLButtom) {
    // get details for the child the user clicked on and open modal

    var req = {
      "APartnerKey": $(HTMLButtom).closest(".row").find("[name=p_partner_key_n]").val()
    };

    this.showWindow(null, "details");
    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_GetChildDetails', req).then(
      function (data) {
        var parsed = JSON.parse(data.data.d);

        console.log(parsed);

        var ASponsorshipStatus = parsed.ASponsorshipStatus;
        var partner = parsed.result.PPartner[0];
        var family = parsed.result.PFamily[0];

        insertData("#detail_modal", {"ASponsorshipStatus":ASponsorshipStatus});
        insertData("#detail_modal", partner);
        insertData("#detail_modal", family);
        $("#detail_modal [name='p_photo_b']").attr("src", "data:image/jpg;base64,"+family.p_photo_b);


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
  }

  saveEdit() {

    var req = translate_to_server(extractData($("#detail_modal")));

    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainChild', req).then(
      function (data) {
        var parsed = JSON.parse(data.data.d);
        if (parsed.result) {
          display_message( i18next.t("forms.saved"), "success");
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
      if (window.File && window.FileReader && window.FileList && window.Blob) {
    //alert("Great success! All the File APIs are supported.");
    } else {
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
        "APhoto":file_content
      };

      api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_MaintainChild', req)
      .then(function (data) {
          var parsed = JSON.parse(data.data.d);
          if (parsed.result) {
            display_message( i18next.t("forms.upload_success"), "success");
          }
      });
    }

    Reader.readAsBinaryString(PhotoField[0].files[0]);

  }

})
