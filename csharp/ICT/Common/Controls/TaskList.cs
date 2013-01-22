//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 chadds
//		 ashleyc
//       sethb
//       christiank
//
// Copyright 2004-2012 by OM International
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Text.RegularExpressions;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Common.Controls
{
    /// <summary>
    /// Displays a stylised list of <see cref="LinkLabel" />s that are built from an <see cref="XmlNode" />.
    /// </summary>
    /// <remarks>
    /// Used in OpenPetra Main Menu's Module Navigation to present the available Submodules.
    ///</remarks>
    public partial class TTaskList : UserControl
    {
        #region global settings

        /// <summary>
        /// Height of a TaskList Item
        /// Used for spacing purposes
        /// This is changed by setting a visual style
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
        private System.Text.RegularExpressions.Regex TrueRegex = new Regex(".*[Tt][Rr][Uu][Ee].*");

        /// <summary>
        /// Regex that is used to check if strings are set to false (regardless of case)
        /// </summary>
        private System.Text.RegularExpressions.Regex FalseRegex = new Regex(".*[Ff][Aa][Ll][Ss][Ee].*");

        /// <summary>
        /// Regex that checks if XmlNodes are actual tasks
        /// Must be the word "Task" followed immediately by at least 1 number. Anything after is arbitrary
        /// </summary>
        //Other possible solutions:
        //	1.) Find a better way to compare the names that doesn't break when language is changed
        //	2.) Make the user of the YAML file place an Attribute on each task such as "NodeType" that identifies a node as a TaskNode
        //  3.) Assume that every node is a task node (however this eliminates the possibility of having properties and such on the parent node)
        private System.Text.RegularExpressions.Regex TaskRegex = new Regex("^Task[0-9].*");

        /// <summary>
        /// Private variable that holds the MasterXmlNode for the TTaskList
        /// </summary>
        private XmlNode InternalMasterXmlNode;

        /// <summary/>
        private const TVisualStylesEnum DEFAULT_STYLE = TVisualStylesEnum.vsShepherd;

        private XmlNode FActiveTaskItem = null;

        private int FTaskListMaxHeight = 0;

        private Dictionary <XmlNode, LinkLabel>FXmlNodeToLinkLabelMapping;

        #endregion

        #region Events (and related methods)

        /// <summary>
        /// Contains data about a Link that got clicked by the user.
        /// </summary>
        public delegate void TaskLinkClicked(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked);

        /// <summary>Fired when a TaskLink got activated (by clicking on it or programmatically).</summary>
        public event TaskLinkClicked ItemActivation;

        private void OnItemActivation(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked)
        {
            if (ItemActivation != null)
            {
                ItemActivation(ATaskList, ATaskListNode, AItemClicked);
            }
        }

        #endregion

        #region Properties (and related functions)

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
                LoadTaskItems(true);
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
        /// Maximum Height that the TaskList needs to be displayed fully. Available only after
        /// <see cref="MasterXmlNode" /> has been set.
        /// </summary>
        public int TaskListMaxHeight
        {
            get
            {
                return FTaskListMaxHeight;
            }
        }

        /// <summary>
        /// Active Task Item.
        /// </summary>
        /// <remarks>Setting this Property to null has the effect that any ActiveTaskItem
        /// will be un-set, i.e. there will be no ActiveTaskItem.</remarks>
        public XmlNode ActiveTaskItem
        {
            get
            {
                return FActiveTaskItem;
            }

            set
            {
                if (value != null)
                {
                    if (!IsDisabled(value))
                    {
                        LinkLabel MatchingLabel = GetLinkLabelForXmlNode(value);

                        if (MatchingLabel != null)
                        {
                            lblTaskItem_LinkClicked(MatchingLabel, new LinkLabelLinkClickedEventArgs(MatchingLabel.Links[0]));
                        }
                    }
                }
                else
                {
                    FActiveTaskItem = null;

                    RemoveActivatedLinkAppearenceFromNonActivated();
                }
            }
        }

        /// <summary>
        /// Method that is called from the setter for the VisualStyle property
        /// </summary>
        private void ChangeVisualStyle()
        {
            //Sets Title Text
            //TODO: Collapsible panel team needs to put in TitleFont, TitleFontColour, HoverTitleFontColour, and TitleHeight

            //Sets Content Font
            this.Font = InternalVisualStyle.ContentFont;
            this.ForeColor = InternalVisualStyle.ContentFontColour;

            //Sets Automatic Numbers & Task Indentation
            this.InternalAutomaticNumbering = InternalVisualStyle.AutomaticNumbering;
            this.TaskIndentation = InternalVisualStyle.TaskIndentation;
            this.TaskHeight = InternalVisualStyle.TaskHeight;

            //If use content gradient
            if (this.VisualStyle.UseContentGradient)
            {
                this.tPnlGradient1.GradientColorBottom = InternalVisualStyle.ContentGradientEnd;
                this.tPnlGradient1.GradientColorTop = InternalVisualStyle.ContentGradientStart;
                this.tPnlGradient1.GradientMode = InternalVisualStyle.ContentGradientMode;
            }
            else
            {
                this.tPnlGradient1.GradientColorBottom = InternalVisualStyle.ContentBackgroundColour;
                this.tPnlGradient1.GradientColorTop = InternalVisualStyle.ContentBackgroundColour;
                this.tPnlGradient1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor necessary for TTaskList to work with the GUI designer
        /// Once the control is on a form, the designer should convert this constructor to the parameterized constructor.
        ///
        /// Will provide defaults:
        ///   default xmlnode is an empty list
        ///   default style is defined in global settings section.
        /// </summary>
        public TTaskList()
            : this(TYml2Xml.CreateXmlDocument(), DEFAULT_STYLE)
        {
        }

        /// <summary>
        /// Will provide defaults:
        ///   default style is defined in global settings section.
        /// </summary>
        public TTaskList(XmlNode AXmlnode)
            : this(AXmlnode, DEFAULT_STYLE)
        {
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
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion

            this.VisualStyle = new Ict.Common.Controls.TVisualStyles(Style);
            this.MasterXmlNode = MasterNode;
        }

        #endregion

        #region LoadTaskItems

        /// <summary>
        /// Default method to load task items
        /// Loads Task Items from the already specified MasterXmlNode for the task list
        /// </summary>
        /// <param name="ARebuildXmlNodeToLinkLabelMapping"/>
        private void LoadTaskItems(bool ARebuildXmlNodeToLinkLabelMapping = false)
        {
            LoadTaskItems(this.InternalMasterXmlNode, 0, "", ARebuildXmlNodeToLinkLabelMapping);
        }

        /// <summary>
        /// Private method to load taskItems of a masterXmlNode
        /// This method is also used to reload task items when style has been changed or a node has been disabled, etc
        /// </summary>
        /// <param name="Node"></param>
        /// <param name="NumberingLevel"></param>
        /// <param name="ParentNumberText"></param>
        /// <param name="ARebuildXmlNodeToLinkLabelMapping"/>
        private void LoadTaskItems(XmlNode Node, int NumberingLevel, String ParentNumberText,
            bool ARebuildXmlNodeToLinkLabelMapping = false)
        {
            this.SuspendLayout();

            if (ARebuildXmlNodeToLinkLabelMapping)
            {
                FXmlNodeToLinkLabelMapping = new Dictionary <XmlNode, LinkLabel>();
            }

            //If this is the base case, reset number of Tasks and clear previously painted Task Items
            if (NumberingLevel == 0)
            {
                NumTasks = 0;
                this.tPnlGradient1.Controls.Clear();

                // Set the 'Padding'
                this.tPnlGradient1.AutoScrollMargin = new Size(VisualStyle.ContentPaddingRight, VisualStyle.ContentPaddingBottom);
            }

            this.tPnlGradient1.Resize += new EventHandler(TTaskList_Resize);
            int CurrentNumbering = 1;
            NumberingLevel++;

            XmlNode TaskNode = Node.FirstChild;

            //Iterate through all children nodes of the node
            while (TaskNode != null)
            {
                if (SkipThisLevel(TaskNode))
                {
                    TaskNode = TaskNode.FirstChild;
                }

                LinkLabel lblTaskItem = new LinkLabel();
                lblTaskItem.Tag = TaskNode;

                if (TaskNode != FActiveTaskItem)
                {
                    SetCommonNonActivatedLinkAppearance(lblTaskItem);
                }
                else
                {
                    SetCommonActivatedLinkAppearance(lblTaskItem);
                }

                lblTaskItem.Name = TaskNode.Name;
                lblTaskItem.AutoSize = true;
                lblTaskItem.Font = VisualStyle.ContentFont;

                //@TODO: This line specifies the indentation by setting the location, however each level is indented the same amount
                // Should allow the first level to be indented a different amount than the rest of the levels
                lblTaskItem.Location = new System.Drawing.Point(VisualStyle.ContentPaddingLeft + (NumberingLevel * this.TaskIndentation),
                    VisualStyle.ContentPaddingTop + (NumTasks * TaskHeight));

                lblTaskItem.LinkClicked += new LinkLabelLinkClickedEventHandler(lblTaskItem_LinkClicked);
                lblTaskItem.Links[0].LinkData = TaskNode;

                lblTaskItem.MouseEnter += new System.EventHandler(this.LinkLabelMouseEnter);
                lblTaskItem.MouseLeave += new System.EventHandler(this.LinkLabelMouseLeave);

                if (IsDisabled(TaskNode))
                {
                    lblTaskItem.Links[0].Enabled = !IsDisabled(TaskNode);
                    lblTaskItem.DisabledLinkColor = VisualStyle.ContentDisabledFontColour;
                    lblTaskItem.LinkBehavior = LinkBehavior.NeverUnderline;
                }

                if (this.IsVisible(TaskNode))
                {
                    //Automatic Numbering
                    String NumberText = ParentNumberText + (CurrentNumbering).ToString() + ".";

                    if (!this.InternalAutomaticNumbering)
                    {
                        lblTaskItem.Text = TLstFolderNavigation.GetLabel(TaskNode);
                    }
                    else
                    {
                        lblTaskItem.Text = NumberText + " " + TLstFolderNavigation.GetLabel(TaskNode);
                        CurrentNumbering++;
                    }

                    this.tPnlGradient1.Controls.Add(lblTaskItem);

                    FXmlNodeToLinkLabelMapping[TaskNode] = lblTaskItem;

                    NumTasks++;

                    //If the TaskNode has Children, do subtasks
                    if ((TaskNode.HasChildNodes)
                        && (!DontShowNestedTasksAsLinks(TaskNode)))
                    {
                        LoadTaskItems(TaskNode, NumberingLevel, NumberText, false);
                    }
                }

                TaskNode = TaskNode.NextSibling;
            }

            FTaskListMaxHeight = tPnlGradient1.GetPreferredSize(new Size()).Height;
            this.ResumeLayout();
        }

        void TTaskList_Resize(object sender, EventArgs e)
        {
            // was setting FTaskListMaxHeight, but this is now set when the tasks are loaded
        }

        void SetCommonActivatedLinkAppearance(LinkLabel ALinkLabel)
        {
            ALinkLabel.LinkColor = VisualStyle.ContentActivatedFontColour;

            if (VisualStyle.ContentActivatedFontUnderline)
            {
                ALinkLabel.LinkBehavior = LinkBehavior.AlwaysUnderline;
            }
            else
            {
                ALinkLabel.LinkBehavior = LinkBehavior.HoverUnderline;
            }

            if (VisualStyle.UseContentBackgroundColours)
            {
                ALinkLabel.BackColor = VisualStyle.ContentActivatedBackgroundColour;
            }
            else
            {
                ALinkLabel.BackColor = Color.Transparent;
            }
        }

        void SetCommonNonActivatedLinkAppearance(LinkLabel ALinkLabel)
        {
            ALinkLabel.LinkColor = VisualStyle.ContentFontColour;

            if (VisualStyle.UseContentBackgroundColours)
            {
                ALinkLabel.BackColor = VisualStyle.ContentBackgroundColour;
            }
            else
            {
                ALinkLabel.BackColor = Color.Transparent;
            }

            ALinkLabel.LinkBehavior = LinkBehavior.HoverUnderline;
        }

        void lblTaskItem_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel ClickedLabel = (LinkLabel)sender;

            FActiveTaskItem = (XmlNode)ClickedLabel.Tag;

            RemoveActivatedLinkAppearenceFromNonActivated();

            // Change Link appearance to signalise to the user that the LinkLabel has been clicked
            // Note: this is different from 'Activated' appearance
            SetCommonActivatedLinkAppearance(ClickedLabel);

            // Fire ItemActivation Event
            OnItemActivation(this, (XmlNode)e.Link.LinkData, (LinkLabel)sender);

            // Repaint all Tasks to reflect their Activated/non-Activated state
            // Note: This re-sets the Link appearance set above to 'Activated' appearance
            LoadTaskItems();
        }

        void RemoveActivatedLinkAppearenceFromNonActivated()
        {
            foreach (Control Task in tPnlGradient1.Controls)
            {
                if (Task.Tag != FActiveTaskItem)
                {
                    SetCommonNonActivatedLinkAppearance((LinkLabel)Task);
                }
            }
        }

        #endregion

        #region GetTaskBy* functions

        /// <summary>
        /// Method to retrieve a TaskNode based on a position in the MasterXmlNode for this TTaskList
        /// This position includes any hidden task items
        /// </summary>
        /// <param name="TaskNumber">A String that specifies the position of the desired element
        ///     "3.1" means the first child of the third element</param>
        /// <returns>XmlNode at specified position or null if there is no element at that position</returns>
        public XmlNode GetTaskByNumber(String TaskNumber)
        {
            return GetTaskByNumber(TaskNumber, true);
        }

        /// <summary>
        /// Method to retrieve a TaskNode based on a position in the MasterXmlNode for this TTaskList
        /// </summary>
        /// <param name="TaskNumber">A String that specifies the position of the desired element
        ///     "3.1" means the first child of the third element
        /// </param>
        /// <param name="IncludeHiddenElements">Flag whether to include task items which are specified hidden</param>
        /// <returns>XmlNode at specified position or null if there is no element at that position</returns>
        public XmlNode GetTaskByNumber(String TaskNumber, bool IncludeHiddenElements)
        {
            return GetTaskByNumber(TaskNumber, IncludeHiddenElements, this.InternalMasterXmlNode);
        }

        /// <summary>
        /// Method to retrieve a TaskNode based on a position in the list
        /// </summary>
        /// <param name="TaskNumber">A String that specifies the position of the desired element
        ///     "3.1" means the first child of the third element
        /// </param>
        /// <param name="IncludeHiddenElements">Flag whether to include task items which are specified hidden</param>
        /// <param name="Node">The root from which to start the search</param>
        /// <returns>XmlNode at the specified position or null if there is no element at that position</returns>
        public XmlNode GetTaskByNumber(String TaskNumber, bool IncludeHiddenElements, XmlNode Node)
        {
            XmlNode TaskNode = Node.FirstChild;
            //char[] period = {'.'};
            int CurrentLevelTaskNumber = Convert.ToInt32(TaskNumber.Split('.')[0]);
            String NextLevelTaskNumber = (TaskNumber.IndexOf(".") >= 0) ? TaskNumber.Substring(TaskNumber.IndexOf(".") + 1) : "";

            int counter = 1;

            //Iterate through all children nodes of the node
            while (TaskNode != null && counter <= CurrentLevelTaskNumber)
            {
                if (TaskRegex.IsMatch(TaskNode.Name))
                {
                    if (IncludeHiddenElements || (TaskNode.Attributes["Hidden"] == null) || !(TrueRegex.IsMatch(TaskNode.Attributes["Hidden"].Value)))
                    {
                        counter++;
                    }
                }

                TaskNode = TaskNode.NextSibling;
            }

            if (TaskNode == null)
            {
                return null;
            }

            if (NextLevelTaskNumber.Equals(""))
            {
                return TaskNode;
            }
            else if (TaskNode.HasChildNodes)
            {
                return GetTaskByNumber(NextLevelTaskNumber, IncludeHiddenElements, TaskNode);
            }

            return null;
        }

        /// <summary>
        /// Method to retrieve an XmlNode which is a child of this TTaskList's MasterXmlNode
        /// </summary>
        /// <param name="TaskName">The Name of the desired XmlNode</param>
        /// <returns>XmlNode Reference</returns>
        public XmlNode GetTaskByName(String TaskName)
        {
            return GetTaskByName(TaskName, this.MasterXmlNode);
        }

        /// <summary>
        /// Method to retrieve an XmlNode which is a child of the specified XmlNode
        /// </summary>
        /// <param name="TaskName">The Name of the desired XmlNode</param>
        /// <param name="Node">The reference to the XmlNode whose descendents should be searched</param>
        /// <returns>XmlNode Reference</returns>
        public XmlNode GetTaskByName(String TaskName, XmlNode Node)
        {
            XmlNode TaskNode = Node.FirstChild;

            //char[] period = {'.'};
            //Iterate through all children nodes of the node
            while (TaskNode != null)
            {
                //If Task is the task being searched for
                if (TaskNode.Name.Equals(TaskName))
                {
                    return TaskNode;
                }

                //Recursively check tasks
                if (TaskNode.HasChildNodes)
                {
                    XmlNode temp = GetTaskByName(TaskName, TaskNode);

                    if (temp != null)
                    {
                        return temp;
                    }
                }

                TaskNode = TaskNode.NextSibling;
            }

            return null;
        }

        #endregion

        private LinkLabel GetLinkLabelForXmlNode(XmlNode AXmlNode)
        {
            LinkLabel ReturnValue = null;
            LinkLabel FoundLinkLabel;

            if (FXmlNodeToLinkLabelMapping != null)
            {
                if (FXmlNodeToLinkLabelMapping.TryGetValue(AXmlNode, out FoundLinkLabel))
                {
                    ReturnValue = FoundLinkLabel;
                }
            }

            return ReturnValue;
        }

        #region get/set Attributes of task items

        /// <summary>
        /// Method to get the attribute of a tasklist node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns>string attribute, or empty string if attribute is null.</returns>
        public string GetAttribute(XmlNode node, string attr)
        {
            if (node == null) 
            {
                throw new ArgumentNullException("Argument 'node' must not be null");
            }
            
            if (node.Attributes == null) 
            {
                throw new ArgumentNullException("Argument 'node' must have Attributes (node.Name='" + node.Name + "')");
            }
            
            if (node.Attributes[attr] != null)
            {
                return node.Attributes[attr].Value;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Method to test if an attribute of XmlNode is true.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <param name="TrueByDefault"></param>
        /// <returns>Boolean whether given Xml Node has the passed attribute set to true</returns>
        public bool AttributeTrue(XmlNode node, string attr, bool TrueByDefault = true)
        {
            if (node == null) 
            {
                throw new ArgumentNullException("Argument 'node' must not be null");
            }
            
            if (node.Attributes == null) 
            {
                throw new ArgumentNullException("Argument 'node' must have Attributes (node.Name='" + node.Name + "')");
            }
            
            if (node.Attributes[attr] == null)
            {
                if (TrueByDefault)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return TrueRegex.IsMatch(node.Attributes[attr].Value);
        }

        /// <summary>
        /// Method to test if an attribute of XmlNode is false.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns>Boolean whether given Xml Node has the passed attribute set to true</returns>
        public bool AttributeFalse(XmlNode node, string attr)
        {
            if (node.Attributes[attr] == null)
            {
                return false;
            }

            return FalseRegex.IsMatch(node.Attributes[attr].Value);
        }

        /// <summary>
        /// Returns whether given Xml Node has the attribute Visible set to true
        /// </summary>
        /// <param name="node"></param>
        /// <returns>True if the Visible attribute is set to true or if it isn't set, otherwise false.</returns>
        public bool IsVisible(XmlNode node)
        {
            return AttributeTrue(node, "Visible");
        }

        /// <summary>
        /// Returns whether given XmlNode has the attribute Enabled set to false
        /// </summary>
        /// <param name="node"></param>
        /// <returns>False if the Enabled Attribute is set to false, otherwise true.</returns>
        public bool IsDisabled(XmlNode node)
        {
            return AttributeFalse(node, "Enabled");
        }

        /// <summary>
        /// Returns whether given Xml Node has the attribute DontShowNestedTasksAsLinks set to true
        /// </summary>
        /// <param name="node"></param>
        /// <returns>True if the DontShowNestedTasksAsLinks attribute is set to true or if it isn't set, otherwise false.</returns>
        public bool DontShowNestedTasksAsLinks(XmlNode node)
        {
            return AttributeTrue(node, "DontShowNestedTasksAsLinks", false);
        }

        /// <summary>
        /// Returns whether given Xml Node has the attribute SkipThisLevel set to true
        /// </summary>
        /// <param name="node"></param>
        /// <returns>True if the SkipThisLevel attribute is set to true or if it isn't set, otherwise false.</returns>
        public bool SkipThisLevel(XmlNode node)
        {
            return AttributeTrue(node, "SkipThisLevel", false);
        }

        /// <summary>
        /// Clears all attributes of a certain type for all descendents of the specified node
        /// </summary>
        /// <param name="AttributeType">The name of the attribute type</param>
        /// <param name="Node">The root node for the tree which will be cleared</param>
        public void ClearAllAttributeOfType(String AttributeType, XmlNode Node)
        {
            XmlNode TaskNode = Node.FirstChild;

            //char[] period = {'.'};
            //Iterate through all children nodes of the node
            while (TaskNode != null)
            {
                //If Task is the task being searched for
                if (TaskNode.Attributes["Active"] != null)
                {
                    TaskNode.Attributes.Remove(TaskNode.Attributes["Active"]);
                }

                //Recursively check tasks
                if (TaskNode.HasChildNodes)
                {
                    ClearAllAttributeOfType(AttributeType, TaskNode);
                }

                TaskNode = TaskNode.NextSibling;
            }
        }

        /// <summary>
        /// Method to change the attribute of a tasklist node, and create the attribute if it's null.
        /// Doesn't handle possible case of the node not being a descendent of the masterNode for this list
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <param name="setting"></param>
        /// <param name="load">determines if the `LoadTaskItems` function is called at the end</param>
        public void ChangeAttribute(XmlNode node, string attr, string setting, bool load)
        {
            if (node.Attributes[attr] != null)
            {
                node.Attributes[attr].Value = setting;
            }
            else
            {
                XmlAttribute xmlAttr = node.OwnerDocument.CreateAttribute(attr);
                xmlAttr.Value = setting;
                node.Attributes.Append(xmlAttr);
            }

            if (load)
            {
                LoadTaskItems();
            }
        }

        /// <summary>
        /// Method to hide a task item, given an XmlNode Object
        /// Doesn't handle possible case of the node not being a descendent of the masterNode for this list
        /// </summary>
        /// <param name="node"></param>
        public void HideTaskItem(XmlNode node)
        {
            ChangeAttribute(node, "Visible", "False", true);
        }

        /// <summary>
        /// Method to make a task item visible, given an XmlNode Object
        /// Doesn't handle possible case of the node not being a descendent of the masterNode for this list
        /// </summary>
        /// <param name="node"></param>
        public void ShowTaskItem(XmlNode node)
        {
            ChangeAttribute(node, "Visible", "True", true);
        }

        /// <summary>
        /// Selects the first TaskItem (=LinkLabel). Beside selecting it, this also fires the
        /// 'ItemActivation' Event for that TaskItem.
        /// </summary>
        public void SelectFirstTaskItem()
        {
            LinkLabel FirstLinkLabel = (LinkLabel) this.tPnlGradient1.Controls[0];

            lblTaskItem_LinkClicked(FirstLinkLabel, new LinkLabelLinkClickedEventArgs
                    (FirstLinkLabel.Links[0]));
        }

        /// <summary>
        /// Fires the 'ItemActivation' Event for the Active TaskItem.
        /// </summary>
        public void FireLinkClickedEventForActiveTaskItem()
        {
            if (ActiveTaskItem != null)
            {
                LinkLabel ActiveTaskItemsLinkLabel = GetLinkLabelForXmlNode(ActiveTaskItem);

                lblTaskItem_LinkClicked(ActiveTaskItemsLinkLabel, new LinkLabelLinkClickedEventArgs
                        (ActiveTaskItemsLinkLabel.Links[0]));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="node"></param>
        public void ActivateTaskItem(XmlNode node)
        {
            ChangeAttribute(node, "Active", "True", true);
        }

        /// <summary>
        /// </summary>
        /// <param name="node"></param>
        public void DeactivateTaskItem(XmlNode node)
        {
            ChangeAttribute(node, "Active", "False", true);
        }

        /// <summary>
        /// Method to hide a task item, given an XmlNode Object
        /// Doesn't handle possible case of the node not being a descendent of the masterNode for this list
        /// </summary>
        /// <param name="node"></param>
        public void DisableTaskItem(XmlNode node)
        {
            if (node == FActiveTaskItem)
            {
                FActiveTaskItem = null;
            }

            ChangeAttribute(node, "Enabled", "False", true);
        }

        /// <summary>
        /// Method to hide a task item, given an XmlNode Object
        /// Doesn't handle possible case of the node not being a descendent of the masterNode for this list
        /// </summary>
        /// <param name="node"></param>
        public void EnableTaskItem(XmlNode node)
        {
            ChangeAttribute(node, "Enabled", "True", true);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for changing the appearence of a TaskList item when the mouse hovers over the TaskItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LinkLabelMouseEnter(object sender, System.EventArgs e)
        {
            LinkLabel lbl = (LinkLabel)sender;
            XmlNode node = this.GetTaskByName(lbl.Name);

            if (node != FActiveTaskItem)
            {
                if (this.VisualStyle.UseContentBackgroundColours)
                {
                    lbl.BackColor = this.VisualStyle.ContentHoverBackgroundColour;
                }
                else
                {
                    lbl.LinkColor = this.VisualStyle.ContentHoverFontColour;
                }
            }
        }

        /// <summary>
        /// Event Handler for reverting the appearence of TaskList item when the mouse leaves the TaskItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LinkLabelMouseLeave(object sender, System.EventArgs e)
        {
            LinkLabel lbl = (LinkLabel)sender;
            XmlNode node = this.GetTaskByName(lbl.Name);

            if (node == FActiveTaskItem)
            {
                if (this.VisualStyle.UseContentBackgroundColours)
                {
                    lbl.BackColor = this.VisualStyle.ContentActivatedBackgroundColour;
                }

                lbl.LinkColor = this.VisualStyle.ContentActivatedFontColour;
            }
            else
            {
                if (this.VisualStyle.UseContentBackgroundColours)
                {
                    lbl.BackColor = Color.Transparent;
                }

                lbl.LinkColor = this.VisualStyle.ContentFontColour;
            }
        }

        #endregion
    }
}