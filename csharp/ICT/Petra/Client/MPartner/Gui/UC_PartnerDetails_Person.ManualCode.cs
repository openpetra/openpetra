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
using System.Windows.Forms;

using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_PartnerDetails_Person
    {
        #region Public Methods

        private void InitializeManualCode()
        {
            // The TextBoxes are padded in YAML, but we need to move the Labels, too...
            lblDecorations.Left = lblDecorations.Left + 45;
            lblAcademicTitle.Left = lblAcademicTitle.Left + 45;

            txtOccupationCode.TextBoxWidth = 200;
            txtOccupationCode.CharacterCasing = CharacterCasing.Upper;
        }

        /// <summary>
        /// Gets the data from all controls on this UserControl.
        /// The data is stored in the DataTables/DataColumns to which the Controls
        /// are mapped.
        /// </summary>
        public void GetDataFromControls2()
        {
            GetDataFromControls(FMainDS.PPerson[0]);
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        #endregion

        private void ValidateDataManual(PPersonRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidatePartnerPersonManual(this,
                ARow,
                @TDataCache.GetCacheableDataTableFromCache,
                ref VerificationResultCollection,
                FValidationControlsDict);
        }

        #region Menu and command key handlers for our user controls

        /// <summary>
        /// Handler for command key processing
        /// </summary>
        private bool ProcessCmdKeyManual(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.E | Keys.Control))
            {
                this.txtPreferredName.Focus();
                return true;
            }

            return false;
        }

        #endregion
    }
}