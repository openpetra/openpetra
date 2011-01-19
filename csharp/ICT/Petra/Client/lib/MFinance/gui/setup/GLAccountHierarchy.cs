// auto generated with nant generateWinforms from GLAccountHierarchy.yaml and template windowTDS
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
//
// Copyright 2004-2011 by OM International
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{

  /// auto generated: GL Account Hierarchy
  public partial class TFrmGLAccountHierarchy: System.Windows.Forms.Form, IFrmPetraEdit
  {
    private TFrmPetraEditUtils FPetraUtilsObject;
    private Ict.Petra.Shared.MFinance.GL.Data.GLSetupTDS FMainDS;

    /// constructor
    public TFrmGLAccountHierarchy(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblDetailAccountCode.Text = Catalog.GetString("Account Code:");
      this.lblDetailAccountType.Text = Catalog.GetString("Account Type:");
      this.lblDetailEngAccountCodeLongDesc.Text = Catalog.GetString("Description Long English:");
      this.lblDetailEngAccountCodeShortDesc.Text = Catalog.GetString("Description Short English:");
      this.lblDetailAccountCodeLongDesc.Text = Catalog.GetString("Description Long Local:");
      this.lblDetailAccountCodeShortDesc.Text = Catalog.GetString("Description Short Local:");
      this.lblDetailValidCcCombo.Text = Catalog.GetString("Valid Cost Centres:");
      this.lblDetailBankAccountFlag.Text = Catalog.GetString("Bank Account:");
      this.lblDetailAccountActiveFlag.Text = Catalog.GetString("Active:");
      this.chkDetailForeignCurrencyFlag.Text = Catalog.GetString("Foreign Currency");
      this.tbbSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.tbbSave.Text = Catalog.GetString("&Save");
      this.tbbAddNewAccount.Text = Catalog.GetString("Add Account");
      this.tbbDeleteUnusedAccount.Text = Catalog.GetString("Delete Account");
      this.tbbExportHierarchy.Text = Catalog.GetString("Export Hierarchy");
      this.tbbImportHierarchy.Text = Catalog.GetString("Import Hierarchy");
      this.mniFileSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.mniFileSave.Text = Catalog.GetString("&Save");
      this.mniFilePrint.Text = Catalog.GetString("&Print...");
      this.mniClose.ToolTipText = Catalog.GetString("Closes this window");
      this.mniClose.Text = Catalog.GetString("&Close");
      this.mniFile.Text = Catalog.GetString("&File");
      this.mniEditUndoCurrentField.Text = Catalog.GetString("Undo &Current Field");
      this.mniEditUndoScreen.Text = Catalog.GetString("&Undo Screen");
      this.mniEditFind.Text = Catalog.GetString("&Find...");
      this.mniEdit.Text = Catalog.GetString("&Edit");
      this.mniAddNewAccount.Text = Catalog.GetString("Add Account");
      this.mniDeleteUnusedAccount.Text = Catalog.GetString("Delete Account");
      this.mniSeparator3.Text = Catalog.GetString("Separator");
      this.mniExportHierarchy.Text = Catalog.GetString("Export Hierarchy");
      this.mniImportHierarchy.Text = Catalog.GetString("Import Hierarchy");
      this.mniAccounts.Text = Catalog.GetString("Accounts");
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("GL Account Hierarchy");
      #endregion

      this.txtDetailAccountCode.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailEngAccountCodeLongDesc.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailEngAccountCodeShortDesc.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailAccountCodeLongDesc.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailAccountCodeShortDesc.Font = TAppSettingsManager.GetDefaultBoldFont();

      FPetraUtilsObject = new TFrmPetraEditUtils(AParentFormHandle, this, stbMain);
      FMainDS = new Ict.Petra.Shared.MFinance.GL.Data.GLSetupTDS();
      FPetraUtilsObject.SetStatusBarText(txtDetailAccountCode, Catalog.GetString("Enter a code for the account."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailAccountType, Catalog.GetString("Choose the type of account (e.g., Asset)"));
      FPetraUtilsObject.SetStatusBarText(txtDetailEngAccountCodeLongDesc, Catalog.GetString("Enter a description in English (full)."));
      FPetraUtilsObject.SetStatusBarText(txtDetailEngAccountCodeShortDesc, Catalog.GetString("Enter a short description in English."));
      FPetraUtilsObject.SetStatusBarText(txtDetailAccountCodeLongDesc, Catalog.GetString("Enter a description of the account (full)."));
      FPetraUtilsObject.SetStatusBarText(txtDetailAccountCodeShortDesc, Catalog.GetString("Enter a short description of the account."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailValidCcCombo, Catalog.GetString("Select cost centre type that may be combined with this account."));
      FPetraUtilsObject.SetStatusBarText(chkDetailAccountActiveFlag, Catalog.GetString("Is this account available for posting transactions?"));
      FPetraUtilsObject.SetStatusBarText(cmbDetailForeignCurrencyCode, Catalog.GetString("Enter a currency code"));
      cmbDetailForeignCurrencyCode.InitialiseUserControl();
      ucoAccountAnalysisAttributes.PetraUtilsObject = FPetraUtilsObject;
      ucoAccountAnalysisAttributes.MainDS = FMainDS;
      ucoAccountAnalysisAttributes.InitUserControl();
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      FPetraUtilsObject.InitActionState();
      chkDetailForeignCurrencyFlagCheckedChanged(null, null);

    }

    void chkDetailForeignCurrencyFlagCheckedChanged(object sender, System.EventArgs e)
    {
      cmbDetailForeignCurrencyCode.Enabled = chkDetailForeignCurrencyFlag.Checked;
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

    private void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
        txtDetailAccountCode.ReadOnly = AReadOnly;
    }

    private void ShowDetails(GLSetupTDSAAccountRow ARow)
    {
        txtDetailAccountCode.Text = ARow.AccountCode;
        txtDetailAccountCode.ReadOnly = (ARow.RowState != DataRowState.Added);
        if (ARow.IsAccountTypeNull())
        {
            cmbDetailAccountType.SelectedIndex = -1;
        }
        else
        {
            cmbDetailAccountType.SetSelectedString(ARow.AccountType);
        }
        if (ARow.IsEngAccountCodeLongDescNull())
        {
            txtDetailEngAccountCodeLongDesc.Text = String.Empty;
        }
        else
        {
            txtDetailEngAccountCodeLongDesc.Text = ARow.EngAccountCodeLongDesc;
        }
        if (ARow.IsEngAccountCodeShortDescNull())
        {
            txtDetailEngAccountCodeShortDesc.Text = String.Empty;
        }
        else
        {
            txtDetailEngAccountCodeShortDesc.Text = ARow.EngAccountCodeShortDesc;
        }
        if (ARow.IsAccountCodeLongDescNull())
        {
            txtDetailAccountCodeLongDesc.Text = String.Empty;
        }
        else
        {
            txtDetailAccountCodeLongDesc.Text = ARow.AccountCodeLongDesc;
        }
        if (ARow.IsAccountCodeShortDescNull())
        {
            txtDetailAccountCodeShortDesc.Text = String.Empty;
        }
        else
        {
            txtDetailAccountCodeShortDesc.Text = ARow.AccountCodeShortDesc;
        }
        if (ARow.IsValidCcComboNull())
        {
            cmbDetailValidCcCombo.SelectedIndex = -1;
        }
        else
        {
            cmbDetailValidCcCombo.SetSelectedString(ARow.ValidCcCombo);
        }
        if (ARow.IsBankAccountFlagNull())
        {
            chkDetailBankAccountFlag.Checked = false;
        }
        else
        {
            chkDetailBankAccountFlag.Checked = ARow.BankAccountFlag;
        }
        if (ARow.IsAccountActiveFlagNull())
        {
            chkDetailAccountActiveFlag.Checked = false;
        }
        else
        {
            chkDetailAccountActiveFlag.Checked = ARow.AccountActiveFlag;
        }
        if (ARow.IsForeignCurrencyFlagNull())
        {
            chkDetailForeignCurrencyFlag.Checked = false;
        }
        else
        {
            chkDetailForeignCurrencyFlag.Checked = ARow.ForeignCurrencyFlag;
        }
        if (ARow.IsForeignCurrencyCodeNull())
        {
            cmbDetailForeignCurrencyCode.SelectedIndex = -1;
        }
        else
        {
            cmbDetailForeignCurrencyCode.SetSelectedString(ARow.ForeignCurrencyCode);
        }
        ShowDetailsManual(ARow);
    }

    private void GetDataFromControls()
    {
        GetDataFromControlsManual();
    }

    private void GetDetailsFromControls(GLSetupTDSAAccountRow ARow)
    {
        if (ARow != null)
        {
            ARow.AccountCode = txtDetailAccountCode.Text;
            if (cmbDetailAccountType.SelectedIndex == -1)
            {
                ARow.SetAccountTypeNull();
            }
            else
            {
                ARow.AccountType = cmbDetailAccountType.GetSelectedString();
            }
            if (txtDetailEngAccountCodeLongDesc.Text.Length == 0)
            {
                ARow.SetEngAccountCodeLongDescNull();
            }
            else
            {
                ARow.EngAccountCodeLongDesc = txtDetailEngAccountCodeLongDesc.Text;
            }
            if (txtDetailEngAccountCodeShortDesc.Text.Length == 0)
            {
                ARow.SetEngAccountCodeShortDescNull();
            }
            else
            {
                ARow.EngAccountCodeShortDesc = txtDetailEngAccountCodeShortDesc.Text;
            }
            if (txtDetailAccountCodeLongDesc.Text.Length == 0)
            {
                ARow.SetAccountCodeLongDescNull();
            }
            else
            {
                ARow.AccountCodeLongDesc = txtDetailAccountCodeLongDesc.Text;
            }
            if (txtDetailAccountCodeShortDesc.Text.Length == 0)
            {
                ARow.SetAccountCodeShortDescNull();
            }
            else
            {
                ARow.AccountCodeShortDesc = txtDetailAccountCodeShortDesc.Text;
            }
            if (cmbDetailValidCcCombo.SelectedIndex == -1)
            {
                ARow.SetValidCcComboNull();
            }
            else
            {
                ARow.ValidCcCombo = cmbDetailValidCcCombo.GetSelectedString();
            }
            ARow.BankAccountFlag = chkDetailBankAccountFlag.Checked;
            ARow.AccountActiveFlag = chkDetailAccountActiveFlag.Checked;
            ARow.ForeignCurrencyFlag = chkDetailForeignCurrencyFlag.Checked;
            if (cmbDetailForeignCurrencyCode.SelectedIndex == -1)
            {
                ARow.SetForeignCurrencyCodeNull();
            }
            else
            {
                ARow.ForeignCurrencyCode = cmbDetailForeignCurrencyCode.GetSelectedString();
            }
        }
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

    /// auto generated
    public void FileSave(object sender, EventArgs e)
    {
      try {
         SaveChanges();
      } catch (CancelSaveException) {}
    }

    /// <summary>
    /// save the changes on the screen
    /// </summary>
    /// <returns></returns>
    public bool SaveChanges()
    {
        FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());

//TODO?  still needed?      FMainDS.AApDocument.Rows[0].BeginEdit();
        GetDataFromControls();

        // TODO: verification

        if (FPetraUtilsObject.VerificationResultCollection.Count == 0)
        {
            foreach (DataTable InspectDT in FMainDS.Tables)
            {
                foreach (DataRow InspectDR in InspectDT.Rows)
                {
                    InspectDR.EndEdit();
                }
            }

            if (!FPetraUtilsObject.HasChanges)
            {
                return true;
            }
            else
            {
                FPetraUtilsObject.WriteToStatusBar("Saving data...");
                this.Cursor = Cursors.WaitCursor;

                TSubmitChangesResult SubmissionResult;
                TVerificationResultCollection VerificationResult;

                Ict.Petra.Shared.MFinance.GL.Data.GLSetupTDS SubmitDS = FMainDS.GetChangesTyped(true);

                if (SubmitDS == null)
                {
                    // There is nothing to be saved.
                    // Update UI
                    FPetraUtilsObject.WriteToStatusBar(Catalog.GetString("There is nothing to be saved."));
                    this.Cursor = Cursors.Default;

                    // We don't have unsaved changes anymore
                    FPetraUtilsObject.DisableSaveButton();

                    return true;
                }

                // Submit changes to the PETRAServer
                try
                {
                    SubmissionResult = StoreManualCode(ref SubmitDS, out VerificationResult);
                }
                catch (System.Net.Sockets.SocketException)
                {
                    FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("The PETRA Server cannot be reached! Data cannot be saved!",
                        "No Server response",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    bool ReturnValue = false;

                    // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
/* TODO ESecurityDBTableAccessDeniedException
*                  catch (ESecurityDBTableAccessDeniedException Exp)
*                  {
*                      FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
*                      this.Cursor = Cursors.Default;
*                      // TODO TMessages.MsgSecurityException(Exp, this.GetType());
*                      bool ReturnValue = false;
*                      // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
*                      return ReturnValue;
*                  }
*/
                catch (EDBConcurrencyException)
                {
                    FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                    this.Cursor = Cursors.Default;

                    // TODO TMessages.MsgDBConcurrencyException(Exp, this.GetType());
                    bool ReturnValue = false;

                    // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
                catch (Exception exp)
                {
                    FPetraUtilsObject.WriteToStatusBar("Data could not be saved!");
                    this.Cursor = Cursors.Default;
                    TLogging.Log(
                        "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine + exp.ToString(),
                        TLoggingType.ToLogfile);
                    MessageBox.Show(
                        "An error occured while trying to connect to the PETRA Server!" + Environment.NewLine +
                        "For details see the log file: " + TLogging.GetLogFileName(),
                        "Server connection error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);

                    // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return false;
                }

                switch (SubmissionResult)
                {
                    case TSubmitChangesResult.scrOK:

                        // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                        FMainDS.AcceptChanges();

                        // Merge back with data from the Server (eg. for getting Sequence values)
                        FMainDS.Merge(SubmitDS, false);

                        // need to accept the new modification ID
                        FMainDS.AcceptChanges();

                        // Update UI
                        FPetraUtilsObject.WriteToStatusBar("Data successfully saved.");
                        this.Cursor = Cursors.Default;

                        // TODO EnableSave(false);

                        // We don't have unsaved changes anymore
                        FPetraUtilsObject.DisableSaveButton();

                        SetPrimaryKeyReadOnly(true);

                        // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return true;

                    case TSubmitChangesResult.scrError:

                        // TODO scrError
                        this.Cursor = Cursors.Default;
                        break;

                    case TSubmitChangesResult.scrNothingToBeSaved:

                        // TODO scrNothingToBeSaved
                        this.Cursor = Cursors.Default;
                        return true;

                    case TSubmitChangesResult.scrInfoNeeded:

                        // TODO scrInfoNeeded
                        this.Cursor = Cursors.Default;
                        break;
                }
            }
        }

        return false;
    }
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        if (e.ActionName == "actSave")
        {
            tbbSave.Enabled = e.Enabled;
            mniFileSave.Enabled = e.Enabled;
        }
        if (e.ActionName == "actAddNewAccount")
        {
            tbbAddNewAccount.Enabled = e.Enabled;
            mniAddNewAccount.Enabled = e.Enabled;
        }
        if (e.ActionName == "actDeleteUnusedAccount")
        {
            tbbDeleteUnusedAccount.Enabled = e.Enabled;
            mniDeleteUnusedAccount.Enabled = e.Enabled;
        }
        if (e.ActionName == "actExportHierarchy")
        {
            tbbExportHierarchy.Enabled = e.Enabled;
            mniExportHierarchy.Enabled = e.Enabled;
        }
        if (e.ActionName == "actImportHierarchy")
        {
            tbbImportHierarchy.Enabled = e.Enabled;
            mniImportHierarchy.Enabled = e.Enabled;
        }
        if (e.ActionName == "actClose")
        {
            mniClose.Enabled = e.Enabled;
        }
        mniFilePrint.Enabled = false;
        mniEditUndoCurrentField.Enabled = false;
        mniEditUndoScreen.Enabled = false;
        mniEditFind.Enabled = false;
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
