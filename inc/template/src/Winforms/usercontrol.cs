// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template usercontrol
//
// DO NOT edit manually, DO NOT edit with the designer
//
{#GPLFILEHEADER}
using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
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
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Client.CommonForms;
{#IFDEF SHAREDVALIDATIONNAMESPACEMODULE}
using {#SHAREDVALIDATIONNAMESPACEMODULE};
{#ENDIF SHAREDVALIDATIONNAMESPACEMODULE}
{#USINGNAMESPACES}

namespace {#NAMESPACE}
{

  /// auto generated user control
  public partial class {#CLASSNAME}: {#BASECLASSNAME}, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;
    
    /// <summary>
    /// Dictionary that contains Controls on whose data Data Validation should be run.
    /// </summary>
    private TValidationControlsDict FValidationControlsDict = new TValidationControlsDict();
    
    private {#DATASETTYPE} FMainDS;

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
        get
        {
            return FPetraUtilsObject;
        }

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

    /// <summary>todoComment</summary>
    public event System.EventHandler DataLoadingStarted;

    /// <summary>todoComment</summary>
    public event System.EventHandler DataLoadingFinished;

    /// to avoid warning CS0067: unused event
    private void OnDataLoadingStarted(object sender, EventArgs e)
    {
        if (DataLoadingStarted != null)
        {
            DataLoadingStarted(sender, e);
        }
    }

    /// to avoid warning CS0067: unused event
    private void OnDataLoadingFinished(object sender, EventArgs e)
    {
        if (DataLoadingFinished != null)
        {
            DataLoadingFinished(sender, e);
        }
    }

    /// needs to be called after FMainDS and FPetraUtilsObject have been set
    public void InitUserControl()
    {
        {#INITUSERCONTROLS}
{#IFDEF ACTIONENABLING}
        FPetraUtilsObject.ActionEnablingEvent += ActionEnabledEvent;
{#ENDIF ACTIONENABLING}
        {#INITMANUALCODE}

        if((FMainDS != null)
          && (FMainDS.{#MASTERTABLE} != null))
        {
{#IFDEF MASTERTABLE OR DETAILTABLE}
        BuildValidationControlsDict();
{#ENDIF MASTERTABLE OR DETAILTABLE}

            if(FMainDS.{#MASTERTABLE}.Count > 0)
            {
                ShowData(FMainDS.{#MASTERTABLE}[0]);
            }
        }
    }
    
    {#EVENTHANDLERSIMPLEMENTATION}
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

{#IFDEF SHOWDATA}
    private void ShowData({#MASTERTABLE}Row ARow)
    {
        FPetraUtilsObject.DisableDataChangedEvent();
        {#SHOWDATA}
        FPetraUtilsObject.EnableDataChangedEvent();
    }

{#ENDIF SHOWDATA}
{#IFDEF UNDODATA}

    private void UndoData(DataRow ARow, Control AControl)
    {
        {#UNDODATA}
    }
{#ENDIF UNDODATA}
    /// <summary>
    /// Performs data validation.
    /// </summary>
    /// <remarks>May be called by the Form that hosts this UserControl to invoke the data validation of
    /// the UserControl.</remarks>    
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
    /// <param name="ADontRecordNewDataValidationRun">Set to false if no new DataValidationRun should be recorded. 
    /// Should be set to true only if called from within this very UserControl to ensure that an external call to the 
    /// UserControl's ValidateAllData Method doesn't change a recorded DataValidationRun that was set from the 
    /// Form/UserControl that embeds this UserControl! (Default=true).</param>
    /// <returns>True if data validation succeeded or if there is no current row, otherwise false.</returns>
    public bool ValidateAllData(bool AProcessAnyDataValidationErrors, Control AValidateSpecificControl = null, bool ADontRecordNewDataValidationRun = true)
    {
        bool ReturnValue = false;
        Control ControlToValidate = null;

        if (!ADontRecordNewDataValidationRun)
        {
            // Record a new Data Validation Run. (All TVerificationResults/TScreenVerificationResults that are created during this 'run' are associated with this 'run' through that.)
            FPetraUtilsObject.VerificationResultCollection.RecordNewDataValidationRun();
        }
        
{#IFDEF SHOWDETAILS}
        {#DETAILTABLETYPE}Row CurrentRow;

        CurrentRow = GetSelectedDetailRow();

        if (CurrentRow != null)
        {
{#ENDIF SHOWDETAILS}
{#IFNDEF SHOWDETAILS}
        if (AValidateSpecificControl != null) 
        {
            ControlToValidate = AValidateSpecificControl;
        }
        else
        {
            ControlToValidate = this.ActiveControl;
        }

{#IFDEF MASTERTABLE}
        GetDataFromControls(GetMasterRow());
        ValidateData(GetMasterRow());
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
            ValidateDataManual(GetMasterRow());
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

{#IFDEF MASTERTABLE}
    private {#MASTERTABLETYPE}Row GetMasterRow()
    {
        if (FMainDS.{#MASTERTABLE}.Rows.Count > 1)
        {
{#IFDEF MULTIPLEMASTERROWS}
            // GetSelectedMasterRow() needs to be implemented in ManualCode file in this case 
            // as it is not automatically created with use of MASTERTABLE.
            return GetSelectedMasterRow();
{#ENDIF MULTIPLEMASTERROWS}
{#IFNDEF MULTIPLEMASTERROWS}
            return FMainDS.{#MASTERTABLE}[0];
{#ENDIFN MULTIPLEMASTERROWS}
        }
        else
        {
            return FMainDS.{#MASTERTABLE}[0];
        }
    }
{#ENDIF MASTERTABLE}        
    
{#IFDEF SHOWDETAILS}
    private void ShowDetails({#DETAILTABLETYPE}Row ARow)
    {
        {#SHOWDETAILS}
    }
{#ENDIF SHOWDETAILS}

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
        
        ValidateAllData(false, (Control)sender, false);
        
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
            FValidationControlsDict);
    }
{#ENDIF MASTERTABLE}
{#IFDEF DETAILTABLE}
    private void ValidateDataDetails({#DETAILTABLE}Row ARow)
    {
        TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

        {#DETAILTABLE}Validation.Validate(this, ARow, ref VerificationResultCollection,
            FValidationControlsDict);
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