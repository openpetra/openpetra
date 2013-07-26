// auto generated with nant generateWinforms from {#XAMLSRCFILE} and template reportwindow.cs
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
using SourceGrid;
using GNU.Gettext;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MReporting;
using System.Resources;
using System.Collections.Specialized;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;

namespace {#NAMESPACE}
{

  /// <summary>
  /// auto generated class for report
  /// </summary>
  public partial class {#CLASSNAME}: System.Windows.Forms.Form, {#INTERFACENAME}
  {
    private {#UTILOBJECTCLASS} FPetraUtilsObject;
    private Boolean FCalledFromExtracts = false;
    
    /// <summary>
    /// constructor
    /// </summary>
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

{#IFDEF CALCULATEFROMMETHOD}
      FPetraUtilsObject.FCalculateFromMethod = "{#CALCULATEFROMMETHOD}";
{#ENDIF CALCULATEFROMMETHOD}
{#IFDEF ISOLATIONLEVEL}
      FPetraUtilsObject.FIsolationLevel = "{#ISOLATIONLEVEL}";
{#ENDIF ISOLATIONLEVEL}
{#IFDEF XMLFILES}
      FPetraUtilsObject.FXMLFiles = "{#XMLFILES}";
{#ENDIF XMLFILES}
      FPetraUtilsObject.FReportName = "{#REPORTNAME}";
      FPetraUtilsObject.FCurrentReport = "{#CURRENTREPORT}";
      FPetraUtilsObject.FSettingsDirectory = "{#REPORTSETTINGSDIRECTORY}";
      
      // Hook up Event that is fired by ucoReportColumns
      // ucoReportColumns.FillColumnGridEventHandler += new TFillColumnGridEventHandler(FPetraUtilsObject.FillColumnGrid);
      FPetraUtilsObject.InitialiseData("");
      // FPetraUtilsObject.InitialiseSettingsGui(ucoReportColumns, mniLoadSettings, /*ConMnuLoadSettings*/null, 
      //                                 mniSaveSettings, mniSaveSettingsAs, mniLoadSettingsDialog, mniMaintainSettings);
      this.SetAvailableFunctions();
      
      {#INITIALISESCREEN}
      
      {#INITUSERCONTROLS}

      {#INITMANUALCODE}
      FPetraUtilsObject.LoadDefaultSettings();
    }

    #region Show Method overrides

    /// <summary>
    /// Override of Form.Show(IWin32Window owner) Method. Caters for singleton Forms.
    /// </summary>
    /// <param name="owner">Any object that implements <see cref="IWin32Window" /> and represents the top-level window that will own this Form. </param>    
    public new void Show(IWin32Window owner)
    {
        Form OpenScreen = TFormsList.GFormsList[this.GetType().FullName];
        bool OpenSelf = true;

        if ((OpenScreen != null)
            && (OpenScreen.Modal != true))            
        {
            if (TFormsList.GSingletonForms.Contains(this.GetType().Name)) 
            {
//                MessageBox.Show("Activating singleton screen of Type '" + this.GetType().FullName + "'.");
                                   
                OpenSelf = false;
                this.Visible = false;   // needed as this.Close() would otherwise bring this Form to the foreground and OpenScreen.BringToFront() would not help...
                this.Close();
                
                OpenScreen.BringToFront();
            }            
        }
        
        if (OpenSelf) 
        {
            if (owner != null) 
            {
                base.Show(owner);    
            }
            else
            {
                base.Show();
            }            
        }        
    }

    /// <summary>
    /// Override of Form.Show() Method. Caters for singleton Forms.
    /// </summary>        
    public new void Show()
    {
        this.Show(null);
    }

    #endregion

    {#EVENTHANDLERSIMPLEMENTATION}

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
    }

    /// helper object for the whole screen
    public Boolean CalledFromExtracts
    {
        get
        {
            return FCalledFromExtracts;
        }

        set
        {
            FCalledFromExtracts = value;
        }
    }
    
#region Parameter/Settings Handling
    /** 
       Reads the selected values from the controls, and stores them into the parameter system of FCalculator

    */
    public void ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
    {
      ACalc.SetMaxDisplayColumns(FPetraUtilsObject.FMaxDisplayColumns);
      
      {#READCONTROLSLOCALVARS}

      {#READCONTROLS}

      {#READCONTROLSVERIFICATION}
    }

    /** 
       Sets the selected values in the controls, using the parameters loaded from a file

    */
    public void SetControls(TParameterList AParameters)
    {
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
      
      {#ADDAVAILABLEFUNCTIONS}
      
      {#SETAVAILABLEFUNCTIONS}
      
    }
#endregion
        
#region Implement interface functions

    /// <summary>
    /// only run this code once during activation
    /// </summary>
    public void RunOnceOnActivation()
    {
        if (CalledFromExtracts)
        {
            tbbGenerateReport.Visible = false;
        }
        else
        {
            tbbGenerateExtract.Visible = false;
        }
        
        {#RUNONCEONACTIVATIONMANUAL}
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
    /// <param name="AReportParameter">Initialisation values needed for some reports</param>
    public void InitialiseData(String AReportParameter)
    {
        FPetraUtilsObject.InitialiseData(AReportParameter);
    }
    
    /// <summary>
    /// Checks / Unchecks the menu item "Wrap Columns"
    /// </summary>
    /// <param name="ACheck">True if menu item is to be checked. Otherwise false</param>
    public void CheckWrapColumnMenuItem(bool ACheck)
    {
        this.mniWrapColumn.Checked = ACheck;
    }

    /// <summary>
    /// activate and deactivate toolbar buttons and menu items depending on ongoing report calculation
    /// </summary>
    /// <param name="ABusy">True if a report is generated and the close button should be disabled.</param>
    public void EnableBusy(bool ABusy)
    {
        mniClose.Enabled = !ABusy;

        if (ABusy == false)
        {
            mniGenerateReport.Text = Catalog.GetString("&Generate Report...");
            tbbGenerateReport.Text = Catalog.GetString("Generate");
            tbbGenerateReport.ToolTipText = Catalog.GetString("Generate a report and display the preview");
        }
        else
        {
            mniGenerateReport.Text = Catalog.GetString("&Cancel Report");
            tbbGenerateReport.Text = Catalog.GetString("Cancel");
            tbbGenerateReport.ToolTipText = Catalog.GetString("Cancel the calculation of the report (after cancelling it might still take a while)");
        }
    }

#endregion

    /// <summary>
    /// allow to store and load settings
    /// </summary>
    /// <param name="AEnabled">True if the store and load settings are to be enabled.</param>
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
        //tbbLoadSettings.Enabled = AEnabled;
        tbbSaveSettings.Enabled = AEnabled;
        tbbSaveSettingsAs.Enabled = AEnabled;
    }

    /// <summary>
    /// this is used for writing the captions of the menu items and toolbar buttons for recently used report settings
    /// </summary>
    /// <returns>false if an item with that index does not exist</returns>
    /// <param name="AIndex"></param>
    /// <param name="mniItem"></param>
    /// <param name="tbbItem"></param>
    public bool GetRecentSettingsItems(int AIndex, out ToolStripItem mniItem, out ToolStripItem tbbItem)
    {
        if (AIndex < 0 || AIndex >= mniLoadSettings.DropDownItems.Count - 2)
        {
            mniItem = null;
            tbbItem = null;
            return false;
        }
        mniItem = mniLoadSettings.DropDownItems[AIndex + 2];
        // TODO
        tbbItem = null;
        return true;
    }

#region Action Handling

    {#ACTIONHANDLERS}

#endregion
  }
}

{##RADIOBUTTONREADCONTROLS}
if ({#RBTNAME}.Checked)
{
    ACalc.AddParameter("{#PARAMNAME}", "{#RBTVALUE}");
    {#READCONTROLS}
}

{##RADIOBUTTONSETCONTROLS}
{#RBTNAME}.Checked = AParameters.Get("{#PARAMNAME}").ToString() == "{#RBTVALUE}";
{#IFDEF SETCONTROLS}
if ({#RBTNAME}.Checked)
{
    {#SETCONTROLS}
}
{#ENDIF SETCONTROLS}

{##CHECKBOXREADCONTROLS}
ACalc.AddParameter("{#PARAMNAME}", this.{#CONTROLNAME}.Checked);

{##CHECKBOXSETCONTROLS}
{#CONTROLNAME}.Checked = AParameters.Get("{#PARAMNAME}").ToBool();

{##TCMBAUTOPOPULATEDREADCONTROLS}
ACalc.AddParameter("{#PARAMNAME}", this.{#CONTROLNAME}.GetSelectedString());

{##TCMBAUTOPOPULATEDSETCONTROLS}
{#IFDEF CLEARIFSETTINGEMPTY}
{#CONTROLNAME}.SetSelectedString(AParameters.Get("{#PARAMNAME}").ToString(), -1);
{#ENDIF CLEARIFSETTINGEMPTY}
{#IFNDEF CLEARIFSETTINGEMPTY}
{#CONTROLNAME}.SetSelectedString(AParameters.Get("{#PARAMNAME}").ToString());
{#ENDIFN CLEARIFSETTINGEMPTY}

{##COMBOBOXREADCONTROLS}
if (this.{#CONTROLNAME}.SelectedItem != null)
{
    ACalc.AddParameter("{#PARAMNAME}", this.{#CONTROLNAME}.SelectedItem.ToString());
}
else
{
    ACalc.AddParameter("{#PARAMNAME}", "");
}

{##COMBOBOXSETCONTROLS}
if (AParameters.Exists("{#PARAMNAME}"))
{
    {#CONTROLNAME}.SelectedValue = AParameters.Get("{#PARAMNAME}").ToString();
}

{##TEXTBOXREADCONTROLS}
ACalc.AddParameter("{#PARAMNAME}", this.{#CONTROLNAME}.Text);

{##TEXTBOXSETCONTROLS}
{#CONTROLNAME}.Text = AParameters.Get("{#PARAMNAME}").ToString();

{##INTEGERTEXTBOXREADCONTROLS}
ACalc.AddParameter("{#PARAMNAME}", this.{#CONTROLNAME}.Text);

{##INTEGERTEXTBOXSETCONTROLS}
{#CONTROLNAME}.NumberValueInt = AParameters.Get("{#PARAMNAME}").ToInt32();

{##DECIMALTEXTBOXREADCONTROLS}
ACalc.AddParameter("{#PARAMNAME}", this.{#CONTROLNAME}.Text);

{##DECIMALTEXTBOXSETCONTROLS}
{#CONTROLNAME}.NumberValueDecimal = AParameters.Get("{#PARAMNAME}").ToDecimal();

{##TCLBVERSATILEREADCONTROLS}
ACalc.AddParameter("{#PARAMNAME}", this.{#CONTROLNAME}.GetCheckedStringList());

{##TCLBVERSATILESETCONTROLS}
{#CONTROLNAME}.SetCheckedStringList(AParameters.Get("{#PARAMNAME}").ToString());

{##TTXTPETRADATEREADCONTROLS}
ACalc.AddParameter("{#PARAMNAME}", this.{#CONTROLNAME}.Date);

{##TTXTPETRADATESETCONTROLS}
{#IFDEF CLEARIFSETTINGEMPTY}
if (!AParameters.Get("{#PARAMNAME}").IsNil())
{
    DateTime {#CONTROLNAME}Date = AParameters.Get("{#PARAMNAME}").ToDate();
    if (({#CONTROLNAME}Date <= DateTime.MinValue)
        || ({#CONTROLNAME}Date >= DateTime.MaxValue))
    {
        {#CONTROLNAME}Date = DateTime.Now;
    }
    {#CONTROLNAME}.Date = {#CONTROLNAME}Date;
}
else
{
    {#CONTROLNAME}.Text = "";
}
{#ENDIF CLEARIFSETTINGEMPTY}
{#IFNDEF CLEARIFSETTINGEMPTY}
DateTime {#CONTROLNAME}Date = AParameters.Get("{#PARAMNAME}").ToDate();
if (({#CONTROLNAME}Date <= DateTime.MinValue)
    || ({#CONTROLNAME}Date >= DateTime.MaxValue))
{
    {#CONTROLNAME}Date = DateTime.Now;
}
{#CONTROLNAME}.Date = {#CONTROLNAME}Date;
{#ENDIFN CLEARIFSETTINGEMPTY}
