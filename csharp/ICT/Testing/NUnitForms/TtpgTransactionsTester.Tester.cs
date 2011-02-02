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
    public class TtpgTransactionsTester
    {
        private TFrmGLBatch tFrmGLBatch;

        public TextBoxTester txtLedgerNumber;
        public TextBoxTester txtBatchNumber;
        public TextBoxTester txtJournalNumber;

        public ButtonTester btnNew2;
        public ButtonTester btnRemove;

        public TCmbAutoPopulatedTester cmbDetailCostCentreCode;
        public TCmbAutoPopulatedTester cmbDetailAccountCode;

        public TextBoxTester txtDetailNarrative;
        public TextBoxTester txtDetailReference;
        public TextBoxTester dtpDetailTransactionDate;
        public TCmbAutoCompleteTester cmbDetailKeyMinistryKey;

        public TextBoxTester txtDebitAmount;
        public TextBoxTester txtDebitAmountBase;
        public TextBoxTester txtCreditAmount;
        public TextBoxTester txtCreditAmountBase;
        public TextBoxTester txtDebitTotalAmount;
        public TextBoxTester txtDebitTotalAmountBase;
        public TextBoxTester txtCreditTotalAmount;
        public TextBoxTester txtCreditTotalAmountBase;

        // public TSgrdDataGridPagedTester grdDetails3;
        private TFrmGLBatchTester tFrmGLBatchTester;

        public TtpgTransactionsTester(TFrmGLBatchTester tFrmGLBatchTester, TFrmGLBatch tFrmGLBatch)
        {
            this.tFrmGLBatch = tFrmGLBatch;
            this.tFrmGLBatchTester = tFrmGLBatchTester;

            txtLedgerNumber = new TextBoxTester("ucoTransactions.pnlContent.pnlInfo.tableLayoutPanel1.txtLedgerNumber", tFrmGLBatch);
            txtBatchNumber = new TextBoxTester("txtBatchNumber", tFrmGLBatch);
            txtJournalNumber = new TextBoxTester("txtJournalNumber", tFrmGLBatch);

            btnNew2 = new ButtonTester("tpgTransactions.btnNew", tFrmGLBatch);
            btnRemove = new ButtonTester("tpgTransactions.btnRemove", tFrmGLBatch);

            txtDetailNarrative = new TextBoxTester("txtDetailNarrative", tFrmGLBatch);
            txtDetailReference = new TextBoxTester("txtDetailReference", tFrmGLBatch);
            dtpDetailTransactionDate = new TextBoxTester("dtpDetailTransactionDate", tFrmGLBatch);
            cmbDetailKeyMinistryKey = new TCmbAutoCompleteTester("cmbDetailKeyMinistryKey", tFrmGLBatch);

            txtDebitAmount = new TextBoxTester("txtDebitAmount", tFrmGLBatch);

            txtDebitAmountBase = new TextBoxTester("txtDebitAmountBase", tFrmGLBatch);
            txtCreditAmount = new TextBoxTester("txtCreditAmount", tFrmGLBatch);
            txtCreditAmountBase = new TextBoxTester("txtCreditAmountBase", tFrmGLBatch);
            txtDebitTotalAmount = new TextBoxTester("txtDebitTotalAmount", tFrmGLBatch);
            txtDebitTotalAmountBase = new TextBoxTester("txtDebitTotalAmountBase", tFrmGLBatch);
            txtCreditTotalAmount = new TextBoxTester("txtCreditTotalAmount", tFrmGLBatch);
            txtCreditTotalAmountBase = new TextBoxTester("txtCreditTotalAmountBase", tFrmGLBatch);

            // grdDetails3 = new TSgrdDataGridPagedTester("grdDetails3",tFrmGLBatch);
        }

        public void SelectThisTab()
        {
            tFrmGLBatchTester.tabGLBatch.SelectTab(2);
        }
    }
}