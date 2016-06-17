//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Collections;
using System.Collections.Generic;

using Ict.Common;
using Ict.Common.Data;

using Ict.Petra.Client.App.Core.RemoteObjects;

using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;


namespace Ict.Petra.Client.App.Core
{
    /// <summary>
    /// Provides Client-side static functions that perform server-side lookups for
    /// the Client.
    /// Classes or GUI controls that need to do a server-side lookup just call the
    /// desired procedures here to get the result from the PetraServer (and therefore
    /// don't need to know about a certain business object that is used on a screen
    /// and its methods).
    /// </summary>
    public class TServerLookup
    {
        /// <summary>
        /// todoComment
        /// </summary>
        public class TMCommon
        {
            #region TServerLookup.TMCommon

            /// <summary>
            /// simple data reader;
            /// checks for permissions of the current user;
            /// </summary>
            /// <param name="ATablename"></param>
            /// <param name="ASearchCriteria">a set of search criteria</param>
            /// <param name="AResultTable">returns typed datatable</param>
            /// <returns></returns>
            public static bool GetData(string ATablename, TSearchCriteria[] ASearchCriteria, out TTypedDataTable AResultTable)
            {
                return TRemote.MCommon.DataReader.WebConnectors.GetData(ATablename, ASearchCriteria, out AResultTable);
            }

            #endregion
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public class TMPartner
        {
            #region TServerLookup.TMPartner

            /**
             *   Gets the ShortName of a Partner.
             *
             *   @param APartnerKey PartnerKey of Partner to find the short name for
             *   @param APartnerShortName ShortName for the found Partner ('' if Partner
             *       doesn't exist or PartnerKey is 0)
             *   @param APartnerClass Partner Class of the found Partner (FAMILY if Partner
             *       doesn't exist or PartnerKey is 0)
             *   @param APartnerStatus Partner Status for the found Partner (spscINACTIVE if Partner
             *       doesn't exist or PartnerKey is 0)
             *   @param AMergedPartners Set to false if the function should return 'false' if
             *     the Partner' Partner Status is MERGED
             *   @return true if Partner was found in DB (except if AMergedPartners is false
             *     and Partner is MERGED) or PartnerKey is 0, otherwise false
             * // future public static methods for MFinance go here...
             */
            public static Boolean GetPartnerShortName(Int64 APartnerKey,
                out String APartnerShortName,
                out TPartnerClass APartnerClass,
                Boolean AMergedPartners)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(APartnerKey,
                    out APartnerShortName,
                    out APartnerClass,
                    AMergedPartners);
            }

            /// <summary>
            /// Returns miscellaneous Partner data.
            /// </summary>
            /// <remarks>Used by the Partner Info UserControl.</remarks>
            /// <param name="APartnerKey">PartnerKey of the Partner for which the data
            /// should be retrieved.</param>
            /// <param name="APartnerInfoScope">Scope of data that should be loaded and
            /// returned by the PetraServer.</param>
            /// <param name="APartnerInfoDS">Typed DataSet that contains the requested Partner data.</param>
            /// <param name="ASeparateDBConnection">If you *must have* a separate DB Connection</param>
            /// <returns>True if the Partner exists, otherwise false.</returns>
            public static Boolean PartnerInfo(Int64 APartnerKey,
                TPartnerInfoScopeEnum APartnerInfoScope,
                out PartnerInfoTDS APartnerInfoDS,
                Boolean ASeparateDBConnection = false)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.PartnerInfo(APartnerKey,
                    APartnerInfoScope, out APartnerInfoDS, ASeparateDBConnection);
            }

            /// <summary>
            /// Returns miscellaneous Partner data.
            /// </summary>
            /// <remarks>Used by the Partner Info UserControl.</remarks>
            /// <param name="APartnerKey">PartnerKey of the Partner for which the data
            /// should be retrieved.</param>
            /// <param name="ALocationKey">LocationKey of the Location for which data
            /// for the Partner specified should be retrieved.</param>
            /// <param name="APartnerInfoScope">Scope of data that should be loaded and
            /// returned by the PetraServer.</param>
            /// <param name="APartnerInfoDS">Typed DataSet that contains the requested Partner data.</param>
            /// <param name="ASeparateDBConnection">If you *must have* a separate DB Connection</param>
            /// <returns>True if the Partner exists, otherwise false.</returns>
            public static Boolean PartnerInfo(Int64 APartnerKey, TLocationPK ALocationKey,
                TPartnerInfoScopeEnum APartnerInfoScope,
                out PartnerInfoTDS APartnerInfoDS,
                Boolean ASeparateDBConnection = false)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.PartnerInfo(APartnerKey,
                    ALocationKey, APartnerInfoScope, out APartnerInfoDS, ASeparateDBConnection);
            }

            /// <summary>
            /// overload
            /// </summary>
            /// <param name="APartnerKey"></param>
            /// <param name="APartnerShortName"></param>
            /// <param name="APartnerClass"></param>
            /// <returns></returns>
            public static Boolean GetPartnerShortName(Int64 APartnerKey, out String APartnerShortName, out TPartnerClass APartnerClass)
            {
                return GetPartnerShortName(APartnerKey, out APartnerShortName, out APartnerClass, true);
            }

            /// <summary>
            /// Verifies the existence of a Partner.
            /// </summary>
            /// <param name="APartnerKey">PartnerKey of Partner to find the short name for</param>
            /// <param name="AValidPartnerClasses">Pass in a Set of valid PartnerClasses that the
            ///  Partner is allowed to have (eg. [PERSON, FAMILY], or an empty Set ( [] ).</param>
            /// <param name="APartnerExists">True if the Partner exists in the database or if PartnerKey is 0.</param>
            /// <param name="APartnerShortName">ShortName for the found Partner ('' if Partner
            ///  doesn't exist or PartnerKey is 0)</param>
            /// <param name="APartnerClass">Partner Class of the found Partner (FAMILY if Partner
            ///  doesn't exist or PartnerKey is 0)</param>
            /// <param name="APartnerStatus">Partner Status</param>
            /// <returns>true if Partner was found in DB (except if AValidPartnerClasses isn't
            ///  an empty Set and the found Partner isn't of a PartnerClass that is in the
            ///  Set) or PartnerKey is 0, otherwise false</returns>
            public static Boolean VerifyPartner(Int64 APartnerKey,
                TPartnerClass[] AValidPartnerClasses,
                out bool APartnerExists,
                out String APartnerShortName,
                out TPartnerClass APartnerClass,
                out TStdPartnerStatusCode APartnerStatus)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.VerifyPartner(APartnerKey,
                    AValidPartnerClasses,
                    out APartnerExists,
                    out APartnerShortName,
                    out APartnerClass,
                    out APartnerStatus);
            }

            /// <summary></summary>
            /// <param name="APartnerKey"></param>
            /// <returns></returns>
            public static Boolean PartnerHasActiveStatus(Int64 APartnerKey)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.PartnerHasActiveStatus(APartnerKey);
            }

            /// <summary>Is this the key of a valid Gift Recipient?</summary>
            /// <param name="APartnerKey"></param>
            /// <returns>True if this is a valid key to a partner that's linked to a Cost Centre (in any ledger)</returns>
            public static Boolean PartnerIsLinkedToCC(Int64 APartnerKey)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.PartnerIsLinkedToCC(APartnerKey);
            }

            /// <summary>Is Partner of type CC linked?</summary>
            /// <param name="ALedgerNumber"></param>
            /// <param name="APartnerKey"></param>
            /// <returns>True if this is a valid key of a partner of type CC that's linked</returns>
            public static Boolean PartnerOfTypeCCIsLinked(Int32 ALedgerNumber, Int64 APartnerKey)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.PartnerOfTypeCCIsLinked(ALedgerNumber, APartnerKey);
            }

            /// <summary>Is Partner of type CC linked?</summary>
            /// <param name="APartnerKey"></param>
            /// <param name="AGiftDate"></param>
            /// <returns>True if this is a valid key of a partner of type CC that's linked</returns>
            public static Boolean PartnerHasCurrentGiftDestination(Int64 APartnerKey, DateTime ? AGiftDate)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.PartnerHasCurrentGiftDestination(APartnerKey, AGiftDate);
            }

            /// <summary>
            /// Verifies the existence of a Partner.
            /// </summary>
            /// <param name="APartnerKey">PartnerKey of Partner to find the short name for</param>
            /// <param name="APartnerShortName">ShortName for the found Partner ('' if Partner
            ///  doesn't exist or PartnerKey is 0)</param>
            /// <param name="APartnerClass">Partner Class of the found Partner (FAMILY if Partner
            ///  doesn't exist or PartnerKey is 0)</param>
            /// <param name="AIsMergedPartner">true if the Partner' Partner Status is MERGED,
            ///  otherwise false</param>
            /// <param name="AUserCanAccessPartner">true if the current user has the rights to
            /// edit this partner</param>
            /// <returns>true if Partner was found in DB (except if AValidPartnerClasses isn't
            ///  an empty Set and the found Partner isn't of a PartnerClass that is in the
            ///  Set) or PartnerKey is 0, otherwise false</returns>
            public static Boolean VerifyPartner(Int64 APartnerKey,
                out String APartnerShortName,
                out TPartnerClass APartnerClass,
                out Boolean AIsMergedPartner,
                out Boolean AUserCanAccessPartner)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.VerifyPartnerAndGetDetails(APartnerKey,
                    out APartnerShortName,
                    out APartnerClass,
                    out AIsMergedPartner,
                    out AUserCanAccessPartner);
            }

            /// <summary>
            /// Verifies the existence of a Partner.
            /// </summary>
            /// <param name="APartnerKey">PartnerKey of Partner to find the short name for</param>
            /// <returns>true if Partner was found in DB (except if AValidPartnerClasses isn't
            ///  an empty Set and the found Partner isn't of a PartnerClass that is in the
            ///  Set) or PartnerKey is 0, otherwise false</returns>
            public static Boolean VerifyPartner(Int64 APartnerKey)
            {
                string PartnerShortName = null;
                TPartnerClass PartnerClass;
                bool IsMergedPartner;
                bool UserCanAccessPartner;

                return VerifyPartner(APartnerKey,
                    out PartnerShortName,
                    out PartnerClass,
                    out IsMergedPartner,
                    out UserCanAccessPartner);
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
            public static Boolean MergedPartnerDetails(Int64 AMergedPartnerPartnerKey,
                out String AMergedPartnerPartnerShortName,
                out TPartnerClass AMergedPartnerPartnerClass,
                out Int64 AMergedIntoPartnerKey,
                out String AMergedIntoPartnerShortName,
                out TPartnerClass AMergedIntoPartnerClass,
                out String AMergedBy,
                out DateTime AMergeDate)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.MergedPartnerDetails(
                    AMergedPartnerPartnerKey,
                    out AMergedPartnerPartnerShortName,
                    out AMergedPartnerPartnerClass,
                    out AMergedIntoPartnerKey,
                    out AMergedIntoPartnerShortName,
                    out AMergedIntoPartnerClass,
                    out AMergedBy,
                    out AMergeDate);
            }

            /// <summary>
            /// Retrieves the description of an extract.
            /// </summary>
            /// <param name="AExtractName">The name which identifies the extract</param>
            /// <param name="AExtractDescription">The description of the extract</param>
            /// <returns>true if the extract was found and the description was retrieved</returns>
            public static Boolean GetExtractDescription(String AExtractName, out String AExtractDescription)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetExtractDescription(AExtractName, out AExtractDescription);
            }

            /// <summary>
            /// Gets the foundation status of the partner
            /// </summary>
            /// <param name="APartnerKey">Partner key of the partner to retrieve the foundation status</param>
            /// <param name="AIsFoundation">true if the partner (organisation) is a foundation. Otherwise false</param>
            /// <returns>true if an entry of the partner was found in table p_organisation</returns>
            public static Boolean GetPartnerFoundationStatus(Int64 APartnerKey, out Boolean AIsFoundation)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerFoundationStatus(APartnerKey, out AIsFoundation);
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
            public static Boolean GetRecentlyUsedPartners(int AMaxPartnersCount,
                ArrayList APartnerClasses,
                out Dictionary <long, string>ARecentlyUsedPartners)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetRecentlyUsedPartners(AMaxPartnersCount,
                    APartnerClasses,
                    out ARecentlyUsedPartners);
            }

            /// <summary>
            /// Gets the family partner key of a person record.
            /// This function should only be called for partners of type person.
            /// </summary>
            /// <param name="APersonKey">Partner key of the person to retrieve the family key for
            /// Partner must be a person.</param>
            /// <returns>Family partner key of the person. A Person must always have a family that it is related to.
            /// False, if there is no partner with the partner key or the partner is not an organisation</returns>
            public static Int64 GetFamilyKeyForPerson(Int64 APersonKey)
            {
                return TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetFamilyKeyForPerson(APersonKey);
            }

            #endregion
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public class TMFinance
        {
            /// <summary>
            /// Get the current posting date range for the specified ledger
            /// </summary>
            /// <param name="ALedgerNumber"></param>
            /// <param name="AStartDateCurrentPeriod"></param>
            /// <param name="AEndDateLastForwardingPeriod"></param>
            /// <returns></returns>
            public static Boolean GetCurrentPostingRangeDates(Int32 ALedgerNumber,
                out DateTime AStartDateCurrentPeriod,
                out DateTime AEndDateLastForwardingPeriod)
            {
                return TRemote.MFinance.GL.WebConnectors.GetCurrentPostingRangeDates(ALedgerNumber,
                    out AStartDateCurrentPeriod,
                    out AEndDateLastForwardingPeriod);
            }

            /// <summary>
            /// Get the start and end dates for the specified period
            /// </summary>
            /// <param name="ALedgerNumber"></param>
            /// <param name="AYearNumber"></param>
            /// <param name="ADiffPeriod"></param>
            /// <param name="APeriodNumber"></param>
            /// <param name="AStartDatePeriod"></param>
            /// <param name="AEndDatePeriod"></param>
            /// <returns></returns>
            public static Boolean GetCurrentPeriodDates(Int32 ALedgerNumber,
                Int32 AYearNumber,
                Int32 ADiffPeriod,
                Int32 APeriodNumber,
                out DateTime AStartDatePeriod,
                out DateTime AEndDatePeriod)
            {
                return TRemote.MFinance.GL.WebConnectors.GetPeriodDates(ALedgerNumber,
                    AYearNumber,
                    ADiffPeriod,
                    APeriodNumber,
                    out AStartDatePeriod,
                    out AEndDatePeriod);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public class TMSysMan
        {
            /// <summary>
            /// Get all the installed Patches
            /// </summary>
            /// <param name="APatchLogDT">Table of installed patches</param>
            /// <returns>true</returns>
            public static Boolean GetInstalledPatches(out Ict.Petra.Shared.MSysMan.Data.SPatchLogTable APatchLogDT)
            {
                return TRemote.MSysMan.Application.WebConnectors.GetInstalledPatches(out APatchLogDT);
            }

            /// <summary>
            /// Get the current database version from the server
            /// </summary>
            /// <param name="APetraDBVersion">Database version</param>
            /// <returns>true</returns>
            public static Boolean GetDBVersion(out System.String APetraDBVersion)
            {
                return TRemote.MSysMan.Application.WebConnectors.GetDBVersion(out APetraDBVersion);
            }
        }
    }
}