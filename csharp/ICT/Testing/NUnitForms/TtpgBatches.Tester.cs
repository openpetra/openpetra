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
    public class TtpgBatchesTester
    {
        // private TUC_GLJournals ucoJournals;
        private TFrmGLBatch tFrmGLBatch;
        public TextBoxTester txtLedgerNumber;

        public RadioButtonTester rbtPosting;
        public RadioButtonTester rbtEditing;
        public RadioButtonTester rbtAll;

        public ButtonTester btnNew;
        public ButtonTester btnCancel;
        public ButtonTester btnPostBatch;

        public TextBoxTester txtDetailBatchDescription;
        public TTxtNumericTextBoxTester txtDetailBatchControlTotal;
        public TextBoxTester dtpDetailDateEffective;

        public LabelTester lblValidDateRange;

        public TSgrdDataGridPagedTester grdDetails;

        private TFrmGLBatchTester tFrmGLBatchTester;

        public TtpgBatchesTester(TFrmGLBatchTester tFrmGLBatchTester, TFrmGLBatch tFrmGLBatch)
        {
            this.tFrmGLBatch = tFrmGLBatch;
            this.tFrmGLBatchTester = tFrmGLBatchTester;


            txtLedgerNumber = new TextBoxTester("ucoBatches.pnlContent.pnlInfo.tableLayoutPanel1.pnlLedgerInfo.tableLayoutPanel2.txtLedgerNumber", tFrmGLBatch);

            rbtPosting = new RadioButtonTester("rbtPosting", tFrmGLBatch);
            rbtEditing = new RadioButtonTester("rbtEditing", tFrmGLBatch);
            rbtAll = new RadioButtonTester("rbtAll", tFrmGLBatch);

            btnNew = new ButtonTester("ucoBatches.pnlContent.pnlDetailGrid.pnlDetailButtons.tableLayoutPanel4.btnNew", tFrmGLBatch);
            btnCancel = new ButtonTester("ucoJournals.pnlContent.pnlDetailGrid.pnlDetailButtons.tableLayoutPanel2.btnCancel", tFrmGLBatch);
            btnPostBatch = new ButtonTester("btnPostBatch", tFrmGLBatch);

            txtDetailBatchDescription = new TextBoxTester("txtDetailBatchDescription", tFrmGLBatch);
            txtDetailBatchControlTotal = new TTxtNumericTextBoxTester("txtDetailBatchControlTotal", tFrmGLBatch);
            dtpDetailDateEffective = new TextBoxTester("ucoBatches.pnlContent.pnlDetails.tableLayoutPanel5.dtpDetailDateEffective", tFrmGLBatch);

            lblValidDateRange = new LabelTester("lblValidDateRange", tFrmGLBatch);
            System.Console.WriteLine("123");
            grdDetails = new TSgrdDataGridPagedTester("ucoBatches.pnlContent.pnlDetailGrid.grdDetails", tFrmGLBatch);
            System.Console.WriteLine("123#");
//            grdDetails.AddASourceGridSelectionTester(
//            	new SourceGridSelectionTester("tpgBatches.grdDetails.Selection"));
        }

        public void SelectThisTab()
        {
            tFrmGLBatchTester.tabGLBatch.SelectTab(0);
        }
    }
}