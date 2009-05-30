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
using System.Windows.Forms;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner
{
    partial class TExtractPartnersSubscribedWinForm
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
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TExtractPartnersSubscribedWinForm));
            this.grdPartners = new Ict.Common.Controls.TsgrdDataGrid();
            this.btnOK = new System.Windows.Forms.Button();
            this.lbltop = new System.Windows.Forms.Label();
            this.lblMiddle = new System.Windows.Forms.Label();
            this.lblBottom = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.tbhMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbrMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).BeginInit();
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
            this.XPMenuItemExtender.SetNewStyleActive(this.mniEdit, true);
            this.mniEdit.Visible = false;

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
            // tbbSave
            //
            this.tbbSave.Visible = false;

            //
            // mniFile
            //
            this.XPMenuItemExtender.SetNewStyleActive(this.mniFile, true);
            this.mniFile.Visible = false;

            //
            // mniFileClose
            //
            this.XPMenuItemExtender.SetMenuGlyph(this.mniFileClose, ((System.Drawing.Image)(resources.GetObject("mniFileClose.MenuGlyph"))));
            this.XPMenuItemExtender.SetNewStyleActive(this.mniFileClose, true);

            //
            // mniHelp
            //
            this.XPMenuItemExtender.SetNewStyleActive(this.mniHelp, true);
            this.mniHelp.Visible = false;

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
            this.tbhMain.Size = new System.Drawing.Size(380, 27);
            this.tbhMain.Visible = false;

            //
            // tbrMain
            //
            this.tbrMain.Visible = false;

            //
            // tbbClose
            //
            this.tbbClose.Enabled = false;
            this.tbbClose.Visible = false;

            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 298);
            this.stbMain.Size = new System.Drawing.Size(380, 22);
            this.stbMain.Visible = false;

            //
            // stpInfo
            //
            this.stpInfo.Width = 363;

            //
            // grdPartners
            //
            this.grdPartners.AlternatingBackgroundColour =
                System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grdPartners.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.grdPartners.AutoFindColumn = ((short)(0));
            this.grdPartners.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;
            this.grdPartners.AutoStretchColumnsToFitWidth = true;
            this.grdPartners.BackColor = System.Drawing.SystemColors.ControlDark;
            this.grdPartners.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdPartners.DeleteQuestionMessage = "You have chosen to delete this record.\'#13#10#13#10\'Do you really want to delete " +
                                                     "it?";
            this.grdPartners.FixedRows = 1;
            this.grdPartners.KeepRowSelectedAfterSort = true;
            this.grdPartners.Location = new System.Drawing.Point(16, 86);
            this.grdPartners.MinimumHeight = 1;
            this.grdPartners.Name = "grdPartners";
            this.grdPartners.Size = new System.Drawing.Size(345, 198);
            this.grdPartners.SortableHeaders = true;
            this.grdPartners.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows | SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) |
                                               SourceGrid.GridSpecialKeys.Shift)));
            this.grdPartners.TabIndex = 1;
            this.grdPartners.TabStop = true;
            this.grdPartners.ToolTipTextDelegate = null;

            //
            // btnOK
            //
            this.btnOK.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(285, 290);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";

            //
            // lbltop
            //
            this.lbltop.Location = new System.Drawing.Point(16, 14);
            this.lbltop.Name = "lbltop";
            this.lbltop.Size = new System.Drawing.Size(312, 16);
            this.lbltop.TabIndex = 5;
            this.lbltop.Text = "These partners already have a Subscription for";

            //
            // lblMiddle
            //
            this.lblMiddle.Location = new System.Drawing.Point(28, 38);
            this.lblMiddle.Name = "lblMiddle";
            this.lblMiddle.Size = new System.Drawing.Size(338, 16);
            this.lblMiddle.TabIndex = 6;

            //
            // lblBottom
            //
            this.lblBottom.Location = new System.Drawing.Point(16, 62);
            this.lblBottom.Name = "lblBottom";
            this.lblBottom.Size = new System.Drawing.Size(338, 16);
            this.lblBottom.TabIndex = 7;
            this.lblBottom.Text = "The Subscription was not added to the following Partners:";

            //
            // btnHelp
            //
            this.btnHelp.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHelp.Location = new System.Drawing.Point(16, 290);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(75, 23);
            this.btnHelp.TabIndex = 8;
            this.btnHelp.Text = "&Help";
            this.btnHelp.Click += new System.EventHandler(this.BtnHelp_Click);

            //
            // TExtractPartnersSubscribedWinForm
            //
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(380, 320);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.lblBottom);
            this.Controls.Add(this.lblMiddle);
            this.Controls.Add(this.lbltop);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grdPartners);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TExtractPartnersSubscribedWinForm";
            this.Text = "Partners Who Were Already Subscribed";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TExtractPartnersSubscribedWinForm_Closing);
            this.Controls.SetChildIndex(this.grdPartners, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.lbltop, 0);
            this.Controls.SetChildIndex(this.lblMiddle, 0);
            this.Controls.SetChildIndex(this.lblBottom, 0);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.stbMain, 0);
            this.Controls.SetChildIndex(this.tbhMain, 0);
            this.tbhMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbrMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).EndInit();
            this.ResumeLayout(false);
        }

        private TsgrdDataGrid grdPartners;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lbltop;
        private System.Windows.Forms.Label lblMiddle;
        private System.Windows.Forms.Label lblBottom;
        private System.Windows.Forms.Button btnHelp;
    }
}