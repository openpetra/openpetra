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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonDialogs;

#region changelog

/*
 * Sort the grid: https://tracker.openpetra.org/view.php?id=5554 - Moray
 */
#endregion

namespace Ict.Petra.Client.MFinance.Gui.Setup.Gift
{
    public partial class TFrmMotivationGroupSetup
    {
        private Int32 FLedgerNumber;
        private string FDescription;

        /// <summary>
        /// maintain motivation details for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                FMainDS = TRemote.MFinance.Gift.WebConnectors.LoadMotivationDetails(FLedgerNumber);

                FMainDS.Merge(new GiftBatchTDS());

                // This code overrides what is in MotivationGroupSetup-generated.cs so we need to re-implement the sort too - Bug #5554
                DataView myDataView = FMainDS.AMotivationGroup.DefaultView;
                myDataView.Sort = AMotivationGroupTable.GetMotivationGroupCodeDBName() + " ASC";
                myDataView.AllowNew = false;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
                grdDetails.AutoSizeCells();

                this.Text = this.Text + "   [Ledger = " + FLedgerNumber.ToString() + "]";

                SelectRowInGrid(1);
                UpdateRecordNumberDisplay();

                FPetraUtilsObject.ApplySecurity(TSecurityChecks.SecurityPermissionsSetupScreensEditingAndSaving);
            }
        }

        private void InitializeManualCode()
        {
            // Get the current description
            FDescription = txtDetailMotivationGroupDescription.Text;
        }

        private void NewRowManual(ref AMotivationGroupRow ARow)
        {
            // Deal with primary key. MotivationGroupCode is unique and is 8 characters.
            string newName = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.AMotivationGroup.Rows.Find(new object[] { FLedgerNumber, newName }) != null)
            {
                while (FMainDS.AMotivationGroup.Rows.Find(new object[] { FLedgerNumber, newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.MotivationGroupCode = newName;
            ARow.LedgerNumber = FLedgerNumber;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewAMotivationGroup();
        }

        private TSubmitChangesResult StoreManualCode(ref GiftBatchTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            TSubmitChangesResult result = TRemote.MFinance.Gift.WebConnectors.SaveMotivationDetails(ref ASubmitChanges);

            if (result == TSubmitChangesResult.scrOK)
            {
                TDataCache.TMFinance.RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum.MotivationGroupList, FLedgerNumber);
            }

            return result;
        }

        // fired when tying in txtDetailMotivationDetailDesc
        private void DescriptionTyped(object sender, EventArgs e)
        {
            // syncs the two description text boxes if they should be synced
            if ((FDescription == txtDetailMotivationGroupDescLocal.Text) || string.IsNullOrEmpty(txtDetailMotivationGroupDescLocal.Text))
            {
                txtDetailMotivationGroupDescLocal.Text = txtDetailMotivationGroupDescription.Text;
            }

            FDescription = txtDetailMotivationGroupDescription.Text;
        }

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TFrmSelectPrintFields.SelectAndPrintGridFields(this, APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[]
                {
                    AMotivationGroupTable.ColumnMotivationGroupCodeId,
                    AMotivationGroupTable.ColumnMotivationGroupDescriptionId,
                    AMotivationGroupTable.ColumnMotivationGroupDescLocalId,
                    AMotivationGroupTable.ColumnGroupStatusId
                });
        }
    }
}