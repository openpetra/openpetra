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
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;

namespace Ict.Tools.CodeGeneration
{
    /// <summary>
    /// This is the abstract base class for all other form writers.
    /// manages the code generators for all the controls
    /// </summary>
    public abstract class TFormWriter
    {
        /// <summary>main function for generating the code</summary>
        public abstract void CreateCode(TCodeStorage AStorage, string AXamlFilename, string ATemplate);
        /// <summary>create the file with the resources</summary>
        public abstract void CreateResourceFile(string AResourceFile, string AResourceTemplate);
        /// <summary>get the name of the designer file</summary>
        public abstract void CreateDesignerFile(string AYamlFilename, XmlNode ARootNode, string ATemplateDir);
        /// <summary>get the name of the destination generated file</summary>
        public abstract string CalculateDestinationFilename(string AYamlFilename);
        /// <summary>get the name of the file with the manual code</summary>
        public abstract string CalculateManualCodeFilename(string AYamlFilename);
        /// <summary>remove the current value of a control property</summary>
        public virtual void ClearControlProperty(string AControlName, string APropertyName)
        {
        }

        /// <summary>get the property of a control</summary>
        public virtual string GetControlProperty(string AControlName, string APropertyName)
        {
            return string.Empty;
        }

        /// <summary>set the property of a control</summary>
        public abstract void SetControlProperty(string AControlName, string APropertyName, string APropertyValue, bool ACreateTranslationForLabel);
        /// <summary>set the property of a control</summary>
        public void SetControlProperty(TControlDef ACtrl, string APropertyName, string APropertyValue)
        {
            SetControlProperty(ACtrl.controlName, APropertyName, APropertyValue, !(ACtrl.GetAttribute("VariableLabelText") == "true"));
        }

        /// <summary>
        /// check if the control has an attribute with the property name in the xml definition
        /// if such an attribute exists, then set it
        /// </summary>
        /// <param name="ACtrl"></param>
        /// <param name="APropertyName"></param>
        public virtual void SetControlProperty(TControlDef ACtrl, string APropertyName)
        {
            if (TYml2Xml.HasAttribute(ACtrl.xmlNode, APropertyName))
            {
                SetControlProperty(ACtrl, APropertyName, TYml2Xml.GetAttribute(ACtrl.xmlNode, APropertyName));
            }
        }

        /// <summary>
        /// for special functionality specific to a control
        /// </summary>
        public virtual void ApplyDerivedFunctionality(IControlGenerator generator, TControlDef control)
        {
            generator.ApplyDerivedFunctionality(this, control);
        }

        /// <summary>create code for calling a function from an event on the control</summary>
        public abstract void CallControlFunction(string AControlName, string AFunctionCall);
        /// <summary>install an event handler for a control</summary>
        public abstract void SetEventHandlerToControl(string AControlName, string AEvent, string AEventHandlerType, string AEventHandlingMethod);
        /// <summary>event handler</summary>
        public abstract void SetEventHandlerFunction(string AControlName, string AEvent, string AEventImplementation);
        /// <summary>add control to a container</summary>
        public abstract void AddContainer(string AControlName);
        /// <summary>create a resource from an image and add to designer</summary>
        public abstract void AddImageToResource(string AControlName, string AImageName, string AImageOrIcon);
        /// <summary>get the data for the form</summary>
        public abstract void InitialiseDataSource(XmlNode curNode, string AControlName);

        /// <summary>
        /// get the extension of the file to be written
        /// </summary>
        public abstract string CodeFileExtension
        {
            get;
        }

        /// <summary>
        /// manage the code objects for this file to be written
        /// </summary>
        public TCodeStorage FCodeStorage = null;

        /// <summary>
        /// the template for this file to be written
        /// </summary>
        public ProcessTemplate FTemplate;

        /// <summary>
        /// get the storage of this code
        /// </summary>
        public TCodeStorage CodeStorage
        {
            get
            {
                return FCodeStorage;
            }
        }

        /// <summary>
        /// get the template code
        /// </summary>
        public ProcessTemplate Template
        {
            get
            {
                return FTemplate;
            }
        }

        /// <summary>
        /// is this a template for a user control
        /// </summary>
        public abstract bool IsUserControlTemplate
        {
            get;
        }

        /// List of all available controls, with prefix and
        ///    function to find out if this fits (e.g. same prefixes for same control)
        private List <IControlGenerator>AvailableControlGenerators = new List <IControlGenerator>();

        /// <summary>
        /// add a control generator so that it can be used for the form generation
        /// </summary>
        /// <param name="AControlGeneratorType"></param>
        public void AddControlGenerator(IControlGenerator AControlGeneratorType)
        {
            AvailableControlGenerators.Add(AControlGeneratorType);
        }

        System.Type FBaseControlGeneratorType = null;

        /// <summary>
        /// this is the type of TControlGenerator. FindControlGenerator uses this base type to generate a generic control generator
        /// </summary>
        public System.Type BaseControlGeneratorType
        {
            set
            {
                FBaseControlGeneratorType = value;
            }
        }

        /// <summary>
        /// get the correct control generator for the control, depending on the prefix of the name, and other parameters
        /// </summary>
        /// <param name="ACtrlDef"></param>
        /// <returns></returns>
        public IControlGenerator FindControlGenerator(TControlDef ACtrlDef)
        {
            IControlGenerator fittingGenerator = null;

            if (ACtrlDef.controlGenerator != null)
            {
                return ACtrlDef.controlGenerator;
            }

            foreach (IControlGenerator generator in AvailableControlGenerators)
            {
                if (generator.ControlFitsNode(ACtrlDef.xmlNode))
                {
                    if (fittingGenerator != null)
                    {
                        throw new Exception(
                            "Error: control with name " + ACtrlDef.xmlNode.Name + " does fit both control generators " +
                            fittingGenerator.ControlType +
                            " and " +
                            generator.ControlType);
                    }

                    fittingGenerator = generator;
                }
            }

            if ((fittingGenerator == null)
                && (!ACtrlDef.controlName.StartsWith("Empty")))
            {
                if (TYml2Xml.HasAttribute(ACtrlDef.xmlNode, "Type") && (FBaseControlGeneratorType != null))
                {
                    return (IControlGenerator)Activator.CreateInstance(FBaseControlGeneratorType, new Object[] { ACtrlDef.xmlNode.Name.Substring(0,
                                                                                                                     3),
                                                                                                                 TYml2Xml.GetAttribute(ACtrlDef.
                                                                                                                     xmlNode, "Type") });
                }

                throw new Exception("Error: cannot find a generator for control with name " + ACtrlDef.xmlNode.Name);
            }

            ACtrlDef.controlGenerator = fittingGenerator;

            return fittingGenerator;
        }

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

            if (StringHelper.TryStrToInt32(ALabelText, -1).ToString() == ALabelText)
            {
                // don't translate digits?
                return false;
            }

            // careful with \n and \r in the string; that is not allowed by gettext
            if (ALabelText.Contains("\\r") || ALabelText.Contains("\\n"))
            {
                throw new Exception("Problem with \\r or \\n");
            }

            return true;
        }

        /// <summary>
        /// write the template to the destination file
        /// </summary>
        /// <param name="ADestinationFile"></param>
        /// <returns></returns>
        public bool WriteFile(string ADestinationFile)
        {
            return FTemplate.FinishWriting(ADestinationFile, Path.GetExtension(ADestinationFile), true);
        }

        /// <summary>
        /// write the resulting file, using the given template
        /// </summary>
        /// <param name="ADestinationFile"></param>
        /// <param name="ATemplateFile"></param>
        /// <returns></returns>
        public bool WriteFile(string ADestinationFile, string ATemplateFile)
        {
            ProcessTemplate LocalTemplate = new ProcessTemplate(ATemplateFile);

            // reuse the codelets that have been generated by CreateCode
            LocalTemplate.FCodelets = FTemplate.FCodelets;
            return LocalTemplate.FinishWriting(ADestinationFile, Path.GetExtension(ADestinationFile), true);
        }
    }
}