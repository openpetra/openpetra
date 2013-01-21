jQuery(document).ready(function() {
    var loginWindow = $("#window_login");
    JQD.util.window_flat();
    $(loginWindow).addClass('window_stack').show();
    // center the window
    loginWindow.css({top:'50%',left:'50%',margin:'-'+(loginWindow.height() / 2)+'px 0 0 -'+(loginWindow.width() / 2)+'px'});
    // enter key defaults to click button
    $("#password").keyup(function(event){
        if(event.keyCode == 13){
            $("#btnLogin").click();
        }
    });    

    $("#btnLogin").click(function() {
      $.ajax({
          type: "POST",
          url: "../server.asmx/Login",
          data: JSON.stringify({'username': $("#username").val(), 'password': $("#password").val()}),
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          success: function(data, status, response) {
            result = JSON.parse(response.responseText);
            if (result['d'] == true)
            {
                // alert("Successful logged in");
                window.location = "Desktop.aspx";
            }
            else
            {
                // alert("Wrong username or password, please try again");
                $('#window_login').effect('shake', { times:3 }, 200);
            }
          },
          error: function(response, status, error) {
            alert("Wrong username or password, please try again");
          },
          fail: function(msg) {
            alert("Fail: Wrong username or password, please try again");
          }
        });      
    });

    return false;
});
