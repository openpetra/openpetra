<%@ Page Language="C#" %>
<%@ Assembly Name="Ict.Common" %>
<%@ Assembly Name="Ict.Common.IO" %>
<%@ Import Namespace="Ict.Common" %>
<%@ Import Namespace="Ict.Common.IO" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web" %>

<script runat="server">

private void UploadFile()
{
    if (Request.Files.Count != 1)
    {
        Console.WriteLine("invalid file count " + Request.Files.Count.ToString());
        Response.Write(String.Format("{{success:false, msg:'{0}'}}", "invalid file count " + Request.Files.Count.ToString()));
        return;
        // throw new Exception("Need one file to be uploaded");
    }

    HttpPostedFile MyFile = Request.Files[0];

    int FileLen = MyFile.ContentLength;
    if (FileLen > 5*1024*1024)
    {
        Console.WriteLine("we do not support files greater than 5 MB " + FileLen.ToString());
        Response.Write(String.Format("{{success:false, msg:'{0}'}}", "we do not support files greater than 5 MB " + FileLen.ToString()));
        return;
        // throw new Exception("we do not support files greater than 5 MB");
    }

    // TODO: convert to jpg
    if (Path.GetExtension(MyFile.FileName).ToLower() != ".jpg" && Path.GetExtension(MyFile.FileName).ToLower() != ".jpeg")
    {
        Console.WriteLine("we only support jpg files");
        Response.Write(String.Format("{{success:false, msg:'{0}'}}", "we only support jpg files"));
        return;
    }
    
    // TODO: scale image
    // TODO: rotate image
    // TODO: allow editing of image, select the photo from a square image etc
    
    string Filename = Path.GetTempFileName();
    
    MyFile.SaveAs(Filename);
    
    string md5SumFileName = TAppSettingsManager.GetValue("Server.PathTemp") + 
        Path.DirectorySeparatorChar +
        TPatchTools.GetMd5Sum(Filename) + ".jpg";
    try
    {
        if (!File.Exists(md5SumFileName))
        {
            File.Move(Filename, md5SumFileName);
        }
        else
        {
            File.Delete(Filename);
        }
        
        Response.Write(String.Format("{{success:true, file:'{0}'}}", Path.GetFileName(md5SumFileName)));
    }
    catch (Exception e)
    {
      TLogging.Log(e.Message);
      Response.Write(String.Format("{{success:false, msg:'{0}'}}", "some problem with uploading the photo"));
    }
    return;
}

</script>

<%
new TAppSettingsManager(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "web.config");
new TLogging(TAppSettingsManager.GetValue("Server.LogFile"));
try
{
    if (Request.Files.Count == 1)
    {
        UploadFile();
    }
    else
    {
        if (HttpContext.Current.Request.Params["image-id"] != null && HttpContext.Current.Request.Params["image-id"].Length > 0)
        {
            string Filename = TAppSettingsManager.GetValue("Server.PathTemp") + Path.DirectorySeparatorChar + Path.GetFileName(HttpContext.Current.Request.Params["image-id"]);
            Response.Buffer = true;
            Response.Clear();
            Response.ClearContent(); 
            Response.ClearHeaders();         
            Response.ContentType = "image/jpeg";
            Response.AppendHeader("Content-Disposition","attachment; filename=photo.jpg");
            Response.WriteFile( Filename );
            // comment Response.End() to avoid System.Threading.ThreadAbortException
            // see http://www.west-wind.com/Weblog/posts/368975.aspx
            Response.End();
        }
    }
}
catch (System.Threading.ThreadAbortException)
{
}
catch (Exception e)
{
    TLogging.Log(e.ToString() + ": " + e.Message);
    TLogging.Log(e.StackTrace);
}
%>
