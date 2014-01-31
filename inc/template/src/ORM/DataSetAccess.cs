// auto generated with nant generateORM
// Do not modify this file manually!
//
{#GPLFILEHEADER}

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Petra.Shared;
using System;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
{#USINGNAMESPACES}

namespace {#NAMESPACE}.Access
{
    {#CONTENTDATASETSANDTABLESANDROWS}
}

{##TYPEDDATASET}
 /// auto generated
[Serializable()]
public class {#DATASETNAME}Access
{
    {#SUBMITCHANGESFUNCTION}
}

{##SUBMITCHANGESFUNCTION}

/// auto generated
static public void SubmitChanges({#DATASETNAME} AInspectDS)
{
    if (AInspectDS == null)
    {
        return;
    }

    bool NewTransaction;
    TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.Serializable, out NewTransaction);

    try
    {
        {#SUBMITCHANGESDELETE}
        {#SUBMITCHANGESINSERT}
        {#SUBMITCHANGESUPDATE}

        if (AInspectDS.ThrowAwayAfterSubmitChanges)
        {
            AInspectDS.Clear();
        }

        if (NewTransaction)
        {
            DBAccess.GDBAccessObj.CommitTransaction();
        }
    }
    catch (Exception e)
    {
        TLogging.Log("exception during saving dataset {#DATASETNAME}:" + e.Message);

        if (NewTransaction)
        {
            DBAccess.GDBAccessObj.RollbackTransaction();
        }

        throw;
    }
}

{##SUBMITCHANGES}
{#IFNDEF UPDATESEQUENCEINOTHERTABLES}
TTypedDataAccess.SubmitChanges(AInspectDS.{#TABLEVARIABLENAME}, SubmitChangesTransaction,
    TTypedDataAccess.eSubmitChangesOperations.{#SQLOPERATION},
    UserInfo.GUserInfo.UserID{#SEQUENCENAMEANDFIELD});
{#ENDIFN UPDATESEQUENCEINOTHERTABLES}
{#IFDEF UPDATESEQUENCEINOTHERTABLES}
if (AInspectDS.{#TABLEVARIABLENAME} != null)
{
    SortedList<Int64, Int32> OldSequenceValuesRow = new SortedList<Int64, Int32>();
    Int32 rowIndex = 0;

    foreach ({#TABLEROWTYPE} origRow in AInspectDS.{#TABLEVARIABLENAME}.Rows)
    {
        if (origRow.RowState != DataRowState.Deleted && origRow.{#SEQUENCEDCOLUMNNAME} < 0)
        {
            OldSequenceValuesRow.Add(origRow.{#SEQUENCEDCOLUMNNAME}, rowIndex);
        }

        rowIndex++;
    }

    TTypedDataAccess.SubmitChanges(AInspectDS.{#TABLEVARIABLENAME}, SubmitChangesTransaction,
        TTypedDataAccess.eSubmitChangesOperations.{#SQLOPERATION},
        UserInfo.GUserInfo.UserID{#SEQUENCENAMEANDFIELD});
    {#UPDATESEQUENCEINOTHERTABLES}
}
{#ENDIF UPDATESEQUENCEINOTHERTABLES}

{##UPDATESEQUENCEINOTHERTABLES}
if (AInspectDS.{#REFERENCINGTABLENAME} != null)
{
    foreach ({#REFERENCINGTABLEROWTYPE} otherRow in AInspectDS.{#REFERENCINGTABLENAME}.Rows)
    {
        if ((otherRow.RowState != DataRowState.Deleted) && {#TESTFORNULL}otherRow.{#REFCOLUMNNAME} < 0)
        {
            otherRow.{#REFCOLUMNNAME} = AInspectDS.{#TABLEVARIABLENAME}[OldSequenceValuesRow[otherRow.{#REFCOLUMNNAME}]].{#SEQUENCEDCOLUMNNAME};
        }
    }
}