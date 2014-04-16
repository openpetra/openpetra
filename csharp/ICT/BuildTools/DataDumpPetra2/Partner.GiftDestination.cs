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
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ict.Tools.DBXML;
using Ict.Common;

namespace Ict.Tools.DataDumpPetra2
{
    /// <summary>
    /// this class helps with upgrading tables in Budget
    /// </summary>
    public class TPartnerGiftDestination : TFixData
    {
        /// <summary>
        /// Populate the empty table PPartnerGiftDestination using PmStaffData
        /// </summary>
        public static int PopulatePPartnerGiftDestination(StringCollection AColumnNames,
            ref string[] ANewRow,
            StreamWriter AWriter,
            StreamWriter AWriterTest)
        {
            List<string[]> ActiveCommitments = new List<string[]>();
            List<string[]> Persons = new List<string[]>();
            int RowCounter = 0;
            
            // default for all new records
            SetValue(AColumnNames, ref ANewRow, "p_active_l", "\\N");
            SetValue(AColumnNames, ref ANewRow, "p_default_gift_destination_l", "\\N");
            SetValue(AColumnNames, ref ANewRow, "p_partner_class_c", "\\N");
            SetValue(AColumnNames, ref ANewRow, "p_commitment_site_key_n", "\\N");
            SetValue(AColumnNames, ref ANewRow, "p_commitment_key_n", "\\N");
            SetValue(AColumnNames, ref ANewRow, "p_comment_c", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_date_created_d", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_created_by_c", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_date_modified_d", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_modified_by_c", "\\N");
            SetValue(AColumnNames, ref ANewRow, "s_modification_id_t", "\\N");
            
            // load the file pm_staff_data.d.gz
            TTable StaffDataTable = TDumpProgressToPostgresql.GetStoreOld().GetTable("pm_staff_data");

            TParseProgressCSV StaffDataParser = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "pm_staff_data.d.gz",
                StaffDataTable.grpTableField.Count);

            StringCollection StaffDataColumnNames = GetColumnNames(StaffDataTable);
            
            // find which records are currently active
            while (true)
            {
                string[] StaffDataRow = StaffDataParser.ReadNextRow();

                if (StaffDataRow == null)
                {
                    break;
                }
                
                try
                {
                    // if commitment is currently active
                    if (DateTime.Parse(GetValue(StaffDataColumnNames, StaffDataRow, "pm_start_of_commitment_d")) <= DateTime.Today
                        && (GetValue(StaffDataColumnNames, StaffDataRow, "pm_end_of_commitment_d") == "\\N"
                            || DateTime.Parse(GetValue(StaffDataColumnNames, StaffDataRow, "pm_end_of_commitment_d")) >= DateTime.Today)
                        && GetValue(StaffDataColumnNames, StaffDataRow, "pm_start_of_commitment_d") 
                            != GetValue(StaffDataColumnNames, StaffDataRow, "pm_end_of_commitment_d"))
                    {
                       ActiveCommitments.Add(StaffDataRow);
                    }
                }
                catch
                {
                    TLogging.Log("WARNING: Invalid date: " + Convert.ToDateTime(GetValue(StaffDataColumnNames, StaffDataRow, "pm_start_of_commitment_d")) +
                                " or " + DateTime.Parse(GetValue(StaffDataColumnNames, StaffDataRow, "pm_end_of_commitment_d")));
                }
            }
            
            // load the file p_person.d.gz
            TTable PersonTable = TDumpProgressToPostgresql.GetStoreOld().GetTable("p_person");

            TParseProgressCSV PersonParser = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "p_person.d.gz",
                PersonTable.grpTableField.Count);

            StringCollection PersonColumnNames = GetColumnNames(PersonTable);
            
            // add all Persons to a list
            while (true)
            {
                string[] PersonRow = PersonParser.ReadNextRow();

                if (PersonRow == null)
                {
                    break;
                }
                
                Persons.Add(PersonRow);
            }
            
            // load the file p_family.d.gz
            TTable FamilyTable = TDumpProgressToPostgresql.GetStoreOld().GetTable("p_family");

            TParseProgressCSV FamilyParser = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "p_family.d.gz",
                FamilyTable.grpTableField.Count);

            StringCollection FamilyColumnNames = GetColumnNames(FamilyTable);
            
            // read through each family
            while (true)
            {
                string[] FamilyRow = FamilyParser.ReadNextRow();

                if (FamilyRow == null)
                {
                    break;
                }
                
                // find Person partners belonging to the family
                List<string[]> PersonsInFamily = Persons.FindAll(e => GetValue(PersonColumnNames, e, "p_family_key_n") ==
                                                                 GetValue(FamilyColumnNames, FamilyRow, "p_partner_key_n"));
                
                bool CommitmentFound = false;
                
                // read through each of the Family's Persons
                foreach (string[] Person in PersonsInFamily)
                {
                    // find if the Person has a currently active commitment
                    string[] Commitment = ActiveCommitments.Find(e => GetValue(StaffDataColumnNames, e, "p_partner_key_n") ==
                                               GetValue(PersonColumnNames, Person, "p_partner_key_n"));
                        
                    // if currently active commitment exists create a new Gift Destination record
                    if (Commitment != null)
                    {
                        SetValue(AColumnNames, ref ANewRow, "p_key_i", RowCounter.ToString());
                        SetValue(AColumnNames, ref ANewRow, "p_partner_key_n", GetValue(FamilyColumnNames, FamilyRow, "p_partner_key_n"));
                        SetValue(AColumnNames, ref ANewRow, "p_field_key_n", GetValue(StaffDataColumnNames, Commitment, "pm_target_field_n"));
                        SetValue(AColumnNames, ref ANewRow, "p_date_effective_d", GetValue(StaffDataColumnNames, Commitment, "pm_start_of_commitment_d"));
                        SetValue(AColumnNames, ref ANewRow, "p_date_expires_d", GetValue(StaffDataColumnNames, Commitment, "pm_end_of_commitment_d"));
                        
                        CommitmentFound = true;
                        
                        // there can only be one active gift destination per family
                        break;
                    }
                }
                
                // if no active commitment is found then search for a "p_om_field_key_n" and use that to create a gift destination
                if (!CommitmentFound)
                {
                    string OMFieldKey = GetValue(FamilyColumnNames, FamilyRow, "p_om_field_key_n");
                    
                    if (OMFieldKey != "\\N" && OMFieldKey != "0")
                    {
                        SetValue(AColumnNames, ref ANewRow, "p_key_i", RowCounter.ToString());
                        SetValue(AColumnNames, ref ANewRow, "p_partner_key_n", GetValue(FamilyColumnNames, FamilyRow, "p_partner_key_n"));
                        SetValue(AColumnNames, ref ANewRow, "p_field_key_n", GetValue(FamilyColumnNames, FamilyRow, "p_om_field_key_n"));
                        SetValue(AColumnNames, ref ANewRow, "p_date_effective_d", DateTime.Today.ToShortDateString());
                        SetValue(AColumnNames, ref ANewRow, "p_date_expires_d", "\\N");
                    
                        CommitmentFound = true;
                    }
                }
                
                // write gift destination to file
                if (CommitmentFound)
                {
                    AWriter.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                    AWriterTest.WriteLine("BEGIN; " + "COPY a_budget FROM stdin;");
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