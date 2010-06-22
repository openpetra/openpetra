// auto generated with nant generateWinforms from UC_PartnerDetails_Organisation.yaml
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
    partial class TUC_PartnerDetails_Organisation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerDetails_Organisation));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.grpNames = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtPreviousName = new System.Windows.Forms.TextBox();
            this.lblPreviousName = new System.Windows.Forms.Label();
            this.txtLocalName = new System.Windows.Forms.TextBox();
            this.lblLocalName = new System.Windows.Forms.Label();
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblReligious = new System.Windows.Forms.Label();
            this.chkReligious = new System.Windows.Forms.CheckBox();
            this.chkFoundation = new System.Windows.Forms.CheckBox();
            this.cmbBusinessCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblBusinessCode = new System.Windows.Forms.Label();
            this.cmbLanguageCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblLanguageCode = new System.Windows.Forms.Label();
            this.cmbAcquisitionCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblAcquisitionCode = new System.Windows.Forms.Label();
            this.txtContactPartnerKey = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.lblContactPartnerKey = new System.Windows.Forms.Label();

            this.pnlContent.SuspendLayout();
            this.grpNames.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpMisc.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
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
            // lblReligious
            //
            this.lblReligious.Location = new System.Drawing.Point(2,2);
            this.lblReligious.Name = "lblReligious";
            this.lblReligious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReligious.AutoSize = true;
            this.lblReligious.Text = "Religious Organisation:";
            this.lblReligious.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            //
            // chkReligious
            //
            this.chkReligious.Location = new System.Drawing.Point(2,2);
            this.chkReligious.Name = "chkReligious";
            this.chkReligious.AutoSize = true;
            this.chkReligious.Text = "";
            this.chkReligious.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            //
            // chkFoundation
            //
            this.chkFoundation.Location = new System.Drawing.Point(2,2);
            this.chkFoundation.Name = "chkFoundation";
            this.chkFoundation.Size = new System.Drawing.Size(150, 28);
            this.chkFoundation.CheckedChanged += new System.EventHandler(this.ShowHideFoundationTab);
            this.chkFoundation.Text = "Foundation";
            this.chkFoundation.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            //
            // cmbBusinessCode
            //
            this.cmbBusinessCode.Location = new System.Drawing.Point(2,2);
            this.cmbBusinessCode.Name = "cmbBusinessCode";
            this.cmbBusinessCode.Size = new System.Drawing.Size(300, 28);
            this.cmbBusinessCode.ListTable = TCmbAutoPopulated.TListTableEnum.BusinessCodeList;
            //
            // lblBusinessCode
            //
            this.lblBusinessCode.Location = new System.Drawing.Point(2,2);
            this.lblBusinessCode.Name = "lblBusinessCode";
            this.lblBusinessCode.AutoSize = true;
            this.lblBusinessCode.Text = "Business Code:";
            this.lblBusinessCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblBusinessCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblBusinessCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            //
            // txtContactPartnerKey
            //
            this.txtContactPartnerKey.Location = new System.Drawing.Point(2,2);
            this.txtContactPartnerKey.Name = "txtContactPartnerKey";
            this.txtContactPartnerKey.Size = new System.Drawing.Size(370, 28);
            this.txtContactPartnerKey.ASpecialSetting = true;
            this.txtContactPartnerKey.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtContactPartnerKey.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtContactPartnerKey.PartnerClass = "";
            this.txtContactPartnerKey.MaxLength = 32767;
            this.txtContactPartnerKey.Tag = "CustomDisableAlthoughInvisible";
            this.txtContactPartnerKey.TextBoxWidth = 80;
            this.txtContactPartnerKey.ButtonWidth = 40;
            this.txtContactPartnerKey.ReadOnly = false;
            this.txtContactPartnerKey.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtContactPartnerKey.ButtonText = "Find";
            //
            // lblContactPartnerKey
            //
            this.lblContactPartnerKey.Location = new System.Drawing.Point(2,2);
            this.lblContactPartnerKey.Name = "lblContactPartnerKey";
            this.lblContactPartnerKey.AutoSize = true;
            this.lblContactPartnerKey.Text = "Contact Partner:";
            this.lblContactPartnerKey.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblContactPartnerKey.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblContactPartnerKey.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 140));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 0));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 100));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.SetColumnSpan(this.lblReligious, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblReligious, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblBusinessCode, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblLanguageCode, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblAcquisitionCode, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblContactPartnerKey, 0, 4);
            this.tableLayoutPanel2.SetColumnSpan(this.cmbBusinessCode, 2);
            this.tableLayoutPanel2.Controls.Add(this.cmbBusinessCode, 1, 1);
            this.tableLayoutPanel2.SetColumnSpan(this.cmbLanguageCode, 2);
            this.tableLayoutPanel2.Controls.Add(this.cmbLanguageCode, 1, 2);
            this.tableLayoutPanel2.SetColumnSpan(this.cmbAcquisitionCode, 2);
            this.tableLayoutPanel2.Controls.Add(this.cmbAcquisitionCode, 1, 3);
            this.tableLayoutPanel2.SetColumnSpan(this.txtContactPartnerKey, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtContactPartnerKey, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.chkReligious, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.chkFoundation, 3, 0);
            this.grpMisc.Text = "Miscellaneous";

            //
            // TUC_PartnerDetails_Organisation
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TUC_PartnerDetails_Organisation";
            this.Text = "";

	
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
        private System.Windows.Forms.Label lblReligious;
        private System.Windows.Forms.CheckBox chkReligious;
        private System.Windows.Forms.CheckBox chkFoundation;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbBusinessCode;
        private System.Windows.Forms.Label lblBusinessCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbLanguageCode;
        private System.Windows.Forms.Label lblLanguageCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbAcquisitionCode;
        private System.Windows.Forms.Label lblAcquisitionCode;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtContactPartnerKey;
        private System.Windows.Forms.Label lblContactPartnerKey;
    }
}
