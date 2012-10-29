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
namespace ControlTestBench
{
partial class TestAll
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
        this.btnChangeText = new System.Windows.Forms.Button();
        this.btnChangeCollapseDirection = new System.Windows.Forms.Button();
        this.btnChangeHostedControlKind = new System.Windows.Forms.Button();
        this.btnChangeUserControlString = new System.Windows.Forms.Button();
        this.btnChangeTaskListNode = new System.Windows.Forms.Button();
        this.btnChangeVisualStyle = new System.Windows.Forms.Button();
        this.txtChangeText = new System.Windows.Forms.TextBox();
        this.txtChangeUserControlString = new System.Windows.Forms.TextBox();
        this.cboChangeCollapseDirection = new System.Windows.Forms.ComboBox();
        this.cboChangeHostedControlKind = new System.Windows.Forms.ComboBox();
        this.cboChangeVisualStyle = new System.Windows.Forms.ComboBox();
        this.rtbChangeTaskListNode = new System.Windows.Forms.RichTextBox();
        this.txtChangeExpandedSize = new System.Windows.Forms.TextBox();
        this.btnChangeExpandedSize = new System.Windows.Forms.Button();
        this.btnTestEmptyConstructor = new System.Windows.Forms.Button();
        this.btnTestFullConstructor = new System.Windows.Forms.Button();
        this.btnTestUserControlVerticalConstructor = new System.Windows.Forms.Button();
        this.btnTestTaskListVerticalConstructor = new System.Windows.Forms.Button();
        this.btnTestTaskListExpandedConstructor = new System.Windows.Forms.Button();
        this.btnTestTaskListHorizontalConstructor = new System.Windows.Forms.Button();
        this.btnTestStacked = new System.Windows.Forms.Button();
        this.pnlContent = new System.Windows.Forms.Panel();
        this.sptContent = new System.Windows.Forms.SplitContainer();
        this.cplFolders = new Ict.Common.Controls.TPnlCollapsible();
        this.pnlFiller = new System.Windows.Forms.Panel();
        this.pnlTasksHoster = new System.Windows.Forms.Panel();
        this.pchTasks1 = new Ict.Common.Controls.TPnlCollapsibleHoster();
        this.pnlShepherdHoster = new System.Windows.Forms.Panel();
        this.cplShepherd = new Ict.Common.Controls.TPnlCollapsible();
        this.pnlTasksHoster2 = new System.Windows.Forms.Panel();
        this.cplTasks4 = new Ict.Common.Controls.TPnlCollapsible();
        this.cplTasks3 = new Ict.Common.Controls.TPnlCollapsible();
        this.cplPartnerInfo = new Ict.Common.Controls.TPnlCollapsible();
        this.cplRightCollapse = new Ict.Common.Controls.TPnlCollapsible();
        this.pnlContent.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.sptContent)).BeginInit();
        this.sptContent.Panel1.SuspendLayout();
        this.sptContent.Panel2.SuspendLayout();
        this.sptContent.SuspendLayout();
        this.pnlFiller.SuspendLayout();
        this.pnlTasksHoster.SuspendLayout();
        this.pnlShepherdHoster.SuspendLayout();
        this.pnlTasksHoster2.SuspendLayout();
        this.SuspendLayout();
        //
        // btnChangeText
        //
        this.btnChangeText.Location = new System.Drawing.Point(538, 262);
        this.btnChangeText.Name = "btnChangeText";
        this.btnChangeText.Size = new System.Drawing.Size(169, 23);
        this.btnChangeText.TabIndex = 0;
        this.btnChangeText.Text = "Change Text ( title && tooltip )";
        this.btnChangeText.UseVisualStyleBackColor = true;
        this.btnChangeText.Click += new System.EventHandler(this.ChangeText);
        //
        // btnChangeCollapseDirection
        //
        this.btnChangeCollapseDirection.Location = new System.Drawing.Point(538, 291);
        this.btnChangeCollapseDirection.Name = "btnChangeCollapseDirection";
        this.btnChangeCollapseDirection.Size = new System.Drawing.Size(169, 23);
        this.btnChangeCollapseDirection.TabIndex = 1;
        this.btnChangeCollapseDirection.Text = "Change Direction";
        this.btnChangeCollapseDirection.UseVisualStyleBackColor = true;
        this.btnChangeCollapseDirection.Click += new System.EventHandler(this.ChangeCollapseDirection);
        //
        // btnChangeHostedControlKind
        //
        this.btnChangeHostedControlKind.Location = new System.Drawing.Point(538, 320);
        this.btnChangeHostedControlKind.Name = "btnChangeHostedControlKind";
        this.btnChangeHostedControlKind.Size = new System.Drawing.Size(169, 23);
        this.btnChangeHostedControlKind.TabIndex = 2;
        this.btnChangeHostedControlKind.Text = "Change Hck";
        this.btnChangeHostedControlKind.UseVisualStyleBackColor = true;
        this.btnChangeHostedControlKind.Click += new System.EventHandler(this.ChangeHostedControlKind);
        //
        // btnChangeUserControlString
        //
        this.btnChangeUserControlString.Location = new System.Drawing.Point(538, 349);
        this.btnChangeUserControlString.Name = "btnChangeUserControlString";
        this.btnChangeUserControlString.Size = new System.Drawing.Size(169, 23);
        this.btnChangeUserControlString.TabIndex = 3;
        this.btnChangeUserControlString.Text = "Change UserControl String";
        this.btnChangeUserControlString.UseVisualStyleBackColor = true;
        this.btnChangeUserControlString.Click += new System.EventHandler(this.ChangeUserControlString);
        //
        // btnChangeTaskListNode
        //
        this.btnChangeTaskListNode.Location = new System.Drawing.Point(538, 407);
        this.btnChangeTaskListNode.Name = "btnChangeTaskListNode";
        this.btnChangeTaskListNode.Size = new System.Drawing.Size(169, 23);
        this.btnChangeTaskListNode.TabIndex = 4;
        this.btnChangeTaskListNode.Text = "Change TaskList Node";
        this.btnChangeTaskListNode.UseVisualStyleBackColor = true;
        this.btnChangeTaskListNode.Click += new System.EventHandler(this.ChangeTaskListNode);
        //
        // btnChangeVisualStyle
        //
        this.btnChangeVisualStyle.Location = new System.Drawing.Point(538, 378);
        this.btnChangeVisualStyle.Name = "btnChangeVisualStyle";
        this.btnChangeVisualStyle.Size = new System.Drawing.Size(169, 23);
        this.btnChangeVisualStyle.TabIndex = 5;
        this.btnChangeVisualStyle.Text = "Change Style";
        this.btnChangeVisualStyle.UseVisualStyleBackColor = true;
        this.btnChangeVisualStyle.Click += new System.EventHandler(this.ChangeVisualStyle);
        //
        // txtChangeText
        //
        this.txtChangeText.Location = new System.Drawing.Point(316, 264);
        this.txtChangeText.Name = "txtChangeText";
        this.txtChangeText.Size = new System.Drawing.Size(216, 20);
        this.txtChangeText.TabIndex = 7;
        //
        // txtChangeUserControlString
        //
        this.txtChangeUserControlString.Location = new System.Drawing.Point(316, 351);
        this.txtChangeUserControlString.Name = "txtChangeUserControlString";
        this.txtChangeUserControlString.Size = new System.Drawing.Size(216, 20);
        this.txtChangeUserControlString.TabIndex = 10;
        //
        // cboChangeCollapseDirection
        //
        this.cboChangeCollapseDirection.FormattingEnabled = true;
        this.cboChangeCollapseDirection.Items.AddRange(new object[] {
                "Horizontal",
                "Vertical"
            });
        this.cboChangeCollapseDirection.Location = new System.Drawing.Point(316, 293);
        this.cboChangeCollapseDirection.Name = "cboChangeCollapseDirection";
        this.cboChangeCollapseDirection.Size = new System.Drawing.Size(216, 21);
        this.cboChangeCollapseDirection.TabIndex = 14;
        //
        // cboChangeHostedControlKind
        //
        this.cboChangeHostedControlKind.FormattingEnabled = true;
        this.cboChangeHostedControlKind.Items.AddRange(new object[] {
                "Task List",
                "UserControl"
            });
        this.cboChangeHostedControlKind.Location = new System.Drawing.Point(316, 322);
        this.cboChangeHostedControlKind.Name = "cboChangeHostedControlKind";
        this.cboChangeHostedControlKind.Size = new System.Drawing.Size(216, 21);
        this.cboChangeHostedControlKind.TabIndex = 15;
        //
        // cboChangeVisualStyle
        //
        this.cboChangeVisualStyle.FormattingEnabled = true;
        this.cboChangeVisualStyle.Items.AddRange(new object[] {
                "vsTaskPanel",
                "vsAccordionPanel",
                "vsDashboard",
                "vsShepherd",
                "vsHorizontalCollapse"
            });
        this.cboChangeVisualStyle.Location = new System.Drawing.Point(316, 380);
        this.cboChangeVisualStyle.Name = "cboChangeVisualStyle";
        this.cboChangeVisualStyle.Size = new System.Drawing.Size(216, 21);
        this.cboChangeVisualStyle.TabIndex = 16;
        //
        // rtbChangeTaskListNode
        //
        this.rtbChangeTaskListNode.Location = new System.Drawing.Point(316, 409);
        this.rtbChangeTaskListNode.Name = "rtbChangeTaskListNode";
        this.rtbChangeTaskListNode.Size = new System.Drawing.Size(216, 105);
        this.rtbChangeTaskListNode.TabIndex = 17;
        this.rtbChangeTaskListNode.Text = "TaskGroup:\n    Task1:\n        Label: Type yml here; then hit button.";
        //
        // txtChangeExpandedSize
        //
        this.txtChangeExpandedSize.Location = new System.Drawing.Point(316, 235);
        this.txtChangeExpandedSize.Name = "txtChangeExpandedSize";
        this.txtChangeExpandedSize.Size = new System.Drawing.Size(216, 20);
        this.txtChangeExpandedSize.TabIndex = 19;
        //
        // btnChangeExpandedSize
        //
        this.btnChangeExpandedSize.Location = new System.Drawing.Point(538, 233);
        this.btnChangeExpandedSize.Name = "btnChangeExpandedSize";
        this.btnChangeExpandedSize.Size = new System.Drawing.Size(169, 23);
        this.btnChangeExpandedSize.TabIndex = 18;
        this.btnChangeExpandedSize.Text = "Change ExpandedSize";
        this.btnChangeExpandedSize.UseVisualStyleBackColor = true;
        this.btnChangeExpandedSize.Click += new System.EventHandler(this.ChangeWidth);
        //
        // btnTestEmptyConstructor
        //
        this.btnTestEmptyConstructor.Location = new System.Drawing.Point(348, 113);
        this.btnTestEmptyConstructor.Name = "btnTestEmptyConstructor";
        this.btnTestEmptyConstructor.Size = new System.Drawing.Size(169, 23);
        this.btnTestEmptyConstructor.TabIndex = 20;
        this.btnTestEmptyConstructor.Text = "Empty Constructor";
        this.btnTestEmptyConstructor.UseVisualStyleBackColor = true;
        this.btnTestEmptyConstructor.Click += new System.EventHandler(this.TestEmptyConstructor);
        //
        // btnTestFullConstructor
        //
        this.btnTestFullConstructor.Location = new System.Drawing.Point(523, 113);
        this.btnTestFullConstructor.Name = "btnTestFullConstructor";
        this.btnTestFullConstructor.Size = new System.Drawing.Size(169, 23);
        this.btnTestFullConstructor.TabIndex = 21;
        this.btnTestFullConstructor.Text = "Full Constructor";
        this.btnTestFullConstructor.UseVisualStyleBackColor = true;
        this.btnTestFullConstructor.Click += new System.EventHandler(this.TestFullConstructor);
        //
        // btnTestUserControlVerticalConstructor
        //
        this.btnTestUserControlVerticalConstructor.Location = new System.Drawing.Point(523, 171);
        this.btnTestUserControlVerticalConstructor.Name = "btnTestUserControlVerticalConstructor";
        this.btnTestUserControlVerticalConstructor.Size = new System.Drawing.Size(169, 23);
        this.btnTestUserControlVerticalConstructor.TabIndex = 23;
        this.btnTestUserControlVerticalConstructor.Text = "UserControl      Vertical";
        this.btnTestUserControlVerticalConstructor.UseVisualStyleBackColor = true;
        this.btnTestUserControlVerticalConstructor.Click += new System.EventHandler(this.TestUserControlVerticalConstructor);
        //
        // btnTestTaskListVerticalConstructor
        //
        this.btnTestTaskListVerticalConstructor.Location = new System.Drawing.Point(523, 142);
        this.btnTestTaskListVerticalConstructor.Name = "btnTestTaskListVerticalConstructor";
        this.btnTestTaskListVerticalConstructor.Size = new System.Drawing.Size(169, 23);
        this.btnTestTaskListVerticalConstructor.TabIndex = 24;
        this.btnTestTaskListVerticalConstructor.Text = "TaskList    Vertical";
        this.btnTestTaskListVerticalConstructor.UseVisualStyleBackColor = true;
        this.btnTestTaskListVerticalConstructor.Click += new System.EventHandler(this.TestTaskListVerticalConstructor);
        //
        // btnTestTaskListExpandedConstructor
        //
        this.btnTestTaskListExpandedConstructor.Location = new System.Drawing.Point(348, 171);
        this.btnTestTaskListExpandedConstructor.Name = "btnTestTaskListExpandedConstructor";
        this.btnTestTaskListExpandedConstructor.Size = new System.Drawing.Size(169, 23);
        this.btnTestTaskListExpandedConstructor.TabIndex = 25;
        this.btnTestTaskListExpandedConstructor.Text = "TaskList    Expanded";
        this.btnTestTaskListExpandedConstructor.UseVisualStyleBackColor = true;
        this.btnTestTaskListExpandedConstructor.Click += new System.EventHandler(this.TestTaskListExpandedConstructor);
        //
        // btnTestTaskListHorizontalConstructor
        //
        this.btnTestTaskListHorizontalConstructor.Location = new System.Drawing.Point(348, 142);
        this.btnTestTaskListHorizontalConstructor.Name = "btnTestTaskListHorizontalConstructor";
        this.btnTestTaskListHorizontalConstructor.Size = new System.Drawing.Size(169, 23);
        this.btnTestTaskListHorizontalConstructor.TabIndex = 26;
        this.btnTestTaskListHorizontalConstructor.Text = "TaskList    Horizontal";
        this.btnTestTaskListHorizontalConstructor.UseVisualStyleBackColor = true;
        this.btnTestTaskListHorizontalConstructor.Click += new System.EventHandler(this.TestTaskListHorizontalConstructor);
        //
        // btnTestStacked
        //
        this.btnTestStacked.Location = new System.Drawing.Point(348, 200);
        this.btnTestStacked.Name = "btnTestStacked";
        this.btnTestStacked.Size = new System.Drawing.Size(169, 23);
        this.btnTestStacked.TabIndex = 27;
        this.btnTestStacked.Text = "Stacked panels";
        this.btnTestStacked.UseVisualStyleBackColor = true;
        this.btnTestStacked.Click += new System.EventHandler(this.TestStacked);
        //
        // pnlContent
        //
        this.pnlContent.BackColor = System.Drawing.Color.White;
        this.pnlContent.Controls.Add(this.sptContent);
        this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
        this.pnlContent.Location = new System.Drawing.Point(0, 0);
        this.pnlContent.Name = "pnlContent";
        this.pnlContent.Size = new System.Drawing.Size(524, 605);
        this.pnlContent.TabIndex = 28;
        //
        // sptContent
        //
        this.sptContent.Dock = System.Windows.Forms.DockStyle.Fill;
        this.sptContent.Location = new System.Drawing.Point(0, 0);
        this.sptContent.Name = "sptContent";
        //
        // sptContent.Panel1
        //
        this.sptContent.Panel1.Controls.Add(this.cplFolders);
        //
        // sptContent.Panel2
        //
        this.sptContent.Panel2.BackColor = System.Drawing.SystemColors.WindowFrame;
        this.sptContent.Panel2.Controls.Add(this.pnlFiller);
        this.sptContent.Panel2.Controls.Add(this.cplPartnerInfo);
        this.sptContent.Size = new System.Drawing.Size(524, 605);
        this.sptContent.SplitterDistance = 125;
        this.sptContent.TabIndex = 0;
        this.sptContent.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.SptContentSplitterMoved);
        //
        // cplFolders
        //
        this.cplFolders.AutoSize = true;
        this.cplFolders.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.cplFolders.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdHorizontal;
        this.cplFolders.Dock = System.Windows.Forms.DockStyle.Left;
        this.cplFolders.ExpandedSize = 181;
        this.cplFolders.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckCollapsiblePanelHoster;
        this.cplFolders.Location = new System.Drawing.Point(0, 0);
        this.cplFolders.Margin = new System.Windows.Forms.Padding(0);
        this.cplFolders.Name = "cplFolders";
        this.cplFolders.Size = new System.Drawing.Size(424, 605);
        this.cplFolders.TabIndex = 0;
        this.cplFolders.Text = "Finance";
        this.cplFolders.UserControlClass = "";
        this.cplFolders.UserControlNamespace = "";
        this.cplFolders.UserControlString = ".";
        this.cplFolders.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsHorizontalCollapse;
        this.cplFolders.Collapsed += new System.EventHandler(this.CplFoldersCollapsed);
        this.cplFolders.Expanded += new System.EventHandler(this.CplFoldersExpanded);
        //
        // pnlFiller
        //
        this.pnlFiller.Controls.Add(this.pnlTasksHoster);
        this.pnlFiller.Controls.Add(this.pnlShepherdHoster);
        this.pnlFiller.Controls.Add(this.pnlTasksHoster2);
        this.pnlFiller.Dock = System.Windows.Forms.DockStyle.Fill;
        this.pnlFiller.Location = new System.Drawing.Point(0, 0);
        this.pnlFiller.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
        this.pnlFiller.Name = "pnlFiller";
        this.pnlFiller.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
        this.pnlFiller.Size = new System.Drawing.Size(402, 429);
        this.pnlFiller.TabIndex = 4;
        //
        // pnlTasksHoster
        //
        this.pnlTasksHoster.Anchor =
            ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                  System.Windows.Forms.AnchorStyles.Left)));
        this.pnlTasksHoster.BackColor = System.Drawing.Color.Wheat;
        this.pnlTasksHoster.Controls.Add(this.pchTasks1);
        this.pnlTasksHoster.Location = new System.Drawing.Point(6, 6);
        this.pnlTasksHoster.Name = "pnlTasksHoster";
        this.pnlTasksHoster.Size = new System.Drawing.Size(240, 446);
        this.pnlTasksHoster.TabIndex = 0;
        //
        // pchTasks1
        //
        this.pchTasks1.AutoSize = true;
        this.pchTasks1.BackColor = System.Drawing.Color.Transparent;
        this.pchTasks1.DistanceBetweenCollapsiblePanels = 30;
        this.pchTasks1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.pchTasks1.Location = new System.Drawing.Point(0, 0);
        this.pchTasks1.Margin = new System.Windows.Forms.Padding(0);
        this.pchTasks1.Name = "pchTasks1";
        this.pchTasks1.Size = new System.Drawing.Size(240, 446);
        this.pchTasks1.TabIndex = 0;
        this.pchTasks1.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsAccordionPanel;
        //
        // pnlShepherdHoster
        //
        this.pnlShepherdHoster.Anchor =
            ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                   System.Windows.Forms.AnchorStyles.Left) |
                                                  System.Windows.Forms.AnchorStyles.Right)));
        this.pnlShepherdHoster.BackColor = System.Drawing.Color.YellowGreen;
        this.pnlShepherdHoster.Controls.Add(this.cplShepherd);
        this.pnlShepherdHoster.Location = new System.Drawing.Point(439, 6);
        this.pnlShepherdHoster.Name = "pnlShepherdHoster";
        this.pnlShepherdHoster.Size = new System.Drawing.Size(0, 446);
        this.pnlShepherdHoster.TabIndex = 3;
        //
        // cplShepherd
        //
        this.cplShepherd.AutoSize = true;
        this.cplShepherd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.cplShepherd.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdHorizontal;
        this.cplShepherd.Dock = System.Windows.Forms.DockStyle.Left;
        this.cplShepherd.ExpandedSize = 181;
        this.cplShepherd.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
        this.cplShepherd.Location = new System.Drawing.Point(0, 0);
        this.cplShepherd.Margin = new System.Windows.Forms.Padding(0);
        this.cplShepherd.Name = "cplShepherd";
        this.cplShepherd.Size = new System.Drawing.Size(424, 446);
        this.cplShepherd.TabIndex = 0;
        this.cplShepherd.Text = "Shepherd 1";
        this.cplShepherd.UserControlClass = "";
        this.cplShepherd.UserControlNamespace = "";
        this.cplShepherd.UserControlString = ".";
        this.cplShepherd.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsShepherd;
        //
        // pnlTasksHoster2
        //
        this.pnlTasksHoster2.Anchor =
            ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                  System.Windows.Forms.AnchorStyles.Left)));
        this.pnlTasksHoster2.BackColor = System.Drawing.Color.Thistle;
        this.pnlTasksHoster2.Controls.Add(this.cplTasks4);
        this.pnlTasksHoster2.Controls.Add(this.cplTasks3);
        this.pnlTasksHoster2.Location = new System.Drawing.Point(252, 6);
        this.pnlTasksHoster2.Name = "pnlTasksHoster2";
        this.pnlTasksHoster2.Size = new System.Drawing.Size(181, 446);
        this.pnlTasksHoster2.TabIndex = 2;
        //
        // cplTasks4
        //
        this.cplTasks4.AutoSize = true;
        this.cplTasks4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.cplTasks4.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdVertical;
        this.cplTasks4.Dock = System.Windows.Forms.DockStyle.Top;
        this.cplTasks4.ExpandedSize = 179;
        this.cplTasks4.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
        this.cplTasks4.Location = new System.Drawing.Point(0, 186);
        this.cplTasks4.Margin = new System.Windows.Forms.Padding(0);
        this.cplTasks4.Name = "cplTasks4";
        this.cplTasks4.Padding = new System.Windows.Forms.Padding(5);
        this.cplTasks4.Size = new System.Drawing.Size(181, 186);
        this.cplTasks4.TabIndex = 1;
        this.cplTasks4.Text = "Tasks 4 (Accordion)";
        this.cplTasks4.UserControlClass = "";
        this.cplTasks4.UserControlNamespace = "";
        this.cplTasks4.UserControlString = ".";
        this.cplTasks4.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsTaskPanel;
        //
        // cplTasks3
        //
        this.cplTasks3.AutoSize = true;
        this.cplTasks3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.cplTasks3.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdVertical;
        this.cplTasks3.Dock = System.Windows.Forms.DockStyle.Top;
        this.cplTasks3.ExpandedSize = 183;
        this.cplTasks3.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
        this.cplTasks3.Location = new System.Drawing.Point(0, 0);
        this.cplTasks3.Margin = new System.Windows.Forms.Padding(0);
        this.cplTasks3.Name = "cplTasks3";
        this.cplTasks3.Padding = new System.Windows.Forms.Padding(5);
        this.cplTasks3.Size = new System.Drawing.Size(181, 186);
        this.cplTasks3.TabIndex = 0;
        this.cplTasks3.Text = "Tasks 3 (Accordion)";
        this.cplTasks3.UserControlClass = "";
        this.cplTasks3.UserControlNamespace = "";
        this.cplTasks3.UserControlString = ".";
        this.cplTasks3.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsTaskPanel;
        //
        // cplPartnerInfo
        //
        this.cplPartnerInfo.AutoSize = true;
        this.cplPartnerInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.cplPartnerInfo.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdVertical;
        this.cplPartnerInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.cplPartnerInfo.ExpandedSize = 200;
        this.cplPartnerInfo.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckUserControl;
        this.cplPartnerInfo.Location = new System.Drawing.Point(0, 429);
        this.cplPartnerInfo.Margin = new System.Windows.Forms.Padding(0);
        this.cplPartnerInfo.Name = "cplPartnerInfo";
        this.cplPartnerInfo.Size = new System.Drawing.Size(402, 176);
        this.cplPartnerInfo.TabIndex = 1;
        this.cplPartnerInfo.Text = "Partner Info";
        this.cplPartnerInfo.UserControlClass = "TUC_PartnerInfo";
        this.cplPartnerInfo.UserControlNamespace = "Ict.Petra.Client.MPartner.Gui";
        this.cplPartnerInfo.UserControlString = "Ict.Petra.Client.MPartner.Gui.TUC_PartnerInfo";
        this.cplPartnerInfo.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsDashboard;
        //
        // cplRightCollapse
        //
        this.cplRightCollapse.AutoSize = true;
        this.cplRightCollapse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.cplRightCollapse.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdHorizontalRight;
        this.cplRightCollapse.Dock = System.Windows.Forms.DockStyle.Right;
        this.cplRightCollapse.ExpandedSize = 250;
        this.cplRightCollapse.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckUserControl;
        this.cplRightCollapse.Location = new System.Drawing.Point(524, 0);
        this.cplRightCollapse.Margin = new System.Windows.Forms.Padding(0);
        this.cplRightCollapse.Name = "cplRightCollapse";
        this.cplRightCollapse.Size = new System.Drawing.Size(424, 605);
        this.cplRightCollapse.TabIndex = 29;
        this.cplRightCollapse.Text = "To-Do Bar";
        this.cplRightCollapse.UserControlClass = "TUC_PartnerInfo";
        this.cplRightCollapse.UserControlNamespace = "Ict.Petra.Client.MPartner.Gui";
        this.cplRightCollapse.UserControlString = "Ict.Petra.Client.MPartner.Gui.TUC_PartnerInfo";
        this.cplRightCollapse.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsHorizontalCollapse;
        //
        // TestAll
        //
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.SystemColors.ControlDark;
        this.ClientSize = new System.Drawing.Size(948, 605);
        this.Controls.Add(this.pnlContent);
        this.Controls.Add(this.btnTestStacked);
        this.Controls.Add(this.btnTestTaskListHorizontalConstructor);
        this.Controls.Add(this.btnTestTaskListExpandedConstructor);
        this.Controls.Add(this.btnTestTaskListVerticalConstructor);
        this.Controls.Add(this.btnTestUserControlVerticalConstructor);
        this.Controls.Add(this.btnTestFullConstructor);
        this.Controls.Add(this.btnTestEmptyConstructor);
        this.Controls.Add(this.txtChangeExpandedSize);
        this.Controls.Add(this.rtbChangeTaskListNode);
        this.Controls.Add(this.cboChangeVisualStyle);
        this.Controls.Add(this.cboChangeHostedControlKind);
        this.Controls.Add(this.cboChangeCollapseDirection);
        this.Controls.Add(this.txtChangeUserControlString);
        this.Controls.Add(this.txtChangeText);
        this.Controls.Add(this.cplRightCollapse);
        this.Controls.Add(this.btnChangeExpandedSize);
        this.Controls.Add(this.btnChangeVisualStyle);
        this.Controls.Add(this.btnChangeTaskListNode);
        this.Controls.Add(this.btnChangeUserControlString);
        this.Controls.Add(this.btnChangeHostedControlKind);
        this.Controls.Add(this.btnChangeCollapseDirection);
        this.Controls.Add(this.btnChangeText);
        this.Name = "TestAll";
        this.Text = "TestAll";
        this.Load += new System.EventHandler(this.TestAllLoad);
        this.pnlContent.ResumeLayout(false);
        this.sptContent.Panel1.ResumeLayout(false);
        this.sptContent.Panel1.PerformLayout();
        this.sptContent.Panel2.ResumeLayout(false);
        this.sptContent.Panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.sptContent)).EndInit();
        this.sptContent.ResumeLayout(false);
        this.pnlFiller.ResumeLayout(false);
        this.pnlTasksHoster.ResumeLayout(false);
        this.pnlTasksHoster.PerformLayout();
        this.pnlShepherdHoster.ResumeLayout(false);
        this.pnlShepherdHoster.PerformLayout();
        this.pnlTasksHoster2.ResumeLayout(false);
        this.pnlTasksHoster2.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private Ict.Common.Controls.TPnlCollapsibleHoster pchTasks1;
    private Ict.Common.Controls.TPnlCollapsible cplRightCollapse;
    private System.Windows.Forms.Panel pnlFiller;
    private Ict.Common.Controls.TPnlCollapsible cplTasks3;
    private Ict.Common.Controls.TPnlCollapsible cplTasks4;
    private System.Windows.Forms.Panel pnlTasksHoster2;
    private Ict.Common.Controls.TPnlCollapsible cplShepherd;
    private System.Windows.Forms.Panel pnlShepherdHoster;
    private Ict.Common.Controls.TPnlCollapsible cplPartnerInfo;
    private System.Windows.Forms.Panel pnlTasksHoster;
    private Ict.Common.Controls.TPnlCollapsible cplFolders;
    private System.Windows.Forms.SplitContainer sptContent;

    private System.Windows.Forms.Panel pnlContent;
    private System.Windows.Forms.Button btnTestStacked;
    private System.Windows.Forms.Button btnTestTaskListExpandedConstructor;
    private System.Windows.Forms.Button btnTestUserControlVerticalConstructor;
    private System.Windows.Forms.Button btnTestTaskListVerticalConstructor;
    private System.Windows.Forms.Button btnTestTaskListHorizontalConstructor;
    private System.Windows.Forms.TextBox txtChangeExpandedSize;
    private System.Windows.Forms.Button btnChangeExpandedSize;
    private System.Windows.Forms.RichTextBox rtbChangeTaskListNode;
    private System.Windows.Forms.Button btnTestFullConstructor;
    private System.Windows.Forms.Button btnTestEmptyConstructor;
    private System.Windows.Forms.TextBox txtChangeUserControlString;
    private System.Windows.Forms.ComboBox cboChangeVisualStyle;
    private System.Windows.Forms.Button btnChangeTaskListNode;
    private System.Windows.Forms.Button btnChangeVisualStyle;
    private System.Windows.Forms.ComboBox cboChangeHostedControlKind;
    private System.Windows.Forms.ComboBox cboChangeCollapseDirection;
    private System.Windows.Forms.Button btnChangeUserControlString;
    private System.Windows.Forms.Button btnChangeHostedControlKind;
    private System.Windows.Forms.Button btnChangeCollapseDirection;
    private System.Windows.Forms.Button btnChangeText;
    private System.Windows.Forms.TextBox txtChangeText;
}
}