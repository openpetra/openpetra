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
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;
using Ict.Common;

namespace Ict.Common.IO
{
    /// <summary>
    /// Some helpful wrapper functions for packing and unpacking files and directories.
    ///
    /// It is mainly used by the patch program.
    /// For some special cases it makes use of an external application, 7zip. This needs to be on the PATH environment variable.
    /// Based on the SharpZipLib library
    /// </summary>
    public class PackTools
    {
        /// <summary>
        /// overloaded
        /// </summary>
        /// <param name="ATargetDirectory"></param>
        /// <param name="AZipFileName"></param>
        public static void Unzip(String ATargetDirectory, String AZipFileName)
        {
            Unzip(ATargetDirectory, AZipFileName, null, false);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ATargetDirectory"></param>
        /// <param name="AZipFileName"></param>
        /// <param name="AFilesToUnzip">if nil, unzip all; otherwise only the specified files (specified by internal path and filename) are unzipped</param>
        /// <param name="ADisregardPaths">store the extracted files in the current directory, despite the directory structure in the zip file</param>
        /// <returns>void</returns>
        public static void Unzip(String ATargetDirectory, String AZipFileName, ArrayList AFilesToUnzip, Boolean ADisregardPaths)
        {
            ZipInputStream zs;
            FileStream fs;
            ZipEntry theEntry;
            String directoryName;
            String fileName;
            String targetDeepDirectory;
            Int32 sourcebytes = 0;

            Byte[] buffer = new Byte[4096];


            zs = new ZipInputStream(System.IO.File.OpenRead(AZipFileName));
            theEntry = zs.GetNextEntry();

            while (theEntry != null)
            {
                directoryName = Path.GetDirectoryName(theEntry.Name.Replace("\\", Path.DirectorySeparatorChar.ToString()));
                fileName = Path.GetFileName(theEntry.Name.Replace("\\", Path.DirectorySeparatorChar.ToString()));

                if ((AFilesToUnzip == null) || AFilesToUnzip.Contains(directoryName + Path.DirectorySeparatorChar + fileName))
                {
                    if (ADisregardPaths)
                    {
                        targetDeepDirectory = ATargetDirectory;
                    }
                    else
                    {
                        targetDeepDirectory = ATargetDirectory + Path.DirectorySeparatorChar + directoryName;
                    }

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(targetDeepDirectory);
                    }

                    if (fileName.Length > 0)
                    {
                        if (System.IO.File.Exists(targetDeepDirectory + Path.DirectorySeparatorChar + fileName))
                        {
                            // had some trouble with files being not fully restored
                            System.IO.File.Delete(targetDeepDirectory + Path.DirectorySeparatorChar + fileName);
                        }

                        fs = System.IO.File.OpenWrite(targetDeepDirectory + Path.DirectorySeparatorChar + fileName);

                        do
                        {
                            sourcebytes = zs.Read(buffer, 0, buffer.Length);
                            fs.Write(buffer, 0, sourcebytes);
                        } while (sourcebytes > 0);

                        fs.Close();
                    }
                }

                theEntry = zs.GetNextEntry();
            }
        }

        private static void ZipDirectoryRecursive(String ARootDirectory,
            String ADirectory,
            ref ZipOutputStream zs,
            StringCollection AExcludeDirectories)
        {
            ZipEntry entry;
            FileStream fs;
            Int32 sourcebytes = 0;

            byte[] buffer = new byte[2096];

            // if ADirectory in AExcludeDirectories then exit
            if ((AExcludeDirectories != null) && (AExcludeDirectories.Contains(Path.GetFileName(ADirectory))))
            {
                return;
            }

            string[] directories = System.IO.Directory.GetDirectories(ADirectory);

            foreach (string dir in directories)
            {
                ZipDirectoryRecursive(ARootDirectory, dir, ref zs, AExcludeDirectories);
            }

            string[] files = System.IO.Directory.GetFiles(ADirectory);

            foreach (string filename in files)
            {
                // Console.WriteLine('adding to zip ' + Path.GetFullPath(filename).tostring().Substring(ARootDirectory.Length+1));
                entry = new ZipEntry(Path.GetFullPath(filename).Substring(ARootDirectory.Length + 1));
                entry.DateTime = System.IO.File.GetLastWriteTime(filename);
                zs.PutNextEntry(entry);
                fs = System.IO.File.OpenRead(filename);

                do
                {
                    sourcebytes = fs.Read(buffer, 0, buffer.Length);
                    zs.Write(buffer, 0, sourcebytes);
                } while (sourcebytes > 0);

                fs.Close();
            }
        }

        /// <summary>
        /// create a zip file from a directory
        /// </summary>
        /// <param name="ADirectory"></param>
        /// <param name="AZipFileName"></param>
        /// <param name="AExcludeDirectories"></param>
        public static void ZipDirectory(String ADirectory, String AZipFileName, StringCollection AExcludeDirectories)
        {
            FileStream os;
            ZipOutputStream zs;

            if (AZipFileName.Length == 0)
            {
                AZipFileName = Path.GetFullPath(ADirectory + "/../" + Path.GetFileName(ADirectory) + ".zip");
            }

            TLogging.Log("Create zip archive " + AZipFileName, TLoggingType.ToConsole);
            os = System.IO.File.OpenWrite(AZipFileName);
            zs = new ZipOutputStream(os);
            zs.SetLevel(5);

            // need absolute path for root directory, otherwise problems
            ZipDirectoryRecursive(Path.GetFullPath(ADirectory), ADirectory, ref zs, AExcludeDirectories);
            zs.Finish();
            zs.Close();
            os.Close();
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ADirectory"></param>
        /// <param name="AZipFileName"></param>
        public static void ZipDirectory(String ADirectory, String AZipFileName)
        {
            ZipDirectory(ADirectory, AZipFileName, null);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ADirectory"></param>
        public static void ZipDirectory(String ADirectory)
        {
            ZipDirectory(ADirectory, "", null);
        }

        /// zip a utf8 string using gzip into a base64 encoded string
        public static string ZipString(string ATextToCompress)
        {
            Byte[] originalText = Encoding.UTF8.GetBytes(ATextToCompress);

            MemoryStream memoryStream = new MemoryStream();
            GZipOutputStream gzStream = new GZipOutputStream(memoryStream);

            gzStream.Write(originalText, 0, originalText.Length);
            gzStream.Flush();
            gzStream.Finish();
            memoryStream.Position = 0;

            Byte[] compressedBuffer = new byte[memoryStream.Length];
            memoryStream.Read(compressedBuffer, 0, compressedBuffer.Length);

            gzStream.Close();

            return Convert.ToBase64String(compressedBuffer);
        }

        /// unzip a base64 encoded string and return the original utf8 string using gzip
        public static string[] UnzipString(string ACompressedString)
        {
            List <String>Result = new List <string>();

            Byte[] compressedBuffer = Convert.FromBase64String(ACompressedString);

            MemoryStream memoryStream = new MemoryStream(compressedBuffer);
            GZipInputStream gzStream = new GZipInputStream(memoryStream);
            StreamReader sr = new StreamReader(gzStream);

            while (!sr.EndOfStream)
            {
                Result.Add(sr.ReadLine());
            }

            sr.Close();
            gzStream.Close();

            return Result.ToArray();
        }

        /// <summary>
        /// extract a tar gz file
        /// </summary>
        /// <param name="ATmpDirectory"></param>
        /// <param name="AFileName"></param>
        public static void ExtractTarGz(String ATmpDirectory, String AFileName)
        {
            FileStream fs;
            GZipInputStream fz;
            TarArchive tar;

            TLogging.Log("extract " + AFileName, TLoggingType.ToConsole);
            fs = System.IO.File.OpenRead(AFileName);
            fz = new GZipInputStream(fs);
            tar = TarArchive.CreateInputTarArchive(fz);
            tar.ExtractContents(ATmpDirectory);
            tar.Close();
            fz.Close();
            fs.Close();
        }

        /// <summary>
        /// extract a tar file
        /// </summary>
        /// <param name="ATmpDirectory"></param>
        /// <param name="AFileName"></param>
        public static void ExtractTar(String ATmpDirectory, String AFileName)
        {
            FileStream fs;
            TarArchive tar;

            TLogging.Log("extract " + AFileName, TLoggingType.ToConsole);
            fs = System.IO.File.OpenRead(AFileName);
            tar = TarArchive.CreateInputTarArchive(fs);
            tar.ExtractContents(ATmpDirectory);
            tar.Close();
            fs.Close();
        }

        /// <summary>
        /// extract a 7zip file, using external 7z installation
        /// </summary>
        /// <param name="ATmpDirectory"></param>
        /// <param name="ASrcFileName"></param>
        public static void Extract7Zip(String ATmpDirectory, String ASrcFileName)
        {
            System.Diagnostics.Process SZipProcess;

            // use the SevenZip from the tools directory
            TLogging.Log("extract " + ASrcFileName, TLoggingType.ToConsole);

            if ((!System.IO.File.Exists(ASrcFileName)))
            {
                throw new Exception("cannot open file " + ASrcFileName);
            }

            SZipProcess = new System.Diagnostics.Process();
            SZipProcess.EnableRaisingEvents = false;
            SZipProcess.StartInfo.FileName = "7z"; // 7z needs to be on the path, e.g. t:\petra\tools
            SZipProcess.StartInfo.Arguments = "x -o" + ATmpDirectory + ' ' + ASrcFileName;
            SZipProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            SZipProcess.EnableRaisingEvents = true;

            if (SZipProcess.Start())
            {
                while ((!SZipProcess.HasExited))
                {
                    Thread.Sleep(500);
                }
            }
        }

        /// <summary>
        /// create a tar file
        /// </summary>
        /// <param name="ADirectory"></param>
        /// <param name="ATarFileName"></param>
        public static void PackTar(String ADirectory, String ATarFileName)
        {
            TarArchive archive;
            FileStream outStream;
            TarEntry entry;

            if (ATarFileName.Length == 0)
            {
                ATarFileName = Path.GetFullPath(ADirectory + "/../" + Path.GetFileName(ADirectory) + ".tar");
            }

            TLogging.Log("Create tar archive " + ATarFileName, TLoggingType.ToConsole);
            System.IO.File.Delete(ATarFileName);
            outStream = System.IO.File.OpenWrite(ATarFileName);
            archive = TarArchive.CreateOutputTarArchive(outStream, TarBuffer.DefaultBlockFactor);
            archive.SetUserInfo(-1, "petra", -1, "petra");

            archive.RootPath = Path.GetFullPath(ADirectory).Replace("\\", "/");

            // just adding the whole directory does not work as expected, it adds an empty directory without a name
            string[] directories = System.IO.Directory.GetDirectories(ADirectory);

            foreach (string dir in directories)
            {
                entry = TarEntry.CreateEntryFromFile(dir);
                archive.WriteEntry(entry, true);
            }

            string[] files = System.IO.Directory.GetFiles(ADirectory);

            foreach (string filename in files)
            {
                entry = TarEntry.CreateEntryFromFile(filename);
                archive.WriteEntry(entry, false);
            }

            archive.Close();
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ADirectory"></param>
        public static void PackTar(String ADirectory)
        {
            PackTar(ADirectory, "");
        }

        /// <summary>
        /// unpack an SRPM file
        /// </summary>
        /// <param name="ATmpDirectory"></param>
        /// <param name="AFileName"></param>
        public static void ExtractSRPM(String ATmpDirectory, String AFileName)
        {
            // src.rpm => src.cpio.gz
            Extract7Zip(ATmpDirectory, AFileName);
            AFileName = Path.GetFileName(AFileName);
            AFileName = AFileName.Substring(0, AFileName.IndexOf("src.rpm"));

            // src.cpio.gz => src.cpio
            Extract7Zip(ATmpDirectory, ATmpDirectory + '/' + AFileName + "src.cpio.gz");
            System.IO.File.Delete(ATmpDirectory + '/' + AFileName + "src.cpio.gz");

            // src.cpio => tar.gz files
            Extract7Zip(ATmpDirectory, ATmpDirectory + '/' + AFileName + "src.cpio");
            System.IO.File.Delete(ATmpDirectory + '/' + AFileName + "src.cpio");
            string[] tarFiles = System.IO.Directory.GetFiles(ATmpDirectory, "*.gz");

            foreach (string fileName in tarFiles)
            {
                ExtractTarGz(ATmpDirectory, fileName);
                System.IO.File.Delete(fileName);
            }
        }
    }
}