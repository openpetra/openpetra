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
        private bool FShowMoneyAsCurrency = false;
        private bool FShowDecimalsAsCurrency = false;
        private bool FShowThousands = true;

        private void InitializeManualCode()
        {
            // Hide invalid Acquisition Codes
            cmbAcquisitionCode.Filter = PAcquisitionTable.GetValidAcquisitionDBName() + " <> 0";

            // set values for controls
            cmbAcquisitionCode.SetSelectedString(TUserDefaults.GetStringDefault(TUserDefaults.PARTNER_ACQUISITIONCODE, "MAILROOM"));
            cmbLanguageCode.SetSelectedString(TUserDefaults.GetStringDefault(MSysManConstants.PARTNER_LANGUAGECODE, "99"));

            FShowMoneyAsCurrency = TUserDefaults.GetBooleanDefault(StringHelper.PARTNER_CURRENCY_FORMAT_AS_CURRENCY, false);
            chkMoneyFormat.Checked = FShowMoneyAsCurrency;

            FShowDecimalsAsCurrency = TUserDefaults.GetBooleanDefault(StringHelper.PARTNER_DECIMAL_FORMAT_AS_CURRENCY, false);
            chkDecimalFormat.Checked = FShowDecimalsAsCurrency;

            FShowThousands = TUserDefaults.GetBooleanDefault(StringHelper.PARTNER_CURRENCY_SHOW_THOUSANDS, true);
            chkShowThousands.Checked = FShowThousands;

            // Examples
            txtCostExample.Context = ".MPartner";
            txtCostExample.CurrencyCode = "USD";
            txtCostExample.NumberValueDecimal = 1234.56m;

            txtNumericExample.Context = ".MPartner";
            txtNumericExample.NumberValueDecimal = 1.75m;
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

            if (FShowMoneyAsCurrency != chkMoneyFormat.Checked)
            {
                FShowMoneyAsCurrency = chkMoneyFormat.Checked;
                TUserDefaults.SetDefault(StringHelper.PARTNER_CURRENCY_FORMAT_AS_CURRENCY, FShowMoneyAsCurrency);
            }

            if (FShowDecimalsAsCurrency != chkDecimalFormat.Checked)
            {
                FShowDecimalsAsCurrency = chkDecimalFormat.Checked;
                TUserDefaults.SetDefault(StringHelper.PARTNER_DECIMAL_FORMAT_AS_CURRENCY, FShowDecimalsAsCurrency);
            }

            if (FShowThousands != chkShowThousands.Checked)
            {
                FShowThousands = chkShowThousands.Checked;
                TUserDefaults.SetDefault(StringHelper.PARTNER_CURRENCY_SHOW_THOUSANDS, FShowThousands);
            }
        }

        private void ExampleCheckChanged(object sender, EventArgs e)
        {
            txtCostExample.OverrideNormalFormatting(!chkMoneyFormat.Checked, chkShowThousands.Checked);
            txtNumericExample.OverrideNormalFormatting(!chkDecimalFormat.Checked, chkShowThousands.Checked);
        }
    }
}