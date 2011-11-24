//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Printing;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MFinance.Gift.WebConnectors
{
    /// <summary>
    /// a letter for all donors of a specific worker
    /// </summary>
    public class TDonorsOfWorkerWebConnector
    {
        /// <summary>
        /// return a table with the details of people that donate for a specific worker
        /// </summary>
        [RequireModulePermission("FINANCE-1")]
        public static NewDonorTDS GetDonorsOfWorker(
            Int64 AWorkerPartnerKey,
            Int32 ALedgerNumber,
            bool ADropForeignAddresses,
            bool ADropPartnersWithNoMailing)
        {
            NewDonorTDS MainDS = new NewDonorTDS();

            TDBTransaction transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                string stmt = TDataBase.ReadSqlFile("Gift.GetDonorsOfWorker.sql");

                OdbcParameter parameter;

                List <OdbcParameter>parameters = new List <OdbcParameter>();
                parameter = new OdbcParameter("WorkerPartnerKey", OdbcType.BigInt);
                parameter.Value = AWorkerPartnerKey;
                parameters.Add(parameter);
                parameter = new OdbcParameter("LedgerNumber", OdbcType.Int);
                parameter.Value = ALedgerNumber;
                parameters.Add(parameter);

                DBAccess.GDBAccessObj.Select(MainDS, stmt, MainDS.AGift.TableName, transaction, parameters.ToArray());

                // drop all previous gifts, keep only the most recent one
                NewDonorTDSAGiftRow previousRow = null;

                Int32 rowCounter = 0;

                while (rowCounter < MainDS.AGift.Rows.Count)
                {
                    NewDonorTDSAGiftRow row = MainDS.AGift[rowCounter];

                    if ((previousRow != null) && (previousRow.DonorKey == row.DonorKey))
                    {
                        MainDS.AGift.Rows.Remove(previousRow);
                    }
                    else
                    {
                        rowCounter++;
                    }

                    previousRow = row;
                }

                // get recipient description
                foreach (NewDonorTDSAGiftRow row in MainDS.AGift.Rows)
                {
                    if (row.RecipientDescription.Length == 0)
                    {
                        row.RecipientDescription = row.MotivationGroupCode + "/" + row.MotivationDetailCode;
                    }
                }

                // best address for each partner
                DataUtilities.CopyTo(
                    TAddressTools.AddPostalAddress(MainDS.AGift,
                        MainDS.AGift.ColumnDonorKey,
                        ADropForeignAddresses,
                        ADropPartnersWithNoMailing,
                        transaction),
                    MainDS.BestAddress);

                // remove all invalid addresses
                rowCounter = 0;

                while (rowCounter < MainDS.BestAddress.Rows.Count)
                {
                    BestAddressTDSLocationRow row = MainDS.BestAddress[rowCounter];

                    if (!row.ValidAddress)
                    {
                        MainDS.BestAddress.Rows.Remove(row);
                    }
                    else
                    {
                        rowCounter++;
                    }
                }
            }
            finally
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return MainDS;
        }
    }
}