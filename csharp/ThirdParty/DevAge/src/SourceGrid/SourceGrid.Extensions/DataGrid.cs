using SourceGrid.Selection;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using DevAge.ComponentModel;

namespace SourceGrid
{
	/// <summary>
	/// A grid control that support load from a System.Data.DataView class, usually used for data binding.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public class DataGrid : GridVirtual
	{
		public DataGrid()
		{
			FixedRows = 1;
			FixedColumns = 0;

			Controller.AddController(new DataGridCellController());

			SelectionMode = GridSelectionMode.Row;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose (disposing);
		}

		/// <summary>
		/// Method used to create the rows object, in this class of type DataGridRows.
		/// </summary>
		protected override RowsBase CreateRowsObject()
		{
			return new DataGridRows(this);
		}

		/// <summary>
		/// Method used to create the columns object, in this class of type DataGridColumns.
		/// </summary>
		protected override ColumnsBase CreateColumnsObject()
		{
			return new DataGridColumns(this);
		}

		protected override SelectionBase CreateSelectionObject()
		{
			SourceGrid.Selection.SelectionBase selObj = base.CreateSelectionObject();

			selObj.EnableMultiSelection = false;
			selObj.FocusStyle = SourceGrid.FocusStyle.RemoveFocusCellOnLeave;
			selObj.FocusRowLeaving += new RowCancelEventHandler(Selection_FocusRowLeaving);

			return selObj;
		}

		private DevAge.ComponentModel.IBoundList mBoundList;

		public override bool EnableSort{
			get{
				if (DataSource == null)
					return false;
				return DataSource.AllowSort;
			}
			set
			{
				if (DataSource == null)
					return;
				DataSource.AllowSort = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the IBoundList used for data binding.
		/// It can be any class that implements the IBoundList interface, usually can be BoundList
		///  (that can be used to bind to a generic List) or BoundDataView (that can be used to bind to a DataView).
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DevAge.ComponentModel.IBoundList DataSource
		{
			get { return mBoundList; }
			set
			{
				Unbind();
				mBoundList = value;
				if (mBoundList != null)
					Bind();
			}
		}

		protected virtual void Unbind()
		{
			if (mBoundList != null)
			{
				mBoundList.ListChanged -= new ListChangedEventHandler(mBoundList_ListChanged);
				mBoundList.ItemDeleted -= new ItemDeletedEventHandler(mBoundList_ItemDeleted);
				mBoundList.ListCleared -= new EventHandler(mBoundList_ListCleared);
			}

			Rows.RowsChanged();
		}

		protected virtual void Bind()
		{
			if (Columns.Count == 0)
				CreateColumns();
			InvalidateDataGridColumns();

			mBoundList.ListChanged += new ListChangedEventHandler(mBoundList_ListChanged);
			mBoundList.ItemDeleted += new ItemDeletedEventHandler(mBoundList_ItemDeleted);
			mBoundList.ListCleared += new EventHandler(mBoundList_ListCleared);
			Rows.RowsChanged();
			Rows.ResetRowHeigth();
		}

		void mBoundList_ListCleared ( object sender, EventArgs e )
		{
			Rows.ResetRowHeigth();
		}

		void mBoundList_ItemDeleted ( object sender, DevAge.ComponentModel.ItemDeletedEventArgs e )
		{
			Rows.RowDeleted(e.Item);
		}

		private void InvalidateDataGridColumns()
		{
			foreach (DataGridColumn column in Columns)
			{
				column.Invalidate();
			}
		}
		
		/// <summary>
		/// Gets the rows information as a DataGridRows object.
		/// </summary>
		public new DataGridRows Rows
		{
			get{return (DataGridRows)base.Rows;}
		}

		/// <summary>
		/// Gets the columns informations as a DataGridColumns object.
		/// </summary>
		public new DataGridColumns Columns
		{
			get{return (DataGridColumns)base.Columns;}
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
		public override Cells.ICellVirtual GetCell(int p_iRow, int p_iCol)
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

			System.ComponentModel.PropertyDescriptor propertyCol = Columns[e.KeyColumn].PropertyColumn;

			if (propertyCol != null)
			{
				ListSortDirection direction;
				if (e.Ascending)
					direction = ListSortDirection.Ascending;
				else
					direction = ListSortDirection.Descending;
				ListSortDescription[] sortsArray = new ListSortDescription[1];
				sortsArray[0] = new ListSortDescription(propertyCol, direction);

				DataSource.ApplySort(new ListSortDescriptionCollection(sortsArray));
			}
			else
				DataSource.ApplySort(null);
		}

		/// <summary>
		/// Automatic create the columns classes based on the specified DataSource.
		/// </summary>
		public void CreateColumns()
		{
			Columns.Clear();
			if (DataSource != null)
			{
				int i = 0;

				if (FixedColumns > 0)
				{
					Columns.Insert(i, DataGridColumn.CreateRowHeader(this));
					i++;
				}

				foreach (System.ComponentModel.PropertyDescriptor prop in DataSource.GetItemProperties())
				{
					Columns.Add(prop.Name,
					            prop.DisplayName,
					            SourceGrid.Cells.DataGrid.Cell.Create(prop.PropertyType, !prop.IsReadOnly));
				}
			}
		}

		/// <summary>
		/// Gets or sets the selected DataRowView.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
		}


		protected override void OnValidating(CancelEventArgs e)
		{
			base.OnValidating(e);

			try
			{
				if (EndEditingRowOnValidate)
				{
					EndEditingRow(false);
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
		public virtual bool DeleteSelectedRows()
		{
			if (string.IsNullOrEmpty(mDeleteQuestionMessage) ||
			    System.Windows.Forms.MessageBox.Show(this, mDeleteQuestionMessage, System.Windows.Forms.Application.ProductName, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
			{
				foreach (int gridRow in Selection.GetSelectionRegion().GetRowsIndex())
				{
					int dataIndex = Rows.IndexToDataSourceIndex(gridRow);
					if (dataIndex < DataSource.Count)
						DataSource.RemoveAt(dataIndex);
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
				EndEditingRow(false);
			}
			catch(Exception exc)
			{
				OnUserException(new ExceptionEventArgs(new EndEditingException( exc ) ) );

				e.Cancel = true;
			}
		}

		private int? mEditingRow;
		/// <summary>
		/// Check if the specified row is the active row (focused), return false if it is not the active row. Then call the BeginEdit on the associated DataRowView. Add a row to the DataView if required. Returns true if the method sucesfully call the BeginEdit and set the EditingRow property.
		/// </summary>
		/// <param name="gridRow"></param>
		/// <returns></returns>
		public bool BeginEditRow(int gridRow)
		{
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

			return true;
		}

		/// <summary>
		/// Calls the CancelEdit or the EndEdit on the editing Row and set to null the editing row.
		/// </summary>
		/// <param name="cancel"></param>
		public void EndEditingRow(bool cancel)
		{
			if (mBoundList != null)
				mBoundList.EndEdit(cancel);

			mEditingRow = null;
		}
	}

	#region Models
	/// <summary>
	/// A Model of type IValueModel used for binding the value to a specified property of the bound object.
	/// Used for the DataGrid control.
	/// </summary>
	public class DataGridValueModel : Cells.Models.IValueModel
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public DataGridValueModel()
		{
		}
		#region IValueModel Members

		public object GetValue(CellContext cellContext)
		{
			DataGrid grid = (DataGrid)cellContext.Grid;

			PropertyDescriptor prop = grid.Columns[cellContext.Position.Column].PropertyColumn;

			int dataIndex = grid.Rows.IndexToDataSourceIndex(cellContext.Position.Row);

			//Check if the row is not outside the valid range (for example to handle the new row)
			if (dataIndex >= grid.DataSource.Count)
				return null;
			else
				return grid.DataSource.GetItemValue(dataIndex, prop);
		}

		public void SetValue(CellContext cellContext, object value)
		{
			DataGrid grid = (DataGrid)cellContext.Grid;
			PropertyDescriptor prop = grid.Columns[cellContext.Position.Column].PropertyColumn;
			object oldValue = GetValue(cellContext);
			
			ValueChangeEventArgs valArgs = new ValueChangeEventArgs(oldValue, value);
			if (cellContext.Grid != null)
				cellContext.Grid.Controller.OnValueChanging(cellContext, valArgs);

			

			

			grid.DataSource.SetEditValue(prop, valArgs.NewValue);

			if (cellContext.Grid != null)
				cellContext.Grid.Controller.OnValueChanged(cellContext, EventArgs.Empty);
		}
		#endregion
	}
	public class DataGridRowHeaderModel : Cells.Models.IValueModel
	{
		public DataGridRowHeaderModel()
		{
		}
		#region IValueModel Members
		public object GetValue(CellContext cellContext)
		{
			DataGrid dataGrid = (DataGrid)cellContext.Grid;
			if (dataGrid.DataSource != null &&
			    dataGrid.DataSource.AllowNew &&
			    cellContext.Position.Row == (dataGrid.Rows.Count - 1))
				return "*";
			else
				return null;
		}

		public void SetValue(CellContext cellContext, object p_Value)
		{
			throw new ApplicationException("Not supported");
		}
		#endregion
	}
	#endregion

	#region Controller
	public class DataGridCellController : Cells.Controllers.ControllerBase
	{
		public override void OnValueChanging(CellContext sender, ValueChangeEventArgs e)
		{
			base.OnValueChanging (sender, e);

			//BeginEdit on the row, set the Cancel = true if failed to start editing.
			bool success = ((DataGrid)sender.Grid).BeginEditRow(sender.Position.Row);
			if (success == false)
				throw new SourceGridException("Failed to editing row " + sender.Position.Row.ToString());
		}

		public override void OnEditStarting(CellContext sender, CancelEventArgs e)
		{
			base.OnEditStarting (sender, e);

			//BeginEdit on the row, set the Cancel = true if failed to start editing.
			bool success = ((DataGrid)sender.Grid).BeginEditRow(sender.Position.Row);
			e.Cancel = !success;
		}
	}
	#endregion


	[Serializable]
	public class EndEditingException : SourceGridException
	{
		public EndEditingException(Exception innerException):
			base(innerException.Message, innerException)
		{
		}
		protected EndEditingException(System.Runtime.Serialization.SerializationInfo p_Info, System.Runtime.Serialization.StreamingContext p_StreamingContext):
			base(p_Info, p_StreamingContext)
		{
		}
	}
}

