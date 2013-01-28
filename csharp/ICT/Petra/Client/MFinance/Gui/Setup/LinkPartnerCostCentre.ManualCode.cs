//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Data;
using Ict.Common.Verification;
using Ict.Common;
using System.Windows.Forms;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmLinkPartnerCostCentre
    {
        private Int32 FLedgerNumber;
        private DataTable PartnerCostCentreTbl;
        private DataView LinkedView;
        private DataView UnlinkedView;

        /// <summary>
        /// Setup the Partner - CostCentre links of this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                PartnerCostCentreTbl = TRemote.MFinance.Setup.WebConnectors.LoadCostCentrePartnerLinks(FLedgerNumber);
            }
        }


        private void RunOnceOnActivationManual()
        {
            LinkedView = new DataView(PartnerCostCentreTbl);
            LinkedView.RowFilter = "IsLinked <> 0";
            LinkedView.AllowNew = false;

            UnlinkedView = new DataView(PartnerCostCentreTbl);
            UnlinkedView.RowFilter = "IsLinked = 0";
            UnlinkedView.AllowNew = false;

            grdLinkedCCs.DataSource = new DevAge.ComponentModel.BoundDataView(LinkedView);
            grdUnlinkedCCs.DataSource = new DevAge.ComponentModel.BoundDataView(UnlinkedView);

            grdLinkedCCs.Columns.Clear();
            grdLinkedCCs.AddTextColumn("Partner Name", PartnerCostCentreTbl.Columns["ShortName"], 240);
            grdLinkedCCs.AddTextColumn("Partner Key", PartnerCostCentreTbl.Columns["PartnerKey"], 90);
            grdLinkedCCs.AddTextColumn("Cost Centre", PartnerCostCentreTbl.Columns["IsLinked"], 90);
            grdLinkedCCs.MouseClick += new MouseEventHandler(grdLinkedCCs_Click);

            grdUnlinkedCCs.Columns.Clear();
            grdUnlinkedCCs.AddTextColumn("Partner Name", PartnerCostCentreTbl.Columns["ShortName"], 240);
            grdUnlinkedCCs.AddTextColumn("Partner Key", PartnerCostCentreTbl.Columns["PartnerKey"], 90);
            grdUnlinkedCCs.MouseClick += new MouseEventHandler(grdUnlinkedCCs_Click);

            btnLink.Text = "\u25B2 Link";
            btnLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnLink.Enabled = false;

            btnUnlink.Text = "\u25BC Unlink";
            btnUnlink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnUnlink.Enabled = false;

            txtCostCentre.TextChanged += new EventHandler(txtCostCentre_TextChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            TVerificationResultCollection VerificationResult;
            TSubmitChangesResult SaveResult = TRemote.MFinance.Setup.WebConnectors.SaveCostCentrePartnerLinks(
                FLedgerNumber, PartnerCostCentreTbl, out VerificationResult);
            return (SaveResult == TSubmitChangesResult.scrOK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FileSave(object sender, EventArgs e)
        {
            try
            {
                SaveChanges();
            }
            catch (CancelSaveException) { }
        }

        /// <summary></summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkCostCentre(object sender, EventArgs e)
        {
        }

        /// <summary></summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnlinkCostCentre(object sender, EventArgs e)
        {
        }

        private void txtCostCentre_TextChanged(object sender, EventArgs e)
        {
            btnLink.Enabled = (txtCostCentre.Text != "");            
        }

       private void grdLinkedCCs_Click(object sender, EventArgs e)
        {
            btnUnlink.Enabled = true;
            btnLink.Enabled = false;
            DataRow Row = ((DataRowView)grdLinkedCCs.SelectedDataRows[0]).Row;
            txtPartnerKey.Text = Convert.ToString(Row["PartnerKey"]);
            txtCostCentre.Text = Convert.ToString(Row["IsLinked"]);
            txtCostCentre.ReadOnly = true;
        }

        private void grdUnlinkedCCs_Click(object sender, EventArgs e)
        {
            btnUnlink.Enabled = false;
            DataRow Row = ((DataRowView)grdUnlinkedCCs.SelectedDataRows[0]).Row;
            txtPartnerKey.Text = Convert.ToString(Row["PartnerKey"]);
            txtCostCentre.Text = "";
            txtCostCentre.ReadOnly = false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
    
}