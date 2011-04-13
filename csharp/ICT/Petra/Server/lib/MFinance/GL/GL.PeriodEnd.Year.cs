//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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
using System;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using Ict.Petra.Server.App.Core.Security;


using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;


using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.GL;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;


namespace Ict.Petra.Server.MFinance.GL.WebConnectors
{
    public partial class TPeriodIntervallConnector
    {
        /// <summary>
        /// Routine to initialize the "Hello" Message if you want to start the
        /// periodic month end.
        /// </summary>
        /// <param name="ALedgerNum"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>True if critical values appeared otherwise false</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool TPeriodYearEndInfo(
            int ALedgerNum,
            out TVerificationResultCollection AVerificationResult)
        {
            return new TYearEnd().RunYearEndInfo(ALedgerNum, out AVerificationResult);
        }

        /// <summary>
        /// Routine to run the finally month end ...
        /// </summary>
        /// <param name="ALedgerNum"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool TPeriodYearEnd(
            int ALedgerNum,
            out TVerificationResultCollection AVerificationResult)
        {
            return new TYearEnd().RunYearEnd(ALedgerNum, false, out AVerificationResult);
        }
    }
}

namespace Ict.Petra.Server.MFinance.GL
{
    public class TYearEnd
    {
        bool blnReentranceMode;
        bool blnCriticalErrors;
        TVerificationResultCollection verificationResults;
        THandleLedgerInfo ledgerInfo;
        TLedgerLock ledgerLock;

        public bool RunYearEndInfo(int ALedgerNum,
            out TVerificationResultCollection AVRCollection)
        {
            blnReentranceMode = false;
            DBAccess.GDBAccessObj.CommitTransaction();
            verificationResults = new TVerificationResultCollection();
            try
            {
                ledgerLock = new TLedgerLock(ALedgerNum);
                YearEndMainInfo(ALedgerNum);
                AVRCollection = verificationResults;
                ledgerLock.UnLock();
                System.Diagnostics.Debug.WriteLine("try blnCriticalErrors: " + blnCriticalErrors.ToString());
                return blnCriticalErrors;
            }
            catch (TerminateException terminate)
            {
                AVRCollection = terminate.ResultCollection();
                ledgerLock.UnLock();
                System.Diagnostics.Debug.WriteLine("cath blnCriticalErrors: ");
                return true;
            }
        }

        public bool RunYearEnd(int ALedgerNum, bool AReentrance,
            out TVerificationResultCollection AVRCollection)
        {
            blnReentranceMode = AReentrance;
            DBAccess.GDBAccessObj.CommitTransaction();
            verificationResults = new TVerificationResultCollection();
            try
            {
                ledgerLock = new TLedgerLock(ALedgerNum);
                YearEndMain(ALedgerNum);
                AVRCollection = verificationResults;
                ledgerLock.UnLock();
                return blnCriticalErrors;
            }
            catch (TerminateException terminate)
            {
                AVRCollection = terminate.ResultCollection();
                ledgerLock.UnLock();
                return true;
            }
        }

        private void YearEndMainInfo(int ALedgerNum)
        {
            ledgerInfo = new THandleLedgerInfo(ALedgerNum);
            verificationResults = new TVerificationResultCollection();
            CheckLedger();
        }

        private void YearEndMain(int ALedgerNum)
        {
            if (!blnReentranceMode)
            {
                ledgerInfo.YearEndProcessStatus = int.Parse(YearEndProcessStatus.RESET_STATUS.ToString());
            }

            ledgerInfo = new THandleLedgerInfo(ALedgerNum);
            verificationResults = new TVerificationResultCollection();
            CheckLedger();

            if (ledgerInfo.YearEndProcessStatus < int.Parse(YearEndProcessStatus.GIFT_CLOSED_OUT.ToString()))
            {
                CloseGifts();
            }

            if (ledgerInfo.YearEndProcessStatus < int.Parse(YearEndProcessStatus.ACCOUNT_CLOSED_OUT.ToString()))
            {
                CloseAccounts();
            }

            if (ledgerInfo.YearEndProcessStatus < int.Parse(YearEndProcessStatus.GLMASTER_CLOSED_OUT.ToString()))
            {
                CloseGLMaster();
            }

            if (ledgerInfo.YearEndProcessStatus < int.Parse(YearEndProcessStatus.BUDGET_CLOSED_OUT.ToString()))
            {
                CloseBudget();
            }

            if (ledgerInfo.YearEndProcessStatus < int.Parse(YearEndProcessStatus.PERIODS_UPDATED.ToString()))
            {
                UpdatePeriods();
            }

            if (ledgerInfo.YearEndProcessStatus < int.Parse(YearEndProcessStatus.SET_NEW_YEAR.ToString()))
            {
                SetNewYear();
            }

            if (ledgerInfo.YearEndProcessStatus < int.Parse(YearEndProcessStatus.LEDGER_UPDATED.ToString()))
            {
                Finish();
            }
        }

        private void CheckLedger()
        {
            if (!ledgerLock.IsLocked)
            {
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("Leger is locked ..."),
                        String.Format(Catalog.GetString("Ledger is locked by the user {0}"),
                            ledgerLock.LockInfo()), "",
                        PeriodEndYearStatus.PEYM_01.ToString(),
                        TResultSeverity.Resv_Critical);
                verificationResults.Add(tvt);
                blnCriticalErrors = true;
            }

            if (!ledgerInfo.ProvisionalYearEndFlag)
            {
                TVerificationResult tvt =
                    new TVerificationResult(Catalog.GetString("YearEndFlag is set "),
                        Catalog.GetString("In this situation you cannot run a year end routine"), "",
                        PeriodEndYearStatus.PEYM_02.ToString(),
                        TResultSeverity.Resv_Critical);
                verificationResults.Add(tvt);
                blnCriticalErrors = true;
                System.Diagnostics.Debug.WriteLine("CheckLedger");
            }
        }

        /// <summary>
        /// Actual there are only retention jobs in this routine ...
        /// </summary>
        private void CloseGifts()
        {
            ledgerInfo.YearEndProcessStatus = (int)YearEndProcessStatus.GIFT_CLOSED_OUT;
        }

        /// <summary>
        /// Actual there are only retention jobs in this routine ...
        /// </summary>
        private void CloseAccounts()
        {
            ledgerInfo.YearEndProcessStatus = (int)YearEndProcessStatus.ACCOUNT_CLOSED_OUT;
        }

        private void CloseGLMaster()
        {
            GetAccountInfo accountInfo = new GetAccountInfo(ledgerInfo);

            accountInfo.Reset();

            while (accountInfo.MoveNext())
            {
                if (accountInfo.PostingStatus)
                {
                }
            }
        }

        private void CloseBudget()
        {
        }

        private void UpdatePeriods()
        {
        }

        private void SetNewYear()
        {
        }

        private void Finish()
        {
        }
    }


    public enum PeriodEndYearStatus
    {
        /// <summary>
        /// The Leger is locked ...
        /// </summary>
        PEYM_01,
        /// <summary>
        /// Openpetra is not in the modus to run a year end procedure.
        /// This is only allowed if you have run the last mont end period of the year
        /// </summary>
        PEYM_02
    }

    /// <summary>
    /// This is the list of status values of a_ledger.a_year_end_process_status_i which has been
    /// copied from petra. The status begins by counting from RESET_Status up to LEDGER_UPDATED
    /// and each higher level status includes the lower level ones.
    /// </summary>
    public enum YearEndProcessStatus
    {
        /// <summary>
        /// Value the status counter is initialized with
        /// </summary>
        RESET_STATUS = 0,

        GIFT_CLOSED_OUT = 1,
        ACCOUNT_CLOSED_OUT = 2,
        GLMASTER_CLOSED_OUT = 3,
        BUDGET_CLOSED_OUT = 4,
        PERIODS_UPDATED = 7,
        SET_NEW_YEAR = 8,

        /// <summary>
        /// The leger is completely updated ...
        /// </summary>
        LEDGER_UPDATED = 10
    }
}