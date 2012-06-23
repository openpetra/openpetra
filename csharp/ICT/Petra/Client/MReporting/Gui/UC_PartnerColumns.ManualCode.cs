//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr, timop
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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using SourceGrid;
using SourceGrid.Selection;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Shared.MPartner.Partner.Data;

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// Description of UC_Columns_ManualCode.
    /// </summary>
    public partial class TFrmUC_PartnerColumns
    {
        private ArrayList FAvailableFunctions;

        /// <summary>this holds the currently configured columns</summary>
        protected TParameterList FColumnParameters;

        /// <summary>this shows which column is currently selected; it is 1 if no column is selected</summary>
        protected int FSelectedColumn;

        /// <summary>helper variable to unselect the column in the grid after cancel or apply</summary>
        private bool FDuringApplyOrCancel;

        /// <summary>
        /// Initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FPetraUtilsObject = APetraUtilsObject;

            FColumnParameters = new TParameterList();
            FColumnParameters.Add("MaxDisplayColumns", 0);
            FPetraUtilsObject.FMaxDisplayColumns = 0;

            FDuringApplyOrCancel = false;

            FSelectedColumn = -1;

            grdColumns.SortableHeaders = false;
            grdColumns.SelectionMode = SourceGrid.GridSelectionMode.Column;
            grdColumns.AutoStretchColumnsToFitWidth = false;

            /* Hook up event that fires when a different row is selected */
            grdColumns.Selection.FocusColumnEntered += new ColumnEventHandler(this.GrdColumns_FocusColumnEntered);
            grdColumns.Selection.CellLostFocus += new ChangeActivePositionEventHandler(this.GrdColumns_CellLostFocus);
            grdColumns.Selection.CellGotFocus += new ChangeActivePositionEventHandler(this.GrdColumns_CellGotFocus);


            btnApply.Enabled = false;
            btnCancel.Enabled = false;
            txtColumnWidth.Enabled = false;
            cmbCalculation.Enabled = false;
        }

        #region Parameter/Settings Handling

        /// <summary>
        /// Sets the available functions (fields) that can be used for this report.
        /// </summary>
        /// <param name="AAvailableFunctions">List of TColumnFunction</param>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
            FAvailableFunctions = AAvailableFunctions;

            foreach (TColumnFunction colfunc in FAvailableFunctions)
            {
                cmbCalculation.Items.Add(colfunc.GetDisplayValue());
            }

            cmbCalculation.Sorted = true;

            if (cmbCalculation.Items.Count > 0)
            {
                cmbCalculation.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Reads the selected values from the controls,
        /// and stores them into the parameter system of FCalculator
        ///
        /// </summary>
        /// <param name="ACalculator"></param>
        /// <param name="AReportAction"></param>
        /// <returns>void</returns>
        public void ReadControls(TRptCalculator ACalculator, TReportActionEnum AReportAction)
        {
            System.Int32 MaxDisplayColumns;

            MaxDisplayColumns = TUC_ColumnHelper.ReadControls(ref FColumnParameters, ref ACalculator);

            FPetraUtilsObject.FMaxDisplayColumns = MaxDisplayColumns;
        }

        /// <summary>
        /// Sets the selected values in the controls, using the parameters loaded from a file
        ///
        /// </summary>
        /// <param name="AParameters"></param>
        /// <returns>void</returns>
        public void SetControls(TParameterList AParameters)
        {
            System.Int32 MaxDisplayColumns;

            MaxDisplayColumns = TUC_ColumnHelper.SetControls(ref FColumnParameters, ref AParameters);

            /* copy values for columns to the current set of parameters */

            FPetraUtilsObject.FMaxDisplayColumns = MaxDisplayColumns;

            FillColumnGrid();
        }

        #endregion

        #region event handler
        private void CmbContentChanged(System.Object sender, System.EventArgs e)
        {
            String SelectedFunction = cmbCalculation.GetSelectedString();

            foreach (TPartnerColumnFunction Func in FAvailableFunctions)
            {
                if (SelectedFunction == Func.GetDisplayValue())
                {
                    txtColumnWidth.Text = Func.FColumnWidth.ToString();
                }
            }
        }

        private void AddColumn(System.Object sender, System.EventArgs e)
        {
            AddColumn(FSelectedColumn);
            btnAddColumn.Enabled = false;
        }

        private void RemoveColumn(System.Object sender, System.EventArgs e)
        {
            // if the column was just added then btnAddColumn is disabled.
            // if user removes it, don't ask the user because he hasn't applied the new column yet.
            RemoveColumn(FSelectedColumn, btnAddColumn.Enabled);
        }

        private void MoveColumn2Left(System.Object sender, System.EventArgs e)
        {
            MoveColumn(FSelectedColumn, FSelectedColumn - 1);
        }

        private void MoveColumn2Right(System.Object sender, System.EventArgs e)
        {
            MoveColumn(FSelectedColumn, FSelectedColumn + 1);
        }

        private void ApplyColumn(System.Object sender, System.EventArgs e)
        {
            if (ApplyColumnAndUnselect(FSelectedColumn))
            {
                btnApply.Enabled = false;
                btnCancel.Enabled = false;
                btnAddColumn.Enabled = true;
            }
        }

        private void CancelColumn(System.Object sender, System.EventArgs e)
        {
            CancelColumn(FSelectedColumn);
            btnAddColumn.Enabled = true;
            btnCancel.Enabled = false;
            btnApply.Enabled = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ASender">sender</param>
        /// <param name="AEventArgs">event</param>
        protected void GrdColumns_CellLostFocus(SelectionBase ASender, ChangeActivePositionEventArgs AEventArgs)
        {
            System.Int32 newcolumn;
            newcolumn = AEventArgs.NewFocusPosition.Column;
            AEventArgs.Cancel = false;

            if ((FSelectedColumn != -1) && (newcolumn != -1) && (newcolumn != FSelectedColumn) && (!SelectColumn(-1)))
            {
                AEventArgs.Cancel = true;
            }
        }

        /// <summary>
        /// Select the new column.
        /// </summary>
        /// <param name="ASender">sender</param>
        /// <param name="AEventArgs">event</param>
        protected void GrdColumns_CellGotFocus(SelectionBase ASender, ChangeActivePositionEventArgs AEventArgs)
        {
            System.Int32 newcolumn;

            if (FDuringApplyOrCancel == true)
            {
                grdColumns.Selection.ResetSelection(false);
                return;
            }

            newcolumn = AEventArgs.NewFocusPosition.Column;
            AEventArgs.Cancel = false;

            if ((FSelectedColumn != -1) && (newcolumn != -1) && (newcolumn != FSelectedColumn) && (!SelectColumn(-1)))
            {
                AEventArgs.Cancel = true;
                grdColumns.Selection.SelectColumn(FSelectedColumn, true);
            }
        }

        /// <summary>
        /// Select the picked column.
        /// </summary>
        /// <param name="ASender">sender</param>
        /// <param name="AEventArgs">event</param>
        protected void GrdColumns_FocusColumnEntered(System.Object ASender, ColumnEventArgs AEventArgs)
        {
            System.Int32 column;

            if (FDuringApplyOrCancel == true)
            {
                grdColumns.Selection.ResetSelection(false);
                return;
            }

            column = AEventArgs.Column;

            if ((column != -1) && (column != FSelectedColumn))
            {
                /* select the picked column */
                SelectColumn(column);
            }
        }

        #endregion

        #region Column Choice Routines

        /// <summary>
        /// This procedure will compare the current settings of the column with the settings stored in FColumnParameters
        /// </summary>
        /// <returns>true if the column has changed, ie. the settings are different
        /// </returns>
        protected bool ColumnChanged(System.Int32 ASelectedColumn)
        {
            bool changed = false;

            if (ASelectedColumn == -1)
            {
                return false;
            }

            if (FColumnParameters.Get("param_calculation", ASelectedColumn).ToString() != cmbCalculation.GetSelectedString())
            {
                changed = true;
            }

            if (this.txtColumnWidth.Text != FColumnParameters.Get("ColumnWidth", ASelectedColumn).ToString())
            {
                changed = true;
            }

            return changed;
        }

        /// <summary>
        /// This procedure will check if there are no changes to the currently selected column,
        /// and then will select the new column
        ///
        /// </summary>
        /// <param name="ASelectedColumn">the number of the selected column (starting with 1)
        /// if it is -1, the edit fields are disabled</param>
        /// <returns>true if the selection was successful.
        /// it could fail if there are changed fields in the current selection
        /// </returns>
        protected bool SelectColumn(System.Int32 ASelectedColumn)
        {
            /* first check if currently selected column is unchanged; */
            /* has anything changed in the currently selected column? */
            if (ColumnChanged(FSelectedColumn))
            {
                if (!FDuringApplyOrCancel)
                {
                    switch (MessageBox.Show("Do you want to lose your changes to this column?", "Cancel column?",
                                MessageBoxButtons.YesNoCancel))
                    {
                        case System.Windows.Forms.DialogResult.No:
                            ApplyColumn(FSelectedColumn, false);
                            FillColumnGrid();
                            break;

                        case System.Windows.Forms.DialogResult.Cancel:
                            return false;

                        default:
                            break;
                    }
                }

                /* if the column was new, it should be removed again */
                /* the position of the column to be selected will change */
                /* this does not work, because ASelectedColumn is probably 1 */
            }

            if (ASelectedColumn > -1)
            {
                grdColumns.Selection.ResetSelection(false);

                grdColumns.Selection.SelectColumn(ASelectedColumn, true);
                this.btnRemoveColumn.Enabled = true;
                this.btnApply.Enabled = true;
                this.btnCancel.Enabled = true;
                this.btnAddColumn.Enabled = true; // should be able to add a column after the selected column
                int MaxColumns = FColumnParameters.Get("MaxDisplayColumns").ToInt();
                this.btnMoveColumn2Right.Enabled = ((ASelectedColumn + 1) < MaxColumns);
                this.btnMoveColumn2Left.Enabled = (ASelectedColumn > 0);

                String calculation =
                    FColumnParameters.GetOrDefault("param_calculation", ASelectedColumn, new TVariant(cmbCalculation.GetSelectedString())).ToString();
                TPartnerColumnFunction Func = (TPartnerColumnFunction)GetFunction(calculation);

                if (Func != null)
                {
                    cmbCalculation.SetSelectedString(Func.GetDisplayValue());
                    txtColumnWidth.Text = FColumnParameters.GetOrDefault("ColumnWidth", ASelectedColumn, new TVariant(Func.FColumnWidth)).ToString();
                }

                cmbCalculation.Enabled = true;
                txtColumnWidth.Enabled = true;
            }
            else
            {
                /* unselect the column */

                if (FSelectedColumn != -1)
                {
                    /* grdColumns.Selection.SelectColumn(FSelectedColumn, false); */
                    grdColumns.Selection.ResetSelection(false);
                    btnAddColumn.Focus();
                }

                this.btnRemoveColumn.Enabled = false;
                this.btnApply.Enabled = false;
                this.btnCancel.Enabled = false;
                this.btnAddColumn.Enabled = true;
                this.btnMoveColumn2Right.Enabled = false;
                this.btnMoveColumn2Left.Enabled = false;

                cmbCalculation.Enabled = false;
                txtColumnWidth.Enabled = false;
            }

            this.FSelectedColumn = ASelectedColumn;
            return true;
        }

        /// <summary>
        /// This procedure will add a new column;
        /// it will check if the currently selected column can be unselected;
        /// the new column is selected
        /// </summary>
        /// <param name="ASelectedColumn">the new column should be inserted after this column
        /// if it is -1, the column will be added at the right</param>
        /// <returns>true if a new column was added
        /// </returns>
        protected bool AddColumn(System.Int32 ASelectedColumn)
        {
            bool ReturnValue;

            System.Int32 NewColumn;
            ReturnValue = false;

            if (SelectColumn(-1))
            {
                NewColumn = TUC_ColumnHelper.AddColumn(ref FColumnParameters, ASelectedColumn);

                FillColumnGrid();
                SelectColumn(NewColumn);
                ApplyColumn(NewColumn, false);
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This procedure will apply the current settings of the column (calling ApplyColumn),
        /// and then will unselect the column
        ///
        /// </summary>
        /// <returns>void</returns>
        protected bool ApplyColumnAndUnselect(System.Int32 ASelectedColumn)
        {
            bool ReturnValue = false;

            if (ApplyColumn(FSelectedColumn, true))
            {
                FDuringApplyOrCancel = true;
                FillColumnGrid();
                SelectColumn(-1);
                FDuringApplyOrCancel = false;
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This procedure will apply the current settings of the column,
        /// and then will unselect the column
        ///
        /// </summary>
        /// <returns>void</returns>
        protected bool ApplyColumn(System.Int32 ASelectedColumn, bool ACheckForDoubleEntries)
        {
            bool ReturnValue;

            ReturnValue = false;
            double ColumnWidth = 2.0;

            if (!ACheckForDoubleEntries
                || TUC_ColumnHelper.CheckAddDoubleEntry(ref FColumnParameters, this.cmbCalculation.GetSelectedString(0), ASelectedColumn))
            {
                try
                {
                    ColumnWidth = Convert.ToDouble(txtColumnWidth.Text);
                    FColumnParameters.Add("param_calculation", new TVariant(cmbCalculation.GetSelectedString()), ASelectedColumn);
                    FColumnParameters.Add("ColumnWidth", new TVariant(ColumnWidth), ASelectedColumn);
                    ReturnValue = true;
                }
                catch (Exception e)
                {
                    if (e.GetType().ToString() == "System.FormatException")
                    {
                        MessageBox.Show("Please insert a correct number value for the column width", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    ReturnValue = false;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// This procedure will undo the current settings of the column,
        /// and then will unselect the column
        ///
        /// </summary>
        /// <returns>void</returns>
        protected virtual void CancelColumn(System.Int32 ASelectedColumn)
        {
            if (!btnAddColumn.Enabled)
            {
                // this column was just added. So we remove it again
                RemoveColumn(ASelectedColumn, false);
            }

            FDuringApplyOrCancel = true;
            SelectColumn(-1);
            FDuringApplyOrCancel = false;
        }

        /// <summary>
        /// This procedure will remove the currently selected column,
        /// and after that no column is selected
        ///
        /// </summary>
        /// <returns>void</returns>
        protected bool RemoveColumn(System.Int32 ASelectedColumn, bool AAskBeforeRemove)
        {
            bool ReturnValue;

            System.Int32 NewMaxColumn;
            ReturnValue = false;

            if ((!AAskBeforeRemove)
                || (MessageBox.Show("Do you really want to delete this column?", "Really delete?",
                        MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                NewMaxColumn = TUC_ColumnHelper.RemoveColumn(ref FColumnParameters, ASelectedColumn);

                /* need to move the following columns to the left */

                FPetraUtilsObject.FMaxDisplayColumns = NewMaxColumn;

                FillColumnGrid();
                FSelectedColumn = -1;
                SelectColumn(-1);
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// This procedure will switch the two columns
        /// and after that the column in the new position is selected
        ///
        /// </summary>
        /// <returns>void</returns>
        protected virtual void MoveColumn(System.Int32 ASelectedColumn, System.Int32 ANewColumnPosition)
        {
            if ((ANewColumnPosition > -1) && (ANewColumnPosition < FColumnParameters.Get("MaxDisplayColumns").ToInt()))
            {
                if (SelectColumn(-1))
                {
                    TUC_ColumnHelper.SwitchColumn(ref FColumnParameters, ASelectedColumn, ANewColumnPosition);

                    FillColumnGrid();
                    SelectColumn(ANewColumnPosition);
                }
            }
        }

        #endregion

        /// <summary>
        /// Fills the column choice datagrid with the current values,
        /// that are stored in the local column parameters
        ///
        /// </summary>
        /// <returns>void</returns>
        public void FillColumnGrid()
        {
            DataTable columnTab;
            DataRow rowContent;
            DataRow rowDisplayWidth;

            System.Int32 counter;
            String calculation;
            System.Int32 rowCounter;
            TPartnerColumnFunction Func;

            columnTab = new System.Data.DataTable();

            /* create columns */
            for (counter = 0; counter <= FColumnParameters.Get("MaxDisplayColumns").ToInt() - 1; counter += 1)
            {
                columnTab.Columns.Add("Column " + Convert.ToString(counter + 1));
            }

            /* first row: name of calculation */
            rowContent = columnTab.NewRow();

            /* second row: display width of the column */
            rowDisplayWidth = columnTab.NewRow();

            for (counter = 0; counter <= FColumnParameters.Get("MaxDisplayColumns").ToInt() - 1; counter += 1)
            {
                calculation = FColumnParameters.GetOrDefault("param_calculation", counter, new TVariant(cmbCalculation.GetSelectedString())).ToString();
                Func = (TPartnerColumnFunction)GetFunction(calculation, FColumnParameters, counter);

                if (Func != null)
                {
                    rowContent[counter] = Func.GetDisplayValue();
                    rowDisplayWidth[counter] =
                        FColumnParameters.GetOrDefault("ColumnWidth", counter, new TVariant(Func.FColumnWidth)).ToString() + " cm";
                }
            }

            rowCounter = 0;
            columnTab.Rows.InsertAt(rowContent, rowCounter);
            rowCounter = rowCounter + 1;
            columnTab.Rows.InsertAt(rowDisplayWidth, rowCounter);

            TUC_ColumnHelper.LoadDataToGrid(ref grdColumns, ref columnTab);
        }

        /// <summary>
        /// get the function object of the given calculation string
        /// </summary>
        /// <returns>nil if the function cannot be found
        /// </returns>
        protected TColumnFunction GetFunction(String calculation)
        {
            return TUC_ColumnHelper.GetFunction(ref FAvailableFunctions, calculation);
        }

        /// <summary>
        /// get the function object of the given calculation string
        /// </summary>
        /// <returns>nil if the function cannot be found
        /// </returns>
        protected TColumnFunction GetFunction(String calculation, TParameterList AParameterList, int AColumnNumber)
        {
            return TUC_ColumnHelper.GetFunction(ref FAvailableFunctions, calculation, AParameterList, AColumnNumber);
        }

        /// get the content and the width for the columns
        public List <KeyValuePair <String, Double>>GetColumnHeadings()
        {
            List <KeyValuePair <String, Double>>ReturnValue = new List <KeyValuePair <String, Double>>();

            System.Int32 MaxDisplayColumns;

            MaxDisplayColumns = FColumnParameters.Get("MaxDisplayColumns").ToInt();

            for (int Counter = 0; Counter < MaxDisplayColumns; Counter += 1)
            {
                ReturnValue.Add(new KeyValuePair <String, Double>
                        (FColumnParameters.Get("param_calculation", Counter).ToString(),
                        FColumnParameters.Get("ColumnWidth", Counter).ToDouble()));
            }

            return ReturnValue;
        }
    }
}