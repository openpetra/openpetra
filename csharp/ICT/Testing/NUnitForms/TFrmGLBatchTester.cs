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
	public sealed class TFrmGLBatchTester : FormTester
	{
		
		public TFrmGLBatch tFrmGLBatch;

		public ToolStripButtonTester tbbPostBatch;
		public ToolStripButtonTester tbbImportFromSpreadSheet;
		public ToolStripButtonTester tbbImportBatches;
		public ToolStripButtonTester tbbExportBatches;

		// public TTabVersatileTester tabGLBatch;
		public ToolStripButtonTester tbbSave;
		
		public TTabVersatileTester tabGLBatch;
		
		public TtpgBatchesTester ttpgBatches;
		public TtpgJournalsTester ttpgJournals;
		public TtpgTransactionsTester ttpgTransactions;
		
					
		public TFrmGLBatchTester() {
			
			tFrmGLBatch = new TFrmGLBatch(IntPtr.Zero);
			
			tbbPostBatch = new ToolStripButtonTester("tbbPostBatch",tFrmGLBatch);
			tbbImportFromSpreadSheet = new ToolStripButtonTester("tbbImportFromSpreadSheet",tFrmGLBatch);
			tbbImportBatches = new ToolStripButtonTester("tbbImportBatches",tFrmGLBatch);
			tbbExportBatches = new ToolStripButtonTester("tbbExportBatches",tFrmGLBatch);
			
			tbbSave = new ToolStripButtonTester("tbbSave",tFrmGLBatch);
			
			tabGLBatch = new TTabVersatileTester("tabGLBatch",tFrmGLBatch);

			ttpgBatches = new TtpgBatchesTester(tFrmGLBatch);
			ttpgJournals = new TtpgJournalsTester(tFrmGLBatch);
			ttpgTransactions = new TtpgTransactionsTester(tFrmGLBatch);
			
		}
		
	}
}
