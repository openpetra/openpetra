//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2014 by OM International
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
using System.Reflection;
using System.Threading;

namespace Ict.Tools.DevelopersAssistantUpdater
{
    class Program
    {
        // This application updates an application file that is running outside the \delivery\bin area
        //  by copying a file from the delivery\bin area to a specified location.
        // In principle the application is generic so could be adapted to update any file.
        // In practice, as it stands, it is used to update the Developers Assistant application

        /// <summary>
        /// The main entry point for the application
        /// </summary>
        /// <param name="args">The application expects one command line argument:  The target path and filename of the application to be updated.</param>
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            // This application is running in delivery\bin which is the same folder location as the source for the copy
            // The target location is specified in the first command line argument.
            // We use the target filename to work out what we are updating

            string targetPath = args[0];
            string targetFilename = Path.GetFileNameWithoutExtension(targetPath);

            string myPath = Assembly.GetExecutingAssembly().Location;
            string sourcePath = String.Empty;

            if (targetFilename.EndsWith(".DevelopersAssistant"))
            {
                sourcePath = myPath.Replace("Updater.exe", ".exe");
            }
            else
            {
                throw new NotImplementedException("The Updater application is not configured to update the specified application.");
            }

            // Wait a few seconds for the existing application to shut down
            Thread.Sleep(4000);

            // Copy the file
            bool success = false;
            try
            {
                File.Copy(sourcePath, targetPath, true);
                success = true;
            }
            catch (Exception)
            {
            }

            // For the OPDA we can append a command line parameter that will display a success/fail message
            string msg = "\"/M:Update";
            msg += (success) ? "Success\"" : "Fail\"";

            // Launch the target application with any parameters
            ProcessStartInfo si = new ProcessStartInfo(targetPath, msg);
            Process p = new Process();
            p.StartInfo = si;
            p.Start();
        }
    }
}