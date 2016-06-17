//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2016 by OM International
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
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;

using NUnit.Framework;

using Ict.Common;
using Ict.Common.Verification;

namespace Ict.Common.Verification.Testing
{
    ///  This is a testing program for TVerificationResultCollection from Ict.Common.Verification.dll
    [TestFixture]
    public class TTestVerificationResultCollection
    {
        /// <summary>
        /// Test initialisation.
        /// </summary>
        [SetUp]
        public void Init()
        {
            Catalog.Init();
            new TLogging("test.log");
        }

        /// <summary>
        /// Test for counting errors and the "has" properties
        /// </summary>
        [Test]
        public void TestCountErrorsAndHasProperties()
        {
            TVerificationResult res;
            TVerificationResultCollection coll = new TVerificationResultCollection();

            Assert.AreEqual(0, coll.Count, "there should be no verification result");
            Assert.AreEqual(0, coll.CountCriticalErrors, "there should be no critical errors");
            Assert.IsFalse(coll.HasOnlyNonCriticalErrors, "HasOnlyNonCriticalErrors: false because there are no errors at all");

            res = new TVerificationResult(null, "test", TResultSeverity.Resv_Info);
            coll.Add(res);
            Assert.AreEqual(1, coll.Count, "there should be 1 verification result");
            Assert.AreEqual(0, coll.CountCriticalErrors, "there should be no critical error");

            res = new TVerificationResult(null, "test", TResultSeverity.Resv_Status);
            coll.Add(res);
            Assert.AreEqual(2, coll.Count, "there should be 2 verification results");
            Assert.AreEqual(0, coll.CountCriticalErrors, "there should be no critical error");
            Assert.IsFalse(coll.HasOnlyNonCriticalErrors, "there are info and status");

            res = new TVerificationResult(null, "test", TResultSeverity.Resv_Noncritical);
            coll.Add(res);
            Assert.AreEqual(3, coll.Count, "there should be 3 verification results");
            Assert.AreEqual(0, coll.CountCriticalErrors, "there should be no critical error");
            Assert.IsFalse(coll.HasCriticalErrors, "should not have critical errors");
            Assert.IsFalse(coll.HasOnlyNonCriticalErrors, "there are info and status and noncritical errors");

            res = new TVerificationResult(null, "test", TResultSeverity.Resv_Critical);
            coll.Add(res);
            Assert.AreEqual(4, coll.Count, "there should be 4 verification results");
            Assert.AreEqual(1, coll.CountCriticalErrors, "there should be 1 critical error");
            Assert.IsTrue(coll.HasCriticalErrors, "should have critical errors");
            Assert.IsFalse(coll.HasOnlyNonCriticalErrors, "HasOnlyNonCriticalErrors: false because there is a critical error");
            Assert.IsTrue(coll.HasCriticalOrNonCriticalErrors, "has critical and non critical errors");

            coll = new TVerificationResultCollection();
            Assert.IsFalse(coll.HasOnlyNonCriticalErrors, "HasOnlyNonCriticalErrors: false because there are no errors at all");

            res = new TVerificationResult(null, "test", TResultSeverity.Resv_Noncritical);
            coll.Add(res);
            Assert.AreEqual(0, coll.CountCriticalErrors, "there should be no critical error");
            Assert.IsFalse(coll.HasCriticalErrors, "should not have critical errors");
            Assert.IsTrue(coll.HasOnlyNonCriticalErrors, "there is only one noncritical errors");
        }

        /// <summary>
        /// Test for adding elements
        /// </summary>
        [Test]
        public void TestAddingElements()
        {
            TVerificationResult res;
            TVerificationResultCollection coll = new TVerificationResultCollection();

            Assert.AreEqual(0, coll.Count, "there should be no verification result");

            coll.AddAndIgnoreNullValue(null);
            Assert.AreEqual(0, coll.Count, "there should be no verification result and no exception thrown");

            res = new TVerificationResult(null, "test", TResultSeverity.Resv_Noncritical);
            coll.AddAndIgnoreNullValue(res);
            Assert.AreEqual(1, coll.Count, "there should one verification result");

            res = new TVerificationResult(null, "test", TResultSeverity.Resv_Noncritical);
            coll.Add(res);
            Assert.AreEqual(2, coll.Count, "there should 2 verification results");

            Exception caught = null;

            try
            {
                coll.Add(null);
            }
            catch (Exception e)
            {
                caught = e;
            }

            Assert.IsInstanceOf(typeof(ArgumentException), caught, "there should be an ArgumentException thrown");

            Assert.AreEqual(2, coll.Count, "there should be 2 verification results");
        }

        /// <summary>
        /// Test for adding a collection
        /// </summary>
        [Test]
        public void TestAddingCollection()
        {
            TVerificationResult res;
            TVerificationResultCollection coll = new TVerificationResultCollection();
            TVerificationResultCollection coll2 = new TVerificationResultCollection();

            res = new TVerificationResult(null, "test1", TResultSeverity.Resv_Noncritical);
            coll2.Add(res);
            res = new TVerificationResult(null, "test2", TResultSeverity.Resv_Noncritical);
            coll2.Add(res);

            Assert.AreEqual(2, coll2.Count, "there should two verification results in the coll2 collection");
            Assert.AreEqual(0, coll.Count, "there should no verification results in the coll collection");

            coll.AddCollection(coll2);
            Assert.AreEqual(2, coll.Count, "there should two verification results in the coll collection after the adding of coll2");

            Assert.AreEqual("test1", coll[0].ResultText, "added result text should be test1");
            Assert.AreEqual("test2", coll[1].ResultText, "added result text should be test2");

            coll2 = new TVerificationResultCollection();
            res = new TVerificationResult(null, "test3", TResultSeverity.Resv_Noncritical);
            coll2.Add(res);
            res = new TVerificationResult(null, "test4", TResultSeverity.Resv_Noncritical);
            coll2.Add(res);
            coll.AddCollection(coll2);
            Assert.AreEqual(4, coll.Count, "there should four verification results in the coll collection after the adding of coll2 another time");
            Assert.AreEqual("test3", coll[2].ResultText, "added result text should be test3");
            Assert.AreEqual("test4", coll[3].ResultText, "added result text should be test4");
        }

        /// <summary>
        /// Test the Contains method
        /// </summary>
        [Test]
        public void TestContains()
        {
            TVerificationResult res1;
            TScreenVerificationResult res2;
            TVerificationResultCollection coll = new TVerificationResultCollection();

            // Contains(IResultInterface)
            res1 = new TVerificationResult(null, "test1", TResultSeverity.Resv_Noncritical);
            Assert.IsFalse(coll.Contains(res1), "should not contain res1");
            coll.Add(res1);
            Assert.IsTrue(coll.Contains(res1), "should contain res1");

            // Contains(string AResultContext)
            res1 = new TVerificationResult("testcontext", "testresulttext", "testcaption", "testresultcode", TResultSeverity.Resv_Noncritical);
            Assert.IsFalse(coll.Contains("testcontext"), "should not contain testcontext");
            coll.Add(res1);
            Assert.IsTrue(coll.Contains("testcontext"), "should contain testcontext (by string)");

            // Contains(object AResultContext) with a control
            TextBox txtTest1 = new TextBox();
            res2 = new TScreenVerificationResult(txtTest1, null, "test1", "t1", txtTest1, TResultSeverity.Resv_Noncritical);
            Assert.IsFalse(coll.Contains(txtTest1), "should not contain txtTest1 box");
            coll.Add(res2);
            Assert.IsTrue(coll.Contains(txtTest1), "should contain txtTest1 box");

            // Contains(DataColumn)
            DataColumn col = new DataColumn("test", typeof(int));
            Assert.IsFalse(coll.Contains(col), "should not contain col");
            res2 = new TScreenVerificationResult(null, col, "test1", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res2);
            Assert.IsTrue(coll.Contains(col), "should contain col");

            // Contains(DataTable)
            DataTable tab = new DataTable("test");
            DataColumn col2 = new DataColumn("test2", typeof(string));
            tab.Columns.Add(col2);
            Assert.IsFalse(coll.Contains(tab), "should not contain tab");
            res2 = new TScreenVerificationResult(null, col2, "test1", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res2);
            Assert.IsTrue(coll.Contains(tab), "should contain tab");
        }

        /// <summary>
        /// Test the Insert and IndexOf methods
        /// </summary>
        [Test]
        public void TestInsertAndIndexOf()
        {
            TVerificationResult res0, res1, res2, res3, res4;
            TVerificationResultCollection coll = new TVerificationResultCollection();

            // insert in the middle
            res1 = new TVerificationResult(null, "test1", TResultSeverity.Resv_Noncritical);
            coll.Add(res1);
            res3 = new TVerificationResult(null, "test3", TResultSeverity.Resv_Noncritical);
            coll.Add(res3);
            Assert.AreEqual(0, coll.IndexOf(res1), "res1 should be at position 0");
            Assert.AreEqual(1, coll.IndexOf(res3), "res3 should be at position 1");
            res2 = new TVerificationResult(null, "test2", TResultSeverity.Resv_Noncritical);
            coll.Insert(1, res2);
            Assert.AreEqual(0, coll.IndexOf(res1), "res1 should be at position 0");
            Assert.AreEqual(1, coll.IndexOf(res2), "res2 should be at position 1");
            Assert.AreEqual(2, coll.IndexOf(res3), "res3 should be at position 2");

            // insert at the front
            res0 = new TVerificationResult(null, "test0", TResultSeverity.Resv_Noncritical);
            coll.Insert(0, res0);
            Assert.AreEqual(0, coll.IndexOf(res0), "res0 should be at position 0");
            Assert.AreEqual(1, coll.IndexOf(res1), "res1 should be at position 1");
            Assert.AreEqual(2, coll.IndexOf(res2), "res2 should be at position 2");
            Assert.AreEqual(3, coll.IndexOf(res3), "res3 should be at position 3");

            // insert at the back
            res4 = new TVerificationResult(null, "test4", TResultSeverity.Resv_Noncritical);
            coll.Insert(4, res4);
            Assert.AreEqual(0, coll.IndexOf(res0), "res0 should be at position 0");
            Assert.AreEqual(1, coll.IndexOf(res1), "res1 should be at position 1");
            Assert.AreEqual(2, coll.IndexOf(res2), "res2 should be at position 2");
            Assert.AreEqual(3, coll.IndexOf(res3), "res3 should be at position 3");
            Assert.AreEqual(4, coll.IndexOf(res4), "res4 should be at position 4");
        }

        /// <summary>
        /// Test the FindBy and FindByAll methods
        /// </summary>
        [Test]
        public void TestFindBy()
        {
            TVerificationResult res0, res1, res2, res3, res4, res5, res6, res7;
            TVerificationResultCollection coll = new TVerificationResultCollection();

            res0 = new TVerificationResult(null, "test0", TResultSeverity.Resv_Noncritical);
            coll.Add(res0);
            res1 = new TVerificationResult(null, "test1", TResultSeverity.Resv_Noncritical);
            coll.Add(res1);
            res2 = new TVerificationResult("stringobject2", "test2", TResultSeverity.Resv_Noncritical);
            coll.Add(res2);
            res3 = new TVerificationResult("stringobject3", "test3", TResultSeverity.Resv_Noncritical);
            coll.Add(res3);
            DataColumn col = new DataColumn("test", typeof(int));
            res4 = new TScreenVerificationResult(null, col, "test4", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res4);
            DataTable tab = new DataTable("test");
            DataColumn col2 = new DataColumn("test2", typeof(string));
            tab.Columns.Add(col2);
            DataColumn col3 = new DataColumn("test3", typeof(string));
            tab.Columns.Add(col3);
            res5 = new TScreenVerificationResult(null, col2, "test5", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res5);
            res6 = new TScreenVerificationResult(null, col, "test6", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res6);
            res7 = new TScreenVerificationResult(null, col3, "test7", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res7);

            // FindBy(index)
            Assert.Throws(typeof(ArgumentOutOfRangeException), delegate { coll.FindBy(20); }, "there is no verification result at index 20");

            for (int i = 0; i < 8; i++)
            {
                TVerificationResult v = null;

                switch (i)
                {
                    case 0: v = res0; break;

                    case 1: v = res1; break;

                    case 2: v = res2; break;

                    case 3: v = res3; break;

                    case 4: v = res4; break;

                    case 5: v = res5; break;

                    case 6: v = res6; break;

                    case 7: v = res7; break;
                }

                Assert.AreEqual(v, coll.FindBy(i), "res" + i.ToString() + " should be at index " + i.ToString());
            }

            // FindBy(object AResultContext)
            Assert.AreEqual(res2, coll.FindBy("stringobject2"), "should find res2 by resultcontext");
            Assert.AreEqual(res3, coll.FindBy("stringobject3"), "should find res3 by resultcontext");
            Assert.AreEqual(null, coll.FindBy("stringobject4"), "there is no verification result with resultcontext stringobject4");
            Assert.AreEqual(null, coll.FindBy(null), "looking for null returns null");

            // FindBy(DataColumn AResultColumn)
            Assert.AreEqual(res4, coll.FindBy(col), "should find res4 by column (first result with that column)");
            Assert.AreEqual(res5, coll.FindBy(col2), "should find res5 by column");
            Assert.AreEqual(res7, coll.FindBy(col3), "should find res7 by column");
            Assert.AreEqual(null, coll.FindBy(new DataColumn("test")), "should not find an unknown column");

            // FindAllBy(DataColumn AResultColumn)
            List <TScreenVerificationResult>result = coll.FindAllBy(col);
            Assert.AreEqual(2, result.Count, "FindAllBy Column should find 2 objects");
            Assert.AreEqual(res4, result[0], "first object should be res4");
            Assert.AreEqual(res6, result[1], "second object should be res6");

            result = coll.FindAllBy(new DataColumn("test"));
            Assert.IsNull(result, "FindAllBy returns null for unknown column");

            // FindAllBy(DataTable ADataTable)
            result = coll.FindAllBy(tab);
            Assert.AreEqual(2, result.Count, "FindAllBy Table should find 2 objects");
            Assert.AreEqual(res5, result[0], "first object should be res5");
            Assert.AreEqual(res7, result[1], "second object should be res7");

            result = coll.FindAllBy(new DataTable("test"));
            Assert.IsNull(result, "FindAllBy returns null for unknown table");
        }

        /// <summary>
        /// Test the RemoveAt method
        /// </summary>
        [Test]
        public void TestRemoveAt()
        {
            TVerificationResult res0, res1, res2, res3;
            TVerificationResultCollection coll = new TVerificationResultCollection();

            res0 = new TVerificationResult(null, "test0", TResultSeverity.Resv_Noncritical);
            coll.Add(res0);
            res1 = new TVerificationResult(null, "test1", TResultSeverity.Resv_Noncritical);
            coll.Add(res1);
            res2 = new TVerificationResult(null, "test2", TResultSeverity.Resv_Noncritical);
            coll.Add(res2);
            res3 = new TVerificationResult(null, "test3", TResultSeverity.Resv_Noncritical);
            coll.Add(res3);

            // RemoveAt(index)
            Assert.Throws(typeof(ArgumentOutOfRangeException), delegate { coll.RemoveAt(4); }, "there is no verification result at index 4");
            // remove from the middle
            coll.RemoveAt(1);
            Assert.AreEqual(res0, coll.FindBy(0), "res0 should be at position 0");
            Assert.AreEqual(res2, coll.FindBy(1), "res2 should be at position 1");
            Assert.AreEqual(res3, coll.FindBy(2), "res3 should be at position 2");
            // remove from the front
            coll.RemoveAt(0);
            Assert.AreEqual(res2, coll.FindBy(0), "res2 should be at position 0");
            Assert.AreEqual(res3, coll.FindBy(1), "res3 should be at position 1");
            // remove from the back
            coll.RemoveAt(1);
            Assert.AreEqual(res2, coll.FindBy(0), "res2 should be at position 0");
            Assert.AreEqual(1, coll.Count, "only one element should be left");
        }

        /// <summary>
        /// Test the Remove methods
        /// </summary>
        [Test]
        public void TestRemove()
        {
            TVerificationResult res0, res1, res2, res3, res4, res5, res6, res7;
            TVerificationResultCollection coll = new TVerificationResultCollection();

            res0 = new TVerificationResult(null, "test0", TResultSeverity.Resv_Noncritical);
            coll.Add(res0);
            res1 = new TVerificationResult(null, "test1", TResultSeverity.Resv_Noncritical);
            coll.Add(res1);
            TextBox tb1 = new TextBox();
            res2 = new TVerificationResult(tb1, "test2", TResultSeverity.Resv_Noncritical);
            coll.Add(res2);
            res3 = new TVerificationResult(tb1, "test3", TResultSeverity.Resv_Noncritical);
            coll.Add(res3);
            DataColumn col = new DataColumn("test", typeof(int));
            res4 = new TScreenVerificationResult(null, col, "test4", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res4);
            DataTable tab = new DataTable("test");
            DataColumn col2 = new DataColumn("test2", typeof(string));
            tab.Columns.Add(col2);
            DataColumn col3 = new DataColumn("test3", typeof(string));
            tab.Columns.Add(col3);
            res5 = new TScreenVerificationResult(null, col2, "test5", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res5);
            res6 = new TScreenVerificationResult(null, col, "test6", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res6);
            res7 = new TScreenVerificationResult(null, col3, "test7", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res7);

            Assert.AreEqual(8, coll.Count, "should be 8 elements at the start of the test");

            // Remove(DataColumn)
            coll.Remove(col);
            Assert.AreEqual(7, coll.Count, "only one element should be removed, even if there are 2 results with column col");
            Assert.AreEqual(4, coll.IndexOf(res5), "res4 was removed");
            coll.Insert(4, res4);
            coll.Remove(new DataColumn("test"));
            Assert.AreEqual(8, coll.Count, "nothing happens when trying to remove unknown column");

            // Remove(IResultInterface value)
            coll.Remove(res1);
            Assert.AreEqual(7, coll.Count, "res1 should have been removed");
            Assert.AreEqual(1, coll.IndexOf(res2), "res1 was removed");
            coll.Insert(1, res1);
            Assert.Throws(typeof(ArgumentException),
                delegate { coll.Remove(new TVerificationResult(null, "test3", TResultSeverity.Resv_Info)); },
                "trying to remove unknown verification result throws ArgumentException");

            // Remove(String AResultColumnName)
            coll.Remove("nonexisting");
            Assert.AreEqual(8, coll.Count, "nothing happens when trying to remove unknown resultcolumnname");
            coll.Remove(col.ColumnName);
            Assert.AreEqual(7, coll.Count, "should have removed res4");
            Assert.AreEqual(res6, coll.FindBy(col), "first result with col should be res6");
            coll.Insert(4, res4);

            // Remove(object AResultContext)
            coll.Remove(new TextBox());
            Assert.AreEqual(8, coll.Count, "nothing happens when trying to remove unknown object");
            coll.Remove(tb1);
            Assert.AreEqual(6, coll.Count, "should have removed res2 and res3");
            Assert.AreEqual(null, coll.FindBy(tb1), "there should not be any result with tb1");
        }

        /// <summary>
        /// Test the DowngradeScreenVerificationResults method
        /// </summary>
        [Test]
        public void TestDowngradeScreenVerificationResults()
        {
            TScreenVerificationResult res0, res1;
            TVerificationResultCollection coll = new TVerificationResultCollection();

            DataColumn col = new DataColumn("test", typeof(int));

            res0 = new TScreenVerificationResult(null, col, "test0", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res0);

            DataColumn col2 = new DataColumn("test2", typeof(int));
            TextBox txtField = new TextBox();
            TVerificationResult resVR = new TVerificationResult(null, "test", TResultSeverity.Resv_Critical);
            res1 = new TScreenVerificationResult(resVR, col2, txtField);
            coll.Add(res1);

            Assert.AreEqual(2, coll.Count, "there should be two results");

            foreach (object o in coll)
            {
                Assert.IsInstanceOf(typeof(TScreenVerificationResult), o, "should be TScreenVerificationResult");
            }

            TVerificationResultCollection.DowngradeScreenVerificationResults(coll);

            Assert.AreEqual(2, coll.Count, "there should be two results after downgrade");

            foreach (object o in coll)
            {
                Assert.IsInstanceOf(typeof(TVerificationResult), o, "should be TVerificationResult");
                Assert.IsNotInstanceOf(typeof(TScreenVerificationResult), o, "should not be TScreenVerificationResult");
            }
        }

        /// <summary>
        /// Test the BuildVerificationResultString method
        /// </summary>
        [Test]
        public void TestBuildVerificationResultString()
        {
            TVerificationResult res0, res1, res2, res3, res4, res5, res6, res7;
            TVerificationResultCollection coll = new TVerificationResultCollection();

            res0 = new TVerificationResult(null, "test0", TResultSeverity.Resv_Noncritical);
            coll.Add(res0);
            res1 = new TVerificationResult(null, "test1", TResultSeverity.Resv_Info);
            coll.Add(res1);
            TextBox tb1 = new TextBox();
            res2 = new TVerificationResult(tb1, "test2", TResultSeverity.Resv_Critical);
            coll.Add(res2);
            res3 = new TVerificationResult(tb1, "test3", TResultSeverity.Resv_Noncritical);
            coll.Add(res3);
            DataColumn col = new DataColumn("test", typeof(int));
            res4 = new TScreenVerificationResult(null, col, "test4", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res4);
            DataTable tab = new DataTable("test");
            DataColumn col2 = new DataColumn("test2", typeof(string));
            tab.Columns.Add(col2);
            DataColumn col3 = new DataColumn("test3", typeof(string));
            tab.Columns.Add(col3);
            res5 = new TScreenVerificationResult(null, col2, "test5", null, TResultSeverity.Resv_Status);
            coll.Add(res5);
            res6 = new TScreenVerificationResult(null, col, "test6", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res6);
            res7 = new TScreenVerificationResult(null, col3, "test7", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res7);

            Console.WriteLine(coll.BuildVerificationResultString());
            Console.WriteLine(coll.BuildVerificationResultString().Replace("\n", "\\n").Replace("\r", "\\r"));

            const string expectedString =
                "\r\n    Problem: test0\r\n    (Non-critical)\r\n\r\n" +
                "\r\n    Status: test1\r\n\r\n\r\nSystem.Windows.Forms.TextBox, Text: " +
                "\r\n    Problem: test2\r\n    (Critical)\r\n\r\nSystem.Windows.Forms.TextBox, Text: " +
                "\r\n    Problem: test3\r\n    (Non-critical)\r\n\r\n" +
                "\r\n    Problem: test4\r\n    (Non-critical)\r\n\r\n" +
                "\r\n    Status: test5\r\n\r\n\r\n" +
                "\r\n    Problem: test6\r\n    (Non-critical)\r\n\r\n" +
                "\r\n    Problem: test7\r\n    (Non-critical)\r\n\r\n";

            Assert.AreEqual(expectedString, coll.BuildVerificationResultString(), "comparing the string");
        }

        /// <summary>
        /// Test the BuildScreenVerificationResultList method
        /// </summary>
        [Test]
        public void TestBuildScreenVerificationResultList()
        {
            TVerificationResult res0, res1, res2, res3, res4, res5, res6, res7, res8, res9, res10;
            TVerificationResultCollection coll = new TVerificationResultCollection();

            res0 = new TVerificationResult(null, "test0", TResultSeverity.Resv_Noncritical);
            coll.Add(res0);
            res1 = new TVerificationResult(null, "test1", TResultSeverity.Resv_Info);
            coll.Add(res1);
            TextBox tb1 = new TextBox();
            res2 = new TVerificationResult(tb1, "test2", TResultSeverity.Resv_Critical);
            coll.Add(res2);
            res3 = new TVerificationResult(tb1, "test3", TResultSeverity.Resv_Noncritical);
            coll.Add(res3);
            DataColumn col = new DataColumn("test", typeof(int));
            res4 = new TScreenVerificationResult(null, col, "test4", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res4);
            DataTable tab = new DataTable("test");
            DataColumn col2 = new DataColumn("test2", typeof(string));
            tab.Columns.Add(col2);
            DataColumn col3 = new DataColumn("test3", typeof(string));
            tab.Columns.Add(col3);
            res5 = new TScreenVerificationResult(null, col2, "test5", null, TResultSeverity.Resv_Status);
            coll.Add(res5);
            res6 = new TScreenVerificationResult(null, col, "test6", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res6);
            res7 = new TScreenVerificationResult(null, col3, "test7", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res7);
            res8 = new TScreenVerificationResult("test8", col3, "test8", null, TResultSeverity.Resv_Noncritical);
            coll.Add(res8);

            string ErrorMessages;
            Control FirstControl;
            coll.BuildScreenVerificationResultList(null, out ErrorMessages, out FirstControl, true);

            Console.WriteLine(ErrorMessages);
            Console.WriteLine(ErrorMessages.Replace("\n", "\\n").Replace("\r", "\\r"));

            string expectedErrorMessages =
                "test4\r\n\r\ntest5\r\n\r\ntest6\r\n\r\ntest7\r\n\r\n";
            Assert.AreEqual(expectedErrorMessages, ErrorMessages, "only show errors of unspecified resultcontext and of TVerificationScreenResult");

            coll.BuildScreenVerificationResultList("test8", out ErrorMessages, out FirstControl, true);

            Console.WriteLine(ErrorMessages);
            Console.WriteLine(ErrorMessages.Replace("\n", "\\n").Replace("\r", "\\r"));

            expectedErrorMessages =
                "test8\r\n\r\n";
            Assert.AreEqual(expectedErrorMessages, ErrorMessages, "only show errors of resultcontext test1 and of TVerificationScreenResult");

            // test first control, but with updatefirstcontrol false
            TextBox tb2 = new TextBox();
            res9 = new TScreenVerificationResult(null, null, "test9", tb2, TResultSeverity.Resv_Critical);
            coll.Add(res9);
            TextBox tb3 = new TextBox();
            res10 = new TScreenVerificationResult(null, null, "test10", tb3, TResultSeverity.Resv_Critical);
            coll.Add(res10);
            coll.BuildScreenVerificationResultList(null, out ErrorMessages, out FirstControl, false);

            Console.WriteLine(ErrorMessages);
            Console.WriteLine(ErrorMessages.Replace("\n", "\\n").Replace("\r", "\\r"));

            expectedErrorMessages =
                "test4\r\n\r\ntest5\r\n\r\ntest6\r\n\r\ntest7\r\n\r\ntest9\r\n\r\ntest10\r\n\r\n";
            Assert.AreEqual(expectedErrorMessages, ErrorMessages, "added test9 and test10");
            Assert.AreEqual(tb2, FirstControl, "expect to return tb2 as first control");
            Assert.AreEqual(null, coll.FirstErrorControl, "expect to not select tb2 as first control");

            // test updatefirstcontrol true
            coll.BuildScreenVerificationResultList(null, out ErrorMessages, out FirstControl, true);
            Assert.AreEqual(tb2, FirstControl, "expect to return tb2 as first control");
            Assert.AreEqual(tb2, coll.FirstErrorControl, "expect to select tb2 as first control");

            // remove res9, so that first control is tb3, but call with updatefirstcontrol false
            coll.Remove(res9);
            coll.BuildScreenVerificationResultList(null, out ErrorMessages, out FirstControl, false);
            Assert.AreEqual(tb3, FirstControl, "expect to return tb3 as first control");
            Assert.AreEqual(tb2, coll.FirstErrorControl, "expect tb2 to be still selected as first control");

            // test other overload of BuildScreenVerificationResult, ignorewarnings etc
            object FirstErrorContext;

            // test ignorewarnings true
            coll.BuildScreenVerificationResultList(out ErrorMessages, out FirstControl, out FirstErrorContext, true, null, true);

            Assert.AreEqual(null, FirstErrorContext, "FirstErrorContext is null");

            Console.WriteLine(ErrorMessages);
            Console.WriteLine(ErrorMessages.Replace("\n", "\\n").Replace("\r", "\\r"));

            expectedErrorMessages = "test1\r\n\r\ntest2\r\n\r\ntest5\r\n\r\ntest10\r\n\r\n";
            Assert.AreEqual(expectedErrorMessages, ErrorMessages, "ignore warnings (ie. noncritical). Include TVerificationResults");

            // test ignorewarnings false
            coll.BuildScreenVerificationResultList(out ErrorMessages, out FirstControl, out FirstErrorContext, true, null, false);

            Console.WriteLine(ErrorMessages);
            Console.WriteLine(ErrorMessages.Replace("\n", "\\n").Replace("\r", "\\r"));

            expectedErrorMessages =
                "test0\r\n\r\ntest1\r\n\r\ntest2\r\n\r\ntest3\r\n\r\ntest4\r\n\r\ntest5\r\n\r\ntest6\r\n\r\ntest7\r\n\r\ntest8\r\n\r\ntest10\r\n\r\n";
            Assert.AreEqual(expectedErrorMessages, ErrorMessages, "do not ignore warnings (ie. noncritical). Include TVerificationResults");

            // test ARestrictToTypeWhichRaisesError, restrict to string errorcontext
            coll.BuildScreenVerificationResultList(out ErrorMessages, out FirstControl, out FirstErrorContext, true, typeof(string), false);
            Console.WriteLine(ErrorMessages);
            Console.WriteLine(ErrorMessages.Replace("\n", "\\n").Replace("\r", "\\r"));

            expectedErrorMessages =
                "test8\r\n\r\n";
            Assert.AreEqual(expectedErrorMessages, ErrorMessages, "restrict to string errorcontext");

            // test AUpdateFirstErrorControl
            coll.BuildScreenVerificationResultList(out ErrorMessages, out FirstControl, out FirstErrorContext, true, null, false);
            // it seems that res2/tb1 is ignored, because it is not a screenverificationresult
            Assert.AreEqual(tb3, FirstControl, "first control should be tb3");
            Assert.AreEqual(tb3, coll.FirstErrorControl, "first error control should be tb3");
            // now testing with res8/tb3 inserted before tb2, and not updating first error control
            coll.Insert(3, res9);
            coll.BuildScreenVerificationResultList(out ErrorMessages, out FirstControl, out FirstErrorContext, false, null, false);
            Assert.AreEqual(tb2, FirstControl, "first control should be tb2");
            Assert.AreEqual(tb3, coll.FirstErrorControl, "first error control should still be tb3");
        }
    }
}