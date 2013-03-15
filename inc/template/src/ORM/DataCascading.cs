// auto generated with nant generateORM
// Do not modify this file manually!
//
{#GPLFILEHEADER}

namespace {#NAMESPACE}
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.Odbc;
    using Ict.Common;
    using Ict.Common.DB;
    using Ict.Common.Verification;
    using Ict.Common.Data;
    using Ict.Tools.DBXML;
    {#USINGNAMESPACES}

    {#TABLECASCADINGLOOP}
}

{##TABLECASCADING}

/// auto generated
public class {#TABLENAME}Cascading : TTypedDataAccess
{
    /// cascading delete
    public static void DeleteByPrimaryKey({#FORMALPARAMETERSPRIMARYKEY}, TDBTransaction ATransaction, bool AWithCascDelete)
    {
{#IFDEF DELETEBYPRIMARYKEYCASCADING}
        int countRow;

        if ((AWithCascDelete == true))
        {
            {#DELETEBYPRIMARYKEYCASCADING}
        }

{#ENDIF DELETEBYPRIMARYKEYCASCADING}
        {#TABLENAME}Access.DeleteByPrimaryKey({#ACTUALPARAMETERSPRIMARYKEY}, ATransaction);
    }
    
    /// cascading delete
    public static void DeleteUsingTemplate({#TABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascDelete)
    {
{#IFDEF DELETEBYTEMPLATECASCADING}
        int countRow;

        if ((AWithCascDelete == true))
        {
            {#DELETEBYTEMPLATECASCADING}
        }

{#ENDIF DELETEBYTEMPLATECASCADING}
        {#TABLENAME}Access.DeleteUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
    }

    /// cascading count
    public static int CountByPrimaryKey({#FORMALPARAMETERSPRIMARYKEY}, TDBTransaction ATransaction, bool AWithCascCount, out List<TRowReferenceInfo>AReferences)
    {
        int OverallReferences = 0;
        AReferences = new List<TRowReferenceInfo>();

{#IFDEF COUNTBYPRIMARYKEYCASCADING}
        int countRow;
        int SingleTableReferences = 0;
        Dictionary<string, object> PKInfo = null;

        if ((AWithCascCount == true))
        {
            {#COUNTBYPRIMARYKEYCASCADING}
        }

{#ENDIF COUNTBYPRIMARYKEYCASCADING}
        return OverallReferences;
    }

    /// cascading count
    public static int CountByPrimaryKey({#FORMALPARAMETERSPRIMARYKEY}, TDBTransaction ATransaction, bool AWithCascCount, out TVerificationResultCollection AVerificationResults,
        TResultSeverity AResultSeverity = TResultSeverity.Resv_Critical)
    {   
        int ReturnValue;
        List<TRowReferenceInfo> References;
        Dictionary<string, object> PKInfo = null;
        
        ReturnValue = {#TABLENAME}Cascading.CountByPrimaryKey({#ACTUALPARAMETERSPRIMARYKEY}, ATransaction, AWithCascCount, out References);

        if(ReturnValue > 0)
        {
            PKInfo = new Dictionary<string, object>({#PRIMARYKEYCOLUMNCOUNT});
            {#PRIMARYKEYINFODICTBUILDING}
        
            AVerificationResults = TTypedDataAccess.BuildVerificationResultCollectionFromRefTables("{#TABLENAME}", "{#THISTABLELABEL}", PKInfo, References, AResultSeverity);
        }
        else
        {
            AVerificationResults = null;
        }
        
        return ReturnValue;
    }
    
    /// cascading count
    public static int CountByPrimaryKey(object[] APrimaryKeyValues, TDBTransaction ATransaction, bool AWithCascCount, out TVerificationResultCollection AVerificationResults,
        TResultSeverity AResultSeverity = TResultSeverity.Resv_Critical)
    {   
        if((APrimaryKeyValues == null)
          || (APrimaryKeyValues.Length == 0))
        {
            throw new ArgumentException("APrimaryKeyValues must not be null and must contain at least one element");
        }
        
        return {#TABLENAME}Cascading.CountByPrimaryKey({#ACTUALPARAMETERSPRIMARYKEYFROMPKARRAY}, ATransaction, AWithCascCount, out AVerificationResults);
    }

    /// cascading count
    public static int CountUsingTemplate({#TABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction, bool AWithCascCount, ref List<TRowReferenceInfo>AReferences)
    {
        int OverallReferences = 0;
        int SingleTableReferences = 0;
        Dictionary<string, object> PKInfo = null;
        
{#IFDEF COUNTBYTEMPLATECASCADING}
        int countRow;

        if ((AWithCascCount == true))
        {
            {#COUNTBYTEMPLATECASCADING}
        }

{#ENDIF COUNTBYTEMPLATECASCADING}

        SingleTableReferences = {#TABLENAME}Access.CountUsingTemplate(ATemplateRow, ATemplateOperators, ATransaction);
        AReferences.Add(new TRowReferenceInfo("{#TABLENAME}", "{#THISTABLELABEL}", SingleTableReferences, ATemplateRow));
        OverallReferences += SingleTableReferences;

        return OverallReferences;
    }
}

{##DELETEBYPRIMARYKEYCASCADING}
{#OTHERTABLENAME}Table {#MYOTHERTABLENAME}Table = {#OTHERTABLENAME}Access.Load{#VIAPROCEDURENAME}({#ACTUALPARAMETERSPRIMARYKEY}, StringHelper.StrSplit("{#CSVLISTOTHERPRIMARYKEYFIELDS}", ","), ATransaction);
for (countRow = 0; (countRow != {#MYOTHERTABLENAME}Table.Rows.Count); countRow = (countRow + 1))
{
{#IFDEF OTHERTABLEALSOCASCADING}
    {#OTHERTABLENAME}Cascading.DeleteUsingTemplate({#MYOTHERTABLENAME}Table[countRow], null, ATransaction, AWithCascDelete);
{#ENDIF OTHERTABLEALSOCASCADING}
{#IFNDEF OTHERTABLEALSOCASCADING}
    {#OTHERTABLENAME}Access.DeleteUsingTemplate({#MYOTHERTABLENAME}Table[countRow], null, ATransaction);
{#ENDIFN OTHERTABLEALSOCASCADING}
}

{##DELETEBYTEMPLATECASCADING}
{#OTHERTABLENAME}Table {#MYOTHERTABLENAME}Table = {#OTHERTABLENAME}Access.Load{#VIAPROCEDURENAME}Template(ATemplateRow, StringHelper.StrSplit("{#CSVLISTOTHERPRIMARYKEYFIELDS}", ","), ATransaction);
for (countRow = 0; (countRow != {#MYOTHERTABLENAME}Table.Rows.Count); countRow = (countRow + 1))
{
{#IFDEF OTHERTABLEALSOCASCADING}
    {#OTHERTABLENAME}Cascading.DeleteUsingTemplate({#MYOTHERTABLENAME}Table[countRow], null, ATransaction, AWithCascDelete);
{#ENDIF OTHERTABLEALSOCASCADING}
{#IFNDEF OTHERTABLEALSOCASCADING}
    {#OTHERTABLENAME}Access.DeleteUsingTemplate({#MYOTHERTABLENAME}Table[countRow], null, ATransaction);
{#ENDIFN OTHERTABLEALSOCASCADING}
}

{##COUNTBYPRIMARYKEYCASCADING}
SingleTableReferences = 0;
{#OTHERTABLENAME}Table {#MYOTHERTABLENAME}Table = {#OTHERTABLENAME}Access.Load{#VIAPROCEDURENAME}({#ACTUALPARAMETERSPRIMARYKEY}, StringHelper.StrSplit("{#CSVLISTOTHERPRIMARYKEYFIELDS}", ","), ATransaction);
for (countRow = 0; (countRow != {#MYOTHERTABLENAME}Table.Rows.Count); countRow = (countRow + 1))
{
    SingleTableReferences += {#OTHERTABLENAME}Cascading.CountUsingTemplate({#MYOTHERTABLENAME}Table[countRow], null, ATransaction, AWithCascCount, ref AReferences);
}
if(SingleTableReferences > 0)
{
    OverallReferences += SingleTableReferences;

    // Create Primary Key information for that referencing DB Table once and add it to last instance of AReferences - that will have been added in the for loop
    PKInfo = new Dictionary<string, object>({#PRIMARYKEYCOLUMNCOUNT2});
    {#PRIMARYKEYINFODICTBUILDING2}
    
    AReferences[AReferences.Count - 1].SetPKInfo(PKInfo);
}

{##COUNTBYTEMPLATECASCADING}
SingleTableReferences = 0;
{#OTHERTABLENAME}Table {#MYOTHERTABLENAME}Table = {#OTHERTABLENAME}Access.Load{#VIAPROCEDURENAME}Template(ATemplateRow, StringHelper.StrSplit("{#CSVLISTOTHERPRIMARYKEYFIELDS}", ","), ATransaction);
for (countRow = 0; (countRow != {#MYOTHERTABLENAME}Table.Rows.Count); countRow = (countRow + 1))
{
    SingleTableReferences = {#OTHERTABLENAME}Cascading.CountUsingTemplate({#MYOTHERTABLENAME}Table[countRow], null, ATransaction, AWithCascCount, ref AReferences);
}
if(SingleTableReferences > 0)
{
    OverallReferences += SingleTableReferences;

    // Create Primary Key information for that referencing DB Table once and add it to last instance of AReferences - that will have been added in the for loop
    PKInfo = new Dictionary<string, object>({#PRIMARYKEYCOLUMNCOUNT2});
    {#PRIMARYKEYINFODICTBUILDING2}
    
    AReferences[AReferences.Count - 1].SetPKInfo(PKInfo);
}


{##PRIMARYKEYINFODICTBUILDING}
PKInfo.Add("{#PKCOLUMNLABEL}", {#PKCOLUMNCONTENT});