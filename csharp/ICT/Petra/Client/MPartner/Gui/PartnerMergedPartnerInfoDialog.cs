//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2016 by OM International
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
using System;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Shared;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// Dialog to display information about a Partner that was
    /// Merged into another Partner.
    /// If information about the Merged-Into Partner is available,
    /// the user can opt to take this Partner instead of the
    /// Merged Partner by choosing 'OK'. If this information isn't
    /// available, the user can only choose 'Cancel'.
    /// </summary>
    /// <remarks>The caller can evaluate the DialogResult to know
    /// whether to use the PartnerKey of the Merged-Into Partner
    /// (DialogResult.OK) or to prevent the user from proceeding
    /// (DialogReusult.Cancel).</remarks>
    public partial class TPartnerMergedPartnerInfoDialog : System.Windows.Forms.Form,
           Ict.Petra.Client.CommonForms.IFrmPetra
    {
        private readonly Ict.Petra.Client.CommonForms.TFrmPetraUtils FPetraUtilsObject;
        private Int64 FMergedPartnerPartnerKey = -1;
        private String FMergedPartnerPartnerShortName;
        private TPartnerClass FMergedPartnerPartnerClass;
        private Int64 FMergedIntoPartnerKey;
        private String FMergedIntoPartnerShortName;
        private TPartnerClass FMergedIntoPartnerClass;
        private String FMergedBy;
        private DateTime FMergeDate;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="AParentForm"></param>
        public TPartnerMergedPartnerInfoDialog(Form AParentForm) : base()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.btnOK.Text = Catalog.GetString("&OK");
            this.btnCancel.Text = Catalog.GetString("&Cancel");
            this.lblHeading.Text = Catalog.GetString("The Partner that you have selected,");
            this.lblHeading2.Text = Catalog.GetString("was Merged into another Partner and is therefore no longer accessible.");
            this.txtMergedPartner.LabelText = Catalog.GetString("Merged, Partner, A   [PERSON]");
            this.label3.Text = Catalog.GetString("Merge Date") + ":";
            this.label1.Text = Catalog.GetString("Partner Class") + ":";
            this.txtMergedIntoPartner.LabelText = Catalog.GetString("Merged-Into, Partner, A");
            this.lblMergedIntoPartnerKeyName.Text = Catalog.GetString("Partner Key && Name") + ":";
            this.txtMergedIntoPartnerClass.Text = Catalog.GetString("PERSON");
            this.label2.Text = Catalog.GetString("Merged By") + ":";
            this.txtMergedBy.Text = Catalog.GetString("DUMMY");
            this.txtMergeDate.Text = Catalog.GetString("17-FEB-2009");
            this.lblMergedIntoPartnerInfo.Text = Catalog.GetString("This is the Partner that it got merged into") + ":";
            this.lblInstructions.Text = Catalog.GetString("Choose \'OK\' to accept the Merged-Into Partner, or \'Cancel\' to stop the operation ");
            this.Text = Catalog.GetString("Merged Partner Information");
            #endregion

            FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(AParentForm, this, stbMain);
        }

        void TPartnerMergedPartnerInfoDialogLoad(object sender, System.EventArgs e)
        {
            UpdateUI();
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// </summary>
        /// <param name="AMergedPartnerPartnerKey">PartnerKey of the Merged Partner.</param>
        /// <param name="AMergedPartnerPartnerShortName">Shortname of the Merged Partner.</param>
        /// <param name="AMergedPartnerPartnerClass">PartnerClass of the Merged Partner.</param>
        public void SetParameters(Int64 AMergedPartnerPartnerKey,
            String AMergedPartnerPartnerShortName,
            TPartnerClass AMergedPartnerPartnerClass)
        {
            FMergedPartnerPartnerKey = AMergedPartnerPartnerKey;
            FMergedPartnerPartnerShortName = AMergedPartnerPartnerShortName;
            FMergedPartnerPartnerClass = AMergedPartnerPartnerClass;
            FMergedIntoPartnerKey = -1;
        }

        /// <summary>
        /// Used for passing parameters to the screen before it is actually shown.
        /// </summary>
        /// <param name="AMergedPartnerPartnerKey">PartnerKey of the Merged Partner.</param>
        /// <param name="AMergedPartnerPartnerShortName">ShortName of the Merged Partner.</param>
        /// <param name="AMergedPartnerPartnerClass">PartnerClass of the Merged Partner.</param>
        /// <param name="AMergedIntoPartnerKey">PartnerKey of the Partner that the Merged Partner got merged into.</param>
        /// <param name="AMergedIntoPartnerShortName">ShortName of the Partner that the Merged Partner got merged into.</param>
        /// <param name="AMergedIntoPartnerClass">PartnerClass of the Partner that the Merged Partner got merged into.</param>
        /// <param name="AMergedBy">The user that merged the two Partners.</param>
        /// <param name="AMergeDate">Date on which the two Partners were merged.</param>
        public void SetParameters(Int64 AMergedPartnerPartnerKey,
            String AMergedPartnerPartnerShortName,
            TPartnerClass AMergedPartnerPartnerClass,
            Int64 AMergedIntoPartnerKey,
            String AMergedIntoPartnerShortName,
            TPartnerClass AMergedIntoPartnerClass,
            String AMergedBy,
            DateTime AMergeDate)
        {
            FMergedPartnerPartnerKey = AMergedPartnerPartnerKey;
            FMergedPartnerPartnerShortName = AMergedPartnerPartnerShortName;
            FMergedPartnerPartnerClass = AMergedPartnerPartnerClass;
            FMergedIntoPartnerKey = AMergedIntoPartnerKey;
            FMergedIntoPartnerShortName = AMergedIntoPartnerShortName;
            FMergedIntoPartnerClass = AMergedIntoPartnerClass;
            FMergedBy = AMergedBy;
            FMergeDate = AMergeDate;
        }

        private void UpdateUI()
        {
            if (FMergedPartnerPartnerKey != -1)
            {
                txtMergedPartner.PartnerKey = FMergedPartnerPartnerKey;
                txtMergedPartner.LabelText = FMergedPartnerPartnerShortName +
                                             "   [" + SharedTypes.PartnerClassEnumToString(FMergedPartnerPartnerClass) +
                                             "]";

                if (FMergedIntoPartnerKey != -1)
                {
                    txtMergedIntoPartner.PartnerKey = FMergedIntoPartnerKey;
                    txtMergedIntoPartner.LabelText = FMergedIntoPartnerShortName;
                    txtMergedIntoPartnerClass.Text = SharedTypes.PartnerClassEnumToString(
                        FMergedIntoPartnerClass);
                    txtMergedBy.Text = FMergedBy;
                    txtMergeDate.Date = FMergeDate;
                }
                else
                {
                    pnlMergedIntoPartner.Visible = false;
                    lblInstructions.Text = MPartnerResourcestrings.StrMergedPartnerNotPossible;
                    btnOK.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Need to call one of the SetParameters Methods to initialise this Dialog!",
                    "Developer Message");
                Close();
            }
        }

        #region Button Click-Events

        private void BtnOK_Click(System.Object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }

        private void BtnHelp_Click(System.Object sender, System.EventArgs e)
        {
            // TODO
        }

        #endregion

        #region Implement interface functions

        /// normally this would be auto generated
        public void RunOnceOnActivation()
        {
        }

        /// normally this would be auto generated
        public bool CanClose()
        {
            return FPetraUtilsObject.CanClose();
        }

        /// normally this would be auto generated
        public TFrmPetraUtils GetPetraUtilsObject()
        {
            return (TFrmPetraUtils)FPetraUtilsObject;
        }

        #endregion
    }
}
