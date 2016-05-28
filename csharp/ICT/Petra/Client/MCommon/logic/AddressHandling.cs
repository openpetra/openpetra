//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2016 by OM International
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
using Ict.Common.Exceptions;

using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MCommon
{
    /// <summary>
    /// Contains functions for handling of Addresses.
    /// </summary>
    public static class TAddressHandling
    {
        /// <summary>
        /// Copies over all columns of a Location Row, except the Primary Key columns
        /// and the last four columns (containing creation and change information).
        /// </summary>
        /// <param name="ACopyLocationsRow">Row to copy data from.</param>
        /// <param name="ADestinationLocationsRow">Row to copy data to.</param>
        public static void CopyLocationData(PLocationRow ACopyLocationsRow, PLocationRow ADestinationLocationsRow)
        {
            ADestinationLocationsRow.Locality = ACopyLocationsRow.Locality;
            ADestinationLocationsRow.StreetName = ACopyLocationsRow.StreetName;
            ADestinationLocationsRow.Address3 = ACopyLocationsRow.Address3;
            ADestinationLocationsRow.City = ACopyLocationsRow.City;
            ADestinationLocationsRow.PostalCode = ACopyLocationsRow.PostalCode;
            ADestinationLocationsRow.County = ACopyLocationsRow.County;
            ADestinationLocationsRow.CountryCode = ACopyLocationsRow.CountryCode;

            // Created/Modified info
            if (!ACopyLocationsRow.IsDateCreatedNull())
            {
                ADestinationLocationsRow.DateCreated = ACopyLocationsRow.DateCreated;
            }

            if (!ACopyLocationsRow.IsDateModifiedNull())
            {
                ADestinationLocationsRow.DateModified = ACopyLocationsRow.DateModified;
            }

            ADestinationLocationsRow.CreatedBy = TSaveConvert.StringColumnToString(((PLocationTable)ACopyLocationsRow.Table).ColumnCreatedBy,
                ACopyLocationsRow);
            ADestinationLocationsRow.ModifiedBy = TSaveConvert.StringColumnToString(((PLocationTable)ACopyLocationsRow.Table).ColumnModifiedBy,
                ACopyLocationsRow);
            ADestinationLocationsRow.ModificationId = ACopyLocationsRow.ModificationId;
        }

        /// <summary>
        /// Creates a new DataRow in the passed PLocation and PPartnerLocation tables.
        /// </summary>
        /// <param name="ALocationDT">Typed PLocation table. If null is passed in it is created
        /// automatically</param>
        /// <param name="APartnerLocationDT">Typed PPartnerLocation table. If null is passed in
        /// it is created automatically</param>
        /// <param name="APartnerKey">PartneKey of the Partner for which the Address should
        /// be created</param>
        /// <param name="APartnerClass">PartnerClass of the Partner for which the Address should
        /// be created</param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey">A LocationKey that the new Location and
        /// PartnerLocation rows would be set to.</param>
        /// <param name="ACopyFromFamilyPartnerKey">Pass in the PartnerKey of a Family Partner
        /// to copy over all data from the Family's Address if that is desired. Note: the DataRow
        /// specified with <paramref name="ACopyFromFamilyPartnerKey"/> must be present in the
        /// APartnerLocationDT table!</param>
        /// <param name="ACopyFromFamilyLocation">Pass in the Location of the Family
        /// Partner specified with <paramref name="ACopyFromFamilyPartnerKey"/> to copy over all
        /// data from the Family's Address if that is desired. Note: the
        /// DataRow specified with <paramref name="ACopyFromFamilyLocation"/> must be present in
        /// both the ALocationDT and APartnerLocationDT table!</param>
        /// <exception cref="ArgumentException">Throws an <see cref="ArgumentException"/> if any
        /// of the Arguments (or their combination) is not valid.</exception>
        public static void CreateNewAddress(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey = -1,
            Int64? ACopyFromFamilyPartnerKey = null,
            TLocationPK ACopyFromFamilyLocation = null)
        {
            PLocationRow NewLocationsRow;
            PLocationRow CopyLocationsRow;
            PPartnerLocationRow NewPartnerLocationRow;
            PPartnerLocationRow CopyPartnerLocationRow;

            #region Argument checks

            if (TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.PartnerClassList).Rows.Find(APartnerClass) == null)
            {
                throw new ArgumentException("APartnerClass needs to be a valid Partner Class");
            }

            // Ensure that ACopyFromFamilyPartnerKey and ACopyFromFamilyLocation go hand in hand
            if ((ACopyFromFamilyPartnerKey.HasValue) || (ACopyFromFamilyLocation != null))
            {
                if (ACopyFromFamilyPartnerKey.HasValue)
                {
                    if (ACopyFromFamilyLocation == null)
                    {
                        throw new ArgumentException("ACopyFromFamilyLocationKey needs to be specified if ACopyFromFamilyPartnerKey is specified");
                    }
                }

                if (ACopyFromFamilyLocation != null)
                {
                    if (!ACopyFromFamilyPartnerKey.HasValue)
                    {
                        throw new ArgumentException("ACopyFromFamilyPartnerKey needs to be specified if ACopyFromFamilyLocationKey is specified");
                    }
                }
            }

            #endregion

            if (ALocationDT == null)
            {
                ALocationDT = new PLocationTable();
            }

            if (APartnerLocationDT == null)
            {
                APartnerLocationDT = new PPartnerLocationTable();
            }

            /*
             * Add new Locations row
             */
            NewLocationsRow = ALocationDT.NewRowTyped(true);

            // Assign Primary Key columns
            NewLocationsRow.SiteKey = SharedConstants.FIXED_SITE_KEY; // TODO: use s_system_parameter.s_site_key_n once p_partner_location actually uses the Petra System SiteKey in the PrimaryKey (instead of 0, which is used currently)
            NewLocationsRow.LocationKey = ANewLocationKey;

            // Copy over Columns of the Row specified with ACopyFromFamilyLocationKey?
            if (ACopyFromFamilyLocation != null)
            {
                CopyLocationsRow = (PLocationRow)ALocationDT.Rows.Find(new Object[] { ACopyFromFamilyLocation.SiteKey,
                                                                                      ACopyFromFamilyLocation.LocationKey });

                if (CopyLocationsRow != null)
                {
                    if (ACountryCode != "")
                    {
                        throw new ArgumentException(
                            "ACountryCode must not be specified because it would be overwritten by the value from the row specified with ACopyFromFamilyLocationKey");
                    }

                    CopyLocationData(CopyLocationsRow, NewLocationsRow);

                    // Now remove the Row that we copied data from
                    CopyLocationsRow.Delete();
                    CopyLocationsRow.AcceptChanges();
                }
                else
                {
                    throw new ArgumentException("Row with specified ACopyFromFamilyLocationKey cannot be found in PLocations DataTable!");
                }
            }
            else
            {
                // No copying > assign values of columns manually
                NewLocationsRow.CountryCode = ACountryCode;
            }

            NewLocationsRow.CreatedBy = UserInfo.GUserInfo.UserID;
            ALocationDT.Rows.Add(NewLocationsRow);

            // If an existing Location shouldn't be copied to a temp LocationKey (<0) then make this DataRow unchanged
            // (prevents this row from getting commited to the PetraServer).
            if (ANewLocationKey >= 0)
            {
                NewLocationsRow.AcceptChanges();
            }

            /*
             * Add new PartnerLocations row
             */
            NewPartnerLocationRow = APartnerLocationDT.NewRowTyped(true);

            // Assign Primary Key columns
            NewPartnerLocationRow.PartnerKey = APartnerKey;
            NewPartnerLocationRow.SiteKey = SharedConstants.FIXED_SITE_KEY; // TODO: use s_system_parameter.s_site_key_n once p_partner_location actually uses the Petra System SiteKey in the PrimaryKey (instead of 0, which is used currently)
            NewPartnerLocationRow.LocationKey = ANewLocationKey;

            // Copy over Columns of the Row specified with ACopyFromFamilyPartnerKey
            // and ACopyFromFamilyLocationKey?
            if (ACopyFromFamilyLocation != null)
            {
                CopyPartnerLocationRow =
                    (PPartnerLocationRow)APartnerLocationDT.Rows.Find(new Object[] { ACopyFromFamilyPartnerKey.Value,
                                                                                     ACopyFromFamilyLocation.SiteKey,
                                                                                     ACopyFromFamilyLocation.LocationKey });

                if (CopyPartnerLocationRow != null)
                {
                    // Security check for LocationType SECURITY_CAN_LOCATIONTYPE
                    if (((CopyPartnerLocationRow.LocationType.EndsWith(SharedConstants.SECURITY_CAN_LOCATIONTYPE)
                          && (UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_ADDRESSCAN))))
                        || (!((CopyPartnerLocationRow.LocationType.EndsWith(SharedConstants.SECURITY_CAN_LOCATIONTYPE)))))
                    {
                        // Copy over all columns, except the Primary Key columns and the last four
                        // columns (containing creation and change information)
                        NewPartnerLocationRow.LocationType = CopyPartnerLocationRow.LocationType;

                        if (!CopyPartnerLocationRow.IsDateGoodUntilNull())
                        {
                            // Copy over DateGoodUntil only if it doesn't lie in the past
                            if (CopyPartnerLocationRow.DateGoodUntil >= DateTime.Today)
                            {
                                NewPartnerLocationRow.DateGoodUntil = CopyPartnerLocationRow.DateGoodUntil;
                            }
                        }

                        // Now remove the Row that we copied data from
                        CopyPartnerLocationRow.Delete();
                        CopyPartnerLocationRow.AcceptChanges();
                    }
                    else
                    {
                        throw new ESecurityGroupAccessDeniedException(
                            "Address copying denied: " + "The Location Type of the Address that should be copied is '" +
                            CopyPartnerLocationRow.LocationType + "' but you are not in Security Group '" + SharedConstants.PETRAGROUP_ADDRESSCAN +
                            "'!");
                    }
                }
                else
                {
                    throw new ArgumentException(
                        String.Format("Row with specified ACopyFromFamilyPartnerKey {0} cannot be found in PPartnerLocations DataTable!",
                            ACopyFromFamilyPartnerKey));
                }
            }
            else
            {
                NewPartnerLocationRow.LocationType = TSharedAddressHandling.GetDefaultLocationType(APartnerClass);
            }

            NewPartnerLocationRow.DateEffective = DateTime.Today;
            NewPartnerLocationRow.CreatedBy = UserInfo.GUserInfo.UserID;

            if (APartnerClass == TPartnerClass.PERSON)
            {
                NewPartnerLocationRow.SendMail = false;
            }
            else
            {
                NewPartnerLocationRow.SendMail = true;
            }

            APartnerLocationDT.Rows.Add(NewPartnerLocationRow);
        }

        /// <summary>
        /// Gets the AddressOrder (p_address_order_i DB field) of a certain Country.
        /// </summary>
        /// <param name="ACountryCode">CountryCode (ISO Code) of a Country.</param>
        /// <returns>AddressOrder for that Country (0 if Country cannot be found).</returns>
        public static Int32 GetAddressOrder(String ACountryCode)
        {
            PCountryTable DataCacheCountryDT = (PCountryTable)TDataCache.TMCommon.GetCacheableCommonTable(TCacheableCommonTablesEnum.CountryList);
            PCountryRow CountryDR = (PCountryRow)DataCacheCountryDT.Rows.Find(ACountryCode);

            return (CountryDR != null) ? CountryDR.AddressOrder : 0;
        }
    }
}