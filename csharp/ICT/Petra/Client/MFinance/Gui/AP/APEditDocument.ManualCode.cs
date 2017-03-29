//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//
// Copyright 2004-2016 by OM International
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
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.MCommon.Gui;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Petra.Client.MPartner.Gui;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using SourceGrid;
using Ict.Petra.Client.MReporting.Gui.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.AP
{
    public partial class TFrmAPEditDocument
    {
        Int32 FDocumentLedgerNumber;
        ALedgerRow FLedgerRow = null;
        SourceGrid.Cells.Editors.ComboBox cmbAnalAttribValues;
        AApAnalAttribRow FPSAttributesRow;
        Boolean FRequireApprovalBeforePosting;
        List <TFormData>FFormDataList = null;

        /// <summary>
        /// Before saving the document, I'll remove any ApAnalAttrib rows that have no values set
        /// (These may have been created by the overly keen method below and are not required at all,
        /// or perhaps they are required but the user has not yet provided values. )
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        void BeforeDataSave(object Sender, EventArgs e)
        {
            for (Int32 RowNum = FMainDS.AApAnalAttrib.Rows.Count; RowNum > 0; RowNum--)
            {
                AApAnalAttribRow Row = (AApAnalAttribRow)FMainDS.AApAnalAttrib.Rows[RowNum - 1];

                if ((Row.RowState != DataRowState.Deleted) && (Row.AnalysisAttributeValue == ""))  // I need to delete empty rows
                {
                    Row.Delete();
                }
            }

            ApDocumentCanPost(FMainDS, FMainDS.AApDocument[0], true); // Various warnings may be generated, but the save still goes ahead.
        }

        private void DataSavingValidated(object sender, CancelEventArgs e)
        {
            if (FMainDS.AApDocument[0].DocumentStatus == MFinanceConstants.AP_DOCUMENT_APPROVED)
            {
                string msg = Catalog.GetString(
                    "You are about to save changes to an 'Approved' document.  When the document has been saved the status will revert to 'Open' and the document will need to be approved again.");
                msg += Environment.NewLine + Environment.NewLine;
                msg += Catalog.GetString("Proceed to save the changes?");

                if (MessageBox.Show(msg, Catalog.GetString("Save Changes"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    FMainDS.AApDocument[0].DocumentStatus = MFinanceConstants.AP_DOCUMENT_OPEN;
                }
            }
        }

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
                TFormsMessage broadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcAPTransactionChanged);
                broadcastMessage.SetMessageDataAPTransaction(txtSupplierName.Text);
                TFormsList.GFormsList.BroadcastFormMessage(broadcastMessage);
            }
        }

        private void InitializeManualCode()
        {
            // When a doument is saved, I'll see about updating my caller.
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(BeforeDataSave);
            FPetraUtilsObject.DataSavingValidated += new TDataSavingValidatedHandler(DataSavingValidated);
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(OnDataSaved);

            btnHint.Height = txtDocumentStatus.Height;
        }

        // When the user enters a total amount for the invoice,
        // I'll copy it into the detail line,
        // IFF there's only one detail, and no value has yet been entered.
        private void InitialiseDetailAmount(object sender, EventArgs e)
        {
            decimal? invoiceTotal = txtTotalAmount.NumberValueDecimal;

            if (!invoiceTotal.HasValue)
            {
                return;
            }

            if (FMainDS.AApDocumentDetail.DefaultView.Count == 1)
            {
                decimal? detailAmount = txtDetailAmount.NumberValueDecimal;

                if (!detailAmount.HasValue || (detailAmount.Value == 0))
                {
                    txtDetailAmount.NumberValueDecimal = invoiceTotal.Value;
                }
            }
        }

        private void RunOnceOnActivationManual()
        {
            lblDiscountDays.Visible = false;
            nudDiscountDays.Visible = false;        // There's currently no discounting, so this
            lblDiscountPercentage.Visible = false;  // just hides the associated controls.
            txtDiscountPercentage.Visible = false;
            txtDetailAmount.Leave += new EventHandler(UpdateDetailBaseAmount);
            txtExchangeRateToBase.Leave += new EventHandler(UpdateDetailBaseAmount);
            txtTotalAmount.Leave += InitialiseDetailAmount;

            UpdateRecordNumberDisplay();

            // If this is a new invoice, move the focus so the user can begin entering it!
            // (Immediately after this method, the grid will get the focus.)
            if (txtDocumentCode.Text == "")
            {
                System.Windows.Forms.Timer focusCorrectionTimer = new System.Windows.Forms.Timer();
                focusCorrectionTimer.Interval = 10;
                focusCorrectionTimer.Tick += new EventHandler(TimerDrivenFocusCorrection);
                focusCorrectionTimer.Start();
            }

            nudCreditTerms.Minimum = 0;
            nudCreditTerms.Maximum = 4000;
        }

        private void TimerDrivenFocusCorrection(Object Sender, EventArgs e)
        {
            System.Windows.Forms.Timer SendingTimer = Sender as System.Windows.Forms.Timer;

            if (SendingTimer != null)
            {
                SendingTimer.Stop();
            }

            txtDocumentCode.Focus();
        }

        private void AnalysisAttributesGrid_RowSelected(System.Object sender, RangeRegionChangedEventArgs e)
        {
            if (grdAnalAttributes.Selection.ActivePosition.IsEmpty() || (grdAnalAttributes.Selection.ActivePosition.Column == 0))
            {
                return;
            }

            if ((GetSelectedAttributeRow() == null) || (FPSAttributesRow == GetSelectedAttributeRow()))
            {
                return;
            }

            FPSAttributesRow = GetSelectedAttributeRow();

            FMainDS.AFreeformAnalysis.DefaultView.RowFilter = String.Format("{0}='{1}' AND {2}=true",
                AFreeformAnalysisTable.GetAnalysisTypeCodeDBName(),
                FPSAttributesRow.AnalysisTypeCode,
                AFreeformAnalysisTable.GetActiveDBName());

            //Refresh the combo values
            int analTypeCodeValuesCount = FMainDS.AFreeformAnalysis.DefaultView.Count;

            if (analTypeCodeValuesCount == 0)
            {
                MessageBox.Show(Catalog.GetString(
                        "No attribute values are defined!"), FPSAttributesRow.AnalysisTypeCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            string[] analTypeValues = new string[analTypeCodeValuesCount];

            FMainDS.AFreeformAnalysis.DefaultView.Sort = AFreeformAnalysisTable.GetAnalysisValueDBName();
            int counter = 0;

            foreach (DataRowView dvr in FMainDS.AFreeformAnalysis.DefaultView)
            {
                AFreeformAnalysisRow faRow = (AFreeformAnalysisRow)dvr.Row;
                analTypeValues[counter++] = faRow.AnalysisValue;
            }

            cmbAnalAttribValues.StandardValuesExclusive = true;
            cmbAnalAttribValues.StandardValues = analTypeValues;
        }

        void AnalysisAttributeValueChanged(object sender, EventArgs e)
        {
            if ("|CANCELLED|POSTED|PARTPAID|PAID|".IndexOf("|" + FMainDS.AApDocument[0].DocumentStatus) >= 0)
            {
                return;
            }

            DevAge.Windows.Forms.DevAgeComboBox valueType = (DevAge.Windows.Forms.DevAgeComboBox)sender;

            int selectedValueIndex = valueType.SelectedIndex;

            if (selectedValueIndex < 0)
            {
                return;
            }
            else if (valueType.Items[selectedValueIndex].ToString() != FPSAttributesRow.AnalysisAttributeValue)
            {
                FPetraUtilsObject.SetChangedFlag();
                FPSAttributesRow.AnalysisAttributeValue = valueType.Items[selectedValueIndex].ToString();
                ValidateAllData(false, TErrorProcessingMode.Epm_All);
            }
        }

        private void LookupExchangeRate(Object sender, EventArgs e)
        {
            TFrmSetupDailyExchangeRate setupDailyExchangeRate =
                new TFrmSetupDailyExchangeRate(FPetraUtilsObject.GetForm());

            decimal selectedExchangeRate;
            DateTime selectedEffectiveDate;
            int selectedEffectiveTime;

            if (setupDailyExchangeRate.ShowDialog(
                    FDocumentLedgerNumber,
                    DateTime.Now,
                    txtSupplierCurrency.Text,
                    1.0m,
                    out selectedExchangeRate,
                    out selectedEffectiveDate,
                    out selectedEffectiveTime) == DialogResult.Cancel)
            {
                return;
            }

            if (txtExchangeRateToBase.NumberValueDecimal != selectedExchangeRate)
            {
                //Enforce save needed condition
                FPetraUtilsObject.SetChangedFlag();
            }

            txtExchangeRateToBase.NumberValueDecimal = selectedExchangeRate;
        }

        private void EnableControls()
        {
            btnDelete.Enabled = (GetSelectedDetailRow() != null);

            // I need to make everything read-only if this document was already posted or cancelled.
            if ("|CANCELLED|POSTED|PARTPAID|PAID|".IndexOf("|" + FMainDS.AApDocument[0].DocumentStatus) >= 0)
            {
                tbbApproveDocument.Enabled = false;
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
                btnDelete.Enabled = false;
                btnLookupExchangeRate.Enabled = false;

                txtDetailNarrative.Enabled = false;
                txtDetailItemRef.Enabled = false;
                txtDetailAmount.Enabled = false;
                cmbDetailCostCentreCode.Enabled = false;
                txtDetailBaseAmount.Enabled = false;
                cmbDetailAccountCode.Enabled = false;
                pnlDetails.Enabled = false;
            }

            tbbPostDocument.Enabled = ("|CANCELLED|POSTED|PARTPAID|PAID".IndexOf("|" + FMainDS.AApDocument[0].DocumentStatus) < 0);
            tbbPayDocument.Enabled = ("|POSTED|PARTPAID".IndexOf("|" + FMainDS.AApDocument[0].DocumentStatus) >= 0);
            tbbReprintRemittanceAdvice.Enabled = ("|PARTPAID|PAID".IndexOf("|" + FMainDS.AApDocument[0].DocumentStatus) >= 0);
            btnHint.Enabled = ("|PARTPAID|PAID".IndexOf("|" + FMainDS.AApDocument[0].DocumentStatus) >= 0);

            if (FRequireApprovalBeforePosting)
            {
                tbbApproveDocument.Visible = true;

                if (FMainDS.AApDocument[0].DocumentStatus == MFinanceConstants.AP_DOCUMENT_OPEN)
                {
                    tbbPostDocument.Enabled = false;
                    tbbPayDocument.Enabled = false;
                    tbbApproveDocument.Enabled = true;
                }
                else
                {
                    tbbApproveDocument.Enabled = false;
                }
            }
            else
            {
                tbbApproveDocument.Visible = false;
            }
        }

        private static bool DetailLineAttributesRequired(ref bool AllPresent, AccountsPayableTDS Atds, AApDocumentDetailRow DetailRow)
        {
            Atds.AAnalysisAttribute.DefaultView.RowFilter =
                String.Format("{0}='{1}'", AAnalysisAttributeTable.GetAccountCodeDBName(), DetailRow.AccountCode);

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

                        // Is the referenced AttribValue active?
                        AFreeformAnalysisRow referencedRow = (AFreeformAnalysisRow)Atds.AFreeformAnalysis.Rows.Find(
                            new Object[] { AttribValueRow.LedgerNumber, AttribValueRow.AnalysisTypeCode, AttribValueRow.AnalysisAttributeValue }
                            );

                        if ((referencedRow == null) || !referencedRow.Active)
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

                AllPresent = IhaveAllMyAttributes;
                return true;
            }
            else
            {
                AllPresent = true; // This detail line is fully specified
                return false;      // No attributes are required
            }
        }

        private void UpdateAttributeLabel(AccountsPayableTDSAApDocumentDetailRow DetailRow)
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

        private void ShowDataManual()
        {
            AccountsPayableTDSAApDocumentRow DocumentRow = FMainDS.AApDocument[0];

            FDocumentLedgerNumber = DocumentRow.LedgerNumber;

            // This will involve a trip to the server to access GLSetupTDS
            TFrmLedgerSettingsDialog settings = new TFrmLedgerSettingsDialog(this, FDocumentLedgerNumber);
            FRequireApprovalBeforePosting = settings.APRequiresApprovalBeforePosting;

            txtTotalAmount.CurrencyCode = DocumentRow.CurrencyCode;
            txtDetailAmount.CurrencyCode = DocumentRow.CurrencyCode;
            dtpDateDue.Date = DocumentRow.DateIssued.AddDays(Convert.ToDouble(nudCreditTerms.Value));

            this.Text = "AP Document Edit - " + TFinanceControls.GetLedgerNumberAndName(FDocumentLedgerNumber);

            FLedgerRow =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FDocumentLedgerNumber))[0];
            txtDetailBaseAmount.CurrencyCode = FLedgerRow.BaseCurrency;
            //txtExchangeRateToBase.SetControlProperties(10);

            //
            // If this document's currency is that of my own ledger,
            // I need to disable the rate of exchange field.
            if (DocumentRow.CurrencyCode == FLedgerRow.BaseCurrency)
            {
                txtExchangeRateToBase.Enabled = false;
                btnLookupExchangeRate.Enabled = false;
            }

            if ((FMainDS.AApDocumentDetail == null) || (FMainDS.AApDocumentDetail.Rows.Count == 0)) // When the document is new, I need to create the first detail line.
            {
                NewDetail(null, null);
            }

            FMainDS.AApDocumentDetail.DefaultView.Sort = AApDocumentDetailTable.GetDetailNumberDBName();

            // Create Text description of Anal Attribs for each DetailRow..
            foreach (AccountsPayableTDSAApDocumentDetailRow DetailRow in FMainDS.AApDocumentDetail.Rows)
            {
                UpdateAttributeLabel(DetailRow);
            }

            EnableControls();
        }

        private void GetDetailDataFromControlsManual(AccountsPayableTDSAApDocumentDetailRow ARow)
        {
            if (ARow == null)
            {
                return;
            }

            UpdateAttributeLabel(ARow);
        }

        private void NewDetail(Object sender, EventArgs e)
        {
            // do validation which will get the entered amounts, so that we can calculate the missing amount for the new detail
            if (ValidateAllData(true, TErrorProcessingMode.Epm_All))
            {
                decimal DetailAmount = FMainDS.AApDocument[0].TotalAmount;

                if (FMainDS.AApDocumentDetail != null)
                {
                    foreach (AApDocumentDetailRow detailRow in FMainDS.AApDocumentDetail.Rows)
                    {
                        if (detailRow.RowState != DataRowState.Deleted)
                        {
                            DetailAmount -= detailRow.IsAmountNull() ? 0 : detailRow.Amount;
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

                    // set the document status to open
                    FMainDS.AApDocument[0].DocumentStatus = MFinanceConstants.AP_DOCUMENT_OPEN;
                    EnableControls();
                    txtDetailNarrative.Focus();
                }
            }
        }

        /// <summary>
        /// Performs checks to determine whether a deletion of the current
        /// row is permissable
        /// </summary>
        /// <param name="ARowToDelete">the currently selected row to be deleted</param>
        /// <param name="ADeletionQuestion">can be changed to a context-sensitive deletion confirmation question</param>
        /// <returns>true if user is permitted and able to delete the current row</returns>
        private bool PreDeleteManual(AApDocumentDetailRow ARowToDelete, ref string ADeletionQuestion)
        {
            GetDataFromControls(FMainDS.AApDocument[0]);
            return true;
        }

        private void PostDeleteManual(AApDocumentDetailRow ARowToDelete,
            bool AAllowDeletion,
            bool ADeletionPerformed,
            string ACompletionMessage)
        {
            if (ADeletionPerformed)
            {
                EnableControls();
            }
        }

        private void ValidateDataManual(AccountsPayableTDSAApDocumentRow ARow)
        {
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
            if (ARow == null)
            {
                return;
            }

            // if this document was already posted, then we need all account and cost centre codes, because old codes might have been used
            bool ActiveOnly = ("|CANCELLED|POSTED|PARTPAID|PAID|".IndexOf("|" + FMainDS.AApDocument[0].DocumentStatus) < 0);

            FPetraUtilsObject.SuppressChangeDetection = true;
            TFinanceControls.InitialiseAccountList(ref cmbDetailAccountCode, ARow.LedgerNumber, true, false, ActiveOnly, false);
            TFinanceControls.InitialiseCostCentreList(ref cmbDetailCostCentreCode, ARow.LedgerNumber, true, false, ActiveOnly, false);
            FPetraUtilsObject.SuppressChangeDetection = false;
            EnableControls();

            Decimal ExchangeRateToBase = 0;

            if (txtExchangeRateToBase.NumberValueDecimal.HasValue)
            {
                ExchangeRateToBase = txtExchangeRateToBase.NumberValueDecimal.Value;
            }

            if (ARow.IsAmountNull() || (ExchangeRateToBase == 0))
            {
                if (txtDetailBaseAmount.NumberValueDecimal != null)
                {
                    txtDetailBaseAmount.NumberValueDecimal = null;
                }
            }
            else
            {
                decimal DetailAmount = Convert.ToDecimal(ARow.Amount);
                DetailAmount /= ExchangeRateToBase;

                if (txtDetailBaseAmount.NumberValueDecimal != DetailAmount)
                {
                    txtDetailBaseAmount.NumberValueDecimal = DetailAmount;
                }
            }
        }

        //
        // Called from cmbDetailAccountCode.SelectedValueChanged,
        // I need to load the Analysis Types Pane with the required attributes for this account,
        // and show any assignments already made.

        void ShowAnalysisAttributesForAccount(object sender, EventArgs e)
        {
            //
            // It's possible that my TDS doesn't even have an AnalAttrib table...

            if (FMainDS.AApAnalAttrib == null)
            {
                FMainDS.Merge(new AApAnalAttribTable());
            }

            if (FPetraUtilsObject.SuppressChangeDetection || (FPreviouslySelectedDetailRow == null))
            {
                return;
            }

            //Empty the grid
            FMainDS.AApAnalAttrib.DefaultView.RowFilter = "1=2";
            FPSAttributesRow = null;

            if (grdAnalAttributes.Columns.Count < 2) // This is initialisation but I moved it here because sometimes this is called before Show().
            {
                grdAnalAttributes.SpecialKeys = GridSpecialKeys.Default | GridSpecialKeys.Tab;

                cmbAnalAttribValues = new SourceGrid.Cells.Editors.ComboBox(typeof(string));
                cmbAnalAttribValues.Control.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbAnalAttribValues.EnableEdit = true;
                cmbAnalAttribValues.EditableMode = EditableMode.Focus;
                cmbAnalAttribValues.Control.SelectedValueChanged += new EventHandler(AnalysisAttributeValueChanged);
                grdAnalAttributes.AddTextColumn("Value",
                    FMainDS.AApAnalAttrib.Columns[AApAnalAttribTable.GetAnalysisAttributeValueDBName()], 120,
                    cmbAnalAttribValues);

                grdAnalAttributes.Selection.SelectionChanged += new RangeRegionChangedEventHandler(AnalysisAttributesGrid_RowSelected);
            }

            grdAnalAttributes.Columns[0].Width = 90; // for some unknown reason, this doesn't work.
            grdAnalAttributes.Columns[1].Width = 120;

            AccountsPayableTDSAApDocumentDetailRow DetailRow = GetSelectedDetailRow();
            DetailRow.AccountCode = cmbDetailAccountCode.GetSelectedString();

            //
            // So I want to remove any attributes attached to this row that don't have the new account code...
            FMainDS.AApAnalAttrib.DefaultView.RowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5} AND {6}<>'{7}'",
                AApAnalAttribTable.GetLedgerNumberDBName(), DetailRow.LedgerNumber,
                AApAnalAttribTable.GetApDocumentIdDBName(), DetailRow.ApDocumentId,
                AApAnalAttribTable.GetDetailNumberDBName(), DetailRow.DetailNumber,
                AApAnalAttribTable.GetAccountCodeDBName(), DetailRow.AccountCode);

            for (Int32 RowIdx = FMainDS.AApAnalAttrib.DefaultView.Count; RowIdx > 0; RowIdx--)
            {
                FMainDS.AApAnalAttrib.DefaultView[RowIdx - 1].Row.Delete();
            }

            FMainDS.AAnalysisAttribute.DefaultView.RowFilter =
                String.Format("{0}='{1}'", AAnalysisAttributeTable.GetAccountCodeDBName(), DetailRow.AccountCode);

            String AccountCodeRowFilter = String.Format("{0}={1} AND {2}={3} AND {4}={5} AND {6}='{7}'",
                AApAnalAttribTable.GetLedgerNumberDBName(), DetailRow.LedgerNumber,
                AApAnalAttribTable.GetApDocumentIdDBName(), DetailRow.ApDocumentId,
                AApAnalAttribTable.GetDetailNumberDBName(), DetailRow.DetailNumber,
                AApAnalAttribTable.GetAccountCodeDBName(), DetailRow.AccountCode);

            foreach (DataRowView rv in FMainDS.AAnalysisAttribute.DefaultView) // List of attributes required for this account
            {
                AAnalysisAttributeRow AttrRow = (AAnalysisAttributeRow)rv.Row;

                FMainDS.AApAnalAttrib.DefaultView.RowFilter = AccountCodeRowFilter +
                                                              String.Format(" AND {0}='{1}'",
                    AApAnalAttribTable.GetAnalysisTypeCodeDBName(), AttrRow.AnalysisTypeCode);

                if (FMainDS.AApAnalAttrib.DefaultView.Count == 0)   // No Entry yet for this attribute. This is likely, given I just deleted everything...
                {
                    AApAnalAttribRow AARow = FMainDS.AApAnalAttrib.NewRowTyped();
                    AARow.LedgerNumber = DetailRow.LedgerNumber;
                    AARow.ApDocumentId = DetailRow.ApDocumentId;
                    AARow.DetailNumber = DetailRow.DetailNumber;
                    AARow.AnalysisTypeCode = AttrRow.AnalysisTypeCode;
                    AARow.AccountCode = DetailRow.AccountCode;
                    FMainDS.AApAnalAttrib.Rows.Add(AARow);
                }
            }

            FMainDS.AApAnalAttrib.DefaultView.RowFilter = AccountCodeRowFilter;
            FMainDS.AApAnalAttrib.DefaultView.Sort = AApAnalAttribTable.GetAnalysisTypeCodeDBName();

            grdAnalAttributes.DataSource = null;
            grdAnalAttributes.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AApAnalAttrib.DefaultView);
            UpdateAttributeLabel(DetailRow);

            if (grdAnalAttributes.Rows.Count > 2)
            {
                grdAnalAttributes.Enabled = true;
                grdAnalAttributes.SelectRowWithoutFocus(1);
                AnalysisAttributesGrid_RowSelected(null, null);
            }
            else
            {
                grdAnalAttributes.Enabled = false;
            }
        }

        private AApAnalAttribRow GetSelectedAttributeRow()
        {
            DataRowView[] SelectedGridRow = grdAnalAttributes.SelectedDataRowsAsDataRowView;

            if (SelectedGridRow.Length >= 1)
            {
                return (AApAnalAttribRow)SelectedGridRow[0].Row;
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Atds"></param>
        /// <param name="AApDocument"></param>
        /// <param name="Awarning">May be set to show that this is just a warning (eg during save operation)</param>
        /// <returns>true if the document TotalAmount equals the sum of its parts!</returns>
        public static bool BatchBalancesOK(AccountsPayableTDS Atds, AApDocumentRow AApDocument, String Awarning = "")
        {
            decimal DocumentBalance = AApDocument.IsTotalAmountNull() ? 0 : AApDocument.TotalAmount;

            if (DocumentBalance == 0)
            {
                String msg = String.Format(Catalog.GetString("The document {0} is empty.{1}"),
                    AApDocument.DocumentCode,
                    Awarning);
                System.Windows.Forms.MessageBox.Show(
                    msg,
                    Catalog.GetString("Balance Problem"));
                return false;
            }

            foreach (AApDocumentDetailRow Row in Atds.AApDocumentDetail.Rows)
            {
                if (Row.ApDocumentId == AApDocument.ApDocumentId) // NOTE: When called from elsewhere, the TDS could contain data for several documents.
                {
                    DocumentBalance -= Row.IsAmountNull() ? 0 : Row.Amount;
                }
            }

            if (DocumentBalance == 0.0m)
            {
                return true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(
                    String.Format(Catalog.GetString("The document {0} Amount does not equal the sum of the detail lines.{1}"),
                        AApDocument.DocumentCode,
                        Awarning),
                    Catalog.GetString("Balance Problem"));
                return false;
            }
        }

        /// <summary>
        /// Check that the cost centres referred to are OK with the accounts I'm using. If not a message is displayed.
        /// </summary>
        /// <param name="Atds"></param>
        /// <param name="AApDocument"></param>
        /// <param name="Awarning">May be set to show that this is just a warning (eg during save operation)</param>
        /// <returns>false if any detail lines have incompatible cost centres.</returns>
        public static bool AllLinesAccountsOK(AccountsPayableTDS Atds, AApDocumentRow AApDocument, String Awarning = "")
        {
            List <String>AccountCodesCostCentres = new List <string>();

            foreach (AApDocumentDetailRow Row in Atds.AApDocumentDetail.Rows)
            {
                if (Row.ApDocumentId == AApDocument.ApDocumentId)  // NOTE: When called from elsewhere, the TDS could contain data for several documents.
                {
                    if ((Row.AccountCode == "") || (Row.CostCentreCode == ""))
                    {
                        MessageBox.Show(
                            String.Format(
                                Catalog.GetString("Account and Cost Centre must be specified in Document {0}.{1}"),
                                AApDocument.DocumentCode,
                                Awarning),
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
                ReportMsg += Awarning;
                MessageBox.Show(ReportMsg, Catalog.GetString("Invalid Account"),
                    MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check the required analysis attributes for the detail lines in this invoice
        /// </summary>
        /// <param name="Atds"></param>
        /// <param name="AApDocument"></param>
        /// <param name="Awarning">May be set to show that this is just a warning (eg during save operation)</param>
        /// <returns>false if any lines don't have the analysis attributes they require</returns>
        public static bool AllLinesHaveAttributes(AccountsPayableTDS Atds, AApDocumentRow AApDocument, String Awarning = "")
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
                                String.Format(
                                    Catalog.GetString("Analysis Attributes are required for account {0} in Document {1}.{2}"),
                                    Row.AccountCode,
                                    AApDocument.DocumentCode,
                                    Awarning),
                                Catalog.GetString("Analysis Attributes"));
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private static bool CurrencyIsOkForPosting(AccountsPayableTDS Atds, AApDocumentRow AApDocument, String Awarning = "")
        {
            if (AApDocument.CurrencyCode != Atds.AApSupplier[0].CurrencyCode)
            {
                System.Windows.Forms.MessageBox.Show(
                    String.Format(
                        Catalog.GetString("Document {0} cannot be posted because the supplier currency has been changed.{1}"),
                        AApDocument.DocumentCode,
                        Awarning),
                    Catalog.GetString("Post Document"));
                return false;
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Atds"></param>
        /// <param name="AApDocument"></param>
        /// <param name="Awarning">May be set to show that this is just a warning (eg during save operation)</param>
        /// <returns></returns>
        public static bool ExchangeRateIsOk(AccountsPayableTDS Atds, AApDocumentRow AApDocument, String Awarning = "")
        {
            if (AApDocument.ExchangeRateToBase == 0)
            {
                System.Windows.Forms.MessageBox.Show(
                    String.Format(Catalog.GetString("No {0} Exchange Rate has been set.{1}"),
                        AApDocument.CurrencyCode,
                        Awarning),
                    Catalog.GetString("Post Document"));
                return false;
            }

            return true;
        }

        /// <summary>
        /// This static function is called from several places
        /// </summary>
        /// <param name="Atds"></param>
        /// <param name="Adocument"></param>
        /// <param name="AWarningOnly">Set true to show that this is just a warning (eg during save operation)</param>
        /// <returns>true if this document seems OK to post.</returns>
        public static bool ApDocumentCanPost(AccountsPayableTDS Atds, AApDocumentRow Adocument, Boolean AWarningOnly = false)
        {
            // If the batch will not balance, or required attributes are missing, I'll stop right here..
            Boolean returnValue = true;
            String warningMsg = AWarningOnly ? "\nThis problem will prevent the document being posted." : "";

            if (!BatchBalancesOK(Atds, Adocument, warningMsg))
            {
                returnValue = false;

                if (!AWarningOnly)
                {
                    return false;
                }
            }

            if (!AllLinesAccountsOK(Atds, Adocument, warningMsg))
            {
                returnValue = false;

                if (!AWarningOnly)
                {
                    return false;
                }
            }

            if (!AllLinesHaveAttributes(Atds, Adocument, warningMsg))
            {
                returnValue = false;

                if (!AWarningOnly)
                {
                    return false;
                }
            }

            if (!ExchangeRateIsOk(Atds, Adocument, warningMsg))
            {
                returnValue = false;

                if (!AWarningOnly)
                {
                    return false;
                }
            }

            if (!CurrencyIsOkForPosting(Atds, Adocument, warningMsg))
            {
                returnValue = false;

                if (!AWarningOnly)
                {
                    return false;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Post a list of AP documents
        /// This static function is called from several places
        /// /// </summary>
        /// <returns>true if everything went OK</returns>
        public static bool PostApDocumentList(AccountsPayableTDS Atds, int ALedgerNumber, List <int>AApDocumentIds, Form AOwnerForm)
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

            AOwnerForm.Cursor = Cursors.WaitCursor;
            Int32 glBatchNumber;

            if (TRemote.MFinance.AP.WebConnectors.PostAPDocuments(
                    ALedgerNumber,
                    AApDocumentIds,
                    PostingDate,
                    false,
                    out glBatchNumber,
                    out Verifications))
            {
                //
                // The GL Posting Register must be printed.
                TFrmBatchPostingRegister ReportGui = new TFrmBatchPostingRegister(null);
                ReportGui.PrintReportNoUi(ALedgerNumber, glBatchNumber);

                AOwnerForm.Cursor = Cursors.Default;
                return true;
            }
            else
            {
                AOwnerForm.Cursor = Cursors.Default;
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
        /// Approve Document for posting.
        /// See very similar function in TFrmAPSupplierTransactions
        /// </summary>
        ///
        private void ApproveDocument(object sender, EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges)
            {
                MessageBox.Show(Catalog.GetString("Document should be saved before approving."), Catalog.GetString(
                        "Approve Document"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            List <Int32>DocsList = new List <int>();

            DocsList.Add(FMainDS.AApDocument[0].ApDocumentId);
            this.Cursor = Cursors.WaitCursor;
            TVerificationResultCollection verificationResult;
            string MsgTitle = Catalog.GetString("Document Approval");

            if (TRemote.MFinance.AP.WebConnectors.ApproveAPDocuments(FMainDS.AApDocument[0].LedgerNumber, DocsList, out verificationResult))
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(Catalog.GetString("Document has been approved."), MsgTitle);

                FMainDS.AApDocument[0].DocumentStatus = MFinanceConstants.AP_DOCUMENT_APPROVED; // The "changed" light isn't lit by this, because the change
                                                                                                // already took place on the server.
                EnableControls();

                //
                // Also refresh the opener
                TFormsMessage broadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcAPTransactionChanged);
                broadcastMessage.SetMessageDataAPTransaction(lblSupplierName.Text);
                TFormsList.GFormsList.BroadcastFormMessage(broadcastMessage);
            }
            else
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(verificationResult.BuildVerificationResultString(), MsgTitle);
            }
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

            if (PostApDocumentList(FMainDS, FMainDS.AApDocument[0].LedgerNumber, TaggedDocuments, this))
            {
                // TODO: print reports on successfully posted batch
                MessageBox.Show(Catalog.GetString("The AP document has been posted successfully!"));

                //
                // Refresh by re-loading the document from the server
                Int32 DocumentId = FMainDS.AApDocument[0].ApDocumentId;
                FMainDS.Clear();
                FPreviouslySelectedDetailRow = null;
                LoadAApDocument(FDocumentLedgerNumber, DocumentId);

                //
                // Also refresh the opener?
                TFormsMessage broadcastMessage = new TFormsMessage(TFormsMessageClassEnum.mcAPTransactionChanged);
                broadcastMessage.SetMessageDataAPTransaction(lblSupplierName.Text);
                TFormsList.GFormsList.BroadcastFormMessage(broadcastMessage);
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

            if (PaymentScreen.AddDocumentsToPayment(FMainDS, FDocumentLedgerNumber, PayTheseDocs))
            {
                PaymentScreen.Show();
            }
        }

        private void DocumentStatusHint_Click(object sender, EventArgs e)
        {
            // Find the payments that relate to this invoice
            string msgPayments = Catalog.GetString("This following payment number(s) relate to this invoice:");

            foreach (AApDocumentPaymentRow paymentRow in FMainDS.AApDocumentPayment.Rows)
            {
                msgPayments += Environment.NewLine;
                msgPayments += "- " + paymentRow.PaymentNumber.ToString();
            }

            MessageBox.Show(msgPayments, Catalog.GetString("Payments"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ReprintRemittanceAdvice(object sender, EventArgs e)
        {
            List <int>paymentNumberList = new List <int>();

            DataView dv = new DataView(FMainDS.AApDocumentPayment);
            dv.RowFilter = string.Format("{0}={1} and {2}={3}",
                AccountsPayableTDSAApDocumentPaymentTable.GetLedgerNumberDBName(), FDocumentLedgerNumber,
                AccountsPayableTDSAApDocumentPaymentTable.GetApDocumentIdDBName(), FMainDS.AApDocument[0].ApDocumentId);
            dv.Sort = string.Format("{0} ASC", AccountsPayableTDSAApDocumentPaymentTable.GetPaymentNumberDBName());

            if (dv.Count > 0)
            {
                for (int i = 0; i < dv.Count; i++)
                {
                    paymentNumberList.Add(((AccountsPayableTDSAApDocumentPaymentRow)dv[i].Row).PaymentNumber);
                }
            }

            if (paymentNumberList.Count > 0)
            {
                TFormLetterFinanceInfo FormLetterFinanceInfo;

                GetTemplaterFinanceInfo(out FormLetterFinanceInfo);

                if (FormLetterFinanceInfo == null)
                {
                    return;
                }

                paymentNumberList.Sort();
                PrintRemittanceAdviceTemplater(paymentNumberList, FDocumentLedgerNumber, FormLetterFinanceInfo);
            }
            else
            {
                MessageBox.Show(Catalog.GetString("Could not find payment associated with this invoice"),
                    Catalog.GetString("Reprint Remittance Advice"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GetTemplaterFinanceInfo(out TFormLetterFinanceInfo AFormLetterFinanceInfo)
        {
            AFormLetterFinanceInfo = null;

            // not implemented in Open Source OpenPetra
            // TFrmFormSelectionDialog formDialog = new TFrmFormSelectionDialog(this.FindForm());

            return;
        }

        private Boolean CreateRemittanceAdviceFormData(TFormLetterFinanceInfo AFormLetterFinanceInfo,
            List <int>APaymentNumberList,
            int ALedgerNumber,
            ref Boolean ACallFinished)
        {
            bool formDataCreated = TRemote.MFinance.AP.WebConnectors.CreateRemittanceAdviceFormData(
                AFormLetterFinanceInfo, APaymentNumberList, ALedgerNumber, out FFormDataList);

            ACallFinished = true;
            return formDataCreated;
        }

        private void PrintRemittanceAdviceTemplater(List <int>APaymentNumberList,
            int ALedgerNumber,
            TFormLetterFinanceInfo AFormLetterFinanceInfo)
        {
            Boolean ThreadFinished = false;
            Boolean FormLetterDataCreated = false;

            // create form letter data in separate thread. This will fill FFormDataList
            Thread t = new Thread(() => FormLetterDataCreated = CreateRemittanceAdviceFormData(
                    AFormLetterFinanceInfo, APaymentNumberList, ALedgerNumber, ref ThreadFinished));

            using (TProgressDialog dialog = new TProgressDialog(t))
            {
                dialog.ShowDialog();
            }

            // wait here until Thread is really finished
            while (!ThreadFinished)
            {
                Thread.Sleep(50);
            }

            if (FormLetterDataCreated)
            {
                if ((FFormDataList == null)
                    || (FFormDataList.Count == 0))
                {
                    MessageBox.Show(Catalog.GetString("Failed to get data from server to print the Remittance Advice"));
                    return;
                }
            }

            // not implemented in Open Source OpenPetra
            // string targetFolder = TTemplaterAccess.GetFormLetterBaseDirectory(TModule.mFinance);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Will be called by TFormsList to inform any Form that is registered in TFormsList
        /// about any 'Forms Messages' that are broadcasted.
        /// </summary>
        /// <remarks>This form 'listens' to such 'Forms Message' broadcasts by
        /// implementing this virtual Method. This Method will be called each time a
        /// 'Forms Message' broadcast occurs.
        /// </remarks>
        /// <param name="AFormsMessage">An instance of a 'Forms Message'. This can be
        /// inspected for parameters in the Method Body and the Form can use those to choose
        /// to react on the Message, or not.</param>
        /// <returns>True if I acted on the Message.</returns>
        public bool ProcessFormsMessage(TFormsMessage AFormsMessage)
        {
            if (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcAPTransactionChanged)
            {
                if (FPetraUtilsObject.HasChanges)
                {
                    if (MessageBox.Show(
                            Catalog.GetString("Something changed - do you need to reload this document? (You will lose any changes)"),
                            Catalog.GetString("AP Edit Document"),
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation) == DialogResult.No)
                    {
                        return false;
                    }
                }

                LoadAApDocument(FMainDS.AApDocument[0].LedgerNumber, FMainDS.AApDocument[0].ApDocumentId);
                FPetraUtilsObject.DisableSaveButton();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
