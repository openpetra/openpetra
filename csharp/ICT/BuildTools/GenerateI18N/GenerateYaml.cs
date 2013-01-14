//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2013 by OM International
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
using System.Linq;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
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
        if (ALanguageCode == "en-EN")
        {
            return;
        }

        string[] yamlfiles = System.IO.Directory.GetFiles(AYamlFilePath, "*.yaml", SearchOption.AllDirectories);

        // load (compiled) po file, and use for the labels
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

        TPoFileParser.WriteUpdatedPoFile(APoFilePath, NewTranslations);
    }

    private static SortedList <string, string>NewTranslations = new SortedList <string, string>();

    private static void ProcessRadioGroupLabels(XmlNode node)
    {
        StringCollection optionalValues =
            TYml2Xml.GetElements(TXMLParser.GetChild(node, "OptionalValues"));

        node.RemoveAll();
        XmlNode OptionalValuesLabel = node.OwnerDocument.CreateElement("LabelsForOptionalValues");
        node.AppendChild(OptionalValuesLabel);

        foreach (string s in optionalValues)
        {
            string label = s;

            if (label.StartsWith("="))
            {
                label = label.Substring(1).Trim();
            }

            XmlNode LabelNode = node.OwnerDocument.CreateElement(TYml2Xml.XMLLIST);
            OptionalValuesLabel.AppendChild(LabelNode);
            TXMLParser.SetAttribute(LabelNode, "name", Catalog.GetString(label));
        }
    }

    private static void AdjustLabel(XmlNode node, TCodeStorage CodeStorage, XmlDocument AOrigLocalisedYaml)
    {
        XmlNode TranslatedNode = TXMLParser.FindNodeRecursive(AOrigLocalisedYaml, node.Name);
        string TranslatedLabel = string.Empty;

        if (TranslatedNode != null)
        {
            TranslatedLabel = TXMLParser.GetAttribute(TranslatedNode, "Label");
        }

        TControlDef ctrlDef = new TControlDef(node, CodeStorage);
        string Label = ctrlDef.Label;

        if ((ctrlDef.GetAttribute("NoLabel") == "true") || (ctrlDef.controlTypePrefix == "pnl")
            || (TXMLParser.FindNodeRecursive(node.OwnerDocument, "act" + ctrlDef.controlName.Substring(ctrlDef.controlTypePrefix.Length)) != null)
            || ctrlDef.GetAttribute("Action").StartsWith("act"))
        {
            Label = string.Empty;
        }

        if ((ctrlDef.controlTypePrefix == "rgr") && (TXMLParser.GetChild(node, "OptionalValues") != null))
        {
            ProcessRadioGroupLabels(node);
        }
        else if (ctrlDef.controlTypePrefix == "mni")
        {
            // drop all attributes
            node.Attributes.RemoveAll();

            List <XmlNode>NodesToDelete = new List <XmlNode>();

            foreach (XmlNode menu in node.ChildNodes)
            {
                if (menu.Name.Contains("Separator"))
                {
                    NodesToDelete.Add(menu);
                    continue;
                }

                AdjustLabel(menu, CodeStorage, AOrigLocalisedYaml);
            }

            foreach (XmlNode menu in NodesToDelete)
            {
                node.RemoveChild(menu);
            }
        }
        else
        {
            // drop all attributes and children nodes
            node.RemoveAll();
        }

        if (Label.Length > 0)
        {
            if ((TranslatedLabel != Label) && (TranslatedLabel != Catalog.GetString(Label)) && (TranslatedLabel.Length > 0))
            {
                // add to po file
                if (!NewTranslations.ContainsKey(Label))
                {
                    NewTranslations.Add(Label, TranslatedLabel);
                }

                TXMLParser.SetAttribute(node, "Label", TranslatedLabel);
            }
            else
            {
                TXMLParser.SetAttribute(node, "Label", Catalog.GetString(Label));
            }
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
                AdjustLabel(control, CodeStorage, localisedYamlFileDoc);
            }
        }

        // get actions node
        XmlNode Actions = TXMLParser.FindNodeRecursive(yamlFile.DocumentElement, "Actions");

        if (Actions != null)
        {
            foreach (XmlNode action in Actions)
            {
                AdjustLabel(action, CodeStorage, localisedYamlFileDoc);
            }
        }

        // menu items
        XmlNode menuitems = TXMLParser.FindNodeRecursive(yamlFile.DocumentElement, "Menu");

        if (menuitems != null)
        {
            List <XmlNode>NodesToDelete = new List <XmlNode>();

            foreach (XmlNode menu in menuitems)
            {
                if (menu.Name.Contains("Separator"))
                {
                    NodesToDelete.Add(menu);
                    continue;
                }

                AdjustLabel(menu, CodeStorage, localisedYamlFileDoc);
            }

            foreach (XmlNode menu in NodesToDelete)
            {
                menuitems.RemoveChild(menu);
            }
        }

        // toolbar buttons
        XmlNode tbbuttons = TXMLParser.FindNodeRecursive(yamlFile.DocumentElement, "Toolbar");

        if (tbbuttons != null)
        {
            List <XmlNode>NodesToDelete = new List <XmlNode>();

            foreach (XmlNode tbb in tbbuttons)
            {
                if (tbb.Name.Contains("Separator"))
                {
                    NodesToDelete.Add(tbb);
                    continue;
                }

                AdjustLabel(tbb, CodeStorage, localisedYamlFileDoc);
            }

            foreach (XmlNode tbb in NodesToDelete)
            {
                tbbuttons.RemoveChild(tbb);
            }
        }

        // TODO parse the cs file for string constants
        // TODO warn about Catalog.GetString calls in manualcode file
        // TODO add constants to localised yaml file

        TYml2Xml.Xml2Yml(yamlFile, localisedYamlFile);
    }
}
}