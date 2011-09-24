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
using System.IO;
using System.Text;
using System.Collections;
using Ict.Common;
using Ict.Tools.DBXML;

namespace DumpPetra2xToOpenPetra
{
/// <summary>
/// Create a Progress .p file for dumping all data to CSV files
/// </summary>
public class TCreateFulldumpProgressCode
{
    private TDataDefinitionStore storeOld;
    private TDataDefinitionStore storeNew;

    private void DumpTable(ref StreamWriter sw, TTable newTable)
    {
        string oldTableName = DataDefinitionDiff.GetOldTableName(newTable.strName);

        TTable oldTable = storeOld.GetTable(oldTableName);

        // if this is a new table in OpenPetra, do not dump anything. the table will be empty in OpenPetra
        if (oldTable == null)
        {
            return;
        }

        // check for parameter, only dump table if parameter empty or equals the table name
        sw.WriteLine("IF pv_tablename_c EQ \"\" OR pv_tablename_c EQ \"" + newTable.strName + "\" THEN DO:");
        sw.WriteLine("OUTPUT STREAM OutFile TO fulldump/" + newTable.strName + ".d.");
        sw.WriteLine("REPEAT FOR " + oldTable.strName + ':');

        sw.WriteLine("    FIND NEXT " + oldTable.strName + " NO-LOCK.");
        sw.WriteLine("    EXPORT STREAM OutFile DELIMITER \",\" ");

        foreach (TTableField newField in newTable.grpTableField.List)
        {
            TTableField oldField = null;

            string oldname = "";

            oldField = oldTable.GetField(newField.strName);

            if ((oldField == null) && (DataDefinitionDiff.GetNewFieldName(newTable.strName, ref oldname, ref newField.strName)))
            {
                oldField = oldTable.GetField(oldname);
            }

            if (oldField != null)
            {
                if (oldField.strName != newField.strName)
                {
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
                    {
                        sw.WriteLine("        " + oldField.strName + " /* new name " + newField.strName + " */");
                    }
                }
                else
                {
                    if ((oldField.strName == "s_created_by_c")
                        || (oldField.strName == "s_modified_by_c")
                        || (oldField.strName == "p_owner_c")
                        || (oldField.strName == "s_user_id_c")
                        || (oldField.strName == "p_relation_name_c")
                        || oldField.strName.EndsWith("_code_c"))
                    {
                        if (!oldField.bNotNull)
                        {
                            sw.WriteLine("        ForceNull(ToUpperCaseAndTrim(" + oldField.strName + "))");
                        }
                        else
                        {
                            sw.WriteLine("        ToUpperCaseAndTrim(" + oldField.strName + ")");
                        }
                    }
                    else if (!oldField.bNotNull
                             && ((oldField.strName == "p_field_key_n")
                                 || (oldField.strName == "pm_gen_app_poss_srv_unit_key_n")
                                 || (oldField.strName == "pm_st_field_charged_n")
                                 || (oldField.strName == "pm_st_current_field_n")
                                 || (oldField.strName == "pm_st_option2_n")
                                 || (oldField.strName == "pm_st_option1_n")
                                 || (oldField.strName == "pm_st_confirmed_option_n")
                                 || (oldField.strName == "pm_office_recruited_by_n")
                                 || (oldField.strName == "a_key_ministry_key_n")
                                 || (oldField.strName == "pm_placement_partner_key_n")
                                 ))
                    {
                        sw.WriteLine("        ForceNullDecimal(" + oldField.strName + ")");
                    }
                    else if ((oldField.strType.ToUpper() == "VARCHAR") && !oldField.bNotNull)
                    {
                        sw.WriteLine("        ForceNull(" + oldField.strName + ")");
                    }
                    else if (oldField.strType.ToUpper() == "BIT")
                    {
                        sw.WriteLine("        WriteLogical(" + oldField.strName + ")");
                    }
                    else if (oldField.strType.ToUpper() == "DATE")
                    {
                        sw.WriteLine("        WriteDate(" + oldField.strName + ")");
                    }
                    else
                    {
                        sw.WriteLine("        " + oldField.strName);
                    }
                }
            }
            else
            {
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

                sw.WriteLine("        " + defaultValue + " /* new field " + newField.strName + " */");
            }
        }

        sw.WriteLine(".");
        sw.WriteLine("END.");
        sw.WriteLine("END.");
    }

    private void DumpSequences(ref StreamWriter AProgressWriter)
    {
        AProgressWriter.WriteLine("IF pv_tablename_c EQ \"sequences\" THEN DO:");
        AProgressWriter.WriteLine("OUTPUT STREAM OutFile TO fulldump/initialiseSequences.sql.");
        ArrayList newSequences = storeNew.GetSequences();
        ArrayList oldSequences = storeOld.GetSequences();

        foreach (TSequence newSequence in newSequences)
        {
            Boolean isNewSequence = true;

            foreach (TSequence oldSequence in oldSequences)
            {
                if (oldSequence.strName == newSequence.strName)
                {
                    isNewSequence = false;
                }
            }

            if (isNewSequence)
            {
                AProgressWriter.WriteLine(
                    "PUT STREAM OutFile UNFORMATTED \"SELECT pg_catalog.setval('" + newSequence.strName + "', " + newSequence.iMinVal.ToString() +
                    ", false);\".");
            }
            else
            {
                AProgressWriter.WriteLine(
                    "PUT STREAM OutFile UNFORMATTED \"SELECT pg_catalog.setval('" + newSequence.strName + "', \" + STRING(CURRENT-VALUE(" +
                    newSequence.strName + ")) + \", false);\".");
            }
        }

        AProgressWriter.WriteLine("END.");
    }

    /// <summary>
    /// create a Progress .p program for dumping the Progress data to CSV files
    /// </summary>
    /// <param name="APetraOldPath"></param>
    /// <param name="APetraNewPath"></param>
    /// <param name="AOutputFile"></param>
    public void GenerateFulldumpCode(String APetraOldPath, String APetraNewPath, String AOutputFile)
    {
        /* existing table: check for new fields, go by the new order */
        /* new fields are written using the default value or NULL */
        TDataDefinitionParser parserOld = new TDataDefinitionParser(APetraOldPath);

        storeOld = new TDataDefinitionStore();
        TDataDefinitionParser parserNew = new TDataDefinitionParser(APetraNewPath, false);
        storeNew = new TDataDefinitionStore();

        System.Console.WriteLine("Reading 2.x xml file {0}...", APetraOldPath);

        if (!parserOld.ParseDocument(ref storeOld, false, true))
        {
            return;
        }

        System.Console.WriteLine("Reading OpenPetra xml file {0}...", APetraNewPath);

        if (!parserNew.ParseDocument(ref storeNew, false, true))
        {
            return;
        }

        DataDefinitionDiff.newVersion = "3.0";
        TTable.GEnabledLoggingMissingFields = false;

        System.Console.WriteLine("Writing file to {0}...", AOutputFile);
        StreamWriter progressWriter = new StreamWriter(AOutputFile);

        // print file header
        progressWriter.WriteLine("/* Generated with DumpPetra2xToOpenPetra.exe */");

        System.Resources.ResourceManager RM = new System.Resources.ResourceManager("DumpPetra2xToOpenPetra.templateCode",
            System.Reflection.Assembly.GetExecutingAssembly());
        progressWriter.WriteLine(RM.GetString("progress_dump_functions"));

        progressWriter.WriteLine();

        ArrayList newTables = storeNew.GetTables();

        foreach (TTable newTable in newTables)
        {
            DumpTable(ref progressWriter, newTable);
        }

        DumpSequences(ref progressWriter);
        progressWriter.WriteLine();
        progressWriter.Close();
        System.Console.WriteLine("Success: file written: {0}", AOutputFile);
        TTable.GEnabledLoggingMissingFields = true;
    }
}
}