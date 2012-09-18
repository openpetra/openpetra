using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SourceGrid
{

	/// <summary>
	/// A dictionary with keys of type Control and values of type LinkedControlValue
	/// </summary>
	public class LinkedControlsList : IEnumerable<LinkedControlValue>
	{
		private Control mParent;
		public LinkedControlsList(Control parent)
		{
			mParent = parent;
		}

		private List<LinkedControlValue> mList = new List<LinkedControlValue>();

		private void RemoveFromParent(LinkedControlValue linkedControl)
		{
			mParent.Controls.Remove(linkedControl.Control);
		}
		
		public void Clear()
		{
			foreach (LinkedControlValue linkedControlValue in mList)
			{
				RemoveFromParent(linkedControlValue);
			}
			mList.Clear();
		}
		
		public void Add(LinkedControlValue linkedControl)
		{
			mList.Add(linkedControl);

			mParent.Controls.Add(linkedControl.Control);
		}

		public void Remove(LinkedControlValue linkedControl)
		{
			mList.Remove(linkedControl);
			RemoveFromParent(linkedControl);
		}

		public LinkedControlValue GetByControl(Control control)
		{
			for (int i = 0; i < mList.Count; i++)
			{
				if (control == mList[i].Control)
				{
					return mList[i];
				}
			}

			return null;
		}

		#region IEnumerable<LinkedControlValue> Members

		public IEnumerator<LinkedControlValue> GetEnumerator()
		{
			return mList.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return mList.GetEnumerator();
		}

		#endregion
	}

	/// <summary>
	/// Determine the scrolling mode of the linked controls.
	/// </summary>
	public enum LinkedControlScrollMode
	{
		None = 0,
		ScrollVertical = 1,
		ScrollHorizontal = 2,
		ScrollBoth = 3,
		BasedOnPosition = 4
	}

	/// <summary>
	/// Linked control value
	/// </summary>
	public class LinkedControlValue
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public LinkedControlValue(Control control, Position position)
		{
			mControl = control;
			mPosition = position;
			m_bUseCellBorder = true;
			mScrollMode = LinkedControlScrollMode.BasedOnPosition;
		}

		private Control mControl;
		public Control Control
		{
			get { return mControl; }
		}

		private Position mPosition;
		/// <summary>
		/// Gets or sets the position of the linked control.
		/// </summary>
		public Position Position
		{
			get{return mPosition;}
			set{mPosition = value;}
		}

		private bool m_bUseCellBorder;

		/// <summary>
		/// Gets or sets if show the cell border. True to insert the editor control inside the border of the cell, false to put the editor control over the entire cell. If you use true remember to set EnableCellDrawOnEdit == true.
		/// </summary>
		public bool UseCellBorder
		{
			get{return m_bUseCellBorder;}
			set{m_bUseCellBorder = value;}
		}

		private LinkedControlScrollMode mScrollMode;
		/// <summary>
		/// Gets or sets the scrolling mode of the control.
		/// </summary>
		public LinkedControlScrollMode ScrollMode
		{
			get{return mScrollMode;}
			set{mScrollMode = value;}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Position.ToString();
		}
	}
}
