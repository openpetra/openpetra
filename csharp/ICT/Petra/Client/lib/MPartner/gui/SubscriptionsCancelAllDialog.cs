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
using Ict.Common.Verification;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Client.App.Formatting;
using Ict.Petra.Client.CommonControls;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Common.Resources;

namespace Ict.Petra.Client.MPartner
{
    /// Cancel All Subscriptions Dialog. Called from Subscriptions Tab on Partner
    /// Edit screen and from 'Deactivate Partner' Dialog.
    public class TPartnerSubscCancelAllWinForm : TFrmPetraDialog
    {
        public const String StrVerificationErrReasonEnded = "A 'Reason Ended' must be given!";

        /// <summary> Required designer variable. </summary>
        private System.ComponentModel.IContainer components = null;
        private TcmbAutoPopulatedComboBox cmbReasonEnded;
        private System.Windows.Forms.Label Label8;
        private System.Windows.Forms.Label Label23;
        private System.Windows.Forms.Panel pnlExplanation;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label Label2;
        private TtxtPetraDate txtDateEnded;
        private DateTime FDateEndedPreset;

        /// <summary>
        /// Set this Property to a Date to preset the 'Date Ended' date in the Dialog to
        /// this date (instead of to today, to which it defaults).
        ///
        /// </summary>
        public DateTime DateEnded
        {
            get
            {
                return FDateEndedPreset;
            }

            set
            {
                FDateEndedPreset = value;
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
                new System.ComponentModel.ComponentResourceManager(typeof(TPartnerSubscCancelAllWinForm));
            this.cmbReasonEnded = new Ict.Petra.Client.CommonControls.TcmbAutoPopulatedComboBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.Label23 = new System.Windows.Forms.Label();
            this.pnlExplanation = new System.Windows.Forms.Panel();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtDateEnded = new Ict.Petra.Client.CommonControls.TtxtPetraDate();
            this.pnlBtnOKCancelHelpLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).BeginInit();
            this.pnlExplanation.SuspendLayout();
            this.SuspendLayout();

            //
            // btnOK
            //
            this.btnOK.Location = new System.Drawing.Point(376, 9);
            this.btnOK.Size = new System.Drawing.Size(90, 21);
            this.sbtForm.SetStatusBarText(this.btnOK, "Accept data and continue");
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);

            //
            // pnlBtnOKCancelHelpLayout
            //
            this.pnlBtnOKCancelHelpLayout.Location = new System.Drawing.Point(0, 118);
            this.pnlBtnOKCancelHelpLayout.Size = new System.Drawing.Size(572, 35);

            //
            // btnCancel
            //
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Location = new System.Drawing.Point(474, 9);
            this.btnCancel.Size = new System.Drawing.Size(90, 21);
            this.sbtForm.SetStatusBarText(this.btnCancel, "Cancel data entry and close");

            //
            // btnHelp
            //
            this.btnHelp.CausesValidation = false;
            this.btnHelp.Location = new System.Drawing.Point(8, 9);
            this.btnHelp.Size = new System.Drawing.Size(90, 21);
            this.sbtForm.SetStatusBarText(this.btnHelp, "Help");

            //
            // stbMain
            //
            this.stbMain.Location = new System.Drawing.Point(0, 153);
            this.stbMain.Size = new System.Drawing.Size(572, 23);

            //
            // stpInfo
            //
            this.stpInfo.Width = 572;

            //
            // cmbReasonEnded
            //
            this.cmbReasonEnded.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.cmbReasonEnded.ComboBoxWidth = 110;
            this.cmbReasonEnded.Filter = null;
            this.cmbReasonEnded.ListTable = Ict.Petra.Client.CommonControls.TcmbAutoPopulatedComboBox.TListTableEnum.ReasonSubscriptionCancelledList;
            this.cmbReasonEnded.Location = new System.Drawing.Point(114, 66);
            this.cmbReasonEnded.Name = "cmbReasonEnded";
            this.cmbReasonEnded.SelectedItem = ((object)(resources.GetObject("cmbReasonEnded.SelectedItem")));
            this.cmbReasonEnded.SelectedValue = null;
            this.cmbReasonEnded.Size = new System.Drawing.Size(448, 22);
            this.cmbReasonEnded.TabIndex = 1;
            this.cmbReasonEnded.Validating += new System.ComponentModel.CancelEventHandler(this.CmbReasonEnded_Validating);

            //
            // Label8
            //
            this.Label8.Location = new System.Drawing.Point(16, 66);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(92, 20);
            this.Label8.TabIndex = 0;
            this.Label8.Text = "&Reason Ended:";
            this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // Label23
            //
            this.Label23.Location = new System.Drawing.Point(16, 90);
            this.Label23.Name = "Label23";
            this.Label23.Size = new System.Drawing.Size(92, 20);
            this.Label23.TabIndex = 2;
            this.Label23.Text = "Date E&nded:";
            this.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            //
            // pnlExplanation
            //
            this.pnlExplanation.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.pnlExplanation.BackColor = System.Drawing.Color.White;
            this.pnlExplanation.Controls.Add(this.Label1);
            this.pnlExplanation.Controls.Add(this.Label2);
            this.pnlExplanation.Location = new System.Drawing.Point(0, 0);
            this.pnlExplanation.Name = "pnlExplanation";
            this.pnlExplanation.Size = new System.Drawing.Size(624, 54);
            this.pnlExplanation.TabIndex = 997;

            //
            // Label1
            //
            this.Label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(6, 5);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(558, 14);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "This Dialog allows cancelling of all active Subscriptions in a single operation.";

            //
            // Label2
            //
            this.Label2.Location = new System.Drawing.Point(6, 22);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(560, 26);
            this.Label2.TabIndex = 0;
            this.Label2.Text = "Select the \'Reason Ended\' and enter the \'Date Ended\'. On clicking OK, these will " +
                               "be applied to all active Subscriptions. The Partner will be left with no active " +
                               "Subscriptions!";

            //
            // txtDateEnded
            //
            this.txtDateEnded.AllowEmpty = false;
            this.txtDateEnded.AllowFutureDate = true;
            this.txtDateEnded.AllowPastDate = true;
            this.txtDateEnded.Description = "Date Ended";
            this.txtDateEnded.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtDateEnded.LeavingOnFailedValidationOK = false;
            this.txtDateEnded.Location = new System.Drawing.Point(114, 90);
            this.txtDateEnded.Name = "txtDateEnded";
            this.txtDateEnded.Size = new System.Drawing.Size(94, 21);
            this.txtDateEnded.TabIndex = 3;

            //
            // TPartnerSubscCancelAllWinForm
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(572, 176);
            this.Controls.Add(this.txtDateEnded);
            this.Controls.Add(this.pnlExplanation);
            this.Controls.Add(this.Label23);
            this.Controls.Add(this.cmbReasonEnded);
            this.Controls.Add(this.Label8);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TPartnerSubscCancelAllWinForm";
            this.Text = "Cancel All Subscriptions";
            this.Load += new System.EventHandler(this.TPartnerSubscCancelAllWinForm_Load);
            this.Controls.SetChildIndex(this.stbMain, 0);
            this.Controls.SetChildIndex(this.pnlBtnOKCancelHelpLayout, 0);
            this.Controls.SetChildIndex(this.Label8, 0);
            this.Controls.SetChildIndex(this.cmbReasonEnded, 0);
            this.Controls.SetChildIndex(this.Label23, 0);
            this.Controls.SetChildIndex(this.pnlExplanation, 0);
            this.Controls.SetChildIndex(this.txtDateEnded, 0);
            this.pnlBtnOKCancelHelpLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stpInfo)).EndInit();
            this.pnlExplanation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

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

        public TPartnerSubscCancelAllWinForm() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            FDateEndedPreset = DateTime.MinValue;
        }

        /// <summary>
        /// Verifies the Reason Ended ComboBox value.
        ///
        /// </summary>
        /// <returns>true if value is allowed, otherwise false
        /// </returns>
        private Boolean VerifyReasonEnded()
        {
            Boolean ReturnValue;

            if (cmbReasonEnded.SelectedItem.ToString() == "")
            {
                MessageBox.Show(StrVerificationErrReasonEnded,
                    ResVerification.StrGenericInvalidData);
                ReturnValue = false;
            }
            else
            {
                ReturnValue = true;
            }

            return ReturnValue;
        }

        private void TPartnerSubscCancelAllWinForm_Load(System.Object sender, System.EventArgs e)
        {
            DateTime DateEnded;

            cmbReasonEnded.InitialiseUserControl();

            if (FDateEndedPreset == DateTime.MinValue)
            {
                DateEnded = DateTime.Today;
            }
            else
            {
                // MessageBox.Show('FDateEndedPreset: ' + FDateEndedPreset.ToString);
                DateEnded = FDateEndedPreset;

                // MessageBox.Show('Presetting the Reason Ended to ''BADADDR''');
                // FDateEnded was set via the DateEnded property, which happens if this
                // Dialog is called from the 'Deactivate Partner' process. In this case
                // the probability is high that this is done because mail was returned
                // undeliverable, so we preset the Reason Ended to 'BADADDR'.
                cmbReasonEnded.SelectedValue = MPartnerConstants.SUBSCRIPTIONS_REASON_ENDED_BADADDR;
            }

            txtDateEnded.Date = DateEnded;
            sbtForm.SetStatusBarText(this.cmbReasonEnded, PSubscriptionTable.GetReasonSubsCancelledCodeHelp());
            sbtForm.SetStatusBarText(this.txtDateEnded, PSubscriptionTable.GetDateCancelledHelp());
            FFormSetupFinished = true;
        }

        private void CmbReasonEnded_Validating(System.Object sender, System.ComponentModel.CancelEventArgs e)
        {
            VerifyReasonEnded();
        }

        /// <summary>
        /// Called by the instantiator of this Dialog to retrieve the values of Fields
        /// on the screen.
        ///
        /// </summary>
        /// <param name="AReasonEnded">Text that gives the reason for ending the Subscriptions</param>
        /// <param name="ADateEnded">Date when the Subscriptions should end (can be empty)</param>
        /// <returns>false if the Dialog is still uninitialised, otherwise true.
        /// </returns>
        public Boolean GetReturnedParameters(out String AReasonEnded, out System.DateTime ADateEnded)
        {
            Boolean ReturnValue;

            AReasonEnded = "";
            ADateEnded = DateTime.Now;

            if (FFormSetupFinished)
            {
                AReasonEnded = cmbReasonEnded.SelectedItem.ToString();
                ADateEnded = txtDateEnded.Date;
                ReturnValue = true;
            }
            else
            {
                ReturnValue = false;
            }

            return ReturnValue;
        }

        private void BtnOK_Click(System.Object sender, System.EventArgs e)
        {
            if (!VerifyReasonEnded())
            {
                cmbReasonEnded.Focus();
                return;
            }

            if (!txtDateEnded.ValidDate(true))
            {
                txtDateEnded.Focus();
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}