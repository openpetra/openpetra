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
        public void RealiseCollapsiblePanelsNow()
        {
            InstantiateCollapsiblePanels();
        }

        
        public TPnlCollapsible GetCollapsiblePanelInstance(int Number)
        {
            return (TPnlCollapsible)this.Controls[0];
        }
                
        public TTaskList GetTaskListInstance(int Number)
        {
            return ((TPnlCollapsible)this.Controls[0]).TaskListInstance;
        }
        
        #endregion
        
        #region Private Methods
        
        private void InstantiateCollapsiblePanels()
        {
            int CollPanelCount = 0;
            this.SuspendLayout();
            
            XmlNode TaskNode = FMasterXmlNode.FirstChild;

            //Iterate through all children nodes of the node
            while (TaskNode != null)
            {
                TPnlCollapsible CollPanel = new TPnlCollapsible(THostedControlKind.hckTaskList, TaskNode, TCollapseDirection.cdVertical, 10, false, FVisualStyle);
                CollPanel.Tag = TaskNode;
                CollPanel.Name = TaskNode.Name;
                CollPanel.Text = TLstFolderNavigation.GetLabel(TaskNode);
                CollPanel.Dock = DockStyle.Top;
                CollPanel.Padding = new System.Windows.Forms.Padding(0, 0, 5, FDistanceBetweenCollapsiblePanels);
                CollPanel.TabIndex = CollPanelCount;

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
                                      
                
                this.Controls.Add(CollPanel);
                
                // Make sure Collapsible Panels are shown in correct order and not in reverse order.
                // (This is needed because we 'stack them up' with 'CollPanel.Dock = DockStyle.Top')
                CollPanel.BringToFront();

                
                CollPanel.RealiseTaskListNow();                
                
                TaskNode = TaskNode.NextSibling;
                CollPanelCount++;                
            }            
            
            this.ResumeLayout();
        }
        
        #endregion
    }
}
