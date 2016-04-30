//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop and mitchvz
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
    public class TMainNavigationTest : NUnitFormTest
    {
        private XPathNavigator FNavigator;
        private Boolean FConnectedToServer = false;

        /// <summary>
        /// start the gui program
        /// </summary>
        public override void Setup()
        {
            // Before Execution of any Test we should do something like
            // nant stopPetraServer
            // nant ResetDatabase
            // nant startPetraServer
            // this may take some time ....
            new TLogging("../../log/TestClient_MainNavigationTest.log");

            // clear the log file
            using (FileStream stream = new FileStream("TestClient_MainNavigationTest.log", FileMode.Create))
                using (TextWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine("");
                }

            FConnectedToServer = false;
            try
            {
                TPetraConnector.Connect("../../etc/TestClient.config");
                FConnectedToServer = true;
            }
            catch (Exception)
            {
                Assert.Fail("Failed to connect to the Petra Server.  Have you forgotten to launch the Server Console");
            }

            TLstTasks.Init(UserInfo.GUserInfo.UserID, TFrmMainWindowNew.HasAccessPermission);

            // load the UINavigation file (csharp\ICT\Petra\Definitions\UINavigation.yml)
            TLogging.Log("loading " + TAppSettingsManager.GetValue("UINavigation.File"));
            XmlNode MainMenuNode = TFrmMainWindowNew.BuildNavigationXml(false);

            // saving a xml file for better understanding how to use the XPath commands
            //StreamWriter sw = new StreamWriter(TAppSettingsManager.GetValue("UINavigation.File") + ".xml");
            //sw.WriteLine(TXMLParser.XmlToStringIndented(MainMenuNode.OwnerDocument));
            //sw.Close();

            FNavigator = MainMenuNode.OwnerDocument.CreateNavigator();

            TLogging.Log("Test Setup finished..." + Environment.NewLine);
        }

        /// <summary>
        /// clean up, disconnect from OpenPetra server
        /// </summary>
        public override void TearDown()
        {
            if (!FConnectedToServer)
            {
                return;
            }

            TPetraConnector.Disconnect();
        }

        /// <summary>
        /// simple test to loop through all nodes of screens
        /// </summary>
        [Test]
        public void TestLoopThroughUINavigation()
        {
            TLogging.Log("Running test 'TestLoopThroughUINavigation'..." + Environment.NewLine);

            // get all nodes that have an attribute ActionOpenScreen
            XPathExpression expr = FNavigator.Compile("//*[@ActionOpenScreen]");
            XPathNodeIterator iterator = FNavigator.Select(expr);

            while (iterator.MoveNext())
            {
                if (iterator.Current is IHasXmlNode)
                {
                    XmlNode ActionNode = ((IHasXmlNode)iterator.Current).GetNode();

                    // look at the permissions module the window came from
                    string Module = TYml2Xml.GetAttributeRecursive(ActionNode, "PermissionsRequired");
                    TLogging.Log(String.Format("{0,-36}{1,-36}{2}", ActionNode.Name, ActionNode.Attributes["ActionOpenScreen"].Value, Module));
                }
            }
        }

        /// <summary>
        /// verify that user DEMO cannot open the System Manager module
        /// </summary>
        [Test]
        public void TestPermissionsSystemManager()
        {
            TLogging.Log("Running test 'TestPermissionsSystemManager'..." + Environment.NewLine);

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

                Assert.AreEqual(false, TFrmMainWindowNew.HasAccessPermission(ActionNode, UserInfo.GUserInfo.UserID, false),
                    "user DEMO should not have permissions for TFrmMaintainUsers");

                // open the screen. should return an error message
                Assert.AreEqual(Catalog.GetString("Sorry, you don't have enough permissions to do this"),
                    TLstTasks.ExecuteAction(ActionNode, null));
            }
        }

        // Run Method 'TestOpenAllWindows' in the context of different Cultures.
        //
        // Note: At the time of this writing (Oct. 2013) there is no better way
        // than using the 'SetCulture' Attribute; once NUnit 3 is out there might
        // be a way of telling NUnit to execute a single method with 1..n Cultures,
        // accoring to the authors of NUnit...

        /// <summary>
        /// Runs the <see cref="TestOpenAllWindows" /> Method under en_US Culture.
        /// </summary>
        [Test]
        [SetCulture("en-US")]
        public void TestOpenAllWindows_en_US()
        {
            TestOpenAllWindows();
        }

        /// <summary>
        /// Runs the <see cref="TestOpenAllWindows" /> Method under en_GB Culture.
        /// </summary>
        [Test]
        [SetCulture("en-GB")]
        public void TestOpenAllWindows_en_GB()
        {
            TestOpenAllWindows();
        }

        /// <summary>
        /// Runs the <see cref="TestOpenAllWindows" /> Method under de_DE Culture.
        /// </summary>
        [Test]
        [SetCulture("de-DE")]
        public void TestOpenAllWindows_de_DE()
        {
            TestOpenAllWindows();
        }

        /// <summary>
        /// Runs the <see cref="TestOpenAllWindows" /> Method under de_AT Culture.
        /// </summary>
        [Test]
        [SetCulture("de-AT")]
        public void TestOpenAllWindows_de_AT()
        {
            TestOpenAllWindows();
        }

        /// <summary>
        /// Runs the <see cref="TestOpenAllWindows" /> Method under de_CH Culture.
        /// </summary>
        [Test]
        [SetCulture("de-CH")]
        public void TestOpenAllWindows_de_CH()
        {
            TestOpenAllWindows();
        }

        /// <summary>
        /// Runs the <see cref="TestOpenAllWindows" /> Method under fr_FR Culture.
        /// </summary>
        [Test]
        [SetCulture("fr-FR")]
        public void TestOpenAllWindows_fr_FR()
        {
            TestOpenAllWindows();
        }

        /// <summary>
        /// verify that all windows open either without error or with proper exception handling
        /// </summary>
        private void TestOpenAllWindows()
        {
            TLogging.Log(String.Format("Running test 'TestOpenAllWindows' with Culture '{0}'...",
                    Thread.CurrentThread.CurrentCulture.ToString()) + Environment.NewLine);

            // get all nodes that have an attribute ActionOpenScreen
            XPathExpression expr = FNavigator.Compile("//*[@ActionOpenScreen]");
            XPathNodeIterator iterator = FNavigator.Select(expr);

            //create counter variables to keep track of number of failures (might do with lists soon...for modules(?))
            int NoSysManPermissionCount = 0;
            int NoOtherPermissionCount = 0;
            int BadFailures = 0;
            int TotalWindowsOpened = 0;

            List <String>notOpened = new List <String>();
            List <String>sysManPermissions = new List <String>();
            List <String>otherPermissions = new List <String>();
            List <String>workingWindows = new List <String>();

            while (iterator.MoveNext())
            {
                if (iterator.Current is IHasXmlNode)
                {
                    XmlNode ActionNode = ((IHasXmlNode)iterator.Current).GetNode();

                    string className = ActionNode.Attributes["ActionOpenScreen"].Value;

                    if (className == "TFrmBankStatementImport")
                    {
                        // skip this one because it pops up an additional dialog that requires user input
                        continue;
                    }

                    // look at the permissions module the window came from
                    string Module = TYml2Xml.GetAttributeRecursive(ActionNode, "PermissionsRequired");

                    TLstTasks.CurrentLedger = TFrmMainWindowNew.CurrentLedger;

                    // Try to open each screen and log the screens that cannot open
                    try
                    {
                        Assert.AreEqual(String.Empty,
                            TLstTasks.ExecuteAction(ActionNode, null));

                        if (TLstTasks.LastOpenedScreen != null)
                        {
                            TLstTasks.LastOpenedScreen.Close();
                        }

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
                                string permissions = TYml2Xml.GetAttributeRecursive(ActionNode, "PermissionsRequired");
                                string WindowAndModule = ActionNode.Name + Environment.NewLine + "            Permission Required: " + permissions;

                                if (permissions.Contains("SYSMAN"))
                                {
                                    NoSysManPermissionCount++;
                                    sysManPermissions.Add(WindowAndModule);
                                }
                                else
                                {
                                    NoOtherPermissionCount++;
                                    otherPermissions.Add(WindowAndModule);
                                }
                            }
                        }
                        else
                        {
                            BadFailures++;

                            string WindowAndModule = ActionNode.Name + Environment.NewLine + "            Permission Required: " +
                                                     TYml2Xml.GetAttributeRecursive(ActionNode, "PermissionsRequired");

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

                        if ((ledgerNumber != String.Empty) && !UserInfo.GUserInfo.IsInModule(ledger))
                        {
                            NoOtherPermissionCount++;
                            string WindowAndModule = ActionNode.Name + Environment.NewLine + "            Permission Required: " +
                                                     TYml2Xml.GetAttributeRecursive(ActionNode, "PermissionsRequired") +
                                                     Environment.NewLine +
                                                     "                                 " + ledger;
                            otherPermissions.Add(WindowAndModule);
                        }
                        else
                        {
                            BadFailures++;
                            string WindowAndModule = ActionNode.Name + Environment.NewLine + "            Permission Required: " +
                                                     TYml2Xml.GetAttributeRecursive(ActionNode, "PermissionsRequired");

                            if (ledgerNumber != String.Empty)
                            {
                                WindowAndModule += (Environment.NewLine +
                                                    "                                 " + ledger);
                            }

                            notOpened.Add(WindowAndModule);
                        }
                    }
                }
            }

            //Give stats about where failures were
            //feel free to change any formatting, I'm not in love with it right now
            string notOpenedString = string.Join(Environment.NewLine + "      ", notOpened.ToArray());
            string sysManPermissionsString = string.Join(Environment.NewLine + "      ", sysManPermissions.ToArray());
            string otherPermissionsString = string.Join(Environment.NewLine + "      ", otherPermissions.ToArray());
            string workingWindowsString = string.Join(Environment.NewLine + "      ", workingWindows.ToArray());

            //print the permissions the user should have
            TLogging.Log(Environment.NewLine + Environment.NewLine + "User Permissions: " + Environment.NewLine +
                UserInfo.GUserInfo.GetPermissions());

            TLogging.Log(Environment.NewLine + Environment.NewLine + "Statistics: " + Environment.NewLine + "Number of windows opened: " +
                TotalWindowsOpened + Environment.NewLine + "      " + workingWindowsString + Environment.NewLine +
                Environment.NewLine + Environment.NewLine + Environment.NewLine + "SYSMAN Permission Exceptions: " +
                sysManPermissions.Count + Environment.NewLine + "      " + sysManPermissionsString + Environment.NewLine +
                Environment.NewLine + Environment.NewLine + Environment.NewLine + "Other Permission Exceptions: " +
                otherPermissions.Count + Environment.NewLine + "      " + otherPermissionsString + Environment.NewLine +
                Environment.NewLine + Environment.NewLine + "Windows that should be opened but couldn't: " +
                notOpened.Count + Environment.NewLine + "      " + notOpenedString + Environment.NewLine);

            //Now the loop is finished so fail if there were exceptions
            Assert.GreaterOrEqual(TotalWindowsOpened, 190, "Expected to open at least 190 windows");
            Assert.GreaterOrEqual(sysManPermissions.Count, 3, "Expected to fail to open at least 3 windows requiring SYSMAN permissions");
            Assert.AreEqual(notOpened.Count, 0, "Failed to open at least one window for unexplained reasons");
            Assert.AreEqual(otherPermissions.Count,
                0,
                "Unexpected failure to open some windows due to a permissions error, when we should have had sufficient permission");

            if (BadFailures > 0)
            {
                Assert.Fail(String.Format("General failure to successfully open {0} window(s).  Maybe there was an exception??", BadFailures));
            }
        }

        //helper method for the below test
        private static bool TruePermission(XmlNode node, string aUserID, bool ACheckLedgerPermissions)
        {
            return true;
        }

        //another helper method to reset the test below
        private static bool FalsePermission(XmlNode node, string aUserId, bool ACheckLedgerPermissions)
        {
            return false;
        }

        /// <summary>
        /// verify that demo user is unable to fool the server to get SYSADMIN permissions
        /// </summary>
        [Test]
        public void TestBrokenClientPermissions()
        {
            TLogging.Log("Running test 'TestBrokenClientPermissions'..." + Environment.NewLine);

            // Give the user access at the client end to open the screen
            TLstTasks.Init(UserInfo.GUserInfo.UserID, TruePermission);

            // Check that the user is 'demo'
            Assert.AreEqual("demo", UserInfo.GUserInfo.UserID.ToLower(), "Test should be run with DEMO user");

            // get the node that opens the screen TFrmMaintainUsers
            XPathExpression expr = FNavigator.Compile("//*[@ActionOpenScreen='TFrmMaintainUsers']");
            XPathNodeIterator iterator = FNavigator.Select(expr);

            iterator.MoveNext();

            if (iterator.Current is IHasXmlNode)
            {
                XmlNode ActionNode = ((IHasXmlNode)iterator.Current).GetNode();

                TLogging.Log(ActionNode.Name + " " + ActionNode.Attributes["ActionOpenScreen"].Value);

                // Check that the node does not give permission to user 'demo'
                Assert.AreEqual(false, TFrmMainWindowNew.HasAccessPermission(ActionNode, UserInfo.GUserInfo.UserID, false),
                    "user DEMO should not have permissions for TFrmMaintainUsers");

                string errorResult = TLstTasks.ExecuteAction(ActionNode, null);

                // The server should have rejected us
                Assert.AreNotEqual(String.Empty, errorResult, "Demo was able to open the screen!");
                Assert.IsTrue(errorResult.Equals(
                        "No access for user DEMO to Module SYSMAN."), "Expected the fail reason to be module access permission");
            }
        }
    }
}