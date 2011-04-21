//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
//
using System;
using System.Windows.Forms;
using System.Drawing.Printing;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.CommonControls
{
    partial class TUC_CountryLabelledComboBox
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
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbCountry = new System.Windows.Forms.ComboBox();
            this.lblCountryName = new System.Windows.Forms.Label();
            this.SuspendLayout();

            //
            // cmbCountry
            //
            this.cmbCountry.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (Byte)0);
            this.cmbCountry.Location = new System.Drawing.Point(0, 0);
            this.cmbCountry.Name = "cmbCountry";
            this.cmbCountry.Size = new System.Drawing.Size(52, 21);
            this.cmbCountry.TabIndex = 85;
            this.cmbCountry.SelectedValueChanged += new System.EventHandler(this.CmbCountry_SelectedValueChanged);
            this.cmbCountry.Leave += new System.EventHandler(this.CmbCountry_Leave);

            //
            // lblCountryName
            //
            this.lblCountryName.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                         System.Windows.Forms.AnchorStyles.Left |
                                         System.Windows.Forms.AnchorStyles.Right;
            this.lblCountryName.BackColor = System.Drawing.Color.Transparent;
            this.lblCountryName.Font =
                new System.Drawing.Font("Verdana", 7.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (Byte)0);
            this.lblCountryName.Location = new System.Drawing.Point(58, 4);
            this.lblCountryName.Name = "lblCountryName";
            this.lblCountryName.Size = new System.Drawing.Size(180, 15);
            this.lblCountryName.TabIndex = 86;
            this.lblCountryName.Text = "Country Name";

            //
            // TUC_CountryLabelledComboBox
            //
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.lblCountryName);
            this.Controls.Add(this.cmbCountry);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (Byte)0);
            this.Name = "TUC_CountryLabelledComboBox";
            this.Size = new System.Drawing.Size(242, 22);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ComboBox cmbCountry;
        private System.Windows.Forms.Label lblCountryName;
    }
}