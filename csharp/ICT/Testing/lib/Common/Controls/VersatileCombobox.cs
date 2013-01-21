//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
using System.Threading;
using System.Globalization;
using Ict.Common.Controls;

namespace Tests.Common.Controls
{
    /// TODO write your comment here
    [TestFixture]
    public class TTestVersatileCombobox
    {
        /// <summary>
        /// testing versatile combobox with string list
        /// </summary>
        [Test]
        public void TestStringCombobox()
        {
            TCmbVersatile cmb = new TCmbVersatile();

            cmb.SetDataSourceStringList("test1,test2,test3");
            Assert.AreEqual(3, cmb.Items.Count, "combobox should have 3 inital items");

            cmb.SetSelectedString("test2");
            Assert.AreEqual("test2", cmb.GetSelectedString(), "get the second string, normal");

            cmb.SetSelectedString("invalid");
            Assert.AreEqual("test2", cmb.GetSelectedString(), "get the second string, after invalid assignment");
            Assert.AreEqual(3, cmb.Items.Count, "keep the items even after invalid assignment");
        }

        /// <summary>
        /// testing versatile combobox with dataview
        /// </summary>
        [Test]
        public void TestDataViewCombobox()
        {
            TCmbVersatile cmb = new TCmbVersatile();

            DataTable t = new DataTable();

            t.Columns.Add("value", typeof(string));
            t.Columns.Add("display", typeof(string));

            DataRow r = t.NewRow();
            r[0] = "test1";
            r[1] = "Test 1";
            t.Rows.Add(r);

            r = t.NewRow();
            r[0] = "test2";
            r[1] = "Test 2";
            t.Rows.Add(r);

            r = t.NewRow();
            r[0] = "test3";
            r[1] = "Test 3";
            t.Rows.Add(r);

            cmb.DisplayMember = "display";
            cmb.ValueMember = "value";
            cmb.DataSource = t.DefaultView;
            cmb.EndUpdate();
            cmb.Invalidate();

            // need to add combobox to a form, otherwise the cmb.Items will be empty
            System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
            frm.Controls.Add(cmb);
            frm.Show();

            Assert.AreEqual(3, ((DataView)cmb.DataSource).Count, "combobox should have 3 inital items in the data source");
            Assert.AreEqual(3, cmb.Items.Count, "combobox should have 3 inital items in the items list");

            cmb.SetSelectedString("test2");
            Assert.AreEqual("test2", cmb.GetSelectedString(), "get the second string, normal");

            cmb.SetSelectedString("invalid");
            Assert.AreEqual("test2", cmb.GetSelectedString(), "get the second string, after invalid assignment");
            Assert.AreEqual(3, cmb.Items.Count, "keep the items even after invalid assignment");
        }
    }
}