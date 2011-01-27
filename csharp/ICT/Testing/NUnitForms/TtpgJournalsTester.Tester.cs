//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangu
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

using System;
using NUnit.Framework;
using NUnit.Extensions.Forms;
using Ict.Petra.Client.MFinance.Gui.GL;

namespace Ict.Testing.NUnitForms
{
    public class TtpgJournalsTester
    {
        private TFrmGLBatch tFrmGLBatch;

        public TextBoxTester txtCurrentPeriod;
        public TextBoxTester txtLedgerNumber;
        public TextBoxTester txtBatchNumber;
        public TextBoxTester txtDebit;
        public TextBoxTester txtCredit;
        public TextBoxTester txtControl;

        public ButtonTester btnAdd;
        public ButtonTester btnCancel;

        public TextBoxTester txtDetailJournalDescription;
        public TCmbAutoCompleteTester cmbDetailSubSystemCode;
        public TCmbAutoPopulatedTester cmbDetailTransactionTypeCode;
        public TCmbAutoPopulatedTester cmbDetailTransactionCurrency;
        public TextBoxTester dtpDetailDateEffective;
        public TextBoxTester txtDetailExchangeRateToBase;

        // public TSgrdDataGridPagedTester grdDetails2;
        private TFrmGLBatchTester tFrmGLBatchTester;

        public TtpgJournalsTester(TFrmGLBatchTester tFrmGLBatchTester, TFrmGLBatch tFrmGLBatch)
        {
            this.tFrmGLBatch = tFrmGLBatch;
            this.tFrmGLBatchTester = tFrmGLBatchTester;

            txtCurrentPeriod = new TextBoxTester("txtCurrentPeriod", tFrmGLBatch);
            txtLedgerNumber = new TextBoxTester("txtLedgerNumber", tFrmGLBatch);
            txtBatchNumber = new TextBoxTester("txtBatchNumber", tFrmGLBatch);
            txtDebit = new TextBoxTester("txtDebit", tFrmGLBatch);
            txtCredit = new TextBoxTester("txtCredit", tFrmGLBatch);
            txtControl = new TextBoxTester("txtControl", tFrmGLBatch);

            btnAdd = new ButtonTester("tpgJournals.btnAdd", tFrmGLBatch);
            btnCancel = new ButtonTester("tpgJournals.btnCancel", tFrmGLBatch);

            txtDetailJournalDescription = new TextBoxTester("txtDetailJournalDescription", tFrmGLBatch);

            cmbDetailSubSystemCode = new TCmbAutoCompleteTester("cmbDetailSubSystemCode", tFrmGLBatch);
            cmbDetailTransactionTypeCode = new TCmbAutoPopulatedTester("cmbDetailTransactionTypeCode", tFrmGLBatch);
            cmbDetailTransactionCurrency = new TCmbAutoPopulatedTester("cmbDetailTransactionCurrency", tFrmGLBatch);

            dtpDetailDateEffective = new TextBoxTester("dtpDetailDateEffective", tFrmGLBatch);
            txtDetailExchangeRateToBase = new TextBoxTester("txtDetailExchangeRateToBase", tFrmGLBatch);
        }

        public void SelectThisTab()
        {
            tFrmGLBatchTester.tabGLBatch.SelectTab(1);
        }
    }
}