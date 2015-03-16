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

using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;


using Ict.Petra.Client.App.Core.RemoteObjects;


namespace Ict.Petra.Client.MFinance.Gui.GL
{
    /// <summary>
    /// Description of GLRevaluation_ManualCode.
    /// </summary>
    public partial class TGLRevaluation
    {
        private Int32 FLedgerNumber;

        private DateTime FperiodStart;
        private DateTime FperiodEnd;

        private List <CurrencyExchange>FcurrencyExchangeList = new List <CurrencyExchange>();
        private DevAge.ComponentModel.BoundList <CurrencyExchange>FBoundList;

        //TFrmSetupDailyExchangeRate tFrmSetupDailyExchangeRate;


        LinkClickDelete linkClickDelete = new LinkClickDelete();

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

            int ic = 0;
            String strRevaluationCurrencies = "";

            foreach (DataRow row in table.Rows)
            {
                bool blnIsLedger = (FLedgerNumber == (int)row["a_ledger_number_i"]);
                bool blnAccountActive = (bool)row["a_account_active_flag_l"];
                bool blnAccountForeign = (bool)row["a_foreign_currency_flag_l"];
                bool blnAccountHasPostings = (bool)row["a_posting_status_l"];
                String AccountCode = (String)row["a_account_code_c"];

                if (blnIsLedger && blnAccountActive
                    && blnAccountForeign && blnAccountHasPostings)
                {
                    if (strRevaluationCurrencies == "")
                    {
                        strRevaluationCurrencies =
                            "[" + (string)row["a_foreign_currency_code_c"];
                    }
                    else
                    {
                        strRevaluationCurrencies = strRevaluationCurrencies +
                                                   "|" + (string)row["a_foreign_currency_code_c"];
                    }

                    string strCurrencyCode = (string)row["a_foreign_currency_code_c"];
                    decimal decExchangeRate = frmExchangeRate.GetLastExchangeValueOfInterval(FLedgerNumber,
                        FperiodStart, FperiodEnd, strCurrencyCode);
                    AddADataRow(ic, AccountCode, strCurrencyCode, decExchangeRate);
                    ++ic;
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
                typeof(bool)).Width = 30;
            grdDetails.Columns.Add("AccountCode", "Account",
                typeof(string)).Width = 60;
            grdDetails.Columns.Add("Currency", "[CUR]",
                typeof(string)).Width = 50;
            grdDetails.Columns.Add("ExchangeRate", Catalog.GetString("Exchange Rate"),
                typeof(decimal)).Width = 110;
            grdDetails.Columns.Add("Status", Catalog.GetString("Status"),
                typeof(string)).Width = 110;

            grdDetails.SelectionMode = SourceGrid.GridSelectionMode.Row;
            grdDetails.CancelEditingWithEscapeKey = false;


            SourceGrid.DataGridColumn gridColumn;

            gridColumn = grdDetails.Columns.Add(
                null, "", new SourceGrid.Cells.Button("..."));
            linkClickDelete.InitFrmData(this, FperiodStart, FperiodEnd);
            gridColumn.DataCell.AddController(linkClickDelete);
        }

        private void AddADataRow(int AIndex, String AAccountCode, string ACurrencyValue, decimal AExchangeRate)
        {
            CurrencyExchange ce = new CurrencyExchange(AAccountCode, ACurrencyValue, AExchangeRate);

            FcurrencyExchangeList.Add(ce);
            FBoundList = new DevAge.ComponentModel.BoundList <CurrencyExchange>
                             (FcurrencyExchangeList);
            grdDetails.DataSource = FBoundList;

            FBoundList.AllowNew = false;
            FBoundList.AllowDelete = false;

            for (Int32 column = 1; column <= 3; column++)
            {
                grdDetails.Columns[column].GetDataCell(AIndex).Editor.EnableEdit = false;
            }

            linkClickDelete.SetDataList(FcurrencyExchangeList);
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
            int intUsedEntries = 0;

            for (int i = 0; i < FcurrencyExchangeList.Count; ++i)
            {
                if (FcurrencyExchangeList[i].DoRevaluation)
                {
                    ++intUsedEntries;
                }
            }

            String ToCostCentre = cmbCostCentres.GetSelectedString();

            if (ToCostCentre == "")
            {
                MessageBox.Show(Catalog.GetString("You must select a revaluation Cost Centre."));
                return;
            }

            if (intUsedEntries == 0)
            {
                MessageBox.Show(Catalog.GetString("No Revaluation operation required."));
                this.Close();
            }

            string[] currencies = new string[intUsedEntries];
            decimal[] rates = new decimal[intUsedEntries];
            int j = 0;

            for (int i = 0; i < FcurrencyExchangeList.Count; ++i)
            {
                if (FcurrencyExchangeList[i].DoRevaluation)
                {
                    currencies[j] = FcurrencyExchangeList[i].Currency;
                    rates[j] = FcurrencyExchangeList[i].ExchangeRate;
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
                    Catalog.GetString("Revaluation errors ..."));
            }
            else
            {
                MessageBox.Show(Catalog.GetString("GL Revaluation complete."));
            }

            this.Close();
        }

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
            /// The revalueation shall not to be done even if the exchange rate is valid.
            /// </summary>
            public const int DO_NO_REVALUATION = 2;

            private string strMessageNotInitialized = Catalog.GetString("Not initialized");
            private string strMessageRunRevaluation = Catalog.GetString("Revaluation");
            private string strMessageRunNoRevaluation = Catalog.GetString("No Revaluation");

            private bool mDoRevaluation = true;
            private string mCurrency = "?";
            private String FAccountCode = "";
            private decimal mExchangeRate = 1.0m;
            private string mStatus = "?";
            private int intStatus;

            private void SetRateAndStatus(decimal ANewExchangeRate)
            {
                if (ANewExchangeRate == 0)
                {
                    if (mDoRevaluation)
                    {
                        intStatus = DO_REVALUATION;
                    }
                    else
                    {
                        intStatus = DO_NO_REVALUATION;
                    }
                }
                else
                {
                    mExchangeRate = ANewExchangeRate;

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

                if (intStatus == IS_NOT_INITIALIZED)
                {
                    mStatus = strMessageNotInitialized;
                }
                else if (intStatus == DO_REVALUATION)
                {
                    mStatus = strMessageRunRevaluation;
                }
                else if (intStatus == DO_NO_REVALUATION)
                {
                    mStatus = strMessageRunNoRevaluation;
                }
            }

            /// <summary>
            /// The only one constructor ...
            /// </summary>
            /// <param name="AAccountCode">Foreign currency Account</param>
            /// <param name="ACurrency">Defines the foreign currency</param>
            /// <param name="AExchangeRate">Defines the exchange rate and 0 is the
            /// value to define a invalid rate.</param>
            public CurrencyExchange(String AAccountCode, string ACurrency, decimal AExchangeRate)
            {
                FAccountCode = AAccountCode;
                mCurrency = ACurrency;
                SetRateAndStatus(AExchangeRate);
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
                        SetRateAndStatus(0.0m);
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
            /// For the exchange rate only the get part is defined. So the user cannot change the
            /// value by using the grid.
            /// </summary>
            public decimal ExchangeRate
            {
                get
                {
                    return mExchangeRate;
                }
            }

            /// <summary>
            /// The status is readonly too ...
            /// </summary>
            public string Status
            {
                get
                {
                    return mStatus;
                }
            }

            /// <summary>
            /// External interface routine to update the exchange rate.
            /// </summary>
            /// <param name="newRate"></param>
            public void updateExchangeRate(decimal newRate)
            {
                if (newRate != mExchangeRate)
                {
                    SetRateAndStatus(newRate);
                }
            }
        }

        private class LinkClickDelete : SourceGrid.Cells.Controllers.ControllerBase
        {
            int ix = 0;

            TGLRevaluation mainForm;
            DateTime dteStart;
            DateTime dteEnd;

            List <CurrencyExchange>currencyExchangeList;


            public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
            {
                base.OnClick(sender, e);

                ++ix;
                System.Diagnostics.Debug.WriteLine(sender.Position.Row.ToString());

                int iRow = sender.Position.Row - 1;

                TFrmSetupDailyExchangeRate frmExchangeRate =
                    new TFrmSetupDailyExchangeRate(mainForm);

                decimal selectedExchangeRate;
                DateTime selectedEffectiveDate;
                int selectedEffectiveTime;

                if (frmExchangeRate.ShowDialog(
                        mainForm.FLedgerNumber,
                        dteStart,
                        dteEnd,
                        currencyExchangeList[iRow].Currency,
                        currencyExchangeList[iRow].ExchangeRate,
                        out selectedExchangeRate,
                        out selectedEffectiveDate,
                        out selectedEffectiveTime) == DialogResult.Cancel)
                {
                    return;
                }

                currencyExchangeList[iRow].updateExchangeRate(selectedExchangeRate);
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
    }
}