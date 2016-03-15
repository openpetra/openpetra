//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2013 by OM International
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
using System.IO;
using System.Data;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO;

namespace Ict.Petra.Server.MCommon.Processing
{
    /// <summary>
    /// Processes the Automated Intranet Export.
    /// </summary>
    public class TProcessAutomatedIntranetExport
    {
        /// <summary>
        /// Gets called in regular intervals from a Timer in Class TTimedProcessing.
        /// </summary>
        /// <param name="ADataBaseObj">Instantiated DB Access object with opened DB connection.</param>
        /// <param name="ARunManually">this is true if the process was called manually from the server admin console</param>
        public static void Process(TDataBase ADataBaseObj, bool ARunManually)
        {
            //At the moment this isn't ready for production use
            return;

#if TODO
            //TODO: create the gpg files from IntranetExport, with parameters for including finance or personnel

            string exportpath = TAppSettingsManager.GetValue("OpenPetra.PathExport");

            if (Directory.Exists(exportpath) == false)
            {
                TLogging.Log("Auto Export directory not found : " + exportpath);
                return;
            }

            //if there is a file there, send it
            bool foundfile = false;

            foreach (FileInfo fi in new DirectoryInfo(exportpath).GetFiles("*.gpg"))
            {
                foundfile = true;

                TLogging.Log("Emailing file: " + fi.FullName);
                //TODO: Get the name of the administrator from the system_defaults table
                bool result = new TSmtpSender().SendEmail("<myemail@example.org>",
                    "OpenPetra Server",
                    "<testdata@intranet.example.org>",
                    "Automatic Intranet Export",
                    "This is an automatic file upload from OpenPetra",
                    new string[] { fi.FullName });

                if (result)
                {
                    TLogging.Log("SMTP Server accepted file: " + fi.FullName);
                    fi.Delete();
                    //TODO: exception handler
                }
                else
                {
                    TLogging.Log("SMTP Server refused file: " + fi.FullName);
                }
            }

            if (!foundfile)
            {
                TLogging.Log("No file found to email");
            }
#endif
        }
    }
}