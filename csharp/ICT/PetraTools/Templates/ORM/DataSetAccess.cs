/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
{#GPLFILEHEADER}

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.DB;
using Ict.Petra.Shared;
using System;
using System.Data;
using System.Data.Odbc;
{#USINGNAMESPACES}

namespace {#NAMESPACE}.Access
{
    {#CONTENTDATASETSANDTABLESANDROWS}
}

{##TYPEDDATASET}
 /// auto generated
[Serializable()]
public class {#DATASETNAME}Access : {#DATASETNAME}
{
    {#SUBMITCHANGESFUNCTION}
}

{##SUBMITCHANGESFUNCTION}

/// auto generated
public TSubmitChangesResult SubmitChanges(out TVerificationResultCollection AVerificationResult)
{
    TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
    TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

    AVerificationResult = new TVerificationResultCollection();

    try
    {
        SubmissionResult = TSubmitChangesResult.scrOK;
        
        {#SUBMITCHANGESDELETE}
        {#SUBMITCHANGESINSERT}
        {#SUBMITCHANGESUPDATE}
        
        if (SubmissionResult == TSubmitChangesResult.scrOK)
        {
            DBAccess.GDBAccessObj.CommitTransaction();
        }
        else
        {
            DBAccess.GDBAccessObj.RollbackTransaction();
        }
    }
    catch (Exception e)
    {
        TLogging.Log("exception during saving dataset {#DATASETNAME}:" + e.Message);

        DBAccess.GDBAccessObj.RollbackTransaction();

        throw new Exception(e.ToString() + " " + e.Message);
    }

    return SubmissionResult;
}

{##SUBMITCHANGES}
if (SubmissionResult == TSubmitChangesResult.scrOK 
    && !TTypedDataAccess.SubmitChanges({#TABLEVARIABLENAME}, SubmitChangesTransaction,
            TTypedDataAccess.eSubmitChangesOperations.{#SQLOPERATION},
            out AVerificationResult,
            UserInfo.GUserInfo.UserID{#SEQUENCENAMEANDFIELD}))
{
    SubmissionResult = TSubmitChangesResult.scrError;
}
