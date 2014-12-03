<%@ Page Language="C#" %>
<%@ Assembly Name="System.IO" %>
<%@ Assembly Name="Ict.Common" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="Ict.Common" %>
<%
    Response.ContentType = "text/html";
    Response.CacheControl = "no-cache";
    string form = Request.QueryString["form"];

    if (form.Length > 0 && !form.Contains("/"))
    {
        string content = string.Empty;
        string path = TAppSettingsManager.GetValue("Forms.Path");
        StringBuilder result = new StringBuilder();
        StringBuilder script = new StringBuilder();

        using (StreamReader sr = new StreamReader(path + "/" + form + ".html"))
        {
            content = sr.ReadToEnd();
        }

        if (content.IndexOf("<head>") == -1)
        {
            using (StreamReader srHeader = new StreamReader(path + "/header.html"))
            {
                content = srHeader.ReadToEnd() + Environment.NewLine + 
                    "<body>" + Environment.NewLine + 
                    content + Environment.NewLine + 
                    "</body></html>";
            }
        }

        // search for a <form>.js file and include the code in the result
        if (content.Contains("\"" + form + ".js"))
        {
            content = content.Replace("\"" + form + ".js", "\"../forms/" + form + ".js");
        }
        else
        {
            if (File.Exists(path + "/" + form + ".js"))
            {
                using (StreamReader sr = new StreamReader(path + "/" + form + ".js"))
                {
                    string jsContent = sr.ReadToEnd();
                    jsContent = jsContent.Replace("url: \"server", "url: \"../server");
                    script.Append("<script>").Append(jsContent).Append("</script>");
                }
            }
        }

        content = content.Replace("</head>", script.ToString() + Environment.NewLine +
            "</head>");

        Int32 previousPosInclude = -1;
        Int32 newPosInclude;
        while ((newPosInclude = content.IndexOf("<!-- include ", previousPosInclude + 1)) != -1)
        {
            if (previousPosInclude != -1)
            {
                Int32 posAfterPreviousInclude = content.IndexOf("-->", previousPosInclude) + "-->".Length;
                result.Append(content.Substring(
                    posAfterPreviousInclude,
                    newPosInclude - posAfterPreviousInclude));
            }
            else
            {
                result.Append(content.Substring(0, newPosInclude));
            }

            string includeFilename = content.Substring(
                newPosInclude + "<!-- include ".Length,
                content.IndexOf("-->", newPosInclude) - "<!-- include ".Length - newPosInclude).Trim();

            if (includeFilename.EndsWith(".css") && File.Exists(path + "/../" + includeFilename))
            {
                using (StreamReader srCss = new StreamReader(path + "/../" + includeFilename))
                {
                    result.Append("<style>").Append(srCss.ReadToEnd()).Append("</style>");
                }
            }
            else if (includeFilename.EndsWith(".js") && File.Exists(path + "/../" + includeFilename))
            {
                using (StreamReader srJs = new StreamReader(path + "/../" + includeFilename))
                {
                    result.Append("<script>").Append(srJs.ReadToEnd()).Append("</script>");
                }
            }
            else
            {
                throw new Exception("Error: cannot find file " + includeFilename);
            }

            previousPosInclude = newPosInclude;
        }

        if (previousPosInclude != -1)
        {
            result.Append(content.Substring(content.IndexOf("-->", previousPosInclude) + "-->".Length));
        }
        else
        {
            result.Append(content);
        }
        
        Response.Write(result.ToString());
    }
%>
