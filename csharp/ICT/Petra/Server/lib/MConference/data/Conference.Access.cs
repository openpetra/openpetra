// auto generated with nant generateORM
// Do not modify this file manually!
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2010 by OM International
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

namespace Ict.Petra.Server.MConference.Data.Access
{

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
        public static PcConferenceTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceTable Data = new PcConferenceTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcConferenceTable.TableId) + " FROM PUB_pc_conference" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadByPrimaryKey(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceTable Data = new PcConferenceTable();
            LoadByPrimaryKey(PcConferenceTable.TableId, Data, new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadByPrimaryKey(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadByPrimaryKey(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadUsingTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceTable Data = new PcConferenceTable();
            LoadUsingTemplate(PcConferenceTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadUsingTemplate(PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadUsingTemplate(PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadUsingTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceTable Data = new PcConferenceTable();
            LoadUsingTemplate(PcConferenceTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaPUnit(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceTable Data = new PcConferenceTable();
            LoadViaForeignKey(PcConferenceTable.TableId, PUnitTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPUnit(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPUnit(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPUnit(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceTable Data = new PcConferenceTable();
            LoadViaForeignKey(PcConferenceTable.TableId, PUnitTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaPUnitTemplate(PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceTable Data = new PcConferenceTable();
            LoadViaForeignKey(PcConferenceTable.TableId, PUnitTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceTable Data = new PcConferenceTable();
            LoadViaForeignKey(PcConferenceTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceTable Data = new PcConferenceTable();
            LoadViaForeignKey(PcConferenceTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceTable Data = new PcConferenceTable();
            LoadViaForeignKey(PcConferenceTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaPcConferenceOptionType(String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcConferenceTable Data = new PcConferenceTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcConferenceOptionType(FillDataSet, AOptionTypeCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaPcConferenceOptionType(String AOptionTypeCode, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionType(AOptionTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPcConferenceOptionType(String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionType(AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaPcConferenceOptionTypeTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcConferenceTable Data = new PcConferenceTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcConferenceOptionTypeTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaPcConferenceOptionTypeTemplate(PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPcConferenceOptionTypeTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPcConferenceOptionTypeTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaPcConferenceOptionTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcConferenceTable Data = new PcConferenceTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcConferenceOptionTypeTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaPcConferenceOptionTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPcConferenceOptionTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaPPerson(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcConferenceTable Data = new PcConferenceTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPerson(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPerson(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPPerson(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPerson(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcConferenceTable Data = new PcConferenceTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPersonTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcConferenceTable Data = new PcConferenceTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPersonTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaPVenue(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcConferenceTable Data = new PcConferenceTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPVenue(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaPVenue(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPVenue(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPVenue(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenue(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcConferenceTable Data = new PcConferenceTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPVenueTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceTable LoadViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcConferenceTable Data = new PcConferenceTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPVenueTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcConferenceTable LoadViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceTable LoadViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcCostTypeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcCostTypeTable Data = new PcCostTypeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcCostTypeTable.TableId) + " FROM PUB_pc_cost_type" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcCostTypeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcCostTypeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcCostTypeTable LoadByPrimaryKey(String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcCostTypeTable Data = new PcCostTypeTable();
            LoadByPrimaryKey(PcCostTypeTable.TableId, Data, new System.Object[1]{ACostTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcCostTypeTable LoadByPrimaryKey(String ACostTypeCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACostTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcCostTypeTable LoadByPrimaryKey(String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcCostTypeTable LoadUsingTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcCostTypeTable Data = new PcCostTypeTable();
            LoadUsingTemplate(PcCostTypeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcCostTypeTable LoadUsingTemplate(PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcCostTypeTable LoadUsingTemplate(PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcCostTypeTable LoadUsingTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcCostTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcCostTypeTable Data = new PcCostTypeTable();
            LoadUsingTemplate(PcCostTypeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcCostTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcCostTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcConferenceOptionTypeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTypeTable Data = new PcConferenceOptionTypeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcConferenceOptionTypeTable.TableId) + " FROM PUB_pc_conference_option_type" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTypeTable LoadByPrimaryKey(String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTypeTable Data = new PcConferenceOptionTypeTable();
            LoadByPrimaryKey(PcConferenceOptionTypeTable.TableId, Data, new System.Object[1]{AOptionTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadByPrimaryKey(String AOptionTypeCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AOptionTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadByPrimaryKey(String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTypeTable LoadUsingTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTypeTable Data = new PcConferenceOptionTypeTable();
            LoadUsingTemplate(PcConferenceOptionTypeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadUsingTemplate(PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadUsingTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadUsingTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTypeTable Data = new PcConferenceOptionTypeTable();
            LoadUsingTemplate(PcConferenceOptionTypeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTypeTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcConferenceOptionTypeTable Data = new PcConferenceOptionTypeTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcConference(FillDataSet, AConferenceKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTypeTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcConferenceOptionTypeTable Data = new PcConferenceOptionTypeTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcConferenceTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTypeTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PcConferenceOptionTypeTable Data = new PcConferenceOptionTypeTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPcConferenceTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTypeTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcConferenceOptionTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTable Data = new PcConferenceOptionTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcConferenceOptionTable.TableId) + " FROM PUB_pc_conference_option" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTable LoadByPrimaryKey(Int64 AConferenceKey, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTable Data = new PcConferenceOptionTable();
            LoadByPrimaryKey(PcConferenceOptionTable.TableId, Data, new System.Object[2]{AConferenceKey, AOptionTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTable LoadByPrimaryKey(Int64 AConferenceKey, String AOptionTypeCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, AOptionTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTable LoadByPrimaryKey(Int64 AConferenceKey, String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTable LoadUsingTemplate(PcConferenceOptionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTable Data = new PcConferenceOptionTable();
            LoadUsingTemplate(PcConferenceOptionTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTable LoadUsingTemplate(PcConferenceOptionRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTable LoadUsingTemplate(PcConferenceOptionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTable LoadUsingTemplate(PcConferenceOptionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTable Data = new PcConferenceOptionTable();
            LoadUsingTemplate(PcConferenceOptionTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTable Data = new PcConferenceOptionTable();
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTable Data = new PcConferenceOptionTable();
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTable Data = new PcConferenceOptionTable();
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTable LoadViaPcConferenceOptionType(String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTable Data = new PcConferenceOptionTable();
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceOptionTypeTable.TableId, Data, new string[1]{"pc_option_type_code_c"},
                new System.Object[1]{AOptionTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConferenceOptionType(String AOptionTypeCode, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionType(AOptionTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConferenceOptionType(String AOptionTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionType(AOptionTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTable LoadViaPcConferenceOptionTypeTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTable Data = new PcConferenceOptionTable();
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceOptionTypeTable.TableId, Data, new string[1]{"pc_option_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConferenceOptionTypeTemplate(PcConferenceOptionTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConferenceOptionTypeTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConferenceOptionTypeTemplate(PcConferenceOptionTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceOptionTable LoadViaPcConferenceOptionTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceOptionTable Data = new PcConferenceOptionTable();
            LoadViaForeignKey(PcConferenceOptionTable.TableId, PcConferenceOptionTypeTable.TableId, Data, new string[1]{"pc_option_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConferenceOptionTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceOptionTable LoadViaPcConferenceOptionTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceOptionTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcDiscountCriteriaTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountCriteriaTable Data = new PcDiscountCriteriaTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcDiscountCriteriaTable.TableId) + " FROM PUB_pc_discount_criteria" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountCriteriaTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountCriteriaTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountCriteriaTable LoadByPrimaryKey(String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountCriteriaTable Data = new PcDiscountCriteriaTable();
            LoadByPrimaryKey(PcDiscountCriteriaTable.TableId, Data, new System.Object[1]{ADiscountCriteriaCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountCriteriaTable LoadByPrimaryKey(String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ADiscountCriteriaCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountCriteriaTable LoadByPrimaryKey(String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ADiscountCriteriaCode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountCriteriaTable LoadUsingTemplate(PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountCriteriaTable Data = new PcDiscountCriteriaTable();
            LoadUsingTemplate(PcDiscountCriteriaTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountCriteriaTable LoadUsingTemplate(PcDiscountCriteriaRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountCriteriaTable LoadUsingTemplate(PcDiscountCriteriaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountCriteriaTable LoadUsingTemplate(PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountCriteriaTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountCriteriaTable Data = new PcDiscountCriteriaTable();
            LoadUsingTemplate(PcDiscountCriteriaTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountCriteriaTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountCriteriaTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcDiscountTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountTable Data = new PcDiscountTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcDiscountTable.TableId) + " FROM PUB_pc_discount" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountTable LoadByPrimaryKey(Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountTable Data = new PcDiscountTable();
            LoadByPrimaryKey(PcDiscountTable.TableId, Data, new System.Object[5]{AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountTable LoadByPrimaryKey(Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadByPrimaryKey(Int64 AConferenceKey, String ADiscountCriteriaCode, String ACostTypeCode, String AValidity, Int32 AUpToAge, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, ADiscountCriteriaCode, ACostTypeCode, AValidity, AUpToAge, AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountTable LoadUsingTemplate(PcDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountTable Data = new PcDiscountTable();
            LoadUsingTemplate(PcDiscountTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountTable LoadUsingTemplate(PcDiscountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadUsingTemplate(PcDiscountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadUsingTemplate(PcDiscountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountTable Data = new PcDiscountTable();
            LoadUsingTemplate(PcDiscountTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountTable Data = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountTable Data = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountTable Data = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountTable LoadViaPcDiscountCriteria(String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountTable Data = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcDiscountCriteriaTable.TableId, Data, new string[1]{"pc_discount_criteria_code_c"},
                new System.Object[1]{ADiscountCriteriaCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcDiscountCriteria(String ADiscountCriteriaCode, TDBTransaction ATransaction)
        {
            return LoadViaPcDiscountCriteria(ADiscountCriteriaCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcDiscountCriteria(String ADiscountCriteriaCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcDiscountCriteria(ADiscountCriteriaCode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountTable LoadViaPcDiscountCriteriaTemplate(PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountTable Data = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcDiscountCriteriaTable.TableId, Data, new string[1]{"pc_discount_criteria_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcDiscountCriteriaTemplate(PcDiscountCriteriaRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcDiscountCriteriaTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcDiscountCriteriaTemplate(PcDiscountCriteriaRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcDiscountCriteriaTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcDiscountCriteriaTemplate(PcDiscountCriteriaRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcDiscountCriteriaTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountTable LoadViaPcDiscountCriteriaTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountTable Data = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcDiscountCriteriaTable.TableId, Data, new string[1]{"pc_discount_criteria_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcDiscountCriteriaTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcDiscountCriteriaTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcDiscountCriteriaTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcDiscountCriteriaTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountTable LoadViaPcCostType(String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountTable Data = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcCostTypeTable.TableId, Data, new string[1]{"pc_cost_type_code_c"},
                new System.Object[1]{ACostTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcCostType(String ACostTypeCode, TDBTransaction ATransaction)
        {
            return LoadViaPcCostType(ACostTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcCostType(String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcCostType(ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountTable LoadViaPcCostTypeTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountTable Data = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcCostTypeTable.TableId, Data, new string[1]{"pc_cost_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcCostTypeTemplate(PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcCostTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcCostTypeTemplate(PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcCostTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcCostTypeTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcCostTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcDiscountTable LoadViaPcCostTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcDiscountTable Data = new PcDiscountTable();
            LoadViaForeignKey(PcDiscountTable.TableId, PcCostTypeTable.TableId, Data, new string[1]{"pc_cost_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcCostTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcCostTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcDiscountTable LoadViaPcCostTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcCostTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcAttendeeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcAttendeeTable Data = new PcAttendeeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcAttendeeTable.TableId) + " FROM PUB_pc_attendee" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcAttendeeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcAttendeeTable LoadByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcAttendeeTable Data = new PcAttendeeTable();
            LoadByPrimaryKey(PcAttendeeTable.TableId, Data, new System.Object[2]{AConferenceKey, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcAttendeeTable LoadByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcAttendeeTable LoadUsingTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcAttendeeTable Data = new PcAttendeeTable();
            LoadUsingTemplate(PcAttendeeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcAttendeeTable LoadUsingTemplate(PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadUsingTemplate(PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadUsingTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcAttendeeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcAttendeeTable Data = new PcAttendeeTable();
            LoadUsingTemplate(PcAttendeeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcAttendeeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcAttendeeTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcAttendeeTable Data = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcAttendeeTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcAttendeeTable Data = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcAttendeeTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcAttendeeTable Data = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcAttendeeTable LoadViaPPerson(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcAttendeeTable Data = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PPersonTable.TableId, Data, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPerson(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPPerson(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPerson(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcAttendeeTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcAttendeeTable Data = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PPersonTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcAttendeeTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcAttendeeTable Data = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PPersonTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcAttendeeTable LoadViaPUnit(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcAttendeeTable Data = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PUnitTable.TableId, Data, new string[1]{"pc_home_office_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPUnit(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPUnit(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPUnit(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcAttendeeTable LoadViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcAttendeeTable Data = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PUnitTable.TableId, Data, new string[1]{"pc_home_office_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPUnitTemplate(PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcAttendeeTable LoadViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcAttendeeTable Data = new PcAttendeeTable();
            LoadViaForeignKey(PcAttendeeTable.TableId, PUnitTable.TableId, Data, new string[1]{"pc_home_office_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcAttendeeTable LoadViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcConferenceCostTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceCostTable Data = new PcConferenceCostTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcConferenceCostTable.TableId) + " FROM PUB_pc_conference_cost" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceCostTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceCostTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceCostTable LoadByPrimaryKey(Int64 AConferenceKey, Int32 AOptionDays, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceCostTable Data = new PcConferenceCostTable();
            LoadByPrimaryKey(PcConferenceCostTable.TableId, Data, new System.Object[2]{AConferenceKey, AOptionDays}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceCostTable LoadByPrimaryKey(Int64 AConferenceKey, Int32 AOptionDays, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, AOptionDays, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceCostTable LoadByPrimaryKey(Int64 AConferenceKey, Int32 AOptionDays, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, AOptionDays, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceCostTable LoadUsingTemplate(PcConferenceCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceCostTable Data = new PcConferenceCostTable();
            LoadUsingTemplate(PcConferenceCostTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceCostTable LoadUsingTemplate(PcConferenceCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceCostTable LoadUsingTemplate(PcConferenceCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceCostTable LoadUsingTemplate(PcConferenceCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceCostTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceCostTable Data = new PcConferenceCostTable();
            LoadUsingTemplate(PcConferenceCostTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceCostTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceCostTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceCostTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceCostTable Data = new PcConferenceCostTable();
            LoadViaForeignKey(PcConferenceCostTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceCostTable LoadViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceCostTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceCostTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceCostTable Data = new PcConferenceCostTable();
            LoadViaForeignKey(PcConferenceCostTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceCostTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceCostTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceCostTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceCostTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceCostTable Data = new PcConferenceCostTable();
            LoadViaForeignKey(PcConferenceCostTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceCostTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceCostTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcExtraCostTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcExtraCostTable.TableId) + " FROM PUB_pc_extra_cost" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadByPrimaryKey(PcExtraCostTable.TableId, Data, new System.Object[3]{AConferenceKey, APartnerKey, AExtraCostKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, APartnerKey, AExtraCostKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, Int32 AExtraCostKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, APartnerKey, AExtraCostKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadUsingTemplate(PcExtraCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadUsingTemplate(PcExtraCostTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadUsingTemplate(PcExtraCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadUsingTemplate(PcExtraCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadUsingTemplate(PcExtraCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadUsingTemplate(PcExtraCostTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcAttendeeTable.TableId, Data, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                new System.Object[2]{AConferenceKey, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendee(AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendee(AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcAttendeeTable.TableId, Data, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPcAttendeeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcAttendeeTable.TableId, Data, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcAttendeeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcAttendeeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPPerson(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PPersonTable.TableId, Data, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPerson(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPPerson(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPerson(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PPersonTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PPersonTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPcCostType(String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcCostTypeTable.TableId, Data, new string[1]{"pc_cost_type_code_c"},
                new System.Object[1]{ACostTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcCostType(String ACostTypeCode, TDBTransaction ATransaction)
        {
            return LoadViaPcCostType(ACostTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcCostType(String ACostTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcCostType(ACostTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPcCostTypeTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcCostTypeTable.TableId, Data, new string[1]{"pc_cost_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcCostTypeTemplate(PcCostTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcCostTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcCostTypeTemplate(PcCostTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcCostTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcCostTypeTemplate(PcCostTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcCostTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPcCostTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PcCostTypeTable.TableId, Data, new string[1]{"pc_cost_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcCostTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcCostTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPcCostTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcCostTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPUnit(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PUnitTable.TableId, Data, new string[1]{"pc_authorising_field_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPUnit(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPUnit(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPUnit(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPUnit(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PUnitTable.TableId, Data, new string[1]{"pc_authorising_field_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPUnitTemplate(PUnitRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPUnitTemplate(PUnitRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcExtraCostTable LoadViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcExtraCostTable Data = new PcExtraCostTable();
            LoadViaForeignKey(PcExtraCostTable.TableId, PUnitTable.TableId, Data, new string[1]{"pc_authorising_field_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcExtraCostTable LoadViaPUnitTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPUnitTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcEarlyLateTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcEarlyLateTable Data = new PcEarlyLateTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcEarlyLateTable.TableId) + " FROM PUB_pc_early_late" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcEarlyLateTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcEarlyLateTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcEarlyLateTable LoadByPrimaryKey(Int64 AConferenceKey, System.DateTime AApplicable, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcEarlyLateTable Data = new PcEarlyLateTable();
            LoadByPrimaryKey(PcEarlyLateTable.TableId, Data, new System.Object[2]{AConferenceKey, AApplicable}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcEarlyLateTable LoadByPrimaryKey(Int64 AConferenceKey, System.DateTime AApplicable, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, AApplicable, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcEarlyLateTable LoadByPrimaryKey(Int64 AConferenceKey, System.DateTime AApplicable, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, AApplicable, AFieldList, ATransaction, null, 0, 0);
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
        public static PcEarlyLateTable LoadUsingTemplate(PcEarlyLateRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcEarlyLateTable Data = new PcEarlyLateTable();
            LoadUsingTemplate(PcEarlyLateTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcEarlyLateTable LoadUsingTemplate(PcEarlyLateRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcEarlyLateTable LoadUsingTemplate(PcEarlyLateRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcEarlyLateTable LoadUsingTemplate(PcEarlyLateRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcEarlyLateTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcEarlyLateTable Data = new PcEarlyLateTable();
            LoadUsingTemplate(PcEarlyLateTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcEarlyLateTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcEarlyLateTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcEarlyLateTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcEarlyLateTable Data = new PcEarlyLateTable();
            LoadViaForeignKey(PcEarlyLateTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcEarlyLateTable LoadViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcEarlyLateTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcEarlyLateTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcEarlyLateTable Data = new PcEarlyLateTable();
            LoadViaForeignKey(PcEarlyLateTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcEarlyLateTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcEarlyLateTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcEarlyLateTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcEarlyLateTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcEarlyLateTable Data = new PcEarlyLateTable();
            LoadViaForeignKey(PcEarlyLateTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcEarlyLateTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcEarlyLateTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcGroupTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcGroupTable Data = new PcGroupTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcGroupTable.TableId) + " FROM PUB_pc_group" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcGroupTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcGroupTable LoadByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcGroupTable Data = new PcGroupTable();
            LoadByPrimaryKey(PcGroupTable.TableId, Data, new System.Object[4]{AConferenceKey, APartnerKey, AGroupType, AGroupName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcGroupTable LoadByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, APartnerKey, AGroupType, AGroupName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadByPrimaryKey(Int64 AConferenceKey, Int64 APartnerKey, String AGroupType, String AGroupName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, APartnerKey, AGroupType, AGroupName, AFieldList, ATransaction, null, 0, 0);
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
        public static PcGroupTable LoadUsingTemplate(PcGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcGroupTable Data = new PcGroupTable();
            LoadUsingTemplate(PcGroupTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcGroupTable LoadUsingTemplate(PcGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadUsingTemplate(PcGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadUsingTemplate(PcGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcGroupTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcGroupTable Data = new PcGroupTable();
            LoadUsingTemplate(PcGroupTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcGroupTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcGroupTable LoadViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcGroupTable Data = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PcAttendeeTable.TableId, Data, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                new System.Object[2]{AConferenceKey, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcGroupTable LoadViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendee(AConferenceKey, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadViaPcAttendee(Int64 AConferenceKey, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendee(AConferenceKey, APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcGroupTable LoadViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcGroupTable Data = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PcAttendeeTable.TableId, Data, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcGroupTable LoadViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadViaPcAttendeeTemplate(PcAttendeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcGroupTable LoadViaPcAttendeeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcGroupTable Data = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PcAttendeeTable.TableId, Data, new string[2]{"pc_conference_key_n", "p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcGroupTable LoadViaPcAttendeeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadViaPcAttendeeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcAttendeeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcGroupTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcGroupTable Data = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcGroupTable LoadViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcGroupTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcGroupTable Data = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcGroupTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcGroupTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcGroupTable Data = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcGroupTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcGroupTable LoadViaPPerson(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcGroupTable Data = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PPersonTable.TableId, Data, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcGroupTable LoadViaPPerson(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPerson(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadViaPPerson(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPerson(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcGroupTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcGroupTable Data = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PPersonTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcGroupTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadViaPPersonTemplate(PPersonRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcGroupTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcGroupTable Data = new PcGroupTable();
            LoadViaForeignKey(PcGroupTable.TableId, PPersonTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcGroupTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcGroupTable LoadViaPPersonTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPersonTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcSupplementTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcSupplementTable Data = new PcSupplementTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcSupplementTable.TableId) + " FROM PUB_pc_supplement" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcSupplementTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcSupplementTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcSupplementTable LoadByPrimaryKey(Int64 AConferenceKey, String AXyzTbdType, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcSupplementTable Data = new PcSupplementTable();
            LoadByPrimaryKey(PcSupplementTable.TableId, Data, new System.Object[2]{AConferenceKey, AXyzTbdType}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcSupplementTable LoadByPrimaryKey(Int64 AConferenceKey, String AXyzTbdType, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, AXyzTbdType, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcSupplementTable LoadByPrimaryKey(Int64 AConferenceKey, String AXyzTbdType, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, AXyzTbdType, AFieldList, ATransaction, null, 0, 0);
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
        public static PcSupplementTable LoadUsingTemplate(PcSupplementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcSupplementTable Data = new PcSupplementTable();
            LoadUsingTemplate(PcSupplementTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcSupplementTable LoadUsingTemplate(PcSupplementRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcSupplementTable LoadUsingTemplate(PcSupplementRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcSupplementTable LoadUsingTemplate(PcSupplementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcSupplementTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcSupplementTable Data = new PcSupplementTable();
            LoadUsingTemplate(PcSupplementTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcSupplementTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcSupplementTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcSupplementTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcSupplementTable Data = new PcSupplementTable();
            LoadViaForeignKey(PcSupplementTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcSupplementTable LoadViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcSupplementTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcSupplementTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcSupplementTable Data = new PcSupplementTable();
            LoadViaForeignKey(PcSupplementTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcSupplementTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcSupplementTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcSupplementTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcSupplementTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcSupplementTable Data = new PcSupplementTable();
            LoadViaForeignKey(PcSupplementTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcSupplementTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcSupplementTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
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
        public static PcConferenceVenueTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceVenueTable Data = new PcConferenceVenueTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PcConferenceVenueTable.TableId) + " FROM PUB_pc_conference_venue" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceVenueTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceVenueTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceVenueTable LoadByPrimaryKey(Int64 AConferenceKey, Int64 AVenueKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceVenueTable Data = new PcConferenceVenueTable();
            LoadByPrimaryKey(PcConferenceVenueTable.TableId, Data, new System.Object[2]{AConferenceKey, AVenueKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceVenueTable LoadByPrimaryKey(Int64 AConferenceKey, Int64 AVenueKey, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, AVenueKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceVenueTable LoadByPrimaryKey(Int64 AConferenceKey, Int64 AVenueKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AConferenceKey, AVenueKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceVenueTable LoadUsingTemplate(PcConferenceVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceVenueTable Data = new PcConferenceVenueTable();
            LoadUsingTemplate(PcConferenceVenueTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceVenueTable LoadUsingTemplate(PcConferenceVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceVenueTable LoadUsingTemplate(PcConferenceVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceVenueTable LoadUsingTemplate(PcConferenceVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceVenueTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceVenueTable Data = new PcConferenceVenueTable();
            LoadUsingTemplate(PcConferenceVenueTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceVenueTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceVenueTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceVenueTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceVenueTable Data = new PcConferenceVenueTable();
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                new System.Object[1]{AConferenceKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPcConference(Int64 AConferenceKey, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPcConference(Int64 AConferenceKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConference(AConferenceKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceVenueTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceVenueTable Data = new PcConferenceVenueTable();
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPcConferenceTemplate(PcConferenceRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceVenueTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceVenueTable Data = new PcConferenceVenueTable();
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PcConferenceTable.TableId, Data, new string[1]{"pc_conference_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPcConferenceTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPcConferenceTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceVenueTable LoadViaPVenue(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceVenueTable Data = new PcConferenceVenueTable();
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PVenueTable.TableId, Data, new string[1]{"p_venue_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPVenue(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPVenue(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPVenue(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenue(APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceVenueTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceVenueTable Data = new PcConferenceVenueTable();
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PVenueTable.TableId, Data, new string[1]{"p_venue_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPVenueTemplate(PVenueRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static PcConferenceVenueTable LoadViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PcConferenceVenueTable Data = new PcConferenceVenueTable();
            LoadViaForeignKey(PcConferenceVenueTable.TableId, PVenueTable.TableId, Data, new string[1]{"p_venue_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PcConferenceVenueTable LoadViaPVenueTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPVenueTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }
}
