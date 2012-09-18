using SourceGrid.Cells.Models;
using System;
using SourceGrid.Cells;

namespace SourceGrid
{
	/// <summary>
	/// This class derive from GridVirtual and create a grid bound to an array.
	/// </summary>
    [System.ComponentModel.ToolboxItem(true)]
    public class ArrayGrid : GridVirtual
	{
		public ArrayGrid():base()
		{
		}
		

		public override bool EnableSort{get;set;}

		protected override RowsBase CreateRowsObject()
		{
			return new ArrayRows(this);
		}

		protected override ColumnsBase CreateColumnsObject()
		{
			return new ArrayColumns(this);
		}

		public override ICellVirtual GetCell(int p_iRow, int p_iCol)
		{
			if (p_iRow < FixedRows &&
				p_iCol < FixedColumns)
				return mHeader;
			else if (p_iRow < FixedRows)
				return mColumnHeader;
			else if (p_iCol < FixedColumns)
				return mRowHeader;
			else
				return mValueCell;
		}

		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public new ArrayRows Rows
		{
			get{return (ArrayRows)base.Rows;}
		}
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public new ArrayColumns Columns
		{
			get{return (ArrayColumns)base.Columns;}
		}

		private Array mDataSource = null;

		/// <summary>
		/// Gets or sets the data source array used to bind the grid.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Array DataSource
		{
			get{return mDataSource;}
			set
			{
				if (value != null && value.Rank != 2)
					throw new SourceGridException("Array dimension not valid, must be an array with 2 dimensions");

				mDataSource = value;
				Bind();
			}
		}

		protected virtual void Bind()
		{
			ValueCell = null;
			if (mDataSource != null)
			{
				mValueCell = new SourceGrid.Cells.Virtual.CellVirtual();
				mValueCell.Model.AddModel(new ArrayValueModel());
                mValueCell.Editor = Cells.Editors.Factory.Create(mDataSource.GetType().GetElementType());
			}

			Rows.RowsChanged();
			Columns.ColumnsChanged();
		}

		private ICellVirtual mColumnHeader = new ArrayColumnHeader();
		/// <summary>
		/// Gets or sets the cell used for the column headers.  Only used when FixedRows is greater than 0.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public ICellVirtual ColumnHeader
		{
			get{return mColumnHeader;}
			set{mColumnHeader = value;}
		}

		private ICellVirtual mRowHeader = new ArrayRowHeader();
		/// <summary>
		/// Gets or sets the cell used for the row headers. Only used when FixedColumns is greater than 0.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public ICellVirtual RowHeader
		{
			get{return mRowHeader;}
			set{mRowHeader = value;}
		}

		private ICellVirtual mHeader = new ArrayHeader();
		/// <summary>
		/// Gets or sets the cell used for the left top position header. Only used when FixedRows and FixedColumns are greater than 0.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public ICellVirtual Header
		{
			get{return mHeader;}
			set{mHeader = value;}
		}

		private ICellVirtual mValueCell;
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public ICellVirtual ValueCell
		{
			get{return mValueCell;}
			set{mValueCell = value;}
		}
	}

	#region Models
	public class ArrayValueModel : IValueModel
	{
		#region IValueModel Members
		public virtual object GetValue(CellContext cellContext)
		{
			Array array = ((ArrayGrid)cellContext.Grid).DataSource;
			return array.GetValue(cellContext.Position.Row - cellContext.Grid.FixedRows, cellContext.Position.Column - cellContext.Grid.FixedColumns);
		}

		public virtual void SetValue(CellContext cellContext, object p_Value)
		{
			Array array = ((ArrayGrid)cellContext.Grid).DataSource;
			object oldValue = array.GetValue(cellContext.Position.Row - cellContext.Grid.FixedRows, cellContext.Position.Column - cellContext.Grid.FixedColumns);
			ValueChangeEventArgs valArgs = new ValueChangeEventArgs(oldValue, p_Value);
			if (cellContext.Grid != null)
				cellContext.Grid.Controller.OnValueChanging(cellContext, valArgs);

			array.SetValue(p_Value, cellContext.Position.Row - cellContext.Grid.FixedRows, cellContext.Position.Column - cellContext.Grid.FixedColumns);

			if (cellContext.Grid != null)
				cellContext.Grid.Controller.OnValueChanged(cellContext, EventArgs.Empty);
		}
		#endregion
	}

	public class ArrayRowHeaderModel : Cells.Models.IValueModel
	{
		public ArrayRowHeaderModel()
		{
		}
		#region IValueModel Members
		public virtual object GetValue(CellContext cellContext)
		{
			return cellContext.Position.Row - cellContext.Grid.FixedRows;
		}

		public virtual void SetValue(CellContext cellContext, object p_Value)
		{
			throw new ApplicationException("Not supported");
		}
		#endregion
	}
	public class ArrayColumnHeaderModel : Cells.Models.IValueModel
	{
		public ArrayColumnHeaderModel()
		{
		}
		#region IValueModel Members
		public virtual object GetValue(CellContext cellContext)
		{
			return cellContext.Position.Column - cellContext.Grid.FixedColumns;
		}

		public virtual void SetValue(CellContext cellContext, object p_Value)
		{
			throw new ApplicationException("Not supported");
		}
		#endregion
	}
	#endregion

	#region Cells
	/// <summary>
	/// A cell header used for the columns. Usually used in the HeaderCell property of a DataGridColumn.
	/// </summary>
	public class ArrayColumnHeader : Cells.Virtual.ColumnHeader
	{
		public ArrayColumnHeader()
		{
			Model.AddModel(new ArrayColumnHeaderModel());
			AutomaticSortEnabled = false;
            ResizeEnabled = false;

//            ColumnSelectorEnabled = true;
//            ColumnFocusEnabled = true;
		}
	}

	/// <summary>
	/// A cell used as left row selector. Usually used in the DataCell property of a DataGridColumn. If FixedColumns is grater than 0 and the columns are automatically created then the first column is created of this type.
	/// </summary>
	public class ArrayRowHeader : Cells.Virtual.RowHeader
	{
		public ArrayRowHeader()
		{
			Model.AddModel(new ArrayRowHeaderModel());
            ResizeEnabled = false;

            //RowSelectorEnabled = true;
		}
	}

	/// <summary>
	/// A cell used for the top/left cell when using DataGridRowHeader.
	/// </summary>
	public class ArrayHeader : Cells.Virtual.Header
	{
		public ArrayHeader()
		{
			Model.AddModel(new SourceGrid.Cells.Models.NullValueModel());
		}
	}
	#endregion

	#region Rows and Columns
	public class ArrayRows : RowsSimpleBase
	{
		public ArrayRows(ArrayGrid grid):base(grid)
		{
		}

		public new ArrayGrid Grid
		{
			get{return (ArrayGrid)base.Grid;}
		}

		public override int Count
		{
			get
			{
				if (Grid.DataSource == null)
					return Grid.FixedRows;
				else
				{
					return Grid.DataSource.GetLength(0) + Grid.FixedRows;
				}
			}
		}

        private AutoSizeMode mAutoSizeMode = AutoSizeMode.Default;
        public AutoSizeMode AutoSizeMode
        {
            get { return mAutoSizeMode; }
            set { mAutoSizeMode = value; }
        }

        public override AutoSizeMode GetAutoSizeMode(int row)
        {
            return mAutoSizeMode;
        }
	}
	public class ArrayColumns : ColumnInfoCollection
	{
		public ArrayColumns(ArrayGrid grid):base(grid)
		{
		}

		public new ArrayGrid Grid
		{
			get{return (ArrayGrid)base.Grid;}
		}

		/*public override int Count
		{
			get
			{
				if (Grid.DataSource == null)
					return Grid.FixedColumns;
				else
				{
					return Grid.DataSource.GetLength(1) + Grid.FixedColumns;
				}
			}
		}*/
/*
        private AutoSizeMode mAutoSizeMode = AutoSizeMode.Default;
        public AutoSizeMode AutoSizeMode
        {
            get { return mAutoSizeMode; }
            set { mAutoSizeMode = value; }
        }

        public override AutoSizeMode GetAutoSizeMode(int row)
        {
            return mAutoSizeMode;
        }*/
	}

	#endregion
}
