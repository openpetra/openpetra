//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.Collections.Generic;
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Server.MSysMan.DBUpgrades
{
    /// <summary>
    /// Upgrade the database
    /// </summary>
    public static partial class TDBUpgrade
    {
        /// <summary>Copied from Ict.Petra.Shared.MPartner.Calculations!</summary>
        private const String PARTNERLOCATION_ICON_COLUMN = "Icon";

        private static string ConcatPartnerAttributes(PPartnerAttributeRow row)
        {
            return row.PartnerKey.ToString() + ";" +
                   row.AttributeType + ";" +
                   row.Value + ";" +
                   row.Current;
        }

        /// Upgrade to version 2015-01
        public static bool UpgradeDatabase201412_201501()
        {
            // There are no new tables and fields

            TDBTransaction SubmitChangesTransaction = null;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref SubmitChangesTransaction,
                ref SubmissionResult,
                delegate
                {
                    PPartnerAttributeTable partnerattributes = new PPartnerAttributeTable();
                    PPartnerLocationTable partnerlocations = PPartnerLocationAccess.LoadAll(SubmitChangesTransaction);

                    // this update only works for very simple databases, only one partner location record per partner...
                    if (partnerlocations.Count > 1000)
                    {
                        throw new Exception("the upgrade has not been optimized for huge databases");
                    }

                    List <Int64>PartnerKeys = new List <Int64>();

                    foreach (PPartnerLocationRow partnerlocation in partnerlocations.Rows)
                    {
                        if (!PartnerKeys.Contains(partnerlocation.PartnerKey))
                        {
                            PartnerKeys.Add(partnerlocation.PartnerKey);
                        }
                        else
                        {
                            TLogging.Log("several locations for partner " + partnerlocation.PartnerKey.ToString());
                        }
                    }

                    // Number for the p_sequence_i Column. Gets increased with every p_partner_attribute record that gets produced!
                    int SequenceNumber = 0;

                    foreach (Int64 partnerkey in PartnerKeys)
                    {
                        // Get that Partner's p_partner_location records from PPartnerLocationRecords
                        DataRow[] CurrentRows = partnerlocations.Select(PPartnerLocationTable.GetPartnerKeyDBName() + " = " + partnerkey.ToString());

                        if (CurrentRows.Length == 0)
                        {
                            continue;
                        }

                        DataTable PPartnersLocationsDT = GetNewPPartnerLocationTableInstance();

                        foreach (DataRow r in CurrentRows)
                        {
                            PPartnersLocationsDT.Rows.Add(r.ItemArray);
                        }

                        TLocationPK bestAddress = Calculations.DetermineBestAddress(PPartnersLocationsDT);

                        int IndexPhone = 0;
                        int IndexEmail = 0;
                        int IndexFax = 0;
                        int IndexUrl = 0;
                        int IndexMobile = 0;

                        List <string>AvoidDuplicates = new List <string>();
                        string AttributeConcatenated;

                        foreach (PPartnerLocationRow partnerlocation in PPartnersLocationsDT.Rows)
                        {
                            bool primaryAddress =
                                (bestAddress.LocationKey == partnerlocation.LocationKey && bestAddress.SiteKey == partnerlocation.SiteKey);
                            bool currentAddress = (((int)partnerlocation[PARTNERLOCATION_ICON_COLUMN]) == 1);
                            bool businessAddress = (partnerlocation.LocationType == "BUSINESS" || partnerlocation.LocationType == "FIELD");
                            // TODO: avoid duplicate entries with the same type

                            if (!partnerlocation.IsEmailAddressNull())
                            {
                                PPartnerAttributeRow partnerattribute = partnerattributes.NewRowTyped();
                                partnerattribute.Sequence = SequenceNumber++;
                                partnerattribute.PartnerKey = partnerlocation.PartnerKey;
                                partnerattribute.Value = partnerlocation.EmailAddress;
                                partnerattribute.AttributeType = MPartnerConstants.ATTR_TYPE_EMAIL;
                                partnerattribute.Current = currentAddress;
                                partnerattribute.Primary = primaryAddress;
                                partnerattribute.Index = IndexEmail++;
                                partnerattribute.Specialised = businessAddress;
                                partnerattribute.NoLongerCurrentFrom = partnerlocation.DateGoodUntil;

                                AttributeConcatenated = ConcatPartnerAttributes(partnerattribute);

                                if (!AvoidDuplicates.Contains(AttributeConcatenated))
                                {
                                    partnerattributes.Rows.Add(partnerattribute);
                                    AvoidDuplicates.Add(AttributeConcatenated);
                                }
                                else
                                {
                                    TLogging.Log("dropping duplicate " + AttributeConcatenated);
                                }

                                partnerlocation.SetEmailAddressNull();
                            }

                            if (!partnerlocation.IsTelephoneNumberNull())
                            {
                                if (!partnerlocation.IsExtensionNull())
                                {
                                    partnerlocation.TelephoneNumber += "-" + partnerlocation.Extension;
                                }

                                PPartnerAttributeRow partnerattribute = partnerattributes.NewRowTyped();
                                partnerattribute.Sequence = SequenceNumber++;
                                partnerattribute.PartnerKey = partnerlocation.PartnerKey;
                                partnerattribute.Value = partnerlocation.TelephoneNumber;
                                partnerattribute.AttributeType = MPartnerConstants.ATTR_TYPE_PHONE;
                                partnerattribute.Current = currentAddress;
                                partnerattribute.Primary = primaryAddress;
                                partnerattribute.Index = IndexPhone++;
                                partnerattribute.Specialised = businessAddress;
                                partnerattribute.NoLongerCurrentFrom = partnerlocation.DateGoodUntil;

                                AttributeConcatenated = ConcatPartnerAttributes(partnerattribute);

                                if (!AvoidDuplicates.Contains(AttributeConcatenated))
                                {
                                    partnerattributes.Rows.Add(partnerattribute);
                                    AvoidDuplicates.Add(AttributeConcatenated);
                                }
                                else
                                {
                                    TLogging.Log("dropping duplicate " + AttributeConcatenated);
                                }

                                partnerlocation.SetTelephoneNumberNull();
                            }

                            if (!partnerlocation.IsFaxNumberNull())
                            {
                                if (!partnerlocation.IsFaxExtensionNull())
                                {
                                    partnerlocation.FaxNumber += "-" + partnerlocation.FaxExtension;
                                }

                                PPartnerAttributeRow partnerattribute = partnerattributes.NewRowTyped();
                                partnerattribute.Sequence = SequenceNumber++;
                                partnerattribute.PartnerKey = partnerlocation.PartnerKey;
                                partnerattribute.Value = partnerlocation.FaxNumber;
                                partnerattribute.AttributeType = MPartnerConstants.ATTR_TYPE_FAX;
                                partnerattribute.Current = currentAddress;
                                partnerattribute.Primary = primaryAddress;
                                partnerattribute.Index = IndexFax++;
                                partnerattribute.Specialised = businessAddress;
                                partnerattribute.NoLongerCurrentFrom = partnerlocation.DateGoodUntil;

                                AttributeConcatenated = ConcatPartnerAttributes(partnerattribute);

                                if (!AvoidDuplicates.Contains(AttributeConcatenated))
                                {
                                    partnerattributes.Rows.Add(partnerattribute);
                                    AvoidDuplicates.Add(AttributeConcatenated);
                                }
                                else
                                {
                                    TLogging.Log("dropping duplicate " + AttributeConcatenated);
                                }

                                partnerlocation.SetFaxNumberNull();
                            }

                            if (!partnerlocation.IsAlternateTelephoneNull())
                            {
                                PPartnerAttributeRow partnerattribute = partnerattributes.NewRowTyped();
                                partnerattribute.Sequence = SequenceNumber++;
                                partnerattribute.PartnerKey = partnerlocation.PartnerKey;
                                partnerattribute.Value = partnerlocation.AlternateTelephone;
                                partnerattribute.AttributeType = MPartnerConstants.ATTR_TYPE_PHONE;
                                partnerattribute.Current = currentAddress;
                                partnerattribute.Primary = primaryAddress;
                                partnerattribute.Index = IndexPhone++;
                                partnerattribute.Specialised = businessAddress;
                                partnerattribute.NoLongerCurrentFrom = partnerlocation.DateGoodUntil;

                                AttributeConcatenated = ConcatPartnerAttributes(partnerattribute);

                                if (!AvoidDuplicates.Contains(AttributeConcatenated))
                                {
                                    partnerattributes.Rows.Add(partnerattribute);
                                    AvoidDuplicates.Add(AttributeConcatenated);
                                }
                                else
                                {
                                    TLogging.Log("dropping duplicate " + AttributeConcatenated);
                                }

                                partnerlocation.SetAlternateTelephoneNull();
                            }

                            if (!partnerlocation.IsMobileNumberNull())
                            {
                                PPartnerAttributeRow partnerattribute = partnerattributes.NewRowTyped();
                                partnerattribute.Sequence = SequenceNumber++;
                                partnerattribute.PartnerKey = partnerlocation.PartnerKey;
                                partnerattribute.Value = partnerlocation.MobileNumber;
                                partnerattribute.AttributeType = MPartnerConstants.ATTR_TYPE_MOBILE_PHONE;
                                partnerattribute.Current = currentAddress;
                                partnerattribute.Primary = primaryAddress;
                                partnerattribute.Index = IndexMobile++;
                                partnerattribute.Specialised = businessAddress;
                                partnerattribute.NoLongerCurrentFrom = partnerlocation.DateGoodUntil;

                                AttributeConcatenated = ConcatPartnerAttributes(partnerattribute);

                                if (!AvoidDuplicates.Contains(AttributeConcatenated))
                                {
                                    partnerattributes.Rows.Add(partnerattribute);
                                    AvoidDuplicates.Add(AttributeConcatenated);
                                }
                                else
                                {
                                    TLogging.Log("dropping duplicate " + AttributeConcatenated);
                                }

                                partnerlocation.SetMobileNumberNull();
                            }

                            if (!partnerlocation.IsUrlNull())
                            {
                                PPartnerAttributeRow partnerattribute = partnerattributes.NewRowTyped();
                                partnerattribute.Sequence = SequenceNumber++;
                                partnerattribute.PartnerKey = partnerlocation.PartnerKey;
                                partnerattribute.Value = partnerlocation.Url;
                                partnerattribute.AttributeType = MPartnerConstants.ATTR_TYPE_WEBSITE;
                                partnerattribute.Current = currentAddress;
                                partnerattribute.Primary = primaryAddress;
                                partnerattribute.Index = IndexUrl++;
                                partnerattribute.Specialised = businessAddress;
                                partnerattribute.NoLongerCurrentFrom = partnerlocation.DateGoodUntil;

                                AttributeConcatenated = ConcatPartnerAttributes(partnerattribute);

                                if (!AvoidDuplicates.Contains(AttributeConcatenated))
                                {
                                    partnerattributes.Rows.Add(partnerattribute);
                                    AvoidDuplicates.Add(AttributeConcatenated);
                                }
                                else
                                {
                                    TLogging.Log("dropping duplicate " + AttributeConcatenated);
                                }

                                partnerlocation.SetUrlNull();
                            }
                        }
                    }

                    PPartnerLocationAccess.SubmitChanges(partnerlocations, SubmitChangesTransaction);
                    PPartnerAttributeAccess.SubmitChanges(partnerattributes, SubmitChangesTransaction);
                    SubmissionResult = TSubmitChangesResult.scrOK;
                });
            return true;
        }

        /// <summary>
        /// Creates an instance of a 'cut-down' PPartnerLocation Table that will be used for storing partial
        /// to-be-imported p_partner_location record information. It will also be used for the 'Best Address'
        /// determination, which is important for the Contact Details migration.
        /// </summary>
        /// <returns>Instance of a 'cut-down' PPartnerLocation Table that will be used for 'Best Address' determination.</returns>
        public static DataTable GetNewPPartnerLocationTableInstance()
        {
            DataTable ReturnValue = new PPartnerLocationTable();
            DataColumn IconForSortingCol;

            // Add special DataColumns that are needed for the 'Best Address' calculation
            ReturnValue.Columns.Add(new System.Data.DataColumn(PARTNERLOCATION_ICON_COLUMN, typeof(Int32)));

            // Add a 'Calculated Column' for sorting
            IconForSortingCol = new DataColumn();
            IconForSortingCol.DataType = System.Type.GetType("System.Int32");
            IconForSortingCol.ColumnName = "Icon_For_Sorting";
            IconForSortingCol.Expression = "IIF(Icon = 1, 2, IIF(Icon = 2, 1, 3))"; // exchanges instead of Icon=1 we get Icon=2
            ReturnValue.Columns.Add(IconForSortingCol);

            return ReturnValue;
        }
    }
}