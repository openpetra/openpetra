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
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;

namespace Ict.Petra.Client.MPartner.Logic
{
    /// <summary>
    /// get all the data from the database
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
        /// return a table with FAMILY partner keys that should be checked for duplicates
        /// </summary>
        public static PPartnerTable GetPartnersToCheck()
        {
            DataSet TempDS = new DataSet();
            PPartnerTable Partners = new PPartnerTable();

            TempDS.Tables.Add(Partners);
            Partners.Constraints.Clear();

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                string stmt = TDataBase.ReadSqlFile(TAppSettingsManager.GetValueStatic("sqlPath") + "GetPartnersToCheck.sql");

                DBAccess.GDBAccessObj.Select(TempDS, stmt, Partners.TableName, transaction);
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return Partners;
        }

        /// <summary>
        /// check if this partner has duplicates are very close matches
        /// </summary>
        /// <param name="APartner"></param>
        /// <returns></returns>
        public static PPartnerTable CheckDuplicates(PPartnerRow APartner)
        {
            DataSet TempDS = new DataSet();
            PPartnerTable Partners = new PPartnerTable();

            TempDS.Tables.Add(Partners);
            Partners.Constraints.Clear();

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                string stmt = TDataBase.ReadSqlFile(TAppSettingsManager.GetValueStatic("sqlPath") + "GetDuplicateName.sql");

                List <OdbcParameter>Parameters = new List <OdbcParameter>();
                Parameters.Add(new OdbcParameter("partnerkey", APartner.PartnerKey));
                Parameters.Add(new OdbcParameter("shortname", APartner.PartnerShortName));

                DBAccess.GDBAccessObj.Select(TempDS, stmt, Partners.TableName, transaction, Parameters.ToArray());
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return Partners;
        }

        /// <summary>
        /// get the address that should be used today for this partner
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="AAddress"></param>
        /// <returns></returns>
        public static string GetBestAddress(Int64 APartnerKey, out PLocationTable AAddress)
        {
            string EmailAddress = "";
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

            AAddress = null;

            DataSet PartnerLocationsDS = new DataSet();

            PartnerLocationsDS.Tables.Add(new PPartnerLocationTable());
            DataTable PartnerLocationTable = PartnerLocationsDS.Tables[PPartnerLocationTable.GetTableName()];

            // add special column BestAddress and Icon
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("BestAddress", typeof(Boolean)));
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("Icon", typeof(Int32)));

            // find all locations of the partner, put it into a dataset
            PPartnerLocationAccess.LoadViaPPartner(PartnerLocationsDS, APartnerKey, Transaction);

            Ict.Petra.Shared.MPartner.Calculations.DeterminePartnerLocationsDateStatus(PartnerLocationsDS);
            Ict.Petra.Shared.MPartner.Calculations.DetermineBestAddress(PartnerLocationsDS);

            foreach (PPartnerLocationRow row in PartnerLocationTable.Rows)
            {
                // find the row with BestAddress = 1
                if (Convert.ToInt32(row["BestAddress"]) == 1)
                {
                    if (!row.IsEmailAddressNull())
                    {
                        EmailAddress = row.EmailAddress;
                    }

                    // we also want the post address, need to load the p_location table:
                    AAddress = PLocationAccess.LoadByPrimaryKey(row.SiteKey, row.LocationKey, Transaction);
                }
            }

            DBAccess.GDBAccessObj.RollbackTransaction();

            return EmailAddress;
        }
    }
}