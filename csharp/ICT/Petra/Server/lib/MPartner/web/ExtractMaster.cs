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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MCommon.queries;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.queries;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MCommon.Data.Cascading;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// methods for extract master list
    /// </summary>
    public class TExtractMasterWebConnector
    {
        /// <summary>
        /// retrieve all extract master records
        /// </summary>
        /// <returns>returns table filled with all extract headers</returns>
        //TODO (gave problems because of DateTime?): [RequireModulePermission("PTNRUSER")]
        public static MExtractMasterTable GetAllExtractHeaders()
        {
            MExtractMasterTable ExtractMasterDT;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            ExtractMasterDT = MExtractMasterAccess.LoadAll(Transaction);

            DBAccess.GDBAccessObj.CommitTransaction();

            return ExtractMasterDT;
        }

        /// <summary>
        /// retrieve all extract master records that match given criteria
        /// </summary>
        /// <param name="AExtractNameFilter"></param>
        /// <param name="AAllUsers"></param>
        /// <param name="AUserCreated"></param>
        /// <param name="AUserModified"></param>
        /// <returns>returns table filled with all extract headers</returns>
        //TODO (gave problems because of DateTime?): [RequireModulePermission("PTNRUSER")]
        public static MExtractMasterTable GetAllExtractHeaders(String AExtractNameFilter, Boolean AAllUsers,
            String AUserCreated, String AUserModified)
        {
            return GetAllExtractHeaders(AExtractNameFilter, "", AAllUsers, AUserCreated, AUserModified,
                null, null, null, null);
        }

        /// <summary>
        /// retrieve all extract master records that match given criteria
        /// </summary>
        /// <param name="AExtractNameFilter"></param>
        /// <param name="AExtractDescFilter"></param>
        /// <param name="AAllUsers"></param>
        /// <param name="AUserCreated"></param>
        /// <param name="AUserModified"></param>
        /// <param name="ADateCreatedFrom"></param>
        /// <param name="ADateCreatedTo"></param>
        /// <param name="ADateModifiedFrom"></param>
        /// <param name="ADateModifiedTo"></param>
        /// <returns>returns table filled with all extract headers</returns>
        //TODO (gave problems because of DateTime?): [RequireModulePermission("PTNRUSER")]
        public static MExtractMasterTable GetAllExtractHeaders(String AExtractNameFilter, String AExtractDescFilter,
            Boolean AAllUsers, String AUserCreated, String AUserModified, DateTime? ADateCreatedFrom,
            DateTime? ADateCreatedTo, DateTime? ADateModifiedFrom, DateTime ? ADateModifiedTo)
        {
            // if no filter is set then call method to get all extracts
            if ((AExtractNameFilter.Length == 0)
                && (AExtractDescFilter.Length == 0)
                && AAllUsers
                && ADateCreatedFrom.HasValue
                && ADateCreatedTo.HasValue
                && ADateModifiedFrom.HasValue
                && ADateModifiedTo.HasValue)
            {
                return GetAllExtractHeaders();
            }

            MExtractMasterTable ExtractMasterDT = new MExtractMasterTable();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            string SqlStmt;
            List <OdbcParameter>SqlParameterList = new List <OdbcParameter>();

            try
            {
                // prepare extract name filter field
                if (AExtractNameFilter == "*")
                {
                    AExtractNameFilter = "";
                }
                else if (AExtractNameFilter.EndsWith("*"))
                {
                    AExtractNameFilter = AExtractNameFilter.Substring(0, AExtractNameFilter.Length - 1);
                }

                AExtractNameFilter = AExtractNameFilter.Replace('*', '%') + "%";

                // Use a direct sql statement rather than db access classes to improve performance as otherwise
                // we would need an extra query for each row of an extract to retrieve partner name and class
                SqlStmt = "SELECT * FROM " + MExtractMasterTable.GetTableDBName() +
                          " WHERE pub_" + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetExtractNameDBName() +
                          " LIKE '" + AExtractNameFilter + "'";

                if (AExtractDescFilter.Length > 0)
                {
                    AExtractDescFilter = AExtractDescFilter.Replace('*', '%');
                    SqlStmt += " AND pub_" + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetExtractDescDBName() +
                               " LIKE '" + AExtractDescFilter + "'";
                }

                if (AUserCreated.Length > 0)
                {
                    SqlStmt += " AND pub_" + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetCreatedByDBName() +
                               " = '" + AUserCreated + "'";
                }

                if (AUserModified.Length > 0)
                {
                    SqlStmt += " AND pub_" + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetModifiedByDBName() +
                               " = '" + AUserModified + "'";
                }

                if (ADateCreatedFrom.HasValue)
                {
                    SqlParameterList.Add(new OdbcParameter("DateCreatedFrom", OdbcType.Date)
                        {
                            Value = ADateCreatedFrom
                        });

                    SqlStmt += " AND pub_" + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetDateCreatedDBName() +
                               " >= ?";
                }

                if (ADateCreatedTo.HasValue)
                {
                    // Add 1 day to date as timestamp is usually set to 00:00:00 and therefore to beginning of day.
                    // Instead add 1 day and make sure that date queried is < (not <=).
                    ADateCreatedTo = ADateCreatedTo.Value.AddDays(1);
                    SqlParameterList.Add(new OdbcParameter("DateCreatedTo", OdbcType.Date)
                        {
                            Value = ADateCreatedTo
                        });

                    SqlStmt += " AND pub_" + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetDateCreatedDBName() +
                               " < ?";
                }

                if (ADateModifiedFrom.HasValue)
                {
                    SqlParameterList.Add(new OdbcParameter("DateModifiedFrom", OdbcType.Date)
                        {
                            Value = ADateModifiedFrom
                        });

                    SqlStmt += " AND pub_" + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetDateModifiedDBName() +
                               " >= ?";
                }

                if (ADateModifiedTo.HasValue)
                {
                    // Add 1 day to date as timestamp is usually set to 00:00:00 and therefore to beginning of day.
                    // Instead add 1 day and make sure that date queried is < (not <=).
                    ADateModifiedTo = ADateModifiedTo.Value.AddDays(1);
                    SqlParameterList.Add(new OdbcParameter("DateModifiedTo", OdbcType.Date)
                        {
                            Value = ADateModifiedTo
                        });

                    SqlStmt += " AND pub_" + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetDateModifiedDBName() +
                               " < ?";
                }

                DBAccess.GDBAccessObj.SelectDT(ExtractMasterDT, SqlStmt, Transaction,
                    SqlParameterList.ToArray(), -1, -1);

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception e)
            {
                TLogging.Log("Problem during load of extract headers: " + e.Message);
                DBAccess.GDBAccessObj.CommitTransaction();
            }

            return ExtractMasterDT;
        }

        /// <summary>
        /// check if extract with given name already exists
        /// </summary>
        /// <param name="AExtractName"></param>
        /// <returns>returns true if extract already exists</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean ExtractExists(String AExtractName)
        {
            MExtractMasterTable TemplateTable;
            MExtractMasterRow TemplateRow;
            Boolean ReturnValue = true;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            TemplateTable = new MExtractMasterTable();
            TemplateRow = TemplateTable.NewRowTyped(false);
            TemplateRow.ExtractName = AExtractName;

            if (MExtractMasterAccess.CountUsingTemplate(TemplateRow, null, Transaction) == 0)
            {
                ReturnValue = false;
            }

            DBAccess.GDBAccessObj.CommitTransaction();

            return ReturnValue;
        }

        /// <summary>
        /// create an empty extract with given name and description
        /// </summary>
        /// <param name="AExtractId"></param>
        /// <param name="AExtractName"></param>
        /// <param name="AExtractDescription"></param>
        /// <returns>returns true if extract was created successfully</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean CreateEmptyExtract(ref int AExtractId, String AExtractName, String AExtractDescription)
        {
            Boolean ResultValue = false;
            TVerificationResultCollection VerificationResult;
            Boolean ExtractExists;

            ResultValue = Server.MPartner.Extracts.TExtractsHandling.CreateNewExtract(AExtractName, AExtractDescription,
                out AExtractId, out ExtractExists, out VerificationResult);

            return ResultValue;
        }

        /// <summary>
        /// create an extract of all family members (persons) existing in a base extract
        /// </summary>
        /// <param name="ABaseExtractId"></param>
        /// <param name="AExtractId"></param>
        /// <param name="AExtractName"></param>
        /// <param name="AExtractDescription"></param>
        /// <returns>returns true if extract was created successfully</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean CreateFamilyMembersExtract(int ABaseExtractId, ref int AExtractId, String AExtractName, String AExtractDescription)
        {
            Boolean ResultValue = false;
            TParameterList ParameterList = new TParameterList();
            TResultList ResultList = new TResultList();

            ParameterList.Add("param_base_extract", ABaseExtractId);
            ParameterList.Add("param_extract_name", AExtractName);
            ParameterList.Add("param_extract_description", AExtractDescription);
            ResultValue = Ict.Petra.Server.MPartner.queries.QueryFamilyMembersExtract.CalculateExtract
                              (ParameterList, ResultList, out AExtractId);

            return ResultValue;
        }

        /// <summary>
        /// create an extract containing all families of persons in a base extract
        /// </summary>
        /// <param name="ABaseExtractId"></param>
        /// <param name="AExtractId"></param>
        /// <param name="AExtractName"></param>
        /// <param name="AExtractDescription"></param>
        /// <returns>returns true if extract was created successfully</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean CreateFamilyExtractForPersons(int ABaseExtractId, ref int AExtractId, String AExtractName, String AExtractDescription)
        {
            Boolean ResultValue = false;
            TParameterList ParameterList = new TParameterList();
            TResultList ResultList = new TResultList();

            ParameterList.Add("param_base_extract", ABaseExtractId);
            ParameterList.Add("param_extract_name", AExtractName);
            ParameterList.Add("param_extract_description", AExtractDescription);
            ResultValue = Ict.Petra.Server.MPartner.queries.QueryFamilyExtractForPersons.CalculateExtract
                              (ParameterList, ResultList, out AExtractId);

            return ResultValue;
        }

        /// <summary>
        /// purge (delete) extracts for specific users and older than x days
        /// </summary>
        /// <param name="ANumberOfDays"></param>
        /// <param name="AAllUsers"></param>
        /// <param name="AUserName"></param>
        /// <returns>returns true if extract was created successfully</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean PurgeExtracts(int ANumberOfDays, Boolean AAllUsers, String AUserName)
        {
            Boolean ResultValue = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            string DeleteStmtTemplate;
            string DeleteStmt;
            string WhereStmtUser = "";
            string WhereStmtMaster = "";
            DateTime PurgeDate = DateTime.Today.AddDays(0 - ANumberOfDays);

            if (!AAllUsers)
            {
                WhereStmtUser = " AND pub_" + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetCreatedByDBName() +
                                " = '" + AUserName + "' ";
            }

            WhereStmtMaster = " pub_" + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetDateCreatedDBName() +
                              " < ?" +
                              " AND ((pub_" + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetDateModifiedDBName() +
                              " IS NULL) OR (" +
                              " pub_" + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetDateModifiedDBName() +
                              " < ?))" +
                              WhereStmtUser;

            DeleteStmtTemplate = "DELETE FROM pub_" + "##cascading_table_extract_id##" +
                                 " WHERE EXISTS (SELECT 1 FROM pub_" + MExtractMasterTable.GetTableDBName() +
                                 " WHERE pub_" + MExtractMasterTable.GetTableDBName() + "." + MExtractMasterTable.GetExtractIdDBName() +
                                 " = pub_" + "##cascading_field_extract_id##" +
                                 " AND " +
                                 WhereStmtMaster +
                                 ")";

            OdbcParameter[] parameterArray = new OdbcParameter[2];
            parameterArray[0] = new OdbcParameter("Date", OdbcType.Date);
            parameterArray[0].Value = ((object)PurgeDate);
            parameterArray[1] = new OdbcParameter("Date", OdbcType.Date);
            parameterArray[1].Value = ((object)PurgeDate);

            try
            {
                // delete MExtractTable
                DeleteStmt = DeleteStmtTemplate.Replace("##cascading_table_extract_id##",
                    MExtractTable.GetTableDBName());
                DeleteStmt = DeleteStmt.Replace("##cascading_field_extract_id##",
                    MExtractTable.GetTableDBName() + "." + MExtractTable.GetExtractIdDBName());
                DBAccess.GDBAccessObj.ExecuteNonQuery(DeleteStmt, Transaction, false, parameterArray);

                // delete MExtractParameterTable
                DeleteStmt = DeleteStmtTemplate.Replace("##cascading_table_extract_id##",
                    MExtractParameterTable.GetTableDBName());
                DeleteStmt = DeleteStmt.Replace("##cascading_field_extract_id##",
                    MExtractParameterTable.GetTableDBName() + "." + MExtractParameterTable.GetExtractIdDBName());
                DBAccess.GDBAccessObj.ExecuteNonQuery(DeleteStmt, Transaction, false, parameterArray);

                // delete SGroupExtractTable
                DeleteStmt = DeleteStmtTemplate.Replace("##cascading_table_extract_id##",
                    SGroupExtractTable.GetTableDBName());
                DeleteStmt = DeleteStmt.Replace("##cascading_field_extract_id##",
                    SGroupExtractTable.GetTableDBName() + "." + SGroupExtractTable.GetExtractIdDBName());
                DBAccess.GDBAccessObj.ExecuteNonQuery(DeleteStmt, Transaction, false, parameterArray);

                // delete MExtractMasterTable
                DeleteStmt = "DELETE FROM pub_" + MExtractMasterTable.GetTableDBName() +
                             " WHERE " + WhereStmtMaster;
                DBAccess.GDBAccessObj.ExecuteNonQuery(DeleteStmt, Transaction, false, parameterArray);

                // commit whole transaction if successful
                DBAccess.GDBAccessObj.CommitTransaction();
                ResultValue = true;
            }
            catch (Exception e)
            {
                TLogging.Log("Problem during purging of extracts: " + e.Message);
                DBAccess.GDBAccessObj.RollbackTransaction();
                ResultValue = false;
            }

            return ResultValue;
        }

        /// <summary>
        /// retrieve extract records and include partner name and class
        /// </summary>
        /// <param name="AExtractId"></param>
        /// <returns>returns table filled with extract rows including partner name and class</returns>
        [RequireModulePermission("PTNRUSER")]
        public static ExtractTDSMExtractTable GetExtractRowsWithPartnerData(int AExtractId)
        {
            ExtractTDSMExtractTable ExtractDT = new ExtractTDSMExtractTable();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            string SqlStmt;

            try
            {
                // Use a direct sql statement rather than db access classes to improve performance as otherwise
                // we would need an extra query for each row of an extract to retrieve partner name and class
                SqlStmt = "SELECT pub_" + MExtractTable.GetTableDBName() + ".*" +
                          ", pub_" + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerShortNameDBName() +
                          ", pub_" + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerClassDBName() +
                          " FROM pub_" + MExtractTable.GetTableDBName() + ", pub_" + PPartnerTable.GetTableDBName() +
                          " WHERE pub_" + MExtractTable.GetTableDBName() + "." + MExtractTable.GetExtractIdDBName() +
                          " = " + AExtractId.ToString() +
                          " AND pub_" + MExtractTable.GetTableDBName() + "." + MExtractTable.GetPartnerKeyDBName() +
                          " = pub_" + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName();

                DBAccess.GDBAccessObj.SelectDT(ExtractDT, SqlStmt, Transaction, null, -1, -1);

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception e)
            {
                TLogging.Log("Problem during load of an extract: " + e.Message);
                DBAccess.GDBAccessObj.CommitTransaction();
            }

            return ExtractDT;
        }

        /// <summary>
        /// save extract master table records (includes cascading delete if record is deleted)
        /// </summary>
        /// <param name="AExtractMasterTable"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>returns table filled with extract rows including partner name and class</returns>
        [RequireModulePermission("PTNRUSER")]
        public static TSubmitChangesResult SaveExtractMaster(ref MExtractMasterTable AExtractMasterTable,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TVerificationResultCollection SingleVerificationResultCollection;
            int ExtractId;
            int CountRecords;
            MExtractMasterRow Row;

            AVerificationResult = null;

            if (AExtractMasterTable != null)
            {
                AVerificationResult = new TVerificationResultCollection();
                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    /* Cascading delete for deleted rows. Once the cascading delete has been done the row
                     * needs to be removed from the table with AcceptChanges as otherwise the later call
                     * to SubmitChanges will complain about those rows that have already been deleted in
                     * the database.
                     * Use a loop to run through the table in reverse Order (Index--) so that the rows
                     * can actually be removed from the table without affecting the access throug Index. */
                    CountRecords = AExtractMasterTable.Rows.Count;

                    for (int Index = CountRecords - 1; Index >= 0; Index--)
                    {
                        Row = (MExtractMasterRow)AExtractMasterTable.Rows[Index];

                        if (Row.RowState == DataRowState.Deleted)
                        {
                            ExtractId = Convert.ToInt32(Row[MExtractMasterTable.GetExtractIdDBName(), DataRowVersion.Original]);
                            MExtractMasterCascading.DeleteByPrimaryKey(ExtractId, SubmitChangesTransaction, true);

                            // accept changes: this actually removes row from table
                            Row.AcceptChanges();
                        }
                    }

                    // now submit all changes to extract master table
                    if (MExtractMasterAccess.SubmitChanges(AExtractMasterTable, SubmitChangesTransaction,
                            out SingleVerificationResultCollection))
                    {
                        SubmissionResult = TSubmitChangesResult.scrOK;
                    }
                    else
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }

                    if (SubmissionResult == TSubmitChangesResult.scrOK)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
                catch (Exception e)
                {
                    TLogging.Log("after submitchanges: exception " + e.Message);

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw new Exception(e.ToString() + " " + e.Message);
                }
            }

            return SubmissionResult;
        }

        /// <summary>
        /// save extract table records
        /// </summary>
        /// <param name="AExtractId"></param>
        /// <param name="AExtractTable"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns>returns table filled with extract rows including partner name and class</returns>
        [RequireModulePermission("PTNRUSER")]
        public static TSubmitChangesResult SaveExtract(int AExtractId, ref MExtractTable AExtractTable,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TVerificationResultCollection SingleVerificationResultCollection;
            int CountExtractRows;
            MExtractMasterTable ExtractMasterDT;

            AVerificationResult = null;

            if (AExtractTable != null)
            {
                AVerificationResult = new TVerificationResultCollection();
                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    if (MExtractAccess.SubmitChanges(AExtractTable, SubmitChangesTransaction,
                            out SingleVerificationResultCollection))
                    {
                        SubmissionResult = TSubmitChangesResult.scrOK;
                    }
                    else
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }

                    // update extract master record with the correct number of extract records
                    if ((AExtractTable.Rows.Count > 0)
                        && (SubmissionResult != TSubmitChangesResult.scrError))
                    {
                        CountExtractRows = MExtractAccess.CountViaMExtractMaster(AExtractId, SubmitChangesTransaction);
                        ExtractMasterDT = MExtractMasterAccess.LoadByPrimaryKey(AExtractId, SubmitChangesTransaction);

                        if (ExtractMasterDT.Rows.Count != 0)
                        {
                            ((MExtractMasterRow)ExtractMasterDT.Rows[0]).KeyCount = CountExtractRows;

                            if (MExtractMasterAccess.SubmitChanges(ExtractMasterDT, SubmitChangesTransaction,
                                    out SingleVerificationResultCollection))
                            {
                                SubmissionResult = TSubmitChangesResult.scrOK;
                            }
                            else
                            {
                                SubmissionResult = TSubmitChangesResult.scrError;
                            }
                        }
                    }

                    if (SubmissionResult == TSubmitChangesResult.scrOK)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
                catch (Exception e)
                {
                    TLogging.Log("after submitchanges: exception " + e.Message);

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw new Exception(e.ToString() + " " + e.Message);
                }
            }

            return SubmissionResult;
        }

        /// <summary>
        /// update email gift statement flag for all partners in given extract
        /// </summary>
        /// <param name="AExtractId"></param>
        /// <param name="AEmailGiftStatement"></param>
        /// <returns>true if update was successful</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean UpdateEmailGiftStatement(int AExtractId, Boolean AEmailGiftStatement)
        {
            Boolean ResultValue = true;
            String EmailGiftStatementValue;

            if (AEmailGiftStatement)
            {
                EmailGiftStatementValue = "true";
            }
            else
            {
                EmailGiftStatementValue = "false";
            }

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            string SqlStmt;

            try
            {
                // Use a direct sql statement rather than db access classes to improve performance as otherwise
                // we would need an extra query for each row of an extract to update data
                SqlStmt = "UPDATE pub_" + PPartnerTable.GetTableDBName() +
                          " SET " + PPartnerTable.GetEmailGiftStatementDBName() + " = " + EmailGiftStatementValue +
                          " WHERE " + PPartnerTable.GetPartnerKeyDBName() +
                          " IN (SELECT " + MExtractTable.GetPartnerKeyDBName() + " FROM pub_" + MExtractTable.GetTableDBName() +
                          " WHERE " + MExtractTable.GetExtractIdDBName() + " = " + AExtractId + ")";

                DBAccess.GDBAccessObj.ExecuteNonQuery(SqlStmt, Transaction);

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception e)
            {
                TLogging.Log("Problem during update of email gift statement for an extract: " + e.Message);
                DBAccess.GDBAccessObj.RollbackTransaction();
                ResultValue = false;
            }

            return ResultValue;
        }

        /// <summary>
        /// update receipt letter frequency and 'receipt each gift' flag for all partners in given extract
        /// </summary>
        /// <param name="AExtractId"></param>
        /// <param name="AUpdateReceiptLetterFrequency"></param>
        /// <param name="AReceiptLetterFrequency"></param>
        /// <param name="AUpdateReceiptEachGift"></param>
        /// <param name="AReceiptEachGift"></param>
        /// <returns>true if update was successful</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean UpdateReceiptFrequency(int AExtractId, Boolean AUpdateReceiptLetterFrequency,
            String AReceiptLetterFrequency, Boolean AUpdateReceiptEachGift, Boolean AReceiptEachGift)
        {
            Boolean ResultValue = true;
            String ReceiptEachGiftValue;
            String FieldUpdate = "";

            if (!AUpdateReceiptLetterFrequency
                && !AUpdateReceiptEachGift)
            {
                // nothing to do
                return ResultValue;
            }

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            string SqlStmt;

            try
            {
                if (AUpdateReceiptLetterFrequency)
                {
                    FieldUpdate = PPartnerTable.GetReceiptLetterFrequencyDBName() +
                                  " = '" + AReceiptLetterFrequency + "'";
                }

                if (AUpdateReceiptEachGift)
                {
                    if (AReceiptEachGift)
                    {
                        ReceiptEachGiftValue = "true";
                    }
                    else
                    {
                        ReceiptEachGiftValue = "false";
                    }

                    if (FieldUpdate.Length > 0)
                    {
                        FieldUpdate = FieldUpdate + ", ";
                    }

                    FieldUpdate = FieldUpdate + PPartnerTable.GetReceiptEachGiftDBName() +
                                  " = " + ReceiptEachGiftValue;
                }

                // Use a direct sql statement rather than db access classes to improve performance as otherwise
                // we would need an extra query for each row of an extract to update data
                SqlStmt = "UPDATE pub_" + PPartnerTable.GetTableDBName() +
                          " SET " + FieldUpdate +
                          " WHERE " + PPartnerTable.GetPartnerKeyDBName() +
                          " IN (SELECT " + MExtractTable.GetPartnerKeyDBName() + " FROM pub_" + MExtractTable.GetTableDBName() +
                          " WHERE " + MExtractTable.GetExtractIdDBName() + " = " + AExtractId + ")";

                DBAccess.GDBAccessObj.ExecuteNonQuery(SqlStmt, Transaction);

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception e)
            {
                TLogging.Log("Problem during update of receipt frequency for an extract: " + e.Message);
                DBAccess.GDBAccessObj.RollbackTransaction();
                ResultValue = false;
            }

            return ResultValue;
        }
    }
}