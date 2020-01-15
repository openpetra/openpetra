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

    api.post('serverMSponsorship.asmx/TSponsorshipWebConnector_GetChildDetails', req).then(
      function (data) {
        var parsed = JSON.parse(data.data.d);

        var ASponsorshipStatus = parsed.ASponsorshipStatus;
        var partner = parsed.result.PPartner[0];
        var family = parsed.result.PFamily[0];

        insertData("#detail_modal", {"ASponsorshipStatus":ASponsorshipStatus});
        insertData("#detail_modal", partner);
        insertData("#detail_modal", family);
        $("#detail_modal [name='p_photo_b']").attr("src", partner.p_photo_b);

        $("#detail_modal").modal("show");
        console.log(ASponsorshipStatus);
        console.log(partner);
        console.log(family);
      }
    );

    uploadNewPhoto() {
      alert("TODO");
    }

  }

  showWindow(HTMLAnchor) {
    // hide all windows in #multi_window and only show the one related to the link
    // also updates buttons

    var show = $(HTMLAnchor).attr("show");

    // nav-bar
    $("[role=tablist] [show]").removeClass("active");
    $(`[role=tablist] [show=${show}]`).addClass("active");

    // window
    $("#multi_window [window]").hide();
    $(`#multi_window [window=${show}]`).show();
  }

})
