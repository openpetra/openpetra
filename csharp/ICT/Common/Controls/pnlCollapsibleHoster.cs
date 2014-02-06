//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
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
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using Ict.Common;
using Ict.Common.Controls;

namespace Ict.Common.Controls
{
    /// <summary>
    /// UserControl which acts as a hosting container for 1..n 'Collapsible Panels' (<see cref="TPnlCollapsible" />).
    /// </summary>
    /// <remarks>
    /// The Collapsible Panels are realised according to the passed in XmlNodes in Property
    /// <see cref="MasterXmlNode" />.
    /// Several <see cref="TPnlCollapsible" /> Controls can be expanded in vertical direction at the same time.
    /// Usually only a single <see cref="LinkLabel" /> in any of the <see cref="TPnlCollapsible" /> Controls can be
    /// active (=underlined LinkLabel). This can be changed by setting Property <see cref="OnlyOneActiveTaskOnAllCollapsiblePanelsTaskLists" />
    /// to false.
    ///</remarks>
    public partial class TPnlCollapsibleHoster : UserControl
    {
        /// <summary></summary>
        private XmlNode FMasterXmlNode;

        /// <summary></summary>
        TVisualStylesEnum FVisualStyle = TVisualStylesEnum.vsTaskPanel;

        /// <summary></summary>
        private int FDistanceBetweenCollapsiblePanels = 5;

        private int FCollPanelCount = 0;

        private bool FOnlyOneActiveTaskOnAllCollapsiblePanelsTaskLists = true;

        private TPnlCollapsible FCollPanelWhereLastItemActivationHappened = null;

        #region Events

        /// <summary>Fired when a TaskLink got activated (by clicking on it or programmatically).</summary>
        [Description("Event is fired when a TaskLink got activated (by clicking on it or programmatically).")]
        [Category("Collapsible Panel")]
        public event TTaskList.TaskLinkClicked ItemActivation;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public TPnlCollapsibleHoster()
        {
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion
        }

        /// <summary>
        /// Constructor for creating an object with a MasterNode and setting the Visual Style
        /// </summary>
        /// <param name="MasterNode">Base Node for the Collapsible Panels</param>
        /// <param name="Style">A TVisualStylesEnum value that specifies the visual style which should be used</param>
        public TPnlCollapsibleHoster(XmlNode MasterNode, TVisualStylesEnum Style)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            FVisualStyle = Style;

            ChangeVisualStyle();

            MasterXmlNode = MasterNode;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Distance between two Collapsible Panels.
        /// </summary>
        [Category("Collapsible Panel Hoster")]
        public int DistanceBetweenCollapsiblePanels
        {
            get
            {
                return FDistanceBetweenCollapsiblePanels;
            }

            set
            {
                FDistanceBetweenCollapsiblePanels = value;
            }
        }

        /// <summary>
        /// Visual Style.
        /// </summary>
        [Description("The Style in which the panel will be displayed. Note: certain Styles only work with certain 'CollapseDirection' settings!")]
        [Category("Collapsible Panel Hoster")]
        public TVisualStylesEnum VisualStyleEnum
        {
            get
            {
                return FVisualStyle;
            }
            set
            {
                FVisualStyle = value;

                ChangeVisualStyle();
            }
        }

        /// <summary></summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public XmlNode MasterXmlNode
        {
            get
            {
                return FMasterXmlNode;
            }
            set
            {
                //Note: we do not check to see if the tasklistnode is valid or not.
                FMasterXmlNode = value;
            }
        }

        /// <summary>
        /// Active Task Item.
        /// </summary>
        /// <remarks>Setting this Property to null has the effect that any ActiveTaskItem
        /// of all hosted CollapsiblePanel instances will be un-set, i.e. there will be no ActiveTaskItem
        /// in any of the hosted CollapsiblePanel instances.</remarks>
        public XmlNode ActiveTaskItem
        {
            get
            {
                XmlNode FoundTaskItem = null;

                if (FCollPanelCount != 0)
                {
                    for (int Counter = 0; Counter < FCollPanelCount; Counter++)
                    {
                        FoundTaskItem = GetCollapsiblePanelInstance(Counter).ActiveTaskItem;

                        if (FoundTaskItem != null)
                        {
                            return FoundTaskItem;
                        }
                    }

                    return null;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (value != null)
                {
                    TPnlCollapsible CollPanel;

                    if (FCollPanelCount != 0)
                    {
                        for (int Counter1 = 0; Counter1 < FCollPanelCount; Counter1++)
                        {
                            CollPanel = GetCollapsiblePanelInstance(Counter1);
                            CollPanel.ActiveTaskItem = value;

                            if (FOnlyOneActiveTaskOnAllCollapsiblePanelsTaskLists)
                            {
                                // Check if that Collapsible Panel has indeed set the ActiveTaskItem to what we asked it for -
                                // it doesn't do that if it doesn't host it. In that case we want to remove the ActiveTaskItem
                                // for all other CollapsiblePanels' TaskLists as we want only *one* ActiveTaskItem in *any* of
                                // the CollapsiblePanels!
                                if (CollPanel.ActiveTaskItem == value)
                                {
                                    for (int InnerCounter = 0; InnerCounter < FCollPanelCount; InnerCounter++)
                                    {
                                        if (InnerCounter != Counter1)
                                        {
                                            GetCollapsiblePanelInstance(InnerCounter).ActiveTaskItem = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int Counter2 = 0; Counter2 < FCollPanelCount; Counter2++)
                    {
                        GetCollapsiblePanelInstance(Counter2).ActiveTaskItem = null;
                    }
                }
            }
        }

        /// <summary>
        /// Whether the Collapsible Panel Hoster only allows one Active Task on all its
        /// CollapsiblePanel's TaskLists (default=true).
        /// </summary>
        public bool OnlyOneActiveTaskOnAllCollapsiblePanelsTaskLists
        {
            get
            {
                return FOnlyOneActiveTaskOnAllCollapsiblePanelsTaskLists;
            }

            set
            {
                FOnlyOneActiveTaskOnAllCollapsiblePanelsTaskLists = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Allow the outside to force the Collapsible Panels Hoster to initialise.
        /// </summary>
        /// <remarks><em><see cref="MasterXmlNode" /> must be set to an instance of XmlNode before calling this Method!</em></remarks>
        public void RealiseCollapsiblePanelsNow()
        {
            InstantiateCollapsiblePanels();
        }

        /// <summary>
        /// Returns the Collapsible Panel for a TaskList Instance.
        /// </summary>
        /// <param name="ANumber">Corresponds with the order of the Collapsible Panels that represent XmlNodes (Range 0..n,
        /// 0 being the Collapsible Panel that represents the first XmlNode).</param>
        /// <returns>Collapsible Panel that corresponds with <paramref name="ANumber" />.</returns>
        public TPnlCollapsible GetCollapsiblePanelInstance(int ANumber)
        {
            XmlNode ChildNode = FMasterXmlNode.ChildNodes[ANumber];

            return GetCollapsiblePanelInstance(ChildNode);
        }

        /// <summary>
        /// Returns the Collapsible Panel for a TaskList Instance.
        /// </summary>
        /// <param name="AChildNode">The XmlNode that the Collapsible Panel represents.</param>
        /// <returns>Collapsible Panel that corresponds with <paramref name="AChildNode" />.</returns>
        public TPnlCollapsible GetCollapsiblePanelInstance(XmlNode AChildNode)
        {
            TPnlCollapsible ReturnValue = null;

            foreach (Control WrapperPanel in this.Controls)
            {
                if (WrapperPanel.Tag == AChildNode)
                {
                    ReturnValue = (TPnlCollapsible)WrapperPanel.Controls[0];
                    break;
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Returns a TaskList Instance.
        /// </summary>
        /// <param name="ANumber">Corresponds with the order of the Collapsible Panels that represent
        /// XmlNodes which host the corresponding Task List instance (Range 0..n, 0 being the Collapsible Panel
        /// that matches the first XmlNode).</param>
        /// <returns>TaskList Instance that corresponds with <paramref name="ANumber" />.</returns>
        public TTaskList GetTaskListInstance(int ANumber)
        {
            TPnlCollapsible CollPanelInstance = GetCollapsiblePanelInstance(ANumber);

            if (CollPanelInstance != null)
            {
                return CollPanelInstance.TaskListInstance;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the TaskList Instance where the last ItemActivation happened.
        /// </summary>
        /// <returns>TaskList Instance where the last ItemActivation happened, null if no TaskList Instance
        /// fired an ItemActivation yet.</returns>
        public TTaskList GetTaskListInstanceWhereLastItemActivationHappened()
        {
            if (FCollPanelWhereLastItemActivationHappened != null)
            {
                return FCollPanelWhereLastItemActivationHappened.TaskListInstance;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a TaskList Instance.
        /// </summary>
        /// <param name="AChildNode">The XmlNode that represents the Collapsible Panel which hosts the corresponding Task List instance.</param>
        /// <returns>TaskList Instance that corresponds with <paramref name="AChildNode" />.</returns>
        public TTaskList GetTaskListInstance(XmlNode AChildNode)
        {
            return GetCollapsiblePanelInstance(AChildNode).TaskListInstance;
        }

        #endregion

        #region Private Methods

        private void InstantiateCollapsiblePanels()
        {
            this.SuspendLayout();

            this.Controls.Clear();
            FCollPanelCount = 0;

            if (FMasterXmlNode == null)
            {
                throw new Exception("MasterXmlNode Property not set to an instance of XmlNode");
            }

            XmlNode TaskNode = FMasterXmlNode.FirstChild;

            //Iterate through all children nodes of the node
            while (TaskNode != null)
            {
                // Create a wrapper Panel. This is only needed to be able to set a distance between Collapsible Panels.
                Panel WrapperPanel = new Panel();
                WrapperPanel.AutoSize = true;
                WrapperPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                WrapperPanel.BackColor = Color.Transparent;
                WrapperPanel.Dock = DockStyle.Top;
                WrapperPanel.Padding = new Padding(0, 0, 0, FDistanceBetweenCollapsiblePanels);
                WrapperPanel.TabIndex = FCollPanelCount;
                WrapperPanel.Tag = TaskNode;
                WrapperPanel.Name = TaskNode.Name;

                // Create a Collapsible Panel
                TPnlCollapsible CollPanel = new TPnlCollapsible(THostedControlKind.hckTaskList,
                    TaskNode,
                    TCollapseDirection.cdVertical,
                    10,
                    false,
                    FVisualStyle);
                CollPanel.Tag = WrapperPanel;
                CollPanel.Name = TaskNode.Name;
                CollPanel.Text = TLstFolderNavigation.GetLabel(TaskNode);
                CollPanel.Dock = DockStyle.Top;
                CollPanel.TabIndex = 0;

                if ((TaskNode.Attributes["Visible"] != null)
                    && (TaskNode.Attributes["Visible"].Value.ToLower() == "false"))
                {
                    CollPanel.Visible = false;
                }

                if ((TaskNode.Attributes["Enabled"] != null)
                    && (TaskNode.Attributes["Enabled"].Value.ToLower() == "false"))
                {
                    CollPanel.Enabled = false;
                }
                else
                {
                    CollPanel.ItemActivation += delegate(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked)
                    {
                        OnItemActivation(ATaskList, ATaskListNode, AItemClicked);
                    };
                }

                WrapperPanel.Height = CollPanel.ExpandedSize + FDistanceBetweenCollapsiblePanels;
                WrapperPanel.Controls.Add(CollPanel);
                this.Controls.Add(WrapperPanel);

                // Make sure the Collapsible Panels' Wrapper Panels are shown in correct order and not in reverse order.
                // (This is needed because we 'stack them up' with '.Controls.Dock = DockStyle.Top')
                WrapperPanel.BringToFront();

                TaskNode = TaskNode.NextSibling;

                FCollPanelCount++;
            }

            this.ResumeLayout();
        }

        /// <summary>
        /// Changes the Visual Style.
        /// </summary>
        private void ChangeVisualStyle()
        {
            TVisualStyles VisualStyle = new TVisualStyles(FVisualStyle);

            if (VisualStyle.CollapsiblePanelDistance != -1)
            {
                FDistanceBetweenCollapsiblePanels = VisualStyle.CollapsiblePanelDistance;
            }

            this.Padding = new Padding(VisualStyle.CollapsiblePanelPadding);
        }

        private void OnItemActivation(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked)
        {
            TTaskList FoundTaskList;
            TPnlCollapsible CollPanel;

            if (FOnlyOneActiveTaskOnAllCollapsiblePanelsTaskLists)
            {
                // Remove the Active Task Item from any Collapsible Panel's TaskList that
                // isn't the sending instance (that is passed in in ATaskList) as we only
                // want *one* Active Task Item in one hosted Collapsible Panel!
                for (int Counter = 0; Counter < FCollPanelCount; Counter++)
                {
                    CollPanel = GetCollapsiblePanelInstance(Counter);
                    FoundTaskList = CollPanel.TaskListInstance;

                    if (FoundTaskList != ATaskList)
                    {
                        FoundTaskList.ActiveTaskItem = null;
                    }
                }
            }

            FCollPanelWhereLastItemActivationHappened = GetCollapsiblePanelInstance(ATaskListNode.ParentNode);

            // Re-fire Event
            if (ItemActivation != null)
            {
                ItemActivation(ATaskList, ATaskListNode, AItemClicked);
            }
        }

        #endregion
    }
}