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
using Ict.Petra.Client.CommonForms;
using Ict.Testing.NUnitForms;
using Ict.Testing.NUnitPetraClient;
using NUnit.Extensions.Forms;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Ict.Testing.NUnitForms
{
    /// <summary>
    /// mainForm is one of the Base-Dialogs in the MFinance-Package.
    /// This file generates a common useable Test-Interface for this dialog.
    /// Please enshure to call "mainForm.LedgerNumber(ledgerNumber) defining a
    /// specific ledger to be edited directly <b>after</b> mainForm.Show().
    ///
    /// Hint - acutally not all controls are connected.
    /// </summary>
    public sealed class TFrmGLAccountHierarchyTester
    {
        /// <summary>
        /// ...
        /// </summary>
        public TFrmGLAccountHierarchy mainForm;

        /// <summary>
        /// ...
        /// </summary>
        public TTrvTreeViewTester trvAccounts;
        /// <summary>
        /// ...
        /// </summary>
        public TextBoxTester txtDetailAccountCode;
        /// <summary>
        /// ...
        /// </summary>
        public TCmbAutoCompleteTester cmbDetailAccountType;
        /// <summary>
        /// ...
        /// </summary>
        public TextBoxTester txtDetailEngAccountCodeLongDesc;
        /// <summary>
        /// ...
        /// </summary>
        public TextBoxTester txtDetailEngAccountCodeShortDesc;
        /// <summary>
        /// ...
        /// </summary>
        public TextBoxTester txtDetailAccountCodeLongDesc;
        /// <summary>
        /// ...
        /// </summary>
        public TextBoxTester txtDetailAccountCodeShortDesc;
        /// <summary>
        /// ...
        /// </summary>
        public TCmbAutoCompleteTester cmbDetailValidCcCombo;
        /// <summary>
        /// ...
        /// </summary>
        public CheckBoxTester chkDetailBankAccountFlag;
        /// <summary>
        /// ...
        /// </summary>
        public CheckBoxTester chkDetailAccountActiveFlag;
        /// <summary>
        /// ...
        /// </summary>
        // public Ict.Petra.Client.MFinance.Gui.Setup.TUC_AccountAnalysisAttributes ucoAccountAnalysisAttributes;
        public ToolStripButtonTester tbbSave;
        /// <summary>
        /// ...
        /// </summary>
        public ToolStripButtonTester tbbAddNewAccount;
        /// <summary>
        /// ...
        /// </summary>
        public ToolStripButtonTester tbbDeleteUnusedAccount;
        /// <summary>
        /// ...
        /// </summary>
        public ToolStripButtonTester tbbExportHierarchy;
        /// <summary>
        /// ...
        /// </summary>
        public ToolStripButtonTester tbbImportHierarchy;
        /// <summary>
        /// ...
        /// </summary>
        public System.Windows.Forms.MenuStrip mnuMain;
        /// <summary>
        /// ...
        /// </summary>
        public CheckBoxTester chkDetailForeignCurrencyFlag;
        /// <summary>
        /// ...
        /// </summary>
        public TCmbAutoPopulatedTester cmbDetailForeignCurrencyCode;
        /// <summary>
        /// ...
        /// </summary>
        public ToolStripMenuItemTester mniClose;
//        public ToolStripMenuItemTester mniFile;
//        public System.Windows.Forms.ToolStripMenuItem mniFileSave;
//        public System.Windows.Forms.ToolStripSeparator mniSeparator0;
//        public System.Windows.Forms.ToolStripMenuItem mniFilePrint;
//        public System.Windows.Forms.ToolStripSeparator mniSeparator1;
//        public System.Windows.Forms.ToolStripMenuItem mniClose;
//        public System.Windows.Forms.ToolStripMenuItem mniEdit;
//        public System.Windows.Forms.ToolStripMenuItem mniEditUndoCurrentField;
//        public System.Windows.Forms.ToolStripMenuItem mniEditUndoScreen;
//        public System.Windows.Forms.ToolStripSeparator mniSeparator2;
//        public System.Windows.Forms.ToolStripMenuItem mniEditFind;
//        public System.Windows.Forms.ToolStripMenuItem mniAccounts;
//        public System.Windows.Forms.ToolStripMenuItem mniAddNewAccount;
//        public System.Windows.Forms.ToolStripMenuItem mniDeleteUnusedAccount;
//        public System.Windows.Forms.ToolStripSeparator mniSeparator3;
//        public System.Windows.Forms.ToolStripMenuItem mniExportHierarchy;
//        public System.Windows.Forms.ToolStripMenuItem mniImportHierarchy;
//        public System.Windows.Forms.ToolStripMenuItem mniHelp;
//        public System.Windows.Forms.ToolStripMenuItem mniHelpPetraHelp;
//        public System.Windows.Forms.ToolStripSeparator mniSeparator4;
//        public System.Windows.Forms.ToolStripMenuItem mniHelpBugReport;
//        public System.Windows.Forms.ToolStripSeparator mniSeparator5;
//        public System.Windows.Forms.ToolStripMenuItem mniHelpAboutPetra;
//        public System.Windows.Forms.ToolStripMenuItem mniHelpDevelopmentTeam;


        // Contructor which initializes the acces to all the controls on the
        // Dialog
        /// <summary>
        /// ...
        /// </summary>
        public TFrmGLAccountHierarchyTester()
        {
            mainForm = new TFrmGLAccountHierarchy(null);

            trvAccounts = new TTrvTreeViewTester("trvAccounts", mainForm);
            txtDetailAccountCode = new TextBoxTester("txtDetailAccountCode", mainForm);
            cmbDetailAccountType = new TCmbAutoCompleteTester("cmbDetailAccountType", mainForm);
            txtDetailEngAccountCodeLongDesc = new TextBoxTester("txtDetailEngAccountCodeLongDesc", mainForm);
            txtDetailEngAccountCodeShortDesc = new TextBoxTester("txtDetailEngAccountCodeShortDesc", mainForm);

            txtDetailAccountCodeLongDesc = new TextBoxTester("txtDetailAccountCodeLongDesc", mainForm);
            txtDetailAccountCodeShortDesc = new TextBoxTester("txtDetailAccountCodeShortDesc", mainForm);

            cmbDetailValidCcCombo = new TCmbAutoCompleteTester("cmbDetailValidCcCombo", mainForm);
            chkDetailBankAccountFlag = new CheckBoxTester("chkDetailBankAccountFlag", mainForm);
            chkDetailAccountActiveFlag = new CheckBoxTester("chkDetailAccountActiveFlag", mainForm);

            tbbSave = new ToolStripButtonTester("tbbSave", mainForm);
            tbbAddNewAccount = new ToolStripButtonTester("tbbAddNewAccount", mainForm);
            tbbDeleteUnusedAccount = new ToolStripButtonTester("tbbDeleteUnusedAccount", mainForm);
            tbbExportHierarchy = new ToolStripButtonTester("tbbExportHierarchy", mainForm);
            tbbImportHierarchy = new ToolStripButtonTester("tbbImportHierarchy", mainForm);

            chkDetailForeignCurrencyFlag = new CheckBoxTester("chkDetailForeignCurrencyFlag", mainForm);
            cmbDetailForeignCurrencyCode = new TCmbAutoPopulatedTester("cmbDetailForeignCurrencyCode", mainForm);


            mniClose = new ToolStripMenuItemTester("mniClose", mainForm);
        }
    }
}