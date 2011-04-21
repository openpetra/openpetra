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
using GNU.Gettext;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TUC_PartnerNotes
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
        ///
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerNotes));
            this.components = new System.ComponentModel.Container();
            this.pnlPartnerDetailsPerson = new System.Windows.Forms.Panel();
            this.btnCreatedPartner = new Ict.Common.Controls.TbtnCreated();
            this.grpNames = new System.Windows.Forms.GroupBox();
            this.pnlNotes = new System.Windows.Forms.Panel();
            this.txtPartnerComment = new System.Windows.Forms.TextBox();
            this.expStringLengthCheckNotes = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.pnlBalloonTipAnchor = new System.Windows.Forms.Panel();
            this.pnlPartnerDetailsPerson.SuspendLayout();
            this.grpNames.SuspendLayout();
            this.pnlNotes.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlPartnerDetailsPerson
            //
            this.pnlPartnerDetailsPerson.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPartnerDetailsPerson.AutoScroll = true;
            this.pnlPartnerDetailsPerson.AutoScrollMinSize = new System.Drawing.Size(550, 330);
            this.pnlPartnerDetailsPerson.Controls.Add(this.pnlBalloonTipAnchor);
            this.pnlPartnerDetailsPerson.Controls.Add(this.btnCreatedPartner);
            this.pnlPartnerDetailsPerson.Controls.Add(this.grpNames);
            this.pnlPartnerDetailsPerson.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerDetailsPerson.Name = "pnlPartnerDetailsPerson";
            this.pnlPartnerDetailsPerson.Size = new System.Drawing.Size(634, 530);
            this.pnlPartnerDetailsPerson.TabIndex = 0;

            //
            // btnCreatedPartner
            //
            this.btnCreatedPartner.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnCreatedPartner.CreatedBy = null;
            this.btnCreatedPartner.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedPartner.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedPartner.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedPartner.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnCreatedPartner.Image")));
            this.btnCreatedPartner.Location = new System.Drawing.Point(616, 2);
            this.btnCreatedPartner.ModifiedBy = null;
            this.btnCreatedPartner.Name = "btnCreatedPartner";
            this.btnCreatedPartner.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedPartner.TabIndex = 25;
            this.btnCreatedPartner.Tag = "dontdisable";

            //
            // grpNames
            //
            this.grpNames.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpNames.Controls.Add(this.pnlNotes);
            this.grpNames.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpNames.Location = new System.Drawing.Point(4, 0);
            this.grpNames.Name = "grpNames";
            this.grpNames.Size = new System.Drawing.Size(610, 176);
            this.grpNames.TabIndex = 0;
            this.grpNames.TabStop = false;
            this.grpNames.Text = "Notes";

            //
            // pnlNotes
            //
            this.pnlNotes.Controls.Add(this.txtPartnerComment);
            this.pnlNotes.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNotes.Location = new System.Drawing.Point(3, 17);
            this.pnlNotes.Name = "pnlNotes";
            this.pnlNotes.Size = new System.Drawing.Size(604, 151);
            this.pnlNotes.TabIndex = 6;

            //
            // txtPartnerComment
            //
            this.txtPartnerComment.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPartnerComment.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtPartnerComment.Location = new System.Drawing.Point(6, 0);
            this.txtPartnerComment.Multiline = true;
            this.txtPartnerComment.Name = "txtPartnerComment";
            this.txtPartnerComment.Size = new System.Drawing.Size(592, 146);
            this.txtPartnerComment.TabIndex = 22;
            this.txtPartnerComment.Text = "";

            //
            // pnlBalloonTipAnchor
            //
            this.pnlBalloonTipAnchor.Location = new System.Drawing.Point(60, 0);
            this.pnlBalloonTipAnchor.Name = "pnlBalloonTipAnchor";
            this.pnlBalloonTipAnchor.Size = new System.Drawing.Size(1, 1);
            this.pnlBalloonTipAnchor.TabIndex = 10;

            //
            // TUC_PartnerNotes
            //
            this.Controls.Add(this.pnlPartnerDetailsPerson);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_PartnerNotes";
            this.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsPerson.ResumeLayout(false);
            this.grpNames.ResumeLayout(false);
            this.pnlNotes.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlPartnerDetailsPerson;
        private System.Windows.Forms.GroupBox grpNames;
        private TbtnCreated btnCreatedPartner;
        private System.Windows.Forms.Panel pnlNotes;
        private System.Windows.Forms.TextBox txtPartnerComment;
        private TexpTextBoxStringLengthCheck expStringLengthCheckNotes;
        private System.Windows.Forms.Panel pnlBalloonTipAnchor;
    }
}