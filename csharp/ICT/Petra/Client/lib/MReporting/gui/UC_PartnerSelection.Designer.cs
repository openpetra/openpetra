// auto generated with nant generateWinforms from UC_PartnerSelection.yaml
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

namespace Ict.Petra.Client.MReporting.Gui
{
    partial class TFrmUC_PartnerSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmUC_PartnerSelection));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grpSelection = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtPartner = new System.Windows.Forms.RadioButton();
            this.txtPartnerKey = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.rbtExtract = new System.Windows.Forms.RadioButton();
            this.txtExtract = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.rbtCurrentStaff = new System.Windows.Forms.RadioButton();
            this.dtpCurrentStaff = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.rbtAllStaff = new System.Windows.Forms.RadioButton();

            this.pnlContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpSelection.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Location = new System.Drawing.Point(2,2);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlContent.Controls.Add(this.tableLayoutPanel1);
            //
            // grpSelection
            //
            this.grpSelection.Location = new System.Drawing.Point(2,2);
            this.grpSelection.Name = "grpSelection";
            this.grpSelection.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.grpSelection.Controls.Add(this.tableLayoutPanel2);
            //
            // rbtPartner
            //
            this.rbtPartner.Location = new System.Drawing.Point(2,2);
            this.rbtPartner.Name = "rbtPartner";
            this.rbtPartner.AutoSize = true;
            this.rbtPartner.CheckedChanged += new System.EventHandler(this.rbtSelectionChange);
            this.rbtPartner.Text = "Partner";
            //
            // txtPartnerKey
            //
            this.txtPartnerKey.Location = new System.Drawing.Point(2,2);
            this.txtPartnerKey.Name = "txtPartnerKey";
            this.txtPartnerKey.Size = new System.Drawing.Size(370, 28);
            this.txtPartnerKey.ASpecialSetting = true;
            this.txtPartnerKey.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtPartnerKey.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtPartnerKey.PartnerClass = "";
            this.txtPartnerKey.MaxLength = 32767;
            this.txtPartnerKey.Tag = "CustomDisableAlthoughInvisible";
            this.txtPartnerKey.TextBoxWidth = 80;
            this.txtPartnerKey.ButtonWidth = 40;
            this.txtPartnerKey.ReadOnly = false;
            this.txtPartnerKey.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtPartnerKey.ButtonText = "Find";
            //
            // rbtExtract
            //
            this.rbtExtract.Location = new System.Drawing.Point(2,2);
            this.rbtExtract.Name = "rbtExtract";
            this.rbtExtract.AutoSize = true;
            this.rbtExtract.CheckedChanged += new System.EventHandler(this.rbtSelectionChange);
            this.rbtExtract.Text = "Extract";
            //
            // txtExtract
            //
            this.txtExtract.Location = new System.Drawing.Point(2,2);
            this.txtExtract.Name = "txtExtract";
            this.txtExtract.Size = new System.Drawing.Size(370, 28);
            this.txtExtract.ASpecialSetting = true;
            this.txtExtract.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtExtract.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.Extract;
            this.txtExtract.PartnerClass = "";
            this.txtExtract.MaxLength = 32767;
            this.txtExtract.Tag = "CustomDisableAlthoughInvisible";
            this.txtExtract.TextBoxWidth = 80;
            this.txtExtract.ButtonWidth = 40;
            this.txtExtract.ReadOnly = false;
            this.txtExtract.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtExtract.ButtonText = "Find";
            //
            // rbtCurrentStaff
            //
            this.rbtCurrentStaff.Location = new System.Drawing.Point(2,2);
            this.rbtCurrentStaff.Name = "rbtCurrentStaff";
            this.rbtCurrentStaff.AutoSize = true;
            this.rbtCurrentStaff.CheckedChanged += new System.EventHandler(this.rbtSelectionChange);
            this.rbtCurrentStaff.Text = "Staff at";
            //
            // dtpCurrentStaff
            //
            this.dtpCurrentStaff.Location = new System.Drawing.Point(2,2);
            this.dtpCurrentStaff.Name = "dtpCurrentStaff";
            this.dtpCurrentStaff.Size = new System.Drawing.Size(94, 28);
            //
            // rbtAllStaff
            //
            this.rbtAllStaff.Location = new System.Drawing.Point(2,2);
            this.rbtAllStaff.Name = "rbtAllStaff";
            this.rbtAllStaff.AutoSize = true;
            this.rbtAllStaff.CheckedChanged += new System.EventHandler(this.rbtSelectionChange);
            this.rbtAllStaff.Text = "All Staff";
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.rbtPartner, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtExtract, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.rbtCurrentStaff, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.rbtAllStaff, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtPartnerKey, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtExtract, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.dtpCurrentStaff, 1, 2);
            this.grpSelection.Text = "Selection";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.grpSelection, 0, 0);

            //
            // TFrmUC_PartnerSelection
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TFrmUC_PartnerSelection";
            this.Text = "";

            this.tableLayoutPanel2.ResumeLayout(false);
            this.grpSelection.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox grpSelection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rbtPartner;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtPartnerKey;
        private System.Windows.Forms.RadioButton rbtExtract;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtExtract;
        private System.Windows.Forms.RadioButton rbtCurrentStaff;
        private Ict.Petra.Client.CommonControls.TtxtPetraDate dtpCurrentStaff;
        private System.Windows.Forms.RadioButton rbtAllStaff;
    }
}
