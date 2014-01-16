//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using GNU.Gettext;

using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MConference.Validation;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MConference.Gui.Setup
{
    public partial class TFrmConferenceMasterSettings
    {
        /// PartnerKey for selected conference to be set from outside
        public static Int64 FPartnerKey {
            private get; set;
        }

        private void InitializeManualCode()
        {
            string ConferenceName;

            // load data into dataset
            FMainDS.Clear();
            FMainDS.Merge(TRemote.MConference.Conference.WebConnectors.LoadConferenceSettings(FPartnerKey, out ConferenceName));

            // display conference name
            this.Text = this.Text + " [" + ConferenceName + "]";
            txtConferenceName.Text = ConferenceName;

            // display campaign code prefix
            txtCampaignPrefixCode.Text = ((PcConferenceRow)FMainDS.PcConference.Rows[0]).OutreachPrefix;

            // display start/end dates
            dtpStartDate.Date = ((PPartnerLocationRow)FMainDS.PPartnerLocation.Rows[0]).DateEffective;
            dtpEndDate.Date = ((PPartnerLocationRow)FMainDS.PPartnerLocation.Rows[0]).DateGoodUntil;

            // enable dtps only if date is null
            if ((dtpStartDate.Date == null) || (dtpStartDate.Date == DateTime.MinValue))
            {
                dtpStartDate.Enabled = true;
            }

            if ((dtpEndDate.Date == null) || (dtpEndDate.Date == DateTime.MinValue))
            {
                dtpEndDate.Enabled = true;
            }

            // display currency
            cmbCurrency.SetSelectedString(((PcConferenceRow)FMainDS.PcConference.Rows[0]).CurrencyCode, -1);

            // set radio buttons and checkbox
            Boolean ChargeCampaign = true;
            Boolean AddAccommodationCosts = false;

            foreach (PcConferenceOptionRow CurrentRow in FMainDS.PcConferenceOption.Rows)
            {
                if ((CurrentRow.OptionTypeCode == "COST_PER_NIGHT") && (CurrentRow.OptionSet == true))
                {
                    ChargeCampaign = false;
                    rbtNight.Checked = true;
                }
                else if ((CurrentRow.OptionTypeCode == "COST_PER_DAY") && (CurrentRow.OptionSet == true))
                {
                    ChargeCampaign = false;
                    rbtDay.Checked = true;
                }
                else if ((CurrentRow.OptionTypeCode == "ADD_ACCOMM_COST_FOR_TOTAL") && (CurrentRow.OptionSet == true))
                {
                    AddAccommodationCosts = true;
                }
            }

            if (ChargeCampaign == true)
            {
                rbtCampaign.Checked = true;
                chkAddAccommodationCosts.Enabled = false;
            }
            else if (AddAccommodationCosts == true)
            {
                chkAddAccommodationCosts.Checked = true;
                txtSpecialRolePreAccommodation.ReadOnly = false;
                txtVolunteerPreAccommodation.ReadOnly = false;
                txtParticipantPreAccommodation.ReadOnly = false;
                txtSpecialRoleAccommodation.ReadOnly = false;
                txtVolunteerAccommodation.ReadOnly = false;
                txtSpecialRoleCampaignAccommodation.ReadOnly = false;
            }

            // display conference discounts
            foreach (PcDiscountRow CurrentRow in FMainDS.PcDiscount.Rows)
            {
                if (CurrentRow.CostTypeCode == "CONFERENCE")
                {
                    if (CurrentRow.Validity == "PRE")
                    {
                        if (CurrentRow.DiscountCriteriaCode == "ROLE")
                        {
                            txtSpecialRolePreAttendance.NumberValueInt = (int)CurrentRow.Discount;
                        }
                        else if (CurrentRow.DiscountCriteriaCode == "VOL")
                        {
                            txtVolunteerPreAttendance.NumberValueInt = (int)CurrentRow.Discount;
                        }
                        else if (CurrentRow.DiscountCriteriaCode == "OTHER")
                        {
                            txtParticipantPreAttendance.NumberValueInt = (int)CurrentRow.Discount;
                        }
                    }
                    else if (CurrentRow.Validity == "CONF")
                    {
                        if (CurrentRow.DiscountCriteriaCode == "ROLE")
                        {
                            txtSpecialRoleAttendance.NumberValueInt = (int)CurrentRow.Discount;
                        }
                        else if (CurrentRow.DiscountCriteriaCode == "VOL")
                        {
                            txtVolunteerAttendance.NumberValueInt = (int)CurrentRow.Discount;
                        }
                    }
                    else if ((CurrentRow.Validity == "POST") && (CurrentRow.DiscountCriteriaCode == "ROLE"))
                    {
                        txtSpecialRoleCampaignAttendance.NumberValueInt = (int)CurrentRow.Discount;
                    }
                }
                else if (CurrentRow.CostTypeCode == "ACCOMMODATION")
                {
                    if (CurrentRow.Validity == "PRE")
                    {
                        if (CurrentRow.DiscountCriteriaCode == "ROLE")
                        {
                            txtSpecialRolePreAccommodation.NumberValueInt = (int)CurrentRow.Discount;
                        }
                        else if (CurrentRow.DiscountCriteriaCode == "VOL")
                        {
                            txtVolunteerPreAccommodation.NumberValueInt = (int)CurrentRow.Discount;
                        }
                        else if (CurrentRow.DiscountCriteriaCode == "OTHER")
                        {
                            txtParticipantPreAccommodation.NumberValueInt = (int)CurrentRow.Discount;
                        }
                    }
                    else if (CurrentRow.Validity == "CONF")
                    {
                        if (CurrentRow.DiscountCriteriaCode == "ROLE")
                        {
                            txtSpecialRoleAccommodation.NumberValueInt = (int)CurrentRow.Discount;
                        }
                        else if (CurrentRow.DiscountCriteriaCode == "VOL")
                        {
                            txtVolunteerAccommodation.NumberValueInt = (int)CurrentRow.Discount;
                        }
                    }
                    else if ((CurrentRow.Validity == "POST") && (CurrentRow.DiscountCriteriaCode == "ROLE"))
                    {
                        txtSpecialRoleCampaignAccommodation.NumberValueInt = (int)CurrentRow.Discount;
                    }
                }
            }

            // display grid containing venue details
            grdVenues.Columns.Clear();
            grdVenues.AddPartnerKeyColumn(Catalog.GetString("Venue Key"), FMainDS.PcConferenceVenue.ColumnVenueKey);
            grdVenues.AddTextColumn(Catalog.GetString("Venue Name"), FMainDS.PcConferenceVenue.ColumnVenueName);

            DataView MyDataView = FMainDS.PcConferenceVenue.DefaultView;
            MyDataView.Sort = "p_venue_name_c ASC";
            MyDataView.AllowNew = false;
            grdVenues.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);
        }

        // disables or enables the checkbox when a different radio button is selected
        private void AttendanceChargeChanged(object sender, EventArgs e)
        {
            if (rbtDay.Checked || rbtNight.Checked)
            {
                chkAddAccommodationCosts.Enabled = true;
            }
            else
            {
                chkAddAccommodationCosts.Checked = false;
                chkAddAccommodationCosts.Enabled = false;
            }
        }

        // Called when the checkbox is changed. Toggles textboxes' ReadOnly property.
        private void UpdateDiscounts(object sender, EventArgs e)
        {
            Boolean AccommodationDiscountsReadOnly = true;

            if (chkAddAccommodationCosts.Checked)
            {
                AccommodationDiscountsReadOnly = false;
            }

            txtSpecialRolePreAccommodation.ReadOnly = AccommodationDiscountsReadOnly;
            txtVolunteerPreAccommodation.ReadOnly = AccommodationDiscountsReadOnly;
            txtParticipantPreAccommodation.ReadOnly = AccommodationDiscountsReadOnly;
            txtSpecialRoleAccommodation.ReadOnly = AccommodationDiscountsReadOnly;
            txtVolunteerAccommodation.ReadOnly = AccommodationDiscountsReadOnly;
            txtSpecialRoleCampaignAccommodation.ReadOnly = AccommodationDiscountsReadOnly;
        }

        // Called with Add button. Adds new venue to conference.
        private void AddVenue(object sender, EventArgs e)
        {
            long ResultVenueKey;
            String ResultVenueName;
            TPartnerClass? PartnerClass;
            TLocationPK ResultLocationPK;

            DataRow[] ExistingVenueDataRows;

            // the user has to select an existing venue to make that venue a conference venue
            try
            {
                // launches partner find screen and returns true if a venue is selected
                if (TPartnerFindScreenManager.OpenModalForm("VENUE", out ResultVenueKey, out ResultVenueName, out PartnerClass, out ResultLocationPK,
                        this))
                {
                    // search for selected venue in dataset
                    ExistingVenueDataRows = FMainDS.PcConferenceVenue.Select(ConferenceSetupTDSPcConferenceVenueTable.GetVenueKeyDBName() +
                        " = " + ResultVenueKey.ToString());

                    // if venue does not already exist for venue
                    if (ExistingVenueDataRows.Length == 0)
                    {
                        ConferenceSetupTDSPcConferenceVenueRow AddedVenue = FMainDS.PcConferenceVenue.NewRowTyped(true);
                        AddedVenue.ConferenceKey = FPartnerKey;
                        AddedVenue.VenueKey = ResultVenueKey;
                        AddedVenue.VenueName = ResultVenueName;
                        FMainDS.PcConferenceVenue.Rows.Add(AddedVenue);
                        FPetraUtilsObject.SetChangedFlag();
                    }
                    // if venue does already exist for venue
                    else
                    {
                        MessageBox.Show(Catalog.GetString("This venue is already included for this conference"),
                            Catalog.GetString("Add Venue to Conference"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception exp)
            {
                throw new EOPAppException("Exception occured while calling VenueFindScreen!", exp);
            }
        }

        // Called with Remove button. Removes a venue from conference.
        private void RemoveVenue(object sender, EventArgs e)
        {
            if (grdVenues.SelectedDataRows.Length == 1)
            {
                long SelectedVenueKey;

                SelectedVenueKey = (Int64)((DataRowView)grdVenues.SelectedDataRows[0]).Row[PcConferenceVenueTable.GetVenueKeyDBName()];

                DataRow RowToRemove = FMainDS.PcConferenceVenue.Rows.Find(new object[] { FPartnerKey, SelectedVenueKey });
                RowToRemove.Delete();
                FPetraUtilsObject.SetChangedFlag();
            }
        }

        // get data from screen and ammend/add to dataset
        private void GetDataFromControlsManual(PcConferenceRow ARow)
        {
            PcConferenceRow ConferenceData = (PcConferenceRow)FMainDS.PcConference.Rows[0];
            PPartnerLocationRow PartnerLocationData = (PPartnerLocationRow)FMainDS.PPartnerLocation.Rows[0];
            DataRowCollection ConferenceOptionData = FMainDS.PcConferenceOption.Rows;

            ConferenceData.CurrencyCode = cmbCurrency.GetSelectedString();
            ConferenceData.Start = dtpStartDate.Date;
            ConferenceData.End = dtpEndDate.Date;
            PartnerLocationData.DateEffective = dtpStartDate.Date;
            PartnerLocationData.DateGoodUntil = dtpEndDate.Date;

            // get data from radio buttons and check button for PcConferenceOption
            string[] OptionTypeCodes =
            {
                "COST_PER_NIGHT", "COST_PER_DAY", "ADD_ACCOMM_COST_FOR_TOTAL"
            };
            Boolean[] OptionSet =
            {
                rbtNight.Checked, rbtDay.Checked, chkAddAccommodationCosts.Checked
            };

            for (int i = 0; i < 3; i++)
            {
                DataRow RowExists = FMainDS.PcConferenceOption.Rows.Find(new object[] { FPartnerKey, OptionTypeCodes[i] });

                // create new row if needed
                if ((RowExists == null) && OptionSet[i])
                {
                    PcConferenceOptionRow RowToAdd = FMainDS.PcConferenceOption.NewRowTyped(true);
                    RowToAdd.ConferenceKey = FPartnerKey;
                    RowToAdd.OptionTypeCode = OptionTypeCodes[i];
                    RowToAdd.OptionSet = true;
                    FMainDS.PcConferenceOption.Rows.Add(RowToAdd);
                }
                // update existing record
                else if ((RowExists != null) && OptionSet[i])
                {
                    ((PcConferenceOptionRow)RowExists).OptionSet = true;
                }
                // delete existing record if discount is 0
                else if ((RowExists != null) && !OptionSet[i])
                {
                    RowExists.Delete();
                }
            }

            // reset the Accommodation text boxs to 0 if no longer needed
            if (!chkAddAccommodationCosts.Checked)
            {
                txtSpecialRolePreAccommodation.NumberValueInt = 0;
                txtVolunteerPreAccommodation.NumberValueInt = 0;
                txtParticipantPreAccommodation.NumberValueInt = 0;
                txtSpecialRoleAccommodation.NumberValueInt = 0;
                txtVolunteerAccommodation.NumberValueInt = 0;
                txtSpecialRoleCampaignAccommodation.NumberValueInt = 0;
            }

            // get data from discount text boxes for PcDiscount
            string[, ] Discounts =
            {
                { "ROLE", "CONFERENCE", "PRE", txtSpecialRolePreAttendance.Text.TrimEnd(new char[] { ' ', '%' }) },
                { "VOL", "CONFERENCE", "PRE", txtVolunteerPreAttendance.Text.TrimEnd(new char[] { ' ', '%' }) },
                { "OTHER", "CONFERENCE", "PRE", txtParticipantPreAttendance.Text.TrimEnd(new char[] { ' ', '%' }) },
                { "ROLE", "CONFERENCE", "CONF", txtSpecialRoleAttendance.Text.TrimEnd(new char[] { ' ', '%' }) },
                { "VOL", "CONFERENCE", "CONF", txtVolunteerAttendance.Text.TrimEnd(new char[] { ' ', '%' }) },
                { "ROLE", "CONFERENCE", "POST", txtSpecialRoleCampaignAttendance.Text.TrimEnd(new char[] { ' ', '%' }) },
                { "ROLE", "ACCOMMODATION", "PRE", txtSpecialRolePreAccommodation.Text.TrimEnd(new char[] { ' ', '%' }) },
                { "VOL", "ACCOMMODATION", "PRE", txtVolunteerPreAccommodation.Text.TrimEnd(new char[] { ' ', '%' }) },
                { "OTHER", "ACCOMMODATION", "PRE", txtParticipantPreAccommodation.Text.TrimEnd(new char[] { ' ', '%' }) },
                { "ROLE", "ACCOMMODATION", "CONF", txtSpecialRoleAccommodation.Text.TrimEnd(new char[] { ' ', '%' }) },
                { "VOL", "ACCOMMODATION", "CONF", txtVolunteerAccommodation.Text.TrimEnd(new char[] { ' ', '%' }) },
                { "ROLE", "ACCOMMODATION", "POST", txtSpecialRoleCampaignAccommodation.Text.TrimEnd(new char[] { ' ', '%' }) }
            };

            for (int i = 0; i < 12; i++)
            {
                DataRow RowExists = FMainDS.PcDiscount.Rows.Find(new object[] { FPartnerKey, Discounts[i, 0], Discounts[i, 1], Discounts[i, 2], -1 });

                if (Discounts[i, 3] == "")
                {
                    Discounts[i, 3] = "0";
                }

                // create new row if needed
                if ((RowExists == null) && (Convert.ToInt32(Discounts[i, 3]) != 0))
                {
                    PcDiscountRow RowToAdd = FMainDS.PcDiscount.NewRowTyped(true);
                    RowToAdd.ConferenceKey = FPartnerKey;
                    RowToAdd.DiscountCriteriaCode = Discounts[i, 0];
                    RowToAdd.CostTypeCode = Discounts[i, 1];
                    RowToAdd.Validity = Discounts[i, 2];
                    RowToAdd.UpToAge = -1;
                    RowToAdd.Percentage = true;
                    RowToAdd.Discount = Convert.ToInt32(Discounts[i, 3]);
                    FMainDS.PcDiscount.Rows.Add(RowToAdd);
                }
                // update existing record
                else if ((RowExists != null) && (Convert.ToInt32(Discounts[i, 3]) != 0))
                {
                    ((PcDiscountRow)RowExists).Discount = Convert.ToInt32(Discounts[i, 3]);
                }
                // delete existing record if discount is 0
                else if ((RowExists != null) && (Convert.ToInt32(Discounts[i, 3]) == 0))
                {
                    RowExists.Delete();
                }
            }
        }

        // save data
        private TSubmitChangesResult StoreManualCode(ref ConferenceSetupTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;
            
            return TRemote.MConference.Conference.WebConnectors.SaveConferenceSetupTDS(ref ASubmitChanges);
        }

        private void ValidateDataManual(PcConferenceRow ARow)
        {
            PcDiscountTable DiscountTable = FMainDS.PcDiscount;

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            TValidationControlsData ValidationControlsData;
            TScreenVerificationResult VerificationResult = null;
            DataColumn ValidationColumn;

            List <string>CriteriaCodesUsed = new List <string>();

            foreach (PcDiscountRow Row in DiscountTable.Rows)
            {
                if ((Row.RowState != DataRowState.Deleted) && (Row.DiscountCriteriaCode != "CHILD"))
                {
                    if (Row.Discount > 100)
                    {
                        ValidationColumn = Row.Table.Columns[PcDiscountTable.ColumnDiscountId];

                        // displays a warning message
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(this, ErrorCodes.GetErrorInfo(
                                    PetraErrorCodes.ERR_DISCOUNT_PERCENTAGE_GREATER_THAN_100)),
                            ValidationColumn, ValidationControlsData.ValidationControl);

                        // Handle addition to/removal from TVerificationResultCollection
                        VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn);
                    }

                    if (!CriteriaCodesUsed.Exists(element => element == Row.DiscountCriteriaCode))
                    {
                        CriteriaCodesUsed.Add(Row.DiscountCriteriaCode);
                    }
                }
            }

            string[] CriteriaCodesUsedArray = CriteriaCodesUsed.ToArray();

            if (!TRemote.MConference.Conference.WebConnectors.CheckDiscountCriteriaCodeExists(CriteriaCodesUsedArray))
            {
                ValidationColumn = DiscountTable.Columns[PcDiscountTable.ColumnDiscountCriteriaCodeId];

                // displays a warning message
                VerificationResult = new TScreenVerificationResult(new TVerificationResult(this, ErrorCodes.GetErrorInfo(
                            PetraErrorCodes.ERR_DISCOUNT_CRITERIA_CODE_DOES_NOT_EXIST)),
                    ValidationColumn, ValidationControlsData.ValidationControl);

                // Handle addition to/removal from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn);
            }
        }
    }
}