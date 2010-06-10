{##TABPAGESELECTIONCHANGED}
/*
 * Raise the following Event to inform the base Form that we might be loading some fresh data.
 * We need to bypass the ChangeDetection routine while this happens.
 */
OnDataLoadingStarted();

{#DYNAMICTABPAGEUSERCONTROLINITIALISATION}

/*
 * Raise the following Event to inform the base Form that we have finished loading fresh data.
 * We need to turn the ChangeDetection routine back on.
 */
OnDataLoadingFinished();

{#INCLUDE dynamictabpage_usercontrol_initialisation.cs}