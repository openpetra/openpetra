//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2013-2017 by OM International
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
using System;
using System.Web;
using System.Web.UI;
using System.IO;
using Ict.Common;
using Ict.Petra.Server.App.WebService;

namespace Ict.Petra.WebServer
{
    public partial class TLoginWindow : System.Web.UI.Page
    {
        public string ServerUrl { get; set; }
        public string Filename { get; set; }
        public string Language { get; set; }

        protected string GetDownloadFile()
        {
            // in development environment, there is no client installer
            if (!Directory.Exists("client"))
            {
                return "NotAvailable";
            }

            string [] files = System.IO.Directory.GetFiles("client", "OpenPetraRemoteSetup-*.exe");
            string filename = String.Empty;
            foreach (string f in files)
            {
                filename=System.IO.Path.GetFileName(f);
            }

            return filename;
        }

        protected void DownloadFile()
        {
            FileInfo file = new FileInfo("client/" + GetDownloadFile());
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + this.Filename + "\"");
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.Flush();
            Response.TransmitFile(file.FullName);
            Response.End();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string ConfigFileName;
            // make sure the correct config file is used
            if (Environment.CommandLine.Contains("/appconfigfile="))
            {
                // this happens when we use fastcgi-mono-server4
                ConfigFileName = Environment.CommandLine.Substring(
                    Environment.CommandLine.IndexOf("/appconfigfile=") + "/appconfigfile=".Length);

                if (ConfigFileName.IndexOf(" ") != -1)
                {
                    ConfigFileName = ConfigFileName.Substring(0, ConfigFileName.IndexOf(" "));
                }
            }
            else
            {
                // this is the normal behaviour when running with local http server
                ConfigFileName = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "web.config";
            }

            new TAppSettingsManager(ConfigFileName);
            this.ServerUrl = TAppSettingsManager.GetValue("Server.Url", "demo.openpetra.org");

            // check for valid user
            TOpenPetraOrgSessionManager myServer = new TOpenPetraOrgSessionManager();

            if (myServer.IsUserLoggedIn())
            {
                // redirect to the main application
                this.Response.Redirect("/Main.aspx");
                return;
            }

            this.Filename = GetDownloadFile();
            this.Filename = this.Filename.Replace("-", "-" + ServerUrl + "-");

            if (Request["download"] == this.Filename)
            {
                DownloadFile();
                return;
            }

            this.Language="en";
            if ((Request.UserLanguages.Length > 1)
                && Request.UserLanguages[0].StartsWith("de")) {
                this.Language="de";
            }
        }
    }
}
