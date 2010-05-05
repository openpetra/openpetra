/* auto generated with nant generateWinforms from UC_PartnerDetails_Person2.yaml
 *
 * DO NOT edit manually, DO NOT edit with the designer
 * use a user control if you need to modify the screen content
 *
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Windows.Forms;
using Mono.Unix;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TUC_PartnerDetails_Person2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerDetails_Person2));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.grpNames = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtPreferredName = new System.Windows.Forms.TextBox();
            this.lblPreferredName = new System.Windows.Forms.Label();
            this.txtPreviousName = new System.Windows.Forms.TextBox();
            this.lblPreviousName = new System.Windows.Forms.Label();
            this.txtLocalName = new System.Windows.Forms.TextBox();
            this.lblLocalName = new System.Windows.Forms.Label();
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dtpDateOfBirth = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblDateOfBirth = new System.Windows.Forms.Label();
            this.txtDecorations = new System.Windows.Forms.TextBox();
            this.lblDecorations = new System.Windows.Forms.Label();
            this.cmbMaritalStatus = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblMaritalStatus = new System.Windows.Forms.Label();
            this.txtAcademicTitle = new System.Windows.Forms.TextBox();
            this.lblAcademicTitle = new System.Windows.Forms.Label();
            this.dtpMaritalStatusSince = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.lblMaritalStatusSince = new System.Windows.Forms.Label();
            this.txtMaritalStatusComment = new System.Windows.Forms.TextBox();
            this.lblMaritalStatusComment = new System.Windows.Forms.Label();
            this.cmbLanguageCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblLanguageCode = new System.Windows.Forms.Label();
            this.cmbAcquisitionCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblAcquisitionCode = new System.Windows.Forms.Label();
            this.txtOccupationCode = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.lblOccupationCode = new System.Windows.Forms.Label();

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
            // txtPreferredName
            //
            this.txtPreferredName.Location = new System.Drawing.Point(2,2);
            this.txtPreferredName.Name = "txtPreferredName";
            this.txtPreferredName.Size = new System.Drawing.Size(240, 28);
            //
            // lblPreferredName
            //
            this.lblPreferredName.Location = new System.Drawing.Point(2,2);
            this.lblPreferredName.Name = "lblPreferredName";
            this.lblPreferredName.AutoSize = true;
            this.lblPreferredName.Text = "Preferred Name:";
            this.lblPreferredName.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblPreferredName.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPreferredName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtPreviousName
            //
            this.txtPreviousName.Name = "txtPreviousName";
            this.txtPreviousName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtPreviousName.AutoSize = true;
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
            this.txtLocalName.Name = "txtLocalName";
            this.txtLocalName.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtLocalName.AutoSize = true;
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
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 140));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 40));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 40));
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, 50));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, 40));
            this.tableLayoutPanel1.Controls.Add(this.lblPreferredName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblLocalName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPreferredName, 1, 0);
            this.tableLayoutPanel1.SetColumnSpan(this.txtLocalName, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtLocalName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblPreviousName, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtPreviousName, 3, 0);
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
            // dtpDateOfBirth
            //
            this.dtpDateOfBirth.Location = new System.Drawing.Point(2,2);
            this.dtpDateOfBirth.Name = "dtpDateOfBirth";
            this.dtpDateOfBirth.Size = new System.Drawing.Size(94, 28);
            //
            // lblDateOfBirth
            //
            this.lblDateOfBirth.Location = new System.Drawing.Point(2,2);
            this.lblDateOfBirth.Name = "lblDateOfBirth";
            this.lblDateOfBirth.AutoSize = true;
            this.lblDateOfBirth.Text = "Date Of Birth:";
            this.lblDateOfBirth.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDateOfBirth.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDateOfBirth.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtDecorations
            //
            this.txtDecorations.Name = "txtDecorations";
            this.txtDecorations.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtDecorations.AutoSize = true;
            //
            // lblDecorations
            //
            this.lblDecorations.Location = new System.Drawing.Point(2,2);
            this.lblDecorations.Name = "lblDecorations";
            this.lblDecorations.AutoSize = true;
            this.lblDecorations.Text = "Decorations:";
            this.lblDecorations.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblDecorations.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDecorations.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // cmbMaritalStatus
            //
            this.cmbMaritalStatus.Location = new System.Drawing.Point(2,2);
            this.cmbMaritalStatus.Name = "cmbMaritalStatus";
            this.cmbMaritalStatus.Size = new System.Drawing.Size(300, 28);
            this.cmbMaritalStatus.ListTable = TCmbAutoPopulated.TListTableEnum.MaritalStatusList;
            //
            // lblMaritalStatus
            //
            this.lblMaritalStatus.Location = new System.Drawing.Point(2,2);
            this.lblMaritalStatus.Name = "lblMaritalStatus";
            this.lblMaritalStatus.AutoSize = true;
            this.lblMaritalStatus.Text = "Marital Status:";
            this.lblMaritalStatus.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblMaritalStatus.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblMaritalStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtAcademicTitle
            //
            this.txtAcademicTitle.Name = "txtAcademicTitle";
            this.txtAcademicTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtAcademicTitle.AutoSize = true;
            //
            // lblAcademicTitle
            //
            this.lblAcademicTitle.Location = new System.Drawing.Point(2,2);
            this.lblAcademicTitle.Name = "lblAcademicTitle";
            this.lblAcademicTitle.AutoSize = true;
            this.lblAcademicTitle.Text = "Academic Title:";
            this.lblAcademicTitle.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblAcademicTitle.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblAcademicTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // dtpMaritalStatusSince
            //
            this.dtpMaritalStatusSince.Location = new System.Drawing.Point(2,2);
            this.dtpMaritalStatusSince.Name = "dtpMaritalStatusSince";
            this.dtpMaritalStatusSince.Size = new System.Drawing.Size(94, 28);
            //
            // lblMaritalStatusSince
            //
            this.lblMaritalStatusSince.Location = new System.Drawing.Point(2,2);
            this.lblMaritalStatusSince.Name = "lblMaritalStatusSince";
            this.lblMaritalStatusSince.AutoSize = true;
            this.lblMaritalStatusSince.Text = "Marital Status Since:";
            this.lblMaritalStatusSince.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblMaritalStatusSince.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblMaritalStatusSince.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtMaritalStatusComment
            //
            this.txtMaritalStatusComment.Name = "txtMaritalStatusComment";
            this.txtMaritalStatusComment.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtMaritalStatusComment.Size = new System.Drawing.Size(150, 46);
            this.txtMaritalStatusComment.Multiline = true;
            this.txtMaritalStatusComment.ScrollBars = ScrollBars.Vertical;
            //
            // lblMaritalStatusComment
            //
            this.lblMaritalStatusComment.Location = new System.Drawing.Point(2,2);
            this.lblMaritalStatusComment.Name = "lblMaritalStatusComment";
            this.lblMaritalStatusComment.AutoSize = true;
            this.lblMaritalStatusComment.Text = "Marital Status Comment:";
            this.lblMaritalStatusComment.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblMaritalStatusComment.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblMaritalStatusComment.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            // txtOccupationCode
            //
            this.txtOccupationCode.Location = new System.Drawing.Point(2,2);
            this.txtOccupationCode.Name = "txtOccupationCode";
            this.txtOccupationCode.Size = new System.Drawing.Size(150, 28);
            this.txtOccupationCode.ASpecialSetting = true;
            this.txtOccupationCode.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtOccupationCode.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.OccupationList;
            this.txtOccupationCode.PartnerClass = "";
            this.txtOccupationCode.MaxLength = 32767;
            this.txtOccupationCode.Tag = "CustomDisableAlthoughInvisible";
            this.txtOccupationCode.TextBoxWidth = 80;
            this.txtOccupationCode.ButtonWidth = 40;
            this.txtOccupationCode.ReadOnly = false;
            this.txtOccupationCode.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtOccupationCode.ButtonText = "Find";
            //
            // lblOccupationCode
            //
            this.lblOccupationCode.Location = new System.Drawing.Point(2,2);
            this.lblOccupationCode.Name = "lblOccupationCode";
            this.lblOccupationCode.AutoSize = true;
            this.lblOccupationCode.Text = "Occupation:";
            this.lblOccupationCode.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblOccupationCode.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblOccupationCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, 140));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 40));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, 40));
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblDateOfBirth, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblMaritalStatus, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblMaritalStatusSince, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblMaritalStatusComment, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblLanguageCode, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblAcquisitionCode, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.lblOccupationCode, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.dtpDateOfBirth, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbMaritalStatus, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.dtpMaritalStatusSince, 1, 2);
            this.tableLayoutPanel2.SetColumnSpan(this.txtMaritalStatusComment, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtMaritalStatusComment, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.cmbLanguageCode, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.cmbAcquisitionCode, 1, 5);
            this.tableLayoutPanel2.SetColumnSpan(this.txtOccupationCode, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtOccupationCode, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.lblDecorations, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblAcademicTitle, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtDecorations, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtAcademicTitle, 3, 1);
            this.grpMisc.Text = "Miscellaneous";

            //
            // TUC_PartnerDetails_Person2
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TUC_PartnerDetails_Person2";
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
        private System.Windows.Forms.TextBox txtPreferredName;
        private System.Windows.Forms.Label lblPreferredName;
        private System.Windows.Forms.TextBox txtPreviousName;
        private System.Windows.Forms.Label lblPreviousName;
        private System.Windows.Forms.TextBox txtLocalName;
        private System.Windows.Forms.Label lblLocalName;
        private System.Windows.Forms.GroupBox grpMisc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpDateOfBirth;
        private System.Windows.Forms.Label lblDateOfBirth;
        private System.Windows.Forms.TextBox txtDecorations;
        private System.Windows.Forms.Label lblDecorations;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbMaritalStatus;
        private System.Windows.Forms.Label lblMaritalStatus;
        private System.Windows.Forms.TextBox txtAcademicTitle;
        private System.Windows.Forms.Label lblAcademicTitle;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpMaritalStatusSince;
        private System.Windows.Forms.Label lblMaritalStatusSince;
        private System.Windows.Forms.TextBox txtMaritalStatusComment;
        private System.Windows.Forms.Label lblMaritalStatusComment;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbLanguageCode;
        private System.Windows.Forms.Label lblLanguageCode;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbAcquisitionCode;
        private System.Windows.Forms.Label lblAcquisitionCode;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtOccupationCode;
        private System.Windows.Forms.Label lblOccupationCode;
    }
}
