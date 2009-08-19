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
namespace Ict.Petra.Server.MCommon.Data.Access
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PLanguageTable.TableId) + " FROM PUB_p_language") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PLanguageTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PLanguageTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PLanguageTable Data = new PLanguageTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PLanguageTable.TableId) + " FROM PUB_p_language" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PLanguageTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PLanguageTable.TableId, ADataSet, new System.Object[1]{ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PLanguageTable LoadByPrimaryKey(String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PLanguageTable Data = new PLanguageTable();
            LoadByPrimaryKey(PLanguageTable.TableId, Data, new System.Object[1]{ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PLanguageTable LoadByPrimaryKey(String ALanguageCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadByPrimaryKey(String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PLanguageTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PLanguageTable LoadUsingTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PLanguageTable Data = new PLanguageTable();
            LoadUsingTemplate(PLanguageTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PLanguageTable LoadUsingTemplate(PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadUsingTemplate(PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadUsingTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PLanguageTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PLanguageTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PLanguageTable Data = new PLanguageTable();
            LoadUsingTemplate(PLanguageTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PLanguageTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_language", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ALanguageCode, TDBTransaction ATransaction)
        {
            return Exists(PLanguageTable.TableId, new System.Object[1]{ALanguageCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_language" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PLanguageTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PLanguageTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_language" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PLanguageTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PLanguageTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 16);
            ParametersArray[0].Value = ((object)(ACurrencyCode));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_language", AFieldList, PLanguageTable.TableId) +
                            " FROM PUB_p_language, PUB_a_currency_language WHERE " +
                            "PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_a_currency_language.a_currency_code_c = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PLanguageTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static PLanguageTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PLanguageTable Data = new PLanguageTable();
            FillDataSet.Tables.Add(Data);
            LoadViaACurrency(FillDataSet, ACurrencyCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PLanguageTable LoadViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_language", AFieldList, PLanguageTable.TableId) +
                            " FROM PUB_p_language, PUB_a_currency_language, PUB_a_currency WHERE " +
                            "PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c") +
                            GenerateWhereClauseLong("PUB_a_currency", ACurrencyTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PLanguageTable.TableId), ATransaction,
                            GetParametersForWhereClause(ACurrencyTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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
        public static PLanguageTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PLanguageTable Data = new PLanguageTable();
            FillDataSet.Tables.Add(Data);
            LoadViaACurrencyTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PLanguageTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_language", AFieldList, PLanguageTable.TableId) +
                            " FROM PUB_p_language, PUB_a_currency_language, PUB_a_currency WHERE " +
                            "PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c") +
                            GenerateWhereClauseLong("PUB_a_currency", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PLanguageTable.TableId), ATransaction,
                            GetParametersForWhereClause(PLanguageTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
        public static PLanguageTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PLanguageTable Data = new PLanguageTable();
            FillDataSet.Tables.Add(Data);
            LoadViaACurrencyTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PLanguageTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
                        GenerateWhereClauseLong("PUB_a_currency_language", PLanguageTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(ACurrencyTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_language, PUB_a_currency_language, PUB_a_currency WHERE " +
                        "PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c" +
                        GenerateWhereClauseLong("PUB_a_currency_language", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PLanguageTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPPerson(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_language", AFieldList, PLanguageTable.TableId) +
                            " FROM PUB_p_language, PUB_pm_person_language WHERE " +
                            "PUB_pm_person_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_pm_person_language.p_partner_key_n = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PLanguageTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static PLanguageTable LoadViaPPerson(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PLanguageTable Data = new PLanguageTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPerson(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PLanguageTable LoadViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPerson(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadViaPPerson(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPerson(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_language", AFieldList, PLanguageTable.TableId) +
                            " FROM PUB_p_language, PUB_pm_person_language, PUB_p_person WHERE " +
                            "PUB_pm_person_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_pm_person_language.p_partner_key_n = PUB_p_person.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_person", PPersonTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PLanguageTable.TableId), ATransaction,
                            GetParametersForWhereClause(PPersonTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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
        public static PLanguageTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PLanguageTable Data = new PLanguageTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPersonTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PLanguageTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_language", AFieldList, PLanguageTable.TableId) +
                            " FROM PUB_p_language, PUB_pm_person_language, PUB_p_person WHERE " +
                            "PUB_pm_person_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_pm_person_language.p_partner_key_n = PUB_p_person.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_person", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PLanguageTable.TableId), ATransaction,
                            GetParametersForWhereClause(PLanguageTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
        public static PLanguageTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PLanguageTable Data = new PLanguageTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPersonTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PLanguageTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
                        GenerateWhereClauseLong("PUB_pm_person_language", PLanguageTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PPersonTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_language, PUB_pm_person_language, PUB_p_person WHERE " +
                        "PUB_pm_person_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_pm_person_language.p_partner_key_n = PUB_p_person.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_pm_person_language", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PLanguageTable.TableId, ASearchCriteria)));
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_language", AFieldList, PLanguageTable.TableId) +
                            " FROM PUB_p_language, PUB_um_job_language WHERE " +
                            "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_um_job_language.pm_unit_key_n = ? AND PUB_um_job_language.pt_position_name_c = ? AND PUB_um_job_language.pt_position_scope_c = ? AND PUB_um_job_language.um_job_key_i = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PLanguageTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static PLanguageTable LoadViaUmJob(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PLanguageTable Data = new PLanguageTable();
            FillDataSet.Tables.Add(Data);
            LoadViaUmJob(FillDataSet, AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PLanguageTable LoadViaUmJob(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, TDBTransaction ATransaction)
        {
            return LoadViaUmJob(AUnitKey, APositionName, APositionScope, AJobKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadViaUmJob(Int64 AUnitKey, String APositionName, String APositionScope, Int32 AJobKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaUmJob(AUnitKey, APositionName, APositionScope, AJobKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet ADataSet, UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_language", AFieldList, PLanguageTable.TableId) +
                            " FROM PUB_p_language, PUB_um_job_language, PUB_um_job WHERE " +
                            "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i") +
                            GenerateWhereClauseLong("PUB_um_job", UmJobTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PLanguageTable.TableId), ATransaction,
                            GetParametersForWhereClause(UmJobTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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
        public static PLanguageTable LoadViaUmJobTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PLanguageTable Data = new PLanguageTable();
            FillDataSet.Tables.Add(Data);
            LoadViaUmJobTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PLanguageTable LoadViaUmJobTemplate(UmJobRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaUmJobTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadViaUmJobTemplate(UmJobRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaUmJobTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadViaUmJobTemplate(UmJobRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaUmJobTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaUmJobTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_language", AFieldList, PLanguageTable.TableId) +
                            " FROM PUB_p_language, PUB_um_job_language, PUB_um_job WHERE " +
                            "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i") +
                            GenerateWhereClauseLong("PUB_um_job", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PLanguageTable.TableId), ATransaction,
                            GetParametersForWhereClause(PLanguageTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
        public static PLanguageTable LoadViaUmJobTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PLanguageTable Data = new PLanguageTable();
            FillDataSet.Tables.Add(Data);
            LoadViaUmJobTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PLanguageTable LoadViaUmJobTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaUmJobTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLanguageTable LoadViaUmJobTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaUmJobTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
                        GenerateWhereClauseLong("PUB_um_job_language", PLanguageTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(UmJobTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaUmJobTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_language, PUB_um_job_language, PUB_um_job WHERE " +
                        "PUB_um_job_language.p_language_code_c = PUB_p_language.p_language_code_c AND PUB_um_job_language.pm_unit_key_n = PUB_um_job.pm_unit_key_n AND PUB_um_job_language.pt_position_name_c = PUB_um_job.pt_position_name_c AND PUB_um_job_language.pt_position_scope_c = PUB_um_job.pt_position_scope_c AND PUB_um_job_language.um_job_key_i = PUB_um_job.um_job_key_i" +
                        GenerateWhereClauseLong("PUB_um_job_language", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PLanguageTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ALanguageCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PLanguageTable.TableId, new System.Object[1]{ALanguageCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PLanguageTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PLanguageTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PLanguageTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PLanguageTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AFrequencyTable.TableId) + " FROM PUB_a_frequency") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AFrequencyTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AFrequencyTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AFrequencyTable Data = new AFrequencyTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AFrequencyTable.TableId) + " FROM PUB_a_frequency" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AFrequencyTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AFrequencyTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AFrequencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AFrequencyTable.TableId, ADataSet, new System.Object[1]{AFrequencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AFrequencyTable LoadByPrimaryKey(String AFrequencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AFrequencyTable Data = new AFrequencyTable();
            LoadByPrimaryKey(AFrequencyTable.TableId, Data, new System.Object[1]{AFrequencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AFrequencyTable LoadByPrimaryKey(String AFrequencyCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AFrequencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AFrequencyTable LoadByPrimaryKey(String AFrequencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AFrequencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AFrequencyTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AFrequencyTable LoadUsingTemplate(AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AFrequencyTable Data = new AFrequencyTable();
            LoadUsingTemplate(AFrequencyTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AFrequencyTable LoadUsingTemplate(AFrequencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AFrequencyTable LoadUsingTemplate(AFrequencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AFrequencyTable LoadUsingTemplate(AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AFrequencyTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AFrequencyTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AFrequencyTable Data = new AFrequencyTable();
            LoadUsingTemplate(AFrequencyTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AFrequencyTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AFrequencyTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_frequency", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AFrequencyCode, TDBTransaction ATransaction)
        {
            return Exists(AFrequencyTable.TableId, new System.Object[1]{AFrequencyCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_frequency" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AFrequencyTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AFrequencyTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_frequency" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AFrequencyTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AFrequencyTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AFrequencyCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AFrequencyTable.TableId, new System.Object[1]{AFrequencyCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AFrequencyTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AFrequencyTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AFrequencyTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AFrequencyTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PInternationalPostalTypeTable.TableId) + " FROM PUB_p_international_postal_type") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PInternationalPostalTypeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PInternationalPostalTypeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PInternationalPostalTypeTable Data = new PInternationalPostalTypeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PInternationalPostalTypeTable.TableId) + " FROM PUB_p_international_postal_type" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PInternationalPostalTypeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PInternationalPostalTypeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PInternationalPostalTypeTable.TableId, ADataSet, new System.Object[1]{AInternatPostalTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PInternationalPostalTypeTable LoadByPrimaryKey(String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PInternationalPostalTypeTable Data = new PInternationalPostalTypeTable();
            LoadByPrimaryKey(PInternationalPostalTypeTable.TableId, Data, new System.Object[1]{AInternatPostalTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PInternationalPostalTypeTable LoadByPrimaryKey(String AInternatPostalTypeCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AInternatPostalTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PInternationalPostalTypeTable LoadByPrimaryKey(String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AInternatPostalTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PInternationalPostalTypeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PInternationalPostalTypeTable LoadUsingTemplate(PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PInternationalPostalTypeTable Data = new PInternationalPostalTypeTable();
            LoadUsingTemplate(PInternationalPostalTypeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PInternationalPostalTypeTable LoadUsingTemplate(PInternationalPostalTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PInternationalPostalTypeTable LoadUsingTemplate(PInternationalPostalTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PInternationalPostalTypeTable LoadUsingTemplate(PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PInternationalPostalTypeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PInternationalPostalTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PInternationalPostalTypeTable Data = new PInternationalPostalTypeTable();
            LoadUsingTemplate(PInternationalPostalTypeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PInternationalPostalTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PInternationalPostalTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_international_postal_type", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AInternatPostalTypeCode, TDBTransaction ATransaction)
        {
            return Exists(PInternationalPostalTypeTable.TableId, new System.Object[1]{AInternatPostalTypeCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_international_postal_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PInternationalPostalTypeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PInternationalPostalTypeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_international_postal_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PInternationalPostalTypeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PInternationalPostalTypeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AInternatPostalTypeCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PInternationalPostalTypeTable.TableId, new System.Object[1]{AInternatPostalTypeCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PInternationalPostalTypeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PInternationalPostalTypeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PInternationalPostalTypeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PInternationalPostalTypeTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PCountryTable.TableId) + " FROM PUB_p_country") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PCountryTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PCountryTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCountryTable Data = new PCountryTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PCountryTable.TableId) + " FROM PUB_p_country" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCountryTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCountryTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PCountryTable.TableId, ADataSet, new System.Object[1]{ACountryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PCountryTable LoadByPrimaryKey(String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCountryTable Data = new PCountryTable();
            LoadByPrimaryKey(PCountryTable.TableId, Data, new System.Object[1]{ACountryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCountryTable LoadByPrimaryKey(String ACountryCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACountryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCountryTable LoadByPrimaryKey(String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACountryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PCountryTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PCountryTable LoadUsingTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCountryTable Data = new PCountryTable();
            LoadUsingTemplate(PCountryTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCountryTable LoadUsingTemplate(PCountryRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCountryTable LoadUsingTemplate(PCountryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCountryTable LoadUsingTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PCountryTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PCountryTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCountryTable Data = new PCountryTable();
            LoadUsingTemplate(PCountryTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCountryTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCountryTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_country", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ACountryCode, TDBTransaction ATransaction)
        {
            return Exists(PCountryTable.TableId, new System.Object[1]{ACountryCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_country" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PCountryTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PCountryTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_country" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PCountryTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PCountryTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPInternationalPostalType(DataSet ADataSet, String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PCountryTable.TableId, PInternationalPostalTypeTable.TableId, ADataSet, new string[1]{"p_internat_postal_type_code_c"},
                new System.Object[1]{AInternatPostalTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PCountryTable LoadViaPInternationalPostalType(String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCountryTable Data = new PCountryTable();
            LoadViaForeignKey(PCountryTable.TableId, PInternationalPostalTypeTable.TableId, Data, new string[1]{"p_internat_postal_type_code_c"},
                new System.Object[1]{AInternatPostalTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCountryTable LoadViaPInternationalPostalType(String AInternatPostalTypeCode, TDBTransaction ATransaction)
        {
            return LoadViaPInternationalPostalType(AInternatPostalTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCountryTable LoadViaPInternationalPostalType(String AInternatPostalTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPInternationalPostalType(AInternatPostalTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(DataSet ADataSet, PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PCountryTable.TableId, PInternationalPostalTypeTable.TableId, ADataSet, new string[1]{"p_internat_postal_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PCountryTable LoadViaPInternationalPostalTypeTemplate(PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCountryTable Data = new PCountryTable();
            LoadViaForeignKey(PCountryTable.TableId, PInternationalPostalTypeTable.TableId, Data, new string[1]{"p_internat_postal_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCountryTable LoadViaPInternationalPostalTypeTemplate(PInternationalPostalTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPInternationalPostalTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCountryTable LoadViaPInternationalPostalTypeTemplate(PInternationalPostalTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPInternationalPostalTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCountryTable LoadViaPInternationalPostalTypeTemplate(PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPInternationalPostalTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPInternationalPostalTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PCountryTable.TableId, PInternationalPostalTypeTable.TableId, ADataSet, new string[1]{"p_internat_postal_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PCountryTable LoadViaPInternationalPostalTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCountryTable Data = new PCountryTable();
            LoadViaForeignKey(PCountryTable.TableId, PInternationalPostalTypeTable.TableId, Data, new string[1]{"p_internat_postal_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCountryTable LoadViaPInternationalPostalTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPInternationalPostalTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCountryTable LoadViaPInternationalPostalTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPInternationalPostalTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPInternationalPostalType(String AInternatPostalTypeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PCountryTable.TableId, PInternationalPostalTypeTable.TableId, new string[1]{"p_internat_postal_type_code_c"},
                new System.Object[1]{AInternatPostalTypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPInternationalPostalTypeTemplate(PInternationalPostalTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PCountryTable.TableId, PInternationalPostalTypeTable.TableId, new string[1]{"p_internat_postal_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPInternationalPostalTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PCountryTable.TableId, PInternationalPostalTypeTable.TableId, new string[1]{"p_internat_postal_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPAddressLayoutCode(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(ACode));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_country", AFieldList, PCountryTable.TableId) +
                            " FROM PUB_p_country, PUB_p_address_layout WHERE " +
                            "PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c AND PUB_p_address_layout.p_address_layout_code_c = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PCountryTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static PCountryTable LoadViaPAddressLayoutCode(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PCountryTable Data = new PCountryTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPAddressLayoutCode(FillDataSet, ACode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PCountryTable LoadViaPAddressLayoutCode(String ACode, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCode(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCountryTable LoadViaPAddressLayoutCode(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCode(ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(DataSet ADataSet, PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_country", AFieldList, PCountryTable.TableId) +
                            " FROM PUB_p_country, PUB_p_address_layout, PUB_p_address_layout_code WHERE " +
                            "PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c AND PUB_p_address_layout.p_address_layout_code_c = PUB_p_address_layout_code.p_code_c") +
                            GenerateWhereClauseLong("PUB_p_address_layout_code", PAddressLayoutCodeTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PCountryTable.TableId), ATransaction,
                            GetParametersForWhereClause(PAddressLayoutCodeTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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
        public static PCountryTable LoadViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PCountryTable Data = new PCountryTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPAddressLayoutCodeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PCountryTable LoadViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCountryTable LoadViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCountryTable LoadViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_country", AFieldList, PCountryTable.TableId) +
                            " FROM PUB_p_country, PUB_p_address_layout, PUB_p_address_layout_code WHERE " +
                            "PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c AND PUB_p_address_layout.p_address_layout_code_c = PUB_p_address_layout_code.p_code_c") +
                            GenerateWhereClauseLong("PUB_p_address_layout_code", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PCountryTable.TableId), ATransaction,
                            GetParametersForWhereClause(PCountryTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
        public static PCountryTable LoadViaPAddressLayoutCodeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PCountryTable Data = new PCountryTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPAddressLayoutCodeTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PCountryTable LoadViaPAddressLayoutCodeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCountryTable LoadViaPAddressLayoutCodeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
                        GenerateWhereClauseLong("PUB_p_address_layout", PCountryTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PAddressLayoutCodeTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPAddressLayoutCodeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_country, PUB_p_address_layout, PUB_p_address_layout_code WHERE " +
                        "PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c AND PUB_p_address_layout.p_address_layout_code_c = PUB_p_address_layout_code.p_code_c" +
                        GenerateWhereClauseLong("PUB_p_address_layout", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PCountryTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ACountryCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PCountryTable.TableId, new System.Object[1]{ACountryCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PCountryTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PCountryTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PCountryTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PCountryTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, ACurrencyTable.TableId) + " FROM PUB_a_currency") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ACurrencyTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static ACurrencyTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACurrencyTable Data = new ACurrencyTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, ACurrencyTable.TableId) + " FROM PUB_a_currency" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACurrencyTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACurrencyTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(ACurrencyTable.TableId, ADataSet, new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static ACurrencyTable LoadByPrimaryKey(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACurrencyTable Data = new ACurrencyTable();
            LoadByPrimaryKey(ACurrencyTable.TableId, Data, new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACurrencyTable LoadByPrimaryKey(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACurrencyTable LoadByPrimaryKey(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ACurrencyTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static ACurrencyTable LoadUsingTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACurrencyTable Data = new ACurrencyTable();
            LoadUsingTemplate(ACurrencyTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACurrencyTable LoadUsingTemplate(ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACurrencyTable LoadUsingTemplate(ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACurrencyTable LoadUsingTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ACurrencyTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static ACurrencyTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACurrencyTable Data = new ACurrencyTable();
            LoadUsingTemplate(ACurrencyTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACurrencyTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACurrencyTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_currency", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return Exists(ACurrencyTable.TableId, new System.Object[1]{ACurrencyCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_currency" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ACurrencyTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(ACurrencyTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_currency" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ACurrencyTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(ACurrencyTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPCountry(DataSet ADataSet, String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ACurrencyTable.TableId, PCountryTable.TableId, ADataSet, new string[1]{"p_country_code_c"},
                new System.Object[1]{ACountryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static ACurrencyTable LoadViaPCountry(String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACurrencyTable Data = new ACurrencyTable();
            LoadViaForeignKey(ACurrencyTable.TableId, PCountryTable.TableId, Data, new string[1]{"p_country_code_c"},
                new System.Object[1]{ACountryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACurrencyTable LoadViaPCountry(String ACountryCode, TDBTransaction ATransaction)
        {
            return LoadViaPCountry(ACountryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACurrencyTable LoadViaPCountry(String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountry(ACountryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet ADataSet, PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ACurrencyTable.TableId, PCountryTable.TableId, ADataSet, new string[1]{"p_country_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static ACurrencyTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACurrencyTable Data = new ACurrencyTable();
            LoadViaForeignKey(ACurrencyTable.TableId, PCountryTable.TableId, Data, new string[1]{"p_country_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACurrencyTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACurrencyTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACurrencyTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ACurrencyTable.TableId, PCountryTable.TableId, ADataSet, new string[1]{"p_country_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static ACurrencyTable LoadViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACurrencyTable Data = new ACurrencyTable();
            LoadViaForeignKey(ACurrencyTable.TableId, PCountryTable.TableId, Data, new string[1]{"p_country_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACurrencyTable LoadViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACurrencyTable LoadViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPCountry(String ACountryCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ACurrencyTable.TableId, PCountryTable.TableId, new string[1]{"p_country_code_c"},
                new System.Object[1]{ACountryCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ACurrencyTable.TableId, PCountryTable.TableId, new string[1]{"p_country_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ACurrencyTable.TableId, PCountryTable.TableId, new string[1]{"p_country_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPLanguage(DataSet ADataSet, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(ALanguageCode));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_currency", AFieldList, ACurrencyTable.TableId) +
                            " FROM PUB_a_currency, PUB_a_currency_language WHERE " +
                            "PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c AND PUB_a_currency_language.p_language_code_c = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ACurrencyTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static ACurrencyTable LoadViaPLanguage(String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            ACurrencyTable Data = new ACurrencyTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPLanguage(FillDataSet, ALanguageCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static ACurrencyTable LoadViaPLanguage(String ALanguageCode, TDBTransaction ATransaction)
        {
            return LoadViaPLanguage(ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACurrencyTable LoadViaPLanguage(String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguage(ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_currency", AFieldList, ACurrencyTable.TableId) +
                            " FROM PUB_a_currency, PUB_a_currency_language, PUB_p_language WHERE " +
                            "PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c AND PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c") +
                            GenerateWhereClauseLong("PUB_p_language", PLanguageTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ACurrencyTable.TableId), ATransaction,
                            GetParametersForWhereClause(PLanguageTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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
        public static ACurrencyTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            ACurrencyTable Data = new ACurrencyTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPLanguageTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static ACurrencyTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACurrencyTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACurrencyTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_currency", AFieldList, ACurrencyTable.TableId) +
                            " FROM PUB_a_currency, PUB_a_currency_language, PUB_p_language WHERE " +
                            "PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c AND PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c") +
                            GenerateWhereClauseLong("PUB_p_language", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ACurrencyTable.TableId), ATransaction,
                            GetParametersForWhereClause(ACurrencyTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
        public static ACurrencyTable LoadViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            ACurrencyTable Data = new ACurrencyTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPLanguageTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static ACurrencyTable LoadViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACurrencyTable LoadViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
                        GenerateWhereClauseLong("PUB_a_currency_language", ACurrencyTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PLanguageTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_currency, PUB_a_currency_language, PUB_p_language WHERE " +
                        "PUB_a_currency_language.a_currency_code_c = PUB_a_currency.a_currency_code_c AND PUB_a_currency_language.p_language_code_c = PUB_p_language.p_language_code_c" +
                        GenerateWhereClauseLong("PUB_a_currency_language", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(ACurrencyTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ACurrencyCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(ACurrencyTable.TableId, new System.Object[1]{ACurrencyCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ACurrencyTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ACurrencyTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(ACurrencyTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ACurrencyTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }
}