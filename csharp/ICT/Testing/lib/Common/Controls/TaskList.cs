//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Seth Bird (sethb)
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
using System.Data;
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.IO;
using System.Xml;
using System.Threading;
using System.Globalization;
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Common.IO;

namespace Tests.Common.Controls
{
    /// <summary>
    /// NUnit tests for TTaskList
    /// </summary>
    [TestFixture]
    public class TTestTaskList
    {
        /// <summary>
        ///
        /// </summary>
        [Test]
        public void CreateWithDefaultConstructor()
        {
            //TODO: should have empty list
        }

        /// <summary>
        /// Returns a hard-coded XmlNode for testing purposes.
        /// </summary>
        /// <returns>Hard-coded XmlNode.</returns>
        public static XmlNode GetTestXmlNode()
        {
            string[] lines = new string[3];

            lines[0] = "TaskGroup:\n";
            lines[1] = "    Task1:\n";
            lines[2] = "        Label: Testing";

            return new TYml2Xml(lines).ParseYML2TaskListRoot();;
        }

        /// <summary>
        /// Returns a hard-coded XmlNode for testing purposes.
        /// </summary>
        /// <returns>Hard-coded XmlNode.</returns>
        public static XmlNode GetTestXmlNode2()
        {
            string[] lines = new string[7];

            lines[0] = "TaskGroup:\n";
            lines[1] = "    Task1:\n";
            lines[2] = "        Label: First Item";
            lines[3] = "    Task2:\n";
            lines[4] = "        Label: Second Item";
            lines[5] = "    Task3:\n";
            lines[6] = "        Label: Third Item";

            return new TYml2Xml(lines).ParseYML2TaskListRoot();
        }

        /// <summary>
        /// </summary>
        public void ExampleCallback()
        {
            MessageBox.Show("The callback worked.",
                "Tests.Common.Controls.TTestTaskList.ExampleCallback",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Some remaining questions:
        ///
        ///  1) Where will the actual function be defined? in some .manual.cs file?
        /// </summary>
        [Test]
        public void TestCallbackOnActivateEvent()
        {
            //node.Attributes["Namespace"] = "Tests.Common.Controls";
            //node.Attributes["Class"] = "TTestTaskList";
            //node.Attributes["Method"] = "ExampleCallback";
            //TTaskList tl = new TTaskList(xmlnode);

            //TODO: manually activate and test callback works?
            //SEE: Gui test in ControlTestBench
        }

        //TODO: what should happen if only some of the "Namespace"/"Class"/"Method" attributes are set? exception?

        //TODO: public XmlNode GetTaskByNumber(String TaskNumber, bool IncludeHiddenElements, XmlNode Node)

        //TODO: public XmlNode GetTaskByName(String TaskName, XmlNode Node)

        //TODO: public void ChangeAttribute(XmlNode node, string attr, string setting, bool load)

        //TODO: public void getAttribute(XmlNode node, string attr)
    }
}