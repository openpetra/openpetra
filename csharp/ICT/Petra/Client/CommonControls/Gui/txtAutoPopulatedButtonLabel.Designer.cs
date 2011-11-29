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
    partial class TtxtAutoPopulatedButtonLabel
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
            this.txtAutoPopulated = new TTxtButtonLabel();
            this.timerGetKey = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();

            //
            // txtAutoPopulated
            //
            this.txtAutoPopulated.Anchor = System.Windows.Forms.AnchorStyles.Top |
                                           System.Windows.Forms.AnchorStyles.Left |
                                           System.Windows.Forms.AnchorStyles.Right;
            this.txtAutoPopulated.ButtonText = "btnButton";
            this.txtAutoPopulated.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtAutoPopulated.ButtonWidth = 72;
            this.txtAutoPopulated.KeyValues = TKeyValuesEnum.OnlyDigits;
            this.txtAutoPopulated.LabelLookUpMember = "";
            this.txtAutoPopulated.LabelSeparatorWidth = 6;
            this.txtAutoPopulated.LabelText = "lblLabel";
            this.txtAutoPopulated.Location = new System.Drawing.Point(0, 0);
            this.txtAutoPopulated.LookUpDataSource = null;
            this.txtAutoPopulated.MaxLength = 32767;
            this.txtAutoPopulated.Name = "txtAutoPopulated";
            this.txtAutoPopulated.ReadOnly = false;
            this.txtAutoPopulated.SeparatorWidth = 4;
            this.txtAutoPopulated.ShowLabel = true;
            this.txtAutoPopulated.Size = new System.Drawing.Size(390, 23);
            this.txtAutoPopulated.TabIndex = 0;
            this.txtAutoPopulated.TextBoxLookUpMember = null;
            this.txtAutoPopulated.TextBoxText = "txtTextBox";
            this.txtAutoPopulated.TextBoxWidth = 100;
            this.txtAutoPopulated.ButtonClick += new TDelegateButtonClick(this.TxtAutoPopulated_ButtonClick);

            //
            // timerGetKey
            //
            this.timerGetKey.Interval = 100;
            this.timerGetKey.Tick += new System.EventHandler(this.TimerGetKey_Tick);

            //
            // TtxtAutoPopulatedButtonLabel
            //
            this.Controls.Add(this.txtAutoPopulated);
            this.Name = "TtxtAutoPopulatedButtonLabel";
            this.Size = new System.Drawing.Size(390, 23);
            this.ResumeLayout(false);
        }

        private TTxtButtonLabel txtAutoPopulated;
        private System.Windows.Forms.Timer timerGetKey;
    }
}