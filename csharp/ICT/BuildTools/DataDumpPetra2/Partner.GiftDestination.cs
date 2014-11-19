//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters, timop
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
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
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
        /// used to store information about a person
        /// </summary>
        private class PersonKeyAndRow
        {
            /// <summary>
            /// constructor
            /// </summary>
            public PersonKeyAndRow(string APersonKey, string[] APersonRow)
            {
                PersonKey = APersonKey;
                PersonRow = APersonRow;
            }

            /// the person key
            public string PersonKey;
            /// the row from the progress file
            public string[] PersonRow;
        }

        /// <summary>
        /// Populate the empty table PPartnerGiftDestination using PmStaffData
        /// </summary>
        public static int PopulatePPartnerGiftDestination(StringCollection AColumnNames,
            ref string[] ANewRow,
            StreamWriter AWriter,
            StreamWriter AWriterTest)
        {
            List <string[]>ActiveCommitments = new List <string[]>();
            List <string[]>Persons = new List <string[]>();
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

                string strStartOfCommitment = GetValue(StaffDataColumnNames, StaffDataRow, "pm_start_of_commitment_d");
                string strEndOfCommitment = GetValue(StaffDataColumnNames, StaffDataRow, "pm_end_of_commitment_d");

                try
                {
                    // if commitment is currently active
                    if ((DateTime.ParseExact(strStartOfCommitment, "dd/mm/yyyy", CultureInfo.InvariantCulture) <= DateTime.Today)
                        && ((strEndOfCommitment == "\\N")
                            || (DateTime.ParseExact(strEndOfCommitment, "dd/mm/yyyy", CultureInfo.InvariantCulture) >= DateTime.Today))
                        && (strStartOfCommitment != strEndOfCommitment))
                    {
                        ActiveCommitments.Add(StaffDataRow);
                    }
                }
                catch
                {
                    TLogging.Log("WARNING: Invalid date in commitment: " +
                        strStartOfCommitment + " or " + strEndOfCommitment);
                }
            }

            // load the file p_person.d.gz
            TTable PersonTable = TDumpProgressToPostgresql.GetStoreOld().GetTable("p_person");

            TParseProgressCSV PersonParser = new TParseProgressCSV(
                TAppSettingsManager.GetValue("fulldumpPath", "fulldump") + Path.DirectorySeparatorChar + "p_person.d.gz",
                PersonTable.grpTableField.Count);

            StringCollection PersonColumnNames = GetColumnNames(PersonTable);
            SortedList <string, List <PersonKeyAndRow>>FamilyKeysWithPersons =
                new SortedList <string, List <PersonKeyAndRow>>();

            // add all Persons to a list
            while (true)
            {
                string[] PersonRow = PersonParser.ReadNextRow();

                if (PersonRow == null)
                {
                    break;
                }

                string familyKey = GetValue(PersonColumnNames, PersonRow, "p_family_key_n");

                if (!FamilyKeysWithPersons.ContainsKey(familyKey))
                {
                    FamilyKeysWithPersons.Add(familyKey, new List <PersonKeyAndRow>());
                }

                FamilyKeysWithPersons[familyKey].Add(
                    new PersonKeyAndRow(
                        GetValue(PersonColumnNames, PersonRow, "p_partner_key_n"), PersonRow));

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

                string familykey = GetValue(FamilyColumnNames, FamilyRow, "p_partner_key_n");

                // find Person partners belonging to the family
                bool CommitmentFound = false;
                int MinimumFamilyId = int.MaxValue;

                // if family contains Persons
                if (FamilyKeysWithPersons.ContainsKey(familykey))
                {
                    // read through each of the Family's Persons
                    foreach (PersonKeyAndRow PersonRecord in FamilyKeysWithPersons[familykey])
                    {
                        // find if the Person has a currently active commitment
                        string[] Commitment = ActiveCommitments.Find(e => GetValue(StaffDataColumnNames, e, "p_partner_key_n") ==
                            PersonRecord.PersonKey);

                        // if currently active commitment exists create a new Gift Destination record
                        if (Commitment != null)
                        {
                            int CurrentFamilyId = Convert.ToInt32(GetValue(PersonColumnNames, PersonRecord.PersonRow, "p_old_omss_family_id_i"));

                            if (CurrentFamilyId < MinimumFamilyId)
                            {
                                SetValue(AColumnNames, ref ANewRow, "p_key_i", RowCounter.ToString());
                                SetValue(AColumnNames, ref ANewRow, "p_partner_key_n", GetValue(FamilyColumnNames, FamilyRow, "p_partner_key_n"));
                                SetValue(AColumnNames, ref ANewRow, "p_field_key_n", GetValue(StaffDataColumnNames, Commitment, "pm_target_field_n"));
                                SetValue(AColumnNames, ref ANewRow, "p_comment_c", "\\N");

                                TTableField tf = new TTableField();
                                tf.strName = "pm_start_of_commitment_d";
                                tf.strType = "DATE";

                                SetValue(AColumnNames, ref ANewRow, "p_date_effective_d",
                                    TFixData.FixValue(GetValue(StaffDataColumnNames, Commitment, "pm_start_of_commitment_d"), tf));
                                tf.strName = "pm_end_of_commitment_d";
                                SetValue(AColumnNames, ref ANewRow, "p_date_expires_d",
                                    TFixData.FixValue(GetValue(StaffDataColumnNames, Commitment, "pm_end_of_commitment_d"), tf));

                                CommitmentFound = true;

                                MinimumFamilyId = CurrentFamilyId;

                                // there can only be one active gift destination per family
                                break;
                            }
                        }
                    }
                }

                // if no active commitment is found then search for a "p_om_field_key_n" and use that to create a gift destination
                if (!CommitmentFound)
                {
                    string OMFieldKey = GetValue(FamilyColumnNames, FamilyRow, "p_om_field_key_n");

                    if ((OMFieldKey != "\\N") && (OMFieldKey != "0"))
                    {
                        SetValue(AColumnNames, ref ANewRow, "p_key_i", RowCounter.ToString());
                        SetValue(AColumnNames, ref ANewRow, "p_partner_key_n", GetValue(FamilyColumnNames, FamilyRow, "p_partner_key_n"));
                        SetValue(AColumnNames, ref ANewRow, "p_field_key_n", GetValue(FamilyColumnNames, FamilyRow, "p_om_field_key_n"));
                        DateTime LastYear = DateTime.Today.AddYears(-1);
                        SetValue(AColumnNames, ref ANewRow, "p_date_effective_d",
                            string.Format("{0}-{1}-{2}", LastYear.Year, LastYear.Month, LastYear.Day));
                        SetValue(AColumnNames, ref ANewRow, "p_date_expires_d", "\\N");
                        SetValue(AColumnNames, ref ANewRow, "p_comment_c", Catalog.GetString("Copied from Petra's OM Field Key."));

                        CommitmentFound = true;
                    }
                }

                // write gift destination to file
                if (CommitmentFound)
                {
                    AWriter.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());

                    if (AWriterTest != null)
                    {
                        AWriterTest.WriteLine("BEGIN; " + "COPY p_partner_gift_destination FROM stdin;");
                        AWriterTest.WriteLine(StringHelper.StrMerge(ANewRow, '\t').Replace("\\\\N", "\\N").ToString());
                        AWriterTest.WriteLine("\\.");
                        AWriterTest.WriteLine("ROLLBACK;");
                    }

                    RowCounter++;
                }
            }

            return RowCounter;
        }
    }
}