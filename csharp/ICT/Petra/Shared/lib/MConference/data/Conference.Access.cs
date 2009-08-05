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
namespace Ict.Petra.Shared.MConference.Data.Access
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
    using Ict.Petra.Shared.MConference.Data;
    using Ict.Petra.Shared.MPartner.Partner.Data;
    using Ict.Petra.Shared.MCommon.Data;

    /// Basic details about a conference
    public class PcConferenceAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PcConference";

        /// original table name in database
        public const string DBTABLENAME = "pc_conference";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcConferenceTable.TableId) + " FROM PUB_pc_conference") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcConferenceTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcConferenceTable.TableId) + " FROM PUB_pc_conference" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out PcConferenceTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PcConferenceTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PcConferenceTable.TableId, ADataSet, new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceTable();
            LoadByPrimaryKey(PcConferenceTable.TableId, AData, new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcConferenceTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceTable();
            LoadUsingTemplate(PcConferenceTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcConferenceTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceTable();
            LoadUsingTemplate(PcConferenceTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return Exists(PcConferenceTable.TableId, new System.Object[1]{AConferenceKey}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcConferenceTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcConferenceTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcConferenceTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcConferenceTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out PcConferenceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceTable();
            LoadViaForeignKey(PcConferenceTable.TableId, PUnitTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(out PcConferenceTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out PcConferenceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcConferenceTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceTable();
            LoadViaForeignKey(PcConferenceTable.TableId, PUnitTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcConferenceTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcConferenceTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcConferenceTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceTable();
            LoadViaForeignKey(PcConferenceTable.TableId, PUnitTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceTable.TableId, PUnitTable.TableId, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceTable.TableId, PUnitTable.TableId, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceTable.TableId, PUnitTable.TableId, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaACurrency(out PcConferenceTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceTable();
            LoadViaForeignKey(PcConferenceTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrency(out PcConferenceTable AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrency(out PcConferenceTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaACurrencyTemplate(out PcConferenceTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceTable();
            LoadViaForeignKey(PcConferenceTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out PcConferenceTable AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out PcConferenceTable AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out PcConferenceTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaACurrencyTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceTable();
            LoadViaForeignKey(PcConferenceTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPcConferenceOptionType(DataSet ADataSet, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AOptionTypeCode));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, PcConferenceTable.TableId) +
                            " FROM PUB_pc_conference, PUB_pc_conference_option WHERE " +
                            "PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_option.pc_option_type_code_c = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionType(DataSet AData, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(AData, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionType(DataSet AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(AData, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionType(out PcConferenceTable AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceOptionType(FillDataSet, AOptionTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionType(out PcConferenceTable AData, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(out AData, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionType(out PcConferenceTable AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(out AData, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet ADataSet, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, PcConferenceTable.TableId) +
                            " FROM PUB_pc_conference, PUB_pc_conference_option, PUB_pc_conference_option_type WHERE " +
                            "PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c") +
                            GenerateWhereClauseLong("PUB_pc_conference_option_type", PcConferenceOptionTypeTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceTable.TableId), ATransaction,
                            GetParametersForWhereClause(PcConferenceOptionTypeTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet AData, PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceOptionTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceTable AData, PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, PcConferenceTable.TableId) +
                            " FROM PUB_pc_conference, PUB_pc_conference_option, PUB_pc_conference_option_type WHERE " +
                            "PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c") +
                            GenerateWhereClauseLong("PUB_pc_conference_option_type", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceTable.TableId), ATransaction,
                            GetParametersForWhereClause(PcConferenceTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceOptionTypeTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPcConferenceOptionType(String AOptionTypeCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AOptionTypeCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_conference_option WHERE " +
                        "PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_option.pc_option_type_code_c = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPcConferenceOptionTypeTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_conference_option, PUB_pc_conference_option_type WHERE " +
                        "PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c" +
                        GenerateWhereClauseLong("PUB_pc_conference_option", PcConferenceTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PcConferenceOptionTypeTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPcConferenceOptionTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_conference_option, PUB_pc_conference_option_type WHERE " +
                        "PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c" +
                        GenerateWhereClauseLong("PUB_pc_conference_option", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PcConferenceTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPPerson(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, PcConferenceTable.TableId) +
                            " FROM PUB_pc_conference, PUB_pc_attendee WHERE " +
                            "PUB_pc_attendee.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_attendee.p_partner_key_n = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPerson(out PcConferenceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPerson(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPPerson(out PcConferenceTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPerson(out PcConferenceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, PcConferenceTable.TableId) +
                            " FROM PUB_pc_conference, PUB_pc_attendee, PUB_p_person WHERE " +
                            "PUB_pc_attendee.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_attendee.p_partner_key_n = PUB_p_person.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_person", PPersonTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceTable.TableId), ATransaction,
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
        public static void LoadViaPPersonTemplate(out PcConferenceTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPersonTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcConferenceTable AData, PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcConferenceTable AData, PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcConferenceTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, PcConferenceTable.TableId) +
                            " FROM PUB_pc_conference, PUB_pc_attendee, PUB_p_person WHERE " +
                            "PUB_pc_attendee.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_attendee.p_partner_key_n = PUB_p_person.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_person", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceTable.TableId), ATransaction,
                            GetParametersForWhereClause(PcConferenceTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
        public static void LoadViaPPersonTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPPersonTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_attendee WHERE " +
                        "PUB_pc_attendee.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_attendee.p_partner_key_n = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_attendee, PUB_p_person WHERE " +
                        "PUB_pc_attendee.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_attendee.p_partner_key_n = PUB_p_person.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_pc_attendee", PcConferenceTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PPersonTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_attendee, PUB_p_person WHERE " +
                        "PUB_pc_attendee.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_attendee.p_partner_key_n = PUB_p_person.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_pc_attendee", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PcConferenceTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPVenue(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, PcConferenceTable.TableId) +
                            " FROM PUB_pc_conference, PUB_pc_conference_venue WHERE " +
                            "PUB_pc_conference_venue.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_venue.p_venue_key_n = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenue(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPVenue(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenue(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenue(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenue(out PcConferenceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPVenue(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPVenue(out PcConferenceTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPVenue(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenue(out PcConferenceTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenue(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet ADataSet, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, PcConferenceTable.TableId) +
                            " FROM PUB_pc_conference, PUB_pc_conference_venue, PUB_p_venue WHERE " +
                            "PUB_pc_conference_venue.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_venue.p_venue_key_n = PUB_p_venue.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_venue", PVenueTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceTable.TableId), ATransaction,
                            GetParametersForWhereClause(PVenueTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceTable AData, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPVenueTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceTable AData, PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceTable AData, PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceTable AData, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference", AFieldList, PcConferenceTable.TableId) +
                            " FROM PUB_pc_conference, PUB_pc_conference_venue, PUB_p_venue WHERE " +
                            "PUB_pc_conference_venue.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_venue.p_venue_key_n = PUB_p_venue.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_venue", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceTable.TableId), ATransaction,
                            GetParametersForWhereClause(PcConferenceTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPVenueTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPVenue(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_conference_venue WHERE " +
                        "PUB_pc_conference_venue.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_venue.p_venue_key_n = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_conference_venue, PUB_p_venue WHERE " +
                        "PUB_pc_conference_venue.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_venue.p_venue_key_n = PUB_p_venue.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_pc_conference_venue", PcConferenceTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PVenueTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference, PUB_pc_conference_venue, PUB_p_venue WHERE " +
                        "PUB_pc_conference_venue.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n AND PUB_pc_conference_venue.p_venue_key_n = PUB_p_venue.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_pc_conference_venue", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PcConferenceTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcConferenceTable.TableId, new System.Object[1]{AConferenceKey}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcConferenceTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcConferenceTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcConferenceTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcConferenceTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Cost types to be used for conference (extra) charges
    public class PcCostTypeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PcCostType";

        /// original table name in database
        public const string DBTABLENAME = "pc_cost_type";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcCostTypeTable.TableId) + " FROM PUB_pc_cost_type") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcCostTypeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcCostTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcCostTypeTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcCostTypeTable.TableId) + " FROM PUB_pc_cost_type" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out PcCostTypeTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PcCostTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PcCostTypeTable.TableId, ADataSet, new System.Object[1]{ACostTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACostTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACostTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcCostTypeTable AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcCostTypeTable();
            LoadByPrimaryKey(PcCostTypeTable.TableId, AData, new System.Object[1]{ACostTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcCostTypeTable AData, String ACostTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ACostTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcCostTypeTable AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcCostTypeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcCostTypeTable AData, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcCostTypeTable();
            LoadUsingTemplate(PcCostTypeTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcCostTypeTable AData, PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcCostTypeTable AData, PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcCostTypeTable AData, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcCostTypeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcCostTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcCostTypeTable();
            LoadUsingTemplate(PcCostTypeTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcCostTypeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcCostTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_cost_type", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ACostTypeCode, TDBTransaction ATransaction)
        {
            return Exists(PcCostTypeTable.TableId, new System.Object[1]{ACostTypeCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_cost_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcCostTypeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcCostTypeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_cost_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcCostTypeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcCostTypeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ACostTypeCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcCostTypeTable.TableId, new System.Object[1]{ACostTypeCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcCostTypeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcCostTypeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcCostTypeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcCostTypeTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Lists types of options that can be used for a conference
    public class PcConferenceOptionTypeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PcConferenceOptionType";

        /// original table name in database
        public const string DBTABLENAME = "pc_conference_option_type";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcConferenceOptionTypeTable.TableId) + " FROM PUB_pc_conference_option_type") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceOptionTypeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcConferenceOptionTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTypeTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcConferenceOptionTypeTable.TableId) + " FROM PUB_pc_conference_option_type" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out PcConferenceOptionTypeTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PcConferenceOptionTypeTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PcConferenceOptionTypeTable.TableId, ADataSet, new System.Object[1]{AOptionTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceOptionTypeTable AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTypeTable();
            LoadByPrimaryKey(PcConferenceOptionTypeTable.TableId, AData, new System.Object[1]{AOptionTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceOptionTypeTable AData, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceOptionTypeTable AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcConferenceOptionTypeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTypeTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTypeTable();
            LoadUsingTemplate(PcConferenceOptionTypeTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTypeTable AData, PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTypeTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTypeTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcConferenceOptionTypeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcConferenceOptionTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTypeTable();
            LoadUsingTemplate(PcConferenceOptionTypeTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTypeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_option_type", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AOptionTypeCode, TDBTransaction ATransaction)
        {
            return Exists(PcConferenceOptionTypeTable.TableId, new System.Object[1]{AOptionTypeCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_option_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcConferenceOptionTypeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcConferenceOptionTypeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_option_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcConferenceOptionTypeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcConferenceOptionTypeTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_pc_conference_option_type", AFieldList, PcConferenceOptionTypeTable.TableId) +
                            " FROM PUB_pc_conference_option_type, PUB_pc_conference_option WHERE " +
                            "PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c AND PUB_pc_conference_option.pc_conference_key_n = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceOptionTypeTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcConferenceOptionTypeTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConference(FillDataSet, AConferenceKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcConferenceOptionTypeTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcConferenceOptionTypeTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference_option_type", AFieldList, PcConferenceOptionTypeTable.TableId) +
                            " FROM PUB_pc_conference_option_type, PUB_pc_conference_option, PUB_pc_conference WHERE " +
                            "PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c AND PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n") +
                            GenerateWhereClauseLong("PUB_pc_conference", PcConferenceTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceOptionTypeTable.TableId), ATransaction,
                            GetParametersForWhereClause(PcConferenceTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTypeTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTypeTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTypeTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTypeTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_pc_conference_option_type", AFieldList, PcConferenceOptionTypeTable.TableId) +
                            " FROM PUB_pc_conference_option_type, PUB_pc_conference_option, PUB_pc_conference WHERE " +
                            "PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c AND PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n") +
                            GenerateWhereClauseLong("PUB_pc_conference", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceOptionTypeTable.TableId), ATransaction,
                            GetParametersForWhereClause(PcConferenceOptionTypeTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AData = new PcConferenceOptionTypeTable();
            FillDataSet.Tables.Add(AData);
            LoadViaPcConferenceTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(AData);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTypeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTypeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(AConferenceKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_option_type, PUB_pc_conference_option WHERE " +
                        "PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c AND PUB_pc_conference_option.pc_conference_key_n = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_option_type, PUB_pc_conference_option, PUB_pc_conference WHERE " +
                        "PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c AND PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n" +
                        GenerateWhereClauseLong("PUB_pc_conference_option", PcConferenceOptionTypeTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PcConferenceTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_option_type, PUB_pc_conference_option, PUB_pc_conference WHERE " +
                        "PUB_pc_conference_option.pc_option_type_code_c = PUB_pc_conference_option_type.pc_option_type_code_c AND PUB_pc_conference_option.pc_conference_key_n = PUB_pc_conference.pc_conference_key_n" +
                        GenerateWhereClauseLong("PUB_pc_conference_option", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PcConferenceOptionTypeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AOptionTypeCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcConferenceOptionTypeTable.TableId, new System.Object[1]{AOptionTypeCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcConferenceOptionTypeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcConferenceOptionTypeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcConferenceOptionTypeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcConferenceOptionTypeTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Lists options that are set for a conference
    public class PcConferenceOptionAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PcConferenceOption";

        /// original table name in database
        public const string DBTABLENAME = "pc_conference_option";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcConferenceOptionTable.TableId) + " FROM PUB_pc_conference_option") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceOptionTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcConferenceOptionTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcConferenceOptionTable.TableId) + " FROM PUB_pc_conference_option" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out PcConferenceOptionTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PcConferenceOptionTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PcConferenceOptionTable.TableId, ADataSet, new System.Object[2]{AConferenceKey, AOptionTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceOptionTable AData, Int64 AConferenceKey, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTable();
            LoadByPrimaryKey(PcConferenceOptionTable.TableId, AData, new System.Object[2]{AConferenceKey, AOptionTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceOptionTable AData, Int64 AConferenceKey, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceOptionTable AData, Int64 AConferenceKey, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcConferenceOptionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcConferenceOptionTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceOptionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceOptionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTable AData, PcConferenceOptionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTable();
            LoadUsingTemplate(PcConferenceOptionTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTable AData, PcConferenceOptionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTable AData, PcConferenceOptionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTable AData, PcConferenceOptionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcConferenceOptionTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcConferenceOptionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTable();
            LoadUsingTemplate(PcConferenceOptionTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceOptionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_option", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AConferenceKey, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            return Exists(PcConferenceOptionTable.TableId, new System.Object[2]{AConferenceKey, AOptionTypeCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcConferenceOptionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_option" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcConferenceOptionTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcConferenceOptionTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_option" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcConferenceOptionTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcConferenceOptionTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcConferenceOptionTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTable();
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcConferenceOptionTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcConferenceOptionTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTable();
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTable();
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceOptionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionType(DataSet ADataSet, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceOptionTypeTable.TableId, ADataSet, new string[1]{"pc_option_type_code_c"},
                new System.Object[1]{AOptionTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionType(DataSet AData, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(AData, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionType(DataSet AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(AData, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionType(out PcConferenceOptionTable AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTable();
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceOptionTypeTable.TableId, AData, new string[1]{"pc_option_type_code_c"},
                new System.Object[1]{AOptionTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionType(out PcConferenceOptionTable AData, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(out AData, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionType(out PcConferenceOptionTable AData, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionType(out AData, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet ADataSet, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceOptionTypeTable.TableId, ADataSet, new string[1]{"pc_option_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet AData, PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceOptionTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTable();
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceOptionTypeTable.TableId, AData, new string[1]{"pc_option_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceOptionTable AData, PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceOptionTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceOptionTable AData, PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceOptionTypeTable.TableId, ADataSet, new string[1]{"pc_option_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceOptionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceOptionTable();
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceOptionTypeTable.TableId, AData, new string[1]{"pc_option_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceOptionTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceOptionTypeTemplate(out PcConferenceOptionTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceOptionTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcConferenceOptionType(String AOptionTypeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceOptionTypeTable.TableId, new string[1]{"pc_option_type_code_c"},
                new System.Object[1]{AOptionTypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceOptionTypeTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceOptionTypeTable.TableId, new string[1]{"pc_option_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceOptionTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceOptionTypeTable.TableId, new string[1]{"pc_option_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcConferenceOptionTable.TableId, new System.Object[2]{AConferenceKey, AOptionTypeCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcConferenceOptionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcConferenceOptionTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcConferenceOptionTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcConferenceOptionTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcConferenceOptionTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Lists possible criterias that must be met for discounts to be applied
    public class PcDiscountCriteriaAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PcDiscountCriteria";

        /// original table name in database
        public const string DBTABLENAME = "pc_discount_criteria";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcDiscountCriteriaTable.TableId) + " FROM PUB_pc_discount_criteria") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcDiscountCriteriaTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcDiscountCriteriaTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountCriteriaTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcDiscountCriteriaTable.TableId) + " FROM PUB_pc_discount_criteria" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out PcDiscountCriteriaTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PcDiscountCriteriaTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PcDiscountCriteriaTable.TableId, ADataSet, new System.Object[1]{ADiscountCriteriaCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ADiscountCriteriaCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ADiscountCriteriaCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcDiscountCriteriaTable AData, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountCriteriaTable();
            LoadByPrimaryKey(PcDiscountCriteriaTable.TableId, AData, new System.Object[1]{ADiscountCriteriaCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcDiscountCriteriaTable AData, String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ADiscountCriteriaCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcDiscountCriteriaTable AData, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ADiscountCriteriaCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcDiscountCriteriaTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcDiscountCriteriaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcDiscountCriteriaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountCriteriaTable AData, PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountCriteriaTable();
            LoadUsingTemplate(PcDiscountCriteriaTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountCriteriaTable AData, PcDiscountCriteriaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountCriteriaTable AData, PcDiscountCriteriaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountCriteriaTable AData, PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcDiscountCriteriaTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcDiscountCriteriaTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountCriteriaTable();
            LoadUsingTemplate(PcDiscountCriteriaTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountCriteriaTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountCriteriaTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_discount_criteria", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            return Exists(PcDiscountCriteriaTable.TableId, new System.Object[1]{ADiscountCriteriaCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_discount_criteria" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcDiscountCriteriaTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcDiscountCriteriaTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_discount_criteria" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcDiscountCriteriaTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcDiscountCriteriaTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcDiscountCriteriaTable.TableId, new System.Object[1]{ADiscountCriteriaCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcDiscountCriteriaTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcDiscountCriteriaTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcDiscountCriteriaTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcDiscountCriteriaTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Lists optional discounts for a conference
    public class PcDiscountAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PcDiscount";

        /// original table name in database
        public const string DBTABLENAME = "pc_discount";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcDiscountTable.TableId) + " FROM PUB_pc_discount") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcDiscountTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcDiscountTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcDiscountTable.TableId) + " FROM PUB_pc_discount" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out PcDiscountTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PcDiscountTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PcDiscountTable.TableId, ADataSet, new System.Object[5]{AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcDiscountTable AData, Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountTable();
            LoadByPrimaryKey(PcDiscountTable.TableId, AData, new System.Object[5]{AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcDiscountTable AData, Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcDiscountTable AData, Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcDiscountTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountTable AData, PcDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountTable();
            LoadUsingTemplate(PcDiscountTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountTable AData, PcDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountTable AData, PcDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountTable AData, PcDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcDiscountTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountTable();
            LoadUsingTemplate(PcDiscountTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_discount", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, TDBTransaction ATransaction)
        {
            return Exists(PcDiscountTable.TableId, new System.Object[5]{AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_discount" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcDiscountTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcDiscountTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_discount" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcDiscountTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcDiscountTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcDiscountTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcDiscountTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcDiscountTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcDiscountTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcDiscountTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcDiscountTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcDiscountTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcDiscountTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcDiscountTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcDiscountTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcDiscountTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcDiscountTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcDiscountTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteria(DataSet ADataSet, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcDiscountTable.TableId, PcDiscountCriteriaTable.TableId, ADataSet, new string[1]{"pc_discount_criteria_code_c"},
                new System.Object[1]{ADiscountCriteriaCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteria(DataSet AData, String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteria(AData, ADiscountCriteriaCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteria(DataSet AData, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteria(AData, ADiscountCriteriaCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteria(out PcDiscountTable AData, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcDiscountCriteriaTable.TableId, AData, new string[1]{"pc_discount_criteria_code_c"},
                new System.Object[1]{ADiscountCriteriaCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteria(out PcDiscountTable AData, String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteria(out AData, ADiscountCriteriaCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteria(out PcDiscountTable AData, String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteria(out AData, ADiscountCriteriaCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(DataSet ADataSet, PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcDiscountTable.TableId, PcDiscountCriteriaTable.TableId, ADataSet, new string[1]{"pc_discount_criteria_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(DataSet AData, PcDiscountCriteriaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(DataSet AData, PcDiscountCriteriaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(out PcDiscountTable AData, PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcDiscountCriteriaTable.TableId, AData, new string[1]{"pc_discount_criteria_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(out PcDiscountTable AData, PcDiscountCriteriaRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(out PcDiscountTable AData, PcDiscountCriteriaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(out PcDiscountTable AData, PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcDiscountTable.TableId, PcDiscountCriteriaTable.TableId, ADataSet, new string[1]{"pc_discount_criteria_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(out PcDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcDiscountCriteriaTable.TableId, AData, new string[1]{"pc_discount_criteria_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(out PcDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcDiscountCriteriaTemplate(out PcDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcDiscountCriteriaTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcDiscountCriteria(String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcDiscountTable.TableId, PcDiscountCriteriaTable.TableId, new string[1]{"pc_discount_criteria_code_c"},
                new System.Object[1]{ADiscountCriteriaCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcDiscountCriteriaTemplate(PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcDiscountTable.TableId, PcDiscountCriteriaTable.TableId, new string[1]{"pc_discount_criteria_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcDiscountCriteriaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcDiscountTable.TableId, PcDiscountCriteriaTable.TableId, new string[1]{"pc_discount_criteria_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPcCostType(DataSet ADataSet, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcDiscountTable.TableId, PcCostTypeTable.TableId, ADataSet, new string[1]{"pc_cost_type_code_c"},
                new System.Object[1]{ACostTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcCostType(DataSet AData, String ACostTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(AData, ACostTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostType(DataSet AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(AData, ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostType(out PcDiscountTable AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcCostTypeTable.TableId, AData, new string[1]{"pc_cost_type_code_c"},
                new System.Object[1]{ACostTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcCostType(out PcDiscountTable AData, String ACostTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(out AData, ACostTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostType(out PcDiscountTable AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(out AData, ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet ADataSet, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcDiscountTable.TableId, PcCostTypeTable.TableId, ADataSet, new string[1]{"pc_cost_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet AData, PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet AData, PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcDiscountTable AData, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcCostTypeTable.TableId, AData, new string[1]{"pc_cost_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcDiscountTable AData, PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcDiscountTable AData, PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcDiscountTable AData, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcDiscountTable.TableId, PcCostTypeTable.TableId, ADataSet, new string[1]{"pc_cost_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcCostTypeTable.TableId, AData, new string[1]{"pc_cost_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcDiscountTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcDiscountTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcCostType(String ACostTypeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcDiscountTable.TableId, PcCostTypeTable.TableId, new string[1]{"pc_cost_type_code_c"},
                new System.Object[1]{ACostTypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcCostTypeTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcDiscountTable.TableId, PcCostTypeTable.TableId, new string[1]{"pc_cost_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcCostTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcDiscountTable.TableId, PcCostTypeTable.TableId, new string[1]{"pc_cost_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcDiscountTable.TableId, new System.Object[5]{AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcDiscountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcDiscountTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcDiscountTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcDiscountTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcDiscountTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Lists the attendees at a conference
    public class PcAttendeeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PcAttendee";

        /// original table name in database
        public const string DBTABLENAME = "pc_attendee";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcAttendeeTable.TableId) + " FROM PUB_pc_attendee") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcAttendeeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcAttendeeTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcAttendeeTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcAttendeeTable.TableId) + " FROM PUB_pc_attendee" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out PcAttendeeTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PcAttendeeTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PcAttendeeTable.TableId, ADataSet, new System.Object[2]{AConferenceKey, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcAttendeeTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcAttendeeTable();
            LoadByPrimaryKey(PcAttendeeTable.TableId, AData, new System.Object[2]{AConferenceKey, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcAttendeeTable AData, Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcAttendeeTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcAttendeeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcAttendeeTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcAttendeeTable();
            LoadUsingTemplate(PcAttendeeTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcAttendeeTable AData, PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcAttendeeTable AData, PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcAttendeeTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcAttendeeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcAttendeeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcAttendeeTable();
            LoadUsingTemplate(PcAttendeeTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcAttendeeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcAttendeeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_attendee", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return Exists(PcAttendeeTable.TableId, new System.Object[2]{AConferenceKey, APartnerKey}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_attendee" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcAttendeeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcAttendeeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_attendee" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcAttendeeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcAttendeeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcAttendeeTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcAttendeeTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcAttendeeTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcAttendeeTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcAttendeeTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcAttendeeTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcAttendeeTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcAttendeeTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcAttendeeTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcAttendeeTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcAttendeeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcAttendeeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcAttendeeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcAttendeeTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcAttendeeTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcAttendeeTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPerson(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcAttendeeTable.TableId, PPersonTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPerson(out PcAttendeeTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PPersonTable.TableId, AData, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPerson(out PcAttendeeTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPerson(out PcAttendeeTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcAttendeeTable.TableId, PPersonTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPersonTemplate(out PcAttendeeTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PPersonTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcAttendeeTable AData, PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcAttendeeTable AData, PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcAttendeeTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcAttendeeTable.TableId, PPersonTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPersonTemplate(out PcAttendeeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PPersonTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcAttendeeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcAttendeeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcAttendeeTable.TableId, PPersonTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcAttendeeTable.TableId, PPersonTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcAttendeeTable.TableId, PPersonTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcAttendeeTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pc_home_office_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out PcAttendeeTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PUnitTable.TableId, AData, new string[1]{"pc_home_office_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(out PcAttendeeTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out PcAttendeeTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcAttendeeTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pc_home_office_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcAttendeeTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PUnitTable.TableId, AData, new string[1]{"pc_home_office_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcAttendeeTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcAttendeeTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcAttendeeTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcAttendeeTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pc_home_office_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcAttendeeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PUnitTable.TableId, AData, new string[1]{"pc_home_office_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcAttendeeTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcAttendeeTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcAttendeeTable.TableId, PUnitTable.TableId, new string[1]{"pc_home_office_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcAttendeeTable.TableId, PUnitTable.TableId, new string[1]{"pc_home_office_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcAttendeeTable.TableId, PUnitTable.TableId, new string[1]{"pc_home_office_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcAttendeeTable.TableId, new System.Object[2]{AConferenceKey, APartnerKey}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcAttendeeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcAttendeeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcAttendeeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcAttendeeTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Charges for the various xyz_tbd options from a conference (currency held in conference master)
    public class PcConferenceCostAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PcConferenceCost";

        /// original table name in database
        public const string DBTABLENAME = "pc_conference_cost";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcConferenceCostTable.TableId) + " FROM PUB_pc_conference_cost") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceCostTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcConferenceCostTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceCostTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcConferenceCostTable.TableId) + " FROM PUB_pc_conference_cost" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out PcConferenceCostTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PcConferenceCostTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, Int32 AOptionDays, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PcConferenceCostTable.TableId, ADataSet, new System.Object[2]{AConferenceKey, AOptionDays}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int32 AOptionDays, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AOptionDays, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int32 AOptionDays, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AOptionDays, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceCostTable AData, Int64 AConferenceKey, Int32 AOptionDays, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceCostTable();
            LoadByPrimaryKey(PcConferenceCostTable.TableId, AData, new System.Object[2]{AConferenceKey, AOptionDays}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceCostTable AData, Int64 AConferenceKey, Int32 AOptionDays, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AOptionDays, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceCostTable AData, Int64 AConferenceKey, Int32 AOptionDays, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AOptionDays, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcConferenceCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcConferenceCostTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceCostTable AData, PcConferenceCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceCostTable();
            LoadUsingTemplate(PcConferenceCostTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceCostTable AData, PcConferenceCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceCostTable AData, PcConferenceCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceCostTable AData, PcConferenceCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcConferenceCostTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcConferenceCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceCostTable();
            LoadUsingTemplate(PcConferenceCostTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceCostTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_cost", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AConferenceKey, Int32 AOptionDays, TDBTransaction ATransaction)
        {
            return Exists(PcConferenceCostTable.TableId, new System.Object[2]{AConferenceKey, AOptionDays}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcConferenceCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_cost" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcConferenceCostTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcConferenceCostTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_cost" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcConferenceCostTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcConferenceCostTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceCostTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcConferenceCostTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceCostTable();
            LoadViaForeignKey(PcConferenceCostTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcConferenceCostTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcConferenceCostTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceCostTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceCostTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceCostTable();
            LoadViaForeignKey(PcConferenceCostTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceCostTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceCostTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceCostTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceCostTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceCostTable();
            LoadViaForeignKey(PcConferenceCostTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceCostTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceCostTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceCostTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceCostTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int32 AOptionDays, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcConferenceCostTable.TableId, new System.Object[2]{AConferenceKey, AOptionDays}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcConferenceCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcConferenceCostTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcConferenceCostTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcConferenceCostTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcConferenceCostTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Contains extra conference costs for individual attendees
    public class PcExtraCostAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PcExtraCost";

        /// original table name in database
        public const string DBTABLENAME = "pc_extra_cost";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcExtraCostTable.TableId) + " FROM PUB_pc_extra_cost") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcExtraCostTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcExtraCostTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcExtraCostTable.TableId) + " FROM PUB_pc_extra_cost" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out PcExtraCostTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PcExtraCostTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PcExtraCostTable.TableId, ADataSet, new System.Object[3]{AConferenceKey, APartnerKey, AExtraCostKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, APartnerKey, AExtraCostKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, APartnerKey, AExtraCostKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcExtraCostTable AData, Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadByPrimaryKey(PcExtraCostTable.TableId, AData, new System.Object[3]{AConferenceKey, APartnerKey, AExtraCostKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcExtraCostTable AData, Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, APartnerKey, AExtraCostKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcExtraCostTable AData, Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, APartnerKey, AExtraCostKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcExtraCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcExtraCostTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcExtraCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcExtraCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcExtraCostTable AData, PcExtraCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadUsingTemplate(PcExtraCostTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcExtraCostTable AData, PcExtraCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcExtraCostTable AData, PcExtraCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcExtraCostTable AData, PcExtraCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcExtraCostTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadUsingTemplate(PcExtraCostTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_extra_cost", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, TDBTransaction ATransaction)
        {
            return Exists(PcExtraCostTable.TableId, new System.Object[3]{AConferenceKey, APartnerKey, AExtraCostKey}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcExtraCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_extra_cost" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcExtraCostTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcExtraCostTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_extra_cost" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcExtraCostTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcExtraCostTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPcAttendee(DataSet ADataSet, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PcAttendeeTable.TableId, ADataSet, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                new System.Object[2]{AConferenceKey, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendee(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(AData, AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendee(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(AData, AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendee(out PcExtraCostTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcAttendeeTable.TableId, AData, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                new System.Object[2]{AConferenceKey, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendee(out PcExtraCostTable AData, Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(out AData, AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendee(out PcExtraCostTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(out AData, AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet ADataSet, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PcAttendeeTable.TableId, ADataSet, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcExtraCostTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcAttendeeTable.TableId, AData, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcExtraCostTable AData, PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcExtraCostTable AData, PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcExtraCostTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PcAttendeeTable.TableId, ADataSet, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcAttendeeTable.TableId, AData, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PcAttendeeTable.TableId, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                new System.Object[2]{AConferenceKey, APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PcAttendeeTable.TableId, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcAttendeeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PcAttendeeTable.TableId, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcExtraCostTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcExtraCostTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcExtraCostTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcExtraCostTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcExtraCostTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcExtraCostTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcExtraCostTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPerson(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PPersonTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPerson(out PcExtraCostTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PPersonTable.TableId, AData, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPerson(out PcExtraCostTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPerson(out PcExtraCostTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PPersonTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPersonTemplate(out PcExtraCostTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PPersonTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcExtraCostTable AData, PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcExtraCostTable AData, PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcExtraCostTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PPersonTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPersonTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PPersonTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PPersonTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PPersonTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PPersonTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPcCostType(DataSet ADataSet, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PcCostTypeTable.TableId, ADataSet, new string[1]{"pc_cost_type_code_c"},
                new System.Object[1]{ACostTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcCostType(DataSet AData, String ACostTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(AData, ACostTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostType(DataSet AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(AData, ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostType(out PcExtraCostTable AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcCostTypeTable.TableId, AData, new string[1]{"pc_cost_type_code_c"},
                new System.Object[1]{ACostTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcCostType(out PcExtraCostTable AData, String ACostTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(out AData, ACostTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostType(out PcExtraCostTable AData, String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostType(out AData, ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet ADataSet, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PcCostTypeTable.TableId, ADataSet, new string[1]{"pc_cost_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet AData, PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet AData, PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcExtraCostTable AData, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcCostTypeTable.TableId, AData, new string[1]{"pc_cost_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcExtraCostTable AData, PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcExtraCostTable AData, PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcExtraCostTable AData, PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PcCostTypeTable.TableId, ADataSet, new string[1]{"pc_cost_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcCostTypeTable.TableId, AData, new string[1]{"pc_cost_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcCostTypeTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcCostTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcCostType(String ACostTypeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PcCostTypeTable.TableId, new string[1]{"pc_cost_type_code_c"},
                new System.Object[1]{ACostTypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcCostTypeTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PcCostTypeTable.TableId, new string[1]{"pc_cost_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcCostTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PcCostTypeTable.TableId, new string[1]{"pc_cost_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pc_authorising_field_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out PcExtraCostTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PUnitTable.TableId, AData, new string[1]{"pc_authorising_field_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnit(out PcExtraCostTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnit(out PcExtraCostTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnit(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pc_authorising_field_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcExtraCostTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PUnitTable.TableId, AData, new string[1]{"pc_authorising_field_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcExtraCostTable AData, PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcExtraCostTable AData, PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcExtraCostTable AData, PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcExtraCostTable.TableId, PUnitTable.TableId, ADataSet, new string[1]{"pc_authorising_field_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PUnitTable.TableId, AData, new string[1]{"pc_authorising_field_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPUnitTemplate(out PcExtraCostTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPUnitTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PUnitTable.TableId, new string[1]{"pc_authorising_field_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PUnitTable.TableId, new string[1]{"pc_authorising_field_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcExtraCostTable.TableId, PUnitTable.TableId, new string[1]{"pc_authorising_field_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcExtraCostTable.TableId, new System.Object[3]{AConferenceKey, APartnerKey, AExtraCostKey}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcExtraCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcExtraCostTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcExtraCostTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcExtraCostTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcExtraCostTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Discounts and Supplements for early or late registration
    public class PcEarlyLateAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PcEarlyLate";

        /// original table name in database
        public const string DBTABLENAME = "pc_early_late";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcEarlyLateTable.TableId) + " FROM PUB_pc_early_late") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcEarlyLateTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcEarlyLateTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcEarlyLateTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcEarlyLateTable.TableId) + " FROM PUB_pc_early_late" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out PcEarlyLateTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PcEarlyLateTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, System.DateTime AApplicable, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PcEarlyLateTable.TableId, ADataSet, new System.Object[2]{AConferenceKey, AApplicable}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, System.DateTime AApplicable, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AApplicable, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, System.DateTime AApplicable, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AApplicable, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcEarlyLateTable AData, Int64 AConferenceKey, System.DateTime AApplicable, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcEarlyLateTable();
            LoadByPrimaryKey(PcEarlyLateTable.TableId, AData, new System.Object[2]{AConferenceKey, AApplicable}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcEarlyLateTable AData, Int64 AConferenceKey, System.DateTime AApplicable, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AApplicable, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcEarlyLateTable AData, Int64 AConferenceKey, System.DateTime AApplicable, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AApplicable, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcEarlyLateRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcEarlyLateTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcEarlyLateRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcEarlyLateRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcEarlyLateTable AData, PcEarlyLateRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcEarlyLateTable();
            LoadUsingTemplate(PcEarlyLateTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcEarlyLateTable AData, PcEarlyLateRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcEarlyLateTable AData, PcEarlyLateRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcEarlyLateTable AData, PcEarlyLateRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcEarlyLateTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcEarlyLateTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcEarlyLateTable();
            LoadUsingTemplate(PcEarlyLateTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcEarlyLateTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcEarlyLateTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_early_late", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AConferenceKey, System.DateTime AApplicable, TDBTransaction ATransaction)
        {
            return Exists(PcEarlyLateTable.TableId, new System.Object[2]{AConferenceKey, AApplicable}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcEarlyLateRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_early_late" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcEarlyLateTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcEarlyLateTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_early_late" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcEarlyLateTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcEarlyLateTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcEarlyLateTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcEarlyLateTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcEarlyLateTable();
            LoadViaForeignKey(PcEarlyLateTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcEarlyLateTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcEarlyLateTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcEarlyLateTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcEarlyLateTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcEarlyLateTable();
            LoadViaForeignKey(PcEarlyLateTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcEarlyLateTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcEarlyLateTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcEarlyLateTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcEarlyLateTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcEarlyLateTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcEarlyLateTable();
            LoadViaForeignKey(PcEarlyLateTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcEarlyLateTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcEarlyLateTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcEarlyLateTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcEarlyLateTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcEarlyLateTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, System.DateTime AApplicable, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcEarlyLateTable.TableId, new System.Object[2]{AConferenceKey, AApplicable}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcEarlyLateRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcEarlyLateTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcEarlyLateTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcEarlyLateTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcEarlyLateTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Contains information about which groups individual attendees are assigned to
    public class PcGroupAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PcGroup";

        /// original table name in database
        public const string DBTABLENAME = "pc_group";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcGroupTable.TableId) + " FROM PUB_pc_group") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcGroupTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcGroupTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcGroupTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcGroupTable.TableId) + " FROM PUB_pc_group" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out PcGroupTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PcGroupTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PcGroupTable.TableId, ADataSet, new System.Object[4]{AConferenceKey, APartnerKey, AGroupType, AGroupName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, APartnerKey, AGroupType, AGroupName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, APartnerKey, AGroupType, AGroupName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcGroupTable AData, Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcGroupTable();
            LoadByPrimaryKey(PcGroupTable.TableId, AData, new System.Object[4]{AConferenceKey, APartnerKey, AGroupType, AGroupName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcGroupTable AData, Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, APartnerKey, AGroupType, AGroupName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcGroupTable AData, Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, APartnerKey, AGroupType, AGroupName, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcGroupTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcGroupTable AData, PcGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcGroupTable();
            LoadUsingTemplate(PcGroupTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcGroupTable AData, PcGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcGroupTable AData, PcGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcGroupTable AData, PcGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcGroupTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcGroupTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcGroupTable();
            LoadUsingTemplate(PcGroupTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcGroupTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcGroupTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_group", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, TDBTransaction ATransaction)
        {
            return Exists(PcGroupTable.TableId, new System.Object[4]{AConferenceKey, APartnerKey, AGroupType, AGroupName}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_group" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcGroupTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcGroupTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_group" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcGroupTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcGroupTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPcAttendee(DataSet ADataSet, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcGroupTable.TableId, PcAttendeeTable.TableId, ADataSet, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                new System.Object[2]{AConferenceKey, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendee(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(AData, AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendee(DataSet AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(AData, AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendee(out PcGroupTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PcAttendeeTable.TableId, AData, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                new System.Object[2]{AConferenceKey, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendee(out PcGroupTable AData, Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(out AData, AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendee(out PcGroupTable AData, Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendee(out AData, AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet ADataSet, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcGroupTable.TableId, PcAttendeeTable.TableId, ADataSet, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcGroupTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PcAttendeeTable.TableId, AData, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcGroupTable AData, PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcGroupTable AData, PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcGroupTable AData, PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcGroupTable.TableId, PcAttendeeTable.TableId, ADataSet, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcGroupTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PcAttendeeTable.TableId, AData, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcGroupTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcAttendeeTemplate(out PcGroupTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcAttendeeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcGroupTable.TableId, PcAttendeeTable.TableId, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                new System.Object[2]{AConferenceKey, APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcGroupTable.TableId, PcAttendeeTable.TableId, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcAttendeeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcGroupTable.TableId, PcAttendeeTable.TableId, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcGroupTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcGroupTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcGroupTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcGroupTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcGroupTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcGroupTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcGroupTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcGroupTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcGroupTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcGroupTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcGroupTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcGroupTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcGroupTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcGroupTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcGroupTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcGroupTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPerson(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcGroupTable.TableId, PPersonTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPerson(out PcGroupTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PPersonTable.TableId, AData, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPerson(out PcGroupTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPerson(out PcGroupTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPerson(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcGroupTable.TableId, PPersonTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPersonTemplate(out PcGroupTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PPersonTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcGroupTable AData, PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcGroupTable AData, PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcGroupTable AData, PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcGroupTable.TableId, PPersonTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadViaPPersonTemplate(out PcGroupTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PPersonTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcGroupTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPersonTemplate(out PcGroupTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPersonTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcGroupTable.TableId, PPersonTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcGroupTable.TableId, PPersonTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcGroupTable.TableId, PPersonTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcGroupTable.TableId, new System.Object[4]{AConferenceKey, APartnerKey, AGroupType, AGroupName}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcGroupTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcGroupTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcGroupTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcGroupTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Xyz_tbd travel supplements (by xyz_tbd ID)
    public class PcSupplementAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PcSupplement";

        /// original table name in database
        public const string DBTABLENAME = "pc_supplement";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcSupplementTable.TableId) + " FROM PUB_pc_supplement") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcSupplementTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcSupplementTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcSupplementTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcSupplementTable.TableId) + " FROM PUB_pc_supplement" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out PcSupplementTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PcSupplementTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, String AXyzTbdType, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PcSupplementTable.TableId, ADataSet, new System.Object[2]{AConferenceKey, AXyzTbdType}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, String AXyzTbdType, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AXyzTbdType, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, String AXyzTbdType, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AXyzTbdType, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcSupplementTable AData, Int64 AConferenceKey, String AXyzTbdType, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcSupplementTable();
            LoadByPrimaryKey(PcSupplementTable.TableId, AData, new System.Object[2]{AConferenceKey, AXyzTbdType}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcSupplementTable AData, Int64 AConferenceKey, String AXyzTbdType, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AXyzTbdType, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcSupplementTable AData, Int64 AConferenceKey, String AXyzTbdType, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AXyzTbdType, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcSupplementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcSupplementTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcSupplementRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcSupplementRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcSupplementTable AData, PcSupplementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcSupplementTable();
            LoadUsingTemplate(PcSupplementTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcSupplementTable AData, PcSupplementRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcSupplementTable AData, PcSupplementRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcSupplementTable AData, PcSupplementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcSupplementTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcSupplementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcSupplementTable();
            LoadUsingTemplate(PcSupplementTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcSupplementTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcSupplementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_supplement", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AConferenceKey, String AXyzTbdType, TDBTransaction ATransaction)
        {
            return Exists(PcSupplementTable.TableId, new System.Object[2]{AConferenceKey, AXyzTbdType}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcSupplementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_supplement" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcSupplementTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcSupplementTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_supplement" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcSupplementTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcSupplementTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcSupplementTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcSupplementTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcSupplementTable();
            LoadViaForeignKey(PcSupplementTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcSupplementTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcSupplementTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcSupplementTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcSupplementTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcSupplementTable();
            LoadViaForeignKey(PcSupplementTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcSupplementTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcSupplementTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcSupplementTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcSupplementTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcSupplementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcSupplementTable();
            LoadViaForeignKey(PcSupplementTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcSupplementTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcSupplementTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcSupplementTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcSupplementTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcSupplementTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, String AXyzTbdType, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcSupplementTable.TableId, new System.Object[2]{AConferenceKey, AXyzTbdType}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcSupplementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcSupplementTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcSupplementTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcSupplementTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcSupplementTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Links venues to conferences
    public class PcConferenceVenueAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PcConferenceVenue";

        /// original table name in database
        public const string DBTABLENAME = "pc_conference_venue";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PcConferenceVenueTable.TableId) + " FROM PUB_pc_conference_venue") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PcConferenceVenueTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static void LoadAll(out PcConferenceVenueTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceVenueTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, PcConferenceVenueTable.TableId) + " FROM PUB_pc_conference_venue" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out PcConferenceVenueTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out PcConferenceVenueTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 AConferenceKey, Int64 AVenueKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PcConferenceVenueTable.TableId, ADataSet, new System.Object[2]{AConferenceKey, AVenueKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 AVenueKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AVenueKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 AConferenceKey, Int64 AVenueKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AConferenceKey, AVenueKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceVenueTable AData, Int64 AConferenceKey, Int64 AVenueKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceVenueTable();
            LoadByPrimaryKey(PcConferenceVenueTable.TableId, AData, new System.Object[2]{AConferenceKey, AVenueKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceVenueTable AData, Int64 AConferenceKey, Int64 AVenueKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AVenueKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out PcConferenceVenueTable AData, Int64 AConferenceKey, Int64 AVenueKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, AConferenceKey, AVenueKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PcConferenceVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcConferenceVenueTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PcConferenceVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceVenueTable AData, PcConferenceVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceVenueTable();
            LoadUsingTemplate(PcConferenceVenueTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceVenueTable AData, PcConferenceVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceVenueTable AData, PcConferenceVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceVenueTable AData, PcConferenceVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PcConferenceVenueTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static void LoadUsingTemplate(out PcConferenceVenueTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceVenueTable();
            LoadUsingTemplate(PcConferenceVenueTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceVenueTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out PcConferenceVenueTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_pc_conference_venue", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 AConferenceKey, Int64 AVenueKey, TDBTransaction ATransaction)
        {
            return Exists(PcConferenceVenueTable.TableId, new System.Object[2]{AConferenceKey, AVenueKey}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PcConferenceVenueRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_venue" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcConferenceVenueTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PcConferenceVenueTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_pc_conference_venue" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PcConferenceVenueTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PcConferenceVenueTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet ADataSet, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(DataSet AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcConferenceVenueTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceVenueTable();
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcConferenceVenueTable AData, Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConference(out PcConferenceVenueTable AData, Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConference(out AData, AConferenceKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceVenueTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceVenueTable();
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceVenueTable AData, PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceVenueTable AData, PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceVenueTable AData, PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PcConferenceTable.TableId, ADataSet, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceVenueTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceVenueTable();
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PcConferenceTable.TableId, AData, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceVenueTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPcConferenceTemplate(out PcConferenceVenueTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPcConferenceTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceVenueTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceVenueTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceVenueTable.TableId, PcConferenceTable.TableId, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPVenue(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PVenueTable.TableId, ADataSet, new string[1]{"p_venue_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenue(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPVenue(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenue(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenue(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenue(out PcConferenceVenueTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceVenueTable();
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PVenueTable.TableId, AData, new string[1]{"p_venue_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenue(out PcConferenceVenueTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPVenue(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenue(out PcConferenceVenueTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenue(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet ADataSet, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PVenueTable.TableId, ADataSet, new string[1]{"p_venue_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceVenueTable AData, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceVenueTable();
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PVenueTable.TableId, AData, new string[1]{"p_venue_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceVenueTable AData, PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceVenueTable AData, PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceVenueTable AData, PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PVenueTable.TableId, ADataSet, new string[1]{"p_venue_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceVenueTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new PcConferenceVenueTable();
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PVenueTable.TableId, AData, new string[1]{"p_venue_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceVenueTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPVenueTemplate(out PcConferenceVenueTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPVenueTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPVenue(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceVenueTable.TableId, PVenueTable.TableId, new string[1]{"p_venue_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceVenueTable.TableId, PVenueTable.TableId, new string[1]{"p_venue_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PcConferenceVenueTable.TableId, PVenueTable.TableId, new string[1]{"p_venue_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 AConferenceKey, Int64 AVenueKey, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PcConferenceVenueTable.TableId, new System.Object[2]{AConferenceKey, AVenueKey}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PcConferenceVenueRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcConferenceVenueTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PcConferenceVenueTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PcConferenceVenueTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(PcConferenceVenueTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }
}