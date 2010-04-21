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
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
using Ict.Common.Controls;
using Ict.Common.Resources;
using System.Resources;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;

TODO: move statusbar things to constructor
TODO: split Designer File
namespace Ict.Petra.Client.MPartner.Gui
{
    /// Deactivate Partner Dialog.
    /// It allows the full 'deactivation' of a Partner in one go.
    public class TPartnerDeactivateDialogWinForm : TFrmPetraDialog
    {
        public const String StrVerificationErrPartnerStatus = "A 'Partner Status' must be given!";

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlExplanation;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.CheckBox chkChangePartnerStatus;
        private System.Windows.Forms.CheckBox chkCancelAllSubscriptions;
        private System.Windows.Forms.CheckBox chkExpireAllCurrentAddr;
        private System.Windows.Forms.Label Label3;
        private System.Windows.Forms.Label Label4;
        private System.Windows.Forms.Label lblExplanation2;
        private TtxtPetraDate txtValidTo;
        private TcmbAutoPopulatedComboBox cmbPartnerStatus;

        #region Windows Form Designer generated code

        /// <summary>
        /// <summary> Required method for Designer support  do not modify the contents of this method with the code editor. </summary> <summary> Required method for Designer support  do not modify the contents of this method with the code editor.
        /// </summary>
        /// </summary>
        /// <returns>void</returns>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(TPartnerDeactivateDialogWinForm));
            this.pnlExplanation = new System.Windows.Forms.Panel();
            this.Label1 = new System.Windows.Forms.Label();
            this.lblExplanation2 = new System.Windows.Forms.Label();
            this.chkChangePartnerStatus = new System.Windows.Forms.CheckBox();
            this.chkCancelAllSubscriptions = new System.Windows.Forms.CheckBox();
            this.chkExpireAllCurrentAddr = new System.Windows.Forms.CheckBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtValidTo = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.cmbPartnerStatus = new Ict.Petra.Client.CommonControls.TcmbAutoPopulatedComboBox();
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).BeginInit();
            this.pnlExplanation.SuspendLayout();
            this.SuspendLayout();

            //
            // btnOK
            //
            this.btnOK.Location = new System.Drawing.Point(352, 11);
            this.btnOK.Size = new System.Drawing.Size(90, 21);
            this.sbtForm.SetStatusBarText(this.btnOK, "Accept data and continue");
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 150);
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(548, 37);

            //
            // btnCancel
            //
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Location = new System.Drawing.Point(450, 11);
            this.btnCancel.Size = new System.Drawing.Size(90, 21);
            this.sbtForm.SetStatusBarText(this.btnCancel, "Cancel data entry and close");

            //
            // btnHelp
            //
            this.btnHelp.Location = new System.Drawing.Point(10, 11);
            this.btnHelp.Size = new System.Drawing.Size(90, 21);
            this.sbtForm.SetStatusBarText(this.btnHelp, "Help");

            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 187);
            this.stbMain.Size = new System.Drawing.Size(548, 23);

            //
            // stpInfo
            //
            this.stpInfo.Width = 548;

            //
            // pnlExplanation
            //
            this.pnlExplanation.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.pnlExplanation.BackColor = System.Drawing.Color.White;
            this.pnlExplanation.Controls.Add(this.Label1);
            this.pnlExplanation.Controls.Add(this.lblExplanation2);
            this.pnlExplanation.Location = new System.Drawing.Point(0, 0);
            this.pnlExplanation.Name = "pnlExplanation";
            this.pnlExplanation.Size = new System.Drawing.Size(548, 42);
            this.pnlExplanation.TabIndex = 998;

            //
            // Label1
            //
            this.Label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(6, 5);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(534, 14);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "This Dialog allows the full \'deactivation\' of a Partner in a single operation.";

            //
            // lblExplanation2
            //
            this.lblExplanation2.Location = new System.Drawing.Point(6, 22);
            this.lblExplanation2.Name = "lblExplanation2";
            this.lblExplanation2.Size = new System.Drawing.Size(534, 19);
            this.lblExplanation2.TabIndex = 0;
            this.lblExplanation2.Text = "Select the operation that you wish to perform for this Partner.";

            //
            // chkChangePartnerStatus
            //
            this.chkChangePartnerStatus.Checked = true;
            this.chkChangePartnerStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChangePartnerStatus.Location = new System.Drawing.Point(16, 51);
            this.chkChangePartnerStatus.Name = "chkChangePartnerStatus";
            this.chkChangePartnerStatus.Size = new System.Drawing.Size(198, 24);
            this.chkChangePartnerStatus.TabIndex = 0;
            this.chkChangePartnerStatus.Text = "Change &Partner Status";
            this.chkChangePartnerStatus.CheckedChanged += new System.EventHandler(this.ChkChangePartnerStatus_CheckedChanged);

            //
            // chkCancelAllSubscriptions
            //
            this.chkCancelAllSubscriptions.Checked = true;
            this.chkCancelAllSubscriptions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCancelAllSubscriptions.Location = new System.Drawing.Point(16, 89);
            this.chkCancelAllSubscriptions.Name = "chkCancelAllSubscriptions";
            this.chkCancelAllSubscriptions.Size = new System.Drawing.Size(198, 24);
            this.chkCancelAllSubscriptions.TabIndex = 3;
            this.chkCancelAllSubscriptions.Text = "Cancel All &Subscriptions";
            this.chkCancelAllSubscriptions.CheckedChanged += new System.EventHandler(this.ChkCancelAllSubscriptions_CheckedChanged);

            //
            // chkExpireAllCurrentAddr
            //
            this.chkExpireAllCurrentAddr.Checked = true;
            this.chkExpireAllCurrentAddr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExpireAllCurrentAddr.Location = new System.Drawing.Point(16, 127);
            this.chkExpireAllCurrentAddr.Name = "chkExpireAllCurrentAddr";
            this.chkExpireAllCurrentAddr.Size = new System.Drawing.Size(261, 24);
            this.chkExpireAllCurrentAddr.TabIndex = 4;
            this.chkExpireAllCurrentAddr.Text = "Set End Date for All Current &Addresses";
            this.chkExpireAllCurrentAddr.CheckedChanged += new System.EventHandler(this.ChkExpireAllCurrentAddr_CheckedChanged);

            //
            // Label3
            //
            this.Label3.Location = new System.Drawing.Point(228, 51);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(122, 23);
            this.Label3.TabIndex = 1;
            this.Label3.Text = "&New Partner Status:";
            this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // Label4
            //
            this.Label4.Location = new System.Drawing.Point(276, 127);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(74, 23);
            this.Label4.TabIndex = 5;
            this.Label4.Text = "&Valid To:";
            this.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // txtValidTo
            //
            this.txtValidTo.AllowEmpty = false;
            this.txtValidTo.AllowFutureDate = false;
            this.txtValidTo.AllowPastDate = true;
            this.txtValidTo.Date = new System.DateTime(2008, 3, 5, 0, 0, 0, 0);
            this.txtValidTo.Description = "Valid To";
            this.txtValidTo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtValidTo.LeavingOnFailedValidationOK = false;
            this.txtValidTo.Location = new System.Drawing.Point(352, 127);
            this.txtValidTo.Name = "txtValidTo";
            this.txtValidTo.Size = new System.Drawing.Size(94, 21);
            this.txtValidTo.TabIndex = 6;
            this.txtValidTo.Text = "05-MAR-2008";

            //
            // cmbPartnerStatus
            //
            this.cmbPartnerStatus.ComboBoxWidth = 95;
            this.cmbPartnerStatus.Filter = null;
            this.cmbPartnerStatus.ListTable = Ict.Petra.Client.CommonControls.TcmbAutoPopulatedComboBox.TListTableEnum.PartnerStatusList;
            this.cmbPartnerStatus.Location = new System.Drawing.Point(352, 51);
            this.cmbPartnerStatus.Name = "cmbPartnerStatus";
            this.cmbPartnerStatus.SelectedItem = ((object)(resources.GetObject("cmbPartnerStatus.SelectedItem")));
            this.cmbPartnerStatus.SelectedValue = null;
            this.cmbPartnerStatus.Size = new System.Drawing.Size(98, 22);
            this.cmbPartnerStatus.TabIndex = 2;

            //
            // TPartnerDeactivateDialogWinForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(548, 210);
            this.Controls.Add(this.cmbPartnerStatus);
            this.Controls.Add(this.txtValidTo);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.chkExpireAllCurrentAddr);
            this.Controls.Add(this.chkCancelAllSubscriptions);
            this.Controls.Add(this.chkChangePartnerStatus);
            this.Controls.Add(this.pnlExplanation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TPartnerDeactivateDialogWinForm";
            this.Text = "Deactivate Partner";
            this.Load += new System.EventHandler(this.TPartnerDeactivateDialogWinForm_Load);
            this.Controls.SetChildIndex(this.stbMain, 0);
            this.Controls.SetChildIndex(this.pnlBtnOKCancelHelpLayout, 0);
            this.Controls.SetChildIndex(this.pnlExplanation, 0);
            this.Controls.SetChildIndex(this.chkChangePartnerStatus, 0);
            this.Controls.SetChildIndex(this.chkCancelAllSubscriptions, 0);
            this.Controls.SetChildIndex(this.chkExpireAllCurrentAddr, 0);
            this.Controls.SetChildIndex(this.Label3, 0);
            this.Controls.SetChildIndex(this.Label4, 0);
            this.Controls.SetChildIndex(this.txtValidTo, 0);
            this.Controls.SetChildIndex(this.cmbPartnerStatus, 0);
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).EndInit();
            this.pnlExplanation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        public TPartnerDeactivateDialogWinForm() : base ()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void EnableDisableBtnOK()
        {
            if ((!chkChangePartnerStatus.Checked) && (!chkCancelAllSubscriptions.Checked) && (!chkExpireAllCurrentAddr.Checked))
            {
                btnOK.Enabled = false;
            }
            else
            {
                btnOK.Enabled = true;
            }
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

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the values of Fields
        /// on the screen.
        ///
        /// </summary>
        /// <param name="ANewPartnerStatusCode">ANewPartnerStatusCode</param>
        /// <param name="ACancelAllSubscriptions">ACancelAllSubscriptions</param>
        /// <param name="AExpireAllCurrentAddresses">AExpireAllCurrentAddresses</param>
        /// <param name="AValidTo">AValidTo</param>
        /// <returns>false if the Dialog is still uninitialised, otherwise true.
        /// </returns>
        public Boolean GetReturnedParameters(out String ANewPartnerStatusCode,
            out Boolean ACancelAllSubscriptions,
            out Boolean AExpireAllCurrentAddresses,
            out DateTime AValidTo)
        {
            ANewPartnerStatusCode = "";
            AValidTo = DateTime.Now;
            AExpireAllCurrentAddresses = false;
            ACancelAllSubscriptions = false;
            Boolean ReturnValue;

            if (FFormSetupFinished)
            {
                // Change Partner Status
                if (chkChangePartnerStatus.Checked)
                {
                    // MessageBox.Show('cmbPartnerStatus.SelectedValue: ' + cmbPartnerStatus.SelectedValue.ToString);
                    ANewPartnerStatusCode = cmbPartnerStatus.SelectedValue.ToString();
                }
                else
                {
                    ANewPartnerStatusCode = "";
                }

                // Cancel All Subscriptions
                ACancelAllSubscriptions = chkCancelAllSubscriptions.Checked;

                // Expire All Current Addresses
                AExpireAllCurrentAddresses = chkExpireAllCurrentAddr.Checked;

                if (AExpireAllCurrentAddresses)
                {
                    // MessageBox.Show('txtValidTo.Date: ' + txtValidTo.Date.ToString);
                    AValidTo = txtValidTo.Date;
                }
                else
                {
                    AValidTo = DateTime.MinValue;
                }

                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        private void ChkCancelAllSubscriptions_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            EnableDisableBtnOK();
        }

        private void ChkChangePartnerStatus_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (chkChangePartnerStatus.Checked)
            {
                cmbPartnerStatus.Enabled = true;
                EnableDisableBtnOK();
            }
            else
            {
                cmbPartnerStatus.Enabled = false;
                EnableDisableBtnOK();
            }
        }

        private void ChkExpireAllCurrentAddr_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (chkExpireAllCurrentAddr.Checked)
            {
                txtValidTo.Enabled = true;
                EnableDisableBtnOK();
            }
            else
            {
                txtValidTo.Enabled = false;
                EnableDisableBtnOK();
            }
        }

        private void TPartnerDeactivateDialogWinForm_Load(System.Object sender, System.EventArgs e)
        {
            cmbPartnerStatus.InitialiseUserControl();

            // Show all Statuses except ACTIVE, MERGED and PRIVATE (these don't make sense when deactivating a Partner...)
            cmbPartnerStatus.Filter = PPartnerTable.GetStatusCodeDBName() + " <> '" +
                                      SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscACTIVE) + "' AND " +
                                      PPartnerTable.GetStatusCodeDBName() + " <> '" +
                                      SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscMERGED) + "' AND " +
                                      PPartnerTable.GetStatusCodeDBName() + " <> '" +
                                      SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscPRIVATE) + "'";

            // Select default value for 'Partner Status' to 'INACTIVE'
            cmbPartnerStatus.SelectedValue = SharedTypes.StdPartnerStatusCodeEnumToString(TStdPartnerStatusCode.spscINACTIVE);

            // Set default value for 'Valid To' to yesterday
            txtValidTo.Date = DateTime.Today.Subtract(new TimeSpan(1, 0, 0));
            FFormSetupFinished = true;
        }

        private Boolean ValidatePartnerStatus()
        {
            Boolean ReturnValue;

            if ((chkChangePartnerStatus.Checked) && (cmbPartnerStatus.SelectedItem.ToString() == ""))
            {
                MessageBox.Show(StrVerificationErrPartnerStatus,
                    ResVerification.StrGenericInvalidData);
                ReturnValue = false;
            }
            else
            {
                ReturnValue = true;
            }

            return ReturnValue;
        }

        private void BtnOK_Click(System.Object sender, System.EventArgs e)
        {
            if (!ValidatePartnerStatus())
            {
                cmbPartnerStatus.Focus();
                return;
            }

            if (chkExpireAllCurrentAddr.Checked)
            {
                if (!txtValidTo.ValidDate(true))
                {
                    txtValidTo.Focus();
                    return;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}