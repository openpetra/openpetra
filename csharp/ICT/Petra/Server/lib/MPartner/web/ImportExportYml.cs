//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       thomass
//       ChristianK
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Xml;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MPartner.ImportExport
{
    /// <summary>
    /// import and export partner data from yml files
    /// </summary>
    public class TImportExportYml
    {
        /// <summary>
        /// Date Format string that is used in the YAML Import/Export format.
        /// </summary>
        /// <remarks>Writing and parsing of dates in the YAML Import/Export format must always done with this Format String
        /// and <see cref="CultureInfo.InvariantCulture" />! This ensures that dates within a file that gets exported
        /// 'somewhere in the world' can be parsed correctly 'somewhere else in the world'.</remarks>
        public const string IMPORTEXPORT_YAML_DATEFORMAT = "yyyy-MM-dd HH:mm:ss";

        static Int64 NewPartnerKey = -1;

        private static void ParsePartners(ref PartnerImportExportTDS AMainDS,
            XmlNode ACurNode,
            TDBTransaction ATransaction,
            ref TVerificationResultCollection AVerificationResult)
        {
            XmlNode LocalNode = ACurNode;

            while (LocalNode != null)
            {
                var LocalNodeChildren = TYml2Xml.GetChildren(LocalNode, false);

                if (LocalNode.Name.StartsWith("PartnerGroup"))
                {
                    ParsePartners(ref AMainDS, LocalNode.FirstChild, ATransaction, ref AVerificationResult);
                }
                else if (LocalNode.Name.StartsWith("Partner"))
                {
                    PPartnerRow PartnerRow = AMainDS.PPartner.NewRowTyped();
                    Boolean IsExistingPartner = false;

                    if (!TYml2Xml.HasAttributeRecursive(LocalNode, "SiteKey"))
                    {
                        throw new Exception(Catalog.GetString("Missing SiteKey Attribute"));
                    }

                    if (!TYml2Xml.HasAttributeRecursive(LocalNode, "status"))
                    {
                        throw new Exception(Catalog.GetString("Missing status Attribute"));
                    }

                    // get a new partner key
                    if (TYml2Xml.HasAttribute(LocalNode, "PartnerKey"))
                    {
                        PartnerRow.PartnerKey = Convert.ToInt64(TYml2Xml.GetAttribute(LocalNode, "PartnerKey"));

                        if (PPartnerAccess.Exists(PartnerRow.PartnerKey, ATransaction))
                        {
                            AMainDS.Merge(PPartnerAccess.LoadByPrimaryKey(PartnerRow.PartnerKey, ATransaction));

                            AMainDS.PPartner.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                                PPartnerTable.GetPartnerKeyDBName(),
                                PartnerRow.PartnerKey);
                            PartnerRow = (PPartnerRow)AMainDS.PPartner.DefaultView[0].Row;
                            IsExistingPartner = true;
                        }
                        else
                        {
                            AMainDS.PPartner.Rows.Add(PartnerRow);
                        }
                    }
                    else
                    {
                        PartnerRow.PartnerKey = TImportExportYml.NewPartnerKey;
                        TImportExportYml.NewPartnerKey--;
                    }

                    String PartnerClass = TYml2Xml.GetAttributeRecursive(LocalNode, "class");
                    TLogging.LogAtLevel(TLogging.DEBUGLEVEL_TRACE,
                        PartnerClass + " " +
                        LocalNode.Name + " " +
                        "PartnerKey=" + PartnerRow.PartnerKey
                        );

                    if (IsExistingPartner && (PartnerClass != PartnerRow.PartnerClass))
                    {
                        throw new Exception(String.Format("Error: Yml contains Existing Partner {0} with a different partner class {1}!",
                                PartnerRow.PartnerKey, PartnerClass));
                    }

                    PartnerRow.PartnerClass = PartnerClass;

                    if (PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
                    {
                        PFamilyRow FamilyRow;

                        if (IsExistingPartner)
                        {
                            AMainDS.Merge(PFamilyAccess.LoadByPrimaryKey(PartnerRow.PartnerKey, ATransaction));

                            AMainDS.PFamily.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                                PFamilyTable.GetPartnerKeyDBName(),
                                PartnerRow.PartnerKey);
                            FamilyRow = (PFamilyRow)AMainDS.PFamily.DefaultView[0].Row;
                        }
                        else
                        {
                            FamilyRow = AMainDS.PFamily.NewRowTyped();
                            FamilyRow.PartnerKey = PartnerRow.PartnerKey;
                            PartnerRow.PartnerClass = MPartnerConstants.PARTNERCLASS_FAMILY;
                            AMainDS.PFamily.Rows.Add(FamilyRow);
                        }

                        FamilyRow.FamilyName = TYml2Xml.GetAttributeRecursive(LocalNode, "LastName");
                        FamilyRow.FirstName = TYml2Xml.GetAttribute(LocalNode, "FirstName");
                        FamilyRow.Title = TYml2Xml.GetAttribute(LocalNode, "Title");

                        if (TYml2Xml.HasAttribute(LocalNode, "CreatedAt"))
                        {
                            FamilyRow.DateCreated = System.DateTime.ParseExact(
                                TYml2Xml.GetAttribute(LocalNode, "CreatedAt"),
                                IMPORTEXPORT_YAML_DATEFORMAT, CultureInfo.InvariantCulture);
                        }

                        PartnerRow.AddresseeTypeCode = MPartnerConstants.PARTNERCLASS_FAMILY;

                        PartnerRow.PartnerShortName =
                            Calculations.DeterminePartnerShortName(FamilyRow.FamilyName, FamilyRow.Title, FamilyRow.FirstName);
                    }

                    if (PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                    {
                        PPersonRow PersonRow;

                        if (IsExistingPartner)
                        {
                            AMainDS.Merge(PPersonAccess.LoadByPrimaryKey(PartnerRow.PartnerKey, ATransaction));

                            AMainDS.PPerson.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                                PPersonTable.GetPartnerKeyDBName(),
                                PartnerRow.PartnerKey);
                            PersonRow = (PPersonRow)AMainDS.PPerson.DefaultView[0].Row;
                        }
                        else
                        {
                            PersonRow = AMainDS.PPerson.NewRowTyped();
                            PersonRow.PartnerKey = PartnerRow.PartnerKey;
                            AMainDS.PPerson.Rows.Add(PersonRow);
                        }

                        PersonRow.FamilyName = TYml2Xml.GetAttributeRecursive(LocalNode, "LastName");
                        PersonRow.FirstName = TYml2Xml.GetAttribute(LocalNode, "FirstName");
                        PersonRow.Title = TYml2Xml.GetAttribute(LocalNode, "Title");

                        if (TYml2Xml.HasAttribute(LocalNode, "CreatedAt"))
                        {
                            PersonRow.DateCreated = System.DateTime.ParseExact(
                                TYml2Xml.GetAttribute(LocalNode, "CreatedAt"),
                                IMPORTEXPORT_YAML_DATEFORMAT, CultureInfo.InvariantCulture);
                        }

                        // PersonRow.Sp
                        PartnerRow.PartnerShortName =
                            Calculations.DeterminePartnerShortName(PersonRow.FamilyName, PersonRow.Title, PersonRow.FirstName);
                    }
                    else if (PartnerClass == MPartnerConstants.PARTNERCLASS_ORGANISATION)
                    {
                        POrganisationRow OrganisationRow;

                        if (IsExistingPartner)
                        {
                            AMainDS.Merge(POrganisationAccess.LoadByPrimaryKey(PartnerRow.PartnerKey, ATransaction));

                            AMainDS.POrganisation.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                                POrganisationTable.GetPartnerKeyDBName(),
                                PartnerRow.PartnerKey);
                            OrganisationRow = (POrganisationRow)AMainDS.POrganisation.DefaultView[0].Row;
                        }
                        else
                        {
                            OrganisationRow = AMainDS.POrganisation.NewRowTyped();
                            OrganisationRow.PartnerKey = PartnerRow.PartnerKey;
                            AMainDS.POrganisation.Rows.Add(OrganisationRow);
                        }

                        OrganisationRow.OrganisationName = TYml2Xml.GetAttributeRecursive(LocalNode, "Name");

                        PartnerRow.PartnerShortName = OrganisationRow.OrganisationName;
                    }
                    else if (PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
                    {
                        PUnitRow UnitRow;

                        if (IsExistingPartner)
                        {
                            AMainDS.Merge(PUnitAccess.LoadByPrimaryKey(PartnerRow.PartnerKey, ATransaction));

                            AMainDS.PUnit.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                                PUnitTable.GetPartnerKeyDBName(),
                                PartnerRow.PartnerKey);
                            UnitRow = (PUnitRow)AMainDS.PUnit.DefaultView[0].Row;
                        }
                        else
                        {
                            UnitRow = AMainDS.PUnit.NewRowTyped();
                            UnitRow.PartnerKey = PartnerRow.PartnerKey;
                            AMainDS.PUnit.Rows.Add(UnitRow);
                        }

                        UnitRow.UnitTypeCode = TYml2Xml.GetAttributeRecursive(LocalNode, "UnitTypeCode");
                        UnitRow.UnitName = TYml2Xml.GetAttributeRecursive(LocalNode, "Name");

                        if (PartnerRow.PartnerKey < -1)
                        {
                            throw new Exception("Invalid Partner Key or No Partner Key - and no proper handling implemented");
                            // from here...

                            /*
                             * AVerificationResult.Add(new TVerificationResult(
                             *  String.Format(Catalog.GetString("Importing Unit {0}"), UnitRow.UnitName),
                             *  Catalog.GetString("You need to provide a partner key for the unit"),
                             *  TResultSeverity.Resv_Critical));
                             */
                            // ...to here: throws Exception in case of a illegal import file?
                            // The above code must have a glitch
                        }

                        if (!TYml2Xml.HasAttribute(LocalNode, "ParentUnitKey"))
                        {
                            throw new Exception(
                                "The currently being processed unit (PartnerKey " +
                                PartnerRow.PartnerKey +
                                ") requires a ParentUnitKey.");
                        }

                        Int64 ParentKey = Convert.ToInt64(TYml2Xml.GetAttributeRecursive(LocalNode, "ParentUnitKey"));
                        UmUnitStructureRow UnitStructureRow = null;

                        if (IsExistingPartner)
                        {
                            AMainDS.Merge(UmUnitStructureAccess.LoadViaPUnitChildUnitKey(PartnerRow.PartnerKey, ATransaction));

                            AMainDS.UmUnitStructure.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                                UmUnitStructureTable.GetChildUnitKeyDBName(),
                                PartnerRow.PartnerKey);

                            if (AMainDS.UmUnitStructure.DefaultView.Count > 0)
                            {
                                UnitStructureRow = (UmUnitStructureRow)AMainDS.UmUnitStructure.DefaultView[0].Row;
                            }
                        }

                        if (UnitStructureRow == null)
                        {
                            UnitStructureRow = AMainDS.UmUnitStructure.NewRowTyped();
                            UnitStructureRow.ParentUnitKey = ParentKey;
                            UnitStructureRow.ChildUnitKey = PartnerRow.PartnerKey;
                            AMainDS.UmUnitStructure.Rows.Add(UnitStructureRow);
                        }
                        else
                        {
                            UnitStructureRow.ParentUnitKey = ParentKey;
                            UnitStructureRow.ChildUnitKey = PartnerRow.PartnerKey;
                        }

                        PartnerRow.PartnerShortName = UnitRow.UnitName;
                    }
                    else if (PartnerClass == MPartnerConstants.PARTNERCLASS_BANK)
                    {
                        PBankRow BankRow;

                        if (IsExistingPartner)
                        {
                            AMainDS.Merge(PBankAccess.LoadByPrimaryKey(PartnerRow.PartnerKey, ATransaction));

                            AMainDS.PBank.DefaultView.RowFilter = String.Format("{0} = '{1}'",
                                PBankTable.GetPartnerKeyDBName(),
                                PartnerRow.PartnerKey);
                            BankRow = (PBankRow)AMainDS.PBank.DefaultView[0].Row;
                        }
                        else
                        {
                            BankRow = AMainDS.PBank.NewRowTyped();
                            BankRow.PartnerKey = PartnerRow.PartnerKey;
                            AMainDS.PBank.Rows.Add(BankRow);
                        }

                        BankRow.BranchName = TYml2Xml.GetAttribute(LocalNode, "BranchName");
                        BankRow.BranchCode = TYml2Xml.GetAttribute(LocalNode, "BranchCode");
                        BankRow.Bic = TYml2Xml.GetAttribute(LocalNode, "BranchBic");
                        BankRow.EpFormatFile = TYml2Xml.GetAttribute(LocalNode, "EpFormatFile");

                        if (TYml2Xml.HasAttribute(LocalNode, "CreatedAt"))
                        {
                            BankRow.DateCreated = System.DateTime.ParseExact(
                                TYml2Xml.GetAttribute(LocalNode, "CreatedAt"),
                                IMPORTEXPORT_YAML_DATEFORMAT, CultureInfo.InvariantCulture);
                        }
                    }
                    else
                    {
                        /*
                         * throw new Exception(
                         *  "Unknown Partner Class" +
                         *  TYml2Xml.GetAttributeRecursive(LocalNode, "class"));
                         */
                        // TODO AVerificationResult add failing problem: unknown partner class
                    }

                    PartnerRow.StatusCode = TYml2Xml.GetAttributeRecursive(LocalNode, "status");

                    // import special types
                    StringCollection SpecialTypes = StringHelper.StrSplit(TYml2Xml.GetAttributeRecursive(LocalNode, "SpecialTypes"), ",");

                    if (IsExistingPartner)
                    {
                        PPartnerTypeAccess.LoadViaPPartner(AMainDS, PartnerRow.PartnerKey, ATransaction);
                    }

                    foreach (string SpecialType in SpecialTypes)
                    {
                        PPartnerTypeRow PartnerTypeRow = null;
                        AMainDS.PPartnerType.DefaultView.RowFilter = String.Format("{0}={1} AND {2}='{3}'",
                            PPartnerTypeTable.GetPartnerKeyDBName(),
                            PartnerRow.PartnerKey,
                            PPartnerTypeTable.GetTypeCodeDBName(),
                            SpecialType
                            );

                        if (AMainDS.PPartnerType.DefaultView.Count > 0)
                        {
                            PartnerTypeRow = (PPartnerTypeRow)AMainDS.PPartnerType.DefaultView[0].Row;
                        }
                        else
                        {
                            PartnerTypeRow = AMainDS.PPartnerType.NewRowTyped();
                            PartnerTypeRow.PartnerKey = PartnerRow.PartnerKey;
                            PartnerTypeRow.TypeCode = SpecialType.Trim();
                            AMainDS.PPartnerType.Rows.Add(PartnerTypeRow);
                        }

                        // Check Partner type exists, or create it
                        bool TypeIsKnown = PTypeAccess.Exists(PartnerTypeRow.TypeCode, ATransaction);

                        if (!TypeIsKnown)
                        {
                            Int32 RowIdx = AMainDS.PType.DefaultView.Find(PartnerTypeRow.TypeCode); // I might have created it a second ago..

                            if (RowIdx < 0)
                            {
                                PTypeRow TypeRow = AMainDS.PType.NewRowTyped();
                                TypeRow.TypeCode = PartnerTypeRow.TypeCode;
                                TypeRow.TypeDescription = "Created from YAML import";
                                AMainDS.PType.Rows.Add(TypeRow);
                            }
                        }
                    }

                    // import subscriptions
                    StringCollection Subscriptions = StringHelper.StrSplit(TYml2Xml.GetAttributeRecursive(LocalNode, "Subscriptions"), ",");

                    foreach (string publicationCode in Subscriptions)
                    {
                        PSubscriptionRow subscription = AMainDS.PSubscription.NewRowTyped();
                        subscription.PartnerKey = PartnerRow.PartnerKey;
                        subscription.PublicationCode = publicationCode.Trim();
                        subscription.ReasonSubsGivenCode = "FREE";
                        AMainDS.PSubscription.Rows.Add(subscription);
                    }

                    // import address
                    XmlNode addressNode = TYml2Xml.GetChild(LocalNode, "Address");

                    if ((addressNode == null) || (TYml2Xml.GetAttributeRecursive(addressNode, "Street").Length == 0))
                    {
                        if (!IsExistingPartner)
                        {
                            // add the empty location
                            PPartnerLocationRow partnerlocation = AMainDS.PPartnerLocation.NewRowTyped(true);
                            partnerlocation.SiteKey = 0;
                            partnerlocation.PartnerKey = PartnerRow.PartnerKey;
                            partnerlocation.DateEffective = DateTime.Now;
                            partnerlocation.LocationType = "HOME";
                            partnerlocation.SendMail = false;
                            AMainDS.PPartnerLocation.Rows.Add(partnerlocation);
                        }
                    }
                    else
                    {
                        PLocationRow location = AMainDS.PLocation.NewRowTyped(true);
                        location.LocationKey = (AMainDS.PLocation.Rows.Count + 1) * -1;
                        location.SiteKey = 0;

                        if (!TYml2Xml.HasAttributeRecursive(LocalNode, "Country"))
                        {
                            throw new Exception(Catalog.GetString("Missing Country Attribute"));
                        }

                        location.CountryCode = TYml2Xml.GetAttributeRecursive(addressNode, "Country");
                        location.StreetName = TYml2Xml.GetAttributeRecursive(addressNode, "Street");
                        location.City = TYml2Xml.GetAttributeRecursive(addressNode, "City");
                        location.PostalCode = TYml2Xml.GetAttributeRecursive(addressNode, "PostCode");
                        AMainDS.PLocation.Rows.Add(location);

                        PPartnerLocationRow partnerlocation = AMainDS.PPartnerLocation.NewRowTyped(true);
                        partnerlocation.SiteKey = 0;
                        partnerlocation.LocationKey = location.LocationKey;
                        partnerlocation.PartnerKey = PartnerRow.PartnerKey;
                        partnerlocation.SendMail = (PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON) ? false : true;
                        partnerlocation.DateEffective = DateTime.Now;
                        partnerlocation.LocationType = "HOME";
                        AMainDS.PPartnerLocation.Rows.Add(partnerlocation);
                    }

                    // import finance details (bank account number)
                    XmlNode financialDetailsNode = TYml2Xml.GetChild(LocalNode, "FinancialDetails");

                    ParseFinancialDetails(AMainDS, financialDetailsNode, PartnerRow.PartnerKey, ATransaction);

                    foreach (var LocalNodeChildNode in LocalNodeChildren)
                    {
                        // import Partner Attributes (Partner Contact Details, etc)
                        if (LocalNodeChildNode.Name.StartsWith("PartnerAttribute"))
                        {
                            ParsePartnerAttributes(AMainDS, LocalNodeChildNode, PartnerRow.PartnerKey, ATransaction);
                        }
                    }
                }

                LocalNode = LocalNode.NextSibling;
            }

            Ict.Petra.Shared.MPartner.Calculations.DeterminePartnerContactDetailAttributes(AMainDS.PPartnerAttribute);
        }

        private static void ParseFinancialDetails(PartnerImportExportTDS AMainDS,
            XmlNode AFinancialDetailsNode,
            Int64 APartnerKey,
            TDBTransaction ATransaction)
        {
            if (AFinancialDetailsNode != null)
            {
                string BankAccountNumber = TYml2Xml.GetAttributeRecursive(AFinancialDetailsNode, "AccountNumber");
                string BankSortCode = TYml2Xml.GetAttributeRecursive(AFinancialDetailsNode, "BankSortCode");

                // do we already have a bank with this sort code?
                Int64 bankPartnerKey = 0;

                AMainDS.PBank.DefaultView.Sort = PBankTable.GetBranchCodeDBName();
                int bankIndex = AMainDS.PBank.DefaultView.Find(BankSortCode);

                if (bankIndex != -1)
                {
                    bankPartnerKey = ((PBankRow)AMainDS.PBank.DefaultView[bankIndex].Row).PartnerKey;
                }

                if (bankPartnerKey == 0)
                {
                    string sqlFindBankBySortCode =
                        String.Format("SELECT * FROM PUB_{0} WHERE {1}=?",
                            PBankTable.GetTableDBName(),
                            PBankTable.GetBranchCodeDBName());

                    OdbcParameter param = new OdbcParameter("branchcode", OdbcType.VarChar);
                    param.Value = BankSortCode;
                    PBankTable bank = new PBankTable();
                    DBAccess.GDBAccessObj.SelectDT(bank, sqlFindBankBySortCode, ATransaction, new OdbcParameter[] {
                            param
                        }, -1, -1);

                    if (bank.Count > 0)
                    {
                        bankPartnerKey = bank[0].PartnerKey;
                    }
                }

                if (bankPartnerKey == 0)
                {
                    // create a new bank record
                    PBankRow bankRow = AMainDS.PBank.NewRowTyped(true);
                    bankRow.PartnerKey = TImportExportYml.NewPartnerKey;
                    TImportExportYml.NewPartnerKey--;
                    bankRow.BranchCode = BankSortCode;
                    bankRow.BranchName = BankSortCode;
                    AMainDS.PBank.Rows.Add(bankRow);
                    bankPartnerKey = bankRow.PartnerKey;
                }

                PBankingDetailsRow bankingDetailsRow = AMainDS.PBankingDetails.NewRowTyped(true);
                bankingDetailsRow.BankingDetailsKey = (AMainDS.PBankingDetails.Rows.Count + 1) * -1;
                bankingDetailsRow.BankingType = 0;
                bankingDetailsRow.BankAccountNumber = BankAccountNumber;
                bankingDetailsRow.BankKey = bankPartnerKey;
                AMainDS.PBankingDetails.Rows.Add(bankingDetailsRow);

                PPartnerBankingDetailsRow partnerBankingDetailsRow = AMainDS.PPartnerBankingDetails.NewRowTyped(true);
                partnerBankingDetailsRow.PartnerKey = APartnerKey;
                partnerBankingDetailsRow.BankingDetailsKey = bankingDetailsRow.BankingDetailsKey;
                AMainDS.PPartnerBankingDetails.Rows.Add(partnerBankingDetailsRow);
            }
        }

        private static void ParsePartnerAttributes(PartnerImportExportTDS AMainDS,
            XmlNode APartnerAttributeNode,
            Int64 APartnerKey,
            TDBTransaction ATransaction)
        {
            PPartnerAttributeRow NewAttributeDR;

            if (APartnerAttributeNode != null)
            {
                if (!TYml2Xml.HasAttributeRecursive(APartnerAttributeNode, "AttributeType"))
                {
                    throw new Exception(Catalog.GetString("PartnerAttribute Node: Missing 'AttributeType' Attribute"));
                }

                if (!TYml2Xml.HasAttributeRecursive(APartnerAttributeNode, "Index"))
                {
                    throw new Exception(Catalog.GetString("PartnerAttribute Node: Missing 'Index' Attribute"));
                }

                if (!TYml2Xml.HasAttributeRecursive(APartnerAttributeNode, "Value"))
                {
                    throw new Exception(Catalog.GetString("PartnerAttribute Node: Missing 'Value' Attribute"));
                }

                if (!TYml2Xml.HasAttributeRecursive(APartnerAttributeNode, "Primary"))
                {
                    throw new Exception(Catalog.GetString("PartnerAttribute Node: Missing 'Primary' Attribute"));
                }

                if (!TYml2Xml.HasAttributeRecursive(APartnerAttributeNode, "WithinOrganisation"))
                {
                    throw new Exception(Catalog.GetString("PartnerAttribute Node: Missing 'WithinOrganisation' Attribute"));
                }

                if (!TYml2Xml.HasAttributeRecursive(APartnerAttributeNode, "Specialised"))
                {
                    throw new Exception(Catalog.GetString("PartnerAttribute Node: Missing 'Specialised' Attribute"));
                }

                if (!TYml2Xml.HasAttributeRecursive(APartnerAttributeNode, "Confidential"))
                {
                    throw new Exception(Catalog.GetString("PartnerAttribute Node: Missing 'Confidential' Attribute"));
                }

                if (!TYml2Xml.HasAttributeRecursive(APartnerAttributeNode, "Current"))
                {
                    throw new Exception(Catalog.GetString("PartnerAttribute Node: Missing 'Current' Attribute"));
                }

                NewAttributeDR = AMainDS.PPartnerAttribute.NewRowTyped(true);

                NewAttributeDR.PartnerKey = APartnerKey;
                NewAttributeDR.AttributeType = TYml2Xml.GetAttributeRecursive(APartnerAttributeNode, "AttributeType");
                NewAttributeDR.Index = Convert.ToInt32(TYml2Xml.GetAttributeRecursive(APartnerAttributeNode, "Index"));
                NewAttributeDR.Value = TYml2Xml.GetAttributeRecursive(APartnerAttributeNode, "Value");
                NewAttributeDR.Primary = Convert.ToBoolean(TYml2Xml.GetAttributeRecursive(APartnerAttributeNode, "Primary"));
                NewAttributeDR.WithinOrganisation = Convert.ToBoolean(TYml2Xml.GetAttributeRecursive(APartnerAttributeNode, "WithinOrganisation"));
                NewAttributeDR.Specialised = Convert.ToBoolean(TYml2Xml.GetAttributeRecursive(APartnerAttributeNode, "Specialised"));
                NewAttributeDR.Confidential = Convert.ToBoolean(TYml2Xml.GetAttributeRecursive(APartnerAttributeNode, "Confidential"));
                NewAttributeDR.Current = Convert.ToBoolean(TYml2Xml.GetAttributeRecursive(APartnerAttributeNode, "Current"));

                if (TYml2Xml.HasAttributeRecursive(APartnerAttributeNode, "NoLongerCurrentFrom"))
                {
                    NewAttributeDR.NoLongerCurrentFrom = System.DateTime.ParseExact(
                        TYml2Xml.GetAttributeRecursive(APartnerAttributeNode, "NoLongerCurrentFrom"),
                        IMPORTEXPORT_YAML_DATEFORMAT, CultureInfo.InvariantCulture);
                }

                if (TYml2Xml.HasAttributeRecursive(APartnerAttributeNode, "Comment"))
                {
                    NewAttributeDR.Comment = TYml2Xml.GetAttributeRecursive(APartnerAttributeNode, "Comment");
                }

                AMainDS.PPartnerAttribute.Rows.Add(NewAttributeDR);
            }
        }

        /// <summary>
        /// imports partner data from file
        /// </summary>
        /// <param name="AXmlPartnerData"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static PartnerImportExportTDS ImportPartners(string AXmlPartnerData, out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction ReadTransaction = null;
            TVerificationResultCollection VerificationResult;

            VerificationResult = new TVerificationResultCollection();

            PartnerImportExportTDS MainDS = new PartnerImportExportTDS();

            XmlDocument doc = new XmlDocument();

            doc.LoadXml(AXmlPartnerData);

            XmlNode root = doc.FirstChild.NextSibling.FirstChild;

            // import partner groups
            // advantage: can inherit some common attributes, eg. partner class, etc

            TImportExportYml.NewPartnerKey = -1;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.RepeatableRead, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    ParsePartners(ref MainDS, root, ReadTransaction, ref VerificationResult);
                });

            AVerificationResult = VerificationResult;

            return MainDS;
        }

        /// <summary>
        /// Load data from db.
        /// Data is held in variable MainDS.PPartner and then MainDS.PLocation, PFamilyAccess etc...
        /// The latter is to get the additional information not present in PPartner but in dependent tables.
        /// </summary>
        /// <param name="AMainDS">
        /// The Datastructure which is filled with the data from the DB.
        /// It should be empty initially.
        /// </param>
        private static void LoadDataFromDB(ref PartnerEditTDS AMainDS)
        {
            TDBTransaction Transaction = null;
            bool SubmissionOK = false;
            PartnerEditTDS MainDS = new PartnerEditTDS();

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    PPartnerAccess.LoadAll(MainDS, Transaction);
                    TLogging.LogAtLevel(TLogging.DEBUGLEVEL_TRACE, "Read Partners from Database : " + MainDS.PPartner.Rows.Count.ToString());
                    TLogging.LogAtLevel(TLogging.DEBUGLEVEL_TRACE, "Now reading additional data for each Partner:");

                    foreach (PPartnerRow partnerRow in MainDS.PPartner.Rows)
                    {
                        long partnerKey = partnerRow.PartnerKey;
                        PLocationAccess.LoadViaPPartner(MainDS, partnerKey, Transaction);
                        PPartnerLocationAccess.LoadViaPPartner(MainDS, partnerKey, Transaction);
                        PPartnerAttributeAccess.LoadViaPPartner(MainDS, partnerKey, Transaction);
                        PPartnerTypeAccess.LoadViaPPartner(MainDS, partnerKey, Transaction);
                        PPersonAccess.LoadViaPPartner(MainDS, partnerKey, Transaction);
                        PFamilyAccess.LoadViaPPartner(MainDS, partnerKey, Transaction);
                        POrganisationAccess.LoadViaPPartnerPartnerKey(MainDS, partnerKey, Transaction);
                        PUnitAccess.LoadViaPPartnerPartnerKey(MainDS, partnerKey, Transaction);
                        UmUnitStructureAccess.LoadViaPUnitChildUnitKey(MainDS, partnerKey, Transaction);
                        PBankAccess.LoadViaPPartnerPartnerKey(MainDS, partnerKey, Transaction);
                    }

                    if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_TRACE)
                    {
                        TLogging.Log("All in all:");
                        SortedList <string, int>sortedtables = new SortedList <string, int>();
                        sortedtables.Add("PLocation", MainDS.PLocation.Count);
                        sortedtables.Add("PPartnerLocation", MainDS.PPartnerLocation.Count);
                        sortedtables.Add("PPartnerAttribute", MainDS.PPartnerAttribute.Count);
                        sortedtables.Add("PPartnerType", MainDS.PPartnerType.Count);
                        sortedtables.Add("PPerson", MainDS.PPerson.Count);
                        sortedtables.Add("PFamily", MainDS.PFamily.Count);
                        sortedtables.Add("POrganisation", MainDS.POrganisation.Count);

                        foreach (KeyValuePair <string, int /*TTypedDataTable*/>pair in sortedtables)
                        {
                            TLogging.Log(pair.Key + " : " + pair.Value.ToString());
                        }
                    }

                    SubmissionOK = true;
                });

            AMainDS.Merge(MainDS);

            DBAccess.GDBAccessObj.RollbackTransaction();
        }

        /// <summary>
        /// Private method used by ExportPartners().
        /// For the given partnerKey, updates given references countryCode and siteKey.
        /// If there is no location for the given partnerKey, leaves them as they are.
        /// </summary>
        /// <param name="MainDS">Reference to the already filled Datastructure</param>
        /// <param name="partnerKey"></param>
        /// <param name="countryCode"></param>
        /// <param name="siteKey"></param>
        /// <returns>
        /// True: if a location was found for given key and the countryCode
        /// and siteKey were updated. False otherwise.
        /// </returns>
        private static bool UpdateCountryAndSiteForGivenPK(
            PartnerEditTDS MainDS,
            long partnerKey,
            ref string countryCode /* default could be "" */,
            ref Int64 siteKey /* default could be -1 */
            )
        {
            bool retval = false;
            // Find partnerLocation for given partner_key
            DataView partnerLocationView = MainDS.PPartnerLocation.DefaultView;

            partnerLocationView.RowFilter = PPartnerLocationTable.GetPartnerKeyDBName() + " = " + partnerKey.ToString();

            if (partnerLocationView.Count > 0)
            {
                // partnerLocation: links one partner to possibly several Locations
                // Just get the first one for now (and disregard the others).
                // TODO: could determine the best address and use that
                PPartnerLocationRow partnerLocationRow = (PPartnerLocationRow)partnerLocationView[0].Row;

                DataView locationView = MainDS.PLocation.DefaultView;
                locationView.RowFilter =
                    PLocationTable.GetSiteKeyDBName() + "=" + partnerLocationRow.SiteKey.ToString() + " AND " +
                    PLocationTable.GetLocationKeyDBName() + "=" + partnerLocationRow.LocationKey.ToString();

                if (locationView.Count > 0)
                {
                    PLocationRow locationRow = (PLocationRow)locationView[0].Row;
                    countryCode = locationRow.CountryCode;
                    siteKey = locationRow.SiteKey;
                    retval = true;
                }
                else     // if there is a partner_location, there has _got_ to be the corresponding location
                {
                    throw new Exception("Error in application: I can't find the Location with LocationKey " +
                        partnerLocationRow.LocationKey.ToString() +
                        " (Sitekey " + partnerLocationRow.SiteKey.ToString() + ")"
                        );
                }
            }

            return retval;
        }

        /// <summary>
        /// Group partners into categories.
        /// A partner's category is defined by his: class, country, status, and sitekey
        /// It is stored as a string e.g. "FAMILY,DE,ACTIVE,0".
        /// </summary>
        /// <returns>
        /// We end up with a Sorted List, with
        ///   - the categories being the keys
        ///   - and each category having a list of partnerKeys attached to it
        /// </returns>
        ///
        public static SortedList <string, List <long>>GroupPartnersIntoCategories(PartnerEditTDS MainDS)
        {
            SortedList <string, List <long>>PartnerCategories = new SortedList <string, List <long>>();

            foreach (PPartnerRow partnerRow in MainDS.PPartner.Rows)
            {
                string countryCode = ""; // default value
                Int64 siteKey = -1; // default value

                long partnerKey = partnerRow.PartnerKey;
                UpdateCountryAndSiteForGivenPK(MainDS, partnerKey, ref countryCode, ref siteKey);

                string category = partnerRow.PartnerClass + "," + countryCode + "," +
                                  partnerRow.StatusCode + "," + siteKey.ToString();

                if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_TRACE)
                {
                    TLogging.Log("Partner " + partnerRow.PartnerKey.ToString("0000000000") + ", Category: " + category);
                }

                if (!PartnerCategories.ContainsKey(category))
                {
                    PartnerCategories.Add(category, new List <long>());
                }

                PartnerCategories[category].Add(partnerRow.PartnerKey);
            }

            return PartnerCategories;
        }

        /// <summary>
        /// return an XmlDocument with all partner info;
        /// the partners are grouped by class, country, status, and sitekey
        /// </summary>
        /// <returns></returns>
        public static string ExportPartners()
        {
            PartnerEditTDS MainDS = new PartnerEditTDS();

            LoadDataFromDB(ref MainDS);

            // Group partners into categories.
            //
            // A partner's category is defined by his: class, country, status, and sitekey
            // It is stored as a string e.g. "FAMILY,DE,ACTIVE,0".
            //
            SortedList <string, List <long>>PartnerCategories = GroupPartnersIntoCategories(MainDS);

            // create XML structure for each category
            XmlDocument PartnerData = TYml2Xml.CreateXmlDocument();
            XmlNode rootNode = PartnerData.FirstChild.NextSibling;

            Int32 groupCounter = 0;

            foreach (string category in PartnerCategories.Keys)
            {
                // get category data
                groupCounter++;
                XmlElement groupNode = PartnerData.CreateElement("PartnerGroup" + groupCounter.ToString());
                rootNode.AppendChild(groupNode);

                Int32 partnerCounter = 0;
                string[] categoryDetails = category.Split(new char[] { ',' });
                // may want to skip the categories with sitekey = -1
                // right now, we still export them and ignore the partners 0 and 1000000 later

                groupNode.SetAttribute("class", categoryDetails[0]);
                groupNode.SetAttribute("Country", categoryDetails[1]);
                groupNode.SetAttribute("status", categoryDetails[2]);
                groupNode.SetAttribute("SiteKey", categoryDetails[3]);

                List <long>partnerKeys = PartnerCategories[category];

                foreach (long partnerKey in partnerKeys)
                {
                    if ((partnerKey != 0) && (partnerKey != 1000000)) // skip organization root and the 0 when exporting
                    {
                        MainDS.PPartner.DefaultView.RowFilter = PPartnerTable.GetPartnerKeyDBName() + "=" + partnerKey.ToString();
                        PPartnerRow partnerRow = (PPartnerRow)MainDS.PPartner.DefaultView[0].Row;

                        PFamilyRow familyRow = null;

                        if (partnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_FAMILY)
                        {
                            MainDS.PFamily.DefaultView.RowFilter = PFamilyTable.GetPartnerKeyDBName() + "=" + partnerKey.ToString();
                            familyRow = (PFamilyRow)MainDS.PFamily.DefaultView[0].Row;
                        }

                        PPersonRow personRow = null;

                        if (partnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_PERSON)
                        {
                            MainDS.PPerson.DefaultView.RowFilter = PPersonTable.GetPartnerKeyDBName() + "=" + partnerKey.ToString();
                            personRow = (PPersonRow)MainDS.PPerson.DefaultView[0].Row;
                        }

                        POrganisationRow organisationRow = null;

                        if (partnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_ORGANISATION)
                        {
                            MainDS.POrganisation.DefaultView.RowFilter = POrganisationTable.GetPartnerKeyDBName() + "=" + partnerKey.ToString();
                            organisationRow = (POrganisationRow)MainDS.POrganisation.DefaultView[0].Row;
                        }

                        PUnitRow unitRow = null;
                        UmUnitStructureRow unitStructureRow = null;

                        if (partnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_UNIT)
                        {
                            MainDS.PUnit.DefaultView.RowFilter = PUnitTable.GetPartnerKeyDBName() + "=" + partnerKey.ToString();
                            unitRow = (PUnitRow)MainDS.PUnit.DefaultView[0].Row;
                            MainDS.UmUnitStructure.DefaultView.RowFilter = UmUnitStructureTable.GetChildUnitKeyDBName() + " = " + partnerKey.ToString();

                            long numParents = MainDS.UmUnitStructure.DefaultView.Count;

                            if (numParents == 1)
                            {
                                unitStructureRow = (UmUnitStructureRow)MainDS.UmUnitStructure.DefaultView[0].Row;
                            }
                            else
                            {
                                throw new Exception(
                                    "Units must have exactly one ParentUnit. " +
                                    "The unit with partnerKey " + partnerKey.ToString() + " has " +
                                    numParents.ToString() + ".");
                            }
                        }

                        PBankRow BankRow = null;

                        if (partnerRow.PartnerClass == MPartnerConstants.PARTNERCLASS_BANK)
                        {
                            MainDS.PBank.DefaultView.RowFilter = PBankTable.GetPartnerKeyDBName() + "=" + partnerKey.ToString();
                            BankRow = (PBankRow)MainDS.PBank.DefaultView[0].Row;
                        }

                        partnerCounter++;
                        XmlElement partnerNode = PartnerData.CreateElement("Partner" + partnerCounter.ToString());
                        groupNode.AppendChild(partnerNode);

                        partnerNode.SetAttribute("PartnerKey", partnerRow.PartnerKey.ToString());

                        //groupNode.SetAttribute("ShortName", partnerRow.PartnerShortName.ToString());

                        if (personRow != null)
                        {
                            partnerNode.SetAttribute("FirstName", personRow.FirstName.ToString());
                            partnerNode.SetAttribute("LastName", personRow.FamilyName.ToString());
                            partnerNode.SetAttribute("Title", personRow.Title.ToString());
                        }
                        else if (familyRow != null)
                        {
                            partnerNode.SetAttribute("FirstName", familyRow.FirstName.ToString());
                            partnerNode.SetAttribute("LastName", familyRow.FamilyName.ToString());
                            partnerNode.SetAttribute("Title", familyRow.Title.ToString());
                        }
                        else if (organisationRow != null)
                        {
                            partnerNode.SetAttribute("Name", organisationRow.OrganisationName.ToString());
                        }
                        else if (unitRow != null)
                        {
                            partnerNode.SetAttribute("Name", unitRow.UnitName.ToString());
                            partnerNode.SetAttribute("UnitTypeCode", unitRow.UnitTypeCode.ToString());

                            if (unitStructureRow != null)
                            {
                                partnerNode.SetAttribute("ParentUnitKey", unitStructureRow.ParentUnitKey.ToString());
                            }
                        }

                        if (BankRow != null)
                        {
                            partnerNode.SetAttribute("BranchName", BankRow.BranchName);
                            partnerNode.SetAttribute("BranchCode", BankRow.BranchCode);
                            partnerNode.SetAttribute("BranchBic", BankRow.Bic);
                            partnerNode.SetAttribute("EpFormatFile", BankRow.EpFormatFile);
                        }

                        partnerNode.SetAttribute("CreatedAt", partnerRow.DateCreated.Value.ToString(
                                IMPORTEXPORT_YAML_DATEFORMAT, CultureInfo.InvariantCulture));

                        // special types
                        string specialTypes = "";
                        MainDS.PPartnerType.DefaultView.RowFilter = PPartnerTypeTable.GetPartnerKeyDBName() + "=" + partnerKey.ToString();

                        foreach (DataRowView rv in MainDS.PPartnerType.DefaultView)
                        {
                            if (specialTypes.Length > 0)
                            {
                                specialTypes += ", ";
                            }

                            specialTypes += ((PPartnerTypeRow)rv.Row).TypeCode;
                        }

                        if (specialTypes.Length > 0)
                        {
                            partnerNode.SetAttribute("SpecialTypes", specialTypes);
                        }

                        // addresses
                        DataView partnerLocationView = MainDS.PPartnerLocation.DefaultView;
                        partnerLocationView.RowFilter =
                            PPartnerLocationTable.GetPartnerKeyDBName() + " = " + partnerRow.PartnerKey.ToString() +
                            "AND " + PPartnerLocationTable.GetLocationKeyDBName() + " <> 0 "; // ignore invalid addresses
                        Int32 addressCounter = 0;

                        foreach (DataRowView rv in partnerLocationView)
                        {
                            XmlElement addressNode = PartnerData.CreateElement("Address" + (addressCounter > 0 ? addressCounter.ToString() : ""));
                            addressCounter++;
                            partnerNode.AppendChild(addressNode);

                            PPartnerLocationRow partnerLocationRow = (PPartnerLocationRow)rv.Row;

                            DataView locationView = MainDS.PLocation.DefaultView;
                            locationView.RowFilter =
                                PLocationTable.GetSiteKeyDBName() + "=" + partnerLocationRow.SiteKey.ToString() + " AND " +
                                PLocationTable.GetLocationKeyDBName() + "=" + partnerLocationRow.LocationKey.ToString();

                            if (locationView.Count > 0)
                            {
                                PLocationRow locationRow = (PLocationRow)locationView[0].Row;

                                addressNode.SetAttribute("Street", locationRow.StreetName);
                                addressNode.SetAttribute("City", locationRow.City);
                                addressNode.SetAttribute("PostCode", locationRow.PostalCode);
                            }
                        }

                        // TODO: notes
                        // TODO: financial details

                        // Partner Attributes (Partner Contact Details, etc)
                        DataView partnerattributeView = MainDS.PPartnerAttribute.DefaultView;
                        partnerattributeView.RowFilter =
                            PPartnerAttributeTable.GetPartnerKeyDBName() + " = " + partnerRow.PartnerKey.ToString();
                        Int32 partnerAttributeCounter = 0;

                        foreach (DataRowView rv in partnerattributeView)
                        {
                            XmlElement partnerAttributeNode =
                                PartnerData.CreateElement("PartnerAttribute" + (partnerAttributeCounter > 0 ? partnerAttributeCounter.ToString() : ""));
                            partnerAttributeCounter++;
                            partnerNode.AppendChild(partnerAttributeNode);

                            PPartnerAttributeRow partnerAttributeRow = (PPartnerAttributeRow)rv.Row;

                            partnerAttributeNode.SetAttribute("AttributeType", partnerAttributeRow.AttributeType);
                            partnerAttributeNode.SetAttribute("Index", partnerAttributeRow.Index.ToString());
                            partnerAttributeNode.SetAttribute("Value", partnerAttributeRow.Value);
                            partnerAttributeNode.SetAttribute("Primary", partnerAttributeRow.Primary.ToString());
                            partnerAttributeNode.SetAttribute("WithinOrganisation", partnerAttributeRow.WithinOrganisation.ToString());
                            partnerAttributeNode.SetAttribute("Specialised", partnerAttributeRow.Specialised.ToString());
                            partnerAttributeNode.SetAttribute("Confidential", partnerAttributeRow.Confidential.ToString());
                            partnerAttributeNode.SetAttribute("Current", partnerAttributeRow.Current.ToString());

                            if (!partnerAttributeRow.IsNoLongerCurrentFromNull())
                            {
                                partnerAttributeNode.SetAttribute("NoLongerCurrentFrom",
                                    partnerAttributeRow.NoLongerCurrentFrom.Value.ToString(IMPORTEXPORT_YAML_DATEFORMAT,
                                        CultureInfo.InvariantCulture));
                            }

                            if (!partnerAttributeRow.IsCommentNull())
                            {
                                partnerAttributeNode.SetAttribute("Comment", partnerAttributeRow.Comment);
                            }
                        }

                        // TODO: This doesn't export as much data as it should?
                    }
                }
            }

            return TXMLParser.XmlToString(PartnerData);
        }
    }
}