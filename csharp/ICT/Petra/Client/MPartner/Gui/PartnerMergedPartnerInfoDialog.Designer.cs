//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2016 by OM International
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
namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TPartnerMergedPartnerInfoDialog
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
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TPartnerMergedPartnerInfoDialog));
            this.flpMainFlow = new System.Windows.Forms.Panel();
            this.lblHeading = new System.Windows.Forms.Label();
            this.lblHeading2 = new System.Windows.Forms.Label();
            this.txtMergedPartner = new Ict.Common.Controls.TTxtPartnerKeyTextBox();
            this.flpMainFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlMergedIntoPartner = new System.Windows.Forms.Panel();
            this.tlpMergedIntoPartner = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMergedIntoPartner = new Ict.Common.Controls.TTxtPartnerKeyTextBox();
            this.lblMergedIntoPartnerKeyName = new System.Windows.Forms.Label();
            this.txtMergedIntoPartnerClass = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMergedBy = new System.Windows.Forms.TextBox();
            this.txtMergeDate = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblMergedIntoPartnerInfo = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.pnlBtnOKCancelHelpLayout = new System.Windows.Forms.Panel();
            this.flpMainFlow.SuspendLayout();
            this.flpMainFlowLayout.SuspendLayout();
            this.pnlMergedIntoPartner.SuspendLayout();
            this.tlpMergedIntoPartner.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 196);
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(470, 32);
            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 423);
            this.stbMain.Name = "stbMain";
            this.stbMain.Size = new System.Drawing.Size(738, 22);
            this.stbMain.TabIndex = 997;
            this.stbMain.UseOpenPetraToolStripRenderer = false;
            //
            // btnOK
            //
            this.btnOK.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(597, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(63, 22);
            this.btnOK.TabIndex = 997;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            this.stbMain.SetHelpText(this.btnOK, "Accept the Merged-Into Partner and continue.");
            //
            // btnCancel
            //
            this.btnCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(665, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 22);
            this.btnCancel.TabIndex = 998;
            this.btnCancel.Text = "&Cancel";
            this.stbMain.SetHelpText(this.btnCancel, "Stop the operation you were carring out.");
            //
            // btnHelp
            //
            this.btnHelp.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.Location = new System.Drawing.Point(9, 4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(26, 22);
            this.btnHelp.TabIndex = 999;
            this.btnHelp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
            this.stbMain.SetHelpText(this.btnHelp, "Help");
            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Controls.Add(this.btnHelp);
            this.pnlBtnOKCancelHelpLayout.Controls.Add(this.btnOK);
            this.pnlBtnOKCancelHelpLayout.Controls.Add(this.btnCancel);
            this.pnlBtnOKCancelHelpLayout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 393);
            this.pnlBtnOKCancelHelpLayout.Name = "pnlBtnOKCancelHelpLayout";
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(738, 30);
            this.pnlBtnOKCancelHelpLayout.TabIndex = 996;
            //
            // flpMainFlow
            //
            this.flpMainFlow.Controls.Add(this.lblHeading);
            this.flpMainFlow.Controls.Add(this.lblHeading2);
            this.flpMainFlow.Controls.Add(this.txtMergedPartner);
            this.flpMainFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMainFlow.Location = new System.Drawing.Point(0, 0);
            this.flpMainFlow.Margin = new System.Windows.Forms.Padding(0);
            this.flpMainFlow.Name = "flpMainFlow";
            this.flpMainFlow.Size = new System.Drawing.Size(467, 75);
            this.flpMainFlow.TabIndex = 1005;
            //
            // lblHeading
            //
            this.lblHeading.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading.Location = new System.Drawing.Point(10, 8);
            this.lblHeading.Name = "lblHeading";
            this.lblHeading.Size = new System.Drawing.Size(385, 21);
            this.lblHeading.TabIndex = 1003;
            this.lblHeading.Text = "The Partner that you have selected,";
            //
            // lblHeading2
            //
            this.lblHeading2.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeading2.Location = new System.Drawing.Point(10, 44);
            this.lblHeading2.Name = "lblHeading2";
            this.lblHeading2.Size = new System.Drawing.Size(600, 29);
            this.lblHeading2.TabIndex = 1002;
            this.lblHeading2.Text = "was Merged into another Partner and is therefore no longer accessible.";
            //
            // txtMergedPartner
            //
            this.txtMergedPartner.BackColor = System.Drawing.SystemColors.Control;
            this.txtMergedPartner.DelegateFallbackLabel = true;
            this.txtMergedPartner.Font = new System.Drawing.Font("Courier New", 9.25F, System.Drawing.FontStyle.Bold);
            this.txtMergedPartner.LabelText = "Merged, Partner, A   [PERSON]";
            this.txtMergedPartner.Location = new System.Drawing.Point(21, 23);
            this.txtMergedPartner.MaxLength = 10;
            this.txtMergedPartner.Name = "txtMergedPartner";
            this.txtMergedPartner.PartnerKey = ((long)(123456789));
            this.txtMergedPartner.ReadOnly = true;
            this.txtMergedPartner.ShowLabel = true;
            this.txtMergedPartner.Size = new System.Drawing.Size(650, 22);
            this.txtMergedPartner.TabIndex = 1004;
            this.txtMergedPartner.TabStop = false;
            this.txtMergedPartner.TextBoxReadOnly = true;
            //
            // flpMainFlowLayout
            //
            this.flpMainFlowLayout.Controls.Add(this.flpMainFlow);
            this.flpMainFlowLayout.Controls.Add(this.pnlMergedIntoPartner);
            this.flpMainFlowLayout.Controls.Add(this.panel2);
            this.flpMainFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMainFlowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMainFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.flpMainFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.flpMainFlowLayout.Name = "flpMainFlowLayout";
            this.flpMainFlowLayout.Size = new System.Drawing.Size(600, 196);
            this.flpMainFlowLayout.TabIndex = 1007;
            this.flpMainFlowLayout.WrapContents = false;
            //
            // pnlMergedIntoPartner
            //
            this.pnlMergedIntoPartner.Controls.Add(this.tlpMergedIntoPartner);
            this.pnlMergedIntoPartner.Controls.Add(this.lblMergedIntoPartnerInfo);
            this.pnlMergedIntoPartner.Location = new System.Drawing.Point(0, 75);
            this.pnlMergedIntoPartner.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMergedIntoPartner.Name = "pnlMergedIntoPartner";
            this.pnlMergedIntoPartner.Size = new System.Drawing.Size(650, 86);
            this.pnlMergedIntoPartner.TabIndex = 1006;
            //
            // tlpMergedIntoPartner
            //
            this.tlpMergedIntoPartner.ColumnCount = 2;
            this.tlpMergedIntoPartner.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMergedIntoPartner.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMergedIntoPartner.Controls.Add(this.label3, 0, 3);
            this.tlpMergedIntoPartner.Controls.Add(this.label1, 0, 1);
            this.tlpMergedIntoPartner.Controls.Add(this.txtMergedIntoPartner, 1, 0);
            this.tlpMergedIntoPartner.Controls.Add(this.lblMergedIntoPartnerKeyName, 0, 0);
            this.tlpMergedIntoPartner.Controls.Add(this.txtMergedIntoPartnerClass, 1, 1);
            this.tlpMergedIntoPartner.Controls.Add(this.label2, 0, 2);
            this.tlpMergedIntoPartner.Controls.Add(this.txtMergedBy, 1, 2);
            this.tlpMergedIntoPartner.Controls.Add(this.txtMergeDate, 1, 3);
            this.tlpMergedIntoPartner.Location = new System.Drawing.Point(21, 14);
            this.tlpMergedIntoPartner.Name = "tlpMergedIntoPartner";
            this.tlpMergedIntoPartner.RowCount = 4;
            this.tlpMergedIntoPartner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMergedIntoPartner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMergedIntoPartner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpMergedIntoPartner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpMergedIntoPartner.Size = new System.Drawing.Size(650, 70);
            this.tlpMergedIntoPartner.TabIndex = 998;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 1007;
            this.label3.Text = "Merge Date:";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 1004;
            this.label1.Text = "Partner Class:";
            //
            // txtMergedIntoPartner
            //
            this.txtMergedIntoPartner.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.txtMergedIntoPartner.BackColor = System.Drawing.SystemColors.Control;
            this.txtMergedIntoPartner.DelegateFallbackLabel = true;
            this.txtMergedIntoPartner.Font = new System.Drawing.Font("Courier New", 9.25F, System.Drawing.FontStyle.Bold);
            this.txtMergedIntoPartner.LabelText = "Merged-Into, Partner, A";
            this.txtMergedIntoPartner.Location = new System.Drawing.Point(138, 3);
            this.txtMergedIntoPartner.MaxLength = 10;
            this.txtMergedIntoPartner.Name = "txtMergedIntoPartner";
            this.txtMergedIntoPartner.PartnerKey = ((long)(123456789));
            this.txtMergedIntoPartner.ReadOnly = true;
            this.txtMergedIntoPartner.ShowLabel = true;
            this.txtMergedIntoPartner.Size = new System.Drawing.Size(650, 22);
            this.txtMergedIntoPartner.TabIndex = 1002;
            this.txtMergedIntoPartner.TabStop = false;
            this.txtMergedIntoPartner.TextBoxReadOnly = true;
            //
            // lblMergedIntoPartnerKeyName
            //
            this.lblMergedIntoPartnerKeyName.AutoSize = true;
            this.lblMergedIntoPartnerKeyName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMergedIntoPartnerKeyName.Location = new System.Drawing.Point(3, 7);
            this.lblMergedIntoPartnerKeyName.Name = "lblMergedIntoPartnerKeyName";
            this.lblMergedIntoPartnerKeyName.Size = new System.Drawing.Size(129, 13);
            this.lblMergedIntoPartnerKeyName.TabIndex = 1003;
            this.lblMergedIntoPartnerKeyName.Text = "Partner Key && Name:";
            //
            // txtMergedIntoPartnerClass
            //
            this.txtMergedIntoPartnerClass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMergedIntoPartnerClass.Location = new System.Drawing.Point(138, 23);
            this.txtMergedIntoPartnerClass.Name = "txtMergedIntoPartnerClass";
            this.txtMergedIntoPartnerClass.ReadOnly = true;
            this.txtMergedIntoPartnerClass.Size = new System.Drawing.Size(390, 14);
            this.txtMergedIntoPartnerClass.TabIndex = 1005;
            this.txtMergedIntoPartnerClass.TabStop = false;
            this.txtMergedIntoPartnerClass.Text = "PERSON";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 1006;
            this.label2.Text = "Merged By:";
            //
            // txtMergedBy
            //
            this.txtMergedBy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMergedBy.Location = new System.Drawing.Point(138, 43);
            this.txtMergedBy.Name = "txtMergedBy";
            this.txtMergedBy.ReadOnly = true;
            this.txtMergedBy.Size = new System.Drawing.Size(269, 14);
            this.txtMergedBy.TabIndex = 1008;
            this.txtMergedBy.TabStop = false;
            this.txtMergedBy.Text = "DUMMY";
            //
            // txtMergeDate
            //
            this.txtMergeDate.AllowEmpty = true;
            this.txtMergeDate.AllowFutureDate = true;
            this.txtMergeDate.AllowPastDate = true;
            this.txtMergeDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMergeDate.Date = new System.DateTime(2009, 2, 17, 0, 0, 0, 0);
            this.txtMergeDate.Description = "Merge Date";
            this.txtMergeDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtMergeDate.LeavingOnFailedValidationOK = true;
            this.txtMergeDate.Location = new System.Drawing.Point(138, 58);
            this.txtMergeDate.Name = "txtMergeDate";
            this.txtMergeDate.ReadOnly = true;
            this.txtMergeDate.Size = new System.Drawing.Size(269, 14);
            this.txtMergeDate.TabIndex = 1009;
            this.txtMergeDate.TabStop = false;
            this.txtMergeDate.Text = "17-FEB-2009";
            //
            // lblMergedIntoPartnerInfo
            //
            this.lblMergedIntoPartnerInfo.Location = new System.Drawing.Point(10, 0);
            this.lblMergedIntoPartnerInfo.Name = "lblMergedIntoPartnerInfo";
            this.lblMergedIntoPartnerInfo.Size = new System.Drawing.Size(447, 14);
            this.lblMergedIntoPartnerInfo.TabIndex = 1002;
            this.lblMergedIntoPartnerInfo.Text = "This is the Partner that it got merged into:";
            //
            // panel2
            //
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.lblInstructions);
            this.panel2.Location = new System.Drawing.Point(0, 161);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(426, 31);
            this.panel2.TabIndex = 1007;
            //
            // lblInstructions
            //
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Location = new System.Drawing.Point(10, 5);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(600, 26);
            this.lblInstructions.TabIndex = 1005;
            this.lblInstructions.Text = "Choose \'OK\' to accept the Merged-Into Partner, or \'Cancel\' to stop the operation " +
                                        "you were carring out.";
            //
            // TPartnerMergedPartnerInfoDialog
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(650, 238);
            this.Controls.Add(this.pnlBtnOKCancelHelpLayout);
            this.Controls.Add(this.flpMainFlowLayout);
            this.Controls.Add(this.stbMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TPartnerMergedPartnerInfoDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Merged Partner Information";
            this.Load += new System.EventHandler(this.TPartnerMergedPartnerInfoDialogLoad);
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            this.flpMainFlow.ResumeLayout(false);
            this.flpMainFlowLayout.ResumeLayout(false);
            this.flpMainFlowLayout.PerformLayout();
            this.pnlMergedIntoPartner.ResumeLayout(false);
            this.tlpMergedIntoPartner.ResumeLayout(false);
            this.tlpMergedIntoPartner.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.FlowLayoutPanel flpMainFlowLayout;
        private System.Windows.Forms.Panel flpMainFlow;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlMergedIntoPartner;
        private System.Windows.Forms.Label lblMergedIntoPartnerInfo;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate txtMergeDate;
        private System.Windows.Forms.TextBox txtMergedBy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMergedIntoPartnerClass;
        private System.Windows.Forms.Label lblMergedIntoPartnerKeyName;
        private Ict.Common.Controls.TTxtPartnerKeyTextBox txtMergedIntoPartner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private Ict.Common.Controls.TTxtPartnerKeyTextBox txtMergedPartner;
        private System.Windows.Forms.Label lblHeading2;
        private System.Windows.Forms.Label lblHeading;
        private System.Windows.Forms.TableLayoutPanel tlpMergedIntoPartner;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnlBtnOKCancelHelpLayout;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnHelp;
    }
}