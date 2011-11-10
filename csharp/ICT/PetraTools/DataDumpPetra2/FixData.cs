//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Collections.Specialized;
using System.Collections.Generic;
using Ict.Common;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// we need to fix some data content, otherwise loading into Postgresql would violate foreign key constraints or other constraints.
    /// do not fix the old Petra 2.x database, since it sometimes depends on these things.
    /// </summary>
    public class TFixData
    {
        private static void SetValue(StringCollection AColumnNames,
            ref string[] ACurrentRow,
            string AColumnName,
            string ANewValue)
        {
            if (AColumnNames.IndexOf(AColumnName) == -1)
            {
                throw new Exception("TFixData.SetValue: Problem with unknown column name " + AColumnName);
            }

            ACurrentRow[AColumnNames.IndexOf(AColumnName)] = ANewValue;
        }

        private static string GetValue(StringCollection AColumnNames,
            string[] ACurrentRow,
            string AColumnName)
        {
            if (AColumnNames.IndexOf(AColumnName) == -1)
            {
                throw new Exception("TFixData.GetValue: Problem with unknown column name " + AColumnName);
            }

            return ACurrentRow[AColumnNames.IndexOf(AColumnName)];
        }

        /// <summary>
        /// fix data that would cause problems for PostgreSQL constraints
        /// </summary>
        /// <param name="ATableName"></param>
        /// <param name="AColumnNames"></param>
        /// <param name="ACSVLines"></param>
        public static void FixData(string ATableName, StringCollection AColumnNames, ref List <string[]>ACSVLines)
        {
            // update pub.a_account_property set a_property_value_c = 'true' where a_property_code_c = 'Bank Account';
            if (ATableName == "a_account_property")
            {
                for (Int32 counter = 0; counter < ACSVLines.Count; counter++)
                {
                    string[] CurrentRow = ACSVLines[counter];

                    if (GetValue(AColumnNames, CurrentRow, "a_property_code_c") == "Bank Account")
                    {
                        SetValue(AColumnNames, ref CurrentRow, "a_property_value_c", "true");
                    }
                }
            }

            // a_email_destination.a_conditional_value_c is sometimes null, but it is part of the primary key
            if (ATableName == "a_email_destination")
            {
                for (Int32 counter = 0; counter < ACSVLines.Count; counter++)
                {
                    string[] CurrentRow = ACSVLines[counter];
                    string ConditionalValue = GetValue(AColumnNames, CurrentRow, "a_conditional_value_c");

                    if ((ConditionalValue == "?") || (ConditionalValue.Length == 0))
                    {
                        SetValue(AColumnNames, ref CurrentRow, "a_conditional_value_c", "NOT SET");
                    }
                }
            }

            // s_user_group contains some SQL_* users, which are not part of the s_user table
            if (ATableName == "s_user_group")
            {
                for (Int32 counter = 0; counter < ACSVLines.Count; counter++)
                {
                    string[] CurrentRow = ACSVLines[counter];

                    if (GetValue(AColumnNames, CurrentRow, "s_user_id_c").StartsWith("SQL_"))
                    {
                        ACSVLines.RemoveAt(counter);
                        counter--;
                    }
                }
            }

            // there is a space in front of the code, which causes a duplicate primary key
            if (ATableName == "p_type")
            {
                bool duplicateExists = false;

                for (Int32 counter = 0; counter < ACSVLines.Count; counter++)
                {
                    string[] CurrentRow = ACSVLines[counter];

                    if (GetValue(AColumnNames, CurrentRow, "p_type_code_c") == "STAFF")
                    {
                        duplicateExists = true;
                    }
                }

                for (Int32 counter = 0; duplicateExists && counter < ACSVLines.Count; counter++)
                {
                    string[] CurrentRow = ACSVLines[counter];

                    if (GetValue(AColumnNames, CurrentRow, "p_type_code_c") == " STAFF")
                    {
                        ACSVLines.RemoveAt(counter);
                        counter--;
                    }
                }
            }

            // there is a space in front of the code, which causes a duplicate primary key
            if (ATableName == "p_reason_subscription_given")
            {
                for (Int32 counter = 0; counter < ACSVLines.Count; counter++)
                {
                    string[] CurrentRow = ACSVLines[counter];

                    if (GetValue(AColumnNames, CurrentRow, "p_code_c") == " FREE")
                    {
                        ACSVLines.RemoveAt(counter);
                        counter--;
                    }
                }
            }

            // fix foreign key, remove space
            if (ATableName == "p_subscription")
            {
                for (Int32 counter = 0; counter < ACSVLines.Count; counter++)
                {
                    string[] CurrentRow = ACSVLines[counter];

                    if (GetValue(AColumnNames, CurrentRow, "p_reason_subs_given_code_c") == " FREE")
                    {
                        SetValue(AColumnNames, ref CurrentRow, "p_reason_subs_given_code_c", "FREE");
                    }
                }
            }

            // pm_person_language, language code cannot be null, should be 99
            if (ATableName == "pm_person_language")
            {
                for (Int32 counter = 0; counter < ACSVLines.Count; counter++)
                {
                    string[] CurrentRow = ACSVLines[counter];
                    string val = GetValue(AColumnNames, CurrentRow, "p_language_code_c");

                    if ((val.Length == 0) || (val == "\\N"))
                    {
                        SetValue(AColumnNames, ref CurrentRow, "p_language_code_c", "99");
                    }
                }
            }

            // p_partner_contact, method of contact cannot be null, should be UNKNOWN
            if (ATableName == "p_partner_contact")
            {
                for (Int32 counter = 0; counter < ACSVLines.Count; counter++)
                {
                    string[] CurrentRow = ACSVLines[counter];
                    string val = GetValue(AColumnNames, CurrentRow, "p_contact_code_c");

                    if ((val.Length == 0) || (val == "\\N"))
                    {
                        SetValue(AColumnNames, ref CurrentRow, "p_contact_code_c", "UNKNOWN");
                    }
                }
            }

            // wrong gift batch status, need to have case sensitive status
            if (ATableName == "a_gift_batch")
            {
                for (Int32 counter = 0; counter < ACSVLines.Count; counter++)
                {
                    string[] CurrentRow = ACSVLines[counter];
                    string val = GetValue(AColumnNames, CurrentRow, "a_batch_status_c");

                    if (val == "posted")
                    {
                        SetValue(AColumnNames, ref CurrentRow, "a_batch_status_c", "Posted");
                    }
                }
            }

            // bank code has too many characters, remove spaces
            if (ATableName == "p_bank")
            {
                for (Int32 counter = 0; counter < ACSVLines.Count; counter++)
                {
                    string[] CurrentRow = ACSVLines[counter];
                    string val = GetValue(AColumnNames, CurrentRow, "p_branch_code_c");

                    if (val.Length > 20)
                    {
                        SetValue(AColumnNames, ref CurrentRow, "p_branch_code_c", val.Replace(" ", ""));
                    }
                }
            }

            // if target field is null or 0, use the home office partner key
            if (ATableName == "pm_staff_data")
            {
                for (Int32 counter = 0; counter < ACSVLines.Count; counter++)
                {
                    string[] CurrentRow = ACSVLines[counter];
                    string val = GetValue(AColumnNames, CurrentRow, "pm_receiving_field_n");

                    if ((val == "0") || (val.Length == 0) || (val == "\\N"))
                    {
                        SetValue(AColumnNames, ref CurrentRow, "pm_receiving_field_n",
                            GetValue(AColumnNames, CurrentRow, "pm_home_office_n"));
                    }
                }
            }

            // pm_st_basic_outreach_id_c cannot be null
            if (ATableName == "pm_short_term_application")
            {
                for (Int32 counter = 0; counter < ACSVLines.Count; counter++)
                {
                    string[] CurrentRow = ACSVLines[counter];
                    string val = GetValue(AColumnNames, CurrentRow, "pm_st_basic_outreach_id_c");

                    if ((val == "0") || (val.Length == 0) || (val == "\\N"))
                    {
                        SetValue(AColumnNames, ref CurrentRow, "pm_st_basic_outreach_id_c", "INVALID");
                    }

                    val = GetValue(AColumnNames, CurrentRow, "pm_st_field_charged_n");

                    if ((val == "0") || (val.Length == 0) || (val == "\\N"))
                    {
                        SetValue(AColumnNames, ref CurrentRow, "pm_st_field_charged_n",
                            GetValue(AColumnNames, CurrentRow, "pm_registration_office_n"));
                    }
                }
            }
        }
    }
}