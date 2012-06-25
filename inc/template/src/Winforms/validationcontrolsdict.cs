{##VALIDATIONCONTROLSDICTADD}
if (FMainDS.{#TABLENAME} != null)
{
    FPetraUtilsObject.ValidationControlsDict.Add({#COLUMNID},
        new TValidationControlsData({#VALIDATIONCONTROL}, {#LABELTEXT}));{#AUTOMATICVALIDATIONCOMMENT}
}
    
{##VALIDATIONCONTROLSDICTADDMULTI}
if (FMainDS.{#TABLENAME} != null)
{
    FPetraUtilsObject.ValidationControlsDict.Add({#COLUMNID},
        new TValidationControlsData({#VALIDATIONCONTROL}, {#LABELTEXT},
                                    {#VALIDATIONCONTROL2}, {#LABELTEXT2}));{#AUTOMATICVALIDATIONCOMMENT}
}