//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2019 by OM International
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using System.IO;
using System.Text;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MSysMan.Common.WebConnectors;
using Ict.Petra.Server.MFinance.Setup.WebConnectors;

namespace Ict.Petra.Server.app.JSClient
{
    /// <summary>
    /// Load navigation from UINavigation.yml and return as json
    /// specific for this user, disabling parts that he does not have access to
    /// </summary>
    public class TUINavigation
    {
        private static bool FMultiLedgerSite = false;
        private static List <string>FLedgersAvailableToUser = null;
        private static int FCurrentLedger = -1;
        private const int LEDGERNUMBER_NO_ACCESS_TO_ANY_LEDGER = -2;

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
                    if (false && (AAvailableLedgers.Rows.Count > 1))
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
                                        FCurrentLedger = LEDGERNUMBER_NO_ACCESS_TO_ANY_LEDGER;
                                    }
                                }
                            }
                        }
                    }
                    else if (AAvailableLedgers.Rows.Count == 1)
                    {
                        // Check access permission for Ledger
                        if (UserInfo.GUserInfo.IsInModule(FormatLedgerNumberForModuleAccess(AAvailableLedgers[0].LedgerNumber)))
                        {
                            // Set the 'Current Ledger' to the only Ledger of the Site.
                            FCurrentLedger = AAvailableLedgers[0].LedgerNumber;
                        }
                        else   // = no Ledgers available to the user at all!
                        {
                            FCurrentLedger = LEDGERNUMBER_NO_ACCESS_TO_ANY_LEDGER;
                        }
                    }
                    else   // = no Ledgers available to the user at all!
                    {
                        FCurrentLedger = LEDGERNUMBER_NO_ACCESS_TO_ANY_LEDGER;
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
            string UINavigationFile = TAppSettingsManager.GetValue("UINavigation.File");

            if (!File.Exists(UINavigationFile))
            {
                throw new Exception ("cannot find file " + UINavigationFile);
            }

            TYml2Xml parser = new TYml2Xml(UINavigationFile);
            XmlDocument UINavigation = parser.ParseYML2XML();

            ALedgerTable AvailableLedgers = new ALedgerTable();

            if (UserInfo.GUserInfo.IsInModule(SharedConstants.PETRAMODULE_FINANCE1))
            {
                AvailableLedgers = TGLSetupWebConnector.GetAvailableLedgers();
            }

            XmlNode OpenPetraNode = UINavigation.FirstChild.NextSibling.FirstChild;
            XmlNode MainMenuNode = OpenPetraNode.FirstChild;

            AddNavigationForEachLedger(MainMenuNode, AvailableLedgers, ADontUseDefaultLedger);

            return MainMenuNode;
        }

        /// <summary>
        /// load the navigation ready to be converted to JSON
        /// </summary>
        public static Dictionary<string, object> LoadNavigationUI(bool ADontUseDefaultLedger = false)
        {
            // Force re-calculation of available Ledgers and correct setting of FCurrentLedger
            FLedgersAvailableToUser = null;

            XmlNode MainMenuNode = BuildNavigationXml(ADontUseDefaultLedger);
            XmlNode DepartmentNode = MainMenuNode.FirstChild;

            Dictionary<string, object> result = new Dictionary<string, object>();

            while (DepartmentNode != null)
            {
                result.Add(DepartmentNode.Name, AddFolder(DepartmentNode, UserInfo.GUserInfo.UserID));

                DepartmentNode = DepartmentNode.NextSibling;
            }

            return result;
        }

        private static string GetCaption(XmlNode ANode, bool AWithoutBrackets=false)
        {
            if (AWithoutBrackets)
            {
                return ANode.Name + "_label";
            }
            else
            {
                return "{" + ANode.Name + "_label}";
            }
        }

        private static Dictionary<string, object> AddFolder(XmlNode AFolderNode, string AUserId)
        {
            // TODO icon?

            // enabled/disabled based on permissions
            bool enabled = true;	
            if ((TYml2Xml.HasAttribute(AFolderNode, "Enabled"))
                && (TYml2Xml.GetAttribute(AFolderNode, "Enabled").ToLower() == "false"))
            {
                enabled = false;
            }
            else
            {
                enabled = HasAccessPermission(AFolderNode, AUserId, false);
            }

            Dictionary<string, object> folder = new Dictionary<string, object>();
            folder.Add("caption", GetCaption(AFolderNode, true));
            folder.Add("icon", TYml2Xml.GetAttribute(AFolderNode, "fa-icon"));

            if (!enabled)
            {
                folder.Add("enabled", "false");
            }

            Dictionary<string, object> items = new Dictionary<string, object>();

            foreach (XmlNode child in AFolderNode.ChildNodes)
            {
                Dictionary<string, object> item = new Dictionary<string, object>();
                item.Add("caption", GetCaption(child, true));
                if (child.ChildNodes.Count == 1 && child.ChildNodes[0].ChildNodes.Count == 0)
                {
                    item.Add("form", child.ChildNodes[0].Name);
                }
                items.Add(child.Name, item);
            }

            folder.Add("items", items);

            return folder;
        }

        private static StringBuilder AddSection(string path, XmlNode ASectionNode, string AUserId)
        {
            // TODO icon?

            // TODO enabled/disabled based on permissions

            StringBuilder ScreenCode = new StringBuilder();

            bool TaskDisplayed = true;

            if ((ASectionNode.FirstChild != null) && (ASectionNode.FirstChild.FirstChild == null))
            {
                ScreenCode.Append("<h3>" + GetCaption(ASectionNode) + "</h3>" + Environment.NewLine);
                TaskDisplayed = false;
            }

            foreach (XmlNode child in ASectionNode.ChildNodes)
            {
                if (child.FirstChild == null)
                {
                    string style = string.Empty;

                    if (!File.Exists(TAppSettingsManager.GetValue("Forms.Path") + "/" + path.Replace('_', '/') + "/" + child.Name + "/" + child.Name + ".html"))
                    {
                        style = " class = 'notimplemented' ";
                        continue;
                    }

                    ScreenCode.Append("<a href='javascript:OpenForm(\"" + path.Replace('_', '/') + "/" + child.Name + "\", \"" +
                        GetCaption(child) + "\")'" + style + ">" + GetCaption(child) + "</a><br/>" + Environment.NewLine);
                    TaskDisplayed = true;
                }
                else
                {
                    if (!TaskDisplayed)
                    {
                        ScreenCode.Append("{notimplementedyet}<br/>" + Environment.NewLine);
                    }

                    ScreenCode.Append("<h3>" + GetCaption(child) + "</h3>" + Environment.NewLine);
                    TaskDisplayed = false;

                    foreach (XmlNode task in child.ChildNodes)
                    {
                        string style = string.Empty;

                        if (!File.Exists(TAppSettingsManager.GetValue("Forms.Path") + "/" + path.Replace('_', '/') + "/" + child.Name + "/" + task.Name + ".html"))
                        {
                            style = " class = 'notimplemented' ";
                            continue;
                        }

                        ScreenCode.Append("<a href='javascript:nav.OpenForm(\"" + path.Replace('_', '/') + "/" + child.Name + "/" + task.Name + "\", \"" +
                            GetCaption(task) + "\")'" + style + ">" + GetCaption(task) + "</a><br/>" + Environment.NewLine);
                        TaskDisplayed = true;
                    }
                }
            }

            if (!TaskDisplayed)
            {
                ScreenCode.Append("{notimplementedyet}<br/>" + Environment.NewLine);
            }

            return ScreenCode;
        }

        private static string FormatLedgerNumberForModuleAccess(int ALedgerNumber)
        {
            return "LEDGER" + ALedgerNumber.ToString("0000");
        }

        private static XmlNode FindSectionNode(XmlNode ACurrentNode, string[] ATaskName, int ADepth = 0)
        {
            while (ACurrentNode != null && ADepth < ATaskName.Length)
            {
                if (ACurrentNode.Name == ATaskName[ADepth])
                {
                    if (ADepth == ATaskName.Length-1)
                    {
                        return ACurrentNode;
                    }

                    XmlNode SectionNode = FindSectionNode(ACurrentNode.FirstChild, ATaskName, ADepth+1);

                    if (SectionNode != null)
                    {
                        return SectionNode;
                    }
                }

                ACurrentNode = ACurrentNode.NextSibling;
            }

            return null;
        }

        /// <summary>
        /// load the html code for a single navigation page, based on the UINavigation file
        /// </summary>
        public static string LoadNavigationPage(string ANavigationPage)
        {
            // TODO: store MainMenuNode in session variable?

            // Force re-calculation of available Ledgers and correct setting of FCurrentLedger
            FLedgersAvailableToUser = null;

            XmlNode MainMenuNode = BuildNavigationXml(false);
            XmlNode DepartmentNode = MainMenuNode.FirstChild;

            StringBuilder ScreenContent = new StringBuilder();

            string[] pageNameSplit = ANavigationPage.Split(new char[] { '_' });

            XmlNode SectionNode = FindSectionNode(DepartmentNode, pageNameSplit);

            if (SectionNode != null)
            {
                ScreenContent.Append(AddSection(ANavigationPage, SectionNode, UserInfo.GUserInfo.UserID));
            }
            else
            {
                return "error: cannot find " + ANavigationPage;
            }

            return ScreenContent.ToString();
        }
    }
}
