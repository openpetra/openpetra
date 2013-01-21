//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
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
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MCommon.Gui
{
    partial class TUC_LocalDataLabelValues
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

                // for logic static objects
                SpecialDispose(disposing);
            }
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grdLocalDataLabelValues = new SourceGrid.Grid();
            this.SuspendLayout();

            //
            // grdOfficeSpecificGrid
            //
            this.grdLocalDataLabelValues.Location = new System.Drawing.Point(0, 0);
            this.grdLocalDataLabelValues.Name = "grdLocalDataLabelValues";
            //this.grdLocalDataLabelValues.Size = new System.Drawing.Size(512, 386);
            this.grdLocalDataLabelValues.SpecialKeys = SourceGrid.GridSpecialKeys.Default;
            this.grdLocalDataLabelValues.TabIndex = 0;
            this.grdLocalDataLabelValues.Dock = System.Windows.Forms.DockStyle.Fill;


            //
            // TUCOfficeSpecificDataLabels
            //
            this.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.Controls.Add(this.grdLocalDataLabelValues);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_LocalDataLabelValues";
            //this.Size = new System.Drawing.Size(526, 400);
            this.SizeChanged += new System.EventHandler(this.GrdLocalDataLabelValues_SizeChanged);
            this.ResumeLayout(false);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        private SourceGrid.Grid grdLocalDataLabelValues;
    }
}