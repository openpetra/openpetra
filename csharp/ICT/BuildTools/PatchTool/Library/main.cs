//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Tools.PatchTool.Library
{
/// <summary>
/// Main function for patch tool, can be called from gui or console
/// </summary>
    public class PatchToolLibrary
    {
        /// <summary>
        /// static run function
        /// </summary>
        public static bool Run()
        {
            try
            {
                // check command line
                new TAppSettingsManager(false);
                string TempPath = TAppSettingsManager.GetValue("OpenPetra.PathTemp");

                new TLogging(TempPath + Path.DirectorySeparatorChar + "PetraPatch.log");
//                Catalog.Init();

                String action = TAppSettingsManager.GetValue("action", false);

                if (action.Equals("create"))
                {
                    PatchCreation.CreateDiff(TempPath,
                        TAppSettingsManager.GetValue("deliverypath"),
                        TAppSettingsManager.GetValue("appname"),
                        TAppSettingsManager.GetValue("zipname"),
                        TAppSettingsManager.GetValue("oldversion"),
                        TAppSettingsManager.GetValue("newversion"));
                }
                else if (action.Equals("preparePatch"))
                {
                    // to be called before installing the patch;
                    // will only copy the files if there is a new patch available
                    TPetraPatchTools patchTools = new TPetraPatchTools(
                        TAppSettingsManager.GetValue("OpenPetra.Path"),
                        TAppSettingsManager.GetValue("OpenPetra.Path") + Path.DirectorySeparatorChar + "bin" + TPatchTools.OPENPETRA_VERSIONPREFIX,
                        TempPath,
                        "",
                        "",
                        TAppSettingsManager.GetValue("OpenPetra.Path.Patches"),
                        "");

                    if (patchTools.CheckForRecentPatch())
                    {
                        patchTools.CopyLatestPatchProgram(TAppSettingsManager.GetValue("OpenPetra.PathTemp"));
                    }
                    else
                    {
                        System.Console.WriteLine(Catalog.GetString("There is no new patch to be installed."));
                        return false;
                    }
                }
                else if (action.Equals("patchRemote"))
                {
                    // basically the same as patchFiles, but will use the status window.
                    PatchApplication.PatchRemoteInstallation();
                }
                else if (action.Equals("patchFiles"))
                {
                    // need to call first preparePatch;
                    // and then run the patch from TmpPatchPath so that the files can be overwritten
                    // this will patch the application files
                    TPetraPatchTools patchTools = new TPetraPatchTools(TAppSettingsManager.GetValue("OpenPetra.Path"),
                        TAppSettingsManager.GetValue("OpenPetra.Path") + Path.DirectorySeparatorChar + "bin" + TPatchTools.OPENPETRA_VERSIONPREFIX,
                        TempPath,
                        TAppSettingsManager.GetValue("OpenPetra.Path.Dat"),
                        "",
                        TAppSettingsManager.GetValue("OpenPetra.Path.Patches"),
                        "");

                    if (patchTools.CheckForRecentPatch())
                    {
                        if (!patchTools.PatchTheFiles())
                        {
                            System.Console.WriteLine(Catalog.GetString("There was a problem installing the patch."));
                            return false;
                        }
                    }
                    else
                    {
                        if ((!patchTools.GetCurrentPatchVersion().Equals(patchTools.GetLatestPatchVersion())))
                        {
                            System.Console.WriteLine(Catalog.GetString(
                                    "You don't have all patches that are necessary for patching to the latest patch."));
                            return false;
                        }
                        else
                        {
                            System.Console.WriteLine(Catalog.GetString("There is no new patch to be installed."));
                            return false;
                        }
                    }
                }
                else if (action.Equals("patchDatabase"))
                {
                    // this should only be called after patchFiles;
                    // this will possibly change the database structure, and modify database content
                    TPetraPatchTools patchTools = new TPetraPatchTools(TAppSettingsManager.GetValue("OpenPetra.Path"),
                        TAppSettingsManager.GetValue("OpenPetra.Path") + Path.DirectorySeparatorChar + "bin" + TPatchTools.OPENPETRA_VERSIONPREFIX,
                        TempPath,
                        "",
                        "",
                        TAppSettingsManager.GetValue("DBPatches.Path"),
                        "");

                    patchTools.RunDBPatches();
                }
                else
                {
                    System.Console.WriteLine(
                        "patch creation:    patchtool -action:create -OpenPetra.PathTemp:u:/tmp/patch -deliverypath:u:/delivery -oldversion:0.0.8-0 -newversion:0.0.10-0");
                    System.Console.WriteLine(
                        "patch application: patchtool -action:patchFiles -C:\"C:\\Program Files (x86)\\OpenPetra\\etc30\\PetraClientRemote.config\" -OpenPetra.Path:\"C:\\Program Files (x86)\\OpenPetra\"");
                    //-OpenPetra.PathTemp:u:/tmp/patch -diffzip:u:/tmp/patch/Patch2.2.3-5_2.2.4-3.zip -apppath:c:/Programme/OpenPetra.org -datpath:c:/Programme/OpenPetra.org/data30");
                    System.Console.WriteLine(
                        "patch application: patchtool -action:patchRemote -C:\"C:\\Program Files (x86)\\OpenPetra\\etc30\\PetraClientRemote.config\" -OpenPetra.Path:\"C:\\Program Files (x86)\\OpenPetra\"");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                TLogging.Log(e.Message, TLoggingType.ToLogfile);
                TLogging.Log(e.StackTrace, TLoggingType.ToLogfile);
                return false;
            }

            return true;
        }
    }
}