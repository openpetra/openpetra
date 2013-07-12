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
using Ict.Common.Controls;
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
            CreateNewACurrency();
        }

        private void NewRowManual(ref ACurrencyRow ARow)
        {
            string newCode = Catalog.GetString("NEWCODE");
            Int32 countNewDetail = 0;

            if (FMainDS.ACurrency.Rows.Find(new object[] { newCode }) != null)
            {
                while (FMainDS.ACurrency.Rows.Find(new object[] { newCode + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newCode += countNewDetail.ToString();
            }

            ARow.CurrencyCode = newCode;
            ARow.CurrencySymbol = String.Empty;
            ARow.CountryCode = "99";
            ARow.CurrencyName = Catalog.GetString("New Currency");
            ARow.DisplayFormat = "->>>,>>>,>>>,>>9.99";
        }

        private bool PreDeleteManual(ACurrencyRow ARowToDelete, ref string ADeletionQuestion)
        {
            ADeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");
            ADeletionQuestion += String.Format("{0}{0}({1} {2}, {3} {4})",
                Environment.NewLine,
                lblDetailCurrencyCode.Text,
                txtDetailCurrencyCode.Text,
                lblDetailCurrencyName.Text,
                txtDetailCurrencyName.Text);
            return true;
        }
        
        
        #region Filter and Find Event Handling
        
        private void FindAndFilter_ArgumentCtrlValueChanged(object AUcoEventSender, TUcoFilterAndFind.TContextEventExtControlValueArgs AUcoEventArgs)
        {
            // TODO
        } 
        
        #endregion        
    }
}