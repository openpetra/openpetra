//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2016 by OM International
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
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB; // Implicit reference
using Ict.Petra.Server.MCommon.Cacheable;
using Ict.Petra.Server.MPartner.Partner.Cacheable;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MReporting;
using Ict.Petra.Server.MReporting.MPartner;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MReporting; // Implicit reference


namespace Ict.Petra.Server.MReporting.MPersonnel
{
    /// <summary>
    /// These are the specific functions for the Personnel module,
    /// that are needed for report generation.
    /// </summary>
    public class TRptUserFunctionsPersonnel : TRptUserFunctions
    {
        private static PmSpecialNeedRow FCachedSpecialNeedRow;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptUserFunctionsPersonnel() : base()
        {
        }

        /// <summary>
        /// functions need to be registered here
        /// </summary>
        /// <param name="ASituation"></param>
        /// <param name="f"></param>
        /// <param name="ops"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override Boolean FunctionSelector(TRptSituation ASituation, String f, TVariant[] ops, out TVariant value)
        {
            if (base.FunctionSelector(ASituation, f, ops, out value))
            {
                return true;
            }

            if (StringHelper.IsSame(f, "GetSiteName"))
            {
                value = new TVariant(GetSiteName());
                return true;
            }

            if (StringHelper.IsSame(f, "GetCurrentCommitmentPeriod"))
            {
                value = new TVariant(GetCurrentCommitmentPeriod(ops[1].ToInt64(), ops[2].ToDate()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetType"))
            {
                value = new TVariant(GetType(ops[1].ToInt64(), ops[2].ToString(), ops[3].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "GenerateUnitHierarchy"))
            {
                value = new TVariant(GenerateUnitHierarchy(ops[1].ToInt64(), ops[2].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetMissingInfo"))
            {
                value = new TVariant(GetMissingInfo(ops[1].ToInt64(), ops[2].ToInt(), ops[3].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetPersonLanguages"))
            {
                value = new TVariant(GetPersonLanguages(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetPassport"))
            {
                value = new TVariant(GetPassport(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetNationalities"))
            {
                value = new TVariant(GetNationalities(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetChurch"))
            {
                value = new TVariant(GetChurch(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetDietary"))
            {
                value = new TVariant(GetDietary(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetMedicalInfo"))
            {
                value = new TVariant(GetMedicalInfo(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetOtherNeeds"))
            {
                value = new TVariant(GetOtherNeeds(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetPartnerContact"))
            {
                value = new TVariant(GetPartnerContact(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "CalculateAge"))
            {
                value = new TVariant(CalculateAge(ops[1].ToDate()));
                return true;
            }

            if (StringHelper.IsSame(f, "CalculateAgeAtDate"))
            {
                value = new TVariant(CalculateAgeAtDate(ops[1].ToDate(), ops[2].ToDate()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetArrivalPoint"))
            {
                value = new TVariant(GetArrivalPoint(ops[1].ToString()));
                return true;
            }

            /*
             * if (isSame(f, 'doSomething')) then
             * begin
             * value := new TVariant();
             * doSomething(ops[1].ToInt(), ops[2].ToString(), ops[3].ToString());
             * exit;
             * end;
             */
            value = new TVariant();
            return false;
        }

        /// <summary>
        /// returns the site name of the current site key,
        /// which is stored in s_system_parameter
        ///
        /// </summary>
        /// <returns>void</returns>
        private String GetSiteName()
        {
            String ReturnValue = "";
            string strSql;
            DataTable tab;
            long SiteKey = -1;
            PPartnerTable PartnerTable;

            strSql = "SELECT PUB_s_system_defaults.s_default_value_c " + "FROM PUB_s_system_defaults " +
                     "WHERE PUB_s_system_defaults.s_default_code_c = 'SiteKey'";

            tab = situation.GetDatabaseConnection().SelectDT(strSql, "table", situation.GetDatabaseConnection().Transaction);

            if (tab.Rows.Count > 0)
            {
                String SiteKeyString = Convert.ToString(tab.Rows[0]["s_default_value_c"]);
                try
                {
                    SiteKey = Convert.ToInt64(SiteKeyString);
                }
                catch (Exception)
                {
                    SiteKey = -1;
                }
            }

            PartnerTable = PPartnerAccess.LoadByPrimaryKey(SiteKey, situation.GetDatabaseConnection().Transaction);

            if (PartnerTable.Rows.Count > 0)
            {
                ReturnValue = (String)PartnerTable.Rows[0][PPartnerTable.GetPartnerShortNameDBName()];
            }

            return ReturnValue;
        }

        /// <summary>
        /// This functions finds the commitment period of a given partner, at a given time
        /// The result is stored in the variables CommitmentStart
        /// and CommitmentEnd. The end date might be empty, even if the start date is set
        ///
        /// It will find the most recent commitment,
        /// that starts on or before the given date and
        /// lasts till or beyond the given date (also open ended).
        /// If no such commitment exists, the most recent commitment of all will be returned.
        ///
        /// </summary>
        /// <returns>s true if a current commitment period was found
        /// </returns>
        private bool GetCurrentCommitmentPeriod(Int64 APartnerKey, DateTime AGivenDate)
        {
            bool ReturnValue;
            string strSql;
            DataTable tab;
            TRptFormatQuery formatQuery;

            System.Object StartDate = DateTime.MinValue;
            System.Object EndDate = DateTime.MinValue;
            ReturnValue = false;
            List<OdbcParameter> odbcparameters = new List<OdbcParameter>();
            odbcparameters .Add(new OdbcParameter("partnerKey", OdbcType.BigInt) { Value = APartnerKey });
            odbcparameters .Add(new OdbcParameter("startOfCommitment", OdbcType.DateTime) { Value = AGivenDate });
            odbcparameters .Add(new OdbcParameter("endOfCommitment", OdbcType.DateTime) { Value = AGivenDate });
            strSql = "SELECT pm_start_of_commitment_d, pm_end_of_commitment_d " + "FROM PUB_pm_staff_data " +
                     "WHERE PUB_pm_staff_data.p_partner_key_n = ? AND pm_start_of_commitment_d <= ? " +
                     "AND (pm_end_of_commitment_d >= ? OR pm_end_of_commitment_d IS NULL) " +
                     "ORDER BY pm_start_of_commitment_d ASC";
            formatQuery = new TRptFormatQuery(strSql, odbcparameters, null, -1, -1);
            formatQuery.ReplaceVariables();
            tab = situation.GetDatabaseConnection().SelectDT(formatQuery.SQLStmt, "table", situation.GetDatabaseConnection().Transaction, formatQuery.OdbcParameters.ToArray());
            formatQuery = null;

            if (tab.Rows.Count > 0)
            {
                // take the last row, the most recent start date
                ReturnValue = true;
                StartDate = tab.Rows[tab.Rows.Count - 1]["pm_start_of_commitment_d"];
                EndDate = tab.Rows[tab.Rows.Count - 1]["pm_end_of_commitment_d"];
            }
            else
            {
                // no commitment period for the given date was found, so find the most recent commitment
                strSql = "SELECT pm_start_of_commitment_d, pm_end_of_commitment_d " + "FROM PUB_pm_staff_data " +
                         "WHERE PUB_pm_staff_data.p_partner_key_n = " + APartnerKey.ToString() + ' ' + "ORDER BY pm_start_of_commitment_d ASC";
                tab = situation.GetDatabaseConnection().SelectDT(strSql, "table", situation.GetDatabaseConnection().Transaction);

                if (tab.Rows.Count > 0)
                {
                    // take the last row, the most recent start date
                    ReturnValue = true;
                    StartDate = tab.Rows[tab.Rows.Count - 1]["pm_start_of_commitment_d"];
                    EndDate = tab.Rows[tab.Rows.Count - 1]["pm_end_of_commitment_d"];
                }
            }

            if (!ReturnValue)
            {
                situation.GetParameters().RemoveVariable("CommitmentStart", -1, -1, eParameterFit.eExact);
                situation.GetParameters().RemoveVariable("CommitmentEnd", -1, -1, eParameterFit.eExact);
            }
            else
            {
                situation.GetParameters().Add("CommitmentStart", new TVariant(StartDate), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                situation.GetParameters().Add("CommitmentEnd", new TVariant(EndDate), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the p_partner_type.p_type_code_c of a partner if the type code matches
        /// with one of the items in ATypeList.
        /// AMatch defines how the type has to match. Possible values are BEGIN or EXACT
        /// </summary>
        /// <param name="APartnerKey">Partner Key of the partner</param>
        /// <param name="ATypeList">List of items seperated by ;</param>
        /// <param name="AMatch">defines how the Partner Type must match</param>
        /// <returns>returns the type code or an empty string</returns>
        private String GetType(Int64 APartnerKey, String ATypeList, String AMatch)
        {
            if (ATypeList == "DEFAULTWORKERTYPES")
            {
                // allows ORGANIZATION SPECIFIC types
                ATypeList = TAppSettingsManager.GetValue("DEFAULTWORKERTYPES_STARTINGWITH", "WORKER;EX-WORKER;ASSOC", false);
            }

            PPartnerTypeTable PartnerType;

            String[] TypeList;

            if (ATypeList.Contains(";"))
            {
                TypeList = ATypeList.Split(';');
            }
            else
            {
                TypeList = ATypeList.Split(',');
            }

            int MatchingPattern = 0;

            if (AMatch == "BEGIN")
            {
                MatchingPattern = 1;
            }
            else if (AMatch == "EXACT")
            {
                MatchingPattern = 2;
            }

            PartnerType = PPartnerTypeAccess.LoadViaPPartner(APartnerKey, situation.GetDatabaseConnection().Transaction);

            foreach (PPartnerTypeRow Row in PartnerType.Rows)
            {
                foreach (String CurrentType in TypeList)
                {
                    switch (MatchingPattern)
                    {
                        case 1:

                            if (Row.TypeCode.StartsWith(CurrentType))
                            {
                                return Row.TypeCode;
                            }

                            break;

                        case 2:

                            if (Row.TypeCode == CurrentType)
                            {
                                return Row.TypeCode;
                            }

                            break;

                        default:
                            break;
                    }
                }
            }

            return "";
        }

        #region Calculation for unit hierarchy report
        /// <summary>
        /// Get recursively all the child units of a unit and puts them into the
        /// results list.
        /// This function is called by the "UnitHierarchyReport"
        /// </summary>
        /// <param name="AUnitKey">Parent unit to get the child unit from</param>
        /// <param name="AWithOutreaches">Indicates if outreaches and conferences should
        /// be included in the result.</param>
        /// <returns>True</returns>
        private bool GenerateUnitHierarchy(long AUnitKey, string AWithOutreaches)
        {
            int ChildRow = 1;

            // stores the child units into the situation results
            GetChildUnits(AUnitKey, 0, (AWithOutreaches == "true"), ref ChildRow);

            return true;
        }

        /// <summary>
        /// Get recursively all the child units of a unit and puts them into the
        /// results list.
        /// </summary>
        /// <param name="AUnitKey">Parent unit to get the child unit from</param>
        /// <param name="AChildLevel">Indicates how deep we are in the recursion</param>
        /// <param name="AWithOutreaches">Indicates if outreaches and conferences should
        /// be included in the result</param>
        /// <param name="AChildRow">the number of the row</param>
        /// <returns>False if the parent unit is not active.
        /// Otherwise true</returns>
        private bool GetChildUnits(long AUnitKey, int AChildLevel, bool AWithOutreaches, ref int AChildRow)
        {
            UmUnitStructureTable UnitStructure;
            PUnitTable UnitTable;
            PPartnerTable PartnerTable;
            UUnitTypeTable UnitType;

            PartnerTable = PPartnerAccess.LoadByPrimaryKey(AUnitKey, situation.GetDatabaseConnection().Transaction);

            if ((PartnerTable.Rows.Count > 0)
                && (((PPartnerRow)PartnerTable.Rows[0]).StatusCode != "ACTIVE"))
            {
                return false;
            }

            string PreceedingWhiteSpaces = new string(' ', AChildLevel * 2);

            UnitStructure = UmUnitStructureAccess.LoadViaPUnitParentUnitKey(AUnitKey, situation.GetDatabaseConnection().Transaction);

            // Add this unit to the results
            UnitTable = PUnitAccess.LoadByPrimaryKey(AUnitKey, situation.GetDatabaseConnection().Transaction);

            if (UnitTable.Rows.Count > 0)
            {
                PUnitRow UnitRow = (PUnitRow)UnitTable.Rows[0];

                string UnitTypeName = UnitRow.UnitTypeCode;

                UnitType = UUnitTypeAccess.LoadByPrimaryKey(UnitRow.UnitTypeCode, situation.GetDatabaseConnection().Transaction);

                if (UnitType.Rows.Count > 0)
                {
                    UnitTypeName = ((UUnitTypeRow)UnitType.Rows[0]).UnitTypeName;
                }

                string UnitKeyString = FormatAsUnitKey(UnitRow.PartnerKey);

                TVariant[] Header =
                {
                    new TVariant(), new TVariant(), new TVariant(), new TVariant()
                };
                TVariant[] Description =
                {
                    new TVariant(), new TVariant()
                };
                TVariant[] Columns =
                {
                    new TVariant(PreceedingWhiteSpaces + UnitKeyString),
                    new TVariant(PreceedingWhiteSpaces + UnitTypeName),
                    new TVariant(PreceedingWhiteSpaces + UnitRow.UnitName),
                    new TVariant(UnitRow.PartnerKey)
                };

                situation.GetResults().AddRow(0, AChildRow++, true, 1, "", "", false,
                    Header, Description, Columns);
            }

            //
            // Add the children to the results
            //

            SortedList <string, long>ChildList = new SortedList <string, long>();
            AChildLevel++;

            foreach (DataRow Row in UnitStructure.Rows)
            {
                // Add the name and the key into a sorted list
                // so we can sort the result alphabetically
                long ChildUnitKey = (long)Row[UmUnitStructureTable.GetChildUnitKeyDBName()];

                if (ChildUnitKey == AUnitKey)
                {
                    continue;
                }

                UnitTable = PUnitAccess.LoadByPrimaryKey(ChildUnitKey, situation.GetDatabaseConnection().Transaction);

                if (UnitTable.Rows.Count < 1)
                {
                    continue;
                }

                PUnitRow UnitRow = (PUnitRow)UnitTable.Rows[0];

                string UnitName = UnitRow.UnitName;
                string UnitTypeName = UnitRow.UnitTypeCode;

                if (!AWithOutreaches
                    && ((UnitTypeName.StartsWith("GA"))
                        || (UnitTypeName.StartsWith("GC"))
                        || (UnitTypeName.StartsWith("TN"))
                        || (UnitTypeName.StartsWith("TS"))))
                {
                    continue;
                }

                // use as key UnitName (for sorting) plus UnitKey so that it is
                // unique. We might have two units with the same name
                ChildList.Add(UnitName + ChildUnitKey.ToString(), ChildUnitKey);
            }

            foreach (KeyValuePair <string, long>kvp in ChildList)
            {
                GetChildUnits(kvp.Value, AChildLevel, AWithOutreaches, ref AChildRow);
            }

            return true;
        }

        /// <summary>
        /// Formats a integer to a 10 character long string to represent a partner key.
        /// </summary>
        /// <param name="AUnitKey"></param>
        /// <returns>string with 10 characters</returns>
        private string FormatAsUnitKey(long AUnitKey)
        {
            string UnitKeyString = AUnitKey.ToString();
            string ReturnValue = "";

            switch (UnitKeyString.Length)
            {
                case 1:
                    ReturnValue = "000000000" + UnitKeyString;
                    break;

                case 2:
                    ReturnValue = "00000000" + UnitKeyString;
                    break;

                case 3:
                    ReturnValue = "0000000" + UnitKeyString;
                    break;

                case 4:
                    ReturnValue = "000000" + UnitKeyString;
                    break;

                case 5:
                    ReturnValue = "00000" + UnitKeyString;
                    break;

                case 6:
                    ReturnValue = "0000" + UnitKeyString;
                    break;

                case 7:
                    ReturnValue = "000" + UnitKeyString;
                    break;

                case 8:
                    ReturnValue = "00" + UnitKeyString;
                    break;

                case 9:
                    ReturnValue = "0" + UnitKeyString;
                    break;

                case 10:
                    ReturnValue = UnitKeyString;
                    break;

                default:
                    ReturnValue = "0000000000";
                    break;
            }

            return ReturnValue;
        }

        #endregion
        /// <summary>
        /// Get the missing information of a short term application partner.
        /// This could be Passport, Date of Birth, Gender, Mother Tongue, Emergency Contact, Event, Travel information
        /// </summary>
        /// <param name="APartnerKey">Partner Key</param>
        /// <param name="AApplicationKey">Application Key</param>
        /// <param name="ARegistrationOffice">Registration Office</param>
        /// <returns>String of all the missing informations for this partner and application</returns>
        private String GetMissingInfo(Int64 APartnerKey, int AApplicationKey, Int64 ARegistrationOffice)
        {
            String MissingInfo = "";
            PmPassportDetailsTable PassportTable;
            PPersonTable PersonTable;
            PPartnerTable PartnerTable;
            PPartnerRelationshipTable PartnerRelationshipTable;
            PmShortTermApplicationTable ShortTermApplicationTable;

            // Check for passport Details
            PassportTable = PmPassportDetailsAccess.LoadViaPPerson(APartnerKey, situation.GetDatabaseConnection().Transaction);
            bool PassportDetailMissing = true;

            for (int Counter = 0; Counter < PassportTable.Rows.Count; ++Counter)
            {
                PmPassportDetailsRow row = (PmPassportDetailsRow)PassportTable.Rows[Counter];

                if (row.FullPassportName.Length > 0)
                {
                    PassportDetailMissing = false;
                }
            }

            if (PassportDetailMissing)
            {
                MissingInfo += " Passport Details,";
            }

            // Check for Date of Birth and Gender
            PersonTable = PPersonAccess.LoadByPrimaryKey(APartnerKey, situation.GetDatabaseConnection().Transaction);

            if (PassportTable.Rows.Count == 0)
            {
                MissingInfo += " Date of Birth, Gender,";
            }
            else
            {
                PPersonRow PersonRow = (PPersonRow)PersonTable.Rows[0];

                if (PersonRow.IsDateOfBirthNull())
                {
                    MissingInfo += " Date of Birth,";
                }

                if (PersonRow.Gender == "Unknown")
                {
                    MissingInfo += " Gender,";
                }
            }

            // Check for mother tongue
            PartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, situation.GetDatabaseConnection().Transaction);

            if (PassportTable.Rows.Count == 0)
            {
                MissingInfo += " Mother Tongue,";
            }
            else if (((PPartnerRow)PartnerTable.Rows[0]).LanguageCode == "99")
            {
                MissingInfo += " Mother Tongue,";
            }

            // Check for partner relationship
            PartnerRelationshipTable = PPartnerRelationshipAccess.LoadViaPPartnerRelationKey(APartnerKey,
                situation.GetDatabaseConnection().Transaction);

            bool HasEmergencyContact = false;

            for (int Counter = 0; Counter < PartnerRelationshipTable.Rows.Count; ++Counter)
            {
                PPartnerRelationshipRow Row = (PPartnerRelationshipRow)PartnerRelationshipTable.Rows[Counter];

                if (Row.PartnerKey == 0)
                {
                    continue;
                }

                if ((Row.RelationName == "PAREND")
                    || (Row.RelationName == "GUARDIAN")
                    || (Row.RelationName == "RELATIVE")
                    || (Row.RelationName == "EMER-1")
                    || (Row.RelationName == "EMER-2")
                    || (Row.RelationName == "NOK-OTHER"))
                {
                    HasEmergencyContact = true;
                    break;
                }
            }

            if (!HasEmergencyContact)
            {
                MissingInfo += " Emergency Contact,";
            }

            // Check for Event and Travel information
            ShortTermApplicationTable = PmShortTermApplicationAccess.LoadByPrimaryKey(APartnerKey,
                AApplicationKey, ARegistrationOffice, situation.GetDatabaseConnection().Transaction);

            bool HasEvent = false;
            bool HasTravelInfo = false;

            for (int Counter = 0; Counter < ShortTermApplicationTable.Rows.Count; ++Counter)
            {
                PmShortTermApplicationRow Row = (PmShortTermApplicationRow)ShortTermApplicationTable.Rows[Counter];

                if (Row.ConfirmedOptionCode != "")
                {
                    HasEvent = true;
                }

                if ((!Row.IsArrivalNull())
                    && (!Row.IsDepartureNull()))
                {
                    HasTravelInfo = true;
                }
            }

            if (!HasEvent)
            {
                MissingInfo += " Event,";
            }

            if (!HasTravelInfo)
            {
                MissingInfo += "Travel Information,";
            }

            // remove the last ,
            if (MissingInfo.Length > 0)
            {
                MissingInfo.Remove(MissingInfo.Length - 1);
            }

            return MissingInfo;
        }

        /// <summary>
        /// Gets all the languages of the person
        /// </summary>
        /// <param name="APartnerKey">Partner Key</param>
        /// <returns></returns>
        private String GetPersonLanguages(Int64 APartnerKey)
        {
            PmPersonLanguageTable LanguageTable;
            PPartnerTable PartnerTable;
            String SpokenLanguages = "";
            String MotherLanguageCode = "99";
            bool MotherTongeInList = false;

            PartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, situation.GetDatabaseConnection().Transaction);

            if (PartnerTable.Rows.Count > 0)
            {
                MotherLanguageCode = ((PPartnerRow)PartnerTable.Rows[0]).LanguageCode;
            }

            LanguageTable = PmPersonLanguageAccess.LoadViaPPerson(APartnerKey, situation.GetDatabaseConnection().Transaction);

            for (int Counter = 0; Counter < LanguageTable.Rows.Count; ++Counter)
            {
                PmPersonLanguageRow Row = (PmPersonLanguageRow)LanguageTable.Rows[Counter];

                if (Row.LanguageCode == MotherLanguageCode)
                {
                    SpokenLanguages += MotherLanguageCode + " (M";
                    MotherTongeInList = true;
                }
                else
                {
                    SpokenLanguages += Row.LanguageCode + " (" + Row.LanguageLevel;
                }

                SpokenLanguages += "), ";
            }

            if (!MotherTongeInList)
            {
                // Insert mother tongue at the first place:
                SpokenLanguages = MotherLanguageCode + " (M), " + SpokenLanguages;
            }

            // remove the last comma and space
            if (SpokenLanguages.Length > 2)
            {
                SpokenLanguages.Remove(SpokenLanguages.Length - 2);
            }

            return SpokenLanguages;
        }

        /// <summary>
        /// Get the passport details and estores them as parameters.
        /// If there is a passport with the MainPassport flag set, then use this passport.
        /// Otherwise use the most recent passport which has a passport number.
        /// </summary>
        /// <param name="APartnerKey">Partner key</param>
        /// <returns>true if one passport was found, otherwise false</returns>
        private bool GetPassport(Int64 APartnerKey)
        {
            PmPassportDetailsTable PassportTable = new PmPassportDetailsTable();

            PmPassportDetailsRow ResultPassportRow = GetLatestPassport(APartnerKey, situation);

            if (ResultPassportRow != null)
            {
                // add the results to the parameters
                foreach (DataColumn col in PassportTable.Columns)
                {
                    situation.GetParameters().Add(StringHelper.UpperCamelCase(col.ColumnName, true,
                            true), new TVariant(ResultPassportRow[col.ColumnName]));
                }
            }
            else
            {
                // add empty results to the parameters.
                // Otherwise the old rsults from the previous calculations will be used.
                foreach (DataColumn col in PassportTable.Columns)
                {
                    situation.GetParameters().Add(StringHelper.UpperCamelCase(col.ColumnName, true,
                            true), new TVariant(""));
                }
            }

            return ResultPassportRow != null;
        }

        /// <summary>
        /// Gets nationalities from all of a Partner's recorded passports
        /// </summary>
        /// <param name="APartnerKey">Partner key</param>
        /// <returns>returns nationalities in a comma seperated string</returns>
        private string GetNationalities(Int64 APartnerKey)
        {
            TCacheable CommonCacheable = new TCacheable();

            StringCollection PassportColumns = StringHelper.StrSplit(
                PmPassportDetailsTable.GetDateOfIssueDBName() + "," +
                PmPassportDetailsTable.GetDateOfExpirationDBName() + "," +
                PmPassportDetailsTable.GetPassportNationalityCodeDBName() + "," +
                PmPassportDetailsTable.GetMainPassportDBName(), ",");

            PmPassportDetailsTable PassportDetailsDT = PmPassportDetailsAccess.LoadViaPPerson(APartnerKey,
                PassportColumns, situation.GetDatabaseConnection().Transaction, null, 0, 0);

            return Ict.Petra.Shared.MPersonnel.Calculations.DeterminePersonsNationalities(
                @CommonCacheable.GetCacheableTable, PassportDetailsDT);
        }

        /// <summary>
        /// Add the address details of the supporting church of a partner to the results
        /// </summary>
        /// <param name="APartnerKey">The partner key of whom the supporting church details should be added</param>
        /// <returns></returns>
        private bool GetChurch(Int64 APartnerKey)
        {
            PPartnerRelationshipTable RelationshipTable;
            PPartnerTable ChurchTable;
            string PhoneNumber;
            string EmailAddress;

            Dictionary <String, String>GatheredResults = new Dictionary <String, String>();

            PPartnerRelationshipRow TemplateRow = new PPartnerRelationshipTable().NewRowTyped(false);

            TemplateRow.RelationKey = APartnerKey;
            TemplateRow.RelationName = "SUPPCHURCH";

            RelationshipTable = PPartnerRelationshipAccess.LoadUsingTemplate(TemplateRow, situation.GetDatabaseConnection().Transaction);

            bool IsFirstAddress = true;

            foreach (PPartnerRelationshipRow Row in RelationshipTable.Rows)
            {
                ChurchTable = PPartnerAccess.LoadByPrimaryKey(Row.PartnerKey, situation.GetDatabaseConnection().Transaction);

                if (ChurchTable.Rows.Count < 1)
                {
                    continue;
                }

                PPartnerLocationRow PartnerLocationRow;
                PLocationTable LocationTable;

                if (!TRptUserFunctionsPartner.GetPartnerBestAddressRow(Row.PartnerKey, situation, out PartnerLocationRow))
                {
                    continue;
                }

                LocationTable = PLocationAccess.LoadByPrimaryKey(PartnerLocationRow.SiteKey,
                    PartnerLocationRow.LocationKey, situation.GetDatabaseConnection().Transaction);

                if (LocationTable.Rows.Count < 1)
                {
                    continue;
                }

                if (IsFirstAddress)
                {
                    GatheredResults.Add("Church-Name", ((PPartnerRow)ChurchTable.Rows[0]).PartnerShortName);
                }
                else
                {
                    GatheredResults["Church-Name"] += ", " + ((PPartnerRow)ChurchTable.Rows[0]).PartnerShortName + " ";
                }

                // Add this church address to the results
                // the variables will be something like Church-PostalCode, Church-StreetName

                // get the location details into the parameters
                foreach (DataColumn col in LocationTable.Columns)
                {
                    if (IsFirstAddress)
                    {
                        GatheredResults.Add("Church-" + StringHelper.UpperCamelCase(col.ColumnName, true, true),
                            LocationTable.Rows[0][col.ColumnName].ToString());
                    }
                    else
                    {
                        GatheredResults["Church-" + StringHelper.UpperCamelCase(col.ColumnName, true, true)] +=
                            ", " + LocationTable.Rows[0][col.ColumnName].ToString();
                    }
                }

                if (IsFirstAddress)
                {
                    // also put the phone number and email etc into the parameters
                    TContactDetailsAggregate.GetPrimaryEmailAndPrimaryPhone(Row.PartnerKey,
                        out PhoneNumber, out EmailAddress);

                    // Add Calculation Parameter for 'Primary Email Address' (String.Empty is supplied if the Partner hasn't got one)
                    situation.GetParameters().AddCalculationParameter("Church-EmailAddress",
                        new TVariant(EmailAddress ?? String.Empty));

                    // Add Calculation Parameter for 'Primary Phone Number' (String.Empty is supplied if the Partner hasn't got one)
                    situation.GetParameters().AddCalculationParameter("Church-Telephone",
                        new TVariant(PhoneNumber ?? String.Empty));

                    // At present we no longer support the reporting of the following, so we set those Calculation Parameters to String.Empty
                    GatheredResults.Add("Church-FaxNumber", String.Empty);
                    GatheredResults.Add("Church-MobileNumber", String.Empty);
                    GatheredResults.Add("Church-AlternateTelephone", String.Empty);
                }

                IsFirstAddress = false;
            }

            if (IsFirstAddress)
            {
                situation.GetParameters().RemoveVariable("Church-Telephone");
                situation.GetParameters().RemoveVariable("Church-FaxNumber");
                situation.GetParameters().RemoveVariable("Church-EmailAddress");
                situation.GetParameters().RemoveVariable("Church-MobileNumber");
                situation.GetParameters().RemoveVariable("Church-AlternateTelephone");
                situation.GetParameters().RemoveVariable("Church-Name");
                situation.GetParameters().RemoveVariable("Church-Locality");
                situation.GetParameters().RemoveVariable("Church-Address3");
                situation.GetParameters().RemoveVariable("Church-City");
                situation.GetParameters().RemoveVariable("Church-CountryCode");
                situation.GetParameters().RemoveVariable("Church-County");
                situation.GetParameters().RemoveVariable("Church-PostalCode");
                situation.GetParameters().RemoveVariable("Church-StreetName");
            }
            else
            {
                foreach (KeyValuePair <String, String>kvp in GatheredResults)
                {
                    situation.GetParameters().Add(kvp.Key, new TVariant(kvp.Value));
                }
            }

            return true;
        }

        private String GetDietary(Int64 APartnerKey)
        {
            String Dietary = "";

            if (RefreshCachedSpecialNeedRow(APartnerKey))
            {
                Dietary = FCachedSpecialNeedRow.DietaryComment;

                if (FCachedSpecialNeedRow.VegetarianFlag)
                {
                    Dietary += Catalog.GetString("Vegetarian");
                }
            }

            return Dietary;
        }

        private String GetMedicalInfo(Int64 APartnerKey)
        {
            String MedicalInfo = "";

            if (RefreshCachedSpecialNeedRow(APartnerKey))
            {
                MedicalInfo = FCachedSpecialNeedRow.MedicalComment;
            }

            return MedicalInfo;
        }

        private String GetOtherNeeds(Int64 APartnerKey)
        {
            String OtherNeeds = "";

            if (RefreshCachedSpecialNeedRow(APartnerKey))
            {
                OtherNeeds = FCachedSpecialNeedRow.OtherSpecialNeed;
            }

            return OtherNeeds;
        }

        /// <summary>
        /// Refreshes the cached special need row for a partner if it needs to.
        /// </summary>
        /// <param name="APartnerKey">Partner key for the special need</param>
        /// <returns>true if the cached row matches the partner key, otherwise false</returns>
        private bool RefreshCachedSpecialNeedRow(Int64 APartnerKey)
        {
            bool ReturnValue = false;

            if ((FCachedSpecialNeedRow == null)
                || (FCachedSpecialNeedRow.PartnerKey != APartnerKey))
            {
                PmSpecialNeedTable SpecialNeedsTable;

                SpecialNeedsTable = PmSpecialNeedAccess.LoadByPrimaryKey(APartnerKey, situation.GetDatabaseConnection().Transaction);

                if (SpecialNeedsTable.Rows.Count > 0)
                {
                    FCachedSpecialNeedRow = (PmSpecialNeedRow)SpecialNeedsTable.Rows[0];
                    ReturnValue = true;
                }
            }
            else if (FCachedSpecialNeedRow.PartnerKey == APartnerKey)
            {
                ReturnValue = true;
            }

            return ReturnValue;
        }

        private String GetPartnerContact(Int64 APartnerKey)
        {
            String PartnerContact = "";
            PPartnerTable PartnerTable;

            PartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, situation.GetDatabaseConnection().Transaction);

            if (PartnerTable.Rows.Count > 0)
            {
                PPartnerRow Row = (PPartnerRow)PartnerTable.Rows[0];

                if (Row.PartnerKey != 0)
                {
                    PartnerContact = "Contact: " + Row.PartnerKey.ToString() + " " + Row.PartnerShortName;
                }
            }

            return PartnerContact;
        }

        /// <summary>
        /// Calculates the current age in years from a given date
        /// </summary>
        /// <param name="ABirthday">date to calculate the age</param>
        /// <returns>The calculated age as a string</returns>
        private String CalculateAge(DateTime ABirthday)
        {
            return Ict.Petra.Shared.MPartner.Calculations.CalculateAge(ABirthday).ToString();
        }

        /// <summary>
        /// Calculates the age in years at a given date
        /// </summary>
        /// <param name="ABirthday">date to calculate the age</param>
        /// <param name="ATestDate">date from when to calculate the age</param>
        /// <returns>The calculated age as a string</returns>
        private String CalculateAgeAtDate(DateTime ABirthday, DateTime ATestDate)
        {
            return Ict.Petra.Shared.MPartner.Calculations.CalculateAge(ABirthday, ATestDate).ToString();
        }

        /// <summary>
        /// Retrieves the full description of an arrival point.
        /// </summary>
        /// <param name="AArrivalPointCode">Short Code of the arrival point</param>
        /// <returns>The description of the arrival point.</returns>
        private String GetArrivalPoint(String AArrivalPointCode)
        {
            String ReturnValue = "";

            PtArrivalPointTable ArrivalTable;

            ArrivalTable = PtArrivalPointAccess.LoadByPrimaryKey(AArrivalPointCode, situation.GetDatabaseConnection().Transaction);

            if (ArrivalTable.Rows.Count > 0)
            {
                ReturnValue = ((PtArrivalPointRow)ArrivalTable.Rows[0]).Description;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Get the passport details and restores them as parameters.
        /// If there is a passport with the MainPassport flag set, then use this passport.
        /// Otherwise use the most recent passport which has a passport number.
        /// </summary>
        /// <param name="APartnerKey">Partner key</param>
        /// <param name="ASituation">A current Report Situation</param>
        /// <returns>true if one passport was found, otherwise false</returns>
        public static PmPassportDetailsRow GetLatestPassport(Int64 APartnerKey, TRptSituation ASituation)
        {
            PmPassportDetailsTable PassportTable = null;
            PmPassportDetailsRow ResultPassportRow = null;

            StringCollection PassportCollumns = new StringCollection();
            StringCollection OrderList = new StringCollection();

            PassportCollumns.Add(PmPassportDetailsTable.GetPassportNationalityCodeDBName());
            PassportCollumns.Add(PmPassportDetailsTable.GetPassportNumberDBName());
            PassportCollumns.Add(PmPassportDetailsTable.GetDateOfExpirationDBName());
            PassportCollumns.Add(PmPassportDetailsTable.GetFullPassportNameDBName());
            OrderList.Add("ORDER BY " + PmPassportDetailsTable.GetDateOfExpirationDBName() + " DESC");

            PassportTable = PmPassportDetailsAccess.LoadViaPPerson(APartnerKey,
                PassportCollumns, ASituation.GetDatabaseConnection().Transaction,
                OrderList, 0, 0);

            // Look for MainPassport flag
            foreach (PmPassportDetailsRow Row in PassportTable.Rows)
            {
                if (!Row.IsMainPassportNull()
                    && Row.MainPassport)
                {
                    ResultPassportRow = Row;
                    break;
                }
            }

            // Look for the most recent passport with a passport number
            if (ResultPassportRow == null)
            {
                foreach (PmPassportDetailsRow Row in PassportTable.Rows)
                {
                    if (Row.PassportNumber.Length > 0)
                    {
                        ResultPassportRow = Row;
                        break;
                    }
                }
            }

            return ResultPassportRow;
        }
    }
}