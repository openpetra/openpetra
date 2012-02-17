//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using System.IO;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using GNU.Gettext;
using Ict.Common.IO;

namespace Ict.Common.Controls
{
    /// <summary>
    /// this class fills a ListView with tasks,
    /// and executes the tasks using reflection
    /// </summary>
    public class TLstTasks : System.Windows.Forms.ListView
    {
        private static string FUserId;
        private static CheckAccessPermissionDelegate FHasAccessPermission;

        /// <summary>
        /// this function checks if the user has access to the navigation node
        /// </summary>
        public delegate bool CheckAccessPermissionDelegate(XmlNode ANode, string AUserId);

        /// <summary>
        /// initialise the permissions callback function for the current user
        /// </summary>
        /// <param name="AUserId"></param>
        /// <param name="AHasAccessPermission"></param>
        public static void Init(string AUserId, CheckAccessPermissionDelegate AHasAccessPermission)
        {
            FUserId = AUserId;
            FHasAccessPermission = AHasAccessPermission;
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public TLstTasks()
        {
        }

        /// <summary>
        /// constructor that generates several groups of tasks from an xml document
        /// </summary>
        /// <param name="ATaskGroups"></param>
        public TLstTasks(XmlNode ATaskGroups)
        {
            this.Dock = DockStyle.Fill;
            this.Name = "lstTasks" + ATaskGroups.Name;
            this.View = System.Windows.Forms.View.Details;
            this.FullRowSelect = true;
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(TaskListMouseUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(TaskListMouseDown);

            ColumnHeader columnHeader = new System.Windows.Forms.ColumnHeader();
            columnHeader.Text = Catalog.GetString("Task");
            columnHeader.Width = 200;
            this.Columns.Add(columnHeader);
            columnHeader = new System.Windows.Forms.ColumnHeader();
            columnHeader.Text = Catalog.GetString("Description");
            columnHeader.Width = 300;
            this.Columns.Add(columnHeader);

            XmlNode TaskGroupNode = ATaskGroups.FirstChild;

            while (TaskGroupNode != null)
            {
                if (TaskGroupNode.Name == "SearchBoxes")
                {
                    // TODO Search boxes
                }
                else
                {
                    System.Windows.Forms.ListViewGroup listViewGroup = new System.Windows.Forms.ListViewGroup(
                        TLstFolderNavigation.GetLabel(TaskGroupNode),
                        System.Windows.Forms.HorizontalAlignment.Left);
                    listViewGroup.Name = TaskGroupNode.Name;
                    this.Groups.Add(listViewGroup);

                    if (TaskGroupNode.FirstChild == null)
                    {
                        // duplicate group node into task; otherwise you would not notice the error in the yml file?
                        ListViewItem task = new ListViewItem(
                            new string[] {
                                TLstFolderNavigation.GetLabel(TaskGroupNode),
                                TYml2Xml.HasAttribute(TaskGroupNode,
                                    "Description") ? Catalog.GetString(TYml2Xml.GetAttribute(TaskGroupNode, "Description")) : ""
                            }
                            );
                        task.Name = TaskGroupNode.Name;
                        task.Group = listViewGroup;
                        task.Tag = TaskGroupNode;

                        if (!FHasAccessPermission(TaskGroupNode, FUserId))
                        {
                            task.ForeColor = Color.Gray;
                        }

                        this.Items.Add(task);
                    }
                    else
                    {
                        XmlNode TaskNode = TaskGroupNode.FirstChild;

                        while (TaskNode != null)
                        {
                            ListViewItem task = new ListViewItem(
                                new string[] {
                                    TLstFolderNavigation.GetLabel(TaskNode),
                                    TYml2Xml.HasAttribute(TaskNode, "Description") ? Catalog.GetString(TYml2Xml.GetAttribute(TaskNode,
                                            "Description")) : ""
                                }
                                );
                            task.Name = TaskNode.Name;
                            task.Group = listViewGroup;
                            task.Tag = TaskNode;

                            if (!FHasAccessPermission(TaskNode, FUserId))
                            {
                                task.ForeColor = Color.Gray;
                            }

                            this.Items.Add(task);
                            TaskNode = TaskNode.NextSibling;
                        }
                    }
                }

                TaskGroupNode = TaskGroupNode.NextSibling;
            }
        }

        private ListViewItem FSelectedTaskItem = null;

        private void TaskListMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ListView lst = (ListView)sender;
            ListViewHitTestInfo info = lst.HitTest(e.Location);

            FSelectedTaskItem = info.Item;
        }

        static private SortedList <string, Assembly>FGUIAssemblies = new SortedList <string, Assembly>();
        static private Form FLastOpenedScreen = null;

        /// <summary>
        /// the object of the last opened screen.
        /// useful for testing
        /// </summary>
        static public Form LastOpenedScreen
        {
            get
            {
                return FLastOpenedScreen;
            }
        }

        /// <summary>
        /// execute action from the navigation tree
        /// </summary>
        /// <returns>the error or status message</returns>
        public static string ExecuteAction(XmlNode node, Form AParentWindow)
        {
            if (!FHasAccessPermission(node, FUserId))
            {
                return Catalog.GetString("Sorry, you don't have enough permissions to do this");
            }

            string strNamespace = TYml2Xml.GetAttributeRecursive(node, "Namespace");

            if (strNamespace.Length == 0)
            {
                return "There is no namespace for " + node.Name;
            }

            if (!FGUIAssemblies.Keys.Contains(strNamespace))
            {
                // work around dlls containing several namespaces, eg Ict.Petra.Client.MFinance.Gui contains AR as well
                string DllName = TAppSettingsManager.ApplicationDirectory + Path.DirectorySeparatorChar + strNamespace;

                if (!System.IO.File.Exists(DllName + ".dll"))
                {
                    DllName = DllName.Substring(0, DllName.LastIndexOf("."));
                }

                try
                {
                    FGUIAssemblies.Add(strNamespace, Assembly.LoadFrom(DllName + ".dll"));
                }
                catch (Exception exp)
                {
                    return "error loading assembly " + strNamespace + ".dll: " + exp.Message;
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
                    return "cannot find class " + strNamespace + "." + className + " for " + node.Name;
                }

                MethodInfo method = classType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);

                if (method != null)
                {
                    List <object>parameters = new List <object>();
                    parameters.Add(AParentWindow);

                    // Check the parameters, if we have such an attribute
                    foreach (ParameterInfo param in method.GetParameters())
                    {
                        // ignore the first letter, A, eg. in ALedgerNumber
                        if (TYml2Xml.HasAttributeRecursive(node, param.Name.Substring(1)))
                        {
                            Object obj = TYml2Xml.GetAttributeRecursive(node, param.Name.Substring(1));

                            if (param.ParameterType == typeof(Int32))
                            {
                                obj = Convert.ToInt32(obj);
                            }
                            else if (param.ParameterType == typeof(Int64))
                            {
                                obj = Convert.ToInt64(obj);
                            }
                            else if (param.ParameterType == typeof(bool))
                            {
                                obj = Convert.ToBoolean(obj);
                            }
                            else if (param.ParameterType == typeof(string))
                            {
                                // leave it as string
                            }
                            else if (param.ParameterType.IsEnum)
                            {
                                obj = Enum.Parse(param.ParameterType, obj.ToString(), true);
                            }
                            else
                            {
                                // to avoid that Icon is set etc, clear obj
                                obj = null;
                            }

                            if (obj != null)
                            {
                                parameters.Add(obj);
                            }
                        }
                    }

                    method.Invoke(null, parameters.ToArray());
                }
                else
                {
                    return "cannot find method " + className + "." + methodName + " for " + node.Name;
                }
            }
            else if (actionOpenScreen.Length > 0)
            {
                string className = actionOpenScreen;

                System.Type classType = asm.GetType(strNamespace + "." + className);

                if (classType == null)
                {
                    return "cannot find class " + strNamespace + "." + className + " for " + node.Name;
                }

                // TODO: check if user has permissions for this screen?
                // needs to be implemented as a static function of the screen, GetRequiredPermission returns the permission that is needed (eg PTNRUSER)
                // also use something similar as in lstFolderNavigation: CheckAccessPermissionDelegate?
                // delegate as a static function that is available from everywhere?

                System.Object screen = Activator.CreateInstance(classType, new object[] { AParentWindow });

                // check for properties and according attributes; this works for the LedgerNumber at the moment
                foreach (PropertyInfo prop in classType.GetProperties())
                {
                    if (TYml2Xml.HasAttributeRecursive(node, prop.Name))
                    {
                        Object obj = TYml2Xml.GetAttributeRecursive(node, prop.Name);

                        if (prop.PropertyType == typeof(Int32))
                        {
                            obj = Convert.ToInt32(obj);
                        }
                        else if (prop.PropertyType == typeof(Int64))
                        {
                            obj = Convert.ToInt64(obj);
                        }
                        else if (prop.PropertyType == typeof(bool))
                        {
                            obj = Convert.ToBoolean(obj);
                        }
                        else if (prop.PropertyType == typeof(string))
                        {
                            // leave it as string
                        }
                        else if (prop.PropertyType.IsEnum)
                        {
                            obj = Enum.Parse(prop.PropertyType, obj.ToString(), true);
                        }
                        else
                        {
                            // to avoid that Icon is set etc, clear obj
                            obj = null;
                        }

                        if (obj != null)
                        {
                            prop.SetValue(screen, obj, null);
                        }
                    }
                }

                MethodInfo method = classType.GetMethod("Show", BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Any,
                    new Type[] { }, null);

                if (method != null)
                {
                    method.Invoke(screen, null);
                    FLastOpenedScreen = (Form)screen;
                }
                else
                {
                    return "cannot find method " + className + ".Show for " + node.Name;
                }
            }
            else if (actionClick.Length == 0)
            {
                return "No action defined for " + node.Name;
            }
            else
            {
                return "Invalid action " + actionClick + " defined for " + node.Name;
            }

            return "";
        }

        private void TaskListMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ListView lst = (ListView)sender;

            Cursor = Cursors.WaitCursor;

            ListViewHitTestInfo info = lst.HitTest(e.Location);

            if ((info.Item != null) && (info.Item == FSelectedTaskItem))
            {
                Control parentForm = Parent;

                while (parentForm != null && !(parentForm is Form))
                {
                    parentForm = parentForm.Parent;
                }

                string message = ExecuteAction((XmlNode)info.Item.Tag, (Form)parentForm);
                WriteToStatusBar(message);
            }

            Cursor = Cursors.Default;
        }

        private TExtStatusBarHelp FStatusbar = null;

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

        private void WriteToStatusBar(string s)
        {
            if (FStatusbar != null)
            {
                FStatusbar.ShowMessage(s);
            }
            else
            {
                // TODO: does this work? which is the current statusbar?
                TLogging.Log(s, TLoggingType.ToStatusBar);
            }
        }
    }
}