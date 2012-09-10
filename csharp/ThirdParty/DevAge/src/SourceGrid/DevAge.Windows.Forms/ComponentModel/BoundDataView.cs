using System;
using System.Collections.Generic;
using System.Text;

namespace DevAge.ComponentModel
{
	/// <summary>
	/// A class to support list binding to a DataView object.
	/// Implement the IBoundList.
	/// </summary>
	[Serializable]
	public class BoundDataView : IBoundList
	{
		private System.Data.DataView m_dataView;
		private System.Data.DataTable m_dataTable;

		[Obsolete("Use property DataView instead")]
		public System.Data.DataView mDataView {
			get { return mDataView; }
		}
		
		[Obsolete("Use property DataTable instead")]
		public System.Data.DataTable mDataTable {
			get { return mDataTable; }
		}
		
		public System.Data.DataView DataView {
			get { return m_dataView; }
		}
		
		public System.Data.DataTable DataTable {
			get { return m_dataTable; }
		}
		

		
		public BoundDataView(System.Data.DataView dataView)
		{
			m_dataView = dataView;

			m_dataView.ListChanged += new System.ComponentModel.ListChangedEventHandler(mDataView_ListChanged);
			// Save it for destructor
			m_dataTable = m_dataView.Table;
			if ( m_dataTable != null )
			{
				m_dataTable.TableCleared += new System.Data.DataTableClearEventHandler(Table_TableCleared);
				m_dataTable.RowDeleted += new System.Data.DataRowChangeEventHandler(Table_RowDeleted);
			}
		}

		~BoundDataView()
		{
			if ( m_dataTable != null )
			{
				m_dataTable.TableCleared -= new System.Data.DataTableClearEventHandler(Table_TableCleared);
				m_dataTable.RowDeleted -= new System.Data.DataRowChangeEventHandler(Table_RowDeleted);
			}
		}

		public event System.ComponentModel.ListChangedEventHandler ListChanged;
		protected virtual void OnListChanged(System.ComponentModel.ListChangedEventArgs e)
		{
			if (ListChanged != null)
				ListChanged(this, e);
		}
		void mDataView_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
		{
			OnListChanged(e);
		}

		public event EventHandler ListCleared;
		protected virtual void OnListCleared(EventArgs e)
		{
			if ( ListCleared != null )
				ListCleared(this, e);
		}
		void Table_TableCleared ( object sender, System.Data.DataTableClearEventArgs e )
		{
			OnListCleared(EventArgs.Empty);
		}

		public event ItemDeletedEventHandler ItemDeleted;
		protected virtual void OnItemDeleted(ItemDeletedEventArgs e)
		{
			if ( ItemDeleted != null )
				ItemDeleted(this, e);
		}
		void Table_RowDeleted ( object sender, System.Data.DataRowChangeEventArgs e )
		{
			OnItemDeleted(new ItemDeletedEventArgs(e.Row));
		}

		private System.Data.DataRowView mEditingRow;
		public virtual int BeginAddNew()
		{
			if (mEditingRow != null)
				throw new DevAgeApplicationException("There is already a row in editing state, call EndEdit first");

			mEditingRow = m_dataView.AddNew();

			mEditingRow.BeginEdit();

			System.Collections.IList list = (System.Collections.IList)m_dataView;

			return list.IndexOf(mEditingRow);
		}

		public virtual void BeginEdit(int index)
		{
			if (mEditingRow != null)
				throw new DevAgeApplicationException("There is already a row in editing state, call EndEdit first");

			mEditingRow = m_dataView[index];

			mEditingRow.BeginEdit();
		}

		public virtual void EndEdit(bool cancel)
		{
			if (mEditingRow == null)
				return;

			if (cancel)
				mEditingRow.CancelEdit();
			else
				mEditingRow.EndEdit();

			mEditingRow = null;

			//when CancelEdit the DataView doesn't automatically call a ListChanged event, so I will call it manually
			if (cancel)
				OnListChanged(new System.ComponentModel.ListChangedEventArgs(System.ComponentModel.ListChangedType.Reset, -1));
		}

		/// <summary>
		/// Gets the current edited object
		/// </summary>
		public virtual object EditedObject
		{
			get { return mEditingRow; }
		}

		public virtual int IndexOf(object item)
		{
			System.Collections.IList list = (System.Collections.IList)m_dataView;

			return list.IndexOf(item);
		}

		public virtual void RemoveAt(int index)
		{
			m_dataView[index].Delete();
		}

		public virtual object this[int index]
		{
			get
			{
				if ( index > m_dataView.Table.Rows.Count)
					throw new ArgumentException(string.Format(
						"Data table does not have row with given index. It has only {0} number of rows," +
						"you requested to return row number {1}",
						m_dataView.Table.Rows.Count,
						index));
				try
				{
					return m_dataView[index];
				}
				catch (InvalidOperationException )
				{
					// we sometimes get this error. Don't know why. Maybe it is multithreading issue
					// It says that internal index is corrupt, number 13.
					// Return null in this case, works fine afaik
					return null;
				}
			}
		}

		public virtual int Count
		{
			get { return m_dataView.Count; }
		}

		public virtual System.ComponentModel.PropertyDescriptorCollection GetItemProperties()
		{
			//Removed for mono compatibility
			//return System.Windows.Forms.ListBindingHelper.GetListItemProperties(mDataView);
			
			if (m_dataView == null)
				return new System.ComponentModel.PropertyDescriptorCollection(null);
			else
				return ((System.ComponentModel.ITypedList)m_dataView).GetItemProperties(null);
		}

		public System.ComponentModel.PropertyDescriptor GetItemProperty(string name, StringComparison comparison)
		{
			foreach (System.ComponentModel.PropertyDescriptor prop in GetItemProperties())
			{
				if (prop.Name.Equals(name, comparison))
					return prop;
			}

			return null;
		}

		public virtual object GetItemValue(int index, System.ComponentModel.PropertyDescriptor property)
		{
			object dataVal = property.GetValue(m_dataView[index]);

			//Convert DbNull to null
			if (System.DBNull.Value == dataVal)
				return null;
			else
				return dataVal;
		}

		public virtual void SetEditValue(System.ComponentModel.PropertyDescriptor property, object value)
		{
			//Convert the null value to DbNull
			if (value == null)
				value = System.DBNull.Value;

			if (mEditingRow == null)
				throw new DevAgeApplicationException("There isn't a row in editing state, call BeginAddNew or BeginEdit first");

			property.SetValue(mEditingRow, value);
		}

		public virtual void ApplySort(System.ComponentModel.ListSortDescriptionCollection sorts)
		{
			System.ComponentModel.IBindingListView listView = (System.ComponentModel.IBindingListView)m_dataView;

			if (sorts != null && sorts.Count > 0)
				listView.ApplySort(sorts);
			else
				listView.RemoveSort();
		}

		private bool mAllowEdit = true;
		public virtual bool AllowEdit
		{
			get { return m_dataView.AllowEdit && mAllowEdit; }
			set { mAllowEdit = value; }
		}

		private bool mAllowNew = true;
		public virtual bool AllowNew
		{
			get { return m_dataView.AllowNew && mAllowNew; }
			set { mAllowNew = value; }
		}

		private bool mAllowDelete = true;
		public virtual bool AllowDelete
		{
			get { return m_dataView.AllowDelete && mAllowDelete; }
			set { mAllowDelete = value; }
		}

		private bool mAllowSort = true;
		public virtual bool AllowSort
		{
			get { return mAllowSort; }
			set { mAllowSort = value; }
		}
	}
}
