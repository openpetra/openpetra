/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       berndr
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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using System.Collections.Specialized;
using Mono.Unix;
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
        
        public System.Windows.Forms.Button GetBtnApply()
        {
        	return btnApply;
        }
        
        /// <summary>
        /// Initialisation
        /// </summary>
		public void InitialiseData()
		{
			btnDummy.Visible = false;
			
        	FColumnParameters = new TParameterList();
            FColumnParameters.Add("MaxDisplayColumns", 0);
            
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
           
            MaxDisplayColumns = FColumnParameters.Get("MaxDisplayColumns").ToInt();
            ACalculator.GetParameters().Add("MaxDisplayColumns", MaxDisplayColumns);

            for (int Counter = 0; Counter <= MaxDisplayColumns - 1; Counter += 1)
            {
            	ACalculator.GetParameters().Copy(FColumnParameters, Counter, -1, eParameterFit.eExact, Counter);
            }
            
            ACalculator.SetMaxDisplayColumns(MaxDisplayColumns);
        }

        /// <summary>
        /// Sets the selected values in the controls, using the parameters loaded from a file
        /// 
        /// </summary>
        /// <param name="AParameters"></param>
        /// <returns>void</returns>
        public void SetControls(TParameterList AParameters)
        {
            System.Int32 MaxDisplayColumns = 0;

            /* copy values for columns to the current set of parameters */
            FColumnParameters.Clear();
            
            if (AParameters.Exists("MaxDisplayColumns"))
            {
	            MaxDisplayColumns = AParameters.Get("MaxDisplayColumns").ToInt();
            }
            
            FColumnParameters.Add("MaxDisplayColumns", MaxDisplayColumns);

            for (int Counter = 0; Counter <= MaxDisplayColumns - 1; Counter += 1)
            {
            	FColumnParameters.Copy(AParameters, Counter, -1, eParameterFit.eExact, Counter);
            }
            FillColumnGrid();
        }
#endregion
		
#region event handler
		private void CmbContentChanged(System.Object sender, System.EventArgs e)
		{
			String SelectedFunction = cmbCalculation.GetSelectedString();
			foreach(TColumnFunction Func in FAvailableFunctions)
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
                TColumnFunction Func = (TColumnFunction)GetFunction(calculation);

                if (Func != null)
                {
                	cmbCalculation.SetSelectedString(Func.GetDisplayValue());
                    txtColumnWidth.Text = FColumnParameters.GetOrDefault("ColumnWidth", ASelectedColumn, new TVariant(Func.FColumnWidth)).ToString();
                }
                
                cmbCalculation.Enabled = true;
                txtColumnWidth.Enabled = true;
                
                
            }
            else

            /* unselect the column */
            {
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
            System.Int32 NewMaxColumn;
            System.Int32 Counter;
            ReturnValue = false;

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

                    /* need to move the columns to the right */
                    for (Counter = NewMaxColumn - 1; Counter >= NewColumn + 1; Counter -= 1)
                    {
                        FColumnParameters.MoveColumn(Counter - 1, Counter);
                    }
                }

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

            if (!ACheckForDoubleEntries || CheckAddDoubleEntry(this.cmbCalculation.GetSelectedString(0), ASelectedColumn))
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

            System.Int32 Counter;
            System.Int32 MaxColumn;
            ReturnValue = false;

            if ((!AAskBeforeRemove)
                || (MessageBox.Show("Do you really want to delete this column?", "Really delete?",
                        MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes))
            {
                FColumnParameters.RemoveColumn(ASelectedColumn);

                /* need to move the following columns to the left */
                MaxColumn = FColumnParameters.Get("MaxDisplayColumns").ToInt();

                for (Counter = ASelectedColumn + 1; Counter <= MaxColumn - 1; Counter += 1)
                {
                    FColumnParameters.MoveColumn(Counter, Counter - 1);
                }

                FColumnParameters.Add("MaxDisplayColumns", MaxColumn - 1);
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
            System.Int32 MaxDisplayColumns;
            System.Int32 Counter;
            System.Int32 ReferencedColumn;

            if ((ANewColumnPosition > -1) && (ANewColumnPosition < FColumnParameters.Get("MaxDisplayColumns").ToInt()))
            {
                if (SelectColumn(-1))
                {
                    FColumnParameters.SwitchColumn(ASelectedColumn, ANewColumnPosition);

                    /* switch the referenced columns in calculation */
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
        protected void FillColumnGrid()
        {
            DataTable columnTab;
            DataRow rowContent;
            DataRow rowDisplayWidth;

            System.Int32 counter;
            String calculation;
            System.Int32 rowCounter;
            SourceGrid.Cells.ColumnHeader myColumnHeader;
            TColumnFunction Func;

//            /* if the columns page is not displayed, don't bother filling the grid */
//            if (TCl_ReportSettings.TabPages.IndexOf(TPg_Columns) == -1)
//            {
//                return;
//            }

//            base.FillColumnGrid();
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
                Func = (TColumnFunction)GetFunction(calculation);

                if (Func != null)
                {
                    rowContent[counter] = calculation;
                    rowDisplayWidth[counter] =
                        FColumnParameters.GetOrDefault("ColumnWidth", counter, new TVariant(Func.FColumnWidth)).ToString() + " cm";
                }
            }

            rowCounter = 0;
            columnTab.Rows.InsertAt(rowContent, rowCounter);
            rowCounter = rowCounter + 1;
            columnTab.Rows.InsertAt(rowDisplayWidth, rowCounter);
            grdColumns.Columns.Clear();

            for (counter = 0; counter <= FColumnParameters.Get("MaxDisplayColumns").ToInt() - 1; counter += 1)
            {
                grdColumns.AddTextColumn("Column " + Convert.ToString(counter + 1), columnTab.Columns[counter]);
                myColumnHeader = (SourceGrid.Cells.ColumnHeader)grdColumns.Columns[counter].HeaderCell;
            }

            grdColumns.DataSource = new DevAge.ComponentModel.BoundDataView(new DataView(columnTab));
            grdColumns.DataSource.AllowEdit = false;
            grdColumns.DataSource.AllowNew = false;
            grdColumns.DataSource.AllowDelete = false;
            grdColumns.AutoSizeCells();

            /* grdColumns.Width := 576;   it is necessary to reassign the width because the columns don't take up the maximum width */
        }
        
        /// <summary>
        /// Checks if there is already a field with the same name in the grid. If yes, ask
        /// if the field should be added again.
        /// </summary>
        /// <param name="AColumnName">Name of the field</param>
        /// <param name="ASelectedColumn">Index of the column in the grid</param>
        /// <returns>True if the field should be added. Otherwise false</returns>
        protected bool CheckAddDoubleEntry(String AColumnName, int ASelectedColumn)
        {
        	bool ReturnValue = true;
        	DataTable ColumnTable = FColumnParameters.ToDataTable();

        	String NewField = "eString:" + AColumnName;

        	foreach(DataRow Row in ColumnTable.Rows)
        	{
        		if ((Row["value"].ToString() == NewField) &&
        		     (((int)Row["column"]) != ASelectedColumn))
        		{
        			if (MessageBox.Show("The column is already there.\nDo you want to add it anyway?", "Add field?", MessageBoxButtons.YesNo) == DialogResult.No)
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
        /// <returns>nil if the function cannot be found
        /// </returns>
        protected TColumnFunction GetFunction(String calculation)
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
	}
}