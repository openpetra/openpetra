//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
//
// Copyright 2004-2013 by OM International
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

namespace Ict.Petra.Server.MFinance.Common
{
    /// <summary>
    /// Handling the information of the General Legder Master Table Period Table ...
    /// </summary>
    public class TGlmpInfo
    {
        AGeneralLedgerMasterPeriodTable aGLMp;
        AGeneralLedgerMasterPeriodRow aGLMpRow;

        /// <summary>
        /// Loads <b>all</b> GLMP data ...
        /// </summary>
        public TGlmpInfo()
        {
            LoadAll();
            aGLMpRow = null;
        }

        /// <summary>
        /// Loads all Data and sets a pointer to a specific period ...
        /// </summary>
        /// <param name="ASequence"></param>
        /// <param name="APeriod"></param>
        public TGlmpInfo(int ASequence, int APeriod)
        {
            LoadAll();
            SetToRow(ASequence, APeriod);
        }

        private void LoadAll()
        {
            bool NewTransaction = false;

            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                aGLMp = AGeneralLedgerMasterPeriodAccess.LoadAll(transaction);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception)
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                throw;
            }
        }

        /// <summary>
        /// Sets a pointer to a row specified by APeriod ...
        /// </summary>
        /// <param name="ASequence"></param>
        /// <param name="APeriod"></param>
        /// <returns></returns>
        public bool SetToRow(int ASequence, int APeriod)
        {
            if (aGLMp.Rows.Count > 0)
            {
                for (int i = 0; i < aGLMp.Rows.Count; ++i)
                {
                    aGLMpRow = aGLMp[i];

                    if (aGLMpRow.GlmSequence == ASequence)
                    {
                        if (aGLMpRow.PeriodNumber == APeriod)
                        {
                            return true;
                        }
                    }
                }
            }

            aGLMpRow = null;
            return false;
        }

        /// <summary>
        /// Returns the data base row value
        /// </summary>
        public decimal ActualBase
        {
            get
            {
                return aGLMpRow.ActualBase;
            }
        }
    }


    /// <summary>
    /// Object to handle the read only glm-infos ...
    /// </summary>
    public class TGet_GLM_Info
    {
        DataTable aGLM;
        int iPtr = 0;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACurrentFinancialYear">Number of the year after the the installation of
        /// the software</param>
        public TGet_GLM_Info(int ALedgerNumber, string AAccountCode, int ACurrentFinancialYear)
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ALedgerNumber;
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar);
            ParametersArray[1].Value = AAccountCode;
            ParametersArray[2] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[2].Value = ACurrentFinancialYear;

            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);
            string strSQL = "SELECT * FROM PUB_" + AGeneralLedgerMasterTable.GetTableDBName() + " ";
            strSQL += "WHERE " + AGeneralLedgerMasterTable.GetLedgerNumberDBName() + " = ? ";
            strSQL += "AND " + AGeneralLedgerMasterTable.GetAccountCodeDBName() + " = ? ";
            strSQL += "AND " + AGeneralLedgerMasterTable.GetYearDBName() + " = ? ";
            aGLM = DBAccess.GDBAccessObj.SelectDT(
                strSQL, AGeneralLedgerMasterTable.GetTableDBName(), transaction, ParametersArray);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AAccountCode"></param>
        /// <param name="ACostCentreCode"></param>
        public TGet_GLM_Info(int ALedgerNumber, string AAccountCode, string ACostCentreCode)
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ALedgerNumber;
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar);
            ParametersArray[1].Value = AAccountCode;
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar);
            ParametersArray[2].Value = ACostCentreCode;

            bool NewTransaction;
            TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);
            string strSQL = "SELECT * FROM PUB_" + AGeneralLedgerMasterTable.GetTableDBName() + " ";
            strSQL += "WHERE " + AGeneralLedgerMasterTable.GetLedgerNumberDBName() + " = ? ";
            strSQL += "AND " + AGeneralLedgerMasterTable.GetAccountCodeDBName() + " = ? ";
            strSQL += "AND " + AGeneralLedgerMasterTable.GetCostCentreCodeDBName() + " = ? ";
            aGLM = DBAccess.GDBAccessObj.SelectDT(
                strSQL, AGeneralLedgerMasterTable.GetTableDBName(), transaction, ParametersArray);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
        }

        /// <summary>
        /// Reads the value of YtdActual and supports a default value of 0 if it not exists.
        /// </summary>
        public decimal YtdActual
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(aGLM.Rows[iPtr][AGeneralLedgerMasterTable.GetYtdActualBaseDBName()]);
                }
                catch (IndexOutOfRangeException)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Reads the value of YtdForeign and supports a default value of 0 if it not exists.
        /// </summary>
        public decimal YtdForeign
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(aGLM.Rows[iPtr][AGeneralLedgerMasterTable.GetYtdActualForeignDBName()]);
                }
                catch (IndexOutOfRangeException)
                {
                    return 0;
                }
                catch (InvalidCastException)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public int Sequence
        {
            get
            {
                return Convert.ToInt32(aGLM.Rows[iPtr][AGeneralLedgerMasterTable.GetGlmSequenceDBName()]);
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public string CostCentreCode
        {
            get
            {
                return aGLM.Rows[iPtr][AGeneralLedgerMasterTable.GetCostCentreCodeDBName()].ToString();
            }
        }
    }

    /// <summary>
    /// GLM-Info but an other handling than TGet_GLM_Info
    /// </summary>
    public class TGlmInfo
    {
        DataTable glmTable;
        DataRow glmRow;
        int iPtr;

        /// <summary>
        /// Loads only GLM Data selected by Ledger Number, Year and Account Code ...
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AYear"></param>
        /// <param name="AAccountCode"></param>
        public TGlmInfo(int ALedgerNumber, int AYear, string AAccountCode)
        {
            OdbcParameter[] ParametersArray;
            ParametersArray = new OdbcParameter[3];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ALedgerNumber;
            ParametersArray[1] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[1].Value = AYear;
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar);
            ParametersArray[2].Value = AAccountCode;

            bool NewTransaction = false;

            try
            {
                TDBTransaction transaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                string strSQL = "SELECT * FROM PUB_" + AGeneralLedgerMasterTable.GetTableDBName() + " ";
                strSQL += "WHERE " + AGeneralLedgerMasterTable.GetLedgerNumberDBName() + " = ? ";
                strSQL += "AND " + AGeneralLedgerMasterTable.GetYearDBName() + " = ? ";
                strSQL += "AND " + AGeneralLedgerMasterTable.GetAccountCodeDBName() + " = ? ";
                glmTable = DBAccess.GDBAccessObj.SelectDT(
                    strSQL, AGeneralLedgerMasterTable.GetTableDBName(), transaction, ParametersArray);

                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception)
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                throw;
            }
        }

        /// <summary>
        /// A Row list can be searched and be resetted here ...
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
            ++iPtr;
            try
            {
                glmRow = glmTable.Rows[iPtr];
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public string AccountCode
        {
            get
            {
                return glmRow[AGeneralLedgerMasterTable.GetAccountCodeDBName()].ToString();
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public string CostCentreCode
        {
            get
            {
                return glmRow[AGeneralLedgerMasterTable.GetCostCentreCodeDBName()].ToString();
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public int GlmSequence
        {
            get
            {
                return Convert.ToInt32(glmRow[AGeneralLedgerMasterTable.GetGlmSequenceDBName()]);
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public decimal YtdActualBase
        {
            get
            {
                return Convert.ToDecimal(glmRow[AGeneralLedgerMasterTable.GetYtdActualBaseDBName()]);
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public decimal ClosingPeriodActualBase
        {
            get
            {
                return Convert.ToDecimal(glmRow[AGeneralLedgerMasterTable.GetClosingPeriodActualBaseDBName()]);
            }
        }
    }
}