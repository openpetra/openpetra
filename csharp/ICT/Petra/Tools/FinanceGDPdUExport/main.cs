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
                string IgnoreCostCentres = TAppSettingsManager.GetValue("IgnoreCostCentres", "xyz");
                string IgnoreAccounts = TAppSettingsManager.GetValue("IgnoreAccounts", "4300S,GIFT");
                string IncludeAccounts = TAppSettingsManager.GetValue("IncludeAccounts", "4310");
                string FinancialYears = TAppSettingsManager.GetValue("FinancialYearNumber", "0");
                string IgnoreTransactionsByReference = TAppSettingsManager.GetValue("IgnoreReference", "L1,L2,L3,L4,L5,L6,L7,L8,L9,L10,L11,L12");
                int FirstFinancialYear = TAppSettingsManager.GetInt32("FirstFinancialYear", DateTime.Now.Year);
                int LedgerNumber = TAppSettingsManager.GetInt32("LedgerNumber", 43);
                char CSVSeparator = TAppSettingsManager.GetValue("CSVSeparator", ";")[0];
                string NewLine = "\r\n";
                string culture = TAppSettingsManager.GetValue("culture", "de-DE");
                string operation = TAppSettingsManager.GetValue("operation", "all");

                string ReportingCostCentres =
                    TFinanceReportingWebConnector.GetReportingCostCentres(LedgerNumber, SummaryCostCentres, IgnoreCostCentres);

                if (TAppSettingsManager.GetBoolean("IgnorePersonCostCentres", true))
                {
                    ReportingCostCentres = TGDPdUExportAccountsAndCostCentres.WithoutPersonCostCentres(LedgerNumber, ReportingCostCentres);
                }

                IgnoreAccounts =
                    TFinanceReportingWebConnector.GetReportingAccounts(LedgerNumber, IgnoreAccounts, IncludeAccounts);

                // set decimal separator, and thousands separator
                Ict.Common.Catalog.SetCulture(culture);

                List <string>CostCentresInvolved = new List <string>();
                List <string>AccountsInvolved = new List <string>();

                SortedList <string, string>TaxAnalysisAttributes = TGDPdUExportTransactions.GetTaxAnalysisAttributes();

                foreach (string FinancialYearString in FinancialYears.Split(new char[] { ',' }))
                {
                    Int32 FinancialYear = Convert.ToInt32(FinancialYearString);

                    string OutputPathForYear = Path.Combine(OutputPath, (FirstFinancialYear + FinancialYear).ToString());

                    if (!Directory.Exists(OutputPathForYear))
                    {
                        Directory.CreateDirectory(OutputPathForYear);
                    }

                    TGDPdUExportTransactions.ExportGLTransactions(OutputPathForYear,
                        CSVSeparator,
                        NewLine,
                        LedgerNumber,
                        FinancialYear,
                        ReportingCostCentres,
                        IgnoreAccounts,
                        IgnoreTransactionsByReference,
                        TaxAnalysisAttributes,
                        ref CostCentresInvolved,
                        ref AccountsInvolved);

                    TGDPdUExportBalances.ExportGLBalances(OutputPathForYear, CSVSeparator, NewLine, LedgerNumber,
                        FinancialYear, ReportingCostCentres,
                        IgnoreAccounts);

                    TGDPdUExportAccountsPayable.Export(OutputPathForYear, CSVSeparator, NewLine, LedgerNumber, FinancialYear, ReportingCostCentres);
                    TGDPdUExportParticipants.Export(OutputPathForYear, CSVSeparator, NewLine,
                        LedgerNumber, FinancialYear,
                        ReportingCostCentres);
                }

                TGDPdUExportAccountsAndCostCentres.ExportCostCentres(OutputPath, CSVSeparator, NewLine, LedgerNumber,
                    CostCentresInvolved);

                TGDPdUExportAccountsAndCostCentres.ExportAccounts(OutputPath, CSVSeparator, NewLine, LedgerNumber,
                    AccountsInvolved);

                TGDPdUExportTransactions.ExportTaxAnalysisAttributes(OutputPath, CSVSeparator, NewLine, TaxAnalysisAttributes);
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