using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevAge.ComponentModel
{
    /// <summary>
    /// An abstract class used for data binding. This class can be used as a base implementation of the IBoundList interface.
    /// You can use the concreate classes BoundList or BoundDataView or a custom class.
    /// To implement you own bound list class simply derive from this class and implement the abstract methods.
    /// </summary>
    [Serializable]
    public abstract class BoundListBase<T> : IBoundList
    {
        private int mEditIndex;
        private T mEditItem;
        private bool mAdding = false;
        private Dictionary<PropertyDescriptor, object> mPreviousValues = new Dictionary<PropertyDescriptor, object>();

        public int BeginAddNew()
        {
            if (mEditItem != null)
                throw new DevAgeApplicationException("There is already a row in editing state, call EndEdit first");

            mEditItem = OnAddNew();

            mEditIndex = Count - 1;

            mAdding = true;

            OnListChanged(new System.ComponentModel.ListChangedEventArgs(System.ComponentModel.ListChangedType.ItemAdded, mEditIndex));

            return mEditIndex;
        }

        public void BeginEdit(int index)
        {
            if (mEditItem != null)
                throw new DevAgeApplicationException("There is already a row in editing state, call EndEdit first");

            mEditItem = (T)this[index];
            mEditIndex = index;
        }

        public void EndEdit(bool cancel)
        {
            if (mEditItem == null)
                return;

            //Cancel edit
            if (cancel)
            {
                //Delete added value
                if (mAdding)
                    RemoveAt(mEditIndex);
                else //restore edited values
                {
                    foreach (KeyValuePair<PropertyDescriptor, object> editVal in mPreviousValues)
                    {
                        editVal.Key.SetValue(mEditItem, editVal.Value);
                    }
                }
            }
            else //Apply edit
            {
                if (mAdding)
                    mAddedItems.Add(mEditItem);
                else
                {
                    //I consider the item edited only if not already edited or if is not a new item
                    if (mEditedItems.Contains(mEditItem) == false &&
                        mAddedItems.Contains(mEditItem) == false)
                        mEditedItems.Add(mEditItem);
                }
            }


            mEditItem = default(T);
            mAdding = false;
            mEditIndex = -1;
            mPreviousValues.Clear();

            OnListChanged(new System.ComponentModel.ListChangedEventArgs(System.ComponentModel.ListChangedType.Reset, -1));
        }

        /// <summary>
        /// Gets the current edited object
        /// </summary>
        public object EditedObject
        {
            get { return mEditItem; }
        }

        public void RemoveAt(int index)
        {
            T item = (T)this[index];

            OnRemoveAt(index);

            if (mAddedItems.Contains(item))
                mAddedItems.Remove(item);
            else
            {
                if (mEditedItems.Contains(item))
                    mEditedItems.Remove(item);

                mRemovedItems.Add(item);
            }

			OnItemDeleted(new ItemDeletedEventArgs(item));
            OnListChanged(new System.ComponentModel.ListChangedEventArgs(System.ComponentModel.ListChangedType.ItemDeleted, index));
        }

		/// <summary>
		/// Clear all list items
		/// </summary>
		public void Clear()
		{
			mEditedItems.Clear();
			mAddedItems.Clear();
			mEditedItems.Clear();
			mEditItem = default(T);
			mAdding = false;
			mEditIndex = -1;
			mPreviousValues.Clear();
			OnListCleared(EventArgs.Empty);
			OnListChanged(new System.ComponentModel.ListChangedEventArgs(System.ComponentModel.ListChangedType.Reset, -1));
		}
		

        public System.ComponentModel.PropertyDescriptorCollection GetItemProperties()
        {
            return System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));
        }

        public object GetItemValue(int index, System.ComponentModel.PropertyDescriptor property)
        {
            return property.GetValue(this[index]);
        }

        public void SetEditValue(System.ComponentModel.PropertyDescriptor property, object value)
        {
            if (mEditItem == null)
                throw new DevAgeApplicationException("There isn't a row in editing state, call BeginAddNew or BeginEdit first");

            //Save the previous value to enable the restore if the user cancel the editing
            if (mPreviousValues.ContainsKey(property) == false)
                mPreviousValues.Add(property, property.GetValue(mEditItem));

            property.SetValue(mEditItem, value);

            OnListChanged(new System.ComponentModel.ListChangedEventArgs(System.ComponentModel.ListChangedType.ItemChanged, mEditIndex, property));
        }

        private bool mAllowEdit = false;
        public bool AllowEdit
        {
            get { return mAllowEdit; }
            set { mAllowEdit = value; }
        }

        private bool mAllowNew = false;
        public bool AllowNew
        {
            get { return mAllowNew; }
            set { mAllowNew = value; }
        }

        private bool mAllowDelete = false;
        public bool AllowDelete
        {
            get { return mAllowDelete; }
            set { mAllowDelete = value; }
        }

        private bool mAllowSort = false;
        /// <summary>
        /// Gets or sets if the sort is enabled. Usually is enabled only if the IList is an instance of List class
        /// </summary>
        public bool AllowSort
        {
            get { return mAllowSort; }
            set { mAllowSort = value; }
        }

        private List<T> mAddedItems = new List<T>();
        public List<T> AddedItems
        {
            get { return mAddedItems; }
        }

        private List<T> mRemovedItems = new List<T>();
        public List<T> RemovedItems
        {
            get { return mRemovedItems; }
        }

        private List<T> mEditedItems = new List<T>();
        public List<T> EditedItems
        {
            get { return mEditedItems; }
        }

        /// <summary>
        /// Get an item property by name
        /// </summary>
        /// <returns></returns>
        public System.ComponentModel.PropertyDescriptor GetItemProperty(string name, StringComparison comparison)
        {
            foreach (System.ComponentModel.PropertyDescriptor prop in GetItemProperties())
            {
                if (prop.Name.Equals(name, comparison))
                    return prop;
            }

            return null;
        }

        public event System.ComponentModel.ListChangedEventHandler ListChanged;
        protected virtual void OnListChanged(System.ComponentModel.ListChangedEventArgs e)
        {
            if (ListChanged != null)
                ListChanged(this, e);
        }

		public event EventHandler ListCleared;
		protected virtual void OnListCleared ( EventArgs e )
		{
			if ( ListCleared != null )
				ListCleared(this, e);
		}

		public event ItemDeletedEventHandler ItemDeleted;
		protected virtual void OnItemDeleted ( ItemDeletedEventArgs e )
		{
			if ( ItemDeleted != null )
				ItemDeleted(this, e);
		}

        #region Abstract methods
        /// <summary>
        /// Create a new item (row) add it at the end of the list and return the new item.
        /// </summary>
        /// <returns></returns>
        protected abstract T OnAddNew();
        /// <summary>
        /// Return the index of the specified item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract int IndexOf(object item);
        /// <summary>
        /// Remove the item at the specified position.
        /// </summary>
        /// <param name="index"></param>
        protected abstract void OnRemoveAt(int index);
        /// <summary>
        /// Remove all items.
        /// </summary>
        protected abstract void OnClear();
        /// <summary>
        /// Return the item at the specified position.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract object this[int index]
        {
            get;
        }
        /// <summary>
        /// Return the row count of the list
        /// </summary>
        public abstract int Count
        {
            get;
        }
        /// <summary>
        /// Sort the list
        /// </summary>
        /// <param name="sorts"></param>
        public abstract void ApplySort(System.ComponentModel.ListSortDescriptionCollection sorts);
		#endregion
    }
}
