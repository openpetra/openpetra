//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2015 by OM International
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
using System.Text;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Exceptions;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Shared.MPartner
{
    /// <summary>
    /// Contains functions to be used by the Server and the Client that perform
    /// certain calculations - specific for the Partner Module.
    /// </summary>
    /// <remarks>This part of this Partial Class holds only Methods that are to do with the
    /// 'Contact Detail' implementation.</remarks>
    public partial class Calculations
    {
        #region Overall Contact Settings (incl. 'Primary E-Mail' and 'Primary Phone Number')

        /// <summary>
        /// Kinds of 'Overall Contact Settings'
        /// </summary>
        [Flags]
        public enum TOverallContSettingKind
        {
            /// <summary>Primary Email Address.</summary>
            ocskPrimaryEmailAddress = 1,

            /// <summary>Primary Phone Number.</summary>
            ocskPrimaryPhoneNumber = 2,

            /// <summary>Email Address Within Organisation.</summary>
            ocskEmailAddressWithinOrg = 4,

            /// <summary>Phone Number Within Organisation.</summary>
            ocskPhoneNumberWithinOrg = 8,

            /// <summary>Primary Contact Method.</summary>
            ocskPrimaryContactMethod = 16,

            // TODO ocskSecondaryEmailAddress (extension based on request from OM D)
//            ocskSecondaryEmailAddress = 32
        }

        /// <summary>
        /// Holds details of a Partners' 'Overall Contact Settings'
        /// </summary>
        public class TPartnersOverallContactSettings
        {
            /// <summary>Primary Email Address.</summary>
            public string PrimaryEmailAddress
            {
                get;
                set;
            }

            /// <summary>Primary Phone Number.</summary>
            public string PrimaryPhoneNumber
            {
                get;
                set;
            }

            /// <summary>Email Address Within Organisation.</summary>
            public string EmailAddressWithinOrg
            {
                get;
                set;
            }

            /// <summary>Phone Number Within Organisation.</summary>
            public string PhoneNumberWithinOrg
            {
                get;
                set;
            }

            /// <summary>Primary Contact Method.</summary>
            public string PrimaryContactMethod
            {
                get;
                set;
            }

            // TODO_LOW SecondaryEmailAddress (extension based on request from OM D)
//            public string SecondaryEmailAddress
//            {
//                get;
//                set;
//            }
        }

        /// <summary>
        /// Holds the 'Overall Contact Settings' of an arbitrary number of Partners.
        /// </summary>
        /// <remarks>Use the public Method <see cref="GetPartnersPrimaryEmailAddress"/>
        /// to get the 'Primary E-Mail Address' of a specific Partner!</remarks>
        public class TOverallContactSettings : Dictionary <Int64, TPartnersOverallContactSettings>
        {
            /// <summary>
            /// Constructor. Simply calls the base Constructor.
            /// </summary>
            public TOverallContactSettings() : base()
            {
            }

            /// <summary>
            /// Convenient Method for getting the 'Primary E-Mail Address' of a Partner.
            /// </summary>
            /// <param name="APartnerKey">PartnerKey of a Partner.</param>
            /// <returns>The 'Primary E-Mail Address' of the Partner with PartnerKey
            /// <paramref name="APartnerKey"/>, or null in case the Partner hasn't
            /// got a Primary Email Address set. null is also returned if the Partner
            /// doesn't exist in this instance of <see cref="TOverallContactSettings"/>.</returns>
            public string GetPartnersPrimaryEmailAddress(Int64 APartnerKey)
            {
                return GetPartnersOvrlContAttribute(APartnerKey, TOverallContSettingKind.ocskPrimaryEmailAddress);
            }

            /// <summary>
            /// Convenient Method for getting the 'Primary Phone Number' of a Partner.
            /// </summary>
            /// <param name="APartnerKey">PartnerKey of a Partner.</param>
            /// <returns>The 'Primary Phone Number' of the Partner with PartnerKey
            /// <paramref name="APartnerKey"/>, or null in case the Partner hasn't
            /// got a Primary Phone Number set. null is also returned if the Partner
            /// doesn't exist in this instance of <see cref="TOverallContactSettings"/>.</returns>
            public string GetPartnersPrimaryPhoneNumber(Int64 APartnerKey)
            {
                return GetPartnersOvrlContAttribute(APartnerKey, TOverallContSettingKind.ocskPrimaryPhoneNumber);
            }

            /// <summary>
            /// Gets the value of a Partners' 'Overall Contact Setting'.
            /// </summary>
            /// <remarks>Other Methods that start with the name 'GetPartners...' exist that return a specific
            /// 'Overall Contact Setting' without the need of specifying <paramref name="AOverallContSettingKind"/>.
            /// </remarks>
            /// <param name="APartnerKey">PartnerKey of a Partner.</param>
            /// <param name="AOverallContSettingKind">The Partners' 'Overall Contact Setting' that the value should
            /// be retrieved for.</param>
            /// <returns>The value of the Partners' 'Overall Contact Setting' specified with
            /// <paramref name="AOverallContSettingKind"/> of the Partner with PartnerKey
            /// <paramref name="APartnerKey"/>, or null in case the Partner hasn't
            /// got a a value set for the specified Partners' 'Overall Contact Setting'. null is also returned if the Partner
            /// doesn't exist in this instance of <see cref="TOverallContactSettings"/>.</returns>
            public string GetPartnersOvrlContAttribute(Int64 APartnerKey, TOverallContSettingKind AOverallContSettingKind)
            {
                TPartnersOverallContactSettings OverallContSettings;

                if (TryGetValue(APartnerKey, out OverallContSettings))
                {
                    switch (AOverallContSettingKind)
                    {
                        case TOverallContSettingKind.ocskPrimaryEmailAddress:
                            return OverallContSettings.PrimaryEmailAddress;

                        case TOverallContSettingKind.ocskPrimaryPhoneNumber:
                            return OverallContSettings.PrimaryPhoneNumber;

                        case TOverallContSettingKind.ocskEmailAddressWithinOrg:
                            return OverallContSettings.EmailAddressWithinOrg;

                        case TOverallContSettingKind.ocskPhoneNumberWithinOrg:
                            return OverallContSettings.PhoneNumberWithinOrg;

                        default:
                            return null;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Partner Attribute Type that denotes the 'Primary Contact Method'.
        /// </summary>
        public const string ATTR_TYPE_PARTNERS_PRIMARY_CONTACT_METHOD = "PARTNERS_PRIMARY_CONTACT_METHOD";

        /// <summary>Column name for the column 'Partner Contact Detail' that can get added to a PPartnerAttribute Table
        /// (gets added by Method <see cref="DeterminePartnerContactDetailAttributes"/>).</summary>
        public static readonly String PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN = "PartnerContactDetail";

        /// <summary>Column name for a column that can get added to a PPartnerAttribute Table (gets added by Method
        /// <see cref="CreateCustomDataColumnsForAttributeTable"/>).</summary>
        public static readonly string CALCCOLUMNNAME_PARTNERATTRIBUTETYPE = "Parent_" + PPartnerAttributeTypeTable.GetAttributeTypeDBName();
        /// <summary>Column name for a column that can get added to a PPartnerAttribute Table (gets added by Method
        /// <see cref="CreateCustomDataColumnsForAttributeTable"/>).</summary>
        public static readonly string CALCCOLUMNNAME_PARENTATTRIBUTEINDEX = "Parent_AttributeIndex";
        /// <summary>Column name for a column that can get added to a PPartnerAttribute Table (gets added by Method
        /// <see cref="CreateCustomDataColumnsForAttributeTable"/>).</summary>
        public static readonly string CALCCOLUMNNAME_PARENTPARENTCATEGORYINDEX = "Parent_Parent_CategoryIndex";
        /// <summary>Column name for a column that can get added to a PPartnerAttribute Table (gets added by Method
        /// <see cref="CreateCustomDataColumnsForAttributeTable"/>).</summary>
        public static readonly string CALCCOLUMNNAME_CONTACTTYPE = "ContactType";
        /// <summary>Column name for a column that can get added to a PPartnerAttribute Table (gets added by Method
        /// <see cref="CreateCustomDataColumnsForAttributeTable"/>).</summary>
        public static readonly string CALCCOLUMNNAME_CATEGORYINDEX = "CategoryIndex";


        /// <summary>
        /// Gets the 'Primary E-Mail Address' of a Partner.
        /// </summary>
        /// <param name="APPartnerAttributeDT"><see cref="PPartnerAttributeTable"/> that contains the Contact Detail
        /// records of the Partner whose 'Primary Email Address' should be determined.</param>
        /// <param name="APrimaryEmailAddress">The 'Primary Email Address' if the Partner has got one, otherwise null.</param>
        /// <returns>True if the Partner has got a 'Primary Email Address', otherwise false.</returns>
        public static bool GetPrimaryEmailAddress(PPartnerAttributeTable APPartnerAttributeDT,
            out string APrimaryEmailAddress)
        {
            TPartnersOverallContactSettings PrimaryContactAttributes = DeterminePrimaryOrWithinOrgSettingsForPartner(
                APPartnerAttributeDT, TOverallContSettingKind.ocskPrimaryEmailAddress);

            return GetPrimaryEmailAddress(PrimaryContactAttributes, out APrimaryEmailAddress);
        }

        /// <summary>
        /// Gets the 'Primary E-Mail Address' of a Partner.
        /// </summary>
        /// <param name="APrimaryContactAttributes"><see cref="TPartnersOverallContactSettings"/> instance that
        /// contains the 'Overall Contact Details' of a certain Partner.</param>
        /// <param name="APrimaryEmailAddress">The 'Primary E-Mail Address' if the Partner has got one, otherwise null.</param>
        /// <returns>True if the Partner has got a 'Primary E-Mail Address', otherwise false.</returns>
        public static bool GetPrimaryEmailAddress(TPartnersOverallContactSettings APrimaryContactAttributes,
            out string APrimaryEmailAddress)
        {
            APrimaryEmailAddress = null;

            if (APrimaryContactAttributes != null)
            {
                // Partners' 'Primary E-Mail Address' (null is supplied if the Partner hasn't got one)
                APrimaryEmailAddress = StringHelper.NullOrEmptyStringToNull(APrimaryContactAttributes.PrimaryEmailAddress);

                return APrimaryEmailAddress != null;
            }

            return false;
        }

        /// <summary>
        /// Gets the 'Primary Phone Number' of a Partner.
        /// </summary>
        /// <param name="APPartnerAttributeDT"><see cref="PPartnerAttributeTable"/> that contains the Contact Detail
        /// records of the Partner whose 'Primary Phone Number' should be determined.</param>
        /// <param name="APrimaryPhoneNumber">The 'Primary Phone Number' if the Partner has got one, otherwise null.</param>
        /// <returns>True if the Partner has got a 'Primary Phone Number', otherwise false.</returns>
        public static bool GetPrimaryPhoneNumber(PPartnerAttributeTable APPartnerAttributeDT,
            out string APrimaryPhoneNumber)
        {
            TPartnersOverallContactSettings PrimaryContactAttributes = DeterminePrimaryOrWithinOrgSettingsForPartner(
                APPartnerAttributeDT, TOverallContSettingKind.ocskPrimaryPhoneNumber);

            return GetPrimaryPhoneNumber(PrimaryContactAttributes, out APrimaryPhoneNumber);
        }

        /// <summary>
        /// Gets the 'Primary Phone Number' of a Partner.
        /// </summary>
        /// <param name="APrimaryContactAttributes"><see cref="TPartnersOverallContactSettings"/> instance that
        /// contains the 'Overall Contact Details' of a certain Partner.</param>
        /// <param name="APrimaryPhoneNumber">The 'Primary Phone Number' if the Partner has got one, otherwise null.</param>
        /// <returns>True if the Partner has got a 'Primary Phone Number', otherwise false.</returns>
        public static bool GetPrimaryPhoneNumber(TPartnersOverallContactSettings APrimaryContactAttributes,
            out string APrimaryPhoneNumber)
        {
            APrimaryPhoneNumber = null;

            if (APrimaryContactAttributes != null)
            {
                // Partners' 'Primary Phone Number' (null is supplied if the Partner hasn't got one)
                APrimaryPhoneNumber = StringHelper.NullOrEmptyStringToNull(APrimaryContactAttributes.PrimaryPhoneNumber);

                return APrimaryPhoneNumber != null;
            }

            return false;
        }

        /// <summary>
        /// Gets the 'Within Organisation E-Mail Address' of a Partner.
        /// </summary>
        /// <param name="APPartnerAttributeDT"><see cref="PPartnerAttributeTable"/> that contains the Contact Detail
        /// records of the Partner whose 'Within Organisation E-Mail Address' should be determined.</param>
        /// <param name="AWithinOrganisationEmailAddress">The 'Within Organisation E-Mail Address' if the Partner has got
        /// one, otherwise null.</param>
        /// <returns>True if the Partner has got a 'Within Organisation E-Mail Address', otherwise false.</returns>
        public static bool GetWithinOrganisationEmailAddress(PPartnerAttributeTable APPartnerAttributeDT,
            out string AWithinOrganisationEmailAddress)
        {
            TPartnersOverallContactSettings PrimaryContactAttributes = DeterminePrimaryOrWithinOrgSettingsForPartner(
                APPartnerAttributeDT, TOverallContSettingKind.ocskEmailAddressWithinOrg);

            return GetWithinOrganisationEmailAddress(PrimaryContactAttributes, out AWithinOrganisationEmailAddress);
        }

        /// <summary>
        /// Gets the 'Within Organisation E-Mail Address' of a Partner.
        /// </summary>
        /// <param name="APrimaryContactAttributes"><see cref="TPartnersOverallContactSettings"/> instance that
        /// contains the 'Overall Contact Details' of a certain Partner.</param>
        /// <param name="AWithinOrganisationEmailAddress">The 'Within Organisation E-Mail Address' if the Partner has got one, otherwise null.</param>
        /// <returns>True if the Partner has got a 'Within Organisation E-Mail Address', otherwise false.</returns>
        public static bool GetWithinOrganisationEmailAddress(TPartnersOverallContactSettings APrimaryContactAttributes,
            out string AWithinOrganisationEmailAddress)
        {
            AWithinOrganisationEmailAddress = null;

            if (APrimaryContactAttributes != null)
            {
                // Partners' 'Within Organisation E-Mail Address' (null is supplied if the Partner hasn't got one)
                AWithinOrganisationEmailAddress = StringHelper.NullOrEmptyStringToNull(
                    APrimaryContactAttributes.EmailAddressWithinOrg);

                return AWithinOrganisationEmailAddress != null;
            }

            return false;
        }

        /// <summary>
        /// Gets the 'Primary Phone Number' and the 'Primary E-mail Address' of a Partner.
        /// </summary>
        /// <param name="APPartnerAttributeDT"><see cref="PPartnerAttributeTable"/> that contains the Contact Detail
        /// records of the Partner whose 'Primary Phone Number' and 'Primary E-mail Address'should be determined.</param>
        /// <param name="APrimaryPhoneNumber">The 'Primary Phone Number' if the Partner has got one, otherwise null.</param>
        /// <param name="APrimaryEmailAddress">The 'Primary E-mail Address' if the Partner has got one, otherwise null.</param>
        /// <returns>True if the Partner has got at least one of the 'Primary E-mail Address' and the 'Primary Phone Number'
        /// Contact Details, otherwise false.</returns>
        public static bool GetPrimaryEmailAndPrimaryPhone(PPartnerAttributeTable APPartnerAttributeDT,
            out string APrimaryPhoneNumber, out string APrimaryEmailAddress)
        {
            TPartnersOverallContactSettings PrimaryContactAttributes = DeterminePrimaryOrWithinOrgSettingsForPartner(
                APPartnerAttributeDT,
                TOverallContSettingKind.ocskPrimaryPhoneNumber |
                TOverallContSettingKind.ocskPrimaryEmailAddress);

            return GetPrimaryEmailAndPrimaryPhone(PrimaryContactAttributes,
                out APrimaryPhoneNumber, out APrimaryEmailAddress);
        }

        /// <summary>
        /// Gets the 'Primary Phone Number' and the 'Primary E-mail Address' of a Partner.
        /// </summary>
        /// <param name="APrimaryContactAttributes"><see cref="TPartnersOverallContactSettings"/> instance that
        /// contains the 'Overall Contact Details' of a certain Partner.
        /// records of the Partner whose 'Primary Phone Number' and 'Primary E-mail Address'should be determined.</param>
        /// <param name="APrimaryPhoneNumber">The 'Primary Phone Number' if the Partner has got one, otherwise null.</param>
        /// <param name="APrimaryEmailAddress">The 'Primary E-mail Address' if the Partner has got one, otherwise null.</param>
        /// <returns>True if the Partner has got at least one of the 'Primary E-mail Address' and the 'Primary Phone Number'
        /// Contact Details, otherwise false.</returns>
        public static bool GetPrimaryEmailAndPrimaryPhone(TPartnersOverallContactSettings APrimaryContactAttributes,
            out string APrimaryPhoneNumber, out string APrimaryEmailAddress)
        {
            APrimaryPhoneNumber = null;
            APrimaryEmailAddress = null;

            if (APrimaryContactAttributes != null)
            {
                // Partners' 'Primary Phone Number' (null is supplied if the Partner hasn't got one)
                APrimaryPhoneNumber = StringHelper.NullOrEmptyStringToNull(APrimaryContactAttributes.PrimaryPhoneNumber);

                // Partners' 'Primary Email Address' (null is supplied if the Partner hasn't got one)
                APrimaryEmailAddress = StringHelper.NullOrEmptyStringToNull(APrimaryContactAttributes.PrimaryEmailAddress);

                return (APrimaryPhoneNumber != null) || (APrimaryEmailAddress != null);
            }

            return false;
        }

        /// <summary>
        /// Determines the 'Primary' and/or 'Within Organisation' setting(s) for the Partner whose PPartnerAttributes records
        /// are contained in <paramref name="APPartnerAttributeDT"/>.
        /// </summary>
        /// <param name="APPartnerAttributeDT"><see cref="PPartnerAttributeTable"/> that contains the Contact Detail
        /// records of a given Partner. The DataTable must have a special DataColumn added that Method
        /// 'DeterminePartnerContactDetailAttributes' adds, hence that Method must be called before calling this Method.</param>
        /// <param name="AOverallContSettingKind">Specify the kind of Overall Contact Setting(s) that you want returned.
        /// Combine multiple ones with the binary OR operator ( | ).</param>
        /// <returns>An instance of <see cref="TPartnersOverallContactSettings"/> that holds the
        /// <see cref="TPartnersOverallContactSettings"/> for the Partner. However, it returns null in case
        /// <paramref name="APPartnerAttributeDT"/> holds no records, or when it contains only one record but this records'
        /// Current flag is false. It also returns null if no record was found that met what was asked for with
        /// <paramref name="AOverallContSettingKind"/>!</returns>
        public static TPartnersOverallContactSettings DeterminePrimaryOrWithinOrgSettingsForPartner(
            PPartnerAttributeTable APPartnerAttributeDT, TOverallContSettingKind AOverallContSettingKind)
        {
            TPartnersOverallContactSettings ReturnValue;
            TOverallContactSettings MockOverallContactSettings = null;
            bool PrimaryIsAskedFor;
            bool WithinOrgIsAskedFor;
            bool PrimaryQuickExitConditionMet;
            bool WithinOrgQuickExitConditionMet;
            TOverallContSettingKind CheckFlags;

            //
            // Several 'quick checks' to save ourselves the creation of (a) DataView(s) it if isn't necessary!
            //
            if (!QuickChecksForPrimaryOrWithinOrgSettings(APPartnerAttributeDT, AOverallContSettingKind, -1,
                    out PrimaryIsAskedFor, out WithinOrgIsAskedFor,
                    out PrimaryQuickExitConditionMet, out WithinOrgQuickExitConditionMet))
            {
                return null;
            }

            ReturnValue = new TPartnersOverallContactSettings();

            //
            // None of the 'quick checks' returned a 'quick result': Create necessary DataView(s) and process what they find
            //

            // Check for Email Addresses
            CheckFlags = TOverallContSettingKind.ocskPrimaryEmailAddress |
                         TOverallContSettingKind.ocskEmailAddressWithinOrg;

            if ((AOverallContSettingKind & CheckFlags) != 0)
            {
                ParsePartnerOvrlCS(APPartnerAttributeDT[0].PartnerKey, APPartnerAttributeDT, ref MockOverallContactSettings,
                    ref ReturnValue, true, PrimaryIsAskedFor, WithinOrgIsAskedFor);
            }

            // Check for Phone Numbers
            CheckFlags = TOverallContSettingKind.ocskPrimaryPhoneNumber |
                         TOverallContSettingKind.ocskPhoneNumberWithinOrg;

            if ((AOverallContSettingKind & CheckFlags) != 0)
            {
                ParsePartnerOvrlCS(APPartnerAttributeDT[0].PartnerKey, APPartnerAttributeDT, ref MockOverallContactSettings,
                    ref ReturnValue, false, PrimaryIsAskedFor, WithinOrgIsAskedFor);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Determines the 'Primary' and/or 'Within Organisation' setting(s) for (each of) the Partner(s) contained in
        /// <paramref name="APPartnerAttributeDT"/>.
        /// </summary>
        /// <param name="APPartnerAttributeDT"><see cref="PPartnerAttributeTable"/> that contains the Contact Detail
        /// records of a given Partner - or of MANY (!) Partners. The DataTable must have a special DataColumn added that
        /// Method 'DeterminePartnerContactDetailAttributes' adds, hence that Method must be called before calling this Method.
        /// </param>
        /// <param name="APartnerKey">Set this to a Partners' PartnerKey to retrieve the desired 'Primary' and/or
        /// 'Within Organisation' setting(s) for a specific Partner only (in case there are many Partners' PPartnerAttribute
        /// records contained in <paramref name="APPartnerAttributeDT"/>). (Default = -1, meaning: determine the
        /// 'Primary' and/or 'Within Organisation' setting(s) for each Partner for which PPartnerAttribute records exist in
        /// <paramref name="APPartnerAttributeDT"/>.
        /// <para>Caveat 1: If you only need to determine the 'Primary' and/or 'Within Organisation' setting(s) for a *single*
        /// Partner and you have an instance of <paramref name="APPartnerAttributeDT"/> that holds only the records for
        /// *that* Partner then it is more effective to call the Method
        /// <see cref="DeterminePrimaryOrWithinOrgSettingsForPartner" /> than the present Method!</para>
        /// <para>Caveat 2: Don't call this Method multiple times, each time specifying a certain PartnerKey! Instead, don't
        /// pass anything for the optional <paramref name="APartnerKey"/> Argument and use the Method
        /// TPrimaryContactAttributes.GetPrimaryEmailAddress for the retrieval of the individual Partners'
        /// 'Primary' and/or 'Within Organisation' setting(s)!</para></param>
        /// <param name="AOverallContSettingKind">Specify the kind of Overall Contact Setting(s) that you want returned.
        /// Combine multiple ones with the binary OR operator ( | ).</param>
        /// <returns>An instance of <see cref="TOverallContactSettings"/> that holds the
        /// <see cref="TPartnersOverallContactSettings"/> for each of the Partner(s) contained in
        /// <paramref name="APPartnerAttributeDT"/>. However, it returns null in case
        /// <paramref name="APPartnerAttributeDT"/> holds no records, or when it contains only one record but this records'
        /// Current flag is false, or this records' PartnerKey isn't the PartnerKey that got passed in with Argument
        /// <paramref name="APartnerKey"/>. It also returns null if no record was found that met what was asked for with
        /// <paramref name="AOverallContSettingKind"/>!</returns>
        public static TOverallContactSettings DeterminePrimaryOrWithinOrgSettings(
            PPartnerAttributeTable APPartnerAttributeDT, TOverallContSettingKind AOverallContSettingKind,
            Int64 APartnerKey = -1)
        {
            TOverallContactSettings OverallContactSettings;
            TPartnersOverallContactSettings MockPartnersOvrlCS = null;
            bool PrimaryIsAskedFor;
            bool WithinOrgIsAskedFor;
            bool PrimaryQuickExitConditionMet;
            bool WithinOrgQuickExitConditionMet;
            TOverallContSettingKind CheckFlags;

            //
            // Several 'quick checks' to save ourselves the creation of (a) DataView(s) it if isn't necessary!
            //
            if (!QuickChecksForPrimaryOrWithinOrgSettings(APPartnerAttributeDT, AOverallContSettingKind, APartnerKey,
                    out PrimaryIsAskedFor, out WithinOrgIsAskedFor,
                    out PrimaryQuickExitConditionMet, out WithinOrgQuickExitConditionMet))
            {
                return null;
            }

            OverallContactSettings = new TOverallContactSettings();

            //
            // None of the 'quick checks' returned a 'quick result': Create necessary DataView(s) and process what they find
            //

            // Check for Email Addresses
            CheckFlags = TOverallContSettingKind.ocskPrimaryEmailAddress |
                         TOverallContSettingKind.ocskEmailAddressWithinOrg;

            if ((AOverallContSettingKind & CheckFlags) != 0)
            {
                ParsePartnerOvrlCS(APartnerKey, APPartnerAttributeDT, ref OverallContactSettings, ref MockPartnersOvrlCS,
                    true, PrimaryIsAskedFor, WithinOrgIsAskedFor);
            }

            // Check for Phone Numbers
            CheckFlags = TOverallContSettingKind.ocskPrimaryPhoneNumber |
                         TOverallContSettingKind.ocskPhoneNumberWithinOrg;

            if ((AOverallContSettingKind & CheckFlags) != 0)
            {
                ParsePartnerOvrlCS(APartnerKey, APPartnerAttributeDT, ref OverallContactSettings, ref MockPartnersOvrlCS,
                    false, PrimaryIsAskedFor, WithinOrgIsAskedFor);
            }

            return OverallContactSettings;
        }

        private static bool QuickChecksForPrimaryOrWithinOrgSettings(PPartnerAttributeTable APPartnerAttributeDT,
            TOverallContSettingKind AOverallContSettingKind, long APartnerKey,
            out bool APrimaryIsAskedFor, out bool AWithinOrgIsAskedFor,
            out bool APrimaryQuickExitConditionMet, out bool AWithinOrgQuickExitConditionMet)
        {
            TOverallContSettingKind CheckFlags;

            APrimaryIsAskedFor = false;
            AWithinOrgIsAskedFor = false;
            APrimaryQuickExitConditionMet = false;
            AWithinOrgQuickExitConditionMet = false;

            if (APPartnerAttributeDT == null)
            {
                throw new ArgumentNullException("APPartnerAttributeDT", "APPartnerAttributeDT Argument must not be null");
            }

            /*
             *  Check that custom DataColumn PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN is a DataColumn of APartnerAttributeDT
             */
            if (!APPartnerAttributeDT.Columns.Contains(PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN))
            {
                throw new ArgumentException(String.Format(
                        "The DataTable passed in through the APartnerAttributeDT Argument must contain the DataColumn '{0}' " +
                        "(which gets added by Method 'DeterminePartnerContactDetailAttributes')!", PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN));
            }

            CheckFlags = TOverallContSettingKind.ocskPrimaryEmailAddress |
                         TOverallContSettingKind.ocskPrimaryPhoneNumber |
                         TOverallContSettingKind.ocskEmailAddressWithinOrg |
                         TOverallContSettingKind.ocskPhoneNumberWithinOrg;

            if ((AOverallContSettingKind & CheckFlags) == 0)
            {
                throw new ArgumentException(String.Format("Passed AOverallContSettingKind {0} is incompatible with this Method!",
                        AOverallContSettingKind));
            }

            if (APPartnerAttributeDT.Rows.Count == 0)
            {
                // No PPartnerAttribute Records at all = there can't possibly be a 'Overall Contact Setting' contained in them!
                return false;
            }

            CheckFlags = TOverallContSettingKind.ocskPrimaryEmailAddress |
                         TOverallContSettingKind.ocskPrimaryPhoneNumber;

            if ((AOverallContSettingKind & CheckFlags) != 0)
            {
                // A 'Primary Email' Address or a 'Primary Phone' is asked for
                APrimaryIsAskedFor = true;
            }

            CheckFlags = TOverallContSettingKind.ocskEmailAddressWithinOrg |
                         TOverallContSettingKind.ocskPhoneNumberWithinOrg;

            if ((AOverallContSettingKind & CheckFlags) != 0)
            {
                // An 'Email Within Organisation' or a 'Phone Within Organsiation' is asked for
                AWithinOrgIsAskedFor = true;
            }

            if (APPartnerAttributeDT.Rows.Count == 1)
            {
                //
                // Only one PPartnerAttribute Record:
                //

                if (((APPartnerAttributeDT[0][PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN] == System.DBNull.Value)
                     || ((bool)APPartnerAttributeDT[0][PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN] != true)))
                {
                    // If the one Record isn't a Partner Contact Detail record then it can't possibly hold an 'Overall Contact Setting'!
                    return false;
                }

                if (!APPartnerAttributeDT[0].Current)
                {
                    // If the one record isn't Current then it can't possibly hold an 'Overall Contact Setting'!
                    return false;
                }

                if ((APartnerKey != -1)
                    && (APPartnerAttributeDT[0].PartnerKey != APartnerKey))
                {
                    // If PartnerKey has been specified and this ones' Rows' PartnerKey isn't the PartnerKey specified,
                    // then it can't possibly hold any 'Primary' or 'Within Organsiation' settings!
                    return false;
                }

                if (APrimaryIsAskedFor)
                {
                    // A 'Primary Email' Address or a 'Primary Phone' is asked for
                    if (!APPartnerAttributeDT[0].Primary)
                    {
                        // If that one record it isn't Primary then it can't possibly be a 'Primary Email' Address or a
                        // 'Primary Phone'!
                        APrimaryQuickExitConditionMet = true;
                    }
                }
                else
                {
                    APrimaryQuickExitConditionMet = true;
                }

                if (AWithinOrgIsAskedFor)
                {
                    // An 'Email Within Organisation' or a 'Phone Within Organsiation' is asked for
                    if (!APPartnerAttributeDT[0].WithinOrganisation)
                    {
                        // If that one record it isn't WithinOrganisation then it can't possibly be an 'Email Within
                        // Organisation' or a 'Phone Within Organsiation'!
                        AWithinOrgQuickExitConditionMet = true;
                    }
                }
                else
                {
                    AWithinOrgQuickExitConditionMet = true;
                }

                // As the one record was Current, but didn't meet the requested 'Primary' nor 'Within Organisation'
                // requirements we can exit this Method here!
                if (APrimaryQuickExitConditionMet
                    && AWithinOrgQuickExitConditionMet)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines the 'System Category' setting(s) for the Partner whose PPartnerAttributes records
        /// are contained in <paramref name="APartnerAttributeDT"/>.
        /// </summary>
        /// <param name="APartnerAttributeDT"><see cref="PPartnerAttributeTable"/> that contains the Contact Detail
        /// records of a given Partner. The DataTable must have a special DataColumn added that Method
        /// 'DeterminePartnerContactDetailAttributes' adds, hence that Method must be called before calling this Method.</param>
        /// <param name="AOverallContactSettings">Pass in an instance of <see cref="TOverallContactSettings" />.</param>
        /// <param name="AOverallContSettingKind">Specify the kind of Overall Contact Setting(s) that you want returned.
        /// Combine multiple ones with the binary OR operator ( | ).</param>
        public static void DeterminePartnerSystemCategorySettings(PPartnerAttributeTable APartnerAttributeDT,
            ref TPartnersOverallContactSettings AOverallContactSettings,
            TOverallContSettingKind AOverallContSettingKind)
        {
            DataView ElegibleSystemCategoryAttributesDV = DeterminePartnerSystemCategoryAttributes(
                APartnerAttributeDT, TSharedDataCache.TMPartner.GetSystemCategorySettingsConcatStr());
            PPartnerAttributeRow PartnerAttributeDR;

            for (int Counter = 0; Counter < ElegibleSystemCategoryAttributesDV.Count; Counter++)
            {
                PartnerAttributeDR = (PPartnerAttributeRow)ElegibleSystemCategoryAttributesDV[Counter].Row;

                if (PartnerAttributeDR.AttributeType == ATTR_TYPE_PARTNERS_PRIMARY_CONTACT_METHOD)
                {
                    if ((AOverallContSettingKind & TOverallContSettingKind.ocskPrimaryContactMethod) ==
                        TOverallContSettingKind.ocskPrimaryContactMethod)
                    {
                        AOverallContactSettings.PrimaryContactMethod = PartnerAttributeDR.Value;
                    }
                }
            }
        }

        /// <summary>Parses the content of <paramref name="APartnerAttributeDT" /> and puts the parse results either
        /// into <paramref name="AOverallContactSettings" /> or into <paramref name="APartnersOvrlCS" /> - depending
        /// on which of the objects isn't null!
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner.</param>
        /// <param name="APartnerAttributeDT"><see cref="PPartnerAttributeTable"/> that contains the Contact Detail
        /// records of a given Partner - or of MANY (!) Partners. The DataTable must have a special DataColumn added that Method
        /// 'DeterminePartnerContactDetailAttributes' adds, hence that Method must be called before calling this Method.</param>
        /// <param name="AOverallContactSettings">Pass in an instance of <see cref="TOverallContactSettings" /> if
        /// you want the parsed content to be added to this object that holds this information for many Partners.</param>
        /// <param name="APartnersOvrlCS">Pass in an instance of <see cref="TPartnersOverallContactSettings" /> if
        /// you want the parsed content to be added to this object that holds this information for a single Partner.</param>
        /// <param name="AParseEmailAttributes">Set to true if Email Attributes should be parsed and to false if
        /// Phone Attributes should be parsed.</param>
        /// <param name="APrimaryIsAskedFor">Set to true if 'Primary' Attributes should be parsed.</param>
        /// <param name="AWithinOrgIsAskedFor">Set to true if 'Within Organisation' Attributes should be parsed.</param>
        /// <remarks>When this Method is called, either <paramref name="AOverallContactSettings" /> must be null OR
        /// <paramref name="APartnersOvrlCS" /> must be null. This Method is designed specifically to be called from either
        /// the 'DeterminePrimaryOrWithinOrgSettingsForPartner' or the 'DeterminePrimaryOrWithinOrgSettings' Method.
        /// If you want to call this Method from somewhere else, care must be taken in order for it to be doing the right thing!
        /// </remarks>
        private static void ParsePartnerOvrlCS(Int64 APartnerKey, PPartnerAttributeTable APartnerAttributeDT,
            ref TOverallContactSettings AOverallContactSettings, ref TPartnersOverallContactSettings APartnersOvrlCS,
            bool AParseEmailAttributes, bool APrimaryIsAskedFor,
            bool AWithinOrgIsAskedFor)
        {
            DataView PartnerAttributesDV;
            string PartnerAttributesConcatStr;
            string PartnerKeyCriteria;
            PPartnerAttributeRow PartnerAttributeDR;
            TPartnersOverallContactSettings PartnersOvrlCS = null;
            Int64 LastPartnerKey = -1;

            PartnerAttributesConcatStr = AParseEmailAttributes ? TSharedDataCache.TMPartner.GetEmailPartnerAttributesConcatStr() :
                                         TSharedDataCache.TMPartner.GetPhonePartnerAttributesConcatStr();

            // Check if a certain Partner should be picked out from the PPartnerAttribute Table (useful if it holds rows for many Partners)
            PartnerKeyCriteria = APartnerKey != -1 ? PPartnerAttributeTable.GetPartnerKeyDBName() + " = " +
                                 APartnerKey.ToString() + " AND " : String.Empty;

            PartnerAttributesDV = new DataView(APartnerAttributeDT,
                PartnerKeyCriteria +
                PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN + " = true AND " +
                PPartnerAttributeTable.GetCurrentDBName() + " = true AND (" +
                PPartnerAttributeTable.GetPrimaryDBName() + " = true OR " +
                PPartnerAttributeTable.GetWithinOrganisationDBName() + " = true) AND " +
                String.Format(PPartnerAttributeTable.GetAttributeTypeDBName() + " IN ({0})",
                    PartnerAttributesConcatStr),
                PPartnerAttributeTable.GetPartnerKeyDBName() + " ASC", DataViewRowState.CurrentRows);

            if (PartnerAttributesDV.Count > 0)
            {
                if (PartnerAttributesDV.Count == 1)
                {
                    // DataView contains only one DataRow, so we can just pick the DataRow with
                    // Index 0 and thus save ourselves the iteration over the DataView below.
                    PartnerAttributeDR = ((PPartnerAttributeRow)PartnerAttributesDV[0].Row);
                    PartnersOvrlCS = APartnersOvrlCS ?? GetNewOrExistingPartnersOvrlCS(PartnerAttributeDR.PartnerKey,
                        AOverallContactSettings);

                    AssignValueToPartnersOvrlContSettgs(PartnerAttributeDR, APrimaryIsAskedFor, AWithinOrgIsAskedFor,
                        AParseEmailAttributes, ref PartnersOvrlCS);

                    if (AOverallContactSettings != null)
                    {
                        AOverallContactSettings[PartnerAttributeDR.PartnerKey] = PartnersOvrlCS;
                    }
                }
                else
                {
                    for (int Counter = 0; Counter < PartnerAttributesDV.Count; Counter++)
                    {
                        PartnerAttributeDR = (PPartnerAttributeRow)PartnerAttributesDV[Counter].Row;

                        if (PartnerAttributeDR.PartnerKey != LastPartnerKey)
                        {
                            if ((AOverallContactSettings != null)
                                && (LastPartnerKey != -1))
                            {
                                // Add the record from the previous iteration
                                AOverallContactSettings[LastPartnerKey] = PartnersOvrlCS;
                            }

                            PartnersOvrlCS = APartnersOvrlCS ?? GetNewOrExistingPartnersOvrlCS(PartnerAttributeDR.PartnerKey,
                                AOverallContactSettings);
                        }

                        AssignValueToPartnersOvrlContSettgs(PartnerAttributeDR, APrimaryIsAskedFor, AWithinOrgIsAskedFor,
                            AParseEmailAttributes, ref PartnersOvrlCS);

                        LastPartnerKey = PartnerAttributeDR.PartnerKey;
                    }

                    if (AOverallContactSettings != null)
                    {
                        // Add the record from the last iteration, too
                        AOverallContactSettings[LastPartnerKey] = PartnersOvrlCS;
                    }
                }
            }
        }

        private static TPartnersOverallContactSettings GetNewOrExistingPartnersOvrlCS(Int64 APartnerKey, TOverallContactSettings AOvrlCS)
        {
            TPartnersOverallContactSettings PartnersOvrlCS;

            if (!AOvrlCS.TryGetValue(APartnerKey, out PartnersOvrlCS))
            {
                PartnersOvrlCS = new TPartnersOverallContactSettings();
            }

            return PartnersOvrlCS;
        }

        private static void AssignValueToPartnersOvrlContSettgs(PPartnerAttributeRow APartnerAttributeDR,
            bool APrimaryIsAskedFor, bool AWithinOrgIsAskedFor, bool AAssignEmailAttributes,
            ref TPartnersOverallContactSettings APartnersOvrlCS)
        {
            if (APrimaryIsAskedFor
                && APartnerAttributeDR.Primary)
            {
                if (AAssignEmailAttributes)
                {
                    APartnersOvrlCS.PrimaryEmailAddress = APartnerAttributeDR.Value;
                }
                else
                {
                    APartnersOvrlCS.PrimaryPhoneNumber = APartnerAttributeDR.Value;
                }
            }

            if (AWithinOrgIsAskedFor
                && APartnerAttributeDR.WithinOrganisation)
            {
                if (AAssignEmailAttributes)
                {
                    APartnersOvrlCS.EmailAddressWithinOrg = APartnerAttributeDR.Value;
                }
                else
                {
                    APartnersOvrlCS.PhoneNumberWithinOrg = APartnerAttributeDR.Value;
                }
            }
        }

        #endregion

        /// <summary>
        /// Determines the Partner Attribute Types that represent Partner Contact Types.
        /// </summary>
        /// <param name="ACacheRetriever">Delegate that returns the a DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MPartner Cache (that is, it needs to work with the <see cref="TCacheablePartnerTablesEnum" /> Enum!</param>
        /// <returns>DataTable containing only Partner Attribute Types that represent Partner Contact Types.</returns>
        public static PPartnerAttributeTypeTable DeterminePartnerContactTypes(TGetCacheableDataTableFromCache ACacheRetriever)
        {
            var ReturnValue = new PPartnerAttributeTypeTable();
            Type tmp;

            ReturnValue.Merge(ACacheRetriever(Enum.GetName(typeof(TCacheablePartnerTablesEnum),
                        TCacheablePartnerTablesEnum.ContactTypeList), out tmp));

            return ReturnValue;
        }

        /// <summary>
        /// Determines the Partner Attribute Types that represent System Categories.
        /// </summary>
        /// <param name="ACacheRetriever">Delegate that returns the a DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MPartner Cache (that is, it needs to work with the <see cref="TCacheablePartnerTablesEnum" /> Enum!</param>
        /// <returns>DataTable containing only Partner Attribute Types that represent System Categories.</returns>
        public static PPartnerAttributeTypeTable DetermineSystemCategoryTypes(TGetCacheableDataTableFromCache ACacheRetriever)
        {
            var ReturnValue = new PPartnerAttributeTypeTable();
            Type tmp;

            ReturnValue.Merge(ACacheRetriever(Enum.GetName(typeof(TCacheablePartnerTablesEnum),
                        TCacheablePartnerTablesEnum.PartnerAttributeSystemCategoryTypeList), out tmp));

            return ReturnValue;
        }

        /// <summary>
        /// Checks if <paramref name="APPartnerAttributeTypeDT"/> isn't null and if so, if it contains records.
        /// If it is null, <paramref name="ACacheRetriever"/> is used to populate
        /// <paramref name="APPartnerAttributeTypeDT"/> from a Cacheable DataTable.
        /// </summary>
        /// <param name="APPartnerAttributeTypeDT">A populated DataTable, or null. In the latter case,
        /// <paramref name="ACacheRetriever"/> must not be null!</param>
        /// <param name="ACacheRetriever">Delegate that returns the a DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MPartner Cache (that is, it needs to work with the <see cref="TCacheablePartnerTablesEnum" /> Enum!</param>
        /// <param name="APartnerContactTypes">Only relevant if <paramref name="APPartnerAttributeTypeDT"/> is null:
        /// in that case it controls which Cacheable DataTable is fetched from the data cache.</param>
        private static void GetPPartnerAttributeTable(ref PPartnerAttributeTypeTable APPartnerAttributeTypeDT,
            TGetCacheableDataTableFromCache ACacheRetriever, bool APartnerContactTypes)
        {
            if (APPartnerAttributeTypeDT == null)
            {
                if (ACacheRetriever == null)
                {
                    throw new ArgumentNullException("ACacheRetriever", "ACacheRetriever must not be null if APPartnerAttributeTypeDT is null");
                }

                if (APartnerContactTypes)
                {
                    // As APPartnerAttributeTypeDT is null we add DataRows from the corresponding Cacheable DataTable
                    APPartnerAttributeTypeDT = DeterminePartnerContactTypes(ACacheRetriever);
                }
                else
                {
                    // As APPartnerAttributeTypeDT is null we add DataRows from the corresponding Cacheable DataTable
                    APPartnerAttributeTypeDT = DetermineSystemCategoryTypes(ACacheRetriever);
                }

                if (APPartnerAttributeTypeDT.Count == 0)
                {
                    throw new EOPAppException(
                        "The DataTable passed in APPartnerAttributeTypeDT was null, so it got populated via the TDataCache. However, the returned Cacheable DataTable contains no records; for this Method to work that DataTable must hold records!");
                }
            }
            else
            {
                if (APPartnerAttributeTypeDT.Count == 0)
                {
                    throw new EOPAppException(
                        "The DataTable passed in APPartnerAttributeTypeDT contains no records; for this Method to work that DataTable must hold records!");
                }
            }
        }

        /// <summary>
        /// Determines which p_partner_attribute_type records have p_category_code_c 'Phone'
        /// </summary>
        /// <param name="APPartnerAttributeTypeDT">Either pass an instance of <see cref="PPartnerAttributeTypeTable"/>
        /// that holds Contact Detail datarows, or pass null. In the latter case, <paramref name="ACacheRetriever"/> must not be null!</param>
        /// <param name="ACacheRetriever">If <paramref name="APPartnerAttributeTypeDT"/> is null you must set this to an
        /// instance of a Delegate that returns that DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MPartner Cache (that is, it needs to work with the
        /// <see cref="TCacheablePartnerTablesEnum" /> Enum!</param>
        /// <returns>DataView that is filtered so that it contains only p_partner_attribute_type records that have
        /// p_category_code_c 'Phone'.</returns>
        public static DataView DeterminePhoneAttributes(PPartnerAttributeTypeTable APPartnerAttributeTypeDT = null,
            TGetCacheableDataTableFromCache ACacheRetriever = null)
        {
            GetPPartnerAttributeTable(ref APPartnerAttributeTypeDT, ACacheRetriever, true);

            return new DataView(APPartnerAttributeTypeDT,
                String.Format(PPartnerAttributeTypeTable.GetCategoryCodeDBName() + " = '{0}' AND " +
                    PPartnerAttributeTypeTable.GetUnassignableDBName() + " = false",
                    "Phone"),
                "", DataViewRowState.CurrentRows);
        }

        /// <summary>
        /// Determines all p_partner_attribute records whose p_attribute_type points to a p_partner_attribute_type record
        /// that has p_category_code_c 'Phone' and returns the result.
        /// </summary>
        /// <param name="APPartnerAttributeDT"><see cref="PPartnerAttributeTable"/> that contains the Contact Detail
        /// records of a given Partner - or of MANY (!) Partners.</param>
        /// <param name="AOnlyCurrentPhoneNumbers">Set to true to only return 'Valid' p_partner_attribute records
        /// (i.e. p_partner_attribute records whose p_current_l Flag is set to true).</param>
        /// <param name="AIncludeFaxNumbers">Set to false to exclude p_partner_attribute records whose
        /// p_attribute_type_c is 'Fax'.</param>
        /// <returns>DataView that is filtered so that it contains only p_partner_attribute records whose p_attribute_type
        /// points to a p_partner_attribute_type record that has p_category_code_c 'Phone'.</returns>
        public static DataView DeterminePartnerPhoneNumbers(PPartnerAttributeTable APPartnerAttributeDT,
            bool AOnlyCurrentPhoneNumbers, bool AIncludeFaxNumbers)
        {
            string CurrentCriteria;
            string NonFaxCriteria;
            string PhoneAttributesConcatStr;

            if (APPartnerAttributeDT == null)
            {
                throw new ArgumentNullException("APPartnerAttributeDT", "APPartnerAttributeDT must not be null");
            }

            PhoneAttributesConcatStr = TSharedDataCache.TMPartner.GetPhonePartnerAttributesConcatStr();

            if (PhoneAttributesConcatStr.Length > 0)
            {
                CurrentCriteria = AOnlyCurrentPhoneNumbers ? PPartnerAttributeTable.GetCurrentDBName() + " = true AND " : String.Empty;
                NonFaxCriteria = AIncludeFaxNumbers ? String.Empty : PPartnerAttributeTable.GetAttributeTypeDBName() + " <> 'Fax' AND ";

                return new DataView(APPartnerAttributeDT,
                    CurrentCriteria +
                    NonFaxCriteria +
                    String.Format(PPartnerAttributeTable.GetAttributeTypeDBName() + " IN ({0})",
                        PhoneAttributesConcatStr),
                    PPartnerAttributeTable.GetIndexDBName() + " ASC", DataViewRowState.CurrentRows);
            }
            else
            {
                return new DataView();
            }
        }

        /// <summary>
        /// Determines the (first) current 'Fax Number' that is contained in the p_partner_attribute records of a given Partner.
        /// </summary>
        /// <param name="APPartnerAttributeDT"><see cref="PPartnerAttributeTable"/> that contains the Contact Detail
        /// records of a given Partner.</param>
        /// <returns>The (first) current 'Fax Number' if it was found, otherwise null. Should there be several current
        /// 'Fax Numbers' then the one that comes first (in the order as seen by the user) will get returned.</returns>
        public static string DeterminePartnerFaxNumber(PPartnerAttributeTable APPartnerAttributeDT)
        {
            string PhoneAttributesConcatStr;

            if (APPartnerAttributeDT == null)
            {
                throw new ArgumentNullException("APPartnerAttributeDT", "APPartnerAttributeDT must not be null");
            }

            //
            // Several 'quick checks' to save ourselves the creation of a DataView it if isn't necessary!
            //
            if (APPartnerAttributeDT.Rows.Count == 0)
            {
                return null;
            }

            if (APPartnerAttributeDT.Rows.Count == 1)
            {
                //
                // Only one PPartnerAttribute Record:
                //

                if (((APPartnerAttributeDT[0][PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN] == System.DBNull.Value)
                     || ((bool)APPartnerAttributeDT[0][PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN] != true)))
                {
                    // If the one Record isn't a Partner Contact Detail record then it can't possibly hold a 'Fax Number'!
                    return null;
                }

                if ((!APPartnerAttributeDT[0].Current)
                    || (APPartnerAttributeDT[0].Primary)
                    || (!APPartnerAttributeDT[0].IsWithinOrganisationNull() && APPartnerAttributeDT[0].WithinOrganisation))
                {
                    // If the one record isn't Current then it can't possibly hold a 'Fax Number',
                    // also not if if is 'Primary' (as there are no 'Primary' Fax Numbers)
                    // and also not if if is 'WithinOrganisation' (as there are no 'WithinOrganisation' Fax Numbers)!
                    return null;
                }
            }

            PhoneAttributesConcatStr = TSharedDataCache.TMPartner.GetPhonePartnerAttributesConcatStr();

            if (PhoneAttributesConcatStr.Length > 0)
            {
                var FaxDV = new DataView(APPartnerAttributeDT,
                    PPartnerAttributeTable.GetCurrentDBName() + " = true AND " +
                    PPartnerAttributeTable.GetAttributeTypeDBName() + " = 'Fax' AND " +
                    String.Format(PPartnerAttributeTable.GetAttributeTypeDBName() + " IN ({0})",
                        PhoneAttributesConcatStr),
                    PPartnerAttributeTable.GetIndexDBName() + " ASC", DataViewRowState.CurrentRows);

                // If no current 'Fax Numbers' were found we return null; otherwise we simply return the first current
                // 'Fax Number' we find; should there be several current 'Fax Numbers' then the one that comes first
                // (in the order as seen by the user) gets returned.
                return FaxDV.Count == 0 ? null : ((PPartnerAttributeRow)FaxDV[0].Row).Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Determines all p_partner_attribute_type records that have p_category_code_c 'Phone' and returns the result.
        /// </summary>
        /// <param name="APPartnerAttributeTypeDT">Either pass an instance of <see cref="PPartnerAttributeTypeTable"/>
        /// that holds datarows *that constitute Contact Details*, or pass null. In the latter case, <paramref name="ACacheRetriever"/> must not be null!</param>
        /// <param name="ACacheRetriever">If <paramref name="APPartnerAttributeTypeDT"/> is null you must set this to an
        /// instance of a Delegate that returns that DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MPartner Cache (that is, it needs to work with the
        /// <see cref="TCacheablePartnerTablesEnum" /> Enum!</param>
        /// <remarks><em>PERFORMANCE NOTE</em>: The return value of this Method is CACHED in the Property
        /// TSharedDataChache.TMPartner.GetPhonePartnerAttributesConcatStr! Use that Property rather than
        /// calling this Method!!!</remarks>
        /// <returns>String that contains all p_partner_attribute_type records that have p_category_code_c 'Phone'.
        /// </returns>
        public static string DeterminePhonePartnerAttributeTypes(PPartnerAttributeTypeTable APPartnerAttributeTypeDT = null,
            TGetCacheableDataTableFromCache ACacheRetriever = null)
        {
            string ReturnValue = String.Empty;
            string PhoneAttributesConcatStr = String.Empty;

            DataView PhoneAttributesDV = DeterminePhoneAttributes(APPartnerAttributeTypeDT, ACacheRetriever);

            for (int Counter = 0; Counter < PhoneAttributesDV.Count; Counter++)
            {
                PhoneAttributesConcatStr += ((PPartnerAttributeTypeRow)PhoneAttributesDV[Counter].Row).AttributeType + "', '";
            }

            if (PhoneAttributesConcatStr.Length > 0)
            {
                ReturnValue = "'" + PhoneAttributesConcatStr.Substring(0, PhoneAttributesConcatStr.Length - 3);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Determines which p_partner_attribute_type records are of p_attribute_type_value_kind_c 'CONTACTDETAIL_EMAILADDRESS'
        /// </summary>
        /// <param name="APPartnerAttributeTypeDT">Either pass an instance of <see cref="PPartnerAttributeTypeTable"/>
        /// that holds Contact Detail datarows, or pass null. In the latter case, <paramref name="ACacheRetriever"/> must not be null!</param>
        /// <param name="ACacheRetriever">If <paramref name="APPartnerAttributeTypeDT"/> is null you must set this to an
        /// instance of a Delegate that returns that DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MPartner Cache (that is, it needs to work with the
        /// <see cref="TCacheablePartnerTablesEnum" /> Enum!</param>
        /// <returns>DataView that is filtered so that it contains only p_partner_attribute_type records are of
        /// p_attribute_type_value_kind_c 'CONTACTDETAIL_EMAILADDRESS'.</returns>
        public static DataView DetermineEmailAttributes(PPartnerAttributeTypeTable APPartnerAttributeTypeDT = null,
            TGetCacheableDataTableFromCache ACacheRetriever = null)
        {
            GetPPartnerAttributeTable(ref APPartnerAttributeTypeDT, ACacheRetriever, true);

            return new DataView(APPartnerAttributeTypeDT,
                String.Format(PPartnerAttributeTypeTable.GetAttributeTypeValueKindDBName() + " = '{0}' AND " +
                    PPartnerAttributeTypeTable.GetUnassignableDBName() + " = false",
                    TPartnerAttributeTypeValueKind.CONTACTDETAIL_EMAILADDRESS),
                "", DataViewRowState.CurrentRows);
        }

        /// <summary>
        /// Determines all p_partner_attribute_type records that are of p_attribute_type_value_kind_c 'CONTACTDETAIL_EMAILADDRESS' and
        /// returns the result.
        /// </summary>
        /// <param name="APPartnerAttributeTypeDT">Either pass an instance of <see cref="PPartnerAttributeTypeTable"/>
        /// that holds datarows *that constitute Contact Details*, or pass null. In the latter case, <paramref name="ACacheRetriever"/> must not be null!</param>
        /// <param name="ACacheRetriever">If <paramref name="APPartnerAttributeTypeDT"/> is null you must set this to an
        /// instance of a Delegate that returns that DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MPartner Cache (that is, it needs to work with the
        /// <see cref="TCacheablePartnerTablesEnum" /> Enum!</param>
        /// <remarks><em>PERFORMANCE NOTE</em>: The return value of this Method is CACHED in the Property
        /// TSharedDataChache.TMPartner.GetEmailPartnerAttributesConcatStr! Use that Property rather than
        /// calling this Method!!!</remarks>
        /// <returns>String that contains all p_partner_attribute_type records are of p_attribute_type_value_kind_c
        /// 'CONTACTDETAIL_EMAILADDRESS'.</returns>
        public static string DetermineEmailPartnerAttributeTypes(PPartnerAttributeTypeTable APPartnerAttributeTypeDT = null,
            TGetCacheableDataTableFromCache ACacheRetriever = null)
        {
            string ReturnValue = String.Empty;
            string EmailAttributesConcatStr = String.Empty;

            DataView EmailAttributesDV = DetermineEmailAttributes(APPartnerAttributeTypeDT, ACacheRetriever);

            for (int Counter = 0; Counter < EmailAttributesDV.Count; Counter++)
            {
                EmailAttributesConcatStr += ((PPartnerAttributeTypeRow)EmailAttributesDV[Counter].Row).AttributeType + "', '";
            }

            if (EmailAttributesConcatStr.Length > 0)
            {
                ReturnValue = "'" + EmailAttributesConcatStr.Substring(0, EmailAttributesConcatStr.Length - 3);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Determines all p_partner_attribute records whose p_attribute_type points to a p_partner_attribute_type record
        /// that is of p_attribute_type_value_kind_c 'CONTACTDETAIL_EMAILADDRESS' and returns the result.
        /// </summary>
        /// <param name="APPartnerAttributeDT"><see cref="PPartnerAttributeTable"/> that contains the Contact Detail
        /// records of a given Partner - or of MANY (!) Partners.</param>
        /// <param name="AOnlyCurrentEmailAddresses">Set to true to only return 'Valid' p_partner_attribute records
        /// (i.e. p_partner_attribute records whose p_current_l Flag is set to true).</param>
        /// <returns>DataView that is filtered so that it contains only p_partner_attribute records whose p_attribute_type
        /// points to a p_partner_attribute_type record that is of p_attribute_type_value_kind_c 'CONTACTDETAIL_EMAILADDRESS'.</returns>
        public static DataView DeterminePartnerEmailAddresses(PPartnerAttributeTable APPartnerAttributeDT,
            bool AOnlyCurrentEmailAddresses)
        {
            string CurrentCriteria;
            string EmailAttributesConcatStr;

            if (APPartnerAttributeDT == null)
            {
                throw new ArgumentNullException("APPartnerAttributeDT", "APPartnerAttributeDT must not be null");
            }

            EmailAttributesConcatStr = TSharedDataCache.TMPartner.GetEmailPartnerAttributesConcatStr();

            if (EmailAttributesConcatStr.Length > 0)
            {
                CurrentCriteria = AOnlyCurrentEmailAddresses ? PPartnerAttributeTable.GetCurrentDBName() + " = true AND " : String.Empty;

                return new DataView(APPartnerAttributeDT,
                    CurrentCriteria +
                    String.Format(PPartnerAttributeTable.GetAttributeTypeDBName() + " IN ({0})",
                        EmailAttributesConcatStr),
                    PPartnerAttributeTable.GetIndexDBName() + " ASC", DataViewRowState.CurrentRows);
            }
            else
            {
                return new DataView();
            }
        }

        /// <summary>
        /// Determines which p_partner_attribute_type records constitute Partner Contact Details.
        /// </summary>
        /// <param name="APPartnerAttributeTypeDT">Either pass an instance of <see cref="PPartnerAttributeTypeTable"/>
        /// that holds datarows *that are part of a System Category*, or pass null. In the latter case,
        /// <paramref name="ACacheRetriever"/> must not be null!</param>
        /// <param name="ACacheRetriever">If <paramref name="APPartnerAttributeTypeDT"/> is null you must set this to an
        /// instance of a Delegate that returns that DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MPartner Cache (that is, it needs to work with the
        /// <see cref="TCacheablePartnerTablesEnum" /> Enum!</param>
        /// <remarks><em>PERFORMANCE NOTE</em>: The return value of this Method is CACHED in the Property
        /// TSharedDataChache.TMPartner.GetPartnerContactDetailAttributeTypesConcatStr! Use that Property rather than
        /// calling this Method!!!</remarks>
        /// <returns>String that contains all p_partner_attribute_type records which are part of a System Category.</returns>
        public static string DeterminePartnerContactDetailAttributeTypes(PPartnerAttributeTypeTable APPartnerAttributeTypeDT = null,
            TGetCacheableDataTableFromCache ACacheRetriever = null)
        {
            string ReturnValue = String.Empty;

            GetPPartnerAttributeTable(ref APPartnerAttributeTypeDT, ACacheRetriever, true);

            for (int Counter = 0; Counter < APPartnerAttributeTypeDT.Count; Counter++)
            {
                ReturnValue += APPartnerAttributeTypeDT[Counter].AttributeType + "', '";
            }

            if (ReturnValue.Length > 0)
            {
                ReturnValue = "'" + ReturnValue.Substring(0, ReturnValue.Length - 3);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Determines which p_partner_attribute_type records are part of a System Category.
        /// </summary>
        /// <param name="APPartnerAttributeTypeDT">Either pass an instance of <see cref="PPartnerAttributeTypeTable"/>
        /// that holds datarows *that are part of a System Category*, or pass null. In the latter case,
        /// <paramref name="ACacheRetriever"/> must not be null!</param>
        /// <param name="ACacheRetriever">If <paramref name="APPartnerAttributeTypeDT"/> is null you must set this to an
        /// instance of a Delegate that returns that DataTable from the data cache (client- or serverside).
        /// Delegate Method needs to be for the MPartner Cache (that is, it needs to work with the
        /// <see cref="TCacheablePartnerTablesEnum" /> Enum!</param>
        /// <remarks><em>PERFORMANCE NOTE</em>: The return value of this Method is CACHED in the Property
        /// TSharedDataChache.TMPartner.GetSystemCategorySettingsConcatStr! Use that Property rather than
        /// calling this Method!!!</remarks>
        /// <returns>String that contains all p_partner_attribute_type records which are part of a System Category.</returns>
        public static string DetermineSystemCategoryAttributeTypes(PPartnerAttributeTypeTable APPartnerAttributeTypeDT = null,
            TGetCacheableDataTableFromCache ACacheRetriever = null)
        {
            string ReturnValue = String.Empty;

            GetPPartnerAttributeTable(ref APPartnerAttributeTypeDT, ACacheRetriever, false);

            for (int Counter = 0; Counter < APPartnerAttributeTypeDT.Count; Counter++)
            {
                ReturnValue += APPartnerAttributeTypeDT[Counter].AttributeType + "', '";
            }

            if (ReturnValue.Length > 0)
            {
                ReturnValue = "'" + ReturnValue.Substring(0, ReturnValue.Length - 3);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Determines all p_partner_attribute records whose p_attribute_type points to any p_partner_attribute_type record
        /// that is part of a System Category.
        /// </summary>
        /// <param name="APPartnerAttributeDT"><see cref="PPartnerAttributeTable"/> that contains the p_partner_attribute
        /// records of a given Partner - or of MANY (!) Partners.</param>
        /// <param name="ASystemCategoriesConcatStr">This needs to be the return value of a call to Method
        /// <see cref="DetermineSystemCategoryAttributeTypes"/>.</param>
        /// <returns>DataView that is filtered so that it contains only p_partner_attribute records whose p_attribute_type points
        /// to any p_partner_attribute_type record that is part of a System Category.</returns>
        public static DataView DeterminePartnerSystemCategoryAttributes(PPartnerAttributeTable APPartnerAttributeDT,
            string ASystemCategoriesConcatStr)
        {
            if (APPartnerAttributeDT == null)
            {
                throw new ArgumentNullException("APPartnerAttributeDT", "APPartnerAttributeDT must not be null");
            }

            if (ASystemCategoriesConcatStr == null)
            {
                throw new ArgumentNullException("ASystemCategoriesConcatStr", "ASystemCategoriesConcatStr must not be null");
            }

            if (ASystemCategoriesConcatStr.Length > 0)
            {
                return new DataView(APPartnerAttributeDT,
                    String.Format(PPartnerAttributeTable.GetAttributeTypeDBName() + " IN ({0})",
                        ASystemCategoriesConcatStr),
                    PPartnerAttributeTable.GetIndexDBName() + " ASC", DataViewRowState.CurrentRows);
            }
            else
            {
                return new DataView();
            }
        }

        /// <summary>
        /// Determines which Partner Attributes *are* and which *aren't* Partner Contact Attributes, and marks them in the
        /// DataColumn 'PartnerContactDetail':
        /// Partner Attributes whose AttributeType is not a PartnerAttributeType whose PartnerAttributeCategory is a
        /// Partner Contact one PPartnerAttributeCategory.PartnerContactCategory Column holds 'false') aren't!
        /// </summary>
        /// <param name="APPartnerAttributeDT">Partner Attribute Table in which the Partner Contact Attributes
        /// should be determined.</param>
        /// <returns>Number of non-Partner Contact Attributes.</returns>
        public static int DeterminePartnerContactDetailAttributes(PPartnerAttributeTable APPartnerAttributeDT)
        {
            string PartnerContactDetailAttributesConcatStr;
            DataView PartnerAttributeDV;

            /*
             *  Add custom DataColumn if its not part of the DataTable yet
             */
            if (!APPartnerAttributeDT.Columns.Contains(PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN))
            {
                APPartnerAttributeDT.Columns.Add(new System.Data.DataColumn(PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN, typeof(Boolean)));
            }

            PartnerContactDetailAttributesConcatStr = TSharedDataCache.TMPartner.GetPartnerContactDetailAttributeTypesConcatStr();

            PartnerAttributeDV = new DataView(APPartnerAttributeDT,
                String.Format(PPartnerAttributeTable.GetAttributeTypeDBName() + " IN ({0})",
                    PartnerContactDetailAttributesConcatStr),
                "", DataViewRowState.CurrentRows);

            for (int Counter = 0; Counter < PartnerAttributeDV.Count; Counter++)
            {
                // Mark every Partner Attribute that *is* a Partner Contact Attribute
                PartnerAttributeDV[Counter].Row[PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN] = true;
            }

            return APPartnerAttributeDT.Rows.Count - PartnerAttributeDV.Count;;
        }

        /// <summary>
        /// Creates custom DataColumns that will be added to an instance of a PPartnerAttribute Table.
        /// </summary>
        /// <returns>void</returns>
        public static void CreateCustomDataColumnsForAttributeTable(PPartnerAttributeTable APPartnerAttributeDT,
            PPartnerAttributeTypeTable APPartnerAttributeTypeDT)
        {
            DataColumn ForeignTableColumn;

            if (!APPartnerAttributeDT.Columns.Contains(CALCCOLUMNNAME_PARTNERATTRIBUTETYPE))
            {
                ForeignTableColumn = new DataColumn();
                ForeignTableColumn.DataType = System.Type.GetType("System.String");
                ForeignTableColumn.ColumnName = CALCCOLUMNNAME_PARTNERATTRIBUTETYPE;
                ForeignTableColumn.Expression = "";  // The real expression will be set in Method 'SetColumnExpressions'!
                APPartnerAttributeDT.Columns.Add(ForeignTableColumn);
            }

            if (!APPartnerAttributeDT.Columns.Contains(CALCCOLUMNNAME_PARENTATTRIBUTEINDEX))
            {
                ForeignTableColumn = new DataColumn();
                ForeignTableColumn.DataType = System.Type.GetType("System.Int32");
                ForeignTableColumn.ColumnName = CALCCOLUMNNAME_PARENTATTRIBUTEINDEX;
                ForeignTableColumn.Expression = "";  // The real expression will be set in Method 'SetColumnExpressions'!
                APPartnerAttributeDT.Columns.Add(ForeignTableColumn);
            }

            if (!APPartnerAttributeDT.Columns.Contains(CALCCOLUMNNAME_PARENTPARENTCATEGORYINDEX))
            {
                ForeignTableColumn = new DataColumn();
                ForeignTableColumn.DataType = System.Type.GetType("System.Int32");
                ForeignTableColumn.ColumnName = CALCCOLUMNNAME_PARENTPARENTCATEGORYINDEX;
                ForeignTableColumn.Expression = "";  // The real expression will be set in Method 'SetColumnExpressions'!
                APPartnerAttributeDT.Columns.Add(ForeignTableColumn);
            }

            if (!APPartnerAttributeDT.Columns.Contains(CALCCOLUMNNAME_CONTACTTYPE))
            {
                ForeignTableColumn = new DataColumn();
                ForeignTableColumn.DataType = System.Type.GetType("System.String");
                ForeignTableColumn.ColumnName = CALCCOLUMNNAME_CONTACTTYPE;
                ForeignTableColumn.Expression = "";  // The real expression will be set in Method 'SetColumnExpressions'!
                APPartnerAttributeDT.Columns.Add(ForeignTableColumn);
            }

            if (!APPartnerAttributeTypeDT.Columns.Contains(CALCCOLUMNNAME_CATEGORYINDEX))
            {
                ForeignTableColumn = new DataColumn();
                ForeignTableColumn.DataType = System.Type.GetType("System.Int32");
                ForeignTableColumn.ColumnName = CALCCOLUMNNAME_CATEGORYINDEX;
                ForeignTableColumn.Expression = "Parent." + PPartnerAttributeCategoryTable.GetIndexDBName();
                APPartnerAttributeTypeDT.Columns.Add(ForeignTableColumn);
            }

            SetColumnExpressions(APPartnerAttributeDT);
        }

        /// <summary>
        /// Sets the Column Expressions for the calculated DataColumns
        /// </summary>
        public static void SetColumnExpressions(PPartnerAttributeTable APPartnerAttributeDT)
        {
            APPartnerAttributeDT.Columns[CALCCOLUMNNAME_PARTNERATTRIBUTETYPE].Expression =
                "Parent." + PPartnerAttributeTypeTable.GetAttributeTypeDBName();

            APPartnerAttributeDT.Columns[CALCCOLUMNNAME_PARENTATTRIBUTEINDEX].Expression =
                "Parent." + PPartnerAttributeTypeTable.GetIndexDBName();

            APPartnerAttributeDT.Columns[CALCCOLUMNNAME_PARENTPARENTCATEGORYINDEX].Expression =
                "Parent.CategoryIndex";

            APPartnerAttributeDT.Columns[CALCCOLUMNNAME_CONTACTTYPE].Expression =
                "IIF(" + PPartnerAttributeTable.GetSpecialisedDBName() + " = true, ISNULL(Parent." +
                PPartnerAttributeTypeTable.GetSpecialLabelDBName() + ", Parent." + PPartnerAttributeTypeTable.GetAttributeTypeDBName() +
                "), Parent." +
                PPartnerAttributeTypeTable.GetAttributeTypeDBName() + ")";
        }

        /// <summary>
        /// Constructs a valid URL string from a Value that is of a Contact Type that has got a Hyperlink Format set up.
        /// </summary>
        /// <param name="AValue">Value that should replace
        /// <see cref="THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER"/> in the Hyperlink Format string.</param>
        /// <param name="AValueKind">Value Kind of Partner Attribute Type. MUST be
        /// <see cref="TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK_WITHVALUE"/>!</param>
        /// <param name="AHyperlinkFormat">Format string for building the URL in which the placeholder gets replaced with <paramref name="AValue"/>.</param>
        /// <returns>URL with the Value replacing THyperLinkHandling.HYPERLINK_WITH_VALUE_VALUE_PLACEHOLDER_IDENTIFIER.</returns>
        public static string BuildLinkWithValue(string AValue, TPartnerAttributeTypeValueKind AValueKind,
            string AHyperlinkFormat)
        {
            string HyperlinkFormat = AHyperlinkFormat;
            string ReturnValue = String.Empty;

            if (AValueKind == TPartnerAttributeTypeValueKind.CONTACTDETAIL_HYPERLINK_WITHVALUE)
            {
                if ((HyperlinkFormat != null)
                    && (HyperlinkFormat != String.Empty))
                {
                    if ((HyperlinkFormat.Contains("{"))
                        && HyperlinkFormat.Contains("}"))
                    {
                        ReturnValue = HyperlinkFormat.Substring(0, HyperlinkFormat.IndexOf('{')) +
                                      AValue;
                        ReturnValue += HyperlinkFormat.Substring(HyperlinkFormat.LastIndexOf('}') + 1);
                    }
                    else
                    {
                        throw new EProblemConstructingHyperlinkException(
                            "The Value should be used to construct a hyperlink-with-value-replacement but the HyperlinkFormat is not correct (it needs to contain both the '{' and '}' characters)");
                    }
                }
                else
                {
                    throw new EOPAppException(
                        "The Value should be used to construct a hyperlink-with-value-replacement but the HyperlinkFormat of the Contact Type is not specified");
                }
            }
            else
            {
                throw new EOPAppException(
                    "The Value should be used to construct a hyperlink-with-value-replacement but the LinkType of the Value Control is not 'TLinkTypes.Http_With_Value_Replacement'");
            }

            return ReturnValue;
        }
    }
}