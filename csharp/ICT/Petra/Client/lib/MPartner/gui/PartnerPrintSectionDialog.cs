/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank, petrih
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SourceGrid;
using Ict.Petra.Client.MPartner.Applink;
using System.Resources;
using SourceGrid.Cells.Controllers;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared;
using Ict.Petra.Client.MPartner;
using Ict.Petra.Client.CommonForms;
using Ict.Common.Controls;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// A Dialog screen via can be printed sections reports of PartnerEdit screen.
    /// This screen uses integers to identify the selected tab. Tabs are:
    /// 0 = Subscription
    /// 1 = Interests
    /// 2 = Contacts
    /// 3 = Reminders
    ///
    /// // Other way to do this would be to use the TPartnerEditTabPageEnum. This case the call from Ict.Petra.Client.MPartner.PartnerEdit should should be changed.
    ///
    ///
    /// If you want to see the Groupbox (now invisible), uncomment SetGroupBoxVisibleInVisible() from two places on this page.
    /// There is already a Delegate function that returns a currently selected dataRow from the tab wanted.
    /// As the SubscriptionTab only is currently working, this function is disableb for the moment.
    /// The meaning is to send this returned dataRow to timos raporting tool to be printed.
    ///
    /// See more information about this screen from PetraWiki 15.2  -> New Print Section Dialog.
    public class TPartnerPrintSectionDialog : TFrmPetraDialog
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private TsgrdDataGrid FgrdPrintList;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.GroupBox grpPrintScope;
        private System.Windows.Forms.RadioButton rdbAll;
        private System.Windows.Forms.RadioButton rdbSelected;
        private System.Windows.Forms.Label lblPrintInfo;
        private Boolean FHasChanges;
        private DataTable FPrintTable;
        private DataView FGridDV;
        private Int32 FSelectedTab;
        private Int64 FPartnerKey;
        private TDelegateDataRowOfCurrentlySelectedRecord FDelegateDataRowOfCurrentlySelectedRecord;
        private Point FPreviousLocation;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TPartnerPrintSectionDialog));
            this.FgrdPrintList = new Ict.Common.Controls.TsgrdDataGrid();
            this.Label1 = new System.Windows.Forms.Label();
            this.grpPrintScope = new System.Windows.Forms.GroupBox();
            this.rdbSelected = new System.Windows.Forms.RadioButton();
            this.rdbAll = new System.Windows.Forms.RadioButton();
            this.lblPrintInfo = new System.Windows.Forms.Label();
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).BeginInit();
            this.grpPrintScope.SuspendLayout();
            this.SuspendLayout();

            //
            // btnOK
            //
todo: move statusbar things to constructor
            this.btnOK.Location = new System.Drawing.Point(184, 8);
            this.btnOK.Name = "btnOK";
            this.sbtForm.SetStatusBarText(this.btnOK, "Print All Selected Reports");
            this.btnOK.Text = "&Print";
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 184);
            this.pnlBtnOKCancelHelpLayout.Name = "pnlBtnOKCancelHelpLayout";
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(346, 32);

            //
            // btnCancel
            //
            this.btnCancel.Location = new System.Drawing.Point(264, 8);
            this.btnCancel.Name = "btnCancel";
            this.sbtForm.SetStatusBarText(this.btnCancel, "Close this window without p" + "rinting");

            //
            // btnHelp
            //
            this.btnHelp.Location = new System.Drawing.Point(8, 8);
            this.btnHelp.Name = "btnHelp";
            this.sbtForm.SetStatusBarText(this.btnHelp, "Help");

            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 216);
            this.stbMain.Name = "stbMain";
            this.stbMain.Size = new System.Drawing.Size(346, 22);

            //
            // stpInfo
            //
            this.stpInfo.Width = 346;

            //
            // FgrdPrintList
            //
            this.FgrdPrintList.AlternatingBackgroundColour = System.Drawing.Color.FromArgb(255, 255, 255);
            this.FgrdPrintList.AutoFindColumn = ((Int16)(1));
            this.FgrdPrintList.BackColor = System.Drawing.SystemColors.ControlDark;
            this.FgrdPrintList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FgrdPrintList.DefaultHeight = 25;
            this.FgrdPrintList.DeleteQuestionMessage = "You have chosen to delete thi" + "s record.'#13#10#13#10'Dou you really want to delete it?";
            this.FgrdPrintList.FixedRows = 1;
            this.FgrdPrintList.Location = new System.Drawing.Point(8, 24);
            this.FgrdPrintList.MinimumHeight = 1;
            this.FgrdPrintList.Name = "FgrdPrintList";
            this.FgrdPrintList.Size = new System.Drawing.Size(332, 102);
            this.FgrdPrintList.SortableHeaders = false;
            this.FgrdPrintList.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));
            this.FgrdPrintList.TabIndex = 0;
            this.FgrdPrintList.TabStop = true;
            this.FgrdPrintList.Click += new System.EventHandler(this.FgrdPrintList_Click);
            this.FgrdPrintList.DoubleClickCell += new TDoubleClickCellEventHandler(this.FgrdPrintList_DoubleClickCell);
            this.sbtForm.SetStatusBarText(this.FgrdPrintList, "Section Reports that can be printed");

            //
            // Label1
            //
            this.Label1.Location = new System.Drawing.Point(8, 8);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(344, 15);
            this.Label1.TabIndex = 998;
            this.Label1.Text = "Check Report(s) that should be printed for this Partn" + "er:";

            //
            // grpPrintScope
            //
            this.grpPrintScope.Controls.Add(this.rdbSelected);
            this.grpPrintScope.Controls.Add(this.rdbAll);
            this.grpPrintScope.Location = new System.Drawing.Point(8, 120);
            this.grpPrintScope.Name = "grpPrintScope";
            this.grpPrintScope.Size = new System.Drawing.Size(332, 66);
            this.grpPrintScope.TabIndex = 1;
            this.grpPrintScope.TabStop = false;
            this.grpPrintScope.Text = "Print Scope for Report 'x'";
            this.grpPrintScope.Visible = false;

            //
            // rdbSelected
            //
            this.rdbSelected.Location = new System.Drawing.Point(16, 40);
            this.rdbSelected.Name = "rdbSelected";
            this.rdbSelected.Size = new System.Drawing.Size(112, 24);
            this.rdbSelected.TabIndex = 1;
            this.rdbSelected.Text = "SelectedRecord";
            this.rdbSelected.CheckedChanged += new System.EventHandler(this.RdbSelected_CheckedChanged);

            //
            // rdbAll
            //
            this.rdbAll.Checked = true;
            this.rdbAll.Location = new System.Drawing.Point(16, 16);
            this.rdbAll.Name = "rdbAll";
            this.rdbAll.TabIndex = 0;
            this.rdbAll.TabStop = true;
            this.rdbAll.Text = "All Records";
            this.rdbAll.Click += new System.EventHandler(this.RdbAll_Click);

            //
            // lblPrintInfo
            //
            this.lblPrintInfo.BackColor = System.Drawing.Color.White;
            this.lblPrintInfo.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.lblPrintInfo.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblPrintInfo.Location = new System.Drawing.Point(8, 24);
            this.lblPrintInfo.Name = "lblPrintInfo";
            this.lblPrintInfo.Size = new System.Drawing.Size(332, 102);
            this.lblPrintInfo.TabIndex = 999;
            this.lblPrintInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            //
            // TPartnerPrintSectionDialog
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(346, 238);
            this.Controls.Add(this.grpPrintScope);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.FgrdPrintList);
            this.Controls.Add(this.lblPrintInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TPartnerPrintSectionDialog";
            this.Text = "Print Section";
            this.Controls.SetChildIndex(this.lblPrintInfo, 0);
            this.Controls.SetChildIndex(this.FgrdPrintList, 0);
            this.Controls.SetChildIndex(this.stbMain, 0);
            this.Controls.SetChildIndex(this.pnlBtnOKCancelHelpLayout, 0);
            this.Controls.SetChildIndex(this.Label1, 0);
            this.Controls.SetChildIndex(this.grpPrintScope, 0);
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).EndInit();
            this.grpPrintScope.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void RdbSelected_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (rdbSelected.Checked)
            {
                this.FgrdPrintList.SelectedDataRowsAsDataRowView[0]["PrintAll"] = false;
            }
        }

        private void RdbAll_Click(System.Object sender, System.EventArgs e)
        {
            if (rdbAll.Checked)
            {
                this.FgrdPrintList.SelectedDataRowsAsDataRowView[0]["PrintAll"] = true;
            }
        }

        private void FgrdPrintList_Click(System.Object sender, System.EventArgs e)
        {
            // SetGroupBoxVisibleInVisible;
        }

        private void BtnOK_Click(System.Object sender, System.EventArgs e)
        {
            ActionOK();
        }

        private void FgrdPrintList_DoubleClickCell(System.Object Sender, SourceGrid.CellContextEventArgs e)
        {
            FGridDV[e.CellContext.Position.Row - 1]["Print"] = (System.Object)((!((Boolean)FGridDV[e.CellContext.Position.Row - 1]["Print"])));
            SendKeys.Send("{ESC}");

            // SetGroupBoxVisibleInVisible;
        }

        #endregion

        #region User generated code

        /// <summary>
        /// Prints all the data from report it gets as parameter
        /// </summary>
        /// <returns>void</returns>
        private void PrintAll(System.Object ReportName)
        {
            Int32 Counter;
            TCmdMPartner cmd;

            this.Cursor = Cursors.WaitCursor;
            EnableDisableUI(false);

            // go trhough all the Rows in datatable (now 4), and print the one given as parameter.
            for (Counter = 0; Counter <= FPrintTable.Rows.Count - 1; Counter += 1)
            {
                if (FPrintTable.Rows[Counter]["Section Report"] == ReportName)
                {
                    lblPrintInfo.Text = "Printing report '" + ReportName.ToString() + "'...";
                    Application.DoEvents();

                    // Progress call:
                    cmd = new TCmdMPartner();
                    cmd.RunPrintPartner(this, FPartnerKey, (FPrintTable.Rows[Counter]["FileName"].ToString()), 0);
                }
            }

            this.Cursor = Cursors.Default;
            EnableDisableUI(true);
        }

        /// <summary>
        /// this procedure can be used to print the currently selected data from each tab This is in use yet.
        /// </summary>
        /// <returns>void</returns>
        private void PrintRow(System.Object ReportName)
        {
            Int32 Counter;
            DataRow SelectedRow;

            this.Cursor = Cursors.WaitCursor;
            EnableDisableUI(false);

            // go through all the Rows in datatable (now 4), and get the currentlyselected data from tab gives as parameter.
            for (Counter = 0; Counter <= FPrintTable.Rows.Count - 1; Counter += 1)
            {
                if (FPrintTable.Rows[Counter]["Section Report"] == ReportName)
                {
                    lblPrintInfo.Text = "Printing report '" + ReportName.ToString() + "'...";
                    Application.DoEvents();
                    SelectedRow = FDelegateDataRowOfCurrentlySelectedRecord(Counter);
                    MessageBox.Show(((PSubscriptionRow)SelectedRow).PublicationCode.ToString());
                }
            }

            this.Cursor = Cursors.Default;
            EnableDisableUI(true);
        }

        /// <summary>
        /// Here the GroupBox is set to visible or invisible ( Is the PrintAll selected or not).
        /// </summary>
        /// <returns>void</returns>
        private void SetGroupBoxVisibleInVisible()
        {
            // If the Row in DataGrid is selected to Print, show the GroupBox.
            if (((Boolean) this.FgrdPrintList.SelectedDataRowsAsDataRowView[0]["Print"]) == true)
            {
                if (!grpPrintScope.Visible)
                {
                    this.grpPrintScope.Visible = true;

                    if (((Boolean) this.FgrdPrintList.SelectedDataRowsAsDataRowView[0]["PrintAll"]) == true)
                    {
                        this.rdbAll.Checked = true;
                    }
                    else
                    {
                        this.rdbSelected.Checked = true;
                    }
                }
            }
            else
            {
                this.grpPrintScope.Visible = false;
            }
        }

        /// <summary>
        /// Set the parameters for this screen.
        /// </summary>
        /// <returns>void</returns>
        public void SetParameters(Int32 SelectedTab,
            TDelegateDataRowOfCurrentlySelectedRecord ADelegateDataRowOfCurrentlySelectedRecord,
            Boolean HasChanges,
            Int64 PartnerKey)
        {
            // Are there any unsaved data at PartnerEdit screen.
            FHasChanges = HasChanges;

            // Current ParterKey.
            FPartnerKey = PartnerKey;

            // Currently selected Tab, see orger above (top of file)
            FSelectedTab = SelectedTab;

            // Delegate function
            FDelegateDataRowOfCurrentlySelectedRecord = ADelegateDataRowOfCurrentlySelectedRecord;

            // Create the DataTable, columns and DataRows.
            CreateColumns();

            // DataBind the DataGrid (to the table just created)
            SetupDataGridDataBinding();

            // Setup the DataGrid's visual appearance
            SetupDataGridVisualAppearance();

            FgrdPrintList.Selection.SelectRow(1, true);

            // As long as we don't implement the grpPrintScope selection, we reduce the Form's height to prevent a void space
            this.ClientSize = new System.Drawing.Size(346, 182);
        }

        /// <summary>
        /// Actions to do when "Print" button is pressed.
        /// </summary>
        /// <returns>void</returns>
        private void ActionOK()
        {
            Int32 Counter;

            // If there is unsaved data at PartnerEdit screen, do nothing.
            if (FHasChanges)
            {
                MessageBox.Show(Resourcestrings.StrErrorNeedToSavePartner1 + Resourcestrings.StrErrorPrintPartner2,
                    Resourcestrings.StrErrorNeedToSavePartnerTitle);
            }
            else
            {
                // Move the Dialog to the left upper corner of the screen so the user can see UI updates on this Form (otherwise this Form would be covered by 4GL)
                FPreviousLocation = this.Location;
                this.Location = new System.Drawing.Point(0, 0);

                // Print all data that the user want to print.
                for (Counter = 0; Counter <= FGridDV.Count - 1; Counter += 1)
                {
                    if (((Boolean)FGridDV[Counter]["Print"]) == true)
                    {
                        if (((Boolean)FGridDV[Counter]["PrintAll"]) == true)
                        {
                            PrintAll(FGridDV[Counter]["Section Report"]);
                        }
                        else
                        {
                            PrintRow(FGridDV[Counter]["Section Report"]);
                        }
                    }
                }

                this.BringToFront();

                // Restore previous location of the Form (normally is was centered on the screen)
                this.Location = FPreviousLocation;
                btnCancel.Focus();
                stbMain.Panels[stbMain.Panels.IndexOf(stpInfo)].Text = "Printing done.";
            }
        }

        /// <summary>
        /// DataBing the DataGrid
        /// </summary>
        /// <returns>void</returns>
        private void SetupDataGridDataBinding()
        {
            FGridDV = FPrintTable.DefaultView;
            FGridDV.AllowNew = false;
            FGridDV.AllowEdit = true;
            FGridDV.AllowDelete = false;

            // DataBind the DataGrid
            FgrdPrintList.DataSource = new DevAge.ComponentModel.BoundDataView(FGridDV);
        }

        /// <summary>
        /// Sets up the visual appearance of the Grid.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected void SetupDataGridVisualAppearance()
        {
            int OriginalRecordListWidth = FgrdPrintList.Width;

            FgrdPrintList.AutoSizeCells();
            FgrdPrintList.Width = OriginalRecordListWidth; /// it is necessary to reassign the width because the columns don't take up the maximum width
        }

        /// <summary>
        /// Create the DataTable and Datacolums and DataRows with values.
        /// </summary>
        /// <returns>void</returns>
        private void CreateColumns()
        {
            DataColumn column1;
            DataRow Row1;
            Int32 i;

            // Here is created the table, that is later used by DataGrid.
            // Now here is only 4 Rows in the table, but eventually there should be seven together,
            // Create the DataTable
            FPrintTable = new DataTable("PrintTable");

            // Create first column.
            column1 = new DataColumn();
            column1.DataType = System.Type.GetType("System.Boolean");
            column1.ColumnName = "Print";
            column1.ReadOnly = false;
            column1.Unique = false;

            // Add the column to DataTable
            FPrintTable.Columns.Add(column1);
            column1 = new DataColumn();
            column1.DataType = System.Type.GetType("System.String");
            column1.ColumnName = "Section Report";
            column1.ReadOnly = true;
            column1.Unique = true;
            FPrintTable.Columns.Add(column1);
            column1 = new DataColumn();
            column1.DataType = System.Type.GetType("System.Boolean");
            column1.ColumnName = "PrintAll";
            column1.ReadOnly = false;
            column1.Unique = false;
            FPrintTable.Columns.Add(column1);
            column1 = new DataColumn();
            column1.DataType = System.Type.GetType("System.String");
            column1.ColumnName = "FileName";
            column1.ReadOnly = true;
            column1.Unique = true;
            FPrintTable.Columns.Add(column1);

            // Here is added the actual values to the Table created above.
            // We use integers here to identify the Tabselected,
            // Go throught all the 4 Rows.
            for (i = 0; i <= 3; i += 1)
            {
                Row1 = FPrintTable.NewRow();

                if (i == FSelectedTab)
                {
                    Row1["Print"] = true;
                }
                else
                {
                    Row1["Print"] = false;
                }

                if (i == 0)
                {
                    // 0 = Subscription
                    Row1["Section Report"] = "Subscriptions";
                    Row1["FileName"] = "pa61000p.p";
                }

                if (i == 1)
                {
                    // 1 = Interests
                    Row1["Section Report"] = "Interests";
                    Row1["FileName"] = "ma1500p.p";
                }

                if (i == 2)
                {
                    // 2 = Contacts
                    Row1["Section Report"] = "Contacts";
                    Row1["FileName"] = "ma1300p.p";
                }

                if (i == 3)
                {
                    // 3 = Reminders
                    Row1["Section Report"] = "Reminders";
                    Row1["FileName"] = "ma1600p.p";
                }

                Row1["PrintAll"] = true;

                // the just created Row is added to DataTable
                FPrintTable.Rows.Add(Row1);
            }

            FgrdPrintList.AddCheckBoxColumn("Print", FPrintTable.Columns["Print"]);
            FgrdPrintList.AddTextColumn("Section Report", FPrintTable.Columns["Section Report"],
                ((short)(FgrdPrintList.Width - FgrdPrintList.Columns[0].Width - 9)));
        }

        /// <summary>
        /// <summary> Clean up any resources being used. </summary>
        /// </summary>
        /// <returns>void</returns>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
        }

        private void EnableDisableUI(bool AEnable)
        {
            FgrdPrintList.Enabled = AEnable;
            grpPrintScope.Enabled = AEnable;
            btnHelp.Enabled = AEnable;
            btnCancel.Enabled = AEnable;
            btnOK.Enabled = AEnable;

            if (AEnable)
            {
                lblPrintInfo.SendToBack();
                sbtForm.SetStatusBarText(this.btnOK, "Print All Selected Reports");
            }
            else
            {
                lblPrintInfo.BringToFront();
                sbtForm.SetStatusBarText(this.btnOK, "Printing selected report(s). Please wait...");
            }

            Application.DoEvents();
        }

        public TPartnerPrintSectionDialog() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        #endregion
    }


    /// <summary>Delegate function that returns currently selected DataRow from a Tab wanted. See tab order above.</summary>
    public delegate DataRow TDelegateDataRowOfCurrentlySelectedRecord(Int32 Selectedtab);


    public class PartnerPrintSectionDialog
    {
        public const String StrAddPartnerType = "Add";
        public const String StrAddPartnerTypeTitle = "Add Partner Type?";
    }
}