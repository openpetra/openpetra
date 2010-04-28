/* auto generated with nant generateWinforms from UC_Sorting.yaml
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

namespace Ict.Petra.Client.MReporting.Gui
{
    partial class TFrmUC_Sorting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmUC_Sorting));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbSortBy1 = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblSortBy1 = new System.Windows.Forms.Label();
            this.cmbSortBy2 = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblSortBy2 = new System.Windows.Forms.Label();
            this.cmbSortBy3 = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblSortBy3 = new System.Windows.Forms.Label();

            this.pnlContent.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlContent.Controls.Add(this.tableLayoutPanel1);
            //
            // cmbSortBy1
            //
            this.cmbSortBy1.Location = new System.Drawing.Point(2,2);
            this.cmbSortBy1.Name = "cmbSortBy1";
            this.cmbSortBy1.Size = new System.Drawing.Size(150, 28);
            this.cmbSortBy1.SelectedValueChanged += new System.EventHandler(this.CmbSortBy_SelectedValueChanged);
            //
            // lblSortBy1
            //
            this.lblSortBy1.Location = new System.Drawing.Point(2,2);
            this.lblSortBy1.Name = "lblSortBy1";
            this.lblSortBy1.AutoSize = true;
            this.lblSortBy1.Text = "Sort First By:";
            this.lblSortBy1.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblSortBy1.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // cmbSortBy2
            //
            this.cmbSortBy2.Location = new System.Drawing.Point(2,2);
            this.cmbSortBy2.Name = "cmbSortBy2";
            this.cmbSortBy2.Size = new System.Drawing.Size(150, 28);
            this.cmbSortBy2.SelectedValueChanged += new System.EventHandler(this.CmbSortBy_SelectedValueChanged);
            //
            // lblSortBy2
            //
            this.lblSortBy2.Location = new System.Drawing.Point(2,2);
            this.lblSortBy2.Name = "lblSortBy2";
            this.lblSortBy2.AutoSize = true;
            this.lblSortBy2.Text = "Sort Then By:";
            this.lblSortBy2.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblSortBy2.Dock = System.Windows.Forms.DockStyle.Right;
            //
            // cmbSortBy3
            //
            this.cmbSortBy3.Location = new System.Drawing.Point(2,2);
            this.cmbSortBy3.Name = "cmbSortBy3";
            this.cmbSortBy3.Size = new System.Drawing.Size(150, 28);
            this.cmbSortBy3.SelectedValueChanged += new System.EventHandler(this.CmbSortBy_SelectedValueChanged);
            //
            // lblSortBy3
            //
            this.lblSortBy3.Location = new System.Drawing.Point(2,2);
            this.lblSortBy3.Name = "lblSortBy3";
            this.lblSortBy3.AutoSize = true;
            this.lblSortBy3.Text = "Sort Last By:";
            this.lblSortBy3.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblSortBy3.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblSortBy1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSortBy2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSortBy3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbSortBy1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbSortBy2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbSortBy3, 1, 2);

            //
            // TFrmUC_Sorting
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 500);
            // this.rpsForm.SetRestoreLocation(this, false);  for the moment false, to avoid problems with size
            this.Controls.Add(this.pnlContent);
            this.Name = "TFrmUC_Sorting";
            this.Text = "";

	
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Ict.Common.Controls.TCmbAutoComplete cmbSortBy1;
        private System.Windows.Forms.Label lblSortBy1;
        private Ict.Common.Controls.TCmbAutoComplete cmbSortBy2;
        private System.Windows.Forms.Label lblSortBy2;
        private Ict.Common.Controls.TCmbAutoComplete cmbSortBy3;
        private System.Windows.Forms.Label lblSortBy3;
    }
}
