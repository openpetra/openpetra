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
using System.IO;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Ict.Petra.Shared.MFinance;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Validation;


namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupAdminGrantsReceivable
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// The applicable Ledger number
        /// </summary>
        public Int32 LedgerNumber
        {
            get
            {
                return FLedgerNumber;
            }

            set
            {
                FLedgerNumber = value;
                FFilter = FLedgerNumber;
                TSetupAdminGrants.PopulateComboBoxes(cmbDetailCostCentreCode, cmbDetailAccountCode, cmbDetailDrAccountCode, FLedgerNumber, true);
                LoadDataAndFinishScreenSetup();

            }
        }

        private void NewRow(System.Object sender, EventArgs e)
        {
            CreateNewAFeesReceivable();
        }

        private void NewRowManual(ref AFeesReceivableRow ARow)
        {
            ARow.LedgerNumber = FLedgerNumber;
            string newName = Ict.Common.Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.AFeesReceivable.Rows.Find(new object[] { FLedgerNumber, newName }) != null)
            {
                while (FMainDS.AFeesReceivable.Rows.Find(new object[] { FLedgerNumber, newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.FeeCode = newName;
            ARow.AccountCode = MFinanceConstants.ADMIN_FEE_INCOME_ACCT.ToString();
            ARow.DrAccountCode = MFinanceConstants.ADMIN_FEE_EXPENSE_ACCT.ToString();
        }

        private void ValidateDataDetailsManual(AFeesReceivableRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_GLSetup.ValidateAdminGrantReceivable(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }

        private void ChargeOptionChanged(object sender, EventArgs e)
        {
            TSetupAdminGrants.ChargeOptionComboChanged((TCmbAutoComplete)sender, lblDetailChargeAmount,
                txtDetailChargeAmount, txtDetailChargePercentage);
        }

        private void SelectByIndex(int rowIndex)
        {
            if (rowIndex >= grdDetails.Rows.Count)
            {
                rowIndex = grdDetails.Rows.Count - 1;
            }

            if ((rowIndex < 1) && (grdDetails.Rows.Count > 1))
            {
                rowIndex = 1;
            }

            if ((rowIndex >= 1) && (grdDetails.Rows.Count > 1))
            {
                grdDetails.Selection.SelectRow(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);
            }
            else
            {
                FPreviouslySelectedDetailRow = null;
            }
        }
    }
}