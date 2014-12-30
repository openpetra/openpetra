//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       bernd
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MCommon.Gui;

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel.ShortTerm
{
    /// <summary>
    /// Description of UC_PartnerSelection.ManualCode.
    /// </summary>
    public partial class TFrmUC_ShortTermerSelection
    {
        /// <summary>Holds the Partner key of the selecte conference or outreach </summary>
        private Int64 FSelectedUnitKey;

        /// <summary>
        /// Indicates if the Unitname, Unitkey and Unitcode should be read during read controls
        /// or if the values from the constructor should be taken.
        /// </summary>
        protected bool FSetControlsWithConstructorValues;

        /// <summary>
        /// Initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FPetraUtilsObject = APetraUtilsObject;

            FSelectedUnitKey = 0;
            // TODO
            // In some cases we might want to set the values for the user control right here.
            // For example if a report is opened from an existing conference, we want to show the
            // data of this conference.
            FSetControlsWithConstructorValues = false;

            txtEventCode.ReadOnly = true;
            rbtThisEventOnly.Checked = true;
            rbtAllParticipants.Checked = true;
        }

        /// <summary>
        /// set the functions and column names that are available
        /// </summary>
        /// <param name="AAvailableFunctions"></param>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
        }

        /// <summary>
        /// read the values from the controls and give them to the calculator
        /// </summary>
        /// <param name="ACalculator"></param>
        /// <param name="AReportAction"></param>
        public void ReadControls(TRptCalculator ACalculator, TReportActionEnum AReportAction)
        {
            if (rbtFromExtract.Checked)
            {
                ACalculator.AddParameter("param_source", "Extract");
            }
            else
            {
                ACalculator.AddParameter("param_source", "Event");
            }

            if (rbtThisEventOnly.Checked)
            {
                ACalculator.AddParameter("param_event_selection", "this");
            }
            else if (rbtRelatedOptions.Checked)
            {
                // Event and related options
                ACalculator.AddParameter("param_event_selection", "related");
            }
            else
            {
                // all events
                ACalculator.AddParameter("param_event_selection", "all");
            }

            ACalculator.AddParameter("param_event_code", txtEventCode.Text);
            ACalculator.AddParameter("param_event_name", lblEventName.Text);
            ACalculator.AddParameter("param_extract_name", txtExtract.Text);

            if (txtEventCode.Text.Length > 5)
            {
                ACalculator.AddParameter("param_conference_code", txtEventCode.Text.Substring(0, 5) + "%");
            }
            else
            {
                ACalculator.AddParameter("param_conference_code", txtEventCode.Text);
            }

            ACalculator.AddParameter("param_unit_key", FSelectedUnitKey.ToString());

            if (chkAccepted.Checked)
            {
                ACalculator.AddParameter("param_application_status_accepted", "true");
            }
            else
            {
                ACalculator.AddParameter("param_application_status_accepted", "false");
            }

            if (chkCancelled.Checked)
            {
                ACalculator.AddParameter("param_application_status_cancelled", "true");
            }
            else
            {
                ACalculator.AddParameter("param_application_status_cancelled", "false");
            }

            if (chkEnquiry.Checked)
            {
                ACalculator.AddParameter("param_application_status_enquiry", "true");
            }
            else
            {
                ACalculator.AddParameter("param_application_status_enquiry", "false");
            }

            if (chkHold.Checked)
            {
                ACalculator.AddParameter("param_application_status_hold", "true");
            }
            else
            {
                ACalculator.AddParameter("param_application_status_hold", "false");
            }

            if (chkRejected.Checked)
            {
                ACalculator.AddParameter("param_application_status_rejected", "true");
            }
            else
            {
                ACalculator.AddParameter("param_application_status_rejected", "false");
            }

            TVerificationResult VerificationResult;

            if ((AReportAction == TReportActionEnum.raGenerate)
                && (!rbtAllEvents.Checked)
                && (txtEventCode.Text.Length == 0))
            {
                // Error: an event must be selected when we generate the report
                // But we allow saving even if no event is selected
                VerificationResult = new TVerificationResult("Select an event to run the report against to.",
                    "No event was selected!",
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            if ((AReportAction == TReportActionEnum.raGenerate)
                && (!chkAccepted.Checked
                    && !chkCancelled.Checked
                    && !chkEnquiry.Checked
                    && !chkHold.Checked
                    && !chkRejected.Checked))
            {
                // Error: at least one status must be checked
                VerificationResult = new TVerificationResult("Select at least one application status.",
                    "No application status selected!",
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }

        /// <summary>
        /// initialise the controls using the parameters
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControls(TParameterList AParameters)
        {
            rbtThisEventOnly.Checked = (AParameters.Get("param_event_selection").ToString() == "this");
            rbtRelatedOptions.Checked = (AParameters.Get("param_event_selection").ToString() == "related");
            rbtAllEvents.Checked = (AParameters.Get("param_event_selection").ToString() == "all");

            rbtAllParticipants.Checked = (AParameters.Get("param_source").ToString() == "Event");
            rbtFromExtract.Checked = (AParameters.Get("param_source").ToString() == "Extract");
            txtEventCode.Text = AParameters.Get("param_event_code").ToString();
            lblEventName.Text = AParameters.Get("param_event_name").ToString();
            txtExtract.Text = AParameters.Get("param_extract_name").ToString();

            chkAccepted.Checked = AParameters.Get("param_application_status_accepted").ToBool();
            chkCancelled.Checked = AParameters.Get("param_application_status_cancelled").ToBool();
            chkHold.Checked = AParameters.Get("param_application_status_hold").ToBool();
            chkEnquiry.Checked = AParameters.Get("param_application_status_enquiry").ToBool();
            chkRejected.Checked = AParameters.Get("param_application_status_rejected").ToBool();

            // TODO in which cases do we take the values from Initialisation and not the one from
            // SetControls?
            FSelectedUnitKey = AParameters.Get("param_unit_key").ToInt64();
        }

        /// <summary>
        /// Checks if some addional info is required in the report. This function must be called
        /// after "ReadControls()" on UC_PartnerColumns. Because it looks if there are some special
        /// values in the ACalculator parameters which requires some special function calls
        /// in the xml file.
        /// </summary>
        /// <param name="ACalculator"></param>
        /// <param name="AReportAction"></param>
        public void CheckAdditionalInfo(TRptCalculator ACalculator, TReportActionEnum AReportAction)
        {
            if (IsAddressDetailUsed(ACalculator))
            {
                ACalculator.AddParameter("param_address_detail", "true");
            }
            else
            {
                ACalculator.AddParameter("param_address_detail", "false");
            }

            if (IsPassportDetailUsed(ACalculator))
            {
                ACalculator.AddParameter("param_passport_detail", "true");
            }
            else
            {
                ACalculator.AddParameter("param_passport_detail", "false");
            }

            if (IsChurchDetailUsed(ACalculator))
            {
                ACalculator.AddParameter("param_church_detail", "true");
            }
            else
            {
                ACalculator.AddParameter("param_church_detail", "false");
            }

            if (IsPartnerNationalitiesUsed(ACalculator))
            {
                ACalculator.AddParameter("param_nationalities", "true");
            }
            else
            {
                ACalculator.AddParameter("param_nationalities", "false");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtEventSelectionChanged(object sender, EventArgs e)
        {
            txtEventCode.Enabled = !rbtAllEvents.Checked;
            btnEvent.Enabled = !rbtAllEvents.Checked;
            lblEventName.Visible = !rbtAllEvents.Checked;
        }

        private void rbtParticipantsSelectionChanged(object sender, EventArgs e)
        {
            txtExtract.Enabled = rbtFromExtract.Checked;
        }

        private void btnEventClicked(object sender, EventArgs e)
        {
            TFrmSelectEvent SelectEventForm = new TFrmSelectEvent(FPetraUtilsObject.GetForm());

            if (SelectEventForm.ShowDialog() == DialogResult.OK)
            {
                txtEventCode.Text = SelectEventForm.FSelectedOutreachCode;
                lblEventName.Text = SelectEventForm.FSelectedUnitName;
                FSelectedUnitKey = SelectEventForm.FSelectedPartnerKey;
            }
            else
            {
                // should we reset the values or keep the old values?
                // For now, just keep the old values.
            }
        }

        /// <summary>
        /// Checks if there is one column which needs address details.
        /// </summary>
        /// <returns></returns>
        private bool IsAddressDetailUsed(TRptCalculator ACalculator)
        {
            // if one part of an address is used
            DataTable ColumnParameters = ACalculator.GetParameters().ToDataTable();
            bool HasAddressDetail = false;

            foreach (DataRow Row in ColumnParameters.Rows)
            {
                String ColumnValue = Row[4].ToString();

                if ((ColumnValue == "eString:Address Street")
                    || (ColumnValue == "eString:Address Post Code")
                    || (ColumnValue == "eString:Address City")
                    || (ColumnValue == "eString:Address State / County / Province")
                    || (ColumnValue == "eString:Address Country")
                    || (ColumnValue == "eString:Primary Email")
                    || (ColumnValue == "eString:Primary Phone")
                    || (ColumnValue == "eString:Address Line 1")
                    || (ColumnValue == "eString:Address Line 3"))
                {
                    HasAddressDetail = true;
                    break;
                }
            }

            return HasAddressDetail;
        }

        /// <summary>
        /// Checks if there is one column which needs passport details.
        /// </summary>
        /// <returns></returns>
        private bool IsPassportDetailUsed(TRptCalculator ACalculator)
        {
            // if one part of an address is used
            DataTable ColumnParameters = ACalculator.GetParameters().ToDataTable();
            bool HasPassportDetail = false;

            foreach (DataRow Row in ColumnParameters.Rows)
            {
                String ColumnValue = Row[4].ToString();

                if ((ColumnValue == "eString:Nationality")
                    || (ColumnValue == "eString:Passport Expiry Date")
                    || (ColumnValue == "eString:Passport Name")
                    || (ColumnValue == "eString:Passport Number")
                    || (ColumnValue == "eString:Passport Date of Issue")
                    || (ColumnValue == "eString:Passport Place of Issue")
                    || (ColumnValue == "eString:Passport Type")
                    || (ColumnValue == "eString:Passport Date of Birth")
                    || (ColumnValue == "eString:Passport Place of Birth")
                    || (ColumnValue == "eString:Passport Country of Issue"))
                {
                    HasPassportDetail = true;
                    break;
                }
            }

            return HasPassportDetail;
        }

        /// <summary>
        /// Checks if there is one column which needs address details of the church
        /// </summary>
        /// <returns></returns>
        private bool IsChurchDetailUsed(TRptCalculator ACalculator)
        {
            // if one part of an address is used
            DataTable ColumnParameters = ACalculator.GetParameters().ToDataTable();

            bool HasAddressDetail = false;

            foreach (DataRow Row in ColumnParameters.Rows)
            {
                String ColumnValue = Row[4].ToString();

                if (ColumnValue.StartsWith("eString:Church"))
                {
                    HasAddressDetail = true;
                    break;
                }
            }

            return HasAddressDetail;
        }

        /// <summary>
        /// Checks if there is one column which needs nationalities of the partner
        /// </summary>
        /// <returns></returns>
        private bool IsPartnerNationalitiesUsed(TRptCalculator ACalculator)
        {
            // if one part of an address is used
            DataTable ColumnParameters = ACalculator.GetParameters().ToDataTable();

            bool HasNationalities = false;

            foreach (DataRow Row in ColumnParameters.Rows)
            {
                String ColumnValue = Row[4].ToString();

                if (ColumnValue.Contains("eString:Nationalities"))
                {
                    HasNationalities = true;
                    break;
                }
            }

            return HasNationalities;
        }
    }
}