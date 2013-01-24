//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       pauln
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
using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Xml;

using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Client.CommonForms.Logic;
using NUnit.Framework;

namespace Ict.Testing.Shepherds
{
    /// <summary>
    /// Shepherd Unit Tests.
    /// </summary>
    [TestFixture]
    public class Test
    {
        private string FTestShepherdYAMLFile;
            
        /// <summary>
        /// ...
        /// </summary>
        [SetUp]
        public void Init()
        {
            new TAppSettingsManager("../../etc/TestClient.config");

            FTestShepherdYAMLFile = TAppSettingsManager.GetValue("UINavigation.File").Substring(0, TAppSettingsManager.GetValue("UINavigation.File").IndexOf("UINavigation.yml")) +
                Path.DirectorySeparatorChar + "Shepherd_Church_Definition.yaml";
            new TLogging("test.log");
        }
        
        /// <summary>
        /// TODO Comment
        /// </summary>
        [Test]
        public void TestTPetraShepherdParseYAMLFileElementsMethod()
        {
            //This method needs to be tested, still, but I am not sure how to do it yet because the attributes that it currently
            //collects are not being saved anywhere in the logic yet; it just peeks at them. We need to figure out what to do with
            //the attributes that are collected by ParseYAMLFileELements()
            TestLogicInterface testShepherdInterface = new TestLogicInterface();
            TPetraShepherdFormLogic testParseYAMLFileELementsLogic =
                new TPetraShepherdFormLogic(FTestShepherdYAMLFile, testShepherdInterface);
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        [Test]
        public void TestTPetraShepherdPagesListConstructor()
        {
            TPetraShepherdPagesList testPetraShepherdPagesList =
                new TPetraShepherdPagesList(FTestShepherdYAMLFile);

            Assert.True(testPetraShepherdPagesList.Pages.Count == 4,
                "Wrong shepherd page count; expected: " + testPetraShepherdPagesList.Pages.Count);

            Assert.True(testPetraShepherdPagesList.Pages.ContainsKey("5"),
                "Shepherd did not contain key 5");
            Assert.True(testPetraShepherdPagesList.Pages.ContainsKey("56"),
                "Shepherd did not contain key 56");
            Assert.True(testPetraShepherdPagesList.Pages.ContainsKey("12"),
                "Shepherd did not contain key 12");

            Assert.False(testPetraShepherdPagesList.Pages.ContainsKey("123"),
                "Shepherd inadvertently contained key 123");
            Assert.False(testPetraShepherdPagesList.Pages.ContainsKey("1532"),
                "Shepherd inadvertently contained key 1532");
            Assert.False(testPetraShepherdPagesList.Pages.ContainsKey("00"),
                "Shepherd inadvertently contained key 00");

            foreach (KeyValuePair<string, TPetraShepherdPage> pair in testPetraShepherdPagesList.Pages)
            {
                switch (pair.Key)
                {
                    case "5":
                        Assert.AreEqual(pair.Value.ID, "5", "The shepherd page 5 did not have the key 5");
                        Assert.AreEqual(pair.Value.Title, "Personnel Shepherd",
                        "The shepherd page 5 did not have the name Personnel Shepherd");
                        Assert.AreEqual(pair.Value.Note, "This is a test, this is only a test.",
                        "The note did not match: This is a test, this ois only a test.");
                        Assert.True(pair.Value.Visible, "The page was not visible.");
                        Assert.True(pair.Value.Enabled, "The page was not enabled");
                        Assert.AreEqual(pair.Value.UserControlNamespace, "Ict.Petra.Client.MPartner.Gui",
                        "The namespace was not: Ict.Petra.Client.MPartner.Gui");
                        Assert.AreEqual(pair.Value.UserControlClassName, "TUC_PartnerDetails_Church",
                        "The user control class name was not: TUC_PartnerDetails_Church");
                        Assert.AreEqual(pair.Value.HelpContext, "HELPME!!!", "The help context was not: HELPME!!!");
                        break;
                }
            }
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        [Test]
        public void TestPetraShepherdFormLogicHandleActionNextNormalCircumstances()
        {
            TestLogicInterface thisIsAtest = new TestLogicInterface();
            TPetraShepherdFormLogic testFormLogic =
                new TPetraShepherdFormLogic(FTestShepherdYAMLFile, thisIsAtest);

            //TESTS to show that the HandleActionNext() method moves from page to page under normal operating circumstances..
            Assert.AreEqual(testFormLogic.CurrentPage.ID, "5");

            testFormLogic.HandleActionNext();
            Assert.AreEqual(testFormLogic.CurrentPage.ID, "56");

            testFormLogic.HandleActionNext();
            Assert.AreEqual(testFormLogic.CurrentPage.ID, "12");

            testFormLogic.HandleActionNext();
            Assert.AreEqual(testFormLogic.CurrentPage.ID, "FINISHPAGE_MASTER");
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        [Test]
        public void TestPetraShepherdFormLogicHandleActionNextOneInvisible()
        {
            //TESTS to show that the HandleActionNext() method moves from page to
            //page when there is a Page that is not visible
            TestLogicInterface thisIsAtest = new TestLogicInterface();
            TPetraShepherdFormLogic testNotVisibleLogic =
                new TPetraShepherdFormLogic(FTestShepherdYAMLFile, thisIsAtest);

            foreach (KeyValuePair<string, TPetraShepherdPage> pair in testNotVisibleLogic.ShepherdPages.Pages)
            {
                switch (pair.Key)
                {
                    case "56":
                        pair.Value.Visible = false;
                        break;
                }
            }

            Assert.True(testNotVisibleLogic.CurrentPage.Visible);

            testNotVisibleLogic.HandleActionNext();
            Assert.AreEqual(testNotVisibleLogic.CurrentPage.ID, "12");
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        [Test]
        public void TestPetraShepherdFormLogicHandleActionNextOneUnenabled()
        {
            //TESTS to show that the HandleActionNext() method moves from page to page when
            //there is a Page that is not enabled
            TestLogicInterface thisIsAtest = new TestLogicInterface();
            TPetraShepherdFormLogic testNotEnabledLogic =
                new TPetraShepherdFormLogic(FTestShepherdYAMLFile, thisIsAtest);

            foreach (KeyValuePair<string, TPetraShepherdPage> pair in testNotEnabledLogic.ShepherdPages.Pages)
            {
                switch (pair.Key)
                {
                    case "56":
                        pair.Value.Enabled = false;
                        break;
                }
            }

            Assert.True(testNotEnabledLogic.CurrentPage.Enabled);

            testNotEnabledLogic.HandleActionNext();
            Assert.AreEqual(testNotEnabledLogic.CurrentPage.ID, "12");
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        [Test]
        public void TestPetraShepherdFormLogicHandleActionNextOneInvisibleOneUnenabled()
        {
            //TESTS to show that the HandleActionNext() method moves from page to page when
            //there is a Page that is not visible nor enabled
            TestLogicInterface thisIsAtest = new TestLogicInterface();
            TPetraShepherdFormLogic testNotVisibleOrEnabledLogic =
                new TPetraShepherdFormLogic(FTestShepherdYAMLFile, thisIsAtest);

            foreach (KeyValuePair<string, TPetraShepherdPage> pair in testNotVisibleOrEnabledLogic.ShepherdPages.Pages)
            {
                switch (pair.Key)
                {
                    case "56":
                        pair.Value.Visible = false;
                        pair.Value.Enabled = false;
                        break;
                }
            }

            Assert.True(testNotVisibleOrEnabledLogic.CurrentPage.Visible);

            testNotVisibleOrEnabledLogic.HandleActionNext();
            Assert.AreEqual(testNotVisibleOrEnabledLogic.CurrentPage.ID, "12");
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        [Test]
        public void TestPetraShepherdFormLogicSwitchToStartPage()
        {
            TestLogicInterface test = new TestLogicInterface();
            TPetraShepherdFormLogic testFormLogicSwitchToStartPage =
                new TPetraShepherdFormLogic(FTestShepherdYAMLFile, test);

            Assert.AreEqual(testFormLogicSwitchToStartPage.CurrentPage.ID, "5",
                "The first page was, unexpectedly, not 5.");
            testFormLogicSwitchToStartPage.HandleActionNext();
            testFormLogicSwitchToStartPage.SwitchToStartPage();
            Assert.AreEqual(testFormLogicSwitchToStartPage.CurrentPage.ID, "5",
                "Did not correctly switch back to the start page.");
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        [Test]
        public void TestPetraShepherdFormLogicSwitchToFinishPage()
        {
            TestLogicInterface test = new TestLogicInterface();
            TPetraShepherdFormLogic testFomLogicSwitchToFinishPage =
                new TPetraShepherdFormLogic(FTestShepherdYAMLFile, test);

            Assert.AreEqual(testFomLogicSwitchToFinishPage.CurrentPage.ID, "5",
                "The first page was, unexpectly, not 5.");
            testFomLogicSwitchToFinishPage.SwitchToFinishPage();
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        [Test]
        public void TestPetraShepherdFormLogicSwitchToBackPage()
        {
            TestLogicInterface test = new TestLogicInterface();
            TPetraShepherdFormLogic testFormLogicBackButton =
                new TPetraShepherdFormLogic(FTestShepherdYAMLFile, test);

            Assert.AreEqual(testFormLogicBackButton.CurrentPage.ID, "5",
                "The first page of the shephred was not 5, as expected.");
            testFormLogicBackButton.HandleActionNext();
            Assert.AreEqual(testFormLogicBackButton.CurrentPage.ID, "56",
                "The second page of the shepherd was not 56, as expected.");
            testFormLogicBackButton.HandleActionBack();
            Assert.AreEqual(testFormLogicBackButton.CurrentPage.ID, "5",
                "The shepherd should have jumped back to page 5, but didn't.");
            testFormLogicBackButton.HandleActionNext();
            testFormLogicBackButton.HandleActionNext();
            testFormLogicBackButton.HandleActionBack();
            Assert.AreEqual(testFormLogicBackButton.CurrentPage.ID, "56",
                "The shepherd should have jumped back to page 56, but didn't.");
            testFormLogicBackButton.HandleActionBack();
            testFormLogicBackButton.HandleActionBack();
            Assert.AreEqual(testFormLogicBackButton.CurrentPage.ID, "5",
                "The shepherd should not have jumped farther back than 5.");
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        [Test]
        public void TestPetraShepherdFormLogicParseYAMLFileElements()
        {
            TestLogicInterface TestLogicInterface = new TestLogicInterface();
            TPetraShepherdFormLogic testFormUpdateShepherdFormProperties =
                new TPetraShepherdFormLogic(FTestShepherdYAMLFile, TestLogicInterface);
            ////how do we test the elements of a GUI element?
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        [Test]
        public void TestPetraShepherdFormLogicGetCurrentPageNumber()
        {
            TestLogicInterface TestLogicInterface = new TestLogicInterface();
            TPetraShepherdFormLogic testGetCurrentPageNumber =
                new TPetraShepherdFormLogic(FTestShepherdYAMLFile, TestLogicInterface);

            Assert.AreEqual(testGetCurrentPageNumber.GetCurrentPageNumber(), 1, "The current page was not 1.");
            testGetCurrentPageNumber.HandleActionNext();
            Assert.AreEqual(testGetCurrentPageNumber.GetCurrentPageNumber(), 2, "The current page was not 2.");
        }

        /// <summary>
        /// TODO Comment
        /// </summary>
        [Test]
        public void TestPetraShepherdFormLogicEnumeratePages()
        {
            TestLogicInterface TestLogicInterface = new TestLogicInterface();
            TPetraShepherdFormLogic testEnumeratePages =
                new TPetraShepherdFormLogic(FTestShepherdYAMLFile, TestLogicInterface);

            Assert.AreEqual(testEnumeratePages.EnumeratePages(), 4,
                "The current number of visible and inactive pages was not 4.");
            testEnumeratePages.HandleActionNext();
            testEnumeratePages.CurrentPage.Enabled = false;
            testEnumeratePages.SwitchToStartPage();
            Assert.AreEqual(testEnumeratePages.EnumeratePages(), 4,
                "The current number of visible and inactive pages was not 3. It may have been tricked up" +
                "by the fact that there was a change of what's visible/enabled.");
        }
    }
}