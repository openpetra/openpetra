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
using Ict.Common.Remoting.Shared;

namespace Ict.Petra.Client.App.PetraClient
{
    public partial class TFrmMainWindowNew
    {
        private const string VIEWTASKS_TILES = "Tiles";
        private const string VIEWTASKS_LIST = "List";

        private static bool FMultiLedgerSite = false;
        private static int FCurrentLedger = -1;
        

        /// <summary>
        /// The currently selected Ledger
        /// </summary>        
        public int CurrentLedger
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
        
        private void InitializeManualCode()
        {
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
        /// For development and testing purposes this Method can open a screen with parameters that
        /// come either from the .config file or Command Line.
        /// The 'Test Action' will not be run if the Control Key is pressed.
        /// </summary>
        /// <remarks>
        /// sample action: TestAction="Namespace=Ict.Petra.Client.MPartner.Gui,ActionOpenScreen=TFrmPartnerEdit2,PartnerKey=0043005002,InitiallySelectedTabPage=petpDetails"
        ///</remarks>
        private void RunTestAction()
        {
            if (System.Windows.Forms.Form.ModifierKeys != Keys.Control)
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
        public static bool HasAccessPermission(XmlNode ANode, string AUserId)
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

            if (TXMLParser.GetAttributeRecursive(ANode, "DependsOnLedger", true).ToLower() == "true")
            {
                // check if the user has permissions for this ledger
                Int32 LedgerNumber = TXMLParser.GetIntAttribute(ANode, "LedgerNumber");

                if (LedgerNumber != -1) 
                {
                    if (!UserInfo.GUserInfo.IsInModule("LEDGER" + LedgerNumber.ToString("0000")))
                    {
                        return false;
                    }                    
                }
            }

            return true;
        }

        private static void AddNavigationForEachLedger(XmlNode AMenuNode, ALedgerTable AAvailableLedgers, bool ADontUseDefaultLedger)
        {
            XmlNode childNode = AMenuNode.FirstChild;

            while (childNode != null)
            {
                if (TXMLParser.GetAttribute(childNode, "DependsOnLedger").ToLower() == "true")
                {
                    // If there is more than one Ledger in the system, show a 'Select Legdger' Collapsible Panel with a Task (=LinkLabel) 
                    // for each Ledger.
                    if (AAvailableLedgers.Rows.Count > 1)
                    {
                        FMultiLedgerSite = true;
                        
                        string label = TXMLParser.GetAttribute(childNode, "Label");
    
                        // Create 'Select Legdger' Node
                        XmlAttribute LabelAttributeLedger = childNode.OwnerDocument.CreateAttribute("Label");
                        XmlElement SelLedgerElmnt = childNode.OwnerDocument.CreateElement("SelectLedger");
                        XmlNode SelectLedgerNode = childNode.AppendChild(SelLedgerElmnt);
                        SelectLedgerNode.Attributes.Append(LabelAttributeLedger);
                        SelectLedgerNode.Attributes["Label"].Value = Catalog.GetString("Select Ledger");  
    
                        // Create 1..n 'Ledger xyz' Nodes
                        foreach (ALedgerRow ledger in AAvailableLedgers.Rows)
                        {
                            XmlElement SpecificLedgerElmnt = childNode.OwnerDocument.CreateElement("Ledger" + ledger.LedgerNumber);
                            XmlNode SpecificLedgerNode = SelectLedgerNode.AppendChild(SpecificLedgerElmnt);
                            XmlAttribute LabelAttributeSpecificLedger = childNode.OwnerDocument.CreateAttribute("Label");
                            SpecificLedgerNode.Attributes.Append(LabelAttributeSpecificLedger);
                            XmlAttribute ledgerNumberAttribute = childNode.OwnerDocument.CreateAttribute("LedgerNumber");
                            ledgerNumberAttribute.Value = ledger.LedgerNumber.ToString();
                            SpecificLedgerNode.Attributes.Append(ledgerNumberAttribute);
                            XmlAttribute ledgerNameAttribute = childNode.OwnerDocument.CreateAttribute("LedgerName");
                            ledgerNameAttribute.Value = ledger.LedgerName;
                            SpecificLedgerNode.Attributes.Append(ledgerNameAttribute);
                                                        
                            if (ledger.LedgerName != String.Empty) 
                            {
                                SpecificLedgerNode.Attributes["Label"].Value = String.Format(Catalog.GetString("Ledger {0} (#{1})"), ledger.LedgerName, ledger.LedgerNumber);    
                            }
                            else
                            {
                                SpecificLedgerNode.Attributes["Label"].Value = String.Format(Catalog.GetString("Ledger #{0}"), ledger.LedgerNumber);
                            }                            

                            if (!HasAccessPermission(SpecificLedgerNode, UserInfo.GUserInfo.UserID))
                            {
                                XmlAttribute enabledAttribute = childNode.OwnerDocument.CreateAttribute("Enabled");
                                enabledAttribute.Value = "false";
                                SpecificLedgerNode.Attributes.Append(enabledAttribute);                                
                            }                                
                        }
                        
                        if (!ADontUseDefaultLedger) 
                        {
                            // Set the 'Current Ledger' to the users' Default Ledger, or if he/she hasn't got one, to the first Ledger of the Site.
                            FCurrentLedger = TUserDefaults.GetInt32Default(TUserDefaults.FINANCE_DEFAULT_LEDGERNUMBER, AAvailableLedgers[0].LedgerNumber);                            
                        }
                    }
                    else if (AAvailableLedgers.Rows.Count == 1)
                    {
                        // Dynamically add Attribute 'SkipThisLevel' to the next child, which would be the child for the Collapsible Panel, 
                        // which we don't need/want for a 'Single Ledger' Site!                        
                        XmlAttribute LabelSkipCollapsibleLevel = childNode.OwnerDocument.CreateAttribute("SkipThisLevel");
                        childNode.ChildNodes[0].Attributes.Append(LabelSkipCollapsibleLevel);
                        childNode.ChildNodes[0].Attributes["SkipThisLevel"].Value = "true";
                        
                        // Set the 'Current Ledger' to the only Ledger of the Site.
                        FCurrentLedger = AAvailableLedgers[0].LedgerNumber;
                    }
                        
                    childNode = childNode.NextSibling;
                }
                else
                {
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

            AddNavigationForEachLedger(MainMenuNode, AvailableLedgers, ADontUseDefaultLedger);

            return MainMenuNode;
        }

        /// <summary>
        /// load or reload the navigation
        /// </summary>
        public void LoadNavigationUI(bool ADontUseDefaultLedger = false)
        {
            XmlNode MainMenuNode = BuildNavigationXml(ADontUseDefaultLedger);
            XmlNode DepartmentNode = MainMenuNode.FirstChild;

            lstFolders.MultiLedgerSite = FMultiLedgerSite;
            lstFolders.CurrentLedger = FCurrentLedger;
            
            lstFolders.ClearFolders();

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
            StringCollection NonClosableForms;
            string FirstNonClosableFormKey;

            return TFormsList.GFormsList.CanCloseAll(out NonClosableForms, out FirstNonClosableFormKey);
        }

        private void HelpImproveTranslations(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://sourceforge.net/apps/mediawiki/openpetraorg/index.php?title=Documentation_for_Translators");
        }
    }
}