//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
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
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TUC_APLedgerSettings
    {
        private TFrmLedgerSettingsDialog FMainForm;
        private Int32 FLedgerNumber;
        private ALedgerInitFlagRow FRequireApprovalDataRow = null;

        /// <summary>
        /// Returns true if approval is required before posting
        /// </summary>
        public Boolean RequireApprovalBeforePosting
        {
            get
            {
                if ((FMainDS != null) && (FMainDS.ALedgerInitFlag != null))
                {
                    return FRequireApprovalDataRow != null;
                }
                else
                {
                    // If this exception fires it means that we have not yet initialised the dataset properly
                    throw new NullReferenceException("The 'InvoicesRequireApproval' property is invalid due to a Null Reference.");
                }
            }
        }

        /// <summary>
        /// Sets the local reference to the main form
        /// </summary>
        public TFrmLedgerSettingsDialog MainForm
        {
            set
            {
                FMainForm = value;
            }
        }

        /// <summary>
        /// This method is called as soon as the ledger number has been set on the main form, which follows immediately after the constructor.
        /// It is used to initialize the controls on the form, based on the values in FMainDS
        /// </summary>
        /// <param name="AMainForm"></param>
        /// <param name="ALedgerNumber"></param>
        public void InitializeScreenData(TFrmLedgerSettingsDialog AMainForm, Int32 ALedgerNumber)
        {
            FMainForm = AMainForm;
            FLedgerNumber = ALedgerNumber;

            // Now we can populate the controls with data
            // This form does not need to know about ALedgerRow (at present) so we can call ShowData with null as parameter
            ShowData(null);
        }

        // Normally this would be in the generated code, but this screen does not have any controls that are bound to this table (yet?)
        // If something changes and ShowData moves to generated code, this will become ShowDataManual
        private void ShowData(ALedgerRow ARow)
        {
            if ((FMainDS != null) && (FMainDS.ALedgerInitFlag != null))
            {
                FRequireApprovalDataRow = (ALedgerInitFlagRow)FMainDS.ALedgerInitFlag.Rows.Find(new object[] { FLedgerNumber, "AP_APPROVE_BLOCK" });
                chkRequireApproval.Checked = (FRequireApprovalDataRow != null);
            }
            else
            {
                throw new NullReferenceException("The GLSetupTDS dataset has not been initialised correctly");
            }
        }

        private void GetDataFromControlsManual(ALedgerRow ARow)
        {
            if ((FRequireApprovalDataRow != null) && !chkRequireApproval.Checked)
            {
                // Turn off the requirement for AP approval
                FRequireApprovalDataRow.Delete();
            }

            if (chkRequireApproval.Checked && (FRequireApprovalDataRow == null))
            {
                // Turn on requirement for AP approval
                FRequireApprovalDataRow = FMainDS.ALedgerInitFlag.NewRowTyped();
                FRequireApprovalDataRow.LedgerNumber = FLedgerNumber;
                FRequireApprovalDataRow.InitOptionName = "AP_APPROVE_BLOCK";
                FMainDS.ALedgerInitFlag.Rows.Add(FRequireApprovalDataRow);
            }
        }

        /// <summary>
        /// Gets the data from the controls on this page and returns true if there are no validation errors
        /// </summary>
        /// <returns></returns>
        public bool GetValidatedData()
        {
            GetDataFromControls((ALedgerRow)FMainDS.ALedger.Rows[0]);
            return true;
        }

        /// <summary>
        /// The data has been saved on the main form, either using the OK or the Apply button
        /// </summary>
        public void OnDataSaved()
        {
            // The data has been saved.  Not sure if we need to get a new reference to our data row of interest??
            FRequireApprovalDataRow = (ALedgerInitFlagRow)FMainDS.ALedgerInitFlag.Rows.Find(new object[] { FLedgerNumber, "AP_APPROVE_BLOCK" });
        }
    }
}