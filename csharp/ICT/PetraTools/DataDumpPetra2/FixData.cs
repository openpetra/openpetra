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
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Ict.Common;
using Ict.Tools.DBXML;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// we need to fix some data content, otherwise loading into Postgresql would violate foreign key constraints or other constraints.
    /// do not fix the old Petra 2.x database, since it sometimes depends on these things.
    /// </summary>
    public class TFixData
    {
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

        private static StringCollection GetColumnNames(TTable ATable)
        {
            StringCollection ColumnNames = new StringCollection();

            foreach (TTableField field in ATable.grpTableField)
            {
                ColumnNames.Add(field.strName);
            }

            return ColumnNames;
        }

        private static string FixValue(string AValue, TTableField AOldField)
        {
            if ((AOldField.strName == "s_created_by_c")
                || (AOldField.strName == "s_modified_by_c")
                || (AOldField.strName == "p_owner_c")
                || (AOldField.strName == "s_user_id_c")
                || (AOldField.strName == "p_relation_name_c")
                || AOldField.strName.EndsWith("_code_c"))
            {
                AValue = AValue.Trim().ToUpper();

                if (!AOldField.bNotNull)
                {
                    if (AValue.Length == 0)
                    {
                        AValue = "\\N";
                    }
                }
            }
            else if (!AOldField.bNotNull
                     && ((AOldField.strName == "p_field_key_n")
                         || (AOldField.strName == "pm_gen_app_poss_srv_unit_key_n")
                         || (AOldField.strName == "pm_st_field_charged_n")
                         || (AOldField.strName == "pm_st_current_field_n")
                         || (AOldField.strName == "pm_st_option2_n")
                         || (AOldField.strName == "pm_st_option1_n")
                         || (AOldField.strName == "pm_st_confirmed_option_n")
                         || (AOldField.strName == "pm_office_recruited_by_n")
                         || (AOldField.strName == "a_key_ministry_key_n")
                         || (AOldField.strName == "pm_placement_partner_key_n")
                         ))
            {
                if (AValue == "0")
                {
                    AValue = "\\N";
                }
            }
            else if ((AValue.Length == 0) && AOldField.strType.Equals("VARCHAR", StringComparison.OrdinalIgnoreCase) && !AOldField.bNotNull)
            {
                AValue = "\\N";
            }
            else if (AOldField.strType.Equals("BIT", StringComparison.OrdinalIgnoreCase))
            {
                AValue = (AValue == "yes") ? "1" : "0";
            }
            else if (AOldField.strType.Equals("DATE", StringComparison.OrdinalIgnoreCase))
            {
                if ((AValue.Length > 0) && (AValue != "\\N"))
                {
                    if (AValue.Length != 10)
                    {
                        TLogging.Log("WARNING: Invalid date: " + AOldField.strName + " " + AValue);
                        AValue = "\\N";
                    }
                    else
                    {
                        // TODO: check for year format, or force all the same in the dump program dmy?
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
        public static int MigrateData(TParseProgressCSV AParser, StreamWriter AWriter, TTable AOldTable, TTable ANewTable)
        {
            StringCollection OldColumnNames = GetColumnNames(AOldTable);
            StringCollection NewColumnNames = GetColumnNames(ANewTable);
            int RowCounter = 0;

            SortedList <string, TTableField>MappingOfFields = new SortedList <string, TTableField>();
            SortedList <string, string>DefaultValues = new SortedList <string, string>();

            foreach (TTableField newField in ANewTable.grpTableField)
            {
                string oldname = "";

                TTableField oldField = AOldTable.GetField(newField.strName);

                if ((oldField == null) && (DataDefinitionDiff.GetNewFieldName(ANewTable.strName, ref oldname, ref newField.strName)))
                {
                    oldField = AOldTable.GetField(oldname);
                }

                MappingOfFields.Add(newField.strName, oldField);

                // prepare the default values once
                // this is a new field. insert default value
                string defaultValue = "?";

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

                DefaultValues.Add(newField.strName, defaultValue);
            }

            string[] NewRow = CreateRow(NewColumnNames);

            while (true)
            {
                string[] OldRow = AParser.ReadNextRow();

                if (OldRow == null)
                {
                    break;
                }

                foreach (TTableField newField in ANewTable.grpTableField)
                {
                    TTableField oldField = MappingOfFields[newField.strName];

                    if (oldField != null)
                    {
                        string value = GetValue(OldColumnNames, OldRow, oldField.strName);

                        value = FixValue(value, oldField);

                        SetValue(NewColumnNames, ref NewRow, newField.strName, value);
                    }
                    else
                    {
                        SetValue(NewColumnNames, ref NewRow, newField.strName, DefaultValues[newField.strName]);
                    }
                }

                if (FixData(ANewTable.strName, NewColumnNames, ref NewRow))
                {
                    RowCounter++;
                    AWriter.WriteLine(StringHelper.StrMerge(NewRow, '\t').Replace("\\\\N", "\\N").ToString());
                }
            }

            return RowCounter;
        }

        static SortedList <string, string[]>PPersonBelieverInfo = null;

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

            if (ATableName == "a_ap_anal_attrib")
            {
                return TFinanceAccountsPayableUpgrader.FixAPAnalAttrib(AColumnNames, ref ANewRow);
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
                if (GetValue(AColumnNames, ANewRow, "p_reason_subs_given_code_c") == " FREE")
                {
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
                string val = GetValue(AColumnNames, ANewRow, "pm_receiving_field_n");

                if ((val == "0") || (val.Length == 0) || (val == "\\N"))
                {
                    SetValue(AColumnNames, ref ANewRow, "pm_receiving_field_n",
                        GetValue(AColumnNames, ANewRow, "pm_home_office_n"));
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

            // pm_personal_data: move values from the p_person table for
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

            return true;
        }
    }
}