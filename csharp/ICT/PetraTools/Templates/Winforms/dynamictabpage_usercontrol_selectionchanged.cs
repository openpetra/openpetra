{##TABPAGESELECTIONCHANGED}
{#IFDEF ISUSERCONTROL}
/*
 * Raise the following Event to inform the base Form that we might be loading some fresh data.
 * We need to bypass the ChangeDetection routine while this happens.
 */
OnDataLoadingStarted();

{#ENDIF ISUSERCONTROL}
{#DYNAMICTABPAGEUSERCONTROLINITIALISATION}

{#IFDEF ISUSERCONTROL}
/*
 * Raise the following Event to inform the base Form that we have finished loading fresh data.
 * We need to turn the ChangeDetection routine back on.
 */
OnDataLoadingFinished();
{#ENDIF ISUSERCONTROL}

{#INCLUDE dynamictabpage_usercontrol_initialisation.cs}