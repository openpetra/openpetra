// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template controlMaintainTable
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
using System.Resources;
using System.Collections.Specialized;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Shared;
using GNU.Gettext;
using SourceGrid;
{#IFDEF SHAREDVALIDATIONNAMESPACEMODULE}
using {#SHAREDVALIDATIONNAMESPACEMODULE};
{#ENDIF SHAREDVALIDATIONNAMESPACEMODULE}
{#USINGNAMESPACES}

namespace {#NAMESPACE}
{

  /// auto generated user control
  public partial class {#CLASSNAME}: System.Windows.Forms.UserControl, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;

    private {#DATASETTYPE} FMainDS;
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

    /// dataset for the whole screen
    public {#DATASETTYPE} MainDS
    {
        set
        {
            FMainDS = value;
        }
    }

    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
        {#INITUSERCONTROLS}
        {#INITMANUALCODE}
{#IFDEF ACTIONENABLING}
        FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
{#ENDIF ACTIONENABLING}
      
        if((FMainDS != null)
          && (FMainDS.{#DETAILTABLE} != null))
        {
            DataView myDataView = FMainDS.{#DETAILTABLE}.DefaultView;
            myDataView.AllowNew = false;
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);

            BuildValidationControlsDict();

            ShowData();
        }
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


    /// make sure that the primary key cannot be edited anymore
    public void SetPrimaryKeyReadOnly(bool AReadOnly)
    {
        {#PRIMARYKEYCONTROLSREADONLY}
    }
    private bool pnlDetailsProtected = false;
    /// <summary>
    /// protect the pnlDetail (only for this tab)
    /// </summary>
    public bool PnlDetailsProtected
    {
        get { return pnlDetailsProtected; }
        set { pnlDetailsProtected = value; }
    }
    
    private void ShowData()
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        {#SHOWDATA}
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
                pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode && !pnlDetailsProtected;
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
{#IFDEF SHOWDETAILS}
    private void ShowDetails({#DETAILTABLETYPE}Row ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        grdDetails.Selection.FocusRowLeaving -= new SourceGrid.RowCancelEventHandler(FocusRowLeaving);

        if (ARow == null)
        {
            pnlDetails.Enabled = false;
            {#CLEARDETAILS}
        }
        else
        {
            FPreviouslySelectedDetailRow = ARow;
            {#SHOWDETAILS}
            pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode && !pnlDetailsProtected;
        }
        FPetraUtilsObject.EnableDataChangedEvent();
        grdDetails.Selection.FocusRowLeaving += new SourceGrid.RowCancelEventHandler(FocusRowLeaving);
    }

    private {#DETAILTABLETYPE}Row FPreviouslySelectedDetailRow = null;
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
{#IFDEF SAVEDETAILS}
            // Transfer data from Controls into the DataTable
            if (FPreviouslySelectedDetailRow != null)
            {
                GetDetailsFromControls(FPreviouslySelectedDetailRow);
            }
{#ENDIF SAVEDETAILS}

            // Display the details of the currently selected Row
            FPreviouslySelectedDetailRow = GetSelectedDetailRow();
            ShowDetails(FPreviouslySelectedDetailRow);
            pnlDetails.Enabled = true;
            
            FCurrentRow = e.Row;
        }
    }
{#ENDIF SHOWDETAILS}
{#IFDEF MASTERTABLE}

	/// get the data from the controls and store in the currently selected detail row
    public void GetDataFromControls({#MASTERTABLETYPE}Row ARow)
    {
{#IFDEF SAVEDATA}
        {#SAVEDATA}
{#ENDIF SAVEDATA}
    }
{#ENDIF MASTERTABLE}
{#IFNDEF MASTERTABLE}

    /// get the data from the controls and store in the currently selected detail row
    public void GetDataFromControls()
    {
{#IFDEF SAVEDETAILS}
        GetDetailsFromControls(FPreviouslySelectedDetailRow);
{#ENDIF SAVEDETAILS}
    }
{#ENDIFN MASTERTABLE}
    
{#IFDEF SAVEDETAILS}

    private void GetDetailsFromControls({#DETAILTABLETYPE}Row ARow)
    {
        if (ARow != null && !pnlDetailsProtected)
        {
            ARow.BeginEdit();
            {#SAVEDETAILS}
            ARow.EndEdit();
        }
    }

    /// <summary>
    /// Performs data validation.
    /// </summary>
    /// <remarks>May be called by the Form that hosts this UserControl to invoke the data validation of
    /// the UserControl.</remarks>    
    /// <param name="ARecordChangeVerification">Set to true if the data validation happens when the user is changing 
    /// to another record, otherwise set it to false.</param>
    /// <param name="AProcessAnyDataValidationErrors">Set to true if data validation errors should be shown to the
    /// user, otherwise set it to false.</param>
    /// <param name="AValidateSpecificControl">Pass in a Control to restrict Data Validation error checking to a 
    /// specific Control for which Data Validation errors might have been recorded. (Default=this.ActiveControl).
    /// <para>
    /// This is useful for restricting Data Validation error checking to the current TabPage of a TabControl in order
    /// to only display Data Validation errors that pertain to the current TabPage. To do this, pass in a TabControl in
    /// this Argument.
    /// </para>
    /// </param>
    /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
    public bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors, Control AValidateSpecificControl = null)
    {
        bool ReturnValue = false;
        Control ControlToValidate = null;

        // Record a new Data Validation Run. (All TVerificationResults/TScreenVerificationResults that are created during this 'run' are associated with this 'run' through that.)
        FPetraUtilsObject.VerificationResultCollection.RecordNewDataValidationRun();

{#IFDEF SHOWDETAILS}
        {#DETAILTABLETYPE}Row CurrentRow;

        CurrentRow = GetSelectedDetailRow();

        if (CurrentRow != null)
        {
{#ENDIF SHOWDETAILS}
        if (AValidateSpecificControl != null) 
        {
            ControlToValidate = AValidateSpecificControl;
        }
        else
        {
            ControlToValidate = this.ActiveControl;
        }

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
                // Only process the Data Validations here if ControlToValidate is not null.
                // It can be null if this.ActiveControl yields null - this would happen if no Control
                // on this UserControl has got the Focus.
                if(ControlToValidate.FindUserControlOrForm(true) == this)
                {
                    ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(false, FPetraUtilsObject.VerificationResultCollection,
                        this.GetType(), ControlToValidate.FindUserControlOrForm(true).GetType());
                }
                else
                {
                    ReturnValue = true;
                }
            }
        }
        else
        {
            ReturnValue = true;
        }

        if(ReturnValue)
        {
            // Remove a possibly shown Validation ToolTip as the data validation succeeded
            FPetraUtilsObject.ValidationToolTip.RemoveAll();
        }

        return ReturnValue;
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
        
        ValidateAllData(true, false, (Control)sender);
        
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

{#IFDEF UNDODATA}
                    if(SingleVerificationResult.ControlValueUndoRequested)
                    {
                        UndoData(SingleVerificationResult.ResultColumn.Table.Rows[0], SingleVerificationResult.ResultControl);
                        SingleVerificationResult.OverrideResultText(SingleVerificationResult.ResultText + Environment.NewLine + Environment.NewLine + 
                            Catalog.GetString("--> The value you entered has been changed back to what it was before! <--"));
                    }

{#ENDIF UNDODATA}
                    if (!SingleVerificationResult.SuppressValidationToolTip) 
                    {
                        FPetraUtilsObject.ValidationToolTipSeverity = SingleVerificationResult.ResultSeverity;

                        if (SingleVerificationResult.ResultTextCaption != String.Empty) 
                        {
                            FPetraUtilsObject.ValidationToolTip.ToolTipTitle += ":  " + SingleVerificationResult.ResultTextCaption;    
                        }

                        FPetraUtilsObject.ValidationToolTip.Show(SingleVerificationResult.ResultText, (Control)sender, 
                            ((Control)sender).Width / 2, ((Control)sender).Height);
                    }
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