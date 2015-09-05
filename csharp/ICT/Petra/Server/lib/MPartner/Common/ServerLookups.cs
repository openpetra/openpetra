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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Exceptions;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors
{
    /// <summary>
    /// Performs server-side lookups for the Client in the MPartner.ServerLookups
    /// sub-namespace.
    ///
    /// </summary>
    public class TPartnerServerLookups
    {
        /// <summary>
        /// Gets the ShortName of a Partner.
        ///
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to find the short name for</param>
        /// <param name="APartnerShortName">ShortName for the found Partner ('' if Partner
        /// doesn't exist or PartnerKey is 0)</param>
        /// <param name="APartnerClass">Partner Class of the found Partner (FAMILY if Partner
        /// doesn't exist or PartnerKey is 0)</param>
        /// <param name="AMergedPartners">Set to false if the function should return 'false' if
        /// the Partner' Partner Status is MERGED</param>
        /// <returns>true if Partner was found in DB (except if AMergedPartners is false
        /// and Partner is MERGED) or PartnerKey is 0, otherwise false
        /// </returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean GetPartnerShortName(Int64 APartnerKey,
            out String APartnerShortName,
            out TPartnerClass APartnerClass,
            Boolean AMergedPartners)
        {
            Boolean ReturnValue;
            TStdPartnerStatusCode PartnerStatus;

            ReturnValue = MCommonMain.RetrievePartnerShortName(APartnerKey, out APartnerShortName, out APartnerClass, out PartnerStatus);

            if (((!AMergedPartners)) && (PartnerStatus == TStdPartnerStatusCode.spscMERGED))
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Gets the ShortName of a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to find the short name for</param>
        /// <param name="APartnerShortName">ShortName for the found Partner</param>
        /// <param name="APartnerClass">Partner Class of the found Partner</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean GetPartnerShortName(Int64 APartnerKey, out String APartnerShortName, out TPartnerClass APartnerClass)
        {
            return GetPartnerShortName(APartnerKey, out APartnerShortName, out APartnerClass, true);
        }

        /// <summary>
        /// Verifies the existence of a Partner.
        ///
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to find the short name for</param>
        /// <param name="AValidPartnerClasses">Pass in a array of valid PartnerClasses that the
        /// Partner is allowed to have (eg. [PERSON, FAMILY], or an empty array ( [] ).</param>
        /// <param name="APartnerExists">True if the Partner exists in the database or if PartnerKey is 0.</param>
        /// <param name="APartnerShortName">ShortName for the found Partner ('' if Partner
        /// doesn't exist or PartnerKey is 0)</param>
        /// <param name="APartnerClass">Partner Class of the found Partner (FAMILY if Partner
        /// doesn't exist or PartnerKey is 0)</param>
        /// <param name="AIsMergedPartner">true if the Partner' Partner Status is MERGED,
        /// otherwise false</param>
        /// <returns>true if Partner was found in DB (except if AValidPartnerClasses isn't
        /// an empty array and the found Partner isn't of a PartnerClass that is in the
        /// Set) or PartnerKey is 0, otherwise false
        /// </returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean VerifyPartner(Int64 APartnerKey,
            TPartnerClass[] AValidPartnerClasses,
            out bool APartnerExists,
            out String APartnerShortName,
            out TPartnerClass APartnerClass,
            out Boolean AIsMergedPartner)
        {
            Boolean ReturnValue;
            TStdPartnerStatusCode PartnerStatus;

            ReturnValue = APartnerExists = MCommonMain.RetrievePartnerShortName(APartnerKey,
                out APartnerShortName,
                out APartnerClass,
                out PartnerStatus);
//          TLogging.LogAtLevel(7, "TPartnerServerLookups.VerifyPartner: " + Convert.ToInt32(AValidPartnerClasses.Length));

            if (AValidPartnerClasses.Length != 0)
            {
                if (Array.BinarySearch(AValidPartnerClasses, APartnerClass) < 0)
                {
                    ReturnValue = false;
                }
            }

            if (PartnerStatus == TStdPartnerStatusCode.spscMERGED)
            {
                AIsMergedPartner = true;
            }
            else
            {
                AIsMergedPartner = false;
            }

            return ReturnValue;
        }

        /// <summary>Is this the key of a valid Gift Recipient?</summary>
        /// <param name="APartnerKey"></param>
        /// <returns>True if this is a valid key to a partner that's linked to a Cost Centre (in any ledger)
        /// or if this is a local site key.</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean PartnerIsLinkedToCC(Int64 APartnerKey)
        {
            TDBTransaction ReadTransaction = null;
            Boolean Ret = false;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    string Query = "SELECT COUNT(*) FROM a_ledger, a_valid_ledger_number " +
                                   "WHERE a_ledger.p_partner_key_n = " + APartnerKey +
                                   " OR a_valid_ledger_number.p_partner_key_n = " + APartnerKey;

                    if (Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(Query, ReadTransaction)) > 0)
                    {
                        Ret = true;
                    }
                });
            return Ret;
        }

        /// <summary>
        /// Verifies the existence of a Partner and checks if the current user can modify it.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to find the short name for</param>
        /// <param name="APartnerShortName">ShortName for the found Partner ('' if Partner
        ///  doesn't exist or PartnerKey is 0)</param>
        /// <param name="APartnerClass">Partner Class of the found Partner (FAMILY if Partner
        ///  doesn't exist or PartnerKey is 0)</param>
        /// <param name="AIsMergedPartner">true if the Partner' Partner Status is MERGED,
        ///  otherwise false</param>
        /// <param name="AUserCanAccessPartner">true if the current user has the rights to
        /// access this partner</param>
        /// <returns>true if Partner was found in DB or Partner key = 0, otherwise false</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean VerifyPartnerAndGetDetails(Int64 APartnerKey,
            out String APartnerShortName,
            out TPartnerClass APartnerClass,
            out Boolean AIsMergedPartner,
            out Boolean AUserCanAccessPartner)
        {
            APartnerShortName = "";
            APartnerClass = TPartnerClass.FAMILY; // Default. This is not really correct but the best compromise if PartnerKey is 0 or Partner isn't found since we have an enum here.
            AIsMergedPartner = false;
            AUserCanAccessPartner = false;

            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            StringCollection RequiredColumns;
            PPartnerTable PartnerTable;
            Boolean ReturnValue = true;
            TStdPartnerStatusCode PartnerStatus = TStdPartnerStatusCode.spscACTIVE;

            // initialise outout Arguments
            if (APartnerKey != 0)
            {
                // only some fields are needed
                RequiredColumns = new StringCollection();
                RequiredColumns.Add(PPartnerTable.GetPartnerShortNameDBName());
                RequiredColumns.Add(PPartnerTable.GetPartnerClassDBName());
                RequiredColumns.Add(PPartnerTable.GetStatusCodeDBName());
                RequiredColumns.Add(PPartnerTable.GetRestrictedDBName());
                RequiredColumns.Add(PPartnerTable.GetUserIdDBName());
                RequiredColumns.Add(PPartnerTable.GetGroupIdDBName());

                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                try
                {
                    PartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, RequiredColumns, ReadTransaction, null, 0, 0);
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel(7, "TPartnerServerLookups.VerifyPartner: committed own transaction.");
                    }
                }

                if (PartnerTable.Rows.Count == 0)
                {
                    ReturnValue = false;
                }
                else
                {
                    // since we loaded by primary key there must just be one partner row
                    APartnerShortName = PartnerTable[0].PartnerShortName;
                    APartnerClass = SharedTypes.PartnerClassStringToEnum(PartnerTable[0].PartnerClass);
                    PartnerStatus = SharedTypes.StdPartnerStatusCodeStringToEnum(PartnerTable[0].StatusCode);

                    // check if user can access partner
                    if (Ict.Petra.Server.MPartner.Common.TSecurity.CanAccessPartner(PartnerTable[0]) == TPartnerAccessLevelEnum.palGranted)
                    {
                        AUserCanAccessPartner = true;
                    }

                    // check if partner is merged
                    if (PartnerStatus == TStdPartnerStatusCode.spscMERGED)
                    {
                        AIsMergedPartner = true;
                    }
                    else
                    {
                        AIsMergedPartner = false;
                    }

                    ReturnValue = true;
                }
            }
            else
            {
                // Return result as valid if Partner Key is 0.
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Verifies the existence of a Partner
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to find the short name for</param>
        /// <returns>true if Partner was found in DB or Partner key = 0, otherwise false</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean VerifyPartner(Int64 APartnerKey)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            PPartnerTable PartnerTable;
            Boolean ReturnValue = true;

            // initialise outout Arguments
            if (APartnerKey != 0)
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                try
                {
                    PartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel(7, "TPartnerServerLookups.VerifyPartner: committed own transaction.");
                    }
                }

                if (PartnerTable.Rows.Count == 0)
                {
                    ReturnValue = false;
                }
                else
                {
                    ReturnValue = true;
                }
            }
            else
            {
                // Return result as valid if Partner Key is 0.
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Verifies the existence of a Partner at a given location
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of Partner to be verified</param>
        /// <param name="ALocationKey">Location Key of Partner to be verified</param>
        /// <param name="AAddressNeitherCurrentNorMailing"></param>
        /// <returns>true if Partner was found in DB at given location, otherwise false</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean VerifyPartnerAtLocation(Int64 APartnerKey,
            TLocationPK ALocationKey, out bool AAddressNeitherCurrentNorMailing)
        {
            AAddressNeitherCurrentNorMailing = true;

            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            PPartnerLocationTable PartnerLocationTable;
            Boolean ReturnValue = true;

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);
            try
            {
                PartnerLocationTable = PPartnerLocationAccess.LoadByPrimaryKey(APartnerKey,
                    ALocationKey.SiteKey,
                    ALocationKey.LocationKey,
                    ReadTransaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }

            if (PartnerLocationTable.Rows.Count == 0)
            {
                ReturnValue = false;
            }
            else
            {
                PPartnerLocationRow Row = (PPartnerLocationRow)PartnerLocationTable.Rows[0];

                // check if the partner location is either current or if it is a mailing address
                if ((Row.DateEffective > DateTime.Today)
                    || (!Row.SendMail)
                    || ((Row.DateGoodUntil != null)
                        && (Row.DateGoodUntil < DateTime.Today)))
                {
                    AAddressNeitherCurrentNorMailing = true;
                }
                else
                {
                    AAddressNeitherCurrentNorMailing = false;
                }

                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns information about a Partner that was Merged and about the
        /// Partner it was merged into.
        /// </summary>
        /// <param name="AMergedPartnerPartnerKey">PartnerKey of Merged Partner.</param>
        /// <param name="AMergedPartnerPartnerShortName">ShortName of Merged Partner.</param>
        /// <param name="AMergedPartnerPartnerClass">Partner Class of Merged Partner.</param>
        /// <param name="AMergedIntoPartnerKey">PartnerKey of Merged-Into Partner. (Only
        /// populated if that information is available.)</param>
        /// <param name="AMergedIntoPartnerShortName">ShortName of Merged-Into Partner. (Only
        /// populated if that information is available.)</param>
        /// <param name="AMergedIntoPartnerClass">PartnerClass of Merged-Into Partner. (Only
        /// populated if that information is available.)</param>
        /// <param name="AMergedBy">User who performed the Partner Merge operation. (Only
        /// populated if that information is available.)</param>
        /// <param name="AMergeDate">Date on which the Partner Merge operation was done. (Only
        /// populated if that information is available.)</param>
        /// <returns>True if (1) Merged Partner exists and (2) its Status is MERGED,
        /// otherwise false.</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean MergedPartnerDetails(Int64 AMergedPartnerPartnerKey,
            out String AMergedPartnerPartnerShortName,
            out TPartnerClass AMergedPartnerPartnerClass,
            out Int64 AMergedIntoPartnerKey,
            out String AMergedIntoPartnerShortName,
            out TPartnerClass AMergedIntoPartnerClass,
            out String AMergedBy,
            out DateTime AMergeDate)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            StringCollection RequiredColumns;
            PPartnerTable MergedPartnerTable;
            PPartnerTable PartnerMergedIntoTable;
            PPartnerMergeTable PartnerMergeTable;
            Boolean ReturnValue = false;

            // Initialise out Arguments
            AMergedPartnerPartnerShortName = "";
            AMergedPartnerPartnerClass = TPartnerClass.FAMILY;  // Default. This is not really correct but the best compromise if PartnerKey is 0 or Partner isn't found since we have an enum here.
            AMergedIntoPartnerKey = -1;
            AMergedIntoPartnerShortName = "";
            AMergedIntoPartnerClass = TPartnerClass.FAMILY;     // Default. This is not really correct but the best compromise if PartnerKey is 0 or Partner isn't found since we have an enum here.
            AMergedBy = "";
            AMergeDate = DateTime.MinValue;

            if (AMergedPartnerPartnerKey != 0)
            {
                /*
                 * First we look up the Partner that was Merged to get some details of it.
                 */

                // only some fields are needed
                RequiredColumns = new StringCollection();
                RequiredColumns.Add(PPartnerTable.GetPartnerShortNameDBName());
                RequiredColumns.Add(PPartnerTable.GetStatusCodeDBName());
                RequiredColumns.Add(PPartnerTable.GetPartnerClassDBName());

                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                try
                {
                    MergedPartnerTable = PPartnerAccess.LoadByPrimaryKey(
                        AMergedPartnerPartnerKey, RequiredColumns, ReadTransaction, null, 0, 0);

                    if (MergedPartnerTable.Rows.Count == 0)
                    {
                        ReturnValue = false;
                    }
                    else
                    {
                        if (SharedTypes.StdPartnerStatusCodeStringToEnum(
                                MergedPartnerTable[0].StatusCode) != TStdPartnerStatusCode.spscMERGED)
                        {
                            ReturnValue = false;
                        }
                        else
                        {
                            AMergedPartnerPartnerShortName = MergedPartnerTable[0].PartnerShortName;
                            AMergedPartnerPartnerClass = SharedTypes.PartnerClassStringToEnum(MergedPartnerTable[0].PartnerClass);

                            /*
                             * Now we look up the Partner that was Merged in the PartnerMerge DB Table
                             * to get the information about the Merged-Into Partner.
                             */
                            PartnerMergeTable = PPartnerMergeAccess.LoadByPrimaryKey(AMergedPartnerPartnerKey, ReadTransaction);

                            if (PartnerMergeTable.Rows.Count == 0)
                            {
                                /*
                                 * Although we didn't find the Merged Partner in the PartnerMerge
                                 * DB Table it is still a valid, Merged Partner, so we return true
                                 * in this case.
                                 */
                                ReturnValue = true;
                            }
                            else
                            {
                                /*
                                 * Now we look up the Merged-Into Partner to get some details of it.
                                 */
                                PartnerMergedIntoTable = PPartnerAccess.LoadByPrimaryKey(
                                    PartnerMergeTable[0].MergeTo, RequiredColumns,
                                    ReadTransaction, null, 0, 0);

                                if (PartnerMergedIntoTable.Rows.Count == 0)
                                {
                                    ReturnValue = false;
                                }
                                else
                                {
                                    AMergedIntoPartnerKey = PartnerMergeTable[0].MergeTo;
                                    AMergedBy = PartnerMergeTable[0].MergedBy;
                                    AMergeDate = PartnerMergeTable[0].MergeDate.Value;
                                    AMergedIntoPartnerShortName = PartnerMergedIntoTable[0].PartnerShortName;
                                    AMergedIntoPartnerClass = SharedTypes.PartnerClassStringToEnum(PartnerMergedIntoTable[0].PartnerClass);

                                    ReturnValue = true;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel(7, "TPartnerServerLookups.MergedPartnerDetails: committed own transaction.");
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Retrieves information about a Partner.
        /// </summary>
        /// <remarks>
        /// The information returned can be of different scope. Scope refers to the amount/detail
        /// of information that is returned. The scope is defined with the
        /// <paramref name="APartnerInfoScope" /> parameter. Larger scope means longer time is
        /// needed for the retrieval of the data and a larger DataSet to transfer to the Client
        /// (if this Method is called from the Client)!
        /// </remarks>
        /// <param name="APartnerKey">PartnerKey of Partner to find the short name for</param>
        /// <param name="APartnerInfoScope">Defines the scope of data that should be returned
        /// for the Partner.</param>
        /// <param name="APartnerInfoDS">Typed DataSet of Type <see cref="PartnerInfoTDS" /> that
        /// contains the Partner Information that was requested for the Partner.</param>
        /// <returns>True if Partner was found in DB, otherwise false.
        /// </returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean PartnerInfo(Int64 APartnerKey,
            TPartnerInfoScopeEnum APartnerInfoScope,
            out PartnerInfoTDS APartnerInfoDS)
        {
            return PartnerInfo(APartnerKey, null, APartnerInfoScope, out APartnerInfoDS);
        }

        /// <summary>
        /// Retrieves information about a Partner.
        /// </summary>
        /// <remarks>
        /// The information returned can be of different scope. Scope refers to the amount/detail
        /// of information that is returned. The scope is defined with the
        /// <paramref name="APartnerInfoScope" /> parameter. Larger scope means longer time is
        /// needed for the retrieval of the data and a larger DataSet to transfer to the Client
        /// (if this Method is called from the Client)!
        /// </remarks>
        /// <param name="APartnerKey">PartnerKey of Partner to find the short name for</param>
        /// <param name="ALocationKey" >Location Key of the Location that the information should be
        /// retrieved for.</param>
        /// <param name="APartnerInfoScope">Defines the scope of data that should be returned
        /// for the Partner.</param>
        /// <param name="APartnerInfoDS">Typed DataSet of Type <see cref="PartnerInfoTDS" /> that
        /// contains the Partner Information that was requested for the Partner.</param>
        /// <returns>True if Partner was found in DB, otherwise false.
        /// </returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean PartnerInfo(Int64 APartnerKey, TLocationPK ALocationKey,
            TPartnerInfoScopeEnum APartnerInfoScope,
            out PartnerInfoTDS APartnerInfoDS)
        {
            const string DATASET_NAME = "PartnerInfo";

            Boolean ReturnValue = false;

            APartnerInfoDS = new PartnerInfoTDS(DATASET_NAME);

            switch (APartnerInfoScope)
            {
                case TPartnerInfoScopeEnum.pisHeadOnly:

                    throw new NotImplementedException();

                case TPartnerInfoScopeEnum.pisPartnerLocationAndRestOnly:

                    if (TServerLookups_PartnerInfo.PartnerLocationAndRestOnly(APartnerKey,
                            ALocationKey, ref APartnerInfoDS))
                    {
                        ReturnValue = true;
                    }
                    else
                    {
                        ReturnValue = false;
                    }

                    break;

                case TPartnerInfoScopeEnum.pisPartnerLocationOnly:

                    if (TServerLookups_PartnerInfo.PartnerLocationOnly(APartnerKey,
                            ALocationKey, ref APartnerInfoDS))
                    {
                        ReturnValue = true;
                    }
                    else
                    {
                        ReturnValue = false;
                    }

                    break;

                case TPartnerInfoScopeEnum.pisLocationPartnerLocationAndRestOnly:

                    if (TServerLookups_PartnerInfo.LocationPartnerLocationAndRestOnly(APartnerKey,
                            ALocationKey, ref APartnerInfoDS))
                    {
                        ReturnValue = true;
                    }
                    else
                    {
                        ReturnValue = false;
                    }

                    break;

                case TPartnerInfoScopeEnum.pisLocationPartnerLocationOnly:

                    if (TServerLookups_PartnerInfo.LocationPartnerLocationOnly(APartnerKey,
                            ALocationKey, ref APartnerInfoDS))
                    {
                        ReturnValue = true;
                    }
                    else
                    {
                        ReturnValue = false;
                    }

                    break;

                case TPartnerInfoScopeEnum.pisPartnerAttributesOnly:

                    if (TServerLookups_PartnerInfo.PartnerAttributesOnly(APartnerKey,
                            ref APartnerInfoDS))
                    {
                        ReturnValue = true;
                    }
                    else
                    {
                        ReturnValue = false;
                    }

                    break;

                case TPartnerInfoScopeEnum.pisFull:

                    if (TServerLookups_PartnerInfo.AllPartnerInfoData(APartnerKey,
                            ref APartnerInfoDS))
                    {
                        ReturnValue = true;
                    }
                    else
                    {
                        ReturnValue = false;
                    }

                    break;

                default:

                    break;
            }

            return ReturnValue;
        }

        /// <summary>Retrieves receipting fields from a partner key.</summary>
        /// <param name="APartnerKey"></param>
        /// <param name="AReceiptEachGift"></param>
        /// <param name="AReceiptLetterFrequency"></param>
        /// <param name="AEmailGiftStatement"></param>
        /// <param name="AAnonymousDonor"></param>
        [RequireModulePermission("FINANCE-2")]
        public static bool GetPartnerReceiptingInfo(
            Int64 APartnerKey,
            out bool AReceiptEachGift,
            out String AReceiptLetterFrequency,
            out bool AEmailGiftStatement,
            out bool AAnonymousDonor)
        {
            TDBTransaction ReadTransaction = null;
            PPartnerTable PartnerTbl = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum, ref ReadTransaction,
                delegate
                {
                    PartnerTbl = PPartnerAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);
                });

            if (PartnerTbl.Rows.Count > 0)
            {
                PPartnerRow Row = PartnerTbl[0];
                AReceiptEachGift = Row.ReceiptEachGift;
                AReceiptLetterFrequency = Row.ReceiptLetterFrequency;
                AEmailGiftStatement = Row.EmailGiftStatement;
                AAnonymousDonor = Row.AnonymousDonor;
                return true;
            }
            else
            {
                AReceiptEachGift = false;
                AReceiptLetterFrequency = "";
                AEmailGiftStatement = false;
                AAnonymousDonor = false;
                return false;
            }
        }

        /// <summary>
        /// Retrieves the description of an extract.
        /// </summary>
        /// <param name="AExtractName">The name which identifies the extract</param>
        /// <param name="AExtractDescription">The description of the extract</param>
        /// <returns>true if the extract was found and the description was retrieved</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean GetExtractDescription(String AExtractName, out String AExtractDescription)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            Boolean ReturnValue = false;

            AExtractDescription = "Can not retrieve description";

            TLogging.LogAtLevel(9, "TPartnerServerLookups.GetExtractDescription called!");

            MExtractMasterTable ExtractMasterDT = new MExtractMasterTable();

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            // Load data

            MExtractMasterTable TemplateExtractDT = new MExtractMasterTable();
            MExtractMasterRow TemplateRow = TemplateExtractDT.NewRowTyped(false);
            TemplateRow.ExtractName = AExtractName;

            try
            {
                ExtractMasterDT = MExtractMasterAccess.LoadUsingTemplate(TemplateRow, ReadTransaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerServerLookups.GetExtractDescription: committed own transaction.");
                }
            }

            if (ExtractMasterDT.Rows.Count < 1)
            {
                ReturnValue = false;
                TLogging.LogAtLevel(7, "TPartnerServerLookups.TPartnerServerLookups.GetExtractDescription: m_extract_master DB Table is empty");
            }
            else
            {
                MExtractMasterRow ExtractRow = ExtractMasterDT.Rows[0] as MExtractMasterRow;

                if (ExtractRow != null)
                {
                    AExtractDescription = ExtractRow.ExtractDesc;
                    ReturnValue = true;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Gets the foundation status of the partner.
        /// This function should only be called for partners of type organisation.
        /// </summary>
        /// <param name="APartnerKey">Partner key of the partner to retrieve the foundation status.
        /// Partner must be an organisation.</param>
        /// <param name="AIsFoundation">true if the partner (organisation) is a foundation. Otherwise false</param>
        /// <returns>True, if an entry of the partner was found in table p_organisation.
        /// False, if there is no partner with the partner key or the partner is not an organisation</returns>
        /// <exception>EOPAppException if we don't find a partner or if the partner is not an organisation</exception>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean GetPartnerFoundationStatus(Int64 APartnerKey, out Boolean AIsFoundation)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            Boolean ReturnValue = false;

            AIsFoundation = false;

            TLogging.LogAtLevel(9, "TPartnerServerLookups.GetPartnerFoundationStatus called!");

            POrganisationTable OrganisationDT = new POrganisationTable();

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            // Load data
            try
            {
                OrganisationDT = POrganisationAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerServerLookups.GetPartnerFoundationStatus: committed own transaction.");
                }
            }

            if (OrganisationDT.Rows.Count < 1)
            {
                // Do we have a partner?
                TStdPartnerStatusCode PartnerStatus;
                String PartnerShortName;
                TPartnerClass PartnerClass;

                if (MCommonMain.RetrievePartnerShortName(APartnerKey, out PartnerShortName, out PartnerClass, out PartnerStatus))
                {
                    // we have a partner but it's not an organisation
                    throw new EOPAppException(
                        "TPartnerServerLookups.GetPartnerFoundationStatus: p_organisation DB Table is empty. The partner key does not refer to an organisation!");
                }
                else
                {
                    // we don't have a valid partner key
                    throw new EOPAppException(
                        "TPartnerServerLookups.GetPartnerFoundationStatus: p_organisation DB Table is empty. The partner key is not valid!");
                }
            }
            else
            {
                POrganisationRow ExtractRow = OrganisationDT.Rows[0] as POrganisationRow;

                if (ExtractRow != null)
                {
                    AIsFoundation = ExtractRow.Foundation;
                    ReturnValue = true;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Get a list of the last used partners from the current user.
        /// </summary>
        /// <param name="AMaxPartnersCount">Maxinum numbers of Partners to return</param>
        /// <param name="APartnerClasses">List of partner classes which kind of partners the result should contain.
        /// If it contains "*" then all recent partners will be returned otherwise only the partners whose
        /// partner class is in APartnerClasses.</param>
        /// <param name="ARecentlyUsedPartners">List of the last used partner names and partner keys</param>
        /// <returns>true if call was successfull</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean GetRecentlyUsedPartners(int AMaxPartnersCount,
            ArrayList APartnerClasses,
            out Dictionary <long, string>ARecentlyUsedPartners)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;

            ARecentlyUsedPartners = new Dictionary <long, string>();

            TLogging.LogAtLevel(9, "TPartnerServerLookups.GetRecentlyUsedPartner called!");

            PRecentPartnersTable RecentPartnersDT = new PRecentPartnersTable();

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            // Load the recently used partners from this user
            try
            {
                RecentPartnersDT = PRecentPartnersAccess.LoadViaSUser(UserInfo.GUserInfo.UserID, ReadTransaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TPartnerServerLookups.GetRecentUsedPartners: committed own transaction.");
                }
            }

            PPartnerTable PartnerDT = new PPartnerTable();

            // Sort the users by date and time they have been last used

            System.Data.DataRow[] RecentPartnerRows = RecentPartnersDT.Select("",
                TTypedDataTable.GetLabel(PRecentPartnersTable.TableId, PRecentPartnersTable.ColumnwhenDateId) + " DESC, " +
                TTypedDataTable.GetLabel(PRecentPartnersTable.TableId, PRecentPartnersTable.ColumnwhenTimeId) + " DESC");

            for (int Counter = 0; Counter < RecentPartnersDT.Rows.Count; ++Counter)
            {
                PRecentPartnersRow RecentPartnerRow = (PRecentPartnersRow)RecentPartnerRows[Counter];

                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                // Get the partner name from the recently used partner
                try
                {
                    PartnerDT = PPartnerAccess.LoadByPrimaryKey(RecentPartnerRow.PartnerKey, ReadTransaction);
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel(7, "TPartnerServerLookups.GetRecentUsedPartners: committed own transaction.");
                    }
                }

                if (PartnerDT.Rows.Count > 0)
                {
                    /* Check the partner class.
                     * If we want this partner then add it to the ARecentlyUsedPartners list
                     * otherwise skip it.
                     */

                    PPartnerRow PartnerRow = (PPartnerRow)PartnerDT.Rows[0];

                    foreach (Object CurrentPartnerClass in APartnerClasses)
                    {
                        string TmpString = CurrentPartnerClass.ToString();

                        if ((TmpString == "*")
                            || (TmpString == PartnerRow.PartnerClass))
                        {
                            // String contains Name and type like this: J. Miller (type FAMILY)
                            ARecentlyUsedPartners.Add(PartnerRow.PartnerKey,
                                PartnerRow.PartnerShortName + " (type " + PartnerRow.PartnerClass + ")");
                        }
                    }
                }

                if (ARecentlyUsedPartners.Count >= AMaxPartnersCount)
                {
                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the family partner key of a person record.
        /// This function should only be called for partners of type person.
        /// </summary>
        /// <param name="APersonKey">Partner key of the person to retrieve the family key for
        /// Partner must be a person.</param>
        /// <returns>Family partner key of the person. A Person must always have a family that it is related to.
        /// False, if there is no partner with the partner key or the partner is not an organisation</returns>
        /// <exception>EOPAppException if we don't find a partner or if the partner is not an organisation</exception>
        [RequireModulePermission("PTNRUSER")]
        public static Int64 GetFamilyKeyForPerson(Int64 APersonKey)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            Int64 ReturnValue = 0;

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            PPersonTable PersonDT = new PPersonTable();

            try
            {
                PersonDT = PPersonAccess.LoadByPrimaryKey(APersonKey, ReadTransaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
            }

            if (PersonDT.Rows.Count == 1)
            {
                ReturnValue = ((PPersonRow)PersonDT.Rows[0]).FamilyKey;
            }
            else
            {
                // we don't have a valid partner key
                throw new EOPAppException(
                    "TPartnerServerLookups.GetFamilyKeyForPerson: The partner key is not valid!");
            }

            return ReturnValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>the country code for this installation of OpenPetra.</returns>
        [RequireModulePermission("PTNRUSER")]
        public static string GetCountryCodeFromSiteLedger()
        {
            bool NewTransaction;
            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            string CountryCode = TAddressTools.GetCountryCodeFromSiteLedger(ReadTransaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }

            return CountryCode;
        }
    }
}