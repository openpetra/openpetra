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
    partial class TUC_CollapsibleSearchPane
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
            this.pnlCommonBackGround = new System.Windows.Forms.Panel();
            this.pnlButtonPanel = new System.Windows.Forms.Panel();
            this.btnResetSearch = new System.Windows.Forms.Button();
            this.btnInitiateSearch = new System.Windows.Forms.Button();
            this.pnlPanelLower = new System.Windows.Forms.Panel();
            this.pnlPanelMiddle = new System.Windows.Forms.Panel();
            this.pnlPanelUpper = new System.Windows.Forms.Panel();
            this.pnlCommonBackGround.SuspendLayout();
            this.pnlButtonPanel.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlCommonBackGround
            //
            this.pnlCommonBackGround.AccessibleDescription = " ";
            this.pnlCommonBackGround.BackColor = System.Drawing.SystemColors.Control;
            this.pnlCommonBackGround.Controls.Add(this.pnlButtonPanel);
            this.pnlCommonBackGround.Controls.Add(this.pnlPanelLower);
            this.pnlCommonBackGround.Controls.Add(this.pnlPanelMiddle);
            this.pnlCommonBackGround.Controls.Add(this.pnlPanelUpper);
            this.pnlCommonBackGround.Location = new System.Drawing.Point(5, 20);
            this.pnlCommonBackGround.Name = "pnlCommonBackGround";
            this.pnlCommonBackGround.Size = new System.Drawing.Size(440, 117);
            this.pnlCommonBackGround.TabIndex = 3;

            //
            // pnlButtonPanel
            //
            this.pnlButtonPanel.Controls.Add(this.btnResetSearch);
            this.pnlButtonPanel.Controls.Add(this.btnInitiateSearch);
            this.pnlButtonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlButtonPanel.Location = new System.Drawing.Point(0, 69);
            this.pnlButtonPanel.Name = "pnlButtonPanel";
            this.pnlButtonPanel.Size = new System.Drawing.Size(440, 23);
            this.pnlButtonPanel.TabIndex = 3;
            this.pnlButtonPanel.Resize += new System.EventHandler(this.PnlButtonPanel_Resize);

            //
            // btnResetSearch
            //
            this.btnResetSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnResetSearch.Location = new System.Drawing.Point(340, 0);
            this.btnResetSearch.Name = "btnResetSearch";
            this.btnResetSearch.Size = new System.Drawing.Size(100, 23);
            this.btnResetSearch.TabIndex = 1;
            this.btnResetSearch.Text = "Reset Search";
            this.btnResetSearch.Click += new System.EventHandler(this.BtnResetSearch_Click);

            //
            // btnInitiateSearch
            //
            this.btnInitiateSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnInitiateSearch.Location = new System.Drawing.Point(0, 0);
            this.btnInitiateSearch.Name = "btnInitiateSearch";
            this.btnInitiateSearch.Size = new System.Drawing.Size(100, 23);
            this.btnInitiateSearch.TabIndex = 0;
            this.btnInitiateSearch.Text = "&Search";
            this.btnInitiateSearch.Click += new System.EventHandler(this.BtnInitiateSearch_Click);

            //
            // pnlPanelLower
            //
            this.pnlPanelLower.BackColor = System.Drawing.SystemColors.Control;
            this.pnlPanelLower.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPanelLower.Location = new System.Drawing.Point(0, 46);
            this.pnlPanelLower.Name = "pnlPanelLower";
            this.pnlPanelLower.Size = new System.Drawing.Size(440, 23);
            this.pnlPanelLower.TabIndex = 2;
            this.pnlPanelLower.Resize += new System.EventHandler(this.PnlPanelLower_Resize);

            //
            // pnlPanelMiddle
            //
            this.pnlPanelMiddle.BackColor = System.Drawing.SystemColors.Control;
            this.pnlPanelMiddle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPanelMiddle.Location = new System.Drawing.Point(0, 23);
            this.pnlPanelMiddle.Name = "pnlPanelMiddle";
            this.pnlPanelMiddle.Size = new System.Drawing.Size(440, 23);
            this.pnlPanelMiddle.TabIndex = 1;
            this.pnlPanelMiddle.Resize += new System.EventHandler(this.PnlPanelMiddle_Resize);

            //
            // pnlPanelUpper
            //
            this.pnlPanelUpper.BackColor = System.Drawing.SystemColors.Control;
            this.pnlPanelUpper.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPanelUpper.Location = new System.Drawing.Point(0, 0);
            this.pnlPanelUpper.Name = "pnlPanelUpper";
            this.pnlPanelUpper.Size = new System.Drawing.Size(440, 23);
            this.pnlPanelUpper.TabIndex = 0;
            this.pnlPanelUpper.Resize += new System.EventHandler(this.PnlPanelUpper_Resize);

            //
            // TUC_CollapsibleSearchPane
            //
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Caption = "<Table 2 Maintain> ";
            this.Controls.Add(this.pnlCommonBackGround);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_CollapsibleSearchPane";
            this.Size = new System.Drawing.Size(451, 152);
// TODO           this.SubCaption = "Find";
            this.CollapsingEvent += new CollapsingEventHandler(this.TUC_CollapsibleSearchPane_CollapsingEvent);
            this.Controls.SetChildIndex(this.pnlCommonBackGround, 0);
            this.pnlCommonBackGround.ResumeLayout(false);
            this.pnlButtonPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlCommonBackGround;
        private System.Windows.Forms.Panel pnlPanelUpper;
        private System.Windows.Forms.Panel pnlPanelMiddle;
        private System.Windows.Forms.Panel pnlPanelLower;
        private System.Windows.Forms.Panel pnlButtonPanel;
        private System.Windows.Forms.Button btnInitiateSearch;
        private System.Windows.Forms.Button btnResetSearch;
        private System.Windows.Forms.Label lblPanelUpper;
        private System.Windows.Forms.Label lblPanelMiddle;
        private System.Windows.Forms.Label lblPanelLower;
        private System.Windows.Forms.TextBox txtPanelMiddle;
        private System.Windows.Forms.TextBox txtPanelLower;
    }
}