//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using GNU.Gettext;
using Ict.Common;
using Ict.Petra.Client.CommonControls;
using Ict.Common.Controls;
using System.Globalization;

namespace Ict.Petra.Client.MPartner.Gui
{
    /// <summary>
    /// UserControl that forms a part of the Find Criteria of the Partner Find
    /// screen.
    ///
    /// @Comment At the moment not functionional, just there to demonstrate how this
    ///   could look like in the future.
    /// </summary>
    public partial class TUC_PartnerFind_PersonnelCriteria_CollapsiblePart : TGrpCollapsible
    {
        /// <summary>
        /// constructor
        /// </summary>
        public TUC_PartnerFind_PersonnelCriteria_CollapsiblePart() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            #region CATALOGI18N

            // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
            this.lblCommitmentFieldOffice.Text = Catalog.GetString("Commitm. Field &Office") + ":" +;
            this.lblCommitmentFOFromTo.Text = Catalog.GetString("From/To") + ":" +;
            this.Label3.Text = Catalog.GetString("/");
            this.chkFOOnlyCurrentCommitments.Text = Catalog.GetString("Only Current");
            this.btnChooseCommitmentFieldOffice.Text = Catalog.GetString("...");
            this.btnChooseEventConferenceOutreach.Text = Catalog.GetString("...");
            this.lblEventConferenceOutreach.Text = Catalog.GetString("Eve&nt/Confer./Camp.") + ":" +;
            this.lblPersonLanguage.Text = Catalog.GetString("Lang&uage") + ":" +;
            #endregion

            // Extender Provider
            this.expStringLengthCheckPersonalCriteriaCollapsiblePart.RetrieveTextboxes(this);
        }

        private void ChkFOOnlyCurrentCommitments_CheckStateChanged(System.Object sender, System.EventArgs e)
        {
            if (chkFOOnlyCurrentCommitments.Checked)
            {
                CustomEnablingDisabling.DisableControl(pnlCommitmentFieldOffice, pnlFromTo);
            }
            else
            {
                CustomEnablingDisabling.EnableControl(pnlCommitmentFieldOffice, pnlFromTo);
            }
        }

        /// <summary>
        /// Private Declarations
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseUserControl()
        {
            CustomEnablingDisabling.DisableControl(pnlCommitmentFieldOffice, pnlFromTo);
        }
    }
}