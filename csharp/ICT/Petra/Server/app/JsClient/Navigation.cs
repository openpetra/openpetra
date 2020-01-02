//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2020 by OM International
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
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Security;
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
        /// <summary>
        /// checks if the user has access to the navigation node
        /// </summary>
        public bool HasAccessPermission(XmlNode ANode, TPetraPrincipal AUserInfo)
        {
            // TODO: if this is an action node, eg. opens a screen, check the static function that tells RequiredPermissions of the screen

            string PermissionsRequired = TXMLParser.GetAttributeRecursive(ANode, "PermissionsRequired", true);

            while (PermissionsRequired.Length > 0)
            {
                string PermissionRequired = StringHelper.GetNextCSV(ref PermissionsRequired);

                if (!AUserInfo.IsInModule(PermissionRequired))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// build an XML document with the navigation
        /// </summary>
        public XmlNode BuildNavigationXml()
        {
            string UINavigationFile = TAppSettingsManager.GetValue("UINavigation.File");

            if (!File.Exists(UINavigationFile))
            {
                throw new Exception ("cannot find file " + UINavigationFile);
            }

            TYml2Xml parser = new TYml2Xml(UINavigationFile);
            XmlDocument UINavigation = parser.ParseYML2XML();

            XmlNode OpenPetraNode = UINavigation.FirstChild.NextSibling.FirstChild;
            XmlNode MainMenuNode = OpenPetraNode.FirstChild;

            return MainMenuNode;
        }

        /// <summary>
        /// load the navigation ready to be converted to JSON
        /// </summary>
        public Dictionary<string, object> LoadNavigationUI()
        {
            TPetraPrincipal userinfo = UserInfo.GetUserInfo();

            XmlNode MainMenuNode = BuildNavigationXml();
            XmlNode DepartmentNode = MainMenuNode.FirstChild;

            Dictionary<string, object> result = new Dictionary<string, object>();

            while (DepartmentNode != null)
            {
                Dictionary<string, object> Folder = AddFolder(DepartmentNode, userinfo);

                if (Folder != null)
                {
                    result.Add(DepartmentNode.Name, Folder);
                }

                DepartmentNode = DepartmentNode.NextSibling;
            }

            return result;
        }

        private string GetCaption(XmlNode ANode, bool AWithoutBrackets=false)
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

        private Dictionary<string, object> GetChildItems(XmlNode AFolderNode, string APath, TPetraPrincipal AUserInfo)
        {
            Dictionary<string, object> items = new Dictionary<string, object>();

            foreach (XmlNode child in AFolderNode.ChildNodes)
            {
                if (!HasAccessPermission(child, AUserInfo))
                {
                    continue;
                }

                Dictionary<string, object> item = new Dictionary<string, object>();
                item.Add("caption", GetCaption(child, true));
                if (child.ChildNodes.Count > 0)
                {
                    item.Add("items", GetChildItems(child, APath + "/" + child.Name, AUserInfo));
                }
                else
                {
                    item.Add("form", child.Name);
                }

                string Action = TXMLParser.GetAttribute(child, "Action");

                if (Action.Length > 0)
                {
                    item.Add("action", Action);
                }

                string Icon = TXMLParser.GetAttributeRecursive(child, "fa-icon", false);

                if (Icon.Length > 0)
                {
                    item.Add("icon", Icon);
                }

                string form = TXMLParser.GetAttribute(child, "Form");

                if (form.Length > 0)
                {
                    item["form"] = form;
                }

                string path = TXMLParser.GetAttributeRecursive(child, "Path", false);

                if (path.Length == 0)
                {
                    path = APath;
                }

                item.Add("path", path);

                if (File.Exists(TAppSettingsManager.GetValue("Forms.Path") + "/" + path.Replace('_', '/') + "/" + child.Name + ".html"))
                {
                    item["htmlexists"] = true;
                }

                items.Add(child.Name, item);
            }

            return items;
        }


        private Dictionary<string, object> AddFolder(XmlNode AFolderNode, TPetraPrincipal AUserInfo)
        {
            // enabled/disabled based on permissions
            bool enabled = true;
            if ((TYml2Xml.HasAttribute(AFolderNode, "Enabled"))
                && (TYml2Xml.GetAttribute(AFolderNode, "Enabled").ToLower() == "false"))
            {
                enabled = false;
            }
            else
            {
                enabled = HasAccessPermission(AFolderNode, AUserInfo);
            }

            if (!enabled)
            {
                return null;
            }

            Dictionary<string, object> folder = new Dictionary<string, object>();
            folder.Add("caption", GetCaption(AFolderNode, true));
            folder.Add("icon", TYml2Xml.GetAttribute(AFolderNode, "fa-icon"));

            if (!enabled)
            {
                folder.Add("enabled", "false");
            }

            folder.Add("items", GetChildItems(AFolderNode, AFolderNode.Name, AUserInfo));

            if (File.Exists(TAppSettingsManager.GetValue("Forms.Path") + "/" + AFolderNode.Name + ".html"))
            {
                folder["htmlexists"] = true;
            }

            return folder;
        }

#if notused
        private StringBuilder AddSection(string path, XmlNode ASectionNode, string AUserId)
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

        private XmlNode FindSectionNode(XmlNode ACurrentNode, string[] ATaskName, int ADepth = 0)
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
        public string LoadNavigationPage(string ANavigationPage)
        {
            // TODO: store MainMenuNode in session variable?

            XmlNode MainMenuNode = BuildNavigationXml();
            XmlNode DepartmentNode = MainMenuNode.FirstChild;

            StringBuilder ScreenContent = new StringBuilder();

            string[] pageNameSplit = ANavigationPage.Split(new char[] { '_' });

            XmlNode SectionNode = FindSectionNode(DepartmentNode, pageNameSplit);

            if (SectionNode != null)
            {
                ScreenContent.Append(AddSection(ANavigationPage, SectionNode, UserInfo.GetUserInfo().UserID));
            }
            else
            {
                return "error: cannot find " + ANavigationPage;
            }

            return ScreenContent.ToString();
        }
#endif
    }
}
