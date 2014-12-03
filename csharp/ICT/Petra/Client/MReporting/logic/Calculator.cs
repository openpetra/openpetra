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
using System.Windows.Forms;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.Interfaces.MReporting;
using Ict.Petra.Shared.MReporting;
using System.Collections.Specialized;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using System.Threading;

namespace Ict.Petra.Client.MReporting.Logic
{
    /// <summary>
    /// this provides the calculation to the client;
    /// connects to the server to prepare the report
    /// </summary>
    public class TRptCalculator
    {
        /// <summary>the settings and parameters for the report</summary>
        protected TParameterList Parameters;

        /// <summary>this will contain the data returned from the server</summary>
        protected TResultList Results;

        /// <summary>will be set by SetMaxDisplayColumns; is needed for temporary columns, e.g. for sums of ledgers</summary>
        protected Int32 NewTempColumn;

        /// <summary>will be set by SetMaxDisplayColumns;</summary>
        protected Int32 MaxDisplayColumns;

        /// <summary>
        /// is this calculator used for a report or an extract
        /// </summary>
        protected bool FCalculatesExtract = false;

        /// <summary>how long did it take to calculate the report</summary>
        protected TimeSpan Duration;
        private TMReportingNamespace.TReportingUIConnectorsNamespace.TReportGeneratorUIConnector FReportingGenerator;
        private Boolean FKeepUpProgressCheck;

        /// <summary>
        /// constructor
        /// </summary>
        public TRptCalculator() : base()
        {
            Parameters = new TParameterList();
            MaxDisplayColumns = -1;
        }

        /// <summary>
        /// get/set if this calculator is used for a report or an extract
        /// </summary>
        public bool CalculatesExtract
        {
            get
            {
                return FCalculatesExtract;
            }

            set
            {
                FCalculatesExtract = value;
            }
        }

        /// <summary>
        /// Setup the sorting.
        /// Uses the param_sortby_readable, and adds missing columns to be calculated, but not displayed
        /// builds param_sortby_columns, which is a list of the column numbers
        /// based on these numbers the rows should be sorted, starting with the last number
        ///
        /// </summary>
        /// <returns>void</returns>
        public void SetupSorting()
        {
            StringCollection Calculations;
            String SortByColumnNumber;

            System.Int32 Counter;
            System.Int32 ColumnPos;

            if (Parameters.Exists("param_sortby_readable"))
            {
                if (MaxDisplayColumns == -1)
                {
                    throw new Exception("MaxDisplayColumns is not set.");
                }

                SortByColumnNumber = "";
                Calculations = StringHelper.StrSplit(Parameters.Get("param_sortby_readable").ToString(), ",");

                // go through all calculations, that have been selected for sorting
                foreach (String calculation in Calculations)
                {
                    // is the calculation displayed in a column?
                    ColumnPos = -1;

                    for (Counter = 0; Counter <= MaxDisplayColumns - 1; Counter += 1)
                    {
                        String ColumnName = Parameters.Get("param_calculation", Counter).ToString();

                        // If the name is DataLabelColumn then we have something like office specific
                        // data and we need to get the real name.
                        if ((ColumnName == "DataLabelColumn")
                            && (Parameters.Exists("param_label", Counter)))
                        {
                            ColumnName = Parameters.Get("param_label", Counter).ToString();
                        }

                        if (ColumnName == calculation)
                        {
                            ColumnPos = Counter;
                            break;
                        }
                    }

                    // if the column is not displayed, add it beyond the MaxDisplayColumns
                    if (ColumnPos == -1)
                    {
                        AddColumnCalculation(NewTempColumn, calculation);
                        ColumnPos = NewTempColumn;
                        NewTempColumn = NewTempColumn + 1;
                    }

                    if (SortByColumnNumber.Length != 0)
                    {
                        SortByColumnNumber = SortByColumnNumber + ',';
                    }

                    SortByColumnNumber = SortByColumnNumber + ColumnPos.ToString();
                }

                if (SortByColumnNumber.Length != 0)
                {
                    Parameters.Add("param_sortby_columns", SortByColumnNumber);
                }
            }
        }

        /// <summary>
        /// Add a column that uses a calculation defined in the xml file
        ///
        /// </summary>
        /// <returns>void</returns>
        public void AddColumnCalculation(int column, String calculation)
        {
            Parameters.Add("param_calculation", new TVariant(calculation), column);
        }

        /// <summary>
        /// Add a column containing financial data
        ///
        /// </summary>
        /// <returns>void</returns>
        public void AddColumnFinancial(int column, System.Int32 ledgerNr, String calculation, bool ytd)
        {
            if (Parameters.Get("param_ledger_number_i").ToInt() != ledgerNr)
            {
                Parameters.Add("param_ledger_number_i", new TVariant(ledgerNr), column);
            }

            if (!Parameters.Exists("param_ytd", -1, -1, eParameterFit.eExact))
            {
                Parameters.Add("param_ytd", new TVariant(ytd), column);
            }

            Parameters.Add("param_calculation", new TVariant(calculation), column);
        }

        /// <summary>
        /// add a column that calculates the sum of several ledgers
        /// makes automatically use of other columns that already provide data of some of the ledgers
        /// make sure you have called SetMaxDisplayColumns before!
        /// </summary>
        /// <param name="AColumn"></param>
        /// <param name="f">the function that should be applied on the values, e.g. add</param>
        /// <param name="ledgerList"></param>
        /// <param name="calculation"></param>
        /// <param name="ytd"></param>
        public void AddColumnFunctionLedgers(int AColumn, String f, StringCollection ledgerList, String calculation, bool ytd)
        {
            Int32 ledgerNr;
            Int32 counterColumn;
            String functioncall;
            bool calculationAlreadyExists;

            if (ledgerList.Count == 1)
            {
                AddColumnFinancial(AColumn, Convert.ToInt32(ledgerList[0]), calculation, ytd);
            }
            else
            {
                functioncall = f + '(';

                if (MaxDisplayColumns == -1)
                {
                    throw new Exception("MaxDisplayColumns is not set.");
                }

                foreach (string ledgerNrStr in ledgerList)
                {
                    ledgerNr = Convert.ToInt32(ledgerNrStr);
                    calculationAlreadyExists = false;

                    for (counterColumn = 0; counterColumn <= MaxDisplayColumns - 1; counterColumn += 1)
                    {
                        if ((Parameters.Get("param_calculation", counterColumn, -1,
                                 eParameterFit.eExact).ToString() == calculation)
                            && (Parameters.Get("param_ledger_number_i",
                                    counterColumn).ToInt() == ledgerNr) && (AColumn != counterColumn)
                            && (Parameters.Get("param_ytd", counterColumn).ToBool() == ytd))
                        {
                            calculationAlreadyExists = true;

                            if (functioncall.Substring(functioncall.Length - 1, 1) != "(")
                            {
                                functioncall = functioncall + ", ";
                            }

                            functioncall = functioncall + "column(" + counterColumn.ToString() + ")";
                            break;
                        }
                    }

                    if (!calculationAlreadyExists)
                    {
                        AddColumnFinancial(NewTempColumn, ledgerNr, calculation, ytd);

                        if (functioncall.Substring(functioncall.Length - 1, 1) != "(")
                        {
                            functioncall = functioncall + ", ";
                        }

                        functioncall = functioncall + "column(" + NewTempColumn.ToString() + ")";
                        NewTempColumn = NewTempColumn + 1;
                    }
                }

                functioncall = functioncall + ")";

                if (!Parameters.Exists("param_calculation", AColumn))
                {
                    Parameters.Add("param_calculation", new TVariant(functioncall), AColumn);
                }
                else
                {
                    Parameters.Add("param_formula", new TVariant(functioncall), AColumn);
                }
            }
        }

        /// <summary>
        /// add a column that calculates something based on other columns
        /// </summary>
        /// <param name="column"></param>
        /// <param name="f">the function that should be applied, containing the column number</param>
        /// <returns>void</returns>
        public void AddColumnFunction(int column, String f)
        {
            Parameters.Add("param_calculation", new TVariant(f), column);
        }

        /// <summary>
        /// Set the layout for a column for printing
        ///
        /// </summary>
        /// <returns>void</returns>
        public void AddColumnLayout(int column, double position, double positionIndented, double width)
        {
            Parameters.Add("ColumnPosition", new TVariant(position), column);
            Parameters.Add("ColumnPositionIndented", new TVariant(positionIndented), column);
            Parameters.Add("ColumnWidth", new TVariant(width), column);
        }

        /// <summary>
        /// the generated report will be written to an CSV file
        /// the separator can be defined in the parameter CSV_separator
        /// </summary>
        /// <param name="filename">the path that the results should be written to</param>
        /// <returns>s false if file could not be written
        /// </returns>
        public bool WriteResultToFile(string filename)
        {
            if ((filename[filename.Length - 1] != '\\') && (filename[filename.Length - 1] != '/'))
            {
                filename = filename + "\\";
            }

            filename = filename + Parameters.Get("currentReport").ToString() + ".csv";
            TLogging.Log("CSV file will be saved in " + filename, TLoggingType.ToConsole);
            return this.Results.WriteCSV(Parameters, filename);
        }

        /// <summary>
        /// needs to be called before adding any columns that could require temporary columns
        /// </summary>
        /// <returns>void</returns>
        public void SetMaxDisplayColumns(Int32 maxDisplayColumns)
        {
            Parameters.Add("MaxDisplayColumns", maxDisplayColumns);
            this.MaxDisplayColumns = maxDisplayColumns;
            NewTempColumn = maxDisplayColumns;
        }

        /// <summary>
        /// for creating a new report;
        /// needs to be called when the reportMain Object
        /// is reused for a second report generation
        /// </summary>
        /// <returns>void</returns>
        public void ResetParameters()
        {
            Parameters.Clear();
        }

        /// <summary>
        /// manually add parameters before the report generation stage
        /// </summary>
        /// <param name="parameterId">name of the parameter</param>
        /// <param name="value">the value of the parameter, converted to a string</param>
        /// <param name="column">additional descriptor where this variable applies
        /// </param>
        /// <returns>void</returns>
        public void AddParameter(String parameterId, String value, int column)
        {
            Parameters.Add(parameterId, new TVariant(value), column);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="value"></param>
        public void AddParameter(String parameterId, String value)
        {
            AddParameter(parameterId, value, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="value"></param>
        /// <param name="column"></param>
        public void AddParameter(String parameterId, System.Object value, int column)
        {
            Parameters.Add(parameterId, new TVariant(value), column);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="value"></param>
        public void AddParameter(String parameterId, System.Object value)
        {
            AddParameter(parameterId, value, -1);
        }

        /// <summary>
        /// Add explicit a string parameter before the report calculation
        /// </summary>
        /// <param name="paramederId">name of the parameter</param>
        /// <param name="value">the value of the parameter</param>
        public void AddStringParameter(String paramederId, String value)
        {
            Parameters.Add(paramederId, new TVariant(value, true), -1);
        }

        /// <summary>
        /// Removes one parameter from the parameter list
        /// </summary>
        /// <param name="parameterId">name of the parameter</param>
        /// <returns>true if successful, otherwise false</returns>
        public bool RemoveParameter(String parameterId)
        {
            bool returnValue = false;

            TParameter ParameterToRemove = Parameters.GetParameter(parameterId);

            if (ParameterToRemove != null)
            {
                Parameters.RemoveVariable(parameterId);
                returnValue = true;
            }

            return returnValue;
        }

        /// <summary>
        /// get the settings and formatting information for the report
        /// </summary>
        /// <returns></returns>
        public TParameterList GetParameters()
        {
            return Parameters;
        }

        /// <summary>
        /// get the result data of the report
        /// </summary>
        /// <returns>void</returns>
        public TResultList GetResults()
        {
            return Results;
        }

        /// <summary>
        /// get the duration of the calculation of the report
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetDuration()
        {
            return Duration;
        }

        /// <summary>
        /// send report as email
        /// </summary>
        public Boolean SendEmail(string AEmailAddresses,
            bool AAttachExcelFile,
            bool AAttachCSVFile,
            bool AAttachPDF,
            bool AWrapColumn,
            out TVerificationResultCollection AVerification)
        {
            return FReportingGenerator.SendEmail(AEmailAddresses, AAttachExcelFile, AAttachCSVFile, AAttachPDF, AWrapColumn, out AVerification);
        }

        /// <summary>
        /// cancel the calculation of the report
        /// </summary>
        public void CancelReportCalculation()
        {
            FReportingGenerator.Cancel();
        }

        /// <summary>
        /// this is where all the calculations take place
        /// </summary>
        /// <returns>true if the report was successfully generated
        /// </returns>
        public Boolean GenerateResultRemoteClient()
        {
            Boolean ReturnValue;
            Thread ProgressCheckThread;

            ReturnValue = false;
            FReportingGenerator = (TMReportingNamespace.TReportingUIConnectorsNamespace.TReportGeneratorUIConnector)TRemote.MReporting.UIConnectors.ReportGenerator();
            FKeepUpProgressCheck = true;

            try
            {
                this.Results = new TResultList();
                FReportingGenerator.Start(this.Parameters.ToDataTable());
                ProgressCheckThread = new Thread(new ThreadStart(AsyncProgressCheckThread));
                ProgressCheckThread.Start();
            }
            catch (Exception e)
            {
                TLogging.Log(e.Message);

                // 'Release' instantiated UIConnector Object on the server side so it can get Garbage Collected there
                TUIConnectorLifetimeHandling.ReleaseUIConnector(FReportingGenerator);

                return false;
            }

            // todo: allow canceling of the calculation of a report
            while (FKeepUpProgressCheck)
            {
                Thread.Sleep(500);
            }

            ReturnValue = FReportingGenerator.GetSuccess();

            // 'Release' instantiated UIConnector Object on the server side so it can get Garbage Collected there
            TUIConnectorLifetimeHandling.ReleaseUIConnector(FReportingGenerator);

            if (ReturnValue)
            {
                if (FCalculatesExtract)
                {
                    TLogging.Log("Extract calculation finished. Look for extract '" +
                        Parameters.Get("param_extract_name").ToString() +
                        "' in Extract Master List.", TLoggingType.ToStatusBar);

                    TFormsMessage BroadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcExtractCreated);

                    BroadcastMessage.SetMessageDataName(Parameters.Get("param_extract_name").ToString());

                    TFormsList.GFormsList.BroadcastFormMessage(BroadcastMessage);
                }
                else
                {
                    TLogging.Log("Report calculation finished.", TLoggingType.ToStatusBar);
                }
            }

            return ReturnValue;
        }

        private void AsyncProgressCheckThread()
        {
            String OldLoggingText;
            DateTime startTime;

            OldLoggingText = "";
            startTime = DateTime.Now;

            while (FKeepUpProgressCheck)
            {
                TProgressState state = FReportingGenerator.Progress;

                if (state.JobFinished)
                {
                    this.Duration = DateTime.Now - startTime;

                    if (FReportingGenerator.GetSuccess() == true)
                    {
                        this.Parameters.LoadFromDataTable(FReportingGenerator.GetParameter());
                        this.Results.LoadFromDataTable(FReportingGenerator.GetResult());
                        this.Results.SetMaxDisplayColumns(this.Parameters.Get("MaxDisplayColumns").ToInt());
                    }
                    else
                    {
                        TLogging.Log(FReportingGenerator.GetErrorMessage());
                    }

                    FKeepUpProgressCheck = false;
                }
                else
                {
                    if ((state.StatusMessage != null) && (!OldLoggingText.Equals(state.StatusMessage)))
                    {
                        TLogging.Log(state.StatusMessage, TLoggingType.ToStatusBar);
                        OldLoggingText = state.StatusMessage;
                    }
                }

                if (FKeepUpProgressCheck)
                {
                    // Sleep for some time. Then check again for latest progress information
                    Thread.Sleep(500);
                }
            }
        }
    }
}
