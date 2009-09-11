/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using DDW;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.CodeGeneration;

namespace Ict.Tools.CodeGeneration.Winforms
{
    /// <summary>
    /// this class writes the new navigation bar using the definition file for the actions and different departments
    /// </summary>
    public class TNavigationGenerator
    {
        public static void LoadPanelNavigation(ref ProcessTemplate Template, string UINavigationFile)
        {
            TYml2Xml UIParser = new TYml2Xml(UINavigationFile);
            XmlDocument xmlDoc = UIParser.ParseYML2XML();

            XmlNode OpenPetraNode = xmlDoc.FirstChild.NextSibling.FirstChild;
            XmlNode SearchBoxesNode = OpenPetraNode.FirstChild;
            XmlNode MainMenuNode = SearchBoxesNode.NextSibling;
            XmlNode DepartmentNode = MainMenuNode.FirstChild;

            while (DepartmentNode != null)
            {
                // add button for the department
                string label = DepartmentNode.Name;

                if (TYml2Xml.HasAttribute(DepartmentNode, "Label"))
                {
                    label = TYml2Xml.GetAttribute(DepartmentNode, "Label");
                }

                ProcessTemplate snippet = Template.GetSnippet("BUTTONCONTROLDECLARATION");
                snippet.SetCodelet("BUTTONNAME", "rbt" + DepartmentNode.Name);
                Template.InsertSnippet("CONTROLDECLARATION", snippet);

                snippet = Template.GetSnippet("BUTTONCONTROLCREATION");
                snippet.SetCodelet("BUTTONNAME", "rbt" + DepartmentNode.Name);
                Template.InsertSnippet("CONTROLCREATION", snippet);

                snippet = Template.GetSnippet("ADDNAVIGATIONBUTTONS");
                snippet.SetCodelet("BUTTONNAME", "rbt" + DepartmentNode.Name);
                Template.InsertSnippet("ADDNAVIGATIONBUTTONS", snippet);

                snippet = Template.GetSnippet("BUTTONCONTROLINITIALISATION");
                snippet.SetCodelet("BUTTONLABEL", label);
                snippet.SetCodelet("BUTTONNAME", "rbt" + DepartmentNode.Name);
                snippet.SetCodelet("BUTTONIMAGE", TYml2Xml.GetAttribute(DepartmentNode, "Icon")); // TODO insert image into resource file
                Template.InsertSnippet("CONTROLINITIALISATION", snippet);

                // TODO IMAGEBUTTONSSETKEYNAME
                Template.AddToCodelet("IMAGEBUTTONSSETKEYNAME", "");

                // TODO ADDNAVIGATIONPANELS
                Template.AddToCodelet("ADDNAVIGATIONPANELS", "");

                DepartmentNode = DepartmentNode.NextSibling;
            }
        }
    }
}