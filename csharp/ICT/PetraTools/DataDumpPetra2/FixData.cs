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

namespace DumpPetra2xToOpenPetra
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
        ACurrentRow[AColumnNames.IndexOf(AColumnName)] = ANewValue;
    }

    private static string GetValue(StringCollection AColumnNames,
        string[] ACurrentRow,
        string AColumnName)
    {
        return ACurrentRow[AColumnNames.IndexOf(AColumnName)];
    }

    public static void FixData(string ATableName, StringCollection AColumnNames, ref List <string[]>ACSVLines)
    {
        /// update pub.a_account_property set a_property_value_c = 'true' where a_property_code_c = 'Bank Account';
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

        /// a_email_destination.a_conditional_value_c is sometimes null, but it is part of the primary key
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

        // TODO:
        // some entries in a_gift_batch.a_batch_status_c have "posted" (lower letter p)
        // These entries must be converted to "Posted" (capital letter P)

        // TODO
#if TODO
        if ((oldField.strName == "pm_target_field_office_n")
            || (oldField.strName == "pm_target_field_n")
            || (oldField.strName == "p_om_field_key_n"))
        {
            sw.WriteLine("        ForceNullDecimal(" + oldField.strName + ") /* new name " + newField.strName + " */");
        }
        else if (oldField.strName.EndsWith("_code_c")
                 || (oldField.strName == "pm_st_recruit_missions_c"))
        {
            if (!oldField.bNotNull)
            {
                sw.WriteLine("        ForceNull(ToUpperCaseAndTrim(" + oldField.strName + "))  /* new name " + newField.strName + " */");
            }
            else
            {
                sw.WriteLine("        ToUpperCaseAndTrim(" + oldField.strName + ") /* new name " + newField.strName + " */");
            }
        }
        else
#endif
    }
}
}