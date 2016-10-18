//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
//
// Copyright 2004-2016 by OM International
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
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Ict.Common.IO;
using Ict.Common;

namespace Ict.Tools.CodeGeneration
{
    /// This class generally parses an YAML file independent of the kind of winform (report, etc)
    /// The code is stored in TCodeStorage
    public class TParseYAMLFormsDefinition
    {
        /// <summary>
        /// the storage of the code for the yaml file
        /// </summary>
        public TCodeStorage FCodeStorage = null;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ACodeStorage"></param>
        public TParseYAMLFormsDefinition(ref TCodeStorage ACodeStorage)
        {
            this.FCodeStorage = ACodeStorage;
        }

        /// <summary>
        /// store the template parameters
        /// </summary>
        /// <param name="AParameters"></param>
        public void LoadTemplateParameters(XmlNode AParameters)
        {
            FCodeStorage.FTemplateParameters = AParameters;
        }

        /// parse basic things: BaseClass, FormTitle, Namespace
        public void LoadFormProperties(XmlNode formNode)
        {
            string ModuleForSecurity;

            FCodeStorage.FBaseClass = TYml2Xml.GetAttribute(formNode, "BaseClass");
            FCodeStorage.FInterfaceName = TYml2Xml.GetAttribute(formNode, "InterfaceName");
            FCodeStorage.FUtilObjectClass = TYml2Xml.GetAttribute(formNode, "UtilObjectClass");
            FCodeStorage.FFormTitle = TYml2Xml.GetAttribute(formNode, "FormTitle");
            FCodeStorage.FNamespace = TYml2Xml.GetAttribute(formNode, "Namespace");
            FCodeStorage.FTemplate = TYml2Xml.GetAttribute(formNode, "Template");

            if (TYml2Xml.HasAttribute(formNode, "ModuleForSecurity"))
            {
                FCodeStorage.FModuleForSecurity = TYml2Xml.GetAttribute(formNode, "ModuleForSecurity");
            }
            else
            {
                if (FCodeStorage.FNamespace != String.Empty)
                {
                    // Attempt to find out the Module ('MPartner', 'MPersonnel', etc.) between 'Ict.Petra.Client.' and '.xxx'
                    ModuleForSecurity = FCodeStorage.FNamespace.Substring("Ict.Petra.Client.".Length);

                    if (!ModuleForSecurity.StartsWith("MReporting"))
                    {
                        if (ModuleForSecurity.IndexOf('.') != -1)
                        {
                            ModuleForSecurity = ModuleForSecurity.Substring(0, ModuleForSecurity.IndexOf('.'));

                            if (ModuleForSecurity != String.Empty)
                            {
                                FCodeStorage.FModuleForSecurity = ModuleForSecurity;
                            }
                        }
                    }
                    else
                    {
                        if (FCodeStorage.FNamespace.Length > "Ict.Petra.Client.MReporting.Gui.".Length)
                        {
                            // Attempt to find out the Module ('MPartner', 'MPersonnel', etc.) after 'Ict.Petra.Client.MReporting.Gui.'
                            ModuleForSecurity = FCodeStorage.FNamespace.Substring("Ict.Petra.Client.MReporting.Gui.".Length);

                            if (ModuleForSecurity != String.Empty)
                            {
                                FCodeStorage.FModuleForSecurity = ModuleForSecurity;
                            }
                        }
                    }
                }
            }

            if (TYml2Xml.HasAttribute(formNode, "ModuleForSecurityDerminedByContext"))
            {
                FCodeStorage.FModuleForSecurityDerminedByContext = true;
            }

            // Check whether a form should automatically execute FPetraUtilsObject.ApplySecurity or not
            if (TYml2Xml.HasAttribute(formNode, "AutomaticApplySecurityExecution")
                && (TYml2Xml.GetAttribute(formNode, "AutomaticApplySecurityExecution").ToLower() == "false"))
            {
                FCodeStorage.FAutomaticApplySecurityExecution = false;
            }

            if ((FCodeStorage.FBaseClass == "System.Windows.Forms.UserControl")
                || (FCodeStorage.FBaseClass == "TGrpCollapsible"))
            {
                FCodeStorage.FClassName = "T" + Path.GetFileNameWithoutExtension(FCodeStorage.FFilename);
            }
            else
            {
                FCodeStorage.FClassName = "TFrm" + Path.GetFileNameWithoutExtension(FCodeStorage.FFilename);
            }

            if (FCodeStorage.HasAttribute("ClassName"))
            {
                FCodeStorage.FClassName = FCodeStorage.GetAttribute("ClassName");
            }

            if (TYml2Xml.HasAttribute(formNode, "WindowHeight"))
            {
                FCodeStorage.FHeight = Convert.ToInt32(TYml2Xml.GetAttribute(formNode, "WindowHeight"));
            }

            if (TYml2Xml.HasAttribute(formNode, "WindowWidth"))
            {
                FCodeStorage.FWidth = Convert.ToInt32(TYml2Xml.GetAttribute(formNode, "WindowWidth"));
            }

            if (TYml2Xml.HasAttribute(formNode, "Height") || TYml2Xml.HasAttribute(formNode, "Width"))
            {
                TLogging.Log("Warning: Please use WindowWidth and WindowHeight, because Width and Height for the root node are invalid");
            }
        }

        /// <summary>
        /// load the layout
        /// </summary>
        /// <param name="ALayoutNode"></param>
        public void LoadLayout(XmlNode ALayoutNode)
        {
            if (ALayoutNode != null)
            {
                List <XmlNode>children = TYml2Xml.GetChildren(ALayoutNode, true);

                if ((children.Count > 0) && (children[0].Name == "Tabs"))
                {
                    foreach (XmlNode curNode in children)
                    {
                        AddTabPage(curNode);
                    }
                }
            }
        }

        /// access permissions etc
        public void LoadSecurity(XmlNode ASecurityNode)
        {
            // todo
        }

        /// <summary>
        /// overload for recursive loading, for initial call
        /// </summary>
        /// <param name="AYamlFilename"></param>
        /// <param name="ASelectedLocalisation"></param>
        /// <returns></returns>
        public Boolean LoadRecursively(string AYamlFilename, string ASelectedLocalisation)
        {
            return LoadRecursively(AYamlFilename, ASelectedLocalisation, false, 0);
        }

        static private SortedList <string, XmlDocument>CachedYamlFiles = new SortedList <string, XmlDocument>();

        /// <summary>
        /// clear the cached yaml files
        /// </summary>
        static public void ClearCachedYamlFiles()
        {
            CachedYamlFiles.Clear();
        }

        /// <summary>
        /// this loads the contents of the yaml file.
        /// it supports inheritance, base elements are overwritten
        /// </summary>
        /// <param name="AYamlFilename"></param>
        /// <param name="ASelectedLocalisation"></param>
        /// <param name="AAlreadyGotLocalisation"></param>
        /// <param name="depth">0 is the last file that is derived from all base files</param>
        /// <returns></returns>
        protected Boolean LoadRecursively(string AYamlFilename,
            string ASelectedLocalisation,
            bool AAlreadyGotLocalisation,
            Int32 depth)
        {
            // check if file exists for localisation
            string localisedFile = null;

            if ((ASelectedLocalisation != null) && !AAlreadyGotLocalisation)
            {
                localisedFile = Path.GetDirectoryName(AYamlFilename) + Path.DirectorySeparatorChar +
                                Path.GetFileNameWithoutExtension(AYamlFilename) + "." + ASelectedLocalisation + ".yaml";

                // first check if there is such a file
                if (!File.Exists(localisedFile))
                {
                    localisedFile = null;
                }
            }

            if (localisedFile == null)
            {
                localisedFile = AYamlFilename;
            }

            string baseyaml;

            if (!TYml2Xml.ReadHeader(localisedFile, out baseyaml))
            {
                throw new Exception("This is not an OpenPetra Yaml file");
            }

            if ((baseyaml.Length > 0) && baseyaml.EndsWith(".yaml"))
            {
                LoadRecursively(System.IO.Path.GetDirectoryName(AYamlFilename) +
                    System.IO.Path.DirectorySeparatorChar +
                    baseyaml,
                    ASelectedLocalisation,
                    localisedFile != AYamlFilename,
                    depth - 1);
            }

            if ((depth == 0) && (FCodeStorage.FXmlNodes != null))
            {
                // apply the tag, so that we know which things have been changed by the last yml file
                TYml2Xml.Tag((XmlNode)FCodeStorage.FXmlNodes[TParseYAMLFormsDefinition.ROOTNODEYML]);
            }

            XmlDocument newDoc = null;

            localisedFile = Path.GetFullPath(localisedFile);

            if (CachedYamlFiles.ContainsKey(localisedFile))
            {
                newDoc = CachedYamlFiles[localisedFile];
            }
            else
            {
                Console.WriteLine("Loading " + localisedFile + "...");
                TYml2Xml yml2xml = new TYml2Xml(localisedFile);
                newDoc = yml2xml.ParseYML2XML();
                CachedYamlFiles.Add(localisedFile, newDoc);
            }

            TYml2Xml.Merge(ref FCodeStorage.FXmlDocument, newDoc, depth);

            if (TLogging.DebugLevel > 0)
            {
                // for debugging:
                FCodeStorage.FXmlDocument.Save(localisedFile + ".xml");
            }

            FCodeStorage.FXmlNodes = TYml2Xml.ReferenceNodes(FCodeStorage.FXmlDocument);
            FCodeStorage.FRootNode = (XmlNode)FCodeStorage.FXmlNodes[TParseYAMLFormsDefinition.ROOTNODEYML];

            if (baseyaml.Length == 0)
            {
                if (FCodeStorage.FXmlNodes[TYml2Xml.ROOTNODEINTERNAL] == null)
                {
                    throw new Exception("TParseYAMLFormsDefinition.LoadRecursively: YML Document could not be properly parsed");
                }

                if (TXMLParser.GetAttribute((XmlNode)FCodeStorage.FXmlNodes[TParseYAMLFormsDefinition.ROOTNODEYML], "BaseYaml").Length > 0)
                {
                    throw new Exception("The BaseYaml attribute must come first!");
                }
            }

            if (depth == 0)
            {
                FCodeStorage.FFilename = AYamlFilename;
                LoadData(FCodeStorage.FXmlNodes);
            }

            return true;
        }

        /// the name used for root node in yml code
        public static string ROOTNODEYML = "RootNode";

        /// <summary>
        /// summary function for processing the yaml file
        /// </summary>
        /// <param name="nodes"></param>
        protected void LoadData(SortedList nodes)
        {
            LoadFormProperties((XmlNode)nodes[TParseYAMLFormsDefinition.ROOTNODEYML]);
            LoadTemplateParameters((XmlNode)nodes["TemplateParameters"]);
            LoadSecurity((XmlNode)nodes["Security"]);
            LoadControls((XmlNode)nodes["Controls"]);
            LoadLayout((XmlNode)nodes["Layout"]);

            // todo: what about popup menus?; can contain menu items from the main menu
            if (FCodeStorage.HasRootControl("mnu"))
            {
                LoadMenu(FCodeStorage.GetRootControl("mnu").controlName, (XmlNode)nodes["Menu"]);
            }

            if (FCodeStorage.HasRootControl("tbr"))
            {
                LoadToolbar(FCodeStorage.GetRootControl("tbr").controlName, (XmlNode)nodes["Toolbar"]);
            }

            LoadActions((XmlNode)nodes["Actions"]);
            LoadEvents((XmlNode)nodes["Events"]);
            LoadReportParameters((XmlNode)nodes["ReportParameters"]);
        }

        Int32 FMenuSeparatorCount = 0;

        /// <summary>
        /// load the menu
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="curNode"></param>
        /// <returns></returns>
        public Boolean LoadMenu(string parentName,
            XmlNode curNode)
        {
            string menuName = curNode.Name;

            if (menuName == "mniSeparator")
            {
                // UniqueName is not stored to yml again; just used temporary
                TYml2Xml.SetAttribute(curNode, "UniqueName", menuName + FMenuSeparatorCount.ToString());
                FMenuSeparatorCount++;
            }

            if (curNode.ParentNode.Name == TParseYAMLFormsDefinition.ROOTNODEYML)
            {
                // add each menu, but obviously not the "Menu" tag
                XmlNode menuNode = curNode;
                List <XmlNode>children = TYml2Xml.GetChildren(menuNode, true);

                foreach (XmlNode childNode in children)
                {
                    LoadMenu(parentName, childNode);

                    // attach the menu to the appropriate root control
                    XmlNode rootMenu = FCodeStorage.GetRootControl("mnu").xmlNode;
                    rootMenu.AppendChild(childNode);
                }

                return true;
            }

            TControlDef menuItem = FCodeStorage.AddControl(curNode);
            menuItem.parentName = parentName;
            List <XmlNode>children2 = TYml2Xml.GetChildren(curNode, true);

            foreach (XmlNode childNode in children2)
            {
                // the check for mni works around problems with elements list, shortcutkeys
                if (childNode.Name.StartsWith("mni"))
                {
                    LoadMenu(menuName, childNode);
                }
            }

            return true;
        }

        Int32 FToolbarSeparatorCount = 0;

        /// <summary>
        /// load the toolbar
        /// </summary>
        /// <param name="parentName"></param>
        /// <param name="curNode"></param>
        /// <returns></returns>
        public Boolean LoadToolbar(string parentName,
            XmlNode curNode)
        {
            List <XmlNode>children = TYml2Xml.GetChildren(curNode, true);

            XmlNode rootBarNode = FCodeStorage.GetRootControl("tbr").xmlNode;

            foreach (XmlNode childNode in children)
            {
                // the check for tbb works around problems with elements list, shortcutkeys
                if (childNode.Name.StartsWith("tbb") || childNode.Name.StartsWith("tbc"))
                {
                    string tbbName = childNode.Name;

                    if (tbbName == "tbbSeparator")
                    {
                        // UniqueName is not stored to yml again; just used temporary
                        TYml2Xml.SetAttribute(childNode, "UniqueName", tbbName + FToolbarSeparatorCount.ToString());
                        FToolbarSeparatorCount++;
                    }

                    TControlDef tbbItem = FCodeStorage.AddControl(childNode);
                    tbbItem.parentName = parentName;

                    rootBarNode.AppendChild(childNode);
                }
                else
                {
                    // use ToolStripControlHost to host any control
                    TControlDef tbbItem = FCodeStorage.AddControl(childNode);
                    string prefix = TControlDef.GetLowerCasePrefix(childNode.Name);
                    tbbItem.parentName = "tch" + childNode.Name.Substring(prefix.Length);

                    XmlNode controlHostNode = rootBarNode.OwnerDocument.CreateElement(tbbItem.parentName);
                    TYml2Xml.SetAttribute(controlHostNode, "depth", TYml2Xml.GetAttribute(childNode, "depth"));
                    TYml2Xml.SetAttribute(controlHostNode, "HostedControl", childNode.Name);
                    rootBarNode.AppendChild(controlHostNode);

                    XmlNode controlsNode = rootBarNode.OwnerDocument.CreateElement("Controls");
                    controlHostNode.AppendChild(controlsNode);
                    XmlNode elementNode = rootBarNode.OwnerDocument.CreateElement("Element");
                    controlsNode.AppendChild(elementNode);
                    TYml2Xml.SetAttribute(elementNode, "name", childNode.Name);

                    TControlDef hostItem = FCodeStorage.AddControl(controlHostNode);
                    hostItem.parentName = parentName;
                }
            }

            return true;
        }

        /// <summary>
        /// load the controls
        /// </summary>
        /// <param name="curNode"></param>
        protected void LoadControls(XmlNode curNode)
        {
            if (curNode != null)
            {
                List <XmlNode>children = TYml2Xml.GetChildren(curNode, true);

                foreach (XmlNode childNode in children)
                {
                    FCodeStorage.AddControl(childNode);
                }
            }
        }

        /// <summary>
        /// load the actions
        /// </summary>
        /// <param name="curNode"></param>
        protected void LoadActions(XmlNode curNode)
        {
            if (curNode != null)
            {
                List <XmlNode>children = TYml2Xml.GetChildren(curNode, true);

                foreach (XmlNode childNode in children)
                {
                    FCodeStorage.AddAction(childNode);
                }
            }
        }

        /// <summary>
        /// load the events
        /// </summary>
        /// <param name="curNode"></param>
        protected void LoadEvents(XmlNode curNode)
        {
            if (curNode != null)
            {
                List <XmlNode>children = TYml2Xml.GetChildren(curNode, true);

                foreach (XmlNode childNode in children)
                {
                    FCodeStorage.AddEvent(childNode);
                }
            }
        }

        /// <summary>
        /// load the parameters of a report
        /// </summary>
        /// <param name="curNode"></param>
        protected void LoadReportParameters(XmlNode curNode)
        {
            if (curNode != null)
            {
                List <XmlNode>children = TYml2Xml.GetChildren(curNode, true);

                foreach (XmlNode childNode in children)
                {
                    FCodeStorage.AddReportParameter(childNode, curNode.Attributes["ColumnFunction"].Value);
                }
            }
        }

        /// <summary>
        /// process a tab page
        /// </summary>
        /// <param name="curNode"></param>
        /// <returns></returns>
        public Boolean AddTabPage(XmlNode curNode)
        {
            // name of tabpage
            TControlDef tabPage = FCodeStorage.AddControl(curNode);

            tabPage.parentName = FCodeStorage.GetRootControl("tab").controlName;
            curNode = TXMLParser.NextNotBlank(curNode.FirstChild);

            if (curNode != null)
            {
                if (curNode.Name == "Controls")
                {
                    // one control per row, align labels
                    StringCollection controls = TYml2Xml.GetElements(curNode);

                    foreach (string ctrlName in controls)
                    {
                        TControlDef ctrl = FCodeStorage.GetControl(ctrlName);

                        if (ctrl != null)
                        {
                            ctrl.parentName = tabPage.controlName;
                        }
                    }
                }
            }

            return true;
        }
    }
}