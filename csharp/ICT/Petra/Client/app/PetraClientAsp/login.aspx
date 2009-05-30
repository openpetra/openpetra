<%@ Page Language="C#" %>
<%@ Import Namespace="System.Web.Security" %>
<html>
<script language="C#" runat=server>
	void Login_Click (object sender, EventArgs e)
	{
		if ((UserName.Value == "demo") && (UserPass.Value == "demo")) {
			FormsAuthentication.RedirectFromLoginPage (UserName.Value, PersistCookie.Checked);
		} else {
			Msg.Text = "Invalid Credentials: Please try again";
		}
	}
</script>
<head><title>OpenPetra.org Login</title>
<link rel="stylesheet" type="text/css" href="/mono-xsp.css">
</head>
<body><mono:MonoSamplesHeader runat="server"/>
<form runat=server>
	<h3><font face="Verdana">OpenPetra.org Login Page</font></h3>
	<table>
		<tr>
		<td>Username:</td>
		<td><input id="UserName" type="text" runat=server/></td>
		<td><ASP:RequiredFieldValidator ControlToValidate="UserName"
			 Display="Static" ErrorMessage="*" runat=server/></td>
		</tr>
		<tr>
		<td>Password:</td>
		<td><input id="UserPass" type=password runat=server/></td>
		<td><ASP:RequiredFieldValidator ControlToValidate="UserPass"
			 Display="Static" ErrorMessage="*" runat=server/></td>
		</tr>
		<tr>
		<td>Persistent Cookie:</td>
		<td><ASP:CheckBox id=PersistCookie runat="server" /> </td>
		<td></td>
		</tr>
	</table>
	<asp:button text="Login" OnClick="Login_Click" runat=server/>
	<p>
	<asp:Label id="Msg" ForeColor="red" Font-Name="Verdana" Font-Size="10" runat=server />
</form>
</body>
</html>

