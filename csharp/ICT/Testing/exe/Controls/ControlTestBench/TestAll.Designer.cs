/*
 * Created by SharpDevelop.
 * User: sbird
 * Date: 7/1/2011
 * Time: 10:02 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
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
            this.cplTasks2 = new Ict.Common.Controls.TPnlCollapsible();
            this.cplTasks1 = new Ict.Common.Controls.TPnlCollapsible();
            this.pnlShepherdHoster = new System.Windows.Forms.Panel();
            this.cplShepherd = new Ict.Common.Controls.TPnlCollapsible();
            this.pnlTasksHoster2 = new System.Windows.Forms.Panel();
            this.cplTasks4 = new Ict.Common.Controls.TPnlCollapsible();
            this.cplTasks3 = new Ict.Common.Controls.TPnlCollapsible();
            this.cplPartnerInfo = new Ict.Common.Controls.TPnlCollapsible();
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
                                    "Vertical"});
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
                                    "UserControl"});
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
                                    "vsHorizontalCollapse"});
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
            this.pnlContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                                    | System.Windows.Forms.AnchorStyles.Left) 
                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Controls.Add(this.sptContent);
            this.pnlContent.Location = new System.Drawing.Point(13, 12);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(923, 581);
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
            this.sptContent.Size = new System.Drawing.Size(923, 581);
            this.sptContent.SplitterDistance = 211;
            this.sptContent.TabIndex = 0;
            this.sptContent.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.SptContentSplitterMoved);
            // 
            // cplFolders
            // 
            this.cplFolders.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdHorizontal;
            this.cplFolders.Dock = System.Windows.Forms.DockStyle.Left;
            this.cplFolders.ExpandedSize = 215;
            this.cplFolders.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
            this.cplFolders.IsCollapsed = false;
            this.cplFolders.Location = new System.Drawing.Point(0, 0);
            this.cplFolders.Margin = new System.Windows.Forms.Padding(0);
            this.cplFolders.Name = "cplFolders";
            this.cplFolders.Size = new System.Drawing.Size(215, 581);
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
            this.pnlFiller.Size = new System.Drawing.Size(708, 431);
            this.pnlFiller.TabIndex = 4;
            // 
            // pnlTasksHoster
            // 
            this.pnlTasksHoster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                                    | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlTasksHoster.BackColor = System.Drawing.Color.Wheat;
            this.pnlTasksHoster.Controls.Add(this.cplTasks2);
            this.pnlTasksHoster.Controls.Add(this.cplTasks1);
            this.pnlTasksHoster.Location = new System.Drawing.Point(6, 6);
            this.pnlTasksHoster.Name = "pnlTasksHoster";
            this.pnlTasksHoster.Size = new System.Drawing.Size(240, 422);
            this.pnlTasksHoster.TabIndex = 0;
            // 
            // cplTasks2
            // 
            this.cplTasks2.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdVertical;
            this.cplTasks2.Dock = System.Windows.Forms.DockStyle.Top;
            this.cplTasks2.ExpandedSize = 200;
            this.cplTasks2.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
            this.cplTasks2.IsCollapsed = false;
            this.cplTasks2.Location = new System.Drawing.Point(0, 200);
            this.cplTasks2.Margin = new System.Windows.Forms.Padding(0);
            this.cplTasks2.Name = "cplTasks2";
            this.cplTasks2.Padding = new System.Windows.Forms.Padding(5);
            this.cplTasks2.Size = new System.Drawing.Size(240, 200);
            this.cplTasks2.TabIndex = 1;
            this.cplTasks2.Text = "Tasks 2 (XP Task Panel)";
            this.cplTasks2.UserControlClass = "";
            this.cplTasks2.UserControlNamespace = "";
            this.cplTasks2.UserControlString = ".";
            this.cplTasks2.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsTaskPanel;
            // 
            // cplTasks1
            // 
            this.cplTasks1.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdVertical;
            this.cplTasks1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cplTasks1.ExpandedSize = 200;
            this.cplTasks1.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
            this.cplTasks1.IsCollapsed = false;
            this.cplTasks1.Location = new System.Drawing.Point(0, 0);
            this.cplTasks1.Margin = new System.Windows.Forms.Padding(0);
            this.cplTasks1.Name = "cplTasks1";
            this.cplTasks1.Padding = new System.Windows.Forms.Padding(5);
            this.cplTasks1.Size = new System.Drawing.Size(240, 200);
            this.cplTasks1.TabIndex = 0;
            this.cplTasks1.Text = "Tasks 1 (XP Task Panel)";
            this.cplTasks1.UserControlClass = "";
            this.cplTasks1.UserControlNamespace = "";
            this.cplTasks1.UserControlString = ".";
            this.cplTasks1.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsTaskPanel;
            // 
            // pnlShepherdHoster
            // 
            this.pnlShepherdHoster.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                                    | System.Windows.Forms.AnchorStyles.Left) 
                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlShepherdHoster.BackColor = System.Drawing.Color.YellowGreen;
            this.pnlShepherdHoster.Controls.Add(this.cplShepherd);
            this.pnlShepherdHoster.Location = new System.Drawing.Point(439, 6);
            this.pnlShepherdHoster.Name = "pnlShepherdHoster";
            this.pnlShepherdHoster.Size = new System.Drawing.Size(266, 422);
            this.pnlShepherdHoster.TabIndex = 3;
            // 
            // cplShepherd
            // 
            this.cplShepherd.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdHorizontal;
            this.cplShepherd.Dock = System.Windows.Forms.DockStyle.Left;
            this.cplShepherd.ExpandedSize = 200;
            this.cplShepherd.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
            this.cplShepherd.IsCollapsed = false;
            this.cplShepherd.Location = new System.Drawing.Point(0, 0);
            this.cplShepherd.Margin = new System.Windows.Forms.Padding(0);
            this.cplShepherd.Name = "cplShepherd";
            this.cplShepherd.Size = new System.Drawing.Size(200, 422);
            this.cplShepherd.TabIndex = 0;
            this.cplShepherd.Text = "Shepherd 1";
            this.cplShepherd.UserControlClass = "";
            this.cplShepherd.UserControlNamespace = "";
            this.cplShepherd.UserControlString = ".";
            this.cplShepherd.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsShepherd;
            // 
            // pnlTasksHoster2
            // 
            this.pnlTasksHoster2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                                    | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlTasksHoster2.BackColor = System.Drawing.Color.Thistle;
            this.pnlTasksHoster2.Controls.Add(this.cplTasks4);
            this.pnlTasksHoster2.Controls.Add(this.cplTasks3);
            this.pnlTasksHoster2.Location = new System.Drawing.Point(252, 6);
            this.pnlTasksHoster2.Name = "pnlTasksHoster2";
            this.pnlTasksHoster2.Size = new System.Drawing.Size(181, 422);
            this.pnlTasksHoster2.TabIndex = 2;
            // 
            // cplTasks4
            // 
            this.cplTasks4.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdVertical;
            this.cplTasks4.Dock = System.Windows.Forms.DockStyle.Top;
            this.cplTasks4.ExpandedSize = 200;
            this.cplTasks4.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
            this.cplTasks4.IsCollapsed = false;
            this.cplTasks4.Location = new System.Drawing.Point(0, 200);
            this.cplTasks4.Margin = new System.Windows.Forms.Padding(0);
            this.cplTasks4.Name = "cplTasks4";
            this.cplTasks4.Padding = new System.Windows.Forms.Padding(5);
            this.cplTasks4.Size = new System.Drawing.Size(181, 200);
            this.cplTasks4.TabIndex = 1;
            this.cplTasks4.Text = "Tasks 4 (Accordion)";
            this.cplTasks4.UserControlClass = "";
            this.cplTasks4.UserControlNamespace = "";
            this.cplTasks4.UserControlString = ".";
            this.cplTasks4.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsAccordionPanel;
            // 
            // cplTasks3
            // 
            this.cplTasks3.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdVertical;
            this.cplTasks3.Dock = System.Windows.Forms.DockStyle.Top;
            this.cplTasks3.ExpandedSize = 200;
            this.cplTasks3.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckTaskList;
            this.cplTasks3.IsCollapsed = false;
            this.cplTasks3.Location = new System.Drawing.Point(0, 0);
            this.cplTasks3.Margin = new System.Windows.Forms.Padding(0);
            this.cplTasks3.Name = "cplTasks3";
            this.cplTasks3.Padding = new System.Windows.Forms.Padding(5);
            this.cplTasks3.Size = new System.Drawing.Size(181, 200);
            this.cplTasks3.TabIndex = 0;
            this.cplTasks3.Text = "Tasks 3 (Accordion)";
            this.cplTasks3.UserControlClass = "";
            this.cplTasks3.UserControlNamespace = "";
            this.cplTasks3.UserControlString = ".";
            this.cplTasks3.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsAccordionPanel;
            // 
            // cplPartnerInfo
            // 
            this.cplPartnerInfo.CollapseDirection = Ict.Common.Controls.TCollapseDirection.cdVertical;
            this.cplPartnerInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cplPartnerInfo.ExpandedSize = 200;
            this.cplPartnerInfo.HostedControlKind = Ict.Common.Controls.THostedControlKind.hckUserControl;
            this.cplPartnerInfo.IsCollapsed = false;
            this.cplPartnerInfo.Location = new System.Drawing.Point(0, 431);
            this.cplPartnerInfo.Margin = new System.Windows.Forms.Padding(0);
            this.cplPartnerInfo.Name = "cplPartnerInfo";
            this.cplPartnerInfo.Size = new System.Drawing.Size(708, 150);
            this.cplPartnerInfo.TabIndex = 1;
            this.cplPartnerInfo.Text = "Partner Info";
            this.cplPartnerInfo.UserControlClass = "TUC_PartnerInfo";
            this.cplPartnerInfo.UserControlNamespace = "Ict.Petra.Client.MPartner.Gui";
            this.cplPartnerInfo.UserControlString = "Ict.Petra.Client.MPartner.Gui.TUC_PartnerInfo";
            this.cplPartnerInfo.VisualStyleEnum = Ict.Common.Controls.TVisualStylesEnum.vsDashboard;
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
            this.Controls.Add(this.btnChangeExpandedSize);
            this.Controls.Add(this.rtbChangeTaskListNode);
            this.Controls.Add(this.cboChangeVisualStyle);
            this.Controls.Add(this.cboChangeHostedControlKind);
            this.Controls.Add(this.cboChangeCollapseDirection);
            this.Controls.Add(this.txtChangeUserControlString);
            this.Controls.Add(this.txtChangeText);
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
            this.sptContent.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sptContent)).EndInit();
            this.sptContent.ResumeLayout(false);
            this.pnlFiller.ResumeLayout(false);
            this.pnlTasksHoster.ResumeLayout(false);
            this.pnlShepherdHoster.ResumeLayout(false);
            this.pnlTasksHoster2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Panel pnlFiller;
        private Ict.Common.Controls.TPnlCollapsible cplTasks3;
        private Ict.Common.Controls.TPnlCollapsible cplTasks4;
        private System.Windows.Forms.Panel pnlTasksHoster2;
        private Ict.Common.Controls.TPnlCollapsible cplShepherd;
        private System.Windows.Forms.Panel pnlShepherdHoster;
        private Ict.Common.Controls.TPnlCollapsible cplPartnerInfo;
        private Ict.Common.Controls.TPnlCollapsible cplTasks1;
        private Ict.Common.Controls.TPnlCollapsible cplTasks2;
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
