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
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
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
        /// Check Discount Criteria row exists
        /// </summary>
        /// <param name="ADiscountCriteriaCode">Primary Key to check exists</param>
        /// <param name="ADiscountCriteriaDescription">Criteria description to add if row does not exist</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static void CreateDiscountCriteriaIfNotExisting(string ADiscountCriteriaCode, string ADiscountCriteriaDescription)
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
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.CreateDiscountCriteriaIfNotExisting: rollback own transaction.");
                }
            }
            
            // add row is it does not exist
            if (!RowExists)
            {
                AddDiscountCriteriaCode(ADiscountCriteriaCode, ADiscountCriteriaDescription);
            }
        }
        
        /// <summary>
        /// Create new Discount Criteria row
        /// </summary>
        /// <param name="ADiscountCriteriaCode">Criteria code to add</param>
        /// <param name="ADiscountCriteriaDescription">Criteria description to add</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        private static void AddDiscountCriteriaCode(string ADiscountCriteriaCode, string ADiscountCriteriaDescription)
        {
            TDBTransaction Transaction;
            PcDiscountCriteriaTable DiscountCriteriaTable = null;
            TVerificationResultCollection VerificationResult;
            
            Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                DiscountCriteriaTable = PcDiscountCriteriaAccess.LoadAll(Transaction);

                // set column values
                PcDiscountCriteriaRow AddRow = DiscountCriteriaTable.NewRowTyped();
                AddRow.DiscountCriteriaCode = ADiscountCriteriaCode;
                AddRow.DiscountCriteriaDesc = ADiscountCriteriaDescription;
                AddRow.DeletableFlag = false;
                
                // add new row to database table
                DiscountCriteriaTable.Rows.Add(AddRow);
                PcDiscountCriteriaAccess.SubmitChanges(DiscountCriteriaTable, Transaction, out VerificationResult);
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
        
         /// <summary>
        /// Check Cost Type row exists
        /// </summary>
        /// <param name="ACostTypeCode">Primary Key to check exists</param>
        /// <param name="ACostTypeDescription">Cost Type description to add if row does not exist</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static void CreateCostTypeIfNotExisting(string ACostTypeCode, string ACostTypeDescription)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            Boolean RowExists = false;
            
            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                RowExists = PcCostTypeAccess.Exists(ACostTypeCode, ReadTransaction);
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
                    TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.CreateCostTypeIfNotExisting: rollback own transaction.");
                }
            }
            
            // add row is it does not exist
            if (!RowExists)
            {
                AddCostTypeCode(ACostTypeCode, ACostTypeDescription);
            }
        }
        
        /// <summary>
        /// Create new Cost Type row
        /// </summary>
        /// <param name="ACostTypeCode">Cost Type Code to add</param>
        /// <param name="ACostTypeDescription">Cost Type Description to add</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        private static void AddCostTypeCode(string ACostTypeCode, string ACostTypeDescription)
        {
            TDBTransaction Transaction;
            PcCostTypeTable CostTypeTable = null;
            TVerificationResultCollection VerificationResult;
            
            Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                CostTypeTable = PcCostTypeAccess.LoadAll(Transaction);

                // set column values
                PcCostTypeRow AddRow = CostTypeTable.NewRowTyped();
                AddRow.CostTypeCode = ACostTypeCode;
                AddRow.CostTypeDescription = ACostTypeDescription;
                AddRow.DeletableFlag = false;
                
                // add new row to database table
                CostTypeTable.Rows.Add(AddRow);
                PcCostTypeAccess.SubmitChanges(CostTypeTable, Transaction, out VerificationResult);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.AddCostTypeCode: commit own transaction.");
            }
        }
        
        /// <summary>
        /// Create new Conference
        /// </summary>
        /// <param name="APartnerKey">match long for conference key</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean ConferenceExists(long APartnerKey)
        {
            TDBTransaction ReadTransaction;
            Boolean Exists = false;
            
            ReadTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            try
            {
                Exists = PcConferenceAccess.Exists(APartnerKey, ReadTransaction);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.CreateNewConference: commit own transaction.");
            }
            
            return Exists;
        }
        
        /// <summary>
        /// Create a new Conference
        /// </summary>
        /// <param name="APartnerKey">match long for conference key</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static void CreateNewConference(long APartnerKey)
        {
            TDBTransaction Transaction;
            TVerificationResultCollection VerificationResult;
            PcConferenceTable ConferenceTable;
            PUnitTable UnitTable;
            PPartnerLocationTable PartnerLocationTable;
            
            Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                ConferenceTable = PcConferenceAccess.LoadAll(Transaction);
                UnitTable = PUnitAccess.LoadByPrimaryKey(APartnerKey, Transaction);
                PartnerLocationTable = PPartnerLocationAccess.LoadAll(Transaction);
                
                DateTime Start = new DateTime();
                DateTime End = new DateTime();
                
                foreach (PPartnerLocationRow PartnerLocationRow in PartnerLocationTable.Rows)
                {
                    if (PartnerLocationRow.PartnerKey == APartnerKey)
                    {
                        if (PartnerLocationRow.DateEffective != null)
                        {
                            Start = (DateTime) PartnerLocationRow.DateEffective;
                        }
                        
                        if (PartnerLocationRow.DateGoodUntil != null)
                        {
                            End = (DateTime) PartnerLocationRow.DateGoodUntil;
                        }
                    }
                }

                // set column values
                PcConferenceRow AddRow = ConferenceTable.NewRowTyped();
                AddRow.ConferenceKey = APartnerKey;
                
                string OutreachPrefix = ((PUnitRow) UnitTable.Rows[0]).OutreachCode;
                
                if (OutreachPrefix.Length > 4)
                {
                    AddRow.OutreachPrefix = OutreachPrefix.Substring(0,5);
                }
                else
                {
                    AddRow.OutreachPrefix = OutreachPrefix;
                }
                
                if (Start != DateTime.MinValue)
                {
                    AddRow.Start = Start;
                }
                
                if (End != DateTime.MinValue)
                {
                    AddRow.End = End;
                }
                
                AddRow.CurrencyCode = "USD";
                
                // add new row to database table
                ConferenceTable.Rows.Add(AddRow);
                PcConferenceAccess.SubmitChanges(ConferenceTable, Transaction, out VerificationResult);
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            finally
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TConferenceDataReaderWebConnector.CreateNewConference: commit own transaction.");
            }
        }
    }
}