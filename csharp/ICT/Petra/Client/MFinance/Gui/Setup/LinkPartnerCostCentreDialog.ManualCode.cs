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
using System.Data;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;

using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MFinance.Logic;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmLinkPartnerCostCentreDialog
    {
        private Int32 FLedgerNumber;
        private DataTable FPartnerCostCentreTbl;
        private DataTable FLocalCostCentres;
        private DataView FLinkedView;
        private DataView FUnlinkedView;

        const int CC_CODE_ONLY = 1;
        const int REPORTS_TO_ONLY = 2;

        /// <summary>
        /// Setup the Partner - CostCentre links of this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;
                //0 last argument means for all partners, otherwise supply a specific partner key
            }
        }

        private void RunOnceOnActivationManual()
        {
            TRemote.MFinance.Setup.WebConnectors.LoadCostCentrePartnerLinks(FLedgerNumber, 0, out FPartnerCostCentreTbl);
            FLocalCostCentres = TRemote.MFinance.Setup.WebConnectors.LoadLocalCostCentres(FLedgerNumber);

            //Setup Cost Centre combo
            TFinanceControls.InitialiseLocalCostCentreList(ref cmbCostCentre, FLedgerNumber, false, FLocalCostCentres);
            cmbCostCentre.Width = 300;
            cmbCostCentre.AttachedLabel.Width = 150;

            //Setup Reports To combo
            TFinanceControls.InitialiseLocalCostCentreList(ref cmbReportsTo, FLedgerNumber, true, FLocalCostCentres);
            cmbReportsTo.Width = 300;
            cmbReportsTo.AttachedLabel.Width = 150;

            lblInvisible.Visible = false;

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
            grdLinkedCCs.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(grdLinkedCCs_Click);

            grdUnlinkedCCs.Columns.Clear();
            grdUnlinkedCCs.AddTextColumn("Partner Name", FPartnerCostCentreTbl.Columns["ShortName"], 240);
            grdUnlinkedCCs.AddTextColumn("Partner Key", FPartnerCostCentreTbl.Columns["PartnerKey"], 90);
            grdUnlinkedCCs.Enter += new EventHandler(grdUnlinkedCCs_Enter);
            grdUnlinkedCCs.Selection.FocusRowEntered += new SourceGrid.RowEventHandler(grdUnlinkedCCs_Click);

            btnLink.Text = "\u25B2 Link";
            btnLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnLink.Enabled = false;

            btnUnlink.Text = "\u25BC Unlink";
            btnUnlink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnUnlink.Enabled = false;
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
            string NewCCCode = cmbCostCentre.GetSelectedString();

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

            string ReportsTo = cmbReportsTo.Text;

            if (ReportsTo == string.Empty)
            {
                MessageBox.Show(String.Format(Catalog.GetString("Error - Select a Cost Centre that {0} will report to."), NewCCCode),
                    Catalog.GetString("Link Cost Centre"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataRow Row = ((DataRowView)grdUnlinkedCCs.SelectedDataRows[0]).Row;
            Row["IsLinked"] = NewCCCode;
            Row["ReportsTo"] = ReportsTo;

            cmbCostCentre.Enabled = false;
            ClearCombos();

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

            ClearCombos();

            btnUnlink.Enabled = false;
            grdLinkedCCs.SelectRowInGrid(-1, false);
        }

        private void ClearCombos(int AWhichCombos = 0)
        {
            if (AWhichCombos != REPORTS_TO_ONLY)
            {
                cmbCostCentre.SelectedIndex = -1;
                cmbCostCentre.Text = string.Empty;
                cmbCostCentre.AttachedLabel.Text = string.Empty;
            }

            if (AWhichCombos != CC_CODE_ONLY)
            {
                cmbReportsTo.SelectedIndex = -1;
                cmbReportsTo.Text = string.Empty;
                cmbReportsTo.AttachedLabel.Text = string.Empty;
            }
        }

        private void CostCentreChanged(object sender, EventArgs e)
        {
            bool ValidCostCentre = ((cmbCostCentre.Enabled == true) && (cmbCostCentre.SelectedIndex > -1));
            bool ValidReportTo = false;

            FLocalCostCentres.DefaultView.RowFilter = "CostCentreCode='" + cmbCostCentre.GetSelectedString() + "'";

            if (FLocalCostCentres.DefaultView.Count > 0)
            {
                cmbReportsTo.SetSelectedString(FLocalCostCentres.DefaultView[0].Row["ReportsTo"].ToString());
            }
            else
            {
                ClearCombos(REPORTS_TO_ONLY);
            }

            ValidReportTo = (cmbReportsTo.Text != string.Empty);

            btnLink.Enabled = ValidCostCentre && ValidReportTo;
        }

        private void grdLinkedCCs_Click(object sender, EventArgs e)
        {
            if (grdLinkedCCs.SelectedDataRows.Length > 0)
            {
                btnUnlink.Enabled = true;
                btnLink.Enabled = false;

                DataRow Row = ((DataRowView)grdLinkedCCs.SelectedDataRows[0]).Row;
                txtPartner.Text = Convert.ToString(Row["PartnerKey"]);

                cmbCostCentre.Enabled = false;
                cmbCostCentre.SetSelectedString(Convert.ToString(Row["IsLinked"]));

                cmbReportsTo.SetSelectedString(Convert.ToString(Row["ReportsTo"]));
                grdUnlinkedCCs.SelectRowInGrid(-1, false);
            }
        }

        private void grdUnlinkedCCs_Enter(object sender, EventArgs e)
        {
            cmbCostCentre.Enabled = true;
            ClearCombos();

            btnUnlink.Enabled = false;
            btnLink.Enabled = false;

            grdLinkedCCs.SelectRowInGrid(-1, false);
        }

        private void grdUnlinkedCCs_Click(object sender, EventArgs e)
        {
            if (grdUnlinkedCCs.SelectedDataRows.Length > 0)
            {
                DataRow Row = ((DataRowView)grdUnlinkedCCs.SelectedDataRows[0]).Row;
                txtPartner.Text = Convert.ToString(Row["PartnerKey"]);
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