/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Ict.Common;

namespace Ict.Tools.PatchTool
{
/// <summary>
/// Main program
/// </summary>
    public class Program
    {
        /// <summary>
        /// static main function
        /// </summary>
        public static void Main(string[] args)
        {
            try
            {
                // check command line
                TAppSettingsManager appOpts = new TAppSettingsManager(false);
                string TempPath = appOpts.GetValue("Petra.PathTemp");
                new TLogging(TempPath + Path.DirectorySeparatorChar + "PetraPatch.log");

                if (!appOpts.HasValue("action"))
                {
                    System.Console.WriteLine(
                        "patch creation:    patchtool -action:create -Petra.PathTemp:u:/tmp/patch -deliverypath:u:/delivery -oldversion:0.0.8-0 -newversion:0.0.10-0");
                    System.Console.WriteLine(
                        "patch application: patchtool -action:apply -Petra.PathTemp:u:/tmp/patch -diffzip:u:/tmp/patch/Patch2.2.3-5_2.2.4-3.zip -apppath:c:/Programme/OpenPetra.org -datpath:c:/Programme/OpenPetra.org/data30");
                    return;
                }

                String action = appOpts.GetValue("action");

                if (action.Equals("create"))
                {
                    PatchCreation.CreateDiff(TempPath,
                        appOpts.GetValue("deliverypath"),
                        appOpts.GetValue("oldversion"),
                        appOpts.GetValue("newversion"));
                }
                else if (action.Equals("apply"))
                {
                    // TODO PatchApplication.ApplyPatch(cmdOpts.GetOptValue("tmppath"));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                TLogging.Log(e.Message, TLoggingType.ToLogfile);
                TLogging.Log(e.StackTrace, TLoggingType.ToLogfile);
            }
        }
    }
}