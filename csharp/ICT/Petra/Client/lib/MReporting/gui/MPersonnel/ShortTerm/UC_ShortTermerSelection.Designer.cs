// auto generated with nant generateWinforms from UC_ShortTermerSelection.yaml
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
    partial class TFrmUC_ShortTermerSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmUC_ShortTermerSelection));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.grpEvent = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlEventSelection = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnEvent = new System.Windows.Forms.Button();
            this.txtEvent = new System.Windows.Forms.TextBox();
            this.rbtThisEventOnly = new System.Windows.Forms.RadioButton();
            this.rbtRelatedOptions = new System.Windows.Forms.RadioButton();
            this.rbtAllEvents = new System.Windows.Forms.RadioButton();
            this.grpParticipants = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtAllParticipants = new System.Windows.Forms.RadioButton();
            this.rbtFromExtract = new System.Windows.Forms.RadioButton();
            this.txtExtract = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.grpApplicationStatus = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.chkAccepted = new System.Windows.Forms.CheckBox();
            this.chkCancelled = new System.Windows.Forms.CheckBox();
            this.chkHold = new System.Windows.Forms.CheckBox();
            this.chkEnquiry = new System.Windows.Forms.CheckBox();
            this.chkRejected = new System.Windows.Forms.CheckBox();

            this.pnlContent.SuspendLayout();
            this.grpEvent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlEventSelection.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.grpParticipants.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.grpApplicationStatus.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.grpApplicationStatus);
            this.pnlContent.Controls.Add(this.grpParticipants);
            this.pnlContent.Controls.Add(this.grpEvent);
            //
            // grpEvent
            //
            this.grpEvent.Name = "grpEvent";
            this.grpEvent.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpEvent.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.grpEvent.Controls.Add(this.tableLayoutPanel1);
            //
            // pnlEventSelection
            //
            this.pnlEventSelection.Location = new System.Drawing.Point(2,2);
            this.pnlEventSelection.Name = "pnlEventSelection";
            this.pnlEventSelection.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlEventSelection.Controls.Add(this.tableLayoutPanel2);
            //
            // btnEvent
            //
            this.btnEvent.Location = new System.Drawing.Point(2,2);
            this.btnEvent.Name = "btnEvent";
            this.btnEvent.AutoSize = true;
            this.btnEvent.Click += new System.EventHandler(this.btnEventClicked);
            this.btnEvent.Text = "Event...";
            //
            // txtEvent
            //
            this.txtEvent.Location = new System.Drawing.Point(2,2);
            this.txtEvent.Name = "txtEvent";
            this.txtEvent.Size = new System.Drawing.Size(150, 28);
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.btnEvent, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtEvent, 1, 0);
            //
            // rbtThisEventOnly
            //
            this.rbtThisEventOnly.Location = new System.Drawing.Point(2,2);
            this.rbtThisEventOnly.Name = "rbtThisEventOnly";
            this.rbtThisEventOnly.AutoSize = true;
            this.rbtThisEventOnly.CheckedChanged += new System.EventHandler(this.rbtEventSelectionChanged);
            this.rbtThisEventOnly.Text = "Consider this event only";
            //
            // rbtRelatedOptions
            //
            this.rbtRelatedOptions.Location = new System.Drawing.Point(2,2);
            this.rbtRelatedOptions.Name = "rbtRelatedOptions";
            this.rbtRelatedOptions.AutoSize = true;
            this.rbtRelatedOptions.CheckedChanged += new System.EventHandler(this.rbtEventSelectionChanged);
            this.rbtRelatedOptions.Text = "Consider this event and related options";
            //
            // rbtAllEvents
            //
            this.rbtAllEvents.Location = new System.Drawing.Point(2,2);
            this.rbtAllEvents.Name = "rbtAllEvents";
            this.rbtAllEvents.AutoSize = true;
            this.rbtAllEvents.CheckedChanged += new System.EventHandler(this.rbtEventSelectionChanged);
            this.rbtAllEvents.Text = "Consider all events";
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.pnlEventSelection, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rbtThisEventOnly, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rbtRelatedOptions, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.rbtAllEvents, 0, 3);
            this.grpEvent.Text = "Event";
            //
            // grpParticipants
            //
            this.grpParticipants.Name = "grpParticipants";
            this.grpParticipants.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpParticipants.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.grpParticipants.Controls.Add(this.tableLayoutPanel3);
            //
            // rbtAllParticipants
            //
            this.rbtAllParticipants.Location = new System.Drawing.Point(2,2);
            this.rbtAllParticipants.Name = "rbtAllParticipants";
            this.rbtAllParticipants.AutoSize = true;
            this.rbtAllParticipants.CheckedChanged += new System.EventHandler(this.rbtParticipantsSelectionChanged);
            this.rbtAllParticipants.Text = "List all participants";
            //
            // rbtFromExtract
            //
            this.rbtFromExtract.Location = new System.Drawing.Point(2,2);
            this.rbtFromExtract.Name = "rbtFromExtract";
            this.rbtFromExtract.AutoSize = true;
            this.rbtFromExtract.CheckedChanged += new System.EventHandler(this.rbtParticipantsSelectionChanged);
            this.rbtFromExtract.Text = "List participants from Extract";
            //
            // txtExtract
            //
            this.txtExtract.Location = new System.Drawing.Point(2,2);
            this.txtExtract.Name = "txtExtract";
            this.txtExtract.Size = new System.Drawing.Size(150, 28);
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
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.rbtAllParticipants, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.rbtFromExtract, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtExtract, 0, 2);
            this.grpParticipants.Text = "Participants at Event";
            //
            // grpApplicationStatus
            //
            this.grpApplicationStatus.Name = "grpApplicationStatus";
            this.grpApplicationStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpApplicationStatus.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.grpApplicationStatus.Controls.Add(this.tableLayoutPanel4);
            //
            // chkAccepted
            //
            this.chkAccepted.Location = new System.Drawing.Point(2,2);
            this.chkAccepted.Name = "chkAccepted";
            this.chkAccepted.AutoSize = true;
            this.chkAccepted.Text = "Accepted";
            this.chkAccepted.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            //
            // chkCancelled
            //
            this.chkCancelled.Location = new System.Drawing.Point(2,2);
            this.chkCancelled.Name = "chkCancelled";
            this.chkCancelled.AutoSize = true;
            this.chkCancelled.Text = "Cancelled";
            this.chkCancelled.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            //
            // chkHold
            //
            this.chkHold.Location = new System.Drawing.Point(2,2);
            this.chkHold.Name = "chkHold";
            this.chkHold.AutoSize = true;
            this.chkHold.Text = "Hold";
            this.chkHold.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            //
            // chkEnquiry
            //
            this.chkEnquiry.Location = new System.Drawing.Point(2,2);
            this.chkEnquiry.Name = "chkEnquiry";
            this.chkEnquiry.AutoSize = true;
            this.chkEnquiry.Text = "Enquiry";
            this.chkEnquiry.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            //
            // chkRejected
            //
            this.chkRejected.Location = new System.Drawing.Point(2,2);
            this.chkRejected.Name = "chkRejected";
            this.chkRejected.AutoSize = true;
            this.chkRejected.Text = "Rejected";
            this.chkRejected.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.chkAccepted, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.chkCancelled, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.chkHold, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.chkEnquiry, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.chkRejected, 4, 0);
            this.grpApplicationStatus.Text = "Application Status";

            //
            // TFrmUC_ShortTermerSelection
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TFrmUC_ShortTermerSelection";
            this.Text = "";

            this.tableLayoutPanel4.ResumeLayout(false);
            this.grpApplicationStatus.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.grpParticipants.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlEventSelection.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.grpEvent.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.GroupBox grpEvent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlEventSelection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnEvent;
        private System.Windows.Forms.TextBox txtEvent;
        private System.Windows.Forms.RadioButton rbtThisEventOnly;
        private System.Windows.Forms.RadioButton rbtRelatedOptions;
        private System.Windows.Forms.RadioButton rbtAllEvents;
        private System.Windows.Forms.GroupBox grpParticipants;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton rbtAllParticipants;
        private System.Windows.Forms.RadioButton rbtFromExtract;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtExtract;
        private System.Windows.Forms.GroupBox grpApplicationStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.CheckBox chkAccepted;
        private System.Windows.Forms.CheckBox chkCancelled;
        private System.Windows.Forms.CheckBox chkHold;
        private System.Windows.Forms.CheckBox chkEnquiry;
        private System.Windows.Forms.CheckBox chkRejected;
    }
}
