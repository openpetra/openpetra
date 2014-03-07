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
using System.Drawing;
using System.Windows.Forms;

using Ict.Common.Controls;

namespace ControlTestBench
{
/// <summary>
/// Description of CollapsiblePanelHosterTest.
/// </summary>
public partial class CollapsiblePanelHosterTest : Form
{
    XmlNode FTestYAMLNode = null;
    TVisualStylesEnum FEnumStyle = TVisualStylesEnum.vsTaskPanel;
    TPnlCollapsibleHoster FCollPanelHoster;

    /// <summary>
    /// Constructor.
    /// </summary>
    public CollapsiblePanelHosterTest()
    {
        //
        // The InitializeComponent() call is required for Windows Forms designer support.
        //
        InitializeComponent();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ATestYAMLNode"></param>
    /// <param name="AEnumStyle"></param>
    public CollapsiblePanelHosterTest(XmlNode ATestYAMLNode, TVisualStylesEnum AEnumStyle) : this()
    {
        FTestYAMLNode = ATestYAMLNode;
        FEnumStyle = AEnumStyle;


        FCollPanelHoster = new TPnlCollapsibleHoster(FTestYAMLNode, FEnumStyle);
        FCollPanelHoster.Dock = DockStyle.Fill;
        pnlCollapsiblePanelHostTest.Controls.Add(FCollPanelHoster);

        FCollPanelHoster.RealiseCollapsiblePanelsNow();

        HookupItemActivationEvent();
    }

    void BtnGetTaskList1Click(object sender, System.EventArgs e)
    {
        MessageBox.Show(String.Format("Task List #1's MasterXmlNode has got {0} Child Nodes",
                FCollPanelHoster.GetTaskListInstance(0).MasterXmlNode.ChildNodes.Count));
    }

    void BtnGetTaskList2Click(object sender, System.EventArgs e)
    {
        MessageBox.Show(String.Format("Task List #2's MasterXmlNode has got {0} Child Nodes",
                FCollPanelHoster.GetTaskListInstance(1).MasterXmlNode.ChildNodes.Count));
    }

    void BtnGetCollPanel1Click(object sender, System.EventArgs e)
    {
        MessageBox.Show(String.Format("Collapsible Panel #1's ExpandedSize is {0} pixels",
                FCollPanelHoster.GetCollapsiblePanelInstance(0).ExpandedSize));
    }

    void BtnGetCollPanel2Click(object sender, System.EventArgs e)
    {
        MessageBox.Show(String.Format("Collapsible Panel #2's ExpandedSize is {0} pixels",
                FCollPanelHoster.GetCollapsiblePanelInstance(1).ExpandedSize));
    }

    void BtnGetActiveTaskClick(object sender, EventArgs e)
    {
        XmlNode ActiveTask = FCollPanelHoster.ActiveTaskItem;

        if (ActiveTask != null)
        {
            MessageBox.Show("Active Task: " + ActiveTask.Name);
        }
        else
        {
            MessageBox.Show("There is no active Task!");
        }
    }

    void BtnSetActiveTaskClick(object sender, EventArgs e)
    {
        FCollPanelHoster.ActiveTaskItem = TaskListTest.FindTaskNodeByName(txtTaskName.Text, FCollPanelHoster.MasterXmlNode);
    }

    private void HookupItemActivationEvent()
    {
        this.FCollPanelHoster.ItemActivation += new TTaskList.TaskLinkClicked(CollPanelHoster_ItemActivation);
    }

    void CollPanelHoster_ItemActivation(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked, object AOtherData)
    {
        MessageBox.Show(String.Format("Task '{0}' with Label '{1}' got clicked.", ATaskListNode.Name, AItemClicked.Text));
    }
}
}