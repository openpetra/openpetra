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
using Ict.Common.Controls;

namespace Ict.Petra.Client.MReporting.Gui
{
    partial class UC_Sorting
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the control.
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
            this.cmbSortby3 = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblSortBy3 = new System.Windows.Forms.Label();
            this.cmbSortby2 = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblSortBy2 = new System.Windows.Forms.Label();
            this.cmbSortby1 = new Ict.Common.Controls.TCmbAutoComplete();
            this.LblSortBy1 = new System.Windows.Forms.Label();

            this.SuspendLayout();

            //
            // cmbSortby3
            //
            this.cmbSortby3.AcceptNewValues = false;
            this.cmbSortby3.CaseSensitiveSearch = false;
            this.cmbSortby3.ColumnsToSearch = null;
            this.cmbSortby3.Location = new System.Drawing.Point(163, 103);
            this.cmbSortby3.Name = "cmbSortby3";
            this.cmbSortby3.Size = new System.Drawing.Size(240, 21);
            this.cmbSortby3.SuppressSelectionColor = true;
            this.cmbSortby3.TabIndex = 11;
            this.cmbSortby3.SelectedIndexChanged += new System.EventHandler(this.CmbSortby_SelectedIndexChanged);

            //
            // lblSortBy3
            //
            this.lblSortBy3.Location = new System.Drawing.Point(38, 103);
            this.lblSortBy3.Name = "lblSortBy3";
            this.lblSortBy3.Size = new System.Drawing.Size(120, 25);
            this.lblSortBy3.TabIndex = 10;
            this.lblSortBy3.Text = "Sort last by:";

            //
            // cmbSortby2
            //
            this.cmbSortby2.AcceptNewValues = false;
            this.cmbSortby2.CaseSensitiveSearch = false;
            this.cmbSortby2.ColumnsToSearch = null;
            this.cmbSortby2.Location = new System.Drawing.Point(163, 69);
            this.cmbSortby2.Name = "cmbSortby2";
            this.cmbSortby2.Size = new System.Drawing.Size(240, 21);
            this.cmbSortby2.SuppressSelectionColor = true;
            this.cmbSortby2.TabIndex = 9;
            this.cmbSortby2.SelectedIndexChanged += new System.EventHandler(this.CmbSortby_SelectedIndexChanged);

            //
            // lblSortBy2
            //
            this.lblSortBy2.Location = new System.Drawing.Point(38, 69);
            this.lblSortBy2.Name = "lblSortBy2";
            this.lblSortBy2.Size = new System.Drawing.Size(120, 25);
            this.lblSortBy2.TabIndex = 8;
            this.lblSortBy2.Text = "Sort then by:";

            //
            // cmbSortby1
            //
            this.cmbSortby1.AcceptNewValues = false;
            this.cmbSortby1.CaseSensitiveSearch = false;
            this.cmbSortby1.ColumnsToSearch = null;
            this.cmbSortby1.Location = new System.Drawing.Point(163, 34);
            this.cmbSortby1.Name = "cmbSortby1";
            this.cmbSortby1.Size = new System.Drawing.Size(240, 21);
            this.cmbSortby1.SuppressSelectionColor = true;
            this.cmbSortby1.TabIndex = 7;
            this.cmbSortby1.SelectedIndexChanged += new System.EventHandler(this.CmbSortby_SelectedIndexChanged);

            //
            // LblSortBy1
            //
            this.LblSortBy1.Location = new System.Drawing.Point(38, 34);
            this.LblSortBy1.Name = "LblSortBy1";
            this.LblSortBy1.Size = new System.Drawing.Size(120, 25);
            this.LblSortBy1.TabIndex = 6;
            this.LblSortBy1.Text = "Sort first by:";

            //
            // UC_Sorting
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UC_Sorting";
            this.Size = new System.Drawing.Size(650, 386);
            this.Controls.Add(cmbSortby1);
            this.Controls.Add(cmbSortby2);
            this.Controls.Add(cmbSortby3);
            this.Controls.Add(LblSortBy1);
            this.Controls.Add(lblSortBy2);
            this.Controls.Add(lblSortBy3);
            this.ResumeLayout(false);
        }

        private TCmbAutoComplete cmbSortby3;
        private System.Windows.Forms.Label lblSortBy3;
        private TCmbAutoComplete cmbSortby2;
        private System.Windows.Forms.Label lblSortBy2;
        private TCmbAutoComplete cmbSortby1;
        private System.Windows.Forms.Label LblSortBy1;
    }
}