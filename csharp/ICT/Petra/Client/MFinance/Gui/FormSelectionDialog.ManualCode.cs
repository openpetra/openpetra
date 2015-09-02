//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2015 by OM International
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
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Remoting.Client;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonControls.Logic;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui
{
    /// manual methods for the generated window
    public partial class TFrmFormSelectionDialog
    {
        private TFinanceFormCodeEnum FFormCodeFilter;
        private String FFormTypeCodeFilter;
        private String FResultFormName;
        private String FResultFormFileName;
        private AFormTable FFormTable;
        private TFormLetterFinanceInfo FResultFormLetterInfo;
        
        /// <summary>
        /// Set parameters before opening the screen
        /// </summary>
        /// <param name="AFormCode"></param>
        /// <param name="AFormTypeCode"></param>
        public void SetParameters(TFinanceFormCodeEnum AFormCode, String AFormTypeCode)
        {
            FFormCodeFilter = AFormCode;
            FFormTypeCodeFilter = AFormTypeCode;

            // retrieve form records from server
            FFormTable = TRemote.MFinance.Common.ServerLookups.WebConnectors.GetForms(FFormCodeFilter, FFormTypeCodeFilter);

            grdForms.DataSource = new DevAge.ComponentModel.BoundDataView(FFormTable.DefaultView);

        }

        /// <summary>
        /// Get parameters after screen has been closed
        /// </summary>
        /// <param name="AFormLetterInfo"></param>
        public void GetResult(out TFormLetterFinanceInfo AFormLetterInfo)
        {
            AFormLetterInfo = FResultFormLetterInfo;
        }

        private void InitializeManualCode()
        {
            FFormTable = new AFormTable();
            grdForms.AddTextColumn("Form Name", FFormTable.ColumnFormName);
            grdForms.AddTextColumn("Form Description", FFormTable.ColumnFormDescription);
            grdForms.AddTextColumn("Form Code", FFormTable.ColumnFormCode);
            grdForms.AddTextColumn("Form Type", FFormTable.ColumnFormTypeCode);
        }

        // returns selected form to caller
        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            AFormRow ResultRow = null;

            // This table has a TypeDeletable column
            DataRowView[] selectedRows = grdForms.SelectedDataRowsAsDataRowView;
            foreach (DataRowView drv in selectedRows)
            {
                ResultRow = (((AFormRow)drv.Row));

                // only consider first selected row
                break;
            }

            if (ResultRow != null)
            {
                FResultFormName = ResultRow.FormName;
                FResultFormFileName = ResultRow.FormFileName;

                FResultFormLetterInfo = null;
                if (FResultFormFileName != "")
                {
                    FResultFormLetterInfo = new TFormLetterFinanceInfo(TTemplaterAccess.GetTagList(FResultFormFileName), FResultFormFileName);
                    FResultFormLetterInfo.MinimumAmount = ResultRow.MinimumAmount;
                    FResultFormLetterInfo.SetOptionsFromFinanceForm (ResultRow.Options);
                }

            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}