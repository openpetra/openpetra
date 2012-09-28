//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 sbird
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
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Ict.Common.Controls;
using Ict.Common.IO;

namespace ControlTestBench
{
    /// <summary>
    /// </summary>
    public partial class CollapsibleTest : Form
    {
        Ict.Common.Controls.TPnlCollapsible FPnl;
        Ict.Common.Controls.TPnlCollapsible FPnl2;
        XmlNode FTestYAMLNode = null;
        TVisualStylesEnum FEnumStyle = TVisualStylesEnum.vsDashboard;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public CollapsibleTest()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ATestYAMLNode"></param>
        /// <param name="AEnumStyle"></param>
        public CollapsibleTest(XmlNode ATestYAMLNode, TVisualStylesEnum AEnumStyle) : this()
        {
            FTestYAMLNode = ATestYAMLNode;
            FEnumStyle = AEnumStyle;            
        }
        
        private void TestEmptyConstructor(object sender, EventArgs e)
        {
            this.Controls.Remove(this.FPnl);

            this.FPnl = new TPnlCollapsible();
            this.FPnl.Text = "&With empty constructor";
            this.Controls.Add(this.FPnl);
        }
        private void TestTaskListVerticalConstructor(object sender, EventArgs e)
        {
            this.Controls.Remove(this.FPnl);

            this.FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList, FTestYAMLNode, TCollapseDirection.cdVertical, 120, false, FEnumStyle);
            this.FPnl.Text = "&Tasks";
            this.Controls.Add(this.FPnl);
        }
        private void TestUserControlVerticalConstructor(object sender, EventArgs e)
        {
            this.Controls.Remove(this.FPnl);

            this.FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, "Ict.Petra.Client.MPartner.Gui.TUC_PartnerInfo", TCollapseDirection.cdVertical, false, FEnumStyle);
            this.FPnl.Text = "&Partner Info";
            this.FPnl.InitUserControl();
            
            this.Controls.Add(this.FPnl);
        }
        private void TestTaskListHorizontalConstructor(object sender, EventArgs e)
        {
            this.Controls.Remove(this.FPnl);

            this.FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList, FTestYAMLNode, TCollapseDirection.cdHorizontal, 183, false, FEnumStyle);
            this.FPnl.BorderStyle = BorderStyle.FixedSingle;
            this.FPnl.Text = "&Finance";
            this.Controls.Add(this.FPnl);
        }
        
        void TestTaskListHorizontalRightConstructor(object sender, System.EventArgs e)
        {
            this.Controls.Remove(this.FPnl);

            this.FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, "Ict.Petra.Client.MPartner.Gui.TUC_PartnerInfo", TCollapseDirection.cdHorizontalRight, 250, false, FEnumStyle);
            this.FPnl.BorderStyle = BorderStyle.FixedSingle;
            this.FPnl.Text = "&To-Do Bar";
            this.Controls.Add(this.FPnl);            
        }
        
        private void TestTaskListExpandedConstructor(object sender, EventArgs e)
        {
            this.Controls.Remove(this.FPnl);

            this.FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList, TCollapseDirection.cdVertical, false);
            this.FPnl.Text = "&TaskList Expanded";
            this.Controls.Add(this.FPnl);
        }
        private void TestFullConstructor(object sender, EventArgs e)
        {
            this.Controls.Remove(this.FPnl);

            this.FPnl = new TPnlCollapsible(FTestYAMLNode, THostedControlKind.hckUserControl, "Foo.Bar", FEnumStyle, TCollapseDirection.cdHorizontal, true);
            this.FPnl.Text = "&With full constructor";
            this.Controls.Add(this.FPnl);
        }
        
        private void TestStacked(object sender, EventArgs e)
        {
            this.Controls.Remove(this.FPnl);
            this.Controls.Remove(this.FPnl2);

            this.FPnl = new TPnlCollapsible();
            this.FPnl2 = new TPnlCollapsible();
            this.Controls.Add(this.FPnl);
            this.Controls.Add(this.FPnl2);
        }

        
        private void ChangeWidth(object sender, EventArgs e)
        {
            this.FPnl.ExpandedSize = int.Parse(txtChangeExpandedSize.Text);
        }
        private void ChangeText(object sender, EventArgs e)
        {
            this.FPnl.Text = txtChangeText.Text;
        }
        
        private void ChangeCollapseDirection(object sender, EventArgs e)
        {
            switch( cboChangeCollapseDirection.Text )
            {
                case "Vertical":
                    this.FPnl.CollapseDirection = TCollapseDirection.cdVertical;
                    break;
                case "Horizontal (left)":
                    this.FPnl.CollapseDirection = TCollapseDirection.cdHorizontal;
                    break;
                case "Horizontal (right)":
                    this.FPnl.CollapseDirection = TCollapseDirection.cdHorizontalRight;
                    break;
            }
        }
        
        private void ChangeHostedControlKind(object sender, EventArgs e)
        {
            switch( cboChangeHostedControlKind.Text )
            {
                case "UserControl":
                    this.FPnl.HostedControlKind = THostedControlKind.hckUserControl;
                    break;
                case "Task List":
                    this.FPnl.HostedControlKind = THostedControlKind.hckTaskList;
                    break;
            }
        }
        
        private void ChangeUserControlString(object sender, EventArgs e)
        {
            this.FPnl.UserControlString = txtChangeUserControlString.Text;

            //Won't test the Namespace and class properties because the are implicitely tested by this one.
        }
        
        private void ChangeTaskListNode(object sender, EventArgs e)
        {
            TYml2Xml parser = new TYml2Xml( rtbChangeTaskListNode.Lines );
            XmlDocument xmldoc = parser.ParseYML2XML();
            this.FPnl.TaskListNode = xmldoc.FirstChild.NextSibling.FirstChild;
        }
        
        private void ChangeVisualStyle(object sender, EventArgs e)
        {
            switch ( cboChangeVisualStyle.Text )
            {
                case "vsAccordionPanel":
                    this.FPnl.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsAccordionPanel;
                    break;
                case "vsTaskPanel":
                    this.FPnl.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsTaskPanel;
                    break;
                case "vsDashboard":
                    this.FPnl.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsDashboard;
                    break;
                case "vsShepherd":
                    this.FPnl.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsShepherd;
                    break;
                case "vsHorizontalCollapse":
                    this.FPnl.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsHorizontalCollapse;
                    break;
            }
        }
        
    }
}
