//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank (original, different implementation by timotheusp)
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

using Ict.Common;
using Ict.Common.IO;

namespace Ict.Common.Controls
{
    /// <summary>
    /// Displays Tasks within Task Groups in the OpenPetra Main Menu.
    /// </summary>
    public partial class TLstTasks : UserControl
    {
        private static string FUserId;
        private static TLstFolderNavigation.CheckAccessPermissionDelegate FHasAccessPermission;
        private static int FCurrentLedger = -1;
        private static int FInitiallySelectedLedger = -1;
        private static TOpenNewOrExistingForm FOpenNewOrExistingForm;
        private static bool FTaxDeductiblePercentageEnabled = false;
        private static bool FTaxGovIdEnabled = false;
        private static string FTaxGovIdLabel = string.Empty;
        private static bool FDevelopersOnly = false;

        private Dictionary <string, TUcoTaskGroup>FGroups = new Dictionary <string, TUcoTaskGroup>();
        private TaskAppearance FTaskAppearance;
        private bool FSingleClickExecution = false;
        private int FMaxTaskWidth;
        private TExtStatusBarHelp FStatusbar = null;
        private string FResourceDirectory = TAppSettingsManager.GetValue("Resource.Dir");
        private string FCurrentGroupName = null;


        static private SortedList <string, Assembly>FGUIAssemblies = new SortedList <string, Assembly>();
        static private Form FLastOpenedScreen = null;

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        public TLstTasks()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            #endregion
        }

        /// <summary>
        /// Constructor. Generates several Groups of Tasks from an xml document.
        /// </summary>
        /// <param name="ATaskGroups"></param>
        /// <param name="ATaskAppearance" >Initial appearance of the Tasks.</param>
        public TLstTasks(XmlNode ATaskGroups, TaskAppearance ATaskAppearance)
        {
            this.SuspendLayout();

            this.Name = "lstTasks" + ATaskGroups.Name;
            this.AutoScroll = true;
            //            this.HorizontalScroll.Enabled = true;
            this.Resize += new EventHandler(ListResize);

            XmlNode TaskGroupNode = ATaskGroups.FirstChild;

            while (TaskGroupNode != null)
            {
                if (TaskGroupNode.Name == "SearchBoxes")
                {
                    // TODO Search boxes
                }
                else
                {
                    TUcoTaskGroup TaskGroup = new TUcoTaskGroup();
                    TaskGroup.GroupTitle = TLstFolderNavigation.GetLabel(TaskGroupNode);
                    TaskGroup.Name = TaskGroupNode.Name;
                    TIconCache.TIconSize IconSize = ATaskAppearance ==
                                                    TaskAppearance.staLargeTile ? TIconCache.TIconSize.is32by32 : TIconCache.TIconSize.is16by16;

                    Groups.Add(TaskGroup.Name, TaskGroup);

                    if (TaskGroupNode.FirstChild == null)
                    {
                        // duplicate group node into task; otherwise you would not notice the error in the yml file?
                        TUcoSingleTask SingleTask = new TUcoSingleTask();
                        SingleTask.TaskTitle = TLstFolderNavigation.GetLabel(TaskGroupNode);
                        SingleTask.TaskDescription = TYml2Xml.HasAttribute(TaskGroupNode,
                            "Description") ? Catalog.GetString(TYml2Xml.GetAttribute(TaskGroupNode, "Description")) : "";
                        SingleTask.Name = TaskGroupNode.Name;
                        SingleTask.TaskGroup = TaskGroup;
                        SingleTask.Tag = TaskGroupNode;
                        SingleTask.TaskAppearance = ATaskAppearance;
                        SingleTask.TaskImagePath = DetermineIconForTask(TaskGroupNode);
                        SingleTask.TaskImage = TIconCache.IconCache.AddOrGetExistingIcon(
                            SingleTask.TaskImagePath, IconSize);
                        SingleTask.RequestForDifferentIconSize += new TRequestForDifferentIconSize(SingleTask_RequestForDifferentIconSize);

                        if (!FHasAccessPermission(TaskGroupNode, FUserId, false))
                        {
                            SingleTask.Enabled = false;
                        }

                        TaskGroup.Add(SingleTask.Name, SingleTask);
                    }
                    else
                    {
                        XmlNode TaskNode = TaskGroupNode.FirstChild;

                        while (TaskNode != null)
                        {
                            try
                            {
                                // this item should only be displayed if Tax Deductible Percentage is enable
                                if (TaskNode.Name == "RecipientTaxDeductiblePercentages")
                                {
                                    if (!FTaxDeductiblePercentageEnabled)
                                    {
                                        continue;
                                    }
                                }

                                TUcoSingleTask SingleTask = new TUcoSingleTask();
                                SingleTask.TaskTitle = TLstFolderNavigation.GetLabel(TaskNode);
                                SingleTask.TaskDescription = TYml2Xml.HasAttribute(TaskNode,
                                    "Description") ? Catalog.GetString(TYml2Xml.GetAttribute(TaskNode, "Description")) : "";

                                // this item should only be displayed on systems with TaxGovId enabled as a system setting (e.g. Austria)
                                if (TaskNode.Name == "ImportPartnerTaxGovIds")
                                {
                                    if (!FTaxGovIdEnabled)
                                    {
                                        continue;
                                    }

                                    // Set the description for the GovId using the system setting for the label
                                    string placeholder = FTaxGovIdLabel.Length == 0 ? "tax" : FTaxGovIdLabel;
                                    SingleTask.TaskDescription = SingleTask.TaskDescription.Replace("---", placeholder);
                                }

                                SingleTask.Name = TaskNode.Name;
                                SingleTask.TaskGroup = TaskGroup;
                                SingleTask.Tag = TaskNode;
                                SingleTask.TaskAppearance = ATaskAppearance;
                                SingleTask.TaskImagePath = DetermineIconForTask(TaskNode);
                                SingleTask.TaskImage = TIconCache.IconCache.AddOrGetExistingIcon(
                                    SingleTask.TaskImagePath, IconSize);
                                SingleTask.RequestForDifferentIconSize += new TRequestForDifferentIconSize(SingleTask_RequestForDifferentIconSize);

                                if (TTaskList.IsDisabled(TaskNode) || !FHasAccessPermission(TaskNode, FUserId, false))
                                {
                                    SingleTask.Enabled = false;
                                }

                                TaskGroup.Add(SingleTask.Name, SingleTask);
                            }
                            finally
                            {
                                TaskNode = TaskNode.NextSibling;
                            }
                        }
                    }

                    // Add TaskGroup to this UserControls' Controls
                    TaskGroup.Dock = DockStyle.Top;
                    TaskGroup.Margin = new Padding(3);
                    TaskGroup.AutoSize = true;
                    TaskGroup.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                    TaskGroup.TaskClicked += new EventHandler(SingleTask_ExecuteTask);
                    TaskGroup.TaskSelected += new EventHandler(SingleTask_TaskSelected);

                    this.Controls.Add(TaskGroup);

                    // Make sure Task Groups are shown in correct order and not in reverse order.
                    // (This is needed because we 'stack them up' with 'TaskGroup.Dock = DockStyle.Top')
                    TaskGroup.BringToFront();
                }

                TaskGroupNode = TaskGroupNode.NextSibling;
            }

            this.ResumeLayout();
        }

        /// <summary>
        /// Manages the opening of a new/showing of an existing Instance of a Form.
        /// </summary>
        /// <remarks>See implementation in Ict.Petra.Client.CommonForms.TFormsList!</remarks>
        /// <remarks>A call to this Method will create a new Instance of the Form if there
        /// was no running Instance, otherwise it will just activate any Instance of the Form
        /// it finds.</remarks>
        /// <param name="AFormWasAlreadyOpened">False if a new Form was opened, true if an
        /// existing Instance of the Form was activated.</param>
        /// <param name="AForm">Type of the Form to be opened.</param>
        /// <param name="AParentForm"></param>
        /// <param name="ARunShowMethod">Set to true to run the Forms' Show() Method. (Default=false).</param>
        /// <param name="AContext">Context in which the Form runs (default=""). Can get evaluated for
        /// security purposes.</param>
        /// <returns>An Instance of the Form (either newly created or just activated).</returns>
        public delegate Form TOpenNewOrExistingForm(Type AForm, Form AParentForm, out bool AFormWasAlreadyOpened, bool ARunShowMethod,
            string AContext = "");

        /// <summary>
        /// This property is used to provide a function which opens a new or existing Form.
        /// </summary>
        /// <description>The Delegate is set up at the start of the application.</description>
        public static TOpenNewOrExistingForm OpenNewOrExistingForm
        {
            get
            {
                return FOpenNewOrExistingForm;
            }

            set
            {
                FOpenNewOrExistingForm = value;
            }
        }

        private Image SingleTask_RequestForDifferentIconSize(string ATaskImagePath, TIconCache.TIconSize AIconSize)
        {
            return TIconCache.IconCache.AddOrGetExistingIcon(ATaskImagePath, AIconSize);
        }

        //
        // Get the Icon for this task. If none was supplied, I'll return my parent's icon.
        private string DetermineIconForTask(XmlNode TaskNode)
        {
            if (TYml2Xml.HasAttribute(TaskNode, "Icon"))
            {
                return FResourceDirectory + System.IO.Path.DirectorySeparatorChar +
                       TaskNode.Attributes["Icon"].Value;
            }

            if (TaskNode.ParentNode != null)
            {
                return DetermineIconForTask(TaskNode.ParentNode);
            }

            return null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Groups that are to be shown in the Task List.
        /// </summary>
        public Dictionary <string, TUcoTaskGroup>Groups
        {
            get
            {
                return FGroups;
            }
        }

        /// <summary>
        /// Appearance of the Task (Large Tile, ListEntry).
        /// </summary>
        public TaskAppearance TaskAppearance
        {
            get
            {
                return FTaskAppearance;
            }

            set
            {
                if (FTaskAppearance != value)
                {
                    FTaskAppearance = value;

                    foreach (var Group in Groups)
                    {
                        Group.Value.TaskAppearance = FTaskAppearance;
                    }
                }
            }
        }

        /// <summary>
        /// Execution of the Task with a single click of the mouse?
        /// </summary>
        public bool SingleClickExecution
        {
            get
            {
                return FSingleClickExecution;
            }

            set
            {
                if (FSingleClickExecution != value)
                {
                    FSingleClickExecution = value;

                    foreach (var Group in Groups)
                    {
                        Group.Value.SingleClickExecution = FSingleClickExecution;
                    }
                }
            }
        }

        /// <summary>
        /// Maximum Task Width.
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

                    foreach (var Group in Groups)
                    {
                        Group.Value.MaxTaskWidth = value;
                    }
                }
            }
        }

        /// <summary>
        /// The object of the last opened screen - useful for testing.
        /// </summary>
        static public Form LastOpenedScreen
        {
            get
            {
                return FLastOpenedScreen;
            }
        }

        /// <summary>
        /// Sets the Status Bar Text so that error messages can be displayed.
        /// </summary>
        public TExtStatusBarHelp Statusbar
        {
            set
            {
                FStatusbar = value;
            }
        }

        /// <summary>
        /// The currently selected Ledger
        /// </summary>
        public static int CurrentLedger
        {
            get
            {
                return FCurrentLedger;
            }

            set
            {
                FCurrentLedger = value;
            }
        }

        /// <summary>
        /// The initially selected ledger (either a user default or the first ledger in the list)
        /// </summary>
        public static int InitiallySelectedLedger
        {
            get
            {
                return FInitiallySelectedLedger;
            }
            set
            {
                FInitiallySelectedLedger = value;
            }
        }

        /// <summary>
        /// Get the group name for the currently selected task item
        /// </summary>
        public string CurrentGroupName
        {
            get
            {
                return FCurrentGroupName;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Fired when a Task is clicked by the user.
        /// </summary>
        public event EventHandler TaskClicked;

        /// <summary>
        /// Fired when a Task is selected by the user (in a region of the Control where a TaskClick isn't fired).
        /// </summary>
        public event EventHandler TaskSelected;

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialise the permissions callback function for the current user.
        /// </summary>
        /// <param name="AUserId"></param>
        /// <param name="AHasAccessPermission"></param>
        /// <param name="ATaxDeductiblePercentageEnabled"></param>
        /// <param name="ATaxGovIdEnabled"></param>
        /// <param name="ATaxGovIdLabel"></param>
        /// <param name="ADevelopersOnly"></param>
        public static void Init(string AUserId,
            TLstFolderNavigation.CheckAccessPermissionDelegate AHasAccessPermission,
            bool ATaxDeductiblePercentageEnabled = false, bool ATaxGovIdEnabled = false, bool ADevelopersOnly = false, string ATaxGovIdLabel = "")
        {
            FUserId = AUserId;
            FHasAccessPermission = AHasAccessPermission;
            FTaxDeductiblePercentageEnabled = ATaxDeductiblePercentageEnabled;
            FTaxGovIdEnabled = ATaxGovIdEnabled;
            FDevelopersOnly = ADevelopersOnly;

            if (FTaxGovIdEnabled)
            {
                FTaxGovIdLabel = ATaxGovIdLabel;
            }
        }

        /// <summary>
        /// Execute action from the navigation tree.
        /// </summary>
        /// <returns>The error or status message.</returns>
        public static string ExecuteAction(XmlNode node, Form AParentWindow)
        {
            bool FormWasAlreadyOpened = false;
            string Context = String.Empty;

            string strNamespace = TYml2Xml.GetAttributeRecursive(node, "Namespace");

            if (strNamespace.Length == 0)
            {
                return "There is no namespace for " + node.Name;
            }

            if (TCommonControlsSecurity.CheckUserAccessToModuleUsingModuleNamespaceName(node) == false)
            {
                return Catalog.GetString("Access denied");
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

                    if (SomeParentDependsOnLedger(node))
                    {
                        parameters.Add((object)FCurrentLedger);
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
                System.Object screen = null;

                System.Type classType = asm.GetType(strNamespace + "." + className);

                if (classType == null)
                {
                    return "cannot find class " + strNamespace + "." + className + " for " + node.Name;
                }

                // TODO: check if user has permissions for this screen?
                // needs to be implemented as a static function of the screen, GetRequiredPermission returns the permission that is needed (eg PTNRUSER)
                // also use something similar as in lstFolderNavigation: CheckAccessPermissionDelegate?
                // delegate as a static function that is available from everywhere?

                // check for Context property
                foreach (PropertyInfo prop in classType.GetProperties())
                {
                    if (TYml2Xml.HasAttributeRecursive(node, prop.Name))
                    {
                        if (prop.Name == "Context")
                        {
                            Context = TYml2Xml.GetAttributeRecursive(node, prop.Name);
                        }
                    }
                }

                try
                {
                    if (OpenNewOrExistingForm != null)
                    {
                        screen = OpenNewOrExistingForm(classType, AParentWindow, out FormWasAlreadyOpened, false, Context);
                    }
                    else
                    {
                        screen = Activator.CreateInstance(classType, new object[] { AParentWindow, Context });
                    }
                }
                catch (System.Reflection.TargetInvocationException E)
                {
                    TLogging.Log(E.ToString());

                    throw;
                }

                if (!FormWasAlreadyOpened)
                {
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

                        if (prop.Name == "LedgerNumber")
                        {
                            if (SomeParentDependsOnLedger(node))
                            {
                                prop.SetValue(screen, (object)FCurrentLedger, null);
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

        #endregion

        #region Private Methods

        /// <summary>
        /// Recursively check all Node's ParentNodes for the 'DependsOnLedger' Attribute to be true.
        /// </summary>
        /// <param name="ANode">Node whose ParentNodes are to be checked.</param>
        /// <returns>True if any of <paramref name="ANode" />' ParentNodes has got the 'DependsOnLedger' Attribute
        /// set to true, otherwise false.</returns>
        private static bool SomeParentDependsOnLedger(XmlNode ANode)
        {
            XmlNode InspectNode = ANode.ParentNode;

            if (InspectNode != null)
            {
                if ((InspectNode.Attributes != null)
                    && (InspectNode.Attributes.Count > 0))
                {
                    if (InspectNode.Attributes["DependsOnLedger"] != null)
                    {
                        return InspectNode.Attributes["DependsOnLedger"].Value == "true";
                    }
                    else
                    {
                        return SomeParentDependsOnLedger(InspectNode);
                    }
                }
                else
                {
                    if (InspectNode.Name != TYml2Xml.ROOTNODEINTERNAL)
                    {
                        return SomeParentDependsOnLedger(InspectNode);
                    }
                    else
                    {
                        // We have reached the top of the 'sensible' ParentNodes and haven't found the 'DependsOnLedger' Attribute
                        // to be true on any of the lower-level ParentNodes
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        void ListResize(object sender, EventArgs e)
        {
            foreach (var Group in Groups)
            {
                Group.Value.MaximumSize = new System.Drawing.Size(this.Width, 0);
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

//      protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
//      {
        //            // Set a fixed width for the control.
        //            // ADD AN EXTRA HEIGHT VALIDATION TO AVOID INITIALIZATION PROBLEMS
        //            // BITWISE 'AND' OPERATION: IF ZERO THEN HEIGHT IS NOT INVOLVED IN THIS OPERATION
        //            if ((specified&BoundsSpecified.Width) == 0 || width == MaxTaskWidth)
        //            {
        //                  if (width < MaxTaskWidth)
        //                  {
        ////TLogging.Log("SetBoundsCore: Before setting ucoTaskGroup " + Name + "'s Width to " + MaxTaskWidth.ToString() + ": Size = " + Size.ToString());
        //                    base.SetBoundsCore(x, y, MaxTaskWidth, height, specified);
        ////TLogging.Log("SetBoundsCore: After setting ucoTaskGroup " + Name + "'s Width to " + MaxTaskWidth.ToString() + ": Size = " + Size.ToString());
        //                }
//          }
        //            else if ((specified&BoundsSpecified.Height) == 0)
        //            {
        //                base.SetBoundsCore(x, y, width, this.Height, specified);
        //            }
//          else
//          {
        //                return;
//          }
        //TLogging.Log("SetBoundsCore: TLstTask " + Name + "'s size: " + Size.ToString());
//      }

        #endregion

        #region Event Handling

        void SingleTask_ExecuteTask(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Control parentForm = Parent;

            while (parentForm != null && !(parentForm is Form))
            {
                parentForm = parentForm.Parent;
            }

            try
            {
                string message = ExecuteAction((XmlNode)((TUcoSingleTask)sender).Tag, (Form)parentForm);
                WriteToStatusBar(message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        void SingleTask_TaskSelected(object sender, EventArgs e)
        {
            this.SuspendLayout();

            foreach (Control TaskGroups in this.Controls)
            {
                foreach (Control TaskGroup in TaskGroups.Controls)
                {
                    foreach (TUcoSingleTask Task in TaskGroup.Controls)
                    {
                        if (Task == sender)
                        {
                            FCurrentGroupName = TaskGroups.Name;
                        }
                        else
                        {
                            Task.DeselectTask();
                        }
                    }
                }
            }

            this.ResumeLayout();
        }

        void FireTaskClicked()
        {
            if (TaskClicked != null)
            {
                TaskClicked(this, null);
            }
        }

        void FireTaskSelected(object sender, EventArgs e)
        {
            if (TaskSelected != null)
            {
                TaskSelected(sender, null);
            }
        }

        #endregion
    }
}
