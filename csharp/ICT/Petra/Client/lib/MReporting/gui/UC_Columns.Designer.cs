/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
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
using Ict.Common.Controls;

namespace Ict.Petra.Client.MReporting.Gui
{
    partial class UC_Columns
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        /// <summary>
        /// Disposes resources used by the control.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_Columns));
            
            this.grdColumns = new Ict.Common.Controls.TSgrdDataGrid();
            this.Btn_MoveColumn2Right = new System.Windows.Forms.Button();
            this.ImL_MoveRight = new System.Windows.Forms.ImageList(this.components);
            this.Btn_MoveColumn2Left = new System.Windows.Forms.Button();
            this.ImL_MoveLeft = new System.Windows.Forms.ImageList(this.components);
            this.GBx_ChooseColCont = new System.Windows.Forms.GroupBox();
            this.Btn_Cancel = new System.Windows.Forms.Button();
            this.BtnApply = new System.Windows.Forms.Button();
            this.Btn_RemoveColumn = new System.Windows.Forms.Button();
            this.Btn_AddColumn = new System.Windows.Forms.Button();

            // 
            // grdColumns
            // 
            this.grdColumns.AlternatingBackgroundColour = System.Drawing.SystemColors.InactiveBorder;
            this.grdColumns.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.grdColumns.AutoFindColumn = ((short)(-1));
            this.grdColumns.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.NoAutoFind;
            this.grdColumns.AutoStretchColumnsToFitWidth = true;
            this.grdColumns.BackColor = System.Drawing.SystemColors.ControlDark;
            this.grdColumns.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdColumns.DeleteQuestionMessage = "You have chosen to delete this record.\'#13#10#13#10\'Do you really want to delete " +
            "it?";
            this.grdColumns.FixedRows = 1;
            this.grdColumns.KeepRowSelectedAfterSort = true;
            this.grdColumns.Location = new System.Drawing.Point(10, 8);
            this.grdColumns.MinimumHeight = 19;
            this.grdColumns.Name = "grdColumns";
            this.grdColumns.Size = new System.Drawing.Size(60, 120);
            this.grdColumns.SortableHeaders = true;
            this.grdColumns.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows | SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) |
                                               SourceGrid.GridSpecialKeys.Shift)));
            this.grdColumns.TabIndex = 27;
            this.grdColumns.TabStop = true;
            this.grdColumns.ToolTipTextDelegate = null;

            // 
            // Btn_MoveColumn2Right
            // 
            this.Btn_MoveColumn2Right.AccessibleDescription = "";
            this.Btn_MoveColumn2Right.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_MoveColumn2Right.Enabled = false;
            this.Btn_MoveColumn2Right.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn_MoveColumn2Right.ImageIndex = 0;
            this.Btn_MoveColumn2Right.ImageList = this.ImL_MoveRight;
            this.Btn_MoveColumn2Right.Location = new System.Drawing.Point(192, 43);
            this.Btn_MoveColumn2Right.Name = "Btn_MoveColumn2Right";
            this.Btn_MoveColumn2Right.Size = new System.Drawing.Size(29, 26);
            this.Btn_MoveColumn2Right.TabIndex = 26;
            this.Btn_MoveColumn2Right.Click += new System.EventHandler(this.Btn_MoveColumn2Right_Click);

            // 
            // ImL_MoveRight
            // 
            this.ImL_MoveRight.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImL_MoveRight.ImageStream")));
            this.ImL_MoveRight.TransparentColor = System.Drawing.Color.Transparent;
            this.ImL_MoveRight.Images.SetKeyName(0, "");
            this.ImL_MoveRight.Images.SetKeyName(1, "");

            // 
            // Btn_MoveColumn2Left
            // 
            this.Btn_MoveColumn2Left.AccessibleDescription = "";
            this.Btn_MoveColumn2Left.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_MoveColumn2Left.Enabled = false;
            this.Btn_MoveColumn2Left.ImageIndex = 0;
            this.Btn_MoveColumn2Left.ImageList = this.ImL_MoveLeft;
            this.Btn_MoveColumn2Left.Location = new System.Drawing.Point(192, 9);
            this.Btn_MoveColumn2Left.Name = "Btn_MoveColumn2Left";
            this.Btn_MoveColumn2Left.Size = new System.Drawing.Size(29, 25);
            this.Btn_MoveColumn2Left.TabIndex = 25;
            this.Btn_MoveColumn2Left.Click += new System.EventHandler(this.Btn_MoveColumn2Left_Click);

            // 
            // ImL_MoveLeft
            // 
            this.ImL_MoveLeft.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImL_MoveLeft.ImageStream")));
            this.ImL_MoveLeft.TransparentColor = System.Drawing.Color.Transparent;
            this.ImL_MoveLeft.Images.SetKeyName(0, "");
            this.ImL_MoveLeft.Images.SetKeyName(1, "");

            // 
            // GBx_ChooseColCont
            // 
            this.GBx_ChooseColCont.Controls.Add(this.Btn_Cancel);
            this.GBx_ChooseColCont.Controls.Add(this.BtnApply);
            this.GBx_ChooseColCont.Location = new System.Drawing.Point(10, 181);
            this.GBx_ChooseColCont.Name = "GBx_ChooseColCont";
            this.GBx_ChooseColCont.Size = new System.Drawing.Size(624, 189);
            this.GBx_ChooseColCont.TabIndex = 22;
            this.GBx_ChooseColCont.TabStop = false;
            this.GBx_ChooseColCont.Text = "Define Column";

            // 
            // Btn_Cancel
            // 
            this.Btn_Cancel.Location = new System.Drawing.Point(288, 155);
            this.Btn_Cancel.Name = "Btn_Cancel";
            this.Btn_Cancel.Size = new System.Drawing.Size(90, 25);
            this.Btn_Cancel.TabIndex = 10;
            this.Btn_Cancel.Text = "Cancel";
            this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);

            // 
            // BtnApply
            // 
            this.BtnApply.Location = new System.Drawing.Point(394, 155);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(90, 25);
            this.BtnApply.TabIndex = 9;
            this.BtnApply.Text = "Apply";
            this.BtnApply.Click += new System.EventHandler(this.BtnApply_Click);

            // 
            // Btn_RemoveColumn
            // 
            this.Btn_RemoveColumn.Enabled = false;
            this.Btn_RemoveColumn.Location = new System.Drawing.Point(115, 146);
            this.Btn_RemoveColumn.Name = "Btn_RemoveColumn";
            this.Btn_RemoveColumn.Size = new System.Drawing.Size(77, 26);
            this.Btn_RemoveColumn.TabIndex = 20;
            this.Btn_RemoveColumn.Text = "&Remove";
            this.Btn_RemoveColumn.Click += new System.EventHandler(this.Btn_RemoveColumn_Click);

            // 
            // Btn_AddColumn
            // 
            this.Btn_AddColumn.Location = new System.Drawing.Point(10, 146);
            this.Btn_AddColumn.Name = "Btn_AddColumn";
            this.Btn_AddColumn.Size = new System.Drawing.Size(76, 26);
            this.Btn_AddColumn.TabIndex = 18;
            this.Btn_AddColumn.Text = "&Add";
            this.Btn_AddColumn.Click += new System.EventHandler(this.Btn_AddColumn_Click);
            
            // 
            // UC_Columns
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UC_Columns";
            this.Controls.Add(this.grdColumns);
            this.Controls.Add(this.Btn_MoveColumn2Right);
            this.Controls.Add(this.Btn_MoveColumn2Left);
            this.Controls.Add(this.GBx_ChooseColCont);
            this.Controls.Add(this.Btn_RemoveColumn);
            this.Controls.Add(this.Btn_AddColumn);
            this.Size = new System.Drawing.Size(650, 386);
            this.ResumeLayout(false);
        }
        private TSgrdDataGrid grdColumns;
        private System.Windows.Forms.Button Btn_Cancel;
        private System.Windows.Forms.GroupBox GBx_ChooseColCont;
        private System.Windows.Forms.Button BtnApply;
        private System.Windows.Forms.Button Btn_RemoveColumn;
        private System.Windows.Forms.Button Btn_AddColumn;
        private System.Windows.Forms.ImageList ImL_MoveRight;
        private System.Windows.Forms.ImageList ImL_MoveLeft;
        private System.Windows.Forms.Button Btn_MoveColumn2Right;
        private System.Windows.Forms.Button Btn_MoveColumn2Left;
   }
}
