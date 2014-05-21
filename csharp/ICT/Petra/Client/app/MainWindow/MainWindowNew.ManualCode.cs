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
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.IO;
using Ict.Common.Controls;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MReporting;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MConference.Data;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.MFinance.Gui.Setup;

namespace Ict.Petra.Client.App.PetraClient
{
    public partial class TFrmMainWindowNew
    {
        #region Resource strings

        private readonly string StrCannotClosePetra1stLine = Catalog.GetString("Cannot close Petra Main Menu.");
        private readonly string StrCannotClosePetra2ndLine = Catalog.GetString("The following window(s) must be closed first:");
        private readonly string StrCannotClosePetraChangeInfoLine = Catalog.GetString(
            "Note: Windows with unsaved changes are marked with '(*)' in this list.");
        private readonly string StrCannotClosePetraTitle = Catalog.GetString("Open Windows Must Be Closed");

        #endregion

        private const string VIEWTASKS_TILES = "Tiles";
        private const string VIEWTASKS_LIST = "List";

        private static bool FMultiLedgerSite = false;
        private static int FCurrentLedger = -1;
        private static List <string>FLedgersAvailableToUser = null;
        TBreadcrumbTrail FBreadcrumbTrail;

        private static bool FConferenceSelected = false;
        private static Int64 FConferenceKey = 0;
        private PetraClient_AutomatedAppTest.TAutomatedAppTest TestRunner;

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
        /// The currently selected Conference
        /// </summary>
        public static Int64 SelectedConferenceKey
        {
            get
            {
                return FConferenceKey;
            }
            set
            {
                FConferenceKey = value;
            }
        }

        private void InitializeManualCode()
        {
            // Currently, only the Main Menu gets an 'OpenPetra styled' StatusBar (an 'OpenPetra styled' StatusBar
            // doesn't go with normal Forms at the moment as pnlContent's BackColor [and UserControls] is white
            // in colour and that doesn't look that good with an 'OpenPetra styled' StatusBar at the bottom).
            stbMain.UseOpenPetraToolStripRenderer = true;

            InitialiseTopPanel();

            LoadNavigationUI();

            Version version = new Version(TClientInfo.ClientAssemblyVersion);

            if (version.Revision > 20)
            {
                this.Text = "OpenPetra.org " + version.ToString(4);
            }
            else
            {
                // leave out 'Revision'
                this.Text = "OpenPetra.org " + version.ToString(3);
            }
        }

        private void RunOnceOnActivationManual()
        {
            RunTestAction();
        }

        /// <summary>
        /// For development and testing purposes this Method can either execute actions that
        /// are set up by the program 'PetraMultiStart' (indicated by 'RunAutoTests=true' on
        /// the command line) OR open a screen with parameters that
        /// come either from the .config file or Command Line (indicated by 'TestAction="xxx"').
        /// The 'Test Action' will not be run if the Control Key is pressed.
        /// </summary>
        /// <remarks>
        /// sample action: TestAction="Namespace=Ict.Petra.Client.MPartner.Gui,ActionOpenScreen=TFrmPartnerEdit2,PartnerKey=0043005002,InitiallySelectedTabPage=petpDetails"
        ///</remarks>
        private void RunTestAction()
        {
            string DisconnectTimeFromCommandLine = TAppSettingsManager.GetValue("DisconnectTime");

            if (TAppSettingsManager.GetBoolean("RunAutoTests", false) == true)
            {
                // We need to manually 'fix up' the value of DisconnectTime that we get from .NET when we request
                // the commandline parameters as an array because .NET removes quotation marks in two places where
                // they were present on the command line. Those two quotation marks need to be there as the call to
                // TVariant.DecodeFromString() will not succeed if they aren't there in their proper places!
                DisconnectTimeFromCommandLine = DisconnectTimeFromCommandLine.Substring(
                    0, DisconnectTimeFromCommandLine.IndexOf(':') + 1) +
                                                "\"" + DisconnectTimeFromCommandLine.Substring(
                    DisconnectTimeFromCommandLine.IndexOf(':') + 1) + "\"";

                TestRunner = new PetraClient_AutomatedAppTest.TAutomatedAppTest(
                    TAppSettingsManager.GetValue("AutoTestConfigFile"),
                    TAppSettingsManager.GetValue("AutoTestParameters"),
                    TVariant.DecodeFromString(DisconnectTimeFromCommandLine).ToDate(),
                    TConnectionManagementBase.GConnectionManagement.ClientName);

                TestRunner.TestForm = this;
                TestRunner.ClientID = TConnectionManagementBase.GConnectionManagement.ClientID;
                TestRunner.Start(this);
            }
            else if (System.Windows.Forms.Form.ModifierKeys != Keys.Control)
            {
                string testAction = TAppSettingsManager.GetValue("TestAction");

                if (testAction != TAppSettingsManager.UNDEFINEDVALUE)
                {
                    XmlDocument temp = new XmlDocument();
                    XmlNode testActionNode = temp.CreateElement("testAction");
                    temp.AppendChild(testActionNode);

                    testAction = testAction.Trim(new char[] { '"' });

                    while (testAction.Length > 0)
                    {
                        string[] pair = StringHelper.GetNextCSV(ref testAction, ",").Split(new char[] { '=' });
                        XmlAttribute attr = temp.CreateAttribute(pair[0]);
                        attr.Value = pair[1];
                        testActionNode.Attributes.Append(attr);
                    }

                    TLstTasks.ExecuteAction(testActionNode, null);
                }
            }
        }

        /// <summary>
        /// checks if the user has access to the navigation node
        /// </summary>
        public static bool HasAccessPermission(XmlNode ANode, string AUserId, bool ACheckLedgerPermissions)
        {
            // TODO: if this is an action node, eg. opens a screen, check the static function that tells RequiredPermissions of the screen

            string PermissionsRequired = TXMLParser.GetAttributeRecursive(ANode, "PermissionsRequired", true);

            while (PermissionsRequired.Length > 0)
            {
                string PermissionRequired = StringHelper.GetNextCSV(ref PermissionsRequired);

                if (!UserInfo.GUserInfo.IsInModule(PermissionRequired))
                {
                    return false;
                }
            }

            if (ACheckLedgerPermissions)
            {
                if (TXMLParser.GetAttributeRecursive(ANode, "DependsOnLedger", true).ToLower() == "true")
                {
                    // check if the user has permissions for this ledger
                    Int32 LedgerNumber = TXMLParser.GetIntAttribute(ANode, "LedgerNumber");

                    if (LedgerNumber != -1)
                    {
                        if (!UserInfo.GUserInfo.IsInModule(FormatLedgerNumberForModuleAccess(LedgerNumber)))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Recurse through the whole menu hierarachy and record all Singleton screens (=screens for which only one instance is to be opened).
        /// </summary>
        /// <param name="AChildNode">'MainMenu' node.</param>
        static void RecordAllSingletonScreens(XmlNode AChildNode)
        {
            XmlNode InspectNode = AChildNode.FirstChild;

            //Iterate through all children nodes of the node
            while (InspectNode != null)
            {
                CheckForAndRecordSingletonScreen(InspectNode);

                // Recurse into deeper levels!
                RecordAllSingletonScreens(InspectNode);

                InspectNode = InspectNode.NextSibling;
            }
        }

        /// <summary>
        /// Checks if a screen should be a Singleton screen (=screens for which only one instance is to be opened) and record the fact.
        /// </summary>
        /// <param name="childNode">Node to inspect.</param>
        static void CheckForAndRecordSingletonScreen(XmlNode childNode)
        {
            string ChildNodeActionOpenScreen = TXMLParser.GetAttribute(childNode, "ActionOpenScreen");

            if (ChildNodeActionOpenScreen.Length > 0)
            {
                if (TXMLParser.GetAttribute(childNode, "Singleton").ToLower() == "true")
                {
                    if (!TFormsList.GSingletonForms.Contains(ChildNodeActionOpenScreen))
                    {
                        TFormsList.GSingletonForms.Add(ChildNodeActionOpenScreen);
                    }
                }
            }
        }

        // displays information about the currently selected conference in the navigation panel
        private static void AddConferenceInformation(XmlNode AMenuNode)
        {
            FConferenceKey = TUserDefaults.GetInt64Default("LASTCONFERENCEWORKEDWITH");

            // Set PartnerKey in conference setup screens for selected conference
            Ict.Petra.Client.MConference.Gui.TConferenceMain.FPartnerKey = FConferenceKey;

            XmlNode childNode = AMenuNode.FirstChild;
            XmlAttribute enabledAttribute;

            while (childNode != null)
            {
                if ((TXMLParser.GetAttribute(childNode, "DependsOnConference").ToLower() == "true") && (FConferenceKey != 0))
                {
                    FConferenceSelected = true; // node only displayed if this is true

                    // Create 'Select Conference' Node
                    XmlAttribute LabelAttributeConference = childNode.OwnerDocument.CreateAttribute("Label");
                    XmlElement SelConferenceElmnt = childNode.OwnerDocument.CreateElement("ConferenceInfo");
                    XmlNode SelectConferenceNode = childNode.AppendChild(SelConferenceElmnt);
                    SelectConferenceNode.Attributes.Append(LabelAttributeConference);
                    SelectConferenceNode.Attributes["Label"].Value = Catalog.GetString("Current Conference");

                    // Create conference details Node
                    XmlElement SpecificConferenceElmnt = childNode.OwnerDocument.CreateElement("Conference" + FConferenceKey);
                    XmlNode SpecificConferenceNode = SelectConferenceNode.AppendChild(SpecificConferenceElmnt);
                    XmlAttribute AttributeConferenceName = childNode.OwnerDocument.CreateAttribute("Label");
                    SpecificConferenceNode.Attributes.Append(AttributeConferenceName);

                    // Disable clicking on node
                    enabledAttribute = childNode.OwnerDocument.CreateAttribute("Enabled");
                    enabledAttribute.Value = "false";
                    SpecificConferenceNode.Attributes.Append(enabledAttribute);

                    // Get conference name
                    string ConferenceName;
                    TPartnerClass PartnerClass;
                    TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(FConferenceKey, out ConferenceName, out PartnerClass);

                    if (ConferenceName != String.Empty)
                    {
                        const int BreakPoint = 28;
                        SpecificConferenceNode.Attributes["Label"].Value = "";

                        // splits the name over multiple lines if it too long
                        while (ConferenceName.Length > BreakPoint)
                        {
                            int IndexOfSpace = ConferenceName.IndexOf(" ", 0);
                            int LastIndexOfSpace = 0;

                            // searches for the last breakpoint for a line
                            while (IndexOfSpace <= BreakPoint && IndexOfSpace != -1)
                            {
                                LastIndexOfSpace = IndexOfSpace;
                                IndexOfSpace = ConferenceName.IndexOf(" ", LastIndexOfSpace + 1);
                            }

                            SpecificConferenceNode.Attributes["Label"].Value += ConferenceName.Substring(0, LastIndexOfSpace) + "\n";

                            ConferenceName = ConferenceName.Remove(0, LastIndexOfSpace + 1);
                        }

                        SpecificConferenceNode.Attributes["Label"].Value += ConferenceName + "\n";
                    }
                    else
                    {
                        SpecificConferenceNode.Attributes["Label"].Value = "Conference Key: " + FConferenceKey;
                    }

                    // Set node values
                    SpecificConferenceNode.Attributes["Label"].Value = SpecificConferenceNode.Attributes["Label"].Value;

                    // only dispay dates if they are valid
                    DateTime StartDate = TRemote.MConference.Conference.WebConnectors.GetStartDate(FConferenceKey);
                    DateTime EndDate = TRemote.MConference.Conference.WebConnectors.GetEndDate(FConferenceKey);

                    if (StartDate != DateTime.MinValue)
                    {
                        SpecificConferenceNode.Attributes["Label"].Value = SpecificConferenceNode.Attributes["Label"].Value +
                                                                           "\nStart: " + StartDate.ToLongDateString();
                    }

                    if (EndDate != DateTime.MinValue)
                    {
                        SpecificConferenceNode.Attributes["Label"].Value = SpecificConferenceNode.Attributes["Label"].Value +
                                                                           "\nEnd: " + EndDate.ToLongDateString();
                    }

                    childNode = childNode.NextSibling;
                }
                else
                {
                    // Recurse into deeper levels!
                    AddConferenceInformation(childNode);

                    childNode = childNode.NextSibling;
                }
            }
        }

        private static void AddNavigationForEachLedger(XmlNode AMenuNode, ALedgerTable AAvailableLedgers, bool ADontUseDefaultLedger)
        {
            XmlNode childNode = AMenuNode.FirstChild;
            int PotentialCurrentLedger;
            ALedgerRow ProcessedLedger;
            XmlAttribute enabledAttribute;
            bool LedgersAvailableToUserCreatedInThisIteration = false;

            //Iterate through all children nodes of the node
            while (childNode != null)
            {
                if (TXMLParser.GetAttribute(childNode, "DependsOnLedger").ToLower() == "true")
                {
                    // If there is more than one Ledger in the system, show a 'Select Ledger' Collapsible Panel with a Task (=LinkLabel)
                    // for each Ledger.
                    if (AAvailableLedgers.Rows.Count > 1)
                    {
                        LedgersAvailableToUserCreatedInThisIteration = false;
                        AAvailableLedgers.DefaultView.Sort = ALedgerTable.GetLedgerNumberDBName() + " ASC";

                        FMultiLedgerSite = true;

                        // Create 'Select Ledger' Node
                        XmlAttribute LabelAttributeLedger = childNode.OwnerDocument.CreateAttribute("Label");
                        XmlElement SelLedgerElmnt = childNode.OwnerDocument.CreateElement("SelectLedger");
                        XmlNode SelectLedgerNode = childNode.AppendChild(SelLedgerElmnt);
                        SelectLedgerNode.Attributes.Append(LabelAttributeLedger);
                        SelectLedgerNode.Attributes["Label"].Value = Catalog.GetString("Select Ledger");

                        // Create 1..n 'Ledger xyz' Nodes
                        foreach (DataRowView Drv in AAvailableLedgers.DefaultView)
                        {
                            ProcessedLedger = (ALedgerRow)Drv.Row;

                            XmlElement SpecificLedgerElmnt = childNode.OwnerDocument.CreateElement("Ledger" + ProcessedLedger.LedgerNumber);
                            XmlNode SpecificLedgerNode = SelectLedgerNode.AppendChild(SpecificLedgerElmnt);
                            XmlAttribute LabelAttributeSpecificLedger = childNode.OwnerDocument.CreateAttribute("Label");
                            SpecificLedgerNode.Attributes.Append(LabelAttributeSpecificLedger);
                            XmlAttribute ledgerNumberAttribute = childNode.OwnerDocument.CreateAttribute("LedgerNumber");
                            ledgerNumberAttribute.Value = ProcessedLedger.LedgerNumber.ToString();
                            SpecificLedgerNode.Attributes.Append(ledgerNumberAttribute);
                            XmlAttribute ledgerNameAttribute = childNode.OwnerDocument.CreateAttribute("LedgerName");
                            ledgerNameAttribute.Value = ProcessedLedger.LedgerName;
                            SpecificLedgerNode.Attributes.Append(ledgerNameAttribute);

                            if (ProcessedLedger.LedgerName != String.Empty)
                            {
                                SpecificLedgerNode.Attributes["Label"].Value = String.Format(Catalog.GetString(
                                        "Ledger {0} (#{1})"), ProcessedLedger.LedgerName, ProcessedLedger.LedgerNumber);
                            }
                            else
                            {
                                SpecificLedgerNode.Attributes["Label"].Value = String.Format(Catalog.GetString(
                                        "Ledger #{0}"), ProcessedLedger.LedgerNumber);
                            }

                            // Check access permission for Ledger
                            if (!HasAccessPermission(SpecificLedgerNode, UserInfo.GUserInfo.UserID, true))
                            {
                                enabledAttribute = childNode.OwnerDocument.CreateAttribute("Enabled");
                                enabledAttribute.Value = "false";
                                SpecificLedgerNode.Attributes.Append(enabledAttribute);
                            }
                            else
                            {
                                if (FLedgersAvailableToUser == null)
                                {
                                    // (Re-)Calculate which Ledgers the user has access to
                                    FLedgersAvailableToUser = new List <string>();
                                    LedgersAvailableToUserCreatedInThisIteration = true;
                                }

                                if (LedgersAvailableToUserCreatedInThisIteration)
                                {
                                    // Add Ledger to the List of Ledgers that are available to the user
                                    if (!FLedgersAvailableToUser.Contains(FormatLedgerNumberForModuleAccess(ProcessedLedger.LedgerNumber)))
                                    {
                                        FLedgersAvailableToUser.Add(FormatLedgerNumberForModuleAccess(ProcessedLedger.LedgerNumber));
                                    }
                                }
                            }
                        }

                        if ((LedgersAvailableToUserCreatedInThisIteration)
                            || (FLedgersAvailableToUser == null))
                        {
                            if (!ADontUseDefaultLedger)
                            {
                                // Set the 'Current Ledger' to the users' Default Ledger, or if he/she hasn't got one, to the first Ledger of the Site.
                                PotentialCurrentLedger = TUserDefaults.GetInt32Default(TUserDefaults.FINANCE_DEFAULT_LEDGERNUMBER,
                                    ((ALedgerRow)AAvailableLedgers.DefaultView[0].Row).LedgerNumber);

                                if ((FLedgersAvailableToUser != null)
                                    && (FLedgersAvailableToUser.Contains(FormatLedgerNumberForModuleAccess(PotentialCurrentLedger))))
                                {
                                    FCurrentLedger = PotentialCurrentLedger;
                                }
                                else
                                {
                                    if (FLedgersAvailableToUser != null)
                                    {
                                        FCurrentLedger = Convert.ToInt32(FLedgersAvailableToUser[0].Substring(6));    // Skip "LEDGER"
                                    }
                                    else   // = no Ledgers available to the user at all!
                                    {
                                        FCurrentLedger = TLstFolderNavigation.LEDGERNUMBER_NO_ACCESS_TO_ANY_LEDGER;
                                    }
                                }
                            }
                        }
                    }
                    else if (AAvailableLedgers.Rows.Count == 1)
                    {
                        // Dynamically add Attribute 'SkipThisLevel' to the next child, which would be the child for the Collapsible Panel,
                        // which we don't need/want for a 'Single Ledger' Site!
                        XmlAttribute LabelSkipCollapsibleLevel = childNode.OwnerDocument.CreateAttribute("SkipThisLevel");
                        childNode.ChildNodes[0].Attributes.Append(LabelSkipCollapsibleLevel);
                        childNode.ChildNodes[0].Attributes["SkipThisLevel"].Value = "true";

                        // Check access permission for Ledger
                        if (UserInfo.GUserInfo.IsInModule(FormatLedgerNumberForModuleAccess(AAvailableLedgers[0].LedgerNumber)))
                        {
                            // Set the 'Current Ledger' to the only Ledger of the Site.
                            FCurrentLedger = AAvailableLedgers[0].LedgerNumber;
                        }
                        else   // = no Ledgers available to the user at all!
                        {
                            FCurrentLedger = TLstFolderNavigation.LEDGERNUMBER_NO_ACCESS_TO_ANY_LEDGER;
                        }
                    }
                    else   // = no Ledgers available to the user at all!
                    {
                        FCurrentLedger = TLstFolderNavigation.LEDGERNUMBER_NO_ACCESS_TO_ANY_LEDGER;
                    }

                    childNode = childNode.NextSibling;
                }
                else
                {
                    // Recurse into deeper levels!
                    AddNavigationForEachLedger(childNode, AAvailableLedgers, ADontUseDefaultLedger);

                    childNode = childNode.NextSibling;
                }
            }
        }

        /// <summary>
        /// build an XML document which includes all ledgers etc.
        /// </summary>
        public static XmlNode BuildNavigationXml(bool ADontUseDefaultLedger = false)
        {
            TYml2Xml parser = new TYml2Xml(TAppSettingsManager.GetValue("UINavigation.File"));
            XmlDocument UINavigation = parser.ParseYML2XML();

            ALedgerTable AvailableLedgers = new ALedgerTable();

            if (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE1))
            {
                AvailableLedgers = TRemote.MFinance.Setup.WebConnectors.GetAvailableLedgers();
            }

            XmlNode OpenPetraNode = UINavigation.FirstChild.NextSibling.FirstChild;
            XmlNode SearchBoxesNode = OpenPetraNode.FirstChild;
            XmlNode MainMenuNode = SearchBoxesNode.NextSibling;

            if (TFormsList.GSingletonForms.Count == 0)      // There is no need to re-record all Singleton screens if this was already done once
            {
                RecordAllSingletonScreens(MainMenuNode);
            }

            AddNavigationForEachLedger(MainMenuNode, AvailableLedgers, ADontUseDefaultLedger);

            if (UserInfo.GUserInfo.IsInModule("PTNRUSER") && UserInfo.GUserInfo.IsInModule("CONFERENCE"))
            {
                AddConferenceInformation(MainMenuNode);
            }

            return MainMenuNode;
        }

        /// <summary>
        /// load or reload the navigation
        /// </summary>
        public void LoadNavigationUI(bool ADontUseDefaultLedger = false)
        {
            // Force re-calculation of available Ledgers and correct setting of FCurrentLedger
            FLedgersAvailableToUser = null;

            XmlNode MainMenuNode = BuildNavigationXml(ADontUseDefaultLedger);
            XmlNode DepartmentNode = MainMenuNode.FirstChild;

            lstFolders.MultiLedgerSite = FMultiLedgerSite;
            lstFolders.CurrentLedger = FCurrentLedger;
            lstFolders.ConferenceSelected = FConferenceSelected;

            lstFolders.ClearFolders();

            lstFolders.SubmoduleChanged += delegate(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked, object AOtherData)
            {
                OnSubmoduleChanged(ATaskList, ATaskListNode, AItemClicked);
            };
            lstFolders.LedgerChanged += delegate(int ALedgerNr, string ALedgerName)
            {
                OnLedgerChanged(ALedgerNr, ALedgerName);
            };

            TPnlModuleNavigation.SubSystemLinkStatus += delegate(int ALedgerNr, TPnlCollapsible APnlCollapsible)
            {
                UpdateSubsystemLinkStatus(ALedgerNr, APnlCollapsible);
            };

            TFrmGLEnableSubsystems.FinanceSubSystemLinkStatus += delegate()
            {
                UpdateFinanceSubsystemLinkStatus();
            };

            TLstTasks.Init(UserInfo.GUserInfo.UserID, HasAccessPermission);

            while (DepartmentNode != null)
            {
                lstFolders.AddFolder(DepartmentNode, UserInfo.GUserInfo.UserID, HasAccessPermission);

                DepartmentNode = DepartmentNode.NextSibling;
            }

            lstFolders.Dashboard = this.dsbContent;
            lstFolders.Statusbar = this.stbMain;

            SetTaskTileSize(TUserDefaults.GetInt16Default(TUserDefaults.MAINMENU_VIEWOPTIONS_TILESIZE, 2));

            SetTasksSingleClickExecution(TUserDefaults.GetBooleanDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_SINGLECLICKEXECUTION, false));

            if (TUserDefaults.GetStringDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_VIEWTASKS, VIEWTASKS_TILES) == VIEWTASKS_TILES)
            {
                ViewTasksAsTiles(this, null);
            }
            else
            {
                ViewTasksAsList(this, null);
            }

            lstFolders.SelectFirstAvailableFolder();
        }

        /// <summary>
        /// This was added for use after "DeleteLedger".
        /// IT ONLY WORKS IF THE USER HAS FINANCE ACCESS!
        /// </summary>
        public void SelectFinanceFolder()
        {
            lstFolders.SelectFolder(1);
        }

        /// <summary>
        /// This was added for use after a new conference has been selected.
        /// IT ONLY WORKS IF THE USER HAS CONFERENCE ACCESS!
        /// </summary>
        public void SelectConferenceFolder()
        {
            lstFolders.SelectFolder(4);
        }

        /// <summary>
        /// This was added for use after user preferences have been set.
        /// </summary>
        public void SelectSettingsFolder()
        {
            lstFolders.SelectFolder(6);
        }

        /// <summary>
        /// Shows the current Ledger in the StatusBar.
        /// </summary>
        public void ShowCurrentLedgerInfoInStatusBar()
        {
            if (FCurrentLedger != TLstFolderNavigation.LEDGERNUMBER_NO_ACCESS_TO_ANY_LEDGER)
            {
                this.stbMain.ShowMessage("Current Ledger is Ledger " + FCurrentLedger.ToString());
            }
            else
            {
                this.stbMain.ShowMessage("There are now NO LEDGERS that you have access to!");
            }
        }

        private void ExitManualCode()
        {
            // make sure the application exits; also important for alternate navigation style main window
            this.Hide();

            PetraClientShutdown.Shutdown.SaveUserDefaultsAndDisconnect();

            PetraClientShutdown.Shutdown.StopPetraClient();
        }

        /// the main navigation form
        public static Form MainForm = null;

        private void ViewTasksAsTiles(object sender, EventArgs e)
        {
            dsbContent.TaskAppearance = TaskAppearance.staLargeTile;

            mniViewTasksTiles.Checked = true;
            mniViewTasksList.Checked = false;

            TUserDefaults.SetDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_VIEWTASKS, VIEWTASKS_TILES);
        }

        private void ViewTasksAsList(object sender, EventArgs e)
        {
            dsbContent.TaskAppearance = TaskAppearance.staListEntry;

            mniViewTasksList.Checked = true;
            mniViewTasksTiles.Checked = false;

            TUserDefaults.SetDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_VIEWTASKS, VIEWTASKS_LIST);
        }

        private void ViewTaskSizeChange(object sender, EventArgs e)
        {
            if (sender == mniViewTaskSizeLarge)
            {
                SetTaskTileSize(1);
            }
            else if (sender == mniViewTaskSizeMedium)
            {
                SetTaskTileSize(2);
            }
            else
            {
                SetTaskTileSize(3);
            }
        }

        private void ViewTasksSingleClickExecution(object sender, EventArgs e)
        {
            SetTasksSingleClickExecution(!mniViewTasksSingleClickExecution.Checked);
        }

        private void SetTasksSingleClickExecution(bool ASingleClick)
        {
            dsbContent.SingleClickExecution = ASingleClick;

            mniViewTasksSingleClickExecution.Checked = ASingleClick;

            TUserDefaults.SetDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_SINGLECLICKEXECUTION, ASingleClick);
        }

        private void SetTaskTileSize(int ATaskTileSize)
        {
            if (ATaskTileSize == 1)
            {
                dsbContent.MaxTaskWidth = 340;

                mniViewTaskSizeLarge.Checked = true;
                mniViewTaskSizeMedium.Checked = false;
                mniViewTaskSizeSmall.Checked = false;
            }
            else if (ATaskTileSize == 2)
            {
                dsbContent.MaxTaskWidth = 280;

                mniViewTaskSizeMedium.Checked = true;
                mniViewTaskSizeLarge.Checked = false;
                mniViewTaskSizeSmall.Checked = false;
            }
            else
            {
                dsbContent.MaxTaskWidth = 210;

                mniViewTaskSizeSmall.Checked = true;
                mniViewTaskSizeLarge.Checked = false;
                mniViewTaskSizeMedium.Checked = false;
            }

            TUserDefaults.SetDefault(TUserDefaults.MAINMENU_VIEWOPTIONS_TILESIZE, ATaskTileSize);
        }

        private bool CanCloseManual()
        {
            Boolean ReturnValue;
            const String INDENTATION = "   ";
            StringCollection NonCloseableForms;
            Boolean CanCloseAllForms;
            String NonCloseableFormsList = "";
            String FirstNonCloseableFormKey;
            String ChangeInfo = "";
            Int16 FormsCounter;

            ReturnValue = false;

            //
            // Check if any Forms are open that cannot be closed and process those
            //
            CanCloseAllForms = TFormsList.GFormsList.CanCloseAll(out NonCloseableForms, out FirstNonCloseableFormKey);

            if (!CanCloseAllForms)
            {
                for (FormsCounter = 0; FormsCounter <= NonCloseableForms.Count - 1; FormsCounter += 1)
                {
                    NonCloseableFormsList = NonCloseableFormsList + INDENTATION + NonCloseableForms[FormsCounter] + "\r\n";
                }

                // Remove trailing Line Feed + Carriage Return
                NonCloseableFormsList = NonCloseableFormsList.Substring(0, NonCloseableFormsList.Length - "\r\n".Length);
            }

            if (CanCloseAllForms)
            {
                //
                // Any remaining Forms can be closed -> close all Forms, except for this Form.
                //
                TFormsList.GFormsList.CloseAllExceptOne(this);

                ReturnValue = true;
            }
            else
            {
                //
                // One or more remaining Forms cannot be closed.
                //

                // Check if any of the Forms in the list has the change indicator
                if (NonCloseableFormsList.IndexOf(PetraEditForm.FORM_CHANGEDDATAINDICATOR) > 0)
                {
                    // Include info about the change indicator in the message
                    ChangeInfo = "\r\n\r\n" + StrCannotClosePetraChangeInfoLine;
                }

                // Present list of Forms that still need to be closed to the user
                MessageBox.Show(
                    StrCannotClosePetra1stLine + "\r\n" + "\r\n" + StrCannotClosePetra2ndLine + "\r\n" + NonCloseableFormsList + ChangeInfo,
                    StrCannotClosePetraTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            //
            // Bring first Form that needs closing to the foreground
            //
            if (!CanCloseAllForms)
            {
                TFormsList.GFormsList.ShowForm(FirstNonCloseableFormKey);
            }

            return ReturnValue;
        }

        private void HelpImproveTranslations(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://sourceforge.net/apps/mediawiki/openpetraorg/index.php?title=Documentation_for_Translators");
        }

        private static string FormatLedgerNumberForModuleAccess(int ALedgerNumber)
        {
            return "LEDGER" + ALedgerNumber.ToString("0000");
        }

        private void InitialiseTopPanel()
        {
            TVisualStyles VisualStyle = new TVisualStyles(TVisualStylesEnum.vsHorizontalCollapse);

            TPnlGradient TopPanel = new TPnlGradient();

            TopPanel.Name = "Top";
            TopPanel.Dock = DockStyle.Fill;
            TopPanel.Padding = new Padding(0, 1, 0, 0);
            TopPanel.GradientColorTop = VisualStyle.TitleGradientStart;
            TopPanel.GradientColorBottom = VisualStyle.TitleGradientEnd;
            TopPanel.DontDrawBottomLine = false;

            pnlTop.Controls.Add(TopPanel);

            // Add Breadcrumb Trail Panel to TopPanel
            FBreadcrumbTrail = new TBreadcrumbTrail(TopPanel);

            // in the future: add SearchBox (still to be created) to TopPanel, too...
        }

        private void OnSubmoduleChanged(TTaskList ATaskList, XmlNode ATaskListNode, LinkLabel AItemClicked)
        {
            const string DetailTextPrefix = "» ";
            string ModuleText = String.Empty;
            string BreadcrumbDetailText = String.Empty;

            if ((ATaskListNode.ParentNode.Attributes["SkipThisLevel"] != null)
                && (ATaskListNode.ParentNode.Attributes["SkipThisLevel"].Value == "true"))
            {
                ModuleText = TLstFolderNavigation.GetLabel(ATaskListNode.ParentNode.ParentNode);
                BreadcrumbDetailText = DetailTextPrefix + AItemClicked.Text;
            }
            else
            {
                if ((ATaskListNode.ParentNode.Attributes["DontShowNestedTasksAsLinks"] == null)
                    || (ATaskListNode.ParentNode.Attributes["DontShowNestedTasksAsLinks"].Value == "false"))
                {
                    if ((ATaskListNode.ParentNode.ParentNode.Attributes["DependsOnLedger"] == null)
                        || (ATaskListNode.ParentNode.ParentNode.Attributes["DependsOnLedger"].Value == "false"))
                    {
                        ModuleText = TLstFolderNavigation.GetLabel(ATaskListNode.ParentNode.ParentNode);
                    }
                    else
                    {
                        ModuleText = Catalog.GetString("Ledger" + " " + FCurrentLedger.ToString());
                    }

                    BreadcrumbDetailText = DetailTextPrefix + TLstFolderNavigation.GetLabel(ATaskListNode.ParentNode) + " ";
                }

                BreadcrumbDetailText += DetailTextPrefix + AItemClicked.Text;
            }

            FBreadcrumbTrail.ModuleText = ModuleText;
            FBreadcrumbTrail.DetailText = BreadcrumbDetailText;

            // this is needed when there is just one ledger existing as item structure will be different
            UpdateSubsystemLinkStatus(FCurrentLedger, ATaskList, ATaskListNode);
        }

        private void OnLedgerChanged(int ALedgerNr, string ALedgerName)
        {
            FBreadcrumbTrail.ModuleText = Catalog.GetString("Ledger" + " " + ALedgerNr.ToString());
            FCurrentLedger = ALedgerNr;

            // Remove any message that is shown in the Status Bar (e.g. the one that is put there when creating a new Ledger)
            this.stbMain.ShowMessage(String.Empty);
        }

        /// <summary>
        /// Returns the Partner Key for the selected conference
        /// </summary>
        public Int64 GetSelectedConferenceKey()
        {
            return FConferenceKey;
        }

        private void UpdateFinanceSubsystemLinkStatus()
        {
            // Only necessary to do something here if Finance Module is currently selected
            // as otherwise handling will be automatically done via link selection change events
            lstFolders.FireSelectedLinkEventIfFolderSelected("Finance");
        }

        private void UpdateSubsystemLinkStatus(int ALedgerNr, TTaskList ATaskList, XmlNode ATaskListNode)
        {
            if (ATaskListNode.ParentNode != null
                && ATaskListNode.ParentNode.Name == "Finance")
            {
                XmlNode TempNode = ATaskListNode.ParentNode.FirstChild;

                while (TempNode != null)
                {
                    if (TempNode.Name == "GiftProcessing")
                    {
                        if (TRemote.MFinance.Setup.WebConnectors.IsGiftProcessingSubsystemActivated(ALedgerNr))
                        {
                            ATaskList.EnableTaskItem(TempNode);
                        }
                        else
                        {
                            ATaskList.DisableTaskItem(TempNode);
                        }
                    }
                    else if (TempNode.Name == "AccountsPayable")
                    {
                        if (TRemote.MFinance.Setup.WebConnectors.IsAccountsPayableSubsystemActivated(ALedgerNr))
                        {
                            ATaskList.EnableTaskItem(TempNode);
                        }
                        else
                        {
                            ATaskList.DisableTaskItem(TempNode);
                        }
                    }

                    TempNode = TempNode.NextSibling;
                }
            }

        }

        private void UpdateSubsystemLinkStatus(int ALedgerNr, TPnlCollapsible APnlCollapsible)
        {
            if (APnlCollapsible == null)
            {
                return;
            }

            UpdateSubsystemLinkStatus(ALedgerNr, APnlCollapsible.TaskListInstance, APnlCollapsible.TaskListNode.FirstChild);
        }
    }
}