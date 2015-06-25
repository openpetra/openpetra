//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
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

using System;
using System.Data;
using System.Data.Odbc;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Server.MCommon.Data.Access;
using System.Diagnostics;

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// Handling the information of the General Legder Master Table Period Table ...
    /// </summary>
    public class TGlmpInfo
    {
        AGeneralLedgerMasterPeriodTable FGLMpTable;
        AGeneralLedgerMasterPeriodRow FGLMpRow;
        Int32 FLedgerNumber = -1;

        /// <summary>
        /// Instead of having several constructors, this single constructor now been separated from the initialisation methods.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public TGlmpInfo(Int32 ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;
        }

        /// <summary>
        /// Load a single row by sequence and period
        /// </summary>
        /// <returns>True if it seemed to work</returns>
        public Boolean LoadBySequence(Int32 ASequence, Int32 APeriod)
        {
            Boolean LoadedOk = false;

            if (ASequence != -1)
            {
                bool NewTransaction = false;

                try
                {
                    TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                        TEnforceIsolationLevel.eilMinimum,
                        out NewTransaction);

                    FGLMpTable = AGeneralLedgerMasterPeriodAccess.LoadByPrimaryKey(ASequence, APeriod, transaction);
                    LoadedOk = FGLMpTable.Rows.Count > 0;
                    FGLMpRow = (LoadedOk) ? FGLMpTable[0] : null;
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
            }

            return LoadedOk;
        }

        /// <summary>
        /// Load all GLMP rows for this Cost Centre in this period
        /// </summary>
        /// <returns></returns>
        public Boolean LoadByCostCentreAccountPeriod(String ACostCentreCode, String AAccountCode, Int32 AYear, Int32 APeriod)
        {
            Boolean LoadedOk = false;

            TDBTransaction transaction = null;

            FGLMpTable = new AGeneralLedgerMasterPeriodTable();
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, ref transaction,
                delegate
                {
                    DBAccess.GDBAccessObj.SelectDT(
                        FGLMpTable,
                        "SELECT a_general_ledger_master_period.* FROM" +
                        " a_general_ledger_master_period, a_general_ledger_master" +
                        " WHERE" +
                        " a_general_ledger_master_period.a_glm_sequence_i=a_general_ledger_master.a_glm_sequence_i" +
                        " AND a_general_ledger_master.a_ledger_number_i = " + FLedgerNumber +
                        " AND a_general_ledger_master.a_year_i = " + AYear +
                        " AND a_general_ledger_master.a_account_code_c = '" + AAccountCode + "'" +
                        " AND a_general_ledger_master.a_cost_centre_code_c = '" + ACostCentreCode + "'" +
                        " AND a_general_ledger_master_period.a_period_number_i=" + APeriod,
                        transaction);
                    LoadedOk = (FGLMpTable.Rows.Count > 0);
                });  // Get NewOrExisting AutoReadTransaction

            FGLMpRow = (LoadedOk) ? FGLMpTable[0] : null;
            return LoadedOk;
        }

        /// <summary>
        /// Check if a row was returned.
        /// </summary>
        public bool RowExists
        {
            get
            {
                return FGLMpRow != null;
            }
        }

        /// <summary>
        /// Loads <b>all</b> GLMP data for the selected year
        /// </summary>
        public void LoadByYear(Int32 AYear)
        {
            bool NewTransaction = false;

            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                AGeneralLedgerMasterTable GLMTemplateTbl = new AGeneralLedgerMasterTable();
                AGeneralLedgerMasterRow GLMTemplateRow = GLMTemplateTbl.NewRowTyped(false);
                GLMTemplateRow.LedgerNumber = FLedgerNumber;
                GLMTemplateRow.Year = AYear;

                FGLMpTable = AGeneralLedgerMasterPeriodAccess.LoadViaAGeneralLedgerMasterTemplate(GLMTemplateRow, transaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
        }

        /// <summary>
        /// Returns the data base row value
        /// </summary>
        public decimal ActualBase
        {
            get
            {
                return (FGLMpRow == null) ? 0 : FGLMpRow.ActualBase;
            }
        }
    }


    /// <summary>
    /// Object to handle the read only glm-infos ...
    /// </summary>
    public class TGet_GLM_Info
    {
        AGeneralLedgerMasterTable FGLMTbl;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACurrentFinancialYear">Number of the year after the the installation of the software</param>
        public TGet_GLM_Info(int ALedgerNumber, string AAccountCode, int ACurrentFinancialYear)
        {
            bool NewTransaction = false;

            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                FGLMTbl = new AGeneralLedgerMasterTable();
                AGeneralLedgerMasterRow GLMTemplateRow = FGLMTbl.NewRowTyped(false);
                GLMTemplateRow.LedgerNumber = ALedgerNumber;
                GLMTemplateRow.AccountCode = AAccountCode;
                GLMTemplateRow.Year = ACurrentFinancialYear;
                FGLMTbl = AGeneralLedgerMasterAccess.LoadUsingTemplate(GLMTemplateRow, transaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
        }

        /// <summary>
        /// THIS METHOD DOES NOT RETURN USEFUL VALUES because the Year is not specified.
        /// It is only called from tests, and those tests pass,
        /// because there's no previous financial year in the database,
        /// because the data returned by this method is from the earliest year in the ledger.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACostCentreCode"></param>
        public TGet_GLM_Info(int ALedgerNumber, string AAccountCode, string ACostCentreCode)
        {
            bool NewTransaction = false;

            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                FGLMTbl = new AGeneralLedgerMasterTable();
                AGeneralLedgerMasterRow GLMTemplateRow = FGLMTbl.NewRowTyped(false);
                GLMTemplateRow.LedgerNumber = ALedgerNumber;
                GLMTemplateRow.AccountCode = AAccountCode;
                GLMTemplateRow.CostCentreCode = ACostCentreCode;
                FGLMTbl = AGeneralLedgerMasterAccess.LoadUsingTemplate(GLMTemplateRow, transaction);

                if (FGLMTbl.Rows.Count == 0)
                {
//                    String msg = TLogging.StackTraceToText(new StackTrace(true));
                    String msg = "";

                    TLogging.Log(String.Format("ERROR: No TGet_GLM_Info row found for ({0}, {1}).",
                            ACostCentreCode, AAccountCode, msg));
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
        }

        /// <summary>
        /// Indicates whether GLM record with given parameters actually exists.
        /// </summary>
        public bool GLMExists
        {
            get
            {
                if ((FGLMTbl == null) || (FGLMTbl.Rows.Count == 0))
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Reads the value of YtdActual and supports a default value of 0 if it not exists.
        /// </summary>
        public decimal YtdActual
        {
            get
            {
                if ((FGLMTbl.Rows.Count == 0) || FGLMTbl[0].IsYtdActualBaseNull())
                {
                    TLogging.Log("TGet_GLM_Info.YtdActual not available.");
                    return 0;
                }

                return FGLMTbl[0].YtdActualBase;
            }
        }

        /// <summary>
        /// Reads the value of YtdForeign and supports a default value of 0 if it not exists.
        /// </summary>
        public decimal YtdForeign
        {
            get
            {
                if ((FGLMTbl.Rows.Count == 0) || FGLMTbl[0].IsYtdActualForeignNull())
                {
                    TLogging.Log("TGet_GLM_Info.YtdForeign not available.");
                    return 0;
                }

                return FGLMTbl[0].YtdActualForeign;
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public int Sequence
        {
            get
            {
                return FGLMTbl[0].GlmSequence;
            }
        }
    }  // TGet_GLM_Info

    /// <summary>
    /// GLM-Info but an other handling than TGet_GLM_Info
    /// </summary>
    public class TGlmInfo
    {
        AGeneralLedgerMasterTable FGLMTbl;
        AGeneralLedgerMasterRow FglmRow;
        int iPtr = -1;

        /// <summary>
        /// Loads only GLM Data selected by Ledger Number, Year and Account Code ...
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ACurrentFinancialYear"></param>
        /// <param name="AAccountCode"></param>
        public TGlmInfo(int ALedgerNumber, int ACurrentFinancialYear, string AAccountCode)
        {
            bool NewTransaction = false;

            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                FGLMTbl = new AGeneralLedgerMasterTable();
                AGeneralLedgerMasterRow GLMTemplateRow = FGLMTbl.NewRowTyped(false);
                GLMTemplateRow.LedgerNumber = ALedgerNumber;
                GLMTemplateRow.AccountCode = AAccountCode;
                GLMTemplateRow.Year = ACurrentFinancialYear;
                FGLMTbl = AGeneralLedgerMasterAccess.LoadUsingTemplate(GLMTemplateRow, transaction);
                TLogging.Log(
                    "TGlmInfo(" + ALedgerNumber + ", " + ACurrentFinancialYear + ", " + AAccountCode + ") has loaded " + FGLMTbl.Rows.Count +
                    " Rows.");
                iPtr = -1;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
        }

        /// <summary>
        /// Set the iterator to the top
        /// </summary>
        public void Reset()
        {
            iPtr = -1;
        }

        /// <summary>
        /// Next Element of the Row list ...
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (++iPtr >= FGLMTbl.Rows.Count)
            {
                return false;
            }

            FglmRow = (AGeneralLedgerMasterRow)FGLMTbl.Rows[iPtr];
            return true;
        }

        /// <summary>
        /// ...
        /// </summary>
        public string AccountCode
        {
            get
            {
                return FglmRow.AccountCode;
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public string CostCentreCode
        {
            get
            {
                return FglmRow.CostCentreCode;
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public int GlmSequence
        {
            get
            {
                return FglmRow.GlmSequence;
            }
        }

        /// <summary>
        /// For some reason, this method is not protected against row is null, or empty field...
        /// </summary>
        public decimal YtdActualBase
        {
            get
            {
                return FglmRow.YtdActualBase;
            }
        }

        /// <summary>
        /// For some reason, this method is not protected against row is null, or empty field...
        /// </summary>
        public decimal ClosingPeriodActualBase
        {
            get
            {
                return FglmRow.ClosingPeriodActualBase;
            }
        }
    }
}