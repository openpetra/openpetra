/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       petrih
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
using System.ComponentModel;
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner
{
    partial class TExtractSubscriptionAddWinForm
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

                if (FLogic != null)
                {
                    FLogic.Dispose();
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
                new System.ComponentModel.ComponentResourceManager(typeof(TExtractSubscriptionAddWinForm));
            this.MenuItem1 = new System.Windows.Forms.MenuItem();
            this.MenuItem2 = new System.Windows.Forms.MenuItem();
            this.MenuItem3 = new System.Windows.Forms.MenuItem();
            this.MenuItem4 = new System.Windows.Forms.MenuItem();
            this.MenuItem5 = new System.Windows.Forms.MenuItem();
            this.MenuItem6 = new System.Windows.Forms.MenuItem();
            this.MenuItem7 = new System.Windows.Forms.MenuItem();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.ucoDetails = new Ict.Petra.Client.MPartner.TUCPartnerSubscription();
            this.lblExtractName = new System.Windows.Forms.Label();
            this.lblExtractNameinfo = new System.Windows.Forms.Label();
            this.lblExtractID = new System.Windows.Forms.Label();
            this.lblExctractIDinfo = new System.Windows.Forms.Label();
            this.pnlBtnOKCancelHelpLayout = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.tbhMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbrMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).BeginInit();
            this.Panel1.SuspendLayout();
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            this.SuspendLayout();

            //
            // mniFileSave
            //
            this.XPMenuItemExtender.SetMenuGlyph(this.mniFileSave, ((System.Drawing.Image)(resources.GetObject("mniFileSave.MenuGlyph"))));
            this.XPMenuItemExtender.SetNewStyleActive(this.mniFileSave, true);

            //
            // mniFileSeparator1
            //
            this.XPMenuItemExtender.SetNewStyleActive(this.mniFileSeparator1, true);

            //
            // mniFilePrint
            //
            this.XPMenuItemExtender.SetMenuGlyph(this.mniFilePrint, ((System.Drawing.Image)(resources.GetObject("mniFilePrint.MenuGlyph"))));
            this.XPMenuItemExtender.SetNewStyleActive(this.mniFilePrint, true);

            //
            // mniFileSeparator2
            //
            this.XPMenuItemExtender.SetNewStyleActive(this.mniFileSeparator2, true);

            //
            // mniEdit
            //
            this.mniEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.MenuItem6, this.MenuItem7, this.MenuItem2, this.MenuItem3,
                                                                                  this.MenuItem4,
                                                                                  this.MenuItem5 });
            this.XPMenuItemExtender.SetNewStyleActive(this.mniEdit, true);

            //
            // mniEditUndoScreen
            //
            this.XPMenuItemExtender.SetMenuGlyph(this.mniEditUndoScreen, ((System.Drawing.Image)(resources.GetObject("mniEditUndoScreen.MenuGlyph"))));
            this.XPMenuItemExtender.SetNewStyleActive(this.mniEditUndoScreen, true);

            //
            // mniEditUndoCurrentField
            //
            this.XPMenuItemExtender.SetMenuGlyph(this.mniEditUndoCurrentField,
                ((System.Drawing.Image)(resources.GetObject("mniEditUndoCurrentField.MenuGlyph"))));
            this.XPMenuItemExtender.SetNewStyleActive(this.mniEditUndoCurrentField, true);

            //
            // mniEditSeparator1
            //
            this.XPMenuItemExtender.SetNewStyleActive(this.mniEditSeparator1, true);

            //
            // mniEditFind
            //
            this.XPMenuItemExtender.SetMenuGlyph(this.mniEditFind, ((System.Drawing.Image)(resources.GetObject("mniEditFind.MenuGlyph"))));
            this.XPMenuItemExtender.SetNewStyleActive(this.mniEditFind, true);

            //
            // mnuMain
            //
            this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.MenuItem1 });

            //
            // mniFile
            //
            this.XPMenuItemExtender.SetNewStyleActive(this.mniFile, true);

            //
            // mniFileClose
            //
            this.XPMenuItemExtender.SetMenuGlyph(this.mniFileClose, ((System.Drawing.Image)(resources.GetObject("mniFileClose.MenuGlyph"))));
            this.XPMenuItemExtender.SetNewStyleActive(this.mniFileClose, true);

            //
            // mniHelp
            //
            this.XPMenuItemExtender.SetNewStyleActive(this.mniHelp, true);

            //
            // mniHelpPetraHelp
            //
            this.XPMenuItemExtender.SetMenuGlyph(this.mniHelpPetraHelp, ((System.Drawing.Image)(resources.GetObject("mniHelpPetraHelp.MenuGlyph"))));
            this.XPMenuItemExtender.SetNewStyleActive(this.mniHelpPetraHelp, true);

            //
            // mniHelpDivider1
            //
            this.XPMenuItemExtender.SetNewStyleActive(this.mniHelpDivider1, true);

            //
            // mniHelpBugReport
            //
            this.XPMenuItemExtender.SetMenuGlyph(this.mniHelpBugReport, ((System.Drawing.Image)(resources.GetObject("mniHelpBugReport.MenuGlyph"))));
            this.XPMenuItemExtender.SetNewStyleActive(this.mniHelpBugReport, true);

            //
            // mniHelpDivider2
            //
            this.XPMenuItemExtender.SetNewStyleActive(this.mniHelpDivider2, true);

            //
            // mniHelpAboutPetra
            //
            this.XPMenuItemExtender.SetMenuGlyph(this.mniHelpAboutPetra, ((System.Drawing.Image)(resources.GetObject("mniHelpAboutPetra.MenuGlyph"))));
            this.XPMenuItemExtender.SetNewStyleActive(this.mniHelpAboutPetra, true);

            //
            // mniHelpDevelopmentTeam
            //
            this.XPMenuItemExtender.SetMenuGlyph(this.mniHelpDevelopmentTeam,
                ((System.Drawing.Image)(resources.GetObject("mniHelpDevelopmentTeam.MenuGlyph"))));
            this.XPMenuItemExtender.SetNewStyleActive(this.mniHelpDevelopmentTeam, true);

            //
            // tbhMain
            //
            this.tbhMain.Name = "tbhMain";
            this.tbhMain.Size = new System.Drawing.Size(776, 27);

            //
            // tbrMain
            //
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Visible = false;

            //
            // tbbClose
            //
            this.tbbClose.Enabled = false;

            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 318);
            this.stbMain.Name = "stbMain";
            this.stbMain.Size = new System.Drawing.Size(776, 22);
            this.stbMain.SizingGrip = false;

            //
            // stpInfo
            //
            this.stpInfo.Width = 776;

            //
            // MenuItem1
            //
            this.MenuItem1.Index = 3;
            this.XPMenuItemExtender.SetNewStyleActive(this.MenuItem1, true);
            this.MenuItem1.OwnerDraw = true;
            this.MenuItem1.Text = "";

            //
            // MenuItem2
            //
            this.MenuItem2.Index = 6;
            this.XPMenuItemExtender.SetNewStyleActive(this.MenuItem2, true);
            this.MenuItem2.OwnerDraw = true;
            this.MenuItem2.Text = "First Record";

            //
            // MenuItem3
            //
            this.MenuItem3.Index = 7;
            this.XPMenuItemExtender.SetNewStyleActive(this.MenuItem3, true);
            this.MenuItem3.OwnerDraw = true;
            this.MenuItem3.Text = "Last Record";

            //
            // MenuItem4
            //
            this.MenuItem4.Index = 8;
            this.XPMenuItemExtender.SetNewStyleActive(this.MenuItem4, true);
            this.MenuItem4.OwnerDraw = true;
            this.MenuItem4.Text = "Previous Record";

            //
            // MenuItem5
            //
            this.MenuItem5.Index = 9;
            this.XPMenuItemExtender.SetNewStyleActive(this.MenuItem5, true);
            this.MenuItem5.OwnerDraw = true;
            this.MenuItem5.Text = "Next Record";

            //
            // MenuItem6
            //
            this.MenuItem6.Index = 4;
            this.XPMenuItemExtender.SetNewStyleActive(this.MenuItem6, true);
            this.MenuItem6.OwnerDraw = true;
            this.MenuItem6.Text = "-";

            //
            // MenuItem7
            //
            this.MenuItem7.Index = 5;
            this.XPMenuItemExtender.SetNewStyleActive(this.MenuItem7, true);
            this.MenuItem7.OwnerDraw = true;
            this.MenuItem7.Text = "-";

            //
            // Panel1
            //
            this.Panel1.Controls.Add(this.ucoDetails);
            this.Panel1.Location = new System.Drawing.Point(8, 40);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(768, 360);
            this.Panel1.TabIndex = 999;

            //
            // ucoDetails
            //
            this.ucoDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucoDetails.ExctractOrPartnerKey = "Extract";
            this.ucoDetails.Location = new System.Drawing.Point(0, 0);
            this.ucoDetails.MainDS = null;
            this.ucoDetails.Name = "ucoDetails";
            this.ucoDetails.PublicationCode = null;
            this.ucoDetails.Size = new System.Drawing.Size(768, 360);
            this.ucoDetails.TabIndex = 4;
            this.ucoDetails.VerificationResultCollection = null;

            //
            // lblExtractName
            //
            this.lblExtractName.Location = new System.Drawing.Point(360, 24);
            this.lblExtractName.Name = "lblExtractName";
            this.lblExtractName.Size = new System.Drawing.Size(152, 16);
            this.lblExtractName.TabIndex = 3;

            //
            // lblExtractNameinfo
            //
            this.lblExtractNameinfo.Location = new System.Drawing.Point(272, 24);
            this.lblExtractNameinfo.Name = "lblExtractNameinfo";
            this.lblExtractNameinfo.Size = new System.Drawing.Size(88, 16);
            this.lblExtractNameinfo.TabIndex = 2;
            this.lblExtractNameinfo.Text = "Extract Name:";

            //
            // lblExtractID
            //
            this.lblExtractID.Location = new System.Drawing.Point(216, 24);
            this.lblExtractID.Name = "lblExtractID";
            this.lblExtractID.Size = new System.Drawing.Size(56, 16);
            this.lblExtractID.TabIndex = 1;

            //
            // lblExctractIDinfo
            //
            this.lblExctractIDinfo.Location = new System.Drawing.Point(144, 24);
            this.lblExctractIDinfo.Name = "lblExctractIDinfo";
            this.lblExctractIDinfo.Size = new System.Drawing.Size(72, 16);
            this.lblExctractIDinfo.TabIndex = 0;
            this.lblExctractIDinfo.Text = "Extract ID:";

            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Controls.Add(this.btnCancel);
            this.pnlBtnOKCancelHelpLayout.Controls.Add(this.btnOK);
            this.pnlBtnOKCancelHelpLayout.Controls.Add(this.btnHelp);
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(8, 288);
            this.pnlBtnOKCancelHelpLayout.Name = "pnlBtnOKCancelHelpLayout";
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(784, 32);
            this.pnlBtnOKCancelHelpLayout.TabIndex = 998;

            //
            // btnCancel
            //
            this.btnCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(674, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 20);
            this.sbtForm.SetStatusBarText(this.btnCancel, "Cancel data entry and close");
            this.btnCancel.TabIndex = 998;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);

            //
            // btnOK
            //
            this.btnOK.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(594, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 20);
            this.sbtForm.SetStatusBarText(this.btnOK, "Accept data and continue");
            this.btnOK.TabIndex = 997;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

            //
            // btnHelp
            //
            this.btnHelp.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right));
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHelp.Location = new System.Drawing.Point(4, 6);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 20);
            this.sbtForm.SetStatusBarText(this.btnHelp, "Help");
            this.btnHelp.TabIndex = 999;
            this.btnHelp.Text = "&Help";
            this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);

            //
            // TExtractSubscriptionAddWinForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(776, 340);
            this.Controls.Add(this.pnlBtnOKCancelHelpLayout);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.lblExctractIDinfo);
            this.Controls.Add(this.lblExtractID);
            this.Controls.Add(this.lblExtractNameinfo);
            this.Controls.Add(this.lblExtractName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TExtractSubscriptionAddWinForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Add Subscription";
            this.Load += new System.EventHandler(this.TExtractSubscriptionAddWinForm_Load);
            this.Closing += new CancelEventHandler(this.TExtractSubscriptionAddWinForm_Closing);
            this.Controls.SetChildIndex(this.lblExtractName, 0);
            this.Controls.SetChildIndex(this.lblExtractNameinfo, 0);
            this.Controls.SetChildIndex(this.lblExtractID, 0);
            this.Controls.SetChildIndex(this.lblExctractIDinfo, 0);
            this.Controls.SetChildIndex(this.Panel1, 0);
            this.Controls.SetChildIndex(this.pnlBtnOKCancelHelpLayout, 0);
            this.Controls.SetChildIndex(this.stbMain, 0);
            this.Controls.SetChildIndex(this.tbhMain, 0);
            this.tbhMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbrMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).EndInit();
            this.Panel1.ResumeLayout(false);
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.MenuItem MenuItem1;
        private System.Windows.Forms.MenuItem MenuItem2;
        private System.Windows.Forms.MenuItem MenuItem3;
        private System.Windows.Forms.MenuItem MenuItem4;
        private System.Windows.Forms.MenuItem MenuItem5;
        private System.Windows.Forms.MenuItem MenuItem6;
        private System.Windows.Forms.MenuItem MenuItem7;
        private TUCPartnerSubscription ucoDetails;
        private System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.Panel pnlBtnOKCancelHelpLayout;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Label lblExctractIDinfo;
        private System.Windows.Forms.Label lblExtractID;
        private System.Windows.Forms.Label lblExtractNameinfo;
        private System.Windows.Forms.Label lblExtractName;
    }
}