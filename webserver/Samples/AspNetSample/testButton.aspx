<script runat="server">
protected void Button_Click(object sender, EventArgs e)
{
    lbl1.Text="Your name is " + txt1.Text;
}
</script>

<html>
<body>

<form runat="server">
Enter your name:
<asp:TextBox id="txt1" runat="server" />
<asp:Button OnClick="Button_Click" Text="Submit" runat="server" />
<p><asp:Label id="lbl1" runat="server" /></p>
</form>

</body>
</html>