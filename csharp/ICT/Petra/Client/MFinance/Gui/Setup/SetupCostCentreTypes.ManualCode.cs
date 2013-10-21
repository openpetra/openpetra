//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christophert
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
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupCostCentreTypes
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// Only see Cost Centre Types for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                string rowFilter = String.Format("{0}={1}",
                    ACostCentreTable.GetLedgerNumberDBName(),
                    FLedgerNumber
                    );
                FMainDS.ACostCentreTypes.DefaultView.RowFilter = rowFilter;
                FMainDS.ACostCentreTypes.DefaultView.Sort = ACostCentreTypesTable.GetCostCentreTypeDBName();

                FFilterPanelControls.SetBaseFilter(rowFilter, true);
            }
        }

        private String UniqueNewKey()
        {
            Int32 KeyIdx = 1;
            String NewKey = "New Type ";

            FMainDS.ACostCentreTypes.DefaultView.Sort = ACostCentreTypesTable.GetCostCentreTypeDBName();

            while (FMainDS.ACostCentreTypes.DefaultView.Find(NewKey + KeyIdx) >= 0)
            {
                KeyIdx++;
            }

            return NewKey + KeyIdx;
        }

        private void NewRow(System.Object sender, EventArgs e)
        {
            // This code derived from the auto-generated CreateNewACostCentreTypes(), because that doesn't cope with LedgerNumber.
            if (ValidateAllData(true, true))
            {
                ACostCentreTypesRow NewRow = FMainDS.ACostCentreTypes.NewRowTyped();
                NewRow.LedgerNumber = FLedgerNumber;
                NewRow.CostCentreType = UniqueNewKey();
                NewRow.CcDescription = "PLEASE ENTER DESCRIPTION";
                FMainDS.ACostCentreTypes.Rows.Add(NewRow);

                FPetraUtilsObject.SetChangedFlag();

                grdDetails.DataSource = null;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.ACostCentreTypes.DefaultView);

                SelectDetailRowByDataTableIndex(FMainDS.ACostCentreTypes.Rows.Count - 1);
                ValidateAllData(true, false);
                txtDetailCostCentreType.Focus();
            }
        }

        private void ShowDetailsManual(ACostCentreTypesRow ARow)
        {
        }
    }
}