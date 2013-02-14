//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using Ict.Tools.DBXML;

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

        static SortedList <Int64, string>SupplierCurrencies = null;
        private static string GetSupplierCurrency(Int64 SupplierPartnerKey)
        {
            if (SupplierCurrencies == null)
            {
                SupplierCurrencies = new SortedList <long, string>();

                // read supplier currencies from a_ap_supplier file
                TTable supplierTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("a_ap_supplier");

                TParseProgressCSV Parser = new TParseProgressCSV(
                    TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_ap_supplier.d.gz",
                    supplierTableOld.grpTableField.Count);

                StringCollection ColumnNames = GetColumnNames(supplierTableOld);

                while (true)
                {
                    string[] OldRow = Parser.ReadNextRow();

                    if (OldRow == null)
                    {
                        break;
                    }

                    SupplierCurrencies.Add(Convert.ToInt64(GetValue(ColumnNames, OldRow, "p_partner_key_n")),
                        GetValue(ColumnNames, OldRow, "a_currency_code_c"));
                }
            }

            return SupplierCurrencies[SupplierPartnerKey];
        }

        /// <summary>
        /// payment stored as Int64, ledgernumber*1000000 + payment number
        /// </summary>
        static SortedList <Int64, string>CurrencyPerPayment = null;
        private static string GetSupplierCurrencyFromPayment(Int32 ALedgerNumber, Int32 APaymentNumber)
        {
            if (CurrencyPerPayment == null)
            {
                SortedList <Int64, string>CurrencyPerDocument = new SortedList <Int64, string>();

                TTable documentTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("a_ap_document");

                TParseProgressCSV Parser = new TParseProgressCSV(
                    TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_ap_document.d.gz",
                    documentTableOld.grpTableField.Count);

                StringCollection ColumnNames = GetColumnNames(documentTableOld);

                while (true)
                {
                    string[] OldRow = Parser.ReadNextRow();

                    if (OldRow == null)
                    {
                        break;
                    }

                    Int64 LedgerAndAPNumber = Convert.ToInt64(GetValue(ColumnNames, OldRow, "a_ledger_number_i")) * 1000000 +
                                              Convert.ToInt64(GetValue(ColumnNames, OldRow, "a_ap_number_i"));

                    CurrencyPerDocument.Add(LedgerAndAPNumber,
                        GetSupplierCurrency(Convert.ToInt64(GetValue(ColumnNames, OldRow, "p_partner_key_n"))));
                }

                CurrencyPerPayment = new SortedList <long, string>();

                TTable documentpaymentTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("a_ap_document_payment");

                Parser = new TParseProgressCSV(
                    TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_ap_document_payment.d.gz",
                    documentpaymentTableOld.grpTableField.Count);

                ColumnNames = GetColumnNames(documentpaymentTableOld);

                while (true)
                {
                    string[] OldRow = Parser.ReadNextRow();

                    if (OldRow == null)
                    {
                        break;
                    }

                    Int64 LedgerAndPaymentNumber = Convert.ToInt64(GetValue(ColumnNames, OldRow, "a_ledger_number_i")) * 1000000 +
                                                   Convert.ToInt64(GetValue(ColumnNames, OldRow, "a_payment_number_i"));

                    if (!CurrencyPerPayment.ContainsKey(LedgerAndPaymentNumber))
                    {
                        CurrencyPerPayment.Add(LedgerAndPaymentNumber,
                            CurrencyPerDocument[
                                Convert.ToInt64(GetValue(ColumnNames, OldRow, "a_ledger_number_i")) * 1000000 +
                                Convert.ToInt64(GetValue(ColumnNames, OldRow, "a_ap_number_i"))]);
                    }
                }
            }

            return CurrencyPerPayment[ALedgerNumber * 1000000 + APaymentNumber];
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

            // we need to set a_currency_code_c which is stored with the supplier
            SetValue(AColumnNames, ref ANewRow, "a_currency_code_c",
                GetSupplierCurrency(Convert.ToInt64(GetValue(AColumnNames, ANewRow, "p_partner_key_n"))));

            return true;
        }

        /// <summary>
        /// fixing table a_ap_payment
        /// </summary>
        public static bool FixAPPayment(StringCollection AColumnNames, ref string[] ANewRow)
        {
            // we need to set a_currency_code_c which is stored with the supplier
            SetValue(AColumnNames, ref ANewRow, "a_currency_code_c",
                GetSupplierCurrencyFromPayment(
                    Convert.ToInt32(GetValue(AColumnNames, ANewRow, "a_ledger_number_i")),
                    Convert.ToInt32(GetValue(AColumnNames, ANewRow, "a_payment_number_i"))));

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