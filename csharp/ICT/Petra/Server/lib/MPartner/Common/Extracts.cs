//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2011 by OM International
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
using Ict.Petra.Server.MCommon.Data.Cascading;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
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

#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine("CreateNewExtract called!");
            }
#endif

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
#if DEBUGMODE
            catch (Exception e)
            {
                ReturnValue = false;

                if (TLogging.DL >= 8)
                {
                    Console.WriteLine(
                        "TExtractsHandling.CreateNewExtract: Exception occured, Transaction ROLLED BACK. Exception: " +
                        e.ToString());
                }
            }
#else
            catch (Exception)
            {
                ReturnValue = false;
            }
#endif

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
#if DEBUGMODE
            catch (Exception Exp)
            {
                if (TLogging.DL >= 8)
                {
                    Console.WriteLine("TExtractsHandling.DeleteExtract: Exception occured. Exception: " + Exp.ToString());
                }

                throw;
            }
#endif
            finally
            {
                if (Success)
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TLogging.DL >= 8)
                        {
                            Console.WriteLine("TExtractsHandling.DeleteExtract: committed own transaction!");
                        }
#endif
                    }
                }
                else
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
#if DEBUGMODE
                        if (TLogging.DL >= 8)
                        {
                            Console.WriteLine("TExtractsHandling.DeleteExtract: ROLLED BACK own transaction!");
                        }
#endif
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
#if DEBUGMODE
            catch (Exception Exp)
            {
                if (TLogging.DL >= 8)
                {
                    Console.WriteLine("TExtractsHandling.CheckExtractExists(AExtractName): Exception occured. Exception: " + Exp.ToString());
                }

                throw;
            }
#endif
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                    if (TLogging.DL >= 8)
                    {
                        Console.WriteLine("TExtractsHandling.CheckExtractExists(AExtractName): committed own transaction!");
                    }
#endif
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
#if DEBUGMODE
                    Console.WriteLine(
                        "TExtractsHandling.GetExtractKeyCount: committed own transaction.");
#endif
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
#if DEBUGMODE
            catch (Exception Exp)
            {
                if (TLogging.DL >= 8)
                {
                    Console.WriteLine("TExtractsHandling.UpdateExtractCount: Exception occured. Exception: " + Exp.ToString());
                }

                throw;
            }
#endif
            finally
            {
                if (Success)
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TLogging.DL >= 8)
                        {
                            Console.WriteLine("TExtractsHandling.UpdateExtractCount: committed own transaction!");
                        }
#endif
                    }
                }
                else
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
#if DEBUGMODE
                        if (TLogging.DL >= 8)
                        {
                            Console.WriteLine("TExtractsHandling.UpdateExtractCount: ROLLED BACK own transaction!");
                        }
#endif
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


#if DEBUGMODE
            if (TLogging.DL >= 8)
            {
                Console.WriteLine(
                    "TExtractsHandling.AddPartnerToExtract:  BestAddressPK: " + BestAddressPK.SiteKey.ToString() + ", " +
                    BestAddressPK.LocationKey.ToString());
            }
#endif

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
#if DEBUGMODE
                catch (Exception Exp)
                {
                    if (TLogging.DL >= 8)
                    {
                        Console.WriteLine("TExtractsHandling.AddPartnerToExtract: Exception occured. Exception: " + Exp.ToString());
                    }

                    throw;
                }
#endif
                finally
                {
                    if (Success)
                    {
                        if (NewTransaction)
                        {
                            DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                            if (TLogging.DL >= 8)
                            {
                                Console.WriteLine("TExtractsHandling.AddPartnerToExtract: committed own transaction!");
                            }
#endif
                        }
                    }
                    else
                    {
                        if (NewTransaction)
                        {
                            DBAccess.GDBAccessObj.RollbackTransaction();
#if DEBUGMODE
                            if (TLogging.DL >= 8)
                            {
                                Console.WriteLine("TExtractsHandling.AddPartnerToExtract: ROLLED BACK own transaction!");
                            }
#endif
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
        /// <returns>True if the new Extract was created, otherwise false.</returns>
        public static bool CreateExtractFromListOfPartnerKeys(
            String AExtractName,
            String AExtractDescription,
            out Int32 ANewExtractId,
            out TVerificationResultCollection AVerificationResults,
            DataTable APartnerKeysTable,
            Int32 APartnerKeyColumn)
        {
            Boolean NewTransaction;

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable,
                TEnforceIsolationLevel.eilMinimum, out NewTransaction);

            ANewExtractId = -1;
            bool ExtractAlreadyExists;

            bool ResultValue = CreateNewExtract(AExtractName,
                AExtractDescription,
                out ANewExtractId,
                out ExtractAlreadyExists,
                out AVerificationResults);

            if (ResultValue)
            {
                MExtractTable ExtractTable = new MExtractTable();

                foreach (DataRow partnerRow in APartnerKeysTable.Rows)
                {
                    // get bestaddresses for the partners
                    Int64 partnerkey = Convert.ToInt64(partnerRow[APartnerKeyColumn]);
                    TVerificationResultCollection LocalVerification;
                    TLocationPK locationPK = TMailing.GetPartnersBestLocation(partnerkey, out LocalVerification);

                    MExtractRow NewRow = ExtractTable.NewRowTyped(false);
                    NewRow.ExtractId = ANewExtractId;
                    NewRow.PartnerKey = partnerkey;
                    NewRow.SiteKey = locationPK.SiteKey;
                    NewRow.LocationKey = locationPK.LocationKey;
                    ExtractTable.Rows.Add(NewRow);
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
    }
}