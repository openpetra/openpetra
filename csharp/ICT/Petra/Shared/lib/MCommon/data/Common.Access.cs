/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
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
namespace Ict.Petra.Shared.MCommon.Data.Access
{
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.Odbc;
    using Ict.Common;
    using Ict.Common.DB;
    using Ict.Common.Verification;
    using Ict.Common.Data;
    using Ict.Petra.Shared;
    using Ict.Petra.Shared.MCommon.Data;
    using Ict.Petra.Shared.MPartner.Partner.Data;
    using Ict.Petra.Shared.MPersonnel.Units.Data;
    using Ict.Petra.Shared.MPartner.Mailroom.Data;

    /// List of language codes
    public class PLanguageAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PLanguage";

        /// original table name in database
        public const string DBTABLENAME = "p_language";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_language_code_c"}) + " FROM PUB_p_language")
                            + GenerateOrderByClause(AOrderBy)), PLanguageTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PLanguageTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out PLanguageTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PLanguageTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ALanguageCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_language_code_c"}) + " FROM PUB_p_language WHERE p_language_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), PLanguageTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ALanguageCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PLanguageTable AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ALanguageCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PLanguageTable AData, String ALanguageCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PLanguageTable AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_language_code_c"}) + " FROM PUB_p_language")
                            + GenerateWhereClause(PLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), PLanguageTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PLanguageTable AData, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PLanguageTable AData, PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PLanguageTable AData, PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PLanguageTable AData, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_language_code_c"}) + " FROM PUB_p_language")
                            + GenerateWhereClause(PLanguageTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), PLanguageTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new PLanguageTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PLanguageTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_language", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String ALanguageCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ALanguageCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_language WHERE p_language_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_language" + GenerateWhereClause(PLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_language" + GenerateWhereClause(PLanguageTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new PLanguageTable(), ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_language", AFieldList, new string[] {
                            "p_language_code_c"}) + " FROM PUB_p_language, PUB_a_currency_language WHERE " +
                            "PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_a_currency_language.a_currency_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), PLanguageTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrency(out PLanguageTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrency(FillDataSet, ACurrencyCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaACurrency(out PLanguageTable AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrency(out PLanguageTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_language", AFieldList, new string[] {
                            "p_language_code_c"}) + " FROM PUB_p_language, PUB_a_currency_language, PUB_a_currency WHERE " +
                            "PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c")
                            + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), PLanguageTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out PLanguageTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrencyTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out PLanguageTable AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out PLanguageTable AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out PLanguageTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_language", AFieldList, new string[] {
                            "p_language_code_c"}) + " FROM PUB_p_language, PUB_a_currency_language, PUB_a_currency WHERE " +
                            "PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c")
                            + GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), PLanguageTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new PLanguageTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out PLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaACurrencyTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out PLanguageTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out PLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_language, PUB_a_currency_language WHERE " +
                        "PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_a_currency_language.a_currency_code_c = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_language, PUB_a_currency_language, PUB_a_currency WHERE " +
                        "PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c" +
                        GenerateWhereClauseLong("PUB_a_currency_language", PLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, ACurrencyTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_language, PUB_a_currency_language, PUB_a_currency WHERE " +
                        "PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c" +
                        GenerateWhereClauseLong("PUB_a_currency_language", PLanguageTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(new PLanguageTable(), ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPPerson(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_language", AFieldList, new string[] {
                            "p_language_code_c"}) + " FROM PUB_p_language, PUB_pm_person_language WHERE " +
                            "PUB_pm_person_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_pm_person_language.p_partner_key_n = ?")
                            + GenerateOrderByClause(AOrderBy)), PLanguageTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPerson(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPerson(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPerson(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPerson(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPerson(out PLanguageTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPerson(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPPerson(out PLanguageTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPerson(out PLanguageTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_language", AFieldList, new string[] {
                            "p_language_code_c"}) + " FROM PUB_p_language, PUB_pm_person_language, PUB_p_person WHERE " +
                            "PUB_pm_person_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_pm_person_language.p_partner_key_n = PUB_p_person.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_person", PPersonTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), PLanguageTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet AData, PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet AData, PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PLanguageTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPersonTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PLanguageTable AData, PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PLanguageTable AData, PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PLanguageTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_language", AFieldList, new string[] {
                            "p_language_code_c"}) + " FROM PUB_p_language, PUB_pm_person_language, PUB_p_person WHERE " +
                            "PUB_pm_person_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_pm_person_language.p_partner_key_n = PUB_p_person.p_partner_key_n")
                            + GenerateWhereClauseLong("PUB_p_person", PPersonTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), PLanguageTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new PLanguageTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPersonTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PLanguageTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_language, PUB_pm_person_language WHERE " +
                        "PUB_pm_person_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_pm_person_language.p_partner_key_n = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_language, PUB_pm_person_language, PUB_p_person WHERE " +
                        "PUB_pm_person_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_pm_person_language.p_partner_key_n = PUB_p_person.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_pm_person_language", PLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PPersonTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_language, PUB_pm_person_language, PUB_p_person WHERE " +
                        "PUB_pm_person_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_pm_person_language.p_partner_key_n = PUB_p_person.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_pm_person_language", PLanguageTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(new PLanguageTable(), ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaUmJob(DataSet ADataSet, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_language", AFieldList, new string[] {
                            "p_language_code_c"}) + " FROM PUB_p_language, PUB_um_job_language WHERE " +
                            "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_um_job_language.pm_unit_key_n = ? AND PUB_um_job_language.pt_position_name_c = ? AND PUB_um_job_language.pt_position_scope_c = ? AND PUB_um_job_language.um_job_key_i = ?")
                            + GenerateOrderByClause(AOrderBy)), PLanguageTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            LoadViaUmJob(AData, AUnitKey, APositionName, APositionScope, AJobKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(DataSet AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJob(AData, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(out PLanguageTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJob(FillDataSet, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJob(out PLanguageTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            LoadViaUmJob(out AData, AUnitKey, APositionName, APositionScope, AJobKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJob(out PLanguageTable AData, Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJob(out AData, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet ADataSet, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_language", AFieldList, new string[] {
                            "p_language_code_c"}) + " FROM PUB_p_language, PUB_um_job_language, PUB_um_job WHERE " +
                            "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i")
                            + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), PLanguageTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, UmJobRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, UmJobRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out PLanguageTable AData, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJobTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out PLanguageTable AData, UmJobRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out PLanguageTable AData, UmJobRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out PLanguageTable AData, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_language", AFieldList, new string[] {
                            "p_language_code_c"}) + " FROM PUB_p_language, PUB_um_job_language, PUB_um_job WHERE " +
                            "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i")
                            + GenerateWhereClauseLong("PUB_um_job", UmJobTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), PLanguageTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new PLanguageTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out PLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PLanguageTable();
            FillDataSet.Tables.Add(AData);
            LoadViaUmJobTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out PLanguageTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(out PLanguageTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaUmJobTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaUmJob(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[4];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AUnitKey));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 60);
            ParametersArray[1].Value = ((object)(APositionName));
            ParametersArray[2] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[2].Value = ((object)(APositionScope));
            ParametersArray[3] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[3].Value = ((object)(AJobKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_language, PUB_um_job_language WHERE " +
                        "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_um_job_language.pm_unit_key_n = ? AND PUB_um_job_language.pt_position_name_c = ? AND PUB_um_job_language.pt_position_scope_c = ? AND PUB_um_job_language.um_job_key_i = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaUmJobTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_language, PUB_um_job_language, PUB_um_job WHERE " +
                        "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i" +
                        GenerateWhereClauseLong("PUB_um_job_language", PLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, UmJobTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaUmJobTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_language, PUB_um_job_language, PUB_um_job WHERE " +
                        "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i" +
                        GenerateWhereClauseLong("PUB_um_job_language", PLanguageTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(new PLanguageTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ALanguageCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ALanguageCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_p_language WHERE p_language_code_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_p_language" + GenerateWhereClause(PLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_p_language" +
                GenerateWhereClause(PLanguageTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PLanguageTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(PLanguageTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("p_language", PLanguageTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("p_language", PLanguageTable.GetColumnStringList(), PLanguageTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("p_language", PLanguageTable.GetColumnStringList(), PLanguageTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PLanguage", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// Units of time. Used in partner letters.  Also used to indicate how often a publication is produced or a receipt is sent to a donor.
    public class AFrequencyAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AFrequency";

        /// original table name in database
        public const string DBTABLENAME = "a_frequency";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_frequency_code_c"}) + " FROM PUB_a_frequency")
                            + GenerateOrderByClause(AOrderBy)), AFrequencyTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AFrequencyTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AFrequencyTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out AFrequencyTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AFrequencyTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AFrequencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[0].Value = ((object)(AFrequencyCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_frequency_code_c"}) + " FROM PUB_a_frequency WHERE a_frequency_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), AFrequencyTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AFrequencyCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AFrequencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AFrequencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AFrequencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AFrequencyTable AData, String AFrequencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AFrequencyTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AFrequencyCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AFrequencyTable AData, String AFrequencyCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AFrequencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AFrequencyTable AData, String AFrequencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AFrequencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_frequency_code_c"}) + " FROM PUB_a_frequency")
                            + GenerateWhereClause(AFrequencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), AFrequencyTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AFrequencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AFrequencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AFrequencyTable AData, AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AFrequencyTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AFrequencyTable AData, AFrequencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AFrequencyTable AData, AFrequencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AFrequencyTable AData, AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_frequency_code_c"}) + " FROM PUB_a_frequency")
                            + GenerateWhereClause(AFrequencyTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), AFrequencyTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new AFrequencyTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AFrequencyTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new AFrequencyTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AFrequencyTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AFrequencyTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_frequency", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String AFrequencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[0].Value = ((object)(AFrequencyCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_frequency WHERE a_frequency_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_frequency" + GenerateWhereClause(AFrequencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_frequency" + GenerateWhereClause(AFrequencyTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new AFrequencyTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AFrequencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 24);
            ParametersArray[0].Value = ((object)(AFrequencyCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_frequency WHERE a_frequency_code_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_frequency" + GenerateWhereClause(AFrequencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_frequency" +
                GenerateWhereClause(AFrequencyTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new AFrequencyTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(AFrequencyTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("a_frequency", AFrequencyTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_frequency", AFrequencyTable.GetColumnStringList(), AFrequencyTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_frequency", AFrequencyTable.GetColumnStringList(), AFrequencyTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table AFrequency", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// Post office mailing zone classification
    public class PInternationalPostalTypeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PInternationalPostalType";

        /// original table name in database
        public const string DBTABLENAME = "p_international_postal_type";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_internat_postal_type_code_c"}) + " FROM PUB_p_international_postal_type")
                            + GenerateOrderByClause(AOrderBy)), PInternationalPostalTypeTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PInternationalPostalTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PInternationalPostalTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out PInternationalPostalTypeTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PInternationalPostalTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(AInternatPostalTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_internat_postal_type_code_c"}) + " FROM PUB_p_international_postal_type WHERE p_internat_postal_type_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), PInternationalPostalTypeTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AInternatPostalTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AInternatPostalTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AInternatPostalTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PInternationalPostalTypeTable AData, String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PInternationalPostalTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, AInternatPostalTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PInternationalPostalTypeTable AData, String AInternatPostalTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AInternatPostalTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PInternationalPostalTypeTable AData, String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AInternatPostalTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_internat_postal_type_code_c"}) + " FROM PUB_p_international_postal_type")
                            + GenerateWhereClause(PInternationalPostalTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), PInternationalPostalTypeTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PInternationalPostalTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PInternationalPostalTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PInternationalPostalTypeTable AData, PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PInternationalPostalTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PInternationalPostalTypeTable AData, PInternationalPostalTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PInternationalPostalTypeTable AData, PInternationalPostalTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PInternationalPostalTypeTable AData, PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_internat_postal_type_code_c"}) + " FROM PUB_p_international_postal_type")
                            + GenerateWhereClause(PInternationalPostalTypeTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), PInternationalPostalTypeTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new PInternationalPostalTypeTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PInternationalPostalTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PInternationalPostalTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PInternationalPostalTypeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PInternationalPostalTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_international_postal_type", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String AInternatPostalTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(AInternatPostalTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_international_postal_type WHERE p_internat_postal_type_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_international_postal_type" + GenerateWhereClause(PInternationalPostalTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_international_postal_type" + GenerateWhereClause(PInternationalPostalTypeTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new PInternationalPostalTypeTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AInternatPostalTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(AInternatPostalTypeCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_p_international_postal_type WHERE p_internat_postal_type_code_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_p_international_postal_type" + GenerateWhereClause(PInternationalPostalTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_p_international_postal_type" +
                GenerateWhereClause(PInternationalPostalTypeTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PInternationalPostalTypeTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(PInternationalPostalTypeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("p_international_postal_type", PInternationalPostalTypeTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("p_international_postal_type", PInternationalPostalTypeTable.GetColumnStringList(), PInternationalPostalTypeTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("p_international_postal_type", PInternationalPostalTypeTable.GetColumnStringList(), PInternationalPostalTypeTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PInternationalPostalType", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// List of countries with their codes
    public class PCountryAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PCountry";

        /// original table name in database
        public const string DBTABLENAME = "p_country";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_country_code_c"}) + " FROM PUB_p_country")
                            + GenerateOrderByClause(AOrderBy)), PCountryTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PCountryTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PCountryTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out PCountryTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PCountryTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 8);
            ParametersArray[0].Value = ((object)(ACountryCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_country_code_c"}) + " FROM PUB_p_country WHERE p_country_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), PCountryTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACountryCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACountryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACountryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PCountryTable AData, String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PCountryTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ACountryCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PCountryTable AData, String ACountryCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ACountryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PCountryTable AData, String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ACountryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_country_code_c"}) + " FROM PUB_p_country")
                            + GenerateWhereClause(PCountryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), PCountryTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PCountryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PCountryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PCountryTable AData, PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PCountryTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PCountryTable AData, PCountryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PCountryTable AData, PCountryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PCountryTable AData, PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "p_country_code_c"}) + " FROM PUB_p_country")
                            + GenerateWhereClause(PCountryTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), PCountryTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new PCountryTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PCountryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PCountryTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PCountryTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PCountryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_country", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String ACountryCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 8);
            ParametersArray[0].Value = ((object)(ACountryCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_country WHERE p_country_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_country" + GenerateWhereClause(PCountryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_country" + GenerateWhereClause(PCountryTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new PCountryTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPInternationalPostalType(DataSet ADataSet, String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(AInternatPostalTypeCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "p_country_code_c"}) + " FROM PUB_p_country WHERE p_internat_postal_type_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), PCountryTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalType(DataSet AData, String AInternatPostalTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPInternationalPostalType(AData, AInternatPostalTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalType(DataSet AData, String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPInternationalPostalType(AData, AInternatPostalTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalType(out PCountryTable AData, String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PCountryTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPInternationalPostalType(FillDataSet, AInternatPostalTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalType(out PCountryTable AData, String AInternatPostalTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPInternationalPostalType(out AData, AInternatPostalTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalType(out PCountryTable AData, String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPInternationalPostalType(out AData, AInternatPostalTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(DataSet ADataSet, PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_country", AFieldList, new string[] {
                            "p_country_code_c"}) + " FROM PUB_p_country, PUB_p_international_postal_type WHERE " +
                            "PUB_p_country.p_internat_postal_type_code_c = PUB_p_international_postal_type.p_internat_postal_type_code_c")
                            + GenerateWhereClauseLong("PUB_p_international_postal_type", PInternationalPostalTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), PCountryTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(DataSet AData, PInternationalPostalTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPInternationalPostalTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(DataSet AData, PInternationalPostalTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPInternationalPostalTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(out PCountryTable AData, PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PCountryTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPInternationalPostalTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(out PCountryTable AData, PInternationalPostalTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPInternationalPostalTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(out PCountryTable AData, PInternationalPostalTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPInternationalPostalTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(out PCountryTable AData, PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPInternationalPostalTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_country", AFieldList, new string[] {
                            "p_country_code_c"}) + " FROM PUB_p_country, PUB_p_international_postal_type WHERE " +
                            "PUB_p_country.p_internat_postal_type_code_c = PUB_p_international_postal_type.p_internat_postal_type_code_c")
                            + GenerateWhereClauseLong("PUB_p_international_postal_type", PInternationalPostalTypeTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), PCountryTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new PCountryTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPInternationalPostalTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPInternationalPostalTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(out PCountryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PCountryTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPInternationalPostalTypeTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(out PCountryTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPInternationalPostalTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(out PCountryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPInternationalPostalTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPInternationalPostalType(String AInternatPostalTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(AInternatPostalTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_country WHERE p_internat_postal_type_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPInternationalPostalTypeTemplate(PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_country, PUB_p_international_postal_type WHERE " +
                "PUB_p_country.p_internat_postal_type_code_c = PUB_p_international_postal_type.p_internat_postal_type_code_c" + GenerateWhereClauseLong("PUB_p_international_postal_type", PInternationalPostalTypeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PInternationalPostalTypeTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPInternationalPostalTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_country, PUB_p_international_postal_type WHERE " +
                "PUB_p_country.p_internat_postal_type_code_c = PUB_p_international_postal_type.p_internat_postal_type_code_c" +
                GenerateWhereClauseLong("PUB_p_international_postal_type", PInternationalPostalTypeTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PInternationalPostalTypeTable(), ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPAddressLayoutCode(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(ACode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_country", AFieldList, new string[] {
                            "p_country_code_c"}) + " FROM PUB_p_country, PUB_p_address_layout WHERE " +
                            "PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c AND PUB_p_address_layout.p_address_layout_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), PCountryTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCode(DataSet AData, String ACode, TDBTransaction ATransaction)
        {
            LoadViaPAddressLayoutCode(AData, ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCode(DataSet AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPAddressLayoutCode(AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCode(out PCountryTable AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PCountryTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPAddressLayoutCode(FillDataSet, ACode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCode(out PCountryTable AData, String ACode, TDBTransaction ATransaction)
        {
            LoadViaPAddressLayoutCode(out AData, ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCode(out PCountryTable AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPAddressLayoutCode(out AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(DataSet ADataSet, PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_country", AFieldList, new string[] {
                            "p_country_code_c"}) + " FROM PUB_p_country, PUB_p_address_layout, PUB_p_address_layout_code WHERE " +
                            "PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c AND PUB_p_address_layout.p_address_layout_code_c = PUB_p_address_layout_code.p_code_c")
                            + GenerateWhereClauseLong("PUB_p_address_layout_code", PAddressLayoutCodeTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), PCountryTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(DataSet AData, PAddressLayoutCodeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPAddressLayoutCodeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(DataSet AData, PAddressLayoutCodeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPAddressLayoutCodeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(out PCountryTable AData, PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PCountryTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPAddressLayoutCodeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(out PCountryTable AData, PAddressLayoutCodeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPAddressLayoutCodeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(out PCountryTable AData, PAddressLayoutCodeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPAddressLayoutCodeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(out PCountryTable AData, PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPAddressLayoutCodeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_country", AFieldList, new string[] {
                            "p_country_code_c"}) + " FROM PUB_p_country, PUB_p_address_layout, PUB_p_address_layout_code WHERE " +
                            "PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c AND PUB_p_address_layout.p_address_layout_code_c = PUB_p_address_layout_code.p_code_c")
                            + GenerateWhereClauseLong("PUB_p_address_layout_code", PAddressLayoutCodeTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), PCountryTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new PCountryTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPAddressLayoutCodeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPAddressLayoutCodeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(out PCountryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PCountryTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPAddressLayoutCodeTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(out PCountryTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPAddressLayoutCodeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(out PCountryTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPAddressLayoutCodeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPAddressLayoutCode(String ACode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(ACode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_country, PUB_p_address_layout WHERE " +
                        "PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c AND PUB_p_address_layout.p_address_layout_code_c = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_country, PUB_p_address_layout, PUB_p_address_layout_code WHERE " +
                        "PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c AND PUB_p_address_layout.p_address_layout_code_c = PUB_p_address_layout_code.p_code_c" +
                        GenerateWhereClauseLong("PUB_p_address_layout", PCountryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PAddressLayoutCodeTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPAddressLayoutCodeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_country, PUB_p_address_layout, PUB_p_address_layout_code WHERE " +
                        "PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c AND PUB_p_address_layout.p_address_layout_code_c = PUB_p_address_layout_code.p_code_c" +
                        GenerateWhereClauseLong("PUB_p_address_layout", PCountryTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(new PCountryTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ACountryCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 8);
            ParametersArray[0].Value = ((object)(ACountryCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_p_country WHERE p_country_code_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_p_country" + GenerateWhereClause(PCountryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_p_country" +
                GenerateWhereClause(PCountryTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PCountryTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(PCountryTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("p_country", PCountryTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("p_country", PCountryTable.GetColumnStringList(), PCountryTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("p_country", PCountryTable.GetColumnStringList(), PCountryTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table PCountry", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }

    /// Unit of money for various countries.
    public class ACurrencyAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "ACurrency";

        /// original table name in database
        public const string DBTABLENAME = "a_currency";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_currency_code_c"}) + " FROM PUB_a_currency")
                            + GenerateOrderByClause(AOrderBy)), ACurrencyTable.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
        {
            LoadAll(AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out ACurrencyTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACurrencyTable();
            FillDataSet.Tables.Add(AData);
            LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadAll(out ACurrencyTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out ACurrencyTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_currency_code_c"}) + " FROM PUB_a_currency WHERE a_currency_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), ACurrencyTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out ACurrencyTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACurrencyTable();
            FillDataSet.Tables.Add(AData);
            LoadByPrimaryKey(FillDataSet, ACurrencyCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out ACurrencyTable AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out ACurrencyTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_currency_code_c"}) + " FROM PUB_a_currency")
                            + GenerateWhereClause(ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), ACurrencyTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ACurrencyTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACurrencyTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ACurrencyTable AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ACurrencyTable AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ACurrencyTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                            "a_currency_code_c"}) + " FROM PUB_a_currency")
                            + GenerateWhereClause(ACurrencyTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), ACurrencyTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new ACurrencyTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ACurrencyTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACurrencyTable();
            FillDataSet.Tables.Add(AData);
            LoadUsingTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ACurrencyTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ACurrencyTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_currency", ATransaction, false));
        }

        /// this method is called by all overloads
        public static int CountByPrimaryKey(String ACurrencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_currency WHERE a_currency_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_currency" + GenerateWhereClause(ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_currency" + GenerateWhereClause(ACurrencyTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(new ACurrencyTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPCountry(DataSet ADataSet, String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 8);
            ParametersArray[0].Value = ((object)(ACountryCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                            "a_currency_code_c"}) + " FROM PUB_a_currency WHERE p_country_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), ACurrencyTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPCountry(DataSet AData, String ACountryCode, TDBTransaction ATransaction)
        {
            LoadViaPCountry(AData, ACountryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountry(DataSet AData, String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPCountry(AData, ACountryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountry(out ACurrencyTable AData, String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACurrencyTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPCountry(FillDataSet, ACountryCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPCountry(out ACurrencyTable AData, String ACountryCode, TDBTransaction ATransaction)
        {
            LoadViaPCountry(out AData, ACountryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountry(out ACurrencyTable AData, String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPCountry(out AData, ACountryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet ADataSet, PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_currency", AFieldList, new string[] {
                            "a_currency_code_c"}) + " FROM PUB_a_currency, PUB_p_country WHERE " +
                            "PUB_a_currency.p_country_code_c = PUB_p_country.p_country_code_c")
                            + GenerateWhereClauseLong("PUB_p_country", PCountryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), ACurrencyTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet AData, PCountryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPCountryTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet AData, PCountryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPCountryTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(out ACurrencyTable AData, PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACurrencyTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPCountryTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(out ACurrencyTable AData, PCountryRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPCountryTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(out ACurrencyTable AData, PCountryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPCountryTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(out ACurrencyTable AData, PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPCountryTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_currency", AFieldList, new string[] {
                            "a_currency_code_c"}) + " FROM PUB_a_currency, PUB_p_country WHERE " +
                            "PUB_a_currency.p_country_code_c = PUB_p_country.p_country_code_c")
                            + GenerateWhereClauseLong("PUB_p_country", PCountryTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), ACurrencyTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new ACurrencyTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPCountryTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPCountryTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(out ACurrencyTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACurrencyTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPCountryTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(out ACurrencyTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPCountryTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(out ACurrencyTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPCountryTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPCountry(String ACountryCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 8);
            ParametersArray[0].Value = ((object)(ACountryCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_currency WHERE p_country_code_c = ?", ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_currency, PUB_p_country WHERE " +
                "PUB_a_currency.p_country_code_c = PUB_p_country.p_country_code_c" + GenerateWhereClauseLong("PUB_p_country", PCountryTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PCountryTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_currency, PUB_p_country WHERE " +
                "PUB_a_currency.p_country_code_c = PUB_p_country.p_country_code_c" +
                GenerateWhereClauseLong("PUB_p_country", PCountryTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new PCountryTable(), ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPLanguage(DataSet ADataSet, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ALanguageCode));
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_currency", AFieldList, new string[] {
                            "a_currency_code_c"}) + " FROM PUB_a_currency, PUB_a_currency_language WHERE " +
                            "PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c AND PUB_a_currency_language.p_language_code_c = ?")
                            + GenerateOrderByClause(AOrderBy)), ACurrencyTable.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet AData, String ALanguageCode, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(AData, ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(AData, ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguage(out ACurrencyTable AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACurrencyTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPLanguage(FillDataSet, ALanguageCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPLanguage(out ACurrencyTable AData, String ALanguageCode, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(out AData, ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguage(out ACurrencyTable AData, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguage(out AData, ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_currency", AFieldList, new string[] {
                            "a_currency_code_c"}) + " FROM PUB_a_currency, PUB_a_currency_language, PUB_p_language WHERE " +
                            "PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c AND PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c")
                            + GenerateWhereClauseLong("PUB_p_language", PLanguageTable.GetColumnStringList(), ATemplateRow, ATemplateOperators))
                            + GenerateOrderByClause(AOrderBy)), ACurrencyTable.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out ACurrencyTable AData, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACurrencyTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPLanguageTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out ACurrencyTable AData, PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out ACurrencyTable AData, PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out ACurrencyTable AData, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_currency", AFieldList, new string[] {
                            "a_currency_code_c"}) + " FROM PUB_a_currency, PUB_a_currency_language, PUB_p_language WHERE " +
                            "PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c AND PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c")
                            + GenerateWhereClauseLong("PUB_p_language", PLanguageTable.GetColumnStringList(), ASearchCriteria))
                            + GenerateOrderByClause(AOrderBy)), ACurrencyTable.GetTableName(), ATransaction,
                            GetParametersForWhereClause(new ACurrencyTable(), ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out ACurrencyTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new ACurrencyTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPLanguageTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out ACurrencyTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(out ACurrencyTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLanguageTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPLanguage(String ALanguageCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ALanguageCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_currency, PUB_a_currency_language WHERE " +
                        "PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c AND PUB_a_currency_language.p_language_code_c = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_currency, PUB_a_currency_language, PUB_p_language WHERE " +
                        "PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c AND PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c" +
                        GenerateWhereClauseLong("PUB_a_currency_language", ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, PLanguageTable.GetPrimKeyColumnOrdList())));
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_currency, PUB_a_currency_language, PUB_p_language WHERE " +
                        "PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c AND PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c" +
                        GenerateWhereClauseLong("PUB_a_currency_language", ACurrencyTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(new ACurrencyTable(), ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ACurrencyCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_a_currency WHERE a_currency_code_c = ?", ATransaction, false, ParametersArray);
        }

        /// auto generated
        public static void DeleteUsingTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_currency" + GenerateWhereClause(ACurrencyTable.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_a_currency" +
                GenerateWhereClause(ACurrencyTable.GetColumnStringList(), ASearchCriteria)), ATransaction, false,
                GetParametersForWhereClause(new ACurrencyTable(), ASearchCriteria));
        }

        /// auto generated
        public static bool SubmitChanges(ACurrencyTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            bool ResultValue = true;
            bool ExceptionReported = false;
            DataRow TheRow = null;
            AVerificationResult = new TVerificationResultCollection();
            for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
            {
                TheRow = ATable[RowCount];
                try
                {
                    if ((TheRow.RowState == DataRowState.Added))
                    {
                        TTypedDataAccess.InsertRow("a_currency", ACurrencyTable.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Modified))
                    {
                        TTypedDataAccess.UpdateRow("a_currency", ACurrencyTable.GetColumnStringList(), ACurrencyTable.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                    }
                    if ((TheRow.RowState == DataRowState.Deleted))
                    {
                        TTypedDataAccess.DeleteRow("a_currency", ACurrencyTable.GetColumnStringList(), ACurrencyTable.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
                    }
                }
                catch (OdbcException ex)
                {
                    ResultValue = false;
                    ExceptionReported = false;
                    if ((ExceptionReported == false))
                    {
                        AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table ACurrency", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                    }
                }
            }
            return ResultValue;
        }
    }
}