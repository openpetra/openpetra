//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//		 sethb
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
using System.Windows.Forms;
using System.Xml;
using Ict.Common.Controls;
using Ict.Common.IO;

namespace ControlTestBench
{
/// <summary>
/// </summary>
public partial class TaskListTest : Form
{
    const string TASKLISTCONTROLNAME = "TaskListDemo";

    XmlNode FTestYAMNode = null;
    TVisualStylesEnum FEnumStyle = TVisualStylesEnum.vsDashboard;

    /// <summary>
    /// </summary>
    public TaskListTest()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ATestYAMLNode"></param>
    /// <param name="AEnumStyle"></param>
    public TaskListTest(XmlNode ATestYAMLNode, TVisualStylesEnum AEnumStyle) : this()
    {
        FTestYAMNode = ATestYAMLNode;
        FEnumStyle = AEnumStyle;
    }

    /// <summary>
    /// </summary>
    public void TestDefaultConstructor(object sender, EventArgs e)
    {
        XmlNode xmlnode = GetTestXMLNode();

        TaskList1 = new Ict.Common.Controls.TTaskList(xmlnode);

        TaskList1.Location = new System.Drawing.Point(10, 10);
        TaskList1.Size = new System.Drawing.Size(300, 200);
        TaskList1.Name = TASKLISTCONTROLNAME;

        AddControlToForm(TaskList1);
        HookupTaskListEvents(TaskList1);
    }

    /// <summary>
    /// </summary>
    public void TestFullConstructor(object sender, EventArgs e)
    {
        System.Xml.XmlNode xmlnode = GetTestXMLNode();

        TaskList1 = new Ict.Common.Controls.TTaskList(xmlnode, FEnumStyle);

        TaskList1.Location = new System.Drawing.Point(10, 10);
        TaskList1.Size = new System.Drawing.Size(300, 200);
        TaskList1.Name = TASKLISTCONTROLNAME;

        AddControlToForm(TaskList1);
        HookupTaskListEvents(TaskList1);
    }

    /// <summary>
    /// This is copied from the older version of the test written by Chadd and Ashley.
    /// </summary>
    void DisableItemButtonClick(object sender, EventArgs e)
    {
        TTaskList TaskList = (TTaskList) this.Controls[TASKLISTCONTROLNAME];
        XmlNode temp = TaskList.GetTaskByName("Task7");

        if (temp != null)
        {
            if (TaskList.IsDisabled(temp))
            {
                TaskList.EnableTaskItem(temp);
            }
            else
            {
                TaskList.DisableTaskItem(temp);
            }
        }
    }

    /// <summary>
    /// This is copied from the older version of the test written by Chadd and Ashley.
    /// </summary>
    void HideItemButtonClick(object sender, EventArgs e)
    {
        TTaskList TaskList = (TTaskList) this.Controls[TASKLISTCONTROLNAME];
        XmlNode temp = TaskList.GetTaskByName("Task4c");

        if (temp != null)
        {
            if (!TaskList.IsVisible(temp))
            {
                TaskList.ShowTaskItem(temp);
            }
            else
            {
                TaskList.HideTaskItem(temp);
            }
        }

        //temp = TaskList.GetTaskByNumber("3");
        //if(temp != null){
        //TaskList.ShowTaskItem(temp);
        //}
    }

    /// <summary>
    /// </summary>
    private XmlNode GetHardCodedXmlNodes()
    {
        string[] lines = new string[7];
        lines[0] = "TaskGroup:\n";
        lines[1] = "    Task1:\n";
        lines[2] = "        Label: First Item";
        lines[3] = "    Task2:\n";
        lines[4] = "        Label: Second Item";
        lines[5] = "    Task3:\n";
        lines[6] = "        Label: Third Item";
        TYml2Xml parser = new TYml2Xml(lines);
        XmlNode xmlnode = parser.ParseYML2TaskListRoot();
        return xmlnode;
    }

    /// <summary>
    /// </summary>
    public void ExampleCallback()
    {
        MessageBox.Show("The callback worked.",
            "Tests.Common.Controls.TTestTaskList.ExampleCallback",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void AddControlToForm(Control AControl)
    {
        Control ExistingTaskListCtrl = this.Controls[TASKLISTCONTROLNAME];

        if (ExistingTaskListCtrl != null)
        {
            this.Controls.Remove(ExistingTaskListCtrl);
        }

        //            AControl.Dock = DockStyle.Left;

        this.Controls.Add(AControl);
    }

    private XmlNode GetTestXMLNode()
    {
        XmlNode xmlnode;

        if (FTestYAMNode == null)
        {
            xmlnode = GetHardCodedXmlNodes();
        }
        else
        {
            xmlnode = FTestYAMNode;
        }

        return xmlnode;
    }

    private void HookupTaskListEvents(TTaskList ATaskList)
    {
        ATaskList.ItemActivation += new TTaskList.TaskLinkClicked(ATaskList_ItemActivation);
    }

    void ATaskList_ItemActivation(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked)
    {
        MessageBox.Show(String.Format("Task '{0}' with Label '{1}' got clicked.", ATaskListNode.Name, AItemClicked.Text));
    }

    void BtnGetActiveTaskClick(object sender, EventArgs e)
    {
        XmlNode ActiveTask = null;
        Control ExistingTaskListCtrl = this.Controls[TASKLISTCONTROLNAME];

        if (ExistingTaskListCtrl != null)
        {
            ActiveTask = ((TTaskList)ExistingTaskListCtrl).ActiveTaskItem;
        }

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
        XmlNode FoundNode = null;

        Control ExistingTaskListCtrl = this.Controls[TASKLISTCONTROLNAME];

        if (ExistingTaskListCtrl != null)
        {
            FoundNode = FindTaskNodeByName(txtTaskName.Text, ((TTaskList)ExistingTaskListCtrl).MasterXmlNode);

            if (FoundNode != null)
            {
                ((TTaskList)ExistingTaskListCtrl).ActiveTaskItem = FoundNode;
            }
        }
        else
        {
            MessageBox.Show("There is no TaskList Instance!");
        }
    }

    /// <summary>
    /// todoComment
    /// </summary>
    /// <param name="AName"></param>
    /// <param name="ASearchNode"></param>
    /// <returns></returns>
    public static XmlNode FindTaskNodeByName(string AName, XmlNode ASearchNode)
    {
        XmlNode FoundNode = null;

        foreach (XmlNode TaskNode in ASearchNode)
        {
            if (TaskNode.Name == AName)
            {
                FoundNode = TaskNode;
                break;
            }

            //If the TaskNode has Children, do subtasks
            if (TaskNode.HasChildNodes)
            {
                FoundNode = FindTaskNodeByName(AName, TaskNode);

                if (FoundNode != null)
                {
                    break;
                }
            }
        }

        return FoundNode;
    }
}
}