//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Tim Ingham
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
using Ict.Petra.Server.App.Core.Security;

namespace Ict.Petra.Server.MSysMan.IntranetExport.WebConnectors
{
    /// <summary>
    /// 
    /// </summary>
    public class TIntranetExportWebConnector
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExportDonationData"></param>
        /// <param name="ExportFieldData"></param>
        /// <param name="ExportPersonData"></param>
        /// <param name="Pswd"></param>
        /// <param name="DaySpan"></param>
        /// <param name="OptionalMetadata"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static String ExportToFile(Boolean ExportDonationData, Boolean ExportFieldData, Boolean ExportPersonData,
            String Pswd, Int32 DaySpan, String OptionalMetadata)
        {
            return "This string returned from the server.";
        }
    }
}
