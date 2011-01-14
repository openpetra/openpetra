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
using System.Text.RegularExpressions;
using Ict.Common;

namespace Ict.Common.Controls
{
	/// <summary>
	/// Description of TaskList.
	/// </summary>
	public partial class TTaskList : UserControl
	{
		private int TaskHeight = 30;
		private int TaskIndentation = 30;
		
		//public TVisualStyle VisualStyle;
		private XmlNode InternalMasterXmlNode;
		
		public XmlNode MasterXmlNode
		{
			get
			{ 
				return MasterXmlNode;
			}
			set
			{
				InternalMasterXmlNode = value;
				LoadTaskItems(InternalMasterXmlNode);
			}
		}
		
		private TVisualStyles InternalVisualStyle;
		
		public TVisualStyles VisualStyle
		{
			get
			{
				return InternalVisualStyle;
			}
			set
			{
				InternalVisualStyle = value;
				ChangeVisualStyle(InternalVisualStyle);
			}
		}
		
		//Private method to change visual style
		private void ChangeVisualStyle(TVisualStyles VisualStyle){
			this.Font = VisualStyle.TitleText;
			this.ForeColor = VisualStyle.TextColour;
			
			
			
			this.Refresh();
		
		}
		
		//Private method to load taskItems of a masterXmlNode
		private void LoadTaskItems(XmlNode MasterXmlNode){
			//@TODO: Implement
//			XmlNode ChildNode = masterXmlNode.FirstChild.FirstChild;
//			Regex TaskGroupRegex = new Regex("TaskGroup.*");
			int NumTasks = 0;
//			int NumGroups = 0;

//			int TaskGroupTitleHeight = 40;
			//Loop that iterates childNodes
/*			while(ChildNode != null){
				
				//Only runs for TaskGroup Nodes
				if(TaskGroupRegex.IsMatch(ChildNode.Name)){
					//Add Task Group Title to Control
					Label TaskGroupTitle = new Label();
					TaskGroupTitle.Text = TLstFolderNavigation.GetLabel(ChildNode);
					TaskGroupTitle.Location = new System.Drawing.Point(10,NumTasks*TaskHeight+NumGroups*TaskGroupTitleHeight);
					this.pnlModule.Controls.Add(TaskGroupTitle);
*/					
				XmlNode TaskNode = MasterXmlNode.FirstChild;//.FirstChild.FirstChild;//@TODO: Find robust way to execute this

				while (TaskNode != null)
				{
					System.Text.RegularExpressions.Regex TaskRegex = new Regex("Task[0-9].*");
					if(TaskRegex.IsMatch(TaskNode.Name)){
						LinkLabel lblTaskItem = new LinkLabel();
						lblTaskItem.LinkColor = VisualStyle.TextColour;
		//				lblTaskItem.Name = TaskNode.Name;
		//				lblTaskItem.Font =
						lblTaskItem.Location = new System.Drawing.Point(TaskIndentation,NumTasks*TaskHeight);
						lblTaskItem.Text = TLstFolderNavigation.GetLabel(TaskNode);
						this.Controls.Add(lblTaskItem);
						NumTasks++;
					}
					TaskNode = TaskNode.NextSibling;
				}
				//NumGroups++;
/*				}
				ChildNode = ChildNode.NextSibling;
			}
			this.Height = NumTasks*TaskHeight + NumGroups * TaskGroupTitleHeight + 20;
			this.pnlModule.Height = NumTasks*TaskHeight + NumGroups * TaskGroupTitleHeight + 20;
*/	
		}

		public TTaskList()
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
		
        //Constructor for creating object with MasterNode
		public TTaskList(XmlNode MasterNode)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.MasterXmlNode = MasterNode;
			
		}
		
		//Constructor for creating an object with a MasterNode and setting the Visual Style
		public TTaskList(XmlNode MasterNode, TVisualStylesEnum  Style) //should this be 
			//TVisualStylesEnum or just TVisual Styles?
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.VisualStyle = new Ict.Common.Controls.TVisualStyles(Style);
			this.MasterXmlNode = MasterNode;

			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
	}
}
