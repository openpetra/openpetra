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
using System.Collections;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;
using Ict.Common.IO;

namespace GenerateGTK
{
/// <summary>
/// Description of Generator.
/// </summary>
public class TGenerator
{
    /// <summary>
    /// this is a general method for all sorts of UI types (BrowseEdit, Find, Reports, etc)
    /// it will call the appropriate generator
    /// </summary>
    /// <param name="AYmlFile">definition of the screen</param>
    /// <param name="APetraXml">definition of the data structure</param>
    /// <param name="AOutputDir">where to write the code file</param>
    /// <returns></returns>
    public static bool GenerateGui(String AYmlFile, String APetraXml, String AOutputDir)
    {
        TYml2Xml ymlparser = new TYml2Xml(AYmlFile);
        XmlDocument doc = ymlparser.ParseYML2XML();
        SortedList XmlNodes = TYml2Xml.ReferenceNodes(doc);
        XmlNode GeneratorNode = (XmlNode)XmlNodes["generator"];

        if (GeneratorNode == null)
        {
            throw new Exception("GenerateGTK: cannot find the root element \"generator\"");
        }

        TDataDefinitionParser PetraXmlParser = new TDataDefinitionParser(APetraXml);
        TDataDefinitionStore PetraXmlStore = new TDataDefinitionStore();

        if (!PetraXmlParser.ParseDocument(ref PetraXmlStore))
        {
            throw new Exception(String.Format("GenerateGTK: problems parsing {0}", APetraXml));
        }

        string OutputFile = AOutputDir + System.IO.Path.DirectorySeparatorChar + System.IO.Path.GetFileNameWithoutExtension(AYmlFile) + ".cs";
        AutoGenerationWriter Writer = new AutoGenerationWriter(OutputFile);
        string UIType = TXMLParser.GetAttribute(GeneratorNode, "type");
        bool success = false;

        if (UIType == "BrowseEdit")
        {
            success = TGenerateBrowseEdit.Generate(Writer, XmlNodes, PetraXmlStore);
        }
        else
        {
            throw new Exception(String.Format("GenerateGTK: unknown UIType {0}", UIType));
        }

        Writer.Close(success);
        return success;
    }
}
}