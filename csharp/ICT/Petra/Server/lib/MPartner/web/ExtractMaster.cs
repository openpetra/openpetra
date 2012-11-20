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
        [RequireModulePermission("PTNRUSER")]
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
        [RequireModulePermission("PTNRUSER")]
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
        [RequireModulePermission("PTNRUSER")]
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
        /// retrieve information if partner has subscription for certain publication
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="APublicationCode"></param>
        /// <returns>true if subscription exists</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean SubscriptionExists(Int64 APartnerKey, String APublicationCode)
        {
            Boolean ResultValue = false;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            ResultValue = PSubscriptionAccess.Exists(APublicationCode, APartnerKey, Transaction);
            DBAccess.GDBAccessObj.CommitTransaction();

            return ResultValue;
        }

        /// <summary>
        /// add subscription for all partners in a given extract
        /// </summary>
        /// <param name="AExtractId">extract to add subscription to</param>
        /// <param name="ATable">table with only one subscription row to be added for each partner</param>
        /// <param name="AExistingSubscriptionPartners">table containing partners that already have subscription for given publication</param>
        /// <param name="ASubscriptionsAdded">number of subscriptions added</param>
        /// <returns>true if adding was successful</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean AddSubscription(int AExtractId, ref PSubscriptionTable ATable,
            out PPartnerTable AExistingSubscriptionPartners, out int ASubscriptionsAdded)
        {
            Boolean ResultValue = true;
            PSubscriptionTable SubscriptionTable = new PSubscriptionTable();
            PSubscriptionRow SubscriptionRowTemplate;
            PSubscriptionRow SubscriptionRow;
            MExtractTable ExtractTable;
            PPartnerTable PartnerTable;
            PPartnerRow PartnerRow;
            TVerificationResultCollection VerificationResultCollection;

            // only use first row in table (as rows can't be serialized as parameters)
            SubscriptionRowTemplate = (PSubscriptionRow)ATable.Rows[0];

            AExistingSubscriptionPartners = new PPartnerTable();
            ASubscriptionsAdded = 0;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                ExtractTable = MExtractAccess.LoadViaMExtractMaster(AExtractId, Transaction);

                // query all rows of given extract
                foreach (MExtractRow ExtractRow in ExtractTable.Rows)
                {
                    // for each extract row either add subscription or add to list of partners already having one
                    if (PSubscriptionAccess.Exists(SubscriptionRowTemplate.PublicationCode, ExtractRow.PartnerKey, Transaction))
                    {
                        PartnerRow = AExistingSubscriptionPartners.NewRowTyped();
                        PartnerTable = PPartnerAccess.LoadByPrimaryKey(ExtractRow.PartnerKey, Transaction);
                        DataUtilities.CopyAllColumnValues(PartnerTable.Rows[0], PartnerRow);
                        AExistingSubscriptionPartners.Rows.Add(PartnerRow);
                    }
                    else
                    {
                        SubscriptionRow = SubscriptionTable.NewRowTyped();
                        DataUtilities.CopyAllColumnValues(SubscriptionRowTemplate, SubscriptionRow);
                        SubscriptionRow.PartnerKey = ExtractRow.PartnerKey;
                        SubscriptionTable.Rows.Add(SubscriptionRow);
                        ASubscriptionsAdded++;
                    }
                }

                // now submit changes to the database
                if (PSubscriptionAccess.SubmitChanges(SubscriptionTable, Transaction, out VerificationResultCollection))
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    ResultValue = true;
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    ResultValue = false;
                }
            }
            catch (Exception e)
            {
                TLogging.Log("Problem during adding of subscription for an extract: " + e.Message);
                DBAccess.GDBAccessObj.RollbackTransaction();
                ResultValue = false;
            }

            return ResultValue;
        }

        /// <summary>
        /// change subscription for all partners in a given extract for which the subscription exists
        /// </summary>
        /// <param name="AExtractId">extract to add subscription to</param>
        /// <param name="ATable">table with only one subscription row to be changed for each partner</param>
        /// <param name="AFieldsToChange">table with only one subscription row to be changed for each partner</param>
        /// <param name="APartnersWithoutSubscription">table containing partners that do not have the given subscription</param>
        /// <param name="ASubscriptionsChanged">number of subscriptions changed</param>
        /// <returns>true if change was successful</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean ChangeSubscription(int AExtractId, ref PSubscriptionTable ATable,
            List <String>AFieldsToChange, out PPartnerTable APartnersWithoutSubscription, out int ASubscriptionsChanged)
        {
            Boolean ResultValue = true;
            PSubscriptionTable SubscriptionTable = new PSubscriptionTable();
            PSubscriptionRow SubscriptionRowTemplate;
            PSubscriptionRow SubscriptionRow;
            MExtractTable ExtractTable;
            PPartnerTable PartnerTable;
            PPartnerRow PartnerRow;
            TVerificationResultCollection VerificationResultCollection;

            // only use first row in table (as rows can't be serialized as parameters)
            SubscriptionRowTemplate = (PSubscriptionRow)ATable.Rows[0];

            APartnersWithoutSubscription = new PPartnerTable();
            ASubscriptionsChanged = 0;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                ExtractTable = MExtractAccess.LoadViaMExtractMaster(AExtractId, Transaction);

                // query all rows of given extract
                foreach (MExtractRow ExtractRow in ExtractTable.Rows)
                {
                    // for each extract row either change subscription or add to list of partners that don't have one
                    if (PSubscriptionAccess.Exists(SubscriptionRowTemplate.PublicationCode, ExtractRow.PartnerKey, Transaction))
                    {
                        SubscriptionTable = PSubscriptionAccess.LoadByPrimaryKey(SubscriptionRowTemplate.PublicationCode,
                            ExtractRow.PartnerKey,
                            Transaction);
                        SubscriptionRow = (PSubscriptionRow)SubscriptionTable.Rows[0];

                        // change field contents
                        if (AFieldsToChange.Contains(PSubscriptionTable.GetSubscriptionStatusDBName()))
                        {
                            SubscriptionRow.SubscriptionStatus = SubscriptionRowTemplate.SubscriptionStatus;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetGratisSubscriptionDBName()))
                        {
                            SubscriptionRow.GratisSubscription = SubscriptionRowTemplate.GratisSubscription;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetNumberComplimentaryDBName()))
                        {
                            SubscriptionRow.NumberComplimentary = SubscriptionRowTemplate.NumberComplimentary;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetPublicationCopiesDBName()))
                        {
                            SubscriptionRow.PublicationCopies = SubscriptionRowTemplate.PublicationCopies;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetReasonSubsGivenCodeDBName()))
                        {
                            SubscriptionRow.ReasonSubsGivenCode = SubscriptionRowTemplate.ReasonSubsGivenCode;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetReasonSubsCancelledCodeDBName()))
                        {
                            SubscriptionRow.ReasonSubsCancelledCode = SubscriptionRowTemplate.ReasonSubsCancelledCode;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetGiftFromKeyDBName()))
                        {
                            SubscriptionRow.GiftFromKey = SubscriptionRowTemplate.GiftFromKey;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetStartDateDBName()))
                        {
                            SubscriptionRow.StartDate = SubscriptionRowTemplate.StartDate;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetExpiryDateDBName()))
                        {
                            SubscriptionRow.ExpiryDate = SubscriptionRowTemplate.ExpiryDate;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetSubscriptionRenewalDateDBName()))
                        {
                            SubscriptionRow.SubscriptionRenewalDate = SubscriptionRowTemplate.SubscriptionRenewalDate;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetDateNoticeSentDBName()))
                        {
                            SubscriptionRow.DateNoticeSent = SubscriptionRowTemplate.DateNoticeSent;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetDateCancelledDBName()))
                        {
                            SubscriptionRow.DateCancelled = SubscriptionRowTemplate.DateCancelled;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetNumberIssuesReceivedDBName()))
                        {
                            SubscriptionRow.NumberIssuesReceived = SubscriptionRowTemplate.NumberIssuesReceived;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetFirstIssueDBName()))
                        {
                            SubscriptionRow.FirstIssue = SubscriptionRowTemplate.FirstIssue;
                        }

                        if (AFieldsToChange.Contains(PSubscriptionTable.GetLastIssueDBName()))
                        {
                            SubscriptionRow.LastIssue = SubscriptionRowTemplate.LastIssue;
                        }

                        // submit changes to the database after each row
                        if (PSubscriptionAccess.SubmitChanges(SubscriptionTable, Transaction, out VerificationResultCollection))
                        {
                            ResultValue = true;
                        }
                        else
                        {
                            DBAccess.GDBAccessObj.RollbackTransaction();
                            ResultValue = false;
                            return ResultValue;
                        }

                        //SubscriptionTable.Rows.Add(SubscriptionRow);
                        ASubscriptionsChanged++;
                    }
                    else
                    {
                        // this partner does not have given subscription, therefore it cannot be changed
                        PartnerRow = APartnersWithoutSubscription.NewRowTyped();
                        PartnerTable = PPartnerAccess.LoadByPrimaryKey(ExtractRow.PartnerKey, Transaction);
                        DataUtilities.CopyAllColumnValues(PartnerTable.Rows[0], PartnerRow);
                        APartnersWithoutSubscription.Rows.Add(PartnerRow);
                    }
                }

                if (ResultValue)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }
            catch (Exception e)
            {
                TLogging.Log("Problem during changing of subscription for an extract: " + e.Message);
                DBAccess.GDBAccessObj.RollbackTransaction();
                ResultValue = false;
            }

            return ResultValue;
        }

        /// <summary>
        /// delete subscription for a partner in a given extract
        /// </summary>
        /// <param name="AExtractId"></param>
        /// <param name="APublicationCode"></param>
        /// <param name="APartnerKey"></param>
        /// <returns>true if deletion was successful</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean DeleteSubscription(int AExtractId, Int64 APartnerKey, String APublicationCode)
        {
            Boolean ResultValue = true;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            string SqlStmt;

            try
            {
                // Use a direct sql statement rather than db access classes to improve performance as otherwise
                // we would need an extra query for each row of an extract to update data
                SqlStmt = "DELETE FROM pub_" + PSubscriptionTable.GetTableDBName() +
                          " WHERE " + PSubscriptionTable.GetPublicationCodeDBName() + " = '" + APublicationCode + "'" +
                          " AND " + PSubscriptionTable.GetPartnerKeyDBName() + " = " + APartnerKey.ToString();

                DBAccess.GDBAccessObj.ExecuteNonQuery(SqlStmt, Transaction);

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception e)
            {
                TLogging.Log("Problem during deletion of subscription for an extract: " + e.Message);
                DBAccess.GDBAccessObj.RollbackTransaction();
                ResultValue = false;
            }

            return ResultValue;
        }

        /// <summary>
        /// update solicitations flag for all partners in given extract
        /// </summary>
        /// <param name="AExtractId"></param>
        /// <param name="ANoSolicitations"></param>
        /// <returns>true if update was successful</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean UpdateSolicitationFlag(int AExtractId, Boolean ANoSolicitations)
        {
            Boolean ResultValue = true;
            String NoSolicitationsValue;

            if (ANoSolicitations)
            {
                NoSolicitationsValue = "true";
            }
            else
            {
                NoSolicitationsValue = "false";
            }

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
            string SqlStmt;

            try
            {
                // Use a direct sql statement rather than db access classes to improve performance as otherwise
                // we would need an extra query for each row of an extract to update data
                SqlStmt = "UPDATE pub_" + PPartnerTable.GetTableDBName() +
                          " SET " + PPartnerTable.GetNoSolicitationsDBName() + " = " + NoSolicitationsValue +
                          " WHERE " + PPartnerTable.GetPartnerKeyDBName() +
                          " IN (SELECT " + MExtractTable.GetPartnerKeyDBName() + " FROM pub_" + MExtractTable.GetTableDBName() +
                          " WHERE " + MExtractTable.GetExtractIdDBName() + " = " + AExtractId + ")";

                DBAccess.GDBAccessObj.ExecuteNonQuery(SqlStmt, Transaction);

                DBAccess.GDBAccessObj.CommitTransaction();
            }
            catch (Exception e)
            {
                TLogging.Log("Problem during update of solicitation flag for an extract: " + e.Message);
                DBAccess.GDBAccessObj.RollbackTransaction();
                ResultValue = false;
            }

            return ResultValue;
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

        /// <summary>
        /// Creates a new extract by combining a list of given extracts.
        /// </summary>
        /// <param name="ANewExtractName">Name of the Extract to be created.</param>
        /// <param name="ANewExtractDescription">Description of the Extract to be created.</param>
        /// <param name="ACombineExtractIdList">List of Ids of extracts to be combined.</param>
        /// <param name="ANewExtractId">Extract Id of the created Extract, or -1 if the
        /// creation of the Extract was not successful.</param>
        /// <param name="AVerificationResults">Nil if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions).</param>
        /// <returns>True if the new combined Extract was created, otherwise false.</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean CombineExtracts(String ANewExtractName,
            String ANewExtractDescription,
            List <Int32>ACombineExtractIdList,
            out Int32 ANewExtractId,
            out TVerificationResultCollection AVerificationResults)
        {
            Boolean ResultValue = true;
            Boolean ExtractAlreadyExists = false;
            MExtractTable ExtractTable;
            MExtractTable CombinedExtractTable = new MExtractTable();
            MExtractRow TemplateRow;
            Boolean NewTransaction;

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            ANewExtractId = -1;
            AVerificationResults = null;


            ResultValue = MPartner.Extracts.TExtractsHandling.CreateNewExtract(ANewExtractName,
                ANewExtractDescription, out ANewExtractId, out ExtractAlreadyExists, out AVerificationResults);

            if (ResultValue && !ExtractAlreadyExists)
            {
                // loop through each extract and combine them
                foreach (Int32 ExtractId in ACombineExtractIdList)
                {
                    ExtractTable = MExtractAccess.LoadViaMExtractMaster(ExtractId, WriteTransaction);

                    foreach (DataRow ExtractRow in ExtractTable.Rows)
                    {
                        if (CombinedExtractTable.Rows.Find(new object[] { ANewExtractId,
                                                                          ((MExtractRow)ExtractRow).PartnerKey,
                                                                          ((MExtractRow)ExtractRow).SiteKey }) == null)
                        {
                            // create and add row to combined extract as it does not exist yet
                            TemplateRow = (MExtractRow)CombinedExtractTable.NewRowTyped(true);
                            TemplateRow.ExtractId = ANewExtractId;
                            TemplateRow.PartnerKey = ((MExtractRow)ExtractRow).PartnerKey;
                            TemplateRow.SiteKey = ((MExtractRow)ExtractRow).SiteKey;
                            TemplateRow.LocationKey = ((MExtractRow)ExtractRow).LocationKey;

                            CombinedExtractTable.Rows.Add(TemplateRow);
                        }
                    }
                }

                try
                {
                    // update key count in master table
                    MExtractMasterTable CombinedExtractMaster = MExtractMasterAccess.LoadByPrimaryKey(ANewExtractId, WriteTransaction);
                    CombinedExtractMaster[0].KeyCount = CombinedExtractTable.Rows.Count;

                    // submit changes in master and then in extract content table which refers to it
                    if (MExtractAccess.SubmitChanges(CombinedExtractTable, WriteTransaction, out AVerificationResults))
                    {
                        ResultValue = MExtractMasterAccess.SubmitChanges(CombinedExtractMaster, WriteTransaction, out AVerificationResults);
                    }
                }
                catch (Exception)
                {
                    ResultValue = false;
                }
            }

            if (ResultValue && NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return ResultValue;
        }

        /// <summary>
        /// Creates a new extract by intersecting a list of given extracts.
        /// </summary>
        /// <param name="ANewExtractName">Name of the Extract to be created.</param>
        /// <param name="ANewExtractDescription">Description of the Extract to be created.</param>
        /// <param name="AIntersectExtractIdList">List of Ids of extracts to be intersected.</param>
        /// <param name="ANewExtractId">Extract Id of the created Extract, or -1 if the
        /// creation of the Extract was not successful.</param>
        /// <param name="AVerificationResults">Nil if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions).</param>
        /// <returns>True if the new intersected Extract was created, otherwise false.</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean IntersectExtracts(String ANewExtractName,
            String ANewExtractDescription,
            List <Int32>AIntersectExtractIdList,
            out Int32 ANewExtractId,
            out TVerificationResultCollection AVerificationResults)
        {
            Boolean ResultValue = true;
            Boolean ExtractAlreadyExists = false;
            Int32 ExtractIndex;
            MExtractTable FirstExtractTable;
            MExtractTable IntersectedExtractTable = new MExtractTable();
            MExtractRow TemplateRow;
            Boolean NewTransaction;
            Boolean PartnerExistsInAllExtracts;

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            ANewExtractId = -1;
            AVerificationResults = null;


            ResultValue = MPartner.Extracts.TExtractsHandling.CreateNewExtract(ANewExtractName,
                ANewExtractDescription, out ANewExtractId, out ExtractAlreadyExists, out AVerificationResults);

            if (ResultValue && !ExtractAlreadyExists)
            {
                if (AIntersectExtractIdList.Count > 0)
                {
                    FirstExtractTable = MExtractAccess.LoadViaMExtractMaster(AIntersectExtractIdList[0], WriteTransaction);

                    // iterate through all partners in first extract and check if this record also exists in all other extracts
                    foreach (DataRow ExtractRow in FirstExtractTable.Rows)
                    {
                        PartnerExistsInAllExtracts = true;

                        // now check if this partner record exists in all other extracts as well
                        for (ExtractIndex = 1;
                             ExtractIndex < AIntersectExtractIdList.Count && PartnerExistsInAllExtracts;
                             ExtractIndex++)
                        {
                            if (!MExtractAccess.Exists(AIntersectExtractIdList[ExtractIndex], ((MExtractRow)ExtractRow).PartnerKey,
                                    ((MExtractRow)ExtractRow).SiteKey, WriteTransaction))
                            {
                                // can stop here as there is at least one extract where partner does not exist
                                PartnerExistsInAllExtracts = false;
                            }
                        }

                        // create and add row to intersected extract as it exists in all extracts
                        if (PartnerExistsInAllExtracts)
                        {
                            TemplateRow = (MExtractRow)IntersectedExtractTable.NewRowTyped(true);
                            TemplateRow.ExtractId = ANewExtractId;
                            TemplateRow.PartnerKey = ((MExtractRow)ExtractRow).PartnerKey;
                            TemplateRow.SiteKey = ((MExtractRow)ExtractRow).SiteKey;
                            TemplateRow.LocationKey = ((MExtractRow)ExtractRow).LocationKey;

                            IntersectedExtractTable.Rows.Add(TemplateRow);
                        }
                    }
                }

                try
                {
                    // update key count in master table
                    MExtractMasterTable IntersectedExtractMaster = MExtractMasterAccess.LoadByPrimaryKey(ANewExtractId, WriteTransaction);
                    IntersectedExtractMaster[0].KeyCount = IntersectedExtractTable.Rows.Count;

                    // submit changes in master and then in extract content table which refers to it
                    if (MExtractAccess.SubmitChanges(IntersectedExtractTable, WriteTransaction, out AVerificationResults))
                    {
                        ResultValue = MExtractMasterAccess.SubmitChanges(IntersectedExtractMaster, WriteTransaction, out AVerificationResults);
                    }
                }
                catch (Exception)
                {
                    ResultValue = false;
                }
            }

            if (ResultValue && NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return ResultValue;
        }

        /// <summary>
        /// Creates a new extract by subractin a list of given extracts from a base extract.
        /// </summary>
        /// <param name="ANewExtractName">Name of the Extract to be created.</param>
        /// <param name="ANewExtractDescription">Description of the Extract to be created.</param>
        /// <param name="ABaseExtractName">Base extract to subtract other extracts from.</param>
        /// <param name="ASubtractExtractIdList">List of Ids of extracts to be subtracted from base extract.</param>
        /// <param name="ANewExtractId">Extract Id of the created Extract, or -1 if the
        /// creation of the Extract was not successful.</param>
        /// <param name="AVerificationResults">Nil if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions).</param>
        /// <returns>True if the new intersected Extract was created, otherwise false.</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean SubtractExtracts(String ANewExtractName,
            String ANewExtractDescription,
            String ABaseExtractName,
            List <Int32>ASubtractExtractIdList,
            out Int32 ANewExtractId,
            out TVerificationResultCollection AVerificationResults)
        {
            Boolean ResultValue = true;
            Boolean ExtractAlreadyExists = false;
            MExtractTable ExtractTable;
            MExtractMasterTable BaseExtractMasterTable;
            MExtractTable BaseExtractTable;
            MExtractTable SubtractedExtractTable = new MExtractTable();
            MExtractRow TemplateRow;
            Boolean NewTransaction;

            List <Int64>SubtractPartnerKeyList = new List <Int64>();

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            ANewExtractId = -1;
            AVerificationResults = null;


            ResultValue = MPartner.Extracts.TExtractsHandling.CreateNewExtract(ANewExtractName,
                ANewExtractDescription, out ANewExtractId, out ExtractAlreadyExists, out AVerificationResults);

            if (ResultValue && !ExtractAlreadyExists)
            {
                // first create a table that contains all partners to be subtracted
                foreach (Int32 ExtractId in ASubtractExtractIdList)
                {
                    ExtractTable = MExtractAccess.LoadViaMExtractMaster(ExtractId, WriteTransaction);

                    foreach (DataRow ExtractRow in ExtractTable.Rows)
                    {
                        if (!SubtractPartnerKeyList.Exists(item => item == ((MExtractRow)ExtractRow).PartnerKey))
                        {
                            SubtractPartnerKeyList.Add(((MExtractRow)ExtractRow).PartnerKey);
                        }
                    }
                }

                if (ASubtractExtractIdList.Count > 0)
                {
                    BaseExtractMasterTable = MExtractMasterAccess.LoadByUniqueKey(ABaseExtractName, WriteTransaction);
                    BaseExtractTable = MExtractAccess.LoadViaMExtractMaster(((MExtractMasterRow)BaseExtractMasterTable.Rows[0]).ExtractId,
                        WriteTransaction);

                    // iterate through all partners in base extract and check if this record also exists in extracts to be subtracted
                    foreach (DataRow ExtractRow in BaseExtractTable.Rows)
                    {
                        if (!SubtractPartnerKeyList.Exists(item => item == ((MExtractRow)ExtractRow).PartnerKey))
                        {
                            // if partner key does not exist in list to subtract then add it to result extract
                            TemplateRow = (MExtractRow)SubtractedExtractTable.NewRowTyped(true);
                            TemplateRow.ExtractId = ANewExtractId;
                            TemplateRow.PartnerKey = ((MExtractRow)ExtractRow).PartnerKey;
                            TemplateRow.SiteKey = ((MExtractRow)ExtractRow).SiteKey;
                            TemplateRow.LocationKey = ((MExtractRow)ExtractRow).LocationKey;

                            SubtractedExtractTable.Rows.Add(TemplateRow);
                        }
                    }
                }

                try
                {
                    // update key count in master table
                    MExtractMasterTable IntersectedExtractMaster = MExtractMasterAccess.LoadByPrimaryKey(ANewExtractId, WriteTransaction);
                    IntersectedExtractMaster[0].KeyCount = SubtractedExtractTable.Rows.Count;

                    // submit changes in master and then in extract content table which refers to it
                    if (MExtractAccess.SubmitChanges(SubtractedExtractTable, WriteTransaction, out AVerificationResults))
                    {
                        ResultValue = MExtractMasterAccess.SubmitChanges(IntersectedExtractMaster, WriteTransaction, out AVerificationResults);
                    }
                }
                catch (Exception)
                {
                    ResultValue = false;
                }
            }

            if (ResultValue && NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return ResultValue;
        }
    }
}