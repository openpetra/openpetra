/* auto generated with nant generateWinforms from UC_PartnerEdit_PartnerTabSet2.yaml
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
    partial class TUC_PartnerEdit_PartnerTabSet2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerEdit_PartnerTabSet2));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tabPartners = new Ict.Common.Controls.TTabVersatile();
            this.tpgAddresses = new System.Windows.Forms.TabPage();
            this.tpgPartnerDetails = new System.Windows.Forms.TabPage();
            this.tpgFoundationDetails = new System.Windows.Forms.TabPage();
            this.tpgSubscriptions = new System.Windows.Forms.TabPage();
            this.tpgPartnerTypes = new System.Windows.Forms.TabPage();
            this.tpgFamilyMembers = new System.Windows.Forms.TabPage();
            this.tpgNotes = new System.Windows.Forms.TabPage();
            this.tpgOfficeSpecific = new System.Windows.Forms.TabPage();

            this.pnlContent.SuspendLayout();
            this.tabPartners.SuspendLayout();
            this.tpgAddresses.SuspendLayout();
            this.tpgPartnerDetails.SuspendLayout();
            this.tpgFoundationDetails.SuspendLayout();
            this.tpgSubscriptions.SuspendLayout();
            this.tpgPartnerTypes.SuspendLayout();
            this.tpgFamilyMembers.SuspendLayout();
            this.tpgNotes.SuspendLayout();
            this.tpgOfficeSpecific.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.tabPartners);
            //
            // tpgAddresses
            //
            this.tpgAddresses.Location = new System.Drawing.Point(2,2);
            this.tpgAddresses.Name = "tpgAddresses";
            this.tpgAddresses.AutoSize = true;
            this.tpgAddresses.Text = "Addresses ({0})";
            this.tpgAddresses.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgPartnerDetails
            //
            this.tpgPartnerDetails.Location = new System.Drawing.Point(2,2);
            this.tpgPartnerDetails.Name = "tpgPartnerDetails";
            this.tpgPartnerDetails.AutoSize = true;
            this.tpgPartnerDetails.Text = "Partner Details";
            this.tpgPartnerDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgFoundationDetails
            //
            this.tpgFoundationDetails.Location = new System.Drawing.Point(2,2);
            this.tpgFoundationDetails.Name = "tpgFoundationDetails";
            this.tpgFoundationDetails.AutoSize = true;
            this.tpgFoundationDetails.Text = "Foundation Details";
            this.tpgFoundationDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgSubscriptions
            //
            this.tpgSubscriptions.Location = new System.Drawing.Point(2,2);
            this.tpgSubscriptions.Name = "tpgSubscriptions";
            this.tpgSubscriptions.AutoSize = true;
            this.tpgSubscriptions.Text = "Subscriptions ({0})";
            this.tpgSubscriptions.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgPartnerTypes
            //
            this.tpgPartnerTypes.Location = new System.Drawing.Point(2,2);
            this.tpgPartnerTypes.Name = "tpgPartnerTypes";
            this.tpgPartnerTypes.AutoSize = true;
            this.tpgPartnerTypes.Text = "Special Types ({0})";
            this.tpgPartnerTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgFamilyMembers
            //
            this.tpgFamilyMembers.Location = new System.Drawing.Point(2,2);
            this.tpgFamilyMembers.Name = "tpgFamilyMembers";
            this.tpgFamilyMembers.AutoSize = true;
            this.tpgFamilyMembers.Text = "Family Members";
            this.tpgFamilyMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgNotes
            //
            this.tpgNotes.Location = new System.Drawing.Point(2,2);
            this.tpgNotes.Name = "tpgNotes";
            this.tpgNotes.AutoSize = true;
            this.tpgNotes.Text = "Notes ({0})";
            this.tpgNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgOfficeSpecific
            //
            this.tpgOfficeSpecific.Location = new System.Drawing.Point(2,2);
            this.tpgOfficeSpecific.Name = "tpgOfficeSpecific";
            this.tpgOfficeSpecific.AutoSize = true;
            this.tpgOfficeSpecific.Text = "Local Data";
            this.tpgOfficeSpecific.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tabPartners
            //
            this.tabPartners.Name = "tabPartners";
            this.tabPartners.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPartners.Controls.Add(this.tpgAddresses);
            this.tabPartners.Controls.Add(this.tpgPartnerDetails);
            this.tabPartners.Controls.Add(this.tpgFoundationDetails);
            this.tabPartners.Controls.Add(this.tpgSubscriptions);
            this.tabPartners.Controls.Add(this.tpgPartnerTypes);
            this.tabPartners.Controls.Add(this.tpgFamilyMembers);
            this.tabPartners.Controls.Add(this.tpgNotes);
            this.tabPartners.Controls.Add(this.tpgOfficeSpecific);
            this.tabPartners.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabPartners.ShowToolTips = true;
            this.tabPartners.SelectedIndexChanged += new System.EventHandler(this.TabSelectionChanged);

            //
            // TUC_PartnerEdit_PartnerTabSet2
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TUC_PartnerEdit_PartnerTabSet2";
            this.Text = "";

	
            this.tpgOfficeSpecific.ResumeLayout(false);
            this.tpgNotes.ResumeLayout(false);
            this.tpgFamilyMembers.ResumeLayout(false);
            this.tpgPartnerTypes.ResumeLayout(false);
            this.tpgSubscriptions.ResumeLayout(false);
            this.tpgFoundationDetails.ResumeLayout(false);
            this.tpgPartnerDetails.ResumeLayout(false);
            this.tpgAddresses.ResumeLayout(false);
            this.tabPartners.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private Ict.Common.Controls.TTabVersatile tabPartners;
        private System.Windows.Forms.TabPage tpgAddresses;
        private System.Windows.Forms.TabPage tpgPartnerDetails;
        private System.Windows.Forms.TabPage tpgFoundationDetails;
        private System.Windows.Forms.TabPage tpgSubscriptions;
        private System.Windows.Forms.TabPage tpgPartnerTypes;
        private System.Windows.Forms.TabPage tpgFamilyMembers;
        private System.Windows.Forms.TabPage tpgNotes;
        private System.Windows.Forms.TabPage tpgOfficeSpecific;
    }
}
