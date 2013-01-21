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
using System.Collections;
using System.Data;
using System.Windows.Forms;

using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonControls;
using Ict.Common;

namespace Ict.Petra.Client.MCommon.Gui
{
    /// <summary>handle event of filter change</summary>
    public delegate void TEventHandlerEventFilterChanged(System.Object sender, System.EventArgs e);

    public partial class TUC_EventFilter
    {
        private bool FDuringInitialization = true;

        #region Events

        /// <summary>todoComment</summary>
        public event TEventHandlerEventFilterChanged EventFilterChanged;

        #endregion

        #region Properties

        /// <summary>Filter Criteria: show conference units</summary>
        public bool IncludeConferenceUnits
        {
            get
            {
                return rbtAll.Checked || rbtConference.Checked;
            }
        }

        /// <summary>Filter Criteria: show outreach units</summary>
        public bool IncludeOutreachUnits
        {
            get
            {
                return rbtAll.Checked || rbtOutreach.Checked;
            }
        }

        /// <summary>Filter Criteria: only show current or future events</summary>
        public bool CurrentAndFutureEventsOnly
        {
            get
            {
                return chkCurrentFutureOnly.Checked;
            }
        }

        /// <summary>Rowfilter initialized according to filter criteria</summary>
        public String NameFilter
        {
            get
            {
                return txtEventName.Text;
            }
        }

        #endregion


        #region Public Methods

        /// arrange the panels and controls according to the partner class
        public void InitialiseUserControl()
        {
            FDuringInitialization = true;
            rbtConference.Checked = true;
            chkCurrentFutureOnly.Checked = true;
            FDuringInitialization = false;
        }

        /// <summary>
        /// Initialisation
        /// </summary>
        public void InitialiseData(Ict.Petra.Client.CommonForms.TFrmPetraUtils APetraUtilsObject)
        {
            // not really needed at the moment. Just needed as dummy code for use in code generation.
        }

        /// <summary>
        /// Sets the available functions (fields) that can be used for a report.
        /// </summary>
        /// <param name="AAvailableFunctions">List of TColumnFunction</param>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
            // this is just a dummy implementation to compiling works for use in reporting
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <param name="ARecordChangeVerification"></param>
        /// <param name="AProcessAnyDataValidationErrors"></param>
        /// <param name="AValidateSpecificControl"></param>
        /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
        public bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors, Control AValidateSpecificControl = null)
        {
            // this is just a dummy implementation to compiling works for use in reporting
            return true;
        }

        /// <summary>
        /// Retrieves data that is in the Controls and puts it into the Tables in FMainDS
        /// </summary>
        public void GetDataFromControls()
        {
        }

        #endregion

        #region Private Methods

        private void EventTypeChanged(System.Object sender, EventArgs e)
        {
            if (FDuringInitialization)
            {
                return;
            }

            RadioButton RadioBtn = (RadioButton)sender;

            if (RadioBtn.Checked)
            {
                this.EventFilterChanged(sender, e);
            }
        }

        private void EventDateChanged(System.Object sender, EventArgs e)
        {
            if (FDuringInitialization)
            {
                return;
            }

            this.EventFilterChanged(sender, e);
        }

        private void FilterEvents(System.Object sender, EventArgs e)
        {
            if (FDuringInitialization)
            {
                return;
            }

            this.EventFilterChanged(sender, e);
        }

        private void ClearFilterEvents(System.Object sender, EventArgs e)
        {
            if (FDuringInitialization)
            {
                return;
            }

            txtEventName.Text = "";
            this.EventFilterChanged(sender, e);
        }

        #endregion


        #region Event handlers


        #endregion
    }
}