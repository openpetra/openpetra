//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
//
// Copyright 2004-2010 by OM International
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
using System.Windows.Forms;
using NUnit.Extensions.Forms;
using Ict.Common.Controls;
using Ict.Testing.NUnitForms;
using SourceGrid;  

namespace Ict.Testing.NUnitForms
{
	/// <summary>
	/// Description of SourceGridSelectionTester.
	/// </summary>
    public class SourceGridSelectionTester : ControlTester
    {
        private SourceGrid.Selection.RowSelection TheObject;

        /// constructor
        public SourceGridSelectionTester(string name)
        {
            Finder <SourceGrid.Selection.RowSelection>finder = 
            	new Finder <SourceGrid.Selection.RowSelection>(name);
            TheObject = finder.Find();
        }

        /// constructor
        public SourceGridSelectionTester()
        {
            Finder <SourceGrid.Selection.RowSelection>finder = 
            	new Finder <SourceGrid.Selection.RowSelection>();
            TheObject = finder.Find();
        }

        
    }
}
