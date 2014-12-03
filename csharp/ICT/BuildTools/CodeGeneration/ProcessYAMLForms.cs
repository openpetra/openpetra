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
using Ict.Common;
using Ict.Common.IO;

namespace Ict.Tools.CodeGeneration
{
    /// <summary>
    /// This class is responsible of parsing the YML file,
    /// and writing the resulting forms file
    /// </summary>
    public class TProcessYAMLForms
    {
        String FYamlFilename;
        String FSelectedLocalisation = null;
        SortedList FFormTypes = new SortedList();

        /// <summary>
        /// the list of xml nodes in the yaml file
        /// </summary>
        public SortedList FXmlNodes;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="AFilename"></param>
        /// <param name="ASelectedLocalisation"></param>
        public TProcessYAMLForms(string AFilename, string ASelectedLocalisation)
        {
            FYamlFilename = AFilename;
            FSelectedLocalisation = ASelectedLocalisation;
        }

        /// <summary>
        /// this tool can work with several writers, eg. winforms, winform for reports, ext.js
        /// </summary>
        /// <param name="AFormType"></param>
        /// <param name="AFormProcessor"></param>
        public void AddWriter(String AFormType, System.Type AFormProcessor)
        {
            FFormTypes.Add(AFormType, AFormProcessor);
        }

        /// <summary>
        /// get a specific writer
        /// </summary>
        /// <param name="AFormType"></param>
        /// <returns></returns>
        private TFormWriter GetWriter(String AFormType)
        {
            Int32 index = FFormTypes.IndexOfKey(AFormType);

            System.Type formTypeClass = null;

            if (index >= 0)
            {
                formTypeClass = (System.Type)FFormTypes.GetByIndex(index);
            }

            if (formTypeClass == null)
            {
                throw new Exception("cannot find form processor \"" + AFormType + "\"");
            }

            return (TFormWriter)Activator.CreateInstance(formTypeClass, new Object[] { AFormType });
        }

        /// <summary>
        /// process the yaml document
        /// </summary>
        /// <returns></returns>
        public Boolean ProcessDocument()
        {
            string baseyaml;

            if (!TYml2Xml.ReadHeader(FYamlFilename, out baseyaml))
            {
                Console.WriteLine("ProcessYAML: cannot recognise type of form");
            }
            else
            {
                new TAppSettingsManager(false);

                //******************
                //* parsing *******
                //******************
                XmlDocument myDoc = TYml2Xml.CreateXmlDocument();
                TCodeStorage codeStorage = new TCodeStorage(myDoc, FXmlNodes);
                TParseYAMLFormsDefinition yamlParser = new TParseYAMLFormsDefinition(ref codeStorage);

                // should not need to be specific to special forms
                yamlParser.LoadRecursively(FYamlFilename, FSelectedLocalisation);

                // for debugging purposes, we can write the xml file that has been parsed from the yaml file
                // codeStorage.FXmlDocument.Save(FYamlFilename + ".xml");

                //****************
                //* output *******
                //****************
                TFormWriter writer = null;

                // get the appropriate derived class from IFormWriter (e.g. TFrmReportWriter)
                XmlNode rootNode = (XmlNode)yamlParser.FCodeStorage.FXmlNodes[TParseYAMLFormsDefinition.ROOTNODEYML];
                string formType = TYml2Xml.GetAttribute(rootNode, "FormType");

                if (formType == "abstract")
                {
                    Console.WriteLine("Ignore yaml file because it has the formtype abstract: " + FYamlFilename);
                    return true;
                }

                // the Template attribute is also quite important, because it determines which code is written
                // FormType is mainly important for the difference of the controls of reports and normal screens
                writer = GetWriter(formType);

                if (writer == null)
                {
                    Console.WriteLine("cannot find writer for {0}", formType);
                    return false;
                }

                writer.YamlFilename = FYamlFilename;

                string templateDir = TAppSettingsManager.GetValue("TemplateDir", true);
                string template = TYml2Xml.GetAttribute(rootNode, "Template");

                if (template.Length > 0)
                {
                    template = templateDir + Path.DirectorySeparatorChar + template + writer.CodeFileExtension;
                }

                string destinationFile = writer.CalculateDestinationFilename();
                string manualCodeFile = writer.CalculateManualCodeFilename();

                // need to know the path to the manual code file in order to call manual functions which would not be called if they do not exist
                codeStorage.ManualCodeFilename = manualCodeFile;

                writer.CreateCode(codeStorage, template);

                writer.CreateResourceFile(templateDir);

                writer.CreateDesignerFile(rootNode, templateDir);

                return writer.WriteFile(destinationFile);
            }

            return false;
        }

        /// <summary>
        /// for some operations, we want to ignore the language specific yaml files
        /// </summary>
        /// <returns>true if the file is language specific</returns>
        public static bool IgnoreLanguageSpecificYamlFile(string yamlfile)
        {
            // only look for main files, not language specific files (*.xy-XY.yaml or *.xy.yaml)
            if (((yamlfile[yamlfile.Length - 11] == '.') && (yamlfile[yamlfile.Length - 8] == '-')) || (yamlfile[yamlfile.Length - 8] == '.'))
            {
                return true;
            }

            return false;
        }
    }
}