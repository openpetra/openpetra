using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DevAge.ComponentModel
{
    /// <summary>
    /// A class derived from BoundListBase that can be used to bind a list control (like SourceGrid) to a generic IList class.
    /// If the IList is an instance of List class then also the Sort is supported.
    /// Implement the IBoundList interface used for data binding.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class BoundList<T> : BoundListBase<T>
    {
		private IList<T> mList;

        public BoundList(IList<T> list)
        {
            mList = list;

            AllowNew = true;
            AllowDelete = true;
            AllowEdit = true;
            AllowSort = mList is List<T>;
        }

        protected override T OnAddNew()
        {
            T editItem = Activator.CreateInstance<T>();

            mList.Add(editItem);

            return editItem;
        }

        public override int IndexOf(object item)
        {
            return mList.IndexOf((T)item);
        }

        protected override void OnRemoveAt(int index)
        {
            mList.RemoveAt(index);
        }

		protected override void OnClear ()
		{
			mList.Clear();
		}

        public override object this[int index]
        {
            get { return mList[index]; }
        }

        public override int Count
        {
            get { return mList.Count; }
        }

        public override void ApplySort(System.ComponentModel.ListSortDescriptionCollection sorts)
        {
            List<T> sortableList = mList as List<T>;

            if (sortableList == null)
                throw new DevAgeApplicationException("Sort not supported, the list must be an instance of List<T>.");

            sortableList.Sort(
                delegate(T x, T y)
                {
                    foreach (System.ComponentModel.ListSortDescription sort in sorts)
                    {
                        IComparable valx = sort.PropertyDescriptor.GetValue(x) as IComparable;
                        IComparable valy = sort.PropertyDescriptor.GetValue(y) as IComparable;

                        //Swap the objects if the sort direction is Descending
                        if (sort.SortDirection == ListSortDirection.Descending)
                        {
                            IComparable tmp = valx;
                            valx = valy;
                            valy = tmp;
                        }

                        if (valx != null && valy != null)
                        {
                            int result = valx.CompareTo(valy);
                            if (result != 0)
                                return result;
                        }
                        else if (valx != null)
                            return 1;
                        else
                            return -1;
                    }

                    return 0;
                }
                );

            OnListChanged(new System.ComponentModel.ListChangedEventArgs(System.ComponentModel.ListChangedType.Reset, -1));
        }
	}
}
