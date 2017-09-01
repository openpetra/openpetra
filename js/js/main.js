function OpenTab(name, title)
{
    tabname = name.replace(/\//g, '_');
    if ($("#TabbedWindows:has(#tab" + tabname + ")").length == 0)
    {
        $("#TabbedWindows").append("<li class='tab' id='tab" + tabname + "'><a href='#'>" +
            "<button class='close closeTab' type='button' id='btnClose" + tabname + "'><span class='glyphicon glyphicon-remove'></span></button>" + title + "</a>" +
            "</li>");
        $("#tab" + tabname).click(function() { ActivateTab(this.getAttribute('id').substring(3)); });
        
        // fetch screen content from the server
        if (name.substring(0, "frm".length) === "frm" || name.indexOf('/frm') > 0)
        {
            src = "lib/loadform.aspx?form=" + tabname;
        }
        else // fetch navigation page
        {
            src = "lib/loadnavpage.aspx?page=" + tabname;
        }

        iframe = '<div class="OpenPetraWindow" id="wnd' + tabname + '">' + 
            '<iframe class="openpetraiframe" id="iframe' + tabname + '" frameborder="0" scrolling="no" src="' + src + '"/>' + 
            '</div>';

        $(iframe).appendTo('#containerIFrames');

        $("#btnClose" + tabname).click(function(e)
        {
            e.preventDefault();
            $("#tab"+tabname).hide();

            // find a neighboring tab that is visible
            focusTab = $("#tab"+tabname).prev();
            while (focusTab.length == 1 && focusTab.css("display") == "none")
            {
                focusTab = focusTab.prev();
            }

            if (focusTab.length == 0)
            {
                focusTab = $("#tab"+tabname).next();
                while (focusTab.length == 1 && focusTab.css("display") == "none")
                {
                    focusTab = focusTab.next();
                }
            }
                
            if (focusTab.length == 0)
            {
                OpenTab("frmHome", "Home");
            }
            else
            {
                ActivateTab(focusTab[0].id.substring(3));
            }
            return false;
        });
    }
    ActivateTab(tabname);
};

function ActivateTab(name)
{
    $(".nav-tabs .tab").removeClass("active");
    $(".OpenPetraWindow").hide();
    $("#tab" + name).show();
    $("#tab" + name).addClass("active");
    $("#wnd" + name).show();
    $('html, body').animate({
                        scrollTop: $("#containerIFrames").offset().top + $("#topnavigation").height
                    }, 1);
}

function AddMenuGroup(name, title, menuitems)
{
    $("#LeftNavigation").append("<a href='#' class='list-group-item' id='mnuGrp" + name + "'>" + title + "</a><ul class='nav' id='mnuLst" + name + "'></ul>");
    menuitems(name);
    $("#mnuGrp" + name).click(function() {
        if (!$(this).hasClass('active'))
        {
            $(".list-group-item").removeClass("active");
            $(".list-group .nav ").hide();
            $(this).addClass("active");
        }
        $(this).next().toggle();
    });
}

function AddMenuItem(parent, name, title, tabtitle)
{
    $("#mnuLst" + parent).append("<li><a href='#' id='" + name + "'>" + title + "</a></li>");
    $("#" + name).click(function() {OpenTab(this.id, tabtitle);});
}

function keepConnection() {
    $.ajax({
        url: '/serverSessionManager.asmx/PollClientTasks',
        success: function(data) {
            // console.log("keepConnection call succeeded");
        }});

    // call every 30 seconds
    setTimeout(keepConnection, 30000);
}

function init() {
    $('[data-toggle=offcanvas]').click(function() {
        $('.row-offcanvas').toggleClass('active');
        if ($('#btnHide').hasClass("hidden")) {
            $('#btnHide').removeClass("hidden");
        } else {
            $('#btnHide').toggle();
        }
        $('#btnShow').toggle();
    });

    setTimeout(keepConnection, 5000);
 
    $("#logout").click(function() {
      $.ajax({
          type: "POST",
          url: "serverSessionManager.asmx/Logout",
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

    OpenTab("frmHome", "Home");
    
    // Chrome: make sure, that after a refresh, we are scrolled to the top
    // see also http://stackoverflow.com/questions/9551396/prevent-refresh-jump-with-javascript
    $(window).on('beforeunload', function() {
        $(window).scrollTop(0);
    });
}
