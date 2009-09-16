/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Xml;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using Mono.Unix;
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Petra.Client.App.PetraClient
{
    public partial class TFrmMainWindowNew
    {
        private XmlDocument FUINavigation = null;

        private void InitializeManualCode()
        {
            LoadNavigationUI();

            sptNavigation.Panel1.BackColor = sptNavigation.BackColor;
            sptNavigation.Panel2.BackColor = sptNavigation.BackColor;
            sptNavigation.BackColor = System.Drawing.Color.DarkGray;
            sptNavigation.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.SptNavigationSplitterMoving);
        }

        private void WriteToStatusBar(string s)
        {
            this.stbMain.ShowMessage(s);
        }

        private void SptNavigationSplitterMoving(object sender, System.Windows.Forms.SplitterCancelEventArgs e)
        {
            // TODO: hide lowest department radio button, add it to panel pnlMoreButtons
        }

        private ListView CreateNewTaskList(XmlNode ASubmoduleNode)
        {
            ListView lstTasks = new System.Windows.Forms.ListView();

            lstTasks.Dock = DockStyle.Fill;
            lstTasks.Name = "lstTasks" + ASubmoduleNode.Name;
            lstTasks.View = System.Windows.Forms.View.Details;
            lstTasks.FullRowSelect = true;
            lstTasks.MouseUp += new System.Windows.Forms.MouseEventHandler(TaskListMouseUp);
            lstTasks.MouseDown += new System.Windows.Forms.MouseEventHandler(TaskListMouseDown);

            ColumnHeader columnHeader = new System.Windows.Forms.ColumnHeader();
            columnHeader.Text = Catalog.GetString("Task");
            columnHeader.Width = 200;
            lstTasks.Columns.Add(columnHeader);
            columnHeader = new System.Windows.Forms.ColumnHeader();
            columnHeader.Text = Catalog.GetString("Description");
            columnHeader.Width = 300;
            lstTasks.Columns.Add(columnHeader);

            XmlNode TaskGroupNode = ASubmoduleNode.FirstChild;

            while (TaskGroupNode != null)
            {
                if (TaskGroupNode.Name == "SearchBoxes")
                {
                    // TODO Search boxes
                }
                else
                {
                    System.Windows.Forms.ListViewGroup listViewGroup = new System.Windows.Forms.ListViewGroup(
                        GetLabel(TaskGroupNode),
                        System.Windows.Forms.HorizontalAlignment.Left);
                    listViewGroup.Name = TaskGroupNode.Name;
                    lstTasks.Groups.Add(listViewGroup);

                    if (TaskGroupNode.FirstChild == null)
                    {
                        // duplicate group node into task; otherwise you would not notice the error in the yml file?
                        ListViewItem task = new ListViewItem(
                            new string[] {
                                GetLabel(TaskGroupNode),
                                Catalog.GetString(TYml2Xml.GetAttribute(TaskGroupNode, "Description"))
                            }
                            );
                        task.Name = TaskGroupNode.Name;
                        task.Group = listViewGroup;
                        task.Tag = TaskGroupNode;
                        lstTasks.Items.Add(task);
                    }
                    else
                    {
                        XmlNode TaskNode = TaskGroupNode.FirstChild;

                        while (TaskNode != null)
                        {
                            ListViewItem task = new ListViewItem(
                                new string[] {
                                    GetLabel(TaskNode),
                                    Catalog.GetString(TYml2Xml.GetAttribute(TaskNode, "Description"))
                                }
                                );
                            task.Name = TaskNode.Name;
                            task.Group = listViewGroup;
                            task.Tag = TaskNode;
                            lstTasks.Items.Add(task);
                            TaskNode = TaskNode.NextSibling;
                        }
                    }
                }

                TaskGroupNode = TaskGroupNode.NextSibling;
            }

            return lstTasks;
        }

        private void LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            Object tag = ((Control)sender).Tag;
            ListView lstTasks = null;

            if (tag.GetType() == typeof(ListView))
            {
                lstTasks = (ListView)tag;
            }
            else
            {
                lstTasks = CreateNewTaskList((XmlNode)tag);
                ((Control)sender).Tag = lstTasks;
            }

            if (pnlContent.Controls.Count > 0)
            {
                pnlContent.Controls.RemoveAt(0);
            }

            pnlContent.Controls.Add(lstTasks);
        }

        private ListViewItem FSelectedTaskItem = null;

        private void TaskListMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ListView lst = (ListView)sender;
            ListViewHitTestInfo info = lst.HitTest(e.Location);

            FSelectedTaskItem = info.Item;
        }

        private string GetNamespace(XmlNode node)
        {
            if (node == null)
            {
                return "";
            }

            if (TYml2Xml.HasAttribute(node, "Namespace"))
            {
                return TYml2Xml.GetAttribute(node, "Namespace");
            }

            return GetNamespace(node.ParentNode);
        }

        private SortedList <string, Assembly>FGUIAssemblies = new SortedList <string, Assembly>();

        private void TaskListMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ListView lst = (ListView)sender;

            Cursor = Cursors.WaitCursor;

            ListViewHitTestInfo info = lst.HitTest(e.Location);

            if ((info.Item != null) && (info.Item == FSelectedTaskItem))
            {
                XmlNode node = (XmlNode)info.Item.Tag;

                string strNamespace = GetNamespace(node);

                if (strNamespace.Length == 0)
                {
                    WriteToStatusBar("There is no namespace for " + node.Name);
                    Cursor = Cursors.Default;
                    return;
                }

                if (!FGUIAssemblies.Keys.Contains(strNamespace))
                {
                    try
                    {
                        FGUIAssemblies.Add(strNamespace, Assembly.LoadFrom(strNamespace + ".dll"));
                    }
                    catch (Exception exp)
                    {
                        WriteToStatusBar("error loading assembly " + strNamespace + ".dll: " + exp.Message);
                        Cursor = Cursors.Default;
                        return;
                    }
                }

                Assembly asm = FGUIAssemblies[strNamespace];
                string actionClick = TYml2Xml.GetAttribute(node, "ActionClick");
                string actionOpenScreen = TYml2Xml.GetAttribute(node, "ActionOpenScreen");

                if (actionClick.Contains("."))
                {
                    string className = actionClick.Substring(0, actionClick.IndexOf("."));
                    string methodName = actionClick.Substring(actionClick.IndexOf(".") + 1);
                    System.Type classType = asm.GetType(strNamespace + "." + className);

                    if (classType == null)
                    {
                        WriteToStatusBar("cannot find class " + strNamespace + "." + className + " for " + node.Name);
                        Cursor = Cursors.Default;
                        return;
                    }

                    MethodInfo method = classType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);

                    if (method != null)
                    {
                        method.Invoke(null, new object[] { this.Handle });
                    }
                    else
                    {
                        WriteToStatusBar("cannot find method " + className + "." + methodName + " for " + node.Name);
                    }
                }
                else if (actionOpenScreen.Length > 0)
                {
                    string className = actionOpenScreen;
                    System.Type classType = asm.GetType(strNamespace + "." + className);

                    if (classType == null)
                    {
                        WriteToStatusBar("cannot find class " + strNamespace + "." + className + " for " + node.Name);
                        Cursor = Cursors.Default;
                        return;
                    }

                    System.Object screen = Activator.CreateInstance(classType, new object[] { this.Handle });

                    // TODO: if has property LedgerNumber, assign currently selected ledger???

                    MethodInfo method = classType.GetMethod("Show", BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Any,
                        new Type[] { }, null);

                    if (method != null)
                    {
                        method.Invoke(screen, null);
                    }
                    else
                    {
                        WriteToStatusBar("cannot find method " + className + ".Show for " + node.Name);
                    }
                }
                else if (actionClick.Length == 0)
                {
                    WriteToStatusBar("No action defined for " + node.Name);
                }
                else
                {
                    WriteToStatusBar("Invalid action " + actionClick + " defined for " + node.Name);
                }
            }

            Cursor = Cursors.Default;
        }

        private XmlNode GetDepartmentFromNavigationFile(string ADepartmentName)
        {
            XmlNode OpenPetraNode = FUINavigation.FirstChild.NextSibling.FirstChild;
            XmlNode SearchBoxesNode = OpenPetraNode.FirstChild;
            XmlNode MainMenuNode = SearchBoxesNode.NextSibling;
            XmlNode DepartmentNode = MainMenuNode.FirstChild;

            while (DepartmentNode != null)
            {
                if (DepartmentNode.Name == ADepartmentName)
                {
                    return DepartmentNode;
                }

                DepartmentNode = DepartmentNode.NextSibling;
            }

            throw new Exception("TFrmMainWindowNew::GetDepartmentFromNavigationFile cannot find department node " + ADepartmentName);
        }

        private string GetLabel(XmlNode ANode)
        {
            return Catalog.GetString(TYml2Xml.HasAttribute(ANode, "Label") ? TYml2Xml.GetAttribute(ANode,
                    "Label") : StringHelper.ReverseUpperCamelCase(ANode.Name));
        }

        private Panel GetOrCreatePanel(string DepartmentName)
        {
            if (this.sptNavigation.Panel1.Controls.ContainsKey("pnl" + DepartmentName))
            {
                return (Panel) this.sptNavigation.Panel1.Controls["pnl" + DepartmentName];
            }
            else
            {
                XmlNode DepartmentNode = GetDepartmentFromNavigationFile(DepartmentName);

                Panel pnlDepartment = new Panel();
                pnlDepartment.Name = "pnl" + DepartmentName;
                pnlDepartment.Dock = DockStyle.Top;
                pnlDepartment.Size = new System.Drawing.Size(sptNavigation.Width, 1);

                XmlNode ModuleNode = DepartmentNode.LastChild;

                while (ModuleNode != null)
                {
                    Panel pnlModule = new Panel();
                    pnlModule.Dock = DockStyle.Top;

                    Panel pnlModuleCaption = new Panel();
                    pnlModuleCaption.Size = new System.Drawing.Size(sptNavigation.Width, 27);
                    pnlModuleCaption.Dock = DockStyle.Top;
                    pnlModuleCaption.Click += new System.EventHandler(this.CollapseModuleMenu);
                    pnlModule.Controls.Add(pnlModuleCaption);

                    Label lblModule = new Label();
                    lblModule.Font = new System.Drawing.Font("Microsoft Sans Serif",
                        12F,
                        System.Drawing.FontStyle.Bold,
                        System.Drawing.GraphicsUnit.Point,
                        ((byte)(0)));
                    lblModule.ForeColor = System.Drawing.Color.Blue;
                    lblModule.Location = new System.Drawing.Point(8, 2);
                    lblModule.Name = "lbl" + ModuleNode.Name;
                    lblModule.Size = new System.Drawing.Size(153, 23);
                    lblModule.Text = GetLabel(ModuleNode);
                    lblModule.Click += new System.EventHandler(this.CollapseModuleMenu);

                    Button btnCollapse = new Button();
                    btnCollapse.Text = "^";
                    btnCollapse.Tag = pnlModule;
                    btnCollapse.Click += new System.EventHandler(this.CollapseModuleMenu);
                    btnCollapse.Size = new System.Drawing.Size(20, 20);
                    btnCollapse.Location = new System.Drawing.Point(163, 4);

                    XmlNode SubmoduleNode = ModuleNode.FirstChild;

                    Int32 CounterSubmodules = 0;

                    while (SubmoduleNode != null)
                    {
                        if (SubmoduleNode.Name == "SearchBoxes")
                        {
                            // todo Search Boxes for Submodule
                        }
                        else
                        {
                            LinkLabel lblSubmodule = new LinkLabel();
                            lblSubmodule.Name = SubmoduleNode.Name;
                            lblSubmodule.Text = GetLabel(SubmoduleNode);
                            lblSubmodule.Location = new System.Drawing.Point(8, 25 + CounterSubmodules * 20);
                            lblSubmodule.Size = new System.Drawing.Size(153, 20);
                            lblSubmodule.Tag = SubmoduleNode;
                            lblSubmodule.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
                            pnlModule.Controls.Add(lblSubmodule);

                            CounterSubmodules++;
                        }

                        SubmoduleNode = SubmoduleNode.NextSibling;
                    }

                    pnlModule.Size = new System.Drawing.Size(sptNavigation.Width, 5 + CounterSubmodules * 20 + 25);
                    pnlModuleCaption.Controls.Add(lblModule);
                    pnlModuleCaption.Controls.Add(btnCollapse);
                    pnlDepartment.Controls.Add(pnlModule);
                    pnlDepartment.Size = new System.Drawing.Size(sptNavigation.Width, pnlDepartment.Height + pnlModule.Height);

                    ModuleNode = ModuleNode.PreviousSibling;
                }

                this.sptNavigation.Panel1.Controls.Add(pnlDepartment);

                return pnlDepartment;
            }
        }

        private void LoadNavigationUI()
        {
            if (FUINavigation == null)
            {
                TAppSettingsManager opts = new TAppSettingsManager();
                TYml2Xml parser = new TYml2Xml(opts.GetValue("UINavigation.File"));
                FUINavigation = parser.ParseYML2XML();
            }

            XmlNode OpenPetraNode = FUINavigation.FirstChild.NextSibling.FirstChild;
            XmlNode SearchBoxesNode = OpenPetraNode.FirstChild;
            XmlNode MainMenuNode = SearchBoxesNode.NextSibling;
            XmlNode DepartmentNode = MainMenuNode.FirstChild;

            while (DepartmentNode != null)
            {
                RadioButton rbt = new System.Windows.Forms.RadioButton();
                this.sptNavigation.Panel2.Controls.Add(rbt);
                rbt.Appearance = System.Windows.Forms.Appearance.Button;
                rbt.Dock = System.Windows.Forms.DockStyle.Bottom;
                rbt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;

                //rbt.ImageKey = "{#BUTTONIMAGE}";
                //rbt.ImageList = this.imageListButtons;
                rbt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
                rbt.Name = "rbt" + DepartmentNode.Name;
                rbt.Text = GetLabel(DepartmentNode);
                rbt.Size = new System.Drawing.Size(200, 24);
                rbt.CheckedChanged += new System.EventHandler(this.DepartmentCheckedChanged);

                DepartmentNode = DepartmentNode.NextSibling;
            }

            ((RadioButton) this.sptNavigation.Panel2.Controls[0]).Checked = true;
        }

        private void DepartmentCheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbtDepartment = (RadioButton)sender;
            Panel pnlDepartment = GetOrCreatePanel(rbtDepartment.Name.Substring(3));

            if (rbtDepartment.Checked)
            {
                lblNavigationCaption.Text = rbtDepartment.Text;
                pnlDepartment.Show();
            }
            else
            {
                pnlDepartment.Hide();
            }
        }

        private void CollapseModuleMenu(object sender, EventArgs e)
        {
            Button btnModuleCollapse;
            Panel pnlModule;

            if (sender.GetType() == typeof(Panel))
            {
                // sender is the module caption panel
                pnlModule = (Panel)((Control)sender).Parent;
            }
            else
            {
                // either the button or label or icon
                pnlModule = (Panel)((Control)sender).Parent.Parent;
            }

            btnModuleCollapse = (Button)pnlModule.Controls[0].Controls[1];

            if (pnlModule.Height == pnlModule.Controls[0].Height)
            {
                // show the menu in full size again
                pnlModule.Height = (Int32)pnlModule.Tag;
                pnlModule.Parent.Height += (Int32)pnlModule.Tag - pnlModule.Controls[0].Height;
                btnModuleCollapse.Text = "^";
            }
            else
            {
                pnlModule.Tag = pnlModule.Height;
                pnlModule.Parent.Height -= (Int32)pnlModule.Tag - pnlModule.Controls[0].Height;
                pnlModule.Height = pnlModule.Controls[0].Height;
                btnModuleCollapse.Text = "v";
            }
        }
    }
}