//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, matthiash
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
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MPersonnel.Personnel;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Server.MPersonnel.Personnel.Data.Access;

namespace Ict.Petra.Server.MPersonnel.WebConnectors
{
    /// <summary>
    /// Description of Personnel.
    /// </summary>
    public class TPersonnelWebConnector
    {
        /// <summary>
        /// this will store PersonnelTDS
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("PERSONNEL")]
        public static TSubmitChangesResult SavePersonnelTDS(ref PersonnelTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            // TODO: calculate debit and credit sums for journal and batch?

            TSubmitChangesResult SubmissionResult = PersonnelTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);

            return SubmissionResult;
        }
    }
}