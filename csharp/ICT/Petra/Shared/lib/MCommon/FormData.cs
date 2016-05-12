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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Ict.Common;
using Ict.Petra.Shared.MCommon;

namespace Ict.Petra.Shared.MCommon
{
    /// <summary>
    /// enum to determine target platform for form data
    /// </summary>
    public enum TFormDataTarget
    {
        /// <summary>
        /// target is Excel (.xlsx)
        /// </summary>
        Excel,

        /// <summary>
        /// target is Word (.docx)
        /// </summary>
        Word
    }

    /// <summary>
    /// enum to determine target platform for form data
    /// </summary>
    public enum TFormDataRetrievalSection
    {
        /// <summary>
        /// retrieve general system info
        /// </summary>
        eGeneral,

        /// <summary>
        /// retrieve formality related data
        /// </summary>
            eFormalGreetings,

        /// <summary>
        /// retrieve info from Partner Table
        /// </summary>
            ePartner,

        /// <summary>
        /// retrieve info from Person Table
        /// </summary>
            ePerson,

        /// <summary>
        /// retrieve info from Gift Table
        /// </summary>
            eGiftTotal,

        /// <summary>
        /// retrieve info from Gift Detail Table
        /// </summary>
            eGiftDetail,

        /// <summary>
        /// retrieve info from Location Table
        /// </summary>
            eLocation,

        /// <summary>
        /// retrieve info from Location Table for address layout block
        /// </summary>
            eLocationBlock,

        /// <summary>
        /// retrieve info from Contacts Table
        /// </summary>
            eContact,

        /// <summary>
        /// retrieve info from Contact Log Table
        /// </summary>
            eContactLog,

        /// <summary>
        /// retrieve info from Subscription Table
        /// </summary>
            eSubscription,

        /// <summary>
        /// retrieve info from Bank tables
        /// </summary>
            eBanking,

        /// <summary>
        /// retrieve info from Language Table
        /// </summary>
            eLanguage,

        /// <summary>
        /// retrieve info from Passport Table
        /// </summary>
            ePassport,

        /// <summary>
        /// retrieve info from past experience Table
        /// </summary>
            eWorkExperience,

        /// <summary>
        /// retrieve info from Skill Table
        /// </summary>
            eSkill,

        /// <summary>
        /// retrieve basic info for family members
        /// </summary>
            eFamilyMember,

        /// <summary>
        /// retrieve info from Special Needs Table
        /// </summary>
            eSpecialNeeds,

        /// <summary>
        /// retrieve info from Personal Data Table
        /// </summary>
            ePersonalData
    }

    /// <summary>
    /// Class to keep information about form letter creation process
    /// </summary>
    [Serializable()]
    public class TFormLetterInfo
    {
        /// <summary>
        /// Class to keep information about formality records
        /// </summary>
        public class Formality
        {
            /// Language Code for Formality
            public String LanguageCode;
            /// Country Code for Formality
            public String CountryCode;
            /// Addressee Type Code for Formality
            public String AddresseeTypeCode;
            /// Formality Level for Formality
            public Int32 FormalityLevel;
            /// Salutation Text for Formality
            public String SalutationText;
            /// Closing Text for Formality
            public String ClosingText;
        }

        /// <summary>
        /// Class to keep information about Form Letter printing options
        /// </summary>
        [Serializable()]
        public class TFormLetterPrintOptions
        {
            /// The selected mailing code
            public string MailingCode
            {
                get; set;
            }

            /// The selected publication codes as a delimited string
            public string PublicationCodes
            {
                get; set;
            }
        }

        /// <summary>A single instance of the options that apply for this Form Letter print run</summary>
        public TFormLetterPrintOptions FormLetterPrintOptions = null;

        /// <summary>
        /// constructor
        /// </summary>
        public TFormLetterInfo(List <String>ATagList, String AFileName, Int32 AFormalityLevel = 1)
        {
            FileName = AFileName;
            RetrievalList = new List <TFormDataRetrievalSection>();
            TagList = ATagList;
            BuildRetrievalList();
            FormalityLevel = AFormalityLevel;
            FormalityList = null;
        }

        private List <String>TagList;
        private List <TFormDataRetrievalSection>RetrievalList;
        private List <Formality>FormalityList;

        /// <summary>
        /// initialize formality structures
        /// </summary>
        /// <param name="ALanguageCode"></param>
        /// <param name="ACountryCode"></param>
        /// <param name="AAddresseeTypeCode"></param>
        /// <param name="AFormalityLevel"></param>
        /// <param name="ASalutationText"></param>
        /// <param name="AClosingText"></param>
        /// <returns></returns>
        public void AddFormality(String ALanguageCode, String ACountryCode, String AAddresseeTypeCode,
            Int32 AFormalityLevel, String ASalutationText, String AClosingText)
        {
            if (FormalityList == null)
            {
                FormalityList = new List <Formality>();
            }

            Formality NewFormality = new Formality();
            NewFormality.LanguageCode = ALanguageCode;
            NewFormality.CountryCode = ACountryCode;
            NewFormality.AddresseeTypeCode = AAddresseeTypeCode;
            NewFormality.FormalityLevel = AFormalityLevel;
            NewFormality.SalutationText = ASalutationText;
            NewFormality.ClosingText = AClosingText;

            FormalityList.Add(NewFormality);
        }

        /// <summary>
        /// check if formality list is already initialized
        /// </summary>
        /// <returns>true if formality list is initialized</returns>
        public Boolean IsFormalityInitialized()
        {
            return FormalityList != null;
        }

        /// <summary>
        /// retrieve salutation and closing text from the best formality record match, according to formality level and other criteria.
        /// This assumes that AFormDataPartner is already filled with name information needed for
        /// producing greetings.
        /// </summary>
        /// <param name="AFormDataPartner">form letter data object to be used and filled</param>
        /// <param name="ASalutationText">out: saluation text (inserts not replaced yet)</param>
        /// <param name="AClosingText">out: closing text (inserts not replaced yet)</param>
        /// <returns>true if salutation and closing text could successfully retrieved</returns>
        public Boolean RetrieveFormalityGreeting(TFormDataPartner AFormDataPartner, out String ASalutationText, out String AClosingText)
        {
            return RetrieveFormalityGreeting(AFormDataPartner.LanguageCode, AFormDataPartner.CountryCode, AFormDataPartner.AddresseeType,
                out ASalutationText, out AClosingText);
        }

        /// <summary>
        /// retrieve salutation and closing text from the best formality record match, according to formality level and other criteria.
        /// This assumes that AFormDataPartner is already filled with name information needed for
        /// producing greetings.
        /// </summary>
        /// <param name="ALanguageCode">form letter data object to be used and filled</param>
        /// <param name="ACountryCode">form letter data object to be used and filled</param>
        /// <param name="AAddresseeTypeCode">form letter data object to be used and filled</param>
        /// <param name="ASalutationText">out: saluation text (inserts not replaced yet)</param>
        /// <param name="AClosingText">out: closing text (inserts not replaced yet)</param>
        /// <returns>true if salutation and closing text could successfully retrieved</returns>
        public Boolean RetrieveFormalityGreeting(String ALanguageCode,
            String ACountryCode,
            String AAddresseeTypeCode,
            out String ASalutationText,
            out String AClosingText)
        {
            Formality resultFormality = null;

            //TODO: this general algorithm was taken over from Petra. Do we need to change how this is done?

            var queryExact = from formality in FormalityList
                             where formality.LanguageCode == ALanguageCode
                             && formality.CountryCode == ACountryCode
                             && formality.AddresseeTypeCode == AAddresseeTypeCode
                             && formality.FormalityLevel == FormalityLevel
                             select formality;

            foreach (var formality in queryExact)
            {
                resultFormality = formality;
                break;
            }

            if (resultFormality == null)
            {
                // no exact match was found --> widen criteria
                // Drop the country requirement and search for formality level upwards
                var queryNoCountryAsc = from formality in FormalityList
                                        where formality.LanguageCode == ALanguageCode
                                        && formality.AddresseeTypeCode == AAddresseeTypeCode
                                        && formality.FormalityLevel >= FormalityLevel
                                        orderby formality.FormalityLevel ascending
                                        select formality;

                // first check
                foreach (var formality in queryNoCountryAsc)
                {
                    resultFormality = formality;
                    break;
                }
            }

            if (resultFormality == null)
            {
                // Drop the country requirement and search for formality level downwards
                var queryNoCountryDsc = from formality in FormalityList
                                        where formality.LanguageCode == ALanguageCode
                                        && formality.AddresseeTypeCode == AAddresseeTypeCode
                                        && formality.FormalityLevel < FormalityLevel
                                        orderby formality.FormalityLevel descending
                                        select formality;

                // first check
                foreach (var formality in queryNoCountryDsc)
                {
                    resultFormality = formality;
                    break;
                }
            }

            if (resultFormality == null)
            {
                // Drop the country as well as language requirement and search for formality level upwards
                var queryNoCountryNoLanguageAsc = from formality in FormalityList
                                                  where formality.AddresseeTypeCode == AAddresseeTypeCode
                                                  && formality.FormalityLevel >= FormalityLevel
                                                  orderby formality.FormalityLevel ascending
                                                  select formality;

                // first check
                foreach (var formality in queryNoCountryNoLanguageAsc)
                {
                    resultFormality = formality;
                    break;
                }
            }

            if (resultFormality == null)
            {
                // Drop the country as well as language requirement and search for formality level downwards
                var queryNoCountryNoLanguageDsc = from formality in FormalityList
                                                  where formality.AddresseeTypeCode == AAddresseeTypeCode
                                                  && formality.FormalityLevel < FormalityLevel
                                                  orderby formality.FormalityLevel descending
                                                  select formality;

                // first check
                foreach (var formality in queryNoCountryNoLanguageDsc)
                {
                    resultFormality = formality;
                    break;
                }
            }

            // nothing found yet: check for default addressee type instead of given one
            if (resultFormality == null)
            {
                // check for default value with exact or higher formality level
                var queryDefaultAddresseeTypeAsc = from formality in FormalityList
                                                   where formality.AddresseeTypeCode == "DEFAULT"
                                                   && formality.FormalityLevel >= FormalityLevel
                                                   orderby formality.FormalityLevel ascending
                                                   select formality;

                // first check
                foreach (var formality in queryDefaultAddresseeTypeAsc)
                {
                    resultFormality = formality;
                    break;
                }
            }

            if (resultFormality == null)
            {
                // check for default value with lower formality level
                var queryDefaultAddresseeTypeDsc = from formality in FormalityList
                                                   where formality.AddresseeTypeCode == "DEFAULT"
                                                   && formality.FormalityLevel < FormalityLevel
                                                   orderby formality.FormalityLevel descending
                                                   select formality;

                // first check
                foreach (var formality in queryDefaultAddresseeTypeDsc)
                {
                    resultFormality = formality;
                    break;
                }
            }

            // nothing found at all: assign empty text for salutation and closing
            if (resultFormality != null)
            {
                ASalutationText = resultFormality.SalutationText;
                AClosingText = resultFormality.ClosingText;
            }
            else
            {
                ASalutationText = "";
                AClosingText = "";
            }

            return resultFormality != null;
        }

        /// <summary>
        /// File name
        /// </summary>
        public String FileName {
            get; set;
        }

        /// <summary>
        /// address layout code to be used
        /// </summary>
        public String AddressLayoutCode {
            get; set;
        }

        /// <summary>
        /// Formality Level
        /// </summary>
        public Int32 FormalityLevel {
            get; set;
        }

        #region Splitting Email addresses

        /// <summary>
        /// If this is true multiple email addresses are split and applied one by one to the same partner on multiple rows
        /// </summary>
        public bool SplitEmailAddresses
        {
            get; set;
        }

        /// <summary>
        /// Value used internally to specify a pointer into one of multiple email addresses
        /// </summary>
        public int CurrentEmailInstance
        {
            get; set;
        }

        /// <summary>
        /// Value used internally to specify the next pointer to use where there are multiple email addresses
        /// </summary>
        public int NextEmailInstance
        {
            get; set;
        }

        #endregion

        /// <summary>
        /// Add retrieval section flag if not there yet
        /// </summary>
        /// <param name="ASection"></param>
        private void AddRetrievalSection(TFormDataRetrievalSection ASection)
        {
            if (!RetrievalList.Contains(ASection))
            {
                RetrievalList.Add(ASection);
            }
        }

        /// <summary>
        /// Add retrieval section flag if tag list contains tags for given Template object
        /// </summary>
        /// <param name="AFormDataTemplate"></param>
        /// <param name="ASection"></param>
        private void BuildRetrievalSection(TFormData AFormDataTemplate, TFormDataRetrievalSection ASection)
        {
            Boolean AddSection = false;

            foreach (var prop in AFormDataTemplate.GetType().GetProperties())
            {
                if (TagList.Contains(prop.Name))
                {
                    AddSection = true;
                }
            }

            if (AddSection)
            {
                AddRetrievalSection(ASection);
            }
        }

        /// <summary>
        /// Build complete retrieval list for this TFormLetterInfo object
        /// </summary>
        private void BuildRetrievalList()
        {
            // initialize list
            RetrievalList.Clear();

            // at the moment always retrieve PPartner record
            RetrievalList.Add(TFormDataRetrievalSection.ePartner);

            TFormDataPerson FormDataPerson = new TFormDataPerson();
            BuildRetrievalSection(FormDataPerson, TFormDataRetrievalSection.ePerson);

            if (TagList.Contains("RecordingField"))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eGeneral);
            }

            if (TagList.Contains("FormalSalutation")
                || TagList.Contains("FormalClosing"))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eFormalGreetings);
            }

            if (TagList.Contains("LocationKey")
                || TagList.Contains("Address1")
                || TagList.Contains("AddressStreet2")
                || TagList.Contains("Address3")
                || TagList.Contains("PostalCode")
                || TagList.Contains("County")
                || TagList.Contains("CountryName")
                || TagList.Contains("City")
                || TagList.Contains("CountryCode")
                || TagList.Contains("CountryInLocalLanguage"))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eLocation);
            }

            if (TagList.Contains("AddressBlock"))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eLocation);
                AddRetrievalSection(TFormDataRetrievalSection.eLocationBlock);
            }

            if (TagList.Contains("PrimaryPhone")
                || TagList.Contains("PrimaryEmail")
                || TagList.Contains("Skype"))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eContact);
            }

            if (TagList.Exists(x => x.StartsWith("ContactLog.")))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eContactLog);
            }

            if (TagList.Exists(x => x.StartsWith("Subscription.")))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eSubscription);
            }

            if (TagList.Contains("BankAccountName")
                || TagList.Contains("BankAccountNumber")
                || TagList.Contains("IBANUnformatted")
                || TagList.Contains("IBANFormatted")
                || TagList.Contains("BankName")
                || TagList.Contains("BankBranchCode")
                || TagList.Contains("BICSwiftCode"))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eBanking);
            }

            if (TagList.Exists(x => x.StartsWith("Language.")))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eLanguage);
            }

            if (TagList.Exists(x => x.StartsWith("Passport")))
            {
                AddRetrievalSection(TFormDataRetrievalSection.ePassport);
            }

            if (TagList.Exists(x => x.StartsWith("GiftTotal")))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eGiftTotal);
            }

            if (TagList.Exists(x => x.StartsWith("Gift."))
                || TagList.Contains("GiftDate"))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eGiftDetail);
            }

            if (TagList.Exists(x => x.StartsWith("Skill")))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eSkill);
            }

            if (TagList.Exists(x => x.StartsWith("WorkExp")))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eWorkExperience);
            }

            if (TagList.Exists(x => x.StartsWith("FamilyMember.")))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eFamilyMember);
            }

            if (TagList.Exists(x => x.EndsWith("Needs"))
                || TagList.Contains("Vegetarian"))
            {
                AddRetrievalSection(TFormDataRetrievalSection.eSpecialNeeds);
            }

            if (TagList.Exists(x => x.Contains("Believer")))
            {
                AddRetrievalSection(TFormDataRetrievalSection.ePersonalData);
            }
        }

        /// <summary>
        /// check if retrieval of data is requested for a certain section
        /// </summary>
        /// <param name="ASection"></param>
        /// <returns></returns>
        public Boolean IsRetrievalRequested(TFormDataRetrievalSection ASection)
        {
            return RetrievalList.Contains(ASection);
        }

        /// <summary>
        /// check if tag list contains certain tag
        /// </summary>
        /// <param name="ATag"></param>
        /// <returns></returns>
        public Boolean ContainsTag(String ATag)
        {
            return TagList.Contains(ATag);
        }

        /// <summary>
        /// This constructor sets all the Form Letter print options and is set by the GUI
        /// </summary>
        /// <param name="AMailingCode"></param>
        /// <param name="APublicationCodes"></param>
        public void AddFormLetterPrintOptions(string AMailingCode, string APublicationCodes)
        {
            FormLetterPrintOptions = new TFormLetterPrintOptions();
            FormLetterPrintOptions.MailingCode = AMailingCode;
            FormLetterPrintOptions.PublicationCodes = APublicationCodes;
        }
    }

    /// <summary>
    /// Class to keep information about form letter creation process for finance letters
    /// </summary>
    [Serializable()]
    public class TFormLetterFinanceInfo : TFormLetterInfo
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TFormLetterFinanceInfo(List <String>ATagList, String AFileName, Int32 AFormalityLevel = 1)
            : base(ATagList, AFileName, AFormalityLevel)
        {
            // initialize all default values
            MinimumAmount = 0;
            AlwaysPrintNewDonor = false;

            GiftsAll = true;
            GiftsOnly = false;
            GiftsInKindOnly = false;
            GiftsOther = false;

            AllAdjustments = true;
            IncludeAdjustmentsOnly = false;
            ExcludeAdjustments = false;
        }

        /// <summary>
        /// is there a minimum amount for printing
        /// </summary>
        public Decimal MinimumAmount {
            get; set;
        }

        /// <summary>
        /// always print if this is a new donor
        /// </summary>
        public Boolean AlwaysPrintNewDonor {
            get; set;
        }

        /// <summary>
        /// print any gift
        /// </summary>
        public Boolean GiftsAll {
            get; set;
        }

        /// <summary>
        /// only print if normal gift
        /// </summary>
        public Boolean GiftsOnly {
            get; set;
        }

        /// <summary>
        /// only print if gift in kind
        /// </summary>
        public Boolean GiftsInKindOnly {
            get; set;
        }

        /// <summary>
        /// only print if neither gift nor gift in kind
        /// </summary>
        public Boolean GiftsOther {
            get; set;
        }

        /// <summary>
        /// print no matter if adjustment or not
        /// </summary>
        public Boolean AllAdjustments {
            get; set;
        }

        /// <summary>
        ///  only print adjustments
        /// </summary>
        public Boolean IncludeAdjustmentsOnly {
            get; set;
        }

        /// <summary>
        /// only print if not adjustment
        /// </summary>
        public Boolean ExcludeAdjustments {
            get; set;
        }

        /// <summary>
        /// set properties through interpretation of option string stored in database
        /// </summary>
        /// <param name="AOptions"></param>
        public void SetOptionsFromFinanceForm(String AOptions)
        {
            String OptionList = AOptions;
            String OptionGift;
            String OptionAdjustment;
            String OptionNewDonor;

            // interpret csv delimited string

            // first option: gift printing
            OptionGift = StringHelper.GetNextCSV(ref OptionList, ",");
            GiftsAll = false;
            GiftsOnly = false;
            GiftsInKindOnly = false;
            GiftsOther = false;

            switch (OptionGift)
            {
                case MCommonConstants.FORM_OPTION_ALL:
                    GiftsAll = true;
                    break;

                case MCommonConstants.FORM_OPTION_GIFTS_ONLY:
                    GiftsOnly = true;
                    break;

                case MCommonConstants.FORM_OPTION_GIFT_IN_KIND_ONLY:
                    GiftsInKindOnly = true;
                    break;

                case MCommonConstants.FORM_OPTION_OTHER:
                    GiftsOther = true;
                    break;

                default:
                    GiftsAll = true;
                    break;
            }

            // second option: adjustment printing
            OptionAdjustment = StringHelper.GetNextCSV(ref OptionList, ",");
            AllAdjustments = false;
            IncludeAdjustmentsOnly = false;
            ExcludeAdjustments = false;

            switch (OptionAdjustment)
            {
                case MCommonConstants.FORM_OPTION_ALL:
                    AllAdjustments = true;
                    break;

                case MCommonConstants.FORM_OPTION_ADJUSTMENTS_ONLY:
                    IncludeAdjustmentsOnly = true;
                    break;

                case MCommonConstants.FORM_OPTION_EXCLUDE_ADJUSTMENTS:
                    ExcludeAdjustments = true;
                    break;

                default:
                    AllAdjustments = true;
                    break;
            }

            // third option: always print new donors
            OptionNewDonor = StringHelper.GetNextCSV(ref OptionList, ",");

            if (OptionNewDonor == "yes")
            {
                AlwaysPrintNewDonor = true;
            }
            else
            {
                AlwaysPrintNewDonor = false;
            }
        }
    }

    /// <summary>
    /// List Class for partner form data objects
    /// </summary>
    [Serializable()]
    public class TFormDataPartnerList
    {
        /// <summary>
        /// constructor (for simple list of TFormDataPartner objects)
        /// </summary>
        public TFormDataPartnerList()
        {
            Partner = new List <TFormDataPartner>();
            FUsedForLabelPrinting = false;
            FColumnIndex = 1;

            P = Partner;
        }

        /// <summary>
        /// constructor (for label printing option)
        /// </summary>
        public TFormDataPartnerList(Boolean AUsedForLabelPrinting, int ANumberOfLabelColumns)
        {
            FNumberOfLabelColumns = ANumberOfLabelColumns;
            FUsedForLabelPrinting = true;
            FColumnIndex = 1;

            if (ANumberOfLabelColumns > 0)
            {
                C1 = new List <TFormDataPartner>();
            }

            if (ANumberOfLabelColumns > 1)
            {
                C2 = new List <TFormDataPartner>();
            }

            if (ANumberOfLabelColumns > 2)
            {
                C3 = new List <TFormDataPartner>();
            }

            if (ANumberOfLabelColumns > 3)
            {
                C4 = new List <TFormDataPartner>();
            }

            if (ANumberOfLabelColumns > 4)
            {
                C5 = new List <TFormDataPartner>();
            }

            if (ANumberOfLabelColumns > 5)
            {
                C6 = new List <TFormDataPartner>();
            }

            if (ANumberOfLabelColumns > 6)
            {
                C7 = new List <TFormDataPartner>();
            }

            if (ANumberOfLabelColumns > 7)
            {
                C8 = new List <TFormDataPartner>();
            }
        }

        /// list of partner records (no columns)
        public List <TFormDataPartner>Partner;
        /// short accessor for Partner (points to the same data)
        public List <TFormDataPartner>P;

        /// list of partner records (make provision for up to 8 columns)
        /// list of partner records for column 1
        public List <TFormDataPartner>C1;
        /// list of partner records for column 2 (optional)
        public List <TFormDataPartner>C2;
        /// list of partner records for column 3 (optional)
        public List <TFormDataPartner>C3;
        /// list of partner records for column 4 (optional)
        public List <TFormDataPartner>C4;
        /// list of partner records for column 5 (optional)
        public List <TFormDataPartner>C5;
        /// list of partner records for column 6 (optional)
        public List <TFormDataPartner>C6;
        /// list of partner records for column 7 (optional)
        public List <TFormDataPartner>C7;
        /// list of partner records for column 8 (optional)
        public List <TFormDataPartner>C8;

        private Boolean FUsedForLabelPrinting;
        private int FNumberOfLabelColumns;
        private int FColumnIndex;

        /// <summary>
        ///  add form data record to list
        /// </summary>
        public void AddFormData(TFormDataPartner ARecord)
        {
            if (FUsedForLabelPrinting)
            {
                switch (FColumnIndex)
                {
                    case 1:
                        C1.Add(ARecord);
                        break;

                    case 2:
                        C2.Add(ARecord);
                        break;

                    case 3:
                        C3.Add(ARecord);
                        break;

                    case 4:
                        C4.Add(ARecord);
                        break;

                    case 5:
                        C5.Add(ARecord);
                        break;

                    case 6:
                        C6.Add(ARecord);
                        break;

                    case 7:
                        C7.Add(ARecord);
                        break;

                    case 8:
                        C8.Add(ARecord);
                        break;

                    default:
                        break;
                }

                FColumnIndex++;

                if (FColumnIndex > FNumberOfLabelColumns)
                {
                    FColumnIndex = 1;
                }
            }
            else
            {
                Partner.Add(ARecord);
            }
        }
    }

    /// <summary>
    /// Base Class for mail merge form data
    /// </summary>
    [Serializable()]
    public class TFormData
    {
        //TODOWB
        //private static class ReflectionUtility
        //{
        //public static string GetPropertyName<T>(Expression<Func<T>> expression)
        //{
        //    MemberExpression body = (MemberExpression)expression.Body;
        //    return body.Member.Name;
        //}

        //private static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
        //{
        //    MemberExpression memberExpression = (MemberExpression)property.Body;
        //
        //    return memberExpression.Member.Name;
        //}
        //}
        //string name = ReflectionUtility.GetPropertyName(() => formData.BankName);

        /// Current Date
        public DateTime ? CurrentDate {
            get; set;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="AFieldList"></param>
        /// <param name="ASection"></param>
        /// <returns></returns>
        public static Boolean RetrieveInfo(List <String>AFieldList, TFormDataRetrievalSection ASection)
        {
            //string name = ReflectionUtility.GetPropertyName(() => formData.BankName);

            return false;
        }
    }

    /// <summary>
    /// Contains data for a Partner to be used for export to forms
    /// </summary>
    [Serializable()]
    public class TFormDataPartner : TFormData
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TFormDataPartner()
        {
            ContactLog = new List <TFormDataContactLog>();
            Subscription = new List <TFormDataSubscription>();
            Gift = new List <TFormDataGift>();
            FamilyMember = new List <TFormDataFamilyMember>();
            Custom1 = Custom2 = Custom3 = Custom4 = Custom5 = Custom6 = Custom7 = Custom8 = String.Empty;
            AddressIsOriginal = true;
        }

        // General Info

        /// field recording data for this partner record (site key)
        public string RecordingField {
            get; set;
        }


        // AddressElement

        ///
        public Boolean IsPersonRecord {
            get; set;
        }
        ///
        public string PartnerKey {
            get; set;
        }

        // what about #DONORKEY??? (is actually partner key but not everybody is donor)

        /// assembled formal salutation (rules taken from p_formality)
        public String FormalSalutation
        {
            get; set;
        }

        /// assembled formal closing (rules taken from p_formality)
        public String FormalClosing {
            get; set;
        }

        ///
        public String Title {
            get; set;
        }
        ///
        public String TitleAndSpace {
            get; set;
        }
        ///
        public String AddresseeType {
            get; set;
        }
        ///
        public String FirstName {
            get; set;
        }
        ///
        public String FirstNameAndSpace {
            get; set;
        }
        ///
        public String FirstInitial {
            get; set;
        }
        ///
        public String FirstInitialAndSpace {
            get; set;
        }
        ///
        public String LastName {
            get; set;
        }
        ///
        public String LastNameAndSpace {
            get; set;
        }
        ///
        public String Name {
            get; set;
        }
        ///
        public String ShortName {
            get; set;
        }
        ///
        public String LocalName {
            get; set;
        }
        ///
        public Int32 LocationKey {
            get; set;
        }
        /// set to true if address was retrieved from original LocationKey, otherwise false if updated (for example to BestAddress)
        public bool AddressIsOriginal {
            get; set;
        }
        ///
        public String Address1 {
            get; set;
        }
        ///
        public String AddressStreet2 {
            get; set;
        }
        ///
        public String Address3 {
            get; set;
        }
        ///
        public String PostalCode {
            get; set;
        }
        ///
        public String County {
            get; set;
        }
        ///
        public String CountryName {
            get; set;
        }
        ///
        public String City {
            get; set;
        }
        ///
        public String CountryCode {
            get; set;
        }
        ///
        public String CountryInLocalLanguage {
            get; set;
        }
        /// assembled address block
        public String AddressBlock {
            get; set;
        }

        // Enclosures related
        ///
        public string PublicationCodes {
            get; set;
        }
        ///
        public string MailingCode {
            get; set;
        }
        ///
        public string Enclosures {
            get; set;
        }

        // Communication
        ///
        public String PrimaryPhone {
            get; set;
        }
        ///
        public String PrimaryEmail {
            get; set;
        }
        ///
        public String Skype {
            get; set;
        }

        // Person
        ///
        public DateTime ? DateOfBirth {
            get; set;
        }
        ///
        public String LanguageCode {
            get; set;
        }
        ///
        public String Notes {
            get; set;
        }

        // Finance related
        ///
        public String BankAccountName {
            get; set;
        }
        ///
        public String BankAccountNumber {
            get; set;
        }
        ///
        public String IBANUnformatted {
            get; set;
        }
        ///
        public String IBANFormatted {
            get; set;
        }
        ///
        public String BankName {
            get; set;
        }
        ///
        public String BankBranchCode {
            get; set;
        }
        ///
        public String BICSwiftCode {
            get; set;
        }
        ///
        public String ReceiptLetterFrequency {
            get; set;
        }

        // Gift related
        ///
        public String GiftTotalAmountCurrency {
            get; set;
        }
        ///
        public String GiftTotalAmount {
            get; set;
        }
        ///
        public String GiftTotalAmountInWords {
            get; set;
        }
        ///
        public String GiftTotalTaxDeductAmount {
            get; set;
        }
        ///
        public String GiftTotalTaxDeductAmountInWords {
            get; set;
        }
        ///
        public String GiftTotalTaxNonDeductAmount {
            get; set;
        }
        ///
        public String GiftTotalTaxNonDeductAmountInWords {
            get; set;
        }
        ///
        public DateTime ? GiftDate {
            get; set;
        }

        // Custom fields
        ///
        public String Custom1 {
            get; set;
        }
        ///
        public String Custom2 {
            get; set;
        }
        ///
        public String Custom3 {
            get; set;
        }
        ///
        public String Custom4 {
            get; set;
        }
        ///
        public String Custom5 {
            get; set;
        }
        ///
        public String Custom6 {
            get; set;
        }
        ///
        public String Custom7 {
            get; set;
        }
        ///
        public String Custom8 {
            get; set;
        }

        /// list of contact logs
        public List <TFormDataContactLog>ContactLog;

        /// list of subscriptions
        public List <TFormDataSubscription>Subscription;

        /// list of gifts
        public List <TFormDataGift>Gift;

        /// list of family members
        /// for Family: all members
        /// for Person: all members except this Person
        public List <TFormDataFamilyMember>FamilyMember;

        /// <summary>
        ///  add contact log record to list
        /// </summary>
        public void AddContactLog(TFormDataContactLog ARecord)
        {
            ContactLog.Add(ARecord);
        }

        /// <summary>
        ///  add subscription record to list
        /// </summary>
        public void AddSubscription(TFormDataSubscription ARecord)
        {
            Subscription.Add(ARecord);
        }

        /// <summary>
        ///  add gift record to list
        /// </summary>
        public void AddGift(TFormDataGift ARecord)
        {
            Gift.Add(ARecord);
        }

        /// <summary>
        ///  add family member record to list
        /// </summary>
        public void AddFamilyMember(TFormDataFamilyMember ARecord)
        {
            FamilyMember.Add(ARecord);
        }
    }

    /// <summary>
    /// Contains data for a Person to be used for export to forms
    /// </summary>
    [Serializable()]
    public class TFormDataPerson : TFormDataPartner
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TFormDataPerson()
        {
            Passport = new List <TFormDataPassport>();
            Language = new List <TFormDataLanguage>();
            Skill = new List <TFormDataSkill>();
            SkillP = new List <TFormDataSkill>();
            SkillNP = new List <TFormDataSkill>();
            WorkExp = new List <TFormDataWorkExperience>();
            WorkExpNonProfit = new List <TFormDataWorkExperience>();
            WorkExpOther = new List <TFormDataWorkExperience>();
        }

        ///
        public String Decorations {
            get; set;
        }
        ///
        public String MiddleName {
            get; set;
        }
        ///
        public String PreferedName {
            get; set;
        }
        ///
        public String AcademicTitle {
            get; set;
        }
        ///
        public String Gender {
            get; set;
        }
        ///
        public String MaritalStatus {
            get; set;
        }
        ///
        public String MaritalStatusDesc {
            get; set;
        }
        ///
        public String OccupationCode {
            get; set;
        }
        ///
        public String Occupation {
            get; set;
        }

        /// Nationality taken from main passport
        public String PassportNationality {
            get; set;
        }

        /// Nationality code taken from main passport
        public String PassportNationalityCode {
            get; set;
        }

        /// Number taken from main passport
        public String PassportNumber {
            get; set;
        }

        /// Name taken from main passport
        public String PassportName {
            get; set;
        }

        ///
        public DateTime ? PassportDateOfIssue {
            get; set;
        }

        /// Place of issue taken from main passport
        public String PassportPlaceOfIssue {
            get; set;
        }

        /// Date of expiry taken from main passport
        public DateTime ? PassportDateOfExpiry {
            get; set;
        }

        /// Place of birth taken from main passport
        public String PassportPlaceOfBirth {
            get; set;
        }

        /// Name of sending church
        public String SendingChurchName {
            get; set;
        }

        /// How many years a believer?
        public String YearsBeliever {
            get; set;
        }

        /// Comment believer
        public String CommentBeliever {
            get; set;
        }

        /// Medical needs
        public String MedicalNeeds {
            get; set;
        }

        /// Dietary needs
        public String DietaryNeeds {
            get; set;
        }

        /// Other needs
        public String OtherNeeds {
            get; set;
        }

        /// Vegetarian
        public Boolean Vegetarian {
            get; set;
        }

        /// list of passports
        public List <TFormDataPassport>Passport;

        /// list of languages
        public List <TFormDataLanguage>Language;

        /// list of skills
        public List <TFormDataSkill>Skill;

        /// list of professional skills
        public List <TFormDataSkill>SkillP;

        /// list of non professional skills
        public List <TFormDataSkill>SkillNP;

        /// list of work experience
        public List <TFormDataWorkExperience>WorkExp;

        /// list of work experience for non-profits
        public List <TFormDataWorkExperience>WorkExpNonProfit;

        /// list of work experience (other than non-profit)
        public List <TFormDataWorkExperience>WorkExpOther;

        /// <summary>
        ///  add passport record to list
        /// </summary>
        public void AddPassport(TFormDataPassport ARecord)
        {
            Passport.Add(ARecord);
        }

        /// <summary>
        ///  add language record to list
        /// </summary>
        public void AddLanguage(TFormDataLanguage ARecord)
        {
            Language.Add(ARecord);
        }

        /// <summary>
        ///  add skill record to list
        /// </summary>
        public void AddSkill(TFormDataSkill ARecord)
        {
            Skill.Add(ARecord);

            if (ARecord.Professional)
            {
                SkillP.Add(ARecord);
            }
            else
            {
                SkillNP.Add(ARecord);
            }
        }

        /// <summary>
        ///  add work experience record to list
        /// </summary>
        public void AddWorkExperience(TFormDataWorkExperience ARecord)
        {
            WorkExp.Add(ARecord);

            if (ARecord.SimilarOrg
                || ARecord.SameOrg)
            {
                WorkExpNonProfit.Add(ARecord);
            }
            else
            {
                WorkExpOther.Add(ARecord);
            }
        }
    }

    /// <summary>
    /// Contains data for a Unit to be used for export to forms
    /// </summary>
    [Serializable()]
    public class TFormDataUnit : TFormDataPartner
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TFormDataUnit()
        {
        }
    }

    /// <summary>
    /// Contains data for a gift detail for/from a partner
    /// </summary>
    [Serializable()]
    public class TFormDataGift : TFormData
    {
        /// Date of donation
        public DateTime ? Date {
            get; set;
        }

        /// Gift Reference
        public String Reference {
            get; set;
        }

        /// Is Gift in Kind?
        public Boolean IsTypeGiftInKind {
            get; set;
        }

        /// Is Gift in Kind?
        public Boolean IsTypeGift {
            get; set;
        }

        /// Currency
        public String AmountCurrency {
            get; set;
        }

        /// Amount in words
        public String AmountInWords {
            get; set;
        }

        /// Amount
        public String Amount {
            get; set;
        }

        /// Tax deducted amount in words
        public String TaxDeductAmountInWords {
            get; set;
        }

        /// Tax deducted amount
        public String TaxDeductAmount {
            get; set;
        }

        /// Tax non deducted amount in words
        public String TaxNonDeductAmountInWords {
            get; set;
        }

        /// Tax non deducted amount
        public String TaxNonDeductAmount {
            get; set;
        }

        /// Comment one
        public String CommentOne {
            get; set;
        }

        /// Comment two
        public String CommentTwo {
            get; set;
        }

        /// Comment three
        public String CommentThree {
            get; set;
        }

        /// Account description
        public String AccountDesc {
            get; set;
        }

        /// Cost centre description
        public String CostCentreDesc {
            get; set;
        }

        /// Field name
        public String FieldName {
            get; set;
        }

        /// Recipient name
        public String RecipientName {
            get; set;
        }

        /// Recipient name (or motivation if recipient key is 0)
        public String RecipientNameOrMotivation {
            get; set;
        }

        /// Mailing Code
        public String MailingCode {
            get; set;
        }
    }

    /// <summary>
    /// Contains data for a contact log entry for a partner
    /// </summary>
    [Serializable()]
    public class TFormDataContactLog : TFormData
    {
        /// Contactor
        public String Contactor {
            get; set;
        }

        /// Notes / Comment
        public String Notes {
            get; set;
        }
    }

    /// <summary>
    /// Contains data for a subscription for a partner
    /// </summary>
    [Serializable()]
    public class TFormDataSubscription : TFormData
    {
        /// Publication Code
        public String PublicationCode {
            get; set;
        }

        /// Status
        public String Status {
            get; set;
        }

        /// Number of copies to send
        public int PublicationCopies {
            get; set;
        }
    }

    /// <summary>
    /// Contains data for a passport record for a person
    /// </summary>
    [Serializable()]
    public class TFormDataPassport : TFormData
    {
        /// Is Main Passport?
        public Boolean IsMainPassport {
            get; set;
        }

        /// Passport Number
        public String Number {
            get; set;
        }

        /// Passport Name
        public String Name {
            get; set;
        }

        ///  Type of passport (code)
        public String TypeCode {
            get; set;
        }

        ///  Type of passport (description)
        public String TypeDescription {
            get; set;
        }

        /// Passport Nationality Code
        public String NationalityCode {
            get; set;
        }

        /// Passport Nationality Name
        public String NationalityName {
            get; set;
        }

        ///  Name as in Passport
        public String PassportName {
            get; set;
        }

        ///
        public DateTime ? DateOfIssue {
            get; set;
        }

        ///  Place of issue
        public String PlaceOfIssue {
            get; set;
        }

        ///
        public DateTime ? DateOfExpiry {
            get; set;
        }

        ///  Place of birth
        public String PlaceOfBirth {
            get; set;
        }
    }

    /// <summary>
    /// Contains data for a language record for a person
    /// </summary>
    [Serializable()]
    public class TFormDataLanguage : TFormData
    {
        ///  Language Code
        public String Code {
            get; set;
        }

        /// Language Name
        public String Name {
            get; set;
        }

        ///  Language Level
        public String Level {
            get; set;
        }

        ///  Language Level Description
        public String LevelDesc {
            get; set;
        }

        ///  Language Comment
        public String Comment {
            get; set;
        }
    }

    /// <summary>
    /// Contains data for a work experience record for a person
    /// </summary>
    [Serializable()]
    public class TFormDataWorkExperience : TFormData
    {
        ///  Start date
        public DateTime ? StartDate {
            get; set;
        }

        ///  End date
        public DateTime ? EndDate {
            get; set;
        }

        /// Location
        public String Location {
            get; set;
        }

        /// Organisation
        public String Organisation {
            get; set;
        }

        /// Role
        public String Role {
            get; set;
        }

        /// Category
        public String Category {
            get; set;
        }

        ///  Worked for our organisation
        public Boolean SameOrg {
            get; set;
        }

        ///  Worked for similar kind of organisation
        public Boolean SimilarOrg {
            get; set;
        }

        ///  Comment
        public String Comment {
            get; set;
        }
    }

    /// <summary>
    /// Contains data for a skill record for a person
    /// </summary>
    [Serializable()]
    public class TFormDataSkill : TFormData
    {
        ///  Skill category
        public String Category {
            get; set;
        }

        /// Description
        public String Description {
            get; set;
        }

        /// Description in local language (if not populate with Description)
        public String DescriptionLocalOrDefault {
            get; set;
        }

        ///  Skill level
        public Int32 Level {
            get; set;
        }

        ///  Skill level description
        public String LevelDesc {
            get; set;
        }

        ///  Years of experience
        public Int32 YearsExp {
            get; set;
        }

        ///  Is this a Professional Skill?
        public Boolean Professional {
            get; set;
        }

        ///  Degree
        public String Degree {
            get; set;
        }

        ///  Comment
        public String Comment {
            get; set;
        }
    }

    /// <summary>
    /// Contains data for a family member
    /// </summary>
    [Serializable()]
    public class TFormDataFamilyMember : TFormData
    {
        ///  Skill category
        public String Name {
            get; set;
        }

        /// Description
        public DateTime ? DateOfBirth {
            get; set;
        }
    }

    /// <summary>
    /// Contains metadata and a list of key/description pairs of data
    /// (used e.g. for printing setup screen keys and descriptions)
    /// </summary>
    [Serializable()]
    public class TFormDataKeyDescriptionList : TFormData
    {
        /// Title for list
        public String Title {
            get; set;
        }

        /// Subtitle for list
        public String SubTitle {
            get; set;
        }

        /// Name of person printing
        public String PrintedBy
        {
            get; set;
        }

        /// Date and time of printing
        public String Date
        {
            get; set;
        }

        /// Full path to the file that is being printed
        public String Filename
        {
            get; set;
        }

        /// Title for Key column
        public String KeyTitle {
            get; set;
        }

        /// Title for Description columnn
        public String DescriptionTitle {
            get; set;
        }

        /// Title for Field3 column
        public String Field3Title {
            get; set;
        }

        /// Title for Field4 column
        public String Field4Title {
            get; set;
        }

        /// Title for Field5 column
        public String Field5Title {
            get; set;
        }

        /// list of key/description pairs
        public List <TFormDataKeyDescription>list;

        /// <summary>
        /// constructor
        /// </summary>
        public TFormDataKeyDescriptionList()
        {
            list = new List <TFormDataKeyDescription>();
        }

        /// <summary>
        ///  add key/description record to list
        /// </summary>
        public void Add(TFormDataKeyDescription ARecord)
        {
            list.Add(ARecord);
        }
    }

    /// <summary>
    /// Contains data for key/description pairs of data
    /// (used e.g. for printing setup screen keys and descriptions)
    /// (includes extra data fields)
    /// </summary>
    [Serializable()]
    public class TFormDataKeyDescription : TFormData
    {
        /// Key
        public String Key {
            get; set;
        }

        /// Description
        public String Description {
            get; set;
        }

        /// Extra Field 3
        public String Field3 {
            get; set;
        }

        /// Extra Field 4
        public String Field4 {
            get; set;
        }

        /// Extra Field 5
        public String Field5 {
            get; set;
        }
    }
}