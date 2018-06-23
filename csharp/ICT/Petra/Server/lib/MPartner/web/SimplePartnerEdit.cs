//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2018 by OM International
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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Xml;
using System.IO;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Server.MPartner.Partner.UIConnectors;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// functions for creating new partners and to edit partners
    /// </summary>
    public class TSimplePartnerEditWebConnector
    {
        /// <summary>
        /// get a partner key for a new partner
        /// </summary>
        /// <param name="AFieldPartnerKey">can be -1, then the default site key is used</param>
        [RequireModulePermission("PTNRUSER")]
        public static Int64 NewPartnerKey(Int64 AFieldPartnerKey)
        {
            Int64 NewPartnerKey = TNewPartnerKey.GetNewPartnerKey(AFieldPartnerKey);

            TNewPartnerKey.SubmitNewPartnerKey(NewPartnerKey - NewPartnerKey % 1000000, NewPartnerKey, ref NewPartnerKey);
            return NewPartnerKey;
        }

        /// <summary>
        /// return the dataset for a new partner
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerEditTDS CreateNewPartner(
            string APartnerClass,
            out List<string> ASubscriptions,
            out List<string> APartnerTypes,
            out string ADefaultEmailAddress,
            out string ADefaultPhoneMobile,
            out string ADefaultPhoneLandline)
        {
            TPartnerEditUIConnector partneredit = new TPartnerEditUIConnector();
            string TmpSiteCountryCode;

            PartnerEditTDS MainDS = partneredit.GetDataNewPartner(
                -1,
                -1,
                SharedTypes.PartnerClassStringToEnum(APartnerClass),
                String.Empty,
                String.Empty,
                false,
                -1,
                -1,
                -1,
                out TmpSiteCountryCode);

            APartnerTypes = new List<string>();
            ASubscriptions = new List<string>();
            ADefaultEmailAddress = String.Empty;
            ADefaultPhoneMobile = String.Empty;
            ADefaultPhoneLandline = String.Empty;

            return MainDS;
        }

        /// <summary>
        /// return the existing data of a partner
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerEditTDS GetPartnerDetails(Int64 APartnerKey,
            out List<string> ASubscriptions,
            out List<string> APartnerTypes,
            out string ADefaultEmailAddress,
            out string ADefaultPhoneMobile,
            out string ADefaultPhoneLandline)
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();
            List<string> Subscriptions = new List<string>();
            List<string> PartnerTypes = new List<string>();
            string DefaultEmailAddress = String.Empty;
            string DefaultPhoneMobile = String.Empty;
            string DefaultPhoneLandline = String.Empty;

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref Transaction,
                delegate
                {
                    PPartnerAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);

                    if (MainDS.PPartner.Rows.Count > 0)
                    {
                        switch (MainDS.PPartner[0].PartnerClass)
                        {
                            case MPartnerConstants.PARTNERCLASS_FAMILY:
                                PFamilyAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                                break;

                            case MPartnerConstants.PARTNERCLASS_PERSON:
                                PPersonAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                                break;

                            case MPartnerConstants.PARTNERCLASS_CHURCH:
                                PChurchAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                                break;

                            case MPartnerConstants.PARTNERCLASS_ORGANISATION:
                                POrganisationAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                                break;

                            case MPartnerConstants.PARTNERCLASS_BANK:
                                PBankAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                                break;

                            case MPartnerConstants.PARTNERCLASS_UNIT:
                                PUnitAccess.LoadByPrimaryKey(MainDS, APartnerKey, Transaction);
                                break;
                        }

                        if (true)
                        {
                            // don't load p_partner_location for the moment, because we have custom fields duplicating p_location.
                            // those custom fields need to be set, then we don't need to deliver p_location
                            // PPartnerLocationAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                            PLocationAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                        }

                        if (true)
                        {
                            PPartnerRelationshipAccess.LoadViaPPartnerPartnerKey(MainDS, APartnerKey, Transaction);
                        }

                        if (true)
                        {
                            PPublicationAccess.LoadAll(MainDS, Transaction);
                            PSubscriptionAccess.LoadViaPPartnerPartnerKey(MainDS, APartnerKey, Transaction);

                            foreach(PSubscriptionRow subscription in MainDS.PSubscription.Rows)
                            {
                                Subscriptions.Add(subscription.PublicationCode);
                            }
                        }

                        PPartnerStatusAccess.LoadAll(MainDS, Transaction);
                        PTypeAccess.LoadAll(MainDS, Transaction);
                        PPartnerTypeAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);

                        foreach(PPartnerTypeRow partnertype in MainDS.PPartnerType.Rows)
                        {
                            PartnerTypes.Add(partnertype.TypeCode);
                        }

                        PPartnerAttributeAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);

                        foreach(PPartnerAttributeRow partnerattr in MainDS.PPartnerAttribute.Rows)
                        {
                            if (partnerattr.AttributeType == MPartnerConstants.ATTR_TYPE_EMAIL)
                            {
                                DefaultEmailAddress = partnerattr.Value;
                            }
                            else if (partnerattr.AttributeType == MPartnerConstants.ATTR_TYPE_PHONE)
                            {
                                DefaultPhoneLandline = partnerattr.Value;
                            }
                            else if (partnerattr.AttributeType == MPartnerConstants.ATTR_TYPE_MOBILE_PHONE)
                            {
                                DefaultPhoneMobile = partnerattr.Value;
                            }
                        }
                    }
                });

            APartnerTypes = PartnerTypes;
            ASubscriptions = Subscriptions;
            ADefaultEmailAddress = DefaultEmailAddress;
            ADefaultPhoneMobile = DefaultPhoneMobile;
            ADefaultPhoneLandline = DefaultPhoneLandline;

            return MainDS;
        }

        /// <summary>
        /// Return the existing data of a partner, including address information and primary phone and email
        /// </summary>
        /// <param name="APartnerKey">The partner</param>
        /// <param name="AWithSubscriptions">Option to return subscriptions information</param>
        /// <param name="AWithRelationships">Option to return relationships information</param>
        /// <param name="APrimaryPhoneNumber">Returns the primary phone</param>
        /// <param name="APrimaryEmailAddress">Returns the primary email</param>
        /// <returns>A PartnerEdit data set</returns>
        [RequireModulePermission("PTNRUSER")]
        public static PartnerEditTDS GetPartnerDetails(Int64 APartnerKey, bool AWithSubscriptions, bool AWithRelationships,
            out string APrimaryPhoneNumber, out string APrimaryEmailAddress)
        {
            // Call the standard method including address details
            List<string> Dummy1, Dummy2;
            string Dummy3, Dummy4, Dummy5;
            PartnerEditTDS MainDS = GetPartnerDetails(APartnerKey, out Dummy1, out Dummy2, out Dummy3, out Dummy4, out Dummy5);

            // Now get the primary email and phone
            PPartnerAttributeTable attributeTable = TContactDetailsAggregate.GetPartnersContactDetailAttributes(APartnerKey);

            Calculations.GetPrimaryEmailAndPrimaryPhone(attributeTable, out APrimaryPhoneNumber, out APrimaryEmailAddress);

            return MainDS;
        }

        /// <summary>
        /// store the imported partner
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static void ImportPartner(PartnerEditTDS AMainDS)
        {
            TDBTransaction ReadTransaction = null;
            TDBTransaction SubmitChangesTransaction = null;
            bool SubmissionOK = false;
            bool ImportDefaultAcquCodeExists = false;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref ReadTransaction,
                delegate
                {
                    ImportDefaultAcquCodeExists = PAcquisitionAccess.Exists(MPartnerConstants.PARTNERIMPORT_AQUISITION_DEFAULT,
                        ReadTransaction);
                });

            if (!ImportDefaultAcquCodeExists)
            {
                PAcquisitionTable AcqTable = new PAcquisitionTable();
                PAcquisitionRow row = AcqTable.NewRowTyped();
                row.AcquisitionCode = MPartnerConstants.PARTNERIMPORT_AQUISITION_DEFAULT;
                row.AcquisitionDescription = Catalog.GetString("Imported Data");
                AcqTable.Rows.Add(row);

                DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref SubmitChangesTransaction, ref SubmissionOK,
                    delegate
                    {
                        PAcquisitionAccess.SubmitChanges(AcqTable, SubmitChangesTransaction);

                        SubmissionOK = true;
                    });
            }

            PartnerEditTDSAccess.SubmitChanges(AMainDS);
        }

        /// <summary>
        /// store the currently edited partner
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool SavePartner(PartnerEditTDS AMainDS,
            List<string> ASubscriptions,
            List<string> APartnerTypes)
        {
            List<string> Dummy1, Dummy2;
            string Dummy3, Dummy4, Dummy5;
            PartnerEditTDS SaveDS = GetPartnerDetails(AMainDS.PPartner[0].PartnerKey, out Dummy1, out Dummy2, out Dummy3, out Dummy4, out Dummy5);

            DataUtilities.CopyDataSet(AMainDS, SaveDS);

            List<string> ExistingPartnerTypes = new List<string>();
            foreach (PPartnerTypeRow partnertype in SaveDS.PPartnerType.Rows)
            {
                if (!APartnerTypes.Contains(partnertype.TypeCode))
                {
                    partnertype.Delete();
                }
                else
                {
                    ExistingPartnerTypes.Add(partnertype.TypeCode);
                }
            }

            // add new partner types
            foreach (string partnertype in APartnerTypes)
            {
                if (!ExistingPartnerTypes.Contains(partnertype))
                {
                    PPartnerTypeRow partnertypeRow = SaveDS.PPartnerType.NewRowTyped();
                    partnertypeRow.PartnerKey = AMainDS.PPartner[0].PartnerKey;
                    partnertypeRow.TypeCode = partnertype;
                    SaveDS.PPartnerType.Rows.Add(partnertypeRow);
                }
            }

            List<string> ExistingSubscriptions = new List<string>();
            foreach (PSubscriptionRow subscription in SaveDS.PSubscription.Rows)
            {
                if (!ASubscriptions.Contains(subscription.PublicationCode))
                {
                    subscription.Delete();
                }
                else
                {
                    ExistingSubscriptions.Add(subscription.PublicationCode);
                }
            }

            // add new subscriptions
            foreach (string subscription in ASubscriptions)
            {
                if (!ExistingSubscriptions.Contains(subscription))
                {
                    PSubscriptionRow subscriptionRow = SaveDS.PSubscription.NewRowTyped();
                    subscriptionRow.PartnerKey = AMainDS.PPartner[0].PartnerKey;
                    subscriptionRow.PublicationCode = subscription;
                    subscriptionRow.ReasonSubsGivenCode = "FREE";
                    SaveDS.PSubscription.Rows.Add(subscriptionRow);
                }
            }

            // TODO: either reuse Partner Edit UIConnector
            // or check for changed partner key, or changed Partner Class, etc.

            // set Partner Short Name
            if (SaveDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
            {
                SaveDS.PPartner[0].PartnerShortName = 
                    Calculations.DeterminePartnerShortName(
                        SaveDS.PPerson[0].FamilyName,
                        SaveDS.PPerson[0].Title,
                        SaveDS.PPerson[0].FirstName,
                        SaveDS.PPerson[0].MiddleName1);
            }
            else if (SaveDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
            {
                SaveDS.PPartner[0].PartnerShortName = 
                    Calculations.DeterminePartnerShortName(
                        SaveDS.PFamily[0].FamilyName,
                        SaveDS.PFamily[0].Title,
                        SaveDS.PFamily[0].FirstName);
            }
            else if (SaveDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
            {
                SaveDS.PPartner[0].PartnerShortName = SaveDS.PUnit[0].UnitName;
            }
            else if (SaveDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_ORGANISATION)
            {
                SaveDS.PPartner[0].PartnerShortName = SaveDS.POrganisation[0].OrganisationName;
            }
            else if (SaveDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_BANK)
            {
                SaveDS.PPartner[0].PartnerShortName = SaveDS.PBank[0].BranchName;
            }

            // TODO: check if location 0 (no address) was changed. we don't want to overwrite that
            // alternative: check if somebody else uses that location, and split the locations. or ask if others should be updated???
            if (SaveDS.PLocation[0].RowState == DataRowState.Modified && SaveDS.PLocation[0].LocationKey == 0)
            {
                TLogging.Log("we cannot update addresses of people with location 0");
                return false;
            }

            PartnerEditTDSAccess.SubmitChanges(SaveDS);

            return true;
        }
    }
}
