//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Common.Controls;
using Ict.Common.IO;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Validation;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmMailingSetup
    {
        private bool FIsModal = false;
        private string FModalResult = string.Empty;

        /// <summary>
        /// Set this to true if the dialog is running modally and displaying Accept/Cancel buttons
        /// </summary>
        public bool RunAsModalDialog
        {
            set
            {
                FIsModal = value;

                if (FIsModal)
                {
                    grdDetails.DoubleClick += grdDetails_DoubleClick;
                }
                else
                {
                    pnlModalButtons.Visible = false;
                }
            }
        }

        /// <summary>
        /// Returns the selected Mailing Code if the Accept button was clicked - otherwise an empty string
        /// </summary>
        public string ModalDialogResult
        {
            get
            {
                return FModalResult;
            }
        }

        private void NewRowManual(ref PMailingRow ARow)
        {
            // Deal with the primary key - we need a unique mailing code
            string newName = Catalog.GetString("NEWVALUE");
            Int32 countNewDetail = 0;

            if (FMainDS.PMailing.Rows.Find(new object[] { newName }) != null)
            {
                while (FMainDS.PMailing.Rows.Find(new object[] { newName + countNewDetail.ToString() }) != null)
                {
                    countNewDetail++;
                }

                newName += countNewDetail.ToString();
            }

            ARow.MailingCode = newName;

            // Initialise the date to today
            ARow.MailingDate = DateTime.Today;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPMailing();
        }

        private void ValidateDataDetailsManual(PMailingRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedPartnerValidation_Partner.ValidateMailingSetup(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }

        private void RunOnceOnActivationManual()
        {
            // We need to set the number of decimal places for the Mailing Cost
            // We check all the ledgers to see which one has the highest number of decimal places.
            ALedgerTable ledgers = TRemote.MFinance.Setup.WebConnectors.GetAvailableLedgers();
            ACurrencyTable currencyTable = (ACurrencyTable)TDataCache.TMCommon.GetCacheableCommonTable(TCacheableCommonTablesEnum.CurrencyCodeList);
            int numDecimalPlaces = 0;

            if (ledgers.Count == 0)
            {
                txtDetailMailingCost.CurrencyCode = "USD";
            }
            else
            {
                for (int i = 0; i < ledgers.Count; i++)
                {
                    string code = ledgers[i].BaseCurrency;
                    ACurrencyRow row = (ACurrencyRow)currencyTable.Rows.Find(code);

                    if (row != null)
                    {
                        int decimals = StringHelper.DecimalPlacesForCurrency(code);

                        if (decimals > numDecimalPlaces)
                        {
                            numDecimalPlaces = decimals;
                        }
                    }
                }

                if (numDecimalPlaces == 0)
                {
                    txtDetailMailingCost.CurrencyCode = TTxtCurrencyTextBox.CURRENCY_STANDARD_0_DP;
                }
                else
                {
                    txtDetailMailingCost.CurrencyCode = TTxtCurrencyTextBox.CURRENCY_STANDARD_2_DP;
                }
            }

            FPetraUtilsObject.DataSaved += FPetraUtilsObject_DataSaved;
        }

        private void FPetraUtilsObject_DataSaved(object Sender, TDataSavedEventArgs e)
        {
            if (e.Success)
            {
                // We need to notify a listener such as the Form Letter dialog
                TFormsMessage broadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcMailingSetupSaved, this.ToString());
                TFormsList.GFormsList.BroadcastFormMessage(broadcastMessage);
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (!ValidateAllData(true, TErrorProcessingMode.Epm_All))
            {
                return;
            }

            if (!FPetraUtilsObject.IsDataSaved())
            {
                return;
            }

            if (FPreviouslySelectedDetailRow != null)
            {
                FModalResult = FPreviouslySelectedDetailRow.MailingCode;
            }

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void grdDetails_DoubleClick(object sender, EventArgs e)
        {
            btnAccept_Click(null, null);
        }

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TStandardFormPrint.PrintGrid(APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[] { 0, 1, 2, 3, 4 },
                new int[]
                {
                    PMailingTable.ColumnMailingCodeId,
                    PMailingTable.ColumnMailingDescriptionId,
                    PMailingTable.ColumnMailingDateId,
                    PMailingTable.ColumnMailingCostId,
                    PMailingTable.ColumnViewableId
                });
        }
    }
}