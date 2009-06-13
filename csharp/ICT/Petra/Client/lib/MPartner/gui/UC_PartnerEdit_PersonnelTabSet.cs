/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       christiank
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
using System.Resources;
using Ict.Petra.Client.App.Core;
using System.Globalization;
using Ict.Petra.Client.CommonControls;
using Ict.Common.Controls;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// UserControl that makes up the TabControl for 'Personnel Data' in Partner Edit
    /// screen.
    ///
    /// @Comment At the moment this UserControl is only a basic implementation,
    ///   just to give us an idea how things might look and work in the future.
    public class TUC_PartnerEdit_PersonnelTabSet : System.Windows.Forms.UserControl
    {
        private static System.Data.DataSet UMultiTableDS;
        private static char[] SECTION_KEYS = new char[]
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
            'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q'
        };
        private static string[] SECTION_NAMES =
            new string[] {
            "Applications", "Interview", "Personal Data", "Passport Details",
            "Personal Documents", "Special Needs", "Valuable Items", "Office Specific Data",
            "Professional Areas", "Languages", "Abilities", "Previous Experience", "Vision",
            "Commitment Period", "Job Assignments", "Progress Reports", "Personal Budgets"
        };

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private TTabVersatile tabPersonnelTab;
        private System.Windows.Forms.TabPage tbpPersonnelIndividualData;
        private System.Windows.Forms.Button btnDeleteAddress;
        private System.Windows.Forms.Button btnNewAddress;

        /// <summary>GroupBox1: System.Windows.Forms.GroupBox;</summary>
        private System.Windows.Forms.Button btnEditAddress;
        private System.Windows.Forms.DataGrid grdPersonnelItemList;
        private System.Windows.Forms.ImageList imlTabIcons;
        private System.Windows.Forms.ImageList imlButtonIcons;
        private System.Windows.Forms.Panel pnlIndividualDataList;
        private System.Windows.Forms.Splitter splIndividualData;
        private System.Windows.Forms.Panel pnlIndividualDataDetails;
        private System.Windows.Forms.Panel pnlPersonnelItemList;
        private System.Windows.Forms.Splitter splItemListDetail;
        private System.Windows.Forms.DataGrid grdPersonnelSections;
        private System.Windows.Forms.Button btnResizePersonnelItemList;

        /// <summary>Private Declarations</summary>
        private DataSet FPersonnelSectionsDS;
        private Boolean FIndividualDataListMaximised;
        private int FIndividualDataListMinimisedSize;

        public DataSet MultiTableDS
        {
            get
            {
                return UMultiTableDS;
            }

            set
            {
                UMultiTableDS = value;
            }
        }


        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerEdit_PersonnelTabSet));
            this.components = new System.ComponentModel.Container();
            this.imlButtonIcons = new System.Windows.Forms.ImageList(this.components);
            this.imlTabIcons = new System.Windows.Forms.ImageList(this.components);
            this.tabPersonnelTab = new Ict.Common.Controls.TTabVersatile();
            this.tbpPersonnelIndividualData = new System.Windows.Forms.TabPage();
            this.pnlIndividualDataDetails = new System.Windows.Forms.Panel();
            this.splIndividualData = new System.Windows.Forms.Splitter();
            this.pnlIndividualDataList = new System.Windows.Forms.Panel();
            this.btnResizePersonnelItemList = new System.Windows.Forms.Button();
            this.grdPersonnelItemList = new System.Windows.Forms.DataGrid();
            this.btnDeleteAddress = new System.Windows.Forms.Button();
            this.btnEditAddress = new System.Windows.Forms.Button();
            this.btnNewAddress = new System.Windows.Forms.Button();
            this.splItemListDetail = new System.Windows.Forms.Splitter();
            this.pnlPersonnelItemList = new System.Windows.Forms.Panel();
            this.grdPersonnelSections = new System.Windows.Forms.DataGrid();
            this.tabPersonnelTab.SuspendLayout();
            this.tbpPersonnelIndividualData.SuspendLayout();
            this.pnlIndividualDataList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPersonnelItemList)).BeginInit();
            this.pnlPersonnelItemList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPersonnelSections)).BeginInit();
            this.SuspendLayout();

            //
            // imlButtonIcons
            //
            this.imlButtonIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imlButtonIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject('i' + "mlButtonIcons.ImageStream")));
            this.imlButtonIcons.TransparentColor = System.Drawing.Color.Transparent;

            //
            // imlTabIcons
            //
            this.imlTabIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imlTabIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject('i' + "mlTabIcons.ImageStream")));
            this.imlTabIcons.TransparentColor = System.Drawing.Color.Transparent;

            //
            // tabPersonnelTab
            //
            this.tabPersonnelTab.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.tabPersonnelTab.Controls.Add(this.tbpPersonnelIndividualData);
            this.tabPersonnelTab.HotTrack = true;
            this.tabPersonnelTab.ImageList = this.imlTabIcons;
            this.tabPersonnelTab.Location = new System.Drawing.Point(1, 1);
            this.tabPersonnelTab.Name = "tabPersonnelTab";
            this.tabPersonnelTab.SelectedIndex = 0;
            this.tabPersonnelTab.Size = new System.Drawing.Size(752, 470);
            this.tabPersonnelTab.TabIndex = 87;

            //
            // tbpPersonnelIndividualData
            //
            this.tbpPersonnelIndividualData.BackColor = System.Drawing.SystemColors.Control;
            this.tbpPersonnelIndividualData.Controls.Add(this.pnlIndividualDataDetails);
            this.tbpPersonnelIndividualData.Controls.Add(this.splIndividualData);
            this.tbpPersonnelIndividualData.Controls.Add(this.pnlIndividualDataList);
            this.tbpPersonnelIndividualData.Controls.Add(this.splItemListDetail);
            this.tbpPersonnelIndividualData.Controls.Add(this.pnlPersonnelItemList);
            this.tbpPersonnelIndividualData.DockPadding.All = 2;
            this.tbpPersonnelIndividualData.ImageIndex = 0;
            this.tbpPersonnelIndividualData.Location = new System.Drawing.Point(4, 23);
            this.tbpPersonnelIndividualData.Name = "tbpPersonnelIndividualData";
            this.tbpPersonnelIndividualData.Size = new System.Drawing.Size(744, 443);
            this.tbpPersonnelIndividualData.TabIndex = 0;
            this.tbpPersonnelIndividualData.Text = "Individual Data";

            //
            // pnlIndividualDataDetails
            //
            this.pnlIndividualDataDetails.AutoScroll = true;
            this.pnlIndividualDataDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlIndividualDataDetails.Location = new System.Drawing.Point(204, 105);
            this.pnlIndividualDataDetails.Name = "pnlIndividualDataDetails";
            this.pnlIndividualDataDetails.Size = new System.Drawing.Size(538, 336);
            this.pnlIndividualDataDetails.TabIndex = 94;

            //
            // splIndividualData
            //
            this.splIndividualData.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splIndividualData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splIndividualData.Dock = System.Windows.Forms.DockStyle.Top;
            this.splIndividualData.Location = new System.Drawing.Point(204, 102);
            this.splIndividualData.MinExtra = 53;
            this.splIndividualData.MinSize = 52;
            this.splIndividualData.Name = "splIndividualData";
            this.splIndividualData.Size = new System.Drawing.Size(538, 3);
            this.splIndividualData.TabIndex = 93;
            this.splIndividualData.TabStop = false;

            //
            // pnlIndividualDataList
            //
            this.pnlIndividualDataList.Controls.Add(this.btnResizePersonnelItemList);
            this.pnlIndividualDataList.Controls.Add(this.grdPersonnelItemList);
            this.pnlIndividualDataList.Controls.Add(this.btnDeleteAddress);
            this.pnlIndividualDataList.Controls.Add(this.btnEditAddress);
            this.pnlIndividualDataList.Controls.Add(this.btnNewAddress);
            this.pnlIndividualDataList.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlIndividualDataList.Location = new System.Drawing.Point(204, 2);
            this.pnlIndividualDataList.Name = "pnlIndividualDataList";
            this.pnlIndividualDataList.Size = new System.Drawing.Size(538, 100);
            this.pnlIndividualDataList.TabIndex = 5;

            //
            // btnResizePersonnelItemList
            //
            this.btnResizePersonnelItemList.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnResizePersonnelItemList.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnResizePersonnelItemList.ImageIndex = 3;
            this.btnResizePersonnelItemList.ImageList = this.imlButtonIcons;
            this.btnResizePersonnelItemList.Location = new System.Drawing.Point(458, 80);
            this.btnResizePersonnelItemList.Name = "btnResizePersonnelItemList";
            this.btnResizePersonnelItemList.Size = new System.Drawing.Size(20, 18);
            this.btnResizePersonnelItemList.TabIndex = 6;
            this.btnResizePersonnelItemList.Click += new System.EventHandler(this.BtnResizePersonnelItemList_Click);

            //
            // grdPersonnelItemList
            //
            this.grdPersonnelItemList.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grdPersonnelItemList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdPersonnelItemList.CaptionVisible = false;
            this.grdPersonnelItemList.DataMember = "";
            this.grdPersonnelItemList.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.grdPersonnelItemList.Location = new System.Drawing.Point(2, 2);
            this.grdPersonnelItemList.Name = "grdPersonnelItemList";
            this.grdPersonnelItemList.Size = new System.Drawing.Size(454, 96);
            this.grdPersonnelItemList.TabIndex = 4;

            //
            // btnDeleteAddress
            //
            this.btnDeleteAddress.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnDeleteAddress.Enabled = false;
            this.btnDeleteAddress.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnDeleteAddress.ImageIndex = 2;
            this.btnDeleteAddress.ImageList = this.imlButtonIcons;
            this.btnDeleteAddress.Location = new System.Drawing.Point(460, 50);
            this.btnDeleteAddress.Name = "btnDeleteAddress";
            this.btnDeleteAddress.Size = new System.Drawing.Size(76, 23);
            this.btnDeleteAddress.TabIndex = 2;
            this.btnDeleteAddress.Text = "     &Delete";
            this.btnDeleteAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // btnEditAddress
            //
            this.btnEditAddress.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnEditAddress.Enabled = false;
            this.btnEditAddress.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnEditAddress.ImageIndex = 1;
            this.btnEditAddress.ImageList = this.imlButtonIcons;
            this.btnEditAddress.Location = new System.Drawing.Point(460, 26);
            this.btnEditAddress.Name = "btnEditAddress";
            this.btnEditAddress.Size = new System.Drawing.Size(76, 23);
            this.btnEditAddress.TabIndex = 1;
            this.btnEditAddress.Text = "       &Edit";
            this.btnEditAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // btnNewAddress
            //
            this.btnNewAddress.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnNewAddress.Enabled = false;
            this.btnNewAddress.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnNewAddress.ImageIndex = 0;
            this.btnNewAddress.ImageList = this.imlButtonIcons;
            this.btnNewAddress.Location = new System.Drawing.Point(460, 2);
            this.btnNewAddress.Name = "btnNewAddress";
            this.btnNewAddress.Size = new System.Drawing.Size(76, 23);
            this.btnNewAddress.TabIndex = 1;
            this.btnNewAddress.Text = "       &New";
            this.btnNewAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // splItemListDetail
            //
            this.splItemListDetail.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splItemListDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splItemListDetail.Location = new System.Drawing.Point(201, 2);
            this.splItemListDetail.Name = "splItemListDetail";
            this.splItemListDetail.Size = new System.Drawing.Size(3, 439);
            this.splItemListDetail.TabIndex = 2;
            this.splItemListDetail.TabStop = false;

            //
            // pnlPersonnelItemList
            //
            this.pnlPersonnelItemList.AutoScroll = true;
            this.pnlPersonnelItemList.Controls.Add(this.grdPersonnelSections);
            this.pnlPersonnelItemList.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlPersonnelItemList.Location = new System.Drawing.Point(2, 2);
            this.pnlPersonnelItemList.Name = "pnlPersonnelItemList";
            this.pnlPersonnelItemList.Size = new System.Drawing.Size(199, 439);
            this.pnlPersonnelItemList.TabIndex = 1;

            //
            // grdPersonnelSections
            //
            this.grdPersonnelSections.AllowNavigation = false;
            this.grdPersonnelSections.AllowSorting = false;
            this.grdPersonnelSections.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdPersonnelSections.CaptionVisible = false;
            this.grdPersonnelSections.ColumnHeadersVisible = false;
            this.grdPersonnelSections.DataMember = "";
            this.grdPersonnelSections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdPersonnelSections.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.grdPersonnelSections.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.grdPersonnelSections.Location = new System.Drawing.Point(0, 0);
            this.grdPersonnelSections.Name = "grdPersonnelSections";
            this.grdPersonnelSections.ParentRowsVisible = false;
            this.grdPersonnelSections.ReadOnly = true;
            this.grdPersonnelSections.RowHeadersVisible = false;
            this.grdPersonnelSections.Size = new System.Drawing.Size(199, 439);
            this.grdPersonnelSections.TabIndex = 0;
            this.grdPersonnelSections.MouseUp += new MouseEventHandler(this.GrdPersonnelSections_MouseUp);

            //
            // TUC_PartnerEdit_PersonnelTabSet
            //
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Controls.Add(this.tabPersonnelTab);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_PartnerEdit_PersonnelTabSet";
            this.Size = new System.Drawing.Size(752, 472);
            this.tabPersonnelTab.ResumeLayout(false);
            this.tbpPersonnelIndividualData.ResumeLayout(false);
            this.pnlIndividualDataList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPersonnelItemList)).EndInit();
            this.pnlPersonnelItemList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPersonnelSections)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        public TUC_PartnerEdit_PersonnelTabSet() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void BtnResizePersonnelItemList_Click(System.Object sender, System.EventArgs e)
        {
            if (!FIndividualDataListMaximised)
            {
                FIndividualDataListMaximised = true;
                FIndividualDataListMinimisedSize = pnlIndividualDataList.Height;
                pnlIndividualDataList.Height = 330;
            }
            else
            {
                FIndividualDataListMaximised = false;
                pnlIndividualDataList.Height = FIndividualDataListMinimisedSize;
            }
        }

        /// <summary>
        /// procedure tabPersonnelTab_SelectedIndexChanged(sender: System.Object; e: System.EventArgs);    procedure btnEditAddress_Click(sender: System.Object; e: System.EventArgs);    procedure btnNewAddress_Click(sender: System.Object; e:
        /// System.EventArgs);    procedure btnDeleteAddress_Click(sender: System.Object; e: System.EventArgs);
        /// </summary>
        /// <returns>void</returns>
        private void GrdPersonnelSections_MouseUp(System.Object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Drawing.Point Pt;
            DataGrid.HitTestInfo Hti;
            Pt = new Point(e.X, e.Y);
            Hti = grdPersonnelSections.HitTest(Pt);

            if (Hti.Type == DataGrid.HitTestType.Cell)
            {
                grdPersonnelSections.CurrentCell = new DataGridCell(Hti.Row, Hti.Column);
                grdPersonnelSections.Select(Hti.Row);
            }
        }

        /*
         * procedure TUC_PartnerEdit_PersonnelTabSet.btnEditAddress_Click(sender: System.Object;
         * e: System.EventArgs);
         * begin
         *
         * end;
         *
         * procedure TUC_PartnerEdit_PersonnelTabSet.btnNewAddress_Click(sender: System.Object;
         * e: System.EventArgs);
         * begin
         *
         * end;
         *
         * procedure TUC_PartnerEdit_PersonnelTabSet.btnDeleteAddress_Click(sender: System.Object;
         * e: System.EventArgs);
         * begin
         *
         * end;
         *
         * procedure TUC_PartnerEdit_PersonnelTabSet.tabPersonnelTab_SelectedIndexChanged(sender: System.Object;
         * e: System.EventArgs);
         * begin
         * case tabPersonnelTab.SelectedIndex of
         * 0:
         * begin
         * end;
         *
         * 1:
         * begin
         * end;
         *
         * 2:
         * begin
         *
         * end;
         * end;
         * end;
         */

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

        public void InitialiseUserControl()
        {
            DataTable PersonnelSectionsTable;
            DataColumn ColumnListKey;
            DataColumn ColumnSection;
            DataColumn ColumnSectionElementCount;
            DataView PersonnelSectionsDataView;
            DataGridTableStyle TableStyle;
            TDataGridTextBoxColumnNotEditableNotNavigable NotEditableTextColumn;

            tbpPersonnelIndividualData.Enabled = false;

            // MessageBox.Show('PersonnelTabSet.InitialiseUserControl');
            FPersonnelSectionsDS = new DataSet("PersonnelSections");
            PersonnelSectionsTable = new DataTable("PersonnelSections");
            ColumnListKey = new DataColumn("ListKey", System.Type.GetType("System.String"));
            ColumnSection = new DataColumn("Section", System.Type.GetType("System.String"));
            ColumnSectionElementCount = new DataColumn("SectionElementCount", System.Type.GetType("System.String"));
            PersonnelSectionsTable.Columns.AddRange(new DataColumn[] { ColumnListKey, ColumnSection, ColumnSectionElementCount });
            FPersonnelSectionsDS.Tables.Add(PersonnelSectionsTable);
            SetupPersonnelSectionDataSet();
            TableStyle = new DataGridTableStyle();
            TableStyle.MappingName = "PersonnelSections";
            TableStyle.ColumnHeadersVisible = false;
            TableStyle.RowHeadersVisible = false;
            NotEditableTextColumn = new TDataGridTextBoxColumnNotEditableNotNavigable();
            NotEditableTextColumn.MappingName = "ListKey";
            NotEditableTextColumn.Width = 20;             /// 17
            TableStyle.GridColumnStyles.Add(NotEditableTextColumn);
            NotEditableTextColumn = new TDataGridTextBoxColumnNotEditableNotNavigable();
            NotEditableTextColumn.MappingName = "Section";
            NotEditableTextColumn.Width = 145;             /// 155
            TableStyle.GridColumnStyles.Add(NotEditableTextColumn);
            NotEditableTextColumn = new TDataGridTextBoxColumnNotEditableNotNavigable();
            NotEditableTextColumn.MappingName = "SectionElementCount";
            NotEditableTextColumn.Width = 30;             /// 25
            NotEditableTextColumn.Alignment = HorizontalAlignment.Center;
            TableStyle.GridColumnStyles.Add(NotEditableTextColumn);
            grdPersonnelSections.TableStyles.Clear();
            grdPersonnelSections.TableStyles.Add(TableStyle);
            FPersonnelSectionsDS.Tables["PersonnelSections"].DefaultView.Sort = "ListKey ASC";
            PersonnelSectionsDataView = FPersonnelSectionsDS.Tables["PersonnelSections"].DefaultView;
            grdPersonnelSections.DataSource = PersonnelSectionsDataView;
            grdPersonnelSections.Select(0);
        }

        private void SetupPersonnelSectionDataSet()
        {
            DataTable PersonnelSectionsTable;
            DataRow SectionDataRow;
            Int16 Counter;

            PersonnelSectionsTable = FPersonnelSectionsDS.Tables["PersonnelSections"];

            for (Counter = 0; Counter <= 16; Counter += 1)
            {
                SectionDataRow = PersonnelSectionsTable.NewRow();
                SectionDataRow["ListKey"] = SECTION_KEYS + ".";
                SectionDataRow["Section"] = SECTION_NAMES + "...";

                if ((Counter == 2) || (Counter == 5))
                {
                    SectionDataRow["SectionElementCount"] = "Yes";
                }
                else
                {
                    SectionDataRow["SectionElementCount"] = '0';
                }

                PersonnelSectionsTable.Rows.Add(SectionDataRow);
            }
        }
    }
}