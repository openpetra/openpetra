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
using Ict.Petra.Shared.MReporting;
using System.Collections;
using Ict.Common;
using Ict.Common.DB;

namespace Ict.Petra.Server.MReporting
{
    /// <summary>
    /// this class can evaluate expressions
    ///
    /// it evaluates condition and functions strings, and returns a value.
    /// It uses TVariant to encode the values.
    /// There is a unit test in Testing/Reporting/TestEvaluator for this unit.
    /// </summary>
    public class TRptEvaluator : TRptSituation
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
        public TRptEvaluator(TParameterList parameters,
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
        public TRptEvaluator(TRptSituation situation) : base(situation)
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
        public TRptEvaluator(TRptSituation situation, int depth, int column, int lineId, int parentRowId) : base(situation, depth, column, lineId,
                                                                                                                parentRowId)
        {
        }

        /// <summary>of System.Type</summary>
        private static ArrayList FUserFunctions = new ArrayList();
        private static TVariant[] CurrentColumns;

        /// <summary>
        /// check if a function actually points to a calculation rather than containing an expression
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Boolean IsFunctionCalculation(String name)
        {
            return ReportStore.IsFunctionCalculation(CurrentReport, name);
        }

        /// <summary>
        /// evaluate an expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Boolean EvaluateCondition(String expression)
        {
            Boolean ReturnValue;

            ReturnValue = true;

            if (expression.Trim().Length > 0)
            {
                ReturnValue = EvaluateFunctionString(expression).ToBool();
            }

            return ReturnValue;
        }

        /// <summary>
        /// evaluate a function call expression
        /// </summary>
        /// <returns>void</returns>
        public TVariant EvaluateFunctionString(String f)
        {
            int after;

            if (f.Length == 0)
            {
                return new TVariant();
            }

            after = -1;
            return EvaluateOperator(f, 0, out after);
        }

        /// <summary>
        /// evaluate one mini expression, one operator and several operands;
        /// works recursively
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="position"></param>
        /// <param name="after"></param>
        /// <returns></returns>
        private TVariant EvaluateOperator(String expression, int position, out int after)
        {
            TVariant ReturnValue;
            int posBracketOpen;

            TVariant[] ops = new TVariant[ReportingConsts.MAX_FUNCTION_PARAMETER + 1 - 0 + 1];
            TVariant op;
            String Operator;
            int counter;
            after = -1;
            posBracketOpen = expression.IndexOf('(', position);

            if (posBracketOpen == -1)
            {
                if (expression.Substring(position).Length == 0)
                {
                    TLogging.Log("ERROR bracket expected: " + expression);
                    return new TVariant();
                }

                // this might be a single operand
                after = expression.IndexOf(')', position);

                if (after == -1)
                {
                    op = new TVariant(expression.Substring(position));
                    ReplaceFunctionVariables(ref op);
                    ReturnValue = op;
                    after = expression.Length;
                }
                else
                {
                    ReturnValue = new TVariant(expression.Substring(position, after - position));
                }

                return ReturnValue;
            }

            Operator = expression.Substring(position, posBracketOpen - position);
            counter = 0;

            do
            {
                counter++;

                if (counter > ReportingConsts.MAX_FUNCTION_PARAMETER)
                {
                    TLogging.Log("ERROR too many parameters");
                    return new TVariant();
                }

                ops[counter] = IdentifyOperand(expression, posBracketOpen + 1, out after);

                if (expression.IndexOf(',', after) == after)
                {
                    posBracketOpen = after;
                }
                else
                {
                    posBracketOpen = 0;
                }
            } while (!(posBracketOpen == 0));

            ReturnValue = EvaluateFunction(Operator, ops);
            after++;
            return ReturnValue;
        }

        /// <summary>
        /// this splits the operators and returns a TVariant (can be a string with SUBCALL: in front of it) no evaluation of subcalls or variables is done, to prevent unwanted side effects
        /// </summary>
        /// <returns>void</returns>
        private TVariant IdentifyOperand(String expression, int position, out int after)
        {
            TVariant ReturnValue;
            int posBracketOpen;
            int posBracketClose;
            int posComma;
            int NumberOfOpenBrackets;
            int origPosition;

            after = -1;
            ReturnValue = new TVariant();
            posBracketOpen = expression.IndexOf('(', position);
            posBracketClose = expression.IndexOf(')', position);
            posComma = expression.IndexOf(',', position);

            if ((posComma >= 0) && (posComma < posBracketClose) && ((posComma < posBracketOpen) || (posBracketOpen == -1)))
            {
                // some parameter, literal
                if (posComma - position < 0)
                {
                    TLogging.Log("error with expression: " + expression);
                    return ReturnValue;
                }

                ReturnValue = new TVariant(expression.Substring(position, posComma - position));
                after = posComma;
            }
            else if ((posBracketClose < posBracketOpen) || (posBracketOpen == -1))
            {
                // last parameter, literal
                if (posBracketClose - position < 0)
                {
                    TLogging.Log("error with expression: " + expression);
                    return ReturnValue;
                }

                ReturnValue = new TVariant(expression.Substring(position, posBracketClose - position));
                after = posBracketClose;
            }
            else if (posBracketOpen < posBracketClose)
            {
                // another operator;
                // should not be evaluated here, to prevent unwanted side effects, e.g. with and/or
                // result := evaluateOperator(expression, position, after);
                // find the string until the bracket is closed again
                NumberOfOpenBrackets = 1;
                posBracketClose = -1;
                expression = expression.Substring(position);
                origPosition = position + 1;
                position = expression.IndexOf('(') + 1;

                while ((position < expression.Length) && (NumberOfOpenBrackets != 0))
                {
                    if (expression[position] == '(')
                    {
                        NumberOfOpenBrackets++;
                    }
                    else if (expression[position] == ')')
                    {
                        NumberOfOpenBrackets--;
                        posBracketClose = position;
                    }

                    position++;
                    after = posBracketClose + origPosition;
                    ReturnValue = new TVariant("SUBCALL:" + expression.Substring(0, posBracketClose + 1));
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// this is only called when we know for sure that we need to know the value of this operand
        /// </summary>
        /// <returns>void</returns>
        private TVariant EvaluateOperand(TVariant op)
        {
            int after;

            if (op != null)
            {
                if (op.ToString().IndexOf("SUBCALL:") == 0)
                {
                    after = -1;
                    op = EvaluateOperator(op.ToString().Substring(8), 0, out after);
                }

                ReplaceFunctionVariables(ref op);
            }

            return op;
        }

        private void ReplaceFunctionVariables(ref TVariant v)
        {
            String s;
            TVariant testS;

            TVariant[] ops = new TVariant[ReportingConsts.MAX_FUNCTION_PARAMETER + 1 - 0 + 1];
            Int32 counter;

            if (v.TypeVariant == eVariantTypes.eString)
            {
                s = v.ToString().Trim();
                s = s.Replace("{{lineId}}", StringHelper.IntToStr(LineId));
                s = s.Replace("{{column}}", StringHelper.IntToStr(column));
                s = s.Replace("{{level}}", StringHelper.IntToStr(Depth));

                // make sure that e.g. HasChildRows is evaluated
                for (counter = 1; counter <= ReportingConsts.MAX_FUNCTION_PARAMETER; counter += 1)
                {
                    ops[counter] = null;
                }

                testS = FunctionSelector(s, ops);

                if (testS != null)
                {
                    s = testS.ToString();
                }

                v = ReplaceVariables(s, false);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="s"></param>
        /// <param name="withQuotes"></param>
        /// <returns></returns>
        protected TVariant ReplaceVariables(String s, Boolean withQuotes)
        {
            TVariant ReturnValue;
            TRptFormatQuery formatQuery;

            formatQuery = new TRptFormatQuery(Parameters, column, Depth);
            ReturnValue = formatQuery.ReplaceVariables(s, withQuotes);
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected TVariant ReplaceVariables(String s)
        {
            return ReplaceVariables(s, true);
        }

        private TVariant EvaluateFunction(String f, TVariant[] ops)
        {
            f = f.Trim();
            return FunctionSelector(f, ops);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="f"></param>
        /// <param name="precalculatedColumns"></param>
        /// <returns></returns>
        public TVariant EvaluateFunctionCalculation(String f, TVariant[] precalculatedColumns)
        {
            CurrentColumns = precalculatedColumns;
            return EvaluateFunctionString(f);
        }

        //
        /// <summary>
        /// a function to calculate the sum of the rows that report to this row
        /// </summary>
        /// <returns>void</returns>
        private decimal GetSumChildren(ref ArrayList children, int column, Boolean AUseDebitCreditIndicator)
        {
            decimal sum;
            decimal value;
            Boolean debit_credit_indicator;

            // true if there is no parameter for this row and ReportingConsts.COLUMN(s)
            Boolean FindDebitCreditIndicator;
            Boolean FirstChild;

            sum = 0;
            debit_credit_indicator = true;

            // the debit_credit_indicator is either already set, e.g. getSumLowerReportCredit,
            // or needs to be set depending on one of the children.
            // In that case we assume, they have the same debit_credit_indicator
            // otherwise raise an exception
            // there is the other situation, e.g. Trial Balance, were debits and credits are printed
            // in separate columns; they still need to be summed up; we don't want to use the debit credit indicator there,
            // and all amounts are treated as positive numbers
            if (AUseDebitCreditIndicator)
            {
                FindDebitCreditIndicator = (!GetParameters().Exists("debit_credit_indicator", column, Depth, eParameterFit.eAllColumnFit));

                if (!FindDebitCreditIndicator)
                {
                    debit_credit_indicator = GetParameters().Get("debit_credit_indicator", column, Depth, eParameterFit.eAllColumnFit).ToBool();
                }
            }
            else
            {
                debit_credit_indicator = false;
                FindDebitCreditIndicator = false;
            }

            FirstChild = true;

            foreach (TResult element in children)
            {
                // If the row won't be displayed (e.g. filtered out by IsLapsedDonor() using DONTDISPLAYROW parameter) then don't bother adding it up. 
                if (!element.display) continue;

                if ((element.column != null) && (!element.column[column].IsZeroOrNull()))
                {
                    if (FindDebitCreditIndicator == true)
                    {
                        if (FirstChild == true)
                        {
                            FirstChild = false;
                            debit_credit_indicator = element.debit_credit_indicator;
                        }
                        else if ((element.debit_credit_indicator != debit_credit_indicator) && (AUseDebitCreditIndicator == true))
                        {
                            throw new Exception("Problem with different Debit/Credit indicators in Totals");
                        }
                    }

                    value = element.column[column].ToDecimal();

                    if ((!AUseDebitCreditIndicator) || (debit_credit_indicator == element.debit_credit_indicator))
                    {
                        sum = sum + value;
                    }
                    else
                    {
                        sum = sum - value;
                    }
                }
            }

            if (AUseDebitCreditIndicator)
            {
                GetParameters().Add("debit_credit_indicator", debit_credit_indicator);
            }

            return sum;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="children"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private decimal GetSumChildren(ref ArrayList children, int column)
        {
            return GetSumChildren(ref children, column, true);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        protected decimal GetSumLowerReportCredit(int lineId, int column)
        {
            decimal ReturnValue;
            ArrayList children;

            GetParameters().Add("debit_credit_indicator", new TVariant(false), column, Depth);
            children = new ArrayList();
            Results.GetChildRows(lineId, ref children);
            ReturnValue = GetSumChildren(ref children, column);
            children.Clear();
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="column"></param>
        /// <param name="AUseDebitCreditIndicator"></param>
        /// <returns></returns>
        protected decimal GetSumLowerReport(int lineId, int column, Boolean AUseDebitCreditIndicator)
        {
            decimal ReturnValue;
            ArrayList children;

            children = new ArrayList();
            Results.GetChildRows(lineId, ref children);
            ReturnValue = GetSumChildren(ref children, column, AUseDebitCreditIndicator);
            children.Clear();
            return ReturnValue;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        protected decimal GetSumLowerReport(int lineId, int column)
        {
            return GetSumLowerReport(lineId, column, true);
        }

        /// <summary>
        /// this does the same as getSumLowerReport, but goes one level deeper.
        /// This is needed e.g. in the account details report
        /// </summary>
        /// <returns>void</returns>
        protected decimal GetSumLower2Report(int lineId, int column, Boolean AUseDebitCreditIndicator)
        {
            decimal ReturnValue;
            ArrayList children;

            children = new ArrayList();
            Results.GetGrandChildRows(lineId, ref children);
            ReturnValue = GetSumChildren(ref children, column, AUseDebitCreditIndicator);
            children.Clear();
            return ReturnValue;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        protected decimal GetSumLower2Report(int lineId, int column)
        {
            return GetSumLower2Report(lineId, column, true);
        }

        /// <summary>
        /// This function will only return the first line of the caption of the given column
        /// </summary>
        /// <returns>the first line of the caption
        /// </returns>
        protected String GetShortCaption(int Acolumn)
        {
            return GetParameters().Get("ColumnCaption", Acolumn).ToString();
        }

        /// <summary>
        /// This function will return the complete caption of the given column
        /// </summary>
        /// <returns>the caption
        /// </returns>
        protected String GetCaption(int Acolumn)
        {
            String ReturnValue;

            ReturnValue = GetParameters().Get("ColumnCaption", Acolumn).ToString();

            if (GetParameters().Exists("ColumnCaption2", Acolumn, -1))
            {
                ReturnValue = ReturnValue + ' ' + GetParameters().Get("ColumnCaption2", Acolumn).ToString();
            }

            if (GetParameters().Exists("ColumnCaption3", Acolumn, -1))
            {
                ReturnValue = ReturnValue + ' ' + GetParameters().Get("ColumnCaption3", Acolumn).ToString();
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="child"></param>
        /// <param name="col"></param>
        /// <param name="master"></param>
        /// <param name="val"></param>
        /// <param name="masterlevel"></param>
        /// <returns></returns>
        protected Boolean GetParentLine(int child, int col, ref int master, ref decimal val, ref int masterlevel)
        {
            Boolean ReturnValue;
            TResult row;

            row = Results.GetRow(child);
            masterlevel = -1;
            ReturnValue = false;

            if (row != null)
            {
                master = row.masterRow;
                masterlevel = row.depth;
                val = row.column[col].ToDecimal();
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="child"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        protected TVariant GetChildValue(int child, int col)
        {
            TVariant ReturnValue;
            TResult row;

            row = Results.GetFirstChildRow(child);
            ReturnValue = new TVariant();

            if (row != null)
            {
                ReturnValue = row.column[col];
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="child"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        protected TVariant GetParentValue(int child, int col)
        {
            TVariant ReturnValue;
            TResult row;

            row = Results.GetRow(child);
            ReturnValue = new TVariant();

            if (row != null)
            {
                ReturnValue = row.column[col];
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="child"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        protected TVariant Get2ndLevelValue(int child, int col)
        {
            TVariant ReturnValue;
            int master;
            decimal masterval;
            int masterlevel;

            ReturnValue = new TVariant();
            master = child;
            masterval = CurrentColumns[col].ToDecimal();
            masterlevel = Depth;

            while (masterlevel > 2)
            {
                child = master;
                GetParentLine(child, col, ref master, ref masterval, ref masterlevel);
            }

            if ((masterlevel == 2) && (masterval != 0))
            {
                ReturnValue = new TVariant(masterval);
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="child"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        protected TVariant Get1stLevelValue(int child, int col)
        {
            TVariant ReturnValue;
            int master;
            decimal masterval;
            int masterlevel;

            ReturnValue = new TVariant();
            master = child;
            masterval = CurrentColumns[col].ToDecimal();
            masterlevel = Depth;

            while (masterlevel > 1)
            {
                child = master;
                GetParentLine(child, col, ref master, ref masterval, ref masterlevel);
            }

            if ((masterlevel == 1) && (masterval != 0))
            {
                ReturnValue = new TVariant(masterval);
            }

            return ReturnValue;
        }

        /// <summary>
        /// function selection
        /// </summary>
        /// <param name="f">function name</param>
        /// <param name="ops">if ops[1] is nil, and f cannot be evaluated, return nil; otherwise if ops is not nil, print an error if f cannot be evaluated</param>
        /// <returns>void</returns>
        private TVariant FunctionSelector(String f, TVariant[] ops)
        {
            TVariant ReturnValue = null;
            TRptUserFunctions rptUserFunctions;
            String s;
            String s2;

            System.Int32 start;
            System.Int32 length;
            String logMessage;
            bool FunctionFound;
            int counter;
            f = f.ToLower();
            TLogging.SetContext("call to function " + f);
            TParameterList myParams = GetParameters();

            if ((f == "eq") || (f == "ne"))
            {
                // check if at least one of the parameters is a variable; otherwise give warning
                if ((ops[1].ToString().IndexOf('{') == -1) && (ops[2].ToString().IndexOf('{') == -1))
                {
                    TLogging.Log(
                        "Warning: comparison should contain at least one variable: " + f.ToString() + '(' + ops[1].ToString() + ',' + ops[2].ToString(
                            ) +
                        ')', TLoggingType.ToLogfile | TLoggingType.ToConsole);
                }
            }

            if ((f == "isnull") || (f == "exists") || (f == "or") || (f == "and") || (f == "iif") || (f == "assign"))
            {
                // need to replace the variables manually
                // either because we don't want them replaced at all (isnull or exists needs the variable name),
                // or because we don't want to evaluate the second parameter if the first one already defines the result (e.g. or)
            }
            else
            {
                for (counter = 1; counter <= ReportingConsts.MAX_FUNCTION_PARAMETER; counter += 1)
                {
                    ops[counter] = EvaluateOperand(ops[counter]);
                }
            }

            if (f == "eq")
            {
                ReturnValue = new TVariant(ops[1].CompareToI(ops[2]) == 0);
            }
            else if (f == "ne")
            {
                ReturnValue = new TVariant(ops[1].CompareToI(ops[2]) != 0);
            }
            else if (f == "lt")
            {
                ReturnValue = new TVariant(ops[1].CompareTo(ops[2]) < 0);
            }
            else if (f == "le")
            {
                ReturnValue = new TVariant(ops[1].CompareTo(ops[2]) <= 0);
            }
            else if (f == "gt")
            {
                ReturnValue = new TVariant(ops[1].CompareTo(ops[2]) > 0);
            }
            else if (f == "ge")
            {
                ReturnValue = new TVariant(ops[1].CompareTo(ops[2]) >= 0);
            }
            else if (f == "sub")
            {
                ReturnValue = new TVariant(ops[1].ToDecimal() - ops[2].ToDecimal());
            }
            else if (f == "adddays")
            {
                ReturnValue = new TVariant(ops[1].ToDate().AddDays(ops[2].ToDouble()));
            }
            else if (f == "add")
            {
                ReturnValue = new TVariant(ops[1].ToDecimal() + ops[2].ToDecimal());
            }
            else if (f == "additems")
            {
                length = ops[1].ToInt() + 1;

                decimal result = 0.0M;

                for (counter = 2; counter <= length; ++counter)
                {
                    result += ops[counter].ToDecimal();
                }

                ReturnValue = new TVariant(result);
            }
            else if (f == "mul")
            {
                ReturnValue = new TVariant(ops[1].ToDecimal() * ops[2].ToDecimal());
            }
            else if (f == "div")
            {
                if (ops[2].ToDecimal() == 0)
                {
                    ReturnValue = new TVariant(0.0);
                }
                else
                {
                    ReturnValue = new TVariant(ops[1].ToDecimal() / ops[2].ToDecimal());
                }
            }
            else if (f == "mod")
            {
                if (ops[2].ToDecimal() == 0)
                {
                    ReturnValue = new TVariant(0.0);
                }
                else
                {
                    ReturnValue = new TVariant(ops[1].ToInt64() % ops[2].ToInt64());
                }
            }
            else if (f == "floor")
            {
                ReturnValue = new TVariant(Math.Floor(ops[1].ToDecimal()));
            }
            else if (f == "round")
            {
                ReturnValue = new TVariant(Math.Round(ops[1].ToDecimal()));
            }
            else if (f == "not")
            {
                ReturnValue = new TVariant(!ops[1].ToBool());
            }
            else if (f == "iif")
            {
                // iif ( condition, value_if_true, value_if_false )
                ops[1] = EvaluateOperand(ops[1]);

                if (ops[1].ToBool() == true)
                {
                    ops[2] = EvaluateOperand(ops[2]);
                    ReturnValue = new TVariant(ops[2]);
                }
                else
                {
                    ops[3] = EvaluateOperand(ops[3]);
                    ReturnValue = new TVariant(ops[3]);
                }
            }
            else if (f == "or")
            {
                ops[1] = EvaluateOperand(ops[1]);

                if (ops[1].ToBool() == false)
                {
                    ops[2] = EvaluateOperand(ops[2]);
                    ReturnValue = new TVariant(ops[2].ToBool());
                }
                else
                {
                    ReturnValue = new TVariant(true);
                }
            }
            else if (f == "and")
            {
                ops[1] = EvaluateOperand(ops[1]);

                if (ops[1].ToBool() == true)
                {
                    ops[2] = EvaluateOperand(ops[2]);
                    ReturnValue = new TVariant(ops[2].ToBool());
                }
                else
                {
                    ReturnValue = new TVariant(false);
                }
            }
            else if (f == "log")
            {
                if (ops[2] != null)
                {
                    ReturnValue = new TVariant(ops[1].ToString() + " " + ops[2].ToString());
                }
                else
                {
                    if (myParams.Exists(ops[1].ToString()))
                    {
                        myParams.Debug(ops[1].ToString());
                        ReturnValue = new TVariant();
                    }
                    else
                    {
                        ReturnValue = ops[1];
                    }
                }

                if (!ReturnValue.IsNil())
                {
                    TLogging.Log(ReturnValue.ToString());
                }
            }
            else if (f == "length")
            {
                ReturnValue = new TVariant(ops[1].ToString().Length);
            }
            else if (StringHelper.IsSame(f, "ContainsCSV"))
            {
                ReturnValue = new TVariant(StringHelper.ContainsCSV(ops[1].ToString(), ops[2].ToString()));
            }
            else if (f == "replace")
            {
                ReturnValue = new TVariant(ops[1].ToString().Replace(ops[2].ToString(), ops[3].ToString()));
            }
            else if ((f == "substring") || (f == "substr"))
            {
                s = ops[1].ToString();
                start = ops[2].ToInt();
                length = ops[3].ToInt();

                if ((start < s.Length) && (start + length <= s.Length) && (length > 0))
                {
                    ReturnValue = new TVariant(s.Substring(start, length));
                }
                else
                {
                    ReturnValue = new TVariant("");
                    TLogging.Log("Text is not long enough or length is wrong: " + s + ' ' + start.ToString() + ' ' + length.ToString());
                }
            }
            else if ((f == "substringright") || (f == "substrright"))
            {
                s = ops[1].ToString();
                length = ops[2].ToInt();
                start = s.Length - length;

                if ((start < s.Length) && (start + length <= s.Length) && (length > 0))
                {
                    ReturnValue = new TVariant(s.Substring(start, length));
                }
                else
                {
                    ReturnValue = new TVariant("");
                    TLogging.Log("Text is not long enough or length is wrong: " + s + ' ' + start.ToString() + ' ' + length.ToString());
                }
            }
            else if ((f == "substringwithoutright") || (f == "substrwithoutright"))
            {
                s = ops[1].ToString();
                length = s.Length - ops[2].ToInt();
                start = 0;

                if ((start < s.Length) && (start + length <= s.Length) && (length > 0))
                {
                    ReturnValue = new TVariant(s.Substring(start, length));
                }
                else
                {
                    ReturnValue = new TVariant("");
                    TLogging.Log("Text is not long enough or length is wrong: " + s + ' ' + start.ToString() + ' ' + length.ToString());
                }
            }
            else if (f == "concatenate")
            {
                s = ops[1].ToString();

                s = s + ops[2].ToString();

                ReturnValue = new TVariant(s);
            }
            else if (f == "concatenateww")
            {
                s = ops[1].ToString();
                length = ops[3].ToInt();

                s = s.PadRight(s.Length + length);

                s = s + ops[2].ToString();

                ReturnValue = new TVariant(s);
            }
            else if (f == "concatenatewithcomma")
            {
                s = ops[1].ToString();
                s2 = ops[2].ToString();

                if ((s.Length > 0)
                    && (s2.Length > 0))
                {
                    s = s + ", ";
                }

                s = s + s2;

                ReturnValue = new TVariant(s);
            }
            else if (f == "format")
            {
                ReturnValue = new TVariant(ops[1].ToFormattedString(ops[2].ToString()));
            }
            else if (f == "formattime")
            {
                String separator = ops[1].ToString();
                String hour = ops[2].ToString();
                String min = ops[3].ToString();
                String sec = "";

                if ((ops.Length > 4) && (ops[4] != null))
                {
                    sec = ops[4].ToString();
                }

                if (hour.Length < 2)
                {
                    hour = "0" + hour;
                }

                if (min.Length < 2)
                {
                    min = "0" + min;
                }

                if ((sec.Length < 2) && (sec.Length > 0))
                {
                    sec = "0" + sec;
                }

                if (sec.Length > 0)
                {
                    ReturnValue = new TVariant(hour + separator + min + separator + sec);
                }
                else
                {
                    ReturnValue = new TVariant(hour + separator + min);
                }
            }
            else if (f == "assign")
            {
                string targetVariableName = ops[1].ToString();

                if (targetVariableName.StartsWith("{") && targetVariableName.EndsWith("}"))
                {
                    targetVariableName = targetVariableName.Substring(1, targetVariableName.Length - 2);
                }

                ops[2] = EvaluateOperand(ops[2]);

                if (myParams.Exists(targetVariableName))
                {
                    // we should overwrite the existing variable, not add on another level
                    TParameter origParameter = myParams.GetParameter(targetVariableName);
                    origParameter.value = ops[2];
                }
                else
                {
                    myParams.Add(targetVariableName, ops[2], -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                }

//              TLogging.Log("Assign: " + targetVariableName + "=" + ops[2].ToString());
                ReturnValue = ops[2];
            }
            else if (f == "exists")
            {
                ReturnValue = new TVariant(myParams.Exists(ops[1].ToString(), column, Depth));
            }
            else if (f == "isnull")
            {
                ReturnValue =
                    new TVariant((!myParams.Exists(ops[1].ToString(), column,
                                      Depth) || myParams.Get(ops[1].ToString(), column, Depth).IsZeroOrNull()));
            }
            else if (f == "template")
            {
                TRptCalculation rptTemplate = ReportStore.GetCalculation(CurrentReport, ops[1].ToString());
                TRptDataCalcCalculation rptTempCalculation = new TRptDataCalcCalculation(this);
                ReturnValue = rptTempCalculation.Calculate(rptTemplate, null);
            }
            else if (f == "columnexist")
            {
                String ColumnID = ops[1].ToString();
                bool ColumnExist = false;

                System.Data.DataTable TempTable = myParams.ToDataTable();
                int numColumns = TempTable.Columns.Count;

                foreach (System.Data.DataRow Row in TempTable.Rows)
                {
                    for (int Counter = 0; Counter < numColumns; ++Counter)
                    {
                        if (Row[Counter].ToString() == ColumnID)
                        {
                            ColumnExist = true;
                            break;
                        }
                    }

                    if (ColumnExist)
                    {
                        break;
                    }
                }

                ReturnValue = new TVariant(ColumnExist);
            }
            else if (f == "conditionrow")
            {
                ReturnValue = new TVariant(ops[1]);

                if (ReturnValue.ToBool() == false)
                {
                    // clear this row, we don't want to display it
                    // set all parameters of this row to NULL
                    myParams.Add("DONTDISPLAYROW", new TVariant(true));
                }
                else
                {
                    myParams.Add("DONTDISPLAYROW", new TVariant(false), -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
                }
            }
            else if (f == "column")
            {
                if ((ops[1].ToInt() >= 0) && (ops[1].ToInt() < CurrentColumns.Length))
                {
                    ReturnValue = new TVariant(CurrentColumns[ops[1].ToInt()]);
                }
                else
                {
                    TLogging.Log("referenced column does not exist: " + ops[1].ToString());
                    ReturnValue = new TVariant();
                }
            }
            else if (f == "HasColumns".ToLower())
            {
                ReturnValue = new TVariant(Results.HasColumns(this.LineId));
            }
            else if (f == "HasChildRows".ToLower())
            {
                ReturnValue = new TVariant(Results.HasChildRows(this.LineId));
            }
            else if (f == "CountChildRows".ToLower())
            {
                ReturnValue = new TVariant(Results.CountChildRows(this.LineId));
            }
            else if (f == "HasChildColumns".ToLower())
            {
                ReturnValue = new TVariant(Results.HasChildColumns(this.LineId));
            }
            else if (f == "invisible".ToLower())
            {
                // need to return true so that calculation happens.
                ReturnValue = new TVariant(true);
            }
            else if (f == "fatherColumn")
            {
                ReturnValue = GetParentValue(ParentRowId, ops[1].ToInt());
            }
            else if (f == "childColumn")
            {
                ReturnValue = GetChildValue(LineId, ops[1].ToInt());
            }
            else if (StringHelper.IsSame(f, "SecondLevelColumn"))
            {
                ReturnValue = Get2ndLevelValue(ParentRowId, ops[1].ToInt());
            }
            else if (StringHelper.IsSame(f, "FirstLevelColumn"))
            {
                ReturnValue = Get1stLevelValue(ParentRowId, ops[1].ToInt());
            }
            else if (StringHelper.IsSame(f, "GetShortCaption"))
            {
                ReturnValue = new TVariant(GetShortCaption(ops[1].ToInt()));
            }
            else if (StringHelper.IsSame(f, "GetCaption"))
            {
                ReturnValue = new TVariant(GetCaption(ops[1].ToInt()));
            }
            else if (StringHelper.IsSame(f, "getSumLower2Report"))
            {
                if (ops[3] == null)
                {
                    ReturnValue = new TVariant(GetSumLower2Report(ops[1].ToInt(), ops[2].ToInt(), true), "currency");
                }
                else
                {
                    ReturnValue = new TVariant(GetSumLower2Report(ops[1].ToInt(), ops[2].ToInt(), ops[3].ToBool()), "currency");
                }
            }
            else if (StringHelper.IsSame(f, "getSumLowerReport"))
            {
                if (ops[3] == null)
                {
                    ReturnValue = new TVariant(GetSumLowerReport(ops[1].ToInt(), ops[2].ToInt(), true), "currency");
                }
                else
                {
                    ReturnValue = new TVariant(GetSumLowerReport(ops[1].ToInt(), ops[2].ToInt(), ops[3].ToBool()), "currency");
                }
            }
            else if (StringHelper.IsSame(f, "getSumLowerReportCredit"))
            {
                ReturnValue = new TVariant(GetSumLowerReportCredit(ops[1].ToInt(), ops[2].ToInt()), "currency");
            }
            else
            {
                FunctionFound = false;

                foreach (System.Type userFunctionsClass in FUserFunctions)
                {
                    if (!FunctionFound)
                    {
                        rptUserFunctions = (TRptUserFunctions)Activator.CreateInstance(userFunctionsClass);

                        if (rptUserFunctions.FunctionSelector(this, f, ops, out ReturnValue))
                        {
                            FunctionFound = true;
                            break;
                        }

                        rptUserFunctions = null;
                    }
                }

                if (!FunctionFound)
                {
                    TRptCalculation calculation = ReportStore.GetCalculation(CurrentReport, f);

                    if (calculation != null)
                    {
                        TRptDataCalcCalculation calc = new TRptDataCalcCalculation(this);
                        ReturnValue = calc.EvaluateCalculation(calculation, null, String.Empty, -1);
                    }
                    else if (ops[1] == null)
                    {
                        // don't print an error if ops[1] is null;
                        // just return f;
                        // this is needed e.g. for HasChildRows etc, called from TRptEvaluator.evaluateOperator
                        ReturnValue = null;
                    }
                    else
                    {
                        ReturnValue = new TVariant();
                        logMessage = "unknown function " + f;

                        if (ops[1] != null)
                        {
                            logMessage = logMessage + ' ' + ops[1].ToString();
                        }

                        if (ops[2] != null)
                        {
                            logMessage = logMessage + ' ' + ops[2].ToString();
                        }

                        TLogging.Log(logMessage);
                    }
                }
            }

            TLogging.SetContext("");
            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AUserFunctionClass"></param>
        public static void AddUserFunctions(System.Type AUserFunctionClass)
        {
            FUserFunctions.Add(AUserFunctionClass);
        }
    }
}