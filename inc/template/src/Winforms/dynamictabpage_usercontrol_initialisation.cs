{##USERCONTROLINITIALISATION}
if ({#TABCONTROLNAME}.SelectedTab == {#CONTROLNAME})
{
    if (!FTabSetup.ContainsKey({#TABKEY})
    {
        {#DYNAMICTABPAGEUSERCONTROLSETUPINLINE1}
    }
    else
    {    
        {#DYNAMICTABPAGESUBSEQUENTACTIVATION}
    }
}

{#INCLUDE dynamictabpage_usercontrol_subsequentactivation.cs}