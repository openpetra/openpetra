//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
//
// Copyright 2004-2014 by OM International
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

namespace Ict.Petra.Client.CommonForms
{
    /// <summary>
    /// An interface associated with a button panel beneath a grid
    /// </summary>
    public interface IButtonPanel
    {
        /// <summary>
        /// Method to update the display of the record count
        /// </summary>
        void UpdateRecordNumberDisplay();
    }

    /// <summary>
    /// A simple grid interface providing methods to get information about a selected row, or to select a specified row
    /// </summary>
    public interface IGridBase
    {
        /// <summary>
        /// Get the row index in the grid, which is 1 more than the row number in the bound data view
        /// </summary>
        Int32 GetSelectedRowIndex();

        /// <summary>
        /// Get the selected grid data as an untyped DataRow
        /// </summary>
        DataRow GetSelectedDataRow();

        /// <summary>
        /// Select the specified row index in the grid.  Can be -1.  The header row is 0.  Data starts at row 1
        /// </summary>
        /// <param name="ARowIndex">The row to select</param>
        void SelectRowInGrid(int ARowIndex);
    }
}