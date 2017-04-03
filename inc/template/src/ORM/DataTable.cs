// auto generated with nant generateORM
// Do not modify this file manually!
//
{#GPLFILEHEADER}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Runtime.Serialization;
using System.Xml;
using Ict.Common;
using Ict.Common.Data;

namespace {#NAMESPACE}
{
    {#TABLELOOP}
}

{##TYPEDTABLE}

{#TABLE_DESCRIPTION}
[Serializable()]
public class {#TABLENAME}Table : {#BASECLASSTABLE}
{
    private static String strCustomReportPermission = "{#CUSTOMREPORTPERMISSION}";
    private static List<String> listCustomReportField = new List<String>{{#INITVARSCUSTOMREPORTFIELDLIST}};
    
    /// TableId for Ict.Common.Data generic functions
    public {#NEW}static short TableId = {#TABLEID};
    {#COLUMNIDS}

{#IFDEF COLUMNINFO}
    private static bool FInitInfoValues = InitInfoValues();
    private static bool InitInfoValues()
    {
        TableInfo.Add(TableId, new TTypedTableInfo(TableId, "{#TABLEVARIABLENAME}", "{#DBTABLENAME}", 
            new TTypedColumnInfo[] { 
                {#COLUMNINFO}
            },
            new int[] { 
                {#COLUMNPRIMARYKEYORDER}
{#IFDEF COLUMNUNIQUEKEYORDER}
            }, new int[] {
                {#COLUMNUNIQUEKEYORDER}
{#ENDIF COLUMNUNIQUEKEYORDER}
            }));

        // try to avoid a compiler warning about unused variable FInitInfoValues which we need for initially calling InitInfoValues once
        FInitInfoValues = true;
        return FInitInfoValues;
    }
{#ENDIF COLUMNINFO}

    /// constructor
    public {#TABLENAME}Table() : 
            base("{#TABLEVARIABLENAME}")
    {
    }
    
    /// constructor
    public {#TABLENAME}Table(string ATablename) : 
            base(ATablename)
    {
    }
    
    /// constructor for serialization
    public {#TABLENAME}Table(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
            base(info, context)
    {
    }

    {#DATACOLUMNS}

    /// create the columns
    protected override void InitClass()
    {
        {#INITCLASSADDCOLUMN}
    }
    
    /// assign columns to properties, set primary key
    public override void InitVars()
    {
        {#INITVARSCOLUMN}
{#IFDEF PRIMARYKEYCOLUMNS}
        this.PrimaryKey = new System.Data.DataColumn[{#PRIMARYKEYCOLUMNSCOUNT}] {
                {#PRIMARYKEYCOLUMNS}};
{#ENDIF PRIMARYKEYCOLUMNS}
    }

    /// Access a typed row by index
    public {#NEW}{#TABLENAME}Row this[int i]
    {
        get
        {
            return (({#TABLENAME}Row)(this.Rows[i]));
        }
    }

    /// create a new typed row
    public {#NEW}{#TABLENAME}Row NewRowTyped(bool AWithDefaultValues)
    {
        {#TABLENAME}Row ret = (({#TABLENAME}Row)(this.NewRow()));
        if ((AWithDefaultValues == true))
        {
            ret.InitValues();
        }
        return ret;
    }
    
    /// create a new typed row, always with default values
    public {#NEW}{#TABLENAME}Row NewRowTyped()
    {
        return this.NewRowTyped(true);
    }
    
    /// new typed row using DataRowBuilder
    protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
    {
        return new {#TABLENAME}Row(builder);
    }
    
    /// get typed set of changes
    public {#NEW}{#TABLENAME}Table GetChangesTyped()
    {
        return (({#TABLENAME}Table)(base.GetChangesTypedInternal()));
    }

    /// return the CamelCase name of the table
    public static {#NEW}string GetTableName()
    {
        return "{#TABLEVARIABLENAME}";
    }

    /// return the name of the table as it is used in the database
    public static {#NEW}string GetTableDBName()
    {
        return "{#DBTABLENAME}";
    }

    /// static method to return the 'Label' of the table as it is used in the database (the 'Label' is usually a short description of what the db table is about)
    public static {#NEW}string GetTableDBLabel()
    {
        return Catalog.GetString("{#DBTABLELABEL}");
    }

    /// instance property to get the 'Label' of the table as it is used in the database (the 'Label' is usually a short description of what the db table is about)
    public override string TableDBLabel
    {
        get
        {
            return {#TABLENAME}Table.GetTableDBLabel();
        }
    }
    
    /// get an odbc parameter for the given column
    public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
    {
        return CreateOdbcParameter(TableId, AColumnNr);
    }

    /// string to indicate which permissions a user needs to access table for custom reports
    /// (e.g. "PTNRUSER", "OR(FINANCE-1,DEVUSER)", "AND(PTNRUSER,FINANCE-1)"
    /// This should be returned by method in derived class
    public static string {#TABLEINTDS}CustomReportPermission()
    {
        return strCustomReportPermission;
    }

    /// string to indicate which permissions a user needs to access table for custom reports
    /// (e.g. "PTNRUSER", "OR(FINANCE-1,DEVUSER)", "AND(PTNRUSER,FINANCE-1)"
    /// This should be returned by method in derived class
    public override string GetCustomReportPermission()
    {
        return strCustomReportPermission;
    }
    
    /// Is this table generally available in custom reports?
    public static bool {#TABLEINTDS}AvailableForCustomReport()
    {
        return {#AVAILABLEFORCUSTOMREPORT};
    }

    /// Is this table generally available in custom reports?
    public override bool IsAvailableForCustomReport()
    {
        return {#AVAILABLEFORCUSTOMREPORT};
    }
    
    /// Return a list of fields that are available for custom reports
    public static List<String> {#TABLEINTDS}CustomReportFieldList()
    {
        return listCustomReportField;
    }

    /// Return a list of fields that are available for custom reports
    public override List<String> GetCustomReportFieldList()
    {
        return listCustomReportField;
    }
    
    {#STATICCOLUMNPROPERTIES}

}

{##COLUMNIDS}
/// used for generic TTypedDataTable functions
public static {#NEW}short Column{#COLUMNNAME}Id = {#COLUMNORDERNUMBER};

{##DATACOLUMN}
{#COLUMN_DESCRIPTION}
public DataColumn Column{#COLUMNNAME};

{##COLUMNINFO}
new TTypedColumnInfo({#COLUMNORDERNUMBER}, "{#COLUMNNAME}", "{#COLUMNDBNAME}", "{#COLUMNLABEL}", {#COLUMNODBCTYPE}, {#COLUMNLENGTH}, {#COLUMNNOTNULL}){#COLUMNCOMMA}

{##INITCLASSADDCOLUMN}
this.Columns.Add(new System.Data.DataColumn("{#COLUMNDBNAME}", typeof({#COLUMNDOTNETTYPENOTNULLABLE})));

{##INITVARSCOLUMN}
this.Column{#COLUMNNAME} = this.Columns["{#COLUMNDBNAME}"];

{##INITVARSCUSTOMREPORTFIELDLIST}
{#LISTDELIMITER}"{#COLUMNDBNAME}"

{##INITVARSCUSTOMREPORTFIELDLISTEMPTY}
{#EMPTY}
    
{##STATICCOLUMNPROPERTIES}

/// get the name of the field in the database for this column
public static {#NEW}string Get{#COLUMNNAME}DBName()
{
    return "{#COLUMNDBNAME}";
}

/// get character length for column
public static {#NEW}short Get{#COLUMNNAME}Length()
{
    return {#COLUMNLENGTH};
}

/// get the help text for the field in the database for this column
public static string Get{#COLUMNNAME}Help()
{
    return "{#COLUMNHELP}";
}

{##TYPEDROW}

{#TABLE_DESCRIPTION}
[Serializable()]
public class {#TABLENAME}Row : {#BASECLASSROW}
{
    private {#TABLENAME}Table myTable;
    
    /// Constructor
    public {#TABLENAME}Row(System.Data.DataRowBuilder rb) : 
            base(rb)
    {
        this.myTable = (({#TABLENAME}Table)(this.Table));
    }

    {#ROWCOLUMNPROPERTIES}

    /// set default values
    public {#OVERRIDE}void InitValues()
    {
        {#ROWSETNULLORDEFAULT}
    }

    {#FUNCTIONSFORNULLVALUES}
}

{##ROWCOLUMNPROPERTY}

{#COLUMN_DESCRIPTION}
public {#COLUMNDOTNETTYPE} {#COLUMNNAME}
{
    get
    {
        object ret;
        ret = this[this.myTable.Column{#COLUMNNAME}.Ordinal];
        if ((ret == System.DBNull.Value))
        {
            {#ACTIONGETNULLVALUE}
        }
        else
        {
            return (({#COLUMNDOTNETTYPE})(ret));
        }
    }
    set
    {
{#IFDEF TESTFORNULL}
        if ({#TESTFORNULL})
        {
            if (!this.IsNull(this.myTable.Column{#COLUMNNAME}))
            {
                Set{#COLUMNNAME}Null();
            }
        }
        else if ((this.IsNull(this.myTable.Column{#COLUMNNAME})
{#ENDIF TESTFORNULL}
{#IFNDEF TESTFORNULL}
        if ((this.IsNull(this.myTable.Column{#COLUMNNAME})
{#ENDIFN TESTFORNULL}
                    || ((({#COLUMNDOTNETTYPE})(this[this.myTable.Column{#COLUMNNAME}])) != value)))
        {
            this[this.myTable.Column{#COLUMNNAME}] = value;
        }
    }
}

{##FUNCTIONSFORNULLVALUES}

/// test for NULL value
public bool Is{#COLUMNNAME}Null()
{
    return this.IsNull(this.myTable.Column{#COLUMNNAME});
}

/// assign NULL value
public void Set{#COLUMNNAME}Null()
{
    this.SetNull(this.myTable.Column{#COLUMNNAME});
}