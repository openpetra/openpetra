//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using System.Collections.Specialized;
using System.Xml;
using System.Windows.Forms;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data; // Implicit reference
using Ict.Common.IO;
using Ict.Common.Controls;
using Ict.Petra.Shared;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MReporting;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.App.PetraClient
{
    public partial class TFrmMainWindowNew
    {
        private void InitializeManualCode()
        {
            LoadNavigationUI();

            // leave out 'Revision' and 'Build'
            this.Text = "OpenPetra.org " +
                        (new Version(TClientInfo.ClientAssemblyVersion)).ToString(3);
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
                string testAction = TAppSettingsManager.GetValueStatic("TestAction");

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

                    TLstTasks.ExecuteAction(testActionNode, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// checks if the user has access to the navigation node
        /// </summary>
        private bool HasAccessPermission(XmlNode ANode, string AUserId)
        {
            // TODO: if this node belongs to a ledger, check if the user has access permission
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

            return true;
        }

        private void AddNavigationForEachLedger(XmlNode AMenuNode, ALedgerTable AAvailableLedgers)
        {
            XmlNode childNode = AMenuNode.FirstChild;

            while (childNode != null)
            {
                if (TXMLParser.GetAttribute(childNode, "DependsOnLedger").ToLower() == "true")
                {
                    string label = TXMLParser.GetAttribute(childNode, "Label");

                    foreach (ALedgerRow ledger in AAvailableLedgers.Rows)
                    {
                        XmlNode NewNode = childNode.Clone();
                        childNode.ParentNode.InsertBefore(NewNode, childNode);
                        XmlAttribute ledgerNumberAttribute = childNode.OwnerDocument.CreateAttribute("LedgerNumber");
                        ledgerNumberAttribute.Value = ledger.LedgerNumber.ToString();
                        NewNode.Attributes.Append(ledgerNumberAttribute);

                        if (AAvailableLedgers.Rows.Count > 1)
                        {
                            NewNode.Attributes["Label"].Value = String.Format(Catalog.GetString(label), ledger.LedgerName, ledger.LedgerNumber);
                        }
                        else
                        {
                            NewNode.Attributes["Label"].Value = String.Format(Catalog.GetString(label), "", "");
                        }
                    }

                    // remove the node that has the place holder for the ledger
                    XmlNode toRemove = childNode;
                    childNode = childNode.NextSibling;
                    AMenuNode.RemoveChild(toRemove);
                }
                else
                {
                    AddNavigationForEachLedger(childNode, AAvailableLedgers);
                    childNode = childNode.NextSibling;
                }
            }
        }

        private void LoadNavigationUI()
        {
            TAppSettingsManager opts = new TAppSettingsManager();
            TYml2Xml parser = new TYml2Xml(opts.GetValue("UINavigation.File"));
            XmlDocument UINavigation = parser.ParseYML2XML();

            ALedgerTable AvailableLedgers = new ALedgerTable();

            if ((UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE1))
                || (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE2))
                || (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE3)))
            {
                AvailableLedgers = TRemote.MFinance.Setup.WebConnectors.GetAvailableLedgers();
            }

            XmlNode OpenPetraNode = UINavigation.FirstChild.NextSibling.FirstChild;
            XmlNode SearchBoxesNode = OpenPetraNode.FirstChild;
            XmlNode MainMenuNode = SearchBoxesNode.NextSibling;
            XmlNode DepartmentNode = MainMenuNode.FirstChild;

            AddNavigationForEachLedger(MainMenuNode, AvailableLedgers);

            while (DepartmentNode != null)
            {
                lstFolders.AddFolder(DepartmentNode, UserInfo.GUserInfo.UserID, HasAccessPermission);

                DepartmentNode = DepartmentNode.NextSibling;
            }

            lstFolders.Dashboard = this.dsbContent;
            lstFolders.Statusbar = this.stbMain;
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

        private void SwitchToClassicNavigation(object sender, EventArgs e)
        {
            MainForm = this;

            if (TFrmMainWindow.MainForm == null)
            {
                TFrmMainWindow.MainForm = new TFrmMainWindow(IntPtr.Zero);
            }

            this.Hide();
            TFrmMainWindow.MainForm.Show();
        }

        private bool CanCloseManual()
        {
            StringCollection NonClosableForms;
            string FirstNonClosableFormKey;

            return TFormsList.GFormsList.CanCloseAll(out NonClosableForms, out FirstNonClosableFormKey);
        }
    }
}