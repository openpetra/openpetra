//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System.Collections.Generic;
using System.Data;
using System.IO;
using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using Ext.Net;
using Ict.Common;
using Ict.Common.IO;
using PetraWebService;

namespace Ict.Petra.WebServer.MConference
{
    public partial class TPageOnlineApplication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void TestDownload(object sender, DirectEventArgs e)
        {
            // EventMask ShowMask is not hidden after download: http://forums.ext.net/showthread.php?4331-CLOSED-Write-HttpResonse-OnEvent-Click
            // but it seems, no javascript can be executed while downloading a file
            X.Js.Call("TestAlert", "Preparing the download...");

            System.Threading.Thread.Sleep(2000);

            this.Response.Clear();
            this.Response.ContentType = "text/plain";
            this.Response.AddHeader("Content-Type", "text/plain");
            this.Response.AddHeader("Content-Disposition", "attachment; filename=test.txt");
            this.Response.Write("Hello World");
            // this.Response.WriteFile(PDFPath);
            this.Response.End();
        }

        protected void TestAlert(object sender, DirectEventArgs e)
        {
            //X.Js.Call("TestAlert", "Preparing the download...");

            // http://forums.ext.net/showthread.php?7714-CLOSED-1.0-Ext.Net.X.Mask.Show-outputs-invalid-script
            //X.Js.Call("ShowMask", "Preparing the download...");

            MaskConfig Config = new MaskConfig();

            Config.Msg = "Preparing the download...";
            X.Mask.Show(Config);
            X.Js.Call("TestAlert", "Preparing the download...");
            System.Threading.Thread.Sleep(2000);
            X.Mask.Hide();
        }
    }
}