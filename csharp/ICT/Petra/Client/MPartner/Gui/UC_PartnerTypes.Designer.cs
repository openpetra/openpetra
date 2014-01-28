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
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TUCPartnerTypes
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
            this.components = new System.ComponentModel.Container();
            this.grdPartnerTypes = new Ict.Common.Controls.TSgrdDataGrid();
            this.SuspendLayout();

            //
            // grdPartnerTypes
            //
            this.grdPartnerTypes.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grdPartnerTypes.AutoFindColumn = ((Int16)(1));
            this.grdPartnerTypes.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;
            this.grdPartnerTypes.BackColor = System.Drawing.SystemColors.ControlDark;
            this.grdPartnerTypes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdPartnerTypes.DeleteQuestionMessage = "You have chosen to delete t" + "his record.'#13#10#13#10'Dou you really want to delete it?";
            this.grdPartnerTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdPartnerTypes.FixedRows = 1;
            this.grdPartnerTypes.Location = new System.Drawing.Point(4, 6);
            this.grdPartnerTypes.MinimumHeight = 1;
            this.grdPartnerTypes.Name = "grdPartnerTypes";
            this.grdPartnerTypes.Size = new System.Drawing.Size(410, 316);
            this.grdPartnerTypes.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));
            this.grdPartnerTypes.TabIndex = 0;
            this.grdPartnerTypes.TabStop = true;
            this.grdPartnerTypes.MouseClick += new MouseEventHandler(this.GrdPartnerTypes_Click);
            this.grdPartnerTypes.SpaceKeyPressed += new TKeyPressedEventHandler(this.GrdPartnerTypes_SpaceKeyPressed);
            this.grdPartnerTypes.EnterKeyPressed += new TKeyPressedEventHandler(this.GrdPartnerTypes_EnterKeyPressed);

            //
            // TUCPartnerTypes
            //
            this.Controls.Add(this.grdPartnerTypes);
            this.Name = "TUCPartnerTypes";
            this.Size = new System.Drawing.Size(574, 336);
            this.ResumeLayout(false);
        }

        private TSgrdDataGrid grdPartnerTypes;
    }
}