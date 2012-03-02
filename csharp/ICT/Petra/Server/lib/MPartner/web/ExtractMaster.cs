//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Common;
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
	            SqlStmt = "SELECT pub_" + MExtractTable.GetTableDBName() + ".*"
	            			+ ", pub_" + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerShortNameDBName()
	            			+ ", pub_" + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerClassDBName()
	            			+ " FROM pub_" + MExtractTable.GetTableDBName() + ", pub_" + PPartnerTable.GetTableDBName()
	            			+ " WHERE pub_" + MExtractTable.GetTableDBName() + "." + MExtractTable.GetExtractIdDBName()
	            			+ " = " + AExtractId.ToString()
	            			+ " AND pub_" + MExtractTable.GetTableDBName() + "." + MExtractTable.GetPartnerKeyDBName()
	            			+ " = pub_" + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName();
	            
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
                	   needs to be removed from the table with AcceptChanges as otherwise the later call
                	   to SubmitChanges will complain about those rows that have already been deleted in 
                	   the database.
                	   Use a loop to run through the table in reverse Order (Index--) so that the rows
                	   can actually be removed from the table without affecting the access throug Index. */
                	CountRecords = AExtractMasterTable.Rows.Count;
                	for (int Index = CountRecords-1; Index >= 0; Index--)
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
                    if (AExtractTable.Rows.Count > 0
                        && SubmissionResult != TSubmitChangesResult.scrError)
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
    }
}