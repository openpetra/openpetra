/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;

namespace Ict.Petra.Client.MPartner
{
    /// UserControl that makes up the TabControl for 'Finance Data' in Partner Edit
    /// screen.
    ///
    /// @Comment At the moment this UserControl is only a very basic implementation,
    ///   just to give us an idea how things might look and work in the future.
    public class TUC_PartnerEdit_FinanceTabSet : System.Windows.Forms.UserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private TTabVersatile tabFinanceTab;
        private System.Windows.Forms.TabPage tbpDonorHistory;

        /// <summary>GroupBox1: System.Windows.Forms.GroupBox;</summary>
        private System.Windows.Forms.ImageList imlTabIcons;
        private System.Windows.Forms.ImageList imlButtonIcons;
        private System.Windows.Forms.TabPage tbpRecipientHistory;
        private System.Windows.Forms.TabPage tbpFinanceReports;
        private System.Windows.Forms.TabPage tbpBankAccounts;
        private System.Windows.Forms.TabPage tbpGiftReceipting;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerEdit_FinanceTabSet));
            this.components = new System.ComponentModel.Container();
            this.imlButtonIcons = new System.Windows.Forms.ImageList(this.components);
            this.imlTabIcons = new System.Windows.Forms.ImageList(this.components);
            this.tabFinanceTab = new Ict.Common.Controls.TTabVersatile();
            this.tbpDonorHistory = new System.Windows.Forms.TabPage();
            this.tbpFinanceReports = new System.Windows.Forms.TabPage();
            this.tbpRecipientHistory = new System.Windows.Forms.TabPage();
            this.tbpBankAccounts = new System.Windows.Forms.TabPage();
            this.tbpGiftReceipting = new System.Windows.Forms.TabPage();
            this.tabFinanceTab.SuspendLayout();
            this.SuspendLayout();

            //
            // imlButtonIcons
            //
            this.imlButtonIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imlButtonIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject('i' + "mlButtonIcons.ImageStream")));
            this.imlButtonIcons.TransparentColor = System.Drawing.Color.Transparent;

            //
            // imlTabIcons
            //
            this.imlTabIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imlTabIcons.TransparentColor = System.Drawing.Color.Transparent;

            //
            // tabFinanceTab
            //
            this.tabFinanceTab.AllowDrop = true;
            this.tabFinanceTab.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.tabFinanceTab.Controls.Add(this.tbpDonorHistory);
            this.tabFinanceTab.Controls.Add(this.tbpFinanceReports);
            this.tabFinanceTab.Controls.Add(this.tbpRecipientHistory);
            this.tabFinanceTab.Controls.Add(this.tbpBankAccounts);
            this.tabFinanceTab.Controls.Add(this.tbpGiftReceipting);
            this.tabFinanceTab.ImageList = this.imlTabIcons;
            this.tabFinanceTab.Location = new System.Drawing.Point(1, 1);
            this.tabFinanceTab.Name = "tabFinanceTab";
            this.tabFinanceTab.SelectedIndex = 0;
            this.tabFinanceTab.Size = new System.Drawing.Size(752, 470);
            this.tabFinanceTab.TabIndex = 87;

            //
            // tbpDonorHistory
            //
            this.tbpDonorHistory.BackColor = System.Drawing.SystemColors.Control;
            this.tbpDonorHistory.DockPadding.All = 2;
            this.tbpDonorHistory.Location = new System.Drawing.Point(4, 23);
            this.tbpDonorHistory.Name = "tbpDonorHistory";
            this.tbpDonorHistory.Size = new System.Drawing.Size(744, 443);
            this.tbpDonorHistory.TabIndex = 0;
            this.tbpDonorHistory.Text = "Donor History";

            //
            // tbpFinanceReports
            //
            this.tbpFinanceReports.Location = new System.Drawing.Point(4, 23);
            this.tbpFinanceReports.Name = "tbpFinanceReports";
            this.tbpFinanceReports.Size = new System.Drawing.Size(744, 443);
            this.tbpFinanceReports.TabIndex = 2;
            this.tbpFinanceReports.Text = "Finance Reports";

            //
            // tbpRecipientHistory
            //
            this.tbpRecipientHistory.Location = new System.Drawing.Point(4, 23);
            this.tbpRecipientHistory.Name = "tbpRecipientHistory";
            this.tbpRecipientHistory.Size = new System.Drawing.Size(744, 443);
            this.tbpRecipientHistory.TabIndex = 1;
            this.tbpRecipientHistory.Text = "Recipient History";

            //
            // tbpBankAccounts
            //
            this.tbpBankAccounts.Location = new System.Drawing.Point(4, 23);
            this.tbpBankAccounts.Name = "tbpBankAccounts";
            this.tbpBankAccounts.Size = new System.Drawing.Size(744, 443);
            this.tbpBankAccounts.TabIndex = 3;
            this.tbpBankAccounts.Text = "Bank Accounts";

            //
            // tbpGiftReceipting
            //
            this.tbpGiftReceipting.Location = new System.Drawing.Point(4, 23);
            this.tbpGiftReceipting.Name = "tbpGiftReceipting";
            this.tbpGiftReceipting.Size = new System.Drawing.Size(744, 443);
            this.tbpGiftReceipting.TabIndex = 4;
            this.tbpGiftReceipting.Text = "Gift Receipting";

            //
            // TUC_PartnerEdit_FinanceTabSet
            //
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Controls.Add(this.tabFinanceTab);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_PartnerEdit_FinanceTabSet";
            this.Size = new System.Drawing.Size(752, 472);
            this.tabFinanceTab.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        public TUC_PartnerEdit_FinanceTabSet() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /*
         * procedure TUC_PartnerEdit_FinanceTabSet.tabFinanceTab_SelectedIndexChanged(sender: System.Object;
         * e: System.EventArgs);
         * begin
         * case tabFinanceTab.SelectedIndex of
         * 0:
         * begin
         * end;
         *
         * 1:
         * begin
         * end;
         *
         * 2:
         * begin
         *
         * end;
         * end;
         * end;
         */

        /// <summary>
        /// procedure tabFinanceTab_SelectedIndexChanged(sender: System.Object; e: System.EventArgs); <summary> Clean up any resources being used. </summary>
        /// </summary>
        /// <returns>void</returns>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
        }

        /// <summary>
        /// Private Declarations
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseUserControl()
        {
            tbpDonorHistory.Enabled = false;
            tbpFinanceReports.Enabled = false;
            tbpRecipientHistory.Enabled = false;
            tbpBankAccounts.Enabled = false;
            tbpGiftReceipting.Enabled = false;
        }
    }
}