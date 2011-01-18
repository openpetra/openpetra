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

namespace Ict.Petra.Client.MCommon.Gui
{
    partial class TUC_EditButtonPane
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUC_EditButtonPane));
            this.components = new  System.ComponentModel.Container();
            this.imlButtonIcons = new System.Windows.Forms.ImageList(this.components);
            this.btnResizeButton = new  System.Windows.Forms.Button();
            this.btnLowerButton = new  System.Windows.Forms.Button();
            this.btnMiddleButton = new  System.Windows.Forms.Button();
            this.btnUpperButton = new  System.Windows.Forms.Button();
            this.SuspendLayout();

            //
            // imlButtonIcons
            //
            this.imlButtonIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imlButtonIcons.ImageStream = (System.Windows.Forms.ImageListStreamer)(resources.GetObject('i' + "mlButtonIcons.ImageStream"));
            this.imlButtonIcons.TransparentColor = System.Drawing.Color.Transparent;

            //
            // btnResizeButton
            //
            this.btnResizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResizeButton.ImageIndex = 6;
            this.btnResizeButton.ImageList = this.imlButtonIcons;
            this.btnResizeButton.Location = new System.Drawing.Point(0, 77);
            this.btnResizeButton.Name = "btnResizeButton";
            this.btnResizeButton.Size = new System.Drawing.Size(20, 18);
            this.btnResizeButton.TabIndex = 3;
            this.btnResizeButton.Text = "Resize Button";
            this.btnResizeButton.Click += new System.EventHandler(this.BtnResizeButton_Click);
            this.btnResizeButton.Enter += new System.EventHandler(this.BtnResizeButton_Enter);
            this.btnResizeButton.Leave += new System.EventHandler(this.BtnResizeButton_Leave);

            //
            // btnLowerButton
            //
            this.btnLowerButton.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnLowerButton.ImageList = this.imlButtonIcons;
            this.btnLowerButton.Location = new System.Drawing.Point(4, 48);
            this.btnLowerButton.Name = "btnLowerButton";
            this.btnLowerButton.Size = new System.Drawing.Size(76, 23);
            this.btnLowerButton.TabIndex = 2;
            this.btnLowerButton.Text = "    LowerButton";
            this.btnLowerButton.Click += new System.EventHandler(this.BtnLowerButton_Click);
            this.btnLowerButton.Enter += new System.EventHandler(this.BtnLowerButton_Enter);
            this.btnLowerButton.Leave += new System.EventHandler(this.BtnLowerButton_Leave);

            //
            // btnMiddleButton
            //
            this.btnMiddleButton.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnMiddleButton.ImageList = this.imlButtonIcons;
            this.btnMiddleButton.Location = new System.Drawing.Point(4, 24);
            this.btnMiddleButton.Name = "btnMiddleButton";
            this.btnMiddleButton.Size = new System.Drawing.Size(76, 23);
            this.btnMiddleButton.TabIndex = 1;
            this.btnMiddleButton.Text = "       MiddleButton";
            this.btnMiddleButton.Click += new System.EventHandler(this.BtnMiddleButton_Click);
            this.btnMiddleButton.Enter += new System.EventHandler(this.BtnMiddleButton_Enter);
            this.btnMiddleButton.Leave += new System.EventHandler(this.BtnMiddleButton_Leave);

            //
            // btnUpperButton
            //
            this.btnUpperButton.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnUpperButton.ImageList = this.imlButtonIcons;
            this.btnUpperButton.Location = new System.Drawing.Point(4, 0);
            this.btnUpperButton.Name = "btnUpperButton";
            this.btnUpperButton.Size = new System.Drawing.Size(76, 23);
            this.btnUpperButton.TabIndex = 0;
            this.btnUpperButton.Text = "        UpperButton";
            this.btnUpperButton.Click += new System.EventHandler(this.BtnUpperButton_Click);
            this.btnUpperButton.Enter += new System.EventHandler(this.BtnUpperButton_Enter);
            this.btnUpperButton.Leave += new System.EventHandler(this.BtnUpperButton_Leave);

            //
            // TUC_EditButtonPane
            //
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.btnResizeButton);
            this.Controls.Add(this.btnLowerButton);
            this.Controls.Add(this.btnMiddleButton);
            this.Controls.Add(this.btnUpperButton);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_EditButtonPane";
            this.Size = new System.Drawing.Size(80, 123);
            this.Enter += new System.EventHandler(this.TUC_EditButtonPane_Enter);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ImageList imlButtonIcons;
        private System.Windows.Forms.Button btnResizeButton;
        private System.Windows.Forms.Button btnLowerButton;
        private System.Windows.Forms.Button btnMiddleButton;
        private System.Windows.Forms.Button btnUpperButton;
    }
}