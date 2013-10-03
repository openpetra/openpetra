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
using System.IO;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Ict.Petra.Shared.MFinance;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Shared;


namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupAccountingPeriod
    {
        private Int32 FLedgerNumber;
        private int FNumberOfAccountingPeriods;
        private Boolean FReadOnly;
        private Boolean FDuringSave;

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

                // now merge account table into dataset as we need descriptions for account codes
                FMainDS.AAccountingPeriod.Merge(TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.AccountingPeriodList,
                        FLedgerNumber));

                LoadDataAndFinishScreenSetup();

                // activate filter so that forward periods are not shown (they need to be in dataset though as they will
                // be modified alongside the normal periods if dates change)
                FNumberOfAccountingPeriods = TRemote.MFinance.Setup.WebConnectors.NumberOfAccountingPeriods(FLedgerNumber);
                FMainDS.AAccountingPeriod.DefaultView.RowFilter = String.Format("{0}>={1} AND {0}<={2}",
                    AAccountingPeriodTable.GetAccountingPeriodNumberDBName(),
                    "1", FNumberOfAccountingPeriods);

                ReadOnly = !TRemote.MFinance.Setup.WebConnectors.IsCalendarChangeAllowed(FLedgerNumber);
            }
        }

        /// <summary>
        /// set read only if screen must not be modified
        /// </summary>
        public Boolean ReadOnly
        {
            get
            {
                return FReadOnly;
            }

            set
            {
                FReadOnly = value;

                txtDetailAccountingPeriodNumber.Enabled = !ReadOnly;
                txtDetailAccountingPeriodDesc.Enabled = !ReadOnly;
                dtpDetailPeriodStartDate.Enabled = !ReadOnly;
                dtpDetailPeriodEndDate.Enabled = !ReadOnly;
            }
        }

        private void InitializeManualCode()
        {
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(DataSavingStarted);
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(DataSaved);
        }

        private void DataSavingStarted(System.Object obj, EventArgs e)
        {
            AAccountingPeriodRow FwdPeriodRow;
            AAccountingPeriodRow PeriodRow;

            // make sure that changes in period dates are applied to existing forward periods
            foreach (DataRow row in FMainDS.AAccountingPeriod.Rows)
            {
                FwdPeriodRow = (AAccountingPeriodRow)row;

                if (FwdPeriodRow.AccountingPeriodNumber > FNumberOfAccountingPeriods)
                {
                    PeriodRow =
                        (AAccountingPeriodRow)FMainDS.AAccountingPeriod.Rows.Find(new object[] { FLedgerNumber,
                                                                                                 (FwdPeriodRow.AccountingPeriodNumber -
                                                                                                  FNumberOfAccountingPeriods) });
                    FwdPeriodRow.PeriodStartDate = PeriodRow.PeriodStartDate.AddYears(1);
                    FwdPeriodRow.PeriodEndDate = PeriodRow.PeriodEndDate.AddYears(1);
                }
            }

            // set flag used during validation
            FDuringSave = true;
        }

        private void DataSaved(System.Object obj, EventArgs e)
        {
            // reset flag used during validation
            FDuringSave = false;
        }

        private void ValidateDataDetailsManual(AAccountingPeriodRow ARow)
        {
            if (FDuringSave)
            {
                AAccountingPeriodRow PeriodRow;

                foreach (DataRow row in FMainDS.AAccountingPeriod.Rows)
                {
                    PeriodRow = (AAccountingPeriodRow)row;

                    if (PeriodRow.AccountingPeriodNumber <= FNumberOfAccountingPeriods)
                    {
                        // only check gap to next record as otherwise messages would appear twice
                        ValidateRecord(PeriodRow, false, true);
                    }
                }
            }
            else
            {
                // if not within save process then
                ValidateRecord(ARow, true, true);
            }
        }

        private void ValidateRecord(AAccountingPeriodRow ARow, Boolean ACheckGapToPrevious, Boolean ACheckGapToNext)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;
            AAccountingPeriodRow OtherRow;
            DataRow OtherDataRow;

            string CurrentDateRangeErrorCode;

            if (FDuringSave)
            {
                CurrentDateRangeErrorCode = PetraErrorCodes.ERR_PERIOD_DATE_RANGE;
            }
            else
            {
                CurrentDateRangeErrorCode = PetraErrorCodes.ERR_PERIOD_DATE_RANGE_WARNING;
            }

            // first run through general checks related to the current AccountingPeriod row
            TSharedFinanceValidation_GLSetup.ValidateAccountingPeriod(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);

            // the following checks need to be done in this ManualCode file as they involve other rows on the screen:

            // check that there is no gap to previous accounting period
            if (ACheckGapToPrevious)
            {
                ValidationColumn = ARow.Table.Columns[AAccountingPeriodTable.ColumnPeriodStartDateId];

                if (FPetraUtilsObject.ValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    if (!ARow.IsPeriodStartDateNull()
                        && (ARow.PeriodStartDate != DateTime.MinValue))
                    {
                        OtherDataRow = FMainDS.AAccountingPeriod.Rows.Find(
                            new string[] { FLedgerNumber.ToString(), (ARow.AccountingPeriodNumber - 1).ToString() });

                        if (OtherDataRow != null)
                        {
                            OtherRow = (AAccountingPeriodRow)OtherDataRow;

                            if (OtherRow.PeriodEndDate != ARow.PeriodStartDate.Date.AddDays(-1))
                            {
                                VerificationResult = new TScreenVerificationResult(new TVerificationResult(this,
                                        ErrorCodes.GetErrorInfo(CurrentDateRangeErrorCode,
                                            new string[] { (ARow.AccountingPeriodNumber - 1).ToString() })),
                                    ValidationColumn, ValidationControlsData.ValidationControl);
                            }
                            else
                            {
                                VerificationResult = null;
                            }

                            // Handle addition/removal to/from TVerificationResultCollection
                            VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn);
                        }
                    }
                }
            }

            // check that there is no gap to next accounting period
            if (ACheckGapToNext)
            {
                ValidationColumn = ARow.Table.Columns[AAccountingPeriodTable.ColumnPeriodEndDateId];

                if (FPetraUtilsObject.ValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    if (!ARow.IsPeriodEndDateNull()
                        && (ARow.PeriodEndDate != DateTime.MinValue))
                    {
                        OtherDataRow = FMainDS.AAccountingPeriod.Rows.Find(
                            new string[] { FLedgerNumber.ToString(), (ARow.AccountingPeriodNumber + 1).ToString() });

                        if (OtherDataRow != null)
                        {
                            OtherRow = (AAccountingPeriodRow)OtherDataRow;

                            if (OtherRow.PeriodStartDate != ARow.PeriodEndDate.Date.AddDays(1))
                            {
                                VerificationResult = new TScreenVerificationResult(new TVerificationResult(this,
                                        ErrorCodes.GetErrorInfo(CurrentDateRangeErrorCode,
                                            new string[] { (ARow.AccountingPeriodNumber).ToString() })),
                                    ValidationColumn, ValidationControlsData.ValidationControl);
                            }
                            else
                            {
                                VerificationResult = null;
                            }

                            // Handle addition/removal to/from TVerificationResultCollection
                            VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn);
                        }
                    }
                }
            }
        }
    }
}