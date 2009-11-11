/* auto generated with nant generateWinforms from MainForm.yaml and template windowFind
 *
 * DO NOT edit manually, DO NOT edit with the designer
 * use a user control if you need to modify the screen content
 *
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MFinance.Gui.BankImport
{

  /// auto generated: Bank import
  public partial class TFrmMainForm: System.Windows.Forms.Form, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private Ict.Petra.Client.CommonForms.TFrmPetraUtils FPetraUtilsObject;

    /// constructor
    public TFrmMainForm(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblBankName.Text = Catalog.GetString("Bank Name:");
      this.lblDateStatement.Text = Catalog.GetString("Date Statement:");
      this.lblStartBalance.Text = Catalog.GetString("Start Balance:");
      this.lblEndBalance.Text = Catalog.GetString("End Balance:");
      this.lblNumberMatched.Text = Catalog.GetString("Number Matched:");
      this.lblValueMatchedGifts.Text = Catalog.GetString("Value Matched Gifts:");
      this.lblValueMatchedGiftBatch.Text = Catalog.GetString("Value Matched Gift Batch:");
      this.lblNumberUnmatched.Text = Catalog.GetString("Number Unmatched:");
      this.lblValueUnmatchedGifts.Text = Catalog.GetString("Value Unmatched Gifts:");
      this.lblNumberOther.Text = Catalog.GetString("Number Other:");
      this.lblValueOtherCredit.Text = Catalog.GetString("Value Other Credit:");
      this.lblValueOtherDebit.Text = Catalog.GetString("Value Other Debit:");
      this.lblNumberAltogether.Text = Catalog.GetString("Number Altogether:");
      this.lblSumCredit.Text = Catalog.GetString("Sum Credit:");
      this.lblSumDebit.Text = Catalog.GetString("Sum Debit:");
      this.rbtAllTransactions.Text = Catalog.GetString("AllTransactions");
      this.rbtMatchedGifts.Text = Catalog.GetString("MatchedGifts");
      this.rbtUnmatchedGifts.Text = Catalog.GetString("UnmatchedGifts");
      this.rbtOther.Text = Catalog.GetString("Other");
      this.rgrFilter.Text = Catalog.GetString("Filter");
      this.btnExport.Text = Catalog.GetString("Export as CSV");
      this.btnPrint.Text = Catalog.GetString("Print");
      this.tbbSplitAndTrain.Text = Catalog.GetString("Split and Train");
      this.tbbSeparator0.Text = Catalog.GetString("Separator");
      this.tbbImportStatement.Text = Catalog.GetString("&Import Statement");
      this.tbbSeparator1.Text = Catalog.GetString("Separator");
      this.tbbProcessAllNewStatements.Text = Catalog.GetString("Create CSV files for all current bank statements");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("Bank import");
      #endregion

      FPetraUtilsObject = new Ict.Petra.Client.CommonForms.TFrmPetraUtils(AParentFormHandle, this, stbMain);
      InitializeManualCode();
      grdResult.Columns.Clear();
      grdResult.AddTextColumn("order", FMainDS.AEpTransaction.ColumnOrder);
      grdResult.AddTextColumn("transaction type", FMainDS.AEpTransaction.ColumnTransactionTypeCode);
      grdResult.AddTextColumn("Account Name", FMainDS.AEpTransaction.ColumnAccountName);
      grdResult.AddTextColumn("DonorKey", FMainDS.AEpTransaction.ColumnDonorKey);
      grdResult.AddTextColumn("DonorShortName", FMainDS.AEpTransaction.ColumnDonorShortName);
      grdResult.AddTextColumn("Account Number", FMainDS.AEpTransaction.ColumnBankAccountNumber);
      grdResult.AddTextColumn("description", FMainDS.AEpTransaction.ColumnDescription);
      grdResult.AddTextColumn("Recipient", FMainDS.AEpTransaction.ColumnRecipientDescription);
      grdResult.AddTextColumn("Transaction Amount", FMainDS.AEpTransaction.ColumnTransactionAmount);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();

    }

    private void TFrmPetra_Activated(object sender, EventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Activated(sender, e);
    }

    private void TFrmPetra_Load(object sender, EventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Load(sender, e);
    }

    private void TFrmPetra_Closing(object sender, CancelEventArgs e)
    {
        FPetraUtilsObject.TFrmPetra_Closing(sender, e);
    }

    private void Form_KeyDown(object sender, KeyEventArgs e)
    {
        FPetraUtilsObject.Form_KeyDown(sender, e);
    }

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position

    }

#region Implement interface functions

    /// auto generated
    public void RunOnceOnActivation()
    {
    }

    /// <summary>
    /// Adds event handlers for the appropiate onChange event to call a central procedure
    /// </summary>
    public void HookupAllControls()
    {
    }

    /// auto generated
    public void HookupAllInContainer(Control container)
    {
        FPetraUtilsObject.HookupAllInContainer(container);
    }

    /// auto generated
    public bool CanClose()
    {
        return FPetraUtilsObject.CanClose();
    }

    /// auto generated
    public TFrmPetraUtils GetPetraUtilsObject()
    {
        return (TFrmPetraUtils)FPetraUtilsObject;
    }
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actExport")
        {
            btnExport.Enabled = e.Enabled;
        }
        if (e.ActionName == "actPrint")
        {
            btnPrint.Enabled = e.Enabled;
        }
        if (e.ActionName == "actSplitAndTrain")
        {
            tbbSplitAndTrain.Enabled = e.Enabled;
        }
        if (e.ActionName == "actImportStatement")
        {
            tbbImportStatement.Enabled = e.Enabled;
        }
        if (e.ActionName == "actProcessAllNewStatements")
        {
            tbbProcessAllNewStatements.Enabled = e.Enabled;
        }
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        mniHelpPetraHelp.Enabled = false;
        mniHelpBugReport.Enabled = false;
        mniHelpAboutPetra.Enabled = false;
        mniHelpDevelopmentTeam.Enabled = false;
    }

    /// auto generated
    protected void actClose(object sender, EventArgs e)
    {
        FPetraUtilsObject.ExecuteAction(eActionId.eClose);
    }

#endregion
  }
}
