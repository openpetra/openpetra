$("document").ready(function () {
  MaintainChildren.show();
});

var MaintainChildren = new (class {
  constructor() {

  }

  filterShow() {
      // get infos from the filter ans search with them
      var filter = extract_data($("#filter"));
      console.log(filter);
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
      )

  }
})
