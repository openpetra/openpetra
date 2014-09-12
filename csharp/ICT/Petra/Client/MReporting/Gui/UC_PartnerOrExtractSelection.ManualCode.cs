//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, andreww
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
using GNU.Gettext;
using Ict.Common;
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
    public partial class TFrmUC_PartnerOrExtractSelection
    {
        /// <summary>
        /// Initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FPetraUtilsObject = APetraUtilsObject;

            rbtPartner.Checked = true;
            txtExtract.Enabled = false;
        }

        /// <summary>
        /// Sets the Restricted Partner Classes in the modal Find Partner Screen
        /// </summary>
        /// <param name="ARestrictedPartnerClasses">RestrictedPartnerClasses in form - "PartnerClass1,PartnerClass2, ... etc"</param>
        public void SetRestrictedPartnerClasses(string ARestrictedPartnerClasses)
        {
            // add '*' if more than one restrictedpartner class
            string[] Classes = ARestrictedPartnerClasses.Split(new Char[] { (',') });

            if (Classes.Length > 1)
            {
                ARestrictedPartnerClasses = "*," + ARestrictedPartnerClasses;
            }

            txtPartnerKey.PartnerClass = ARestrictedPartnerClasses;
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
            if (rbtPartner.Checked)
            {
                ACalculator.AddParameter("param_selection", "one partner");
            }
            else if (rbtExtract.Checked)
            {
                ACalculator.AddParameter("param_selection", "an extract");
            }

            ACalculator.AddParameter("param_extract", txtExtract.Text);
            ACalculator.AddParameter("param_partnerkey", txtPartnerKey.Text);

            if ((AReportAction == TReportActionEnum.raGenerate)
                && rbtPartner.Checked
                && (txtPartnerKey.Text == "0000000000"))
            {
                TVerificationResult VerificationMessage = new TVerificationResult(
                    Catalog.GetString("Enter a valid Partner Key."),
                    Catalog.GetString("No Partner Key entered!"),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationMessage);
            }

            if ((AReportAction == TReportActionEnum.raGenerate)
                && rbtExtract.Checked
                && (txtExtract.Text == ""))
            {
                TVerificationResult VerificationMessage = new TVerificationResult(
                    Catalog.GetString("Enter an extract name."),
                    Catalog.GetString("No extract name entered!"), TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationMessage);
            }
        }

        /// <summary>
        /// initialise the controls using the parameters
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControls(TParameterList AParameters)
        {
            rbtPartner.Checked = (AParameters.Get("param_selection").ToString() == "one partner");
            rbtExtract.Checked = (AParameters.Get("param_selection").ToString() == "an extract");

            txtPartnerKey.Text = AParameters.Get("param_partnerkey").ToString();
            txtExtract.Text = AParameters.Get("param_extract").ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbtSelectionChange(object sender, EventArgs e)
        {
            RadioButton RBtn = sender as RadioButton;

            if (RBtn.Name == "rbtPartner")
            {
                txtPartnerKey.Enabled = RBtn.Checked;
            }
            else if (RBtn.Name == "rbtExtract")
            {
                txtExtract.Enabled = RBtn.Checked;
            }
        }
    }
}