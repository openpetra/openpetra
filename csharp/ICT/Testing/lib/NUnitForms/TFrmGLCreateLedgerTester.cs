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
using System.IO;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.MFinance.Gui;
using Ict.Petra.Client.MFinance.Gui.GL;
using Ict.Petra.Client.MFinance.Gui.Setup;
using Ict.Testing.NUnitForms;
using Ict.Testing.NUnitPetraClient;
using NUnit.Extensions.Forms;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Ict.Testing.NUnitForms
{
    /// <summary>
    /// Description of TFrmGLCreateLedgerTester.
    /// </summary>
    public sealed class TFrmGLCreateLedgerTester
    {
        public TCmbAutoPopulatedTester cmbCountryCode;
        public TCmbAutoPopulatedTester cmbBaseCurrency;
        public TCmbAutoPopulatedTester cmbIntlCurrency;
        public TextBoxTester txtLedgerName;

        public TextBoxTester dtpCalendarStartDate;
        public NumericUpDownTester nudLedgerNumber;
        public NumericUpDownTester nudNumberOfPeriods;
        public NumericUpDownTester nudCurrentPeriod;
        public NumericUpDownTester nudNumberOfFwdPostingPeriods;

        public ToolStripButtonTester tbbCreate;

        TFrmGLCreateLedger tFrmGLCreateLedger;

        private static TFrmGLCreateLedgerTester instance = new TFrmGLCreateLedgerTester();


        public static TFrmGLCreateLedgerTester Instance {
            get
            {
                return instance;
            }
        }

        private TFrmGLCreateLedgerTester()
        {
            tFrmGLCreateLedger = new TFrmGLCreateLedger(IntPtr.Zero);

            nudLedgerNumber = new NumericUpDownTester("nudLedgerNumber", tFrmGLCreateLedger);

            txtLedgerName = new TextBoxTester("txtLedgerName", tFrmGLCreateLedger);

            cmbCountryCode = new TCmbAutoPopulatedTester("cmbCountryCode", tFrmGLCreateLedger);
            cmbBaseCurrency = new TCmbAutoPopulatedTester("cmbBaseCurrency", tFrmGLCreateLedger);
            cmbIntlCurrency = new TCmbAutoPopulatedTester("cmbIntlCurrency", tFrmGLCreateLedger);

            TextBoxTester dtpCalendarStartDate = new TextBoxTester("dtpCalendarStartDate", tFrmGLCreateLedger);

            nudNumberOfPeriods = new NumericUpDownTester("nudNumberOfPeriods", tFrmGLCreateLedger);
            nudCurrentPeriod = new NumericUpDownTester("nudCurrentPeriod", tFrmGLCreateLedger);
            nudNumberOfFwdPostingPeriods = new NumericUpDownTester("nudNumberOfFwdPostingPeriods", tFrmGLCreateLedger);

            tbbCreate = new ToolStripButtonTester("tbbCreate", tFrmGLCreateLedger);
        }
    }
}