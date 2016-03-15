//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Threading;
using GNU.Gettext;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared.MReporting;
using Ict.Common.Verification;
using SourceGrid;
using SourceGrid.Selection;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using Ict.Common;
using Ict.Petra.Client.CommonForms;
using Ict.Common.Controls;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// This class provides helper functions for UC_Columns
    /// </summary>
    public class TUC_ColumnHelper
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TUC_ColumnHelper()
        {
        }

        /// <summary>
        /// This procedure will switch the two columns
        /// </summary>
        /// <param name="AColumnParameters">List with the current columns</param>
        /// <param name="AFrom">Index of the column to move</param>
        /// <param name="ATo">Index of the new position of the column to move</param>
        /// <returns>void</returns>
        public static void SwitchColumn(ref TParameterList AColumnParameters, int AFrom, int ATo)
        {
            System.Int32 MaxDisplayColumns;
            System.Int32 Counter;
            System.Int32 ReferencedColumn;

            AColumnParameters.SwitchColumn(AFrom, ATo);

            /* switch the referenced columns in calculation */
            MaxDisplayColumns = AColumnParameters.Get("MaxDisplayColumns").ToInt();

            for (Counter = 0; Counter <= MaxDisplayColumns - 1; Counter += 1)
            {
                if (AColumnParameters.Exists("FirstColumn", Counter))
                {
                    ReferencedColumn = AColumnParameters.Get("FirstColumn", Counter).ToInt();

                    if (ReferencedColumn == AFrom)
                    {
                        ReferencedColumn = ATo;
                    }
                    else if (ReferencedColumn == ATo)
                    {
                        ReferencedColumn = AFrom;
                    }

                    AColumnParameters.Add("FirstColumn", new TVariant(ReferencedColumn), Counter);
                }

                if (AColumnParameters.Exists("SecondColumn", Counter))
                {
                    ReferencedColumn = AColumnParameters.Get("SecondColumn", Counter).ToInt();

                    if (ReferencedColumn == AFrom)
                    {
                        ReferencedColumn = ATo;
                    }
                    else if (ReferencedColumn == ATo)
                    {
                        ReferencedColumn = AFrom;
                    }

                    AColumnParameters.Add("SecondColumn", new TVariant(ReferencedColumn), Counter);
                }
            }
        }

        /// <summary>
        /// Checks if there is already a field with the same name in the grid. If yes, ask
        /// if the field should be added again.
        /// </summary>
        /// <param name="AColumnParameters">List with the current columns</param>
        /// <param name="AColumnName">Name of the field</param>
        /// <param name="ASelectedColumn">Index of the column in the grid</param>
        /// <returns>True if the field should be added. Otherwise false</returns>
        public static bool CheckAddDoubleEntry(ref TParameterList AColumnParameters, String AColumnName, int ASelectedColumn)
        {
            bool ReturnValue = true;
            DataTable ColumnTable = AColumnParameters.ToDataTable();

            String NewField = "eString:" + AColumnName;

            foreach (DataRow Row in ColumnTable.Rows)
            {
                if ((Row["value"].ToString() == NewField)
                    && (((int)Row["column"]) != ASelectedColumn))
                {
                    if (MessageBox.Show("The column is already there.\nDo you want to add it anyway?", "Add field?",
                            MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        ReturnValue = false;
                    }

                    break;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// get the function System.Object of the given calculation string
        /// </summary>
        /// <param name="AAvailableFunctions">List with the current functions</param>
        /// <param name="ACalculation">Name of the function</param>
        /// <returns>nil if the function cannot be found
        /// </returns>
        public static TColumnFunction GetFunction(ref ArrayList AAvailableFunctions, String ACalculation)
        {
            if (AAvailableFunctions != null)
            {
                foreach (TColumnFunction Func in AAvailableFunctions)
                {
                    if (Func.GetDisplayValue() == ACalculation)
                    {
                        return Func;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// get the function System.Object of the given calculation string
        /// </summary>
        /// <param name="AAvailableFunctions">List with the current functions</param>
        /// <param name="ACalculation">Name of the function</param>
        /// <param name="AParameterList">the current list of parameters</param>
        /// <param name="AColumnNumber">Number of the column</param>
        /// <returns>nil if the function cannot be found
        /// </returns>
        public static TColumnFunction GetFunction(ref ArrayList AAvailableFunctions,
            String ACalculation,
            TParameterList AParameterList,
            int AColumnNumber)
        {
            TColumnFunction ReturnValue;

            ReturnValue = GetFunction(ref AAvailableFunctions, ACalculation);

            if (ReturnValue == null)
            {
                /* this might be a general function that has a parameter, that is displayed */
                if (AAvailableFunctions != null)
                {
                    foreach (TColumnFunction Func in AAvailableFunctions)
                    {
                        if (Func.FDescription == ACalculation)
                        {
                            /* found an entry with e.g. DataLabelColumn */
                            /* now need to check if this columns FCalculationParameterValue is used */
                            if (AParameterList.Get(Func.FCalculationParameterName, AColumnNumber).ToString() == Func.FCalculationParameterValue)
                            {
                                return Func;
                            }
                        }
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Remove one colum from the parameter list.
        /// </summary>
        /// <param name="AColumnParameters">List with the current columns</param>
        /// <param name="AColumnIndex">Index of the column to remove</param>
        /// <returns>the MaxDisplayColumns number</returns>
        public static System.Int32 RemoveColumn(ref TParameterList AColumnParameters, int AColumnIndex)
        {
            AColumnParameters.RemoveColumn(AColumnIndex);

            /* need to move the following columns to the left */
            System.Int32 MaxColumn = AColumnParameters.Get("MaxDisplayColumns").ToInt() - 1;

            for (int Counter = AColumnIndex + 1; Counter <= MaxColumn; Counter += 1)
            {
                AColumnParameters.MoveColumn(Counter, Counter - 1);
            }

            AColumnParameters.Add("MaxDisplayColumns", MaxColumn);

            return MaxColumn;
        }

        /// <summary>
        /// Adds a new column to the column parameter list at the specified index.
        /// </summary>
        /// <param name="AColumnParameters">List with the current columns</param>
        /// <param name="AColumnIndex">Index where the new column is added</param>
        /// <returns>the MaxDisplayColumns number</returns>
        public static System.Int32 AddColumn(ref TParameterList AColumnParameters, int AColumnIndex)
        {
            System.Int32 NewColumn;
            System.Int32 NewMaxColumn;
            System.Int32 Counter;

            if (AColumnIndex == -1)
            {
                NewMaxColumn = AColumnParameters.Get("MaxDisplayColumns").ToInt();
                NewColumn = NewMaxColumn;
                AColumnParameters.Add("MaxDisplayColumns", NewColumn + 1);
            }
            else
            {
                NewColumn = AColumnIndex + 1;
                NewMaxColumn = AColumnParameters.Get("MaxDisplayColumns").ToInt() + 1;
                AColumnParameters.Add("MaxDisplayColumns", NewMaxColumn);

                /* need to move the columns to the right */
                for (Counter = NewMaxColumn - 1; Counter >= NewColumn + 1; Counter -= 1)
                {
                    AColumnParameters.MoveColumn(Counter - 1, Counter);
                }
            }

            return NewColumn;
        }

        /// <summary>
        /// Reads the selected values from the controls,
        /// and stores them into the parameter system of FCalculator
        ///
        /// </summary>
        /// <param name="AColumnParameters">List with the current columns</param>
        /// <param name="ACalculator"></param>
        /// <returns>the MaxDisplayColumns number</returns>
        public static System.Int32 ReadControls(ref TParameterList AColumnParameters, ref TRptCalculator ACalculator)
        {
            System.Int32 MaxDisplayColumns;

            MaxDisplayColumns = AColumnParameters.Get("MaxDisplayColumns").ToInt();
            ACalculator.GetParameters().Add("MaxDisplayColumns", MaxDisplayColumns);

            for (int Counter = 0; Counter <= MaxDisplayColumns - 1; Counter += 1)
            {
                ACalculator.GetParameters().Copy(AColumnParameters, Counter, -1, eParameterFit.eExact, Counter);

                if (!ACalculator.GetParameters().Exists("param_ytd", Counter))
                {
                    // if param_ytd is not set for the column then add it.
                    ACalculator.GetParameters().Add("param_ytd", new TVariant(false), Counter);
                }
            }

            ACalculator.SetMaxDisplayColumns(MaxDisplayColumns);

            return MaxDisplayColumns;
        }

        /// <summary>
        /// Sets the selected values in the controls, using the parameters loaded from a file
        ///
        /// </summary>
        /// <param name="AColumnParameters">List with the current columns</param>
        /// <param name="AParameters"></param>
        /// <returns>the MaxDisplayColumns number</returns>
        public static System.Int32 SetControls(ref TParameterList AColumnParameters, ref TParameterList AParameters)
        {
            System.Int32 MaxDisplayColumns = 0;

            /* copy values for columns to the current set of parameters */
            AColumnParameters.Clear();

            if (AParameters.Exists("MaxDisplayColumns"))
            {
                MaxDisplayColumns = AParameters.Get("MaxDisplayColumns").ToInt();
            }

            AColumnParameters.Add("MaxDisplayColumns", MaxDisplayColumns);

            for (int Counter = 0; Counter <= MaxDisplayColumns - 1; Counter += 1)
            {
                AColumnParameters.Copy(AParameters, Counter, -1, eParameterFit.eExact, Counter);
            }

            return MaxDisplayColumns;
        }

        /// <summary>
        /// Loads the data of the column parameters to the grid
        /// </summary>
        /// <param name="AGrid">Grid to show the values</param>
        /// <param name="AColumnTable">Table that holds the column parameter data</param>
        public static void LoadDataToGrid(ref TSgrdDataGrid AGrid,
            ref DataTable AColumnTable)
        {
            AGrid.Columns.Clear();

            for (int counter = 0; counter < AColumnTable.Columns.Count; ++counter)
            {
                AGrid.AddTextColumn("Column " + Convert.ToString(counter + 1), AColumnTable.Columns[counter]);
                // SourceGrid.Cells.ColumnHeader myColumnHeader = (SourceGrid.Cells.ColumnHeader)AGrid.Columns[counter].HeaderCell;
            }

            AGrid.DataSource = new DevAge.ComponentModel.BoundDataView(new DataView(AColumnTable));
            AGrid.DataSource.AllowEdit = false;
            AGrid.DataSource.AllowNew = false;
            AGrid.DataSource.AllowDelete = false;
            AGrid.AutoSizeCells();
        }
    }
}