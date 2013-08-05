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
        this.tabControl1 = new System.Windows.Forms.TabControl();
        this.tabPage1 = new System.Windows.Forms.TabPage();
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
        this.txtCurrencyCode = new System.Windows.Forms.TextBox();
        this.lblCurrencyCode = new System.Windows.Forms.Label();
        this.tabPage2 = new System.Windows.Forms.TabPage();
        this.groupBox7 = new System.Windows.Forms.GroupBox();
        this.chkDynCtrl2 = new System.Windows.Forms.CheckBox();
        this.txtLblDynCtrl2 = new System.Windows.Forms.TextBox();
        this.groupBox6 = new System.Windows.Forms.GroupBox();
        this.lblDynCtrl1 = new System.Windows.Forms.Label();
        this.txtDynCtrl1 = new System.Windows.Forms.TextBox();
        this.chkDynamicCtrl2Extra = new System.Windows.Forms.CheckBox();
        this.chkDynamicCtrl2Find = new System.Windows.Forms.CheckBox();
        this.chkDynamicCtrl2Std = new System.Windows.Forms.CheckBox();
        this.label5 = new System.Windows.Forms.Label();
        this.label6 = new System.Windows.Forms.Label();
        this.label9 = new System.Windows.Forms.Label();
        this.label10 = new System.Windows.Forms.Label();
        this.chkDynamicCtrl1Extra = new System.Windows.Forms.CheckBox();
        this.chkDynamicCtrl1Find = new System.Windows.Forms.CheckBox();
        this.chkDynamicCtrl1Std = new System.Windows.Forms.CheckBox();
        this.tabPage3 = new System.Windows.Forms.TabPage();
        this.groupBox8 = new System.Windows.Forms.GroupBox();
        this.cmbDynCtrl3 = new Ict.Common.Controls.TCmbAutoComplete();
        this.lblDynCtrl3 = new System.Windows.Forms.Label();
        this.label12 = new System.Windows.Forms.Label();
        this.label13 = new System.Windows.Forms.Label();
        this.label14 = new System.Windows.Forms.Label();
        this.label15 = new System.Windows.Forms.Label();
        this.chkDynamicCtrl3Extra = new System.Windows.Forms.CheckBox();
        this.chkDynamicCtrl3Find = new System.Windows.Forms.CheckBox();
        this.chkDynamicCtrl3Std = new System.Windows.Forms.CheckBox();
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
        this.groupBox3 = new System.Windows.Forms.GroupBox();
        this.btnAllowedToSetFilterToInactive = new System.Windows.Forms.Button();
        this.button1 = new System.Windows.Forms.Button();
        this.btnFocusFirstArgumentControl = new System.Windows.Forms.Button();
        this.btnCollapseExpandPanel = new System.Windows.Forms.Button();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.btnHideShowFIAOLblExtra = new System.Windows.Forms.Button();
        this.btnHideShowAFBtnExtra = new System.Windows.Forms.Button();
        this.btnHideShowKFTOBtnExtra = new System.Windows.Forms.Button();
        this.groupBox4 = new System.Windows.Forms.GroupBox();
        this.groupBox9 = new System.Windows.Forms.GroupBox();
        this.txtEventsLog = new System.Windows.Forms.TextBox();
        this.groupBox2.SuspendLayout();
        this.groupBox5.SuspendLayout();
        this.tabControl1.SuspendLayout();
        this.tabPage1.SuspendLayout();
        this.pnlYear.SuspendLayout();
        this.pnlCurrencyName.SuspendLayout();
        this.pnlCurrencyCode.SuspendLayout();
        this.tabPage2.SuspendLayout();
        this.groupBox7.SuspendLayout();
        this.groupBox6.SuspendLayout();
        this.tabPage3.SuspendLayout();
        this.groupBox8.SuspendLayout();
        this.panel1.SuspendLayout();
        this.grpExtraFilterPanel.SuspendLayout();
        this.grpStandardFilterPanel.SuspendLayout();
        this.groupBox3.SuspendLayout();
        this.groupBox1.SuspendLayout();
        this.groupBox4.SuspendLayout();
        this.groupBox9.SuspendLayout();
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
        this.btnTestFullConstructor.Location = new System.Drawing.Point(6, 213);
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
        this.FUcoFilterAndFind.ShowApplyFilterButton = Ict.Common.Controls.TUcoFilterAndFind.FilterContext.None;
        this.FUcoFilterAndFind.ShowFilterIsAlwaysOnLabel = Ict.Common.Controls.TUcoFilterAndFind.FilterContext.None;
        this.FUcoFilterAndFind.ShowKeepFilterTurnedOnButton = Ict.Common.Controls.TUcoFilterAndFind.FilterContext.None;
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
        this.groupBox2.Controls.Add(this.btnTestFullConstructor);
        this.groupBox2.Location = new System.Drawing.Point(191, 50);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Size = new System.Drawing.Size(782, 248);
        this.groupBox2.TabIndex = 9;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "Full Constructor Options";
        // 
        // groupBox5
        // 
        this.groupBox5.Controls.Add(this.tabControl1);
        this.groupBox5.Location = new System.Drawing.Point(476, 16);
        this.groupBox5.Name = "groupBox5";
        this.groupBox5.Size = new System.Drawing.Size(300, 220);
        this.groupBox5.TabIndex = 7;
        this.groupBox5.TabStop = false;
        this.groupBox5.Text = "Controls for Panels";
        // 
        // tabControl1
        // 
        this.tabControl1.Controls.Add(this.tabPage1);
        this.tabControl1.Controls.Add(this.tabPage2);
        this.tabControl1.Controls.Add(this.tabPage3);
        this.tabControl1.Location = new System.Drawing.Point(6, 19);
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Size = new System.Drawing.Size(288, 194);
        this.tabControl1.TabIndex = 16;
        // 
        // tabPage1
        // 
        this.tabPage1.Controls.Add(this.chkYearExtra);
        this.tabPage1.Controls.Add(this.chkYearFind);
        this.tabPage1.Controls.Add(this.chkYearStd);
        this.tabPage1.Controls.Add(this.pnlYear);
        this.tabPage1.Controls.Add(this.chkCurrencyNameExtra);
        this.tabPage1.Controls.Add(this.chkCurrencyNameFind);
        this.tabPage1.Controls.Add(this.chkCurrencyNameStd);
        this.tabPage1.Controls.Add(this.pnlCurrencyName);
        this.tabPage1.Controls.Add(this.label8);
        this.tabPage1.Controls.Add(this.label7);
        this.tabPage1.Controls.Add(this.label4);
        this.tabPage1.Controls.Add(this.label3);
        this.tabPage1.Controls.Add(this.chkCurrencyCodeExtra);
        this.tabPage1.Controls.Add(this.chkCurrencyCodeFind);
        this.tabPage1.Controls.Add(this.chkCurrencyCodeStd);
        this.tabPage1.Controls.Add(this.pnlCurrencyCode);
        this.tabPage1.Location = new System.Drawing.Point(4, 22);
        this.tabPage1.Name = "tabPage1";
        this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage1.Size = new System.Drawing.Size(280, 168);
        this.tabPage1.TabIndex = 0;
        this.tabPage1.Text = "Panel Instances";
        this.tabPage1.UseVisualStyleBackColor = true;
        // 
        // chkYearExtra
        // 
        this.chkYearExtra.Checked = true;
        this.chkYearExtra.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkYearExtra.Location = new System.Drawing.Point(209, 142);
        this.chkYearExtra.Name = "chkYearExtra";
        this.chkYearExtra.Size = new System.Drawing.Size(18, 18);
        this.chkYearExtra.TabIndex = 31;
        this.chkYearExtra.UseVisualStyleBackColor = true;
        // 
        // chkYearFind
        // 
        this.chkYearFind.Location = new System.Drawing.Point(249, 142);
        this.chkYearFind.Name = "chkYearFind";
        this.chkYearFind.Size = new System.Drawing.Size(18, 18);
        this.chkYearFind.TabIndex = 30;
        this.chkYearFind.UseVisualStyleBackColor = true;
        // 
        // chkYearStd
        // 
        this.chkYearStd.Location = new System.Drawing.Point(165, 142);
        this.chkYearStd.Name = "chkYearStd";
        this.chkYearStd.Size = new System.Drawing.Size(18, 18);
        this.chkYearStd.TabIndex = 29;
        this.chkYearStd.UseVisualStyleBackColor = true;
        // 
        // pnlYear
        // 
        this.pnlYear.BackColor = System.Drawing.Color.Gold;
        this.pnlYear.Controls.Add(this.cmbYear);
        this.pnlYear.Controls.Add(this.lblYear);
        this.pnlYear.Location = new System.Drawing.Point(4, 122);
        this.pnlYear.Name = "pnlYear";
        this.pnlYear.Size = new System.Drawing.Size(133, 43);
        this.pnlYear.TabIndex = 22;
        this.pnlYear.Tag = "NoAutomaticArgumentClearButton";
        // 
        // cmbYear
        // 
        this.cmbYear.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.cmbYear.FormattingEnabled = true;
        this.cmbYear.Items.AddRange(new object[] {
                        "",
                        "2012",
                        "2013"});
        this.cmbYear.Location = new System.Drawing.Point(3, 19);
        this.cmbYear.Name = "cmbYear";
        this.cmbYear.Size = new System.Drawing.Size(127, 21);
        this.cmbYear.TabIndex = 1;
        this.cmbYear.Tag = "ClearValue=2";
        this.cmbYear.Text = "2012";
        // 
        // lblYear
        // 
        this.lblYear.AutoSize = true;
        this.lblYear.Font = new System.Drawing.Font("Verdana", 7F);
        this.lblYear.Location = new System.Drawing.Point(3, 0);
        this.lblYear.Name = "lblYear";
        this.lblYear.Size = new System.Drawing.Size(35, 12);
        this.lblYear.TabIndex = 0;
        this.lblYear.Text = "&Year:";
        // 
        // chkCurrencyNameExtra
        // 
        this.chkCurrencyNameExtra.Location = new System.Drawing.Point(209, 91);
        this.chkCurrencyNameExtra.Name = "chkCurrencyNameExtra";
        this.chkCurrencyNameExtra.Size = new System.Drawing.Size(18, 18);
        this.chkCurrencyNameExtra.TabIndex = 28;
        this.chkCurrencyNameExtra.UseVisualStyleBackColor = true;
        // 
        // chkCurrencyNameFind
        // 
        this.chkCurrencyNameFind.Location = new System.Drawing.Point(249, 91);
        this.chkCurrencyNameFind.Name = "chkCurrencyNameFind";
        this.chkCurrencyNameFind.Size = new System.Drawing.Size(18, 18);
        this.chkCurrencyNameFind.TabIndex = 27;
        this.chkCurrencyNameFind.UseVisualStyleBackColor = true;
        // 
        // chkCurrencyNameStd
        // 
        this.chkCurrencyNameStd.Checked = true;
        this.chkCurrencyNameStd.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkCurrencyNameStd.Location = new System.Drawing.Point(165, 91);
        this.chkCurrencyNameStd.Name = "chkCurrencyNameStd";
        this.chkCurrencyNameStd.Size = new System.Drawing.Size(18, 18);
        this.chkCurrencyNameStd.TabIndex = 26;
        this.chkCurrencyNameStd.UseVisualStyleBackColor = true;
        // 
        // pnlCurrencyName
        // 
        this.pnlCurrencyName.BackColor = System.Drawing.Color.DarkSalmon;
        this.pnlCurrencyName.Controls.Add(this.txtCurrencyName);
        this.pnlCurrencyName.Controls.Add(this.lblCurrencyName);
        this.pnlCurrencyName.Location = new System.Drawing.Point(4, 73);
        this.pnlCurrencyName.Name = "pnlCurrencyName";
        this.pnlCurrencyName.Size = new System.Drawing.Size(133, 43);
        this.pnlCurrencyName.TabIndex = 19;
        this.pnlCurrencyName.Tag = "KeepBackColour";
        // 
        // txtCurrencyName
        // 
        this.txtCurrencyName.Font = new System.Drawing.Font("Verdana", 8F);
        this.txtCurrencyName.Location = new System.Drawing.Point(3, 17);
        this.txtCurrencyName.Name = "txtCurrencyName";
        this.txtCurrencyName.Size = new System.Drawing.Size(79, 20);
        this.txtCurrencyName.TabIndex = 1;
        // 
        // lblCurrencyName
        // 
        this.lblCurrencyName.AutoSize = true;
        this.lblCurrencyName.Font = new System.Drawing.Font("Verdana", 7F);
        this.lblCurrencyName.Location = new System.Drawing.Point(3, 0);
        this.lblCurrencyName.Name = "lblCurrencyName";
        this.lblCurrencyName.Size = new System.Drawing.Size(94, 12);
        this.lblCurrencyName.TabIndex = 0;
        this.lblCurrencyName.Text = "Cu&rrency Name:";
        // 
        // label8
        // 
        this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label8.Location = new System.Drawing.Point(239, 16);
        this.label8.Name = "label8";
        this.label8.Size = new System.Drawing.Size(37, 23);
        this.label8.TabIndex = 25;
        this.label8.Text = "Find";
        // 
        // label7
        // 
        this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label7.Location = new System.Drawing.Point(194, 16);
        this.label7.Name = "label7";
        this.label7.Size = new System.Drawing.Size(48, 23);
        this.label7.TabIndex = 24;
        this.label7.Text = "Extra";
        // 
        // label4
        // 
        this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label4.Location = new System.Drawing.Point(161, 16);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(27, 23);
        this.label4.TabIndex = 23;
        this.label4.Text = "Std.";
        // 
        // label3
        // 
        this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label3.Location = new System.Drawing.Point(157, 0);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(120, 23);
        this.label3.TabIndex = 21;
        this.label3.Text = "Should Appear On...";
        this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // chkCurrencyCodeExtra
        // 
        this.chkCurrencyCodeExtra.Location = new System.Drawing.Point(209, 42);
        this.chkCurrencyCodeExtra.Name = "chkCurrencyCodeExtra";
        this.chkCurrencyCodeExtra.Size = new System.Drawing.Size(18, 18);
        this.chkCurrencyCodeExtra.TabIndex = 20;
        this.chkCurrencyCodeExtra.UseVisualStyleBackColor = true;
        // 
        // chkCurrencyCodeFind
        // 
        this.chkCurrencyCodeFind.Location = new System.Drawing.Point(249, 42);
        this.chkCurrencyCodeFind.Name = "chkCurrencyCodeFind";
        this.chkCurrencyCodeFind.Size = new System.Drawing.Size(18, 18);
        this.chkCurrencyCodeFind.TabIndex = 18;
        this.chkCurrencyCodeFind.UseVisualStyleBackColor = true;
        // 
        // chkCurrencyCodeStd
        // 
        this.chkCurrencyCodeStd.Checked = true;
        this.chkCurrencyCodeStd.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkCurrencyCodeStd.Location = new System.Drawing.Point(165, 42);
        this.chkCurrencyCodeStd.Name = "chkCurrencyCodeStd";
        this.chkCurrencyCodeStd.Size = new System.Drawing.Size(18, 18);
        this.chkCurrencyCodeStd.TabIndex = 17;
        this.chkCurrencyCodeStd.UseVisualStyleBackColor = true;
        // 
        // pnlCurrencyCode
        // 
        this.pnlCurrencyCode.BackColor = System.Drawing.Color.DarkGray;
        this.pnlCurrencyCode.Controls.Add(this.txtCurrencyCode);
        this.pnlCurrencyCode.Controls.Add(this.lblCurrencyCode);
        this.pnlCurrencyCode.Location = new System.Drawing.Point(4, 24);
        this.pnlCurrencyCode.Name = "pnlCurrencyCode";
        this.pnlCurrencyCode.Size = new System.Drawing.Size(133, 43);
        this.pnlCurrencyCode.TabIndex = 16;
        this.pnlCurrencyCode.Tag = "KeepBackColour";
        // 
        // txtCurrencyCode
        // 
        this.txtCurrencyCode.Font = new System.Drawing.Font("Verdana", 8F);
        this.txtCurrencyCode.Location = new System.Drawing.Point(3, 17);
        this.txtCurrencyCode.Name = "txtCurrencyCode";
        this.txtCurrencyCode.Size = new System.Drawing.Size(79, 20);
        this.txtCurrencyCode.TabIndex = 1;
        // 
        // lblCurrencyCode
        // 
        this.lblCurrencyCode.AutoSize = true;
        this.lblCurrencyCode.Font = new System.Drawing.Font("Verdana", 7F);
        this.lblCurrencyCode.Location = new System.Drawing.Point(3, 0);
        this.lblCurrencyCode.Name = "lblCurrencyCode";
        this.lblCurrencyCode.Size = new System.Drawing.Size(91, 12);
        this.lblCurrencyCode.TabIndex = 0;
        this.lblCurrencyCode.Text = "C&urrency Code:";
        // 
        // tabPage2
        // 
        this.tabPage2.Controls.Add(this.groupBox7);
        this.tabPage2.Controls.Add(this.groupBox6);
        this.tabPage2.Controls.Add(this.chkDynamicCtrl2Extra);
        this.tabPage2.Controls.Add(this.chkDynamicCtrl2Find);
        this.tabPage2.Controls.Add(this.chkDynamicCtrl2Std);
        this.tabPage2.Controls.Add(this.label5);
        this.tabPage2.Controls.Add(this.label6);
        this.tabPage2.Controls.Add(this.label9);
        this.tabPage2.Controls.Add(this.label10);
        this.tabPage2.Controls.Add(this.chkDynamicCtrl1Extra);
        this.tabPage2.Controls.Add(this.chkDynamicCtrl1Find);
        this.tabPage2.Controls.Add(this.chkDynamicCtrl1Std);
        this.tabPage2.Location = new System.Drawing.Point(4, 22);
        this.tabPage2.Name = "tabPage2";
        this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
        this.tabPage2.Size = new System.Drawing.Size(280, 168);
        this.tabPage2.TabIndex = 1;
        this.tabPage2.Text = "Control Instances #1";
        this.tabPage2.UseVisualStyleBackColor = true;
        // 
        // groupBox7
        // 
        this.groupBox7.BackColor = System.Drawing.Color.YellowGreen;
        this.groupBox7.Controls.Add(this.chkDynCtrl2);
        this.groupBox7.Controls.Add(this.txtLblDynCtrl2);
        this.groupBox7.Location = new System.Drawing.Point(10, 91);
        this.groupBox7.Name = "groupBox7";
        this.groupBox7.Size = new System.Drawing.Size(149, 64);
        this.groupBox7.TabIndex = 46;
        this.groupBox7.TabStop = false;
        this.groupBox7.Text = "DynamicCtrl2";
        // 
        // chkDynCtrl2
        // 
        this.chkDynCtrl2.AutoSize = true;
        this.chkDynCtrl2.Location = new System.Drawing.Point(6, 34);
        this.chkDynCtrl2.Name = "chkDynCtrl2";
        this.chkDynCtrl2.Size = new System.Drawing.Size(85, 17);
        this.chkDynCtrl2.TabIndex = 1;
        this.chkDynCtrl2.Tag = "ClearValue=true";
        this.chkDynCtrl2.Text = "ChkDynCtrl2";
        this.chkDynCtrl2.UseVisualStyleBackColor = true;
        // 
        // txtLblDynCtrl2
        // 
        this.txtLblDynCtrl2.Location = new System.Drawing.Point(43, 17);
        this.txtLblDynCtrl2.Name = "txtLblDynCtrl2";
        this.txtLblDynCtrl2.Size = new System.Drawing.Size(100, 20);
        this.txtLblDynCtrl2.TabIndex = 0;
        this.txtLblDynCtrl2.Text = "DynCtrlLabe&l2:";
        // 
        // groupBox6
        // 
        this.groupBox6.BackColor = System.Drawing.Color.Violet;
        this.groupBox6.Controls.Add(this.lblDynCtrl1);
        this.groupBox6.Controls.Add(this.txtDynCtrl1);
        this.groupBox6.Location = new System.Drawing.Point(10, 21);
        this.groupBox6.Name = "groupBox6";
        this.groupBox6.Size = new System.Drawing.Size(149, 64);
        this.groupBox6.TabIndex = 45;
        this.groupBox6.TabStop = false;
        this.groupBox6.Text = "DynamicCtrl1";
        // 
        // lblDynCtrl1
        // 
        this.lblDynCtrl1.Location = new System.Drawing.Point(37, 34);
        this.lblDynCtrl1.Name = "lblDynCtrl1";
        this.lblDynCtrl1.Size = new System.Drawing.Size(90, 23);
        this.lblDynCtrl1.TabIndex = 1;
        this.lblDynCtrl1.Text = "DynCtrlLabel&1:";
        // 
        // txtDynCtrl1
        // 
        this.txtDynCtrl1.AutoCompleteCustomSource.AddRange(new string[] {
                        "MYTEXT",
                        "ALEX",
                        "PIA",
                        "JOSEPH",
                        "JOHN",
                        "MELISSA"});
        this.txtDynCtrl1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
        this.txtDynCtrl1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
        this.txtDynCtrl1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
        this.txtDynCtrl1.Location = new System.Drawing.Point(17, 15);
        this.txtDynCtrl1.Name = "txtDynCtrl1";
        this.txtDynCtrl1.Size = new System.Drawing.Size(59, 20);
        this.txtDynCtrl1.TabIndex = 0;
        this.txtDynCtrl1.Text = "MYTEXT";
        // 
        // chkDynamicCtrl2Extra
        // 
        this.chkDynamicCtrl2Extra.Checked = true;
        this.chkDynamicCtrl2Extra.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkDynamicCtrl2Extra.Location = new System.Drawing.Point(209, 118);
        this.chkDynamicCtrl2Extra.Name = "chkDynamicCtrl2Extra";
        this.chkDynamicCtrl2Extra.Size = new System.Drawing.Size(18, 18);
        this.chkDynamicCtrl2Extra.TabIndex = 41;
        this.chkDynamicCtrl2Extra.UseVisualStyleBackColor = true;
        // 
        // chkDynamicCtrl2Find
        // 
        this.chkDynamicCtrl2Find.Location = new System.Drawing.Point(249, 118);
        this.chkDynamicCtrl2Find.Name = "chkDynamicCtrl2Find";
        this.chkDynamicCtrl2Find.Size = new System.Drawing.Size(18, 18);
        this.chkDynamicCtrl2Find.TabIndex = 40;
        this.chkDynamicCtrl2Find.UseVisualStyleBackColor = true;
        // 
        // chkDynamicCtrl2Std
        // 
        this.chkDynamicCtrl2Std.Location = new System.Drawing.Point(165, 118);
        this.chkDynamicCtrl2Std.Name = "chkDynamicCtrl2Std";
        this.chkDynamicCtrl2Std.Size = new System.Drawing.Size(18, 18);
        this.chkDynamicCtrl2Std.TabIndex = 39;
        this.chkDynamicCtrl2Std.UseVisualStyleBackColor = true;
        // 
        // label5
        // 
        this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label5.Location = new System.Drawing.Point(239, 16);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(37, 23);
        this.label5.TabIndex = 38;
        this.label5.Text = "Find";
        // 
        // label6
        // 
        this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label6.Location = new System.Drawing.Point(194, 16);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(48, 23);
        this.label6.TabIndex = 37;
        this.label6.Text = "Extra";
        // 
        // label9
        // 
        this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label9.Location = new System.Drawing.Point(161, 16);
        this.label9.Name = "label9";
        this.label9.Size = new System.Drawing.Size(27, 23);
        this.label9.TabIndex = 36;
        this.label9.Text = "Std.";
        // 
        // label10
        // 
        this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label10.Location = new System.Drawing.Point(157, 0);
        this.label10.Name = "label10";
        this.label10.Size = new System.Drawing.Size(120, 23);
        this.label10.TabIndex = 35;
        this.label10.Text = "Should Appear On...";
        this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // chkDynamicCtrl1Extra
        // 
        this.chkDynamicCtrl1Extra.Location = new System.Drawing.Point(209, 47);
        this.chkDynamicCtrl1Extra.Name = "chkDynamicCtrl1Extra";
        this.chkDynamicCtrl1Extra.Size = new System.Drawing.Size(18, 18);
        this.chkDynamicCtrl1Extra.TabIndex = 34;
        this.chkDynamicCtrl1Extra.UseVisualStyleBackColor = true;
        // 
        // chkDynamicCtrl1Find
        // 
        this.chkDynamicCtrl1Find.Location = new System.Drawing.Point(249, 47);
        this.chkDynamicCtrl1Find.Name = "chkDynamicCtrl1Find";
        this.chkDynamicCtrl1Find.Size = new System.Drawing.Size(18, 18);
        this.chkDynamicCtrl1Find.TabIndex = 33;
        this.chkDynamicCtrl1Find.UseVisualStyleBackColor = true;
        // 
        // chkDynamicCtrl1Std
        // 
        this.chkDynamicCtrl1Std.Checked = true;
        this.chkDynamicCtrl1Std.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkDynamicCtrl1Std.Location = new System.Drawing.Point(165, 47);
        this.chkDynamicCtrl1Std.Name = "chkDynamicCtrl1Std";
        this.chkDynamicCtrl1Std.Size = new System.Drawing.Size(18, 18);
        this.chkDynamicCtrl1Std.TabIndex = 32;
        this.chkDynamicCtrl1Std.UseVisualStyleBackColor = true;
        // 
        // tabPage3
        // 
        this.tabPage3.Controls.Add(this.groupBox8);
        this.tabPage3.Controls.Add(this.label12);
        this.tabPage3.Controls.Add(this.label13);
        this.tabPage3.Controls.Add(this.label14);
        this.tabPage3.Controls.Add(this.label15);
        this.tabPage3.Controls.Add(this.chkDynamicCtrl3Extra);
        this.tabPage3.Controls.Add(this.chkDynamicCtrl3Find);
        this.tabPage3.Controls.Add(this.chkDynamicCtrl3Std);
        this.tabPage3.Location = new System.Drawing.Point(4, 22);
        this.tabPage3.Name = "tabPage3";
        this.tabPage3.Size = new System.Drawing.Size(280, 168);
        this.tabPage3.TabIndex = 2;
        this.tabPage3.Text = "Control Instances #2";
        this.tabPage3.UseVisualStyleBackColor = true;
        // 
        // groupBox8
        // 
        this.groupBox8.BackColor = System.Drawing.Color.Tomato;
        this.groupBox8.Controls.Add(this.cmbDynCtrl3);
        this.groupBox8.Controls.Add(this.lblDynCtrl3);
        this.groupBox8.Location = new System.Drawing.Point(10, 21);
        this.groupBox8.Name = "groupBox8";
        this.groupBox8.Size = new System.Drawing.Size(149, 64);
        this.groupBox8.TabIndex = 53;
        this.groupBox8.TabStop = false;
        this.groupBox8.Text = "DynamicCtrl3";
        // 
        // cmbDynCtrl3
        // 
        this.cmbDynCtrl3.AcceptNewValues = true;
        this.cmbDynCtrl3.CaseSensitiveSearch = true;
        this.cmbDynCtrl3.ColumnsToSearch = "#VALUE#, #DISPLAY#";
        this.cmbDynCtrl3.DescriptionMember = null;
        this.cmbDynCtrl3.FormattingEnabled = true;
        this.cmbDynCtrl3.Items.AddRange(new object[] {
                        "One",
                        "Two",
                        "Three",
                        "Four",
                        "Five",
                        "Six",
                        " "});
        this.cmbDynCtrl3.Location = new System.Drawing.Point(6, 37);
        this.cmbDynCtrl3.MaxDropDownItems = 3;
        this.cmbDynCtrl3.Name = "cmbDynCtrl3";
        this.cmbDynCtrl3.Size = new System.Drawing.Size(121, 21);
        this.cmbDynCtrl3.SuppressSelectionColor = true;
        this.cmbDynCtrl3.TabIndex = 2;
        this.cmbDynCtrl3.Tag = "ClearValue=6";
        // 
        // lblDynCtrl3
        // 
        this.lblDynCtrl3.Location = new System.Drawing.Point(37, 16);
        this.lblDynCtrl3.Name = "lblDynCtrl3";
        this.lblDynCtrl3.Size = new System.Drawing.Size(90, 23);
        this.lblDynCtrl3.TabIndex = 1;
        this.lblDynCtrl3.Text = "DynCtrlLabel&3:";
        // 
        // label12
        // 
        this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label12.Location = new System.Drawing.Point(239, 16);
        this.label12.Name = "label12";
        this.label12.Size = new System.Drawing.Size(37, 23);
        this.label12.TabIndex = 52;
        this.label12.Text = "Find";
        // 
        // label13
        // 
        this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label13.Location = new System.Drawing.Point(194, 16);
        this.label13.Name = "label13";
        this.label13.Size = new System.Drawing.Size(48, 23);
        this.label13.TabIndex = 51;
        this.label13.Text = "Extra";
        // 
        // label14
        // 
        this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label14.Location = new System.Drawing.Point(161, 16);
        this.label14.Name = "label14";
        this.label14.Size = new System.Drawing.Size(27, 23);
        this.label14.TabIndex = 50;
        this.label14.Text = "Std.";
        // 
        // label15
        // 
        this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label15.Location = new System.Drawing.Point(157, 0);
        this.label15.Name = "label15";
        this.label15.Size = new System.Drawing.Size(120, 23);
        this.label15.TabIndex = 49;
        this.label15.Text = "Should Appear On...";
        this.label15.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // chkDynamicCtrl3Extra
        // 
        this.chkDynamicCtrl3Extra.Location = new System.Drawing.Point(209, 47);
        this.chkDynamicCtrl3Extra.Name = "chkDynamicCtrl3Extra";
        this.chkDynamicCtrl3Extra.Size = new System.Drawing.Size(18, 18);
        this.chkDynamicCtrl3Extra.TabIndex = 48;
        this.chkDynamicCtrl3Extra.UseVisualStyleBackColor = true;
        // 
        // chkDynamicCtrl3Find
        // 
        this.chkDynamicCtrl3Find.Location = new System.Drawing.Point(249, 47);
        this.chkDynamicCtrl3Find.Name = "chkDynamicCtrl3Find";
        this.chkDynamicCtrl3Find.Size = new System.Drawing.Size(18, 18);
        this.chkDynamicCtrl3Find.TabIndex = 47;
        this.chkDynamicCtrl3Find.UseVisualStyleBackColor = true;
        // 
        // chkDynamicCtrl3Std
        // 
        this.chkDynamicCtrl3Std.Checked = true;
        this.chkDynamicCtrl3Std.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkDynamicCtrl3Std.Location = new System.Drawing.Point(165, 47);
        this.chkDynamicCtrl3Std.Name = "chkDynamicCtrl3Std";
        this.chkDynamicCtrl3Std.Size = new System.Drawing.Size(18, 18);
        this.chkDynamicCtrl3Std.TabIndex = 46;
        this.chkDynamicCtrl3Std.UseVisualStyleBackColor = true;
        // 
        // label2
        // 
        this.label2.Location = new System.Drawing.Point(304, 180);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(32, 23);
        this.label2.TabIndex = 6;
        this.label2.Text = "(px)";
        // 
        // txtControlWidth
        // 
        this.txtControlWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtControlWidth.Location = new System.Drawing.Point(254, 178);
        this.txtControlWidth.Name = "txtControlWidth";
        this.txtControlWidth.Size = new System.Drawing.Size(44, 20);
        this.txtControlWidth.TabIndex = 5;
        this.txtControlWidth.Text = "175";
        this.txtControlWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        // 
        // label1
        // 
        this.label1.Location = new System.Drawing.Point(148, 181);
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
        // groupBox3
        // 
        this.groupBox3.Controls.Add(this.btnAllowedToSetFilterToInactive);
        this.groupBox3.Controls.Add(this.button1);
        this.groupBox3.Controls.Add(this.btnFocusFirstArgumentControl);
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
        // btnAllowedToSetFilterToInactive
        // 
        this.btnAllowedToSetFilterToInactive.Location = new System.Drawing.Point(254, 155);
        this.btnAllowedToSetFilterToInactive.Name = "btnAllowedToSetFilterToInactive";
        this.btnAllowedToSetFilterToInactive.Size = new System.Drawing.Size(188, 23);
        this.btnAllowedToSetFilterToInactive.TabIndex = 8;
        this.btnAllowedToSetFilterToInactive.Text = "Filter Allowed to be Inactive?";
        this.btnAllowedToSetFilterToInactive.UseVisualStyleBackColor = true;
        this.btnAllowedToSetFilterToInactive.Click += new System.EventHandler(this.BtnAllowedToSetFilterToInactiveClick);
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(254, 132);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(188, 23);
        this.button1.TabIndex = 8;
        this.button1.Text = "Show Find Tab";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.Button1Click);
        // 
        // btnFocusFirstArgumentControl
        // 
        this.btnFocusFirstArgumentControl.Location = new System.Drawing.Point(23, 155);
        this.btnFocusFirstArgumentControl.Name = "btnFocusFirstArgumentControl";
        this.btnFocusFirstArgumentControl.Size = new System.Drawing.Size(188, 23);
        this.btnFocusFirstArgumentControl.TabIndex = 7;
        this.btnFocusFirstArgumentControl.Text = "Focus First ArgumentControl";
        this.btnFocusFirstArgumentControl.UseVisualStyleBackColor = true;
        this.btnFocusFirstArgumentControl.Click += new System.EventHandler(this.BtnFocusFirstArgumentControlClick);
        // 
        // btnCollapseExpandPanel
        // 
        this.btnCollapseExpandPanel.Location = new System.Drawing.Point(23, 132);
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
        // groupBox9
        // 
        this.groupBox9.Controls.Add(this.txtEventsLog);
        this.groupBox9.Location = new System.Drawing.Point(667, 304);
        this.groupBox9.Name = "groupBox9";
        this.groupBox9.Size = new System.Drawing.Size(300, 184);
        this.groupBox9.TabIndex = 11;
        this.groupBox9.TabStop = false;
        this.groupBox9.Text = "Events Log";
        // 
        // txtEventsLog
        // 
        this.txtEventsLog.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtEventsLog.Location = new System.Drawing.Point(3, 16);
        this.txtEventsLog.Multiline = true;
        this.txtEventsLog.Name = "txtEventsLog";
        this.txtEventsLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtEventsLog.Size = new System.Drawing.Size(294, 165);
        this.txtEventsLog.TabIndex = 0;
        // 
        // FilterFindTest
        // 
        this.AcceptButton = this.btnTestFullConstructor;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoSize = true;
        this.ClientSize = new System.Drawing.Size(985, 500);
        this.Controls.Add(this.groupBox9);
        this.Controls.Add(this.groupBox3);
        this.Controls.Add(this.groupBox2);
        this.Controls.Add(this.FUcoFilterAndFind);
        this.Controls.Add(this.btnTestDefaultConstructor);
        this.Name = "FilterFindTest";
        this.Text = "Find/Filter Test Form Window";
        this.groupBox2.ResumeLayout(false);
        this.groupBox2.PerformLayout();
        this.groupBox5.ResumeLayout(false);
        this.tabControl1.ResumeLayout(false);
        this.tabPage1.ResumeLayout(false);
        this.pnlYear.ResumeLayout(false);
        this.pnlYear.PerformLayout();
        this.pnlCurrencyName.ResumeLayout(false);
        this.pnlCurrencyName.PerformLayout();
        this.pnlCurrencyCode.ResumeLayout(false);
        this.pnlCurrencyCode.PerformLayout();
        this.tabPage2.ResumeLayout(false);
        this.groupBox7.ResumeLayout(false);
        this.groupBox7.PerformLayout();
        this.groupBox6.ResumeLayout(false);
        this.groupBox6.PerformLayout();
        this.tabPage3.ResumeLayout(false);
        this.groupBox8.ResumeLayout(false);
        this.panel1.ResumeLayout(false);
        this.grpExtraFilterPanel.ResumeLayout(false);
        this.grpStandardFilterPanel.ResumeLayout(false);
        this.groupBox3.ResumeLayout(false);
        this.groupBox1.ResumeLayout(false);
        this.groupBox4.ResumeLayout(false);
        this.groupBox9.ResumeLayout(false);
        this.groupBox9.PerformLayout();
        this.ResumeLayout(false);
    }
    private System.Windows.Forms.Button btnAllowedToSetFilterToInactive;
    private System.Windows.Forms.TextBox txtEventsLog;
    private System.Windows.Forms.GroupBox groupBox9;
    private System.Windows.Forms.Button btnFocusFirstArgumentControl;
    private System.Windows.Forms.CheckBox chkDynamicCtrl3Std;
    private System.Windows.Forms.CheckBox chkDynamicCtrl3Find;
    private System.Windows.Forms.CheckBox chkDynamicCtrl3Extra;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label lblDynCtrl3;
    private Ict.Common.Controls.TCmbAutoComplete cmbDynCtrl3;
    private System.Windows.Forms.GroupBox groupBox8;
    private System.Windows.Forms.TabPage tabPage3;
    private System.Windows.Forms.TextBox txtDynCtrl1;
    private System.Windows.Forms.Label lblDynCtrl1;
    private System.Windows.Forms.GroupBox groupBox6;
    private System.Windows.Forms.TextBox txtLblDynCtrl2;
    private System.Windows.Forms.CheckBox chkDynCtrl2;
    private System.Windows.Forms.GroupBox groupBox7;
    private System.Windows.Forms.CheckBox chkDynamicCtrl1Std;
    private System.Windows.Forms.CheckBox chkDynamicCtrl1Find;
    private System.Windows.Forms.CheckBox chkDynamicCtrl1Extra;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.CheckBox chkDynamicCtrl2Std;
    private System.Windows.Forms.CheckBox chkDynamicCtrl2Find;
    private System.Windows.Forms.CheckBox chkDynamicCtrl2Extra;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.CheckBox chkCurrencyCodeStd;
    private System.Windows.Forms.CheckBox chkCurrencyCodeFind;
    private System.Windows.Forms.CheckBox chkCurrencyCodeExtra;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label lblCurrencyName;
    private System.Windows.Forms.TextBox txtCurrencyName;
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