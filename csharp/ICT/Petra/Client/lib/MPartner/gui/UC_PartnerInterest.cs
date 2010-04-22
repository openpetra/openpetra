/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       martaj
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
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.CommonControls;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using SourceGrid;
using System.Globalization;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MPartner;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    ///
    /// Since this UserControl can be re-used, it is no longer necessary to recreate
    /// all the layout and fields and verification rules in other places than the
    /// Partner Edit screen.
    ///
    /// @Comment Info from Progress' Interest Maintenance Screen (ma1500e.w) has now
    ///   been transferred to Petra Interest Screen under a new tab called Interests.
    ///   Category, Interest, Country, Field, Level and Comment fields were
    ///   transferred as they were.
    ///
    /// @Comment A special thing about this screen are the two ComboBoxes
    ///   'Interest Category' and 'Interest'. The value for 'Interest Category' isn't
    ///   stored in the DB Table that this UserControl works with
    ///   (p_partner_interest), only the value for 'Interest'. Therefore we need to
    ///   set up 'Interest Category' whenever the PerformDataBinding method is
    ///   called to show the correct entry that goes with the value of
    ///   'Interest' (this is done in procedure InitialiseDependentComboBoxes).
    ///   The other thing that needs to be done: when the user chooses a different
    ///   value for 'Interest Category', 'Interest' needs to be filtered to only
    ///   show records that go with the category that got selected in
    ///   'Interest Category' (this is done in procedure UpdateDependentComboBoxes).
    /// </summary>
    public class TUCPartnerInterest : TPetraUserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlInterest;
        private System.Windows.Forms.GroupBox grpInterests;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblInterest;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.TextBox txtLevel;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.ToolTip tipLevel;
        private TCmbAutoPopulated cmbCountryCode;
        private TCmbAutoPopulated cmbCategory;

        /// <summary>cmbCategory: System.Windows.Forms.ComboBox;</summary>
        private TCmbAutoPopulated cmbInterest;
        private TbtnCreated btnCreatedInterest;
        private TexpTextBoxStringLengthCheck expStringLengthCheckInterest;
        private TtxtAutoPopulatedButtonLabel TtxtAutoPopulatedButtonLabel1;

        /// <summary>used for calling the Delegate Function to retrieve a partner key</summary>
        protected TDelegateGetPartnerShortName FDelegateGetPartnerShortName;

        /// <summary>holds the DataSet that contains most data that is used on the screen</summary>
        private new PartnerEditTDS FMainDS;
        private PInterestTable FInterestDT;
        private PInterestCategoryTable FInterestCategoryDT;

        /// <summary>Currently selected Interest</summary>
        private String FInterest;

        /// <summary>DataView for the interest record we are working with</summary>
        private DataView FPartnerInterestDV;
        private DataView FInterestDV;

        /// <summary>Interest value we are working with.</summary>
        public String Interest
        {
            get
            {
                return FInterest;
            }

            set
            {
                /// Sets the Interest
                FInterest = value;

                if (FPartnerInterestDV == null)
                {
                }
                else
                {
                    FPartnerInterestDV.RowFilter = PPartnerInterestTable.GetInterestDBName() + " = '" + FInterest + "'";

                    // cmbInterest.Filter := PInterestTable.GetCategoryDBName() + ' = ''' +
                    // cmbCategory.Selectedvalue.ToString + '''';
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TUCPartnerInterest));
            this.components = new System.ComponentModel.Container();
            this.pnlInterest = new System.Windows.Forms.Panel();
            this.grpInterests = new System.Windows.Forms.GroupBox();
            this.TtxtAutoPopulatedButtonLabel1 = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.cmbCountryCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.cmbInterest = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.cmbCategory = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblComment = new System.Windows.Forms.Label();
            this.lblCountry = new System.Windows.Forms.Label();
            this.txtLevel = new System.Windows.Forms.TextBox();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblInterest = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.btnCreatedInterest = new Ict.Common.Controls.TbtnCreated();
            this.expStringLengthCheckInterest = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.tipLevel = new System.Windows.Forms.ToolTip(this.components);
            this.pnlInterest.SuspendLayout();
            this.grpInterests.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlInterest
            //
            this.pnlInterest.AutoScroll = true;
            this.pnlInterest.AutoScrollMinSize = new System.Drawing.Size(670, 240);
            this.pnlInterest.BackColor = System.Drawing.SystemColors.Control;
            this.pnlInterest.Controls.Add(this.grpInterests);
            this.pnlInterest.Controls.Add(this.btnCreatedInterest);
            this.pnlInterest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInterest.Location = new System.Drawing.Point(0, 0);
            this.pnlInterest.Name = "pnlInterest";
            this.pnlInterest.Size = new System.Drawing.Size(676, 304);
            this.pnlInterest.TabIndex = 0;
            this.pnlInterest.Tag = "CustomDisableAlthoughInvisible";
            this.pnlInterest.Paint += new PaintEventHandler(this.PnlInterest_Paint);

            //
            // grpInterests
            //
            this.grpInterests.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInterests.Controls.Add(this.TtxtAutoPopulatedButtonLabel1);
            this.grpInterests.Controls.Add(this.txtComment);
            this.grpInterests.Controls.Add(this.cmbCountryCode);
            this.grpInterests.Controls.Add(this.cmbInterest);
            this.grpInterests.Controls.Add(this.cmbCategory);
            this.grpInterests.Controls.Add(this.lblComment);
            this.grpInterests.Controls.Add(this.lblCountry);
            this.grpInterests.Controls.Add(this.txtLevel);
            this.grpInterests.Controls.Add(this.lblLevel);
            this.grpInterests.Controls.Add(this.lblInterest);
            this.grpInterests.Controls.Add(this.lblCategory);
            this.grpInterests.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpInterests.Location = new System.Drawing.Point(4, 4);
            this.grpInterests.Name = "grpInterests";
            this.grpInterests.Size = new System.Drawing.Size(650, 300);
            this.grpInterests.TabIndex = 0;
            this.grpInterests.TabStop = false;
            this.grpInterests.Tag = "CustomDisableAlthoughInvisible";
            this.grpInterests.Enter += new System.EventHandler(this.GrpInterests_Enter);

            //
            // TtxtAutoPopulatedButtonLabel1
            //
            this.TtxtAutoPopulatedButtonLabel1.ASpecialSetting = true;
            this.TtxtAutoPopulatedButtonLabel1.ButtonText = "Field";
            this.TtxtAutoPopulatedButtonLabel1.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TtxtAutoPopulatedButtonLabel1.ButtonWidth = 72;
            this.TtxtAutoPopulatedButtonLabel1.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.OccupationList;
            this.TtxtAutoPopulatedButtonLabel1.Location = new System.Drawing.Point(40, 90);
            this.TtxtAutoPopulatedButtonLabel1.MaxLength = 32767;
            this.TtxtAutoPopulatedButtonLabel1.Name = "TtxtAutoPopulatedButtonLabel1";
            this.TtxtAutoPopulatedButtonLabel1.PartnerClass = "";
            this.TtxtAutoPopulatedButtonLabel1.PreventFaultyLeaving = false;
            this.TtxtAutoPopulatedButtonLabel1.ReadOnly = false;
            this.TtxtAutoPopulatedButtonLabel1.Size = new System.Drawing.Size(390, 23);
            this.TtxtAutoPopulatedButtonLabel1.TabIndex = 23;
            this.TtxtAutoPopulatedButtonLabel1.TextBoxWidth = 100;
            this.TtxtAutoPopulatedButtonLabel1.Load += new System.EventHandler(this.TtxtAutoPopulatedButtonLabel1_Load);

            //
            // txtComment
            //
            this.txtComment.Anchor =
                ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top |
                                                        System.Windows.Forms.AnchorStyles.Bottom) |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComment.Location = new System.Drawing.Point(114, 140);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(524, 120);
            this.txtComment.TabIndex = 22;
            this.txtComment.Tag = "CustomDisableAlthoughInvisible";
            this.txtComment.Text = "txtComment";
            this.txtComment.TextChanged += new System.EventHandler(this.TxtComment_TextChanged);

            //
            // cmbCountryCode
            //
            this.cmbCountryCode.ComboBoxWidth = 50;
            this.cmbCountryCode.Filter = null;
            this.cmbCountryCode.ListTable = TCmbAutoPopulated.TListTableEnum.CountryList;
            this.cmbCountryCode.Location = new System.Drawing.Point(114, 65);
            this.cmbCountryCode.Name = "cmbCountryCode";
            this.cmbCountryCode.SelectedItem = ((System.Object)(resources.GetObject("cm" + "bCountryCode.SelectedItem")));
            this.cmbCountryCode.SelectedValue = null;
            this.cmbCountryCode.Size = new System.Drawing.Size(468, 22);
            this.cmbCountryCode.TabIndex = 17;
            this.cmbCountryCode.Tag = "CustomDisableAlthoughInvisible";

            //
            // cmbInterest
            //
            this.cmbInterest.ComboBoxWidth = 130;
            this.cmbInterest.Filter = null;
            this.cmbInterest.ListTable = TCmbAutoPopulated.TListTableEnum.InterestList;
            this.cmbInterest.Location = new System.Drawing.Point(114, 40);
            this.cmbInterest.Name = "cmbInterest";
            this.cmbInterest.SelectedItem = ((System.Object)(resources.GetObject("cmbIn" + "terest.SelectedItem")));
            this.cmbInterest.SelectedValue = null;
            this.cmbInterest.Size = new System.Drawing.Size(234, 22);
            this.cmbInterest.TabIndex = 15;
            this.cmbInterest.Tag = "CustomDisableAlthoughInvisible";

            //
            // cmbCategory
            //
            this.cmbCategory.ComboBoxWidth = 130;
            this.cmbCategory.Filter = null;
            this.cmbCategory.ListTable = TCmbAutoPopulated.TListTableEnum.InterestCategoryList;
            this.cmbCategory.Location = new System.Drawing.Point(114, 15);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.SelectedItem = ((System.Object)(resources.GetObject("cmbCa" + "tegory.SelectedItem")));
            this.cmbCategory.SelectedValue = null;
            this.cmbCategory.Size = new System.Drawing.Size(234, 22);
            this.cmbCategory.TabIndex = 13;
            this.cmbCategory.Tag = "CustomDisableAlthoughInvisible";
            this.cmbCategory.SelectedValueChanged += new TSelectedValueChangedEventHandler(this.CmbCategory_SelectedValueChanged);

            //
            // lblComment
            //
            this.lblComment.Location = new System.Drawing.Point(24, 134);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(88, 23);
            this.lblComment.TabIndex = 21;
            this.lblComment.Text = "Co&mment:";
            this.lblComment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblCountry
            //
            this.lblCountry.Location = new System.Drawing.Point(24, 64);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(88, 23);
            this.lblCountry.TabIndex = 16;
            this.lblCountry.Text = "C&ountry Code:";
            this.lblCountry.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCountry.Click += new System.EventHandler(this.LblCountry_Click);

            //
            // txtLevel
            //
            this.txtLevel.Location = new System.Drawing.Point(114, 116);
            this.txtLevel.Name = "txtLevel";
            this.txtLevel.Size = new System.Drawing.Size(32, 20);
            this.txtLevel.TabIndex = 20;
            this.txtLevel.Tag = "CustomDisableAlthoughInvisible";
            this.txtLevel.Text = "00";
            this.txtLevel.TextChanged += new System.EventHandler(this.TxtLevel_TextChanged);

            //
            // lblLevel
            //
            this.lblLevel.Location = new System.Drawing.Point(24, 115);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(88, 23);
            this.lblLevel.TabIndex = 19;
            this.lblLevel.Text = "&Level:";
            this.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblInterest
            //
            this.lblInterest.Location = new System.Drawing.Point(24, 40);
            this.lblInterest.Name = "lblInterest";
            this.lblInterest.Size = new System.Drawing.Size(88, 23);
            this.lblInterest.TabIndex = 14;
            this.lblInterest.Text = "&Interest:";
            this.lblInterest.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // lblCategory
            //
            this.lblCategory.Location = new System.Drawing.Point(24, 16);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(88, 23);
            this.lblCategory.TabIndex = 12;
            this.lblCategory.Text = "&Category:";
            this.lblCategory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // btnCreatedInterest
            //
            this.btnCreatedInterest.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnCreatedInterest.CreatedBy = null;
            this.btnCreatedInterest.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedInterest.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedInterest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedInterest.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnCreatedInterest.Image")));
            this.btnCreatedInterest.Location = new System.Drawing.Point(656, 4);
            this.btnCreatedInterest.ModifiedBy = null;
            this.btnCreatedInterest.Name = "btnCreatedInterest";
            this.btnCreatedInterest.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedInterest.TabIndex = 23;
            this.btnCreatedInterest.Tag = "dontdisable";

            //
            // TUCPartnerInterest
            //
            this.Controls.Add(this.pnlInterest);
            this.Name = "TUCPartnerInterest";
            this.Size = new System.Drawing.Size(676, 304);
            this.pnlInterest.ResumeLayout(false);
            this.grpInterests.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        public TUCPartnerInterest() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void CmbCategory_SelectedValueChanged(System.Object Sender, System.EventArgs e)
        {
        }

        private void TtxtAutoPopulatedButtonLabel1_Load(System.Object sender, System.EventArgs e)
        {
        }

        private void TxtComment_TextChanged(System.Object sender, System.EventArgs e)
        {
        }

        private void TxtLevel_TextChanged(System.Object sender, System.EventArgs e)
        {
        }

        /// <summary>
        /// Sets this Usercontrol visible or unvisile true = visible, false = invisible.
        /// </summary>
        /// <returns>void</returns>
        public void MakeScreenInvisible(Boolean value)
        {
            // Set the groupbox of this UserControl to visible or invisible.
            this.grpInterests.Visible = value;
        }

        private void CmbInterestCategory_SelectedValueChanged(System.Object Sender, System.EventArgs e)
        {
            UpdateDependentComboBoxes();
        }

        /// <summary>
        /// FVerificationResultCollection:  TVerificationResultCollection; <summary> Clean up any resources being used. </summary>
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

        /// <summary>
        /// Sets the screenparts when editMode is on
        /// </summary>
        /// <returns>void</returns>
        public void EditModeForInterest()
        {
            PPartnerInterestRow RowInt;

            this.cmbInterest.Enabled = false;

            // if any Interest
            if (FPartnerInterestDV.Count > 0)
            {
                // get the selected Row.
                RowInt = (PPartnerInterestRow)FPartnerInterestDV[0].Row;
                this.cmbInterest.Text = RowInt.Interest;
                this.cmbCountryCode.Text = RowInt.Country;
                this.TtxtAutoPopulatedButtonLabel1.Text = RowInt.FieldKey.ToString();
                this.txtLevel.Text = RowInt.Level.ToString();
                this.txtComment.Text = RowInt.Comment;
            }
        }

        //* Sets the FPartnerInterestDV unable to edit.
        // Comment from ProcessEditRecord in the Subscription:
        /// <summary>
        /// Call EndEdit on all DataRows that are editable in the Detail UserControl. */
        /// </summary>
        /// <returns>void</returns>
        public void EndEdit()
        {
            FPartnerInterestDV.AllowEdit = false;
        }

        /// <summary>
        /// returns the Interest selected in combobox.
        /// </summary>
        /// <returns>void</returns>
        public String GetSelectedInterest()
        {
            String ReturnValue;

            ReturnValue = this.cmbInterest.SelectedItem.ToString();
            FInterest = ReturnValue;
            return ReturnValue;
        }

        /// <summary>
        /// Returns the selected dataRow (from PartnerInterest table)mj function GetSelectedDataRow(out tmpRow : PPartnerInterestRow): Boolean;
        /// </summary>
        /// <returns>void</returns>
        public DataTable GetPartnerInterestDT()
        {
            // var
            // Row : PPartnerInterestRow;
            // Row := FMainDS.PPartnerInterest.Row[0] as PPartnerInterestRow;
            MessageBox.Show("get partnerintersrt: " + FMainDS.Tables.Count.ToString());

            // result := FPartnerInterestDV.
            return FMainDS.PPartnerInterest;
        }

        /// <summary>
        /// Initialises Delegate Function to retrieve a partner key
        ///
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseDelegateGetPartnerShortName(TDelegateGetPartnerShortName ADelegateFunction)
        {
            // set the delegate function from the calling System.Object
            FDelegateGetPartnerShortName = ADelegateFunction;
        }

        /// <summary>
        /// DataBinds the controls on this UserControl
        ///
        /// </summary>
        /// <param name="AMainDS">DataSet that contains most data that is used on the screen</param>
        /// <param name="AInterest">Interest that specifies to which record the fields should
        /// be databound
        /// </param>
        /// <returns>void</returns>
        public void PerformDataBinding(PartnerEditTDS AMainDS, String AInterest)
        {
            Binding NullableNumberFormatBinding;

            // FMainDS := AMainDS;
            try
            {
                FMainDS = AMainDS;
            }
            catch (System.NullReferenceException e)
            {
                MessageBox.Show(" From TUCPartnerInterest.PerformDataBinding :" + e.StackTrace);
            }

            // create a dataview
            FPartnerInterestDV = new DataView(FMainDS.PPartnerInterest);
            FInterestDV = new DataView(FMainDS.PInterest);
            FInterestCategoryDT = (PInterestCategoryTable)TDataCache.TMPartner.GetCacheablePartnerTable(
                TCacheablePartnerTablesEnum.InterestCategoryList);
            MessageBox.Show("roiws in intesrest category" + FInterestCategoryDT.Rows.Count.ToString());

            // filter DataView to show only a certain record
            FPartnerInterestDV.RowFilter = PPartnerInterestTable.GetInterestDBName() + " = '" + AInterest + "'";

            //
            MessageBox.Show("int.perfdatb, Intersrt : " + AInterest);
            FInterestDV.RowFilter = PInterestTable.GetInterestDBName() + " = '" + AInterest + "'";
            this.txtComment.DataBindings.Add("Text", FPartnerInterestDV, PPartnerInterestTable.GetCommentDBName());

            // Data Bind a text field from the screen to an Integer field in the database
            NullableNumberFormatBinding = new Binding("Text", FPartnerInterestDV, PPartnerInterestTable.GetLevelDBName());
            NullableNumberFormatBinding.Format += new ConvertEventHandler(DataBinding.Int32ToNullableNumber);
            NullableNumberFormatBinding.Parse += new ConvertEventHandler(DataBinding.NullableNumberToInt32);
            this.txtLevel.DataBindings.Add(NullableNumberFormatBinding);

            // DataBind AutoPopulatingComboBoxes and DependentComboBoxes
            cmbInterest.PerformDataBinding(FPartnerInterestDV, PPartnerInterestTable.GetInterestDBName());
            cmbCategory.PerformDataBinding(FInterestCategoryDT, PInterestCategoryTable.GetCategoryDBName());

            // cmbCategory.Items.Add()
            InitialiseDependentComboBoxes();

            // DataBind AutoPopulatingComboBox and AutoPopulatedButtonLabel
            cmbCountryCode.PerformDataBinding(FPartnerInterestDV, PPartnerInterestTable.GetCountryDBName());
            this.TtxtAutoPopulatedButtonLabel1.PerformDataBinding(FPartnerInterestDV, PPartnerInterestTable.GetFieldKeyDBName());

            // Hook up to cmbInterestCategory SelectedValueChanged Event
            this.cmbCategory.SelectedValueChanged += new TSelectedValueChangedEventHandler(this.CmbInterestCategory_SelectedValueChanged);

            // Hook up the Event that retrieves the ShortName for the FieldKey of the
            // (TtxtAutoPopulatedButtonLabel1 TextBox
            TtxtAutoPopulatedButtonLabel1.VerifyUserEntry += new TDelegateVerifyUserEntry(this.VerifyFieldKey);

            // Extender Provider
            this.expStringLengthCheckInterest.RetrieveTextboxes(this);
            btnCreatedInterest.UpdateFields(FMainDS.PPartnerInterest);
            SetStatusBarText(this.cmbCategory, PInterestTable.GetCategoryHelp());
            SetStatusBarText(this.cmbInterest, PPartnerInterestTable.GetInterestHelp());
            SetStatusBarText(this.cmbCountryCode, PPartnerInterestTable.GetCountryHelp());
            SetStatusBarText(this.TtxtAutoPopulatedButtonLabel1, PPartnerInterestTable.GetFieldKeyHelp());
            SetStatusBarText(this.txtLevel, PPartnerInterestTable.GetLevelHelp());
            SetStatusBarText(this.txtComment, PPartnerInterestTable.GetCommentHelp());
        }

        /// <summary>
        /// If FPartnerInterestDV is empty, true is returned
        /// </summary>
        /// <returns>void</returns>
        public Boolean DataviewIsEmpty()
        {
            Boolean ReturnValue;

            // if there is no Interests true is returned, else false will be returned.
            ReturnValue = false;

            if ((FPartnerInterestDV == null) || (FPartnerInterestDV.Count < 1))
            {
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Switches between 'Read Only Mode' (TDataModeEnum.dmBrowse) and 'Edit Mode' (TDataModeEnum.dmEdit) of
        /// the UserControl.
        /// </summary>
        /// <param name="ADataMode">Specify dmBrowse for read-only mode or dmEdit for edit mode.
        /// </param>
        /// <returns>void</returns>
        public void SetMode(TDataModeEnum aDataMode)
        {
            if (aDataMode == TDataModeEnum.dmBrowse)
            {
                CustomEnablingDisabling.DisableControlGroup(grpInterests);
            }
            else
            {
                CustomEnablingDisabling.EnableControlGroup(grpInterests);
            }
        }

        /// <summary>
        /// property VerificationResultCollection: TVerificationResultCollection read FVerificationResultCollection write FVerificationResultCollection; Sets everything needed editable for a set up of new Interest
        /// </summary>
        /// <returns>void</returns>
        public void SetScreenPartsForNewInterest()
        {
            // The ScreenParts for a new Interest are set here
            this.cmbCategory.Enabled = true;
            this.cmbInterest.Enabled = true;
            this.cmbCountryCode.Enabled = true;
            this.TtxtAutoPopulatedButtonLabel1.Text = "00000000";
            this.txtLevel.Text = "00";
            this.txtComment.Text = "txtComment";
        }

        /// <summary>
        /// Revert all changes made since BeginEdit(?) was called on a DataRow. This
        /// affects the data in the DataTables to which the Controls are DataBound to
        /// and the displayed information in the DataBound Controls.
        ///
        /// Based on the two parameters the procedure also determines whether it needs
        /// to delete the affected DataRows as well.
        ///
        /// </summary>
        /// <param name="ARecordAdded">Set to true if a new was just created
        /// </param>
        /// <returns>void</returns>
        public void CancelEditing(Boolean ANewFromLocation0, Boolean ARecordAdded)
        {
            System.Windows.Forms.CurrencyManager InterestCurrencyManager;
            DataRow FInterestDR;
            Boolean DeleteRows;

            // Get CurrencyManager that is associated with the DataTables to which the
            // Controls are DataBound.
            InterestCurrencyManager = (System.Windows.Forms.CurrencyManager)BindingContext[FPartnerInterestDV];

            // Revert all changes made since BeginEdit(?) was called on a DataRow. This
            // affects the data in the DataTables to which the Controls are DataBound to
            // and the displayed information in the DataBound Controls.
            InterestCurrencyManager.CancelCurrentEdit();
            DeleteRows = false;

            // Determine whether we need to delete the affected DataRows as well
            if (FPartnerInterestDV[0].Row.RowState == DataRowState.Added)
            {
                if (!FMainDS.PPartner[0].HasVersion(DataRowVersion.Original))
                {
                    // Cancelling Editing with a New Partner
                    if (((new DataView(FMainDS.PPartnerInterest, "", "", DataViewRowState.CurrentRows).Count > 1)) && ARecordAdded)
                    {
                        DeleteRows = true;
                    }
                    else
                    {
                    }

                    // action to do...
                }
                else
                {
                    // Cancelling Editing with an existing Partner
                    if (ARecordAdded)
                    {
                        DeleteRows = true;
                    }
                }

                if (DeleteRows)
                {
                    // In addition to cancelling the Edit, we also delete the DataRows
                    FInterestDR = FPartnerInterestDV[0].Row;
                    try
                    {
                        FPartnerInterestDV.Table.Rows.Remove(FInterestDR);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Adds a filter so that only the interest for the selected category is */
        /// </summary>
        /// <returns>void</returns>
        private void InitialiseDependentComboBoxes()
        {
            PInterestRow InterestRow;

            // Get reference to cacheable Table 'InterestList'
            FInterestDT = (PInterestTable)TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.InterestList);

            if (FInterestDT.ColumnInterest.ToString() == PInterestTable.GetInterestDBName())
            {
            }
            // FInterestDT is empty
            // MessageBox.Show('Initialise = ' + FInterestDT.ToString);
            else
            {
                // Find the corresponding DataRow in the InterestList Table
                InterestRow = (PInterestRow)FInterestDT.Rows.Find(new Object[] { cmbInterest.SelectedValue.ToString() });
                cmbCategory.InitialiseUserControl();

                // does everything that PerformDataBinding does  except DataBinding
                cmbCategory.SelectedValue = InterestRow.Category;
                cmbInterest.Filter = PInterestTable.GetCategoryDBName() + " = '" + InterestRow.Category + "'";
                UpdateDependentComboBoxes();

                // MessageBox.Show('cmbInterest.SelectedValue' + cmbInterest.SelectedValue.ToString);
            }
        }

        private void UpdateDependentComboBoxes()
        {
            /// type todotimo timotodo  TEnumSet = set of 0..255;
            PInterestCategoryRow InterestCategoryRow;

            // SubrangeSet: TEnumSet;
            try
            {
                // Filter the Interest field
                cmbInterest.Filter = PInterestTable.GetCategoryDBName() + " = '" + cmbCategory.SelectedValue.ToString() + "'";
                cmbInterest.SelectedValue = "";
                InterestCategoryRow = (PInterestCategoryRow)FInterestCategoryDT.Rows.Find(new Object[] { cmbCategory.SelectedValue.ToString() });
                this.tipLevel.SetToolTip(this.txtLevel,
                    "From " + InterestCategoryRow.LevelRangeLow.ToString() + " to " + InterestCategoryRow.LevelRangeHigh.ToString());

                // Validation of Category Level
                // SubrangeSet := [InterestCategoryRow.LevelRangeLow..InterestCategoryRow.LevelRangeHigh];
                // if Convert.ToInt16(txtLevel.Text) in SubrangeSet then
                // begin
                // end
                // else
                // begin
                // MessageBox.Show('Category Level is invalid');
                // end;
            }
            catch (Exception Exp)
            {
                MessageBox.Show(Exp.ToString());
            }
        }

        private void LblCountry_Click(System.Object sender, System.EventArgs e)
        {
        }

        private void GrpInterests_Enter(System.Object sender, System.EventArgs e)
        {
        }

        private void PnlInterest_Paint(System.Object sender, System.Windows.Forms.PaintEventArgs e)
        {
        }

        #region Public Methods
        #endregion
        #region Helper Functions

        /// <summary>
        /// Verifies the Partner Key in the Field field
        /// </summary>
        /// <returns>void</returns>
        private Boolean VerifyFieldKey(String APartnerKeyString, out String APartnerShortName)
        {
            Boolean ReturnValue;

            APartnerKeyString = "";
            APartnerShortName = "";
            Int64 PartnerKeyToVerify;
            TPartnerClass PartnerClass;

            // TLogging.Log('TUC_PartnerInterest.VerifyFieldKey.APartnerKeyString: '+ APartnerKeyString, [TLoggingType.ToLogfile]);
            ReturnValue = false;
            PartnerKeyToVerify = StringHelper.StrToPartnerKey(APartnerKeyString);

            if (this.FDelegateGetPartnerShortName != null)
            {
                try
                {
                    // TLogging.Log('TUC_PartnerInterest.VerifyFieldKey. (this.FDelegateGetPartnerShortName != null)', [TLoggingType.ToLogfile]);
                    ReturnValue = this.FDelegateGetPartnerShortName(PartnerKeyToVerify, out APartnerShortName, out PartnerClass);
                }
                finally
                {
                }

                // raise EVerificationMissing.Create('this.FDelegateGetPartnerShortName could not be called!');
                if ((PartnerKeyToVerify != 0) && (PartnerClass != TPartnerClass.UNIT))
                {
                    MessageBox.Show("Partner Class must be Unit");

                    // change later
                }
            }
            else
            {
                TLogging.Log("TUC_PartnerInterest.VerifyFieldKey. NOT (this.FDelegateGetPartnerShortName != null)", TLoggingType.ToLogfile);
                ReturnValue = false;
            }

            return ReturnValue;
        }

        #endregion
    }
}