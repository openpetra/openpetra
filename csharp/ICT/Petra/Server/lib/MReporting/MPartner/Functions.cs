//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2017 by OM International
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


using Ict.Common;
using Ict.Common.Conversion;
using Ict.Common.Data; // Implicit reference
using Ict.Common.DB;
using Ict.Common.IO; // Implicit reference
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MCommon.queries;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Cacheable;
using Ict.Petra.Server.MReporting;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MReporting;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Odbc;
using System.Data;
using System.Diagnostics;
using Ict.Petra.Server.MPartner.Partner.UIConnectors;
using Ict.Petra.Server.MPartner.DataAggregates;

namespace Ict.Petra.Server.MReporting.MPartner
{
    /// <summary>
    /// user defined functions for partner module
    /// </summary>
    public class TRptUserFunctionsPartner : TRptUserFunctions
    {
        #region members for publication statistical report

        /// <summary> Holds the results of the calculation of publication statistical report </summary>
        private static DataTable FStatisticalReportDataTable;

        /// <summary> Holds the result for the row "Percent:" of the publication statistical report </summary>
        private static Dictionary <String, String>FStatisticalReportPercentage;

        /// <summary> Holds the number of active partner. This is needed for calculation of the
        /// publication statistical report. </summary>
        private static int FNumberOfActivePartner;

        /// <summary> These constants define special rows for the publication statistical report </summary>
        private const String ROW_FOREIGN = "*FOREIGN*";
        private const String ROW_NONE = "*NONE*";
        private const String ROW_PERCENT = "Percent:";
        private const String ROW_TOTAL = "Totals:";
        private const String ROW_COUNT = "Counts:";
        private const String COLUMN_CHURCH = "Churches";
        private const String COLUMN_DONOR = "Donors";
        private const String COLUMN_APPLICANTS = "Applicants";
        private const String COLUMN_EXPARTICIPANTS = "ExParticipants";
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public TRptUserFunctionsPartner() : base()
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

            if (StringHelper.IsSame(f, "GetPartnerLabelValues"))
            {
                value = new TVariant(GetPartnerLabelValues());
                return true;
            }

            if (StringHelper.IsSame(f, "GetPartnerBestAddress"))
            {
                value = new TVariant(GetPartnerBestAddress(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetPartnerContactDetails"))
            {
                value = new TVariant(GetPartnerContactDetails(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetPartnerOverallContactSettings"))
            {
                value = new TVariant(GetPartnerOverallContactSettings(ops[1].ToInt64(), ops[2].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetAddressString"))
            {
                value = new TVariant(GetAddressString(ops[1].ToInt64(), ops[2].ToInt32(), ops[3].ToInt32()));
                return true;
            }

            if (StringHelper.IsSame(f, "AddressMeetsPostCodeCriteriaOrEmpty"))
            {
                value =
                    new TVariant(AddressMeetsPostCodeCriteriaOrEmpty(ops[1].ToBool(), ops[2].ToString(), ops[3].ToString(), ops[4].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetPartnerShortname"))
            {
                value = new TVariant(GetPartnerShortName(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetFieldOfPartner"))
            {
                value = new TVariant(GetFieldOfPartner(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetFieldOfPartnerOnDate"))
            {
                value = new TVariant(GetFieldOfPartnerOnDate(ops[1].ToInt64(), ops[2].ToDate()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetFieldKeyOfPartner"))
            {
                value = new TVariant(GetFieldKeyOfPartner(ops[1].ToInt64(), ops[2].ToDate()));
                return true;
            }

            if (StringHelper.IsSame(f, "CheckAccountNumber"))
            {
                value = new TVariant(CheckAccountNumber(ops[1].ToString(), ops[2].ToString(), ops[3].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetCountyPublicationStatistic"))
            {
                value = new TVariant(GetCountyPublicationStatistic(ops[1].ToString(), ops[2].ToString(), ops[3].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "CalculatePublicationStatisticPercentage"))
            {
                value = new TVariant(FillStatisticalReportResultTable());
                return true;
            }

            if (StringHelper.IsSame(f, "GetNumberOfAllPublications"))
            {
                value = new TVariant(GetNumberOfAllPublications());
                return true;
            }

            if (StringHelper.IsSame(f, "ConvertIntToTime"))
            {
                value = new TVariant(ConvertIntToTime(ops[1].ToInt(), ops[2].ToInt()));
                return true;
            }

            if (StringHelper.IsSame(f, "DetermineAddressDateStatus"))
            {
                value = new TVariant(DetermineAddressDateStatus(ops[1].ToString(), ops[2].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetSubscriptions"))
            {
                value = new TVariant(GetSubscriptions(ops[1].ToInt64(), ops[2].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetFirstEntryFromSQLStatement"))
            {
                value = new TVariant(GetFirstEntryFromSQLStatement(ops[1].ToString(), ops[2].ToString(), ops[3].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetPartnerTypes"))
            {
                value = new TVariant(GetPartnerTypes(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetProfession"))
            {
                value = new TVariant(GetProfession(ops[1].ToInt64()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetOccupation"))
            {
                value = new TVariant(GetOccupation(ops[1].ToString()));
                return true;
            }

            value = new TVariant();
            return false;
        }

        private bool GetPartnerLabelValues()
        {
            PPartnerTable PartnerTable;

            System.Data.DataTable mTable;
            System.Int32 col;
            TRptCalculation mRptCalculation;
            TRptDataCalcCalculation mRptDataCalcCalculation;
            TRptFormatQuery mRptCalcResult;
            mRptCalculation = situation.GetReportStore().GetCalculation(situation.GetCurrentReport(), "PartnerLabelValue");
            mRptDataCalcCalculation = new TRptDataCalcCalculation(situation);
            mRptCalcResult = mRptDataCalcCalculation.EvaluateCalculationAll(mRptCalculation,
                null,
                mRptCalculation.rptGrpTemplate,
                mRptCalculation.rptGrpQuery);
            List <int>AddedColumns = new List <int>();

            if (mRptCalcResult.IsZeroOrNull())
            {
                return false;
            }

            mTable = situation.GetDatabaseConnection().SelectDT(mRptCalcResult.SQLStmt, "table", situation.GetDatabaseConnection().Transaction, mRptCalcResult.OdbcParameters.ToArray());

            foreach (DataRow mRow in mTable.Rows)
            {
                TVariant LabelValue = new TVariant();

                // Data Type: (char | integer | float | currency | boolean | date | time | partnerkey | lookup)
                if (mRow["LabelDataType"].ToString() == "char")
                {
                    LabelValue = new TVariant(mRow["LabelValueChar"].ToString());
                }
                else if (mRow["LabelDataType"].ToString() == "integer")
                {
                    LabelValue = new TVariant(mRow["LabelValueInt"]);
                }
                else if (mRow["LabelDataType"].ToString() == "float")
                {
                    // todo p_num_decimal_places_i
                    LabelValue = new TVariant(mRow["LabelValueNum"]);
                }
                else if (mRow["LabelDataType"].ToString() == "currency")
                {
                    LabelValue = new TVariant(StringHelper.FormatUsingCurrencyCode(
                            Convert.ToDecimal(mRow["LabelValueCurrency"]), mRow["CurrencyCode"].ToString()) +
                        ' ' + mRow["CurrencyCode"].ToString());
                }
                else if (mRow["LabelDataType"].ToString() == "boolean")
                {
                    LabelValue = new TVariant(mRow["LabelValueBool"]);
                }
                else if (mRow["LabelDataType"].ToString() == "date")
                {
                    LabelValue = new TVariant(mRow["LabelValueDate"]);
                }
                else if (mRow["LabelDataType"].ToString() == "time")
                {
                    // todo needs testing
                    LabelValue = new TVariant(Conversions.Int32TimeToDateTime(Convert.ToInt32(mRow["LabelValueTime"])).ToString("t"));
                }
                else if (mRow["LabelDataType"].ToString() == "lookup")
                {
                    // todo p_lookup_category_code_c
                    LabelValue = new TVariant(mRow["LabelValueLookup"]);
                }
                else if (mRow["LabelDataType"].ToString() == "partnerkey")
                {
                    // retrieve the shortname of this partner
                    LabelValue = new TVariant(mRow["LabelValuePartnerKey"]);
                    PartnerTable = PPartnerAccess.LoadByPrimaryKey(LabelValue.ToInt64(),
                        StringHelper.StrSplit(PPartnerTable.GetPartnerShortNameDBName(), ","), situation.GetDatabaseConnection().Transaction);

                    if (PartnerTable.Rows.Count != 0)
                    {
                        LabelValue = new TVariant(PartnerTable[0].PartnerShortName);
                    }
                }
                else
                {
                    LabelValue = new TVariant("unknown data label type");
                }

                for (col = 0; col <= situation.GetParameters().Get("MaxDisplayColumns").ToInt() - 1; col += 1)
                {
                    if (!AddedColumns.Contains(col))
                    {
                        situation.GetParameters().RemoveVariable("LabelValue", col, situation.GetDepth(), eParameterFit.eBestFit);
                    }

                    if (situation.GetParameters().Exists("param_label", col, -1, eParameterFit.eExact))
                    {
                        if (mRow["LabelName"].ToString() == situation.GetParameters().Get("param_label", col, -1, eParameterFit.eExact).ToString())
                        {
                            if (!LabelValue.IsNil())
                            {
                                situation.GetParameters().Add("LabelValue",
                                    LabelValue,
                                    col, situation.GetDepth(),
                                    null, null, ReportingConsts.CALCULATIONPARAMETERS);
                                AddedColumns.Add(col);
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Determine the status of an address given the "date effective from" and
        /// "date good unil" dates.
        /// The format of the date is dd/mm/yyyy
        /// </summary>
        /// <param name="DateEffectiveString">Date from when the address is valid</param>
        /// <param name="DateGoodUntilString">Date until the address is valid</param>
        /// <returns> -1 if input String is not a date
        ///             1 if address is current
        ///             2 if address is future
        ///             3 if address is expiered
        ///  </returns>
        private int DetermineAddressDateStatus(String DateEffectiveString, String DateGoodUntilString)
        {
            DateTime DateEffective;
            DateTime DateGoodUntil;
            TVerificationResult VerificationResult = null;

            if (DateEffectiveString.Length == 0)
            {
                DateEffective = new DateTime(1, 1, 1);
            }
            else
            {
                DateEffective = TDate.LongDateStringToDateTime2(DateEffectiveString, "",
                    out VerificationResult, false);
            }

            if (VerificationResult != null)
            {
                return -1;
            }

            if (DateGoodUntilString.Length == 0)
            {
                DateGoodUntil = new DateTime(2999, 12, 31);
            }
            else
            {
                DateGoodUntil = TDate.LongDateStringToDateTime2(DateGoodUntilString, "",
                    out VerificationResult, false);
            }

            if (VerificationResult != null)
            {
                return -1;
            }

            DataSet DS = new DataSet();
            DS.Tables.Add(new PPartnerLocationTable());
            PPartnerLocationTable TempTable = (PPartnerLocationTable)DS.Tables[PPartnerLocationTable.GetTableName()];

            TempTable.Columns.Add(new System.Data.DataColumn("BestAddress", typeof(Boolean)));
            TempTable.Columns.Add(new System.Data.DataColumn("Icon", typeof(Int32)));

            DataRow row = TempTable.NewRow();
            row[PPartnerLocationTable.GetDateGoodUntilDBName()] = DateGoodUntil;
            row[PPartnerLocationTable.GetDateEffectiveDBName()] = DateEffective;
            // set the values that must not be null
            row[PPartnerLocationTable.GetPartnerKeyDBName()] = 0;
            row[PPartnerLocationTable.GetSiteKeyDBName()] = 0;
            row[PPartnerLocationTable.GetLocationKeyDBName()] = 0;

            TempTable.Rows.Add(row);

            Calculations.DeterminePartnerLocationsDateStatus(DS);

            int Status = (Int32)TempTable.Rows[0]["Icon"];
            return Status;
        }

        /// <summary>
        /// Get all subscriptions with a defined status of a Partner.
        /// </summary>
        /// <param name="APartnerKey">Partner key</param>
        /// <param name="AStatusString">List of statuses for which to get the subscriptions.</param>
        /// <returns>List of subscriptions</returns>
        private String GetSubscriptions(Int64 APartnerKey, String AStatusString)
        {
            String ReturnValue = "";
            PSubscriptionTable table;
            StringCollection fields;

            fields = new StringCollection();
            fields.Add(PSubscriptionTable.GetPublicationCodeDBName());
            fields.Add(PSubscriptionTable.GetSubscriptionStatusDBName());

            table = (PSubscriptionTable)PSubscriptionAccess.LoadViaPPartnerPartnerKey(APartnerKey, fields,
                situation.GetDatabaseConnection().Transaction);

            foreach (DataRow dataRow in table.Rows)
            {
                PSubscriptionRow SubscriptionRow = (PSubscriptionRow)dataRow;

                if ((!SubscriptionRow.IsSubscriptionStatusNull())
                    && (AStatusString.Contains(SubscriptionRow.SubscriptionStatus)))
                {
                    ReturnValue = ReturnValue + "  " + SubscriptionRow.PublicationCode;
                }
            }

            if (ReturnValue.Length == 0)
            {
                ReturnValue = "No Subscriptions";
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns the first row of a data table that is the result of a sql statement from the xml file.
        /// If ReplaceString and Replacement are not null then the ReplaceString that is in
        /// the sql command is replaced by replacement.
        /// </summary>
        /// <param name="ASqlID">the identifier of the sql statement</param>
        /// <param name="AReplaceString">String in the sql command that should be replaced</param>
        /// <param name="AReplacement">the replacement</param>
        /// <returns>true if successful, otherwise false</returns>
        private bool GetFirstEntryFromSQLStatement(String ASqlID, String AReplaceString, String AReplacement)
        {
            DataTable ResultTable;
            bool ReturnValue = false;

            if (!GetDataTableFromXmlSqlStatement(ASqlID, out ResultTable, AReplaceString, AReplacement))
            {
                return false;
            }

            foreach (DataColumn col in ResultTable.Columns)
            {
                // remove the old value
                situation.GetParameters().RemoveVariable(col.ColumnName);
            }

            if (ResultTable.Rows.Count > 0)
            {
                DataRow row = ResultTable.Rows[0];

                foreach (DataColumn col in ResultTable.Columns)
                {
                    // add the new value
                    situation.GetParameters().Add(col.ColumnName, new TVariant(row[col.ColumnName]));
                }

                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// this will find the best p_partner_location, and export the values to parameters (p_street_name_c, etc.)
        /// </summary>
        /// <returns>void</returns>
        private bool GetPartnerBestAddress(Int64 APartnerKey)
        {
            bool ReturnValue = false;

            // reset the variables
            PLocationTable LocationTable = new PLocationTable();

            foreach (DataColumn col in LocationTable.Columns)
            {
                situation.GetParameters().RemoveVariable(StringHelper.UpperCamelCase(col.ColumnName, true, true));
            }

            DataTable PartnerLocationTable = new PPartnerLocationTable();

            foreach (DataColumn col in PartnerLocationTable.Columns)
            {
                // do not clear the partner key, in case we do not have an address for the partner
                if (col.ColumnName != PPartnerLocationTable.GetPartnerKeyDBName())
                {
                    situation.GetParameters().RemoveVariable(StringHelper.UpperCamelCase(col.ColumnName, true, true));
                }
            }

            situation.GetParameters().RemoveVariable("FirstName");
            situation.GetParameters().RemoveVariable("FamilyName");

            StringCollection NameColumnNames = new StringCollection();
            NameColumnNames.Add(PPartnerTable.GetPartnerShortNameDBName());
            PPartnerTable PartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, NameColumnNames,
                situation.GetDatabaseConnection().Transaction);
            string PartnerShortName = PartnerTable.Rows[0][PPartnerTable.GetPartnerShortNameDBName()].ToString();

            situation.GetParameters().AddCalculationParameter("NameWithTitle",
                new TVariant(Ict.Petra.Shared.MPartner.Calculations.FormatShortName(PartnerShortName, eShortNameFormat.eReverseShortname)));

            DataSet PartnerLocationsDS = new DataSet();
            PartnerLocationsDS.Tables.Add(new PPartnerLocationTable());
            PartnerLocationTable = PartnerLocationsDS.Tables[PPartnerLocationTable.GetTableName()];

            // add special column BestAddress and Icon
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("BestAddress", typeof(Boolean)));
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("Icon", typeof(Int32)));

            // find all locations of the Partner, put them into a dataset
            PPartnerLocationAccess.LoadViaPPartner(PartnerLocationsDS, APartnerKey, situation.GetDatabaseConnection().Transaction);

            // uses Calculations.Addresses.cs, 'DetermineBestAddress' Method
            Calculations.DeterminePartnerLocationsDateStatus(PartnerLocationsDS);
            Calculations.DetermineBestAddress(PartnerLocationsDS);

            foreach (PPartnerLocationRow row in PartnerLocationTable.Rows)
            {
                // find the row with BestAddress = 1
                if (Convert.ToInt32(row["BestAddress"]) == 1)
                {
                    // get the partner location details into the parameters
                    foreach (DataColumn col in PartnerLocationTable.Columns)
                    {
                        situation.GetParameters().AddCalculationParameter(StringHelper.UpperCamelCase(col.ColumnName, true,
                                true), new TVariant(row[col.ColumnName]));
                    }

                    // find the location record with that address
                    LocationTable = PLocationAccess.LoadByPrimaryKey(row.SiteKey, row.LocationKey, situation.GetDatabaseConnection().Transaction);

                    // put the found values in the parameters
                    if (LocationTable.Rows.Count > 0)
                    {
                        // get the location details into the parameters
                        foreach (DataColumn col in LocationTable.Columns)
                        {
                            situation.GetParameters().AddCalculationParameter(StringHelper.UpperCamelCase(col.ColumnName, true,
                                    true), new TVariant(LocationTable.Rows[0][col.ColumnName]));
                        }

                        // get the Partner Firstname and Surname as well; depends on the partner class
                        // first try person
                        NameColumnNames = new StringCollection();
                        NameColumnNames.Add(PPersonTable.GetFirstNameDBName());
                        NameColumnNames.Add(PPersonTable.GetFamilyNameDBName());
                        PPersonTable PersonTable = PPersonAccess.LoadByPrimaryKey(APartnerKey, NameColumnNames,
                            situation.GetDatabaseConnection().Transaction);

                        if (PersonTable.Rows.Count > 0)
                        {
                            situation.GetParameters().AddCalculationParameter("FirstName",
                                new TVariant(PersonTable.Rows[0][PPersonTable.GetFirstNameDBName()]));
                            situation.GetParameters().AddCalculationParameter("FamilyName",
                                new TVariant(PersonTable.Rows[0][PPersonTable.GetFamilyNameDBName()]));
                        }
                        else
                        {
                            // then it was a family?
                            NameColumnNames = new StringCollection();
                            NameColumnNames.Add(PFamilyTable.GetFirstNameDBName());
                            NameColumnNames.Add(PFamilyTable.GetFamilyNameDBName());
                            PFamilyTable FamilyTable = PFamilyAccess.LoadByPrimaryKey(APartnerKey, NameColumnNames,
                                situation.GetDatabaseConnection().Transaction);

                            if (FamilyTable.Rows.Count > 0)
                            {
                                situation.GetParameters().AddCalculationParameter("FirstName",
                                    new TVariant(FamilyTable.Rows[0][PFamilyTable.GetFirstNameDBName()]));
                                situation.GetParameters().AddCalculationParameter("FamilyName",
                                    new TVariant(FamilyTable.Rows[0][PFamilyTable.GetFamilyNameDBName()]));
                            }
                            else
                            {
                                // it was an organisation or church, just use the shortname
                                situation.GetParameters().RemoveVariable("FirstName");
                                situation.GetParameters().RemoveVariable("FamilyName");

                                if (PartnerTable.Rows.Count > 0)
                                {
                                    situation.GetParameters().AddCalculationParameter("FamilyName",
                                        new TVariant(PartnerShortName));
                                }
                            }
                        }

                        GetPartnerContactDetails(APartnerKey);
                    }

                    ReturnValue = true;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// this will find the contact details (p_partner_attribute) for this partner and export the values to parameters
        /// </summary>
        /// <returns>void</returns>
        private bool GetPartnerContactDetails(Int64 APartnerKey)
        {
            string PhoneNumber;
            string EmailAddress;
            string FaxNumber;

            // reset the variables
            situation.GetParameters().RemoveVariable("EmailAddress");
            situation.GetParameters().RemoveVariable("PrimaryEmail");
            situation.GetParameters().RemoveVariable("TelephoneNumber");
            situation.GetParameters().RemoveVariable("PrimaryPhone");
            situation.GetParameters().RemoveVariable("FaxNumber");
            situation.GetParameters().RemoveVariable("MobileNumber");
            situation.GetParameters().RemoveVariable("AlternateTelephone");
            situation.GetParameters().RemoveVariable("Url");
            situation.GetParameters().RemoveVariable("Extension");
            situation.GetParameters().RemoveVariable("FaxExtension");


            // Also put the Primary Phone Number, 'Primary E-mail Address' and Fax Number into the parameters
            TContactDetailsAggregate.GetPrimaryEmailAndPrimaryPhoneAndFax(APartnerKey,
                out PhoneNumber, out EmailAddress, out FaxNumber);

            // Now get any additional phone numbers that are current
            string moreMobileNumbers, moreLandlineNumbers;
            TContactDetailsAggregate.GetPartnersAdditionalPhoneNumbers(APartnerKey, out moreMobileNumbers, out moreLandlineNumbers);

            // Add Calculation Parameter for 'Primary Email Address' (String.Empty is supplied if the Partner hasn't got one)
            situation.GetParameters().AddCalculationParameter("EmailAddress",
                new TVariant(EmailAddress ?? String.Empty));
            situation.GetParameters().AddCalculationParameter("PrimaryEmail",
                new TVariant(EmailAddress ?? String.Empty));

            // Add Calculation Parameter for 'Primary Phone Number' (String.Empty is supplied if the Partner hasn't got one)
            situation.GetParameters().AddCalculationParameter("TelephoneNumber",
                new TVariant(PhoneNumber ?? String.Empty));
            situation.GetParameters().AddCalculationParameter("PrimaryPhone",
                new TVariant(PhoneNumber ?? String.Empty));

            // Add Calculation Parameter for 'Fax Number' (String.Empty is supplied if the Partner hasn't got one)
            situation.GetParameters().AddCalculationParameter("FaxNumber",
                new TVariant(FaxNumber ?? String.Empty));

            situation.GetParameters().AddCalculationParameter("MobileNumber", new TVariant(moreMobileNumbers ?? String.Empty));
            situation.GetParameters().AddCalculationParameter("AlternateTelephone", new TVariant(moreLandlineNumbers ?? String.Empty));

            // At present we no longer support the reporting of the following, so we set those Calculation Parameters to String.Empty
            situation.GetParameters().AddCalculationParameter("Url", new TVariant(String.Empty));
            // Extension and FaxExtension no longer exist in the Contact Details scheme so we set those Calculation Parameters to String.Empty
            situation.GetParameters().AddCalculationParameter("Extension", new TVariant(String.Empty));
            situation.GetParameters().AddCalculationParameter("FaxExtension", new TVariant(String.Empty));

            return true;
        }

        /// <summary>
        /// This will find all 'Overall Contact Settings' for the Contact Details of a Partner and return them.
        /// </summary>
        /// <returns>String containing the 'Overall Contact Settings' for the Contact Details of a Partner.</returns>
        private string GetPartnerOverallContactSettings(Int64 APartnerKey, string APartnerClass)
        {
            const string SEPARATOR = "; ";
            string HeadingStr = Catalog.GetString("Overall Contact Settings:  ");
            string AllContactDetailsStr = HeadingStr;

            Calculations.TPartnersOverallContactSettings PartnersOvrlCS;
            var PartnerClass = SharedTypes.PartnerClassStringToEnum(APartnerClass);

            PartnersOvrlCS = TContactDetailsAggregate.GetPartnersOverallCS(APartnerKey,
                Calculations.TOverallContSettingKind.ocskPrimaryContactMethod |
                Calculations.TOverallContSettingKind.ocskPrimaryEmailAddress |
                Calculations.TOverallContSettingKind.ocskSecondaryEmailAddress |
                Calculations.TOverallContSettingKind.ocskPrimaryPhoneNumber |
                Calculations.TOverallContSettingKind.ocskEmailAddressWithinOrg |
                Calculations.TOverallContSettingKind.ocskPhoneNumberWithinOrg);

            if (PartnersOvrlCS != null)
            {
                if (PartnersOvrlCS.PrimaryPhoneNumber != null)
                {
                    AllContactDetailsStr += Catalog.GetString("Primary Phone: ") +
                                            PartnersOvrlCS.PrimaryPhoneNumber + SEPARATOR;
                }

                if (PartnersOvrlCS.PrimaryEmailAddress != null)
                {
                    AllContactDetailsStr += Catalog.GetString("Primary E-Mail: ") +
                                            PartnersOvrlCS.PrimaryEmailAddress + SEPARATOR;
                }

                if (PartnerClass == TPartnerClass.FAMILY)
                {
                    if (PartnersOvrlCS.SecondaryEmailAddress != null)
                    {
                        AllContactDetailsStr += Catalog.GetString("Secondary Email Address: ") +
                                                PartnersOvrlCS.SecondaryEmailAddress + SEPARATOR;
                    }
                }
                else if (PartnerClass == TPartnerClass.PERSON)
                {
                    if (PartnersOvrlCS.PhoneNumberWithinOrg != null)
                    {
                        AllContactDetailsStr += Catalog.GetString("Office Phone: ") +
                                                PartnersOvrlCS.PhoneNumberWithinOrg + SEPARATOR;
                    }

                    if (PartnersOvrlCS.EmailAddressWithinOrg != null)
                    {
                        AllContactDetailsStr += Catalog.GetString("Office E-Mail: ") +
                                                PartnersOvrlCS.EmailAddressWithinOrg + SEPARATOR;
                    }
                }

                AllContactDetailsStr += Catalog.GetString("Primary Contact Method: ");

                if (PartnersOvrlCS.PrimaryContactMethod != null)
                {
                    AllContactDetailsStr += PartnersOvrlCS.PrimaryContactMethod + SEPARATOR;
                }
                else
                {
                    AllContactDetailsStr += "Not specified" + SEPARATOR;
                }
            }

            if (AllContactDetailsStr != HeadingStr)
            {
                // Remove last SEPARATOR
                AllContactDetailsStr = AllContactDetailsStr.Substring(0, AllContactDetailsStr.Length - SEPARATOR.Length);
            }
            else
            {
                AllContactDetailsStr += Catalog.GetString("None");
            }

            situation.GetParameters().AddCalculationParameter("Username", new TVariant(UserInfo.GUserInfo.UserID));

            return AllContactDetailsStr;
        }

        /// <summary>
        /// this will return true if the best address is in the given postal region, postcode range and county it will not reset the data returned by best address
        /// </summary>
        /// <returns>void</returns>
        private bool AddressMeetsPostCodeCriteriaOrEmpty(bool ABestAddressWasFound,
            String APostalRegion,
            String APostCodeFrom,
            String APostCodeTo)
        {
            if (!ABestAddressWasFound)
            {
                // only return true if the postcode parameters and county parameters are empty
                return (APostalRegion.Length == 0) && (APostCodeFrom.Length == 0) && (APostCodeTo.Length == 0);
            }

            return ExtractQueryBase.AddressMeetsPostCodeCriteriaOrEmpty(situation.GetParameters().Get("PostalCode").ToString(),
                APostalRegion, APostCodeFrom, APostCodeTo);
        }

        private String GetPartnerShortName(Int64 APartnerKey)
        {
            String ReturnValue;
            PPartnerTable table;
            StringCollection fields;

            ReturnValue = "N/A";
            fields = new StringCollection();
            fields.Add(PPartnerTable.GetPartnerShortNameDBName());
            table = PPartnerAccess.LoadByPrimaryKey(APartnerKey, fields, situation.GetDatabaseConnection().Transaction);

            if (table.Rows.Count > 0)
            {
                ReturnValue = table[0].PartnerShortName;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Get the field Key of one partner
        /// </summary>
        /// <param name="APartnerKey">partnerkey</param>
        /// <param name="ACommitmentDate">date on which partner is at field</param>
        /// <returns>The field key if it was found. Otherwise 0</returns>
        private List <Int64>GetFieldKeyOfPartner(Int64 APartnerKey, DateTime ACommitmentDate)
        {
            List <Int64>ListOfFields = new List <Int64>();
            String PartnerClass = "";
            DateTime QualifyingDate = ACommitmentDate;

            PPartnerTable tbl = PPartnerAccess.LoadByPrimaryKey(APartnerKey, situation.GetDatabaseConnection().Transaction);

            if (tbl.Rows.Count > 0)
            {
                PartnerClass = tbl[0].PartnerClass;

                // If the partner given is a family, then defer to family member with id=0 to determine the (target) field
                if (PartnerClass == TPartnerClass.FAMILY.ToString())
                {
                    PPersonTable familyMembers = PPersonAccess.LoadViaPFamily(APartnerKey, situation.GetDatabaseConnection().Transaction);
                    familyMembers.DefaultView.RowFilter = "p_family_id_i = 0";

                    if (familyMembers.DefaultView.Count > 0)
                    {
                        return GetFieldKeyOfPartner(((PPersonRow)familyMembers.DefaultView[0].Row).PartnerKey, QualifyingDate);
                    }
                }
                // If the partner given is a person then check commitment records of the person to determine the (target) field
                else if (PartnerClass == TPartnerClass.PERSON.ToString())
                {
                    PmStaffDataTable commitments = PmStaffDataAccess.LoadViaPPerson(APartnerKey, situation.GetDatabaseConnection().Transaction);
                    commitments.DefaultView.RowFilter = "pm_start_of_commitment_d <= '" + QualifyingDate.Date.ToString("yyyy/MM/dd") +
                                                        "' AND (pm_end_of_commitment_d IS NULL OR pm_end_of_commitment_d >= '" +
                                                        QualifyingDate.Date.ToString("yyyy/MM/dd") + "')";
                    commitments.DefaultView.Sort = "pm_start_of_commitment_d";

                    foreach (DataRowView rowView in commitments.DefaultView)
                    {
                        DataRow row = rowView.Row;
                        ListOfFields.Add(((PmStaffDataRow)row).ReceivingField);
                    }
                }
                else if (PartnerClass == TPartnerClass.UNIT.ToString())
                {
                    // If the partner given is a keymin, I need to find the parent field:
                    PPartnerTypeTable TypeTbl = PPartnerTypeAccess.LoadViaPPartner(APartnerKey, situation.GetDatabaseConnection().Transaction);

                    if (TypeTbl.Rows.Count > 0)
                    {
                        String PartnerType = TypeTbl[0].TypeCode;

                        if ((PartnerType == "FIELD") || (PartnerType == "LEDGER")) // I've been given a field already (perhaps by recursion)
                        {
                            ListOfFields.Add(APartnerKey);
                        }
                        else
                        {
                            UmUnitStructureTable UnitTbl = UmUnitStructureAccess.LoadViaPUnitChildUnitKey(APartnerKey,
                                situation.GetDatabaseConnection().Transaction);

                            if (UnitTbl.Rows.Count > 0)
                            {
                                return GetFieldKeyOfPartner(UnitTbl[0].ParentUnitKey, QualifyingDate);
                            }
                        }
                    }
                }
            }

            return ListOfFields;
        }

        /// <summary>
        /// Get the field name of one partner
        /// </summary>
        /// <param name="APartnerKey">partnerkey</param>
        /// <returns>The field name if it was found. Otherwise empty string</returns>
        private String GetFieldOfPartner(Int64 APartnerKey)
        {
            return GetFieldOfPartnerOnDate(APartnerKey, DateTime.Today.Date);
        }

        /// <summary>
        /// Get the field name of one partner on a certain date
        /// </summary>
        /// <param name="APartnerKey">partnerkey</param>
        /// <param name="ACommitmentDate">date on which partner is at field</param>
        /// <returns>The field name if it was found. Otherwise empty string</returns>
        private String GetFieldOfPartnerOnDate(Int64 APartnerKey, DateTime ACommitmentDate)
        {
            List <Int64>FieldKeys = GetFieldKeyOfPartner(APartnerKey, ACommitmentDate);
            String FieldKeysText = "";

            if (FieldKeys.Count > 0)
            {
                // concatenate field keys, separated by comma (in most cases there is just one field but someone may have commitments to several fields)
                foreach (Int64 FieldKey in FieldKeys)
                {
                    if (FieldKeysText.Length > 0)
                    {
                        FieldKeysText += ", ";
                    }

                    FieldKeysText += GetPartnerShortName(FieldKey);
                }
            }

            return FieldKeysText;
        }

        /// <summary>
        /// Checks the validity of a bank account number.
        /// This function checks the validity of a bank account number by performing a
        /// country-specific check on the submitted account number.
        /// If a bank partner key is submitted, this function is looking
        /// up the bank's location country - this is convenient since the calling procedure
        /// does not need to know the bank's country in this case.
        /// </summary>
        /// <param name="AAccountNumber">Account number</param>
        /// <param name="ABankCountryCode">Country code of the bank
        /// (optional - needs only to be specified if bank_partner_key_n is not submitted)</param>
        /// <param name="ABankPartnerKey">Parner key of the bank
        /// (optional - needs only to be specified if bank_country_code_c is not submitted)</param>
        /// <returns> -1 = length check failed.
        ///            0 = invalid account number
        ///            1 = valid account number
        ///            2 = probably valid - account number cannot be validated by country-specific check
        ///            3 = account number could not be validated - no country-specific check implemented
        ///            4 = Bank partner could not be found
        /// </returns>
        private int CheckAccountNumber(String AAccountNumber, String ABankCountryCode, String ABankPartnerKey)
        {
            int ReturnValue;
            long PartnerKey = -1;

            ReturnValue = 1;

            PLocationTable LocationTable;
            PPartnerLocationRow PartnerLocationRow;

            if (ABankPartnerKey.Length > 0)
            {
                try
                {
                    PartnerKey = Convert.ToInt64(ABankPartnerKey);
                }
                catch (Exception)
                {
                    ReturnValue = 4;
                    return ReturnValue;
                }
            }

            if ((ABankCountryCode.Length == 0)
                || (ABankCountryCode == "?"))
            {
                if (GetPartnerBestAddressRow(PartnerKey, situation, out PartnerLocationRow))
                {
                    LocationTable = PLocationAccess.LoadByPrimaryKey(PartnerLocationRow.SiteKey,
                        PartnerLocationRow.LocationKey,
                        situation.GetDatabaseConnection().Transaction);

                    if (LocationTable.Rows.Count > 0)
                    {
                        ABankCountryCode = ((PLocationRow)LocationTable.Rows[0]).CountryCode;
                    }
                    else
                    {
                        ReturnValue = 4;
                        return ReturnValue;
                    }
                }
                else
                {
                    ReturnValue = 4;
                    return ReturnValue;
                }
            }

            Ict.Petra.Shared.MFinance.CommonRoutines FinanceRoutines = new Ict.Petra.Shared.MFinance.CommonRoutines();
            return FinanceRoutines.CheckAccountNumber(AAccountNumber, ABankCountryCode);
        }

        /// <summary>
        /// Find the best address of a partner
        /// </summary>
        /// <param name="APartnerKey">Partner key</param>
        /// <param name="ASituation">describing the current state of the report generator</param>
        /// <param name="AAddressRow">best address</param>
        /// <returns>true if a best address was found, otherwise false</returns>
        public static bool GetPartnerBestAddressRow(long APartnerKey, TRptSituation ASituation, out PPartnerLocationRow AAddressRow)
        {
            bool FoundBestAddress = false;

            AAddressRow = null;
            PPartnerLocationTable PartnerLocationTable;

            PartnerLocationTable = new PPartnerLocationTable();

            // add special column BestAddress and Icon
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("BestAddress", typeof(Boolean)));
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("Icon", typeof(Int32)));

            // find all locations of the partner, put it into a dataset
            PartnerLocationTable = PPartnerLocationAccess.LoadViaPPartner(APartnerKey, ASituation.GetDatabaseConnection().Transaction);

            // uses Ict.Petra.Shared.MPartner.Calculations.pas, DetermineBestAddress
            Calculations.DeterminePartnerLocationsDateStatus(PartnerLocationTable, DateTime.Today);
            Calculations.DetermineBestAddress(PartnerLocationTable);

            foreach (PPartnerLocationRow row in PartnerLocationTable.Rows)
            {
                // find the row with BestAddress = 1
                if (Convert.ToInt32(row["BestAddress"]) == 1)
                {
                    AAddressRow = row;
                    FoundBestAddress = true;
                }
            }

            return FoundBestAddress;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ASiteKey"></param>
        /// <param name="ALocationKey"></param>
        /// <param name="AAddressOrder"></param>
        /// <returns></returns>
        public String GetAddressString(long ASiteKey, Int32 ALocationKey, Int32 AAddressOrder)
        {
            String Ret = "";
            PLocationTable LocTbl = PLocationAccess.LoadByPrimaryKey(ASiteKey, ALocationKey,
                situation.GetDatabaseConnection().Transaction);

            if (LocTbl.Rows.Count > 0)
            {
                PLocationRow LocRow = LocTbl[0];
                Ret = Calculations.DetermineLocationString(LocRow, Calculations.TPartnerLocationFormatEnum.plfCommaSeparated,
                    AAddressOrder);
            }

            return Ret;
        }

        #region calculation for publication statistical report

        /// <summary>
        /// Calculates one row of the current county for the publication statistical report.
        /// If ACounty is empty then it calculates the row where partner addresses
        /// have no county specified
        /// </summary>
        /// <param name="ACountryCode">Country Code</param>
        /// <param name="ACounty">County</param>
        /// <param name="PublicationCodes"></param>
        /// <returns></returns>
        private bool GetCountyPublicationStatistic(String ACountryCode, String ACounty, string PublicationCodes)
        {
            // if this is the first call...
            if (FStatisticalReportDataTable == null)
            {
                CalculatePublicationStatisticalReport(ACountryCode, PublicationCodes);
            }

            FillStatisticalReportResultTable(ACounty);

            return true;
        }

        /// <summary>
        /// Calculates the line "Counts" in the publication statistical report.
        /// It includes the number of copies sent to each partner plus the bulk publications.
        ///
        /// </summary>
        /// <returns></returns>
        private bool GetNumberOfAllPublications()
        {
            FillStatisticalReportResultTable(ROW_COUNT);

            FStatisticalReportDataTable.Clear();
            FStatisticalReportDataTable = null;
            FStatisticalReportPercentage.Clear();
            FStatisticalReportPercentage = null;

            return true;
        }

        /// <summary>
        /// Returns a data table that is the result of a sql statement from the xml file.
        /// If ReplaceString and Replacement are not null then the ReplaceString that is in
        /// the sql command is replaced by replacement.
        /// </summary>
        /// <param name="ASqlID">the identifier of the sql statement</param>
        /// <param name="ADataTable">Result</param>
        /// <param name="ReplaceString">String in the sql command that should be replaced</param>
        /// <param name="Replacement">the replacement</param>
        /// <returns>true if successful, otherwise false</returns>
        private bool GetDataTableFromXmlSqlStatement(String ASqlID, out DataTable ADataTable, String ReplaceString, String Replacement)
        {
            ADataTable = null;
            TRptCalculation ReportCalculation;
            TRptDataCalcCalculation ReportDataCalcCalculation;
            TRptFormatQuery ReportCalcResult;

            ReportCalculation = situation.GetReportStore().GetCalculation(situation.GetCurrentReport(), ASqlID);

            ReportDataCalcCalculation = new TRptDataCalcCalculation(situation);
            ReportCalcResult = ReportDataCalcCalculation.EvaluateCalculationAll(ReportCalculation,
                null, ReportCalculation.rptGrpTemplate, ReportCalculation.rptGrpQuery);

            if (ReportCalcResult.IsZeroOrNull())
            {
                return false;
            }

            String SqlStatement = ReportCalcResult.SQLStmt;

            if ((ReplaceString != null)
                && (Replacement != null))
            {
                SqlStatement = SqlStatement.Replace(ReplaceString, Replacement);
            }

            ADataTable = situation.GetDatabaseConnection().SelectDT(SqlStatement, "table", situation.GetDatabaseConnection().Transaction, ReportCalcResult.OdbcParameters.ToArray());

            return true;
        }

        /// <summary>
        /// Initializes the SubscriptionCounter. Adds for each found subscription a
        /// key pair value where the key is the supscription code and the value is 0.
        /// </summary>
        /// <param name="SubscriptionCounter">The SubscriptionCounter to initialize</param>
        private void InitSubscriptionCounter(ref Dictionary <String, int>SubscriptionCounter)
        {
            PPublicationTable PublicationTable = new PPublicationTable();

            PublicationTable = PPublicationAccess.LoadAll(situation.GetDatabaseConnection().Transaction);

            foreach (PPublicationRow row in PublicationTable.Rows)
            {
                SubscriptionCounter.Add(row.PublicationCode, 0);
            }
        }

        /// <summary>
        /// Initialises the Table that holds the result for calculating the
        /// publication statistical report.
        /// Set up the rows and columns for the table.
        /// </summary>
        /// <returns>Dictionary that holds for each publication the row index in the table</returns>
        private Dictionary <String, int>InitStatisticalReportTable()
        {
            Dictionary <String, int>SubscriptionCounter = new Dictionary <String, int>();
            InitSubscriptionCounter(ref SubscriptionCounter);

            FStatisticalReportDataTable = new DataTable();
            FStatisticalReportPercentage = new Dictionary <String, String>();

            DataColumn Column = new DataColumn();
            Column.DataType = Type.GetType("System.String");
            Column.ColumnName = "County";
            Column.DefaultValue = "";
            FStatisticalReportDataTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.ColumnName = COLUMN_DONOR;
            Column.DefaultValue = 0;
            FStatisticalReportDataTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.ColumnName = COLUMN_EXPARTICIPANTS;
            Column.DefaultValue = 0;
            FStatisticalReportDataTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.ColumnName = COLUMN_CHURCH;
            Column.DefaultValue = 0;
            FStatisticalReportDataTable.Columns.Add(Column);

            Column = new DataColumn();
            Column.DataType = Type.GetType("System.Int32");
            Column.ColumnName = COLUMN_APPLICANTS;
            Column.DefaultValue = 0;
            FStatisticalReportDataTable.Columns.Add(Column);

            foreach (KeyValuePair <String, int>ValuePair in SubscriptionCounter)
            {
                Column = new DataColumn();
                Column.DataType = Type.GetType("System.Int32");
                Column.ColumnName = ValuePair.Key;
                Column.DefaultValue = 0;
                FStatisticalReportDataTable.Columns.Add(Column);
            }

            // Add the rows to the table
            int CurrentRowCounter = 0;
            Dictionary <String, int>CountyRowList = new Dictionary <String, int>();

            DataRow TmpRow = FStatisticalReportDataTable.NewRow();
            TmpRow[0] = ROW_FOREIGN;
            FStatisticalReportDataTable.Rows.Add(TmpRow);
            CountyRowList.Add(ROW_FOREIGN, CurrentRowCounter++);

            TmpRow = FStatisticalReportDataTable.NewRow();
            TmpRow[0] = ROW_NONE;
            FStatisticalReportDataTable.Rows.Add(TmpRow);
            CountyRowList.Add(ROW_NONE, CurrentRowCounter++);

            // Get all counties from country
            DataTable AllCountiesDT;
            GetDataTableFromXmlSqlStatement("GetAllCounties", out AllCountiesDT, null, null);

            foreach (DataRow Row in AllCountiesDT.Rows)
            {
                TmpRow = FStatisticalReportDataTable.NewRow();
                TmpRow[0] = (String)Row[0];
                FStatisticalReportDataTable.Rows.Add(TmpRow);

                if (!CountyRowList.ContainsKey(((String)Row[0]).ToLower()))
                {
                    CountyRowList.Add(((String)Row[0]).ToLower(), CurrentRowCounter++);
                }
                else
                {
                    CurrentRowCounter++;
                }
            }

            TmpRow = FStatisticalReportDataTable.NewRow();
            TmpRow[0] = ROW_PERCENT;
            FStatisticalReportDataTable.Rows.Add(TmpRow);
            CountyRowList.Add(ROW_PERCENT, CurrentRowCounter++);

            TmpRow = FStatisticalReportDataTable.NewRow();
            TmpRow[0] = ROW_TOTAL;
            FStatisticalReportDataTable.Rows.Add(TmpRow);
            CountyRowList.Add(ROW_TOTAL, CurrentRowCounter++);

            TmpRow = FStatisticalReportDataTable.NewRow();
            TmpRow[0] = ROW_COUNT;
            FStatisticalReportDataTable.Rows.Add(TmpRow);
            CountyRowList.Add(ROW_COUNT, CurrentRowCounter++);

            return CountyRowList;
        }

        /// <summary>
        /// Main calculation of the publication statistical report.
        /// </summary>
        /// <param name="ACountryCode"></param>
        /// <param name="APublicationCodes"></param>
        private void CalculatePublicationStatisticalReport(String ACountryCode, string APublicationCodes)
        {
            DataTable PartnerTable = new DataTable();;

            PLocationTable LocationTable;
            PLocationRow LocationRow;

            // split list of publication codes, and remove the quotes
            // preparing for SQL Parameter
            string[] PublicationCodes = APublicationCodes.Split(new char[] {','});
            string PublicationCodesPlaceholders = string.Empty;
            List<OdbcParameter> PublicationCodesParameters = new List<OdbcParameter>();

            for (int pubIndex = 0; pubIndex < PublicationCodes.Length; pubIndex++)
            {
                PublicationCodesParameters.Add(new OdbcParameter("publicationcode" + pubIndex.ToString(), OdbcType.VarChar)
                                   { Value = PublicationCodes[pubIndex].Trim(new char[] {'"', '\'', ' '}) });

                if (pubIndex > 0)
                {
                    PublicationCodesPlaceholders += ",";
                }

                PublicationCodesPlaceholders += "?";
            }
            

            // load all active partners who have subscriptions for publications in the list,
            // are a donor, are a church, are an applicant or are an ex-worker
            string Query = "SELECT DISTINCT p_partner.*," +

                           // is partner a donor?
                           " CASE WHEN EXISTS (SELECT * FROM PUB_a_gift" +
                           " WHERE a_gift.p_donor_key_n = p_partner.p_partner_key_n) THEN 'yes'" +
                           " ELSE '' END AS " + COLUMN_DONOR + "," +

                           // is partner an ex-worker?
                           " CASE WHEN EXISTS (SELECT * FROM p_partner_type " +
                           " WHERE p_partner_type.p_partner_key_n = p_partner.p_partner_key_n" +
                           " AND p_partner_type.p_type_code_c LIKE 'EX-WORKER%') THEN 'yes'" +
                           " ELSE 'no' END AS " + COLUMN_EXPARTICIPANTS + "," +

                           // is partner an applicant?
                           " CASE WHEN EXISTS (SELECT * FROM p_partner_type " +
                           " WHERE p_partner_type.p_partner_key_n = p_partner.p_partner_key_n" +
                           " AND p_partner_type.p_type_code_c LIKE 'APPLIED%') THEN 'yes'" +
                           " ELSE 'no' END AS " + COLUMN_APPLICANTS +

                           " FROM p_partner" +
                           " WHERE p_partner.p_status_code_c = 'ACTIVE'" +

                           // if have subscriptions for publications in the list
                           " AND (EXISTS (SELECT * FROM p_subscription" +
                           " WHERE p_subscription.p_partner_key_n = p_partner.p_partner_key_n" +

                           " AND p_subscription.p_publication_code_c IN (" + PublicationCodesPlaceholders + "))" +

                           // if a donor
                           " OR EXISTS (SELECT * FROM PUB_a_gift" +
                           " WHERE a_gift.p_donor_key_n = p_partner.p_partner_key_n)" +

                           // if a church
                           " OR p_partner.p_partner_class_c = 'CHURCH'" +

                           // if an applicant or an ex-worker
                           " OR EXISTS (SELECT * FROM p_partner_type " +
                           " WHERE p_partner_type.p_partner_key_n = p_partner.p_partner_key_n" +
                           " AND (p_partner_type.p_type_code_c LIKE 'EX-WORKER%'" +
                           " OR p_partner_type.p_type_code_c LIKE 'APPLIED%')))";

            PartnerTable = DBAccess.GDBAccessObj.SelectDT(PartnerTable, Query, situation.GetDatabaseConnection().Transaction, PublicationCodesParameters.ToArray());

            // get total number of active partners
            FNumberOfActivePartner = PPartnerAccess.CountViaPPartnerStatus("ACTIVE", situation.GetDatabaseConnection().Transaction);

            PSubscriptionTable SubscriptionTable = new PSubscriptionTable();

            // load all subscriptions for publications in the list
            Query = "SELECT * FROM p_subscription" +
                    " WHERE p_subscription.p_publication_code_c IN (" + PublicationCodesPlaceholders + ")";
            SubscriptionTable = (PSubscriptionTable)DBAccess.GDBAccessObj.SelectDT(SubscriptionTable, Query,
                situation.GetDatabaseConnection().Transaction, PublicationCodesParameters.ToArray());

            PPartnerLocationRow PartnerLocationRow;

            Dictionary <String, int>SubscriptionCounter = new Dictionary <String, int>();
            InitSubscriptionCounter(ref SubscriptionCounter);

            Dictionary <String, int>CountyRowList = InitStatisticalReportTable();

            // foreach partner that satisfied the above criteria
            foreach (DataRow PartnerRow in PartnerTable.Rows)
            {
                Int64 PartnerKey = Convert.ToInt64(PartnerRow[PPartnerTable.GetPartnerKeyDBName()]);

                if (GetPartnerBestAddressRow(PartnerKey, situation, out PartnerLocationRow))
                {
                    if (PartnerLocationRow == null)
                    {
                        continue;
                    }

                    if ((!PartnerLocationRow.IsDateGoodUntilNull())
                        && (PartnerLocationRow.DateGoodUntil < System.DateTime.Today)) // Address is old - don't use this.
                    {
                        continue;
                    }

                    LocationTable = PLocationAccess.LoadByPrimaryKey(PartnerLocationRow.SiteKey,
                        PartnerLocationRow.LocationKey, situation.GetDatabaseConnection().Transaction);

                    if (LocationTable.Rows.Count < 1) // This is a fault - there should be a single row returned.
                    {
                        continue;
                    }

                    LocationRow = LocationTable[0];

                    // TODO what do we do with partners that have 0 as Location key?
                    if (LocationRow.LocationKey == 0)
                    {
                        continue;
                    }

                    String RowName = ROW_FOREIGN;

                    // Use county if in this country, else foreign
                    if ((!LocationRow.IsCountryCodeNull())
                        && (LocationRow.CountryCode == ACountryCode))
                    {
                        // partner is OK
                        if (LocationRow.County.Length > 0)
                        {
                            // County
                            RowName = LocationRow.County.ToLower().Trim();
                        }
                        else
                        {
                            // *NONE*
                            RowName = ROW_NONE;
                        }
                    }

                    // get the subscriptions that this partner receives (if any)
                    DataRow[] Subscriptions = SubscriptionTable.Select(PPartnerTable.GetPartnerKeyDBName() + " = " + PartnerKey);

                    foreach (DataRow Row in Subscriptions)
                    {
                        PSubscriptionRow SubscriptionRow = (PSubscriptionRow)Row;

                        // if there is a cancelled date set, then don't use this subscription in the report
                        if (SubscriptionRow.IsDateCancelledNull()
                            && ((SubscriptionRow.SubscriptionStatus == "PROVISIONAL")
                                || (SubscriptionRow.SubscriptionStatus == "PERMANENT")
                                || (SubscriptionRow.SubscriptionStatus == "GIFT")))
                        {
                            // Add Value to Table
                            AddToStatisticalReportTable(CountyRowList[RowName], SubscriptionRow.PublicationCode, 1);

                            // Add number of copies to overall "Count:" column
                            AddToStatisticalReportTable(CountyRowList[ROW_COUNT], SubscriptionRow.PublicationCode, SubscriptionRow.PublicationCopies);
                        }
                    }

                    // partner is a donor
                    if (PartnerRow[COLUMN_DONOR].ToString() == "yes")
                    {
                        AddToStatisticalReportTable(CountyRowList[RowName], COLUMN_DONOR, 1);
                    }

                    // partner is church
                    if (PartnerRow[PPartnerTable.GetPartnerClassDBName()].ToString() == "CHURCH")
                    {
                        AddToStatisticalReportTable(CountyRowList[RowName], COLUMN_CHURCH, 1);
                    }

                    // partner is an applicant
                    if (PartnerRow[COLUMN_APPLICANTS].ToString() == "yes")
                    {
                        AddToStatisticalReportTable(CountyRowList[RowName], COLUMN_APPLICANTS, 1);
                    }

                    // partner is an ex-worker
                    if (PartnerRow[COLUMN_EXPARTICIPANTS].ToString() == "yes")
                    {
                        AddToStatisticalReportTable(CountyRowList[RowName], COLUMN_EXPARTICIPANTS, 1);
                    }
                }
            }             // end for each

            CalculateTotals();
        }

        /// <summary>
        /// Fills the values of one line in the publication statistical report.
        /// The first column contains the county name which is the identifier.
        /// </summary>
        /// <param name="ARow">County</param>
        private bool FillStatisticalReportResultTable(String ARow)
        {
            // Delete old label values, so that they are not copied over from previous county
            for (int col = 0; col <= situation.GetParameters().Get("MaxDisplayColumns").ToInt() - 1; col += 1)
            {
                if ("Publication" == situation.GetParameters().Get("param_label", col, -1, eParameterFit.eExact).ToString())
                {
                    situation.GetParameters().RemoveVariable("Publication", col, situation.GetDepth(), eParameterFit.eBestFit);
                }
            }

            int RowIndex = -1;

            for (int Counter = 0; Counter < FStatisticalReportDataTable.Rows.Count; ++Counter)
            {
                String CountyName = (String)FStatisticalReportDataTable.Rows[Counter]["County"];

                if (CountyName == ARow)
                {
                    RowIndex = Counter;
                    break;
                }
            }

            if (RowIndex == -1)
            {
                // County not found
                return false;
            }

            DataRow Row = FStatisticalReportDataTable.Rows[RowIndex];

            for (int col = 0; col <= situation.GetParameters().Get("MaxDisplayColumns").ToInt() - 1; col += 1)
            {
                if (situation.GetParameters().Exists("param_label", col, -1, eParameterFit.eExact))
                {
                    String ParameterLabel = situation.GetParameters().Get("param_label", col, -1, eParameterFit.eExact).ToString();

                    if (ParameterLabel == "Publication")
                    {
                        String PublicationName = situation.GetParameters().Get("ColumnCaption", col, -1, eParameterFit.eExact).ToString();

                        for (int ColumnCounter = 5; ColumnCounter < FStatisticalReportDataTable.Columns.Count; ++ColumnCounter)
                        {
                            if (FStatisticalReportDataTable.Columns[ColumnCounter].ColumnName == PublicationName)
                            {
                                situation.GetParameters().Add("Publication",
                                    new TVariant(Row.ItemArray[ColumnCounter]),
                                    col, -1,
                                    null, null, ReportingConsts.CALCULATIONPARAMETERS);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (ARow != ROW_COUNT)
                        {
                            FillFirstColumns(ParameterLabel, col,
                                (String)Row.ItemArray[0],
                                new TVariant((int)Row.ItemArray[1]),
                                new TVariant((int)Row.ItemArray[2]),
                                new TVariant((int)Row.ItemArray[3]),
                                new TVariant((int)Row.ItemArray[4]));
                        }
                        else
                        {
                            FillFirstColumns(ParameterLabel, col,
                                (String)Row.ItemArray[0],
                                new TVariant(""),
                                new TVariant(""),
                                new TVariant(""),
                                new TVariant(""));
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Fill one of the first five columns of one row of the publication statistical report
        /// 1. column is County name
        /// 2. column is number of donors
        /// 3. column is number of ex workers
        /// 4. column is number of churches
        /// 5. column is number of applicants
        /// </summary>
        /// <param name="AParameterLabel">defines which of the first five columns should be filled</param>
        /// <param name="AColumn">Column index in the result</param>
        /// <param name="ACountyName"></param>
        /// <param name="ADonors"></param>
        /// <param name="AExParticipants"></param>
        /// <param name="AChurches"></param>
        /// <param name="AApplicants"></param>
        private void FillFirstColumns(String AParameterLabel, int AColumn, String ACountyName,
            TVariant ADonors, TVariant AExParticipants,
            TVariant AChurches, TVariant AApplicants)
        {
            TVariant ValueToUse = null;

            if (AParameterLabel == "County")
            {
                ValueToUse = new TVariant(ACountyName);
            }
            else if (AParameterLabel == "Donors")
            {
                ValueToUse = ADonors;
            }
            else if (AParameterLabel == "ExParticipants")
            {
                ValueToUse = AExParticipants;
            }
            else if (AParameterLabel == "Churches")
            {
                ValueToUse = AChurches;
            }
            else if (AParameterLabel == "Applicants")
            {
                ValueToUse = AApplicants;
            }

            if (ValueToUse != null)
            {
                situation.GetParameters().Add(AParameterLabel,
                    ValueToUse,
                    AColumn, -1,
                    null, null, ReportingConsts.CALCULATIONPARAMETERS);
            }
        }

        /// <summary>
        /// Fills the values of one line in the publication statistical report.
        /// The first column contains the county name which is the identifier.
        /// </summary>
        private bool FillStatisticalReportResultTable()
        {
            // Delete old label values, so that they are not copied over from previous partners
            for (int col = 0; col <= situation.GetParameters().Get("MaxDisplayColumns").ToInt() - 1; col += 1)
            {
                if ("Publication" == situation.GetParameters().Get("param_label", col, -1, eParameterFit.eExact).ToString())
                {
                    situation.GetParameters().RemoveVariable("Publication", col, situation.GetDepth(), eParameterFit.eBestFit);
                }
            }

            if (FStatisticalReportPercentage.Count > 0)
            {
                for (int col = 0; col <= situation.GetParameters().Get("MaxDisplayColumns").ToInt() - 1; col += 1)
                {
                    if (situation.GetParameters().Exists("param_label", col, -1, eParameterFit.eExact))
                    {
                        String ParameterLabel = situation.GetParameters().Get("param_label", col, -1, eParameterFit.eExact).ToString();

                        if (ParameterLabel == "Publication")
                        {
                            String PublicationName = situation.GetParameters().Get("ColumnCaption", col, -1, eParameterFit.eExact).ToString();

                            situation.GetParameters().Add("Publication",
                                FStatisticalReportPercentage[PublicationName],
                                col, -1,
                                null, null, ReportingConsts.CALCULATIONPARAMETERS);
                        }
                        else
                        {
                            FillFirstColumns(ParameterLabel, col, ROW_PERCENT,
                                new TVariant(FStatisticalReportPercentage[COLUMN_DONOR]),
                                new TVariant(FStatisticalReportPercentage[COLUMN_EXPARTICIPANTS]),
                                new TVariant(FStatisticalReportPercentage[COLUMN_CHURCH]),
                                new TVariant(FStatisticalReportPercentage[COLUMN_APPLICANTS]));
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Add a value to the existing value in the publication statistical report table
        /// </summary>
        /// <param name="RowIndex">row</param>
        /// <param name="ColumnName">column</param>
        /// <param name="AddedValue">value to add to the existing value</param>
        private void AddToStatisticalReportTable(int RowIndex, String ColumnName, int AddedValue)
        {
            if (ColumnName.Length > 0)
            {
                int OldValue = (int)FStatisticalReportDataTable.Rows[RowIndex][ColumnName];
                FStatisticalReportDataTable.Rows[RowIndex][ColumnName] = OldValue + AddedValue;
            }
        }

        /// <summary>
        /// Calculates the Row "Percent" and "Totals" for the publication statistical report
        /// </summary>
        private void CalculateTotals()
        {
            int NumColumns = FStatisticalReportDataTable.Columns.Count;
            int NumRows = FStatisticalReportDataTable.Rows.Count;

            for (int ColumnIndex = 1; ColumnIndex < NumColumns; ++ColumnIndex)
            {
                int Totals = 0;
                int RowIndex = 0;

                for (RowIndex = 0; RowIndex < NumRows - 3; ++RowIndex)
                {
                    Totals += (int)FStatisticalReportDataTable.Rows[RowIndex][ColumnIndex];
                }

                // Totals:
                FStatisticalReportDataTable.Rows[RowIndex + 1][ColumnIndex] = Totals;

                // Percent:
                double CalcPercent = Totals * 100.0 / FNumberOfActivePartner;

                FStatisticalReportDataTable.Rows[RowIndex][ColumnIndex] = CalcPercent;                 //.ToString("F");
                FStatisticalReportPercentage.Add(FStatisticalReportDataTable.Columns[ColumnIndex].ColumnName,
                    CalcPercent.ToString("F"));
            }
        }

        #endregion calculation for publication statistical report

        /// <summary>
        /// Returns a comma separated list of all partner types of a partner
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <returns></returns>
        private String GetPartnerTypes(Int64 APartnerKey)
        {
            PPartnerTypeTable PartnerType;
            String ReturnValue = "";

            PartnerType = PPartnerTypeAccess.LoadViaPPartner(APartnerKey, situation.GetDatabaseConnection().Transaction);

            foreach (PPartnerTypeRow Row in PartnerType.Rows)
            {
                ReturnValue = ReturnValue + Row.TypeCode + ',';
            }

            if (ReturnValue.Length > 0)
            {
                // Remove last comma
                ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 1);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Converts a 4GL integer value as a string time. The format parameter defines the output.
        /// AFormat = 1 "hh"
        /// AFormat = 2 "hh:mm"
        /// AFormat = 3 "hh:mm:ss"
        /// </summary>
        /// <param name="A4glTime"></param>
        /// <param name="AFormat"></param>
        /// <returns></returns>
        private string ConvertIntToTime(int A4glTime, int AFormat)
        {
            String ReturnValue = "";
            String Sseconds = "0";
            String Sminutes = "0";
            String Shours = "0";

            int seconds = A4glTime % 60;

            A4glTime = A4glTime / 60;
            int minutes = A4glTime % 60;
            int hours = A4glTime / 60;

            if (seconds < 10)
            {
                Sseconds += seconds.ToString();
            }
            else
            {
                Sseconds = seconds.ToString();
            }

            if (minutes < 10)
            {
                Sminutes += minutes.ToString();
            }
            else
            {
                Sminutes = minutes.ToString();
            }

            if (hours < 10)
            {
                Shours += hours.ToString();
            }
            else
            {
                Shours = hours.ToString();
            }

            switch (AFormat)
            {
                case 1:
                    ReturnValue = Shours;
                    break;

                case 2:
                    ReturnValue = Shours + ":" + Sminutes;
                    break;

                case 3:
                    ReturnValue = Shours + ":" + Sminutes + ":" + Sseconds;
                    break;
            }

            return ReturnValue;
        }

        private String GetProfession(Int64 APartnerKey)
        {
            PPersonTable PersonTable;
            POccupationTable OccupationTable;
            String Profession = "";

            PersonTable = PPersonAccess.LoadByPrimaryKey(APartnerKey, situation.GetDatabaseConnection().Transaction);

            if (PersonTable.Rows.Count > 0)
            {
                if (!((PPersonRow)PersonTable.Rows[0]).IsOccupationCodeNull())
                {
                    String OccupationCode = ((PPersonRow)PersonTable.Rows[0]).OccupationCode;

                    TPartnerCacheable PartnerCacheable = new TPartnerCacheable();
                    System.Type TypeOfTable;

                    OccupationTable = (POccupationTable)PartnerCacheable.GetCacheableTable(
                        TCacheablePartnerTablesEnum.OccupationList, "", false, out TypeOfTable);

                    DataRow[] OccupationRows = OccupationTable.Select(POccupationTable.GetOccupationCodeDBName() + " = '" + OccupationCode + "'");

                    if (OccupationRows.Length > 0)
                    {
                        Profession = ((POccupationRow)OccupationRows[0]).OccupationDescription;
                    }
                }
            }

            return Profession;
        }

        /// <summary>
        /// Get the Occupation Code and Occupation description from p_occupation table and stores them
        /// in the report variables OccupationDescription and Occupation
        /// </summary>
        /// <param name="AOccupationCode">The unique key to retrieve the data</param>
        /// <returns></returns>
        private bool GetOccupation(String AOccupationCode)
        {
            bool ReturnValue = false;

            if (AOccupationCode.Length > 0)
            {
                POccupationTable OccupationTable;

                OccupationTable = POccupationAccess.LoadByPrimaryKey(AOccupationCode, situation.GetDatabaseConnection().Transaction);

                if (OccupationTable.Rows.Count > 0)
                {
                    POccupationRow OccupationRow = (POccupationRow)OccupationTable.Rows[0];

                    situation.GetParameters().Add("OccupationDescription",
                        OccupationRow.OccupationDescription,
                        -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                    situation.GetParameters().Add("Occupation",
                        OccupationRow.OccupationCode,
                        -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                    ReturnValue = true;
                }
            }

            if (!ReturnValue)
            {
                situation.GetParameters().RemoveVariable("OccupationDescription", -1, -1, eParameterFit.eExact);
                situation.GetParameters().RemoveVariable("Occupation", -1, -1, eParameterFit.eExact);
            }

            return ReturnValue;
        }
    }
}
