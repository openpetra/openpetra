<%@ Page Language="C#" %>
<%@ Assembly Name="System.IO" %>
<%@ Assembly Name="Ict.Common" %>
<%@ Assembly Name="Ict.Petra.Server.app.JsClient" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Ict.Common" %>
<%@ Import Namespace="Ict.Petra.Server.app.JSClient" %>
<%
    Response.ContentType = "text/html";
    Response.CacheControl = "no-cache";
    string page = Request.QueryString["page"];

    if (page.Length > 0)
    {
        string path = TAppSettingsManager.GetValue("Forms.Path");
    
        using (StreamReader sr = new StreamReader(path + "/header.html"))
        {
            string header = sr.ReadToEnd();
            Response.Write(header);
        }

        string content = TUINavigation.LoadNavigationPage(page);
        content = content.Replace("OpenTab", "window.parent.OpenTab");
        Response.Write(content);
    }
%>
