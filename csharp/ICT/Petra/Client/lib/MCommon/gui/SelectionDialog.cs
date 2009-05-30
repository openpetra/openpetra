/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       morayh
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
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MCommon
{
    /// todoComment
    public class TSelectionDialog : System.Windows.Forms.Form
    {
        // TODO: TFrmPetraDialog
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).BeginInit();
            this.SuspendLayout();

            //
            // btnOK
            //
            this.btnOK.Name = "btnOK";
            this.sbtForm.SetStatusBarText(this.btnOK, "Accept data and continue");

            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 214);
            this.pnlBtnOKCancelHelpLayout.Name = "pnlBtnOKCancelHelpLayout";
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(294, 32);

            //
            // btnCancel
            //
            this.btnCancel.Name = "btnCancel";
            this.sbtForm.SetStatusBarText(this.btnCancel, "Cancel data entry and close");

            //
            // btnHelp
            //
            this.btnHelp.Location = new System.Drawing.Point(210, 8);
            this.btnHelp.Name = "btnHelp";
            this.sbtForm.SetStatusBarText(this.btnHelp, "Help");

            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 246);
            this.stbMain.Name = "stbMain";
            this.stbMain.Size = new System.Drawing.Size(294, 22);

            //
            // stpInfo
            //
            this.stpInfo.Width = 294;

            //
            // TSelectionDialog
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(294, 268);
            this.Name = "TSelectionDialog";
            this.Text = "Selection";
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        /// <summary>
        /// <summary> Clean up any resources being used. </summary>
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
        public TSelectionDialog() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}