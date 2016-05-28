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
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;

//using Ict.Petra.Client.MReporting.Gui;
//using Ict.Petra.Client.MReporting.Gui.MPersonnel;
//using Ict.Petra.Client.MReporting.Gui.MPersonnel.ShortTerm;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_ApplicationPage_Event
    {
        private TDelegateCheckEventApplicationDuplicate FDelegateCheckEventApplicationDuplicate = null;

        /// <summary>event to signalize change in event applied for</summary>
        public event TDelegatePartnerChanged ApplicationEventChanged;

        /// <summary>delegate to check for event duplicates for this person</summary>
        public delegate bool TDelegateCheckEventApplicationDuplicate(int AApplicationKey, Int64 ARegistrationOfficeKey, Int64 AEventKey);

        #region Public Methods

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        #region Properties
        /// return label text for "Event" field
        public String EventLabelText
        {
            get
            {
                return lblEvent.Text;
            }
        }

        /// return code value for "Event"
        public String EventValueCode
        {
            get
            {
                return txtEvent.Text;
            }
        }

        /// return label value for "Event"
        public String EventValueLabel
        {
            get
            {
                return txtEvent.LabelText;
            }
        }
        #endregion

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADelegateFunction"></param>
        public void InitialiseDelegateCheckEventApplicationDuplicate(TDelegateCheckEventApplicationDuplicate ADelegateFunction)
        {
            // set the delegate function from the calling System.Object
            FDelegateCheckEventApplicationDuplicate = ADelegateFunction;
        }

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
        }

        private void GetDataFromControlsManual(PmGeneralApplicationRow ARow)
        {
            // need to make sure that partner key fields that are not referring to p_partner table
            // but to other tables like p_unit or p_person are set to NULL when they are empty (and
            // not to 0 as then foreign key constraints will fail)
            if ((txtFieldCharged.Text.Length != 0)
                && (Convert.ToInt64(txtFieldCharged.Text) == 0))
            {
                FMainDS.PmShortTermApplication[0].SetStFieldChargedNull();
            }

            if ((txtPlacementPerson.Text.Length != 0)
                && (Convert.ToInt64(txtPlacementPerson.Text) == 0))
            {
                ARow.SetPlacementPartnerKeyNull();
            }

            if ((FMainDS.PmShortTermApplication != null) && (FMainDS.PmShortTermApplication.Rows.Count > 0))
            {
                if (txtEvent.Text.Length == 0)
                {
                    FMainDS.PmShortTermApplication[0].SetConfirmedOptionCodeNull();
                }
                else
                {
                    FMainDS.PmShortTermApplication[0].ConfirmedOptionCode =
                        TRemote.MPersonnel.Person.DataElements.WebConnectors.GetOutreachCode(Convert.ToInt64(txtEvent.Text));
                }
            }
        }

        private void ValidateDataManual(PmGeneralApplicationRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;

            TSharedPersonnelValidation_Personnel.ValidateGeneralApplicationManual(this, ARow, true, ref VerificationResultCollection,
                FValidationControlsDict);

            TSharedPersonnelValidation_Personnel.ValidateEventApplicationManual(this,
                FMainDS.PmShortTermApplication[0],
                ref VerificationResultCollection,
                FValidationControlsDict);

            if (FDelegateCheckEventApplicationDuplicate != null)
            {
                // Same 'Event' must only exist for one application per person
                ValidationColumn = FMainDS.PmShortTermApplication[0].Table.Columns[PmShortTermApplicationTable.ColumnStConfirmedOptionId];

                if (FValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
                {
                    if (FDelegateCheckEventApplicationDuplicate(ARow.ApplicationKey, ARow.RegistrationOffice,
                            FMainDS.PmShortTermApplication[0].StConfirmedOption))
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(this,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_APPLICATION_DUPLICATE_EVENT,
                                    new string[] { FMainDS.PmShortTermApplication[0].StConfirmedOption.ToString() })),
                            ValidationColumn, ValidationControlsData.ValidationControl);

                        // Handle addition to/removal from TVerificationResultCollection.
                        // Only add/remove verification result if duplicate found as same field has already been
                        // handled in TSharedPersonnelValidation_Personnel.ValidateEventApplicationManual
                        VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn);
                    }
                }
            }
        }

        private void ProcessApplicationEventChanged(Int64 APartnerKey, String APartnerShortName, bool AValidSelection)
        {
            // trigger event so other controls can react to it
            this.ApplicationEventChanged(APartnerKey, APartnerShortName, AValidSelection);
        }

        #endregion
    }
}