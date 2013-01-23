//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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

namespace Ict.Common.Controls
{
    partial class TLstFolderNavigation
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
            new System.ComponentModel.ComponentResourceManager(typeof(TRbtNavigationButton));

            this.sptNavigation = new System.Windows.Forms.SplitContainer();
            this.pnlMoreButtons = new TPnlGradient();
            this.sptNavigation.Panel1.SuspendLayout();
            this.sptNavigation.Panel2.SuspendLayout();
            this.sptNavigation.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlMoreButtons
            //
            this.pnlMoreButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlMoreButtons.Location = new System.Drawing.Point(0, 438);
            this.pnlMoreButtons.Name = "pnlMoreButtons";
            this.pnlMoreButtons.Size = new System.Drawing.Size(200, 28);
            this.pnlMoreButtons.TabIndex = 2;

            //
            // sptNavigation
            //
            this.sptNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sptNavigation.Location = new System.Drawing.Point(0, 42);
            this.sptNavigation.Name = "sptNavigation";
            this.sptNavigation.Orientation = System.Windows.Forms.Orientation.Horizontal;

            //
            // sptNavigation.Panel1
            //
            this.sptNavigation.Panel1.AutoScroll = true;

            //
            // sptNavigation.Panel2
            //
            this.sptNavigation.Size = new System.Drawing.Size(200, 396);
            this.sptNavigation.SplitterDistance = 210;
            this.sptNavigation.TabIndex = 6;
            this.sptNavigation.Panel1.BackColor = sptNavigation.BackColor;
            this.sptNavigation.Panel2.BackColor = sptNavigation.BackColor;
            this.sptNavigation.BackColor = System.Drawing.Color.DarkGray;
            this.sptNavigation.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.SptNavigationSplitterMoved);
            this.sptNavigation.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.SptNavigationSplitterMoving);

            //
            // TLstFolderNavigation
            //
            this.Controls.Add(this.sptNavigation);
            this.Controls.Add(this.pnlMoreButtons);
            this.DoubleBuffered = true;
            this.sptNavigation.Panel1.ResumeLayout(false);
            this.sptNavigation.Panel2.ResumeLayout(false);
            this.sptNavigation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.SplitContainer sptNavigation;
        private TPnlGradient pnlMoreButtons;
    }
}