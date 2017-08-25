//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2017 by OM International
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
using Ict.Common.Data;
using Ict.Common.Verification;

using Ict.Petra.Shared;

namespace Ict.Petra.Shared.MPartner.Validation
{
    /// <summary>
    /// Contains helper functions for the shared validation of Partner data.
    /// </summary>
    public static class TSharedPartnerValidationHelper
    {
        /// <summary>Delegate for invoking the verification of the existence of a Partner.</summary>
        public delegate bool TVerifyPartner(Int64 APartnerKey, TPartnerClass[] AValidPartnerClasses, out bool APartnerExists,
            out String APartnerShortName, out TPartnerClass APartnerClass, out TStdPartnerStatusCode APartnerStatus);

        /// <summary>Delegate for confirming that a Partner is active.</summary>
        public delegate Boolean TPartnerHasActiveStatus(Int64 APartner);

        /// <summary>Delegate to determine Partner is linked to CC</summary>
        /// <param name="APartnerKey"></param>
        /// <returns></returns>
        public delegate Boolean TPartnerIsLinkedToCC(Int64 APartnerKey);

        /// <summary>Delegate to determine Partner of type CC is linked</summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APartnerKey"></param>
        /// <returns></returns>
        public delegate Boolean TPartnerOfTypeCCIsLinked(Int32 ALedgerNumber, Int64 APartnerKey);

        /// <summary>Delegate to determine Partner has a valid gift destination</summary>
        /// <param name="APartnerKey"></param>
        /// <param name="AGiftDate"></param>
        /// <returns></returns>
        public delegate Boolean TPartnerHasCurrentGiftDestination(Int64 APartnerKey, DateTime ? AGiftDate);

        /// <summary>
        /// Reference to the Delegate for invoking the verification of the existence of a Partner.
        /// </summary>
        private static TVerifyPartner FDelegateVerifyPartner;

        private static TPartnerHasActiveStatus FDelegatePartnerHasActiveStatus;

        private static TPartnerIsLinkedToCC FDelegatePartnerIsLinkedToCC;

        private static TPartnerOfTypeCCIsLinked FDelegatePartnerOfTypeCCIsLinked;

        private static TPartnerHasCurrentGiftDestination FDelegatePartnerHasCurrentGiftDestination;

        /// <summary>
        /// This property is used to provide a function which invokes the verification of the existence of a Partner.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TVerifyPartner VerifyPartnerDelegate
        {
            get
            {
                return FDelegateVerifyPartner;
            }

            set
            {
                FDelegateVerifyPartner = value;
            }
        }

        /// <summary></summary>
        public static TPartnerHasActiveStatus PartnerHasActiveStatusDelegate
        {
            get
            {
                return FDelegatePartnerHasActiveStatus;
            }
            set
            {
                FDelegatePartnerHasActiveStatus = value;
            }
        }

        /// <summary>
        /// A function must be provided before the helper function is called.
        /// </summary>
        public static TPartnerIsLinkedToCC PartnerIsLinkedToCCDelegate
        {
            get
            {
                return FDelegatePartnerIsLinkedToCC;
            }
            set
            {
                FDelegatePartnerIsLinkedToCC = value;
            }
        }

        /// <summary>
        /// A function must be provided before the helper function is called.
        /// </summary>
        public static TPartnerOfTypeCCIsLinked PartnerOfTypeCCIsLinkedDelegate
        {
            get
            {
                return FDelegatePartnerOfTypeCCIsLinked;
            }
            set
            {
                FDelegatePartnerOfTypeCCIsLinked = value;
            }
        }

        /// <summary>
        /// A function must be provided before the helper function is called.
        /// </summary>
        public static TPartnerHasCurrentGiftDestination PartnerHasCurrentGiftDestinationDelegate
        {
            get
            {
                return FDelegatePartnerHasCurrentGiftDestination;
            }
            set
            {
                FDelegatePartnerHasCurrentGiftDestination = value;
            }
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
        public static bool VerifyPartner(Int64 APartnerKey, TPartnerClass[] AValidPartnerClasses, out bool APartnerExists,
            out String APartnerShortName, out TPartnerClass APartnerClass, out TStdPartnerStatusCode APartnerStatus)
        {
            if (FDelegateVerifyPartner != null)
            {
                return FDelegateVerifyPartner(APartnerKey, AValidPartnerClasses, out APartnerExists, out APartnerShortName,
                    out APartnerClass, out APartnerStatus);
            }
            else
            {
                throw new InvalidOperationException("Delegate 'TVerifyPartner' must be initialised before calling this Method");
            }
        }

        /// <summary></summary>
        /// <param name="APartnerKey"></param>
        /// <returns>True if the status of this partner is listed as Active (Bypasses the status enum.)</returns>
        public static Boolean PartnerHasActiveStatus(Int64 APartnerKey)
        {
            if (FDelegatePartnerHasActiveStatus != null)
            {
                return FDelegatePartnerHasActiveStatus(APartnerKey);
            }
            else
            {
                throw new InvalidOperationException("Delegate 'PartnerHasActiveStatus' must be initialised before calling this Method");
            }
        }

        /// <summary>Attempts to call a delegate...</summary>
        /// <param name="APartnerKey"></param>
        /// <returns></returns>
        public static Boolean PartnerIsLinkedToCC(Int64 APartnerKey)
        {
            if (FDelegatePartnerIsLinkedToCC != null)
            {
                return FDelegatePartnerIsLinkedToCC(APartnerKey);
            }
            else
            {
                throw new InvalidOperationException("Delegate 'PartnerIsLinkedToCC' must be initialised before calling this Method");
            }
        }

        /// <summary>Attempts to call a delegate...</summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="APartnerKey"></param>
        /// <returns></returns>
        public static Boolean PartnerOfTypeCCIsLinked(Int32 ALedgerNumber, Int64 APartnerKey)
        {
            if (FDelegatePartnerOfTypeCCIsLinked != null)
            {
                return FDelegatePartnerOfTypeCCIsLinked(ALedgerNumber, APartnerKey);
            }
            else
            {
                throw new InvalidOperationException("Delegate 'PartnerOfTypeCCIsLinked' must be initialised before calling this Method");
            }
        }

        /// <summary>Attempts to call a delegate...</summary>
        /// <param name="APartnerKey"></param>
        /// <param name="AGiftDate"></param>
        /// <returns></returns>
        public static Boolean PartnerHasCurrentGiftDestination(Int64 APartnerKey, DateTime ? AGiftDate)
        {
            if (FDelegatePartnerHasCurrentGiftDestination != null)
            {
                return FDelegatePartnerHasCurrentGiftDestination(APartnerKey, AGiftDate);
            }
            else
            {
                throw new InvalidOperationException("Delegate 'PartnerHasCurrentGiftDestination' must be initialised before calling this Method");
            }
        }
    }
}
