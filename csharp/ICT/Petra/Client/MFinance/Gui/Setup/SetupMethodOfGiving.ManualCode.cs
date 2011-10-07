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

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupMethodOfGiving
    {
        private void NewRow(System.Object sender, EventArgs e)
        {
            CreateNewAMethodOfGiving();
        }

        private void NewRowManual(ref Ict.Petra.Shared.MFinance.Account.Data.AMethodOfGivingRow ARow)
        {
            string newName = Ict.Common.Catalog.GetString("NEWTYPE");
            Int32 countNewDetail = 0;

            if (FMainDS.AMethodOfGiving.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.AMethodOfGiving.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.MethodOfGivingCode = newName;
        }

        private void DeleteRow(System.Object sender, EventArgs e)
        {
            Ict.Petra.Shared.MFinance.Account.Data.AMethodOfGivingRow actualRow = GetSelectedDetailRow();

            SelectByIndex(-1);
            FMainDS.AMethodOfGiving.Rows.Remove(actualRow);
            FPreviouslySelectedDetailRow = null;
        }

        private void SelectByIndex(int rowIndex)
        {
            if (rowIndex >= grdDetails.Rows.Count)
            {
                rowIndex = grdDetails.Rows.Count - 1;
            }

            if ((rowIndex < 1) && (grdDetails.Rows.Count > 1))
            {
                rowIndex = 1;
            }

            if ((rowIndex >= 1) && (grdDetails.Rows.Count > 1))
            {
                grdDetails.Selection.SelectRow(rowIndex, true);
                FPreviouslySelectedDetailRow = GetSelectedDetailRow();
                ShowDetails(FPreviouslySelectedDetailRow);
            }
            else
            {
                FPreviouslySelectedDetailRow = null;
            }
        }
    }
}