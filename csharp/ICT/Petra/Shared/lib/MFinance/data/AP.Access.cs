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
        public static void LoadAll(out AApSupplierTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApSupplierTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AApSupplierTable.TableId) + " FROM PUB_a_ap_supplier" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AApSupplierTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AApSupplierTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadByPrimaryKey(out AApSupplierTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApSupplierTable();
            LoadByPrimaryKey(AApSupplierTable.TableId, AData, new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AApSupplierTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AApSupplierTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AApSupplierTable AData, AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApSupplierTable();
            LoadUsingTemplate(AApSupplierTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApSupplierTable AData, AApSupplierRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApSupplierTable AData, AApSupplierRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApSupplierTable AData, AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AApSupplierTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApSupplierTable();
            LoadUsingTemplate(AApSupplierTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApSupplierTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApSupplierTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaPPartner(out AApSupplierTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApSupplierTable();
            LoadViaForeignKey(AApSupplierTable.TableId, PPartnerTable.TableId, AData, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartner(out AApSupplierTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartner(out AApSupplierTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartner(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaPPartnerTemplate(out AApSupplierTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApSupplierTable();
            LoadViaForeignKey(AApSupplierTable.TableId, PPartnerTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out AApSupplierTable AData, PPartnerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out AApSupplierTable AData, PPartnerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out AApSupplierTable AData, PPartnerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaPPartnerTemplate(out AApSupplierTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApSupplierTable();
            LoadViaForeignKey(AApSupplierTable.TableId, PPartnerTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out AApSupplierTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaPPartnerTemplate(out AApSupplierTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaPPartnerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaACurrency(out AApSupplierTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApSupplierTable();
            LoadViaForeignKey(AApSupplierTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                new System.Object[1]{ACurrencyCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrency(out AApSupplierTable AData, String ACurrencyCode, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrency(out AApSupplierTable AData, String ACurrencyCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrency(out AData, ACurrencyCode, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaACurrencyTemplate(out AApSupplierTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApSupplierTable();
            LoadViaForeignKey(AApSupplierTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AApSupplierTable AData, ACurrencyRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AApSupplierTable AData, ACurrencyRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AApSupplierTable AData, ACurrencyRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaACurrencyTemplate(out AApSupplierTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApSupplierTable();
            LoadViaForeignKey(AApSupplierTable.TableId, ACurrencyTable.TableId, AData, new string[1]{"a_currency_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AApSupplierTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACurrencyTemplate(out AApSupplierTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACurrencyTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadAll(out AApDocumentTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AApDocumentTable.TableId) + " FROM PUB_a_ap_document" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AApDocumentTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AApDocumentTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadByPrimaryKey(out AApDocumentTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentTable();
            LoadByPrimaryKey(AApDocumentTable.TableId, AData, new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentTable AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AApDocumentTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentTable();
            LoadUsingTemplate(AApDocumentTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentTable AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentTable AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AApDocumentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentTable();
            LoadUsingTemplate(AApDocumentTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedger(out AApDocumentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(out AApDocumentTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out AApDocumentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AApDocumentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AApDocumentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApSupplier(out AApDocumentTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, AApSupplierTable.TableId, AData, new string[1]{"p_partner_key_n"},
                new System.Object[1]{APartnerKey}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApSupplier(out AApDocumentTable AData, Int64 APartnerKey, TDBTransaction ATransaction)
        {
            LoadViaAApSupplier(out AData, APartnerKey, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApSupplier(out AApDocumentTable AData, Int64 APartnerKey, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApSupplier(out AData, APartnerKey, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApSupplierTemplate(out AApDocumentTable AData, AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, AApSupplierTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApSupplierTemplate(out AApDocumentTable AData, AApSupplierRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApSupplierTemplate(out AApDocumentTable AData, AApSupplierRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApSupplierTemplate(out AApDocumentTable AData, AApSupplierRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApSupplierTemplate(out AApDocumentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, AApSupplierTable.TableId, AData, new string[1]{"p_partner_key_n"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApSupplierTemplate(out AApDocumentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApSupplierTemplate(out AApDocumentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApSupplierTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccount(out AApDocumentTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_ap_account_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccount(out AApDocumentTable AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccount(out AApDocumentTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccountTemplate(out AApDocumentTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_ap_account_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentTable AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentTable AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccountTemplate(out AApDocumentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentTable();
            LoadViaForeignKey(AApDocumentTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_ap_account_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadAll(out ACrdtNoteInvoiceLinkTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ACrdtNoteInvoiceLinkTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, ACrdtNoteInvoiceLinkTable.TableId) + " FROM PUB_a_crdt_note_invoice_link" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out ACrdtNoteInvoiceLinkTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out ACrdtNoteInvoiceLinkTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadByPrimaryKey(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ACrdtNoteInvoiceLinkTable();
            LoadByPrimaryKey(ACrdtNoteInvoiceLinkTable.TableId, AData, new System.Object[3]{ALedgerNumber, ACreditNoteNumber, AInvoiceNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ACreditNoteNumber, AInvoiceNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 ACreditNoteNumber, Int32 AInvoiceNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, ACreditNoteNumber, AInvoiceNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out ACrdtNoteInvoiceLinkTable AData, ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ACrdtNoteInvoiceLinkTable();
            LoadUsingTemplate(ACrdtNoteInvoiceLinkTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ACrdtNoteInvoiceLinkTable AData, ACrdtNoteInvoiceLinkRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ACrdtNoteInvoiceLinkTable AData, ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ACrdtNoteInvoiceLinkTable AData, ACrdtNoteInvoiceLinkRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out ACrdtNoteInvoiceLinkTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ACrdtNoteInvoiceLinkTable();
            LoadUsingTemplate(ACrdtNoteInvoiceLinkTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ACrdtNoteInvoiceLinkTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out ACrdtNoteInvoiceLinkTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentCreditNoteNumber(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_credit_note_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumber(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumber(out AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumber(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumber(out AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_credit_note_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_credit_note_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentCreditNoteNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentCreditNoteNumberTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedger(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out ACrdtNoteInvoiceLinkTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ACrdtNoteInvoiceLinkTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ACrdtNoteInvoiceLinkTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ACrdtNoteInvoiceLinkTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out ACrdtNoteInvoiceLinkTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ACrdtNoteInvoiceLinkTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out ACrdtNoteInvoiceLinkTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentInvoiceNumber(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_invoice_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumber(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumber(out AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumber(out ACrdtNoteInvoiceLinkTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumber(out AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentInvoiceNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_invoice_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentInvoiceNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new ACrdtNoteInvoiceLinkTable();
            LoadViaForeignKey(ACrdtNoteInvoiceLinkTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_invoice_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentInvoiceNumberTemplate(out ACrdtNoteInvoiceLinkTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentInvoiceNumberTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadAll(out AApDocumentDetailTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AApDocumentDetailTable.TableId) + " FROM PUB_a_ap_document_detail" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AApDocumentDetailTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AApDocumentDetailTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadByPrimaryKey(out AApDocumentDetailTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadByPrimaryKey(AApDocumentDetailTable.TableId, AData, new System.Object[3]{ALedgerNumber, AApNumber, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentDetailTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentDetailTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AApDocumentDetailTable AData, AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadUsingTemplate(AApDocumentDetailTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentDetailTable AData, AApDocumentDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentDetailTable AData, AApDocumentDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentDetailTable AData, AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadUsingTemplate(AApDocumentDetailTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedger(out AApDocumentDetailTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(out AApDocumentDetailTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out AApDocumentDetailTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AApDocumentDetailTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentDetailTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentDetailTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentDetailTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocument(out AApDocumentDetailTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocument(out AApDocumentDetailTable AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(out AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocument(out AApDocumentDetailTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(out AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentTemplate(out AApDocumentDetailTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentDetailTable AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentDetailTable AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentDetailTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaACostCentre(out AApDocumentDetailTable AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ACostCentreTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                new System.Object[2]{ALedgerNumber, ACostCentreCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentre(out AApDocumentDetailTable AData, Int32 ALedgerNumber, String ACostCentreCode, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(out AData, ALedgerNumber, ACostCentreCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentre(out AApDocumentDetailTable AData, Int32 ALedgerNumber, String ACostCentreCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentre(out AData, ALedgerNumber, ACostCentreCode, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaACostCentreTemplate(out AApDocumentDetailTable AData, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ACostCentreTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(out AApDocumentDetailTable AData, ACostCentreRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(out AApDocumentDetailTable AData, ACostCentreRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(out AApDocumentDetailTable AData, ACostCentreRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaACostCentreTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, ACostCentreTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_cost_centre_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaACostCentreTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaACostCentreTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccount(out AApDocumentDetailTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccount(out AApDocumentDetailTable AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccount(out AApDocumentDetailTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccountTemplate(out AApDocumentDetailTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentDetailTable AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentDetailTable AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentDetailTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccountTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentDetailTable();
            LoadViaForeignKey(AApDocumentDetailTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApDocumentDetailTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadAll(out AApPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApPaymentTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AApPaymentTable.TableId) + " FROM PUB_a_ap_payment" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AApPaymentTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AApPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadByPrimaryKey(out AApPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApPaymentTable();
            LoadByPrimaryKey(AApPaymentTable.TableId, AData, new System.Object[2]{ALedgerNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AApPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AApPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AApPaymentTable AData, AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApPaymentTable();
            LoadUsingTemplate(AApPaymentTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApPaymentTable AData, AApPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApPaymentTable AData, AApPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApPaymentTable AData, AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AApPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApPaymentTable();
            LoadUsingTemplate(AApPaymentTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedger(out AApPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(out AApPaymentTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out AApPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AApPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApPaymentTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApPaymentTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AApPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaSUser(out AApPaymentTable AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, SUserTable.TableId, AData, new string[1]{"s_user_id_c"},
                new System.Object[1]{AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUser(out AApPaymentTable AData, String AUserId, TDBTransaction ATransaction)
        {
            LoadViaSUser(out AData, AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUser(out AApPaymentTable AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUser(out AData, AUserId, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaSUserTemplate(out AApPaymentTable AData, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, SUserTable.TableId, AData, new string[1]{"s_user_id_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(out AApPaymentTable AData, SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(out AApPaymentTable AData, SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(out AApPaymentTable AData, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaSUserTemplate(out AApPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, SUserTable.TableId, AData, new string[1]{"s_user_id_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(out AApPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(out AApPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccount(out AApPaymentTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccount(out AApPaymentTable AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccount(out AApPaymentTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccountTemplate(out AApPaymentTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApPaymentTable AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApPaymentTable AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApPaymentTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccountTemplate(out AApPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApPaymentTable();
            LoadViaForeignKey(AApPaymentTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadAll(out AApDocumentPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentPaymentTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AApDocumentPaymentTable.TableId) + " FROM PUB_a_ap_document_payment" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AApDocumentPaymentTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AApDocumentPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadByPrimaryKey(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentPaymentTable();
            LoadByPrimaryKey(AApDocumentPaymentTable.TableId, AData, new System.Object[3]{ALedgerNumber, AApNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AApDocumentPaymentTable AData, AApDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentPaymentTable();
            LoadUsingTemplate(AApDocumentPaymentTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentPaymentTable AData, AApDocumentPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentPaymentTable AData, AApDocumentPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentPaymentTable AData, AApDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AApDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentPaymentTable();
            LoadUsingTemplate(AApDocumentPaymentTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedger(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AApDocumentPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentPaymentTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentPaymentTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AApDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocument(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocument(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(out AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocument(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(out AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentTemplate(out AApDocumentPaymentTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentPaymentTable AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentPaymentTable AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentPaymentTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentTemplate(out AApDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AApDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApPayment(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApPaymentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                new System.Object[2]{ALedgerNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApPayment(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadViaAApPayment(out AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApPayment(out AApDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApPayment(out AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApPaymentTemplate(out AApDocumentPaymentTable AData, AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApPaymentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApPaymentTemplate(out AApDocumentPaymentTable AData, AApPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApPaymentTemplate(out AApDocumentPaymentTable AData, AApPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApPaymentTemplate(out AApDocumentPaymentTable AData, AApPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApPaymentTemplate(out AApDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApDocumentPaymentTable();
            LoadViaForeignKey(AApDocumentPaymentTable.TableId, AApPaymentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApPaymentTemplate(out AApDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApPaymentTemplate(out AApDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApPaymentTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadAll(out AEpPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpPaymentTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AEpPaymentTable.TableId) + " FROM PUB_a_ep_payment" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AEpPaymentTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AEpPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadByPrimaryKey(out AEpPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpPaymentTable();
            LoadByPrimaryKey(AEpPaymentTable.TableId, AData, new System.Object[2]{ALedgerNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AEpPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AEpPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AEpPaymentTable AData, AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpPaymentTable();
            LoadUsingTemplate(AEpPaymentTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AEpPaymentTable AData, AEpPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AEpPaymentTable AData, AEpPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AEpPaymentTable AData, AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AEpPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpPaymentTable();
            LoadUsingTemplate(AEpPaymentTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AEpPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AEpPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedger(out AEpPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(out AEpPaymentTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out AEpPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AEpPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpPaymentTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpPaymentTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AEpPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaSUser(out AEpPaymentTable AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, SUserTable.TableId, AData, new string[1]{"s_user_id_c"},
                new System.Object[1]{AUserId}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUser(out AEpPaymentTable AData, String AUserId, TDBTransaction ATransaction)
        {
            LoadViaSUser(out AData, AUserId, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUser(out AEpPaymentTable AData, String AUserId, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUser(out AData, AUserId, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaSUserTemplate(out AEpPaymentTable AData, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, SUserTable.TableId, AData, new string[1]{"s_user_id_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(out AEpPaymentTable AData, SUserRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(out AEpPaymentTable AData, SUserRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(out AEpPaymentTable AData, SUserRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaSUserTemplate(out AEpPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, SUserTable.TableId, AData, new string[1]{"s_user_id_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(out AEpPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaSUserTemplate(out AEpPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaSUserTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccount(out AEpPaymentTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccount(out AEpPaymentTable AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccount(out AEpPaymentTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccountTemplate(out AEpPaymentTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AEpPaymentTable AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AEpPaymentTable AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AEpPaymentTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccountTemplate(out AEpPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpPaymentTable();
            LoadViaForeignKey(AEpPaymentTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_bank_account_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AEpPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AEpPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadAll(out AEpDocumentPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpDocumentPaymentTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AEpDocumentPaymentTable.TableId) + " FROM PUB_a_ep_document_payment" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AEpDocumentPaymentTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AEpDocumentPaymentTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadByPrimaryKey(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpDocumentPaymentTable();
            LoadByPrimaryKey(AEpDocumentPaymentTable.TableId, AData, new System.Object[3]{ALedgerNumber, AApNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AEpDocumentPaymentTable AData, AEpDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpDocumentPaymentTable();
            LoadUsingTemplate(AEpDocumentPaymentTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AEpDocumentPaymentTable AData, AEpDocumentPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AEpDocumentPaymentTable AData, AEpDocumentPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AEpDocumentPaymentTable AData, AEpDocumentPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AEpDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpDocumentPaymentTable();
            LoadUsingTemplate(AEpDocumentPaymentTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AEpDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AEpDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedger(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AEpDocumentPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpDocumentPaymentTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpDocumentPaymentTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpDocumentPaymentTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AEpDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AEpDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocument(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                new System.Object[2]{ALedgerNumber, AApNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocument(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(out AData, ALedgerNumber, AApNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocument(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 AApNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocument(out AData, ALedgerNumber, AApNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentTemplate(out AEpDocumentPaymentTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AEpDocumentPaymentTable AData, AApDocumentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AEpDocumentPaymentTable AData, AApDocumentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AEpDocumentPaymentTable AData, AApDocumentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentTemplate(out AEpDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AApDocumentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_ap_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AEpDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentTemplate(out AEpDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAEpPayment(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AEpPaymentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                new System.Object[2]{ALedgerNumber, APaymentNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAEpPayment(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, TDBTransaction ATransaction)
        {
            LoadViaAEpPayment(out AData, ALedgerNumber, APaymentNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAEpPayment(out AEpDocumentPaymentTable AData, Int32 ALedgerNumber, Int32 APaymentNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAEpPayment(out AData, ALedgerNumber, APaymentNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAEpPaymentTemplate(out AEpDocumentPaymentTable AData, AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AEpPaymentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAEpPaymentTemplate(out AEpDocumentPaymentTable AData, AEpPaymentRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAEpPaymentTemplate(out AEpDocumentPaymentTable AData, AEpPaymentRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAEpPaymentTemplate(out AEpDocumentPaymentTable AData, AEpPaymentRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAEpPaymentTemplate(out AEpDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AEpDocumentPaymentTable();
            LoadViaForeignKey(AEpDocumentPaymentTable.TableId, AEpPaymentTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_payment_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAEpPaymentTemplate(out AEpDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAEpPaymentTemplate(out AEpDocumentPaymentTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAEpPaymentTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadAll(out AApAnalAttribTable AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            DBAccess.GDBAccessObj.SelectDT(AData, GenerateSelectClause(AFieldList, AApAnalAttribTable.TableId) + " FROM PUB_a_ap_anal_attrib" + GenerateOrderByClause(AOrderBy), ATransaction, null, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadAll(out AApAnalAttribTable AData, TDBTransaction ATransaction)
        {
            LoadAll(out AData, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadAll(out AApAnalAttribTable AData, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadByPrimaryKey(out AApAnalAttribTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadByPrimaryKey(AApAnalAttribTable.TableId, AData, new System.Object[4]{ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AApAnalAttribTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadByPrimaryKey(out AApAnalAttribTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadByPrimaryKey(out AData, ALedgerNumber, AApNumber, ADetailNumber, AAnalysisTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AApAnalAttribTable AData, AApAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadUsingTemplate(AApAnalAttribTable.TableId, AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApAnalAttribTable AData, AApAnalAttribRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApAnalAttribTable AData, AApAnalAttribRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApAnalAttribTable AData, AApAnalAttribRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadUsingTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadUsingTemplate(AApAnalAttribTable.TableId, AData, ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadUsingTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadUsingTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentDetail(out AApAnalAttribTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AApDocumentDetailTable.TableId, AData, new string[3]{"a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"},
                new System.Object[3]{ALedgerNumber, AApNumber, ADetailNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetail(out AApAnalAttribTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetail(out AData, ALedgerNumber, AApNumber, ADetailNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetail(out AApAnalAttribTable AData, Int32 ALedgerNumber, Int32 AApNumber, Int32 ADetailNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetail(out AData, ALedgerNumber, AApNumber, ADetailNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentDetailTemplate(out AApAnalAttribTable AData, AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AApDocumentDetailTable.TableId, AData, new string[3]{"a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(out AApAnalAttribTable AData, AApDocumentDetailRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(out AApAnalAttribTable AData, AApDocumentDetailRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(out AApAnalAttribTable AData, AApDocumentDetailRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAApDocumentDetailTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AApDocumentDetailTable.TableId, AData, new string[3]{"a_ledger_number_i", "a_ap_number_i", "a_detail_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAApDocumentDetailTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAApDocumentDetailTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedger(out AApAnalAttribTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                new System.Object[1]{ALedgerNumber}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedger(out AApAnalAttribTable AData, Int32 ALedgerNumber, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedger(out AApAnalAttribTable AData, Int32 ALedgerNumber, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedger(out AData, ALedgerNumber, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AApAnalAttribTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApAnalAttribTable AData, ALedgerRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApAnalAttribTable AData, ALedgerRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApAnalAttribTable AData, ALedgerRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaALedgerTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, ALedgerTable.TableId, AData, new string[1]{"a_ledger_number_i"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaALedgerTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaALedgerTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAnalysisAttribute(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisAttributeTable.TableId, AData, new string[3]{"a_ledger_number_i", "a_account_code_c", "a_analysis_type_code_c"},
                new System.Object[3]{ALedgerNumber, AAccountCode, AAnalysisTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttribute(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttribute(out AData, ALedgerNumber, AAccountCode, AAnalysisTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttribute(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAccountCode, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttribute(out AData, ALedgerNumber, AAccountCode, AAnalysisTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAnalysisAttributeTemplate(out AApAnalAttribTable AData, AAnalysisAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisAttributeTable.TableId, AData, new string[3]{"a_ledger_number_i", "a_account_code_c", "a_analysis_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(out AApAnalAttribTable AData, AAnalysisAttributeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(out AApAnalAttribTable AData, AAnalysisAttributeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(out AApAnalAttribTable AData, AAnalysisAttributeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAnalysisAttributeTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisAttributeTable.TableId, AData, new string[3]{"a_ledger_number_i", "a_account_code_c", "a_analysis_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisAttributeTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisAttributeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAnalysisType(out AApAnalAttribTable AData, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisTypeTable.TableId, AData, new string[1]{"a_analysis_type_code_c"},
                new System.Object[1]{AAnalysisTypeCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAnalysisType(out AApAnalAttribTable AData, String AAnalysisTypeCode, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisType(out AData, AAnalysisTypeCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisType(out AApAnalAttribTable AData, String AAnalysisTypeCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisType(out AData, AAnalysisTypeCode, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAnalysisTypeTemplate(out AApAnalAttribTable AData, AAnalysisTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisTypeTable.TableId, AData, new string[1]{"a_analysis_type_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAnalysisTypeTemplate(out AApAnalAttribTable AData, AAnalysisTypeRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisTypeTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisTypeTemplate(out AApAnalAttribTable AData, AAnalysisTypeRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisTypeTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisTypeTemplate(out AApAnalAttribTable AData, AAnalysisTypeRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisTypeTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAnalysisTypeTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAnalysisTypeTable.TableId, AData, new string[1]{"a_analysis_type_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAnalysisTypeTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisTypeTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAnalysisTypeTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAnalysisTypeTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccount(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                new System.Object[2]{ALedgerNumber, AAccountCode}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccount(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAccountCode, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccount(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAccountCode, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccount(out AData, ALedgerNumber, AAccountCode, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccountTemplate(out AApAnalAttribTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApAnalAttribTable AData, AAccountRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApAnalAttribTable AData, AAccountRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApAnalAttribTable AData, AAccountRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAAccountTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AAccountTable.TableId, AData, new string[2]{"a_ledger_number_i", "a_account_code_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAAccountTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAAccountTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAFreeformAnalysis(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AFreeformAnalysisTable.TableId, AData, new string[3]{"a_ledger_number_i", "a_analysis_type_code_c", "a_analysis_attribute_value_c"},
                new System.Object[3]{ALedgerNumber, AAnalysisTypeCode, AAnalysisValue}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysis(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysis(out AData, ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysis(out AApAnalAttribTable AData, Int32 ALedgerNumber, String AAnalysisTypeCode, String AAnalysisValue, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysis(out AData, ALedgerNumber, AAnalysisTypeCode, AAnalysisValue, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAFreeformAnalysisTemplate(out AApAnalAttribTable AData, AFreeformAnalysisRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AFreeformAnalysisTable.TableId, AData, new string[3]{"a_ledger_number_i", "a_analysis_type_code_c", "a_analysis_attribute_value_c"},
                ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(out AApAnalAttribTable AData, AFreeformAnalysisRow ATemplateRow, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(out AApAnalAttribTable AData, AFreeformAnalysisRow ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(out AApAnalAttribTable AData, AFreeformAnalysisRow ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
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
        public static void LoadViaAFreeformAnalysisTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
        {
            AData = new AApAnalAttribTable();
            LoadViaForeignKey(AApAnalAttribTable.TableId, AFreeformAnalysisTable.TableId, AData, new string[3]{"a_ledger_number_i", "a_analysis_type_code_c", "a_analysis_attribute_value_c"},
                ASearchCriteria, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(out AData, ASearchCriteria, null, ATransaction, null, 0, 0);
        }

        /// auto generated
        public static void LoadViaAFreeformAnalysisTemplate(out AApAnalAttribTable AData, TSearchCriteria[] ASearchCriteria, StringCollection AFieldList, TDBTransaction ATransaction)
        {
            LoadViaAFreeformAnalysisTemplate(out AData, ASearchCriteria, AFieldList, ATransaction, null, 0, 0);
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