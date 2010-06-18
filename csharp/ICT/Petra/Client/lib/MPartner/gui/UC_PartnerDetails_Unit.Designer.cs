// auto generated with nant generateWinforms from UC_PartnerDetails_Unit.yaml
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
using System.Windows.Forms;
using Mono.Unix;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TUC_PartnerDetails_Unit
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerDetails_Unit));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.grpNames = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtPreviousName = new System.Windows.Forms.TextBox();
            this.lblPreviousName = new System.Windows.Forms.Label();
            this.txtLocalName = new System.Windows.Forms.TextBox();
            this.lblLocalName = new System.Windows.Forms.Label();
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbCountryCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblCountryCode = new System.Windows.Forms.Label();
            this.cmbUnitType = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblUnitType = new System.Windows.Forms.Label();
            this.cmbLanguageCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblLanguageCode = new System.Windows.Forms.Label();
            this.cmbAcquisitionCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblAcquisitionCode = new System.Windows.Forms.Label();
            this.grpCampaignInfo = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtXyzTbdCode = new System.Windows.Forms.TextBox();
            this.lblXyzTbdCode = new System.Windows.Forms.Label();
            this.txtXyzTbdCost = new System.Windows.Forms.TextBox();
            this.lblXyzTbdCost = new System.Windows.Forms.Label();
            this.cmbXyzTbdCostCurrencyCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();

            this.pnlContent.SuspendLayout();
            this.grpNames.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpMisc.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.grpCampaignInfo.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.grpCampaignInfo);
            this.pnlContent.Controls.Add(this.grpMisc);
            this.pnlContent.Controls.Add(this.grpNames);
            //
            // grpNames
            //
            this.grpNames.Name = "grpNames";
            this.grpNames.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpNames.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.grpNames.Controls.Add(this.tableLayoutPanel1);
            //
            // txtPreviousName
            //
            this.txtPreviousName.Location = new System.Drawing.Point(2,2);
            this.txtPreviousName.Name = "txtPreviousName";
            this.txtPreviousName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPreviousName.Size = new System.Drawing.Size(150, 28);
            //
            // lblPreviousName
            //
            this.lblPreviousName.Location = new System.Drawing.Point(2,2);
            this.lblPreviousName.Name = "lblPreviousName";
            this.lblPreviousName.AutoSize = true;
            this.lblPreviousName.Text = "Previous Name:";
            this.lblPreviousName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPreviousName.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPreviousName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtLocalName
            //
            this.txtLocalName.Location = new System.Drawing.Point(2,2);
            this.txtLocalName.Name = "txtLocalName";
            this.txtLocalName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocalName.Size = new System.Drawing.Size(150, 28);
            //
            // lblLocalName
            //
            this.lblLocalName.Location = new System.Drawing.Point(2,2);
            this.lblLocalName.Name = "lblLocalName";
            this.lblLocalName.AutoSize = true;
            this.lblLocalName.Text = "Local Name:";
            this.lblLocalName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblLocalName.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblLocalName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 120));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblPreviousName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblLocalName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPreviousName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtLocalName, 1, 1);
            this.grpNames.Text = "Names";
            //
            // grpMisc
            //
            this.grpMisc.Name = "grpMisc";
            this.grpMisc.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpMisc.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.grpMisc.Controls.Add(this.tableLayoutPanel2);
            //
            // cmbCountryCode
            //
            this.cmbCountryCode.Location = new System.Drawing.Point(2,2);
            this.cmbCountryCode.Name = "cmbCountryCode";
            this.cmbCountryCode.Size = new System.Drawing.Size(300, 28);
            this.cmbCountryCode.ListTable = TCmbAutoPopulated.TListTableEnum.CountryList;
            //
            // lblCountryCode
            //
            this.lblCountryCode.Location = new System.Drawing.Point(2,2);
            this.lblCountryCode.Name = "lblCountryCode";
            this.lblCountryCode.AutoSize = true;
            this.lblCountryCode.Text = "Country Code:";
            this.lblCountryCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCountryCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCountryCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbUnitType
            //
            this.cmbUnitType.Location = new System.Drawing.Point(2,2);
            this.cmbUnitType.Name = "cmbUnitType";
            this.cmbUnitType.Size = new System.Drawing.Size(300, 28);
            this.cmbUnitType.ListTable = TCmbAutoPopulated.TListTableEnum.UnitTypeList;
            //
            // lblUnitType
            //
            this.lblUnitType.Location = new System.Drawing.Point(2,2);
            this.lblUnitType.Name = "lblUnitType";
            this.lblUnitType.AutoSize = true;
            this.lblUnitType.Text = "Unit Type:";
            this.lblUnitType.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblUnitType.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblUnitType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbLanguageCode
            //
            this.cmbLanguageCode.Location = new System.Drawing.Point(2,2);
            this.cmbLanguageCode.Name = "cmbLanguageCode";
            this.cmbLanguageCode.Size = new System.Drawing.Size(300, 28);
            this.cmbLanguageCode.ListTable = TCmbAutoPopulated.TListTableEnum.LanguageCodeList;
            //
            // lblLanguageCode
            //
            this.lblLanguageCode.Location = new System.Drawing.Point(2,2);
            this.lblLanguageCode.Name = "lblLanguageCode";
            this.lblLanguageCode.AutoSize = true;
            this.lblLanguageCode.Text = "Language Code:";
            this.lblLanguageCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblLanguageCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblLanguageCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbAcquisitionCode
            //
            this.cmbAcquisitionCode.Location = new System.Drawing.Point(2,2);
            this.cmbAcquisitionCode.Name = "cmbAcquisitionCode";
            this.cmbAcquisitionCode.Size = new System.Drawing.Size(300, 28);
            this.cmbAcquisitionCode.ListTable = TCmbAutoPopulated.TListTableEnum.AcquisitionCodeList;
            //
            // lblAcquisitionCode
            //
            this.lblAcquisitionCode.Location = new System.Drawing.Point(2,2);
            this.lblAcquisitionCode.Name = "lblAcquisitionCode";
            this.lblAcquisitionCode.AutoSize = true;
            this.lblAcquisitionCode.Text = "Acquisition Code:";
            this.lblAcquisitionCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblAcquisitionCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblAcquisitionCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 120));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblCountryCode, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblUnitType, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblLanguageCode, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblAcquisitionCode, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.cmbCountryCode, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbUnitType, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.cmbLanguageCode, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.cmbAcquisitionCode, 1, 3);
            this.grpMisc.Text = "Miscellaneous";
            //
            // grpCampaignInfo
            //
            this.grpCampaignInfo.Name = "grpCampaignInfo";
            this.grpCampaignInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCampaignInfo.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.grpCampaignInfo.Controls.Add(this.tableLayoutPanel3);
            //
            // txtXyzTbdCode
            //
            this.txtXyzTbdCode.Location = new System.Drawing.Point(2,2);
            this.txtXyzTbdCode.Name = "txtXyzTbdCode";
            this.txtXyzTbdCode.Size = new System.Drawing.Size(150, 28);
            //
            // lblXyzTbdCode
            //
            this.lblXyzTbdCode.Location = new System.Drawing.Point(2,2);
            this.lblXyzTbdCode.Name = "lblXyzTbdCode";
            this.lblXyzTbdCode.AutoSize = true;
            this.lblXyzTbdCode.Text = "Xyz Tbd Code:";
            this.lblXyzTbdCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblXyzTbdCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblXyzTbdCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtXyzTbdCost
            //
            this.txtXyzTbdCost.Location = new System.Drawing.Point(2,2);
            this.txtXyzTbdCost.Name = "txtXyzTbdCost";
            this.txtXyzTbdCost.Size = new System.Drawing.Size(90, 28);
            //
            // lblXyzTbdCost
            //
            this.lblXyzTbdCost.Location = new System.Drawing.Point(2,2);
            this.lblXyzTbdCost.Name = "lblXyzTbdCost";
            this.lblXyzTbdCost.AutoSize = true;
            this.lblXyzTbdCost.Text = "Xyz Tbd Cost:";
            this.lblXyzTbdCost.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblXyzTbdCost.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblXyzTbdCost.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbXyzTbdCostCurrencyCode
            //
            this.cmbXyzTbdCostCurrencyCode.Location = new System.Drawing.Point(2,2);
            this.cmbXyzTbdCostCurrencyCode.Name = "cmbXyzTbdCostCurrencyCode";
            this.cmbXyzTbdCostCurrencyCode.Size = new System.Drawing.Size(60, 28);
            this.cmbXyzTbdCostCurrencyCode.ListTable = TCmbAutoPopulated.TListTableEnum.CurrencyCodeList;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 120));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 100));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 65));
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.lblXyzTbdCode, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblXyzTbdCost, 0, 1);
            this.tableLayoutPanel3.SetColumnSpan(this.txtXyzTbdCode, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtXyzTbdCode, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtXyzTbdCost, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmbXyzTbdCostCurrencyCode, 2, 1);
            this.grpCampaignInfo.Text = "Campaign Information";

            //
            // TUC_PartnerDetails_Unit
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TUC_PartnerDetails_Unit";
            this.Text = "";

	
            this.tableLayoutPanel3.ResumeLayout(false);
            this.grpCampaignInfo.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.grpMisc.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.grpNames.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.GroupBox grpNames;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtPreviousName;
        private System.Windows.Forms.Label lblPreviousName;
        private System.Windows.Forms.TextBox txtLocalName;
        private System.Windows.Forms.Label lblLocalName;
        private System.Windows.Forms.GroupBox grpMisc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbCountryCode;
        private System.Windows.Forms.Label lblCountryCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbUnitType;
        private System.Windows.Forms.Label lblUnitType;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbLanguageCode;
        private System.Windows.Forms.Label lblLanguageCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbAcquisitionCode;
        private System.Windows.Forms.Label lblAcquisitionCode;
        private System.Windows.Forms.GroupBox grpCampaignInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox txtXyzTbdCode;
        private System.Windows.Forms.Label lblXyzTbdCode;
        private System.Windows.Forms.TextBox txtXyzTbdCost;
        private System.Windows.Forms.Label lblXyzTbdCost;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbXyzTbdCostCurrencyCode;
    }
}
