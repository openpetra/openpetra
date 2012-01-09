<%@ Page Language="C#" %>
<%@ Assembly Name="Ict.Common" %>
<%@ Import Namespace="Ict.Common" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web" %>

<script runat="server">

private void UploadFile()
{
    Console.WriteLine("Uploading");
    if (Request.Files.Count != 1)
    {
        Console.WriteLine("invalid file count " + Request.Files.Count.ToString());
        Response.Write(String.Format("{{success:false, msg:{0}}}", "invalid file count " + Request.Files.Count.ToString()));
        return;
        // throw new Exception("Need one file to be uploaded");
    }

    HttpPostedFile MyFile = Request.Files[0];

    int FileLen = MyFile.ContentLength;
    if (FileLen > 10*1024*1024)
    {
        Console.WriteLine("we do not support files greater than 10 MB " + FileLen.ToString());
        Response.Write(String.Format("{{success:false, msg:{0}}}", "we do not support files greater than 10 MB " + FileLen.ToString()));
        return;
        // throw new Exception("we do not support files greater than 10 MB");
    }

    // TODO: convert to jpg
    if (Path.GetExtension(MyFile.FileName).ToLower() != ".jpg" && Path.GetExtension(MyFile.FileName).ToLower() != ".jpeg")
    {
        Console.WriteLine("we only support jpg files");
        Response.Write(String.Format("{{success:false, msg:{0}}}", "we only support jpg files"));
        return;
    }
    
    // TODO: scale image
    // TODO: rotate image
    // TODO: allow editing of image, select the photo from a square image etc
    
    string Filename;
    
    do
    {
        Filename = Path.GetTempFileName();
        Filename = Path.ChangeExtension(Filename, Path.GetExtension(MyFile.FileName));
    }
    while (File.Exists(Filename));

    MyFile.SaveAs(Filename);
    
    Response.Write(String.Format("{{success:true, file:'{0}'}}", Path.GetFileName(Filename)));
    return;
}

</script>

<%
if (Request.Files.Count == 1)
{
    UploadFile();
}
else
{
    if (HttpContext.Current.Request.Params["image-id"] != null && HttpContext.Current.Request.Params["image-id"].Length > 0)
    {
        string Filename = Path.GetDirectoryName(Path.GetTempFileName()) + Path.DirectorySeparatorChar + Path.GetFileName(HttpContext.Current.Request.Params["image-id"]);
        Response.Buffer = true;
        Response.Clear();
        Response.ClearContent(); 
        Response.ClearHeaders();         
        Response.ContentType = "image/jpeg";
        Response.AppendHeader("Content-Disposition","attachment; filename=photo.jpg");
        Response.WriteFile( Filename );
        Response.End();        
    }
}
%>
