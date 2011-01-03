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
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;

namespace Ict.Petra.Server.MFinance.Gift.Data.Access
{

    /// Media"" types of money received.  Eg: Cash, Check Credit Card.
    public class AMethodOfPaymentAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AMethodOfPayment";

        /// original table name in database
        public const string DBTABLENAME = "a_method_of_payment";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AMethodOfPaymentTable.TableId) + " FROM PUB_a_method_of_payment") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AMethodOfPaymentTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AMethodOfPaymentTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMethodOfPaymentTable Data = new AMethodOfPaymentTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AMethodOfPaymentTable.TableId) + " FROM PUB_a_method_of_payment" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AMethodOfPaymentTable.TableId, ADataSet, new System.Object[1]{AMethodOfPaymentCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AMethodOfPaymentCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, AMethodOfPaymentCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadByPrimaryKey(String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMethodOfPaymentTable Data = new AMethodOfPaymentTable();
            LoadByPrimaryKey(AMethodOfPaymentTable.TableId, Data, new System.Object[1]{AMethodOfPaymentCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadByPrimaryKey(String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AMethodOfPaymentCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadByPrimaryKey(String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(AMethodOfPaymentCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AMethodOfPaymentTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMethodOfPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMethodOfPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadUsingTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMethodOfPaymentTable Data = new AMethodOfPaymentTable();
            LoadUsingTemplate(AMethodOfPaymentTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadUsingTemplate(AMethodOfPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadUsingTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadUsingTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AMethodOfPaymentTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AMethodOfPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMethodOfPaymentTable Data = new AMethodOfPaymentTable();
            LoadUsingTemplate(AMethodOfPaymentTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_method_of_payment", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            return Exists(AMethodOfPaymentTable.TableId, new System.Object[1]{AMethodOfPaymentCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_method_of_payment" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AMethodOfPaymentTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AMethodOfPaymentTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_method_of_payment" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AMethodOfPaymentTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AMethodOfPaymentTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaSFile(DataSet ADataSet, String AFileName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMethodOfPaymentTable.TableId, SFileTable.TableId, ADataSet, new string[1]{"a_process_to_call_c"},
                new System.Object[1]{AFileName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSFile(DataSet AData, String AFileName, TDBTransaction ATransaction)
        {
            LoadViaSFile(AData, AFileName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSFile(DataSet AData, String AFileName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSFile(AData, AFileName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadViaSFile(String AFileName, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMethodOfPaymentTable Data = new AMethodOfPaymentTable();
            LoadViaForeignKey(AMethodOfPaymentTable.TableId, SFileTable.TableId, Data, new string[1]{"a_process_to_call_c"},
                new System.Object[1]{AFileName}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadViaSFile(String AFileName, TDBTransaction ATransaction)
        {
            return LoadViaSFile(AFileName, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadViaSFile(String AFileName, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSFile(AFileName, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSFileTemplate(DataSet ADataSet, SFileRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMethodOfPaymentTable.TableId, SFileTable.TableId, ADataSet, new string[1]{"a_process_to_call_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSFileTemplate(DataSet AData, SFileRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSFileTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSFileTemplate(DataSet AData, SFileRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSFileTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadViaSFileTemplate(SFileRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMethodOfPaymentTable Data = new AMethodOfPaymentTable();
            LoadViaForeignKey(AMethodOfPaymentTable.TableId, SFileTable.TableId, Data, new string[1]{"a_process_to_call_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadViaSFileTemplate(SFileRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaSFileTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadViaSFileTemplate(SFileRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSFileTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadViaSFileTemplate(SFileRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSFileTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSFileTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMethodOfPaymentTable.TableId, SFileTable.TableId, ADataSet, new string[1]{"a_process_to_call_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSFileTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaSFileTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSFileTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSFileTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadViaSFileTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMethodOfPaymentTable Data = new AMethodOfPaymentTable();
            LoadViaForeignKey(AMethodOfPaymentTable.TableId, SFileTable.TableId, Data, new string[1]{"a_process_to_call_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadViaSFileTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaSFileTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMethodOfPaymentTable LoadViaSFileTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSFileTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaSFile(String AFileName, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMethodOfPaymentTable.TableId, SFileTable.TableId, new string[1]{"a_process_to_call_c"},
                new System.Object[1]{AFileName}, ATransaction);
        }

        /// auto generated
        public static int CountViaSFileTemplate(SFileRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMethodOfPaymentTable.TableId, SFileTable.TableId, new string[1]{"a_process_to_call_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaSFileTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMethodOfPaymentTable.TableId, SFileTable.TableId, new string[1]{"a_process_to_call_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AMethodOfPaymentTable.TableId, new System.Object[1]{AMethodOfPaymentCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AMethodOfPaymentTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AMethodOfPaymentTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AMethodOfPaymentTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// This is used to track a partner's reason for contacting the organisation/sending money. Divided into Motivation Detail codes.
    public class AMotivationGroupAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AMotivationGroup";

        /// original table name in database
        public const string DBTABLENAME = "a_motivation_group";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AMotivationGroupTable.TableId) + " FROM PUB_a_motivation_group") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AMotivationGroupTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AMotivationGroupTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationGroupTable Data = new AMotivationGroupTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AMotivationGroupTable.TableId) + " FROM PUB_a_motivation_group" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationGroupTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AMotivationGroupTable.TableId, ADataSet, new System.Object[2]{ALedgerNumber, AMotivationGroupCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AMotivationGroupCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AMotivationGroupCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationGroupTable Data = new AMotivationGroupTable();
            LoadByPrimaryKey(AMotivationGroupTable.TableId, Data, new System.Object[2]{ALedgerNumber, AMotivationGroupCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationGroupTable LoadByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AMotivationGroupCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AMotivationGroupCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AMotivationGroupTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMotivationGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMotivationGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadUsingTemplate(AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationGroupTable Data = new AMotivationGroupTable();
            LoadUsingTemplate(AMotivationGroupTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationGroupTable LoadUsingTemplate(AMotivationGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadUsingTemplate(AMotivationGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadUsingTemplate(AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AMotivationGroupTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AMotivationGroupTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationGroupTable Data = new AMotivationGroupTable();
            LoadUsingTemplate(AMotivationGroupTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationGroupTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_group", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            return Exists(AMotivationGroupTable.TableId, new System.Object[2]{ALedgerNumber, AMotivationGroupCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_group" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AMotivationGroupTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AMotivationGroupTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_group" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AMotivationGroupTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AMotivationGroupTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationGroupTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationGroupTable Data = new AMotivationGroupTable();
            LoadViaForeignKey(AMotivationGroupTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationGroupTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationGroupTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationGroupTable Data = new AMotivationGroupTable();
            LoadViaForeignKey(AMotivationGroupTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationGroupTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationGroupTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationGroupTable Data = new AMotivationGroupTable();
            LoadViaForeignKey(AMotivationGroupTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationGroupTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationGroupTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationGroupTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationGroupTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationGroupTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AMotivationGroupTable.TableId, new System.Object[2]{ALedgerNumber, AMotivationGroupCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AMotivationGroupTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AMotivationGroupTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AMotivationGroupTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Used as a subdvision of motivation group. Details of the reason money has been received, where it is going (cost centre and account), and fees to be charged on it.
    public class AMotivationDetailAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AMotivationDetail";

        /// original table name in database
        public const string DBTABLENAME = "a_motivation_detail";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AMotivationDetailTable.TableId) + " FROM PUB_a_motivation_detail") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AMotivationDetailTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AMotivationDetailTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AMotivationDetailTable.TableId) + " FROM PUB_a_motivation_detail" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AMotivationDetailTable.TableId, ADataSet, new System.Object[3]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadByPrimaryKey(AMotivationDetailTable.TableId, Data, new System.Object[3]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AMotivationDetailTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadUsingTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadUsingTemplate(AMotivationDetailTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadUsingTemplate(AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadUsingTemplate(AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadUsingTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AMotivationDetailTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AMotivationDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadUsingTemplate(AMotivationDetailTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_detail", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            return Exists(AMotivationDetailTable.TableId, new System.Object[3]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_detail" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AMotivationDetailTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AMotivationDetailTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_detail" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AMotivationDetailTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AMotivationDetailTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAMotivationGroup(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, AMotivationGroupTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_motivation_group_code_c"},
                new System.Object[2]{ALedgerNumber, AMotivationGroupCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMotivationGroup(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroup(AData, ALedgerNumber, AMotivationGroupCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationGroup(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroup(AData, ALedgerNumber, AMotivationGroupCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAMotivationGroup(Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, AMotivationGroupTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_motivation_group_code_c"},
                new System.Object[2]{ALedgerNumber, AMotivationGroupCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAMotivationGroup(Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationGroup(ALedgerNumber, AMotivationGroupCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAMotivationGroup(Int32 ALedgerNumber, String AMotivationGroupCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationGroup(ALedgerNumber, AMotivationGroupCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationGroupTemplate(DataSet ADataSet, AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, AMotivationGroupTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_motivation_group_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMotivationGroupTemplate(DataSet AData, AMotivationGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroupTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationGroupTemplate(DataSet AData, AMotivationGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroupTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAMotivationGroupTemplate(AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, AMotivationGroupTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_motivation_group_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAMotivationGroupTemplate(AMotivationGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationGroupTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAMotivationGroupTemplate(AMotivationGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationGroupTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAMotivationGroupTemplate(AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationGroupTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationGroupTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, AMotivationGroupTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_motivation_group_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMotivationGroupTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroupTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationGroupTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationGroupTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAMotivationGroupTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, AMotivationGroupTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_motivation_group_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAMotivationGroupTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationGroupTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAMotivationGroupTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationGroupTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAMotivationGroup(Int32 ALedgerNumber, String AMotivationGroupCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, AMotivationGroupTable.TableId, new string[2]{"a_ledger_number_i", "a_motivation_group_code_c"},
                new System.Object[2]{ALedgerNumber, AMotivationGroupCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAMotivationGroupTemplate(AMotivationGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, AMotivationGroupTable.TableId, new string[2]{"a_ledger_number_i", "a_motivation_group_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAMotivationGroupTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, AMotivationGroupTable.TableId, new string[2]{"a_ledger_number_i", "a_motivation_group_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet ADataSet, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet ADataSet, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            return LoadViaACostCentre(ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentre(ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_recipient_key_n"},
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
        public static AMotivationDetailTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_recipient_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_recipient_key_n"},
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
        public static AMotivationDetailTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_recipient_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_recipient_key_n"},
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
        public static AMotivationDetailTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailTable Data = new AMotivationDetailTable();
            LoadViaForeignKey(AMotivationDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_recipient_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, PPartnerTable.TableId, new string[1]{"p_recipient_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, PPartnerTable.TableId, new string[1]{"p_recipient_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailTable.TableId, PPartnerTable.TableId, new string[1]{"p_recipient_key_n"},
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_motivation_detail", AFieldList, AMotivationDetailTable.TableId) +
                            " FROM PUB_a_motivation_detail, PUB_s_group_motivation WHERE " +
                            "PUB_s_group_motivation.a_ledger_number_i = PUB_a_motivation_detail.a_ledger_number_i AND PUB_s_group_motivation.a_motivation_group_code_c = PUB_a_motivation_detail.a_motivation_group_code_c AND PUB_s_group_motivation.a_motivation_detail_code_c = PUB_a_motivation_detail.a_motivation_detail_code_c AND PUB_s_group_motivation.s_group_id_c = ? AND PUB_s_group_motivation.s_group_unit_key_n = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AMotivationDetailTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static AMotivationDetailTable LoadViaSGroup(String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AMotivationDetailTable Data = new AMotivationDetailTable();
            FillDataSet.Tables.Add(Data);
            LoadViaSGroup(FillDataSet, AGroupId, AUnitKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaSGroup(String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            return LoadViaSGroup(AGroupId, AUnitKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaSGroup(String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroup(AGroupId, AUnitKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet ADataSet, SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_motivation_detail", AFieldList, AMotivationDetailTable.TableId) +
                            " FROM PUB_a_motivation_detail, PUB_s_group_motivation, PUB_s_group WHERE " +
                            "PUB_s_group_motivation.a_ledger_number_i = PUB_a_motivation_detail.a_ledger_number_i AND PUB_s_group_motivation.a_motivation_group_code_c = PUB_a_motivation_detail.a_motivation_group_code_c AND PUB_s_group_motivation.a_motivation_detail_code_c = PUB_a_motivation_detail.a_motivation_detail_code_c AND PUB_s_group_motivation.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_motivation.s_group_unit_key_n = PUB_s_group.s_unit_key_n") +
                            GenerateWhereClauseLong("PUB_s_group", SGroupTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AMotivationDetailTable.TableId), ATransaction,
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
        public static AMotivationDetailTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AMotivationDetailTable Data = new AMotivationDetailTable();
            FillDataSet.Tables.Add(Data);
            LoadViaSGroupTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_motivation_detail", AFieldList, AMotivationDetailTable.TableId) +
                            " FROM PUB_a_motivation_detail, PUB_s_group_motivation, PUB_s_group WHERE " +
                            "PUB_s_group_motivation.a_ledger_number_i = PUB_a_motivation_detail.a_ledger_number_i AND PUB_s_group_motivation.a_motivation_group_code_c = PUB_a_motivation_detail.a_motivation_group_code_c AND PUB_s_group_motivation.a_motivation_detail_code_c = PUB_a_motivation_detail.a_motivation_detail_code_c AND PUB_s_group_motivation.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_motivation.s_group_unit_key_n = PUB_s_group.s_unit_key_n") +
                            GenerateWhereClauseLong("PUB_s_group", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AMotivationDetailTable.TableId), ATransaction,
                            GetParametersForWhereClause(AMotivationDetailTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
        public static AMotivationDetailTable LoadViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AMotivationDetailTable Data = new AMotivationDetailTable();
            FillDataSet.Tables.Add(Data);
            LoadViaSGroupTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailTable LoadViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
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
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_detail, PUB_s_group_motivation WHERE " +
                        "PUB_s_group_motivation.a_ledger_number_i = PUB_a_motivation_detail.a_ledger_number_i AND PUB_s_group_motivation.a_motivation_group_code_c = PUB_a_motivation_detail.a_motivation_group_code_c AND PUB_s_group_motivation.a_motivation_detail_code_c = PUB_a_motivation_detail.a_motivation_detail_code_c AND PUB_s_group_motivation.s_group_id_c = ? AND PUB_s_group_motivation.s_group_unit_key_n = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_detail, PUB_s_group_motivation, PUB_s_group WHERE " +
                        "PUB_s_group_motivation.a_ledger_number_i = PUB_a_motivation_detail.a_ledger_number_i AND PUB_s_group_motivation.a_motivation_group_code_c = PUB_a_motivation_detail.a_motivation_group_code_c AND PUB_s_group_motivation.a_motivation_detail_code_c = PUB_a_motivation_detail.a_motivation_detail_code_c AND PUB_s_group_motivation.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_motivation.s_group_unit_key_n = PUB_s_group.s_unit_key_n" +
                        GenerateWhereClauseLong("PUB_s_group_motivation", AMotivationDetailTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(SGroupTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_detail, PUB_s_group_motivation, PUB_s_group WHERE " +
                        "PUB_s_group_motivation.a_ledger_number_i = PUB_a_motivation_detail.a_ledger_number_i AND PUB_s_group_motivation.a_motivation_group_code_c = PUB_a_motivation_detail.a_motivation_group_code_c AND PUB_s_group_motivation.a_motivation_detail_code_c = PUB_a_motivation_detail.a_motivation_detail_code_c AND PUB_s_group_motivation.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_motivation.s_group_unit_key_n = PUB_s_group.s_unit_key_n" +
                        GenerateWhereClauseLong("PUB_s_group_motivation", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(AMotivationDetailTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AMotivationDetailTable.TableId, new System.Object[3]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AMotivationDetailTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AMotivationDetailTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AMotivationDetailTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// motivation details can have several fees
    public class AMotivationDetailFeeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AMotivationDetailFee";

        /// original table name in database
        public const string DBTABLENAME = "a_motivation_detail_fee";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AMotivationDetailFeeTable.TableId) + " FROM PUB_a_motivation_detail_fee") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AMotivationDetailFeeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AMotivationDetailFeeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailFeeTable Data = new AMotivationDetailFeeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AMotivationDetailFeeTable.TableId) + " FROM PUB_a_motivation_detail_fee" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AMotivationDetailFeeTable.TableId, ADataSet, new System.Object[4]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailFeeTable Data = new AMotivationDetailFeeTable();
            LoadByPrimaryKey(AMotivationDetailFeeTable.TableId, Data, new System.Object[4]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AMotivationDetailFeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AMotivationDetailFeeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMotivationDetailFeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AMotivationDetailFeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadUsingTemplate(AMotivationDetailFeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailFeeTable Data = new AMotivationDetailFeeTable();
            LoadUsingTemplate(AMotivationDetailFeeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadUsingTemplate(AMotivationDetailFeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadUsingTemplate(AMotivationDetailFeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadUsingTemplate(AMotivationDetailFeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AMotivationDetailFeeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AMotivationDetailFeeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailFeeTable Data = new AMotivationDetailFeeTable();
            LoadUsingTemplate(AMotivationDetailFeeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_motivation_detail_fee", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, TDBTransaction ATransaction)
        {
            return Exists(AMotivationDetailFeeTable.TableId, new System.Object[4]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AMotivationDetailFeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_detail_fee" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AMotivationDetailFeeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AMotivationDetailFeeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_motivation_detail_fee" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AMotivationDetailFeeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AMotivationDetailFeeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailFeeTable.TableId, AMotivationDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                new System.Object[3]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailFeeTable Data = new AMotivationDetailFeeTable();
            LoadViaForeignKey(AMotivationDetailFeeTable.TableId, AMotivationDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                new System.Object[3]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetail(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetail(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet ADataSet, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailFeeTable.TableId, AMotivationDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailFeeTable Data = new AMotivationDetailFeeTable();
            LoadViaForeignKey(AMotivationDetailFeeTable.TableId, AMotivationDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AMotivationDetailFeeTable.TableId, AMotivationDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadViaAMotivationDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AMotivationDetailFeeTable Data = new AMotivationDetailFeeTable();
            LoadViaForeignKey(AMotivationDetailFeeTable.TableId, AMotivationDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadViaAMotivationDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AMotivationDetailFeeTable LoadViaAMotivationDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailFeeTable.TableId, AMotivationDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                new System.Object[3]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailFeeTable.TableId, AMotivationDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAMotivationDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AMotivationDetailFeeTable.TableId, AMotivationDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, String AFeeCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AMotivationDetailFeeTable.TableId, new System.Object[4]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFeeCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AMotivationDetailFeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AMotivationDetailFeeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AMotivationDetailFeeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AMotivationDetailFeeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Stores administrative fees and grants which have been calculated on gifts.
    public class AProcessedFeeAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AProcessedFee";

        /// original table name in database
        public const string DBTABLENAME = "a_processed_fee";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AProcessedFeeTable.TableId) + " FROM PUB_a_processed_fee") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AProcessedFeeTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AProcessedFeeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AProcessedFeeTable.TableId) + " FROM PUB_a_processed_fee" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, String AFeeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AProcessedFeeTable.TableId, ADataSet, new System.Object[5]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFeeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, String AFeeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFeeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, String AFeeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFeeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, String AFeeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadByPrimaryKey(AProcessedFeeTable.TableId, Data, new System.Object[5]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFeeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, String AFeeCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFeeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, String AFeeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFeeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AProcessedFeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AProcessedFeeTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AProcessedFeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AProcessedFeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadUsingTemplate(AProcessedFeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadUsingTemplate(AProcessedFeeTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadUsingTemplate(AProcessedFeeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadUsingTemplate(AProcessedFeeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadUsingTemplate(AProcessedFeeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AProcessedFeeTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AProcessedFeeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadUsingTemplate(AProcessedFeeTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_processed_fee", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, String AFeeCode, TDBTransaction ATransaction)
        {
            return Exists(AProcessedFeeTable.TableId, new System.Object[5]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFeeCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AProcessedFeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_processed_fee" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AProcessedFeeTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AProcessedFeeTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_processed_fee" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AProcessedFeeTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AProcessedFeeTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet ADataSet, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AProcessedFeeTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadViaForeignKey(AProcessedFeeTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            return LoadViaACostCentre(ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentre(ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AProcessedFeeTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadViaForeignKey(AProcessedFeeTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AProcessedFeeTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadViaForeignKey(AProcessedFeeTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AProcessedFeeTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AProcessedFeeTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AProcessedFeeTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AProcessedFeeTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadViaForeignKey(AProcessedFeeTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AProcessedFeeTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadViaForeignKey(AProcessedFeeTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AProcessedFeeTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadViaForeignKey(AProcessedFeeTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AProcessedFeeTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AProcessedFeeTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AProcessedFeeTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAAccountingPeriod(DataSet ADataSet, Int32 ALedgerNumber, Int32 AAccountingPeriodNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AProcessedFeeTable.TableId, AAccountingPeriodTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_period_number_i"},
                new System.Object[2]{ALedgerNumber, AAccountingPeriodNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountingPeriod(DataSet AData, Int32 ALedgerNumber, Int32 AAccountingPeriodNumber, TDBTransaction ATransaction)
        {
            LoadViaAAccountingPeriod(AData, ALedgerNumber, AAccountingPeriodNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountingPeriod(DataSet AData, Int32 ALedgerNumber, Int32 AAccountingPeriodNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountingPeriod(AData, ALedgerNumber, AAccountingPeriodNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAAccountingPeriod(Int32 ALedgerNumber, Int32 AAccountingPeriodNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadViaForeignKey(AProcessedFeeTable.TableId, AAccountingPeriodTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_period_number_i"},
                new System.Object[2]{ALedgerNumber, AAccountingPeriodNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAAccountingPeriod(Int32 ALedgerNumber, Int32 AAccountingPeriodNumber, TDBTransaction ATransaction)
        {
            return LoadViaAAccountingPeriod(ALedgerNumber, AAccountingPeriodNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAAccountingPeriod(Int32 ALedgerNumber, Int32 AAccountingPeriodNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountingPeriod(ALedgerNumber, AAccountingPeriodNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountingPeriodTemplate(DataSet ADataSet, AAccountingPeriodRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AProcessedFeeTable.TableId, AAccountingPeriodTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_period_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountingPeriodTemplate(DataSet AData, AAccountingPeriodRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountingPeriodTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountingPeriodTemplate(DataSet AData, AAccountingPeriodRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountingPeriodTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAAccountingPeriodTemplate(AAccountingPeriodRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadViaForeignKey(AProcessedFeeTable.TableId, AAccountingPeriodTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_period_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAAccountingPeriodTemplate(AAccountingPeriodRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAAccountingPeriodTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAAccountingPeriodTemplate(AAccountingPeriodRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountingPeriodTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAAccountingPeriodTemplate(AAccountingPeriodRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountingPeriodTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountingPeriodTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AProcessedFeeTable.TableId, AAccountingPeriodTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_period_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountingPeriodTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAAccountingPeriodTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountingPeriodTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountingPeriodTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAAccountingPeriodTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadViaForeignKey(AProcessedFeeTable.TableId, AAccountingPeriodTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_period_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAAccountingPeriodTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAAccountingPeriodTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAAccountingPeriodTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountingPeriodTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAAccountingPeriod(Int32 ALedgerNumber, Int32 AAccountingPeriodNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AProcessedFeeTable.TableId, AAccountingPeriodTable.TableId, new string[2]{"a_ledger_number_i", "a_period_number_i"},
                new System.Object[2]{ALedgerNumber, AAccountingPeriodNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountingPeriodTemplate(AAccountingPeriodRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AProcessedFeeTable.TableId, AAccountingPeriodTable.TableId, new string[2]{"a_ledger_number_i", "a_period_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountingPeriodTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AProcessedFeeTable.TableId, AAccountingPeriodTable.TableId, new string[2]{"a_ledger_number_i", "a_period_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAGiftDetail(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AProcessedFeeTable.TableId, AGiftDetailTable.TableId, ADataSet, new string[4]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i", "a_detail_number_i"},
                new System.Object[4]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAGiftDetail(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadViaAGiftDetail(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftDetail(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftDetail(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAGiftDetail(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadViaForeignKey(AProcessedFeeTable.TableId, AGiftDetailTable.TableId, Data, new string[4]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i", "a_detail_number_i"},
                new System.Object[4]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAGiftDetail(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return LoadViaAGiftDetail(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAGiftDetail(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAGiftDetail(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftDetailTemplate(DataSet ADataSet, AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AProcessedFeeTable.TableId, AGiftDetailTable.TableId, ADataSet, new string[4]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i", "a_detail_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAGiftDetailTemplate(DataSet AData, AGiftDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAGiftDetailTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftDetailTemplate(DataSet AData, AGiftDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftDetailTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAGiftDetailTemplate(AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadViaForeignKey(AProcessedFeeTable.TableId, AGiftDetailTable.TableId, Data, new string[4]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i", "a_detail_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAGiftDetailTemplate(AGiftDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAGiftDetailTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAGiftDetailTemplate(AGiftDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAGiftDetailTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAGiftDetailTemplate(AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAGiftDetailTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftDetailTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AProcessedFeeTable.TableId, AGiftDetailTable.TableId, ADataSet, new string[4]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i", "a_detail_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAGiftDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAGiftDetailTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftDetailTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAGiftDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AProcessedFeeTable Data = new AProcessedFeeTable();
            LoadViaForeignKey(AProcessedFeeTable.TableId, AGiftDetailTable.TableId, Data, new string[4]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i", "a_detail_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAGiftDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAGiftDetailTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AProcessedFeeTable LoadViaAGiftDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAGiftDetailTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAGiftDetail(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AProcessedFeeTable.TableId, AGiftDetailTable.TableId, new string[4]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i", "a_detail_number_i"},
                new System.Object[4]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaAGiftDetailTemplate(AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AProcessedFeeTable.TableId, AGiftDetailTable.TableId, new string[4]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i", "a_detail_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAGiftDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AProcessedFeeTable.TableId, AGiftDetailTable.TableId, new string[4]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i", "a_detail_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, String AFeeCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AProcessedFeeTable.TableId, new System.Object[5]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFeeCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AProcessedFeeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AProcessedFeeTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AProcessedFeeTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AProcessedFeeTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Templates of gift batches which can be copied into the gift system.
    public class ARecurringGiftBatchAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "ARecurringGiftBatch";

        /// original table name in database
        public const string DBTABLENAME = "a_recurring_gift_batch";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, ARecurringGiftBatchTable.TableId) + " FROM PUB_a_recurring_gift_batch") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ARecurringGiftBatchTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static ARecurringGiftBatchTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, ARecurringGiftBatchTable.TableId) + " FROM PUB_a_recurring_gift_batch" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(ARecurringGiftBatchTable.TableId, ADataSet, new System.Object[2]{ALedgerNumber, ABatchNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadByPrimaryKey(ARecurringGiftBatchTable.TableId, Data, new System.Object[2]{ALedgerNumber, ABatchNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ARecurringGiftBatchTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ARecurringGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ARecurringGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadUsingTemplate(ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadUsingTemplate(ARecurringGiftBatchTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadUsingTemplate(ARecurringGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadUsingTemplate(ARecurringGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadUsingTemplate(ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ARecurringGiftBatchTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static ARecurringGiftBatchTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadUsingTemplate(ARecurringGiftBatchTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            return Exists(ARecurringGiftBatchTable.TableId, new System.Object[2]{ALedgerNumber, ABatchNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ARecurringGiftBatchTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(ARecurringGiftBatchTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift_batch" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ARecurringGiftBatchTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(ARecurringGiftBatchTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftBatchTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftBatchTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftBatchTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet ADataSet, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftBatchTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftBatchTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftBatchTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet ADataSet, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            return LoadViaACostCentre(ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentre(ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftBatchTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftBatchTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftBatchTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static ARecurringGiftBatchTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static ARecurringGiftBatchTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static ARecurringGiftBatchTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftBatchTable Data = new ARecurringGiftBatchTable();
            LoadViaForeignKey(ARecurringGiftBatchTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftBatchTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftBatchTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftBatchTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftBatchTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(ARecurringGiftBatchTable.TableId, new System.Object[2]{ALedgerNumber, ABatchNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ARecurringGiftBatchTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ARecurringGiftBatchTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(ARecurringGiftBatchTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Templates of donor gift information which can be copied into the gift system with recurring gift batches.
    public class ARecurringGiftAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "ARecurringGift";

        /// original table name in database
        public const string DBTABLENAME = "a_recurring_gift";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, ARecurringGiftTable.TableId) + " FROM PUB_a_recurring_gift") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ARecurringGiftTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static ARecurringGiftTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, ARecurringGiftTable.TableId) + " FROM PUB_a_recurring_gift" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(ARecurringGiftTable.TableId, ADataSet, new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadByPrimaryKey(ARecurringGiftTable.TableId, Data, new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ARecurringGiftTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ARecurringGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ARecurringGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadUsingTemplate(ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadUsingTemplate(ARecurringGiftTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadUsingTemplate(ARecurringGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadUsingTemplate(ARecurringGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadUsingTemplate(ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ARecurringGiftTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static ARecurringGiftTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadUsingTemplate(ARecurringGiftTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            return Exists(ARecurringGiftTable.TableId, new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ARecurringGiftTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(ARecurringGiftTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ARecurringGiftTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(ARecurringGiftTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaARecurringGiftBatch(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, ARecurringGiftBatchTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                new System.Object[2]{ALedgerNumber, ABatchNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaARecurringGiftBatch(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatch(AData, ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaARecurringGiftBatch(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatch(AData, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaARecurringGiftBatch(Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, ARecurringGiftBatchTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                new System.Object[2]{ALedgerNumber, ABatchNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaARecurringGiftBatch(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGiftBatch(ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaARecurringGiftBatch(Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGiftBatch(ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaARecurringGiftBatchTemplate(DataSet ADataSet, ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, ARecurringGiftBatchTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaARecurringGiftBatchTemplate(DataSet AData, ARecurringGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatchTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaARecurringGiftBatchTemplate(DataSet AData, ARecurringGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatchTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaARecurringGiftBatchTemplate(ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, ARecurringGiftBatchTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaARecurringGiftBatchTemplate(ARecurringGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGiftBatchTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaARecurringGiftBatchTemplate(ARecurringGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGiftBatchTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaARecurringGiftBatchTemplate(ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGiftBatchTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaARecurringGiftBatchTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, ARecurringGiftBatchTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaARecurringGiftBatchTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatchTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaARecurringGiftBatchTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftBatchTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaARecurringGiftBatchTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, ARecurringGiftBatchTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaARecurringGiftBatchTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGiftBatchTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaARecurringGiftBatchTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGiftBatchTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaARecurringGiftBatch(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, ARecurringGiftBatchTable.TableId, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                new System.Object[2]{ALedgerNumber, ABatchNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaARecurringGiftBatchTemplate(ARecurringGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, ARecurringGiftBatchTable.TableId, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaARecurringGiftBatchTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, ARecurringGiftBatchTable.TableId, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAMethodOfGiving(DataSet ADataSet, String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, AMethodOfGivingTable.TableId, ADataSet, new string[1]{"a_method_of_giving_code_c"},
                new System.Object[1]{AMethodOfGivingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMethodOfGiving(DataSet AData, String AMethodOfGivingCode, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGiving(AData, AMethodOfGivingCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfGiving(DataSet AData, String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGiving(AData, AMethodOfGivingCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfGiving(String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, AMethodOfGivingTable.TableId, Data, new string[1]{"a_method_of_giving_code_c"},
                new System.Object[1]{AMethodOfGivingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfGiving(String AMethodOfGivingCode, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGiving(AMethodOfGivingCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfGiving(String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGiving(AMethodOfGivingCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet ADataSet, AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, AMethodOfGivingTable.TableId, ADataSet, new string[1]{"a_method_of_giving_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet AData, AMethodOfGivingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet AData, AMethodOfGivingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfGivingTemplate(AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, AMethodOfGivingTable.TableId, Data, new string[1]{"a_method_of_giving_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfGivingTemplate(AMethodOfGivingRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGivingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfGivingTemplate(AMethodOfGivingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGivingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfGivingTemplate(AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGivingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, AMethodOfGivingTable.TableId, ADataSet, new string[1]{"a_method_of_giving_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfGivingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, AMethodOfGivingTable.TableId, Data, new string[1]{"a_method_of_giving_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfGivingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGivingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfGivingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGivingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAMethodOfGiving(String AMethodOfGivingCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, AMethodOfGivingTable.TableId, new string[1]{"a_method_of_giving_code_c"},
                new System.Object[1]{AMethodOfGivingCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAMethodOfGivingTemplate(AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, AMethodOfGivingTable.TableId, new string[1]{"a_method_of_giving_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAMethodOfGivingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, AMethodOfGivingTable.TableId, new string[1]{"a_method_of_giving_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAMethodOfPayment(DataSet ADataSet, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, AMethodOfPaymentTable.TableId, ADataSet, new string[1]{"a_method_of_payment_code_c"},
                new System.Object[1]{AMethodOfPaymentCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMethodOfPayment(DataSet AData, String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPayment(AData, AMethodOfPaymentCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfPayment(DataSet AData, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPayment(AData, AMethodOfPaymentCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfPayment(String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, AMethodOfPaymentTable.TableId, Data, new string[1]{"a_method_of_payment_code_c"},
                new System.Object[1]{AMethodOfPaymentCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfPayment(String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPayment(AMethodOfPaymentCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfPayment(String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPayment(AMethodOfPaymentCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet ADataSet, AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, AMethodOfPaymentTable.TableId, ADataSet, new string[1]{"a_method_of_payment_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet AData, AMethodOfPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet AData, AMethodOfPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfPaymentTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, AMethodOfPaymentTable.TableId, Data, new string[1]{"a_method_of_payment_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfPaymentTemplate(AMethodOfPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPaymentTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfPaymentTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPaymentTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfPaymentTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPaymentTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, AMethodOfPaymentTable.TableId, ADataSet, new string[1]{"a_method_of_payment_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfPaymentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, AMethodOfPaymentTable.TableId, Data, new string[1]{"a_method_of_payment_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfPaymentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPaymentTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaAMethodOfPaymentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPaymentTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAMethodOfPayment(String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, AMethodOfPaymentTable.TableId, new string[1]{"a_method_of_payment_code_c"},
                new System.Object[1]{AMethodOfPaymentCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAMethodOfPaymentTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, AMethodOfPaymentTable.TableId, new string[1]{"a_method_of_payment_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAMethodOfPaymentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, AMethodOfPaymentTable.TableId, new string[1]{"a_method_of_payment_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_donor_key_n"},
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
        public static ARecurringGiftTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_donor_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_donor_key_n"},
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
        public static ARecurringGiftTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_donor_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_donor_key_n"},
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
        public static ARecurringGiftTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftTable Data = new ARecurringGiftTable();
            LoadViaForeignKey(ARecurringGiftTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_donor_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, PPartnerTable.TableId, new string[1]{"p_donor_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, PPartnerTable.TableId, new string[1]{"p_donor_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftTable.TableId, PPartnerTable.TableId, new string[1]{"p_donor_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(ARecurringGiftTable.TableId, new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ARecurringGiftTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ARecurringGiftTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(ARecurringGiftTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Store recipient information for the recurring gift.
    public class ARecurringGiftDetailAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "ARecurringGiftDetail";

        /// original table name in database
        public const string DBTABLENAME = "a_recurring_gift_detail";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, ARecurringGiftDetailTable.TableId) + " FROM PUB_a_recurring_gift_detail") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ARecurringGiftDetailTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static ARecurringGiftDetailTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, ARecurringGiftDetailTable.TableId) + " FROM PUB_a_recurring_gift_detail" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(ARecurringGiftDetailTable.TableId, ADataSet, new System.Object[4]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadByPrimaryKey(ARecurringGiftDetailTable.TableId, Data, new System.Object[4]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, ARecurringGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ARecurringGiftDetailTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ARecurringGiftDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ARecurringGiftDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadUsingTemplate(ARecurringGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadUsingTemplate(ARecurringGiftDetailTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadUsingTemplate(ARecurringGiftDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadUsingTemplate(ARecurringGiftDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadUsingTemplate(ARecurringGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ARecurringGiftDetailTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static ARecurringGiftDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadUsingTemplate(ARecurringGiftDetailTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return Exists(ARecurringGiftDetailTable.TableId, new System.Object[4]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(ARecurringGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ARecurringGiftDetailTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(ARecurringGiftDetailTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_recurring_gift_detail" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ARecurringGiftDetailTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(ARecurringGiftDetailTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaARecurringGift(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, ARecurringGiftTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaARecurringGift(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            LoadViaARecurringGift(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaARecurringGift(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGift(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaARecurringGift(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, ARecurringGiftTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaARecurringGift(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGift(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaARecurringGift(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGift(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaARecurringGiftTemplate(DataSet ADataSet, ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, ARecurringGiftTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaARecurringGiftTemplate(DataSet AData, ARecurringGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaARecurringGiftTemplate(DataSet AData, ARecurringGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaARecurringGiftTemplate(ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, ARecurringGiftTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaARecurringGiftTemplate(ARecurringGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGiftTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaARecurringGiftTemplate(ARecurringGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGiftTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaARecurringGiftTemplate(ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGiftTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaARecurringGiftTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, ARecurringGiftTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaARecurringGiftTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaARecurringGiftTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaARecurringGiftTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaARecurringGiftTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, ARecurringGiftTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaARecurringGiftTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGiftTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaARecurringGiftTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaARecurringGiftTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaARecurringGift(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, ARecurringGiftTable.TableId, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaARecurringGiftTemplate(ARecurringGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, ARecurringGiftTable.TableId, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaARecurringGiftTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, ARecurringGiftTable.TableId, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, AMotivationDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                new System.Object[3]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, AMotivationDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                new System.Object[3]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetail(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetail(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet ADataSet, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, AMotivationDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, AMotivationDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, AMotivationDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaAMotivationDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, AMotivationDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaAMotivationDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaAMotivationDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, AMotivationDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                new System.Object[3]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, AMotivationDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAMotivationDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, AMotivationDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKey(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_recipient_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKey(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKey(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKey(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKey(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientKey(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_recipient_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKey(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientKey(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKey(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_recipient_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_recipient_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKeyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKeyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKeyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_recipient_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_recipient_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKeyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKeyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartnerRecipientKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, new string[1]{"p_recipient_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerRecipientKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, new string[1]{"p_recipient_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerRecipientKeyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, new string[1]{"p_recipient_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPMailing(DataSet ADataSet, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PMailingTable.TableId, ADataSet, new string[1]{"p_mailing_code_c"},
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
        public static ARecurringGiftDetailTable LoadViaPMailing(String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PMailingTable.TableId, Data, new string[1]{"p_mailing_code_c"},
                new System.Object[1]{AMailingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPMailing(String AMailingCode, TDBTransaction ATransaction)
        {
            return LoadViaPMailing(AMailingCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPMailing(String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMailing(AMailingCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet ADataSet, PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PMailingTable.TableId, ADataSet, new string[1]{"p_mailing_code_c"},
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
        public static ARecurringGiftDetailTable LoadViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PMailingTable.TableId, Data, new string[1]{"p_mailing_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPMailingTemplate(PMailingRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PMailingTable.TableId, ADataSet, new string[1]{"p_mailing_code_c"},
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
        public static ARecurringGiftDetailTable LoadViaPMailingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PMailingTable.TableId, Data, new string[1]{"p_mailing_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPMailingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPMailingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPMailing(String AMailingCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, PMailingTable.TableId, new string[1]{"p_mailing_code_c"},
                new System.Object[1]{AMailingCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, PMailingTable.TableId, new string[1]{"p_mailing_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPMailingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, PMailingTable.TableId, new string[1]{"p_mailing_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"a_recipient_ledger_number_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumber(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumber(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientLedgerNumber(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"a_recipient_ledger_number_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientLedgerNumber(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumber(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientLedgerNumber(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumber(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"a_recipient_ledger_number_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"a_recipient_ledger_number_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumberTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumberTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumberTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"a_recipient_ledger_number_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ARecurringGiftDetailTable Data = new ARecurringGiftDetailTable();
            LoadViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"a_recipient_ledger_number_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumberTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ARecurringGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumberTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartnerRecipientLedgerNumber(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, new string[1]{"a_recipient_ledger_number_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerRecipientLedgerNumberTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, new string[1]{"a_recipient_ledger_number_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerRecipientLedgerNumberTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ARecurringGiftDetailTable.TableId, PPartnerTable.TableId, new string[1]{"a_recipient_ledger_number_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(ARecurringGiftDetailTable.TableId, new System.Object[4]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(ARecurringGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ARecurringGiftDetailTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ARecurringGiftDetailTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(ARecurringGiftDetailTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Information describing groups (batches) of gifts.
    public class AGiftBatchAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AGiftBatch";

        /// original table name in database
        public const string DBTABLENAME = "a_gift_batch";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AGiftBatchTable.TableId) + " FROM PUB_a_gift_batch") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AGiftBatchTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AGiftBatchTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AGiftBatchTable.TableId) + " FROM PUB_a_gift_batch" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AGiftBatchTable.TableId, ADataSet, new System.Object[2]{ALedgerNumber, ABatchNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadByPrimaryKey(AGiftBatchTable.TableId, Data, new System.Object[2]{ALedgerNumber, ABatchNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AGiftBatchTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadUsingTemplate(AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadUsingTemplate(AGiftBatchTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadUsingTemplate(AGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadUsingTemplate(AGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadUsingTemplate(AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AGiftBatchTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AGiftBatchTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadUsingTemplate(AGiftBatchTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_batch", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            return Exists(AGiftBatchTable.TableId, new System.Object[2]{ALedgerNumber, ABatchNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_batch" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AGiftBatchTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AGiftBatchTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_batch" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AGiftBatchTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AGiftBatchTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftBatchTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadViaForeignKey(AGiftBatchTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftBatchTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadViaForeignKey(AGiftBatchTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftBatchTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadViaForeignKey(AGiftBatchTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftBatchTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftBatchTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftBatchTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet ADataSet, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftBatchTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadViaForeignKey(AGiftBatchTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftBatchTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadViaForeignKey(AGiftBatchTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftBatchTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadViaForeignKey(AGiftBatchTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftBatchTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftBatchTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftBatchTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_account_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet ADataSet, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftBatchTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadViaForeignKey(AGiftBatchTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            return LoadViaACostCentre(ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentre(ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftBatchTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadViaForeignKey(AGiftBatchTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftBatchTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadViaForeignKey(AGiftBatchTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftBatchTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftBatchTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftBatchTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_cost_centre_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftBatchTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static AGiftBatchTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadViaForeignKey(AGiftBatchTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftBatchTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static AGiftBatchTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadViaForeignKey(AGiftBatchTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftBatchTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static AGiftBatchTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftBatchTable Data = new AGiftBatchTable();
            LoadViaForeignKey(AGiftBatchTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftBatchTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftBatchTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftBatchTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftBatchTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AGiftBatchTable.TableId, new System.Object[2]{ALedgerNumber, ABatchNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AGiftBatchTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AGiftBatchTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AGiftBatchTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Information on the donor's giving. Points to the gift detail records.
    public class AGiftAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AGift";

        /// original table name in database
        public const string DBTABLENAME = "a_gift";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AGiftTable.TableId) + " FROM PUB_a_gift") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AGiftTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AGiftTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AGiftTable.TableId) + " FROM PUB_a_gift" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AGiftTable.TableId, ADataSet, new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadByPrimaryKey(AGiftTable.TableId, Data, new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AGiftTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadUsingTemplate(AGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadUsingTemplate(AGiftTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadUsingTemplate(AGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadUsingTemplate(AGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadUsingTemplate(AGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AGiftTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AGiftTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadUsingTemplate(AGiftTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            return Exists(AGiftTable.TableId, new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AGiftTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AGiftTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AGiftTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AGiftTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAGiftBatch(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, AGiftBatchTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                new System.Object[2]{ALedgerNumber, ABatchNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAGiftBatch(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatch(AData, ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftBatch(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatch(AData, ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAGiftBatch(Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, AGiftBatchTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                new System.Object[2]{ALedgerNumber, ABatchNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaAGiftBatch(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            return LoadViaAGiftBatch(ALedgerNumber, ABatchNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAGiftBatch(Int32 ALedgerNumber, Int32 ABatchNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAGiftBatch(ALedgerNumber, ABatchNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftBatchTemplate(DataSet ADataSet, AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, AGiftBatchTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAGiftBatchTemplate(DataSet AData, AGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatchTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftBatchTemplate(DataSet AData, AGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatchTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAGiftBatchTemplate(AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, AGiftBatchTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaAGiftBatchTemplate(AGiftBatchRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAGiftBatchTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAGiftBatchTemplate(AGiftBatchRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAGiftBatchTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAGiftBatchTemplate(AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAGiftBatchTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftBatchTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, AGiftBatchTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAGiftBatchTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatchTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftBatchTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftBatchTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAGiftBatchTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, AGiftBatchTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaAGiftBatchTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAGiftBatchTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAGiftBatchTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAGiftBatchTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAGiftBatch(Int32 ALedgerNumber, Int32 ABatchNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, AGiftBatchTable.TableId, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                new System.Object[2]{ALedgerNumber, ABatchNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaAGiftBatchTemplate(AGiftBatchRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, AGiftBatchTable.TableId, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAGiftBatchTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, AGiftBatchTable.TableId, new string[2]{"a_ledger_number_i", "a_batch_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAMethodOfGiving(DataSet ADataSet, String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, AMethodOfGivingTable.TableId, ADataSet, new string[1]{"a_method_of_giving_code_c"},
                new System.Object[1]{AMethodOfGivingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMethodOfGiving(DataSet AData, String AMethodOfGivingCode, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGiving(AData, AMethodOfGivingCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfGiving(DataSet AData, String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGiving(AData, AMethodOfGivingCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfGiving(String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, AMethodOfGivingTable.TableId, Data, new string[1]{"a_method_of_giving_code_c"},
                new System.Object[1]{AMethodOfGivingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfGiving(String AMethodOfGivingCode, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGiving(AMethodOfGivingCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfGiving(String AMethodOfGivingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGiving(AMethodOfGivingCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet ADataSet, AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, AMethodOfGivingTable.TableId, ADataSet, new string[1]{"a_method_of_giving_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet AData, AMethodOfGivingRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet AData, AMethodOfGivingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfGivingTemplate(AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, AMethodOfGivingTable.TableId, Data, new string[1]{"a_method_of_giving_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfGivingTemplate(AMethodOfGivingRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGivingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfGivingTemplate(AMethodOfGivingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGivingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfGivingTemplate(AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGivingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, AMethodOfGivingTable.TableId, ADataSet, new string[1]{"a_method_of_giving_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfGivingTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfGivingTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfGivingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, AMethodOfGivingTable.TableId, Data, new string[1]{"a_method_of_giving_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfGivingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGivingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfGivingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfGivingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAMethodOfGiving(String AMethodOfGivingCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, AMethodOfGivingTable.TableId, new string[1]{"a_method_of_giving_code_c"},
                new System.Object[1]{AMethodOfGivingCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAMethodOfGivingTemplate(AMethodOfGivingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, AMethodOfGivingTable.TableId, new string[1]{"a_method_of_giving_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAMethodOfGivingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, AMethodOfGivingTable.TableId, new string[1]{"a_method_of_giving_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAMethodOfPayment(DataSet ADataSet, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, AMethodOfPaymentTable.TableId, ADataSet, new string[1]{"a_method_of_payment_code_c"},
                new System.Object[1]{AMethodOfPaymentCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMethodOfPayment(DataSet AData, String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPayment(AData, AMethodOfPaymentCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfPayment(DataSet AData, String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPayment(AData, AMethodOfPaymentCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfPayment(String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, AMethodOfPaymentTable.TableId, Data, new string[1]{"a_method_of_payment_code_c"},
                new System.Object[1]{AMethodOfPaymentCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfPayment(String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPayment(AMethodOfPaymentCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfPayment(String AMethodOfPaymentCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPayment(AMethodOfPaymentCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet ADataSet, AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, AMethodOfPaymentTable.TableId, ADataSet, new string[1]{"a_method_of_payment_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet AData, AMethodOfPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet AData, AMethodOfPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfPaymentTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, AMethodOfPaymentTable.TableId, Data, new string[1]{"a_method_of_payment_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfPaymentTemplate(AMethodOfPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPaymentTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfPaymentTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPaymentTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfPaymentTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPaymentTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, AMethodOfPaymentTable.TableId, ADataSet, new string[1]{"a_method_of_payment_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMethodOfPaymentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMethodOfPaymentTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfPaymentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, AMethodOfPaymentTable.TableId, Data, new string[1]{"a_method_of_payment_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfPaymentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPaymentTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaAMethodOfPaymentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMethodOfPaymentTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAMethodOfPayment(String AMethodOfPaymentCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, AMethodOfPaymentTable.TableId, new string[1]{"a_method_of_payment_code_c"},
                new System.Object[1]{AMethodOfPaymentCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAMethodOfPaymentTemplate(AMethodOfPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, AMethodOfPaymentTable.TableId, new string[1]{"a_method_of_payment_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAMethodOfPaymentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, AMethodOfPaymentTable.TableId, new string[1]{"a_method_of_payment_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_donor_key_n"},
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
        public static AGiftTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_donor_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_donor_key_n"},
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
        public static AGiftTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_donor_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_donor_key_n"},
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
        public static AGiftTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftTable Data = new AGiftTable();
            LoadViaForeignKey(AGiftTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_donor_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, PPartnerTable.TableId, new string[1]{"p_donor_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, PPartnerTable.TableId, new string[1]{"p_donor_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftTable.TableId, PPartnerTable.TableId, new string[1]{"p_donor_key_n"},
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
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_a_gift", AFieldList, AGiftTable.TableId) +
                            " FROM PUB_a_gift, PUB_s_group_gift WHERE " +
                            "PUB_s_group_gift.a_ledger_number_i = PUB_a_gift.a_ledger_number_i AND PUB_s_group_gift.a_batch_number_i = PUB_a_gift.a_batch_number_i AND PUB_s_group_gift.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i AND PUB_s_group_gift.s_group_id_c = ? AND PUB_s_group_gift.s_group_unit_key_n = ?") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AGiftTable.TableId), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
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
        public static AGiftTable LoadViaSGroup(String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AGiftTable Data = new AGiftTable();
            FillDataSet.Tables.Add(Data);
            LoadViaSGroup(FillDataSet, AGroupId, AUnitKey, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaSGroup(String AGroupId, Int64 AUnitKey, TDBTransaction ATransaction)
        {
            return LoadViaSGroup(AGroupId, AUnitKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaSGroup(String AGroupId, Int64 AUnitKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroup(AGroupId, AUnitKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet ADataSet, SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift", AFieldList, AGiftTable.TableId) +
                            " FROM PUB_a_gift, PUB_s_group_gift, PUB_s_group WHERE " +
                            "PUB_s_group_gift.a_ledger_number_i = PUB_a_gift.a_ledger_number_i AND PUB_s_group_gift.a_batch_number_i = PUB_a_gift.a_batch_number_i AND PUB_s_group_gift.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i AND PUB_s_group_gift.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_gift.s_group_unit_key_n = PUB_s_group.s_unit_key_n") +
                            GenerateWhereClauseLong("PUB_s_group", SGroupTable.TableId, ATemplateRow, ATemplateOperators)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AGiftTable.TableId), ATransaction,
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
        public static AGiftTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AGiftTable Data = new AGiftTable();
            FillDataSet.Tables.Add(Data);
            LoadViaSGroupTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSGroupTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_a_gift", AFieldList, AGiftTable.TableId) +
                            " FROM PUB_a_gift, PUB_s_group_gift, PUB_s_group WHERE " +
                            "PUB_s_group_gift.a_ledger_number_i = PUB_a_gift.a_ledger_number_i AND PUB_s_group_gift.a_batch_number_i = PUB_a_gift.a_batch_number_i AND PUB_s_group_gift.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i AND PUB_s_group_gift.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_gift.s_group_unit_key_n = PUB_s_group.s_unit_key_n") +
                            GenerateWhereClauseLong("PUB_s_group", ASearchCriteria)) +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AGiftTable.TableId), ATransaction,
                            GetParametersForWhereClause(AGiftTable.TableId, ASearchCriteria), AStartRecord, AMaxRecords);
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
        public static AGiftTable LoadViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DataSet FillDataSet = new DataSet();
            AGiftTable Data = new AGiftTable();
            FillDataSet.Tables.Add(Data);
            LoadViaSGroupTemplate(FillDataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            FillDataSet.Tables.Remove(Data);
            return Data;
        }

        /// auto generated
        public static AGiftTable LoadViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaSGroupTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftTable LoadViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
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
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift, PUB_s_group_gift WHERE " +
                        "PUB_s_group_gift.a_ledger_number_i = PUB_a_gift.a_ledger_number_i AND PUB_s_group_gift.a_batch_number_i = PUB_a_gift.a_batch_number_i AND PUB_s_group_gift.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i AND PUB_s_group_gift.s_group_id_c = ? AND PUB_s_group_gift.s_group_unit_key_n = ?",
                        ATransaction, false, ParametersArray));
        }

        /// auto generated
        public static int CountViaSGroupTemplate(SGroupRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift, PUB_s_group_gift, PUB_s_group WHERE " +
                        "PUB_s_group_gift.a_ledger_number_i = PUB_a_gift.a_ledger_number_i AND PUB_s_group_gift.a_batch_number_i = PUB_a_gift.a_batch_number_i AND PUB_s_group_gift.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i AND PUB_s_group_gift.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_gift.s_group_unit_key_n = PUB_s_group.s_unit_key_n" +
                        GenerateWhereClauseLong("PUB_s_group_gift", AGiftTable.TableId, ATemplateRow, ATemplateOperators)), ATransaction, false,
                        GetParametersForWhereClauseWithPrimaryKey(SGroupTable.TableId, ATemplateRow)));
        }

        /// auto generated
        public static int CountViaSGroupTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift, PUB_s_group_gift, PUB_s_group WHERE " +
                        "PUB_s_group_gift.a_ledger_number_i = PUB_a_gift.a_ledger_number_i AND PUB_s_group_gift.a_batch_number_i = PUB_a_gift.a_batch_number_i AND PUB_s_group_gift.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i AND PUB_s_group_gift.s_group_id_c = PUB_s_group.s_group_id_c AND PUB_s_group_gift.s_group_unit_key_n = PUB_s_group.s_unit_key_n" +
                        GenerateWhereClauseLong("PUB_s_group_gift", ASearchCriteria)), ATransaction, false,
                        GetParametersForWhereClause(AGiftTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AGiftTable.TableId, new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AGiftTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AGiftTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AGiftTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// The gift recipient information for a gift.  A single gift can be split among more than one recipient.  A gift detail record is created for each recipient.
    public class AGiftDetailAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AGiftDetail";

        /// original table name in database
        public const string DBTABLENAME = "a_gift_detail";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AGiftDetailTable.TableId) + " FROM PUB_a_gift_detail") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AGiftDetailTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AGiftDetailTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AGiftDetailTable.TableId) + " FROM PUB_a_gift_detail" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AGiftDetailTable.TableId, ADataSet, new System.Object[4]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadByPrimaryKey(AGiftDetailTable.TableId, Data, new System.Object[4]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AGiftDetailTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AGiftDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AGiftDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadUsingTemplate(AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadUsingTemplate(AGiftDetailTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadUsingTemplate(AGiftDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadUsingTemplate(AGiftDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadUsingTemplate(AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AGiftDetailTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AGiftDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadUsingTemplate(AGiftDetailTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_gift_detail", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return Exists(AGiftDetailTable.TableId, new System.Object[4]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_detail" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AGiftDetailTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AGiftDetailTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_gift_detail" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AGiftDetailTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AGiftDetailTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAGift(DataSet ADataSet, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, AGiftTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAGift(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            LoadViaAGift(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGift(DataSet AData, Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGift(AData, ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAGift(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, AGiftTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAGift(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            return LoadViaAGift(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAGift(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAGift(ALedgerNumber, ABatchNumber, AGiftTransactionNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftTemplate(DataSet ADataSet, AGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, AGiftTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAGiftTemplate(DataSet AData, AGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAGiftTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftTemplate(DataSet AData, AGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAGiftTemplate(AGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, AGiftTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAGiftTemplate(AGiftRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAGiftTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAGiftTemplate(AGiftRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAGiftTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAGiftTemplate(AGiftRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAGiftTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, AGiftTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAGiftTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAGiftTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAGiftTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAGiftTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAGiftTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, AGiftTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAGiftTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAGiftTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAGiftTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAGiftTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAGift(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, AGiftTable.TableId, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                new System.Object[3]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaAGiftTemplate(AGiftRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, AGiftTable.TableId, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAGiftTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, AGiftTable.TableId, new string[3]{"a_ledger_number_i", "a_batch_number_i", "a_gift_transaction_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet ADataSet, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, AMotivationDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                new System.Object[3]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetail(DataSet AData, Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetail(AData, ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, AMotivationDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                new System.Object[3]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetail(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetail(ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet ADataSet, AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, AMotivationDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, AMotivationDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, AMotivationDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAMotivationDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAMotivationDetailTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAMotivationDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, AMotivationDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAMotivationDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaAMotivationDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAMotivationDetailTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAMotivationDetail(Int32 ALedgerNumber, String AMotivationGroupCode, String AMotivationDetailCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, AMotivationDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                new System.Object[3]{ALedgerNumber, AMotivationGroupCode, AMotivationDetailCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAMotivationDetailTemplate(AMotivationDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, AMotivationDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAMotivationDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, AMotivationDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_motivation_group_code_c", "a_motivation_detail_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKey(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_recipient_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKey(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKey(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKey(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKey(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientKey(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_recipient_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKey(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientKey(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKey(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_recipient_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_recipient_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKeyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKeyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKeyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_recipient_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientKeyTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientKeyTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_recipient_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKeyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientKeyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientKeyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartnerRecipientKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, new string[1]{"p_recipient_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerRecipientKeyTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, new string[1]{"p_recipient_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerRecipientKeyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, new string[1]{"p_recipient_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPMailing(DataSet ADataSet, String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, PMailingTable.TableId, ADataSet, new string[1]{"p_mailing_code_c"},
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
        public static AGiftDetailTable LoadViaPMailing(String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, PMailingTable.TableId, Data, new string[1]{"p_mailing_code_c"},
                new System.Object[1]{AMailingCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPMailing(String AMailingCode, TDBTransaction ATransaction)
        {
            return LoadViaPMailing(AMailingCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPMailing(String AMailingCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMailing(AMailingCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet ADataSet, PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, PMailingTable.TableId, ADataSet, new string[1]{"p_mailing_code_c"},
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
        public static AGiftDetailTable LoadViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, PMailingTable.TableId, Data, new string[1]{"p_mailing_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPMailingTemplate(PMailingRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPMailingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, PMailingTable.TableId, ADataSet, new string[1]{"p_mailing_code_c"},
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
        public static AGiftDetailTable LoadViaPMailingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, PMailingTable.TableId, Data, new string[1]{"p_mailing_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPMailingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPMailingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPMailingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPMailing(String AMailingCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, PMailingTable.TableId, new string[1]{"p_mailing_code_c"},
                new System.Object[1]{AMailingCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaPMailingTemplate(PMailingRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, PMailingTable.TableId, new string[1]{"p_mailing_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPMailingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, PMailingTable.TableId, new string[1]{"p_mailing_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"a_recipient_ledger_number_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumber(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumber(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumber(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientLedgerNumber(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"a_recipient_ledger_number_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientLedgerNumber(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumber(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientLedgerNumber(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumber(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"a_recipient_ledger_number_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"a_recipient_ledger_number_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumberTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumberTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumberTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"a_recipient_ledger_number_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerRecipientLedgerNumberTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerRecipientLedgerNumberTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, Data, new string[1]{"a_recipient_ledger_number_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumberTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaPPartnerRecipientLedgerNumberTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerRecipientLedgerNumberTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartnerRecipientLedgerNumber(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, new string[1]{"a_recipient_ledger_number_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerRecipientLedgerNumberTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, new string[1]{"a_recipient_ledger_number_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerRecipientLedgerNumberTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, PPartnerTable.TableId, new string[1]{"a_recipient_ledger_number_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet ADataSet, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            return LoadViaACostCentre(ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentre(ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AGiftDetailTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AGiftDetailTable Data = new AGiftDetailTable();
            LoadViaForeignKey(AGiftDetailTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AGiftDetailTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AGiftDetailTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AGiftDetailTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AGiftTransactionNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AGiftDetailTable.TableId, new System.Object[4]{ALedgerNumber, ABatchNumber, AGiftTransactionNumber, ADetailNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AGiftDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AGiftDetailTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AGiftDetailTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AGiftDetailTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }
}
