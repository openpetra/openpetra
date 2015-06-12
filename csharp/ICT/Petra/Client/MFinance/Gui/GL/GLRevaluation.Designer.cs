//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
//
// Copyright 2004-2015 by OM International
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
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager
                resources = new System.ComponentModel.ComponentResourceManager(
                typeof(TFrmGLBatch));

            lblAccountText = new Label();
            lblAccountValue = new Label();

            lblDateEnd = new Label();
            lblDateEndValue = new Label();

            lblRevCur = new Label();
            lblRevCurValue = new Label();

            lblCostCentre = new Label();
            cmbCostCentres = new TCmbAutoPopulated();

            grdDetails = new SourceGrid.DataGrid();

            btnRevaluate = new Button();
            btnCancel = new Button();
            SuspendLayout();

            int leftColStart = 32;
            int leftColWidth = 180;
            int colSpace = 15;
            int rowY = 5;
            int rowHeight = 25;
            int rightColWidth = 280;

            // int buttonTop = 210;
            int buttonLeft = 120;

            //
            // lblAccountText
            //
            lblAccountText.Location = new System.Drawing.Point(leftColStart, rowY);
            lblAccountText.Name = "lblAccountText";
            lblAccountText.Size = new System.Drawing.Size(leftColWidth, 21);
            lblAccountText.TextAlign = ContentAlignment.MiddleRight;

            //
            // lblAccountValue
            //
            lblAccountValue.Location = new System.Drawing.Point(leftColStart + leftColWidth + colSpace, rowY);
            lblAccountValue.Name = "lblAccountValue";
            lblAccountValue.Size = new System.Drawing.Size(144, 21);
            lblAccountValue.TextAlign = ContentAlignment.MiddleLeft;

            rowY = rowY + rowHeight;

            //
            // lblDateEnd
            //
            lblDateEnd.Location = new System.Drawing.Point(leftColStart, rowY);
            lblDateEnd.Name = "lblDateEnd";
            lblDateEnd.Size = new System.Drawing.Size(leftColWidth, 21);
            lblDateEnd.TextAlign = ContentAlignment.MiddleRight;

            //
            // lblDateEndValue
            //
            lblDateEndValue.Location = new System.Drawing.Point(leftColStart + leftColWidth + colSpace, rowY);
            lblDateEndValue.Name = "lblDateEndValue";
            lblDateEndValue.Size = new System.Drawing.Size(rightColWidth, 21);
            lblDateEndValue.TextAlign = ContentAlignment.MiddleLeft;

            rowY = rowY + rowHeight;

            //
            // lblRevCur
            //
            lblRevCur.Location = new System.Drawing.Point(leftColStart, rowY);
            lblRevCur.Name = "lblRevCur";
            lblRevCur.Size = new System.Drawing.Size(leftColWidth, 21);
            lblRevCur.TextAlign = ContentAlignment.MiddleRight;

            //
            // lblRevCurValue
            //
            lblRevCurValue.Location = new System.Drawing.Point(leftColStart + leftColWidth + colSpace, rowY);
            lblRevCurValue.Name = "lblRevCurValue";
            lblRevCurValue.Size = new System.Drawing.Size(rightColWidth, 21);
            lblRevCurValue.TextAlign = ContentAlignment.MiddleLeft;

            rowY = rowY + rowHeight;

            //
            // Cost Centre
            //

            lblCostCentre.Location = new System.Drawing.Point(leftColStart, rowY);
            lblCostCentre.Name = "lblCostCentre";
            lblCostCentre.Size = new System.Drawing.Size(leftColWidth, 21);
            lblCostCentre.TextAlign = ContentAlignment.MiddleRight;

            cmbCostCentres.Location = new System.Drawing.Point(leftColStart + leftColWidth + colSpace, rowY);
            cmbCostCentres.Name = "clbCostCentres";
            cmbCostCentres.Size = new System.Drawing.Size(rightColWidth, 22);
            cmbCostCentres.ListTable = TCmbAutoPopulated.TListTableEnum.UserDefinedList;

            rowY = rowY + rowHeight + 20;

            grdDetails.Location = new System.Drawing.Point(leftColStart, rowY);
            grdDetails.Size = new System.Drawing.Size(440, 200);
            grdDetails.BorderStyle = BorderStyle.FixedSingle;

            rowY = rowY + 230;

            //
            // btnRevaluate
            //
            btnRevaluate.Location = new System.Drawing.Point(buttonLeft, rowY);
            btnRevaluate.Name = "btnRevaluate";
            btnRevaluate.Size = new System.Drawing.Size(120, 23);
            btnRevaluate.TabIndex = 2;
            btnRevaluate.Text = "Revalue";
            btnRevaluate.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            btnCancel.Location = new System.Drawing.Point(150 + buttonLeft, rowY);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(120, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            //
            // GLRevaluation
            //
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Font = new System.Drawing.Font("Verdana", 8.25f);


            ClientSize = new System.Drawing.Size(500, rowY + 50);
            CancelButton = btnCancel;
            Controls.AddRange(new Control[] {
                    btnCancel,
                    btnRevaluate,
                    lblAccountText,
                    lblAccountValue,
                    lblDateEnd,
                    lblDateEndValue,
                    lblRevCur,
                    lblRevCurValue,
                    lblCostCentre,
                    cmbCostCentres,
                    this.grdDetails
                });

            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");

            Name = "GLRevaluation";
            Text = "Revaluation ...";
            ResumeLayout(false);
        }

        private Button btnCancel;
        private Button btnRevaluate;
        private Label lblAccountText;
        private Label lblAccountValue;
        private Label lblDateEnd;
        private Label lblDateEndValue;
        private Label lblRevCur;
        private Label lblRevCurValue;
        private Label lblCostCentre;
        private Ict.Petra.Client.CommonControls.TCmbAutoPopulated cmbCostCentres;

        private SourceGrid.DataGrid grdDetails;
    }
}