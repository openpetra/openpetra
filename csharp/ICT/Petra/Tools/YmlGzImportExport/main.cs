//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2017 by OM International
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
using System.Data;
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using Ict.Petra.Server.App.Core;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Server.MSysMan.ImportExport.WebConnectors;


namespace Ict.Petra.Tools.MSysMan.YmlGzImportExport
{
    /// This will import and export the database via YmlGz file
    public class TYmlGzImportExport
    {
        private static bool DumpYmlGz(string YmlFile)
        {
            string YmlGZData = TImportExportWebConnector.ExportAllTables();
            YmlFile = Path.GetFullPath(YmlFile);

            FileStream fs = new FileStream(YmlFile, FileMode.Create);
            byte[] buffer = Convert.FromBase64String(YmlGZData);
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();
            TLogging.Log("backup has been written to " + YmlFile);

            return true;
        }

        private static bool LoadYmlGz(string YmlFile)
        {
            string restoreFile = Path.GetFullPath(YmlFile);

            if (!File.Exists(restoreFile) || !restoreFile.EndsWith(".yml.gz"))
            {
                Console.WriteLine("invalid filename or no read permission for " + restoreFile + ", please try again");
                return false;
            }

            string YmlGZData = string.Empty;

            try
            {
                FileStream fs = new FileStream(restoreFile, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                YmlGZData = Convert.ToBase64String(buffer);
            }
            catch (Exception e)
            {
                TLogging.Log("cannot open file " + restoreFile);
                TLogging.Log(e.ToString());
                return false;
            }

            if (TImportExportWebConnector.ResetDatabase(YmlGZData))
            {
                TLogging.Log("backup has been restored from " + restoreFile);
            }
            else
            {
                TLogging.Log("there have been problems with the restore");
                return false;
            }

            return true;
        }

        /// main method
        public static void Main(string[] args)
        {
            new TAppSettingsManager();
            new TLogging();

            if (!TAppSettingsManager.HasValue("YmlGzFile") || !TAppSettingsManager.HasValue("Action"))
            {
                TLogging.Log("sample call: -C:../../etc/TestServer.config -Action:dump -YmlGzFile:test.yml.gz");
                TLogging.Log("sample call: -C:../../etc/TestServer.config -Action:load -YmlGzFile:test.yml.gz");
                Environment.Exit(-1);
            }

            string YmlFile = TAppSettingsManager.GetValue("YmlGzFile");
            string Action = TAppSettingsManager.GetValue("Action");

            TServerManager.TheServerManager = new TServerManager();

            bool ExitWithError = false;

            try
            {
                if (Action == "dump")
                {
                    if (!DumpYmlGz(YmlFile))
                    {
                        ExitWithError = true;
                    }
                }
                else if (Action == "load")
                {
                    if (!LoadYmlGz(YmlFile))
                    {
                        ExitWithError = true;
                    }
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                ExitWithError = true;
            }

            TServerManager.TheServerManager.StopServer();

            if (TAppSettingsManager.GetValue("interactive", "true") == "true")
            {
                Console.WriteLine("Please press Enter to continue...");
                Console.ReadLine();
            }

            if (ExitWithError)
            {
                Environment.Exit(-1);
            }
        }
    }
}
