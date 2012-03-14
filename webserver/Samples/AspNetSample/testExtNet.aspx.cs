using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Ext.Net;

namespace MyButtonTest
{
    public partial class TMyButtonTest : System.Web.UI.Page
    {
        protected void Button_Click(object sender, DirectEventArgs e)
        {
            X.Msg.Alert("Server Time", DateTime.Now.ToLongTimeString()).Show();
        }
    }
}