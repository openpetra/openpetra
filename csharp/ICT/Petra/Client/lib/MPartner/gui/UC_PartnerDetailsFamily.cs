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
using Ict.Petra.Client.CommonForms;
using Ict.Common.Controls;
using System.Globalization;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Client.CommonControls;
using Ict.Common;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// UserControl for editing Partner Details for a Partner of Partner Class FAMILY.
    public class TUC_PartnerDetailsFamily : TPetraUserControl
    {
        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlPartnerDetailsPerson;
        private System.Windows.Forms.GroupBox grpNames;
        private System.Windows.Forms.GroupBox grpMisc;
        private TbtnCreated btnCreatedFamily;
        private System.Windows.Forms.Panel pnlLocalName;
        private System.Windows.Forms.Label lblLocalName;
        private System.Windows.Forms.TextBox txtLocalName;
        private System.Windows.Forms.Panel pnlPreferedPreviousName;
        private System.Windows.Forms.Label lblPreviousName;
        private System.Windows.Forms.TextBox txtPreviousName;
        private System.Windows.Forms.Panel pnlAcquisition;
        private System.Windows.Forms.Label lblAcquisitionCode;
        private TCmbAutoPopulated cmbAcquisitionCode;
        private System.Windows.Forms.Panel pnlLanguageCode;
        private System.Windows.Forms.Label lblLanguageCode;
        private TCmbAutoPopulated cmbLanguageCode;
        private System.Windows.Forms.Panel pnlMaritalSince;
        private System.Windows.Forms.Label lblMaritalStatusSince;
        private System.Windows.Forms.TextBox txtMaritalStatusSince;
        private System.Windows.Forms.Panel pnlMarital;
        private System.Windows.Forms.Label lblMaritalStatus;
        private TCmbAutoPopulated cmbMaritalStatus;
        private TexpTextBoxStringLengthCheck expStringLengthCheckFamily;
        private System.Windows.Forms.Label lblMaritalStatusComment;
        private System.Windows.Forms.TextBox txtMaritalStatusComment;

        /// <summary> Clean up any resources being used. </summary>
        protected new PartnerEditTDS FMainDS;
        private System.Object FSelectedAcquisitionCode;
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
                new System.ComponentModel.ComponentResourceManager(typeof(TUC_PartnerDetailsFamily));
            this.components = new System.ComponentModel.Container();
            this.pnlPartnerDetailsPerson = new System.Windows.Forms.Panel();
            this.grpMisc = new System.Windows.Forms.GroupBox();
            this.pnlAcquisition = new System.Windows.Forms.Panel();
            this.lblAcquisitionCode = new System.Windows.Forms.Label();
            this.cmbAcquisitionCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlLanguageCode = new System.Windows.Forms.Panel();
            this.lblLanguageCode = new System.Windows.Forms.Label();
            this.cmbLanguageCode = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.pnlMaritalSince = new System.Windows.Forms.Panel();
            this.txtMaritalStatusComment = new System.Windows.Forms.TextBox();
            this.lblMaritalStatusComment = new System.Windows.Forms.Label();
            this.lblMaritalStatusSince = new System.Windows.Forms.Label();
            this.txtMaritalStatusSince = new System.Windows.Forms.TextBox();
            this.pnlMarital = new System.Windows.Forms.Panel();
            this.lblMaritalStatus = new System.Windows.Forms.Label();
            this.cmbMaritalStatus = new Ict.Petra.Client.CommonControls.TCmbAutoPopulated();
            this.grpNames = new System.Windows.Forms.GroupBox();
            this.pnlLocalName = new System.Windows.Forms.Panel();
            this.lblLocalName = new System.Windows.Forms.Label();
            this.txtLocalName = new System.Windows.Forms.TextBox();
            this.pnlPreferedPreviousName = new System.Windows.Forms.Panel();
            this.lblPreviousName = new System.Windows.Forms.Label();
            this.txtPreviousName = new System.Windows.Forms.TextBox();
            this.btnCreatedFamily = new Ict.Common.Controls.TbtnCreated();
            this.expStringLengthCheckFamily = new Ict.Petra.Client.CommonControls.TexpTextBoxStringLengthCheck(this.components);
            this.pnlPartnerDetailsPerson.SuspendLayout();
            this.grpMisc.SuspendLayout();
            this.pnlAcquisition.SuspendLayout();
            this.pnlLanguageCode.SuspendLayout();
            this.pnlMaritalSince.SuspendLayout();
            this.pnlMarital.SuspendLayout();
            this.grpNames.SuspendLayout();
            this.pnlLocalName.SuspendLayout();
            this.pnlPreferedPreviousName.SuspendLayout();
            this.SuspendLayout();

            //
            // pnlPartnerDetailsPerson
            //
            this.pnlPartnerDetailsPerson.AutoScroll = true;
            this.pnlPartnerDetailsPerson.AutoScrollMinSize = new System.Drawing.Size(550, 330);
            this.pnlPartnerDetailsPerson.Controls.Add(this.grpMisc);
            this.pnlPartnerDetailsPerson.Controls.Add(this.grpNames);
            this.pnlPartnerDetailsPerson.Controls.Add(this.btnCreatedFamily);
            this.pnlPartnerDetailsPerson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPartnerDetailsPerson.Location = new System.Drawing.Point(0, 0);
            this.pnlPartnerDetailsPerson.Name = "pnlPartnerDetailsPerson";
            this.pnlPartnerDetailsPerson.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsPerson.TabIndex = 0;

            //
            // grpMisc
            //
            this.grpMisc.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMisc.Controls.Add(this.pnlAcquisition);
            this.grpMisc.Controls.Add(this.pnlLanguageCode);
            this.grpMisc.Controls.Add(this.pnlMaritalSince);
            this.grpMisc.Controls.Add(this.pnlMarital);
            this.grpMisc.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpMisc.Location = new System.Drawing.Point(4, 68);
            this.grpMisc.Name = "grpMisc";
            this.grpMisc.Size = new System.Drawing.Size(610, 164);
            this.grpMisc.TabIndex = 2;
            this.grpMisc.TabStop = false;
            this.grpMisc.Text = "Miscellaneous:";

            //
            // pnlAcquisition
            //
            this.pnlAcquisition.BackColor = System.Drawing.SystemColors.Control;
            this.pnlAcquisition.Controls.Add(this.lblAcquisitionCode);
            this.pnlAcquisition.Controls.Add(this.cmbAcquisitionCode);
            this.pnlAcquisition.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAcquisition.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.pnlAcquisition.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlAcquisition.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlAcquisition.Location = new System.Drawing.Point(3, 136);
            this.pnlAcquisition.Name = "pnlAcquisition";
            this.pnlAcquisition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlAcquisition.Size = new System.Drawing.Size(604, 24);
            this.pnlAcquisition.TabIndex = 3;

            //
            // lblAcquisitionCode
            //
            this.lblAcquisitionCode.Location = new System.Drawing.Point(17, 0);
            this.lblAcquisitionCode.Name = "lblAcquisitionCode";
            this.lblAcquisitionCode.Size = new System.Drawing.Size(108, 22);
            this.lblAcquisitionCode.TabIndex = 12;
            this.lblAcquisitionCode.Text = "&Acquisition Code:";
            this.lblAcquisitionCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // cmbAcquisitionCode
            //
            this.cmbAcquisitionCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAcquisitionCode.ComboBoxWidth = 95;
            this.cmbAcquisitionCode.Filter = null;
            this.cmbAcquisitionCode.ListTable = TCmbAutoPopulated.TListTableEnum.AcquisitionCodeList;
            this.cmbAcquisitionCode.Location = new System.Drawing.Point(127, 0);
            this.cmbAcquisitionCode.Name = "cmbAcquisitionCode";
            this.cmbAcquisitionCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbAcquisitionCode.SelectedItem")));
            this.cmbAcquisitionCode.SelectedValue = null;
            this.cmbAcquisitionCode.Size = new System.Drawing.Size(412, 22);
            this.cmbAcquisitionCode.TabIndex = 9;
            this.cmbAcquisitionCode.Leave += new System.EventHandler(this.CmbAcquisitionCode_Leave);

            //
            // pnlLanguageCode
            //
            this.pnlLanguageCode.BackColor = System.Drawing.SystemColors.Control;
            this.pnlLanguageCode.Controls.Add(this.lblLanguageCode);
            this.pnlLanguageCode.Controls.Add(this.cmbLanguageCode);
            this.pnlLanguageCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLanguageCode.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.pnlLanguageCode.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlLanguageCode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlLanguageCode.Location = new System.Drawing.Point(3, 112);
            this.pnlLanguageCode.Name = "pnlLanguageCode";
            this.pnlLanguageCode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlLanguageCode.Size = new System.Drawing.Size(604, 24);
            this.pnlLanguageCode.TabIndex = 2;

            //
            // lblLanguageCode
            //
            this.lblLanguageCode.Location = new System.Drawing.Point(17, 0);
            this.lblLanguageCode.Name = "lblLanguageCode";
            this.lblLanguageCode.Size = new System.Drawing.Size(108, 22);
            this.lblLanguageCode.TabIndex = 15;
            this.lblLanguageCode.Text = "Lang&uage Code:";
            this.lblLanguageCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // cmbLanguageCode
            //
            this.cmbLanguageCode.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLanguageCode.ComboBoxWidth = 57;
            this.cmbLanguageCode.Filter = null;
            this.cmbLanguageCode.ListTable = TCmbAutoPopulated.TListTableEnum.LanguageCodeList;
            this.cmbLanguageCode.Location = new System.Drawing.Point(127, 0);
            this.cmbLanguageCode.Name = "cmbLanguageCode";
            this.cmbLanguageCode.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbLanguageCode.SelectedItem")));
            this.cmbLanguageCode.SelectedValue = null;
            this.cmbLanguageCode.Size = new System.Drawing.Size(468, 22);
            this.cmbLanguageCode.TabIndex = 8;

            //
            // pnlMaritalSince
            //
            this.pnlMaritalSince.Controls.Add(this.txtMaritalStatusComment);
            this.pnlMaritalSince.Controls.Add(this.lblMaritalStatusComment);
            this.pnlMaritalSince.Controls.Add(this.lblMaritalStatusSince);
            this.pnlMaritalSince.Controls.Add(this.txtMaritalStatusSince);
            this.pnlMaritalSince.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMaritalSince.Location = new System.Drawing.Point(3, 40);
            this.pnlMaritalSince.Name = "pnlMaritalSince";
            this.pnlMaritalSince.Size = new System.Drawing.Size(604, 72);
            this.pnlMaritalSince.TabIndex = 1;

            //
            // txtMaritalStatusComment
            //
            this.txtMaritalStatusComment.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMaritalStatusComment.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtMaritalStatusComment.Location = new System.Drawing.Point(127, 24);
            this.txtMaritalStatusComment.Multiline = true;
            this.txtMaritalStatusComment.Name = "txtMaritalStatusComment";
            this.txtMaritalStatusComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMaritalStatusComment.Size = new System.Drawing.Size(472, 44);
            this.txtMaritalStatusComment.TabIndex = 7;
            this.txtMaritalStatusComment.Text = "";

            //
            // lblMaritalStatusComment
            //
            this.lblMaritalStatusComment.Font = new System.Drawing.Font("Verdana", 8.25f);
            this.lblMaritalStatusComment.Location = new System.Drawing.Point(1, 28);
            this.lblMaritalStatusComment.Name = "lblMaritalStatusComment";
            this.lblMaritalStatusComment.Size = new System.Drawing.Size(124, 38);
            this.lblMaritalStatusComment.TabIndex = 17;
            this.lblMaritalStatusComment.Text = "Marital Status Comment:";
            this.lblMaritalStatusComment.TextAlign = System.Drawing.ContentAlignment.TopRight;

            //
            // lblMaritalStatusSince
            //
            this.lblMaritalStatusSince.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.lblMaritalStatusSince.Location = new System.Drawing.Point(1, 0);
            this.lblMaritalStatusSince.Name = "lblMaritalStatusSince";
            this.lblMaritalStatusSince.Size = new System.Drawing.Size(124, 22);
            this.lblMaritalStatusSince.TabIndex = 13;
            this.lblMaritalStatusSince.Text = "M&arital Status Since:";
            this.lblMaritalStatusSince.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtMaritalStatusSince
            //
            this.txtMaritalStatusSince.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.txtMaritalStatusSince.Location = new System.Drawing.Point(127, 0);
            this.txtMaritalStatusSince.Name = "txtMaritalStatusSince";
            this.txtMaritalStatusSince.Size = new System.Drawing.Size(94, 21);
            this.txtMaritalStatusSince.TabIndex = 5;
            this.txtMaritalStatusSince.Text = "";

            //
            // pnlMarital
            //
            this.pnlMarital.Controls.Add(this.lblMaritalStatus);
            this.pnlMarital.Controls.Add(this.cmbMaritalStatus);
            this.pnlMarital.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMarital.Location = new System.Drawing.Point(3, 16);
            this.pnlMarital.Name = "pnlMarital";
            this.pnlMarital.Size = new System.Drawing.Size(604, 24);
            this.pnlMarital.TabIndex = 0;

            //
            // lblMaritalStatus
            //
            this.lblMaritalStatus.Font = new System.Drawing.Font("Verdana",
                8.25f,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                (byte)0);
            this.lblMaritalStatus.Location = new System.Drawing.Point(17, 0);
            this.lblMaritalStatus.Name = "lblMaritalStatus";
            this.lblMaritalStatus.Size = new System.Drawing.Size(108, 22);
            this.lblMaritalStatus.TabIndex = 12;
            this.lblMaritalStatus.Text = "&Marital Status:";
            this.lblMaritalStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // cmbMaritalStatus
            //
            this.cmbMaritalStatus.ComboBoxWidth = 39;
            this.cmbMaritalStatus.Filter = null;
            this.cmbMaritalStatus.ListTable = TCmbAutoPopulated.TListTableEnum.MaritalStatusList;
            this.cmbMaritalStatus.Location = new System.Drawing.Point(127, 0);
            this.cmbMaritalStatus.Name = "cmbMaritalStatus";
            this.cmbMaritalStatus.SelectedItem = ((System.Object)(resources.GetObject('c' + "mbMaritalStatus.SelectedItem")));
            this.cmbMaritalStatus.SelectedValue = null;
            this.cmbMaritalStatus.Size = new System.Drawing.Size(230, 22);
            this.cmbMaritalStatus.TabIndex = 3;

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
            this.grpNames.TabIndex = 1;
            this.grpNames.TabStop = false;
            this.grpNames.Text = "Names:";

            //
            // pnlLocalName
            //
            this.pnlLocalName.Controls.Add(this.lblLocalName);
            this.pnlLocalName.Controls.Add(this.txtLocalName);
            this.pnlLocalName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLocalName.Location = new System.Drawing.Point(3, 40);
            this.pnlLocalName.Name = "pnlLocalName";
            this.pnlLocalName.Size = new System.Drawing.Size(604, 24);
            this.pnlLocalName.TabIndex = 7;

            //
            // lblLocalName
            //
            this.lblLocalName.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.lblLocalName.Location = new System.Drawing.Point(17, 0);
            this.lblLocalName.Name = "lblLocalName";
            this.lblLocalName.Size = new System.Drawing.Size(108, 23);
            this.lblLocalName.TabIndex = 11;
            this.lblLocalName.Text = "&Local Name:";
            this.lblLocalName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtLocalName
            //
            this.txtLocalName.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top |
                                                       System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocalName.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.txtLocalName.Location = new System.Drawing.Point(127, 0);
            this.txtLocalName.Name = "txtLocalName";
            this.txtLocalName.Size = new System.Drawing.Size(472, 21);
            this.txtLocalName.TabIndex = 2;
            this.txtLocalName.Text = "";

            //
            // pnlPreferedPreviousName
            //
            this.pnlPreferedPreviousName.Controls.Add(this.lblPreviousName);
            this.pnlPreferedPreviousName.Controls.Add(this.txtPreviousName);
            this.pnlPreferedPreviousName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPreferedPreviousName.Location = new System.Drawing.Point(3, 16);
            this.pnlPreferedPreviousName.Name = "pnlPreferedPreviousName";
            this.pnlPreferedPreviousName.Size = new System.Drawing.Size(604, 24);
            this.pnlPreferedPreviousName.TabIndex = 0;

            //
            // lblPreviousName
            //
            this.lblPreviousName.Font =
                new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
            this.lblPreviousName.Location = new System.Drawing.Point(17, 0);
            this.lblPreviousName.Name = "lblPreviousName";
            this.lblPreviousName.Size = new System.Drawing.Size(108, 22);
            this.lblPreviousName.TabIndex = 10;
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
            // btnCreatedFamily
            //
            this.btnCreatedFamily.Anchor =
                ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            this.btnCreatedFamily.CreatedBy = null;
            this.btnCreatedFamily.DateCreated = new System.DateTime((System.Int64) 0);
            this.btnCreatedFamily.DateModified = new System.DateTime((System.Int64) 0);
            this.btnCreatedFamily.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreatedFamily.Image = ((System.Drawing.Image)(resources.GetObject('b' + "tnCreatedFamily.Image")));
            this.btnCreatedFamily.Location = new System.Drawing.Point(616, 2);
            this.btnCreatedFamily.ModifiedBy = null;
            this.btnCreatedFamily.Name = "btnCreatedFamily";
            this.btnCreatedFamily.Size = new System.Drawing.Size(14, 16);
            this.btnCreatedFamily.TabIndex = 3;
            this.btnCreatedFamily.Tag = "dontdisable";

            //
            // TUC_PartnerDetailsFamily
            //
            this.Controls.Add(this.pnlPartnerDetailsPerson);
            this.Name = "TUC_PartnerDetailsFamily";
            this.Size = new System.Drawing.Size(634, 330);
            this.pnlPartnerDetailsPerson.ResumeLayout(false);
            this.grpMisc.ResumeLayout(false);
            this.pnlAcquisition.ResumeLayout(false);
            this.pnlLanguageCode.ResumeLayout(false);
            this.pnlMaritalSince.ResumeLayout(false);
            this.pnlMarital.ResumeLayout(false);
            this.grpNames.ResumeLayout(false);
            this.pnlLocalName.ResumeLayout(false);
            this.pnlPreferedPreviousName.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        public TUC_PartnerDetailsFamily() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // I18N: assign proper font which helps to read asian characters
            txtPreviousName.Font = TAppSettingsManager.GetDefaultBoldFont();
            txtLocalName.Font = TAppSettingsManager.GetDefaultBoldFont();
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

        #region Public methods
        public new void InitialiseUserControl()
        {
            Binding DateFormatBinding;

            // Names
            txtPreviousName.DataBindings.Add("Text", FMainDS, PPartnerTable.GetTableName() + '.' + PPartnerTable.GetPreviousNameDBName());
            txtLocalName.DataBindings.Add("Text", FMainDS, PPartnerTable.GetTableName() + '.' + PPartnerTable.GetPartnerShortNameLocDBName());

            // Miscellaneous
            DateFormatBinding = new Binding("Text", FMainDS.PFamily, PFamilyTable.GetMaritalStatusSinceDBName());
            DateFormatBinding.Format += new ConvertEventHandler(DataBinding.DateTimeToLongDateString);
            DateFormatBinding.Parse += new ConvertEventHandler(DataBinding.LongDateStringToDateTime);
            txtMaritalStatusSince.DataBindings.Add(DateFormatBinding);
            this.txtMaritalStatusComment.DataBindings.Add("Text", FMainDS,
                PFamilyTable.GetTableName() + '.' + PFamilyTable.GetMaritalStatusCommentDBName());

            // DataBind AutoPopulatingComboBoxes
            cmbAcquisitionCode.PerformDataBinding(FMainDS, PPartnerTable.GetTableName() + '.' + PPartnerTable.GetAcquisitionCodeDBName());
            cmbMaritalStatus.PerformDataBinding(FMainDS, PFamilyTable.GetTableName() + '.' + PFamilyTable.GetMaritalStatusDBName());
            cmbLanguageCode.PerformDataBinding(FMainDS, PPartnerTable.GetTableName() + '.' + PPartnerTable.GetLanguageCodeDBName());
            btnCreatedFamily.UpdateFields(FMainDS.PFamily);

            // Extender Provider
            this.expStringLengthCheckFamily.RetrieveTextboxes(this);

            // Set StatusBar Texts
            SetStatusBarText(txtPreviousName, PPartnerTable.GetPreviousNameHelp());
            SetStatusBarText(txtLocalName, PPartnerTable.GetPartnerShortNameLocHelp());
            SetStatusBarText(txtMaritalStatusSince, PFamilyTable.GetMaritalStatusSinceHelp());
            SetStatusBarText(cmbLanguageCode, PPartnerTable.GetLanguageCodeHelp());
            SetStatusBarText(cmbAcquisitionCode, PPartnerTable.GetAcquisitionCodeHelp());
            SetStatusBarText(cmbMaritalStatus, PFamilyTable.GetMaritalStatusHelp());
            ApplySecurity();
        }

        #endregion

        #region Helper functions
        private void ApplySecurity()
        {
            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PPartnerTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_partner
                // MessageBox.Show('Disabling p_partner fields...');
                CustomEnablingDisabling.DisableControl(pnlPreferedPreviousName, txtPreviousName);
                CustomEnablingDisabling.DisableControl(pnlLocalName, txtLocalName);
                CustomEnablingDisabling.DisableControl(pnlAcquisition, cmbAcquisitionCode);
                CustomEnablingDisabling.DisableControl(pnlAcquisition, cmbLanguageCode);
            }

            if (!UserInfo.GUserInfo.IsTableAccessOK(TTableAccessPermission.tapMODIFY, PFamilyTable.GetTableDBName()))
            {
                // need to disable all Fields that are DataBound to p_family
                CustomEnablingDisabling.DisableControlGroup(pnlMarital);
                CustomEnablingDisabling.DisableControlGroup(pnlMaritalSince);
            }
        }

        #endregion

        #region Event Handlers
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
                        UseAlthoughUnassignable = MessageBox.Show(
                            CommonResourcestrings.StrErrorTheCodeIsNoLongerActive1 + " '" +
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