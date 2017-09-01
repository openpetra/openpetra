function ExportAllData() {
    showPleaseWait();
    $.ajax({
      type: "POST",
      url: "serverMSysMan.asmx/TImportExportWebConnector_ExportAllTables",
      data: JSON.stringify({
            }),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(data, status, response) {
        result = JSON.parse(response.responseText);
        if (result['d'] != 0)
        {
            //data = JXG.Util.Base64.decodeAsArray(result['d']);
            data = result['d'];
            //console.log(data);
            var byteString = atob(data);

            // Convert that text into a byte array.
            var ab = new ArrayBuffer(byteString.length);
            var ia = new Uint8Array(ab);
            for (var i = 0; i < byteString.length; i++) {
                ia[i] = byteString.charCodeAt(i);
            }

            var blob = new Blob([ia], {type : "application/gzip"});
            var url = URL.createObjectURL(blob);
            var a = document.createElement("a");
            a.style = "display: none";
            a.href = url;
            a.download = "exportedDatabase.yml.gz";

            // For Mozilla we need to add the link, otherwise the click won't work
            // see https://support.mozilla.org/de/questions/968992
            document.body.appendChild(a);

            a.click();
            URL.revokeObjectURL(url);
        }
        else
        {
            console.debug("something went wrong");
        }
        hidePleaseWait();
      },
      error: function(response, status, error) {
        console.debug(error);
        console.debug(JSON.stringify(response.responseJSON));
        alert("Server error, please try again later");
        hidePleaseWait();
      },
      fail: function(msg) {
        console.debug(msg);
        alert("Server failure, please try again later");
        hidePleaseWait();
      }
    });
}

jQuery(document).ready(function() {

    $('#btnExportData').on('click', function (e) {
        ExportAllData();
    });
});

function showPleaseWait() {
    $('#myModal').modal();
}
function hidePleaseWait() {
    $('#myModal').modal('hide');
}
