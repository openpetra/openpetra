//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 chadds
//		 ashleyc
//
// Copyright 2004-2010 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
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
	/// GUI element that shows a stylized list of items built from an XmlNode
	/// </summary>
	public partial class TTaskList : UserControl
	{
		/// <summary>
		/// Height of a TaskList Item
		/// Used for spacing purposes
		/// </summary>
		private int TaskHeight = 30;
		
		/// <summary>
		/// Amount to indent each level of the TaskList
		/// This is changed by setting a visual style
		/// </summary>
		private int TaskIndentation = 30;
		
		/// <summary>
		/// Counter to keep track of the number of tasks and subtasks for spacing purposes
		/// </summary>
		private int NumTasks = 0;
		
		/// <summary>
		/// Regex that is used to check if strings are set to true (regardless of case)
		/// </summary>
		//@HACK: This works for now, but if the language changes in the YAML file this could break everything
		private System.Text.RegularExpressions.Regex TrueRegex = new Regex(".*[Tt][Rr][Uu][Ee].*");
		
		/// <summary>
		/// Regex that checks if XmlNodes are actual tasks
		/// Must be the word "Task" followed immediately by at least 1 number. Anything after is arbitrary
		/// </summary>
		//@HACK: This works for now, but if the language changes in the YAML file this could break things
		//Other possible solutions:
		//	1.) Find a better way to compare the names that doesn't break when language is changed
		//	2.) Make the user of the YAML file place an Attribute on each task such as "NodeType" that identifies a node as a TaskNode
		//  3.) Assume that every node is a task node (however this eliminates the possibility of having properties and such on the parent node)
		private System.Text.RegularExpressions.Regex TaskRegex = new Regex("^Task[0-9].*");

		/// <summary>
		/// Private variable that holds the MasterXmlNode for the TTaskList
		/// </summary>
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
		
		/// <summary>
		/// Method that is called from the setter for the VisualStyle property
		/// </summary>
		private void ChangeVisualStyle(){
			//Sets Title Text
			//TODO: Collapsible panel team needs to put in TitleText, TitleTextColour, HoverTitleTextColour, and TitleHeight
			
			//Sets Content Text
			this.Font = InternalVisualStyle.ContentText;
			this.ForeColor = InternalVisualStyle.ContentTextColour;
			
			//Sets Automatic Numbers & Task Indentation
			this.InternalAutomaticNumbering = InternalVisualStyle.AutomaticNumbering;
      		this.TaskIndentation = InternalVisualStyle.TaskIndentation;
      		
      		//If use content gradient
      		if(this.VisualStyle.UseContentGradient){
		  		this.tPnlGradient1.GradientColorBottom = InternalVisualStyle.ContentGradientEnd;
			    this.tPnlGradient1.GradientColorTop = InternalVisualStyle.ContentGradientStart;
			    this.tPnlGradient1.GradientMode = InternalVisualStyle.ContentGradientMode;
      		}
      		else{
      		    this.tPnlGradient1.GradientColorBottom = InternalVisualStyle.ContentBackgroundColour;
			    this.tPnlGradient1.GradientColorTop = InternalVisualStyle.ContentBackgroundColour;
			    this.tPnlGradient1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
      		}
      		
     	}
     	
		/// <summary>
		/// Default method to load task items
		/// Loads Task Items from the already specified MasterXmlNode for the task list
		/// </summary>
		private void LoadTaskItems(){
			LoadTaskItems(this.InternalMasterXmlNode, 0, "");
			
		}
		
		/// <summary>
		/// Private method to load taskItems of a masterXmlNode
		/// This method is also used to reload task items when style has been changed or a node has been disabled, etc
		/// </summary>
		/// <param name="Node"></param>
		/// <param name="NumberingLevel"></param>
		/// <param name="ParentNumberText"></param>
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
					//lblTaskItem.ActiveLinkColor = VisualStyle.ContentHoverTextColour;
					lblTaskItem.Name = TaskNode.Name;
					lblTaskItem.BackColor = Color.Transparent;
					lblTaskItem.Name = TaskNode.Name;
					lblTaskItem.AutoSize = true;
					lblTaskItem.Font = VisualStyle.ContentText;
					lblTaskItem.LinkBehavior = LinkBehavior.HoverUnderline;
					
					
					//@TODO: This line specifies the indentation by setting the location, however each level is indented the same amount
					// Should allow the first level to be indented a different amount than the rest of the levels
					lblTaskItem.Location = new System.Drawing.Point(NumberingLevel * this.TaskIndentation,NumTasks*TaskHeight);

					
					//@TODO: Implement Hovering Behavior for links
						//Includes changing Link Color
						//Background color for hovering is already implemented
					//@TODO: Implement Active Behavior for links
						//Includes adding or removing underline, changing link color
						//Background color for link is already implemented
					if(VisualStyle.UseContentBackgroundColours){
						lblTaskItem.MouseEnter += new System.EventHandler(this.LinkLabelMouseEnter);
						lblTaskItem.MouseLeave += new System.EventHandler(this.LinkLabelMouseLeave);
						lblTaskItem.Click += new System.EventHandler(this.LinkLabelClicked);
						if(TaskNode.Attributes["Active"] != null && TrueRegex.IsMatch(TaskNode.Attributes["Active"].Value)){
							lblTaskItem.BackColor = this.VisualStyle.ContentActivatedBackgroundColour;
						}
					}
					if(TaskNode.Attributes["Active"] != null && TrueRegex.IsMatch(TaskNode.Attributes["Active"].Value)){
					    lblTaskItem.LinkColor = VisualStyle.ContentActivatedTextColour;
					}
					else{
					    lblTaskItem.LinkColor = VisualStyle.ContentTextColour;
					}

			
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
		/// Default constructor necessary for TTaskList to work with the GUI designer
		/// Once the control is on a form, the designer should convert this constructor to the parameterized constructor
		/// </summary>
		public TTaskList(){
		    
		}
	
		/// <summary>
		/// Constructor for creating an object with a MasterNode and setting the Visual Style
		/// </summary>
		/// <param name="MasterNode">Base Node for the TaskList</param>
		/// <param name="Style">A TVisualStylesEnum value that specifies the visual style which should be used</param>
		public TTaskList(XmlNode MasterNode, TVisualStylesEnum Style)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.VisualStyle = new Ict.Common.Controls.TVisualStyles(Style);
			this.MasterXmlNode = MasterNode;
		}
		
		/// <summary>
		/// Returns whether given Xml Node has the attribute hidden set to true
		/// </summary>
		/// <param name="node"></param>
		/// <returns>Boolean whether given Xml Node has the attribute hidden set to true</returns>
		public bool IsHidden(XmlNode node){
			return ((node.Attributes["Hidden"] != null) && TrueRegex.IsMatch(node.Attributes["Hidden"].Value));
		}
		
		/// <summary>
		/// Returns boolean whether given XmlNode has the attribute Enabled set to false
		/// </summary>
		/// <param name="node"></param>
		/// <returns>Boolean whether given Xml Node has the attribute enabled set to false</returns>
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
		
		/// <summary>
		/// Method to retrieve a TaskNode based on a position in the MasterXmlNode for this TTaskList
		/// This position includes any hidden task items
		/// </summary>
		/// <param name="TaskNumber">A String that specifies the position of the desired element
		/// 	"3.1" means the first child of the third element</param>
		/// <returns>XmlNode at specified position or null if there is no element at that position</returns>
		public XmlNode GetTaskByNumber(String TaskNumber){
			return GetTaskByNumber(TaskNumber, true);
		}
		
		/// <summary>
		/// Method to retrieve a TaskNode based on a position in the MasterXmlNode for this TTaskList
		/// </summary>
		/// <param name="TaskNumber">A String that specifies the position of the desired element
		/// 	"3.1" means the first child of the third element
		/// </param>
		/// <param name="IncludeHiddenElements">Flag whether to include task items which are specified hidden</param>
		/// <returns>XmlNode at specified position or null if there is no element at that position</returns>
		public XmlNode GetTaskByNumber(String TaskNumber, bool IncludeHiddenElements){
			return GetTaskByNumber(TaskNumber,IncludeHiddenElements, this.InternalMasterXmlNode);
		}
		
		/// <summary>
		/// Method to retrieve a TaskNode based on a position in the list
		/// </summary>
		/// <param name="TaskNumber">A String that specifies the position of the desired element
		/// 	"3.1" means the first child of the third element
		/// </param>
		/// <param name="IncludeHiddenElements">Flag whether to include task items which are specified hidden</param>
		/// <param name="Node">The root from which to start the search</param>
		/// <returns>XmlNode at the specified position or null if there is no element at that position</returns>
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
		
		/// <summary>
		/// Method to retrieve an XmlNode which is a child of this TTaskList's MasterXmlNode
		/// </summary>
		/// <param name="TaskName">The Name of the desired XmlNode</param>
		/// <returns>XmlNode Reference</returns>
		public XmlNode GetTaskByName(String TaskName){
			return GetTaskByName(TaskName, this.MasterXmlNode);
		}
		
		/// <summary>
		/// Method to retrieve an XmlNode which is a child of the specified XmlNode
		/// </summary>
		/// <param name="TaskName">The Name of the desired XmlNode</param>
		/// <param name="Node">The reference to the XmlNode whose descendents should be searched</param>
		/// <returns>XmlNode Reference</returns>
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
		
		/// <summary>
		/// Clears all attributes of a certain type for all descendents of the specified node
		/// </summary>
		/// <param name="AttributeType">The name of the attribute type</param>
		/// <param name="Node">The root node for the tree which will be cleared</param>
		public void ClearAllAttributeOfType(String AttributeType, XmlNode Node){
						XmlNode TaskNode = Node.FirstChild;
			//char[] period = {'.'};
						//Iterate through all children nodes of the node
			while (TaskNode != null)
			{
				//If the node is a task node
				if(TaskRegex.IsMatch(TaskNode.Name)){
					//If Task is the task being searched for
					if(TaskNode.Attributes["Active"] != null){
						TaskNode.Attributes.Remove(TaskNode.Attributes["Active"]);
					}
					//Recursively check tasks
					if(TaskNode.HasChildNodes){
						ClearAllAttributeOfType(AttributeType,TaskNode);
					}
				}
				TaskNode = TaskNode.NextSibling;
			}
		}
		
		//Event Listeners for MouseHover on LinkLabel
		/// <summary>
		/// Event Listener for changing a TaskList item when the mouse hovers
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void LinkLabelMouseEnter(object sender, System.EventArgs e)
		{
		    LinkLabel lbl = (LinkLabel)sender;
		    if(this.VisualStyle.UseContentBackgroundColours){
			    lbl.BackColor = this.VisualStyle.ContentHoverBackgroundColour;
		    }
		    else{
		        lbl.LinkColor = this.VisualStyle.ContentHoverTextColour;
		    }
		}
		/// <summary>
		/// Event Listener for reverting TaskList item style when the mouse leaves
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void LinkLabelMouseLeave(object sender, System.EventArgs e)
		{
			LinkLabel lbl = (LinkLabel)sender;
			XmlNode node = this.GetTaskByName(lbl.Name);
			if(node.Attributes["Active"] != null && TrueRegex.IsMatch(node.Attributes["Active"].Value)){
			    if(this.VisualStyle.UseContentBackgroundColours){
                    lbl.BackColor = this.VisualStyle.ContentActivatedBackgroundColour;
			    }
			    lbl.LinkColor = this.VisualStyle.ContentHoverTextColour;
			}
			else{
			    if(this.VisualStyle.UseContentBackgroundColours){
				    lbl.BackColor = Color.Transparent;
			    }
			    lbl.LinkColor = this.VisualStyle.ContentTextColour;
			}
		}
		
		/// <summary>
		/// Event listener to change TaskList item style when the item is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void LinkLabelClicked(object sender, EventArgs e)
		{
			LinkLabel lbl = (LinkLabel)sender;
			ClearAllAttributeOfType("Active",this.MasterXmlNode);
			XmlNode node = this.GetTaskByName(lbl.Name);
			if(node.Attributes["Active"] != null){
				node.Attributes["Active"].Value = "True";
			}
			else{
				XmlAttribute ActiveAttribute = node.OwnerDocument.CreateAttribute("Active");
				ActiveAttribute.Value = "True";
				node.Attributes.Append(ActiveAttribute);
			}
			this.LoadTaskItems();//@HACK: This runs a lot of unnessecary code: just need to clear background colors for all link labels
			
		}
	}
}

						
						
						
