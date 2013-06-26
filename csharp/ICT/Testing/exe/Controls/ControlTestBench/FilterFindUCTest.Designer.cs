//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2013 by OM International
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
using Ict.Common.Controls;

namespace ControlTestBench
{
partial class FilterFindTest
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
        this.btnTestDefaultConstructor = new System.Windows.Forms.Button();
        this.btnTestFullConstructor = new System.Windows.Forms.Button();
        this.btnHideShowAFBtnStd = new System.Windows.Forms.Button();
        this.btnHideShowKFTOBtnStd = new System.Windows.Forms.Button();
        this.btnHideShowFIAOLblStd = new System.Windows.Forms.Button();
        this.FUcoFilterAndFind = new Ict.Common.Controls.TUcoFilterAndFind();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.groupBox5 = new System.Windows.Forms.GroupBox();
        this.chkYearExtra = new System.Windows.Forms.CheckBox();
        this.chkYearFind = new System.Windows.Forms.CheckBox();
        this.chkYearStd = new System.Windows.Forms.CheckBox();
        this.pnlYear = new System.Windows.Forms.Panel();
        this.cmbYear = new System.Windows.Forms.ComboBox();
        this.lblYear = new System.Windows.Forms.Label();
        this.chkCurrencyNameExtra = new System.Windows.Forms.CheckBox();
        this.chkCurrencyNameFind = new System.Windows.Forms.CheckBox();
        this.chkCurrencyNameStd = new System.Windows.Forms.CheckBox();
        this.pnlCurrencyName = new System.Windows.Forms.Panel();
        this.btnUnsetCurrencyName = new System.Windows.Forms.Button();
        this.txtCurrencyName = new System.Windows.Forms.TextBox();
        this.lblCurrencyName = new System.Windows.Forms.Label();
        this.label8 = new System.Windows.Forms.Label();
        this.label7 = new System.Windows.Forms.Label();
        this.label4 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.chkCurrencyCodeExtra = new System.Windows.Forms.CheckBox();
        this.chkCurrencyCodeFind = new System.Windows.Forms.CheckBox();
        this.chkCurrencyCodeStd = new System.Windows.Forms.CheckBox();
        this.pnlCurrencyCode = new System.Windows.Forms.Panel();
        this.btnUnsetCurrencyCode = new System.Windows.Forms.Button();
        this.txtCurrencyCode = new System.Windows.Forms.TextBox();
        this.lblCurrencyCode = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.txtControlWidth = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.panel1 = new System.Windows.Forms.Panel();
        this.grpExtraFilterPanel = new System.Windows.Forms.GroupBox();
        this.btnFilterIsAlwaysOnLabelExtra = new System.Windows.Forms.CheckBox();
        this.btnKeepFilterTurnedOnButtonExtra = new System.Windows.Forms.CheckBox();
        this.btnApplyFilterButtonExtra = new System.Windows.Forms.CheckBox();
        this.grpStandardFilterPanel = new System.Windows.Forms.GroupBox();
        this.btnFilterIsAlwaysOnLabelStd = new System.Windows.Forms.CheckBox();
        this.btnKeepFilterTurnedOnButtonStd = new System.Windows.Forms.CheckBox();
        this.btnApplyFilterButtonStd = new System.Windows.Forms.CheckBox();
        this.rbtTwoFilterPanels = new System.Windows.Forms.RadioButton();
        this.rbtOneFilterPanel = new System.Windows.Forms.RadioButton();
        this.chkShowFindTab = new System.Windows.Forms.CheckBox();
        this.groupBox3 = new System.Windows.Forms.GroupBox();
        this.button1 = new System.Windows.Forms.Button();
        this.btnCollapseExpandPanel = new System.Windows.Forms.Button();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.btnHideShowFIAOLblExtra = new System.Windows.Forms.Button();
        this.btnHideShowAFBtnExtra = new System.Windows.Forms.Button();
        this.btnHideShowKFTOBtnExtra = new System.Windows.Forms.Button();
        this.groupBox4 = new System.Windows.Forms.GroupBox();
        this.groupBox2.SuspendLayout();
        this.groupBox5.SuspendLayout();
        this.pnlYear.SuspendLayout();
        this.pnlCurrencyName.SuspendLayout();
        this.pnlCurrencyCode.SuspendLayout();
        this.panel1.SuspendLayout();
        this.grpExtraFilterPanel.SuspendLayout();
        this.grpStandardFilterPanel.SuspendLayout();
        this.groupBox3.SuspendLayout();
        this.groupBox1.SuspendLayout();
        this.groupBox4.SuspendLayout();
        this.SuspendLayout();
        // 
        // btnTestDefaultConstructor
        // 
        this.btnTestDefaultConstructor.Location = new System.Drawing.Point(191, 12);
        this.btnTestDefaultConstructor.Name = "btnTestDefaultConstructor";
        this.btnTestDefaultConstructor.Size = new System.Drawing.Size(147, 23);
        this.btnTestDefaultConstructor.TabIndex = 0;
        this.btnTestDefaultConstructor.Text = "Test Default Constructor";
        this.btnTestDefaultConstructor.UseVisualStyleBackColor = true;
        this.btnTestDefaultConstructor.Click += new System.EventHandler(this.TestDefaultConstructor);
        // 
        // btnTestFullConstructor
        // 
        this.btnTestFullConstructor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnTestFullConstructor.Location = new System.Drawing.Point(6, 206);
        this.btnTestFullConstructor.Name = "btnTestFullConstructor";
        this.btnTestFullConstructor.Size = new System.Drawing.Size(147, 23);
        this.btnTestFullConstructor.TabIndex = 1;
        this.btnTestFullConstructor.Text = "Test Full Constructor";
        this.btnTestFullConstructor.UseVisualStyleBackColor = true;
        this.btnTestFullConstructor.Click += new System.EventHandler(this.TestFullConstructor);
        // 
        // btnHideShowAFBtnStd
        // 
        this.btnHideShowAFBtnStd.Location = new System.Drawing.Point(6, 17);
        this.btnHideShowAFBtnStd.Name = "btnHideShowAFBtnStd";
        this.btnHideShowAFBtnStd.Size = new System.Drawing.Size(188, 23);
        this.btnHideShowAFBtnStd.TabIndex = 4;
        this.btnHideShowAFBtnStd.Text = "Hide/Show AF-Button";
        this.btnHideShowAFBtnStd.UseVisualStyleBackColor = true;
        this.btnHideShowAFBtnStd.Click += new System.EventHandler(this.BtnHideShowAFBtnClick);
        // 
        // btnHideShowKFTOBtnStd
        // 
        this.btnHideShowKFTOBtnStd.Location = new System.Drawing.Point(6, 46);
        this.btnHideShowKFTOBtnStd.Name = "btnHideShowKFTOBtnStd";
        this.btnHideShowKFTOBtnStd.Size = new System.Drawing.Size(188, 23);
        this.btnHideShowKFTOBtnStd.TabIndex = 5;
        this.btnHideShowKFTOBtnStd.Text = "Hide/Show KFTO-Button";
        this.btnHideShowKFTOBtnStd.UseVisualStyleBackColor = true;
        this.btnHideShowKFTOBtnStd.Click += new System.EventHandler(this.BtnHideShowKFTOBtnClick);
        // 
        // btnHideShowFIAOLblStd
        // 
        this.btnHideShowFIAOLblStd.Location = new System.Drawing.Point(6, 76);
        this.btnHideShowFIAOLblStd.Name = "btnHideShowFIAOLblStd";
        this.btnHideShowFIAOLblStd.Size = new System.Drawing.Size(188, 23);
        this.btnHideShowFIAOLblStd.TabIndex = 6;
        this.btnHideShowFIAOLblStd.Text = "Hide/Show FIAO-Label";
        this.btnHideShowFIAOLblStd.UseVisualStyleBackColor = true;
        this.btnHideShowFIAOLblStd.Click += new System.EventHandler(this.BtnHideShowFIAOLblClick);
        // 
        // FUcoFilterAndFind
        // 
        this.FUcoFilterAndFind.BackColor = System.Drawing.Color.LightSteelBlue;
        this.FUcoFilterAndFind.Dock = System.Windows.Forms.DockStyle.Left;
        this.FUcoFilterAndFind.Location = new System.Drawing.Point(0, 0);
        this.FUcoFilterAndFind.Name = "FUcoFilterAndFind";
        this.FUcoFilterAndFind.ShowApplyFilterButton = Ict.Common.Controls.TUcoFilterAndFind.FilterContext.fcNone;
        this.FUcoFilterAndFind.ShowExtraFilter = false;
        this.FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = Ict.Common.Controls.TUcoFilterAndFind.FilterContext.fcNone;
        this.FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = Ict.Common.Controls.TUcoFilterAndFind.FilterContext.fcNone;
        this.FUcoFilterAndFind.Size = new System.Drawing.Size(150, 500);
        this.FUcoFilterAndFind.TabIndex = 8;
        // 
        // groupBox2
        // 
        this.groupBox2.Controls.Add(this.groupBox5);
        this.groupBox2.Controls.Add(this.label2);
        this.groupBox2.Controls.Add(this.txtControlWidth);
        this.groupBox2.Controls.Add(this.label1);
        this.groupBox2.Controls.Add(this.panel1);
        this.groupBox2.Controls.Add(this.chkShowFindTab);
        this.groupBox2.Controls.Add(this.btnTestFullConstructor);
        this.groupBox2.Location = new System.Drawing.Point(191, 50);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(782, 236);
        this.groupBox2.TabIndex = 9;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "Full Constructor Options";
        // 
        // groupBox5
        // 
        this.groupBox5.Controls.Add(this.chkYearExtra);
        this.groupBox5.Controls.Add(this.chkYearFind);
        this.groupBox5.Controls.Add(this.chkYearStd);
        this.groupBox5.Controls.Add(this.pnlYear);
        this.groupBox5.Controls.Add(this.chkCurrencyNameExtra);
        this.groupBox5.Controls.Add(this.chkCurrencyNameFind);
        this.groupBox5.Controls.Add(this.chkCurrencyNameStd);
        this.groupBox5.Controls.Add(this.pnlCurrencyName);
        this.groupBox5.Controls.Add(this.label8);
        this.groupBox5.Controls.Add(this.label7);
        this.groupBox5.Controls.Add(this.label4);
        this.groupBox5.Controls.Add(this.label3);
        this.groupBox5.Controls.Add(this.chkCurrencyCodeExtra);
        this.groupBox5.Controls.Add(this.chkCurrencyCodeFind);
        this.groupBox5.Controls.Add(this.chkCurrencyCodeStd);
        this.groupBox5.Controls.Add(this.pnlCurrencyCode);
        this.groupBox5.Location = new System.Drawing.Point(476, 16);
        this.groupBox5.Name = "groupBox5";
        this.groupBox5.Size = new System.Drawing.Size(300, 213);
        this.groupBox5.TabIndex = 7;
        this.groupBox5.TabStop = false;
        this.groupBox5.Text = "Controls for Panels";
        // 
        // chkYearExtra
        // 
        this.chkYearExtra.Location = new System.Drawing.Point(211, 153);
        this.chkYearExtra.Name = "chkYearExtra";
        this.chkYearExtra.Size = new System.Drawing.Size(18, 18);
        this.chkYearExtra.TabIndex = 15;
        this.chkYearExtra.UseVisualStyleBackColor = true;
        // 
        // chkYearFind
        // 
        this.chkYearFind.Checked = true;
        this.chkYearFind.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkYearFind.Location = new System.Drawing.Point(251, 153);
        this.chkYearFind.Name = "chkYearFind";
        this.chkYearFind.Size = new System.Drawing.Size(18, 18);
        this.chkYearFind.TabIndex = 14;
        this.chkYearFind.UseVisualStyleBackColor = true;
        // 
        // chkYearStd
        // 
        this.chkYearStd.Location = new System.Drawing.Point(167, 153);
        this.chkYearStd.Name = "chkYearStd";
        this.chkYearStd.Size = new System.Drawing.Size(18, 18);
        this.chkYearStd.TabIndex = 13;
        this.chkYearStd.UseVisualStyleBackColor = true;
        // 
        // pnlYear
        // 
        this.pnlYear.BackColor = System.Drawing.Color.Gold;
        this.pnlYear.Controls.Add(this.cmbYear);
        this.pnlYear.Controls.Add(this.lblYear);
        this.pnlYear.Location = new System.Drawing.Point(6, 133);
        this.pnlYear.Name = "pnlYear";
        this.pnlYear.Size = new System.Drawing.Size(133, 43);
        this.pnlYear.TabIndex = 4;
        // 
        // cmbYear
        // 
        this.cmbYear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
        this.cmbYear.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.cmbYear.FormattingEnabled = true;
        this.cmbYear.Items.AddRange(new object[] {
                        "2012",
                        "2013"});
        this.cmbYear.Location = new System.Drawing.Point(3, 19);
        this.cmbYear.Name = "cmbYear";
        this.cmbYear.Size = new System.Drawing.Size(127, 21);
        this.cmbYear.TabIndex = 1;
        this.cmbYear.Text = "2012";
        // 
        // lblYear
        // 
        this.lblYear.Font = new System.Drawing.Font("Verdana", 7F);
        this.lblYear.Location = new System.Drawing.Point(3, 0);
        this.lblYear.Name = "lblYear";
        this.lblYear.Size = new System.Drawing.Size(100, 22);
        this.lblYear.TabIndex = 0;
        this.lblYear.Text = "Year:";
        // 
        // chkCurrencyNameExtra
        // 
        this.chkCurrencyNameExtra.Location = new System.Drawing.Point(211, 100);
        this.chkCurrencyNameExtra.Name = "chkCurrencyNameExtra";
        this.chkCurrencyNameExtra.Size = new System.Drawing.Size(18, 18);
        this.chkCurrencyNameExtra.TabIndex = 12;
        this.chkCurrencyNameExtra.UseVisualStyleBackColor = true;
        // 
        // chkCurrencyNameFind
        // 
        this.chkCurrencyNameFind.Location = new System.Drawing.Point(251, 100);
        this.chkCurrencyNameFind.Name = "chkCurrencyNameFind";
        this.chkCurrencyNameFind.Size = new System.Drawing.Size(18, 18);
        this.chkCurrencyNameFind.TabIndex = 11;
        this.chkCurrencyNameFind.UseVisualStyleBackColor = true;
        // 
        // chkCurrencyNameStd
        // 
        this.chkCurrencyNameStd.Checked = true;
        this.chkCurrencyNameStd.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkCurrencyNameStd.Location = new System.Drawing.Point(167, 100);
        this.chkCurrencyNameStd.Name = "chkCurrencyNameStd";
        this.chkCurrencyNameStd.Size = new System.Drawing.Size(18, 18);
        this.chkCurrencyNameStd.TabIndex = 10;
        this.chkCurrencyNameStd.UseVisualStyleBackColor = true;
        // 
        // pnlCurrencyName
        // 
        this.pnlCurrencyName.BackColor = System.Drawing.Color.DarkSalmon;
        this.pnlCurrencyName.Controls.Add(this.btnUnsetCurrencyName);
        this.pnlCurrencyName.Controls.Add(this.txtCurrencyName);
        this.pnlCurrencyName.Controls.Add(this.lblCurrencyName);
        this.pnlCurrencyName.Location = new System.Drawing.Point(6, 84);
        this.pnlCurrencyName.Name = "pnlCurrencyName";
        this.pnlCurrencyName.Size = new System.Drawing.Size(133, 43);
        this.pnlCurrencyName.TabIndex = 3;
        // 
        // btnUnsetCurrencyName
        // 
        this.btnUnsetCurrencyName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnUnsetCurrencyName.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.btnUnsetCurrencyName.Font = new System.Drawing.Font("Verdana", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnUnsetCurrencyName.Location = new System.Drawing.Point(109, 17);
        this.btnUnsetCurrencyName.Name = "btnUnsetCurrencyName";
        this.btnUnsetCurrencyName.Size = new System.Drawing.Size(18, 18);
        this.btnUnsetCurrencyName.TabIndex = 2;
        this.btnUnsetCurrencyName.Text = "X";
        this.btnUnsetCurrencyName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        this.btnUnsetCurrencyName.UseVisualStyleBackColor = true;
        // 
        // txtCurrencyName
        // 
        this.txtCurrencyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
        this.txtCurrencyName.Font = new System.Drawing.Font("Verdana", 8F);
        this.txtCurrencyName.Location = new System.Drawing.Point(3, 17);
        this.txtCurrencyName.Name = "txtCurrencyName";
        this.txtCurrencyName.Size = new System.Drawing.Size(100, 20);
        this.txtCurrencyName.TabIndex = 1;
        // 
        // lblCurrencyName
        // 
        this.lblCurrencyName.Font = new System.Drawing.Font("Verdana", 7F);
        this.lblCurrencyName.Location = new System.Drawing.Point(3, 0);
        this.lblCurrencyName.Name = "lblCurrencyName";
        this.lblCurrencyName.Size = new System.Drawing.Size(100, 22);
        this.lblCurrencyName.TabIndex = 0;
        this.lblCurrencyName.Text = "Currency Name:";
        // 
        // label8
        // 
        this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label8.Location = new System.Drawing.Point(241, 25);
        this.label8.Name = "label8";
        this.label8.Size = new System.Drawing.Size(37, 23);
        this.label8.TabIndex = 9;
        this.label8.Text = "Find";
        // 
        // label7
        // 
        this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label7.Location = new System.Drawing.Point(196, 25);
        this.label7.Name = "label7";
        this.label7.Size = new System.Drawing.Size(48, 23);
        this.label7.TabIndex = 8;
        this.label7.Text = "Extra";
        // 
        // label4
        // 
        this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label4.Location = new System.Drawing.Point(163, 25);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(27, 23);
        this.label4.TabIndex = 5;
        this.label4.Text = "Std.";
        // 
        // label3
        // 
        this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label3.Location = new System.Drawing.Point(159, 9);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(120, 23);
        this.label3.TabIndex = 4;
        this.label3.Text = "Should Appear On...";
        this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // chkCurrencyCodeExtra
        // 
        this.chkCurrencyCodeExtra.Location = new System.Drawing.Point(211, 51);
        this.chkCurrencyCodeExtra.Name = "chkCurrencyCodeExtra";
        this.chkCurrencyCodeExtra.Size = new System.Drawing.Size(18, 18);
        this.chkCurrencyCodeExtra.TabIndex = 3;
        this.chkCurrencyCodeExtra.UseVisualStyleBackColor = true;
        // 
        // chkCurrencyCodeFind
        // 
        this.chkCurrencyCodeFind.Location = new System.Drawing.Point(251, 51);
        this.chkCurrencyCodeFind.Name = "chkCurrencyCodeFind";
        this.chkCurrencyCodeFind.Size = new System.Drawing.Size(18, 18);
        this.chkCurrencyCodeFind.TabIndex = 2;
        this.chkCurrencyCodeFind.UseVisualStyleBackColor = true;
        // 
        // chkCurrencyCodeStd
        // 
        this.chkCurrencyCodeStd.Checked = true;
        this.chkCurrencyCodeStd.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkCurrencyCodeStd.Location = new System.Drawing.Point(167, 51);
        this.chkCurrencyCodeStd.Name = "chkCurrencyCodeStd";
        this.chkCurrencyCodeStd.Size = new System.Drawing.Size(18, 18);
        this.chkCurrencyCodeStd.TabIndex = 1;
        this.chkCurrencyCodeStd.UseVisualStyleBackColor = true;
        // 
        // pnlCurrencyCode
        // 
        this.pnlCurrencyCode.BackColor = System.Drawing.Color.DarkGray;
        this.pnlCurrencyCode.Controls.Add(this.btnUnsetCurrencyCode);
        this.pnlCurrencyCode.Controls.Add(this.txtCurrencyCode);
        this.pnlCurrencyCode.Controls.Add(this.lblCurrencyCode);
        this.pnlCurrencyCode.Location = new System.Drawing.Point(6, 35);
        this.pnlCurrencyCode.Name = "pnlCurrencyCode";
        this.pnlCurrencyCode.Size = new System.Drawing.Size(133, 43);
        this.pnlCurrencyCode.TabIndex = 0;
        // 
        // btnUnsetCurrencyCode
        // 
        this.btnUnsetCurrencyCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnUnsetCurrencyCode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.btnUnsetCurrencyCode.Font = new System.Drawing.Font("Verdana", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnUnsetCurrencyCode.Location = new System.Drawing.Point(109, 17);
        this.btnUnsetCurrencyCode.Name = "btnUnsetCurrencyCode";
        this.btnUnsetCurrencyCode.Size = new System.Drawing.Size(18, 18);
        this.btnUnsetCurrencyCode.TabIndex = 2;
        this.btnUnsetCurrencyCode.Text = "X";
        this.btnUnsetCurrencyCode.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        this.btnUnsetCurrencyCode.UseVisualStyleBackColor = true;
        // 
        // txtCurrencyCode
        // 
        this.txtCurrencyCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                        | System.Windows.Forms.AnchorStyles.Right)));
        this.txtCurrencyCode.Font = new System.Drawing.Font("Verdana", 8F);
        this.txtCurrencyCode.Location = new System.Drawing.Point(3, 17);
        this.txtCurrencyCode.Name = "txtCurrencyCode";
        this.txtCurrencyCode.Size = new System.Drawing.Size(100, 20);
        this.txtCurrencyCode.TabIndex = 1;
        // 
        // lblCurrencyCode
        // 
        this.lblCurrencyCode.Font = new System.Drawing.Font("Verdana", 7F);
        this.lblCurrencyCode.Location = new System.Drawing.Point(3, 0);
        this.lblCurrencyCode.Name = "lblCurrencyCode";
        this.lblCurrencyCode.Size = new System.Drawing.Size(100, 22);
        this.lblCurrencyCode.TabIndex = 0;
        this.lblCurrencyCode.Text = "Currency Code:";
        // 
        // label2
        // 
        this.label2.Location = new System.Drawing.Point(369, 180);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(32, 23);
        this.label2.TabIndex = 6;
        this.label2.Text = "(px)";
        // 
        // txtControlWidth
        // 
        this.txtControlWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtControlWidth.Location = new System.Drawing.Point(319, 178);
        this.txtControlWidth.Name = "txtControlWidth";
        this.txtControlWidth.Size = new System.Drawing.Size(44, 20);
        this.txtControlWidth.TabIndex = 5;
        this.txtControlWidth.Text = "175";
        this.txtControlWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        // 
        // label1
        // 
        this.label1.Location = new System.Drawing.Point(213, 181);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(100, 23);
        this.label1.TabIndex = 4;
        this.label1.Text = "Control Width:";
        this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
        // 
        // panel1
        // 
        this.panel1.Controls.Add(this.grpExtraFilterPanel);
        this.panel1.Controls.Add(this.grpStandardFilterPanel);
        this.panel1.Controls.Add(this.rbtTwoFilterPanels);
        this.panel1.Controls.Add(this.rbtOneFilterPanel);
        this.panel1.Location = new System.Drawing.Point(3, 13);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(454, 157);
        this.panel1.TabIndex = 0;
        // 
        // grpExtraFilterPanel
        // 
        this.grpExtraFilterPanel.Controls.Add(this.btnFilterIsAlwaysOnLabelExtra);
        this.grpExtraFilterPanel.Controls.Add(this.btnKeepFilterTurnedOnButtonExtra);
        this.grpExtraFilterPanel.Controls.Add(this.btnApplyFilterButtonExtra);
        this.grpExtraFilterPanel.Enabled = false;
        this.grpExtraFilterPanel.Location = new System.Drawing.Point(245, 38);
        this.grpExtraFilterPanel.Name = "grpExtraFilterPanel";
        this.grpExtraFilterPanel.Size = new System.Drawing.Size(200, 111);
        this.grpExtraFilterPanel.TabIndex = 5;
        this.grpExtraFilterPanel.TabStop = false;
        this.grpExtraFilterPanel.Text = "\'Extra\' Filter Panel";
        // 
        // btnFilterIsAlwaysOnLabelExtra
        // 
        this.btnFilterIsAlwaysOnLabelExtra.Appearance = System.Windows.Forms.Appearance.Button;
        this.btnFilterIsAlwaysOnLabelExtra.Location = new System.Drawing.Point(6, 79);
        this.btnFilterIsAlwaysOnLabelExtra.Name = "btnFilterIsAlwaysOnLabelExtra";
        this.btnFilterIsAlwaysOnLabelExtra.Size = new System.Drawing.Size(188, 24);
        this.btnFilterIsAlwaysOnLabelExtra.TabIndex = 4;
        this.btnFilterIsAlwaysOnLabelExtra.Text = "\'Filter Is Always On\' Label";
        this.btnFilterIsAlwaysOnLabelExtra.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.btnFilterIsAlwaysOnLabelExtra.UseVisualStyleBackColor = true;
        // 
        // btnKeepFilterTurnedOnButtonExtra
        // 
        this.btnKeepFilterTurnedOnButtonExtra.Appearance = System.Windows.Forms.Appearance.Button;
        this.btnKeepFilterTurnedOnButtonExtra.Location = new System.Drawing.Point(6, 49);
        this.btnKeepFilterTurnedOnButtonExtra.Name = "btnKeepFilterTurnedOnButtonExtra";
        this.btnKeepFilterTurnedOnButtonExtra.Size = new System.Drawing.Size(188, 24);
        this.btnKeepFilterTurnedOnButtonExtra.TabIndex = 3;
        this.btnKeepFilterTurnedOnButtonExtra.Text = "\'Keep Filter Turned On\' Button";
        this.btnKeepFilterTurnedOnButtonExtra.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.btnKeepFilterTurnedOnButtonExtra.UseVisualStyleBackColor = true;
        // 
        // btnApplyFilterButtonExtra
        // 
        this.btnApplyFilterButtonExtra.Appearance = System.Windows.Forms.Appearance.Button;
        this.btnApplyFilterButtonExtra.Location = new System.Drawing.Point(6, 19);
        this.btnApplyFilterButtonExtra.Name = "btnApplyFilterButtonExtra";
        this.btnApplyFilterButtonExtra.Size = new System.Drawing.Size(188, 24);
        this.btnApplyFilterButtonExtra.TabIndex = 2;
        this.btnApplyFilterButtonExtra.Text = "\'Apply Filter\' Button";
        this.btnApplyFilterButtonExtra.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.btnApplyFilterButtonExtra.UseVisualStyleBackColor = true;
        // 
        // grpStandardFilterPanel
        // 
        this.grpStandardFilterPanel.Controls.Add(this.btnFilterIsAlwaysOnLabelStd);
        this.grpStandardFilterPanel.Controls.Add(this.btnKeepFilterTurnedOnButtonStd);
        this.grpStandardFilterPanel.Controls.Add(this.btnApplyFilterButtonStd);
        this.grpStandardFilterPanel.Location = new System.Drawing.Point(14, 38);
        this.grpStandardFilterPanel.Name = "grpStandardFilterPanel";
        this.grpStandardFilterPanel.Size = new System.Drawing.Size(200, 111);
        this.grpStandardFilterPanel.TabIndex = 3;
        this.grpStandardFilterPanel.TabStop = false;
        this.grpStandardFilterPanel.Text = "\'Standard\' Filter Panel";
        // 
        // btnFilterIsAlwaysOnLabelStd
        // 
        this.btnFilterIsAlwaysOnLabelStd.Appearance = System.Windows.Forms.Appearance.Button;
        this.btnFilterIsAlwaysOnLabelStd.Location = new System.Drawing.Point(6, 79);
        this.btnFilterIsAlwaysOnLabelStd.Name = "btnFilterIsAlwaysOnLabelStd";
        this.btnFilterIsAlwaysOnLabelStd.Size = new System.Drawing.Size(188, 24);
        this.btnFilterIsAlwaysOnLabelStd.TabIndex = 4;
        this.btnFilterIsAlwaysOnLabelStd.Text = "\'Filter Is Always On\' Label";
        this.btnFilterIsAlwaysOnLabelStd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.btnFilterIsAlwaysOnLabelStd.UseVisualStyleBackColor = true;
        // 
        // btnKeepFilterTurnedOnButtonStd
        // 
        this.btnKeepFilterTurnedOnButtonStd.Appearance = System.Windows.Forms.Appearance.Button;
        this.btnKeepFilterTurnedOnButtonStd.Location = new System.Drawing.Point(6, 49);
        this.btnKeepFilterTurnedOnButtonStd.Name = "btnKeepFilterTurnedOnButtonStd";
        this.btnKeepFilterTurnedOnButtonStd.Size = new System.Drawing.Size(188, 24);
        this.btnKeepFilterTurnedOnButtonStd.TabIndex = 3;
        this.btnKeepFilterTurnedOnButtonStd.Text = "\'Keep Filter Turned On\' Button";
        this.btnKeepFilterTurnedOnButtonStd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.btnKeepFilterTurnedOnButtonStd.UseVisualStyleBackColor = true;
        // 
        // btnApplyFilterButtonStd
        // 
        this.btnApplyFilterButtonStd.Appearance = System.Windows.Forms.Appearance.Button;
        this.btnApplyFilterButtonStd.Location = new System.Drawing.Point(6, 19);
        this.btnApplyFilterButtonStd.Name = "btnApplyFilterButtonStd";
        this.btnApplyFilterButtonStd.Size = new System.Drawing.Size(188, 24);
        this.btnApplyFilterButtonStd.TabIndex = 2;
        this.btnApplyFilterButtonStd.Text = "\'Apply Filter\' Button";
        this.btnApplyFilterButtonStd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.btnApplyFilterButtonStd.UseVisualStyleBackColor = true;
        // 
        // rbtTwoFilterPanels
        // 
        this.rbtTwoFilterPanels.Location = new System.Drawing.Point(245, 8);
        this.rbtTwoFilterPanels.Name = "rbtTwoFilterPanels";
        this.rbtTwoFilterPanels.Size = new System.Drawing.Size(118, 24);
        this.rbtTwoFilterPanels.TabIndex = 1;
        this.rbtTwoFilterPanels.Text = "&Two Filter Panels";
        this.rbtTwoFilterPanels.UseVisualStyleBackColor = true;
        this.rbtTwoFilterPanels.CheckedChanged += new System.EventHandler(this.RbtTwoFilterPanelsCheckedChanged);
        // 
        // rbtOneFilterPanel
        // 
        this.rbtOneFilterPanel.Checked = true;
        this.rbtOneFilterPanel.Location = new System.Drawing.Point(14, 8);
        this.rbtOneFilterPanel.Name = "rbtOneFilterPanel";
        this.rbtOneFilterPanel.Size = new System.Drawing.Size(104, 24);
        this.rbtOneFilterPanel.TabIndex = 0;
        this.rbtOneFilterPanel.TabStop = true;
        this.rbtOneFilterPanel.Text = "&One Filter Panel";
        this.rbtOneFilterPanel.UseVisualStyleBackColor = true;
        this.rbtOneFilterPanel.CheckedChanged += new System.EventHandler(this.RbtOneFilterPanelCheckedChanged);
        // 
        // chkShowFindTab
        // 
        this.chkShowFindTab.Location = new System.Drawing.Point(59, 176);
        this.chkShowFindTab.Name = "chkShowFindTab";
        this.chkShowFindTab.Size = new System.Drawing.Size(104, 24);
        this.chkShowFindTab.TabIndex = 3;
        this.chkShowFindTab.Text = "Show \'Fi&nd\' Tab";
        this.chkShowFindTab.UseVisualStyleBackColor = true;
        // 
        // groupBox3
        // 
        this.groupBox3.Controls.Add(this.button1);
        this.groupBox3.Controls.Add(this.btnCollapseExpandPanel);
        this.groupBox3.Controls.Add(this.groupBox1);
        this.groupBox3.Controls.Add(this.groupBox4);
        this.groupBox3.Location = new System.Drawing.Point(191, 304);
        this.groupBox3.Name = "groupBox3";
        this.groupBox3.Size = new System.Drawing.Size(463, 184);
        this.groupBox3.TabIndex = 10;
        this.groupBox3.TabStop = false;
        this.groupBox3.Text = "Manipulations on the running instance";
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(254, 143);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(188, 23);
        this.button1.TabIndex = 8;
        this.button1.Text = "Show Find Tab";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.Button1Click);
        // 
        // btnCollapseExpandPanel
        // 
        this.btnCollapseExpandPanel.Location = new System.Drawing.Point(23, 143);
        this.btnCollapseExpandPanel.Name = "btnCollapseExpandPanel";
        this.btnCollapseExpandPanel.Size = new System.Drawing.Size(188, 23);
        this.btnCollapseExpandPanel.TabIndex = 7;
        this.btnCollapseExpandPanel.Text = "Collapse/Expand Panel";
        this.btnCollapseExpandPanel.UseVisualStyleBackColor = true;
        this.btnCollapseExpandPanel.Click += new System.EventHandler(this.BtnCollapseExpandPanelClick);
        // 
        // groupBox1
        // 
        this.groupBox1.Controls.Add(this.btnHideShowFIAOLblExtra);
        this.groupBox1.Controls.Add(this.btnHideShowAFBtnExtra);
        this.groupBox1.Controls.Add(this.btnHideShowKFTOBtnExtra);
        this.groupBox1.Location = new System.Drawing.Point(248, 19);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(200, 108);
        this.groupBox1.TabIndex = 7;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "\'Extra\' Filter Panel";
        // 
        // btnHideShowFIAOLblExtra
        // 
        this.btnHideShowFIAOLblExtra.Location = new System.Drawing.Point(6, 76);
        this.btnHideShowFIAOLblExtra.Name = "btnHideShowFIAOLblExtra";
        this.btnHideShowFIAOLblExtra.Size = new System.Drawing.Size(188, 23);
        this.btnHideShowFIAOLblExtra.TabIndex = 6;
        this.btnHideShowFIAOLblExtra.Text = "Hide/Show FIAO-Label";
        this.btnHideShowFIAOLblExtra.UseVisualStyleBackColor = true;
        this.btnHideShowFIAOLblExtra.Click += new System.EventHandler(this.BtnHideShowFIAOLblClick);
        // 
        // btnHideShowAFBtnExtra
        // 
        this.btnHideShowAFBtnExtra.Location = new System.Drawing.Point(6, 17);
        this.btnHideShowAFBtnExtra.Name = "btnHideShowAFBtnExtra";
        this.btnHideShowAFBtnExtra.Size = new System.Drawing.Size(188, 23);
        this.btnHideShowAFBtnExtra.TabIndex = 4;
        this.btnHideShowAFBtnExtra.Text = "Hide/Show AF-Button";
        this.btnHideShowAFBtnExtra.UseVisualStyleBackColor = true;
        this.btnHideShowAFBtnExtra.Click += new System.EventHandler(this.BtnHideShowAFBtnClick);
        // 
        // btnHideShowKFTOBtnExtra
        // 
        this.btnHideShowKFTOBtnExtra.Location = new System.Drawing.Point(6, 46);
        this.btnHideShowKFTOBtnExtra.Name = "btnHideShowKFTOBtnExtra";
        this.btnHideShowKFTOBtnExtra.Size = new System.Drawing.Size(188, 23);
        this.btnHideShowKFTOBtnExtra.TabIndex = 5;
        this.btnHideShowKFTOBtnExtra.Text = "Hide/Show KFTO-Button";
        this.btnHideShowKFTOBtnExtra.UseVisualStyleBackColor = true;
        this.btnHideShowKFTOBtnExtra.Click += new System.EventHandler(this.BtnHideShowKFTOBtnClick);
        // 
        // groupBox4
        // 
        this.groupBox4.Controls.Add(this.btnHideShowFIAOLblStd);
        this.groupBox4.Controls.Add(this.btnHideShowAFBtnStd);
        this.groupBox4.Controls.Add(this.btnHideShowKFTOBtnStd);
        this.groupBox4.Location = new System.Drawing.Point(17, 19);
        this.groupBox4.Name = "groupBox4";
        this.groupBox4.Size = new System.Drawing.Size(200, 108);
        this.groupBox4.TabIndex = 0;
        this.groupBox4.TabStop = false;
        this.groupBox4.Text = "\'Standard\' Filter Panel";
        // 
        // FilterFindTest
        // 
        this.AcceptButton = this.btnTestFullConstructor;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoSize = true;
        this.ClientSize = new System.Drawing.Size(985, 500);
        this.Controls.Add(this.groupBox3);
        this.Controls.Add(this.groupBox2);
        this.Controls.Add(this.FUcoFilterAndFind);
        this.Controls.Add(this.btnTestDefaultConstructor);
        this.Name = "FilterFindTest";
        this.Text = "Find/Filter Test Form Window";
        this.groupBox2.ResumeLayout(false);
        this.groupBox2.PerformLayout();
        this.groupBox5.ResumeLayout(false);
        this.pnlYear.ResumeLayout(false);
        this.pnlCurrencyName.ResumeLayout(false);
        this.pnlCurrencyName.PerformLayout();
        this.pnlCurrencyCode.ResumeLayout(false);
        this.pnlCurrencyCode.PerformLayout();
        this.panel1.ResumeLayout(false);
        this.grpExtraFilterPanel.ResumeLayout(false);
        this.grpStandardFilterPanel.ResumeLayout(false);
        this.groupBox3.ResumeLayout(false);
        this.groupBox1.ResumeLayout(false);
        this.groupBox4.ResumeLayout(false);
        this.ResumeLayout(false);
    }
    private System.Windows.Forms.CheckBox chkCurrencyCodeStd;
    private System.Windows.Forms.CheckBox chkCurrencyCodeFind;
    private System.Windows.Forms.CheckBox chkCurrencyCodeExtra;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label lblCurrencyName;
    private System.Windows.Forms.TextBox txtCurrencyName;
    private System.Windows.Forms.Button btnUnsetCurrencyName;
    private System.Windows.Forms.Panel pnlCurrencyName;
    private System.Windows.Forms.CheckBox chkCurrencyNameStd;
    private System.Windows.Forms.CheckBox chkCurrencyNameFind;
    private System.Windows.Forms.CheckBox chkCurrencyNameExtra;
    private System.Windows.Forms.Label lblYear;
    private System.Windows.Forms.ComboBox cmbYear;
    private System.Windows.Forms.Panel pnlYear;
    private System.Windows.Forms.CheckBox chkYearStd;
    private System.Windows.Forms.CheckBox chkYearFind;
    private System.Windows.Forms.CheckBox chkYearExtra;
    private System.Windows.Forms.Label lblCurrencyCode;
    private System.Windows.Forms.TextBox txtCurrencyCode;
    private System.Windows.Forms.Button btnUnsetCurrencyCode;
    private System.Windows.Forms.Panel pnlCurrencyCode;
    private System.Windows.Forms.GroupBox groupBox5;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button btnCollapseExpandPanel;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtControlWidth;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.Button btnHideShowKFTOBtnExtra;
    private System.Windows.Forms.Button btnHideShowAFBtnExtra;
    private System.Windows.Forms.Button btnHideShowFIAOLblExtra;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.CheckBox btnApplyFilterButtonExtra;
    private System.Windows.Forms.CheckBox btnKeepFilterTurnedOnButtonExtra;
    private System.Windows.Forms.CheckBox btnFilterIsAlwaysOnLabelExtra;
    private System.Windows.Forms.GroupBox grpExtraFilterPanel;
    private System.Windows.Forms.CheckBox btnFilterIsAlwaysOnLabelStd;
    private System.Windows.Forms.RadioButton rbtOneFilterPanel;
    private System.Windows.Forms.RadioButton rbtTwoFilterPanels;
    private System.Windows.Forms.CheckBox btnApplyFilterButtonStd;
    private System.Windows.Forms.CheckBox btnKeepFilterTurnedOnButtonStd;
    private System.Windows.Forms.GroupBox grpStandardFilterPanel;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.CheckBox chkShowFindTab;
    private System.Windows.Forms.GroupBox groupBox2;
    private Ict.Common.Controls.TUcoFilterAndFind FUcoFilterAndFind;

    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Button btnHideShowFIAOLblStd;
    private System.Windows.Forms.Button btnHideShowAFBtnStd;
    private System.Windows.Forms.Button btnHideShowKFTOBtnStd;
    private System.Windows.Forms.Button btnTestFullConstructor;
    private System.Windows.Forms.Button btnTestDefaultConstructor;
    
}
}