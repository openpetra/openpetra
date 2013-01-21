{##DYNAMICTABPAGESUBSEQUENTACTIVATION}
OnTabPageEvent(new TTabPageEventArgs({#CONTROLNAME}, FUco{#CONTROLNAMEWITHOUTPREFIX}, "SubsequentActivation"));

/*
 * The following command seems strange and unnecessary; however, it is necessary
 * to make things scale correctly on "Large Fonts (120DPI)" display setting.
 */
if (TClientSettings.GUIRunningOnNonStandardDPI)
{
    FUco{#CONTROLNAMEWITHOUTPREFIX}.AdjustAfterResizing();
}

FCurrentUserControl = FUco{#CONTROLNAMEWITHOUTPREFIX};