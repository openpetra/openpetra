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
        private IFormWriter GetWriter(String AFormType)
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

            return (IFormWriter)Activator.CreateInstance(formTypeClass, new Object[] { AFormType });
        }

        public Boolean ProcessDocument()
        {
            string baseyaml;

            if (!TYml2Xml.ReadHeader(FYamlFilename, out baseyaml))
            {
                Console.WriteLine("ProcessYAML: cannot recognise type of form");
            }
            else
            {
                TAppSettingsManager opts = new TAppSettingsManager(false);

                string destinationFile = System.IO.Path.GetDirectoryName(FYamlFilename) +
                                         System.IO.Path.DirectorySeparatorChar +
                                         System.IO.Path.GetFileNameWithoutExtension(FYamlFilename) +
                                         ".cs";
                string manualCodeFile = System.IO.Path.GetDirectoryName(FYamlFilename) +
                                        System.IO.Path.DirectorySeparatorChar +
                                        System.IO.Path.GetFileNameWithoutExtension(FYamlFilename) +
                                        ".ManualCode.cs";
                string designerFile = System.IO.Path.GetDirectoryName(FYamlFilename) +
                                      System.IO.Path.DirectorySeparatorChar +
                                      System.IO.Path.GetFileNameWithoutExtension(FYamlFilename) +
                                      ".Designer.cs";
                string templateDir = opts.GetValue("TemplateDir", true);
                string ResourceFile = System.IO.Path.GetDirectoryName(FYamlFilename) +
                                      System.IO.Path.DirectorySeparatorChar +
                                      System.IO.Path.GetFileNameWithoutExtension(FYamlFilename) +
                                      ".resx";

                //******************
                //* parsing *******
                //******************
                XmlDocument myDoc = TYml2Xml.CreateXmlDocument();
                TCodeStorage codeStorage = new TCodeStorage(myDoc, FXmlNodes, manualCodeFile);
                TParseYAMLFormsDefinition yamlParser = new TParseYAMLFormsDefinition(ref codeStorage);

                codeStorage.FTargetWinforms = opts.GetValue("TargetPlatform", "net-2.0");

                // should not need to be specific to special forms
                yamlParser.LoadRecursively(FYamlFilename, FSelectedLocalisation);

                // todo: parse the existing cs file as well and merge into existing data from YAML file
                // load the existing file if it exists
                if (File.Exists(designerFile))
                {
                    // find the output file, and parse it
// TODO          CSParser csharpfile = new CSParser(designerFile);
                    // todo: make safety copy???
// TODO          TParseDesignerCode.ParseDesignerSection(csharpfile, codeStorage);
                }

                //******************
                //* maintain *******
                //******************
                codeStorage.HouseKeeping();

                codeStorage.FXmlDocument.Save(FYamlFilename + ".xml");


                codeStorage.UpdateLanguageFile();

                // todo: update the yaml file
                //       this could also write back changes in labels, etc.
                //       what about keeping comments in the yaml file?
                //       write new added controls/menuitems/etc back to the yaml file???

                //****************
                //* output *******
                //****************
                IFormWriter writer = null;

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

                string template = TYml2Xml.GetAttribute(rootNode, "Template");

                if (template.Length > 0)
                {
                    template = templateDir + Path.DirectorySeparatorChar + template + ".cs";
                }

/*        else if (File.Exists(destinationFile))
 *      {
 *        // if file exists, load it as the template
 *        template = destinationFile;
 *      }
 */

                writer.CreateCode(codeStorage, FYamlFilename, template);

                // Create the resource file...
                string ResourceTemplate = templateDir + Path.DirectorySeparatorChar + "resources.resx";
                writer.CreateResourceFile(ResourceFile, ResourceTemplate);

                // use TXMLParser.GetAttribute in order to not use the base value
                // TODO we disabled the base classes, so we can't check for "abstract"
                // if (TXMLParser.GetAttribute(rootNode, "ClassType") != "abstract")
                {
                    // only write the designer file if the file is not an abstract base class
                    // write the designer code
                    // that is merged from the existing code
                    // and the definitions in the yaml file

                    string designerTemplate = String.Empty;

                    if (TYml2Xml.HasAttribute(rootNode, "DesignerTemplate"))
                    {
                        designerTemplate = TYml2Xml.GetAttribute(rootNode, "DesignerTemplate") + ".cs";
                    }
                    else
                    {
                        designerTemplate = "designer.cs";
                    }

                    designerTemplate = templateDir + Path.DirectorySeparatorChar + designerTemplate;

                    writer.WriteFile(designerFile, designerTemplate);
                }

                return writer.WriteFile(destinationFile);
            }

            return false;
        }
    }
}