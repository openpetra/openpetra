/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
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
using System;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Collections.Generic;
using Ict.Common.DB;
using Ict.Common;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;

namespace Ict.Petra.Plugins.SQL
{
    public class TSQLTools
    {
        public static string GetBestEmailAddress(Int64 APartnerKey)
        {
            string EmailAddress = "";
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

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
                    EmailAddress = row.EmailAddress;

                    // just if wanted the post address, we would need to load the p_location table:
                    // PLocationAccess.LoadByPrimaryKey(out LocationTable, row.SiteKey, row.LocationKey, Transaction);
                }
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
            return EmailAddress;
        }

        /// <summary>
        /// get the best address in a Dataset (contains tables PPartnerLocation and PLocation)
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <returns></returns>
        public static DataSet GetBestAddress(Int64 APartnerKey)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

            DataSet PartnerLocationsDS = new DataSet();

            PartnerLocationsDS.Tables.Add(new PPartnerLocationTable());
            PartnerLocationsDS.Tables.Add(new PLocationTable());
            DataTable PartnerLocationTable = PartnerLocationsDS.Tables[PPartnerLocationTable.GetTableName()];

            // add special column BestAddress and Icon
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("BestAddress", typeof(Boolean)));
            PartnerLocationTable.Columns.Add(new System.Data.DataColumn("Icon", typeof(Int32)));

            // find all locations of the partner, put it into a dataset
            PPartnerLocationAccess.LoadViaPPartner(PartnerLocationsDS, APartnerKey, Transaction);

            // TODO: parameter for current date?
            Ict.Petra.Shared.MPartner.Calculations.DeterminePartnerLocationsDateStatus(PartnerLocationsDS);
            Ict.Petra.Shared.MPartner.Calculations.DetermineBestAddress(PartnerLocationsDS);

            int Counter = 0;

            while (Counter < PartnerLocationTable.Rows.Count)
            {
                PPartnerLocationRow row = (PPartnerLocationRow)PartnerLocationTable.Rows[Counter];

                // find the row with the best address
                if (Convert.ToInt32(row["BestAddress"]) == 1)
                {
                    // for the post address, we need to load the p_location table:
                    PLocationAccess.LoadByPrimaryKey(PartnerLocationsDS, row.SiteKey, row.LocationKey, Transaction);
                    Counter++;
                }
                else
                {
                    // delete the other addresses
                    PartnerLocationTable.Rows.Remove(row);
                }
            }

            DBAccess.GDBAccessObj.RollbackTransaction();
            return PartnerLocationsDS;
        }

        /// <summary>
        /// get the best address for all partners in the dataset
        /// </summary>
        /// <param name="ADataSet"></param>
        /// <param name="ATableNameWithPartnerKeys"></param>
        /// <param name="AColumnNameWithPartnerKeys"></param>
        public static void GetBestAddress(DataSet ADataSet, string ATableNameWithPartnerKeys, string AColumnNameWithPartnerKeys)
        {
            foreach (DataRow row in ADataSet.Tables[ATableNameWithPartnerKeys].Rows)
            {
                ADataSet.Merge(GetBestAddress(Convert.ToInt64(row[AColumnNameWithPartnerKeys])));
                
        		PPartnerLocationTable PartnerLocationTable = (PPartnerLocationTable)ADataSet.Tables[PPartnerLocationTable.GetTableName()];
        		PLocationTable LocationTable = (PLocationTable)ADataSet.Tables[PLocationTable.GetTableName()];
        
        		PartnerLocationTable.InitVars();       
        		LocationTable.InitVars();
            }
        }
    }
}