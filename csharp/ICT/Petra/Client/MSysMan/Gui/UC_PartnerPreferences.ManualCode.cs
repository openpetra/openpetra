//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Ict.Common;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MSysMan;

namespace Ict.Petra.Client.MSysMan.Gui
{
    /// manual methods for the generated window
    public partial class TUC_PartnerPreferences
    {
        private void InitializeManualCode()
        {
        }

        /// <summary>
        /// Gets the data from all UserControls on this TabControl.
        /// </summary>
        /// <returns>void</returns>
        public void GetDataFromControls()
        {
        }
        
        /// <summary>
        /// Saves any changed preferences to s_user_defaults
        /// </summary>
        /// <returns>void</returns>
        public bool SavePartnerTab()
        {
            
            return true;
        }

        /// <summary>
        /// Performs data validation.
        /// </summary>
        /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing
        /// to another record, otherwise set it to false.</param>
        /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
        /// user, otherwise set it to false.</param>
        /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
        public bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors)
        {
            bool ReturnValue = true;
            
            return ReturnValue;
        }
    }
}