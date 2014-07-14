{##FILTERANDFINDDECLARATIONS}
#region Filter and Find

TFilterAndFindPanel FFilterAndFindObject = null;

#endregion

{##FILTERANDFINDMETHODS}
/// Handler for the menu items Edit/Filter and Edit/Find
/// If this is part of a user control, it can be called from the parent
public void MniFilterFind_Click(object sender, EventArgs e)
{
    if ((this.ActiveControl == null) || ValidateAllData(true, true))
    {
        FFilterAndFindObject.MniFilterFind_Click(sender, e);
    }
}

#region IFilterAndFind interface methods

/// <summary>
/// Required interface method
/// </summary>
public void InitialiseFilterFindParameters(out TUcoFilterAndFind.FilterAndFindParameters AParams)
{
    // Settings are defined in the YAML file
    AParams = new TUcoFilterAndFind.FilterAndFindParameters(
        {#FINDANDFILTERINITIALWIDTH}, {#FINDANDFILTERINITIALLYEXPANDED},
        {#FINDANDFILTERAPPLYFILTERBUTTONCONTEXT}, {#FINDANDFILTERSHOWKEEPFILTERTURNEDONBUTTONCONTEXT}, 
        {#FINDANDFILTERSHOWFILTERISALWAYSONLABELCONTEXT});
}

/// <summary>
/// Required interface method
/// </summary>
public void CreateFilterFindPanels()
{
    TIndividualFilterFindPanel iffp;

    // Standard Filter Panel
    {#INDIVIDUALFILTERPANELS}

    // Extra Filter Panel
    {#INDIVIDUALEXTRAFILTERPANELS}
        
    // Find Panel
    {#INDIVIDUALFINDPANELS}

    // Filter/Find Panel Events
    {#INDIVIDUALFILTERFINDPANELEVENTS}
        
    // Filter/Find Panel Properties
    {#INDIVIDUALFILTERFINDPANELPROPERTIES}
        
    // Optional manual code is applied here
	{#CREATEFILTERFINDPANELSMANUAL}
}

/// <summary>
/// Required interface method
/// </summary>
public bool DoValidation(bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors)
{
	return ValidateAllData(ARecordChangeVerification, AProcessAnyDataValidationErrors);
}

/// <summary>
/// Required interface method
/// </summary>
public void AddNumericColumnConversions()
{
    {#NUMERICFILTERFINDCOLUMNS}
}

/// <summary>
/// Required interface method
/// </summary>
public void FilterToggled()
{
    {#FILTERTOGGLEDMANUAL}
}

/// <summary>
/// Required interface method
/// </summary>
public bool IsMatchingRow(DataRow ARow)
{
	return {#ISMATCHINGROW}(ARow);
}

/// <summary>
/// Required interface method
/// </summary>
public void ApplyFilterString(ref string AFilterString)
{
    {#APPLYFILTERMANUAL}
}

#endregion

{##SNIPCLONELABEL}
TCloneFilterFindControl.ShallowClone<Label>({#CLONEDFROMLABEL}, T{#PANELTYPE}PanelControls.{#PANELTYPEUC}_NAME_SUFFIX),

{##SNIPINDIVIDUALFILTERFINDPANEL}
iffp = new TIndividualFilterFindPanel(
    {#CLONELABEL}TCloneFilterFindControl.{#CONTROLCLONE}<{#CONTROLTYPE}>({#CLONEDFROMCONTROL}, T{#PANELTYPE}PanelControls.{#PANELTYPEUC}_NAME_SUFFIX),
    {#DETAILTABLETYPE}Table.Get{#COLUMNNAME}DBName(),
    "{#COLUMNDATATYPE}",
    {#TAG});
FFilterAndFindObject.{#PANELTYPE}PanelControls.F{#PANELSUBTYPE}Panels.Add(iffp);

{##SNIPINDIVIDUALFILTERFINDPANELNOCOLUMN}
iffp = new TIndividualFilterFindPanel(
    {#CLONELABEL}TCloneFilterFindControl.{#CONTROLCLONE}<{#CONTROLTYPE}>({#CLONEDFROMCONTROL}, T{#PANELTYPE}PanelControls.{#PANELTYPEUC}_NAME_SUFFIX),
    null,
    null,
    {#TAG});
FFilterAndFindObject.{#PANELTYPE}PanelControls.F{#PANELSUBTYPE}Panels.Add(iffp);

{##SNIPDYNAMICCREATECONTROL}
{#CONTROLTYPE} {#CONTROLNAME} = new {#CONTROLTYPE}();
{#CONTROLNAME}.Name = "{#CONTROLNAME}";

{##SNIPDYNAMICCREATECONTROLWITHTEXT}
{#CONTROLTYPE} {#CONTROLNAME} = new {#CONTROLTYPE}();
{#CONTROLNAME}.Name = "{#CONTROLNAME}";
{#CONTROLNAME}.Text = "{#CONTROLTEXT}";

{##SNIPDYNAMICEVENTHANDLER}
(({#CONTROLTYPE})FFilterAndFindObject.{#PANELTYPE}PanelControls.FindControlByName("{#CONTROLNAME}")).{#EVENTNAME} += new System.EventHandler(this.{#EVENTHANDLER});

{##SNIPDYNAMICSETPROPERTY}
(({#CONTROLTYPE})FFilterAndFindObject.{#PANELTYPE}PanelControls.FindControlByName("{#CONTROLNAME}")).{#PROPERTYNAME} = {#PROPERTYVALUE};

{##SNIPNUMERICFILTERFINDCOLUMN}
FMainDS.{#DETAILTABLE}.Columns.Add(new DataColumn("{#COLUMNNAME}_text", typeof(System.String), "CONVERT({#COLUMNNAME}, 'System.String')"));

{##SNIPDYNAMICCREATERGR}
{#CREATERGR}

{#CREATERBTNS}

{#CLONERGR}

