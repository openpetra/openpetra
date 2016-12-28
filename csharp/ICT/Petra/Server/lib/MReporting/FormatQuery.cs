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
using System.Data.Odbc;
using System.Collections.Generic;
using Ict.Petra.Shared.MReporting;
using Ict.Common;

namespace Ict.Petra.Server.MReporting
{
    /// <summary>
    /// tools to format an SQL query so that it is useful for the SQL server
    /// </summary>
    public class TRptFormatQuery
    {
        private TParameterList parameters;
        private int column;
        private int depth;
        private List<OdbcParameter> FOdbcParameters;
        private string FSQLStmt;
        private TVariant FVariantValue = new TVariant();

        /// <summary>
        /// constructor
        /// </summary>
        public TRptFormatQuery(string ASQLStmt = "", List<OdbcParameter> AOdbcParameters = null, TParameterList parameters = null, int column = -1, int depth = -1)
        {
            this.FSQLStmt = ASQLStmt;

            if (AOdbcParameters == null)
            {
                this.FOdbcParameters = new List<OdbcParameter>();
            }
            else
            {
                this.FOdbcParameters = AOdbcParameters;
            }

            this.parameters = parameters;
            this.column = column;
            this.depth = depth;
        }

        /// read the sql statement
        public string SQLStmt
        {
            get
            {
                return this.FSQLStmt.Replace("PARAMETER?", "?");
            }
        }

        /// <summary>
        /// returns true if this is not an sql statement but a single value that can be accessed through the VariantValue property
        /// </summary>
        /// <returns></returns>
        public bool IsVariant
        {
            get
            {
                return this.FSQLStmt == "TVariant";
            }
        }
        
        /// <summary>
        /// returns true if the value is zero or null
        /// </summary>
        public bool IsZeroOrNull()
        {
            if (IsVariant)
            {
                return FVariantValue.IsZeroOrNull();
            }
            TVariant v = new TVariant(this.FSQLStmt);
            return v.IsZeroOrNull();
        }

        /// <summary>
        /// returns the value if it is not an SQL statement.
        /// </summary>
        public TVariant VariantValue
        {
            get
            {
                if (!IsVariant)
                {
                    if (this.FOdbcParameters.Count == 0)
                    {
                        // we assume this is a single value, not an SQL statement
                        return new TVariant(this.FSQLStmt);
                    }

                    TLogging.Log("expected a single variant value: " + this.FSQLStmt);
                    throw new Exception("expected a single variant value, not an SQL string");
                }

                return this.FVariantValue;
            }
        }

        /// read the odbc parameters
        public List<OdbcParameter> OdbcParameters
        {
            get
            {
                return this.FOdbcParameters;
            }
        }

        /// <summary>
        /// add more text to the sql statement
        /// </summary>
        public void Add(string sql)
        {
            this.FSQLStmt += sql;
        }

        /// <summary>
        /// combine two sql statements with parameters
        /// </summary>
        public void Add(TRptFormatQuery AQueryToAdd)
        {
            this.FSQLStmt += AQueryToAdd.SQLStmt;

            this.parameters.Add(AQueryToAdd.parameters);
        }

        // do not print warning too many times for the same variable
        private static SortedList <string, Int32>VariablesNotFound = new SortedList <string, int>();

        /// <summary>
        /// Replace parameters with ODBC Parameters
        /// </summary>
        /// <param name="searchOpen"></param>
        /// <param name="searchClose"></param>
        protected void ReplaceVariablesPattern(
            String searchOpen,
            String searchClose)
        {
            int bracket = this.FSQLStmt.IndexOf(searchOpen);

            while (bracket != -1)
            {
                int firstRealChar = bracket + searchOpen.Length;
                int paramEndIdx = this.FSQLStmt.IndexOf(searchClose, firstRealChar);

                if (paramEndIdx <= 0)
                {
                    // missing closing bracket; can happen with e.g. #testdate; should be #testdate#
                    if (this.FSQLStmt.Length > bracket + 20)
                    {
                        throw new Exception("Cannot find closing bracket " + searchClose + " for " + this.FSQLStmt.Substring(bracket, 20));
                    }
                    else
                    {
                        throw new Exception("Cannot find closing bracket " + searchClose + " for " + this.FSQLStmt.Substring(bracket));
                    }
                }

                String parameter = this.FSQLStmt.Substring(firstRealChar, paramEndIdx - firstRealChar);
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
                                "Variable " + parameter + " empty or not found (column: " + column.ToString() +
                                "; level: " + depth.ToString() + "). " + this.FSQLStmt);
                        }
                        else if (CountWarning % 20 == 0)
                        {
                            TLogging.Log("20 times: Variable " + parameter + " empty or not found.");
                        }
                    }
                }

                try
                {
                    if (this.FSQLStmt.Length == (searchOpen + parameter + searchClose).Length)
                    {
                        // replace the whole value, return as a TVariant
                        this.FVariantValue = newvalue;
                        this.FSQLStmt = "TVariant";
                        return;
                    }

                    if (newvalue.TypeVariant == eVariantTypes.eDateTime)
                    {
                        // remove the time from the timestamp, only use the date at 0:00
                        DateTime date = newvalue.ToDate();
                        newvalue = new TVariant(new DateTime(date.Year, date.Month, date.Day));
                    }

                    this.AddOdbcParameters(searchOpen, parameter, searchClose, newvalue);
                }
                catch (Exception e)
                {
                    throw new Exception(
                        "While trying to format parameter " + parameter + ", there was a problem with formatting." + Environment.NewLine + e.Message);
                }
                bracket = this.FSQLStmt.IndexOf(searchOpen);
            } // while
        }

        /// <summary>
        /// add an odbc parameter, and replace the placeholders
        /// </summary>
        public void AddOdbcParameters(string APrefix, string AName, string APostfix, TVariant AValue)
        {
            int pos = 0;
            int parampos = 0;
            int parameterIndex = 0;
            while ((pos = this.FSQLStmt.IndexOf(APrefix + AName + APostfix, pos)) != -1)
            {
                while ((parampos != -1) && (parampos <= pos))
                {
                    parampos = this.FSQLStmt.IndexOf("PARAMETER?", parampos);
                    if ((parampos != -1) && (parampos <= pos))
                    {
                        parameterIndex++;
                    }
                }
                pos++;
                parampos = pos;
                this.FOdbcParameters.Insert(parameterIndex, AValue.ToOdbcParameter(AName));
            }

            this.FSQLStmt = this.FSQLStmt.Replace(APrefix + AName + APostfix, "PARAMETER?");
        }

        /// <summary>
        /// replace all place holders with values stored in the parameters list
        /// </summary>
        public void ReplaceVariables()
        {
            ReplaceVariablesPattern("{{", "}}");
            ReplaceVariablesPattern("{#", "#}");
            ReplaceVariablesPattern("{", "}");
        }
    }
}