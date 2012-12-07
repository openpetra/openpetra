//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Resources;

namespace Ict.Tools.PatchTool.Library
{
/// <summary>
/// Window to display the patching process progress
/// </summary>
    public class TFrmStatus : System.Windows.Forms.Form
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnOK;

        /// <summary>Private Declarations</summary>
        private Boolean FHasError;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources;
            resources = new System.Resources.ResourceManager(typeof(TFrmStatus));
            this.btnOK = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.SuspendLayout();

            /*  */
            /* btnOK */
            /*  */
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(144, 248);
            this.btnOK.Name = "btnOK";
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";

            /*  */
            /* txtStatus */
            /*  */
            this.txtStatus.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatus.Location = new System.Drawing.Point(0, 8);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStatus.Size = new System.Drawing.Size(372, 232);
            this.txtStatus.TabIndex = 2;
            this.txtStatus.Text = "";

            /*  */
            /* TFrmStatus */
            /*  */
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(376, 273);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TFrmStatus";
            this.Text = "Installing the OpenPetra patch";
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
        /// constructor
        /// </summary>
        public TFrmStatus() : base()
        {
            InitializeComponent();
            FHasError = false;
            btnOK.Enabled = false;
        }

        /// <summary>
        /// add a new line to the status window
        /// </summary>
        /// <param name="s"></param>
        public void AddLine(string s)
        {
            if (txtStatus.Text.Length > 0)
            {
                txtStatus.Text = txtStatus.Text + Environment.NewLine + s;
            }
            else
            {
                txtStatus.Text = s;
            }

            txtStatus.ScrollToCaret();

            if ((s.ToLower().IndexOf("error") != -1) || (s.ToLower().IndexOf("problem") != -1) || (s.ToLower().IndexOf("note:") != -1))
            {
                FHasError = true;
                btnOK.Enabled = true;
            }

            /* there can be several "success"ful patches, so look out for keyword "finished" */
            if (s.ToLower().IndexOf("finished") != -1)
            {
                btnOK.Enabled = true;
            }
        }

        /// <summary>
        /// close the window if there has not been an error
        /// </summary>
        /// <returns></returns>
        public Boolean CloseIfNoError()
        {
            Boolean ReturnValue;

            ReturnValue = false;

            if (!FHasError)
            {
                ReturnValue = true;
                Close();
            }

            return ReturnValue;
        }
    }
}