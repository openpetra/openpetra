{##SHOWDATAFORCOLUMN}
{#IFDEF NOTDEFAULTTABLE}
{#IFDEF CANBENULL}
{#SETROWVALUEORNULL}
{#ENDIF CANBENULL}
{#IFNDEF CANBENULL}
{#SETROWVALUE}
{#ENDIFN CANBENULL}
{#ENDIF NOTDEFAULTTABLE}
{#IFNDEF NOTDEFAULTTABLE}
{#IFDEF CANBENULL}
{#SETVALUEORNULL}
{#ENDIF CANBENULL}
{#IFNDEF CANBENULL}
{#SETCONTROLVALUE}
{#ENDIFN CANBENULL}
{#ENDIFN NOTDEFAULTTABLE}


{##UNDODATAFORCOLUMN}
if(AControl.Name == "{#CONTROLNAME}")
{
{#IFDEF NOTDEFAULTTABLE}
{#UNDOROWVALUE}
{#ENDIF NOTDEFAULTTABLE}
{#IFNDEF NOTDEFAULTTABLE}
{#UNDOCONTROLVALUE}
{#ENDIFN NOTDEFAULTTABLE}
}

{##GETDATAFORCOLUMNTHATCANBENULL}
if((AControl == null)
  || (AControl.Name == "{#CONTROLNAME}"))
{
{#IFDEF CANBENULL}
{#IFDEF NOTDEFAULTTABLE}
{#GETROWVALUEORNULL}
{#ENDIF NOTDEFAULTTABLE}
{#IFNDEF NOTDEFAULTTABLE}
{#GETVALUEORNULL}
{#ENDIFN NOTDEFAULTTABLE}
{#ENDIF CANBENULL}
{#IFNDEF CANBENULL}
{#ROW}.{#COLUMNNAME} = {#CONTROLVALUE};
{#ENDIFN CANBENULL}
}

{##SETROWVALUEORNULL}
if ({#NOTDEFAULTTABLE} == null || (({#NOTDEFAULTTABLE}.Rows.Count > 0) && ({#NOTDEFAULTTABLE}[0].Is{#COLUMNNAME}Null())))
{
    {#SETNULLVALUE}
}
else
{
    if ({#NOTDEFAULTTABLE}.Rows.Count > 0)
    {
        {#SETCONTROLVALUE}
    }
}

{##SETROWVALUE}
if ({#NOTDEFAULTTABLE} != null)
{
    {#SETCONTROLVALUE}
}
else
{
    {#SETNULLVALUE}
}

{##UNDOROWVALUE}
if ({#NOTDEFAULTTABLE} != null)
{
    {#UNDOCONTROLVALUE}
}

{##GETROWVALUEORNULL}
if (({#NOTDEFAULTTABLE} != null) && ({#NOTDEFAULTTABLE}.Rows.Count > 0))
{
    if ({#DETERMINECONTROLISNULL})
    {
        {#NOTDEFAULTTABLE}[0].Set{#COLUMNNAME}Null();
    }
    else
    {
        {#NOTDEFAULTTABLE}[0].{#COLUMNNAME} = {#CONTROLVALUE};
    }
}

{##GETROWVALUEORNULLSTRING}
if (({#NOTDEFAULTTABLE} != null) && ({#NOTDEFAULTTABLE}.Rows.Count > 0))
{
    {#ROW}.{#COLUMNNAME} = {#CONTROLVALUE};
}

{##SETVALUEORNULL}
if ({#ROW}.Is{#COLUMNNAME}Null())
{
    {#SETNULLVALUE}
}
else
{
    {#SETCONTROLVALUE}
}

{##GETVALUEORNULL}
if ({#DETERMINECONTROLISNULL})
{
    if (!{#ROW}.Is{#COLUMNNAME}Null())
    {
        {#ROW}.Set{#COLUMNNAME}Null();
    }
}
else
{
    {#ROW}.{#COLUMNNAME} = {#CONTROLVALUE};
}

{##GETSELECTEDDETAILROW}
DataRowView[] SelectedGridRow = grdDetails.SelectedDataRowsAsDataRowView;

if (SelectedGridRow.Length >= 1)
{
    return ({#DETAILTABLETYPE}Row)SelectedGridRow[0].Row;
}

return null;

{##PROCESSCMDKEYCTRLF}
if (keyData == (Keys.F | Keys.Control))
{
    {#ACTIONCLICK}("mniEditFind", null);
    return true;
}
if (keyData == Keys.F3)
{
    {#ACTIONCLICK}("mniEditFindNext", null);
    return true;
}
if (keyData == (Keys.F3 | Keys.Shift))
{
    {#ACTIONCLICK}("mniEditFindPrevious", null);
    return true;
}

{##PROCESSCMDKEYCTRLR}
if (keyData == (Keys.R | Keys.Control))
{
    {#ACTIONCLICK}("mniEditFilter", null);
    return true;
}

{##PROCESSCMDKEYCTRLS}
if (keyData == (Keys.S | Keys.Control))
{
    if (mniFileSave.Enabled)
    {
        FileSave(null, null);
    }
    return true;
}


{##PROCESSCMDKEYMANUAL}
if (ProcessCmdKeyManual(ref msg, keyData))
{
    return true;
}

{##PROCESSCMDKEYCTRLL}
if (keyData == (Keys.L | Keys.Control))
{
    grdDetails.Focus();
    return true;
}

{##PROCESSCMDKEYSELECTROW}
if (keyData == (Keys.Home | Keys.Control))
{
    SelectRowInGrid(1);
    FocusFirstEditableControl();
    return true;
}
if (keyData == ((Keys.Up | Keys.Control)))
{
    SelectRowInGrid(GetSelectedRowIndex() - 1);
    FocusFirstEditableControl();
    return true;
}
if (keyData == (Keys.Down | Keys.Control))
{
    SelectRowInGrid(GetSelectedRowIndex() + 1);
    FocusFirstEditableControl();
    return true;
}
if (keyData == ((Keys.End | Keys.Control)))
{
    SelectRowInGrid(grdDetails.Rows.Count - 1);
    FocusFirstEditableControl();
    return true;
}

{##FOCUSFIRSTDETAILSPANELCONTROL}
// Build up a list of controls on this panel that is sorted in true nested tab order
SortedList<string, Control> controlsSortedByTabOrder = new SortedList<string, Control>();
GetSortedControlList(ref controlsSortedByTabOrder, pnlDetails, String.Empty);

int index;
for (index = 0; index < controlsSortedByTabOrder.Count; index++)
{
    if (controlsSortedByTabOrder.Values[index] as TextBox != null)
    {
        if (!((TextBox)controlsSortedByTabOrder.Values[index]).ReadOnly)
        {
            break;
        }
    }
    else if (controlsSortedByTabOrder.Values[index].Enabled)
    {
        break;
    }
}
    
if (index < controlsSortedByTabOrder.Count)
{
    controlsSortedByTabOrder.Values[index].Focus();
}
