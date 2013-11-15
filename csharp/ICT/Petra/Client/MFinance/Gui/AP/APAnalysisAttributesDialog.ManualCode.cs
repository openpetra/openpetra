//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
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
using System.Collections.Generic;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.AP
{
    public partial class TFrmAPAnalysisAttributesDialog
    {
        private AccountsPayableTDS FMainDS;
        private AApAnalAttribRow PrevSelectedRow = null;
        /// <summary>True if it seems the assignments have changed</summary>
        public bool DetailsChanged = false;

        private void InitializeManualCode()
        {
            FMainDS = new AccountsPayableTDS();
        }

        /// <summary>
        /// Set the context for this form
        /// </summary>
        /// <param name="AMainDS"></param>
        /// <param name="DetailRow"></param>
        public void Initialise(ref AccountsPayableTDS AMainDS, AApDocumentDetailRow DetailRow)
        {
            FMainDS = AMainDS;

            // grdDetails will display all the applicable attributes for this item.
            // The user can't add or remove any - they must provide values for each one.

            // First I'll find out whether I already have records for the required entries:
            FMainDS.AAnalysisAttribute.DefaultView.RowFilter =
                String.Format("{0}={1} AND {2}={3}", AAnalysisAttributeTable.GetAccountCodeDBName(), DetailRow.AccountCode,
                    AAnalysisAttributeTable.GetActiveDBName(), true);

            foreach (DataRowView rv in FMainDS.AAnalysisAttribute.DefaultView)
            {
                object[] RequiredRow = new object[]
                {
                    FMainDS.AApDocument[0].ApDocumentId,
                    DetailRow.DetailNumber,
                    ((AAnalysisAttributeRow)rv.Row).AnalysisTypeCode
                };

                AApAnalAttribRow FoundRow = (AApAnalAttribRow)FMainDS.AApAnalAttrib.Rows.Find(RequiredRow);

                if (FoundRow == null)
                {
                    FoundRow = FMainDS.AApAnalAttrib.NewRowTyped();
                    FoundRow.LedgerNumber = FMainDS.AApDocument[0].LedgerNumber;
                    FoundRow.ApDocumentId = FMainDS.AApDocument[0].ApDocumentId;
                    FoundRow.DetailNumber = DetailRow.DetailNumber;
                    FoundRow.AccountCode = ((AAnalysisAttributeRow)rv.Row).AccountCode;
                    FoundRow.AnalysisTypeCode = ((AAnalysisAttributeRow)rv.Row).AnalysisTypeCode;
                    FMainDS.AApAnalAttrib.Rows.Add(FoundRow);
                }
            }

            // So now I should see the required rows:
            FMainDS.AApAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5}",
                AApAnalAttribTable.GetApDocumentIdDBName(), DetailRow.ApDocumentId,
                AApAnalAttribTable.GetDetailNumberDBName(), DetailRow.DetailNumber,
                AApAnalAttribTable.GetAccountCodeDBName(), DetailRow.AccountCode
                );
            FMainDS.AApAnalAttrib.DefaultView.AllowNew = false;
            FMainDS.AApAnalAttrib.DefaultView.AllowEdit = false;

            grdDetails.Columns.Clear();
            grdDetails.AddTextColumn("Type Code", FMainDS.AApAnalAttrib.ColumnAnalysisTypeCode);
            grdDetails.AddTextColumn("Attribute", FMainDS.AApAnalAttrib.ColumnAnalysisAttributeValue);
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AApAnalAttrib.DefaultView);

            grdDetails.Refresh();
            grdDetails.Selection.SelectRow(1, true);
            FocusedRowChanged(null, null);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UnloadDetails(object sender, EventArgs e)
        {
            if (PrevSelectedRow != null)
            {
                string SelectedValue = (string)cmbDetailAttrib.SelectedItem;

                if ((SelectedValue != null) && (SelectedValue.Length > 0))
                {
                    DetailsChanged |= (PrevSelectedRow.AnalysisAttributeValue != SelectedValue);
                    PrevSelectedRow.AnalysisAttributeValue = SelectedValue;
                }

                grdDetails.Refresh();
            }
        }

        /// <summary>
        /// Load cmbDetailAttrib with the Attrib options for this type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
        {
            UnloadDetails(null, null);

            DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length < 1)
            {
                return;
            }

            AApAnalAttribRow Row = (AApAnalAttribRow)SelectedGridRow[0].Row;

            FMainDS.AFreeformAnalysis.DefaultView.RowFilter =
                String.Format("{0}='{1}' AND {2}={3}", AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(), Row.AnalysisTypeCode,
                    AFreeformAnalysisTable.GetActiveDBName(), true);

            cmbDetailAttrib.Items.Clear();

            foreach (DataRowView rv in FMainDS.AFreeformAnalysis.DefaultView)
            {
                AFreeformAnalysisRow MasterRow = (AFreeformAnalysisRow)rv.Row;
                cmbDetailAttrib.Items.Add(MasterRow.AnalysisValue);
            }

            if (Row.AnalysisAttributeValue != "") // If there's an existing value, I'll pre-select it
            {
                cmbDetailAttrib.SetSelectedString(Row.AnalysisAttributeValue);
            }

            if (cmbDetailAttrib.Items.Count == 1) // If there's only one choice, I'll make it!
            {
                Row.AnalysisAttributeValue = (string)cmbDetailAttrib.Items[0];
                grdDetails.Refresh();
            }

            PrevSelectedRow = Row;
        }
    }
}