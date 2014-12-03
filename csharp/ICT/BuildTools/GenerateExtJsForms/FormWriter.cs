//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2014 by OM International
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
    /// <summary>
    /// This class writes code to a template
    /// but it is not aware of the content and the origin of the content
    /// the code generators that are loaded have the knowledge to generate proper code
    /// </summary>
    public class TExtJsFormsWriter : TFormWriter
    {
        private string FFormName = "";

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AFormType"></param>
        public TExtJsFormsWriter(string AFormType)
        {
            new TAppSettingsManager(false);

            AddControlGenerator(new TextFieldGenerator());
            AddControlGenerator(new TextAreaGenerator());
            AddControlGenerator(new FieldSetGenerator());
            AddControlGenerator(new CheckboxGenerator());
            AddControlGenerator(new DateTimePickerGenerator());
            AddControlGenerator(new RadioGroupSimpleGenerator());
            AddControlGenerator(new RadioGroupComplexGenerator());
            AddControlGenerator(new RadioButtonGenerator());
            AddControlGenerator(new GroupBoxGenerator());
            AddControlGenerator(new LabelGenerator());
            AddControlGenerator(new CompositeGenerator());
            AddControlGenerator(new ButtonGenerator());
            AddControlGenerator(new ComboboxGenerator());
            AddControlGenerator(new AssistantGenerator());
            AddControlGenerator(new AssistantPageGenerator());
            AddControlGenerator(new InlineGenerator());
            AddControlGenerator(new UploadGenerator());
            AddControlGenerator(new GridGenerator());
            AddControlGenerator(new HiddenFieldGenerator());
        }

        /// <summary>
        /// the file extension for the result file
        /// </summary>
        public override string CodeFileExtension
        {
            get
            {
                return ".js";
            }
        }

        /// <summary>
        /// not needed for ext.js
        /// </summary>
        public override void SetControlProperty(string AControlName, string APropertyName, string APropertyValue, bool ACreateTranslationForLabel)
        {
            // not implemented
        }

        /// <summary>
        /// not needed for ext.js
        /// </summary>
        public override void SetEventHandlerToControl(string AControlName, string AEvent, string AEventHandlerType, string AEventHandlingMethod)
        {
            // not implemented
        }

        /// <summary>
        /// not needed for ext.js
        /// </summary>
        public override void SetEventHandlerFunction(string AControlName, string AEvent, string AEventImplementation)
        {
            // not implemented
        }

        /// <summary>
        /// not needed for ext.js
        /// </summary>
        public override void AddImageToResource(string AControlName, string AImageName, string AImageOrIcon)
        {
            // not implemented
        }

        /// <summary>
        /// not needed for ext.js
        /// </summary>
        public override void CreateResourceFile(string ATemplateDir)
        {
            // not implemented
        }

        /// <summary>
        /// not needed for ext.js
        /// </summary>
        public override void CreateDesignerFile(XmlNode ARootNode, string ATemplateDir)
        {
            // not implemented
        }

        /// <summary>get the name of the destination generated file</summary>
        public override string CalculateDestinationFilename()
        {
            string relativePath = Path.GetDirectoryName(Path.GetFullPath(YamlFilename)).
                                  Substring(Path.GetFullPath(TAppSettingsManager.GetValue("ymlroot")).Length);

            string generatedFilesPath = TAppSettingsManager.GetValue("deliveryPath") +
                                        System.IO.Path.DirectorySeparatorChar +
                                        relativePath +
                                        System.IO.Path.DirectorySeparatorChar +
                                        "gen";

            if (!Directory.Exists(generatedFilesPath))
            {
                Directory.CreateDirectory(generatedFilesPath);
            }

            return generatedFilesPath +
                   System.IO.Path.DirectorySeparatorChar +
                   System.IO.Path.GetFileNameWithoutExtension(YamlFilename) +
                   this.CodeFileExtension;
        }

        /// <summary>get the name of the file with the manual code</summary>
        public override string CalculateManualCodeFilename()
        {
            return System.IO.Path.GetDirectoryName(YamlFilename) +
                   System.IO.Path.DirectorySeparatorChar +
                   System.IO.Path.GetFileNameWithoutExtension(YamlFilename) +
                   ".ManualCode" + this.CodeFileExtension;
        }

        /// <summary>
        /// not needed for ext.js
        /// </summary>
        public override void CallControlFunction(string AControlName, string AFunctionCall)
        {
            // not implemented
        }

        /// <summary>
        /// not needed for ext.js
        /// </summary>
        public override void AddContainer(string AControlName)
        {
            // not implemented
        }

        /// <summary>
        /// not needed for ext.js
        /// </summary>
        public override void InitialiseDataSource(XmlNode curNode, string AControlName)
        {
            // not implemented
        }

        /// <summary>
        /// not needed for ext.js
        /// </summary>
        public override bool IsUserControlTemplate
        {
            get
            {
                // not implemented
                return true;
            }
        }

        private ProcessTemplate FLanguageFileTemplate = null;

        /// <summary>
        /// add a string that is translatable
        /// </summary>
        /// <param name="ACtrlSnippet"></param>
        /// <param name="APlaceHolder"></param>
        /// <param name="ACtrl"></param>
        /// <param name="AText"></param>
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

        /// <summary>
        /// main function for creating a control
        /// </summary>
        /// <param name="ACtrl"></param>
        /// <param name="ATemplate"></param>
        /// <param name="AItemsPlaceholder"></param>
        /// <param name="ANodeName"></param>
        /// <param name="AWriter"></param>
        public static void InsertControl(TControlDef ACtrl,
            ProcessTemplate ATemplate,
            string AItemsPlaceholder,
            string ANodeName,
            TFormWriter AWriter)
        {
            XmlNode controlsNode = TXMLParser.GetChild(ACtrl.xmlNode, ANodeName);

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
                foreach (XmlNode childNode in childNodes)
                {
                    string child = TYml2Xml.GetElementName(childNode);
                    TControlDef childCtrl = AWriter.FCodeStorage.FindOrCreateControl(child, ACtrl.controlName);

                    if ((ANodeName != "HiddenValues") && (childCtrl.controlTypePrefix == "hid"))
                    {
                        // somehow, hidden values get into the controls list as well. we don't want them there
                        continue;
                    }

                    IControlGenerator ctrlGen = AWriter.FindControlGenerator(childCtrl);

                    if (ctrlGen is FieldSetGenerator)
                    {
                        InsertControl(AWriter.FCodeStorage.FindOrCreateControl(child,
                                ACtrl.controlName), ATemplate, AItemsPlaceholder, ANodeName, AWriter);
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

        /// <summary>
        /// insert the buttons for the form, eg. submit button, cancel button, etc
        /// </summary>
        /// <param name="ACtrl"></param>
        /// <param name="ATemplate"></param>
        /// <param name="AItemsPlaceholder"></param>
        /// <param name="AWriter"></param>
        public static void InsertButtons(TControlDef ACtrl, ProcessTemplate ATemplate, string AItemsPlaceholder, TFormWriter AWriter)
        {
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
            AddResourceString(FTemplate, "FORMCAPTIONPage", ctrl, "page");
            AddResourceString(FTemplate, "FORMCAPTIONOf", ctrl, "of");

            InsertControl(ctrl, FTemplate, "FORMITEMSDEFINITION", "Controls", this);

            InsertButtons(ctrl, FTemplate, "BUTTONS", this);
        }

        /// based on the code model, create the code;
        /// using the code generators that have been loaded
        public override void CreateCode(TCodeStorage ACodeStorage, string ATemplateFile)
        {
            FCodeStorage = ACodeStorage;
            TControlGenerator.FCodeStorage = ACodeStorage;
            FTemplate = new ProcessTemplate(ATemplateFile);
            FFormName = Path.GetFileNameWithoutExtension(YamlFilename).Replace("-", "_");

            // drop language specific part of the name
            if (FFormName.Contains("."))
            {
                FFormName = FFormName.Substring(0, FFormName.IndexOf("."));
            }

            FFormName = FFormName.ToUpper()[0] + FFormName.Substring(1);
            FFormName += "Form";

            // load default header with license and copyright
            string templateDir = TAppSettingsManager.GetValue("TemplateDir", true);
            FTemplate.AddToCodelet("GPLFILEHEADER",
                ProcessTemplate.LoadEmptyFileComment(templateDir + Path.DirectorySeparatorChar + ".." +
                    Path.DirectorySeparatorChar));

            FTemplate.SetCodelet("UPLOADFORM", "");
            FTemplate.SetCodelet("CHECKFORVALIDUPLOAD", "");

            FLanguageFileTemplate = FTemplate.GetSnippet("LANGUAGEFILE");

            // find the first control that is a panel or groupbox or tab control
            if (FCodeStorage.HasRootControl("content"))
            {
                AddRootControl("content");
            }

            InsertCodeIntoTemplate(YamlFilename);

            string languagefilepath = Path.GetDirectoryName(YamlFilename) + Path.DirectorySeparatorChar +
                                      Path.GetFileNameWithoutExtension(YamlFilename) + "-lang-template.js";
            File.WriteAllText(languagefilepath, FLanguageFileTemplate.FinishWriting(true));
        }

        /// <summary>
        /// insert all variables into the template
        /// </summary>
        /// <param name="AXAMLFilename"></param>
        public virtual void InsertCodeIntoTemplate(string AXAMLFilename)
        {
            FTemplate.SetCodelet("FORMWIDTH", FCodeStorage.FWidth.ToString());
            FTemplate.SetCodelet("FORMHEIGHT", FCodeStorage.FHeight.ToString());

            if (FCodeStorage.HasAttribute("LabelWidth"))
            {
                FTemplate.SetCodelet("LABELWIDTH", FCodeStorage.GetAttribute("LabelWidth"));
            }
            else
            {
                FTemplate.SetCodelet("LABELWIDTH", "140");
            }

            FTemplate.SetCodelet("FORMNAME", FFormName);
            FTemplate.SetCodelet("FORMTYPE", "T" + FFormName);

            string FormHeader = "true";

            if (FCodeStorage.HasAttribute("FormHeader"))
            {
                FormHeader = FCodeStorage.GetAttribute("FormHeader");
            }

            FTemplate.SetCodelet("FORMHEADER", FormHeader);

            string FormFrame = "true";

            if (FCodeStorage.HasAttribute("FormFrame"))
            {
                FormFrame = FCodeStorage.GetAttribute("FormFrame");
            }

            FTemplate.SetCodelet("FORMFRAME", FormFrame);

            FLanguageFileTemplate.SetCodelet("FORMNAME", FFormName);
            FLanguageFileTemplate.SetCodelet("FORMTYPE", "T" + FFormName);
        }
    }
}