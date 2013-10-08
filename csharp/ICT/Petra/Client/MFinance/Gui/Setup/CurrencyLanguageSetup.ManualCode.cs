//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       alanP
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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmCurrencyLanguageSetup
    {
        private void InitializeManualCode()
        {
            // The controls have some characters that are not valid as Xml element names
            // So we have to do additional initialization here.
            rbtWords.Text = "In Words";
            rbtNumeric.Text = "As a Number";
            rbtPerHundred.Text = "As a Number/100";
            rbtNone.Text = "Don't Show/None";

            // And because the text has changed we need to reposition each control
            rbtNumeric.Left += 20;
            rbtPerHundred.Left += 40;
            rbtNone.Left += 65;

            rbtWords.Width += 20;
            rbtNumeric.Width += 20;
            rbtPerHundred.Width += 35;
            rbtNone.Width += 60;

            rgrDetailDecimalOptions.Width += 150;

            // Finally we line up our labels precisely
            lblUnit.Left = txtDetailUnitLabelPlural.Left;
            lblDecimal.Left = txtDetailDecimalLabelPlural.Left;

            // Add event handlers for our radio buttons so we can display our print sample text
            rbtNone.CheckedChanged += new EventHandler(rbtNone_CheckedChanged);
            rbtNumeric.CheckedChanged += new EventHandler(rbtNumeric_CheckedChanged);
            rbtPerHundred.CheckedChanged += new EventHandler(rbtPerHundred_CheckedChanged);
            rbtWords.CheckedChanged += new EventHandler(rbtWords_CheckedChanged);

            txtDetailUnitLabelSingular.GotFocus += new EventHandler(txtCtrl_Enter);
            txtDetailUnitLabelPlural.GotFocus += new EventHandler(txtCtrl_Enter);
            txtDetailDecimalLabelSingular.GotFocus += new EventHandler(txtCtrl_Enter);
            txtDetailDecimalLabelPlural.GotFocus += new EventHandler(txtCtrl_Enter);

        }

        void txtCtrl_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void NewRowManual(ref ACurrencyLanguageRow ARow)
        {
            // Deal with primary key.  CurrencyCode (varchar(16)) and LanguageCode (varchar(20)) are unique.
            if (FMainDS.ACurrencyLanguage.Rows.Count == 0)
            {
                // Our first row is always USD/EN
                ARow.CurrencyCode = "USD";
                ARow.LanguageCode = "EN";
                return;
            }
            else
            {
                // We already have some rows, so we use the currently selected language in the comboBox as a starter
                // This may or may not be the most recently added language depending on how the main data set has been sorted
                // But once we have started adding rows it should remain the 'active' row
                string prevLanguage = cmbDetailLanguageCode.cmbCombobox.Text;

                // Try and find some popular or at least likely currencies
                // Remember that we will almost certainly select a currency that the user does NOT want in their language
                // So this will be the one we always propose!!!!
                string[] tryCurrencies =
                {
                    "USD", "GBP", "EUR", "INR", "AUD", "CAD", "CHF", "CNY", "JPY", "NZD", "PHP", "PKR", "ZAR"
                };
                int nTryCurrency = 0;

                bool bFoundCurrency = true;

                while (FMainDS.ACurrencyLanguage.Rows.Find(new object[] { tryCurrencies[nTryCurrency], prevLanguage }) != null)
                {
                    if (++nTryCurrency == tryCurrencies.Length)
                    {
                        // We have exhausted our popular choices
                        bFoundCurrency = false;
                        break;
                    }
                }

                if (bFoundCurrency)
                {
                    ARow.CurrencyCode = tryCurrencies[nTryCurrency];
                    ARow.LanguageCode = prevLanguage;
                    return;
                }

                // So we have tried all the popular currencies in the current language
                // Now we fall back to trying all currencies in the current languages.
                // Remember that we will almost certainly select a currency that the user does NOT want in their language
                // So this will be the one we always propose!!!!
                Type DataTableType;

                ACurrencyTable allCurrencies = new ACurrencyTable();
                DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("CurrencyCodeList", String.Empty, null, out DataTableType);
                allCurrencies.Merge(CacheDT);

                nTryCurrency = 0;
                bFoundCurrency = true;

                while (FMainDS.ACurrencyLanguage.Rows.Find(new object[] { allCurrencies.Rows[nTryCurrency][0].ToString(), prevLanguage }) != null)
                {
                    if (++nTryCurrency == tryCurrencies.Length)
                    {
                        bFoundCurrency = false;
                        break;
                    }
                }

                if (bFoundCurrency)
                {
                    ARow.CurrencyCode = allCurrencies.Rows[nTryCurrency][0].ToString();
                    ARow.LanguageCode = prevLanguage;
                }

                // We could at this point start trying other languages - but there seems little point since the currency list contains
                // currencies that will never be used since they no longer exist!!  It is therefore assumed that by this time
                // the user has gone on to a different langauge where we will have started again with USD ....
            }
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewACurrencyLanguage();
        }

        private void GetDetailDataFromControlsManual(ACurrencyLanguageRow ARow)
        {
            // Set the actual values that will go in the database column for DecimalOptions
            if (rbtWords.Checked)
            {
                ARow.DecimalOptions = "Words";
            }

            if (rbtNumeric.Checked)
            {
                ARow.DecimalOptions = "Numeric";
            }

            if (rbtPerHundred.Checked)
            {
                ARow.DecimalOptions = "PerHundred";
            }

            if (rbtNone.Checked)
            {
                ARow.DecimalOptions = "None";
            }
        }

        private void ShowDetailsManual(ACurrencyLanguageRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            // deal with our own conversion.  Words is the default so must be last in the 'if' sequence
            if (String.Compare(ARow.DecimalOptions, "None", true) == 0)
            {
                rbtNone.Checked = true;
            }
            else if (String.Compare(ARow.DecimalOptions, "Numeric", true) == 0)
            {
                rbtNumeric.Checked = true;
            }
            else if (String.Compare(ARow.DecimalOptions, "PerHundred", true) == 0)
            {
                rbtPerHundred.Checked = true;
            }
            else
            {
                rbtWords.Checked = true;
            }
        }

        // TODO: When we have the actual printing routine we should use that to generate the example text
        //       in place of the fixed text below because that is the definitive sample text ...
        //  This will be the c# translation of the 4GL method x_curamt.p
        private void rbtNone_CheckedChanged(object Sender, EventArgs e)
        {
            lblPrintSample.Text = "e.g. 2105 dollars";
            SetDecimalEnabledState(false);
        }

        private void rbtWords_CheckedChanged(object Sender, EventArgs e)
        {
            lblPrintSample.Text = "e.g. two thousand one hundred five dollars and one cent";
            SetDecimalEnabledState(true);
        }

        private void rbtNumeric_CheckedChanged(object Sender, EventArgs e)
        {
            lblPrintSample.Text = "e.g. 2105 dollars 01 cent";
            SetDecimalEnabledState(true);
        }

        private void rbtPerHundred_CheckedChanged(object Sender, EventArgs e)
        {
            lblPrintSample.Text = "e.g. 2105 dollars 01/100 cent";
            SetDecimalEnabledState(true);
        }

        private void SetDecimalEnabledState(bool bEnable)
        {
            txtDetailDecimalLabelPlural.Enabled = bEnable;
            txtDetailDecimalLabelSingular.Enabled = bEnable;

            if (!bEnable)
            {
                txtDetailDecimalLabelPlural.Text = String.Empty;
                txtDetailDecimalLabelSingular.Text = String.Empty;
            }
        }
    }
}