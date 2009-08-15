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
namespace Ict.Petra.Shared.MFinance.AP.Data.Access
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
    using Ict.Petra.Shared.MFinance.AP.Data;
    using Ict.Petra.Shared.MPartner.Partner.Data;
    using Ict.Petra.Shared.MCommon.Data;
    using Ict.Petra.Shared.MFinance.Account.Data;
    using Ict.Petra.Shared.MSysMan.Data;

    /// This table defines the concept of a supplier in the AP system and is the centre of the AP system.
    public class AApSupplierAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AApSupplier";

        /// original table name in database
        public const string DBTABLENAME = "a_ap_supplier";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AApSupplierTable.TableId) + " FROM PUB_a_ap_supplier") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AApSupplierTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AApSupplierTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApSupplierTable Data = new AApSupplierTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AApSupplierTable.TableId) + " FROM PUB_a_ap_supplier" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApSupplierTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AApSupplierTable.TableId, ADataSet, new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadByPrimaryKey(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApSupplierTable Data = new AApSupplierTable();
            LoadByPrimaryKey(AApSupplierTable.TableId, Data, new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApSupplierTable LoadByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadByPrimaryKey(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AApSupplierTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApSupplierRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApSupplierRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadUsingTemplate(AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApSupplierTable Data = new AApSupplierTable();
            LoadUsingTemplate(AApSupplierTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApSupplierTable LoadUsingTemplate(AApSupplierRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadUsingTemplate(AApSupplierRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadUsingTemplate(AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AApSupplierTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AApSupplierTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApSupplierTable Data = new AApSupplierTable();
            LoadUsingTemplate(AApSupplierTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApSupplierTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_supplier", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return Exists(AApSupplierTable.TableId, new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_supplier" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AApSupplierTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AApSupplierTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_supplier" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AApSupplierTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AApSupplierTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaPPartner(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApSupplierTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
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
        public static AApSupplierTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApSupplierTable Data = new AApSupplierTable();
            LoadViaForeignKey(AApSupplierTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApSupplierTable LoadViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadViaPPartner(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartner(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApSupplierTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
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
        public static AApSupplierTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApSupplierTable Data = new AApSupplierTable();
            LoadViaForeignKey(AApSupplierTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApSupplierTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApSupplierTable.TableId, PPartnerTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
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
        public static AApSupplierTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApSupplierTable Data = new AApSupplierTable();
            LoadViaForeignKey(AApSupplierTable.TableId, PPartnerTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApSupplierTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaPPartnerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaPPartner(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApSupplierTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(PPartnerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApSupplierTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaPPartnerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApSupplierTable.TableId, PPartnerTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACurrency(DataSet ADataSet, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApSupplierTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static AApSupplierTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApSupplierTable Data = new AApSupplierTable();
            LoadViaForeignKey(AApSupplierTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApSupplierTable LoadViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadViaACurrency(String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrency(ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApSupplierTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static AApSupplierTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApSupplierTable Data = new AApSupplierTable();
            LoadViaForeignKey(AApSupplierTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApSupplierTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApSupplierTable.TableId, ACurrencyTable.TableId, ADataSet, new string[1]{"a_currency_code_c"},
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
        public static AApSupplierTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApSupplierTable Data = new AApSupplierTable();
            LoadViaForeignKey(AApSupplierTable.TableId, ACurrencyTable.TableId, Data, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApSupplierTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApSupplierTable LoadViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACurrencyTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACurrency(String ACurrencyCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApSupplierTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApSupplierTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACurrencyTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApSupplierTable.TableId, ACurrencyTable.TableId, new string[1]{"a_currency_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AApSupplierTable.TableId, new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AApSupplierTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AApSupplierTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AApSupplierTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AApSupplierTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// This is either an invoice or a credit note in the Accounts Payable system.
    public class AApDocumentAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AApDocument";

        /// original table name in database
        public const string DBTABLENAME = "a_ap_document";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AApDocumentTable.TableId) + " FROM PUB_a_ap_document") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AApDocumentTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AApDocumentTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentTable Data = new AApDocumentTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AApDocumentTable.TableId) + " FROM PUB_a_ap_document" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AApDocumentTable.TableId, ADataSet, new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentTable Data = new AApDocumentTable();
            LoadByPrimaryKey(AApDocumentTable.TableId, Data, new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AApDocumentTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadUsingTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentTable Data = new AApDocumentTable();
            LoadUsingTemplate(AApDocumentTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentTable LoadUsingTemplate(AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadUsingTemplate(AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadUsingTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AApDocumentTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AApDocumentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentTable Data = new AApDocumentTable();
            LoadUsingTemplate(AApDocumentTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            return Exists(AApDocumentTable.TableId, new System.Object[2]{ALedgerNumber, AApNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AApDocumentTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AApDocumentTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AApDocumentTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AApDocumentTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApDocumentTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentTable Data = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApDocumentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentTable Data = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApDocumentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentTable Data = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAApSupplier(DataSet ADataSet, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentTable.TableId, AApSupplierTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApSupplier(DataSet AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaAApSupplier(AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApSupplier(DataSet AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApSupplier(AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaAApSupplier(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentTable Data = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, AApSupplierTable.TableId, Data, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentTable LoadViaAApSupplier(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return LoadViaAApSupplier(APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaAApSupplier(Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApSupplier(APartnerKey, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApSupplierTemplate(DataSet ADataSet, AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentTable.TableId, AApSupplierTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApSupplierTemplate(DataSet AData, AApSupplierRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApSupplierTemplate(DataSet AData, AApSupplierRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaAApSupplierTemplate(AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentTable Data = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, AApSupplierTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentTable LoadViaAApSupplierTemplate(AApSupplierRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAApSupplierTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaAApSupplierTemplate(AApSupplierRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApSupplierTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaAApSupplierTemplate(AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApSupplierTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApSupplierTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentTable.TableId, AApSupplierTable.TableId, ADataSet, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApSupplierTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApSupplierTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaAApSupplierTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentTable Data = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, AApSupplierTable.TableId, Data, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentTable LoadViaAApSupplierTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAApSupplierTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaAApSupplierTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApSupplierTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAApSupplier(Int64 APartnerKey, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentTable.TableId, AApSupplierTable.TableId, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, ATransaction);
        }

        /// auto generated
        public static int CountViaAApSupplierTemplate(AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentTable.TableId, AApSupplierTable.TableId, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAApSupplierTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentTable.TableId, AApSupplierTable.TableId, new string[1]{"p_partner_key_n"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet ADataSet, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_ap_account_c"},
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
        public static AApDocumentTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentTable Data = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_ap_account_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_ap_account_c"},
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
        public static AApDocumentTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentTable Data = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_ap_account_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_ap_account_c"},
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
        public static AApDocumentTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentTable Data = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_ap_account_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_ap_account_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_ap_account_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_ap_account_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AApDocumentTable.TableId, new System.Object[2]{ALedgerNumber, AApNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AApDocumentTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AApDocumentTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AApDocumentTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AApDocumentTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// This table receives a new entry when a credit note is applied to an invoice. Since the invoices and credit notes share the same table, we need a way to link the two, and this is the role of this table.
    public class ACrdtNoteInvoiceLinkAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "ACrdtNoteInvoiceLink";

        /// original table name in database
        public const string DBTABLENAME = "a_crdt_note_invoice_link";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, ACrdtNoteInvoiceLinkTable.TableId) + " FROM PUB_a_crdt_note_invoice_link") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(ACrdtNoteInvoiceLinkTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static ACrdtNoteInvoiceLinkTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACrdtNoteInvoiceLinkTable Data = new ACrdtNoteInvoiceLinkTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, ACrdtNoteInvoiceLinkTable.TableId) + " FROM PUB_a_crdt_note_invoice_link" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(ACrdtNoteInvoiceLinkTable.TableId, ADataSet, new System.Object[3]{ALedgerNumber, ACreditNoteNumber, AInvoiceNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ACreditNoteNumber, AInvoiceNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, ACreditNoteNumber, AInvoiceNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACrdtNoteInvoiceLinkTable Data = new ACrdtNoteInvoiceLinkTable();
            LoadByPrimaryKey(ACrdtNoteInvoiceLinkTable.TableId, Data, new System.Object[3]{ALedgerNumber, ACreditNoteNumber, AInvoiceNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ACreditNoteNumber, AInvoiceNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, ACreditNoteNumber, AInvoiceNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ACrdtNoteInvoiceLinkTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ACrdtNoteInvoiceLinkRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadUsingTemplate(ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACrdtNoteInvoiceLinkTable Data = new ACrdtNoteInvoiceLinkTable();
            LoadUsingTemplate(ACrdtNoteInvoiceLinkTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadUsingTemplate(ACrdtNoteInvoiceLinkRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadUsingTemplate(ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadUsingTemplate(ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(ACrdtNoteInvoiceLinkTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static ACrdtNoteInvoiceLinkTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACrdtNoteInvoiceLinkTable Data = new ACrdtNoteInvoiceLinkTable();
            LoadUsingTemplate(ACrdtNoteInvoiceLinkTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_crdt_note_invoice_link", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, TDBTransaction ATransaction)
        {
            return Exists(ACrdtNoteInvoiceLinkTable.TableId, new System.Object[3]{ALedgerNumber, ACreditNoteNumber, AInvoiceNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_crdt_note_invoice_link" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ACrdtNoteInvoiceLinkTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(ACrdtNoteInvoiceLinkTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_crdt_note_invoice_link" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(ACrdtNoteInvoiceLinkTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(ACrdtNoteInvoiceLinkTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumber(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_credit_note_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumber(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumber(AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumber(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumber(AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentCreditNoteNumber(Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACrdtNoteInvoiceLinkTable Data = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_credit_note_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentCreditNoteNumber(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentCreditNoteNumber(ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentCreditNoteNumber(Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentCreditNoteNumber(ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(DataSet ADataSet, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_credit_note_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(DataSet AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(DataSet AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentCreditNoteNumberTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACrdtNoteInvoiceLinkTable Data = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_credit_note_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentCreditNoteNumberTemplate(AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentCreditNoteNumberTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentCreditNoteNumberTemplate(AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentCreditNoteNumberTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentCreditNoteNumberTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentCreditNoteNumberTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_credit_note_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentCreditNoteNumberTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACrdtNoteInvoiceLinkTable Data = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_credit_note_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentCreditNoteNumberTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentCreditNoteNumberTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentCreditNoteNumberTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentCreditNoteNumberTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAApDocumentCreditNoteNumber(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_credit_note_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaAApDocumentCreditNoteNumberTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_credit_note_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAApDocumentCreditNoteNumberTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_credit_note_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static ACrdtNoteInvoiceLinkTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACrdtNoteInvoiceLinkTable Data = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static ACrdtNoteInvoiceLinkTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACrdtNoteInvoiceLinkTable Data = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static ACrdtNoteInvoiceLinkTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACrdtNoteInvoiceLinkTable Data = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumber(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_invoice_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumber(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumber(AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumber(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumber(AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentInvoiceNumber(Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACrdtNoteInvoiceLinkTable Data = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_invoice_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentInvoiceNumber(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentInvoiceNumber(ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentInvoiceNumber(Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentInvoiceNumber(ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(DataSet ADataSet, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_invoice_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(DataSet AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(DataSet AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentInvoiceNumberTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACrdtNoteInvoiceLinkTable Data = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_invoice_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentInvoiceNumberTemplate(AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentInvoiceNumberTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentInvoiceNumberTemplate(AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentInvoiceNumberTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentInvoiceNumberTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentInvoiceNumberTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_invoice_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentInvoiceNumberTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            ACrdtNoteInvoiceLinkTable Data = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_invoice_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentInvoiceNumberTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentInvoiceNumberTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static ACrdtNoteInvoiceLinkTable LoadViaAApDocumentInvoiceNumberTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentInvoiceNumberTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAApDocumentInvoiceNumber(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_invoice_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaAApDocumentInvoiceNumberTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_invoice_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAApDocumentInvoiceNumberTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_invoice_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(ACrdtNoteInvoiceLinkTable.TableId, new System.Object[3]{ALedgerNumber, ACreditNoteNumber, AInvoiceNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ACrdtNoteInvoiceLinkTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(ACrdtNoteInvoiceLinkTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(ACrdtNoteInvoiceLinkTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(ACrdtNoteInvoiceLinkTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// An invoice or credit note consists out of several items, or details. This table contains all these details.
    public class AApDocumentDetailAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AApDocumentDetail";

        /// original table name in database
        public const string DBTABLENAME = "a_ap_document_detail";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AApDocumentDetailTable.TableId) + " FROM PUB_a_ap_document_detail") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AApDocumentDetailTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AApDocumentDetailTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AApDocumentDetailTable.TableId) + " FROM PUB_a_ap_document_detail" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AApDocumentDetailTable.TableId, ADataSet, new System.Object[3]{ALedgerNumber, AApNumber, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadByPrimaryKey(AApDocumentDetailTable.TableId, Data, new System.Object[3]{ALedgerNumber, AApNumber, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AApNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AApNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AApDocumentDetailTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApDocumentDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApDocumentDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadUsingTemplate(AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadUsingTemplate(AApDocumentDetailTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadUsingTemplate(AApDocumentDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadUsingTemplate(AApDocumentDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadUsingTemplate(AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AApDocumentDetailTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AApDocumentDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadUsingTemplate(AApDocumentDetailTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document_detail", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return Exists(AApDocumentDetailTable.TableId, new System.Object[3]{ALedgerNumber, AApNumber, ADetailNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document_detail" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AApDocumentDetailTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AApDocumentDetailTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document_detail" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AApDocumentDetailTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AApDocumentDetailTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApDocumentDetailTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApDocumentDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApDocumentDetailTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentDetailTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentDetailTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentDetailTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAApDocument(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocument(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocument(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            return LoadViaAApDocument(ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocument(ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet ADataSet, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAApDocumentTemplate(AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAApDocumentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAApDocumentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAApDocumentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentDetailTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentDetailTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAApDocumentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentDetailTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaACostCentre(DataSet ADataSet, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
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
        public static AApDocumentDetailTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            return LoadViaACostCentre(ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentre(ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
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
        public static AApDocumentDetailTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ACostCentreTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
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
        public static AApDocumentDetailTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ACostCentreTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaACostCentreTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaACostCentre(Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentDetailTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaACostCentreTemplate(ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentDetailTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaACostCentreTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentDetailTable.TableId, ACostCentreTable.TableId, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet ADataSet, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_account_code_c"},
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
        public static AApDocumentDetailTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_account_code_c"},
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
        public static AApDocumentDetailTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_account_code_c"},
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
        public static AApDocumentDetailTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentDetailTable Data = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentDetailTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentDetailTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentDetailTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentDetailTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AApDocumentDetailTable.TableId, new System.Object[3]{ALedgerNumber, AApNumber, ADetailNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AApDocumentDetailTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AApDocumentDetailTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AApDocumentDetailTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AApDocumentDetailTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Records all payments that have been made against an accounts payable detail.
    public class AApPaymentAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AApPayment";

        /// original table name in database
        public const string DBTABLENAME = "a_ap_payment";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AApPaymentTable.TableId) + " FROM PUB_a_ap_payment") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AApPaymentTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AApPaymentTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApPaymentTable Data = new AApPaymentTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AApPaymentTable.TableId) + " FROM PUB_a_ap_payment" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApPaymentTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AApPaymentTable.TableId, ADataSet, new System.Object[2]{ALedgerNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApPaymentTable Data = new AApPaymentTable();
            LoadByPrimaryKey(AApPaymentTable.TableId, Data, new System.Object[2]{ALedgerNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApPaymentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AApPaymentTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadUsingTemplate(AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApPaymentTable Data = new AApPaymentTable();
            LoadUsingTemplate(AApPaymentTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApPaymentTable LoadUsingTemplate(AApPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadUsingTemplate(AApPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadUsingTemplate(AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AApPaymentTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AApPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApPaymentTable Data = new AApPaymentTable();
            LoadUsingTemplate(AApPaymentTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_payment", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            return Exists(AApPaymentTable.TableId, new System.Object[2]{ALedgerNumber, APaymentNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_payment" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AApPaymentTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AApPaymentTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_payment" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AApPaymentTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AApPaymentTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApPaymentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApPaymentTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApPaymentTable Data = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApPaymentTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApPaymentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApPaymentTable Data = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApPaymentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApPaymentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApPaymentTable Data = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApPaymentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApPaymentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApPaymentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApPaymentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaSUser(DataSet ADataSet, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApPaymentTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"s_user_id_c"},
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
        public static AApPaymentTable LoadViaSUser(String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApPaymentTable Data = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, SUserTable.TableId, Data, new string[1]{"s_user_id_c"},
                new System.Object[1]{AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApPaymentTable LoadViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            return LoadViaSUser(AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadViaSUser(String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUser(AUserId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApPaymentTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"s_user_id_c"},
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
        public static AApPaymentTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApPaymentTable Data = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, SUserTable.TableId, Data, new string[1]{"s_user_id_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApPaymentTable LoadViaSUserTemplate(SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApPaymentTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"s_user_id_c"},
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
        public static AApPaymentTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApPaymentTable Data = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, SUserTable.TableId, Data, new string[1]{"s_user_id_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApPaymentTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApPaymentTable.TableId, SUserTable.TableId, new string[1]{"s_user_id_c"},
                new System.Object[1]{AUserId}, ATransaction);
        }

        /// auto generated
        public static int CountViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApPaymentTable.TableId, SUserTable.TableId, new string[1]{"s_user_id_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaSUserTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApPaymentTable.TableId, SUserTable.TableId, new string[1]{"s_user_id_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet ADataSet, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApPaymentTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
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
        public static AApPaymentTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApPaymentTable Data = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApPaymentTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApPaymentTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
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
        public static AApPaymentTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApPaymentTable Data = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApPaymentTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApPaymentTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
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
        public static AApPaymentTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApPaymentTable Data = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApPaymentTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApPaymentTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApPaymentTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApPaymentTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApPaymentTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AApPaymentTable.TableId, new System.Object[2]{ALedgerNumber, APaymentNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AApPaymentTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AApPaymentTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AApPaymentTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AApPaymentTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// This table links the different payments to actual invoices and credit notes.
    public class AApDocumentPaymentAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AApDocumentPayment";

        /// original table name in database
        public const string DBTABLENAME = "a_ap_document_payment";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AApDocumentPaymentTable.TableId) + " FROM PUB_a_ap_document_payment") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AApDocumentPaymentTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AApDocumentPaymentTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentPaymentTable Data = new AApDocumentPaymentTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AApDocumentPaymentTable.TableId) + " FROM PUB_a_ap_document_payment" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AApDocumentPaymentTable.TableId, ADataSet, new System.Object[3]{ALedgerNumber, AApNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentPaymentTable Data = new AApDocumentPaymentTable();
            LoadByPrimaryKey(AApDocumentPaymentTable.TableId, Data, new System.Object[3]{ALedgerNumber, AApNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AApNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AApNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AApDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AApDocumentPaymentTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApDocumentPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApDocumentPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadUsingTemplate(AApDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentPaymentTable Data = new AApDocumentPaymentTable();
            LoadUsingTemplate(AApDocumentPaymentTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadUsingTemplate(AApDocumentPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadUsingTemplate(AApDocumentPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadUsingTemplate(AApDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AApDocumentPaymentTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AApDocumentPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentPaymentTable Data = new AApDocumentPaymentTable();
            LoadUsingTemplate(AApDocumentPaymentTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_document_payment", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            return Exists(AApDocumentPaymentTable.TableId, new System.Object[3]{ALedgerNumber, AApNumber, APaymentNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AApDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document_payment" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AApDocumentPaymentTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AApDocumentPaymentTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_document_payment" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AApDocumentPaymentTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AApDocumentPaymentTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApDocumentPaymentTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentPaymentTable Data = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApDocumentPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentPaymentTable Data = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApDocumentPaymentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentPaymentTable Data = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentPaymentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentPaymentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentPaymentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAApDocument(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocument(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocument(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentPaymentTable Data = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            return LoadViaAApDocument(ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocument(ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet ADataSet, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentPaymentTable Data = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApDocumentTemplate(AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApDocumentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentPaymentTable Data = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApDocumentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApDocumentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentPaymentTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentPaymentTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAApDocumentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentPaymentTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAApPayment(DataSet ADataSet, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApPaymentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                new System.Object[2]{ALedgerNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApPayment(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadViaAApPayment(AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApPayment(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApPayment(AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApPayment(Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentPaymentTable Data = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApPaymentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                new System.Object[2]{ALedgerNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApPayment(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            return LoadViaAApPayment(ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApPayment(Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApPayment(ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApPaymentTemplate(DataSet ADataSet, AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApPaymentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApPaymentTemplate(DataSet AData, AApPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApPaymentTemplate(DataSet AData, AApPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApPaymentTemplate(AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentPaymentTable Data = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApPaymentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApPaymentTemplate(AApPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAApPaymentTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApPaymentTemplate(AApPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApPaymentTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApPaymentTemplate(AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApPaymentTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApPaymentTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApPaymentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApPaymentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApPaymentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApPaymentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApDocumentPaymentTable Data = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApPaymentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApPaymentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAApPaymentTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApDocumentPaymentTable LoadViaAApPaymentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApPaymentTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAApPayment(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentPaymentTable.TableId, AApPaymentTable.TableId, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                new System.Object[2]{ALedgerNumber, APaymentNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaAApPaymentTemplate(AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentPaymentTable.TableId, AApPaymentTable.TableId, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAApPaymentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApDocumentPaymentTable.TableId, AApPaymentTable.TableId, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AApDocumentPaymentTable.TableId, new System.Object[3]{ALedgerNumber, AApNumber, APaymentNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AApDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AApDocumentPaymentTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AApDocumentPaymentTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AApDocumentPaymentTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AApDocumentPaymentTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// This table acts as a queue for electronic payments. If an invoice is paid electronically, the payment is added to this table. A EP program will go through this table paying all entries to GL and moving them to the a_ap_payment table.
    public class AEpPaymentAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AEpPayment";

        /// original table name in database
        public const string DBTABLENAME = "a_ep_payment";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AEpPaymentTable.TableId) + " FROM PUB_a_ep_payment") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AEpPaymentTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AEpPaymentTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpPaymentTable Data = new AEpPaymentTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AEpPaymentTable.TableId) + " FROM PUB_a_ep_payment" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpPaymentTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AEpPaymentTable.TableId, ADataSet, new System.Object[2]{ALedgerNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpPaymentTable Data = new AEpPaymentTable();
            LoadByPrimaryKey(AEpPaymentTable.TableId, Data, new System.Object[2]{ALedgerNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpPaymentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AEpPaymentTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AEpPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AEpPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadUsingTemplate(AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpPaymentTable Data = new AEpPaymentTable();
            LoadUsingTemplate(AEpPaymentTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpPaymentTable LoadUsingTemplate(AEpPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadUsingTemplate(AEpPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadUsingTemplate(AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AEpPaymentTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AEpPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpPaymentTable Data = new AEpPaymentTable();
            LoadUsingTemplate(AEpPaymentTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ep_payment", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            return Exists(AEpPaymentTable.TableId, new System.Object[2]{ALedgerNumber, APaymentNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ep_payment" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AEpPaymentTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AEpPaymentTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ep_payment" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AEpPaymentTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AEpPaymentTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpPaymentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AEpPaymentTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpPaymentTable Data = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpPaymentTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpPaymentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AEpPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpPaymentTable Data = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpPaymentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AEpPaymentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpPaymentTable Data = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpPaymentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpPaymentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpPaymentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpPaymentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaSUser(DataSet ADataSet, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpPaymentTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"s_user_id_c"},
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
        public static AEpPaymentTable LoadViaSUser(String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpPaymentTable Data = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, SUserTable.TableId, Data, new string[1]{"s_user_id_c"},
                new System.Object[1]{AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpPaymentTable LoadViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            return LoadViaSUser(AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadViaSUser(String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUser(AUserId, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpPaymentTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"s_user_id_c"},
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
        public static AEpPaymentTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpPaymentTable Data = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, SUserTable.TableId, Data, new string[1]{"s_user_id_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpPaymentTable LoadViaSUserTemplate(SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpPaymentTable.TableId, SUserTable.TableId, ADataSet, new string[1]{"s_user_id_c"},
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
        public static AEpPaymentTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpPaymentTable Data = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, SUserTable.TableId, Data, new string[1]{"s_user_id_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpPaymentTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadViaSUserTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaSUserTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaSUser(String AUserId, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpPaymentTable.TableId, SUserTable.TableId, new string[1]{"s_user_id_c"},
                new System.Object[1]{AUserId}, ATransaction);
        }

        /// auto generated
        public static int CountViaSUserTemplate(SUserRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpPaymentTable.TableId, SUserTable.TableId, new string[1]{"s_user_id_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaSUserTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpPaymentTable.TableId, SUserTable.TableId, new string[1]{"s_user_id_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet ADataSet, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpPaymentTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
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
        public static AEpPaymentTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpPaymentTable Data = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpPaymentTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpPaymentTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
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
        public static AEpPaymentTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpPaymentTable Data = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpPaymentTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpPaymentTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
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
        public static AEpPaymentTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpPaymentTable Data = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpPaymentTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpPaymentTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpPaymentTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpPaymentTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpPaymentTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AEpPaymentTable.TableId, new System.Object[2]{ALedgerNumber, APaymentNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AEpPaymentTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AEpPaymentTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AEpPaymentTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AEpPaymentTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// This table links the different EP payments to actual invoices and credit notes.
    public class AEpDocumentPaymentAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AEpDocumentPayment";

        /// original table name in database
        public const string DBTABLENAME = "a_ep_document_payment";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AEpDocumentPaymentTable.TableId) + " FROM PUB_a_ep_document_payment") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AEpDocumentPaymentTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AEpDocumentPaymentTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpDocumentPaymentTable Data = new AEpDocumentPaymentTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AEpDocumentPaymentTable.TableId) + " FROM PUB_a_ep_document_payment" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AEpDocumentPaymentTable.TableId, ADataSet, new System.Object[3]{ALedgerNumber, AApNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpDocumentPaymentTable Data = new AEpDocumentPaymentTable();
            LoadByPrimaryKey(AEpDocumentPaymentTable.TableId, Data, new System.Object[3]{ALedgerNumber, AApNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AApNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AApNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AEpDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AEpDocumentPaymentTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AEpDocumentPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AEpDocumentPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadUsingTemplate(AEpDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpDocumentPaymentTable Data = new AEpDocumentPaymentTable();
            LoadUsingTemplate(AEpDocumentPaymentTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadUsingTemplate(AEpDocumentPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadUsingTemplate(AEpDocumentPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadUsingTemplate(AEpDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AEpDocumentPaymentTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AEpDocumentPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpDocumentPaymentTable Data = new AEpDocumentPaymentTable();
            LoadUsingTemplate(AEpDocumentPaymentTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ep_document_payment", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            return Exists(AEpDocumentPaymentTable.TableId, new System.Object[3]{ALedgerNumber, AApNumber, APaymentNumber}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AEpDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ep_document_payment" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AEpDocumentPaymentTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AEpDocumentPaymentTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ep_document_payment" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AEpDocumentPaymentTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AEpDocumentPaymentTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AEpDocumentPaymentTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpDocumentPaymentTable Data = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AEpDocumentPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpDocumentPaymentTable Data = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AEpDocumentPaymentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpDocumentPaymentTable Data = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpDocumentPaymentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpDocumentPaymentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpDocumentPaymentTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAApDocument(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocument(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocument(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpDocumentPaymentTable Data = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            return LoadViaAApDocument(ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocument(ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet ADataSet, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpDocumentPaymentTable Data = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAApDocumentTemplate(AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AApDocumentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAApDocumentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpDocumentPaymentTable Data = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AApDocumentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAApDocumentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAApDocumentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAApDocument(Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpDocumentPaymentTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaAApDocumentTemplate(AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpDocumentPaymentTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAApDocumentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpDocumentPaymentTable.TableId, AApDocumentTable.TableId, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAEpPayment(DataSet ADataSet, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AEpPaymentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                new System.Object[2]{ALedgerNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAEpPayment(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadViaAEpPayment(AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAEpPayment(DataSet AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAEpPayment(AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAEpPayment(Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpDocumentPaymentTable Data = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AEpPaymentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                new System.Object[2]{ALedgerNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAEpPayment(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            return LoadViaAEpPayment(ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAEpPayment(Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAEpPayment(ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAEpPaymentTemplate(DataSet ADataSet, AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AEpPaymentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAEpPaymentTemplate(DataSet AData, AEpPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAEpPaymentTemplate(DataSet AData, AEpPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAEpPaymentTemplate(AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpDocumentPaymentTable Data = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AEpPaymentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAEpPaymentTemplate(AEpPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAEpPaymentTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAEpPaymentTemplate(AEpPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAEpPaymentTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAEpPaymentTemplate(AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAEpPaymentTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAEpPaymentTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AEpPaymentTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAEpPaymentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAEpPaymentTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAEpPaymentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AEpDocumentPaymentTable Data = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AEpPaymentTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAEpPaymentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAEpPaymentTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AEpDocumentPaymentTable LoadViaAEpPaymentTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAEpPaymentTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAEpPayment(Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpDocumentPaymentTable.TableId, AEpPaymentTable.TableId, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                new System.Object[2]{ALedgerNumber, APaymentNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaAEpPaymentTemplate(AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpDocumentPaymentTable.TableId, AEpPaymentTable.TableId, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAEpPaymentTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AEpDocumentPaymentTable.TableId, AEpPaymentTable.TableId, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AEpDocumentPaymentTable.TableId, new System.Object[3]{ALedgerNumber, AApNumber, APaymentNumber}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AEpDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AEpDocumentPaymentTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AEpDocumentPaymentTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AEpDocumentPaymentTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AEpDocumentPaymentTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }

    /// Analysis Attributes applied to an AP for posting to the GL.
    public class AApAnalAttribAccess : TTypedDataAccess
    {

        /// CamelCase version of table name
        public const string DATATABLENAME = "AApAnalAttrib";

        /// original table name in database
        public const string DBTABLENAME = "a_ap_anal_attrib";

        /// this method is called by all overloads
        public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, AApAnalAttribTable.TableId) + " FROM PUB_a_ap_anal_attrib") +
                            GenerateOrderByClause(AOrderBy)), TTypedDataTable.GetTableName(AApAnalAttribTable.TableId), ATransaction, AStartRecord, AMaxRecords);
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
        public static AApAnalAttribTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            DBAccess.GDBAccessObj.SelectDT(Data, GenerateSelectClause(AFieldList, AApAnalAttribTable.TableId) + " FROM PUB_a_ap_anal_attrib" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadAll(TDBTransaction ATransaction)
        {
            return LoadAll(null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadAll(StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadAll(AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadByPrimaryKey(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadByPrimaryKey(AApAnalAttribTable.TableId, ADataSet, new System.Object[4]{ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(AData, ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadByPrimaryKey(AApAnalAttribTable.TableId, Data, new System.Object[4]{ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadByPrimaryKey(ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, AApAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AApAnalAttribTable.TableId, ADataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApAnalAttribRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(DataSet AData, AApAnalAttribRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadUsingTemplate(AApAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadUsingTemplate(AApAnalAttribTable.TableId, Data, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadUsingTemplate(AApAnalAttribRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadUsingTemplate(AApAnalAttribRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadUsingTemplate(AApAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static void LoadUsingTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadUsingTemplate(AApAnalAttribTable.TableId, ADataSet, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
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
        public static AApAnalAttribTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadUsingTemplate(AApAnalAttribTable.TableId, Data, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadUsingTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadUsingTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// this method is called by all overloads
        public static int CountAll(TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_a_ap_anal_attrib", ATransaction, false));
        }

        /// check if a row exists by using the primary key
        public static bool Exists(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            return Exists(AApAnalAttribTable.TableId, new System.Object[4]{ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode}, ATransaction);
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(AApAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_anal_attrib" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AApAnalAttribTable.TableId), ATemplateRow, ATemplateOperators)), ATransaction, false,
                   GetParametersForWhereClause(AApAnalAttribTable.TableId, ATemplateRow)));
        }

        /// this method is called by all overloads
        public static int CountUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_a_ap_anal_attrib" + GenerateWhereClause(TTypedDataTable.GetColumnStringList(AApAnalAttribTable.TableId), ASearchCriteria)), ATransaction, false,
            GetParametersForWhereClause(AApAnalAttribTable.TableId, ASearchCriteria)));
        }

        /// auto generated
        public static void LoadViaAApDocumentDetail(DataSet ADataSet, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AApDocumentDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"},
                new System.Object[3]{ALedgerNumber, AApNumber, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetail(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetail(AData, ALedgerNumber, AApNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetail(DataSet AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetail(AData, ALedgerNumber, AApNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAApDocumentDetail(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AApDocumentDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"},
                new System.Object[3]{ALedgerNumber, AApNumber, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAApDocumentDetail(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentDetail(ALedgerNumber, AApNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAApDocumentDetail(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentDetail(ALedgerNumber, AApNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(DataSet ADataSet, AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AApDocumentDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(DataSet AData, AApDocumentDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(DataSet AData, AApDocumentDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAApDocumentDetailTemplate(AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AApDocumentDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAApDocumentDetailTemplate(AApDocumentDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentDetailTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAApDocumentDetailTemplate(AApDocumentDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentDetailTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAApDocumentDetailTemplate(AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentDetailTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AApDocumentDetailTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAApDocumentDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AApDocumentDetailTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAApDocumentDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentDetailTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAApDocumentDetailTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAApDocumentDetailTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAApDocumentDetail(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AApDocumentDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"},
                new System.Object[3]{ALedgerNumber, AApNumber, ADetailNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaAApDocumentDetailTemplate(AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AApDocumentDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAApDocumentDetailTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AApDocumentDetailTable.TableId, new string[3]{"a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaALedger(DataSet ADataSet, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApAnalAttribTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaALedger(Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedger(ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApAnalAttribTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, ALedgerTable.TableId, ADataSet, new string[1]{"a_ledger_number_i"},
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
        public static AApAnalAttribTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, ALedgerTable.TableId, Data, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaALedgerTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaALedger(Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(ALedgerRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaALedgerTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, ALedgerTable.TableId, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttribute(DataSet ADataSet, Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisAttributeTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_account_code_c", "a_analysis_type_code_c"},
                new System.Object[3]{ALedgerNumber, AAccountCode, AAnalysisTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttribute(DataSet AData, Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttribute(AData, ALedgerNumber, AAccountCode, AAnalysisTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttribute(DataSet AData, Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttribute(AData, ALedgerNumber, AAccountCode, AAnalysisTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisAttribute(Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisAttributeTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_account_code_c", "a_analysis_type_code_c"},
                new System.Object[3]{ALedgerNumber, AAccountCode, AAnalysisTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisAttribute(Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisAttribute(ALedgerNumber, AAccountCode, AAnalysisTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisAttribute(Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisAttribute(ALedgerNumber, AAccountCode, AAnalysisTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(DataSet ADataSet, AAnalysisAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisAttributeTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_account_code_c", "a_analysis_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(DataSet AData, AAnalysisAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(DataSet AData, AAnalysisAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisAttributeTemplate(AAnalysisAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisAttributeTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_account_code_c", "a_analysis_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisAttributeTemplate(AAnalysisAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisAttributeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisAttributeTemplate(AAnalysisAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisAttributeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisAttributeTemplate(AAnalysisAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisAttributeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisAttributeTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_account_code_c", "a_analysis_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisAttributeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisAttributeTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_account_code_c", "a_analysis_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisAttributeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisAttributeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisAttributeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisAttributeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAAnalysisAttribute(Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AAnalysisAttributeTable.TableId, new string[3]{"a_ledger_number_i", "a_account_code_c", "a_analysis_type_code_c"},
                new System.Object[3]{ALedgerNumber, AAccountCode, AAnalysisTypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAAnalysisAttributeTemplate(AAnalysisAttributeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AAnalysisAttributeTable.TableId, new string[3]{"a_ledger_number_i", "a_account_code_c", "a_analysis_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAAnalysisAttributeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AAnalysisAttributeTable.TableId, new string[3]{"a_ledger_number_i", "a_account_code_c", "a_analysis_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAAnalysisType(DataSet ADataSet, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisTypeTable.TableId, ADataSet, new string[1]{"a_analysis_type_code_c"},
                new System.Object[1]{AAnalysisTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAnalysisType(DataSet AData, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisType(AData, AAnalysisTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisType(DataSet AData, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisType(AData, AAnalysisTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisType(String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisTypeTable.TableId, Data, new string[1]{"a_analysis_type_code_c"},
                new System.Object[1]{AAnalysisTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisType(String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisType(AAnalysisTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisType(String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisType(AAnalysisTypeCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisTypeTemplate(DataSet ADataSet, AAnalysisTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisTypeTable.TableId, ADataSet, new string[1]{"a_analysis_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAnalysisTypeTemplate(DataSet AData, AAnalysisTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisTypeTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisTypeTemplate(DataSet AData, AAnalysisTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisTypeTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisTypeTemplate(AAnalysisTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisTypeTable.TableId, Data, new string[1]{"a_analysis_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisTypeTemplate(AAnalysisTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisTypeTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisTypeTemplate(AAnalysisTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisTypeTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisTypeTemplate(AAnalysisTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisTypeTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisTypeTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisTypeTable.TableId, ADataSet, new string[1]{"a_analysis_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAnalysisTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisTypeTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisTypeTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisTypeTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisTypeTable.TableId, Data, new string[1]{"a_analysis_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisTypeTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAnalysisTypeTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAnalysisTypeTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAAnalysisType(String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AAnalysisTypeTable.TableId, new string[1]{"a_analysis_type_code_c"},
                new System.Object[1]{AAnalysisTypeCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAAnalysisTypeTemplate(AAnalysisTypeRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AAnalysisTypeTable.TableId, new string[1]{"a_analysis_type_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAAnalysisTypeTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AAnalysisTypeTable.TableId, new string[1]{"a_analysis_type_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAAccount(DataSet ADataSet, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_account_code_c"},
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
        public static AApAnalAttribTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAccount(Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccount(ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_account_code_c"},
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
        public static AApAnalAttribTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAccountTable.TableId, ADataSet, new string[2]{"a_ledger_number_i", "a_account_code_c"},
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
        public static AApAnalAttribTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAccountTable.TableId, Data, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAAccountTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAAccount(Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(AAccountRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAAccountTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AAccountTable.TableId, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysis(DataSet ADataSet, Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AFreeformAnalysisTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_analysis_type_code_c", "a_analysis_attribute_value_c"},
                new System.Object[3]{ALedgerNumber, AAnalysisTypeCode, AAnalysisValue}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysis(DataSet AData, Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysis(AData, ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysis(DataSet AData, Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysis(AData, ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAFreeformAnalysis(Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AFreeformAnalysisTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_analysis_type_code_c", "a_analysis_attribute_value_c"},
                new System.Object[3]{ALedgerNumber, AAnalysisTypeCode, AAnalysisValue}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAFreeformAnalysis(Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, TDBTransaction ATransaction)
        {
            return LoadViaAFreeformAnalysis(ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAFreeformAnalysis(Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAFreeformAnalysis(ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(DataSet ADataSet, AFreeformAnalysisRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AFreeformAnalysisTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_analysis_type_code_c", "a_analysis_attribute_value_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(DataSet AData, AFreeformAnalysisRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(DataSet AData, AFreeformAnalysisRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAFreeformAnalysisTemplate(AFreeformAnalysisRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AFreeformAnalysisTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_analysis_type_code_c", "a_analysis_attribute_value_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAFreeformAnalysisTemplate(AFreeformAnalysisRow ATemplateRow, TDBTransaction ATransaction)
        {
            return LoadViaAFreeformAnalysisTemplate(ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAFreeformAnalysisTemplate(AFreeformAnalysisRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAFreeformAnalysisTemplate(ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAFreeformAnalysisTemplate(AFreeformAnalysisRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAFreeformAnalysisTemplate(ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(DataSet ADataSet, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            LoadViaForeignKey(AApAnalAttribTable.TableId, AFreeformAnalysisTable.TableId, ADataSet, new string[3]{"a_ledger_number_i", "a_analysis_type_code_c", "a_analysis_attribute_value_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(DataSet AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAFreeformAnalysisTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AApAnalAttribTable Data = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AFreeformAnalysisTable.TableId, Data, new string[3]{"a_ledger_number_i", "a_analysis_type_code_c", "a_analysis_attribute_value_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
            return Data;
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAFreeformAnalysisTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return LoadViaAFreeformAnalysisTemplate(ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static AApAnalAttribTable LoadViaAFreeformAnalysisTemplate(TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            return LoadViaAFreeformAnalysisTemplate(ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static int CountViaAFreeformAnalysis(Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AFreeformAnalysisTable.TableId, new string[3]{"a_ledger_number_i", "a_analysis_type_code_c", "a_analysis_attribute_value_c"},
                new System.Object[3]{ALedgerNumber, AAnalysisTypeCode, AAnalysisValue}, ATransaction);
        }

        /// auto generated
        public static int CountViaAFreeformAnalysisTemplate(AFreeformAnalysisRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AFreeformAnalysisTable.TableId, new string[3]{"a_ledger_number_i", "a_analysis_type_code_c", "a_analysis_attribute_value_c"},
                ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static int CountViaAFreeformAnalysisTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            return CountViaForeignKey(AApAnalAttribTable.TableId, AFreeformAnalysisTable.TableId, new string[3]{"a_ledger_number_i", "a_analysis_type_code_c", "a_analysis_attribute_value_c"},
                ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static void DeleteByPrimaryKey(Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            DeleteByPrimaryKey(AApAnalAttribTable.TableId, new System.Object[4]{ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode}, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(AApAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AApAnalAttribTable.TableId, ATemplateRow, ATemplateOperators, ATransaction);
        }

        /// auto generated
        public static void DeleteUsingTemplate(TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            DeleteUsingTemplate(AApAnalAttribTable.TableId, ASearchCriteria, ATransaction);
        }

        /// auto generated
        public static bool SubmitChanges(AApAnalAttribTable ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
        {
            return SubmitChanges(AApAnalAttribTable.TableId, ATable, ATransaction, out AVerificationResult, UserInfo.GUserInfo.UserID);
        }
    }
}