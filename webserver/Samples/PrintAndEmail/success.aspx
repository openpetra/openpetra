<%@ Page Language="C#" %>
<html>
<head>
<title>Your application has been submitted successfully</title>
<body onload="window.open('<%Response.Write(HttpContext.Current.Request.Params["download"]);%>','Download')">
Thank you for your application for the Conference!<br/>
This is not a confirmation of the registration yet.<br/>
A PDF file has been sent to your email address with your registration details, and it should also automatically download now (otherwise you can click this link: <a href="<%Response.Write(HttpContext.Current.Request.Params["download"]);%>">download application form</a>)<br/>
You and your parents have to sign that document, and send it to us as a letter.<br/>
<br/>
We will confirm your registration as soon as we can.<br/>
<br/>
All the best,<br/>
  The Team from the conference

</body>
</html>
