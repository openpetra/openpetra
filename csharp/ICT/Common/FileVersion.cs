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
using System.Diagnostics;

namespace Ict.Common
{
    /// <summary>
    /// our own type for version information for a file
    /// </summary>
    [Serializable()]
    public class TFileVersionInfo
    {
        /// <summary>MajorPart.MinorPart.BuildPart-PrivatePart</summary>
        public UInt16 FileMajorPart;

        /// <summary>MajorPart.MinorPart.BuildPart-PrivatePart</summary>
        public UInt16 FileMinorPart;

        /// <summary>MajorPart.MinorPart.BuildPart-PrivatePart</summary>
        public UInt16 FileBuildPart;

        /// <summary>MajorPart.MinorPart.BuildPart-PrivatePart</summary>
        public UInt16 FilePrivatePart;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ARPMStyleVersion">2.2.35: major.minor.buildprivate</param>
        /// <returns>void</returns>
        public TFileVersionInfo(String ARPMStyleVersion)
        {
            // 2.2.35-99: major.minor.build-private
            // also works for 2.2.35.99
            String[] VersionParts = ARPMStyleVersion.Split(new char[] { '.', '-' });

            FileMajorPart = 0;
            FileMinorPart = 0;
            FileBuildPart = 0;
            FilePrivatePart = 0;

            if (VersionParts.Length > 0)
            {
                FileMajorPart = System.Convert.ToUInt16(VersionParts[0]);
            }

            if (VersionParts.Length > 1)
            {
                FileMinorPart = System.Convert.ToUInt16(VersionParts[1]);
            }

            if (VersionParts.Length > 2)
            {
                FileBuildPart = System.Convert.ToUInt16(VersionParts[2]);
            }

            if (VersionParts.Length > 3)
            {
                FilePrivatePart = System.Convert.ToUInt16(VersionParts[3]);
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AInfo"></param>
        public TFileVersionInfo(FileVersionInfo AInfo)
        {
            FileMajorPart = System.Convert.ToUInt16(AInfo.FileMajorPart);
            FileMinorPart = System.Convert.ToUInt16(AInfo.FileMinorPart);
            FileBuildPart = System.Convert.ToUInt16(AInfo.FileBuildPart);
            FilePrivatePart = System.Convert.ToUInt16(AInfo.FilePrivatePart);
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AVersion"></param>
        public TFileVersionInfo(System.Version AVersion)
        {
            FileMajorPart = System.Convert.ToUInt16(AVersion.Major);
            FileMinorPart = System.Convert.ToUInt16(AVersion.Minor);
            FileBuildPart = System.Convert.ToUInt16(AVersion.Build);
            FilePrivatePart = System.Convert.ToUInt16(AVersion.Revision);
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="AInfo"></param>
        public TFileVersionInfo(TFileVersionInfo AInfo)
        {
            FileMajorPart = AInfo.FileMajorPart;
            FileMinorPart = AInfo.FileMinorPart;
            FileBuildPart = AInfo.FileBuildPart;
            FilePrivatePart = AInfo.FilePrivatePart;
        }

        /// <summary>
        /// constructor
        /// </summary>
        public TFileVersionInfo()
        {
        }

        /// <summary>
        /// compare two file versions
        /// </summary>
        /// <returns>-1 if this &lt; ACmp, 1 if this &gt; ACmp, and 0 if equals</returns>
        public Int16 Compare(TFileVersionInfo ACmp)
        {
            Int16 ReturnValue;

            if (FileMajorPart > ACmp.FileMajorPart)
            {
                ReturnValue = 1;
            }
            else if (FileMajorPart < ACmp.FileMajorPart)
            {
                ReturnValue = -1;
            }
            else if (FileMinorPart > ACmp.FileMinorPart)
            {
                ReturnValue = 1;
            }
            else if (FileMinorPart < ACmp.FileMinorPart)
            {
                ReturnValue = -1;
            }
            else if (FileBuildPart > ACmp.FileBuildPart)
            {
                ReturnValue = 1;
            }
            else if (FileBuildPart < ACmp.FileBuildPart)
            {
                ReturnValue = -1;
            }
            else if (FilePrivatePart > ACmp.FilePrivatePart)
            {
                ReturnValue = 1;
            }
            else if (FilePrivatePart < ACmp.FilePrivatePart)
            {
                ReturnValue = -1;
            }
            else
            {
                ReturnValue = 0;
            }

            return ReturnValue;
        }

        /// <summary>
        /// compare two file versions, while ignoring the private part
        /// </summary>
        /// <returns>-1 if this &lt; ACmp, 1 if this &gt; ACmp, and 0 if equals</returns>
        public Int16 CompareWithoutPrivatePart(TFileVersionInfo ACmp)
        {
            Int16 ReturnValue;

            if (FileMajorPart > ACmp.FileMajorPart)
            {
                ReturnValue = 1;
            }
            else if (FileMajorPart < ACmp.FileMajorPart)
            {
                ReturnValue = -1;
            }
            else if (FileMinorPart > ACmp.FileMinorPart)
            {
                ReturnValue = 1;
            }
            else if (FileMinorPart < ACmp.FileMinorPart)
            {
                ReturnValue = -1;
            }
            else if (FileBuildPart > ACmp.FileBuildPart)
            {
                ReturnValue = 1;
            }
            else if (FileBuildPart < ACmp.FileBuildPart)
            {
                ReturnValue = -1;
            }
            else
            {
                ReturnValue = 0;
            }

            return ReturnValue;
        }

        /// <summary>
        /// print file version to string
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return FileMajorPart.ToString() + '.' + FileMinorPart.ToString() + '.' + FileBuildPart.ToString() + '.' + FilePrivatePart.ToString();
        }

        /// <summary>
        /// print file version to string, with hyphen as last separator
        /// </summary>
        /// <returns></returns>
        public String ToStringDotsHyphen()
        {
            return FileMajorPart.ToString() + '.' + FileMinorPart.ToString() + '.' + FileBuildPart.ToString() + '-' + FilePrivatePart.ToString();
        }

        /// <summary>
        /// get the version of the current application.
        /// Parse version.txt in the same directory if that file exists.
        /// Otherwise use the version of the exe or dll file
        /// </summary>
        /// <returns></returns>
        public static TFileVersionInfo GetApplicationVersion()
        {
            TFileVersionInfo Result = new TFileVersionInfo();

            // retrieve the current version of the server from the file version.txt in the bin directory
            // this is easier to manage than to check the assembly version in case you only need to quickly update the client
            string BinPath = TAppSettingsManager.ApplicationDirectory;

            if (File.Exists(BinPath + Path.DirectorySeparatorChar + "version.txt"))
            {
                StreamReader srVersion = new StreamReader(BinPath + Path.DirectorySeparatorChar + "version.txt");
                Result = new TFileVersionInfo(srVersion.ReadLine());
                srVersion.Close();
            }
            else if ((System.Reflection.Assembly.GetEntryAssembly() != null) && (System.Reflection.Assembly.GetEntryAssembly().GetName() != null))
            {
                Result = new TFileVersionInfo(System.Reflection.Assembly.GetEntryAssembly().GetName().Version);
            }
            else
            {
                // this is with the web services, started with xsp.exe, or running from NUnit
                Result = new TFileVersionInfo(new Version(0, 0, 0, 0));
            }

            return Result;
        }
    }
}