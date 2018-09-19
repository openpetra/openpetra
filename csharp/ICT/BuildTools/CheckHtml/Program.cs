//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <tp@tbits.net>
//
// Copyright 2004-2018 by OM International
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
using System.Text.RegularExpressions;

using Ict.Common;

namespace Ict.Tools.CheckHtml
{
    class Program
    {
        private static String sampleCall =
            "CheckHTML -formsdir:../../../openpetra-client-js/src/forms/";

        public static void Main(string[] args)
        {
            TCmdOpts cmd = new TCmdOpts();

            new TAppSettingsManager(false);

            TLogging.DebugLevel = TAppSettingsManager.GetInt32("debuglevel", 0);

            if (!cmd.IsFlagSet("formsdir"))
            {
                Console.WriteLine("call: " + sampleCall);
                return;
            }

            String FormsDir = cmd.GetOptValue("formsdir");

            try
            {
                CheckHTML(FormsDir);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(-1);
            }
        }

        private static void CheckHTML(string AFormsDir)
        {
            string Errors = string.Empty;
            foreach (var file in Directory.GetFiles(AFormsDir,
                         "*.html",
                         SearchOption.AllDirectories))
            {
                TLogging.Log("checking file " + file);
                string contents = File.ReadAllText(file);

                // Drop all white spaces
                contents = contents.Replace("\n", "");
                contents = contents.Replace("\t", "");
                contents = contents.Replace(" ", "");

                if (contents.Contains("tpl_edit"))
                {
                    // this is an edit window
                    if (!contents.Contains("modal-header")
                        || !contents.Contains("modal-body")
                        || !contents.Contains("modal-footer"))
                    {
                        Errors += file.Replace(AFormsDir, "") +
                            ": missing modal-header, modal-body or modal-footer for the tpl_edit window" +
                            Environment.NewLine;
                    }

                    Regex rgx = new Regex("<label.*?for=.*?>");

                    if (rgx.IsMatch(contents))
                    {
                        Errors += file.Replace(AFormsDir, "") +
                            ": don't use label with for because we have the phantom object for the template which would cause multiple ids." +
                            Environment.NewLine;
                        Match m = rgx.Match(contents);
                        Errors += " eg. " + m.Value + Environment.NewLine;
                    }
                }
            }

            if (Errors.Length > 0)
            {
                throw new Exception(Errors);
            }
        }
    }
}
