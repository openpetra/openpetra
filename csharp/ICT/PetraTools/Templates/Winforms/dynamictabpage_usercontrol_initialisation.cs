{##USERCONTROLINITIALISATION}
if ({#TABCONTROLNAME}.SelectedTab == {#CONTROLNAME})
{
    if (!FTabSetup.ContainsKey(TDynamicLoadableUserControls.dluc{#CONTROLNAMEWITHOUTPREFIX}))
    {
        if (TClientSettings.DelayedDataLoading)
        {
            // Signalise the user that data is beeing loaded
            this.Cursor = Cursors.AppStarting;
        }

        FUco{#CONTROLNAMEWITHOUTPREFIX} = ({#DYNAMICCONTROLTYPE})DynamicLoadUserControl(TDynamicLoadableUserControls.dluc{#CONTROLNAMEWITHOUTPREFIX});
        FUco{#CONTROLNAMEWITHOUTPREFIX}.MainDS = FMainDS;
        FUco{#CONTROLNAMEWITHOUTPREFIX}.PetraUtilsObject = FPetraUtilsObject;
        FUco{#CONTROLNAMEWITHOUTPREFIX}.InitUserControl();
{#IFDEF ISUSERCONTROL}
        ((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUco{#CONTROLNAMEWITHOUTPREFIX});
{#ENDIF ISUSERCONTROL}

        OnTabPageEvent(new TTabPageEventArgs({#CONTROLNAME}, FUco{#CONTROLNAMEWITHOUTPREFIX}, "InitialActivation"));

        this.Cursor = Cursors.Default;
    }
    else
    {    
        OnTabPageEvent(new TTabPageEventArgs({#CONTROLNAME}, FUco{#CONTROLNAMEWITHOUTPREFIX}, "SubsequentActivation"));
        
        /*
         * The following command seems strange and unnecessary; however, it is necessary
         * to make things scale correctly on "Large Fonts (120DPI)" display setting.
         */
        if (TClientSettings.GUIRunningOnNonStandardDPI)
        {
            FUco{#CONTROLNAMEWITHOUTPREFIX}.AdjustAfterResizing();
        }        
    }
}