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

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Description of GLRevaluation.
    /// </summary>
    public partial class TPeriodEndMonthly
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
            this.btnMonthEnd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            int leftColStart = 32;
            int leftColWidth = 240;
            int colSpace = 15;
            int rowY = 5;
            int rowHeight = 25;
            int rightColWidth = 350;

            int buttonTop = 210;
            int buttonLeft = 140;


            this.ClientSize = new System.Drawing.Size(600, rowY + 50);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnMonthEnd);

            //this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "GLMonthEnd";
            this.Text = "Periodic End - Month ...";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnMonthEnd;
    }
}