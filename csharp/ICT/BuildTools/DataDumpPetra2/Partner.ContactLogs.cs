//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       andreww
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
using Ict.Common;
using Ict.Tools.DBXML;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// Imports normalized ContactLog records from PartnerContacts
    /// </summary>
    public class TContactLogs : TFixData
    {
        /// <summary>
        /// Populate the empty table PPartnerGiftDestination using PmStaffData
        /// </summary>
        public static int PopulatePContactLog(StringCollection AColumnNames,
            ref string[] ANewRow,
            StreamWriter AWriter,
            StreamWriter AWriterTest)
        {
            // load the file p_partner_contact.d.gz
            TTable PartnerContact = TDumpProgressToPostgresql.GetStoreOld().GetTable("p_partner_contact");

            TParseProgressCSV ParserPartnerContact = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "p_partner_contact.d.gz",
                PartnerContact.grpTableField.Count);


            TTable ContactLog = TDumpProgressToPostgresql.GetStoreNew().GetTable("p_contact_log");
            StringCollection ContactLogColumnNames = GetColumnNames(ContactLog);

            string[] ContactLogRow = new string[ContactLogColumnNames.Count];
            int RowCounter = 0;

            string[] OldRow = ParserPartnerContact.ReadNextRow();

            while (OldRow != null)
            {
                //string ContactLogKey = TSequenceWriter.GetNextSequenceValue("seq_contact").ToString();

                for (int i = 0; i < OldRow.Length; i++)
                {
                    OldRow[i] = string.IsNullOrWhiteSpace(OldRow[i]) ? "\\N" : OldRow[i];
                }

                // p_contact_log row
                SetValue(ContactLogColumnNames, ref ContactLogRow, "p_contact_log_id_i", OldRow[0]); //ContactLogKey);
                SetValue(ContactLogColumnNames, ref ContactLogRow, "s_contact_date_d", OldRow[2]);
                SetValue(ContactLogColumnNames, ref ContactLogRow, "s_contact_time_i", OldRow[3]);

                if ((OldRow[4].Length == 0) || (OldRow[4] == "\\N"))
                {
                    SetValue(ContactLogColumnNames, ref ContactLogRow, "p_contact_code_c", "UNKNOWN");
                }
                else
                {
                    SetValue(ContactLogColumnNames, ref ContactLogRow, "p_contact_code_c", OldRow[4].ToUpper());
                }

                SetValue(ContactLogColumnNames, ref ContactLogRow, "p_contactor_c", OldRow[5]);
                SetValue(ContactLogColumnNames, ref ContactLogRow, "p_contact_message_id_c", OldRow[6]);
                SetValue(ContactLogColumnNames, ref ContactLogRow, "p_contact_comment_c", OldRow[7]);
                SetValue(ContactLogColumnNames, ref ContactLogRow, "s_module_id_c", OldRow[8]);
                SetValue(ContactLogColumnNames, ref ContactLogRow, "s_user_id_c", OldRow[9].ToUpper());
                SetValue(ContactLogColumnNames, ref ContactLogRow, "p_mailing_code_c", OldRow[10].ToUpper());
                SetValue(ContactLogColumnNames, ref ContactLogRow, "p_restricted_l", OldRow[11]);
                SetValue(ContactLogColumnNames, ref ContactLogRow, "p_contact_location_c", OldRow[12]);
                SetValue(ContactLogColumnNames, ref ContactLogRow, "s_date_created_d", OldRow[13]);
                SetValue(ContactLogColumnNames, ref ContactLogRow, "s_created_by_c", OldRow[14].ToUpper());
                SetValue(ContactLogColumnNames, ref ContactLogRow, "s_date_modified_d", OldRow[15]);
                SetValue(ContactLogColumnNames, ref ContactLogRow, "s_modified_by_c", OldRow[16].ToUpper());
                SetValue(ContactLogColumnNames, ref ContactLogRow, "s_modification_id_t", OldRow[17]);

                // p_partner_contact row
                SetValue(AColumnNames, ref ANewRow, "p_partner_key_n", OldRow[1]);
                SetValue(AColumnNames, ref ANewRow, "p_contact_log_id_i", OldRow[0]); //ContactLogKey);

                // write test records
                if (AWriterTest != null)
                {
                    AWriterTest.WriteLine("BEGIN; " + "COPY p_contact_log FROM stdin;");
                    AWriterTest.WriteLine(StringHelper.StrMerge(ContactLogRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("\\.");
                    AWriterTest.WriteLine("ROLLBACK;");
                }

                if (AWriterTest != null)
                {
                    AWriterTest.WriteLine("BEGIN; " + "COPY p_partner_contact FROM stdin;");
                    AWriterTest.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("\\.");
                    AWriterTest.WriteLine("ROLLBACK;");
                }

                OldRow = ParserPartnerContact.ReadNextRow();

                AWriter.WriteLine("COPY p_contact_log FROM stdin;");
                AWriter.WriteLine(StringHelper.StrMerge(ContactLogRow, '\t').Replace("\\\\N", "\\N").ToString());
                AWriter.WriteLine("\\.");
                AWriter.WriteLine("COPY p_partner_contact FROM stdin;");
                AWriter.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());

                if (OldRow != null)
                {
                    AWriter.WriteLine("\\.");
                }
                
                RowCounter += 2; // ContactLog record and PartnerContact record
            }

            return RowCounter;
        }
    }
}