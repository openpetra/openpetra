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
using System.Collections.Generic;
using Ict.Petra.Shared.MReporting;
using Ict.Common;

namespace Ict.Petra.Server.MReporting
{
    /// <summary>
    /// convert parameters for sql query
    /// </summary>
    public delegate TVariant TConvertProc(TVariant S);

    /// <summary>
    /// tools to format an SQL query so that it is useful for the SQL server
    /// </summary>
    public class TRptFormatQuery
    {
        private TParameterList parameters;
        private int column;
        private int depth;

        /// <summary>
        /// create a copy
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TVariant Id(TVariant value)
        {
            return new TVariant(value);
        }

        /// <summary>
        /// format a date in a form that will be adjusted
        /// for each database in FormatQueryRDBMSSpecific;
        /// (correct order of month/day etc)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TVariant FormatDate(TVariant value)
        {
            String list;
            String day;
            String month;
            String year;
            String resultString;

            if (value.TypeVariant == eVariantTypes.eDateTime)
            {
                resultString = "#" + value.DateToString("yyyy/MM/dd") + "#";

                // it seems, the separators (e.g. , /, .) are not considered
                resultString = resultString.Replace(value.DateToString("yyyy/MM/dd")[4], '-');
            }
            else
            {
                list = value.ToString();

                // try all available separators, defined in Ict.Common.StringHelper
                day = StringHelper.GetNextCSV(ref list, ".", true);

                if (list.Length == 0)
                {
                    throw new Exception("Ict.Petra.Server.MReporting.FormatQuery: Cannot decode date " + value.ToString());
                }

                month = StringHelper.GetNextCSV(ref list, ".", true);
                year = StringHelper.GetNextCSV(ref list, ".", true);

                resultString = String.Format("#{0:4}-{1:2}-{2:2}#",
                    year, month, day);
            }

            return new TVariant(resultString, true); // explicit string
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="column"></param>
        /// <param name="depth"></param>
        public TRptFormatQuery(TParameterList parameters, int column, int depth)
        {
            this.parameters = parameters;
            this.column = column;
            this.depth = depth;
        }

        // do not print warning too many times for the same variable
        private static SortedList <string, Int32>VariablesNotFound = new SortedList <string, int>();

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="searchOpen"></param>
        /// <param name="searchClose"></param>
        /// <param name="newOpen"></param>
        /// <param name="newClose"></param>
        /// <param name="convert"></param>
        /// <returns></returns>
        protected TVariant ReplaceVariablesPattern(TVariant orig,
            String searchOpen,
            String searchClose,
            String newOpen,
            String newClose,
            TConvertProc convert)
        {
            int position = 0;
            String resultString = orig.ToString();
            TVariant ReturnValue = null;
            int bracket = resultString.IndexOf(searchOpen, position);

            if (bracket == -1)
            {
                // no brackets, therefore use the original TVariant, so that the type information is not lost
                ReturnValue = orig;
            }

            while (bracket != -1)
            {
                int firstRealChar = bracket + searchOpen.Length;
                int paramEndIdx = resultString.IndexOf(searchClose, firstRealChar);

                if (paramEndIdx <= 0)
                {
                    // missing closing bracket; can happen with e.g. #testdate; should be #testdate#
                    if (resultString.Length > bracket + 20)
                    {
                        throw new Exception("Cannot find closing bracket " + searchClose + " for " + resultString.Substring(bracket, 20));
                    }
                    else
                    {
                        throw new Exception("Cannot find closing bracket " + searchClose + " for " + resultString.Substring(bracket));
                    }
                }

                String parameter = resultString.Substring(firstRealChar, paramEndIdx - firstRealChar);
                bool ParameterExists = false;
                TVariant newvalue;

                if (parameters != null)
                {
                    if (parameter.IndexOf("GLOBAL:") == 0)
                    {
                        newvalue = parameters.Get(parameter.Substring(7), -1, -1, eParameterFit.eExact);
                    }
                    else if (parameter.IndexOf("ALLLEVELS:") == 0)
                    {
                        newvalue = parameters.Get(parameter.Substring(10), -1, depth, eParameterFit.eBestFitEvenLowerLevel);
                    }
                    else
                    {
                        newvalue = parameters.Get(parameter, column, depth, eParameterFit.eBestFitEvenLowerLevel);
                    }

                    ParameterExists = (newvalue.TypeVariant != eVariantTypes.eEmpty);
                }
                else
                {
                    newvalue = new TVariant();
                }

                if (!ParameterExists)
                {
                    // if date is given, use the parameter itself
                    if ((parameter[0] >= '0') && (parameter[0] <= '9'))
                    {
                        newvalue = new TVariant(parameter);
                    }
                    else
                    {
                        int CountWarning = 1;

                        // do not print warning too many times for the same variable
                        if (!VariablesNotFound.ContainsKey(parameter))
                        {
                            VariablesNotFound.Add(parameter, 1);
                        }
                        else
                        {
                            VariablesNotFound[parameter] = VariablesNotFound[parameter] + 1;
                            CountWarning = VariablesNotFound[parameter];
                        }

                        if (CountWarning < 5)
                        {
                            // this can be alright, for empty values; for example method of giving can be empty; for report GiftTransactions
                            TLogging.Log(
                                "Variable " + parameter + " could not be found (column: " + column.ToString() +
                                "; level: " + depth.ToString() + "). " + resultString);
                        }
                        else if (CountWarning % 20 == 0)
                        {
                            TLogging.Log("20 times: Variable " + parameter + " empty or not found.");
                        }
                    }
                }

                try
                {
                    if (resultString.Length == (searchOpen + parameter + searchClose).Length)
                    {
                        // replace the whole value, return as a TVariant
                        ReturnValue = convert(newvalue);
                    }

                    resultString = resultString.Replace(searchOpen + parameter + searchClose, newOpen + convert(newvalue).ToString() + newClose);
                }
                catch (Exception e)
                {
                    throw new Exception(
                        "While trying to format parameter " + parameter + ", there was a problem with formatting." + Environment.NewLine + e.Message);
                }
                bracket = resultString.IndexOf(searchOpen, position);
            } // while

            if (ReturnValue == null)
            {
                // there has not been just a single value
                ReturnValue = new TVariant(resultString, true); // explicit string
            }

            return ReturnValue;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="s"></param>
        /// <param name="withQuotes"></param>
        /// <returns></returns>
        public TVariant ReplaceVariables(String s, Boolean withQuotes)
        {
            TVariant ReturnValue;

            // find all variables given in
            // name translates into "value"
            // name is changed to value
            // #name# is changed to "value" (date)
            // if the variable should be retrieved with getParameter("variablename", 1,1,true) => use GLOBAL:variablename
            // var
            // todo: Integer;  need to make it work for postgresql as well
            ReturnValue = new TVariant(s);
            ReturnValue = ReplaceVariablesPattern(ReturnValue, "{{", "}}", "", "", new TConvertProc(Id));

            ReturnValue = ReplaceVariablesPattern(ReturnValue, "{#", "#}", "", "", new TConvertProc(FormatDate));

            if (withQuotes)
            {
                ReturnValue = ReplaceVariablesPattern(ReturnValue, "{", "}", "'", "'", new TConvertProc(Id));
            }
            else
            {
                ReturnValue = ReplaceVariablesPattern(ReturnValue, "{", "}", "", "", new TConvertProc(Id));
            }

            return ReturnValue;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public TVariant ReplaceVariables(String s)
        {
            return ReplaceVariables(s, true);
        }
    }
}