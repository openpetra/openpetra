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
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.ExtJs
{
    /*
     * This class writes code to a template
     * but it is not aware of the content and the origin of the content
     * the code generators that are loaded have the knowledge to generate proper code
     */
    public class TExtJsFormsWriter : TFormWriter
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AFormType"></param>
        public TExtJsFormsWriter(string AFormType)
        {
            TAppSettingsManager settings = new TAppSettingsManager(false);

            AddControlGenerator(new TextFieldGenerator());
            AddControlGenerator(new FieldSetGenerator());
            AddControlGenerator(new CheckboxGenerator());
            AddControlGenerator(new DateTimePickerGenerator());
            AddControlGenerator(new RadioGroupSimpleGenerator());
            AddControlGenerator(new RadioButtonGenerator());
        }

        public override string CodeFileExtension
        {
            get
            {
                return ".js";
            }
        }

        public override void SetControlProperty(string AControlName, string APropertyName, string APropertyValue)
        {
            // TODO
        }

        public override void SetEventHandlerToControl(string AControlName, string AEvent, string AEventHandlerType, string AEventHandlingMethod)
        {
            // TODO
        }

        public override void SetEventHandlerFunction(string AControlName, string AEvent, string AEventImplementation)
        {
            // TODO
        }

        public override void AddImageToResource(string AControlName, string AImageName, string AImageOrIcon)
        {
            // TODO
        }

        public override void CreateResourceFile(string AYamlFilename, string ATemplateDir)
        {
            // TODO
        }

        public override void CreateDesignerFile(string AYamlFilename, XmlNode ARootNode, string ATemplateDir)
        {
            // TODO
        }

        public override void CallControlFunction(string AControlName, string AFunctionCall)
        {
            // TODO
        }

        public override void AddContainer(string AControlName)
        {
            // TODO
        }

        public override void InitialiseDataSource(XmlNode curNode, string AControlName)
        {
            // TODO
        }

        public override bool IsUserControlTemplate
        {
            get
            {
                // TODO
                return true;
            }
        }

        private void InsertControl(TControlDef ACtrl)
        {
            IControlGenerator generator = FindControlGenerator(ACtrl);

            XmlNode controlsNode = TXMLParser.GetChild(ACtrl.xmlNode, "Controls");

            List <XmlNode>childNodes = TYml2Xml.GetChildren(controlsNode, true);

            if ((childNodes.Count > 0) && childNodes[0].Name.StartsWith("Row"))
            {
                foreach (XmlNode row in TYml2Xml.GetChildren(controlsNode, true))
                {
                    ProcessTemplate snippetRowDefinition = FTemplate.GetSnippet("ROWDEFINITION");

                    StringCollection children = TYml2Xml.GetElements(controlsNode, row.Name);

                    foreach (string child in children)
                    {
                        TControlDef childCtrl = FCodeStorage.FindOrCreateControl(child, ACtrl.controlName);
                        IControlGenerator ctrlGen = FindControlGenerator(childCtrl);
                        ProcessTemplate ctrlSnippet = ctrlGen.SetControlProperties(this, childCtrl);
                        ProcessTemplate snippetCellDefinition = FTemplate.GetSnippet("CELLDEFINITION");

                        if ((children.Count == 1) && ctrlGen is RadioGroupSimpleGenerator)
                        {
                            // do not use the ROWDEFINITION, but insert control directly
                            // this helps with aligning the label for the group radio buttons
                            snippetRowDefinition.InsertSnippet("ITEMS", ctrlSnippet, ",");
                        }
                        else
                        {
                            snippetCellDefinition.SetCodelet("COLUMNWIDTH", (1.0 / children.Count).ToString().Replace(",", "."));
                            snippetCellDefinition.InsertSnippet("ITEM", ctrlSnippet);
                            snippetRowDefinition.InsertSnippet("ITEMS", snippetCellDefinition, ",");
                        }
                    }

                    FTemplate.InsertSnippet("FORMITEMSDEFINITION", snippetRowDefinition, ",");
                }
            }
            else
            {
                StringCollection children = TYml2Xml.GetElements(ACtrl.xmlNode, "Controls");

                foreach (string child in children)
                {
                    InsertControl(FCodeStorage.FindOrCreateControl(child, ACtrl.controlName));
                }
            }
        }

        private void AddRootControl(string prefix)
        {
            TControlDef ctrl = FCodeStorage.GetRootControl(prefix);

            InsertControl(ctrl);

            // TODO insert buttons
            FTemplate.AddToCodelet("BUTTONS", "{text: 'Cancel'}");
        }

        /// based on the code model, create the code;
        /// using the code generators that have been loaded
        public override void CreateCode(TCodeStorage ACodeStorage, string AXAMLFilename, string ATemplateFile)
        {
            FCodeStorage = ACodeStorage;
            TControlGenerator.FCodeStorage = ACodeStorage;
            FTemplate = new ProcessTemplate(ATemplateFile);

            // load default header with license and copyright
            TAppSettingsManager opts = new TAppSettingsManager(false);
            string templateDir = opts.GetValue("TemplateDir", true);
            FTemplate.AddToCodelet("GPLFILEHEADER",
                ProcessTemplate.LoadEmptyFileComment(templateDir + Path.DirectorySeparatorChar + ".." +
                    Path.DirectorySeparatorChar));

            // find the first control that is a panel or groupbox or tab control
            if (FCodeStorage.HasRootControl("content"))
            {
                AddRootControl("content");
            }

            InsertCodeIntoTemplate(AXAMLFilename);
        }

        public virtual void InsertCodeIntoTemplate(string AXAMLFilename)
        {
            FTemplate.SetCodelet("FORMWIDTH", FCodeStorage.FWidth.ToString());
            FTemplate.SetCodelet("LABELWIDTH", "140");
            FTemplate.SetCodelet("FORMNAME", Path.GetFileNameWithoutExtension(AXAMLFilename));
        }
    }
}