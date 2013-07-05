//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration
{
    /// This class represents the code of a windows form in memory
    /// There are other classes that fill this model by reading from the Designer.cs file or the yaml file
    /// There are other classes that will write the code
    ///
    /// each xml element is directly linked in the FXmlNodes list, no matter which depth it is in the actual xml structure
    /// each control (menu, gui controls, etc) is linked in the FControlList
    /// all changes are done to the xml structure as well as to the FControlList and the FXmlNodes
    /// this allows easy saving of the modified xml
    public class TCodeStorage
    {
        /// contains all controls, ie also menus etc; this is a sorted list for easily finding values, but also keep them ordered
        public Dictionary <string, TControlDef>FControlList = new Dictionary <string, TControlDef>();

        /// it seems, on Mono the Dictionary gets sorted differently, therefore it is not useful for getting the RootControl etc; so we use a specific SortedList for this
        public SortedList <int, TControlDef>FSortedControlList = new SortedList <int, TControlDef>();

        /// <summary>
        /// list of event handlers
        /// </summary>
        public Dictionary <string, TEventHandler>FEventList = new Dictionary <string, TEventHandler>();

        /// <summary>
        /// list of action handlers
        /// </summary>
        public Dictionary <string, TActionHandler>FActionList = new Dictionary <string, TActionHandler>();

        /// <summary>
        /// list of report parameters
        /// </summary>
        public Dictionary <string, TReportParameter>FReportParameterList = new Dictionary <string, TReportParameter>();

        /// <summary>class to inherit from</summary>
        public string FBaseClass = "";
        /// <summary>interface to be inherited</summary>
        public string FInterfaceName = "";
        /// <summary>the type of UtilObject, eg. for edit window or other type of window</summary>
        public string FUtilObjectClass = "";
        /// <summary>the class for the generated file</summary>
        public string FClassName = "";
        /// <summary>the title for the form</summary>
        public string FFormTitle = "";
        /// <summary>the namespace for the generated file</summary>
        public string FNamespace = "";
        /// <summary>the name of the yaml file</summary>
        public string FFilename = "";
        /// <summary>the name of the manualcode file</summary>
        public string FManualCodeFilename = "";
        /// <summary>store the code for installing the handlers in this variable</summary>
        public string FEventHandler = "";
        /// <summary>store code in this variable for the event handlers</summary>
        public string FEventHandlersImplementation = "";
        /// <summary>store the code for the action handlers in this variable</summary>
        public string FActionHandlers = "";
        /// <summary>store code in this variable for the report parameters</summary>
        public string FReportParametersImplementation = "";
        /// <summary>height of the generate window</summary>
        public Int32 FHeight = 500;
        /// <summary>width of the generate window</summary>
        public Int32 FWidth = 700;
        /// <summary>list of variables that will be inserted into the template code</summary>
        public XmlNode FTemplateParameters = null;
        /// <summary>the xml representation of the yaml file</summary>
        public XmlDocument FXmlDocument = null;
        /// <summary>all xml nodes of the yaml file</summary>
        public SortedList FXmlNodes = null;
        /// <summary>the root xml node of the yaml file</summary>
        public XmlNode FRootNode = null;

        private string FManualCodeFileContent = "";

        private const string STOCK_NEW_RECORD_IMAGE = "New_Record.ico";
        private const string STOCK_DELETE_RECORD_IMAGE = "Delete_Record.ico";

        /// <summary>constructor</summary>
        public TCodeStorage(XmlDocument AXmlDocument, SortedList AXmlNodes)
        {
            FXmlDocument = AXmlDocument;
            FXmlNodes = AXmlNodes;
        }

        /// <summary>
        /// set the path to the file containing manual code for the form;
        /// insert calls to manual functions which would not be called if they do not exist
        /// </summary>
        public string ManualCodeFilename
        {
            set
            {
                FManualCodeFilename = value;
            }
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
            string pathAndName = System.IO.Path.GetDirectoryName(Path.GetFullPath(this.FFilename)).Replace("\\", "/");

            // cut off after /Client/lib
            pathAndName = pathAndName.Substring(0, pathAndName.IndexOf("ICT/Petra/Client/") + "ICT/Petra/Client".Length);

            // use only last part of namespace after Ict.Petra.Client
            ANamespaceAndClassname = ANamespaceAndClassname.Substring("Ict.Petra.Client".Length);
            pathAndName += ANamespaceAndClassname.Substring(0, ANamespaceAndClassname.LastIndexOf(".")).Replace(".", "/");

            // get the file name without TFrm prefix
            string filename = "/" + ANamespaceAndClassname.Substring(ANamespaceAndClassname.LastIndexOf(".") + 1 + 4);
            string alternativeFileName = filename;

            if (!System.IO.File.Exists(pathAndName + filename + ".cs"))
            {
                filename += "-generated";
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
                    pathAndName + alternativeFileName + ".cs");
            }

            string ManualCodeFile = pathAndName + filename + ".ManualCode.cs";

            if (!System.IO.File.Exists(ManualCodeFile))
            {
                ManualCodeFile = ManualCodeFile.Replace("-generated.cs", ".ManualCode.cs");
            }

            if (System.IO.File.Exists(ManualCodeFile))
            {
                System.IO.StreamReader r = new System.IO.StreamReader(ManualCodeFile);
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
                    else if (!ctrl.controlName.StartsWith("Empty"))
                    {
                        result.Add(ctrl);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// get a control from the list of controls defined in the yaml file
        /// </summary>
        /// <param name="AControlName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// find the appropriate root control for this window.
        /// can be a tab page container, a group panel, a user control or a simple panel
        /// </summary>
        /// <param name="APrefix"></param>
        /// <returns></returns>
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

        /// <summary>
        /// which node does this control belong to?
        /// </summary>
        /// <param name="AControlName"></param>
        /// <param name="AParentName"></param>
        /// <returns></returns>
        public XmlNode GetCorrectCollection(string AControlName, string AParentName)
        {
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

        /// only to be called by TParseXAML when loading the controls from the file
        /// don't call this for creating new nodes; use FindOrCreateControl instead
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

        /// <summary>
        /// get the definition of the control, if it has not been loaded yet from yaml then do it now
        /// </summary>
        /// <param name="AControlName"></param>
        /// <param name="AParentName"></param>
        /// <returns></returns>
        public TControlDef FindOrCreateControl(string AControlName, string AParentName)
        {
            TControlDef result = GetControl(AControlName);

            if (result != null)
            {
                // or should we throw an exception?
                return result;
            }

            if (AControlName == "pnlEmpty")
            {
                int countEmpty = 1;

                foreach (string name in FControlList.Keys)
                {
                    if (name.StartsWith("pnlEmpty"))
                    {
                        countEmpty++;
                    }
                }

                AControlName += countEmpty.ToString();
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

            if ((parentCtrl != null) && !AControlName.StartsWith("pnlEmpty"))
            {
                XmlNode parentNode = parentCtrl.xmlNode;
                XmlNode controls = TXMLParser.GetChild(parentNode, "Controls");

                if (controls == null)
                {
                    controls = FXmlDocument.CreateElement("Controls");
                    parentNode.AppendChild(controls);
                }

                XmlNode element = FXmlDocument.CreateElement(TYml2Xml.XMLLIST);
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

        /// only to be called by TParseXAML when loading the Events from the file
        /// don't call this for creating new nodes; use FindOrCreateControl instead
        public TEventHandler AddEvent(XmlNode AParsedNode)
        {
            if (AParsedNode.Name == "base")
            {
                throw new Exception("should not parse the 'base' node this way");
            }

            string EventClass = TYml2Xml.GetAttribute(AParsedNode, "class");
            string EventMethod = TYml2Xml.GetAttribute(AParsedNode, "method");

            TEventHandler result = new TEventHandler(AParsedNode.Name, EventClass, EventMethod);
            FEventList.Add(AParsedNode.Name, result);

            return result;
        }

        /// <summary>
        /// load an action from yaml
        /// </summary>
        /// <param name="AParsedNode"></param>
        /// <returns></returns>
        public TActionHandler AddAction(XmlNode AParsedNode)
        {
            if (AParsedNode.Name == "base")
            {
                throw new Exception("should not parse the 'base' node this way");
            }

            //actClose: {Label=&Close, ActionClick=MniFile_Close, Tooltip=Closes this window, Image=Close.ico}

            string ActionLabel = TYml2Xml.GetAttribute(AParsedNode, "Label");
            string ActionClick = TYml2Xml.GetAttribute(AParsedNode, "ActionClick");
            string ActionTooltip = TYml2Xml.GetAttribute(AParsedNode, "Tooltip");
            string ActionImage = TYml2Xml.GetAttribute(AParsedNode, "Image");
            string ActionId = TYml2Xml.GetAttribute(AParsedNode, "ActionId");

            if (AParsedNode.Name == "actNew")
            {
                if (ActionImage.ToLower() == "none")
                {
                    ActionImage = String.Empty;
                }
                else if (ActionImage == String.Empty)
                {
                    ActionImage = STOCK_NEW_RECORD_IMAGE;
                }
            }
            else if (AParsedNode.Name == "actDelete")
            {
                if (ActionImage.ToLower() == "none")
                {
                    ActionImage = String.Empty;
                }
                else if (ActionImage == String.Empty)
                {
                    ActionImage = STOCK_DELETE_RECORD_IMAGE;
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

        /// <summary>
        /// load a report parameter from yaml
        /// </summary>
        /// <param name="AParsedNode"></param>
        /// <param name="AColumnFunctionClassName"></param>
        /// <returns></returns>
        public TReportParameter AddReportParameter(XmlNode AParsedNode, string AColumnFunctionClassName)
        {
            if (AParsedNode.Name == "base")
            {
                throw new Exception("should not parse the 'base' node this way");
            }

            string ReportDescription = TYml2Xml.GetAttribute(AParsedNode, "Name");
            string ReportParameter = TYml2Xml.GetAttribute(AParsedNode, "Parameter");

            TReportParameter result = new TReportParameter(AColumnFunctionClassName, ReportDescription, ReportParameter);
            FReportParameterList.Add(AParsedNode.Name, result);

            return result;
        }
    }

    #region Helper Classes
    /// <summary>
    /// report parameter, used for the report system
    /// </summary>
    public class TReportParameter
    {
        /// <summary>classname for function</summary>
        public string columnFunctionClassName;
        /// <summary>description</summary>
        public string functionDescription;
        /// <summary>parameters</summary>
        public string functionParameters;
        /// <summary>constructor</summary>
        public TReportParameter(string AColumnFunctionClassName, string AFunctionDescription, string AFunctionParameters)
        {
            this.columnFunctionClassName = AColumnFunctionClassName;
            this.functionDescription = AFunctionDescription;
            this.functionParameters = AFunctionParameters;
        }
    }

    /// <summary>
    /// event handler
    /// </summary>
    public class TEventHandler
    {
        /// <summary>name</summary>
        public string eventName;
        /// <summary>type</summary>
        public string eventType;
        /// <summary>handler</summary>
        public string eventHandler;
        /// <summary>constructor</summary>
        public TEventHandler(string eventName, string eventType, string eventHandler)
        {
            this.eventName = eventName;
            this.eventHandler = eventHandler;
            this.eventType = eventType;
        }
    }

    /// <summary>action handler</summary>
    public class TActionHandler
    {
        /// <summary>
        /// name of the action with leading prefix act
        /// </summary>
        public string actionName;
        /// <summary>name of the function to be called when action is clicked</summary>
        public string actionClick;

        /// <summary>
        /// ActionId eg eHelp, and other default actions that are hardwired
        /// </summary>
        public string actionId;
        /// <summary>label to be written on the button or menu item</summary>
        public string actionLabel;
        /// <summary>tooltip to be shown</summary>
        public string actionTooltip;
        /// <summary>image to be displayed on the button or menu item</summary>
        public string actionImage;
        /// <summary>reference to the yaml file</summary>
        public XmlNode actionNode;
        /// <summary>constructor</summary>
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

    /// <summary>
    /// definition of a control to be embedded in the window
    /// </summary>
    public class TControlDef
    {
        /// <summary>
        /// Name of Panel that contains Buttons that are related to a Grid (such as 'Add' and 'Delete')
        /// </summary>
        public const string STR_BUTTON_PANEL_NAME = "pnlButtons";
        
        /// <summary>
        /// Name of Panel that contains Buttons that are related to a Grid (such as 'Add' and 'Delete')
        /// </summary>        
        public const string STR_DETAIL_BUTTON_PANEL_NAME = "pnlDetailButtons";

        /// <summary>
        /// Name of Panel that contains Buttons that are related to a Grid (such as 'Add' and 'Delete')
        /// </summary>        
        public const string STR_INNER_BUTTON_PANEL_NAME = "pnlButtonsInner";
        
        /// <summary>
        /// construtor
        /// </summary>
        /// <param name="node"></param>
        /// <param name="ACodeStorage"></param>
        public TControlDef(XmlNode node, TCodeStorage ACodeStorage)
        {
            xmlNode = node;
            controlTypePrefix = GetLowerCasePrefix(xmlNode.Name);
            FCodeStorage = ACodeStorage;
        }

        /// <summary>prefix that identifies the type of the control, eg. btn</summary>
        public string controlTypePrefix = "";
        /// <summary>the generator for this type of control, eg ButtonGenerator</summary>
        public IControlGenerator controlGenerator = null;
        /// <summary>name of the parent control</summary>
        public string parentName = "";
        /// <summary>order of the control among the other controls</summary>
        public int order = -1;
        /// <summary>the node in the yaml file</summary>
        public XmlNode xmlNode = null;
        /// <summary>control belongs to this storage</summary>
        public TCodeStorage FCodeStorage = null;
        /// <summary>the children of this control</summary>
        public List <TControlDef>Children = new List <TControlDef>();
        /// <summary>in the grid layout, the row number where this control appears</summary>
        public int rowNumber = -1;
        /// <summary>if this controls spans several columns in the grid layout</summary>
        public int colSpan = 1;
        /// <summary>if this controls spans several rows in the grid layout</summary>
        public int rowSpan = 1;
        /// <summary>if this controls spans several columns in the grid layout, and has no label, this will include the label columns as well</summary>
        public int colSpanWithLabel = 1;
        /// <summary>if this controls has no label, we need one more column</summary>
        public bool hasLabel = true;

        /// <summary>
        /// write attribute value to the yaml
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetAttribute(string name, string value)
        {
            TYml2Xml.SetAttribute(xmlNode, name, value);
        }

        /// <summary>
        /// check for attribute in the yaml
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasAttribute(string name)
        {
            return TYml2Xml.HasAttribute(xmlNode, name);
        }

        /// <summary>
        /// remove attribute
        /// </summary>
        /// <param name="name"></param>
        public void ClearAttribute(string name)
        {
            TYml2Xml.ClearAttribute(xmlNode, name);
        }

        /// <summary>
        /// get value of attribute from yaml
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetAttribute(string name)
        {
            return TYml2Xml.GetAttribute(xmlNode, name);
        }

        /// <summary>
        /// get value of attribute from yaml
        /// </summary>
        public string GetAttribute(string name, string ADefaultValue)
        {
            if (!TYml2Xml.HasAttribute(xmlNode, name))
            {
                return ADefaultValue;
            }

            return TYml2Xml.GetAttribute(xmlNode, name);
        }

        /// simple string function to return the prefix that is in lowercase letters
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

        /// <summary>
        /// property for the label to be displayed before the control or on the button
        /// </summary>
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

                if (controlName.StartsWith("Detail") && (controlName.Length > "Detail".Length) && char.IsUpper(controlName["Detail".Length]))
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

        /// <summary>
        /// get the width
        /// </summary>
        public int Width
        {
            get
            {
                if (HasAttribute("Width"))
                {
                    return Convert.ToInt32(GetAttribute("Width"));
                }
                else if (HasAttribute("DefaultWidth"))
                {
                    return Convert.ToInt32(GetAttribute("DefaultWidth"));
                }

                return 20;
            }
        }

        /// <summary>
        /// get the height
        /// </summary>
        public int Height
        {
            get
            {
                if (HasAttribute("Height"))
                {
                    return Convert.ToInt32(GetAttribute("Height"));
                }
                else if (HasAttribute("DefaultHeight"))
                {
                    return Convert.ToInt32(GetAttribute("DefaultHeight"));
                }

                return 20;
            }
        }

        /// <summary>
        /// property for the name of the control, including the prefix
        /// </summary>
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

        /// <summary>
        /// the type of the control
        /// </summary>
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

        /// <summary>
        /// get the number of children of this control
        /// </summary>
        public int NumberChildren
        {
            get
            {
                return FCodeStorage.GetChildren(this).Count;
            }
        }
        
        /// <summary>
        /// True if the Control is a Grid Button Panel.
        /// </summary>
        public bool IsGridButtonPanel
        {
            get
            {
TLogging.Log("Control: " + this.controlName + "  is IsGridButtonPanel: " + 
                       (this.controlName == STR_BUTTON_PANEL_NAME 
                              || this.controlName == STR_DETAIL_BUTTON_PANEL_NAME 
                              || this.controlName == STR_INNER_BUTTON_PANEL_NAME).ToString()     );
                return (this.controlName == STR_BUTTON_PANEL_NAME 
                    || this.controlName == STR_DETAIL_BUTTON_PANEL_NAME
                    || this.controlName == STR_INNER_BUTTON_PANEL_NAME);
            }
        }
        
        /// <summary>
        /// True if the Control is a horizontal Grid Button Panel.
        /// </summary>
        public bool IsHorizontalGridButtonPanel
        {
            get
            {
                return IsGridButtonPanel && 
                    (HasAttribute("ControlsOrientation"))
                        && (GetAttribute("ControlsOrientation").ToLower() == "horizontal");

            }
        }
        
        /// <summary>
        /// True if the Control is placed on a Grid Button Panel.
        /// </summary>
        public bool IsOnGridButtonPanel
        {
            get
            {
                return ((this.parentName == String.Empty)
                        || (this.parentName == STR_INNER_BUTTON_PANEL_NAME));
            }
        }
        
        /// <summary>
        /// True if the Control is placed on a horizontal Grid Button Panel.
        /// </summary>
        public bool IsOnHorizontalGridButtonPanel
        {
            get
            {
                TControlDef ParentControl = FCodeStorage.GetControl(this.parentName);
TLogging.Log("IsOnHorizontalGridButtonPanel:   Control: " + this.controlName + "; Parent: " + this.parentName);
TLogging.Log("IsOnHorizontalGridButtonPanel result: " + (IsOnGridButtonPanel 
                    && ((ParentControl == null) 
                    || (ParentControl.HasAttribute("ControlsOrientation"))
                        && (ParentControl.GetAttribute("ControlsOrientation").ToLower() == "horizontal"))).ToString());
                return IsOnGridButtonPanel 
                    && ((ParentControl == null) 
                    || (ParentControl.HasAttribute("ControlsOrientation"))
                        && (ParentControl.GetAttribute("ControlsOrientation").ToLower() == "horizontal"));             
            }
        }        
    }

    /// <summary>
    /// for sorting the control items
    /// </summary>
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