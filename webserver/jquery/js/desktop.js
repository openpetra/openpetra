var screensLoaded = "";

function loadScreen(filenameWithoutExtension)
{
  if (screensLoaded.indexOf("["+filenameWithoutExtension+"]")==-1)
  {
    $.get(filenameWithoutExtension + ".html", function(data){
        $('#desktop').append(data);
    });  

    fileref=document.createElement('script');
    fileref.setAttribute("type","text/javascript");
    fileref.setAttribute("src", filenameWithoutExtension + ".js");
    document.getElementsByTagName("head")[0].appendChild(fileref);

    screensLoaded += "[" + filenameWithoutExtension + "]";
  }
  else
  {
    window[filenameWithoutExtension.replace("/","")]();
  }
}

jQuery(document).ready(function() {

    $("#usermanagement").click(function() {
       loadScreen("MSysMan/UserManagement");
       JQD.util.clear_active();
    });

    $("#logout").click(function() {
      $.ajax({
          type: "POST",
          url: "../server.asmx/Logout",
          data: "{}",
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          success: function(data, status, response) {
            result = JSON.parse(response.responseText);
            if (result['d'] == true)
            {
                // alert("Successful logged out");
                window.location = "Default.aspx";
            }
            else
            {
                alert("could not log out");
            }
          },
          error: function(response, status, error) {
            alert("Error: could not log out");
          },
          fail: function(msg) {
            alert("Fail: could not log out");
          }
        });      
    });

    return false;
});
