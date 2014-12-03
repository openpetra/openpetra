jQuery(document).ready(function() {
    $("#btnLogin").click(function(e) {
        e.preventDefault();
        user=$("#txtEmail").val();
        pwd=$("#txtPassword").val();
        if (user == "" && pwd == "") {
            user = "demo";
            pwd = "demo";
        }
        $.ajax({
          type: "POST",
          url: "serverSessionManager.asmx/Login",
          data: JSON.stringify({'username': user, 'password': pwd}),
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          success: function(data, status, response) {
            result = JSON.parse(response.responseText);
            if (result['d'] == 0)
            {
                // alert("Successful logged in");
                window.location = "Main.aspx";
            }
            else
            {
                alert("Wrong username or password, please try again");
                // $('#container').effect('shake', { times:3 }, 200);
            }
          },
          error: function(response, status, error) {
            console.debug(error);
            console.debug(JSON.stringify(response.responseJSON));
            alert("Server error, please try again later");
          },
          fail: function(msg) {
            alert("Server failure, please try again later");
          }
        });      
    });
});
