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
    /// Validates the {#TABLENAME} DataTable.
    /// </summary>
    /// <param name="AContext">Context that describes where the data validation failed.</param>
    /// <param name="ARow">The <see cref="DataRow" /> which holds the the data against which the validation is run.</param>
    /// <param name="AVerificationResultCollection">Will be filled with any <see cref="TVerificationResult" /> items if
    /// data validation errors occur.</param>
    /// <param name="AValidationControlsDict">A <see cref="TValidationControlsDict" /> containing the Controls that
    /// display data that is about to be validated.</param>
    public static void Validate(object AContext, {#TABLENAME}Row ARow,
        ref TVerificationResultCollection AVerificationResultCollection, TValidationControlsDict AValidationControlsDict)
    {
        DataColumn ValidationColumn;
        TValidationControlsData ValidationControlsData;
        TVerificationResult VerificationResult;

        {#VALIDATECOLUMNS}
    }
}

{##VALIDATECOLUMN}
ValidationColumn = ARow.Table.Columns[{#TABLENAME}Table.Column{#COLUMNNAME}Id];

if (AValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
{
    {#COLUMNSPECIFICCHECK}

    // Handle addition to/removal from TVerificationResultCollection
    AVerificationResultCollection.Auto_Add_Or_AddOrRemove(AContext, VerificationResult, ValidationColumn);
}

{##CHECKEMPTYSTRING}
VerificationResult = TStringChecks.StringMustNotBeEmpty(ARow.{#COLUMNNAME},
    ValidationControlsData.ValidationControlLabel,
    AContext, ValidationColumn, ValidationControlsData.ValidationControl);

{##CHECKGENERALNOTNULL}
VerificationResult = TGeneralChecks.ValueMustNotBeNull(ARow.Is{#COLUMNNAME}Null() ? null : "",
    ValidationControlsData.ValidationControlLabel,
    AContext, ValidationColumn, ValidationControlsData.ValidationControl);
    