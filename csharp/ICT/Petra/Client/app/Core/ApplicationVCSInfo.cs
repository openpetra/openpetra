//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Windows.Forms;

using Ict.Common.Remoting.Client;

namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Stores Version Control System (VCS) Information about the
    /// VCS check-out that the build of the application is built against.
    /// </summary>
    public static class TApplicationVCSInfo
    {
        /// <summary>
        /// Used for storing information about the VCS check-out that the
        /// build of the application is built against.
        /// </summary>
        public struct ApplicationVCSData
        {
            bool FIsInitialised;
            string FVCSName;
            string FRevisionID;
            string FRevisionDate;
            string FRevisionCheckoutDate;
            string FRevisionNumber;

            /// <summary>
            /// Name of the Version Control System (VCS) - e.g. 'Bazaar'.
            /// </summary>
            public string VCSName
            {
                get
                {
                    return FVCSName;
                }

                set
                {
                    FVCSName = value;
                    FIsInitialised = true;
                }
            }

            /// <summary>
            /// Revision ID of the checkout from the VCS.
            /// </summary>
            /// <remarks><see cref="RevisionNumber"/> might be more telling... (it is
            /// in case of Bazaar)</remarks>
            public string RevisionID
            {
                get
                {
                    return FRevisionID;
                }

                set
                {
                    FRevisionID = value;
                    FIsInitialised = true;
                }
            }

            /// <summary>
            /// Date of the Revision according to the VCS.
            /// </summary>
            public string RevisionDate
            {
                get
                {
                    return FRevisionDate;
                }

                set
                {
                    FRevisionDate = value;
                    FIsInitialised = true;
                }
            }

            /// <summary>
            /// Date that the Revision was checked out locally from the VCS.
            /// </summary>
            public string RevisionCheckoutDate
            {
                get
                {
                    return FRevisionCheckoutDate;
                }

                set
                {
                    FRevisionCheckoutDate = value;
                    FIsInitialised = true;
                }
            }

            /// <summary>
            /// Revision Number of the checkout from the VCS.
            /// </summary>
            public string RevisionNumber
            {
                get
                {
                    return FRevisionNumber;
                }

                set
                {
                    FRevisionNumber = value;
                    FIsInitialised = true;
                }
            }

            /// <summary>
            /// True if this struct has been initialised with data, otherwise false. It will only be
            /// initialised if a VCS-data containing file was found at application startup -
            /// at that time the Method <see cref="DetermineApplicationVCSInfo"/> gets
            /// called for that purpose!
            /// </summary>
            public bool IsInitialised
            {
                get
                {
                    return FIsInitialised;
                }
            }
        }

        /// <summary>
        /// Public static instance of the single <see cref="ApplicationVCSData"/> struct that there ever is.
        /// </summary>
        /// <remarks>
        /// Inquire this instances' <see cref="ApplicationVCSData.IsInitialised"/> Property to find out if the
        /// struct has been populated with data, or not!
        /// </remarks>
        public static ApplicationVCSData AppVCSData;

        /// <summary>
        /// To be called at application startup time for the determination of the data that
        /// <see cref="AppVCSData"/> should contain. It will only contain data if a VCS-data-
        /// containing file was found!
        /// </summary>
        public static void DetermineApplicationVCSInfo()
        {
            const string VCS_REVISION_FILE_NAME = "vcs-revision.txt";

            string VCSInfoFilePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\" + VCS_REVISION_FILE_NAME;
            String TextLine;
            int LineCounter = 0;

            if (File.Exists(VCSInfoFilePath))
            {
                using (StreamReader sr = File.OpenText(VCSInfoFilePath))
                {
                    while ((TextLine = sr.ReadLine()) != null)
                    {
                        switch (LineCounter)
                        {
                            case 0:

                                // Check this is a file that holds output that the "bzr version-info" command generates
                                // This file is put into the 'bin' folder ONLY when an Installer is built!
                                if (!TextLine.StartsWith("revision-id:"))
                                {
                                    return;
                                }
                                else
                                {
                                    TApplicationVCSInfo.AppVCSData = new TApplicationVCSInfo.ApplicationVCSData();

                                    TApplicationVCSInfo.AppVCSData.VCSName = "Bazaar";
                                    TApplicationVCSInfo.AppVCSData.RevisionID = TextLine.Substring(TextLine.IndexOf(':') + 2);
                                }

                                break;

                            case 1:
                                TApplicationVCSInfo.AppVCSData.RevisionDate = TextLine.Substring(TextLine.IndexOf(':') + 2);

                                break;

                            case 2:
                                TApplicationVCSInfo.AppVCSData.RevisionCheckoutDate = TextLine.Substring(TextLine.IndexOf(':') + 2);

                                break;

                            case 3:
                                TApplicationVCSInfo.AppVCSData.RevisionNumber = TextLine.Substring(TextLine.IndexOf(':') + 2);

                                break;
                        }

                        LineCounter++;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Helper Class for the applications' Version
    /// </summary>
    public static class TApplicationVersion
    {
        /// <summary>
        /// Returns the application's Version
        /// </summary>
        /// <returns>The application's Version.</returns>
        public static string GetApplicationVersion()
        {
            string ReturnValue = String.Empty;
            
            var AssemblyVers = new Version(TClientInfo.ClientAssemblyVersion);

            if (AssemblyVers.Revision > 20)
            {
                ReturnValue += AssemblyVers.ToString(4);
            }
            else
            {
                // leave out 'Revision'
                ReturnValue += AssemblyVers.ToString(3);
            }

#if DEBUG
            if (TApplicationVCSInfo.AppVCSData.IsInitialised)
            {
                ReturnValue += "         [ " + TApplicationVCSInfo.AppVCSData.VCSName + " Rev. " +
                                    TApplicationVCSInfo.AppVCSData.RevisionNumber + " ]";
            }
#endif  
            return ReturnValue;
        }
    }
}