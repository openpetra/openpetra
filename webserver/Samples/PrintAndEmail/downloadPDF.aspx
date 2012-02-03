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
    if (HttpContext.Current.Request.Params["pdf-id"] != null && HttpContext.Current.Request.Params["pdf-id"].Length > 0)
    {
        string LinkFilename = TAppSettingsManager.GetValue("Server.PathData") + Path.DirectorySeparatorChar + "downloads" + Path.DirectorySeparatorChar +  Path.GetFileName(HttpContext.Current.Request.Params["pdf-id"]) + ".txt";
        
        StreamReader rw = new StreamReader(LinkFilename);
        string pdfFileName = TAppSettingsManager.GetValue("Server.PathData") + Path.DirectorySeparatorChar + rw.ReadLine() + Path.DirectorySeparatorChar + rw.ReadLine();
        
        Response.Buffer = true;
        Response.Clear();
        Response.ClearContent(); 
        Response.ClearHeaders();         
        Response.ContentType = "application/pdf";
        Response.AppendHeader("Content-Disposition","attachment; filename=application.pdf");
        TLogging.Log(pdfFileName);
        Response.WriteFile( pdfFileName );
        // comment Response.End() to avoid System.Threading.ThreadAbortException
        // see http://www.west-wind.com/Weblog/posts/368975.aspx
        Response.End();
    }
}
catch (System.Threading.ThreadAbortException)
{
}
catch (Exception e)
{
    TLogging.Log(e.ToString() + ": " + e.Message);
    TLogging.Log(e.StackTrace);
    return;
}    
%>

