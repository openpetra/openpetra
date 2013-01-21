<%@ Page Language="C#" %>
<%@ Assembly Name="Ict.Common" %>
<%@ Assembly Name="Ict.Common.IO" %>
<%@ Import Namespace="Ict.Common" %>
<%@ Import Namespace="Ict.Common.IO" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web" %>

<%
new TAppSettingsManager(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "web.config");
new TLogging(TAppSettingsManager.GetValue("Server.LogFile"));
try
{
    if (HttpContext.Current.Request.Params["id"] != null && HttpContext.Current.Request.Params["id"].Length > 0)
    {
        string Filename = TAppSettingsManager.GetValue("Server.PathData") + Path.DirectorySeparatorChar + "photos" + Path.DirectorySeparatorChar + Path.GetFileName(HttpContext.Current.Request.Params["id"]);
        Console.WriteLine("Serving " + Filename);
        if (!File.Exists(Filename))
        {
            Filename = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "img/default_blank.gif";
        }
        Response.Buffer = true;
        Response.Clear();
        Response.ClearContent(); 
        Response.ClearHeaders();         
        Response.ContentType = "image/jpeg";
        Response.AppendHeader("Content-Disposition","attachment; filename=photo.jpg");
        Response.WriteFile( Filename );
        // Response.End(); avoid System.Threading.ThreadAbortException 
    }
}
catch (Exception e)
{
    TLogging.Log(e.ToString() + ": " + e.Message);
    TLogging.Log(e.StackTrace);
}    
%>
