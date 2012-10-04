//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Configuration;
using System.IO;
using System.Collections.Generic;
using Ict.Testing.NUnitPetraServer;
using Ict.Petra.Server.App.Core;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Server.MFinance.Reporting.WebConnectors;


namespace Ict.Petra.Tools.MFinance.Server.GDPdUExport
{
    /// This will export the finance data for the tax office, according to GDPdU
    public class TGDPdUExport
    {
        /// main method
        public static void Main(string[] args)
        {
            TPetraServerConnector.Connect("../../etc/TestServer.config");
            
            try
            {
                string OutputPath = TAppSettingsManager.GetValue("OutputPath", "../../delivery/GDPdU/data");
                
                if (!Directory.Exists(OutputPath))
                {
                    Directory.CreateDirectory(OutputPath);
                }
                
                string SummaryCostCentres = TAppSettingsManager.GetValue("SummaryCostCentres", "4300S");
                int FinancialYear = TAppSettingsManager.GetInt32("FinancialYearNumber", 0);
                int FirstFinancialYear = TAppSettingsManager.GetInt32("FirstFinancialYear", DateTime.Now.Year);
                int LedgerNumber = TAppSettingsManager.GetInt32("LedgerNumber", 43);
                char CSVSeparator = TAppSettingsManager.GetValue("CSVSeparator", ";")[0];
                string NewLine = "\r\n";
                string culture = TAppSettingsManager.GetValue("culture", "de-DE");
                string operation = TAppSettingsManager.GetValue("operation", "all");
                
                string ReportingCostCentres = 
                    TFinanceReportingWebConnector.GetReportingCostCentres(LedgerNumber, SummaryCostCentres);
                
                // set decimal separator, and thousands separator
                Ict.Common.Catalog.SetCulture(culture);
            
                string OutputPathForYear = Path.Combine(OutputPath, (FirstFinancialYear + FinancialYear).ToString());
                
                if (!Directory.Exists(OutputPathForYear))
                {
                    Directory.CreateDirectory(OutputPathForYear);
                }
                
                if (operation == "all" || operation == "costcentre")
                {
                    TGDPdUExportAccountsAndCostCentres.ExportCostCentres(OutputPath, CSVSeparator, NewLine, LedgerNumber, ReportingCostCentres);
                }
                
                if (operation == "all" || operation == "account")
                {
                    TGDPdUExportAccountsAndCostCentres.ExportAccounts(OutputPath, CSVSeparator, NewLine, LedgerNumber, ReportingCostCentres);
                }
                
                if (operation == "all" || operation == "transaction")
                {
                    TGDPdUExportTransactions.ExportGLTransactions(OutputPathForYear, CSVSeparator, NewLine, LedgerNumber, FinancialYear, ReportingCostCentres);
                }
                
                if (operation == "all" || operation == "balance")
                {
                    TGDPdUExportBalances.ExportGLBalances(OutputPathForYear, CSVSeparator, NewLine, LedgerNumber, FinancialYear, ReportingCostCentres);
                }
                
            }
            catch (Exception e)
            {
                TLogging.Log(e.ToString());
            }
            
            if (TAppSettingsManager.GetValue("interactive", "true") == "true")
            {
                Console.WriteLine("Please press Enter to continue...");
                Console.ReadLine();
            }
        }
    }
}