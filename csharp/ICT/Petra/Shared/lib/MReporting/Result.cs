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
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Ict.Common;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml;
using Ict.Common.IO;

namespace Ict.Petra.Shared.MReporting
{
    /// <summary>
    /// compare that is able to sort the result of a report
    /// </summary>
    public class TRowComparer : System.Object, IComparer
    {
        /// <summary>list of integers; the column numbers by which the rows should be sorted</summary>
        public ArrayList FColumnList;
        /// <summary>Enables sorting of multiple levels. This will make the result into a flat table</summary>
        private Boolean FCompareMultipleLevels;

        /// <summary>
        /// Create a comparer, based on the comma separated list of column numbers
        ///
        /// </summary>
        /// <param name="AColumns">comma separated list of integers, describing the columns that the sorting should be based upon</param>
        /// <param name="ACompareMultipleLevels">Indicator if sorting of multiple levels is allowed.</param>
        /// <returns>void</returns>
        public TRowComparer(String AColumns, Boolean ACompareMultipleLevels)
        {
            StringCollection ColumnStringList;

            FColumnList = new ArrayList();
            ColumnStringList = StringHelper.StrSplit(AColumns, ",");

            foreach (String s in ColumnStringList)
            {
                FColumnList.Add((System.Object)Convert.ToInt32(s));
            }

            FCompareMultipleLevels = ACompareMultipleLevels;
        }

        /// <summary>
        /// Compare two rows of type TResult, using the set of column numbers
        /// implements the IComparer interface
        /// </summary>
        /// <param name="x">First object of TResult to compare</param>
        /// <param name="y">Second object of TResult to compare</param>
        /// <returns>same as IComparer.Compare: -1 means x is less than y, =0 means x=y, 1 means x>y
        /// </returns>
        public System.Int32 Compare(System.Object x, System.Object y)
        {
            System.Int32 ReturnValue;
            TResult Row1;
            TResult Row2;
            System.Int32 Counter;
            System.Int32 Column;
            System.Int32 cmp;
            Row1 = (TResult)x;
            Row2 = (TResult)y;

            // for the moment, it only works with plain reports
            if (!FCompareMultipleLevels && (Row1.masterRow != Row2.masterRow))
            {
                throw new Exception("TRowComparer: Sorting of multilevel is not allowed.");
            }

            ReturnValue = 0;

            // start with the least significant column, ie. from the back
            for (Counter = FColumnList.Count - 1; Counter >= 0; Counter -= 1)
            {
                Column = (System.Int32)FColumnList[Counter];

                if ((Row1.column[Column].IsNil())
                    && (Row2.column[Column].IsNil()))
                {
                    // if both entries are empty, they are equal
                    continue;
                }

                if (Row1.column[Column].IsNil())
                {
                    // if one is empty
                    cmp = 1;
                }
                else if (Row2.column[Column].IsNil())
                {
                    // if the other is empty
                    cmp = -1;
                }
                else
                {
                    cmp = (Row1.column[Column].CompareTo(Row2.column[Column]));
                }

                if (cmp != 0)
                {
                    ReturnValue = cmp;
                }
            }

            return ReturnValue;
        }
    }

    /// <summary>
    /// This represents one single line of the result.
    /// The lines are related to each other in a hierarchy,
    /// which is represented by masterRow and childRow.
    ///
    /// </summary>
    public class TResult
    {
        /// <summary>
        /// the parent row for this current row
        /// </summary>
        public int masterRow;

        /// <summary>
        /// the identification number of this row
        /// </summary>
        public int childRow;

        /// <summary>
        /// current depth
        /// </summary>
        public int depth;

        /// <summary>
        /// should this row be displayed
        /// </summary>
        public Boolean display;

        /// <summary>
        /// is this a debit or a credit
        /// </summary>
        public Boolean debit_credit_indicator;

        /// <summary>
        /// another identifier, but independent of hierarchy; used for debugging and other references
        /// </summary>
        public string code;

        /// <summary>
        /// condition for this row to be displayed
        /// </summary>
        public string condition;

        /// <summary>
        /// header for this row
        /// </summary>
        public TVariant[] header =
        {
            new TVariant(), new TVariant()
        };

        /// <summary>
        /// description on the left
        /// </summary>
        public TVariant[] descr =
        {
            new TVariant(), new TVariant()
        };

        /// <summary>
        /// values for each column
        /// </summary>
        public TVariant[] column;

        /// <summary>
        /// constructor
        ///
        /// </summary>
        /// <returns>void</returns>
        public TResult(int masterRow,
            int childRow,
            Boolean display,
            int depth,
            String code,
            string condition,
            Boolean debit_credit_indicator,
            TVariant[] header,
            TVariant[] descr,
            TVariant[] column)
        {
            int i;

            this.masterRow = masterRow;
            this.childRow = childRow;
            this.display = display;
            this.depth = depth;
            this.code = code;
            this.condition = condition;
            this.debit_credit_indicator = debit_credit_indicator;

            for (i = 0; i <= 1; i += 1)
            {
                this.header[i] = new TVariant(header[i]);
            }

            for (i = 0; i <= 1; i += 1)
            {
                this.descr[i] = new TVariant(descr[i]);
            }

            this.column = new TVariant[column.Length];

            for (i = 0; i < column.Length; i += 1)
            {
                this.column[i] = new TVariant(column[i]);
            }
        }

        /// <summary>
        /// copy constructor; creates a copy of the given object
        ///
        /// </summary>
        /// <returns>void</returns>
        public TResult(TResult copy)
        {
            Assign(copy);
        }

        /// <summary>
        /// copies the values of the given object into the self object
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Assign(TResult copy)
        {
            int i;

            this.masterRow = copy.masterRow;
            this.childRow = copy.childRow;
            this.display = copy.display;
            this.depth = copy.depth;
            this.code = copy.code;
            this.condition = copy.condition;
            this.debit_credit_indicator = copy.debit_credit_indicator;

            for (i = 0; i <= 1; i += 1)
            {
                this.header[i] = new TVariant(copy.header[i]);
            }

            for (i = 0; i <= 1; i += 1)
            {
                this.descr[i] = new TVariant(copy.descr[i]);
            }

            column = new TVariant[copy.column.Length];

            for (i = 0; i < copy.column.Length; i += 1)
            {
                this.column[i] = new TVariant(copy.column[i]);
            }
        }
    }

    /// <summary>
    /// This class contains a collection of result lines.
    /// The result can be exported as a CSV file.
    ///
    /// </summary>
    public class TResultList
    {
        /// <summary>the list of TResult objects</summary>
        private ArrayList results;

        /// <summary>the most right column that should be displayed (start counting at 1)</summary>
        private Int32 MaxDisplayColumns;

        /// <summary>
        /// add a row to the result
        /// </summary>
        /// <param name="masterRow"></param>
        /// <param name="childRow"></param>
        /// <param name="display"></param>
        /// <param name="depth"></param>
        /// <param name="code"></param>
        /// <param name="condition"></param>
        /// <param name="debit_credit_indicator"></param>
        /// <param name="header"></param>
        /// <param name="descr"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public TResult AddRow(int masterRow,
            int childRow,
            Boolean display,
            int depth,
            String code,
            string condition,
            Boolean debit_credit_indicator,
            TVariant[] header,
            TVariant[] descr,
            TVariant[] column)
        {
            foreach (TResult existingElement in results)
            {
                if (existingElement.code == code)
                {
                    TLogging.Log("Warning: TResult.AddRow: duplicate row codes! there is already a row with code " +
                        code);
                    // throw new Exception("TResult.AddRow: duplicate row codes! there is already a row with code " +
                    //    code);
                }
            }

            TResult element = new TResult(masterRow, childRow, display, depth, code, condition, debit_credit_indicator, header, descr, column);
            results.Add(element);
            return element;
        }

        /// <summary>
        /// Constructor
        /// creates the list for the TResult objects
        ///
        /// </summary>
        /// <returns>void</returns>
        public TResultList()
        {
            MaxDisplayColumns = 0;
            results = new ArrayList();
        }

        /// <summary>
        /// Copy Constructor
        /// creates a copy of another Resultlist;
        /// this is required to be able to print and export to CSV with the correctly formatted dates
        ///
        /// </summary>
        /// <returns>void</returns>
        public TResultList(TResultList copy)
        {
            MaxDisplayColumns = copy.MaxDisplayColumns;
            results = new ArrayList();

            foreach (TResult r in copy.results)
            {
                results.Add(new TResult(r));
            }
        }

        /// <summary>
        /// clear the result list
        /// </summary>
        public void Clear()
        {
            results.Clear();
        }

        /// <summary>
        /// Sort the result.
        /// only sorts the children of the same parent line.
        /// First column has higher precedence, so the sorting starts with the last column
        ///
        /// </summary>
        /// <param name="AColumns">comma separated list of integers, describing the columns that the sorting should be based upon
        /// </param>
        /// <returns>void</returns>
        public void Sort(String AColumns)
        {
            Sort(AColumns, false);
        }

        /// <summary>
        /// Sort the result.
        /// If AMakeFlatTable is false, then it only sorts the children of the same parent line.
        /// Otherwise it sorts all lines but the result will be that all children have the same parent line.
        /// First column has higher precedence, so the sorting starts with the last column
        /// </summary>
        /// <param name="AColumns">comma separated list of integers, describing the columns that the sorting should be based upon</param>
        /// <param name="AMakeFlatTable">Indicator if we should make a flat table. This allows sorting with multiple levels</param>
        /// <returns>void</returns>
        public void Sort(String AColumns, Boolean AMakeFlatTable)
        {
            TRowComparer RowComparer;

            RowComparer = new TRowComparer(AColumns, AMakeFlatTable);

            /* idea for sorting:
             * for each master
             * collect its children in an Arraylist
             * sort the list by all given columns
             * negate all master and child numbers in the results array;
             *  that is to avoid to not being able to differ
             *  between already renamed rows and still to be done rows
             * get a range of valid numbers from the minimum and maximum of the to be sorted children
             * for each child, give it a new valid number, according to the new position
             * change all rows reporting to that row accordingly
             * after that has been done, change all the other negated numbers back
             */
            if (results.Count < 1)
            {
                return;
            }

            this.results.Sort(0, results.Count, RowComparer);

            if (AMakeFlatTable)
            {
                foreach (TResult row in results)
                {
                    row.childRow = results.IndexOf(row) + 1;
                    // make a flat table
                    row.masterRow = 0;
                }
            }
            else
            {
                foreach (TResult row in results)
                {
                    row.childRow = results.IndexOf(row) + 1;
                }
            }
        }

        /// <summary>
        /// Update an existing row, identified by the masterRow and childRow numbers.
        /// This is needed for the second calculation, which involves other columns,
        /// whose values did not exist in the first run
        ///
        /// </summary>
        /// <returns>void</returns>
        public Boolean UpdateRow(int masterRow, int childRow, TVariant[] column)
        {
            foreach (TResult element in results)
            {
                if ((element.masterRow == masterRow) && (element.childRow == childRow))
                {
                    for (int i = 0; i <= column.Length - 1; i++)
                    {
                        element.column[i] = new TVariant(column[i]);
                    }

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// get the deepest visible level for the given parent row (how deep is the deepest child)
        /// </summary>
        /// <param name="AMasterRow"></param>
        /// <returns></returns>
        public int GetDeepestVisibleLevel(int AMasterRow)
        {
            int deepest = 0;

            // need to go recursive, because the display variable on a higher level can hide all the leafs, even if they have display value true
            // solves bug http:bugs.om.org/petra/show_bug.cgi?id=678
            if ((!HasChildRows(AMasterRow)))
            {
                TResult element = GetRow(AMasterRow);

                if (element != null)
                {
                    deepest = element.depth;
                }
            }
            else
            {
                // recursively search the branch that starts with the given element
                ArrayList children = new ArrayList();
                GetChildRows(AMasterRow, ref children);

                foreach (TResult element2 in children)
                {
                    if (element2.display)
                    {
                        int depth = GetDeepestVisibleLevel(element2.childRow);

                        if (depth > deepest)
                        {
                            deepest = depth;
                        }
                    }
                }
            }

            return deepest;
        }

        /// <summary>
        /// overloaded version; look for the depth of the deepest child in the whole report
        /// </summary>
        /// <returns></returns>
        public int GetDeepestVisibleLevel()
        {
            return GetDeepestVisibleLevel(0);
        }

        /// <summary>
        /// sort the results by their childrow code; using sort by insertion
        /// needed for excel export
        /// </summary>
        /// <returns>void</returns>
        public Boolean SortChildren()
        {
            int left = 0;
            int right = results.Count - 1;

            for (int i = left + 1; i <= right; i += 1)
            {
                int j = i;
                TResult current = (TResult)results[j];
                TResult tempRow = new TResult(current);
                TResult previous;

                if (j > left)
                {
                    previous = (TResult)results[j - 1];
                }
                else
                {
                    previous = null;
                }

                while ((j > left) && (previous.childRow > tempRow.childRow))
                {
                    current = (TResult)results[j];
                    current.Assign(previous);
                    j--;

                    if (j > left)
                    {
                        previous = (TResult)results[j - 1];
                    }
                    else
                    {
                        previous = null;
                    }
                }

                current = (TResult)results[j];
                current.Assign(tempRow);
            }

            return true;
        }

        /// <summary>
        /// sort rows by master
        /// </summary>
        /// <param name="sortedList"></param>
        /// <param name="masterRow"></param>
        public void CreateSortedListByMaster(ArrayList sortedList, int masterRow)
        {
            for (int counter = 0; counter < results.Count; counter++)
            {
                TResult element = (TResult)results[counter];

                if (element.masterRow == masterRow)
                {
                    CreateSortedListByMaster(sortedList, element.childRow);
                    sortedList.Add(element);
                }
            }
        }

        /// <summary>
        /// This stores the resultlist and parameterlist into a binary file (using the Datatable conversion);
        /// This can be used for debugging the printing, and saving time on calculating the report by reusing previous results
        ///
        /// </summary>
        /// <returns>void</returns>
        public void WriteBinaryFile(TParameterList AParameters, String AFilename)
        {
            DataTable dt = ToDataTable(AParameters);
            FileStream fs = new FileStream(AFilename, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(fs, dt);

            dt = AParameters.ToDataTable();
            bf.Serialize(fs, dt);

            fs.Close();
        }

        /// <summary>
        /// This loads the resultlist and parameterlist from a binary file (using the Datatable conversion);
        /// This can be used for debugging the printing, and saving time on calculating the report by reusing previous results
        ///
        /// </summary>
        /// <returns>void</returns>
        public void ReadBinaryFile(String AFilename, out TParameterList AParameters)
        {
            FileStream fs = new FileStream(AFilename, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            DataTable dt = (DataTable)bf.Deserialize(fs);

            results = new ArrayList();
            LoadFromDataTable(dt);

            dt = (DataTable)bf.Deserialize(fs);
            AParameters = new TParameterList();
            AParameters.LoadFromDataTable(dt);

            fs.Close();
        }

        /// <summary>
        /// This loads the resultlist from a datatable.
        /// Mainly used for sending the resultlist over a remote connection
        /// </summary>
        /// <param name="table">the datatable that contains a collection of results
        /// </param>
        /// <returns>void</returns>
        public void LoadFromDataTable(System.Data.DataTable table)
        {
            Int32 maxColumn;
            Int32 i;

            TVariant[] header =
            {
                new TVariant(), new TVariant()
            };
            TVariant[] descr =
            {
                new TVariant(), new TVariant()
            };
            TVariant[] column;

            results.Clear();

            foreach (DataRow row in table.Rows)
            {
                header[0] = TVariant.DecodeFromString(row["header1"].ToString());
                header[1] = TVariant.DecodeFromString(row["header2"].ToString());
                descr[0] = TVariant.DecodeFromString(row["descr1"].ToString());
                descr[1] = TVariant.DecodeFromString(row["descr2"].ToString());
                maxColumn = Convert.ToInt32(row["maxcolumn"]);
                column = new TVariant[maxColumn];

                for (i = 0; i <= maxColumn - 1; i += 1)
                {
                    column[i] = TVariant.DecodeFromString(row["column" + i.ToString()].ToString());
                }

                AddRow(Convert.ToInt32(row["masterRow"]), Convert.ToInt32(row["childRow"]), Convert.ToBoolean(row["display"]),
                    Convert.ToInt32(row["depth"]), row["code"].ToString(), row["condition"].ToString(), Convert.ToBoolean(
                        row["debit_credit_indicator"]), header, descr, column);
            }
        }

        /// <summary>
        /// This stores the resultlist into a datatable.
        /// Mainly used for sending the resultlist over a remote connection
        /// </summary>
        /// <returns>the datatable that contains a collection of results
        /// </returns>
        public System.Data.DataTable ToDataTable(TParameterList parameters)
        {
            int maxColumn = 0;

            for (int i = 0; i < MaxDisplayColumns; i++)
            {
                if ((!parameters.Get("ColumnWidth", i, -1, eParameterFit.eBestFit).IsNil()))
                {
                    maxColumn = i + 1;
                }
            }

            DataTable ReturnValue = new System.Data.DataTable();
            ReturnValue.Columns.Add(new System.Data.DataColumn("masterRow", typeof(System.Int32)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("childRow", typeof(System.Int32)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("display", typeof(bool)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("depth", typeof(System.Int32)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("code", typeof(String)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("condition", typeof(String)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("debit_credit_indicator", typeof(bool)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("header1", typeof(String)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("header2", typeof(String)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("descr1", typeof(String)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("descr2", typeof(String)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("maxcolumn", typeof(System.Int32)));

            for (int i = 0; i < maxColumn; i++)
            {
                ReturnValue.Columns.Add(new System.Data.DataColumn("column" + i.ToString(), typeof(String)));
            }

            foreach (TResult element in results)
            {
                DataRow row = ReturnValue.NewRow();
                row["maxcolumn"] = (System.Object)maxColumn;
                row["masterRow"] = (System.Object)element.masterRow;
                row["childRow"] = (System.Object)element.childRow;
                row["display"] = (System.Object)element.display;
                row["depth"] = (System.Object)element.depth;
                row["code"] = element.code;
                row["condition"] = element.condition;
                row["debit_credit_indicator"] = (System.Object)element.debit_credit_indicator;
                row["header1"] = element.header[0].EncodeToString();
                row["header2"] = element.header[1].EncodeToString();
                row["descr1"] = element.descr[0].EncodeToString();
                row["descr2"] = element.descr[1].EncodeToString();

                for (int i = 0; i < maxColumn; i++)
                {
                    row["column" + i.ToString()] = element.column[i].EncodeToString();
                }

                ReturnValue.Rows.InsertAt(row, ReturnValue.Rows.Count);
            }

            return ReturnValue;
        }

        /// <summary>
        /// This formats the dates for different output, for example printing
        /// </summary>
        /// <param name="AParameters">the current parameters, environmnent variables, for formatting</param>
        /// <param name="AOutputType">if this is 'Localized' then the dates are formatted in the format DD-MMM-YYYY</param>
        /// <returns>s a new copy of the result, with the correct formatting
        /// </returns>
        public TResultList ConvertToFormattedStrings(TParameterList AParameters, String AOutputType)
        {
            TResultList ReturnValue = new TResultList(this);
            Int32 i;

            foreach (TResult r in ReturnValue.results)
            {
                for (i = 0; i <= 1; i++)
                {
                    r.header[i] = new TVariant(r.header[i].ToFormattedString("", AOutputType));
                }

                for (i = 0; i <= 1; i++)
                {
                    r.descr[i] = new TVariant(r.descr[i].ToFormattedString("", AOutputType));
                }

                for (i = 0; i < r.column.Length; i++)
                {
                    if (r.column[i].TypeVariant == eVariantTypes.eString)
                    {
                        r.column[i] = new TVariant(r.column[i].ToString(), true);
                    }
                    else
                    {
                        // format thousands only or without decimals
                        if (StringHelper.IsCurrencyFormatString(r.column[i].FormatString) && AParameters.Exists("param_currency_format"))
                        {
                            r.column[i] = new TVariant(r.column[i].ToFormattedString(AParameters.Get(
                                        "param_currency_format").ToString(), AOutputType), true);
                        }
                        else
                        {
                            r.column[i] = new TVariant(r.column[i].ToFormattedString("", AOutputType), true);
                        }
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// overload, use Localized formatting
        /// </summary>
        /// <param name="AParameters"></param>
        /// <returns></returns>
        public TResultList ConvertToFormattedStrings(TParameterList AParameters)
        {
            return ConvertToFormattedStrings(AParameters, "Localized");
        }

        /// <summary>
        /// This stores the resultlist into a CSV file.
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="csvfilename"></param>
        /// <param name="separator">if this has the value FIND_BEST_SEPARATOR,
        /// then first the parameters will be checked for CSV_separator, and if that parameter does not exist,
        /// then the CurrentCulture is checked, for the local language settings</param>
        /// <param name="ADebugging">if true, thent the currency and date values are written encoded, not localized</param>
        /// <param name="AExportOnlyLowestLevel">if true, only the lowest level of AParameters are exported (level with higest depth)
        /// otherwise all levels in AParameter are exported</param>
        /// <returns>true for success</returns>
        public bool WriteCSV(TParameterList AParameters,
            string csvfilename,
            string separator = "FIND_BEST_SEPARATOR",
            Boolean ADebugging = false,
            Boolean AExportOnlyLowestLevel = false)
        {
            StreamWriter csvStream;

            try
            {
                // don't append; use the local encoding, e.g. to support Umlauts
                csvStream = new StreamWriter(csvfilename, false, System.Text.Encoding.Default);
            }
            catch (System.Exception)
            {
                return false;
            }

            List <string>lines = WriteCSVInternal(AParameters, separator, ADebugging, AExportOnlyLowestLevel);

            foreach (string line in lines)
            {
                csvStream.WriteLine(line);
            }

            csvStream.Close();

            return true;
        }

        /// <summary>
        /// This returns the resultlist as lines for a CSV file
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="separator">if this has the value FIND_BEST_SEPARATOR,
        /// then first the parameters will be checked for CSV_separator, and if that parameter does not exist,
        /// then the CurrentCulture is checked, for the local language settings</param>
        /// <param name="ADebugging">if true, thent the currency and date values are written encoded, not localized</param>
        /// <param name="AExportOnlyLowestLevel">if true, only the lowest level of AParameters are exported (level with higest depth)
        /// otherwise all levels in AParameter are exported</param>
        /// <returns>the lines to be written to the CSV file</returns>
        public List <string>WriteCSVInternal(TParameterList AParameters,
            string separator = "FIND_BEST_SEPARATOR",
            Boolean ADebugging = false,
            Boolean AExportOnlyLowestLevel = false)
        {
            List <string>lines = new List <string>();
            int i;
            string strLine;
            ArrayList sortedList;
            bool display;
            bool useIndented;
            TParameterList FormattedParameters;
            TResultList FormattedResult;

            // myEncoding: Encoding;
            // bytes: array of byte;
            if (separator == "FIND_BEST_SEPARATOR")
            {
                if (AParameters.Exists("CSV_separator"))
                {
                    separator = AParameters.Get("CSV_separator").ToString();

                    if (separator.ToUpper() == "TAB")
                    {
                        separator = new String((char)9, 1);
                    }
                    else if (separator.ToUpper() == "SPACE")
                    {
                        separator = " ";
                    }
                }
                else
                {
                    separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
                }
            }

            if (ADebugging == false)
            {
                FormattedParameters = AParameters.ConvertToFormattedStrings("CSV");
                FormattedResult = ConvertToFormattedStrings(FormattedParameters, "CSV");
            }
            else
            {
                FormattedParameters = AParameters;
                FormattedResult = this;
            }

            // write headings
            strLine = "";

            // for debugging:
            // strLine = StringHelper.AddCSV(strLine, "masterRow", separator);
            // strLine = StringHelper.AddCSV(strLine, "childRow", separator);
            // strLine = StringHelper.AddCSV(strLine, "depth", separator);

            strLine = StringHelper.AddCSV(strLine, "id", separator);

            if (FormattedParameters.Exists("ControlSource", ReportingConsts.HEADERPAGELEFT1,
                    -1, eParameterFit.eBestFit))
            {
                strLine = StringHelper.AddCSV(strLine, FormattedParameters.Get("ControlSource",
                        ReportingConsts.HEADERPAGELEFT1,
                        -1, eParameterFit.eBestFit).ToString(), separator);
            }

            if (FormattedParameters.Exists("ControlSource", ReportingConsts.HEADERPAGELEFT2,
                    -1, eParameterFit.eBestFit))
            {
                strLine = StringHelper.AddCSV(strLine, FormattedParameters.Get("ControlSource",
                        ReportingConsts.HEADERPAGELEFT2,
                        -1, eParameterFit.eBestFit).ToString(), separator);
            }

            if (FormattedParameters.Exists("ControlSource", ReportingConsts.HEADERCOLUMN,
                    -1, eParameterFit.eBestFit))
            {
                strLine = StringHelper.AddCSV(strLine, "header 1", separator);
                strLine = StringHelper.AddCSV(strLine, "header 0", separator);
            }

            useIndented = false;

            for (i = 0; i <= FormattedParameters.Get("lowestLevel").ToInt(); i++)
            {
                if (FormattedParameters.Exists("indented", ReportingConsts.ALLCOLUMNS, i, eParameterFit.eBestFit))
                {
                    useIndented = true;
                }
            }

            MaxDisplayColumns = AParameters.Get("MaxDisplayColumns").ToInt32();

            for (i = 0; i < MaxDisplayColumns; i++)
            {
                if ((!FormattedParameters.Get("ColumnCaption", i, -1, eParameterFit.eBestFit).IsNil()))
                {
                    strLine =
                        StringHelper.AddCSV(strLine,
                            (FormattedParameters.Get("ColumnCaption",
                                 i, -1, eParameterFit.eBestFit).ToString() + ' ' +
                             FormattedParameters.Get("ColumnCaption2",
                                 i, -1,
                                 eParameterFit.eBestFit).ToString(false) + ' ' +
                             FormattedParameters.Get("ColumnCaption3", i, -1, eParameterFit.eBestFit).ToString(
                                 false)).Trim(), separator);

                    if (useIndented)
                    {
                        strLine = StringHelper.AddCSV(strLine, "", separator);
                    }
                }
            }

            lines.Add(strLine);
            FormattedResult.SortChildren();
            sortedList = new ArrayList();
            FormattedResult.CreateSortedListByMaster(sortedList, 0);

            int LowestLevel = -1;

            if (AExportOnlyLowestLevel)
            {
                // find the highest level
                foreach (TResult element in sortedList)
                {
                    if (element.depth > LowestLevel)
                    {
                        LowestLevel = element.depth;
                    }
                }
            }

            // write each row to CSV file
            foreach (TResult element in sortedList)
            {
                if (AExportOnlyLowestLevel
                    && (element.depth < LowestLevel))
                {
                    continue;
                }

                if (element.display)
                {
                    strLine = "";

                    // for debugging
                    // strLine = StringHelper.AddCSV(strLine, element.masterRow.ToString(), separator);
                    // strLine = StringHelper.AddCSV(strLine, element.childRow.ToString(), separator);
                    // strLine = StringHelper.AddCSV(strLine, element.depth.ToString(), separator);

                    strLine = StringHelper.AddCSV(strLine, element.code, separator);

                    if (FormattedParameters.Exists("ControlSource", ReportingConsts.HEADERPAGELEFT1, -1, eParameterFit.eBestFit))
                    {
                        if (ADebugging)
                        {
                            strLine = StringHelper.AddCSV(strLine, element.descr[0].EncodeToString(), separator);
                        }
                        else
                        {
                            strLine = StringHelper.AddCSV(strLine, element.descr[0].ToString(), separator);
                        }
                    }

                    if (FormattedParameters.Exists("ControlSource", ReportingConsts.HEADERPAGELEFT2, -1, eParameterFit.eBestFit))
                    {
                        if (ADebugging)
                        {
                            strLine = StringHelper.AddCSV(strLine, element.descr[1].EncodeToString(), separator);
                        }
                        else
                        {
                            strLine = StringHelper.AddCSV(strLine, element.descr[1].ToString(), separator);
                        }
                    }

                    if (FormattedParameters.Exists("ControlSource", ReportingConsts.HEADERCOLUMN, -1, eParameterFit.eBestFit))
                    {
                        if (ADebugging)
                        {
                            strLine = StringHelper.AddCSV(strLine, element.header[1].EncodeToString(), separator);
                            strLine = StringHelper.AddCSV(strLine, element.header[0].EncodeToString(), separator);
                        }
                        else
                        {
                            strLine = StringHelper.AddCSV(strLine, element.header[1].ToString(), separator);
                            strLine = StringHelper.AddCSV(strLine, element.header[0].ToString(), separator);
                        }
                    }

                    /* TODO: try to export in the right codepage, to print umlaut and other special characters correctly
                     * if element.childRow = 7 then
                     * begin
                     * myEncoding := System.Text.Encoding.get_ASCII;
                     * TLogging.Log(Encoding.Default.EncodingName);
                     * TLogging.Log(element.column[0].ToString());
                     * SetLength(bytes, Encoding.Default.GetByteCount(element.column[0].ToString()));
                     * bytes := Encoding.Default.GetBytes(element.column[0].ToString());
                     * TLogging.Log(myEncoding.GetChars(bytes));
                     * // this will still not help with Excel
                     * end;
                     */
                    display = false;

                    for (i = 0; i <= MaxDisplayColumns - 1; i += 1)
                    {
                        if (FormattedParameters.Get("indented", i, element.depth, eParameterFit.eAllColumnFit).ToBool() == true)
                        {
                            strLine = StringHelper.AddCSV(strLine, "", separator);
                        }

                        if (((element.column[i] != null) && (!element.column[i].IsNil())) || (ADebugging))
                        {
                            display = true;

                            if (ADebugging)
                            {
                                strLine = StringHelper.AddCSV(strLine, element.column[i].EncodeToString(), separator);
                            }
                            else
                            {
                                strLine = StringHelper.AddCSV(strLine, element.column[i].ToString().Trim(), separator);
                            }
                        }
                        else
                        {
                            strLine = StringHelper.AddCSV(strLine, "", separator);
                        }

                        if ((FormattedParameters.Get("indented", i, element.depth, eParameterFit.eAllColumnFit).ToBool() != true) && useIndented)
                        {
                            strLine = StringHelper.AddCSV(strLine, "", separator);
                        }
                    }

                    if (display)
                    {
                        lines.Add(strLine);
                    }
                }
            }

            sortedList = null;
            return lines;
        }

        /// <summary>
        /// overload; no specific separator, find the best for the current localisation
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="csvfilename"></param>
        /// <param name="AExportOnlyLowestLevel"></param>
        /// <returns></returns>
        public bool WriteCSV(TParameterList AParameters, string csvfilename, Boolean AExportOnlyLowestLevel)
        {
            return WriteCSV(AParameters, csvfilename, "FIND_BEST_SEPARATOR", false, AExportOnlyLowestLevel);
        }

        /// <summary>
        /// This stores the resultlist into a XmlDocument (to be saved as Excel file)
        /// </summary>
        /// <param name="AParameters"></param>
        /// <param name="AExportOnlyLowestLevel">if true, only the lowest level of AParameters are exported (level with higest depth)
        /// otherwise all levels in AParameter are exported</param>
        /// <returns>the XmlDocument</returns>
        public XmlDocument WriteXmlDocument(TParameterList AParameters, Boolean AExportOnlyLowestLevel = false)
        {
            List <string>lines = WriteCSVInternal(AParameters, ";", false, AExportOnlyLowestLevel);

            return TCsv2Xml.ParseCSV2Xml(lines, ";");
        }

        /// <summary>
        /// needed for TRptSituation.processAllRows
        ///
        /// </summary>
        /// <returns>void</returns>
        public ArrayList GetResults()
        {
            return results;
        }

        /// <summary>
        /// This function checks if the given row has
        /// at least one column not equal 0 or NULL
        ///
        /// </summary>
        /// <returns>void</returns>
        public Boolean HasColumns(int row)
        {
            TResult element = GetRow(row);

            if (element == null)
            {
                return false;
            }

            for (int column = 0; column < MaxDisplayColumns; column++)
            {
                if ((element.column[column] != null) && (!element.column[column].IsZeroOrNull()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This function checks if the master has children
        ///
        /// </summary>
        /// <returns>void</returns>
        public Boolean HasChildRows(int master)
        {
            int counter = 0;

            while (counter < results.Count)
            {
                TResult element = (TResult)results[counter];
                counter++;

                if (element.display && (element.masterRow == master))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This function returns the row
        ///
        /// </summary>
        /// <returns>void</returns>
        public TResult GetRow(int lineId)
        {
            foreach (TResult element in results)
            {
                if (element.display && (element.childRow == lineId))
                {
                    return element;
                }
            }

            return null;
        }

        /// <summary>
        /// This function returns the row
        ///
        /// </summary>
        /// <returns>void</returns>
        public TResult GetRow(String codeLine)
        {
            foreach (TResult element in results)
            {
                if (element.display && (element.code == codeLine))
                {
                    return element;
                }
            }

            return null;
        }

        /// <summary>
        /// This function returns the first child row of the master
        ///
        /// </summary>
        /// <returns>void</returns>
        public TResult GetFirstChildRow(int master)
        {
            foreach (TResult element in results)
            {
                if (element.masterRow == master)
                {
                    return element;
                }
            }

            return null;
        }

        /// <summary>
        /// This function returns the child rows of the master
        ///
        /// </summary>
        /// <returns>void</returns>
        public Boolean GetChildRows(int master, ref ArrayList list)
        {
            foreach (TResult element in results)
            {
                if (element.masterRow == master)
                {
                    list.Add(element);
                }
            }

            return list.Count > 0;
        }

        /// <summary>
        /// This function returns the grand children rows of the master
        ///
        /// </summary>
        /// <returns>void</returns>
        public Boolean GetGrandChildRows(int master, ref ArrayList list)
        {
            ArrayList grandChildren;

            foreach (TResult element in results)
            {
                if (element.masterRow == master)
                {
                    grandChildren = new ArrayList();
                    GetChildRows(element.childRow, ref grandChildren);

                    foreach (TResult grandChild in grandChildren)
                    {
                        list.Add(grandChild);
                    }

                    grandChildren.Clear();
                }
            }

            return list.Count > 0;
        }

        /// <summary>
        /// This function checks if the children of the master have
        /// at least one column not equal 0 or NULL
        ///
        /// </summary>
        /// <returns>void</returns>
        public Boolean HasChildColumns(int master)
        {
            int counter = 0;

            while (counter < results.Count)
            {
                TResult element = (TResult)results[counter];
                counter++;

                if (element.masterRow == master)
                {
                    for (int i = 0; i < MaxDisplayColumns; i++)
                    {
                        if ((element.column[i] != null) && (!element.column[i].IsZeroOrNull()))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// the maximum number of displayed columns
        /// </summary>
        /// <returns></returns>
        public int GetMaxDisplayColumns()
        {
            return MaxDisplayColumns;
        }

        /// <summary>
        /// set the maximum number of displayed columns
        /// </summary>
        /// <param name="col"></param>
        public void SetMaxDisplayColumns(int col)
        {
            MaxDisplayColumns = col;
        }
    }
}