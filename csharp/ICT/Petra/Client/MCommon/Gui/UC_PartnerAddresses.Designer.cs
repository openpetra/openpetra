//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
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
using System.Drawing.Printing;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MCommon.Gui
{
    partial class TUCPartnerAddresses
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUCPartnerAddresses));
            this.components = new System.ComponentModel.Container();
            this.imlRecordIcons = new System.Windows.Forms.ImageList(this.components);
            this.tipMain = new System.Windows.Forms.ToolTip(this.components);
            this.btnMaximiseMinimiseGrid = new System.Windows.Forms.Button();
            this.imlButtonIcons = new System.Windows.Forms.ImageList(this.components);
            this.grdRecordList = new Ict.Common.Controls.TSgrdDataGrid();
            this.pnlRecordList = new System.Windows.Forms.Panel();
            this.btnDeleteRecord = new System.Windows.Forms.Button();
            this.btnNewRecord = new System.Windows.Forms.Button();
            this.splListSectionDetailsSection = new System.Windows.Forms.Splitter();
            this.ucoDetails = new Ict.Petra.Client.MCommon.Gui.TUC_PartnerAddress();
            this.pnlBalloonTipAnchor = new System.Windows.Forms.Panel();
            this.pnlRecordList.SuspendLayout();
            this.SuspendLayout();

            //
            // imlRecordIcons
            //
            this.imlRecordIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imlRecordIcons.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imlRecordIcons.ImageStream");
            this.imlRecordIcons.TransparentColor = System.Drawing.Color.Transparent;

            //
            // tipMain
            //
            this.tipMain.AutoPopDelay = 1000;
            this.tipMain.InitialDelay = 500;
            this.tipMain.ReshowDelay = 100;

            //
            // btnMaximiseMinimiseGrid
            //
            this.btnMaximiseMinimiseGrid.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnMaximiseMinimiseGrid.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMaximiseMinimiseGrid.ImageIndex = 5;
            this.btnMaximiseMinimiseGrid.ImageList = this.imlButtonIcons;
            this.btnMaximiseMinimiseGrid.Location = new System.Drawing.Point(660, 86);
            this.btnMaximiseMinimiseGrid.Name = "btnMaximiseMinimiseGrid";
            this.btnMaximiseMinimiseGrid.Size = new System.Drawing.Size(20, 18);
            this.btnMaximiseMinimiseGrid.TabIndex = 1;
            this.tipMain.SetToolTip(this.btnMaximiseMinimiseGrid, "Make List higher/sm" + "aller");
            this.btnMaximiseMinimiseGrid.Click += new System.EventHandler(this.BtnMaximiseMinimiseGrid_Click);

            //
            // imlButtonIcons
            //
            this.imlButtonIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imlButtonIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject('i' + "mlButtonIcons.ImageStream")));
            this.imlButtonIcons.TransparentColor = System.Drawing.Color.Transparent;

            //
            // grdRecordList
            //
            this.grdRecordList.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grdRecordList.AutoFindColumn = ((short)(2));
            this.grdRecordList.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;
            this.grdRecordList.BackColor = System.Drawing.SystemColors.ControlDark;
            this.grdRecordList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdRecordList.DeleteQuestionMessage = "You have chosen to delete thi" + "s record.'#13#10#13#10'Do you really want to delete it?";
            this.grdRecordList.FixedRows = 1;
            this.grdRecordList.Location = new System.Drawing.Point(4, 6);
            this.grdRecordList.MinimumHeight = 19;
            this.grdRecordList.Name = "grdRecordList";
            this.grdRecordList.Size = new System.Drawing.Size(652, 99);
            this.grdRecordList.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));
            this.grdRecordList.TabIndex = 0;
            this.grdRecordList.TabStop = true;
            this.grdRecordList.DoubleClickCell += new TDoubleClickCellEventHandler(this.GrdRecordList_DoubleClickCell);
            this.grdRecordList.InsertKeyPressed += new TKeyPressedEventHandler(this.GrdRecordList_InsertKeyPressed);
            this.grdRecordList.EnterKeyPressed += new TKeyPressedEventHandler(this.GrdRecordList_EnterKeyPressed);
            this.grdRecordList.DeleteKeyPressed += new TKeyPressedEventHandler(this.GrdRecordList_DeleteKeyPressed);

            //
            // pnlRecordList
            //
            this.pnlRecordList.BackColor = System.Drawing.SystemColors.Control;
            this.pnlRecordList.Controls.Add(this.pnlBalloonTipAnchor);
            this.pnlRecordList.Controls.Add(this.grdRecordList);
            this.pnlRecordList.Controls.Add(this.btnMaximiseMinimiseGrid);
            this.pnlRecordList.Controls.Add(this.btnDeleteRecord);
            this.pnlRecordList.Controls.Add(this.btnNewRecord);
            this.pnlRecordList.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRecordList.Location = new System.Drawing.Point(0, 0);
            this.pnlRecordList.Name = "pnlRecordList";

            // this.rpsUserControl.SetRestoreLocation(this.pnlRecordList, true); Disabled TH
            this.pnlRecordList.Size = new System.Drawing.Size(740, 108);
            this.pnlRecordList.TabIndex = 0;

            //
            // btnDeleteRecord
            //
            this.btnDeleteRecord.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnDeleteRecord.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnDeleteRecord.ImageIndex = 2;
            this.btnDeleteRecord.ImageList = this.imlButtonIcons;
            this.btnDeleteRecord.Location = new System.Drawing.Point(664, 34);
            this.btnDeleteRecord.Name = "btnDeleteRecord";
            this.btnDeleteRecord.Size = new System.Drawing.Size(76, 23);
            this.btnDeleteRecord.TabIndex = 4;
            this.btnDeleteRecord.Text = "      &Delete";
            this.btnDeleteRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteRecord.Click += new System.EventHandler(this.BtnDeleteRecord_Click);

            //
            // btnNewRecord
            //
            this.btnNewRecord.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnNewRecord.BackColor = System.Drawing.SystemColors.Control;
            this.btnNewRecord.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnNewRecord.ImageIndex = 0;
            this.btnNewRecord.ImageList = this.imlButtonIcons;
            this.btnNewRecord.Location = new System.Drawing.Point(664, 10);
            this.btnNewRecord.Name = "btnNewRecord";
            this.btnNewRecord.Size = new System.Drawing.Size(76, 23);
            this.btnNewRecord.TabIndex = 2;
            this.btnNewRecord.Text = "       &New";
            this.btnNewRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewRecord.Click += new System.EventHandler(this.BtnNewRecord_Click);

            //
            // splListSectionDetailsSection
            //
            this.splListSectionDetailsSection.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splListSectionDetailsSection.Dock = System.Windows.Forms.DockStyle.Top;
            this.splListSectionDetailsSection.Location = new System.Drawing.Point(0, 108);
            this.splListSectionDetailsSection.MinExtra = 53;
            this.splListSectionDetailsSection.MinSize = 52;
            this.splListSectionDetailsSection.Name = "splListSectionDetailsSection";
            this.splListSectionDetailsSection.Size = new System.Drawing.Size(740, 4);
            this.splListSectionDetailsSection.TabIndex = 3;
            this.splListSectionDetailsSection.TabStop = false;

            //
            // ucoDetails
            //
            this.ucoDetails.AutoSize = true;
            this.ucoDetails.AutoScroll = true;
            this.ucoDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucoDetails.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.ucoDetails.Key = 0;
            this.ucoDetails.Location = new System.Drawing.Point(0, 112);
            this.ucoDetails.Name = "ucoDetails";
            this.ucoDetails.Size = new System.Drawing.Size(740, 328);
            this.ucoDetails.TabIndex = 4;

            //
            // pnlBalloonTipAnchor
            //
            this.pnlBalloonTipAnchor.Location = new System.Drawing.Point(60, 0);
            this.pnlBalloonTipAnchor.Name = "pnlBalloonTipAnchor";
            this.pnlBalloonTipAnchor.Size = new System.Drawing.Size(1, 1);
            this.pnlBalloonTipAnchor.TabIndex = 5;

            //
            // TUCPartnerAddresses
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.Controls.Add(this.ucoDetails);
            this.Controls.Add(this.splListSectionDetailsSection);
            this.Controls.Add(this.pnlRecordList);
            this.Name = "TUCPartnerAddresses";
            this.Size = new System.Drawing.Size(740, 440);
            this.pnlRecordList.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ImageList imlButtonIcons;
        private System.Windows.Forms.Panel pnlRecordList;
        private System.Windows.Forms.Button btnMaximiseMinimiseGrid;
        private System.Windows.Forms.Button btnDeleteRecord;
        private System.Windows.Forms.Button btnNewRecord;
        private System.Windows.Forms.ImageList imlRecordIcons;
        private System.Windows.Forms.ToolTip tipMain;
        private TSgrdDataGrid grdRecordList;
        private System.Windows.Forms.Splitter splListSectionDetailsSection;
        private TUC_PartnerAddress ucoDetails;
        private System.Windows.Forms.Panel pnlBalloonTipAnchor;
    }
}