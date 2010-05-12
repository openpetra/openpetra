//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using Ict.Common.DB;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.BankImport
{
    /// <summary>
    /// we need a dataset for the columns of the grid
    /// </summary>
    public class TGetData
    {
        private static TDataBase db;

        /// open the db connection
        public static bool InitDBConnection()
        {
            // establish connection to database
            TAppSettingsManager settings = new TAppSettingsManager(false);

            db = new TDataBase();

            new TLogging("debug.log");
            db.DebugLevel = settings.GetInt16("DebugLevel", 0);

            TDBType dbtype = CommonTypes.ParseDBType(settings.GetValue("Server.RDBMSType"));

            if (dbtype != TDBType.ProgressODBC)
            {
                throw new Exception("at the moment only Progress ODBC db is supported");
            }

            db.EstablishDBConnection(dbtype,
                settings.GetValue("Server.ODBC_DSN"),
                "",
                "",
                settings.GetValue("odbc.username"),
                settings.GetValue("odbc.password"),
                "");
            DBAccess.GDBAccessObj = db;

            return true;
        }

        /// <summary>
        /// return a table with gift details for the given date with donor partner keys and bank account numbers
        /// </summary>
        public static void GetGiftsByDate(ref BankImportTDS AMainDS, DateTime ADateEffective)
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

            // first get all gifts, even those that have no bank account associated
            string stmt = TDataBase.ReadSqlFile("GetDonationsByDate.sql");

            OdbcParameter[] parameters = new OdbcParameter[2];
            parameters[0] = new OdbcParameter("ADateEffective", OdbcType.Date);
            parameters[0].Value = ADateEffective;
            parameters[1] = new OdbcParameter("ALedgerNumber", OdbcType.Int);
            parameters[1].Value = Convert.ToInt32(TAppSettingsManager.GetValueStatic("LedgerNumber"));
            DBAccess.GDBAccessObj.Select(AMainDS, stmt, AMainDS.AGiftDetail.TableName, transaction, parameters);

            // get PartnerKey and banking details (most important BankAccountNumber) for all donations on the given date
            stmt = TDataBase.ReadSqlFile("GetBankAccountByDate.sql");
            parameters = new OdbcParameter[1];
            parameters[0] = new OdbcParameter("ADateEffective", OdbcType.Date);
            parameters[0].Value = ADateEffective;

            // There can be several donors with the same banking details
            AMainDS.PBankingDetails.Constraints.Clear();

            DBAccess.GDBAccessObj.Select(AMainDS, stmt, AMainDS.PBankingDetails.TableName, transaction, parameters);

            DBAccess.GDBAccessObj.RollbackTransaction();

            //            XmlDocument doc = TDataBase.DataTableToXml(AMainDS.AGiftDetail);
            //            TCsv2Xml.Xml2Csv(doc, "test.csv");
        }

        /// get the donor key and shortname by the bank account number
        public static Int64 GetDonorByAccountNumber(string ABankAccountNumber, out string ADonorShortName)
        {
            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

            string stmt = TDataBase.ReadSqlFile("GetDonorByBankAccount.sql");

            OdbcParameter[] parameters = new OdbcParameter[1];
            parameters[0] = new OdbcParameter("ABankAccountNumber", OdbcType.VarChar);
            parameters[0].Value = ABankAccountNumber;
            DataTable donorTable = DBAccess.GDBAccessObj.SelectDT(stmt, "donor", transaction, parameters);

            DBAccess.GDBAccessObj.RollbackTransaction();

            ADonorShortName = "UNKNOWN";

            if (donorTable.Rows.Count == 1)
            {
                ADonorShortName = donorTable.Rows[0].ItemArray[donorTable.Columns["DonorShortName"].Ordinal].ToString();
                return Convert.ToInt64(donorTable.Rows[0].ItemArray[donorTable.Columns["DonorKey"].Ordinal]);
            }
            else if (donorTable.Rows.Count > 1)
            {
                ADonorShortName = "DUPLICATE_DONORS_BANKACCOUNT";
            }

            return -1;
        }
    }
}