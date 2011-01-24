// auto generated with nant generateWinforms from UC_PartnerColumns.yaml
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MReporting.Gui
{
    partial class TFrmUC_PartnerColumns
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFrmUC_PartnerColumns));

            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlColumns = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.grdColumns = new Ict.Common.Controls.TSgrdDataGridPaged();
            this.btnDummy = new System.Windows.Forms.Button();
            this.pnlMoveColumn = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMoveColumn2Left = new System.Windows.Forms.Button();
            this.btnMoveColumn2Right = new System.Windows.Forms.Button();
            this.pnlAddRemButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddColumn = new System.Windows.Forms.Button();
            this.btnRemoveColumn = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.grpDefineColumn = new System.Windows.Forms.GroupBox();
            this.pnlColumnDefinition = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbCalculation = new Ict.Common.Controls.TCmbAutoComplete();
            this.lblCalculation = new System.Windows.Forms.Label();
            this.txtColumnWidth = new System.Windows.Forms.TextBox();
            this.lblColumnWidth = new System.Windows.Forms.Label();
            this.lblCm = new System.Windows.Forms.Label();
            this.pnlLowerButtons = new System.Windows.Forms.Panel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();

            this.pnlContent.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlColumns.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlMoveColumn.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlAddRemButtons.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.grpDefineColumn.SuspendLayout();
            this.pnlColumnDefinition.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.pnlLowerButtons.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();

            //
            // pnlContent
            //
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.AutoSize = true;
            this.pnlContent.Controls.Add(this.pnlBottom);
            this.pnlContent.Controls.Add(this.pnlTop);
            //
            // pnlTop
            //
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.AutoSize = true;
            this.pnlTop.Controls.Add(this.pnlColumns);
            this.pnlTop.Controls.Add(this.pnlAddRemButtons);
            //
            // pnlColumns
            //
            this.pnlColumns.Name = "pnlColumns";
            this.pnlColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlColumns.AutoSize = true;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.AutoSize = true;
            this.pnlColumns.Controls.Add(this.tableLayoutPanel1);
            //
            // pnlGrid
            //
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.AutoSize = true;
            //
            // tableLayoutPanel2
            //
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.AutoSize = true;
            this.pnlGrid.Controls.Add(this.tableLayoutPanel2);
            //
            // grdColumns
            //
            this.grdColumns.Name = "grdColumns";
            this.grdColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // btnDummy
            //
            this.btnDummy.Location = new System.Drawing.Point(2,2);
            this.btnDummy.Name = "btnDummy";
            this.btnDummy.Visible = false;
            this.btnDummy.AutoSize = true;
            this.btnDummy.Text = "DummyButton";
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Controls.Add(this.grdColumns, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDummy, 0, 1);
            //
            // pnlMoveColumn
            //
            this.pnlMoveColumn.Location = new System.Drawing.Point(2,2);
            this.pnlMoveColumn.Name = "pnlMoveColumn";
            this.pnlMoveColumn.AutoSize = true;
            //
            // tableLayoutPanel3
            //
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.AutoSize = true;
            this.pnlMoveColumn.Controls.Add(this.tableLayoutPanel3);
            //
            // btnMoveColumn2Left
            //
            this.btnMoveColumn2Left.Location = new System.Drawing.Point(2,2);
            this.btnMoveColumn2Left.Name = "btnMoveColumn2Left";
            this.btnMoveColumn2Left.AutoSize = true;
            this.btnMoveColumn2Left.Click += new System.EventHandler(this.MoveColumn2Left);
            this.btnMoveColumn2Left.Text = "Left:";
            //
            // btnMoveColumn2Right
            //
            this.btnMoveColumn2Right.Location = new System.Drawing.Point(2,2);
            this.btnMoveColumn2Right.Name = "btnMoveColumn2Right";
            this.btnMoveColumn2Right.AutoSize = true;
            this.btnMoveColumn2Right.Click += new System.EventHandler(this.MoveColumn2Right);
            this.btnMoveColumn2Right.Text = "Right";
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Controls.Add(this.btnMoveColumn2Left, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnMoveColumn2Right, 0, 1);
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Controls.Add(this.pnlGrid, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlMoveColumn, 1, 0);
            //
            // pnlAddRemButtons
            //
            this.pnlAddRemButtons.Name = "pnlAddRemButtons";
            this.pnlAddRemButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAddRemButtons.AutoSize = true;
            //
            // tableLayoutPanel4
            //
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.AutoSize = true;
            this.pnlAddRemButtons.Controls.Add(this.tableLayoutPanel4);
            //
            // btnAddColumn
            //
            this.btnAddColumn.Location = new System.Drawing.Point(2,2);
            this.btnAddColumn.Name = "btnAddColumn";
            this.btnAddColumn.AutoSize = true;
            this.btnAddColumn.Click += new System.EventHandler(this.AddColumn);
            this.btnAddColumn.Text = "Add";
            //
            // btnRemoveColumn
            //
            this.btnRemoveColumn.Location = new System.Drawing.Point(2,2);
            this.btnRemoveColumn.Name = "btnRemoveColumn";
            this.btnRemoveColumn.AutoSize = true;
            this.btnRemoveColumn.Click += new System.EventHandler(this.RemoveColumn);
            this.btnRemoveColumn.Text = "Remove";
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnAddColumn, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnRemoveColumn, 1, 0);
            //
            // pnlBottom
            //
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.AutoSize = true;
            this.pnlBottom.Controls.Add(this.grpDefineColumn);
            //
            // grpDefineColumn
            //
            this.grpDefineColumn.Name = "grpDefineColumn";
            this.grpDefineColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDefineColumn.AutoSize = true;
            this.grpDefineColumn.Controls.Add(this.pnlColumnDefinition);
            this.grpDefineColumn.Controls.Add(this.pnlLowerButtons);
            //
            // pnlColumnDefinition
            //
            this.pnlColumnDefinition.Name = "pnlColumnDefinition";
            this.pnlColumnDefinition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlColumnDefinition.AutoSize = true;
            //
            // tableLayoutPanel5
            //
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.AutoSize = true;
            this.pnlColumnDefinition.Controls.Add(this.tableLayoutPanel5);
            //
            // cmbCalculation
            //
            this.cmbCalculation.Location = new System.Drawing.Point(2,2);
            this.cmbCalculation.Name = "cmbCalculation";
            this.cmbCalculation.Size = new System.Drawing.Size(150, 28);
            this.cmbCalculation.SelectedValueChanged += new System.EventHandler(this.CmbContentChanged);
            //
            // lblCalculation
            //
            this.lblCalculation.Location = new System.Drawing.Point(2,2);
            this.lblCalculation.Name = "lblCalculation";
            this.lblCalculation.AutoSize = true;
            this.lblCalculation.Text = "Content of Column::";
            this.lblCalculation.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblCalculation.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblCalculation.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // txtColumnWidth
            //
            this.txtColumnWidth.Location = new System.Drawing.Point(2,2);
            this.txtColumnWidth.Name = "txtColumnWidth";
            this.txtColumnWidth.Size = new System.Drawing.Size(150, 28);
            //
            // lblColumnWidth
            //
            this.lblColumnWidth.Location = new System.Drawing.Point(2,2);
            this.lblColumnWidth.Name = "lblColumnWidth";
            this.lblColumnWidth.AutoSize = true;
            this.lblColumnWidth.Text = "Width of Column::";
            this.lblColumnWidth.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.lblColumnWidth.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblColumnWidth.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // lblCm
            //
            this.lblCm.Location = new System.Drawing.Point(2,2);
            this.lblCm.Name = "lblCm";
            this.lblCm.AutoSize = true;
            this.lblCm.Text = "cm:";
            this.lblCm.Margin = new System.Windows.Forms.Padding(3, 7, 3, 0);
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Controls.Add(this.lblCalculation, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblColumnWidth, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.cmbCalculation, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.txtColumnWidth, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.lblCm, 2, 1);
            //
            // pnlLowerButtons
            //
            this.pnlLowerButtons.Name = "pnlLowerButtons";
            this.pnlLowerButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLowerButtons.AutoSize = true;
            //
            // tableLayoutPanel6
            //
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.AutoSize = true;
            this.pnlLowerButtons.Controls.Add(this.tableLayoutPanel6);
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(2,2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.AutoSize = true;
            this.btnCancel.Click += new System.EventHandler(this.CancelColumn);
            this.btnCancel.Text = "&Cancel";
            //
            // btnApply
            //
            this.btnApply.Location = new System.Drawing.Point(2,2);
            this.btnApply.Name = "btnApply";
            this.btnApply.AutoSize = true;
            this.btnApply.Click += new System.EventHandler(this.ApplyColumn);
            this.btnApply.Text = "A&pply";
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Controls.Add(this.btnCancel, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnApply, 1, 0);
            this.grpDefineColumn.Text = "Define Column";

            //
            // TFrmUC_PartnerColumns
            //
            this.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(this.pnlContent);

            this.Name = "TFrmUC_PartnerColumns";
            this.Text = "";

            this.tableLayoutPanel6.ResumeLayout(false);
            this.pnlLowerButtons.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.pnlColumnDefinition.ResumeLayout(false);
            this.grpDefineColumn.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.pnlAddRemButtons.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlMoveColumn.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlColumns.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlColumns;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Ict.Common.Controls.TSgrdDataGridPaged grdColumns;
        private System.Windows.Forms.Button btnDummy;
        private System.Windows.Forms.Panel pnlMoveColumn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnMoveColumn2Left;
        private System.Windows.Forms.Button btnMoveColumn2Right;
        private System.Windows.Forms.Panel pnlAddRemButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnAddColumn;
        private System.Windows.Forms.Button btnRemoveColumn;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.GroupBox grpDefineColumn;
        private System.Windows.Forms.Panel pnlColumnDefinition;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private Ict.Common.Controls.TCmbAutoComplete cmbCalculation;
        private System.Windows.Forms.Label lblCalculation;
        private System.Windows.Forms.TextBox txtColumnWidth;
        private System.Windows.Forms.Label lblColumnWidth;
        private System.Windows.Forms.Label lblCm;
        private System.Windows.Forms.Panel pnlLowerButtons;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
    }
}
