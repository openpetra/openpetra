/* auto generated with nant generateWinforms from {#XAMLSRCFILE} 
 *
 * DO NOT edit manually, DO NOT edit with the designer
 * use a user control if you need to modify the screen content
 *
 */
{#GPLFILEHEADER}
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Mono.Unix;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using System.Resources;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Logic;

namespace {#NAMESPACE}
{

  /// <summary>
  /// auto generated class for report
  /// </summary>
  public partial class {#CLASSNAME}: System.Windows.Forms.Form, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;
      
    /// <summary>
    /// constructor
    /// </summary>
    public {#CLASSNAME}(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
      #region CATALOGI18N

      // this code has been inserted by GenerateI18N, all changes in this region will be overwritten by GenerateI18N
      {#CATALOGI18N}
      #endregion

      FPetraUtilsObject = new {#UTILOBJECTCLASS}(AParentFormHandle, this, stbMain);

      FPetraUtilsObject.FXMLFiles = "{#XMLFILES}";
      FPetraUtilsObject.FReportName = "{#REPORTNAME}";
      FPetraUtilsObject.FCurrentReport = "{#CURRENTREPORT}";
      
      // Hook up Event that is fired by ucoReportColumns
      // ucoReportColumns.FillColumnGridEventHandler += new TFillColumnGridEventHandler(FPetraUtilsObject.FillColumnGrid);
      FPetraUtilsObject.InitialiseData("");
      // FPetraUtilsObject.InitialiseSettingsGui(ucoReportColumns, mniLoadSettings, /*ConMnuLoadSettings*/null, 
      //                                 mniSaveSettings, mniSaveSettingsAs, mniLoadSettingsDialog, mniMaintainSettings);
      // this.SetAvailableFunctions();
      // ucoReportColumns.InitialiseData(FPetraUtilsObject.FColumnParameters);
      
      {#INITIALISESCREEN}
    }

    {#EVENTHANDLERSIMPLEMENTATION}

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
    }
#region Parameter/Settings Handling
    /** 
       Reads the selected values from the controls, and stores them into the parameter system of FCalculator

    */
    public void ReadControls(TRptCalculator ACalc)
    {
      //ucoReportSorting.ReadControls(ACalc);
      //ucoReportOutput.ReadControls(ACalc);
      
      {#READCONTROLSLOCALVARS}

      {#READCONTROLS}

      {#READCONTROLSVERIFICATION}
    }

    /** 
       Sets the selected values in the controls, using the parameters loaded from a file

    */
    public void SetControls(TParameterList AParameters)
    {
      //ucoReportSorting.SetControls(AParameters);
      //ucoReportOutput.SetControls(AParameters);

      {#SETCONTROLSLOCALVARS}

      {#SETCONTROLS}
    }
#endregion

#region Column Functions and Calculations
    /** 
       This will add functions to the list of available functions

    */
    public void SetAvailableFunctions()
    {
      //ArrayList availableFunctions = FPetraUtilsObject.InitAvailableFunctions();

      {#SETAVAILABLEFUNCTIONS}
      
      //ucoReportColumns.SetAvailableFunctions(availableFunctions);
      //ucoReportSorting.SetAvailableFunctions(availableFunctions);
    }
#endregion
        
#region Implement interface functions

    /// <summary>
    /// only run this code once during activation
    /// </summary>
    public void RunOnceOnActivation()
    {
        {#RUNONCEINTERFACEIMPLEMENTATION}
    }

    /// <summary>
    /// Adds event handlers for the appropiate onChange event to call a central procedure
    /// </summary>
    /// <returns>void</returns>
    public void HookupAllControls()
    {
        {#HOOKUPINTERFACEIMPLEMENTATION}
    }
    
    /// <summary>
    /// check if report window can be closed
    /// </summary>
    public bool CanClose()
    {
        return FPetraUtilsObject.CanClose();
    }
        
    /// <summary>
    /// access to the utility object
    /// </summary>
    public TFrmPetraUtils GetPetraUtilsObject()
    {
        return (TFrmPetraUtils)FPetraUtilsObject;
    }
    
    /// <summary>
    /// initialisation
    /// </summary>
    public void InitialiseData(String AReportParameter)
    {
        FPetraUtilsObject.InitialiseData(AReportParameter);
    }
#endregion

    /// <summary>
    /// allow to store and load settings
    /// </summary>
    public void EnableSettings(bool AEnabled)
    {   
        foreach (ToolStripItem item in mniLoadSettings.DropDownItems)
        {
            item.Enabled = AEnabled;
        }
        mniLoadSettings.Enabled = AEnabled;
        mniSaveSettings.Enabled = AEnabled;
        mniSaveSettingsAs.Enabled = AEnabled;
        mniMaintainSettings.Enabled = AEnabled;
        //tbbLoad.Enabled = AEnabled;
        tbbSave.Enabled = AEnabled;
        tbbSaveAs.Enabled = AEnabled;
    }

    /// <summary>
    /// activate and deactivate toolbar buttons and menu items depending on ongoing report calculation
    /// </summary>
    public void EnableBusy(bool ABusy)
    {
        mniClose.Enabled = !ABusy;

        if (ABusy == false)
        {
            mniGenerateReport.Text = "&Generate Report...";
            tbbGenerate.Text = "Generate";
            tbbGenerate.ToolTipText = "Generate a report and display the preview";
        }
        else
        {
            mniGenerateReport.Text = "&Cancel Report";
            tbbGenerate.Text = "Cancel";
            tbbGenerate.ToolTipText = "Cancel the calculation of the report (after cancelling it might still take a while)";
        }
    }
    
#region Action Handling

    {#ACTIONHANDLERS}

#endregion
  }
}
