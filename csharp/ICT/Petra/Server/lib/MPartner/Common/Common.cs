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
using System.Collections.Specialized;
using System.Data.Odbc;
using System.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MPartner.Common
{
    /// <summary>
    /// Create a new partner key, depending on the selected field;
    /// make sure, there are no duplicate partner keys
    ///
    /// Behaviour (same as in Petra 2.1, ma1001.w):
    /// A screen allows the user to either use the "default next partner key", or enter a custom partnerkey.
    /// If this is cancelled, nothing happens to the database.
    /// If it is confirmed, then the "default next partner key" is increased in the database.
    /// 2 users on the "partnerkey selection screen" at the same time, get the same partnerkey:
    /// When the second user hits ok, his partner key will be increased, to the next currently free key
    /// If a key is manually entered and it is already being used for a partner,
    /// then the screen should not allow to proceed to the partner edit screen.
    /// Problem (same as in Petra 2.1):
    /// If entering a partner key that already exists, then the ok button on the "partnerkey selection screen" cannot really check.
    /// saving the second one will fail, so you lose the data entered because you need to cancel the edit screen.
    ///
    /// How to use:
    /// First call GetNewPartnerKey to get the "default next partner key"
    /// When the user has made his choice about the partnerkey, call SubmitNewPartnerKey;
    /// if it returns true, use the returned partnerkey (in the var parameter);
    /// otherwise if it returns false and -1 for the partnerkey, the user has to enter a different key
    ///
    /// </summary>
    public class TNewPartnerKey
    {
        /// <summary>
        /// this returns the default next available (highest) partner key of the given field
        /// </summary>
        /// <param name="AFieldPartnerKey">if this is -1, then the sitekey defined in System Parameters is used</param>
        /// <returns>void</returns>
        public static System.Int64 GetNewPartnerKey(System.Int64 AFieldPartnerKey)
        {
            Boolean NewTransaction;

            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            if (AFieldPartnerKey == -1)
            {
                AFieldPartnerKey = DomainManager.GSiteKey;
            }

            PPartnerLedgerTable PartnerLedgerTable = PPartnerLedgerAccess.LoadByPrimaryKey(AFieldPartnerKey, ReadTransaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();

                if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_TRACE)
                {
                    Console.WriteLine("TNewPartnerKey.GetNewPartnerKey: committed own transaction.");
                }
            }

            return PartnerLedgerTable[0].PartnerKey + PartnerLedgerTable[0].LastPartnerId + 1;
        }

        /// <summary>
        /// this checks if the new key is still available,
        /// and makes sure it will not be used as a default key anymore
        /// </summary>
        /// <param name="AFieldPartnerKey"></param>
        /// <param name="AOriginalDefaultKey">this has been previously retrieved from GetNewPartnerKey</param>
        /// <param name="ANewPartnerKey">the user proposes this key for a new partner; the function can change it and return a valid value, or -1</param>
        /// <returns>whether or not ANewPartnerKey has a valid new partner key;
        /// if it cannot be assigned, the function returns false, and ANewPartnerKey is -1
        /// </returns>
        public static bool SubmitNewPartnerKey(System.Int64 AFieldPartnerKey, System.Int64 AOriginalDefaultKey, ref System.Int64 ANewPartnerKey)
        {
            bool ReturnValue;
            TDBTransaction ReadTransaction;
            TDBTransaction WriteTransaction;
            Boolean NewTransaction = false;
            PPartnerLedgerTable PartnerLedgerDT;

            System.Int64 CurrentDefaultPartnerKey;
            TVerificationResultCollection VerificationResult = null;
            Boolean NewPartnerKeySubmissionOK;
            ReturnValue = true;
            NewPartnerKeySubmissionOK = false;

            if (ANewPartnerKey == AOriginalDefaultKey)
            {
                // The user has selected the default
                try
                {
                    // Fetch the partner ledger record to update the last key
                    ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                        TEnforceIsolationLevel.eilMinimum,
                        out NewTransaction);
                    PartnerLedgerDT = PPartnerLedgerAccess.LoadByPrimaryKey(AFieldPartnerKey, ReadTransaction);
                    CurrentDefaultPartnerKey = PartnerLedgerDT[0].PartnerKey + PartnerLedgerDT[0].LastPartnerId + 1;

                    if (ANewPartnerKey != CurrentDefaultPartnerKey)
                    {
                        // Someone else has updated this since, so we will use the new default
                        ANewPartnerKey = CurrentDefaultPartnerKey;
                    }

                    // Now check that this does not exist, and increment until we
                    // find one which does not
                    while (PPartnerAccess.Exists(ANewPartnerKey, ReadTransaction))
                    {
                        ANewPartnerKey = ANewPartnerKey + 1;
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();

                        if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_TRACE)
                        {
                            Console.WriteLine("TNewPartnerKey.SubmitNewPartnerKey: committed own transaction.");
                        }
                    }
                }
                PartnerLedgerDT[0].LastPartnerId = (int)(ANewPartnerKey - PartnerLedgerDT[0].PartnerKey);
                WriteTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    NewPartnerKeySubmissionOK = PPartnerLedgerAccess.SubmitChanges(PartnerLedgerDT, WriteTransaction, out VerificationResult);
                }
                finally
                {
                    if (NewPartnerKeySubmissionOK)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                        throw new ApplicationException(Messages.BuildMessageFromVerificationResult(
                                "An Error occured while creating a new Partner Key:", VerificationResult));
                    }
                }
            }
            // end of: The user has selected the default
            else
            {
                try
                {
                    // check if the Partner Key is already being used
                    ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                        TEnforceIsolationLevel.eilMinimum,
                        out NewTransaction);

                    if (PPartnerAccess.Exists(ANewPartnerKey, ReadTransaction))
                    {
                        ANewPartnerKey = -1;
                        ReturnValue = false;
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();

                        if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_TRACE)
                        {
                            Console.WriteLine("TNewPartnerKey.SubmitNewPartnerKey: committed own transaction.");
                        }
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// reserve a number of partner keys, to be used by the calling function.
        /// useful to create many partner at once, eg. for the demodata
        /// </summary>
        /// <param name="AFieldPartnerKey"></param>
        /// <param name="ANumberOfKeys"></param>
        /// <returns>the first valid partner key to use</returns>
        public static System.Int64 ReservePartnerKeys(System.Int64 AFieldPartnerKey, ref Int32 ANumberOfKeys)
        {
            if (AFieldPartnerKey == -1)
            {
                AFieldPartnerKey = DomainManager.GSiteKey;
            }

            TDBTransaction WriteTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            PPartnerLedgerTable PartnerLedgerDT = PPartnerLedgerAccess.LoadByPrimaryKey(AFieldPartnerKey, WriteTransaction);

            Int64 NextPartnerKey = PartnerLedgerDT[0].PartnerKey + PartnerLedgerDT[0].LastPartnerId + 1;

            Int64 NextUsedKey =
                Convert.ToInt64(DBAccess.GDBAccessObj.ExecuteScalar("SELECT MIN(p_partner_key_n) FROM PUB_p_partner WHERE p_partner_key_n >= " +
                        NextPartnerKey.ToString(), WriteTransaction));

            if (NextUsedKey < NextPartnerKey + ANumberOfKeys)
            {
                ANumberOfKeys = Convert.ToInt32(NextUsedKey - NextPartnerKey);
            }

            PartnerLedgerDT[0].LastPartnerId = Convert.ToInt32((NextPartnerKey + ANumberOfKeys - 1) - PartnerLedgerDT[0].PartnerKey);

            bool NewPartnerKeySubmissionOK = false;
            TVerificationResultCollection VerificationResult;

            try
            {
                NewPartnerKeySubmissionOK = PPartnerLedgerAccess.SubmitChanges(PartnerLedgerDT, WriteTransaction, out VerificationResult);
            }
            finally
            {
                if (NewPartnerKeySubmissionOK)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }

            return NextPartnerKey;
        }
    }
}