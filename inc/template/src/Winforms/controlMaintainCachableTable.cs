// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template controlMaintainCachableTable
//
// DO NOT edit manually, DO NOT edit with the designer
//
{#GPLFILEHEADER}
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Common.Remoting.Shared;
{#IFDEF SHAREDVALIDATIONNAMESPACEMODULE}
using {#SHAREDVALIDATIONNAMESPACEMODULE};
{#ENDIF SHAREDVALIDATIONNAMESPACEMODULE}
{#USINGNAMESPACES}

namespace {#NAMESPACE}
{

  /// auto generated: {#FORMTITLE}
  public partial class {#CLASSNAME}: System.Windows.Forms.UserControl, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;
{#FILTERVAR}
{#IFDEF DATASETTYPE}
    private {#DATASETTYPE} FMainDS;
{#ENDIF DATASETTYPE}
{#IFNDEF DATASETTYPE}
    private class FMainDS 
    {
        public static {#DETAILTABLE}Table {#DETAILTABLE};
    }
{#ENDIFN DATASETTYPE} 
{#IFDEF SHOWDETAILS}

    private int FCurrentRow;
{#ENDIF SHOWDETAILS}

    /// constructor
    public {#CLASSNAME}() : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      {#CATALOGI18N}
      #endregion

      {#ASSIGNFONTATTRIBUTES}

    }

    /// helper object for the whole screen
    public {#UTILOBJECTCLASS} PetraUtilsObject
    {
        set
        {
            FPetraUtilsObject = value;
        }
    }
    {#IFDEF DATASETTYPE}
        /// dataset for the whole screen
    public {#DATASETTYPE} MainDS
    {
        set
        {
            FMainDS = value;
        }
    }
    {#ENDIF DATASETTYPE}

    /// <summary>Loads the data for the screen and finishes the setting up of the screen.</summary>
    /// <returns>void</returns>    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
      {#INITUSERCONTROLS}
      Type DataTableType;
      
      // Load Data
{#IFNDEF DATASETTYPE}
      FMainDS.{#DETAILTABLE} = new {#DETAILTABLE}Table();
{#ENDIFN DATASETTYPE}      
      DataTable CacheDT = TDataCache.{#CACHEABLETABLERETRIEVEMETHOD}({#CACHEABLETABLE}, {#CACHEABLETABLESPECIFICFILTERLOAD}, out DataTableType);
      FMainDS.{#DETAILTABLE}.Merge(CacheDT);    
      
      {#INITMANUALCODE}
{#IFDEF ACTIONENABLING}
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
{#ENDIF ACTIONENABLING}
      
      DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
      myDataView.AllowNew = false;
      grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

      ShowData();
    }
    
    {#EVENTHANDLERSIMPLEMENTATION}

    /// automatically generated, create a new record of {#DETAILTABLE} and display on the edit screen
    public bool CreateNew{#DETAILTABLE}()
    {
        if(ValidateAllData(true, true))
        {
{#IFNDEF CANFINDWEBCONNECTOR_CREATEDETAIL}
            // we create the table locally, no dataset
            {#DETAILTABLETYPE}Row NewRow = FMainDS.{#DETAILTABLE}.NewRowTyped(true);
            {#INITNEWROWMANUAL}
            FMainDS.{#DETAILTABLE}.Rows.Add(NewRow);
{#ENDIFN CANFINDWEBCONNECTOR_CREATEDETAIL}
{#IFDEF CANFINDWEBCONNECTOR_CREATEDETAIL}
            FMainDS.Merge({#WEBCONNECTORDETAIL}.Create{#DETAILTABLE}({#CREATEDETAIL_ACTUALPARAMETERS_LOCAL}));
{#ENDIF CANFINDWEBCONNECTOR_CREATEDETAIL}

            FPetraUtilsObject.SetChangedFlag();

            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(FMainDS.{#DETAILTABLE}.DefaultView);
            grdDetails.Refresh();
            SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1);

            return true;
        }
        else
        {
            return false;
        }
    }

    private void SelectDetailRowByDataTableIndex(Int32 ARowNumberInTable)
    {
        Int32 RowNumberGrid = -1;
        for (int Counter = 0; Counter < grdDetails.DataSource.Count; Counter++)
        {
            bool found = true;
            foreach (DataColumn myColumn in FMainDS.{#DETAILTABLE}.PrimaryKey)
            {
                string value1 = FMainDS.{#DETAILTABLE}.Rows[ARowNumberInTable][myColumn].ToString();
                string value2 = (grdDetails.DataSource as DevAge.ComponentModel.BoundDataView).DataView[Counter][myColumn.Ordinal].ToString();
                if (value1 != value2)
                {
                    found = false;
                }
            }
            if (found)
            {
                RowNumberGrid = Counter + 1;
                break;
            }
        }

        grdDetails.SelectRowInGrid(RowNumberGrid);
    }

{#IFDEF SHOWDETAILS OR GENERATEGETSELECTEDDETAILROW}

    /// return the selected row
    public {#DETAILTABLETYPE}Row GetSelectedDetailRow()
    {
        {#GETSELECTEDDETAILROW}
    }
{#ENDIF SHOWDETAILS OR GENERATEGETSELECTEDDETAILROW}


    private void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
        {#PRIMARYKEYCONTROLSREADONLY}
    }

    private void ShowData()
    {
        FPetraUtilsObject.DisableDataChangedEvent();
{#IFDEF SHOWDATA}        
        {#SHOWDATA}
{#ENDIF SHOWDATA}        
        pnlDetails.Enabled = false;
        if (FMainDS.{#DETAILTABLE} != null)
        {
            DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
{#IFDEF DETAILTABLESORT}
            myDataView.Sort = "{#DETAILTABLESORT}";
{#ENDIF DETAILTABLESORT}
{#IFDEF DETAILTABLEFILTER}
            myDataView.RowFilter = {#DETAILTABLEFILTER};
{#ENDIF DETAILTABLEFILTER}
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

{#IFDEF UNDODATA}

    private void UndoData(DataRow ARow, Control AControl)
    {
        {#UNDODATA}
    }
{#ENDIF UNDODATA}

    /// <summary>
    /// Performs data validation.
    /// </summary>
    /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing 
    /// to another record, otherwise set it to false.</param>
    /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
    /// user, otherwise set it to false.</param>
    /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>    
    private bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors)
    {
        bool ReturnValue = false;

        // Records a new Data Validation Run. (All TVerificationResults/TScreenVerificationResults that are created during this 'run' are associated with this 'run' through that.)
        FPetraUtilsObject.VerificationResultCollection.RecordNewDataValidationRun();

{#IFDEF SHOWDETAILS}
        {#DETAILTABLETYPE}Row CurrentRow;
        // AlanP chnaged this from = GetSelectedDetailRow() because that caused errors when the sort headers of the grid were clicked       
        CurrentRow = FPreviouslySelectedDetailRow;

        if (CurrentRow != null)
        {
{#ENDIF SHOWDETAILS}        
{#IFNDEF SHOWDETAILS}
{#IFDEF MASTERTABLE}
        GetDataFromControls(FMainDS.{#MASTERTABLE}[0]);
        ValidateData(FMainDS.{#MASTERTABLE}[0]);
{#ENDIF MASTERTABLE}        
{#ENDIFN SHOWDETAILS}
{#IFDEF SHOWDETAILS}
        GetDetailsFromControls(CurrentRow);
        ValidateDataDetails(CurrentRow);
{#ENDIF SHOWDETAILS}

{#IFDEF VALIDATEDATADETAILSMANUAL}
{#IFDEF SHOWDETAILS}
            ValidateDataDetailsManual(CurrentRow);
{#ENDIF SHOWDETAILS}
{#ENDIF VALIDATEDATADETAILSMANUAL}
{#IFDEF VALIDATEDATAMANUAL}
{#IFDEF MASTERTABLE}
            ValidateDataManual(FMainDS.{#MASTERTABLE}[0]);
{#ENDIF MASTERTABLE}
{#ENDIF VALIDATEDATAMANUAL}
{#IFDEF PERFORMUSERCONTROLVALIDATION}

            // Perform validation in UserControls, too
            {#USERCONTROLVALIDATION}
{#ENDIF PERFORMUSERCONTROLVALIDATION}

            if (AProcessAnyDataValidationErrors)
            {
                ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
                    this.GetType());    
            }
{#IFDEF SHOWDETAILS}            
        }
        else
        {
            ReturnValue = true;
        }
{#ENDIF SHOWDETAILS}

        if(ReturnValue)
        {
            // Remove a possibly shown Validation ToolTip as the data validation succeeded
            FPetraUtilsObject.ValidationToolTip.RemoveAll();
        }

        return ReturnValue;
    }

{#IFDEF SHOWDATA}
    private void ShowData({#MASTERTABLETYPE}Row ARow)
    {
        {#SHOWDATA}
    }
{#ENDIF SHOWDATA}

{#IFDEF SHOWDETAILS}
    private void ShowDetails({#DETAILTABLE}Row ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
{#IFDEF SAVEDETAILS}
        grdDetails.Selection.FocusRowLeaving -= new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
{#ENDIF SAVEDETAILS}

        if (ARow == null)
        {
            pnlDetails.Enabled = false;
            {#CLEARDETAILS}
        }
        else
        {
            FPreviouslySelectedDetailRow = ARow;
            {#SHOWDETAILS}
            pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
        }

        FPetraUtilsObject.EnableDataChangedEvent();
{#IFDEF SAVEDETAILS}
        grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
{#ENDIF SAVEDETAILS}
    }

    private {#DETAILTABLE}Row FPreviouslySelectedDetailRow = null;
{#IFDEF SAVEDETAILS}

    private void FocusRowLeaving(object sender, SourceGrid.RowCancelEventArgs e)
    {        
        if (grdDetails.Focused)
        {
            if (!ValidateAllData(true, true))
            {
                e.Cancel = true;                
            }
        }
        else
        {
            // This is needed because of a strange quirk in the Grid: if the user clicks with the Mouse to a different Row
            // (not when using the keyboard!), then the Method 'FocusRowLeaving' gets called twice, the second time 
            // grdDetails.Focused is false. We need to Cancel in this case, otherwise the user can leave the Row with a 
            // mouse click on another Row although it contains invalid data!!!
            e.Cancel = true;
        }        
    }
{#ENDIF SAVEDETAILS}

    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
        if(e.Row != FCurrentRow)
        {
            // Transfer data from Controls into the DataTable
            GetDetailsFromControls(FPreviouslySelectedDetailRow);
            
            // Display the details of the currently selected Row
            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
            ShowDetails(FPreviouslySelectedDetailRow);
            pnlDetails.Enabled = true;
            
            FCurrentRow = e.Row;
        }
    }
{#ENDIF SHOWDETAILS}
    
{#IFDEF MASTERTABLE}

    private void GetDataFromControls({#MASTERTABLETYPE}Row ARow)
    {
{#IFDEF SAVEDATA}
        {#SAVEDATA}
{#ENDIF SAVEDATA}
    }
{#ENDIF MASTERTABLE}
{#IFNDEF MASTERTABLE}

    private void GetDataFromControls()
    {
{#IFDEF SAVEDATA}
        {#SAVEDATA}
{#ENDIF SAVEDATA}
    }
{#ENDIFN MASTERTABLE}
{#IFDEF SAVEDETAILS}
    private void GetDetailsFromControls({#DETAILTABLE}Row ARow)
    {
        if (ARow != null)
        {
            ARow.BeginEdit();
            {#SAVEDETAILS}
            ARow.EndEdit();
        }
    }
{#ENDIF SAVEDETAILS}

#region Implement interface functions
    
    /// auto generated
    public void RunOnceOnActivation()
    {
        {#RUNONCEINTERFACEIMPLEMENTATION}
    }

    /// <summary>
    /// Adds event handlers for the appropiate onChange event to call a central procedure
    /// </summary>
    public void HookupAllControls()
    {
        {#HOOKUPINTERFACEIMPLEMENTATION}
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

    /// <summary>
    /// Get the latest data from the controls and validate it
    /// </summary>
    /// <returns>True if data was validated successfully, otherwise false.  If successful, it is safe to call SaveChanges()</returns>    
    public bool ValidateBeforeSave()
    {
        // Call this before any call to SaveChanges().  It will automatically get the data from the current controls first.
        return ValidateAllData(false, true);
    }

    /// <summary>
    /// save the changes on the screen
    /// </summary>
    /// <returns>True if data was saved successfully, otherwise false.</returns>    
    public bool SaveChanges()
    {
        // Be sure to have called ValidateBeforeSave() before calling this method

        // Clear any validation errors so that the following call to ValidateAllData starts with a 'clean slate'.
        FPetraUtilsObject.VerificationResultCollection.Clear();

        foreach (DataRow InspectDR in FMainDS.{#DETAILTABLE}.Rows)
        {
            InspectDR.EndEdit();
        }

        if (!FPetraUtilsObject.HasChanges)
        {
            return true;
        }
        else
        {
            FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataInProgress);
            this.Cursor = Cursors.WaitCursor;

            TSubmitChangesResult SubmissionResult;
            TVerificationResultCollection VerificationResult;

            Ict.Common.Data.TTypedDataTable SubmitDT = FMainDS.{#DETAILTABLE}.GetChangesTyped();

            if (SubmitDT == null)
            {
                // nothing to be saved, so it is ok to close the screen etc
                return true;
            }
                
            // Submit changes to the PETRAServer
            try
            {
                SubmissionResult = TDataCache.{#CACHEABLETABLESAVEMETHOD}({#CACHEABLETABLE}, ref SubmitDT{#CACHEABLETABLESPECIFICFILTERSAVE}, out VerificationResult);
            }
            catch (ESecurityDBTableAccessDeniedException Exp)
            {
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                this.Cursor = Cursors.Default;

                TMessages.MsgSecurityException(Exp, this.GetType());
                    
                return false;
            }
            catch (EDBConcurrencyException Exp)
            {
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                this.Cursor = Cursors.Default;

                TMessages.MsgDBConcurrencyException(Exp, this.GetType());
                    
                return false;
            }
            catch (Exception)
            {
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                this.Cursor = Cursors.Default;
                    
                throw;
            }

            switch (SubmissionResult)
            {
                case TSubmitChangesResult.scrOK:

                    // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                    FMainDS.{#DETAILTABLE}.AcceptChanges();

                    // Merge back with data from the Server (eg. for getting Sequence values)
                    FMainDS.{#DETAILTABLE}.Merge(SubmitDT, false);

                    // need to accept the new modification ID
                    FMainDS.{#DETAILTABLE}.AcceptChanges();

                    // Update UI
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataSuccessful);
                    this.Cursor = Cursors.Default;

                    SetPrimaryKeyReadOnly(true);

                    return true;

                case TSubmitChangesResult.scrError:
                    this.Cursor = Cursors.Default;
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataErrorOccured);

                    MessageBox.Show(Messages.BuildMessageFromVerificationResult(null, VerificationResult), 
                        Catalog.GetString("Data Cannot Be Saved"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                    FPetraUtilsObject.SubmitChangesContinue = false;
                        
                    return false;

                case TSubmitChangesResult.scrNothingToBeSaved:
                    this.Cursor = Cursors.Default;
                    return true;

                case TSubmitChangesResult.scrInfoNeeded:

                    // TODO scrInfoNeeded
                    this.Cursor = Cursors.Default;
                    break;
            }
        }

        return false;
    }

#endregion
{#IFDEF ACTIONENABLING}

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        {#ACTIONENABLING}
        {#ACTIONENABLINGDISABLEMISSINGFUNCS}
    }

    {#ACTIONHANDLERS}

#endregion
{#ENDIF ACTIONENABLING}

#region Data Validation
    
    private void ControlValidatedHandler(object sender, EventArgs e)
    {
        TScreenVerificationResult SingleVerificationResult;
        
        ValidateAllData(true, false);
        
        FPetraUtilsObject.ValidationToolTip.RemoveAll();
        
        if (FPetraUtilsObject.VerificationResultCollection.Count > 0) 
        {
            for (int Counter = 0; Counter < FPetraUtilsObject.VerificationResultCollection.Count; Counter++) 
            {
                SingleVerificationResult = (TScreenVerificationResult)FPetraUtilsObject.VerificationResultCollection[Counter];
                
                if (SingleVerificationResult.ResultControl == sender) 
                {
                    if (FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested)
                    {
                        SingleVerificationResult.ResultControl.Focus();
                        FPetraUtilsObject.VerificationResultCollection.FocusOnFirstErrorControlRequested = false;
                    }

                    FPetraUtilsObject.ValidationToolTipSeverity = SingleVerificationResult.ResultSeverity;

                    if (SingleVerificationResult.ResultTextCaption != String.Empty) 
                    {
                        FPetraUtilsObject.ValidationToolTip.ToolTipTitle += ":  " + SingleVerificationResult.ResultTextCaption;    
                    }
{#IFDEF UNDODATA}

                    if(SingleVerificationResult.ControlValueUndoRequested)
                    {
                        UndoData(SingleVerificationResult.ResultColumn.Table.Rows[0], SingleVerificationResult.ResultControl);
                        SingleVerificationResult.OverrideResultText(SingleVerificationResult.ResultText + Environment.NewLine + Environment.NewLine + 
                            Catalog.GetString("--> The value you entered has been changed back to what it was before! <--"));
                    }
{#ENDIF UNDODATA}

                    FPetraUtilsObject.ValidationToolTip.Show(SingleVerificationResult.ResultText, (Control)sender, 
                        ((Control)sender).Width / 2, ((Control)sender).Height);
                }
            }
        }
    }
{#IFDEF MASTERTABLE}
    private void ValidateData({#MASTERTABLE}Row ARow)
    {
        TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

        {#MASTERTABLE}Validation.Validate(this, ARow, ref VerificationResultCollection,
            FPetraUtilsObject.ValidationControlsDict);
    }
{#ENDIF MASTERTABLE}
{#IFDEF DETAILTABLE}
    private void ValidateDataDetails({#DETAILTABLE}Row ARow)
    {
        TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

        {#DETAILTABLE}Validation.Validate(this, ARow, ref VerificationResultCollection,
            FPetraUtilsObject.ValidationControlsDict);
    }
{#ENDIF DETAILTABLE}
{#IFDEF MASTERTABLE OR DETAILTABLE}

    private void BuildValidationControlsDict()
    {
{#IFDEF ADDCONTROLTOVALIDATIONCONTROLSDICT}
        {#ADDCONTROLTOVALIDATIONCONTROLSDICT}
{#ENDIF ADDCONTROLTOVALIDATIONCONTROLSDICT}
    }
{#ENDIF MASTERTABLE OR DETAILTABLE}    

#endregion
  }
}

{#INCLUDE copyvalues.cs}
{#INCLUDE validationcontrolsdict.cs}