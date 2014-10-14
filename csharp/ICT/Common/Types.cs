//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Data;
using System.Data.Odbc;
using System.Runtime.Serialization;
using Ict.Common;

namespace Ict.Common
{
    /// <summary>
    /// enum for several runtime environments
    /// </summary>
    public enum TExecutingCLREnum
    {
        /// <summary>
        /// unknown
        /// </summary>
        eclrUnknown,

        /// <summary>
        /// Microsoft .Net
        /// </summary>
        eclrMicrosoftDotNetFramework,

        /// <summary>
        /// Mono
        /// </summary>
        eclrMono,

        /// <summary>
        /// DotGnu (not really supported)
        /// </summary>
        eclrDotGNUPortableNet
    };

    /// <summary>
    /// enum for handling date values that are null
    /// </summary>
    public enum TNullHandlingEnum
    {
        /// <summary>
        /// lowest possible date
        /// </summary>
        nhReturnLowestDate,

        /// <summary>
        /// highest possible date
        /// </summary>
        nhReturnHighestDate
    };

    /// <summary>
    /// enum for the Operating System that this program is running on
    /// </summary>
    public enum TExecutingOSEnum
    {
        /// <summary>
        /// Linux
        /// </summary>
        eosLinux,

        /// <summary>
        /// Win98 up to Windows Millenium (not really supported)
        /// </summary>
        eosWin98ToWinME,

        /// <summary>
        /// Windows NT and later
        /// </summary>
        eosWinNTOrLater,

        /// WinXP
        eosWinXP,

        /// WinVista
        eosWinVista,

        /// Win7
        eosWin7,

        /// Win8
        eosWin8,

        /// <summary>
        /// unknown and unsupported
        /// </summary>
        oesUnsupportedPlatform
    };

    /// <summary>
    /// Type of RDBMS (Relational Database Management System)
    /// </summary>
    public enum TDBType
    {
        /// <summary>The PostgreSQL RDBMS</summary>
        PostgreSQL,

        /// <summary>The MySQL RDBMS</summary>
        MySQL,

        /// <summary>Using ODBC to access legacy Progress databases</summary>
        ProgressODBC,

        /// <summary>For standalone and demo system, SQLite</summary>
        SQLite
    }

    /// <summary>
    /// several modes a data edit screen can be in
    /// </summary>
    public enum TDataModeEnum
    {
        /// <summary>
        /// just browsing the data, viewing, read only
        /// </summary>
        dmBrowse,

        /// <summary>
        /// edit the data
        /// </summary>
            dmEdit,

        /// <summary>
        /// add new data
        /// </summary>
            dmAdd
    };

    /// <summary>
    /// enum for the connection between client and server
    /// </summary>
    public enum TClientServerConnectionType
    {
        /// <summary>
        /// inside a LAN network (quite fast)
        /// </summary>
        csctLAN,

        /// <summary>
        /// remote through VPN, can be slow
        /// </summary>
        csctRemote,

        /// <summary>
        /// standalones run the client and the server on one machine
        /// </summary>
        csctLocal
    };

    /// <summary>
    /// enum for states in the asynchronous execution
    /// </summary>
    public enum TAsyncExecProgressState
    {
        /// <summary>
        /// read to start
        /// </summary>
        Aeps_ReadyToStart,

        /// <summary>
        /// currently running
        /// </summary>
        Aeps_Executing,

        /// <summary>
        /// finished
        /// </summary>
        Aeps_Finished,

        /// <summary>
        /// is stopping at the moment
        /// </summary>
        Aeps_Stopping,

        /// <summary>
        /// stopped
        /// </summary>
        Aeps_Stopped
    };

    /// <summary>
    /// different states when submitting some data to the server
    /// </summary>
    public enum TSubmitChangesResult
    {
        /// <summary>
        /// submission was ok, data has been saved to database
        /// </summary>
        scrOK,

        /// <summary>
        /// there has been an error during submission
        /// </summary>
        scrError,

        /// <summary>
        /// there is no new data, therefore nothing needs to be written to the database
        /// </summary>
        scrNothingToBeSaved,

        /// <summary>
        /// more info (user interaction) needed, before saving of data is possible
        /// </summary>
        scrInfoNeeded
    };

    /// <summary>
    /// search criteria for SQL queries
    /// </summary>
    [Serializable()]
    public class TSearchCriteria
    {
        /// <summary>
        /// how to compare; defaults to equals
        /// </summary>
        public string comparator = "=";

        /// <summary>
        /// field to compare
        /// </summary>
        public string fieldname;

        /// <summary>
        /// which value to search for
        /// </summary>
        public Object searchvalue;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AFieldName"></param>
        /// <param name="ASearchValue"></param>
        public TSearchCriteria(string AFieldName, Object ASearchValue)
        {
            fieldname = AFieldName;
            searchvalue = ASearchValue;
        }
    }

    /// <summary>
    /// Class for HyperLink handling. Used by the 'TtxtLinkTextBox' and 'TRtbHyperlinks' Controls.
    /// </summary>
    public static class THyperLinkHandling
    {
        /// <summary>Prefix for an Email Link.</summary>
        public const string HYPERLINK_PREFIX_EMAILLINK = "||email||";
        
        /// <summary>Prefix for a HTTP Link.</summary>
        public const string HYPERLINK_PREFIX_URLLINK = "||hyperlink||";
        
        /// <summary>Prefix for a HTTP Link where a part of that link gets constructed by supplying a value.</summary>
        public const string HYPERLINK_PREFIX_URLWITHVALUELINK = "||hyperlink_with_value||";
        
        /// <summary>Prefix for a HTTPS Link.</summary>
        public const string HYPERLINK_PREFIX_SECUREDURL = "||securehyperlink||";
        
        /// <summary>Prefix for a FTP Link.</summary>
        public const string HYPERLINK_PREFIX_FTPLINK = "||FTP||";
        
        /// <summary>Prefix for a Skype Link.</summary>
        public const string HYPERLINK_PREFIX_SKYPELINK = "||skype||";
        
        /// <summary>Identifier for a Value that will get replaced in a Link that is prefixed with 
        /// <see cref="HYPERLINK_PREFIX_URLWITHVALUELINK"/>.</summary>
        public const string HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER = "{VALUE}";
        
        #region THyperLinkType Enum

        /// <summary>
        /// Types of Hyperlinks that TtxtLinkTextBox supports.
        /// </summary>
        public enum THyperLinkType
        {
            /// <summary>
            /// Act as a regular TextBox
            /// </summary>
            None,
    
            /// <summary>
            /// Act as a http:// or https:// hyperlink
            /// </summary>
            Http,
    
            /// <summary>
            /// Act as a http:// or https:// hyperlink where a part of the URL is replaced with a custom value.
            /// </summary>
            Http_With_Value_Replacement,
            
            /// <summary>
            /// Act as a ftp:// hyperlink
            /// </summary>
            Ftp,
    
            /// <summary>
            /// Act as a mailto: hyperlink
            /// </summary>
            Email,
            
            /// <summary>
            /// Get the Skype.exe application to start a call to the supplied Skype ID
            /// </summary>
            Skype
        }
    
        #endregion
        
        /// <summary>
        /// Parses a string to a <see cref="THyperLinkType" />.
        /// </summary>
        /// <param name="AHyperLinkType">String that should get parsed into a <see cref="THyperLinkType" />.</param>
        /// <returns></returns>
        public static THyperLinkType ParseHyperLinkType(string AHyperLinkType)
        {
            THyperLinkType ReturnValue;

            ReturnValue = THyperLinkType.None;

            if (String.Equals(AHyperLinkType, String.Empty))
            {
                ReturnValue = THyperLinkType.None;
            }
            else if (String.Equals(AHyperLinkType, HYPERLINK_PREFIX_EMAILLINK))
            {
                ReturnValue = THyperLinkType.Email;
            }
            else if (String.Equals(AHyperLinkType, HYPERLINK_PREFIX_URLLINK))
            {
                ReturnValue = THyperLinkType.Http;
            }
            else if (String.Equals(AHyperLinkType, HYPERLINK_PREFIX_URLWITHVALUELINK))
            {
                ReturnValue = THyperLinkType.Http_With_Value_Replacement;
            }
            else if (String.Equals(AHyperLinkType, HYPERLINK_PREFIX_SECUREDURL))
            {
                ReturnValue = THyperLinkType.Http;
            }
            else if (String.Equals(AHyperLinkType, HYPERLINK_PREFIX_FTPLINK))
            {
                ReturnValue = THyperLinkType.Ftp;
            }
            else if (String.Equals(AHyperLinkType, HYPERLINK_PREFIX_SKYPELINK))
            {
                ReturnValue = THyperLinkType.Skype;
            }

            return ReturnValue;
        }        
    }
    
    /// <summary>
    /// some functions that are useful for operating with the enums defined in Ict.Common
    /// </summary>
    public class CommonTypes
    {
        /// <summary>
        /// convert the string to the enum for the RDBMS System
        /// </summary>
        /// <param name="ADBType">defines the chosen database system</param>
        /// <returns>enum value</returns>
        public static TDBType ParseDBType(String ADBType)
        {
            if (ADBType.ToLower() == "postgresql")
            {
                return TDBType.PostgreSQL;
            }

            if (ADBType.ToLower() == "mysql")
            {
                return TDBType.MySQL;
            }

            if (ADBType.ToLower() == "sqlite")
            {
                return TDBType.SQLite;
            }

            if (ADBType.ToLower() == "progress")
            {
                return TDBType.ProgressODBC;
            }

            throw new Exception(Catalog.GetString("invalid database system"));
        }
        
        /// <summary>
        /// convert the enum to string for the Operating System
        /// </summary>
        /// <param name="AExecutingOS">defines the operating system</param>
        /// <returns>string representing the operating system</returns>
        public static String ExecutingOSEnumToString(TExecutingOSEnum AExecutingOS)
        {
            return ExecutingOSEnumToString(AExecutingOS, false);
        }

        /// <summary>
        /// convert the enum to string for the Operating System
        /// </summary>
        /// <param name="AExecutingOS">defines the operating system</param>
        /// <param name="ALongDescription">we want a long description of the OS</param>
        /// <returns>string representing the operating system</returns>
        public static String ExecutingOSEnumToString(TExecutingOSEnum AExecutingOS, Boolean ALongDescription)
        {
            String ReturnValue;

            switch (AExecutingOS)
            {
                case TExecutingOSEnum.eosLinux:
                    ReturnValue = "Linux";
                    break;

                case TExecutingOSEnum.eosWin98ToWinME:

                    if (ALongDescription)
                    {
                        ReturnValue = "Windows98 to WindowsME";
                    }
                    else
                    {
                        ReturnValue = "Windows98/ME";
                    }

                    break;

                case TExecutingOSEnum.eosWinXP:
                    return "Windows XP / Server 2003";

                case TExecutingOSEnum.eosWinVista:
                    return "Windows Vista";

                case TExecutingOSEnum.eosWin7:
                    return "Windows 7 / Server 2008";

                case TExecutingOSEnum.eosWinNTOrLater:

                    if (ALongDescription)
                    {
                        ReturnValue = "WindowsNT or later";
                    }
                    else
                    {
                        ReturnValue = "WindowsNT/XP/Win7/2008";
                    }

                    break;

                case TExecutingOSEnum.oesUnsupportedPlatform:

                    if (ALongDescription)
                    {
                        ReturnValue = "#UNSUPPORTED PLATFORM!#";
                    }
                    else
                    {
                        ReturnValue = "#UNSUPPORTED!#";
                    }

                    break;

                default:
                    ReturnValue = null;
                    break;
            }

            return ReturnValue;
        }
    }


    /// <summary>
    /// Thrown when OpenPetra encounters a problem when trying to launch a Hyperlink.
    /// </summary>
    public class EProblemLaunchingHyperlinkException : Exception
    {
        /// <summary>
        /// Constructor with inner Exception
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="message"></param>
        public EProblemLaunchingHyperlinkException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Constructor without inner Exception
        /// </summary>
        /// <param name="message"></param>
        public EProblemLaunchingHyperlinkException(string message)
            : base(message)
        {
        }
    }
    
    /// <summary>
    /// some static methods for the save conversion of dates to objects and objects to dates
    /// </summary>
    public class TSaveConvert : object
    {
        #region TSaveConvert

        /// <summary>
        /// Converts a date value that is stored in a TObject to a DateTime value that is
        /// guaranteed to be valid.
        ///
        /// In case the date value in the TObject is empty, the lowest possible date
        /// is returned.
        ///
        /// @comment Very useful for untyped data in DataSets that is known to be of
        /// DateTime type.
        ///
        /// </summary>
        /// <param name="ADateObject">TObject containing a date value</param>
        /// <returns>A valid DateTime
        /// </returns>
        public static DateTime ObjectToDate(object ADateObject)
        {
            return ObjectToDate(ADateObject, TNullHandlingEnum.nhReturnLowestDate);
        }

        /// <summary>
        /// Converts a date value that is stored in a TObject to a DateTime value that is
        /// guaranteed to be valid.
        ///
        /// In case the date value in the TObject is empty, either the lowest or the
        /// highest possible date is returned.
        ///
        /// @comment Very useful for untyped data in DataSets that is known to be of
        /// DateTime type.
        ///
        /// </summary>
        /// <param name="ADateObject">TObject containing a date value</param>
        /// <param name="ANullHandling">Switch to return either the lowest (nhReturnLowestDate)
        /// or the highest (nhReturnHighestDate) possible date in case the date value
        /// in the TObject is empty</param>
        /// <returns>A valid DateTime
        /// </returns>
        public static DateTime ObjectToDate(object ADateObject, TNullHandlingEnum ANullHandling)
        {
            DateTime ReturnValue;

            if (ADateObject != null)
            {
                if (!(ADateObject.ToString() == ""))
                {
                    ReturnValue = Convert.ToDateTime(ADateObject);
                }
                else
                {
                    if (ANullHandling == TNullHandlingEnum.nhReturnLowestDate)
                    {
                        ReturnValue = DateTime.MinValue;
                    }
                    else
                    {
                        ReturnValue = DateTime.MaxValue;
                    }
                }
            }
            else
            {
                if (ANullHandling == TNullHandlingEnum.nhReturnLowestDate)
                {
                    ReturnValue = DateTime.MinValue;
                }
                else
                {
                    ReturnValue = DateTime.MaxValue;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Converts a DataColumn that holds a DateTime into a DateTime that is
        /// guaranteed to be valid.
        ///
        /// In case the date value in the DataColumn is DBNull, the lowest possible date
        /// is returned.
        ///
        /// @comment Very useful for DataColumns in Typed DataTables that are of DateTime
        /// type. Using this function, no Exception is thrown when trying to get the
        /// value of a DataColumn of Type DateTime that is DBNull.
        ///
        /// </summary>
        /// <param name="ADataColumn">DataColumn of Type DateTime</param>
        /// <param name="ADataRow">DataRow in which the value is found</param>
        /// <returns>A valid DateTime
        /// </returns>
        public static DateTime DateColumnToDate(DataColumn ADataColumn, DataRow ADataRow)
        {
            return DateColumnToDate(ADataColumn, ADataRow, TNullHandlingEnum.nhReturnLowestDate);
        }

        /// <summary>
        /// Converts a DataColumn that holds a DateTime into a DateTime that is
        /// guaranteed to be valid.
        ///
        /// In case the date value in the DataColumn is DBNull, the lowest possible date
        /// is returned.
        ///
        /// @comment Very useful for DataColumns in Typed DataTables that are of DateTime
        /// type. Using this function, no Exception is thrown when trying to get the
        /// value of a DataColumn of Type DateTime that is DBNull.
        ///
        /// </summary>
        /// <param name="ADataColumn">DataColumn of Type DateTime</param>
        /// <param name="ADataRow">DataRow in which the value is found</param>
        /// <param name="ANullHandling">Switch to return either the lowest (nhReturnLowestDate)
        /// or the highest (nhReturnHighestDate) possible date in case the date value
        /// in the TObject is empty</param>
        /// <returns>A valid DateTime
        /// </returns>
        public static DateTime DateColumnToDate(DataColumn ADataColumn, DataRow ADataRow, TNullHandlingEnum ANullHandling)
        {
            DateTime ReturnValue;

            if (ADataRow.IsNull(ADataColumn))
            {
                if (ANullHandling == TNullHandlingEnum.nhReturnLowestDate)
                {
                    ReturnValue = DateTime.MinValue;
                }
                else
                {
                    ReturnValue = DateTime.MaxValue;
                }

                // MessageBox.Show('Column is DBNull!');
            }
            else
            {
                // MessageBox.Show('Column is not DBNull!');
                ReturnValue = (DateTime)ADataRow[ADataColumn];
            }

            return ReturnValue;
        }

        /// <summary>
        /// Converts a DataColumn that holds a String into a String that is guaranteed
        /// to be valid.
        ///
        /// In case the String value in the DataColumn is DBNull, an empty String ('')
        /// is returned.
        ///
        /// @comment Very useful for DataColumns in Typed DataTables that are of String
        /// type. Using this function, no Exception is thrown when trying to get the
        /// value of a DataColumn of Type String that is DBNull.
        ///
        /// </summary>
        /// <param name="ADataColumn">DataColumn of Type DateTime</param>
        /// <param name="ADataRow">DataRow in which the value is found</param>
        /// <returns>A valid String
        /// </returns>
        public static String StringColumnToString(DataColumn ADataColumn, DataRow ADataRow)
        {
            String ReturnValue;

            if (ADataRow.IsNull(ADataColumn))
            {
                ReturnValue = "";
            }
            else
            {
                ReturnValue = (String)ADataRow[ADataColumn];
            }

            return ReturnValue;
        }

        #endregion
    }

    /// <summary>
    /// simple attribute for marking methods that should not be remoted.
    /// the code generator for generateGlue will take note of this attribute.
    /// </summary>
    public class NoRemotingAttribute : System.Attribute
    {
    }

    /// <summary>
    ///  this class contains some attribute classes (at the moment only one)
    /// </summary>
    public class Attributes
    {
        /// <summary>
        /// This custom .NET Attribute is used to mark functions that return resourcestrings.
        /// These Attributes can later be found and the functions can be changed to return
        /// resourcestrings from any source/repository instead of having them hardcoded in code.
        /// </summary>
        [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
        public class ResourceStringAttribute : System.Attribute
        {
            //Private fields.
            private string FResourceStringName;
            private string FResourceNamespace;
            private bool FTranslated;

            /// <summary>
            /// This constructor defines two required parameters.
            /// </summary>
            /// <param name="AResourceStringName"></param>
            /// <param name="AResourceNamespace"></param>
            public ResourceStringAttribute(string AResourceStringName, string AResourceNamespace)
            {
                this.FResourceStringName = AResourceStringName;
                this.FResourceNamespace = AResourceNamespace;
                this.FTranslated = false;
            }

            /// <summary>
            /// Define Name property.
            /// This is a read-only attribute.
            /// </summary>
            public virtual string ResourceStringName
            {
                get
                {
                    return FResourceStringName;
                }
            }

            /// <summary>
            /// Define ResourceNamespace property.
            /// This is a read-only attribute.
            /// </summary>
            public virtual string ResourceNamespace
            {
                get
                {
                    return FResourceNamespace;
                }
            }

            /// <summary>
            /// Define Translated property.
            /// This is a read/write attribute.
            /// </summary>
            public virtual bool Translated
            {
                get
                {
                    return FTranslated;
                }

                set
                {
                    FTranslated = value;
                }
            }
        }
    }
}