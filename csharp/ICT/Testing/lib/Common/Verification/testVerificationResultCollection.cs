//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2017 by OM International
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
            new TLogging("../../log/test.log");
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
            Assert.AreEqual(1, coll.Count, "there should be one verification result");

            res = new TVerificationResult(null, "test", TResultSeverity.Resv_Noncritical);
            coll.Add(res);
            Assert.AreEqual(2, coll.Count, "there should be 2 verification results");

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

            Assert.AreEqual(2, coll2.Count, "there should be two verification results in the coll2 collection");
            Assert.AreEqual(0, coll.Count, "there should be no verification results in the coll collection");

            coll.AddCollection(coll2);
            Assert.AreEqual(2, coll.Count, "there should be two verification results in the coll collection after the adding of coll2");

            Assert.AreEqual("test1", coll[0].ResultText, "added result text should be test1");
            Assert.AreEqual("test2", coll[1].ResultText, "added result text should be test2");

            coll2 = new TVerificationResultCollection();
            res = new TVerificationResult(null, "test3", TResultSeverity.Resv_Noncritical);
            coll2.Add(res);
            res = new TVerificationResult(null, "test4", TResultSeverity.Resv_Noncritical);
            coll2.Add(res);
            coll.AddCollection(coll2);
            Assert.AreEqual(4, coll.Count, "there should be four verification results in the coll collection after the adding of coll2 another time");
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
            TVerificationResult res0, res1, res4, res5, res6, res7;
            TVerificationResultCollection coll = new TVerificationResultCollection();

            res0 = new TVerificationResult(null, "test0", TResultSeverity.Resv_Noncritical);
            coll.Add(res0);
            res1 = new TVerificationResult(null, "test1", TResultSeverity.Resv_Noncritical);
            coll.Add(res1);
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

            Assert.AreEqual(6, coll.Count, "should be 6 elements at the start of the test");

            // Remove(DataColumn)
            coll.Remove(col);
            Assert.AreEqual(5, coll.Count, "only one element should be removed, even if there are 2 results with column col");
            Assert.AreEqual(2, coll.IndexOf(res5), "res4 was removed");
            coll.Insert(2, res4);
            coll.Remove(new DataColumn("test"));
            Assert.AreEqual(6, coll.Count, "nothing happens when trying to remove unknown column");

            // Remove(IResultInterface value)
            coll.Remove(res1);
            Assert.AreEqual(5, coll.Count, "res1 should have been removed");
            coll.Insert(1, res1);
            Assert.Throws(typeof(ArgumentException),
                delegate { coll.Remove(new TVerificationResult(null, "test3", TResultSeverity.Resv_Info)); },
                "trying to remove unknown verification result throws ArgumentException");

            // Remove(String AResultColumnName)
            coll.Remove("nonexisting");
            Assert.AreEqual(6, coll.Count, "nothing happens when trying to remove unknown resultcolumnname");
            coll.Remove(col.ColumnName);
            Assert.AreEqual(5, coll.Count, "should have removed res4");
            Assert.AreEqual(res6, coll.FindBy(col), "first result with col should be res6");
            coll.Insert(4, res4);
        }

        /// <summary>
        /// Test the BuildVerificationResultString method
        /// </summary>
        [Test]
        public void TestBuildVerificationResultString()
        {
            TVerificationResult res0, res1, res4, res5, res6, res7;
            TVerificationResultCollection coll = new TVerificationResultCollection();

            res0 = new TVerificationResult(null, "test0", TResultSeverity.Resv_Noncritical);
            coll.Add(res0);
            res1 = new TVerificationResult(null, "test1", TResultSeverity.Resv_Info);
            coll.Add(res1);
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

            string expectedString =
                Environment.NewLine + "    Problem: test0" + Environment.NewLine + "    (Non-critical)" + Environment.NewLine + Environment.NewLine +
                Environment.NewLine + "    Status: test1" + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                Environment.NewLine + "    Problem: test4" + Environment.NewLine + "    (Non-critical)" + Environment.NewLine + Environment.NewLine +
                Environment.NewLine + "    Status: test5" + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                Environment.NewLine + "    Problem: test6" + Environment.NewLine + "    (Non-critical)" + Environment.NewLine + Environment.NewLine +
                Environment.NewLine + "    Problem: test7" + Environment.NewLine + "    (Non-critical)" + Environment.NewLine + Environment.NewLine;

            Assert.AreEqual(expectedString, coll.BuildVerificationResultString(), "comparing the string");
        }

        /// <summary>
        /// Test the BuildScreenVerificationResultList method
        /// </summary>
        [Test]
        public void TestBuildScreenVerificationResultList()
        {
            TVerificationResult res0, res1, res4, res5, res6, res7, res8;
            TVerificationResultCollection coll = new TVerificationResultCollection();

            res0 = new TVerificationResult(null, "test0", TResultSeverity.Resv_Noncritical);
            coll.Add(res0);
            res1 = new TVerificationResult(null, "test1", TResultSeverity.Resv_Info);
            coll.Add(res1);
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

            String ErrorMessages;
            Object testObject;
            coll.BuildScreenVerificationResultList(out ErrorMessages, out testObject, null, true);

            coll.BuildScreenVerificationResultList("test8", out ErrorMessages);

            Console.WriteLine(ErrorMessages);
            Console.WriteLine(ErrorMessages.Replace("\n", "\\n").Replace("\r", "\\r"));

            String expectedErrorMessages =
                "test8" + Environment.NewLine + Environment.NewLine;
            Assert.AreEqual(expectedErrorMessages, ErrorMessages, "only show errors of resultcontext test1 and of TVerificationScreenResult");
        }
    }
}
