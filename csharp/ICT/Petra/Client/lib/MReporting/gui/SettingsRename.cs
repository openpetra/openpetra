/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
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

namespace Ict.Petra.Client.MReporting.Gui
{
    /// <summary>
    /// form that helps with renaming report settings
    /// </summary>
    public class TFrmSettingsRename : System.Windows.Forms.Form
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblNewName;
        private System.Windows.Forms.Label lblOldName;
        private System.Windows.Forms.TextBox txtNewName;
        private System.Windows.Forms.TextBox txtOldName;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;

        /// <summary>
        /// the new name after renaming
        /// </summary>
        public string NewName
        {
            get
            {
                return txtNewName.Text;
            }

            set
            {
                txtNewName.Text = value;
            }
        }

        /// <summary>
        /// the old name before renaming
        /// </summary>
        public string OldName
        {
            get
            {
                return txtOldName.Text;
            }

            set
            {
                txtOldName.Text = value;
            }
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.lblNewName = new System.Windows.Forms.Label();
            this.lblOldName = new System.Windows.Forms.Label();
            this.txtNewName = new System.Windows.Forms.TextBox();
            this.txtOldName = new System.Windows.Forms.TextBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            //
            // lblNewName
            //
            this.lblNewName.Location = new System.Drawing.Point(8, 64);
            this.lblNewName.Name = "lblNewName";
            this.lblNewName.Size = new System.Drawing.Size(72, 23);
            this.lblNewName.TabIndex = 2;
            this.lblNewName.Text = "New Name:";

            //
            // lblOldName
            //
            this.lblOldName.Location = new System.Drawing.Point(8, 24);
            this.lblOldName.Name = "lblOldName";
            this.lblOldName.Size = new System.Drawing.Size(64, 23);
            this.lblOldName.TabIndex = 0;
            this.lblOldName.Text = "Old Name:";

            //
            // txtNewName
            //
            this.txtNewName.Location = new System.Drawing.Point(88, 64);
            this.txtNewName.Name = "txtNewName";
            this.txtNewName.Size = new System.Drawing.Size(256, 20);
            this.txtNewName.TabIndex = 3;
            this.txtNewName.Text = "New Name";

            //
            // txtOldName
            //
            this.txtOldName.Enabled = false;
            this.txtOldName.Location = new System.Drawing.Point(88, 24);
            this.txtOldName.Name = "txtOldName";
            this.txtOldName.Size = new System.Drawing.Size(256, 20);
            this.txtOldName.TabIndex = 1;
            this.txtOldName.Text = "OldName";

            //
            // BtnOk
            //
            this.BtnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnOk.Location = new System.Drawing.Point(88, 96);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.TabIndex = 4;
            this.BtnOk.Text = "&Ok";

            //
            // BtnCancel
            //
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(192, 96);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.TabIndex = 5;
            this.BtnCancel.Text = "&Cancel";

            //
            // TFrmSettingsRename
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(376, 133);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.txtOldName);
            this.Controls.Add(this.txtNewName);
            this.Controls.Add(this.lblOldName);
            this.Controls.Add(this.lblNewName);
            this.Name = "TFrmSettingsRename";
            this.Text = "Rename the Settings";
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
        public TFrmSettingsRename() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }
    }
}