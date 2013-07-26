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
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TUC_Application_Event
    {
        // <summary>holds a reference to the Proxy System.Object of the Serverside UIConnector</summary>
        //private IPartnerUIConnectorsPartnerEdit FPartnerEditUIConnector;

        private IndividualDataTDS FMainDS;          // FMainDS is NOT of Type 'PartnerEditTDS' in this UserControl!!!
        private ApplicationTDS FApplicationDS;
        private int CurrentTabIndex = 0;

        /// <summary>Application Event changed</summary>
        public delegate void TDelegateApplicationEventChanged(Int64 APartnerKey,
            int AApplicationKey,
            Int64 ARegistrationOffice,
            Int64 AEventKey,
            String AEventName);

        /// <summary>event to signalize change in event applied for</summary>
        public event TDelegateApplicationEventChanged ApplicationEventChanged;

        #region Properties

        /// dataset for the whole screen
        public IndividualDataTDS MainDS
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

        /// return label text for "Event" field
        public String EventLabelText
        {
            get
            {
                return ucoEvent.EventLabelText;
            }
        }
    
        /// return code value for "Event"
        public String EventValueCode
        {
            get
            {
                return ucoEvent.EventValueCode;
            }
        }
    
        /// return label value for "Event"
        public String EventValueLabel
        {
            get
            {
                return ucoEvent.EventValueLabel;
            }
        }
        
        #endregion

        #region Public Methods

        /// <summary>todoComment</summary>
        public event TRecalculateScreenPartsEventHandler RecalculateScreenParts;

        /// <summary>todoComment</summary>
        public event THookupPartnerEditDataChangeEventHandler HookupDataChange;

        /// <summary>
        /// Sets up the screen logic, retrieves data, databinds the Grid and the Detail
        /// UserControl.
        /// </summary>
        /// <returns>void</returns>
        public void InitialiseUserControl()
        {
            FApplicationDS = new ApplicationTDS();
            FApplicationDS.InitVars();

            // the following lines are just dummy code to remove compiler warnings as those members are never used
            if (FTabSetup == null)
            {
                FTabSetup = null;
            }

            if (FTabPageEvent == null)
            {
                FTabPageEvent = null;
            }

            ucoEvent.PetraUtilsObject = FPetraUtilsObject;
            ucoApplicant.PetraUtilsObject = FPetraUtilsObject;
            ucoTravel.PetraUtilsObject = FPetraUtilsObject;

            ucoEvent.MainDS = FApplicationDS;
            ucoApplicant.MainDS = FApplicationDS;
            ucoTravel.MainDS = FApplicationDS;

            // enable control to react to modified event or field key in details part
            ucoEvent.ApplicationEventChanged += new TDelegatePartnerChanged(ProcessApplicationEventChanged);

            // handle tab changing in case validation fails
            tabApplicationEvent.Selecting += new TabControlCancelEventHandler(TabSelectionChanging);

            // initialize delegate method on event page so it can be called from there for validation purposes
            ucoEvent.InitialiseDelegateCheckEventApplicationDuplicate(CheckEventApplicationDuplicate);
        }

        /// <summary>
        /// Display data in control based on data from Rows
        /// </summary>
        /// <param name="AGeneralAppRow"></param>
        /// <param name="AEventAppRow"></param>
        public void ShowDetails(PmGeneralApplicationRow AGeneralAppRow, PmShortTermApplicationRow AEventAppRow)
        {
            ShowData(AGeneralAppRow, AEventAppRow);
        }

        /// <summary>
        /// Read data from controls into Row parameters
        /// </summary>
        /// <param name="ARow"></param>
        /// <param name="AEventAppRow"></param>
        public void GetDetails(PmGeneralApplicationRow ARow, PmShortTermApplicationRow AEventAppRow)
        {
            GetDataFromControls(ARow, AEventAppRow);
        }

        /// <summary>
        /// This Method is needed for UserControls who get dynamicly loaded on TabPages.
        /// Since we don't have controls on this UserControl that need adjusting after resizing
        /// on 'Large Fonts (120 DPI)', we don't need to do anything here.
        /// </summary>
        public void AdjustAfterResizing()
        {
        }

        /// <summary>
        /// Check if there is already an application existing for that person at this event
        /// </summary>
        /// <param name="AApplicationKey"></param>
        /// <param name="ARegistrationOfficeKey"></param>
        /// <param name="AEventKey"></param>
        public bool CheckEventApplicationDuplicate(int AApplicationKey, Int64 ARegistrationOfficeKey,
            Int64 AEventKey)
        {
            PmShortTermApplicationRow EventApplicationRow;

            foreach (DataRow ApplicationRow in FMainDS.PmShortTermApplication.Rows)
            {
                if (ApplicationRow.RowState != DataRowState.Deleted)
                {
                    EventApplicationRow = (PmShortTermApplicationRow)ApplicationRow;

                    if (((EventApplicationRow.ApplicationKey != AApplicationKey)
                         || (EventApplicationRow.RegistrationOffice != ARegistrationOfficeKey))
                        && !EventApplicationRow.IsStConfirmedOptionNull()
                        && (EventApplicationRow.StConfirmedOption == AEventKey))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Private Methods

        private void RethrowRecalculateScreenParts(System.Object sender, TRecalculateScreenPartsEventArgs e)
        {
            OnRecalculateScreenParts(e);
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

        private void InitializeManualCode()
        {
        }

        private void GetDataFromControlsManual(PmGeneralApplicationRow ARow)
        {
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <remarks>May be called by the Form that hosts this UserControl to invoke the data validation of
        /// the UserControl.</remarks>
        /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
        /// user, otherwise set it to false.</param>
        /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
        public bool ValidateAllData(bool AProcessAnyDataValidationErrors)
        {
            bool ReturnValue = false;

            ReturnValue = ucoEvent.ValidateAllData(AProcessAnyDataValidationErrors);

            if (ReturnValue)
            {
                ReturnValue = ucoApplicant.ValidateAllData(AProcessAnyDataValidationErrors);
            }

            if (ReturnValue)
            {
                ReturnValue = ucoTravel.ValidateAllData(AProcessAnyDataValidationErrors);
            }

            return ReturnValue;
        }

        private void ShowData(PmGeneralApplicationRow AGeneralAppRow, PmShortTermApplicationRow AEventAppRow)
        {
            // clear dataset and create a copy of the row to be displayed so Dataset contains only one set of records
            FApplicationDS.PmShortTermApplication.Rows.Clear();
            FApplicationDS.PmGeneralApplication.Rows.Clear();

            PmGeneralApplicationRow GeneralAppRowCopy = (PmGeneralApplicationRow)FApplicationDS.PmGeneralApplication.NewRow();
            PmShortTermApplicationRow EventAppRowCopy = (PmShortTermApplicationRow)FApplicationDS.PmShortTermApplication.NewRow();

            DataUtilities.CopyAllColumnValues(AGeneralAppRow, GeneralAppRowCopy);
            DataUtilities.CopyAllColumnValues(AEventAppRow, EventAppRowCopy);

            FApplicationDS.PmGeneralApplication.Rows.Add(GeneralAppRowCopy);
            FApplicationDS.PmShortTermApplication.Rows.Add(EventAppRowCopy);

            ucoEvent.ShowDetails(GeneralAppRowCopy);
            ucoApplicant.ShowDetails(GeneralAppRowCopy);
            ucoTravel.ShowDetails(GeneralAppRowCopy);
        }

        private void GetDataFromControls(PmGeneralApplicationRow ARow, PmShortTermApplicationRow AEventAppRow)
        {
            ucoEvent.GetDetails(FApplicationDS.PmGeneralApplication[0]);
            ucoApplicant.GetDetails(FApplicationDS.PmGeneralApplication[0]);
            ucoTravel.GetDetails(FApplicationDS.PmGeneralApplication[0]);

            DataUtilities.CopyAllColumnValues(FApplicationDS.PmGeneralApplication[0], ARow);
            DataUtilities.CopyAllColumnValues(FApplicationDS.PmShortTermApplication[0], AEventAppRow);
        }

        private void ProcessApplicationEventChanged(Int64 AEventKey, String AEventName, bool AValidSelection)
        {
            PmGeneralApplicationRow Row;

            Row = (PmGeneralApplicationRow)FApplicationDS.PmGeneralApplication.Rows[0];

            // trigger event so parent controls can react
            this.ApplicationEventChanged(Row.PartnerKey, Row.ApplicationKey, Row.RegistrationOffice, AEventKey, AEventName);
        }

        private void TabSelectionChanging(object sender, TabControlCancelEventArgs e)
        {
            FPetraUtilsObject.VerificationResultCollection.Clear();

            if (CurrentTabIndex == 0)
            {
                FCurrentUserControl = ucoEvent;

                if (!ucoEvent.ValidateAllData(true, FCurrentUserControl))
                {
                    e.Cancel = true;

                    FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = true;
                }
            }
            else if (CurrentTabIndex == 1)
            {
                FCurrentUserControl = ucoApplicant;

                if (!ucoApplicant.ValidateAllData(true, FCurrentUserControl))
                {
                    e.Cancel = true;

                    FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = true;
                }
            }
            else
            {
                FCurrentUserControl = ucoTravel;

                if (!ucoTravel.ValidateAllData(true, FCurrentUserControl))
                {
                    e.Cancel = true;

                    FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = true;
                }
            }

            if (!e.Cancel)
            {
                CurrentTabIndex = tabApplicationEvent.SelectedIndex;
            }
        }

        #endregion
    }
}