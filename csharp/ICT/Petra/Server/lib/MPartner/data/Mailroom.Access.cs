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
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Server.MPartner.Mailroom.Data.Access
{

    /// Master file for extracts.  Contains names for the extract id
    public class MExtractMasterAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "MExtractMaster";

        /// original table name in database
        public const string DBTABLENAME = "m_extract_master";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, MExtractMasterTable.TableId) + " FROM PUB_m_extract_master") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(MExtractMasterTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static MExtractMasterTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractMasterTable Data = new MExtractMasterTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, MExtractMasterTable.TableId) + " FROM PUB_m_extract_master" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(MExtractMasterTable.TableId, ADataSet, new System.Object[1]{AExtractId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AExtractId, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AExtractId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AExtractId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadByPrimaryKey(Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractMasterTable Data = new MExtractMasterTable();
            LoadByPrimaryKey(MExtractMasterTable.TableId, Data, new System.Object[1]{AExtractId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadByPrimaryKey(Int32 AExtractId, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AExtractId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadByPrimaryKey(Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AExtractId, AFieldList, ATransaction, null, 0, 0);
        }
        /// this method is called by all overloads
        public static void LoadByUniqueKey(DataSet ADataSet, String AExtractName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByUniqueKey(MExtractMasterTable.TableId, ADataSet, new System.Object[1]{AExtractName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByUniqueKey(DataSet AData, String AExtractName, TDBTransaction ATransaction)
        {
            LoadByUniqueKey(AData, AExtractName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByUniqueKey(DataSet AData, String AExtractName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByUniqueKey(AData, AExtractName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadByUniqueKey(String AExtractName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractMasterTable Data = new MExtractMasterTable();
            LoadByUniqueKey(MExtractMasterTable.TableId, Data, new System.Object[1]{AExtractName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadByUniqueKey(String AExtractName, TDBTransaction ATransaction)
        {
            return LoadByUniqueKey(AExtractName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadByUniqueKey(String AExtractName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByUniqueKey(AExtractName, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(MExtractMasterTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, MExtractMasterRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, MExtractMasterRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadUsingTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractMasterTable Data = new MExtractMasterTable();
            LoadUsingTemplate(MExtractMasterTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadUsingTemplate(MExtractMasterRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadUsingTemplate(MExtractMasterRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadUsingTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(MExtractMasterTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static MExtractMasterTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractMasterTable Data = new MExtractMasterTable();
            LoadUsingTemplate(MExtractMasterTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_m_extract_master", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 AExtractId, TDBTransaction ATransaction)
        {
            return Exists(MExtractMasterTable.TableId, new System.Object[1]{AExtractId}, ATransaction);
        }

        /// check if a row exists by using the unique key
        public static bool Exists(String AExtractName, TDBTransaction ATransaction)
        {
            return Exists(MExtractMasterTable.TableId, new System.Object[1]{AExtractName}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_m_extract_master" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(MExtractMasterTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(MExtractMasterTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_m_extract_master" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(MExtractMasterTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(MExtractMasterTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaSUser(DataSet ADataSet, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractMasterTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"m_manual_mod_by_c"},
                new System.Object[1]{AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUser(DataSet AData, String AUserId, TDBTransaction ATransaction)
        {
            LoadViaSUser(AData, AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUser(DataSet AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUser(AData, AUserId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSUser(String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractMasterTable Data = new MExtractMasterTable();
            LoadViaForeignKey(MExtractMasterTable.TableId, SUserTable.TableId, Data, new string[1]{"m_manual_mod_by_c"},
                new System.Object[1]{AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            return LoadViaSUser(AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSUser(String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUser(AUserId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractMasterTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"m_manual_mod_by_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractMasterTable Data = new MExtractMasterTable();
            LoadViaForeignKey(MExtractMasterTable.TableId, SUserTable.TableId, Data, new string[1]{"m_manual_mod_by_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSUserTemplate(SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractMasterTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"m_manual_mod_by_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractMasterTable Data = new MExtractMasterTable();
            LoadViaForeignKey(MExtractMasterTable.TableId, SUserTable.TableId, Data, new string[1]{"m_manual_mod_by_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractMasterTable.TableId, SUserTable.TableId, new string[1]{"m_manual_mod_by_c"},
                new System.Object[1]{AUserId}, ATransaction);
        }

        /// auto generated
        public static int CountViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractMasterTable.TableId, SUserTable.TableId, new string[1]{"m_manual_mod_by_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaSUserTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractMasterTable.TableId, SUserTable.TableId, new string[1]{"m_manual_mod_by_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaMExtractType(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractMasterTable.TableId, MExtractTypeTable.TableId, ADataSet, new string[1]{"m_extract_type_code_c"},
                new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaMExtractType(DataSet AData, String ACode, TDBTransaction ATransaction)
        {
            LoadViaMExtractType(AData, ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractType(DataSet AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaMExtractType(AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaMExtractType(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractMasterTable Data = new MExtractMasterTable();
            LoadViaForeignKey(MExtractMasterTable.TableId, MExtractTypeTable.TableId, Data, new string[1]{"m_extract_type_code_c"},
                new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadViaMExtractType(String ACode, TDBTransaction ATransaction)
        {
            return LoadViaMExtractType(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaMExtractType(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractType(ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractTypeTemplate(DataSet ADataSet, MExtractTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractMasterTable.TableId, MExtractTypeTable.TableId, ADataSet, new string[1]{"m_extract_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaMExtractTypeTemplate(DataSet AData, MExtractTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaMExtractTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractTypeTemplate(DataSet AData, MExtractTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaMExtractTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaMExtractTypeTemplate(MExtractTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractMasterTable Data = new MExtractMasterTable();
            LoadViaForeignKey(MExtractMasterTable.TableId, MExtractTypeTable.TableId, Data, new string[1]{"m_extract_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadViaMExtractTypeTemplate(MExtractTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaMExtractTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaMExtractTypeTemplate(MExtractTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaMExtractTypeTemplate(MExtractTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractMasterTable.TableId, MExtractTypeTable.TableId, ADataSet, new string[1]{"m_extract_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaMExtractTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaMExtractTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaMExtractTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaMExtractTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractMasterTable Data = new MExtractMasterTable();
            LoadViaForeignKey(MExtractMasterTable.TableId, MExtractTypeTable.TableId, Data, new string[1]{"m_extract_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadViaMExtractTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaMExtractTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaMExtractTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaMExtractType(String ACode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractMasterTable.TableId, MExtractTypeTable.TableId, new string[1]{"m_extract_type_code_c"},
                new System.Object[1]{ACode}, ATransaction);
        }

        /// auto generated
        public static int CountViaMExtractTypeTemplate(MExtractTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractMasterTable.TableId, MExtractTypeTable.TableId, new string[1]{"m_extract_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaMExtractTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractMasterTable.TableId, MExtractTypeTable.TableId, new string[1]{"m_extract_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaSGroup(DataSet ADataSet, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AGroupId));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AUnitKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_m_extract_master", AFieldList, MExtractMasterTable.TableId) +
                            " FROM PUB_m_extract_master, PUB_s_group_extract WHERE " +
                            "PUB_s_group_extract.m_extract_id_i = PUB_m_extract_master.m_extract_id_i AND PUB_s_group_extract.s_group_id_c = ? AND PUB_s_group_extract.s_group_unit_key_n = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(MExtractMasterTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSGroup(DataSet AData, String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            LoadViaSGroup(AData, AGroupId, AUnitKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroup(DataSet AData, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroup(AData, AGroupId, AUnitKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSGroup(String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            MExtractMasterTable Data = new MExtractMasterTable();
            FillDataSet.Tables.Add(Data);
            LoadViaSGroup(FillDataSet, AGroupId, AUnitKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSGroup(String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            return LoadViaSGroup(AGroupId, AUnitKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSGroup(String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroup(AGroupId, AUnitKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet ADataSet, SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_m_extract_master", AFieldList, MExtractMasterTable.TableId) +
                            " FROM PUB_m_extract_master, PUB_s_group_extract, PUB_s_group WHERE " +
                            "PUB_s_group_extract.m_extract_id_i = PUB_m_extract_master.m_extract_id_i AND PUB_s_group_extract.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_extract.s_group_unit_key_n = PUB_s_group.s_unit_key_n") +
                            GenerateWhereClauseLong("PUB_s_group", SGroupTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(MExtractMasterTable.TableId), ATransaction,
                            GetParametersForWhereClause(SGroupTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet AData, SGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet AData, SGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            MExtractMasterTable Data = new MExtractMasterTable();
            FillDataSet.Tables.Add(Data);
            LoadViaSGroupTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_m_extract_master", AFieldList, MExtractMasterTable.TableId) +
                            " FROM PUB_m_extract_master, PUB_s_group_extract, PUB_s_group WHERE " +
                            "PUB_s_group_extract.m_extract_id_i = PUB_m_extract_master.m_extract_id_i AND PUB_s_group_extract.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_extract.s_group_unit_key_n = PUB_s_group.s_unit_key_n") +
                            GenerateWhereClauseLong("PUB_s_group", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(MExtractMasterTable.TableId), ATransaction,
                            GetParametersForWhereClause(MExtractMasterTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            MExtractMasterTable Data = new MExtractMasterTable();
            FillDataSet.Tables.Add(Data);
            LoadViaSGroupTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractMasterTable LoadViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaSGroup(String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AGroupId));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AUnitKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_m_extract_master, PUB_s_group_extract WHERE " +
                        "PUB_s_group_extract.m_extract_id_i = PUB_m_extract_master.m_extract_id_i AND PUB_s_group_extract.s_group_id_c = ? AND PUB_s_group_extract.s_group_unit_key_n = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_m_extract_master, PUB_s_group_extract, PUB_s_group WHERE " +
                        "PUB_s_group_extract.m_extract_id_i = PUB_m_extract_master.m_extract_id_i AND PUB_s_group_extract.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_extract.s_group_unit_key_n = PUB_s_group.s_unit_key_n" +
                        GenerateWhereClauseLong("PUB_s_group_extract", MExtractMasterTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(SGroupTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_m_extract_master, PUB_s_group_extract, PUB_s_group WHERE " +
                        "PUB_s_group_extract.m_extract_id_i = PUB_m_extract_master.m_extract_id_i AND PUB_s_group_extract.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_extract.s_group_unit_key_n = PUB_s_group.s_unit_key_n" +
                        GenerateWhereClauseLong("PUB_s_group_extract", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(MExtractMasterTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 AExtractId, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(MExtractMasterTable.TableId, new System.Object[1]{AExtractId}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(MExtractMasterTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(MExtractMasterTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(MExtractMasterTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID, "seq_extract_number", "m_extract_id_i");
        }
    }

    /// Contains the list of partners in each mailing extract
    public class MExtractAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "MExtract";

        /// original table name in database
        public const string DBTABLENAME = "m_extract";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, MExtractTable.TableId) + " FROM PUB_m_extract") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(MExtractTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static MExtractTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTable Data = new MExtractTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, MExtractTable.TableId) + " FROM PUB_m_extract" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 AExtractId, Int64 APartnerKey, Int64 ASiteKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(MExtractTable.TableId, ADataSet, new System.Object[3]{AExtractId, APartnerKey, ASiteKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AExtractId, Int64 APartnerKey, Int64 ASiteKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AExtractId, APartnerKey, ASiteKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AExtractId, Int64 APartnerKey, Int64 ASiteKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AExtractId, APartnerKey, ASiteKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadByPrimaryKey(Int32 AExtractId, Int64 APartnerKey, Int64 ASiteKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTable Data = new MExtractTable();
            LoadByPrimaryKey(MExtractTable.TableId, Data, new System.Object[3]{AExtractId, APartnerKey, ASiteKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTable LoadByPrimaryKey(Int32 AExtractId, Int64 APartnerKey, Int64 ASiteKey, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AExtractId, APartnerKey, ASiteKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadByPrimaryKey(Int32 AExtractId, Int64 APartnerKey, Int64 ASiteKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AExtractId, APartnerKey, ASiteKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, MExtractRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(MExtractTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, MExtractRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, MExtractRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadUsingTemplate(MExtractRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTable Data = new MExtractTable();
            LoadUsingTemplate(MExtractTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTable LoadUsingTemplate(MExtractRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadUsingTemplate(MExtractRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadUsingTemplate(MExtractRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(MExtractTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static MExtractTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTable Data = new MExtractTable();
            LoadUsingTemplate(MExtractTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_m_extract", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 AExtractId, Int64 APartnerKey, Int64 ASiteKey, TDBTransaction ATransaction)
        {
            return Exists(MExtractTable.TableId, new System.Object[3]{AExtractId, APartnerKey, ASiteKey}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(MExtractRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_m_extract" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(MExtractTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(MExtractTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_m_extract" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(MExtractTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(MExtractTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaMExtractMaster(DataSet ADataSet, Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractTable.TableId, MExtractMasterTable.TableId, ADataSet, new string[1]{"m_extract_id_i"},
                new System.Object[1]{AExtractId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaMExtractMaster(DataSet AData, Int32 AExtractId, TDBTransaction ATransaction)
        {
            LoadViaMExtractMaster(AData, AExtractId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMaster(DataSet AData, Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaMExtractMaster(AData, AExtractId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaMExtractMaster(Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTable Data = new MExtractTable();
            LoadViaForeignKey(MExtractTable.TableId, MExtractMasterTable.TableId, Data, new string[1]{"m_extract_id_i"},
                new System.Object[1]{AExtractId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTable LoadViaMExtractMaster(Int32 AExtractId, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMaster(AExtractId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaMExtractMaster(Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMaster(AExtractId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet ADataSet, MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractTable.TableId, MExtractMasterTable.TableId, ADataSet, new string[1]{"m_extract_id_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet AData, MExtractMasterRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaMExtractMasterTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet AData, MExtractMasterRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaMExtractMasterTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTable Data = new MExtractTable();
            LoadViaForeignKey(MExtractTable.TableId, MExtractMasterTable.TableId, Data, new string[1]{"m_extract_id_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTable LoadViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractTable.TableId, MExtractMasterTable.TableId, ADataSet, new string[1]{"m_extract_id_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaMExtractMasterTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaMExtractMasterTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaMExtractMasterTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTable Data = new MExtractTable();
            LoadViaForeignKey(MExtractTable.TableId, MExtractMasterTable.TableId, Data, new string[1]{"m_extract_id_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTable LoadViaMExtractMasterTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaMExtractMasterTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaMExtractMaster(Int32 AExtractId, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractTable.TableId, MExtractMasterTable.TableId, new string[1]{"m_extract_id_i"},
                new System.Object[1]{AExtractId}, ATransaction);
        }

        /// auto generated
        public static int CountViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractTable.TableId, MExtractMasterTable.TableId, new string[1]{"m_extract_id_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaMExtractMasterTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractTable.TableId, MExtractMasterTable.TableId, new string[1]{"m_extract_id_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTable Data = new MExtractTable();
            LoadViaForeignKey(MExtractTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTable LoadViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTable Data = new MExtractTable();
            LoadViaForeignKey(MExtractTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTable Data = new MExtractTable();
            LoadViaForeignKey(MExtractTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPLocation(DataSet ADataSet, Int64 ASiteKey, Int32 ALocationKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractTable.TableId, PLocationTable.TableId, ADataSet, new string[2]{"p_site_key_n", "p_location_key_i"},
                new System.Object[2]{ASiteKey, ALocationKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLocation(DataSet AData, Int64 ASiteKey, Int32 ALocationKey, TDBTransaction ATransaction)
        {
            LoadViaPLocation(AData, ASiteKey, ALocationKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLocation(DataSet AData, Int64 ASiteKey, Int32 ALocationKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLocation(AData, ASiteKey, ALocationKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPLocation(Int64 ASiteKey, Int32 ALocationKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTable Data = new MExtractTable();
            LoadViaForeignKey(MExtractTable.TableId, PLocationTable.TableId, Data, new string[2]{"p_site_key_n", "p_location_key_i"},
                new System.Object[2]{ASiteKey, ALocationKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTable LoadViaPLocation(Int64 ASiteKey, Int32 ALocationKey, TDBTransaction ATransaction)
        {
            return LoadViaPLocation(ASiteKey, ALocationKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPLocation(Int64 ASiteKey, Int32 ALocationKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLocation(ASiteKey, ALocationKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLocationTemplate(DataSet ADataSet, PLocationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractTable.TableId, PLocationTable.TableId, ADataSet, new string[2]{"p_site_key_n", "p_location_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLocationTemplate(DataSet AData, PLocationRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPLocationTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLocationTemplate(DataSet AData, PLocationRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLocationTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPLocationTemplate(PLocationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTable Data = new MExtractTable();
            LoadViaForeignKey(MExtractTable.TableId, PLocationTable.TableId, Data, new string[2]{"p_site_key_n", "p_location_key_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTable LoadViaPLocationTemplate(PLocationRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPLocationTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPLocationTemplate(PLocationRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLocationTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPLocationTemplate(PLocationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLocationTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLocationTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractTable.TableId, PLocationTable.TableId, ADataSet, new string[2]{"p_site_key_n", "p_location_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPLocationTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPLocationTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLocationTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPLocationTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPLocationTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTable Data = new MExtractTable();
            LoadViaForeignKey(MExtractTable.TableId, PLocationTable.TableId, Data, new string[2]{"p_site_key_n", "p_location_key_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTable LoadViaPLocationTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPLocationTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTable LoadViaPLocationTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLocationTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPLocation(Int64 ASiteKey, Int32 ALocationKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractTable.TableId, PLocationTable.TableId, new string[2]{"p_site_key_n", "p_location_key_i"},
                new System.Object[2]{ASiteKey, ALocationKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPLocationTemplate(PLocationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractTable.TableId, PLocationTable.TableId, new string[2]{"p_site_key_n", "p_location_key_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPLocationTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractTable.TableId, PLocationTable.TableId, new string[2]{"p_site_key_n", "p_location_key_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 AExtractId, Int64 APartnerKey, Int64 ASiteKey, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(MExtractTable.TableId, new System.Object[3]{AExtractId, APartnerKey, ASiteKey}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(MExtractRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(MExtractTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(MExtractTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(MExtractTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Contains a list of extract type which is needed when extracts need to be rerun
    public class MExtractTypeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "MExtractType";

        /// original table name in database
        public const string DBTABLENAME = "m_extract_type";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, MExtractTypeTable.TableId) + " FROM PUB_m_extract_type") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(MExtractTypeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static MExtractTypeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTypeTable Data = new MExtractTypeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, MExtractTypeTable.TableId) + " FROM PUB_m_extract_type" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTypeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTypeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(MExtractTypeTable.TableId, ADataSet, new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTypeTable LoadByPrimaryKey(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTypeTable Data = new MExtractTypeTable();
            LoadByPrimaryKey(MExtractTypeTable.TableId, Data, new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTypeTable LoadByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTypeTable LoadByPrimaryKey(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, MExtractTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(MExtractTypeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, MExtractTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, MExtractTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTypeTable LoadUsingTemplate(MExtractTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTypeTable Data = new MExtractTypeTable();
            LoadUsingTemplate(MExtractTypeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTypeTable LoadUsingTemplate(MExtractTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTypeTable LoadUsingTemplate(MExtractTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTypeTable LoadUsingTemplate(MExtractTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(MExtractTypeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static MExtractTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractTypeTable Data = new MExtractTypeTable();
            LoadUsingTemplate(MExtractTypeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractTypeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_m_extract_type", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ACode, TDBTransaction ATransaction)
        {
            return Exists(MExtractTypeTable.TableId, new System.Object[1]{ACode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(MExtractTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_m_extract_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(MExtractTypeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(MExtractTypeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_m_extract_type" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(MExtractTypeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(MExtractTypeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(MExtractTypeTable.TableId, new System.Object[1]{ACode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(MExtractTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(MExtractTypeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(MExtractTypeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(MExtractTypeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Contains a list of parameters that an extract was run with (so it can be rerun)
    public class MExtractParameterAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "MExtractParameter";

        /// original table name in database
        public const string DBTABLENAME = "m_extract_parameter";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, MExtractParameterTable.TableId) + " FROM PUB_m_extract_parameter") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(MExtractParameterTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static MExtractParameterTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractParameterTable Data = new MExtractParameterTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, MExtractParameterTable.TableId) + " FROM PUB_m_extract_parameter" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractParameterTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 AExtractId, String AParameterCode, Int32 AValueIndex, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(MExtractParameterTable.TableId, ADataSet, new System.Object[3]{AExtractId, AParameterCode, AValueIndex}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AExtractId, String AParameterCode, Int32 AValueIndex, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AExtractId, AParameterCode, AValueIndex, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AExtractId, String AParameterCode, Int32 AValueIndex, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AExtractId, AParameterCode, AValueIndex, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadByPrimaryKey(Int32 AExtractId, String AParameterCode, Int32 AValueIndex, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractParameterTable Data = new MExtractParameterTable();
            LoadByPrimaryKey(MExtractParameterTable.TableId, Data, new System.Object[3]{AExtractId, AParameterCode, AValueIndex}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractParameterTable LoadByPrimaryKey(Int32 AExtractId, String AParameterCode, Int32 AValueIndex, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AExtractId, AParameterCode, AValueIndex, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadByPrimaryKey(Int32 AExtractId, String AParameterCode, Int32 AValueIndex, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AExtractId, AParameterCode, AValueIndex, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, MExtractParameterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(MExtractParameterTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, MExtractParameterRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, MExtractParameterRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadUsingTemplate(MExtractParameterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractParameterTable Data = new MExtractParameterTable();
            LoadUsingTemplate(MExtractParameterTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractParameterTable LoadUsingTemplate(MExtractParameterRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadUsingTemplate(MExtractParameterRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadUsingTemplate(MExtractParameterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(MExtractParameterTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static MExtractParameterTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractParameterTable Data = new MExtractParameterTable();
            LoadUsingTemplate(MExtractParameterTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractParameterTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_m_extract_parameter", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 AExtractId, String AParameterCode, Int32 AValueIndex, TDBTransaction ATransaction)
        {
            return Exists(MExtractParameterTable.TableId, new System.Object[3]{AExtractId, AParameterCode, AValueIndex}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(MExtractParameterRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_m_extract_parameter" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(MExtractParameterTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(MExtractParameterTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_m_extract_parameter" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(MExtractParameterTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(MExtractParameterTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaMExtractMaster(DataSet ADataSet, Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractParameterTable.TableId, MExtractMasterTable.TableId, ADataSet, new string[1]{"m_extract_id_i"},
                new System.Object[1]{AExtractId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaMExtractMaster(DataSet AData, Int32 AExtractId, TDBTransaction ATransaction)
        {
            LoadViaMExtractMaster(AData, AExtractId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMaster(DataSet AData, Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaMExtractMaster(AData, AExtractId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadViaMExtractMaster(Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractParameterTable Data = new MExtractParameterTable();
            LoadViaForeignKey(MExtractParameterTable.TableId, MExtractMasterTable.TableId, Data, new string[1]{"m_extract_id_i"},
                new System.Object[1]{AExtractId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractParameterTable LoadViaMExtractMaster(Int32 AExtractId, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMaster(AExtractId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadViaMExtractMaster(Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMaster(AExtractId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet ADataSet, MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractParameterTable.TableId, MExtractMasterTable.TableId, ADataSet, new string[1]{"m_extract_id_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet AData, MExtractMasterRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaMExtractMasterTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet AData, MExtractMasterRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaMExtractMasterTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractParameterTable Data = new MExtractParameterTable();
            LoadViaForeignKey(MExtractParameterTable.TableId, MExtractMasterTable.TableId, Data, new string[1]{"m_extract_id_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractParameterTable LoadViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(MExtractParameterTable.TableId, MExtractMasterTable.TableId, ADataSet, new string[1]{"m_extract_id_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaMExtractMasterTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaMExtractMasterTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadViaMExtractMasterTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            MExtractParameterTable Data = new MExtractParameterTable();
            LoadViaForeignKey(MExtractParameterTable.TableId, MExtractMasterTable.TableId, Data, new string[1]{"m_extract_id_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static MExtractParameterTable LoadViaMExtractMasterTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static MExtractParameterTable LoadViaMExtractMasterTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaMExtractMaster(Int32 AExtractId, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractParameterTable.TableId, MExtractMasterTable.TableId, new string[1]{"m_extract_id_i"},
                new System.Object[1]{AExtractId}, ATransaction);
        }

        /// auto generated
        public static int CountViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractParameterTable.TableId, MExtractMasterTable.TableId, new string[1]{"m_extract_id_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaMExtractMasterTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(MExtractParameterTable.TableId, MExtractMasterTable.TableId, new string[1]{"m_extract_id_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 AExtractId, String AParameterCode, Int32 AValueIndex, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(MExtractParameterTable.TableId, new System.Object[3]{AExtractId, AParameterCode, AValueIndex}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(MExtractParameterRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(MExtractParameterTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(MExtractParameterTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(MExtractParameterTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Lists mailings that are being tracked.   When entering gifts, the mailing that motivated the gift can be indicated.
    public class PMailingAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PMailing";

        /// original table name in database
        public const string DBTABLENAME = "p_mailing";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PMailingTable.TableId) + " FROM PUB_p_mailing") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PMailingTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PMailingTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMailingTable Data = new PMailingTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PMailingTable.TableId) + " FROM PUB_p_mailing" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMailingTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMailingTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PMailingTable.TableId, ADataSet, new System.Object[1]{AMailingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AMailingCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AMailingCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AMailingCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMailingTable LoadByPrimaryKey(String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMailingTable Data = new PMailingTable();
            LoadByPrimaryKey(PMailingTable.TableId, Data, new System.Object[1]{AMailingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMailingTable LoadByPrimaryKey(String AMailingCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AMailingCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMailingTable LoadByPrimaryKey(String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AMailingCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PMailingTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PMailingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PMailingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMailingTable LoadUsingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMailingTable Data = new PMailingTable();
            LoadUsingTemplate(PMailingTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMailingTable LoadUsingTemplate(PMailingRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMailingTable LoadUsingTemplate(PMailingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMailingTable LoadUsingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PMailingTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PMailingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMailingTable Data = new PMailingTable();
            LoadUsingTemplate(PMailingTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMailingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMailingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_mailing", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AMailingCode, TDBTransaction ATransaction)
        {
            return Exists(PMailingTable.TableId, new System.Object[1]{AMailingCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_mailing" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PMailingTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PMailingTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_mailing" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PMailingTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PMailingTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AMailingCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PMailingTable.TableId, new System.Object[1]{AMailingCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PMailingTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PMailingTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PMailingTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// This table contains the address layouts generally available for the user.
    public class PAddressLayoutCodeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PAddressLayoutCode";

        /// original table name in database
        public const string DBTABLENAME = "p_address_layout_code";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PAddressLayoutCodeTable.TableId) + " FROM PUB_p_address_layout_code") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PAddressLayoutCodeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PAddressLayoutCodeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutCodeTable Data = new PAddressLayoutCodeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PAddressLayoutCodeTable.TableId) + " FROM PUB_p_address_layout_code" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PAddressLayoutCodeTable.TableId, ADataSet, new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadByPrimaryKey(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutCodeTable Data = new PAddressLayoutCodeTable();
            LoadByPrimaryKey(PAddressLayoutCodeTable.TableId, Data, new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadByPrimaryKey(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PAddressLayoutCodeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PAddressLayoutCodeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PAddressLayoutCodeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadUsingTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutCodeTable Data = new PAddressLayoutCodeTable();
            LoadUsingTemplate(PAddressLayoutCodeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadUsingTemplate(PAddressLayoutCodeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadUsingTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadUsingTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PAddressLayoutCodeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PAddressLayoutCodeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutCodeTable Data = new PAddressLayoutCodeTable();
            LoadUsingTemplate(PAddressLayoutCodeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_address_layout_code", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ACode, TDBTransaction ATransaction)
        {
            return Exists(PAddressLayoutCodeTable.TableId, new System.Object[1]{ACode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_address_layout_code" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PAddressLayoutCodeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PAddressLayoutCodeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_address_layout_code" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PAddressLayoutCodeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PAddressLayoutCodeTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPCountry(DataSet ADataSet, String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 8);
            ParametersArray[0].Value = ((object)(ACountryCode));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_address_layout_code", AFieldList, PAddressLayoutCodeTable.TableId) +
                            " FROM PUB_p_address_layout_code, PUB_p_address_layout WHERE " +
                            "PUB_p_address_layout.p_address_layout_code_c = PUB_p_address_layout_code.p_code_c AND PUB_p_address_layout.p_country_code_c = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PAddressLayoutCodeTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static PAddressLayoutCodeTable LoadViaPCountry(String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PAddressLayoutCodeTable Data = new PAddressLayoutCodeTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPCountry(FillDataSet, ACountryCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadViaPCountry(String ACountryCode, TDBTransaction ATransaction)
        {
            return LoadViaPCountry(ACountryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadViaPCountry(String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountry(ACountryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet ADataSet, PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_address_layout_code", AFieldList, PAddressLayoutCodeTable.TableId) +
                            " FROM PUB_p_address_layout_code, PUB_p_address_layout, PUB_p_country WHERE " +
                            "PUB_p_address_layout.p_address_layout_code_c = PUB_p_address_layout_code.p_code_c AND PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c") +
                            GenerateWhereClauseLong("PUB_p_country", PCountryTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PAddressLayoutCodeTable.TableId), ATransaction,
                            GetParametersForWhereClause(PCountryTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
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
        public static PAddressLayoutCodeTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PAddressLayoutCodeTable Data = new PAddressLayoutCodeTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPCountryTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_address_layout_code", AFieldList, PAddressLayoutCodeTable.TableId) +
                            " FROM PUB_p_address_layout_code, PUB_p_address_layout, PUB_p_country WHERE " +
                            "PUB_p_address_layout.p_address_layout_code_c = PUB_p_address_layout_code.p_code_c AND PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c") +
                            GenerateWhereClauseLong("PUB_p_country", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PAddressLayoutCodeTable.TableId), ATransaction,
                            GetParametersForWhereClause(PAddressLayoutCodeTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
        public static PAddressLayoutCodeTable LoadViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PAddressLayoutCodeTable Data = new PAddressLayoutCodeTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPCountryTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutCodeTable LoadViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPCountry(String ACountryCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 8);
            ParametersArray[0].Value = ((object)(ACountryCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_address_layout_code, PUB_p_address_layout WHERE " +
                        "PUB_p_address_layout.p_address_layout_code_c = PUB_p_address_layout_code.p_code_c AND PUB_p_address_layout.p_country_code_c = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_address_layout_code, PUB_p_address_layout, PUB_p_country WHERE " +
                        "PUB_p_address_layout.p_address_layout_code_c = PUB_p_address_layout_code.p_code_c AND PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c" +
                        GenerateWhereClauseLong("PUB_p_address_layout", PAddressLayoutCodeTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PCountryTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_address_layout_code, PUB_p_address_layout, PUB_p_country WHERE " +
                        "PUB_p_address_layout.p_address_layout_code_c = PUB_p_address_layout_code.p_code_c AND PUB_p_address_layout.p_country_code_c = PUB_p_country.p_country_code_c" +
                        GenerateWhereClauseLong("PUB_p_address_layout", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PAddressLayoutCodeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PAddressLayoutCodeTable.TableId, new System.Object[1]{ACode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PAddressLayoutCodeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PAddressLayoutCodeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PAddressLayoutCodeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// This table contains the address lines used in laying out an address. Eg a form letter address layout
    public class PAddressLayoutAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PAddressLayout";

        /// original table name in database
        public const string DBTABLENAME = "p_address_layout";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PAddressLayoutTable.TableId) + " FROM PUB_p_address_layout") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PAddressLayoutTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PAddressLayoutTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutTable Data = new PAddressLayoutTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PAddressLayoutTable.TableId) + " FROM PUB_p_address_layout" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ACountryCode, String AAddressLayoutCode, Int32 AAddressLineNumber, String AAddressLineCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PAddressLayoutTable.TableId, ADataSet, new System.Object[4]{ACountryCode, AAddressLayoutCode, AAddressLineNumber, AAddressLineCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACountryCode, String AAddressLayoutCode, Int32 AAddressLineNumber, String AAddressLineCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACountryCode, AAddressLayoutCode, AAddressLineNumber, AAddressLineCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACountryCode, String AAddressLayoutCode, Int32 AAddressLineNumber, String AAddressLineCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACountryCode, AAddressLayoutCode, AAddressLineNumber, AAddressLineCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadByPrimaryKey(String ACountryCode, String AAddressLayoutCode, Int32 AAddressLineNumber, String AAddressLineCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutTable Data = new PAddressLayoutTable();
            LoadByPrimaryKey(PAddressLayoutTable.TableId, Data, new System.Object[4]{ACountryCode, AAddressLayoutCode, AAddressLineNumber, AAddressLineCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutTable LoadByPrimaryKey(String ACountryCode, String AAddressLayoutCode, Int32 AAddressLineNumber, String AAddressLineCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACountryCode, AAddressLayoutCode, AAddressLineNumber, AAddressLineCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadByPrimaryKey(String ACountryCode, String AAddressLayoutCode, Int32 AAddressLineNumber, String AAddressLineCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACountryCode, AAddressLayoutCode, AAddressLineNumber, AAddressLineCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PAddressLayoutRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PAddressLayoutTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PAddressLayoutRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PAddressLayoutRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadUsingTemplate(PAddressLayoutRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutTable Data = new PAddressLayoutTable();
            LoadUsingTemplate(PAddressLayoutTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutTable LoadUsingTemplate(PAddressLayoutRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadUsingTemplate(PAddressLayoutRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadUsingTemplate(PAddressLayoutRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PAddressLayoutTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PAddressLayoutTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutTable Data = new PAddressLayoutTable();
            LoadUsingTemplate(PAddressLayoutTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_address_layout", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ACountryCode, String AAddressLayoutCode, Int32 AAddressLineNumber, String AAddressLineCode, TDBTransaction ATransaction)
        {
            return Exists(PAddressLayoutTable.TableId, new System.Object[4]{ACountryCode, AAddressLayoutCode, AAddressLineNumber, AAddressLineCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PAddressLayoutRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_address_layout" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PAddressLayoutTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PAddressLayoutTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_address_layout" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PAddressLayoutTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PAddressLayoutTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPCountry(DataSet ADataSet, String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PAddressLayoutTable.TableId, PCountryTable.TableId, ADataSet, new string[1]{"p_country_code_c"},
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
        public static PAddressLayoutTable LoadViaPCountry(String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutTable Data = new PAddressLayoutTable();
            LoadViaForeignKey(PAddressLayoutTable.TableId, PCountryTable.TableId, Data, new string[1]{"p_country_code_c"},
                new System.Object[1]{ACountryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPCountry(String ACountryCode, TDBTransaction ATransaction)
        {
            return LoadViaPCountry(ACountryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPCountry(String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountry(ACountryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet ADataSet, PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PAddressLayoutTable.TableId, PCountryTable.TableId, ADataSet, new string[1]{"p_country_code_c"},
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
        public static PAddressLayoutTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutTable Data = new PAddressLayoutTable();
            LoadViaForeignKey(PAddressLayoutTable.TableId, PCountryTable.TableId, Data, new string[1]{"p_country_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PAddressLayoutTable.TableId, PCountryTable.TableId, ADataSet, new string[1]{"p_country_code_c"},
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
        public static PAddressLayoutTable LoadViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutTable Data = new PAddressLayoutTable();
            LoadViaForeignKey(PAddressLayoutTable.TableId, PCountryTable.TableId, Data, new string[1]{"p_country_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPCountry(String ACountryCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PAddressLayoutTable.TableId, PCountryTable.TableId, new string[1]{"p_country_code_c"},
                new System.Object[1]{ACountryCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PAddressLayoutTable.TableId, PCountryTable.TableId, new string[1]{"p_country_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PAddressLayoutTable.TableId, PCountryTable.TableId, new string[1]{"p_country_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCode(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PAddressLayoutTable.TableId, PAddressLayoutCodeTable.TableId, ADataSet, new string[1]{"p_address_layout_code_c"},
                new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PAddressLayoutTable LoadViaPAddressLayoutCode(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutTable Data = new PAddressLayoutTable();
            LoadViaForeignKey(PAddressLayoutTable.TableId, PAddressLayoutCodeTable.TableId, Data, new string[1]{"p_address_layout_code_c"},
                new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPAddressLayoutCode(String ACode, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCode(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPAddressLayoutCode(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCode(ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(DataSet ADataSet, PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PAddressLayoutTable.TableId, PAddressLayoutCodeTable.TableId, ADataSet, new string[1]{"p_address_layout_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PAddressLayoutTable LoadViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutTable Data = new PAddressLayoutTable();
            LoadViaForeignKey(PAddressLayoutTable.TableId, PAddressLayoutCodeTable.TableId, Data, new string[1]{"p_address_layout_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PAddressLayoutTable.TableId, PAddressLayoutCodeTable.TableId, ADataSet, new string[1]{"p_address_layout_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PAddressLayoutTable LoadViaPAddressLayoutCodeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLayoutTable Data = new PAddressLayoutTable();
            LoadViaForeignKey(PAddressLayoutTable.TableId, PAddressLayoutCodeTable.TableId, Data, new string[1]{"p_address_layout_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPAddressLayoutCodeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLayoutTable LoadViaPAddressLayoutCodeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPAddressLayoutCode(String ACode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PAddressLayoutTable.TableId, PAddressLayoutCodeTable.TableId, new string[1]{"p_address_layout_code_c"},
                new System.Object[1]{ACode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PAddressLayoutTable.TableId, PAddressLayoutCodeTable.TableId, new string[1]{"p_address_layout_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPAddressLayoutCodeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PAddressLayoutTable.TableId, PAddressLayoutCodeTable.TableId, new string[1]{"p_address_layout_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ACountryCode, String AAddressLayoutCode, Int32 AAddressLineNumber, String AAddressLineCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PAddressLayoutTable.TableId, new System.Object[4]{ACountryCode, AAddressLayoutCode, AAddressLineNumber, AAddressLineCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PAddressLayoutRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PAddressLayoutTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PAddressLayoutTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PAddressLayoutTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// This contains the elements which make up an address. Eg Name etc
    public class PAddressElementAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PAddressElement";

        /// original table name in database
        public const string DBTABLENAME = "p_address_element";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PAddressElementTable.TableId) + " FROM PUB_p_address_element") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PAddressElementTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PAddressElementTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressElementTable Data = new PAddressElementTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PAddressElementTable.TableId) + " FROM PUB_p_address_element" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressElementTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressElementTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AAddressElementCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PAddressElementTable.TableId, ADataSet, new System.Object[1]{AAddressElementCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AAddressElementCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AAddressElementCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AAddressElementCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AAddressElementCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressElementTable LoadByPrimaryKey(String AAddressElementCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressElementTable Data = new PAddressElementTable();
            LoadByPrimaryKey(PAddressElementTable.TableId, Data, new System.Object[1]{AAddressElementCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressElementTable LoadByPrimaryKey(String AAddressElementCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AAddressElementCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressElementTable LoadByPrimaryKey(String AAddressElementCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AAddressElementCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PAddressElementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PAddressElementTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PAddressElementRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PAddressElementRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressElementTable LoadUsingTemplate(PAddressElementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressElementTable Data = new PAddressElementTable();
            LoadUsingTemplate(PAddressElementTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressElementTable LoadUsingTemplate(PAddressElementRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressElementTable LoadUsingTemplate(PAddressElementRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressElementTable LoadUsingTemplate(PAddressElementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PAddressElementTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PAddressElementTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressElementTable Data = new PAddressElementTable();
            LoadUsingTemplate(PAddressElementTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressElementTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressElementTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_address_element", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AAddressElementCode, TDBTransaction ATransaction)
        {
            return Exists(PAddressElementTable.TableId, new System.Object[1]{AAddressElementCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PAddressElementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_address_element" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PAddressElementTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PAddressElementTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_address_element" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PAddressElementTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PAddressElementTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AAddressElementCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PAddressElementTable.TableId, new System.Object[1]{AAddressElementCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PAddressElementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PAddressElementTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PAddressElementTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PAddressElementTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// This is an address line which consists of address elements.  Used along with p_address_layout and p_address_element to define layout of an address for different countries.
    public class PAddressLineAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PAddressLine";

        /// original table name in database
        public const string DBTABLENAME = "p_address_line";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PAddressLineTable.TableId) + " FROM PUB_p_address_line") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PAddressLineTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PAddressLineTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLineTable Data = new PAddressLineTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PAddressLineTable.TableId) + " FROM PUB_p_address_line" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLineTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AAddressLineCode, Int32 AAddressElementPosition, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PAddressLineTable.TableId, ADataSet, new System.Object[2]{AAddressLineCode, AAddressElementPosition}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AAddressLineCode, Int32 AAddressElementPosition, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AAddressLineCode, AAddressElementPosition, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AAddressLineCode, Int32 AAddressElementPosition, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AAddressLineCode, AAddressElementPosition, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadByPrimaryKey(String AAddressLineCode, Int32 AAddressElementPosition, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLineTable Data = new PAddressLineTable();
            LoadByPrimaryKey(PAddressLineTable.TableId, Data, new System.Object[2]{AAddressLineCode, AAddressElementPosition}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLineTable LoadByPrimaryKey(String AAddressLineCode, Int32 AAddressElementPosition, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AAddressLineCode, AAddressElementPosition, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadByPrimaryKey(String AAddressLineCode, Int32 AAddressElementPosition, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AAddressLineCode, AAddressElementPosition, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PAddressLineRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PAddressLineTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PAddressLineRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PAddressLineRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadUsingTemplate(PAddressLineRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLineTable Data = new PAddressLineTable();
            LoadUsingTemplate(PAddressLineTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLineTable LoadUsingTemplate(PAddressLineRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadUsingTemplate(PAddressLineRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadUsingTemplate(PAddressLineRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PAddressLineTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PAddressLineTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLineTable Data = new PAddressLineTable();
            LoadUsingTemplate(PAddressLineTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLineTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_address_line", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AAddressLineCode, Int32 AAddressElementPosition, TDBTransaction ATransaction)
        {
            return Exists(PAddressLineTable.TableId, new System.Object[2]{AAddressLineCode, AAddressElementPosition}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PAddressLineRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_address_line" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PAddressLineTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PAddressLineTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_address_line" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PAddressLineTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PAddressLineTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPAddressElement(DataSet ADataSet, String AAddressElementCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PAddressLineTable.TableId, PAddressElementTable.TableId, ADataSet, new string[1]{"p_address_element_code_c"},
                new System.Object[1]{AAddressElementCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPAddressElement(DataSet AData, String AAddressElementCode, TDBTransaction ATransaction)
        {
            LoadViaPAddressElement(AData, AAddressElementCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressElement(DataSet AData, String AAddressElementCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPAddressElement(AData, AAddressElementCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadViaPAddressElement(String AAddressElementCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLineTable Data = new PAddressLineTable();
            LoadViaForeignKey(PAddressLineTable.TableId, PAddressElementTable.TableId, Data, new string[1]{"p_address_element_code_c"},
                new System.Object[1]{AAddressElementCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLineTable LoadViaPAddressElement(String AAddressElementCode, TDBTransaction ATransaction)
        {
            return LoadViaPAddressElement(AAddressElementCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadViaPAddressElement(String AAddressElementCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressElement(AAddressElementCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressElementTemplate(DataSet ADataSet, PAddressElementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PAddressLineTable.TableId, PAddressElementTable.TableId, ADataSet, new string[1]{"p_address_element_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPAddressElementTemplate(DataSet AData, PAddressElementRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPAddressElementTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressElementTemplate(DataSet AData, PAddressElementRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPAddressElementTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadViaPAddressElementTemplate(PAddressElementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLineTable Data = new PAddressLineTable();
            LoadViaForeignKey(PAddressLineTable.TableId, PAddressElementTable.TableId, Data, new string[1]{"p_address_element_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLineTable LoadViaPAddressElementTemplate(PAddressElementRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPAddressElementTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadViaPAddressElementTemplate(PAddressElementRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressElementTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadViaPAddressElementTemplate(PAddressElementRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressElementTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressElementTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PAddressLineTable.TableId, PAddressElementTable.TableId, ADataSet, new string[1]{"p_address_element_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPAddressElementTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPAddressElementTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressElementTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPAddressElementTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadViaPAddressElementTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddressLineTable Data = new PAddressLineTable();
            LoadViaForeignKey(PAddressLineTable.TableId, PAddressElementTable.TableId, Data, new string[1]{"p_address_element_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddressLineTable LoadViaPAddressElementTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPAddressElementTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddressLineTable LoadViaPAddressElementTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressElementTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPAddressElement(String AAddressElementCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PAddressLineTable.TableId, PAddressElementTable.TableId, new string[1]{"p_address_element_code_c"},
                new System.Object[1]{AAddressElementCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPAddressElementTemplate(PAddressElementRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PAddressLineTable.TableId, PAddressElementTable.TableId, new string[1]{"p_address_element_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPAddressElementTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PAddressLineTable.TableId, PAddressElementTable.TableId, new string[1]{"p_address_element_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AAddressLineCode, Int32 AAddressElementPosition, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PAddressLineTable.TableId, new System.Object[2]{AAddressLineCode, AAddressElementPosition}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PAddressLineRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PAddressLineTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PAddressLineTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PAddressLineTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// This is used to override titles that might be different in the address than that in the letter.
    /// Eg      German     Herr   Herrn
    /// ""Sehr geehrter Herr Starling"" in the letter and ""Herrn Starling"" in the address.
    public class PAddresseeTitleOverrideAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PAddresseeTitleOverride";

        /// original table name in database
        public const string DBTABLENAME = "p_addressee_title_override";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PAddresseeTitleOverrideTable.TableId) + " FROM PUB_p_addressee_title_override") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PAddresseeTitleOverrideTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PAddresseeTitleOverrideTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddresseeTitleOverrideTable Data = new PAddresseeTitleOverrideTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PAddresseeTitleOverrideTable.TableId) + " FROM PUB_p_addressee_title_override" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ALanguageCode, String ATitle, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PAddresseeTitleOverrideTable.TableId, ADataSet, new System.Object[2]{ALanguageCode, ATitle}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ALanguageCode, String ATitle, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALanguageCode, ATitle, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ALanguageCode, String ATitle, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALanguageCode, ATitle, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadByPrimaryKey(String ALanguageCode, String ATitle, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddresseeTitleOverrideTable Data = new PAddresseeTitleOverrideTable();
            LoadByPrimaryKey(PAddresseeTitleOverrideTable.TableId, Data, new System.Object[2]{ALanguageCode, ATitle}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadByPrimaryKey(String ALanguageCode, String ATitle, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALanguageCode, ATitle, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadByPrimaryKey(String ALanguageCode, String ATitle, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALanguageCode, ATitle, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PAddresseeTitleOverrideRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PAddresseeTitleOverrideTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PAddresseeTitleOverrideRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PAddresseeTitleOverrideRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadUsingTemplate(PAddresseeTitleOverrideRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddresseeTitleOverrideTable Data = new PAddresseeTitleOverrideTable();
            LoadUsingTemplate(PAddresseeTitleOverrideTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadUsingTemplate(PAddresseeTitleOverrideRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadUsingTemplate(PAddresseeTitleOverrideRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadUsingTemplate(PAddresseeTitleOverrideRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PAddresseeTitleOverrideTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PAddresseeTitleOverrideTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddresseeTitleOverrideTable Data = new PAddresseeTitleOverrideTable();
            LoadUsingTemplate(PAddresseeTitleOverrideTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_addressee_title_override", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ALanguageCode, String ATitle, TDBTransaction ATransaction)
        {
            return Exists(PAddresseeTitleOverrideTable.TableId, new System.Object[2]{ALanguageCode, ATitle}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PAddresseeTitleOverrideRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_addressee_title_override" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PAddresseeTitleOverrideTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PAddresseeTitleOverrideTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_addressee_title_override" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PAddresseeTitleOverrideTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PAddresseeTitleOverrideTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet ADataSet, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PAddresseeTitleOverrideTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_language_code_c"},
                new System.Object[1]{ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PAddresseeTitleOverrideTable LoadViaPLanguage(String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddresseeTitleOverrideTable Data = new PAddresseeTitleOverrideTable();
            LoadViaForeignKey(PAddresseeTitleOverrideTable.TableId, PLanguageTable.TableId, Data, new string[1]{"p_language_code_c"},
                new System.Object[1]{ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadViaPLanguage(String ALanguageCode, TDBTransaction ATransaction)
        {
            return LoadViaPLanguage(ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadViaPLanguage(String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguage(ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PAddresseeTitleOverrideTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_language_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PAddresseeTitleOverrideTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddresseeTitleOverrideTable Data = new PAddresseeTitleOverrideTable();
            LoadViaForeignKey(PAddresseeTitleOverrideTable.TableId, PLanguageTable.TableId, Data, new string[1]{"p_language_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PAddresseeTitleOverrideTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_language_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PAddresseeTitleOverrideTable LoadViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PAddresseeTitleOverrideTable Data = new PAddresseeTitleOverrideTable();
            LoadViaForeignKey(PAddresseeTitleOverrideTable.TableId, PLanguageTable.TableId, Data, new string[1]{"p_language_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PAddresseeTitleOverrideTable LoadViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPLanguage(String ALanguageCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PAddresseeTitleOverrideTable.TableId, PLanguageTable.TableId, new string[1]{"p_language_code_c"},
                new System.Object[1]{ALanguageCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PAddresseeTitleOverrideTable.TableId, PLanguageTable.TableId, new string[1]{"p_language_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PAddresseeTitleOverrideTable.TableId, PLanguageTable.TableId, new string[1]{"p_language_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ALanguageCode, String ATitle, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PAddresseeTitleOverrideTable.TableId, new System.Object[2]{ALanguageCode, ATitle}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PAddresseeTitleOverrideRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PAddresseeTitleOverrideTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PAddresseeTitleOverrideTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PAddresseeTitleOverrideTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Specific greetings from a user to a partner
    public class PCustomisedGreetingAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PCustomisedGreeting";

        /// original table name in database
        public const string DBTABLENAME = "p_customised_greeting";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PCustomisedGreetingTable.TableId) + " FROM PUB_p_customised_greeting") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PCustomisedGreetingTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PCustomisedGreetingTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCustomisedGreetingTable Data = new PCustomisedGreetingTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PCustomisedGreetingTable.TableId) + " FROM PUB_p_customised_greeting" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 APartnerKey, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PCustomisedGreetingTable.TableId, ADataSet, new System.Object[2]{APartnerKey, AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, String AUserId, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, AUserId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadByPrimaryKey(Int64 APartnerKey, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCustomisedGreetingTable Data = new PCustomisedGreetingTable();
            LoadByPrimaryKey(PCustomisedGreetingTable.TableId, Data, new System.Object[2]{APartnerKey, AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadByPrimaryKey(Int64 APartnerKey, String AUserId, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(APartnerKey, AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadByPrimaryKey(Int64 APartnerKey, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(APartnerKey, AUserId, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PCustomisedGreetingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PCustomisedGreetingTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PCustomisedGreetingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PCustomisedGreetingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadUsingTemplate(PCustomisedGreetingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCustomisedGreetingTable Data = new PCustomisedGreetingTable();
            LoadUsingTemplate(PCustomisedGreetingTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadUsingTemplate(PCustomisedGreetingRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadUsingTemplate(PCustomisedGreetingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadUsingTemplate(PCustomisedGreetingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PCustomisedGreetingTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PCustomisedGreetingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCustomisedGreetingTable Data = new PCustomisedGreetingTable();
            LoadUsingTemplate(PCustomisedGreetingTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_customised_greeting", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 APartnerKey, String AUserId, TDBTransaction ATransaction)
        {
            return Exists(PCustomisedGreetingTable.TableId, new System.Object[2]{APartnerKey, AUserId}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PCustomisedGreetingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_customised_greeting" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PCustomisedGreetingTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PCustomisedGreetingTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_customised_greeting" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PCustomisedGreetingTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PCustomisedGreetingTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PCustomisedGreetingTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCustomisedGreetingTable Data = new PCustomisedGreetingTable();
            LoadViaForeignKey(PCustomisedGreetingTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PCustomisedGreetingTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCustomisedGreetingTable Data = new PCustomisedGreetingTable();
            LoadViaForeignKey(PCustomisedGreetingTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PCustomisedGreetingTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCustomisedGreetingTable Data = new PCustomisedGreetingTable();
            LoadViaForeignKey(PCustomisedGreetingTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PCustomisedGreetingTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PCustomisedGreetingTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PCustomisedGreetingTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaSUser(DataSet ADataSet, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PCustomisedGreetingTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"s_user_id_c"},
                new System.Object[1]{AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUser(DataSet AData, String AUserId, TDBTransaction ATransaction)
        {
            LoadViaSUser(AData, AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUser(DataSet AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUser(AData, AUserId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaSUser(String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCustomisedGreetingTable Data = new PCustomisedGreetingTable();
            LoadViaForeignKey(PCustomisedGreetingTable.TableId, SUserTable.TableId, Data, new string[1]{"s_user_id_c"},
                new System.Object[1]{AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            return LoadViaSUser(AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaSUser(String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUser(AUserId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PCustomisedGreetingTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"s_user_id_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCustomisedGreetingTable Data = new PCustomisedGreetingTable();
            LoadViaForeignKey(PCustomisedGreetingTable.TableId, SUserTable.TableId, Data, new string[1]{"s_user_id_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaSUserTemplate(SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PCustomisedGreetingTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"s_user_id_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PCustomisedGreetingTable Data = new PCustomisedGreetingTable();
            LoadViaForeignKey(PCustomisedGreetingTable.TableId, SUserTable.TableId, Data, new string[1]{"s_user_id_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PCustomisedGreetingTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PCustomisedGreetingTable.TableId, SUserTable.TableId, new string[1]{"s_user_id_c"},
                new System.Object[1]{AUserId}, ATransaction);
        }

        /// auto generated
        public static int CountViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PCustomisedGreetingTable.TableId, SUserTable.TableId, new string[1]{"s_user_id_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaSUserTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PCustomisedGreetingTable.TableId, SUserTable.TableId, new string[1]{"s_user_id_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 APartnerKey, String AUserId, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PCustomisedGreetingTable.TableId, new System.Object[2]{APartnerKey, AUserId}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PCustomisedGreetingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PCustomisedGreetingTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PCustomisedGreetingTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PCustomisedGreetingTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Contains the text used in letters
    public class PFormalityAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PFormality";

        /// original table name in database
        public const string DBTABLENAME = "p_formality";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PFormalityTable.TableId) + " FROM PUB_p_formality") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PFormalityTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PFormalityTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormalityTable Data = new PFormalityTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PFormalityTable.TableId) + " FROM PUB_p_formality" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormalityTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ALanguageCode, String ACountryCode, String AAddresseeTypeCode, Int32 AFormalityLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PFormalityTable.TableId, ADataSet, new System.Object[4]{ALanguageCode, ACountryCode, AAddresseeTypeCode, AFormalityLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ALanguageCode, String ACountryCode, String AAddresseeTypeCode, Int32 AFormalityLevel, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALanguageCode, ACountryCode, AAddresseeTypeCode, AFormalityLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ALanguageCode, String ACountryCode, String AAddresseeTypeCode, Int32 AFormalityLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALanguageCode, ACountryCode, AAddresseeTypeCode, AFormalityLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadByPrimaryKey(String ALanguageCode, String ACountryCode, String AAddresseeTypeCode, Int32 AFormalityLevel, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormalityTable Data = new PFormalityTable();
            LoadByPrimaryKey(PFormalityTable.TableId, Data, new System.Object[4]{ALanguageCode, ACountryCode, AAddresseeTypeCode, AFormalityLevel}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormalityTable LoadByPrimaryKey(String ALanguageCode, String ACountryCode, String AAddresseeTypeCode, Int32 AFormalityLevel, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALanguageCode, ACountryCode, AAddresseeTypeCode, AFormalityLevel, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadByPrimaryKey(String ALanguageCode, String ACountryCode, String AAddresseeTypeCode, Int32 AFormalityLevel, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALanguageCode, ACountryCode, AAddresseeTypeCode, AFormalityLevel, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PFormalityRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PFormalityTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PFormalityRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PFormalityRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadUsingTemplate(PFormalityRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormalityTable Data = new PFormalityTable();
            LoadUsingTemplate(PFormalityTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormalityTable LoadUsingTemplate(PFormalityRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadUsingTemplate(PFormalityRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadUsingTemplate(PFormalityRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PFormalityTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PFormalityTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormalityTable Data = new PFormalityTable();
            LoadUsingTemplate(PFormalityTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormalityTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_formality", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ALanguageCode, String ACountryCode, String AAddresseeTypeCode, Int32 AFormalityLevel, TDBTransaction ATransaction)
        {
            return Exists(PFormalityTable.TableId, new System.Object[4]{ALanguageCode, ACountryCode, AAddresseeTypeCode, AFormalityLevel}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PFormalityRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_formality" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PFormalityTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PFormalityTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_formality" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PFormalityTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PFormalityTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet ADataSet, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormalityTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_language_code_c"},
                new System.Object[1]{ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PFormalityTable LoadViaPLanguage(String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormalityTable Data = new PFormalityTable();
            LoadViaForeignKey(PFormalityTable.TableId, PLanguageTable.TableId, Data, new string[1]{"p_language_code_c"},
                new System.Object[1]{ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormalityTable LoadViaPLanguage(String ALanguageCode, TDBTransaction ATransaction)
        {
            return LoadViaPLanguage(ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPLanguage(String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguage(ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormalityTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_language_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PFormalityTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormalityTable Data = new PFormalityTable();
            LoadViaForeignKey(PFormalityTable.TableId, PLanguageTable.TableId, Data, new string[1]{"p_language_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormalityTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormalityTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_language_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PFormalityTable LoadViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormalityTable Data = new PFormalityTable();
            LoadViaForeignKey(PFormalityTable.TableId, PLanguageTable.TableId, Data, new string[1]{"p_language_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormalityTable LoadViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPLanguage(String ALanguageCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormalityTable.TableId, PLanguageTable.TableId, new string[1]{"p_language_code_c"},
                new System.Object[1]{ALanguageCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormalityTable.TableId, PLanguageTable.TableId, new string[1]{"p_language_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormalityTable.TableId, PLanguageTable.TableId, new string[1]{"p_language_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPCountry(DataSet ADataSet, String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormalityTable.TableId, PCountryTable.TableId, ADataSet, new string[1]{"p_country_code_c"},
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
        public static PFormalityTable LoadViaPCountry(String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormalityTable Data = new PFormalityTable();
            LoadViaForeignKey(PFormalityTable.TableId, PCountryTable.TableId, Data, new string[1]{"p_country_code_c"},
                new System.Object[1]{ACountryCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormalityTable LoadViaPCountry(String ACountryCode, TDBTransaction ATransaction)
        {
            return LoadViaPCountry(ACountryCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPCountry(String ACountryCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountry(ACountryCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet ADataSet, PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormalityTable.TableId, PCountryTable.TableId, ADataSet, new string[1]{"p_country_code_c"},
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
        public static PFormalityTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormalityTable Data = new PFormalityTable();
            LoadViaForeignKey(PFormalityTable.TableId, PCountryTable.TableId, Data, new string[1]{"p_country_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormalityTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPCountryTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormalityTable.TableId, PCountryTable.TableId, ADataSet, new string[1]{"p_country_code_c"},
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
        public static PFormalityTable LoadViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormalityTable Data = new PFormalityTable();
            LoadViaForeignKey(PFormalityTable.TableId, PCountryTable.TableId, Data, new string[1]{"p_country_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormalityTable LoadViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPCountryTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPCountry(String ACountryCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormalityTable.TableId, PCountryTable.TableId, new string[1]{"p_country_code_c"},
                new System.Object[1]{ACountryCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPCountryTemplate(PCountryRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormalityTable.TableId, PCountryTable.TableId, new string[1]{"p_country_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPCountryTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormalityTable.TableId, PCountryTable.TableId, new string[1]{"p_country_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPAddresseeType(DataSet ADataSet, String AAddresseeTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormalityTable.TableId, PAddresseeTypeTable.TableId, ADataSet, new string[1]{"p_addressee_type_code_c"},
                new System.Object[1]{AAddresseeTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPAddresseeType(DataSet AData, String AAddresseeTypeCode, TDBTransaction ATransaction)
        {
            LoadViaPAddresseeType(AData, AAddresseeTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddresseeType(DataSet AData, String AAddresseeTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPAddresseeType(AData, AAddresseeTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPAddresseeType(String AAddresseeTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormalityTable Data = new PFormalityTable();
            LoadViaForeignKey(PFormalityTable.TableId, PAddresseeTypeTable.TableId, Data, new string[1]{"p_addressee_type_code_c"},
                new System.Object[1]{AAddresseeTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormalityTable LoadViaPAddresseeType(String AAddresseeTypeCode, TDBTransaction ATransaction)
        {
            return LoadViaPAddresseeType(AAddresseeTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPAddresseeType(String AAddresseeTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddresseeType(AAddresseeTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddresseeTypeTemplate(DataSet ADataSet, PAddresseeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormalityTable.TableId, PAddresseeTypeTable.TableId, ADataSet, new string[1]{"p_addressee_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPAddresseeTypeTemplate(DataSet AData, PAddresseeTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPAddresseeTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddresseeTypeTemplate(DataSet AData, PAddresseeTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPAddresseeTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPAddresseeTypeTemplate(PAddresseeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormalityTable Data = new PFormalityTable();
            LoadViaForeignKey(PFormalityTable.TableId, PAddresseeTypeTable.TableId, Data, new string[1]{"p_addressee_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormalityTable LoadViaPAddresseeTypeTemplate(PAddresseeTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPAddresseeTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPAddresseeTypeTemplate(PAddresseeTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddresseeTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPAddresseeTypeTemplate(PAddresseeTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddresseeTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddresseeTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormalityTable.TableId, PAddresseeTypeTable.TableId, ADataSet, new string[1]{"p_addressee_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPAddresseeTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPAddresseeTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddresseeTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPAddresseeTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPAddresseeTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormalityTable Data = new PFormalityTable();
            LoadViaForeignKey(PFormalityTable.TableId, PAddresseeTypeTable.TableId, Data, new string[1]{"p_addressee_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormalityTable LoadViaPAddresseeTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPAddresseeTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormalityTable LoadViaPAddresseeTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddresseeTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPAddresseeType(String AAddresseeTypeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormalityTable.TableId, PAddresseeTypeTable.TableId, new string[1]{"p_addressee_type_code_c"},
                new System.Object[1]{AAddresseeTypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPAddresseeTypeTemplate(PAddresseeTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormalityTable.TableId, PAddresseeTypeTable.TableId, new string[1]{"p_addressee_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPAddresseeTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormalityTable.TableId, PAddresseeTypeTable.TableId, new string[1]{"p_addressee_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ALanguageCode, String ACountryCode, String AAddresseeTypeCode, Int32 AFormalityLevel, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PFormalityTable.TableId, new System.Object[4]{ALanguageCode, ACountryCode, AAddresseeTypeCode, AFormalityLevel}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PFormalityRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PFormalityTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PFormalityTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PFormalityTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Text for form letters
    public class PFormLetterBodyAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PFormLetterBody";

        /// original table name in database
        public const string DBTABLENAME = "p_form_letter_body";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PFormLetterBodyTable.TableId) + " FROM PUB_p_form_letter_body") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PFormLetterBodyTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PFormLetterBodyTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterBodyTable Data = new PFormLetterBodyTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PFormLetterBodyTable.TableId) + " FROM PUB_p_form_letter_body" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterBodyTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ABodyName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PFormLetterBodyTable.TableId, ADataSet, new System.Object[1]{ABodyName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ABodyName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ABodyName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ABodyName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ABodyName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadByPrimaryKey(String ABodyName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterBodyTable Data = new PFormLetterBodyTable();
            LoadByPrimaryKey(PFormLetterBodyTable.TableId, Data, new System.Object[1]{ABodyName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterBodyTable LoadByPrimaryKey(String ABodyName, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ABodyName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadByPrimaryKey(String ABodyName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ABodyName, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PFormLetterBodyTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PFormLetterBodyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PFormLetterBodyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadUsingTemplate(PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterBodyTable Data = new PFormLetterBodyTable();
            LoadUsingTemplate(PFormLetterBodyTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterBodyTable LoadUsingTemplate(PFormLetterBodyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadUsingTemplate(PFormLetterBodyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadUsingTemplate(PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PFormLetterBodyTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PFormLetterBodyTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterBodyTable Data = new PFormLetterBodyTable();
            LoadUsingTemplate(PFormLetterBodyTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterBodyTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_form_letter_body", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ABodyName, TDBTransaction ATransaction)
        {
            return Exists(PFormLetterBodyTable.TableId, new System.Object[1]{ABodyName}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_form_letter_body" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PFormLetterBodyTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PFormLetterBodyTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_form_letter_body" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PFormLetterBodyTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PFormLetterBodyTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaSUser(DataSet ADataSet, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterBodyTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"p_owner_c"},
                new System.Object[1]{AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUser(DataSet AData, String AUserId, TDBTransaction ATransaction)
        {
            LoadViaSUser(AData, AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUser(DataSet AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUser(AData, AUserId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadViaSUser(String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterBodyTable Data = new PFormLetterBodyTable();
            LoadViaForeignKey(PFormLetterBodyTable.TableId, SUserTable.TableId, Data, new string[1]{"p_owner_c"},
                new System.Object[1]{AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterBodyTable LoadViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            return LoadViaSUser(AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadViaSUser(String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUser(AUserId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterBodyTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"p_owner_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterBodyTable Data = new PFormLetterBodyTable();
            LoadViaForeignKey(PFormLetterBodyTable.TableId, SUserTable.TableId, Data, new string[1]{"p_owner_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterBodyTable LoadViaSUserTemplate(SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterBodyTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"p_owner_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterBodyTable Data = new PFormLetterBodyTable();
            LoadViaForeignKey(PFormLetterBodyTable.TableId, SUserTable.TableId, Data, new string[1]{"p_owner_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterBodyTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterBodyTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterBodyTable.TableId, SUserTable.TableId, new string[1]{"p_owner_c"},
                new System.Object[1]{AUserId}, ATransaction);
        }

        /// auto generated
        public static int CountViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterBodyTable.TableId, SUserTable.TableId, new string[1]{"p_owner_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaSUserTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterBodyTable.TableId, SUserTable.TableId, new string[1]{"p_owner_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ABodyName, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PFormLetterBodyTable.TableId, new System.Object[1]{ABodyName}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PFormLetterBodyTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PFormLetterBodyTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PFormLetterBodyTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Configuration info for form letters
    public class PFormLetterDesignAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PFormLetterDesign";

        /// original table name in database
        public const string DBTABLENAME = "p_form_letter_design";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PFormLetterDesignTable.TableId) + " FROM PUB_p_form_letter_design") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PFormLetterDesignTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PFormLetterDesignTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterDesignTable Data = new PFormLetterDesignTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PFormLetterDesignTable.TableId) + " FROM PUB_p_form_letter_design" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterDesignTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ADesignName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PFormLetterDesignTable.TableId, ADataSet, new System.Object[1]{ADesignName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ADesignName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ADesignName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ADesignName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ADesignName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadByPrimaryKey(String ADesignName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterDesignTable Data = new PFormLetterDesignTable();
            LoadByPrimaryKey(PFormLetterDesignTable.TableId, Data, new System.Object[1]{ADesignName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterDesignTable LoadByPrimaryKey(String ADesignName, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ADesignName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadByPrimaryKey(String ADesignName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ADesignName, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PFormLetterDesignRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PFormLetterDesignTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PFormLetterDesignRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PFormLetterDesignRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadUsingTemplate(PFormLetterDesignRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterDesignTable Data = new PFormLetterDesignTable();
            LoadUsingTemplate(PFormLetterDesignTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterDesignTable LoadUsingTemplate(PFormLetterDesignRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadUsingTemplate(PFormLetterDesignRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadUsingTemplate(PFormLetterDesignRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PFormLetterDesignTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PFormLetterDesignTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterDesignTable Data = new PFormLetterDesignTable();
            LoadUsingTemplate(PFormLetterDesignTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterDesignTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_form_letter_design", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ADesignName, TDBTransaction ATransaction)
        {
            return Exists(PFormLetterDesignTable.TableId, new System.Object[1]{ADesignName}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PFormLetterDesignRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_form_letter_design" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PFormLetterDesignTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PFormLetterDesignTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_form_letter_design" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PFormLetterDesignTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PFormLetterDesignTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPFormLetterBody(DataSet ADataSet, String ABodyName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterDesignTable.TableId, PFormLetterBodyTable.TableId, ADataSet, new string[1]{"p_body_name_c"},
                new System.Object[1]{ABodyName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPFormLetterBody(DataSet AData, String ABodyName, TDBTransaction ATransaction)
        {
            LoadViaPFormLetterBody(AData, ABodyName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPFormLetterBody(DataSet AData, String ABodyName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPFormLetterBody(AData, ABodyName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPFormLetterBody(String ABodyName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterDesignTable Data = new PFormLetterDesignTable();
            LoadViaForeignKey(PFormLetterDesignTable.TableId, PFormLetterBodyTable.TableId, Data, new string[1]{"p_body_name_c"},
                new System.Object[1]{ABodyName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPFormLetterBody(String ABodyName, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBody(ABodyName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPFormLetterBody(String ABodyName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBody(ABodyName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPFormLetterBodyTemplate(DataSet ADataSet, PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterDesignTable.TableId, PFormLetterBodyTable.TableId, ADataSet, new string[1]{"p_body_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPFormLetterBodyTemplate(DataSet AData, PFormLetterBodyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPFormLetterBodyTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPFormLetterBodyTemplate(DataSet AData, PFormLetterBodyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPFormLetterBodyTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPFormLetterBodyTemplate(PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterDesignTable Data = new PFormLetterDesignTable();
            LoadViaForeignKey(PFormLetterDesignTable.TableId, PFormLetterBodyTable.TableId, Data, new string[1]{"p_body_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPFormLetterBodyTemplate(PFormLetterBodyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBodyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPFormLetterBodyTemplate(PFormLetterBodyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBodyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPFormLetterBodyTemplate(PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBodyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPFormLetterBodyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterDesignTable.TableId, PFormLetterBodyTable.TableId, ADataSet, new string[1]{"p_body_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPFormLetterBodyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPFormLetterBodyTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPFormLetterBodyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPFormLetterBodyTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPFormLetterBodyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterDesignTable Data = new PFormLetterDesignTable();
            LoadViaForeignKey(PFormLetterDesignTable.TableId, PFormLetterBodyTable.TableId, Data, new string[1]{"p_body_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPFormLetterBodyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBodyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPFormLetterBodyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBodyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPFormLetterBody(String ABodyName, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterDesignTable.TableId, PFormLetterBodyTable.TableId, new string[1]{"p_body_name_c"},
                new System.Object[1]{ABodyName}, ATransaction);
        }

        /// auto generated
        public static int CountViaPFormLetterBodyTemplate(PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterDesignTable.TableId, PFormLetterBodyTable.TableId, new string[1]{"p_body_name_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPFormLetterBodyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterDesignTable.TableId, PFormLetterBodyTable.TableId, new string[1]{"p_body_name_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCode(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterDesignTable.TableId, PAddressLayoutCodeTable.TableId, ADataSet, new string[1]{"p_address_layout_code_c"},
                new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PFormLetterDesignTable LoadViaPAddressLayoutCode(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterDesignTable Data = new PFormLetterDesignTable();
            LoadViaForeignKey(PFormLetterDesignTable.TableId, PAddressLayoutCodeTable.TableId, Data, new string[1]{"p_address_layout_code_c"},
                new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPAddressLayoutCode(String ACode, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCode(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPAddressLayoutCode(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCode(ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(DataSet ADataSet, PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterDesignTable.TableId, PAddressLayoutCodeTable.TableId, ADataSet, new string[1]{"p_address_layout_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PFormLetterDesignTable LoadViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterDesignTable Data = new PFormLetterDesignTable();
            LoadViaForeignKey(PFormLetterDesignTable.TableId, PAddressLayoutCodeTable.TableId, Data, new string[1]{"p_address_layout_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPAddressLayoutCodeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterDesignTable.TableId, PAddressLayoutCodeTable.TableId, ADataSet, new string[1]{"p_address_layout_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PFormLetterDesignTable LoadViaPAddressLayoutCodeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterDesignTable Data = new PFormLetterDesignTable();
            LoadViaForeignKey(PFormLetterDesignTable.TableId, PAddressLayoutCodeTable.TableId, Data, new string[1]{"p_address_layout_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPAddressLayoutCodeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterDesignTable LoadViaPAddressLayoutCodeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPAddressLayoutCodeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPAddressLayoutCode(String ACode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterDesignTable.TableId, PAddressLayoutCodeTable.TableId, new string[1]{"p_address_layout_code_c"},
                new System.Object[1]{ACode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPAddressLayoutCodeTemplate(PAddressLayoutCodeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterDesignTable.TableId, PAddressLayoutCodeTable.TableId, new string[1]{"p_address_layout_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPAddressLayoutCodeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterDesignTable.TableId, PAddressLayoutCodeTable.TableId, new string[1]{"p_address_layout_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ADesignName, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PFormLetterDesignTable.TableId, new System.Object[1]{ADesignName}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PFormLetterDesignRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PFormLetterDesignTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PFormLetterDesignTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PFormLetterDesignTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Insertions for a body of text for a given extract and partner
    public class PFormLetterInsertAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PFormLetterInsert";

        /// original table name in database
        public const string DBTABLENAME = "p_form_letter_insert";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PFormLetterInsertTable.TableId) + " FROM PUB_p_form_letter_insert") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PFormLetterInsertTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PFormLetterInsertTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterInsertTable Data = new PFormLetterInsertTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PFormLetterInsertTable.TableId) + " FROM PUB_p_form_letter_insert" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterInsertTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ASequence, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PFormLetterInsertTable.TableId, ADataSet, new System.Object[1]{ASequence}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ASequence, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ASequence, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ASequence, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ASequence, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadByPrimaryKey(Int32 ASequence, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterInsertTable Data = new PFormLetterInsertTable();
            LoadByPrimaryKey(PFormLetterInsertTable.TableId, Data, new System.Object[1]{ASequence}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterInsertTable LoadByPrimaryKey(Int32 ASequence, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ASequence, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadByPrimaryKey(Int32 ASequence, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ASequence, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PFormLetterInsertRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PFormLetterInsertTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PFormLetterInsertRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PFormLetterInsertRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadUsingTemplate(PFormLetterInsertRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterInsertTable Data = new PFormLetterInsertTable();
            LoadUsingTemplate(PFormLetterInsertTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterInsertTable LoadUsingTemplate(PFormLetterInsertRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadUsingTemplate(PFormLetterInsertRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadUsingTemplate(PFormLetterInsertRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PFormLetterInsertTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PFormLetterInsertTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterInsertTable Data = new PFormLetterInsertTable();
            LoadUsingTemplate(PFormLetterInsertTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterInsertTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_form_letter_insert", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ASequence, TDBTransaction ATransaction)
        {
            return Exists(PFormLetterInsertTable.TableId, new System.Object[1]{ASequence}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PFormLetterInsertRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_form_letter_insert" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PFormLetterInsertTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PFormLetterInsertTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_form_letter_insert" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PFormLetterInsertTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PFormLetterInsertTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPFormLetterBody(DataSet ADataSet, String ABodyName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterInsertTable.TableId, PFormLetterBodyTable.TableId, ADataSet, new string[1]{"p_body_name_c"},
                new System.Object[1]{ABodyName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPFormLetterBody(DataSet AData, String ABodyName, TDBTransaction ATransaction)
        {
            LoadViaPFormLetterBody(AData, ABodyName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPFormLetterBody(DataSet AData, String ABodyName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPFormLetterBody(AData, ABodyName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPFormLetterBody(String ABodyName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterInsertTable Data = new PFormLetterInsertTable();
            LoadViaForeignKey(PFormLetterInsertTable.TableId, PFormLetterBodyTable.TableId, Data, new string[1]{"p_body_name_c"},
                new System.Object[1]{ABodyName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPFormLetterBody(String ABodyName, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBody(ABodyName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPFormLetterBody(String ABodyName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBody(ABodyName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPFormLetterBodyTemplate(DataSet ADataSet, PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterInsertTable.TableId, PFormLetterBodyTable.TableId, ADataSet, new string[1]{"p_body_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPFormLetterBodyTemplate(DataSet AData, PFormLetterBodyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPFormLetterBodyTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPFormLetterBodyTemplate(DataSet AData, PFormLetterBodyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPFormLetterBodyTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPFormLetterBodyTemplate(PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterInsertTable Data = new PFormLetterInsertTable();
            LoadViaForeignKey(PFormLetterInsertTable.TableId, PFormLetterBodyTable.TableId, Data, new string[1]{"p_body_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPFormLetterBodyTemplate(PFormLetterBodyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBodyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPFormLetterBodyTemplate(PFormLetterBodyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBodyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPFormLetterBodyTemplate(PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBodyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPFormLetterBodyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterInsertTable.TableId, PFormLetterBodyTable.TableId, ADataSet, new string[1]{"p_body_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPFormLetterBodyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPFormLetterBodyTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPFormLetterBodyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPFormLetterBodyTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPFormLetterBodyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterInsertTable Data = new PFormLetterInsertTable();
            LoadViaForeignKey(PFormLetterInsertTable.TableId, PFormLetterBodyTable.TableId, Data, new string[1]{"p_body_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPFormLetterBodyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBodyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPFormLetterBodyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPFormLetterBodyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPFormLetterBody(String ABodyName, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterInsertTable.TableId, PFormLetterBodyTable.TableId, new string[1]{"p_body_name_c"},
                new System.Object[1]{ABodyName}, ATransaction);
        }

        /// auto generated
        public static int CountViaPFormLetterBodyTemplate(PFormLetterBodyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterInsertTable.TableId, PFormLetterBodyTable.TableId, new string[1]{"p_body_name_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPFormLetterBodyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterInsertTable.TableId, PFormLetterBodyTable.TableId, new string[1]{"p_body_name_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterInsertTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterInsertTable Data = new PFormLetterInsertTable();
            LoadViaForeignKey(PFormLetterInsertTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterInsertTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterInsertTable Data = new PFormLetterInsertTable();
            LoadViaForeignKey(PFormLetterInsertTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterInsertTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterInsertTable Data = new PFormLetterInsertTable();
            LoadViaForeignKey(PFormLetterInsertTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterInsertTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterInsertTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterInsertTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaMExtractMaster(DataSet ADataSet, Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterInsertTable.TableId, MExtractMasterTable.TableId, ADataSet, new string[1]{"m_extract_id_i"},
                new System.Object[1]{AExtractId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaMExtractMaster(DataSet AData, Int32 AExtractId, TDBTransaction ATransaction)
        {
            LoadViaMExtractMaster(AData, AExtractId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMaster(DataSet AData, Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaMExtractMaster(AData, AExtractId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaMExtractMaster(Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterInsertTable Data = new PFormLetterInsertTable();
            LoadViaForeignKey(PFormLetterInsertTable.TableId, MExtractMasterTable.TableId, Data, new string[1]{"m_extract_id_i"},
                new System.Object[1]{AExtractId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaMExtractMaster(Int32 AExtractId, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMaster(AExtractId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaMExtractMaster(Int32 AExtractId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMaster(AExtractId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet ADataSet, MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterInsertTable.TableId, MExtractMasterTable.TableId, ADataSet, new string[1]{"m_extract_id_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet AData, MExtractMasterRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaMExtractMasterTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet AData, MExtractMasterRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaMExtractMasterTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterInsertTable Data = new PFormLetterInsertTable();
            LoadViaForeignKey(PFormLetterInsertTable.TableId, MExtractMasterTable.TableId, Data, new string[1]{"m_extract_id_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PFormLetterInsertTable.TableId, MExtractMasterTable.TableId, ADataSet, new string[1]{"m_extract_id_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaMExtractMasterTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaMExtractMasterTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaMExtractMasterTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaMExtractMasterTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PFormLetterInsertTable Data = new PFormLetterInsertTable();
            LoadViaForeignKey(PFormLetterInsertTable.TableId, MExtractMasterTable.TableId, Data, new string[1]{"m_extract_id_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaMExtractMasterTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PFormLetterInsertTable LoadViaMExtractMasterTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaMExtractMasterTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaMExtractMaster(Int32 AExtractId, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterInsertTable.TableId, MExtractMasterTable.TableId, new string[1]{"m_extract_id_i"},
                new System.Object[1]{AExtractId}, ATransaction);
        }

        /// auto generated
        public static int CountViaMExtractMasterTemplate(MExtractMasterRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterInsertTable.TableId, MExtractMasterTable.TableId, new string[1]{"m_extract_id_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaMExtractMasterTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PFormLetterInsertTable.TableId, MExtractMasterTable.TableId, new string[1]{"m_extract_id_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ASequence, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PFormLetterInsertTable.TableId, new System.Object[1]{ASequence}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PFormLetterInsertRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PFormLetterInsertTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PFormLetterInsertTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PFormLetterInsertTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Defines the attributes of different label types.  Eg: for address labels.
    public class PLabelAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PLabel";

        /// original table name in database
        public const string DBTABLENAME = "p_label";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PLabelTable.TableId) + " FROM PUB_p_label") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PLabelTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PLabelTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PLabelTable Data = new PLabelTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PLabelTable.TableId) + " FROM PUB_p_label" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PLabelTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PLabelTable.TableId, ADataSet, new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadByPrimaryKey(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PLabelTable Data = new PLabelTable();
            LoadByPrimaryKey(PLabelTable.TableId, Data, new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PLabelTable LoadByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadByPrimaryKey(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PLabelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PLabelTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PLabelRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PLabelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadUsingTemplate(PLabelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PLabelTable Data = new PLabelTable();
            LoadUsingTemplate(PLabelTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PLabelTable LoadUsingTemplate(PLabelRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadUsingTemplate(PLabelRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadUsingTemplate(PLabelRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PLabelTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PLabelTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PLabelTable Data = new PLabelTable();
            LoadUsingTemplate(PLabelTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PLabelTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_label", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ACode, TDBTransaction ATransaction)
        {
            return Exists(PLabelTable.TableId, new System.Object[1]{ACode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PLabelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_label" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PLabelTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PLabelTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_label" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PLabelTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PLabelTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaSForm(DataSet ADataSet, String AFormName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PLabelTable.TableId, SFormTable.TableId, ADataSet, new string[1]{"s_form_name_c"},
                new System.Object[1]{AFormName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSForm(DataSet AData, String AFormName, TDBTransaction ATransaction)
        {
            LoadViaSForm(AData, AFormName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSForm(DataSet AData, String AFormName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSForm(AData, AFormName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadViaSForm(String AFormName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PLabelTable Data = new PLabelTable();
            LoadViaForeignKey(PLabelTable.TableId, SFormTable.TableId, Data, new string[1]{"s_form_name_c"},
                new System.Object[1]{AFormName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PLabelTable LoadViaSForm(String AFormName, TDBTransaction ATransaction)
        {
            return LoadViaSForm(AFormName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadViaSForm(String AFormName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSForm(AFormName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSFormTemplate(DataSet ADataSet, SFormRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PLabelTable.TableId, SFormTable.TableId, ADataSet, new string[1]{"s_form_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSFormTemplate(DataSet AData, SFormRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSFormTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSFormTemplate(DataSet AData, SFormRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSFormTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadViaSFormTemplate(SFormRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PLabelTable Data = new PLabelTable();
            LoadViaForeignKey(PLabelTable.TableId, SFormTable.TableId, Data, new string[1]{"s_form_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PLabelTable LoadViaSFormTemplate(SFormRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaSFormTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadViaSFormTemplate(SFormRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSFormTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadViaSFormTemplate(SFormRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSFormTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSFormTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PLabelTable.TableId, SFormTable.TableId, ADataSet, new string[1]{"s_form_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSFormTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaSFormTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSFormTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSFormTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadViaSFormTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PLabelTable Data = new PLabelTable();
            LoadViaForeignKey(PLabelTable.TableId, SFormTable.TableId, Data, new string[1]{"s_form_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PLabelTable LoadViaSFormTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaSFormTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PLabelTable LoadViaSFormTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSFormTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaSForm(String AFormName, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PLabelTable.TableId, SFormTable.TableId, new string[1]{"s_form_name_c"},
                new System.Object[1]{AFormName}, ATransaction);
        }

        /// auto generated
        public static int CountViaSFormTemplate(SFormRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PLabelTable.TableId, SFormTable.TableId, new string[1]{"s_form_name_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaSFormTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PLabelTable.TableId, SFormTable.TableId, new string[1]{"s_form_name_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PLabelTable.TableId, new System.Object[1]{ACode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PLabelRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PLabelTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PLabelTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PLabelTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Master record for Mail Merge output creation
    public class PMergeFormAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PMergeForm";

        /// original table name in database
        public const string DBTABLENAME = "p_merge_form";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PMergeFormTable.TableId) + " FROM PUB_p_merge_form") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PMergeFormTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PMergeFormTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMergeFormTable Data = new PMergeFormTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PMergeFormTable.TableId) + " FROM PUB_p_merge_form" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMergeFormTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFormTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AMergeFormName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PMergeFormTable.TableId, ADataSet, new System.Object[1]{AMergeFormName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AMergeFormName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AMergeFormName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AMergeFormName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AMergeFormName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFormTable LoadByPrimaryKey(String AMergeFormName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMergeFormTable Data = new PMergeFormTable();
            LoadByPrimaryKey(PMergeFormTable.TableId, Data, new System.Object[1]{AMergeFormName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMergeFormTable LoadByPrimaryKey(String AMergeFormName, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AMergeFormName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFormTable LoadByPrimaryKey(String AMergeFormName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AMergeFormName, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PMergeFormRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PMergeFormTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PMergeFormRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PMergeFormRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFormTable LoadUsingTemplate(PMergeFormRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMergeFormTable Data = new PMergeFormTable();
            LoadUsingTemplate(PMergeFormTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMergeFormTable LoadUsingTemplate(PMergeFormRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFormTable LoadUsingTemplate(PMergeFormRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFormTable LoadUsingTemplate(PMergeFormRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PMergeFormTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PMergeFormTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMergeFormTable Data = new PMergeFormTable();
            LoadUsingTemplate(PMergeFormTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMergeFormTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFormTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_merge_form", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AMergeFormName, TDBTransaction ATransaction)
        {
            return Exists(PMergeFormTable.TableId, new System.Object[1]{AMergeFormName}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PMergeFormRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_merge_form" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PMergeFormTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PMergeFormTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_merge_form" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PMergeFormTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PMergeFormTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AMergeFormName, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PMergeFormTable.TableId, new System.Object[1]{AMergeFormName}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PMergeFormRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PMergeFormTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PMergeFormTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PMergeFormTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Fields within a Mail Merge Form
    public class PMergeFieldAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PMergeField";

        /// original table name in database
        public const string DBTABLENAME = "p_merge_field";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PMergeFieldTable.TableId) + " FROM PUB_p_merge_field") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PMergeFieldTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PMergeFieldTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMergeFieldTable Data = new PMergeFieldTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PMergeFieldTable.TableId) + " FROM PUB_p_merge_field" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMergeFieldTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AMergeFormName, String AMergeFieldName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PMergeFieldTable.TableId, ADataSet, new System.Object[2]{AMergeFormName, AMergeFieldName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AMergeFormName, String AMergeFieldName, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AMergeFormName, AMergeFieldName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AMergeFormName, String AMergeFieldName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AMergeFormName, AMergeFieldName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadByPrimaryKey(String AMergeFormName, String AMergeFieldName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMergeFieldTable Data = new PMergeFieldTable();
            LoadByPrimaryKey(PMergeFieldTable.TableId, Data, new System.Object[2]{AMergeFormName, AMergeFieldName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMergeFieldTable LoadByPrimaryKey(String AMergeFormName, String AMergeFieldName, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AMergeFormName, AMergeFieldName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadByPrimaryKey(String AMergeFormName, String AMergeFieldName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AMergeFormName, AMergeFieldName, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PMergeFieldRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PMergeFieldTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PMergeFieldRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PMergeFieldRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadUsingTemplate(PMergeFieldRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMergeFieldTable Data = new PMergeFieldTable();
            LoadUsingTemplate(PMergeFieldTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMergeFieldTable LoadUsingTemplate(PMergeFieldRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadUsingTemplate(PMergeFieldRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadUsingTemplate(PMergeFieldRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PMergeFieldTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PMergeFieldTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMergeFieldTable Data = new PMergeFieldTable();
            LoadUsingTemplate(PMergeFieldTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMergeFieldTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_merge_field", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AMergeFormName, String AMergeFieldName, TDBTransaction ATransaction)
        {
            return Exists(PMergeFieldTable.TableId, new System.Object[2]{AMergeFormName, AMergeFieldName}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PMergeFieldRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_merge_field" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PMergeFieldTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PMergeFieldTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_merge_field" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PMergeFieldTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PMergeFieldTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPMergeForm(DataSet ADataSet, String AMergeFormName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PMergeFieldTable.TableId, PMergeFormTable.TableId, ADataSet, new string[1]{"p_merge_form_name_c"},
                new System.Object[1]{AMergeFormName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPMergeForm(DataSet AData, String AMergeFormName, TDBTransaction ATransaction)
        {
            LoadViaPMergeForm(AData, AMergeFormName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMergeForm(DataSet AData, String AMergeFormName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMergeForm(AData, AMergeFormName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadViaPMergeForm(String AMergeFormName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMergeFieldTable Data = new PMergeFieldTable();
            LoadViaForeignKey(PMergeFieldTable.TableId, PMergeFormTable.TableId, Data, new string[1]{"p_merge_form_name_c"},
                new System.Object[1]{AMergeFormName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMergeFieldTable LoadViaPMergeForm(String AMergeFormName, TDBTransaction ATransaction)
        {
            return LoadViaPMergeForm(AMergeFormName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadViaPMergeForm(String AMergeFormName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMergeForm(AMergeFormName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMergeFormTemplate(DataSet ADataSet, PMergeFormRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PMergeFieldTable.TableId, PMergeFormTable.TableId, ADataSet, new string[1]{"p_merge_form_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPMergeFormTemplate(DataSet AData, PMergeFormRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPMergeFormTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMergeFormTemplate(DataSet AData, PMergeFormRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMergeFormTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadViaPMergeFormTemplate(PMergeFormRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMergeFieldTable Data = new PMergeFieldTable();
            LoadViaForeignKey(PMergeFieldTable.TableId, PMergeFormTable.TableId, Data, new string[1]{"p_merge_form_name_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMergeFieldTable LoadViaPMergeFormTemplate(PMergeFormRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPMergeFormTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadViaPMergeFormTemplate(PMergeFormRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMergeFormTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadViaPMergeFormTemplate(PMergeFormRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMergeFormTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMergeFormTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PMergeFieldTable.TableId, PMergeFormTable.TableId, ADataSet, new string[1]{"p_merge_form_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPMergeFormTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPMergeFormTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMergeFormTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMergeFormTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadViaPMergeFormTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMergeFieldTable Data = new PMergeFieldTable();
            LoadViaForeignKey(PMergeFieldTable.TableId, PMergeFormTable.TableId, Data, new string[1]{"p_merge_form_name_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMergeFieldTable LoadViaPMergeFormTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPMergeFormTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMergeFieldTable LoadViaPMergeFormTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMergeFormTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPMergeForm(String AMergeFormName, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PMergeFieldTable.TableId, PMergeFormTable.TableId, new string[1]{"p_merge_form_name_c"},
                new System.Object[1]{AMergeFormName}, ATransaction);
        }

        /// auto generated
        public static int CountViaPMergeFormTemplate(PMergeFormRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PMergeFieldTable.TableId, PMergeFormTable.TableId, new string[1]{"p_merge_form_name_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPMergeFormTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PMergeFieldTable.TableId, PMergeFormTable.TableId, new string[1]{"p_merge_form_name_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AMergeFormName, String AMergeFieldName, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PMergeFieldTable.TableId, new System.Object[2]{AMergeFormName, AMergeFieldName}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PMergeFieldRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PMergeFieldTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PMergeFieldTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PMergeFieldTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Postcode ranges for each region
    public class PPostcodeRangeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PPostcodeRange";

        /// original table name in database
        public const string DBTABLENAME = "p_postcode_range";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PPostcodeRangeTable.TableId) + " FROM PUB_p_postcode_range") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPostcodeRangeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PPostcodeRangeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPostcodeRangeTable Data = new PPostcodeRangeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PPostcodeRangeTable.TableId) + " FROM PUB_p_postcode_range" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPostcodeRangeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRangeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ARange, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PPostcodeRangeTable.TableId, ADataSet, new System.Object[1]{ARange}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ARange, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ARange, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ARange, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ARange, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRangeTable LoadByPrimaryKey(String ARange, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPostcodeRangeTable Data = new PPostcodeRangeTable();
            LoadByPrimaryKey(PPostcodeRangeTable.TableId, Data, new System.Object[1]{ARange}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPostcodeRangeTable LoadByPrimaryKey(String ARange, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ARange, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRangeTable LoadByPrimaryKey(String ARange, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ARange, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PPostcodeRangeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PPostcodeRangeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PPostcodeRangeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PPostcodeRangeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRangeTable LoadUsingTemplate(PPostcodeRangeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPostcodeRangeTable Data = new PPostcodeRangeTable();
            LoadUsingTemplate(PPostcodeRangeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPostcodeRangeTable LoadUsingTemplate(PPostcodeRangeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRangeTable LoadUsingTemplate(PPostcodeRangeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRangeTable LoadUsingTemplate(PPostcodeRangeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PPostcodeRangeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PPostcodeRangeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPostcodeRangeTable Data = new PPostcodeRangeTable();
            LoadUsingTemplate(PPostcodeRangeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPostcodeRangeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRangeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_postcode_range", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ARange, TDBTransaction ATransaction)
        {
            return Exists(PPostcodeRangeTable.TableId, new System.Object[1]{ARange}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PPostcodeRangeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_postcode_range" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PPostcodeRangeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PPostcodeRangeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_postcode_range" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PPostcodeRangeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PPostcodeRangeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ARange, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PPostcodeRangeTable.TableId, new System.Object[1]{ARange}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PPostcodeRangeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PPostcodeRangeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PPostcodeRangeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PPostcodeRangeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Postcode regions and the ranges they contain.
    public class PPostcodeRegionAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PPostcodeRegion";

        /// original table name in database
        public const string DBTABLENAME = "p_postcode_region";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PPostcodeRegionTable.TableId) + " FROM PUB_p_postcode_region") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPostcodeRegionTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PPostcodeRegionTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPostcodeRegionTable Data = new PPostcodeRegionTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PPostcodeRegionTable.TableId) + " FROM PUB_p_postcode_region" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPostcodeRegionTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ARegion, String ARange, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PPostcodeRegionTable.TableId, ADataSet, new System.Object[2]{ARegion, ARange}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ARegion, String ARange, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ARegion, ARange, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ARegion, String ARange, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ARegion, ARange, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadByPrimaryKey(String ARegion, String ARange, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPostcodeRegionTable Data = new PPostcodeRegionTable();
            LoadByPrimaryKey(PPostcodeRegionTable.TableId, Data, new System.Object[2]{ARegion, ARange}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPostcodeRegionTable LoadByPrimaryKey(String ARegion, String ARange, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ARegion, ARange, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadByPrimaryKey(String ARegion, String ARange, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ARegion, ARange, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PPostcodeRegionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PPostcodeRegionTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PPostcodeRegionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PPostcodeRegionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadUsingTemplate(PPostcodeRegionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPostcodeRegionTable Data = new PPostcodeRegionTable();
            LoadUsingTemplate(PPostcodeRegionTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPostcodeRegionTable LoadUsingTemplate(PPostcodeRegionRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadUsingTemplate(PPostcodeRegionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadUsingTemplate(PPostcodeRegionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PPostcodeRegionTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PPostcodeRegionTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPostcodeRegionTable Data = new PPostcodeRegionTable();
            LoadUsingTemplate(PPostcodeRegionTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPostcodeRegionTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_postcode_region", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ARegion, String ARange, TDBTransaction ATransaction)
        {
            return Exists(PPostcodeRegionTable.TableId, new System.Object[2]{ARegion, ARange}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PPostcodeRegionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_postcode_region" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PPostcodeRegionTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PPostcodeRegionTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_postcode_region" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PPostcodeRegionTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PPostcodeRegionTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPPostcodeRange(DataSet ADataSet, String ARange, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPostcodeRegionTable.TableId, PPostcodeRangeTable.TableId, ADataSet, new string[1]{"p_range_c"},
                new System.Object[1]{ARange}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPostcodeRange(DataSet AData, String ARange, TDBTransaction ATransaction)
        {
            LoadViaPPostcodeRange(AData, ARange, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPostcodeRange(DataSet AData, String ARange, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPostcodeRange(AData, ARange, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadViaPPostcodeRange(String ARange, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPostcodeRegionTable Data = new PPostcodeRegionTable();
            LoadViaForeignKey(PPostcodeRegionTable.TableId, PPostcodeRangeTable.TableId, Data, new string[1]{"p_range_c"},
                new System.Object[1]{ARange}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPostcodeRegionTable LoadViaPPostcodeRange(String ARange, TDBTransaction ATransaction)
        {
            return LoadViaPPostcodeRange(ARange, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadViaPPostcodeRange(String ARange, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPostcodeRange(ARange, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPostcodeRangeTemplate(DataSet ADataSet, PPostcodeRangeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPostcodeRegionTable.TableId, PPostcodeRangeTable.TableId, ADataSet, new string[1]{"p_range_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPostcodeRangeTemplate(DataSet AData, PPostcodeRangeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPostcodeRangeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPostcodeRangeTemplate(DataSet AData, PPostcodeRangeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPostcodeRangeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadViaPPostcodeRangeTemplate(PPostcodeRangeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPostcodeRegionTable Data = new PPostcodeRegionTable();
            LoadViaForeignKey(PPostcodeRegionTable.TableId, PPostcodeRangeTable.TableId, Data, new string[1]{"p_range_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPostcodeRegionTable LoadViaPPostcodeRangeTemplate(PPostcodeRangeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPostcodeRangeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadViaPPostcodeRangeTemplate(PPostcodeRangeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPostcodeRangeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadViaPPostcodeRangeTemplate(PPostcodeRangeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPostcodeRangeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPostcodeRangeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPostcodeRegionTable.TableId, PPostcodeRangeTable.TableId, ADataSet, new string[1]{"p_range_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPostcodeRangeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPostcodeRangeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPostcodeRangeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPostcodeRangeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadViaPPostcodeRangeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPostcodeRegionTable Data = new PPostcodeRegionTable();
            LoadViaForeignKey(PPostcodeRegionTable.TableId, PPostcodeRangeTable.TableId, Data, new string[1]{"p_range_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPostcodeRegionTable LoadViaPPostcodeRangeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPostcodeRangeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPostcodeRegionTable LoadViaPPostcodeRangeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPostcodeRangeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPostcodeRange(String ARange, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPostcodeRegionTable.TableId, PPostcodeRangeTable.TableId, new string[1]{"p_range_c"},
                new System.Object[1]{ARange}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPostcodeRangeTemplate(PPostcodeRangeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPostcodeRegionTable.TableId, PPostcodeRangeTable.TableId, new string[1]{"p_range_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPostcodeRangeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPostcodeRegionTable.TableId, PPostcodeRangeTable.TableId, new string[1]{"p_range_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ARegion, String ARange, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PPostcodeRegionTable.TableId, new System.Object[2]{ARegion, ARange}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PPostcodeRegionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PPostcodeRegionTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PPostcodeRegionTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PPostcodeRegionTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Details of a publication
    public class PPublicationAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PPublication";

        /// original table name in database
        public const string DBTABLENAME = "p_publication";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PPublicationTable.TableId) + " FROM PUB_p_publication") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPublicationTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PPublicationTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationTable Data = new PPublicationTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PPublicationTable.TableId) + " FROM PUB_p_publication" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String APublicationCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PPublicationTable.TableId, ADataSet, new System.Object[1]{APublicationCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String APublicationCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APublicationCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String APublicationCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APublicationCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadByPrimaryKey(String APublicationCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationTable Data = new PPublicationTable();
            LoadByPrimaryKey(PPublicationTable.TableId, Data, new System.Object[1]{APublicationCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationTable LoadByPrimaryKey(String APublicationCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(APublicationCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadByPrimaryKey(String APublicationCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(APublicationCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PPublicationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PPublicationTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PPublicationRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PPublicationRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadUsingTemplate(PPublicationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationTable Data = new PPublicationTable();
            LoadUsingTemplate(PPublicationTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationTable LoadUsingTemplate(PPublicationRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadUsingTemplate(PPublicationRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadUsingTemplate(PPublicationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PPublicationTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PPublicationTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationTable Data = new PPublicationTable();
            LoadUsingTemplate(PPublicationTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_publication", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String APublicationCode, TDBTransaction ATransaction)
        {
            return Exists(PPublicationTable.TableId, new System.Object[1]{APublicationCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PPublicationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_publication" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PPublicationTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PPublicationTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_publication" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PPublicationTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PPublicationTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAFrequency(DataSet ADataSet, String AFrequencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPublicationTable.TableId, AFrequencyTable.TableId, ADataSet, new string[1]{"a_frequency_code_c"},
                new System.Object[1]{AFrequencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAFrequency(DataSet AData, String AFrequencyCode, TDBTransaction ATransaction)
        {
            LoadViaAFrequency(AData, AFrequencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFrequency(DataSet AData, String AFrequencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFrequency(AData, AFrequencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaAFrequency(String AFrequencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationTable Data = new PPublicationTable();
            LoadViaForeignKey(PPublicationTable.TableId, AFrequencyTable.TableId, Data, new string[1]{"a_frequency_code_c"},
                new System.Object[1]{AFrequencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationTable LoadViaAFrequency(String AFrequencyCode, TDBTransaction ATransaction)
        {
            return LoadViaAFrequency(AFrequencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaAFrequency(String AFrequencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAFrequency(AFrequencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFrequencyTemplate(DataSet ADataSet, AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPublicationTable.TableId, AFrequencyTable.TableId, ADataSet, new string[1]{"a_frequency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAFrequencyTemplate(DataSet AData, AFrequencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAFrequencyTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFrequencyTemplate(DataSet AData, AFrequencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFrequencyTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaAFrequencyTemplate(AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationTable Data = new PPublicationTable();
            LoadViaForeignKey(PPublicationTable.TableId, AFrequencyTable.TableId, Data, new string[1]{"a_frequency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationTable LoadViaAFrequencyTemplate(AFrequencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAFrequencyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaAFrequencyTemplate(AFrequencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAFrequencyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaAFrequencyTemplate(AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAFrequencyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFrequencyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPublicationTable.TableId, AFrequencyTable.TableId, ADataSet, new string[1]{"a_frequency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAFrequencyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAFrequencyTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFrequencyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFrequencyTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaAFrequencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationTable Data = new PPublicationTable();
            LoadViaForeignKey(PPublicationTable.TableId, AFrequencyTable.TableId, Data, new string[1]{"a_frequency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationTable LoadViaAFrequencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAFrequencyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaAFrequencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAFrequencyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAFrequency(String AFrequencyCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPublicationTable.TableId, AFrequencyTable.TableId, new string[1]{"a_frequency_code_c"},
                new System.Object[1]{AFrequencyCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAFrequencyTemplate(AFrequencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPublicationTable.TableId, AFrequencyTable.TableId, new string[1]{"a_frequency_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAFrequencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPublicationTable.TableId, AFrequencyTable.TableId, new string[1]{"a_frequency_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPLanguage(DataSet ADataSet, String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPublicationTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_publication_language_c"},
                new System.Object[1]{ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PPublicationTable LoadViaPLanguage(String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationTable Data = new PPublicationTable();
            LoadViaForeignKey(PPublicationTable.TableId, PLanguageTable.TableId, Data, new string[1]{"p_publication_language_c"},
                new System.Object[1]{ALanguageCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationTable LoadViaPLanguage(String ALanguageCode, TDBTransaction ATransaction)
        {
            return LoadViaPLanguage(ALanguageCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaPLanguage(String ALanguageCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguage(ALanguageCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPublicationTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_publication_language_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PPublicationTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationTable Data = new PPublicationTable();
            LoadViaForeignKey(PPublicationTable.TableId, PLanguageTable.TableId, Data, new string[1]{"p_publication_language_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPLanguageTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPublicationTable.TableId, PLanguageTable.TableId, ADataSet, new string[1]{"p_publication_language_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PPublicationTable LoadViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationTable Data = new PPublicationTable();
            LoadViaForeignKey(PPublicationTable.TableId, PLanguageTable.TableId, Data, new string[1]{"p_publication_language_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationTable LoadViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPLanguageTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPLanguage(String ALanguageCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPublicationTable.TableId, PLanguageTable.TableId, new string[1]{"p_publication_language_c"},
                new System.Object[1]{ALanguageCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(PLanguageRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPublicationTable.TableId, PLanguageTable.TableId, new string[1]{"p_publication_language_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPLanguageTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPublicationTable.TableId, PLanguageTable.TableId, new string[1]{"p_publication_language_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_publication", AFieldList, PPublicationTable.TableId) +
                            " FROM PUB_p_publication, PUB_p_subscription WHERE " +
                            "PUB_p_subscription.p_publication_code_c = PUB_p_publication.p_publication_code_c AND PUB_p_subscription.p_partner_key_n = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPublicationTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PPublicationTable Data = new PPublicationTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPartner(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PPublicationTable LoadViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_publication", AFieldList, PPublicationTable.TableId) +
                            " FROM PUB_p_publication, PUB_p_subscription, PUB_p_partner WHERE " +
                            "PUB_p_subscription.p_publication_code_c = PUB_p_publication.p_publication_code_c AND PUB_p_subscription.p_partner_key_n = PUB_p_partner.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPublicationTable.TableId), ATransaction,
                            GetParametersForWhereClause(PPartnerTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PPublicationTable Data = new PPublicationTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPartnerTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PPublicationTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_publication", AFieldList, PPublicationTable.TableId) +
                            " FROM PUB_p_publication, PUB_p_subscription, PUB_p_partner WHERE " +
                            "PUB_p_subscription.p_publication_code_c = PUB_p_publication.p_publication_code_c AND PUB_p_subscription.p_partner_key_n = PUB_p_partner.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_partner", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPublicationTable.TableId), ATransaction,
                            GetParametersForWhereClause(PPublicationTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PPublicationTable Data = new PPublicationTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPartnerTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PPublicationTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_publication, PUB_p_subscription WHERE " +
                        "PUB_p_subscription.p_publication_code_c = PUB_p_publication.p_publication_code_c AND PUB_p_subscription.p_partner_key_n = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_publication, PUB_p_subscription, PUB_p_partner WHERE " +
                        "PUB_p_subscription.p_publication_code_c = PUB_p_publication.p_publication_code_c AND PUB_p_subscription.p_partner_key_n = PUB_p_partner.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_p_subscription", PPublicationTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PPartnerTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_publication, PUB_p_subscription, PUB_p_partner WHERE " +
                        "PUB_p_subscription.p_publication_code_c = PUB_p_publication.p_publication_code_c AND PUB_p_subscription.p_partner_key_n = PUB_p_partner.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_p_subscription", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PPublicationTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String APublicationCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PPublicationTable.TableId, new System.Object[1]{APublicationCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PPublicationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PPublicationTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PPublicationTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PPublicationTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Cost of a publication
    public class PPublicationCostAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PPublicationCost";

        /// original table name in database
        public const string DBTABLENAME = "p_publication_cost";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PPublicationCostTable.TableId) + " FROM PUB_p_publication_cost") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPublicationCostTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PPublicationCostTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationCostTable Data = new PPublicationCostTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PPublicationCostTable.TableId) + " FROM PUB_p_publication_cost" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationCostTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String APublicationCode, System.DateTime ADateEffective, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PPublicationCostTable.TableId, ADataSet, new System.Object[2]{APublicationCode, ADateEffective}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String APublicationCode, System.DateTime ADateEffective, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APublicationCode, ADateEffective, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String APublicationCode, System.DateTime ADateEffective, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APublicationCode, ADateEffective, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadByPrimaryKey(String APublicationCode, System.DateTime ADateEffective, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationCostTable Data = new PPublicationCostTable();
            LoadByPrimaryKey(PPublicationCostTable.TableId, Data, new System.Object[2]{APublicationCode, ADateEffective}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationCostTable LoadByPrimaryKey(String APublicationCode, System.DateTime ADateEffective, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(APublicationCode, ADateEffective, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadByPrimaryKey(String APublicationCode, System.DateTime ADateEffective, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(APublicationCode, ADateEffective, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PPublicationCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PPublicationCostTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PPublicationCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PPublicationCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadUsingTemplate(PPublicationCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationCostTable Data = new PPublicationCostTable();
            LoadUsingTemplate(PPublicationCostTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationCostTable LoadUsingTemplate(PPublicationCostRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadUsingTemplate(PPublicationCostRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadUsingTemplate(PPublicationCostRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PPublicationCostTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PPublicationCostTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationCostTable Data = new PPublicationCostTable();
            LoadUsingTemplate(PPublicationCostTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationCostTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_publication_cost", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String APublicationCode, System.DateTime ADateEffective, TDBTransaction ATransaction)
        {
            return Exists(PPublicationCostTable.TableId, new System.Object[2]{APublicationCode, ADateEffective}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PPublicationCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_publication_cost" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PPublicationCostTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PPublicationCostTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_publication_cost" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PPublicationCostTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PPublicationCostTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPPublication(DataSet ADataSet, String APublicationCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPublicationCostTable.TableId, PPublicationTable.TableId, ADataSet, new string[1]{"p_publication_code_c"},
                new System.Object[1]{APublicationCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPublication(DataSet AData, String APublicationCode, TDBTransaction ATransaction)
        {
            LoadViaPPublication(AData, APublicationCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPublication(DataSet AData, String APublicationCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPublication(AData, APublicationCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadViaPPublication(String APublicationCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationCostTable Data = new PPublicationCostTable();
            LoadViaForeignKey(PPublicationCostTable.TableId, PPublicationTable.TableId, Data, new string[1]{"p_publication_code_c"},
                new System.Object[1]{APublicationCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationCostTable LoadViaPPublication(String APublicationCode, TDBTransaction ATransaction)
        {
            return LoadViaPPublication(APublicationCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadViaPPublication(String APublicationCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPublication(APublicationCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPublicationTemplate(DataSet ADataSet, PPublicationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPublicationCostTable.TableId, PPublicationTable.TableId, ADataSet, new string[1]{"p_publication_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPublicationTemplate(DataSet AData, PPublicationRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPublicationTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPublicationTemplate(DataSet AData, PPublicationRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPublicationTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadViaPPublicationTemplate(PPublicationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationCostTable Data = new PPublicationCostTable();
            LoadViaForeignKey(PPublicationCostTable.TableId, PPublicationTable.TableId, Data, new string[1]{"p_publication_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationCostTable LoadViaPPublicationTemplate(PPublicationRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPublicationTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadViaPPublicationTemplate(PPublicationRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPublicationTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadViaPPublicationTemplate(PPublicationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPublicationTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPublicationTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPublicationCostTable.TableId, PPublicationTable.TableId, ADataSet, new string[1]{"p_publication_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPublicationTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPublicationTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPublicationTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPublicationTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadViaPPublicationTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationCostTable Data = new PPublicationCostTable();
            LoadViaForeignKey(PPublicationCostTable.TableId, PPublicationTable.TableId, Data, new string[1]{"p_publication_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationCostTable LoadViaPPublicationTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPublicationTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadViaPPublicationTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPublicationTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPublication(String APublicationCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPublicationCostTable.TableId, PPublicationTable.TableId, new string[1]{"p_publication_code_c"},
                new System.Object[1]{APublicationCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPublicationTemplate(PPublicationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPublicationCostTable.TableId, PPublicationTable.TableId, new string[1]{"p_publication_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPublicationTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPublicationCostTable.TableId, PPublicationTable.TableId, new string[1]{"p_publication_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPublicationCostTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"p_currency_code_c"},
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
        public static PPublicationCostTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationCostTable Data = new PPublicationCostTable();
            LoadViaForeignKey(PPublicationCostTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"p_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationCostTable LoadViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPublicationCostTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"p_currency_code_c"},
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
        public static PPublicationCostTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationCostTable Data = new PPublicationCostTable();
            LoadViaForeignKey(PPublicationCostTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"p_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationCostTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPublicationCostTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"p_currency_code_c"},
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
        public static PPublicationCostTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPublicationCostTable Data = new PPublicationCostTable();
            LoadViaForeignKey(PPublicationCostTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"p_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPublicationCostTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPublicationCostTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPublicationCostTable.TableId, ACurrencyTable.TableId, new string[1]{"p_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPublicationCostTable.TableId, ACurrencyTable.TableId, new string[1]{"p_currency_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPublicationCostTable.TableId, ACurrencyTable.TableId, new string[1]{"p_currency_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String APublicationCode, System.DateTime ADateEffective, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PPublicationCostTable.TableId, new System.Object[2]{APublicationCode, ADateEffective}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PPublicationCostRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PPublicationCostTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PPublicationCostTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PPublicationCostTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// List of reasons for giving a subscription
    public class PReasonSubscriptionGivenAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PReasonSubscriptionGiven";

        /// original table name in database
        public const string DBTABLENAME = "p_reason_subscription_given";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PReasonSubscriptionGivenTable.TableId) + " FROM PUB_p_reason_subscription_given") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PReasonSubscriptionGivenTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PReasonSubscriptionGivenTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PReasonSubscriptionGivenTable Data = new PReasonSubscriptionGivenTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PReasonSubscriptionGivenTable.TableId) + " FROM PUB_p_reason_subscription_given" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PReasonSubscriptionGivenTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionGivenTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PReasonSubscriptionGivenTable.TableId, ADataSet, new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionGivenTable LoadByPrimaryKey(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PReasonSubscriptionGivenTable Data = new PReasonSubscriptionGivenTable();
            LoadByPrimaryKey(PReasonSubscriptionGivenTable.TableId, Data, new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PReasonSubscriptionGivenTable LoadByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionGivenTable LoadByPrimaryKey(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PReasonSubscriptionGivenRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PReasonSubscriptionGivenTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PReasonSubscriptionGivenRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PReasonSubscriptionGivenRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionGivenTable LoadUsingTemplate(PReasonSubscriptionGivenRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PReasonSubscriptionGivenTable Data = new PReasonSubscriptionGivenTable();
            LoadUsingTemplate(PReasonSubscriptionGivenTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PReasonSubscriptionGivenTable LoadUsingTemplate(PReasonSubscriptionGivenRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionGivenTable LoadUsingTemplate(PReasonSubscriptionGivenRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionGivenTable LoadUsingTemplate(PReasonSubscriptionGivenRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PReasonSubscriptionGivenTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PReasonSubscriptionGivenTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PReasonSubscriptionGivenTable Data = new PReasonSubscriptionGivenTable();
            LoadUsingTemplate(PReasonSubscriptionGivenTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PReasonSubscriptionGivenTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionGivenTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_reason_subscription_given", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ACode, TDBTransaction ATransaction)
        {
            return Exists(PReasonSubscriptionGivenTable.TableId, new System.Object[1]{ACode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PReasonSubscriptionGivenRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_reason_subscription_given" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PReasonSubscriptionGivenTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PReasonSubscriptionGivenTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_reason_subscription_given" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PReasonSubscriptionGivenTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PReasonSubscriptionGivenTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PReasonSubscriptionGivenTable.TableId, new System.Object[1]{ACode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PReasonSubscriptionGivenRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PReasonSubscriptionGivenTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PReasonSubscriptionGivenTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PReasonSubscriptionGivenTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// List of reasons for cancelling a subscription
    public class PReasonSubscriptionCancelledAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PReasonSubscriptionCancelled";

        /// original table name in database
        public const string DBTABLENAME = "p_reason_subscription_cancelled";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PReasonSubscriptionCancelledTable.TableId) + " FROM PUB_p_reason_subscription_cancelled") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PReasonSubscriptionCancelledTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PReasonSubscriptionCancelledTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PReasonSubscriptionCancelledTable Data = new PReasonSubscriptionCancelledTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PReasonSubscriptionCancelledTable.TableId) + " FROM PUB_p_reason_subscription_cancelled" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PReasonSubscriptionCancelledTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionCancelledTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PReasonSubscriptionCancelledTable.TableId, ADataSet, new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionCancelledTable LoadByPrimaryKey(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PReasonSubscriptionCancelledTable Data = new PReasonSubscriptionCancelledTable();
            LoadByPrimaryKey(PReasonSubscriptionCancelledTable.TableId, Data, new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PReasonSubscriptionCancelledTable LoadByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionCancelledTable LoadByPrimaryKey(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PReasonSubscriptionCancelledRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PReasonSubscriptionCancelledTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PReasonSubscriptionCancelledRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PReasonSubscriptionCancelledRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionCancelledTable LoadUsingTemplate(PReasonSubscriptionCancelledRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PReasonSubscriptionCancelledTable Data = new PReasonSubscriptionCancelledTable();
            LoadUsingTemplate(PReasonSubscriptionCancelledTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PReasonSubscriptionCancelledTable LoadUsingTemplate(PReasonSubscriptionCancelledRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionCancelledTable LoadUsingTemplate(PReasonSubscriptionCancelledRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionCancelledTable LoadUsingTemplate(PReasonSubscriptionCancelledRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PReasonSubscriptionCancelledTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PReasonSubscriptionCancelledTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PReasonSubscriptionCancelledTable Data = new PReasonSubscriptionCancelledTable();
            LoadUsingTemplate(PReasonSubscriptionCancelledTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PReasonSubscriptionCancelledTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PReasonSubscriptionCancelledTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_reason_subscription_cancelled", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String ACode, TDBTransaction ATransaction)
        {
            return Exists(PReasonSubscriptionCancelledTable.TableId, new System.Object[1]{ACode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PReasonSubscriptionCancelledRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_reason_subscription_cancelled" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PReasonSubscriptionCancelledTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PReasonSubscriptionCancelledTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_reason_subscription_cancelled" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PReasonSubscriptionCancelledTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PReasonSubscriptionCancelledTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String ACode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PReasonSubscriptionCancelledTable.TableId, new System.Object[1]{ACode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PReasonSubscriptionCancelledRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PReasonSubscriptionCancelledTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PReasonSubscriptionCancelledTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PReasonSubscriptionCancelledTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Details of which partners receive which publications.
    public class PSubscriptionAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PSubscription";

        /// original table name in database
        public const string DBTABLENAME = "p_subscription";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PSubscriptionTable.TableId) + " FROM PUB_p_subscription") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PSubscriptionTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PSubscriptionTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PSubscriptionTable.TableId) + " FROM PUB_p_subscription" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String APublicationCode, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PSubscriptionTable.TableId, ADataSet, new System.Object[2]{APublicationCode, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String APublicationCode, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APublicationCode, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String APublicationCode, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APublicationCode, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadByPrimaryKey(String APublicationCode, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadByPrimaryKey(PSubscriptionTable.TableId, Data, new System.Object[2]{APublicationCode, APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadByPrimaryKey(String APublicationCode, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(APublicationCode, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadByPrimaryKey(String APublicationCode, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(APublicationCode, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PSubscriptionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PSubscriptionTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PSubscriptionRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PSubscriptionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadUsingTemplate(PSubscriptionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadUsingTemplate(PSubscriptionTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadUsingTemplate(PSubscriptionRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadUsingTemplate(PSubscriptionRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadUsingTemplate(PSubscriptionRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PSubscriptionTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PSubscriptionTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadUsingTemplate(PSubscriptionTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_subscription", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String APublicationCode, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return Exists(PSubscriptionTable.TableId, new System.Object[2]{APublicationCode, APartnerKey}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PSubscriptionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_subscription" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PSubscriptionTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PSubscriptionTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_subscription" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PSubscriptionTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PSubscriptionTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPPublication(DataSet ADataSet, String APublicationCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PPublicationTable.TableId, ADataSet, new string[1]{"p_publication_code_c"},
                new System.Object[1]{APublicationCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPublication(DataSet AData, String APublicationCode, TDBTransaction ATransaction)
        {
            LoadViaPPublication(AData, APublicationCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPublication(DataSet AData, String APublicationCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPublication(AData, APublicationCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPublication(String APublicationCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PPublicationTable.TableId, Data, new string[1]{"p_publication_code_c"},
                new System.Object[1]{APublicationCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPublication(String APublicationCode, TDBTransaction ATransaction)
        {
            return LoadViaPPublication(APublicationCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPublication(String APublicationCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPublication(APublicationCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPublicationTemplate(DataSet ADataSet, PPublicationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PPublicationTable.TableId, ADataSet, new string[1]{"p_publication_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPublicationTemplate(DataSet AData, PPublicationRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPublicationTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPublicationTemplate(DataSet AData, PPublicationRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPublicationTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPublicationTemplate(PPublicationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PPublicationTable.TableId, Data, new string[1]{"p_publication_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPublicationTemplate(PPublicationRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPublicationTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPublicationTemplate(PPublicationRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPublicationTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPublicationTemplate(PPublicationRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPublicationTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPublicationTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PPublicationTable.TableId, ADataSet, new string[1]{"p_publication_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPublicationTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPublicationTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPublicationTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPublicationTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPublicationTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PPublicationTable.TableId, Data, new string[1]{"p_publication_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPublicationTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPublicationTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPublicationTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPublicationTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPublication(String APublicationCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PPublicationTable.TableId, new string[1]{"p_publication_code_c"},
                new System.Object[1]{APublicationCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPublicationTemplate(PPublicationRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PPublicationTable.TableId, new string[1]{"p_publication_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPublicationTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PPublicationTable.TableId, new string[1]{"p_publication_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPartnerPartnerKey(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerPartnerKey(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerPartnerKey(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerPartnerKey(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerPartnerKey(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerPartnerKey(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerPartnerKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPartnerKey(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerPartnerKey(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPartnerKey(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerPartnerKeyTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerPartnerKeyTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerPartnerKeyTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerPartnerKeyTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerPartnerKeyTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerPartnerKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerPartnerKeyTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPartnerKeyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerPartnerKeyTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPartnerKeyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerPartnerKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPartnerKeyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerPartnerKeyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerPartnerKeyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerPartnerKeyTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerPartnerKeyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerPartnerKeyTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerPartnerKeyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerPartnerKeyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPartnerKeyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerPartnerKeyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPartnerKeyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartnerPartnerKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerPartnerKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerPartnerKeyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPartnerGiftFromKey(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_gift_from_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerGiftFromKey(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerGiftFromKey(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerGiftFromKey(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerGiftFromKey(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerGiftFromKey(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_gift_from_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerGiftFromKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerGiftFromKey(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerGiftFromKey(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerGiftFromKey(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerGiftFromKeyTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_gift_from_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerGiftFromKeyTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerGiftFromKeyTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerGiftFromKeyTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerGiftFromKeyTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerGiftFromKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_gift_from_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerGiftFromKeyTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerGiftFromKeyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerGiftFromKeyTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerGiftFromKeyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerGiftFromKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerGiftFromKeyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerGiftFromKeyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_gift_from_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerGiftFromKeyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerGiftFromKeyTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerGiftFromKeyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerGiftFromKeyTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerGiftFromKeyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_gift_from_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerGiftFromKeyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerGiftFromKeyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPPartnerGiftFromKeyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerGiftFromKeyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartnerGiftFromKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, new string[1]{"p_gift_from_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerGiftFromKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, new string[1]{"p_gift_from_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerGiftFromKeyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PPartnerTable.TableId, new string[1]{"p_gift_from_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionGiven(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionGivenTable.TableId, ADataSet, new string[1]{"p_reason_subs_given_code_c"},
                new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionGiven(DataSet AData, String ACode, TDBTransaction ATransaction)
        {
            LoadViaPReasonSubscriptionGiven(AData, ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionGiven(DataSet AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPReasonSubscriptionGiven(AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionGiven(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionGivenTable.TableId, Data, new string[1]{"p_reason_subs_given_code_c"},
                new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionGiven(String ACode, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionGiven(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionGiven(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionGiven(ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionGivenTemplate(DataSet ADataSet, PReasonSubscriptionGivenRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionGivenTable.TableId, ADataSet, new string[1]{"p_reason_subs_given_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionGivenTemplate(DataSet AData, PReasonSubscriptionGivenRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPReasonSubscriptionGivenTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionGivenTemplate(DataSet AData, PReasonSubscriptionGivenRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPReasonSubscriptionGivenTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionGivenTemplate(PReasonSubscriptionGivenRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionGivenTable.TableId, Data, new string[1]{"p_reason_subs_given_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionGivenTemplate(PReasonSubscriptionGivenRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionGivenTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionGivenTemplate(PReasonSubscriptionGivenRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionGivenTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionGivenTemplate(PReasonSubscriptionGivenRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionGivenTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionGivenTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionGivenTable.TableId, ADataSet, new string[1]{"p_reason_subs_given_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionGivenTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPReasonSubscriptionGivenTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionGivenTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPReasonSubscriptionGivenTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionGivenTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionGivenTable.TableId, Data, new string[1]{"p_reason_subs_given_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionGivenTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionGivenTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionGivenTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionGivenTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPReasonSubscriptionGiven(String ACode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionGivenTable.TableId, new string[1]{"p_reason_subs_given_code_c"},
                new System.Object[1]{ACode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPReasonSubscriptionGivenTemplate(PReasonSubscriptionGivenRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionGivenTable.TableId, new string[1]{"p_reason_subs_given_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPReasonSubscriptionGivenTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionGivenTable.TableId, new string[1]{"p_reason_subs_given_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionCancelled(DataSet ADataSet, String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionCancelledTable.TableId, ADataSet, new string[1]{"p_reason_subs_cancelled_code_c"},
                new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionCancelled(DataSet AData, String ACode, TDBTransaction ATransaction)
        {
            LoadViaPReasonSubscriptionCancelled(AData, ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionCancelled(DataSet AData, String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPReasonSubscriptionCancelled(AData, ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionCancelled(String ACode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionCancelledTable.TableId, Data, new string[1]{"p_reason_subs_cancelled_code_c"},
                new System.Object[1]{ACode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionCancelled(String ACode, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionCancelled(ACode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionCancelled(String ACode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionCancelled(ACode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionCancelledTemplate(DataSet ADataSet, PReasonSubscriptionCancelledRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionCancelledTable.TableId, ADataSet, new string[1]{"p_reason_subs_cancelled_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionCancelledTemplate(DataSet AData, PReasonSubscriptionCancelledRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPReasonSubscriptionCancelledTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionCancelledTemplate(DataSet AData, PReasonSubscriptionCancelledRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPReasonSubscriptionCancelledTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionCancelledTemplate(PReasonSubscriptionCancelledRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionCancelledTable.TableId, Data, new string[1]{"p_reason_subs_cancelled_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionCancelledTemplate(PReasonSubscriptionCancelledRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionCancelledTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionCancelledTemplate(PReasonSubscriptionCancelledRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionCancelledTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionCancelledTemplate(PReasonSubscriptionCancelledRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionCancelledTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionCancelledTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionCancelledTable.TableId, ADataSet, new string[1]{"p_reason_subs_cancelled_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionCancelledTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPReasonSubscriptionCancelledTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPReasonSubscriptionCancelledTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPReasonSubscriptionCancelledTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionCancelledTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PSubscriptionTable Data = new PSubscriptionTable();
            LoadViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionCancelledTable.TableId, Data, new string[1]{"p_reason_subs_cancelled_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionCancelledTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionCancelledTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PSubscriptionTable LoadViaPReasonSubscriptionCancelledTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPReasonSubscriptionCancelledTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPReasonSubscriptionCancelled(String ACode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionCancelledTable.TableId, new string[1]{"p_reason_subs_cancelled_code_c"},
                new System.Object[1]{ACode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPReasonSubscriptionCancelledTemplate(PReasonSubscriptionCancelledRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionCancelledTable.TableId, new string[1]{"p_reason_subs_cancelled_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPReasonSubscriptionCancelledTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PSubscriptionTable.TableId, PReasonSubscriptionCancelledTable.TableId, new string[1]{"p_reason_subs_cancelled_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String APublicationCode, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PSubscriptionTable.TableId, new System.Object[2]{APublicationCode, APartnerKey}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PSubscriptionRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PSubscriptionTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PSubscriptionTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PSubscriptionTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Possible attributes for partner contacts.  Gives the description of each attribute code.  An attribute is a type of contact that was made or which occurred with a partner.
    public class PContactAttributeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PContactAttribute";

        /// original table name in database
        public const string DBTABLENAME = "p_contact_attribute";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PContactAttributeTable.TableId) + " FROM PUB_p_contact_attribute") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PContactAttributeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PContactAttributeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PContactAttributeTable Data = new PContactAttributeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PContactAttributeTable.TableId) + " FROM PUB_p_contact_attribute" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PContactAttributeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AContactAttributeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PContactAttributeTable.TableId, ADataSet, new System.Object[1]{AContactAttributeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AContactAttributeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AContactAttributeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AContactAttributeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AContactAttributeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeTable LoadByPrimaryKey(String AContactAttributeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PContactAttributeTable Data = new PContactAttributeTable();
            LoadByPrimaryKey(PContactAttributeTable.TableId, Data, new System.Object[1]{AContactAttributeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PContactAttributeTable LoadByPrimaryKey(String AContactAttributeCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AContactAttributeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeTable LoadByPrimaryKey(String AContactAttributeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AContactAttributeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PContactAttributeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PContactAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PContactAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeTable LoadUsingTemplate(PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PContactAttributeTable Data = new PContactAttributeTable();
            LoadUsingTemplate(PContactAttributeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PContactAttributeTable LoadUsingTemplate(PContactAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeTable LoadUsingTemplate(PContactAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeTable LoadUsingTemplate(PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PContactAttributeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PContactAttributeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PContactAttributeTable Data = new PContactAttributeTable();
            LoadUsingTemplate(PContactAttributeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PContactAttributeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_contact_attribute", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AContactAttributeCode, TDBTransaction ATransaction)
        {
            return Exists(PContactAttributeTable.TableId, new System.Object[1]{AContactAttributeCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_contact_attribute" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PContactAttributeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PContactAttributeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_contact_attribute" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PContactAttributeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PContactAttributeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AContactAttributeCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PContactAttributeTable.TableId, new System.Object[1]{AContactAttributeCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PContactAttributeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PContactAttributeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PContactAttributeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Possible attribute details for each contact attribute.  Breaks down the attribute into more specifice information that applies to a contact with a partner.
    public class PContactAttributeDetailAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PContactAttributeDetail";

        /// original table name in database
        public const string DBTABLENAME = "p_contact_attribute_detail";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PContactAttributeDetailTable.TableId) + " FROM PUB_p_contact_attribute_detail") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PContactAttributeDetailTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PContactAttributeDetailTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PContactAttributeDetailTable Data = new PContactAttributeDetailTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PContactAttributeDetailTable.TableId) + " FROM PUB_p_contact_attribute_detail" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PContactAttributeDetailTable.TableId, ADataSet, new System.Object[2]{AContactAttributeCode, AContactAttrDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AContactAttributeCode, AContactAttrDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AContactAttributeCode, AContactAttrDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadByPrimaryKey(String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PContactAttributeDetailTable Data = new PContactAttributeDetailTable();
            LoadByPrimaryKey(PContactAttributeDetailTable.TableId, Data, new System.Object[2]{AContactAttributeCode, AContactAttrDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadByPrimaryKey(String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AContactAttributeCode, AContactAttrDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadByPrimaryKey(String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AContactAttributeCode, AContactAttrDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PContactAttributeDetailTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PContactAttributeDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PContactAttributeDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadUsingTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PContactAttributeDetailTable Data = new PContactAttributeDetailTable();
            LoadUsingTemplate(PContactAttributeDetailTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadUsingTemplate(PContactAttributeDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadUsingTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadUsingTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PContactAttributeDetailTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PContactAttributeDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PContactAttributeDetailTable Data = new PContactAttributeDetailTable();
            LoadUsingTemplate(PContactAttributeDetailTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_contact_attribute_detail", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            return Exists(PContactAttributeDetailTable.TableId, new System.Object[2]{AContactAttributeCode, AContactAttrDetailCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_contact_attribute_detail" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PContactAttributeDetailTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PContactAttributeDetailTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_contact_attribute_detail" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PContactAttributeDetailTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PContactAttributeDetailTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPContactAttribute(DataSet ADataSet, String AContactAttributeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PContactAttributeDetailTable.TableId, PContactAttributeTable.TableId, ADataSet, new string[1]{"p_contact_attribute_code_c"},
                new System.Object[1]{AContactAttributeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPContactAttribute(DataSet AData, String AContactAttributeCode, TDBTransaction ATransaction)
        {
            LoadViaPContactAttribute(AData, AContactAttributeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttribute(DataSet AData, String AContactAttributeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPContactAttribute(AData, AContactAttributeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPContactAttribute(String AContactAttributeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PContactAttributeDetailTable Data = new PContactAttributeDetailTable();
            LoadViaForeignKey(PContactAttributeDetailTable.TableId, PContactAttributeTable.TableId, Data, new string[1]{"p_contact_attribute_code_c"},
                new System.Object[1]{AContactAttributeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPContactAttribute(String AContactAttributeCode, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttribute(AContactAttributeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPContactAttribute(String AContactAttributeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttribute(AContactAttributeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeTemplate(DataSet ADataSet, PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PContactAttributeDetailTable.TableId, PContactAttributeTable.TableId, ADataSet, new string[1]{"p_contact_attribute_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPContactAttributeTemplate(DataSet AData, PContactAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeTemplate(DataSet AData, PContactAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPContactAttributeTemplate(PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PContactAttributeDetailTable Data = new PContactAttributeDetailTable();
            LoadViaForeignKey(PContactAttributeDetailTable.TableId, PContactAttributeTable.TableId, Data, new string[1]{"p_contact_attribute_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPContactAttributeTemplate(PContactAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPContactAttributeTemplate(PContactAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPContactAttributeTemplate(PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PContactAttributeDetailTable.TableId, PContactAttributeTable.TableId, ADataSet, new string[1]{"p_contact_attribute_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPContactAttributeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPContactAttributeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PContactAttributeDetailTable Data = new PContactAttributeDetailTable();
            LoadViaForeignKey(PContactAttributeDetailTable.TableId, PContactAttributeTable.TableId, Data, new string[1]{"p_contact_attribute_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPContactAttributeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPContactAttributeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPContactAttribute(String AContactAttributeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PContactAttributeDetailTable.TableId, PContactAttributeTable.TableId, new string[1]{"p_contact_attribute_code_c"},
                new System.Object[1]{AContactAttributeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPContactAttributeTemplate(PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PContactAttributeDetailTable.TableId, PContactAttributeTable.TableId, new string[1]{"p_contact_attribute_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPContactAttributeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PContactAttributeDetailTable.TableId, PContactAttributeTable.TableId, new string[1]{"p_contact_attribute_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPPartnerContact(DataSet ADataSet, Int32 AContactId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AContactId));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_contact_attribute_detail", AFieldList, PContactAttributeDetailTable.TableId) +
                            " FROM PUB_p_contact_attribute_detail, PUB_p_partner_contact_attribute WHERE " +
                            "PUB_p_partner_contact_attribute.p_contact_attribute_code_c = PUB_p_contact_attribute_detail.p_contact_attribute_code_c AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = PUB_p_contact_attribute_detail.p_contact_attr_detail_code_c AND PUB_p_partner_contact_attribute.p_contact_id_i = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PContactAttributeDetailTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerContact(DataSet AData, Int32 AContactId, TDBTransaction ATransaction)
        {
            LoadViaPPartnerContact(AData, AContactId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerContact(DataSet AData, Int32 AContactId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerContact(AData, AContactId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPPartnerContact(Int32 AContactId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PContactAttributeDetailTable Data = new PContactAttributeDetailTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPartnerContact(FillDataSet, AContactId, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPPartnerContact(Int32 AContactId, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContact(AContactId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPPartnerContact(Int32 AContactId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContact(AContactId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerContactTemplate(DataSet ADataSet, PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_contact_attribute_detail", AFieldList, PContactAttributeDetailTable.TableId) +
                            " FROM PUB_p_contact_attribute_detail, PUB_p_partner_contact_attribute, PUB_p_partner_contact WHERE " +
                            "PUB_p_partner_contact_attribute.p_contact_attribute_code_c = PUB_p_contact_attribute_detail.p_contact_attribute_code_c AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = PUB_p_contact_attribute_detail.p_contact_attr_detail_code_c AND PUB_p_partner_contact_attribute.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i") +
                            GenerateWhereClauseLong("PUB_p_partner_contact", PPartnerContactTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PContactAttributeDetailTable.TableId), ATransaction,
                            GetParametersForWhereClause(PPartnerContactTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerContactTemplate(DataSet AData, PPartnerContactRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerContactTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerContactTemplate(DataSet AData, PPartnerContactRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerContactTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPPartnerContactTemplate(PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PContactAttributeDetailTable Data = new PContactAttributeDetailTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPartnerContactTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPPartnerContactTemplate(PPartnerContactRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContactTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPPartnerContactTemplate(PPartnerContactRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContactTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPPartnerContactTemplate(PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContactTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerContactTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_contact_attribute_detail", AFieldList, PContactAttributeDetailTable.TableId) +
                            " FROM PUB_p_contact_attribute_detail, PUB_p_partner_contact_attribute, PUB_p_partner_contact WHERE " +
                            "PUB_p_partner_contact_attribute.p_contact_attribute_code_c = PUB_p_contact_attribute_detail.p_contact_attribute_code_c AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = PUB_p_contact_attribute_detail.p_contact_attr_detail_code_c AND PUB_p_partner_contact_attribute.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i") +
                            GenerateWhereClauseLong("PUB_p_partner_contact", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PContactAttributeDetailTable.TableId), ATransaction,
                            GetParametersForWhereClause(PContactAttributeDetailTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerContactTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerContactTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerContactTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerContactTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPPartnerContactTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PContactAttributeDetailTable Data = new PContactAttributeDetailTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPartnerContactTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPPartnerContactTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContactTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PContactAttributeDetailTable LoadViaPPartnerContactTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContactTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPPartnerContact(Int32 AContactId, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Int);
            ParametersArray[0].Value = ((object)(AContactId));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_contact_attribute_detail, PUB_p_partner_contact_attribute WHERE " +
                        "PUB_p_partner_contact_attribute.p_contact_attribute_code_c = PUB_p_contact_attribute_detail.p_contact_attribute_code_c AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = PUB_p_contact_attribute_detail.p_contact_attr_detail_code_c AND PUB_p_partner_contact_attribute.p_contact_id_i = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPPartnerContactTemplate(PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_contact_attribute_detail, PUB_p_partner_contact_attribute, PUB_p_partner_contact WHERE " +
                        "PUB_p_partner_contact_attribute.p_contact_attribute_code_c = PUB_p_contact_attribute_detail.p_contact_attribute_code_c AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = PUB_p_contact_attribute_detail.p_contact_attr_detail_code_c AND PUB_p_partner_contact_attribute.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i" +
                        GenerateWhereClauseLong("PUB_p_partner_contact_attribute", PContactAttributeDetailTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PPartnerContactTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPPartnerContactTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_contact_attribute_detail, PUB_p_partner_contact_attribute, PUB_p_partner_contact WHERE " +
                        "PUB_p_partner_contact_attribute.p_contact_attribute_code_c = PUB_p_contact_attribute_detail.p_contact_attribute_code_c AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = PUB_p_contact_attribute_detail.p_contact_attr_detail_code_c AND PUB_p_partner_contact_attribute.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i" +
                        GenerateWhereClauseLong("PUB_p_partner_contact_attribute", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PContactAttributeDetailTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PContactAttributeDetailTable.TableId, new System.Object[2]{AContactAttributeCode, AContactAttrDetailCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PContactAttributeDetailTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PContactAttributeDetailTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PContactAttributeDetailTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// How contacts are made
    public class PMethodOfContactAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PMethodOfContact";

        /// original table name in database
        public const string DBTABLENAME = "p_method_of_contact";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PMethodOfContactTable.TableId) + " FROM PUB_p_method_of_contact") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PMethodOfContactTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PMethodOfContactTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMethodOfContactTable Data = new PMethodOfContactTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PMethodOfContactTable.TableId) + " FROM PUB_p_method_of_contact" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMethodOfContactTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMethodOfContactTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AMethodOfContactCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PMethodOfContactTable.TableId, ADataSet, new System.Object[1]{AMethodOfContactCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AMethodOfContactCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AMethodOfContactCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AMethodOfContactCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AMethodOfContactCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMethodOfContactTable LoadByPrimaryKey(String AMethodOfContactCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMethodOfContactTable Data = new PMethodOfContactTable();
            LoadByPrimaryKey(PMethodOfContactTable.TableId, Data, new System.Object[1]{AMethodOfContactCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMethodOfContactTable LoadByPrimaryKey(String AMethodOfContactCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AMethodOfContactCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMethodOfContactTable LoadByPrimaryKey(String AMethodOfContactCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AMethodOfContactCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PMethodOfContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PMethodOfContactTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PMethodOfContactRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PMethodOfContactRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMethodOfContactTable LoadUsingTemplate(PMethodOfContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMethodOfContactTable Data = new PMethodOfContactTable();
            LoadUsingTemplate(PMethodOfContactTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMethodOfContactTable LoadUsingTemplate(PMethodOfContactRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMethodOfContactTable LoadUsingTemplate(PMethodOfContactRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMethodOfContactTable LoadUsingTemplate(PMethodOfContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PMethodOfContactTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PMethodOfContactTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PMethodOfContactTable Data = new PMethodOfContactTable();
            LoadUsingTemplate(PMethodOfContactTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PMethodOfContactTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PMethodOfContactTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_method_of_contact", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AMethodOfContactCode, TDBTransaction ATransaction)
        {
            return Exists(PMethodOfContactTable.TableId, new System.Object[1]{AMethodOfContactCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PMethodOfContactRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_method_of_contact" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PMethodOfContactTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PMethodOfContactTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_method_of_contact" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PMethodOfContactTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PMethodOfContactTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AMethodOfContactCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PMethodOfContactTable.TableId, new System.Object[1]{AMethodOfContactCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PMethodOfContactRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PMethodOfContactTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PMethodOfContactTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PMethodOfContactTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Details of contacts with partners
    public class PPartnerContactAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PPartnerContact";

        /// original table name in database
        public const string DBTABLENAME = "p_partner_contact";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PPartnerContactTable.TableId) + " FROM PUB_p_partner_contact") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPartnerContactTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PPartnerContactTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PPartnerContactTable.TableId) + " FROM PUB_p_partner_contact" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 AContactId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PPartnerContactTable.TableId, ADataSet, new System.Object[1]{AContactId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AContactId, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AContactId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AContactId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AContactId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadByPrimaryKey(Int32 AContactId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadByPrimaryKey(PPartnerContactTable.TableId, Data, new System.Object[1]{AContactId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadByPrimaryKey(Int32 AContactId, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AContactId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadByPrimaryKey(Int32 AContactId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AContactId, AFieldList, ATransaction, null, 0, 0);
        }
        /// this method is called by all overloads
        public static void LoadByUniqueKey(DataSet ADataSet, Int64 APartnerKey, System.DateTime AContactDate, Int32 AContactTime, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByUniqueKey(PPartnerContactTable.TableId, ADataSet, new System.Object[3]{APartnerKey, AContactDate, AContactTime}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByUniqueKey(DataSet AData, Int64 APartnerKey, System.DateTime AContactDate, Int32 AContactTime, TDBTransaction ATransaction)
        {
            LoadByUniqueKey(AData, APartnerKey, AContactDate, AContactTime, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByUniqueKey(DataSet AData, Int64 APartnerKey, System.DateTime AContactDate, Int32 AContactTime, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByUniqueKey(AData, APartnerKey, AContactDate, AContactTime, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadByUniqueKey(Int64 APartnerKey, System.DateTime AContactDate, Int32 AContactTime, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadByUniqueKey(PPartnerContactTable.TableId, Data, new System.Object[3]{APartnerKey, AContactDate, AContactTime}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadByUniqueKey(Int64 APartnerKey, System.DateTime AContactDate, Int32 AContactTime, TDBTransaction ATransaction)
        {
            return LoadByUniqueKey(APartnerKey, AContactDate, AContactTime, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadByUniqueKey(Int64 APartnerKey, System.DateTime AContactDate, Int32 AContactTime, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByUniqueKey(APartnerKey, AContactDate, AContactTime, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PPartnerContactTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PPartnerContactRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PPartnerContactRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadUsingTemplate(PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadUsingTemplate(PPartnerContactTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadUsingTemplate(PPartnerContactRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadUsingTemplate(PPartnerContactRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadUsingTemplate(PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PPartnerContactTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PPartnerContactTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadUsingTemplate(PPartnerContactTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_partner_contact", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 AContactId, TDBTransaction ATransaction)
        {
            return Exists(PPartnerContactTable.TableId, new System.Object[1]{AContactId}, ATransaction);
        }

        /// check if a row exists by using the unique key
        public static bool Exists(Int64 APartnerKey, System.DateTime AContactDate, Int32 AContactTime, TDBTransaction ATransaction)
        {
            return Exists(PPartnerContactTable.TableId, new System.Object[3]{APartnerKey, AContactDate, AContactTime}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_partner_contact" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PPartnerContactTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PPartnerContactTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_partner_contact" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PPartnerContactTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PPartnerContactTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPMailing(DataSet ADataSet, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, PMailingTable.TableId, ADataSet, new string[1]{"p_mailing_code_c"},
                new System.Object[1]{AMailingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPMailing(DataSet AData, String AMailingCode, TDBTransaction ATransaction)
        {
            LoadViaPMailing(AData, AMailingCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMailing(DataSet AData, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMailing(AData, AMailingCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMailing(String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, PMailingTable.TableId, Data, new string[1]{"p_mailing_code_c"},
                new System.Object[1]{AMailingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMailing(String AMailingCode, TDBTransaction ATransaction)
        {
            return LoadViaPMailing(AMailingCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMailing(String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMailing(AMailingCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet ADataSet, PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, PMailingTable.TableId, ADataSet, new string[1]{"p_mailing_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet AData, PMailingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet AData, PMailingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, PMailingTable.TableId, Data, new string[1]{"p_mailing_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMailingTemplate(PMailingRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, PMailingTable.TableId, ADataSet, new string[1]{"p_mailing_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMailingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMailingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, PMailingTable.TableId, Data, new string[1]{"p_mailing_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMailingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMailingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPMailing(String AMailingCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, PMailingTable.TableId, new string[1]{"p_mailing_code_c"},
                new System.Object[1]{AMailingCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, PMailingTable.TableId, new string[1]{"p_mailing_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPMailingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, PMailingTable.TableId, new string[1]{"p_mailing_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPMethodOfContact(DataSet ADataSet, String AMethodOfContactCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, PMethodOfContactTable.TableId, ADataSet, new string[1]{"p_contact_code_c"},
                new System.Object[1]{AMethodOfContactCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPMethodOfContact(DataSet AData, String AMethodOfContactCode, TDBTransaction ATransaction)
        {
            LoadViaPMethodOfContact(AData, AMethodOfContactCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMethodOfContact(DataSet AData, String AMethodOfContactCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMethodOfContact(AData, AMethodOfContactCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMethodOfContact(String AMethodOfContactCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, PMethodOfContactTable.TableId, Data, new string[1]{"p_contact_code_c"},
                new System.Object[1]{AMethodOfContactCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMethodOfContact(String AMethodOfContactCode, TDBTransaction ATransaction)
        {
            return LoadViaPMethodOfContact(AMethodOfContactCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMethodOfContact(String AMethodOfContactCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMethodOfContact(AMethodOfContactCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMethodOfContactTemplate(DataSet ADataSet, PMethodOfContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, PMethodOfContactTable.TableId, ADataSet, new string[1]{"p_contact_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPMethodOfContactTemplate(DataSet AData, PMethodOfContactRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPMethodOfContactTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMethodOfContactTemplate(DataSet AData, PMethodOfContactRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMethodOfContactTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMethodOfContactTemplate(PMethodOfContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, PMethodOfContactTable.TableId, Data, new string[1]{"p_contact_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMethodOfContactTemplate(PMethodOfContactRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPMethodOfContactTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMethodOfContactTemplate(PMethodOfContactRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMethodOfContactTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMethodOfContactTemplate(PMethodOfContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMethodOfContactTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMethodOfContactTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, PMethodOfContactTable.TableId, ADataSet, new string[1]{"p_contact_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPMethodOfContactTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPMethodOfContactTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMethodOfContactTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPMethodOfContactTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMethodOfContactTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, PMethodOfContactTable.TableId, Data, new string[1]{"p_contact_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMethodOfContactTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPMethodOfContactTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPMethodOfContactTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMethodOfContactTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPMethodOfContact(String AMethodOfContactCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, PMethodOfContactTable.TableId, new string[1]{"p_contact_code_c"},
                new System.Object[1]{AMethodOfContactCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPMethodOfContactTemplate(PMethodOfContactRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, PMethodOfContactTable.TableId, new string[1]{"p_contact_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPMethodOfContactTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, PMethodOfContactTable.TableId, new string[1]{"p_contact_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaSModule(DataSet ADataSet, String AModuleId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, SModuleTable.TableId, ADataSet, new string[1]{"s_module_id_c"},
                new System.Object[1]{AModuleId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSModule(DataSet AData, String AModuleId, TDBTransaction ATransaction)
        {
            LoadViaSModule(AData, AModuleId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSModule(DataSet AData, String AModuleId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSModule(AData, AModuleId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSModule(String AModuleId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, SModuleTable.TableId, Data, new string[1]{"s_module_id_c"},
                new System.Object[1]{AModuleId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSModule(String AModuleId, TDBTransaction ATransaction)
        {
            return LoadViaSModule(AModuleId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSModule(String AModuleId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSModule(AModuleId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSModuleTemplate(DataSet ADataSet, SModuleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, SModuleTable.TableId, ADataSet, new string[1]{"s_module_id_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSModuleTemplate(DataSet AData, SModuleRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSModuleTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSModuleTemplate(DataSet AData, SModuleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSModuleTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSModuleTemplate(SModuleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, SModuleTable.TableId, Data, new string[1]{"s_module_id_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSModuleTemplate(SModuleRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaSModuleTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSModuleTemplate(SModuleRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSModuleTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSModuleTemplate(SModuleRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSModuleTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSModuleTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, SModuleTable.TableId, ADataSet, new string[1]{"s_module_id_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSModuleTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaSModuleTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSModuleTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSModuleTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSModuleTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, SModuleTable.TableId, Data, new string[1]{"s_module_id_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSModuleTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaSModuleTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSModuleTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSModuleTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaSModule(String AModuleId, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, SModuleTable.TableId, new string[1]{"s_module_id_c"},
                new System.Object[1]{AModuleId}, ATransaction);
        }

        /// auto generated
        public static int CountViaSModuleTemplate(SModuleRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, SModuleTable.TableId, new string[1]{"s_module_id_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaSModuleTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, SModuleTable.TableId, new string[1]{"s_module_id_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaSUser(DataSet ADataSet, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"s_user_id_c"},
                new System.Object[1]{AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUser(DataSet AData, String AUserId, TDBTransaction ATransaction)
        {
            LoadViaSUser(AData, AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUser(DataSet AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUser(AData, AUserId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSUser(String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, SUserTable.TableId, Data, new string[1]{"s_user_id_c"},
                new System.Object[1]{AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            return LoadViaSUser(AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSUser(String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUser(AUserId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"s_user_id_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, SUserTable.TableId, Data, new string[1]{"s_user_id_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSUserTemplate(SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"s_user_id_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactTable Data = new PPartnerContactTable();
            LoadViaForeignKey(PPartnerContactTable.TableId, SUserTable.TableId, Data, new string[1]{"s_user_id_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, SUserTable.TableId, new string[1]{"s_user_id_c"},
                new System.Object[1]{AUserId}, ATransaction);
        }

        /// auto generated
        public static int CountViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, SUserTable.TableId, new string[1]{"s_user_id_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaSUserTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactTable.TableId, SUserTable.TableId, new string[1]{"s_user_id_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPContactAttributeDetail(DataSet ADataSet, String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AContactAttributeCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[1].Value = ((object)(AContactAttrDetailCode));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_partner_contact", AFieldList, PPartnerContactTable.TableId) +
                            " FROM PUB_p_partner_contact, PUB_p_partner_contact_attribute WHERE " +
                            "PUB_p_partner_contact_attribute.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_p_partner_contact_attribute.p_contact_attribute_code_c = ? AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPartnerContactTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetail(DataSet AData, String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeDetail(AData, AContactAttributeCode, AContactAttrDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetail(DataSet AData, String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeDetail(AData, AContactAttributeCode, AContactAttrDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPContactAttributeDetail(String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PPartnerContactTable Data = new PPartnerContactTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPContactAttributeDetail(FillDataSet, AContactAttributeCode, AContactAttrDetailCode, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPContactAttributeDetail(String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetail(AContactAttributeCode, AContactAttrDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPContactAttributeDetail(String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetail(AContactAttributeCode, AContactAttrDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetailTemplate(DataSet ADataSet, PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_partner_contact", AFieldList, PPartnerContactTable.TableId) +
                            " FROM PUB_p_partner_contact, PUB_p_partner_contact_attribute, PUB_p_contact_attribute_detail WHERE " +
                            "PUB_p_partner_contact_attribute.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_p_partner_contact_attribute.p_contact_attribute_code_c = PUB_p_contact_attribute_detail.p_contact_attribute_code_c AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = PUB_p_contact_attribute_detail.p_contact_attr_detail_code_c") +
                            GenerateWhereClauseLong("PUB_p_contact_attribute_detail", PContactAttributeDetailTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPartnerContactTable.TableId), ATransaction,
                            GetParametersForWhereClause(PContactAttributeDetailTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetailTemplate(DataSet AData, PContactAttributeDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeDetailTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetailTemplate(DataSet AData, PContactAttributeDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeDetailTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPContactAttributeDetailTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PPartnerContactTable Data = new PPartnerContactTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPContactAttributeDetailTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPContactAttributeDetailTemplate(PContactAttributeDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetailTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPContactAttributeDetailTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetailTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPContactAttributeDetailTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetailTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetailTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_partner_contact", AFieldList, PPartnerContactTable.TableId) +
                            " FROM PUB_p_partner_contact, PUB_p_partner_contact_attribute, PUB_p_contact_attribute_detail WHERE " +
                            "PUB_p_partner_contact_attribute.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_p_partner_contact_attribute.p_contact_attribute_code_c = PUB_p_contact_attribute_detail.p_contact_attribute_code_c AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = PUB_p_contact_attribute_detail.p_contact_attr_detail_code_c") +
                            GenerateWhereClauseLong("PUB_p_contact_attribute_detail", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPartnerContactTable.TableId), ATransaction,
                            GetParametersForWhereClause(PPartnerContactTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeDetailTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeDetailTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPContactAttributeDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PPartnerContactTable Data = new PPartnerContactTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPContactAttributeDetailTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPContactAttributeDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetailTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPContactAttributeDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetailTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPContactAttributeDetail(String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[0].Value = ((object)(AContactAttributeCode));
            ParametersArray[1] = new OdbcParameter("", OdbcType.VarChar, 32);
            ParametersArray[1].Value = ((object)(AContactAttrDetailCode));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_partner_contact, PUB_p_partner_contact_attribute WHERE " +
                        "PUB_p_partner_contact_attribute.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_p_partner_contact_attribute.p_contact_attribute_code_c = ? AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPContactAttributeDetailTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_partner_contact, PUB_p_partner_contact_attribute, PUB_p_contact_attribute_detail WHERE " +
                        "PUB_p_partner_contact_attribute.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_p_partner_contact_attribute.p_contact_attribute_code_c = PUB_p_contact_attribute_detail.p_contact_attribute_code_c AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = PUB_p_contact_attribute_detail.p_contact_attr_detail_code_c" +
                        GenerateWhereClauseLong("PUB_p_partner_contact_attribute", PPartnerContactTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PContactAttributeDetailTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPContactAttributeDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_partner_contact, PUB_p_partner_contact_attribute, PUB_p_contact_attribute_detail WHERE " +
                        "PUB_p_partner_contact_attribute.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_p_partner_contact_attribute.p_contact_attribute_code_c = PUB_p_contact_attribute_detail.p_contact_attribute_code_c AND PUB_p_partner_contact_attribute.p_contact_attr_detail_code_c = PUB_p_contact_attribute_detail.p_contact_attr_detail_code_c" +
                        GenerateWhereClauseLong("PUB_p_partner_contact_attribute", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PPartnerContactTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaPPartnerPPartnerReminder(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_partner_contact", AFieldList, PPartnerContactTable.TableId) +
                            " FROM PUB_p_partner_contact, PUB_p_partner_reminder WHERE " +
                            "PUB_p_partner_reminder.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_p_partner_reminder.p_partner_key_n = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPartnerContactTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerPPartnerReminder(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerPPartnerReminder(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerPPartnerReminder(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerPPartnerReminder(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerPPartnerReminder(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PPartnerContactTable Data = new PPartnerContactTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPartnerPPartnerReminder(FillDataSet, APartnerKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerPPartnerReminder(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPPartnerReminder(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerPPartnerReminder(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPPartnerReminder(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerPPartnerReminderTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_partner_contact", AFieldList, PPartnerContactTable.TableId) +
                            " FROM PUB_p_partner_contact, PUB_p_partner_reminder, PUB_p_partner WHERE " +
                            "PUB_p_partner_reminder.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_p_partner_reminder.p_partner_key_n = PUB_p_partner.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_partner", PPartnerTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPartnerContactTable.TableId), ATransaction,
                            GetParametersForWhereClause(PPartnerTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerPPartnerReminderTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerPPartnerReminderTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerPPartnerReminderTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerPPartnerReminderTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerPPartnerReminderTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PPartnerContactTable Data = new PPartnerContactTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPartnerPPartnerReminderTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerPPartnerReminderTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPPartnerReminderTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerPPartnerReminderTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPPartnerReminderTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerPPartnerReminderTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPPartnerReminderTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerPPartnerReminderTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_partner_contact", AFieldList, PPartnerContactTable.TableId) +
                            " FROM PUB_p_partner_contact, PUB_p_partner_reminder, PUB_p_partner WHERE " +
                            "PUB_p_partner_reminder.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_p_partner_reminder.p_partner_key_n = PUB_p_partner.p_partner_key_n") +
                            GenerateWhereClauseLong("PUB_p_partner", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPartnerContactTable.TableId), ATransaction,
                            GetParametersForWhereClause(PPartnerContactTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerPPartnerReminderTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerPPartnerReminderTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerPPartnerReminderTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerPPartnerReminderTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerPPartnerReminderTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PPartnerContactTable Data = new PPartnerContactTable();
            FillDataSet.Tables.Add(Data);
            LoadViaPPartnerPPartnerReminderTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerPPartnerReminderTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPPartnerReminderTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaPPartnerPPartnerReminderTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerPPartnerReminderTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaPPartnerPPartnerReminder(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[1];
            ParametersArray[0] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[0].Value = ((object)(APartnerKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_partner_contact, PUB_p_partner_reminder WHERE " +
                        "PUB_p_partner_reminder.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_p_partner_reminder.p_partner_key_n = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaPPartnerPPartnerReminderTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_partner_contact, PUB_p_partner_reminder, PUB_p_partner WHERE " +
                        "PUB_p_partner_reminder.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_p_partner_reminder.p_partner_key_n = PUB_p_partner.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_p_partner_reminder", PPartnerContactTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(PPartnerTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaPPartnerPPartnerReminderTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_partner_contact, PUB_p_partner_reminder, PUB_p_partner WHERE " +
                        "PUB_p_partner_reminder.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_p_partner_reminder.p_partner_key_n = PUB_p_partner.p_partner_key_n" +
                        GenerateWhereClauseLong("PUB_p_partner_reminder", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PPartnerContactTable.TableId, ASearchCriteria)));
        }

        /// auto generated LoadViaLinkTable
        public static void LoadViaSGroup(DataSet ADataSet, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AGroupId));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AUnitKey));
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_p_partner_contact", AFieldList, PPartnerContactTable.TableId) +
                            " FROM PUB_p_partner_contact, PUB_s_group_partner_contact WHERE " +
                            "PUB_s_group_partner_contact.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_s_group_partner_contact.s_group_id_c = ? AND PUB_s_group_partner_contact.s_group_unit_key_n = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPartnerContactTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSGroup(DataSet AData, String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            LoadViaSGroup(AData, AGroupId, AUnitKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroup(DataSet AData, String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroup(AData, AGroupId, AUnitKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSGroup(String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PPartnerContactTable Data = new PPartnerContactTable();
            FillDataSet.Tables.Add(Data);
            LoadViaSGroup(FillDataSet, AGroupId, AUnitKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSGroup(String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            return LoadViaSGroup(AGroupId, AUnitKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSGroup(String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroup(AGroupId, AUnitKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet ADataSet, SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_partner_contact", AFieldList, PPartnerContactTable.TableId) +
                            " FROM PUB_p_partner_contact, PUB_s_group_partner_contact, PUB_s_group WHERE " +
                            "PUB_s_group_partner_contact.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_s_group_partner_contact.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_partner_contact.s_group_unit_key_n = PUB_s_group.s_unit_key_n") +
                            GenerateWhereClauseLong("PUB_s_group", SGroupTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPartnerContactTable.TableId), ATransaction,
                            GetParametersForWhereClause(SGroupTable.TableId, ATemplateRow), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet AData, SGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet AData, SGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PPartnerContactTable Data = new PPartnerContactTable();
            FillDataSet.Tables.Add(Data);
            LoadViaSGroupTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_p_partner_contact", AFieldList, PPartnerContactTable.TableId) +
                            " FROM PUB_p_partner_contact, PUB_s_group_partner_contact, PUB_s_group WHERE " +
                            "PUB_s_group_partner_contact.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_s_group_partner_contact.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_partner_contact.s_group_unit_key_n = PUB_s_group.s_unit_key_n") +
                            GenerateWhereClauseLong("PUB_s_group", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPartnerContactTable.TableId), ATransaction,
                            GetParametersForWhereClause(PPartnerContactTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSGroupTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            PPartnerContactTable Data = new PPartnerContactTable();
            FillDataSet.Tables.Add(Data);
            LoadViaSGroupTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactTable LoadViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated CountViaLinkTable
        public static int CountViaSGroup(String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            OdbcParameter[] ParametersArray = new OdbcParameter[2];
            ParametersArray[0] = new OdbcParameter("", OdbcType.VarChar, 20);
            ParametersArray[0].Value = ((object)(AGroupId));
            ParametersArray[1] = new OdbcParameter("", OdbcType.Decimal, 10);
            ParametersArray[1].Value = ((object)(AUnitKey));
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_partner_contact, PUB_s_group_partner_contact WHERE " +
                        "PUB_s_group_partner_contact.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_s_group_partner_contact.s_group_id_c = ? AND PUB_s_group_partner_contact.s_group_unit_key_n = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_partner_contact, PUB_s_group_partner_contact, PUB_s_group WHERE " +
                        "PUB_s_group_partner_contact.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_s_group_partner_contact.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_partner_contact.s_group_unit_key_n = PUB_s_group.s_unit_key_n" +
                        GenerateWhereClauseLong("PUB_s_group_partner_contact", PPartnerContactTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(SGroupTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_partner_contact, PUB_s_group_partner_contact, PUB_s_group WHERE " +
                        "PUB_s_group_partner_contact.p_contact_id_i = PUB_p_partner_contact.p_contact_id_i AND PUB_s_group_partner_contact.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_partner_contact.s_group_unit_key_n = PUB_s_group.s_unit_key_n" +
                        GenerateWhereClauseLong("PUB_s_group_partner_contact", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(PPartnerContactTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 AContactId, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PPartnerContactTable.TableId, new System.Object[1]{AContactId}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PPartnerContactTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PPartnerContactTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PPartnerContactTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID, "seq_contact", "p_contact_id_i");
        }
    }

    /// Associates a p_contact_attribute_detail with a p_partner_contact.  A contact with a partner may have more than one p_contact_attribute_detail associated with it.
    public class PPartnerContactAttributeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "PPartnerContactAttribute";

        /// original table name in database
        public const string DBTABLENAME = "p_partner_contact_attribute";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, PPartnerContactAttributeTable.TableId) + " FROM PUB_p_partner_contact_attribute") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(PPartnerContactAttributeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static PPartnerContactAttributeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactAttributeTable Data = new PPartnerContactAttributeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, PPartnerContactAttributeTable.TableId) + " FROM PUB_p_partner_contact_attribute" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 AContactId, String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(PPartnerContactAttributeTable.TableId, ADataSet, new System.Object[3]{AContactId, AContactAttributeCode, AContactAttrDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AContactId, String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AContactId, AContactAttributeCode, AContactAttrDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 AContactId, String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AContactId, AContactAttributeCode, AContactAttrDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadByPrimaryKey(Int32 AContactId, String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactAttributeTable Data = new PPartnerContactAttributeTable();
            LoadByPrimaryKey(PPartnerContactAttributeTable.TableId, Data, new System.Object[3]{AContactId, AContactAttributeCode, AContactAttrDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadByPrimaryKey(Int32 AContactId, String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AContactId, AContactAttributeCode, AContactAttrDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadByPrimaryKey(Int32 AContactId, String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AContactId, AContactAttributeCode, AContactAttrDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, PPartnerContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PPartnerContactAttributeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PPartnerContactAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, PPartnerContactAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadUsingTemplate(PPartnerContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactAttributeTable Data = new PPartnerContactAttributeTable();
            LoadUsingTemplate(PPartnerContactAttributeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadUsingTemplate(PPartnerContactAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadUsingTemplate(PPartnerContactAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadUsingTemplate(PPartnerContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(PPartnerContactAttributeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static PPartnerContactAttributeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactAttributeTable Data = new PPartnerContactAttributeTable();
            LoadUsingTemplate(PPartnerContactAttributeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_p_partner_contact_attribute", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 AContactId, String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            return Exists(PPartnerContactAttributeTable.TableId, new System.Object[3]{AContactId, AContactAttributeCode, AContactAttrDetailCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(PPartnerContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_partner_contact_attribute" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PPartnerContactAttributeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(PPartnerContactAttributeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_p_partner_contact_attribute" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(PPartnerContactAttributeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(PPartnerContactAttributeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetail(DataSet ADataSet, String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeDetailTable.TableId, ADataSet, new string[2]{"p_contact_attribute_code_c", "p_contact_attr_detail_code_c"},
                new System.Object[2]{AContactAttributeCode, AContactAttrDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetail(DataSet AData, String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeDetail(AData, AContactAttributeCode, AContactAttrDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetail(DataSet AData, String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeDetail(AData, AContactAttributeCode, AContactAttrDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeDetail(String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactAttributeTable Data = new PPartnerContactAttributeTable();
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeDetailTable.TableId, Data, new string[2]{"p_contact_attribute_code_c", "p_contact_attr_detail_code_c"},
                new System.Object[2]{AContactAttributeCode, AContactAttrDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeDetail(String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetail(AContactAttributeCode, AContactAttrDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeDetail(String AContactAttributeCode, String AContactAttrDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetail(AContactAttributeCode, AContactAttrDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetailTemplate(DataSet ADataSet, PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeDetailTable.TableId, ADataSet, new string[2]{"p_contact_attribute_code_c", "p_contact_attr_detail_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetailTemplate(DataSet AData, PContactAttributeDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeDetailTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetailTemplate(DataSet AData, PContactAttributeDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeDetailTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeDetailTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactAttributeTable Data = new PPartnerContactAttributeTable();
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeDetailTable.TableId, Data, new string[2]{"p_contact_attribute_code_c", "p_contact_attr_detail_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeDetailTemplate(PContactAttributeDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetailTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeDetailTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetailTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeDetailTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetailTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetailTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeDetailTable.TableId, ADataSet, new string[2]{"p_contact_attribute_code_c", "p_contact_attr_detail_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeDetailTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeDetailTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactAttributeTable Data = new PPartnerContactAttributeTable();
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeDetailTable.TableId, Data, new string[2]{"p_contact_attribute_code_c", "p_contact_attr_detail_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetailTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeDetailTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPContactAttributeDetail(String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeDetailTable.TableId, new string[2]{"p_contact_attribute_code_c", "p_contact_attr_detail_code_c"},
                new System.Object[2]{AContactAttributeCode, AContactAttrDetailCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPContactAttributeDetailTemplate(PContactAttributeDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeDetailTable.TableId, new string[2]{"p_contact_attribute_code_c", "p_contact_attr_detail_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPContactAttributeDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeDetailTable.TableId, new string[2]{"p_contact_attribute_code_c", "p_contact_attr_detail_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPContactAttribute(DataSet ADataSet, String AContactAttributeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeTable.TableId, ADataSet, new string[1]{"p_contact_attribute_code_c"},
                new System.Object[1]{AContactAttributeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPContactAttribute(DataSet AData, String AContactAttributeCode, TDBTransaction ATransaction)
        {
            LoadViaPContactAttribute(AData, AContactAttributeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttribute(DataSet AData, String AContactAttributeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPContactAttribute(AData, AContactAttributeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttribute(String AContactAttributeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactAttributeTable Data = new PPartnerContactAttributeTable();
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeTable.TableId, Data, new string[1]{"p_contact_attribute_code_c"},
                new System.Object[1]{AContactAttributeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttribute(String AContactAttributeCode, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttribute(AContactAttributeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttribute(String AContactAttributeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttribute(AContactAttributeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeTemplate(DataSet ADataSet, PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeTable.TableId, ADataSet, new string[1]{"p_contact_attribute_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPContactAttributeTemplate(DataSet AData, PContactAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeTemplate(DataSet AData, PContactAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeTemplate(PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactAttributeTable Data = new PPartnerContactAttributeTable();
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeTable.TableId, Data, new string[1]{"p_contact_attribute_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeTemplate(PContactAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeTemplate(PContactAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeTemplate(PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeTable.TableId, ADataSet, new string[1]{"p_contact_attribute_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPContactAttributeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPContactAttributeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPContactAttributeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactAttributeTable Data = new PPartnerContactAttributeTable();
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeTable.TableId, Data, new string[1]{"p_contact_attribute_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPContactAttributeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPContactAttributeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPContactAttribute(String AContactAttributeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeTable.TableId, new string[1]{"p_contact_attribute_code_c"},
                new System.Object[1]{AContactAttributeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPContactAttributeTemplate(PContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeTable.TableId, new string[1]{"p_contact_attribute_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPContactAttributeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactAttributeTable.TableId, PContactAttributeTable.TableId, new string[1]{"p_contact_attribute_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPartnerContact(DataSet ADataSet, Int32 AContactId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PPartnerContactTable.TableId, ADataSet, new string[1]{"p_contact_id_i"},
                new System.Object[1]{AContactId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerContact(DataSet AData, Int32 AContactId, TDBTransaction ATransaction)
        {
            LoadViaPPartnerContact(AData, AContactId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerContact(DataSet AData, Int32 AContactId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerContact(AData, AContactId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPPartnerContact(Int32 AContactId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactAttributeTable Data = new PPartnerContactAttributeTable();
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PPartnerContactTable.TableId, Data, new string[1]{"p_contact_id_i"},
                new System.Object[1]{AContactId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPPartnerContact(Int32 AContactId, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContact(AContactId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPPartnerContact(Int32 AContactId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContact(AContactId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerContactTemplate(DataSet ADataSet, PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PPartnerContactTable.TableId, ADataSet, new string[1]{"p_contact_id_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerContactTemplate(DataSet AData, PPartnerContactRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerContactTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerContactTemplate(DataSet AData, PPartnerContactRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerContactTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPPartnerContactTemplate(PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactAttributeTable Data = new PPartnerContactAttributeTable();
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PPartnerContactTable.TableId, Data, new string[1]{"p_contact_id_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPPartnerContactTemplate(PPartnerContactRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContactTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPPartnerContactTemplate(PPartnerContactRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContactTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPPartnerContactTemplate(PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContactTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerContactTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PPartnerContactTable.TableId, ADataSet, new string[1]{"p_contact_id_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerContactTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerContactTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerContactTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerContactTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPPartnerContactTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            PPartnerContactAttributeTable Data = new PPartnerContactAttributeTable();
            LoadViaForeignKey(PPartnerContactAttributeTable.TableId, PPartnerContactTable.TableId, Data, new string[1]{"p_contact_id_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPPartnerContactTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContactTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static PPartnerContactAttributeTable LoadViaPPartnerContactTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerContactTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartnerContact(Int32 AContactId, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactAttributeTable.TableId, PPartnerContactTable.TableId, new string[1]{"p_contact_id_i"},
                new System.Object[1]{AContactId}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerContactTemplate(PPartnerContactRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactAttributeTable.TableId, PPartnerContactTable.TableId, new string[1]{"p_contact_id_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerContactTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(PPartnerContactAttributeTable.TableId, PPartnerContactTable.TableId, new string[1]{"p_contact_id_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 AContactId, String AContactAttributeCode, String AContactAttrDetailCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(PPartnerContactAttributeTable.TableId, new System.Object[3]{AContactId, AContactAttributeCode, AContactAttrDetailCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(PPartnerContactAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PPartnerContactAttributeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(PPartnerContactAttributeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(PPartnerContactAttributeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }
}
