//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Drawing.Printing;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TUC_PartnerFind_PersonnelCriteria_CollapsiblePart
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
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblCommitmentFieldOffice = new System.Windows.Forms.Label();
            this.pnlCommitmentFieldOffice = new System.Windows.Forms.Panel();
            this.lblCommitmentFOFromTo = new System.Windows.Forms.Label();
            this.pnlFromTo = new System.Windows.Forms.Panel();
            this.Label3 = new System.Windows.Forms.Label();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.chkFOOnlyCurrentCommitments = new System.Windows.Forms.CheckBox();
            this.btnChooseCommitmentFieldOffice = new System.Windows.Forms.Button();
            this.txtPartnerKey = new System.Windows.Forms.TextBox();
            this.pnlEventConferenceOutreach = new System.Windows.Forms.Panel();
            this.btnChooseEventConferenceOutreach = new System.Windows.Forms.Button();
            this.txtEventConferenceOutreach = new System.Windows.Forms.TextBox();
            this.lblEventConferenceOutreach = new System.Windows.Forms.Label();
            this.pnlPersonLanguage = new System.Windows.Forms.Panel();
            this.cmbPersonLanguage = new System.Windows.Forms.ComboBox();
            this.lblPersonLanguage = new System.Windows.Forms.Label();
            this.expStringLengthCheckPersonalCriteriaCollapsiblePart = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(
                this.components);
            this.pnlCommitmentFieldOffice.SuspendLayout();
            this.pnlFromTo.SuspendLayout();
            this.pnlEventConferenceOutreach.SuspendLayout();
            this.pnlPersonLanguage.SuspendLayout();
            this.SuspendLayout();

            //
            // lblCommitmentFieldOffice
            //
            this.lblCommitmentFieldOffice.Location = new System.Drawing.Point(2, 2);
            this.lblCommitmentFieldOffice.Name = "lblCommitmentFieldOffice";
            this.lblCommitmentFieldOffice.Size = new System.Drawing.Size(142, 23);
            this.lblCommitmentFieldOffice.TabIndex = 3;
            this.lblCommitmentFieldOffice.Text = "Commitm. Field &Office:";
            this.lblCommitmentFieldOffice.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlCommitmentFieldOffice
            //
            this.pnlCommitmentFieldOffice.Controls.Add(this.lblCommitmentFOFromTo);
            this.pnlCommitmentFieldOffice.Controls.Add(this.pnlFromTo);
            this.pnlCommitmentFieldOffice.Controls.Add(this.chkFOOnlyCurrentCommitments);
            this.pnlCommitmentFieldOffice.Controls.Add(this.btnChooseCommitmentFieldOffice);
            this.pnlCommitmentFieldOffice.Controls.Add(this.txtPartnerKey);
            this.pnlCommitmentFieldOffice.Controls.Add(this.lblCommitmentFieldOffice);
            this.pnlCommitmentFieldOffice.Location = new System.Drawing.Point(2, 16);
            this.pnlCommitmentFieldOffice.Name = "pnlCommitmentFieldOffice";
            this.pnlCommitmentFieldOffice.Size = new System.Drawing.Size(294, 60);
            this.pnlCommitmentFieldOffice.TabIndex = 0;
            this.pnlCommitmentFieldOffice.Tag = "BeginGroup";

            //
            // lblCommitmentFOFromTo
            //
            this.lblCommitmentFOFromTo.Location = new System.Drawing.Point(2, 42);
            this.lblCommitmentFOFromTo.Name = "lblCommitmentFOFromTo";
            this.lblCommitmentFOFromTo.Size = new System.Drawing.Size(142, 14);
            this.lblCommitmentFOFromTo.TabIndex = 7;
            this.lblCommitmentFOFromTo.Text = "From/To:";
            this.lblCommitmentFOFromTo.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlFromTo
            //
            this.pnlFromTo.Controls.Add(this.Label3);
            this.pnlFromTo.Controls.Add(this.TextBox1);
            this.pnlFromTo.Controls.Add(this.TextBox2);
            this.pnlFromTo.Location = new System.Drawing.Point(146, 38);
            this.pnlFromTo.Name = "pnlFromTo";
            this.pnlFromTo.Size = new System.Drawing.Size(150, 22);
            this.pnlFromTo.TabIndex = 10;

            //
            // Label3
            //
            this.Label3.Location = new System.Drawing.Point(70, 2);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(8, 14);
            this.Label3.TabIndex = 9;
            this.Label3.Text = "/";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            //
            // TextBox1
            //
            this.TextBox1.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.TextBox1.Location = new System.Drawing.Point(0, 0);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(68, 21);
            this.TextBox1.TabIndex = 8;
            this.TextBox1.Text = "";

            //
            // TextBox2
            //
            this.TextBox2.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.TextBox2.Location = new System.Drawing.Point(78, 0);
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Size = new System.Drawing.Size(68, 21);
            this.TextBox2.TabIndex = 9;
            this.TextBox2.Text = "";

            //
            // chkFOOnlyCurrentCommitments
            //
            this.chkFOOnlyCurrentCommitments.Checked = true;
            this.chkFOOnlyCurrentCommitments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFOOnlyCurrentCommitments.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkFOOnlyCurrentCommitments.Location = new System.Drawing.Point(146, 24);
            this.chkFOOnlyCurrentCommitments.Name = "chkFOOnlyCurrentCommitments";
            this.chkFOOnlyCurrentCommitments.Size = new System.Drawing.Size(102, 12);
            this.chkFOOnlyCurrentCommitments.TabIndex = 6;
            this.chkFOOnlyCurrentCommitments.Text = "Only Current";
            this.chkFOOnlyCurrentCommitments.CheckStateChanged += new System.EventHandler(this.ChkFOOnlyCurrentCommitments_CheckStateChanged);

            //
            // btnChooseCommitmentFieldOffice
            //
            this.btnChooseCommitmentFieldOffice.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnChooseCommitmentFieldOffice.Location = new System.Drawing.Point(234, 0);
            this.btnChooseCommitmentFieldOffice.Name = "btnChooseCommitmentFieldOffic" + 'e';
            this.btnChooseCommitmentFieldOffice.Size = new System.Drawing.Size(20, 20);
            this.btnChooseCommitmentFieldOffice.TabIndex = 5;
            this.btnChooseCommitmentFieldOffice.Text = "...";

            //
            // txtPartnerKey
            //
            this.txtPartnerKey.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtPartnerKey.Location = new System.Drawing.Point(146, 0);
            this.txtPartnerKey.Name = "txtPartnerKey";
            this.txtPartnerKey.Size = new System.Drawing.Size(86, 21);
            this.txtPartnerKey.TabIndex = 4;
            this.txtPartnerKey.Text = "";

            //
            // pnlEventConferenceOutreach
            //
            this.pnlEventConferenceOutreach.Controls.Add(this.btnChooseEventConferenceOutreach);
            this.pnlEventConferenceOutreach.Controls.Add(this.txtEventConferenceOutreach);
            this.pnlEventConferenceOutreach.Controls.Add(this.lblEventConferenceOutreach);
            this.pnlEventConferenceOutreach.Location = new System.Drawing.Point(2, 76);
            this.pnlEventConferenceOutreach.Name = "pnlEventConferenceOutreach";
            this.pnlEventConferenceOutreach.Size = new System.Drawing.Size(294, 21);
            this.pnlEventConferenceOutreach.TabIndex = 5;

            //
            // btnChooseEventConferenceOutreach
            //
            this.btnChooseEventConferenceOutreach.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnChooseEventConferenceOutreach.Location = new System.Drawing.Point(234, 0);
            this.btnChooseEventConferenceOutreach.Name = "btnChooseEventConferenceCam" + "paign";
            this.btnChooseEventConferenceOutreach.Size = new System.Drawing.Size(20, 20);
            this.btnChooseEventConferenceOutreach.TabIndex = 6;
            this.btnChooseEventConferenceOutreach.Text = "...";

            //
            // txtEventConferenceOutreach
            //
            this.txtEventConferenceOutreach.Font = new System.Drawing.Font("Verdan" + 'a',
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtEventConferenceOutreach.Location = new System.Drawing.Point(146, 0);
            this.txtEventConferenceOutreach.Name = "txtEventConferenceOutreach";
            this.txtEventConferenceOutreach.Size = new System.Drawing.Size(86, 21);
            this.txtEventConferenceOutreach.TabIndex = 1;
            this.txtEventConferenceOutreach.Text = "";

            //
            // lblEventConferenceOutreach
            //
            this.lblEventConferenceOutreach.Location = new System.Drawing.Point(2, 2);
            this.lblEventConferenceOutreach.Name = "lblEventConferenceOutreach";
            this.lblEventConferenceOutreach.Size = new System.Drawing.Size(142, 23);
            this.lblEventConferenceOutreach.TabIndex = 0;
            this.lblEventConferenceOutreach.Text = "Eve&nt/Confer./Camp.:";
            this.lblEventConferenceOutreach.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // pnlPersonLanguage
            //
            this.pnlPersonLanguage.Controls.Add(this.cmbPersonLanguage);
            this.pnlPersonLanguage.Controls.Add(this.lblPersonLanguage);
            this.pnlPersonLanguage.Location = new System.Drawing.Point(2, 98);
            this.pnlPersonLanguage.Name = "pnlPersonLanguage";
            this.pnlPersonLanguage.Size = new System.Drawing.Size(294, 21);
            this.pnlPersonLanguage.TabIndex = 6;

            //
            // cmbPersonLanguage
            //
            this.cmbPersonLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPersonLanguage.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.cmbPersonLanguage.Items.AddRange(new Object[] { "English", "German", "Spanish" });
            this.cmbPersonLanguage.Location = new System.Drawing.Point(146, 0);
            this.cmbPersonLanguage.MaxDropDownItems = 9;
            this.cmbPersonLanguage.Name = "cmbPersonLanguage";
            this.cmbPersonLanguage.Size = new System.Drawing.Size(144, 21);
            this.cmbPersonLanguage.TabIndex = 1;

            //
            // lblPersonLanguage
            //
            this.lblPersonLanguage.Location = new System.Drawing.Point(2, 2);
            this.lblPersonLanguage.Name = "lblPersonLanguage";
            this.lblPersonLanguage.Size = new System.Drawing.Size(142, 23);
            this.lblPersonLanguage.TabIndex = 0;
            this.lblPersonLanguage.Text = "Lang&uage:";
            this.lblPersonLanguage.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // TUC_PartnerFind_PersonnelCriteria_CollapsiblePart
            //
            this.Caption = "Personnel Criteria";
            this.Controls.Add(this.pnlPersonLanguage);
            this.Controls.Add(this.pnlEventConferenceOutreach);
            this.Controls.Add(this.pnlCommitmentFieldOffice);
            this.Name = "TUC_PartnerFind_PersonnelCriteria_CollapsiblePart";
            this.Size = new System.Drawing.Size(302, 126);
            this.Controls.SetChildIndex(this.pnlCommitmentFieldOffice, 0);
            this.Controls.SetChildIndex(this.pnlEventConferenceOutreach, 0);
            this.Controls.SetChildIndex(this.pnlPersonLanguage, 0);
            this.pnlCommitmentFieldOffice.ResumeLayout(false);
            this.pnlFromTo.ResumeLayout(false);
            this.pnlEventConferenceOutreach.ResumeLayout(false);
            this.pnlPersonLanguage.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblCommitmentFieldOffice;
        private System.Windows.Forms.Panel pnlCommitmentFieldOffice;
        private System.Windows.Forms.TextBox txtPartnerKey;
        private System.Windows.Forms.Button btnChooseCommitmentFieldOffice;
        private System.Windows.Forms.CheckBox chkFOOnlyCurrentCommitments;
        private System.Windows.Forms.TextBox TextBox1;
        private System.Windows.Forms.Label lblCommitmentFOFromTo;
        private System.Windows.Forms.TextBox TextBox2;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.Panel pnlEventConferenceOutreach;
        private System.Windows.Forms.TextBox txtEventConferenceOutreach;
        private System.Windows.Forms.Label lblEventConferenceOutreach;
        private System.Windows.Forms.Button btnChooseEventConferenceOutreach;
        private System.Windows.Forms.Panel pnlPersonLanguage;
        private System.Windows.Forms.ComboBox cmbPersonLanguage;
        private System.Windows.Forms.Label lblPersonLanguage;
        private System.Windows.Forms.Panel pnlFromTo;
        private TexpTextBoxStringLengthCheck expStringLengthCheckPersonalCriteriaCollapsiblePart;
    }
}