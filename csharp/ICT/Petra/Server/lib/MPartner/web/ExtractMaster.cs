//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Server.MPartner.Mailroom.Data.Access;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MCommon.Data.Cascading;
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MPartner.Partner.WebConnectors
{
    /// <summary>
    /// methods for extract master list
    /// </summary>
    public class TExtractMasterWebConnector
    {
        /// <summary>
        /// retrieve all extract master records
        /// </summary>
        /// <returns>returns table filled with all extract headers</returns>
        [RequireModulePermission("PTNRUSER")]
        public static MExtractMasterTable GetAllExtractHeaders()
        {
            MExtractMasterTable ExtractMasterDT;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            ExtractMasterDT = MExtractMasterAccess.LoadAll(Transaction);

            DBAccess.GDBAccessObj.CommitTransaction();

            return ExtractMasterDT;
        }

        /// <summary>
        /// retrieve all extract master records
        /// </summary>
        /// <param name="AExtractId"></param>
        /// <returns>returns true if deletion was successful</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean DeleteExtract(int AExtractId)
        {
            Boolean ReturnValue = true;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            MExtractMasterCascading.DeleteByPrimaryKey(AExtractId, Transaction, true);

            DBAccess.GDBAccessObj.CommitTransaction();

            return ReturnValue;
        }

        /// <summary>
        /// check if extract with given name already exists
        /// </summary>
        /// <param name="AExtractName"></param>
        /// <returns>returns true if extract already exists</returns>
        [RequireModulePermission("PTNRUSER")]
        public static Boolean ExtractExists(String AExtractName)
        {
            MExtractMasterTable TemplateTable;
            MExtractMasterRow TemplateRow;
            Boolean ReturnValue = true;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            TemplateTable = new MExtractMasterTable();
            TemplateRow = TemplateTable.NewRowTyped(false);
            TemplateRow.ExtractName = AExtractName;

            if (MExtractMasterAccess.CountUsingTemplate(TemplateRow, null, Transaction) == 0)
            {
                ReturnValue = false;
            }

            DBAccess.GDBAccessObj.CommitTransaction();

            return ReturnValue;
        }
    }
}