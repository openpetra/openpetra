// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template windowFind
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
using SourceGrid;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.CommonControls;
{#IFDEF SHAREDVALIDATIONNAMESPACEMODULE}
using {#SHAREDVALIDATIONNAMESPACEMODULE};
{#ENDIF SHAREDVALIDATIONNAMESPACEMODULE}
{#USINGNAMESPACES}

namespace {#NAMESPACE}
{

  /// auto generated: {#FORMTITLE}
  public partial class {#CLASSNAME}: System.Windows.Forms.Form, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;
{#IFDEF DATASETTYPE}
    private {#DATASETTYPE} FMainDS;
{#ENDIF DATASETTYPE}

{#IFDEF UICONNECTORTYPE}

    /// <summary>holds a reference to the Proxy object of the Serverside UIConnector</summary>
    private {#UICONNECTORTYPE} FUIConnector = null;
{#ENDIF UICONNECTORTYPE}

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
{#IFDEF DATASETTYPE}
      FMainDS = new {#DATASETTYPE}();
{#ENDIF DATASETTYPE}
      {#INITUSERCONTROLS}
{#IFDEF ACTIONENABLING}
      FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
{#ENDIF ACTIONENABLING}
      {#INITMANUALCODE}

      {#INITACTIONSTATE}
      
{#IFDEF UICONNECTORCREATE}
      FUIConnector = {#UICONNECTORCREATE}();
      // Register Object with the TEnsureKeepAlive Class so that it doesn't get GC'd
      TEnsureKeepAlive.Register(FUIConnector);
{#ENDIF UICONNECTORCREATE}

{#IFDEF MASTERTABLE OR DETAILTABLE}
      BuildValidationControlsDict();
{#ENDIF MASTERTABLE OR DETAILTABLE}
    }

    {#EVENTHANDLERSIMPLEMENTATION}

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position

{#IFDEF UICONNECTORCREATE}
        if (FUIConnector != null)
        {
            // UnRegister Object from the TEnsureKeepAlive Class so that the Object can get GC'd on the PetraServer
            TEnsureKeepAlive.UnRegister(FUIConnector);
            FUIConnector = null;
        }
{#ENDIF UICONNECTORCREATE}
    }

{#IFDEF SHOWDATA}
    private void ShowData({#MASTERTABLE}Row ARow)
    {
        {#SHOWDATA}
    }
{#ENDIF SHOWDATA}

{#IFDEF MASTERTABLE}

    private void GetDataFromControls({#MASTERTABLETYPE}Row ARow, Control AControl=null)
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

    private void GetDetailsFromControls({#DETAILTABLETYPE}Row ARow, Control AControl=null)
    {
        if (ARow != null)
        {
            ARow.BeginEdit();
            {#SAVEDETAILS}
            ARow.EndEdit();
        }
    }
{#IFDEF GENERATECONTROLUPDATEDATAHANDLER}

    private void ControlUpdateDataHandler(object sender, EventArgs e)
    {
        GetDetailsFromControls(FPreviouslySelectedDetailRow, (Control)sender);
    }
{#ENDIF GENERATECONTROLUPDATEDATAHANDLER}
{#ENDIF SAVEDETAILS}
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
    /// <param name="AValidateSpecificControl">Pass in a Control to restrict Data Validation error checking to a 
    /// specific Control for which Data Validation errors might have been recorded. (Default=this.ActiveControl).
    /// <para>
    /// This is useful for restricting Data Validation error checking to the current TabPage of a TabControl in order
    /// to only display Data Validation errors that pertain to the current TabPage. To do this, pass in a TabControl in
    /// this Argument.
    /// </para>    
    /// </param>
    /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>    
    private bool ValidateAllData(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors, Control AValidateSpecificControl = null)
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
        if (FMainDS.{#MASTERTABLE}.Rows.Count > 0)
        {
          GetDataFromControls(FMainDS.{#MASTERTABLE}[0]);
          ValidateData(FMainDS.{#MASTERTABLE}[0]);
        }
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
        if (FMainDS.{#MASTERTABLE}.Rows.Count > 0)
        {
            ValidateDataManual(FMainDS.{#MASTERTABLE}[0]);
        }
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
            // on this Form has got the Focus.
            if (ControlToValidate != null) 
            {
                if(ControlToValidate.FindUserControlOrForm(true) == this)
                {
{#IFDEF SHOWDETAILS}
                    ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(ARecordChangeVerification, FPetraUtilsObject.VerificationResultCollection,
                        this.GetType(), ARecordChangeVerification ? ControlToValidate.FindUserControlOrForm(true).GetType() : null);
{#ENDIF SHOWDETAILS}
{#IFNDEF SHOWDETAILS}
                    ReturnValue = TDataValidation.ProcessAnyDataValidationErrors(false, FPetraUtilsObject.VerificationResultCollection,
                        this.GetType(), ControlToValidate.FindUserControlOrForm(true).GetType());
{#ENDIFN SHOWDETAILS}
                }
                else
                {
                    ReturnValue = true;
                }
            }            
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
{#IFDEF SHOWDETAILS OR GENERATEGETSELECTEDDETAILROW}

    /// return the selected row
    public {#DETAILTABLETYPE}Row GetSelectedDetailRow()
    {
        {#GETSELECTEDDETAILROW}
    }
{#ENDIF SHOWDETAILS OR GENERATEGETSELECTEDDETAILROW}

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
        if (FMainDS != null)
        {
{#IFDEF ADDCONTROLTOVALIDATIONCONTROLSDICT}
            {#ADDCONTROLTOVALIDATIONCONTROLSDICT}
{#ENDIF ADDCONTROLTOVALIDATIONCONTROLSDICT}
        }
    }
{#ENDIF MASTERTABLE OR DETAILTABLE}    

#endregion

  }
}

{#INCLUDE copyvalues.cs}
{#INCLUDE validationcontrolsdict.cs}