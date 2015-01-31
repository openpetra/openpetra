<%@ Page Language="C#" %>
<!DOCTYPE html>
<html lang="en">
<body>

<%
string [] files = System.IO.Directory.GetFiles("/var/www/openpetra/client", "*.exe");

foreach (string file in files)
{
        Response.Write("<a href=\"" + System.IO.Path.GetFileName(file) + "\">" + System.IO.Path.GetFileName(file) +"</a> " + System.IO.File.GetLastWriteTime(file).ToString() + " " + new System.IO.FileInfo(file).Length.ToString() + " bytes<br/>\n");
}

files = System.IO.Directory.GetFiles("/var/www/openpetra/client", "*.zip");

foreach (string file in files)
{
        Response.Write("<a href=\"" + System.IO.Path.GetFileName(file) + "\">" + System.IO.Path.GetFileName(file) +"</a> " + System.IO.File.GetLastWriteTime(file).ToString() + " " + new System.IO.FileInfo(file).Length.ToString() + " bytes<br/>\n");
}
%>

</body>
</html>

