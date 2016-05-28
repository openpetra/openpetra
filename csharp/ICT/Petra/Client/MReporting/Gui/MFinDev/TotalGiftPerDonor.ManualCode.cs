//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
//
// Copyright 2004-2011 by OM International
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
using Ict.Petra.Client.MReporting.Gui.MFinance;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MReporting;

namespace Ict.Petra.Client.MReporting.Gui.MFinDev
{
    public partial class TFrmTotalGiftPerDonor
    {
        private Int32 FLedgerNumber;

        /// <summary>
        /// the report should be run for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                uco_Selection.InitialiseLedger(FLedgerNumber);
                uco_Selection.ShowAccountHierarchy(false);
                uco_Selection.ShowCurrencySelection(false);
                uco_Selection.EnableDateSelection(true);

                FPetraUtilsObject.LoadDefaultSettings();
            }
        }

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            // make sure that for each group one radio button is selected
            uco_Selection.VerifyRadioButtonSelection(FPetraUtilsObject);
            TFrmPetraReportingUtils.VerifyRadioButtonSelection(rgrFormatCurrency, FPetraUtilsObject);
        }

        private void ReadControlsManual(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            ACalc.AddParameter("param_ledger_number_i", FLedgerNumber);
            ACalc.AddParameter("param_currency", "base");
            ACalc.AddParameter("param_ytd", "mixed");
            ACalc.AddParameter("param_currency", "base");
            ACalc.AddParameter("param_depth", "standard");

            String CountryCode = "*";

            if (chkOnlyFromCountry.Checked)
            {
                CountryCode = cmbCountry.GetSelectedString();

                if (CountryCode.Length == 0)
                {
                    CountryCode = "*";
                }
            }

            ACalc.AddParameter("param_country_code", CountryCode);
        }

        private void SetControlsManual(TParameterList AParameters)
        {
            String CountryCode = AParameters.Get("param_country_code").ToString();

            if ((CountryCode.Length > 0)
                && (CountryCode != "*"))
            {
                cmbCountry.SetSelectedString(CountryCode);
            }
        }
    }
}