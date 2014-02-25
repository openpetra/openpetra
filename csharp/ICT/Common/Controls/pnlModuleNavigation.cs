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
using System.Xml;
using System.Windows.Forms;

using Ict.Common.IO;

namespace Ict.Common.Controls
{
    /// <summary>
    /// Represents an OpenPetra Module in the OpenPetra Main Menu.
    /// Contains a single instance of a <see cref="TPnlCollapsible" /> Control (FCollapsibleNavigation)
    /// which collapses in horizontal direction.
    /// </summary>
    /// <remarks>
    /// The heading of the <see cref="TPnlCollapsible" /> Control shows the name of the current OpenPetra Module.
    /// The OpenPetra Main Menu can show various OpenPetra Modules, but only one <see cref="TPnlModuleNavigation" />
    /// instance is seen by the user at any given time.
    /// Also hold a reference to the currently displayed <see cref="TLstTasks" /> Control in FCurrentTaskList.
    /// Although there can be 1..n <see cref="TLstTasks" /> Controls per Sub-Module, only one is currently displayed.
    /// </remarks>
    public partial class TPnlModuleNavigation : System.Windows.Forms.Panel
    {
        private Ict.Common.Controls.TPnlCollapsible FCollapsibleNavigation = new TPnlCollapsible();
        private int FCurrentLedger = -1;
        private bool FIsLedgerBasedModule = false;
        private bool FSuppressLedgerChangedEvent = false;
        private bool FIsConferenceBasedModule = false;

        /// Constructor.
        public TPnlModuleNavigation()
        {
        }

        /// <summary>
        /// This is the content panel which will host the task lists
        /// </summary>
        private TDashboard FDashboard;

        /// <summary>
        /// This is the currently displayed Task List
        /// </summary>
        private TLstTasks FCurrentTaskList = null;

        private int FMaxTaskWidth;
        private TExtStatusBarHelp FStatusbar = null;

        #region Events

        /// <summary>
        /// Contains data about a Ledger that got selected by the user.
        /// </summary>
        public delegate void LedgerSelected(int ALedgerNr, string ALedgerName);

        /// <summary>Fired when a Ledger got selected by the user (by clicking on it's LinkLabel).</summary>
        public event LedgerSelected LedgerChanged;

        /// <summary>Delegate to update subsystem link status which needs to be updated by caller.</summary>
        public delegate void UpdateSubsystemLinkStatus(int ALedgerNr, TPnlCollapsible APnlCollapsible);

        /// <summary>Store Delegate to update subsystem link status</summary>
        private static UpdateSubsystemLinkStatus FSubSystemLinkStatus;

        #endregion

        #region Properties

        /// <summary>Name of the Module</summary>
        public new string Text
        {
            get
            {
                return FCollapsibleNavigation.Text;
            }
            set
            {
                FCollapsibleNavigation.Text = value;
            }
        }

        /// <summary>Task List Xml Node</summary>
        public string TaskListNodeName
        {
            get
            {
                return FCollapsibleNavigation.TaskListNode.Name;
            }
        }

        /// <summary>
        /// Delegate for determinig a help topic for a given Form and Control.
        /// </summary>
        public static UpdateSubsystemLinkStatus SubSystemLinkStatus
        {
            get
            {
                return FSubSystemLinkStatus;
            }
            set
            {
                FSubSystemLinkStatus = value;
            }
        }

        #endregion

        /// <summary>
        /// The currently selected Ledger
        /// </summary>
        public int CurrentLedger
        {
            get
            {
                return FCurrentLedger;
            }

            set
            {
                if ((FIsLedgerBasedModule)
                    && (FCurrentLedger != value))
                {
                    FCurrentLedger = value;

                    // Select the LinkLabel that represents the current Ledger if the 'Select Ledger' Collapsible Panel is present
                    if (FCollapsibleNavigation.CollapsiblePanelHosterInstance.GetTaskListInstance(1) != null)
                    {
                        SelectCurrentLedgerLink();
                    }
                }
                else
                {
                    FCurrentLedger = value;
                }
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AFolderNode"></param>
        /// <param name="ADashboard"></param>
        /// <param name="AExpandedWidth"></param>
        /// <param name="AMultiLedgerSite"></param>
        /// <param name="AConferenceSelected"></param>
        public TPnlModuleNavigation(XmlNode AFolderNode, TDashboard ADashboard, int AExpandedWidth, bool AMultiLedgerSite, bool AConferenceSelected)
        {
            this.FDashboard = ADashboard;
            this.Dock = DockStyle.Top;

            // AutoSize causes trouble on Mono
            // FCollapsibleNavigation.AutoSize = true;
            // FCollapsibleNavigation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            FCollapsibleNavigation.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdHorizontal;
            FCollapsibleNavigation.Dock = System.Windows.Forms.DockStyle.Left;
            FCollapsibleNavigation.ExpandedSize = AExpandedWidth;
            FCollapsibleNavigation.Location = new System.Drawing.Point(0, 0);
            FCollapsibleNavigation.Margin = new System.Windows.Forms.Padding(0);
            FCollapsibleNavigation.TabIndex = 0;
            FCollapsibleNavigation.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsHorizontalCollapse;
            FCollapsibleNavigation.Collapsed += delegate(object sender, EventArgs e) {
                OnCollapsed();
            };
            FCollapsibleNavigation.Expanded += delegate(object sender, EventArgs e) {
                OnExpanded();
            };
            FCollapsibleNavigation.ItemActivation += delegate(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked, object AOtherData)
            {
                OnItemActivation(ATaskList, ATaskListNode, AItemClicked, AOtherData);
            };

            if (AMultiLedgerSite
                && (TXMLParser.GetAttribute(AFolderNode, "DependsOnLedger").ToLower() == "true"))
            {
                FIsLedgerBasedModule = true;
            }
            else if (AConferenceSelected
                     && (TXMLParser.GetAttribute(AFolderNode, "DependsOnConference").ToLower() == "true"))
            {
                FIsConferenceBasedModule = true;
            }

            if (FIsLedgerBasedModule || FIsConferenceBasedModule
                || (TXMLParser.GetAttribute(AFolderNode, "ShowAsCollapsiblePanels").ToLower() == "true"))
            {
                FCollapsibleNavigation.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckCollapsiblePanelHoster;
            }
            else
            {
                FCollapsibleNavigation.HostedControlKind = THostedControlKind.hckTaskList;
            }

            FCollapsibleNavigation.TaskListNode = AFolderNode;

            FCollapsibleNavigation.InitUserControl();

            if (FIsLedgerBasedModule)
            {
                // We want Ledgers to be Selected (if there are multiple Ledgers in the Site) *AND* at the same time Sub-Modules
                // in the top Collapsible Panel
                FCollapsibleNavigation.CollapsiblePanelHosterInstance.OnlyOneActiveTaskOnAllCollapsiblePanelsTaskLists = false;
            }

            this.Controls.Add(FCollapsibleNavigation);
        }

        #region Properties

        /// <summary>
        /// Maximum Task Width
        /// </summary>
        public int MaxTaskWidth
        {
            get
            {
                return FMaxTaskWidth;
            }

            set
            {
                if (FMaxTaskWidth != value)
                {
                    FMaxTaskWidth = value;

                    FCurrentTaskList.MaxTaskWidth = FMaxTaskWidth;
                }
            }
        }

        #endregion

        #region Events

        /// <summary>Event is fired after the panel has collapsed.</summary>
        public event System.EventHandler Collapsed;

        /// <summary>Event is fired after the panel has expanded.</summary>
        public event System.EventHandler Expanded;

        /// <summary>Fired when a TaskLink got activated (by clicking on it or programmatically).</summary>
        public event TTaskList.TaskLinkClicked ItemActivation;

        #endregion

        #region Public Methods

        /// <summary>
        /// set the statusbar so that error messages can be displayed
        /// </summary>
        public TExtStatusBarHelp Statusbar
        {
            set
            {
                FStatusbar = value;
            }
        }

        /// <summary>
        /// make sure that the content panel is populated with the contents of the first link;
        /// this might be called when selecting a folder
        /// </summary>
        public void SelectFirstLink()
        {
            if (FCollapsibleNavigation.HostedControlKind == THostedControlKind.hckCollapsiblePanelHoster)
            {
                FCollapsibleNavigation.CollapsiblePanelHosterInstance.GetTaskListInstance(0).SelectFirstTaskItem();
            }
            else
            {
                FCollapsibleNavigation.TaskListInstance.SelectFirstTaskItem();
            }
        }

        /// <summary>
        /// Fires the LinkClicked Event (which results in the ItemActivation Event) for the currently ActiveTaskItem.
        /// </summary>
        public void FireSelectedLinkEvent()
        {
            if (FCollapsibleNavigation.HostedControlKind == THostedControlKind.hckCollapsiblePanelHoster)
            {
                if (FIsLedgerBasedModule)
                {
                    FCollapsibleNavigation.CollapsiblePanelHosterInstance.GetTaskListInstance(0).FireLinkClickedEventForActiveTaskItem();
                }
                else
                {
                    FCollapsibleNavigation.CollapsiblePanelHosterInstance.GetTaskListInstanceWhereLastItemActivationHappened().
                    FireLinkClickedEventForActiveTaskItem();
                }
            }
            else
            {
                FCollapsibleNavigation.TaskListInstance.FireLinkClickedEventForActiveTaskItem();
            }
        }

        #endregion

        #region Private Methods

        private void SelectCurrentLedgerLink()
        {
            XmlNode CurrentLedgerNode;

            if (FIsLedgerBasedModule)
            {
                CurrentLedgerNode = FCollapsibleNavigation.CollapsiblePanelHosterInstance.GetTaskListInstance(1).GetTaskByName(
                    "Ledger" + FCurrentLedger.ToString());

                FSuppressLedgerChangedEvent = true;

                FCollapsibleNavigation.CollapsiblePanelHosterInstance.ActiveTaskItem = CurrentLedgerNode;

                FSuppressLedgerChangedEvent = false;
            }
        }

        private void LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Object tag = ((Control)sender).Tag;

            if (tag.GetType() == typeof(TLstTasks))
            {
                FCurrentTaskList = (TLstTasks)tag;
//TLogging.Log("LinkClicked for existing " + FCurrentTaskList.Name);
            }
            else
            {
                FCurrentTaskList = new TLstTasks((XmlNode)tag, FDashboard.TaskAppearance);
//TLogging.Log("LinkClicked for NEW " + FCurrentTaskList.Name);
                ((Control)sender).Tag = FCurrentTaskList;
            }

            FCurrentTaskList.Statusbar = FStatusbar;
            FCurrentTaskList.Dock = DockStyle.Fill;
            TLstTasks.CurrentLedger = FCurrentLedger;

            FDashboard.ShowTaskList(FCurrentTaskList);
//            Invalidate();
        }

        /// <summary>
        /// Raises the 'Collapsed' Event if something subscribed to it.
        /// </summary>
        private void OnCollapsed()
        {
            this.Width = FCollapsibleNavigation.Width;

            if (Collapsed != null)
            {
                Collapsed(this, new EventArgs());
            }
        }

        /// <summary>
        /// Raises the 'Expanded' Event if something subscribed to it.
        /// </summary>
        private void OnExpanded()
        {
            this.Width = FCollapsibleNavigation.Width;

            if (Expanded != null)
            {
                Expanded(this, new EventArgs());
            }
        }

        private void OnItemActivation(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked, object AOtherData)
        {
            if (ATaskListNode.Attributes["LedgerNumber"] == null)
            {
                if ((AOtherData != null)
                    && (FSubSystemLinkStatus != null)
                    && (AOtherData.GetType() == typeof(TPnlCollapsible)))
                {
                    FSubSystemLinkStatus(FCurrentLedger, (TPnlCollapsible)AOtherData);
                }

                LinkClicked(AItemClicked, null);
            }
            else
            {
                if (!FSuppressLedgerChangedEvent)
                {
                    OnLedgerChanged(Convert.ToInt32(ATaskListNode.Attributes["LedgerNumber"].Value), ATaskListNode.Attributes["LedgerName"].Value);

                    if ((AOtherData != null)
                        && (FSubSystemLinkStatus != null)
                        && (AOtherData.GetType() == typeof(TPnlCollapsible)))
                    {
                        FSubSystemLinkStatus(FCurrentLedger, (TPnlCollapsible)AOtherData);
                    }
                }
            }

            // Re-fire Event
            if (ItemActivation != null)
            {
                ItemActivation(ATaskList, ATaskListNode, AItemClicked, AOtherData);
            }
        }

        private void OnLedgerChanged(int ALedgerNr, string ALedgerName)
        {
            FCurrentLedger = ALedgerNr;

            if (FCurrentTaskList != null)
            {
                TLstTasks.CurrentLedger = FCurrentLedger;
            }

            if (LedgerChanged != null)
            {
                LedgerChanged(ALedgerNr, ALedgerName);
            }
        }

        #endregion
    }
}