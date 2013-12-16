//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using Ict.Common.Controls;

namespace Ict.Petra.Client.MPartner.Gui
{
    partial class TUC_PartnerFind_ByPartnerDetails
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerFind_ByPartnerDetails));

            this.btnSearch = new System.Windows.Forms.Button();
            this.chkDetailedResults = new System.Windows.Forms.CheckBox();
            this.btnClearCriteria = new System.Windows.Forms.Button();
            this.grpCriteria = new System.Windows.Forms.GroupBox();
            this.linCriteriaDivider = new DevAge.Windows.Forms.Line();
            this.btnCustomCriteriaDemo = new System.Windows.Forms.Button();
            this.ucoPartnerFindCriteria = new Ict.Petra.Client.MPartner.Gui.TUC_PartnerFindCriteria();
            this.ucoPartnerInfo = new TPnlCollapsible();
            this.grpResult = new System.Windows.Forms.GroupBox();
            this.pnlBlankSearchResult = new System.Windows.Forms.Panel();
            this.lblSearchInfo = new System.Windows.Forms.Label();
            this.spcPartnerFindByDetails = new System.Windows.Forms.SplitContainer();
            this.pnlPartnerInfoContainer = new System.Windows.Forms.Panel();
            this.grpCriteria.SuspendLayout();
            this.grpResult.SuspendLayout();
            this.pnlBlankSearchResult.SuspendLayout();
            this.spcPartnerFindByDetails.Panel1.SuspendLayout();
            this.spcPartnerFindByDetails.Panel2.SuspendLayout();
            this.spcPartnerFindByDetails.SuspendLayout();

            //
            // btnSearch
            //
            this.btnSearch.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(8, 200);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(83, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = " &Search";
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);

            //
            // chkDetailedResults
            //
            this.chkDetailedResults.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDetailedResults.Checked = true;
            this.chkDetailedResults.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDetailedResults.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkDetailedResults.Location = new System.Drawing.Point(108, 199);
            this.chkDetailedResults.Name = "chkDetailedResults";
            this.chkDetailedResults.Size = new System.Drawing.Size(120, 24);
            this.chkDetailedResults.TabIndex = 2;
            this.chkDetailedResults.Text = "Detailed Results";
            this.chkDetailedResults.CheckedChanged += new System.EventHandler(this.ChkDetailedResults_CheckedChanged);

            //
            // btnClearCriteria
            //
            this.btnClearCriteria.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearCriteria.Image = ((System.Drawing.Image)(resources.GetObject("btnClearCriteria.Image")));
            this.btnClearCriteria.Location = new System.Drawing.Point(434, 200);
            this.btnClearCriteria.Name = "btnClearCriteria";
            this.btnClearCriteria.Size = new System.Drawing.Size(75, 23);
            this.btnClearCriteria.TabIndex = 3;
            this.btnClearCriteria.Text = "Clea&r";
            this.btnClearCriteria.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClearCriteria.Click += new System.EventHandler(this.BtnClearCriteria_Click);

            //
            // grpCriteria
            //
            this.grpCriteria.Controls.Add(this.linCriteriaDivider);
            this.grpCriteria.Controls.Add(this.btnCustomCriteriaDemo);
            this.grpCriteria.Controls.Add(this.ucoPartnerFindCriteria);
            this.grpCriteria.Controls.Add(this.btnSearch);
            this.grpCriteria.Controls.Add(this.chkDetailedResults);
            this.grpCriteria.Controls.Add(this.btnClearCriteria);
            this.grpCriteria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCriteria.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpCriteria.Location = new System.Drawing.Point(0, 0);
            this.grpCriteria.Name = "grpCriteria";
            this.grpCriteria.Size = new System.Drawing.Size(516, 227);
            this.grpCriteria.TabIndex = 0;
            this.grpCriteria.TabStop = false;
            this.grpCriteria.Text = "Find Criteria";
            this.grpCriteria.Enter += new System.EventHandler(this.GrpCriteria_Enter);

            //
            // linCriteriaDivider
            //
            this.linCriteriaDivider.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.linCriteriaDivider.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.linCriteriaDivider.FirstColor = System.Drawing.SystemColors.ControlDark;
            this.linCriteriaDivider.LineStyle = DevAge.Windows.Forms.LineStyle.Horizontal;
            this.linCriteriaDivider.Location = new System.Drawing.Point(6, 196);
            this.linCriteriaDivider.Name = "linCriteriaDivider";
            this.linCriteriaDivider.SecondColor = System.Drawing.SystemColors.ControlLightLight;
            this.linCriteriaDivider.Size = new System.Drawing.Size(505, 2);
            this.linCriteriaDivider.TabIndex = 5;
            this.linCriteriaDivider.TabStop = false;

            //
            // btnCustomCriteriaDemo
            //
            this.btnCustomCriteriaDemo.Anchor =
                ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCustomCriteriaDemo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCustomCriteriaDemo.Location = new System.Drawing.Point(283, 204);
            this.btnCustomCriteriaDemo.Name = "btnCustomCriteriaDemo";
            this.btnCustomCriteriaDemo.Size = new System.Drawing.Size(144, 20);
            this.btnCustomCriteriaDemo.TabIndex = 4;
            this.btnCustomCriteriaDemo.Text = "Custom Criteria Demo";
            this.btnCustomCriteriaDemo.Visible = false;
            this.btnCustomCriteriaDemo.Click += new System.EventHandler(this.BtnCustomCriteriaDemo_Click);

            //
            // ucoPartnerFindCriteria
            //
            this.ucoPartnerFindCriteria.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.ucoPartnerFindCriteria.AutoScroll = true;
            this.ucoPartnerFindCriteria.BackColor = System.Drawing.SystemColors.Control;
            this.ucoPartnerFindCriteria.CriteriaFieldsLeft = null;
            this.ucoPartnerFindCriteria.CriteriaFieldsRight = null;
            this.ucoPartnerFindCriteria.CriteriaSetupMode = false;
            this.ucoPartnerFindCriteria.Font = new System.Drawing.Font("Verdana",
                8.25F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));
            this.ucoPartnerFindCriteria.Location = new System.Drawing.Point(2, 14);
            this.ucoPartnerFindCriteria.Name = "ucoPartnerFindCriteria";
            this.ucoPartnerFindCriteria.Size = new System.Drawing.Size(508, 179);
            this.ucoPartnerFindCriteria.TabIndex = 0;

            //
            // ucoPartnerInfo
            //
            this.ucoPartnerInfo.Dock = DockStyle.Bottom;
            this.ucoPartnerInfo.AutoScroll = true;
            this.ucoPartnerInfo.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucoPartnerInfo.Location = new System.Drawing.Point(0, 8);
            this.ucoPartnerInfo.Name = "ucoPartnerInfo";
            this.ucoPartnerInfo.Text = "Partner Info";
            this.ucoPartnerInfo.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.ucoPartnerInfo.Size = new System.Drawing.Size(515, 110);
            this.ucoPartnerInfo.TabIndex = 0;
            this.ucoPartnerInfo.VisualStyleEnum = TVisualStylesEnum.vsDashboard;
            this.ucoPartnerInfo.Collapsed += new System.EventHandler(this.UcoPartnerInfo_Collapsed);
            this.ucoPartnerInfo.Expanded += new System.EventHandler(this.UcoPartnerInfo_Expanded);

            //
            // grpResult
            //
            this.grpResult.Controls.Add(this.pnlBlankSearchResult);
            this.grpResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpResult.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpResult.Location = new System.Drawing.Point(0, 3);
            this.grpResult.Name = "grpResult";
            this.grpResult.Padding = new System.Windows.Forms.Padding(6, 3, 6, 4);
            this.grpResult.Size = new System.Drawing.Size(516, 106);
            this.grpResult.TabIndex = 1;
            this.grpResult.TabStop = false;
            this.grpResult.Text = "Fin&d Result";

            //
            // pnlBlankSearchResult
            //
            this.pnlBlankSearchResult.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlBlankSearchResult.Controls.Add(this.lblSearchInfo);
            this.pnlBlankSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBlankSearchResult.Location = new System.Drawing.Point(6, 17);
            this.pnlBlankSearchResult.Name = "pnlBlankSearchResult";
            this.pnlBlankSearchResult.Size = new System.Drawing.Size(504, 85);
            this.pnlBlankSearchResult.TabIndex = 5;

            //
            // lblSearchInfo
            //
            this.lblSearchInfo.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.lblSearchInfo.Font =
                new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchInfo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblSearchInfo.Location = new System.Drawing.Point(0, -4);
            this.lblSearchInfo.Name = "lblSearchInfo";
            this.lblSearchInfo.Size = new System.Drawing.Size(508, 88);
            this.lblSearchInfo.TabIndex = 0;
            this.lblSearchInfo.Text = "Searching...";
            this.lblSearchInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            //
            // spcPartnerFindByDetails
            //
            this.spcPartnerFindByDetails.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.spcPartnerFindByDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcPartnerFindByDetails.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spcPartnerFindByDetails.Location = new System.Drawing.Point(0, 0);
            this.spcPartnerFindByDetails.Name = "spcPartnerFindByDetails";
            this.spcPartnerFindByDetails.Orientation = System.Windows.Forms.Orientation.Horizontal;

            //
            // spcPartnerFindByDetails.Panel1
            //
            this.spcPartnerFindByDetails.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.spcPartnerFindByDetails.Panel1.Controls.Add(this.grpCriteria);
            this.spcPartnerFindByDetails.Panel1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 5);

            //
            // spcPartnerFindByDetails.Panel2
            //
            this.spcPartnerFindByDetails.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.spcPartnerFindByDetails.Panel2.Controls.Add(this.grpResult);
            this.spcPartnerFindByDetails.Panel2.Controls.Add(this.pnlPartnerInfoContainer);
            this.spcPartnerFindByDetails.Panel2.Padding = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.spcPartnerFindByDetails.Size = new System.Drawing.Size(519, 457);
            this.spcPartnerFindByDetails.SplitterDistance = 232;
            this.spcPartnerFindByDetails.SplitterWidth = 3;
            this.spcPartnerFindByDetails.TabIndex = 2;
            this.spcPartnerFindByDetails.TabStop = false;

            //
            // pnlPartnerInfoContainer
            //
            this.pnlPartnerInfoContainer.AutoSize = true;
            this.pnlPartnerInfoContainer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.pnlPartnerInfoContainer.Controls.Add(ucoPartnerInfo);
            this.pnlPartnerInfoContainer.BackColor = System.Drawing.SystemColors.Control;
            this.pnlPartnerInfoContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPartnerInfoContainer.Location = new System.Drawing.Point(0, 109);
            this.pnlPartnerInfoContainer.Name = "pnlPartnerInfoContainer";
            this.pnlPartnerInfoContainer.Size = new System.Drawing.Size(516, 110);
            this.pnlPartnerInfoContainer.TabIndex = 2;

            //
            // TUC_PartnerFind_ByPartnerDetails
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "TUC_PartnerFind_ByPartnerDetails";
            this.Controls.Add(this.spcPartnerFindByDetails);
            this.grpCriteria.ResumeLayout(false);
            this.grpCriteria.PerformLayout();
            this.grpResult.ResumeLayout(false);
            this.pnlBlankSearchResult.ResumeLayout(false);
            this.spcPartnerFindByDetails.Panel1.ResumeLayout(false);
            this.spcPartnerFindByDetails.Panel2.ResumeLayout(false);
            this.spcPartnerFindByDetails.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlPartnerInfoContainer;
        private DevAge.Windows.Forms.Line linCriteriaDivider;
        private System.Windows.Forms.SplitContainer spcPartnerFindByDetails;
        private System.Windows.Forms.GroupBox grpCriteria;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClearCriteria;
        private System.Windows.Forms.GroupBox grpResult;
        private TSgrdDataGridPaged grdResult;
        private System.Windows.Forms.CheckBox chkDetailedResults;
        private System.Windows.Forms.Panel pnlBlankSearchResult;
        private System.Windows.Forms.Label lblSearchInfo;
        private TUC_PartnerFindCriteria ucoPartnerFindCriteria;
        private TPnlCollapsible ucoPartnerInfo;
        private System.Windows.Forms.Button btnCustomCriteriaDemo;
    }
}