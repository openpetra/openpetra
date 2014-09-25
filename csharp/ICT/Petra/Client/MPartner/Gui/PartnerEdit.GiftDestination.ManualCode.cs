//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// Class for updating Gift Destinations based on changes to Commitment records
    /// </summary>
    public class TGiftDestination
    {
        // Dataset containing all data from PartnerEdit screen
        private PartnerEditTDS FInspectDS;

        // Datatable containing original commitment records (i.e. what they looked like before editing)
        private PmStaffDataTable FOriginalCommitments;

        private IndividualDataTDS FIndividualDataDS;

        // new Gift Destination records that have been created as a result of changes to Commitments
        private int FNewRecords = 0;

        /// <summary>
        /// Gives the user the option to update Gift Destination records if commitments have been added
        /// </summary>
        /// <param name="AInspectDS"></param>
        public bool UpdateGiftDestination(ref PartnerEditTDS AInspectDS)
        {
            PmStaffDataTable EligibleCommitments = new PmStaffDataTable();

            FIndividualDataDS = new IndividualDataTDS();
            FInspectDS = AInspectDS;

            FIndividualDataDS.Merge(AInspectDS);

            // return if no changes have been made to commitments
            if ((FIndividualDataDS.PmStaffData == null) || (FIndividualDataDS.PmStaffData.Rows.Count == 0))
            {
                return false;
            }

            // load original (currently saved) PmStaffData
            FOriginalCommitments = TRemote.MPersonnel.WebConnectors.LoadPersonellStaffData(AInspectDS.PPartner[0].PartnerKey).PmStaffData;

            if (!GetEligibleRowsForUpdatingGiftDestination(ref EligibleCommitments))
            {
                // no eligible records
                return false;
            }

            // iterate through each eligible row
            foreach (PmStaffDataRow EligibleCommitmentRow in EligibleCommitments.Rows)
            {
                UpdateGiftDestinationForSingleCommitment(EligibleCommitmentRow);
            }

            return true;
        }

        /// Determine which (if any) new or modified rows are eligible to update the gift destination
        private bool GetEligibleRowsForUpdatingGiftDestination(ref PmStaffDataTable AEligibleCommitments)
        {
            PmStaffDataRow OriginalCommitmentRow = null;

            foreach (PmStaffDataRow Row in FIndividualDataDS.PmStaffData.Rows)
            {
                // Commitments only trigger changes to Gift Destination if they are new and apply to future dates.
                if (Row.RowState == DataRowState.Deleted)
                {
                    // need to temporarily undelete row
                    Row.RejectChanges();

                    if (Row.IsEndOfCommitmentNull() || (Row.EndOfCommitment >= DateTime.Today))
                    {
                        AEligibleCommitments.LoadDataRow(Row.ItemArray, true);
                    }

                    Row.Delete();
                }
                else
                {
                    OriginalCommitmentRow = (PmStaffDataRow)FOriginalCommitments.Rows.Find(
                        new object[] { Row.SiteKey, Row.Key });

                    if ((Row.RowState != DataRowState.Unchanged) && (Row.IsEndOfCommitmentNull() || (Row.EndOfCommitment >= DateTime.Today)
                                                                     || ((OriginalCommitmentRow != null)
                                                                         && (OriginalCommitmentRow.IsEndOfCommitmentNull()
                                                                             || (OriginalCommitmentRow.EndOfCommitment >= DateTime.Today)))))
                    {
                        AEligibleCommitments.LoadDataRow(Row.ItemArray, true);
                    }
                }
            }

            if (AEligibleCommitments.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }

        private void UpdateGiftDestinationForSingleCommitment(PmStaffDataRow AEligibleCommitmentRow)
        {
            List <PPartnerGiftDestinationRow>ActiveGiftDestinationsWhichCanBeEnded = new List <PPartnerGiftDestinationRow>();
            PPartnerGiftDestinationRow GiftDestinationWhichCanBeModified = null;
            PPartnerGiftDestinationRow GiftDestinationWhichCanBeDeactivated = null;
            DataRowState RowState = DataRowState.Unchanged;
            PmStaffDataRow OriginalCommitmentRow = null;

            // get the rowstate of EligibleCommitmentRow
            DataRow TempRow = FIndividualDataDS.PmStaffData.Rows.Find(new object[] { AEligibleCommitmentRow.SiteKey, AEligibleCommitmentRow.Key });

            if (TempRow != null)
            {
                RowState = TempRow.RowState;
            }
            else
            {
                RowState = DataRowState.Deleted;
            }

            // If gift destination records already exist for this partner's family...
            if ((FInspectDS.PPartnerGiftDestination != null) && (FInspectDS.PPartnerGiftDestination.Rows.Count > 0))
            {
                // Get the original record (before it was edited)
                if ((RowState == DataRowState.Modified) || (RowState == DataRowState.Deleted))
                {
                    OriginalCommitmentRow = (PmStaffDataRow)FOriginalCommitments.Rows.Find(
                        new object[] { AEligibleCommitmentRow.SiteKey, AEligibleCommitmentRow.Key });
                }

                CompareCommitmentToGiftDestinations(RowState, AEligibleCommitmentRow, OriginalCommitmentRow,
                    ref GiftDestinationWhichCanBeModified, ref GiftDestinationWhichCanBeDeactivated, ref ActiveGiftDestinationsWhichCanBeEnded);
            }

            DealWithPotentialGiftDestinationUpdates(RowState, AEligibleCommitmentRow, GiftDestinationWhichCanBeModified,
                GiftDestinationWhichCanBeDeactivated, ActiveGiftDestinationsWhichCanBeEnded);
        }

        /// Iterate through each gift destination and compare with commitment record.
        /// Determine if gift destination can be updated and how.
        private void CompareCommitmentToGiftDestinations(DataRowState ARowState,
            PmStaffDataRow AEligibleCommitmentRow,
            PmStaffDataRow AOriginalCommitmentRow,
            ref PPartnerGiftDestinationRow AGiftDestinationWhichCanBeModified,
            ref PPartnerGiftDestinationRow AGiftDestinationWhichCanBeDeactivated,
            ref List <PPartnerGiftDestinationRow>AActiveGiftDestinationsWhichCanBeEnded)
        {
            // only changes to recieving field, start data and end date can update the Gift Destination
            if ((ARowState == DataRowState.Modified)
                && (AOriginalCommitmentRow.ReceivingField == AEligibleCommitmentRow.ReceivingField)
                && (AOriginalCommitmentRow.StartOfCommitment == AEligibleCommitmentRow.StartOfCommitment)
                && (AOriginalCommitmentRow.EndOfCommitment == AEligibleCommitmentRow.EndOfCommitment))
            {
                return;
            }

            bool Repeat = true;

            while (Repeat)
            {
                Repeat = false;

                foreach (PPartnerGiftDestinationRow CurrentRow in FInspectDS.PPartnerGiftDestination.Rows)
                {
                    // if two records are the same
                    if ((AOriginalCommitmentRow != null)
                        && (AOriginalCommitmentRow.ReceivingField == CurrentRow.FieldKey)
                        && (AOriginalCommitmentRow.StartOfCommitment == CurrentRow.DateEffective)
                        && (AOriginalCommitmentRow.EndOfCommitment == CurrentRow.DateExpires)
                        && (CurrentRow.DateEffective != CurrentRow.DateExpires))
                    {
                        if ((ARowState == DataRowState.Modified)
                            && (AOriginalCommitmentRow.StartOfCommitment == AEligibleCommitmentRow.StartOfCommitment)
                            && (AOriginalCommitmentRow.ReceivingField == AEligibleCommitmentRow.ReceivingField))
                        {
                            // if the field and start date have not been changed then we can update the Gift Destination
                            AGiftDestinationWhichCanBeModified = CurrentRow;
                            continue;
                        }
                        else if (ARowState == DataRowState.Deleted)
                        {
                            AGiftDestinationWhichCanBeDeactivated = CurrentRow;
                            return;
                        }
                    }
                    else if (ARowState == DataRowState.Deleted)
                    {
                        continue;
                    }
                    else if (CurrentRow.DateEffective == CurrentRow.DateExpires)
                    {
                        // ignore inactive Gift Destination
                        continue;
                    }

                    if ((AEligibleCommitmentRow.IsEndOfCommitmentNull()
                         && (CurrentRow.IsDateExpiresNull() || (CurrentRow.DateExpires >= AEligibleCommitmentRow.StartOfCommitment)))
                        || !AEligibleCommitmentRow.IsEndOfCommitmentNull()
                        && ((CurrentRow.IsDateExpiresNull() && (CurrentRow.DateEffective <= AEligibleCommitmentRow.EndOfCommitment))
                            || (!CurrentRow.IsDateExpiresNull()
                                && (((CurrentRow.DateEffective >= AEligibleCommitmentRow.StartOfCommitment)
                                     && (CurrentRow.DateEffective <= AEligibleCommitmentRow.EndOfCommitment))
                                    || (CurrentRow.DateExpires >= AEligibleCommitmentRow.StartOfCommitment)
                                    && (CurrentRow.DateExpires <= AEligibleCommitmentRow.EndOfCommitment)))))
                    {
                        AActiveGiftDestinationsWhichCanBeEnded.Add(CurrentRow);
                    }
                }
            }
        }

        /// Ask the user if they want to update the Gift Destination and then make the changes
        private void DealWithPotentialGiftDestinationUpdates(DataRowState ARowState, PmStaffDataRow AEligibleCommitmentRow,
            PPartnerGiftDestinationRow AGiftDestinationWhichCanBeModified,
            PPartnerGiftDestinationRow AGiftDestinationWhichCanBeDeactivated, List <PPartnerGiftDestinationRow>AActiveGiftDestinationsWhichCanBeEnded)
        {
            string UnitName = "";
            string ActiveGiftDestinations = "";
            string EndOfCommitment;
            bool CreateNewGiftDestination = false;
            TPartnerClass PartnerClass;

            // set end date to display to user in messagebox
            if (AEligibleCommitmentRow.IsEndOfCommitmentNull())
            {
                EndOfCommitment = "'Open Ended'";
            }
            else
            {
                EndOfCommitment = AEligibleCommitmentRow.EndOfCommitment.Value.ToShortDateString();
            }

            // get the receiving fields short name
            TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(AEligibleCommitmentRow.ReceivingField,
                out UnitName,
                out PartnerClass);
            UnitName = UnitName + " (" + AEligibleCommitmentRow.ReceivingField + ")";

            // if existing active Gift Destination/s can be ended in order to add a Gift Destination for new commitment
            if (AActiveGiftDestinationsWhichCanBeEnded.Count > 0)
            {
                string Message = "";

                // get gift destination field's short name
                foreach (PPartnerGiftDestinationRow Row in AActiveGiftDestinationsWhichCanBeEnded)
                {
                    string ActiveGiftDestinationName = "";

                    TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(
                        Row.FieldKey, out ActiveGiftDestinationName, out PartnerClass);

                    // add conjunctions
                    if (AActiveGiftDestinationsWhichCanBeEnded.Count > 1)
                    {
                        if (AActiveGiftDestinationsWhichCanBeEnded.IndexOf(Row) == AActiveGiftDestinationsWhichCanBeEnded.Count - 1)
                        {
                            ActiveGiftDestinations += " and ";
                        }
                        else if (AActiveGiftDestinationsWhichCanBeEnded.IndexOf(Row) < AActiveGiftDestinationsWhichCanBeEnded.Count - 1)
                        {
                            ActiveGiftDestinations += ", ";
                        }
                    }

                    ActiveGiftDestinations += "'" + ActiveGiftDestinationName + "' (" + Row.FieldKey + ")";
                }

                // two different messages depending on number of gift destinations that need to be closed
                if (AActiveGiftDestinationsWhichCanBeEnded.Count == 1)
                {
                    Message = Catalog.GetString(string.Format(
                            "This Person's Family has an existing Gift Destination record to the field {0} that is active during the period {1} to {2}."
                            +
                            "{3}Would you like to shorten or deactivate this Gift Destination and make this new Commitment to " +
                            "the field '{4}' the active Gift Destination for this period?",
                            ActiveGiftDestinations, AEligibleCommitmentRow.StartOfCommitment.ToShortDateString(), EndOfCommitment,
                            "\n\n", UnitName));
                }
                else
                {
                    Message = Catalog.GetString(string.Format(
                            "This Person's Family has existing Gift Destination records to the fields {0} that are active during the period {1} to {2}."
                            +
                            "{3}Would you like to shorten or deactivate these Gift Destinations and make this new Commitment to " +
                            "the field '{4}' the active Gift Destination for this period?",
                            ActiveGiftDestinations, AEligibleCommitmentRow.StartOfCommitment.ToShortDateString(), EndOfCommitment,
                            "\n\n", UnitName));
                }

                // offer to end Gift Destination/s and use new commitment instead
                if (MessageBox.Show(Message,
                        Catalog.GetString("Update Gift Destination"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    CreateNewGiftDestination = true;

                    foreach (PPartnerGiftDestinationRow Row in AActiveGiftDestinationsWhichCanBeEnded)
                    {
                        if (Row.DateEffective >= AEligibleCommitmentRow.StartOfCommitment)
                        {
                            // deactivate future gift destinations
                            Row.DateExpires = Row.DateEffective;
                        }
                        else
                        {
                            // shorten older gift destinations
                            if (Row.DateEffective != AEligibleCommitmentRow.StartOfCommitment)
                            {
                                Row.DateExpires = AEligibleCommitmentRow.StartOfCommitment.AddDays(-1);
                            }
                            else
                            {
                                Row.DateExpires = AEligibleCommitmentRow.StartOfCommitment;
                            }
                        }
                    }
                }
            }
            // if an existing gift destination can be updated from a modified commitment
            else if (AGiftDestinationWhichCanBeModified != null)
            {
                // offer to modify this Gift Destination
                if (MessageBox.Show(Catalog.GetString(string.Format(
                                "This Person's Family has an existing Gift Destination record which matches a modified commitment." +
                                "{0}Would you like to update the Gift Destination record to the field '{1}' " +
                                "for the period {2} to {3}?",
                                "\n\n", UnitName,
                                AEligibleCommitmentRow.StartOfCommitment.ToShortDateString(), EndOfCommitment)),
                        Catalog.GetString("Update Gift Destination"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    AGiftDestinationWhichCanBeModified.DateExpires = AEligibleCommitmentRow.EndOfCommitment;
                }
            }
            // if an existing gift destination can be made inactive because of a deleted commitment
            else if (AGiftDestinationWhichCanBeDeactivated != null)
            {
                // offer to deactivate this Gift Destination
                if (MessageBox.Show(Catalog.GetString(string.Format(
                                "This Person's Family has an existing Gift Destination record which matches a deleted commitment.{0}" +
                                "Would you like to deactivate the Gift Destination record to the field '{1}'" +
                                "for the period {2} to {3}?",
                                "\n\n", UnitName,
                                AEligibleCommitmentRow.StartOfCommitment.ToShortDateString(), EndOfCommitment)),
                        Catalog.GetString("Update Gift Destination"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    AGiftDestinationWhichCanBeDeactivated.DateExpires = AGiftDestinationWhichCanBeDeactivated.DateEffective;
                }
            }
            // if there is no active Gift Destination during dates of new commitment
            else if (ARowState != DataRowState.Deleted)
            {
                // offer to create new Gift Destination using new commitment
                if (MessageBox.Show(Catalog.GetString(string.Format(
                                "This Person's Family does not have an active Gift Destination during the period " +
                                "{0} to {1}.{2}Would you like to make this new Commitment to the field " +
                                "'{3}' the active Gift Destination for this period?",
                                AEligibleCommitmentRow.StartOfCommitment.ToShortDateString(), EndOfCommitment, "\n\n",
                                UnitName)),
                        Catalog.GetString("Update Gift Destination"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    CreateNewGiftDestination = true;
                }
            }

            /* create a brand new Gift Destination */

            if (CreateNewGiftDestination)
            {
                if (FInspectDS.PPartnerGiftDestination == null)
                {
                    FInspectDS.Merge(new PPartnerGiftDestinationTable());
                }

                // create new Gift Destination
                PPartnerGiftDestinationRow NewRow = FInspectDS.PPartnerGiftDestination.NewRowTyped(true);
                NewRow.Key = TRemote.MPartner.Partner.WebConnectors.GetNewKeyForPartnerGiftDestination() + FNewRecords;
                NewRow.PartnerKey = ((PPersonRow)FInspectDS.PPerson.Rows[0]).FamilyKey;
                NewRow.FieldKey = AEligibleCommitmentRow.ReceivingField;
                NewRow.DateEffective = AEligibleCommitmentRow.StartOfCommitment;
                NewRow.DateExpires = AEligibleCommitmentRow.EndOfCommitment;
                FInspectDS.PPartnerGiftDestination.Rows.Add(NewRow);

                FNewRecords++;
            }
        }
    }
}