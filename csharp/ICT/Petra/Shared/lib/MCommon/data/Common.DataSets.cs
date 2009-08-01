/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       auto generated
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
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
            if (((this.TableDataLabelLookupCategoryList != null)
                        && (this.TableDataLabelLookupList != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDataLabelLookup1", "DataLabelLookupCategoryList", new string[] {
                                "p_category_code_c"}, "DataLabelLookupList", new string[] {
                                "p_category_code_c"}));
            }
            if (((this.TableDataLabelList != null)
                        && (this.TableDataLabelUseList != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKDataLabelUse1", "DataLabelList", new string[] {
                                "p_key_i"}, "DataLabelUseList", new string[] {
                                "p_data_label_key_i"}));
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

    ///
    [Serializable()]
    public class CacheableTablesTDSContentsTable : TTypedDataTable
    {
        /// Name of the Cached DataTable
        public DataColumn ColumnTableName;
        /// Tells whether the data in the Cached DataTable is the same than the external source of the data
        public DataColumn ColumnDataUpToDate;
        /// Tells whether data in the Cached DataTable was changed
        public DataColumn ColumnDataChanged;
        /// Tells whether changed data in the CachedDataTable was saved externally
        public DataColumn ColumnChangesSavedExternally;
        /// Date and Time when the DataTable was added to the Cache
        public DataColumn ColumnCachedSince;
        /// Date and Time when the DataTable was last handed out of the Cache
        public DataColumn ColumnLastAccessed;
        /// HashCode of the Cached DataTable
        public DataColumn ColumnHashCode;
        /// Size of the contents of the Cached DataTable (in Bytes)
        public DataColumn ColumnTableSize;

        private static short TableId = -1;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "CacheableTablesTDSContents", "CacheableTablesTDSContents",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "TableName", "TableName", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(1, "DataUpToDate", "DataUpToDate", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(2, "DataChanged", "DataChanged", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(3, "ChangesSavedExternally", "ChangesSavedExternally", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(4, "CachedSince", "CachedSince", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(5, "LastAccessed", "LastAccessed", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(6, "HashCode", "HashCode", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(7, "TableSize", "TableSize", OdbcType.Int, -1, false)
                },
                new string[] {

                }));
            return true;
        }

        /// constructor
        public CacheableTablesTDSContentsTable() :
                base("CacheableTablesTDSContents")
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

        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("TableName", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("DataUpToDate", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("DataChanged", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("ChangesSavedExternally", typeof(bool)));
            this.Columns.Add(new System.Data.DataColumn("CachedSince", typeof(DateTime)));
            this.Columns.Add(new System.Data.DataColumn("LastAccessed", typeof(DateTime)));
            this.Columns.Add(new System.Data.DataColumn("HashCode", typeof(string)));
            this.Columns.Add(new System.Data.DataColumn("TableSize", typeof(Int32)));
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
        }

        /// Access a typed row by index
        public CacheableTablesTDSContentsRow this[int i]
        {
            get
            {
                return ((CacheableTablesTDSContentsRow)(this.Rows[i]));
            }
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

        /// create a new typed row, always with default values
        public CacheableTablesTDSContentsRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }

        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new CacheableTablesTDSContentsRow(builder);
        }

        /// get typed set of changes
        public CacheableTablesTDSContentsTable GetChangesTyped()
        {
            return ((CacheableTablesTDSContentsTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        protected static string GetTableNameDBName()
        {
            return "TableName";
        }

        /// get character length for column
        public static short GetTableNameLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        protected static string GetDataUpToDateDBName()
        {
            return "DataUpToDate";
        }

        /// get character length for column
        public static short GetDataUpToDateLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        protected static string GetDataChangedDBName()
        {
            return "DataChanged";
        }

        /// get character length for column
        public static short GetDataChangedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        protected static string GetChangesSavedExternallyDBName()
        {
            return "ChangesSavedExternally";
        }

        /// get character length for column
        public static short GetChangesSavedExternallyLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        protected static string GetCachedSinceDBName()
        {
            return "CachedSince";
        }

        /// get character length for column
        public static short GetCachedSinceLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        protected static string GetLastAccessedDBName()
        {
            return "LastAccessed";
        }

        /// get character length for column
        public static short GetLastAccessedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        protected static string GetHashCodeDBName()
        {
            return "HashCode";
        }

        /// get character length for column
        public static short GetHashCodeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        protected static string GetTableSizeDBName()
        {
            return "TableSize";
        }

        /// get character length for column
        public static short GetTableSizeLength()
        {
            return -1;
        }

    }

    ///
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

        /// Name of the Cached DataTable
        public string TableName
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
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTableName)
                            || (((string)(this[this.myTable.ColumnTableName])) != value)))
                {
                    this[this.myTable.ColumnTableName] = value;
                }
            }
        }

        /// Tells whether the data in the Cached DataTable is the same than the external source of the data
        public bool DataUpToDate
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDataUpToDate)
                            || (((bool)(this[this.myTable.ColumnDataUpToDate])) != value)))
                {
                    this[this.myTable.ColumnDataUpToDate] = value;
                }
            }
        }

        /// Tells whether data in the Cached DataTable was changed
        public bool DataChanged
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDataChanged)
                            || (((bool)(this[this.myTable.ColumnDataChanged])) != value)))
                {
                    this[this.myTable.ColumnDataChanged] = value;
                }
            }
        }

        /// Tells whether changed data in the CachedDataTable was saved externally
        public bool ChangesSavedExternally
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
                    return ((bool)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnChangesSavedExternally)
                            || (((bool)(this[this.myTable.ColumnChangesSavedExternally])) != value)))
                {
                    this[this.myTable.ColumnChangesSavedExternally] = value;
                }
            }
        }

        /// Date and Time when the DataTable was added to the Cache
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

        /// Date and Time when the DataTable was last handed out of the Cache
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

        /// HashCode of the Cached DataTable
        public string HashCode
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
                    return ((string)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnHashCode)
                            || (((string)(this[this.myTable.ColumnHashCode])) != value)))
                {
                    this[this.myTable.ColumnHashCode] = value;
                }
            }
        }

        /// Size of the contents of the Cached DataTable (in Bytes)
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
                        && (this.TablePFamily != null)))
            {
                this.FConstraints.Add(new TTypedConstraint("FKFamily1", "PPartner", new string[] {
                                "p_partner_key_n"}, "PFamily", new string[] {
                                "p_partner_key_n"}));
            }
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
                                "p_site_key_n", "pm_key_n"}, "PPartnerFieldOfService", new string[] {
                                "p_commitment_site_key_n", "p_commitment_key_n"}));
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