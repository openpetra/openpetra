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
    partial class TUCOfficeSpecificDataLabels
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.grdOfficeSpecificGrid = new SourceGrid.Grid();

// TODO            this.sbtUCOfficeSpecificDataLabels = new EWSoftware.StatusBarText.StatusBarTextProvider(this.components);
            this.SuspendLayout();

            //
            // lblInfo
            //
            this.lblInfo.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.lblInfo.Location = new System.Drawing.Point(6, 6);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(196, 14);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Office Specific Data Labels";

            //
            // grdOfficeSpecificGrid
            //
            this.grdOfficeSpecificGrid.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grdOfficeSpecificGrid.Location = new System.Drawing.Point(0, 0);
            this.grdOfficeSpecificGrid.Name = "grdOfficeSpecificGrid";
            this.grdOfficeSpecificGrid.Size = new System.Drawing.Size(512, 386);
            this.grdOfficeSpecificGrid.SpecialKeys = SourceGrid.GridSpecialKeys.Default;
            this.grdOfficeSpecificGrid.TabIndex = 0;

            //
            // TUCOfficeSpecificDataLabels
            //
            this.Controls.Add(this.grdOfficeSpecificGrid);
            this.Controls.Add(this.lblInfo);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUCOfficeSpecificDataLabels";
            this.Size = new System.Drawing.Size(526, 400);
            this.SizeChanged += new System.EventHandler(this.GrdOfficeSpecificGrid_SizeChanged);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblInfo;
        private SourceGrid.Grid grdOfficeSpecificGrid;
    }
}