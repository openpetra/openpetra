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
        private Boolean FReadOnly;

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
        }

        private void ValidateDataDetailsManual(AAccountingPeriodRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;
            AAccountingPeriodRow OtherRow;
            DataRow OtherDataRow;

            // first run through general checks related to the current AccountingPeriod row
            TSharedFinanceValidation_GLSetup.ValidateAccountingPeriod(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);

            // the following checks need to be done in this ManualCode file as they involve other rows on the screen:

            // check that there is no gap to previous accounting period
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
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PERIOD_DATE_RANGE,
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

            // check that there is no gap to next accounting period
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
                                    ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PERIOD_DATE_RANGE,
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