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
using Ict.Common.IO;
using ICSharpCode.SharpZipLib.BZip2;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.DB;

namespace Ict.Tools.PatchTool.Library
{
    /// there is a patch between each build and the previous build.
    /// when we send out a patch, we include all the latest builds. they are all copied to netpatches,
    /// and picked up from there;
    /// the patch program (database patch) is only run at the end after all
    /// builds of that patch have been installed;
    /// the patch application program and required dlls need to be copied
    /// from bin to another location first and executed from there,
    /// so that they can be patched too;
    /// create a zip archive of the bin directory before applying any patches;
    public class PatchApplication
    {
        private static TFrmStatus StatusWindow;
        private static Thread StatusWindowThread;

        /// <summary>
        /// open the status window; used for patching the remote client
        /// </summary>
        public static void RunStatusWindow()
        {
            StatusWindow = new TFrmStatus();
            StatusWindow.ShowDialog();
        }

        /// <summary>
        /// add a status message to the status window
        /// </summary>
        public static void WriteToStatusWindow(String s)
        {
            if (StatusWindowThread == null)
            {
                StatusWindowThread = new Thread(new ThreadStart(RunStatusWindow));
                StatusWindowThread.IsBackground = true;
                StatusWindowThread.Start();

                while (StatusWindow == null)
                {
                    Thread.Sleep(100);
                }
            }

            if (!StatusWindow.IsHandleCreated || StatusWindow.IsDisposed)
            {
                return;
            }

            // avoid exception: Cross thread operation not valid
            StatusWindow.Invoke((MethodInvoker) delegate
                {
                    StatusWindow.AddLine(s);
                });
        }

        /// <summary>
        /// to be called from PetraClient
        /// the latest patch zip files have already been copied;
        /// </summary>
        public static void PatchRemoteInstallation()
        {
            String oldPatchVersion;
            TPetraPatchTools patchTools;
            Boolean startPetraClient;

            System.Diagnostics.Process PetraClientProcess;
            startPetraClient = true;
            TLogging.SetStatusBarProcedure(new TLogging.TStatusCallbackProcedure(WriteToStatusWindow));
            patchTools =
                new TPetraPatchTools(TAppSettingsManager.GetValue("OpenPetra.Path"),
                    TAppSettingsManager.GetValue("OpenPetra.Path") + Path.DirectorySeparatorChar + "bin" + TPatchTools.OPENPETRA_VERSIONPREFIX,
                    TAppSettingsManager.GetValue("OpenPetra.PathTemp"),
                    "",                  // appOpts.GetValue("OpenPetra.Path.Dat"),
                    "",
                    TAppSettingsManager.GetValue("OpenPetra.Path.Patches"),
                    TAppSettingsManager.GetValue("OpenPetra.Path.RemotePatches"));

            if (patchTools.CheckForRecentPatch())
            {
                oldPatchVersion = patchTools.GetCurrentPatchVersion();

                if (patchTools.PatchTheFiles())
                {
                    patchTools.CheckForRecentPatch();
                    TLogging.Log("The patch was installed successfully.");
                    TLogging.Log("Your OpenPetra was on patch " + oldPatchVersion + ", " + "and is now on patch " + patchTools.GetCurrentPatchVersion());
                }
                else
                {
                    startPetraClient = false;
                }
            }
            else
            {
                // todo: will this ever be executed? this should be checked by PetraClient
                if ((!patchTools.GetCurrentPatchVersion().Equals(patchTools.GetLatestPatchVersion())))
                {
                    TLogging.Log("Problem: You don't have all patches that are necessary for patching to the latest patch.");
                }
                else
                {
                    TLogging.Log("Note: There is no new patch to be installed.");
                }
            }

            if (StatusWindowThread != null)
            {
                StatusWindowThread.Join();
            }

            if (startPetraClient)
            {
                // restart Petra Client if patch was successful
                PetraClientProcess = new System.Diagnostics.Process();
                PetraClientProcess.EnableRaisingEvents = false;
                PetraClientProcess.StartInfo.FileName = TAppSettingsManager.GetValue("OpenPetra.Path.Bin") + Path.DirectorySeparatorChar +
                                                        "PetraClient.exe";
                PetraClientProcess.StartInfo.Arguments = "-C:\"" + TAppSettingsManager.GetValue("C") + "\"";
                PetraClientProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                PetraClientProcess.Start();
            }
        }

        /// <summary>
        /// check if the file can be renamed. if not, the program is still running.
        /// the patch cannot modify such files if the program is still running
        /// </summary>
        /// <param name="APathToExeFile"></param>
        /// <returns></returns>
        public static Boolean ProcessStillRunning(String APathToExeFile)
        {
            try
            {
                File.Move(APathToExeFile, APathToExeFile + ".test");
                File.Move(APathToExeFile + ".test", APathToExeFile);
            }
            catch (Exception)
            {
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// patchtool methods specific to OpenPetra patching and OpenPetra directory structure
    /// </summary>
    public class TPetraPatchTools : TPatchTools
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TPetraPatchTools(String AInstallPath,
            String ABinPath,
            String ATmpPath,
            String ADatPath,
            String ADBPath,
            String APatchesPath,
            String ARemotePatchesPath)
            : base(AInstallPath, ABinPath, TPatchTools.OPENPETRA_VERSIONPREFIX, ATmpPath, ADatPath, ADBPath, APatchesPath, ARemotePatchesPath)
        {
        }

        private Boolean GetMatch(String AFileName, out String AAction, out String ATargetFile)
        {
            AAction = Path.GetExtension(AFileName).Substring(1);
            AFileName = AFileName.Substring(0, AFileName.Length - AAction.Length - 1).Replace("\\", "/");
            ATargetFile = FInstallPath + AFileName.Substring(AFileName.IndexOf("/"));
            return true;
        }

        /// <summary>
        /// this is called by an external install program (innosetup or bash script).
        /// need to run RunDBPatches independently this is used for the Standalone Installer (and Network installer)
        /// </summary>
        /// <returns>true if any patch was installed</returns>
        public Boolean PatchTheFiles()
        {
            Boolean ReturnValue = false;

            // this can only be called after CheckForRecentPatch
            if (FListOfNewPatches == null)
            {
                throw new Exception("TPatchTools.InstallPatches: need to call CheckForRecentPatch first!");
            }

            // todo: RunDBPatch for patches that have not been applied to the database yet, although the binaries have been installed already?
            // this is probably not necessary for network; it is done by innosetup for standalone; also see RunDBPatches
            foreach (String patch in FListOfNewPatches.GetValueList())
            {
                // apply the patch
                if (!ApplyPetraPatch(patch))
                {
                    TLogging.Log("There is a problem with installing patch " + patch);
                    return false;
                }

                ReturnValue = true;
            }

            TLogging.Log("Patching has been finished.");
            return ReturnValue;
        }

        private void ApplyPatchRecursively(String APatchRootDirectory, String APatchDirectory)
        {
            string[] directories = System.IO.Directory.GetDirectories(APatchDirectory);

            foreach (string dir in directories)
            {
                ApplyPatchRecursively(APatchRootDirectory, dir);
            }

            TPatchTools patch = new TPatchTools();
            string[] files = System.IO.Directory.GetFiles(APatchDirectory);

            foreach (string filename in files)
            {
                // what to do with the file: add, remove, patch
                string action;
                String TargetFile;

                // find a match with the registered File Patterns
                if ((!GetMatch(filename.Substring(APatchRootDirectory.Length + 1), out action, out TargetFile)))
                {
                    throw new Exception("cannot find a destination path for file " + filename.Substring(APatchRootDirectory.Length + 1));
                }

                // make sure that the path exists
                string TargetPath = Path.GetDirectoryName(TargetFile);

                if (!Directory.Exists(TargetPath))
                {
                    Directory.CreateDirectory(TargetPath);
                }

                // Console.WriteLine(filename + ' ' + TargetFile);
                if (action == "new")
                {
                    if (System.IO.File.Exists(TargetFile))
                    {
                        // prepare for undo; make a copy of the original file
                        System.IO.File.Copy(TargetFile, TargetFile + ".bak", true);
                        File.Delete(TargetFile);
                    }

                    // unzip the file
                    BZip2.Decompress(System.IO.File.OpenRead(filename), System.IO.File.OpenWrite(TargetFile), true);
                }
                else if ((action == "patch") && File.Exists(TargetFile))
                {
                    try
                    {
                        // safety copy
                        System.IO.File.Copy(TargetFile, TargetFile + ".bak", true);

                        // apply patch
                        if (System.IO.File.Exists(TargetFile + ".new"))
                        {
                            System.IO.File.Delete(TargetFile + ".new");
                        }

                        patch.ApplyPatch(TargetFile, TargetFile + ".new", filename);

                        // remove original, rename file
                        if (System.IO.File.Exists(TargetFile + ".new"))
                        {
                            if (System.IO.File.Exists(TargetFile))
                            {
                                System.IO.File.Delete(TargetFile);
                            }

                            System.IO.File.Move(TargetFile + ".new", TargetFile);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("problem patching file " + TargetFile + ": " + e.Message);
                    }
                }
                else if (action == "rem")
                {
                    if (System.IO.File.Exists(TargetFile))
                    {
                        // safety copy
                        System.IO.File.Copy(TargetFile, TargetFile + ".bak");

                        // remove file
                        System.IO.File.Delete(TargetFile);
                    }
                }
                else if (action == "skip")
                {
                    // skip server files on a remote system
                }
            }
        }

        /// <summary>
        /// apply one single database patch.
        /// if this is the last patch, an email could be sent, etc
        /// </summary>
        /// <param name="ADesiredVersion"></param>
        /// <param name="ALastPatch"></param>
        /// <returns></returns>
        private Boolean RunDBPatch(TFileVersionInfo ADesiredVersion, Boolean ALastPatch)
        {
            // TODO: run sql script or code from DLL to update the database
            // TODO: if last patch, send an email to central support etc
            TLogging.Log("RunDBPatch " + ADesiredVersion.ToString());


            // find appropriate sql script that updates to this version
            string[] files = Directory.GetFiles(FPatchesPath,
                String.Format("*_{0}.{1}.{2}.sql",
                    ADesiredVersion.FileMajorPart,
                    ADesiredVersion.FileMinorPart,
                    ADesiredVersion.FileBuildPart));

            if (files.Length > 1)
            {
                throw new Exception("There are too many files for upgrading to version " + ADesiredVersion.ToString() + " in " + FPatchesPath);
            }

            if (files.Length == 0)
            {
                throw new Exception("We cannot find a file for upgrading to version " + ADesiredVersion.ToString() + " in " + FPatchesPath);
            }

            TLogging.Log("Applying " + Path.GetFileName(files[0]));

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction();

            // always upgrade the version number
            DBAccess.GDBAccessObj.ExecuteNonQuery(
                String.Format("UPDATE s_system_defaults SET s_default_value_c = '{0}.{1}.{2}-0' WHERE s_default_code_c='CurrentDatabaseVersion'",
                    ADesiredVersion.FileMajorPart,
                    ADesiredVersion.FileMinorPart,
                    ADesiredVersion.FileBuildPart), Transaction);

            StreamReader reader = new StreamReader(files[0]);
            string line;
            string stmt = string.Empty;

            while ((line = reader.ReadLine()) != null)
            {
                if (!line.Trim().StartsWith("--"))
                {
                    if (!line.Trim().EndsWith(";"))
                    {
                        stmt += line.Trim() + " ";
                    }
                    else
                    {
                        stmt += line.Trim();
                        DBAccess.GDBAccessObj.ExecuteNonQuery(stmt, Transaction);
                    }
                }

                if (stmt.Length > 0)
                {
                    DBAccess.GDBAccessObj.ExecuteNonQuery(stmt, Transaction);
                }
            }

            DBAccess.GDBAccessObj.CommitTransaction();

            return true;
        }

        /// <summary>
        /// runs all patches against the database;
        /// can send an email when the last patch has been applied (see RunDBPatch)
        /// </summary>
        public Boolean RunDBPatches()
        {
            Boolean ReturnValue = false;
            TFileVersionInfo dbVersion;
            TFileVersionInfo appVersion;
            TFileVersionInfo desiredVersion;
            Boolean lastPatch;

            DBAccess.GDBAccessObj = new TDataBase();
            DBAccess.GDBAccessObj.EstablishDBConnection(CommonTypes.ParseDBType(TAppSettingsManager.GetValue("Server.RDBMSType")),
                TAppSettingsManager.GetValue("Server.DBHostOrFile"),
                TAppSettingsManager.GetValue("Server.DBPort"),
                TAppSettingsManager.GetValue("Server.DBName"),
                TAppSettingsManager.GetValue("Server.DBUserName"),
                TAppSettingsManager.GetValue("Server.DBPassword"),
                "");

            dbVersion = GetDBPatchLevel();
            dbVersion.FilePrivatePart = 0;
            TLogging.Log("Current version of the database is " + dbVersion.ToString());

            if (dbVersion != null)
            {
                ReturnValue = true;

                StreamReader sr = new StreamReader(FPatchesPath + Path.DirectorySeparatorChar + "version.txt");
                appVersion = new TFileVersionInfo(sr.ReadLine());
                appVersion.FilePrivatePart = 0;
                TLogging.Log("We want to update to version " + appVersion.ToString());
                sr.Close();

                if (dbVersion.Compare(appVersion) == 0)
                {
                    // rerun the last patch; perhaps the patch file itself was patched
                    ReturnValue = RunDBPatch(dbVersion, true);
                }

                for (Int32 Counter = dbVersion.FileBuildPart + 1; Counter <= appVersion.FileBuildPart; Counter += 1)
                {
                    if (ReturnValue)
                    {
                        desiredVersion = new TFileVersionInfo(dbVersion);
                        desiredVersion.FileBuildPart = (ushort)Counter;
                        lastPatch = (Counter == appVersion.FileBuildPart);
                        ReturnValue = RunDBPatch(desiredVersion, lastPatch);
                    }
                }
            }

            return ReturnValue;
        }

        private TFileVersionInfo GetDBPatchLevel()
        {
            // TODO: read current patch level from the database (table s_system_defaults, default code CurrentDatabaseVersion)
            string currentVersion = (string)DBAccess.GDBAccessObj.ExecuteScalar(
                "SELECT s_default_value_c FROM s_system_defaults where s_default_code_c='CurrentDatabaseVersion'",
                IsolationLevel.ReadUncommitted);

            return new TFileVersionInfo(currentVersion);
        }

        private void UndoPatchRecursively(String APatchRootDirectory, String APatchDirectory)
        {
            string[] directories = System.IO.Directory.GetDirectories(APatchDirectory);

            foreach (string dir in directories)
            {
                UndoPatchRecursively(APatchRootDirectory, dir);
            }

            string[] files = System.IO.Directory.GetFiles(APatchDirectory);

            foreach (string filename in files)
            {
                // what to do with the file: add, remove, patch
                string Action;
                String TargetFile;

                // find a match with the registered File Patterns
                if ((!GetMatch(filename.Substring(APatchRootDirectory.Length + 1), out Action, out TargetFile)))
                {
                    TLogging.Log("cannot find a destination path for file " + filename.Substring(APatchRootDirectory.Length + 1));
                }
                else
                {
                    // Console.WriteLine(filename + ' ' + TargetFile);
                    if (Action == "new")
                    {
                        // delete the file
                        System.IO.File.Delete(TargetFile);

                        // if the file existed already before, restore the backup
                        if (System.IO.File.Exists(TargetFile + ".bak"))
                        {
                            System.IO.File.Move(TargetFile + ".bak", TargetFile);
                        }
                    }
                    else if (Action == "rem")
                    {
                        // restore the file
                        if (System.IO.File.Exists(TargetFile + ".bak") && (!System.IO.File.Exists(TargetFile)))
                        {
                            System.IO.File.Move(TargetFile + ".bak", TargetFile);
                        }
                    }
                    else if (Action == "patch")
                    {
                        // restore the file
                        if (System.IO.File.Exists(TargetFile + ".bak"))
                        {
                            if (System.IO.File.Exists(TargetFile))
                            {
                                System.IO.File.Delete(TargetFile);
                            }

                            System.IO.File.Move(TargetFile + ".bak", TargetFile);
                        }
                    }
                }
            }
        }

        private void CleanupPatchRecursively(String APatchRootDirectory, String APatchDirectory)
        {
            string[] directories = System.IO.Directory.GetDirectories(APatchDirectory);

            foreach (string dir in directories)
            {
                CleanupPatchRecursively(APatchRootDirectory, dir);
            }

            string[] files = System.IO.Directory.GetFiles(APatchDirectory);

            foreach (string filename in files)
            {
                // what to do with the file: add, remove, patch
                string Action;
                String TargetFile;

                // find a match with the registered File Patterns
                if ((!GetMatch(filename.Substring(APatchRootDirectory.Length + 1), out Action, out TargetFile)))
                {
                    TLogging.Log("cannot find a destination path for file " + filename.Substring(APatchRootDirectory.Length + 1));
                }
                else
                {
                    // Console.WriteLine(filename + ' ' + TargetFile);
                    if (Action == "new")
                    {
                        // delete the backup
                        if (System.IO.File.Exists(TargetFile + ".bak"))
                        {
                            System.IO.File.Delete(TargetFile + ".bak");
                        }
                    }
                    else if (Action == "rem")
                    {
                        // delete the backup
                        if (System.IO.File.Exists(TargetFile + ".bak"))
                        {
                            System.IO.File.Delete(TargetFile + ".bak");
                        }
                    }
                    else if (Action == "patch")
                    {
                        // delete the backup
                        if (System.IO.File.Exists(TargetFile + ".bak"))
                        {
                            System.IO.File.Delete(TargetFile + ".bak");
                        }
                    }
                }
            }
        }

        private Boolean ApplyPetraPatch(String APatchFile)
        {
            Boolean ReturnValue;
            String TempPath;

            string[] directories;
            String[] files;
            ReturnValue = false;
            TLogging.Log("installing patch " + APatchFile);

            // create temp directory
            TempPath = Path.GetFullPath(Path.GetTempPath() + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(APatchFile));

            if (Directory.Exists(TempPath))
            {
                //  remove old temp directory: for testing patches, otherwise we are using the old version
                Directory.Delete(TempPath, true);
            }

            Directory.CreateDirectory(TempPath);

            // extract the tar patch file into the temp directory
            PackTools.Unzip(TempPath, APatchFile);

            // make sure that PetraClient has been stopped
            if (PatchApplication.ProcessStillRunning(FBinPath + Path.DirectorySeparatorChar + "PetraClient.exe"))
            {
                TLogging.Log("Wait for PetraClient to stop...");

                while (PatchApplication.ProcessStillRunning(FBinPath + Path.DirectorySeparatorChar + "PetraClient.exe"))
                {
                    TLogging.Log("Wait for PetraClient to stop...");
                }
            }

            try
            {
                // apply the patch
                TLogging.Log("applying patch, please wait");

                // go through all directories in patch, all files
                directories = System.IO.Directory.GetDirectories(TempPath);

                foreach (string dir in directories)
                {
                    ApplyPatchRecursively(TempPath, dir);
                }

                // rename the manually patched .dll and .exe files in net-patches directory,
                // since they should be part of the official patch in bin directory

                // go through bin/netpatches (or sapatches or remotepatches)
                directories = System.IO.Directory.GetDirectories(FBinPath, "*patches");

                foreach (string dir in directories)
                {
                    // delete the file.old if it already exists, if there is a file without .old
                    // that way we prevent the deletion of nrr (non routine request) files that have gone to the wrong directory
                    files = System.IO.Directory.GetFiles(dir, "*.old");

                    foreach (string filename in files)
                    {
                        if (System.IO.File.Exists(filename.Substring(0, filename.Length - 4)))
                        {
                            System.IO.File.Delete(filename);
                        }
                        else
                        {
                            // restore the .dll file (nrr)
                            System.IO.File.Move(filename, filename.Substring(0, filename.Length - 4));
                        }
                    }

                    // rename .dll file to file.dll.old
                    files = System.IO.Directory.GetFiles(directories[0], "*.dll");

                    foreach (string filename in files)
                    {
                        System.IO.File.Move(filename, filename + ".old");
                    }

                    // rename .exe file to file.exe.old
                    files = System.IO.Directory.GetFiles(directories[0], "*.exe");

                    foreach (string filename in files)
                    {
                        System.IO.File.Move(filename, filename + ".old");
                    }
                }

                // delete the .dll.orig files, because we have applied the new version for all files
                // those files have been created manually when we give out special dll files (not recommended)
                files = System.IO.Directory.GetFiles(FBinPath, "*.orig");

                foreach (string filename in files)
                {
                    if (System.IO.File.Exists(filename.Substring(0, filename.Length - 5)))
                    {
                        System.IO.File.Delete(filename);
                    }
                }

                TLogging.Log("Patch " + APatchFile + " has been applied successfully!");
                ReturnValue = true;
            }
            catch (Exception e)
            {
                TLogging.Log("Patch could not be installed: " + e.Message);
                TLogging.Log(e.ToString());
                TLogging.Log(e.StackTrace);
                TLogging.Log("Restoring the situation before the patch...");

                // if unsuccessful, restore the previous situation;
                // go through all directories in patch, all files
                directories = System.IO.Directory.GetDirectories(TempPath);

                foreach (string dir in directories)
                {
                    UndoPatchRecursively(TempPath, dir);
                }

                TLogging.Log("Please contact your IT support and send the file " + TLogWriter.GetLogFileName());
            }

            // clean up the safety backup files of the patch
            directories = System.IO.Directory.GetDirectories(TempPath);

            foreach (string dir in directories)
            {
                CleanupPatchRecursively(TempPath, dir);
            }

            // delete temp directory recursively (and the backup zip files)
            Directory.Delete(TempPath, true);
            return ReturnValue;
        }
    }
}