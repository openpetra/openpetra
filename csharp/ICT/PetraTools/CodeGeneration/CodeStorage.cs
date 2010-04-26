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
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration
{
    /*
     * This class represents the code of a windows form in memory
     * There are other classes that fill this model by reading from the Designer.cs file or the yaml file
     * There are other classes that will write the code
     *
     * each xml element is directly linked in the FXmlNodes list, no matter which depth it is in the actual xml structure
     * each control (menu, gui controls, etc) is linked in the FControlList
     * all changes are done to the xml structure as well as to the FControlList and the FXmlNodes
     * this allows easy saving of the modified xml
     */
    public class TCodeStorage
    {
        /// contains all controls, ie also menus etc; this is a sorted list for easily finding values, but also keep them ordered
        public Dictionary <string, TControlDef>FControlList = new Dictionary <string, TControlDef>();

        /// it seems, on Mono the Dictionary gets sorted differently, therefore it is not useful for getting the RootControl etc; so we use a specific SortedList for this
        public SortedList <int, TControlDef>FSortedControlList = new SortedList <int, TControlDef>();
        public Dictionary <string, TEventHandler>FEventList = new Dictionary <string, TEventHandler>();
        public Dictionary <string, TActionHandler>FActionList = new Dictionary <string, TActionHandler>();

        //public ArrayList FActionList = new ArrayList();
        public string FBaseClass = "";
        public string FInterfaceName = "";
        public string FUtilObjectClass = "";
        public string FClassName = "";
        public string FFormTitle = "";
        public string FNamespace = "";
        public string FFilename = "";
        public string FManualCodeFilename = "";
        public string FEventHandler = "";
        public string FEventHandlersImplementation = "";
        public string FActionHandlers = "";

        /// can be net-2.0 for Windows .net, or mono-2.0 for Mono; mainly to resolve issues with TableLayoutPanel and AutoSize etc
        public string FTargetWinforms = "net-2.0";

        public Int32 FHeight = 500;
        public Int32 FWidth = 700;
        public XmlNode FTemplateParameters = null;
        public XmlDocument FXmlDocument = null;
        public SortedList FXmlNodes = null;
        public XmlNode FRootNode = null;

        private string FManualCodeFileContent = "";

        public TCodeStorage(XmlDocument AXmlDocument, SortedList AXmlNodes, string AManualCodeFilename)
        {
            FXmlDocument = AXmlDocument;
            FXmlNodes = AXmlNodes;
            FManualCodeFilename = AManualCodeFilename;
        }

        /// <summary>
        /// check if the text is in the manual code file for the file that is about to be generated
        /// </summary>
        /// <param name="ASearchText"></param>
        /// <returns></returns>
        public bool ManualFileExistsAndContains(string ASearchText)
        {
            if ((FManualCodeFilename.Length > 0) && (FManualCodeFileContent.Length == 0) && System.IO.File.Exists(FManualCodeFilename))
            {
                System.IO.StreamReader r = new System.IO.StreamReader(FManualCodeFilename);
                FManualCodeFileContent = r.ReadToEnd();
                r.Close();
            }

            return FManualCodeFileContent.Contains(ASearchText);
        }

        /// <summary>
        /// check if main or ManualCode file contains the search text
        /// </summary>
        /// <param name="ANamespaceAndClassname">only works for files in the same directory at the moment</param>
        /// <param name="ASearchText"></param>
        /// <returns></returns>
        public bool ImplementationContains(string ANamespaceAndClassname, string ASearchText)
        {
            string pathAndName = System.IO.Path.GetDirectoryName(this.FFilename).Replace("\\", "/");

            // cut off after /Client/lib
            pathAndName = pathAndName.Substring(0, pathAndName.IndexOf("Client/lib") + "Client/lib".Length);

            // use only last part of namespace after Ict.Petra.Client
            ANamespaceAndClassname = ANamespaceAndClassname.Substring("Ict.Petra.Client".Length).Replace(".Gui.", ".gui.");
            pathAndName += ANamespaceAndClassname.Substring(0, ANamespaceAndClassname.LastIndexOf(".")).Replace(".", "/");

            // get the file name without TFrm prefix
            string filename = "/" + ANamespaceAndClassname.Substring(ANamespaceAndClassname.LastIndexOf(".") + 1 + 4);
            string alternativePathName = pathAndName;

            if (!System.IO.File.Exists(pathAndName + filename + ".cs"))
            {
                // try to use path name without the last part of the namespace
                // eg Ict.Petra.Client.MFinance.Gui.Setup.TFrmSetupCurrency is in MFinance/Gui/SetupCurrency.cs

                pathAndName = pathAndName.Substring(0, pathAndName.LastIndexOf("/"));
            }

            if (System.IO.File.Exists(pathAndName + filename + ".cs"))
            {
                System.IO.StreamReader r = new System.IO.StreamReader(pathAndName + filename + ".cs");
                string temp = r.ReadToEnd();
                r.Close();

                if (temp.Contains(ASearchText))
                {
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Warning naming conventions: cannot find file " +
                    pathAndName + filename + ".cs or " +
                    alternativePathName + filename + ".cs");
            }

            if (System.IO.File.Exists(pathAndName + filename + ".ManualCode.cs"))
            {
                System.IO.StreamReader r = new System.IO.StreamReader(pathAndName + filename + ".ManualCode.cs");
                string temp = r.ReadToEnd();
                r.Close();

                if (temp.Contains(ASearchText))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// returns a list of controls that are not used anywhere on the screen
        /// </summary>
        /// <returns></returns>
        public List <TControlDef>GetOrphanedControls()
        {
            List <TControlDef>result = new List <TControlDef>();

            foreach (TControlDef ctrl in FSortedControlList.Values)
            {
                if ((ctrl.parentName == null) || (ctrl.parentName.Length == 0))
                {
                    if (ctrl.HasAttribute("RootControl") && (ctrl.GetAttribute("RootControl").ToLower() == "true"))
                    {
                        // a root control has no parent by definition
                    }
                    else if ((GetRootControl("content", false) == ctrl) || (GetRootControl("mnu", false) == ctrl)
                             || (GetRootControl("tbr", false) == ctrl) || (GetRootControl("stb", false) == ctrl))
                    {
                        // root control has no parent
                    }
                    else
                    {
                        result.Add(ctrl);
                    }
                }
            }

            return result;
        }

        public TControlDef GetControl(string AControlName)
        {
            if (FControlList.ContainsKey(AControlName))
            {
                return (TControlDef)FControlList[AControlName];
            }

            return null;
        }

        /// <summary>
        /// get the main menustrip, tabpage, statusstrip etc;
        /// if prefix is content, the first available pnl, uco, tpg or grp control is returned;
        /// the first control can be overwritten with attribute RootControl=true; this is necessary for the reports, see tabReportSettings
        /// </summary>
        /// <param name="APrefix">can be mnu, tab, tbr, sbt, stb, content</param>
        /// <param name="AThrowException">if there is no root control, throw an exception</param>
        /// <returns></returns>
        public TControlDef GetRootControl(string APrefix, bool AThrowException)
        {
            TControlDef firstControl = null;

            foreach (TControlDef ctrl in FSortedControlList.Values)
            {
                if (ctrl.controlTypePrefix == APrefix)
                {
                    if (firstControl == null)
                    {
                        firstControl = ctrl;
                    }

                    if (ctrl.HasAttribute("RootControl") && (ctrl.GetAttribute("RootControl").ToLower() == "true"))
                    {
                        return ctrl;
                    }
                }

                if ((APrefix == "content")
                    && ((ctrl.controlTypePrefix == "tab")
                        || (ctrl.controlTypePrefix == "grp")
                        || (ctrl.controlTypePrefix == "uco")
                        || (ctrl.controlTypePrefix == "pnl")))
                {
                    if (firstControl == null)
                    {
                        firstControl = ctrl;
                    }

                    if (ctrl.HasAttribute("RootControl") && (ctrl.GetAttribute("RootControl").ToLower() == "true"))
                    {
                        return ctrl;
                    }
                }
            }

            if (firstControl != null)
            {
                return firstControl;
            }

            if (AThrowException)
            {
                throw new Exception("cannot find a default control for prefix " + APrefix);
            }

            return null;
        }

        /// <summary>
        /// get the main menustrip, tabpage, statusstrip etc;
        /// if prefix is content, the first available pnl, uco, tpg or grp control is returned;
        /// the first control can be overwritten with attribute RootControl=true; this is necessary for the reports, see tabReportSettings
        /// </summary>
        /// <param name="APrefix">can be mnu, tab, tbr, sbt, stb, content</param>
        /// <returns></returns>
        public TControlDef GetRootControl(string APrefix)
        {
            return GetRootControl(APrefix, true);
        }

        public bool HasRootControl(string APrefix)
        {
            foreach (TControlDef ctrl in FSortedControlList.Values)
            {
                if (ctrl.controlTypePrefix == APrefix)
                {
                    return true;
                }

                if ((APrefix == "content")
                    && ((ctrl.controlTypePrefix == "tab")
                        || (ctrl.controlTypePrefix == "grp")
                        || (ctrl.controlTypePrefix == "uco")
                        || (ctrl.controlTypePrefix == "pnl")))
                {
                    return true;
                }
            }

            return false;
        }

        public XmlNode GetCorrectCollection(string AControlName, string AParentName)
        {
            // which node does this control belong to?
            string prefix = TControlDef.GetLowerCasePrefix(AControlName);
            XmlNode collectionNode = (XmlNode)FXmlNodes["Controls"];

            if (prefix == "mni")
            {
                collectionNode = (XmlNode)FXmlNodes["Menu"];

                // submenus
                if (AParentName.Length > 0)
                {
                    collectionNode = (XmlNode)FXmlNodes[AParentName];
                }
            }
            else if ((prefix == "tbb") || (prefix == "tbc") || (prefix == "tch"))
            {
                collectionNode = (XmlNode)FXmlNodes["Toolbar"];
            }
            else if (prefix == "tpg")
            {
                collectionNode = ((XmlNode)FXmlNodes["Layout"])["Tabs"];
            }

            return collectionNode;
        }

        /// <summary>
        /// check if the root node has the given attribute
        /// </summary>
        /// <param name="AAttributeName"></param>
        /// <returns></returns>
        public bool HasAttribute(string AAttributeName)
        {
            return TYml2Xml.HasAttribute(FRootNode, AAttributeName);
        }

        /// <summary>
        /// get the value of the attribute of the root node
        /// </summary>
        /// <param name="AAttributeName"></param>
        /// <returns></returns>
        public string GetAttribute(string AAttributeName)
        {
            return TYml2Xml.GetAttribute(FRootNode, AAttributeName);
        }

        public static TDataDefinitionStore FPetraXMLStore = null;
        public static SortedList <string, TTable>FDatasetTables = null;

        /// <summary>
        /// load the dataset tables
        /// </summary>
        /// <param name="ADataSetTypeWithNamespace"></param>
        /// <returns></returns>
        public SortedList <string, TTable>LoadDatasetTables(string ADataSetTypeWithNamespace)
        {
            if (!ADataSetTypeWithNamespace.StartsWith("Ict.Petra.Shared"))
            {
                throw new Exception("the DatasetType must contain the full namespace, starting with Ict.Petra.Shared");
            }

            string[] datasetTypeSplit = ADataSetTypeWithNamespace.Split(new char[] { '.' });
            string module = datasetTypeSplit[3];
            string datasetName = datasetTypeSplit[datasetTypeSplit.Length - 1];

            // find the correct xml file for the dataset.
            // look in Ict/Petra/Shared/lib/MODULE/data
            string dataPath = CSParser.ICTPath + "/Petra/Shared/lib/" + module + "/data/";

            DirectoryInfo directory = new DirectoryInfo(dataPath);
            FileInfo[] xmlFiles = directory.GetFiles("*.xml");
            XmlNode datasetNode = null;

            foreach (FileInfo fileinfo in xmlFiles)
            {
                if (datasetNode == null)
                {
                    TXMLParser parser = new TXMLParser(dataPath + "/" + fileinfo.Name, false);
                    datasetNode = parser.GetDocument().SelectSingleNode(String.Format("//DataSet[@name='{0}']", datasetName));
                }
            }

            if (datasetNode == null)
            {
                throw new Exception("cannot find the xml file for dataset " + ADataSetTypeWithNamespace);
            }

            SortedList <string, TTable>result = new SortedList <string, TTable>();
            XmlNodeList tables = datasetNode.SelectNodes("Table|CustomTable");

            foreach (XmlNode tableNode in tables)
            {
                TTable table;
                string tablename;

                if (tableNode.Name == "Table")
                {
                    tablename = TTable.NiceTableName(tableNode.Attributes["sqltable"].Value);
                    table = FPetraXMLStore.GetTable(tablename);
                    table.strVariableNameInDataset = TXMLParser.HasAttribute(tableNode, "name") ? tableNode.Attributes["name"].Value : tablename;

                    if ((tableNode.SelectNodes("CustomField").Count > 0) || (tableNode.SelectNodes("Field").Count > 0))
                    {
                        table.strDotNetName = datasetName + tablename;
                    }
                }
                else
                {
                    table = new TTable();
                    tablename = tableNode.Attributes["name"].Value;
                    table.strName = tablename;
                    table.strDotNetName = tablename;
                    table.strVariableNameInDataset = tablename;
                }

                // add the custom fields if there are any
                XmlNodeList customFields = tableNode.SelectNodes("CustomField");

                foreach (XmlNode customField in customFields)
                {
                    TTableField newField = new TTableField();
                    newField.strName = customField.Attributes["name"].Value;
                    newField.strNameDotNet = newField.strName;
                    newField.strType = customField.Attributes["type"].Value;
                    newField.strTypeDotNet = customField.Attributes["type"].Value;
                    newField.strTableName = tablename;
                    newField.strDescription = "";
                    newField.bNotNull =
                        TXMLParser.HasAttribute(customField, "notnull") && TXMLParser.GetAttribute(customField, "notnull").ToLower() == "true";
                    table.grpTableField.List.Add(newField);
                }

                // add other fields from other tables that are defined in petra.xml
                XmlNodeList otherFields = tableNode.SelectNodes("Field");

                foreach (XmlNode otherField in otherFields)
                {
                    TTable otherTable = FPetraXMLStore.GetTable(otherField.Attributes["sqltable"].Value);
                    TTableField newField = new TTableField(otherTable.GetField(otherField.Attributes["sqlfield"].Value));
                    newField.strTableName = tablename;
                    table.grpTableField.List.Add(newField);
                }

                result.Add(tablename, table);
            }

            return result;
        }

        /// <summary>
        /// resolve a reference to a field in the database;
        /// support master and detail tables
        /// </summary>
        /// <param name="ACtrl"></param>
        /// <param name="ADataField">string with potential reference to data field</param>
        /// <param name="AIsDetailNotMaster">returns if this is a field in the master or in the detail table</param>
        /// <param name="AShowWarningNonExistingField">fail on non existing field</param>
        /// <returns>reference to the table field, or null</returns>
        public TTableField GetTableField(TControlDef ACtrl,
            string ADataField,
            out bool AIsDetailNotMaster,
            bool AShowWarningNonExistingField)
        {
            string tablename = "";
            string fieldname = "";

            // is there an explicit specification for which datafield to use
            if (ADataField.IndexOf(".") > 0)
            {
                tablename = ADataField.Split('.')[0];
                fieldname = ADataField.Split('.')[1];
            }
            else
            {
                if (ACtrl != null)
                {
                    if (ACtrl.HasAttribute("Unbound") && (ACtrl.GetAttribute("Unbound").ToLower() == "true"))
                    {
                        AIsDetailNotMaster = false;
                        return null;
                    }

                    if (ADataField.Length == 0)
                    {
                        ADataField = ACtrl.controlName.Substring(ACtrl.controlTypePrefix.Length);
                    }

                    if (ADataField.StartsWith("Detail"))
                    {
                        fieldname = ADataField;

                        if (fieldname.StartsWith("Detail"))
                        {
                            fieldname = fieldname.Substring("Detail".Length);
                        }

                        tablename = GetAttribute("DetailTable");
                    }
                    else
                    {
                        // check for other tables in the typed Dataset
                        if (FDatasetTables != null)
                        {
                            // ADataField can either contain just the field, or the table name and the field

                            // does ADataField start with a table name?
                            foreach (TTable table2 in FDatasetTables.Values)
                            {
                                if (ADataField.StartsWith(table2.strDotNetName) || ADataField.StartsWith(TTable.NiceTableName(table2.strName)))
                                {
                                    if (ADataField.StartsWith(table2.strDotNetName))
                                    {
                                        tablename = table2.strDotNetName;
                                    }
                                    else
                                    {
                                        tablename = TTable.NiceTableName(table2.strName);
                                    }

                                    foreach (TTableField field in table2.grpTableField.List)
                                    {
                                        if (TTable.NiceFieldName(field.strName) == ADataField.Substring(tablename.Length))
                                        {
                                            fieldname = TTable.NiceFieldName(field.strName);
                                        }

                                        if (field.strNameDotNet == ADataField.Substring(tablename.Length))
                                        {
                                            fieldname = field.strNameDotNet;
                                        }
                                    }
                                }
                            }

                            // check if the master table has such a field
                            if ((fieldname.Length == 0) && HasAttribute("MasterTable"))
                            {
                                TTable table2 = FDatasetTables[GetAttribute("MasterTable")];

                                foreach (TTableField field in table2.grpTableField.List)
                                {
                                    if ((TTable.NiceFieldName(field.strName) == ADataField) || (field.strNameDotNet == ADataField))
                                    {
                                        tablename = GetAttribute("MasterTable");
                                        fieldname = ADataField;
                                    }
                                }
                            }

                            // check if there is a unique match for this control name in all the tables
                            if (fieldname.Length == 0)
                            {
                                foreach (TTable table2 in FDatasetTables.Values)
                                {
                                    foreach (TTableField field in table2.grpTableField.List)
                                    {
                                        if ((TTable.NiceFieldName(field.strName) == ADataField) || (field.strNameDotNet == ADataField))
                                        {
                                            if (fieldname.Length > 0)
                                            {
                                                throw new Exception(
                                                    String.Format("there are a several tables with field {0}: {1} and {2}",
                                                        fieldname, tablename, table2.strDotNetName));
                                            }

                                            if ((table2.strDotNetName != null) && (table2.strDotNetName.Length > 0))
                                            {
                                                tablename = table2.strDotNetName;
                                            }
                                            else
                                            {
                                                tablename = TTable.NiceTableName(table2.strName);
                                            }

                                            if ((field.strNameDotNet != null) && (field.strNameDotNet.Length > 0))
                                            {
                                                fieldname = field.strNameDotNet;
                                            }
                                            else
                                            {
                                                fieldname = TTable.NiceFieldName(field.strName);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            tablename = GetAttribute("MasterTable");
                            fieldname = ADataField;
                        }
                    }
                }
                else
                {
                    // eg used for columns in datagrid
                    if (ADataField.StartsWith("Detail"))
                    {
                        tablename = GetAttribute("DetailTable");
                        fieldname = ADataField.Substring("Detail".Length);
                    }
                    else
                    {
                        tablename = GetAttribute("MasterTable");
                        fieldname = ADataField;
                    }
                }
            }

            AIsDetailNotMaster = GetAttribute("DetailTable") == tablename;

            if (tablename.Length == 0)
            {
                // this is probably no data field at all
                if (AShowWarningNonExistingField)
                {
                    Console.WriteLine("Expected data field but cannot resolve " + ADataField);
                }

                return null;
            }

            TTable table = null;

            if ((FDatasetTables != null) && FDatasetTables.ContainsKey(tablename))
            {
                table = FDatasetTables[tablename];
            }
            else
            {
                table = FPetraXMLStore.GetTable(tablename);
            }

            if (table == null)
            {
                if (!AShowWarningNonExistingField)
                {
                    return null;
                }

                throw new Exception("Cannot find table: " + tablename);
            }

            return table.GetField(fieldname, AShowWarningNonExistingField);
        }

        // only to be called by TParseXAML when loading the controls from the file
        // don't call this for creating new nodes; use FindOrCreateControl instead
        public TControlDef AddControl(XmlNode AParsedNode)
        {
            if (AParsedNode.Name == "base")
            {
                throw new Exception("should not parse the 'base' node this way");
            }

            string parsedNodeName = AParsedNode.Name;

            if (TYml2Xml.HasAttribute(AParsedNode, "UniqueName"))
            {
                parsedNodeName = TYml2Xml.GetAttribute(AParsedNode, "UniqueName");
            }

            TControlDef result = GetControl(parsedNodeName);

            if (result != null)
            {
                // this node already existed in a base yaml and is now being overwritten

                // this should never happen
                string nameDetails = AParsedNode.Name;
                XmlNode parentNode = AParsedNode.ParentNode;

                while ((parentNode != null) && (parentNode.Name != "AutoConvertedYML2XML"))
                {
                    nameDetails = parentNode.Name + "." + nameDetails;
                    parentNode = parentNode.ParentNode;
                }

                throw new Exception("please use FindOrCreateControl(XmlNode) only when loading from the XAML file (" +
                    nameDetails + ")");
            }

            result = new TControlDef(AParsedNode, this);
            FControlList.Add(parsedNodeName, result);
            FSortedControlList.Add(FSortedControlList.Count, result);
            return result;
        }

        public TControlDef FindOrCreateControl(string AControlName, string AParentName)
        {
            TControlDef result = GetControl(AControlName);

            if (result != null)
            {
                // or should we throw an exception?
                return result;
            }

            XmlNode collectionNode = GetCorrectCollection(AControlName, AParentName);

            XmlElement newNode = FXmlDocument.CreateElement(AControlName);
            collectionNode.AppendChild(newNode);
            FXmlNodes.Add(AControlName, newNode);
            result = new TControlDef(newNode, this);
            result.parentName = AParentName;
            FControlList.Add(AControlName, result);
            FSortedControlList.Add(FSortedControlList.Count, result);
            TControlDef parentCtrl = GetControl(AParentName);

            if (parentCtrl != null)
            {
                XmlNode parentNode = parentCtrl.xmlNode;
                XmlNode controls = TXMLParser.GetChild(parentNode, "Controls");

                if (controls == null)
                {
                    controls = FXmlDocument.CreateElement("Controls");
                    parentNode.AppendChild(controls);
                }

                XmlNode element = FXmlDocument.CreateElement("Element");
                XmlAttribute attr = FXmlDocument.CreateAttribute("name");
                attr.Value = AControlName;
                element.Attributes.Append(attr);
                controls.AppendChild(element);
            }

            return result;
        }

        /// <summary>
        /// get all controls that have parentName set to the given parent control
        /// </summary>
        /// <param name="AParent"></param>
        /// <returns></returns>
        public List <TControlDef>GetChildren(TControlDef AParent)
        {
            List <TControlDef>result = new List <TControlDef>();

            foreach (TControlDef item in this.FSortedControlList.Values)
            {
                if (item.parentName == AParent.controlName)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        // only to be called by TParseXAML when loading the Events from the file
        // don't call this for creating new nodes; use FindOrCreateControl instead
        public TEventHandler AddEvent(XmlNode AParsedNode)
        {
            if (AParsedNode.Name == "base")
            {
                throw new Exception("should not parse the 'base' node this way");
            }

            string EventClass = "";
            string EventMethod = "";

            foreach (XmlAttribute attrib in AParsedNode.Attributes)
            {
                if (attrib.Name == "class")
                {
                    EventClass = attrib.Value;
                }
                else if (attrib.Name == "method")
                {
                    EventMethod = attrib.Value;
                }
            }

            TEventHandler result = new TEventHandler(AParsedNode.Name, EventClass, EventMethod);
            FEventList.Add(AParsedNode.Name, result);

            return result;
        }

        public TActionHandler AddAction(XmlNode AParsedNode)
        {
            if (AParsedNode.Name == "base")
            {
                throw new Exception("should not parse the 'base' node this way");
            }

            //actClose: {Label=&Close, ActionClick=MniFile_Close, Tooltip=Closes this window, Image=Close.ico}

            string ActionLabel = "";
            string ActionClick = "";
            string ActionTooltip = "";
            string ActionImage = "";
            string ActionId = "";

            foreach (XmlAttribute attrib in AParsedNode.Attributes)
            {
                switch (attrib.Name)
                {
                    case "Label":
                        ActionLabel = attrib.Value;
                        break;

                    case "ActionClick":
                        ActionClick = attrib.Value;
                        break;

                    case "ActionId":
                        ActionId = attrib.Value;
                        break;

                    case "Tooltip":
                        ActionTooltip = attrib.Value;
                        break;

                    case "Image":
                        ActionImage = attrib.Value;
                        break;
                }
            }

            TActionHandler result = new TActionHandler(AParsedNode, AParsedNode.Name, ActionClick, ActionId, ActionLabel, ActionTooltip, ActionImage);

            if (FActionList.ContainsKey(AParsedNode.Name))
            {
                FActionList.Remove(AParsedNode.Name);
            }

            FActionList.Add(AParsedNode.Name, result);
            return result;
        }

        public void UpdateLanguageFile()
        {
            // todo: update the .po file with any text from yaml and the source code (edited by designer)
        }

        public void HouseKeeping()
        {
        }
    }

    #region Helper Classes
    public class TEventHandler
    {
        public string eventName, eventType, eventHandler;
        public TEventHandler(string eventName, string eventType, string eventHandler)
        {
            this.eventName = eventName;
            this.eventHandler = eventHandler;
            this.eventType = eventType;
        }
    }

    public class TActionHandler
    {
        /// <summary>
        /// name of the action with leading prefix act
        /// </summary>
        public string actionName;
        public string actionClick;

        /// <summary>
        /// ActionId eg eHelp, and other default actions that are hardwired
        /// </summary>
        public string actionId;
        public string actionLabel, actionTooltip, actionImage, activeValidRowInGrid;
        public XmlNode actionNode;
        public TActionHandler(XmlNode AActionNode, string AName, string AActionClick, string AActionId, string ALabel, string ATooltip, string AImage)
        {
            actionNode = AActionNode;
            actionName = AName;
            actionClick = AActionClick;
            actionId = AActionId;
            actionLabel = ALabel;
            actionTooltip = ATooltip;
            actionImage = AImage;
        }
    }
    public class TControlDef
    {
        public TControlDef(XmlNode node, TCodeStorage ACodeStorage)
        {
            xmlNode = node;
            controlTypePrefix = GetLowerCasePrefix(xmlNode.Name);
            FCodeStorage = ACodeStorage;
        }

        public string controlTypePrefix = "";
        public string parentName = "";
        public int order = -1;
        public XmlNode xmlNode = null;
        public TCodeStorage FCodeStorage = null;
        public int rowNumber = -1;
        public int colSpan = 1;
        public int rowSpan = 1;

        /// e.g. tableLayoutPanel1.ColumnStyles.Add
        //public string otherFunctionCalls = "";
        // what about events

        public void SetAttribute(string name, string value)
        {
            TYml2Xml.SetAttribute(xmlNode, name, value);
        }

        public bool HasAttribute(string name)
        {
            return TYml2Xml.HasAttribute(xmlNode, name);
        }

        public void ClearAttribute(string name)
        {
            TYml2Xml.ClearAttribute(xmlNode, name);
        }

        public string GetAttribute(string name)
        {
            return TYml2Xml.GetAttribute(xmlNode, name);
        }

        /**
         * simple string function to return the prefix that is in lowercase letters
         */
        public static string GetLowerCasePrefix(string s)
        {
            int countLowerCase = 0;

            while (s.Substring(countLowerCase, 1).ToLower() ==
                   s.Substring(countLowerCase, 1))
            {
                countLowerCase++;
            }

            if (countLowerCase > 0)
            {
                return s.Substring(0, countLowerCase);
            }

            return "";
        }

        /// get the action for this control;
        /// this can be directly defined in attribute Action,
        /// or there is an action with the same name as the control, just different prefix
        public TActionHandler GetAction()
        {
            string ActionToPerform = this.GetAttribute("Action");

            if (!FCodeStorage.FActionList.ContainsKey(ActionToPerform))
            {
                ActionToPerform = "act" + this.controlName.Substring(this.controlTypePrefix.Length);
            }

            if (FCodeStorage.FActionList.ContainsKey(ActionToPerform))
            {
                return FCodeStorage.FActionList[ActionToPerform];
            }

            return null;
        }

        public string Label
        {
            get
            {
                // todo: if labelText is empty in the yaml file,
                // get it from petra.xml, if this is a database field
                if (HasAttribute("Label"))
                {
                    return GetAttribute("Label");
                }

                TActionHandler handler = GetAction();

                if (handler != null)
                {
                    if (handler.actionLabel.Length > 0)
                    {
                        return handler.actionLabel;
                    }
                }

                string controlName = xmlNode.Name.Substring(3);

                if (controlName.StartsWith("Detail"))
                {
                    // this is for controls that belong to the detail table by convention
                    controlName = controlName.Substring("Detail".Length);
                }

                return StringHelper.ReverseUpperCamelCase(controlName);
            }
            set
            {
                SetAttribute("Label", value);
            }
        }

        public string controlName
        {
            get
            {
                if (TYml2Xml.HasAttribute(xmlNode, "UniqueName"))
                {
                    return TYml2Xml.GetAttribute(xmlNode, "UniqueName");
                }

                return xmlNode.Name;
            }
        }

        public string controlType
        {
            get
            {
                if (xmlNode.Name.StartsWith("uco") && TYml2Xml.HasAttribute(xmlNode, "Type"))
                {
                    return TYml2Xml.GetAttribute(xmlNode, "Type");
                }

                if (xmlNode.Name.StartsWith("uco"))
                {
                    return "TUco" + xmlNode.Name.Substring(3);
                }

                return "";
            }
        }
        public int NumberChildren
        {
            get
            {
                return FCodeStorage.GetChildren(this).Count;
            }
        }
    }

    public class CtrlItemOrderComparer : IComparer <TControlDef>
    {
        /// <summary>
        /// compare two nodes; considering base nodes and depth of the node, and the order attribute
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns>+1 if node1 is greater than node2, -1 if node1 is less than node2, and 0 if they are the same or identical</returns>
        public int Compare(TControlDef node1, TControlDef node2)
        {
            return YamlItemOrderComparer.CompareNodes(node1.xmlNode, node2.xmlNode);
        }
    }
    #endregion
}