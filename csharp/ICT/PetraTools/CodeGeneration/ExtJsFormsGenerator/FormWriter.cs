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

            InsertCodeIntoTemplate(AXAMLFilename);
        }

        public virtual void InsertCodeIntoTemplate(string AXAMLFilename)
        {
            // TODO
        }
    }
}