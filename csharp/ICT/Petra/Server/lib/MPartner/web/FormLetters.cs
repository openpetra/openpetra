//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using System.Data.Odbc;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.DB.Exceptions;
using Ict.Common.Remoting.Server;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Partner.Cacheable;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPersonnel.Person.Cacheable;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MCommon.queries;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.queries;
using Ict.Petra.Server.MCommon.Cacheable;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MCommon.Data.Cascading;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// methods related to form letters
    /// </summary>
    public class TFormLettersWebConnector
    {
        /// <summary>
        /// populate form data for given extract and list of fields
        /// </summary>
        /// <param name="AExtractId">Extract of partners to be used</param>
        /// <param name="AFormLetterInfo">Info about form letter (tag list etc.)</param>
        /// <param name="AFormDataList">list with populated form data</param>
        /// <returns>returns true if list was created successfully</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean FillFormDataFromExtract(Int32 AExtractId, TFormLetterInfo AFormLetterInfo,
            out List <TFormData>AFormDataList)
        {
            Boolean ReturnValue = true;

            List <TFormData>dataList = new List <TFormData>();
            MExtractTable ExtractTable;
            Int32 RowCounter = 0;

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(), Catalog.GetString("Create Partner Form Letter"));

            TDBTransaction ReadTransaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    ExtractTable = MExtractAccess.LoadViaMExtractMaster(AExtractId, ReadTransaction);

                    RowCounter = 0;

                    // query all rows of given extract
                    foreach (MExtractRow ExtractRow in ExtractTable.Rows)
                    {
                        RowCounter++;
                        AFormLetterInfo.NextEmailInstance = 0;

                        do
                        {
                            String ShortName;
                            TPartnerClass PartnerClass;
                            TStdPartnerStatusCode PartnerStatusCode;

                            if (MCommonMain.RetrievePartnerShortName(ExtractRow.PartnerKey, out ShortName, out PartnerClass, out PartnerStatusCode,
                                    ReadTransaction))
                            {
                                if (PartnerClass == TPartnerClass.PERSON)
                                {
                                    TFormDataPerson formData = new TFormDataPerson();
                                    AFormLetterInfo.CurrentEmailInstance = AFormLetterInfo.NextEmailInstance;
                                    FillFormDataFromPerson(ExtractRow.PartnerKey, formData, AFormLetterInfo, ExtractRow.SiteKey,
                                        ExtractRow.LocationKey);
                                    // at the moment we include all partners, also the ones that had outdated addresses which have been updated during FillFormDataFromPartner
                                    //if (formData.AddressIsOriginal)
                                    //{
                                    dataList.Add(formData);
                                    //}
                                }
                                else
                                {
                                    TFormDataPartner formData = new TFormDataPartner();
                                    AFormLetterInfo.CurrentEmailInstance = AFormLetterInfo.NextEmailInstance;
                                    FillFormDataFromPartner(ExtractRow.PartnerKey, formData, AFormLetterInfo, ExtractRow.SiteKey,
                                        ExtractRow.LocationKey);
                                    // at the moment we include all partners, also the ones that had outdated addresses which have been updated during FillFormDataFromPartner
                                    //if (formData.AddressIsOriginal)
                                    //{
                                    dataList.Add(formData);
                                    //}
                                }
                            }
                        } while (AFormLetterInfo.NextEmailInstance > AFormLetterInfo.CurrentEmailInstance);

                        if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob)
                        {
                            dataList.Clear();
                            ReturnValue = false;
                            TLogging.Log("Retrieve Partner Form Letter Data - Job cancelled");
                            break;
                        }

                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Retrieving Partner Data"),
                            (RowCounter * 100) / ExtractTable.Rows.Count);
                    }
                });

            TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

            AFormDataList = new List <TFormData>();
            AFormDataList = dataList;
            return ReturnValue;
        }

        /// <summary>
        /// Populate form data for given partner key and list of fields.
        /// This only fills Partner Data, not Personnel or Finance. This method can be called from
        /// Personnel and Finance Form Letter methods to fill Partner Data.
        /// </summary>
        /// <param name="APartnerKey">Key of partner record to be used</param>
        /// <param name="AFormDataPartner">Form Data Object</param>
        /// <param name="AFormLetterInfo">Info class for form letter</param>
        /// <param name="ASiteKey">Site key for location record</param>
        /// <param name="ALocationKey">Key for location record</param>
        /// <returns>returns list with populated form data</returns>
        [RequireModulePermission("PTNRUSER")]
        public static TFormDataPartner FillFormDataFromPartner(Int64 APartnerKey,
            TFormDataPartner AFormDataPartner,
            TFormLetterInfo AFormLetterInfo,
            Int64 ASiteKey = 0,
            Int32 ALocationKey = 0)
        {
            if (AFormDataPartner == null)
            {
                return null;
            }

            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    FillFormDataFromPartner(APartnerKey, AFormDataPartner, AFormLetterInfo, ReadTransaction, ASiteKey, ALocationKey);
                });

            return AFormDataPartner;
        }

        /// <summary>
        /// Populate form data for given partner key and list of fields.
        /// This only fills Partner Data, not Personnel or Finance. This method can be called from
        /// Personnel and Finance Form Letter methods to fill Partner Data.
        /// </summary>
        /// <param name="APartnerKey">Key of partner record to be used</param>
        /// <param name="AFormDataPerson">Form Data Object</param>
        /// <param name="AFormLetterInfo">Info class for form letter</param>
        /// <param name="ASiteKey">Site key for location record</param>
        /// <param name="ALocationKey">Key for location record</param>
        /// <returns>returns list with populated form data</returns>
        [RequireModulePermission("PTNRUSER")]
        public static TFormDataPerson FillFormDataFromPerson(Int64 APartnerKey,
            TFormDataPerson AFormDataPerson,
            TFormLetterInfo AFormLetterInfo,
            Int64 ASiteKey = 0,
            Int32 ALocationKey = 0)
        {
            if (AFormDataPerson == null)
            {
                return null;
            }

            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    FillFormDataFromPerson(APartnerKey, AFormDataPerson, AFormLetterInfo, ReadTransaction, ASiteKey, ALocationKey);
                });

            return AFormDataPerson;
        }

        /// <summary>
        /// Populate form data for given partner key and list of fields.
        /// This only fills Partner Data, not Personnel or Finance. This method can be called from
        /// Personnel and Finance Form Letter methods to fill Partner Data.
        /// </summary>
        /// <param name="APartnerKey">Key of partner record to be used</param>
        /// <param name="AFormDataPartner">form letter data object to be filled</param>
        /// <param name="AFormLetterInfo">Info class for form letter</param>
        /// <param name="AReadTransaction">Db transaction</param>
        /// <param name="ASiteKey">Site key for location record</param>
        /// <param name="ALocationKey">Key for location record</param>
        /// <returns>returns list with populated form data</returns>
        [RequireModulePermission("PTNRUSER")]
        public static void FillFormDataFromPartner(Int64 APartnerKey,
            TFormDataPartner AFormDataPartner,
            TFormLetterInfo AFormLetterInfo,
            TDBTransaction AReadTransaction,
            Int64 ASiteKey = 0,
            Int32 ALocationKey = 0)
        {
            TPartnerClass PartnerClass;
            String ShortName;
            TStdPartnerStatusCode PartnerStatusCode;
            Int64 FamilyKey = 0;

            if (AFormDataPartner == null)
            {
                return;
            }

            if (MCommonMain.RetrievePartnerShortName(APartnerKey, out ShortName, out PartnerClass, out PartnerStatusCode, AReadTransaction))
            {
                // set current date
                AFormDataPartner.CurrentDate = DateTime.Today;

                // retrieve general Partner information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eGeneral))
                {
                    String SiteName;
                    TPartnerClass SiteClass;

                    if (TPartnerServerLookups.GetPartnerShortName(DomainManager.GSiteKey, out SiteName, out SiteClass))
                    {
                        AFormDataPartner.RecordingField = SiteName;
                    }
                }

                // retrieve general Partner information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.ePartner))
                {
                    PPartnerTable PartnerTable;
                    PPartnerRow PartnerRow;
                    PartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, AReadTransaction);

                    if (PartnerTable.Count > 0)
                    {
                        PartnerRow = (PPartnerRow)PartnerTable.Rows[0];

                        AFormDataPartner.PartnerKey = PartnerRow.PartnerKey.ToString("0000000000");
                        AFormDataPartner.PartnerClass = PartnerRow.PartnerClass;
                        AFormDataPartner.StatusCode = PartnerRow.StatusCode;
                        AFormDataPartner.Name = Calculations.FormatShortName(PartnerRow.PartnerShortName, eShortNameFormat.eReverseWithoutTitle);
                        AFormDataPartner.ShortName = PartnerRow.PartnerShortName;
                        AFormDataPartner.LocalName = PartnerRow.PartnerShortNameLoc;
                        AFormDataPartner.AddresseeType = PartnerRow.AddresseeTypeCode;
                        AFormDataPartner.LanguageCode = PartnerRow.LanguageCode;
                        AFormDataPartner.Notes = PartnerRow.Comment;
                        AFormDataPartner.ReceiptLetterFrequency = PartnerRow.ReceiptLetterFrequency;

                        // initialize
                        AFormDataPartner.Title = "";
                        AFormDataPartner.TitleAndSpace = "";

                        if (PartnerClass == TPartnerClass.PERSON)
                        {
                            PPersonTable PersonTable;
                            PPersonRow PersonRow;
                            PersonTable = PPersonAccess.LoadByPrimaryKey(APartnerKey, AReadTransaction);

                            if (PersonTable.Count > 0)
                            {
                                PersonRow = (PPersonRow)PersonTable.Rows[0];

                                AFormDataPartner.FirstName = PersonRow.FirstName;
                                AFormDataPartner.LastName = PersonRow.FamilyName;
                                AFormDataPartner.Title = PersonRow.Title;
                            }
                        }
                        else if (PartnerClass == TPartnerClass.FAMILY)
                        {
                            PFamilyTable FamilyTable;
                            PFamilyRow FamilyRow;
                            FamilyTable = PFamilyAccess.LoadByPrimaryKey(APartnerKey, AReadTransaction);

                            if (FamilyTable.Count > 0)
                            {
                                FamilyRow = (PFamilyRow)FamilyTable.Rows[0];

                                AFormDataPartner.FirstName = FamilyRow.FirstName;
                                AFormDataPartner.LastName = FamilyRow.FamilyName;
                                AFormDataPartner.Title = FamilyRow.Title;
                            }
                        }
                        else
                        {
                            // last name is Partner Short Name
                            // except: if UNIT then don't print partner name (it should be contained in next address line)
                            if (PartnerClass != TPartnerClass.UNIT)
                            {
                                AFormDataPartner.LastName = PartnerRow.PartnerShortName;
                            }
                        }

                        // add space only if first name is not empty
                        AFormDataPartner.FirstNameAndSpace = AFormDataPartner.FirstName;

                        if ((AFormDataPartner.FirstNameAndSpace != null)
                            && (AFormDataPartner.FirstNameAndSpace.Length > 0))
                        {
                            AFormDataPartner.FirstNameAndSpace += " ";
                        }

                        // add space only if last name is not empty
                        AFormDataPartner.LastNameAndSpace = AFormDataPartner.LastName;

                        if ((AFormDataPartner.LastNameAndSpace != null)
                            && (AFormDataPartner.LastNameAndSpace.Length > 0))
                        {
                            AFormDataPartner.LastNameAndSpace += " ";
                        }

                        // add space only if title is not empty
                        AFormDataPartner.TitleAndSpace = AFormDataPartner.Title;

                        if ((AFormDataPartner.TitleAndSpace != null)
                            && (AFormDataPartner.TitleAndSpace.Length > 0))
                        {
                            AFormDataPartner.TitleAndSpace += " ";
                        }
                    }

                    if ((AFormDataPartner.FirstName != null)
                        && (AFormDataPartner.FirstName.Length > 0))
                    {
                        AFormDataPartner.FirstInitial = TFormLetterTools.ConvertIfUpperCase(AFormDataPartner.FirstName.Substring(0, 1), true);
                        AFormDataPartner.FirstInitialAndSpace = AFormDataPartner.FirstInitial;

                        // only add space if first initial is not empty
                        if (AFormDataPartner.FirstInitialAndSpace.Length > 0)
                        {
                            AFormDataPartner.FirstInitialAndSpace += " ";
                        }
                    }
                }

                // retrieve Family member information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eFamilyMember))
                {
                    // Retrieve family key for FAMILY class. In case of PERSON this has already been done earlier.
                    if (PartnerClass == TPartnerClass.PERSON)
                    {
                        PPersonTable PersonTable = PPersonAccess.LoadByPrimaryKey(APartnerKey, AReadTransaction);

                        if (PersonTable.Count > 0)
                        {
                            PPersonRow PersonRow = (PPersonRow)PersonTable.Rows[0];
                            FamilyKey = PersonRow.FamilyKey;
                        }
                    }
                    else if (PartnerClass == TPartnerClass.FAMILY)
                    {
                        FamilyKey = APartnerKey;
                    }

                    if (FamilyKey != 0)
                    {
                        PPersonTable FamilyMembersTable;
                        TFormDataFamilyMember FamilyMemberRecord;
                        String PersonShortName;
                        TPartnerClass PersonClass;
                        FamilyMembersTable = PPersonAccess.LoadViaPFamily(FamilyKey, AReadTransaction);

                        foreach (PPersonRow PersonRow in FamilyMembersTable.Rows)
                        {
                            // only add this person if it is not the main record
                            if (PersonRow.PartnerKey != APartnerKey)
                            {
                                FamilyMemberRecord = new TFormDataFamilyMember();

                                TPartnerServerLookups.GetPartnerShortName(PersonRow.PartnerKey, out PersonShortName, out PersonClass);
                                FamilyMemberRecord.Name = PersonShortName;
                                FamilyMemberRecord.DateOfBirth = PersonRow.DateOfBirth;

                                AFormDataPartner.AddFamilyMember(FamilyMemberRecord);
                            }
                        }
                    }
                }

                // retrieve Contact information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eContact))
                {
                    string Phone;
                    string Email;

                    // retrieve primary phone and primary email
                    TContactDetailsAggregate.GetPrimaryEmailAndPrimaryPhone(APartnerKey, out Phone, out Email);

                    AFormDataPartner.PrimaryPhone = Phone;
                    AFormDataPartner.PrimaryEmail = Email;

                    if (AFormLetterInfo.SplitEmailAddresses && (Email != null) && (Email.Length > 0))
                    {
                        // We have been instructed to split multiple email addresses
                        string[] addresses = StringHelper.SplitEmailAddresses(Email);

                        if (AFormLetterInfo.CurrentEmailInstance < addresses.Length)
                        {
                            // Extract the correct one and use it for this partner instance
                            AFormDataPartner.PrimaryEmail = addresses[AFormLetterInfo.CurrentEmailInstance];
                        }

                        if ((AFormLetterInfo.CurrentEmailInstance + 1) < addresses.Length)
                        {
                            // There is another one available so return the next instance in our FormLetterInfo
                            AFormLetterInfo.NextEmailInstance = AFormLetterInfo.CurrentEmailInstance + 1;
                        }
                        else
                        {
                            AFormLetterInfo.NextEmailInstance = -1;
                        }
                    }

                    // check for skype as it may not often be used
                    // if there is more than one skype id then at the moment the first one found is used
                    if (AFormLetterInfo.ContainsTag("Skype"))
                    {
                        PPartnerAttributeTable AttributeTable = PPartnerAttributeAccess.LoadViaPPartner(APartnerKey, AReadTransaction);

                        foreach (PPartnerAttributeRow AttributeRow in AttributeTable.Rows)
                        {
                            if (AttributeRow.AttributeType == "Skype") // check if we can maybe use constant value instead of string
                            {
                                AFormDataPartner.Skype = AttributeRow.Value;
                                break;
                            }
                        }
                    }
                }

                // retrieve Contact Detail information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eContactDetail))
                {
                    PPartnerAttributeTable ContactDetailTable;
                    TFormDataContactDetail ContactDetailRecord;

                    ContactDetailTable = PPartnerAttributeAccess.LoadViaPPartner(APartnerKey, AReadTransaction);

                    foreach (PPartnerAttributeRow ContactDetailRow in ContactDetailTable.Rows)
                    {
                        // find attribute type row
                        TPartnerCacheable CachePopulator = new TPartnerCacheable();
                        PPartnerAttributeTypeTable TypeTable =
                            (PPartnerAttributeTypeTable)CachePopulator.GetCacheableTable(TCacheablePartnerTablesEnum.ContactTypeList);
                        PPartnerAttributeTypeRow TypeRow = (PPartnerAttributeTypeRow)TypeTable.Rows.Find(
                            ContactDetailRow.AttributeType);

                        if (TypeRow != null)
                        {
                            // find attribute category row from type row
                            PPartnerAttributeCategoryTable CategoryTable =
                                (PPartnerAttributeCategoryTable)CachePopulator.GetCacheableTable(TCacheablePartnerTablesEnum.
                                    ContactCategoryList);
                            PPartnerAttributeCategoryRow CategoryRow =
                                (PPartnerAttributeCategoryRow)CategoryTable.Rows.Find(TypeRow.CategoryCode);

                            // only add to contact details if category is a contact category
                            if (CategoryRow.PartnerContactCategory)
                            {
                                ContactDetailRecord = new TFormDataContactDetail();

                                // retrieve category from attribute type row
                                ContactDetailRecord.Category = TypeRow.CategoryCode;

                                ContactDetailRecord.Type = ContactDetailRow.AttributeType;
                                ContactDetailRecord.Value = ContactDetailRow.Value;
                                ContactDetailRecord.IsCurrent = ContactDetailRow.Current;

                                ContactDetailRecord.IsBusiness = ContactDetailRow.Specialised;
                                ContactDetailRecord.IsConfidential = ContactDetailRow.Confidential;
                                ContactDetailRecord.Comment = ContactDetailRow.Comment;

                                AFormDataPartner.AddContactDetail(ContactDetailRecord);
                            }
                        }
                    }
                }

                // retrieve Subscription information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eSubscription)
                    || ((AFormLetterInfo.FormLetterPrintOptions != null)
                        && (AFormLetterInfo.FormLetterPrintOptions.PublicationCodes.Length > 0)))
                {
                    PSubscriptionTable SubscriptionTable;
                    TFormDataSubscription SubscriptionRecord;

                    SubscriptionTable = PSubscriptionAccess.LoadViaPPartnerPartnerKey(APartnerKey, AReadTransaction);

                    foreach (PSubscriptionRow SubscriptionRow in SubscriptionTable.Rows)
                    {
                        SubscriptionRecord = new TFormDataSubscription();

                        SubscriptionRecord.PublicationCode = SubscriptionRow.PublicationCode;
                        SubscriptionRecord.Status = SubscriptionRow.SubscriptionStatus;
                        SubscriptionRecord.PublicationCopies =
                            SubscriptionRow.IsPublicationCopiesNull() ? 1 : SubscriptionRow.PublicationCopies;

                        AFormDataPartner.AddSubscription(SubscriptionRecord);
                    }
                }

                // retrieve Location and formality information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eLocation)
                    || AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eLocationBlock)
                    || AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eFormalGreetings))
                {
                    PLocationTable LocationTable;
                    PLocationRow LocationRow;
                    String CountryName = "";

                    if (ALocationKey == 0)
                    {
                        // no address set -> retrieve best address
                        TAddressTools.GetBestAddress(APartnerKey, out LocationTable, out CountryName, AReadTransaction);
                    }
                    else
                    {
                        if (PPartnerLocationAccess.Exists(APartnerKey, ASiteKey, ALocationKey, AReadTransaction))
                        {
                            // given location key is found for this partner
                            LocationTable = PLocationAccess.LoadByPrimaryKey(ASiteKey, ALocationKey, AReadTransaction);
                            AFormDataPartner.AddressIsOriginal = true;
                        }
                        else
                        {
                            // given location key not found for this partner
                            // -> update with best address and set flag "AddressIsOriginal" to false
                            TAddressTools.GetBestAddress(APartnerKey, out LocationTable, out CountryName, AReadTransaction);
                            AFormDataPartner.AddressIsOriginal = false;
                        }
                    }

                    if (LocationTable.Count > 0)
                    {
                        LocationRow = (PLocationRow)LocationTable.Rows[0];
                        AFormDataPartner.LocationKey = LocationRow.LocationKey;
                        AFormDataPartner.Address1 = LocationRow.Locality;
                        AFormDataPartner.AddressStreet2 = LocationRow.StreetName;
                        AFormDataPartner.Address3 = LocationRow.Address3;
                        AFormDataPartner.PostalCode = LocationRow.PostalCode;
                        AFormDataPartner.County = LocationRow.County;
                        AFormDataPartner.CountryName = CountryName;
                        AFormDataPartner.City = LocationRow.City;
                        AFormDataPartner.CountryCode = LocationRow.CountryCode;

                        // retrieve country name from country table
                        TCacheable CachePopulator = new TCacheable();
                        PCountryTable CountryTable = (PCountryTable)CachePopulator.GetCacheableTable(TCacheableCommonTablesEnum.CountryList);
                        PCountryRow CountryRow = (PCountryRow)CountryTable.Rows.Find(new object[] { LocationRow.CountryCode });

                        if (CountryRow != null)
                        {
                            AFormDataPartner.CountryName = CountryRow.CountryName;
                            AFormDataPartner.CountryInLocalLanguage = CountryRow.CountryNameLocal;
                        }

                        if (AFormLetterInfo.FormLetterPrintOptions != null)
                        {
                            AFormDataPartner.MailingCode = AFormLetterInfo.FormLetterPrintOptions.MailingCode;
                            AFormDataPartner.PublicationCodes = AFormLetterInfo.FormLetterPrintOptions.PublicationCodes;
                            AFormDataPartner.Enclosures = BuildEnclosuresList(AFormDataPartner, AFormLetterInfo);
                        }
                    }

                    // build address block (need to have retrieved location data beforehand)
                    if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eLocationBlock))
                    {
                        AFormDataPartner.AddressBlock = TFormLetterTools.BuildAddressBlock(AFormDataPartner,
                            AFormLetterInfo.AddressLayoutCode,
                            PartnerClass,
                            AReadTransaction);
                    }

                    // retrieve formality information (need to have retrieved country, language and addressee type beforehand)
                    if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eFormalGreetings))
                    {
                        String SalutationText;
                        String ClosingText;

                        InitializeFormality(AFormLetterInfo, AReadTransaction);
                        AFormLetterInfo.RetrieveFormalityGreeting(AFormDataPartner, out SalutationText, out ClosingText);
                        ResolveGreetingPlaceholders(AFormDataPartner, AFormLetterInfo, APartnerKey, ShortName, PartnerClass, ref SalutationText,
                            ref ClosingText, AReadTransaction);

                        AFormDataPartner.FormalSalutation = SalutationText;
                        AFormDataPartner.FormalClosing = ClosingText;
                    }
                }

                // retrieve Contact Log information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eContactLog))
                {
                    PContactLogTable ContactLogTable;
                    TFormDataContactLog ContactLogRecord;
                    ContactLogTable = PContactLogAccess.LoadViaPPartnerPPartnerContact(APartnerKey, AReadTransaction);

                    foreach (PContactLogRow ContactLogRow in ContactLogTable.Rows)
                    {
                        ContactLogRecord = new TFormDataContactLog();

                        ContactLogRecord.Date = ContactLogRow.ContactDate;
                        ContactLogRecord.Contactor = ContactLogRow.Contactor;
                        ContactLogRecord.ContactCode = ContactLogRow.ContactCode;
                        ContactLogRecord.Notes = ContactLogRow.ContactComment;

                        AFormDataPartner.AddContactLog(ContactLogRecord);
                    }
                }

                // retrieve banking information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eBanking))
                {
                    PBankingDetailsUsageTable BankingDetailsUsageTable = new PBankingDetailsUsageTable();
                    PBankingDetailsUsageRow BankingDetailsUsageTemplateRow = BankingDetailsUsageTable.NewRowTyped(false);
                    BankingDetailsUsageTemplateRow.PartnerKey = APartnerKey;
                    BankingDetailsUsageTemplateRow.Type = MPartnerConstants.BANKINGUSAGETYPE_MAIN;

                    BankingDetailsUsageTable = PBankingDetailsUsageAccess.LoadUsingTemplate(BankingDetailsUsageTemplateRow, AReadTransaction);

                    if (BankingDetailsUsageTable.Count > 0)
                    {
                        // in this case there is a main bank account for this partner
                        PBankingDetailsTable BankingDetailsTable;
                        PBankingDetailsRow BankingDetailsRow;

                        BankingDetailsTable =
                            (PBankingDetailsTable)(PBankingDetailsAccess.LoadByPrimaryKey(((PBankingDetailsUsageRow)BankingDetailsUsageTable.
                                                                                           Rows[0]).
                                                       BankingDetailsKey, AReadTransaction));

                        if (BankingDetailsTable.Count > 0)
                        {
                            BankingDetailsRow = (PBankingDetailsRow)BankingDetailsTable.Rows[0];
                            AFormDataPartner.BankAccountName = BankingDetailsRow.AccountName;
                            AFormDataPartner.BankAccountNumber = BankingDetailsRow.BankAccountNumber;
                            AFormDataPartner.IBANUnformatted = BankingDetailsRow.Iban;
                            //formData.IBANFormatted = ...;

                            // now retrieve bank information
                            PBankTable BankTable;
                            PBankRow BankRow;

                            BankTable = (PBankTable)(PBankAccess.LoadByPrimaryKey(BankingDetailsRow.BankKey, AReadTransaction));

                            if (BankTable.Count > 0)
                            {
                                BankRow = (PBankRow)BankTable.Rows[0];
                                AFormDataPartner.BankName = BankRow.BranchName;
                                AFormDataPartner.BankBranchCode = BankRow.BranchCode;
                                AFormDataPartner.BICSwiftCode = BankRow.Bic;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Populate form data for given partner key and list of fields.
        /// This only fills Partner Data, not Personnel or Finance. This method can be called from
        /// Personnel and Finance Form Letter methods to fill Partner Data.
        /// </summary>
        /// <param name="APartnerKey">Key of partner record to be used</param>
        /// <param name="AFormDataPerson">form letter data object to be filled</param>
        /// <param name="AFormLetterInfo">Info class for form letter</param>
        /// <param name="AReadTransaction">Db transaction</param>
        /// <param name="ASiteKey">Site key for location record</param>
        /// <param name="ALocationKey">Key for location record</param>
        /// <returns>returns list with populated form data</returns>
        [RequireModulePermission("PTNRUSER")]
        public static void FillFormDataFromPerson(Int64 APartnerKey,
            TFormDataPerson AFormDataPerson,
            TFormLetterInfo AFormLetterInfo,
            TDBTransaction AReadTransaction,
            Int64 ASiteKey = 0,
            Int32 ALocationKey = 0)
        {
            if (AFormDataPerson == null)
            {
                return;
            }

            // fill basic partner data
            FillFormDataFromPartner(APartnerKey, AFormDataPerson, AFormLetterInfo, AReadTransaction, ASiteKey, ALocationKey);

            AFormDataPerson.IsPersonRecord = true;

            // retrieve Person information
            if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.ePerson))
            {
                PPersonTable PersonTable;
                PPersonRow PersonRow;
                PersonTable = PPersonAccess.LoadByPrimaryKey(APartnerKey, AReadTransaction);

                if (PersonTable.Count > 0)
                {
                    PersonRow = (PPersonRow)PersonTable.Rows[0];
                    AFormDataPerson.Title = PersonRow.Title;
                    AFormDataPerson.Decorations = PersonRow.Decorations;
                    AFormDataPerson.MiddleName = PersonRow.MiddleName1;
                    AFormDataPerson.PreferredName = PersonRow.PreferedName;
                    AFormDataPerson.AcademicTitle = PersonRow.AcademicTitle;
                    AFormDataPerson.DateOfBirth = PersonRow.DateOfBirth;
                    AFormDataPerson.Gender = PersonRow.Gender;
                    AFormDataPerson.MaritalStatus = PersonRow.MaritalStatus;

                    if (!PersonRow.IsMaritalStatusNull()
                        && (PersonRow.MaritalStatus != ""))
                    {
                        // retrieve marital status description from marital status table
                        TPartnerCacheable CachePopulator = new TPartnerCacheable();
                        PtMaritalStatusTable MaritalStatusTable =
                            (PtMaritalStatusTable)CachePopulator.GetCacheableTable(TCacheablePartnerTablesEnum.MaritalStatusList);
                        PtMaritalStatusRow MaritalStatusRow =
                            (PtMaritalStatusRow)MaritalStatusTable.Rows.Find(new object[] { PersonRow.MaritalStatus });

                        if (MaritalStatusRow != null)
                        {
                            AFormDataPerson.MaritalStatusDesc = MaritalStatusRow.Description;
                        }
                    }

                    AFormDataPerson.OccupationCode = PersonRow.OccupationCode;

                    if (!PersonRow.IsOccupationCodeNull()
                        && (PersonRow.OccupationCode != ""))
                    {
                        // retrieve occupation description from occupation table
                        TPartnerCacheable CachePopulator = new TPartnerCacheable();
                        POccupationTable OccupationTable =
                            (POccupationTable)CachePopulator.GetCacheableTable(TCacheablePartnerTablesEnum.OccupationList);
                        POccupationRow OccupationRow = (POccupationRow)OccupationTable.Rows.Find(new object[] { PersonRow.OccupationCode });

                        if (OccupationRow != null)
                        {
                            AFormDataPerson.Occupation = OccupationRow.OccupationDescription;
                        }
                    }

                    // Get supporting church, if there is one.  (Actually there may be more than one!)
                    // The RelationKey should hold the PERSON key and PartnerKey should hold supporter key
                    PPartnerRelationshipTable tmpTable = new PPartnerRelationshipTable();
                    PPartnerRelationshipRow templateRow = tmpTable.NewRowTyped(false);
                    templateRow.RelationName = "SUPPCHURCH";
                    templateRow.RelationKey = APartnerKey;

                    PPartnerRelationshipTable supportingChurchTable =
                        PPartnerRelationshipAccess.LoadUsingTemplate(templateRow, AReadTransaction);
                    int supportingChurchCount = supportingChurchTable.Rows.Count;

                    // If the user has got RelationKey and PartnerKey back to front we will get no results
                    AFormDataPerson.SendingChurchName = String.Empty;

                    for (int i = 0; i < supportingChurchCount; i++)
                    {
                        // Go round each supporting church
                        // Get the short name for the sending church
                        // Foreign key constraint means that this row is bound to exist
                        string churchName;
                        TPartnerClass churchClass;
                        TStdPartnerStatusCode churchStatus;
                        long supportingChurchKey = ((PPartnerRelationshipRow)supportingChurchTable.Rows[i]).PartnerKey;

                        if (MCommonMain.RetrievePartnerShortName(supportingChurchKey, out churchName, out churchClass, out churchStatus,
                                AReadTransaction))
                        {
                            // The church name can be empty but that would be unusual
                            // churchClass should be CHURCH or ORGANISATION if everything is the right way round
                            // but we do not check this - nor churchStatus
                            if (churchName.Length == 0)
                            {
                                churchName = Catalog.GetString("Not available");
                            }

                            if (supportingChurchCount > 1)
                            {
                                if (i > 0)
                                {
                                    AFormDataPerson.SendingChurchName += Catalog.GetString(" AND ");
                                }

                                AFormDataPerson.SendingChurchName += String.Format("{0}: '{1}'", i + 1, churchName);
                            }
                            else
                            {
                                AFormDataPerson.SendingChurchName += String.Format("'{0}'", churchName);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Populate AFormLetterInfo with relevant formality info if not done so yet
        /// </summary>
        /// <param name="AFormLetterInfo">object to initialize</param>
        /// <param name="AReadTransaction"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        private static void InitializeFormality(TFormLetterInfo AFormLetterInfo,
            TDBTransaction AReadTransaction)
        {
            if (!AFormLetterInfo.IsFormalityInitialized())
            {
                PFormalityTable FormalityTable = PFormalityAccess.LoadAll(AReadTransaction);

                foreach (PFormalityRow formalityRow in FormalityTable.Rows)
                {
                    AFormLetterInfo.AddFormality(formalityRow.LanguageCode,
                        formalityRow.CountryCode,
                        formalityRow.AddresseeTypeCode,
                        formalityRow.FormalityLevel,
                        formalityRow.SalutationText,
                        formalityRow.ComplimentaryClosingText);
                }
            }
        }

        /// <summary>
        /// Resolve placeholders in greetings (salutation and closing text)
        /// </summary>
        /// <param name="AFormData"></param>
        /// <param name="AFormLetterInfo"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerShortName"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ASalutationText"></param>
        /// <param name="AClosingText"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        private static void ResolveGreetingPlaceholders(TFormDataPartner AFormData,
            TFormLetterInfo AFormLetterInfo,
            Int64 APartnerKey,
            String APartnerShortName,
            TPartnerClass APartnerClass,
            ref String ASalutationText,
            ref String AClosingText,
            TDBTransaction ATransaction)
        {
            PPartnerTable PartnerTable = null;
            PPartnerRow PartnerRow = null;
            Boolean Resolved = false;

            // now the salutation and closing have to be amended if they contain insert statements ("<N....>")
            // optimization: only check this if not ORGANIZATION or CHURCH as in this case the greetings may be different (taken from contact partner)
            if ((APartnerClass != TPartnerClass.CHURCH)
                && (APartnerClass != TPartnerClass.ORGANISATION))
            {
                if ((!ASalutationText.Contains("<N"))
                    && (!AClosingText.Contains("<N")))
                {
                    // nothing to resolve
                    return;
                }
            }

            DataTable PartnerSpecificTable = null;

            switch (APartnerClass)
            {
                case TPartnerClass.PERSON:
                    PartnerSpecificTable = PPersonAccess.LoadByPrimaryKey(APartnerKey, ATransaction);
                    break;

                case TPartnerClass.FAMILY:
                    PartnerSpecificTable = PFamilyAccess.LoadByPrimaryKey(APartnerKey, ATransaction);
                    break;

                case TPartnerClass.ORGANISATION:
                    POrganisationRow OrganisationRow;
                    POrganisationTable OrganisationTable = POrganisationAccess.LoadByPrimaryKey(APartnerKey, ATransaction);

                    if (OrganisationTable.Count > 0)
                    {
                        OrganisationRow = (POrganisationRow)OrganisationTable.Rows[0];

                        if (!OrganisationRow.IsContactPartnerKeyNull()
                            && (OrganisationRow.ContactPartnerKey != 0))
                        {
                            PartnerTable = PPartnerAccess.LoadByPrimaryKey(OrganisationRow.ContactPartnerKey, ATransaction);

                            if (PartnerTable.Rows.Count > 0)
                            {
                                PartnerRow = (PPartnerRow)PartnerTable.Rows[0];

                                // In this case the original addressee has changed (it is now the contact partner of the ORGANIZATION).
                                // We have to assume that the country code is the same as for the original addressee.
                                AFormLetterInfo.RetrieveFormalityGreeting(PartnerRow.LanguageCode,
                                    AFormData.CountryCode,
                                    PartnerRow.AddresseeTypeCode,
                                    out ASalutationText,
                                    out AClosingText);

                                // recursive call: use contact partner details instead of organisation details
                                ResolveGreetingPlaceholders(AFormData,
                                    AFormLetterInfo,
                                    OrganisationRow.ContactPartnerKey,
                                    PartnerRow.PartnerShortName,
                                    SharedTypes.PartnerClassStringToEnum(PartnerRow.PartnerClass),
                                    ref ASalutationText,
                                    ref AClosingText,
                                    ATransaction);
                                Resolved = true;
                            }
                            else // contact partner not found --> use organisation
                            {
                                // replace name placeholder with organisation short name
                                PartnerSpecificTable = POrganisationAccess.LoadByPrimaryKey(APartnerKey, ATransaction);
                            }
                        }
                        else
                        {
                            // replace name placeholder with organisation short name
                            PartnerSpecificTable = POrganisationAccess.LoadByPrimaryKey(APartnerKey, ATransaction);
                        }
                    }

                    break;

                case TPartnerClass.CHURCH:
                    PChurchRow ChurchRow;
                    PChurchTable ChurchTable = PChurchAccess.LoadByPrimaryKey(APartnerKey, ATransaction);

                    if (ChurchTable.Count > 0)
                    {
                        ChurchRow = (PChurchRow)ChurchTable.Rows[0];

                        if (!ChurchRow.IsContactPartnerKeyNull()
                            && (ChurchRow.ContactPartnerKey != 0))
                        {
                            PartnerTable = PPartnerAccess.LoadByPrimaryKey(ChurchRow.ContactPartnerKey, ATransaction);

                            if (PartnerTable.Rows.Count > 0)
                            {
                                PartnerRow = (PPartnerRow)PartnerTable.Rows[0];

                                // In this case the original addressee has changed (it is now the contact partner of the CHURCH).
                                // We have to assume that the country code is the same as for the original addressee.
                                AFormLetterInfo.RetrieveFormalityGreeting(PartnerRow.LanguageCode,
                                    AFormData.CountryCode,
                                    PartnerRow.AddresseeTypeCode,
                                    out ASalutationText,
                                    out AClosingText);

                                // recursive call: use contact partner details instead of church details
                                ResolveGreetingPlaceholders(AFormData, AFormLetterInfo, ChurchRow.ContactPartnerKey, PartnerRow.PartnerShortName,
                                    SharedTypes.PartnerClassStringToEnum(
                                        PartnerRow.PartnerClass), ref ASalutationText, ref AClosingText, ATransaction);
                                Resolved = true;
                            }
                            else // contact partner not found --> use church
                            {
                                // replace name placeholder with church short name
                                PartnerSpecificTable = PChurchAccess.LoadByPrimaryKey(APartnerKey, ATransaction);
                            }
                        }
                        else
                        {
                            // replace name placeholder with church short name
                            PartnerSpecificTable = PChurchAccess.LoadByPrimaryKey(APartnerKey, ATransaction);
                        }
                    }

                    break;

                default:
                    break;
            }

            if (!Resolved
                && (PartnerSpecificTable != null)
                && (PartnerSpecificTable.Rows.Count > 0))
            {
                ResolveGreetingPlaceholderText(PartnerSpecificTable.Rows[0],
                    APartnerKey,
                    APartnerShortName,
                    APartnerClass,
                    ref ASalutationText,
                    ATransaction);
                ResolveGreetingPlaceholderText(PartnerSpecificTable.Rows[0],
                    APartnerKey,
                    APartnerShortName,
                    APartnerClass,
                    ref AClosingText,
                    ATransaction);
            }
        }

        /// <summary>
        /// Resolve placeholders in greeting (salutation or closing text)
        /// </summary>
        /// <param name="APartnerSpecificRow"></param>
        /// <param name="APartnerKey"></param>
        /// <param name="APartnerShortName"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="AGreetingText"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        private static void ResolveGreetingPlaceholderText(DataRow APartnerSpecificRow,
            Int64 APartnerKey,
            String APartnerShortName,
            TPartnerClass APartnerClass,
            ref String AGreetingText,
            TDBTransaction ATransaction)
        {
            String ResolvedText = "";
            Boolean InsideBracket = false;
            Boolean BracketResolved = false;

            // now the salutation and closing have to be amended if they contain insert statements ("<N....>")
            if (!AGreetingText.Contains("<N"))
            {
                // nothing to resolve
                return;
            }

            foreach (char c in AGreetingText)
            {
                if (c == '<')
                {
                    if (!InsideBracket)
                    {
                        // open bracket
                        InsideBracket = true;
                        BracketResolved = false;
                    }
                    else
                    {
                        // close bracket
                        InsideBracket = false;
                        ResolvedText.TrimEnd(' ');
                    }

                    // we can skip over this character now
                    continue;
                }

                if (InsideBracket)
                {
                    switch (APartnerClass)
                    {
                        case TPartnerClass.PERSON:

                            switch (c)
                            {
                                case 'T':
                                    ResolvedText += ((PPersonRow)APartnerSpecificRow).Title + " ";
                                    break;

                                case 'P':
                                    ResolvedText += ((PPersonRow)APartnerSpecificRow).FirstName + " ";
                                    break;

                                case 'F':
                                    ResolvedText += ((PPersonRow)APartnerSpecificRow).FamilyName + " ";
                                    break;

                                case 'I':

                                    if (((PPersonRow)APartnerSpecificRow).FirstName.Length > 0)
                                    {
                                        ResolvedText += ((PPersonRow)APartnerSpecificRow).FirstName.Substring(0, 1) + " ";
                                    }

                                    break;

                                case 'A':
                                    ResolvedText += ((PPersonRow)APartnerSpecificRow).AcademicTitle + " ";
                                    break;

                                default:
                                    break;
                            }

                            break;

                        case TPartnerClass.FAMILY:

                            switch (c)
                            {
                                case 'T':
                                    ResolvedText += ((PFamilyRow)APartnerSpecificRow).Title + " ";
                                    break;

                                case 'P':
                                    ResolvedText += ((PFamilyRow)APartnerSpecificRow).FirstName + " ";
                                    break;

                                case 'F':
                                    ResolvedText += ((PFamilyRow)APartnerSpecificRow).FamilyName + " ";
                                    break;

                                case 'I':

                                    if (((PFamilyRow)APartnerSpecificRow).FirstName.Length > 0)
                                    {
                                        ResolvedText += ((PFamilyRow)APartnerSpecificRow).FirstName.Substring(0, 1) + " ";
                                    }

                                    break;

                                default:
                                    break;
                            }

                            break;

                        default:

                            // for everything else just replace contents of <> bracket with short name
                            if (InsideBracket)
                            {
                                if (!BracketResolved)
                                {
                                    ResolvedText += APartnerShortName;
                                    BracketResolved = true;
                                }
                            }

                            break;
                    }
                }
                else
                {
                    // outside brackets: just keep character as part of result string
                    ResolvedText += c;
                }
            }

            AGreetingText = ResolvedText;
        }

        /// <summary>
        /// build and return the address according to country and address layout code
        /// </summary>
        /// <param name="AAddressLayoutBlock"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static String PreviewAddressBlock(String AAddressLayoutBlock)
        {
            String AddressLayoutBlock = AAddressLayoutBlock;
            TFormDataPerson FormDataPerson = new TFormDataPerson();

            // set up dummy data to be used in preview string
            FormDataPerson.PartnerKey = "0000012345";
            FormDataPerson.Title = "Mr.";
            FormDataPerson.FirstName = "Mike";
            FormDataPerson.LastName = "Miller";
            FormDataPerson.ShortName = "Miller, Mike, Mr.";
            FormDataPerson.LocalName = "Miller";
            FormDataPerson.Address1 = "c/o This Company";
            FormDataPerson.AddressStreet2 = "59 Main Street";
            FormDataPerson.Address3 = "New District";
            FormDataPerson.City = "Any Town";
            FormDataPerson.PostalCode = "1234";
            FormDataPerson.LocationKey = 55555;
            FormDataPerson.County = "His County";
            FormDataPerson.CountryCode = "XY";
            FormDataPerson.CountryName = "New Country";
            FormDataPerson.MiddleName = "Adam";
            FormDataPerson.Decorations = "Gold Medallist";
            FormDataPerson.AddresseeType = "1-MALE";
            FormDataPerson.AcademicTitle = "Dr.";
            FormDataPerson.CountryInLocalLanguage = "Local Country";

            if (AddressLayoutBlock.Contains("[[UseContact]]"))
            {
                if (AddressLayoutBlock.Contains("[[Org/Church]]"))
                {
                    AddressLayoutBlock = AddressLayoutBlock.Replace("[[Org/Church]]", "The Organisation");
                }

                AddressLayoutBlock = AddressLayoutBlock.Replace("[[UseContact]]", "");
            }

            // use standard mechanism to build address block string
            // make sure to not use ORGANIZATION or CHURCH since Transaction parameter is null
            return TFormLetterTools.BuildAddressBlock(AddressLayoutBlock, FormDataPerson, TPartnerClass.PERSON, null);
        }

        /// <summary>
        /// Public Method to return the Address Block
        /// </summary>
        /// <param name="AFormData"></param>
        /// <param name="AAddressLayoutCode"></param>
        /// <param name="APartnerClass"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static String BuildAddressBlock(TFormDataPartner AFormData, String AAddressLayoutCode, TPartnerClass APartnerClass)
        {
            String AddressBlock = "";

            TDBTransaction Transaction = null;
            bool SubmissionOk = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref Transaction, ref SubmissionOk,
                delegate
                {
                    AddressBlock = TFormLetterTools.BuildAddressBlock(AFormData, AAddressLayoutCode, APartnerClass, Transaction);
                });

            return AddressBlock;
        }

        /// <summary>
        /// Builds the list of enclosures string
        /// </summary>
        [RequireModulePermission("PTNRUSER")]
        private static string BuildEnclosuresList(TFormDataPartner AFormData, TFormLetterInfo ALetterInfo)
        {
            string publicationsInMailing = "," + AFormData.PublicationCodes + ",";
            string publicationsForThisPartner = string.Empty;
            string ReturnValue = string.Empty;

            foreach (TFormDataSubscription item in AFormData.Subscription)
            {
                if (publicationsInMailing.Contains("," + item.PublicationCode + ","))
                {
                    if (publicationsForThisPartner.Length > 0)
                    {
                        publicationsForThisPartner += ",  ";
                    }

                    publicationsForThisPartner += (item.PublicationCode + ": " + item.PublicationCopies.ToString());
                }
            }

            if (publicationsForThisPartner.Length > 0)
            {
                ReturnValue = "( " + publicationsForThisPartner + " )";
            }

            return ReturnValue;
        }

        /// <summary>
        /// Update the partner subscriptions for specifed publications.  Partner keys come from an extract
        /// </summary>
        /// <param name="AExtractId">Extract ID</param>
        /// <param name="APublicationsCSVList">CSV list of publications</param>
        /// <param name="AResultMessages"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool UpdateSubscriptionsReceivedFromExtract(Int32 AExtractId,
            String APublicationsCSVList,
            out TVerificationResultCollection AResultMessages)
        {
            MExtractTable ExtractTable;
            Int32 RowCounter = 0;
            TVerificationResultCollection messages = null;

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(), Catalog.GetString("Updating Partner Subscriptions"));

            TDBTransaction Transaction = null;
            bool SubmissionOk = false;
            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref Transaction, ref SubmissionOk,
                delegate
                {
                    try
                    {
                        ExtractTable = MExtractAccess.LoadViaMExtractMaster(AExtractId, Transaction);

                        RowCounter = 0;
                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Updating Partner Subscriptions"),
                            0.0m);

                        // query all rows of given extract
                        foreach (MExtractRow ExtractRow in ExtractTable.Rows)
                        {
                            RowCounter++;
                            UpdateSubscriptionsReceived(ExtractRow.PartnerKey, APublicationsCSVList, Transaction);

                            if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob)
                            {
                                TLogging.Log("UpdateSubscriptionsReceivedFromExtract - Job cancelled");
                                break;
                            }

                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Updating Partner Subscriptions"),
                                (RowCounter * 100) / ExtractTable.Rows.Count);
                        }

                        SubmissionOk = true;
                    }
                    catch (Exception ex)
                    {
                        if (TDBExceptionHelper.IsTransactionSerialisationException(ex))
                        {
                            messages = new TVerificationResultCollection();
                            messages.Add(new TVerificationResult("UpdateSubscriptions",
                                    ErrorCodeInventory.RetrieveErrCodeInfo(PetraErrorCodes.ERR_DB_SERIALIZATION_EXCEPTION)));
                        }
                        else
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                    }
                });

            AResultMessages = messages;
            return SubmissionOk;
        }

        /// <summary>
        /// Update the partner subscriptions for specifed publications.  An individual Partner keys is supplied
        /// </summary>
        /// <param name="APartnerKey">The specific partner key</param>
        /// <param name="APublicationsCSVList">CSV list of publications</param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        public static bool UpdateSubscriptionsReceivedForPartner(long APartnerKey, String APublicationsCSVList)
        {
            TDBTransaction Transaction = null;
            bool SubmissionOk = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref Transaction, ref SubmissionOk,
                delegate
                {
                    UpdateSubscriptionsReceived(APartnerKey, APublicationsCSVList, Transaction);
                    SubmissionOk = true;
                });

            return SubmissionOk;
        }

        private static void UpdateSubscriptionsReceived(long APartnerKey, String APublicationsCSVList, TDBTransaction ATransaction)
        {
            bool dataChanged = false;
            string publicationCSVList = "," + APublicationsCSVList + ",";
            PSubscriptionTable subsTable = PSubscriptionAccess.LoadViaPPartnerPartnerKey(APartnerKey, ATransaction);

            foreach (PSubscriptionRow row in subsTable.Rows)
            {
                string pubCode = "," + row.PublicationCode + ",";

                if (publicationCSVList.Contains(pubCode))
                {
                    // Here is a publication to update
                    dataChanged = true;
                    row.NumberIssuesReceived++;
                    row.LastIssue = DateTime.Today;

                    if (!row.FirstIssue.HasValue)
                    {
                        row.FirstIssue = DateTime.Today;
                    }

                    TLogging.LogAtLevel(1,
                        "Updated subscription info for partner " + APartnerKey.ToString() + "   " + row.PublicationCode + "   Issues are now " +
                        row.NumberIssuesReceived.ToString());
                }
            }

            if (dataChanged)
            {
                PSubscriptionAccess.SubmitChanges(subsTable, ATransaction);
            }
        }
    }
}
