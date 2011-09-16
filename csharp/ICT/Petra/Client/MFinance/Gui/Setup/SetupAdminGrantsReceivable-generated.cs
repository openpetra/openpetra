// auto generated with nant generateWinforms from SetupAdminGrantsReceivable.yaml and template windowMaintainTable
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

namespace Ict.Petra.Client.MFinance.Gui.Setup
{

  /// auto generated: Maintain Receivable Administration Grants
  public partial class TFrmSetupAdminGrantsReceivable: System.Windows.Forms.Form, IFrmPetraEdit
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    private class FMainDS
    {
        public static AFeesReceivableTable AFeesReceivable;
    }

    /// constructor
    public TFrmSetupAdminGrantsReceivable(IntPtr AParentFormHandle) : base()
    {
      Control[] FoundCheckBoxes;

      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.btnNew.Text = Catalog.GetString("&New");
      this.lblDetailFeeCode.Text = Catalog.GetString("Fee Code:");
      this.lblDetailFeeDescription.Text = Catalog.GetString("Description:");
      this.lblDetailChargeOption.Text = Catalog.GetString("Charge Option:");
      this.lblDetailChargePercentage.Text = Catalog.GetString("Charge Percentage:");
      this.lblDetailChargeAmount.Text = Catalog.GetString("Charge Amount:");
      this.lblToBeDebited.Text = Catalog.GetString("To Be Debited:");
      this.lblToBeCredited.Text = Catalog.GetString("To Be Credited:");
      this.txtForeignReceivingFund.Text = Catalog.GetString("Foreign Receiving Fund");
      this.lblForeignReceivingFund.Text = Catalog.GetString("Cost Centre:");
      this.lblDetailDrAccountCode.Text = Catalog.GetString("Account:");
      this.grpAssignment.Text = Catalog.GetString("Assignment");
      this.tbbSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.tbbSave.Text = Catalog.GetString("&Save");
      this.tbbNew.Text = Catalog.GetString("New Receivable Admin. Grant");
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
      this.Text = Catalog.GetString("Maintain Receivable Administration Grants");
      #endregion

      this.txtDetailFeeCode.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailFeeDescription.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtForeignReceivingFund.Font = TAppSettingsManager.GetDefaultBoldFont();

      FPetraUtilsObject = new TFrmPetraEditUtils(AParentFormHandle, this, stbMain);
      FPetraUtilsObject.SetStatusBarText(txtDetailFeeCode, Catalog.GetString("Enter a code for this fee"));
      FPetraUtilsObject.SetStatusBarText(txtDetailFeeDescription, Catalog.GetString("Enter a description of what this fee is for."));
      FPetraUtilsObject.SetStatusBarText(txtDetailChargeAmount, Catalog.GetString("Enter the amount"));
      FPetraUtilsObject.SetStatusBarText(cmbDetailCostCentreCode, Catalog.GetString("Enter a cost centre code"));
      FPetraUtilsObject.SetStatusBarText(cmbDetailDrAccountCode, Catalog.GetString("Selecct an expense account code to debit for this fee."));
      FPetraUtilsObject.SetStatusBarText(cmbDetailAccountCode, Catalog.GetString("Enter an account code"));
      FMainDS.AFeesReceivable = new AFeesReceivableTable();
      Ict.Common.Data.TTypedDataTable TypedTable;
      TRemote.MCommon.DataReader.GetData(AFeesReceivableTable.GetTableDBName(), null, out TypedTable);
      FMainDS.AFeesReceivable.Merge(TypedTable);
      grdDetails.Columns.Clear();
      grdDetails.AddTextColumn("Fee Code", FMainDS.AFeesReceivable.ColumnFeeCode);
      grdDetails.AddTextColumn("Description", FMainDS.AFeesReceivable.ColumnFeeDescription);
      grdDetails.AddTextColumn("Charge Option", FMainDS.AFeesReceivable.ColumnChargeOption);
      grdDetails.AddCurrencyColumn("Charge Percentage", FMainDS.AFeesReceivable.ColumnChargePercentage);
      grdDetails.AddCurrencyColumn("Charge Amount", FMainDS.AFeesReceivable.ColumnChargeAmount);
      grdDetails.AddTextColumn("DR Account Code", FMainDS.AFeesReceivable.ColumnDrAccountCode);
      grdDetails.AddTextColumn("Cost Centre Code", FMainDS.AFeesReceivable.ColumnCostCentreCode);
      grdDetails.AddTextColumn("Account Code", FMainDS.AFeesReceivable.ColumnAccountCode);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      DataView myDataView = FMainDS.AFeesReceivable.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

      // Ensure that the Details Panel is disabled if there are no records
      if (FMainDS.AFeesReceivable.Rows.Count == 0)
      {
        ShowDetails(null);
      }

      FPetraUtilsObject.InitActionState();

      /*
       * Automatically disable 'Deletable' CheckBox (it must not get changed by the user because records where the
       * 'Deletable' flag is true are system records that must not be deleted)
       */
      FoundCheckBoxes = this.Controls.Find("chkDetailDeletable", true);

      if (FoundCheckBoxes.Length > 0)
      {
          FoundCheckBoxes[0].Enabled = false;
      }
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

    /// automatically generated, create a new record of AFeesReceivable and display on the edit screen
    /// we create the table locally, no dataset
    public bool CreateNewAFeesReceivable()
    {
        AFeesReceivableRow NewRow = FMainDS.AFeesReceivable.NewRowTyped();
        NewRowManual(ref NewRow);
        FMainDS.AFeesReceivable.Rows.Add(NewRow);

        FPetraUtilsObject.SetChangedFlag();

        grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AFeesReceivable.DefaultView);
        grdDetails.Refresh();
        SelectDetailRowByDataTableIndex(FMainDS.AFeesReceivable.Rows.Count - 1);

        return true;
    }

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.AFeesReceivable.PrimaryKey)
            {
                string value1 = FMainDS.AFeesReceivable.Rows[ARowNumberInTable][myColumn].ToString();
                string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[Counter][myColumn.Ordinal].ToString();
                if (value1 != value2)
                {
                    found = false;
                }
            }
            if (found)
            {
                RowNumberGrid = Counter + 1;
            }
        }
        grdDetails.Selection.ResetSelection(false);
        grdDetails.Selection.SelectRow(RowNumberGrid, true);
        // scroll to the row
        grdDetails.ShowCell(new SourceGrid.Position(RowNumberGrid, 0), true);

        FocusedRowChanged(this, new SourceGrid.RowEventArgs(RowNumberGrid));
    }

    /// return the selected row
    private AFeesReceivableRow GetSelectedDetailRow()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            return (AFeesReceivableRow)SelectedGridRow[0].Row;
        }

        return null;
    }

    private void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
        txtDetailFeeCode.ReadOnly = AReadOnly;
    }

    private void ShowDetails(AFeesReceivableRow ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        if (ARow == null)
        {
            pnlDetails.Enabled = false;
        }
        else
        {
            FPreviouslySelectedDetailRow = ARow;
            txtDetailFeeCode.Text = ARow.FeeCode;
            txtDetailFeeCode.ReadOnly = (ARow.RowState != DataRowState.Added);
            if (ARow.IsFeeDescriptionNull())
            {
                txtDetailFeeDescription.Text = String.Empty;
            }
            else
            {
                txtDetailFeeDescription.Text = ARow.FeeDescription;
            }
            cmbDetailChargeOption.SetSelectedString(ARow.ChargeOption);
            if (ARow.IsChargePercentageNull())
            {
                txtDetailChargePercentage.NumberValueDecimal = null;
            }
            else
            {
                txtDetailChargePercentage.NumberValueDecimal = Convert.ToDecimal(ARow.ChargePercentage);
            }
            if (ARow.IsChargeAmountNull())
            {
                txtDetailChargeAmount.NumberValueDecimal = null;
            }
            else
            {
                txtDetailChargeAmount.NumberValueDecimal = Convert.ToDecimal(ARow.ChargeAmount);
            }
            cmbDetailCostCentreCode.SetSelectedString(ARow.CostCentreCode);
            cmbDetailDrAccountCode.SetSelectedString(ARow.DrAccountCode);
            cmbDetailAccountCode.SetSelectedString(ARow.AccountCode);
            pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private AFeesReceivableRow FPreviouslySelectedDetailRow = null;
    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
        // get the details from the previously selected row
        if (FPreviouslySelectedDetailRow != null)
        {
            GetDetailsFromControls(FPreviouslySelectedDetailRow);
        }
        // display the details of the currently selected row
        FPreviouslySelectedDetailRow = GetSelectedDetailRow();
        ShowDetails(FPreviouslySelectedDetailRow);
        pnlDetails.Enabled = true;
    }

    private void GetDetailsFromControls(AFeesReceivableRow ARow)
    {
        if (ARow != null)
        {
            ARow.BeginEdit();
            ARow.FeeCode = txtDetailFeeCode.Text;
            if (txtDetailFeeDescription.Text.Length == 0)
            {
                ARow.SetFeeDescriptionNull();
            }
            else
            {
                ARow.FeeDescription = txtDetailFeeDescription.Text;
            }
            ARow.ChargeOption = cmbDetailChargeOption.GetSelectedString();
            if (txtDetailChargePercentage.NumberValueDecimal == null)
            {
                ARow.SetChargePercentageNull();
            }
            else
            {
                ARow.ChargePercentage = Convert.ToDecimal(txtDetailChargePercentage.NumberValueDecimal);
            }
            if (txtDetailChargeAmount.NumberValueDecimal == null)
            {
                ARow.SetChargeAmountNull();
            }
            else
            {
                ARow.ChargeAmount = Convert.ToDecimal(txtDetailChargeAmount.NumberValueDecimal);
            }
            ARow.CostCentreCode = cmbDetailCostCentreCode.GetSelectedString();
            ARow.DrAccountCode = cmbDetailDrAccountCode.GetSelectedString();
            ARow.AccountCode = cmbDetailAccountCode.GetSelectedString();
            ARow.EndEdit();
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
        GetDetailsFromControls(FPreviouslySelectedDetailRow);

        // TODO: verification

        if (FPetraUtilsObject.VerificationResultCollection.Count == 0)
        {
            foreach (DataRow InspectDR in FMainDS.AFeesReceivable.Rows)
            {
                InspectDR.EndEdit();
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

                Ict.Common.Data.TTypedDataTable SubmitDT = FMainDS.AFeesReceivable.GetChangesTyped();

                if (SubmitDT == null)
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
                    SubmissionResult = TRemote.MCommon.DataReader.SaveData(AFeesReceivableTable.GetTableDBName(), ref SubmitDT, out VerificationResult);
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
                        FMainDS.AFeesReceivable.AcceptChanges();

                        // Merge back with data from the Server (eg. for getting Sequence values)
                        FMainDS.AFeesReceivable.Merge(SubmitDT, false);

                        // need to accept the new modification ID
                        FMainDS.AFeesReceivable.AcceptChanges();

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
        if (e.ActionName == "actNew")
        {
            btnNew.Enabled = e.Enabled;
            tbbNew.Enabled = e.Enabled;
        }
        if (e.ActionName == "actSave")
        {
            tbbSave.Enabled = e.Enabled;
            mniFileSave.Enabled = e.Enabled;
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
