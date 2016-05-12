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
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Server.MReporting;
using System.Data.Odbc;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.IO; // Implicit reference
using Ict.Common.Remoting.Server;
using Ict.Petra.Server.MReporting.MFinance;
using Ict.Petra.Server.MReporting.MFinDev;
using Ict.Petra.Server.MReporting.MPartner;
using Ict.Petra.Server.MReporting.MPersonnel;
using Ict.Petra.Server.MReporting.MConference;
using System.IO;

namespace Ict.Petra.Server.MReporting.Calculator
{
    /// <summary>
    /// calculate a report
    /// </summary>
    public class TRptDataCalculator : TRptSituation
    {
        /// <summary>where to search for the standard reports (xml files)</summary>
        protected String FPathStandardReports;

        /// <summary>where to search for the custom reports (xml files)</summary>
        protected String FPathCustomReports;

        static bool HasBeenInitialized = false;
        private static void InitializeUnit()
        {
            TRptEvaluator.AddUserFunctions(typeof(TRptUserFunctionsFinance));
            TRptEvaluator.AddUserFunctions(typeof(TRptUserFunctionsDate));
            TRptEvaluator.AddUserFunctions(typeof(TRptUserFunctionsFinDev));
            TRptEvaluator.AddUserFunctions(typeof(TRptUserFunctionsPartner));
            TRptEvaluator.AddUserFunctions(typeof(TRptUserFunctionsPersonnel));
            TRptEvaluator.AddUserFunctions(typeof(TRptUserFunctionsConference));
            HasBeenInitialized = true;
        }

        /// <summary>
        /// @param connection The System.Object that represents the established ODBC connection to the Petra Database
        /// </summary>
        /// <param name="connection">The System.Object that represents the established ODBC connection to the Petra Database
        /// </param>
        /// <param name="APathStandardReports"></param>
        /// <param name="APathCustomReports"></param>
        public TRptDataCalculator(TDataBase connection, String APathStandardReports, String APathCustomReports)
            : base(new TParameterList(), new TResultList(), new TReportStore(), null, connection, 0, -1, -1, -1)
        {
            if (!HasBeenInitialized)
            {
                InitializeUnit();
            }

            FPathStandardReports = APathStandardReports;
            FPathCustomReports = APathCustomReports;
        }

        /// <summary>
        /// this is where all the calculations take place
        /// </summary>
        /// <returns>true if the report was successfully generated
        /// </returns>
        public Boolean GenerateResult(ref TParameterList parameterlist,
            ref TResultList resultlist,
            ref String AErrorMessage,
            ref Exception AException)
        {
            Boolean ReturnValue = false;

            if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_REPORTING)
            {
                // for timing of reports
                TLogging.Log("start calculating", TLoggingType.ToLogfile);
            }

            AErrorMessage = "";
            AException = null;

            try
            {
                this.Parameters = parameterlist;

                if (!Parameters.Exists("calculateFromMethod"))
                {
                    LoadReportDefinitionFiles(Parameters.Get("xmlfiles").ToString());
                    this.CurrentReport = this.ReportStore.Get(Parameters.Get("currentReport").ToString());

                    if (this.CurrentReport == null)
                    {
                        TLogging.Log("report \"" + Parameters.Get("currentReport").ToString() + "\" could not be found. XML file missing?");
                        return false;
                    }

                    InitColumns();
                    InitColumnsFormat();
                }

                InitParameterLedgers();

                if (Parameters.Get("param_multiperiod").ToBool())
                {
                    InitMultiPeriodColumns();
                }

                Results.SetMaxDisplayColumns(Parameters.Get("MaxDisplayColumns").ToInt());

                if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_REPORTING)
                {
                    Parameters.Save(Path.GetDirectoryName(TSrvSetting.ServerLogFile) + Path.DirectorySeparatorChar + "LogParamAfterPreproc.xml", true);
                }

                // to avoid still having in the status line: loading common.xml, although he is already working on the report
                TLogging.Log("Preparing data for the report... ", TLoggingType.ToStatusBar);

                if (Calculate())
                {
                    if (Parameters.Get("CancelReportCalculation").ToBool() == true)
                    {
                        AErrorMessage = "Report calculation was cancelled";
                        return false;
                    }

                    resultlist = this.Results;

                    if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_REPORTING)
                    {
                        string FilePath = Path.GetDirectoryName(TSrvSetting.ServerLogFile) + Path.DirectorySeparatorChar;
                        Parameters.Save(FilePath + "LogParamAfterCalculation.xml", true);
                        Results.WriteCSV(Parameters, FilePath + Path.DirectorySeparatorChar + "ReportResults.csv", ",", true, false);
                    }

                    ReturnValue = true;
                }
            }
            catch (Exception Exc)
            {
                TLogging.Log(Exc.ToString());
                TLogging.Log(Exc.StackTrace);

                if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_REPORTING)
                {
                    Parameters.Save(Path.GetDirectoryName(TSrvSetting.ServerLogFile) + Path.DirectorySeparatorChar + "LogAfterException.xml", true);
                }

                Console.WriteLine(Exc.StackTrace);

                AErrorMessage = Exc.Message;
                AException = Exc;
            }

            if (TLogging.DebugLevel >= TLogging.DEBUGLEVEL_REPORTING)
            {
                // for timing of reports
                TLogging.Log("finished calculating", TLoggingType.ToLogfile);
            }

            return ReturnValue;
        }

        private SortedList <string, Assembly>FReportAssemblies = new SortedList <string, Assembly>();

        /// <summary>
        /// as an alternative to calculate reports from an xml file, you can also write a method now that calculates the result for a report or extract
        /// </summary>
        /// <param name="ANamespaceClassAndMethodName"></param>
        /// <returns></returns>
        protected Boolean CalculateFromMethod(string ANamespaceClassAndMethodName)
        {
            string methodName = ANamespaceClassAndMethodName.Substring(ANamespaceClassAndMethodName.LastIndexOf(".") + 1);

            ANamespaceClassAndMethodName = ANamespaceClassAndMethodName.Substring(0, ANamespaceClassAndMethodName.LastIndexOf("."));
            string className = ANamespaceClassAndMethodName.Substring(ANamespaceClassAndMethodName.LastIndexOf(".") + 1);
            string namespaceName = ANamespaceClassAndMethodName.Substring(0, ANamespaceClassAndMethodName.LastIndexOf("."));

            if (!FReportAssemblies.Keys.Contains(namespaceName))
            {
                // work around dlls containing several namespaces, eg Ict.Petra.Client.MFinance.Gui contains AR as well
                string DllName = (TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + namespaceName).ToString().
                                 Replace("Ict.Petra.Server.", "Ict.Petra.Server.lib.");

                if (!System.IO.File.Exists(DllName + ".dll"))
                {
                    DllName = DllName.Substring(0, DllName.LastIndexOf("."));
                }

                try
                {
                    FReportAssemblies.Add(namespaceName, Assembly.LoadFrom(DllName + ".dll"));
                }
                catch (Exception exp)
                {
                    throw new Exception("error loading assembly " + namespaceName + ".dll: " + exp.Message);
                }
            }

            Assembly asm = FReportAssemblies[namespaceName];

            System.Type classType = asm.GetType(namespaceName + "." + className);

            if (classType == null)
            {
                throw new Exception("cannot find class " + namespaceName + "." + className + " for method " + methodName);
            }

            MethodInfo method = classType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);

            if (method != null)
            {
                return (bool)method.Invoke(null, new object[] { this.Parameters, this.Results });
            }
            else
            {
                throw new Exception("cannot find method " + className + "." + methodName);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        protected Boolean Calculate()
        {
            this.Results.Clear();

            if (Parameters.Exists("calculateFromMethod"))
            {
                if (!CalculateFromMethod(Parameters.Get("calculateFromMethod").ToString()))
                {
                    TLogging.Log(TLogging.LOG_PREFIX_ERROR + "Could not calculate from method (or report was cancelled).");
                    return false;
                }
            }
            else
            {
                TRptDataCalcHeaderFooter calcHeaderFooter = new TRptDataCalcHeaderFooter(this, -1, -1, 0, 0);
                calcHeaderFooter.Calculate(CurrentReport.pagefield, CurrentReport.pageswitch);
                InitColumnCaptions();
                TRptDataCalcLevel calclevel = new TRptDataCalcLevel(this, 0, -1, 0, 0);

                if ((calclevel.Calculate(CurrentReport.GetLevel("main"), 0) == -1) || (Parameters.Get("CancelReportCalculation").ToBool() == true))
                {
                    TLogging.Log(TLogging.LOG_PREFIX_ERROR + "Could not calculate main level (or report was cancelled).");
                    return false;
                }

                InitColumnLayout();
            }

            // call after calculating, because new parameters will be added
            InitDetailReports();

            return true;
        }

        /// <summary>
        /// load the xml files that define the report
        /// </summary>
        /// <param name="xmlfiles">a comma separated list of file names
        /// </param>
        /// <returns>void</returns>
        protected bool LoadReportDefinitionFiles(String xmlfiles)
        {
            string xmlfile;
            TReportParser reportParser;

            if (xmlfiles.Length == 0)
            {
                throw new Exception("No xmlfile defined to be loaded");
            }

            xmlfiles = xmlfiles.Replace("\\", "/");
            xmlfile = StringHelper.GetNextCSV(ref xmlfiles).Trim();

            while (xmlfile.Length != 0)
            {
                TLogging.Log("Loading " + xmlfile, TLoggingType.ToStatusBar);

                if (!System.IO.File.Exists(xmlfile) && System.IO.File.Exists(FPathCustomReports + '/' + xmlfile))
                {
                    xmlfile = FPathCustomReports + '/' + xmlfile;
                }

                if (!System.IO.File.Exists(xmlfile) && System.IO.File.Exists(FPathStandardReports + '/' + xmlfile))
                {
                    xmlfile = FPathStandardReports + '/' + xmlfile;
                }

                if (!System.IO.File.Exists(xmlfile))
                {
                    throw new Exception("Error: Cannot find the xml file " + xmlfile + " in " + FPathStandardReports + " or " + FPathCustomReports);
                }

                reportParser = new TReportParser(xmlfile);
                reportParser.ParseDocument(ref ReportStore);
                reportParser = null;
                xmlfile = StringHelper.GetNextCSV(ref xmlfiles).Trim();
            }

            return true;
        }

        /// <summary>
        /// sets the currentYear, the currentPeriod, the number of accounting and forwarding periods of the given ledger
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void InitParameterLedger(int ledgernr, int column)
        {
            int numberAccountingPeriods;
            int numberForwardingPeriods;
            int currentPeriod;
            int currentYear;
            int diffYear;
            string strSql;
            DataTable tab;
            DataRow row;

            // Get the number of accounting and forwarding periods from the given ledger
            strSql = "SELECT a_number_fwd_posting_periods_i, a_number_of_accounting_periods_i, a_current_period_i, a_current_financial_year_i " +
                     "FROM PUB_a_ledger WHERE a_ledger_number_i = " + ledgernr.ToString();
            tab = DatabaseConnection.SelectDT(strSql, "InitParameterLedger_TempTable", DatabaseConnection.Transaction);

            if (tab.Rows.Count == 1)
            {
                row = tab.Rows[0];
                numberForwardingPeriods = Convert.ToInt16(row["a_number_fwd_posting_periods_i"]);
                numberAccountingPeriods = Convert.ToInt16(row["a_number_of_accounting_periods_i"]);
                currentPeriod = Convert.ToInt16(row["a_current_period_i"]);
                currentYear = Convert.ToInt16(row["a_current_financial_year_i"]);
                Parameters.Add("param_ledger_number_i", new TVariant(ledgernr), column, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                Parameters.Add("param_current_period_i", new TVariant(currentPeriod), column, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                Parameters.Add("param_current_financial_year_i", new TVariant(
                        currentYear), column, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                Parameters.Add("param_number_of_accounting_periods_i", new TVariant(
                        numberAccountingPeriods), column, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                Parameters.Add("param_number_fwd_posting_periods_i", new TVariant(
                        numberForwardingPeriods), column, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);

                // which year has been selected for the standard ledger?
                diffYear = Parameters.Get("param_current_financial_year_i").ToInt() - Parameters.Get("param_year_i").ToInt();

                // apply same difference to this ledger; assume that both ledgers are in the current calendar year
                Parameters.Add("param_year_i", currentYear - diffYear, column, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);

                // diffperiod: not done at the moment; would be quite hard to set the different financial period differences anywhere
            }
            else
            {
                throw new Exception("Data for ledger " + ledgernr.ToString() + " not found.");
            }
        }

        /// <summary>
        /// overload; same ledger for all columns
        /// </summary>
        /// <param name="ledgernr"></param>
        protected void InitParameterLedger(int ledgernr)
        {
            InitParameterLedger(ledgernr, -1);
        }

        /// <summary>
        /// init the settings of the ledgers; will use the parameters param_ledger_number_i for each column and the overall ledger (column -1)
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void InitParameterLedgers()
        {
            System.Int32 column;
            System.Int32 ledgerNumber;
            System.Int32 otherLedgerNumber;
            ledgerNumber = -1;
            otherLedgerNumber = -1;

            if (Parameters.Exists("param_ledger_number_i", -1, -1, eParameterFit.eExact))
            {
                ledgerNumber = Parameters.Get("param_ledger_number_i", -1, -1, eParameterFit.eExact).ToInt();
                InitParameterLedger(ledgerNumber);
            }

            column = 1;

            while (Parameters.Exists("param_calculation", column, -1, eParameterFit.eExact))
            {
                if (Parameters.Exists("param_ledger_number_i", column, -1, eParameterFit.eExact))
                {
                    otherLedgerNumber = Parameters.Get("param_ledger_number_i", column, -1, eParameterFit.eExact).ToInt();

                    if (ledgerNumber == -1)
                    {
                        ledgerNumber = otherLedgerNumber;
                    }

                    InitParameterLedger(otherLedgerNumber, column);
                }

                column = column + 1;
            }

            if ((ledgerNumber != otherLedgerNumber) && (otherLedgerNumber != -1))
            {
                Parameters.Add("use_several_ledgers", new TVariant(true), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
            }
        }

        /// <summary>
        /// If this report was started with an incomplete (or new) settings file,
        /// add all available calculations as columns where strReturns not equals 'row'.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void InitColumns()
        {
            Int32 counterColumn;

            if ((!Parameters.Exists("MaxDisplayColumns") || (Parameters.Get("MaxDisplayColumns").ToInt() < 1)))
            {
                // this report was started with an incomplete (or new) settings file.
                // Add all available calculations as columns where strReturns = 'text' or 'amount'
                counterColumn = 0;

                if (CurrentReport.rptGrpCalculation != null)
                {
                    foreach (TRptCalculation calc in CurrentReport.rptGrpCalculation)
                    {
                        if ((calc.strReturns == "text") || (calc.strReturns == "amount"))
                        {
                            Parameters.Add("param_calculation", new TVariant(calc.strId), counterColumn);
                            counterColumn = counterColumn + 1;
                        }
                    }
                }

                Parameters.Add("MaxDisplayColumns", new TVariant(counterColumn));
                Results.SetMaxDisplayColumns(counterColumn);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="columnNr"></param>
        protected void InitColumnFormat(Int32 columnNr)
        {
            TRptCalculation rptCalculation;
            String calculation;
            Int32 columnReferenced;
            String format;
            Int32 posColumn;

            format = "";

            // if the format already has been set, leave it
            if (Parameters.Exists("ColumnFormat", columnNr, -1, eParameterFit.eExact))
            {
                return;
            }

            calculation = Parameters.Get("param_calculation", columnNr).ToString();

            // check the calculation for the format
            rptCalculation = ReportStore.GetCalculation(CurrentReport, calculation);

            if (rptCalculation != null)
            {
                format = rptCalculation.strReturnsFormat;
            }
            else if (calculation.IndexOf("column(") != -1)
            {
                // if it is a calculation based on other columns, get the format of the first column
                posColumn = calculation.IndexOf("column(") + 7;
                columnReferenced = Convert.ToInt32(calculation.Substring(posColumn, calculation.IndexOf(')', posColumn) - posColumn));
                format = Parameters.Get("ColumnFormat", columnReferenced).ToString();
            }

            if (format.Length != 0)
            {
                Parameters.Add("ColumnFormat", new TVariant(format), columnNr);
            }
        }

        /// <summary>
        /// can only be called after generateResult, or when the xml files have been loaded, because it collects the format from the calculation, whis is defined in the report defition file
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void InitColumnsFormat()
        {
            Int32 counterColumn;

            counterColumn = Results.GetMaxDisplayColumns();

            // init the format of the columns that are not visible (beyond the maxDisplayColumn value)
            while (Parameters.Exists("param_calculation", counterColumn, -1, eParameterFit.eExact))
            {
                InitColumnFormat(counterColumn);
                counterColumn = counterColumn + 1;
            }

            for (counterColumn = 0; counterColumn <= Results.GetMaxDisplayColumns() - 1; counterColumn += 1)
            {
                InitColumnFormat(counterColumn);
            }
        }

        /// <summary>
        /// calculate the positions for the columns
        ///
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void InitColumnLayout()
        {
            System.Int32 lowestlevel;
            double MostRightPosition;
            double MostRightPosition2;
            System.Int32 Counter;
            String ColumnFormat;
            TVariant ColumnPosition;
            TVariant ColumnPositionIndented;
            lowestlevel = this.Results.GetDeepestVisibleLevel();
            Parameters.Add("lowestLevel", new TVariant(lowestlevel), -1, ReportingConsts.APPLICATIONPARAMETERS);

            // use the columns of the first subreport (it always exists) to calculate the position of the columns and the width of the report
            // but setting parameters has to be done in the global settings (subreport = 1)
            Parameters.Add("CurrentSubReport", new TVariant(0), -1);

            // get the most right position of the descr columns of the lowest level
            MostRightPosition = 0;

            if (Parameters.Exists("ColumnPosition", ReportingConsts.COLUMNLEFT + 1, lowestlevel))
            {
                MostRightPosition = MostRightPosition + Parameters.Get("ColumnPosition", ReportingConsts.COLUMNLEFT + 1, lowestlevel).ToDouble();
            }

            if (Parameters.Exists("ColumnWidth", ReportingConsts.COLUMNLEFT + 1, lowestlevel))
            {
                MostRightPosition = MostRightPosition + Parameters.Get("ColumnWidth", ReportingConsts.COLUMNLEFT + 1, lowestlevel).ToDouble();
            }

            MostRightPosition2 = 0;

            if (Parameters.Exists("ColumnPosition", ReportingConsts.COLUMNLEFT + 2, lowestlevel))
            {
                MostRightPosition2 = MostRightPosition2 + Parameters.Get("ColumnPosition", ReportingConsts.COLUMNLEFT + 2, lowestlevel).ToDouble();
            }

            if (Parameters.Exists("ColumnWidth", ReportingConsts.COLUMNLEFT + 2, lowestlevel))
            {
                MostRightPosition2 = MostRightPosition2 + Parameters.Get("ColumnWidth", ReportingConsts.COLUMNLEFT + 2, lowestlevel).ToDouble();
            }

            if (MostRightPosition2 > MostRightPosition)
            {
                MostRightPosition = MostRightPosition2;
            }

            for (Counter = 0; Counter <= Results.GetMaxDisplayColumns() - 1; Counter += 1)
            {
                // space between columns
                MostRightPosition = MostRightPosition + 0.0;

                if (Parameters.Exists("ColumnPosition", Counter))
                {
                    MostRightPosition = Parameters.Get("ColumnPosition", Counter).ToDouble();
                }

                ColumnPosition = new TVariant(MostRightPosition);
                ColumnPositionIndented = Parameters.Get("ColumnPositionIndented", Counter);
                TVariant ColumnWidth = new TVariant();

                if (!Parameters.Exists("ColumnWidth", Counter))
                {
                    if (Parameters.Exists("ColumnFormat", Counter, -1, eParameterFit.eExact))
                    {
                        // todo: width of currency depends on thousandsonly, nodecimals, etc.
                        ColumnFormat = Parameters.Get("ColumnFormat", Counter, -1, eParameterFit.eExact).ToString();

                        // all measurements in cm
                        if (ColumnFormat == "percentage")
                        {
                            // was 2.0, but the caption needs space as well, so same width as currency
                            ColumnWidth = new TVariant(2.5);
                        }
                        else if (ColumnFormat == "currency")
                        {
                            ColumnWidth = new TVariant(2.5);
                        }
                        else
                        {
                            ColumnWidth = new TVariant(3);
                        }

                        ColumnPositionIndented = new TVariant(0.5);
                    }
                    else
                    {
                        ColumnWidth = new TVariant(4);
                    }
                }
                else
                {
                    ColumnWidth = Parameters.Get("ColumnWidth", Counter);
                }

                MostRightPosition = MostRightPosition + ColumnWidth.ToDouble();

                if (!ColumnPositionIndented.IsNil())
                {
                    MostRightPosition = MostRightPosition + ColumnPositionIndented.ToDouble();
                }

                // save the values to the global report level
                Parameters.Add("CurrentSubReport", new TVariant(-1), -1);
                Parameters.Add("ColumnPosition", new TVariant(ColumnPosition), Counter);
                Parameters.Add("ColumnWidth", new TVariant(ColumnWidth), Counter);

                if (!ColumnPositionIndented.IsNil())
                {
                    Parameters.Add("ColumnPositionIndented", ColumnPositionIndented, Counter);
                }

                Parameters.Add("CurrentSubReport", new TVariant(0), -1);
            }

            Parameters.Add("CurrentSubReport", new TVariant(-1), -1);
            Parameters.Add("ReportWidth", new TVariant(MostRightPosition));
        }

        /// <summary>
        /// This formats the caption of a column, into up to 3 rows
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void FormatCaption(String AParameterName, TVariant ACaption, System.Int32 AColumn)
        {
            int br;
            String caption;

            // v, newValue: TVariant;
            // Counter: integer;
            // TODO: client side formatting of column captions? e.g. month names should be localised
            // problems: there can be \n inside strings;
            // need to split strings by the \n
            caption = ACaption.ToString();
            br = caption.IndexOf("\\n");

            if (br != -1)
            {
                Parameters.Add(AParameterName, new TVariant(caption.Substring(0, br), true), AColumn);

                // Tlogging.Log(parameters.Get(AParameterName).EncodeToString());
                caption = caption.Substring(br + 2, caption.Length - br - 2);
                br = caption.IndexOf("\\n");

                if (br != -1)
                {
                    Parameters.Add(AParameterName + '2', new TVariant(caption.Substring(0, br), true), AColumn);

                    // Tlogging.Log(parameters.Get(AParameterName+'2').EncodeToString());
                    caption = caption.Substring(br + 2, caption.Length - br - 2);
                    Parameters.Add(AParameterName + '3', new TVariant(caption, true), AColumn);
                }
                else
                {
                    Parameters.Add(AParameterName + '2', new TVariant(caption, true), AColumn);
                }
            }
            else
            {
                Parameters.Add(AParameterName, new TVariant(caption, true), AColumn);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        protected void InitColumnCaptions()
        {
            TRptCalculation rptCalculation;
            TRptDataCalcValue rptDataCalcValue;
            int column;

            for (column = 0; column <= Results.GetMaxDisplayColumns() - 1; column += 1)
            {
                rptCalculation = ReportStore.GetCalculation(CurrentReport, Parameters.Get("param_calculation", column).ToString());

                if (rptCalculation != null)
                {
                    if (rptCalculation.strAlign.Length != 0)
                    {
                        Parameters.Add("ColumnAlign", new TVariant(rptCalculation.strAlign), column);
                    }
                    else
                    {
                        if (StringHelper.IsCurrencyFormatString(rptCalculation.strReturnsFormat)
                            || (rptCalculation.strReturnsFormat.ToLower().IndexOf("percentage") != -1))
                        {
                            Parameters.Add("ColumnAlign", new TVariant("right"), column);
                        }
                        else
                        {
                            Parameters.Add("ColumnAlign", new TVariant("left"), column);
                        }
                    }

                    if (rptCalculation.strReturnsFormat.Length != 0)
                    {
                        Parameters.Add("ColumnFormat", new TVariant(rptCalculation.strReturnsFormat), column, -1);
                    }

                    if (rptCalculation.rptGrpCaption != null)
                    {
                        rptDataCalcValue = new TRptDataCalcValue(this, -1, column, -1, 0);
                        FormatCaption("ColumnCaption", rptDataCalcValue.Calculate(rptCalculation.rptGrpCaption), column);
                    }

                    if (rptCalculation.rptGrpShortCaption != null)
                    {
                        rptDataCalcValue = new TRptDataCalcValue(this, -1, column, -1, 0);
                        FormatCaption("ColumnShortCaption", rptDataCalcValue.Calculate(rptCalculation.rptGrpShortCaption), column);
                    }
                }
            }
        }

        /// <summary>
        /// This procedure uses the calculation columns,
        /// and splits them each up into 12 columns, for each period
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void InitMultiPeriodColumns()
        {
            System.Int32 MaxDisplayColumns;
            System.Int32 NumberColumns;
            System.Int32 Counter;
            System.Int32 Month;
            System.Int32 NewColumn;
            TParameterList TempParameters;
            String Formula;
            System.Int32 CurrentPos;
            System.Int32 NumberPos;
            System.Int32 ColumnNumber;
            String Numberstring;

            // then each column must be written back, 12 times
            // the references must be changed to other columns
            // what about ledger sums, etc?
            MaxDisplayColumns = Parameters.Get("MaxDisplayColumns").ToInt();
            NumberColumns = MaxDisplayColumns;

            while (Parameters.Exists("param_calculation", NumberColumns))
            {
                NumberColumns = NumberColumns + 1;
            }

            // the current column calculation settings must be saved somewhere
            TempParameters = new TParameterList();

            for (Counter = 0; Counter <= NumberColumns - 1; Counter++)
            {
                TempParameters.Copy(Parameters, Counter, -1, eParameterFit.eBestFit, -1);
            }

            // clear the current column settings
            for (Counter = 0; Counter <= NumberColumns - 1; Counter++)
            {
                Parameters.RemoveColumn(Counter);
            }

            for (Counter = 0; Counter <= NumberColumns - 1; Counter++)
            {
                //TODO: Calendar vs Financial Date Handling - Check if this should use number of financial perids and not month
                for (Month = 1; Month <= 12; Month++)
                {
                    NewColumn = Counter * 13 + Month - 1;
                    Parameters.Copy(TempParameters, Counter, -1, eParameterFit.eExact, NewColumn);
                    Parameters.Add("param_column_period_i", new TVariant(Month), NewColumn);
                    Parameters.Add("param_start_period_i", new TVariant(Month), NewColumn);
                    Parameters.Add("param_end_period_i", new TVariant(Month), NewColumn);
                    Parameters.Add("param_ytd", new TVariant(false), NewColumn);

                    if (Parameters.Exists("FirstColumn", NewColumn))
                    {
                        Parameters.Add("FirstColumn", new TVariant(Parameters.Get("FirstColumn", NewColumn).ToInt() * 13 + Month - 1), NewColumn);
                    }

                    if (Parameters.Exists("SecondColumn", NewColumn))
                    {
                        Parameters.Add("SecondColumn", new TVariant(Parameters.Get("SecondColumn", NewColumn).ToInt() * 13 + Month - 1), NewColumn);
                    }

                    if (Parameters.Exists("param_formula", NewColumn))
                    {
                        Formula = Parameters.Get("param_formula", NewColumn).ToString();

                        // replace column(x) with column(mul(13,x))
                        CurrentPos = 0;
                        CurrentPos = Formula.ToLower().IndexOf("column(", CurrentPos);

                        while (CurrentPos != -1)
                        {
                            NumberPos = CurrentPos + ("column(").Length;
                            Numberstring = Formula.Substring(NumberPos, Formula.IndexOf(')', NumberPos) - NumberPos);
                            ColumnNumber = Convert.ToInt32(Numberstring);
                            Formula =
                                Formula.Substring(0,
                                    NumberPos) + "mul(13," + ColumnNumber.ToString() + ')' + Formula.Substring(Formula.IndexOf(')', NumberPos));
                            CurrentPos = Formula.ToLower().IndexOf("column(", CurrentPos + 1);
                        }

                        Parameters.Add("param_formula", new TVariant(Formula), NewColumn);
                    }
                }

                //TODO: Calendar vs Financial Date Handling - Check if this should use financial year/periods and not assume calendar
                // the ytd column
                NewColumn = Counter * 13 + 12;
                Parameters.Copy(TempParameters, Counter, -1, eParameterFit.eExact, NewColumn);
                Parameters.Add("param_column_period_i", new TVariant(12), NewColumn);
                Parameters.Add("param_ytd", new TVariant(true), NewColumn);

                // TODO: also apply the formula/ reference changes to columns!!!!
            }

            Parameters.Add("MaxDisplayColumns", NumberColumns * 13);
        }

        /// <summary>
        /// need to tell the client that there are detail reports available
        /// </summary>
        /// <returns>void</returns>
        protected void InitDetailReports()
        {
            int Counter = 0;
            String detailReportCSV;

            // remove all param_detail_report_ parameters first

            while (Parameters.Exists("param_detail_report_" + Counter.ToString()) == true)
            {
                Parameters.RemoveVariable("param_detail_report_" + Counter.ToString());
                Counter++;
            }

            Counter = 0;

            if ((CurrentReport != null) && (CurrentReport.rptGrpDetailReport != null))
            {
                foreach (TRptDetailReport detailReport in CurrentReport.rptGrpDetailReport)
                {
                    detailReportCSV = "";
                    detailReportCSV = StringHelper.AddCSV(detailReportCSV, detailReport.strId, ",");
                    detailReportCSV = StringHelper.AddCSV(detailReportCSV, detailReport.strAction, ",");
                    detailReportCSV = StringHelper.AddCSV(detailReportCSV, detailReport.strQuery, ",");

                    if (detailReport.rptGrpParameter != null)
                    {
                        foreach (TRptParameter parameter in detailReport.rptGrpParameter)
                        {
                            detailReportCSV = StringHelper.AddCSV(detailReportCSV, parameter.strName, ",");
                            detailReportCSV = StringHelper.AddCSV(detailReportCSV, parameter.strValue, ",");
                        }
                    }

                    Parameters.Add("param_detail_report_" + Counter.ToString(), detailReportCSV);
                    Counter++;
                }
            }

            // add a detail report for each partnerkey column
            // use columnFormat, because looking at the format "eInteger:partnerkey:" of the values of a row is not so easy
            for (int ColCounter = 0; ColCounter <= Parameters.Get("MaxDisplayColumns").ToInt() - 1; ColCounter++)
            {
                if (Parameters.Get("ColumnFormat", ColCounter).ToString().ToLower() == "partnerkey")
                {
                    detailReportCSV = "";
                    detailReportCSV = StringHelper.AddCSV(detailReportCSV, "Open \"" + Parameters.Get("ColumnCaption",
                            ColCounter).ToString() + "\" in Partner Edit Screen", ",");
                    detailReportCSV = StringHelper.AddCSV(detailReportCSV, "PartnerEditScreen", ",");
                    detailReportCSV = StringHelper.AddCSV(detailReportCSV, ColCounter.ToString(), ",");
                    Parameters.Add("param_detail_report_" + Counter.ToString(), detailReportCSV);
                    Counter++;
                }
            }
        }
    }
}