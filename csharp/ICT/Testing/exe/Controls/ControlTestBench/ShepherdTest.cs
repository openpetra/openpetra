//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 Taylor Students
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
using System.Drawing;
using System.Windows.Forms;

using Ict.Common.Controls;

namespace ControlTestBench
{
    /// <summary>
    /// Description of ShepherdTest.
    /// </summary>
    public partial class ShepherdTest : Form
    {
        XmlNode FTestYAMNode = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShepherdTest()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="AXmlNode"></param>
        public ShepherdTest(XmlNode AXmlNode)
        {
            FTestYAMNode = AXmlNode;

            this.Controls.Clear();

            this.tPnlCollapsible1 = new TPnlCollapsible(THostedControlKind.hckTaskList,
                FTestYAMNode,
                TCollapseDirection.cdHorizontal,
                240,
                false,
                TVisualStylesEnum.vsShepherd);
            this.tPnlCollapsible1.BorderStyle = BorderStyle.FixedSingle;
            this.tPnlCollapsible1.Text = "My Shepherd Test";
            tPnlCollapsible1.Dock = DockStyle.Left;
            this.Controls.Add(this.tPnlCollapsible1);

            this.Size = new Size(750, 400);

            HookupItemActivationEvent();
//        tPnlCollapsible1.VisualStyleEnum = TVisualStylesEnum.vsShepherd;
//        tPnlCollapsible1.TaskListNode = AXmlNode;
//        tPnlCollapsible1.Collapse();
//        tPnlCollapsible1.Expand();
        }

        private void HookupItemActivationEvent()
        {
            ((TPnlCollapsible) this.Controls[0]).ItemActivation += new TTaskList.TaskLinkClicked(CollPanel_ItemActivation);
        }

        void CollPanel_ItemActivation(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked, object AOtherData)
        {
            MessageBox.Show(String.Format("Task '{0}' with Label '{1}' got clicked.", ATaskListNode.Name, AItemClicked.Text));
        }
    }
}