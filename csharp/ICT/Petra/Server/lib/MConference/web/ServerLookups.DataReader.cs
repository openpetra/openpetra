//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MConference.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Server.App.Core.Security;


namespace Ict.Petra.Server.MConference.Conference.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MCommon DataReader sub-namespace.
    ///
    /// </summary>
    public class TConferenceDataReaderWebConnector
    {
        /// <summary>
        /// Return selected conference's currency code and currency name
        /// </summary>
        /// <param name="APartnerKey">match long for conference key</param>
        /// <param name="ACurrencyCode">CurrencyCode for selected conference</param>
        /// <param name="ACurrencyName">CurrencyName for selected conference</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean GetCurrency(Int64 APartnerKey, out string ACurrencyCode, out string ACurrencyName)
        {
            ACurrencyCode = "";
            ACurrencyName = "";
            
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            PcConferenceTable ConferenceTable;
            ACurrencyTable CurrencyTable;
            Boolean ReturnValue = false;
            
            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);
                
                if (ConferenceTable.Rows.Count == 0)
                {
                    ReturnValue = false;
                }
                else
                {
                    ACurrencyCode = ((PcConferenceRow) ConferenceTable.Rows[0]).CurrencyCode;
                    
                    // use the obtained currency code to retrieve the currency name
                    CurrencyTable = ACurrencyAccess.LoadByPrimaryKey(ACurrencyCode, ReadTransaction);
                    ACurrencyName = CurrencyTable[0].CurrencyName;
                    
                    ReturnValue = true;
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.GetCurrency: rollback own transaction.");
                }
            }
            
            return ReturnValue;
        }
        
        /// <summary>
        /// Return selected conference's start date
        /// </summary>
        /// <param name="APartnerKey">match long for conference key</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static DateTime GetStartDate(Int64 APartnerKey)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            PcConferenceTable ConferenceTable;
            DateTime ConferenceStartDate = new DateTime();
            
            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);
                
                ConferenceStartDate = (DateTime) ((PcConferenceRow) ConferenceTable.Rows[0]).Start;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.GetStartDate: rollback own transaction.");
                }
            }
            
            return ConferenceStartDate;
        }
        
        /// <summary>
        /// Return selected conference's end date
        /// </summary>
        /// <param name="APartnerKey">match long for conference key</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static DateTime GetEndDate(Int64 APartnerKey)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            PcConferenceTable ConferenceTable;
            DateTime ConferenceEndDate = new DateTime();
            
            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                ConferenceTable = PcConferenceAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);
                
                ConferenceEndDate = (DateTime) ((PcConferenceRow) ConferenceTable.Rows[0]).End;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.GetEndDate: rollback own transaction.");
                }
            }
            
            return ConferenceEndDate;
        }
        
        /// <summary>
        /// Check Discount Criteria Code foreign key exists
        /// </summary>
        /// <param name="ADiscountCriteriaCode">Criteria code to check</param>
        /// <param name="ACreateNewIfMissing">Create foreign key if it does not already exist.</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static void CheckDiscountCriteriaCode(string ADiscountCriteriaCode, Boolean ACreateNewIfMissing)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            Boolean RowExists = false;
            
            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                RowExists = PcDiscountCriteriaAccess.Exists(ADiscountCriteriaCode, ReadTransaction);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.CheckDiscountCriteriaCode: rollback own transaction.");
                }
            }
            
            if (!RowExists && ACreateNewIfMissing)
            {
                AddDiscountCriteriaCode(ADiscountCriteriaCode);
            }
        }
        
        /// <summary>
        /// Create Discount Criteria Code foreign key
        /// </summary>
        /// <param name="ADiscountCriteriaCode">Criteria code to add</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static void AddDiscountCriteriaCode(string ADiscountCriteriaCode)
        {
            TDBTransaction Transaction;
            PcDiscountCriteriaTable DiscountCriteriaTable = null;
            
            Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                DiscountCriteriaTable = PcDiscountCriteriaAccess.LoadAll(Transaction);

                PcDiscountCriteriaRow AddRow = DiscountCriteriaTable.NewRowTyped();
                AddRow.DiscountCriteriaCode = ADiscountCriteriaCode;
                AddRow.DiscountCriteriaDesc = ADiscountCriteriaCode;
                AddRow.DeletableFlag = false;
                PcDiscountCriteriaAccess.AddOrModifyRecord(ADiscountCriteriaCode, DiscountCriteriaTable, AddRow, true, Transaction);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.AddDiscountCriteriaCode: commit own transaction.");
            }
        }
    }
}