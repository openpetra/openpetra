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
    /// <remarks>The Collapsible Panels are realised according to the passed in XmlNodes in Property 
    /// <see cref="MasterXmlNode" />!</remarks>
    public partial class TPnlCollapsibleHoster : UserControl
    {
        /// <summary></summary>
        private XmlNode FMasterXmlNode;
        
        /// <summary></summary>
        TVisualStylesEnum FVisualStyle = TVisualStylesEnum.vsTaskPanel;
        
        /// <summary></summary>
        private int FDistanceBetweenCollapsiblePanels = 5;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public TPnlCollapsibleHoster()
        {
            InitializeComponent();
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
            MasterXmlNode = MasterNode;
        }

        
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
            return GetCollapsiblePanelInstance(ANumber).TaskListInstance;
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
            int CollPanelCount = 0;
            this.SuspendLayout();

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
                WrapperPanel.Padding = new Padding (0, 0, 0, FDistanceBetweenCollapsiblePanels);
                WrapperPanel.TabIndex = CollPanelCount;
                WrapperPanel.Tag = TaskNode;
                WrapperPanel.Name = TaskNode.Name;
                
                // Create a Collapsible Panel
                TPnlCollapsible CollPanel = new TPnlCollapsible(THostedControlKind.hckTaskList, TaskNode, TCollapseDirection.cdVertical, 10, false, FVisualStyle);
                CollPanel.Tag = WrapperPanel;
                CollPanel.Name = TaskNode.Name;
                CollPanel.Text = TLstFolderNavigation.GetLabel(TaskNode);
                CollPanel.Dock = DockStyle.Top;
                CollPanel.TabIndex = 0;                

                if((TaskNode.Attributes["Visible"] != null)
                   && (TaskNode.Attributes["Visible"].Value.ToLower() == "false"))
                {
                    CollPanel.Visible = false;
                }
                   
                if((TaskNode.Attributes["Enabled"] != null)
                   && (TaskNode.Attributes["Enabled"].Value.ToLower() == "false"))
                {
                    CollPanel.Enabled = false;
                }
                                      
                // Make the Collapsible Panel create and display its Task List
                CollPanel.RealiseTaskListNow();      
                
                WrapperPanel.Height = CollPanel.ExpandedSize + FDistanceBetweenCollapsiblePanels;
                WrapperPanel.Controls.Add(CollPanel);
                this.Controls.Add(WrapperPanel);

                // Make sure the Collapsible Panels' Wrapper Panels are shown in correct order and not in reverse order.
                // (This is needed because we 'stack them up' with '.Controls.Dock = DockStyle.Top')
                WrapperPanel.BringToFront();
                
                TaskNode = TaskNode.NextSibling;
                
                CollPanelCount++;                
            }            
            
            this.ResumeLayout();
        }

        #endregion
    }
}
