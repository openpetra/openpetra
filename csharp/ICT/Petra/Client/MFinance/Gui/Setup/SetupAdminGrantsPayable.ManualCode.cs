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
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict.Petra.Shared.MFinance;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonDialogs;


namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupAdminGrantsPayable
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// The applicable Ledger Number
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
                TSetupAdminGrants.PopulateComboBoxes(cmbDetailCostCentreCode, cmbDetailAccountCode, cmbDetailDrAccountCode, FLedgerNumber, false);
                LoadDataAndFinishScreenSetup();
            }
        }

        #region Extra Data Set for Fees receivable

        private TExtraDS FExtraDS = new TExtraDS();
        private class TExtraDS : TTypedDataSet
        {
            private AFeesReceivableTable FFeesReceivableTable;

            public AFeesReceivableTable AFeesReceivable
            {
                get
                {
                    return this.FFeesReceivableTable;
                }
            }

            protected override void InitTables()
            {
                this.Tables.Add(new AFeesReceivableTable("AFeesReceivable"));
            }

            protected override void InitTables(System.Data.DataSet ds)
            {
                if ((ds.Tables.IndexOf("AFeesReceivable") != -1))
                {
                    this.Tables.Add(new AFeesReceivableTable("AFeesReceivable"));
                }
            }

            protected override void MapTables()
            {
                this.InitVars();
                base.MapTables();

                if ((this.FFeesReceivableTable != null))
                {
                    this.FFeesReceivableTable.InitVars();
                }
            }

            public override void InitVars()
            {
                this.DataSetName = "PrivateScreenTDS";
                this.FFeesReceivableTable = ((AFeesReceivableTable)(this.Tables["AFeesReceivable"]));
            }

            protected override void InitConstraints()
            {
            }
        }

        #endregion

        private void RunOnceOnActivationManual()
        {
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);

            // Load the data from the Fees Receivable cached table
            Type DataTableType;
            DataTable CacheDT = TDataCache.GetSpecificallyFilteredCacheableDataTableFromCache("FeesReceivableList",
                "Ledger",
                FFilter,
                out DataTableType);
            FExtraDS.AFeesReceivable.Merge(CacheDT);
            FExtraDS.AFeesReceivable.DefaultView.Sort = String.Format("{0}, {1} ASC",
                AFeesReceivableTable.GetLedgerNumberDBName(), AFeesReceivableTable.GetFeeCodeDBName());

            ALedgerRow LedgerRow =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber))[0];
            txtDetailChargeAmount.CurrencyCode = LedgerRow.BaseCurrency;

            SelectRowInGrid(1);
            UpdateRecordNumberDisplay();
        }

        void FPetraUtilsObject_DataSaved(object Sender, TDataSavedEventArgs e)
        {
            if (!e.Success)
            {
                string msg = Catalog.GetString(
                    "The data was not saved successfully.  If the reason given was that you, or another user, has modified the same data, please check that you have not used the same fee code for a 'Payable' and a 'Receivable' Administration Grant.");
                MessageBox.Show(msg, Catalog.GetString("Additional Help For Administration Grants"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void NewRow(System.Object sender, EventArgs e)
        {
            CreateNewAFeesPayable();
        }

        private void NewRowManual(ref AFeesPayableRow ARow)
        {
            ARow.LedgerNumber = FLedgerNumber;
            string newName = Ict.Common.Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.AFeesPayable.Rows.Find(new object[] { FLedgerNumber, newName }) != null)
            {
                while (FMainDS.AFeesPayable.Rows.Find(new object[] { FLedgerNumber, newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.FeeCode = newName;
            ARow.AccountCode = MFinanceConstants.ADMIN_FEE_INCOME_ACCT.ToString();
            ARow.DrAccountCode = MFinanceConstants.ADMIN_FEE_EXPENSE_ACCT.ToString();
        }

        private void ValidateDataDetailsManual(AFeesPayableRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_GLSetup.ValidateAdminGrantPayable(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);

            // Need to check the Fee Code has not been used in FeesReceivable
            ValidateFeeCode(ARow.FeeCode, VerificationResultCollection);
        }

        private void ValidateFeeCode(string AFeeCode, TVerificationResultCollection AVerificationResultCollection)
        {
            TScreenVerificationResult result = null;
            string context = "PayableCrossCodeCheck";

            if (FExtraDS.AFeesReceivable.DefaultView.Find(new object[] { LedgerNumber, AFeeCode }) >= 0)
            {
                // oops - we have this code in the other data set
                result = new TScreenVerificationResult(context,
                    FMainDS.AFeesPayable.ColumnFeeCode,
                    Catalog.GetString("The Fee Code has already been used as a Fee Code in the 'Receivable Administration Grants' screen"),
                    CommonErrorCodes.ERR_DUPLICATE_RECORD,
                    txtDetailFeeCode,
                    TResultSeverity.Resv_Critical);
            }

            AVerificationResultCollection.Auto_Add_Or_AddOrRemove(context, result, FMainDS.AFeesPayable.ColumnFeeCode);
        }

        private void ChargeOptionChanged(object sender, EventArgs e)
        {
            TSetupAdminGrants.ChargeOptionComboChanged((TCmbAutoComplete)sender, lblDetailChargeAmount,
                txtDetailChargeAmount, txtDetailChargePercentage);
        }

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TFrmSelectPrintFields.SelectAndPrintGridFields(this, APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[]
                {
                    AFeesPayableTable.ColumnFeeCodeId,
                    AFeesPayableTable.ColumnFeeDescriptionId,
                    AFeesPayableTable.ColumnChargeOptionId,
                    AFeesPayableTable.ColumnChargePercentageId,
                    AFeesPayableTable.ColumnChargeAmountId,
                    AFeesPayableTable.ColumnDrAccountCodeId,
                    AFeesPayableTable.ColumnCostCentreCodeId,
                    AFeesPayableTable.ColumnAccountCodeId
                });
        }
    }
}