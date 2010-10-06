// auto generated with nant generateWinforms from UC_SetupAnalysisValues.yaml and template controlMaintainCachableTable
//
// DO NOT edit manually, DO NOT edit with the designer
//
//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       auto generated
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
using Mono.Unix;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared.MFinance.Account.Data;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{

  /// auto generated:
  public partial class TUC_SetupAnalysisValues: System.Windows.Forms.UserControl, Ict.Petra.Client.CommonForms.IFrmPetra
  {
    private TFrmPetraEditUtils FPetraUtilsObject;

    private Ict.Petra.Shared.MFinance.GL.Data.GLSetupTDS FMainDS;
    /// constructor
    public TUC_SetupAnalysisValues() : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.lblDetailLedgerNumber.Text = Catalog.GetString("Values for this Type and Ledger Number:");
      this.btnNew.Text = Catalog.GetString("&New");
      this.btnDelete.Text = Catalog.GetString("&Delete");
      this.lblDetailAnalysisValue.Text = Catalog.GetString("Description:");
      this.lblDetailActive.Text = Catalog.GetString("Active:");
      #endregion

      this.txtDetailLedgerNumber.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailAnalysisValue.Font = TAppSettingsManager.GetDefaultBoldFont();

    }

    /// helper object for the whole screen
    public TFrmPetraEditUtils PetraUtilsObject
    {
        set
        {
            FPetraUtilsObject = value;
        }
    }
        /// dataset for the whole screen
    public Ict.Petra.Shared.MFinance.GL.Data.GLSetupTDS MainDS
    {
        set
        {
            FMainDS = value;
        }
    }
    /// <summary>Loads the data for the screen and finishes the setting up of the screen.</summary>
    /// <returns>void</returns>    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
      FPetraUtilsObject.SetStatusBarText(txtDetailLedgerNumber, Catalog.GetString("Ledger Number "));
      FPetraUtilsObject.SetStatusBarText(txtDetailAnalysisValue, Catalog.GetString("Value of analysis code"));
      FPetraUtilsObject.SetStatusBarText(chkDetailActive, Catalog.GetString("Select if analysis attribute value can be used"));
      Type DataTableType;

      // Load Data
      DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("FreeformAnalysisList", String.Empty, null, out DataTableType);
      FMainDS.AFreeformAnalysis.Merge(CacheDT);

      grdDetails.Columns.Clear();
      grdDetails.AddTextColumn("Value", FMainDS.AFreeformAnalysis.ColumnAnalysisValue);
      grdDetails.AddCheckBoxColumn("Active", FMainDS.AFreeformAnalysis.ColumnActive);
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      DataView myDataView = FMainDS.AFreeformAnalysis.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

      ShowData();
    }

    /// automatically generated, create a new record of AFreeformAnalysis and display on the edit screen
    public bool CreateNewAFreeformAnalysis()
    {
        // we create the table locally, no dataset
        AFreeformAnalysisRow NewRow = FMainDS.AFreeformAnalysis.NewRowTyped(true);
        NewRowManual(ref NewRow);
        FMainDS.AFreeformAnalysis.Rows.Add(NewRow);

        FPetraUtilsObject.SetChangedFlag();

        grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.AFreeformAnalysis.DefaultView);
        grdDetails.Refresh();
        SelectDetailRowByDataTableIndex(FMainDS.AFreeformAnalysis.Rows.Count - 1);

        return true;
    }

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.AFreeformAnalysis.PrimaryKey)
            {
                string value1 = FMainDS.AFreeformAnalysis.Rows[ARowNumberInTable][myColumn].ToString();
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
    public AFreeformAnalysisRow GetSelectedDetailRow()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            return (AFreeformAnalysisRow)SelectedGridRow[0].Row;
        }

        return null;
    }

    private void ShowData()
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        pnlDetails.Enabled = false;
        if (FMainDS.AFreeformAnalysis != null)
        {
            DataView myDataView = FMainDS.AFreeformAnalysis.DefaultView;
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            if (myDataView.Count > 0)
            {
                grdDetails.Selection.ResetSelection(false);
                grdDetails.Selection.SelectRow(1, true);
                FocusedRowChanged(this, new SourceGrid.RowEventArgs(1));
                pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
            }
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private void ShowDetails(AFreeformAnalysisRow ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        if (ARow == null)
        {
            pnlDetails.Enabled = false;
        }
        else
        {
            FPreviouslySelectedDetailRow = ARow;
            txtDetailLedgerNumber.Text = ARow.LedgerNumber.ToString();
            txtDetailLedgerNumber.ReadOnly = (ARow.RowState != DataRowState.Added);
            txtDetailAnalysisValue.Text = ARow.AnalysisValue;
            txtDetailAnalysisValue.ReadOnly = (ARow.RowState != DataRowState.Added);
            if (ARow.IsActiveNull())
            {
                chkDetailActive.Checked = false;
            }
            else
            {
                chkDetailActive.Checked = ARow.Active;
            }
            pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private AFreeformAnalysisRow FPreviouslySelectedDetailRow = null;
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
    }

    /// get the data from the controls and store in the currently selected detail row
    public void GetDataFromControls()
    {
        GetDetailsFromControls(FPreviouslySelectedDetailRow);
    }

    private void GetDetailsFromControls(AFreeformAnalysisRow ARow)
    {
        if (ARow != null)
        {
            ARow.BeginEdit();
            ARow.AnalysisValue = txtDetailAnalysisValue.Text;
            ARow.Active = chkDetailActive.Checked;
            GetDetailDataFromControlsManual(ARow);
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
            foreach (DataRow InspectDR in FMainDS.AFreeformAnalysis.Rows)
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

                Ict.Common.Data.TTypedDataTable SubmitDT = FMainDS.AFreeformAnalysis.GetChangesTyped();

                if (SubmitDT == null)
                {
                    // nothing to be saved, so it is ok to close the screen etc
                    return true;
                }

                // Submit changes to the PETRAServer
                try
                {
                    SubmissionResult = TDataCache.SaveChangedCacheableDataTableToPetraServer("FreeformAnalysisList", ref SubmitDT, out VerificationResult);
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
                        FMainDS.AFreeformAnalysis.AcceptChanges();

                        // Merge back with data from the Server (eg. for getting Sequence values)
                        FMainDS.AFreeformAnalysis.Merge(SubmitDT, false);

                        // need to accept the new modification ID
                        FMainDS.AFreeformAnalysis.AcceptChanges();

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
        if (e.ActionName == "actNew")
        {
            btnNew.Enabled = e.Enabled;
        }
        if (e.ActionName == "actDelete")
        {
            btnDelete.Enabled = e.Enabled;
        }
    }

#endregion
  }
}
