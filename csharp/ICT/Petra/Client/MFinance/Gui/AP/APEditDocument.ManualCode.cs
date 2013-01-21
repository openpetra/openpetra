//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
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
using System.Collections.Generic;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MFinance.Validation;

namespace Ict.Petra.Client.MFinance.Gui.AP
{
    public partial class TFrmAPEditDocument
    {
        Int32 FLedgerNumber;
        ALedgerRow FLedgerRow = null;

        /// <summary>
        /// When this document is saved in the database, I can check whether
        /// my calling form should be updated.
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void OnDataSaved(object Sender, TDataSavedEventArgs e)
        {
            if (e.Success)
            {
                if (FPetraUtilsObject.GetCallerForm().GetType() == typeof(TFrmAPSupplierTransactions))
                {
                    ((TFrmAPSupplierTransactions)FPetraUtilsObject.GetCallerForm()).Reload();
                }
            }
        }

        private void InitializeManualCode()
        {
            // When a doument is saved, I'll see about updating my caller.
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(OnDataSaved);
        }

        private void RunOnceOnActivationManual()
        {
            lblDiscountDays.Visible = false;
            nudDiscountDays.Visible = false;        // There's currently no discounting, so this
            lblDiscountPercentage.Visible = false;  // just hides the associated controls.
            txtDiscountPercentage.Visible = false;
            txtDetailAmount.TextChanged += new EventHandler(UpdateDetailBaseAmount);
            txtExchangeRateToBase.TextChanged += new EventHandler(UpdateDetailBaseAmount);
        }

        private void LookupExchangeRate(Object sender, EventArgs e)
        {
            decimal CurrentRate = TExchangeRateCache.GetDailyExchangeRate(txtSupplierCurrency.Text, FLedgerRow.BaseCurrency, DateTime.Now);

            txtExchangeRateToBase.NumberValueDecimal = CurrentRate;
        }

        private void EnableControls()
        {
            btnRemoveDetail.Enabled = (GetSelectedDetailRow() != null);

            // I need to make everything read-only if this document was already posted.
            if ("|POSTED|PARTPAID|PAID|".IndexOf("|" + FMainDS.AApDocument[0].DocumentStatus) >= 0)
            {
                tbbPostDocument.Enabled = false;

                txtSupplierName.Enabled = false;
                txtSupplierCurrency.Enabled = false;
                txtDocumentCode.Enabled = false;
                cmbDocumentType.Enabled = false;
                txtReference.Enabled = false;
                dtpDateIssued.Enabled = false;
                nudDiscountDays.Enabled = false;
                txtDiscountPercentage.Enabled = false;
                txtTotalAmount.Enabled = false;
                txtExchangeRateToBase.Enabled = false;

                btnAddDetail.Enabled = false;
                btnRemoveDetail.Enabled = false;
                btnLookupExchangeRate.Enabled = false;
                btnAnalysisAttributes.Enabled = false;

                txtDetailNarrative.Enabled = false;
                txtDetailItemRef.Enabled = false;
                txtDetailAmount.Enabled = false;
                cmbDetailCostCentreCode.Enabled = false;
                btnUseTaxAccount.Enabled = false;
                txtDetailBaseAmount.Enabled = false;
                cmbDetailAccountCode.Enabled = false;
            }

            tbbPostDocument.Enabled = ("|POSTED|PARTPAID|PAID".IndexOf("|" + FMainDS.AApDocument[0].DocumentStatus) < 0);
            tbbPayDocument.Enabled = ("|POSTED|PARTPAID".IndexOf("|" + FMainDS.AApDocument[0].DocumentStatus) >= 0);
        }

        private static bool DetailLineAttributesRequired(ref bool AllPresent, AccountsPayableTDS Atds, AApDocumentDetailRow DetailRow)
        {
            Atds.AAnalysisAttribute.DefaultView.RowFilter =
                String.Format("{0}='{1}'", AAnalysisAttributeTable.GetAccountCodeDBName(), DetailRow.AccountCode);         // Do I need Cost Centre in here too?

            if (Atds.AAnalysisAttribute.DefaultView.Count > 0)
            {
                bool IhaveAllMyAttributes = true;

                //
                // It's possible that my TDS doesn't even have an AnalAttrib table...

                if (Atds.AApAnalAttrib == null)
                {
                    Atds.Merge(new AApAnalAttribTable());
                }

                foreach (DataRowView rv in Atds.AAnalysisAttribute.DefaultView)
                {
                    AAnalysisAttributeRow AttrRow = (AAnalysisAttributeRow)rv.Row;

                    Atds.AApAnalAttrib.DefaultView.RowFilter =
                        String.Format("{0}={1} AND {2}='{3}'",
                            AApAnalAttribTable.GetDetailNumberDBName(), DetailRow.DetailNumber,
                            AApAnalAttribTable.GetAccountCodeDBName(), AttrRow.AccountCode);

                    if (Atds.AApAnalAttrib.DefaultView.Count == 0)
                    {
                        IhaveAllMyAttributes = false;
                        break;
                    }

                    foreach (DataRowView rv2 in Atds.AApAnalAttrib.DefaultView)
                    {
                        AApAnalAttribRow AttribValueRow = (AApAnalAttribRow)rv2.Row;

                        if (AttribValueRow.AnalysisAttributeValue == "")
                        {
                            IhaveAllMyAttributes = false;
                            break;
                        }
                    }

                    if (IhaveAllMyAttributes == false)  // because of the test above..
                    {
                        break;
                    }
                }

                if (IhaveAllMyAttributes)
                {
                    AllPresent = true;         // This detail line is fully specified
                }
                else
                {
                    AllPresent = false;         // This detail line requires attributes
                }

                return true;
            }
            else
            {
                AllPresent = true; // This detail line is fully specified
                return false;         // No attributes are required
            }
        }

        /// <summary>
        /// Called from cmbDetailAccountCode.SelectedValueChanged
        /// When an account is selected for a detail line,
        /// I need to determine whether analysis attributes are required for this account
        /// and if they are, whether I already have all the attributes required.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void CheckAccountRequiredAttr(Object sender, EventArgs e)
        {
            // I'm not doing this if this document was already posted.
            if ("|POSTED|PARTPAID|PAID|".IndexOf("|" + FMainDS.AApDocument[0].DocumentStatus) >= 0)
            {
                return;
            }

            string AccountCode = cmbDetailAccountCode.GetSelectedString();

            if (AccountCode.Length == 0)
            {
                return;
            }

            FPreviouslySelectedDetailRow.AccountCode = AccountCode;

            bool AllPresent = true;

            if (DetailLineAttributesRequired(ref AllPresent, FMainDS, FPreviouslySelectedDetailRow))
            {
                btnAnalysisAttributes.Enabled = true;

                if (AllPresent)
                {
                    btnAnalysisAttributes.ForeColor = System.Drawing.Color.Green;             // This detail line is fully specified
                }
                else
                {
                    btnAnalysisAttributes.ForeColor = System.Drawing.Color.Red;             // This detail line requires attributes
                }
            }
            else
            {
                btnAnalysisAttributes.Enabled = false;         // No attributes are required
            }
        }

        private void ShowDataManual()
        {
            AccountsPayableTDSAApDocumentRow DocumentRow = FMainDS.AApDocument[0];
            AApSupplierRow SupplierRow = FMainDS.AApSupplier[0];

            txtTotalAmount.CurrencySymbol = SupplierRow.CurrencyCode;
            txtDetailAmount.CurrencySymbol = SupplierRow.CurrencyCode;

            FLedgerNumber = DocumentRow.LedgerNumber;
            this.Text += " - " + TFinanceControls.GetLedgerNumberAndName(FLedgerNumber);

            ALedgerTable Tbl = TRemote.MFinance.AP.WebConnectors.GetLedgerInfo(FLedgerNumber);
            FLedgerRow = Tbl[0];
            txtDetailBaseAmount.CurrencySymbol = FLedgerRow.BaseCurrency;
            dtpDateDue.Date = DocumentRow.DateIssued.AddDays(Convert.ToDouble(nudCreditTerms.Value));

            if (FMainDS.AApDocumentDetail != null) // When the form is new, this can be null.
            {
                FMainDS.AApDocumentDetail.DefaultView.Sort = AApDocumentDetailTable.GetDetailNumberDBName();

                // Create Text description of Anal Attribs for each DetailRow..
                foreach (AccountsPayableTDSAApDocumentDetailRow DetailRow in FMainDS.AApDocumentDetail.Rows)
                {
                    string strAnalAttr = "";
                    FMainDS.AApAnalAttrib.DefaultView.RowFilter =
                        String.Format("{0}={1}", AApAnalAttribTable.GetDetailNumberDBName(), DetailRow.DetailNumber);

                    foreach (DataRowView rv in FMainDS.AApAnalAttrib.DefaultView)
                    {
                        AApAnalAttribRow Row = (AApAnalAttribRow)rv.Row;

                        if (strAnalAttr.Length > 0)
                        {
                            strAnalAttr += ", ";
                        }

                        strAnalAttr += (Row.AnalysisTypeCode + "=" + Row.AnalysisAttributeValue);
                    }

                    DetailRow.AnalAttr = strAnalAttr;
                }
            }

            EnableControls();
        }

        private void NewDetail(Object sender, EventArgs e)
        {
            // get the entered amounts, so that we can calculate the missing amount for the new detail
            GetDetailsFromControls(FPreviouslySelectedDetailRow);

            decimal DetailAmount = FMainDS.AApDocument[0].TotalAmount;

            if (FMainDS.AApDocumentDetail != null)
            {
                foreach (AApDocumentDetailRow detailRow in FMainDS.AApDocumentDetail.Rows)
                {
                    if (detailRow.RowState != DataRowState.Deleted)
                    {
                        DetailAmount -= detailRow.Amount;
                    }
                }
            }

            if (DetailAmount < 0)
            {
                DetailAmount = 0;
            }

            if (CreateAApDocumentDetail(
                    FMainDS.AApDocument[0].LedgerNumber,
                    FMainDS.AApDocument[0].ApDocumentId,
                    FMainDS.AApSupplier[0].DefaultExpAccount,
                    FMainDS.AApSupplier[0].DefaultCostCentre,
                    DetailAmount,
                    FMainDS.AApDocument[0].LastDetailNumber))
            {
                FMainDS.AApDocument[0].LastDetailNumber++;

                // for the moment, set all to approved, since we don't yet support approval of documents
                FMainDS.AApDocument[0].DocumentStatus = MFinanceConstants.AP_DOCUMENT_APPROVED;
                EnableControls();
                txtDetailNarrative.Focus();
            }
        }

        private void RemoveDetail(Object sender, EventArgs e)
        {
            AApDocumentDetailRow Row = GetSelectedDetailRow();

            if (Row == null)
            {
                return;
            }

            GetDataFromControls(FMainDS.AApDocument[0]);
            int rowIndex = grdDetails.Selection.GetSelectionRegion().GetRowsIndex()[0];
//          MessageBox.Show("Deleting "+ Row.Narrative, "Remove Row");

            Row.Delete();          // This row is not removed, but marked for deletion.

            // I have to prevent the auto-generated code from attempting to access this deleted row.
            grdDetails.Selection.SelectRow(rowIndex, true);
            FPreviouslySelectedDetailRow = GetSelectedDetailRow();

            if (FPreviouslySelectedDetailRow != null)
            {
                ShowDetails(FPreviouslySelectedDetailRow);
            }

            // Then I need to re-draw the grid, and enable controls as appropriate.
            grdDetails.Refresh();
            FPetraUtilsObject.SetChangedFlag();
            EnableControls();
        }

        /// <summary>
        /// Display the Analysis Attributes form for this selected detail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Analyse(Object sender, EventArgs e)
        {
            TFrmAPAnalysisAttributes AnalAttrForm = new TFrmAPAnalysisAttributes(this);

            GetDetailsFromControls(FPreviouslySelectedDetailRow);

            AnalAttrForm.Initialise(ref FMainDS, FPreviouslySelectedDetailRow);
            AnalAttrForm.ShowDialog();
            ShowData(FMainDS.AApDocument[0]);

//          CheckAccountRequiredAttr(null, null);
            if (AnalAttrForm.DetailsChanged)
            {
                FPetraUtilsObject.SetChangedFlag();
            }
        }

        private void UseTaxAccount(Object sender, EventArgs e)
        {
        }

        private void ValidateDataManual(AccountsPayableTDSAApDocumentRow ARow)
        {
            DataColumn ValidationColumn;

            ValidationColumn = ARow.Table.Columns[AccountsPayableTDSAApDocumentTable.ColumnDocumentCodeId];

            FPetraUtilsObject.VerificationResultCollection.AddOrRemove(
                TStringChecks.StringMustNotBeEmpty(ARow.DocumentCode,
                    lblDocumentCode.Text,
                    this, ValidationColumn, txtDocumentCode), ValidationColumn);
        }

        private void ValidateDataDetailsManual(AApDocumentDetailRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_AP.ValidateApDocumentDetailManual(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
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

        private void UpdateDetailBaseAmount(object sender, EventArgs e)
        {
            if ((txtExchangeRateToBase.NumberValueDecimal.HasValue)
                && (txtDetailAmount.NumberValueDecimal.HasValue))
            {
                decimal ExchangeRate = 1.0m;

                if (txtExchangeRateToBase.NumberValueDecimal.HasValue)
                {
                    ExchangeRate = txtExchangeRateToBase.NumberValueDecimal.Value;
                }

                if (ExchangeRate != 0)
                {
                    txtDetailBaseAmount.NumberValueDecimal =
                        txtDetailAmount.NumberValueDecimal / ExchangeRate;
                }
            }
        }

        /// initialise some comboboxes
        private void BeforeShowDetailsManual(AApDocumentDetailRow ARow)
        {
            grdDetails.Columns[1].Width = pnlDetailGrid.Width - 380;   // It doesn't really work having these here -
            grdDetails.Columns[0].Width = 90;                          // there's something else that overrides these settings.
            grdDetails.Columns[2].Width = 200;
            grdDetails.Columns[3].Width = 90;

            // if this form is readonly, then we need all account and cost centre codes, because old codes might have been used
            bool ActiveOnly = this.Enabled;

            TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, ARow.LedgerNumber, true, false, ActiveOnly, false);
            TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, ARow.LedgerNumber, true, false, ActiveOnly, false);
            EnableControls();

            Decimal ExchangeRateToBase = 0;

            if (txtExchangeRateToBase.NumberValueDecimal.HasValue)
            {
                ExchangeRateToBase = txtExchangeRateToBase.NumberValueDecimal.Value;
            }

            if (ARow.IsAmountNull() || (ExchangeRateToBase == 0))
            {
                txtDetailBaseAmount.NumberValueDecimal = null;
            }
            else
            {
                decimal DetailAmount = Convert.ToDecimal(ARow.Amount);
                DetailAmount /= ExchangeRateToBase;
                txtDetailBaseAmount.NumberValueDecimal = DetailAmount;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Atds"></param>
        /// <param name="AApDocument"></param>
        /// <returns>true if the document TotalAmount equals the sum of its parts!</returns>
        public static bool BatchBalancesOK(AccountsPayableTDS Atds, AApDocumentRow AApDocument)
        {
            decimal DocumentBalance = AApDocument.TotalAmount;

            foreach (AApDocumentDetailRow Row in Atds.AApDocumentDetail.Rows)
            {
                if (Row.ApDocumentId == AApDocument.ApDocumentId) // NOTE: When called from elsewhere, the TDS could contain data for several documents.
                {
                    DocumentBalance -= Row.Amount;
                }
            }

            if (DocumentBalance == 0.0m)
            {
                return true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(Catalog.GetString(
                        "The document Amount does not equal the sum of the detail lines."), Catalog.GetString("Balance Problem"));
                return false;
            }
        }

        /// <summary>
        /// Check that the cost centres referred to are OK with the accounts I'm using. If not a message is displayed.
        /// </summary>
        /// <param name="Atds"></param>
        /// <param name="AApDocument"></param>
        /// <returns>false if any detail lines have incompatible cost centres.</returns>
        public static bool AllLinesAccountsOK(AccountsPayableTDS Atds, AApDocumentRow AApDocument)
        {
            List <String>AccountCodesCostCentres = new List <string>();

            foreach (AApDocumentDetailRow Row in Atds.AApDocumentDetail.Rows)
            {
                if (Row.ApDocumentId == AApDocument.ApDocumentId)  // NOTE: When called from elsewhere, the TDS could contain data for several documents.
                {
                    if ((Row.AccountCode == "") || (Row.CostCentreCode == ""))
                    {
                        MessageBox.Show(
                            Catalog.GetString("Account and Cost Centre must be specified."),
                            Catalog.GetString("Post Document"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;
                    }

                    String AccountCostCentre = Row.AccountCode + "|" + Row.CostCentreCode;

                    if (!AccountCodesCostCentres.Contains(AccountCostCentre))
                    {
                        AccountCodesCostCentres.Add(AccountCostCentre);
                    }
                }
            }

            //
            // The check is done on the server..

            String ReportMsg = TRemote.MFinance.AP.WebConnectors.CheckAccountsAndCostCentres(AApDocument.LedgerNumber, AccountCodesCostCentres);

            if (ReportMsg != "")
            {
                MessageBox.Show(ReportMsg, Catalog.GetString("Invalid Account"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check the required analysis attributes for the detail lines in this invoice
        /// </summary>
        /// <param name="Atds"></param>
        /// <param name="AApDocument"></param>
        /// <returns>false if any lines don't have the analysis attributes they require</returns>
        public static bool AllLinesHaveAttributes(AccountsPayableTDS Atds, AApDocumentRow AApDocument)
        {
            foreach (AApDocumentDetailRow Row in Atds.AApDocumentDetail.Rows)
            {
                if (Row.ApDocumentId == AApDocument.ApDocumentId)  // NOTE: When called from elsewhere, the TDS could contain data for several documents.
                {
                    bool AllPresent = true;

                    if (DetailLineAttributesRequired(ref AllPresent, Atds, Row))
                    {
                        if (!AllPresent)
                        {
                            System.Windows.Forms.MessageBox.Show(
                                String.Format(Catalog.GetString("Analysis Attributes are required for account {0}."), Row.AccountCode),
                                Catalog.GetString("Analysis Attributes"));
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// This static function is called from several places
        /// </summary>
        /// <param name="Atds"></param>
        /// <param name="Adocument"></param>
        /// <returns>true if this document seems OK to post.</returns>
        public static bool ApDocumentCanPost(AccountsPayableTDS Atds, AApDocumentRow Adocument)
        {
            // If the batch will not balance, or required attributes are missing, I'll stop right here..

            if (!BatchBalancesOK(Atds, Adocument))
            {
                return false;
            }

            if (!AllLinesAccountsOK(Atds, Adocument))
            {
                return false;
            }

            if (!AllLinesHaveAttributes(Atds, Adocument))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Post a list of AP documents
        /// This static function is called from several places
        /// /// </summary>
        /// <returns>true if everything went OK</returns>
        public static bool PostApDocumentList(AccountsPayableTDS Atds, int ALedgerNumber, List <int>AApDocumentIds)
        {
            TVerificationResultCollection Verifications;

            TDlgGLEnterDateEffective dateEffectiveDialog = new TDlgGLEnterDateEffective(
                ALedgerNumber,
                Catalog.GetString("Select posting date"),
                Catalog.GetString("The date effective for posting") + ":");

            if (dateEffectiveDialog.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show(Catalog.GetString("Posting was cancelled."), Catalog.GetString(
                        "No Success"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            DateTime PostingDate = dateEffectiveDialog.SelectedDate;

            if (TRemote.MFinance.AP.WebConnectors.PostAPDocuments(
                    ALedgerNumber,
                    AApDocumentIds,
                    PostingDate,
                    false,
                    out Verifications))
            {
                return true;
            }
            else
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

            return false;
        }

        /// <summary>
        /// Post document as a GL Batch
        /// See very similar function in TFrmAPSupplierTransactions
        /// </summary>
        private void PostDocument(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Catalog.GetString("Document should be saved before posting."), Catalog.GetString(
                        "Post Document"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            GetDataFromControls(FMainDS.AApDocument[0]);

            // TODO: make sure that there are uptodate exchange rates

            if (!ApDocumentCanPost(FMainDS, FMainDS.AApDocument[0]))
            {
                return;
            }

            List <Int32>TaggedDocuments = new List <Int32>();

            TaggedDocuments.Add(FMainDS.AApDocument[0].ApDocumentId);

            if (PostApDocumentList(FMainDS, FMainDS.AApDocument[0].LedgerNumber, TaggedDocuments))
            {
                // TODO: print reports on successfully posted batch
                MessageBox.Show(Catalog.GetString("The AP document has been posted successfully!"));

                //
                // Refresh by re-loading the document from the server
                Int32 DocumentId = FMainDS.AApDocument[0].ApDocumentId;
                FMainDS.Clear();
                LoadAApDocument(FLedgerNumber, DocumentId);

                //
                // Also refresh the opener?
                Form Opener = FPetraUtilsObject.GetCallerForm();

                if (Opener.GetType() == typeof(TFrmAPSupplierTransactions))
                {
                    ((TFrmAPSupplierTransactions)Opener).Reload();
                }
            }
        }

        private void PayDocument(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Catalog.GetString("Document should be saved before paying."), Catalog.GetString(
                        "Pay Document"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            TFrmAPPayment PaymentScreen = new TFrmAPPayment(this);

            List <int>PayTheseDocs = new List <int>();

            PayTheseDocs.Add(FMainDS.AApDocument[0].ApDocumentId);
            PaymentScreen.AddDocumentsToPayment(FMainDS, FLedgerNumber, PayTheseDocs);
            PaymentScreen.Show();
        }
    }
}