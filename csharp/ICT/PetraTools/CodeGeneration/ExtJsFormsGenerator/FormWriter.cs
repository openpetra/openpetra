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
        private string FFormName = "";

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
            AddControlGenerator(new GroupBoxGenerator());
            AddControlGenerator(new LabelGenerator());
            AddControlGenerator(new CompositeGenerator());
            AddControlGenerator(new ButtonGenerator());
            AddControlGenerator(new ComboboxGenerator());
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

        private ProcessTemplate FLanguageFileTemplate = null;

        public void AddResourceString(ProcessTemplate ACtrlSnippet, string APlaceHolder, TControlDef ACtrl, string AText)
        {
            string strName;

            if (ACtrl == null)
            {
                strName = APlaceHolder;
            }
            else
            {
                strName = ACtrl.controlName + APlaceHolder;
            }

            ACtrlSnippet.SetCodelet(APlaceHolder, strName);
            FTemplate.AddToCodelet("RESOURCESTRINGS", strName + ":'" + AText + "'," + Environment.NewLine);

            // write to app-lang-en.js file
            FLanguageFileTemplate.AddToCodelet("RESOURCESTRINGS", strName + ":'" + AText + "'," + Environment.NewLine);
        }

        /// <summary>
        /// default anchor
        /// </summary>
        private static string ANCHOR_DEFAULT_COLUMN = "95%";

        /// <summary>
        /// anchor used for column span, just one column in row
        /// </summary>
        private static string ANCHOR_SINGLE_COLUMN = "97.5%";

        /// <summary>
        /// anchor used for column with no label
        /// </summary>
        private static string ANCHOR_HIDDEN_LABEL = "96.25%";

        private static void LayoutCellInForm(TControlDef ACtrl,
            Int32 AChildrenCount,
            ProcessTemplate ACtrlSnippet,
            ProcessTemplate ASnippetCellDefinition)
        {
            if (ACtrl.HasAttribute("labelWidth"))
            {
                ASnippetCellDefinition.SetCodelet("LABELWIDTH", ACtrl.GetAttribute("labelWidth"));
            }

            if (ACtrl.HasAttribute("columnWidth"))
            {
                ASnippetCellDefinition.SetCodelet("COLUMNWIDTH", ACtrl.GetAttribute("columnWidth").Replace(",", "."));
            }
            else
            {
                ASnippetCellDefinition.SetCodelet("COLUMNWIDTH", (1.0 / AChildrenCount).ToString().Replace(",", "."));
            }

            string Anchor = ANCHOR_DEFAULT_COLUMN;

            if (AChildrenCount == 1)
            {
                Anchor = ANCHOR_SINGLE_COLUMN;
            }

            if (ACtrl.HasAttribute("columnWidth"))
            {
                Anchor = "94%";
            }

            if (ACtrl.GetAttribute("hideLabel") == "true")
            {
                ACtrlSnippet.SetCodelet("HIDELABEL", "true");
                Anchor = ANCHOR_HIDDEN_LABEL;
            }

            ACtrlSnippet.SetCodelet("ANCHOR", Anchor);
        }

        public static void InsertControl(TControlDef ACtrl, ProcessTemplate ATemplate, string AItemsPlaceholder, TFormWriter AWriter)
        {
            XmlNode controlsNode = TXMLParser.GetChild(ACtrl.xmlNode, "Controls");

            List <XmlNode>childNodes = TYml2Xml.GetChildren(controlsNode, true);

            if ((childNodes.Count > 0) && childNodes[0].Name.StartsWith("Row"))
            {
                foreach (XmlNode row in TYml2Xml.GetChildren(controlsNode, true))
                {
                    ProcessTemplate snippetRowDefinition = AWriter.FTemplate.GetSnippet("ROWDEFINITION");

                    StringCollection children = TYml2Xml.GetElements(controlsNode, row.Name);

                    foreach (string child in children)
                    {
                        TControlDef childCtrl = AWriter.FCodeStorage.FindOrCreateControl(child, ACtrl.controlName);
                        IControlGenerator ctrlGen = AWriter.FindControlGenerator(childCtrl);
                        ProcessTemplate ctrlSnippet = ctrlGen.SetControlProperties(AWriter, childCtrl);
                        ProcessTemplate snippetCellDefinition = AWriter.FTemplate.GetSnippet("CELLDEFINITION");

                        LayoutCellInForm(childCtrl, children.Count, ctrlSnippet, snippetCellDefinition);

                        if ((children.Count == 1) && ctrlGen is RadioGroupSimpleGenerator)
                        {
                            // do not use the ROWDEFINITION, but insert control directly
                            // this helps with aligning the label for the group radio buttons
                            snippetRowDefinition.InsertSnippet("ITEMS", ctrlSnippet, ",");
                        }
                        else
                        {
                            snippetCellDefinition.InsertSnippet("ITEM", ctrlSnippet);
                            snippetRowDefinition.InsertSnippet("ITEMS", snippetCellDefinition, ",");
                        }
                    }

                    ATemplate.InsertSnippet(AItemsPlaceholder, snippetRowDefinition, ",");
                }
            }
            else
            {
                StringCollection children = TYml2Xml.GetElements(ACtrl.xmlNode, "Controls");

                foreach (string child in children)
                {
                    TControlDef childCtrl = AWriter.FCodeStorage.FindOrCreateControl(child, ACtrl.controlName);
                    IControlGenerator ctrlGen = AWriter.FindControlGenerator(childCtrl);

                    if (ctrlGen is FieldSetGenerator)
                    {
                        InsertControl(AWriter.FCodeStorage.FindOrCreateControl(child, ACtrl.controlName), ATemplate, AItemsPlaceholder, AWriter);
                    }
                    else
                    {
                        ProcessTemplate ctrlSnippet = ctrlGen.SetControlProperties(AWriter, childCtrl);
                        ProcessTemplate snippetCellDefinition = AWriter.FTemplate.GetSnippet("CELLDEFINITION");

                        LayoutCellInForm(childCtrl, -1, ctrlSnippet, snippetCellDefinition);

                        ATemplate.InsertSnippet(AItemsPlaceholder, ctrlSnippet, ",");
                    }
                }
            }
        }

        public static void InsertButtons(TControlDef ACtrl, ProcessTemplate ATemplate, string AItemsPlaceholder, TFormWriter AWriter)
        {
            XmlNode controlsNode = TXMLParser.GetChild(ACtrl.xmlNode, "Buttons");

            List <XmlNode>childNodes = TYml2Xml.GetChildren(controlsNode, true);

            StringCollection children = TYml2Xml.GetElements(ACtrl.xmlNode, "Buttons");

            foreach (string child in children)
            {
                TControlDef childCtrl = AWriter.FCodeStorage.FindOrCreateControl(child, ACtrl.controlName);
                IControlGenerator ctrlGen = AWriter.FindControlGenerator(childCtrl);

                ProcessTemplate ctrlSnippet = ctrlGen.SetControlProperties(AWriter, childCtrl);
                ProcessTemplate snippetCellDefinition = AWriter.FTemplate.GetSnippet("CELLDEFINITION");

                LayoutCellInForm(childCtrl, -1, ctrlSnippet, snippetCellDefinition);

                ATemplate.InsertSnippet(AItemsPlaceholder, ctrlSnippet, ",");
            }
        }

        private void AddRootControl(string prefix)
        {
            TControlDef ctrl = FCodeStorage.GetRootControl(prefix);

            AddResourceString(FTemplate, "FORMCAPTION", ctrl, FCodeStorage.FFormTitle);

            InsertControl(ctrl, FTemplate, "FORMITEMSDEFINITION", this);

            InsertButtons(ctrl, FTemplate, "BUTTONS", this);
        }

        /// based on the code model, create the code;
        /// using the code generators that have been loaded
        public override void CreateCode(TCodeStorage ACodeStorage, string AXAMLFilename, string ATemplateFile)
        {
            FCodeStorage = ACodeStorage;
            TControlGenerator.FCodeStorage = ACodeStorage;
            FTemplate = new ProcessTemplate(ATemplateFile);
            FFormName = Path.GetFileNameWithoutExtension(AXAMLFilename);

            // load default header with license and copyright
            TAppSettingsManager opts = new TAppSettingsManager(false);
            string templateDir = opts.GetValue("TemplateDir", true);
            FTemplate.AddToCodelet("GPLFILEHEADER",
                ProcessTemplate.LoadEmptyFileComment(templateDir + Path.DirectorySeparatorChar + ".." +
                    Path.DirectorySeparatorChar));

            FLanguageFileTemplate = FTemplate.GetSnippet("LANGUAGEFILE");

            // find the first control that is a panel or groupbox or tab control
            if (FCodeStorage.HasRootControl("content"))
            {
                AddRootControl("content");
            }

            InsertCodeIntoTemplate(AXAMLFilename);

            string languagefilepath = Path.GetDirectoryName(AXAMLFilename) + Path.DirectorySeparatorChar +
                                      Path.GetFileNameWithoutExtension(AXAMLFilename) + "-lang-template.js";
            File.WriteAllText(languagefilepath, FLanguageFileTemplate.FinishWriting(true));
        }

        public virtual void InsertCodeIntoTemplate(string AXAMLFilename)
        {
            FTemplate.SetCodelet("FORMWIDTH", FCodeStorage.FWidth.ToString());
            FTemplate.SetCodelet("LABELWIDTH", "140");
            FTemplate.SetCodelet("FORMNAME", FFormName);
            FLanguageFileTemplate.SetCodelet("FORMNAME", FFormName);
        }
    }
}