<%@ Page Language="C#" %>
<%@ Import Namespace="System.IO" %>
<html>
	<head>
		<title>OpenPetra API</title>
		<base href='/api/'>
	</head>
<body>
<h1>API for OpenPetra</h1>
<%
	string [] fileEntries = Directory.GetFiles(HttpContext.Current.Server.MapPath("."));
	
        foreach (string fileName in fileEntries)
	{
		if (fileName.EndsWith(".asmx"))
		{
			Response.Write("<a href='" + Path.GetFileName(fileName) + "'>" + Path.GetFileNameWithoutExtension(fileName) + "</a><br/>");
		}
	}
%>
</body>
</html>
