/* auto generated with nant generateWinforms from GiftMotivationSetup.yaml and template windowEdit
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
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Client.MFinance.Gui.Gift
{

  /// auto generated: Gift Motivations
  public partial class TFrmGiftMotivationSetup: System.Windows.Forms.Form, IFrmPetraEdit
  {
    private TFrmPetraEditUtils FPetraUtilsObject;
    private Ict.Petra.Shared.MFinance.Gift.Data.GiftBatchTDS FMainDS;

    /// constructor
    public TFrmGiftMotivationSetup(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.btnAddMotivationDetail.Text = Catalog.GetString("Add Detail");
      this.lblDetailMotivationGroupCode.Text = Catalog.GetString("Group:");
      this.lblDetailMotivationDetailCode.Text = Catalog.GetString("Detail:");
      this.lblDetailMotivationDetailDesc.Text = Catalog.GetString("Description:");
      this.lblDetailAccountCode.Text = Catalog.GetString("Account:");
      this.lblDetailCostCentreCode.Text = Catalog.GetString("Cost Centre:");
      this.chkDetailMotivationStatus.Text = Catalog.GetString("Active");
      this.chkDetailReceipt.Text = Catalog.GetString("Print Receipt");
      this.tbbSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.tbbSave.Text = Catalog.GetString("&Save");
      this.tbbAddMotivationDetail.Text = Catalog.GetString("Add Detail");
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
      this.Text = Catalog.GetString("Gift Motivations");
      #endregion

      this.txtDetailMotivationGroupCode.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailMotivationDetailCode.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailMotivationDetailDesc.Font = TAppSettingsManager.GetDefaultBoldFont();

      FPetraUtilsObject = new TFrmPetraEditUtils(AParentFormHandle, this, stbMain);
      FPetraUtilsObject.SetStatusBarText(txtDetailMotivationGroupCode, Catalog.GetString("Enter a motivation group code"));
      FPetraUtilsObject.SetStatusBarText(txtDetailMotivationDetailCode, Catalog.GetString("Enter a motivation detail code"));
      FPetraUtilsObject.SetStatusBarText(txtDetailMotivationDetailDesc, Catalog.GetString("Enter a description"));
      FPetraUtilsObject.SetStatusBarText(cmbDetailAccountCode, Catalog.GetString("Enter an account code"));
      FPetraUtilsObject.SetStatusBarText(cmbDetailCostCentreCode, Catalog.GetString("Enter a cost centre code"));
      FPetraUtilsObject.SetStatusBarText(chkDetailMotivationStatus, Catalog.GetString("Is this motivation code still in use?"));
      FPetraUtilsObject.SetStatusBarText(chkDetailReceipt, Catalog.GetString("Do you want receipts for gifts with this motivation code?"));
      FMainDS = new Ict.Petra.Shared.MFinance.Gift.Data.GiftBatchTDS();
      grdDetails.Columns.Clear();
      grdDetails.AddTextColumn("Motivation Group Code", FMainDS.AMotivationDetail.ColumnMotivationGroupCode);
      grdDetails.AddTextColumn("Motivation Detail Code", FMainDS.AMotivationDetail.ColumnMotivationDetailCode);
      grdDetails.AddTextColumn("Detail Description", FMainDS.AMotivationDetail.ColumnMotivationDetailDesc);
      grdDetails.AddTextColumn("Account Code", FMainDS.AMotivationDetail.ColumnAccountCode);
      grdDetails.AddTextColumn("Cost Centre Code", FMainDS.AMotivationDetail.ColumnCostCentreCode);
      grdDetails.AddCheckBoxColumn("Motivation Status", FMainDS.AMotivationDetail.ColumnMotivationStatus);
      grdDetails.AddCheckBoxColumn("Print Receipt", FMainDS.AMotivationDetail.ColumnReceipt);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      DataView myDataView = FMainDS.AMotivationDetail.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
      grdDetails.AutoSizeCells();

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

    /// automatically generated, create a new record of AMotivationDetail and display on the edit screen
    /// we create the table locally, no dataset
    public bool CreateNewAMotivationDetail()
    {
        AMotivationDetailRow NewRow = FMainDS.AMotivationDetail.NewRowTyped();
        NewRowManual(ref NewRow);
        FMainDS.AMotivationDetail.Rows.Add(NewRow);

        FPetraUtilsObject.SetChangedFlag();

        grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AMotivationDetail.DefaultView);
        grdDetails.Refresh();
        SelectDetailRowByDataTableIndex(FMainDS.AMotivationDetail.Rows.Count - 1);

        return true;
    }

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.AMotivationDetail.PrimaryKey)
            {
                string value1 = FMainDS.AMotivationDetail.Rows[ARowNumberInTable][myColumn].ToString();
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
    private AMotivationDetailRow GetSelectedDetailRow()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            return (AMotivationDetailRow)SelectedGridRow[0].Row;
        }

        return null;
    }

    private void ShowDetails(AMotivationDetailRow ARow)
    {
        txtDetailMotivationGroupCode.Text = ARow.MotivationGroupCode;
        txtDetailMotivationGroupCode.ReadOnly = (ARow.RowState != DataRowState.Added);
        txtDetailMotivationDetailCode.Text = ARow.MotivationDetailCode;
        txtDetailMotivationDetailCode.ReadOnly = (ARow.RowState != DataRowState.Added);
        txtDetailMotivationDetailDesc.Text = ARow.MotivationDetailDesc;
        cmbDetailAccountCode.SetSelectedString(ARow.AccountCode);
        cmbDetailCostCentreCode.SetSelectedString(ARow.CostCentreCode);
        chkDetailMotivationStatus.Checked = ARow.MotivationStatus;
        if (ARow.IsReceiptNull())
        {
            chkDetailReceipt.Checked = false;
        }
        else
        {
            chkDetailReceipt.Checked = ARow.Receipt;
        }
    }

    private AMotivationDetailRow FPreviouslySelectedDetailRow = null;
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

    private void GetDetailsFromControls(AMotivationDetailRow ARow)
    {
        if (ARow != null)
        {
            ARow.MotivationGroupCode = txtDetailMotivationGroupCode.Text;
            ARow.MotivationDetailCode = txtDetailMotivationDetailCode.Text;
            ARow.MotivationDetailDesc = txtDetailMotivationDetailDesc.Text;
            ARow.AccountCode = cmbDetailAccountCode.GetSelectedString();
            ARow.CostCentreCode = cmbDetailCostCentreCode.GetSelectedString();
            ARow.MotivationStatus = chkDetailMotivationStatus.Checked;
            ARow.Receipt = chkDetailReceipt.Checked;
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

                Ict.Petra.Shared.MFinance.Gift.Data.GiftBatchTDS SubmitDS = FMainDS.GetChangesTyped(true);

                if (SubmitDS == null)
                {
                    // nothing to be saved, so it is ok to close the screen etc
                    return true;
                }

                // Submit changes to the PETRAServer
                try
                {
                    // SubmissionResult = WEBCONNECTORMASTER.SaveAMotivationDetail(ref SubmitDS, out VerificationResult);
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
        if (e.ActionName == "actAddMotivationDetail")
        {
            btnAddMotivationDetail.Enabled = e.Enabled;
            tbbAddMotivationDetail.Enabled = e.Enabled;
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
