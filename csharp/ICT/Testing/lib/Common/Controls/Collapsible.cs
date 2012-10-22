//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Seth Bird (sethb)
//       christiank
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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Globalization;
using System.Xml;
using System.Windows.Forms;
using NUnit.Framework;
using NUnit.Framework.Constraints;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.IO;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Testing.NUnitTools;

namespace Tests.Common.Controls
{
    ///<summary>
    /// This makes sure that on every property change, and on any constructor call, the collapsible panel never enters
    /// an unstable state where a runtime exception is thrown. This is enforcable via:
    ///   1) automatically defaulting to default values when left blank
    ///   2) Simply ignoring the instruction if it would cause a problem.
    ///</summary>
    [TestFixture]
    public class TTestCollapsible
    {
        const string HOSTEDUSERCONTROL = "Ict.Petra.Client.MPartner.Gui.TUC_PartnerInfo";

        TPnlCollapsible FPnl;

        #region helperfunctions/setup/teardown

        ///<summary>
        /// This function defines what a "stable" state is -- meaning that if the pnlCollapsible is
        /// stable, then there should not be any chance of a runtime error (except for the case of the usercontrolnamespace
        /// string being wrong. That cannot be be checked correctly before runtime).
        ///</summary>
        void assertIsStable(TPnlCollapsible APnl, bool ACheckHckDataMatch = true)
        {
            APnl.AssertDirStyleMatch();

            if (ACheckHckDataMatch)
            {
                APnl.AssertHckDataMatch();
            }

            Assert.AreEqual(null, APnl.TaskListNode); //required because expand() may be called at any moment, and it will throw an error if TaskListNode is null. So we require it's existance at all times.
        }

        ///<summary>
        /// This function defines what a "stable" state is -- meaning that if the pnlCollapsible is
        /// stable, then there should not be any chance of a runtime error (except for the case of the usercontrolnamespace
        /// string being wrong. That cannot be be checked correctly before runtime).
        ///</summary>
        void assertIsStable2(TPnlCollapsible APnl)
        {
            APnl.AssertDirStyleMatch();
            APnl.AssertHckDataMatch();

            Assert.AreNotEqual(null, APnl.TaskListNode); //required because expand() may be called at any moment, and it will throw an error if TaskListNode is null. So we require it's existance at all times.
        }

        ///<summary>
        /// This function defines what a "stable" state is -- meaning that if the pnlCollapsible is
        /// stable, then there should not be any chance of a runtime error (except for the case of the usercontrolnamespace
        /// string being wrong. That cannot be be checked correctly before runtime).
        ///</summary>
        void assertIsStable3(TPnlCollapsible APnl)
        {
            APnl.AssertDirStyleMatch();
            APnl.AssertHckDataMatch();

            Assert.AreNotEqual(null, APnl.TaskListNode); //required because expand() may be called at any moment, and it will throw an error if TaskListNode is null. So we require it's existance at all times.
        }

        ///<summary/>
        [SetUp]
        public void Setup()
        {
            new TLogging("TestCommonControls.log");

            this.FPnl = new TPnlCollapsible(new object[] { THostedControlKind.hckUserControl, HOSTEDUSERCONTROL });
            assertIsStable(FPnl);
        }

        #endregion


        ///<summary/>
        [Test]
        public void TestUserControlStringProperty()
        {
            FPnl.UserControlString = "System.Console.Write";
            Assert.AreEqual("System.Console", FPnl.UserControlNamespace);
            Assert.AreEqual("Write", FPnl.UserControlClass);
            Assert.AreEqual("System.Console.Write", FPnl.UserControlString);
            Assert.AreNotEqual("System.CONSOLE.Write", FPnl.UserControlString);

            assertIsStable(FPnl);
        }

        ///<summary/>
        [Test]
        public void StableAfterCollapseExpand()
        {
            FPnl.Expand();
            assertIsStable(FPnl);
            FPnl.Collapse();
            assertIsStable(FPnl);
        }

        #region test Constructors (including that all input types are recognised and handled correctly

        /// <summary/>
        [Test]
        public void TestConstructorDefault()
        {
            FPnl = new TPnlCollapsible(new object[] { });

            assertIsStable(FPnl, false);

            FPnl = new TPnlCollapsible(new object[] { });
            FPnl.UserControlNamespace = HOSTEDUSERCONTROL;

            assertIsStable(FPnl);

            // To ensure Unit Test code coverage only - nothing to assert here...
            FPnl.InitUserControl();
        }

        /// <summary/>
        [Test]
        public void TestConstructorHck()
        {
            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl);
            Assert.AreEqual(THostedControlKind.hckUserControl, FPnl.HostedControlKind);

            try
            {
                assertIsStable(FPnl);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(EInsufficientDataSetForHostedControlKindException), e.GetType());
            }
        }

        /// <summary/>
        [Test]
        public void TestConstructorInteger()
        {
            FPnl = new TPnlCollapsible(1337);
            Assert.AreEqual(1337, FPnl.ExpandedSize);
            FPnl.UserControlNamespace = HOSTEDUSERCONTROL;
            assertIsStable(FPnl);
        }

        /// <summary/>
        [Test]
        public void TestConstructorString()
        {
            FPnl = new TPnlCollapsible("System.Console.Write");
            Assert.AreEqual("System.Console", FPnl.UserControlNamespace);
            Assert.AreEqual("Write", FPnl.UserControlClass);
            Assert.AreEqual("System.Console.Write", FPnl.UserControlString);

            assertIsStable(FPnl);
        }

        /// <summary/>
        [Test]
        public void TestConstructorStringMalformed()
        {
            //again, we intentionally don't bother checking the usercontrol stuff very much at all. Below is expected behavior
            FPnl = new TPnlCollapsible("malformed UserControlString.");
            Assert.AreEqual("malformed UserControlString", FPnl.UserControlNamespace);
            Assert.AreEqual("", FPnl.UserControlClass);
            Assert.AreEqual("malformed UserControlString.", FPnl.UserControlString);

            assertIsStable(FPnl);
        }

        /// <summary/>
        [Test]
        public void TestConstructorStringMalformedNoPeriod()
        {
            //again, we intentionally don't bother checking the usercontrol stuff very much at all. Below is expected behavior
            //This is more for documentation for now.
            FPnl = new TPnlCollapsible("malformed UserControlString with no period at end");
            Assert.AreEqual("", FPnl.UserControlNamespace);
            Assert.AreEqual("", FPnl.UserControlClass);
            Assert.AreEqual(".", FPnl.UserControlString);

            try
            {
                assertIsStable(FPnl);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(EInsufficientDataSetForHostedControlKindException), e.GetType());
            }
        }

        /// <summary/>
        [Test]
        public void TestConstructorDirection()
        {
            FPnl = new TPnlCollapsible(TCollapseDirection.cdHorizontal);
            Assert.AreEqual(TPnlCollapsible.DEFAULT_STYLE[TCollapseDirection.cdHorizontal], FPnl.VisualStyleEnum);
            Assert.AreEqual(TCollapseDirection.cdHorizontal, FPnl.CollapseDirection);
            FPnl = new TPnlCollapsible(TCollapseDirection.cdVertical);
            Assert.AreEqual(TPnlCollapsible.DEFAULT_STYLE[TCollapseDirection.cdVertical], FPnl.VisualStyleEnum);
            Assert.AreEqual(TCollapseDirection.cdVertical, FPnl.CollapseDirection);

            assertIsStable(FPnl, false);
        }

        /// <summary/>
        [Test]
        public void TestConstructorVisualStyle()
        {
            FPnl = new TPnlCollapsible(TVisualStylesEnum.vsDashboard);
            Assert.AreEqual(TVisualStylesEnum.vsDashboard, FPnl.VisualStyleEnum);

            assertIsStable(FPnl, false);
        }

        /// <summary/>
        [Test]
        public void TestConstructorVisualStyleBad()
        {
            // This should not change the style because vsShepherd isn't compatible with
            // default direction of vertical, and direction trumps style.
            FPnl = new TPnlCollapsible(TVisualStylesEnum.vsShepherd);
            Assert.AreNotEqual(TVisualStylesEnum.vsShepherd, FPnl.VisualStyleEnum);
            Assert.AreEqual(TPnlCollapsible.DEFAULT_STYLE[FPnl.CollapseDirection], FPnl.VisualStyleEnum);

            assertIsStable(FPnl, false);
        }

        /// <summary/>
        [Test]
        public void TestConstructorStyleDirection()
        {
            FPnl = new TPnlCollapsible(TVisualStylesEnum.vsShepherd, TCollapseDirection.cdHorizontal);
            Assert.AreEqual(TVisualStylesEnum.vsShepherd, FPnl.VisualStyleEnum);
            Assert.AreEqual(TCollapseDirection.cdHorizontal, FPnl.CollapseDirection);

            assertIsStable(FPnl, false);
        }

        /// <summary/>
        [Test]
        public void TestConstructorDirectionStyle()
        {
            FPnl = new TPnlCollapsible(TCollapseDirection.cdHorizontal, TVisualStylesEnum.vsShepherd);
            Assert.AreEqual(TVisualStylesEnum.vsShepherd, FPnl.VisualStyleEnum);
            Assert.AreEqual(TCollapseDirection.cdHorizontal, FPnl.CollapseDirection);

            assertIsStable(FPnl, false);
        }

        /// <summary/>
        [Test]
        public void TestConstructorBool()
        {
            FPnl = new TPnlCollapsible(false);
            Assert.AreEqual(false, FPnl.IsCollapsed);

            assertIsStable(FPnl, false);
        }

        /// <summary/>
        [Test]
        public void TestConstructorXmlNode()
        {
            XmlNode TestXmlNode = TTestTaskList.GetTestXmlNode();

            FPnl = new TPnlCollapsible(TestXmlNode, THostedControlKind.hckTaskList);
            Assert.AreEqual(TestXmlNode, FPnl.TaskListNode);
            assertIsStable2(FPnl);

            //Now we want to see if we can realise the task list without it throwing an exception.
            FPnl.Expand();
            assertIsStable2(FPnl);
        }

        /// <summary/>
        [Test]
        public void TestConstructorTaskListWithoutXmlNode()
        {
            FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList);

            try
            {
                assertIsStable2(FPnl);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(EInsufficientDataSetForHostedControlKindException), e.GetType());
            }

            //Now we want to check that we can't realise the task list without it throwing an exception.
            try
            {
                FPnl.Expand();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ENoTaskListNodeSpecifiedException), e.GetType());
            }
        }

        /// <summary/>
        [Test]
        public void TestConstructorUserControlWithoutUserControlString()
        {
            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl);

            try
            {
                assertIsStable(FPnl);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(EInsufficientDataSetForHostedControlKindException), e.GetType());
            }

            //Now we want to check that we can't realise the task list without it throwing an exception.
            try
            {
                FPnl.Expand();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(EUserControlInvalidNamespaceSpecifiedException), e.GetType());
            }
        }

        /// <summary/>
        [Test]
        public void TestConstructorUserControlWithInvalidClass()
        {
            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, HOSTEDUSERCONTROL + "WRONG");

            assertIsStable(FPnl);

            //Now we want to check that we can't realise the task list without it throwing an exception.
            try
            {
                FPnl.Expand();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(EUserControlCantInstantiateClassException), e.GetType());
            }
        }

        /// <summary/>
        [Test]
        public void TestConstructorMultipleSameArguments()
        {
            // when given multiple arguments of same type, simply use the latest.
            FPnl = new TPnlCollapsible("first.control.given", "System.Console.Write");
            Assert.AreEqual("System.Console", FPnl.UserControlNamespace);
            Assert.AreEqual("Write", FPnl.UserControlClass);
            Assert.AreEqual("System.Console.Write", FPnl.UserControlString);

            assertIsStable(FPnl);
        }

        /// <summary/>
        [Test]
        public void TestConstructorWinFormsDesigner()
        {
            // To ensure Unit Test code coverage only - nothing to assert here...
            FPnl = new TPnlCollapsible();
        }

        #endregion

        #region test Property Setters

        // don't need to test each property setter function since that's implicitly tested by the cunstructor tests which call the setters.
        // Except for the following Setters, which the constructor does not support/use.

        ///<summary/>
        [Test]
        public void TestSetter_Text()
        {
            String testString = "new Title";

            FPnl.Text = testString;
            Assert.AreEqual(testString, FPnl.Text);

            assertIsStable(FPnl);
        }

        ///<summary/>
        [Test]
        public void TestSetter_CollapsiblePanelHosterInstance()
        {
            FPnl = new TPnlCollapsible(THostedControlKind.hckCollapsiblePanelHoster, TTestTaskList.GetTestXmlNode());
            FPnl.RealiseCollapsiblePanelHoster();

            TPnlCollapsibleHoster CpH1 = FPnl.CollapsiblePanelHosterInstance;

            TPnlCollapsibleHoster CpH2 = new TPnlCollapsibleHoster(TTestTaskList.GetTestXmlNode2(), TVisualStylesEnum.vsTaskPanel);

            FPnl.CollapsiblePanelHosterInstance = CpH2;

            Assert.AreNotEqual(CpH1, FPnl.CollapsiblePanelHosterInstance);

            assertIsStable2(FPnl);
        }

        ///<summary/>
        [Test]
        public void TestSetter_TaskListInstance()
        {
            FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList, TTestTaskList.GetTestXmlNode());
            FPnl.RealiseTaskListNow();

            TTaskList TLst1 = FPnl.TaskListInstance;
            int ExpandedSize = FPnl.ExpandedSize;

            TTaskList TLst2 = new TTaskList(TTestTaskList.GetTestXmlNode2());

            FPnl.TaskListInstance = TLst2;

            Assert.AreNotEqual(TLst1, FPnl.TaskListInstance);
            Assert.AreNotEqual(ExpandedSize, FPnl.ExpandedSize);

            assertIsStable2(FPnl);
        }

        ///<summary/>
        [Test]
        public void TestSetter_UserControlClass_UserControlNamespace_UserControlString()
        {
            // Changing of UserControl after realising a first UserControl specified through Constructor
            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, HOSTEDUSERCONTROL);
            FPnl.RealiseUserControlNow();

            UserControl UC1 = FPnl.UserControlInstance;

            FPnl.UserControlClass = "TUC_Subscription";
            FPnl.UserControlNamespace = "Ict.Petra.Client.MPartner.Gui";
            FPnl.RealiseUserControlNow();

            Assert.AreNotEqual(UC1, FPnl.UserControlInstance);
            Assert.IsInstanceOf <Ict.Petra.Client.MPartner.Gui.TUC_Subscription>(FPnl.UserControlInstance);

            // Assigning of UserControl with UserControlClass and UserControlNamespace Properties
            FPnl = new TPnlCollapsible(new object[] { });
            FPnl.UserControlClass = "TUC_Subscription";
            FPnl.UserControlNamespace = "Ict.Petra.Client.MPartner.Gui";
            FPnl.RealiseUserControlNow();

            Assert.IsInstanceOf <Ict.Petra.Client.MPartner.Gui.TUC_Subscription>(FPnl.UserControlInstance);


            // Assigning of UserControl with UserControlString Property
            FPnl = new TPnlCollapsible(new object[] { });
            FPnl.UserControlString = "Ict.Petra.Client.MPartner.Gui.TUC_Subscription";

            // Calling Expand() has the side effect of instantiating the UserControl
            FPnl.Expand();

            UserControl UC2 = FPnl.UserControlInstance;
            Assert.IsInstanceOf <Ict.Petra.Client.MPartner.Gui.TUC_Subscription>(UC2);

            // Assures that a further call only makes a previously instantiated UserControl visible again
            FPnl.Expand();
            Assert.AreEqual(UC2, FPnl.UserControlInstance);
        }

        ///<summary/>
        [Test]
        public void TestSetter_VisualStyleEnum()
        {
            FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList, TTestTaskList.GetTestXmlNode());

            Assert.AreEqual(TVisualStylesEnum.vsAccordionPanel, FPnl.VisualStyleEnum);

            FPnl.VisualStyleEnum = TVisualStylesEnum.vsTaskPanel;
            Assert.AreEqual(TVisualStylesEnum.vsTaskPanel, FPnl.VisualStyleEnum);

            FPnl.Collapse();  // To ensure Unit Test code coverage only - nothing to assert here...
            FPnl.Expand();  // To ensure Unit Test code coverage only - nothing to assert here...
            FPnl.RealiseTaskListNow();  // To ensure Unit Test code coverage only - nothing to assert here...

            FPnl.VisualStyleEnum = TVisualStylesEnum.vsAccordionPanel;
            Assert.AreEqual(TVisualStylesEnum.vsAccordionPanel, FPnl.VisualStyleEnum);

            FPnl.VisualStyleEnum = TVisualStylesEnum.vsDashboard;
            Assert.AreEqual(TVisualStylesEnum.vsDashboard, FPnl.VisualStyleEnum);
            FPnl.Expand();  // To ensure Unit Test code coverage only - nothing to assert here...

            // The following Tests should work with 'Assert.AreNotEqual' because the Visual Style we try to assign is only valid
            // for a different CollapseDirection: cdHorizontal or cdHorizontalRight
            FPnl.VisualStyleEnum = TVisualStylesEnum.vsHorizontalCollapse;
            Assert.AreNotEqual(TVisualStylesEnum.vsHorizontalCollapse, FPnl.VisualStyleEnum);

            FPnl.VisualStyleEnum = TVisualStylesEnum.vsShepherd;
            Assert.AreNotEqual(TVisualStylesEnum.vsShepherd, FPnl.VisualStyleEnum);


            // Specifiying 'TVisualStylesEnum.vsHorizontalCollapse' in the next statement, which is invalid and...
            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, HOSTEDUSERCONTROL, TVisualStylesEnum.vsHorizontalCollapse);

            // ...gets automatically corrected to TVisualStylesEnum.vsDashboard by the Control!
            Assert.AreEqual(TVisualStylesEnum.vsDashboard, FPnl.VisualStyleEnum);

            FPnl.VisualStyleEnum = TVisualStylesEnum.vsAccordionPanel;
            Assert.AreEqual(TVisualStylesEnum.vsAccordionPanel, FPnl.VisualStyleEnum);

            FPnl.VisualStyleEnum = TVisualStylesEnum.vsTaskPanel;
            Assert.AreEqual(TVisualStylesEnum.vsTaskPanel, FPnl.VisualStyleEnum);

            FPnl.VisualStyleEnum = TVisualStylesEnum.vsDashboard;
            Assert.AreEqual(TVisualStylesEnum.vsDashboard, FPnl.VisualStyleEnum);

            // The following Tests should work with 'Assert.AreNotEqual' because the Visual Style we try to assign is only valid
            // for a different CollapseDirection: cdHorizontal or cdHorizontalRight
            FPnl.VisualStyleEnum = TVisualStylesEnum.vsHorizontalCollapse;
            Assert.AreNotEqual(TVisualStylesEnum.vsHorizontalCollapse, FPnl.VisualStyleEnum);

            FPnl.VisualStyleEnum = TVisualStylesEnum.vsShepherd;
            Assert.AreNotEqual(TVisualStylesEnum.vsShepherd, FPnl.VisualStyleEnum);


            FPnl = new TPnlCollapsible(THostedControlKind.hckCollapsiblePanelHoster, TTestTaskList.GetTestXmlNode());

            Assert.AreEqual(TVisualStylesEnum.vsAccordionPanel, FPnl.VisualStyleEnum);

            FPnl.VisualStyleEnum = TVisualStylesEnum.vsTaskPanel;
            Assert.AreEqual(TVisualStylesEnum.vsTaskPanel, FPnl.VisualStyleEnum);

            FPnl.VisualStyleEnum = TVisualStylesEnum.vsAccordionPanel;
            Assert.AreEqual(TVisualStylesEnum.vsAccordionPanel, FPnl.VisualStyleEnum);

            FPnl.VisualStyleEnum = TVisualStylesEnum.vsDashboard;
            Assert.AreEqual(TVisualStylesEnum.vsDashboard, FPnl.VisualStyleEnum);

            // The following Tests should work with 'Assert.AreNotEqual' because the Visual Style we try to assign is only valid
            // for a different CollapseDirection: cdHorizontal or cdHorizontalRight
            FPnl.VisualStyleEnum = TVisualStylesEnum.vsHorizontalCollapse;
            Assert.AreNotEqual(TVisualStylesEnum.vsHorizontalCollapse, FPnl.VisualStyleEnum);

            FPnl.VisualStyleEnum = TVisualStylesEnum.vsShepherd;
            Assert.AreNotEqual(TVisualStylesEnum.vsShepherd, FPnl.VisualStyleEnum);


            FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList,
                TTestTaskList.GetTestXmlNode(), TVisualStylesEnum.vsShepherd, TCollapseDirection.cdHorizontal);

            Assert.AreEqual(TVisualStylesEnum.vsShepherd, FPnl.VisualStyleEnum);

            FPnl.VisualStyleEnum = TVisualStylesEnum.vsHorizontalCollapse;
            Assert.AreEqual(TVisualStylesEnum.vsHorizontalCollapse, FPnl.VisualStyleEnum);


            // To ensure Unit Test code coverage only - nothing to assert here...
            FPnl.RealiseTaskListNow();
            FPnl.VisualStyleEnum = TVisualStylesEnum.vsShepherd;


            // To ensure Unit Test code coverage only - nothing to assert here...
            FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList,
                TTestTaskList.GetTestXmlNode(), TVisualStylesEnum.vsHorizontalCollapse, TCollapseDirection.cdHorizontal);
            FPnl.VisualStyleEnum = TVisualStylesEnum.vsHorizontalCollapse_InfoPanelWithGradient;
        }

        /// <summary>
        /// Tests the ActiveTaskItem Property.
        /// </summary>
        [Test]
        public void TestSetter_ActiveTaskItem()
        {
            XmlNode TestNode = TTestTaskList.GetTestXmlNode();

            FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList, TestNode);
            FPnl.Expand();

            Assert.IsNull(FPnl.ActiveTaskItem);

            FPnl.ActiveTaskItem = TestNode.ChildNodes[1];
            Assert.AreEqual(FPnl.ActiveTaskItem, TestNode.ChildNodes[1]);

            FPnl.ActiveTaskItem = TestNode.ChildNodes[2];
            Assert.AreEqual(FPnl.ActiveTaskItem, TestNode.ChildNodes[2]);

            FPnl.ActiveTaskItem = null;

            Assert.IsNull(FPnl.ActiveTaskItem);

            FPnl.TaskListNode = null;

            Assert.IsNull(FPnl.ActiveTaskItem);
        }

        #endregion

        ///<summary />
        [Test]
        public void TestCollapsiblePanelHosterOtherTests()
        {
            // Ensure ENoTaskListNodeSpecifiedException is thrown if no TaskListNode got specified
            FPnl = new TPnlCollapsible(THostedControlKind.hckCollapsiblePanelHoster);

            try
            {
                FPnl.RealiseCollapsiblePanelHoster();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ENoTaskListNodeSpecifiedException), e.GetType());
            }

            // To ensure Unit Test code coverage only - nothing to assert here...
            FPnl = new TPnlCollapsible(THostedControlKind.hckCollapsiblePanelHoster,
                TTestTaskList.GetTestXmlNode(), TCollapseDirection.cdHorizontalRight);
            FPnl.VisualStyleEnum = TVisualStylesEnum.vsHorizontalCollapse;

            FPnl.RealiseCollapsiblePanelHoster();

            // To ensure Unit Test code coverage only - nothing to assert here...
            FPnl.Expand();
        }

        ///<summary>
        /// This makes sure that the content of panel undergoes lazy evaluation.
        /// ie, TaskListInstance and/or UserControlInstance are not defined before expand().
        /// </summary>
        /// <remarks>See also <see cref="TestLazyInitialisationCumulative" />!</remarks>
        [Test]
        public void TestLazyInitialisation()
        {
            // UserControl
            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, HOSTEDUSERCONTROL);
            Assert.AreEqual(null, FPnl.UserControlInstance);    // Lazy initialisation!
            Assert.AreEqual(null, FPnl.TaskListInstance);
            Assert.AreEqual(null, FPnl.CollapsiblePanelHosterInstance);

            assertIsStable(FPnl);

            FPnl.Expand();
            Assert.AreNotEqual(null, FPnl.UserControlInstance); // Now initialised as Expand() got called
            Assert.AreEqual(null, FPnl.TaskListInstance);
            Assert.AreEqual(null, FPnl.CollapsiblePanelHosterInstance);

            assertIsStable(FPnl);
            FPnl.Collapse();
            assertIsStable(FPnl);

            // TaskList
            FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList, TTestTaskList.GetTestXmlNode());
            Assert.AreEqual(null, FPnl.TaskListInstance);  // Lazy initialisation!

            assertIsStable2(FPnl);

            FPnl.Expand();
            Assert.AreNotEqual(null, FPnl.TaskListInstance);     // Now initialised as Expand() got called
            Assert.AreEqual(null, FPnl.UserControlInstance);
            Assert.AreEqual(null, FPnl.CollapsiblePanelHosterInstance);

            assertIsStable2(FPnl);
            FPnl.Collapse();
            assertIsStable2(FPnl);

            // CollapsiblePanelHoster
            FPnl = new TPnlCollapsible(THostedControlKind.hckCollapsiblePanelHoster, TTestTaskList.GetTestXmlNode());
            Assert.AreEqual(null, FPnl.CollapsiblePanelHosterInstance);  // Lazy initialisation!

            assertIsStable2(FPnl);

            FPnl.Expand();
            Assert.AreNotEqual(null, FPnl.CollapsiblePanelHosterInstance); // Now initialised as Expand() got called
            Assert.AreEqual(null, FPnl.UserControlInstance);
            Assert.AreEqual(null, FPnl.TaskListInstance);


            assertIsStable2(FPnl);
            FPnl.Collapse();
            assertIsStable2(FPnl);
        }

        ///<summary>
        /// This makes sure that the content of panel undergoes lazy evaluation.
        /// ie, TaskListInstance and/or UserControlInstance are not defined before expand().
        /// </summary>
        /// <remarks>Similar to <see cref="TestLazyInitialisation" />, but cumulative - it changes the HostedControlKind
        /// on the same instance of <see cref="TPnlCollapsible" />!</remarks>
        [Test]
        public void TestLazyInitialisationCumulative()
        {
            // UserControl
            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, HOSTEDUSERCONTROL);
            Assert.AreEqual(null, FPnl.UserControlInstance);    // Lazy initialisation!
            Assert.AreEqual(null, FPnl.TaskListInstance);
            Assert.AreEqual(null, FPnl.CollapsiblePanelHosterInstance);

            assertIsStable(FPnl);

            FPnl.Expand();
            Assert.AreNotEqual(null, FPnl.UserControlInstance); // Now initialised as Expand() got called
            Assert.AreEqual(null, FPnl.TaskListInstance);
            Assert.AreEqual(null, FPnl.CollapsiblePanelHosterInstance);

            FPnl.TaskListNode = TTestTaskList.GetTestXmlNode();
            Assert.AreEqual(null, FPnl.TaskListInstance);  // Lazy initialisation!

            // TaskList
            FPnl.HostedControlKind = THostedControlKind.hckTaskList;
            FPnl.Collapse();
            FPnl.Expand();
            Assert.AreNotEqual(null, FPnl.TaskListInstance);     // Now initialised as Expand() got called
            Assert.AreNotEqual(null, FPnl.UserControlInstance);  // This still exists from previous operation!
            Assert.AreEqual(null, FPnl.CollapsiblePanelHosterInstance);

            assertIsStable2(FPnl);

            // CollapsiblePanelHoster
            FPnl.HostedControlKind = THostedControlKind.hckCollapsiblePanelHoster;
            Assert.AreEqual(null, FPnl.CollapsiblePanelHosterInstance); // Lazy initialisation!
            Assert.AreNotEqual(null, FPnl.TaskListInstance);  // This still exists from previous operation!
            Assert.AreNotEqual(null, FPnl.UserControlInstance);  // This still exists from previous operation!

            FPnl.Toggle(); // same as FPnl.Collapse(); above
            FPnl.Toggle(); // same as FPnl.Expand(); above
            Assert.AreNotEqual(null, FPnl.CollapsiblePanelHosterInstance);  // Now initialised as Expand() got called (trough Toggle())
            Assert.AreNotEqual(null, FPnl.TaskListInstance);  // This still exists from previous operation!
            Assert.AreNotEqual(null, FPnl.UserControlInstance);  // This still exists from previous operation!

            assertIsStable2(FPnl);
        }

        ///<summary>
        /// This makes sure that the Unit Test covers all code in the Expand() Method.
        /// </summary>
        [Test]
        public void TestExpandAdditionalCodeCoverage()
        {
            // UserControl
            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, HOSTEDUSERCONTROL);
            FPnl.Expand();

            FPnl.HostedControlKind = THostedControlKind.hckTaskList;
            FPnl.TaskListNode = TTestTaskList.GetTestXmlNode();

            FPnl.Expand();

            FPnl.HostedControlKind = THostedControlKind.hckCollapsiblePanelHoster;
            FPnl.Expand();

            FPnl.HostedControlKind = THostedControlKind.hckTaskList;
            FPnl.Expand();

            FPnl.HostedControlKind = THostedControlKind.hckUserControl;
            FPnl.Expand();
        }

        /// <summary>
        /// Checks that the IsCollapsed Property is always giving the correct information.
        /// </summary>
        [Test]
        public void TestIsCollapsed()
        {
            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, HOSTEDUSERCONTROL);

            Assert.IsTrue(FPnl.IsCollapsed);

            FPnl.Toggle();
            Assert.IsFalse(FPnl.IsCollapsed);
            FPnl.Toggle();
            Assert.IsTrue(FPnl.IsCollapsed);
            FPnl.Collapse();
            Assert.IsTrue(FPnl.IsCollapsed);
            FPnl.Expand();
            Assert.IsFalse(FPnl.IsCollapsed);

            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, HOSTEDUSERCONTROL, false);
            Assert.IsFalse(FPnl.IsCollapsed);

            FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList, true);
            Assert.IsTrue(FPnl.IsCollapsed);
            FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList, false);
            Assert.IsFalse(FPnl.IsCollapsed);

            FPnl = new TPnlCollapsible(THostedControlKind.hckCollapsiblePanelHoster, true);
            Assert.IsTrue(FPnl.IsCollapsed);
            FPnl = new TPnlCollapsible(THostedControlKind.hckCollapsiblePanelHoster, false);
            Assert.IsFalse(FPnl.IsCollapsed);
        }

        /// <summary>
        /// Checks for DirStyleMismatch to be detected
        /// </summary>
        [Test]
        public void TestDirStyleMismatch()
        {
            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, HOSTEDUSERCONTROL);

            try
            {
                FPnl.AssertDirStyleMatch(TCollapseDirection.cdHorizontal, TVisualStylesEnum.vsTaskPanel);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(EVisualStyleAndDirectionMismatchException), e.GetType());
            }
        }

        /// <summary>
        /// Checks for DirStyleMismatch to be detected
        /// </summary>
        [Test]
        public void TestHckDataMismatch()
        {
            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl);

            try
            {
                FPnl.AssertHckDataMatch();
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(EInsufficientDataSetForHostedControlKindException), e.GetType());
            }
        }

        /// <summary>
        /// Tests the static StylesForDirection Method.
        /// </summary>
        [Test]
        public void TestStylesForDirection()
        {
            List <TVisualStylesEnum>AvailableStyles;

            AvailableStyles = TPnlCollapsible.StylesForDirection(TCollapseDirection.cdVertical);
            Assert.AreNotEqual(0, AvailableStyles.Count);

            AvailableStyles = TPnlCollapsible.StylesForDirection(TCollapseDirection.cdHorizontal);
            Assert.AreNotEqual(0, AvailableStyles.Count);

            AvailableStyles = TPnlCollapsible.StylesForDirection(TCollapseDirection.cdHorizontalRight);
            Assert.AreNotEqual(0, AvailableStyles.Count);
        }

        /// <summary>
        /// Checks that the ToggleDirection() Method works.
        /// </summary>
        [Test]
        public void TestToggleDirection()
        {
            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, HOSTEDUSERCONTROL, TCollapseDirection.cdVertical);
            Assert.AreEqual(TCollapseDirection.cdVertical, FPnl.CollapseDirection);

            FPnl.ToggleDirection();

            Assert.AreEqual(TCollapseDirection.cdHorizontal, FPnl.CollapseDirection);

            FPnl.ToggleDirection();

            Assert.AreEqual(TCollapseDirection.cdVertical, FPnl.CollapseDirection);
        }

        /// <summary>
        /// Checks that the TestGetHardCodedXmlNodes_ForDesignerOnly() Method works.
        /// </summary>
        [Test]
        public void TestGetHardCodedXmlNodes_ForDesignerOnly()
        {
            FPnl = new TPnlCollapsible(THostedControlKind.hckTaskList, TTestTaskList.GetTestXmlNode());

            // Need to resort to .NET Reflection as GetHardCodedXmlNodes_ForDesignerOnly() is a Private Method...
            MethodInfo InvokedPrivateMethod = FPnl.GetType().GetMethod("GetHardCodedXmlNodes_ForDesignerOnly",
                BindingFlags.NonPublic | BindingFlags.Instance);

            XmlNode Nodes = (XmlNode)InvokedPrivateMethod.Invoke(FPnl, new object[] { });
            Assert.IsNotNull(Nodes);
        }

        /// <summary>
        /// Checks that the Events that are exposed by the Control are fired correctly.
        /// </summary>
        [Test]
        public void TestEvents()
        {
            // Not just test that Events are fired, but that the RIGHT Events are fired under
            // the RIGHT circumstances, and that no wrong Event is fired under wrong circumstances!
            // (Uses EventHandlerCapture Class for that as NUnit doesn't provide Asserts for
            // something nifty like that!)
            FPnl = new TPnlCollapsible(THostedControlKind.hckUserControl, HOSTEDUSERCONTROL);
            var EhcExpanded = new TNUnitEventHandlerCheck <System.EventArgs>();
            var EhcCollapsed = new TNUnitEventHandlerCheck <System.EventArgs>();

            FPnl.Expanded += EhcExpanded.Handler;
            FPnl.Collapsed += EhcCollapsed.Handler;

            // Assert that the Expanded Event is fired when the Control is Expanded (and that the Collapsed Event isn't)
            TNUnitEventAsserter.Assert(EhcExpanded, TNUnitEventAsserter.GotRaised <System.EventArgs>(), () => FPnl.Expand());
            TNUnitEventAsserter.Assert(EhcCollapsed, TNUnitEventAsserter.DidNotGetRaised <System.EventArgs>(), () => FPnl.Expand());

            // Assert that the Collapsed Event is fired when the Control is Expanded (and that the Expanded Event isn't)
            TNUnitEventAsserter.Assert(EhcCollapsed, TNUnitEventAsserter.GotRaised <System.EventArgs>(), () => FPnl.Collapse());
            TNUnitEventAsserter.Assert(EhcExpanded, TNUnitEventAsserter.DidNotGetRaised <System.EventArgs>(), () => FPnl.Collapse());

            FPnl.Expanded -= EhcExpanded.Handler;
            FPnl.Collapsed -= EhcCollapsed.Handler;

            // Assert that the Expanded/Collapsed Events are fired when the Control's Toggle Button is clicked
            // Need to resort to .NET Reflection as BtnToggleClick() is a Private Method...
            MethodInfo InvokedPrivateMethod = FPnl.GetType().GetMethod("BtnToggleClick",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // It might look a bit weird to check Assert AreNotSame, but only this way we find out that the
            // .Invoke calls below do in fact do something!
            FPnl.Expanded += delegate(object sender, EventArgs e) {
                Assert.AreNotSame(FPnl, null);
            };
            FPnl.Expanded += delegate(object sender, EventArgs e) {
                Assert.AreSame(FPnl, sender);
            };
            FPnl.Collapsed += delegate(object sender, EventArgs e) {
                Assert.AreNotSame(FPnl, null);
            };
            FPnl.Collapsed += delegate(object sender, EventArgs e) {
                Assert.AreSame(FPnl, sender);
            };

            InvokedPrivateMethod.Invoke(FPnl, new object[] { FPnl, null });
            InvokedPrivateMethod.Invoke(FPnl, new object[] { FPnl, null });
        }
    }
}