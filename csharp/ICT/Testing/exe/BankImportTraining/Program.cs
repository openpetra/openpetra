//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data.Odbc;
using NUnit.Framework;
using Ict.Testing.NUnitTools;
using Ict.Testing.NUnitPetraServer;
using Ict.Petra.Server.MFinance.GL;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.AP.WebConnectors;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Server.MFinance.ImportExport.WebConnectors;
using Ict.Petra.ClientPlugins.BankStatementImport.BankImportFromMT940;
using Ict.Petra.Shared.Interfaces.Plugins.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Testing.Finance.Bankimport
{
    /// <summary>
    /// do the bankimport training. job too long for http remoting at the moment
    /// </summary>
    internal sealed class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        private static void Main(string[] args)
        {
            // need to call with config file as parameter: -C:/home/USERNAME/etc/PetraServerConsole.config
            TPetraServerConnector.Connect();

            TBankStatementImport import = new TBankStatementImport();

            BankImportTDS StatementAndTransactionsDS = import.ImportBankStatementNonInteractive(
                TAppSettingsManager.GetInt32("ledger"),
                TAppSettingsManager.GetValue("bankaccount"),
                TAppSettingsManager.GetValue("file"));

            Int32 AFirstStatementKey;
            TBankImportWebConnector.StoreNewBankStatement(StatementAndTransactionsDS, out AFirstStatementKey);

            TPetraServerConnector.Disconnect();
        }
    }
}