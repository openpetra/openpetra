//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2018 by OM International
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
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory;
using NamespaceHierarchy;
using Ict.Common;
using Ict.Common.IO;
using Ict.Tools.CodeGeneration;
using System.Threading;

namespace GenerateSharedCode
{
    /// <summary>
    /// create the interfaces for the shared code
    /// </summary>
    public class CreateInterfaces
    {
        /// <summary>
        /// add using namespaces, defined in the yml file in the interface directory
        /// </summary>
        public static string AddNamespacesFromYmlFile(String AOutputPath, string AModuleName)
        {
            string result = string.Empty;

            if (AOutputPath.Contains("ICT/Petra/Plugins"))
            {
                // for plugins
                string pluginWithNamespace = TAppSettingsManager.GetValue("plugin");
                result += "using " + pluginWithNamespace + ".Data;" + Environment.NewLine;
            }

            if (File.Exists(AOutputPath + Path.DirectorySeparatorChar + "InterfacesUsingNamespaces.yml"))
            {
                TYml2Xml reader = new TYml2Xml(AOutputPath + Path.DirectorySeparatorChar + "InterfacesUsingNamespaces.yml");
                XmlDocument doc = reader.ParseYML2XML();

                XmlNode RootNode = doc.DocumentElement.FirstChild;

                StringCollection usingNamespaces = TYml2Xml.GetElements(RootNode, AModuleName);

                foreach (string s in usingNamespaces)
                {
                    result += "using " + s.Trim() + ";" + Environment.NewLine;
                }
            }

            return result;
        }
    }
}
