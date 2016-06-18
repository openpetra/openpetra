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

namespace Ict.Petra.Shared.MPartner
{
    /// <summary>
    /// enumeration for the tab pages on Partner Edit screen
    /// </summary>
    public enum TPartnerEditTabPageEnum
    {
        /// <summary>
        /// the default page
        /// </summary>
        petpDefault,

        /// <summary>
        /// address page
        /// </summary>
        petpAddresses,

        /// <summary>
        /// detail page (different for each partner class)
        /// </summary>
        petpDetails,

        /// <summary>
        /// page for contacts
        /// </summary>
        petpContactDetails,

        /// <summary>
        /// detail page for partner class foundations
        /// </summary>
        petpFoundationDetails,

        /// <summary>
        /// page for subscriptions
        /// </summary>
        petpSubscriptions,

        /// <summary>
        /// page for partner types
        /// </summary>
        petpPartnerTypes,

        /// <summary>
        /// page for family members
        /// </summary>
        petpFamilyMembers,

        /// <summary>
        /// page for office specific data labels
        /// </summary>
        petpOfficeSpecific,

        /// <summary>
        /// page for interests
        /// </summary>
        petpInterests,

        /// <summary>
        /// page for reminders
        /// </summary>
        petpReminders,

        /// <summary>
        /// page for relationships between partners
        /// </summary>
        petpPartnerRelationships,

        /// <summary>
        /// page for contact management
        /// </summary>
        petpContacts,

        /// <summary>
        /// page for notes about the partner
        /// </summary>
        petpNotes,

        /// <summary>
        /// page for bank accounts and other finance details of the partner
        /// </summary>
        petpFinanceDetails,

        /// <summary>
        /// page for individual data about the partner (Personnel Module)
        /// </summary>
        petpPersonnelIndividualData,

        /// <summary>
        /// page for applications of the partner (Personnel Module)
        /// </summary>
        petpPersonnelApplications
    };

    /// <summary>
    /// Specifies the Scope of data that is to be returned to the Partner Info UserControl.
    /// </summary>
    public enum TPartnerInfoScopeEnum
    {
        /// <summary>Partner 'head' data only</summary>
        pisHeadOnly,

        /// <summary>Partner Location only, excluding rest of the data and 'head' data</summary>
        pisPartnerLocationOnly,

        /// <summary>Partner Location and rest of the data only, excluding 'head' data</summary>
        pisPartnerLocationAndRestOnly,

        /// <summary>Location and Partner Location only, excluding rest of the data and 'head' data</summary>
        pisLocationPartnerLocationOnly,

        /// <summary>Location, Partner Location and rest of the data only, excluding 'head' data</summary>
        pisLocationPartnerLocationAndRestOnly,

        /// <summary>Partner Contact Details only, excluding rest of the data and 'head' data</summary>
        pisPartnerAttributesOnly,

        /// <summary>All PartnerInfo data</summary>
        pisFull
    }

    /// different ways to format the shortname of a partner
    /// used eg by Ict.Petra.Shared.MPartner.Calculations.FormatShortName()
    public enum eShortNameFormat
    {
        /// lastname, firstname, title
        eShortname,

        /// title firstname lastname
        eReverseShortname,

        /// title
        eOnlyTitle,

        /// family name
        eOnlySurname,

        /// first name
        eOnlyFirstname,

        /// firstname l.; useful for data protection
        eReverseLastnameInitialsOnly,

        /// firstname lastname
        eReverseWithoutTitle,

        /// lastname, firstname (just remove title field)
        eJustRemoveTitle
    };

    /// <summary>
    /// 'Value Kinds' of PPartnerAttributeType records. They describe what kind of value the 'Value' of a PPartnerAttribute record represents.
    /// </summary>
    public enum TPartnerAttributeTypeValueKind
    {
        /// <summary>
        /// Value which is not a kind of value that the other members of this Enumeration represent.
        /// </summary>
        CONTACTDETAIL_GENERAL,

        /// <summary>
        /// Hyperlink (for various protocols, e.g. HTTP, HTTPS, FTP, etc. )
        /// </summary>
        CONTACTDETAIL_HYPERLINK,

        /// <summary>
        /// Hyperlink that contains a text part that is replaced with a variable value
        /// </summary>
        CONTACTDETAIL_HYPERLINK_WITHVALUE,

        /// <summary>
        /// E-Mail Address
        /// </summary>
        CONTACTDETAIL_EMAILADDRESS,

        /// <summary>
        /// Skype ID
        /// </summary>
        CONTACTDETAIL_SKYPEID
    };

    /// <summary>
    /// Defines which merge action we need data for (used in Partner Merge)
    /// </summary>
    public enum TMergeActionEnum
    {
        /// <summary>
        /// Address
        /// </summary>
        ADDRESS,

        /// <summary>
        /// Contact Detail
        /// </summary>
        CONTACTDETAIL,

        /// <summary>
        /// Bank Account
        /// </summary>
        BANKACCOUNT
    };

    /// <summary>
    /// Class that holds a combination of SiteKey and LocationKey.
    /// </summary>
    [Serializable()]
    public class TLocationPK
    {
        private Int64 FSiteKey;
        private Int32 FLocationKey;

        /// <summary>
        /// SiteKey.
        /// </summary>
        public Int64 SiteKey
        {
            get
            {
                return FSiteKey;
            }

            set
            {
                FSiteKey = value;
            }
        }

        /// <summary>
        /// LocationKey.
        /// </summary>
        public Int32 LocationKey
        {
            get
            {
                return FLocationKey;
            }

            set
            {
                FLocationKey = value;
            }
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public TLocationPK() : base()
        {
            FSiteKey = -1;
            FLocationKey = -1;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ASiteKey">SiteKey.</param>
        /// <param name="ALocationKey">LocationKey.</param>
        public TLocationPK(Int64 ASiteKey, Int32 ALocationKey) : base()
        {
            FSiteKey = ASiteKey;
            FLocationKey = ALocationKey;
        }

        /// <summary>
        /// Returns true if objects are the same.
        /// </summary>
        /// <param name="AObject"></param>
        /// <returns></returns>
        public override bool Equals(object AObject)
        {
            if ((AObject == null) || !(AObject is TLocationPK))
            {
                return false;
            }

            return ((TLocationPK)AObject).LocationKey == this.LocationKey
                   && ((TLocationPK)AObject).SiteKey == this.SiteKey;
        }

        /// <summary>
        /// Returns a unique hash code for this object.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)FSiteKey + FLocationKey;
        }
    }

    /// <summary>
    /// Static helper class for creating 'deep copies' of two-dimensional <see cref="TLocationPK" /> arrays.
    /// </summary>
    public static class TLocationPKCopyHelper
    {
        /// <summary>
        /// Creates a 'deep' copy (i.e. not just copying of references but a real duplication of data)
        /// of a two-dimensional <see cref="TLocationPK" /> array, optionally exitending the resulting
        /// two-dimensional array by a number of empty array items.
        /// </summary>
        /// <param name="ASource">Two-dimensionaly <see cref="TLocationPK" /> array to copy from.</param>
        /// <param name="AExtendArrayByNEmptyItems">Specify the number of array items that you want the resulting array to be
        /// extended by (default = 0).</param>
        /// <returns>Deep Copy of <paramref name="ASource"/>, optionally extended by a number of empty array items.</returns>
        public static TLocationPK[, ] CopyTLocationPKArray(TLocationPK[, ] ASource, int AExtendArrayByNEmptyItems = 0)
        {
            TLocationPK[, ] ReturnValue = new TLocationPK[(ASource.Length / 2) + AExtendArrayByNEmptyItems, 2];

            for (int Counter = 0; Counter < ASource.Length / 2; Counter++)
            {
                ReturnValue[Counter, 0] = ASource[Counter, 0];
                ReturnValue[Counter, 1] = ASource[Counter, 1];
            }

            return ReturnValue;
        }
    }
}
