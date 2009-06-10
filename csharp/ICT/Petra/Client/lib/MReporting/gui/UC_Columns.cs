/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using SourceGrid;
using SourceGrid.Selection;
using Mono.Unix;
using Ict.Petra.Shared.MReporting;
using Ict.Common.Controls;
using Ict.Common;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MReporting.Gui
{
	/// <summary>
	/// used to tell the main form to fill the column grid
	/// </summary>
    public delegate void TFillColumnGridEventHandler(System.Object Sender);
    
    /// <summary>
    /// A control that offers managing the columns and calculations per column for the output of a report
    /// </summary>
    public partial class UC_Columns : UserControl
    {
        /// List of functions between columns, that are available for this report; is set by SetAvailableFunctions
        protected ArrayList FAvailableFunctions = null;
        
        /// this shows which column is currently selected; it is 1 if no column is selected
        protected int FSelectedColumn;

        /// helper variable to unselect the column in the grid after cancel or apply
        private bool FDuringApplyOrCancel;
        
        /// <summary>
        /// constructor
        /// </summary>
        public UC_Columns()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
#region CATALOGI18N
// this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.GBx_ChooseColCont.Text = Catalog.GetString("Define Column");
            this.Btn_Cancel.Text = Catalog.GetString("Cancel");
            this.BtnApply.Text = Catalog.GetString("Apply");
            this.Btn_RemoveColumn.Text = Catalog.GetString("&Remove");
            this.Btn_AddColumn.Text = Catalog.GetString("&Add");
#endregion
            
            FSelectedColumn = -1;
            FDuringApplyOrCancel = false;
            
            grdColumns.AlternatingBackgroundColour = Color.Empty;
            grdColumns.SortableHeaders = false;
            grdColumns.SelectionMode = SourceGrid.GridSelectionMode.Column;
            grdColumns.AutoStretchColumnsToFitWidth = false;

            // Hook up event that fires when a different row is selected 
            grdColumns.Selection.FocusColumnEntered += new ColumnEventHandler(this.GrdColumns_FocusColumnEntered);
            grdColumns.Selection.CellLostFocus += new ChangeActivePositionEventHandler(this.GrdColumns_CellLostFocus);
            grdColumns.Selection.CellGotFocus += new ChangeActivePositionEventHandler(this.GrdColumns_CellGotFocus);
        }

        private TFrmPetraUtils FPetraUtilsObject;
        /// <summary>
        /// utilities for Petra forms
        /// </summary>
        public TFrmPetraUtils PetraUtilsObject
        {
        	get
        	{
        		return FPetraUtilsObject;
        	}
        	set
        	{
        		FPetraUtilsObject = value;
        	}
        }
        
        /// <summary>
        /// the current list of parameters
        /// </summary>
        protected TParameterList FColumnParameters;
        
        /// <summary>
        /// init the control with the parameters
        /// </summary>
        /// <param name="AColumnParameters"></param>
        public void InitialiseData(TParameterList AColumnParameters)
        {
            FColumnParameters = AColumnParameters;
            InitialiseFunctions();
            SelectedColumn = -1;
        }

        /// Custom Event that can be triggered in order for the column grid to be filled by the main reporting form
        public event TFillColumnGridEventHandler FillColumnGridEventHandler;

        /**
         * Raises Event FillColumnGrid.
         */
        protected void OnFillColumnGrid()
        {
            if (FillColumnGridEventHandler != null)
            {
                FillColumnGridEventHandler(this);
            }
        }
        
        private void GrdColumns_CellLostFocus(SelectionBase ASender, ChangeActivePositionEventArgs AEventArgs)
        {
            System.Int32 newcolumn;
            newcolumn = AEventArgs.NewFocusPosition.Column;
            AEventArgs.Cancel = false;

            if ((FSelectedColumn != -1) && (newcolumn != -1) && (newcolumn != FSelectedColumn) && (!SelectColumn(-1)))
            {
                AEventArgs.Cancel = true;
            }
        }

        private void GrdColumns_CellGotFocus(SelectionBase ASender, ChangeActivePositionEventArgs AEventArgs)
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

        private void GrdColumns_FocusColumnEntered(System.Object ASender, ColumnEventArgs AEventArgs)
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
                // select the picked column 
                SelectColumn(column);
            }
        }

        private void Btn_RemoveColumn_Click(System.Object sender, System.EventArgs e)
        {
            RemoveColumn(FSelectedColumn);
        }

        private void Btn_AddColumn_Click(System.Object sender, System.EventArgs e)
        {
            AddColumn(FSelectedColumn);
        }

        private void BtnApply_Click(System.Object sender, System.EventArgs e)
        {
            ApplyColumnAndUnselect(FSelectedColumn);
        }

        private void Btn_Cancel_Click(System.Object sender, System.EventArgs e)
        {
            CancelColumn(FSelectedColumn);
        }

        private void Btn_MoveColumn2Left_Click(System.Object sender, System.EventArgs e)
        {
            MoveColumn(FSelectedColumn, FSelectedColumn - 1);
        }

        private void Btn_MoveColumn2Right_Click(System.Object sender, System.EventArgs e)
        {
            MoveColumn(FSelectedColumn, FSelectedColumn + 1);
        }

        #region Column Choice Routines

        /**
         * This procedure will check if there are no changes to the currently selected column,
         * and then will select the new column
         *
         * @param ASelectedColumn the number of the selected column (starting with 1)
         * if it is -1, the edit fields are disabled
         * @return true if the selection was successful.
         * it could fail if there are changed fields in the current selection
         *
         */
        public virtual bool SelectColumn(System.Int32 ASelectedColumn)
        {
            // first check if currently selected column is unchanged; 
            // this check needs to happen in a derived class 
            // select column 
            if (ASelectedColumn > -1)
            {
                grdColumns.Selection.ResetSelection(false);
                grdColumns.Selection.SelectColumn(ASelectedColumn, true);
                this.Btn_RemoveColumn.Enabled = true;
                this.BtnApply.Enabled = true;
                this.Btn_Cancel.Enabled = true;
                // Btn_AddColumn: should be able to add a column after the selected column
                this.Btn_AddColumn.Enabled = true;
                this.Btn_MoveColumn2Right.Enabled = (ASelectedColumn < -1);
                this.Btn_MoveColumn2Left.Enabled = (ASelectedColumn > 0);
            }
            else

            // unselect the column 
            {
                if (FSelectedColumn != -1)
                {
                    // grdColumns.Selection.SelectColumn(FSelectedColumn, false); 
                    grdColumns.Selection.ResetSelection(false);
                    Btn_AddColumn.Focus();
                }

                this.Btn_RemoveColumn.Enabled = false;
                this.BtnApply.Enabled = false;
                this.Btn_Cancel.Enabled = false;
                this.Btn_AddColumn.Enabled = true;
                this.Btn_MoveColumn2Right.Enabled = false;
                this.Btn_MoveColumn2Left.Enabled = false;
            }

            this.FSelectedColumn = ASelectedColumn;
            return true;
        }

        /**
         * This procedure will add a new column;
         * it will check if the currently selected column can be unselected;
         * the new column is selected
         * @param ASelectedColumn the new column should be inserted after this column
         * if it is -1, the column will be added at the right
         * @return true if a new column was added
         *
         */
        protected bool AddColumn(System.Int32 ASelectedColumn)
        {
            bool ReturnValue;

            System.Int32 NewColumn;
            System.Int32 NewMaxColumn;
            System.Int32 Counter;
            ReturnValue = false;

            if (ColumnChanged(ASelectedColumn))
            {
                MessageBox.Show("First apply or cancel the current column. Then you can add a new column!", "Adding a column not possible");
                this.grdColumns.Size = new System.Drawing.Size(this.grdColumns.Width, 120);
                return false;
            }

            if (SelectColumn(-1))
            {
                if (ASelectedColumn == -1)
                {
                	NewMaxColumn = FColumnParameters.Get("MaxDisplayColumns").ToInt();
                    NewColumn = NewMaxColumn;
                    FColumnParameters.Add("MaxDisplayColumns", NewColumn + 1);
                }
                else
                {
                    NewColumn = ASelectedColumn + 1;
                    NewMaxColumn = FColumnParameters.Get("MaxDisplayColumns").ToInt() + 1;
                    FColumnParameters.Add("MaxDisplayColumns", NewMaxColumn);

                    // need to move the columns to the right 
                    for (Counter = NewMaxColumn - 1; Counter <= NewColumn + 1; Counter -= 1)
                    {
                        FColumnParameters.MoveColumn(Counter - 1, Counter);
                    }
                }

                OnFillColumnGrid();
                SelectColumn(NewColumn);
                ApplyColumn(NewColumn);
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /**
         * This procedure will apply the current settings of the column (calling ApplyColumn),
         * and then will unselect the column
         *
         */
        protected void ApplyColumnAndUnselect(System.Int32 ASelectedColumn)
        {
            if (ApplyColumn(FSelectedColumn))
            {
                FDuringApplyOrCancel = true;
                OnFillColumnGrid();
                SelectColumn(-1);
                FDuringApplyOrCancel = false;
            }
        }

        /**
         * This procedure will apply the current settings of the column
         *
         */
        protected virtual bool ApplyColumn(System.Int32 ASelectedColumn)
        {
            return true;
        }

        /**
         * This procedure will compare the current settings of the column with the settings stored in FColumnParameters
         * @return true if the column has changed, ie. the settings are different
         *
         */
        public virtual bool ColumnChanged(System.Int32 ASelectedColumn)
        {
            return false;
        }

        /// <summary>
        /// check if a column has been changed
        /// this should be called before loading new settings or saving the current settings
        /// </summary>
        /// <param name="AErrorMessage"></param>
        /// <returns>true if there is no changed column </returns>
        public bool CheckForUnchangedColumn(string AErrorMessage)
        {
            // has anything changed in the currently selected column? 
            if (SelectedColumnChanged())
            {
                MessageBox.Show(
                    AErrorMessage + Environment.NewLine + "Please first apply the changes to the current column, " +
                    Environment.NewLine +
                    "or cancel the changes.",
                    "Column changed");
                return false;
            }
            else
            {
                SelectColumn(-1);
            }
            return true;
        }
        
        /// <summary>
        /// the currently selected column in the grid
        /// </summary>
        public Int32 SelectedColumn
        {
            get
            {
                return FSelectedColumn;
            }
            set 
            {
                SelectColumn(SelectedColumn);
            }
        }
        
        /**
         * This procedure will compare the current settings of the currently selected column with the settings stored in FColumnParameters
         * @return true if the column has changed, ie. the settings are different
         *
         */
        public bool SelectedColumnChanged()
        {
            return ColumnChanged(FSelectedColumn);
        }
        
        /**
         * This procedure will undo the current settings of the column,
         * and then will unselect the column
         *
         */
        protected virtual void CancelColumn(System.Int32 ASelectedColumn)
        {
            FDuringApplyOrCancel = true;
            SelectColumn(-1);
            FDuringApplyOrCancel = false;
        }

        /**
         * This procedure will remove the currently selected column,
         * and after that no column is selected
         *
         */
        protected virtual bool RemoveColumn(System.Int32 ASelectedColumn, bool AAskBeforeRemove)
        {
            bool ReturnValue;

            System.Int32 Counter;
            System.Int32 MaxColumn;
            ReturnValue = false;

            if ((!AAskBeforeRemove)
                || (MessageBox.Show("Do you really want to delete this column?", "Really delete?",
                        MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                FColumnParameters.RemoveColumn(ASelectedColumn);

                // need to move the following columns to the left 
                MaxColumn = FColumnParameters.Get("MaxDisplayColumns").ToInt();

                for (Counter = ASelectedColumn + 1; Counter <= MaxColumn - 1; Counter += 1)
                {
                    FColumnParameters.MoveColumn(Counter, Counter - 1);
                }

                FColumnParameters.Add("MaxDisplayColumns", MaxColumn - 1);
                OnFillColumnGrid();
                FSelectedColumn = -1;
                SelectColumn(-1);
                ReturnValue = true;
            }

            this.grdColumns.Size = new System.Drawing.Size(this.grdColumns.Width, 120);
            return ReturnValue;
        }

        /// <summary>
        /// overload
        /// </summary>
        /// <param name="ASelectedColumn"></param>
        /// <returns></returns>
        protected virtual bool RemoveColumn(System.Int32 ASelectedColumn)
        {
            return RemoveColumn(ASelectedColumn, true);
        }

        /**
         * This procedure will switch the two columns
         * and after that the column in the new position is selected
         *
         */
        protected virtual void MoveColumn(System.Int32 ASelectedColumn, System.Int32 ANewColumnPosition)
        {
            System.Int32 MaxDisplayColumns;
            System.Int32 Counter;
            System.Int32 ReferencedColumn;

            if ((ANewColumnPosition > -1) && (ANewColumnPosition < FColumnParameters.Get("MaxDisplayColumns").ToInt()))
            {
                if (SelectColumn(-1))
                {
                    FColumnParameters.SwitchColumn(ASelectedColumn, ANewColumnPosition);

                    // switch the referenced columns in calculation 
                    MaxDisplayColumns = FColumnParameters.Get("MaxDisplayColumns").ToInt();

                    for (Counter = 0; Counter <= MaxDisplayColumns - 1; Counter += 1)
                    {
                        if (FColumnParameters.Exists("FirstColumn", Counter))
                        {
                            ReferencedColumn = FColumnParameters.Get("FirstColumn", Counter).ToInt();

                            if (ReferencedColumn == ASelectedColumn)
                            {
                                ReferencedColumn = ANewColumnPosition;
                            }
                            else if (ReferencedColumn == ANewColumnPosition)
                            {
                                ReferencedColumn = ASelectedColumn;
                            }

                            FColumnParameters.Add("FirstColumn", new TVariant(ReferencedColumn), Counter);
                        }

                        if (FColumnParameters.Exists("SecondColumn", Counter))
                        {
                            ReferencedColumn = FColumnParameters.Get("SecondColumn", Counter).ToInt();

                            if (ReferencedColumn == ASelectedColumn)
                            {
                                ReferencedColumn = ANewColumnPosition;
                            }
                            else if (ReferencedColumn == ANewColumnPosition)
                            {
                                ReferencedColumn = ASelectedColumn;
                            }

                            FColumnParameters.Add("SecondColumn", new TVariant(ReferencedColumn), Counter);
                        }
                    }

                    OnFillColumnGrid();
                    SelectColumn(ANewColumnPosition);
                }
            }
        }

        #endregion

        #region Column Functions and Calculations

        /**
         * Remove an advertised function;
         * that is necessary for some of the derived reports;
         * e.g. on the monthly reports you don't want to see a "Actual End of Year"
         *
         */
        protected void RemoveAvailableFunction(String AName)
        {
            foreach (TColumnFunction colfunc in FAvailableFunctions)
            {
                if (colfunc.GetDisplayValue() == AName)
                {
                    FAvailableFunctions.Remove(colfunc);
                    return;
                }
            }
        }

        /**
         * This will add functions to the list of available functions
         *
         */
        public virtual ArrayList SetAvailableFunctions()
        {
            FAvailableFunctions = new ArrayList();
            return FAvailableFunctions;
        }

        /**
         * get the function object of the given calculation string
         * @return nil if the function cannot be found
         *
         */
        public TColumnFunction GetFunction(String calculation)
        {
            if (FAvailableFunctions != null)
            {
                foreach (TColumnFunction Func in FAvailableFunctions)
                {
                    if (Func.GetDisplayValue() == calculation)
                    {
                        return Func;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// get the function for the calculation; if it cannot be found, check the parameters;
        /// this is special important eg. for data labels (FCalculationParameterValue)
        /// </summary>
        /// <param name="AParameterList"></param>
        /// <param name="ACalculationName"></param>
        /// <param name="AColumnNr"></param>
        /// <returns></returns>
        protected TColumnFunction GetFunction(TParameterList AParameterList, String ACalculationName, System.Int32 AColumnNr)
        {
            TColumnFunction ReturnValue;

            ReturnValue = GetFunction(ACalculationName);

            if (ReturnValue == null)
            {
                // this might be a general function that has a parameter, that is displayed 
                if (FAvailableFunctions != null)
                {
                    foreach (TColumnFunction Func in FAvailableFunctions)
                    {
                        if (Func.FDescription == ACalculationName)
                        {
                            // found an entry with e.g. DataLabelColumn 
                            // now need to check if this columns FCalculationParameterValue is used 
                            if (AParameterList.Get(Func.FCalculationParameterName, AColumnNr).ToString() == Func.FCalculationParameterValue)
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
        /// virtual function for initialising the functions
        /// TODO: replace with better design pattern
        /// </summary>
        public virtual void InitialiseFunctions()
        {
            
        }
        
        #endregion

        /// <summary>
        /// get the grid that displays the columns
        /// </summary>
        /// <returns></returns>
        public TSgrdDataGrid GetColumnsGrid()
        {
            return grdColumns;
        }
    }

    /**
     * This class contains all information needed to specify a function for 2 columns,
     * or a function that is applied to one column;
     * once it is created and added to a TFrmReporting System.Object, in one of its derived SetAvailableFunctions() Methods,
     * it will be available in the comboboxes.
     *
     */
    public class TColumnFunction
    {
        /// the name of the function, e.g. Variance
        public String FDescription;

        /// number of columns involved, e.g 1 or 2
        public System.Int32 FNumberColumns;

        /// e.g. param_label; name that the parameter should be stored under
        public String FCalculationParameterName;

        /// e.g.: Regional Director; This will be displayed
        public String FCalculationParameterValue;

        #region TColumnFunction Implementation

        /** todo: add column default width for personnel reports? }
         * constructor used for functions between columns
         *
         */
        public TColumnFunction(String ADescription, System.Int32 ANumberColumns)
        {
            FDescription = ADescription;
            FNumberColumns = ANumberColumns;
            FCalculationParameterName = "";
            FCalculationParameterValue = "";
        }

        /**
         * constructor used for calculations, that don't depend on other columns
         *
         */
        public TColumnFunction(String ADescription)
        {
            FDescription = ADescription;
            FNumberColumns = 0;
            FCalculationParameterName = "";
            FCalculationParameterValue = "";
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ADescription"></param>
        /// <param name="ACalculationParameterName"></param>
        /// <param name="ACalculationParameterValue"></param>
        public TColumnFunction(String ADescription, String ACalculationParameterName, String ACalculationParameterValue)
        {
            FNumberColumns = 0;
            FDescription = ADescription;
            FCalculationParameterName = ACalculationParameterName;
            FCalculationParameterValue = ACalculationParameterValue;
        }

        /// <summary>
        /// format a string that describes the current selected function for the column
        /// </summary>
        /// <returns></returns>
        public String GetDisplayValue()
        {
            String ReturnValue;

            ReturnValue = FDescription;

            if (FCalculationParameterValue != "")
            {
                ReturnValue = FCalculationParameterValue;
            }

            return ReturnValue;
        }

        #endregion
    }
}
