//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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

namespace Ict.Tools.DBXML
{
    /// <summary>
    /// this class knows all the differences
    /// between the current database structure and the previous version
    /// This should perhaps be implemented in XML rather than hardcoded here
    /// </summary>
    public class DataDefinitionDiff
    {
        /// <summary>
        /// the current version
        /// </summary>
        public static string newVersion = "3.0";

        /// <summary>
        /// test if the name of the table has been renamed
        /// </summary>
        /// <param name="oldname">the name of the table in question</param>
        /// <returns>returns the same name or the new name of the table</returns>
        public static string GetNewTableName(String oldname)
        {
            string newname = oldname;

            // if (oldname = 'um_job_ability') then
            // newname := 'um_job_requirement';
            return newname;
        }

        /// <summary>
        /// test if the table has been renamed, and return the old name or the current
        /// </summary>
        /// <param name="newname">the table to be tested</param>
        /// <returns>the old or the current name</returns>
        public static string GetOldTableName(String newname)
        {
            string oldname = newname;

            // if (newname = 'um_job_requirement') then
            // oldname := 'um_job_ability';
            return oldname;
        }

        static TRenamedField[] NewFieldNames = null;

        /// <summary>
        /// see if the column has been renamed
        /// </summary>
        /// <param name="tablename">the table (if table name also has been renamed, need entries for both below)</param>
        /// <param name="oldname">the old field name (returns the old name)</param>
        /// <param name="newname">the new name (returns the new name)</param>
        /// <returns>true if the names are different, the field has been renamed</returns>
        public static Boolean GetNewFieldName(String tablename, ref String oldname, ref String newname)
        {
            if (oldname == "")
            {
                oldname = newname;
            }
            else
            {
                newname = oldname;
            }

            // for 2.1 to 2.2:
            if (newVersion == "2.2")
            {
                // for example:
                // NewFieldNames = new TRenamedField[1];
                // NewFieldNames[0] = new TRenamedField("a_budget_period", "a_budget_this_year_n", "a_budget_base_n");
                // if the table name has been renamed as well, need a TRenamedField for both old and new table
            }

            foreach (TRenamedField row in NewFieldNames)
            {
                if (row.TableName == tablename)
                {
                    if (newname == row.NewFieldName)
                    {
                        oldname = row.OldFieldName;
                    }
                    else if (oldname == row.OldFieldName)
                    {
                        newname = row.NewFieldName;
                    }
                }
            }

            return newname != oldname;
        }
    }

    /// <summary>
    /// easy structure for tracking renamed column names
    /// </summary>
    public class TRenamedField
    {
        /// <summary>
        /// the table that the column belongs to
        /// </summary>
        public string TableName;

        /// <summary>
        /// the old name of the column
        /// </summary>
        public String OldFieldName;

        /// <summary>
        /// the new name of the column
        /// </summary>
        public String NewFieldName;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ATableName">the table that the column belongs to</param>
        /// <param name="AOldFieldName">the old name of the column</param>
        /// <param name="ANewFieldName">the new name of the column</param>
        public TRenamedField(string ATableName, string AOldFieldName, string ANewFieldName)
        {
            TableName = ATableName;
            OldFieldName = AOldFieldName;
            NewFieldName = ANewFieldName;
        }
    }
}