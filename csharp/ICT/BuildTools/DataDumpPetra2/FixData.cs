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
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Ict.Common;
using Ict.Tools.DBXML;
using ICSharpCode.SharpZipLib.GZip;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// we need to fix some data content, otherwise loading into Postgresql would violate foreign key constraints or other constraints.
    /// do not fix the old Petra 2.x database, since it sometimes depends on these things.
    /// </summary>
    public class TFixData
    {
        private static int FormSequence = 1;
        private static List <string>PostcodeRegionsList = new List <string>();

        /// <summary>
        /// set a value of a row, position given by AColumnNames
        /// </summary>
        protected static void SetValue(StringCollection AColumnNames,
            ref string[] ACurrentRow,
            string AColumnName,
            string ANewValue)
        {
            int index = AColumnNames.IndexOf(AColumnName);

            if (index == -1)
            {
                throw new Exception("TFixData.SetValue: Problem with unknown column name " + AColumnName);
            }

            ACurrentRow[index] = ANewValue;
        }

        /// <summary>
        /// get a value from a row, position given by AColumnNames
        /// </summary>
        protected static string GetValue(StringCollection AColumnNames,
            string[] ACurrentRow,
            string AColumnName)
        {
            int index = AColumnNames.IndexOf(AColumnName);

            if (index == -1)
            {
                throw new Exception("TFixData.GetValue: Problem with unknown column name " + AColumnName);
            }

            return ACurrentRow[index];
        }

        private static string[] CreateRow(StringCollection AColumnNames)
        {
            return new string[AColumnNames.Count];
        }

        /// <summary>
        /// get the names of the columns for the given table
        /// </summary>
        /// <param name="ATable"></param>
        /// <returns></returns>
        protected static StringCollection GetColumnNames(TTable ATable)
        {
            StringCollection ColumnNames = new StringCollection();

            foreach (TTableField field in ATable.grpTableField)
            {
                ColumnNames.Add(field.strName);
            }

            return ColumnNames;
        }

        private static string FixValue(string AValue, TTableField ANewField)
        {
            if (ANewField.strName == "s_modification_id_t")
            {
                AValue = "\\N";
            }
            else if ((ANewField.strName == "s_created_by_c")
                     || (ANewField.strName == "s_merged_by_c")
                     || (ANewField.strName == "s_modified_by_c")
                     || (ANewField.strName == "m_manual_mod_by_c")
                     || (ANewField.strName == "p_country_of_issue_c")
                     || (ANewField.strName == "a_transaction_currency_c")
                     || (ANewField.strName == "pm_st_leadership_rating_c")
                     || (ANewField.strName == "pm_passport_details_type_c")
                     || (ANewField.strName == "p_marital_status_c")
                     || (ANewField.strName == "p_owner_c")
                     || (ANewField.strName == "s_user_id_c")
                     || (ANewField.strName == "p_relation_name_c")
                     || ANewField.strName.EndsWith("_code_c"))
            {
                AValue = AValue.Trim().ToUpper();

                if (!ANewField.bNotNull)
                {
                    if (AValue.Length == 0)
                    {
                        AValue = "\\N";
                    }
                }
            }
            else if (!ANewField.bNotNull
                     && ((ANewField.strName == "p_field_key_n")
                         || (ANewField.strName == "p_bank_key_n")
                         || (ANewField.strName == "p_partner_key_n")
                         || (ANewField.strName == "p_contact_partner_key_n")
                         || (ANewField.strName == "p_recipient_key_n")
                         || (ANewField.strName == "a_recipient_ledger_number_n")
                         || (ANewField.strName == "pm_gen_app_poss_srv_unit_key_n")
                         || (ANewField.strName == "a_ilt_processing_centre_n")
                         || (ANewField.strName == "pm_st_field_charged_n")
                         || (ANewField.strName == "pm_st_current_field_n")
                         || (ANewField.strName == "pm_st_option2_n")
                         || (ANewField.strName == "pm_st_option1_n")
                         || (ANewField.strName == "pm_st_confirmed_option_n")
                         || (ANewField.strName == "pm_office_recruited_by_n")
                         || (ANewField.strName == "pm_home_office_n")
                         || (ANewField.strName == "p_primary_office_n")
                         || (ANewField.strName == "p_value_partner_key_n")
                         || (ANewField.strName == "pm_receiving_field_office_n")
                         || (ANewField.strName == "a_key_ministry_key_n")
                         || (ANewField.strName == "pm_placement_partner_key_n")
                         || (ANewField.strName == "pm_contact_partner_key_n")
                         ))
            {
                if (AValue == "0")
                {
                    AValue = "\\N";
                }
            }
            else if (!ANewField.bNotNull
                     && ((ANewField.strName == "pt_qualification_area_name_c")
                         || (ANewField.strName == "pm_passport_details_type_c")
                         ))
            {
                if (AValue == "")
                {
                    AValue = "\\N";
                }
            }
            else if ((AValue.Length == 0) && ANewField.strType.Equals("VARCHAR", StringComparison.OrdinalIgnoreCase) && !ANewField.bNotNull)
            {
                AValue = "\\N";
            }
            else if (ANewField.strType.Equals("BIT", StringComparison.OrdinalIgnoreCase))
            {
                AValue = (AValue == "yes") ? "1" : "0";
            }
            else if (ANewField.strType.Equals("DATE", StringComparison.OrdinalIgnoreCase))
            {
                if ((AValue.Length > 0) && (AValue != "\\N"))
                {
                    if (AValue.Length != 10)
                    {
                        TLogging.Log("WARNING: Invalid date: " + ANewField.strName + " " + AValue);
                        AValue = "\\N";
                    }
                    else
                    {
                        // fulldump23.p does write all dates in format dmy
                        // 15/04/2010 => 2010-04-15
                        AValue = string.Format("{0}-{1}-{2}", AValue.Substring(6, 4), AValue.Substring(3, 2), AValue.Substring(0, 2));
                    }
                }
            }

            return AValue;
        }

        /// <summary>
        /// fix data that would cause problems for PostgreSQL constraints
        /// </summary>
        public static int MigrateData(TParseProgressCSV AParser, StreamWriter AWriter, StreamWriter AWriterTest, TTable AOldTable, TTable ANewTable)
        {
            StringCollection OldColumnNames = GetColumnNames(AOldTable);
            StringCollection NewColumnNames = GetColumnNames(ANewTable);
            int RowCounter = 0;

            List <TTableField>MappingOfFields = new List <TTableField>();
            List <string>DefaultValues = new List <string>();

            foreach (TTableField newField in ANewTable.grpTableField)
            {
                string oldname = "";

                TTableField oldField = AOldTable.GetField(newField.strName);

                if ((oldField == null) && (DataDefinitionDiff.GetNewFieldName(ANewTable.strName, ref oldname, ref newField.strName)))
                {
                    oldField = AOldTable.GetField(oldname);
                }

                MappingOfFields.Add(oldField);

                // prepare the default values once
                // this is a new field. insert default value
                string defaultValue = "\\N";

                if ((newField.strInitialValue != null) && (newField.strInitialValue.Length > 0))
                {
                    if (newField.strInitialValue.ToUpper() == "TODAY")
                    {
                        // it does not make sense to set s_date_created_d to today during conversion.
                        // so no change to defaultValue.
                    }
                    else if (newField.strType.ToUpper() == "VARCHAR")
                    {
                        defaultValue = '"' + newField.strInitialValue + "\"";
                    }
                    else if (newField.strType.ToUpper() == "BIT")
                    {
                        defaultValue = newField.strInitialValue;

                        if (newField.strFormat.Contains(newField.strInitialValue))
                        {
                            defaultValue = newField.strFormat.StartsWith(newField.strInitialValue) ? "0" : "1";
                        }
                    }
                    else
                    {
                        defaultValue = newField.strInitialValue;
                    }
                }

                DefaultValues.Add(defaultValue);
            }

            string[] NewRow = CreateRow(NewColumnNames);

            // This existing and populated table's data is completely changed. We do not want to import any of it's contents.
            if (ANewTable.strName == "pt_language_level")
            {
                RowCounter += FixData(ANewTable.strName, NewColumnNames, ref NewRow, AWriter, AWriterTest);

                return RowCounter;
            }

            while (true)
            {
                string[] OldRow = AParser.ReadNextRow();

                if (OldRow == null)
                {
                    break;
                }

                int fieldCounter = 0;

                foreach (TTableField newField in ANewTable.grpTableField)
                {
                    TTableField oldField = MappingOfFields[fieldCounter];

                    if (oldField != null)
                    {
                        string value = GetValue(OldColumnNames, OldRow, oldField.strName);

                        value = FixValue(value, newField);

                        SetValue(NewColumnNames, ref NewRow, newField.strName, value);
                    }
                    else
                    {
                        SetValue(NewColumnNames, ref NewRow, newField.strName, DefaultValues[fieldCounter]);
                    }

                    fieldCounter++;
                }

                if (FixData(ANewTable.strName, NewColumnNames, ref NewRow))
                {
                    RowCounter++;
                    AWriter.WriteLine(CSVFile.StrMergeSpecial(NewRow));
                    AWriterTest.WriteLine("BEGIN; " + "COPY " + ANewTable.strName + " FROM stdin;");
                    AWriterTest.WriteLine(CSVFile.StrMergeSpecial(NewRow));
                    AWriterTest.WriteLine("\\.");
                    AWriterTest.WriteLine("ROLLBACK;");
                }
            }

            RowCounter += FixData(ANewTable.strName, NewColumnNames, ref NewRow, AWriter, AWriterTest);

            return RowCounter;
        }

        static SortedList <string, string[]>PPersonBelieverInfo = null;
        static string FPreviousLoginTime = string.Empty;

        /// <summary>
        /// fix data that would cause problems for PostgreSQL constraints
        /// </summary>
        /// <returns>false if the row should be dropped</returns>
        public static bool FixData(string ATableName, StringCollection AColumnNames, ref string[] ANewRow)
        {
            if (ATableName == "a_budget")
            {
                return false;
            }

            if (ATableName == "a_budget_period")
            {
                return false;
            }

            // update pub.a_account_property set a_property_value_c = 'true' where a_property_code_c = 'Bank Account';
            if (ATableName == "a_account_property")
            {
                if (GetValue(AColumnNames, ANewRow, "a_property_code_c") == "Bank Account")
                {
                    SetValue(AColumnNames, ref ANewRow, "a_property_value_c", "true");
                }
            }

            if (ATableName == "a_ap_document")
            {
                return TFinanceAccountsPayableUpgrader.FixAPDocument(AColumnNames, ref ANewRow);
            }

            if (ATableName == "a_ap_payment")
            {
                return TFinanceAccountsPayableUpgrader.FixAPPayment(AColumnNames, ref ANewRow);
            }

            if (ATableName == "a_ap_anal_attrib")
            {
                return TFinanceAccountsPayableUpgrader.FixAPAnalAttrib(AColumnNames, ref ANewRow);
            }

            if (ATableName == "a_ap_document_detail")
            {
                return TFinanceAccountsPayableUpgrader.FixAPDocumentDetail(AColumnNames, ref ANewRow);
            }

            if (ATableName == "a_ap_document_payment")
            {
                return TFinanceAccountsPayableUpgrader.FixAPDocumentPayment(AColumnNames, ref ANewRow);
            }

            if (ATableName == "s_login")
            {
                SetValue(AColumnNames, ref ANewRow, "s_login_process_id_r", TSequenceWriter.GetNextSequenceValue("seq_login_process_id").ToString());

                string LoginTime = GetValue(AColumnNames, ANewRow, "s_login_time_i");

                while (FPreviousLoginTime == LoginTime)
                {
                    int intLoginTime = Convert.ToInt32(LoginTime);
                    LoginTime = (intLoginTime + 1).ToString();
                    SetValue(AColumnNames, ref ANewRow, "s_login_time_i", LoginTime);
                }

                FPreviousLoginTime = LoginTime;
            }

            if (ATableName == "a_journal")
            {
                // date of entry must not be NULL
                if (GetValue(AColumnNames, ANewRow, "a_date_of_entry_d") == "\\N")
                {
                    SetValue(AColumnNames, ref ANewRow, "a_date_of_entry_d",
                        GetValue(AColumnNames, ANewRow, "a_date_effective_d"));
                }
            }

            // a_email_destination.a_conditional_value_c is sometimes null, but it is part of the primary key
            if (ATableName == "a_email_destination")
            {
                string ConditionalValue = GetValue(AColumnNames, ANewRow, "a_conditional_value_c");

                if ((ConditionalValue == "\\N") || (ConditionalValue.Length == 0))
                {
                    SetValue(AColumnNames, ref ANewRow, "a_conditional_value_c", "NOT SET");
                }
            }

            // s_user_group contains some SQL_* users, which are not part of the s_user table
            if (ATableName == "s_user_group")
            {
                if (GetValue(AColumnNames, ANewRow, "s_user_id_c").StartsWith("SQL_"))
                {
                    // do not write this line
                    return false;
                }
            }

            // there is a space in front of the code, which causes a duplicate primary key
            if (ATableName == "p_type")
            {
                if (GetValue(AColumnNames, ANewRow, "p_type_code_c") == " STAFF")
                {
                    return false;
                }
            }

            // there is a space in front of the code, which causes a duplicate primary key
            if (ATableName == "p_reason_subscription_given")
            {
                if (GetValue(AColumnNames, ANewRow, "p_code_c") == " FREE")
                {
                    return false;
                }
            }

            // fix foreign key, remove space
            if (ATableName == "p_subscription")
            {
                string value = GetValue(AColumnNames, ANewRow, "p_reason_subs_given_code_c");

                if (value == " FREE")
                {
                    SetValue(AColumnNames, ref ANewRow, "p_reason_subs_given_code_c", "FREE");
                }
                else if (value.Length == 0)
                {
                    // p_reason_subs_given_code_c must not be NULL
                    SetValue(AColumnNames, ref ANewRow, "p_reason_subs_given_code_c", "FREE");
                }
            }

            // pm_person_language, language code cannot be null, should be 99.
            // Old language levels need mapped to new levels.
            if (ATableName == "pm_person_language")
            {
                string val = GetValue(AColumnNames, ANewRow, "p_language_code_c");

                if ((val.Length == 0) || (val == "\\N"))
                {
                    SetValue(AColumnNames, ref ANewRow, "p_language_code_c", "99");
                }

                int val2 = Convert.ToInt32(GetValue(AColumnNames, ANewRow, "pt_language_level_i"));

                if (val2 <= 3)
                {
                    SetValue(AColumnNames, ref ANewRow, "pt_language_level_i", "1");
                }
                else if ((val2 >= 4) && (val2 <= 7))
                {
                    SetValue(AColumnNames, ref ANewRow, "pt_language_level_i", "2");
                }
                else if (val2 >= 8)
                {
                    SetValue(AColumnNames, ref ANewRow, "pt_language_level_i", "3");
                }
            }

            // um_unit_language, language code cannot be null, should be 99.
            // Old language levels need mapped to new levels.
            if (ATableName == "um_unit_language")
            {
                string val = GetValue(AColumnNames, ANewRow, "p_language_code_c");

                if ((val.Length == 0) || (val == "\\N"))
                {
                    SetValue(AColumnNames, ref ANewRow, "p_language_code_c", "99");
                }

                int val2 = Convert.ToInt32(GetValue(AColumnNames, ANewRow, "pt_language_level_i"));

                if (val2 <= 3)
                {
                    SetValue(AColumnNames, ref ANewRow, "pt_language_level_i", "1");
                }
                else if ((val2 >= 4) && (val2 <= 7))
                {
                    SetValue(AColumnNames, ref ANewRow, "pt_language_level_i", "2");
                }
                else if (val2 >= 8)
                {
                    SetValue(AColumnNames, ref ANewRow, "pt_language_level_i", "3");
                }
            }

            // Old language levels need mapped to new levels.
            if (ATableName == "um_job_language")
            {
                int val = Convert.ToInt32(GetValue(AColumnNames, ANewRow, "pt_language_level_i"));

                if (val <= 3)
                {
                    SetValue(AColumnNames, ref ANewRow, "pt_language_level_i", "1");
                }
                else if ((val >= 4) && (val <= 7))
                {
                    SetValue(AColumnNames, ref ANewRow, "pt_language_level_i", "2");
                }
                else if (val >= 8)
                {
                    SetValue(AColumnNames, ref ANewRow, "pt_language_level_i", "3");
                }
            }

            // p_partner_contact, method of contact cannot be null, should be UNKNOWN
            if (ATableName == "p_partner_contact")
            {
                string val = GetValue(AColumnNames, ANewRow, "p_contact_code_c");

                if ((val.Length == 0) || (val == "\\N"))
                {
                    SetValue(AColumnNames, ref ANewRow, "p_contact_code_c", "UNKNOWN");
                }
            }

            if (ATableName == "a_batch")
            {
                return TFinanceGeneralLedgerUpgrader.FixABatch(AColumnNames, ref ANewRow);
            }

            if (ATableName == "a_budget_type")
            {
                return TFinanceBudgetUpgrader.FixABudgetType(AColumnNames, ref ANewRow);
            }

            if (ATableName == "a_account")
            {
                return TFinanceBudgetUpgrader.FixABudgetType(AColumnNames, ref ANewRow);
            }

            if (ATableName == "a_motivation_detail")
            {
                return TFinanceGeneralLedgerUpgrader.FixAMotivationDetail(AColumnNames, ref ANewRow);
            }

            // wrong gift batch status, need to have case sensitive status
            if (ATableName == "a_gift_batch")
            {
                string val = GetValue(AColumnNames, ANewRow, "a_batch_status_c");

                if (val == "posted")
                {
                    SetValue(AColumnNames, ref ANewRow, "a_batch_status_c", "Posted");
                }
            }

            // bank code has too many characters, remove spaces
            if (ATableName == "p_bank")
            {
                string val = GetValue(AColumnNames, ANewRow, "p_branch_code_c");

                if (val.Length > 20)
                {
                    SetValue(AColumnNames, ref ANewRow, "p_branch_code_c", val.Replace(" ", ""));
                }
            }

            // if target field is null or 0, use the home office partner key
            if (ATableName == "pm_staff_data")
            {
                string ReceivingField = GetValue(AColumnNames, ANewRow, "pm_receiving_field_n");
                string HomeOffice = GetValue(AColumnNames, ANewRow, "pm_home_office_n");

                if ((HomeOffice == "0") || (HomeOffice.Length == 0) || (HomeOffice == "\\N"))
                {
                    HomeOffice = GetValue(AColumnNames, ANewRow, "pm_office_recruited_by_n");
                    SetValue(AColumnNames, ref ANewRow, "pm_home_office_n", HomeOffice);
                }

                if ((ReceivingField == "0") || (ReceivingField.Length == 0) || (ReceivingField == "\\N"))
                {
                    SetValue(AColumnNames, ref ANewRow, "pm_receiving_field_n", HomeOffice);
                }
            }

            // pm_st_basic_outreach_id_c cannot be null
            if (ATableName == "pm_short_term_application")
            {
                string val = GetValue(AColumnNames, ANewRow, "pm_st_basic_outreach_id_c");

                if ((val == "0") || (val.Length == 0) || (val == "\\N"))
                {
                    SetValue(AColumnNames, ref ANewRow, "pm_st_basic_outreach_id_c",
                        GetValue(AColumnNames, ANewRow, "pm_registration_office_n") + "-" +
                        GetValue(AColumnNames, ANewRow, "pm_application_key_i"));
                }

                val = GetValue(AColumnNames, ANewRow, "pm_st_field_charged_n");

                if ((val == "0") || (val.Length == 0) || (val == "\\N"))
                {
                    SetValue(AColumnNames, ref ANewRow, "pm_st_field_charged_n",
                        GetValue(AColumnNames, ANewRow, "pm_registration_office_n"));
                }
            }

            if (ATableName == "um_job_qualification")
            {
                if (GetValue(AColumnNames, ANewRow, "pt_qualification_area_name_c") == "")
                {
                    SetValue(AColumnNames, ref ANewRow, "pt_qualification_area_name_c", "OTHER");
                }
            }

            if (ATableName == "p_person")
            {
                string val = GetValue(AColumnNames, ANewRow, "p_family_id_i");

                if ((val == "") || (val.Length == 0) || (val == "\\N"))
                {
                    // p_family_id_i is now NOT NULL, but for merged partners, it is reset in Petra 2.x
                    SetValue(AColumnNames, ref ANewRow, "p_family_id_i", "-1");
                }
            }

            // pm_personal_data: move values from the p_person table for believer info
            if (ATableName == "pm_personal_data")
            {
                if (PPersonBelieverInfo == null)
                {
                    PPersonBelieverInfo = new SortedList <string, string[]>();

                    // load the file p_person.d.gz so that we can access the values for each person
                    TTable personTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("p_person");

                    TParseProgressCSV Parser = new TParseProgressCSV(
                        TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "p_person.d.gz",
                        personTableOld.grpTableField.Count);

                    StringCollection PersonColumnNames = GetColumnNames(personTableOld);

                    string personKey = string.Empty;
                    string believerSinceYear = string.Empty;
                    string believerSinceComment = string.Empty;

                    while (true)
                    {
                        string[] OldRow = Parser.ReadNextRow();

                        if (OldRow == null)
                        {
                            break;
                        }

                        personKey = GetValue(PersonColumnNames, OldRow, "p_partner_key_n");
                        believerSinceComment = GetValue(PersonColumnNames, OldRow, "p_believer_since_comment_c");
                        believerSinceYear = GetValue(PersonColumnNames, OldRow, "p_believer_since_year_i");
                        PPersonBelieverInfo.Add(personKey, new string[] { believerSinceComment, believerSinceYear });
                    }
                }

                string partnerkey = GetValue(AColumnNames, ANewRow, "p_partner_key_n");

                string[] believerInfo = PPersonBelieverInfo[partnerkey];
                SetValue(AColumnNames, ref ANewRow, "p_believer_since_comment_c", believerInfo[0]);
                SetValue(AColumnNames, ref ANewRow, "p_believer_since_year_i", believerInfo[1]);
            }

            // new sequence for pc_room_alloc
            if (ATableName == "pc_room_alloc")
            {
                SetValue(AColumnNames, ref ANewRow, "pc_key_i", TSequenceWriter.GetNextSequenceValue("seq_room_alloc").ToString());
            }

            if (ATableName == "a_form_element")
            {
                if (GetValue(AColumnNames, ANewRow, "a_form_sequence_i") == "0")
                {
                    SetValue(AColumnNames, ref ANewRow, "a_form_sequence_i", FormSequence.ToString());
                    FormSequence++;
                }
            }

            if (ATableName == "p_postcode_region")
            {
                string CurrentRegion = ANewRow[0];

                foreach (string OldRegion in PostcodeRegionsList)
                {
                    if (CurrentRegion == OldRegion)
                    {
                        return false;
                    }
                }

                PostcodeRegionsList.Add(CurrentRegion);
            }

            // p_partner_status, 'DIED', 'INACTIVE' and 'MERGED' partners are not active
            if (ATableName == "p_partner_status")
            {
                string val = GetValue(AColumnNames, ANewRow, "p_status_code_c");

                if ((val == "DIED") || (val == "INACTIVE") || (val == "MERGED"))
                {
                    SetValue(AColumnNames, ref ANewRow, "p_partner_is_active_l", "0");
                }
            }

            // phone and fax extensions should be '0' rather than null
            if (ATableName == "p_partner_location")
            {
                string val = GetValue(AColumnNames, ANewRow, "p_extension_i");

                if ((val.Length == 0) || (val == "\\N"))
                {
                    SetValue(AColumnNames, ref ANewRow, "p_extension_i", "0");
                }

                val = GetValue(AColumnNames, ANewRow, "p_fax_extension_i");

                if ((val.Length == 0) || (val == "\\N"))
                {
                    SetValue(AColumnNames, ref ANewRow, "p_fax_extension_i", "0");
                }
            }

            // renaming "Gift Receipting" to "Gift Processing" (Mantis 1930)
            if (ATableName == "a_sub_system")
            {
                string val = GetValue(AColumnNames, ANewRow, "a_sub_system_name_c");

                if (val == "Gift Receipting")
                {
                    SetValue(AColumnNames, ref ANewRow, "a_sub_system_name_c", "Gift Processing");
                }
            }

            // renaming "Gift Receipting" to "Gift Processing" (Mantis 1930)
            if (ATableName == "a_transaction_type")
            {
                string val = GetValue(AColumnNames, ANewRow, "a_transaction_type_description_c");

                if (val == "Gift Receipting")
                {
                    SetValue(AColumnNames, ref ANewRow, "a_transaction_type_description_c", "Gift Processing");
                }
            }

            // A new password and password salt needs to be generated for every user.
            // Passwords are writed to a file in the fulldump folder.
            if (ATableName == "s_user")
            {
                string Password;
                string Salt;
                string PasswordHash;

                PasswordHelper.GetNewPasswordSaltAndHash(out Password, out Salt, out PasswordHash);

                SetValue(AColumnNames, ref ANewRow, "s_password_salt_c", Salt);
                SetValue(AColumnNames, ref ANewRow, "s_password_hash_c", PasswordHash);

                // the user will have to change their password the first time they log in
                SetValue(AColumnNames, ref ANewRow, "s_password_needs_change_l", "true");

                // write userIDs and new passwords to a file
                string UserPasswordsDir = TAppSettingsManager.GetValue("fulldumpPath", "fulldump") +
                                          Path.DirectorySeparatorChar + "_credentials.txt";
                StreamWriter MyWriter;

                if (File.Exists(UserPasswordsDir))
                {
                    MyWriter = File.AppendText(UserPasswordsDir);
                }
                else
                {
                    FileStream outStreamCount = File.Create(UserPasswordsDir);
                    MyWriter = new StreamWriter(outStreamCount);
                }

                string UserID = GetValue(AColumnNames, ANewRow, "s_user_id_c");

                MyWriter.WriteLine(UserID + "\t" + Password);

                MyWriter.Close();
            }

            return true;
        }

        private static int FixData(string ATableName,
            StringCollection AColumnNames,
            ref string[] ANewRow,
            StreamWriter AWriter,
            StreamWriter AWriterTest)
        {
            int RowCounter = 0;

            if (ATableName == "a_budget_revision")
            {
                RowCounter = TFinanceBudgetUpgrader.PopulateABudgetRevision(AColumnNames, ref ANewRow, AWriter, AWriterTest);
            }

            if (ATableName == "a_budget")
            {
                RowCounter = TFinanceBudgetUpgrader.FixABudget(AColumnNames, ref ANewRow, AWriter, AWriterTest);
            }

            if (ATableName == "a_budget_period")
            {
                RowCounter = TFinanceBudgetUpgrader.FixABudgetPeriod(AColumnNames, ref ANewRow, AWriter, AWriterTest);
            }

            if (ATableName == "s_system_defaults")
            {
                // load the file a_system_parameter.d.gz so that we can access s_system_parameter, s_site_key_n
                TTable systemParameterTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("s_system_parameter");

                TParseProgressCSV Parser = new TParseProgressCSV(
                    TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "s_system_parameter.d.gz",
                    systemParameterTableOld.grpTableField.Count);

                StringCollection ColumnNames = GetColumnNames(systemParameterTableOld);

                while (true)
                {
                    string[] OldRow = Parser.ReadNextRow();

                    if (OldRow == null)
                    {
                        break;
                    }

                    // needs to be added to s_system_defaults name=SiteKey
                    string SiteKey = GetValue(ColumnNames, OldRow, "s_site_key_n");

                    SetValue(AColumnNames, ref ANewRow, "s_default_code_c", "SiteKey");
                    SetValue(AColumnNames, ref ANewRow, "s_default_description_c", "there has to be one site key for the database");
                    SetValue(AColumnNames, ref ANewRow, "s_default_value_c", SiteKey);

                    AWriter.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("BEGIN; " + "COPY " + ATableName + " FROM stdin;");
                    AWriterTest.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("\\.");
                    AWriterTest.WriteLine("ROLLBACK;");
                    RowCounter++;
                }
            }

            // this is a new table with new data (also in basedata)
            if (ATableName == "pt_skill_category")
            {
                // Default categories that old abilities and qualifications are mapped to. Basedata - pt_skill_category
                string[, ] SkillCategories = new string[, ] {
                    {
                        "COMMUNICATION", "e.g. Graphic Designer, Journalist, Photographer"
                    },
                    {
                        "EDUCATION", "e.g. Teacher, Lecturer"
                    },
                    {
                        "FINANCE", "e.g. Accountant, Auditor, Bookkeeper"
                    },
                    {
                        "FOOD", "e.g. Caterer, Baker, Cook"
                    },
                    {
                        "LAW", "e.g. Solicitor, Lawyer, Judge"
                    },
                    {
                        "MEDICAL", "e.g. Dentist, Doctor, Nurse"
                    },
                    {
                        "MINISTRY", "e.g. Pastor, Evangelist, Counsellor"
                    },
                    {
                        "MUSIC", "e.g. Musician, Singer"
                    },
                    {
                        "OFFICE", "e.g. Manager, Secretary, Computer Programmer"
                    },
                    {
                        "OTHER", ""
                    },
                    {
                        "PEOPLE", "e.g. Personnel Administrator, Social Worker"
                    },
                    {
                        "PRACTICAL", "e.g. Cleaner, Driver, Farmer"
                    },
                    {
                        "SEA", "e.g. Captain, Engineer, Deck Hand"
                    },
                    {
                        "TECHNICAL", "e.g. Engineer, Carpenter, Electrician"
                    }
                };

                for (int i = 0; i < SkillCategories.GetLength(0); i++)
                {
                    SetValue(AColumnNames, ref ANewRow, "pt_code_c", SkillCategories[i, 0]);
                    SetValue(AColumnNames, ref ANewRow, "pt_description_c", SkillCategories[i, 1]);
                    SetValue(AColumnNames, ref ANewRow, "pt_unassignable_flag_l", "0");
                    SetValue(AColumnNames, ref ANewRow, "pt_unassignable_date_d", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "pt_deletable_flag_l", "0");
                    SetValue(AColumnNames, ref ANewRow, "s_date_created_d", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "s_created_by_c", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "s_date_modified_d", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "s_modified_by_c", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "s_modification_id_t", "\\N");

                    AWriter.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("BEGIN; " + "COPY " + ATableName + " FROM stdin;");
                    AWriterTest.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("\\.");
                    AWriterTest.WriteLine("ROLLBACK;");
                    RowCounter++;
                }
            }

            // this is a new table with new data (also in basedata)
            if (ATableName == "pt_skill_level")
            {
                // Default levels that old abilities and qualifications levels are mapped to. Basedata - pt_skill_level
                string[] SkillLevel = new string[] {
                    "1", "2", "3", "4", "99"
                };
                string[] SkillLevelDescription = new string[] {
                    "Basic", "Moderate", "Competent", "Professional", "Level of ability not known"
                };

                for (int i = 0; i < SkillLevel.Length; i++)
                {
                    SetValue(AColumnNames, ref ANewRow, "pt_level_i", SkillLevel[i]);
                    SetValue(AColumnNames, ref ANewRow, "pt_description_c", SkillLevelDescription[i]);
                    SetValue(AColumnNames, ref ANewRow, "pt_unassignable_flag_l", "0");
                    SetValue(AColumnNames, ref ANewRow, "pt_unassignable_date_d", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "pt_deletable_flag_l", "0");
                    SetValue(AColumnNames, ref ANewRow, "s_date_created_d", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "s_created_by_c", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "s_date_modified_d", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "s_modified_by_c", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "s_modification_id_t", "\\N");

                    AWriter.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("BEGIN; " + "COPY " + ATableName + " FROM stdin;");
                    AWriterTest.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("\\.");
                    AWriterTest.WriteLine("ROLLBACK;");
                    RowCounter++;
                }
            }

            // data from pm_person_ability and pm_person_qualification are copied into this new table
            if (ATableName == "pm_person_skill")
            {
                // Default categories that old abilities and qualifications are mapped to. Basedata - pt_skill_category
                string[] SkillCategories = new string[] {
                    "COMMUNICATION", "EDUCATION", "FINANCE", "FOOD", "LAW", "MEDICAL", "MINISTRY", "MUSIC", "OFFICE",
                    "PEOPLE", "PRACTICAL", "SEA", "TECHNICAL"
                };

                String SkillCategory;
                String Description;
                int SkillLevel;

                //*** Copy from pm_person_ability ***//

                // load the file pm_person_ability.d.gz
                TTable PersonAbility = TDumpProgressToPostgresql.GetStoreOld().GetTable("pm_person_ability");

                TParseProgressCSV ParserAbility = new TParseProgressCSV(
                    TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "pm_person_ability.d.gz",
                    PersonAbility.grpTableField.Count);

                StringCollection PersonAbilityColumnNames = GetColumnNames(PersonAbility);

                // these columns will be the same for all records
                SetValue(AColumnNames, ref ANewRow, "pm_description_local_c", "\\N");
                SetValue(AColumnNames, ref ANewRow, "pm_description_language_c", "\\N");
                SetValue(AColumnNames, ref ANewRow, "pm_current_occupation_l", "0");
                SetValue(AColumnNames, ref ANewRow, "pm_degree_c", "\\N");
                SetValue(AColumnNames, ref ANewRow, "pm_year_of_degree_i", "0");

                while (true)
                {
                    string[] OldRow = ParserAbility.ReadNextRow();

                    if (OldRow == null)
                    {
                        break;
                    }

                    // map old ability_area_name to new skill category
                    String AbilityAreaName = GetValue(PersonAbilityColumnNames, OldRow, "pt_ability_area_name_c");
                    SkillCategory = "OTHER";
                    Description = "";

                    foreach (string Category in SkillCategories)
                    {
                        if (AbilityAreaName.Substring(0, 3) == Category.Substring(0, 3))
                        {
                            SkillCategory = Category;
                            break;
                        }
                    }

                    // copy old ability_area description from pt_ability_area to new description

                    // load the file pt_ability_area.d.gz so that we can access the values for each person
                    TTable AbilityArea = TDumpProgressToPostgresql.GetStoreOld().GetTable("pt_ability_area");

                    TParseProgressCSV ParserAbilityArea = new TParseProgressCSV(
                        TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "pt_ability_area.d.gz",
                        AbilityArea.grpTableField.Count);

                    StringCollection AbilityAreaColumnNames = GetColumnNames(AbilityArea);

                    while (true)
                    {
                        string[] OldAbilityRow = ParserAbilityArea.ReadNextRow();

                        if (OldAbilityRow == null)
                        {
                            break;
                        }

                        if (GetValue(AbilityAreaColumnNames, OldAbilityRow, "pt_ability_area_name_c") == AbilityAreaName)
                        {
                            Description = GetValue(AbilityAreaColumnNames, OldAbilityRow, "pt_ability_area_descr_c");
                            break;
                        }
                    }

                    // map old ability level to new skill level
                    int AbilityLevel = Convert.ToInt32(GetValue(PersonAbilityColumnNames, OldRow, "pt_ability_level_i"));
                    SkillLevel = 99; // remains 99 if unknown

                    if ((AbilityLevel >= 0) && (AbilityLevel <= 3))
                    {
                        SkillLevel = 1;
                    }
                    else if ((AbilityLevel >= 4) && (AbilityLevel <= 5))
                    {
                        SkillLevel = 2;
                    }
                    else if ((AbilityLevel >= 6) && (AbilityLevel <= 7))
                    {
                        SkillLevel = 3;
                    }
                    else if ((AbilityLevel >= 8) && (AbilityLevel <= 10))
                    {
                        SkillLevel = 4;
                    }

                    string Comment = GetValue(PersonAbilityColumnNames, OldRow, "pm_comment_c");

                    if (SkillCategory == "OTHER")
                    {
                        Comment += " Copied from Petra (Ability Area Name: " + AbilityAreaName;
                    }

                    SetValue(AColumnNames, ref ANewRow, "pm_person_skill_key_i", RowCounter.ToString());
                    SetValue(AColumnNames, ref ANewRow, "p_partner_key_n", GetValue(PersonAbilityColumnNames, OldRow, "p_partner_key_n"));
                    SetValue(AColumnNames, ref ANewRow, "pm_skill_category_code_c", SkillCategory);
                    SetValue(AColumnNames, ref ANewRow, "pm_description_english_c", Description);
                    SetValue(AColumnNames, ref ANewRow, "pm_skill_level_i", SkillLevel.ToString());
                    SetValue(AColumnNames, ref ANewRow, "pm_years_of_experience_i",
                        GetValue(PersonAbilityColumnNames, OldRow, "pm_years_of_experience_i"));
                    SetValue(AColumnNames, ref ANewRow, "pm_years_of_experience_as_of_d",
                        GetValue(PersonAbilityColumnNames, OldRow, "pm_years_of_experience_as_of_d"));
                    SetValue(AColumnNames, ref ANewRow, "pm_professional_skill_l", "0");
                    SetValue(AColumnNames, ref ANewRow, "pm_comment_c", Comment);
                    SetValue(AColumnNames, ref ANewRow, "s_date_created_d", GetValue(PersonAbilityColumnNames, OldRow, "s_date_created_d"));
                    SetValue(AColumnNames, ref ANewRow, "s_created_by_c", GetValue(PersonAbilityColumnNames, OldRow, "s_created_by_c"));
                    SetValue(AColumnNames, ref ANewRow, "s_date_modified_d", GetValue(PersonAbilityColumnNames, OldRow, "s_date_modified_d"));
                    SetValue(AColumnNames, ref ANewRow, "s_modified_by_c", GetValue(PersonAbilityColumnNames, OldRow, "s_modified_by_c"));
                    SetValue(AColumnNames, ref ANewRow, "s_modification_id_t", GetValue(PersonAbilityColumnNames, OldRow, "s_modification_id_t"));

                    AWriter.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("BEGIN; " + "COPY " + ATableName + " FROM stdin;");
                    AWriterTest.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("\\.");
                    AWriterTest.WriteLine("ROLLBACK;");
                    RowCounter++;
                }

                //*** Copy from pm_person_qualification ***//

                // load the file pm_person_qualification.d.gz
                TTable PersonQualification = TDumpProgressToPostgresql.GetStoreOld().GetTable("pm_person_qualification");

                TParseProgressCSV ParserQualification = new TParseProgressCSV(
                    TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "pm_person_qualification.d.gz",
                    PersonQualification.grpTableField.Count);

                StringCollection PersonQualificationColumnNames = GetColumnNames(PersonQualification);

                while (true)
                {
                    string[] OldRow = ParserQualification.ReadNextRow();

                    if (OldRow == null)
                    {
                        break;
                    }

                    // map old qualification_area_name to new skill category
                    String QualificationAreaName = GetValue(PersonAbilityColumnNames, OldRow, "pt_ability_area_name_c");
                    SkillCategory = "OTHER";
                    Description = "";

                    foreach (string Category in SkillCategories)
                    {
                        if (QualificationAreaName.Substring(0, 3) == Category.Substring(0, 3))
                        {
                            SkillCategory = Category;
                            break;
                        }
                    }

                    // copy old qualification_area description from pt_qualification_area to new description

                    // load the file pt_ability_area.d.gz so that we can access the values for each person
                    TTable QualificationArea = TDumpProgressToPostgresql.GetStoreOld().GetTable("pt_qualification_area");

                    TParseProgressCSV ParserQualificationArea = new TParseProgressCSV(
                        TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "pt_qualification_area.d.gz",
                        QualificationArea.grpTableField.Count);

                    StringCollection QualificationAreaColumnNames = GetColumnNames(QualificationArea);

                    while (true)
                    {
                        string[] OldQualificationRow = ParserQualificationArea.ReadNextRow();

                        if (OldQualificationRow == null)
                        {
                            break;
                        }

                        if (GetValue(QualificationAreaColumnNames, OldQualificationRow, "pt_qualification_area_name_c") == QualificationAreaName)
                        {
                            Description = GetValue(QualificationAreaColumnNames, OldQualificationRow, "pt_qualification_area_descr_c");
                            break;
                        }
                    }

                    // map old Qualification level to new skill level
                    int QualificationLevel = Convert.ToInt32(GetValue(PersonQualificationColumnNames, OldRow, "pt_qualification_level_i"));
                    SkillLevel = 99; // remains 99 if unknown

                    if ((QualificationLevel >= 0) && (QualificationLevel <= 3))
                    {
                        SkillLevel = 1;
                    }
                    else if ((QualificationLevel >= 4) && (QualificationLevel <= 5))
                    {
                        SkillLevel = 2;
                    }
                    else if ((QualificationLevel >= 6) && (QualificationLevel <= 7))
                    {
                        SkillLevel = 3;
                    }
                    else if ((QualificationLevel >= 8) && (QualificationLevel <= 10))
                    {
                        SkillLevel = 4;
                    }

                    string Comment = GetValue(PersonQualificationColumnNames, OldRow, "pm_comment_c");

                    if (SkillCategory == "OTHER")
                    {
                        Comment += " Copied from Petra (Qualification Area Name: " + QualificationAreaName;
                    }

                    // copy old qualification level description from pt_qualification_area to new comment

                    // load the file pt_qualification_level.d.gz so that we can access the values for each person
                    TTable QualificationLevelTable = TDumpProgressToPostgresql.GetStoreOld().GetTable("pt_qualification_level");

                    TParseProgressCSV ParserQualificationLevel = new TParseProgressCSV(
                        TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "pt_qualification_level.d.gz",
                        QualificationLevelTable.grpTableField.Count);

                    StringCollection QualificationLevelColumnNames = GetColumnNames(QualificationLevelTable);

                    while (true)
                    {
                        string[] OldQualificationLevelRow = ParserQualificationLevel.ReadNextRow();

                        if (OldQualificationLevelRow == null)
                        {
                            break;
                        }

                        if (GetValue(QualificationLevelColumnNames, OldQualificationLevelRow,
                                "pt_qualification_level_i") == QualificationLevel.ToString())
                        {
                            Comment += " Qualification Level from Petra: " + QualificationLevel + " - " +
                                       GetValue(QualificationLevelColumnNames, OldQualificationLevelRow, "pt_qualification_level_descr_c");
                            break;
                        }
                    }

                    SetValue(AColumnNames, ref ANewRow, "pm_person_skill_key_i", RowCounter.ToString());
                    SetValue(AColumnNames, ref ANewRow, "p_partner_key_n", GetValue(PersonQualificationColumnNames, OldRow, "p_partner_key_n"));
                    SetValue(AColumnNames, ref ANewRow, "pm_skill_category_code_c", SkillCategory);
                    SetValue(AColumnNames, ref ANewRow, "pm_description_english_c", Description);
                    SetValue(AColumnNames, ref ANewRow, "pm_skill_level_i", SkillLevel.ToString());
                    SetValue(AColumnNames, ref ANewRow, "pm_years_of_experience_i",
                        GetValue(PersonQualificationColumnNames, OldRow, "pm_years_of_experience_i"));
                    SetValue(AColumnNames, ref ANewRow, "pm_years_of_experience_as_of_d",
                        GetValue(PersonQualificationColumnNames, OldRow, "pm_years_of_experience_as_of_d"));
                    SetValue(AColumnNames, ref ANewRow, "pm_professional_skill_l", "1");
                    SetValue(AColumnNames, ref ANewRow, "pm_comment_c", Comment);
                    SetValue(AColumnNames, ref ANewRow, "s_date_created_d", GetValue(PersonAbilityColumnNames, OldRow, "s_date_created_d"));
                    SetValue(AColumnNames, ref ANewRow, "s_created_by_c", GetValue(PersonAbilityColumnNames, OldRow, "s_created_by_c"));
                    SetValue(AColumnNames, ref ANewRow, "s_date_modified_d", GetValue(PersonAbilityColumnNames, OldRow, "s_date_modified_d"));
                    SetValue(AColumnNames, ref ANewRow, "s_modified_by_c", GetValue(PersonAbilityColumnNames, OldRow, "s_modified_by_c"));
                    SetValue(AColumnNames, ref ANewRow, "s_modification_id_t", GetValue(PersonAbilityColumnNames, OldRow, "s_modification_id_t"));

                    AWriter.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("BEGIN; " + "COPY " + ATableName + " FROM stdin;");
                    AWriterTest.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("\\.");
                    AWriterTest.WriteLine("ROLLBACK;");
                    RowCounter++;
                }
            }

            // This existing and populated table's data is completely changed. We do not want to import any of it's contents.
            // This data is also in the basedata
            if (ATableName == "pt_language_level")
            {
                // Default language levels data
                string[] LanguageLevels = new string[] {
                    "1", "2", "3", "99"
                };
                string[] LanguageLevelDescriptions = new string[] {
                    "BASIC", "INTERMEDIATE", "ADVANCED", "UNKNOWN"
                };
                string[] LanguageComments = new string[] {
                    "Uses a narrow range of language, adequate for basic needs and simple situations. " +
                    "Does not really have sufficient language to cope with normal day-to-day, real-life communication, " +
                    "but basic communication is possible with adequate opportunities for assistance.",
                    "Uses the language independently and effectively in familiar situations. Rather frequent lapses in accuracy, fluency, " +
                    "appropriateness and organisation, but usually succeeds in communication and comprehending the general message.",
                    "Uses a full range of language with proficiency approaching that in the learner's own mother tongue. " +
                    "Copes well even with demanding and complex language situations. Makes minor lapses in accuracy, fluency, " +
                    "appropriateness and organisation which do not affect communication.",
                    "Speaks the language to some extent, level unknown."
                };

                for (int i = 0; i < 4; i++)
                {
                    SetValue(AColumnNames, ref ANewRow, "pt_language_level_i", LanguageLevels[i]);
                    SetValue(AColumnNames, ref ANewRow, "pt_language_level_descr_c", LanguageLevelDescriptions[i]);
                    SetValue(AColumnNames, ref ANewRow, "pt_unassignable_flag_l", "0");
                    SetValue(AColumnNames, ref ANewRow, "pt_unassignable_date_d", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "pt_deletable_flag_l", "0");
                    SetValue(AColumnNames, ref ANewRow, "pt_language_comment_c", LanguageComments[i]);
                    SetValue(AColumnNames, ref ANewRow, "s_date_created_d", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "s_created_by_c", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "s_date_modified_d", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "s_modified_by_c", "\\N");
                    SetValue(AColumnNames, ref ANewRow, "s_modification_id_t", "\\N");

                    AWriter.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("BEGIN; " + "COPY " + ATableName + " FROM stdin;");
                    AWriterTest.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("\\.");
                    AWriterTest.WriteLine("ROLLBACK;");
                    RowCounter++;
                }
            }

            return RowCounter;
        }
    }
}