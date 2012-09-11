
using System;
using System.ComponentModel;
using DevAge.ComponentModel;
using SourceGrid.Cells;
using SourceGrid.Selection;

namespace SourceGrid.Extensions.PingGrids
{
	[System.ComponentModel.ToolboxItem(true)]
	public class PingGrid : GridVirtual
	{
		public PingGrid()
		{
			FixedRows = 1;
			FixedColumns = 0;
			
			Controller.AddController(new PingGridCellController());
			
			this.DataSource = new EmptyPingSource();
			SelectionMode = GridSelectionMode.Row;
		}
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose (disposing);
		}
		
		/// <summary>
		/// Method used to create the rows object, in this class of type PingGridRows.
		/// </summary>
		protected override RowsBase CreateRowsObject()
		{
			return new PingGridRows(this);
		}
		
		/// <summary>
		/// Method used to create the columns object, in this class of type DataGridColumns.
		/// </summary>
		protected override ColumnsBase CreateColumnsObject()
		{
			return new PingGridColumns(this);
		}
		
		protected override SelectionBase CreateSelectionObject()
		{
			SourceGrid.Selection.SelectionBase selObj = base.CreateSelectionObject();
			
			selObj.EnableMultiSelection = true;
			selObj.FocusStyle = SourceGrid.FocusStyle.RemoveFocusCellOnLeave;
			selObj.FocusRowLeaving += new RowCancelEventHandler(Selection_FocusRowLeaving);
			
			return selObj;
		}
		
		private IPingData mBoundList;
		
		/// <summary>
		/// Sorting is always enabled
		/// </summary>
		public override bool EnableSort{
			get{
				return true;
			}
			set
			{
			}
		}
		
		/// <summary>
		/// Gets or sets the IBoundList used for data binding.
		/// It can be any class that implements the IBoundList interface, usually can be BoundList
		///  (that can be used to bind to a generic List) or BoundDataView (that can be used to bind to a DataView).
		/// </summary>
		public IPingData DataSource
		{
			get { return mBoundList; }
			set
			{
				Unbind();
				if (value == null)
					mBoundList = new EmptyPingSource();
				else
					mBoundList = value;
				if (mBoundList != null)
					Bind();
			}
		}
		
		protected virtual void Unbind()
		{
			if (mBoundList != null)
			{
				//mBoundList.ListChanged -= new ListChangedEventHandler(mBoundList_ListChanged);
				//mBoundList.ItemDeleted -= new ItemDeletedEventHandler(mBoundList_ItemDeleted);
				//mBoundList.ListCleared -= new EventHandler(mBoundList_ListCleared);
			}
			
			Rows.RowsChanged();
		}
		
		protected virtual void Bind()
		{
			//if (Columns.Count == 0)
			//	CreateColumns();
			//InvalidateDataGridColumns();
			
			//mBoundList.ListChanged += new ListChangedEventHandler(mBoundList_ListChanged);
			//mBoundList.ItemDeleted += new ItemDeletedEventHandler(mBoundList_ItemDeleted);
			//mBoundList.ListCleared += new EventHandler(mBoundList_ListCleared);
			Rows.RowsChanged();
			//Rows.ResetRowHeigth();
		}
		
		[Obsolete]
		void mBoundList_ListCleared ( object sender, EventArgs e )
		{
			Rows.ResetRowHeigth();
		}
		
		[Obsolete]
		void mBoundList_ItemDeleted ( object sender, DevAge.ComponentModel.ItemDeletedEventArgs e )
		{
			Rows.RowDeleted(e.Item);
		}
		
		[Obsolete]
		private void InvalidateDataGridColumns()
		{
			foreach (PingGridColumn column in Columns)
			{
				column.Invalidate();
			}
		}
		
		/// <summary>
		/// Gets the rows information as a PingGridRows object.
		/// </summary>
		public new PingGridRows Rows
		{
			get{return (PingGridRows)base.Rows;}
		}
		
		/// <summary>
		/// Gets the columns informations as a PingGridColumns object.
		/// </summary>
		public new PingGridColumns Columns
		{
			get{return (PingGridColumns)base.Columns;}
		}
		
		protected virtual void mBoundList_ListChanged(object sender, ListChangedEventArgs e)
		{
			if (base.IsSuspended() == true)
				return;
			Rows.RowsChanged();
			Invalidate(true);
		}
		
		/// <summary>
		/// Gets a specified Cell by its row and column.
		/// </summary>
		/// <param name="p_iRow"></param>
		/// <param name="p_iCol"></param>
		/// <returns></returns>
		public override ICellVirtual GetCell(int p_iRow, int p_iCol)
		{
			if (mBoundList == null)
				return null;
			if (p_iCol >= Columns.Count)
				return null;
			
			if (p_iRow < FixedRows)
				return Columns[p_iCol].HeaderCell;
			else
				return Columns[p_iCol].GetDataCell(p_iRow);
		}
		
		protected override void OnSortingRangeRows(SortRangeRowsEventArgs e)
		{
			base.OnSortingRangeRows (e);
			
			if (DataSource == null || DataSource.AllowSort == false)
				return;
			var prop = this.Columns[e.KeyColumn].PropertyName;
			DataSource.ApplySort(prop, e.Ascending);
			
			// force redraw
			this.Invalidate();
		}
		
		/// <summary>
		/// Gets or sets the selected DataRowView.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Obsolete]
		public object[] SelectedDataRows
		{
			get
			{
				if (mBoundList == null)
					return new System.Data.DataRowView[0];
				
				int[] rowsSel = Selection.GetSelectionRegion().GetRowsIndex();
				
				int count = 0;
				for (int i = 0; i < rowsSel.Length; i++)
				{
					object objRow= Rows.IndexToDataSourceRow(rowsSel[i]);
					if (objRow != null)
						count++;
				}
				
				object[] dataRows = new object[count];
				int indexRows = 0;
				for (int i = 0; i < rowsSel.Length; i++)
				{
					object objRow = Rows.IndexToDataSourceRow(rowsSel[i]);
					if (objRow != null)
					{
						dataRows[indexRows] = objRow;
						indexRows++;
					}
				}
				return dataRows;
			}
			set
			{
				Selection.ResetSelection(false);
				
				if (mBoundList != null && value != null)
				{
					for (int i = 0; i < value.Length; i++)
					{
						for (int r = FixedRows; r < Rows.Count; r++)
						{
							object objRow = Rows.IndexToDataSourceRow(r);
							
							if (object.ReferenceEquals(objRow, value[i]))
							{
								Selection.SelectRow(r, true);
								break;
							}
						}
					}
				}
			}
		}
		
		/*[Obsolete]
		protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
		{
			base.OnKeyDown(e);
	
			if (e.KeyCode == System.Windows.Forms.Keys.Delete &&
			    mBoundList != null &&
			    mBoundList.AllowDelete &&
			    e.Handled == false &&
			    mDeleteRowsWithDeleteKey)
			{
				object[] rows = SelectedDataRows;
				if (rows != null && rows.Length > 0)
					DeleteSelectedRows();
	
				e.Handled = true;
			}
			else if (e.KeyCode == System.Windows.Forms.Keys.Escape &&
			         e.Handled == false &&
			         mCancelEditingWithEscapeKey)
			{
				EndEditingRow(true);
	
				e.Handled = true;
			}
		}*/
		
		
		protected override void OnValidating(CancelEventArgs e)
		{
			base.OnValidating(e);
			
			try
			{
				//if (EndEditingRowOnValidate)
				{
					//EndEditingRow(false);
				}
			}
			catch (Exception ex)
			{
				OnUserException(new ExceptionEventArgs( ex ));
			}
		}
		
		private bool mEndEditingRowOnValidate = true;
		/// <summary>
		/// Gets or sets a property to force an End Editing when the control loose the focus
		/// </summary>
		[System.ComponentModel.DefaultValue(true)]
		[Obsolete]
		public bool EndEditingRowOnValidate
		{
			get { return mEndEditingRowOnValidate; }
			set { mEndEditingRowOnValidate = value; }
		}
		
		private bool mDeleteRowsWithDeleteKey = true;
		/// <summary>
		/// Gets or sets if enable the delete of the selected rows when pressing Delete key.
		/// </summary>
		[System.ComponentModel.DefaultValue(true)]
		public bool DeleteRowsWithDeleteKey
		{
			get{return mDeleteRowsWithDeleteKey;}
			set{mDeleteRowsWithDeleteKey = value;}
		}
		
		private bool mCancelEditingWithEscapeKey = true;
		
		/// <summary>
		/// Gets or sets if enable the Cancel Editing feature when pressing escape key
		/// </summary>
		[System.ComponentModel.DefaultValue(true)]
		public bool CancelEditingWithEscapeKey
		{
			get{return mCancelEditingWithEscapeKey;}
			set{mCancelEditingWithEscapeKey = value;}
		}
		
		private string mDeleteQuestionMessage = "Are you sure to delete all the selected rows?";
		/// <summary>
		/// Message showed with the DeleteSelectedRows method. Set to null to not show any message.
		/// </summary>
		public string DeleteQuestionMessage
		{
			get{return mDeleteQuestionMessage;}
			set{mDeleteQuestionMessage = value;}
		}
		
		/// <summary>
		/// Delete all the selected rows.
		/// </summary>
		/// <returns>Returns true if one or more row is deleted otherwise false.</returns>
		[Obsolete]
		public virtual bool DeleteSelectedRows()
		{
			if (string.IsNullOrEmpty(mDeleteQuestionMessage) ||
			    System.Windows.Forms.MessageBox.Show(this, mDeleteQuestionMessage, System.Windows.Forms.Application.ProductName, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
			{
				foreach (int gridRow in Selection.GetSelectionRegion().GetRowsIndex())
				{
					int dataIndex = Rows.IndexToDataSourceIndex(gridRow);
					//if (dataIndex < DataSource.Count)
					//	DataSource.RemoveAt(dataIndex);
				}
				
				return true;
			}
			
			return false;
		}
		
		/// <summary>
		/// AutoSize the columns based on the visible range and autosize the rows based on it's contents.
		/// </summary>
		public override void AutoSizeCells()
		{
			Columns.AutoSizeView();
			for ( int i = 0; i < Rows.Count; i++ )
				Rows.AutoSizeRow(i);
		}
		
		private void Selection_FocusRowLeaving(object sender, RowCancelEventArgs e)
		{
			try
			{
				//EndEditingRow(false);
			}
			catch(Exception exc)
			{
				OnUserException(new ExceptionEventArgs(new EndEditingException( exc ) ) );
				
				e.Cancel = true;
			}
		}
		
		/// <summary>
		/// Check if the specified row is the active row (focused), return false if it is not the active row. Then call the BeginEdit on the associated DataRowView. Add a row to the DataView if required. Returns true if the method sucesfully call the BeginEdit and set the EditingRow property.
		/// </summary>
		/// <param name="gridRow"></param>
		/// <returns></returns>
		[Obsolete]
		public bool BeginEditRow(int gridRow)
		{
			/*
			if (mEditingRow != null && mEditingRow.Value == gridRow)
				return true;
	
			EndEditingRow(false); //Terminate the old edit if present
	
			if (DataSource != null)
			{
				int dataIndex = Rows.IndexToDataSourceIndex(gridRow);
	
				// add this here to check if we have permission for edition
				if (!DataSource.AllowEdit)
					return false;
	
				if (dataIndex == DataSource.Count && DataSource.AllowNew) //Last Row
				{
					DataSource.BeginAddNew();
				}
				else if (dataIndex < DataSource.Count)
				{
					DataSource.BeginEdit(dataIndex);
				}
			}
	
			mEditingRow = gridRow;
			 */
			return true;
		}
		
		/// <summary>
		/// Calls the CancelEdit or the EndEdit on the editing Row and set to null the editing row.
		/// </summary>
		/// <param name="cancel"></param>
		[Obsolete]
		public void EndEditingRow(bool cancel)
		{
			//if (mBoundList != null)
			//	mBoundList.EndEdit(cancel);
			
			//mEditingRow = null;
		}
	}
}
