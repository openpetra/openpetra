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

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_ApplicationPage_Field
    {
        // <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        //private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        /// <summary>event to signalize change in field applied for</summary>
        public event TDelegatePartnerChanged ApplicationFieldChanged;

        #region Properties

        /// return label text for "Field" field
        public String FieldLabelText
        {
            get
            {
                return lblField.Text;
            }
        }

        /// return code value for "Field"
        public String FieldValueCode
        {
            get
            {
                return txtField.Text;
            }
        }

        /// return label value for "Field"
        public String FieldValueLabel
        {
            get
            {
                return txtField.LabelText;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

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

        private void OnRecalculateScreenParts(TRecalculateScreenPartsEventArgs e)
        {
            if (RecalculateScreenParts != null)
            {
                RecalculateScreenParts(this, e);
            }
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

        #region Private Methods

        private void InitializeManualCode()
        {
        }

        private void GetDataFromControlsManual(PmGeneralApplicationRow ARow)
        {
            // need to make sure that partner key fields that are not referring to p_partner table
            // but to other tables like p_unit or p_person are set to NULL when they are empty (and
            // not to 0 as then foreign key constraints will fail)
            // HOWEVER: at the moment the thinking is (according to WolfgangB, and Petra 2.x's implementation)
            // that the Target Field needs to be specified for an Application to be valid!
//            if ((txtField.Text.Length != 0)
//                && (Convert.ToInt64(txtField.Text) == 0))
//            {
//                ARow.SetGenAppPossSrvUnitKeyNull();
//            }

            if ((txtPlacementPerson.Text.Length != 0)
                && (Convert.ToInt64(txtPlacementPerson.Text) == 0))
            {
                ARow.SetPlacementPartnerKeyNull();
            }
        }

        private void ValidateDataManual(PmGeneralApplicationRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPersonnelValidation_Personnel.ValidateGeneralApplicationManual(this, ARow, false, ref VerificationResultCollection,
                FValidationControlsDict);

            TSharedPersonnelValidation_Personnel.ValidateFieldApplicationManual(this,
                FMainDS.PmYearProgramApplication[0],
                ref VerificationResultCollection,
                FValidationControlsDict);
        }

        private void ProcessApplicationFieldChanged(Int64 APartnerKey, String APartnerShortName, bool AValidSelection)
        {
            // trigger event so other controls can react to it
            this.ApplicationFieldChanged(APartnerKey, APartnerShortName, AValidSelection);
        }

        #endregion
    }
}