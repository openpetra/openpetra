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

            // pm_person_language, language code cannot be null, should be 99
            if (ATableName == "pm_person_language")
            {
                string val = GetValue(AColumnNames, ANewRow, "p_language_code_c");

                if ((val.Length == 0) || (val == "\\N"))
                {
                    SetValue(AColumnNames, ref ANewRow, "p_language_code_c", "99");
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
                return TFinanceGeneralLedgerUpgrader.FixABudgetType(AColumnNames, ref ANewRow);
            }

            if (ATableName == "a_account")
            {
                return TFinanceGeneralLedgerUpgrader.FixABudgetType(AColumnNames, ref ANewRow);
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
                // in Petra 2.x, there never has been a record in this table.
                // so if there is a budget, we need to create a revision 0 for each year

                // load the file a_budget.d.gz so that we can access the values for each person
                TTable budgetTableOld = TDumpProgressToPostgresql.GetStoreOld().GetTable("a_budget");

                TParseProgressCSV Parser = new TParseProgressCSV(
                    TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "a_budget.d.gz",
                    budgetTableOld.grpTableField.Count);

                StringCollection BudgetColumnNames = GetColumnNames(budgetTableOld);

                List <string>Revisions = new List <string>();

                string LedgerNumber = string.Empty;
                string YearNumber = string.Empty;
                SetValue(AColumnNames, ref ANewRow, "a_revision_i", "0");
                SetValue(AColumnNames, ref ANewRow, "a_description_c", "default");
                SetValue(AColumnNames, ref ANewRow, "s_date_created_d", "\\N");
                SetValue(AColumnNames, ref ANewRow, "s_created_by_c", "\\N");
                SetValue(AColumnNames, ref ANewRow, "s_date_modified_d", "\\N");
                SetValue(AColumnNames, ref ANewRow, "s_modified_by_c", "\\N");
                SetValue(AColumnNames, ref ANewRow, "s_modification_id_t", "\\N");

                while (true)
                {
                    string[] OldRow = Parser.ReadNextRow();

                    if (OldRow == null)
                    {
                        break;
                    }

                    LedgerNumber = GetValue(BudgetColumnNames, OldRow, "a_ledger_number_i");
                    YearNumber = GetValue(BudgetColumnNames, OldRow, "a_year_i");

                    if (!Revisions.Contains(LedgerNumber + "_" + YearNumber))
                    {
                        SetValue(AColumnNames, ref ANewRow, "a_ledger_number_i", LedgerNumber);
                        SetValue(AColumnNames, ref ANewRow, "a_year_i", YearNumber);

                        AWriter.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                        AWriterTest.WriteLine("BEGIN; " + "COPY " + ATableName + " FROM stdin;");
                        AWriterTest.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                        AWriterTest.WriteLine("\\.");
                        AWriterTest.WriteLine("ROLLBACK;");
                        RowCounter++;

                        Revisions.Add(LedgerNumber + "_" + YearNumber);
                    }
                }
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

            return RowCounter;
        }
    }
}