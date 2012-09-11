using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace SourceGrid
{
	/// <summary>
	/// Summary description for ListEditor.
	/// </summary>
	public class ListEditor : System.Windows.Forms.UserControl
	{
		private Grid grid;
		private System.Windows.Forms.Button btDown;
        private System.Windows.Forms.Button btUp;
        private System.Windows.Forms.Button btRemove;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btrRefreshList;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ListEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			grid.Selection.FocusRowLeaving += new RowCancelEventHandler(Selection_FocusRowLeaving);
			grid.Selection.FocusRowEntered += new RowEventHandler(Selection_FocusRowEntered);
            grid.Selection.FocusStyle = FocusStyle.None;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ListEditor));
			this.grid = new Grid();
            this.btDown = new System.Windows.Forms.Button();
            this.btUp = new System.Windows.Forms.Button();
            this.btRemove = new System.Windows.Forms.Button();
            this.btAdd = new System.Windows.Forms.Button();
            this.btrRefreshList = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// grid
			// 
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grid.AutoStretchColumnsToFitWidth = false;
			this.grid.AutoStretchRowsToFitHeight = false;
			this.grid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.grid.CustomSort = false;
			this.grid.Location = new System.Drawing.Point(4, 32);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(332, 216);
			this.grid.SpecialKeys = GridSpecialKeys.Default;
			this.grid.TabIndex = 0;
			// 
			// btDown
			// 
			this.btDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btDown.Enabled = false;
			this.btDown.Image = ((System.Drawing.Image)(resources.GetObject("btDown.Image")));
			this.btDown.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btDown.Location = new System.Drawing.Point(312, 4);
			this.btDown.Name = "btDown";
			this.btDown.Size = new System.Drawing.Size(24, 23);
			this.btDown.TabIndex = 1;
			this.btDown.Click += new System.EventHandler(this.btDown_Click);
			// 
			// btUp
			// 
			this.btUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btUp.Enabled = false;
			this.btUp.Image = ((System.Drawing.Image)(resources.GetObject("btUp.Image")));
            this.btUp.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btUp.Location = new System.Drawing.Point(284, 4);
			this.btUp.Name = "btUp";
			this.btUp.Size = new System.Drawing.Size(24, 23);
			this.btUp.TabIndex = 2;
			this.btUp.Click += new System.EventHandler(this.btUp_Click);
			// 
			// btRemove
			// 
			this.btRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btRemove.Enabled = false;
			this.btRemove.Image = ((System.Drawing.Image)(resources.GetObject("btRemove.Image")));
            this.btRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btRemove.Location = new System.Drawing.Point(220, 4);
			this.btRemove.Name = "btRemove";
			this.btRemove.Size = new System.Drawing.Size(24, 23);
			this.btRemove.TabIndex = 3;
			this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
			// 
			// btAdd
			// 
			this.btAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btAdd.Image = ((System.Drawing.Image)(resources.GetObject("btAdd.Image")));
            this.btAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btAdd.Location = new System.Drawing.Point(192, 4);
			this.btAdd.Name = "btAdd";
			this.btAdd.Size = new System.Drawing.Size(24, 23);
			this.btAdd.TabIndex = 4;
			this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
			// 
			// btrRefreshList
			// 
			this.btrRefreshList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btrRefreshList.Image = ((System.Drawing.Image)(resources.GetObject("btrRefreshList.Image")));
            this.btrRefreshList.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.btrRefreshList.Location = new System.Drawing.Point(252, 4);
			this.btrRefreshList.Name = "btrRefreshList";
			this.btrRefreshList.Size = new System.Drawing.Size(24, 23);
			this.btrRefreshList.TabIndex = 5;
			this.btrRefreshList.Click += new System.EventHandler(this.btrRefreshList_Click);
			// 
			// ListEditor
			// 
			this.Controls.Add(this.btrRefreshList);
			this.Controls.Add(this.btAdd);
			this.Controls.Add(this.btRemove);
			this.Controls.Add(this.btUp);
			this.Controls.Add(this.btDown);
			this.Controls.Add(this.grid);
			this.Name = "ListEditor";
			this.Size = new System.Drawing.Size(340, 252);
			this.ResumeLayout(false);

		}
		#endregion

		private ArrayList m_List;

		public ArrayList List
		{
			get{return m_List;}
			set{m_List = value;}
		}

		private Type m_ItemType;

		public Type ItemType
		{
			get{return m_ItemType;}
			set{m_ItemType = value;LoadEditors();}
		}

		private Cells.Editors.EditorBase[] m_Editors;

		[Browsable(false)]
		public Cells.Editors.EditorBase[] Editors
		{
			get{return m_Editors;}
			set{m_Editors = value;}
		}

		private System.Reflection.PropertyInfo[] m_Properties;

		[Browsable(false)]
		public System.Reflection.PropertyInfo[] Properties
		{
			get{return m_Properties;}
			set{m_Properties = value;}
		}

		private void LoadEditors()
		{
			if (m_ItemType != null)
			{
				m_Properties = m_ItemType.GetProperties();

				ArrayList l_Tmp = new ArrayList();
				//remove not Browsable attribute
				for (int i = 0; i < m_Properties.Length; i++)
				{
					AttributeCollection attributes = 
						TypeDescriptor.GetProperties(m_ItemType)[m_Properties[i].Name].Attributes;
 
					// Checks to see if the value of the BrowsableAttribute is Yes.
					if(attributes[typeof(BrowsableAttribute)].Equals(BrowsableAttribute.Yes)) 
					{
						l_Tmp.Add(m_Properties[i]);
					}
				}

				m_Editors = new Cells.Editors.EditorBase[l_Tmp.Count];
				m_Properties = new System.Reflection.PropertyInfo[l_Tmp.Count];

				for (int i = 0; i < l_Tmp.Count; i++)
				{
					System.Reflection.PropertyInfo prop = (System.Reflection.PropertyInfo)l_Tmp[i];
					m_Properties[i] = prop;
                    m_Editors[i] = Cells.Editors.Factory.Create(prop.PropertyType);
					if (m_Editors[i] != null)
						m_Editors[i].EnableEdit = prop.CanWrite;
				}
			}
			else
				m_Editors = null;
		}

		public void LoadList()
		{
			if (m_ItemType == null)
				throw new ApplicationException("ItemType is null");
			if (m_List == null)
				m_List = new ArrayList();

			if (m_Properties.Length != m_Editors.Length)
				throw new ApplicationException("Properteis.Length != Editors.Length");

			grid.FixedRows = 1;
			grid.FixedColumns = 0;
			grid.Redim(m_List.Count+grid.FixedRows, m_Properties.Length+grid.FixedColumns);

			//HeaderCell
			//grid[0,0] = new Cells.Header();
			for (int i = 0; i < m_Properties.Length; i++)
			{
				Cells.ColumnHeader l_Header = new Cells.ColumnHeader(m_Properties[i].Name);
				grid[0, i+grid.FixedColumns] = l_Header;

				l_Header.AutomaticSortEnabled = false;
//				//If the column type support the IComparable then I can use the value to sort the column, otherwise I use the string representation for sort.
//				if (typeof(IComparable).IsAssignableFrom(m_Properties[i].PropertyType))
//					l_Header.Comparer = new ValueCellComparer();
//				else
//					l_Header.Comparer = new DisplayStringCellComparer();
			}

			for (int r = 0; r < m_List.Count; r++)
			{
				PopulateRow(r+grid.FixedRows, m_List[r]);
			}

			grid.AutoStretchColumnsToFitWidth = true;
            grid.AutoSizeCells();
		}

		private void PopulateRow(int p_GridRow, object p_Object)
		{
			for (int c = 0; c < m_Properties.Length; c++)
			{
				PopulateCell(p_GridRow, c+grid.FixedColumns, m_Properties[c], m_Editors[c], p_Object);
			}
			grid.Rows[p_GridRow].Tag = p_Object;
		}

		private void PopulateCell(int p_GridRow, int p_GridCol, System.Reflection.PropertyInfo p_PropInfo, Cells.Editors.EditorBase p_Editor, object p_Object)
		{
			grid[p_GridRow, p_GridCol] = new Cells.Cell(p_PropInfo.GetValue(p_Object,null));
			grid[p_GridRow, p_GridCol].Editor = p_Editor;

			Cells.Controllers.BindProperty l_Bind = new Cells.Controllers.BindProperty(p_PropInfo, p_Object);
			grid[p_GridRow, p_GridCol].AddController(l_Bind);

			Cells.Controllers.CustomEvents l_CustomEvents = new Cells.Controllers.CustomEvents();
			l_CustomEvents.ValueChanged += new EventHandler(Grid_ValueChanged);
			grid[p_GridRow, p_GridCol].AddController(l_CustomEvents);
		}

		private void btAdd_Click(object sender, System.EventArgs e)
		{
			try
			{
				int l_Row = grid.RowsCount;
				grid.Rows.Insert(l_Row);
				object l_Obj = Activator.CreateInstance(m_ItemType);
				m_List.Add(l_Obj);
				PopulateRow(l_Row, l_Obj);

				OnListChanged(EventArgs.Empty);
			}
			catch(Exception err)
			{
				DevAge.Windows.Forms.ErrorDialog.Show(this,err, "Error");
			}
		}

		public event EventHandler ListChanged;

		protected virtual void OnListChanged(EventArgs e)
		{
			if (ListChanged!=null)
				ListChanged(this, e);
		}

		private void Grid_ValueChanged(object sender, EventArgs e)
		{
			OnListChanged(e);
		}

		private void btRemove_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (grid.Selection.ActivePosition.IsEmpty() == false &&
					grid.Selection.ActivePosition.Row >= grid.FixedRows)
				{
					m_List.Remove(grid.Rows[grid.Selection.ActivePosition.Row].Tag);
					grid.Rows.Remove(grid.Selection.ActivePosition.Row);

					OnListChanged(EventArgs.Empty);
				}
			}
			catch(Exception err)
			{
				DevAge.Windows.Forms.ErrorDialog.Show(this,err, "Error");
			}
		}

		private void btrRefreshList_Click(object sender, System.EventArgs e)
		{
			try
			{
				LoadList();
			}
			catch(Exception err)
			{
				DevAge.Windows.Forms.ErrorDialog.Show(this,err, "Error");
			}
		}

		private void btUp_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (grid.Selection.ActivePosition.IsEmpty() == false &&
					grid.Selection.ActivePosition.Row >= grid.FixedRows)
				{
					object tag = grid.Rows[grid.Selection.ActivePosition.Row].Tag;
					int gridRow = grid.Selection.ActivePosition.Row;
					int listIndex = m_List.IndexOf(tag);
					m_List.Remove(tag);
					m_List.Insert(listIndex-1, tag);
					grid.Rows.Move(gridRow, gridRow-1);

					grid.Selection.FocusRow(gridRow - 1);

					OnListChanged(EventArgs.Empty);
				}
			}
			catch(Exception err)
			{
				DevAge.Windows.Forms.ErrorDialog.Show(this,err, "Error");
			}
		}

		private void btDown_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (grid.Selection.ActivePosition.IsEmpty() == false &&
					grid.Selection.ActivePosition.Row >= grid.FixedRows &&
					grid.Selection.ActivePosition.Row < grid.Rows.Count - 1)
				{
					object tag = grid.Rows[grid.Selection.ActivePosition.Row].Tag;
					int gridRow = grid.Selection.ActivePosition.Row;
					int listIndex = m_List.IndexOf(tag);
					m_List.Remove(tag);
					m_List.Insert(listIndex+1, tag);
					grid.Rows.Move(gridRow, gridRow+1);

					grid.Selection.FocusRow(gridRow+1);

					OnListChanged(EventArgs.Empty);
				}
			}
			catch(Exception err)
			{
				DevAge.Windows.Forms.ErrorDialog.Show(this,err, "Error");
			}
		}

		public bool EnableAdd
		{
			get{return btAdd.Visible;}
			set{btAdd.Visible = value;}
		}
		public bool EnableRemove
		{
			get{return btRemove.Visible;}
			set{btRemove.Visible = value;}
		}
		public bool EnableRefresh
		{
			get{return btrRefreshList.Visible;}
			set{btrRefreshList.Visible = value;}
		}
		public bool EnableMove
		{
			get{return btUp.Visible;}
			set{btUp.Visible = value;btDown.Visible = value;}
		}

		private void Selection_FocusRowLeaving(object sender, RowCancelEventArgs e)
		{
			btDown.Enabled = false;
			btUp.Enabled = false;
			btRemove.Enabled = false;
		}

		private void Selection_FocusRowEntered(object sender, RowEventArgs e)
		{
			btDown.Enabled = true;
			btUp.Enabled = true;
			btRemove.Enabled = true;
		}
	}
}
