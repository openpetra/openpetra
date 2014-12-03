<%@ Page Language="C#" %>
<%@ Assembly Name="Ict.Petra.Server.app.JsClient" %>
<%@ Assembly Name="Ict.Petra.Server.app.WebService" %>
<%@ Import Namespace="Ict.Petra.Server.app.JSClient" %>
<%@ Import Namespace="Ict.Petra.Server.App.WebService" %>
<%
    Response.ContentType = "text/javascript";
    Response.CacheControl = "no-cache";
	TOpenPetraOrgSessionManager myServer = new TOpenPetraOrgSessionManager();

	if (!myServer.IsUserLoggedIn())
	{
		Response.Write("unauthorized access");
		return;
	}
%>

jQuery(document).ready(function() {
    // TODO: disable modules the user has no access permissions for
    // TODO: translate menu items
<%

    Response.Write(TUINavigation.LoadNavigationUI());
%>
});


