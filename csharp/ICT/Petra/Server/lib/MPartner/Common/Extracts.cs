//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Data;
using System.Collections.Generic;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon.Data.Cascading;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Common;

namespace Ict.Petra.Server.MPartner.Extracts
{
    /// <summary>
    /// Contains Partner Module Extracts handling Business Objects.
    /// These Business Objects handle the retrieval, verification and saving of data.
    ///
    /// @Comment These Business Objects can be instantiated by other Server Objects
    ///          (usually UIConnectors) and also directly from the Instantiator
    ///          classes (only when it makes sense AND they derive from
    ///          TConfigurableMBRObject).
    /// </summary>
    public static class TExtractsHandling
    {
        /// <summary>
        /// Creates a new extract with the given extract name and extract description in the
        /// m_extract_master data table. The Extract Id and the Extract Name must both be
        /// unique in the Petra Database.
        /// </summary>
        /// <param name="AExtractName">Name of the Extract to be created.</param>
        /// <param name="AExtractDescription">Description of the Extract to be created.</param>
        /// <param name="ANewExtractId">Extract Id of the created Extract, or -1 if the
        /// creation of the Extract was not successful.</param>
        /// <param name="AExtractAlreadyExists">True if there is already an extract with
        /// the given name, otherwise false.</param>
        /// <param name="AVerificationResults">Nil if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions).</param>
        /// <returns>True if the new Extract was created, otherwise false.</returns>
        public static bool CreateNewExtract(String AExtractName,
            String AExtractDescription,
            out Int32 ANewExtractId,
            out Boolean AExtractAlreadyExists,
            out TVerificationResultCollection AVerificationResults)
        {
            TDBTransaction WriteTransaction;
            Boolean NewTransaction;
            Boolean ReturnValue = false;

            ANewExtractId = -1;
            AExtractAlreadyExists = false;
            AVerificationResults = null;
            TLogging.LogAtLevel(9, "CreateNewExtract called!");
            WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                // Check if there is already an extract with the extract name
                if (!CheckExtractExists(AExtractName))
                {
                    // The extract name is unique. So create the new extract...
                    MExtractMasterTable NewExtractMasterDT = new MExtractMasterTable();

                    MExtractMasterRow TemplateRow = (MExtractMasterRow)NewExtractMasterDT.NewRowTyped(true);
                    TemplateRow.ExtractName = AExtractName;
                    TemplateRow.ExtractDesc = AExtractDescription;
                    TemplateRow.ExtractId = -1;   // initialize id negative so sequence can be used

                    NewExtractMasterDT.Rows.Add(TemplateRow);

                    if (!MExtractMasterAccess.SubmitChanges(NewExtractMasterDT, WriteTransaction, out AVerificationResults))
                    {
                        // something went wrong
                        ReturnValue = false;
                    }
                    else
                    {
                        // Get the Extract Id
                        TemplateRow = (MExtractMasterRow)NewExtractMasterDT.Rows[0];
                        ANewExtractId = TemplateRow.ExtractId;
                        ReturnValue = true;
                    }
                }
                else
                {
                    AExtractAlreadyExists = true;
                    ReturnValue = false;
                }
            }
            catch (Exception e)
            {
                ReturnValue = false;
                TLogging.LogAtLevel(8, "TExtractsHandling.CreateNewExtract: Exception occured, Transaction ROLLED BACK. Exception: " + e.ToString());
            }

            if (ReturnValue && NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else if (NewTransaction)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return ReturnValue;
        }

        /// <summary>
        /// Deletes an Extract.
        /// </summary>
        /// <param name="AExtractId">ExtractId of the Extract that should get
        /// deleted.</param>
        /// <param name="AExtractNotDeletable">True if the Deletable Flag of the
        /// Extract if false.</param>
        /// <param name="AVerificationResult">Nil if all verifications are OK,
        /// otherwise filled with a TVerificationResult object.</param>
        /// <returns>True if the Extract was deleted, otherwise false.</returns>
        public static bool DeleteExtract(int AExtractId,
            out bool AExtractNotDeletable,
            out TVerificationResult AVerificationResult)
        {
            TDBTransaction WriteTransaction;
            MExtractMasterTable ExtractMasterDT;
            Boolean NewTransaction;
            Boolean Success = false;

            AVerificationResult = null;
            AExtractNotDeletable = false;

            WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                ExtractMasterDT = MExtractMasterAccess.LoadByPrimaryKey(AExtractId,
                    WriteTransaction);

                if (ExtractMasterDT.Rows.Count == 1)
                {
                    if (ExtractMasterDT[0].Deletable)
                    {
                        MExtractMasterCascading.DeleteByPrimaryKey(AExtractId,
                            WriteTransaction, true);
                        Success = true;
                    }
                    else
                    {
                        AExtractNotDeletable = true;
                        Success = false;
                    }
                }
                else
                {
                    AVerificationResult = new TVerificationResult(
                        "TExtractsHandling.DeleteExtract", "Extract with Extract Id " + AExtractId.ToString() +
                        "doesn't exist!", TResultSeverity.Resv_Critical);
                    Success = false;
                }
            }
            catch (Exception Exp)
            {
                TLogging.LogAtLevel(8, "TExtractsHandling.DeleteExtract: Exception occured: " + Exp.ToString());
                throw;
            }
            finally
            {
                if (Success)
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel(8, "TExtractsHandling.DeleteExtract: committed own transaction!");
                    }
                }
                else
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                        TLogging.LogAtLevel(8, "TExtractsHandling.DeleteExtract: ROLLED BACK own transaction!");
                    }
                }
            }

            return Success;
        }

        /// <summary>
        /// Checks whether an Extract with a certain Extract Name exists.
        /// </summary>
        /// <param name="AExtractName">Name of the Extract to check for.</param>
        /// <returns>True if an Extract with the specified Extract Name exists,
        /// otherwise false.</returns>
        public static bool CheckExtractExists(string AExtractName)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            Boolean ReturnValue = false;
            MExtractMasterRow TemplateRow;

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            // Check if there is already an extract with the extract name
            try
            {
                TemplateRow = new MExtractMasterTable().NewRowTyped(false);
                TemplateRow.ExtractName = AExtractName;

                if (MExtractMasterAccess.CountUsingTemplate(TemplateRow, null, ReadTransaction) > 0)
                {
                    ReturnValue = true;
                }
            }
            catch (Exception Exp)
            {
                TLogging.LogAtLevel(8, "TExtractsHandling.CheckExtractExists: Exception occured: " + Exp.ToString());
                throw;
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(8, "TExtractsHandling.CheckExtractExists: committed own transaction!");
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks whether an Extract with a certain Extract Id exists.
        /// </summary>
        /// <param name="AExtractId"></param>
        ///  <returns>True if an Extract with the specified Extract Id exists,
        /// otherwise false.</returns>
        public static bool CheckExtractExists(int AExtractId)
        {
            if (GetExtractKeyCount(AExtractId) != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the Key Count of an Extract.
        /// </summary>
        /// <param name="AExtractId">ExtractId of the Extract that the KeyCount should
        /// get returned for.</param>
        /// <returns>The Key Count of the Extract, or -1 if the Extract with the given
        /// Extract Id doesn't exist.</returns>
        public static Int32 GetExtractKeyCount(int AExtractId)
        {
            Boolean NewTransaction;

            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                MExtractMasterTable ExtractDT = MExtractMasterAccess.LoadByPrimaryKey(AExtractId, ReadTransaction);

                if (ExtractDT.Rows.Count == 1)
                {
                    return ExtractDT[0].KeyCount;
                }
                else
                {
                    return -1;
                }
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(8, "TExtractsHandling.GetExtractKeyCount: committed own transaction.");
                }
            }
        }

        /// <summary>
        /// Updates the KeyCount on an Extract.
        /// </summary>
        /// <param name="AExtractId">ExtractId of the Extract that the KeyCount should
        /// get updated for.</param>
        /// <param name="ACount">New Key Count.</param>
        /// <param name="AVerificationResult">Contains errors or DB call exceptions, if there are any.</param>
        /// <returns>True if the updating was successful, otherwise false.</returns>
        public static bool UpdateExtractKeyCount(int AExtractId, int ACount,
            out TVerificationResultCollection AVerificationResult)
        {
            Boolean NewTransaction;
            Boolean Success = false;

            AVerificationResult = null;

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                MExtractMasterTable ExtractMasterDT = MExtractMasterAccess.LoadByPrimaryKey(AExtractId,
                    WriteTransaction);

                if (ExtractMasterDT.Rows.Count == 1)
                {
                    ExtractMasterDT[0].KeyCount = ACount;

                    if (!MExtractMasterAccess.SubmitChanges(ExtractMasterDT, WriteTransaction,
                            out AVerificationResult))
                    {
                        Success = false;
                    }
                    else
                    {
                        Success = true;
                    }
                }
                else
                {
                    AVerificationResult.Add(new TVerificationResult(
                            "TExtractsHandling.UpdateExtractCount", "Extract with Extract Id " + AExtractId.ToString() +
                            "doesn't exist!", TResultSeverity.Resv_Critical));
                    Success = false;
                }
            }
            catch (Exception Exp)
            {
                TLogging.LogAtLevel(8, "TExtractsHandling.UpdateExtractCount: Exception occured. Exception: " + Exp.ToString());
                throw;
            }
            finally
            {
                if (Success)
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel(8, "TExtractsHandling.UpdateExtractCount: committed own transaction!");
                    }
                }
                else
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                        TLogging.LogAtLevel(8, "TExtractsHandling.UpdateExtractCount: ROLLED BACK own transaction!");
                    }
                }
            }

            return Success;
        }

        /// <summary>
        /// Adds a Partner to an Extract, if they are not already present
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner</param>
        /// <param name="AExtractId">ExtractId of the Extract that the Partner should
        /// get added to.</param>
        /// <param name="AVerificationResult">Contains DB call exceptions, if there are any.</param>
        /// <returns>True if the Partner was added to the Extract, otherwise false.</returns>
        public static bool AddPartnerToExtract(Int64 APartnerKey,
            int AExtractId, out TVerificationResultCollection AVerificationResult)
        {
            TLocationPK BestAddressPK;

            AVerificationResult = null;

            BestAddressPK = TMailing.GetPartnersBestLocation(APartnerKey, out AVerificationResult);
//          TLogging.LogAtLevel(8, "TExtractsHandling.AddPartnerToExtract:  BestAddressPK: " + BestAddressPK.SiteKey.ToString() + ", " + BestAddressPK.LocationKey.ToString());

            if (BestAddressPK.LocationKey != -1)
            {
                return AddPartnerToExtract(APartnerKey, BestAddressPK,
                    AExtractId, out AVerificationResult);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a Partner to an Extract, if they are not already present
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner</param>
        /// <param name="ALocationPK">Location PK of a Partner's PartnerLocation
        /// (usually the LocationPK of the 'Best Address' of the Partner).</param>
        /// <param name="AExtractId">ExtractId of the Extract that the Partner should
        /// get added to.</param>
        /// <param name="AVerificationResult">Contains DB call exceptions, if there are any.</param>
        /// <returns>True if the Partner was added to the Extract, otherwise false.</returns>
        public static bool AddPartnerToExtract(Int64 APartnerKey,
            TLocationPK ALocationPK, int AExtractId,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction WriteTransaction;
            MExtractTable TemplateTable;
            MExtractRow TemplateRow;
            MExtractRow NewRow;
            Boolean NewTransaction;
            Boolean Success = false;

            AVerificationResult = null;

            if (APartnerKey > 0)
            {
                WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    IsolationLevel.Serializable,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                try
                {
                    /*
                     * First check whether the Partner isn't already in that Extract
                     */
                    TemplateTable = new MExtractTable();
                    TemplateRow = TemplateTable.NewRowTyped(false);
                    TemplateRow.ExtractId = AExtractId;
                    TemplateRow.PartnerKey = APartnerKey;

                    if (MExtractAccess.CountUsingTemplate(TemplateRow, null, WriteTransaction) == 0)
                    {
                        /*
                         * Add Partner to Extract.
                         */
                        NewRow = TemplateTable.NewRowTyped(false);
                        NewRow.ExtractId = AExtractId;
                        NewRow.PartnerKey = APartnerKey;
                        NewRow.SiteKey = ALocationPK.SiteKey;
                        NewRow.LocationKey = ALocationPK.LocationKey;
                        TemplateTable.Rows.Add(NewRow);

                        if (!MExtractAccess.SubmitChanges(TemplateTable, WriteTransaction,
                                out AVerificationResult))
                        {
                            Success = false;
                            return false;
                        }
                        else
                        {
                            Success = true;
                            return true;
                        }
                    }
                    else
                    {
                        Success = true;

                        // Partner is already in that Extract -> Partner does not get added.
                        return false;
                    }
                }
                catch (Exception Exp)
                {
                    TLogging.LogAtLevel(8, "TExtractsHandling.AddPartnerToExtract: Exception occured. Exception: " + Exp.ToString());
                    throw;
                }
                finally
                {
                    if (Success)
                    {
                        if (NewTransaction)
                        {
                            DBAccess.GDBAccessObj.CommitTransaction();
                            TLogging.LogAtLevel(8, "TExtractsHandling.AddPartnerToExtract: committed own transaction!");
                        }
                    }
                    else
                    {
                        if (NewTransaction)
                        {
                            DBAccess.GDBAccessObj.RollbackTransaction();
                            TLogging.LogAtLevel(8, "TExtractsHandling.AddPartnerToExtract: ROLLED BACK own transaction!");
                        }
                    }
                }
            }
            else
            {
                // Invalid PartnerKey -> return false;
                return false;
            }
        }

        /// <summary>
        /// create an extract from a list of best addresses
        /// </summary>
        /// <param name="AExtractName">Name of the Extract to be created.</param>
        /// <param name="AExtractDescription">Description of the Extract to be created.</param>
        /// <param name="ANewExtractId">Extract Id of the created Extract, or -1 if the
        /// creation of the Extract was not successful.</param>
        /// <param name="AExtractAlreadyExists">True if there is already an extract with
        /// the given name, otherwise false.</param>
        /// <param name="AVerificationResults">Nil if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions).</param>
        /// <param name="ABestAddressTable"></param>
        /// <param name="AIncludeNonValidAddresses">you might want to include invalid addresses if an email was sent</param>
        /// <returns>True if the new Extract was created, otherwise false.</returns>
        public static bool CreateExtractFromBestAddressTable(
            String AExtractName,
            String AExtractDescription,
            out Int32 ANewExtractId,
            out Boolean AExtractAlreadyExists,
            out TVerificationResultCollection AVerificationResults,
            BestAddressTDSLocationTable ABestAddressTable,
            bool AIncludeNonValidAddresses)
        {
            Boolean NewTransaction;

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            ANewExtractId = -1;

            bool ResultValue = CreateNewExtract(AExtractName,
                AExtractDescription,
                out ANewExtractId,
                out AExtractAlreadyExists,
                out AVerificationResults);

            if (ResultValue)
            {
                MExtractTable ExtractTable = new MExtractTable();

                foreach (BestAddressTDSLocationRow row in ABestAddressTable.Rows)
                {
                    if (AIncludeNonValidAddresses || row.ValidAddress)
                    {
                        MExtractRow NewRow = ExtractTable.NewRowTyped(false);
                        NewRow.ExtractId = ANewExtractId;
                        NewRow.PartnerKey = row.PartnerKey;
                        NewRow.SiteKey = row.SiteKey;
                        NewRow.LocationKey = row.LocationKey;
                        ExtractTable.Rows.Add(NewRow);
                    }
                }

                if (ExtractTable.Rows.Count > 0)
                {
                    try
                    {
                        MExtractMasterTable ExtractMaster = MExtractMasterAccess.LoadByPrimaryKey(ANewExtractId, WriteTransaction);
                        ExtractMaster[0].KeyCount = ExtractTable.Rows.Count;

                        if (MExtractAccess.SubmitChanges(ExtractTable, WriteTransaction, out AVerificationResults))
                        {
                            ResultValue = MExtractMasterAccess.SubmitChanges(ExtractMaster, WriteTransaction, out AVerificationResults);
                        }
                    }
                    catch (Exception)
                    {
                        ResultValue = false;
                    }
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
        /// create an extract from a list of best addresses
        /// </summary>
        /// <param name="AExtractName">Name of the Extract to be created.</param>
        /// <param name="AExtractDescription">Description of the Extract to be created.</param>
        /// <param name="ANewExtractId">Extract Id of the created Extract, or -1 if the
        /// creation of the Extract was not successful.</param>
        /// <param name="AVerificationResults">Nil if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions).</param>
        /// <param name="APartnerKeysTable"></param>
        /// <param name="APartnerKeyColumn">number of the column that contains the partner keys</param>
        /// <param name="AAddressFilterAdded">true if location key fields exist in APartnerKeysTable</param>
        /// <param name="ACommitTransaction">true if transaction should committed at end of method</param>
        /// <returns>True if the new Extract was created, otherwise false.</returns>
        public static bool CreateExtractFromListOfPartnerKeys(
            String AExtractName,
            String AExtractDescription,
            out Int32 ANewExtractId,
            out TVerificationResultCollection AVerificationResults,
            DataTable APartnerKeysTable,
            Int32 APartnerKeyColumn,
            bool AAddressFilterAdded,
            bool ACommitTransaction)
        {
            if (AAddressFilterAdded)
            {
                // if address filter was added then site key is in third and location in fourth column
                return CreateExtractFromListOfPartnerKeys(AExtractName, AExtractDescription, out ANewExtractId,
                    out AVerificationResults, APartnerKeysTable,
                    APartnerKeyColumn, 2, 3, ACommitTransaction);
            }
            else
            {
                // if no address filter was added (no location keys were added): set location and site key to -1
                return CreateExtractFromListOfPartnerKeys(AExtractName, AExtractDescription, out ANewExtractId,
                    out AVerificationResults, APartnerKeysTable,
                    APartnerKeyColumn, -1, -1, ACommitTransaction);
            }
        }

        /// <summary>
        /// create an extract from a list of best addresses
        /// </summary>
        /// <param name="AExtractName">Name of the Extract to be created.</param>
        /// <param name="AExtractDescription">Description of the Extract to be created.</param>
        /// <param name="ANewExtractId">Extract Id of the created Extract, or -1 if the
        /// creation of the Extract was not successful.</param>
        /// <param name="AVerificationResults">Nil if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions).</param>
        /// <param name="APartnerKeysTable"></param>
        /// <param name="APartnerKeyColumn">number of the column that contains the partner keys</param>
        /// <param name="ASiteKeyColumn">number of the column that contains the site keys</param>
        /// <param name="ALocationKeyColumn">number of the column that contains the location keys</param>
        /// <param name="ACommitTransaction">true if transaction should committed at end of method</param>
        /// <returns>True if the new Extract was created, otherwise false.</returns>
        public static bool CreateExtractFromListOfPartnerKeys(
            String AExtractName,
            String AExtractDescription,
            out Int32 ANewExtractId,
            out TVerificationResultCollection AVerificationResults,
            DataTable APartnerKeysTable,
            Int32 APartnerKeyColumn,
            Int32 ASiteKeyColumn,
            Int32 ALocationKeyColumn,
            bool ACommitTransaction)
        {
            ANewExtractId = -1;
            bool ExtractAlreadyExists;

            // create new extract master record
            bool ResultValue = CreateNewExtract(AExtractName,
                AExtractDescription,
                out ANewExtractId,
                out ExtractAlreadyExists,
                out AVerificationResults);

            if (ResultValue)
            {
                ResultValue = ExtendExtractFromListOfPartnerKeys(ANewExtractId, out AVerificationResults,
                    APartnerKeysTable, APartnerKeyColumn, ASiteKeyColumn, ALocationKeyColumn,
                    true, ACommitTransaction);
            }

            return ResultValue;
        }

        /// <summary>
        /// create an extract from a list of best addresses
        /// </summary>
        /// <param name="AExtractId">Extract Id of the Extract to extend</param>
        /// <param name="AVerificationResults">Nil if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions).</param>
        /// <param name="APartnerKeysTable"></param>
        /// <param name="APartnerKeyColumn">number of the column that contains the partner keys</param>
        /// <param name="AAddressFilterAdded">true if location key fields exist in APartnerKeysTable</param>
        /// <param name="AIgnoreDuplicates">true if duplicates should be looked out for. Can be set to false if called only once and not several times per extract.</param>
        /// <param name="ACommitTransaction">true if transaction should be committed at end of method</param>
        /// <returns>True if the new Extract was created, otherwise false.</returns>
        public static bool ExtendExtractFromListOfPartnerKeys(
            Int32 AExtractId,
            out TVerificationResultCollection AVerificationResults,
            DataTable APartnerKeysTable,
            Int32 APartnerKeyColumn,
            bool AAddressFilterAdded,
            bool AIgnoreDuplicates,
            bool ACommitTransaction)
        {
            if (AAddressFilterAdded)
            {
                // if address filter was added then site key is in third and location in fourth column
                return ExtendExtractFromListOfPartnerKeys(AExtractId,
                    out AVerificationResults, APartnerKeysTable,
                    APartnerKeyColumn, 2, 3, AIgnoreDuplicates, ACommitTransaction);
            }
            else
            {
                // if no address filter was added (no location keys were added): set location and site key to -1
                return ExtendExtractFromListOfPartnerKeys(AExtractId,
                    out AVerificationResults, APartnerKeysTable,
                    APartnerKeyColumn, -1, -1, AIgnoreDuplicates, ACommitTransaction);
            }
        }

        /// <summary>
        /// extend an extract from a list of best addresses
        /// </summary>
        /// <param name="AExtractId">Extract Id of the Extract to extend</param>
        /// <param name="AVerificationResults">Nil if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions).</param>
        /// <param name="APartnerKeysTable"></param>
        /// <param name="APartnerKeyColumn">number of the column that contains the partner keys</param>
        /// <param name="ASiteKeyColumn">number of the column that contains the site keys</param>
        /// <param name="ALocationKeyColumn">number of the column that contains the location keys</param>
        /// <param name="AIgnoreDuplicates">true if duplicates should be looked out for. Can be set to false if called only once and not several times per extract.</param>
        /// <param name="ACommitTransaction">true if transaction should be committed at end of method</param>
        /// <returns>True if the extract was successfully extended, otherwise false.</returns>
        public static bool ExtendExtractFromListOfPartnerKeys(
            Int32 AExtractId,
            out TVerificationResultCollection AVerificationResults,
            DataTable APartnerKeysTable,
            Int32 APartnerKeyColumn,
            Int32 ASiteKeyColumn,
            Int32 ALocationKeyColumn,
            bool AIgnoreDuplicates,
            bool ACommitTransaction)
        {
            bool ResultValue = true;
            Boolean NewTransaction;
            int RecordCounter = 0;
            PPartnerLocationTable PartnerLocationKeysTable;
            Int64 PartnerKey;

            AVerificationResults = null;

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            try
            {
                MExtractTable ExtractTable = new MExtractTable();
                ExtractTable = MExtractAccess.LoadViaMExtractMaster(AExtractId, WriteTransaction);

                // Location Keys need to be determined as extracts do not only need partner keys but
                // also Location Keys.
                DetermineBestLocationKeys(APartnerKeysTable, APartnerKeyColumn, ASiteKeyColumn,
                    ALocationKeyColumn, out PartnerLocationKeysTable,
                    WriteTransaction);

                // use the returned table which contains partner and location keys to build the extract
                foreach (PPartnerLocationRow PartnerLocationRow in PartnerLocationKeysTable.Rows)
                {
                    PartnerKey = Convert.ToInt64(PartnerLocationRow[PPartnerLocationTable.GetPartnerKeyDBName()]);

                    if (PartnerKey > 0)
                    {
                        RecordCounter += 1;
                        TLogging.Log("Preparing Partner " + PartnerKey.ToString() + " (Record Number " + RecordCounter.ToString() + ")");

                        // add row for partner to extract and fill with contents
                        MExtractRow NewRow = ExtractTable.NewRowTyped();
                        NewRow.ExtractId = AExtractId;
                        NewRow.PartnerKey = PartnerKey;
                        NewRow.SiteKey = Convert.ToInt64(PartnerLocationRow[PPartnerLocationTable.GetSiteKeyDBName()]);
                        NewRow.LocationKey = Convert.ToInt32(PartnerLocationRow[PPartnerLocationTable.GetLocationKeyDBName()]);

                        // only add row if it does not already exist for this partner
                        if (AIgnoreDuplicates || !ExtractTable.Rows.Contains(new object[] { NewRow.ExtractId, NewRow.PartnerKey, NewRow.SiteKey }))
                        {
                            ExtractTable.Rows.Add(NewRow);
                        }
                    }
                }

                if (ExtractTable.Rows.Count > 0)
                {
                    // update field in extract master for quick access to number of partners in extract
                    MExtractMasterTable ExtractMaster = MExtractMasterAccess.LoadByPrimaryKey(AExtractId, WriteTransaction);
                    ExtractMaster[0].KeyCount = ExtractTable.Rows.Count;

                    ExtractTable.ThrowAwayAfterSubmitChanges = true; // no need to keep data as this increases speed significantly
                    if (MExtractAccess.SubmitChanges(ExtractTable, WriteTransaction, out AVerificationResults))
                    {
                        ResultValue = MExtractMasterAccess.SubmitChanges(ExtractMaster, WriteTransaction, out AVerificationResults);
                    }
                    else
                    {
                        ResultValue = false;
                    }
                }
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                ResultValue = false;
            }

            if (ACommitTransaction)
            {
                if (ResultValue && NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return ResultValue;
        }

        /// <summary>
        /// finalize extract from a list of best addresses (commit or rollback current transaction)
        /// </summary>
        /// <param name="ACommitTransaction">true if transaction needs to be committed, otherwise rollback</param>
        /// <returns>True if the new Extract was created, otherwise false.</returns>
        public static void FinishExtractFromListOfPartnerKeys(bool ACommitTransaction)
        {
#if TODO
            Boolean NewTransaction;

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            if (ACommitTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
#endif
        }

        /// <summary>
        /// Determine location keys for partners needed for extract, depending on if location information
        /// was retrieved by query or not.
        /// </summary>
        /// <param name="APartnerKeysTable"></param>
        /// <param name="APartnerKeyColumn"></param>
        /// <param name="ASiteKeyColumn"></param>
        /// <param name="ALocationKeyColumn"></param>
        /// <param name="APartnerLocationKeysTable"></param>
        /// <param name="ATransaction"></param>
        private static void DetermineBestLocationKeys(
            DataTable APartnerKeysTable,
            Int32 APartnerKeyColumn,
            Int32 ASiteKeyColumn,
            Int32 ALocationKeyColumn,
            out PPartnerLocationTable APartnerLocationKeysTable,
            TDBTransaction ATransaction)
        {
            Int64 PartnerKey;
            Int64 PreviousPartnerKey;
            Int32 NumberOfPartnerRows = APartnerKeysTable.Rows.Count;
            DataRow partnerRow;

            List <TLocationPK>LocationKeyList = new List <TLocationPK>();

            APartnerLocationKeysTable = new PPartnerLocationTable();

            // don't go further if table is empty
            if (APartnerKeysTable.Rows.Count == 0)
            {
                return;
            }

            // If location column exists then check if there is more than one location key for a partner.
            // If so then determine the best of the found addresses.
            if (ALocationKeyColumn >= 0)
            {
                PreviousPartnerKey = -1;

                // Rows are sorted by partner key. Create a list of location keys per partner and determine
                // the best of those addresses.
                for (int ii=NumberOfPartnerRows-1; ii>=0; ii--)
                {
                    partnerRow = APartnerKeysTable.Rows[ii];
                    PartnerKey = Convert.ToInt64(partnerRow[APartnerKeyColumn]);

                    if ((PartnerKey != PreviousPartnerKey)
                        && (PreviousPartnerKey != -1))
                    {
                        if (!DetermineAndAddBestLocationKey(PreviousPartnerKey, LocationKeyList,
                                                            ref APartnerLocationKeysTable, ATransaction))
                        {
                            // if no address could be found then remove this partner
                            APartnerKeysTable.Rows[ii+1].Delete();
                        }
                        else
                        {
                            // add first location for next partner
                            LocationKeyList.Clear();
                            LocationKeyList.Add(new TLocationPK(Convert.ToInt64(partnerRow[ASiteKeyColumn]),
                                    Convert.ToInt32(partnerRow[ALocationKeyColumn])));
                        }

                    }
                    else
                    {
                        // add location for this partner
                        LocationKeyList.Add(new TLocationPK(Convert.ToInt64(partnerRow[ASiteKeyColumn]),
                                Convert.ToInt32(partnerRow[ALocationKeyColumn])));
                    }

                    // prepare for next round of loop
                    PreviousPartnerKey = PartnerKey;
                }

                // process last partner key after loop through all records
                if (!DetermineAndAddBestLocationKey(PreviousPartnerKey, LocationKeyList,
                                                    ref APartnerLocationKeysTable, ATransaction))
                {
                    // if no address could be found then remove this partner
                    APartnerKeysTable.Rows[0].Delete();
                }
            }
            else
            {
                // If no location information was retrieved with earlier query then find best address
                // for partner.
                for (int ii=NumberOfPartnerRows-1; ii>=0; ii--)
                {
                    partnerRow = APartnerKeysTable.Rows[ii];
                    PartnerKey = Convert.ToInt64(partnerRow[APartnerKeyColumn]);

                    if (!DetermineAndAddBestLocationKey(PartnerKey, LocationKeyList,
                                                        ref APartnerLocationKeysTable, ATransaction))
                    {
                        // if no address could be found then remove this partner
                        partnerRow.Delete();
                    }
                }
            }
        }

        /// <summary>
        /// Determine best location for partner out of a list of possible locations. Or simply find best one
        /// if no suggestion is made.
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="ALocationKeyList"></param>
        /// <param name="APartnerLocationKeysTable"></param>
        /// <param name="ATransaction"></param>
        /// <returns>True if the address was found and added, otherwise false.</returns>
        private static Boolean DetermineAndAddBestLocationKey(
            Int64 APartnerKey,
            List <TLocationPK>ALocationKeyList,
            ref PPartnerLocationTable APartnerLocationKeysTable,
            TDBTransaction ATransaction)
        {
            PPartnerLocationTable AllPartnerLocationTable;
            PPartnerLocationTable FilteredPartnerLocationTable = new PPartnerLocationTable();
            TLocationPK LocationPK = new TLocationPK();
            PPartnerLocationRow PartnerLocationKeyRow;
            PPartnerLocationRow PartnerLocationRowCopy;
            TLocationPK BestLocationPK;

            if (ALocationKeyList.Count == 0)
            {
                // no list suggested: find best address in db for this partner
                TVerificationResultCollection LocalVerification;
                BestLocationPK = TMailing.GetPartnersBestLocation(APartnerKey, out LocalVerification);
            }
            else if (ALocationKeyList.Count == 1)
            {
                // only one location suggested: take this one
                BestLocationPK = ALocationKeyList[0];
            }
            else
            {
                // Process location key list related to partner.
                // In order to use Calculations.DetermineBestAddress we need to first retrieve full data
                // for all suggested records from the db. Therefore load all locations for this partner
                // and then create a table of the ones that are suggested.
                AllPartnerLocationTable = PPartnerLocationAccess.LoadViaPPartner(APartnerKey, ATransaction);

                foreach (PPartnerLocationRow PartnerLocationRow in AllPartnerLocationTable.Rows)
                {
                    LocationPK.SiteKey = PartnerLocationRow.SiteKey;
                    LocationPK.LocationKey = PartnerLocationRow.LocationKey;

                    if (ALocationKeyList.Contains(LocationPK))
                    {
                        PartnerLocationRowCopy = (PPartnerLocationRow)FilteredPartnerLocationTable.NewRow();
                        DataUtilities.CopyAllColumnValues(PartnerLocationRow, PartnerLocationRowCopy);
                        FilteredPartnerLocationTable.Rows.Add(PartnerLocationRowCopy);
                    }
                }

                BestLocationPK = Calculations.DetermineBestAddress(FilteredPartnerLocationTable);
            }

            // create new row, initialize it and add it to the table
            if (BestLocationPK.LocationKey != -1)
            {
                PartnerLocationKeyRow = (PPartnerLocationRow)APartnerLocationKeysTable.NewRow();
                PartnerLocationKeyRow[PPartnerLocationTable.GetPartnerKeyDBName()] = APartnerKey;
                PartnerLocationKeyRow[PPartnerLocationTable.GetSiteKeyDBName()] = BestLocationPK.SiteKey;
                PartnerLocationKeyRow[PPartnerLocationTable.GetLocationKeyDBName()] = BestLocationPK.LocationKey;
                APartnerLocationKeysTable.Rows.Add(PartnerLocationKeyRow);
                return true; 
            }
            else
            {
                return false;                
            }
        }
    }
}