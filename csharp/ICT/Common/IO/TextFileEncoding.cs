//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanp
//
// Copyright 2004-2016 by OM International
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
using System.Text;
using System.Windows.Forms;

namespace Ict.Common.IO
{
    /// <summary>
    /// A class to handle the various character encoding supported by Open Petra
    /// </summary>
    public class TTextFileEncoding : DataTable
    {
        /// <summary> User Preference Key name for allowing MBCS Code pages such as Chinese, Japanese and Korean.  (Applies to Petra imports) </summary>
        public const string ALLOW_MBCS_TEXT_ENCODING = "Allow_MBCS_Text_Encoding";

        /// <summary> Gets the column name for the Code </summary>
        public static string ColumnCodeDbName
        {
            get; private set;
        }
        /// <summary> Gets the column name for the Description </summary>
        public static string ColumnDescriptionDbName
        {
            get; private set;
        }
        /// <summary> Gets the column name for the Class </summary>
        public static string ColumnClassDbName
        {
            get; private set;
        }
        /// <summary> Gets the column ordinal for the Code in an instance of TTextfileEncoding </summary>
        public int ColumnCodeId
        {
            get
            {
                return this.Columns[ColumnCodeDbName].Ordinal;
            }
        }
        /// <summary> Gets the column ordinal for the Description in an instance of TTextfileEncoding </summary>
        public int ColumnDescriptionId
        {
            get
            {
                return this.Columns[ColumnDescriptionDbName].Ordinal;
            }
        }
        /// <summary> Gets the column ordinal for the Class in an instance of TTextfileEncoding </summary>
        public int ColumnClassId
        {
            get
            {
                return this.Columns[ColumnClassDbName].Ordinal;
            }
        }

        // Constants for column names and column content
        private const string COLUMN_CODE = "code";
        private const string COLUMN_DESCRIPTION = "description";
        private const string COLUMN_CLASS = "class";
        private const string CLASS_UNICODE = "Unicode";
        private const string CLASS_ANSI_SINGLE_BYTE = "ANSI_SB";
        private const string CLASS_ANSI_MULTI_BYTE = "ANSI_MB";

        /// <summary>
        /// Main constructor
        /// </summary>
        public TTextFileEncoding()
        {
            CreateDataTable();
        }

        // Method to create the table and fill it with data
        private void CreateDataTable()
        {
            ColumnCodeDbName = COLUMN_CODE;
            ColumnDescriptionDbName = COLUMN_DESCRIPTION;
            ColumnClassDbName = COLUMN_CLASS;

            DataColumn col = new DataColumn(ColumnCodeDbName, typeof(System.String));
            this.Columns.Add(col);

            col = new DataColumn(ColumnDescriptionDbName, typeof(System.String));
            this.Columns.Add(col);

            col = new DataColumn(ColumnClassDbName, typeof(System.String));
            this.Columns.Add(col);

            // These are the code pages that we support for our imports.
            // They include the Unicode ones and the ANSI ones supported by Petra
            // We always support the user's own code page even if not included in this list
            CreateRow(1200, CLASS_UNICODE);                         // UTF-16
            CreateRow(1201, CLASS_UNICODE);                         // Unicode FFFE
            CreateRow(12000, CLASS_UNICODE);                        // UTF 32
            CreateRow(12001, CLASS_UNICODE);                        // UTF 32BE
            CreateRow(65000, CLASS_UNICODE);                        // UTF-7
            CreateRow(65001, CLASS_UNICODE);                        // UTF-8
            CreateRow(1250, CLASS_ANSI_SINGLE_BYTE);
            CreateRow(1251, CLASS_ANSI_SINGLE_BYTE);
            CreateRow(1252, CLASS_ANSI_SINGLE_BYTE);
            CreateRow(1253, CLASS_ANSI_SINGLE_BYTE);
            CreateRow(1254, CLASS_ANSI_SINGLE_BYTE);
            CreateRow(1257, CLASS_ANSI_SINGLE_BYTE);
            CreateRow(949, CLASS_ANSI_MULTI_BYTE);                  // Korean
            CreateRow(932, CLASS_ANSI_MULTI_BYTE);                  // Japanese
            CreateRow(950, CLASS_ANSI_MULTI_BYTE);                  // Chinese

            AddUserCodePageRow();
        }

        /// <summary> Adds a row to the table containing the data supplied.  The number of items in the array must match the number of columns </summary>
        private void CreateRow(int ACodePage, string AClass)
        {
            DataRow row = this.NewRow();

            row[0] = ACodePage.ToString();
            row[1] = Encoding.GetEncoding(ACodePage).EncodingName;
            row[2] = AClass;
            this.Rows.Add(row);
        }

        private void AddUserCodePageRow()
        {
            Encoding userEncoding = Encoding.Default;
            int userCodePage = userEncoding.CodePage;
            bool gotAlready = false;

            for (int i = 0; i < this.Rows.Count; i++)
            {
                DataRow row = this.Rows[i];

                if (Convert.ToInt32(row[ColumnCodeDbName]) == userCodePage)
                {
                    gotAlready = true;
                    break;
                }
            }

            if (gotAlready == false)
            {
                CreateRow(userCodePage, userEncoding.IsSingleByte ? CLASS_ANSI_SINGLE_BYTE : CLASS_ANSI_MULTI_BYTE);
            }
        }

        /// <summary>
        /// This method sets the content of a ComboBox that was populated from a TTextFileEncoding data table.
        /// </summary>
        /// <param name="AComboBox">The ComboBox whose DataSource is a TTextFileEncoding data table</param>
        /// <param name="AHasBOM">True if the file has a BOM</param>
        /// <param name="AIsAmbiguousUTF">True if the encoding is ambiguous</param>
        /// <param name="AEncoding">The initial encoding</param>
        public static void SetComboBoxProperties(ComboBox AComboBox, bool AHasBOM, bool AIsAmbiguousUTF, Encoding AEncoding)
        {
            DataView dv = (DataView)AComboBox.DataSource;

            string rowFilter = string.Empty;
            bool allowMBCS = false;                 // User can change this with a user setting (below)
            bool needsWindows = true;

            if (FRetrieveUserDefaultBoolean != null)
            {
                allowMBCS = FRetrieveUserDefaultBoolean(TTextFileEncoding.ALLOW_MBCS_TEXT_ENCODING, false);
            }

            if (IsUnicodeCodePage(AEncoding.CodePage))
            {
                // Deal with filtering the UTF ones
                rowFilter += string.Format("({0}={1}) ", TTextFileEncoding.ColumnCodeDbName, AEncoding.CodePage);

                if ((allowMBCS == true) && (AHasBOM == false))
                {
                    // With no BOM a UTF code page can always be a MBCS ANSI one
                    rowFilter += string.Format(" OR ({0}='{1}')", TTextFileEncoding.ColumnClassDbName, CLASS_ANSI_MULTI_BYTE);
                }

                if (AIsAmbiguousUTF)
                {
                    // Although it is 'probably' Unicode there were not enough examples to be statistically certain
                    rowFilter += "OR ";
                }
                else
                {
                    // This is NOT ambiguous so we won't need the Windows code pages
                    needsWindows = false;
                }
            }

            if (needsWindows)
            {
                rowFilter += string.Format("({0}='{1}'{2})",
                    TTextFileEncoding.ColumnClassDbName,
                    TTextFileEncoding.CLASS_ANSI_SINGLE_BYTE,
                    allowMBCS ?
                    string.Format(" OR {0}='{1}'", TTextFileEncoding.ColumnClassDbName, TTextFileEncoding.CLASS_ANSI_MULTI_BYTE) : "");
            }

            dv.RowFilter = rowFilter;

            AComboBox.Enabled = dv.Count > 1;
        }

        /// <summary>
        /// Returns true if the code page is not a supported Unicode page so is assumed to be one of our supported Windows ANSI code pages
        /// </summary>
        public static bool IsWindowsANSICodePage(int ACodePage)
        {
            return ACodePage != 1200 && ACodePage != 1201 && ACodePage != 12000 && ACodePage != 12001 && ACodePage != 65000 && ACodePage != 65001;
        }

        /// <summary>
        /// Returns true if the code page is one of our supported Unicode pages
        /// </summary>
        public static bool IsUnicodeCodePage(int ACodePage)
        {
            return ACodePage == 1200 || ACodePage == 1201 || ACodePage == 12000 || ACodePage == 12001 || ACodePage == 65000 || ACodePage == 65001;
        }

        #region Delegate for obtaining a user default boolean value

        /// <summary>
        /// Declaration of a delegate to retrieve a boolean value from the user defaults
        /// </summary>
        public delegate Boolean TRetrieveUserDefaultBoolean(String AKey, Boolean ADefault);

        // Delegate for accessing boolean user preference
        private static TRetrieveUserDefaultBoolean FRetrieveUserDefaultBoolean;

        /// <summary>
        /// Get/set the function pointer for retrieving a boolean user default
        /// </summary>
        public static TRetrieveUserDefaultBoolean RetrieveUserDefaultBoolean
        {
            get
            {
                return FRetrieveUserDefaultBoolean;
            }

            set
            {
                FRetrieveUserDefaultBoolean = value;
            }
        }

        #endregion
    }
}