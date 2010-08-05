// auto generated with nant generateWinforms from CountrySetup.yaml and template windowMaintainTable
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
using Ict.Petra.Shared.MCommon.Data;

namespace Ict.Petra.Client.MCommon.Gui.Setup
{

  /// auto generated: Maintain Countries
  public partial class TFrmCountrySetup: System.Windows.Forms.Form, IFrmPetraEdit
  {
    private TFrmPetraEditUtils FPetraUtilsObject;
    private class FMainDS
    {
        public static PCountryTable PCountry;
    }

    /// constructor
    public TFrmCountrySetup(IntPtr AParentFormHandle) : base()
    {
      Control[] FoundCheckBoxes;

      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      this.btnNew.Text = Catalog.GetString("New");
      this.lblDetailCountryCode.Text = Catalog.GetString("Country Code:");
      this.lblDetailCountryName.Text = Catalog.GetString("Country Name:");
      this.lblDetailUndercover.Text = Catalog.GetString("Undercover:");
      this.lblDetailInternatAccessCode.Text = Catalog.GetString("Int'l Access Code:");
      this.lblDetailInternatTelephoneCode.Text = Catalog.GetString("Int'l Dialling Code:");
      this.lblDetailAddressOrder.Text = Catalog.GetString("Address Display Order:");
      this.lblDetailInternatPostalTypeCode.Text = Catalog.GetString("Int'l Postal Type:");
      this.lblDetailTimeZoneMinimum.Text = Catalog.GetString("Time Zone From:");
      this.lblDetailTimeZoneMaximum.Text = Catalog.GetString("To:");
      this.lblDetailDeletable.Text = Catalog.GetString("Deletable:");
      this.tbbSave.ToolTipText = Catalog.GetString("Saves changed data");
      this.tbbSave.Text = Catalog.GetString("&Save");
      this.tbbNew.Text = Catalog.GetString("New Country");
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
      this.Text = Catalog.GetString("Maintain Countries");
      #endregion

      this.txtDetailCountryCode.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailCountryName.Font = TAppSettingsManager.GetDefaultBoldFont();
      this.txtDetailInternatAccessCode.Font = TAppSettingsManager.GetDefaultBoldFont();

      FPetraUtilsObject = new TFrmPetraEditUtils(AParentFormHandle, this, stbMain);
      FPetraUtilsObject.SetStatusBarText(txtDetailCountryCode, Catalog.GetString("Enter an internationally accepted (ISO) country code"));
      FPetraUtilsObject.SetStatusBarText(txtDetailCountryName, Catalog.GetString("Enter the full name of the country"));
      FPetraUtilsObject.SetStatusBarText(chkDetailUndercover, Catalog.GetString("Select if the country is politically sensitive"));
      FPetraUtilsObject.SetStatusBarText(txtDetailInternatAccessCode, Catalog.GetString("International telephone access code needed to dial OUT"));
      FPetraUtilsObject.SetStatusBarText(txtDetailInternatTelephoneCode, Catalog.GetString("Enter the International Code needed to dial INTO the country"));
      FPetraUtilsObject.SetStatusBarText(cmbDetailAddressOrder, Catalog.GetString("0 = International; 1 = European; 2 = American"));
      cmbDetailAddressOrder.InitialiseUserControl();
      FPetraUtilsObject.SetStatusBarText(cmbDetailInternatPostalTypeCode, Catalog.GetString("Enter an international postal type code"));
      cmbDetailInternatPostalTypeCode.InitialiseUserControl();
      FPetraUtilsObject.SetStatusBarText(txtDetailTimeZoneMinimum, Catalog.GetString("Enter the minimum time zone +/- relative to GMT"));
      FPetraUtilsObject.SetStatusBarText(txtDetailTimeZoneMaximum, Catalog.GetString("Enter the maximum time zone +/- relative to GMT"));
      FPetraUtilsObject.SetStatusBarText(chkDetailDeletable, Catalog.GetString("This code is Required for System operation by other code"));

      /*
       * Automatically disable 'Deletable' CheckBox (it must not get changed by the user because records where the
       * 'Deletable' flag is true are system records that must not be deleted)
       */
      FoundCheckBoxes = this.Controls.Find("chkDetailDeletable", true);

      if (FoundCheckBoxes.Length > 0)
      {
          FoundCheckBoxes[0].Enabled = false;
      }

      LoadDataAndFinishScreenSetup();
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

    /// <summary>Loads the data for the screen and finishes the setting up of the screen.</summary>
    /// <returns>void</returns>
    private void LoadDataAndFinishScreenSetup()
    {
      Type DataTableType;

      // Load Data
      FMainDS.PCountry = new PCountryTable();
      DataTable CacheDT = TDataCache.GetCacheableDataTableFromCache("CountryList", String.Empty, null, out DataTableType);
      FMainDS.PCountry.Merge(CacheDT);

      grdDetails.Columns.Clear();
      grdDetails.AddTextColumn("Country Code", FMainDS.PCountry.ColumnCountryCode);
      grdDetails.AddTextColumn("Country Name", FMainDS.PCountry.ColumnCountryName);
      grdDetails.AddTextColumn("Time Zone Min.", FMainDS.PCountry.ColumnTimeZoneMinimum);
      grdDetails.AddTextColumn("Time Zone Max.", FMainDS.PCountry.ColumnTimeZoneMaximum);
      grdDetails.AddCheckBoxColumn("Undercover", FMainDS.PCountry.ColumnUndercover);
      grdDetails.AddCheckBoxColumn("Deletable", FMainDS.PCountry.ColumnDeletable);

      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      DataView myDataView = FMainDS.PCountry.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
      grdDetails.AutoSizeCells();

      // Ensure that the Details Panel is disabled if there are no records
      if (FMainDS.PCountry.Rows.Count == 0)
      {
        ShowDetails(null);
      }

      FPetraUtilsObject.InitActionState();
    }

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position

    }

    /// automatically generated, create a new record of PCountry and display on the edit screen
    /// we create the table locally, no dataset
    public bool CreateNewPCountry()
    {
        PCountryRow NewRow = FMainDS.PCountry.NewRowTyped();
        NewRowManual(ref NewRow);
        FMainDS.PCountry.Rows.Add(NewRow);

        FPetraUtilsObject.SetChangedFlag();

        grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.PCountry.DefaultView);
        grdDetails.Refresh();
        SelectDetailRowByDataTableIndex(FMainDS.PCountry.Rows.Count - 1);

        return true;
    }

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.PCountry.PrimaryKey)
            {
                string value1 = FMainDS.PCountry.Rows[ARowNumberInTable][myColumn].ToString();
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
    private PCountryRow GetSelectedDetailRow()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            return (PCountryRow)SelectedGridRow[0].Row;
        }

        return null;
    }

    private void ShowDetails(PCountryRow ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        if (ARow == null)
        {
            pnlDetails.Enabled = false;
        }
        else
        {
            FPreviouslySelectedDetailRow = ARow;
            txtDetailCountryCode.Text = ARow.CountryCode;
            txtDetailCountryCode.ReadOnly = (ARow.RowState != DataRowState.Added);
            txtDetailCountryName.Text = ARow.CountryName;
            if (ARow.IsUndercoverNull())
            {
                chkDetailUndercover.Checked = false;
            }
            else
            {
                chkDetailUndercover.Checked = ARow.Undercover;
            }
            if (ARow.IsInternatAccessCodeNull())
            {
                txtDetailInternatAccessCode.Text = String.Empty;
            }
            else
            {
                txtDetailInternatAccessCode.Text = ARow.InternatAccessCode;
            }
            if (ARow.IsInternatTelephoneCodeNull())
            {
                txtDetailInternatTelephoneCode.NumberValueInt = null;
            }
            else
            {
                txtDetailInternatTelephoneCode.NumberValueInt = ARow.InternatTelephoneCode;
            }
            if (ARow.IsAddressOrderNull())
            {
                cmbDetailAddressOrder.SelectedIndex = -1;
            }
            else
            {
                cmbDetailAddressOrder.SetSelectedInt32(ARow.AddressOrder);
            }
            if (ARow.IsInternatPostalTypeCodeNull())
            {
                cmbDetailInternatPostalTypeCode.SelectedIndex = -1;
            }
            else
            {
                cmbDetailInternatPostalTypeCode.SetSelectedString(ARow.InternatPostalTypeCode);
            }
            if (ARow.IsTimeZoneMinimumNull())
            {
                txtDetailTimeZoneMinimum.NumberValueDouble = null;
            }
            else
            {
                txtDetailTimeZoneMinimum.NumberValueDecimal = Convert.ToDecimal(ARow.TimeZoneMinimum);
            }
            if (ARow.IsTimeZoneMaximumNull())
            {
                txtDetailTimeZoneMaximum.NumberValueDouble = null;
            }
            else
            {
                txtDetailTimeZoneMaximum.NumberValueDecimal = Convert.ToDecimal(ARow.TimeZoneMaximum);
            }
            chkDetailDeletable.Checked = ARow.Deletable;
            pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
        }
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private PCountryRow FPreviouslySelectedDetailRow = null;
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

    private void GetDetailsFromControls(PCountryRow ARow)
    {
        if (ARow != null)
        {
            ARow.CountryCode = txtDetailCountryCode.Text;
            ARow.CountryName = txtDetailCountryName.Text;
            ARow.Undercover = chkDetailUndercover.Checked;
            if (txtDetailInternatAccessCode.Text.Length == 0)
            {
                ARow.SetInternatAccessCodeNull();
            }
            else
            {
                ARow.InternatAccessCode = txtDetailInternatAccessCode.Text;
            }
            if (txtDetailInternatTelephoneCode.NumberValueInt == null)
            {
                ARow.SetInternatTelephoneCodeNull();
            }
            else
            {
                ARow.InternatTelephoneCode = Convert.ToInt32(txtDetailInternatTelephoneCode.NumberValueInt);
            }
            if (cmbDetailAddressOrder.SelectedIndex == -1)
            {
                ARow.SetAddressOrderNull();
            }
            else
            {
                ARow.AddressOrder = cmbDetailAddressOrder.GetSelectedInt32();
            }
            if (cmbDetailInternatPostalTypeCode.SelectedIndex == -1)
            {
                ARow.SetInternatPostalTypeCodeNull();
            }
            else
            {
                ARow.InternatPostalTypeCode = cmbDetailInternatPostalTypeCode.GetSelectedString();
            }
            if (txtDetailTimeZoneMinimum.NumberValueDouble == null)
            {
                ARow.SetTimeZoneMinimumNull();
            }
            else
            {
                ARow.TimeZoneMinimum = Convert.ToDecimal(txtDetailTimeZoneMinimum.NumberValueDecimal);
            }
            if (txtDetailTimeZoneMaximum.NumberValueDouble == null)
            {
                ARow.SetTimeZoneMaximumNull();
            }
            else
            {
                ARow.TimeZoneMaximum = Convert.ToDecimal(txtDetailTimeZoneMaximum.NumberValueDecimal);
            }
            ARow.Deletable = chkDetailDeletable.Checked;
            GetDetailDataFromControlsManual(ARow);
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
            foreach (DataRow InspectDR in FMainDS.PCountry.Rows)
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

                Ict.Common.Data.TTypedDataTable SubmitDT = FMainDS.PCountry.GetChangesTyped();

                if (SubmitDT == null)
                {
                    // nothing to be saved, so it is ok to close the screen etc
                    return true;
                }

                // Submit changes to the PETRAServer
                try
                {
                    SubmissionResult = TDataCache.SaveChangedCacheableDataTableToPetraServer("CountryList", ref SubmitDT, out VerificationResult);
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
                        FMainDS.PCountry.AcceptChanges();

                        // Merge back with data from the Server (eg. for getting Sequence values)
                        FMainDS.PCountry.Merge(SubmitDT, false);

                        // need to accept the new modification ID
                        FMainDS.PCountry.AcceptChanges();

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
