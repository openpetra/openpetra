//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2017 by OM International
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
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared.MReporting;
using System.Data.Odbc;

namespace Ict.Petra.Server.MReporting
{
    /// <summary>
    /// calculate the current level
    /// </summary>
    public class TRptDataCalcLevel : TRptSituation
    {
        /// <summary>
        /// the detail of the current level
        /// </summary>
        protected TRptDetail rptDetail;

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
        public TRptDataCalcLevel(TParameterList parameters,
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
        /// <param name="depth"></param>
        /// <param name="column"></param>
        /// <param name="lineId"></param>
        /// <param name="parentRowId"></param>
        public TRptDataCalcLevel(TRptSituation situation, int depth, int column, int lineId, int parentRowId) :
            base(situation, depth, column, lineId, parentRowId)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="situation"></param>
        public TRptDataCalcLevel(TRptSituation situation) : base(situation)
        {
        }

        /// <summary>
        /// calculate a level
        ///
        /// </summary>
        /// <returns>The number of the line calculated. -1 if unsuccessful
        /// </returns>
        public int Calculate(TRptLevel rptLevel, int masterRow)
        {
            if (rptLevel == null)
            {
                return -1;
            }

            TRptDetail rptDetail = rptLevel.rptDetail;

            if (rptDetail == null)
            {
                return -1;
            }

            if (Parameters.Get("CancelReportCalculation").ToBool() == true)
            {
                // TLogging.Log('Report calculation was cancelled', [ToStatusBar]);
                return -1;
            }

            int thisRunningCode = GetNextRunningCode();
            List <TRptField>rptGrpField = rptDetail.rptGrpField;
            List <TRptLowerLevel>rptGrpLowerLevel = rptDetail.rptGrpLowerLevel;
//          TLogging.Log("[MReporting\\Calculation.cs] Calculate(" + rptLevel.strName + ", " + masterRow + ")");

            if (rptDetail.rptSwitch != null)
            {
                TRptDataCalcSwitch calcSwitch = new TRptDataCalcSwitch(this);
                calcSwitch.Calculate(rptDetail.rptSwitch, out rptGrpLowerLevel, out rptGrpField);
            }

            if (rptGrpLowerLevel != null)
            {
                TRptDataCalcLowerLevel calcLowerLevel;

                if (Depth == 0)
                {
                    Int16 subreport = 0;

                    foreach (TRptLowerLevel rptLowerLevel in rptGrpLowerLevel)
                    {
                        Parameters.Add("CurrentSubReport", subreport);
                        calcLowerLevel = new TRptDataCalcLowerLevel(this);
                        calcLowerLevel.Calculate(rptLowerLevel, thisRunningCode);
                        subreport++;

                        if (Parameters.Get("CancelReportCalculation").ToBool() == true)
                        {
                            // TLogging.Log('Report calculation was cancelled', [ToStatusBar]);
                            return -1;
                        }
                    }

                    Parameters.Add("CurrentSubReport", -1);
                }
                else
                {
                    foreach (TRptLowerLevel rptLowerLevel in rptGrpLowerLevel)
                    {
                        calcLowerLevel = new TRptDataCalcLowerLevel(this);
                        calcLowerLevel.Calculate(rptLowerLevel, thisRunningCode);

                        if (Parameters.Get("CancelReportCalculation").ToBool() == true)
                        {
                            // TLogging.Log('Report calculation was cancelled', [ToStatusBar]);
                            return -1;
                        }
                    }
                }
            }
            else if (rptGrpField != null)
            {
                TRptDataCalcField calcGrpField = new TRptDataCalcField(this);
                calcGrpField.Calculate(rptGrpField);
            }

            TRptDataCalcHeaderFooter calcHeaderFooter = new TRptDataCalcHeaderFooter(this);
            calcHeaderFooter.Calculate(rptLevel.rptGrpHeaderField, rptLevel.rptGrpHeaderSwitch);
            calcHeaderFooter.Calculate(rptLevel.rptGrpFooterField, rptLevel.rptGrpFooterSwitch, rptLevel.strFooterLine, rptLevel.strFooterSpace);

            string strIdentification = rptLevel.strIdentification;
            string strId = "";

            while (strIdentification.Length != 0)
            {
                if (strId.Length != 0)
                {
                    strId += '/';
                }

                string strTemp = StringHelper.GetNextCSV(ref strIdentification).Trim();

                if (Parameters.Exists(strTemp, -1, Depth))
                {
                    strId += Parameters.Get(strTemp, -1, Depth).ToString(false);
                }
                else
                {
                    strId += strTemp;
                }
            }

            this.LineId = thisRunningCode;
            this.ParentRowId = masterRow;
            TRptDataCalcResult calcResult = new TRptDataCalcResult(this, Depth, -1, this.LineId, this.ParentRowId);

            if (calcResult.SavePrecalculation(masterRow, rptLevel.strCondition, strId))
            {
                // only write log if something was actually saved.
                TLogging.Log("Preparing " + strId, TLoggingType.ToStatusBar);
            }

            if (Parameters.Get("CancelReportCalculation").ToBool() == true)
            {
                // TLogging.Log('Report calculation was cancelled', [ToStatusBar]);
                return -1;
            }

            if (thisRunningCode == 0)
            {
                // at this point all the values are precalculated
                // go again through the numbers, and calculate the function results, e.g. variance on ReportingConsts.COLUMNs
                calcResult.RecalculateFunctionColumns();
                calcResult.CheckDisplayStatus();

                if (Parameters.Exists("param_sortby_columns"))
                {
                    TLogging.Log("Sorting...", TLoggingType.ToStatusBar);

                    Boolean SortMultipleLevels = false;

                    if (Parameters.Exists("param_sort_multiple_levels"))
                    {
                        // if we allow sorting of multiple levels, the result will be changed to flat table
                        // and we will end up with all the results in level 0. We might want this in some reports (e.g. Personnel-Birthday report)
                        SortMultipleLevels = true;
                    }

                    calcResult.GetResults().Sort(Parameters.Get("param_sortby_columns").ToString(), SortMultipleLevels);
                }

                TLogging.Log("Finished", TLoggingType.ToStatusBar);
            }

            //// Reset the row display parameter ready for the next level
            if (Parameters.Exists("DONTDISPLAYROW") && Parameters.Get("DONTDISPLAYROW").ToBool())
            {
                Parameters.Add("DONTDISPLAYROW", false);
            }

            return thisRunningCode;
        }
    }

    /// <summary>
    /// calculate a switch (several case elements)
    /// </summary>
    public class TRptDataCalcSwitch : TRptEvaluator
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
        public TRptDataCalcSwitch(TParameterList parameters,
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
        public TRptDataCalcSwitch(TRptSituation situation) : base(situation)
        {
        }

        /// <summary>
        /// based on the evaluation of the condition for each case, return the one case that fits
        /// </summary>
        /// <param name="rptSwitch"></param>
        /// <returns></returns>
        protected TRptCase GetFittingCase(TRptSwitch rptSwitch)
        {
            TRptCase ReturnValue;
            TRptCase rptCase;
            int counter;

            counter = 0;
            ReturnValue = null;

            if (rptSwitch.rptGrpCases != null)
            {
                while ((counter < rptSwitch.rptGrpCases.Count) && (ReturnValue == null))
                {
                    rptCase = (TRptCase)rptSwitch.rptGrpCases[counter];

                    if (EvaluateCondition(rptCase.strCondition))
                    {
                        ReturnValue = rptCase;
                    }

                    counter++;
                }
            }

            if (ReturnValue == null)
            {
                ReturnValue = rptSwitch.rptDefault;
            }

            return ReturnValue;
        }

        /// <summary>
        /// this procedure should be used in levels only
        /// </summary>
        /// <returns>void</returns>
        public void Calculate(TRptSwitch rptSwitch, out List <TRptLowerLevel>rptGrpLowerLevel, out List <TRptField>rptGrpField)
        {
            TRptDataCalcSwitch calcSwitch;
            TRptCase rptCase;

            rptGrpField = null;
            rptGrpLowerLevel = null;
            rptCase = GetFittingCase(rptSwitch);

            if (rptCase != null)
            {
                rptGrpField = rptCase.rptGrpField;
                rptGrpLowerLevel = rptCase.rptGrpLowerLevel;

                if (rptCase.rptSwitch != null)
                {
                    calcSwitch = new TRptDataCalcSwitch(this);
                    calcSwitch.Calculate(rptCase.rptSwitch, out rptGrpLowerLevel, out rptGrpField);
                }
            }
        }

        /// <summary>
        /// this function can be used in calculations and other places as well
        /// </summary>
        public TRptFormatQuery Calculate(List <TRptSwitch>rptGrpSwitch)
        {
            TRptFormatQuery ReturnValue = new TRptFormatQuery(string.Empty, null, this.Parameters, this.column, this.Depth);

            foreach (TRptSwitch rptSwitch in rptGrpSwitch)
            {
                ReturnValue.Add(Calculate(rptSwitch));
            }

            return ReturnValue;
        }

        /// <summary>
        /// calculate the value of a switch
        /// </summary>
        /// <param name="rptSwitch"></param>
        /// <returns></returns>
        public TRptFormatQuery Calculate(TRptSwitch rptSwitch)
        {
            TRptFormatQuery ReturnValue;
            TRptDataCalcField rptDataCalcField;
            TRptDataCalcValue rptDataCalcValue;
            TRptDataCalcSwitch rptDataCalcSwitch;
            TRptCase rptCase;

            ReturnValue = new TRptFormatQuery(string.Empty, null, this.Parameters, this.column, this.Depth);
            rptCase = GetFittingCase(rptSwitch);

            if (rptCase == null)
            {
                return ReturnValue;
            }

            if (rptCase.rptSwitch != null)
            {
                rptDataCalcSwitch = new TRptDataCalcSwitch(this);
                ReturnValue.Add(rptDataCalcSwitch.Calculate(rptCase.rptSwitch));
            }
            else
            {
                if (rptCase.rptGrpField != null)
                {
                    rptDataCalcField = new TRptDataCalcField(this);
                    rptDataCalcField.Calculate(rptCase.rptGrpField);
                }

                if (rptCase.rptGrpValue != null)
                {
                    rptDataCalcValue = new TRptDataCalcValue(this);
                    ReturnValue.Add(rptDataCalcValue.Calculate(rptCase.rptGrpValue));
                }
            }

            return ReturnValue;
        }
    }

    /// <summary>
    /// calculate the next level deeper, if the condition is true
    /// </summary>
    public class TRptDataCalcLowerLevel : TRptEvaluator
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
        public TRptDataCalcLowerLevel(TParameterList parameters,
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
        public TRptDataCalcLowerLevel(TRptSituation situation) : base(situation)
        {
        }

        /// <summary>
        /// calculate the next level deeper
        /// </summary>
        /// <param name="rptLowerLevel"></param>
        /// <param name="masterRow"></param>
        public void Calculate(TRptLowerLevel rptLowerLevel, int masterRow)
        {
            TRptDataCalcLevel rptDataCalcLevel;
            TRptCalculation rptCalculation;
            TRptDataCalcCalculation rptDataCalcCalculation;
            TRptDataCalcParameter rptDataCalcParameter;

            if (!EvaluateCondition(rptLowerLevel.strCondition))
            {
                return;
            }

            if (rptLowerLevel.rptGrpParameter != null)
            {
                rptDataCalcParameter = new TRptDataCalcParameter(this);
                rptDataCalcParameter.Calculate(new TRptFormatQuery(string.Empty, null, this.Parameters, this.column, this.Depth), rptLowerLevel.rptGrpParameter);
            }

            if (rptLowerLevel.strCalculation.Length == 0)
            {
                rptDataCalcLevel = new TRptDataCalcLevel(this);
                rptDataCalcLevel.Depth++;
                rptDataCalcLevel.Calculate(CurrentReport.GetLevel(rptLowerLevel.strLevel), masterRow);
            }
            else
            {
                rptCalculation = ReportStore.GetCalculation(CurrentReport, rptLowerLevel.strCalculation);

                if (rptCalculation == null)
                {
                    TLogging.Log("Calculation not found:" + rptLowerLevel.strCalculation);
                    return;
                }

                rptDataCalcCalculation = new TRptDataCalcCalculation(this);

                rptDataCalcCalculation.EvaluateCalculation(rptCalculation, rptLowerLevel.rptGrpParameter, rptLowerLevel.strLevel, masterRow);
            }
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TRptDataCalcField : TRptEvaluator
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
        public TRptDataCalcField(TParameterList parameters,
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
        public TRptDataCalcField(TRptSituation situation) : base(situation)
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="rptGrpField"></param>
        public void Calculate(List <TRptField>rptGrpField)
        {
            foreach (TRptField rptField in rptGrpField)
            {
                Calculate(rptField);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="rptField"></param>
        public void Calculate(TRptField rptField)
        {
            int cmpos;
            string whichField;
            TRptDataCalcCalculation rptDataCalcCalculation;
            String list;
            String element;

            column = -1;
            whichField = rptField.strWhichfield;

            if (whichField.IndexOf("columns") == 0)
            {
                column = ReportingConsts.ALLCOLUMNS;
            }
            else if (whichField.IndexOf("column ") == 0)
            {
                if (whichField[7] == '{')
                {
                    String ParameterName = whichField.Substring(8, whichField.Length - 9);
                    column = GetParameters().Get(ParameterName).ToInt();
                }
                else
                {
                    column = (Int32)StringHelper.StrToInt(whichField.Substring(7, whichField.Length - 7));
                }
            }
            else if (whichField.IndexOf("left ") == 0)
            {
                column = ReportingConsts.COLUMNLEFT + (Int32)StringHelper.StrToInt(whichField.Substring(5, whichField.Length - 5)) + 1;
            }
            else if (whichField.IndexOf("header ") == 0)
            {
                column = ReportingConsts.HEADERCOLUMN + (Int32)StringHelper.StrToInt(whichField.Substring(7, whichField.Length - 7)) + 1;
            }
            else
            {
                if (whichField == "title1")
                {
                    column = ReportingConsts.HEADERTITLE1;
                }

                if (whichField == "title2")
                {
                    column = ReportingConsts.HEADERTITLE2;
                }

                if (whichField == "descr1")
                {
                    column = ReportingConsts.HEADERDESCR1;
                }

                if (whichField == "descr2")
                {
                    column = ReportingConsts.HEADERDESCR2;
                }

                if (whichField == "descr3")
                {
                    column = ReportingConsts.HEADERDESCR3;
                }

                if (whichField == "period1")
                {
                    column = ReportingConsts.HEADERPERIOD;
                }

                if (whichField == "period2")
                {
                    column = ReportingConsts.HEADERPERIOD2;
                }

                if (whichField == "period3")
                {
                    column = ReportingConsts.HEADERPERIOD3;
                }

                if (whichField == "left1")
                {
                    column = ReportingConsts.HEADERPAGELEFT1;
                }

                if (whichField == "left2")
                {
                    GetParameters().Add("FOOTER2LINES", new TVariant(true), -1, Depth);
                    column = ReportingConsts.HEADERPAGELEFT2;
                }

                if (whichField == "type")
                {
                    column = ReportingConsts.HEADERTYPE;
                }
            }

            if (rptField.strAlign != "")
            {
                GetParameters().Add("ColumnAlign", new TVariant(rptField.strAlign), column, Depth);
            }

            if (rptField.strFormat.Length != 0)
            {
                GetParameters().Add("ColumnFormat", new TVariant(rptField.strFormat), column, Depth);
            }

            if (rptField.strLine.IndexOf("above") != -1)
            {
                GetParameters().Add("LineAbove", new TVariant(true), column, Depth);
            }

            if (rptField.strLine.IndexOf("below") != -1)
            {
                GetParameters().Add("LineBelow", new TVariant(true), column, Depth);
            }

            if (rptField.strPos.IndexOf("indented") != -1)
            {
                GetParameters().Add("indented", new TVariant(true), column, Depth);
            }

            cmpos = rptField.strPos.IndexOf("cm");

            if (cmpos != -1)
            {
                GetParameters().Add("ColumnPosition", new TVariant(StringHelper.TryStrToDecimal(rptField.strPos.Substring(0,
                                cmpos), 0.0M)), column, Depth);
            }

            cmpos = rptField.strWidth.IndexOf("cm");

            if (cmpos != -1)
            {
                GetParameters().Add("ColumnWidth", new TVariant(StringHelper.TryStrToDecimal(rptField.strWidth.Substring(0,
                                cmpos), 0.0M)), column, Depth);
            }

            if (rptField.strCalculation.Length != 0)
            {
                rptDataCalcCalculation = new TRptDataCalcCalculation(this);

                // allow several helper calculations to be executed after each other;
                // e.g. giftbatchexport; first get the accumulated description of the gift details, then get bank account details
                list = rptField.strCalculation;

                while (list.Length != 0)
                {
                    element = StringHelper.GetNextCSV(ref list).Trim();
                    rptDataCalcCalculation.EvaluateHelperCalculation(element);
                }
            }

            if (column != -1)
            {
                if (rptField.rptGrpValue != null)
                {
                    // overwrite the global column calculation
                    GetParameters().Add("param_calculation", new TVariant(), column, Depth, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                }

                Calculate(rptField.rptGrpValue, rptField.rptGrpFieldDetail);
            }
            else
            {
                throw new Exception("ERROR: whichField problem: " + whichField);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="rptGrpValue"></param>
        /// <param name="rptGrpFieldDetail"></param>
        public void Calculate(List <TRptValue>rptGrpValue, List <TRptFieldDetail>rptGrpFieldDetail)
        {
            TVariant value;
            TRptDataCalcValue rptDataCalcValue;

            if (Depth == -1)
            {
                value = new TVariant();

                // this is only in header; value is directly calculated
                if (rptGrpFieldDetail != null)
                {
                    value = Calculate(rptGrpFieldDetail);
                }
                else
                {
                    if (rptGrpValue != null)
                    {
                        rptDataCalcValue = new TRptDataCalcValue(this);
                        value = rptDataCalcValue.Calculate(rptGrpValue).ConvertToVariant();
                    }
                }

                GetParameters().Add("ControlSource", value, column, Depth);
            }
            else
            {
                if (rptGrpValue != null)
                {
                    GetParameters().Add("ControlSource", new TVariant("rptGrpValue"), column, Depth, null, rptGrpValue, -1);
                }
                else
                {
                    GetParameters().Add("ControlSource", new TVariant("calculation"), column, Depth);
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="rptGrpFieldDetail"></param>
        /// <returns></returns>
        public TVariant Calculate(List <TRptFieldDetail>rptGrpFieldDetail)
        {
            TVariant ReturnValue;
            TRptDataCalcValue rptDataCalcValue;
            TRptDataCalcSwitch rptDataCalcSwitch;

            ReturnValue = new TVariant();

            foreach (TRptFieldDetail rptFieldDetail in rptGrpFieldDetail)
            {
                if (EvaluateCondition(rptFieldDetail.strCondition))
                {
                    if (rptFieldDetail.rptGrpValue != null)
                    {
                        rptDataCalcValue = new TRptDataCalcValue(this);
                        ReturnValue.Add(rptDataCalcValue.Calculate(rptFieldDetail.rptGrpValue).ConvertToVariant());
                    }
                    else
                    {
                        rptDataCalcSwitch = new TRptDataCalcSwitch(this);
                        ReturnValue.Add(rptDataCalcSwitch.Calculate(rptFieldDetail.rptSwitch).ConvertToVariant());
                    }
                }
            }

            return ReturnValue;
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TRptDataCalcValue : TRptEvaluator
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
        public TRptDataCalcValue(TParameterList parameters,
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
        /// <param name="depth"></param>
        /// <param name="column"></param>
        /// <param name="lineId"></param>
        /// <param name="parentRowId"></param>
        public TRptDataCalcValue(TRptSituation situation, int depth, int column, int lineId, int parentRowId) :
            base(situation, depth, column, lineId, parentRowId)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="situation"></param>
        public TRptDataCalcValue(TRptSituation situation) : base(situation)
        {
        }

        /// <summary>
        /// make sure there is no semicolon, no quote, no comment sign.
        /// see also https://www.owasp.org/index.php/SQL_Injection_Prevention_Cheat_Sheet#Defense_Option_4:_Escaping_All_User_Supplied_Input
        /// throws an exception if suspicious characters are found
        /// </summary>
        /// <param name="s"></param>
        private static void ValidateAgainstSQLInjection(string s)
        {
            if (s.IndexOfAny(new char[]{'\'', ';'}) != -1
                || s.IndexOf("--") != -1
                || s.IndexOf("/*") != -1)
            {
                throw new Exception("possible sql injection attack? " + s);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="rptGrpValue"></param>
        /// <param name="AWithSpaceBetweenValues"></param>
        /// <returns></returns>
        public TRptFormatQuery Calculate(List <TRptValue>rptGrpValue, Boolean AWithSpaceBetweenValues = false)
        {
            TRptFormatQuery ReturnValue = new TRptFormatQuery(string.Empty, null, this.Parameters, this.column, this.Depth);
            String listValue;
            String listText;
            Boolean first;
            TRptDataCalcCalculation rptDataCalcCalculation;

            foreach (TRptValue rptValue in rptGrpValue)
            {
                if (EvaluateCondition(rptValue.strCondition))
                {
                    if ((AWithSpaceBetweenValues == true) && (ReturnValue.SQLStmt.Length > 0))
                    {
                        ReturnValue.Add(" ");
                    }

                    if (rptValue.strFunction == "csv")
                    {
                        // this is only for sql queries
                        listValue = Parameters.Get(rptValue.strVariable, column, Depth).ToString();

                        if ((listValue.Length > 3) && (listValue.Substring(0, 4) == "CSV:"))
                        {
                            listValue = listValue.Substring(4);
                        }

                        first = true;
                        ReturnValue.Add(" (");

                        if (listValue.Length == 0)
                        {
                            // this would make the sql query invalid; so insert a false condition to at least make it run, and return no records
                            ReturnValue.Add(" 1 = 0 ");
                        }

                        while (listValue.Length != 0)
                        {
                            bool IsFirstPair = true;
                            string value = StringHelper.GetNextCSV(ref listValue).Trim();

                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                ReturnValue.Add(" OR ");
                            }

                            if (rptValue.strText.IndexOf(',') != -1)
                            {
                                // there are two things to compare (e.g. motivation group and motivation detail)

                                // motivation group and detail come in pairs, separated by comma, enclosed by quotes (getNextCSV strips the quotes)
                                string valuePair = value;
                                listText = rptValue.strText;
                                string calculationText = rptValue.strCalculation;

                                // if the format is defined in the xml file, it overwrites the value we get from (new TVariant(value).TypeVariant == eVariantTypes.xxx
                                bool ValueIsNumber = (rptValue.strFormat == "Number");
                                bool ValueIsText = (rptValue.strFormat == "Text");

                                ReturnValue.Add("(");

                                while (listText.Length != 0)
                                {
                                    if (IsFirstPair)
                                    {
                                        // we have taken out the first value already from listValue
                                        value = valuePair;
                                        IsFirstPair = false;
                                    }
                                    else
                                    {
                                        value = StringHelper.GetNextCSV(ref listValue).Trim();
                                    }

                                    if (listText.Length < rptValue.strText.Length)
                                    {
                                        if (calculationText.Length > 0)
                                        {
                                            ValidateAgainstSQLInjection(calculationText);
                                            ReturnValue.Add(" " + calculationText + " ");
                                        }
                                        else
                                        {
                                            ReturnValue.Add(" AND ");
                                        }
                                    }

                                    //todo: allow integer as well; problem with motivation detail codes that are just numbers;
                                    //todo: specify type with text, variable names and type
                                    ValidateAgainstSQLInjection(value.ToString());
                                    if (ValueIsNumber)
                                    {
                                        ReturnValue.Add(StringHelper.GetNextCSV(ref listText).Trim() + " = {{valueparameter}} ");
                                        ReturnValue.AddOdbcParameters("{{", "valueparameter", "}}", new TVariant(value));
                                    }
                                    else
                                    {
                                        ReturnValue.Add(StringHelper.GetNextCSV(ref listText).Trim() + " = {valueparameter} ");
                                        ReturnValue.AddOdbcParameters("{", "valueparameter", "}", new TVariant(value));
                                    }
                                }

                                ReturnValue.Add(")");
                            }
                            else
                            {
                                ReturnValue.Add(' ' + rptValue.strText + " = {valueparameter} ");
                                ValidateAgainstSQLInjection(value.ToString());
                                ReturnValue.AddOdbcParameters("{", "valueparameter", "}", new TVariant(value));
                            }
                        }

                        ReturnValue.Add(") ");
                    }
                    else
                    {
                        if (rptValue.strText.Length != 0)
                        {
                            // this can be a full query: so no ValidateAgainstSQLInjection(rptValue.strText);
                            ReturnValue.Add(new TVariant(rptValue.strText), rptValue.strFormat);
                        }

                        if (rptValue.strVariable.Length != 0)
                        {
                            TVariant pvalue = Parameters.Get(rptValue.strVariable, column, Depth);
                            ValidateAgainstSQLInjection(pvalue.ToString());
                            ReturnValue.Add(pvalue, rptValue.strFormat);
                        }

                        if (rptValue.strCalculation.Length != 0)
                        {
                            rptDataCalcCalculation = new TRptDataCalcCalculation(this);
                            TVariant cvalue = rptDataCalcCalculation.EvaluateHelperCalculation(rptValue.strCalculation);
                            ValidateAgainstSQLInjection(cvalue.ToString());
                            ReturnValue.Add(cvalue, rptValue.strFormat);
                        }

                        if (rptValue.strFunction.Length != 0)
                        {
                            TVariant fvalue = EvaluateFunctionString(rptValue.strFunction);
                            ValidateAgainstSQLInjection(fvalue.ToString());
                            ReturnValue.Add(fvalue, rptValue.strFormat);
                        }
                    }
                }
            }

            return ReturnValue;
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TRptDataCalcParameter : TRptSituation
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
        public TRptDataCalcParameter(TParameterList parameters,
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
        public TRptDataCalcParameter(TRptSituation situation) : base(situation)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="query"></param>
        /// <param name="rptGrpParameter"></param>
        /// <returns></returns>
        public TRptFormatQuery Calculate(TRptFormatQuery query, List <TRptParameter>rptGrpParameter)
        {
            foreach (TRptParameter rptParameter in rptGrpParameter)
            {
                string name = rptParameter.strName;
                TVariant value;

                if (rptParameter.strValue.Length != 0)
                {
                    value = new TVariant(rptParameter.strValue);
                    Parameters.Add(name, value, -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                }
                else
                {
                    if (rptParameter.rptGrpValue != null)
                    {
                        TRptDataCalcValue rptDataCalcValue = new TRptDataCalcValue(this);
                        value = rptDataCalcValue.Calculate(rptParameter.rptGrpValue).ConvertToVariant();
                        Parameters.Add(name, value, -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                    }
                    else
                    {
                        value = Parameters.Get(name, column, Depth);
                    }
                }

                if (value.IsNil())
                {
                    TLogging.Log(
                        "Variable " + name + " could not be found (column: " + column.ToString() + "; level: " + Depth.ToString() + ")." + ' ' +
                        query.SQLStmt);
                }

                query.AddOdbcParameters("{{", name, "}}", value);
                query.AddOdbcParameters("{", name, "}", value);
            }

            return query;
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TRptDataCalcCalculation : TRptEvaluator
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
        public TRptDataCalcCalculation(TParameterList parameters,
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
        public TRptDataCalcCalculation(TRptSituation situation) : base(situation)
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="rptGrpParameter"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private TRptFormatQuery ApplyParametersToQuery(List <TRptParameter>rptGrpParameter, TRptFormatQuery query)
        {
            if (rptGrpParameter != null)
            {
                TRptDataCalcParameter rptDataCalcParameter = new TRptDataCalcParameter(this);
                return rptDataCalcParameter.Calculate(query, rptGrpParameter);
            }

            return query;
        }

        private SortedList <string, Assembly>FReportAssemblies = new SortedList <string, Assembly>();

        /// <summary>
        /// instead of executing an sql query, we can run a method that returns a DataTable
        /// </summary>
        /// <param name="ANamespaceClassAndMethodName"></param>
        private DataTable CalculateFromMethod(string ANamespaceClassAndMethodName)
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
                try
                {
                    return (DataTable)method.Invoke(null, new object[] { this.Parameters, this.Results });
                }
                catch (Exception e)
                {
                    TLogging.Log("Problem while calling " + ANamespaceClassAndMethodName);
                    TLogging.Log(e.ToString());
                    return null;
                }
            }
            else
            {
                throw new Exception("cannot find method " + className + "." + methodName);
            }
        }

        private String ReplaceQuotesForSql(String strSql, String CalculationId)
        {
            if (strSql.IndexOf("\"") > 0)
            {
                TLogging.Log("Warning: SQL Contains double quotes for calculation \"" + CalculationId + "\"");
                strSql = strSql.Replace('"', '\'');
            }

            return strSql;
        }

        /// <summary>
        /// execute sql query, or do any other calculation to get the result
        /// </summary>
        /// <param name="rptCalculation"></param>
        /// <param name="rptGrpParameter"></param>
        /// <param name="strLowerLevel">can be empty string if there is no lower level to be calculated depending on the result of this query</param>
        /// <param name="masterRow"></param>
        public TVariant EvaluateCalculation(TRptCalculation rptCalculation,
            List <TRptParameter>rptGrpParameter,
            string strLowerLevel,
            int masterRow)
        {
            // depending on existing lower level, we want the parameters on that level, otherwise on the current level
            int StoreResultsAtDepth = (strLowerLevel.Length == 0) ? Depth : Depth + 1;

            TRptFormatQuery qry = this.Calculate(rptCalculation, rptGrpParameter);
            string strSql = String.Empty;

            if (qry.IsVariant)
            {
                strSql = qry.VariantValue.ToString();
            }
            else
            {
                strSql = qry.SQLStmt;
            }

            if (strSql.Length == 0)
            {
                return new TVariant();
            }

            // ShowMessage (query);
            if (strSql.StartsWith("CSV:"))
            {
                strSql = strSql.Substring(4);

                while (strSql.Length > 0)
                {
                    string strReturns = rptCalculation.strReturns;

                    while (strReturns.Length != 0)
                    {
                        string strName = StringHelper.GetNextCSV(ref strReturns).Trim();

                        // the parameters are stored first
                        Parameters.Add(strName, StringHelper.GetNextCSV(
                                ref strSql).Trim(), -1, Depth + 1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                    }

                    if (strLowerLevel != String.Empty)
                    {
                        TRptDataCalcLevel rptDataCalcLevel = new TRptDataCalcLevel(this);
                        rptDataCalcLevel.Depth++;
                        rptDataCalcLevel.Calculate(CurrentReport.GetLevel(strLowerLevel), masterRow);
                    }
                }
            }
            else if (strSql.StartsWith("Ict.Petra.Server."))
            {
                DataTable tab = CalculateFromMethod(strSql);

                if (tab == null)
                {
                    throw new Exception("Error in " + strSql);
                }
                else if (tab.Rows.Count > 0)
                {
                    string strReturns = rptCalculation.strReturns;

                    if (strReturns.ToLower() == "automatic")
                    {
                        strReturns = string.Empty;

                        foreach (DataColumn col in tab.Columns)
                        {
                            strReturns = StringHelper.AddCSV(strReturns, col.ColumnName);
                        }
                    }

                    foreach (DataRow row in tab.Rows)
                    {
                        this.AddResultsToParameter(strReturns, rptCalculation.strReturnsFormat, row, StoreResultsAtDepth);

                        if (strLowerLevel != String.Empty)
                        {
                            TRptDataCalcLevel rptDataCalcLevel = new TRptDataCalcLevel(this);
                            rptDataCalcLevel.Depth++;
                            rptDataCalcLevel.Calculate(CurrentReport.GetLevel(strLowerLevel), masterRow);
                        }
                    }
                }
            }
            else
            {
                if (strSql.StartsWith("NO-SQL"))
                {
                    // Don't execute the result as SQL. This can be used to sequentially execute calculations/functions in a query.
                    // example: ap_payment_export, Select Payments by Batch Number
                    strSql = strSql.Substring(7);
                    Parameters.Add(rptCalculation.strId, strSql, -1, StoreResultsAtDepth, null, null, ReportingConsts.CALCULATIONPARAMETERS);

                    if (strLowerLevel != String.Empty)
                    {
                        TRptDataCalcLevel rptDataCalcLevel = new TRptDataCalcLevel(this);
                        rptDataCalcLevel.Depth++;
                        rptDataCalcLevel.Calculate(CurrentReport.GetLevel(strLowerLevel), masterRow);
                    }

                    return this.Parameters.Get(rptCalculation.strId, -1, StoreResultsAtDepth, eParameterFit.eBestFitEvenLowerLevel);
                }
                else if (strSql.Length > 0)
                {
                    strSql = ReplaceQuotesForSql(strSql, rptCalculation.strId);
                    DataTable tab = DatabaseConnection.SelectDT(strSql, "EvaluateCalculation_TempTable", DatabaseConnection.Transaction, qry.OdbcParameters.ToArray());
                    string strReturns = rptCalculation.strReturns;

                    if (strReturns.ToLower() == "automatic")
                    {
                        strReturns = "";

                        foreach (DataColumn col in tab.Columns)
                        {
                            strReturns = StringHelper.AddCSV(strReturns, col.ColumnName);
                        }
                    }

                    foreach (DataRow row in tab.Rows)
                    {
                        this.AddResultsToParameter(strReturns, rptCalculation.strReturnsFormat, row, StoreResultsAtDepth);

                        if (strLowerLevel != String.Empty)
                        {
                            TRptDataCalcLevel rptDataCalcLevel = new TRptDataCalcLevel(this);
                            rptDataCalcLevel.Depth++;
                            rptDataCalcLevel.Calculate(CurrentReport.GetLevel(strLowerLevel), masterRow);
                        }
                    }
                }
            } // else

            if (this.Parameters.Exists(rptCalculation.strReturns, -1, StoreResultsAtDepth, eParameterFit.eBestFitEvenLowerLevel))
            {
                return this.Parameters.Get(rptCalculation.strReturns, -1, StoreResultsAtDepth, eParameterFit.eBestFitEvenLowerLevel);
            }

            return new TVariant();
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="calculation"></param>
        /// <returns></returns>
        public TVariant EvaluateHelperCalculation(String calculation)
        {
            TVariant ReturnValue;
            TRptCalculation rptCalculation;
            DataTable tab;
            string strReturns;
            Boolean firstRow;

            ReturnValue = new TVariant();
            rptCalculation = ReportStore.GetCalculation(CurrentReport, calculation);

            if (rptCalculation == null)
            {
                TLogging.Log("Calculation " + calculation + " could not be found.");
                return ReturnValue;
            }

            ReturnValue = Calculate(rptCalculation, null).VariantValue;

            if (ReturnValue.IsZeroOrNull() || (ReturnValue.ToString().Trim().ToUpper().IndexOf("SELECT") != 0))
            {
                return ReturnValue;
            }

            if (rptCalculation.strReturnsFormat.ToLower() == "list")
            {
                // reset the variables, so we are able to start a new list
                strReturns = rptCalculation.strReturns;

                while (strReturns.Length != 0)
                {
                    Parameters.Add(StringHelper.GetNextCSV(ref strReturns).Trim(), "", -1, Depth, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                }
            }

            String strSql = ReturnValue.ToString();
            strSql = ReplaceQuotesForSql(strSql, rptCalculation.strId);
            tab = DatabaseConnection.SelectDT(strSql, "EvaluateHelperCalculation", DatabaseConnection.Transaction);

            if ((tab == null) || (tab.Rows.Count == 0))
            {
                // reset the variables, we don't want to display the results of the previous calculation
                strReturns = rptCalculation.strReturns;

                while (strReturns.Length != 0)
                {
                    Parameters.Add(StringHelper.GetNextCSV(ref strReturns).Trim(), "", -1, Depth, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                }
            }

            ReturnValue = new TVariant();
            firstRow = true;

            foreach (DataRow row in tab.Rows)
            {
                if (!firstRow)
                {
                    ReturnValue.Add(new TVariant(", "));
                }

                firstRow = false;

                if (Convert.ToString(row[0]) != new TVariant(row[0]).ToString())
                {
                    ReturnValue.Add(new TVariant(Convert.ToString(row[0])));
                }
                else
                {
                    ReturnValue.Add(new TVariant(row[0]));
                }

                AddResultsToParameter(rptCalculation.strReturns, rptCalculation.strReturnsFormat, row, Depth);
            }

            return ReturnValue;
        }

        /// <summary>
        /// this function is used for calculations that are based on a function, e.g. variance
        /// it returns true, if the first (and only) value is a function, and there is no condition
        /// </summary>
        /// <param name="rptGrpValue"></param>
        /// <param name="strFunction"></param>
        /// <returns></returns>
        private Boolean GetFunctionCalculation(List <TRptValue>rptGrpValue, out String strFunction)
        {
            TRptValue rptValue;

            strFunction = "";
            Boolean ReturnValue = false;

            if ((rptGrpValue != null) && (rptGrpValue.Count == 1))
            {
                rptValue = (TRptValue)rptGrpValue[0];

                if ((rptValue.strFunction.Length != 0) && (rptValue.strCondition.Length == 0))
                {
                    strFunction = rptValue.strFunction;
                    ReturnValue = true;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="rptGrpQuery"></param>
        /// <param name="precalculatedColumns"></param>
        /// <param name="resultValue"></param>
        /// <returns></returns>
        public Boolean EvaluateCalculationFunction(List <TRptQuery>rptGrpQuery, ref TVariant[] precalculatedColumns, ref TVariant resultValue)
        {
            Boolean ReturnValue;
            String strFunction;
            TRptQuery rptFirstQuery;

            ReturnValue = false;
            resultValue = new TVariant();
            rptFirstQuery = null;

            if ((rptGrpQuery != null) && (rptGrpQuery.Count == 1))
            {
                rptFirstQuery = (TRptQuery)rptGrpQuery[0];
            }

            if ((rptFirstQuery != null) && GetFunctionCalculation(rptFirstQuery.rptGrpValue, out strFunction))
            {
                resultValue = EvaluateFunctionCalculation(strFunction, precalculatedColumns);
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="rptCalculation"></param>
        /// <param name="rptGrpParameter"></param>
        /// <param name="rptGrpTemplate"></param>
        /// <param name="rptGrpQuery"></param>
        /// <returns></returns>
        public TRptFormatQuery EvaluateCalculationAll(TRptCalculation rptCalculation,
            List <TRptParameter>rptGrpParameter,
            List <TRptQuery>rptGrpTemplate,
            List <TRptQuery>rptGrpQuery)
        {
            TRptFormatQuery ReturnValue;
            TRptDataCalcValue rptDataCalcValue;
            TRptDataCalcSwitch rptDataCalcSwitch;

            ReturnValue = new TRptFormatQuery(string.Empty, null, this.Parameters, this.column, this.Depth);

            if (rptGrpQuery == null)
            {
                return ReturnValue;
            }

            foreach (TRptQuery rptQuery in rptGrpQuery)
            {
                if (EvaluateCondition(rptQuery.strCondition))
                {
                    if (rptQuery.rptGrpValue != null)
                    {
                        rptDataCalcValue = new TRptDataCalcValue(this);

                        if (!ReturnValue.IsZeroOrNull())
                        {
                            ReturnValue.Add(" ");
                        }

                        ReturnValue.Add(rptDataCalcValue.Calculate(rptQuery.rptGrpValue, true));
                    }

                    if (rptQuery.rptGrpParameter != null)
                    {
                        // insert template with parameters
                        ReturnValue.Add(" ");
                        ReturnValue.Add(EvaluateCalculationAll(rptCalculation, rptQuery.rptGrpParameter, null, rptGrpTemplate));
                    }

                    if (rptQuery.rptGrpSwitch != null)
                    {
                        if (!ReturnValue.IsZeroOrNull())
                        {
                            ReturnValue.Add(" ");
                        }

                        rptDataCalcSwitch = new TRptDataCalcSwitch(this);
                        ReturnValue.Add(rptDataCalcSwitch.Calculate(rptQuery.rptGrpSwitch).ConvertToVariant());
                    }
                }
            }

            if (ReturnValue.SQLStmt.IndexOf("{") != -1)
            {
                ReturnValue = ApplyParametersToQuery(rptGrpParameter, ReturnValue);
                ReturnValue.ReplaceVariables();
            }

            // TLogging.log('Result of TRptDataCalcCalculation.evaluateCalculationAll: '+result.encodetostring());
            return ReturnValue;
        }

        /// <summary>
        /// </summary>
        /// <param name="rptCalculation"></param>
        /// <param name="rptGrpParameter"></param>
        /// <returns></returns>
        public TRptFormatQuery Calculate(TRptCalculation rptCalculation, List <TRptParameter>rptGrpParameter)
        {
            return EvaluateCalculationAll(rptCalculation, rptGrpParameter, rptCalculation.rptGrpTemplate, rptCalculation.rptGrpQuery);
        }

        /// <summary>
        /// This procedure adds the returned values from the sql query to the Parameters,
        /// according to the returns attribute of the calculation
        /// </summary>
        /// <returns>void</returns>
        public void AddResultsToParameter(String AStrReturns, String AStrReturnsFormat, DataRow ARow, Int32 ADepth)
        {
            // the results of each column should be added to one variable per column

            String strReturns = AStrReturns;
            Boolean AddToList = AStrReturnsFormat.ToLower() == "list";

            while (strReturns.Length != 0)
            {
                String strName = StringHelper.GetNextCSV(ref strReturns).Trim();

                // in sqlite, eg. accountdetailcommon.xml, select j.a_transaction_currency_c will keep the j; other db's will remove the table name
                if (!ARow.Table.Columns.Contains(strName))
                {
                    foreach (DataColumn col in ARow.Table.Columns)
                    {
                        if (col.ColumnName.Contains("." + strName))
                        {
                            strName = col.ColumnName;
                        }
                    }
                }

                TVariant value;

                // the parameters are stored first
                if (ARow[strName].GetType() == typeof(String))
                {
                    value = new TVariant(Convert.ToString(ARow[strName]));
                }
                else
                {
                    value = new TVariant(ARow[strName]);
                }

                Boolean ClearParameter = true;

                if ((AddToList) && (!value.IsNil()))
                {
                    TVariant newValue = new TVariant(Parameters.Get(strName, -1, ADepth));
                    newValue.Add(value, "", false);

                    // no format given, that needs to be done in the column calculation or field value
                    Parameters.Add(strName, newValue, -1, ADepth, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                    ClearParameter = false;
                }
                else
                {
                    value.ApplyFormatString(AStrReturnsFormat);

                    // only add values that are displayed on the sheet; required for the credit and debit columns on an account detail report; the sum would be wrong
                    if (value.ToFormattedString().Length != 0)
                    {
                        if (value.IsNil())
                        {
                            // eg. Sql Sum should return 0, not null. AStrReturnsFormat is Currency
                            value = new TVariant(value.ToFormattedString());
                        }

                        Parameters.Add(strName, value, -1, ADepth, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                        ClearParameter = false;
                    }
                }

                if (ClearParameter == true)
                {
                    // make sure that the variable is empty; otherwise the calculator would perhaps use the values from previous rows.
                    // this is important for all columns that are directly based on results from the query
                    Parameters.Add(strName, "", -1, ADepth, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                }
            }
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    public class TRptDataCalcHeaderFooter : TRptSituation
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
        public TRptDataCalcHeaderFooter(TParameterList parameters,
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
        /// <param name="depth"></param>
        /// <param name="column"></param>
        /// <param name="lineId"></param>
        /// <param name="parentRowId"></param>
        public TRptDataCalcHeaderFooter(TRptSituation situation, int depth, int column, int lineId, int parentRowId) :
            base(situation, depth, column, lineId, parentRowId)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="situation"></param>
        public TRptDataCalcHeaderFooter(TRptSituation situation) : base(situation)
        {
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="rptGrpField"></param>
        /// <param name="rptGrpSwitch"></param>
        /// <param name="strLine"></param>
        /// <param name="strSpace"></param>
        public void Calculate(List <TRptField>rptGrpField, List <TRptSwitch>rptGrpSwitch, string strLine = "", string strSpace = "")
        {
            TRptDataCalcSwitch calcSwitch;
            TRptDataCalcField calcField;

            if ((strLine != null) && (strLine.IndexOf("above") != -1))
            {
                GetParameters().Add("FullLineAbove", new TVariant(true), -1, Depth);
            }

            if ((strLine != null) && (strLine.IndexOf("below") != -1))
            {
                GetParameters().Add("FullLineBelow", new TVariant(true), -1, Depth);
            }

            if ((strSpace != null) && (strSpace.IndexOf("above") != -1))
            {
                GetParameters().Add("SpaceLineAbove", new TVariant(true), -1, Depth);
            }

            if ((strSpace != null) && (strSpace.IndexOf("below") != -1))
            {
                GetParameters().Add("SpaceLineBelow", new TVariant(true), -1, Depth);
            }

            if (rptGrpSwitch != null)
            {
                column = -1;
                calcSwitch = new TRptDataCalcSwitch(this);
                calcSwitch.Calculate(rptGrpSwitch);
            }

            if (rptGrpField != null)
            {
                calcField = new TRptDataCalcField(this);
                calcField.Calculate(rptGrpField);
            }
        }
    }
}