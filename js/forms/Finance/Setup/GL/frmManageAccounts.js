function ShowTree(html) {
    $("#trvAccountHierarchy").html(html);
}

// load account hierarchy from server
function LoadAccountHierarchy() {
    $.ajax({
      type: "POST",
      url: "serverMFinance.asmx/TGLSetupWebConnector_LoadAccountHierarchyHtmlCode",
      data: JSON.stringify({
            // TODO LedgerNumber not hard coded
            'ALedgerNumber': 43, 
            'AAccountHierarchyCode': 'STANDARD', 
            }),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(data, status, response) {
        result = JSON.parse(response.responseText);
        if (result['d'] != 0)
        {
            // alert("Successful result");
            ShowTree(result['d']);
            EnableTree();
            // collapse some branches
            $('#acctASSETS span')[0].click();
            $('#acctLIABS span')[0].click();
            $('#acctINC span')[0].click();
            $('#acctEXP span')[0].click();
        }
        else
        {
            console.debug("something went wrong");
        }
      },
      error: function(response, status, error) {
        console.debug(error);
        console.debug(JSON.stringify(response.responseJSON));
        alert("Server error, please try again later");
      },
      fail: function(msg) {
        console.debug(msg);
        alert("Server failure, please try again later");
      }
    });
}

function ExportAccountHierarchy() {
    $.ajax({
      type: "POST",
      url: "serverMFinance.asmx/TGLSetupWebConnector_ExportAccountHierarchyYmlGz",
      data: JSON.stringify({
            // TODO LedgerNumber not hard coded
            'ALedgerNumber': 43, 
            'AAccountHierarchyName': 'STANDARD', 
            }),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function(data, status, response) {
        result = JSON.parse(response.responseText);
        if (result['d'] != 0)
        {
            // using download feature from HTML5, see http://caniuse.com/download
            // and http://www.w3.org/TR/html/links.html#downloading-resources
            var pom = document.createElement('a');
            pom.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(JXG.decompress(result['d'])));
            pom.setAttribute('download', "accounthierarchy.yml");
            
            // For Mozilla we need to add the link, otherwise the click won't work
            // see https://support.mozilla.org/de/questions/968992
            document.body.appendChild(pom);
            
            pom.click();
        }
        else
        {
            console.debug("something went wrong");
        }
      },
      error: function(response, status, error) {
        console.debug(error);
        console.debug(JSON.stringify(response.responseJSON));
        alert("Server error, please try again later");
      },
      fail: function(msg) {
        console.debug(msg);
        alert("Server failure, please try again later");
      }
    });
}

jQuery(document).ready(function() {

    $('#btnExportAccountHierarchy').on('click', function (e) {
        ExportAccountHierarchy();
    });
    $('#btnImportAccountHierarchy').on('click', function (e) {
        $("#fileImportAccountHierarchy").click();
    });
    $('#fileImportAccountHierarchy').change(function() {
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
                    url: "serverMFinance.asmx/TGLSetupWebConnector_ImportAccountHierarchy",
                      data: JSON.stringify({
                            // TODO LedgerNumber not hard coded
                            'ALedgerNumber': 43, 
                            'AHierarchyName': 'STANDARD', 
                            'AYmlAccountHierarchy': base64EncodedFileContent.concat(":base64"), 
                            }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(){
                            LoadAccountHierarchy();
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
    
    LoadAccountHierarchy();
});

function showPleaseWait() {
    $('#myModal').modal();
}
function hidePleaseWait() {
    $('#myModal').modal('hide');
}
