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
using System;
using System.Data;
using System.Windows.Forms;
using NUnit.Extensions.Forms;
using Ict.Common.Controls;
using Ict.Testing.NUnitForms;
using SourceGrid;

namespace Ict.Testing.NUnitForms
{
    /// <summary>
    /// test for SourceGrid
    /// </summary>
    public class TSgrdDataGridPagedTester : ControlTester
    {
        private TSgrdDataGridPaged _TheObject;

        /// constructor
        public TSgrdDataGridPagedTester(string name, Form form)
        {
            Finder <TSgrdDataGridPaged>finder = new Finder <TSgrdDataGridPaged>(name, form);
            _TheObject = finder.Find();
        }

        /// constructor
        public TSgrdDataGridPagedTester(string name, string formName)
        {
            Finder <TSgrdDataGridPaged>finder = new Finder <TSgrdDataGridPaged>(name, new FormFinder().Find(formName));
            _TheObject = finder.Find();
        }

        /// constructor
        public TSgrdDataGridPagedTester(string name)
        {
            Finder <TSgrdDataGridPaged>finder = new Finder <TSgrdDataGridPaged>(name);
            _TheObject = finder.Find();
        }

        /// <summary>
        /// access the properties of the source grid
        /// </summary>
        public new TSgrdDataGridPaged Properties
        {
            get
            {
                return _TheObject;
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public override object TheObject
        {
            get
            {
                return _TheObject;
            }
        }

        /// <summary>
        /// ...
        /// </summary>
        public void SelectRow(int ARowNumber)
        {
            Properties.Selection.SelectRow(ARowNumber, true);
            System.Console.WriteLine(Properties.Selection.GetSelectionRegion().ToString());

            if (Properties.SelectedDataRowsAsDataRowView.Length == 0)
            {
                throw new System.Exception("Cannot select row " + ARowNumber + " because it is outside the available row numbers");
            }

//            FireEvent("SelectionChanged",
//                       new RangeRegionChangedEventArgs(new RangeRegion(
//                              new Position(ARowNumber, 0)),
//                              new RangeRegion(new Position(FOldSelectedRow,0))));
            FireEvent("FocusRowEntered",
                new RowEventArgs(ARowNumber));
//            FOldSelectedRow = ARowNumber;
        }
    }
}