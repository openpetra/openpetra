using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
	/// <summary>
	/// Summary description for ButtonMultiSelection.
	/// </summary>
	[DefaultEvent("Click")]
	public class ButtonMultiSelection : System.Windows.Forms.UserControl, IButtonControl
	{
		private System.Windows.Forms.Button btMain;
		private DevAge.Windows.Forms.DropDownButton btArrow;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ButtonMultiSelection()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			base.BackColor = Color.Transparent;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ButtonMultiSelection));
            this.btMain = new System.Windows.Forms.Button();
			this.btArrow = new DevAge.Windows.Forms.DropDownButton();
			this.SuspendLayout();
			// 
			// btMain
			// 
			this.btMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btMain.Location = new System.Drawing.Point(0, 0);
			this.btMain.Name = "btMain";
			this.btMain.Size = new System.Drawing.Size(61, 23);
			this.btMain.TabIndex = 0;
			this.btMain.Text = "button1";
			this.btMain.Click += new System.EventHandler(this.btMain_Click);
			// 
			// btArrow
			// 
			this.btArrow.Dock = System.Windows.Forms.DockStyle.Right;
			this.btArrow.Location = new System.Drawing.Point(61, 0);
			this.btArrow.Name = "btArrow";
			this.btArrow.Size = new System.Drawing.Size(14, 23);
			this.btArrow.TabIndex = 1;
			this.btArrow.Click += new System.EventHandler(this.btArrow_Click);
			// 
			// ButtonMultiSelection
			// 
			this.Controls.Add(this.btMain);
			this.Controls.Add(this.btArrow);
			this.Name = "ButtonMultiSelection";
			this.Size = new System.Drawing.Size(75, 23);
			this.ResumeLayout(false);

		}
		#endregion

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get
			{
				return btMain.Text;
			}
			set
			{
				btMain.Text = value;
				base.Text = value;
			}
		}

		[Browsable(false)]
		public new Color BackColor
		{
			get{return base.BackColor;}
			set{base.BackColor = value;}
		}	

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				btMain.ForeColor = value;
				btArrow.ForeColor = value;
				base.ForeColor = value;
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				btMain.Font = value;
				btArrow.Font = value;
				base.Font = value;
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual System.Drawing.ContentAlignment TextAlign
		{
			get{return btMain.TextAlign;}
			set{btMain.TextAlign = value;}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual Image Image
		{
			get{return btMain.Image;}
			set{btMain.Image = value;}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public virtual System.Drawing.ContentAlignment ImageAlign
		{
			get{return btMain.ImageAlign;}
			set{btMain.ImageAlign = value;}
		}
	
		#region IButtonControl Members

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public virtual DialogResult DialogResult
		{
			get{return btMain.DialogResult;}
			set{btMain.DialogResult = value;}
		}

		public void PerformClick()
		{
			btMain.PerformClick();
		}

		public void NotifyDefault(bool value)
		{
			btMain.NotifyDefault(value);
		}

		#endregion

		private event SubButtonItemEventHandler m_Click;

		public new event SubButtonItemEventHandler Click
		{
			add{m_Click+=value;}
			remove{m_Click-=value;}
		}

		protected virtual void OnClick(SubButtonItemEventArgs e)
		{
			if (e.ButtonItem!=null)
				e.ButtonItem.InvokeItemClick(EventArgs.Empty);

			if (m_Click != null)
				m_Click(this,e);
		}
		
		internal void InvokeSubItemClick(SubButtonItemEventArgs e)
		{
			OnClick(e);
		}

		private void btMain_Click(object sender, System.EventArgs e)
		{
			if (m_Mode==ButtonMultiSelectionMode.InvokeFirstAction)
			{
				if (m_ButtonsItems!=null && m_ButtonsItems.Count>0)
					OnClick(new SubButtonItemEventArgs(m_ButtonsItems[0]));
				else
					OnClick(new SubButtonItemEventArgs(null));
			}
			else
			{
				btArrow_Click(sender,e);
			}
		}

		private ContextMenu l_ContextMenu = new ContextMenu();

		private void btArrow_Click(object sender, System.EventArgs e)
		{
			if (m_ButtonsItems!=null && m_ButtonsItems.Count>0)
			{
				l_ContextMenu.MenuItems.Clear();
				foreach(SubButtonItem b in m_ButtonsItems)
				{
					b.Owner = this;
					l_ContextMenu.MenuItems.Add(b.m_MenuItem);
				}

				l_ContextMenu.Show(btMain,new Point(0,btMain.Height));
			}
		}

		private SubButtonItemCollection m_ButtonsItems = new SubButtonItemCollection();

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public SubButtonItemCollection ButtonsItems
		{
			get{return m_ButtonsItems;}
			set{m_ButtonsItems = value;}
		}

		private ButtonMultiSelectionMode m_Mode = ButtonMultiSelectionMode.InvokeFirstAction;
		[DefaultValue(ButtonMultiSelectionMode.InvokeFirstAction)]
		public ButtonMultiSelectionMode Mode
		{
			get{return m_Mode;}
			set{m_Mode = value;}
		}
	}

	#region Support Classes

	public enum ButtonMultiSelectionMode
	{
		InvokeFirstAction = 0,
		ShowContextMenu = 1
	}

	public class SubButtonItemEventArgs : EventArgs
	{
		private SubButtonItem m_SubItem;
		public SubButtonItem ButtonItem
		{
			get{return m_SubItem;}
			set{m_SubItem = value;}
		}

		public SubButtonItemEventArgs(SubButtonItem p_Item)
		{
			m_SubItem = p_Item;
		}

	}

	public delegate void SubButtonItemEventHandler(object sender, SubButtonItemEventArgs e);

	public class SubButtonItem
	{
		public SubButtonItem():this("NewItem")
		{
		}
		public SubButtonItem(string p_Text):this(p_Text,(EventHandler)null)
		{
		}

		public SubButtonItem(string p_Text, EventHandler p_Event):this(p_Text, p_Event, null)
		{
		}

		public SubButtonItem(string p_Text, EventHandler p_Event, Image p_Image)
		{
			if (p_Image == null)
				m_MenuItem = new MenuItem();
			else
			{
				m_MenuItem = new MenuItem();
                //TODO add the image
			}

			DefConstructor(p_Text, p_Event);
		}

		public SubButtonItem(string p_Text, EventHandler p_Event, ImageList p_ImageList, int p_ImageIndex)
		{
			if (p_ImageList == null)
				m_MenuItem = new MenuItem();
			else
			{
				m_MenuItem = new MenuItem();
                //TODO add the image
            }

			DefConstructor(p_Text, p_Event);
		}

		private void DefConstructor(string p_Text, EventHandler p_Event)
		{
			m_MenuItem.Click+=new EventHandler(Menu_Click);

			Text = p_Text;
			if (p_Event!=null)
				Click += p_Event;
		}

		internal MenuItem m_MenuItem;

		private object m_Tag;
		public object Tag
		{
			get{return m_Tag;}
			set{m_Tag = value;}
		}

		public string Text
		{
			get{return m_MenuItem.Text;}
			set{m_MenuItem.Text = value;}
		}

		private event EventHandler m_Click;

		public void InvokeItemClick(EventArgs e)
		{
			if (m_Click!=null)
				m_Click(this,e);
		}

		public event EventHandler Click
		{
			add{m_Click += value;}
			remove{m_Click -= value;}
		}

		private ButtonMultiSelection m_ParentButton;
		public ButtonMultiSelection Owner
		{
			get{return m_ParentButton;}
			set{m_ParentButton = value;}
		}


		private void Menu_Click(object sender, EventArgs e)
		{
			if (m_ParentButton!=null)
				m_ParentButton.InvokeSubItemClick(new SubButtonItemEventArgs(this));
		}
	}


	/// <summary>
	/// A collection of elements of type SubButtonItem
	/// </summary>
	public class SubButtonItemCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the SubButtonItemCollection class.
		/// </summary>
		public SubButtonItemCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the SubButtonItemCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new SubButtonItemCollection.
		/// </param>
		public SubButtonItemCollection(SubButtonItem[] items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the SubButtonItemCollection class, containing elements
		/// copied from another instance of SubButtonItemCollection
		/// </summary>
		/// <param name="items">
		/// The SubButtonItemCollection whose elements are to be added to the new SubButtonItemCollection.
		/// </param>
		public SubButtonItemCollection(SubButtonItemCollection items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this SubButtonItemCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this SubButtonItemCollection.
		/// </param>
		public virtual void AddRange(SubButtonItem[] items)
		{
			foreach (SubButtonItem item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another SubButtonItemCollection to the end of this SubButtonItemCollection.
		/// </summary>
		/// <param name="items">
		/// The SubButtonItemCollection whose elements are to be added to the end of this SubButtonItemCollection.
		/// </param>
		public virtual void AddRange(SubButtonItemCollection items)
		{
			foreach (SubButtonItem item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type SubButtonItem to the end of this SubButtonItemCollection.
		/// </summary>
		/// <param name="value">
		/// The SubButtonItem to be added to the end of this SubButtonItemCollection.
		/// </param>
		public virtual void Add(SubButtonItem value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic SubButtonItem value is in this SubButtonItemCollection.
		/// </summary>
		/// <param name="value">
		/// The SubButtonItem value to locate in this SubButtonItemCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this SubButtonItemCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(SubButtonItem value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this SubButtonItemCollection
		/// </summary>
		/// <param name="value">
		/// The SubButtonItem value to locate in the SubButtonItemCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(SubButtonItem value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the SubButtonItemCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the SubButtonItem is to be inserted.
		/// </param>
		/// <param name="value">
		/// The SubButtonItem to insert.
		/// </param>
		public virtual void Insert(int index, SubButtonItem value)
		{
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the SubButtonItem at the given index in this SubButtonItemCollection.
		/// </summary>
		public virtual SubButtonItem this[int index]
		{
			get
			{
				return (SubButtonItem) this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific SubButtonItem from this SubButtonItemCollection.
		/// </summary>
		/// <param name="value">
		/// The SubButtonItem value to remove from this SubButtonItemCollection.
		/// </param>
		public virtual void Remove(SubButtonItem value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by SubButtonItemCollection.GetEnumerator.
		/// </summary>
		public class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(SubButtonItemCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public SubButtonItem Current
			{
				get
				{
					return (SubButtonItem) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (SubButtonItem) (this.wrapped.Current);
				}
			}

			public bool MoveNext()
			{
				return this.wrapped.MoveNext();
			}

			public void Reset()
			{
				this.wrapped.Reset();
			}
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the elements of this SubButtonItemCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual SubButtonItemCollection.Enumerator GetEnumerator()
		{
			return new SubButtonItemCollection.Enumerator(this);
		}
	}

	#endregion
}
