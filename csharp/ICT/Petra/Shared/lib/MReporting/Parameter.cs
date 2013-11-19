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
using System.IO;
using Ict.Common.IO;
using Ict.Common;
using Ict.Common.Remoting.Server;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Globalization;
using Ict.Petra.Shared.MReporting;
using System.Xml;

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
    public class TParameter
    {
        /// <summary>
        /// name of the parameter
        /// </summary>
        public String name;

        /// <summary>can be between 1 and 12 and more, or one of the constants defined in Ict.Petra.Shared.MReporting.Consts</summary>
        public int column;

        /// <summary>level 1 is the main level, the bigger the number, the lower the level.</summary>
        public int level;

        /// <summary>there can be several subreports on a report, with different formatting (lines etc). default is 1 for report wide settings, 0 for the first report</summary>
        public int subreport;

        /// <summary>the value of this parameter</summary>
        public TVariant value;

        /// <summary>CALCULATIONPARAMETERS should not be written back to the UI</summary>
        public int paramType;

        /// <summary>can be used for later calculations, when the depending values have been calculated</summary>
        public System.Object pRptElement;

        /// <summary>can be used for later calculations, when the depending values have been calculated</summary>
        public System.Object pRptGroup;

        /// <summary>
        /// constructor
        /// </summary>
        public TParameter(String pname,
            TVariant pvalue,
            int pcolumn,
            int plevel,
            int psubreport,
            System.Object pRptElement,
            System.Object pRptGroup,
            int paramType)
        {
            name = pname;
            value = pvalue;
            column = pcolumn;
            level = plevel;
            subreport = psubreport;
            this.pRptElement = pRptElement;
            this.pRptGroup = pRptGroup;
            this.paramType = paramType;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="pname"></param>
        /// <param name="pvalue"></param>
        /// <param name="pcolumn"></param>
        /// <param name="plevel"></param>
        /// <param name="psubreport"></param>
        /// <param name="pRptElement"></param>
        /// <param name="pRptGroup"></param>
        public TParameter(String pname, TVariant pvalue, int pcolumn, int plevel, int psubreport, System.Object pRptElement, System.Object pRptGroup)
            : this(pname, pvalue, pcolumn, plevel, psubreport, pRptElement, pRptGroup, -1)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="pname"></param>
        /// <param name="pvalue"></param>
        /// <param name="pcolumn"></param>
        /// <param name="plevel"></param>
        /// <param name="psubreport"></param>
        /// <param name="pRptElement"></param>
        public TParameter(String pname, TVariant pvalue, int pcolumn, int plevel, int psubreport, System.Object pRptElement)
            : this(pname, pvalue, pcolumn, plevel, psubreport, pRptElement, null, -1)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="pname"></param>
        /// <param name="pvalue"></param>
        /// <param name="pcolumn"></param>
        /// <param name="plevel"></param>
        /// <param name="psubreport"></param>
        public TParameter(String pname, TVariant pvalue, int pcolumn, int plevel, int psubreport)
            : this(pname, pvalue, pcolumn, plevel, psubreport, null, null, -1)
        {
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
            subreport = copy.subreport;
            this.pRptElement = copy.pRptElement;
            this.pRptGroup = copy.pRptGroup;
            this.paramType = copy.paramType;
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
        get{
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
                        Convert.ToInt32(row["column"]), Convert.ToInt32(row["level"]), Convert.ToInt16(row["subreport"])));
            }

            if ((TLogging.DebugLevel >= TLogging.DEBUGLEVEL_REPORTING) && (TSrvSetting.ServerLogFile.Length > 0))
            {
                Save(Path.GetDirectoryName(TSrvSetting.ServerLogFile) + Path.DirectorySeparatorChar + "param.xml");
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
            ReturnValue.Columns.Add(new System.Data.DataColumn("subreport", typeof(System.Int32)));
            ReturnValue.Columns.Add(new System.Data.DataColumn("value", typeof(String)));

            foreach (TParameter element in Fparameters)
            {
                if (element.paramType == ReportingConsts.CALCULATIONPARAMETERS)
                {
                    continue;
                }

                row = ReturnValue.NewRow();
                row["name"] = element.name;
                row["column"] = (System.Object)element.column;
                row["level"] = (System.Object)element.level;
                row["subreport"] = (System.Object)element.subreport;
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
                    Add(element2.name, element2.value, ADestColumn, element2.level, element2.subreport);
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
                Add(element.name, element.value, element.column, element.level, element.subreport);
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
        /// Procedure to move a column
        /// That means, all parameters in ANewColumn will be deleted,
        /// and all parameters in the column AOldColumn will be changed to column ANewColumn.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void MoveColumn(int AOldColumn, int ANewColumn)
        {
            // remove all parameters in column ANewColumn
            RemoveColumn(ANewColumn);

            // move all parameters from column AOldColumn to ANewColumn
            foreach (TParameter element in Fparameters)
            {
                if (element.column == AOldColumn)
                {
                    element.column = ANewColumn;
                }
            }
        }

        /// <summary>
        /// Switch to columns; can be used to move a column forward.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void SwitchColumn(int AColumn1, int AColumn2)
        {
            foreach (TParameter element in Fparameters)
            {
                if (element.column == AColumn1)
                {
                    element.column = AColumn2;
                }
                else if (element.column == AColumn2)
                {
                    element.column = AColumn1;
                }
            }
        }

        /// <summary>
        /// Remove a column; will not move the following columns
        ///
        /// </summary>
        /// <returns>void</returns>
        public void RemoveColumn(int AColumn)
        {
            TParameter element;

            System.Int32 Counter;
            Counter = 0;

            while (Counter < Fparameters.Count)
            {
                element = (TParameter)Fparameters[Counter];

                if (element.column == AColumn)
                {
                    Fparameters.RemoveAt(Counter);
                }
                else
                {
                    Counter = Counter + 1;
                }
            }
        }

        /// <summary>
        /// Common procedure to add a parameter, expects the value as a variant
        /// </summary>
        public void Add(String parameterId, TVariant value, int column, int depth, System.Object pRptElement, System.Object pRptGroup, int paramType)
        {
            Int32 subreport;

            if (parameterId != "CurrentSubReport")
            {
                subreport = GetOrDefault("CurrentSubReport", -1, new TVariant(-1)).ToInt();
            }
            else
            {
                subreport = -1;
                paramType = ReportingConsts.CALCULATIONPARAMETERS;
            }

            // find if there is already an element in the list with the exact same column/level combination
            foreach (TParameter element in Fparameters)
            {
                if ((element.name == parameterId) && (element.level == depth) && (element.column == column) && (element.subreport == subreport))
                {
                    element.value = value;
                    element.pRptElement = pRptElement;
                    element.pRptGroup = pRptGroup;
                    return;
                }
            }

            // else add a new element
            TParameter element2 = new TParameter(parameterId, value, column, depth, subreport, pRptElement, pRptGroup, paramType);
            Fparameters.Add(element2);
        }

        /// <summary>
        /// overloaded method
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="value"></param>
        /// <param name="column"></param>
        /// <param name="depth"></param>
        public void Add(String parameterId, TVariant value, int column, int depth)
        {
            Add(parameterId, value, column, depth, null, null, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="value"></param>
        /// <param name="column"></param>
        public void Add(String parameterId, TVariant value, int column)
        {
            Add(parameterId, value, column, -1, null, null, -1);
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="value"></param>
        public void AddCalculationParameter(String parameterId, TVariant value)
        {
            Add(parameterId, value, -1, -1, null, null, ReportingConsts.CALCULATIONPARAMETERS);
        }

        /// <summary>
        /// overloaded add
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="value"></param>
        public void Add(String parameterId, TVariant value)
        {
            Add(parameterId, value, -1, -1, null, null, -1);
        }

        /// <summary>
        /// procedure to add a parameter, expects the value as a variant;
        /// used when loading from xml file (value of subreport is known)
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Add(String parameterId, TVariant value, int column, int depth, int subreport)
        {
            Add("CurrentSubReport", new TVariant(subreport));
            Add(parameterId, value, column, depth, null, null, -1);
        }

        /// <summary>
        /// Procedure to add a parameter of type Boolean
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Add(String parameterId, bool value, int column, int depth, System.Object pRptElement, System.Object pRptGroup, int paramType)
        {
            Add(parameterId, new TVariant(value), column, depth, pRptElement, pRptGroup, paramType);
        }

        /// <summary>
        /// overloaded add
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="value"></param>
        public void Add(String parameterId, bool value)
        {
            Add(parameterId, value, -1, -1, null, null, -1);
        }

        /// <summary>
        /// Procedure to add a parameter of type Decimal
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Add(String parameterId, decimal value, int column, int depth, System.Object pRptElement, System.Object pRptGroup, int paramType)
        {
            Add(parameterId, new TVariant(value), column, depth, pRptElement, pRptGroup, paramType);
        }

        /// <summary>
        /// overloaded add for Decimal
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="value"></param>
        public void Add(String parameterId, decimal value)
        {
            Add(parameterId, value, -1, -1, null, null, -1);
        }

        /// <summary>
        /// Procedure to add a parameter of type String
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Add(String parameterId, String value, int column, int depth, System.Object pRptElement, System.Object pRptGroup, int paramType)
        {
            Add(parameterId, new TVariant(value), column, depth, pRptElement, pRptGroup, paramType);
        }

        /// <summary>
        /// overloaded add for string
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="value"></param>
        public void Add(String parameterId, String value)
        {
            Add(parameterId, value, -1, -1, null, null, -1);
        }

        /// <summary>
        /// Procedure to add a parameter of type DateTime
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Add(String parameterId,
            System.DateTime value,
            int column,
            int depth,
            System.Object pRptElement,
            System.Object pRptGroup,
            int paramType)
        {
            Add(parameterId, new TVariant(value), column, depth, pRptElement, pRptGroup, paramType);
        }

        /// <summary>
        /// overloaded add for date
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="value"></param>
        public void Add(String parameterId, System.DateTime value)
        {
            Add(parameterId, value, -1, -1, null, null, -1);
        }

        /// <summary>
        /// Procedure to add a parameter of type Int32
        ///
        /// </summary>
        /// <returns>void</returns>
        public void Add(String parameterId,
            System.Int32 value,
            int column,
            int depth,
            System.Object pRptElement,
            System.Object pRptGroup,
            int paramType)
        {
            Add(parameterId, new TVariant(value), column, depth, pRptElement, pRptGroup, paramType);
        }

        /// <summary>
        /// overloaded add for int32
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="value"></param>
        public void Add(String parameterId, System.Int32 value)
        {
            Add(parameterId, value, -1, -1, null, null, -1);
        }

        /// <summary>
        /// Remove a variable; it will not exist anymore
        ///
        /// </summary>
        /// <returns>void</returns>
        public void RemoveVariable(String AParameterId, int AColumn, int ADepth, eParameterFit AExact)
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
        /// overloaded version of RemoveVariable; uses bestfit
        /// </summary>
        /// <param name="AParameterId"></param>
        /// <param name="AColumn"></param>
        public void RemoveVariable(String AParameterId, int AColumn)
        {
            RemoveVariable(AParameterId, AColumn, -1, eParameterFit.eBestFit);
        }

        /// <summary>
        /// remove variable completely from list, all occurances
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
        public Boolean Exists(String parameterId, int column, int depth, eParameterFit exact)
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
        /// overloaded method
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="column"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public Boolean Exists(String parameterId, int column, int depth)
        {
            return Exists(parameterId, column, depth, eParameterFit.eBestFit);
        }

        /// <summary>
        /// overloaded
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public Boolean Exists(String parameterId, int column)
        {
            return Exists(parameterId, column, -1, eParameterFit.eBestFit);
        }

        /// <summary>
        /// overloaded version of exists
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        public Boolean Exists(String parameterId)
        {
            return Exists(parameterId, -1, -1, eParameterFit.eBestFit);
        }

        /// <summary>
        /// Prints a message to log with all occurances of the given variable in the parameter list
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
                              element.level.ToString() + ' ' + element.subreport.ToString() + Environment.NewLine;
                }
            }

            TLogging.Log(message);
        }

        /// <summary>
        /// Common procedure to retrieve a parameter of any type; will return a TVariant object
        /// </summary>
        /// <returns>void</returns>
        public TVariant Get(String parameterId, int column, int depth, eParameterFit exact)
        {
            TParameter element = GetParameter(parameterId, column, depth, exact);

            if (element != null)
            {
                return element.value;
            }

            return new TVariant();
        }

        /// <summary>
        /// overloaded method
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="column"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public TVariant Get(String parameterId, int column, int depth)
        {
            return Get(parameterId, column, depth, eParameterFit.eBestFit);
        }

        /// <summary>
        /// overloaded version
        /// </summary>
        /// <param name="parameterId"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public TVariant Get(String parameterId, int column)
        {
            return Get(parameterId, column, -1, eParameterFit.eBestFit);
        }

        /// <summary>
        /// overloaded version of Get
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        public TVariant Get(String parameterId)
        {
            return Get(parameterId, -1, -1, eParameterFit.eBestFit);
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
        /// get an untyped reference to an object of TRptGrpValue
        /// </summary>
        /// <returns>an untyped reference to an object of TRptGrpValue
        /// </returns>
        public System.Object GetGrpValue(String parameterId, int column, int depth, eParameterFit exact)
        {
            System.Object ReturnValue;
            TParameter element;
            ReturnValue = null;
            element = GetParameter(parameterId, column, depth, exact);

            if ((element != null) && (element.pRptGroup != null))
            {
                ReturnValue = element.pRptGroup;
            }

            return ReturnValue;
        }

        /// <summary>
        /// overload for GetGrpValue
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        public System.Object GetGrpValue(String parameterId)
        {
            return GetGrpValue(parameterId, -1, -1, eParameterFit.eBestFit);
        }

        /// <summary>
        /// Common procedure to retrieve a parameter of any type; will return a TParameter object
        ///
        /// </summary>
        /// <returns>void</returns>
        public TParameter GetParameter(String parameterId, int column, int depth, eParameterFit exact)
        {
            int subreport = -1;

            TParameter ReturnValue = null;
            TParameter columnFit = null;
            TParameter commonFit = null;
            TParameter allColumnFit = null;
            TParameter bestLevelFit = null;
            TParameter lowerLevelFit = null;
            TParameter anyFit = null;
            int closestLevel = -1;
            int lowerLevel = 20;

            if (parameterId != "CurrentSubReport")
            {
                subreport = GetOrDefault("CurrentSubReport", -1, new TVariant(-1)).ToInt();
            }

/* Perhaps I'll get more speed if I remove this..
 *          // just to be careful: if curly brackets were used by accident in the xml file, remove them
 *          if (parameterId[0] == '{')
 *          {
 *              TLogging.Log(
 *                  "deprecated: " + parameterId +
 *                  "; make sure your report xml file is correct, you might be using a variable with brackets in a function call to exists or isnull");
 *              parameterId = parameterId.Substring(1, parameterId.Length - 2);
 *
 *              if ((parameterId[0] == '{') || (parameterId[0] == '#'))
 *              {
 *                  parameterId = parameterId.Substring(1, parameterId.Length - 2);
 *              }
 *          }
 */
            foreach (TParameter element in Fparameters)
            {
                if (((element.subreport == subreport) || (element.subreport == -1)) && StringHelper.IsSame(element.name, parameterId))
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
                    if ((element.level == depth) && (element.column == ReportingConsts.ALLCOLUMNS))
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

                    // we are looking for any occurance
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
        /// overload for GetParameter
        /// </summary>
        /// <param name="parameterId"></param>
        /// <returns></returns>
        public TParameter GetParameter(String parameterId)
        {
            return GetParameter(parameterId, -1, -1, eParameterFit.eBestFit);
        }

        /// <summary>
        /// Read the parameters from a text file (xml format);
        /// used for loading settings
        /// </summary>
        /// <param name="filename">relative or absolute filename
        /// </param>
        /// <returns>void</returns>
        public void Load(String filename)
        {
            XmlNode startNode;
            XmlNode node;
            TXMLParser myDoc;
            String level;
            String column;
            int levelNr;
            int columnNr;
            int subreport;

            myDoc = new TXMLParser(filename, false);
            try
            {
                startNode = myDoc.GetDocument().DocumentElement;

                if (startNode.Name.ToLower() == "parameters")
                {
                    node = startNode.FirstChild;

                    while (node != null)
                    {
                        if (node.Name == "Parameter")
                        {
                            column = TXMLParser.GetAttribute(node, "column");
                            level = TXMLParser.GetAttribute(node, "level");
                            subreport = TXMLParser.GetIntAttribute(node, "subreport");
                            columnNr = -1;
                            levelNr = -1;

                            if (column.Length != 0)
                            {
                                columnNr = (int)StringHelper.StrToInt(column);
                            }

                            if (level.Length != 0)
                            {
                                levelNr = (int)StringHelper.StrToInt(level);
                            }

                            Add(TXMLParser.GetAttribute(node, "id"),
                                TVariant.DecodeFromString(TXMLParser.GetAttribute(node, "value")),
                                columnNr, levelNr, subreport);
                        }

                        node = node.NextSibling;
                    }
                }
            }
            catch (Exception E)
            {
                throw new Exception(E.Message);
            }
        }

        /// <summary>
        /// Write all the parameters to a text file (xml format);
        /// used for storing settings
        /// </summary>
        /// <param name="filename">relative or absolute filename</param>
        /// <param name="AWithDebugInfo">should internal values be printed, only true for Testing
        /// </param>
        /// <returns>void</returns>
        public void Save(String filename, bool AWithDebugInfo = false)
        {
            XmlTextWriter textWriter;

            // create a file
            textWriter = new XmlTextWriter(filename, null);

            // Opens the document
            textWriter.WriteStartDocument();

            // Write first element
            textWriter.WriteWhitespace(new String((char)10, 1));
            textWriter.WriteStartElement("Parameters");
            textWriter.WriteWhitespace(new String((char)10, 1));

            foreach (TParameter element in Fparameters)
            {
                if (AWithDebugInfo
                    || ((element.paramType != ReportingConsts.CALCULATIONPARAMETERS) && (element.value.ToString() != "rptGrpValue")
                        && (element.value.ToString() != "calculation")))
                {
                    textWriter.WriteWhitespace("  ");
                    textWriter.WriteStartElement("Parameter", "");
                    textWriter.WriteStartAttribute("id", "");
                    textWriter.WriteString(element.name);
                    textWriter.WriteEndAttribute();

                    if (element.column != -1)
                    {
                        textWriter.WriteStartAttribute("column", "");
                        textWriter.WriteString(element.column.ToString());
                        textWriter.WriteEndAttribute();
                    }

                    if (element.level != -1)
                    {
                        textWriter.WriteStartAttribute("level", "");
                        textWriter.WriteString(element.level.ToString());
                        textWriter.WriteEndAttribute();
                    }

                    if (element.subreport != -1)
                    {
                        textWriter.WriteStartAttribute("subreport", "");
                        textWriter.WriteString(element.subreport.ToString());
                        textWriter.WriteEndAttribute();
                    }

                    textWriter.WriteAttributeString("value", element.value.EncodeToString());
                    textWriter.WriteEndElement();
                    textWriter.WriteWhitespace(new String((char)10, 1));
                }
            }

            // Ends the document.
            textWriter.WriteEndDocument();

            // close writer
            textWriter.Close();
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
}