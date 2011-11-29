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
using System.Data;
using Ict.Common;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.MCommon
{
    /// <summary>
    /// Contains functions for handling of Addresses.
    /// </summary>
    public class TAddressHandling : System.Object
    {
        #region TAddressHandling

        /// <summary>
        /// Creates a new DataRow in the passed PLocation and PPartnerLocation tables.
        ///
        /// </summary>
        /// <param name="ALocationDT">Typed PLocation table. If nil is passed in it is created
        /// automatically</param>
        /// <param name="APartnerLocationDT">Typed PPartnerLocation table. If nil is passed in
        /// it is created automatically</param>
        /// <param name="APartnerKey">PartneKey of the Partner for which the Address should
        /// be created</param>
        /// <param name="APartnerClass">PartnerClass of the Partner for which the Address should
        /// be created</param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey">A LocationKey that the new Location and
        /// PartnerLocation rows would be set to.</param>
        /// <returns>void</returns>
        /// <exception cref="ArgumentException"> if any of the Arguments (or their combination) is
        /// not valid
        /// </exception>
        public static void CreateNewAddress(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey)
        {
            CreateNewAddressInternal(ALocationDT, APartnerLocationDT, APartnerKey, APartnerClass, ACountryCode, ANewLocationKey);
        }

        /// <summary>
        /// Creates a new DataRow in the passed PLocation and PPartnerLocation tables.
        ///
        /// </summary>
        /// <param name="ALocationDT">Typed PLocation table. If nil is passed in it is created
        /// automatically</param>
        /// <param name="APartnerLocationDT">Typed PPartnerLocation table. If nil is passed in
        /// it is created automatically</param>
        /// <param name="APartnerKey">PartneKey of the Partner for which the Address should
        /// be created</param>
        /// <param name="APartnerClass">PartnerClass of the Partner for which the Address should
        /// be created</param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey">A LocationKey that the new Location and
        /// PartnerLocation rows would be set to.</param>
        /// <param name="ACopyFromPartnerLocationKey">Pass in a PartnerLocation Key if certain
        /// data (e-mail address, URL and mobile phone number) should be copied over
        /// to the new Address. Note: the DataRow specified with
        /// ACopyFromPartnerLocationKey must be present in the APartnerLocationDT
        /// table!</param>
        /// <param name="ACopyFromPartnerSiteKey">A SiteKey to find the location that should be the source of the copy</param>
        /// <returns>void</returns>
        /// <exception cref="ArgumentException"> if any of the Arguments (or their combination) is
        /// not valid
        /// </exception>
        public static void CreateNewAddress(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int32 ACopyFromPartnerLocationKey,
            Int64 ACopyFromPartnerSiteKey)
        {
            CreateNewAddressInternal(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                ACopyFromPartnerLocationKey,
                ACopyFromPartnerSiteKey);
        }

        /// <summary>
        /// Creates a new DataRow in the passed PLocation and PPartnerLocation tables.
        ///
        /// </summary>
        /// <param name="ALocationDT">Typed PLocation table. If nil is passed in it is created
        /// automatically</param>
        /// <param name="APartnerLocationDT">Typed PPartnerLocation table. If nil is passed in
        /// it is created automatically</param>
        /// <param name="APartnerKey">PartneKey of the Partner for which the Address should
        /// be created</param>
        /// <param name="APartnerClass">PartnerClass of the Partner for which the Address should
        /// be created</param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey">A LocationKey that the new Location and
        /// PartnerLocation rows would be set to.</param>
        /// <param name="ACopyFromFamilyPartnerKey">Pass in the PartnerKey of a Family Partner
        /// to copy over all data from the Family's Address. Note: the DataRow
        /// specified with ACopyFromFamilyPartnerKey must be present in the
        /// APartnerLocationDT table!</param>
        /// <param name="ACopyFromFamilyLocationKey">Pass in the LocationKey of the Family
        /// Partner specified in ACopyFromFamilyPartnerKey to copy over all data from
        /// the Family's Address. Note: the DataRow specified with
        /// ACopyFromFamilyLocationKey must be present in both the ALocationDT and
        /// APartnerLocationDT table!</param>
        /// <param name="ACopyFromFamilySiteKey">A SiteKey to find the location that should be the source of the copy</param>
        /// <param name="ACopyFromFamilyOnlyLocation"></param>
        /// <param name="ADeleteDataRowCopiedFrom"></param>
        /// <returns>void</returns>
        /// <exception cref="ArgumentException"> if any of the Arguments (or their combination) is
        /// not valid
        /// </exception>
        public static void CreateNewAddress(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int64 ACopyFromFamilyPartnerKey,
            Int32 ACopyFromFamilyLocationKey,
            Int64 ACopyFromFamilySiteKey,
            Boolean ACopyFromFamilyOnlyLocation,
            Boolean ADeleteDataRowCopiedFrom)
        {
            CreateNewAddressInternal(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                -99,
                -99,
                ACopyFromFamilyPartnerKey,
                ACopyFromFamilyLocationKey,
                ACopyFromFamilySiteKey,
                ACopyFromFamilyOnlyLocation,
                ADeleteDataRowCopiedFrom);
        }

        /// <summary>
        /// </summary>
        /// <param name="ALocationDT"></param>
        /// <param name="APartnerLocationDT"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey"></param>
        /// <param name="ACopyFromFamilyPartnerKey"></param>
        /// <param name="ACopyFromFamilyLocationKey"></param>
        /// <param name="ACopyFromFamilySiteKey"></param>
        /// <param name="ACopyFromFamilyOnlyLocation"></param>
        public static void CreateNewAddress(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int64 ACopyFromFamilyPartnerKey,
            Int32 ACopyFromFamilyLocationKey,
            Int64 ACopyFromFamilySiteKey,
            Boolean ACopyFromFamilyOnlyLocation)
        {
            CreateNewAddress(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                ACopyFromFamilyPartnerKey,
                ACopyFromFamilyLocationKey,
                ACopyFromFamilySiteKey,
                ACopyFromFamilyOnlyLocation,
                true);
        }

        /// <summary>
        /// </summary>
        /// <param name="ALocationDT"></param>
        /// <param name="APartnerLocationDT"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey"></param>
        /// <param name="ACopyFromFamilyPartnerKey"></param>
        /// <param name="ACopyFromFamilyLocationKey"></param>
        /// <param name="ACopyFromFamilySiteKey"></param>
        public static void CreateNewAddress(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int64 ACopyFromFamilyPartnerKey,
            Int32 ACopyFromFamilyLocationKey,
            Int64 ACopyFromFamilySiteKey)
        {
            CreateNewAddress(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                ACopyFromFamilyPartnerKey,
                ACopyFromFamilyLocationKey,
                ACopyFromFamilySiteKey,
                false,
                true);
        }

        /// <summary>
        /// </summary>
        /// <param name="ALocationDT"></param>
        /// <param name="APartnerLocationDT"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey"></param>
        /// <param name="ACopyFromPartnerLocationKey"></param>
        /// <param name="ACopyFromPartnerSiteKey"></param>
        /// <param name="ACopyFromFamilyPartnerKey"></param>
        /// <param name="ACopyFromFamilyLocationKey"></param>
        /// <param name="ACopyFromFamilySiteKey"></param>
        /// <param name="ACopyFromFamilyOnlyLocation"></param>
        /// <param name="ADeleteDataRowCopiedFrom"></param>
        public static void CreateNewAddress(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int32 ACopyFromPartnerLocationKey,
            Int64 ACopyFromPartnerSiteKey,
            Int64 ACopyFromFamilyPartnerKey,
            Int32 ACopyFromFamilyLocationKey,
            Int64 ACopyFromFamilySiteKey,
            Boolean ACopyFromFamilyOnlyLocation,
            Boolean ADeleteDataRowCopiedFrom)
        {
            CreateNewAddressInternal(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                ACopyFromPartnerLocationKey,
                ACopyFromPartnerSiteKey,
                ACopyFromFamilyPartnerKey,
                ACopyFromFamilyLocationKey,
                ACopyFromFamilySiteKey,
                ACopyFromFamilyOnlyLocation,
                ADeleteDataRowCopiedFrom);
        }

        /// <summary>
        /// </summary>
        /// <param name="ALocationDT"></param>
        /// <param name="APartnerLocationDT"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey"></param>
        /// <param name="ACopyFromPartnerLocationKey"></param>
        /// <param name="ACopyFromPartnerSiteKey"></param>
        /// <param name="ACopyFromFamilyPartnerKey"></param>
        /// <param name="ACopyFromFamilyLocationKey"></param>
        /// <param name="ACopyFromFamilySiteKey"></param>
        /// <param name="ACopyFromFamilyOnlyLocation"></param>
        public static void CreateNewAddress(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int32 ACopyFromPartnerLocationKey,
            Int64 ACopyFromPartnerSiteKey,
            Int64 ACopyFromFamilyPartnerKey,
            Int32 ACopyFromFamilyLocationKey,
            Int64 ACopyFromFamilySiteKey,
            Boolean ACopyFromFamilyOnlyLocation)
        {
            CreateNewAddress(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                ACopyFromPartnerLocationKey,
                ACopyFromPartnerSiteKey,
                ACopyFromFamilyPartnerKey,
                ACopyFromFamilyLocationKey,
                ACopyFromFamilySiteKey,
                ACopyFromFamilyOnlyLocation,
                true);
        }

        /// <summary>
        /// </summary>
        /// <param name="ALocationDT"></param>
        /// <param name="APartnerLocationDT"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey"></param>
        /// <param name="ACopyFromPartnerLocationKey"></param>
        /// <param name="ACopyFromPartnerSiteKey"></param>
        /// <param name="ACopyFromFamilyPartnerKey"></param>
        /// <param name="ACopyFromFamilyLocationKey"></param>
        /// <param name="ACopyFromFamilySiteKey"></param>
        public static void CreateNewAddress(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int32 ACopyFromPartnerLocationKey,
            Int64 ACopyFromPartnerSiteKey,
            Int64 ACopyFromFamilyPartnerKey,
            Int32 ACopyFromFamilyLocationKey,
            Int64 ACopyFromFamilySiteKey)
        {
            CreateNewAddress(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                ACopyFromPartnerLocationKey,
                ACopyFromPartnerSiteKey,
                ACopyFromFamilyPartnerKey,
                ACopyFromFamilyLocationKey,
                ACopyFromFamilySiteKey,
                false,
                true);
        }

        /// <summary>
        /// </summary>
        /// <param name="AFoundAddressLocationRow"></param>
        /// <param name="ADestinationLocationRow"></param>
        public static void CopyFoundAddressLocationData(PLocationRow AFoundAddressLocationRow, PLocationRow ADestinationLocationRow)
        {
            CopyLocationData(AFoundAddressLocationRow, ADestinationLocationRow);
        }

        /// <summary>
        /// Copies over all columns of a Location Row, except the Primary Key columns
        /// and the last four columns (containing creation and change information).
        ///
        /// </summary>
        /// <param name="ACopyLocationsRow">Row to copy data from</param>
        /// <param name="ADestinationLocationsRow">Row to copy data to
        /// </param>
        /// <returns>void</returns>
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
        ///
        /// </summary>
        /// <param name="ALocationDT">Typed PLocation table. If nil is passed in it is created
        /// automatically</param>
        /// <param name="APartnerLocationDT">Typed PPartnerLocation table. If nil is passed in
        /// it is created automatically</param>
        /// <param name="APartnerKey">PartneKey of the Partner for which the Address should
        /// be created</param>
        /// <param name="APartnerClass">PartnerClass of the Partner for which the Address should
        /// be created</param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey">A LocationKey that the new Location and
        /// PartnerLocation rows would be set to.</param>
        /// <param name="ACopyFromPartnerLocationKey">Pass in a PartnerLocation Key if certain
        /// data (e-mail address, URL and mobile phone number) should be copied over
        /// to the new Address (the following parameters must then be -1 to be legal)
        /// Default: -1 (=no copy). Note: the DataRow specified with
        /// ACopyFromPartnerLocationKey must be present in the APartnerLocationDT
        /// table!</param>
        /// <param name="ACopyFromPartnerSiteKey">A SiteKey to find the location that should
        /// be the source of the copy</param>
        /// <param name="ACopyFromFamilyPartnerKey">Pass in the PartnerKey of a Family Partner
        /// to copy over all data from the Family's Address
        /// (ACopyFromPartnerLocationKey must be -1 to be legal). Default: -1
        /// (=no copy). Note: the DataRow specified with ACopyFromFamilyPartnerKey must
        /// be present in the APartnerLocationDT table!</param>
        /// <param name="ACopyFromFamilyLocationKey">Pass in the LocationKey of the Family
        /// Partner specified in ACopyFromFamilyPartnerKey to copy over all data from
        /// the Family's Address (ACopyFromPartnerLocationKey must be -1 to be legal)
        /// Default: -1 (=no copy). Note: the DataRow specified with
        /// ACopyFromFamilyLocationKey must be present in both the ALocationDT and
        /// APartnerLocationDT table!</param>
        /// <param name="ACopyFromFamilySiteKey">A SiteKey to find the location that should be the
        /// source of the copy</param>
        /// <param name="ACopyFromFamilyOnlyLocation"></param>
        /// <param name="ADeleteDataRowCopiedFrom"></param>
        /// <returns>void</returns>
        /// <exception cref="ArgumentException">If any of the Arguments (or their combination) is
        /// not valid
        /// </exception>
        public static void CreateNewAddressInternal(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int32 ACopyFromPartnerLocationKey,
            Int64 ACopyFromPartnerSiteKey,
            Int64 ACopyFromFamilyPartnerKey,
            Int32 ACopyFromFamilyLocationKey,
            Int64 ACopyFromFamilySiteKey,
            Boolean ACopyFromFamilyOnlyLocation,
            Boolean ADeleteDataRowCopiedFrom)
        {
            PLocationRow NewLocationsRow;
            PLocationRow CopyLocationsRow;
            PPartnerLocationRow NewPartnerLocationRow;
            PPartnerLocationRow CopyPartnerLocationRow;

            #region Check parameters

            if (ALocationDT == null)
            {
                ALocationDT = new PLocationTable();
            }

            if (APartnerLocationDT == null)
            {
                APartnerLocationDT = new PPartnerLocationTable();
            }

            if (TStaticDataTables.TMPartner.GetStaticTable(TStaticPartnerTablesEnum.PartnerClassList).Rows.Find(APartnerClass) == null)
            {
                throw new ArgumentException("APartnerClass needs to be a valid Partner Class");
            }

            if (ACopyFromPartnerLocationKey != -99)
            {
                if ((ACopyFromFamilyLocationKey != -99) && (!ACopyFromFamilyOnlyLocation))
                {
                    throw new ArgumentException("ACopyFromFamilyLocationKey needs to be -99 if ACopyFromPartnerLocationKey is specified");
                }
            }

            if ((ACopyFromFamilyPartnerKey != -1) || (ACopyFromFamilyLocationKey != -99))
            {
                if ((ACopyFromPartnerLocationKey != -99) && (!ACopyFromFamilyOnlyLocation))
                {
                    throw new ArgumentException(
                        "ACopyFromPartnerLocationKey needs to be -99 if ACopyFromFamilyPartnerKey and ACopyFromFamilyLocationKey are specified");
                }

                if (ACopyFromFamilyPartnerKey != -1)
                {
                    if (ACopyFromFamilyLocationKey == -99)
                    {
                        throw new ArgumentException("ACopyFromFamilyLocationKey needs to be specified if ACopyFromFamilyPartnerKey is specified");
                    }
                }

                if (ACopyFromFamilyLocationKey != -99)
                {
                    if (ACopyFromFamilyPartnerKey == -1)
                    {
                        throw new ArgumentException("ACopyFromFamilyPartnerKey needs to be specified if ACopyFromFamilyLocationKey is specified");
                    }
                }
            }

            #endregion

            /*
             * Add new Locations row
             */
            NewLocationsRow = ALocationDT.NewRowTyped(true);

            // Assign Primary Key columns
            NewLocationsRow.SiteKey = SharedConstants.FIXED_SITE_KEY; // TODO: use s_system_parameter.s_site_key_n once p_partner_location actually uses the Petra System SiteKey in the PrimaryKey (instead of 0, which is used currently)
            NewLocationsRow.LocationKey = ANewLocationKey;

            // Copy over Columns of the Row specified with ACopyFromFamilyLocationKey?
            if (ACopyFromFamilyLocationKey != -99)
            {
                CopyLocationsRow = (PLocationRow)ALocationDT.Rows.Find(new Object[] { ACopyFromFamilySiteKey, ACopyFromFamilyLocationKey });

                if (CopyLocationsRow != null)
                {
                    if (ACountryCode != "")
                    {
                        throw new ArgumentException(
                            "ACountryCode must not be specified because it would be overwritten by the value from the row specified with ACopyFromFamilyLocationKey");
                    }

                    CopyLocationData(CopyLocationsRow, NewLocationsRow);

                    if (ADeleteDataRowCopiedFrom)
                    {
                        CopyLocationsRow.Delete();
                        CopyLocationsRow.AcceptChanges();
                    }
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
            if ((!ACopyFromFamilyOnlyLocation) && (ACopyFromFamilyLocationKey != -99))
            {
                CopyPartnerLocationRow =
                    (PPartnerLocationRow)APartnerLocationDT.Rows.Find(new Object[] { ACopyFromFamilyPartnerKey, ACopyFromFamilySiteKey,
                                                                                     ACopyFromFamilyLocationKey });

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
                        NewPartnerLocationRow.TelephoneNumber = CopyPartnerLocationRow.TelephoneNumber;
                        NewPartnerLocationRow.Extension = CopyPartnerLocationRow.Extension;
                        NewPartnerLocationRow.FaxNumber = CopyPartnerLocationRow.FaxNumber;
                        NewPartnerLocationRow.FaxExtension = CopyPartnerLocationRow.FaxExtension;
                        NewPartnerLocationRow.AlternateTelephone = CopyPartnerLocationRow.AlternateTelephone;
                        NewPartnerLocationRow.MobileNumber = CopyPartnerLocationRow.MobileNumber;
                        NewPartnerLocationRow.EmailAddress = CopyPartnerLocationRow.EmailAddress;
                        NewPartnerLocationRow.Url = CopyPartnerLocationRow.Url;

                        if (!CopyPartnerLocationRow.IsDateGoodUntilNull())
                        {
                            // Copy over DateGoodUntil only if it doesn't lie in the past
                            if (CopyPartnerLocationRow.DateGoodUntil >= DateTime.Today)
                            {
                                NewPartnerLocationRow.DateGoodUntil = CopyPartnerLocationRow.DateGoodUntil;
                            }
                        }

                        if (ADeleteDataRowCopiedFrom)
                        {
                            CopyPartnerLocationRow.Delete();
                            CopyPartnerLocationRow.AcceptChanges();
                        }
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
                    throw new ArgumentException("Row with specified ACopyFromFamilyPartnerKey cannot be found in PPartnerLocations DataTable!");
                }
            }
            else
            {
                NewPartnerLocationRow.LocationType = TSharedAddressHandling.GetDefaultLocationType(APartnerClass);

                // Copy over Columns of the Row specified with ACopyFromPartnerLocationKey?
                if (ACopyFromPartnerLocationKey != -99)
                {
                    CopyPartnerLocationRow = (PPartnerLocationRow)APartnerLocationDT.Rows.Find(new Object[] { APartnerKey, ACopyFromPartnerSiteKey,
                                                                                                              ACopyFromPartnerLocationKey });

                    if (CopyPartnerLocationRow != null)
                    {
                        // Security check for LocationType SECURITY_CAN_LOCATIONTYPE
                        if (((CopyPartnerLocationRow.LocationType.EndsWith(SharedConstants.SECURITY_CAN_LOCATIONTYPE)
                              && (UserInfo.GUserInfo.IsInGroup(SharedConstants.PETRAGROUP_ADDRESSCAN))))
                            || (!((CopyPartnerLocationRow.LocationType.EndsWith(SharedConstants.SECURITY_CAN_LOCATIONTYPE)))))
                        {
                            if (!CopyPartnerLocationRow.IsUrlNull())
                            {
                                NewPartnerLocationRow.Url = CopyPartnerLocationRow.Url;
                            }

                            if (!CopyPartnerLocationRow.IsEmailAddressNull())
                            {
                                NewPartnerLocationRow.EmailAddress = CopyPartnerLocationRow.EmailAddress;
                            }

                            if (!CopyPartnerLocationRow.IsMobileNumberNull())
                            {
                                NewPartnerLocationRow.MobileNumber = CopyPartnerLocationRow.MobileNumber;
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Row with specified ACopyFromPartnerLocationKey cannot be found in PPartnerLocations DataTable!");
                    }
                }
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
        /// </summary>
        /// <param name="ALocationDT"></param>
        /// <param name="APartnerLocationDT"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey"></param>
        /// <param name="ACopyFromPartnerLocationKey"></param>
        /// <param name="ACopyFromPartnerSiteKey"></param>
        /// <param name="ACopyFromFamilyPartnerKey"></param>
        /// <param name="ACopyFromFamilyLocationKey"></param>
        /// <param name="ACopyFromFamilySiteKey"></param>
        /// <param name="ACopyFromFamilyOnlyLocation"></param>
        public static void CreateNewAddressInternal(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int32 ACopyFromPartnerLocationKey,
            Int64 ACopyFromPartnerSiteKey,
            Int64 ACopyFromFamilyPartnerKey,
            Int32 ACopyFromFamilyLocationKey,
            Int64 ACopyFromFamilySiteKey,
            Boolean ACopyFromFamilyOnlyLocation)
        {
            CreateNewAddressInternal(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                ACopyFromPartnerLocationKey,
                ACopyFromPartnerSiteKey,
                ACopyFromFamilyPartnerKey,
                ACopyFromFamilyLocationKey,
                ACopyFromFamilySiteKey,
                ACopyFromFamilyOnlyLocation,
                true);
        }

        /// <summary>
        /// </summary>
        /// <param name="ALocationDT"></param>
        /// <param name="APartnerLocationDT"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey"></param>
        /// <param name="ACopyFromPartnerLocationKey"></param>
        /// <param name="ACopyFromPartnerSiteKey"></param>
        /// <param name="ACopyFromFamilyPartnerKey"></param>
        /// <param name="ACopyFromFamilyLocationKey"></param>
        /// <param name="ACopyFromFamilySiteKey"></param>
        public static void CreateNewAddressInternal(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int32 ACopyFromPartnerLocationKey,
            Int64 ACopyFromPartnerSiteKey,
            Int64 ACopyFromFamilyPartnerKey,
            Int32 ACopyFromFamilyLocationKey,
            Int64 ACopyFromFamilySiteKey)
        {
            CreateNewAddressInternal(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                ACopyFromPartnerLocationKey,
                ACopyFromPartnerSiteKey,
                ACopyFromFamilyPartnerKey,
                ACopyFromFamilyLocationKey,
                ACopyFromFamilySiteKey,
                false,
                true);
        }

        /// <summary>
        /// </summary>
        /// <param name="ALocationDT"></param>
        /// <param name="APartnerLocationDT"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey"></param>
        /// <param name="ACopyFromPartnerLocationKey"></param>
        /// <param name="ACopyFromPartnerSiteKey"></param>
        /// <param name="ACopyFromFamilyPartnerKey"></param>
        /// <param name="ACopyFromFamilyLocationKey"></param>
        public static void CreateNewAddressInternal(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int32 ACopyFromPartnerLocationKey,
            Int64 ACopyFromPartnerSiteKey,
            Int64 ACopyFromFamilyPartnerKey,
            Int32 ACopyFromFamilyLocationKey)
        {
            CreateNewAddressInternal(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                ACopyFromPartnerLocationKey,
                ACopyFromPartnerSiteKey,
                ACopyFromFamilyPartnerKey,
                ACopyFromFamilyLocationKey,
                -99,
                false,
                true);
        }

        /// <summary>
        /// </summary>
        /// <param name="ALocationDT"></param>
        /// <param name="APartnerLocationDT"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey"></param>
        /// <param name="ACopyFromPartnerLocationKey"></param>
        /// <param name="ACopyFromPartnerSiteKey"></param>
        /// <param name="ACopyFromFamilyPartnerKey"></param>
        public static void CreateNewAddressInternal(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int32 ACopyFromPartnerLocationKey,
            Int64 ACopyFromPartnerSiteKey,
            Int64 ACopyFromFamilyPartnerKey)
        {
            CreateNewAddressInternal(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                ACopyFromPartnerLocationKey,
                ACopyFromPartnerSiteKey,
                ACopyFromFamilyPartnerKey,
                -99,
                -99,
                false,
                true);
        }

        /// <summary>
        /// </summary>
        /// <param name="ALocationDT"></param>
        /// <param name="APartnerLocationDT"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey"></param>
        /// <param name="ACopyFromPartnerLocationKey"></param>
        /// <param name="ACopyFromPartnerSiteKey"></param>
        public static void CreateNewAddressInternal(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int32 ACopyFromPartnerLocationKey,
            Int64 ACopyFromPartnerSiteKey)
        {
            CreateNewAddressInternal(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                ACopyFromPartnerLocationKey,
                ACopyFromPartnerSiteKey,
                -1,
                -99,
                -99,
                false,
                true);
        }

        /// <summary>
        /// </summary>
        /// <param name="ALocationDT"></param>
        /// <param name="APartnerLocationDT"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey"></param>
        /// <param name="ACopyFromPartnerLocationKey"></param>
        public static void CreateNewAddressInternal(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey,
            Int32 ACopyFromPartnerLocationKey)
        {
            CreateNewAddressInternal(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                ACopyFromPartnerLocationKey,
                -99,
                -1,
                -99,
                -99,
                false,
                true);
        }

        /// <summary>
        /// </summary>
        /// <param name="ALocationDT"></param>
        /// <param name="APartnerLocationDT"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="ANewLocationKey"></param>
        public static void CreateNewAddressInternal(PLocationTable ALocationDT,
            PPartnerLocationTable APartnerLocationDT,
            Int64 APartnerKey,
            TPartnerClass APartnerClass,
            String ACountryCode,
            Int32 ANewLocationKey)
        {
            CreateNewAddressInternal(ALocationDT,
                APartnerLocationDT,
                APartnerKey,
                APartnerClass,
                ACountryCode,
                ANewLocationKey,
                -99,
                -99,
                -1,
                -99,
                -99,
                false,
                true);
        }

        /// <summary>
        /// Gets the AdressOrder (p_address_order_i DB field) of a certain Country.
        ///
        /// </summary>
        /// <param name="ACountryCode">CountryCode (ISO Code) of a Country</param>
        /// <returns>AddressOrder for that Country (0 if Country cannot be found).
        /// </returns>
        public static Int32 GetAddressOrder(String ACountryCode)
        {
            Int32 ReturnValue;
            PCountryTable DataCacheCountryDT;
            PCountryRow CountryDR;

            DataCacheCountryDT = (PCountryTable)TDataCache.TMCommon.GetCacheableCommonTable(TCacheableCommonTablesEnum.CountryList);
            CountryDR = (PCountryRow)DataCacheCountryDT.Rows.Find(ACountryCode);

            if (CountryDR != null)
            {
                ReturnValue = CountryDR.AddressOrder;
            }
            else
            {
                ReturnValue = 0;
            }

            return ReturnValue;
        }

        #endregion
    }
}