/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using Ict.Petra.Server.MReporting;
using Ict.Petra.Shared.MReporting;
using Ict.Common;
using Ict.Common.DB;
using System.Diagnostics;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner;
using System.Collections;
using System.Collections.Specialized;
using System.Data.Odbc;
using System.Data;

namespace Ict.Petra.Server.MReporting.MPartner
{
    /// <summary>
    /// user defined functions for partner module
    /// </summary>
    public class TRptUserFunctionsPartner : TRptUserFunctions
    {
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

            if (StringHelper.IsSame(f, "AddressMeetsPostCodeCriteriaOrEmpty"))
            {
                value =
                    new TVariant(AddressMeetsPostCodeCriteriaOrEmpty(ops[1].ToBool(), ops[2].ToString(), ops[3].ToString(), ops[4].ToString(),
                            ops[5].ToString(), ops[6].ToString()));
                return true;
            }

            if (StringHelper.IsSame(f, "GetPartnerShortname"))
            {
                value = new TVariant(GetPartnerShortName(ops[1].ToInt64()));
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
            TVariant mRptCalcResult;
            mRptCalculation = situation.GetReportStore().GetCalculation(situation.GetCurrentReport(), "PartnerLabelValue");
            mRptDataCalcCalculation = new TRptDataCalcCalculation(situation);
            mRptCalcResult = mRptDataCalcCalculation.EvaluateCalculationAll(mRptCalculation,
                null,
                mRptCalculation.rptGrpTemplate,
                mRptCalculation.rptGrpQuery);

            if (mRptCalcResult.IsZeroOrNull())
            {
                return false;
            }

            mTable = situation.GetDatabaseConnection().SelectDT(mRptCalcResult.ToString(), "", situation.GetDatabaseConnection().Transaction);

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
                    // todo p_currency_code_c; using correct formatting
                    // TLogging.Log(TVariant.Create(mRow['LabelValueCurrency']).ToString());
                    // LabelValue := new TVariant(
                    // FormatCurrency(
                    // TVariant.Create(mRow['LabelValueCurrency']),
                    // '#,##0.00;#,##0.00;0.00;0'));
                    // if string comes in, TVariant converts it to a double, but leaves 0;
                    // adding the string of the currency code helps for the moment
                    LabelValue = new TVariant(new TVariant(mRow["LabelValueCurrency"]).ToString() + ' ' + mRow["CurrencyCode"].ToString());
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
                    PPartnerAccess.LoadByPrimaryKey(out PartnerTable, LabelValue.ToInt64(),
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
                    situation.GetParameters().RemoveVariable("LabelValue", col, situation.GetDepth(), eParameterFit.eBestFit);

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
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// this will find the best p_partner_location, and export the values to parameters (p_street_name_c, etc.)
        /// </summary>
        /// <returns>void</returns>
        private bool GetPartnerBestAddress(Int64 APartnerKey)
        {
            bool ReturnValue;
            DataSet PartnerLocationsDS;
            DataTable PartnerLocationTable;
            PLocationTable LocationTable;
            PFamilyTable FamilyTable;
            PPersonTable PersonTable;
            PPartnerTable PartnerTable;
            StringCollection NameColumnNames;

            ReturnValue = false;

            // reset the variables
            LocationTable = new PLocationTable();

            foreach (DataColumn col in LocationTable.Columns)
            {
                situation.GetParameters().RemoveVariable(StringHelper.UpperCamelCase(col.ColumnName, true, true));
            }

            PartnerLocationTable = new PPartnerLocationTable();

            foreach (DataColumn col in PartnerLocationTable.Columns)
            {
                situation.GetParameters().RemoveVariable(StringHelper.UpperCamelCase(col.ColumnName, true, true));
            }

            situation.GetParameters().RemoveVariable("FirstName");
            situation.GetParameters().RemoveVariable("FamilyName");
            PartnerLocationsDS = new DataSet();
            PartnerLocationsDS.Tables.Add(new PPartnerLocationTable());
            PartnerLocationTable = PartnerLocationsDS.Tables[PPartnerLocationTable.GetTableName()];

            // add special column BestAddress and Icon
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("BestAddress", typeof(Boolean)));
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("Icon", typeof(Int32)));

            // find all locations of the partner, put it into a dataset
            PPartnerLocationAccess.LoadViaPPartner(PartnerLocationsDS, APartnerKey, situation.GetDatabaseConnection().Transaction);

            // uses Ict.Petra.Shared.MPartner.Calculations.pas, DetermineBestAddress
            Calculations.DeterminePartnerLocationsDateStatus(PartnerLocationsDS);
            Calculations.DetermineBestAddress(PartnerLocationsDS);

            foreach (PPartnerLocationRow row in PartnerLocationTable.Rows)
            {
                // find the row with BestAddress = 1
                if (Convert.ToInt32(row["BestAddress"]) == 1)
                {
                    // find the location record with that address
                    PLocationAccess.LoadByPrimaryKey(out LocationTable, row.SiteKey, row.LocationKey, situation.GetDatabaseConnection().Transaction);

                    // put the found values in the parameters
                    if (LocationTable.Rows.Count > 0)
                    {
                        // get the location details into the parameters
                        foreach (DataColumn col in LocationTable.Columns)
                        {
                            situation.GetParameters().Add(StringHelper.UpperCamelCase(col.ColumnName, true,
                                    true), new TVariant(LocationTable.Rows[0][col.ColumnName]));
                        }

                        // also put the phone number and email etc into the parameters
                        foreach (DataColumn col in PartnerLocationTable.Columns)
                        {
                            situation.GetParameters().Add(StringHelper.UpperCamelCase(col.ColumnName, true,
                                    true), new TVariant(PartnerLocationTable.Rows[0][col.ColumnName]));
                        }

                        // get the Partner Firstname and Surname as well; depends on the partner class
                        // first try person
                        NameColumnNames = new StringCollection();
                        NameColumnNames.Add(PPersonTable.GetFirstNameDBName());
                        NameColumnNames.Add(PPersonTable.GetFamilyNameDBName());
                        PPersonAccess.LoadByPrimaryKey(out PersonTable, APartnerKey, NameColumnNames, situation.GetDatabaseConnection().Transaction);

                        if (PersonTable.Rows.Count > 0)
                        {
                            situation.GetParameters().Add("FirstName", new TVariant(PersonTable.Rows[0][PPersonTable.GetFirstNameDBName()]));
                            situation.GetParameters().Add("FamilyName", new TVariant(PersonTable.Rows[0][PPersonTable.GetFamilyNameDBName()]));
                        }
                        else
                        {
                            // then it was a family?
                            NameColumnNames = new StringCollection();
                            NameColumnNames.Add(PFamilyTable.GetFirstNameDBName());
                            NameColumnNames.Add(PFamilyTable.GetFamilyNameDBName());
                            PFamilyAccess.LoadByPrimaryKey(out FamilyTable, APartnerKey, NameColumnNames,
                                situation.GetDatabaseConnection().Transaction);

                            if (FamilyTable.Rows.Count > 0)
                            {
                                situation.GetParameters().Add("FirstName", new TVariant(FamilyTable.Rows[0][PFamilyTable.GetFirstNameDBName()]));
                                situation.GetParameters().Add("FamilyName", new TVariant(FamilyTable.Rows[0][PFamilyTable.GetFamilyNameDBName()]));
                            }
                            else
                            {
                                // it was an organisation or church, just use the shortname
                                situation.GetParameters().RemoveVariable("FirstName");
                                situation.GetParameters().RemoveVariable("FamilyName");
                                NameColumnNames = new StringCollection();
                                NameColumnNames.Add(PPartnerTable.GetPartnerShortNameDBName());
                                PPartnerAccess.LoadByPrimaryKey(out PartnerTable, APartnerKey, NameColumnNames,
                                    situation.GetDatabaseConnection().Transaction);

                                if (PartnerTable.Rows.Count > 0)
                                {
                                    situation.GetParameters().Add("FamilyName",
                                        new TVariant(PartnerTable.Rows[0][PPartnerTable.GetPartnerShortNameDBName()]));
                                }
                            }
                        }

                        ReturnValue = true;
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// this will return true if the best address is in the given postal region, postcode range and county it will not reset the data returned by best address
        /// </summary>
        /// <returns>void</returns>
        private bool AddressMeetsPostCodeCriteriaOrEmpty(bool ABestAddressWasFound,
            String ARadioSelection,
            String APostalRegion,
            String APostCodeFrom,
            String APostCodeTo,
            String ACounty)
        {
            bool ReturnValue;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            String postalCode;

            if (ARadioSelection != "DonorsCounty")
            {
                ACounty = "";
            }

            if (ARadioSelection != "DonorsPostalRegion")
            {
                APostalRegion = "";
            }

            if (ARadioSelection != "DonorsPostcode")
            {
                APostCodeFrom = "";
                APostCodeTo = "";
            }

            if (!ABestAddressWasFound)
            {
                // only return true if the postcode parameters and county parameters are empty
                return (APostalRegion.Length == 0) && (APostCodeFrom.Length == 0) && (APostCodeTo.Length == 0) && (ACounty.Length == 0);
            }

            ReturnValue = false;

            if (APostalRegion.Length > 0)
            {
                postalCode = situation.GetParameters().Get("PostalCode").ToString();
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted, out NewTransaction);
                try
                {
// todo, wrong combination of table/columns
//          TmpTable = DBAccess.GDBAccessObj.SelectDT("SELECT 1 " + " FROM PUB." + PPostcodeRangeTable.GetTableDBName() + " AS p " + " WHERE p.p_region_c = '" + APostalRegion + "' " + " AND p.p_from_c <= '" + postalCode + "' AND p.p_to_c >= '" +
// postalCode
// + "'", "temp", ReadTransaction);
//          if (TmpTable.Rows.Count > 0)
                    {
                        ReturnValue = true;
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                }
            }
            else if ((APostCodeFrom.Length > 0) && (APostCodeTo.Length > 0))
            {
                postalCode = situation.GetParameters().Get("PostalCode").ToString();

                if ((String.CompareOrdinal(APostCodeFrom.ToLower(),
                         postalCode.ToLower()) <= 0) && (String.CompareOrdinal(APostCodeTo.ToLower(), postalCode.ToLower()) >= 0))
                {
                    ReturnValue = true;
                }
            }
            else if (ACounty.Length > 0)
            {
                if (ACounty.ToLower() == situation.GetParameters().Get("County").ToString().ToLower())
                {
                    ReturnValue = true;
                }
            }

            return ReturnValue;
        }

        private String GetPartnerShortName(Int64 APartnerKey)
        {
            String ReturnValue;
            PPartnerTable table;
            StringCollection fields;

            ReturnValue = "N/A";
            fields = new StringCollection();
            fields.Add(PPartnerTable.GetPartnerShortNameDBName());
            PPartnerAccess.LoadByPrimaryKey(out table, APartnerKey, fields, situation.GetDatabaseConnection().Transaction);

            if (table.Rows.Count > 0)
            {
                ReturnValue = table[0].PartnerShortName;
            }

            return ReturnValue;
        }
    }
}