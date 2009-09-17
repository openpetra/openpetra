/* auto generated with nant generateWinforms from MainWindowNew.yaml
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

namespace Ict.Petra.Client.App.PetraClient
{
    partial class TFrmMainWindowNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmMainWindowNew));

            this.dsbContent = new Ict.Common.Controls.TDashboard();
            this.lstFolders = new Ict.Common.Controls.TLstFolderNavigation();
            this.stbMain = new Ict.Common.Controls.TExtStatusBarHelp();

            this.stbMain.SuspendLayout();
            this.dsbContent.SuspendLayout();
            this.lstFolders.SuspendLayout();
        	this.SuspendLayout();

            //
            // stbMain
            //
            this.stbMain.Name = "stbMain";
            this.stbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.stbMain.AutoSize = true;
        	//
        	// lstFolders
        	//
        	this.lstFolders.Dock = System.Windows.Forms.DockStyle.Left;
        	this.lstFolders.Location = new System.Drawing.Point(0, 0);
        	this.lstFolders.Name = "lstFolders";
        	this.lstFolders.Size = new System.Drawing.Size(200, 466);
        	this.lstFolders.TabIndex = 0;
        	//
        	// dsbContent
        	//
        	this.dsbContent.Dock = System.Windows.Forms.DockStyle.Fill;
        	this.dsbContent.Name = "dsbContent";

            //
            // TFrmMainWindowNew
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.Controls.Add(dsbContent);
        	this.Controls.Add(this.lstFolders);
            this.Controls.Add(this.stbMain);
            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            this.Name = "TFrmMainWindowNew";
            this.Text = "";

            this.stbMain.ResumeLayout(false);
        	this.lstFolders.ResumeLayout(false);
            this.dsbContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Ict.Common.Controls.TLstFolderNavigation lstFolders;
        private Ict.Common.Controls.TDashboard dsbContent;
        private Ict.Common.Controls.TExtStatusBarHelp stbMain;
    }
}