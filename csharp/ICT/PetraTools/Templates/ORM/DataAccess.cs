/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
{#GPLFILEHEADER}
namespace {#NAMESPACE}
{
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.Odbc;
    using Ict.Common;
    using Ict.Common.DB;
    using Ict.Common.Verification;
    using Ict.Common.Data;
    using Ict.Petra.Shared;
    {#USINGNAMESPACES}

    {#TABLEACCESSLOOP}
}

{##TABLEACCESS}

/// {#TABLE_DESCRIPTION}
public class {#TABLENAME}Access : TTypedDataAccess
{

    /// CamelCase version of table name
    public const string DATATABLENAME = "{#TABLENAME}";

    /// original table name in database
    public const string DBTABLENAME = "{#SQLTABLENAME}";

    /// this method is called by all overloads
    public static void LoadAll(DataSet ADataSet, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
    {
        ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                        {#CSVLISTPRIMARYKEYFIELDS}}) + " FROM PUB_{#SQLTABLENAME}") 
                        + GenerateOrderByClause(AOrderBy)), {#TABLENAME}Table.GetTableName(), ATransaction, AStartRecord, AMaxRecords);
    }

    /// auto generated
    public static void LoadAll(DataSet AData, TDBTransaction ATransaction)
    {
        LoadAll(AData, null, ATransaction, null, 0, 0);
    }
    
    /// auto generated
    public static void LoadAll(DataSet AData, StringCollection AFieldList, TDBTransaction ATransaction)
    {
        LoadAll(AData, AFieldList, ATransaction, null, 0, 0);
    }

    /// auto generated
    public static void LoadAll(out {#TABLENAME}Table AData, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
    {
        DataSet FillDataSet = new DataSet();
        AData = new {#TABLENAME}Table();
        FillDataSet.Tables.Add(AData);
        LoadAll(FillDataSet, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        FillDataSet.Tables.Remove(AData);
    }
    
    /// auto generated
    public static void LoadAll(out {#TABLENAME}Table AData, TDBTransaction ATransaction)
    {
        LoadAll(out AData, null, ATransaction, null, 0, 0);
    }
    
    /// auto generated
    public static void LoadAll(out {#TABLENAME}Table AData, StringCollection AFieldList, TDBTransaction ATransaction)
    {
        LoadAll(out AData, AFieldList, ATransaction, null, 0, 0);
    }
{#IFDEF FORMALPARAMETERSPRIMARYKEY}

    /// this method is called by all overloads
    public static void LoadByPrimaryKey(DataSet ADataSet, {#FORMALPARAMETERSPRIMARYKEY}, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
    {
        {#ODBCPARAMETERSPRIMARYKEY}
        ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                        {#CSVLISTPRIMARYKEYFIELDS}}) + " FROM PUB_{#SQLTABLENAME} WHERE {#WHERECLAUSEPRIMARYKEY}") 
                        + GenerateOrderByClause(AOrderBy)), {#TABLENAME}Table.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
    }

    /// auto generated
    public static void LoadByPrimaryKey(DataSet AData, {#FORMALPARAMETERSPRIMARYKEY}, TDBTransaction ATransaction)
    {
        LoadByPrimaryKey(AData, {#ACTUALPARAMETERSPRIMARYKEY}, null, ATransaction, null, 0, 0);
    }
    
    /// auto generated
    public static void LoadByPrimaryKey(DataSet AData, {#FORMALPARAMETERSPRIMARYKEY}, StringCollection AFieldList, TDBTransaction ATransaction)
    {
        LoadByPrimaryKey(AData, {#ACTUALPARAMETERSPRIMARYKEY}, AFieldList, ATransaction, null, 0, 0);
    }

    /// auto generated
    public static void LoadByPrimaryKey(out {#TABLENAME}Table AData, {#FORMALPARAMETERSPRIMARYKEY}, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
    {
        DataSet FillDataSet = new DataSet();
        AData = new {#TABLENAME}Table();
        FillDataSet.Tables.Add(AData);
        LoadByPrimaryKey(FillDataSet, {#ACTUALPARAMETERSPRIMARYKEY}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        FillDataSet.Tables.Remove(AData);
    }
    
    /// auto generated
    public static void LoadByPrimaryKey(out {#TABLENAME}Table AData, {#FORMALPARAMETERSPRIMARYKEY}, TDBTransaction ATransaction)
    {
        LoadByPrimaryKey(out AData, {#ACTUALPARAMETERSPRIMARYKEY}, null, ATransaction, null, 0, 0);
    }
    
    /// auto generated
    public static void LoadByPrimaryKey(out {#TABLENAME}Table AData, {#FORMALPARAMETERSPRIMARYKEY}, StringCollection AFieldList, TDBTransaction ATransaction)
    {
        LoadByPrimaryKey(out AData, {#ACTUALPARAMETERSPRIMARYKEY}, AFieldList, ATransaction, null, 0, 0);
    }
{#ENDIF FORMALPARAMETERSPRIMARYKEY}

    /// this method is called by all overloads
    public static void LoadUsingTemplate(DataSet ADataSet, {#TABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
    {
        ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClause(AFieldList, new string[] {
                        {#CSVLISTPRIMARYKEYFIELDS}}) + " FROM PUB_{#SQLTABLENAME}") 
                        + GenerateWhereClause({#TABLENAME}Table.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                        + GenerateOrderByClause(AOrderBy)), {#TABLENAME}Table.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
    }
    
    /// auto generated
    public static void LoadUsingTemplate(DataSet AData, {#TABLENAME}Row ATemplateRow, TDBTransaction ATransaction)
    {
        LoadUsingTemplate(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
    }
    
    /// auto generated
    public static void LoadUsingTemplate(DataSet AData, {#TABLENAME}Row ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
    {
        LoadUsingTemplate(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
    }
    
    /// auto generated
    public static void LoadUsingTemplate(out {#TABLENAME}Table AData, {#TABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
    {
        DataSet FillDataSet = new DataSet();
        AData = new {#TABLENAME}Table();
        FillDataSet.Tables.Add(AData);
        LoadUsingTemplate(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
        FillDataSet.Tables.Remove(AData);
    }
    
    /// auto generated
    public static void LoadUsingTemplate(out {#TABLENAME}Table AData, {#TABLENAME}Row ATemplateRow, TDBTransaction ATransaction)
    {
        LoadUsingTemplate(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
    }
    
    /// auto generated
    public static void LoadUsingTemplate(out {#TABLENAME}Table AData, {#TABLENAME}Row ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
    {
        LoadUsingTemplate(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
    }
    
    /// auto generated
    public static void LoadUsingTemplate(out {#TABLENAME}Table AData, {#TABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
    {
        LoadUsingTemplate(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
    }

    /// this method is called by all overloads
    public static int CountAll(TDBTransaction ATransaction)
    {
        return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_{#SQLTABLENAME}", ATransaction, false));
    }
{#IFDEF FORMALPARAMETERSPRIMARYKEY}

    /// this method is called by all overloads
    public static int CountByPrimaryKey({#FORMALPARAMETERSPRIMARYKEY}, TDBTransaction ATransaction)
    {
        {#ODBCPARAMETERSPRIMARYKEY}
        return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_{#SQLTABLENAME} WHERE {#WHERECLAUSEPRIMARYKEY}", ATransaction, false, ParametersArray));
    }
{#ENDIF FORMALPARAMETERSPRIMARYKEY}
    
    /// this method is called by all overloads
    public static int CountUsingTemplate({#TABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
    {
        return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_{#SQLTABLENAME}" + GenerateWhereClause({#TABLENAME}Table.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow)));
    }
    {#VIAOTHERTABLE}
    {#VIALINKTABLE}

{#IFDEF FORMALPARAMETERSPRIMARYKEY}

    /// auto generated
    public static void DeleteByPrimaryKey({#FORMALPARAMETERSPRIMARYKEY}, TDBTransaction ATransaction)
    {
        {#ODBCPARAMETERSPRIMARYKEY}
        DBAccess.GDBAccessObj.ExecuteNonQuery("DELETE FROM PUB_{#SQLTABLENAME} WHERE {#WHERECLAUSEPRIMARYKEY}", ATransaction, false, ParametersArray);
    }
{#ENDIF FORMALPARAMETERSPRIMARYKEY}
    
    /// auto generated
    public static void DeleteUsingTemplate({#TABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
    {
        DBAccess.GDBAccessObj.ExecuteNonQuery(("DELETE FROM PUB_{#SQLTABLENAME}" + GenerateWhereClause({#TABLENAME}Table.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow));
    }
    
    /// auto generated
    public static bool SubmitChanges({#TABLENAME}Table ATable, TDBTransaction ATransaction, out TVerificationResultCollection AVerificationResult)
    {
        bool ResultValue = true;
        bool ExceptionReported = false;
        DataRow TheRow = null;
        AVerificationResult = new TVerificationResultCollection();
        for (RowCount = 0; (RowCount != ATable.Rows.Count); RowCount = (RowCount + 1))
        {
            TheRow = ATable[RowCount];
            try
            {
                if ((TheRow.RowState == DataRowState.Added))
                {
{#IFDEF SEQUENCENAME}
                    (({#TABLENAME}Row)(TheRow)).{#SEQUENCEFIELD} = ({#SEQUENCECAST}(DBAccess.GDBAccessObj.GetNextSequenceValue("{#SEQUENCENAME}", ATransaction)));
{#ENDIF SEQUENCENAME}
                    TTypedDataAccess.InsertRow("{#SQLTABLENAME}", {#TABLENAME}Table.GetColumnStringList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
                }
                if ((TheRow.RowState == DataRowState.Modified))
                {
{#IFDEF FORMALPARAMETERSPRIMARYKEY}
                    TTypedDataAccess.UpdateRow("{#SQLTABLENAME}", {#TABLENAME}Table.GetColumnStringList(), {#TABLENAME}Table.GetPrimKeyColumnOrdList(), ref TheRow, ATransaction, UserInfo.GUserInfo.UserID);
{#ENDIF FORMALPARAMETERSPRIMARYKEY}
{#IFNDEF FORMALPARAMETERSPRIMARYKEY}
                    AVerificationResult.Add(new TVerificationResult("[DB] NO PRIMARY KEY", "Cannot update record because table {#TABLENAME} has no primary key." +
                                "", "Primary Key missing", "{#SQLTABLENAME}", TResultSeverity.Resv_Critical));
{#ENDIFN FORMALPARAMETERSPRIMARYKEY}
                }
                if ((TheRow.RowState == DataRowState.Deleted))
                {
{#IFDEF FORMALPARAMETERSPRIMARYKEY}
                    TTypedDataAccess.DeleteRow("{#SQLTABLENAME}", {#TABLENAME}Table.GetColumnStringList(), {#TABLENAME}Table.GetPrimKeyColumnOrdList(), TheRow, ATransaction);
{#ENDIF FORMALPARAMETERSPRIMARYKEY}
{#IFNDEF FORMALPARAMETERSPRIMARYKEY}
                    AVerificationResult.Add(new TVerificationResult("[DB] NO PRIMARY KEY", "Cannot delete record because table {#TABLENAME} has no primary key." +
                                "", "Primary Key missing", "{#SQLTABLENAME}", TResultSeverity.Resv_Critical));
{#ENDIFN FORMALPARAMETERSPRIMARYKEY}
                }
            }
            catch (OdbcException ex)
            {
                ResultValue = false;
                ExceptionReported = false;
                if ((ExceptionReported == false))
                {
                    AVerificationResult.Add(new TVerificationResult("[ODBC]", ex.Errors[0].Message, "ODBC error for table {#TABLENAME}", ex.Errors[0].NativeError.ToString(), TResultSeverity.Resv_Critical));
                }
            }
        }
        return ResultValue;
    }
}

{##VIAOTHERTABLE}

/// auto generated
public static void Load{#VIAPROCEDURENAME}(DataSet ADataSet, {#FORMALPARAMETERSOTHERPRIMARYKEY}, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
{
    {#ODBCPARAMETERSFOREIGNKEY}
    ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClause(AFieldList, new string[] {
                    {#CSVLISTPRIMARYKEYFIELDS}}) + " FROM PUB_{#SQLTABLENAME} WHERE {#WHERECLAUSEFOREIGNKEY}") 
                    + GenerateOrderByClause(AOrderBy)), {#TABLENAME}Table.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}(DataSet AData, {#FORMALPARAMETERSOTHERPRIMARYKEY}, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}(AData, {#ACTUALPARAMETERSOTHERPRIMARYKEY}, null, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}(DataSet AData, {#FORMALPARAMETERSOTHERPRIMARYKEY}, StringCollection AFieldList, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}(AData, {#ACTUALPARAMETERSOTHERPRIMARYKEY}, AFieldList, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}(out {#TABLENAME}Table AData, {#FORMALPARAMETERSOTHERPRIMARYKEY}, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
{
    DataSet FillDataSet = new DataSet();
    AData = new {#TABLENAME}Table();
    FillDataSet.Tables.Add(AData);
    Load{#VIAPROCEDURENAME}(FillDataSet, {#ACTUALPARAMETERSOTHERPRIMARYKEY}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
    FillDataSet.Tables.Remove(AData);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}(out {#TABLENAME}Table AData, {#FORMALPARAMETERSOTHERPRIMARYKEY}, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}(out AData, {#ACTUALPARAMETERSOTHERPRIMARYKEY}, null, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}(out {#TABLENAME}Table AData, {#FORMALPARAMETERSOTHERPRIMARYKEY}, StringCollection AFieldList, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}(out AData, {#ACTUALPARAMETERSOTHERPRIMARYKEY}, AFieldList, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(DataSet ADataSet, {#OTHERTABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
{
    ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_{#SQLTABLENAME}", AFieldList, new string[] {
                    {#CSVLISTPRIMARYKEYFIELDS}}) + " FROM PUB_{#SQLTABLENAME}, PUB_{#SQLOTHERTABLENAME} WHERE " +
                    "{#WHERECLAUSEVIAOTHERTABLE}") 
                    + GenerateWhereClauseLong("PUB_{#SQLOTHERTABLENAME}", {#OTHERTABLENAME}Table.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                    + GenerateOrderByClause(AOrderBy)), {#TABLENAME}Table.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(DataSet AData, {#OTHERTABLENAME}Row ATemplateRow, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}Template(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(DataSet AData, {#OTHERTABLENAME}Row ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}Template(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(out {#TABLENAME}Table AData, {#OTHERTABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
{
    DataSet FillDataSet = new DataSet();
    AData = new {#TABLENAME}Table();
    FillDataSet.Tables.Add(AData);
    Load{#VIAPROCEDURENAME}Template(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
    FillDataSet.Tables.Remove(AData);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(out {#TABLENAME}Table AData, {#OTHERTABLENAME}Row ATemplateRow, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}Template(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(out {#TABLENAME}Table AData, {#OTHERTABLENAME}Row ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}Template(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(out {#TABLENAME}Table AData, {#OTHERTABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}Template(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
}

/// auto generated
public static int Count{#VIAPROCEDURENAME}({#FORMALPARAMETERSOTHERPRIMARYKEY}, TDBTransaction ATransaction)
{
    {#ODBCPARAMETERSFOREIGNKEY}
    return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_{#SQLTABLENAME} WHERE {#WHERECLAUSEFOREIGNKEY}", ATransaction, false, ParametersArray));
}

/// auto generated
public static int Count{#VIAPROCEDURENAME}Template({#OTHERTABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
{
    return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_{#SQLTABLENAME}, PUB_{#SQLOTHERTABLENAME} WHERE " +
        "{#WHERECLAUSEVIAOTHERTABLE}" + GenerateWhereClauseLong("PUB_{#SQLOTHERTABLENAME}", {#OTHERTABLENAME}Table.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, {#OTHERTABLENAME}Table.GetPrimKeyColumnOrdList())));
}

{##VIALINKTABLE}

/// auto generated LoadViaLinkTable
public static void Load{#VIAPROCEDURENAME}(DataSet ADataSet, {#FORMALPARAMETERSOTHERPRIMARYKEY}, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
{
    {#ODBCPARAMETERSFOREIGNKEY}
    ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, ((GenerateSelectClauseLong("PUB_{#SQLTABLENAME}", AFieldList, new string[] {
                    {#CSVLISTPRIMARYKEYFIELDS}}) + " FROM PUB_{#SQLTABLENAME}, PUB_{#SQLLINKTABLENAME} WHERE " + 
                    "{#WHERECLAUSEVIALINKTABLE}") 
                    + GenerateOrderByClause(AOrderBy)), {#TABLENAME}Table.GetTableName(), ATransaction, ParametersArray, AStartRecord, AMaxRecords);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}(DataSet AData, {#FORMALPARAMETERSOTHERPRIMARYKEY}, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}(AData, {#ACTUALPARAMETERSOTHERPRIMARYKEY}, null, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}(DataSet AData, {#FORMALPARAMETERSOTHERPRIMARYKEY}, StringCollection AFieldList, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}(AData, {#ACTUALPARAMETERSOTHERPRIMARYKEY}, AFieldList, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}(out {#TABLENAME}Table AData, {#FORMALPARAMETERSOTHERPRIMARYKEY}, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
{
    DataSet FillDataSet = new DataSet();
    AData = new {#TABLENAME}Table();
    FillDataSet.Tables.Add(AData);
    Load{#VIAPROCEDURENAME}(FillDataSet, {#ACTUALPARAMETERSOTHERPRIMARYKEY}, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
    FillDataSet.Tables.Remove(AData);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}(out {#TABLENAME}Table AData, {#FORMALPARAMETERSOTHERPRIMARYKEY}, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}(out AData, {#ACTUALPARAMETERSOTHERPRIMARYKEY}, null, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}(out {#TABLENAME}Table AData, {#FORMALPARAMETERSOTHERPRIMARYKEY}, StringCollection AFieldList, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}(out AData, {#ACTUALPARAMETERSOTHERPRIMARYKEY}, AFieldList, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(DataSet ADataSet, {#OTHERTABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
{
    ADataSet = DBAccess.GDBAccessObj.Select(ADataSet, (((GenerateSelectClauseLong("PUB_{#SQLTABLENAME}", AFieldList, new string[] {
                    {#CSVLISTPRIMARYKEYFIELDS}}) + " FROM PUB_{#SQLTABLENAME}, PUB_{#SQLLINKTABLENAME}, PUB_{#SQLOTHERTABLENAME} WHERE " +
                    "{#WHERECLAUSEALLVIATABLES}") 
                    + GenerateWhereClauseLong("PUB_{#SQLOTHERTABLENAME}", {#OTHERTABLENAME}Table.GetColumnStringList(), ATemplateRow, ATemplateOperators)) 
                    + GenerateOrderByClause(AOrderBy)), {#TABLENAME}Table.GetTableName(), ATransaction, GetParametersForWhereClause(ATemplateRow), AStartRecord, AMaxRecords);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(DataSet AData, {#OTHERTABLENAME}Row ATemplateRow, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}Template(AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(DataSet AData, {#OTHERTABLENAME}Row ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}Template(AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(out {#TABLENAME}Table AData, {#OTHERTABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction, StringCollection AOrderBy, int AStartRecord, int AMaxRecords)
{
    DataSet FillDataSet = new DataSet();
    AData = new {#TABLENAME}Table();
    FillDataSet.Tables.Add(AData);
    Load{#VIAPROCEDURENAME}Template(FillDataSet, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, AOrderBy, AStartRecord, AMaxRecords);
    FillDataSet.Tables.Remove(AData);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(out {#TABLENAME}Table AData, {#OTHERTABLENAME}Row ATemplateRow, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}Template(out AData, ATemplateRow, null, null, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(out {#TABLENAME}Table AData, {#OTHERTABLENAME}Row ATemplateRow, StringCollection AFieldList, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}Template(out AData, ATemplateRow, null, AFieldList, ATransaction, null, 0, 0);
}

/// auto generated
public static void Load{#VIAPROCEDURENAME}Template(out {#TABLENAME}Table AData, {#OTHERTABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, StringCollection AFieldList, TDBTransaction ATransaction)
{
    Load{#VIAPROCEDURENAME}Template(out AData, ATemplateRow, ATemplateOperators, AFieldList, ATransaction, null, 0, 0);
}

/// auto generated CountViaLinkTable
public static int Count{#VIAPROCEDURENAME}({#FORMALPARAMETERSOTHERPRIMARYKEY}, TDBTransaction ATransaction)
{
    {#ODBCPARAMETERSFOREIGNKEY}
    return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar("SELECT COUNT(*) FROM PUB_{#SQLTABLENAME}, PUB_{#SQLLINKTABLENAME} WHERE " +
                "{#WHERECLAUSEVIALINKTABLE}",
                ATransaction, false, ParametersArray));
}

/// auto generated
public static int Count{#VIAPROCEDURENAME}Template({#OTHERTABLENAME}Row ATemplateRow, StringCollection ATemplateOperators, TDBTransaction ATransaction)
{
    return Convert.ToInt32(DBAccess.GDBAccessObj.ExecuteScalar(("SELECT COUNT(*) FROM PUB_{#SQLTABLENAME}, PUB_{#SQLLINKTABLENAME}, PUB_{#SQLOTHERTABLENAME} WHERE " +
                "{#WHERECLAUSEALLVIATABLES}" +
                GenerateWhereClauseLong("PUB_{#SQLLINKTABLENAME}", {#TABLENAME}Table.GetColumnStringList(), ATemplateRow, ATemplateOperators)), ATransaction, false, GetParametersForWhereClause(ATemplateRow, {#OTHERTABLENAME}Table.GetPrimKeyColumnOrdList())));
}