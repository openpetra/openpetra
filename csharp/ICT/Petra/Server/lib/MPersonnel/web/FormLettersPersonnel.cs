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
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;
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
using Ict.Petra.Server.MPartner.Partner.WebConnectors;
using Ict.Common.Remoting.Server;

namespace Ict.Petra.Server.MPersonnel.Person.DataElements.WebConnectors
{
    /// <summary>
    /// methods related to form letters for Personnel Module
    /// </summary>
    public class TFormLettersPersonnelWebConnector
    {
        /// <summary>
        /// populate form data for given extract and list of fields
        /// </summary>
        /// <param name="AExtractId">Extract of partners to be used</param>
        /// <param name="AFormLetterInfo">Info about form letter (tag list etc.)</param>
        /// <param name="AFormDataList">list with populated form data</param>
        /// <returns>returns true if list was created successfully</returns>
        [RequireModulePermission("AND(PERSONNEL,PTNRUSER)")]
        public static Boolean FillFormDataFromExtract(Int32 AExtractId, TFormLetterInfo AFormLetterInfo,
            out List <TFormData>AFormDataList)
        {
            Boolean ReturnValue = true;

            List <TFormData>dataList = new List <TFormData>();
            MExtractTable ExtractTable;
            Int32 RowCounter = 0;

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(), Catalog.GetString("Create Personnel Form Letter"));

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
                        TFormDataPerson FormDataPerson = new TFormDataPerson();
                        FillFormDataFromPersonnel(ExtractRow.PartnerKey, FormDataPerson, AFormLetterInfo, ExtractRow.SiteKey, ExtractRow.LocationKey);
                        dataList.Add(FormDataPerson);

                        if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob)
                        {
                            dataList.Clear();
                            ReturnValue = false;
                            TLogging.Log("Retrieve Personnel Form Letter Data - Job cancelled");
                            break;
                        }

                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Retrieving Personnel Data"),
                            (RowCounter * 100) / ExtractTable.Rows.Count);
                    }
                });

            TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

            AFormDataList = new List <TFormData>();
            AFormDataList = dataList;
            return ReturnValue;
        }

        /// <summary>
        /// populate form data for given partner key and list of fields
        /// This Personnel Data and will also make a call to fill Partner Data.
        /// </summary>
        /// <param name="APartnerKey">Key of partner record to be used</param>
        /// <param name="AFormDataPerson">Form Data Object</param>
        /// <param name="AFormLetterInfo">Info class for form letter</param>
        /// <param name="ASiteKey">Site key for location record</param>
        /// <param name="ALocationKey">Key for location record</param>
        /// <returns>returns list with populated form data</returns>
        [RequireModulePermission("AND(PERSONNEL,PTNRUSER)")]
        public static TFormDataPerson FillFormDataFromPersonnel(Int64 APartnerKey,
            TFormDataPerson AFormDataPerson,
            TFormLetterInfo AFormLetterInfo,
            Int64 ASiteKey = 0,
            Int32 ALocationKey = 0)
        {
            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    FillFormDataFromPersonnel(APartnerKey, AFormDataPerson, AFormLetterInfo, ReadTransaction, ASiteKey, ALocationKey);
                });

            return AFormDataPerson;
        }

        /// <summary>
        /// populate form data for given partner key and list of fields
        /// This Personnel Data and will also make a call to fill Partner Data.
        /// </summary>
        /// <param name="APartnerKey">Key of partner record to be used</param>
        /// <param name="AFormDataPerson">Form Data Object</param>
        /// <param name="AFormLetterInfo">Info class for form letter</param>
        /// <param name="AReadTransaction">Transaction</param>
        /// <param name="ASiteKey">Site key for location record</param>
        /// <param name="ALocationKey">Key for location record</param>
        /// <returns>returns list with populated form data</returns>
        [RequireModulePermission("AND(PERSONNEL,PTNRUSER)")]
        public static void FillFormDataFromPersonnel(Int64 APartnerKey,
            TFormDataPerson AFormDataPerson,
            TFormLetterInfo AFormLetterInfo,
            TDBTransaction AReadTransaction,
            Int64 ASiteKey = 0,
            Int32 ALocationKey = 0)
        {
            TPartnerClass PartnerClass;
            String ShortName;
            TStdPartnerStatusCode PartnerStatusCode;

            if (AFormDataPerson == null)
            {
                return;
            }

            if (MCommonMain.RetrievePartnerShortName(APartnerKey, out ShortName, out PartnerClass, out PartnerStatusCode, AReadTransaction))
            {
                // first retrieve all partner information
                TFormLettersWebConnector.FillFormDataFromPerson(APartnerKey,
                    AFormDataPerson,
                    AFormLetterInfo,
                    AReadTransaction,
                    ASiteKey,
                    ALocationKey);

                // retrieve Special Needs information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eSpecialNeeds))
                {
                    PmSpecialNeedTable SpecialNeedTable;
                    PmSpecialNeedRow SpecialNeedRow;
                    SpecialNeedTable = PmSpecialNeedAccess.LoadViaPPerson(APartnerKey, AReadTransaction);

                    if (SpecialNeedTable.Count > 0)
                    {
                        SpecialNeedRow = (PmSpecialNeedRow)SpecialNeedTable.Rows[0];
                        AFormDataPerson.DietaryNeeds = SpecialNeedRow.DietaryComment;
                        AFormDataPerson.MedicalNeeds = SpecialNeedRow.MedicalComment;
                        AFormDataPerson.OtherNeeds = SpecialNeedRow.OtherSpecialNeed;
                        AFormDataPerson.Vegetarian = SpecialNeedRow.VegetarianFlag;
                    }
                }

                // retrieve Personal Data information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.ePersonalData))
                {
                    PmPersonalDataTable PersonalDataTable;
                    PmPersonalDataRow PersonalDataRow;
                    PersonalDataTable = PmPersonalDataAccess.LoadViaPPerson(APartnerKey, AReadTransaction);

                    if (PersonalDataTable.Count > 0)
                    {
                        PersonalDataRow = (PmPersonalDataRow)PersonalDataTable.Rows[0];

                        if (!PersonalDataRow.IsBelieverSinceYearNull()
                            && (PersonalDataRow.BelieverSinceYear != 0))
                        {
                            AFormDataPerson.YearsBeliever = (DateTime.Today.Year - PersonalDataRow.BelieverSinceYear).ToString();
                        }

                        AFormDataPerson.CommentBeliever = PersonalDataRow.BelieverSinceComment;
                    }
                }

                // retrieve Passport information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.ePassport))
                {
                    PmPassportDetailsTable PassportTable;
                    TFormDataPassport PassportRecord;
                    PassportTable = PmPassportDetailsAccess.LoadViaPPerson(APartnerKey, AReadTransaction);
                    Boolean MainPassportFound = false;

                    foreach (PmPassportDetailsRow PassportRow in PassportTable.Rows)
                    {
                        // only list "full" passports that have not expired yet
                        if ((PassportRow.IsDateOfExpirationNull()
                             || (PassportRow.DateOfExpiration >= DateTime.Today))
                            && (PassportRow.PassportDetailsType == "P"))
                        {
                            PassportRecord = new TFormDataPassport();

                            PassportRecord.IsMainPassport = PassportRow.MainPassport;
                            PassportRecord.Number = PassportRow.PassportNumber;
                            PassportRecord.PassportName = PassportRow.FullPassportName;
                            PassportRecord.NationalityCode = PassportRow.PassportNationalityCode;

                            // retrieve country name from country table
                            TCacheable CachePopulator = new TCacheable();
                            PCountryTable CountryTable =
                                (PCountryTable)CachePopulator.GetCacheableTable(TCacheableCommonTablesEnum.CountryList);
                            PCountryRow CountryRow =
                                (PCountryRow)CountryTable.Rows.Find(new object[] { PassportRow.PassportNationalityCode });

                            if (CountryRow != null)
                            {
                                PassportRecord.NationalityName = CountryRow.CountryName;
                            }

                            PassportRecord.TypeCode = PassportRow.PassportDetailsType;
                            // retrieve passport type name from type table
                            TPersonnelCacheable PersonnelCachePopulator = new TPersonnelCacheable();
                            PtPassportTypeTable TypeTable =
                                (PtPassportTypeTable)PersonnelCachePopulator.GetCacheableTable(TCacheablePersonTablesEnum.
                                    PassportTypeList);
                            PtPassportTypeRow TypeRow =
                                (PtPassportTypeRow)TypeTable.Rows.Find(new object[] { PassportRow.PassportDetailsType });

                            if (TypeRow != null)
                            {
                                PassportRecord.TypeDescription = TypeRow.Description;
                            }

                            PassportRecord.DateOfIssue = PassportRow.DateOfIssue;
                            PassportRecord.PlaceOfIssue = PassportRow.PlaceOfIssue;
                            PassportRecord.DateOfExpiry = PassportRow.DateOfExpiration;
                            PassportRecord.PlaceOfBirth = PassportRow.PlaceOfBirth;

                            // set number and nationality in main record (only for main passport or if there is just one)
                            if (PassportRow.MainPassport || (PassportTable.Count == 1))
                            {
                                AFormDataPerson.PassportNumber = PassportRecord.Number;
                                AFormDataPerson.PassportNationality = PassportRecord.NationalityName;
                                AFormDataPerson.PassportNationalityCode = PassportRecord.NationalityCode;
                                AFormDataPerson.PassportName = PassportRecord.PassportName;
                                AFormDataPerson.PassportDateOfIssue = PassportRecord.DateOfIssue;
                                AFormDataPerson.PassportPlaceOfIssue = PassportRecord.PlaceOfIssue;
                                AFormDataPerson.PassportDateOfExpiry = PassportRecord.DateOfExpiry;
                                AFormDataPerson.PassportPlaceOfBirth = PassportRecord.PlaceOfBirth;

                                MainPassportFound = true;
                            }

                            // If the PassportName has not been set yet then make sure it is set on Person level from at least one passport
                            // (this will not be necessary any longer once the tick box for "Main Passport" is implemented)
                            if (!MainPassportFound)
                            {
                                AFormDataPerson.PassportName = PassportRecord.PassportName;
                            }

                            AFormDataPerson.AddPassport(PassportRecord);
                        }
                    }
                }

                // retrieve Language information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eLanguage))
                {
                    PmPersonLanguageTable PersonLanguageTable;
                    TFormDataLanguage PersonLanguageRecord;
                    PersonLanguageTable = PmPersonLanguageAccess.LoadViaPPerson(APartnerKey, AReadTransaction);

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

                        AFormDataPerson.AddLanguage(PersonLanguageRecord);
                    }
                }

                // retrieve Skill information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eSkill))
                {
                    PmPersonSkillTable PersonSkillTable;
                    TFormDataSkill PersonSkillRecord;
                    TFormDataDegree PersonDegreeRecord;
                    PersonSkillTable = PmPersonSkillAccess.LoadViaPPerson(APartnerKey, AReadTransaction);

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

                        AFormDataPerson.AddSkill(PersonSkillRecord);

                        // now add a degree record if a degree is mentioned
                        if (!PersonSkillRow.IsDegreeNull()
                            && (PersonSkillRow.Degree.Length > 0))
                        {
                            PersonDegreeRecord = new TFormDataDegree();
                            PersonDegreeRecord.Name = PersonSkillRow.Degree;
                            PersonDegreeRecord.Year = PersonSkillRow.YearOfDegree.ToString();

                            AFormDataPerson.AddDegree(PersonDegreeRecord);
                        }
                    }
                }

                // retrieve past work experience information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eWorkExperience))
                {
                    TFormDataWorkExperience PersonExpRecord;

                    /*
                     * currently we don't include application records in the work experience data
                     *
                     * String UnitShortName;
                     * TPartnerClass UnitClass;
                     *
                     * // retrieve applications for short term events
                     * String SqlStmt = TDataBase.ReadSqlFile("Personnel.FormLetters.GetAppTravelDates.sql");
                     *
                     * OdbcParameter[] parameters = new OdbcParameter[1];
                     * parameters[0] = new OdbcParameter("PartnerKey", OdbcType.BigInt);
                     * parameters[0].Value = APartnerKey;
                     *
                     * DataTable travelData = DBAccess.GDBAccessObj.SelectDT(SqlStmt, "TravelDates", ReadTransaction, parameters);
                     *
                     * for (int i = 0; i < travelData.Rows.Count; i++)
                     * {
                     *  PersonExpRecord = new TFormDataWorkExperience();
                     *
                     *  if ((travelData.Rows[i][0]).GetType() == typeof(DateTime))
                     *  {
                     *      PersonExpRecord.StartDate = (DateTime?)travelData.Rows[i][0];
                     *  }
                     *
                     *  if ((travelData.Rows[i][1]).GetType() == typeof(DateTime))
                     *  {
                     *      PersonExpRecord.EndDate = (DateTime?)travelData.Rows[i][1];
                     *  }
                     *
                     *  PersonExpRecord.Organisation = "";
                     *  PersonExpRecord.Role = "";
                     *  PersonExpRecord.Category = "";
                     *  PersonExpRecord.SameOrg = true;
                     *  PersonExpRecord.SimilarOrg = true;
                     *  PersonExpRecord.Comment = "";
                     *
                     *  // check if unit exists and use unit name as location
                     *  if (TPartnerServerLookups.GetPartnerShortName((Int64)travelData.Rows[i][2], out UnitShortName, out UnitClass))
                     *  {
                     *      PersonExpRecord.Location = UnitShortName;
                     *  }
                     *  else
                     *  {
                     *      PersonExpRecord.Location = travelData.Rows[i][3].ToString();
                     *  }
                     *
                     *  AFormDataPerson.AddWorkExperience(PersonExpRecord);
                     * }
                     */

                    // retrieve actual past experience records
                    PmPastExperienceTable PersonExpTable;
                    PersonExpTable = PmPastExperienceAccess.LoadViaPPerson(APartnerKey, AReadTransaction);

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

                        AFormDataPerson.AddWorkExperience(PersonExpRecord);
                    }
                }

                // retrieve Commitment information
                if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eCommitment)
                    || AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eFutureCommitment))
                {
                    String FieldName;
                    TPartnerClass FieldPartnerClass;
                    PmStaffDataTable PersonCommitmentTable;
                    PmStaffDataRow PersonCommitmentRow;
                    TFormDataCommitment PersonCommitmentRecord;
                    PersonCommitmentTable = PmStaffDataAccess.LoadViaPPerson(APartnerKey, AReadTransaction);
                    PersonCommitmentTable.DefaultView.Sort = PmStaffDataTable.GetStartOfCommitmentDBName() + " DESC";

                    if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eCommitment))
                    {
                        foreach (DataRowView rv in PersonCommitmentTable.DefaultView)
                        {
                            PersonCommitmentRow = (PmStaffDataRow)rv.Row;
                            PersonCommitmentRecord = new TFormDataCommitment();

                            PersonCommitmentRecord.StartDate = PersonCommitmentRow.StartOfCommitment;
                            PersonCommitmentRecord.EndDate = PersonCommitmentRow.EndOfCommitment;
                            PersonCommitmentRecord.Status = PersonCommitmentRow.StatusCode;

                            PersonCommitmentRecord.ReceivingFieldKey = PersonCommitmentRow.ReceivingField.ToString("0000000000");
                            TPartnerServerLookups.GetPartnerShortName(PersonCommitmentRow.ReceivingField, out FieldName,
                                out FieldPartnerClass);
                            PersonCommitmentRecord.ReceivingFieldName = FieldName;

                            PersonCommitmentRecord.SendingFieldKey = PersonCommitmentRow.HomeOffice.ToString("0000000000");
                            TPartnerServerLookups.GetPartnerShortName(PersonCommitmentRow.HomeOffice, out FieldName,
                                out FieldPartnerClass);
                            PersonCommitmentRecord.SendingFieldName = FieldName;

                            PersonCommitmentRecord.RecruitingFieldKey = PersonCommitmentRow.OfficeRecruitedBy.ToString("0000000000");
                            TPartnerServerLookups.GetPartnerShortName(PersonCommitmentRow.OfficeRecruitedBy, out FieldName,
                                out FieldPartnerClass);
                            PersonCommitmentRecord.RecruitingFieldName = FieldName;

                            PersonCommitmentRecord.Comment = PersonCommitmentRow.StaffDataComments;

                            AFormDataPerson.AddCommitment(PersonCommitmentRecord);
                        }
                    }

                    if (AFormLetterInfo.IsRetrievalRequested(TFormDataRetrievalSection.eFutureCommitment))
                    {
                        foreach (DataRowView rv in PersonCommitmentTable.DefaultView)
                        {
                            PersonCommitmentRow = (PmStaffDataRow)rv.Row;

                            if (PersonCommitmentRow.StartOfCommitment >= DateTime.Today)
                            {
                                TPartnerServerLookups.GetPartnerShortName(PersonCommitmentRow.ReceivingField, out FieldName,
                                    out FieldPartnerClass);
                                AFormDataPerson.FutureFieldName = FieldName;
                                AFormDataPerson.FutureCommitStartDate = PersonCommitmentRow.StartOfCommitment;
                                AFormDataPerson.FutureCommitEndDate = PersonCommitmentRow.EndOfCommitment;

                                // only use the first commitment (list is sorted by start date)
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// populate form data for given applicant key
        /// This Applicant Data and will also make a call to fill Personnel Data.
        /// </summary>
        /// <param name="AEventPartnerKey">Key of event record to be used</param>
        /// <param name="APartnerKey">Key of partner record to be used</param>
        /// <param name="AFormDataApplicant">Object containing Applicant data</param>
        /// <param name="AFormLetterInfo">Info class for form letter</param>
        /// <param name="ASiteKey">Site key for location record</param>
        /// <param name="ALocationKey">Key for location record</param>
        /// <returns></returns>
        [RequireModulePermission("AND(PERSONNEL,PTNRUSER)")]
        public static void FillFormDataFromApplicant(Int64 AEventPartnerKey, Int64 APartnerKey,
            TFormDataApplicant AFormDataApplicant,
            TFormLetterInfo AFormLetterInfo,
            Int64 ASiteKey = 0,
            Int32 ALocationKey = 0)
        {
            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref ReadTransaction,
                delegate
                {
                    FillFormDataFromApplicant(AEventPartnerKey, APartnerKey, AFormDataApplicant, AFormLetterInfo, ReadTransaction, ASiteKey,
                        ALocationKey);
                });
        }

        /// <summary>
        /// populate form data for given applicant key
        /// This Applicant Data and will also make a call to fill Personnel Data.
        /// </summary>
        /// <param name="AEventPartnerKey">Key of event record to be used</param>
        /// <param name="APartnerKey">Key of partner record to be used</param>
        /// <param name="AFormDataApplicant">Object containing Applicant data</param>
        /// <param name="AFormLetterInfo">Info class for form letter</param>
        /// <param name="AReadTransaction">Db transaction</param>
        /// <param name="ASiteKey">Site key for location record</param>
        /// <param name="ALocationKey">Key for location record</param>
        /// <returns></returns>
        [RequireModulePermission("AND(PERSONNEL,PTNRUSER)")]
        public static void FillFormDataFromApplicant(Int64 AEventPartnerKey, Int64 APartnerKey,
            TFormDataApplicant AFormDataApplicant,
            TFormLetterInfo AFormLetterInfo,
            TDBTransaction AReadTransaction,
            Int64 ASiteKey = 0,
            Int32 ALocationKey = 0)
        {
            TPartnerClass PartnerClass;
            String ShortName;
            TStdPartnerStatusCode PartnerStatusCode;

            if (AFormDataApplicant == null)
            {
                return;
            }

            if (MCommonMain.RetrievePartnerShortName(APartnerKey, out ShortName, out PartnerClass, out PartnerStatusCode, AReadTransaction))
            {
                // first retrieve all personnel information
                TFormLettersPersonnelWebConnector.FillFormDataFromPersonnel(APartnerKey,
                    AFormDataApplicant,
                    AFormLetterInfo,
                    AReadTransaction,
                    ASiteKey,
                    ALocationKey);

                // retrieve Applicant information
                PmShortTermApplicationTable ShortTermAppTable;
                PmShortTermApplicationRow ShortTermAppRow;

                PmShortTermApplicationRow template = new PmShortTermApplicationTable().NewRowTyped(false);

                template.PartnerKey = APartnerKey;
                template.StConfirmedOption = AEventPartnerKey;

                ShortTermAppTable = PmShortTermApplicationAccess.LoadUsingTemplate(template, AReadTransaction);

                if (ShortTermAppTable.Count > 0)
                {
                    ShortTermAppRow = (PmShortTermApplicationRow)ShortTermAppTable.Rows[0];
                    AFormDataApplicant.EventPartnerKey = AEventPartnerKey.ToString("0000000000");
                    AFormDataApplicant.ArrivalDate = ShortTermAppRow.Arrival;
                    AFormDataApplicant.ArrivalTime = new DateTime(ShortTermAppRow.Arrival == null ? 0 : ((DateTime)ShortTermAppRow.Arrival).Year,
                        ShortTermAppRow.Arrival == null ? 0 : ((DateTime)ShortTermAppRow.Arrival).Month,
                        ShortTermAppRow.Arrival == null ? 0 : ((DateTime)ShortTermAppRow.Arrival).Day,
                        ShortTermAppRow.ArrivalHour, ShortTermAppRow.ArrivalMinute, 0);
                    AFormDataApplicant.ArrivalFlightNumber = ShortTermAppRow.FromCongTravelInfo;
                    AFormDataApplicant.ArrivalComment = ShortTermAppRow.ArrivalComments;
                    AFormDataApplicant.DepartureDate = ShortTermAppRow.Departure;
                    AFormDataApplicant.DepartureTime = new DateTime(ShortTermAppRow.Departure == null ? 0 : ((DateTime)ShortTermAppRow.Arrival).Year,
                        ShortTermAppRow.Departure == null ? 0 : ((DateTime)ShortTermAppRow.Arrival).Month,
                        ShortTermAppRow.Departure == null ? 0 : ((DateTime)ShortTermAppRow.Arrival).Day,
                        ShortTermAppRow.DepartureHour, ShortTermAppRow.DepartureMinute, 0);
                    AFormDataApplicant.DepartureFlightNumber = ShortTermAppRow.ToCongTravelInfo;
                    AFormDataApplicant.DepartureComment = ShortTermAppRow.DepartureComments;
                    AFormDataApplicant.EventRole = ShortTermAppRow.StCongressCode;
                    AFormDataApplicant.CampaignType = ShortTermAppRow.ConfirmedOptionCode.Substring(5, 6);
                    AFormDataApplicant.FellowshipGroup = ShortTermAppRow.StFgCode;

                    TPartnerClass PartnerClassDummy = new TPartnerClass();
                    AFormDataApplicant.ChargedFieldKey = ShortTermAppRow.StFieldCharged.ToString("0000000000");

                    string chargedFieldName = "";

                    if (!ShortTermAppRow.IsStCurrentFieldNull())
                    {
                        TPartnerServerLookups.GetPartnerShortName(ShortTermAppRow.StCurrentField, out chargedFieldName, out PartnerClassDummy);
                    }

                    AFormDataApplicant.SendingFieldName = chargedFieldName;

                    /*ApplicantFormData.SendingFieldKey = ShortTermAppRow.StCurrentField.ToString("0000000000");
                     *
                     *
                     * string sendingFieldName;
                     * TPartnerServerLookups.GetPartnerShortName(ShortTermAppRow.StCurrentField, out sendingFieldName, out PartnerClassDummy);
                     * ApplicantFormData.SendingFieldName = sendingFieldName;*/

                    //TODO: SendingFieldKey & -name;  ReceivingFieldKey & -name


                    PmGeneralApplicationTable GeneralAppTable;
                    PmGeneralApplicationRow GeneralAppRow;

                    GeneralAppTable = PmGeneralApplicationAccess.LoadByPrimaryKey(APartnerKey,
                        ShortTermAppRow.ApplicationKey,
                        ShortTermAppRow.RegistrationOffice,
                        AReadTransaction);

                    if (GeneralAppTable.Count > 0)
                    {
                        GeneralAppRow = (PmGeneralApplicationRow)GeneralAppTable.Rows[0];
                        AFormDataApplicant.RegistrationDate = GeneralAppRow.GenAppDate;
                        AFormDataApplicant.ApplicationComment = GeneralAppRow.Comment;
                    }
                }
            }
        }
    }
}
