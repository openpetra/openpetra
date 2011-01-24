// auto generated with nant generateWinforms from UC_ConferenceSelection.yaml
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
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MReporting.Gui.MConference
{
    partial class TFrmUC_ConferenceSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmUC_ConferenceSelection));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.grpSelectConference = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtConference = new System.Windows.Forms.RadioButton();
            this.txtConference = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.rbtAllConferences = new System.Windows.Forms.RadioButton();
            this.grpSelectAttendees = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbtAllAttendees = new System.Windows.Forms.RadioButton();
            this.rbtExtract = new System.Windows.Forms.RadioButton();
            this.txtExtract = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.rbtOneAttendee = new System.Windows.Forms.RadioButton();
            this.txtOneAttendee = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();

            this.pnlContent.SuspendLayout();
            this.grpSelectConference.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpSelectAttendees.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.grpSelectAttendees);
            this.pnlContent.Controls.Add(this.grpSelectConference);
            //
            // grpSelectConference
            //
            this.grpSelectConference.Name = "grpSelectConference";
            this.grpSelectConference.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpSelectConference.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.grpSelectConference.Controls.Add(this.tableLayoutPanel1);
            //
            // rbtConference
            //
            this.rbtConference.Location = new System.Drawing.Point(2,2);
            this.rbtConference.Name = "rbtConference";
            this.rbtConference.Size = new System.Drawing.Size(150, 28);
            this.rbtConference.CheckedChanged += new System.EventHandler(this.rbtConferenceSelectionChange);
            this.rbtConference.Text = "Conference";
            //
            // txtConference
            //
            this.txtConference.Location = new System.Drawing.Point(2,2);
            this.txtConference.Name = "txtConference";
            this.txtConference.Size = new System.Drawing.Size(400, 28);
            this.txtConference.ASpecialSetting = true;
            this.txtConference.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtConference.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.Conference;
            this.txtConference.PartnerClass = "";
            this.txtConference.MaxLength = 32767;
            this.txtConference.Tag = "CustomDisableAlthoughInvisible";
            this.txtConference.TextBoxWidth = 80;
            this.txtConference.ButtonWidth = 40;
            this.txtConference.ReadOnly = false;
            this.txtConference.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtConference.ButtonText = "Find";
            //
            // rbtAllConferences
            //
            this.rbtAllConferences.Location = new System.Drawing.Point(2,2);
            this.rbtAllConferences.Name = "rbtAllConferences";
            this.rbtAllConferences.AutoSize = true;
            this.rbtAllConferences.CheckedChanged += new System.EventHandler(this.rbtConferenceSelectionChange);
            this.rbtAllConferences.Text = "All Conferences";
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.rbtConference, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rbtAllConferences, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtConference, 1, 0);
            this.grpSelectConference.Text = "Select Conference";
            //
            // grpSelectAttendees
            //
            this.grpSelectAttendees.Name = "grpSelectAttendees";
            this.grpSelectAttendees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSelectAttendees.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.grpSelectAttendees.Controls.Add(this.tableLayoutPanel2);
            //
            // rbtAllAttendees
            //
            this.rbtAllAttendees.Location = new System.Drawing.Point(2,2);
            this.rbtAllAttendees.Name = "rbtAllAttendees";
            this.rbtAllAttendees.Size = new System.Drawing.Size(150, 28);
            this.rbtAllAttendees.CheckedChanged += new System.EventHandler(this.rbtAttendeeSelectionChange);
            this.rbtAllAttendees.Text = "All Attendees";
            //
            // rbtExtract
            //
            this.rbtExtract.Location = new System.Drawing.Point(2,2);
            this.rbtExtract.Name = "rbtExtract";
            this.rbtExtract.AutoSize = true;
            this.rbtExtract.CheckedChanged += new System.EventHandler(this.rbtAttendeeSelectionChange);
            this.rbtExtract.Text = "From Extract";
            //
            // txtExtract
            //
            this.txtExtract.Location = new System.Drawing.Point(2,2);
            this.txtExtract.Name = "txtExtract";
            this.txtExtract.Size = new System.Drawing.Size(400, 28);
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
            // rbtOneAttendee
            //
            this.rbtOneAttendee.Location = new System.Drawing.Point(2,2);
            this.rbtOneAttendee.Name = "rbtOneAttendee";
            this.rbtOneAttendee.AutoSize = true;
            this.rbtOneAttendee.CheckedChanged += new System.EventHandler(this.rbtAttendeeSelectionChange);
            this.rbtOneAttendee.Text = "One Attendee";
            //
            // txtOneAttendee
            //
            this.txtOneAttendee.Location = new System.Drawing.Point(2,2);
            this.txtOneAttendee.Name = "txtOneAttendee";
            this.txtOneAttendee.Size = new System.Drawing.Size(370, 28);
            this.txtOneAttendee.ASpecialSetting = true;
            this.txtOneAttendee.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtOneAttendee.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtOneAttendee.PartnerClass = "";
            this.txtOneAttendee.MaxLength = 32767;
            this.txtOneAttendee.Tag = "CustomDisableAlthoughInvisible";
            this.txtOneAttendee.TextBoxWidth = 80;
            this.txtOneAttendee.ButtonWidth = 40;
            this.txtOneAttendee.ReadOnly = false;
            this.txtOneAttendee.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtOneAttendee.ButtonText = "Find";
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.rbtAllAttendees, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbtExtract, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.rbtOneAttendee, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtExtract, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtOneAttendee, 1, 2);
            this.grpSelectAttendees.Text = "SelectAttendees";

            //
            // TFrmUC_ConferenceSelection
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TFrmUC_ConferenceSelection";
            this.Text = "";

            this.tableLayoutPanel2.ResumeLayout(false);
            this.grpSelectAttendees.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.grpSelectConference.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.GroupBox grpSelectConference;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton rbtConference;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtConference;
        private System.Windows.Forms.RadioButton rbtAllConferences;
        private System.Windows.Forms.GroupBox grpSelectAttendees;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton rbtAllAttendees;
        private System.Windows.Forms.RadioButton rbtExtract;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtExtract;
        private System.Windows.Forms.RadioButton rbtOneAttendee;
        private Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel txtOneAttendee;
    }
}
