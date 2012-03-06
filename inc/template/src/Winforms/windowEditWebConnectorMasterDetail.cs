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
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
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
                break;
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
    }
{#ENDIF SHOWDATA}

{#IFDEF SHOWDETAILS}
    private void ShowDetails({#DETAILTABLETYPE}Row ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        {#SHOWDETAILS}
        FPetraUtilsObject.EnableDataChangedEvent();
    }

    private {#DETAILTABLETYPE}Row FPreviouslySelectedDetailRow = null;
    private void FocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
    {
{#IFDEF SAVEDETAILS}
        // get the details from the previously selected row
        if (FPreviouslySelectedDetailRow != null)
        {
            GetDetailsFromControls(FPreviouslySelectedDetailRow);
        }
{#ENDIF SAVEDETAILS}
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
        FPetraUtilsObject.OnDataSavingStart(this, new System.EventArgs());

//TODO?  still needed?      FMainDS.AApDocument.Rows[0].BeginEdit();
        GetDataFromControls(FMainDS.{#MASTERTABLE}[0]);

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

                {#DATASETTYPE} SubmitDS = FMainDS.GetChangesTyped(true);
                
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
                    SubmissionResult = {#WEBCONNECTORMASTER}.Save{#MASTERTABLE}(ref SubmitDS, out VerificationResult);
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
        {#ACTIONENABLING}
        {#ACTIONENABLINGDISABLEMISSINGFUNCS}
    }

    {#ACTIONHANDLERS}

#endregion
  }
}

{#INCLUDE copyvalues.cs}