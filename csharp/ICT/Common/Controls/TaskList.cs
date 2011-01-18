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
		private int NumTasks = 0;
		private System.Text.RegularExpressions.Regex TrueRegex = new Regex(".*[Tt][Rr][Uu][Ee].*");
		private System.Text.RegularExpressions.Regex TaskRegex = new Regex("^Task[0-9].*");
		//public TVisualStyle VisualStyle;
		private XmlNode InternalMasterXmlNode;
		/// <summary>
		/// Root Node for the Task List
		/// </summary>
		public XmlNode MasterXmlNode
		{
			get
			{ 
				return InternalMasterXmlNode;
			}
			set
			{
				InternalMasterXmlNode = value;
				LoadTaskItems();
			}
		}
		
		private bool InternalAutomaticNumbering;
		/// <summary>
		/// Flag whether or not to use Automatic Numbering
		/// </summary>
		public bool AutomaticNumbering
		{
			get
			{
				return InternalAutomaticNumbering;
			}
			set
			{
				InternalAutomaticNumbering = value;
				LoadTaskItems();
			}
		}
		
		private TVisualStyles InternalVisualStyle;
		/// <summary>
		/// The Visual Style to apply to the Task List
		/// </summary>
		public TVisualStyles VisualStyle
		{
			get
			{
				return InternalVisualStyle;
			}
			set
			{
				InternalVisualStyle = value;
				ChangeVisualStyle();
			}
		}
		
		//Private method to change visual style
		private void ChangeVisualStyle(){
			//Sets Title Text
			//TODO: Collapsible panel team needs to put in TitleText, TitleTextColour, HoverTitleTextColour, and TitleHeight
			
			//Sets Content Text
			this.Font = InternalVisualStyle.ContentText;
			this.ForeColor = InternalVisualStyle.ContentTextColour;
			
			//Sets Automatic Numbers & Task Indentation
			this.InternalAutomaticNumbering = InternalVisualStyle.AutomaticNumbering;
      		this.TaskIndentation = InternalVisualStyle.TaskIndentation;
      		
      		this.tPnlGradient1.GradientColorBottom = InternalVisualStyle.PanelGradientEnd;
		    this.tPnlGradient1.GradientColorTop = InternalVisualStyle.PanelGradientStart;
		    this.tPnlGradient1.GradientMode = InternalVisualStyle.PanelGradientMode;
      		
     	}
     	
/*		private void ChangeVisualStyle(){
			this.Font = InternalVisualStyle.TitleText;
			this.ForeColor = InternalVisualStyle.ContentTextColour;
			this.tPnlGradient1.GradientColorBottom = InternalVisualStyle.PanelGradientEnd;
			this.tPnlGradient1.GradientColorTop = InternalVisualStyle.PanelGradientStart;
			this.tPnlGradient1.GradientMode = InternalVisualStyle.PanelGradientMode;
			this.InternalAutomaticNumbering = InternalVisualStyle.AutomaticNumbering;
			this.TaskIndentation = InternalVisualStyle.TaskIndentation;
		
		}
*/		
		private void LoadTaskItems(){
			LoadTaskItems(this.InternalMasterXmlNode, 0, "");
			
		}
		
		//Private method to load taskItems of a masterXmlNode
		private void LoadTaskItems(XmlNode Node, int NumberingLevel, String ParentNumberText){

			//If this is the base case, reset number of Tasks and clear previously painted Task Items
			if(NumberingLevel == 0){ 
				NumTasks = 0;
				this.tPnlGradient1.Controls.Clear();
			}
			int CurrentNumbering = 1;
			NumberingLevel++;
		
			XmlNode TaskNode = Node.FirstChild;
			
			//Iterate through all children nodes of the node
			while (TaskNode != null)
			{
				//If the node is a task node...
				if(TaskRegex.IsMatch(TaskNode.Name)){
					LinkLabel lblTaskItem = new LinkLabel();
					lblTaskItem.VisitedLinkColor = InternalVisualStyle.ContentActivatedTextColour;
					lblTaskItem.LinkColor = VisualStyle.ContentTextColour;
					lblTaskItem.ActiveLinkColor = VisualStyle.ContentHoverTextColour;
					lblTaskItem.BackColor = Color.Transparent;
					lblTaskItem.Name = TaskNode.Name;
					lblTaskItem.AutoSize = true;
					lblTaskItem.Font = VisualStyle.ContentText;
					lblTaskItem.Location = new System.Drawing.Point(NumberingLevel * this.TaskIndentation,NumTasks*TaskHeight);
					lblTaskItem.LinkBehavior = LinkBehavior.HoverUnderline;
					if(VisualStyle.UseContentBackgroundColours){
						lblTaskItem.MouseEnter += new System.EventHandler(this.LinkLabelMouseEnter);
						lblTaskItem.MouseLeave += new System.EventHandler(this.LinkLabelMouseLeave);
						lblTaskItem.Click += new System.EventHandler(this.LinkLabelClicked);
					}
					//@TODO: Implement Hovering Behavior for links
						//Includes changing Link Color and Background Colr
					//@TODO: Implement Active Behavior for links
						//Includes adding or removing underline, changing background color, changing link color
					
			
					if(TaskNode.Attributes["Enabled"] != null){
						lblTaskItem.Enabled = TrueRegex.IsMatch(TaskNode.Attributes["Enabled"].Value);
					}
					
					//If the task has an attribute of hidden, don't add it to the panel or count it in Num Tasks
					if(TaskNode.Attributes["Hidden"] == null || !(TrueRegex.IsMatch(TaskNode.Attributes["Hidden"].Value))){
						
						//Automatic Numbering
						String NumberText = ParentNumberText + (CurrentNumbering).ToString() + ".";
						if(!this.InternalAutomaticNumbering)
						{
							lblTaskItem.Text = TLstFolderNavigation.GetLabel(TaskNode);
						}
						else
						{
							lblTaskItem.Text = NumberText + " " + TLstFolderNavigation.GetLabel(TaskNode);
							CurrentNumbering++;
						}
						
						this.tPnlGradient1.Controls.Add(lblTaskItem);
						NumTasks++;
						//If the TaskNode has Children, do subtasks
						if(TaskNode.HasChildNodes){
							LoadTaskItems(TaskNode,NumberingLevel,NumberText);
						}
					}

				}
				TaskNode = TaskNode.NextSibling;
			}

		}

	
		/// <summary>
		/// Constructor for creating an object with a MasterNode and setting the Visual Style
		/// </summary>
		/// <param name="MasterNode"></param>
		/// <param name="Style"></param>
		public TTaskList(XmlNode MasterNode, TVisualStylesEnum Style)
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
		
		/// <summary>
		/// Returns whether given Xml Node has the attribute hidden set to true
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool IsHidden(XmlNode node){
			return ((node.Attributes["Hidden"] != null) && TrueRegex.IsMatch(node.Attributes["Hidden"].Value));
		}
		
		/// <summary>
		/// Returns boolean whether given XmlNode has the attribute Enabled set to false
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public bool IsDisabled(XmlNode node){
			return ((node.Attributes["Enabled"] != null) && !(TrueRegex.IsMatch(node.Attributes["Enabled"].Value)));
		}
		
		/// <summary>
		/// Method to hide a task item, given an XmlNode Object
		/// Doesn't handle possible case of the node not being a descendent of the masterNode for this list
		/// </summary>
		/// <param name="node"></param>
		public void HideTaskItem(XmlNode node){
			if(node.Attributes["Hidden"] != null){
				node.Attributes["Hidden"].Value = "True";
			}
			else{
				XmlAttribute HiddenElement = node.OwnerDocument.CreateAttribute("Hidden");
				HiddenElement.Value = "True";
				node.Attributes.Append(HiddenElement);
			}
	  		LoadTaskItems();
		}
		/// <summary>
		/// Method to hide a task item, given an XmlNode Object
		/// Doesn't handle possible case of the node not being a descendent of the masterNode for this list
		/// </summary>
		/// <param name="node"></param>
		public void ShowTaskItem(XmlNode node){
			if(node.Attributes["Hidden"] != null){
				node.Attributes["Hidden"].Value = "False";
			}
			else{
				XmlAttribute HiddenElement = node.OwnerDocument.CreateAttribute("Hidden");
				HiddenElement.Value = "False";
				node.Attributes.Append(HiddenElement);
			}
	  		LoadTaskItems();
		}
		
		/// <summary>
		/// Method to hide a task item, given an XmlNode Object
		/// Doesn't handle possible case of the node not being a descendent of the masterNode for this list
		/// </summary>
		/// <param name="node"></param>
		public void DisableTaskItem(XmlNode node){
			if(node.Attributes["Enabled"] != null){
				node.Attributes["Enabled"].Value = "False";
			}
			else{
				XmlAttribute EnableAttribute = node.OwnerDocument.CreateAttribute("Enabled");
				EnableAttribute.Value = "False";
				node.Attributes.Append(EnableAttribute);
			}
	  		LoadTaskItems();
		}
		/// <summary>
		/// Method to hide a task item, given an XmlNode Object
		/// Doesn't handle possible case of the node not being a descendent of the masterNode for this list
		/// </summary>
		/// <param name="node"></param>
		public void EnableTaskItem(XmlNode node){
			if(node.Attributes["Enabled"] != null){
				node.Attributes["Enabled"].Value = "True";
			}
			else{
				XmlAttribute EnableAttribute = node.OwnerDocument.CreateAttribute("Enabled");
				EnableAttribute.Value = "True";
				node.Attributes.Append(EnableAttribute);
			}
	  		LoadTaskItems();
		}
		
		public XmlNode GetTaskByNumber(String TaskNumber){
			return GetTaskByNumber(TaskNumber, true);
		}
		
		public XmlNode GetTaskByNumber(String TaskNumber, bool IncludeHiddenElements){
			return GetTaskByNumber(TaskNumber,IncludeHiddenElements, this.InternalMasterXmlNode);
		}
		
		public XmlNode GetTaskByNumber(String TaskNumber, bool IncludeHiddenElements ,XmlNode Node){
			XmlNode TaskNode = Node.FirstChild;
			//char[] period = {'.'};
			int CurrentLevelTaskNumber = Convert.ToInt32(TaskNumber.Split('.')[0]);
			String Number2 = (TaskNumber.Split('.').Length > 1) ? TaskNumber.Split('.')[1] : null;
			String NextLevelTaskNumber = (TaskNumber.IndexOf(".") >= 0) ? TaskNumber.Substring(TaskNumber.IndexOf(".") + 1) : "";
			
			int counter = 1;
			
			//Iterate through all children nodes of the node
			while (TaskNode != null && counter <= CurrentLevelTaskNumber)
			{
				if(TaskRegex.IsMatch(TaskNode.Name)){
					if(IncludeHiddenElements || TaskNode.Attributes["Hidden"] == null || !(TrueRegex.IsMatch(TaskNode.Attributes["Hidden"].Value))){
						counter++;
					}
				}
				TaskNode = TaskNode.NextSibling;
			}
			if(TaskNode == null){
				return null;
			}
			if(NextLevelTaskNumber.Equals("")){
				return TaskNode;
			}
			else if(TaskNode.HasChildNodes){
				return GetTaskByNumber(NextLevelTaskNumber, IncludeHiddenElements, TaskNode);
			}
			return null;
		}
		
		public XmlNode GetTaskByName(String TaskName){
			return GetTaskByName(TaskName, this.MasterXmlNode);
		}
		
		public XmlNode GetTaskByName(String TaskName, XmlNode Node){
			XmlNode TaskNode = Node.FirstChild;
			//char[] period = {'.'};
						//Iterate through all children nodes of the node
			while (TaskNode != null)
			{
				//If the node is a task node
				if(TaskRegex.IsMatch(TaskNode.Name)){
					//If Task is the task being searched for
					if(TaskNode.Name.Equals(TaskName)){
						return TaskNode;
					}
					//Recursively check tasks
					if(TaskNode.HasChildNodes){
						XmlNode temp = GetTaskByName(TaskName,TaskNode);
						if(temp != null){
							return temp;
						}
					}
				}
				TaskNode = TaskNode.NextSibling;
			}
			return null;
		}
		
		//Event Listeners for MouseHover on LinkLabel
		void LinkLabelMouseEnter(object sender, System.EventArgs e)
		{
			LinkLabel lbl = (LinkLabel)sender;
			lbl.BackColor = this.VisualStyle.ContentHoverBackgroundColour;
		}
		void LinkLabelMouseLeave(object sender, System.EventArgs e)
		{
			LinkLabel lbl = (LinkLabel)sender;
			lbl.BackColor = Color.Transparent;
		}
		void LinkLabelClicked(object sender, EventArgs e)
		{
			LinkLabel lbl = (LinkLabel)sender;
			this.LoadTaskItems();//@HACK: This runs a lot of unnessecary code: just need to clear background colors for all link labels
			lbl.BackColor = this.VisualStyle.ContentActivatedBackgroundColour;
		}
	}
}

						
						
						
