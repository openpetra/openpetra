jQuery(document).ready(function() {

    $('#btnResetData').on('click', function (e) {
        $("#fileResetDatabase").click();
    });
    $('#fileResetDatabase').change(function() {
        var filename = $(this).val();

        // see http://www.html5rocks.com/en/tutorials/file/dndfiles/
        if (window.File && window.FileReader && window.FileList && window.Blob) {
            //alert("Great success! All the File APIs are supported.");
        } else {
          alert('The File APIs are not fully supported in this browser.');
        }

        var reader = new FileReader();

        reader.onload = (function(theFile) {
            return function(e) {
                s=e.target.result;
                showPleaseWait();
                base64EncodedFileContent = s.substring(s.indexOf("base64,") + "base64,".length);

                $.ajax({ 
                    type: "POST",
                    url: "serverMSysMan.asmx/TImportExportWebConnector_ResetDatabase",
                      data: JSON.stringify({
                            'AZippedNewDatabaseData': base64EncodedFileContent, 
                            }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(data, status, response) {
                            result = JSON.parse(response.responseText);
                            console.log(result['d']);
                            if (result['d'] == true) {
                                alert("Database has been uploaded");
                            } else {
                                alert("There has been a problem uploading the database");
                            }
                            hidePleaseWait();
                           //alert( "Data Uploaded: ");
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
            };
        })($(this)[0].files[0]);

        // Read in the file as a data URL.
        reader.readAsDataURL($(this)[0].files[0]);
    });
});

function showPleaseWait() {
    $('#myModal').modal();
}
function hidePleaseWait() {
    $('#myModal').modal('hide');
}
