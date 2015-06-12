//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu, timop
//
// Copyright 2004-2015 by OM International
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
using System;
using System.Data;
using System.Collections.Generic;

using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core;

using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;


using Ict.Petra.Client.App.Core.RemoteObjects;


namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Description of GLRevaluation_ManualCode.
    /// </summary>
    public partial class TGLRevaluation : IFrmPetra
    {
        private Int32 FLedgerNumber;

        private DateTime FperiodStart;
        private DateTime FperiodEnd;

        private List <CurrencyExchange>FcurrencyExchangeList = new List <CurrencyExchange>();
        private DevAge.ComponentModel.BoundList <CurrencyExchange>FBoundList;

        //TFrmSetupDailyExchangeRate tFrmSetupDailyExchangeRate;


        ClickController linkController = new ClickController();

        /// <summary>
        /// use this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                TRemote.MFinance.GL.WebConnectors.GetCurrentPeriodDates(FLedgerNumber, out FperiodStart, out FperiodEnd);

                CreateDataGridHeader();
                String currencyList = GetListOfRevaluationCurrencies();

                this.lblAccountText.Text = Catalog.GetString("Ledger:");

                lblAccountValue.Text = GetLedgerInfo(FLedgerNumber);

                lblDateEnd.Text = Catalog.GetString("Revaluation Date:");
                lblDateEndValue.Text = FperiodEnd.ToLongDateString();

                lblRevCur.Text = Catalog.GetString("Revaluation Currencies:");
                lblRevCurValue.Text = currencyList;

                lblCostCentre.Text = Catalog.GetString("Revaluation Cost Centre:");
                TFinanceControls.InitialiseCostCentreList(ref cmbCostCentres, FLedgerNumber, true, false, true, true);
                cmbCostCentres.SetSelectedString(
                    TRemote.MFinance.GL.WebConnectors.GetStandardCostCentre(FLedgerNumber)
                    );
            }
        }

        private String GetListOfRevaluationCurrencies()
        {
            TFrmSetupDailyExchangeRate frmExchangeRate =
                new TFrmSetupDailyExchangeRate(this);

            DataTable table = TDataCache.TMFinance.GetCacheableFinanceTable(
                TCacheableFinanceTablesEnum.AccountList, FLedgerNumber);

            int RowIndex = 0;
            String strRevaluationCurrencies = "";

            foreach (DataRow row in table.Rows)
            {
                bool blnIsLedger = (FLedgerNumber == (int)row["a_ledger_number_i"]);
                bool blnAccountActive = (bool)row["a_account_active_flag_l"];
                bool blnAccountForeign = (bool)row["a_foreign_currency_flag_l"];
                bool blnAccountHasPostings = (bool)row["a_posting_status_l"];
                String AccountCode = (String)row["a_account_code_c"];

                if (blnIsLedger && blnAccountActive && blnAccountForeign && blnAccountHasPostings)
                {
                    if (strRevaluationCurrencies == "")
                    {
                        strRevaluationCurrencies = "[" + (string)row["a_foreign_currency_code_c"];
                    }
                    else
                    {
                        strRevaluationCurrencies += ("|" + (string)row["a_foreign_currency_code_c"]);
                    }

                    string strCurrencyCode = (string)row["a_foreign_currency_code_c"];
                    DateTime dateEffectiveFrom;
                    decimal decExchangeRate = frmExchangeRate.GetLastExchangeValueOfInterval(FLedgerNumber,
                        FperiodStart, FperiodEnd, strCurrencyCode, out dateEffectiveFrom);
                    AddADataRow(RowIndex, AccountCode, strCurrencyCode, decExchangeRate, dateEffectiveFrom);
                    ++RowIndex;
                }
            }

            if (strRevaluationCurrencies != "")
            {
                strRevaluationCurrencies = strRevaluationCurrencies + "]";
            }

            return strRevaluationCurrencies;
        }

        private void CreateDataGridHeader()
        {
            grdDetails.BorderStyle = BorderStyle.FixedSingle;

            grdDetails.Columns.Add("DoRevaluation", "...",
                typeof(bool));
            grdDetails.Columns.Add("AccountCode", "Account",
                typeof(string));
            grdDetails.Columns.Add("Currency", "[CUR]",
                typeof(string));
            grdDetails.Columns.Add("ExchangeRate", Catalog.GetString("Rate"),
                typeof(String));
            grdDetails.Columns.Add("Effective", Catalog.GetString("Effective"),
                typeof(string));
            grdDetails.Columns.Add("Status", Catalog.GetString("Status"),
                typeof(string));
            SourceGrid.DataGridColumn gridColumn =
                grdDetails.Columns.Add(null, "", new SourceGrid.Cells.Button("..."));
            linkController.InitFrmData(this, FperiodStart, FperiodEnd);
            gridColumn.DataCell.AddController(linkController);

            grdDetails.SelectionMode = SourceGrid.GridSelectionMode.Row;
            grdDetails.CancelEditingWithEscapeKey = false;

            // Set up for auto-sizing
            grdDetails.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.None;
            grdDetails.Columns[6].AutoSizeMode = SourceGrid.AutoSizeMode.None;

            for (int i = 1; i < 6; i++)
            {
                grdDetails.Columns[i].AutoSizeMode = SourceGrid.AutoSizeMode.EnableAutoSize | SourceGrid.AutoSizeMode.EnableStretch;
            }
        }

        private void AddADataRow(int AIndex, String AAccountCode, string ACurrencyValue, decimal AExchangeRate, DateTime AdateEffectiveFrom)
        {
            CurrencyExchange ce = new CurrencyExchange(AAccountCode, ACurrencyValue, AExchangeRate, AdateEffectiveFrom);

            FcurrencyExchangeList.Add(ce);
            FBoundList = new DevAge.ComponentModel.BoundList <CurrencyExchange>
                             (FcurrencyExchangeList);
            grdDetails.DataSource = FBoundList;

            FBoundList.AllowNew = false;
            FBoundList.AllowDelete = false;

            linkController.SetDataList(FcurrencyExchangeList);
        }

        private String GetLedgerInfo(Int32 ALedgerNumber)
        {
            ALedgerRow ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                     TCacheableFinanceTablesEnum.LedgerDetails, ALedgerNumber))[0];
            String strBaseCurrency = ledger.BaseCurrency;
            String LedgerName = TRemote.MFinance.Reporting.WebConnectors.GetLedgerName(FLedgerNumber);

            LedgerName = String.Format("{0} - {1} [{2}]", FLedgerNumber, LedgerName, strBaseCurrency);
            return LedgerName;
        }

        private void CancelRevaluation(object btn, EventArgs e)
        {
            this.Close();
        }

        private void RunRevaluation(object btn, EventArgs e)
        {
            String ToCostCentre = cmbCostCentres.GetSelectedString();

            if (ToCostCentre == "")
            {
                MessageBox.Show(Catalog.GetString("You must select a revaluation Cost Centre."));
                return;
            }

            int intUsedEntries = 0;

            for (int i = 0; i < FcurrencyExchangeList.Count; ++i)
            {
                if (FcurrencyExchangeList[i].DoRevaluation)
                {
                    ++intUsedEntries;
                }
            }

            if (intUsedEntries == 0)
            {
                MessageBox.Show(Catalog.GetString("No Revaluation operation required."));
                return;
            }

            string[] currencies = new string[intUsedEntries];
            decimal[] rates = new decimal[intUsedEntries];
            int j = 0;

            for (int i = 0; i < FcurrencyExchangeList.Count; ++i)
            {
                if (FcurrencyExchangeList[i].DoRevaluation)
                {
                    currencies[j] = FcurrencyExchangeList[i].Currency;
                    rates[j] = FcurrencyExchangeList[i].mExchangeRate;
                    j++;
                }
            }

            this.Cursor = Cursors.WaitCursor;
            this.Refresh();
            TVerificationResultCollection verificationResult;
            bool blnRevalutationState =
                TRemote.MFinance.GL.WebConnectors.Revaluate(FLedgerNumber,
                    currencies, rates, ToCostCentre, out verificationResult);
            this.Cursor = Cursors.Default;

            if (blnRevalutationState)
            {
                MessageBox.Show(verificationResult.BuildVerificationResultString(),
                    Catalog.GetString("Revaluation error"));
            }
            else
            {
                MessageBox.Show(verificationResult.BuildVerificationResultString(),
                    Catalog.GetString("Revaluation success"));
            }

            this.Close();
        }

        /*  Form event handlers
         *
         */

        void TGLRevaluation_Load(object sender, EventArgs e)
        {
            // Do intial sizing
            TGLRevaluation_Resize(null, null);

            // Set the initial window position if we have stored it.
            FPetraUtilsObject.TFrmPetra_Load(sender, e);
        }

        void TGLRevaluation_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Remember the window position
            FPetraUtilsObject.TFrmPetra_Closing(sender, e);
        }

        void TGLRevaluation_Resize(object sender, EventArgs e)
        {
            int halfWidth = this.Width / 2;

            btnRevaluate.Left = halfWidth - btnRevaluate.Width - 10;
            btnCancel.Left = halfWidth + 10;
            btnRevaluate.Top = this.Height - btnRevaluate.Height - 60;
            btnCancel.Top = btnRevaluate.Top;

            lblAccountText.Left = halfWidth - lblAccountText.Width - 40;
            lblAccountValue.Left = halfWidth - 30;
            lblCostCentre.Left = halfWidth - lblCostCentre.Width - 40;
            cmbCostCentres.Left = halfWidth - 30;
            lblDateEnd.Left = halfWidth - lblDateEnd.Width - 40;
            lblDateEndValue.Left = halfWidth - 30;
            lblRevCur.Left = halfWidth - lblRevCur.Width - 40;
            lblRevCurValue.Left = halfWidth - 30;

            // Auto-size the grid columns
            grdDetails.AutoSizeCells(new SourceGrid.Range(1, 1, grdDetails.Rows.Count - 1, 5));
        }

        #region FPetraUtilsObject and IFrmPetra interface

        // FPetraUtilsObject and IFrmPetra interface
        private TFrmPetraUtils FPetraUtilsObject = null;

        /// <summary>
        /// Interface method
        /// </summary>
        public void RunOnceOnActivation()
        {
            // Nothing to do
        }

        /// <summary>
        /// Interface method
        /// </summary>
        public bool CanClose()
        {
            return true;
        }

        /// <summary>
        /// Interface method
        /// </summary>
        public TFrmPetraUtils GetPetraUtilsObject()
        {
            return FPetraUtilsObject;
        }

        #endregion

        #region public class CurrencyExchange

        /// <summary>
        /// A CurrencyExchange-Element is a member of a currencyExchangeList which is used as a
        /// data base local to the revaluation gui. Here the settings of the user dialogs are stored
        /// </summary>
        public class CurrencyExchange
        {
            /// <summary>
            /// In this status the exchange rate is not defined. So you cannot change between
            /// DO_REVALUATION and DO_NO_REVALUATION.
            /// </summary>
            public const int IS_NOT_INITIALIZED = 0;

            /// <summary>
            /// The Revaluation shall be done ...
            /// </summary>
            public const int DO_REVALUATION = 1;

            /// <summary>
            /// The revaluation shall not to be done even if the exchange rate is valid.
            /// </summary>
            public const int DO_NO_REVALUATION = 2;

            private bool mDoRevaluation = true;
            private string mCurrency = "?";
            private String FAccountCode = "";
            private DateTime FdateEffectiveFrom;
            /// <summary>
            /// I've made this public because the ExchangeRate property is now a string.
            /// </summary>
            public decimal mExchangeRate = 1.0m;
            private int intStatus;

            private void SetRateAndStatus(decimal ANewExchangeRate, DateTime AeffectiveDate)
            {
                mExchangeRate = ANewExchangeRate;
                FdateEffectiveFrom = AeffectiveDate;

                if (mExchangeRate == 0) // This is set when user de-selects a row
                {
                    intStatus = DO_NO_REVALUATION;
                    mDoRevaluation = false;
                }

                if (mExchangeRate == 1.0m)
                {
                    intStatus = IS_NOT_INITIALIZED;
                    mDoRevaluation = false;
                }
                else
                {
                    intStatus = DO_REVALUATION;
                    mDoRevaluation = true;
                }
            }

            /// <summary>
            /// The only one constructor ...
            /// </summary>
            /// <param name="AAccountCode">Foreign currency Account</param>
            /// <param name="ACurrency">Defines the foreign currency</param>
            /// <param name="AExchangeRate">Defines the exchange rate and 0 is the value to define a invalid rate.</param>
            /// <param name="AdateEffectiveFrom">How old is this Exchange Rate</param>
            public CurrencyExchange(String AAccountCode, string ACurrency, decimal AExchangeRate, DateTime AdateEffectiveFrom)
            {
                FAccountCode = AAccountCode;
                mCurrency = ACurrency;
                SetRateAndStatus(AExchangeRate, AdateEffectiveFrom);
            }

            /// <summary>
            /// DoRevalution is defined in the set and the get-paths and so the user can
            /// use the checkbox in the grid to change the value.
            /// </summary>
            public bool DoRevaluation
            {
                get
                {
                    return mDoRevaluation;
                }
                set
                {
                    if (intStatus != IS_NOT_INITIALIZED)
                    {
                        mDoRevaluation = value;
//                      SetRateAndStatus(0.0m, DateTime.Now);
                    }
                }
            }


            /// <summary>
            /// For AccountCode only the get part is defined. So the user cannot change the
            /// value by using the grid.
            /// </summary>
            public string AccountCode
            {
                get
                {
                    return FAccountCode;
                }
            }

            /// <summary>
            /// For currency only the get part is defined. So the user cannot change the
            /// value by using the grid.
            /// </summary>
            public string Currency
            {
                get
                {
                    return mCurrency;
                }
            }

            /// <summary>
            /// Only the get part is defined. So the user cannot change the value by using the grid.
            /// </summary>
            public String ExchangeRate
            {
                get
                {
                    return (mDoRevaluation) ? mExchangeRate.ToString("G6") : "";
                }
            }

            /// <summary>
            /// Only the get part is defined. So the user cannot change the value by using the grid.
            /// </summary>
            public String Effective
            {
                get
                {
                    return (mDoRevaluation) ? FdateEffectiveFrom.ToString("dd-MMM") : "";
                }
            }

            /// <summary>
            /// The status is readonly too ...
            /// </summary>
            public string Status
            {
                get
                {
                    if (intStatus == IS_NOT_INITIALIZED)
                    {
                        return Catalog.GetString("Not initialized");
                    }

                    return (mDoRevaluation) ? Catalog.GetString("Revaluation") : Catalog.GetString("No Revaluation");
                }
            }

            /// <summary>
            ///
            /// </summary>
            public Boolean Update
            {
                set
                {
                    MessageBox.Show("Update!");
                }
            }

            /// <summary>
            /// External interface routine to update the exchange rate.
            /// </summary>
            /// <param name="newRate"></param>
            /// <param name="AeffectiveDate"></param>
            public void updateExchangeRate(decimal newRate, DateTime AeffectiveDate)
            {
                if (newRate != mExchangeRate)
                {
                    SetRateAndStatus(newRate, AeffectiveDate);
                }
            }
        }

        #endregion

        #region private class ClickController

        private class ClickController : SourceGrid.Cells.Controllers.ControllerBase
        {
            TGLRevaluation mainForm;
            DateTime dteStart;
            DateTime dteEnd;

            List <CurrencyExchange>currencyExchangeList;


            public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
            {
                base.OnMouseUp(sender, e);
                int iRow = sender.Position.Row - 1;

                if (iRow < 0)
                {
                    return;
                }

                TFrmSetupDailyExchangeRate frmExchangeRate = new TFrmSetupDailyExchangeRate(mainForm);

                decimal selectedExchangeRate;
                DateTime selectedEffectiveDate;
                int selectedEffectiveTime;

                if (frmExchangeRate.ShowDialog(
                        mainForm.FLedgerNumber,
                        dteStart,
                        dteEnd,
                        currencyExchangeList[iRow].Currency,
                        currencyExchangeList[iRow].mExchangeRate,
                        out selectedExchangeRate,
                        out selectedEffectiveDate,
                        out selectedEffectiveTime) != DialogResult.Cancel)
                {
                    currencyExchangeList[iRow].updateExchangeRate(selectedExchangeRate, selectedEffectiveDate);
                    sender.Grid.InvalidateRange(new SourceGrid.Range(sender.Position.Row, 1, sender.Position.Row, 4));
                }
            }

            public void InitFrmData(TGLRevaluation AMain, DateTime ADateStart, DateTime ADateEnd)
            {
                mainForm = AMain;
                dteStart = ADateStart;
                dteEnd = ADateEnd;
            }

            public void SetDataList(List <CurrencyExchange>ACurrencyExchangeList)
            {
                currencyExchangeList = ACurrencyExchangeList;
            }
        }

        #endregion
    }
}