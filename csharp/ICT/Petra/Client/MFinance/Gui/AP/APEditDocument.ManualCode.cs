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
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.AP
{
    public partial class TFrmAPEditDocument
    {
        private void NewDetail(Object sender, EventArgs e)
        {
            // get the entered amounts, so that we can calculate the missing amount for the new detail
            GetDetailsFromControls(FPreviouslySelectedDetailRow);

            decimal DetailAmount = FMainDS.AApDocument[0].TotalAmount;

            if (FMainDS.AApDocumentDetail != null)
            {
                foreach (AApDocumentDetailRow detailRow in FMainDS.AApDocumentDetail.Rows)
                {
                    DetailAmount -= detailRow.Amount;
                }
            }

            if (DetailAmount < 0)
            {
                DetailAmount = 0;
            }

            CreateAApDocumentDetail(
                FMainDS.AApDocument[0].LedgerNumber,
                FMainDS.AApDocument[0].ApNumber,
                FMainDS.AApSupplier[0].DefaultExpAccount,
                FMainDS.AApSupplier[0].DefaultCostCentre,
                DetailAmount,
                FMainDS.AApDocument[0].LastDetailNumber);
            FMainDS.AApDocument[0].LastDetailNumber++;

            // for the moment, set all to approved, since we don't support approval of documents at the moment
            FMainDS.AApDocument[0].DocumentStatus = MFinanceConstants.AP_DOCUMENT_APPROVED;
        }

        private void ValidateDataManual(AApDocumentDetailRow ARow)
        {
            DataColumn ValidationColumn;

//TLogging.Log("ValidateDataManual:  CountryCode = " + ARow[PCountryTable.ColumnCountryCodeId].ToString() +
//                         "; InternatTelephoneCode = " + ARow.InternatTelephoneCode.ToString());
            // 'International Telephone Code' must be positive or 0
            ValidationColumn = ARow.Table.Columns[AApDocumentDetailTable.ColumnAmountId];

            FPetraUtilsObject.VerificationResultCollection.AddOrRemove(
                TNumericalChecks.IsPositiveOrZeroDecimal(ARow.Amount,
                    lblDetailAmount.Text,
                    this, ValidationColumn, txtDetailAmount), ValidationColumn);

//            FPetraUtilsObject.VerificationResultCollection.AddOrRemove(
//                TNumericalChecks.IsPositiveOrZeroInteger(txtDetailInternatTelephoneCode.NumberValueInt,
//                    lblDetailInternatTelephoneCode.Text,
//                    this, ValidationColumn, txtDetailInternatTelephoneCode), ValidationColumn);

//            // 'Time Zone From' must be <= 'Time Zone To'
//            ValidationColumn = ARow.Table.Columns[PCountryTable.ColumnTimeZoneMinimumId];
//            FPetraUtilsObject.VerificationResultCollection.AddOrRemove(
//                TNumericalChecks.FirstLesserOrEqualThanSecondDecimal(
//                    ARow.TimeZoneMinimum, ARow.TimeZoneMaximum,
//                    lblDetailTimeZoneMinimum.Text, lblDetailTimeZoneMaximum.Text,
//                    this, ValidationColumn, txtDetailTimeZoneMinimum), ValidationColumn);

//            FPetraUtilsObject.VerificationResultCollection.Add(new TScreenVerificationResult( "TestContext", ValidationColumn, "test warning", txtDetailTimeZoneMinimum, TResultSeverity.Resv_Noncritical));
        }

        private void UpdateCreditTerms(object sender, TPetraDateChangedEventArgs e)
        {
            if (sender == dtpDateDue)
            {
                if ((dtpDateDue.Date.HasValue)
                    && (dtpDateIssued.Date.HasValue))
                {
                    int diffDays = (dtpDateDue.Date.Value - dtpDateIssued.Date.Value).Days;

                    if (diffDays < 0)
                    {
                        diffDays = 0;
                        dtpDateDue.Date = dtpDateIssued.Date.Value;
                    }

                    nudCreditTerms.Value = diffDays;
                }
            }
            else if ((sender == dtpDateIssued) || (sender == nudCreditTerms))
            {
                if ((dtpDateIssued.Date.HasValue))
                {
                    dtpDateDue.Date = dtpDateIssued.Date.Value.AddDays((double)nudCreditTerms.Value);
                }
            }
        }

        private void UpdateCreditTermsOverload(object sender, EventArgs e)
        {
            UpdateCreditTerms(sender, null);
        }

        /// initialise some comboboxes
        private void BeforeShowDetailsManual(AApDocumentDetailRow ARow)
        {
            // if this form is readonly, then we need all account and cost centre codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;

            TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, ARow.LedgerNumber, true, false, ActiveOnly, false);
            TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, ARow.LedgerNumber, true, false, ActiveOnly, false);
        }

        /// <summary>
        /// Post document as a GL Batch
        /// see very similar function in TFrmAPSupplierTransactions
        /// </summary>
        private void PostDocument(object sender, EventArgs e)
        {
            List <Int32>TaggedDocuments = new List <Int32>();

            TaggedDocuments.Add(FMainDS.AApDocument[0].ApNumber);

            if (TaggedDocuments.Count == 0)
            {
                return;
            }

            // TODO: make sure that there are uptodate exchange rates

            TVerificationResultCollection Verifications;

            TDlgGLEnterDateEffective dateEffectiveDialog = new TDlgGLEnterDateEffective(
                FMainDS.AApDocument[0].LedgerNumber,
                Catalog.GetString("Select posting date"),
                Catalog.GetString("The date effective for posting") + ":");

            if (dateEffectiveDialog.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show(Catalog.GetString("The payment was cancelled."), Catalog.GetString(
                        "No Success"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime PostingDate = dateEffectiveDialog.SelectedDate;

            if (!TRemote.MFinance.AP.WebConnectors.PostAPDocuments(
                    FMainDS.AApDocument[0].LedgerNumber,
                    TaggedDocuments,
                    PostingDate,
                    out Verifications))
            {
                string ErrorMessages = String.Empty;

                foreach (TVerificationResult verif in Verifications)
                {
                    ErrorMessages += "[" + verif.ResultContext + "] " +
                                     verif.ResultTextCaption + ": " +
                                     verif.ResultText + Environment.NewLine;
                }

                System.Windows.Forms.MessageBox.Show(ErrorMessages, Catalog.GetString("Posting failed"));
            }
            else
            {
                // TODO: print reports on successfully posted batch
                MessageBox.Show(Catalog.GetString("The AP document has been posted successfully!"));

                // TODO: show posting register of GL Batch?

                // TODO: refresh the screen, to reflect that the transactions have been posted
                // TODO: refresh/notify other screens as well?
            }
        }
    }
}