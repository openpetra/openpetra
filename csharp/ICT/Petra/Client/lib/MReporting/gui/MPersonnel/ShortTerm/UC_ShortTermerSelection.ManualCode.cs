//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       bernd
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// Description of UC_PartnerSelection.ManualCode.
    /// </summary>
    public partial class TFrmUC_ShortTermerSelection
    {
        /// <summary>Holds the Partner key of the selecte conference or campaign </summary>
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

            long PartnerKey = -1;

            try
            {
                // TODO
//	                PartnerKey = StringHelper.StrToPartnerKey(AReportParameter);
                FSelectedUnitKey = PartnerKey;
                FSetControlsWithConstructorValues = true;
            }
            catch
            {
                FSelectedUnitKey = 0;
                FSetControlsWithConstructorValues = false;
            }

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

            ACalculator.AddParameter("param_event_name", txtEvent.Text);
            ACalculator.AddParameter("param_extract_name", txtExtract.Text);
            // TODO
//            FCalculator.AddParameter("param_event_description", lblEventCode.Text);
//            FCalculator.AddParameter("param_extract_description", lblExtract.Text);

            if (txtEvent.Text.Length > 5)
            {
                ACalculator.AddParameter("param_conference_code", txtEvent.Text.Substring(0, 5) + "%");
            }
            else
            {
                ACalculator.AddParameter("param_conference_code", txtEvent.Text);
            }

            // TODO
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
                && (txtEvent.Text.Length == 0))
            {
                // Error: an event must be selected when we generate the report
                // But we allow saving even if no event is selected
                VerificationResult = new TVerificationResult("Select an event to run the report against to.",
                    "No event was selected!",
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }

            if (!chkAccepted.Checked
                && !chkCancelled.Checked
                && !chkEnquiry.Checked
                && !chkHold.Checked
                && !chkRejected.Checked)
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
            txtEvent.Text = AParameters.Get("param_event_name").ToString();
            txtExtract.Text = AParameters.Get("param_extract_name").ToString();

            chkAccepted.Checked = AParameters.Get("param_application_status_accepted").ToBool();
            chkCancelled.Checked = AParameters.Get("param_application_status_cancelled").ToBool();
            chkHold.Checked = AParameters.Get("param_application_status_hold").ToBool();
            chkEnquiry.Checked = AParameters.Get("param_application_status_enquiry").ToBool();
            chkRejected.Checked = AParameters.Get("param_application_status_rejected").ToBool();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtEventSelectionChanged(object sender, EventArgs e)
        {
            txtEvent.Enabled = !rbtAllEvents.Checked;
            btnEvent.Enabled = !rbtAllEvents.Checked;
        }

        private void rbtParticipantsSelectionChanged(object sender, EventArgs e)
        {
            txtExtract.Enabled = rbtFromExtract.Checked;
        }

        private void btnEventClicked(object sender, EventArgs e)
        {
            // TODO open event selection dialog
        }
    }
}