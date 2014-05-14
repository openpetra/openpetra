//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
//
// Copyright 2004-2011 by OM International
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

using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using SourceGrid;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ict.Petra.Client.MFinance.Gui.GL
{
    partial class TGLRevaluation
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
            System.ComponentModel.ComponentResourceManager
                resources = new System.ComponentModel.ComponentResourceManager(
                typeof(TFrmGLBatch));

            this.lblAccountText = new System.Windows.Forms.Label();
            this.lblAccountValue = new System.Windows.Forms.Label();

            this.lblDateEnd = new System.Windows.Forms.Label();
            this.lblDateEndValue = new System.Windows.Forms.Label();

            this.lblRevCur = new System.Windows.Forms.Label();
            this.lblRevCurValue = new System.Windows.Forms.Label();

            grdDetails = new SourceGrid.DataGrid();

            this.btnRevaluate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            int leftColStart = 32;
            int leftColWidth = 240;
            int colSpace = 15;
            int rowY = 5;
            int rowHeight = 25;
            int rightColWidth = 350;

            // int buttonTop = 210;
            int buttonLeft = 140;

            //
            // lblAccountText
            //
            this.lblAccountText.Location = new System.Drawing.Point(leftColStart, rowY);
            this.lblAccountText.Name = "lblAccountText";
            this.lblAccountText.Size = new System.Drawing.Size(leftColWidth, 21);
            lblAccountText.TextAlign = ContentAlignment.MiddleRight;

            //
            // lblAccountValue
            //
            this.lblAccountValue.Location = new System.Drawing.Point(
                leftColStart + leftColWidth + colSpace, rowY);
            this.lblAccountValue.Name = "lblAccountValue";
            this.lblAccountValue.Size = new System.Drawing.Size(144, 21);
            lblAccountValue.TextAlign = ContentAlignment.MiddleLeft;

            rowY = rowY + rowHeight;

            //
            // lblDateEnd
            //
            this.lblDateEnd.Location = new System.Drawing.Point(leftColStart, rowY);
            this.lblDateEnd.Name = "lblDateEnd";
            this.lblDateEnd.Size = new System.Drawing.Size(leftColWidth, 21);
            lblDateEnd.TextAlign = ContentAlignment.MiddleRight;

            //
            // lblDateEndValue
            //
            this.lblDateEndValue.Location = new System.Drawing.Point(
                leftColStart + leftColWidth + colSpace, rowY);
            this.lblDateEndValue.Name = "lblDateEndValue";
            this.lblDateEndValue.Size = new System.Drawing.Size(rightColWidth, 21);
            lblDateEndValue.TextAlign = ContentAlignment.MiddleLeft;

            rowY = rowY + rowHeight;

            //
            // lblRevCur
            //
            this.lblRevCur.Location = new System.Drawing.Point(leftColStart, rowY);
            this.lblRevCur.Name = "lblRevCur";
            this.lblRevCur.Size = new System.Drawing.Size(leftColWidth, 21);
            lblRevCur.TextAlign = ContentAlignment.MiddleRight;

            //
            // lblRevCurValue
            //
            this.lblRevCurValue.Location = new System.Drawing.Point(
                leftColStart + leftColWidth + colSpace, rowY);
            this.lblRevCurValue.Name = "lblRevCurValue";
            this.lblRevCurValue.Size = new System.Drawing.Size(rightColWidth, 21);
            lblRevCurValue.TextAlign = ContentAlignment.MiddleLeft;


            rowY = rowY + rowHeight;

            this.grdDetails.Location = new System.Drawing.Point(leftColStart, rowY);
            this.grdDetails.Size = new System.Drawing.Size(550, 200);
            grdDetails.BorderStyle = BorderStyle.FixedSingle;
            //grdDetails.FixedRows = 3;
            //grdDetails.a

            rowY = rowY + 230;

            //
            // btnRevaluate
            //
            this.btnRevaluate.Location = new System.Drawing.Point(buttonLeft, rowY);
            this.btnRevaluate.Name = "btnRevaluate";
            this.btnRevaluate.Size = new System.Drawing.Size(144, 23);
            this.btnRevaluate.TabIndex = 2;
            this.btnRevaluate.Text = "Revaluate";
            this.btnRevaluate.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(200 + buttonLeft, rowY);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(137, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // GLRevaluation
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Verdana", 8.25f);


            this.ClientSize = new System.Drawing.Size(600, rowY + 50);
            this.CancelButton = btnCancel;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRevaluate);
            this.Controls.Add(this.lblAccountText);
            this.Controls.Add(this.lblAccountValue);
            this.Controls.Add(this.lblDateEnd);
            this.Controls.Add(this.lblDateEndValue);
            this.Controls.Add(this.lblRevCur);
            this.Controls.Add(this.lblRevCurValue);
            this.Controls.Add(this.grdDetails);

            this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            this.Name = "GLRevaluation";
            this.Text = "Revaluation ...";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRevaluate;
        private System.Windows.Forms.Label lblAccountText;
        private System.Windows.Forms.Label lblAccountValue;
        private System.Windows.Forms.Label lblDateEnd;
        private System.Windows.Forms.Label lblDateEndValue;
        private System.Windows.Forms.Label lblRevCur;
        private System.Windows.Forms.Label lblRevCurValue;

        private SourceGrid.DataGrid grdDetails;
    }
}