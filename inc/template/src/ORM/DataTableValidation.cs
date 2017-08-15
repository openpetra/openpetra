// auto generated with nant generateORM
// Do not modify this file manually!
//
{#GPLFILEHEADER}

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Runtime.Serialization;
using System.Xml;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Validation;
using {#DATATABLENAMESPACE};

namespace {#NAMESPACE}
{
    {#TABLELOOP}
}

{##TABLEVALIDATION}

{#TABLE_DESCRIPTION}
public class {#TABLENAME}Validation
{
    /// <summary>
    /// Validates a row in the {#TABLENAME} DataTable.
    /// </summary>
{#IFNDEF VALIDATECOLUMNS}
    /// <remarks>No automatic data validation code was generated for this DB Table.</remarks>
{#ENDIFN VALIDATECOLUMNS}
    /// <param name="AContext">Context that describes where the data validation failed.</param>
    /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
    /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
    /// data validation errors occur.</param>
    public static void Validate(object AContext, {#TABLENAME}Row ARow,
        ref TVerificationResultCollection AVerificationResultCollection)
    {
{#IFDEF VALIDATECOLUMNS}
        DataColumn ValidationColumn;
        TVerificationResult VerificationResult;

{#IFNDEF DELETABLEROWVALIDATION}
        // Don't validate deleted DataRows
{#ENDIFN DELETABLEROWVALIDATION}
        if ((ARow.RowState == DataRowState.Deleted) || (ARow.RowState == DataRowState.Detached))
        {
            {#DELETABLEROWVALIDATION}
            return;
        }
        
        {#VALIDATECOLUMNS}
{#ENDIF VALIDATECOLUMNS}
{#IFNDEF VALIDATECOLUMNS}
        // No automatic data validation code was generated for this DB Table.
{#ENDIFN VALIDATECOLUMNS}
    }

    /// <summary>
    /// Validates the {#TABLENAME} DataTable.
    /// </summary>
    /// <param name="ASubmitTable">validate all rows in this table</param>
    /// <param name="AVerificationResult">Will be filled with any <see cref="TVerificationResult" /> items if
    /// data validation errors occur.</param>
    public static void Validate(
        TTypedDataTable ASubmitTable,
        ref TVerificationResultCollection AVerificationResult)
    {
        for (int Counter = 0; Counter < ASubmitTable.Rows.Count; Counter++)
        {
            if (ASubmitTable.Rows[Counter].RowState != DataRowState.Deleted)
            {
                Validate("{#TABLENAME}Validation " +
                    " (Error in Row #" + Counter.ToString() + ")",  // No translation of message text since the server's messages should be all in English
                    ({#TABLENAME}Row)ASubmitTable.Rows[Counter], ref AVerificationResult);
            }
        }
    }
}

{##VALIDATECOLUMN}

if (!ARow.Is{#COLUMNNAME}Null())
{
    // {#COLUMNSPECIFICCOMMENT}
    ValidationColumn = ARow.Table.Columns[{#TABLENAME}Table.Column{#COLUMNNAME}Id];

    if (true)
    {
        {#COLUMNSPECIFICCHECK}

        // Handle addition to/removal from TVerificationResultCollection
        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
    }
}

{##VALIDATECOLUMN2}

// {#COLUMNSPECIFICCOMMENT}
ValidationColumn = ARow.Table.Columns[{#TABLENAME}Table.Column{#COLUMNNAME}Id];

if (true)
{
    {#COLUMNSPECIFICCHECK}

    // Handle addition to/removal from TVerificationResultCollection
    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult);
}

{##SNIPDELETABLEROWVALIDATION}
if (ARow.RowState == DataRowState.Deleted)
{
    // Special case for server side validation during saving
    if (Convert.ToBoolean(ARow[{#TABLENAME}Table.Column{#COLUMNNAME}Id, DataRowVersion.Original]) == false)
    {
        // Add an error notification
        AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext,
            new TVerificationResult(AContext, ErrorCodes.GetErrorInfo(CommonErrorCodes.ERR_RECORD_DELETION_NOT_POSSIBLE_BY_DESIGN)));
    }
}
// No further validation on deleted or detached rows


{##CHECKEMPTYSTRING}
VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.{#COLUMNNAME},
    String.Empty,
    AContext, ValidationColumn);

{##CHECKSTRINGLENGTH}
VerificationResult = TStringChecks.StringLengthLesserOrEqual(ARow.{#COLUMNNAME}, {#COLUMNLENGTH},
    String.Empty,
    AContext, ValidationColumn);

{##CHECKNUMBERRANGE}
VerificationResult = TNumericalChecks.IsNumberPrecisionNotExceeded(ARow.{#COLUMNNAME}, {#NUMBEROFDECIMALDIGITS}, {#NUMBEROFFRACTIONALDIGITS},
    String.Empty,
    AContext, ValidationColumn);

{##CHECKEMPTYDATE}
VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.{#COLUMNNAME},
    String.Empty, AVerificationResultCollection, true,
    AContext, ValidationColumn);

{##CHECKVALIDDATE}
VerificationResult = TSharedValidationControlHelper.IsNotInvalidDate(ARow.{#COLUMNNAME},
    String.Empty, AVerificationResultCollection, false,
    AContext, ValidationColumn);
    

{##CHECKGENERALNOTNULL}
VerificationResult = TGeneralChecks.ValueMustNotBeNull(ARow.Is{#COLUMNNAME}Null() ? null : "",
    String.Empty,
    AContext, ValidationColumn);
    
