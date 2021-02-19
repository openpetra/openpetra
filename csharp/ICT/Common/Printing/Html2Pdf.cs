//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2021 by OM International
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
using System.Diagnostics;

using Ict.Common;
using Ict.Common.IO;

namespace Ict.Common.Printing
{
    /// usw wkhtmltopdf to print HTML to PDF
    public class Html2Pdf
    {
        /// <summary>
        /// Create a PDF file from the HTML
        /// </summary>
        public static bool HTMLToPDF(string AHtmlText, string AOutputPDFFilename)
        {
            // export HTML including the CSS to a single file.
            string HTMLFile = TFileHelper.GetTempFileName(
                "htmlreport",
                ".html");

            string CSSContent = String.Empty;

            // ApplicationDirectory points to eg. /home/openpetra/server/bin, we want /home/openpetra/
            string InstallPath = Path.GetFullPath(TAppSettingsManager.GetValue("ApplicationDirectory") + "/../../");

            using (StreamReader sr = new StreamReader(InstallPath + "client/css/report.css"))
            {
                CSSContent = sr.ReadToEnd();
            }

            string BootstrapCSSContent = String.Empty;
            using (StreamReader sr = new StreamReader(InstallPath + "bootstrap-4.0/bootstrap.min.css"))
            {
                BootstrapCSSContent = sr.ReadToEnd();
            }

            string BundledJSContent = string.Empty;
            using (StreamReader sr = new StreamReader(InstallPath + "bootstrap-4.0/bootstrap.bundle.min.js"))
            {
                BundledJSContent = sr.ReadToEnd();
            }

            using (StreamWriter sw = new StreamWriter(HTMLFile))
            {
                AHtmlText = AHtmlText.
                    Replace("<link href=\"/css/report.css\" rel=\"stylesheet\">",
                            "<style>" + 
                            BootstrapCSSContent + Environment.NewLine +
                            CSSContent + "</style>" + Environment.NewLine +
                            "<script>" + BundledJSContent + "</script>");
                sw.Write(AHtmlText);
                sw.Close();
            }

            Process process = new Process();
            process.StartInfo.FileName = TAppSettingsManager.GetValue("wkhtmltopdf.Path", "/usr/local/bin/wkhtmltopdf");
            process.StartInfo.Arguments = HTMLFile + " " + AOutputPDFFilename;
            process.Start();
            process.WaitForExit();

            File.Delete(HTMLFile);

            return true;
        }
    }
}
