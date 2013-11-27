//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Security.Cryptography;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.Winforms
{
    /// <summary>
    /// This class writes code to a template
    /// but it is not aware of the content and the origin of the content
    /// the code generators that are loaded have the knowledge to generate proper code
    /// </summary>
    public class TWinFormsWriter : TFormWriter
    {
        /// <summary>will write code into this variable</summary>
        public String FInterfaceControlHookup = "";
        /// <summary>will write code into this variable</summary>
        public String FInterfaceRunOnce = "";
        /// <summary>will write code into this variable</summary>
        public String FInterfaceCanClose = "";
        /// <summary>will write code into this variable</summary>
        public String FSuspendLayout;
        /// <summary>will write code into this variable</summary>
        public String FResumePerformLayout;
        /// <summary>will write code into this variable</summary>
        public String FResourceDirectory = "";
        /// <summary>store image resources in this xml document for the resource file</summary>
        private XmlDocument FImageResources;

        /// <summary>
        /// constructor. initialise the generators for the controls
        /// </summary>
        /// <param name="AFormType"></param>
        public TWinFormsWriter(string AFormType)
        {
            FResourceDirectory = TAppSettingsManager.GetValue("ResourceDir", true);

            FImageResources = new XmlDocument();
            XmlElement root = FImageResources.CreateElement("root");
            FImageResources.AppendChild(root);

            BaseControlGeneratorType = typeof(TControlGenerator);

            if (AFormType == "report")
            {
                AddControlGenerator(new TabControlGenerator());
                AddControlGenerator(new TabPageGenerator());
                AddControlGenerator(new MenuGenerator());
                AddControlGenerator(new MenuItemGenerator());
                AddControlGenerator(new MenuItemSeparatorGenerator());
                AddControlGenerator(new ToolbarButtonGenerator());
                AddControlGenerator(new ToolbarComboBoxGenerator());
                AddControlGenerator(new ToolbarSeparatorGenerator());
                AddControlGenerator(new StatusBarGenerator());

                //          AddControlGenerator(new StatusBarTextGenerator());
                AddControlGenerator(new ToolBarGenerator());
                AddControlGenerator(new GroupBoxGenerator());
                AddControlGenerator(new LabelGenerator());
                AddControlGenerator(new ButtonGenerator());
                AddControlGenerator(new RangeGenerator());
                AddControlGenerator(new PanelGenerator());
                AddControlGenerator(new ExtendedPanelGenerator());
                AddControlGenerator(new CheckBoxReportGenerator());
                AddControlGenerator(new TClbVersatileReportGenerator());
                AddControlGenerator(new DateTimePickerReportGenerator());
                AddControlGenerator(new TextBoxReportGenerator());
                AddControlGenerator(new TTxtAutoPopulatedButtonLabelGenerator());
                AddControlGenerator(new TTxtCurrencyTextBoxReportGenerator());
                AddControlGenerator(new TTxtNumericTextBoxReportGenerator());
                AddControlGenerator(new ComboBoxReportGenerator());
                AddControlGenerator(new TcmbAutoPopulatedReportGenerator());
                AddControlGenerator(new RadioGroupComplexReportGenerator());
                AddControlGenerator(new RadioGroupSimpleReportGenerator());
                AddControlGenerator(new RadioButtonReportGenerator());
                AddControlGenerator(new UserControlReportGenerator());
                AddControlGenerator(new SourceGridReportGenerator());
            }
            else
            {
                AddControlGenerator(new TabControlGenerator());
                AddControlGenerator(new TabPageGenerator());
                AddControlGenerator(new MenuGenerator());
                AddControlGenerator(new MenuItemGenerator());
                AddControlGenerator(new MenuItemSeparatorGenerator());
                AddControlGenerator(new ToolbarControlHostGenerator());
                AddControlGenerator(new ToolbarTextBoxGenerator());
                AddControlGenerator(new ToolbarLabelGenerator());
                AddControlGenerator(new ToolbarButtonGenerator());
                AddControlGenerator(new ToolbarComboBoxGenerator());
                AddControlGenerator(new ToolbarSeparatorGenerator());
                AddControlGenerator(new StatusBarGenerator());
                AddControlGenerator(new ToolBarGenerator());
                AddControlGenerator(new GroupBoxGenerator());
                AddControlGenerator(new RangeGenerator());
                AddControlGenerator(new PanelGenerator());
                AddControlGenerator(new ExtendedPanelGenerator());
                AddControlGenerator(new SplitContainerGenerator());
                AddControlGenerator(new UserControlGenerator());
                AddControlGenerator(new LabelGenerator());
                AddControlGenerator(new LinkLabelGenerator());
                AddControlGenerator(new ButtonGenerator());
                AddControlGenerator(new CheckBoxGenerator());
                AddControlGenerator(new TClbVersatileGenerator());
                AddControlGenerator(new DateTimePickerGenerator());
                AddControlGenerator(new TreeViewGenerator());
                AddControlGenerator(new TextBoxGenerator());
                AddControlGenerator(new TTxtAutoPopulatedButtonLabelGenerator());
                AddControlGenerator(new TTxtCurrencyTextBoxGenerator());
                AddControlGenerator(new TTxtNumericTextBoxGenerator());
                AddControlGenerator(new ComboBoxGenerator());
                AddControlGenerator(new TcmbAutoPopulatedGenerator());
                AddControlGenerator(new TcmbAutoCompleteGenerator());
                AddControlGenerator(new TCmbVersatileGenerator());
                AddControlGenerator(new PrintPreviewGenerator());
                AddControlGenerator(new PrintPreviewWithToolbarGenerator());
                AddControlGenerator(new RadioGroupComplexGenerator());
                AddControlGenerator(new RadioGroupSimpleGenerator());
                AddControlGenerator(new RadioGroupNoBorderGenerator());
                AddControlGenerator(new RadioButtonGenerator());
                AddControlGenerator(new NumericUpDownGenerator());
                AddControlGenerator(new WinformsGridGenerator());
                AddControlGenerator(new SourceGridGenerator());
            }
        }

        /// <summary>
        /// get the file extension for the resulting file
        /// </summary>
        public override string CodeFileExtension
        {
            get
            {
                return ".cs";
            }
        }

        private Dictionary <string, Dictionary <string, string>>FControlProperties = new Dictionary <string, Dictionary <string, string>>();

        private void WriteAllControls()
        {
            foreach (string controlName in FControlProperties.Keys)
            {
                FTemplate.AddToCodelet("CONTROLINITIALISATION",
                    Environment.NewLine + "//" + Environment.NewLine +
                    "// " + controlName + Environment.NewLine + "//" + Environment.NewLine);

                string attributes = string.Empty;
                string events = string.Empty;
                string addControls = string.Empty;

                foreach (string propertyName in FControlProperties[controlName].Keys)
                {
                    string line = FControlProperties[controlName][propertyName] + ";" + Environment.NewLine;

                    if (line.Contains("Controls.Add("))
                    {
                        addControls += line;
                    }
                    else if (line.Contains(" += new "))
                    {
                        events += line;
                    }
                    else
                    {
                        if ((propertyName == "Size")
                            && FControlProperties[controlName].ContainsKey("Dock")
                            && FControlProperties[controlName]["Dock"].Contains("DockStyle.Fill"))
                        {
                            // do not write Size for Dock Fill
                        }

                        if ((propertyName == "Location")
                            && FControlProperties[controlName].ContainsKey("Dock"))
                        {
                            // do not write Location for Dock
                        }
                        else if ((propertyName == "Anchor")
                                 && FControlProperties[controlName].ContainsKey("Dock")
                                 && FControlProperties[controlName]["Dock"].Contains("DockStyle.Fill"))
                        {
                            // no anchor for Dock Fill
                        }
                        else if ((propertyName == "AutoSize")
                                 && FControlProperties[controlName].ContainsKey("Dock")
                                 && FControlProperties[controlName]["Dock"].Contains("DockStyle.Fill"))
                        {
                            // do not write AutoSize for Dock Fill
                        }
                        else if ((propertyName == "AutoSize")
                                 && FControlProperties[controlName].ContainsKey("Size"))
                        {
                            // do not write AutoSize
                        }
                        else
                        {
                            attributes += line;
                        }
                    }
                }

                FTemplate.AddToCodelet("CONTROLINITIALISATION", attributes);
                FTemplate.AddToCodelet("CONTROLINITIALISATION", events);
                FTemplate.AddToCodelet("CONTROLINITIALISATION", addControls);
            }
        }

        /// <summary>
        /// remove the current value of a control property
        /// </summary>
        /// <param name="AControlName"></param>
        /// <param name="APropertyName"></param>
        /// <returns></returns>
        public override void ClearControlProperty(string AControlName, string APropertyName)
        {
            if (FControlProperties.ContainsKey(AControlName) && FControlProperties[AControlName].ContainsKey(APropertyName))
            {
                FControlProperties[AControlName].Remove(APropertyName);
            }
        }

        /// <summary>
        /// retrieve the current value of a control property
        /// </summary>
        /// <param name="AControlName"></param>
        /// <param name="APropertyName"></param>
        /// <returns></returns>
        public override string GetControlProperty(string AControlName, string APropertyName)
        {
            if (FControlProperties.ContainsKey(AControlName) && FControlProperties[AControlName].ContainsKey(APropertyName))
            {
                return FControlProperties[AControlName][APropertyName];
            }

            return string.Empty;
        }

        /// this can be used for properties, event handlers, etc
        private void SetControlProperty(string AControlName, string APropertyName, string APropertyValue)
        {
            if (!FControlProperties.ContainsKey(AControlName))
            {
                FControlProperties.Add(AControlName, new Dictionary <string, string>());
            }

            if (!FControlProperties[AControlName].ContainsKey(APropertyName))
            {
                FControlProperties[AControlName].Add(APropertyName, string.Empty);
            }

            FControlProperties[AControlName][APropertyName] = APropertyValue;
        }

        /// <summary>
        /// set the properties of the controls in the Designer file
        /// </summary>
        /// <param name="AControlName"></param>
        /// <param name="APropertyName"></param>
        /// <param name="APropertyValue"></param>
        /// <param name="ACreateTranslationForLabel"></param>
        public override void SetControlProperty(string AControlName, string APropertyName, string APropertyValue, bool ACreateTranslationForLabel)
        {
            if (APropertyName == "Dock")
            {
                if (!APropertyValue.StartsWith("System.Windows.Forms.DockStyle."))
                {
                    APropertyValue = "System.Windows.Forms.DockStyle." + APropertyValue;
                }
            }

            SetControlProperty(AControlName, APropertyName,
                "this." + AControlName + "." + APropertyName + " = " + APropertyValue);

            if (ACreateTranslationForLabel)
            {
                if (APropertyName.EndsWith("Text"))
                {
                    if (ProperI18NCatalogGetString(StringHelper.TrimQuotes(APropertyValue)))
                    {
                        if (APropertyValue.EndsWith(":\""))
                        {
                            FTemplate.AddToCodelet("CATALOGI18N",
                                "this." + AControlName + "." + APropertyName + " = Catalog.GetString(" +
                                APropertyValue.Substring(0, APropertyValue.Length - 2) + "\") + \":\";" + Environment.NewLine);
                        }
                        else
                        {
                            FTemplate.AddToCodelet("CATALOGI18N",
                                "this." + AControlName + "." + APropertyName + " = Catalog.GetString(" + APropertyValue + ");" + Environment.NewLine);
                        }
                    }
                }
                else if (AControlName.StartsWith("dtp")
                         && (APropertyName == "Description"))
                {
                    FTemplate.AddToCodelet("CATALOGI18N",
                        "this." + AControlName + "." + APropertyName + " = Catalog.GetString(" + APropertyValue + ");" + Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// deal with event handlers
        /// </summary>
        /// <param name="AControlName"></param>
        /// <param name="AEvent"></param>
        /// <param name="AEventHandlerType"></param>
        /// <param name="AEventHandlingMethod"></param>
        public override void SetEventHandlerToControl(string AControlName, string AEvent, string AEventHandlerType, string AEventHandlingMethod)
        {
            string CodeletName = "CONTROLINITIALISATION";

            if (AEventHandlingMethod.Contains("FPetraUtilsObject"))
            {
                CodeletName = "INITUSERCONTROLS";
            }
            else if (!AEventHandlingMethod.StartsWith("this."))
            {
                AEventHandlingMethod = "this." + AEventHandlingMethod;
            }

            if (CodeletName == "CONTROLINITIALISATION")
            {
                SetControlProperty(AControlName, AEvent,
                    "this." + AControlName + "." + AEvent +
                    " += new " + AEventHandlerType + "(" + AEventHandlingMethod + ")");
            }
            else
            {
                FTemplate.AddToCodelet(CodeletName,
                    "this." + AControlName + "." + AEvent +
                    " += new " + AEventHandlerType + "(" + AEventHandlingMethod + ");" + Environment.NewLine);
            }
        }

        private void SetEventHandlerForForm(TEventHandler handler)
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

        /// <summary>
        /// event handlers
        /// </summary>
        /// <param name="AControlName"></param>
        /// <param name="AEvent"></param>
        /// <param name="AEventImplementation"></param>
        public override void SetEventHandlerFunction(string AControlName, string AEvent, string AEventImplementation)
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

        private void AddReportParameterImplementaion(TReportParameter AReportParameter)
        {
            FCodeStorage.FReportParametersImplementation +=
                "FPetraUtilsObject.AddAvailableFunction(new " +
                AReportParameter.columnFunctionClassName + "(\"" +
                AReportParameter.functionDescription + "\", " +
                AReportParameter.functionParameters + "));" +
                Environment.NewLine;
        }

        private void AddActionHandlerImplementation(TActionHandler AAction)
        {
            // the actual call what happens when the action is executed
            // only create an action handler for calls to FPetraUtilsObject, because that would not work in the designer
            if (AAction.actionId.Length > 0)
            {
                string ActionHandler =
                    "/// auto generated" + Environment.NewLine +
                    "protected void " + AAction.actionName + "(object sender, EventArgs e)" + Environment.NewLine +
                    "{" + Environment.NewLine;
                ActionHandler += "    FPetraUtilsObject.ExecuteAction(eActionId." + AAction.actionId + ");" + Environment.NewLine;
                ActionHandler += "}" + Environment.NewLine + Environment.NewLine;

                FCodeStorage.FActionHandlers += ActionHandler;
            }
            else if (AAction.actionClick.StartsWith("FPetraUtilsObject"))
            {
                string ActionHandler =
                    "/// auto generated" + Environment.NewLine +
                    "protected void " + AAction.actionName + "(object sender, EventArgs e)" + Environment.NewLine +
                    "{" + Environment.NewLine;
                ActionHandler += "    " + AAction.actionClick + "(sender, e);" + Environment.NewLine;
                ActionHandler += "}" + Environment.NewLine + Environment.NewLine;

                FCodeStorage.FActionHandlers += ActionHandler;
            }
        }

        /// <summary>
        /// get the md5sum hash of a file
        /// </summary>
        private String GetMd5Sum(String AFilename)
        {
            FileStream fs = new FileStream(AFilename, FileMode.Open, FileAccess.Read);
            MD5CryptoServiceProvider cr = new MD5CryptoServiceProvider();
            string ReturnValue = BitConverter.ToString(cr.ComputeHash(fs)).Replace("-", "").ToLower();

            fs.Close();
            return ReturnValue;
        }

        /// <summary>
        /// Adds an image as a xml resource to FImageResources
        /// </summary>
        /// <param name="AControlName">the name how the image is addressed from the resources</param>
        /// <param name="AImageName">the full file name of the image</param>
        /// <param name="AImageOrIcon">this is a bitmap or an icon</param>
        public override void AddImageToResource(string AControlName, string AImageName, string AImageOrIcon)
        {
            if (AImageName.Length < 1)
            {
                return;
            }

            if (!File.Exists(FResourceDirectory + System.IO.Path.DirectorySeparatorChar + AImageName))
            {
                TLogging.Log("Warning !!! Cannot find image file " + FResourceDirectory + System.IO.Path.DirectorySeparatorChar + AImageName);
                return;
            }

            // only add ComponentResourceManager to generated code if it is required
            FTemplate.SetCodelet("RESOURCES", "true");

            string Md5SumImageFile = GetMd5Sum(FResourceDirectory + System.IO.Path.DirectorySeparatorChar + AImageName);
            string formattedImage = Image2Base64(FResourceDirectory + System.IO.Path.DirectorySeparatorChar + AImageName, AImageOrIcon);

            if (formattedImage.Length == 0)
            {
                return;
            }

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
            ImageElement.InnerText = formattedImage;

            XmlNode DestNode = FImageResources.LastChild;
            XmlNode HeaderNode = DestNode.AppendChild(HeaderElement);
            HeaderNode.AppendChild(FImageResources.CreateComment(Md5SumImageFile));
            HeaderNode.AppendChild(ImageElement);
        }

        /// <summary>
        /// Reads the binary data of the image and returns it as a formatted Base64 string
        /// which then can be used in a .resx file.
        /// It seems for bitmaps for buttons and toolbars and menuitems, we need gif bitmaps
        /// but for window icons we need ico files
        /// </summary>
        /// <param name="AImageFileName">Full file name of the image</param>
        /// <param name="AImageOrIcon">is this an icon or bitmap</param>
        /// <returns>the formatted Base64 representation of the image</returns>
        private string Image2Base64(string AImageFileName, string AImageOrIcon)
        {
            if (!File.Exists(AImageFileName))
            {
                return "";
            }

            // AlanP added this retry loop after getting fed up with generating the solution and finding that
            //  access to go.gif.resx is denied.  The assumption is that multi-threaded code generation
            //  wants to access a well-used image file in multiple places.  The resx file is only a temporary
            //  file.  A few lines down we delete it, which is probably why it fails to open.
            // If that is not the cause the code shouldn't do any harm.
            int nTries = 0;
            int nTriesLimit = 4;                // 4 tries over 2 seconds should be enough!
            string exceptionMsg = String.Empty;
            TXMLParser parser = null;

            while (nTries < nTriesLimit)
            {
                try
                {
                    System.Resources.ResXResourceWriter w = new System.Resources.ResXResourceWriter(AImageFileName + ".resx");

                    if (AImageOrIcon == "Icon")
                    {
                        w.AddResource(AImageFileName, new Icon(AImageFileName));
                    }
                    else if ((AImageOrIcon == "Bitmap") && (Path.GetExtension(AImageFileName) == ".ico"))
                    {
                        w.AddResource(AImageFileName, (new Icon(AImageFileName)).ToBitmap());
                    }
                    else
                    {
                        w.AddResource(AImageFileName, new Bitmap(AImageFileName));
                    }

                    w.Close();

                    parser = new TXMLParser(AImageFileName + ".resx", false);
                    File.Delete(AImageFileName + ".resx");

                    nTries = 999;           // success!
                }
                catch (Exception ex)        // probably an IO exception
                {
                    nTries++;
                    parser = null;
                    exceptionMsg = ex.Message;
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(500);
                }
            }

            if ((nTries == nTriesLimit) || (parser == null))
            {
                throw new IOException(String.Format(
                        "The system had {0} attempts to create, parse and delete the file {1}.resx as a resource but the following IO exception was raised: {2}",
                        nTries, AImageFileName, exceptionMsg));
            }

            XmlDocument doc = parser.GetDocument();
            XmlNode child = doc.DocumentElement.FirstChild;

            while (child != null)
            {
                if ((child.Name == "data") && (TYml2Xml.GetAttribute(child, "name") == AImageFileName))
                {
                    return child.InnerText;
                }

                child = child.NextSibling;
            }

            return string.Empty;
        }

        /// <summary>
        /// Creates from the template resource file and the generated resource data
        /// a new resource file
        /// </summary>
        /// <param name="AYamlFilename">The path of the yaml file is used to calculate the name of the resource file</param>
        /// <param name="ATemplateDir">The path to the templates</param>
        public override void CreateResourceFile(string AYamlFilename, string ATemplateDir)
        {
            string ResourceFile = System.IO.Path.GetDirectoryName(AYamlFilename) +
                                  System.IO.Path.DirectorySeparatorChar +
                                  System.IO.Path.GetFileNameWithoutExtension(AYamlFilename) +
                                  "-generated.resx";

            XmlDocument OrigResourceDoc = null;

            if (File.Exists(ResourceFile))
            {
                OrigResourceDoc = (new TXMLParser(ResourceFile, false)).GetDocument();
            }

            string ResourceTemplate = ATemplateDir + Path.DirectorySeparatorChar + "resources.resx";

            XmlDocument DestinationDoc = new XmlDocument();

            DestinationDoc.Load(ResourceTemplate);
            XmlNode DestinationNode = DestinationDoc.LastChild;

            XmlNode ParentNode = FImageResources.FirstChild;

            foreach (XmlNode ChildNode in ParentNode)
            {
                XmlNode OrigDataNode = null;

                if (OrigResourceDoc != null)
                {
                    OrigDataNode = OrigResourceDoc.DocumentElement.FirstChild;
                }

                while (OrigDataNode != null && !(OrigDataNode.Name == "data"
                                                 && TYml2Xml.GetAttribute(OrigDataNode, "name") == TYml2Xml.GetAttribute(ChildNode, "name")))
                {
                    OrigDataNode = OrigDataNode.NextSibling;
                }

                // compare the md5sum of the original icon file. use original icon encoding for the same file.
                // it seems the translation of images to base64 is always different, depending on the machine
                if (OrigDataNode != null)
                {
                    if (!OrigDataNode.HasChildNodes || (!OrigDataNode.FirstChild.InnerText.Equals(ChildNode.FirstChild.InnerText)))
                    {
                        OrigDataNode = null;
                    }
                }

                XmlNode NewNode = DestinationDoc.ImportNode(OrigDataNode == null ? ChildNode : OrigDataNode, true);
                DestinationNode.AppendChild(NewNode);
            }

            DestinationDoc.Save(ResourceFile + ".new");
            TTextFile.UpdateFile(ResourceFile, true);
        }

        /// write the designer code using the definitions in the yaml file
        public override void CreateDesignerFile(string AYamlFilename, XmlNode ARootNode, string ATemplateDir)
        {
            string DesignerFile = System.IO.Path.GetDirectoryName(AYamlFilename) +
                                  System.IO.Path.DirectorySeparatorChar +
                                  System.IO.Path.GetFileNameWithoutExtension(AYamlFilename) +
                                  "-generated.Designer.cs";

            string designerTemplate = String.Empty;

            if (TYml2Xml.HasAttribute(ARootNode, "DesignerTemplate"))
            {
                designerTemplate = TYml2Xml.GetAttribute(ARootNode, "DesignerTemplate") + ".cs";
            }
            else
            {
                designerTemplate = "designer.cs";
            }

            designerTemplate = ATemplateDir + Path.DirectorySeparatorChar + designerTemplate;

            WriteFile(DesignerFile, designerTemplate);
        }

        /// <summary>get the name of the destination generated file</summary>
        public override string CalculateDestinationFilename(string AYamlFilename)
        {
            return System.IO.Path.GetDirectoryName(AYamlFilename) +
                   System.IO.Path.DirectorySeparatorChar +
                   System.IO.Path.GetFileNameWithoutExtension(AYamlFilename) +
                   "-generated" + this.CodeFileExtension;
        }

        /// <summary>get the name of the file with the manual code</summary>
        public override string CalculateManualCodeFilename(string AYamlFilename)
        {
            return System.IO.Path.GetDirectoryName(AYamlFilename) +
                   System.IO.Path.DirectorySeparatorChar +
                   System.IO.Path.GetFileNameWithoutExtension(AYamlFilename) +
                   ".ManualCode" + this.CodeFileExtension;
        }

        /// <summary>
        /// call a function of the control
        /// </summary>
        /// <param name="AControlName"></param>
        /// <param name="AFunctionCall"></param>
        public override void CallControlFunction(string AControlName, string AFunctionCall)
        {
            SetControlProperty(AControlName, AFunctionCall,
                "this." + AControlName + "." + AFunctionCall);
        }

        /// <summary>
        /// control is a container
        /// </summary>
        /// <param name="AControlName"></param>
        public override void AddContainer(string AControlName)
        {
            FSuspendLayout += "this." + AControlName + ".SuspendLayout();" + Environment.NewLine;
            FResumePerformLayout = "this." + AControlName + ".ResumeLayout(false);" + Environment.NewLine + FResumePerformLayout;
        }

        /// returns the initialiseCode, see e.g. ProcessReportForm
        public virtual string ProcessDataSource(XmlNode curNode, string AControlName)
        {
            // todo: depends how the data source is connected; see example in WriteReportForm.cs
            return "";
        }

        /// <summary>
        /// dependancies of the controls on each other
        /// </summary>
        public SortedList FControlDataTypes = new SortedList();

        /// <summary>
        /// get the data
        /// </summary>
        /// <param name="curNode"></param>
        /// <param name="AControlName"></param>
        public override void InitialiseDataSource(XmlNode curNode, string AControlName)
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
                        ',',
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

        /// <summary>
        /// write the code for initialising the controls
        /// </summary>
        protected void FinishUpInitialisingControls()
        {
            // if no other control depends on a combobox, e.g. cmbPostalRegionRegion, don't require any code
            foreach (string dependsOn in FControlDataTypes.GetKeyList())
            {
                string currentContent = FTemplate.FCodelets["INITIALISE_" + dependsOn].ToString();

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
            PanelLayoutGenerator.countTableLayoutPanel = 0;
            PanelLayoutGenerator.ResetTabIndex();
        }

        /// <summary>
        /// is this describing a user control or a windows form?
        /// </summary>
        public override bool IsUserControlTemplate
        {
            get
            {
                return !FTemplate.FTemplateCode.Contains(": System.Windows.Forms.Form");
            }
        }

        private void AddRootControl(string prefix)
        {
            TControlDef ctrl = FCodeStorage.GetRootControl(prefix);

            IControlGenerator generator = FindControlGenerator(ctrl);

            if (generator.RequiresChildren)
            {
                // don't add toolbar if there are no toolbar buttons
                if (ctrl.NumberChildren == 0)
                {
                    return;
                }
            }

            generator.GenerateControl(this, ctrl);

            if (generator.AddControlToContainer)
            {
                FTemplate.AddToCodelet("ADDMAINCONTROLS", "this.Controls.Add(this." + ctrl.controlName + ");" + Environment.NewLine);
            }

            if ((prefix == "mnu")
                && !((FCodeStorage.GetAttribute("BaseClass").Contains("UserControl"))
                     || (FCodeStorage.GetAttribute("BaseClass").Contains("TGrpCollapsible"))))
            {
                FTemplate.AddToCodelet("ADDMAINCONTROLS", "this.MainMenuStrip = " + ctrl.controlName + ";" + Environment.NewLine);
            }
        }

        private void HandleWebConnector(string AFunctionType,
            string AMasterOrDetail,
            string ATableName,
            string AServerWebConnectorNamespace,
            List <TypeDeclaration>AWebConnectorClasses)
        {
            TypeDeclaration WebConnectorClass;

            MethodDeclaration MethodInWebConnector = CSParser.GetWebConnectorMethod(AWebConnectorClasses,
                AFunctionType,
                ATableName,
                out WebConnectorClass);

            if (MethodInWebConnector != null)
            {
                // get all parameters without VerificationResult
                bool HasVerification = false;
                string actualParameters = String.Empty;
                string actualParametersLocal = String.Empty;
                string formalParameters = String.Empty;
                bool firstParameter = true;

                foreach (ParameterDeclarationExpression p in MethodInWebConnector.Parameters)
                {
                    if (!firstParameter)
                    {
                        actualParameters += ", ";
                        actualParametersLocal += ", ";
                        formalParameters += ", ";
                    }

                    firstParameter = false;

                    if ((ParameterModifiers.Out & p.ParamModifier) > 0)
                    {
                        actualParameters += "out ";
                        actualParametersLocal += "out ";
                        formalParameters += "out ";
                    }

                    if ((ParameterModifiers.Ref & p.ParamModifier) > 0)
                    {
                        actualParameters += "ref ";
                        actualParametersLocal += "ref ";
                        formalParameters += "ref ";
                    }

                    if (p.TypeReference.Type == "TVerificationResultCollection")
                    {
                        HasVerification = true;
                    }
                    else
                    {
                        actualParameters += p.ParameterName;
                        actualParametersLocal += (p.ParameterName[0] == 'A' ? 'F' : p.ParameterName[0]) + p.ParameterName.Substring(1);
                        formalParameters += p.TypeReference.Type + " " + p.ParameterName;
                    }
                }

                FTemplate.AddToCodelet("CANFINDWEBCONNECTOR_" + AFunctionType.ToUpper() + AMasterOrDetail, "true");
                FTemplate.SetCodelet("WEBCONNECTOR" + AMasterOrDetail, "TRemote." +
                    AServerWebConnectorNamespace.Substring("Ict.Petra.Server.".Length));
                FTemplate.AddToCodelet(AFunctionType.ToUpper() + AMasterOrDetail + "_ACTUALPARAMETERS", actualParameters);
                FTemplate.AddToCodelet(AFunctionType.ToUpper() + AMasterOrDetail + "_ACTUALPARAMETERS_LOCAL", actualParametersLocal);
                FTemplate.AddToCodelet(AFunctionType.ToUpper() + AMasterOrDetail + "_FORMALPARAMETERS", formalParameters);

                if (HasVerification)
                {
                    FTemplate.AddToCodelet(AFunctionType.ToUpper() + AMasterOrDetail + "_WITHVERIFICATION", "true");
                }
                else
                {
                    FTemplate.AddToCodelet(AFunctionType.ToUpper() + AMasterOrDetail + "_WITHOUTVERIFICATION", "true");
                }
            }
        }

        private void HandleWebConnectors(string AGuiNamespace)
        {
            // AGuiNamespace is eg. Ict.Petra.Client.MFinance.Gui.AccountsPayable
            // ServerWebConnectorNamespace should be Ict.Petra.Server.MFinance.AccountsPayable.WebConnectors
            string ServerWebConnectorNamespace = AGuiNamespace.Replace("Gui.", "").Replace("Client", "Server") + ".WebConnectors";

            List <TypeDeclaration>WebConnectorClasses = CSParser.GetWebConnectorClasses(ServerWebConnectorNamespace);

            if (WebConnectorClasses != null)
            {
                HandleWebConnector("Create", "MASTER", FCodeStorage.GetAttribute("MasterTable"), ServerWebConnectorNamespace, WebConnectorClasses);
                HandleWebConnector("Create", "DETAIL", FCodeStorage.GetAttribute("DetailTable"), ServerWebConnectorNamespace, WebConnectorClasses);
                HandleWebConnector("Load", "MASTER", FCodeStorage.GetAttribute("MasterTable"), ServerWebConnectorNamespace, WebConnectorClasses);

                FTemplate.SetCodelet("WEBCONNECTORTDS", "TRemote." +
                    ServerWebConnectorNamespace.Substring("Ict.Petra.Server.".Length));
                FTemplate.SetCodelet("WEBCONNECTORTDS", "TRemote." +
                    ServerWebConnectorNamespace.Substring("Ict.Petra.Server.".Length));
            }
        }

        // Adds substitute code for {#DELETEREFERENCECOUNT} if the YAML file has a btnDelete
        // Otherwise the substitute text will be empty string
        private void AddDeleteReferenceCountImplementation()
        {
            try
            {
                // We need the module namespace in the server that contains the reference count code (eg MPartner)
                // This is not necessarily the module where the table comes in the Data Store
                // The generateORMReferenceCount code uses the Client yaml file location to choose the module namespace
                // It can do this because all the calls are static
                string[] dirItems = FCodeStorage.FManualCodeFilename.Split(new char[] { Path.DirectorySeparatorChar });
                string rootNamespace = String.Empty;

                // Find the last time Client is a folder name
                for (int i = 0; i < dirItems.Length - 1; i++)
                {
                    if (dirItems[i] == "Client")
                    {
                        rootNamespace = dirItems[i + 1];
                    }
                }

                ProcessTemplate singleSnippet = FTemplate.GetSnippet("SNIPDELETEREFERENCECOUNT");
                ProcessTemplate multiSnippet = FTemplate.GetSnippet("SNIPMULTIDELETEREFERENCECOUNT");
                singleSnippet.SetCodelet("CONNECTORNAMESPACE", rootNamespace);
                multiSnippet.SetCodelet("CONNECTORNAMESPACE", rootNamespace);

                string cacheableTableName = FCodeStorage.GetAttribute("CacheableTable");

                if (cacheableTableName != String.Empty)
                {
                    singleSnippet.SetCodelet("CACHEABLETABLENAME", cacheableTableName);
                    multiSnippet.SetCodelet("CACHEABLETABLENAME", cacheableTableName);
                }
                else
                {
                    singleSnippet.SetCodelet("NONCACHEABLETABLENAME", FCodeStorage.GetAttribute("DetailTable"));
                    multiSnippet.SetCodelet("NONCACHEABLETABLENAME", FCodeStorage.GetAttribute("DetailTable"));
                }

                FTemplate.InsertSnippet("DELETEREFERENCECOUNT", singleSnippet);
                FTemplate.InsertSnippet("MULTIDELETEREFERENCECOUNT", multiSnippet);
            }
            catch (KeyNotFoundException)
            {
                // If this exception fires, you are probably trying to add a delete implementation to a class that should not have it.
                // You may need to change the IF clause that calls this AddDeleteReferenceCountImplementation method in CreateCode below...
                Console.WriteLine("Warning : AddDeleteReferenceCountImplementation had KeyNotFoundException in {0}", FCodeStorage.FClassName);
            }
        }

        /// based on the code model, create the code;
        /// using the code generators that have been loaded
        public override void CreateCode(TCodeStorage ACodeStorage, string AXAMLFilename, string ATemplateFile)
        {
            ResetAllValues();
            FCodeStorage = ACodeStorage;
            TControlGenerator.FCodeStorage = ACodeStorage;
            FTemplate = new ProcessTemplate(ATemplateFile);

            // load default header with license and copyright
            string templateDir = TAppSettingsManager.GetValue("TemplateDir", true);
            FTemplate.AddToCodelet("GPLFILEHEADER",
                ProcessTemplate.LoadEmptyFileComment(templateDir + Path.DirectorySeparatorChar + ".." +
                    Path.DirectorySeparatorChar));

            // init some template variables that can be empty
            FTemplate.AddToCodelet("INITUSERCONTROLS", "");
            FTemplate.AddToCodelet("INITMANUALCODE", "");
            FTemplate.AddToCodelet("GRIDMULTISELECTION", "");
            FTemplate.AddToCodelet("RUNONCEONACTIVATIONMANUAL", "");
            FTemplate.AddToCodelet("EXITMANUALCODE", "");
            FTemplate.AddToCodelet("CANCLOSEMANUAL", "");
            FTemplate.AddToCodelet("INITNEWROWMANUAL", "");
            FTemplate.AddToCodelet("DELETERECORD", "");
            FTemplate.AddToCodelet("DELETEREFERENCECOUNT", "");
            FTemplate.AddToCodelet("MULTIDELETEREFERENCECOUNT", "");
            FTemplate.AddToCodelet("ENABLEDELETEBUTTON", "");
            FTemplate.AddToCodelet("CANDELETESELECTION", "");
            FTemplate.AddToCodelet("PREDELETEMANUAL", "");
            FTemplate.AddToCodelet("DELETEROWMANUAL", "");
            FTemplate.AddToCodelet("POSTDELETEMANUAL", "");
            FTemplate.AddToCodelet("PREMULTIDELETEMANUAL", "");
            FTemplate.AddToCodelet("DELETEMULTIROWMANUAL", "");
            FTemplate.AddToCodelet("POSTMULTIDELETEMANUAL", "");
            FTemplate.AddToCodelet("MULTIDELETEDELETABLE", "");
            FTemplate.AddToCodelet("SELECTTABMANUAL", "");
            FTemplate.AddToCodelet("STOREMANUALCODE", "");
            FTemplate.AddToCodelet("FINDANDFILTERHOOKUPEVENTS", "");
            FTemplate.AddToCodelet("ACTIONENABLINGDISABLEMISSINGFUNCS", "");
            FTemplate.AddToCodelet("PRIMARYKEYCONTROLSREADONLY", "");
            FTemplate.AddToCodelet("SHOWDETAILSMANUAL", "");
            FTemplate.AddToCodelet("CLEARDETAILS", "");
            FTemplate.AddToCodelet("CATALOGI18N", "");
            FTemplate.AddToCodelet("DYNAMICTABPAGEUSERCONTROLENUM", "");
            FTemplate.AddToCodelet("DYNAMICTABPAGEUSERCONTROLINITIALISATION", "");
            FTemplate.AddToCodelet("DYNAMICTABPAGEUSERCONTROLLOADING", "");
            FTemplate.AddToCodelet("ASSIGNFONTATTRIBUTES", "");
            FTemplate.AddToCodelet("CUSTOMDISPOSING", "");
            FTemplate.AddToCodelet("DYNAMICTABPAGEUSERCONTROLDECLARATION", "");
            FTemplate.AddToCodelet("DYNAMICTABPAGEBASICS", "");
            FTemplate.AddToCodelet("IGNOREFIRSTTABPAGESELECTIONCHANGEDEVENT", "");
            FTemplate.AddToCodelet("DYNAMICTABPAGEUSERCONTROLSETUPMETHODS", "");
            FTemplate.AddToCodelet("ELSESTATEMENT", "");
//            FTemplate.AddToCodelet("VALIDATEDETAILS", "");

            if (FCodeStorage.ManualFileExistsAndContains("void BeforeShowDetailsManual"))
            {
                FTemplate.AddToCodelet("SHOWDETAILS", "BeforeShowDetailsManual(ARow);" + Environment.NewLine);
            }

            FTemplate.AddToCodelet("INITACTIONSTATE", "FPetraUtilsObject.InitActionState();" + Environment.NewLine);

            if (FCodeStorage.ManualFileExistsAndContains("InitializeManualCode"))
            {
                FTemplate.AddToCodelet("INITMANUALCODE", "InitializeManualCode();" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("RunOnceOnActivationManual"))
            {
                FTemplate.AddToCodelet("RUNONCEONACTIVATIONMANUAL", "RunOnceOnActivationManual();" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("ExitManualCode"))
            {
                FTemplate.AddToCodelet("EXITMANUALCODE", "ExitManualCode();" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("CanCloseManual"))
            {
                FTemplate.AddToCodelet("CANCLOSEMANUAL", " && CanCloseManual()");
            }

            if (FCodeStorage.ManualFileExistsAndContains("NewRowManual"))
            {
                FTemplate.AddToCodelet("INITNEWROWMANUAL", "NewRowManual(ref NewRow);" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("PreDeleteManual"))
            {
                FTemplate.AddToCodelet("PREDELETEMANUAL",
                    "AllowDeletion = PreDeleteManual(FPreviouslySelectedDetailRow, ref DeletionQuestion);" + Environment.NewLine);
                FTemplate.AddToCodelet("PREMULTIDELETEMANUAL",
                    "AllowDeletion = PreDeleteManual(rowToDelete, ref DeletionQuestion);" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("DeleteRowManual"))
            {
                FTemplate.AddToCodelet("DELETEROWMANUAL",
                    "DeletionPerformed = DeleteRowManual(FPreviouslySelectedDetailRow, ref CompletionMessage);" + Environment.NewLine);
                FTemplate.AddToCodelet("DELETEMULTIROWMANUAL",
                    "DeletionPerformed = AllowDeletion && DeleteRowManual(rowToDelete, ref CompletionMessage);" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("PostDeleteManual"))
            {
                FTemplate.AddToCodelet("POSTDELETEMANUAL",
                    "PostDeleteManual(FPreviouslySelectedDetailRow, AllowDeletion, DeletionPerformed, CompletionMessage);" + Environment.NewLine);
                FTemplate.AddToCodelet("POSTMULTIDELETEMANUAL",
                    "PostDeleteManual(rowToDelete, AllowDeletion, DeletionPerformed, String.Empty);" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("SelectTabManual"))
            {
                FTemplate.AddToCodelet("SELECTTABMANUAL",
                    "//Call code to execute on selection of new tab" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("StoreManualCode"))
            {
                FTemplate.AddToCodelet("STOREMANUALCODE",
                    "SubmissionResult = StoreManualCode(ref SubmitDS, out VerificationResult);" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("FindAndFilterHookUpEvents"))
            {
                FTemplate.AddToCodelet("FINDANDFILTERHOOKUPEVENTS", "FindAndFilterHookUpEvents();" + Environment.NewLine);
            }

            if (FCodeStorage.HasAttribute("DatasetType"))
            {
                FTemplate.SetCodelet("DATASETTYPE", FCodeStorage.GetAttribute("DatasetType"));
                FTemplate.SetCodelet("SHORTDATASETTYPE",
                    FCodeStorage.GetAttribute("DatasetType").Substring(FCodeStorage.GetAttribute("DatasetType").LastIndexOf(".") + 1));
                FTemplate.SetCodelet("MANAGEDDATASETORTYPE", "true");
            }
            else
            {
                FTemplate.SetCodelet("DATASETTYPE", String.Empty);
            }

//    FTemplate.SetCodelet("MANAGEDDATASETORTYPE", "true");

            XmlNode UsingNamespacesNode = TYml2Xml.GetChild(FCodeStorage.FRootNode, "UsingNamespaces");

            if (UsingNamespacesNode != null)
            {
                foreach (string s in TYml2Xml.GetElements(FCodeStorage.FRootNode, "UsingNamespaces"))
                {
                    FTemplate.AddToCodelet("USINGNAMESPACES", "using " + s + ";" + Environment.NewLine);
                }
            }
            else
            {
                FTemplate.AddToCodelet("USINGNAMESPACES", "");
            }

            SortedList <string, TTable>DataSetTables = null;

            // load the dataset if there is a dataset defined for this screen. this allows us to reference customtables and custom fields
            if (FCodeStorage.HasAttribute("DatasetType"))
            {
                DataSetTables = TDataBinding.LoadDatasetTables(CSParser.ICTPath, FCodeStorage.GetAttribute("DatasetType"), FCodeStorage);
            }
            else
            {
                TDataBinding.FCodeStorage = FCodeStorage;
                TDataBinding.ResetCurrentDataset();
            }

            if (FCodeStorage.HasAttribute("MasterTable"))
            {
                if ((DataSetTables != null) && DataSetTables.ContainsKey(FCodeStorage.GetAttribute("MasterTable")))
                {
                    TTable table = DataSetTables[FCodeStorage.GetAttribute("MasterTable")];
                    FTemplate.AddToCodelet("MASTERTABLE", table.strVariableNameInDataset);
                    FTemplate.AddToCodelet("MASTERTABLETYPE", table.strDotNetName);
                    FTemplate.SetCodelet("SHAREDVALIDATIONNAMESPACEMODULE",
                        String.Format("Ict.Petra.Shared.{0}.Validation",
                            TTable.GetNamespace(table.strGroup)));
                }
                else
                {
                    FTemplate.AddToCodelet("MASTERTABLE", FCodeStorage.GetAttribute("MasterTable"));

                    TTable table = TDataBinding.FPetraXMLStore.GetTable(FCodeStorage.GetAttribute("MasterTable"));
                    FTemplate.SetCodelet("SHAREDVALIDATIONNAMESPACEMODULE",
                        String.Format("Ict.Petra.Shared.{0}.Validation",
                            TTable.GetNamespace(table.strGroup)));

                    if (FCodeStorage.HasAttribute("MasterTableType"))
                    {
                        FTemplate.AddToCodelet("MASTERTABLETYPE", FCodeStorage.GetAttribute("MasterTableType"));
                    }
                    else
                    {
                        FTemplate.AddToCodelet("MASTERTABLETYPE", FCodeStorage.GetAttribute("MasterTable"));
                    }
                }
            }
            else
            {
                FTemplate.AddToCodelet("MASTERTABLE", "");
                FTemplate.AddToCodelet("MASTERTABLETYPE", "");
            }

            if (FCodeStorage.HasAttribute("DetailTable"))
            {
                string detailTable;

                if ((DataSetTables != null) && DataSetTables.ContainsKey(FCodeStorage.GetAttribute("DetailTable")))
                {
                    TTable table = DataSetTables[FCodeStorage.GetAttribute("DetailTable")];

                    if (table == null)
                    {
                        throw new Exception("invalid DetailTable, does not exist: " + FCodeStorage.GetAttribute("DetailTable"));
                    }

                    FTemplate.SetCodelet("SHAREDVALIDATIONNAMESPACEMODULE",
                        String.Format("Ict.Petra.Shared.{0}.Validation",
                            TTable.GetNamespace(table.strGroup)));

                    detailTable = table.strVariableNameInDataset;
                    FTemplate.AddToCodelet("DETAILTABLE", detailTable);
                    FTemplate.AddToCodelet("DETAILTABLETYPE", table.strDotNetName);
                }
                else
                {
                    detailTable = FCodeStorage.GetAttribute("DetailTable");
                    FTemplate.AddToCodelet("DETAILTABLE", detailTable);
                    FTemplate.AddToCodelet("DETAILTABLETYPE", detailTable);

                    TTable table = TDataBinding.FPetraXMLStore.GetTable(detailTable);
                    FTemplate.SetCodelet("SHAREDVALIDATIONNAMESPACEMODULE",
                        String.Format("Ict.Petra.Shared.{0}.Validation",
                            TTable.GetNamespace(table.strGroup)));
                }

                if ((FTemplate.FSnippets.Keys.Contains("INLINETYPEDDATASET"))
                    && (!FCodeStorage.HasAttribute("DatasetType")))
                {
                    ProcessTemplate inlineTypedDataSetSnippet = FTemplate.GetSnippet("INLINETYPEDDATASET");
                    inlineTypedDataSetSnippet.SetCodelet("DETAILTABLE", detailTable);

                    FTemplate.InsertSnippet("INLINETYPEDDATASET", inlineTypedDataSetSnippet);
                }
            }
            else
            {
                FTemplate.AddToCodelet("DETAILTABLE", "");
                FTemplate.AddToCodelet("DETAILTABLETYPE", "");
            }

            if (FCodeStorage.HasAttribute("TempTableName"))
            {
                FTemplate.AddToCodelet("TEMPTABLENAME", FCodeStorage.GetAttribute("TempTableName"));
            }

            if (FCodeStorage.HasAttribute("CacheableTable"))
            {
                FTemplate.AddToCodelet("CACHEABLETABLE", "\"" + FCodeStorage.GetAttribute("CacheableTable") + "\"");

                if (FCodeStorage.HasAttribute("CacheableTableSpecificFilter"))
                {
                    FTemplate.AddToCodelet("FILTERVAR", "object FFilter;");

                    string CacheableTableSpecificFilter = FCodeStorage.GetAttribute("CacheableTableSpecificFilter");

                    if (CacheableTableSpecificFilter.StartsWith("\""))
                    {
                        FTemplate.AddToCodelet("LOADDATAONCONSTRUCTORRUN", "LoadDataAndFinishScreenSetup();");
                        FTemplate.AddToCodelet("CACHEABLETABLERETRIEVEMETHOD", "GetCacheableDataTableFromCache");
                        FTemplate.AddToCodelet("DISPLAYFILTERINFORMTITLE", "this.Text = this.Text + \"   [Filtered]\";");
                        FTemplate.AddToCodelet("CACHEABLETABLESPECIFICFILTERLOAD", "" + CacheableTableSpecificFilter + ", FFilter");
                        FTemplate.AddToCodelet("CACHEABLETABLESPECIFICFILTERSAVE", ", " + CacheableTableSpecificFilter + ", FFilter");
                    }
                    else
                    {
                        FTemplate.AddToCodelet("LOADDATAONCONSTRUCTORRUN",
                            "// LoadDataAndFinishScreenSetup() needs to be called manually after setting FFilter!");
                        FTemplate.AddToCodelet("CACHEABLETABLERETRIEVEMETHOD", "GetSpecificallyFilteredCacheableDataTableFromCache");
                        FTemplate.AddToCodelet("DISPLAYFILTERINFORMTITLE",
                            "this.Text = this.Text + \"   [" + CacheableTableSpecificFilter + " = \" + FFilter.ToString() + \"]\";");
                        FTemplate.AddToCodelet("CACHEABLETABLESPECIFICFILTERLOAD", "\"" + CacheableTableSpecificFilter + "\", FFilter");
                        FTemplate.AddToCodelet("CACHEABLETABLESPECIFICFILTERSAVE", ", FFilter");
                    }

                    FTemplate.AddToCodelet("CACHEABLETABLESAVEMETHOD", "SaveSpecificallyFilteredCacheableDataTableToPetraServer");
                    //FTemplate.AddToCodelet("CACHEABLETABLESPECIFICFILTERSAVE", "FFilter, out VerificationResult");
                }
                else
                {
                    FTemplate.AddToCodelet("FILTERVAR", "");
                    FTemplate.AddToCodelet("DISPLAYFILTERINFORMTITLE", "");
                    FTemplate.AddToCodelet("LOADDATAONCONSTRUCTORRUN", "LoadDataAndFinishScreenSetup();");
                    FTemplate.AddToCodelet("CACHEABLETABLERETRIEVEMETHOD", "GetCacheableDataTableFromCache");
                    FTemplate.AddToCodelet("CACHEABLETABLESPECIFICFILTERLOAD", "String.Empty, null");
                    FTemplate.AddToCodelet("CACHEABLETABLESPECIFICFILTERSAVE", "");
                    FTemplate.AddToCodelet("CACHEABLETABLESAVEMETHOD", "SaveChangedCacheableDataTableToPetraServer");
                    //FTemplate.AddToCodelet("CACHEABLETABLESPECIFICFILTERSAVE", "out VerificationResult");
                }
            }

            if (FCodeStorage.HasAttribute("GenerateGetSelectedDetailRow")
                && (FCodeStorage.GetAttribute("GenerateGetSelectedDetailRow") == "true"))
            {
                FTemplate.AddToCodelet("GENERATEGETSELECTEDDETAILROW", "true");
            }

            if (FCodeStorage.HasAttribute("MultipleMasterRows")
                && (FCodeStorage.GetAttribute("MultipleMasterRows") == "true"))
            {
                FTemplate.AddToCodelet("MULTIPLEMASTERROWS", "true");
            }

            // Note - this IF clause needs to be the same as the one in generateReferenceCountConnectors.cs which is generating the server side code
            // Do Ctrl+F to find: this IF clause needs to be the same
            // in that file
            if ((FCodeStorage.GetAttribute("DetailTable") != String.Empty)
                && (FCodeStorage.FControlList.ContainsKey("btnDelete")
                    || FCodeStorage.FControlList.ContainsKey("btnDeleteType")
                    || FCodeStorage.FControlList.ContainsKey("btnDeleteExtract")
                    || FCodeStorage.FControlList.ContainsKey("btnDeleteDetail")
                    || (FCodeStorage.FControlList.ContainsKey("btnRemoveDetail") && (FCodeStorage.GetAttribute("FormType") != "report"))))
            {
                // We always auto-generate code to calculate the record reference count when a delete button exists unless specified in YAML
                if (TYml2Xml.GetAttribute((XmlNode)FCodeStorage.FXmlNodes["actDelete"], "SkipReferenceCheck").ToLower() != "true")
                {
                    AddDeleteReferenceCountImplementation();
                }

                // The generated code only writes the delete button event handler if there is a delete button and there is no manual code to handle the event
                if ((FCodeStorage.FActionList.ContainsKey("actDelete") && (FCodeStorage.FActionList["actDelete"].actionClick != "DeleteRecord"))
                    || FCodeStorage.ManualFileExistsAndContains("btnDelete.Click +=")
                    || FCodeStorage.ManualFileExistsAndContains("void DeleteRecord(")
                    || FCodeStorage.ManualFileExistsAndContains("void DeleteDetail(")
                    || FCodeStorage.ManualFileExistsAndContains("void DeleteRow(")
                    || FCodeStorage.ManualFileExistsAndContains("void RemoveDetail("))
                {
                    // Handled by manual code
                    Console.WriteLine("Skipping DeleteRecord() handler because it is handled by " +
                        Path.GetFileNameWithoutExtension(FCodeStorage.FManualCodeFilename));
                }
                else
                {
                    // Write the event handler to call the DeleteTableName() method
                    string deleteRecord = String.Format(
                        "{1}{0}private void DeleteRecord(Object sender, EventArgs e){1}{0}{{{1}{0}{0}Delete{2}();{1}{0}}}{1}{1}",
                        "    ",
                        Environment.NewLine,
                        FCodeStorage.GetAttribute("DetailTable"));
                    FTemplate.AddToCodelet("DELETERECORD", deleteRecord);

                    ProcessTemplate snippetMultiDelete = FTemplate.GetSnippet("SNIPMULTIDELETEDELETABLE");
                    ProcessTemplate snippetCanDelete = FTemplate.GetSnippet("SNIPCANDELETESELECTION");
                    bool bRequiresCanDeleteSelection = false;

                    // Write the one-line codelet that handles enable/disable of the delete button
                    string enableDelete = "btnDelete.Enabled = pnlDetails.Enabled";
                    string enableDeleteExtra = " && CanDeleteSelection()";

                    if (FCodeStorage.FControlList.ContainsKey("chkDetailDeletable")
                        || FCodeStorage.FControlList.ContainsKey("chkDeletable"))
                    {
                        enableDelete += enableDeleteExtra;
                        snippetMultiDelete.SetCodelet("DELETEABLEFLAG", "Deletable");
                        FTemplate.InsertSnippet("MULTIDELETEDELETABLE", snippetMultiDelete);
                        snippetCanDelete.SetCodelet("DELETEABLEFLAG", "Deletable");
                        bRequiresCanDeleteSelection = true;
                    }
                    else if (FCodeStorage.FControlList.ContainsKey("chkDetailDeletableFlag")
                             || FCodeStorage.FControlList.ContainsKey("chkDeletableFlag"))
                    {
                        enableDelete += enableDeleteExtra;
                        snippetMultiDelete.SetCodelet("DELETEABLEFLAG", "DeletableFlag");
                        FTemplate.InsertSnippet("MULTIDELETEDELETABLE", snippetMultiDelete);
                        snippetCanDelete.SetCodelet("DELETEABLEFLAG", "DeletableFlag");
                        bRequiresCanDeleteSelection = true;
                    }
                    else if (FCodeStorage.FControlList.ContainsKey("chkDetailTypeDeletable"))
                    {
                        enableDelete += enableDeleteExtra;
                        snippetMultiDelete.SetCodelet("DELETEABLEFLAG", "TypeDeletable");
                        FTemplate.InsertSnippet("MULTIDELETEDELETABLE", snippetMultiDelete);
                        snippetCanDelete.SetCodelet("DELETEABLEFLAG", "TypeDeletable");
                        bRequiresCanDeleteSelection = true;
                    }

                    enableDelete += ";" + Environment.NewLine;
                    FTemplate.AddToCodelet("ENABLEDELETEBUTTON", enableDelete);

                    if (bRequiresCanDeleteSelection)
                    {
                        snippetCanDelete.SetCodelet("DETAILTABLE", FCodeStorage.GetAttribute("DetailTable"));
                        FTemplate.InsertSnippet("CANDELETESELECTION", snippetCanDelete);
                    }
                }
            }

            // find the first control that is a panel or groupbox or tab control
            if (FCodeStorage.HasRootControl("content"))
            {
                AddRootControl("content");
            }

            // Toolbar
            if (FCodeStorage.HasRootControl("tbr"))
            {
                AddRootControl("tbr");
            }

            // Main Menu
            if (FCodeStorage.HasRootControl("mnu"))
            {
                AddRootControl("mnu");
            }

            // Statusbar
            if (FCodeStorage.HasRootControl("stb"))
            {
                AddRootControl("stb");
            }

            // check for controls that have no parent
            List <TControlDef>orphans = FCodeStorage.GetOrphanedControls();

            if (orphans.Count > 0)
            {
                string msg = String.Empty;

                foreach (TControlDef o in orphans)
                {
                    msg += o.controlName + " ";
                }

                TLogging.Log("Warning !!! There are some controls that will not be part of the screen (missing parent control?): " + msg);
            }

            // add form events
            foreach (TEventHandler handler in FCodeStorage.FEventList.Values)
            {
                SetEventHandlerForForm(handler);
            }

            foreach (TReportParameter ReportPara in FCodeStorage.FReportParameterList.Values)
            {
                AddReportParameterImplementaion(ReportPara);
            }

            XmlNode rootNode = (XmlNode)FCodeStorage.FXmlNodes[TParseYAMLFormsDefinition.ROOTNODEYML];

            if (TYml2Xml.HasAttribute(rootNode, "UIConnectorType") && TYml2Xml.HasAttribute(rootNode, "UIConnectorCreate"))
            {
                FTemplate.AddToCodelet("UICONNECTORTYPE", TYml2Xml.GetAttribute(rootNode, "UIConnectorType"));
                FTemplate.AddToCodelet("UICONNECTORCREATE", TYml2Xml.GetAttribute(rootNode, "UIConnectorCreate"));
            }

            HandleWebConnectors(TYml2Xml.GetAttribute(rootNode, "Namespace"));

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
                    string actionEnabling =
                        "if (e.ActionName == \"" + handler.actionName + "\")" + Environment.NewLine +
                        "{" + Environment.NewLine +
                        "    {#ENABLEDEPENDINGACTIONS_" + handler.actionName + "}" + Environment.NewLine +
                        "}" + Environment.NewLine + Environment.NewLine;
                    Template.AddToCodelet("ACTIONENABLING", actionEnabling);
                    Template.AddToCodelet(
                        "INITACTIONSTATE",
                        "ActionEnabledEvent(null, new ActionEventArgs(" +
                        "\"" + handler.actionName + "\", " + TYml2Xml.GetAttribute(handler.actionNode,
                            "InitialValue") + "));" + Environment.NewLine);
                }

                if (TYml2Xml.HasAttribute(handler.actionNode, "Enabled"))
                {
                    Template.AddToCodelet(
                        "ENABLEDEPENDINGACTIONS_" + TYml2Xml.GetAttribute(handler.actionNode, "Enabled"),
                        "FPetraUtilsObject.EnableAction(\"" + handler.actionName + "\", e.Enabled);" + Environment.NewLine);
                }
            }

            WriteAllControls();

            FinishUpInitialisingControls();

            if (FCodeStorage.ManualFileExistsAndContains("void ShowDataManual()"))
            {
                FTemplate.AddToCodelet("SHOWDATA", "ShowDataManual();" + Environment.NewLine);
            }
            else if (FCodeStorage.ManualFileExistsAndContains("void ShowDataManual("))
            {
                FTemplate.AddToCodelet("SHOWDATA", "ShowDataManual(ARow);" + Environment.NewLine);
            }

            if (FCodeStorage.FControlList.ContainsKey("pnlDetails"))
            {
                FTemplate.AddToCodelet("CLEARDETAILS", "FPetraUtilsObject.ClearControls(pnlDetails);" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("void ShowDetailsManual"))
            {
                FTemplate.AddToCodelet("SHOWDETAILS", "ShowDetailsManual(ARow);" + Environment.NewLine);
                FTemplate.AddToCodelet("CLEARDETAILS", "ShowDetailsManual(ARow);" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("GetDataFromControlsManual()"))
            {
                FTemplate.AddToCodelet("SAVEDATA", "GetDataFromControlsManual();" + Environment.NewLine);
            }
            else if (FCodeStorage.ManualFileExistsAndContains("GetDataFromControlsManual("))
            {
                FTemplate.AddToCodelet("SAVEDATA", "GetDataFromControlsManual(ARow);" + Environment.NewLine);
            }

//            if (FCodeStorage.ManualFileExistsAndContains("ValidateDetailsManual"))
//            {
//                ProcessTemplate validateSnippet = FTemplate.GetSnippet("VALIDATEDETAILS");
//                FTemplate.InsertSnippet("VALIDATEDETAILS", validateSnippet);
//            }
//
            if (FCodeStorage.ManualFileExistsAndContains("GetDetailDataFromControlsManual"))
            {
                FTemplate.AddToCodelet("SAVEDETAILS", "GetDetailDataFromControlsManual(ARow);" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("void ReadControlsManual"))
            {
                FTemplate.AddToCodelet("READCONTROLS", "ReadControlsManual(ACalc, AReportAction);" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("void SetControlsManual"))
            {
                FTemplate.AddToCodelet("SETCONTROLS", "SetControlsManual(AParameters);" + Environment.NewLine);
            }

            if (FCodeStorage.ManualFileExistsAndContains("ValidateDataManual"))
            {
                FTemplate.SetCodelet("VALIDATEDATAMANUAL", "true");
            }

            if (FCodeStorage.ManualFileExistsAndContains("ValidateDataDetailsManual"))
            {
                FTemplate.SetCodelet("VALIDATEDATADETAILSMANUAL", "true");
            }

            if (FCodeStorage.ManualFileExistsAndContains("GetSelectedDetailRowManual"))
            {
                FTemplate.AddToCodelet("GETSELECTEDDETAILROW", "return GetSelectedDetailRowManual();" + Environment.NewLine);
            }
            else
            {
                string getSelectedDetailRow =
                    "DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;" + Environment.NewLine + Environment.NewLine +
                    "if (SelectedGridRow.Length >= 1)" + Environment.NewLine +
                    "{" + Environment.NewLine +
                    "    return ({#DETAILTABLETYPE}Row)SelectedGridRow[0].Row;" + Environment.NewLine +
                    "}" + Environment.NewLine + Environment.NewLine +
                    "return null;" + Environment.NewLine + Environment.NewLine;

                FTemplate.SetCodelet("GETSELECTEDDETAILROW", getSelectedDetailRow);
            }

            InsertCodeIntoTemplate(AXAMLFilename);
        }

        /// <summary>
        /// insert all variables into the template
        /// </summary>
        /// <param name="AXAMLFilename"></param>
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
            FTemplate.ReplacePlaceHolder("ADDAVAILABLEFUNCTIONS", FCodeStorage.FReportParametersImplementation);

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
    }
}