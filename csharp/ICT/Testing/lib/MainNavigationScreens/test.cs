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
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Threading;

using Ict.Common;
using Ict.Common.IO;
using Ict.Common.Controls;
using Ict.Testing.NUnitForms;
using Ict.Testing.NUnitPetraClient;
using NUnit.Extensions.Forms;
using NUnit.Framework;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.PetraClient;
using Ict.Petra.Shared;

namespace Tests.MainNavigationScreens
{
    /// Testing the screens that can be opened from the main navigation
    [TestFixture]
    public class TMainNavigationTest
    {
        private XPathNavigator FNavigator;

        /// <summary>
        /// start the gui program
        /// </summary>
        [TestFixtureSetUp]
        public void Setup()
        {
            // Before Execution of any Test we should do something like
            // nant stopPetraServer
            // nant ResetDatabase
            // nant startPetraServer
            // this may take some time ....
            new TLogging("TestClient.log");
            TPetraConnector.Connect("../../etc/TestClient.config");

            TLstTasks.Init(UserInfo.GUserInfo.UserID, TFrmMainWindowNew.HasAccessPermission);

            // load the UINavigation file (csharp\ICT\Petra\Definitions\UINavigation.yml)
            TLogging.Log("loading " + TAppSettingsManager.GetValue("UINavigation.File"));
            XmlNode MainMenuNode = TFrmMainWindowNew.BuildNavigationXml();

            // saving a xml file for better understanding how to use the XPath commands
            StreamWriter sw = new StreamWriter(TAppSettingsManager.GetValue("UINavigation.File") + ".xml");
            sw.WriteLine(TXMLParser.XmlToStringIndented(MainMenuNode.OwnerDocument));
            sw.Close();

            FNavigator = MainMenuNode.OwnerDocument.CreateNavigator();
        }

        /// <summary>
        /// clean up, disconnect from OpenPetra server
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            TPetraConnector.Disconnect();
        }

        /// <summary>
        /// simple test to loop through all nodes of screens
        /// </summary>
        [Test]
        public void TestLoopThroughUINavigation()
        {
            // get all nodes that have an attribute ActionOpenScreen
            XPathExpression expr = FNavigator.Compile("//*[@ActionOpenScreen]");
            XPathNodeIterator iterator = FNavigator.Select(expr);

            while (iterator.MoveNext())
            {
                if (iterator.Current is IHasXmlNode)
                {
                    XmlNode ActionNode = ((IHasXmlNode)iterator.Current).GetNode();
                    TLogging.Log(ActionNode.Name + " " + ActionNode.Attributes["ActionOpenScreen"].Value);
                }
            }
        }

        /// <summary>
        /// verify that user can open the Partner Find screen
        /// </summary>
        [Test]
        [Ignore("problem with exception ThreadStateException")]
        public void TestOpenPartnerFind()
        {
            // get the node that opens the screen
            XPathExpression expr = FNavigator.Compile("//*[@ActionClick='TFrmPartnerMain.FindPartner']");
            XPathNodeIterator iterator = FNavigator.Select(expr);

            iterator.MoveNext();

            if (iterator.Current is IHasXmlNode)
            {
                XmlNode ActionNode = ((IHasXmlNode)iterator.Current).GetNode();

                TLogging.Log(ActionNode.Name + " " + ActionNode.Attributes["ActionClick"].Value);

                // set apartment state to STA, otherwise Partner Find would not open, ThreadStateException
                // this does not seem to work
                Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

                // open the screen.
                Assert.AreEqual(String.Empty,
                    TLstTasks.ExecuteAction(ActionNode, null));
            }
        }

        /// <summary>
        /// verify that user can open the language setting dialog
        /// </summary>
        [Test]
        public void TestLanguageDialog()
        {
            // get the node that opens the screen
            XPathExpression expr = FNavigator.Compile("//*[@ActionOpenScreen='TFrmMaintainLanguageCulture']");
            XPathNodeIterator iterator = FNavigator.Select(expr);

            iterator.MoveNext();

            if (iterator.Current is IHasXmlNode)
            {
                XmlNode ActionNode = ((IHasXmlNode)iterator.Current).GetNode();

                TLogging.Log(ActionNode.Name + " " + ActionNode.Attributes["ActionOpenScreen"].Value);

                // open the screen.
                Assert.AreEqual(String.Empty,
                    TLstTasks.ExecuteAction(ActionNode, null));

                Assert.AreEqual(Catalog.GetString("Maintain Language and Culture"),
                    TLstTasks.LastOpenedScreen.Text);
            }
        }

        /// <summary>
        /// verify that user DEMO cannot open the System Manager module
        /// </summary>
        [Test]
        public void TestPermissionsSystemManager()
        {
            Assert.AreEqual("demo", UserInfo.GUserInfo.UserID.ToLower(), "Test should be run with DEMO user");

            // get the node that opens the screen TFrmMaintainUsers
            XPathExpression expr = FNavigator.Compile("//*[@ActionOpenScreen='TFrmMaintainUsers']");
            XPathNodeIterator iterator = FNavigator.Select(expr);

            iterator.MoveNext();

            if (iterator.Current is IHasXmlNode)
            {
                XmlNode ActionNode = ((IHasXmlNode)iterator.Current).GetNode();

                TLogging.Log(ActionNode.Name + " " + ActionNode.Attributes["ActionOpenScreen"].Value);

                Assert.AreEqual(false, TFrmMainWindowNew.HasAccessPermission(ActionNode, UserInfo.GUserInfo.UserID),
                    "user DEMO should not have permissions for TFrmMaintainUsers");

                // open the screen. should return an error message
                Assert.AreEqual(Catalog.GetString("Sorry, you don't have enough permissions to do this"),
                    TLstTasks.ExecuteAction(ActionNode, null));
            }
        }
    }
}