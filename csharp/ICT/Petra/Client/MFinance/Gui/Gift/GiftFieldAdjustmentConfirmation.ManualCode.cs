//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2012 by OM International
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
using GNU.Gettext;

using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{
    public partial class TFrmGiftFieldAdjustmentConfirmation
    {
        private GiftBatchTDS FMainDS = new GiftBatchTDS();

        /// <summary>
        /// Dataset containing gift details to confirm
        /// </summary>
        public GiftBatchTDS MainDS
        {
            set
            {
                FMainDS = value;

                DataView myDataView = FMainDS.AGiftDetail.DefaultView;
                myDataView.AllowNew = false;
                grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

                lblRecordCounter.TextAlign = System.Drawing.ContentAlignment.TopRight;
                UpdateRecordNumberDisplay();

                btnOK.Select();
            }
        }

        ///<summary>
        /// Update the text in the button panel indicating details of the record count
        /// </summary>
        public void UpdateRecordNumberDisplay()
        {
            if (grdDetails.DataSource != null)
            {
                int RecordCount = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).Count;
                lblRecordCounter.Text = String.Format(
                    Catalog.GetPluralString(MCommonResourcestrings.StrSingularRecordCount, MCommonResourcestrings.StrPluralRecordCount, RecordCount, true),
                    RecordCount);
            }
        }

        private void BtnOKClick(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void BtnHelpClick(object sender, EventArgs e)
        {
            // TODO
        }
    }
}