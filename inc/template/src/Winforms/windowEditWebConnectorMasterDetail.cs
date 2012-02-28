// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template windowEditWebConnectorMasterDetail
//
// DO NOT edit manually, DO NOT edit with the designer
//
{#GPLFILEHEADER}
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
using Ict.Common.Remoting.Shared;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.MCommon;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
{#USINGNAMESPACES}

namespace {#NAMESPACE}
{

  /// auto generated: {#FORMTITLE}
  public partial class {#CLASSNAME}: System.Windows.Forms.Form, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;
    private {#DATASETTYPE} FMainDS;

    /// constructor
    public {#CLASSNAME}(Form AParentForm) : base()
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
      
      FPetraUtilsObject = new {#UTILOBJECTCLASS}(AParentForm, this, stbMain);
      {#INITUSERCONTROLS}
      FMainDS = new {#DATASETTYPE}();
      {#INITMANUALCODE}
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;

      {#INITACTIONSTATE}
{#IFDEF DATAVALIDATION}

      BuildValidationControlsDict();
{#ENDIF DATAVALIDATION}
    }

    {#EVENTHANDLERSIMPLEMENTATION}

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position

    }

{#IFDEF CANFINDWEBCONNECTOR_CREATEMASTER}
    /// automatically generated function from webconnector
    public bool Create{#MASTERTABLE}({#CREATEMASTER_FORMALPARAMETERS})
    {
{#IFDEF CREATEMASTER_WITHVERIFICATION}
        TVerificationResultCollection VerificationResult;

        FMainDS = {#WEBCONNECTORMASTER}.Create{#MASTERTABLE}({#CREATEMASTER_ACTUALPARAMETERS}, out VerificationResult);

        if (VerificationResult != null && VerificationResult.Count > 0)
        {
            return CreateMasterManual({#CREATEMASTER_ACTUALPARAMETERS}, VerificationResult);
        }
        else
        {
            FPetraUtilsObject.SetChangedFlag();

            ShowData(FMainDS.{#MASTERTABLE}[0]);
            
            return true;
        }
{#ENDIF CREATEMASTER_WITHVERIFICATION}
{#IFDEF CREATEMASTER_WITHOUTVERIFICATION}
        FMainDS = {#WEBCONNECTORMASTER}.Create{#MASTERTABLE}({#CREATEMASTER_ACTUALPARAMETERS});

        FPetraUtilsObject.SetChangedFlag();

        ShowData(FMainDS.{#MASTERTABLE}[0]);
        
        return true;
{#ENDIF CREATEMASTER_WITHOUTVERIFICATION}
    }
{#ENDIF CANFINDWEBCONNECTOR_CREATEMASTER}

{#IFDEF CANFINDWEBCONNECTOR_CREATEDETAIL}
    /// automatically generated, create a new record of {#DETAILTABLE} and display on the edit screen
    public bool Create{#DETAILTABLE}({#CREATEDETAIL_FORMALPARAMETERS})
    {
        if(ValidateAllData(true, true))
        {    
            FMainDS.Merge({#WEBCONNECTORDETAIL}.Create{#DETAILTABLE}({#CREATEDETAIL_ACTUALPARAMETERS}));
            FMainDS.InitVars();
            FMainDS.{#DETAILTABLE}.InitVars();

            FPetraUtilsObject.SetChangedFlag();

            DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            grdDetails.Refresh();
            SelectDetailRowByDataTableIndex(FMainDS.{#DETAILTABLE}.Rows.Count - 1);
        
            return true;
        }
        else
        {
            return false;
        }
    }
{#ENDIF CANFINDWEBCONNECTOR_CREATEDETAIL}
{#IFDEF DETAILTABLE}

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
            }
        }

        grdDetails.SelectRowInGrid(RowNumberGrid);
    }

    /// return the selected row
    private {#DETAILTABLETYPE}Row GetSelectedDetailRow()
    {
        DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

        if (SelectedGridRow.Length >= 1)
        {
            return ({#DETAILTABLETYPE}Row)SelectedGridRow[0].Row;
        }

        return null;
    }
{#ENDIF DETAILTABLE}

{#IFDEF CANFINDWEBCONNECTOR_LOADMASTER}

    /// automatically generated function from webconnector
    public bool Load{#MASTERTABLE}({#LOADMASTER_FORMALPARAMETERS})
    {
        FMainDS.Merge({#WEBCONNECTORMASTER}.Load{#MASTERTABLE}({#LOADMASTER_ACTUALPARAMETERS}));

        ShowData(FMainDS.{#MASTERTABLE}[0]);
        
        return true;
    }
{#ENDIF CANFINDWEBCONNECTOR_LOADMASTER}

    private void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
        {#PRIMARYKEYCONTROLSREADONLY}
    }

{#IFDEF SHOWDATA}
    private void ShowData({#MASTERTABLETYPE}Row ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
{#IFDEF SAVEDETAILS}
        grdDetails.Selection.FocusRowLeaving -= new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
{#ENDIF SAVEDETAILS}
        
        {#SHOWDATA}
{#IFDEF DETAILTABLE}
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
            if (FMainDS.{#DETAILTABLE}.Rows.Count > 0)
            {
                grdDetails.SelectRowInGrid(1);
                pnlDetails.Enabled = true;
            }
        }
{#ENDIF DETAILTABLE}
        FPetraUtilsObject.EnableDataChangedEvent();
{#IFDEF SAVEDETAILS}
        grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
{#ENDIF SAVEDETAILS}
    }
{#ENDIF SHOWDATA}

{#IFDEF SHOWDETAILS}
    private void ShowDetails({#DETAILTABLETYPE}Row ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        {#SHOWDETAILS}
        FPetraUtilsObject.EnableDataChangedEvent();
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
        // display the details of the currently selected row
        FPreviouslySelectedDetailRow = GetSelectedDetailRow();
        ShowDetails(FPreviouslySelectedDetailRow);
        pnlDetails.Enabled = true;
    }
{#ENDIF SHOWDETAILS}
    
    
{#IFDEF SAVEDATA}
    private void GetDataFromControls({#MASTERTABLETYPE}Row ARow)
    {
        {#SAVEDATA}
{#IFDEF SAVEDETAILS}
        GetDetailsFromControls(FPreviouslySelectedDetailRow);
{#ENDIF SAVEDETAILS}
    }
{#ENDIF SAVEDATA}

{#IFDEF SAVEDETAILS}
    private void GetDetailsFromControls({#DETAILTABLETYPE}Row ARow)
    {
        if (ARow != null)
        {
            ARow.BeginEdit();
            {#SAVEDETAILS}
            ARow.BeginEdit();
        }
    }
{#ENDIF SAVEDETAILS}

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
        {#DETAILTABLE}Row CurrentRow;

        GetDataFromControls(FMainDS.{#MASTERTABLE}[0]);

        // TODO Generate automatic validation of data, based on the DB Table specifications (e.g. 'not null' checks)
{#IFDEF VALIDATEDATAMANUAL}
        ValidateDataManual(FMainDS.{#MASTERTABLE}[0]);
{#ENDIF VALIDATEDATAMANUAL}


        CurrentRow = GetSelectedDetailRow();
        
        if (CurrentRow != null)
        {
            // TODO Generate automatic validation of data, based on the DB Table specifications (e.g. 'not null' checks)
{#IFDEF VALIDATEDATADETAILSMANUAL}
            ValidateDataDetailsManual(CurrentRow);
{#ENDIF VALIDATEDATADETAILSMANUAL}
{#IFDEF PERFORMUSERCONTROLVALIDATION}

            // Perform validation in UserControls, too
            {#USERCONTROLVALIDATION}
{#ENDIF PERFORMUSERCONTROLVALIDATION}
        }
        
        if (AProcessAnyDataValidationErrors)
        {
            ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
                this.GetType());
        }

        if(ReturnValue)
        {
            // Remove a possibly shown Validation ToolTip as the data validation succeeded
            FPetraUtilsObject.ValidationToolTip.RemoveAll();
        }

        return ReturnValue;
    }

#region Implement interface functions

    /// auto generated
    public void RunOnceOnActivation()
    {
        {#RUNONCEONACTIVATIONMANUAL}
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
        bool ReturnValue = false;
        
        FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());

//TODO?  still needed?      FMainDS.AApDocument.Rows[0].BeginEdit();
        GetDataFromControls(FMainDS.{#MASTERTABLE}[0]);

        // Clear any validation errors so that the following call to ValidateAllData starts with a 'clean slate'.
        FPetraUtilsObject.VerificationResultCollection.Clear();

        if (ValidateAllData(false, true))
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
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataInProgress);
                this.Cursor = Cursors.WaitCursor;

                TSubmitChangesResult SubmissionResult;
                TVerificationResultCollection VerificationResult;

                {#DATASETTYPE} SubmitDS = FMainDS.GetChangesTyped(true);
                
                if (SubmitDS == null)
                {
                    // There is nothing to be saved.
                    // Update UI
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);
                    this.Cursor = Cursors.Default;

                    // We don't have unsaved changes anymore
                    FPetraUtilsObject.DisableSaveButton();

                    return true;
                }

                // Submit changes to the PETRAServer
                try
                {
                    SubmissionResult = {#WEBCONNECTORMASTER}.Save{#MASTERTABLE}(ref SubmitDS, out VerificationResult);
                }
                catch (ESecurityDBTableAccessDeniedException Exp)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                    this.Cursor = Cursors.Default;

                    TMessages.MsgSecurityException(Exp, this.GetType());
                    
                    ReturnValue = false;
                    FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
                catch (EDBConcurrencyException Exp)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);

                    this.Cursor = Cursors.Default;

                    TMessages.MsgDBConcurrencyException(Exp, this.GetType());

                    ReturnValue = false;
                    FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                    return ReturnValue;
                }
                catch (Exception)
                {
                    FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataException);
                    this.Cursor = Cursors.Default;

                    FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));                    
                    throw;
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
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataSuccessful);
                        this.Cursor = Cursors.Default;

                        // We don't have unsaved changes anymore
                        FPetraUtilsObject.DisableSaveButton();

                        SetPrimaryKeyReadOnly(true);

                        ReturnValue = true;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        break;

                    case TSubmitChangesResult.scrError:
                        this.Cursor = Cursors.Default;
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataErrorOccured);

                        MessageBox.Show(Messages.BuildMessageFromVerificationResult(null, VerificationResult), 
                            Catalog.GetString("Data Cannot Be Saved"), MessageBoxButtons.OK, MessageBoxIcon.Error);

                        FPetraUtilsObject.SubmitChangesContinue = false;

                        ReturnValue = false;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        break;

                    case TSubmitChangesResult.scrNothingToBeSaved:

                        this.Cursor = Cursors.Default;
                        FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);

                        // We don't have unsaved changes anymore
                        FPetraUtilsObject.DisableSaveButton();
                        
                        ReturnValue = true;
                        FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
                        break;

                    case TSubmitChangesResult.scrInfoNeeded:

                        // TODO scrInfoNeeded
                        this.Cursor = Cursors.Default;
                        break;
                }
            }
            else
            {
                // Update UI
                FPetraUtilsObject.WriteToStatusBar(MCommonResourcestrings.StrSavingDataNothingToSave);
                this.Cursor = Cursors.Default;
                FPetraUtilsObject.DisableSaveButton();

                // We don't have unsaved changes anymore
                FPetraUtilsObject.HasChanges = false;

                ReturnValue = true;
                FPetraUtilsObject.OnDataSaved(this, new TDataSavedEventArgs(ReturnValue));
            }                
        }

        return ReturnValue;
    }
#endregion

#region Action Handling

    /// auto generated
    public void ActionEnabledEvent(object sender, ActionEventArgs e)
    {
        {#ACTIONENABLING}
        {#ACTIONENABLINGDISABLEMISSINGFUNCS}
    }

    {#ACTIONHANDLERS}

#endregion
{#IFDEF DATAVALIDATION}

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
                    FPetraUtilsObject.ValidationToolTip.Show(SingleVerificationResult.ResultText, (Control)sender, 
                        ((Control)sender).Width / 2, ((Control)sender).Height);
                }
            }
        }
    }

    private void BuildValidationControlsDict()
    {
        {#ADDCONTROLTOVALIDATIONCONTROLSDICT}
    }
    
#endregion
{#ENDIF DATAVALIDATION}
  }
}

{#INCLUDE copyvalues.cs}
{#INCLUDE validationcontrolsdict.cs}