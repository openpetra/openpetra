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
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using Ict.Common.IO;
using Ict.Tools.DBXML;
using Ict.Tools.CodeGeneration;

namespace GenerateGTK
{
/// <summary>
/// This generates a BrowseEdit window
/// </summary>
public class TGenerateBrowseEdit
{
    /// <summary>
    /// Generate the Browse Edit based on the input parameters
    /// </summary>
    /// <param name="AWriter">this is where to code is written into</param>
    /// <param name="AXmlNodes">the XmlNodes of the parsed yml file</param>
    /// <param name="APetraXmlStore">datastructure needed for knowing the types of the fields</param>
    /// <returns>true on success</returns>
    public static bool Generate(AutoGenerationWriter AWriter, SortedList AXmlNodes, TDataDefinitionStore APetraXmlStore)
    {
        XmlNode GeneratorNode = (XmlNode)AXmlNodes["generator"];
        string Module = TXMLParser.GetAttribute(GeneratorNode, "module");
        string Title = TXMLParser.GetAttribute(GeneratorNode, "title");
        string Class = TXMLParser.GetAttribute(GeneratorNode, "class");
        string FormattedTableName = TXMLParser.GetAttribute(GeneratorNode, "model_class");
        int Width = 700;
        int Height = 500;
        bool WithDeleteOperation = true;
        bool WithAddOperation = true;
        bool WithEditOperation = true;

        if (AXmlNodes["operations"] != null)
        {
            StringCollection operations = TYml2Xml.GetElements((XmlNode)AXmlNodes["operations"]);
            WithDeleteOperation = operations.Contains("Delete");
            WithAddOperation = operations.Contains("Add");
            WithEditOperation = operations.Contains("Edit");
        }

        // TODO: check access permissions
        // TODO: change toolbar buttons depending on the current state
        // TODO: load data
        // TODO: submit changes

        if (Title.Length == 0)
        {
            Title = "Maintain " + FormattedTableName + " Table";
        }

        TTable Table = APetraXmlStore.GetTable(FormattedTableName);

        AWriter.WriteComment("This is an automatically generated file:" +
            Environment.NewLine +
            "Do not change it manually. " +
            Environment.NewLine +
            "Change the yaml file instead and regenerate the cs file");
        AWriter.WriteLine("using Gtk;");
        AWriter.WriteLine("using System;");
        AWriter.WriteLine("using System.Collections;");
        AWriter.WriteLine("using System.Data;");
        AWriter.WriteLine("using Mono.Unix;");
        AWriter.WriteLine("using Ict.Common.GTK;");
        AWriter.WriteLine("using Ict.Petra.Client.App.Core.RemoteObjects;");
        AWriter.WriteLine(String.Format("using Ict.Petra.Shared.Interfaces.{0};", Module));
        AWriter.WriteLine(String.Format("using Ict.Petra.Shared.{0}.Data;", Module));

        AWriter.WriteLine();
        AWriter.StartBlock(String.Format("namespace Ict.Petra.Client.{0}", Module));
        AWriter.WriteLine("/// Auto generated class");
        AWriter.StartBlock(String.Format("public partial class {0}: TFrmBrowseEdit", Class));
        AWriter.StartBlock(String.Format("public {0}() : base(Catalog.GetString(\"{1}\"), {2}, {3})", Class, Title, Width, Height));

        AWriter.WriteLine(String.Format("InitToolbar({0},{1},{2});",
                WithEditOperation ? "true" : "false",
                WithAddOperation ? "true" : "false",
                WithDeleteOperation ? "true" : "false"));

        // TODO show waiting cursor
        AWriter.WriteLine(String.Format("I{0}UIConnectorsMaintainTable UIConnector = TRemote.{0}.UIConnectors.MaintainTable(\"{1}\");", Module,
                Table.strName));
        AWriter.WriteLine("DataTable table = UIConnector.GetData();");

        AWriter.WriteLine("InitGrid(table);");

        // TODO: captions for grid; Catalog.GetString
        // TODO: filter
        // TODO: edit details
        // TODO show normal cursor
        AWriter.WriteLine("AssembleAndShow();");
        AWriter.EndBlock();

        AWriter.EndBlock();
        AWriter.EndBlock();
        return true;
    }
}
}