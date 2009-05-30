/* Auto generated with nant generateORM
 * based on CommonTypedDataSets.xml
 * Do not modify this file manually!
 */
namespace Ict.Petra.Shared.MCommon.Data
{
    using Ict.Common;
    using Ict.Common.Data;
    using System;
    using System.Data;
    using System.Data.Odbc;
    using Ict.Petra.Shared.MPartner.Partner.Data;
    using Ict.Petra.Shared.MPersonnel.Personnel.Data;
    
    
    /// auto generated
    [Serializable()]
    public class OfficeSpecificDataLabelsTDS : TTypedDataSet
    {
        
        private PDataLabelTable TableDataLabelList;
        
        private PDataLabelUseTable TableDataLabelUseList;
        
        private PDataLabelLookupTable TableDataLabelLookupList;
        
        private PDataLabelLookupCategoryTable TableDataLabelLookupCategoryList;
        
        private PDataLabelValueApplicationTable TablePDataLabelValueApplication;
        
        private PDataLabelValuePartnerTable TablePDataLabelValuePartner;
        
        /// auto generated
        public OfficeSpecificDataLabelsTDS() : 
                base("OfficeSpecificDataLabelsTDS")
        {
        }
        
        /// auto generated for serialization
        public OfficeSpecificDataLabelsTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// auto generated
        public OfficeSpecificDataLabelsTDS(string ADatasetName) : 
                base(ADatasetName)
        {
        }
        
        /// auto generated
        public PDataLabelTable DataLabelList
        {
            get
            {
                return this.TableDataLabelList;
            }
        }
        
        /// auto generated
        public PDataLabelUseTable DataLabelUseList
        {
            get
            {
                return this.TableDataLabelUseList;
            }
        }
        
        /// auto generated
        public PDataLabelLookupTable DataLabelLookupList
        {
            get
            {
                return this.TableDataLabelLookupList;
            }
        }
        
        /// auto generated
        public PDataLabelLookupCategoryTable DataLabelLookupCategoryList
        {
            get
            {
                return this.TableDataLabelLookupCategoryList;
            }
        }
        
        /// auto generated
        public PDataLabelValueApplicationTable PDataLabelValueApplication
        {
            get
            {
                return this.TablePDataLabelValueApplication;
            }
        }
        
        /// auto generated
        public PDataLabelValuePartnerTable PDataLabelValuePartner
        {
            get
            {
                return this.TablePDataLabelValuePartner;
            }
        }
        
        /// auto generated
        public new virtual OfficeSpecificDataLabelsTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((OfficeSpecificDataLabelsTDS)(base.GetChangesTyped(removeEmptyTables)));
        }
        
        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new PDataLabelTable("DataLabelList"));
            this.Tables.Add(new PDataLabelUseTable("DataLabelUseList"));
            this.Tables.Add(new PDataLabelLookupTable("DataLabelLookupList"));
            this.Tables.Add(new PDataLabelLookupCategoryTable("DataLabelLookupCategoryList"));
            this.Tables.Add(new PDataLabelValueApplicationTable("PDataLabelValueApplication"));
            this.Tables.Add(new PDataLabelValuePartnerTable("PDataLabelValuePartner"));
        }
        
        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("DataLabelList") != -1))
            {
                this.Tables.Add(new PDataLabelTable("DataLabelList"));
            }
            if ((ds.Tables.IndexOf("DataLabelUseList") != -1))
            {
                this.Tables.Add(new PDataLabelUseTable("DataLabelUseList"));
            }
            if ((ds.Tables.IndexOf("DataLabelLookupList") != -1))
            {
                this.Tables.Add(new PDataLabelLookupTable("DataLabelLookupList"));
            }
            if ((ds.Tables.IndexOf("DataLabelLookupCategoryList") != -1))
            {
                this.Tables.Add(new PDataLabelLookupCategoryTable("DataLabelLookupCategoryList"));
            }
            if ((ds.Tables.IndexOf("PDataLabelValueApplication") != -1))
            {
                this.Tables.Add(new PDataLabelValueApplicationTable("PDataLabelValueApplication"));
            }
            if ((ds.Tables.IndexOf("PDataLabelValuePartner") != -1))
            {
                this.Tables.Add(new PDataLabelValuePartnerTable("PDataLabelValuePartner"));
            }
        }
        
        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TableDataLabelList != null))
            {
                this.TableDataLabelList.InitVars();
            }
            if ((this.TableDataLabelUseList != null))
            {
                this.TableDataLabelUseList.InitVars();
            }
            if ((this.TableDataLabelLookupList != null))
            {
                this.TableDataLabelLookupList.InitVars();
            }
            if ((this.TableDataLabelLookupCategoryList != null))
            {
                this.TableDataLabelLookupCategoryList.InitVars();
            }
            if ((this.TablePDataLabelValueApplication != null))
            {
                this.TablePDataLabelValueApplication.InitVars();
            }
            if ((this.TablePDataLabelValuePartner != null))
            {
                this.TablePDataLabelValuePartner.InitVars();
            }
        }
        
        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "OfficeSpecificDataLabelsTDS";
            this.TableDataLabelList = ((PDataLabelTable)(this.Tables["DataLabelList"]));
            this.TableDataLabelUseList = ((PDataLabelUseTable)(this.Tables["DataLabelUseList"]));
            this.TableDataLabelLookupList = ((PDataLabelLookupTable)(this.Tables["DataLabelLookupList"]));
            this.TableDataLabelLookupCategoryList = ((PDataLabelLookupCategoryTable)(this.Tables["DataLabelLookupCategoryList"]));
            this.TablePDataLabelValueApplication = ((PDataLabelValueApplicationTable)(this.Tables["PDataLabelValueApplication"]));
            this.TablePDataLabelValuePartner = ((PDataLabelValuePartnerTable)(this.Tables["PDataLabelValuePartner"]));
        }
        
        /// auto generated
        protected override void InitConstraints()
        {
            if (((this.TableDataLabelLookupCategoryList != null) 
                        && (this.TableDataLabelList != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDataLabel2", "DataLabelLookupCategoryList", new string[] {
                                "p_category_code_c"}, "DataLabelList", new string[] {
                                "p_lookup_category_code_c"}));
            }
            if (((this.TableDataLabelList != null) 
                        && (this.TableDataLabelUseList != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDataLabelUse1", "DataLabelList", new string[] {
                                "p_key_i"}, "DataLabelUseList", new string[] {
                                "p_data_label_key_i"}));
            }
            if (((this.TableDataLabelLookupCategoryList != null) 
                        && (this.TableDataLabelLookupList != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDataLabelLookup1", "DataLabelLookupCategoryList", new string[] {
                                "p_category_code_c"}, "DataLabelLookupList", new string[] {
                                "p_category_code_c"}));
            }
            if (((this.TableDataLabelList != null) 
                        && (this.TablePDataLabelValueApplication != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDataLabelValueApplication2", "DataLabelList", new string[] {
                                "p_key_i"}, "PDataLabelValueApplication", new string[] {
                                "p_data_label_key_i"}));
            }
            if (((this.TableDataLabelList != null) 
                        && (this.TablePDataLabelValuePartner != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDataLabelValuePartner2", "DataLabelList", new string[] {
                                "p_key_i"}, "PDataLabelValuePartner", new string[] {
                                "p_data_label_key_i"}));
            }
            this.FRelations.Add(new TTypedRelation("LabelUse", "DataLabelList", new string[] {
                            "p_key_i"}, "DataLabelUseList", new string[] {
                            "p_data_label_key_i"}, false));
        }
    }
    
    /// auto generated custom table
    [Serializable()]
    public class CacheableTablesTDSContentsTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnTableName;
        
        /// 
        public DataColumn ColumnDataUpToDate;
        
        /// 
        public DataColumn ColumnDataChanged;
        
        /// 
        public DataColumn ColumnChangesSavedExternally;
        
        /// 
        public DataColumn ColumnCachedSince;
        
        /// 
        public DataColumn ColumnLastAccessed;
        
        /// 
        public DataColumn ColumnHashCode;
        
        /// 
        public DataColumn ColumnTableSize;
        
        /// constructor
        public CacheableTablesTDSContentsTable() : 
                base("Contents")
        {
        }
        
        /// constructor
        public CacheableTablesTDSContentsTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public CacheableTablesTDSContentsTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public CacheableTablesTDSContentsRow this[int i]
        {
            get
            {
                return ((CacheableTablesTDSContentsRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetTableNameDBName()
        {
            return "TableName";
        }
        
        /// get help text for column
        public static string GetTableNameHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetTableNameLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDataUpToDateDBName()
        {
            return "DataUpToDate";
        }
        
        /// get help text for column
        public static string GetDataUpToDateHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetDataUpToDateLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDataChangedDBName()
        {
            return "DataChanged";
        }
        
        /// get help text for column
        public static string GetDataChangedHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetDataChangedLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetChangesSavedExternallyDBName()
        {
            return "ChangesSavedExternally";
        }
        
        /// get help text for column
        public static string GetChangesSavedExternallyHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetChangesSavedExternallyLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetCachedSinceDBName()
        {
            return "CachedSince";
        }
        
        /// get help text for column
        public static string GetCachedSinceHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetCachedSinceLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetLastAccessedDBName()
        {
            return "LastAccessed";
        }
        
        /// get help text for column
        public static string GetLastAccessedHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetLastAccessedLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetHashCodeDBName()
        {
            return "HashCode";
        }
        
        /// get help text for column
        public static string GetHashCodeHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetHashCodeLabel()
        {
            return "";
        }
        
        /// get the name of the field in the database for this column
        public static string GetTableSizeDBName()
        {
            return "TableSize";
        }
        
        /// get help text for column
        public static string GetTableSizeHelp()
        {
            return null;
        }
        
        /// get label of column
        public static string GetTableSizeLabel()
        {
            return "";
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "Contents";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "Contents";
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnTableName = this.Columns["TableName"];
            this.ColumnDataUpToDate = this.Columns["DataUpToDate"];
            this.ColumnDataChanged = this.Columns["DataChanged"];
            this.ColumnChangesSavedExternally = this.Columns["ChangesSavedExternally"];
            this.ColumnCachedSince = this.Columns["CachedSince"];
            this.ColumnLastAccessed = this.Columns["LastAccessed"];
            this.ColumnHashCode = this.Columns["HashCode"];
            this.ColumnTableSize = this.Columns["TableSize"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnTableName};
        }
        
        /// create a new typed row
        public CacheableTablesTDSContentsRow NewRowTyped(bool AWithDefaultValues)
        {
            CacheableTablesTDSContentsRow ret = ((CacheableTablesTDSContentsRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new CacheableTablesTDSContentsRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("TableName", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("DataUpToDate", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("DataChanged", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("ChangesSavedExternally", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("CachedSince", typeof(DateTime)));
            this.Columns.Add(new System.Data.DataColumn("LastAccessed", typeof(DateTime)));
            this.Columns.Add(new System.Data.DataColumn("HashCode", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("TableSize", typeof(Int32)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnTableName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnDataUpToDate))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnDataChanged))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnChangesSavedExternally))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnCachedSince))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnLastAccessed))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnHashCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnTableSize))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            return null;
        }
    }
    
    /// CustomRow auto generated
    [Serializable()]
    public class CacheableTablesTDSContentsRow : System.Data.DataRow
    {
        
        private CacheableTablesTDSContentsTable myTable;
        
        /// Constructor
        public CacheableTablesTDSContentsRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((CacheableTablesTDSContentsTable)(this.Table));
        }
        
        /// 
        public String TableName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTableName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTableName) 
                            || (((String)(this[this.myTable.ColumnTableName])) != value)))
                {
                    this[this.myTable.ColumnTableName] = value;
                }
            }
        }
        
        /// 
        public Boolean DataUpToDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDataUpToDate.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Boolean)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDataUpToDate) 
                            || (((Boolean)(this[this.myTable.ColumnDataUpToDate])) != value)))
                {
                    this[this.myTable.ColumnDataUpToDate] = value;
                }
            }
        }
        
        /// 
        public Boolean DataChanged
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDataChanged.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Boolean)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDataChanged) 
                            || (((Boolean)(this[this.myTable.ColumnDataChanged])) != value)))
                {
                    this[this.myTable.ColumnDataChanged] = value;
                }
            }
        }
        
        /// 
        public Boolean ChangesSavedExternally
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnChangesSavedExternally.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Boolean)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnChangesSavedExternally) 
                            || (((Boolean)(this[this.myTable.ColumnChangesSavedExternally])) != value)))
                {
                    this[this.myTable.ColumnChangesSavedExternally] = value;
                }
            }
        }
        
        /// 
        public DateTime CachedSince
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCachedSince.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCachedSince) 
                            || (((DateTime)(this[this.myTable.ColumnCachedSince])) != value)))
                {
                    this[this.myTable.ColumnCachedSince] = value;
                }
            }
        }
        
        /// 
        public DateTime LastAccessed
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastAccessed.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnLastAccessed) 
                            || (((DateTime)(this[this.myTable.ColumnLastAccessed])) != value)))
                {
                    this[this.myTable.ColumnLastAccessed] = value;
                }
            }
        }
        
        /// 
        public String HashCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnHashCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnHashCode) 
                            || (((String)(this[this.myTable.ColumnHashCode])) != value)))
                {
                    this[this.myTable.ColumnHashCode] = value;
                }
            }
        }
        
        /// 
        public Int32 TableSize
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTableSize.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Int32)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTableSize) 
                            || (((Int32)(this[this.myTable.ColumnTableSize])) != value)))
                {
                    this[this.myTable.ColumnTableSize] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnTableName);
            this.SetNull(this.myTable.ColumnDataUpToDate);
            this.SetNull(this.myTable.ColumnDataChanged);
            this.SetNull(this.myTable.ColumnChangesSavedExternally);
            this.SetNull(this.myTable.ColumnCachedSince);
            this.SetNull(this.myTable.ColumnLastAccessed);
            this.SetNull(this.myTable.ColumnHashCode);
            this.SetNull(this.myTable.ColumnTableSize);
        }
        
        /// test for NULL value
        public bool IsTableNameNull()
        {
            return this.IsNull(this.myTable.ColumnTableName);
        }
        
        /// assign NULL value
        public void SetTableNameNull()
        {
            this.SetNull(this.myTable.ColumnTableName);
        }
        
        /// test for NULL value
        public bool IsDataUpToDateNull()
        {
            return this.IsNull(this.myTable.ColumnDataUpToDate);
        }
        
        /// assign NULL value
        public void SetDataUpToDateNull()
        {
            this.SetNull(this.myTable.ColumnDataUpToDate);
        }
        
        /// test for NULL value
        public bool IsDataChangedNull()
        {
            return this.IsNull(this.myTable.ColumnDataChanged);
        }
        
        /// assign NULL value
        public void SetDataChangedNull()
        {
            this.SetNull(this.myTable.ColumnDataChanged);
        }
        
        /// test for NULL value
        public bool IsChangesSavedExternallyNull()
        {
            return this.IsNull(this.myTable.ColumnChangesSavedExternally);
        }
        
        /// assign NULL value
        public void SetChangesSavedExternallyNull()
        {
            this.SetNull(this.myTable.ColumnChangesSavedExternally);
        }
        
        /// test for NULL value
        public bool IsCachedSinceNull()
        {
            return this.IsNull(this.myTable.ColumnCachedSince);
        }
        
        /// assign NULL value
        public void SetCachedSinceNull()
        {
            this.SetNull(this.myTable.ColumnCachedSince);
        }
        
        /// test for NULL value
        public bool IsLastAccessedNull()
        {
            return this.IsNull(this.myTable.ColumnLastAccessed);
        }
        
        /// assign NULL value
        public void SetLastAccessedNull()
        {
            this.SetNull(this.myTable.ColumnLastAccessed);
        }
        
        /// test for NULL value
        public bool IsHashCodeNull()
        {
            return this.IsNull(this.myTable.ColumnHashCode);
        }
        
        /// assign NULL value
        public void SetHashCodeNull()
        {
            this.SetNull(this.myTable.ColumnHashCode);
        }
        
        /// test for NULL value
        public bool IsTableSizeNull()
        {
            return this.IsNull(this.myTable.ColumnTableSize);
        }
        
        /// assign NULL value
        public void SetTableSizeNull()
        {
            this.SetNull(this.myTable.ColumnTableSize);
        }
    }
    
    /// auto generated
    [Serializable()]
    public class CacheableTablesTDS : TTypedDataSet
    {
        
        private CacheableTablesTDSContentsTable TableContents;
        
        /// auto generated
        public CacheableTablesTDS() : 
                base("CacheableTablesTDS")
        {
        }
        
        /// auto generated for serialization
        public CacheableTablesTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// auto generated
        public CacheableTablesTDS(string ADatasetName) : 
                base(ADatasetName)
        {
        }
        
        /// auto generated
        public CacheableTablesTDSContentsTable Contents
        {
            get
            {
                return this.TableContents;
            }
        }
        
        /// auto generated
        public new virtual CacheableTablesTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((CacheableTablesTDS)(base.GetChangesTyped(removeEmptyTables)));
        }
        
        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new CacheableTablesTDSContentsTable("Contents"));
        }
        
        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("Contents") != -1))
            {
                this.Tables.Add(new CacheableTablesTDSContentsTable("Contents"));
            }
        }
        
        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TableContents != null))
            {
                this.TableContents.InitVars();
            }
        }
        
        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "CacheableTablesTDS";
            this.TableContents = ((CacheableTablesTDSContentsTable)(this.Tables["Contents"]));
        }
        
        /// auto generated
        protected override void InitConstraints()
        {
        }
    }
    
    /// auto generated
    [Serializable()]
    public class FieldOfServiceTDS : TTypedDataSet
    {
        
        private PPartnerTable TablePPartner;
        
        private PPartnerFieldOfServiceTable TablePPartnerFieldOfService;
        
        private PPersonTable TablePPerson;
        
        private PFamilyTable TablePFamily;
        
        private PmStaffDataTable TablePmStaffData;
        
        /// auto generated
        public FieldOfServiceTDS() : 
                base("FieldOfServiceTDS")
        {
        }
        
        /// auto generated for serialization
        public FieldOfServiceTDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// auto generated
        public FieldOfServiceTDS(string ADatasetName) : 
                base(ADatasetName)
        {
        }
        
        /// auto generated
        public PPartnerTable PPartner
        {
            get
            {
                return this.TablePPartner;
            }
        }
        
        /// auto generated
        public PPartnerFieldOfServiceTable PPartnerFieldOfService
        {
            get
            {
                return this.TablePPartnerFieldOfService;
            }
        }
        
        /// auto generated
        public PPersonTable PPerson
        {
            get
            {
                return this.TablePPerson;
            }
        }
        
        /// auto generated
        public PFamilyTable PFamily
        {
            get
            {
                return this.TablePFamily;
            }
        }
        
        /// auto generated
        public PmStaffDataTable PmStaffData
        {
            get
            {
                return this.TablePmStaffData;
            }
        }
        
        /// auto generated
        public new virtual FieldOfServiceTDS GetChangesTyped(bool removeEmptyTables)
        {
            return ((FieldOfServiceTDS)(base.GetChangesTyped(removeEmptyTables)));
        }
        
        /// auto generated
        protected override void InitTables()
        {
            this.Tables.Add(new PPartnerTable("PPartner"));
            this.Tables.Add(new PPartnerFieldOfServiceTable("PPartnerFieldOfService"));
            this.Tables.Add(new PPersonTable("PPerson"));
            this.Tables.Add(new PFamilyTable("PFamily"));
            this.Tables.Add(new PmStaffDataTable("PmStaffData"));
        }
        
        /// auto generated
        protected override void InitTables(System.Data.DataSet ds)
        {
            if ((ds.Tables.IndexOf("PPartner") != -1))
            {
                this.Tables.Add(new PPartnerTable("PPartner"));
            }
            if ((ds.Tables.IndexOf("PPartnerFieldOfService") != -1))
            {
                this.Tables.Add(new PPartnerFieldOfServiceTable("PPartnerFieldOfService"));
            }
            if ((ds.Tables.IndexOf("PPerson") != -1))
            {
                this.Tables.Add(new PPersonTable("PPerson"));
            }
            if ((ds.Tables.IndexOf("PFamily") != -1))
            {
                this.Tables.Add(new PFamilyTable("PFamily"));
            }
            if ((ds.Tables.IndexOf("PmStaffData") != -1))
            {
                this.Tables.Add(new PmStaffDataTable("PmStaffData"));
            }
        }
        
        /// auto generated
        protected override void MapTables()
        {
            this.InitVars();
            base.MapTables();
            if ((this.TablePPartner != null))
            {
                this.TablePPartner.InitVars();
            }
            if ((this.TablePPartnerFieldOfService != null))
            {
                this.TablePPartnerFieldOfService.InitVars();
            }
            if ((this.TablePPerson != null))
            {
                this.TablePPerson.InitVars();
            }
            if ((this.TablePFamily != null))
            {
                this.TablePFamily.InitVars();
            }
            if ((this.TablePmStaffData != null))
            {
                this.TablePmStaffData.InitVars();
            }
        }
        
        /// auto generated
        public override void InitVars()
        {
            this.DataSetName = "FieldOfServiceTDS";
            this.TablePPartner = ((PPartnerTable)(this.Tables["PPartner"]));
            this.TablePPartnerFieldOfService = ((PPartnerFieldOfServiceTable)(this.Tables["PPartnerFieldOfService"]));
            this.TablePPerson = ((PPersonTable)(this.Tables["PPerson"]));
            this.TablePFamily = ((PFamilyTable)(this.Tables["PFamily"]));
            this.TablePmStaffData = ((PmStaffDataTable)(this.Tables["PmStaffData"]));
        }
        
        /// auto generated
        protected override void InitConstraints()
        {
            if (((this.TablePPartner != null) 
                        && (this.TablePPartnerFieldOfService != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerFieldOfService1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPartnerFieldOfService", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePmStaffData != null) 
                        && (this.TablePPartnerFieldOfService != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPartnerFieldOfService4", "PmStaffData", new string[] {
                                "p_site_key_n",
                                "pm_key_n"}, "PPartnerFieldOfService", new string[] {
                                "p_commitment_site_key_n",
                                "p_commitment_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePPerson != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPerson1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PPerson", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePFamily != null) 
                        && (this.TablePPerson != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKPerson2", "PFamily", new string[] {
                                "p_partner_key_n"}, "PPerson", new string[] {
                                "p_family_key_n"}));
            }
            if (((this.TablePPartner != null) 
                        && (this.TablePFamily != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKFamily1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PFamily", new string[] {
                                "p_partner_key_n"}));
            }
            if (((this.TablePPerson != null) 
                        && (this.TablePmStaffData != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKStaffData1", "PPerson", new string[] {
                                "p_partner_key_n"}, "PmStaffData", new string[] {
                                "p_partner_key_n"}));
            }
        }
    }
}
