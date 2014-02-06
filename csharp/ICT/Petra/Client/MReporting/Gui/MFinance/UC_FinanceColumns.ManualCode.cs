//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MReporting;


namespace Ict.Petra.Client.MReporting.Gui.MFinance
{
    /// <summary>
    /// Description of TFrmUC_FinanceColumns.
    /// </summary>
    public partial class TFrmUC_FinanceColumns
    {
        private ArrayList FAvailableFunctions;

        /// <summary>this holds the currently configured columns</summary>
        protected TParameterList FColumnParameters;

        /// <summary>this shows which column is currently selected; it is 1 if no column is selected</summary>
        protected int FSelectedColumn;

        /// <summary>helper variable to unselect the column in the grid after cancel or apply</summary>
        private bool FDuringApplyOrCancel;

        /// <summary>Private Declarations  needed to prevent endless recursion, when updating the column comboboxes</summary>
        private bool FUpdatingRelationColumns;


        /// <summary>
        /// Initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FUpdatingRelationColumns = false;

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
            rbtFromGL.Checked = true;
            rbtFromGL.Enabled = false;
            rbtCalculateExisting.Enabled = false;
            cmbYearSelection.Enabled = false;
            chkYTD.Enabled = false;
            clbLedger.Enabled = false;
        }

        /// <summary>
        /// Show / Hide the YTD CheckBox and Ledger Selection Grid
        /// </summary>
        /// <param name="AShowYTDBox"></param>
        /// <param name="AShowLedgerGrid"></param>
        public void SetVisibility(bool AShowYTDBox, bool AShowLedgerGrid)
        {
            chkYTD.Visible = AShowYTDBox;
            clbLedger.Visible = AShowLedgerGrid;
        }

        #region Parameter/Settings Handling

        /// <summary>
        /// Sets the available functions (fields) that can be used for this report.
        /// </summary>
        /// <param name="AAvailableFunctions">List of TColumnFunction</param>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
            FAvailableFunctions = AAvailableFunctions;
            StringCollection AvailableCalculations;
            StringCollection AvailableRelations;

            /* initialise the 2 comboboxes, with the calculations and functions */
            AvailableCalculations = new StringCollection();
            AvailableRelations = new StringCollection();

            foreach (TColumnFunction Func in FAvailableFunctions)
            {
                if (Func.FNumberColumns == 0)
                {
                    AvailableCalculations.Add(Func.FDescription);
                }
                else
                {
                    AvailableRelations.Add(Func.FDescription);
                }
            }

            cmbYearSelection.SetDataSourceStringList(AvailableCalculations);
            cmbColumnRelation.SetDataSourceStringList(AvailableRelations);

            if (AvailableCalculations.Count > 0)
            {
                cmbYearSelection.SelectedIndex = 0;
            }

            if (AvailableRelations.Count > 0)
            {
                cmbColumnRelation.SelectedIndex = 0;
            }

            /* load available ledgers into listbox */
            DataTable LedgerTable;

            TRemote.MFinance.Cacheable.WebConnectors.RefreshCacheableTable(TCacheableFinanceTablesEnum.LedgerNameList, out LedgerTable);

            LedgerTable.Columns.Add("Selection", Type.GetType("System.Boolean"));

            foreach (DataRow Row in LedgerTable.Rows)
            {
                Row["Selection"] = false;
            }

            clbLedger.SelectionMode = GridSelectionMode.Cell;
            clbLedger.Columns.Clear();
            clbLedger.AddCheckBoxColumn("", LedgerTable.Columns["Selection"], 17, false);
            clbLedger.AddTextColumn(Catalog.GetString("Code"), LedgerTable.Columns["LedgerNumber"], 40);
            clbLedger.AddTextColumn(Catalog.GetString("Ledger Name"), LedgerTable.Columns["LedgerName"], 200);
            clbLedger.DataBindGrid(LedgerTable, "LedgerNumber", "Selection", "LedgerNumber", false, true, false);
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

            for (int Counter = 0; Counter <= FColumnParameters.Get("MaxDisplayColumns").ToInt() - 1; Counter += 1)
            {
                String SelectedLedgers = FColumnParameters.Get("param_selected_ledgers", Counter).ToString(false);

                if (SelectedLedgers.Length != 0)
                {
                    ACalculator.AddColumnFunctionLedgers(Counter, "add",
                        StringHelper.StrSplit(SelectedLedgers, ","),
                        FColumnParameters.Get("param_calculation", Counter).ToString(), FColumnParameters.Get("param_ytd", Counter).ToBool());
                }
            }

            // set the global param_ytd; that is needed for formatting the header of some reports
            String ytdMixed = "";

            for (int Counter = 0; Counter <= FColumnParameters.Get("MaxDisplayColumns").ToInt() - 1; ++Counter)
            {
                TVariant ParamYtd = FColumnParameters.Get("param_ytd", Counter);

                if (!ParamYtd.IsZeroOrNull())
                {
                    if (ytdMixed.Length == 0)
                    {
                        ytdMixed = ParamYtd.ToString();
                    }

                    if (ParamYtd.ToString() != ytdMixed)
                    {
                        ytdMixed = "mixed";
                    }
                }
            }

            if (ytdMixed.Length != 0)
            {
                ACalculator.AddParameter("param_ytd", ytdMixed);
            }
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

            chkYTD.Checked = AParameters.Get("param_ytd").ToBool();

            FillColumnGrid();
        }

        #endregion

        #region event handler

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

        private void rbtColumTypeChanged(System.Object sender, System.EventArgs e)
        {
            clbLedger.Enabled = rbtFromGL.Checked;
            chkYTD.Enabled = rbtFromGL.Checked;
            cmbYearSelection.Enabled = rbtFromGL.Checked;

            cmbColumnRelation.Enabled = rbtCalculateExisting.Checked;
            cmbColumnSelection1.Enabled = rbtCalculateExisting.Checked;
            cmbColumnSelection2.Enabled = rbtCalculateExisting.Checked;
        }

        private void cmbYearSelection_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            UpdateCalculationDetails(cmbYearSelection.GetSelectedString(), FSelectedColumn);
        }

        private void cmbColumnSelection_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            UpdateRelationColumns(cmbColumnRelation.GetSelectedString(), cmbColumnSelection1.GetSelectedInt32(), cmbColumnSelection2.GetSelectedInt32());
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
        /// This procedure sets the values of the two comboboxes, that define the columns for the variance etc.
        /// It will only allow reasonable columns for selection.
        ///
        /// </summary>
        /// <param name="ACalculation">the currently selected relation between up to 2 columns</param>
        /// <param name="AColumn1">the number of the first column, starting with 1; if -1, there is no selection</param>
        /// <param name="AColumn2">the number of the second column, starting with 1; if -1, there is no selection
        /// </param>
        /// <returns>void</returns>
        protected void UpdateRelationColumns(String ACalculation, System.Int32 AColumn1, System.Int32 AColumn2)
        {
            System.Int32 Counter;
            String Calculation;
            DataTable Table1;
            DataTable Table2;
            DataRow Row;
            TColumnFunction Func;

            /* prevent recursion, when selected index is changed on the column comboboxes */
            if (FUpdatingRelationColumns)
            {
                return;
            }

            if (FColumnParameters == null)
            {
                // during initialisation we don't have any columns
                return;
            }

            FUpdatingRelationColumns = true;
            Func = GetFunction(ACalculation);
            cmbColumnSelection1.Enabled = (Func != null) && (Func.FNumberColumns >= 1);
            cmbColumnSelection2.Enabled = (Func != null) && (Func.FNumberColumns >= 2);

            if (!cmbColumnSelection1.Enabled)
            {
                AColumn1 = -1;
            }

            if (!cmbColumnSelection2.Enabled)
            {
                AColumn2 = -1;
            }

            Table1 = new DataTable();
            Table2 = new DataTable();
            Table1.Columns.Add("display", typeof(String));
            Table1.Columns.Add("value", typeof(System.Int32));
            Table2.Columns.Add("display", typeof(String));
            Table2.Columns.Add("value", typeof(System.Int32));

            for (Counter = 0; Counter <= FColumnParameters.Get("MaxDisplayColumns").ToInt() - 1; Counter += 1)
            {
                /* don't add the current column */
                if (Counter != FSelectedColumn)
                {
                    Calculation = FColumnParameters.Get("param_calculation", Counter).ToString();

                    /* don't add the column that is selected in the other combobox */
                    if (AColumn2 != Counter)
                    {
                        Row = Table1.NewRow();
                        Row["display"] = "Column " + Convert.ToString(Counter + 1) + " " + Calculation;
                        Row["value"] = Counter;
                        Table1.Rows.InsertAt(Row, Table1.Rows.Count);
                    }

                    if (AColumn1 != Counter)
                    {
                        Row = Table2.NewRow();
                        Row["display"] = "Column " + Convert.ToString(Counter + 1) + " " + Calculation;
                        Row["value"] = Counter;
                        Table2.Rows.InsertAt(Row, Table1.Rows.Count);
                    }
                }
            }

            cmbColumnSelection1.BeginUpdate();
            cmbColumnSelection1.DisplayMember = "display";
            cmbColumnSelection1.ValueMember = "value";
            cmbColumnSelection1.DataSource = new DataView(Table1);
            cmbColumnSelection1.EndUpdate();
            cmbColumnSelection2.BeginUpdate();
            cmbColumnSelection2.DisplayMember = "display";
            cmbColumnSelection2.ValueMember = "value";
            cmbColumnSelection2.DataSource = new DataView(Table2);
            cmbColumnSelection2.EndUpdate();
            cmbColumnSelection1.SetSelectedInt32(AColumn1);
            cmbColumnSelection2.SetSelectedInt32(AColumn2);
            FUpdatingRelationColumns = false;
        }

        /// <summary>
        /// This will enable or disable the ytd checkbox, depending on the selected calculation
        /// (e.g. "Actual End of Year" is always ytd)
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void UpdateCalculationDetails(String ACalculation, int ASelectedColumn)
        {
            TFinanceColumnFunction CurrentFunction;

            CurrentFunction = (TFinanceColumnFunction)GetFunction(ACalculation);

            if (CurrentFunction == null)
            {
                return;
            }

            chkYTD.Enabled = CurrentFunction.FDisplayYTD;

            if (!CurrentFunction.FDisplayYTD)
            {
                chkYTD.Checked = CurrentFunction.FDefaultYTD;
            }
            else
            {
                if ((FColumnParameters != null) && (ASelectedColumn >= 0))
                {
                    chkYTD.Checked = FColumnParameters.GetOrDefault("param_ytd", ASelectedColumn, new TVariant(CurrentFunction.FDefaultYTD)).ToBool();
                }
            }
        }

        /// <summary>
        /// This procedure will compare the current settings of the column with the settings stored in FColumnParameters
        /// </summary>
        /// <returns>true if the column has changed, ie. the settings are different
        /// </returns>
        protected bool ColumnChanged(System.Int32 ASelectedColumn)
        {
            bool Changed = false;
            String Calculation;

            System.Int32 Column1;
            System.Int32 Column2;
            TColumnFunction Func;

            if (ASelectedColumn == -1)
            {
                return false;
            }

            /* hint: check what happens in ApplyButtons */
            if (rbtFromGL.Checked)
            {
                if (FColumnParameters.Get("param_calculation", ASelectedColumn).ToString() != cmbYearSelection.GetSelectedString())
                {
                    Changed = true;
                }
                else if (chkYTD.Visible && (chkYTD.Checked != FColumnParameters.Get("param_ytd", ASelectedColumn).ToBool()))
                {
                    Changed = true;
                }

                if (clbLedger.Visible
                    && (clbLedger.GetCheckedStringList() != FColumnParameters.Get("param_selected_ledgers", ASelectedColumn).ToString(false)))
                {
                    Changed = true;
                }
            }
            else
            {
                Calculation = cmbColumnRelation.GetSelectedString();
                Column1 = cmbColumnSelection1.GetSelectedInt32();
                Column2 = cmbColumnSelection2.GetSelectedInt32();
                Func = GetFunction(Calculation);

                if (Func != null)
                {
                    if ((Func.FNumberColumns > 0) && (Column1 != FColumnParameters.Get("FirstColumn", ASelectedColumn).ToInt()))
                    {
                        Changed = true;
                    }

                    if ((Func.FNumberColumns > 1) && (Column2 != FColumnParameters.Get("SecondColumn", ASelectedColumn).ToInt()))
                    {
                        Changed = true;
                    }
                }

                if (FColumnParameters.Get("param_calculation", ASelectedColumn).ToString() != Calculation)
                {
                    Changed = true;
                }
            }

            return Changed;
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

                rbtFromGL.Enabled = true;
                rbtCalculateExisting.Enabled = true;

                String Calculation =
                    FColumnParameters.GetOrDefault("param_calculation", ASelectedColumn, new TVariant(cmbYearSelection.GetSelectedString())).ToString();

                //TFinanceColumnFunction Func = (TFinanceColumnFunction)GetFunction(Calculation);

                if (cmbYearSelection.FindString(Calculation) != -1)
                {
                    rbtFromGL.Checked = true;
                    clbLedger.Enabled = true;
                    cmbYearSelection.Enabled = true;
                    chkYTD.Enabled = true;

                    /* empty the selected columns */
                    UpdateRelationColumns(Calculation, -1, -1);
                    UpdateCalculationDetails(Calculation, ASelectedColumn);
                    cmbYearSelection.SetSelectedString(Calculation);

                    if (clbLedger.Visible == true)
                    {
                        clbLedger.SetCheckedStringList(FColumnParameters.GetOrDefault("param_selected_ledgers", ASelectedColumn,
                                new TVariant(FColumnParameters.Get("param_ledger_number_i").ToString(false))).ToString());
                    }
                }
                else
                {
                    int Column1 = FColumnParameters.Get("FirstColumn", ASelectedColumn).ToInt();
                    int Column2 = FColumnParameters.Get("SecondColumn", ASelectedColumn).ToInt();
                    rbtCalculateExisting.Checked = true;
                    cmbColumnRelation.Enabled = true;

                    /* update the available columns; has to be done after CB_Relation.Enabled, because that triggers UpdateRelationColumns again */
                    UpdateRelationColumns(Calculation, Column1, Column2);
                    cmbColumnRelation.SetSelectedString(Calculation);

                    if (clbLedger.Visible == true)
                    {
                        clbLedger.ClearSelected();
                    }
                }
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

                rbtFromGL.Enabled = false;
                rbtCalculateExisting.Enabled = false;

                if (clbLedger.Visible == true)
                {
                    clbLedger.SetCheckedStringList("");
                    clbLedger.Enabled = false;
                }

                cmbYearSelection.Enabled = false;
                chkYTD.Enabled = false;
                cmbColumnRelation.Enabled = false;
                cmbColumnSelection1.Enabled = false;
                cmbColumnSelection2.Enabled = false;
            }

            FSelectedColumn = ASelectedColumn;
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
            String Calculation;

            System.Int32 Column1;
            System.Int32 Column2;
            TColumnFunction Func;
            ReturnValue = false;

            if (rbtFromGL.Checked)
            {
                Calculation = cmbYearSelection.GetSelectedString(0);
            }
            else
            {
                Calculation = cmbColumnRelation.GetSelectedString(0);
            }

            if (!ACheckForDoubleEntries
                || TUC_ColumnHelper.CheckAddDoubleEntry(ref FColumnParameters, Calculation, ASelectedColumn))
            {
                if (rbtFromGL.Checked)
                {
                    FColumnParameters.Add("param_calculation", new TVariant(cmbYearSelection.GetSelectedString(0)), ASelectedColumn);

                    if (chkYTD.Visible)
                    {
                        FColumnParameters.Add("param_ytd", new TVariant(chkYTD.Checked), ASelectedColumn);
                    }

                    /* only add selected ledgers, if the list box is visible (multiledger screen) */
                    if (clbLedger.Visible)
                    {
                        FColumnParameters.Add("param_selected_ledgers", new TVariant(clbLedger.GetCheckedStringList()), ASelectedColumn);
                    }
                }
                else
                {
                    Calculation = cmbColumnRelation.GetSelectedString();
                    Column1 = cmbColumnSelection1.GetSelectedInt32();
                    Column2 = cmbColumnSelection2.GetSelectedInt32();
                    Func = GetFunction(Calculation);
                    FColumnParameters.Add("param_calculation", new TVariant(Calculation), ASelectedColumn);

                    if (Func != null)
                    {
                        if (Func.FNumberColumns > 0)
                        {
                            FColumnParameters.Add("FirstColumn", new TVariant(Column1), ASelectedColumn);
                        }

                        if (Func.FNumberColumns > 1)
                        {
                            FColumnParameters.Add("SecondColumn", new TVariant(Column2), ASelectedColumn);
                        }
                    }

                    FColumnParameters.Add("param_ytd", new TVariant(), ASelectedColumn);
                    FColumnParameters.RemoveVariable("param_selected_ledgers", ASelectedColumn);
                }

                ReturnValue = true;
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
            DataTable ColumnTab;
            DataRow RowContent;
            DataRow RowAppliesTo;
            DataRow RowYTD;

            String AppliesTo;

            System.Int32 Column1;
            System.Int32 Column2;
            bool ShowAppliesTo;
            System.Int32 RowCounter;
            TVariant Ytd;

            ColumnTab = new System.Data.DataTable();

            /* create columns */
            for (int Counter = 0; Counter <= FColumnParameters.Get("MaxDisplayColumns").ToInt() - 1; Counter += 1)
            {
                ColumnTab.Columns.Add("Column " + Convert.ToString(Counter + 1));
            }

            /* first row: name of calculation */
            RowContent = ColumnTab.NewRow();

            /* second row: if necessary with number of involved columns */
            RowAppliesTo = ColumnTab.NewRow();

            /* third row: Ytd */
            RowYTD = ColumnTab.NewRow();
            ShowAppliesTo = false;

            for (int Counter = 0; Counter <= FColumnParameters.Get("MaxDisplayColumns").ToInt() - 1; Counter += 1)
            {
                String Calculation =
                    FColumnParameters.GetOrDefault("param_calculation", Counter, new TVariant(cmbColumnRelation.GetSelectedString())).ToString();
                AppliesTo = "";

                if (cmbColumnRelation.FindString(Calculation) != -1)
                {
                    Column1 = FColumnParameters.Get("FirstColumn", Counter).ToInt();
                    Column2 = FColumnParameters.Get("SecondColumn", Counter).ToInt();
                    AppliesTo = AppliesTo + " (Column " + Convert.ToString(Column1 + 1);

                    if (Column2 != -1)
                    {
                        AppliesTo = AppliesTo + " and " + Convert.ToString(Column2 + 1);
                    }

                    AppliesTo = AppliesTo + ")";
                    ShowAppliesTo = true;
                }
                else if (FColumnParameters.Exists("param_selected_ledgers", Counter))
                {
                    AppliesTo = AppliesTo + FColumnParameters.Get("param_selected_ledgers", Counter).ToString();
                    ShowAppliesTo = true;
                }

                RowAppliesTo[Counter] = AppliesTo;
                RowContent[Counter] = Calculation;

                /* YTD */
                Ytd = FColumnParameters.Get("param_ytd", Counter);

                if (Ytd.IsZeroOrNull())
                {
                    RowYTD[Counter] = "";
                }
                else
                {
                    if (Ytd.ToBool())
                    {
                        RowYTD[Counter] = "YTD";
                    }
                    else
                    {
                        RowYTD[Counter] = "non-YTD";
                    }
                }
            }

            RowCounter = 0;
            ColumnTab.Rows.InsertAt(RowContent, RowCounter);

            if (ShowAppliesTo)
            {
                RowCounter = RowCounter + 1;
                ColumnTab.Rows.InsertAt(RowAppliesTo, RowCounter);
            }

            RowCounter = RowCounter + 1;
            ColumnTab.Rows.InsertAt(RowYTD, RowCounter);
            grdColumns.Columns.Clear();

            for (int Counter = 0; Counter <= FColumnParameters.Get("MaxDisplayColumns").ToInt() - 1; Counter += 1)
            {
                grdColumns.AddTextColumn("Column " + Convert.ToString(Counter + 1), ColumnTab.Columns[Counter]);
            }

            grdColumns.DataSource = new DevAge.ComponentModel.BoundDataView(new DataView(ColumnTab));
            grdColumns.DataSource.AllowEdit = false;
            grdColumns.DataSource.AllowNew = false;
            grdColumns.DataSource.AllowDelete = false;
            grdColumns.AutoSizeCells();

            TUC_ColumnHelper.LoadDataToGrid(ref grdColumns, ref ColumnTab);
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