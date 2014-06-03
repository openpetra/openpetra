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
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MSysMan;

namespace Ict.Petra.Client.MSysMan.Gui
{
    /// manual methods for the generated window
    public partial class TUC_PartnerPreferences
    {
        private void InitializeManualCode()
        {
            // Hide invalid Acquisition Codes
            cmbAcquisitionCode.Filter = PAcquisitionTable.GetValidAcquisitionDBName() + " <> 0";

            // set values for controls
            cmbAcquisitionCode.SetSelectedString(TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_ACQUISITIONCODE, "MAILROOM"));
            cmbLanguageCode.SetSelectedString(TUserDefaults.GetStringDefault(MSysManConstants.PARTNER_LANGUAGECODE, "99"));
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
        public void SavePartnerTab()
        {
            TUserDefaults.SetDefault(TUserDefaults.PARTNER_ACQUISITIONCODE, cmbAcquisitionCode.GetSelectedString());
            TUserDefaults.SetDefault(TUserDefaults.PARTNER_LANGUAGECODE, cmbLanguageCode.GetSelectedString());
        }
    }
}