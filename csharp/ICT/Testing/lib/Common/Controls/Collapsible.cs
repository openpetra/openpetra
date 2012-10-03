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
using System.IO;
using System.Threading;
using System.Globalization;
using System.Xml;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.IO;
using Ict.Petra.Client.MPartner.Gui;

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
                      
        ///<summary/>
        [SetUp]
        public void Setup()
        {
            new TLogging("TestCommonControls.log");
            
            this.FPnl = new TPnlCollapsible(new object[] {THostedControlKind.hckUserControl, HOSTEDUSERCONTROL});
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
            FPnl = new TPnlCollapsible(new object[] {});

            assertIsStable(FPnl, false);
            
            FPnl = new TPnlCollapsible(new object[] {});
            FPnl.UserControlNamespace = HOSTEDUSERCONTROL;
            
            assertIsStable(FPnl);
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
                Assert.AreEqual(typeof(EIncompatibleHostedControlKindException), e.GetType());
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
                Assert.AreEqual(typeof(EIncompatibleHostedControlKindException), e.GetType());
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
        public void TestConstructorMultipleSameArguments()
        {
            // when given multiple arguments of same type, simply use the latest.
            FPnl = new TPnlCollapsible("first.control.given", "System.Console.Write");
            Assert.AreEqual("System.Console", FPnl.UserControlNamespace);
            Assert.AreEqual("Write", FPnl.UserControlClass);
            Assert.AreEqual("System.Console.Write", FPnl.UserControlString);

            assertIsStable(FPnl);
        }

        #endregion

        // don't need to test each property setter function since that's implicitly tested by the cunstructor tests which call the setters.
        // Except for setting the text, which the constructor does not support.

        ///<summary/>
        [Test]
        public void TestSetterText()
        {
            String testString = "new Title";

            FPnl.Text = testString;
            Assert.AreEqual(testString, FPnl.Text);

            assertIsStable(FPnl);
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
    }
}
