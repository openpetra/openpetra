//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2015 by OM International
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
using System.Collections;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.BZip2;
using Ict.Common;
using Ict.Common.IO;
using System.Collections.Specialized;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Ict.Common.IO
{
    /// <summary>
    /// Some file version functions for working with patch files
    /// </summary>
    public class TPatchFileVersionInfo : TFileVersionInfo
    {
        /// <summary>
        /// constructor for base class
        /// </summary>
        /// <param name="version"></param>
        public TPatchFileVersionInfo(TFileVersionInfo version)
            : base(version)
        {
        }

        /// <summary>
        /// returns the version numbers of the patch;
        /// e.g. Patch-win_2.2.35_2.2.43.zip should return 2.2.35 and 2.2.43
        /// </summary>
        /// <param name="APatchZipFile"></param>
        /// <returns></returns>
        public static StringCollection GetVersionsFromDiffZipName(string APatchZipFile)
        {
            return StringHelper.StrSplit(Path.GetFileNameWithoutExtension(APatchZipFile).Substring(Path.GetFileNameWithoutExtension(APatchZipFile).
                    IndexOf("_") + 1), "_");
        }

        /// <summary>
        /// what is the latest patch; look at the name of the diff file
        /// </summary>
        /// <param name="APatchZipFile"></param>
        /// <returns></returns>
        public static TFileVersionInfo GetLatestPatchVersionFromDiffZipName(String APatchZipFile)
        {
            StringCollection versions = GetVersionsFromDiffZipName(APatchZipFile);

            if (versions.Count < 2)
            {
                throw new Exception("GetLatestPatchVersionFromDiffZipName: invalid name " + APatchZipFile);
            }

            return new TFileVersionInfo(versions[1]);
        }

        /// <summary>
        /// what is the start version; look at the name of the diff file
        /// </summary>
        /// <param name="APatchZipFile"></param>
        /// <returns></returns>
        public static TFileVersionInfo GetStartVersionFromDiffZipName(String APatchZipFile)
        {
            StringCollection versions = GetVersionsFromDiffZipName(APatchZipFile);

            return new TFileVersionInfo(versions[0]);
        }

        /// <summary>
        /// would this patch file apply to the current installed version
        /// </summary>
        /// <param name="APatchZipFile"></param>
        /// <returns></returns>
        public Boolean PatchApplies(String APatchZipFile)
        {
            return TPatchFileVersionInfo.PatchApplies(this, APatchZipFile);
        }

        /// <summary>
        /// would this patch file apply to the current installed version
        /// </summary>
        static public Boolean PatchApplies(TFileVersionInfo ACurrentVersion, string APatchZipFile)
        {
            StringCollection versions = GetVersionsFromDiffZipName(APatchZipFile);
            TFileVersionInfo patchStartVersion = new TFileVersionInfo(versions[0]);

            // generic patch
            if (patchStartVersion.FilePrivatePart == 0)
            {
                TFileVersionInfo patchEndVersion = new TFileVersionInfo(versions[1]);
                return patchEndVersion.Compare(ACurrentVersion) > 0;
            }

            return patchStartVersion.Compare(ACurrentVersion) == 0;
        }

        /// <summary>
        /// would this patch file apply to the current installed version
        /// </summary>
        /// <param name="APatchZipFile"></param>
        /// <param name="AMaxVersion">maximum version to upgrade to, usually this is the version of the exe files</param>
        /// <returns></returns>
        public Boolean PatchApplies(String APatchZipFile, TFileVersionInfo AMaxVersion)
        {
            try
            {
                StringCollection versions = GetVersionsFromDiffZipName(APatchZipFile);
                TFileVersionInfo patchStartVersion = new TFileVersionInfo(versions[0]);
                TFileVersionInfo patchEndVersion = new TFileVersionInfo(versions[1]);

                return patchStartVersion.Compare(this) == 0 && patchEndVersion.Compare(AMaxVersion) <= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    /// <summary>
    /// info about a patch file
    /// </summary>
    public class TPatchFileInfo
    {
        /// <summary>todoComment</summary>
        public DateTime NewFileDateTime;

        /// <summary>todoComment</summary>
        public string OldMd5sum;

        /// <summary>todoComment</summary>
        public string NewMd5sum;

        /// <summary>todoComment</summary>
        public TFileVersionInfo StoredVersion;

        /// <summary>todoComment</summary>
        public Int32 FormatVersion;

        /// <summary>todoComment</summary>
        public Int32 newsize;

        /// <summary>todoComment</summary>
        /// <summary>todoComment</summary>
        public Int32 bzctrllen;

        /// <summary>todoComment</summary>
        public Int32 bzdatalen;
    }

    /// <summary>
    /// This class provides functions for easy
    /// generation and application of binary patches to program files.
    ///
    /// It is based on the program bsdiff from Colin Percival.
    /// Find the original source and license here:
    /// http://www.daemonology.net/bsdiff/
    ///
    /// The modified format (based on BSDIFF40) is:
    ///   0 8 "ICTDIFF1"
    ///   8 8 X
    ///   16  8 Y
    ///   24  8 sizeof(newfile)
    ///   32 8 Date and Time of the new file (Ticks, Int64)
    ///   40 32 Md5sum of old file
    ///   72 32 Md5sum of new file
    ///   104 8 FileVersion of the old dll
    ///   112  X bzip2(control block)
    ///   112+X  Y bzip2(diff block)
    ///   112+X+Y  ??? bzip2(extra block)
    ///   112  X bzip2(control block)
    ///   112+X  Y bzip2(diff block)
    ///   112+X+Y  ??? bzip2(extra block)
    ///
    /// with control block a set of triples (x,y,z) meaning "add x bytes
    /// from oldfile to x bytes from the diff block; copy y bytes from the
    /// extra block; seek forwards in oldfile by z bytes".
    ///
    /// This implementation does only support BSDIFF40, version 2
    /// </summary>
    public class TPatchTools
    {
        #region Resourcestrings

        private static readonly string StrProblemConnecting = Catalog.GetString("There is a problem connecting to {0}");

        private static readonly string StrProblemConnectingTitle = Catalog.GetString("Cannot find patches on server");

        #endregion

        /// <summary>maximum is 9, quite slow, and memory consuming</summary>
        public const Int32 BZ_COMPRESSION_LEVEL = 5;

        /// <summary>our own user defined header for patch files</summary>
        public const Int32 HEADER_SIZE = 112;

        /// <summary>identify our patch version</summary>
        public const String FORMAT_DESCR = "OPODIFF1";

        /// <summary>we need that constants for finding the path for bin30 in the zip patch file</summary>
        public const string OPENPETRA_VERSIONPREFIX = "30";

        /// <summary>todoComment</summary>
        protected SortedList FListOfNewPatches;

        /// <summary>todoComment</summary>
        protected string FInstallPath;

        /// <summary>eg. bin30 has postfix 30</summary>
        protected string FVersionPostFix;

        /// <summary>todoComment</summary>
        protected string FBinPath;

        /// <summary>todoComment</summary>
        protected string FTmpPath;

        /// <summary>todoComment</summary>
        protected string FDatPath;

        /// <summary>todoComment</summary>
        protected string FPatchesPath;

        /// <summary>todoComment</summary>
        protected string FRemotePatchesPath;

        /// <summary>todoComment</summary>
        protected TFileVersionInfo FCurrentlyInstalledVersion;

        /// <summary>todoComment</summary>
        protected TFileVersionInfo FLatestAvailablePatch;

        /// <summary>
        /// constructor
        /// </summary>
        public TPatchTools()
        {
            FListOfNewPatches = null;
        }

        /// <summary>
        /// required for creating the binary diff
        /// </summary>
        private void offtout(Int64 x, ref byte[] buf)
        {
            Int64 y;

            y = x;

            if (x < 0)
            {
                y = -x;
            }

            buf[0] = (byte)(y % 256);
            y = y - buf[0];
            y = y / 256;
            buf[1] = (byte)(y % 256);
            y = y - buf[1];
            y = y / 256;
            buf[2] = (byte)(y % 256);
            y = y - buf[2];
            y = y / 256;
            buf[3] = (byte)(y % 256);
            y = y - buf[3];
            y = y / 256;
            buf[4] = (byte)(y % 256);
            y = y - buf[4];
            y = y / 256;
            buf[5] = (byte)(y % 256);
            y = y - buf[5];
            y = y / 256;
            buf[6] = (byte)(y % 256);
            y = y - buf[6];
            y = y / 256;
            buf[7] = (byte)(y % 256);

            if (x < 0)
            {
                buf[7] = (byte)(buf[7] | 0x80);
            }
        }

        /// <summary>
        /// required for applying a binary patch
        /// </summary>
        private Int64 offtin(byte[] buf, Int32 offset)
        {
            Int64 y;

            y = buf[offset + 7] & 0x7F;
            y = y * 256;
            y = y + buf[offset + 6];
            y = y * 256;
            y = y + buf[offset + 5];
            y = y * 256;
            y = y + buf[offset + 4];
            y = y * 256;
            y = y + buf[offset + 3];
            y = y * 256;
            y = y + buf[offset + 2];
            y = y * 256;
            y = y + buf[offset + 1];
            y = y * 256;
            y = y + buf[offset + 0];

            if ((buf[offset + 7] & 0x80) != 0)
            {
                y = -y;
            }

            return y;
        }

        /// <summary>
        /// required for applying a binary patch
        /// </summary>
        private Int32 loopread(ref BZip2InputStream zipStream, ref byte[] buf, Int32 offset, Int32 nbytes)
        {
            Int32 ptr;
            Int32 lenread;

            ptr = 0;

            while (ptr < nbytes)
            {
                lenread = zipStream.Read(buf, offset + ptr, nbytes);

                if (lenread == 0)
                {
                    return ptr;
                }

                if (lenread == -1)
                {
                    return -1;
                }

                ptr = ptr + lenread;
            }

            return ptr;
        }

        /// <summary>
        /// required for applying a binary patch
        /// </summary>
        private FileStream bz2read(ref BZip2InputStream bz, Int32 offset, string fname)
        {
            FileStream ReturnValue;
            FileStream fs;

            try
            {
                fs = new FileStream(fname, FileMode.Open, FileAccess.Read, FileShare.Read);
                fs.Seek(offset, SeekOrigin.Begin);
                bz = new BZip2InputStream(fs);
                ReturnValue = fs;
            }
            catch (Exception e)
            {
                throw new Exception("Cannot open file " + fname + " for reading; " + e.Message);
            }
            return ReturnValue;
        }

        /// <summary>
        /// required for creating the binary diff
        /// </summary>
        private void CreateDiff_External(String AFileName1,
            String AFileName2,
            out byte[] cb,
            out byte[] db,
            out byte[] eb,
            out Int32 cblen,
            out Int32 dblen,
            out Int32 eblen)
        {
            System.Diagnostics.Process BSDIFFProcess;
            BinaryReader r;
            FileStream fsTemp;
            Int32 i;
            byte[] header = new byte[32];

            string bsdiffPath = TAppSettingsManager.ApplicationDirectory + "/../../csharp/ThirdParty/bsdiff/bsdiff";

            if (Utilities.DetermineExecutingOS().ToString().StartsWith("eosWin"))
            {
                bsdiffPath += ".exe";
            }

            bsdiffPath = Path.GetFullPath(bsdiffPath);

            if (!File.Exists(bsdiffPath))
            {
                bsdiffPath = "bsdiff";
            }

            // make an external call to bsdiff, it will create a BSDIFF4.0 patch file
            BSDIFFProcess = new System.Diagnostics.Process();
            BSDIFFProcess.EnableRaisingEvents = false;
            BSDIFFProcess.StartInfo.FileName = bsdiffPath; // TODO bsdiff should be on the PATH
            BSDIFFProcess.StartInfo.Arguments = '"' + AFileName1 + "\" \"" + AFileName2 + "\" temp.patch";
            BSDIFFProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            BSDIFFProcess.EnableRaisingEvents = true;
            try
            {
                if (!BSDIFFProcess.Start())
                {
                    throw new Exception("cannot start bsdiff");
                }
            }
            catch (Exception)
            {
                TLogging.Log("Cannot start external bsdiff program. Is it on the path?");
                TLogging.Log("Arguments: " + BSDIFFProcess.StartInfo.Arguments);
                throw new Exception("Problem running bsdiff.exe");
            }

            while ((!BSDIFFProcess.HasExited))
            {
                Thread.Sleep(500);
            }

            try
            {
                fsTemp = new FileStream("temp.patch", FileMode.Open, FileAccess.Read);
            }
            catch (Exception)
            {
                throw new Exception("Cannot open the temporary file generated by bsdiff");
            }
            r = new BinaryReader(fsTemp);

            // read the standard BSDIFF header
            header = r.ReadBytes(32);

            if (header.Length != 32)
            {
                throw new Exception("Corrupt temp patch");
            }

            for (i = 0; i <= 7; i += 1)
            {
                if (header[i] != (byte)("BSDIFF40"[i]))
                {
                    throw new Exception("wrong version");
                }
            }

            cblen = (int)offtin(header, 8);
            dblen = (int)offtin(header, 16);
            eblen = (int)fsTemp.Length - (32 + cblen + dblen);
            cb = r.ReadBytes(cblen);
            db = r.ReadBytes(dblen);
            eb = r.ReadBytes(eblen);
            r.Close();
            fsTemp.Close();
        }

        /// <summary>
        /// Create a binary diff between AFileName1 and AFileName2, and store the differences in AFileNameDiff this version uses the C bsdiff program to generate the patch; much faster
        /// </summary>
        public void CreateDiff(String AFileName1, String AFileName2, String AFileNameDiff)
        {
            FileStream fs1;
            FileStream fs2;
            FileStream fsDiff;
            BinaryWriter w;
            Int64 newsize;

            byte[] cb;
            byte[] db;
            byte[] eb;
            Int32 cblen;
            Int32 dblen;
            Int32 eblen;
            byte[] header = new byte[HEADER_SIZE - 1 - 0 + 1];
            byte[] buf = new byte[7 - 0 + 1];
            Int32 i;
            FileVersionInfo ver;
            String oldMd5Sum;
            String newMd5Sum;
            CreateDiff_External(AFileName1, AFileName2, out cb, out db, out eb, out cblen, out dblen, out eblen);
            try
            {
                // open it, even though we don't need the file; just to be sure the external diff could access it as well
                fs1 = new FileStream(AFileName1, FileMode.Open, FileAccess.Read);
            }
            catch (Exception)
            {
                throw new Exception("Cannot open file " + AFileName1 + " for reading");
            }
            try
            {
                fs2 = new FileStream(AFileName2, FileMode.Open, FileAccess.Read);
            }
            catch (Exception)
            {
                throw new Exception("Cannot open file " + AFileName2 + " for reading");
            }
            try
            {
                fsDiff = new FileStream(AFileNameDiff, FileMode.Create);
            }
            catch (Exception)
            {
                throw new Exception("Cannot write to file " + AFileNameDiff);
            }

            // oldsize := fs1.Length;
            fs1.Close();
            newsize = fs2.Length;
            fs2.Close();
            w = new BinaryWriter(fsDiff);

            // for header format see the unit description
            for (i = 0; i <= 7; i++)
            {
                header[i] = (byte)FORMAT_DESCR[i];
            }

            for (i = 8; i <= HEADER_SIZE - 1; i += 1)
            {
                header[i] = 0;
            }

            w.Write(header);

            // insert the control block
            w.Write(cb, 0, cblen);

            // write the diff block
            w.Write(db, 0, dblen);

            // write the extra block
            w.Write(eb, 0, eblen);

            // write the lengths of each block to the header
            w.Seek(8, SeekOrigin.Begin);
            offtout(cblen, ref buf);
            w.Write(buf, 0, 8);
            w.Seek(16, SeekOrigin.Begin);
            offtout(dblen, ref buf);
            w.Write(buf, 0, 8);
            w.Seek(24, SeekOrigin.Begin);
            offtout(newsize, ref buf);
            w.Write(buf, 0, 8);

            // write the last modification time of the new file
            offtout(System.IO.File.GetLastWriteTime(AFileName2).Ticks, ref buf);
            w.Write(buf, 0, 8);

            // write the md5sum of the old file
            oldMd5Sum = GetMd5Sum(AFileName1);

            for (i = 0; i <= 31; i += 1)
            {
                w.Write(Convert.ToByte(oldMd5Sum[i]));
            }

            // write the md5sum of the new file
            newMd5Sum = GetMd5Sum(AFileName2);

            for (i = 0; i <= 31; i += 1)
            {
                w.Write(Convert.ToByte(newMd5Sum[i]));
            }

            // file version of the old dll
            ver = FileVersionInfo.GetVersionInfo(AFileName1);

            // each part of the version is a 16 bit value; together the version is saved in 64 bites
            w.Write(Convert.ToByte(ver.FileMajorPart / 256));
            w.Write(Convert.ToByte(ver.FileMajorPart % 256));
            w.Write(Convert.ToByte(ver.FileMinorPart / 256));
            w.Write(Convert.ToByte(ver.FileMinorPart % 256));
            w.Write(Convert.ToByte(ver.FileBuildPart / 256));
            w.Write(Convert.ToByte(ver.FileBuildPart % 256));
            w.Write(Convert.ToByte(ver.FilePrivatePart / 256));
            w.Write(Convert.ToByte(ver.FilePrivatePart % 256));
            w.Close();
            fsDiff.Close();
        }

        /// <summary>
        /// called by ApplyPatch, can also be useful to analyse patch files
        /// </summary>
        public Boolean ReadHeader(String APatchFile, out TPatchFileInfo patchFileInfo)
        {
            FileStream fsHeader;
            BinaryReader brHeader;
            Int32 i;

            byte[] header = new byte[HEADER_SIZE];
            patchFileInfo = new TPatchFileInfo();
            fsHeader = new FileStream(APatchFile, FileMode.Open);
            brHeader = new BinaryReader(fsHeader);

            if ((int)fsHeader.Length < HEADER_SIZE)
            {
                throw new Exception("Corrupt patch (1)");
            }

            // see header format in unit description
            header = brHeader.ReadBytes(HEADER_SIZE);
            brHeader.Close();
            fsHeader.Close();

            if (header.Length != HEADER_SIZE)
            {
                throw new Exception("Corrupt patch (2)");
            }

            for (i = 0; i <= 7; i++)
            {
                if (header[i] != (byte)(FORMAT_DESCR)[i])
                {
                    throw new Exception("wrong version");
                }
            }

            patchFileInfo.FormatVersion = 2;
            patchFileInfo.bzctrllen = (int)offtin(header, 8);
            patchFileInfo.bzdatalen = (int)offtin(header, 16);
            patchFileInfo.newsize = (int)offtin(header, 24);

            if ((patchFileInfo.bzctrllen < 0) || (patchFileInfo.bzdatalen < 0) || (patchFileInfo.newsize < 0))
            {
                throw new Exception("Corrupt patch (3)");
            }

            patchFileInfo.NewFileDateTime = new DateTime(offtin(header, 32));
            patchFileInfo.OldMd5sum = "";

            for (i = 0; i <= 31; i += 1)
            {
                patchFileInfo.OldMd5sum = patchFileInfo.OldMd5sum + Convert.ToChar(header[40 + i]);
            }

            patchFileInfo.NewMd5sum = "";

            for (i = 0; i <= 31; i += 1)
            {
                patchFileInfo.NewMd5sum = patchFileInfo.NewMd5sum + Convert.ToChar(header[72 + i]);
            }

            patchFileInfo.StoredVersion = new TFileVersionInfo();
            patchFileInfo.StoredVersion.FileMajorPart = (UInt16)(((UInt16)(header[104]) * 256 + header[105]));
            patchFileInfo.StoredVersion.FileMinorPart = (UInt16)(header[106] * 256 + header[107]);
            patchFileInfo.StoredVersion.FileBuildPart = (UInt16)(header[108] * 256 + header[109]);
            patchFileInfo.StoredVersion.FilePrivatePart = (UInt16)(header[110] * 256 + header[111]);
            return true;
        }

        /// <summary>
        /// Apply a patch to an existing file, and create a new file applies a single patch to one file; if the oldfile does not have the expected checksum:   if there is already a file with the additional extension .orig, then this is compared with
        /// the md5sum of the new file   also the old file is compared with the md5sum of the new file.   if the file is already there, it is copied to ANewFile generally: oldfile is not touched, and there is always a newfile created, either copied
        /// or patched. the orig file is left untouched
        /// </summary>
        public Boolean ApplyPatch(String AOldFile, String ANewFile, String APatchFile)
        {
            TPatchFileInfo patchFileInfo = null;
            BZip2InputStream bzCtrl = null;
            BZip2InputStream bzDiff = null;
            BZip2InputStream bzExtra = null;
            FileStream fsCtrl;
            FileStream fsDiff;
            FileStream fsExtra;
            FileStream fsOld;
            FileStream fsNew;
            BinaryReader brOld;
            BinaryWriter bwNew;
            Int32 oldsize;
            FileVersionInfo ver;
            Int32 i;
            Int32 oldpos;
            Int32 newpos;
            Int32 lenread;

            byte[] buf = new byte[9];

            if (AOldFile.Equals(ANewFile) == true)
            {
                throw new Exception("OldFileName name must be different from NewFileName");
            }

            ReadHeader(APatchFile, out patchFileInfo);

            if ((!CheckMd5Sum(AOldFile, patchFileInfo.OldMd5sum)))
            {
                // see if there is a file with extension .orig
                // we might have given the patched file already, and moved the original file to .orig
                if (System.IO.File.Exists(AOldFile + ".orig") && CheckMd5Sum(AOldFile + ".orig", patchFileInfo.OldMd5sum))
                {
                    System.IO.File.Copy(AOldFile + ".orig", AOldFile, true);
                }
                else if (System.IO.File.Exists(AOldFile + ".orig") && CheckMd5Sum(AOldFile + ".orig", patchFileInfo.NewMd5sum))
                {
                    System.IO.File.Copy(AOldFile + ".orig", ANewFile, true);
                }
                else if (CheckMd5Sum(AOldFile, patchFileInfo.NewMd5sum))
                {
                    // the new file is already there
                    System.IO.File.Copy(AOldFile, ANewFile, true);
                    return true;
                }
                else
                {
                    throw new Exception("different base file, md5sum does not match. Expected: " + patchFileInfo.OldMd5sum);
                }
            }

            ver = FileVersionInfo.GetVersionInfo(AOldFile);

            if (patchFileInfo.StoredVersion.Compare(new TFileVersionInfo(ver)) != 0)
            {
                throw new Exception(
                    "the existing file has an unexpected version number, expected " + patchFileInfo.StoredVersion.ToString() + ", but was " +
                    ver.FileVersion);
            }

            fsCtrl = bz2read(ref bzCtrl, HEADER_SIZE, APatchFile);
            fsDiff = bz2read(ref bzDiff, HEADER_SIZE + patchFileInfo.bzctrllen, APatchFile);
            fsExtra = bz2read(ref bzExtra, HEADER_SIZE + patchFileInfo.bzctrllen + patchFileInfo.bzdatalen, APatchFile);
            try
            {
                fsOld = new FileStream(AOldFile, FileMode.Open);
                brOld = new BinaryReader(fsOld);
            }
            catch (Exception)
            {
                throw new Exception("Cannot read file " + AOldFile);
            }
            oldsize = (int)fsOld.Length;
            byte[] old = brOld.ReadBytes(oldsize);

            if (old.Length != oldsize)
            {
                throw new Exception("old file " + AOldFile + " has invalid size");
            }

            brOld.Close();
            fsOld.Close();
            byte[] pNew = new byte[patchFileInfo.newsize + 1];
            Int32[] ctrl = new Int32[3];
            oldpos = 0;
            newpos = 0;

            while (newpos < patchFileInfo.newsize)
            {
                for (i = 0; i <= patchFileInfo.FormatVersion; i += 1)
                {
                    // we only support version 2
                    lenread = loopread(ref bzCtrl, ref buf, 0, 8);

                    if (lenread < 8)
                    {
                        throw new Exception("Corrupt patch (4)");
                    }

                    ctrl[i] = (Int32)offtin(buf, 0);
                }

                if (newpos + ctrl[0] > patchFileInfo.newsize)
                {
                    throw new Exception("Corrupt patch (5)");
                }

                lenread = loopread(ref bzDiff, ref pNew, newpos, ctrl[0]);

                if ((lenread < 0) || (lenread != ctrl[0]))
                {
                    throw new Exception("Corrupt patch (6)");
                }

                for (i = 0; i < ctrl[0]; i++)
                {
                    if ((oldpos + i >= 0) && (oldpos + i < oldsize))
                    {
                        pNew[newpos + i] = (byte)(((Int32)pNew[newpos + i] + (Int32)old[oldpos + i]) % 256);
                    }
                }

                newpos = newpos + ctrl[0];
                oldpos = oldpos + ctrl[0];

                if (patchFileInfo.FormatVersion == 2)
                {
                    if (newpos + ctrl[1] > patchFileInfo.newsize)
                    {
                        throw new Exception("Corrupt patch (7)");
                    }

                    lenread = loopread(ref bzExtra, ref pNew, newpos, ctrl[1]);

                    if ((lenread < 0) || (lenread != ctrl[1]))
                    {
                        throw new Exception("Corrupt patch (8)");
                    }

                    newpos = newpos + ctrl[1];
                    oldpos = oldpos + ctrl[2];
                }
            }

            // make sure there is nothing left to read
            if ((loopread(ref bzCtrl, ref buf, 0, 1) != 0)
                || (loopread(ref bzDiff, ref buf, 0, 1) != 0)
                || (loopread(ref bzExtra, ref buf, 0, 1) != 0))
            {
                throw new Exception("Corrupt patch (9)");
            }

            bzCtrl.Close();
            bzDiff.Close();
            bzExtra.Close();
            fsCtrl.Close();
            fsDiff.Close();
            fsExtra.Close();
            fsNew = new FileStream(ANewFile, FileMode.Create);
            bwNew = new BinaryWriter(fsNew);
            bwNew.Write(pNew, 0, patchFileInfo.newsize);
            bwNew.Close();
            fsNew.Close();
            System.IO.File.SetLastWriteTime(ANewFile, patchFileInfo.NewFileDateTime);

            if ((!CheckMd5Sum(ANewFile, patchFileInfo.NewMd5sum)))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// get the md5sum hash of a file
        /// </summary>
        /// <param name="ADLLName"></param>
        /// <returns></returns>
        public static String GetMd5Sum(String ADLLName)
        {
            String ReturnValue;
            MD5CryptoServiceProvider cr;
            FileStream fs;

            fs = new FileStream(ADLLName, FileMode.Open, FileAccess.Read);
            cr = new MD5CryptoServiceProvider();
            ReturnValue = BitConverter.ToString(cr.ComputeHash(fs)).Replace("-", "").ToLower();
            fs.Close();
            return ReturnValue;
        }

        /// <summary>
        /// check the hash sum of a file, and compare it to an expected value
        /// </summary>
        /// <param name="ADLLName"></param>
        /// <param name="AExpectedMd5Sum"></param>
        /// <returns></returns>
        public Boolean CheckMd5Sum(String ADLLName, String AExpectedMd5Sum)
        {
            return GetMd5Sum(ADLLName).Equals(AExpectedMd5Sum);
        }

        /// <summary>
        /// compare the size and md5sum of files
        /// </summary>
        public Boolean IsSame(String AFileName1, String AFileName2)
        {
            Int64 size1;
            Int64 size2;
            FileStream fs = System.IO.File.OpenRead(AFileName1);

            size1 = fs.Length;
            fs.Close();
            fs = System.IO.File.OpenRead(AFileName2);
            size2 = fs.Length;
            fs.Close();

            // only if size is the same make the effort to compare the checksums
            if (size1 == size2)
            {
                if (GetMd5Sum(AFileName1).Equals(GetMd5Sum(AFileName2)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// constructor;
        /// to be used to actually install patches
        /// </summary>
        public TPatchTools(String AInstallPath,
            String ABinPath,
            String AVersionPostFix,
            String ATmpPath,
            String ADatPath,
            String APatchesPath,
            String ARemotePatchesPath) : base()
        {
            FInstallPath = Path.GetFullPath(AInstallPath);
            FVersionPostFix = AVersionPostFix;
            FBinPath = ABinPath;

            if (ATmpPath.Length > 0)
            {
                FTmpPath = Path.GetFullPath(ATmpPath);
            }
            else
            {
                FTmpPath = "";
            }

            if (ADatPath.Length > 0)
            {
                FDatPath = Path.GetFullPath(ADatPath);
            }

            if (APatchesPath.Length > 0)
            {
                FPatchesPath = Path.GetFullPath(APatchesPath);
            }
            else
            {
                FPatchesPath = "";
            }

            if (ARemotePatchesPath.StartsWith("http://") || ARemotePatchesPath.StartsWith("https://"))
            {
                FRemotePatchesPath = ARemotePatchesPath;
            }
            else if (ARemotePatchesPath.Length > 0)
            {
                FRemotePatchesPath = Path.GetFullPath(ARemotePatchesPath);
            }
            else
            {
                FRemotePatchesPath = "";
            }

            FListOfNewPatches = new SortedList();
        }

        /// <summary>
        /// check whether there is a patch available; if this is a remote version, try to download a patch from the server this will also get the version of the currently installed code, and the list of patches that can be installed, in local
        /// variables
        /// </summary>
        public Boolean CheckForRecentPatch()
        {
            string StatusMessage;

            return CheckForRecentPatch(true, out StatusMessage);
        }

        /// <summary>
        /// check whether there is a patch available; if this is a remote version, try to download a patch from the server this will also get the version of the currently installed code, and the list of patches that can be installed, in local
        /// variables
        /// </summary>
        /// <param name="AShowStatus">Set to true to show status messages as a MessageBox
        /// if there is a problem, or to false to have them returned in
        /// <paramref name="AStatusMessage" /></param>.
        /// <param name="AStatusMessage">A Status Message in case there is a problem AND
        /// <paramref name="AShowStatus" /> is false.</param>
        public Boolean CheckForRecentPatch(bool AShowStatus, out string AStatusMessage)
        {
            string localname;
            TFileVersionInfo fileStartVersion;
            TFileVersionInfo filePatchVersion;
            string directoryListing;
            string searchPattern;
            string filesignature;

            AStatusMessage = "";
            FListOfNewPatches = new SortedList();

            // first get the version of the currently installed binaries
            // read from version.txt in the bin directory; it contains the currently installed version in RPM style (e.g. 3.0.0-14)
            if (!File.Exists(FBinPath + Path.DirectorySeparatorChar + "version.txt"))
            {
                throw new Exception(String.Format("Cannot search for new patch, since I cannot find file {0}", FBinPath +
                        Path.DirectorySeparatorChar + "version.txt"));
            }

            StreamReader srVersion = new StreamReader(FBinPath + Path.DirectorySeparatorChar + "version.txt");
            FCurrentlyInstalledVersion = new TFileVersionInfo(srVersion.ReadLine());
            srVersion.Close();

            if (FRemotePatchesPath.StartsWith("http://") || FRemotePatchesPath.StartsWith("https://"))
            {
                directoryListing = THTTPUtils.ReadWebsite(FRemotePatchesPath);

                if (directoryListing == null)
                {
                    if (AShowStatus)
                    {
                        MessageBox.Show(String.Format(StrProblemConnecting, FRemotePatchesPath),
                            StrProblemConnectingTitle);
                    }
                    else
                    {
                        AStatusMessage = String.Format(StrProblemConnecting, FRemotePatchesPath);
                    }

                    return false;
                }

                // find all the files names that match the pattern
                // eg <a href="Patch2.2.7-2_2.2.8-0.zip">
                searchPattern = "<a href=\"";

                while (directoryListing.IndexOf(searchPattern) != -1)
                {
                    string filename = directoryListing.Substring(
                        directoryListing.IndexOf(searchPattern) + searchPattern.Length,
                        directoryListing.Substring(
                            directoryListing.IndexOf(searchPattern) + searchPattern.Length).IndexOf('"'));
                    directoryListing = directoryListing.Substring(directoryListing.IndexOf(searchPattern) + searchPattern.Length);

                    // signature e.g: gr1100o.r">gr1100o.r</a>                    14-Oct-2008 13:11  153K
                    filesignature = directoryListing.Substring(0, directoryListing.IndexOf("\n"));

                    if (filename.ToLower().StartsWith("patch") && filename.ToLower().EndsWith(".zip"))
                    {
                        filePatchVersion = TPatchFileVersionInfo.GetLatestPatchVersionFromDiffZipName(filename);

                        if ((filePatchVersion.Compare(FCurrentlyInstalledVersion) > 0)
                            && (!System.IO.File.Exists(FPatchesPath + Path.DirectorySeparatorChar + Path.GetFileName(filename))))
                        {
                            string LocalName = FPatchesPath + Path.DirectorySeparatorChar + Path.GetFileName(filename);
                            FListOfNewPatches.Add(LocalName, LocalName);
                        }
                    }
                    else if (filename.Contains("Setup") && filename.EndsWith(".exe"))
                    {
                        // ignore setup executable
                    }
                    else if (filename.ToLower().EndsWith(".dll")
                             || filename.ToLower().EndsWith(".exe"))
                    {
                        // download .dll/.exe files from the netpatches directory if there is no file with same date already
                        if (System.IO.File.Exists(FPatchesPath + Path.DirectorySeparatorChar + filename + ".signature"))
                        {
                            StreamReader sr = new StreamReader(FPatchesPath + Path.DirectorySeparatorChar + filename + ".signature");

                            if (sr.ReadLine().Trim() == filesignature.Trim())
                            {
                                if (System.IO.File.Exists(FPatchesPath + Path.DirectorySeparatorChar + filename + ".old")
                                    && !System.IO.File.Exists(FPatchesPath + Path.DirectorySeparatorChar + filename))
                                {
                                    // the file has been renamed by the patch, and needs to be reinstated
                                    System.IO.File.Move(FPatchesPath + Path.DirectorySeparatorChar + filename + ".old",
                                        FPatchesPath + Path.DirectorySeparatorChar + filename);
                                }

                                filename = "ALREADY_LATEST";
                            }

                            sr.Close();
                        }

                        if (filename != "ALREADY_LATEST")
                        {
                            string localFile = FPatchesPath + Path.DirectorySeparatorChar + filename;

                            if (filename.ToLower().EndsWith(".dll")
                                || filename.ToLower().EndsWith(".exe"))
                            {
                                localFile = FBinPath + Path.DirectorySeparatorChar + filename;

                                // make backup copy of .dll/.exe if there does not already one exist; don't overwrite .orig files
                                if (!File.Exists(localFile + ".orig"))
                                {
                                    System.IO.File.Move(localFile, localFile + ".orig");
                                }
                            }

                            THTTPUtils.DownloadFile(FRemotePatchesPath + "/" + filename, localFile);
                            StreamWriter sw = new StreamWriter(FPatchesPath + Path.DirectorySeparatorChar + filename + ".signature");
                            sw.WriteLine(filesignature);
                            sw.Close();
                        }
                    }
                }

                // todo: remove local .r files if they are not in the net-patches directory
                // todo: the same for *.dll and *.exe; careful: only create an .orig copy once.
                // todo: somehow revert to .orig dll/exe file if there is no patched file in net-patches on the server?
                // todo: patch has to remove dll and exe from net-patches
                // todo: on the server side as well; check net-patches
            }
            else if ((FRemotePatchesPath.Length > 0) && (System.IO.Directory.Exists(FRemotePatchesPath)))
            {
                // check if there are newer patches on the remote server
                string[] files = System.IO.Directory.GetFiles(FRemotePatchesPath, "Patch*.zip");

                foreach (string filename in files)
                {
                    filePatchVersion = TPatchFileVersionInfo.GetLatestPatchVersionFromDiffZipName(filename);

                    if ((filePatchVersion.Compare(FCurrentlyInstalledVersion) > 0)
                        && ((!System.IO.File.Exists(FPatchesPath + Path.DirectorySeparatorChar + Path.GetFileName(filename)))))
                    {
                        localname = FPatchesPath + Path.DirectorySeparatorChar + Path.GetFileName(filename);
                        FListOfNewPatches.Add(localname, localname);
                    }
                }
            }

            // create a list of patches that should be installed
            string[] patchfiles = System.IO.Directory.GetFiles(FPatchesPath, "Patch*.zip");

            foreach (string filename in patchfiles)
            {
                fileStartVersion = TPatchFileVersionInfo.GetStartVersionFromDiffZipName(filename);
                filePatchVersion = TPatchFileVersionInfo.GetLatestPatchVersionFromDiffZipName(filename);

                if ((fileStartVersion.FilePrivatePart == 0 || fileStartVersion.Compare(FCurrentlyInstalledVersion) >= 0)
                    && (filePatchVersion.Compare(FCurrentlyInstalledVersion) > 0))
                {
                    FListOfNewPatches.Add(filename, filename);
                }
            }

            // see if any patches are missing; is there a direct line between FCurrentlyInstalledVersion and FLatestAvailablePatch?
            FListOfNewPatches = CheckPatchesConsistent(FListOfNewPatches);
            return FListOfNewPatches.Count > 0;
        }

        /// <summary>
        /// see if any patches are missing; is there a direct line between FCurrentlyInstalledVersion and FLatestAvailablePatch?
        /// </summary>
        /// <returns>return a list of all patches that should be applied. empty list if there is a problem</returns>
        public SortedList CheckPatchesConsistent(SortedList AOrderedListOfAllPatches)
        {
            SortedList ResultPatchList = new SortedList();
            TFileVersionInfo testPatchVersion;

            // get the latest patch that is available
            FLatestAvailablePatch = new TFileVersionInfo(FCurrentlyInstalledVersion);

            foreach (string patch in AOrderedListOfAllPatches.GetValueList())
            {
                testPatchVersion = TPatchFileVersionInfo.GetLatestPatchVersionFromDiffZipName(patch);

                if (testPatchVersion.Compare(FLatestAvailablePatch) > 0)
                {
                    FLatestAvailablePatch = testPatchVersion;
                }
            }

            // drop unnecessary patch files
            // ie. patch files leading to the same version, eg. 2.2.11-1 and 2.2.12-2 to 2.2.12-3
            // we only want the biggest step
            testPatchVersion = new TFileVersionInfo(FCurrentlyInstalledVersion);
            bool patchesAvailable = true;

            while (patchesAvailable)
            {
                StringCollection applyingPatches = new StringCollection();

                foreach (string patch in AOrderedListOfAllPatches.GetValueList())
                {
                    if (TPatchFileVersionInfo.PatchApplies(testPatchVersion, patch))
                    {
                        applyingPatches.Add(patch);
                    }
                }

                patchesAvailable = (applyingPatches.Count > 0);

                if (applyingPatches.Count > 0)
                {
                    // see which of the applying patches takes us further
                    string highestPatch = applyingPatches[0];
                    TFileVersionInfo highestPatchVersion = TPatchFileVersionInfo.GetLatestPatchVersionFromDiffZipName(highestPatch);

                    foreach (string patch in applyingPatches)
                    {
                        if (TPatchFileVersionInfo.GetLatestPatchVersionFromDiffZipName(patch).Compare(highestPatchVersion) > 0)
                        {
                            highestPatch = patch;
                            highestPatchVersion = TPatchFileVersionInfo.GetLatestPatchVersionFromDiffZipName(highestPatch);
                        }
                    }

                    ResultPatchList.Add(highestPatch, highestPatch);
                    testPatchVersion = highestPatchVersion;
                }
            }

            if (FLatestAvailablePatch.Compare(testPatchVersion) != 0)
            {
                TLogging.Log("missing patchfile from version " + testPatchVersion.ToString() + " to " + FLatestAvailablePatch.ToString());
                return new SortedList();
            }

            return ResultPatchList;
        }

        /// <summary>
        /// the patch of the current installed application
        /// </summary>
        /// <returns></returns>
        public string GetCurrentPatchVersion()
        {
            return FCurrentlyInstalledVersion.ToString();
        }

        /// <summary>
        /// the latest available patch version
        /// </summary>
        /// <returns></returns>
        public string GetLatestPatchVersion()
        {
            return FLatestAvailablePatch.ToString();
        }

        /// <summary>
        /// this procedure makes sure that the latest version of the patch tool is being used;
        /// the latest available executable and required dlls are copied to the patch tmp directory
        /// </summary>
        /// <returns>void</returns>
        public void CopyLatestPatchProgram(String APatchDirectory)
        {
            String remotename;
            ArrayList PatchExecutableFiles;

            PatchExecutableFiles = new ArrayList();
            string binPath = "openpetraorg" + Path.DirectorySeparatorChar + "bin" + FVersionPostFix + Path.DirectorySeparatorChar;
            PatchExecutableFiles.Add(binPath + "Ict.Common.dll");
            PatchExecutableFiles.Add(binPath + "Ict.Common.IO.dll");
            PatchExecutableFiles.Add(binPath + "ICSharpCode.SharpZipLib.dll");
            PatchExecutableFiles.Add(binPath + "Ict.Tools.PatchTool.exe");
            PatchExecutableFiles.Add(binPath + "Ict.Tools.PatchTool.Library.dll");
            PatchExecutableFiles.Add(binPath + "GNU.Gettext.dll");

            // copy the PatchTool.exe and required files from the currently installed application to a temp directory
            foreach (string patchExeFile in PatchExecutableFiles)
            {
                if (File.Exists(FBinPath + Path.DirectorySeparatorChar + Path.GetFileName(patchExeFile)))
                {
                    System.IO.File.Copy(FBinPath + Path.DirectorySeparatorChar + Path.GetFileName(patchExeFile),
                        APatchDirectory + Path.DirectorySeparatorChar + Path.GetFileName(patchExeFile), true);
                }
            }

            // compiling on Linux with Mono, we cannot use the manifest that tells the UAC that running a exe with patch in the name is fine without administrator rights
            // therefore we rename the file so that the normal user can execute it
            string newNameUAC = APatchDirectory + Path.DirectorySeparatorChar + "Ict.Tools.Ptchtool.exe";
            if (File.Exists(newNameUAC))
            {
               File.Delete(newNameUAC);
            }

            File.Move(APatchDirectory + Path.DirectorySeparatorChar + "Ict.Tools.PatchTool.exe", newNameUAC);

            // check for the latest version of those files in the new patches
            foreach (string patch in FListOfNewPatches.GetValueList())
            {
                // download the patch file from the remote server if it is not available locally yet
                // todo: display progress? could do it via the remoting connection?
                if (FRemotePatchesPath.StartsWith("http://") || FRemotePatchesPath.StartsWith("https://"))
                {
                    remotename = FRemotePatchesPath + "/" + Path.GetFileName(patch);
                    THTTPUtils.DownloadFile(remotename, patch);
                }
                else if (!System.IO.File.Exists(patch) && (FRemotePatchesPath.Length > 0))
                {
                    remotename = FRemotePatchesPath + Path.DirectorySeparatorChar + Path.GetFileName(patch);
                    System.IO.File.Copy(remotename, patch);
                }

                PackTools.Unzip(APatchDirectory, patch, PatchExecutableFiles, true);
            }
        }
    }
}