{##SETSECURITYCONTEXT}
FSecurityContext = {#SECURITYCONTEXT};

{##APPLYSECURITY}
var SecurityPermissionRequired = new List<string>();
SecurityPermissionRequired.Add(TSecurityChecks.SECURITYPERMISSION_EDITING_AND_SAVING_OF_SETUP_DATA);
{#IFDEF APPLYSECURITYMANUAL}
FPetraUtilsObject.ApplySecurity(SecurityPermissionRequired, ApplySecurityManual);
{#ENDIF APPLYSECURITYMANUAL}
{#IFNDEF APPLYSECURITYMANUAL}
FPetraUtilsObject.ApplySecurity(SecurityPermissionRequired);
{#ENDIFN APPLYSECURITYMANUAL}

{##APPLYNOSECURITY}
// FPetraUtilsObject.ApplySecurity doesn't get called here because 'AutomaticApplySecurityExecution' was set to false in the Form's YAML file