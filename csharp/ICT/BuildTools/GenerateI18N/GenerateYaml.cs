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
using System.Text.RegularExpressions;
using System.Collections;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.CodeGeneration;
using Ict.Tools.DBXML;

namespace GenerateI18N
{
/// <summary>
/// create a yaml file for localisation
/// </summary>
public class GenerateYamlFiles
{
    /// <summary>
    /// write yaml files for localisation
    /// </summary>
    /// <param name="ALanguageCode"></param>
    /// <param name="AYamlFilePath"></param>
    /// <param name="APoFilePath"></param>
    public static void WriteYamlFiles(string ALanguageCode, string AYamlFilePath, string APoFilePath)
    {
        // TODO parse po file, and use for the labels

        string[] yamlfiles = System.IO.Directory.GetFiles(AYamlFilePath, "*.yaml", SearchOption.AllDirectories);

        Catalog.SetLanguage(ALanguageCode);
        Catalog.Init();

        foreach (string yamlfile in yamlfiles)
        {
            // only look for main files, not language specific files (*.xy-XY.yaml or *.xy.yaml)
            if (((yamlfile[yamlfile.Length - 11] == '.') && (yamlfile[yamlfile.Length - 8] == '-')) || (yamlfile[yamlfile.Length - 8] == '.'))
            {
                continue;
            }

            CreateLocalisedYamlFile(ALanguageCode, yamlfile);
        }
    }

    private static void AdjustLabel(XmlNode node, TCodeStorage CodeStorage)
    {
        TControlDef ctrlDef = new TControlDef(node, CodeStorage);
        string Label = ctrlDef.Label;

        if ((ctrlDef.GetAttribute("NoLabel") == "true") || (ctrlDef.controlTypePrefix == "pnl"))
        {
            Label = string.Empty;
        }

        if (ctrlDef.controlTypePrefix == "mni")
        {
            // drop all attributes
            node.Attributes.RemoveAll();

            foreach (XmlNode menu in node.ChildNodes)
            {
                AdjustLabel(menu, CodeStorage);
            }
        }
        else
        {
            // drop all attributes and children nodes
            node.RemoveAll();
        }

        if (Label.Length > 0)
        {
            TXMLParser.SetAttribute(node, "Label", Catalog.GetString(Label));
        }
    }

    private static void CreateLocalisedYamlFile(string ALanguageCode, string AYamlfile)
    {
        TLogging.Log(AYamlfile);

        string localisedYamlFile = AYamlfile.Replace(".yaml", "." + ALanguageCode + ".yaml");

        TYml2Xml parser;
        XmlDocument localisedYamlFileDoc = new XmlDocument();

        if (File.Exists(localisedYamlFile))
        {
            // TODO parse the localised yaml file and add translations to the po file
            parser = new TYml2Xml(localisedYamlFile);
            localisedYamlFileDoc = parser.ParseYML2XML();
        }

        // parse the yaml file
        parser = new TYml2Xml(AYamlfile);
        XmlDocument yamlFile = parser.ParseYML2XML();

        SortedList xmlNodes = new SortedList();
        TCodeStorage CodeStorage = new TCodeStorage(yamlFile, xmlNodes);

        XmlNode RootNode = TXMLParser.FindNodeRecursive(yamlFile.DocumentElement, "RootNode");
        RootNode.Attributes.RemoveAll();
        TXMLParser.SetAttribute(RootNode, "BaseYaml", Path.GetFileName(AYamlfile));

        // get controls node
        XmlNode Controls = TXMLParser.FindNodeRecursive(yamlFile.DocumentElement, "Controls");

        if (Controls != null)
        {
            foreach (XmlNode control in Controls)
            {
                AdjustLabel(control, CodeStorage);
            }
        }

        // get actions node
        XmlNode Actions = TXMLParser.FindNodeRecursive(yamlFile.DocumentElement, "Actions");

        if (Actions != null)
        {
            foreach (XmlNode action in Actions)
            {
                AdjustLabel(action, CodeStorage);
            }
        }

        // menu items
        XmlNode menuitems = TXMLParser.FindNodeRecursive(yamlFile.DocumentElement, "Menu");

        if (menuitems != null)
        {
            foreach (XmlNode menu in menuitems)
            {
                AdjustLabel(menu, CodeStorage);
            }
        }

        // toolbar buttons
        XmlNode tbbuttons = TXMLParser.FindNodeRecursive(yamlFile.DocumentElement, "Toolbar");

        if (tbbuttons != null)
        {
            foreach (XmlNode tbb in tbbuttons)
            {
                AdjustLabel(tbb, CodeStorage);
            }
        }

        // TODO parse the cs file for string constants
        // TODO warn about Catalog.GetString calls in manualcode file
        // TODO add constants to localised yaml file

        TYml2Xml.Xml2Yml(yamlFile, localisedYamlFile);
    }
}
}