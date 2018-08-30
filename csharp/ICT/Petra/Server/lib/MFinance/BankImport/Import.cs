//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <tp@tbits.net>
//
// Copyright 2004-2018 by OM International
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Xml;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.IO;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.BankImport.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MFinance.BankImport.Data.Access;
using Ict.Petra.Server.App.Core;

namespace Ict.Petra.Server.MFinance.BankImport.Logic
{
    /// <summary>
    /// import a bank statement from a file
    /// </summary>
    public class TBankStatementImport
    {
        /// <summary>
        /// upload new bank statement so that it can be used for matching etc.
        /// </summary>
        public static TSubmitChangesResult StoreNewBankStatement(BankImportTDS AStatementAndTransactionsDS,
            out Int32 AFirstStatementKey)
        {
            string MyClientID = DomainManager.GClientID.ToString();

            AFirstStatementKey = -1;

            TProgressTracker.InitProgressTracker(MyClientID,
                Catalog.GetString("Processing new bank statements"),
                AStatementAndTransactionsDS.AEpStatement.Rows.Count + 1);

            TProgressTracker.SetCurrentState(MyClientID,
                Catalog.GetString("Saving to database"),
                0);

            try
            {
                // Must not throw away the changes because we need the correct statement keys
                AStatementAndTransactionsDS.DontThrowAwayAfterSubmitChanges = true;
                BankImportTDSAccess.SubmitChanges(AStatementAndTransactionsDS);

                AFirstStatementKey = -1;

                if (AStatementAndTransactionsDS != null)
                {
                    TProgressTracker.SetCurrentState(MyClientID,
                        Catalog.GetString("starting to train"),
                        1);

                    AFirstStatementKey = AStatementAndTransactionsDS.AEpStatement[0].StatementKey;

                    // search for already posted gift batches, and do the matching for these imported statements
                    TBankImportMatching.Train(AStatementAndTransactionsDS.AEpStatement);
                }

                TProgressTracker.FinishJob(MyClientID);
            }
            catch (Exception ex)
            {
                TLogging.Log(ex.ToString());
                TProgressTracker.CancelJob(MyClientID);
                return TSubmitChangesResult.scrError;
            }

            return TSubmitChangesResult.scrOK;
        }
    }
}
