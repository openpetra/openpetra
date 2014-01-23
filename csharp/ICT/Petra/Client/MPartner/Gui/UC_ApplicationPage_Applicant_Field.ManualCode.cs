//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Validation;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_ApplicationPage_Applicant_Field
    {
        // <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        //private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        #region Public Methods

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        private void RethrowRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            OnRecalculateScreenParts(e);
        }

        /// <summary>
        /// Display data in control based on data from ARow
        /// </summary>
        /// <param name="ARow"></param>
        public void ShowDetails(PmGeneralApplicationRow ARow)
        {
            ShowData(ARow);
            EnableDisableStatusRelatedDateFields(null, null);
            EnableDisableReceivingFieldAcceptanceDate(null, null);
            ApplicationCurrencyChanged(null, null);
        }

        /// <summary>
        /// Read data from controls into ARow parameter
        /// </summary>
        /// <param name="ARow"></param>
        public void GetDetails(PmGeneralApplicationRow ARow)
        {
            GetDataFromControls(ARow);
        }

        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        #endregion

        #region Private Methods

        private void InitializeManualCode()
        {
        }

        private void GetDataFromControlsManual(PmGeneralApplicationRow ARow)
        {
        }

        private void ValidateDataManual(PmGeneralApplicationRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPersonnelValidation_Personnel.ValidateGeneralApplicationManual(this, ARow, false, ref VerificationResultCollection,
                FValidationControlsDict);

            TSharedPersonnelValidation_Personnel.ValidateFieldApplicationManual(this,
                FMainDS.PmYearProgramApplication[0],
                ref VerificationResultCollection,
                FValidationControlsDict);
        }

        private void EnableDisableStatusRelatedDateFields(Object sender, EventArgs e)
        {
            String ApplicationStatus = cmbApplicationStatus.GetSelectedString();

            dtpCancellationDate.Enabled = false;
            dtpAcceptanceDate.Enabled = false;

            if (ApplicationStatus != "")
            {
                if (ApplicationStatus.StartsWith("A"))
                {
                    dtpAcceptanceDate.Enabled = true;
                    dtpCancellationDate.Enabled = false;
                }
                else if (ApplicationStatus.StartsWith("C")
                         || ApplicationStatus.StartsWith("R"))
                {
                    dtpAcceptanceDate.Enabled = false;
                    dtpCancellationDate.Enabled = true;
                }
            }
        }

        private void EnableDisableReceivingFieldAcceptanceDate(Object sender, EventArgs e)
        {
            dtpFieldAcceptance.Enabled = chkAcceptedByReceivingField.Checked;
        }

        private void ApplicationCurrencyChanged(Object sender, EventArgs e)
        {
            String Currency;

            Currency = cmbApplicationCurrency.GetSelectedString();

            txtJoiningCharge.CurrencyCode = Currency;
            txtAgreedSupport.CurrencyCode = Currency;
        }

        private void ApplicationStatusChanged(object sender, EventArgs e)
        {
            EnableDisableStatusRelatedDateFields(sender, e);

            if (dtpCancellationDate.Enabled)
            {
                if (dtpCancellationDate.TextLength == 0)
                {
                    dtpCancellationDate.Date = DateTime.Now.Date;
                }
            }
            else
            {
                dtpCancellationDate.Date = null;
            }

            if (dtpAcceptanceDate.Enabled)
            {
                if (dtpAcceptanceDate.TextLength == 0)
                {
                    dtpAcceptanceDate.Date = DateTime.Now.Date;
                }
            }
            else
            {
                dtpAcceptanceDate.Date = null;
            }
        }

        private void ReceivingFieldAcceptanceChanged(object sender, EventArgs e)
        {
            EnableDisableReceivingFieldAcceptanceDate(sender, e);

            if (!chkAcceptedByReceivingField.Checked)
            {
                dtpFieldAcceptance.Date = null;
            }
            else
            {
                dtpFieldAcceptance.Date = DateTime.Now.Date;
            }
        }

        #endregion
    }
}