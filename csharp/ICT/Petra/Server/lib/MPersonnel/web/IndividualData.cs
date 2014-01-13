//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Runtime.Remoting;
using System.Xml;
using System.IO;
using GNU.Gettext;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPersonnel.Person;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Shared.MPersonnel.Units.Data;
using Ict.Petra.Server.MPersonnel.Units.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MCommon.Cacheable;
using Ict.Petra.Server.MCommon.UIConnectors;
using Ict.Petra.Server.MCommon.WebConnectors;
using Ict.Petra.Server.MPartner;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Cacheable;
using Ict.Petra.Server.App.Core.Security;


namespace Ict.Petra.Server.MPersonnel.Person.DataElements.WebConnectors
{
    /// <summary>
    /// Web Connector for the Individual Data of a PERSON.
    /// </summary>
    public class TIndividualDataWebConnector
    {
        /// <summary>
        /// Passes data as a Typed DataSet to the caller, containing a DataTable that corresponds with <paramref name="AIndivDataItem"></paramref>.
        /// </summary>
        /// <remarks>Starts and ends a DB Transaction automatically if there isn't one running yet.</remarks>
        /// <param name="APartnerKey">PartnerKey of the Person to load data for.</param>
        /// <param name="AIndivDataItem">The Individual Data Item for which data should be returned.</param>
        /// <returns>A Typed DataSet containing a DataTable that corresponds with <paramref name="AIndivDataItem"></paramref>.</returns>
        [RequireModulePermission("AND(PERSONNEL,PTNRUSER)")]
        public static IndividualDataTDS GetData(Int64 APartnerKey, TIndividualDataItemEnum AIndivDataItem)
        {
            IndividualDataTDS ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;

            ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.RepeatableRead,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);
            ReturnValue = GetData(APartnerKey, AIndivDataItem, ReadTransaction);

            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel(7, "TIndividualDataWebConnector.GetData: committed own transaction.");
            }

            return ReturnValue;
        }

        /// <summary>
        /// Passes data as a Typed DataSet to the caller, containing a DataTable that corresponds with <paramref name="AIndivDataItem"></paramref>.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Person to load data for.</param>
        /// <param name="AIndivDataItem">The Individual Data Item for which data should be returned.</param>
        /// <param name="AReadTransaction">Open Database transaction.</param>
        /// <returns>A Typed DataSet containing a DataTable that corresponds with <paramref name="AIndivDataItem"></paramref>.</returns>
        private static IndividualDataTDS GetData(Int64 APartnerKey, TIndividualDataItemEnum AIndivDataItem, TDBTransaction AReadTransaction)
        {
            IndividualDataTDS IndividualDataDS = new IndividualDataTDS("IndividualData");   // create the IndividualDataTDS DataSet that will later be passed to the Client
            IndividualDataTDSMiscellaneousDataTable MiscellaneousDataDT;
            IndividualDataTDSMiscellaneousDataRow MiscellaneousDataDR;

            #region Create 'Miscellaneous' DataRow

            MiscellaneousDataDT = IndividualDataDS.MiscellaneousData;
            MiscellaneousDataDR = MiscellaneousDataDT.NewRowTyped(false);
            MiscellaneousDataDR.PartnerKey = APartnerKey;

            MiscellaneousDataDT.Rows.Add(MiscellaneousDataDR);

            #endregion

            switch (AIndivDataItem)
            {
                case TIndividualDataItemEnum.idiSummary:
                    BuildSummaryData(APartnerKey, ref IndividualDataDS, AReadTransaction);

                    DetermineItemCounts(MiscellaneousDataDR, AReadTransaction);
                    break;

                case TIndividualDataItemEnum.idiPersonalLanguages:
                    PmPersonLanguageAccess.LoadViaPPerson(IndividualDataDS, APartnerKey, AReadTransaction);

                    PLanguageTable LanguageTable = (PLanguageTable)TSharedDataCache.TMCommon.GetCacheableCommonTable(
                    TCacheableCommonTablesEnum.LanguageCodeList);
                    PLanguageRow LanguageRow;

                    foreach (IndividualDataTDSPmPersonLanguageRow PersonLanguageRow in IndividualDataDS.PmPersonLanguage.Rows)
                    {
                        LanguageRow = (PLanguageRow)LanguageTable.Rows.Find(new object[] { PersonLanguageRow.LanguageCode });

                        if (LanguageRow != null)
                        {
                            PersonLanguageRow.LanguageDescription = LanguageRow.LanguageDescription;
                        }
                    }

                    break;

                case TIndividualDataItemEnum.idiSpecialNeeds:
                    PmSpecialNeedAccess.LoadByPrimaryKey(IndividualDataDS, APartnerKey, AReadTransaction);
                    break;

                case TIndividualDataItemEnum.idiPreviousExperiences:
                    PmPastExperienceAccess.LoadViaPPerson(IndividualDataDS, APartnerKey, AReadTransaction);
                    break;

                case TIndividualDataItemEnum.idiPersonalDocuments:
                    PmDocumentAccess.LoadViaPPerson(IndividualDataDS, APartnerKey, AReadTransaction);
                    break;

                case TIndividualDataItemEnum.idiJobAssignments:
                    PmJobAssignmentAccess.LoadViaPPartner(IndividualDataDS, APartnerKey, AReadTransaction);
                    break;

                case TIndividualDataItemEnum.idiLocalPersonnelData:
                    // TODO: Fix this so LocalPersonnelData can actually load some data
                    bool labelsAvailable;
                    TOfficeSpecificDataLabelsUIConnector OfficeSpecificDataLabelsUIConnector;
                    OfficeSpecificDataLabelsUIConnector = new TOfficeSpecificDataLabelsUIConnector(APartnerKey,
                    TOfficeSpecificDataLabelUseEnum.Personnel);
                    IndividualDataDS.Merge(OfficeSpecificDataLabelsUIConnector.GetDataLocalPartnerDataValues(APartnerKey, out labelsAvailable, false,
                        AReadTransaction));
                    break;

                case TIndividualDataItemEnum.idiProgressReports:
                    PmPersonEvaluationAccess.LoadViaPPerson(IndividualDataDS, APartnerKey, AReadTransaction);
                    break;

                case TIndividualDataItemEnum.idiCommitmentPeriods:
                    PmStaffDataAccess.LoadViaPPerson(IndividualDataDS, APartnerKey, AReadTransaction);
                    break;

                case TIndividualDataItemEnum.idiPersonSkills:
                    PmPersonSkillAccess.LoadViaPPerson(IndividualDataDS, APartnerKey, AReadTransaction);
                    break;

                case TIndividualDataItemEnum.idiPersonalAbilities:
                    PmPersonAbilityAccess.LoadViaPPerson(IndividualDataDS, APartnerKey, AReadTransaction);
                    break;

                case TIndividualDataItemEnum.idiPassportDetails:
                    PmPassportDetailsAccess.LoadViaPPerson(IndividualDataDS, APartnerKey, AReadTransaction);

                    PCountryTable CountryTable = (PCountryTable)TSharedDataCache.TMCommon.GetCacheableCommonTable(
                    TCacheableCommonTablesEnum.CountryList);
                    PCountryRow CountryRow;

                    foreach (IndividualDataTDSPmPassportDetailsRow PassportRow in IndividualDataDS.PmPassportDetails.Rows)
                    {
                        CountryRow = (PCountryRow)CountryTable.Rows.Find(new object[] { PassportRow.PassportNationalityCode });

                        if (CountryRow != null)
                        {
                            PassportRow.PassportNationalityName = CountryRow.CountryName;
                        }
                    }

                    break;

                case TIndividualDataItemEnum.idiPersonalData:
                    PmPersonalDataAccess.LoadByPrimaryKey(IndividualDataDS, APartnerKey, AReadTransaction);
                    break;

                case TIndividualDataItemEnum.idiEmergencyData:
                    PmPersonalDataAccess.LoadByPrimaryKey(IndividualDataDS, APartnerKey, AReadTransaction);
                    break;

                case TIndividualDataItemEnum.idiApplications:
                    PmGeneralApplicationAccess.LoadViaPPersonPartnerKey(IndividualDataDS, APartnerKey, AReadTransaction);
                    PmShortTermApplicationAccess.LoadViaPPerson(IndividualDataDS, APartnerKey, AReadTransaction);
                    PmYearProgramApplicationAccess.LoadViaPPerson(IndividualDataDS, APartnerKey, AReadTransaction);

                    IndividualDataTDSPmGeneralApplicationRow GenAppRow;
                    TPartnerClass PartnerClass;
                    TStdPartnerStatusCode PartnerStatus;
                    String EventOrFieldName;

                    //TODO: now go through all short and long term apps and set the
                    // two fields in general app for EventOrFieldName and ApplicationForEventOrField
                    foreach (PmShortTermApplicationRow ShortTermRow in IndividualDataDS.PmShortTermApplication.Rows)
                    {
                        GenAppRow = (IndividualDataTDSPmGeneralApplicationRow)IndividualDataDS.PmGeneralApplication.Rows.Find
                                        (new object[] { ShortTermRow.PartnerKey,
                                                        ShortTermRow.ApplicationKey, ShortTermRow.RegistrationOffice });
                        GenAppRow.ApplicationForEventOrField = Catalog.GetString("Event");

                        if (!ShortTermRow.IsStConfirmedOptionNull())
                        {
                            Ict.Petra.Server.MCommon.MCommonMain.RetrievePartnerShortName
                                (ShortTermRow.StConfirmedOption, out EventOrFieldName, out PartnerClass, out PartnerStatus);
                            GenAppRow.EventOrFieldName = EventOrFieldName;
                        }
                    }

                    foreach (PmYearProgramApplicationRow LongTermRow in IndividualDataDS.PmYearProgramApplication.Rows)
                    {
                        GenAppRow = (IndividualDataTDSPmGeneralApplicationRow)IndividualDataDS.PmGeneralApplication.Rows.Find
                                        (new object[] { LongTermRow.PartnerKey,
                                                        LongTermRow.ApplicationKey, LongTermRow.RegistrationOffice });
                        GenAppRow.ApplicationForEventOrField = Catalog.GetString("Field");

                        if (!GenAppRow.IsGenAppPossSrvUnitKeyNull())
                        {
                            Ict.Petra.Server.MCommon.MCommonMain.RetrievePartnerShortName
                                (GenAppRow.GenAppPossSrvUnitKey, out EventOrFieldName, out PartnerClass, out PartnerStatus);
                            GenAppRow.EventOrFieldName = EventOrFieldName;
                        }
                    }

                    break;

                    // TODO: work on all cases/load data for all Individual Data items
            }

            return IndividualDataDS;
        }

        /// <summary>
        /// Retrieves data that will be shown on the 'Overview' UserControl and adds it to <paramref name="AIndividualDataDS" />.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the PERSON to load data for.</param>
        /// <param name="AIndividualDataDS">Typed DataSet of Type <see cref="IndividualDataTDS" />. Needs to be instantiated already!</param>
        [RequireModulePermission("AND(PERSONNEL,PTNRUSER)")]
        public static bool GetSummaryData(Int64 APartnerKey, ref IndividualDataTDS AIndividualDataDS)
        {
            Boolean NewTransaction;

            TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                Ict.Petra.Server.MCommon.MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
                TEnforceIsolationLevel.eilMinimum,
                out NewTransaction);

            try
            {
                BuildSummaryData(APartnerKey, ref AIndividualDataDS, ReadTransaction);
            }
            finally
            {
                if (NewTransaction)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    TLogging.LogAtLevel(7, "TIndividualDataWebConnector.BuildSummaryData commited own transaction.");
                }
            }

            return true;
        }

        /// <summary>
        /// Retrieves data that will be shown on the 'Overview' UserControl and adds it to <paramref name="AIndividualDataDS" />.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Person to load data for.</param>
        /// <param name="AIndividualDataDS">Typed DataSet of Type <see cref="IndividualDataTDS" />. Needs to be instantiated already!</param>
        /// <param name="AReadTransaction">Open Database transaction.</param>
        /// <returns>void</returns>
        private static void BuildSummaryData(Int64 APartnerKey, ref IndividualDataTDS AIndividualDataDS, TDBTransaction AReadTransaction)
        {
            string StrNotAvailable = Catalog.GetString("Not Available");
            IndividualDataTDSSummaryDataTable SummaryDT;
            IndividualDataTDSSummaryDataRow SummaryDR;
            IndividualDataTDSMiscellaneousDataRow MiscellaneousDataDR = AIndividualDataDS.MiscellaneousData[0];
            PPersonTable PPersonDT;
            PPersonRow PersonDR = null;
            PmPassportDetailsTable PassportDetailsDT;
            PmStaffDataTable PmStaffDataDT;
            PmStaffDataRow PmStaffDataDR = null;
            PmJobAssignmentTable PmJobAssignmentDT = null;
            PUnitTable PUnitDT = null;
            PmJobAssignmentRow PmJobAssignmentDR;
            IndividualDataTDSJobAssignmentStaffDataCombinedRow JobAssiStaffDataCombDR;
            int JobAssiStaffDataCombKey = 0;
            TCacheable CommonCacheable = new TCacheable();
            TPartnerCacheable PartnerCacheable = new TPartnerCacheable();
            string MaritalStatusDescr;
            StringCollection PassportColumns;
            string Nationalities;
            PPartnerRelationshipTable PartnerRelationshipDT;
            PPartnerTable PartnerDT;
            PPartnerRow PartnerDR = null;
            PLocationRow LocationDR;
            PPartnerLocationRow PartnerLocationDR;
            string PhoneNumber;
            string PhoneExtension = String.Empty;
            Int64 ChurchPartnerKey;

            SummaryDT = new IndividualDataTDSSummaryDataTable();
            SummaryDR = SummaryDT.NewRowTyped(false);

            SummaryDR.PartnerKey = APartnerKey;

            #region Person Info

            PPersonDT = PPersonAccess.LoadByPrimaryKey(APartnerKey, AReadTransaction);

            if (PPersonDT.Rows.Count == 1)
            {
                PersonDR = (PPersonRow)PPersonDT.Rows[0];
            }

            if (PersonDR != null)
            {
                SummaryDR.DateOfBirth = PersonDR.DateOfBirth;
                SummaryDR.Gender = PersonDR.Gender;

                MaritalStatusDescr = PartnerCodeHelper.GetMaritalStatusDescription(
                    @PartnerCacheable.GetCacheableTable, PersonDR.MaritalStatus);

                if (MaritalStatusDescr != String.Empty)
                {
                    MaritalStatusDescr = " - " + MaritalStatusDescr;
                }

                SummaryDR.MaritalStatus = PersonDR.MaritalStatus + MaritalStatusDescr;
            }
            else
            {
                SummaryDR.SetDateOfBirthNull();
                SummaryDR.Gender = StrNotAvailable;
                SummaryDR.MaritalStatus = StrNotAvailable;
            }

            #region Nationalities

            PassportColumns = StringHelper.StrSplit(
                PmPassportDetailsTable.GetDateOfIssueDBName() + "," +
                PmPassportDetailsTable.GetDateOfExpirationDBName() + "," +
                PmPassportDetailsTable.GetPassportNationalityCodeDBName() + "," +
                PmPassportDetailsTable.GetMainPassportDBName(), ",");

            PassportDetailsDT = PmPassportDetailsAccess.LoadViaPPerson(APartnerKey,
                PassportColumns, AReadTransaction, null, 0, 0);

            Nationalities = Ict.Petra.Shared.MPersonnel.Calculations.DeterminePersonsNationalities(
                @CommonCacheable.GetCacheableTable, PassportDetailsDT);

            if (Nationalities != String.Empty)
            {
                SummaryDR.Nationalities = Nationalities;
            }
            else
            {
                SummaryDR.Nationalities = StrNotAvailable;
            }

            #endregion

            #region Phone and Email (from 'Best Address')

            ServerCalculations.DetermineBestAddress(APartnerKey, out PartnerLocationDR, out LocationDR);

            if (LocationDR != null)
            {
                SummaryDR.EmailAddress = PartnerLocationDR.EmailAddress;

                if (PartnerLocationDR.TelephoneNumber != String.Empty)
                {
                    PhoneNumber = PartnerLocationDR.TelephoneNumber;

                    if (!PartnerLocationDR.IsExtensionNull())
                    {
                        PhoneExtension = PartnerLocationDR.Extension.ToString();
                    }

                    SummaryDR.TelephoneNumber = Calculations.FormatIntlPhoneNumber(PhoneNumber, PhoneExtension, LocationDR.CountryCode,
                        @CommonCacheable.GetCacheableTable);
                }
                else if (PartnerLocationDR.MobileNumber != String.Empty)
                {
                    SummaryDR.TelephoneNumber = Calculations.FormatIntlPhoneNumber(PartnerLocationDR.MobileNumber,
                        String.Empty, LocationDR.CountryCode, @CommonCacheable.GetCacheableTable) + " " +
                                                Catalog.GetString("(Mobile)");
                }
                else
                {
                    SummaryDR.TelephoneNumber = StrNotAvailable;
                }
            }
            else
            {
                SummaryDR.TelephoneNumber = StrNotAvailable;
                SummaryDR.EmailAddress = StrNotAvailable;
            }

            #endregion

            #endregion

            #region Commitments/Jobs

            PmStaffDataDT = PmStaffDataAccess.LoadViaPPerson(APartnerKey, AReadTransaction);
            MiscellaneousDataDR.ItemsCountCommitmentPeriods = PmStaffDataDT.Rows.Count;

            // First check if the PERSON has got any Commitments
            if (PmStaffDataDT.Rows.Count > 0)
            {
                foreach (DataRow DR in PmStaffDataDT.Rows)
                {
                    JobAssiStaffDataCombDR = AIndividualDataDS.JobAssignmentStaffDataCombined.NewRowTyped(false);
                    JobAssiStaffDataCombDR.Key = JobAssiStaffDataCombKey++;
                    JobAssiStaffDataCombDR.PartnerKey = APartnerKey;

                    PmStaffDataDR = (PmStaffDataRow)DR;

                    if (!(PmStaffDataDR.IsReceivingFieldNull())
                        && (PmStaffDataDR.ReceivingField != 0))
                    {
                        PUnitDT = PUnitAccess.LoadByPrimaryKey(PmStaffDataDR.ReceivingField, AReadTransaction);

                        JobAssiStaffDataCombDR.FieldKey = PmStaffDataDR.ReceivingField;
                        JobAssiStaffDataCombDR.FieldName = PUnitDT[0].UnitName;
                    }
                    else
                    {
                        JobAssiStaffDataCombDR.FieldKey = 0;
                        JobAssiStaffDataCombDR.FieldName = "[None]";
                    }

                    JobAssiStaffDataCombDR.Position = PmStaffDataDR.JobTitle;
                    JobAssiStaffDataCombDR.FromDate = PmStaffDataDR.StartOfCommitment;
                    JobAssiStaffDataCombDR.ToDate = PmStaffDataDR.EndOfCommitment;

                    AIndividualDataDS.JobAssignmentStaffDataCombined.Rows.Add(JobAssiStaffDataCombDR);
                }
            }
            else
            {
                // The PERSON hasn't got any Commitments, therefore check if the PERSON has any Job Assignments

                PmJobAssignmentDT = PmJobAssignmentAccess.LoadViaPPartner(APartnerKey, AReadTransaction);

                if (PmJobAssignmentDT.Rows.Count > 0)
                {
                    foreach (DataRow DR in PmJobAssignmentDT.Rows)
                    {
                        JobAssiStaffDataCombDR = AIndividualDataDS.JobAssignmentStaffDataCombined.NewRowTyped(false);
                        JobAssiStaffDataCombDR.Key = JobAssiStaffDataCombKey++;
                        JobAssiStaffDataCombDR.PartnerKey = APartnerKey;

                        PmJobAssignmentDR = (PmJobAssignmentRow)DR;

                        if (PmJobAssignmentDR.UnitKey != 0)
                        {
                            PUnitDT = PUnitAccess.LoadByPrimaryKey(PmJobAssignmentDR.UnitKey, AReadTransaction);

                            JobAssiStaffDataCombDR.FieldKey = PmJobAssignmentDR.UnitKey;
                            JobAssiStaffDataCombDR.FieldName = PUnitDT[0].UnitName;
                        }
                        else
                        {
                            JobAssiStaffDataCombDR.FieldKey = 0;
                            JobAssiStaffDataCombDR.FieldName = "[None]";
                        }

                        JobAssiStaffDataCombDR.Position = PmJobAssignmentDR.PositionName;
                        JobAssiStaffDataCombDR.FromDate = PmJobAssignmentDR.FromDate;
                        JobAssiStaffDataCombDR.ToDate = PmJobAssignmentDR.ToDate;

                        AIndividualDataDS.JobAssignmentStaffDataCombined.Rows.Add(JobAssiStaffDataCombDR);
                    }
                }
            }

            #endregion

            #region Church Info

            SummaryDR.ChurchName = StrNotAvailable;
            SummaryDR.ChurchAddress = StrNotAvailable;
            SummaryDR.ChurchPhone = StrNotAvailable;
            SummaryDR.ChurchPastor = StrNotAvailable;
            SummaryDR.ChurchPastorsPhone = StrNotAvailable;
            SummaryDR.NumberOfShownSupportingChurchPastors = 0;

            // Find SUPPCHURCH Relationship
            PartnerRelationshipDT = PPartnerRelationshipAccess.LoadUsingTemplate(new TSearchCriteria[] {
                    new TSearchCriteria(PPartnerRelationshipTable.GetRelationKeyDBName(), APartnerKey),
                    new TSearchCriteria(PPartnerRelationshipTable.GetRelationNameDBName(), "SUPPCHURCH")
                },
                AReadTransaction);

            SummaryDR.NumberOfShownSupportingChurches = PartnerRelationshipDT.Rows.Count;

            if (PartnerRelationshipDT.Rows.Count > 0)
            {
                ChurchPartnerKey = PartnerRelationshipDT[0].PartnerKey;

                // Load Church Partner
                PartnerDT = PPartnerAccess.LoadByPrimaryKey(ChurchPartnerKey, AReadTransaction);

                if (PartnerDT.Rows.Count > 0)
                {
                    PartnerDR = PartnerDT[0];

                    // Church Name
                    if (PartnerDR.PartnerShortName != String.Empty)
                    {
                        SummaryDR.ChurchName = PartnerDR.PartnerShortName;
                    }

                    #region Church Address and Phone

                    ServerCalculations.DetermineBestAddress(PartnerRelationshipDT[0].PartnerKey, out PartnerLocationDR, out LocationDR);

                    if (LocationDR != null)
                    {
                        SummaryDR.ChurchAddress = Calculations.DetermineLocationString(LocationDR,
                            Calculations.TPartnerLocationFormatEnum.plfCommaSeparated);

                        // Church Phone
                        if (PartnerLocationDR.TelephoneNumber != String.Empty)
                        {
                            PhoneNumber = PartnerLocationDR.TelephoneNumber;

                            if (!PartnerLocationDR.IsExtensionNull())
                            {
                                PhoneExtension = PartnerLocationDR.Extension.ToString();
                            }

                            SummaryDR.ChurchPhone = Calculations.FormatIntlPhoneNumber(PhoneNumber, PhoneExtension, LocationDR.CountryCode,
                                @CommonCacheable.GetCacheableTable);
                        }
                        else if (PartnerLocationDR.MobileNumber != String.Empty)
                        {
                            SummaryDR.ChurchPhone = Calculations.FormatIntlPhoneNumber(PartnerLocationDR.MobileNumber,
                                String.Empty, LocationDR.CountryCode, @CommonCacheable.GetCacheableTable) + " " +
                                                    Catalog.GetString("(Mobile)");
                        }
                    }

                    #endregion

                    #region Pastor

                    // Find PASTOR Relationship
                    PartnerRelationshipDT.Rows.Clear();
                    PartnerRelationshipDT = PPartnerRelationshipAccess.LoadUsingTemplate(new TSearchCriteria[] {
                            new TSearchCriteria(PPartnerRelationshipTable.GetPartnerKeyDBName(), ChurchPartnerKey),
                            new TSearchCriteria(PPartnerRelationshipTable.GetRelationNameDBName(), "PASTOR")
                        },
                        AReadTransaction);

                    SummaryDR.NumberOfShownSupportingChurchPastors = PartnerRelationshipDT.Rows.Count;

                    if (PartnerRelationshipDT.Rows.Count > 0)
                    {
                        // Load PASTOR Partner
                        PartnerDT = PPartnerAccess.LoadByPrimaryKey(PartnerRelationshipDT[0].RelationKey, AReadTransaction);

                        if (PartnerDT.Rows.Count > 0)
                        {
                            PartnerDR = PartnerDT[0];

                            // Pastor's Name
                            if (PartnerDR.PartnerShortName != String.Empty)
                            {
                                SummaryDR.ChurchPastor = PartnerDR.PartnerShortName;
                            }

                            #region Pastor's Phone

                            ServerCalculations.DetermineBestAddress(PartnerRelationshipDT[0].RelationKey,
                                out PartnerLocationDR, out LocationDR);

                            if (LocationDR != null)
                            {
                                // Pastor's Phone
                                if (PartnerLocationDR.TelephoneNumber != String.Empty)
                                {
                                    PhoneNumber = PartnerLocationDR.TelephoneNumber;

                                    if (!PartnerLocationDR.IsExtensionNull())
                                    {
                                        PhoneExtension = PartnerLocationDR.Extension.ToString();
                                    }

                                    SummaryDR.ChurchPastorsPhone = Calculations.FormatIntlPhoneNumber(PhoneNumber,
                                        PhoneExtension, LocationDR.CountryCode, @CommonCacheable.GetCacheableTable);
                                }
                                else if (PartnerLocationDR.MobileNumber != String.Empty)
                                {
                                    SummaryDR.ChurchPastorsPhone = Calculations.FormatIntlPhoneNumber(PartnerLocationDR.MobileNumber,
                                        String.Empty, LocationDR.CountryCode, @CommonCacheable.GetCacheableTable) + " " +
                                                                   Catalog.GetString("(Mobile)");
                                }
                            }

                            #endregion
                        }
                    }

                    #endregion
                }
            }

            #endregion

            // Add Summary DataRow to Summary DataTable
            SummaryDT.Rows.Add(SummaryDR);

            // Add Row to 'SummaryData' DataTable in Typed DataSet 'IndividualDataTDS'
            AIndividualDataDS.Merge(SummaryDT);
        }

        /// <summary>
        /// Determines the number of DataRows for the Individual Data Items that work on multiple DataRows.
        /// </summary>
        /// <param name="AMiscellaneousDataDR">Instance of <see cref="IndividualDataTDSMiscellaneousDataRow" />.</param>
        /// <param name="AReadTransaction">Open Database transaction.</param>
        /// <returns>void</returns>
        private static void DetermineItemCounts(IndividualDataTDSMiscellaneousDataRow AMiscellaneousDataDR, TDBTransaction AReadTransaction)
        {
            Int64 PartnerKey = AMiscellaneousDataDR.PartnerKey;

            // Note: Commitment Records are counted already in BuildSummaryData and therefore don't need to be done here.

            AMiscellaneousDataDR.ItemsCountPassportDetails = PmPassportDetailsAccess.CountViaPPerson(PartnerKey, AReadTransaction);
            AMiscellaneousDataDR.ItemsCountPersonalDocuments = PmDocumentAccess.CountViaPPerson(PartnerKey, AReadTransaction);
            AMiscellaneousDataDR.ItemsCountProfessionalAreas = PmPersonQualificationAccess.CountViaPPerson(PartnerKey, AReadTransaction);
            AMiscellaneousDataDR.ItemsCountPersonalLanguages = PmPersonLanguageAccess.CountViaPPerson(PartnerKey, AReadTransaction);
            AMiscellaneousDataDR.ItemsCountPersonalAbilities = PmPersonAbilityAccess.CountViaPPerson(PartnerKey, AReadTransaction);
            AMiscellaneousDataDR.ItemsCountPreviousExperience = PmPastExperienceAccess.CountViaPPerson(PartnerKey, AReadTransaction);
            AMiscellaneousDataDR.ItemsCountCommitmentPeriods = PmStaffDataAccess.CountViaPPerson(PartnerKey, AReadTransaction);
            AMiscellaneousDataDR.ItemsCountJobAssignments = PmJobAssignmentAccess.CountViaPPartner(PartnerKey, AReadTransaction);
            AMiscellaneousDataDR.ItemsCountProgressReports = PmPersonEvaluationAccess.CountViaPPerson(PartnerKey, AReadTransaction);
            AMiscellaneousDataDR.ItemsCountPersonSkills = PmPersonSkillAccess.CountViaPPerson(PartnerKey, AReadTransaction);
            AMiscellaneousDataDR.ItemsCountApplications = PmGeneralApplicationAccess.CountViaPPersonPartnerKey(PartnerKey, AReadTransaction);
        }

        /// <summary>
        /// Saves data from the Individual Data UserControls (contained in a DataSet).
        /// </summary>
        /// <param name="AInspectDS">DataSet for the Personnel Individual Data.</param>
        /// <param name="APartnerEditInspectDS">DataSet for the whole Partner Edit screen.</param>
        /// <param name="ASubmitChangesTransaction">Open Database transaction.</param>
        /// <param name="AVerificationResult">Nil if all verifications are OK and all DB calls
        /// succeded, otherwise filled with 1..n TVerificationResult objects
        /// (can also contain DB call exceptions).</param>
        /// <returns>
        /// True if all verifications are OK and all DB calls succeeded, false if
        /// any verification or DB call failed.
        /// </returns>
        [NoRemoting]
        public static TSubmitChangesResult SubmitChangesServerSide(ref IndividualDataTDS AInspectDS,
            ref PartnerEditTDS APartnerEditInspectDS,
            TDBTransaction ASubmitChangesTransaction,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult;
            TVerificationResultCollection SingleVerificationResultCollection;

            PmJobAssignmentTable PmJobAssignmentTableSubmit;

            AVerificationResult = new TVerificationResultCollection();

            if (AInspectDS != null)
            {
                SubmissionResult = TSubmitChangesResult.scrOK;

                // Job Assignments: make sure that jobs exist for assignments
                if (AInspectDS.Tables.Contains(PmJobAssignmentTable.GetTableName())
                    && (AInspectDS.PmJobAssignment.Rows.Count > 0))
                {
                    UmJobTable JobTableTemp = new UmJobTable();

                    UmJobTable JobTableSubmit = new UmJobTable();
                    UmJobRow JobRow;

                    PmJobAssignmentTableSubmit = AInspectDS.PmJobAssignment;

                    // every job_assignment_row needs to have a row that it references in um_job
                    foreach (PmJobAssignmentRow JobAssignmentRow in PmJobAssignmentTableSubmit.Rows)
                    {
                        if (JobAssignmentRow.RowState != DataRowState.Deleted)
                        {
                            JobTableTemp = UmJobAccess.LoadByPrimaryKey(JobAssignmentRow.UnitKey,
                                JobAssignmentRow.PositionName,
                                JobAssignmentRow.PositionScope,
                                JobAssignmentRow.JobKey,
                                ASubmitChangesTransaction);

                            // if no corresponding job record found then we need to create one
                            // (job key was already set on client side to new value so merging back to the
                            // client does not cause problems because of primary key change)
                            if (JobTableTemp.Count == 0)
                            {
                                JobRow = (UmJobRow)JobTableSubmit.NewRow();

                                JobRow.UnitKey = JobAssignmentRow.UnitKey;
                                JobRow.PositionName = JobAssignmentRow.PositionName;
                                JobRow.PositionScope = JobAssignmentRow.PositionScope;
                                JobRow.JobKey = JobAssignmentRow.JobKey;
                                JobRow.FromDate = JobAssignmentRow.FromDate;
                                JobRow.ToDate = JobAssignmentRow.ToDate;
                                JobRow.CommitmentPeriod = "None";
                                JobRow.TrainingPeriod = "None";

                                // Need to update the JobKey field in job assignment table record from job record
                                JobAssignmentRow.JobKey = JobRow.JobKey;

                                JobTableSubmit.Rows.Add(JobRow);
                            }
                            else
                            {
                                // job record exists: in this case we need to update JobKey in
                                // the Job Assignment Record from Job Row
                                JobAssignmentRow.JobKey = ((UmJobRow)JobTableTemp.Rows[0]).JobKey;
                            }
                        }
                    }

                    // submit table with newly created jobs
                    if (JobTableSubmit.Rows.Count > 0)
                    {
                        if (UmJobAccess.SubmitChanges(JobTableSubmit, ASubmitChangesTransaction,
                                out SingleVerificationResultCollection))
                        {
                            SubmissionResult = TSubmitChangesResult.scrOK;
                        }
                        else
                        {
                            SubmissionResult = TSubmitChangesResult.scrError;
                            AVerificationResult.AddCollection(SingleVerificationResultCollection);
                            TLogging.LogAtLevel(9,
                                Messages.BuildMessageFromVerificationResult(
                                    "TIndividualDataWebConnector.SubmitChangesServerSide VerificationResult: ",
                                    AVerificationResult));
                        }
                    }
                }

                // now submit the whole dataset at once
                if (SubmissionResult != TSubmitChangesResult.scrError)
                {
                    IndividualDataTDSAccess.SubmitChanges(AInspectDS);
                    
                    SubmissionResult = TSubmitChangesResult.scrOK;

                    // Need to merge tables back into APartnerEditInspectDS so the updated s_modification_id_t is returned
                    // correctly to the Partner Edit.
                    // Unfortunately this can't be done simply by using merge method of the dataset since they are two different
                    // types but has to be done per table.
                    if (AInspectDS.Tables.Contains(PmSpecialNeedTable.GetTableName())
                        && (AInspectDS.PmSpecialNeed.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmSpecialNeedTable.GetTableName()].Merge(AInspectDS.PmSpecialNeed);
                    }

                    if (AInspectDS.Tables.Contains(PmPersonAbilityTable.GetTableName())
                        && (AInspectDS.PmPersonAbility.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmPersonAbilityTable.GetTableName()].Merge(AInspectDS.PmPersonAbility);
                    }

                    if (AInspectDS.Tables.Contains(PmPassportDetailsTable.GetTableName())
                        && (AInspectDS.PmPassportDetails.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmPassportDetailsTable.GetTableName()].Merge(AInspectDS.PmPassportDetails);
                    }

                    if (AInspectDS.Tables.Contains(PmPersonalDataTable.GetTableName())
                        && (AInspectDS.PmPersonalData.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmPersonalDataTable.GetTableName()].Merge(AInspectDS.PmPersonalData);
                    }

                    if (AInspectDS.Tables.Contains(PmPersonLanguageTable.GetTableName())
                        && (AInspectDS.PmPersonLanguage.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmPersonLanguageTable.GetTableName()].Merge(AInspectDS.PmPersonLanguage);
                    }

                    if (AInspectDS.Tables.Contains(PmPersonEvaluationTable.GetTableName())
                        && (AInspectDS.PmPersonEvaluation.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmPersonEvaluationTable.GetTableName()].Merge(AInspectDS.PmPersonEvaluation);
                    }

                    if (AInspectDS.Tables.Contains(PmStaffDataTable.GetTableName())
                        && (AInspectDS.PmStaffData.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmStaffDataTable.GetTableName()].Merge(AInspectDS.PmStaffData);
                    }

                    if (AInspectDS.Tables.Contains(PmPersonSkillTable.GetTableName())
                        && (AInspectDS.PmPersonSkill.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmPersonSkillTable.GetTableName()].Merge(AInspectDS.PmPersonSkill);
                    }

                    if (AInspectDS.Tables.Contains(PmPastExperienceTable.GetTableName())
                        && (AInspectDS.PmPastExperience.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmPastExperienceTable.GetTableName()].Merge(AInspectDS.PmPastExperience);
                    }

                    if (AInspectDS.Tables.Contains(PmDocumentTable.GetTableName())
                        && (AInspectDS.PmDocument.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmDocumentTable.GetTableName()].Merge(AInspectDS.PmDocument);
                    }

                    if (AInspectDS.Tables.Contains(PmJobAssignmentTable.GetTableName())
                        && (AInspectDS.PmJobAssignment.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmJobAssignmentTable.GetTableName()].Merge(AInspectDS.PmJobAssignment);
                    }

                    if (AInspectDS.Tables.Contains(PmGeneralApplicationTable.GetTableName())
                        && (AInspectDS.PmGeneralApplication.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmGeneralApplicationTable.GetTableName()].Merge(AInspectDS.PmGeneralApplication);
                    }

                    if (AInspectDS.Tables.Contains(PmShortTermApplicationTable.GetTableName())
                        && (AInspectDS.PmShortTermApplication.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmShortTermApplicationTable.GetTableName()].Merge(AInspectDS.PmShortTermApplication);
                    }

                    if (AInspectDS.Tables.Contains(PmYearProgramApplicationTable.GetTableName())
                        && (AInspectDS.PmYearProgramApplication.Rows.Count > 0))
                    {
                        APartnerEditInspectDS.Tables[PmYearProgramApplicationTable.GetTableName()].Merge(AInspectDS.PmYearProgramApplication);
                    }
                }
            }
            else
            {
                TLogging.LogAtLevel(8, "TIndividualDataWebConnector.SubmitChangesServerSide: AInspectDS = nil!");
                SubmissionResult = TSubmitChangesResult.scrNothingToBeSaved;
            }

            return SubmissionResult;
        }
    }
}