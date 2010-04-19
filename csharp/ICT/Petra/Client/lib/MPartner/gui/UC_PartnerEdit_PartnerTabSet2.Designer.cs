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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTest = new System.Windows.Forms.Label();
            this.tpgDetails = new System.Windows.Forms.TabPage();
            this.ucoPartnerDetails = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Person2();
            this.tpgFoundationDetails = new System.Windows.Forms.TabPage();
            this.tpgSubscriptions = new System.Windows.Forms.TabPage();
            this.tpgSpecialTypes = new System.Windows.Forms.TabPage();
            this.tpgFamilyMembers = new System.Windows.Forms.TabPage();
            this.tpgOfficeSpecific = new System.Windows.Forms.TabPage();
            this.tpgNotes = new System.Windows.Forms.TabPage();

            this.pnlContent.SuspendLayout();
            this.tabPartners.SuspendLayout();
            this.tpgAddresses.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tpgDetails.SuspendLayout();
            this.tpgFoundationDetails.SuspendLayout();
            this.tpgSubscriptions.SuspendLayout();
            this.tpgSpecialTypes.SuspendLayout();
            this.tpgFamilyMembers.SuspendLayout();
            this.tpgOfficeSpecific.SuspendLayout();
            this.tpgNotes.SuspendLayout();

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
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.tpgAddresses.Controls.Add(this.tableLayoutPanel1);
            //
            // lblTest
            //
            this.lblTest.Location = new System.Drawing.Point(2,2);
            this.lblTest.Name = "lblTest";
            this.lblTest.AutoSize = true;
            this.lblTest.Text = "Test only:";
            this.lblTest.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblTest, 0, 0);
            this.tpgAddresses.Text = "Addresses ({0})";
            this.tpgAddresses.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgDetails
            //
            this.tpgDetails.Location = new System.Drawing.Point(2,2);
            this.tpgDetails.Name = "tpgDetails";
            this.tpgDetails.AutoSize = true;
            this.tpgDetails.Controls.Add(this.ucoPartnerDetails);
            //
            // ucoPartnerDetails
            //
            this.ucoPartnerDetails.Name = "ucoPartnerDetails";
            this.ucoPartnerDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpgDetails.Text = "Partner Details";
            this.tpgDetails.Dock = System.Windows.Forms.DockStyle.Fill;
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
            // tpgSpecialTypes
            //
            this.tpgSpecialTypes.Location = new System.Drawing.Point(2,2);
            this.tpgSpecialTypes.Name = "tpgSpecialTypes";
            this.tpgSpecialTypes.AutoSize = true;
            this.tpgSpecialTypes.Text = "Special Types ({0})";
            this.tpgSpecialTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgFamilyMembers
            //
            this.tpgFamilyMembers.Location = new System.Drawing.Point(2,2);
            this.tpgFamilyMembers.Name = "tpgFamilyMembers";
            this.tpgFamilyMembers.AutoSize = true;
            this.tpgFamilyMembers.Text = "Family Members";
            this.tpgFamilyMembers.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgOfficeSpecific
            //
            this.tpgOfficeSpecific.Location = new System.Drawing.Point(2,2);
            this.tpgOfficeSpecific.Name = "tpgOfficeSpecific";
            this.tpgOfficeSpecific.AutoSize = true;
            this.tpgOfficeSpecific.Text = "Local Data";
            this.tpgOfficeSpecific.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tpgNotes
            //
            this.tpgNotes.Location = new System.Drawing.Point(2,2);
            this.tpgNotes.Name = "tpgNotes";
            this.tpgNotes.AutoSize = true;
            this.tpgNotes.Text = "Notes ({0})";
            this.tpgNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // tabPartners
            //
            this.tabPartners.Name = "tabPartners";
            this.tabPartners.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPartners.Controls.Add(this.tpgAddresses);
            this.tabPartners.Controls.Add(this.tpgDetails);
            this.tabPartners.Controls.Add(this.tpgFoundationDetails);
            this.tabPartners.Controls.Add(this.tpgSubscriptions);
            this.tabPartners.Controls.Add(this.tpgSpecialTypes);
            this.tabPartners.Controls.Add(this.tpgFamilyMembers);
            this.tabPartners.Controls.Add(this.tpgOfficeSpecific);
            this.tabPartners.Controls.Add(this.tpgNotes);

            //
            // TUC_PartnerEdit_PartnerTabSet2
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 500);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.pnlContent);
            this.Name = "TUC_PartnerEdit_PartnerTabSet2";
            this.Text = "";

	
            this.tpgNotes.ResumeLayout(false);
            this.tpgOfficeSpecific.ResumeLayout(false);
            this.tpgFamilyMembers.ResumeLayout(false);
            this.tpgSpecialTypes.ResumeLayout(false);
            this.tpgSubscriptions.ResumeLayout(false);
            this.tpgFoundationDetails.ResumeLayout(false);
            this.tpgDetails.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tpgAddresses.ResumeLayout(false);
            this.tabPartners.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlContent;
        private Ict.Common.Controls.TTabVersatile tabPartners;
        private System.Windows.Forms.TabPage tpgAddresses;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblTest;
        private System.Windows.Forms.TabPage tpgDetails;
        private Ict.Petra.Client.MPartner.Gui.TUC_PartnerDetails_Person2 ucoPartnerDetails;
        private System.Windows.Forms.TabPage tpgFoundationDetails;
        private System.Windows.Forms.TabPage tpgSubscriptions;
        private System.Windows.Forms.TabPage tpgSpecialTypes;
        private System.Windows.Forms.TabPage tpgFamilyMembers;
        private System.Windows.Forms.TabPage tpgOfficeSpecific;
        private System.Windows.Forms.TabPage tpgNotes;
    }
}
