/* auto generated with nant generateWinforms from GLAccountHierarchy.yaml and template windowTDS
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
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace Ict.Petra.Client.MFinance.Gui.GL
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
      this.lblDetailAccountCode.Text = Catalog.GetString("Detail Account Code:");
      this.lblDetailAccountType.Text = Catalog.GetString("Detail Account Type:");
      this.lblDetailAccountCodeLongDesc.Text = Catalog.GetString("Description Long:");
      this.lblDetailAccountCodeShortDesc.Text = Catalog.GetString("Description Short:");
      this.lblDetailValidCcCombo.Text = Catalog.GetString("Valid Cost Centres:");
      this.chkDetailAccountActiveFlag.Text = Catalog.GetString("Active");
      this.tbbSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.tbbSave.Text = Catalog.GetString("&Save");
      this.tbbAddNewAccount.Text = Catalog.GetString("Add Account");
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
      this.mniHelpPetraHelp.Text = Catalog.GetString("&Petra Help");
      this.mniHelpBugReport.Text = Catalog.GetString("Bug &Report");
      this.mniHelpAboutPetra.Text = Catalog.GetString("&About Petra");
      this.mniHelpDevelopmentTeam.Text = Catalog.GetString("&The Development Team...");
      this.mniHelp.Text = Catalog.GetString("&Help");
      this.Text = Catalog.GetString("GL Account Hierarchy");
      #endregion

      FPetraUtilsObject = new TFrmPetraEditUtils(AParentFormHandle, this, stbMain);
      FMainDS = new Ict.Petra.Shared.MFinance.GL.Data.GLSetupTDS();
      FPetraUtilsObject.SetStatusBarText(txtDetailAccountCode, Catalog.GetString("Enter a code for the account."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailAccountType, Catalog.GetString("Choose the type of account (e.g., Asset)"));
      FPetraUtilsObject.SetStatusBarText(txtDetailAccountCodeLongDesc, Catalog.GetString("Enter a description of the account (full)."));
      FPetraUtilsObject.SetStatusBarText(txtDetailAccountCodeShortDesc, Catalog.GetString("Enter a short description of the account."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailValidCcCombo, Catalog.GetString("Select cost centre type that may be combined with this account."));
      FPetraUtilsObject.SetStatusBarText(chkDetailAccountActiveFlag, Catalog.GetString("Is this account available for posting transactions?"));
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

    private void ShowDetails(Int32 ACurrentDetailIndex)
    {
        txtDetailAccountCode.Text = FMainDS.AAccount[ACurrentDetailIndex].AccountCode;
        if (FMainDS.AAccount[ACurrentDetailIndex].IsAccountTypeNull())
        {
            cmbDetailAccountType.SelectedIndex = -1;
        }
        else
        {
            cmbDetailAccountType.SetSelectedString(FMainDS.AAccount[ACurrentDetailIndex].AccountType);
        }
        if (FMainDS.AAccount[ACurrentDetailIndex].IsAccountCodeLongDescNull())
        {
            txtDetailAccountCodeLongDesc.Text = String.Empty;
        }
        else
        {
            txtDetailAccountCodeLongDesc.Text = FMainDS.AAccount[ACurrentDetailIndex].AccountCodeLongDesc;
        }
        if (FMainDS.AAccount[ACurrentDetailIndex].IsAccountCodeShortDescNull())
        {
            txtDetailAccountCodeShortDesc.Text = String.Empty;
        }
        else
        {
            txtDetailAccountCodeShortDesc.Text = FMainDS.AAccount[ACurrentDetailIndex].AccountCodeShortDesc;
        }
        if (FMainDS.AAccount[ACurrentDetailIndex].IsValidCcComboNull())
        {
            cmbDetailValidCcCombo.SelectedIndex = -1;
        }
        else
        {
            cmbDetailValidCcCombo.SetSelectedString(FMainDS.AAccount[ACurrentDetailIndex].ValidCcCombo);
        }
        if (FMainDS.AAccount[ACurrentDetailIndex].IsAccountActiveFlagNull())
        {
            chkDetailAccountActiveFlag.Checked = false;
        }
        else
        {
            chkDetailAccountActiveFlag.Checked = FMainDS.AAccount[ACurrentDetailIndex].AccountActiveFlag;
        }
    }

    private void GetDataFromControls()
    {
        GetDataFromControlsManual();
    }

    private void GetDetailsFromControls(Int32 ACurrentDetailIndex)
    {
        if (ACurrentDetailIndex != -1)
        {
            FMainDS.AAccount[ACurrentDetailIndex].AccountCode = txtDetailAccountCode.Text;
            if (cmbDetailAccountType.SelectedIndex == -1)
            {
                FMainDS.AAccount[ACurrentDetailIndex].SetAccountTypeNull();
            }
            else
            {
                FMainDS.AAccount[ACurrentDetailIndex].AccountType = cmbDetailAccountType.GetSelectedString();
            }
            if (txtDetailAccountCodeLongDesc.Text.Length == 0)
            {
                FMainDS.AAccount[ACurrentDetailIndex].SetAccountCodeLongDescNull();
            }
            else
            {
                FMainDS.AAccount[ACurrentDetailIndex].AccountCodeLongDesc = txtDetailAccountCodeLongDesc.Text;
            }
            if (txtDetailAccountCodeShortDesc.Text.Length == 0)
            {
                FMainDS.AAccount[ACurrentDetailIndex].SetAccountCodeShortDescNull();
            }
            else
            {
                FMainDS.AAccount[ACurrentDetailIndex].AccountCodeShortDesc = txtDetailAccountCodeShortDesc.Text;
            }
            if (cmbDetailValidCcCombo.SelectedIndex == -1)
            {
                FMainDS.AAccount[ACurrentDetailIndex].SetValidCcComboNull();
            }
            else
            {
                FMainDS.AAccount[ACurrentDetailIndex].ValidCcCombo = cmbDetailValidCcCombo.GetSelectedString();
            }
            if (chkDetailAccountActiveFlag.Checked)
            {
                FMainDS.AAccount[ACurrentDetailIndex].SetAccountActiveFlagNull();
            }
            else
            {
                FMainDS.AAccount[ACurrentDetailIndex].AccountActiveFlag = chkDetailAccountActiveFlag.Checked;
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
        SaveChanges();
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

            if (FPetraUtilsObject.HasChanges)
            {
                FPetraUtilsObject.WriteToStatusBar("Saving data...");
                this.Cursor = Cursors.WaitCursor;

                TSubmitChangesResult SubmissionResult;
                TVerificationResultCollection VerificationResult;

                Ict.Petra.Shared.MFinance.GL.Data.GLSetupTDS SubmitDS = FMainDS.GetChangesTyped(true);

                // Submit changes to the PETRAServer
                try
                {
                    SubmissionResult = TRemote.MFinance.GL.WebConnectors.SaveGLSetupTDS(ref SubmitDS, out VerificationResult);
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
                    bool ReturnValue = false;

                    // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
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

                        // TODO OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        return true;

                    case TSubmitChangesResult.scrError:

                        // TODO scrError
                        break;

                    case TSubmitChangesResult.scrNothingToBeSaved:

                        // TODO scrNothingToBeSaved
                        break;

                    case TSubmitChangesResult.scrInfoNeeded:

                        // TODO scrInfoNeeded
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
