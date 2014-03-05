//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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
using System;
using System.Drawing;
using System.Windows.Forms;
using Ict.Common;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    public partial class TPeriodEnd
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Standard GUI-Routine ...
        /// </summary>
        /// <param name="disposing"></param>
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

        private void InitializeComponent()
        {
            // In normal cases here TPeriodEnd shall be inserted but
            // this will not work. A MissingManifestResourceException is thrown
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TFrmGLBatch));

            this.btnPeriodEnd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbxMessage = new TextBox();
            this.SuspendLayout();

            tbxMessage.Location = new System.Drawing.Point(15, 15);
            tbxMessage.Name = "tbxMessage";
            tbxMessage.Multiline = true;
            tbxMessage.Size = new Size(this.Width - 30, this.Height - 100);
            tbxMessage.ScrollBars = ScrollBars.Vertical;
            tbxMessage.ReadOnly = true;


            //
            // btnPeriodEnd
            //
            this.btnPeriodEnd.Name = "btnPeriodEnd";
            this.btnPeriodEnd.Size = new System.Drawing.Size(144, 23);
            this.btnPeriodEnd.TabIndex = 2;

            if (blnIsInMonthMode)
            {
                this.btnPeriodEnd.Text = Catalog.GetString("Month End");
            }
            else
            {
                this.btnPeriodEnd.Text = Catalog.GetString("Year End");
            }

            this.btnPeriodEnd.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(137, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = Catalog.GetString("Cancel");
            this.btnCancel.UseVisualStyleBackColor = true;


            this.ClientSize = new System.Drawing.Size(400, 300);
            this.CancelButton = btnCancel;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPeriodEnd);
            this.Controls.Add(this.tbxMessage);

            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "GLMonthEnd";

            if (blnIsInMonthMode)
            {
                this.Text = Catalog.GetString("Periodic End - Month ...");
            }
            else
            {
                this.Text = Catalog.GetString("Periodic End - Year ...");
            }

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnPeriodEnd;
        private System.Windows.Forms.TextBox tbxMessage;
    }
}