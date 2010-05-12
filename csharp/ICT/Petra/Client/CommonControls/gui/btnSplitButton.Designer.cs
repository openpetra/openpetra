//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timh
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
    partial class SplitButton
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
        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            this.SplitButtonBase1 = new System.Windows.Forms.Button();
            this.mnuMatchStyle = new System.Windows.Forms.ContextMenu();
            this.mnuMatchStartsWith = new System.Windows.Forms.MenuItem();
            this.mnuMatchEndsWith = new System.Windows.Forms.MenuItem();
            this.mnuMatchContains = new System.Windows.Forms.MenuItem();
            this.mnuMatchExact = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();

            //
            // SplitButtonBase1
            //
//TODO            this.SplitButtonBase1.AlwaysDropDown = true;
//            this.SplitButtonBase1.CalculateSplitRect = false;
            this.SplitButtonBase1.Dock = System.Windows.Forms.DockStyle.Fill;

//            this.SplitButtonBase1.HoverLuminosity = 10;
            this.SplitButtonBase1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SplitButtonBase1.ImageIndex = 0;
            this.SplitButtonBase1.Location = new System.Drawing.Point(0, 0);
            this.SplitButtonBase1.Name = "SplitButtonBase1";
            this.SplitButtonBase1.Size = new System.Drawing.Size(282, 76);

//            this.SplitButtonBase1.SplitHeight = 76;
//            this.SplitButtonBase1.SplitWidth = 12;
            this.SplitButtonBase1.TabIndex = 0;
            this.SplitButtonBase1.TabStop = false;
            this.SplitButtonBase1.Text = "SplitButtonBase1";
            this.SplitButtonBase1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // mnuMatchStyle
            //
            this.mnuMatchStyle.MenuItems.AddRange(new MenuItem[] { this.mnuMatchStartsWith, this.mnuMatchEndsWith, this.mnuMatchContains,
                                                                   this.mnuMatchExact });

            //
            // mnuMatchStartsWith
            //
            this.mnuMatchStartsWith.Index = 0;
            this.mnuMatchStartsWith.Text = "Starts with search term --*";
            this.mnuMatchStartsWith.Click += new System.EventHandler(MnuMatchStartsWith_Click);

            //
            // mnuMatchEndsWith
            //
            this.mnuMatchEndsWith.Index = 1;
            this.mnuMatchEndsWith.Text = "Ends with search term *--";
            this.mnuMatchEndsWith.Click += new System.EventHandler(MnuMatchEndsWith_Click);

            //
            // mnuMatchContains
            //
            this.mnuMatchContains.Index = 2;
            this.mnuMatchContains.Text = "Contains search term *-*";
            this.mnuMatchContains.Click += new System.EventHandler(this.MnuMatchContains_Click);

            //
            // mnuMatchExact
            //
            this.mnuMatchExact.Index = 3;
            this.mnuMatchExact.Text = "Exactly matches search term ---";
            this.mnuMatchExact.Click += new System.EventHandler(this.MnuMatchExact_Click);

            //
            // SplitButton
            //
            this.Controls.Add(this.SplitButtonBase1);
            this.Name = "SplitButton";
            this.Size = new System.Drawing.Size(282, 76);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button SplitButtonBase1;
        private System.Windows.Forms.ContextMenu mnuMatchStyle;
        private System.Windows.Forms.MenuItem mnuMatchStartsWith;
        private System.Windows.Forms.MenuItem mnuMatchEndsWith;
        private System.Windows.Forms.MenuItem mnuMatchContains;
        private System.Windows.Forms.MenuItem mnuMatchExact;
    }
}