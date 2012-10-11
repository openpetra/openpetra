//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop and mitchvz
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
using System.Windows; //for the close() method
using System.Collections.Generic;

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
            new TLogging("TestClient_MainNavigationTest.log");

            // clear the log file
            using (FileStream stream = new FileStream("TestClient_MainNavigationTest.log", FileMode.Create))
                using (TextWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("");
                }

            TPetraConnector.Connect("../../etc/TestClient.config");

            TLstTasks.Init(UserInfo.GUserInfo.UserID, TFrmMainWindowNew.HasAccessPermission);

            // load the UINavigation file (csharp\ICT\Petra\Definitions\UINavigation.yml)
            TLogging.Log("loading " + TAppSettingsManager.GetValue("UINavigation.File"));
            XmlNode MainMenuNode = TFrmMainWindowNew.BuildNavigationXml(false);

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

            //change demo's permissions in the xml back to normal
            TLstTasks.Init(UserInfo.GUserInfo.UserID, FalsePermission);

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

        /// <summary>
        /// verify that all windows open either without error or with proper exception handling
        /// </summary>
        [Test]
        public void TestOpenAllWindows()
        {
            // get all nodes that have an attribute ActionOpenScreen
            XPathExpression expr = FNavigator.Compile("//*[@ActionOpenScreen]");
            XPathNodeIterator iterator = FNavigator.Select(expr);

            //create counter variables to keep track of number of failures (might do with lists soon...for modules(?))
            int NoPermissionCount = 0;
            int BadFailures = 0;
            int TotalWindowsOpened = 0;

            List <String>notOpened = new List <String>();
            List <String>permissions = new List <String>();
            List <String>workingWindows = new List <String>();

            while (iterator.MoveNext())
            {
                if (iterator.Current is IHasXmlNode)
                {
                    XmlNode ActionNode = ((IHasXmlNode)iterator.Current).GetNode();
                    // look at the permissions module the window came from
                    string Module = TXMLParser.GetAttributeRecursive(ActionNode, "PermissionsRequired", true);

                    // Try to open each screen and log the screens that cannot open
                    try
                    {
                        Assert.AreEqual(String.Empty,
                            TLstTasks.ExecuteAction(ActionNode, null));
                        TLstTasks.LastOpenedScreen.Close();
                        TotalWindowsOpened++;
                        string WindowAndModule = ActionNode.Name + Environment.NewLine + "            Permission Required: " +
                                                 Module;
                        workingWindows.Add(WindowAndModule);

                        //make sure the user had the permissions to open the windows that it opened
                        if (!UserInfo.GUserInfo.IsInModule(Module) && !Module.Contains(""))
                        {
                            BadFailures++;
                            workingWindows.Add("User did not have permission to access " + Module);
                        }
                    }
                    catch (AssertionException e)
                    {
                        TLogging.Log("Window can't be opened: " + ActionNode.Name + " " + ActionNode.Attributes["ActionOpenScreen"].Value +
                            Environment.NewLine + e.ToString());

                        // if the failure is a permission failure, just log it but don't fail the test
                        if (Catalog.GetString("Sorry, you don't have enough permissions to do this") ==
                            TLstTasks.ExecuteAction(ActionNode, null))
                        {
                            // make sure user didn't have the necessary permissions to open that window
                            // true means user should have been able to open without error
                            if (UserInfo.GUserInfo.IsInModule(Module))
                            {
                                BadFailures++;
                                string whyFailed = "User should have been able to open " + ActionNode.Name + " with his " +
                                                   Module + " permission...";
                                notOpened.Add(whyFailed);
                            }
                            else
                            {
                                NoPermissionCount++;
                                string WindowAndModule = ActionNode.Name + Environment.NewLine + "            Permission Required: " +
                                                         TXMLParser.GetAttributeRecursive(ActionNode, "PermissionsRequired", true);
                                permissions.Add(WindowAndModule);
                            }
                        }
                        else
                        {
                            BadFailures++;

                            string WindowAndModule = ActionNode.Name + Environment.NewLine + "            Permission Required: " +
                                                     TXMLParser.GetAttributeRecursive(ActionNode, "PermissionsRequired", true);

                            notOpened.Add(WindowAndModule);
                        }
                    }
                    // if the exception has to due with a ledger that the user doesn't have permission to access,
                    // make it a permission exception. Else, fail the test.
                    catch (Exception e)
                    {
                        TLogging.Log("Window can't be opened: " + ActionNode.Name + " " + ActionNode.Attributes["ActionOpenScreen"].Value +
                            Environment.NewLine + e.ToString());

                        string ledgerNumber = TXMLParser.GetAttributeRecursive(ActionNode, "LedgerNumber", true);
                        string ledger = "LEDGER00" + ledgerNumber;

                        if (!UserInfo.GUserInfo.IsInModule(ledger))
                        {
                            NoPermissionCount++;
                            string WindowAndModule = ActionNode.Name + Environment.NewLine + "            Permission Required: " +
                                                     TXMLParser.GetAttributeRecursive(ActionNode, "PermissionsRequired",
                                true) + Environment.NewLine +
                                                     "                                 " + ledger;
                            permissions.Add(WindowAndModule);
                        }
                        else
                        {
                            BadFailures++;
                            string WindowAndModule = ActionNode.Name + Environment.NewLine + "            Permission Required: " +
                                                     TXMLParser.GetAttributeRecursive(ActionNode, "PermissionsRequired",
                                true) + Environment.NewLine +
                                                     "                                 " + ledger;
                            notOpened.Add(WindowAndModule);
                        }
                    }
                }
            }

            //Give stats about where failures were
            //feel free to change any formatting, I'm not in love with it right now
            string notOpenedString = string.Join(Environment.NewLine + "      ", notOpened.ToArray());
            string permissionsString = string.Join(Environment.NewLine + "      ", permissions.ToArray());
            string workingWindowsString = string.Join(Environment.NewLine + "      ", workingWindows.ToArray());

            //print the permissions the user should have
            TLogging.Log(Environment.NewLine + Environment.NewLine + "User Permissions: " + Environment.NewLine +
                UserInfo.GUserInfo.GetPermissions());

            TLogging.Log(Environment.NewLine + Environment.NewLine + "Statistics: " + Environment.NewLine + "Number of windows opened: " +
                TotalWindowsOpened + Environment.NewLine + "      " + workingWindowsString + Environment.NewLine +
                Environment.NewLine + Environment.NewLine + Environment.NewLine + "Permission Exceptions: " +
                permissions.Count + Environment.NewLine + "      " + permissionsString + Environment.NewLine +
                Environment.NewLine + Environment.NewLine + "Windows that should be opened but couldn't: " +
                notOpened.Count + Environment.NewLine + "      " + notOpenedString + Environment.NewLine);

            //Now the loop is finished so fail if there were exceptions
            if (BadFailures > 0)
            {
                Assert.Fail();
            }
        }

        //helper method for the below test
        private static bool TruePermission(XmlNode node, string aUserID)
        {
            return true;
        }

        //another helper method to reset the test below
        private static bool FalsePermission(XmlNode node, string aUserId)
        {
            return false;
        }

        /// <summary>
        /// verify that demo user is unable to fool the server to get SYSADMIN permissions
        /// </summary>
        [Test]
        public void TestBrokenClientPermissions()
        {
            TLstTasks.Init(UserInfo.GUserInfo.UserID, TruePermission);

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

                try
                {
                    TLstTasks.ExecuteAction(ActionNode, null);

                    Assert.Fail("Demo was able to open the screen!");
                }
                catch (ApplicationException e)
                {
                    Assert.AreEqual(e.Message, "Exception has been thrown by the target of an invocation.");
                }
            }
        }
    }
}