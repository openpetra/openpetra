//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2021 by OM International
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
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MCommon.Data.Access;
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
        public static Int64 NewPartnerKey(Int64 AFieldPartnerKey = -1)
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
            Int64 SiteKey = DomainManager.GSiteKey;

            PartnerEditTDS MainDS = partneredit.GetDataNewPartner(
                SiteKey,
                NewPartnerKey(),
                SharedTypes.PartnerClassStringToEnum(APartnerClass),
                String.Empty,
                String.Empty,
                false,
                -1,
                -1,
                -1,
                out TmpSiteCountryCode);

            MainDS.PPartner[0].ReceiptLetterFrequency = "Annual";

            PLocationRow location = MainDS.PLocation.NewRowTyped();
            location.SiteKey = SiteKey;
            // TODO: read country code from SystemDefaults table
            location.CountryCode = "DE";
            location.LocationKey = -1;
            MainDS.PLocation.Rows.Add(location);

            TDBTransaction Transaction = new TDBTransaction();

            DBAccess.ReadTransaction( ref Transaction,
                delegate
                {
                    PCountryAccess.LoadAll(MainDS, Transaction);
                    PPublicationAccess.LoadAll(MainDS, Transaction);
                    PPartnerStatusAccess.LoadAll(MainDS, Transaction);
                    PPartnerClassesAccess.LoadByPrimaryKey(MainDS, "FAMILY", Transaction);
                    PPartnerClassesAccess.LoadByPrimaryKey(MainDS, "ORGANISATION", Transaction);

                    PTypeRow templateRow = MainDS.PType.NewRowTyped();
                    templateRow.SystemType = false;
                    templateRow.SetTypeDeletableNull();
                    templateRow.SetDateCreatedNull();
                    PTypeAccess.LoadUsingTemplate(MainDS, templateRow, Transaction);
                });

            APartnerTypes = new List<string>();
            ASubscriptions = new List<string>();
            ADefaultEmailAddress = String.Empty;
            ADefaultPhoneMobile = String.Empty;
            ADefaultPhoneLandline = String.Empty;

            return MainDS;
        }

        private static Int64 GetOwnPartnerKeyForUser()
        {
            Int64 PartnerKey = -1;
            TDBTransaction Transaction = new TDBTransaction();

            DBAccess.ReadTransaction( ref Transaction,
                delegate
                {
                    string sql = "SELECT p_partner_key_n FROM PUB_s_user WHERE s_user_id_c = ?";

                    OdbcParameter[] parameters = new OdbcParameter[1];
                    parameters[0] = new OdbcParameter("UserID", OdbcType.VarChar);
                    parameters[0].Value = UserInfo.GetUserInfo().UserID;

                    DataTable result = Transaction.DataBaseObj.SelectDT(sql, "user", Transaction, parameters);

                    if (result.Rows.Count == 1)
                    {
                        PartnerKey = Convert.ToInt64(result.Rows[0][0]);
                    }
                });

            if (PartnerKey == -1)
            {
                throw new Exception("cannot find partner for this user");
            }

            return PartnerKey;
        }

        /// <summary>
        /// return the existing data of my own partner record for self service
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PARTNERSELFSERVICE")]
        public static PartnerEditTDS GetPartnerDetailsSelfService(
            out List<string> ASubscriptions,
            out List<string> APartnerTypes,
            out string ADefaultEmailAddress,
            out string ADefaultPhoneMobile,
            out string ADefaultPhoneLandline)
        {
            return GetPartnerDetails(GetOwnPartnerKeyForUser(),
                out ASubscriptions,
                out APartnerTypes,
                out ADefaultEmailAddress,
                out ADefaultPhoneMobile,
                out ADefaultPhoneLandline);
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

            TDBTransaction Transaction = new TDBTransaction();

            DBAccess.ReadTransaction( ref Transaction,
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
                            PPartnerLocationAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                            PLocationAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                        }

                        if (true)
                        {
                            PPartnerRelationshipAccess.LoadViaPPartnerPartnerKey(MainDS, APartnerKey, Transaction);
                        }

                        if (true)
                        {
                            PCountryAccess.LoadAll(MainDS, Transaction);
                        }

                        if (true)
                        {
                            PPartnerClassesAccess.LoadByPrimaryKey(MainDS, "FAMILY", Transaction);
                            PPartnerClassesAccess.LoadByPrimaryKey(MainDS, "ORGANISATION", Transaction);
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

                        if (true)
                        {
                            PBankingDetailsAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                            PPartnerBankingDetailsAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);
                            PBankingDetailsUsageAccess.LoadViaPPartner(MainDS, APartnerKey, Transaction);

                            foreach(PartnerEditTDSPBankingDetailsRow banking in MainDS.PBankingDetails.Rows)
                            {
                                PBankAccess.LoadByPrimaryKey(MainDS, banking.BankKey, Transaction);
                                banking.Bic = MainDS.PBank[0].Bic;
                                banking.BranchName = MainDS.PBank[0].BranchName;
                                banking.Iban = TSEPAWriterDirectDebit.FormatIBAN(banking.Iban);
                                MainDS.PBank.Rows.Clear();
                            }

                            foreach (PartnerEditTDSPBankingDetailsRow bd in MainDS.PBankingDetails.Rows)
                            {
                                bd.MainAccount =
                                    (MainDS.PBankingDetailsUsage.Rows.Find(
                                         new object[] { APartnerKey, bd.BankingDetailsKey, MPartnerConstants.BANKINGUSAGETYPE_MAIN }) != null);
                            }

                            MainDS.PBankingDetailsUsage.Rows.Clear();
                        }

                        PPartnerStatusAccess.LoadAll(MainDS, Transaction);
                        PTypeRow templateRow = MainDS.PType.NewRowTyped();
                        templateRow.SystemType = false;
                        templateRow.SetTypeDeletableNull();
                        templateRow.SetDateCreatedNull();
                        PTypeAccess.LoadUsingTemplate(MainDS, templateRow, Transaction);
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

            TDBTransaction ReadTransaction = new TDBTransaction();
            string PrimaryPhoneNumber = String.Empty;
            string PrimaryEmailAddress = String.Empty;

            DBAccess.ReadTransaction( ref ReadTransaction,
                delegate
                {
                    // Now get the primary email and phone
                    PPartnerAttributeTable attributeTable = TContactDetailsAggregate.GetPartnersContactDetailAttributes(APartnerKey);

                    Calculations.GetPrimaryEmailAndPrimaryPhone(ReadTransaction, attributeTable, out PrimaryPhoneNumber, out PrimaryEmailAddress);
                });

            APrimaryPhoneNumber = PrimaryPhoneNumber;
            APrimaryEmailAddress = PrimaryEmailAddress;

            return MainDS;
        }

        /// <summary>
        /// store the imported partner
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static void ImportPartner(PartnerEditTDS AMainDS)
        {
            TDBTransaction ReadTransaction = new TDBTransaction();
            TDBTransaction SubmitChangesTransaction = new TDBTransaction();
            bool SubmissionOK = false;
            bool ImportDefaultAcquCodeExists = false;

            DBAccess.ReadTransaction( ref ReadTransaction,
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

                DBAccess.WriteTransaction(ref SubmitChangesTransaction, ref SubmissionOK,
                    delegate
                    {
                        PAcquisitionAccess.SubmitChanges(AcqTable, SubmitChangesTransaction);

                        SubmissionOK = true;
                    });
            }

            PartnerEditTDSAccess.SubmitChanges(AMainDS);
        }

        /// <summary>
        /// return the existing data of my own partner record for self service
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PARTNERSELFSERVICE")]
        public static bool SavePartnerSelfService(PartnerEditTDS AMainDS,
            List<string> ASubscriptions,
            List<string> APartnerTypes,
            bool ASendMail,
            string ADefaultEmailAddress,
            string ADefaultPhoneMobile,
            string ADefaultPhoneLandline,
            out TVerificationResultCollection AVerificationResult)
        {
            if (AMainDS.PPartner[0].PartnerKey != GetOwnPartnerKeyForUser())
            {
                throw new Exception("No permission to edit this partner");
            }

            return SavePartner(AMainDS,
                ASubscriptions,
                APartnerTypes,
                new List<string>(),
                ASendMail,
                ADefaultEmailAddress,
                ADefaultPhoneMobile,
                ADefaultPhoneLandline,
                out AVerificationResult);
        }

        /// <summary>
        /// store the currently edited partner
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool SavePartner(PartnerEditTDS AMainDS,
            List<string> ASubscriptions,
            List<string> APartnerTypes,
            List<string> AChanges,
            bool ASendMail,
            string ADefaultEmailAddress,
            string ADefaultPhoneMobile,
            string ADefaultPhoneLandline,
            out TVerificationResultCollection AVerificationResult)
        {
            List<string> Dummy1, Dummy2;
            string Dummy3, Dummy4, Dummy5;
            PartnerEditTDS SaveDS;
            AVerificationResult = new TVerificationResultCollection();

            if (AMainDS.PPartner[0].ModificationId == DateTime.MinValue)
            {
                // this is a new partner
                SaveDS = AMainDS;

                if (SaveDS.PPartner[0].PartnerKey == -1)
                {
                    SaveDS.PPartner[0].PartnerKey = NewPartnerKey();
                }

                if (SaveDS.PFamily.Count > 0)
                {
                    SaveDS.PFamily[0].PartnerKey = SaveDS.PPartner[0].PartnerKey;
                }
                else if (SaveDS.PPerson.Count > 0)
                {
                    SaveDS.PPerson[0].PartnerKey = SaveDS.PPartner[0].PartnerKey;
                }
                else if (SaveDS.PChurch.Count > 0)
                {
                    SaveDS.PChurch[0].PartnerKey = SaveDS.PPartner[0].PartnerKey;
                }
                else if (SaveDS.POrganisation.Count > 0)
                {
                    SaveDS.POrganisation[0].PartnerKey = SaveDS.PPartner[0].PartnerKey;
                }

                PPartnerLocationRow partnerlocation = SaveDS.PPartnerLocation.NewRowTyped();
                partnerlocation.PartnerKey = SaveDS.PPartner[0].PartnerKey;
                partnerlocation.LocationKey = SaveDS.PLocation[0].LocationKey;
                partnerlocation.SiteKey = SaveDS.PLocation[0].SiteKey;
                partnerlocation.SendMail = ASendMail;
                SaveDS.PPartnerLocation.Rows.Add(partnerlocation);
            }
            else
            {
                SaveDS = GetPartnerDetails(AMainDS.PPartner[0].PartnerKey, out Dummy1, out Dummy2, out Dummy3, out Dummy4, out Dummy5);
                DataUtilities.CopyDataSet(AMainDS, SaveDS);
            }

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

            bool foundDefaultEmailAddress = false;
            bool foundDefaultPhoneLandLine = false;
            bool foundDefaultMobileLandLine = false;

            foreach(PPartnerAttributeRow partnerattr in SaveDS.PPartnerAttribute.Rows)
            {
                if (partnerattr.AttributeType == MPartnerConstants.ATTR_TYPE_EMAIL)
                {
                    partnerattr.Value = ADefaultEmailAddress;
                    foundDefaultEmailAddress = true;
                }
                else if (partnerattr.AttributeType == MPartnerConstants.ATTR_TYPE_PHONE)
                {
                    partnerattr.Value = ADefaultPhoneLandline;
                    foundDefaultPhoneLandLine = true;
                }
                else if (partnerattr.AttributeType == MPartnerConstants.ATTR_TYPE_MOBILE_PHONE)
                {
                    partnerattr.Value = ADefaultPhoneMobile;
                    foundDefaultMobileLandLine = true;
                }
            }

            if (!foundDefaultEmailAddress)
            {
                PPartnerAttributeRow partnerattr = SaveDS.PPartnerAttribute.NewRowTyped();
                partnerattr.PartnerKey = SaveDS.PPartner[0].PartnerKey;
                partnerattr.AttributeType = MPartnerConstants.ATTR_TYPE_EMAIL;
                partnerattr.Value = ADefaultEmailAddress;
                partnerattr.Index = 0;
                SaveDS.PPartnerAttribute.Rows.Add(partnerattr);
            }

            if (!foundDefaultPhoneLandLine)
            {
                PPartnerAttributeRow partnerattr = SaveDS.PPartnerAttribute.NewRowTyped();
                partnerattr.PartnerKey = SaveDS.PPartner[0].PartnerKey;
                partnerattr.AttributeType = MPartnerConstants.ATTR_TYPE_PHONE;
                partnerattr.Value = ADefaultPhoneLandline;
                partnerattr.Index = 0;
                SaveDS.PPartnerAttribute.Rows.Add(partnerattr);
            }

            if (!foundDefaultMobileLandLine)
            {
                PPartnerAttributeRow partnerattr = SaveDS.PPartnerAttribute.NewRowTyped();
                partnerattr.PartnerKey = SaveDS.PPartner[0].PartnerKey;
                partnerattr.AttributeType = MPartnerConstants.ATTR_TYPE_MOBILE_PHONE;
                partnerattr.Value = ADefaultPhoneMobile;
                partnerattr.Index = 0;
                SaveDS.PPartnerAttribute.Rows.Add(partnerattr);
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
                // check if we have a valid family name
                if (SaveDS.PFamily[0].FamilyName.Trim().Length == 0)
                {
                    AVerificationResult.Add(new TVerificationResult("error", "Please specify the family name", "",
                        "MaintainPartners.ErrMissingFamilyName", TResultSeverity.Resv_Critical));
                    return false;
                }

                // check if we have a valid title
                if (SaveDS.PFamily[0].Title.Trim().Length == 0)
                {
                    AVerificationResult.Add(new TVerificationResult("error", "Please specify the title", "",
                        "MaintainPartners.ErrMissingTitle", TResultSeverity.Resv_Critical));
                    return false;
                }

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
                // check if we have a valid organisation name
                if (SaveDS.POrganisation[0].OrganisationName.Trim().Length == 0)
                {
                    AVerificationResult.Add(new TVerificationResult("error", "Please specify the organisation name", "",
                        "MaintainPartners.ErrMissingOrganisationName", TResultSeverity.Resv_Critical));
                    return false;
                }

                SaveDS.PPartner[0].PartnerShortName = SaveDS.POrganisation[0].OrganisationName;
            }
            else if (SaveDS.PPartner[0].PartnerClass == MPartnerConstants.PARTNERCLASS_BANK)
            {
                SaveDS.PPartner[0].PartnerShortName = SaveDS.PBank[0].BranchName;
            }

            // change legacy addresses. create a new separate location for each partner
            if (SaveDS.PLocation[0].LocationKey == 0)
            {
                PLocationRow location = SaveDS.PLocation.NewRowTyped();
                DataUtilities.CopyAllColumnValues(SaveDS.PLocation[0], location); 
                location.SiteKey = DomainManager.GSiteKey;
                location.LocationKey = -1;
                SaveDS.PLocation.Rows.Clear();
                SaveDS.PLocation.Rows.Add(location);

                PPartnerLocationRow plocation = SaveDS.PPartnerLocation.NewRowTyped();
                DataUtilities.CopyAllColumnValues(SaveDS.PPartnerLocation[0], plocation); 
                plocation.LocationKey = -1;
                plocation.SiteKey = DomainManager.GSiteKey;
                SaveDS.PPartnerLocation[0].Delete();
                SaveDS.PPartnerLocation.Rows.Add(plocation);
            }

            // check if we have a valid country code
            if (SaveDS.PLocation[0].CountryCode.Trim().Length == 0)
            {
                AVerificationResult.Add(new TVerificationResult("error", "The country code is missing", TResultSeverity.Resv_Critical));
                return false;
            }

            TDBTransaction Transaction = new TDBTransaction();
            bool WrongCountryCode = false;

            DBAccess.ReadTransaction( ref Transaction,
                delegate
                {
                    WrongCountryCode = !PCountryAccess.Exists(SaveDS.PLocation[0].CountryCode, Transaction);
                });

            if (WrongCountryCode)
            {
                AVerificationResult.Add(new TVerificationResult("error", "The country code does not match a country", TResultSeverity.Resv_Critical));
                return false;
            }

            DataSet ResponseDS = new PartnerEditTDS();
            TPartnerEditUIConnector uiconnector = new TPartnerEditUIConnector(SaveDS.PPartner[0].PartnerKey);

            // we search in every possible changed list, and added the data_type to NeededChanges
            // everything in this list will be validated by TDataHistoryWebConnector.RegisterChanges
            // it throws an error if a needed change can't be validated 
            List<string> NeededChanges = new List<string>();

            foreach (PLocationRow Loc in SaveDS.PLocation.Rows)
            {
                if (Loc.RowState == DataRowState.Modified) { NeededChanges.Add("address"); } 
            }

            foreach (PPartnerAttributeRow Attr in SaveDS.PPartnerAttribute.Rows)
            {
                if (Attr.RowState == DataRowState.Modified) {

                    if (Attr.AttributeType == "E-Mail") { NeededChanges.Add("email address"); }
                    if (Attr.AttributeType == "Phone") { NeededChanges.Add("phone landline"); }
                    if (Attr.AttributeType == "Mobile Phone") { NeededChanges.Add("phone mobile"); }

                }
            }

            // only run if it's not a new user create call
            bool run_after_create = false;
            if (AMainDS.PPartner[0].ModificationId != DateTime.MinValue)
            {
                bool consent_success = TDataHistoryWebConnector.RegisterChanges(AChanges, NeededChanges);
                if (consent_success == false) {
                    AVerificationResult.Add(new TVerificationResult("error", "consent_error", "consent_error", TResultSeverity.Resv_Critical));
                    return false;
                }
            }
            else { run_after_create = true; }

            try
            {
                TSubmitChangesResult result = uiconnector.SubmitChanges(
                    ref SaveDS,
                    ref ResponseDS,
                    out AVerificationResult);

                if (run_after_create) {
                    // after user entry exists, then enter inital changes
                    TDataHistoryWebConnector.RegisterChanges(AChanges, new List<string>());
                }

                return result == TSubmitChangesResult.scrOK;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
                AVerificationResult.Add(new TVerificationResult("error", e.Message, TResultSeverity.Resv_Critical));
                return false;
            }
        }

        /// <summary>
        /// delete the partner
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool DeletePartner(Int64 APartnerKey,
            out TVerificationResultCollection AVerificationResult)
        {
            string ErrorMsg = String.Empty;

            if (!TPartnerWebConnector.CanPartnerBeDeleted(APartnerKey, out ErrorMsg))
            {
                AVerificationResult = new TVerificationResultCollection();
                AVerificationResult.Add(new TVerificationResult("error", ErrorMsg, TResultSeverity.Resv_Critical));
                return false;
            }

            return TPartnerWebConnector.DeletePartner(APartnerKey, out AVerificationResult);
        }

        /// <summary>
        /// delete all FAMILY and ORGANISATION partners. useful for initial import of contacts, trial and error.
        /// </summary>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool DeleteAllPartners(
            out TVerificationResultCollection AVerificationResult)
        {
            string ErrorMsg = String.Empty;
            TVerificationResultCollection VerificationResult = new TVerificationResultCollection();

            List<Int64> PartnersToDelete = new List<Int64>();

            TDBTransaction Transaction = new TDBTransaction();

            DBAccess.ReadTransaction( ref Transaction,
                delegate
                {
                    PPartnerTable partners = PPartnerAccess.LoadAll(Transaction);

                    foreach (PPartnerRow row in partners.Rows)
                    {
                        if (row.PartnerClass == "FAMILY" || row.PartnerClass == "ORGANISATION")
                        {
                            if (!TPartnerWebConnector.CanPartnerBeDeleted(row.PartnerKey, out ErrorMsg, Transaction.DataBaseObj))
                            {
                                VerificationResult.Add(new TVerificationResult("error", ErrorMsg + " " + row.PartnerKey.ToString(), TResultSeverity.Resv_Critical));
                                break;
                            }

                            PartnersToDelete.Add(row.PartnerKey);
                        }
                    }
                });

            if (VerificationResult.HasCriticalErrors)
            {
                AVerificationResult = new TVerificationResultCollection(VerificationResult);
                return false;
            }

            Transaction = new TDBTransaction();
            bool Submit = false;
            DBAccess.WriteTransaction( ref Transaction,
                ref Submit,
                delegate
                {
                    foreach (Int64 PartnerKey in PartnersToDelete)
                    {
                        if (!TPartnerWebConnector.DeletePartner(PartnerKey, out VerificationResult, Transaction.DataBaseObj))
                        {
                            break;
                        }
                    }

                    if (!VerificationResult.HasCriticalErrors)
                    {
                        Submit = true;
                    }
                });

            if (VerificationResult.HasCriticalErrors)
            {
                AVerificationResult = new TVerificationResultCollection(VerificationResult);
                return false;
            }

            AVerificationResult = new TVerificationResultCollection();
            return true;
        }

        /// this will use an external web service to check the IBAN, and get the BIC and the name of the bank
        [RequireModulePermission("PTNRUSER")]
        public static bool ValidateIBAN(string AIban, out string ABic, out string ABankName,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = new TVerificationResultCollection();
            ABic = String.Empty;
            ABankName = String.Empty;

            if ((AIban == null) || (AIban.Trim() == String.Empty))
            {
                AVerificationResult.Add(new TVerificationResult("error", "The IBAN is invalid", "",
                    "MaintainPartners.ErrInvalidIBAN", TResultSeverity.Resv_Critical));
                return false;
            }

            string IBANCheckURL = TAppSettingsManager.GetValue("IBANCheck.Url", "https://kontocheck.solidcharity.com/?iban=");
            string url = IBANCheckURL + AIban.Replace(" ", "");
            string result = THTTPUtils.ReadWebsite(url);

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(result);

                XmlNode IbanCheck = TXMLParser.GetChild(doc.DocumentElement, "iban");
                if (IbanCheck.InnerText == "0")
                {
                    AVerificationResult.Add(new TVerificationResult("error", "The IBAN is invalid", "",
                        "MaintainPartners.ErrInvalidIBAN", TResultSeverity.Resv_Critical));
                    return false;
                }

                XmlNode BankName = TXMLParser.GetChild(doc.DocumentElement, "bankname");
                XmlNode City = TXMLParser.GetChild(doc.DocumentElement, "city");
                ABankName = BankName.InnerText + ", " + City.InnerText;
                XmlNode BIC = TXMLParser.GetChild(doc.DocumentElement, "bic");
                ABic = BIC.InnerText;
            }
            catch (Exception)
            {
                TLogging.Log("Error validating IBAN: " + AIban.Replace(" ", ""));
                AVerificationResult.Add(new TVerificationResult("error", "The IBAN is invalid", "",
                    "MaintainPartners.ErrInvalidIBAN", TResultSeverity.Resv_Critical));
                return false;
            }

            return true;
        }

        /// Find or create a partner for the bank with the given BIC
        [NoRemoting]
        public static Int64 FindOrCreateBank(string ABIC, string ABankName)
        {
            Int64 PartnerKey = -1;
            TDBTransaction Transaction = new TDBTransaction();

            DBAccess.ReadTransaction( ref Transaction,
                delegate
                {
                    string sql = "SELECT p_partner_key_n FROM PUB_p_bank WHERE p_bic_c = ?";

                    OdbcParameter[] parameters = new OdbcParameter[1];
                    parameters[0] = new OdbcParameter("bic", OdbcType.VarChar);
                    parameters[0].Value = ABIC;

                    DataTable result = Transaction.DataBaseObj.SelectDT(sql, "bank", Transaction, parameters);

                    if (result.Rows.Count >= 1)
                    {
                        PartnerKey = Convert.ToInt64(result.Rows[0][0]);
                    }
                });

            if (PartnerKey != -1)
            {
                return PartnerKey;
            }

            // we need to create a bank with this BIC
            List<string> Subscriptions;
            List<string> PartnerTypes;
            string DefaultEmailAddress;
            string DefaultPhoneMobile;
            string DefaultPhoneLandline;

            PartnerEditTDS MainDS = CreateNewPartner(MPartnerConstants.PARTNERCLASS_BANK,
                out Subscriptions,
                out PartnerTypes,
                out DefaultEmailAddress,
                out DefaultPhoneMobile,
                out DefaultPhoneLandline);

            PBankRow bankRow = MainDS.PBank[0];
            bankRow.Bic = ABIC;
            bankRow.BranchName = ABankName;

            try
            {
                PartnerEditTDSAccess.SubmitChanges(MainDS);
                return bankRow.PartnerKey;
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }

            return -1;
        }

        /// <summary>
        /// this will return the bank accounts of the partner
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static bool GetBankAccounts(Int64 APartnerKey, out PartnerEditTDSPBankingDetailsTable PBankingDetails)
        {
            List<string> Dummy1, Dummy2;
            string Dummy3, Dummy4, Dummy5;
            PartnerEditTDS MainDS = GetPartnerDetails(APartnerKey, out Dummy1, out Dummy2, out Dummy3, out Dummy4, out Dummy5);

            PBankingDetails = new PartnerEditTDSPBankingDetailsTable(); // MainDS.PBankingDetails;
            PBankingDetails = MainDS.PBankingDetails;

            return true;
        }

        /// <summary>
        /// this will create, save and delete a bank account
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        public static bool MaintainBankAccounts(
            string action,
            Int32 ABankingDetailsKey,
            Int64 APartnerKey,
            string AAccountName,
            string AIban,
            out TVerificationResultCollection AVerificationResult)
        {
            List<string> Dummy1, Dummy2;
            string Dummy3, Dummy4, Dummy5;
            PartnerEditTDS MainDS = GetPartnerDetails(APartnerKey, out Dummy1, out Dummy2, out Dummy3, out Dummy4, out Dummy5);

            AVerificationResult = new TVerificationResultCollection();

            AIban = AIban.Replace(" ", "");
            string BIC = String.Empty;
            string BankName = String.Empty;
            if ((action == "create" || action == "edit"))
            {
                // Validate IBAN, and calculate the BIC
                if (!ValidateIBAN(AIban, out BIC, out BankName, out AVerificationResult))
                {
                    return false;
                }
            }

            if (action == "create")
            {
                PartnerEditTDSPBankingDetailsRow row = MainDS.PBankingDetails.NewRowTyped();
                row.BankingDetailsKey = -1;
                row.BankingType = 0; // BANK ACCOUNT
                row.AccountName = AAccountName;
                row.Iban = AIban;
                row.BankKey = FindOrCreateBank(BIC, BankName);
                row.MainAccount = MainDS.PBankingDetails.Count == 0;
                MainDS.PBankingDetails.Rows.Add(row);

                PPartnerBankingDetailsRow pdrow = MainDS.PPartnerBankingDetails.NewRowTyped();
                pdrow.PartnerKey = APartnerKey;
                pdrow.BankingDetailsKey = -1;
                MainDS.PPartnerBankingDetails.Rows.Add(pdrow);

                DataSet ResponseDS = new PartnerEditTDS();
                TPartnerEditUIConnector uiconnector = new TPartnerEditUIConnector(APartnerKey);

                try
                {
                    TSubmitChangesResult result = uiconnector.SubmitChanges(
                        ref MainDS,
                        ref ResponseDS,
                        out AVerificationResult);

                    return result == TSubmitChangesResult.scrOK;
                }
                catch (Exception e)
                {
                    TLogging.Log(e.ToString());
                    AVerificationResult.Add(new TVerificationResult("error", e.Message, TResultSeverity.Resv_Critical));
                    return false;
                }
            }
            else if (action == "edit")
            {
                foreach (PartnerEditTDSPBankingDetailsRow row in MainDS.PBankingDetails.Rows)
                {
                    if (row.BankingDetailsKey == ABankingDetailsKey)
                    {
                        row.AccountName = AAccountName;
                        row.Iban = AIban;
                        row.BankKey = FindOrCreateBank(BIC, BankName);
                    }
                }

                DataSet ResponseDS = new PartnerEditTDS();
                TPartnerEditUIConnector uiconnector = new TPartnerEditUIConnector(APartnerKey);

                try
                {
                    TSubmitChangesResult result = uiconnector.SubmitChanges(
                        ref MainDS,
                        ref ResponseDS,
                        out AVerificationResult);

                    return result == TSubmitChangesResult.scrOK;
                }
                catch (Exception e)
                {
                    TLogging.Log(e.ToString());
                    AVerificationResult.Add(new TVerificationResult("error", e.Message, TResultSeverity.Resv_Critical));
                    return false;
                }
            }
            else if (action == "delete")
            {
                foreach (PPartnerBankingDetailsRow row in MainDS.PPartnerBankingDetails.Rows)
                {
                    if (row.BankingDetailsKey == ABankingDetailsKey)
                    {
                        row.Delete();
                    }
                }

                bool wasMainAccount = false;
                PartnerEditTDSPBankingDetailsRow otherAccount = null;
                foreach (PartnerEditTDSPBankingDetailsRow row in MainDS.PBankingDetails.Rows)
                {
                    if (row.BankingDetailsKey == ABankingDetailsKey)
                    {
                        if (!row.IsMainAccountNull())
                        {
                            wasMainAccount = row.MainAccount;
                        }
                        row.Delete();
                    }
                    else if (otherAccount == null) {
                        otherAccount = row;
                    }
                }

                // make another bank account the main account
                if (wasMainAccount && (otherAccount != null)) {
                    otherAccount.MainAccount = true;
                }

                DataSet ResponseDS = new PartnerEditTDS();
                TPartnerEditUIConnector uiconnector = new TPartnerEditUIConnector(APartnerKey);

                try
                {
                    TSubmitChangesResult result = uiconnector.SubmitChanges(
                        ref MainDS,
                        ref ResponseDS,
                        out AVerificationResult);

                    return result == TSubmitChangesResult.scrOK;
                }
                catch (Exception e)
                {
                    TLogging.Log(e.ToString());
                    AVerificationResult.Add(new TVerificationResult("error", e.Message, TResultSeverity.Resv_Critical));
                    return false;
                }
            }

            if (!TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
            {
                TLogging.Log(AVerificationResult.BuildVerificationResultString());
            }

            return false;
        }
    }
}
