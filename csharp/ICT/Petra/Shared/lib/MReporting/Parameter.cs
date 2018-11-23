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
using System.IO;
using Ict.Common.IO;
using Ict.Common;
using Ict.Common.Remoting.Server;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Globalization;
using Ict.Petra.Shared.MReporting;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Ict.Petra.Shared.MReporting
{
    /**
     * This is used to specify, how strictly the column and level value should be fit
     * by the result of a search for a parameter.
     */
    public enum eParameterFit
    {
        /// <summary>
        /// will fit both column and value exactly.
        /// </summary>
        eExact,

        /// <summary>
        /// will fit the closest column and level, only looking to the current and upper levels
        /// </summary>
        eBestFit,

        /// <summary>
        /// will also include values in the levels below the current level
        /// </summary>
        eAllColumnFit,

        /// <summary>
        /// will only consider values whose column has the value ALLCOLUMNS (99).
        /// </summary>
        eBestFitEvenLowerLevel
    };

    /// <summary>
    /// This class is able to hold one value,
    /// and knows to which column and level this variable applies.
    /// </summary>
    public class TParameter: IComparable
    {
        /// if this is set in column, the parameter applies to all columns
        public const int ALLCOLUMNS = 99;
        
        /// <summary>
        /// name of the parameter
        /// </summary>
        public String name;

        /// <summary>can be between 1 and 12 and more</summary>
        public int column;

        /// <summary>level 1 is the main level, the bigger the number, the lower the level.</summary>
        public int level;

        /// <summary>the value of this parameter</summary>
        public TVariant value;

        /// <summary>
        /// constructor
        /// </summary>
        public TParameter(String pname,
            TVariant pvalue,
            int pcolumn,
            int plevel)
        {
            name = pname;
            value = pvalue;
            column = pcolumn;
            level = plevel;
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="copy"></param>
        public TParameter(TParameter copy)
        {
            name = copy.name;
            value = copy.value;
            column = copy.column;
            level = copy.level;
        }

        /// <summary>
        /// compare two objects for sorting
        /// </summary>
        int IComparable.CompareTo(Object B)
        {
            TParameter a = this;
            TParameter b = (TParameter) B;

            if (a.name == b.name)
            {
                if (a.level == b.level)
                {
                    if (a.column == b.column)
                    {
                        return 0;
                    }

                    return (a.column > b.column ? 1 : -1);
                }

                return (a.level > b.level ? 1 : -1);
            }

            return a.name.CompareTo(b.name);
        }
    }

    /// <summary>
    /// This class is a container class that holds an unlimited number of TParameter objects.
    /// It provides functions to add parameters of all possible types.
    /// It provides functions for retrieving the value of a given parameter,
    /// depending on column and level.
    /// The data can be stored to an xml file and loaded from an xml file.
    ///
    /// </summary>
    public class TParameterList
    {
        /// <summary>the collection TParameter objects</summary>
        private ArrayList Fparameters;

        /// <summary>
        /// Constructor
        /// initialises the member variable parameters
        ///
        /// </summary>
        /// <returns>void</returns>
        public TParameterList()
        {
            Fparameters = new ArrayList();
        }

        /// <summary>
        /// Copy Constructor
        /// creates a copy of another ParameterList;
        /// this is required to be able to print and export to CSV with the correctly formatted dates
        ///
        /// </summary>
        /// <returns>void</returns>
        public TParameterList(TParameterList copy)
        {
            Fparameters = new ArrayList();

            foreach (TParameter p in copy.Fparameters)
            {
                Fparameters.Add(new TParameter(p));
            }
        }

        /// <summary>
        /// Clear all parameters from the current list
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Clear()
        {
            Fparameters.Clear();
        }

        /// <summary>
        /// Get at the actual list.
        /// </summary>
        public ArrayList Elems
        {
            get
            {
                return Fparameters;
            }
        }

        /// <summary>
        /// This loads the parameters from a datatable
        /// Mainly used for sending the parameters over a remote connection
        /// </summary>
        /// <param name="param">the datatable that contains a collection of parameters
        /// </param>
        /// <returns>void</returns>
        public void LoadFromDataTable(System.Data.DataTable param)
        {
            Fparameters.Clear();

            foreach (System.Data.DataRow row in param.Rows)
            {
                Fparameters.Add(new TParameter(row["name"].ToString(), TVariant.DecodeFromString(row["value"].ToString()),
                        Convert.ToInt32(row["column"]), Convert.ToInt32(row["level"])));
            }

            if ((TLogging.DebugLevel >= TLogging.DEBUGLEVEL_REPORTING) && (TSrvSetting.ServerLogFile.Length > 0))
            {
                Save(Path.GetDirectoryName(TSrvSetting.ServerLogFile) + Path.DirectorySeparatorChar + "param.json");
            }
        }

        /// <summary>
        /// This stores the parameters into a datatable
        /// Mainly used for sending the parameters over a remote connection
        /// </summary>
        /// <returns>the datatable that contains a collection of parameters
        /// </returns>
        public System.Data.DataTable ToDataTable()
        {
            System.Data.DataTable ReturnValue;
            System.Data.DataRow row;
            ReturnValue = new System.Data.DataTable();
            ReturnValue.Columns.Add(new System.Data.DataColumn("name", typeof(String)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("column", typeof(System.Int32)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("level", typeof(System.Int32)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("value", typeof(String)));

            foreach (TParameter element in Fparameters)
            {
                row = ReturnValue.NewRow();
                row["name"] = element.name;
                row["column"] = (System.Object)element.column;
                row["level"] = (System.Object)element.level;
                row["value"] = element.value.EncodeToString();
                ReturnValue.Rows.InsertAt(row, ReturnValue.Rows.Count);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Procedure to copy all parameters of one column from another parameter list;
        /// The column is first emptied in this parameter list, before the copying takes place.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Copy(TParameterList AOtherList, int column, int depth, eParameterFit exact, int ADestColumn)
        {
            TParameter element;

            System.Int32 Counter;

            if (ADestColumn == -1)
            {
                ADestColumn = column;
            }

            // remove all parameters in column
            Counter = 0;

            while (Counter < Fparameters.Count)
            {
                element = (TParameter)Fparameters[Counter];

                if (element.column == ADestColumn)
                {
                    Fparameters.RemoveAt(Counter);
                }
                else
                {
                    Counter = Counter + 1;
                }
            }

            // copy parameters from other list
            foreach (TParameter element2 in AOtherList.Fparameters)
            {
                if (element2.column == column)
                {
                    Add(element2.name, element2.value, ADestColumn, element2.level);
                }
            }
        }

        /// <summary>
        /// copy the whole list
        /// </summary>
        /// <param name="AOtherList"></param>
        public void Copy(TParameterList AOtherList)
        {
            Copy(AOtherList, -1, -1, eParameterFit.eBestFit, -1);
        }

        /// <summary>
        /// add the parameters from another list, overwriting existing values, but not deleting parameters as Copy does
        /// </summary>
        /// <param name="AOtherList"></param>
        public void Add(TParameterList AOtherList)
        {
            foreach (TParameter element in AOtherList.Fparameters)
            {
                Add(element.name, element.value, element.column, element.level);
            }
        }

        /// <summary>
        /// add all parameters that do not have an equivalent in this parameter list
        /// </summary>
        public void CopyMissing(TParameterList AOtherList)
        {
            foreach (TParameter param in AOtherList.Fparameters)
            {
                /*
                 * Do not use ParameterList.Exists() because that
                 * function should be renamed to
                 * ParameterList.ExistsOrEmpty(). We only actually
                 * want to check ParamterList.Exists() here. Instead,
                 * just check if GetParamter() returns null.
                 */
                if (GetParameter(param.name) == null)
                {
                    Add(param.name, param.value, param.column);
                }
            }
        }

        /// <summary>
        /// Common procedure to add a parameter, expects the value as a variant
        /// </summary>
        public void Add(String parameterId, TVariant value, int column = -1, int depth = -1)
        {
            // find if there is already an element in the list with the exact same column/level combination
            foreach (TParameter element in Fparameters)
            {
                if ((element.name == parameterId) && (element.level == depth) && (element.column == column))
                {
                    element.value = value;
                    return;
                }
            }

            // else add a new element
            TParameter element2 = new TParameter(parameterId, value, column, depth);
            Fparameters.Add(element2);
        }

        /// <summary>
        /// Procedure to add a parameter of type Boolean
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Add(String parameterId, bool value, int column = -1, int depth = -1)
        {
            Add(parameterId, new TVariant(value), column, depth);
        }

        /// <summary>
        /// Procedure to add a parameter of type Decimal
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Add(String parameterId, decimal value, int column = -1, int depth = -1)
        {
            Add(parameterId, new TVariant(value), column, depth);
        }

        /// <summary>
        /// Procedure to add a parameter of type String
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Add(String parameterId, String value, int column = -1, int depth = -1)
        {
            Add(parameterId, new TVariant(value), column, depth);
        }

        /// <summary>
        /// Procedure to add a parameter of type DateTime
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Add(String parameterId,
            System.DateTime value,
            int column = -1,
            int depth = -1)
        {
            Add(parameterId, new TVariant(value), column, depth);
        }

        /// <summary>
        /// Procedure to add a parameter of type Int32
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Add(String parameterId,
            System.Int32 value,
            int column = -1,
            int depth = -1)
        {
            Add(parameterId, new TVariant(value), column, depth);
        }

        /// <summary>
        /// Remove a variable; it will not exist anymore
        ///
        /// </summary>
        /// <returns>void</returns>
        public void RemoveVariable(String AParameterId, int AColumn, int ADepth = -1, eParameterFit AExact = eParameterFit.eBestFit)
        {
            TParameter element;

            element = GetParameter(AParameterId, AColumn, ADepth, AExact);

            while (element != null)
            {
                Fparameters.Remove(element);

                element = GetParameter(AParameterId, AColumn, ADepth, AExact);
            }
        }

        /// <summary>
        /// remove variable completely from list, all occurrences
        /// </summary>
        /// <param name="AParameterId"></param>
        public void RemoveVariable(String AParameterId)
        {
            TParameter toDelete = null;

            do
            {
                if (toDelete != null)
                {
                    Fparameters.Remove(toDelete);
                    toDelete = null;
                }

                foreach (TParameter element in Fparameters)
                {
                    if (StringHelper.IsSame(element.name, AParameterId))
                    {
                        toDelete = element;
                    }
                }
            } while (toDelete != null);
        }

        /// <summary>
        /// Test if a value other than NOTFOUND would be returned
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="column"></param>
        /// <param name="depth"></param>
        /// <param name="exact">determines how strictly a match has to fit the request; can be eExact, eBestFit, eAllColumnFit, eBestFitEvenLowerLevel</param>
        /// <returns>true if the parameter exists in the current collection
        /// </returns>
        public Boolean Exists(String parameterId, int column = -1, int depth = -1, eParameterFit exact = eParameterFit.eBestFit)
        {
            Boolean ReturnValue;
            TParameter element;

            ReturnValue = false;
            element = GetParameter(parameterId, column, depth, exact);

            if (element != null)
            {
                ReturnValue = ((!element.value.IsNil()));
            }

            return ReturnValue;
        }

        /// <summary>
        /// Prints a message to log with all occurrences of the given variable in the parameter list
        /// This can be helpful for debugging.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Debug(String parameterId)
        {
            String message;

            message = Environment.NewLine;

            foreach (TParameter element in Fparameters)
            {
                if (StringHelper.IsSame(element.name, parameterId))
                {
                    message = message + element.name + ' ' + element.value.ToString() + ' ' + element.column.ToString() + ' ' +
                              element.level.ToString() + ' ' + Environment.NewLine;
                }
            }

            TLogging.Log(message);
        }

        /// <summary>
        /// Common procedure to retrieve a parameter of any type; will return a TVariant object
        /// </summary>
        /// <returns>void</returns>
        public TVariant Get(String parameterId, int column = -1, int depth = -1, eParameterFit exact = eParameterFit.eBestFit)
        {
            TParameter element = GetParameter(parameterId, column, depth, exact);

            if (element != null)
            {
                return element.value;
            }

            return new TVariant();
        }

        /// <summary>
        /// Common procedure to retrieve a parameter or a default value; will return a TVariant object
        ///
        /// </summary>
        /// <returns>void</returns>
        public TVariant GetOrDefault(String parameterId, int column, TVariant ADefault)
        {
            TVariant ReturnValue;

            ReturnValue = Get(parameterId, column, -1, eParameterFit.eBestFit);

            if (ReturnValue.IsNil())
            {
                ReturnValue = ADefault;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Common procedure to retrieve a parameter of any type; will return a TParameter object
        ///
        /// </summary>
        /// <returns>void</returns>
        public TParameter GetParameter(String parameterId, int column = -1, int depth = -1, eParameterFit exact = eParameterFit.eBestFit)
        {
            TParameter ReturnValue = null;
            TParameter columnFit = null;
            TParameter commonFit = null;
            TParameter allColumnFit = null;
            TParameter bestLevelFit = null;
            TParameter lowerLevelFit = null;
            TParameter anyFit = null;
            int closestLevel = -1;
            int lowerLevel = 20;

            foreach (TParameter element in Fparameters)
            {
                if (StringHelper.IsSame(element.name, parameterId))
                {
                    // is there an exact match?
                    if ((element.level == depth) && (element.column == column))
                    {
                        return element;
                    }

                    // there is a global match
                    if (((element.level == -1) || (element.level == -2)) && (element.column == -1))
                    {
                        commonFit = element;
                    }

                    // there is a match for all data columns (ALLCOLUMNS)
                    if ((element.level == depth) && (element.column == TParameter.ALLCOLUMNS))
                    {
                        allColumnFit = element;
                    }

                    // has the exact column and is valid for all lines (1) or (2)
                    if (((element.level == -1) || (element.level == -2)) && (element.column == column))
                    {
                        columnFit = element;
                    }

                    // has the exact column or an column = 1 and the next line above
                    if (((element.column == -1) || (element.column == column)) && (element.level > closestLevel) && (element.level <= depth))
                    {
                        bestLevelFit = element;
                        closestLevel = element.level;
                    }

                    // has the exact column or an column = 1 and a lower line
                    if (((element.column == -1) || (element.column == column)) && (element.level < lowerLevel) && (element.level > depth))
                    {
                        lowerLevelFit = element;
                        lowerLevel = element.level;
                    }

                    // we are looking for any occurrence
                    if ((depth == -1) && (column == -1))
                    {
                        anyFit = element;
                    }
                } // if
            } // foreach

            if (exact == eParameterFit.eExact)
            {
                // no exact fitting element was found
                return null;
            }

            if (exact == eParameterFit.eAllColumnFit)
            {
                if (allColumnFit != null)
                {
                    ReturnValue = allColumnFit;
                }

                return ReturnValue;
            }

            if (columnFit != null)
            {
                return columnFit;
            }

            if (bestLevelFit != null)
            {
                return bestLevelFit;
            }

            if (commonFit != null)
            {
                return commonFit;
            }

            if ((lowerLevelFit != null) && (exact == eParameterFit.eBestFitEvenLowerLevel))
            {
                return lowerLevelFit;
            }

            if ((ReturnValue == null) && (exact == eParameterFit.eBestFit) && (anyFit != null))
            {
                return anyFit;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Read the parameters from a text file (json format);
        /// used for loading settings
        /// </summary>
        /// <param name="filename">relative or absolute filename
        /// </param>
        /// <returns>void</returns>
        public void Load(String filename)
        {
            String jsonString;

            if (!System.IO.File.Exists(filename))
            {
                throw new Exception("file " + filename + " could not be found.");
            }

            using (StreamReader sr = new StreamReader(filename))
            {
                jsonString = sr.ReadToEnd();
            }

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Object[] list = (Object[])serializer.DeserializeObject(jsonString);

                foreach (Dictionary<string, object> param in list)
                {
                    string name = String.Empty;
                    int columnNr = -1, levelNr = -1;
                    TVariant value = new TVariant();
                    foreach (KeyValuePair<string, object> entry in param)
                    {
                        if (entry.Key == "name")
                        {
                            name = entry.Value.ToString();
                        }
                        else if (entry.Key == "column")
                        {
                            columnNr = Convert.ToInt32(entry.Value);
                        }
                        else if (entry.Key == "level")
                        {
                            levelNr = Convert.ToInt32(entry.Value);
                        }
                        else if (entry.Key == "value")
                        {
                            value = TVariant.DecodeFromString(entry.Value.ToString());
                        }
                    }

                    Add(name, value, columnNr, levelNr);
                }
            }
            catch (Exception E)
            {
                throw new Exception(E.Message);
            }
        }

        /// <summary>
        /// useful for storing the parameter lists and comparing in unit tests
        /// </summary>
        public void Sort()
        {
            Fparameters.Sort();
        }

        /// <summary>
        /// Write all the parameters to a text file (json format);
        /// used for storing settings
        /// </summary>
        /// <param name="filename">relative or absolute filename</param>
        public void Save(String filename)
        {
            List<Object> list = new List<object>();

            foreach (TParameter element in Fparameters)
            {
                Dictionary<string,object> param = new Dictionary<string, object>();

                param.Add("name", element.name);
                if (element.column != -1)
                {
                    param.Add("column", element.column);
                }

                if (element.level != -1)
                {
                    param.Add("level", element.level);
                }

                param.Add("value", element.value.EncodeToString());

                list.Add(param);
            }

            // write the json file
            string data = JsonConvert.SerializeObject(list, Formatting.Indented).
                                     Replace("{" + Environment.NewLine + "    ", "{").
                                     Replace("," + Environment.NewLine + "    ", ", ").
                                     Replace(Environment.NewLine + "  }", "}").
                                     Replace(Environment.NewLine + "  ", Environment.NewLine + "\t");
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.Write(data);
            }
        }

        /// <summary>
        /// This formats the dates for different output, for example printing
        /// </summary>
        /// <param name="AOutputType">if this is 'Localized' then the dates are formatted in the format DD-MMM-YYYY</param>
        /// <returns>s a new copy of the parameters, with the correct formatting
        /// </returns>
        public TParameterList ConvertToFormattedStrings(String AOutputType)
        {
            TParameterList ReturnValue;

            ReturnValue = new TParameterList(this);

            foreach (TParameter p in ReturnValue.Fparameters)
            {
                if (p.value.TypeVariant != eVariantTypes.eString)
                {
                    p.value = new TVariant(p.value.ToFormattedString("", AOutputType));
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// overload, use Localized as default output type
        /// </summary>
        /// <returns></returns>
        public TParameterList ConvertToFormattedStrings()
        {
            return ConvertToFormattedStrings("Localized");
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class TColumnSettingCollection
    {
        private Dictionary <string, TColumnSetting>FColumns = new Dictionary <string, TColumnSetting>();

        /// <summary>
        /// Returns the number of TColumnsSettings
        /// </summary>
        public int Count
        {
            get
            {
                return FColumns.Count;
            }
        }

        /// <summary>
        /// Resizes all Column Widths
        /// </summary>
        /// <param name="AFactor"></param>
        public void ResizeAllColumns(float AFactor)
        {
            foreach (KeyValuePair <string, TColumnSetting>KVPair in FColumns)
            {
                KVPair.Value.Width = KVPair.Value.Width * AFactor;
            }
        }

        /// <summary>
        /// Gets the FastReport "Left" for a Position
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public float GetLeftValueForPositionIndex(int pos)
        {
            float returnValue = 0;

            foreach (KeyValuePair <string, TColumnSetting>KVPair in FColumns)
            {
                if (KVPair.Value.Position < pos)
                {
                    returnValue += KVPair.Value.Width;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the FastReport "Width" for a Position. Returns 0 if index not found.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public float GetWidthValueForPositionIndex(int pos)
        {
            foreach (KeyValuePair <string, TColumnSetting>KVPair in FColumns)
            {
                if (KVPair.Value.Position == pos)
                {
                    return KVPair.Value.Width;
                }
            }

            return 0;
        }

        /// <summary>
        /// Gets the total width of all columns.
        /// </summary>
        /// <returns></returns>
        public float GetTotalWidth()
        {
            float ReturnValue = 0;

            foreach (KeyValuePair <string, TColumnSetting>KVPair in FColumns)
            {
                ReturnValue += KVPair.Value.Width;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Sets the Column Settings for an existing or a new Column Name
        /// </summary>
        /// <param name="AColumnName"></param>
        /// <param name="AColumnSetting"></param>
        public void SetSettingForColumn(String AColumnName, TColumnSetting AColumnSetting)
        {
            if (FColumns.ContainsKey(AColumnName))
            {
                FColumns[AColumnName] = AColumnSetting;
            }
            else
            {
                FColumns.Add(AColumnName, AColumnSetting);
            }
        }

        /// <summary>
        /// Sets the Column Settings for an existing or a new Column Name stored inside the AColumnSetting object.
        /// </summary>
        /// <param name="AColumnSetting"></param>
        public void SetSettingForColumn(TColumnSetting AColumnSetting)
        {
            if (FColumns.ContainsKey(AColumnSetting.ColumnName))
            {
                FColumns[AColumnSetting.FColumnName] = AColumnSetting;
            }
            else
            {
                if (AColumnSetting.FColumnName != String.Empty)
                {
                    FColumns.Add(AColumnSetting.FColumnName, AColumnSetting);
                }
                else
                {
                    throw new Exception(Catalog.GetString("TColumnSetting does not contain a name."));
                }
            }
        }

        /// <summary>
        /// Returns true if TColumnSetting exists for Column Name
        /// </summary>
        /// <param name="AColumnName"></param>
        /// <returns></returns>
        public Boolean HasSettingForColumn(String AColumnName)
        {
            if (FColumns.ContainsKey(AColumnName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the TColumnSetting by Column Name
        /// </summary>
        /// <param name="AColumnName"></param>
        /// <returns></returns>
        public TColumnSetting GetSettingForColumn(String AColumnName)
        {
            if (FColumns.ContainsKey(AColumnName))
            {
                return FColumns[AColumnName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the TColumnSetting by Column Index
        /// </summary>
        /// <param name="AColumnIndex"></param>
        /// <returns></returns>
        public TColumnSetting GetSettingForColumn(int AColumnIndex)
        {
            if (FColumns.ContainsKey(FColumns.Keys.ElementAt(AColumnIndex)))
            {
                return FColumns[FColumns.Keys.ElementAt(AColumnIndex)];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Serialises the Collection
        /// </summary>
        /// <returns></returns>
        public string SerialiseCollection()
        {
            List <string>collection = new List <string>();

            foreach (KeyValuePair <string, TColumnSetting>KVPair in FColumns)
            {
                if (KVPair.Value.FColumnName == String.Empty)
                {
                    KVPair.Value.FColumnName = KVPair.Key;
                }

                collection.Add(KVPair.Value.Serialise());
            }

            return String.Join("&&", collection);
        }

        /// <summary>
        /// Deserialises the Collection
        /// </summary>
        /// <param name="ASerialisedCollection"></param>
        public void DeserialiseCollection(string ASerialisedCollection)
        {
            string[] ColumnSettingArray = ASerialisedCollection.Split(new string[] { "&&" }, StringSplitOptions.None);

            foreach (string ColumnSetting in ColumnSettingArray)
            {
                TColumnSetting tcs = new TColumnSetting(ColumnSetting);
                SetSettingForColumn(tcs);
            }
        }
    }
    /// <summary>
    ///
    /// </summary>
    public class TColumnSetting
    {
        /// <summary>
        /// Column Name
        /// </summary>
        public String FColumnName;
        /// <summary>
        /// Gets or Sets the Column Name. It is not allowed to be empty.
        /// </summary>
        public string ColumnName
        {
            get
            {
                return FColumnName;
            }

            set
            {
                if (value != String.Empty)
                {
                    FColumnName = value;
                }
                else
                {
                    throw new Exception(Catalog.GetString("Input string for FColumnName is empty."));
                }
            }
        }

        /// <summary>
        /// Wdith of the Column
        /// </summary>
        public float Width {
            get; set;
        }

        /// <summary>
        /// Position of the Column
        /// </summary>
        public int Position {
            get; set;
        }


        /// <summary>
        /// Default Contructor.
        /// </summary>
        /// <param name="AColumnName">The Name of the Column</param>
        /// <param name="AWidth">The Width of the Column</param>
        /// <param name="APosition">The Position of the Column</param>
        public TColumnSetting(string AColumnName, float AWidth, int APosition)
        {
            ColumnName = AColumnName;
            Width = AWidth;
            Position = APosition;
        }

        /// <summary>
        /// Constructor that takes a Serialised String
        /// </summary>
        /// <param name="ASerialisedString"></param>
        public TColumnSetting(string ASerialisedString)
        {
            Deserialise(ASerialisedString);
        }

        /// <summary>
        /// Serialises All Members to String
        /// </summary>
        /// <returns></returns>
        public string Serialise()
        {
            List <string>AllMembersAsString = new List <string>();
            AllMembersAsString.Add(ColumnName);
            AllMembersAsString.Add(Width.ToString());
            AllMembersAsString.Add(Position.ToString());
            return String.Join("#$#", AllMembersAsString);
        }

        /// <summary>
        /// Deserialises String to Object
        /// </summary>
        /// <param name="FAllMembersAsSerialisedString"></param>
        public void Deserialise(string FAllMembersAsSerialisedString)
        {
            string[] VariablesArray = FAllMembersAsSerialisedString.Split(new string[] { "#$#" }, StringSplitOptions.None);

            ColumnName = VariablesArray[0];
            Width = float.Parse(VariablesArray[1]);
            Position = int.Parse(VariablesArray[2]);
        }
    }
}
