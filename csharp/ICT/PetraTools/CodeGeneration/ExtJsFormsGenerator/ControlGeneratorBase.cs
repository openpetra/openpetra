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
using System.Xml;
using System.Collections.Specialized;
using System.Collections.Generic;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;
using Ict.Common.IO;
using Ict.Common;

namespace Ict.Tools.CodeGeneration.ExtJs
{
    public class TControlGenerator : IControlGenerator
    {
        public string FPrefix;
        public string FControlType;
        public bool FAutoSize = false;
        public bool FLocation = true;
        public bool FGenerateLabel = true;

        /// the readonly property eg of Textbox still allows tooltips and copy to clipboard, which enable=false would not allow
        public bool FHasReadOnlyProperty = false;
        public bool FAddControlToContainer = true;
        public bool FRequiresChildren = false;
        public Int32 FDefaultWidth = 300;
        public Int32 FDefaultHeight = 28;
        public string FControlDefinitionSnippetName = "ITEMDEFINITION";

        public static TCodeStorage FCodeStorage;

        public TControlGenerator(string APrefix, string AControlType)
        {
            FPrefix = APrefix;
            FControlType = AControlType;
        }

        /// <summary>
        /// should this control only be created if there are children controls? eg toolbar
        /// </summary>
        public bool RequiresChildren
        {
            get
            {
                return FRequiresChildren;
            }
        }

        public bool GenerateLabel(TControlDef ctrl)
        {
            // not required for ext.js
            return false;
        }

        public String ControlType
        {
            get
            {
                return FControlType;
            }
            set
            {
                FControlType = value;
            }
        }

        /// <summary>
        /// the name of the snippet in the template for Readcontrols and setcontrols, in captial letters
        /// </summary>
        public string TemplateSnippetName
        {
            get
            {
                return "";
            }
            set
            {
            }
        }

        public bool AddControlToContainer
        {
            get
            {
                return FAddControlToContainer;
            }
            set
            {
                FAddControlToContainer = value;
            }
        }

        /// use the prefix to see if the control matches the current class
        public bool SimplePrefixMatch(XmlNode curNode)
        {
            return curNode.Name.StartsWith(FPrefix);
        }

        // overwrite for more complicated matches,
        // if the same prefix is used for more than one control type
        // e.g. txt
        public virtual bool ControlFitsNode(XmlNode curNode)
        {
            return SimplePrefixMatch(curNode);
        }

        public virtual void GenerateDeclaration(TFormWriter writer, TControlDef ctrl)
        {
            // TODO
        }

        /// <summary>
        /// fill in the attributes for the control
        /// </summary>
        /// <param name="ASnippetControl"></param>
        /// <param name="ctrl"></param>
        public virtual ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ACtrl)
        {
            ProcessTemplate snippetControl = writer.FTemplate.GetSnippet(FControlDefinitionSnippetName);

            snippetControl.SetCodelet("ITEMNAME", ACtrl.controlName);
            snippetControl.SetCodelet("ITEMID", ACtrl.controlName);
            snippetControl.SetCodelet("XTYPE", FControlType);

            if (ACtrl.HasAttribute("xtype"))
            {
                snippetControl.SetCodelet("XTYPE", ACtrl.GetAttribute("xtype"));
            }

            if (ACtrl.HasAttribute("maxLength"))
            {
                snippetControl.SetCodelet("MAXLENGTH", ACtrl.GetAttribute("maxLength"));
            }

            if (ACtrl.HasAttribute("minLength"))
            {
                snippetControl.SetCodelet("MINLENGTH", ACtrl.GetAttribute("minLength"));
            }

            ((TExtJsFormsWriter)writer).AddResourceString(snippetControl, "LABEL", ACtrl, ACtrl.Label);
            ((TExtJsFormsWriter)writer).AddResourceString(snippetControl, "HELP", ACtrl, ACtrl.GetAttribute("Help"));

            if (ACtrl.HasAttribute("allowBlank") && (ACtrl.GetAttribute("allowBlank") == "true"))
            {
                snippetControl.SetCodelet("ALLOWBLANK", "true");
            }

            if (ACtrl.HasAttribute("inputType"))
            {
                snippetControl.SetCodelet("INPUTTYPE", ACtrl.GetAttribute("inputType"));
            }

            if (ACtrl.HasAttribute("vtype"))
            {
                snippetControl.SetCodelet("VTYPE", ACtrl.GetAttribute("vtype"));
            }

            if (ACtrl.HasAttribute("DateFormat"))
            {
                snippetControl.SetCodelet("DATEFORMAT", ACtrl.GetAttribute("DateFormat"));
            }

            if (ACtrl.HasAttribute("ShowToday"))
            {
                snippetControl.SetCodelet("SHOWTODAY", ACtrl.GetAttribute("ShowToday"));
            }

            if (ACtrl.HasAttribute("MinDateYear"))
            {
                snippetControl.SetCodelet("MINYEAR", ACtrl.GetAttribute("MinDateYear"));
                snippetControl.SetCodelet("MINMONTH", ACtrl.GetAttribute("MinDateMonth"));
                snippetControl.SetCodelet("MINDAY", ACtrl.GetAttribute("MinDateDay"));
            }

            if (ACtrl.HasAttribute("MaxDateYear"))
            {
                snippetControl.SetCodelet("MAXYEAR", ACtrl.GetAttribute("MaxDateYear"));
                snippetControl.SetCodelet("MAXMONTH", ACtrl.GetAttribute("MaxDateMonth"));
                snippetControl.SetCodelet("MAXDAY", ACtrl.GetAttribute("MaxDateDay"));
            }

            if (ACtrl.HasAttribute("DefaultYear"))
            {
                snippetControl.SetCodelet("DEFAULTYEAR", ACtrl.GetAttribute("DefaultYear"));
                snippetControl.SetCodelet("DEFAULTMONTH", ACtrl.GetAttribute("DefaultMonth"));
                snippetControl.SetCodelet("DEFAULTDAY", ACtrl.GetAttribute("DefaultDay"));
            }

            if (ACtrl.HasAttribute("otherPasswordField"))
            {
                snippetControl.SetCodelet("OTHERPASSWORDFIELD", ACtrl.GetAttribute("otherPasswordField"));
                writer.FTemplate.SetCodelet("PASSWORDTWICE", "yes");
                ((TExtJsFormsWriter)writer).AddResourceString(snippetControl, "strErrorPasswordLength", null,
                    ACtrl.GetAttribute("ValidationErrorLength"));
                ((TExtJsFormsWriter)writer).AddResourceString(snippetControl, "strErrorPasswordNoMatch", null,
                    ACtrl.GetAttribute("ValidationErrorMatching"));
            }

            if (ACtrl.GetAttribute("vtype") == "forcetick")
            {
                writer.FTemplate.SetCodelet("FORCECHECKBOX", "true");
                ((TExtJsFormsWriter)writer).AddResourceString(snippetControl, "strErrorCheckboxRequired", null,
                    ACtrl.GetAttribute("ErrorCheckboxRequired"));
            }

            if (FDefaultWidth != -1)
            {
                snippetControl.SetCodelet("WIDTH", FDefaultWidth.ToString());
            }

            snippetControl.SetCodelet("CUSTOMATTRIBUTES", "");

            return snippetControl;
        }

        public virtual void OnChangeDataType(TFormWriter writer, XmlNode curNode)
        {
            OnChangeDataType(writer, curNode, curNode.Name);
        }

        public virtual void OnChangeDataType(TFormWriter writer, XmlNode curNode, string controlName)
        {
            // TODO
        }

        // e.g. used for controls on Reports (readparameter, etc)
        public virtual void ApplyDerivedFunctionality(TFormWriter writer, XmlNode curNode)
        {
        }
    }
}