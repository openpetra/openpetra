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
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Petra.Client.CommonForms;
using System.Globalization;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Formatting;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// UserControl for editing Partner Details for a Partner of Partner Class
    /// VENUE.
    ///
    /// @Comment: Info from Progress' Venue Screen (ma1000mv.w) has now been
    ///   transferred to Petra Partner Edit Screen.
    ///   Venue Code, Currency Code and Contact Partner fields transferred as they
    ///   were.
    ///   Notes field has its own tab in Petra and is therefore omitted.
    ///   Buildings grid has been left out for now.
    public class TUC_PartnerDetailsVenue : TPetraUserControl
    {
        private System.Object FSelectedAcquisitionCode;

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlPartnerDetailsVenue;
        private System.Windows.Forms.GroupBox grpNames;
        private System.Windows.Forms.Panel pnlLocalName;
        private System.Windows.Forms.Label lblPartnerShortNameLoc;
        private System.Windows.Forms.TextBox txtPartnerShortNameLoc;
        private System.Windows.Forms.Panel pnlPreferedPreviousName;
        private System.Windows.Forms.Label lblPreviousName;
        private System.Windows.Forms.TextBox txtPreviousName;
        private System.Windows.Forms.GroupBox grpMisc;
        private System.Windows.Forms.TextBox txtVenueCode;
        private System.Windows.Forms.Label lblVenueCode;
        private System.Windows.Forms.Label iblCurrencyCode;
        private System.Windows.Forms.Label lblCurrencyCode;
        private TCmbAutoPopulated cmbCurrencyCode;
        private TtxtAutoPopulatedButtonLabel txtContactPartnerBox;
        private TbtnCreated btnCreatedVenue;
        private TexpTextBoxStringLengthCheck expStringLengthCheckVenue;
        private TCmbAutoPopulated cmbLanguageCode;
        private System.Windows.Forms.Label lblLanguageCode;
        private System.Windows.Forms.Label lblAcquisitionCode;
        private TCmbAutoPopulated cmbAcquisitionCode;
        protected new PartnerEditTDS FMainDS;
        protected Boolean FPerformDataBindingDone;
        public new PartnerEditTDS MainDS
        {
            get
            {
                return FMainDS;
            }

            set
            {
                FMainDS = value;
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
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerDetailsVenue));
            this.components = new System.ComponentModel.Container();
            this.pnlPartnerDetailsVenue = new System.Windows.Forms.Panel();
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.txtContactPartnerBox = new Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel();
            this.lblAcquisitionCode = new System.Windows.Forms.Label();
            this.cmbAcquisitionCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.cmbLanguageCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblLanguageCode = new System.Windows.Forms.Label();
            this.cmbCurrencyCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.lblCurrencyCode = new System.Windows.Forms.Label();
            this.lblVenueCode = new System.Windows.Forms.Label();
            this.txtVenueCode = new System.Windows.Forms.TextBox();
            this.grpNames = new System.Windows.Forms.GroupBox();
            this.pnlLocalName = new System.Windows.Forms.Panel();
            this.lblPartnerShortNameLoc = new System.Windows.Forms.Label();
            this.txtPartnerShortNameLoc = new System.Windows.Forms.TextBox();
            this.pnlPreferedPreviousName = new System.Windows.Forms.Panel();
            this.lblPreviousName = new System.Windows.Forms.Label();
            this.txtPreviousName = new System.Windows.Forms.TextBox();
            this.btnCreatedVenue = new Ict.Common.Controls.TbtnCreated();
            this.iblCurrencyCode = new System.Windows.Forms.Label();
            this.expStringLengthCheckVenue = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.pnlPartnerDetailsVenue.SuspendLayout();
            this.grpMisc.SuspendLayout();
            this.grpNames.SuspendLayout();
            this.pnlLocalName.SuspendLayout();
            this.pnlPreferedPreviousName.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlPartnerDetailsVenue
            //
            this.pnlPartnerDetailsVenue.AutoScroll = true;
            this.pnlPartnerDetailsVenue.AutoScrollMinSize = new System.Drawing.Size(550, 330);
            this.pnlPartnerDetailsVenue.Controls.Add(this.grpMisc);
            this.pnlPartnerDetailsVenue.Controls.Add(this.grpNames);
            this.pnlPartnerDetailsVenue.Controls.Add(this.btnCreatedVenue);
            this.pnlPartnerDetailsVenue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPartnerDetailsVenue.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerDetailsVenue.Name = "pnlPartnerDetailsVenue";
            this.pnlPartnerDetailsVenue.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsVenue.TabIndex = 0;
            this.pnlPartnerDetailsVenue.Paint += new PaintEventHandler(this.PnlPartnerDetailsVenue_Paint);

            //
            // grpMisc
            //
            this.grpMisc.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMisc.Controls.Add(this.txtContactPartnerBox);
            this.grpMisc.Controls.Add(this.lblAcquisitionCode);
            this.grpMisc.Controls.Add(this.cmbAcquisitionCode);
            this.grpMisc.Controls.Add(this.cmbLanguageCode);
            this.grpMisc.Controls.Add(this.lblLanguageCode);
            this.grpMisc.Controls.Add(this.cmbCurrencyCode);
            this.grpMisc.Controls.Add(this.lblCurrencyCode);
            this.grpMisc.Controls.Add(this.lblVenueCode);
            this.grpMisc.Controls.Add(this.txtVenueCode);
            this.grpMisc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpMisc.Location = new System.Drawing.Point(4, 68);
            this.grpMisc.Name = "grpMisc";
            this.grpMisc.Size = new System.Drawing.Size(610, 140);
            this.grpMisc.TabIndex = 1;
            this.grpMisc.TabStop = false;
            this.grpMisc.Text = "Miscellaneous:";

            //
            // txtContactPartnerBox
            //
            this.txtContactPartnerBox.ASpecialSetting = true;
            this.txtContactPartnerBox.ButtonText = "Contact &Partner";
            this.txtContactPartnerBox.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtContactPartnerBox.ButtonWidth = 108;
            this.txtContactPartnerBox.ListTable = TtxtAutoPopulatedButtonLabel.TListTableEnum.PartnerKey;
            this.txtContactPartnerBox.Location = new System.Drawing.Point(20, 110);
            this.txtContactPartnerBox.MaxLength = 32767;
            this.txtContactPartnerBox.Name = "txtContactPartnerBox";
            this.txtContactPartnerBox.PartnerClass = "";
            this.txtContactPartnerBox.PreventFaultyLeaving = false;
            this.txtContactPartnerBox.ReadOnly = false;
            this.txtContactPartnerBox.Size = new System.Drawing.Size(582, 23);
            this.txtContactPartnerBox.TabIndex = 8;
            this.txtContactPartnerBox.TextBoxWidth = 80;
            this.txtContactPartnerBox.VerificationResultCollection = null;

            //
            // lblAcquisitionCode
            //
            this.lblAcquisitionCode.Location = new System.Drawing.Point(14, 86);
            this.lblAcquisitionCode.Name = "lblAcquisitionCode";
            this.lblAcquisitionCode.Size = new System.Drawing.Size(112, 22);
            this.lblAcquisitionCode.TabIndex = 6;
            this.lblAcquisitionCode.Text = "&Acquisition Code:";
            this.lblAcquisitionCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // cmbAcquisitionCode
            //
            this.cmbAcquisitionCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAcquisitionCode.ComboBoxWidth = 105;
            this.cmbAcquisitionCode.Filter = null;
            this.cmbAcquisitionCode.ListTable = TCmbAutoPopulated.TListTableEnum.AcquisitionCodeList;
            this.cmbAcquisitionCode.Location = new System.Drawing.Point(130, 87);
            this.cmbAcquisitionCode.Name = "cmbAcquisitionCode";
            this.cmbAcquisitionCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbAcquisitionCode.SelectedItem")));
            this.cmbAcquisitionCode.SelectedValue = null;
            this.cmbAcquisitionCode.Size = new System.Drawing.Size(412, 22);
            this.cmbAcquisitionCode.TabIndex = 7;
            this.cmbAcquisitionCode.Leave += new System.EventHandler(this.CmbAcquisitionCode_Leave);

            //
            // cmbLanguageCode
            //
            this.cmbLanguageCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLanguageCode.ComboBoxWidth = 57;
            this.cmbLanguageCode.Filter = null;
            this.cmbLanguageCode.ListTable = TCmbAutoPopulated.TListTableEnum.LanguageCodeList;
            this.cmbLanguageCode.Location = new System.Drawing.Point(130, 63);
            this.cmbLanguageCode.Name = "cmbLanguageCode";
            this.cmbLanguageCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbLanguageCode.SelectedItem")));
            this.cmbLanguageCode.SelectedValue = null;
            this.cmbLanguageCode.Size = new System.Drawing.Size(476, 22);
            this.cmbLanguageCode.TabIndex = 5;

            //
            // lblLanguageCode
            //
            this.lblLanguageCode.Location = new System.Drawing.Point(16, 60);
            this.lblLanguageCode.Name = "lblLanguageCode";
            this.lblLanguageCode.Size = new System.Drawing.Size(110, 24);
            this.lblLanguageCode.TabIndex = 4;
            this.lblLanguageCode.Text = "Lan&guage Code:";
            this.lblLanguageCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // cmbCurrencyCode
            //
            this.cmbCurrencyCode.ComboBoxWidth = 60;
            this.cmbCurrencyCode.Filter = null;
            this.cmbCurrencyCode.ListTable = TCmbAutoPopulated.TListTableEnum.CurrencyCodeList;
            this.cmbCurrencyCode.Location = new System.Drawing.Point(130, 38);
            this.cmbCurrencyCode.Name = "cmbCurrencyCode";
            this.cmbCurrencyCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbCurrencyCode.SelectedItem")));
            this.cmbCurrencyCode.SelectedValue = null;
            this.cmbCurrencyCode.Size = new System.Drawing.Size(378, 22);
            this.cmbCurrencyCode.TabIndex = 3;

            //
            // lblCurrencyCode
            //
            this.lblCurrencyCode.Location = new System.Drawing.Point(18, 38);
            this.lblCurrencyCode.Name = "lblCurrencyCode";
            this.lblCurrencyCode.Size = new System.Drawing.Size(108, 23);
            this.lblCurrencyCode.TabIndex = 2;
            this.lblCurrencyCode.Text = "C&urrency Code:";
            this.lblCurrencyCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCurrencyCode.Click += new System.EventHandler(this.LblCurrencyCode_Click);

            //
            // lblVenueCode
            //
            this.lblVenueCode.Location = new System.Drawing.Point(18, 16);
            this.lblVenueCode.Name = "lblVenueCode";
            this.lblVenueCode.Size = new System.Drawing.Size(108, 23);
            this.lblVenueCode.TabIndex = 0;
            this.lblVenueCode.Text = "&Venue Code:";
            this.lblVenueCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtVenueCode
            //
            this.txtVenueCode.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
            this.txtVenueCode.Location = new System.Drawing.Point(130, 16);
            this.txtVenueCode.Name = "txtVenueCode";
            this.txtVenueCode.Size = new System.Drawing.Size(107, 21);
            this.txtVenueCode.TabIndex = 1;
            this.txtVenueCode.Text = "";

            //
            // grpNames
            //
            this.grpNames.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpNames.Controls.Add(this.pnlLocalName);
            this.grpNames.Controls.Add(this.pnlPreferedPreviousName);
            this.grpNames.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpNames.Location = new System.Drawing.Point(4, 0);
            this.grpNames.Name = "grpNames";
            this.grpNames.Size = new System.Drawing.Size(610, 68);
            this.grpNames.TabIndex = 0;
            this.grpNames.TabStop = false;
            this.grpNames.Text = "Names:";

            //
            // pnlLocalName
            //
            this.pnlLocalName.Controls.Add(this.lblPartnerShortNameLoc);
            this.pnlLocalName.Controls.Add(this.txtPartnerShortNameLoc);
            this.pnlLocalName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLocalName.Location = new System.Drawing.Point(3, 41);
            this.pnlLocalName.Name = "pnlLocalName";
            this.pnlLocalName.Size = new System.Drawing.Size(604, 24);
            this.pnlLocalName.TabIndex = 1;

            //
            // lblPartnerShortNameLoc
            //
            this.lblPartnerShortNameLoc.Location = new System.Drawing.Point(16, 0);
            this.lblPartnerShortNameLoc.Name = "lblPartnerShortNameLoc";
            this.lblPartnerShortNameLoc.Size = new System.Drawing.Size(108, 23);
            this.lblPartnerShortNameLoc.TabIndex = 0;
            this.lblPartnerShortNameLoc.Text = "&Local Name:";
            this.lblPartnerShortNameLoc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPartnerShortNameLoc.Click += new System.EventHandler(this.LblLocalName_Click);

            //
            // txtPartnerShortNameLoc
            //
            this.txtPartnerShortNameLoc.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPartnerShortNameLoc.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtPartnerShortNameLoc.Location = new System.Drawing.Point(127, 0);
            this.txtPartnerShortNameLoc.Name = "txtPartnerShortNameLoc";
            this.txtPartnerShortNameLoc.Size = new System.Drawing.Size(472, 21);
            this.txtPartnerShortNameLoc.TabIndex = 1;
            this.txtPartnerShortNameLoc.Text = "";

            //
            // pnlPreferedPreviousName
            //
            this.pnlPreferedPreviousName.Controls.Add(this.lblPreviousName);
            this.pnlPreferedPreviousName.Controls.Add(this.txtPreviousName);
            this.pnlPreferedPreviousName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPreferedPreviousName.Location = new System.Drawing.Point(3, 17);
            this.pnlPreferedPreviousName.Name = "pnlPreferedPreviousName";
            this.pnlPreferedPreviousName.Size = new System.Drawing.Size(604, 24);
            this.pnlPreferedPreviousName.TabIndex = 0;

            //
            // lblPreviousName
            //
            this.lblPreviousName.Location = new System.Drawing.Point(16, 0);
            this.lblPreviousName.Name = "lblPreviousName";
            this.lblPreviousName.Size = new System.Drawing.Size(108, 22);
            this.lblPreviousName.TabIndex = 0;
            this.lblPreviousName.Text = "P&revious Name:";
            this.lblPreviousName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtPreviousName
            //
            this.txtPreviousName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPreviousName.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtPreviousName.Location = new System.Drawing.Point(127, 0);
            this.txtPreviousName.Name = "txtPreviousName";
            this.txtPreviousName.Size = new System.Drawing.Size(472, 21);
            this.txtPreviousName.TabIndex = 1;
            this.txtPreviousName.Text = "";

            //
            // btnCreatedVenue
            //
            this.btnCreatedVenue.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnCreatedVenue.CreatedBy = null;
            this.btnCreatedVenue.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedVenue.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedVenue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedVenue.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnCreatedVenue.Image")));
            this.btnCreatedVenue.Location = new System.Drawing.Point(616, 2);
            this.btnCreatedVenue.ModifiedBy = null;
            this.btnCreatedVenue.Name = "btnCreatedVenue";
            this.btnCreatedVenue.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedVenue.TabIndex = 2;
            this.btnCreatedVenue.Tag = "dontdisable";

            //
            // iblCurrencyCode
            //
            this.iblCurrencyCode.Location = new System.Drawing.Point(16, 2);
            this.iblCurrencyCode.Name = "iblCurrencyCode";
            this.iblCurrencyCode.Size = new System.Drawing.Size(108, 22);
            this.iblCurrencyCode.TabIndex = 1;
            this.iblCurrencyCode.Text = "C&urrency Code:";
            this.iblCurrencyCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // TUC_PartnerDetailsVenue
            //
            this.Controls.Add(this.pnlPartnerDetailsVenue);
            this.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.Name = "TUC_PartnerDetailsVenue";
            this.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsVenue.ResumeLayout(false);
            this.grpMisc.ResumeLayout(false);
            this.grpNames.ResumeLayout(false);
            this.pnlLocalName.ResumeLayout(false);
            this.pnlPreferedPreviousName.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        public TUC_PartnerDetailsVenue() : base()
        {
            FPerformDataBindingDone = false;

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void PnlPartnerDetailsVenue_Paint(System.Object sender, System.Windows.Forms.PaintEventArgs e)
        {
        }

        private void LblCurrencyCode_Click(System.Object sender, System.EventArgs e)
        {
        }

        private void LblLocalName_Click(System.Object sender, System.EventArgs e)
        {
        }

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

        public new void InitialiseUserControl()
        {
            this.txtPartnerShortNameLoc.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPartnerShortNameLocDBName());
            this.txtPreviousName.DataBindings.Add("Text", FMainDS.PPartner, PPartnerTable.GetPreviousNameDBName());
            this.txtVenueCode.DataBindings.Add("Text", FMainDS.PVenue, PVenueTable.GetVenueCodeDBName());

            // DataBind AutoPopulatingComboBox and AutoPopulatedButtonLabel
            cmbCurrencyCode.PerformDataBinding(FMainDS.PVenue, PVenueTable.GetCurrencyCodeDBName());
            txtContactPartnerBox.PerformDataBinding(FMainDS.PVenue.DefaultView, PVenueTable.GetContactPartnerKeyDBName());
            this.cmbLanguageCode.PerformDataBinding(FMainDS.PPartner, PPartnerTable.GetLanguageCodeDBName());
            this.cmbAcquisitionCode.PerformDataBinding(FMainDS, PPartnerTable.GetTableName() + '.' + PPartnerTable.GetAcquisitionCodeDBName());
            btnCreatedVenue.UpdateFields(FMainDS.PVenue);

            // Extender Provider
            this.expStringLengthCheckVenue.RetrieveTextboxes(this);
            SetStatusBarText(txtContactPartnerBox, PVenueTable.GetContactPartnerKeyHelp());
            SetStatusBarText(cmbCurrencyCode, PVenueTable.GetCurrencyCodeHelp());
            SetStatusBarText(txtPartnerShortNameLoc, PPartnerTable.GetPartnerShortNameHelp());
            SetStatusBarText(txtPreviousName, PPartnerTable.GetPreviousNameHelp());
            SetStatusBarText(txtVenueCode, PVenueTable.GetVenueCodeHelp());
            SetStatusBarText(cmbAcquisitionCode, PPartnerTable.GetAcquisitionCodeHelp());
            SetStatusBarText(cmbLanguageCode, PPartnerTable.GetLanguageCodeHelp());
            #region Verification
            txtContactPartnerBox.VerificationResultCollection = FVerificationResultCollection;
            #endregion
        }

        #region Event Handlers

        /// <summary>
        /// checks that the Acquisition Code is valid.
        /// </summary>
        /// <returns>void</returns>
        private void CmbAcquisitionCode_Leave(System.Object sender, System.EventArgs e)
        {
            DataTable DataCacheAcquisitionCodeTable;
            PAcquisitionRow TmpRow;
            DialogResult UseAlthoughUnassignable;

            try
            {
                // check if the publication selected is valid, if not, gives warning.
                DataCacheAcquisitionCodeTable = TDataCache.TMPartner.GetCacheablePartnerTable(TCacheablePartnerTablesEnum.AcquisitionCodeList);
                TmpRow = (PAcquisitionRow)DataCacheAcquisitionCodeTable.Rows.Find(new Object[] { this.cmbAcquisitionCode.SelectedItem.ToString() });

                if (TmpRow != null)
                {
                    if (TmpRow.ValidAcquisition)
                    {
                        FSelectedAcquisitionCode = cmbAcquisitionCode.SelectedItem;
                    }
                    else
                    {
                        UseAlthoughUnassignable = MessageBox.Show(CommonResourcestrings.StrErrorTheCodeIsNoLongerActive1 + " '" +
                            this.cmbAcquisitionCode.SelectedItem.ToString() + "' " +
                            CommonResourcestrings.StrErrorTheCodeIsNoLongerActive2 + "\r\n" +
                            CommonResourcestrings.StrErrorTheCodeIsNoLongerActive3 + "\r\n" +
                            "\r\n" + "Message Number: " + ErrorCodes.PETRAERRORCODE_VALUEUNASSIGNABLE +
                            "\r\n" + "File Name: " + this.GetType().FullName, "Invalid Data Entered",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);

                        if (UseAlthoughUnassignable == System.Windows.Forms.DialogResult.No)
                        {
                            // If user selects not to use the publication, the recent publication code is selected.
                            this.cmbAcquisitionCode.SelectedItem = FSelectedAcquisitionCode;
                        }
                        else
                        {
                            FSelectedAcquisitionCode = cmbAcquisitionCode.SelectedItem;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}