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
using System.Drawing;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.CodeGeneration;

namespace Ict.Tools.CodeGeneration.Winforms
{
    /*
     * This class writes code to a template
     * but it is not aware of the content and the origin of the content
     * the code generators that are loaded have the knowledge to generate proper code
     */
    public class TWinFormsWriter : IFormWriter
    {
        public String FInterfaceControlHookup = "";
        public String FInterfaceRunOnce = "";
        public String FInterfaceCanClose = "";
        public String FSuspendLayout, FResumePerformLayout;
        public String FResourceDirectory = "";
        private XmlDocument FImageResources;

        public TWinFormsWriter(string AFormType)
        {
            TAppSettingsManager settings = new TAppSettingsManager(false);

            FResourceDirectory = settings.GetValue("ResourceDir", true);

            FImageResources = new XmlDocument();
            XmlElement root = FImageResources.CreateElement("root");
            FImageResources.AppendChild(root);

            if (AFormType == "report")
            {
                AvailableControlGenerators.Add(new TabControlGenerator());
                AvailableControlGenerators.Add(new TabPageGenerator());
                AvailableControlGenerators.Add(new MenuGenerator());
                AvailableControlGenerators.Add(new MenuItemGenerator());
                AvailableControlGenerators.Add(new MenuItemSeparatorGenerator());
                AvailableControlGenerators.Add(new ToolbarButtonGenerator());
                AvailableControlGenerators.Add(new ToolbarSeparatorGenerator());
                AvailableControlGenerators.Add(new StatusBarGenerator());

                //			AvailableControlGenerators.Add(new StatusBarTextGenerator());
                AvailableControlGenerators.Add(new ToolBarGenerator());
                AvailableControlGenerators.Add(new GroupBoxGenerator());
                AvailableControlGenerators.Add(new RangeGenerator());
                AvailableControlGenerators.Add(new PanelGenerator());
                AvailableControlGenerators.Add(new CheckBoxReportGenerator());
                AvailableControlGenerators.Add(new DateTimePickerReportGenerator());
                AvailableControlGenerators.Add(new TextBoxReportGenerator());
            }
            else
            {
                AvailableControlGenerators.Add(new TabControlGenerator());
                AvailableControlGenerators.Add(new TabPageGenerator());
                AvailableControlGenerators.Add(new MenuGenerator());
                AvailableControlGenerators.Add(new MenuItemGenerator());
                AvailableControlGenerators.Add(new MenuItemSeparatorGenerator());
                AvailableControlGenerators.Add(new ToolbarButtonGenerator());
                AvailableControlGenerators.Add(new ToolbarSeparatorGenerator());
                AvailableControlGenerators.Add(new StatusBarGenerator());

                //			AvailableControlGenerators.Add(new StatusBarTextGenerator());
                AvailableControlGenerators.Add(new ToolBarGenerator());
                AvailableControlGenerators.Add(new GroupBoxGenerator());
                AvailableControlGenerators.Add(new RangeGenerator());
                AvailableControlGenerators.Add(new PanelGenerator());
                AvailableControlGenerators.Add(new UserControlGenerator());
                AvailableControlGenerators.Add(new LabelGenerator());
                AvailableControlGenerators.Add(new ButtonGenerator());
                AvailableControlGenerators.Add(new CheckBoxGenerator());
                AvailableControlGenerators.Add(new DateTimePickerGenerator());
                AvailableControlGenerators.Add(new TextBoxGenerator());
                AvailableControlGenerators.Add(new TTxtAutoPopulatedButtonLabelGenerator());
                AvailableControlGenerators.Add(new ComboBoxGenerator());
                AvailableControlGenerators.Add(new TcmbAutoPopulatedGenerator());
                AvailableControlGenerators.Add(new TcmbAutoCompleteGenerator());
                AvailableControlGenerators.Add(new RadioGroupComplexGenerator());
                AvailableControlGenerators.Add(new RadioGroupSimpleGenerator());
                AvailableControlGenerators.Add(new RadioGroupNoBorderGenerator());
                AvailableControlGenerators.Add(new RadioButtonGenerator());
                AvailableControlGenerators.Add(new NumericUpDownGenerator());
                AvailableControlGenerators.Add(new GridGenerator());
            }
        }

        public TCodeStorage CodeStorage
        {
            get
            {
                return FCodeStorage;
            }
        }

        #region ControlGenerators

        // List of all available controls, with prefix and
        //    function to find out if this fits (e.g. same prefixes for same control)
        // ArrayList of ControlGenerator
        public ArrayList AvailableControlGenerators = new ArrayList();
        public IControlGenerator FindControlGenerator(XmlNode curNode)
        {
            IControlGenerator fittingGenerator = null;

            foreach (IControlGenerator generator in AvailableControlGenerators)
            {
                if (generator.ControlFitsNode(curNode))
                {
                    if (fittingGenerator != null)
                    {
                        throw new Exception(
                            "Error: control with name " + curNode.Name + " does fit both control generators " + fittingGenerator.ControlType +
                            " and " +
                            generator.ControlType);
                    }

                    fittingGenerator = generator;
                }
            }

            if (fittingGenerator == null)
            {
                throw new Exception("Error: cannot find a generator for control with name " + curNode.Name);
            }

            return fittingGenerator;
        }

        #endregion

        /// <summary>
        /// check if the label should be translated;
        /// e.g. separators for menu items and empty strings cannot be translated;
        /// special workarounds for linebreaks are required;
        /// also called by GenerateI18N, class TGenerateCatalogStrings
        /// </summary>
        /// <param name="ALabelText">the label in english</param>
        /// <returns>true if this is a proper string</returns>
        public static bool ProperI18NCatalogGetString(string ALabelText)
        {
            // if there is MANUALTRANSLATION then don't translate; that is a workaround for \r\n in labels;
            // see eg. Client\lib\MPartner\gui\UC_PartnerInfo.Designer.cs, lblLoadingPartnerLocation.Text
            if (ALabelText.Contains("MANUALTRANSLATION"))
            {
                return false;
            }

            if (ALabelText.Trim().Length == 0)
            {
                return false;
            }

            if (ALabelText.Trim() == "-")
            {
                // menu separators etc
                return false;
            }

            // careful with \n and \r in the string; that is not allowed by gettext
            if (ALabelText.Contains("\\r") || ALabelText.Contains("\\n"))
            {
                throw new Exception("Problem with \\r or \\n");
            }

            return true;
        }

        public void SetControlProperty(string AControlName, string APropertyName, string APropertyValue)
        {
            if (APropertyName == "Dock")
            {
                if (!APropertyValue.StartsWith("System.Windows.Forms.DockStyle."))
                {
                    APropertyValue = "System.Windows.Forms.DockStyle." + APropertyValue;
                }
            }

            FTemplate.AddToCodelet("CONTROLINITIALISATION",
                "this." + AControlName + "." + APropertyName + " = " + APropertyValue + ";" + Environment.NewLine);

            if (APropertyName.EndsWith("Text"))
            {
                if (ProperI18NCatalogGetString(StringHelper.TrimQuotes(APropertyValue)))
                {
                    FTemplate.AddToCodelet("CATALOGI18N",
                        "this." + AControlName + "." + APropertyName + " = Catalog.GetString(" + APropertyValue + ");" + Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// check if the control has an attribute with the property name in the xml definition
        /// if such an attribute exists, then set it
        /// </summary>
        /// <param name="ACtrl"></param>
        /// <param name="APropertyName"></param>
        public void SetControlProperty(TControlDef ACtrl, string APropertyName)
        {
            if (TYml2Xml.HasAttribute(ACtrl.xmlNode, APropertyName))
            {
                SetControlProperty(ACtrl.controlName, APropertyName, TYml2Xml.GetAttribute(ACtrl.xmlNode, APropertyName));
            }
        }

        public void SetEventHandlerToControl(string AControlName, string AEvent)
        {
            FTemplate.AddToCodelet("CONTROLINITIALISATION",
                "this." + AControlName + "." + AEvent +
                " += new System.EventHandler(this." +
                AControlName + AEvent + ");" + Environment.NewLine);
        }

        public void SetEventHandlerToControl(string AControlName, string AEvent, string AEventHandlerType, string AEventHandlingMethod)
        {
            FTemplate.AddToCodelet("INITUSERCONTROLS",
                "this." + AControlName + "." + AEvent +
                " += new " + AEventHandlerType + "(" + AEventHandlingMethod + ");" + Environment.NewLine);
        }

        public void SetEventHandlerForForm(TEventHandler handler)
        {
            string localname = handler.eventHandler;

            if (localname.StartsWith("FPetraUtilsObject."))
            {
                localname = localname.Substring("FPetraUtilsObject.".Length);
                FCodeStorage.FEventHandler += "    this." + handler.eventName + " += new " + handler.eventType + "(this." + localname + ");" +
                                              Environment.NewLine;
                string objname = localname.Substring(0, localname.IndexOf("_") + 1);
                SetEventHandlerFunction(objname, handler.eventName, handler.eventHandler + "(sender, e);");
            }
            else
            {
                FCodeStorage.FEventHandler += "    this." + handler.eventName + " += new " + handler.eventType + "(this." + localname + ");" +
                                              Environment.NewLine;
            }
        }

        public void SetEventHandlerFunction(string AControlName, string AEvent, string AEventImplementation)
        {
            string EventArgsType = "EventArgs";

            if (AEvent == "Closing")
            {
                EventArgsType = "CancelEventArgs";
            }
            else if (AEvent == "KeyDown")
            {
                EventArgsType = "KeyEventArgs";
            }

            FCodeStorage.FEventHandlersImplementation +=
                "private void " + AControlName + AEvent +
                "(object sender, " + EventArgsType + " e)" + Environment.NewLine +
                "{" + Environment.NewLine +
                "    " + AEventImplementation + Environment.NewLine +
                "}" + Environment.NewLine + Environment.NewLine;
        }

        public void AddActionHandlerImplementation(TActionHandler AAction)
        {
            // the actual call what happens when the action is executed
            string ActionHandler =
                "/// auto generated" + Environment.NewLine +
                "protected void " + AAction.actionName + "(object sender, EventArgs e)" + Environment.NewLine +
                "{" + Environment.NewLine;

            if (AAction.actionId.Length > 0)
            {
                ActionHandler += "    FPetraUtilsObject.ExecuteAction(eActionId." + AAction.actionId + ");" + Environment.NewLine;
            }
            else
            {
                if (AAction.actionClick.Length > 0)
                {
                    ActionHandler += "    " + AAction.actionClick + "(sender, e);" + Environment.NewLine;
                }
                else
                {
                    ActionHandler += "    // TODO action " + AAction.actionName + Environment.NewLine;

                    //if we did not want the handler with just a TODO comment, we would need to return here.
                }
            }

            ActionHandler += "}" + Environment.NewLine + Environment.NewLine;

            FCodeStorage.FActionHandlers += ActionHandler;
        }

        /// <summary>
        /// Adds an image as a xml resource to FImageResources
        /// </summary>
        /// <param name="AControlName">the name how the image is addressed from the resources</param>
        /// <param name="AImageName">the full file name of the image</param>
        /// <param name="AImageOrIcon">this is a bitmap or an icon</param>
        public void AddImageToResource(string AControlName, string AImageName, string AImageOrIcon)
        {
            if (AImageName.Length < 1)
            {
                return;
            }

            string TempFileName = @"temp.xml";

            if (!Image2Xml(FResourceDirectory + System.IO.Path.DirectorySeparatorChar + AImageName, TempFileName, AImageOrIcon))
            {
                return;
            }

            XmlDocument TmpDoc = new XmlDocument();
            TmpDoc.Load(TempFileName);

            XmlNode TmpNode = TmpDoc.LastChild;
            XmlNode DestNode = FImageResources.LastChild;

            XmlElement HeaderElement = FImageResources.CreateElement("data");

            if (AImageOrIcon == "Icon")
            {
                HeaderElement.SetAttribute("name", AControlName);
                HeaderElement.SetAttribute("type", "System.Drawing.Icon, System.Drawing");
            }
            else
            {
                HeaderElement.SetAttribute("name", AControlName + ".Glyph");
                HeaderElement.SetAttribute("type", "System.Drawing.Bitmap, System.Drawing");
            }

            HeaderElement.SetAttribute("mimetype", "application/x-microsoft.net.object.bytearray.base64");

            XmlElement ImageElement = FImageResources.CreateElement("value");
            string formattedImage = "\n        ";
            string unformattedImage = TmpNode.InnerText.Trim();

            for (int counter = 0; counter < unformattedImage.Length; counter += 80)
            {
                int toCopy = 80;

                if (counter + toCopy > unformattedImage.Length)
                {
                    toCopy = unformattedImage.Length - counter;
                }

                formattedImage += unformattedImage.Substring(counter, toCopy) + '\n';

                if (counter + 80 < unformattedImage.Length)
                {
                    formattedImage += "        ";
                }
            }

            ImageElement.InnerText = formattedImage;

            XmlNode HeaderNode = DestNode.AppendChild(HeaderElement);
            HeaderNode.AppendChild(ImageElement);
        }

        /// <summary>
        /// Reads the binary data of the image and stores it in ATempFileName as xml data
        /// which then can be used in a .resx file.
        /// It seems for bitmaps for buttons and toolbars and menuitems, we need gif bitmaps
        /// but for window icons we need ico files
        /// </summary>
        /// <param name="AImageFileName">Full file name of the image</param>
        /// <param name="ATempFileName">Xml File where the image is stored</param>
        /// <param name="AImageOrIcon">is this an icon or bitmap</param>
        /// <returns>true if the image was properly stored in the Xml file</returns>
        private bool Image2Xml(string AImageFileName, string ATempFileName, string AImageOrIcon)
        {
            int bufferSize = 10000;

            byte[] buffer = new byte[bufferSize];
            int readBytes = 0;

            if (!File.Exists(AImageFileName))
            {
                return false;
            }

            if (!AImageFileName.EndsWith(".gif") && (AImageOrIcon == "Bitmap"))
            {
                // if we don't have a .gif file, create it
                Bitmap iconBitmap = new Bitmap(AImageFileName, true);

                int DotIndex = AImageFileName.LastIndexOf('.');

                if (DotIndex > 0)
                {
                    AImageFileName.Remove(DotIndex);
                }

                AImageFileName = AImageFileName + ".gif";

                iconBitmap.Save(AImageFileName, System.Drawing.Imaging.ImageFormat.Gif);
            }

            using (XmlWriter writer = XmlWriter.Create(ATempFileName))
                  {
                      FileStream inputFile = new FileStream(AImageFileName, FileMode.Open, FileAccess.Read, FileShare.Read);

                      writer.WriteStartDocument();
                      writer.WriteStartElement("value");
                      writer.WriteCharEntity('\n');

                      for (int counter = 0; counter < 8; counter++)
                      {
                          writer.WriteCharEntity(' ');
                      }

                      BinaryReader br = new BinaryReader(inputFile);

                      do
                      {
                          readBytes = br.Read(buffer, 0, bufferSize);
                          writer.WriteBase64(buffer, 0, readBytes);
                      } while (bufferSize <= readBytes);

                      br.Close();

                      writer.WriteEndElement();           // value
                      writer.WriteEndDocument();
                  }
                  return true;
        }

        /// <summary>
        /// Creates from the template resource file and the generated resource data
        /// a new resource file
        /// </summary>
        /// <param name="AResourceFile">The file to be created</param>
        /// <param name="AResourceTemplate">The template resource file</param>
        public void CreateResourceFile(string AResourceFile, string AResourceTemplate)
        {
            XmlDocument DestinationDoc = new XmlDocument();

            DestinationDoc.Load(AResourceTemplate);
            XmlNode DestinationNode = DestinationDoc.LastChild;

            XmlNode ParentNode = FImageResources.FirstChild;

            foreach (XmlNode ChildNode in ParentNode)
            {
                XmlNode NewNode = DestinationDoc.ImportNode(ChildNode, true);
                DestinationNode.AppendChild(NewNode);
            }

            DestinationDoc.Save(AResourceFile + ".new");
            TTextFile.UpdateFile(AResourceFile);
        }

        public void CallControlFunction(string AControlName, string AFunctionCall)
        {
            FTemplate.AddToCodelet("CONTROLINITIALISATION",
                "this." + AControlName + "." + AFunctionCall + ";" + Environment.NewLine);
        }

        public void AddContainer(string AControlName)
        {
            FSuspendLayout += "this." + AControlName + ".SuspendLayout();" + Environment.NewLine;
            FResumePerformLayout = "this." + AControlName + ".ResumeLayout(false);" + Environment.NewLine + FResumePerformLayout;
        }

        public virtual void ApplyDerivedFunctionality(IControlGenerator generator, XmlNode curNode)
        {
            generator.ApplyDerivedFunctionality(this, curNode);
        }

        // returns the initialiseCode, see e.g. ProcessReportForm
        public virtual string ProcessDataSource(XmlNode curNode, string AControlName)
        {
            // todo: depends how the data source is connected; see example in WriteReportForm.cs
            return "";
        }

        public SortedList FControlDataTypes = new SortedList();
        public void InitialiseDataSource(XmlNode curNode, string AControlName)
        {
            string InitialiseCodelet = ProcessDataSource(curNode, AControlName);

            if (TYml2Xml.HasAttribute(curNode, "DependsOn"))
            {
                string dependsOn = TYml2Xml.GetAttribute(curNode, "DependsOn");
                int index = FControlDataTypes.IndexOfKey(dependsOn);

                if (index == -1)
                {
                    throw new Exception("problem with DependsOn " + dependsOn + " of control " + AControlName);
                }

                string dependsOnType = FControlDataTypes.GetByIndex(index).ToString();
                FTemplate.AddToCodelet("INITIALISE_" + dependsOn, InitialiseCodelet);
                FTemplate.AddToCodelet("INITIALISESCREEN", StringHelper.UpperCamelCase(dependsOn,
                        ",",
                        false,
                        false) + "_Initialise(" + dependsOn + ".GetSelected" +
                    dependsOnType + "());" + Environment.NewLine);
            }
            else
            {
                FTemplate.AddToCodelet("INITIALISESCREEN", InitialiseCodelet);
            }

            if (TYml2Xml.HasAttribute(curNode, "OnChangeDataType"))
            {
                FControlDataTypes.Add(curNode.Name, TYml2Xml.GetAttribute(curNode, "OnChangeDataType"));
            }
        }

        protected void FinishUpInitialisingControls()
        {
            // if no other control depends on a combobox, e.g. cmbPostalRegionRegion, don't require any code
            foreach (string dependsOn in FControlDataTypes.GetKeyList())
            {
                int index = FControlDataTypes.IndexOfKey(dependsOn);
                string currentContent = FTemplate.AddToCodelet("INITIALISE_" + dependsOn, "");

                if (currentContent.Length == 0)
                {
                    FTemplate.AddToCodelet("INITIALISE_" + dependsOn, "BLANK");
                }
            }
        }

        /// <summary>
        /// this function is needed because the writer is called twice, one time for the designer,
        /// and one time for the normal code
        /// </summary>
        protected void ResetAllValues()
        {
            FSuspendLayout = "";
            FResumePerformLayout = "";
            FControlDataTypes = new SortedList();
            TableLayoutPanelGenerator.countTableLayoutPanel = 0;
        }

        public TCodeStorage FCodeStorage = null;
        public ProcessTemplate FTemplate;

        public ProcessTemplate Template
        {
            get
            {
                return FTemplate;
            }
        }

        private void AddRootControl(string prefix)
        {
            TControlDef ctrl = FCodeStorage.GetRootControl(prefix);

            IControlGenerator generator = FindControlGenerator(ctrl.xmlNode);

            if (generator.RequiresChildren)
            {
                // don't add toolbar if there are no toolbar buttons
                if (ctrl.NumberChildren == 0)
                {
                    return;
                }
            }

            generator.GenerateDeclaration(this, ctrl);
            generator.SetControlProperties(this, ctrl);

            if (generator.AddControlToContainer)
            {
                FTemplate.AddToCodelet("ADDMAINCONTROLS", "this.Controls.Add(this." + ctrl.controlName + ");" + Environment.NewLine);
            }

            if (prefix == "mnu")
            {
                FTemplate.AddToCodelet("ADDMAINCONTROLS", "this.MainMenuStrip = " + ctrl.controlName + ";" + Environment.NewLine);
            }
        }

        /*
         * based on the code model, create the code;
         * using the code generators that have been loaded
         */
        public virtual void CreateCode(TCodeStorage ACodeStorage, string AXAMLFilename)
        {
            ResetAllValues();
            FCodeStorage = ACodeStorage;
            FTemplate = new ProcessTemplate();

            // load default header with license and copyright
            TAppSettingsManager opts = new TAppSettingsManager(false);
            string templateDir = opts.GetValue("TemplateDir", true);
            StreamReader sr = new StreamReader(templateDir + Path.DirectorySeparatorChar + ".." +
                Path.DirectorySeparatorChar + "EmptyFileComment.txt");
            string fileheader = sr.ReadToEnd();
            sr.Close();
            fileheader = fileheader.Replace(">>>> Put your full name or just a shortname here <<<<", "auto generated");
            FTemplate.AddToCodelet("GPLFILEHEADER", fileheader);

            // init some template variables that can be empty
            FTemplate.AddToCodelet("INITUSERCONTROLS", "");
            FTemplate.AddToCodelet("INITMANUALCODE", "");

            FTemplate.AddToCodelet("INITACTIONSTATE", "FPetraUtilsObject.InitActionState();" + Environment.NewLine);

            if (FCodeStorage.FHasManualCodeInOtherFile)
            {
                FTemplate.AddToCodelet("INITMANUALCODE", "InitializeManualCode();");
            }

            // find the first control that is a panel or groupbox or tab control
            AddRootControl("content");

            // Toolbar
            AddRootControl("tbr");

            // Main Menu
            AddRootControl("mnu");

            // Statusbar
            AddRootControl("stb");

            // add form events
            foreach (TEventHandler handler in FCodeStorage.FEventList.Values)
            {
                SetEventHandlerForForm(handler);
            }

            XmlNode rootNode = (XmlNode)FCodeStorage.FXmlNodes["RootNode"];

            if (TYml2Xml.HasAttribute(rootNode, "Icon"))
            {
                string iconFileName = TYml2Xml.GetAttribute(rootNode, "Icon");
                FTemplate.AddToCodelet("ADDMAINCONTROLS",
                    "this.Icon = (System.Drawing.Icon)resources.GetObject(\"$this.Icon\");" + Environment.NewLine);
                AddImageToResource("$this.Icon", iconFileName, "Icon");
            }

            // add title
            if (ProperI18NCatalogGetString(FCodeStorage.FFormTitle))
            {
                FTemplate.AddToCodelet("CATALOGI18N",
                    "this.Text = Catalog.GetString(\"" + FCodeStorage.FFormTitle + "\");" + Environment.NewLine);
            }

            // add actions
            foreach (TActionHandler handler in FCodeStorage.FActionList.Values)
            {
                if (!handler.actionName.StartsWith("cnd"))
                {
                    AddActionHandlerImplementation(handler);
                }

                if (TYml2Xml.HasAttribute(handler.actionNode, "InitialValue"))
                {
                    FCodeStorage.FActionHandlers +=
                        "/// auto generated" + Environment.NewLine +
                        "protected void Activate" + handler.actionName.Substring(3) +
                        "(bool AEnabled)" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    {#ENABLEDEPENDINGACTIONS_" + handler.actionName + "}" + Environment.NewLine +
                        "}" + Environment.NewLine + Environment.NewLine;
                    Template.AddToCodelet(
                        "INITACTIONSTATE",
                        "Activate" + handler.actionName.Substring(3) + "(" + TYml2Xml.GetAttribute(handler.actionNode,
                            "InitialValue") + ");" + Environment.NewLine);
                }

                if (TYml2Xml.HasAttribute(handler.actionNode, "Enabled"))
                {
                    Template.AddToCodelet(
                        "ENABLEDEPENDINGACTIONS_" + TYml2Xml.GetAttribute(handler.actionNode, "Enabled"),
                        "FPetraUtilsObject.EnableAction(\"" + handler.actionName + "\", AEnabled);" + Environment.NewLine);
                }
            }

            FinishUpInitialisingControls();
            InsertCodeIntoTemplate(AXAMLFilename);
        }

        public virtual void InsertCodeIntoTemplate(string AXAMLFilename)
        {
            FTemplate.ReplacePlaceHolder("BASECLASSNAME", FCodeStorage.FBaseClass, "System.Windows.Forms.Form");
            FTemplate.ReplacePlaceHolder("INTERFACENAME", FCodeStorage.FInterfaceName, "Ict.Petra.Client.CommonForms.IFrmPetra");
            FTemplate.ReplacePlaceHolder("UTILOBJECTCLASS", FCodeStorage.FUtilObjectClass, "Ict.Petra.Client.CommonForms.TFrmPetraUtils");
            FTemplate.ReplacePlaceHolder("FORMTITLE", FCodeStorage.FFormTitle);
            FTemplate.ReplacePlaceHolder("NAMESPACE", FCodeStorage.FNamespace);
            FTemplate.ReplacePlaceHolder("CLASSNAME", FCodeStorage.FClassName);
            FTemplate.ReplacePlaceHolder("SUSPENDLAYOUT", FSuspendLayout);
            FTemplate.ReplacePlaceHolder("FORMSIZE", FCodeStorage.FWidth.ToString() + ", " + FCodeStorage.FHeight.ToString());
            FTemplate.ReplacePlaceHolder("CLASSEVENTHANDLER", FCodeStorage.FEventHandler);
            FTemplate.ReplacePlaceHolder("RESUMELAYOUT", FResumePerformLayout);
            FTemplate.ReplacePlaceHolder("XAMLSRCFILE", System.IO.Path.GetFileName(AXAMLFilename));

            if (FCodeStorage.FEventHandlersImplementation.Length == 0)
            {
                FCodeStorage.FEventHandlersImplementation = "BLANK";
            }

            FTemplate.ReplacePlaceHolder("EVENTHANDLERSIMPLEMENTATION", FCodeStorage.FEventHandlersImplementation);
            FTemplate.ReplacePlaceHolder("ACTIONHANDLERS", FCodeStorage.FActionHandlers);

            FTemplate.ReplacePlaceHolder("HOOKUPINTERFACEIMPLEMENTATION", FInterfaceControlHookup);
            FTemplate.ReplacePlaceHolder("RUNONCEINTERFACEIMPLEMENTATION", FInterfaceRunOnce);
            FTemplate.ReplacePlaceHolder("CANCLOSEINTERFACEIMPLEMENTATION", FInterfaceCanClose);

            if (FCodeStorage.FTemplateParameters != null)
            {
                FTemplate.processTemplateParameters(FCodeStorage.FTemplateParameters);
            }

            // todo: other parts that are not in the designer section,
            // but need to be updated in the main code file; need to use special regions
            //string autoActionSection = designWriter.CreateAutoActionSection();
            //writer.ReplaceRegion("CodeGenerator Managed Code", autoActionSection);
        }

        public bool WriteFile(string ADestinationFile, string ATemplateFile)
        {
            ProcessTemplate LocalTemplate = new ProcessTemplate(ATemplateFile);

            // reuse the codelets that have been generated by CreateCode
            LocalTemplate.FCodelets = FTemplate.FCodelets;
            return LocalTemplate.FinishWriting(ADestinationFile, ".cs", true);
        }
    }
}