//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
//
using System;
using Ict.Common;
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.MCommon.Gui.Setup
{
    public partial class TFrmSetupCurrency
    {
        /// <summary>
        /// I'm using my own code here because that provided by the auto-generated code 
        /// causes a NULL value DB error.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            if (ValidateAllData(true, true))
            {
                ACurrencyRow NewRow = FMainDS.ACurrency.NewRowTyped();
                NewRow.CurrencyCode = "";
                NewRow.CurrencyName = Catalog.GetString("New Currency");
                NewRow.CurrencySymbol = "";
                NewRow.CountryCode = "99";
                NewRow.DisplayFormat = "->>>,>>>,>>>,>>9.99";
                FMainDS.ACurrency.Rows.Add(NewRow);

                FPetraUtilsObject.SetChangedFlag();

                grdDetails.DataSource = null;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ACurrency.DefaultView);

                SelectDetailRowByDataTableIndex(FMainDS.ACurrency.Rows.Count - 1);
                txtDetailCurrencyCode.Focus();
            }
        }

        private void DeleteRow(System.Object sender, EventArgs e)
        {
            DeleteACurrency();
        }
    }
}