//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2012 by OM International
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
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Validation;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPersonnel.Personnel.Validation;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_ApplicationPage_Travel
    {
        // <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        //private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        #region Public Methods

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        /// <summary>
        /// Display data in control based on data from ARow
        /// </summary>
        /// <param name="ARow"></param>
        public void ShowDetails(PmGeneralApplicationRow ARow)
        {
            // set member
            //FApplicationDR = ARow;

            ShowData(ARow);
        }

        /// <summary>
        /// Read data from controls into ARow parameter
        /// </summary>
        /// <param name="ARow"></param>
        public void GetDetails(PmGeneralApplicationRow ARow)
        {
            GetDataFromControls(ARow);
        }

        private void OnHookupDataChange(THookupPartnerEditDataChangeEventArgs e)
        {
            if (HookupDataChange != null)
            {
                HookupDataChange(this, e);
            }
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamically loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        #endregion

        #region Private Methods

        private void InitializeManualCode()
        {
            // hour and minute text fields can only contain two digits maximum
            txtArrivalTimeHour.MaxLength = 2;
            txtArrivalTimeMinute.MaxLength = 2;
            txtDepartureTimeHour.MaxLength = 2;
            txtDepartureTimeMinute.MaxLength = 2;
        }

        private void GetDataFromControlsManual(PmGeneralApplicationRow ARow)
        {
        }

        private void ValidateDataManual(PmGeneralApplicationRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPersonnelValidation_Personnel.ValidateGeneralApplicationManual(this, ARow, true, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);

            // We need to call the generated validation code for PmShortTermApplication manually as the UserControl's generate code calls
            // generated validation code only for the MasterTable, PmGeneralApplication!
            PmShortTermApplicationValidation.Validate(this,
                FMainDS.PmShortTermApplication[0], ref VerificationResultCollection,
                FValidationControlsDict);

            TSharedPersonnelValidation_Personnel.ValidateEventApplicationManual(this,
                FMainDS.PmShortTermApplication[0],
                ref VerificationResultCollection,
                FValidationControlsDict);
        }

        #endregion
    }
}