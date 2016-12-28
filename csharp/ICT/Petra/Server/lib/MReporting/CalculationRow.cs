//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2016 by OM International
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
using Ict.Petra.Shared.MReporting;
using Ict.Common;
using Ict.Common.DB;
using System.Collections;
using System.Data;
using System.Data.Odbc;

namespace Ict.Petra.Server.MReporting
{
    /// <summary>
    /// todoComment
    /// </summary>
    public class TRptDataCalcResult : TRptEvaluator
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="results"></param>
        /// <param name="reportStore"></param>
        /// <param name="report"></param>
        /// <param name="dataDB"></param>
        /// <param name="depth"></param>
        /// <param name="column"></param>
        /// <param name="lineId"></param>
        /// <param name="parentRowId"></param>
        public TRptDataCalcResult(TParameterList parameters,
            TResultList results,
            TReportStore reportStore,
            TRptReport report,
            TDataBase dataDB,
            int depth,
            int column,
            int lineId,
            int parentRowId)
            : base(parameters, results, reportStore, report, dataDB, depth, column, lineId, parentRowId)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="situation"></param>
        public TRptDataCalcResult(TRptSituation situation) : base(situation)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="situation"></param>
        /// <param name="depth"></param>
        /// <param name="column"></param>
        /// <param name="lineId"></param>
        /// <param name="parentRowId"></param>
        public TRptDataCalcResult(TRptSituation situation, int depth, int column, int lineId, int parentRowId) : base(situation, depth, column,
                                                                                                                     lineId, parentRowId)
        {
        }

        /// <summary>
        /// recalculate row after all the columns have been calculated already,
        /// but now the functions based on other columns are calculated.
        /// </summary>
        /// <param name="situation"></param>
        /// <param name="row"></param>
        public static void RecalculateRow(TRptSituation situation, TResult row)
        {
            int counter;
            string strCalculation;
            TVariant ColumnCalc;
            TVariant levelCalc;
            TRptDataCalcResult rptDataCalcResult;
            String ColumnFormat;
            TVariant value;

            for (counter = 0; counter <= row.column.Length - 1; counter += 1)
            {
                // calculation is used for display in the GUI, formula is used for adding ledgers
                ColumnCalc = situation.GetParameters().Get("param_formula", counter, -1, eParameterFit.eExact);

                if (ColumnCalc.IsZeroOrNull())
                {
                    ColumnCalc = situation.GetParameters().Get("param_calculation", counter, -1, eParameterFit.eExact);
                }

                levelCalc = situation.GetParameters().Get("param_formula", ReportingConsts.ALLCOLUMNS, row.depth, eParameterFit.eExact);

                if (levelCalc.IsZeroOrNull())
                {
                    levelCalc = situation.GetParameters().Get("param_calculation", ReportingConsts.ALLCOLUMNS, row.depth, eParameterFit.eExact);
                }

                strCalculation = "";

                if ((!ColumnCalc.IsZeroOrNull() && situation.GetReportStore().IsFunctionCalculation(situation.GetCurrentReport(), ColumnCalc.ToString())))
                {
                    // e.g. add(Column(1), Column(2))
                    strCalculation = ColumnCalc.ToString();
                }
                else if ((!levelCalc.IsZeroOrNull()
                          && situation.GetReportStore().IsFunctionCalculation(situation.GetCurrentReport(), levelCalc.ToString())))
                {
                    // e.g. getSumLowerReport
                    strCalculation = levelCalc.ToString();
                }

                if (situation.GetReportStore().IsFunctionCalculation(situation.GetCurrentReport(), strCalculation))
                {
                    rptDataCalcResult = new TRptDataCalcResult(situation, row.depth, counter, row.childRow, row.masterRow);
                    ColumnFormat = "";

                    if (situation.GetParameters().Exists("ColumnFormat", counter, row.depth))
                    {
                        ColumnFormat = situation.GetParameters().Get("ColumnFormat", counter, row.depth).ToString();
                    }

                    value = rptDataCalcResult.Precalculate(row.column);
                    value.ApplyFormatString(ColumnFormat);

                    if (value.ToFormattedString().Length > 0)
                    {
                        row.column[counter] = value;
                    }
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="col"></param>
        /// <param name="precalculatedColumns"></param>
        protected void CalculateColumnValue(System.Int32 col, ref TVariant[] precalculatedColumns)
        {
            TRptDataCalcValue rptDataCalcValue;

            List <TRptValue>rptGrpValue;
            TParameter parameter;
            String ColumnFormat;
            TVariant value;

            column = col;
            parameter = Parameters.GetParameter("ControlSource", column, Depth, eParameterFit.eExact);

            if (parameter == null)
            {
                parameter = Parameters.GetParameter("ControlSource", column, -1, eParameterFit.eExact);
            }

            if (parameter == null)
            {
                parameter = Parameters.GetParameter("ControlSource", ReportingConsts.ALLCOLUMNS, Depth, eParameterFit.eBestFit);
            }

            if ((parameter != null) && (parameter.value.ToString() == "rptGrpValue") && (parameter.pRptGroup != null))
            {
                rptGrpValue = (List <TRptValue> )parameter.pRptGroup;
                rptDataCalcValue = new TRptDataCalcValue(this);
                ColumnFormat = "";

                if (Parameters.Exists("ColumnFormat", column, Depth))
                {
                    ColumnFormat = GetParameters().Get("ColumnFormat", column, Depth).ToString();
                }

                value = rptDataCalcValue.Calculate(rptGrpValue);
                value.ApplyFormatString(ColumnFormat);

                if (value.ToFormattedString().Length > 0)
                {
                    precalculatedColumns[column] = value;
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="masterRow"></param>
        /// <param name="condition"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Boolean SavePrecalculation(int masterRow, String condition, String code)
        {
            Boolean ReturnValue;

            TVariant[] precalculatedColumns;
            TVariant[] precalculatedDescr = new TVariant[2];
            TVariant[] header = new TVariant[2];
            Boolean display;
            Boolean hideRow = false;
            Boolean debit_credit_indicator;
            Boolean param_hide_empty_lines = false;
            int counter;
            String values;
            TRptDataCalcValue rptDataCalcValue;
            List <TRptValue>rptGrpValue;
            int maxDisplayColumns;
            int numberColumns;
            int ColumnPartnerName = -1;
            int ColumnPartnerKey = -1;
            TResult newRow;
            ReturnValue = false;
            condition = condition.ToLower();
            maxDisplayColumns = Results.GetMaxDisplayColumns();
            numberColumns = maxDisplayColumns;

            while (Parameters.Exists("param_calculation", numberColumns, -1))
            {
                numberColumns = numberColumns + 1;
            }

            precalculatedColumns = new TVariant[numberColumns];

            for (counter = 0; counter <= maxDisplayColumns - 1; counter += 1)
            {
                precalculatedColumns[counter] = new TVariant();
            }

            if (!Parameters.Exists("debit_credit_indicator", -1, Depth))
            {
                debit_credit_indicator = true;
            }
            else
            {
                debit_credit_indicator = Parameters.Get("debit_credit_indicator", -1, Depth).ToBool();
            }

            if (Parameters.Get("param_hide_empty_lines").ToString() == "true")
            {
                param_hide_empty_lines = true;
            }

            if (Depth > 0)
            {
                if (debit_credit_indicator)
                {
                    Parameters.Add("debit_credit_indicator", true, -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                }
                else
                {
                    Parameters.Add("debit_credit_indicator", false, -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                }

                values = Parameters.Get("ControlSource", ReportingConsts.ALLCOLUMNS, Depth).ToString();

                if (values == "calculation")
                {
                    // first calculate the invisible helper Columns
                    for (counter = maxDisplayColumns; counter <= numberColumns - 1; counter += 1)
                    {
                        column = counter;
                        precalculatedColumns[column] = Precalculate(precalculatedColumns);
                    }

                    // calculate the visible Columns
                    for (counter = 0; counter <= maxDisplayColumns - 1; counter += 1)
                    {
                        column = counter;
                        precalculatedColumns[column] = Precalculate(precalculatedColumns);

                        if (param_hide_empty_lines)
                        {
                            // if this parameter is set, hide the row when all columns are empty, except
                            // of the column partner name and partner key.
                            TParameter CurrentParameter = Parameters.GetParameter("param_calculation", column, -1, eParameterFit.eExact);

                            if (CurrentParameter.value.ToString() == "Partner Name")
                            {
                                ColumnPartnerName = CurrentParameter.column;
                            }
                            else if (CurrentParameter.value.ToString() == "Partner Key")
                            {
                                ColumnPartnerKey = CurrentParameter.column;
                            }
                        }
                    }

                    if (param_hide_empty_lines)
                    {
                        hideRow = IsRowEmpty(ref precalculatedColumns, ColumnPartnerName, ColumnPartnerKey, 0, maxDisplayColumns - 1);
                    }
                }
                else
                {
                    // first calculate the invisible helper Columns
                    for (counter = maxDisplayColumns; counter <= numberColumns - 1; counter += 1)
                    {
                        CalculateColumnValue(counter, ref precalculatedColumns);
                    }

                    for (counter = 0; counter <= maxDisplayColumns - 1; counter += 1)
                    {
                        CalculateColumnValue(counter, ref precalculatedColumns);
                    }
                }

                rptGrpValue = (List <TRptValue> )Parameters.GetGrpValue("ControlSource",
                    ReportingConsts.HEADERCOLUMN + 1,
                    Depth,
                    eParameterFit.eExact);

                if (rptGrpValue != null)
                {
                    Parameters.Add("headerVISIBLE", new TVariant(true), -1, Depth);
                    column = ReportingConsts.HEADERCOLUMN + 1;
                    rptDataCalcValue = new TRptDataCalcValue(this);
                    header[0] = rptDataCalcValue.Calculate(rptGrpValue);
                }

                rptGrpValue = (List <TRptValue> )Parameters.GetGrpValue("ControlSource",
                    ReportingConsts.HEADERCOLUMN + 2,
                    Depth,
                    eParameterFit.eExact);

                if (rptGrpValue != null)
                {
                    Parameters.Add("headerVISIBLE", new TVariant(true), -1, Depth);
                    column = ReportingConsts.HEADERCOLUMN + 2;
                    rptDataCalcValue = new TRptDataCalcValue(this);
                    header[1] = rptDataCalcValue.Calculate(rptGrpValue);
                }

                for (counter = 0; counter <= 1; counter += 1)
                {
                    column = counter - 10;
                    rptGrpValue = (List <TRptValue> )Parameters.GetGrpValue("ControlSource", column, Depth, eParameterFit.eExact);

                    if (rptGrpValue != null)
                    {
                        rptDataCalcValue = new TRptDataCalcValue(this);
                        precalculatedDescr[counter] = rptDataCalcValue.Calculate(rptGrpValue);
                    }
                }

                debit_credit_indicator = Parameters.Get("debit_credit_indicator", column, Depth).ToBool();
                display = true;
                newRow = Results.AddRow(masterRow,
                    LineId,
                    display,
                    Depth,
                    code,
                    condition,
                    debit_credit_indicator,
                    header,
                    precalculatedDescr,
                    precalculatedColumns);
                ReturnValue = true;

                if (condition.Length != 0)
                {
                    column = -1;
                    display = EvaluateCondition(condition);
                }

                if (Parameters.Exists("DONTDISPLAYROW") && (Parameters.Get("DONTDISPLAYROW").ToBool() == true))
                {
                    display = false;
                }

                if ((newRow != null) && ((!display) || hideRow))
                {
                    newRow.display = false;
                    ReturnValue = false;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// go through the whole result list and check,
        /// which rows should be displayed,
        /// using the condition that was saved with them
        ///
        /// needed for e.g. calculations based on lower rows
        /// </summary>
        /// <returns>void</returns>
        public void CheckDisplayStatus()
        {
            TResult element;
            int counter;
            bool display;
            ArrayList ResultsArray;

            ResultsArray = Results.GetResults();
            counter = ResultsArray.Count - 1;

            while (counter >= 0)
            {
                element = (TResult)ResultsArray[counter];
                counter--;
                display = element.display;

                if (element.condition.Length != 0)
                {
                    // reevaluating the condition is dangerous, if variables of the current situation are referenced
                    if (element.condition.IndexOf('{') == -1)
                    {
                        this.column = -1;
                        this.LineId = element.childRow;
                        this.Depth = element.depth;
                        this.ParentRowId = element.masterRow;
                        display = EvaluateCondition(element.condition);

                        // todo: should that not be the result of the condition that is evalulated? if it returns string 'invisible', then don't display
                        if (element.condition.ToLower().IndexOf("invisible") != -1)
                        {
                            display = false;
                        }
                    }
                }

                element.display = display;
            }

            // actually remove the lines that should not be displayed
            counter = 0;

            while (counter < ResultsArray.Count)
            {
                element = (TResult)ResultsArray[counter];

                if (element.display)
                {
                    counter++;
                }
                else
                {
                    ResultsArray.RemoveAt(counter);
                }
            }

            // todo: ? reset the lowestlevel variable, it might have changed
        }

        /// <summary>
        /// todoComment
        /// </summary>
        public void RecalculateFunctionColumns()
        {
            ProcessAllRows(new TProcessResult(RecalculateRow));
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="precalculatedColumns"></param>
        /// <returns></returns>
        public TVariant Precalculate(TVariant[] precalculatedColumns)
        {
            String strCalculation;
            TRptDataCalcCalculation rptDataCalcCalculation;
            TRptCalculation rptCalculation;

            TVariant ReturnValue = new TVariant();

            // calculation is used for display in the GUI, formula is used for adding ledgers
            if ((!GetParameters().Exists("param_calculation", column, Depth)))
            {
                return ReturnValue;
            }

            if (GetParameters().Exists("param_formula", column, Depth))
            {
                strCalculation = GetParameters().Get("param_formula", column, Depth).ToString();
            }
            else
            {
                strCalculation = GetParameters().Get("param_calculation", column, Depth).ToString();
            }

            rptCalculation = ReportStore.GetCalculation(CurrentReport, strCalculation);

            if (rptCalculation == null)
            {
                ReturnValue = EvaluateFunctionCalculation(strCalculation, precalculatedColumns);
            }
            else
            {
                rptDataCalcCalculation = new TRptDataCalcCalculation(this);

                if (!rptDataCalcCalculation.EvaluateCalculationFunction(rptCalculation.rptGrpQuery, ref precalculatedColumns, ref ReturnValue))
                {
                    ReturnValue = rptDataCalcCalculation.EvaluateCalculationAll(rptCalculation,
                        null,
                        rptCalculation.rptGrpTemplate,
                        rptCalculation.rptGrpQuery).VariantValue;

                    if (ReturnValue.IsZeroOrNull())
                    {
                        ReturnValue.ApplyFormatString(rptCalculation.strReturnsFormat);
                        return ReturnValue;
                    }
                }

                ReturnValue.ApplyFormatString(rptCalculation.strReturnsFormat);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks if all values of a row are empty. But it ignores the columns of the PartnerKey and PartnerName
        /// </summary>
        /// <param name="APrecalculatedColumns">Values of the row</param>
        /// <param name="AColumnPartnerName">Index of the column that is ignored</param>
        /// <param name="AColumnPartnerKey">Index of the column that is ignored</param>
        /// <param name="AStartIndex">Start index of columns to check</param>
        /// <param name="AEndIndex">End index of columns to check</param>
        /// <returns>true if the row is empty, otherwise false</returns>
        private bool IsRowEmpty(ref TVariant[] APrecalculatedColumns, int AColumnPartnerName, int AColumnPartnerKey,
            int AStartIndex, int AEndIndex)
        {
            bool returnValue = true;

            for (int Counter = AStartIndex; Counter <= AEndIndex; ++Counter)
            {
                if ((Counter == AColumnPartnerName)
                    || (Counter == AColumnPartnerKey))
                {
                    // don't check the column of partner name and partner key because
                    // they should never be empty.
                    continue;
                }

                if (APrecalculatedColumns[Counter].ToString().Length > 0)
                {
                    returnValue = false;
                    break;
                }
            }

            return returnValue;
        }
    }
}