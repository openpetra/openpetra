//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2018 by OM International
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
using System.Collections.Generic;
using Ict.Common;
using Ict.Common.DB;
using System.IO;

namespace Ict.Petra.Server.MReporting
{
    /// parse and replace HTML templates for reports
    public class HTMLTemplateProcessor
    {
        private string FHTMLTemplate;
        private Dictionary<string, string> FSQLQueries = new Dictionary<string, string>();
        // do not print warning too many times for the same variable
        private static SortedList<string, Int32> VariablesNotFound = new SortedList<string, int>();

        /// <summary>
        /// constructor
        /// </summary>
        public HTMLTemplateProcessor(string AHTMLTemplate, TParameterList AParameters)
        {
            FHTMLTemplate = AHTMLTemplate;

            SeparateSQLQueries();

            //FHTMLTemplate = InsertParameters(AHTMLTemplate, AParameters);
        }

        /// separate the sql queries from the HTML template
        private void SeparateSQLQueries()
        {
            int pos = FHTMLTemplate.IndexOf("<!-- BeginSQL ");
            while (pos != -1)
            {
                int posAfterName = FHTMLTemplate.IndexOf("-->", pos);
                string name = FHTMLTemplate.Substring(pos + "<!-- BeginSQL ".Length, posAfterName - (pos + "<!-- BeginSQL ".Length)).Trim();
                int posAfterSQL = FHTMLTemplate.IndexOf("<!-- EndSQL", pos);
                string sql = FHTMLTemplate.Substring(posAfterName + "-->".Length, posAfterSQL - (posAfterName + "-->".Length)).Trim();
                TLogging.Log("name: " + name);
                TLogging.Log("sql: " + sql);
                FSQLQueries.Add(name, sql);

                // remove sql from template
                FHTMLTemplate = FHTMLTemplate.Substring(0, pos) +
                    FHTMLTemplate.Substring(FHTMLTemplate.IndexOf("-->", posAfterSQL)+"-->".Length);
                pos = FHTMLTemplate.IndexOf("<!-- BeginSQL");
            }

            TLogging.Log(FHTMLTemplate);
        }

        /// <summary>
        ///  the processed HTML
        /// </summary>
        public string GetHTML()
        {
            return FHTMLTemplate;
        }

        /*
        private string InsertParameters(string searchOpen, string searchClose, string template, TParameterList parameters)
        {
            int bracket = template.IndexOf(searchOpen);

            while (bracket != -1)
            {
                int firstRealChar = bracket + searchOpen.Length;
                int paramEndIdx = template.IndexOf(searchClose, firstRealChar);

                if (paramEndIdx <= 0)
                {
                    // missing closing bracket; can happen with e.g. #testdate; should be #testdate#
                    if (template.Length > bracket + 20)
                    {
                        throw new Exception("Cannot find closing bracket " + searchClose + " for " + template.Substring(bracket, 20));
                    }
                    else
                    {
                        throw new Exception("Cannot find closing bracket " + searchClose + " for " + template.Substring(bracket));
                    }
                }

                String parameter = template.Substring(firstRealChar, paramEndIdx - firstRealChar);
                bool ParameterExists = false;
                TVariant newvalue;

                if (parameters != null)
                {
                    newvalue = parameters.Get(parameter, -1, -1, eParameterFit.eBestFitEvenLowerLevel);

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

                bracket = template.IndexOf(searchOpen);
            } // while

        }

        private string InsertParameters(string AHTMLTemplate, TParameterList AParameters)
        {
            AHtmlTemplate = InsertParameters(" IN {{", "}}", AHtmlTemplate, AParameters);
            AHtmlTemplate = InsertParameters("{{", "}}", AHtmlTemplate, AParameters);
            AHtmlTemplate = InsertParameters("{", "}", AHtmlTemplate, AParameters);

            return AHTMLTemplate;
        }
        */
    }
}
