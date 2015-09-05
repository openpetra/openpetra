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
                        dataList.Add(FillFormDataFromPartner(ExtractRow.PartnerKey, AFormLetterInfo, ExtractRow.SiteKey, ExtractRow.LocationKey));

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
        /// <param name="AFormLetterInfo">Info class for form letter</param>
        /// <param name="ASiteKey">Site key for location record</param>
        /// <param name="ALocationKey">Key for location record</param>
        /// <returns>returns list with populated form data</returns>
        [RequireModulePermission("PTNRUSER")]
        public static TFormData FillFormDataFromPartner(Int64 APartnerKey,
            TFormLetterInfo AFormLetterInfo,
            Int64 ASiteKey = 0,
            Int32 ALocationKey = 0)
        {
            TPartnerClass PartnerClass;
            String ShortName;
            TStdPartnerStatusCode PartnerStatusCode;

            TFormDataPartner formData = null;

            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    if (MCommonMain.RetrievePartnerShortName(APartnerKey, out ShortName, out PartnerClass, out PartnerStatusCode, ReadTransaction))
                    {
                        switch (PartnerClass)
                        {
                            case TPartnerClass.PERSON:
                                formData = new TFormDataPerson();
                                formData.IsPersonRecord = true;
                                break;

                            default:
                                formData = new TFormDataPartner();
                                formData.IsPersonRecord = false;
                                break;
                        }

                        FillFormDataFromPartner(APartnerKey, ref formData, AFormLetterInfo, ASiteKey, ALocationKey);
                    }
                });

            return formData;
        }

        /// <summary>
        /// Populate form data for given partner key and list of fields.
        /// This only fills Partner Data, not Personnel or Finance. This method can be called from
        /// Personnel and Finance Form Letter methods to fill Partner Data.
        /// </summary>
        /// <param name="APartnerKey">Key of partner record to be used</param>
        /// <param name="AFormDataPartner">form letter data object to be filled</param>
        /// <param name="AFormLetterInfo">Info class for form letter</param>
        /// <param name="ASiteKey">Site key for location record</param>
        /// <param name="ALocationKey">Key for location record</param>
        /// <returns>returns list with populated form data</returns>
        [RequireModulePermission("PTNRUSER")]
        public static void FillFormDataFromPartner(Int64 APartnerKey,
            ref TFormDataPartner AFormDataPartner,
            TFormLetterInfo AFormLetterInfo,
            Int64 ASiteKey = 0,
            Int32 ALocationKey = 0)
        {
            TPartnerClass PartnerClass;
            String ShortName;
            TStdPartnerStatusCode PartnerStatusCode;
            Int64 FamilyKey = 0;

            TFormDataPartner formData = null;

            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    if (MCommonMain.RetrievePartnerShortName(APartnerKey, out ShortName, out PartnerClass, out PartnerStatusCode, ReadTransaction))
                    {
                        switch (PartnerClass)
                        {
                            case TPartnerClass.PERSON:
                                formData = new TFormDataPerson();
                                formData.IsPersonRecord = true;
                                break;

                            default:
                                formData = new TFormDataPartner();
                                formData.IsPersonRecord = false;
                                break;
                        }

                        // set current date
                        formData.CurrentDate = DateTime.Today;

                        // retrieve general Partner information
                        if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eGeneral))
                        {
                            String SiteName;
                            TPartnerClass SiteClass;

                            if (TPartnerServerLookups.GetPartnerShortName(DomainManager.GSiteKey, out SiteName, out SiteClass))
                            {
                                formData.RecordingField = SiteName;
                            }
                        }

                        // retrieve general Partner information
                        if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.ePartner))
                        {
                            PPartnerTable PartnerTable;
                            PPartnerRow PartnerRow;
                            PartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

                            if (PartnerTable.Count > 0)
                            {
                                PartnerRow = (PPartnerRow)PartnerTable.Rows[0];

                                formData.PartnerKey = PartnerRow.PartnerKey.ToString("0000000000");
                                formData.Name = Calculations.FormatShortName(PartnerRow.PartnerShortName, eShortNameFormat.eReverseWithoutTitle);
                                formData.ShortName = PartnerRow.PartnerShortName;
                                formData.LocalName = PartnerRow.PartnerShortNameLoc;
                                formData.AddresseeType = PartnerRow.AddresseeTypeCode;
                                formData.LanguageCode = PartnerRow.LanguageCode;
                                formData.Notes = PartnerRow.Comment;
                                formData.ReceiptLetterFrequency = PartnerRow.ReceiptLetterFrequency;

                                if (PartnerRow.PartnerShortName.Contains(","))
                                {
                                    formData.Title = Calculations.FormatShortName(PartnerRow.PartnerShortName, eShortNameFormat.eOnlyTitle);
                                }
                                else
                                {
                                    formData.Title = "";
                                }
                            }

                            if (PartnerClass == TPartnerClass.PERSON)
                            {
                                PPersonTable PersonTable;
                                PPersonRow PersonRow;
                                PersonTable = PPersonAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

                                if (PersonTable.Count > 0)
                                {
                                    PersonRow = (PPersonRow)PersonTable.Rows[0];

                                    formData.FirstName = PersonRow.FirstName;
                                    formData.LastName = PersonRow.FamilyName;
                                }
                            }
                            else if (PartnerClass == TPartnerClass.FAMILY)
                            {
                                PFamilyTable FamilyTable;
                                PFamilyRow FamilyRow;
                                FamilyTable = PFamilyAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

                                if (FamilyTable.Count > 0)
                                {
                                    FamilyRow = (PFamilyRow)FamilyTable.Rows[0];

                                    formData.FirstName = FamilyRow.FirstName;
                                    formData.LastName = FamilyRow.FamilyName;
                                }
                            }
                        }

                        // retrieve Person information
                        if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.ePerson)
                            && (PartnerClass == TPartnerClass.PERSON)
                            && (formData.GetType() == typeof(TFormDataPerson)))
                        {
                            PPersonTable PersonTable;
                            PPersonRow PersonRow;
                            TFormDataPerson PersonFormData = (TFormDataPerson)formData;
                            PersonTable = PPersonAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);

                            if (PersonTable.Count > 0)
                            {
                                PersonRow = (PPersonRow)PersonTable.Rows[0];
                                PersonFormData.Title = PersonRow.Title;
                                PersonFormData.Decorations = PersonRow.Decorations;
                                PersonFormData.MiddleName = PersonRow.MiddleName1;
                                PersonFormData.PreferedName = PersonRow.PreferedName;
                                PersonFormData.DateOfBirth = PersonRow.DateOfBirth;
                                PersonFormData.Gender = PersonRow.Gender;
                                PersonFormData.MaritalStatus = PersonRow.MaritalStatus;
                                PersonFormData.OccupationCode = PersonRow.OccupationCode;

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
                                        PersonFormData.Occupation = OccupationRow.OccupationDescription;
                                    }
                                }

                                // Get supporting church, if there is one.  (Actually there may be more than one!)
                                // The RelationKey should hold the PERSON key and PartnerKey should hold supporter key
                                PPartnerRelationshipTable tmpTable = new PPartnerRelationshipTable();
                                PPartnerRelationshipRow templateRow = tmpTable.NewRowTyped(false);
                                templateRow.RelationName = "SUPPCHURCH";
                                templateRow.RelationKey = APartnerKey;

                                PPartnerRelationshipTable supportingChurchTable =
                                    PPartnerRelationshipAccess.LoadUsingTemplate(templateRow, ReadTransaction);
                                int supportingChurchCount = supportingChurchTable.Rows.Count;

                                // If the user has got RelationKey and PartnerKey back to front we will get no results
                                PersonFormData.SendingChurchName = String.Empty;

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
                                            ReadTransaction))
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
                                                PersonFormData.SendingChurchName += Catalog.GetString(" AND ");
                                            }

                                            PersonFormData.SendingChurchName += String.Format("{0}: '{1}'", i + 1, churchName);
                                        }
                                        else
                                        {
                                            PersonFormData.SendingChurchName += String.Format("'{0}'", churchName);
                                        }
                                    }
                                }

                                // we need this for later in case we retrieve family members
                                FamilyKey = PersonRow.FamilyKey;

                                // retrieve Special Needs information
                                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eSpecialNeeds))
                                {
                                    PmSpecialNeedTable SpecialNeedTable;
                                    PmSpecialNeedRow SpecialNeedRow;
                                    SpecialNeedTable = PmSpecialNeedAccess.LoadViaPPerson(APartnerKey, ReadTransaction);

                                    if (SpecialNeedTable.Count > 0)
                                    {
                                        SpecialNeedRow = (PmSpecialNeedRow)SpecialNeedTable.Rows[0];
                                        PersonFormData.DietaryNeeds = SpecialNeedRow.DietaryComment;
                                        PersonFormData.MedicalNeeds = SpecialNeedRow.MedicalComment;
                                        PersonFormData.OtherNeeds = SpecialNeedRow.OtherSpecialNeed;
                                        PersonFormData.Vegetarian = SpecialNeedRow.VegetarianFlag;
                                    }
                                }

                                // retrieve Personal Data information
                                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.ePersonalData))
                                {
                                    PmPersonalDataTable PersonalDataTable;
                                    PmPersonalDataRow PersonalDataRow;
                                    PersonalDataTable = PmPersonalDataAccess.LoadViaPPerson(APartnerKey, ReadTransaction);

                                    if (PersonalDataTable.Count > 0)
                                    {
                                        PersonalDataRow = (PmPersonalDataRow)PersonalDataTable.Rows[0];

                                        if (!PersonalDataRow.IsBelieverSinceYearNull()
                                            && (PersonalDataRow.BelieverSinceYear != 0))
                                        {
                                            PersonFormData.YearsBeliever = (DateTime.Today.Year - PersonalDataRow.BelieverSinceYear).ToString();
                                        }

                                        PersonFormData.CommentBeliever = PersonalDataRow.BelieverSinceComment;
                                    }
                                }
                            }
                        }

                        // retrieve Family member information
                        if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eFamilyMember))
                        {
                            // Retrieve family key for FAMILY class. In case of PERSON this has already been done earlier.
                            if (PartnerClass == TPartnerClass.FAMILY)
                            {
                                FamilyKey = APartnerKey;
                            }

                            if (FamilyKey != 0)
                            {
                                PPersonTable FamilyMembersTable;
                                TFormDataFamilyMember FamilyMemberRecord;
                                String PersonShortName;
                                TPartnerClass PersonClass;
                                FamilyMembersTable = PPersonAccess.LoadViaPFamily(FamilyKey, ReadTransaction);

                                foreach (PPersonRow PersonRow in FamilyMembersTable.Rows)
                                {
                                    // only add this person if it is not the main record
                                    if (PersonRow.PartnerKey != APartnerKey)
                                    {
                                        FamilyMemberRecord = new TFormDataFamilyMember();

                                        TPartnerServerLookups.GetPartnerShortName(PersonRow.PartnerKey, out PersonShortName, out PersonClass);
                                        FamilyMemberRecord.Name = PersonShortName;
                                        FamilyMemberRecord.DateOfBirth = PersonRow.DateOfBirth;

                                        formData.AddFamilyMember(FamilyMemberRecord);
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

                            formData.PrimaryPhone = Phone;
                            formData.PrimaryEmail = Email;

                            // check for skype as it may not often be used
                            // if there is more than one skype id then at the moment the first one found is used
                            if (AFormLetterInfo.ContainsTag("Skype"))
                            {
                                PPartnerAttributeTable AttributeTable = PPartnerAttributeAccess.LoadViaPPartner(APartnerKey, ReadTransaction);

                                foreach (PPartnerAttributeRow AttributeRow in AttributeTable.Rows)
                                {
                                    if (AttributeRow.AttributeType == "Skype") // check if we can maybe use constant value instead of string
                                    {
                                        formData.Skype = AttributeRow.Value;
                                        break;
                                    }
                                }
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
                                TAddressTools.GetBestAddress(APartnerKey, out LocationTable, out CountryName, ReadTransaction);
                            }
                            else
                            {
                                LocationTable = PLocationAccess.LoadByPrimaryKey(ASiteKey, ALocationKey, ReadTransaction);
                            }

                            if (LocationTable.Count > 0)
                            {
                                LocationRow = (PLocationRow)LocationTable.Rows[0];
                                formData.LocationKey = LocationRow.LocationKey;
                                formData.Address1 = LocationRow.Locality;
                                formData.AddressStreet2 = LocationRow.StreetName;
                                formData.Address3 = LocationRow.Address3;
                                formData.PostalCode = LocationRow.PostalCode;
                                formData.County = LocationRow.County;
                                formData.CountryName = CountryName;
                                formData.City = LocationRow.City;
                                formData.CountryCode = LocationRow.CountryCode;

                                // retrieve country name from country table
                                TCacheable CachePopulator = new TCacheable();
                                PCountryTable CountryTable = (PCountryTable)CachePopulator.GetCacheableTable(TCacheableCommonTablesEnum.CountryList);
                                PCountryRow CountryRow = (PCountryRow)CountryTable.Rows.Find(new object[] { LocationRow.CountryCode });

                                if (CountryRow != null)
                                {
                                    formData.CountryName = CountryRow.CountryName;
                                    formData.CountryInLocalLanguage = CountryRow.CountryNameLocal;
                                }
                            }

                            // build address block (need to have retrieved location data beforehand)
                            if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eLocationBlock))
                            {
                                formData.AddressBlock = BuildAddressBlock(formData, AFormLetterInfo.AddressLayoutCode, PartnerClass, ReadTransaction);
                            }

                            // retrieve formality information (need to have retrieved country, language and addressee type beforehand)
                            if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eFormalGreetings))
                            {
                                String SalutationText;
                                String ClosingText;

                                InitializeFormality(AFormLetterInfo, ReadTransaction);
                                AFormLetterInfo.RetrieveFormalityGreeting(formData, out SalutationText, out ClosingText);
                                ResolveGreetingPlaceholders(formData, AFormLetterInfo, APartnerKey, ShortName, PartnerClass, ref SalutationText,
                                    ref ClosingText, ReadTransaction);

                                formData.FormalSalutation = SalutationText;
                                formData.FormalClosing = ClosingText;
                            }
                        }

                        // retrieve Contact Log information
                        if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eContactLog))
                        {
                            PContactLogTable ContactLogTable;
                            TFormDataContactLog ContactLogRecord;
                            ContactLogTable = PContactLogAccess.LoadViaPPartnerPPartnerContact(APartnerKey, ReadTransaction);

                            foreach (PContactLogRow ContactLogRow in ContactLogTable.Rows)
                            {
                                ContactLogRecord = new TFormDataContactLog();

                                ContactLogRecord.Contactor = ContactLogRow.Contactor;
                                ContactLogRecord.Notes = ContactLogRow.ContactComment;

                                formData.AddContactLog(ContactLogRecord);
                            }
                        }

                        // retrieve Subscription information
                        if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eSubscription))
                        {
                            PSubscriptionTable SubscriptionTable;
                            TFormDataSubscription SubscriptionRecord;

                            SubscriptionTable = PSubscriptionAccess.LoadViaPPartnerPartnerKey(APartnerKey, ReadTransaction);

                            foreach (PSubscriptionRow SubscriptionRow in SubscriptionTable.Rows)
                            {
                                SubscriptionRecord = new TFormDataSubscription();

                                SubscriptionRecord.PublicationCode = SubscriptionRow.PublicationCode;
                                SubscriptionRecord.Status = SubscriptionRow.SubscriptionStatus;

                                formData.AddSubscription(SubscriptionRecord);
                            }
                        }

                        // retrieve banking information
                        if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eBanking))
                        {
                            PBankingDetailsUsageTable BankingDetailsUsageTable = new PBankingDetailsUsageTable();
                            PBankingDetailsUsageRow BankingDetailsUsageTemplateRow = BankingDetailsUsageTable.NewRowTyped(false);
                            BankingDetailsUsageTemplateRow.PartnerKey = APartnerKey;
                            BankingDetailsUsageTemplateRow.Type = MPartnerConstants.BANKINGUSAGETYPE_MAIN;

                            BankingDetailsUsageTable = PBankingDetailsUsageAccess.LoadUsingTemplate(BankingDetailsUsageTemplateRow, ReadTransaction);

                            if (BankingDetailsUsageTable.Count > 0)
                            {
                                // in this case there is a main bank account for this partner
                                PBankingDetailsTable BankingDetailsTable;
                                PBankingDetailsRow BankingDetailsRow;

                                BankingDetailsTable =
                                    (PBankingDetailsTable)(PBankingDetailsAccess.LoadByPrimaryKey(((PBankingDetailsUsageRow)BankingDetailsUsageTable.
                                                                                                   Rows[0]).
                                                               BankingDetailsKey, ReadTransaction));

                                if (BankingDetailsTable.Count > 0)
                                {
                                    BankingDetailsRow = (PBankingDetailsRow)BankingDetailsTable.Rows[0];
                                    formData.BankAccountName = BankingDetailsRow.AccountName;
                                    formData.BankAccountNumber = BankingDetailsRow.BankAccountNumber;
                                    formData.IBANUnformatted = BankingDetailsRow.Iban;
                                    //formData.IBANFormatted = ...;

                                    // now retrieve bank information
                                    PBankTable BankTable;
                                    PBankRow BankRow;

                                    BankTable = (PBankTable)(PBankAccess.LoadByPrimaryKey(BankingDetailsRow.BankKey, ReadTransaction));

                                    if (BankTable.Count > 0)
                                    {
                                        BankRow = (PBankRow)BankTable.Rows[0];
                                        formData.BankName = BankRow.BranchName;
                                        formData.BankBranchCode = BankRow.BranchCode;
                                        formData.BICSwiftCode = BankRow.Bic;
                                    }
                                }
                            }
                        }

                        if ((PartnerClass == TPartnerClass.PERSON)
                            && (formData.GetType() == typeof(TFormDataPerson)))
                        {
                            // retrieve Passport information
                            if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.ePassport))
                            {
                                PmPassportDetailsTable PassportTable;
                                TFormDataPassport PassportRecord;
                                PassportTable = PmPassportDetailsAccess.LoadViaPPerson(APartnerKey, ReadTransaction);

                                foreach (PmPassportDetailsRow PassportRow in PassportTable.Rows)
                                {
                                    PassportRecord = new TFormDataPassport();

                                    PassportRecord.PassportName = PassportRow.FullPassportName;
                                    PassportRecord.NationalityCode = PassportRow.PassportNationalityCode;

                                    // retrieve country name from country table
                                    TCacheable CachePopulator = new TCacheable();
                                    PCountryTable CountryTable =
                                        (PCountryTable)CachePopulator.GetCacheableTable(TCacheableCommonTablesEnum.CountryList);
                                    PCountryRow CountryRow = (PCountryRow)CountryTable.Rows.Find(new object[] { PassportRow.PassportNationalityCode });

                                    if (CountryRow != null)
                                    {
                                        PassportRecord.NationalityName = CountryRow.CountryName;
                                    }

                                    PassportRecord.TypeCode = PassportRow.PassportDetailsType;
                                    // retrieve passport type name from type table
                                    TPersonnelCacheable PersonnelCachePopulator = new TPersonnelCacheable();
                                    PtPassportTypeTable TypeTable =
                                        (PtPassportTypeTable)PersonnelCachePopulator.GetCacheableTable(TCacheablePersonTablesEnum.PassportTypeList);
                                    PtPassportTypeRow TypeRow = (PtPassportTypeRow)TypeTable.Rows.Find(new object[] { PassportRow.PassportDetailsType });

                                    if (TypeRow != null)
                                    {
                                        PassportRecord.TypeDescription = TypeRow.Description;
                                    }

                                    // set number and nationality in main record (only for main passport or if there is just one)
                                    if (PassportRow.MainPassport || (PassportTable.Count == 1))
                                    {
                                        ((TFormDataPerson)formData).PassportNumber = PassportRow.PassportNumber;
                                        ((TFormDataPerson)formData).PassportNationality = PassportRow.PassportNationalityCode;
                                        ((TFormDataPerson)formData).PassportName = PassportRow.FullPassportName;
                                    }

                                    PassportRecord.DateOfIssue = PassportRow.DateOfIssue;
                                    PassportRecord.PlaceOfIssue = PassportRow.PlaceOfIssue;
                                    PassportRecord.DateOfExpiry = PassportRow.DateOfExpiration;
                                    PassportRecord.PlaceOfBirth = PassportRow.PlaceOfBirth;

                                    ((TFormDataPerson)formData).AddPassport(PassportRecord);
                                }
                            }

                            // retrieve Language information
                            if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eLanguage))
                            {
                                PmPersonLanguageTable PersonLanguageTable;
                                TFormDataLanguage PersonLanguageRecord;
                                PersonLanguageTable = PmPersonLanguageAccess.LoadViaPPerson(APartnerKey, ReadTransaction);

                                foreach (PmPersonLanguageRow PersonLanguageRow in PersonLanguageTable.Rows)
                                {
                                    PersonLanguageRecord = new TFormDataLanguage();

                                    PersonLanguageRecord.Code = PersonLanguageRow.LanguageCode;

                                    // retrieve language name from language table
                                    TCacheable CachePopulator = new TCacheable();
                                    PLanguageTable LanguageTable =
                                        (PLanguageTable)CachePopulator.GetCacheableTable(TCacheableCommonTablesEnum.LanguageCodeList);
                                    PLanguageRow LanguageRow = (PLanguageRow)LanguageTable.Rows.Find(new object[] { PersonLanguageRow.LanguageCode });

                                    if (LanguageRow != null)
                                    {
                                        PersonLanguageRecord.Name = LanguageRow.LanguageDescription;
                                    }

                                    PersonLanguageRecord.Level = PersonLanguageRow.LanguageLevel.ToString();

                                    // retrieve language level name from language level table
                                    TPersonnelCacheable CachePopulatorPersonnel = new TPersonnelCacheable();
                                    PtLanguageLevelTable LanguageLevelTable =
                                        (PtLanguageLevelTable)CachePopulatorPersonnel.GetCacheableTable(TCacheablePersonTablesEnum.LanguageLevelList);
                                    PtLanguageLevelRow LanguageLevelRow =
                                        (PtLanguageLevelRow)LanguageLevelTable.Rows.Find(new object[] { PersonLanguageRow.LanguageLevel });

                                    if (LanguageLevelRow != null)
                                    {
                                        PersonLanguageRecord.LevelDesc = LanguageLevelRow.LanguageLevelDescr;
                                    }

                                    PersonLanguageRecord.Comment = PersonLanguageRow.Comment;

                                    ((TFormDataPerson)formData).AddLanguage(PersonLanguageRecord);
                                }
                            }

                            // retrieve Skill information
                            if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eSkill))
                            {
                                PmPersonSkillTable PersonSkillTable;
                                TFormDataSkill PersonSkillRecord;
                                PersonSkillTable = PmPersonSkillAccess.LoadViaPPerson(APartnerKey, ReadTransaction);

                                foreach (PmPersonSkillRow PersonSkillRow in PersonSkillTable.Rows)
                                {
                                    PersonSkillRecord = new TFormDataSkill();

                                    PersonSkillRecord.Category = PersonSkillRow.SkillCategoryCode;
                                    PersonSkillRecord.Description = PersonSkillRow.DescriptionEnglish;

                                    // if no description in local language then use english
                                    PersonSkillRecord.DescriptionLocalOrDefault = PersonSkillRow.DescriptionLocal;

                                    if (PersonSkillRow.DescriptionLocal != "")
                                    {
                                        PersonSkillRecord.DescriptionLocalOrDefault = PersonSkillRow.DescriptionEnglish;
                                    }

                                    PersonSkillRecord.Level = PersonSkillRow.SkillLevel;

                                    // retrieve skill level name from skill level table
                                    TPersonnelCacheable CachePopulatorPersonnel = new TPersonnelCacheable();
                                    PtSkillLevelTable SkillLevelTable =
                                        (PtSkillLevelTable)CachePopulatorPersonnel.GetCacheableTable(TCacheablePersonTablesEnum.SkillLevelList);
                                    PtSkillLevelRow SkillLevelRow =
                                        (PtSkillLevelRow)SkillLevelTable.Rows.Find(new object[] { PersonSkillRow.SkillLevel });

                                    if (SkillLevelRow != null)
                                    {
                                        PersonSkillRecord.LevelDesc = SkillLevelRow.Description;
                                    }

                                    PersonSkillRecord.YearsExp = PersonSkillRow.YearsOfExperience;
                                    PersonSkillRecord.Professional = PersonSkillRow.ProfessionalSkill;
                                    PersonSkillRecord.Degree = PersonSkillRow.Degree;
                                    PersonSkillRecord.Comment = PersonSkillRow.Comment;

                                    ((TFormDataPerson)formData).AddSkill(PersonSkillRecord);
                                }
                            }

                            // retrieve past work experience information
                            if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eWorkExperience))
                            {
                                TFormDataWorkExperience PersonExpRecord;
                                String UnitShortName;
                                TPartnerClass UnitClass;

                                // retrieve applications for short term events
                                String SqlStmt = TDataBase.ReadSqlFile("Personnel.FormLetters.GetAppTravelDates.sql");

                                OdbcParameter[] parameters = new OdbcParameter[1];
                                parameters[0] = new OdbcParameter("PartnerKey", OdbcType.BigInt);
                                parameters[0].Value = APartnerKey;

                                DataTable travelData = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "TravelDates", ReadTransaction, parameters);

                                for (int i = 0; i < travelData.Rows.Count; i++)
                                {
                                    PersonExpRecord = new TFormDataWorkExperience();

                                    if ((travelData.Rows[i][0]).GetType() == typeof(DateTime))
                                    {
                                        PersonExpRecord.StartDate = (DateTime?)travelData.Rows[i][0];
                                    }

                                    if ((travelData.Rows[i][1]).GetType() == typeof(DateTime))
                                    {
                                        PersonExpRecord.EndDate = (DateTime?)travelData.Rows[i][1];
                                    }

                                    PersonExpRecord.Organisation = "";
                                    PersonExpRecord.Role = "";
                                    PersonExpRecord.Category = "";
                                    PersonExpRecord.SameOrg = true;
                                    PersonExpRecord.SimilarOrg = true;
                                    PersonExpRecord.Comment = "";

                                    // check if unit exists and use unit name as location
                                    if (TPartnerServerLookups.GetPartnerShortName((Int64)travelData.Rows[i][2], out UnitShortName, out UnitClass))
                                    {
                                        PersonExpRecord.Location = UnitShortName;
                                    }
                                    else
                                    {
                                        PersonExpRecord.Location = travelData.Rows[i][3].ToString();
                                    }

                                    ((TFormDataPerson)formData).AddWorkExperience(PersonExpRecord);
                                }

                                // retrieve actual past experience records
                                PmPastExperienceTable PersonExpTable;
                                PersonExpTable = PmPastExperienceAccess.LoadViaPPerson(APartnerKey, ReadTransaction);

                                foreach (PmPastExperienceRow PersonExpRow in PersonExpTable.Rows)
                                {
                                    PersonExpRecord = new TFormDataWorkExperience();

                                    PersonExpRecord.StartDate = PersonExpRow.StartDate;
                                    PersonExpRecord.EndDate = PersonExpRow.EndDate;
                                    PersonExpRecord.Location = PersonExpRow.PrevLocation;
                                    PersonExpRecord.Organisation = PersonExpRow.OtherOrganisation;
                                    PersonExpRecord.Role = PersonExpRow.PrevRole;
                                    PersonExpRecord.Category = PersonExpRow.Category;
                                    PersonExpRecord.SameOrg = PersonExpRow.PrevWorkHere;
                                    PersonExpRecord.SimilarOrg = PersonExpRow.PrevWork;
                                    PersonExpRecord.Comment = PersonExpRow.PastExpComments;

                                    ((TFormDataPerson)formData).AddWorkExperience(PersonExpRecord);
                                }
                            }
                        }
                    }
                });

            AFormDataPartner = formData;
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
            return BuildAddressBlock(AddressLayoutBlock, FormDataPerson, TPartnerClass.PERSON, null);
        }

        /// <summary>
        /// build and return the address according to country and address layout code
        /// </summary>
        /// <param name="AFormData"></param>
        /// <param name="AAddressLayoutCode"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        private static String BuildAddressBlock(TFormDataPartner AFormData,
            String AAddressLayoutCode,
            TPartnerClass APartnerClass,
            TDBTransaction ATransaction)
        {
            PAddressBlockTable AddressBlockTable;
            String AddressLayoutBlock = "";

            if ((AAddressLayoutCode == null)
                || (AAddressLayoutCode == ""))
            {
                // this should not happen but just in case we use SMLLABEL as default layout code
                // NOTE: AlanP for the present we do not support address blocks for multiple languages, so the following has been fixed at 99
                //AddressBlockTable = PAddressBlockAccess.LoadByPrimaryKey(AFormData.CountryCode, "SMLLABEL", ATransaction);
                AddressBlockTable = PAddressBlockAccess.LoadByPrimaryKey("99", "SMLLABEL", ATransaction);
            }
            else
            {
                // NOTE: AlanP for the present we do not support address blocks for multiple languages, so the following has been fixed at 99
                //AddressBlockTable = PAddressBlockAccess.LoadByPrimaryKey(AFormData.CountryCode, AAddressLayoutCode, ATransaction);
                AddressBlockTable = PAddressBlockAccess.LoadByPrimaryKey("99", AAddressLayoutCode, ATransaction);
            }

            if (AddressBlockTable.Count == 0)
            {
                return "";
            }
            else
            {
                PAddressBlockRow AddressBlockRow = (PAddressBlockRow)AddressBlockTable.Rows[0];
                AddressLayoutBlock = AddressBlockRow.AddressBlockText;
            }

            return BuildAddressBlock(AddressLayoutBlock, AFormData, APartnerClass, ATransaction);
        }

        /// <summary>
        /// build and return the address according to the template address layout block
        /// </summary>
        /// <param name="AAddressLayoutBlock"></param>
        /// <param name="AFormData"></param>
        /// <param name="APartnerClass"></param>
        /// <param name="ATransaction"></param>
        /// <returns></returns>
        [RequireModulePermission("PTNRUSER")]
        private static String BuildAddressBlock(String AAddressLayoutBlock,
            TFormDataPartner AFormData,
            TPartnerClass APartnerClass,
            TDBTransaction ATransaction)
        {
            String AddressBlock = "";

            List <String>AddressTokenList = new List <String>();
            String AddressLineText = "";
            Boolean PrintAnyway = false;
            Boolean CapsOn = false;
            Boolean UseContact = false;

            PPersonTable PersonTable;
            PPersonRow PersonRow = null;
            PFamilyTable FamilyTable;
            PFamilyRow FamilyRow = null;
            Int64 ContactPartnerKey = 0;


            AddressTokenList = BuildTokenListFromAddressLayoutBlock(AAddressLayoutBlock);

            // initialize values
            AddressLineText = "";
            PrintAnyway = false;

            foreach (String AddressLineToken in AddressTokenList)
            {
                switch (AddressLineToken)
                {
                    case "[[AcademicTitle]]":

                        if (UseContact)
                        {
                            if (PersonRow != null)
                            {
                                AddressLineText += ConvertIfUpperCase(PersonRow.AcademicTitle, CapsOn);
                            }
                        }
                        else
                        {
                            if (AFormData.GetType() == typeof(TFormDataPerson))
                            {
                                AddressLineText += ConvertIfUpperCase(((TFormDataPerson)AFormData).AcademicTitle, CapsOn);
                            }
                        }

                        break;

                    case "[[AddresseeType]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.AddresseeType, CapsOn);
                        break;

                    case "[[Address3]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.Address3, CapsOn);
                        break;

                    case "[[CapsOn]]":
                        CapsOn = true;
                        break;

                    case "[[CapsOff]]":
                        CapsOn = false;
                        break;

                    case "[[City]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.City, CapsOn);
                        break;

                    case "[[CountryName]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.CountryName, CapsOn);
                        break;

                    case "[[CountryInLocalLanguage]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.CountryInLocalLanguage, CapsOn);
                        break;

                    case "[[County]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.County, CapsOn);
                        break;

                    case "[[UseContact]]":

                        /* Get the person or family record that is acting as the contact
                         *  only applicable to churches and organisations. */
                        switch (APartnerClass)
                        {
                            case TPartnerClass.CHURCH:
                                PChurchTable ChurchTable;
                                PChurchRow ChurchRow;
                                ChurchTable = PChurchAccess.LoadByPrimaryKey(Convert.ToInt64(AFormData.PartnerKey), ATransaction);

                                if (ChurchTable.Count > 0)
                                {
                                    ChurchRow = (PChurchRow)ChurchTable.Rows[0];

                                    if (!ChurchRow.IsContactPartnerKeyNull())
                                    {
                                        ContactPartnerKey = ChurchRow.ContactPartnerKey;
                                    }
                                }

                                break;

                            case TPartnerClass.ORGANISATION:
                                POrganisationTable OrganisationTable;
                                POrganisationRow OrganisationRow;
                                OrganisationTable = POrganisationAccess.LoadByPrimaryKey(Convert.ToInt64(AFormData.PartnerKey), ATransaction);

                                if (OrganisationTable.Count > 0)
                                {
                                    OrganisationRow = (POrganisationRow)OrganisationTable.Rows[0];

                                    if (!OrganisationRow.IsContactPartnerKeyNull())
                                    {
                                        ContactPartnerKey = OrganisationRow.ContactPartnerKey;
                                    }
                                }

                                break;

                            default:
                                ContactPartnerKey = 0;
                                break;
                        }

                        if (ContactPartnerKey > 0)
                        {
                            PersonTable = PPersonAccess.LoadByPrimaryKey(ContactPartnerKey, ATransaction);

                            if (PersonTable.Count > 0)
                            {
                                PersonRow = (PPersonRow)PersonTable.Rows[0];
                            }
                            else
                            {
                                FamilyTable = PFamilyAccess.LoadByPrimaryKey(ContactPartnerKey, ATransaction);

                                if (FamilyTable.Count > 0)
                                {
                                    FamilyRow = (PFamilyRow)FamilyTable.Rows[0];
                                }
                            }
                        }

                        UseContact = true;
                        break;

                    case "[[CountryCode]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.CountryCode, CapsOn);
                        break;

                    case "[[Decorations]]":

                        if (UseContact)
                        {
                            if (PersonRow != null)
                            {
                                AddressLineText += ConvertIfUpperCase(PersonRow.Decorations, CapsOn);
                            }
                        }
                        else
                        {
                            if (AFormData.GetType() == typeof(TFormDataPerson))
                            {
                                AddressLineText += ConvertIfUpperCase(((TFormDataPerson)AFormData).Decorations, CapsOn);
                            }
                        }

                        break;

                    case "[[FirstName]]":

                        if (UseContact)
                        {
                            if (PersonRow != null)
                            {
                                AddressLineText += ConvertIfUpperCase(PersonRow.FirstName, CapsOn);
                            }
                            else if (FamilyRow != null)
                            {
                                AddressLineText += ConvertIfUpperCase(FamilyRow.FirstName, CapsOn);
                            }
                        }
                        else
                        {
                            AddressLineText += ConvertIfUpperCase(AFormData.FirstName, CapsOn);
                        }

                        break;

                    case "[[FirstInitial]]":

                        if (UseContact)
                        {
                            if (PersonRow != null)
                            {
                                if (PersonRow.FirstName.Length > 0)
                                {
                                    AddressLineText += ConvertIfUpperCase(PersonRow.FirstName.Substring(0, 1), CapsOn);
                                }
                            }
                            else if (FamilyRow != null)
                            {
                                if (PersonRow.FirstName.Length > 0)
                                {
                                    AddressLineText += ConvertIfUpperCase(FamilyRow.FirstName.Substring(0, 1), CapsOn);
                                }
                            }
                        }
                        else
                        {
                            if (AFormData.FirstName.Length > 0)
                            {
                                AddressLineText += ConvertIfUpperCase(AFormData.FirstName.Substring(0, 1), CapsOn);
                            }
                        }

                        break;

                    case "[[LastName]]":

                        if (UseContact)
                        {
                            if (PersonRow != null)
                            {
                                AddressLineText += ConvertIfUpperCase(PersonRow.FamilyName, CapsOn);
                            }
                            else if (FamilyRow != null)
                            {
                                AddressLineText += ConvertIfUpperCase(FamilyRow.FamilyName, CapsOn);
                            }
                        }
                        else
                        {
                            AddressLineText += ConvertIfUpperCase(AFormData.LastName, CapsOn);
                        }

                        break;

                    case "[[Address1]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.Address1, CapsOn);
                        break;

                    case "[[MiddleName]]":

                        if (UseContact)
                        {
                            if (PersonRow != null)
                            {
                                AddressLineText += ConvertIfUpperCase(PersonRow.MiddleName1, CapsOn);
                            }
                        }
                        else
                        {
                            if (AFormData.GetType() == typeof(TFormDataPerson))
                            {
                                AddressLineText += ConvertIfUpperCase(((TFormDataPerson)AFormData).MiddleName, CapsOn);
                            }
                        }

                        break;

                    case "[[Org/Church]]":

                        /* if the contact person is being printed then might still want the
                         *  Organisation or Church name printed.  This does it but only if there
                         *  is a valid contact. */
                        if (UseContact)
                        {
                            AddressLineText += ConvertIfUpperCase(AFormData.ShortName, CapsOn);
                        }

                        break;

                    case "[[PartnerKey]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.PartnerKey, CapsOn);
                        break;

                    case "[[ShortName]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.ShortName, CapsOn);
                        break;

                    case "[[LocalName]]":

                        if (AFormData.LocalName != "")
                        {
                            AddressLineText += ConvertIfUpperCase(AFormData.LocalName, CapsOn);
                        }
                        else
                        {
                            AddressLineText += ConvertIfUpperCase(AFormData.ShortName, CapsOn);
                        }

                        break;

                    case "[[PostalCode]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.PostalCode, CapsOn);
                        break;

                    case "[[Tab]]":
                        AddressLineText += "\t";
                        break;

                    case "[[Space]]":
                        AddressLineText += " ";
                        break;

                    case "[[AddressStreet2]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.AddressStreet2, CapsOn);
                        break;

                    case "[[Title]]":
                        AddressLineText += ConvertIfUpperCase(AFormData.Title, CapsOn);
                        break;

                    case "[[NoSuppress]]":
                        PrintAnyway = true;
                        break;

                    case "[[LocationKey]]":
                        AddressLineText += AFormData.LocationKey;
                        break;

                    case "[[LineFeed]]":

                        // only add line if not empty and not suppressed
                        if (PrintAnyway
                            || (!PrintAnyway
                                && (AddressLineText.Trim() != "")))
                        {
                            AddressBlock += AddressLineText + "\r\n";
                        }

                        // reset values
                        AddressLineText = "";
                        PrintAnyway = false;
                        break;

                    default:
                        AddressLineText += ConvertIfUpperCase(AddressLineToken, CapsOn);
                        break;
                }
            }

            // this is only for last line if there was no line feed:
            // only add line if not empty and not suppressed
            if (PrintAnyway
                || (!PrintAnyway
                    && (AddressLineText.Trim() != "")))
            {
                AddressBlock += AddressLineText + "\r\n";
            }

            // or just get the element list from cached table (since we need to get different ones depending on country)

            return AddressBlock;
        }

        /// <summary>
        /// build and return the address according to country and address layout code
        /// </summary>
        /// <param name="AAddressLayoutBlock"></param>
        /// <returns>list of token built from address layout string</returns>
        [RequireModulePermission("PTNRUSER")]
        private static List <String>BuildTokenListFromAddressLayoutBlock(String AAddressLayoutBlock)
        {
            List <String>TokenList = new List <String>();
            String AddressBlock = AAddressLayoutBlock;
            Int32 IndexStartToken;
            Int32 IndexEndToken;

            AddressBlock = AddressBlock.Replace("\r\n", "[[LineFeed]]");

            do
            {
                IndexStartToken = AddressBlock.IndexOf("[[");

                if (IndexStartToken == 0)
                {
                    // we have reached a real token --> find index of end of token
                    IndexEndToken = AddressBlock.IndexOf("]]");
                    TokenList.Add(AddressBlock.Substring(0, IndexEndToken + 2));
                    AddressBlock = AddressBlock.Substring(IndexEndToken + 2);
                }
                else if (IndexStartToken > 0)
                {
                    // this is normal text before the next token --> just add this whole text as one "token"
                    TokenList.Add(AddressBlock.Substring(0, IndexStartToken));
                    AddressBlock = AddressBlock.Substring(IndexStartToken);
                }
                else if (IndexStartToken < 0)
                {
                    // no more token to be found --> just append rest of string
                    TokenList.Add(AddressBlock);
                    AddressBlock = "";
                }
            } while (AddressBlock.Length > 0);

            return TokenList;
        }

        /// <summary>
        /// convert a string to uppercase if needed (or otherwise return as is)
        /// </summary>
        /// <param name="AString"></param>
        /// <param name="AConvertToUpperCase"></param>
        /// <returns></returns>
        private static String ConvertIfUpperCase(String AString, Boolean AConvertToUpperCase)
        {
            if (AConvertToUpperCase)
            {
                return AString.ToUpper();
            }

            return AString;
        }
    }
}
