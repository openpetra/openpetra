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
    public partial class TFrmLinkPartnerCostCentreDialog
    {
        private Int32 FLedgerNumber;
        private DataTable FPartnerCostCentreTbl;
        private DataTable FLocalCostCentres;
        private DataView FLinkedView;
        private DataView FUnlinkedView;

        /// <summary>
        /// Setup the Partner - CostCentre links of this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                FPartnerCostCentreTbl = TRemote.MFinance.Setup.WebConnectors.LoadCostCentrePartnerLinks(FLedgerNumber);
                FLocalCostCentres = TRemote.MFinance.Setup.WebConnectors.LoadLocalCostCentres(FLedgerNumber);
            }
        }


        private void RunOnceOnActivationManual()
        {
            cmbReportsTo.Items.Clear();

            foreach (DataRow Row in FLocalCostCentres.Rows)
            {
                if (Convert.ToBoolean(Row["Posting"]) == false)
                {
                    cmbReportsTo.Items.Add(Row["CostCentreCode"]);
                }
            }

            FLinkedView = new DataView(FPartnerCostCentreTbl);
            FLinkedView.RowFilter = "IsLinked <> '0'";
            FLinkedView.AllowNew = false;

            FUnlinkedView = new DataView(FPartnerCostCentreTbl);
            FUnlinkedView.RowFilter = "IsLinked = '0'";
            FUnlinkedView.AllowNew = false;

            grdLinkedCCs.DataSource = new DevAge.ComponentModel.BoundDataView(FLinkedView);
            grdUnlinkedCCs.DataSource = new DevAge.ComponentModel.BoundDataView(FUnlinkedView);

            grdLinkedCCs.Columns.Clear();
            grdLinkedCCs.AddTextColumn("Partner Name", FPartnerCostCentreTbl.Columns["ShortName"], 240);
            grdLinkedCCs.AddTextColumn("Partner Key", FPartnerCostCentreTbl.Columns["PartnerKey"], 90);
            grdLinkedCCs.AddTextColumn("Cost Centre", FPartnerCostCentreTbl.Columns["IsLinked"], 90);
            grdLinkedCCs.AddTextColumn("Reports To", FPartnerCostCentreTbl.Columns["ReportsTo"], 90);
            grdLinkedCCs.MouseClick += new MouseEventHandler(grdLinkedCCs_Click);

            grdUnlinkedCCs.Columns.Clear();
            grdUnlinkedCCs.AddTextColumn("Partner Name", FPartnerCostCentreTbl.Columns["ShortName"], 240);
            grdUnlinkedCCs.AddTextColumn("Partner Key", FPartnerCostCentreTbl.Columns["PartnerKey"], 90);
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
        public void SaveChanges()
        {
            TRemote.MFinance.Setup.WebConnectors.SaveCostCentrePartnerLinks(
                FLedgerNumber, FPartnerCostCentreTbl);            
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FileSave(object sender, EventArgs e)
        {
            SaveChanges();
        }

        /// <summary></summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkCostCentre(object sender, EventArgs e)
        {
            String NewCCCode = txtCostCentre.Text;

            //
            // I can link to this Cost Centre, IF it's not already linked to someone else!
            FPartnerCostCentreTbl.DefaultView.Sort = ("IsLinked");

            if (FPartnerCostCentreTbl.DefaultView.Find(NewCCCode) >= 0)
            {
                MessageBox.Show(String.Format(Catalog.GetString("Error - {0} has already been assigned to a partner."), NewCCCode),
                    Catalog.GetString("Link Cost Centre"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            String ReportsTo = cmbReportsTo.Text;

            if (ReportsTo == "")
            {
                MessageBox.Show(String.Format(Catalog.GetString("Error - Select a Cost Centre that {0} will report to."), NewCCCode),
                    Catalog.GetString("Link Cost Centre"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow Row = ((DataRowView)grdUnlinkedCCs.SelectedDataRows[0]).Row;
            Row["IsLinked"] = NewCCCode;
            Row["ReportsTo"] = ReportsTo;
            txtCostCentre.ReadOnly = true;
            txtCostCentre.Text = "";
            btnLink.Enabled = false;
            grdUnlinkedCCs.SelectRowInGrid(-1, false);
        }

        /// <summary></summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnlinkCostCentre(object sender, EventArgs e)
        {
            DataRow Row = ((DataRowView)grdLinkedCCs.SelectedDataRows[0]).Row;

            Row["IsLinked"] = '0';
            txtCostCentre.Text = "";
            btnUnlink.Enabled = false;
            grdLinkedCCs.SelectRowInGrid(-1, false);
        }

        private void txtCostCentre_TextChanged(object sender, EventArgs e)
        {
            btnLink.Enabled = ((txtCostCentre.ReadOnly == false) && (txtCostCentre.Text != ""));
            FLocalCostCentres.DefaultView.RowFilter = "CostCentreCode='" + txtCostCentre.Text + "'";
            cmbReportsTo.Enabled = (FLocalCostCentres.DefaultView.Count == 0); // The name is not an existing Cost Centre.

            if (!cmbReportsTo.Enabled)
            {
                cmbReportsTo.Text = FLocalCostCentres.DefaultView[0].Row["ReportsTo"].ToString();
            }
        }

        private void grdLinkedCCs_Click(object sender, EventArgs e)
        {
            if (grdLinkedCCs.SelectedDataRows.Length > 0)
            {
                btnUnlink.Enabled = true;
                btnLink.Enabled = false;
                DataRow Row = ((DataRowView)grdLinkedCCs.SelectedDataRows[0]).Row;
                txtPartner.Text = Convert.ToString(Row["PartnerKey"]);
                txtCostCentre.ReadOnly = true;
                cmbReportsTo.Enabled = false;
                txtCostCentre.Text = Convert.ToString(Row["IsLinked"]);
                cmbReportsTo.Text = Convert.ToString(Row["ReportsTo"]);
                grdUnlinkedCCs.SelectRowInGrid(-1, false);
            }
        }

        private void grdUnlinkedCCs_Click(object sender, EventArgs e)
        {
            if (grdUnlinkedCCs.SelectedDataRows.Length > 0)
            {
                btnUnlink.Enabled = false;
                DataRow Row = ((DataRowView)grdUnlinkedCCs.SelectedDataRows[0]).Row;
                txtPartner.Text = Convert.ToString(Row["PartnerKey"]);
                txtCostCentre.ReadOnly = false;
                cmbReportsTo.Enabled = true;
                txtCostCentre.Text = "";
                grdLinkedCCs.SelectRowInGrid(-1, false);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnOK_Click(object sender, EventArgs e)
        {
            SaveChanges();
            Close();
        }
    }
}