//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Data;

using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;

namespace Ict.Petra.Server.MPartner.Common
{
    /// <summary>
    /// Contains security-related functions for Partners that that can be used by any Class.
    ///
    /// Note: There are other security-related Methods to be found in Shared.
    ///       Class: Ict.Petra.Shared.MParter, Security.cs, Class TSecurity.
    /// </summary>
    public static class TSecurity
    {
        /// <summary>
        /// Tests whether the current user has access to a particular Partner.
        /// </summary>
        /// <remarks>
        /// <para>Corresponds to Progress 4GL Method 'CanAccessPartner' in
        /// common/sp_partn.p</para>
        /// <para>A shared implementation of this Method exists that has two additional
        /// Arguments. It needs the Foundation Row to be passed in, but has the
        /// advantage of not needing a Server roundtrip for a DB lookup!</para>
        /// </remarks>
        /// <param name="APartnerRow">Partner for which access should be checked for.</param>
        /// <returns><see cref="T:TPartnerAccessLevelEnum.palGranted" /> if access
        /// to the Partner is granted, otherwise a different
        /// <see cref="T:TPartnerAccessLevelEnum" /> value.</returns>
        public static TPartnerAccessLevelEnum CanAccessPartner(
            PPartnerRow APartnerRow)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            PFoundationTable FoundationTable;

            if (APartnerRow.PartnerKey != 0)
            {
                // If PartnerClass is ORGANISATION, we need to check if it is a Foundation
                if (APartnerRow.PartnerClass == SharedTypes.PartnerClassEnumToString(
                        TPartnerClass.ORGANISATION))
                {
                    ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                        IsolationLevel.ReadCommitted,
                        TEnforceIsolationLevel.eilMinimum,
                        out NewTransaction);

                    // Load Foundation
                    try
                    {
                        FoundationTable = PFoundationAccess.LoadByPrimaryKey(APartnerRow.PartnerKey,
                            ReadTransaction);

                        if (FoundationTable.Rows.Count > 0)
                        {
                            // The ORGANISATION is a Foundation, we need to check Foundation Security
                            return Ict.Petra.Shared.MPartner.TSecurity.CanAccessPartner(APartnerRow,
                                true, FoundationTable[0]);
                        }
                        else
                        {
                            // The ORGANISATION isn't a Foundation, we don't need to check Foundation Security
                            return Ict.Petra.Shared.MPartner.TSecurity.CanAccessPartner(APartnerRow,
                                false, null);
                        }
                    }
                    finally
                    {
                        if (NewTransaction)
                        {
                            DBAccess.GDBAccessObj.CommitTransaction();
                            TLogging.LogAtLevel(8, "TSecurity.CanAccessPartnerByKey: committed own transaction.");
                        }
                    }
                }
                else
                {
                    // PartnerClass isn't ORGANISATION, we don't need to check Foundation Security
                    return Ict.Petra.Shared.MPartner.TSecurity.CanAccessPartner(APartnerRow,
                        false, null);
                }
            }
            else
            {
                // Invalid Partner. Access Level is Granted in this case.
                return TPartnerAccessLevelEnum.palGranted;
            }
        }

        /// <summary>
        /// Tests whether the current user has access to a particular Partner.
        /// </summary>
        /// <remarks>This Method throws an <see cref="T:ESecurityPartnerAccessDeniedException" />
        /// if access to the Partner is not granted, thereby ensuring that a denied access
        /// doesn't go unnoticed.</remarks>
        /// <param name="APartnerRow">Partner for which access should be checked for.</param>
        /// <returns>void</returns>
        /// <exception cref="T:ESecurityPartnerAccessDeniedException">Thrown if access is not granted.</exception>
        public static void CanAccessPartnerExc(PPartnerRow APartnerRow)
        {
            TPartnerAccessLevelEnum AccessLevel;

            AccessLevel = CanAccessPartner(APartnerRow);

            Ict.Petra.Shared.MPartner.TSecurity.AccessLevelExceptionEvaluatorAndThrower(
                APartnerRow, AccessLevel);
        }

        /// <summary>
        /// Tests whether the current user has access to a particular Partner.
        /// </summary>
        /// <remarks>Corresponds to Progress 4GL Method 'CanAccessPartner' in
        /// common/sp_partn.p</remarks>
        /// <param name="APartnerKey">PartnerKey of Partner for which access should
        /// be checked for.</param>
        /// <param name="AThrowExceptionIfDenied"></param>
        /// <returns><see cref="TPartnerAccessLevelEnum.palGranted" /> if access
        /// to the Partner is granted (or Partner doesn't exist), otherwise a different
        /// <see cref="TPartnerAccessLevelEnum" /> value.</returns>
        public static TPartnerAccessLevelEnum CanAccessPartnerByKey(Int64 APartnerKey,
            bool AThrowExceptionIfDenied)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            PPartnerTable PartnerTable;

            if (APartnerKey != 0)
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                // Load Partner
                try
                {
                    PartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

                    if (PartnerTable.Rows.Count > 0)
                    {
                        // Partner exists, now check Access Level
                        if (!AThrowExceptionIfDenied)
                        {
                            return CanAccessPartner(PartnerTable[0]);
                        }
                        else
                        {
                            CanAccessPartnerExc(PartnerTable[0]);

                            /*
                             * The previous Method call would throw an Exception
                             * in case access would not be granted and program execution
                             * would leave this Method at that point; if it doesn't
                             * then this implies that access is granted.
                             */
                            return TPartnerAccessLevelEnum.palGranted;
                        }
                    }
                    else
                    {
                        // Partner not found. Access Level is Granted in this case.
                        return TPartnerAccessLevelEnum.palGranted;
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel(8, "TSecurity.CanAccessPartnerByKey: committed own transaction.");
                    }
                }
            }
            else
            {
                // Invalid Partner. Access Level is Granted in this case.
                return TPartnerAccessLevelEnum.palGranted;
            }
        }
    }
}