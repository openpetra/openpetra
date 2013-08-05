//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
    partial class TUC_MultiMotivationDetailSelection
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
            this.clbMotivations = new TClbVersatile();
            this.SuspendLayout();

            //
            // clbMotivations
            //
            this.clbMotivations.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                         System.Windows.Forms.AnchorStyles.Bottom |
                                         System.Windows.Forms.AnchorStyles.Left |
                                         System.Windows.Forms.AnchorStyles.Right;
            this.clbMotivations.BackColor = System.Drawing.SystemColors.ControlDark;
            this.clbMotivations.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.clbMotivations.DeleteQuestionMessage = "You have chosen to delete t" + "his record.'#13#10#13#10'Do you really want to delete it?";
            this.clbMotivations.FixedRows = 1;
            this.clbMotivations.Location = new System.Drawing.Point(192, 103);
            this.clbMotivations.MinimumHeight = 19;
            this.clbMotivations.Name = "clbMotivations";
            this.clbMotivations.Size = new System.Drawing.Size(337, 119);
            this.clbMotivations.SpecialKeys = SourceGrid.GridSpecialKeys.Arrows |
                                              SourceGrid.GridSpecialKeys.PageDownUp |
                                              SourceGrid.GridSpecialKeys.Enter |
                                              SourceGrid.GridSpecialKeys.Escape |
                                              SourceGrid.GridSpecialKeys.Control |
                                              SourceGrid.GridSpecialKeys.Shift;
            this.clbMotivations.TabIndex = 4;
            this.clbMotivations.TabStop = true;

            //
            // TUC_MultiMotivationDetailSelection
            //
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.clbMotivations);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (Byte)0);
            this.Name = "TUC_MultiMotivationDetailSelection";
            this.Size = new System.Drawing.Size(242, 22);
            this.ResumeLayout(false);
        }

        private TClbVersatile clbMotivations;
    }
}