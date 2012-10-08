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
using System.Collections.Generic;
using System.Collections.Specialized;
using Ict.Common;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// this class helps with upgrading tables in AP
    /// </summary>
    public class TFinanceAccountsPayableUpgrader : TFixData
    {
        /// static table for ap document numbers, store to temp file in fulldump folder
        static SortedList <string, long>APDocumentNumberToId = null;

        /// <summary>
        /// load the ap numbers and ids from temp file
        /// </summary>
        public static void LoadAPDocumentNumberToId()
        {
            if (APDocumentNumberToId == null)
            {
                APDocumentNumberToId = new SortedList <string, long>();

                string filename = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "_APDocumentIDs.txt";

                if (File.Exists(filename))
                {
                    StreamReader sr = new StreamReader(filename);
                    string[] line;

                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine().Split(new char[] { ',' });

                        APDocumentNumberToId.Add(line[0], Convert.ToInt64(line[1]));
                    }

                    sr.Close();
                }
            }
        }

        /// <summary>
        /// store the ap numbers and ids
        /// </summary>
        public static void WriteAPDocumentNumberToId()
        {
            if (APDocumentNumberToId != null)
            {
                string filename = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "_APDocumentIDs.txt";

                StreamWriter sw = new StreamWriter(filename);

                foreach (string s in APDocumentNumberToId.Keys)
                {
                    sw.WriteLine(s + "," + APDocumentNumberToId[s].ToString());
                }

                sw.Close();
            }
        }

        /// <summary>
        /// fixing table a_ap_document
        /// </summary>
        public static bool FixAPDocument(StringCollection AColumnNames, ref string[] ANewRow)
        {
            TSequenceWriter.LoadSequences();
            LoadAPDocumentNumberToId();

            string LedgerNumberAndAPNumber =
                GetValue(AColumnNames, ANewRow, "a_ledger_number_i") + "_" +
                GetValue(AColumnNames, ANewRow, "a_ap_number_i");

            if (APDocumentNumberToId.ContainsKey(LedgerNumberAndAPNumber))
            {
                SetValue(AColumnNames, ref ANewRow, "a_ap_document_id_i", APDocumentNumberToId[LedgerNumberAndAPNumber].ToString());
            }
            else
            {
                long newValue = TSequenceWriter.GetNextSequenceValue("seq_ap_document");

                SetValue(AColumnNames, ref ANewRow, "a_ap_document_id_i", newValue.ToString());
                APDocumentNumberToId.Add(LedgerNumberAndAPNumber, newValue);
            }

            if (GetValue(AColumnNames, ANewRow, "a_date_issued_d") == "\\N")
            {
                SetValue(AColumnNames, ref ANewRow, "a_date_issued_d",
                    GetValue(AColumnNames, ANewRow, "a_date_entered_d"));
            }

            return true;
        }

        /// <summary>
        /// fixing table a_ap_anal_attrib
        /// </summary>
        public static bool FixAPAnalAttrib(StringCollection AColumnNames, ref string[] ANewRow)
        {
            TSequenceWriter.LoadSequences();
            LoadAPDocumentNumberToId();

            // a_ap_anal_attrib does not contain a_ap_number_i anymore, but the value has been copied into a_ap_document_id_i
            string LedgerNumberAndAPNumber =
                GetValue(AColumnNames, ANewRow, "a_ledger_number_i") + "_" +
                GetValue(AColumnNames, ANewRow, "a_ap_document_id_i");

            if (APDocumentNumberToId.ContainsKey(LedgerNumberAndAPNumber))
            {
                SetValue(AColumnNames, ref ANewRow, "a_ap_document_id_i", APDocumentNumberToId[LedgerNumberAndAPNumber].ToString());
            }
            else
            {
                long newValue = TSequenceWriter.GetNextSequenceValue("seq_ap_document");

                SetValue(AColumnNames, ref ANewRow, "a_ap_document_id_i", newValue.ToString());
                APDocumentNumberToId.Add(LedgerNumberAndAPNumber, newValue);
            }

            return true;
        }

        /// <summary>
        /// fixing table a_ap_document_detail
        /// </summary>
        public static bool FixAPDocumentDetail(StringCollection AColumnNames, ref string[] ANewRow)
        {
            TSequenceWriter.LoadSequences();
            LoadAPDocumentNumberToId();

            // a_ap_document_detail does not contain a_ap_number_i anymore, but the value has been copied into a_ap_document_id_i
            string LedgerNumberAndAPNumber =
                GetValue(AColumnNames, ANewRow, "a_ledger_number_i") + "_" +
                GetValue(AColumnNames, ANewRow, "a_ap_document_id_i");

            if (APDocumentNumberToId.ContainsKey(LedgerNumberAndAPNumber))
            {
                SetValue(AColumnNames, ref ANewRow, "a_ap_document_id_i", APDocumentNumberToId[LedgerNumberAndAPNumber].ToString());
            }
            else
            {
                long newValue = TSequenceWriter.GetNextSequenceValue("seq_ap_document");

                SetValue(AColumnNames, ref ANewRow, "a_ap_document_id_i", newValue.ToString());
                APDocumentNumberToId.Add(LedgerNumberAndAPNumber, newValue);
            }

            return true;
        }

        /// <summary>
        /// fixing table a_ap_document_payment
        /// </summary>
        public static bool FixAPDocumentPayment(StringCollection AColumnNames, ref string[] ANewRow)
        {
            TSequenceWriter.LoadSequences();
            LoadAPDocumentNumberToId();

            // a_ap_document_payment does not contain a_ap_number_i anymore, but the value has been copied into a_ap_document_id_i
            string LedgerNumberAndAPNumber =
                GetValue(AColumnNames, ANewRow, "a_ledger_number_i") + "_" +
                GetValue(AColumnNames, ANewRow, "a_ap_document_id_i");

            if (APDocumentNumberToId.ContainsKey(LedgerNumberAndAPNumber))
            {
                SetValue(AColumnNames, ref ANewRow, "a_ap_document_id_i", APDocumentNumberToId[LedgerNumberAndAPNumber].ToString());
            }
            else
            {
                long newValue = TSequenceWriter.GetNextSequenceValue("seq_ap_document");

                SetValue(AColumnNames, ref ANewRow, "a_ap_document_id_i", newValue.ToString());
                APDocumentNumberToId.Add(LedgerNumberAndAPNumber, newValue);
            }

            return true;
        }
    }
}