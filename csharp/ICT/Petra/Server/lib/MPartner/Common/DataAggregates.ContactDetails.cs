//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2014 by OM International
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

using Ict.Common.DB;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;

namespace Ict.Petra.Server.MPartner.DataAggregates
{
    /// <summary>
    /// The TContactDetailsAggregate Class contains logic for working with Partner Contact Details.
    /// </summary>
    public static class TContactDetailsAggregate
    {
        /// <summary>
        /// Gets the 'Primary Phone Number' of a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner.</param>
        /// <param name="APrimaryPhoneNumber">The 'Primary Phone Number' if the Partner has got one, otherwise null.</param>
        /// <returns>True if the Partner has got a 'Primary Phone Number', otherwise false.</returns>
        public static bool GetPrimaryPhoneNumber(Int64 APartnerKey, out string APrimaryPhoneNumber)
        {
            Calculations.TPartnersOverallContactSettings PrimaryContactAttributes = GetPartnersOverallCS(APartnerKey,
                Calculations.TOverallContSettingKind.ocskPrimaryPhoneNumber);

            return Calculations.GetPrimaryPhoneNumber(PrimaryContactAttributes,
                out APrimaryPhoneNumber);
        }

        /// <summary>
        /// Gets the 'Primary Email Address' of a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner.</param>
        /// <param name="APrimaryEmailAddress">The 'Primary Email Address' if the Partner has got one, otherwise null.</param>
        /// <returns>True if the Partner has got a 'Primary Email Address', otherwise false.</returns>
        public static bool GetPrimaryEmailAddress(Int64 APartnerKey, out string APrimaryEmailAddress)
        {
            Calculations.TPartnersOverallContactSettings PrimaryContactAttributes = GetPartnersOverallCS(APartnerKey,
                Calculations.TOverallContSettingKind.ocskPrimaryEmailAddress);

            return Calculations.GetPrimaryEmailAddress(PrimaryContactAttributes,
                out APrimaryEmailAddress);
        }

        /// <summary>
        /// Determines the 'Primary Phone Number' and the 'Primary E-mail Address' of a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner.</param>
        /// <param name="APrimaryPhoneNumber">The 'Primary Phone Number' if the Partner has got one, otherwise null.</param>
        /// <param name="APrimaryEmailAddress">The 'Primary E-mail Address' if the Partner has got one, otherwise null.</param>
        /// <returns>True if the Partner has got at least one of the 'Primary E-mail Address' and the 'Primary Phone Number'
        /// Contact Details, otherwise false.</returns>
        public static bool GetPrimaryEmailAndPrimaryPhone(Int64 APartnerKey,
            out string APrimaryPhoneNumber, out string APrimaryEmailAddress)
        {
            Calculations.TPartnersOverallContactSettings PrimaryContactAttributes = GetPartnersOverallCS(APartnerKey,
                Calculations.TOverallContSettingKind.ocskPrimaryEmailAddress |
                Calculations.TOverallContSettingKind.ocskPrimaryPhoneNumber);

            return Calculations.GetPrimaryEmailAndPrimaryPhone(PrimaryContactAttributes,
                out APrimaryPhoneNumber, out APrimaryEmailAddress);
        }

        /// <summary>
        /// Determines the 'Primary Phone Number', 'Primary E-mail Address' and the 'Fax Number' of a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner.</param>
        /// <param name="APrimaryPhoneNumber">The 'Primary Phone Number' if the Partner has got one, otherwise null.</param>
        /// <param name="APrimaryEmailAddress">The 'Primary E-mail Address' if the Partner has got one, otherwise null.</param>
        /// <param name="AFaxNumber">The (first) current 'Fax Number' if it was found, otherwise null. Should there be several
        /// current 'Fax Numbers' then the one that comes first (in the order as seen by the user) will get returned in this
        /// Argument.</param>
        /// <returns>True if the Partner has got at least one of the 'Primary E-mail Address' and the 'Primary Phone Number'
        /// Contact Details or a 'Fax Number', otherwise false.</returns>
        public static bool GetPrimaryEmailAndPrimaryPhoneAndFax(Int64 APartnerKey,
            out string APrimaryPhoneNumber, out string APrimaryEmailAddress, out string AFaxNumber)
        {
            bool ReturnValue = false;
            PPartnerAttributeTable PartnerAttributeDT;

            Calculations.TPartnersOverallContactSettings PrimaryContactAttributes = GetPartnersOverallCS(APartnerKey,
                Calculations.TOverallContSettingKind.ocskPrimaryEmailAddress |
                Calculations.TOverallContSettingKind.ocskPrimaryPhoneNumber, out PartnerAttributeDT);

            if (PartnerAttributeDT != null)
            {
                AFaxNumber = Calculations.DeterminePartnerFaxNumber(PartnerAttributeDT);

                if (AFaxNumber != null)
                {
                    ReturnValue = true;
                }

                if (Calculations.GetPrimaryEmailAndPrimaryPhone(PrimaryContactAttributes,
                        out APrimaryPhoneNumber, out APrimaryEmailAddress))
                {
                    ReturnValue = true;
                }
            }
            else
            {
                APrimaryPhoneNumber = null;
                APrimaryEmailAddress = null;
                AFaxNumber = null;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Gets the 'Within Organisation Email Address' of a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner.</param>
        /// <param name="AWithinOrganisationEmailAddress">The 'Within Organisation Email Address' if the Partner has got one, otherwise null.</param>
        /// <returns>True if the Partner has got a 'Within Organisation Email Address', otherwise false.</returns>
        public static bool GetWithinOrganisationEmailAddress(Int64 APartnerKey, out string AWithinOrganisationEmailAddress)
        {
            Calculations.TPartnersOverallContactSettings PrimaryContactAttributes = GetPartnersOverallCS(APartnerKey,
                Calculations.TOverallContSettingKind.ocskEmailAddressWithinOrg);

            return Calculations.GetWithinOrganisationEmailAddress(PrimaryContactAttributes,
                out AWithinOrganisationEmailAddress);
        }

        /// <summary>
        /// Determines the 'Primary' and/or 'Within Organisation' setting(s) for a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner.</param>
        /// <param name="AOverallContSettingKind">Specify the kind of Overall Contact Setting(s) that you want returned.
        /// Combine multiple ones with the binary OR operator ( | ).</param>
        /// <returns>An instance of <see cref="Calculations.TPartnersOverallContactSettings"/> that holds the
        /// <see cref="Calculations.TPartnersOverallContactSettings"/> for the Partner. However, it returns null
        /// in case the Partner hasn't got any p_partner_attribute records, or when the Partner has no p_partner_attribute
        /// records that constitute Contact Detail records, or when the Partner has got only one p_partner_attribute record
        /// but this records' Current flag is false. It also returns null if no record was found that met what was asked for
        /// with <paramref name="AOverallContSettingKind"/>!</returns>
        public static Calculations.TPartnersOverallContactSettings GetPartnersOverallCS(Int64 APartnerKey,
            Calculations.TOverallContSettingKind AOverallContSettingKind)
        {
            PPartnerAttributeTable PartnerAttributeDT;

            return GetPartnersOverallCS(APartnerKey, AOverallContSettingKind, out PartnerAttributeDT);
        }

        /// <summary>
        /// Determines the 'Primary' and/or 'Within Organisation' setting(s) for a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner.</param>
        /// <param name="AOverallContSettingKind">Specify the kind of Overall Contact Setting(s) that you want returned.
        /// Combine multiple ones with the binary OR operator ( | ).</param>
        /// <param name="APartnerAttributeDT">Contains the Partners' p_partner_attribute records. The ones that are
        /// 'Contact Details' have 'true' in the Column
        /// <see cref="Calculations.PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN"/>!</param>
        /// <returns>An instance of <see cref="Calculations.TPartnersOverallContactSettings"/> that holds the
        /// <see cref="Calculations.TPartnersOverallContactSettings"/> for the Partner. However, it returns null
        /// in case the Partner hasn't got any p_partner_attribute records, or when the Partner has no p_partner_attribute
        /// records that constitute Contact Detail records, or when the Partner has got only one p_partner_attribute record
        /// but this records' Current flag is false. It also returns null if no record was found that met what was asked for
        /// with <paramref name="AOverallContSettingKind"/>!</returns>
        public static Calculations.TPartnersOverallContactSettings GetPartnersOverallCS(Int64 APartnerKey,
            Calculations.TOverallContSettingKind AOverallContSettingKind, out PPartnerAttributeTable APartnerAttributeDT)
        {
            Calculations.TPartnersOverallContactSettings PrimaryContactAttributes = null;
            TDBTransaction ReadTransaction = null;
            PPartnerAttributeTable PartnerAttributeDT = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    // Load all PPartnerAttribute records of the Partner and put them into a DataTable
                    PartnerAttributeDT = PPartnerAttributeAccess.LoadViaPPartner(APartnerKey, ReadTransaction);

                    if (PartnerAttributeDT.Rows.Count > 0)
                    {
                        Calculations.DeterminePartnerContactDetailAttributes(PartnerAttributeDT);

                        PrimaryContactAttributes = Calculations.DeterminePrimaryOrWithinOrgSettingsForPartner(
                            PartnerAttributeDT, AOverallContSettingKind);

                        if (((AOverallContSettingKind & Calculations.TOverallContSettingKind.ocskPrimaryContactMethod) ==
                             Calculations.TOverallContSettingKind.ocskPrimaryContactMethod)
                            || ((AOverallContSettingKind & Calculations.TOverallContSettingKind.ocskSecondaryEmailAddress) ==
                                Calculations.TOverallContSettingKind.ocskSecondaryEmailAddress))
                        {
                            if (PrimaryContactAttributes == null)
                            {
                                PrimaryContactAttributes = new Calculations.TPartnersOverallContactSettings();
                            }

                            Calculations.DeterminePartnerSystemCategorySettings(
                                PartnerAttributeDT, ref PrimaryContactAttributes, AOverallContSettingKind);
                        }
                    }
                });

            APartnerAttributeDT = PartnerAttributeDT;

            return PrimaryContactAttributes;
        }

        /// <summary>
        /// Gets the Contact Detail Attributes for a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner.</param>
        /// <returns>An instance of <see cref="PPartnerAttributeTable"/> that holds the
        /// p_partner_attribute records for the Partner. Every Partner Attribute that *is* a Partner Contact Attribute
        /// is marked with 'true' in the special Column
        /// <see cref="Ict.Petra.Shared.MPartner.Calculations.PARTNERATTRIBUTE_PARTNERCONTACTDETAIL_COLUMN"/>!
        /// </returns>
        private static PPartnerAttributeTable GetPartnersContactDetailAttributes(Int64 APartnerKey)
        {
            PPartnerAttributeTable ReturnValue = null;
            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    // Load all PPartnerAttribute records of the Partner and put them into a DataTable
                    ReturnValue = PPartnerAttributeAccess.LoadViaPPartner(APartnerKey, ReadTransaction);

                    if (ReturnValue.Rows.Count > 0)
                    {
                        Calculations.DeterminePartnerContactDetailAttributes(ReturnValue);
                    }
                });

            return ReturnValue;
        }
    }
}