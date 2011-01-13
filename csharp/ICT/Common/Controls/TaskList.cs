/*
 * Created by SharpDevelop.
 * User: Taylor Students
 * Date: 13/01/2011
 * Time: 11:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Xml;
using Ict.Common;

namespace Ict.Common.Controls
{
	/// <summary>
	/// Description of TaskList.
	/// </summary>
	public partial class TaskList : UserControl
	{
		//public TVisualStyle VisualStyle;
		public XmlNode MasterXmlNode 
		{ 
			set
			{
				loadTaskItems(value);
				MasterXmlNode = value;
			}
		}
		
		//Private method to load taskItems of a masterXmlNode
		private void loadTaskItems(XmlNode masterXmlNode){
			//@TODO: Implement
			while (SubTaskNode != null)
			{
				LinkLabel lblSubTaskItem = new LinkLabel();
				lblSubTaskItem.Name = SubTaskNode.Name;
//				lblSubTaskItem.Font = 
				lblSubTaskItem.Text = TLstFolderNavigation.GetLabel(SubTaskNode);
				this.pnlModule.Controls.Add(lblSubTaskItem);
				
				SubTaskNode = SubTaskNode.NextSibling;
			}
		}

		public TaskList()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//this.VisualStyle = new TVisualStyles(VisualStylesEnum.
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		public TaskList(XmlNode masterNode)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.MasterXmlNode = masterNode;
			//this.VisualStyle = new TVisualStyles(VisualStylesEnum.
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	}
}
