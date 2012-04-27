{##DYNAMICTABPAGEUSERCONTROLSETUP}
if (TClientSettings.DelayedDataLoading)
{
    // Signalise the user that data is beeing loaded
    this.Cursor = Cursors.AppStarting;
}

FUco{#CONTROLNAMEWITHOUTPREFIX} = ({#DYNAMICCONTROLTYPE})DynamicLoadUserControl(TDynamicLoadableUserControls.dluc{#CONTROLNAMEWITHOUTPREFIX});
FUco{#CONTROLNAMEWITHOUTPREFIX}.MainDS = FMainDS;
FUco{#CONTROLNAMEWITHOUTPREFIX}.PetraUtilsObject = FPetraUtilsObject;
PreInitUserControl(FUco{#CONTROLNAMEWITHOUTPREFIX});
FUco{#CONTROLNAMEWITHOUTPREFIX}.InitUserControl();
{#IFDEF ISUSERCONTROL}
((IFrmPetraEdit)(this.ParentForm)).GetPetraUtilsObject().HookupAllInContainer(FUco{#CONTROLNAMEWITHOUTPREFIX});
{#ENDIF ISUSERCONTROL}

OnTabPageEvent(new TTabPageEventArgs({#CONTROLNAME}, FUco{#CONTROLNAMEWITHOUTPREFIX}, "InitialActivation"));

this.Cursor = Cursors.Default;

FCurrentUserControl = FUco{#CONTROLNAMEWITHOUTPREFIX};