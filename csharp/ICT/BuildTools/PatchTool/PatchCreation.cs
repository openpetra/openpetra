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
using ICSharpCode.SharpZipLib.BZip2;
using System.Collections;
using System.IO;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Tools.PatchTool
{
    /// <summary>
    /// create a patch file between 2 tar gz files
    /// </summary>
    public class PatchCreation
    {
        private static void PreparePatchTmpDirectory(String ATmpDirectory)
        {
            // empty temp directory
            // take a backup to be able to reuse existing diff files
            if (Directory.Exists(ATmpDirectory + "bak"))
            {
                Directory.Delete(ATmpDirectory + "bak", true);
            }

            if (Directory.Exists(ATmpDirectory))
            {
                Directory.Move(ATmpDirectory, ATmpDirectory + "bak");
            }

            Directory.CreateDirectory(ATmpDirectory);
        }

        private static void UnzipTarFile(String ATmpDirectory, String AFileName, String AAppName)
        {
            string[] directories;
            PreparePatchTmpDirectory(ATmpDirectory);

            // openpetraorg-0.0.10-0.tar.gz -> openpetraorg-0.0.10-0\bin30 etc
            PackTools.ExtractTarGz(ATmpDirectory, AFileName);

            // remove version number from directory name...
            directories = System.IO.Directory.GetDirectories(ATmpDirectory);

            foreach (string dir in directories)
            {
                if (Path.GetFileName(dir).IndexOf(AAppName + "-") == 0)
                {
                    System.IO.Directory.Move(dir, dir.Substring(0, dir.Length - Path.GetFileName(dir).Length) + AAppName);
                }
            }
        }

        private static void CreateDiffFiles(String ATmpDirectory,
            String ARootPatchDirectory,
            String AOldDirectory,
            String ANewDirectory,
            String ARecursiveSubDir)
        {
            string OldDirectory = AOldDirectory + '/' + ARecursiveSubDir;
            string NewDirectory = ANewDirectory + '/' + ARecursiveSubDir;
            string PatchDirectory = ARootPatchDirectory + '/' + ARecursiveSubDir;

            if (!Directory.Exists(PatchDirectory))
            {
                Directory.CreateDirectory(PatchDirectory);
            }

            if (!Directory.Exists(OldDirectory))
            {
                Directory.CreateDirectory(OldDirectory);
            }

            string[] directories = Directory.GetDirectories(NewDirectory);

            foreach (string dir in directories)
            {
                CreateDiffFiles(ATmpDirectory,
                    ARootPatchDirectory,
                    AOldDirectory, ANewDirectory, dir.Substring(ANewDirectory.Length + 1));
            }

            // compare the files file by file
            TLogging.Log("enter directory " + OldDirectory, TLoggingType.ToConsole);
            string[] files = System.IO.Directory.GetFiles(OldDirectory);

            foreach (string filenameLoop in files)
            {
                bool OldPatchFileReused = false;
                string filename = Path.GetFileName(filenameLoop);

                if (System.IO.File.Exists(NewDirectory + '/' + filename))
                {
                    TPatchTools patch = new TPatchTools();

                    // compare if files are actually different
                    if (!patch.IsSame(filenameLoop, NewDirectory + '/' + Path.GetFileName(filename)))
                    {
                        // create binary diff
                        if (!Directory.Exists(PatchDirectory))
                        {
                            Directory.CreateDirectory(PatchDirectory);
                        }

                        string OldPatchFile = ARootPatchDirectory + "bak" + PatchDirectory.Substring(ARootPatchDirectory.Length) + '/' +
                                              Path.GetFileName(filename) + ".patch";

                        if (System.IO.File.Exists(OldPatchFile))
                        {
                            // could reuse, if checksum of the result is the same
                            TPatchFileInfo OldPatchFileInfo;
                            patch.ReadHeader(OldPatchFile, out OldPatchFileInfo);

                            if (patch.CheckMd5Sum(NewDirectory + '/' + Path.GetFileName(filename),
                                    OldPatchFileInfo.NewMd5sum) && patch.CheckMd5Sum(filenameLoop, OldPatchFileInfo.OldMd5sum))
                            {
                                // found a match, so no need for recreating the patch file; just copy it
                                TLogging.Log("reusing diff " + OldPatchFile, TLoggingType.ToConsole);
                                System.IO.File.Copy(OldPatchFile, PatchDirectory + '/' + Path.GetFileName(filename) + ".patch");
                                OldPatchFileReused = true;
                            }
                        }

                        if (!OldPatchFileReused)
                        {
                            TLogging.Log("create diff " + PatchDirectory + '/' + Path.GetFileName(filename), TLoggingType.ToConsole);
                            patch.CreateDiff(filenameLoop, NewDirectory + '/' + Path.GetFileName(filename), PatchDirectory + '/' +
                                Path.GetFileName(filename) + ".patch");
                        }

                        // if this is a file required for the patch program, include the new version
                        if ((Path.GetFileName(filename) == "Ict.Common.dll") || (Path.GetFileName(filename) == "Ict.Common.IO.dll")
                            || (Path.GetFileName(filename) == "ICSharpCode.SharpZipLib.dll")
                            || (Path.GetFileName(filename).ToLower() == "Ict.Tools.PatchTool.exe".ToLower()))
                        {
                            // don't compress here:
                            // the whole patch directory is zipped anyways in the end;
                            // and it is easier to extract before running the patch
                            System.IO.File.Copy(NewDirectory + '/' + Path.GetFileName(filename), PatchDirectory + '/' + Path.GetFileName(
                                    filename), true);
                        }
                    }
                }
            }

            // add new files (zip them)
            files = System.IO.Directory.GetFiles(NewDirectory);

            foreach (string filename in files)
            {
                if (!System.IO.File.Exists(OldDirectory + '/' + Path.GetFileName(filename)))
                {
                    TLogging.Log("zip a new file " + NewDirectory + '/' + Path.GetFileName(filename), TLoggingType.ToConsole);

                    if (!Directory.Exists(PatchDirectory))
                    {
                        Directory.CreateDirectory(PatchDirectory);
                    }

                    try
                    {
                        BZip2.Compress(System.IO.File.OpenRead(NewDirectory + '/' + Path.GetFileName(filename)),
                            System.IO.File.OpenWrite(PatchDirectory + '/' + Path.GetFileName(filename) + ".new"), true, 4096);
                    }
                    catch (Exception)
                    {
                        throw new Exception("Cannot write to file " + PatchDirectory + '/' + Path.GetFileName(filename) + ".new");
                    }
                }
            }

            // tell to remove files
            files = System.IO.Directory.GetFiles(OldDirectory);

            foreach (string filename in files)
            {
                if (!System.IO.File.Exists(NewDirectory + '/' + Path.GetFileName(filename)))
                {
                    TLogging.Log("file removed: " + OldDirectory + '/' + Path.GetFileName(filename), TLoggingType.ToConsole);
                    FileStream stream = System.IO.File.OpenWrite(PatchDirectory + '/' + Path.GetFileName(filename) + ".rem");
                    stream.Close();
                }
            }

            // todo: something about changes to config files? xml file with instructions?
            // todo: check if all files can be matched when installing the patch
        }

        /// <summary>
        /// create a patch file containing all differences between two versions of the software
        /// </summary>
        public static void CreateDiff(String ATmpDirectory, String ADeliveryDirectory,
            String AAppName,
            String AZipName,
            String oldPatch, String newPatch)
        {
            if (Directory.Exists(ATmpDirectory + Path.DirectorySeparatorChar + oldPatch))
            {
                // never reuse an unzipped tar file, because it might be from a different language
                Directory.Delete(ATmpDirectory + Path.DirectorySeparatorChar + oldPatch, true);
            }

            string OldTarFile = ADeliveryDirectory + Path.DirectorySeparatorChar +
                                AZipName + "-" + oldPatch + ".tar.gz";

            if (File.Exists(OldTarFile))
            {
                UnzipTarFile(ATmpDirectory + Path.DirectorySeparatorChar + oldPatch,
                    OldTarFile,
                    AAppName);
            }

            if (Directory.Exists(ATmpDirectory + Path.DirectorySeparatorChar + newPatch))
            {
                // never reuse an unzipped tar file, because we might have done two builds with the same build number.
                // then the patch file would be old
                Directory.Delete(ATmpDirectory + Path.DirectorySeparatorChar + newPatch, true);
            }

            UnzipTarFile(ATmpDirectory + Path.DirectorySeparatorChar + newPatch,
                ADeliveryDirectory + Path.DirectorySeparatorChar + AZipName + "-" + newPatch + ".tar.gz",
                AAppName);

            string DiffDirectory = ATmpDirectory + '/' + "Patch-win" + "_" + oldPatch + '_' + newPatch;

            // clear the diff directory
            PreparePatchTmpDirectory(DiffDirectory);

            CreateDiffFiles(ATmpDirectory,
                DiffDirectory,
                ATmpDirectory + Path.DirectorySeparatorChar + oldPatch + Path.DirectorySeparatorChar + "openpetraorg-" + oldPatch,
                ATmpDirectory + Path.DirectorySeparatorChar + newPatch + Path.DirectorySeparatorChar + "openpetraorg-" + newPatch,
                string.Empty);

            // put it all into a zip file
            string ZipFileName = TAppSettingsManager.GetValue("OutputZipFilename");
            PackTools.ZipDirectory(DiffDirectory, DiffDirectory + "/../" + ZipFileName);

            // copy that file to the delivery directory
            System.IO.File.Copy(DiffDirectory + "/../" + ZipFileName,
                ADeliveryDirectory + Path.DirectorySeparatorChar + ZipFileName, true);
            TLogging.Log("Successfully created file " + ADeliveryDirectory + Path.DirectorySeparatorChar + ZipFileName);
        }
    }
}