/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
namespace Ict.Petra.Shared.MFinance.AR.Data
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Odbc;
    using System.Runtime.Serialization;
    using System.Xml;
    using Ict.Common;
    using Ict.Common.Data;
    
    
    /// used for invoicing
    [Serializable()]
    public class ATaxTypeTable : TTypedDataTable
    {
        
        /// This is whether it is GST, VAT
        public DataColumn ColumnTaxTypeCode;
        
        /// This is a short description which is 32 characters long
        public DataColumn ColumnTaxTypeDescription;
        
        /// The date the record was created.
        public DataColumn ColumnDateCreated;
        
        /// User ID of who created this record.
        public DataColumn ColumnCreatedBy;
        
        /// The date the record was modified.
        public DataColumn ColumnDateModified;
        
        /// User ID of who last modified this record.
        public DataColumn ColumnModifiedBy;
        
        /// This identifies the current version of the record.
        public DataColumn ColumnModificationId;
        
        /// constructor
        public ATaxTypeTable() : 
                base("ATaxType")
        {
        }
        
        /// constructor
        public ATaxTypeTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public ATaxTypeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public ATaxTypeRow this[int i]
        {
            get
            {
                return ((ATaxTypeRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetTaxTypeCodeDBName()
        {
            return "a_tax_type_code_c";
        }
        
        /// get help text for column
        public static string GetTaxTypeCodeHelp()
        {
            return "Enter a tax type code";
        }
        
        /// get label of column
        public static string GetTaxTypeCodeLabel()
        {
            return "Tax Type Code";
        }
        
        /// get character length for column
        public static short GetTaxTypeCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTaxTypeDescriptionDBName()
        {
            return "a_tax_type_description_c";
        }
        
        /// get help text for column
        public static string GetTaxTypeDescriptionHelp()
        {
            return "Enter a description";
        }
        
        /// get label of column
        public static string GetTaxTypeDescriptionLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetTaxTypeDescriptionLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }
        
        /// get help text for column
        public static string GetDateCreatedHelp()
        {
            return "The date the record was created.";
        }
        
        /// get label of column
        public static string GetDateCreatedLabel()
        {
            return "Created Date";
        }
        
        /// get display format for column
        public static short GetDateCreatedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }
        
        /// get help text for column
        public static string GetCreatedByHelp()
        {
            return "User ID of who created this record.";
        }
        
        /// get label of column
        public static string GetCreatedByLabel()
        {
            return "Created By";
        }
        
        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }
        
        /// get help text for column
        public static string GetDateModifiedHelp()
        {
            return "The date the record was modified.";
        }
        
        /// get label of column
        public static string GetDateModifiedLabel()
        {
            return "Modified Date";
        }
        
        /// get display format for column
        public static short GetDateModifiedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }
        
        /// get help text for column
        public static string GetModifiedByHelp()
        {
            return "User ID of who last modified this record.";
        }
        
        /// get label of column
        public static string GetModifiedByLabel()
        {
            return "Modified By";
        }
        
        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }
        
        /// get help text for column
        public static string GetModificationIdHelp()
        {
            return "This identifies the current version of the record.";
        }
        
        /// get label of column
        public static string GetModificationIdLabel()
        {
            return "";
        }
        
        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "ATaxType";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_tax_type";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Tax Type";
        }
        
        /// get the index number of fields that are part of the primary key
        public static Int32[] GetPrimKeyColumnOrdList()
        {
            return new int[] {
                    0};
        }
        
        /// get the names of the columns
        public static String[] GetColumnStringList()
        {
            return new string[] {
                    "a_tax_type_code_c",
                    "a_tax_type_description_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnTaxTypeCode = this.Columns["a_tax_type_code_c"];
            this.ColumnTaxTypeDescription = this.Columns["a_tax_type_description_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnTaxTypeCode};
        }
        
        /// get typed set of changes
        public ATaxTypeTable GetChangesTyped()
        {
            return ((ATaxTypeTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public ATaxTypeRow NewRowTyped(bool AWithDefaultValues)
        {
            ATaxTypeRow ret = ((ATaxTypeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public ATaxTypeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new ATaxTypeRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_tax_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_type_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnTaxTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnTaxTypeDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
            }
            if ((ACol == ColumnDateCreated))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCreatedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateModified))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnModifiedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnModificationId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 150);
            }
            return null;
        }
    }
    
    /// used for invoicing
    [Serializable()]
    public class ATaxTypeRow : System.Data.DataRow
    {
        
        private ATaxTypeTable myTable;
        
        /// Constructor
        public ATaxTypeRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((ATaxTypeTable)(this.Table));
        }
        
        /// This is whether it is GST, VAT
        public String TaxTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxTypeCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTaxTypeCode) 
                            || (((String)(this[this.myTable.ColumnTaxTypeCode])) != value)))
                {
                    this[this.myTable.ColumnTaxTypeCode] = value;
                }
            }
        }
        
        /// This is a short description which is 32 characters long
        public String TaxTypeDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxTypeDescription.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTaxTypeDescription) 
                            || (((String)(this[this.myTable.ColumnTaxTypeDescription])) != value)))
                {
                    this[this.myTable.ColumnTaxTypeDescription] = value;
                }
            }
        }
        
        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateCreated])) != value)))
                {
                    this[this.myTable.ColumnDateCreated] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateCreatedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateCreatedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCreatedBy) 
                            || (((String)(this[this.myTable.ColumnCreatedBy])) != value)))
                {
                    this[this.myTable.ColumnCreatedBy] = value;
                }
            }
        }
        
        /// The date the record was modified.
        public System.DateTime DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateModified])) != value)))
                {
                    this[this.myTable.ColumnDateModified] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateModifiedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateModifiedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModifiedBy) 
                            || (((String)(this[this.myTable.ColumnModifiedBy])) != value)))
                {
                    this[this.myTable.ColumnModifiedBy] = value;
                }
            }
        }
        
        /// This identifies the current version of the record.
        public String ModificationId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModificationId.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModificationId) 
                            || (((String)(this[this.myTable.ColumnModificationId])) != value)))
                {
                    this[this.myTable.ColumnModificationId] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnTaxTypeCode);
            this.SetNull(this.myTable.ColumnTaxTypeDescription);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsTaxTypeDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnTaxTypeDescription);
        }
        
        /// assign NULL value
        public void SetTaxTypeDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnTaxTypeDescription);
        }
        
        /// test for NULL value
        public bool IsDateCreatedNull()
        {
            return this.IsNull(this.myTable.ColumnDateCreated);
        }
        
        /// assign NULL value
        public void SetDateCreatedNull()
        {
            this.SetNull(this.myTable.ColumnDateCreated);
        }
        
        /// test for NULL value
        public bool IsCreatedByNull()
        {
            return this.IsNull(this.myTable.ColumnCreatedBy);
        }
        
        /// assign NULL value
        public void SetCreatedByNull()
        {
            this.SetNull(this.myTable.ColumnCreatedBy);
        }
        
        /// test for NULL value
        public bool IsDateModifiedNull()
        {
            return this.IsNull(this.myTable.ColumnDateModified);
        }
        
        /// assign NULL value
        public void SetDateModifiedNull()
        {
            this.SetNull(this.myTable.ColumnDateModified);
        }
        
        /// test for NULL value
        public bool IsModifiedByNull()
        {
            return this.IsNull(this.myTable.ColumnModifiedBy);
        }
        
        /// assign NULL value
        public void SetModifiedByNull()
        {
            this.SetNull(this.myTable.ColumnModifiedBy);
        }
        
        /// test for NULL value
        public bool IsModificationIdNull()
        {
            return this.IsNull(this.myTable.ColumnModificationId);
        }
        
        /// assign NULL value
        public void SetModificationIdNull()
        {
            this.SetNull(this.myTable.ColumnModificationId);
        }
    }
    
    /// This is used by the invoicing
    [Serializable()]
    public class ATaxTableTable : TTypedDataTable
    {
        
        /// This is used as a key field in most of the accounting system files
        public DataColumn ColumnLedgerNumber;
        
        /// The tax type is always the same, e.g. VAT
        public DataColumn ColumnTaxTypeCode;
        
        /// this describes whether it is e.g. the standard, reduced or zero rate of VAT
        public DataColumn ColumnTaxRateCode;
        
        /// this describes when this particular percentage rate has become valid by law
        public DataColumn ColumnTaxValidFrom;
        
        /// This is a short description which is 32 charcters long
        public DataColumn ColumnTaxRateDescription;
        
        /// Tax rate
        public DataColumn ColumnTaxRate;
        
        /// flag that prevents this rate from being used, e.g. if it has been replaced by another rate
        public DataColumn ColumnActive;
        
        /// The date the record was created.
        public DataColumn ColumnDateCreated;
        
        /// User ID of who created this record.
        public DataColumn ColumnCreatedBy;
        
        /// The date the record was modified.
        public DataColumn ColumnDateModified;
        
        /// User ID of who last modified this record.
        public DataColumn ColumnModifiedBy;
        
        /// This identifies the current version of the record.
        public DataColumn ColumnModificationId;
        
        /// constructor
        public ATaxTableTable() : 
                base("ATaxTable")
        {
        }
        
        /// constructor
        public ATaxTableTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public ATaxTableTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public ATaxTableRow this[int i]
        {
            get
            {
                return ((ATaxTableRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }
        
        /// get help text for column
        public static string GetLedgerNumberHelp()
        {
            return "Enter the ledger number";
        }
        
        /// get label of column
        public static string GetLedgerNumberLabel()
        {
            return "Ledger Number";
        }
        
        /// get display format for column
        public static short GetLedgerNumberLength()
        {
            return 4;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTaxTypeCodeDBName()
        {
            return "a_tax_type_code_c";
        }
        
        /// get help text for column
        public static string GetTaxTypeCodeHelp()
        {
            return "Enter a tax type code";
        }
        
        /// get label of column
        public static string GetTaxTypeCodeLabel()
        {
            return "Tax Type Code";
        }
        
        /// get character length for column
        public static short GetTaxTypeCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTaxRateCodeDBName()
        {
            return "a_tax_rate_code_c";
        }
        
        /// get help text for column
        public static string GetTaxRateCodeHelp()
        {
            return "Enter a tax rate code";
        }
        
        /// get label of column
        public static string GetTaxRateCodeLabel()
        {
            return "Tax Rate Code";
        }
        
        /// get character length for column
        public static short GetTaxRateCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTaxValidFromDBName()
        {
            return "a_tax_valid_from_d";
        }
        
        /// get help text for column
        public static string GetTaxValidFromHelp()
        {
            return "this describes when this particular percentage rate has become valid by law";
        }
        
        /// get label of column
        public static string GetTaxValidFromLabel()
        {
            return "a_tax_valid_from_d";
        }
        
        /// get the name of the field in the database for this column
        public static string GetTaxRateDescriptionDBName()
        {
            return "a_tax_rate_description_c";
        }
        
        /// get help text for column
        public static string GetTaxRateDescriptionHelp()
        {
            return "Enter a description";
        }
        
        /// get label of column
        public static string GetTaxRateDescriptionLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetTaxRateDescriptionLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTaxRateDBName()
        {
            return "a_tax_rate_n";
        }
        
        /// get help text for column
        public static string GetTaxRateHelp()
        {
            return "Enter a tax rate.";
        }
        
        /// get label of column
        public static string GetTaxRateLabel()
        {
            return "Tax Rate";
        }
        
        /// get display format for column
        public static short GetTaxRateLength()
        {
            return 7;
        }
        
        /// get the name of the field in the database for this column
        public static string GetActiveDBName()
        {
            return "a_active_l";
        }
        
        /// get help text for column
        public static string GetActiveHelp()
        {
            return "flag that prevents this rate from being used, e.g. if it has been replaced by ano" +
                "ther rate";
        }
        
        /// get label of column
        public static string GetActiveLabel()
        {
            return "a_active_l";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }
        
        /// get help text for column
        public static string GetDateCreatedHelp()
        {
            return "The date the record was created.";
        }
        
        /// get label of column
        public static string GetDateCreatedLabel()
        {
            return "Created Date";
        }
        
        /// get display format for column
        public static short GetDateCreatedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }
        
        /// get help text for column
        public static string GetCreatedByHelp()
        {
            return "User ID of who created this record.";
        }
        
        /// get label of column
        public static string GetCreatedByLabel()
        {
            return "Created By";
        }
        
        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }
        
        /// get help text for column
        public static string GetDateModifiedHelp()
        {
            return "The date the record was modified.";
        }
        
        /// get label of column
        public static string GetDateModifiedLabel()
        {
            return "Modified Date";
        }
        
        /// get display format for column
        public static short GetDateModifiedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }
        
        /// get help text for column
        public static string GetModifiedByHelp()
        {
            return "User ID of who last modified this record.";
        }
        
        /// get label of column
        public static string GetModifiedByLabel()
        {
            return "Modified By";
        }
        
        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }
        
        /// get help text for column
        public static string GetModificationIdHelp()
        {
            return "This identifies the current version of the record.";
        }
        
        /// get label of column
        public static string GetModificationIdLabel()
        {
            return "";
        }
        
        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "ATaxTable";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_tax_table";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Tax Table";
        }
        
        /// get the index number of fields that are part of the primary key
        public static Int32[] GetPrimKeyColumnOrdList()
        {
            return new int[] {
                    0,
                    1,
                    2,
                    3};
        }
        
        /// get the names of the columns
        public static String[] GetColumnStringList()
        {
            return new string[] {
                    "a_ledger_number_i",
                    "a_tax_type_code_c",
                    "a_tax_rate_code_c",
                    "a_tax_valid_from_d",
                    "a_tax_rate_description_c",
                    "a_tax_rate_n",
                    "a_active_l",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnTaxTypeCode = this.Columns["a_tax_type_code_c"];
            this.ColumnTaxRateCode = this.Columns["a_tax_rate_code_c"];
            this.ColumnTaxValidFrom = this.Columns["a_tax_valid_from_d"];
            this.ColumnTaxRateDescription = this.Columns["a_tax_rate_description_c"];
            this.ColumnTaxRate = this.Columns["a_tax_rate_n"];
            this.ColumnActive = this.Columns["a_active_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLedgerNumber,
                    this.ColumnTaxTypeCode,
                    this.ColumnTaxRateCode,
                    this.ColumnTaxValidFrom};
        }
        
        /// get typed set of changes
        public ATaxTableTable GetChangesTyped()
        {
            return ((ATaxTableTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public ATaxTableRow NewRowTyped(bool AWithDefaultValues)
        {
            ATaxTableRow ret = ((ATaxTableRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public ATaxTableRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new ATaxTableRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_rate_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_valid_from_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_rate_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_rate_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_active_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLedgerNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnTaxTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnTaxRateCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnTaxValidFrom))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnTaxRateDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
            }
            if ((ACol == ColumnTaxRate))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnActive))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnDateCreated))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCreatedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateModified))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnModifiedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnModificationId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 150);
            }
            return null;
        }
    }
    
    /// This is used by the invoicing
    [Serializable()]
    public class ATaxTableRow : System.Data.DataRow
    {
        
        private ATaxTableTable myTable;
        
        /// Constructor
        public ATaxTableRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((ATaxTableTable)(this.Table));
        }
        
        /// This is used as a key field in most of the accounting system files
        public Int32 LedgerNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLedgerNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLedgerNumber) 
                            || (((Int32)(this[this.myTable.ColumnLedgerNumber])) != value)))
                {
                    this[this.myTable.ColumnLedgerNumber] = value;
                }
            }
        }
        
        /// The tax type is always the same, e.g. VAT
        public String TaxTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxTypeCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTaxTypeCode) 
                            || (((String)(this[this.myTable.ColumnTaxTypeCode])) != value)))
                {
                    this[this.myTable.ColumnTaxTypeCode] = value;
                }
            }
        }
        
        /// this describes whether it is e.g. the standard, reduced or zero rate of VAT
        public String TaxRateCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxRateCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTaxRateCode) 
                            || (((String)(this[this.myTable.ColumnTaxRateCode])) != value)))
                {
                    this[this.myTable.ColumnTaxRateCode] = value;
                }
            }
        }
        
        /// this describes when this particular percentage rate has become valid by law
        public System.DateTime TaxValidFrom
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxValidFrom.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTaxValidFrom) 
                            || (((System.DateTime)(this[this.myTable.ColumnTaxValidFrom])) != value)))
                {
                    this[this.myTable.ColumnTaxValidFrom] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime TaxValidFromLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnTaxValidFrom], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime TaxValidFromHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnTaxValidFrom.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// This is a short description which is 32 charcters long
        public String TaxRateDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxRateDescription.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTaxRateDescription) 
                            || (((String)(this[this.myTable.ColumnTaxRateDescription])) != value)))
                {
                    this[this.myTable.ColumnTaxRateDescription] = value;
                }
            }
        }
        
        /// Tax rate
        public Double TaxRate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxRate.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Double)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTaxRate) 
                            || (((Double)(this[this.myTable.ColumnTaxRate])) != value)))
                {
                    this[this.myTable.ColumnTaxRate] = value;
                }
            }
        }
        
        /// flag that prevents this rate from being used, e.g. if it has been replaced by another rate
        public Boolean Active
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnActive.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnActive) 
                            || (((Boolean)(this[this.myTable.ColumnActive])) != value)))
                {
                    this[this.myTable.ColumnActive] = value;
                }
            }
        }
        
        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateCreated])) != value)))
                {
                    this[this.myTable.ColumnDateCreated] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateCreatedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateCreatedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCreatedBy) 
                            || (((String)(this[this.myTable.ColumnCreatedBy])) != value)))
                {
                    this[this.myTable.ColumnCreatedBy] = value;
                }
            }
        }
        
        /// The date the record was modified.
        public System.DateTime DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateModified])) != value)))
                {
                    this[this.myTable.ColumnDateModified] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateModifiedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateModifiedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModifiedBy) 
                            || (((String)(this[this.myTable.ColumnModifiedBy])) != value)))
                {
                    this[this.myTable.ColumnModifiedBy] = value;
                }
            }
        }
        
        /// This identifies the current version of the record.
        public String ModificationId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModificationId.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModificationId) 
                            || (((String)(this[this.myTable.ColumnModificationId])) != value)))
                {
                    this[this.myTable.ColumnModificationId] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnTaxTypeCode);
            this.SetNull(this.myTable.ColumnTaxRateCode);
            this.SetNull(this.myTable.ColumnTaxValidFrom);
            this.SetNull(this.myTable.ColumnTaxRateDescription);
            this[this.myTable.ColumnTaxRate.Ordinal] = 0;
            this[this.myTable.ColumnActive.Ordinal] = true;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsTaxRateDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnTaxRateDescription);
        }
        
        /// assign NULL value
        public void SetTaxRateDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnTaxRateDescription);
        }
        
        /// test for NULL value
        public bool IsTaxRateNull()
        {
            return this.IsNull(this.myTable.ColumnTaxRate);
        }
        
        /// assign NULL value
        public void SetTaxRateNull()
        {
            this.SetNull(this.myTable.ColumnTaxRate);
        }
        
        /// test for NULL value
        public bool IsActiveNull()
        {
            return this.IsNull(this.myTable.ColumnActive);
        }
        
        /// assign NULL value
        public void SetActiveNull()
        {
            this.SetNull(this.myTable.ColumnActive);
        }
        
        /// test for NULL value
        public bool IsDateCreatedNull()
        {
            return this.IsNull(this.myTable.ColumnDateCreated);
        }
        
        /// assign NULL value
        public void SetDateCreatedNull()
        {
            this.SetNull(this.myTable.ColumnDateCreated);
        }
        
        /// test for NULL value
        public bool IsCreatedByNull()
        {
            return this.IsNull(this.myTable.ColumnCreatedBy);
        }
        
        /// assign NULL value
        public void SetCreatedByNull()
        {
            this.SetNull(this.myTable.ColumnCreatedBy);
        }
        
        /// test for NULL value
        public bool IsDateModifiedNull()
        {
            return this.IsNull(this.myTable.ColumnDateModified);
        }
        
        /// assign NULL value
        public void SetDateModifiedNull()
        {
            this.SetNull(this.myTable.ColumnDateModified);
        }
        
        /// test for NULL value
        public bool IsModifiedByNull()
        {
            return this.IsNull(this.myTable.ColumnModifiedBy);
        }
        
        /// assign NULL value
        public void SetModifiedByNull()
        {
            this.SetNull(this.myTable.ColumnModifiedBy);
        }
        
        /// test for NULL value
        public bool IsModificationIdNull()
        {
            return this.IsNull(this.myTable.ColumnModificationId);
        }
        
        /// assign NULL value
        public void SetModificationIdNull()
        {
            this.SetNull(this.myTable.ColumnModificationId);
        }
    }
    
    /// there are several categories that are can use invoicing: catering, hospitality, store and fees
    [Serializable()]
    public class AArCategoryTable : TTypedDataTable
    {
        
        /// categories help to specify certain discounts and group articles etc
        public DataColumn ColumnArCategoryCode;
        
        /// description of this category
        public DataColumn ColumnArDescription;
        
        /// description of this category in the local language
        public DataColumn ColumnArLocalDescription;
        
        /// The date the record was created.
        public DataColumn ColumnDateCreated;
        
        /// User ID of who created this record.
        public DataColumn ColumnCreatedBy;
        
        /// The date the record was modified.
        public DataColumn ColumnDateModified;
        
        /// User ID of who last modified this record.
        public DataColumn ColumnModifiedBy;
        
        /// This identifies the current version of the record.
        public DataColumn ColumnModificationId;
        
        /// constructor
        public AArCategoryTable() : 
                base("AArCategory")
        {
        }
        
        /// constructor
        public AArCategoryTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public AArCategoryTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public AArCategoryRow this[int i]
        {
            get
            {
                return ((AArCategoryRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetArCategoryCodeDBName()
        {
            return "a_ar_category_code_c";
        }
        
        /// get help text for column
        public static string GetArCategoryCodeHelp()
        {
            return "categories help to specify certain discounts and group articles etc";
        }
        
        /// get label of column
        public static string GetArCategoryCodeLabel()
        {
            return "a_ar_category_code_c";
        }
        
        /// get character length for column
        public static short GetArCategoryCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDescriptionDBName()
        {
            return "a_ar_description_c";
        }
        
        /// get help text for column
        public static string GetArDescriptionHelp()
        {
            return "description of this category";
        }
        
        /// get label of column
        public static string GetArDescriptionLabel()
        {
            return "a_ar_description_c";
        }
        
        /// get character length for column
        public static short GetArDescriptionLength()
        {
            return 150;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArLocalDescriptionDBName()
        {
            return "a_ar_local_description_c";
        }
        
        /// get help text for column
        public static string GetArLocalDescriptionHelp()
        {
            return "description of this category in the local language";
        }
        
        /// get label of column
        public static string GetArLocalDescriptionLabel()
        {
            return "a_ar_local_description_c";
        }
        
        /// get character length for column
        public static short GetArLocalDescriptionLength()
        {
            return 150;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }
        
        /// get help text for column
        public static string GetDateCreatedHelp()
        {
            return "The date the record was created.";
        }
        
        /// get label of column
        public static string GetDateCreatedLabel()
        {
            return "Created Date";
        }
        
        /// get display format for column
        public static short GetDateCreatedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }
        
        /// get help text for column
        public static string GetCreatedByHelp()
        {
            return "User ID of who created this record.";
        }
        
        /// get label of column
        public static string GetCreatedByLabel()
        {
            return "Created By";
        }
        
        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }
        
        /// get help text for column
        public static string GetDateModifiedHelp()
        {
            return "The date the record was modified.";
        }
        
        /// get label of column
        public static string GetDateModifiedLabel()
        {
            return "Modified Date";
        }
        
        /// get display format for column
        public static short GetDateModifiedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }
        
        /// get help text for column
        public static string GetModifiedByHelp()
        {
            return "User ID of who last modified this record.";
        }
        
        /// get label of column
        public static string GetModifiedByLabel()
        {
            return "Modified By";
        }
        
        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }
        
        /// get help text for column
        public static string GetModificationIdHelp()
        {
            return "This identifies the current version of the record.";
        }
        
        /// get label of column
        public static string GetModificationIdLabel()
        {
            return "";
        }
        
        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "AArCategory";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ar_category";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Invoicing Category";
        }
        
        /// get the index number of fields that are part of the primary key
        public static Int32[] GetPrimKeyColumnOrdList()
        {
            return new int[] {
                    0};
        }
        
        /// get the names of the columns
        public static String[] GetColumnStringList()
        {
            return new string[] {
                    "a_ar_category_code_c",
                    "a_ar_description_c",
                    "a_ar_local_description_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnArCategoryCode = this.Columns["a_ar_category_code_c"];
            this.ColumnArDescription = this.Columns["a_ar_description_c"];
            this.ColumnArLocalDescription = this.Columns["a_ar_local_description_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnArCategoryCode};
        }
        
        /// get typed set of changes
        public AArCategoryTable GetChangesTyped()
        {
            return ((AArCategoryTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public AArCategoryRow NewRowTyped(bool AWithDefaultValues)
        {
            AArCategoryRow ret = ((AArCategoryRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public AArCategoryRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AArCategoryRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ar_category_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_local_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnArCategoryCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnArDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 300);
            }
            if ((ACol == ColumnArLocalDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 300);
            }
            if ((ACol == ColumnDateCreated))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCreatedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateModified))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnModifiedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnModificationId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 150);
            }
            return null;
        }
    }
    
    /// there are several categories that are can use invoicing: catering, hospitality, store and fees
    [Serializable()]
    public class AArCategoryRow : System.Data.DataRow
    {
        
        private AArCategoryTable myTable;
        
        /// Constructor
        public AArCategoryRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((AArCategoryTable)(this.Table));
        }
        
        /// categories help to specify certain discounts and group articles etc
        public String ArCategoryCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArCategoryCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArCategoryCode) 
                            || (((String)(this[this.myTable.ColumnArCategoryCode])) != value)))
                {
                    this[this.myTable.ColumnArCategoryCode] = value;
                }
            }
        }
        
        /// description of this category
        public String ArDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDescription.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDescription) 
                            || (((String)(this[this.myTable.ColumnArDescription])) != value)))
                {
                    this[this.myTable.ColumnArDescription] = value;
                }
            }
        }
        
        /// description of this category in the local language
        public String ArLocalDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArLocalDescription.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArLocalDescription) 
                            || (((String)(this[this.myTable.ColumnArLocalDescription])) != value)))
                {
                    this[this.myTable.ColumnArLocalDescription] = value;
                }
            }
        }
        
        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateCreated])) != value)))
                {
                    this[this.myTable.ColumnDateCreated] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateCreatedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateCreatedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCreatedBy) 
                            || (((String)(this[this.myTable.ColumnCreatedBy])) != value)))
                {
                    this[this.myTable.ColumnCreatedBy] = value;
                }
            }
        }
        
        /// The date the record was modified.
        public System.DateTime DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateModified])) != value)))
                {
                    this[this.myTable.ColumnDateModified] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateModifiedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateModifiedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModifiedBy) 
                            || (((String)(this[this.myTable.ColumnModifiedBy])) != value)))
                {
                    this[this.myTable.ColumnModifiedBy] = value;
                }
            }
        }
        
        /// This identifies the current version of the record.
        public String ModificationId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModificationId.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModificationId) 
                            || (((String)(this[this.myTable.ColumnModificationId])) != value)))
                {
                    this[this.myTable.ColumnModificationId] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnArCategoryCode);
            this.SetNull(this.myTable.ColumnArDescription);
            this.SetNull(this.myTable.ColumnArLocalDescription);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsArDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnArDescription);
        }
        
        /// assign NULL value
        public void SetArDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnArDescription);
        }
        
        /// test for NULL value
        public bool IsArLocalDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnArLocalDescription);
        }
        
        /// assign NULL value
        public void SetArLocalDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnArLocalDescription);
        }
        
        /// test for NULL value
        public bool IsDateCreatedNull()
        {
            return this.IsNull(this.myTable.ColumnDateCreated);
        }
        
        /// assign NULL value
        public void SetDateCreatedNull()
        {
            this.SetNull(this.myTable.ColumnDateCreated);
        }
        
        /// test for NULL value
        public bool IsCreatedByNull()
        {
            return this.IsNull(this.myTable.ColumnCreatedBy);
        }
        
        /// assign NULL value
        public void SetCreatedByNull()
        {
            this.SetNull(this.myTable.ColumnCreatedBy);
        }
        
        /// test for NULL value
        public bool IsDateModifiedNull()
        {
            return this.IsNull(this.myTable.ColumnDateModified);
        }
        
        /// assign NULL value
        public void SetDateModifiedNull()
        {
            this.SetNull(this.myTable.ColumnDateModified);
        }
        
        /// test for NULL value
        public bool IsModifiedByNull()
        {
            return this.IsNull(this.myTable.ColumnModifiedBy);
        }
        
        /// assign NULL value
        public void SetModifiedByNull()
        {
            this.SetNull(this.myTable.ColumnModifiedBy);
        }
        
        /// test for NULL value
        public bool IsModificationIdNull()
        {
            return this.IsNull(this.myTable.ColumnModificationId);
        }
        
        /// assign NULL value
        public void SetModificationIdNull()
        {
            this.SetNull(this.myTable.ColumnModificationId);
        }
    }
    
    /// defines an item that can be sold or a service that can be charged for; this can be used for catering, hospitality, store and fees; it can describe a specific book, or a group of equally priced books
    [Serializable()]
    public class AArArticleTable : TTypedDataTable
    {
        
        /// code that uniquely identifies the item; can also be a code of a group of equally priced items
        public DataColumn ColumnArArticleCode;
        
        /// this article belongs to a certain category (catering, hospitality, store, fees)
        public DataColumn ColumnArCategoryCode;
        
        /// this article falls into a special tax/VAT category
        public DataColumn ColumnTaxTypeCode;
        
        /// describes whether this describes a specific item, e.g. book, or a group of equally priced items
        public DataColumn ColumnArSpecificArticle;
        
        /// description of this article
        public DataColumn ColumnArDescription;
        
        /// description of this article in the local language
        public DataColumn ColumnArLocalDescription;
        
        /// The date the record was created.
        public DataColumn ColumnDateCreated;
        
        /// User ID of who created this record.
        public DataColumn ColumnCreatedBy;
        
        /// The date the record was modified.
        public DataColumn ColumnDateModified;
        
        /// User ID of who last modified this record.
        public DataColumn ColumnModifiedBy;
        
        /// This identifies the current version of the record.
        public DataColumn ColumnModificationId;
        
        /// constructor
        public AArArticleTable() : 
                base("AArArticle")
        {
        }
        
        /// constructor
        public AArArticleTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public AArArticleTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public AArArticleRow this[int i]
        {
            get
            {
                return ((AArArticleRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetArArticleCodeDBName()
        {
            return "a_ar_article_code_c";
        }
        
        /// get help text for column
        public static string GetArArticleCodeHelp()
        {
            return "code that uniquely identifies the item; can also be a code of a group of equally " +
                "priced items";
        }
        
        /// get label of column
        public static string GetArArticleCodeLabel()
        {
            return "a_ar_article_code_c";
        }
        
        /// get character length for column
        public static short GetArArticleCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArCategoryCodeDBName()
        {
            return "a_ar_category_code_c";
        }
        
        /// get help text for column
        public static string GetArCategoryCodeHelp()
        {
            return "this article belongs to a certain category (catering, hospitality, store, fees)";
        }
        
        /// get label of column
        public static string GetArCategoryCodeLabel()
        {
            return "a_ar_category_code_c";
        }
        
        /// get character length for column
        public static short GetArCategoryCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTaxTypeCodeDBName()
        {
            return "a_tax_type_code_c";
        }
        
        /// get help text for column
        public static string GetTaxTypeCodeHelp()
        {
            return "this article falls into a special tax/VAT category";
        }
        
        /// get label of column
        public static string GetTaxTypeCodeLabel()
        {
            return "a_tax_type_code_c";
        }
        
        /// get character length for column
        public static short GetTaxTypeCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArSpecificArticleDBName()
        {
            return "a_ar_specific_article_l";
        }
        
        /// get help text for column
        public static string GetArSpecificArticleHelp()
        {
            return "describes whether this describes a specific item, e.g. book, or a group of equall" +
                "y priced items";
        }
        
        /// get label of column
        public static string GetArSpecificArticleLabel()
        {
            return "a_ar_specific_article_l";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDescriptionDBName()
        {
            return "a_ar_description_c";
        }
        
        /// get help text for column
        public static string GetArDescriptionHelp()
        {
            return "description of this article";
        }
        
        /// get label of column
        public static string GetArDescriptionLabel()
        {
            return "a_ar_description_c";
        }
        
        /// get character length for column
        public static short GetArDescriptionLength()
        {
            return 150;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArLocalDescriptionDBName()
        {
            return "a_ar_local_description_c";
        }
        
        /// get help text for column
        public static string GetArLocalDescriptionHelp()
        {
            return "description of this article in the local language";
        }
        
        /// get label of column
        public static string GetArLocalDescriptionLabel()
        {
            return "a_ar_local_description_c";
        }
        
        /// get character length for column
        public static short GetArLocalDescriptionLength()
        {
            return 150;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }
        
        /// get help text for column
        public static string GetDateCreatedHelp()
        {
            return "The date the record was created.";
        }
        
        /// get label of column
        public static string GetDateCreatedLabel()
        {
            return "Created Date";
        }
        
        /// get display format for column
        public static short GetDateCreatedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }
        
        /// get help text for column
        public static string GetCreatedByHelp()
        {
            return "User ID of who created this record.";
        }
        
        /// get label of column
        public static string GetCreatedByLabel()
        {
            return "Created By";
        }
        
        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }
        
        /// get help text for column
        public static string GetDateModifiedHelp()
        {
            return "The date the record was modified.";
        }
        
        /// get label of column
        public static string GetDateModifiedLabel()
        {
            return "Modified Date";
        }
        
        /// get display format for column
        public static short GetDateModifiedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }
        
        /// get help text for column
        public static string GetModifiedByHelp()
        {
            return "User ID of who last modified this record.";
        }
        
        /// get label of column
        public static string GetModifiedByLabel()
        {
            return "Modified By";
        }
        
        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }
        
        /// get help text for column
        public static string GetModificationIdHelp()
        {
            return "This identifies the current version of the record.";
        }
        
        /// get label of column
        public static string GetModificationIdLabel()
        {
            return "";
        }
        
        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "AArArticle";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ar_article";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Chargeable Item";
        }
        
        /// get the index number of fields that are part of the primary key
        public static Int32[] GetPrimKeyColumnOrdList()
        {
            return new int[] {
                    0};
        }
        
        /// get the names of the columns
        public static String[] GetColumnStringList()
        {
            return new string[] {
                    "a_ar_article_code_c",
                    "a_ar_category_code_c",
                    "a_tax_type_code_c",
                    "a_ar_specific_article_l",
                    "a_ar_description_c",
                    "a_ar_local_description_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnArArticleCode = this.Columns["a_ar_article_code_c"];
            this.ColumnArCategoryCode = this.Columns["a_ar_category_code_c"];
            this.ColumnTaxTypeCode = this.Columns["a_tax_type_code_c"];
            this.ColumnArSpecificArticle = this.Columns["a_ar_specific_article_l"];
            this.ColumnArDescription = this.Columns["a_ar_description_c"];
            this.ColumnArLocalDescription = this.Columns["a_ar_local_description_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnArArticleCode};
        }
        
        /// get typed set of changes
        public AArArticleTable GetChangesTyped()
        {
            return ((AArArticleTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public AArArticleRow NewRowTyped(bool AWithDefaultValues)
        {
            AArArticleRow ret = ((AArArticleRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public AArArticleRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AArArticleRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ar_article_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_category_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_specific_article_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_local_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnArArticleCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnArCategoryCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnTaxTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnArSpecificArticle))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnArDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 300);
            }
            if ((ACol == ColumnArLocalDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 300);
            }
            if ((ACol == ColumnDateCreated))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCreatedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateModified))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnModifiedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnModificationId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 150);
            }
            return null;
        }
    }
    
    /// defines an item that can be sold or a service that can be charged for; this can be used for catering, hospitality, store and fees; it can describe a specific book, or a group of equally priced books
    [Serializable()]
    public class AArArticleRow : System.Data.DataRow
    {
        
        private AArArticleTable myTable;
        
        /// Constructor
        public AArArticleRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((AArArticleTable)(this.Table));
        }
        
        /// code that uniquely identifies the item; can also be a code of a group of equally priced items
        public String ArArticleCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArArticleCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArArticleCode) 
                            || (((String)(this[this.myTable.ColumnArArticleCode])) != value)))
                {
                    this[this.myTable.ColumnArArticleCode] = value;
                }
            }
        }
        
        /// this article belongs to a certain category (catering, hospitality, store, fees)
        public String ArCategoryCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArCategoryCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArCategoryCode) 
                            || (((String)(this[this.myTable.ColumnArCategoryCode])) != value)))
                {
                    this[this.myTable.ColumnArCategoryCode] = value;
                }
            }
        }
        
        /// this article falls into a special tax/VAT category
        public String TaxTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxTypeCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTaxTypeCode) 
                            || (((String)(this[this.myTable.ColumnTaxTypeCode])) != value)))
                {
                    this[this.myTable.ColumnTaxTypeCode] = value;
                }
            }
        }
        
        /// describes whether this describes a specific item, e.g. book, or a group of equally priced items
        public Boolean ArSpecificArticle
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArSpecificArticle.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnArSpecificArticle) 
                            || (((Boolean)(this[this.myTable.ColumnArSpecificArticle])) != value)))
                {
                    this[this.myTable.ColumnArSpecificArticle] = value;
                }
            }
        }
        
        /// description of this article
        public String ArDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDescription.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDescription) 
                            || (((String)(this[this.myTable.ColumnArDescription])) != value)))
                {
                    this[this.myTable.ColumnArDescription] = value;
                }
            }
        }
        
        /// description of this article in the local language
        public String ArLocalDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArLocalDescription.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArLocalDescription) 
                            || (((String)(this[this.myTable.ColumnArLocalDescription])) != value)))
                {
                    this[this.myTable.ColumnArLocalDescription] = value;
                }
            }
        }
        
        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateCreated])) != value)))
                {
                    this[this.myTable.ColumnDateCreated] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateCreatedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateCreatedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCreatedBy) 
                            || (((String)(this[this.myTable.ColumnCreatedBy])) != value)))
                {
                    this[this.myTable.ColumnCreatedBy] = value;
                }
            }
        }
        
        /// The date the record was modified.
        public System.DateTime DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateModified])) != value)))
                {
                    this[this.myTable.ColumnDateModified] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateModifiedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateModifiedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModifiedBy) 
                            || (((String)(this[this.myTable.ColumnModifiedBy])) != value)))
                {
                    this[this.myTable.ColumnModifiedBy] = value;
                }
            }
        }
        
        /// This identifies the current version of the record.
        public String ModificationId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModificationId.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModificationId) 
                            || (((String)(this[this.myTable.ColumnModificationId])) != value)))
                {
                    this[this.myTable.ColumnModificationId] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnArArticleCode);
            this.SetNull(this.myTable.ColumnArCategoryCode);
            this.SetNull(this.myTable.ColumnTaxTypeCode);
            this.SetNull(this.myTable.ColumnArSpecificArticle);
            this.SetNull(this.myTable.ColumnArDescription);
            this.SetNull(this.myTable.ColumnArLocalDescription);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsArSpecificArticleNull()
        {
            return this.IsNull(this.myTable.ColumnArSpecificArticle);
        }
        
        /// assign NULL value
        public void SetArSpecificArticleNull()
        {
            this.SetNull(this.myTable.ColumnArSpecificArticle);
        }
        
        /// test for NULL value
        public bool IsArDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnArDescription);
        }
        
        /// assign NULL value
        public void SetArDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnArDescription);
        }
        
        /// test for NULL value
        public bool IsArLocalDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnArLocalDescription);
        }
        
        /// assign NULL value
        public void SetArLocalDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnArLocalDescription);
        }
        
        /// test for NULL value
        public bool IsDateCreatedNull()
        {
            return this.IsNull(this.myTable.ColumnDateCreated);
        }
        
        /// assign NULL value
        public void SetDateCreatedNull()
        {
            this.SetNull(this.myTable.ColumnDateCreated);
        }
        
        /// test for NULL value
        public bool IsCreatedByNull()
        {
            return this.IsNull(this.myTable.ColumnCreatedBy);
        }
        
        /// assign NULL value
        public void SetCreatedByNull()
        {
            this.SetNull(this.myTable.ColumnCreatedBy);
        }
        
        /// test for NULL value
        public bool IsDateModifiedNull()
        {
            return this.IsNull(this.myTable.ColumnDateModified);
        }
        
        /// assign NULL value
        public void SetDateModifiedNull()
        {
            this.SetNull(this.myTable.ColumnDateModified);
        }
        
        /// test for NULL value
        public bool IsModifiedByNull()
        {
            return this.IsNull(this.myTable.ColumnModifiedBy);
        }
        
        /// assign NULL value
        public void SetModifiedByNull()
        {
            this.SetNull(this.myTable.ColumnModifiedBy);
        }
        
        /// test for NULL value
        public bool IsModificationIdNull()
        {
            return this.IsNull(this.myTable.ColumnModificationId);
        }
        
        /// assign NULL value
        public void SetModificationIdNull()
        {
            this.SetNull(this.myTable.ColumnModificationId);
        }
    }
    
    /// assign a price to an article, which can be updated by time
    [Serializable()]
    public class AArArticlePriceTable : TTypedDataTable
    {
        
        /// code that identifies the item to be sold or service to be charged
        public DataColumn ColumnArArticleCode;
        
        /// date from which this price is valid
        public DataColumn ColumnArDateValidFrom;
        
        /// the value of the item in base currency
        public DataColumn ColumnArAmount;
        
        /// the currency in which the price is given
        public DataColumn ColumnCurrencyCode;
        
        /// The date the record was created.
        public DataColumn ColumnDateCreated;
        
        /// User ID of who created this record.
        public DataColumn ColumnCreatedBy;
        
        /// The date the record was modified.
        public DataColumn ColumnDateModified;
        
        /// User ID of who last modified this record.
        public DataColumn ColumnModifiedBy;
        
        /// This identifies the current version of the record.
        public DataColumn ColumnModificationId;
        
        /// constructor
        public AArArticlePriceTable() : 
                base("AArArticlePrice")
        {
        }
        
        /// constructor
        public AArArticlePriceTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public AArArticlePriceTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public AArArticlePriceRow this[int i]
        {
            get
            {
                return ((AArArticlePriceRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetArArticleCodeDBName()
        {
            return "a_ar_article_code_c";
        }
        
        /// get help text for column
        public static string GetArArticleCodeHelp()
        {
            return "code that identifies the item to be sold or service to be charged";
        }
        
        /// get label of column
        public static string GetArArticleCodeLabel()
        {
            return "a_ar_article_code_c";
        }
        
        /// get character length for column
        public static short GetArArticleCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDateValidFromDBName()
        {
            return "a_ar_date_valid_from_d";
        }
        
        /// get help text for column
        public static string GetArDateValidFromHelp()
        {
            return "date from which this price is valid";
        }
        
        /// get label of column
        public static string GetArDateValidFromLabel()
        {
            return "a_ar_date_valid_from_d";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArAmountDBName()
        {
            return "a_ar_amount_n";
        }
        
        /// get help text for column
        public static string GetArAmountHelp()
        {
            return "the value of the item in base currency";
        }
        
        /// get label of column
        public static string GetArAmountLabel()
        {
            return "Amount in Base Currency";
        }
        
        /// get display format for column
        public static short GetArAmountLength()
        {
            return 19;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCurrencyCodeDBName()
        {
            return "a_currency_code_c";
        }
        
        /// get help text for column
        public static string GetCurrencyCodeHelp()
        {
            return "the currency in which the price is given";
        }
        
        /// get label of column
        public static string GetCurrencyCodeLabel()
        {
            return "a_currency_code_c";
        }
        
        /// get character length for column
        public static short GetCurrencyCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }
        
        /// get help text for column
        public static string GetDateCreatedHelp()
        {
            return "The date the record was created.";
        }
        
        /// get label of column
        public static string GetDateCreatedLabel()
        {
            return "Created Date";
        }
        
        /// get display format for column
        public static short GetDateCreatedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }
        
        /// get help text for column
        public static string GetCreatedByHelp()
        {
            return "User ID of who created this record.";
        }
        
        /// get label of column
        public static string GetCreatedByLabel()
        {
            return "Created By";
        }
        
        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }
        
        /// get help text for column
        public static string GetDateModifiedHelp()
        {
            return "The date the record was modified.";
        }
        
        /// get label of column
        public static string GetDateModifiedLabel()
        {
            return "Modified Date";
        }
        
        /// get display format for column
        public static short GetDateModifiedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }
        
        /// get help text for column
        public static string GetModifiedByHelp()
        {
            return "User ID of who last modified this record.";
        }
        
        /// get label of column
        public static string GetModifiedByLabel()
        {
            return "Modified By";
        }
        
        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }
        
        /// get help text for column
        public static string GetModificationIdHelp()
        {
            return "This identifies the current version of the record.";
        }
        
        /// get label of column
        public static string GetModificationIdLabel()
        {
            return "";
        }
        
        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "AArArticlePrice";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ar_article_price";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "ArticlePrice";
        }
        
        /// get the index number of fields that are part of the primary key
        public static Int32[] GetPrimKeyColumnOrdList()
        {
            return new int[] {
                    0,
                    1};
        }
        
        /// get the names of the columns
        public static String[] GetColumnStringList()
        {
            return new string[] {
                    "a_ar_article_code_c",
                    "a_ar_date_valid_from_d",
                    "a_ar_amount_n",
                    "a_currency_code_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnArArticleCode = this.Columns["a_ar_article_code_c"];
            this.ColumnArDateValidFrom = this.Columns["a_ar_date_valid_from_d"];
            this.ColumnArAmount = this.Columns["a_ar_amount_n"];
            this.ColumnCurrencyCode = this.Columns["a_currency_code_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnArArticleCode,
                    this.ColumnArDateValidFrom};
        }
        
        /// get typed set of changes
        public AArArticlePriceTable GetChangesTyped()
        {
            return ((AArArticlePriceTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public AArArticlePriceRow NewRowTyped(bool AWithDefaultValues)
        {
            AArArticlePriceRow ret = ((AArArticlePriceRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public AArArticlePriceRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AArArticlePriceRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ar_article_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_date_valid_from_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_currency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnArArticleCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnArDateValidFrom))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnArAmount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnCurrencyCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnDateCreated))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCreatedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateModified))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnModifiedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnModificationId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 150);
            }
            return null;
        }
    }
    
    /// assign a price to an article, which can be updated by time
    [Serializable()]
    public class AArArticlePriceRow : System.Data.DataRow
    {
        
        private AArArticlePriceTable myTable;
        
        /// Constructor
        public AArArticlePriceRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((AArArticlePriceTable)(this.Table));
        }
        
        /// code that identifies the item to be sold or service to be charged
        public String ArArticleCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArArticleCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArArticleCode) 
                            || (((String)(this[this.myTable.ColumnArArticleCode])) != value)))
                {
                    this[this.myTable.ColumnArArticleCode] = value;
                }
            }
        }
        
        /// date from which this price is valid
        public System.DateTime ArDateValidFrom
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDateValidFrom.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDateValidFrom) 
                            || (((System.DateTime)(this[this.myTable.ColumnArDateValidFrom])) != value)))
                {
                    this[this.myTable.ColumnArDateValidFrom] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ArDateValidFromLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDateValidFrom], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ArDateValidFromHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDateValidFrom.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// the value of the item in base currency
        public Double ArAmount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArAmount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Double)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArAmount) 
                            || (((Double)(this[this.myTable.ColumnArAmount])) != value)))
                {
                    this[this.myTable.ColumnArAmount] = value;
                }
            }
        }
        
        /// the currency in which the price is given
        public String CurrencyCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCurrencyCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCurrencyCode) 
                            || (((String)(this[this.myTable.ColumnCurrencyCode])) != value)))
                {
                    this[this.myTable.ColumnCurrencyCode] = value;
                }
            }
        }
        
        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateCreated])) != value)))
                {
                    this[this.myTable.ColumnDateCreated] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateCreatedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateCreatedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCreatedBy) 
                            || (((String)(this[this.myTable.ColumnCreatedBy])) != value)))
                {
                    this[this.myTable.ColumnCreatedBy] = value;
                }
            }
        }
        
        /// The date the record was modified.
        public System.DateTime DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateModified])) != value)))
                {
                    this[this.myTable.ColumnDateModified] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateModifiedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateModifiedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModifiedBy) 
                            || (((String)(this[this.myTable.ColumnModifiedBy])) != value)))
                {
                    this[this.myTable.ColumnModifiedBy] = value;
                }
            }
        }
        
        /// This identifies the current version of the record.
        public String ModificationId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModificationId.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModificationId) 
                            || (((String)(this[this.myTable.ColumnModificationId])) != value)))
                {
                    this[this.myTable.ColumnModificationId] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnArArticleCode);
            this.SetNull(this.myTable.ColumnArDateValidFrom);
            this.SetNull(this.myTable.ColumnArAmount);
            this.SetNull(this.myTable.ColumnCurrencyCode);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsDateCreatedNull()
        {
            return this.IsNull(this.myTable.ColumnDateCreated);
        }
        
        /// assign NULL value
        public void SetDateCreatedNull()
        {
            this.SetNull(this.myTable.ColumnDateCreated);
        }
        
        /// test for NULL value
        public bool IsCreatedByNull()
        {
            return this.IsNull(this.myTable.ColumnCreatedBy);
        }
        
        /// assign NULL value
        public void SetCreatedByNull()
        {
            this.SetNull(this.myTable.ColumnCreatedBy);
        }
        
        /// test for NULL value
        public bool IsDateModifiedNull()
        {
            return this.IsNull(this.myTable.ColumnDateModified);
        }
        
        /// assign NULL value
        public void SetDateModifiedNull()
        {
            this.SetNull(this.myTable.ColumnDateModified);
        }
        
        /// test for NULL value
        public bool IsModifiedByNull()
        {
            return this.IsNull(this.myTable.ColumnModifiedBy);
        }
        
        /// assign NULL value
        public void SetModifiedByNull()
        {
            this.SetNull(this.myTable.ColumnModifiedBy);
        }
        
        /// test for NULL value
        public bool IsModificationIdNull()
        {
            return this.IsNull(this.myTable.ColumnModificationId);
        }
        
        /// assign NULL value
        public void SetModificationIdNull()
        {
            this.SetNull(this.myTable.ColumnModificationId);
        }
    }
    
    /// defines a discount that depends on other conditions or can just be assigned to an invoice or article
    [Serializable()]
    public class AArDiscountTable : TTypedDataTable
    {
        
        /// code that identifies the discount
        public DataColumn ColumnArDiscountCode;
        
        /// date from which this discount is valid
        public DataColumn ColumnArDateValidFrom;
        
        /// this discount has only be created on the fly and should not be reusable elsewhere
        public DataColumn ColumnArAdhoc;
        
        /// flag that prevents this discount from being used, to avoid too long lists in comboboxes etc
        public DataColumn ColumnActive;
        
        /// discount percentage; can be negative for expensive rooms etc
        public DataColumn ColumnArDiscountPercentage;
        
        /// the absolute discount that is substracted from the article price; can be negative as well
        public DataColumn ColumnArDiscountAbsolute;
        
        /// the absolute amount that is charged if this discount applies; e.g. 3 books for 5 Pound
        public DataColumn ColumnArAbsoluteAmount;
        
        /// the currency in which the absolute discount or amount is given
        public DataColumn ColumnCurrencyCode;
        
        /// this discount applies for this number of items that are bought at the same time
        public DataColumn ColumnArNumberOfItems;
        
        /// this discount applies for all of the items if at least this number of items is bought at the same time
        public DataColumn ColumnArMinimumNumberOfItems;
        
        /// this discount applies for this number of nights that the individual or group stays; this is needed because 100 people staying for one night do cost more than 50 people staying for 2 nights
        public DataColumn ColumnArNumberOfNights;
        
        /// this discount applies for all of the nights if the individual or group stays at least for the given amount of nights; this is needed because 100 people staying for one night do cost more than 50 people staying for 2 nights 
        public DataColumn ColumnArMinimumNumberOfNights;
        
        /// this discount applies when a whole room is booked rather than just a bed
        public DataColumn ColumnArWholeRoom;
        
        /// this discount applies for a children (e.g. meals)
        public DataColumn ColumnArChildren;
        
        /// this discount applies when the booking has been done so many days before the stay (using ph_booking.ph_confirmed_d and ph_in_d)
        public DataColumn ColumnArEarlyBookingDays;
        
        /// this discount applies when the payment has been received within the given number of days after the invoice has been charged
        public DataColumn ColumnArEarlyPaymentDays;
        
        /// this discount applies if the article code matches
        public DataColumn ColumnArArticleCode;
        
        /// this discounts applies to partners of this type
        public DataColumn ColumnPartnerTypeCode;
        
        /// The date the record was created.
        public DataColumn ColumnDateCreated;
        
        /// User ID of who created this record.
        public DataColumn ColumnCreatedBy;
        
        /// The date the record was modified.
        public DataColumn ColumnDateModified;
        
        /// User ID of who last modified this record.
        public DataColumn ColumnModifiedBy;
        
        /// This identifies the current version of the record.
        public DataColumn ColumnModificationId;
        
        /// constructor
        public AArDiscountTable() : 
                base("AArDiscount")
        {
        }
        
        /// constructor
        public AArDiscountTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public AArDiscountTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public AArDiscountRow this[int i]
        {
            get
            {
                return ((AArDiscountRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDiscountCodeDBName()
        {
            return "a_ar_discount_code_c";
        }
        
        /// get help text for column
        public static string GetArDiscountCodeHelp()
        {
            return "code that identifies the discount";
        }
        
        /// get label of column
        public static string GetArDiscountCodeLabel()
        {
            return "a_ar_discount_code_c";
        }
        
        /// get character length for column
        public static short GetArDiscountCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDateValidFromDBName()
        {
            return "a_ar_date_valid_from_d";
        }
        
        /// get help text for column
        public static string GetArDateValidFromHelp()
        {
            return "date from which this discount is valid";
        }
        
        /// get label of column
        public static string GetArDateValidFromLabel()
        {
            return "a_ar_date_valid_from_d";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArAdhocDBName()
        {
            return "a_ar_adhoc_l";
        }
        
        /// get help text for column
        public static string GetArAdhocHelp()
        {
            return "this discount has only be created on the fly and should not be reusable elsewhere" +
                "";
        }
        
        /// get label of column
        public static string GetArAdhocLabel()
        {
            return "a_ar_adhoc_l";
        }
        
        /// get the name of the field in the database for this column
        public static string GetActiveDBName()
        {
            return "a_active_l";
        }
        
        /// get help text for column
        public static string GetActiveHelp()
        {
            return "flag that prevents this discount from being used, to avoid too long lists in comb" +
                "oboxes etc";
        }
        
        /// get label of column
        public static string GetActiveLabel()
        {
            return "a_active_l";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDiscountPercentageDBName()
        {
            return "a_ar_discount_percentage_n";
        }
        
        /// get help text for column
        public static string GetArDiscountPercentageHelp()
        {
            return "discount percentage; can be negative for expensive rooms etc";
        }
        
        /// get label of column
        public static string GetArDiscountPercentageLabel()
        {
            return "a_ar_discount_percentage_n";
        }
        
        /// get display format for column
        public static short GetArDiscountPercentageLength()
        {
            return 7;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDiscountAbsoluteDBName()
        {
            return "a_ar_discount_absolute_n";
        }
        
        /// get help text for column
        public static string GetArDiscountAbsoluteHelp()
        {
            return "the absolute discount that is substracted from the article price; can be negative" +
                " as well";
        }
        
        /// get label of column
        public static string GetArDiscountAbsoluteLabel()
        {
            return "a_ar_discount_absolute_n";
        }
        
        /// get display format for column
        public static short GetArDiscountAbsoluteLength()
        {
            return 19;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArAbsoluteAmountDBName()
        {
            return "a_ar_absolute_amount_n";
        }
        
        /// get help text for column
        public static string GetArAbsoluteAmountHelp()
        {
            return "the absolute amount that is charged if this discount applies; e.g. 3 books for 5 " +
                "Pound";
        }
        
        /// get label of column
        public static string GetArAbsoluteAmountLabel()
        {
            return "a_ar_absolute_amount_n";
        }
        
        /// get display format for column
        public static short GetArAbsoluteAmountLength()
        {
            return 18;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCurrencyCodeDBName()
        {
            return "a_currency_code_c";
        }
        
        /// get help text for column
        public static string GetCurrencyCodeHelp()
        {
            return "the currency in which the absolute discount or amount is given";
        }
        
        /// get label of column
        public static string GetCurrencyCodeLabel()
        {
            return "a_currency_code_c";
        }
        
        /// get character length for column
        public static short GetCurrencyCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArNumberOfItemsDBName()
        {
            return "a_ar_number_of_items_i";
        }
        
        /// get help text for column
        public static string GetArNumberOfItemsHelp()
        {
            return "this discount applies for this number of items that are bought at the same time";
        }
        
        /// get label of column
        public static string GetArNumberOfItemsLabel()
        {
            return "a_ar_number_of_items_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArMinimumNumberOfItemsDBName()
        {
            return "a_ar_minimum_number_of_items_i";
        }
        
        /// get help text for column
        public static string GetArMinimumNumberOfItemsHelp()
        {
            return "this discount applies for all of the items if at least this number of items is bo" +
                "ught at the same time";
        }
        
        /// get label of column
        public static string GetArMinimumNumberOfItemsLabel()
        {
            return "a_ar_minimum_number_of_items_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArNumberOfNightsDBName()
        {
            return "a_ar_number_of_nights_i";
        }
        
        /// get help text for column
        public static string GetArNumberOfNightsHelp()
        {
            return "this discount applies for this number of nights that the individual or group stay" +
                "s; this is needed because 100 people staying for one night do cost more than 50 " +
                "people staying for 2 nights";
        }
        
        /// get label of column
        public static string GetArNumberOfNightsLabel()
        {
            return "a_ar_number_of_nights_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArMinimumNumberOfNightsDBName()
        {
            return "a_ar_minimum_number_of_nights_i";
        }
        
        /// get help text for column
        public static string GetArMinimumNumberOfNightsHelp()
        {
            return "this discount applies for all of the nights if the individual or group stays at l" +
                "east for the given amount of nights; this is needed because 100 people staying f" +
                "or one night do cost more than 50 people staying for 2 nights ";
        }
        
        /// get label of column
        public static string GetArMinimumNumberOfNightsLabel()
        {
            return "a_ar_minimum_number_of_nights_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArWholeRoomDBName()
        {
            return "a_ar_whole_room_l";
        }
        
        /// get help text for column
        public static string GetArWholeRoomHelp()
        {
            return "this discount applies when a whole room is booked rather than just a bed";
        }
        
        /// get label of column
        public static string GetArWholeRoomLabel()
        {
            return "a_ar_whole_room_l";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArChildrenDBName()
        {
            return "a_ar_children_l";
        }
        
        /// get help text for column
        public static string GetArChildrenHelp()
        {
            return "this discount applies for a children (e.g. meals)";
        }
        
        /// get label of column
        public static string GetArChildrenLabel()
        {
            return "a_ar_children_l";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArEarlyBookingDaysDBName()
        {
            return "a_ar_early_booking_days_i";
        }
        
        /// get help text for column
        public static string GetArEarlyBookingDaysHelp()
        {
            return "this discount applies when the booking has been done so many days before the stay" +
                " (using ph_booking.ph_confirmed_d and ph_in_d)";
        }
        
        /// get label of column
        public static string GetArEarlyBookingDaysLabel()
        {
            return "a_ar_early_booking_days_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArEarlyPaymentDaysDBName()
        {
            return "a_ar_early_payment_days_i";
        }
        
        /// get help text for column
        public static string GetArEarlyPaymentDaysHelp()
        {
            return "this discount applies when the payment has been received within the given number " +
                "of days after the invoice has been charged";
        }
        
        /// get label of column
        public static string GetArEarlyPaymentDaysLabel()
        {
            return "a_ar_early_payment_days_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArArticleCodeDBName()
        {
            return "a_ar_article_code_c";
        }
        
        /// get help text for column
        public static string GetArArticleCodeHelp()
        {
            return "this discount applies if the article code matches";
        }
        
        /// get label of column
        public static string GetArArticleCodeLabel()
        {
            return "a_ar_article_code_c";
        }
        
        /// get character length for column
        public static short GetArArticleCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerTypeCodeDBName()
        {
            return "p_partner_type_code_c";
        }
        
        /// get help text for column
        public static string GetPartnerTypeCodeHelp()
        {
            return "this discounts applies to partners of this type";
        }
        
        /// get label of column
        public static string GetPartnerTypeCodeLabel()
        {
            return "p_partner_type_code_c";
        }
        
        /// get character length for column
        public static short GetPartnerTypeCodeLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }
        
        /// get help text for column
        public static string GetDateCreatedHelp()
        {
            return "The date the record was created.";
        }
        
        /// get label of column
        public static string GetDateCreatedLabel()
        {
            return "Created Date";
        }
        
        /// get display format for column
        public static short GetDateCreatedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }
        
        /// get help text for column
        public static string GetCreatedByHelp()
        {
            return "User ID of who created this record.";
        }
        
        /// get label of column
        public static string GetCreatedByLabel()
        {
            return "Created By";
        }
        
        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }
        
        /// get help text for column
        public static string GetDateModifiedHelp()
        {
            return "The date the record was modified.";
        }
        
        /// get label of column
        public static string GetDateModifiedLabel()
        {
            return "Modified Date";
        }
        
        /// get display format for column
        public static short GetDateModifiedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }
        
        /// get help text for column
        public static string GetModifiedByHelp()
        {
            return "User ID of who last modified this record.";
        }
        
        /// get label of column
        public static string GetModifiedByLabel()
        {
            return "Modified By";
        }
        
        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }
        
        /// get help text for column
        public static string GetModificationIdHelp()
        {
            return "This identifies the current version of the record.";
        }
        
        /// get label of column
        public static string GetModificationIdLabel()
        {
            return "";
        }
        
        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "AArDiscount";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ar_discount";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Discount";
        }
        
        /// get the index number of fields that are part of the primary key
        public static Int32[] GetPrimKeyColumnOrdList()
        {
            return new int[] {
                    0,
                    1};
        }
        
        /// get the names of the columns
        public static String[] GetColumnStringList()
        {
            return new string[] {
                    "a_ar_discount_code_c",
                    "a_ar_date_valid_from_d",
                    "a_ar_adhoc_l",
                    "a_active_l",
                    "a_ar_discount_percentage_n",
                    "a_ar_discount_absolute_n",
                    "a_ar_absolute_amount_n",
                    "a_currency_code_c",
                    "a_ar_number_of_items_i",
                    "a_ar_minimum_number_of_items_i",
                    "a_ar_number_of_nights_i",
                    "a_ar_minimum_number_of_nights_i",
                    "a_ar_whole_room_l",
                    "a_ar_children_l",
                    "a_ar_early_booking_days_i",
                    "a_ar_early_payment_days_i",
                    "a_ar_article_code_c",
                    "p_partner_type_code_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnArDiscountCode = this.Columns["a_ar_discount_code_c"];
            this.ColumnArDateValidFrom = this.Columns["a_ar_date_valid_from_d"];
            this.ColumnArAdhoc = this.Columns["a_ar_adhoc_l"];
            this.ColumnActive = this.Columns["a_active_l"];
            this.ColumnArDiscountPercentage = this.Columns["a_ar_discount_percentage_n"];
            this.ColumnArDiscountAbsolute = this.Columns["a_ar_discount_absolute_n"];
            this.ColumnArAbsoluteAmount = this.Columns["a_ar_absolute_amount_n"];
            this.ColumnCurrencyCode = this.Columns["a_currency_code_c"];
            this.ColumnArNumberOfItems = this.Columns["a_ar_number_of_items_i"];
            this.ColumnArMinimumNumberOfItems = this.Columns["a_ar_minimum_number_of_items_i"];
            this.ColumnArNumberOfNights = this.Columns["a_ar_number_of_nights_i"];
            this.ColumnArMinimumNumberOfNights = this.Columns["a_ar_minimum_number_of_nights_i"];
            this.ColumnArWholeRoom = this.Columns["a_ar_whole_room_l"];
            this.ColumnArChildren = this.Columns["a_ar_children_l"];
            this.ColumnArEarlyBookingDays = this.Columns["a_ar_early_booking_days_i"];
            this.ColumnArEarlyPaymentDays = this.Columns["a_ar_early_payment_days_i"];
            this.ColumnArArticleCode = this.Columns["a_ar_article_code_c"];
            this.ColumnPartnerTypeCode = this.Columns["p_partner_type_code_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnArDiscountCode,
                    this.ColumnArDateValidFrom};
        }
        
        /// get typed set of changes
        public AArDiscountTable GetChangesTyped()
        {
            return ((AArDiscountTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public AArDiscountRow NewRowTyped(bool AWithDefaultValues)
        {
            AArDiscountRow ret = ((AArDiscountRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public AArDiscountRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AArDiscountRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ar_discount_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_date_valid_from_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_adhoc_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_active_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_discount_percentage_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_discount_absolute_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_absolute_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_currency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_number_of_items_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_minimum_number_of_items_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_number_of_nights_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_minimum_number_of_nights_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_whole_room_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_children_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_early_booking_days_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_early_payment_days_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_article_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnArDiscountCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnArDateValidFrom))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnArAdhoc))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnActive))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnArDiscountPercentage))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 5);
            }
            if ((ACol == ColumnArDiscountAbsolute))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnArAbsoluteAmount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnCurrencyCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnArNumberOfItems))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnArMinimumNumberOfItems))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnArNumberOfNights))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnArMinimumNumberOfNights))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnArWholeRoom))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnArChildren))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnArEarlyBookingDays))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnArEarlyPaymentDays))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnArArticleCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnPartnerTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateCreated))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCreatedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateModified))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnModifiedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnModificationId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 150);
            }
            return null;
        }
    }
    
    /// defines a discount that depends on other conditions or can just be assigned to an invoice or article
    [Serializable()]
    public class AArDiscountRow : System.Data.DataRow
    {
        
        private AArDiscountTable myTable;
        
        /// Constructor
        public AArDiscountRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((AArDiscountTable)(this.Table));
        }
        
        /// code that identifies the discount
        public String ArDiscountCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDiscountCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDiscountCode) 
                            || (((String)(this[this.myTable.ColumnArDiscountCode])) != value)))
                {
                    this[this.myTable.ColumnArDiscountCode] = value;
                }
            }
        }
        
        /// date from which this discount is valid
        public System.DateTime ArDateValidFrom
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDateValidFrom.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDateValidFrom) 
                            || (((System.DateTime)(this[this.myTable.ColumnArDateValidFrom])) != value)))
                {
                    this[this.myTable.ColumnArDateValidFrom] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ArDateValidFromLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDateValidFrom], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ArDateValidFromHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDateValidFrom.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// this discount has only be created on the fly and should not be reusable elsewhere
        public Boolean ArAdhoc
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArAdhoc.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnArAdhoc) 
                            || (((Boolean)(this[this.myTable.ColumnArAdhoc])) != value)))
                {
                    this[this.myTable.ColumnArAdhoc] = value;
                }
            }
        }
        
        /// flag that prevents this discount from being used, to avoid too long lists in comboboxes etc
        public Boolean Active
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnActive.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnActive) 
                            || (((Boolean)(this[this.myTable.ColumnActive])) != value)))
                {
                    this[this.myTable.ColumnActive] = value;
                }
            }
        }
        
        /// discount percentage; can be negative for expensive rooms etc
        public Decimal ArDiscountPercentage
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDiscountPercentage.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Decimal)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDiscountPercentage) 
                            || (((Decimal)(this[this.myTable.ColumnArDiscountPercentage])) != value)))
                {
                    this[this.myTable.ColumnArDiscountPercentage] = value;
                }
            }
        }
        
        /// the absolute discount that is substracted from the article price; can be negative as well
        public Double ArDiscountAbsolute
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDiscountAbsolute.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Double)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDiscountAbsolute) 
                            || (((Double)(this[this.myTable.ColumnArDiscountAbsolute])) != value)))
                {
                    this[this.myTable.ColumnArDiscountAbsolute] = value;
                }
            }
        }
        
        /// the absolute amount that is charged if this discount applies; e.g. 3 books for 5 Pound
        public Double ArAbsoluteAmount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArAbsoluteAmount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Double)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArAbsoluteAmount) 
                            || (((Double)(this[this.myTable.ColumnArAbsoluteAmount])) != value)))
                {
                    this[this.myTable.ColumnArAbsoluteAmount] = value;
                }
            }
        }
        
        /// the currency in which the absolute discount or amount is given
        public String CurrencyCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCurrencyCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCurrencyCode) 
                            || (((String)(this[this.myTable.ColumnCurrencyCode])) != value)))
                {
                    this[this.myTable.ColumnCurrencyCode] = value;
                }
            }
        }
        
        /// this discount applies for this number of items that are bought at the same time
        public Int32 ArNumberOfItems
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArNumberOfItems.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnArNumberOfItems) 
                            || (((Int32)(this[this.myTable.ColumnArNumberOfItems])) != value)))
                {
                    this[this.myTable.ColumnArNumberOfItems] = value;
                }
            }
        }
        
        /// this discount applies for all of the items if at least this number of items is bought at the same time
        public Int32 ArMinimumNumberOfItems
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArMinimumNumberOfItems.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnArMinimumNumberOfItems) 
                            || (((Int32)(this[this.myTable.ColumnArMinimumNumberOfItems])) != value)))
                {
                    this[this.myTable.ColumnArMinimumNumberOfItems] = value;
                }
            }
        }
        
        /// this discount applies for this number of nights that the individual or group stays; this is needed because 100 people staying for one night do cost more than 50 people staying for 2 nights
        public Int32 ArNumberOfNights
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArNumberOfNights.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnArNumberOfNights) 
                            || (((Int32)(this[this.myTable.ColumnArNumberOfNights])) != value)))
                {
                    this[this.myTable.ColumnArNumberOfNights] = value;
                }
            }
        }
        
        /// this discount applies for all of the nights if the individual or group stays at least for the given amount of nights; this is needed because 100 people staying for one night do cost more than 50 people staying for 2 nights 
        public Int32 ArMinimumNumberOfNights
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArMinimumNumberOfNights.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnArMinimumNumberOfNights) 
                            || (((Int32)(this[this.myTable.ColumnArMinimumNumberOfNights])) != value)))
                {
                    this[this.myTable.ColumnArMinimumNumberOfNights] = value;
                }
            }
        }
        
        /// this discount applies when a whole room is booked rather than just a bed
        public Boolean ArWholeRoom
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArWholeRoom.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnArWholeRoom) 
                            || (((Boolean)(this[this.myTable.ColumnArWholeRoom])) != value)))
                {
                    this[this.myTable.ColumnArWholeRoom] = value;
                }
            }
        }
        
        /// this discount applies for a children (e.g. meals)
        public Boolean ArChildren
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArChildren.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnArChildren) 
                            || (((Boolean)(this[this.myTable.ColumnArChildren])) != value)))
                {
                    this[this.myTable.ColumnArChildren] = value;
                }
            }
        }
        
        /// this discount applies when the booking has been done so many days before the stay (using ph_booking.ph_confirmed_d and ph_in_d)
        public Int32 ArEarlyBookingDays
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArEarlyBookingDays.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnArEarlyBookingDays) 
                            || (((Int32)(this[this.myTable.ColumnArEarlyBookingDays])) != value)))
                {
                    this[this.myTable.ColumnArEarlyBookingDays] = value;
                }
            }
        }
        
        /// this discount applies when the payment has been received within the given number of days after the invoice has been charged
        public Int32 ArEarlyPaymentDays
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArEarlyPaymentDays.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnArEarlyPaymentDays) 
                            || (((Int32)(this[this.myTable.ColumnArEarlyPaymentDays])) != value)))
                {
                    this[this.myTable.ColumnArEarlyPaymentDays] = value;
                }
            }
        }
        
        /// this discount applies if the article code matches
        public String ArArticleCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArArticleCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArArticleCode) 
                            || (((String)(this[this.myTable.ColumnArArticleCode])) != value)))
                {
                    this[this.myTable.ColumnArArticleCode] = value;
                }
            }
        }
        
        /// this discounts applies to partners of this type
        public String PartnerTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartnerTypeCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPartnerTypeCode) 
                            || (((String)(this[this.myTable.ColumnPartnerTypeCode])) != value)))
                {
                    this[this.myTable.ColumnPartnerTypeCode] = value;
                }
            }
        }
        
        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateCreated])) != value)))
                {
                    this[this.myTable.ColumnDateCreated] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateCreatedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateCreatedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCreatedBy) 
                            || (((String)(this[this.myTable.ColumnCreatedBy])) != value)))
                {
                    this[this.myTable.ColumnCreatedBy] = value;
                }
            }
        }
        
        /// The date the record was modified.
        public System.DateTime DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateModified])) != value)))
                {
                    this[this.myTable.ColumnDateModified] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateModifiedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateModifiedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModifiedBy) 
                            || (((String)(this[this.myTable.ColumnModifiedBy])) != value)))
                {
                    this[this.myTable.ColumnModifiedBy] = value;
                }
            }
        }
        
        /// This identifies the current version of the record.
        public String ModificationId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModificationId.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModificationId) 
                            || (((String)(this[this.myTable.ColumnModificationId])) != value)))
                {
                    this[this.myTable.ColumnModificationId] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnArDiscountCode);
            this.SetNull(this.myTable.ColumnArDateValidFrom);
            this[this.myTable.ColumnArAdhoc.Ordinal] = true;
            this[this.myTable.ColumnActive.Ordinal] = true;
            this[this.myTable.ColumnArDiscountPercentage.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnArDiscountAbsolute);
            this.SetNull(this.myTable.ColumnArAbsoluteAmount);
            this.SetNull(this.myTable.ColumnCurrencyCode);
            this.SetNull(this.myTable.ColumnArNumberOfItems);
            this.SetNull(this.myTable.ColumnArMinimumNumberOfItems);
            this.SetNull(this.myTable.ColumnArNumberOfNights);
            this.SetNull(this.myTable.ColumnArMinimumNumberOfNights);
            this.SetNull(this.myTable.ColumnArWholeRoom);
            this.SetNull(this.myTable.ColumnArChildren);
            this.SetNull(this.myTable.ColumnArEarlyBookingDays);
            this.SetNull(this.myTable.ColumnArEarlyPaymentDays);
            this.SetNull(this.myTable.ColumnArArticleCode);
            this.SetNull(this.myTable.ColumnPartnerTypeCode);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsArAdhocNull()
        {
            return this.IsNull(this.myTable.ColumnArAdhoc);
        }
        
        /// assign NULL value
        public void SetArAdhocNull()
        {
            this.SetNull(this.myTable.ColumnArAdhoc);
        }
        
        /// test for NULL value
        public bool IsActiveNull()
        {
            return this.IsNull(this.myTable.ColumnActive);
        }
        
        /// assign NULL value
        public void SetActiveNull()
        {
            this.SetNull(this.myTable.ColumnActive);
        }
        
        /// test for NULL value
        public bool IsArDiscountPercentageNull()
        {
            return this.IsNull(this.myTable.ColumnArDiscountPercentage);
        }
        
        /// assign NULL value
        public void SetArDiscountPercentageNull()
        {
            this.SetNull(this.myTable.ColumnArDiscountPercentage);
        }
        
        /// test for NULL value
        public bool IsArDiscountAbsoluteNull()
        {
            return this.IsNull(this.myTable.ColumnArDiscountAbsolute);
        }
        
        /// assign NULL value
        public void SetArDiscountAbsoluteNull()
        {
            this.SetNull(this.myTable.ColumnArDiscountAbsolute);
        }
        
        /// test for NULL value
        public bool IsArAbsoluteAmountNull()
        {
            return this.IsNull(this.myTable.ColumnArAbsoluteAmount);
        }
        
        /// assign NULL value
        public void SetArAbsoluteAmountNull()
        {
            this.SetNull(this.myTable.ColumnArAbsoluteAmount);
        }
        
        /// test for NULL value
        public bool IsArNumberOfItemsNull()
        {
            return this.IsNull(this.myTable.ColumnArNumberOfItems);
        }
        
        /// assign NULL value
        public void SetArNumberOfItemsNull()
        {
            this.SetNull(this.myTable.ColumnArNumberOfItems);
        }
        
        /// test for NULL value
        public bool IsArMinimumNumberOfItemsNull()
        {
            return this.IsNull(this.myTable.ColumnArMinimumNumberOfItems);
        }
        
        /// assign NULL value
        public void SetArMinimumNumberOfItemsNull()
        {
            this.SetNull(this.myTable.ColumnArMinimumNumberOfItems);
        }
        
        /// test for NULL value
        public bool IsArNumberOfNightsNull()
        {
            return this.IsNull(this.myTable.ColumnArNumberOfNights);
        }
        
        /// assign NULL value
        public void SetArNumberOfNightsNull()
        {
            this.SetNull(this.myTable.ColumnArNumberOfNights);
        }
        
        /// test for NULL value
        public bool IsArMinimumNumberOfNightsNull()
        {
            return this.IsNull(this.myTable.ColumnArMinimumNumberOfNights);
        }
        
        /// assign NULL value
        public void SetArMinimumNumberOfNightsNull()
        {
            this.SetNull(this.myTable.ColumnArMinimumNumberOfNights);
        }
        
        /// test for NULL value
        public bool IsArWholeRoomNull()
        {
            return this.IsNull(this.myTable.ColumnArWholeRoom);
        }
        
        /// assign NULL value
        public void SetArWholeRoomNull()
        {
            this.SetNull(this.myTable.ColumnArWholeRoom);
        }
        
        /// test for NULL value
        public bool IsArChildrenNull()
        {
            return this.IsNull(this.myTable.ColumnArChildren);
        }
        
        /// assign NULL value
        public void SetArChildrenNull()
        {
            this.SetNull(this.myTable.ColumnArChildren);
        }
        
        /// test for NULL value
        public bool IsArEarlyBookingDaysNull()
        {
            return this.IsNull(this.myTable.ColumnArEarlyBookingDays);
        }
        
        /// assign NULL value
        public void SetArEarlyBookingDaysNull()
        {
            this.SetNull(this.myTable.ColumnArEarlyBookingDays);
        }
        
        /// test for NULL value
        public bool IsArEarlyPaymentDaysNull()
        {
            return this.IsNull(this.myTable.ColumnArEarlyPaymentDays);
        }
        
        /// assign NULL value
        public void SetArEarlyPaymentDaysNull()
        {
            this.SetNull(this.myTable.ColumnArEarlyPaymentDays);
        }
        
        /// test for NULL value
        public bool IsArArticleCodeNull()
        {
            return this.IsNull(this.myTable.ColumnArArticleCode);
        }
        
        /// assign NULL value
        public void SetArArticleCodeNull()
        {
            this.SetNull(this.myTable.ColumnArArticleCode);
        }
        
        /// test for NULL value
        public bool IsPartnerTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerTypeCode);
        }
        
        /// assign NULL value
        public void SetPartnerTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnPartnerTypeCode);
        }
        
        /// test for NULL value
        public bool IsDateCreatedNull()
        {
            return this.IsNull(this.myTable.ColumnDateCreated);
        }
        
        /// assign NULL value
        public void SetDateCreatedNull()
        {
            this.SetNull(this.myTable.ColumnDateCreated);
        }
        
        /// test for NULL value
        public bool IsCreatedByNull()
        {
            return this.IsNull(this.myTable.ColumnCreatedBy);
        }
        
        /// assign NULL value
        public void SetCreatedByNull()
        {
            this.SetNull(this.myTable.ColumnCreatedBy);
        }
        
        /// test for NULL value
        public bool IsDateModifiedNull()
        {
            return this.IsNull(this.myTable.ColumnDateModified);
        }
        
        /// assign NULL value
        public void SetDateModifiedNull()
        {
            this.SetNull(this.myTable.ColumnDateModified);
        }
        
        /// test for NULL value
        public bool IsModifiedByNull()
        {
            return this.IsNull(this.myTable.ColumnModifiedBy);
        }
        
        /// assign NULL value
        public void SetModifiedByNull()
        {
            this.SetNull(this.myTable.ColumnModifiedBy);
        }
        
        /// test for NULL value
        public bool IsModificationIdNull()
        {
            return this.IsNull(this.myTable.ColumnModificationId);
        }
        
        /// assign NULL value
        public void SetModificationIdNull()
        {
            this.SetNull(this.myTable.ColumnModificationId);
        }
    }
    
    /// defines which discount applies to which category to limit the options in the UI
    [Serializable()]
    public class AArDiscountPerCategoryTable : TTypedDataTable
    {
        
        /// refers to a certain category (catering, hospitality, store, fees)
        public DataColumn ColumnArCategoryCode;
        
        /// code that identifies the discount
        public DataColumn ColumnArDiscountCode;
        
        /// date is only required for foreign key; this applies to all of that discount code; therefore the primary key of this table does not include the date
        public DataColumn ColumnArDateValidFrom;
        
        /// The date the record was created.
        public DataColumn ColumnDateCreated;
        
        /// User ID of who created this record.
        public DataColumn ColumnCreatedBy;
        
        /// The date the record was modified.
        public DataColumn ColumnDateModified;
        
        /// User ID of who last modified this record.
        public DataColumn ColumnModifiedBy;
        
        /// This identifies the current version of the record.
        public DataColumn ColumnModificationId;
        
        /// constructor
        public AArDiscountPerCategoryTable() : 
                base("AArDiscountPerCategory")
        {
        }
        
        /// constructor
        public AArDiscountPerCategoryTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public AArDiscountPerCategoryTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public AArDiscountPerCategoryRow this[int i]
        {
            get
            {
                return ((AArDiscountPerCategoryRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetArCategoryCodeDBName()
        {
            return "a_ar_category_code_c";
        }
        
        /// get help text for column
        public static string GetArCategoryCodeHelp()
        {
            return "refers to a certain category (catering, hospitality, store, fees)";
        }
        
        /// get label of column
        public static string GetArCategoryCodeLabel()
        {
            return "a_ar_category_code_c";
        }
        
        /// get character length for column
        public static short GetArCategoryCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDiscountCodeDBName()
        {
            return "a_ar_discount_code_c";
        }
        
        /// get help text for column
        public static string GetArDiscountCodeHelp()
        {
            return "code that identifies the discount";
        }
        
        /// get label of column
        public static string GetArDiscountCodeLabel()
        {
            return "a_ar_discount_code_c";
        }
        
        /// get character length for column
        public static short GetArDiscountCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDateValidFromDBName()
        {
            return "a_ar_date_valid_from_d";
        }
        
        /// get help text for column
        public static string GetArDateValidFromHelp()
        {
            return "date is only required for foreign key; this applies to all of that discount code;" +
                " therefore the primary key of this table does not include the date";
        }
        
        /// get label of column
        public static string GetArDateValidFromLabel()
        {
            return "a_ar_date_valid_from_d";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }
        
        /// get help text for column
        public static string GetDateCreatedHelp()
        {
            return "The date the record was created.";
        }
        
        /// get label of column
        public static string GetDateCreatedLabel()
        {
            return "Created Date";
        }
        
        /// get display format for column
        public static short GetDateCreatedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }
        
        /// get help text for column
        public static string GetCreatedByHelp()
        {
            return "User ID of who created this record.";
        }
        
        /// get label of column
        public static string GetCreatedByLabel()
        {
            return "Created By";
        }
        
        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }
        
        /// get help text for column
        public static string GetDateModifiedHelp()
        {
            return "The date the record was modified.";
        }
        
        /// get label of column
        public static string GetDateModifiedLabel()
        {
            return "Modified Date";
        }
        
        /// get display format for column
        public static short GetDateModifiedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }
        
        /// get help text for column
        public static string GetModifiedByHelp()
        {
            return "User ID of who last modified this record.";
        }
        
        /// get label of column
        public static string GetModifiedByLabel()
        {
            return "Modified By";
        }
        
        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }
        
        /// get help text for column
        public static string GetModificationIdHelp()
        {
            return "This identifies the current version of the record.";
        }
        
        /// get label of column
        public static string GetModificationIdLabel()
        {
            return "";
        }
        
        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "AArDiscountPerCategory";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ar_discount_per_category";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Available Discount per Category";
        }
        
        /// get the index number of fields that are part of the primary key
        public static Int32[] GetPrimKeyColumnOrdList()
        {
            return new int[] {
                    0,
                    1};
        }
        
        /// get the names of the columns
        public static String[] GetColumnStringList()
        {
            return new string[] {
                    "a_ar_category_code_c",
                    "a_ar_discount_code_c",
                    "a_ar_date_valid_from_d",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnArCategoryCode = this.Columns["a_ar_category_code_c"];
            this.ColumnArDiscountCode = this.Columns["a_ar_discount_code_c"];
            this.ColumnArDateValidFrom = this.Columns["a_ar_date_valid_from_d"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnArCategoryCode,
                    this.ColumnArDiscountCode};
        }
        
        /// get typed set of changes
        public AArDiscountPerCategoryTable GetChangesTyped()
        {
            return ((AArDiscountPerCategoryTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public AArDiscountPerCategoryRow NewRowTyped(bool AWithDefaultValues)
        {
            AArDiscountPerCategoryRow ret = ((AArDiscountPerCategoryRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public AArDiscountPerCategoryRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AArDiscountPerCategoryRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ar_category_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_discount_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_date_valid_from_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnArCategoryCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnArDiscountCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnArDateValidFrom))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnDateCreated))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCreatedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateModified))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnModifiedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnModificationId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 150);
            }
            return null;
        }
    }
    
    /// defines which discount applies to which category to limit the options in the UI
    [Serializable()]
    public class AArDiscountPerCategoryRow : System.Data.DataRow
    {
        
        private AArDiscountPerCategoryTable myTable;
        
        /// Constructor
        public AArDiscountPerCategoryRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((AArDiscountPerCategoryTable)(this.Table));
        }
        
        /// refers to a certain category (catering, hospitality, store, fees)
        public String ArCategoryCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArCategoryCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArCategoryCode) 
                            || (((String)(this[this.myTable.ColumnArCategoryCode])) != value)))
                {
                    this[this.myTable.ColumnArCategoryCode] = value;
                }
            }
        }
        
        /// code that identifies the discount
        public String ArDiscountCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDiscountCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDiscountCode) 
                            || (((String)(this[this.myTable.ColumnArDiscountCode])) != value)))
                {
                    this[this.myTable.ColumnArDiscountCode] = value;
                }
            }
        }
        
        /// date is only required for foreign key; this applies to all of that discount code; therefore the primary key of this table does not include the date
        public System.DateTime ArDateValidFrom
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDateValidFrom.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDateValidFrom) 
                            || (((System.DateTime)(this[this.myTable.ColumnArDateValidFrom])) != value)))
                {
                    this[this.myTable.ColumnArDateValidFrom] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ArDateValidFromLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDateValidFrom], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ArDateValidFromHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDateValidFrom.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateCreated])) != value)))
                {
                    this[this.myTable.ColumnDateCreated] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateCreatedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateCreatedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCreatedBy) 
                            || (((String)(this[this.myTable.ColumnCreatedBy])) != value)))
                {
                    this[this.myTable.ColumnCreatedBy] = value;
                }
            }
        }
        
        /// The date the record was modified.
        public System.DateTime DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateModified])) != value)))
                {
                    this[this.myTable.ColumnDateModified] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateModifiedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateModifiedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModifiedBy) 
                            || (((String)(this[this.myTable.ColumnModifiedBy])) != value)))
                {
                    this[this.myTable.ColumnModifiedBy] = value;
                }
            }
        }
        
        /// This identifies the current version of the record.
        public String ModificationId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModificationId.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModificationId) 
                            || (((String)(this[this.myTable.ColumnModificationId])) != value)))
                {
                    this[this.myTable.ColumnModificationId] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnArCategoryCode);
            this.SetNull(this.myTable.ColumnArDiscountCode);
            this.SetNull(this.myTable.ColumnArDateValidFrom);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsDateCreatedNull()
        {
            return this.IsNull(this.myTable.ColumnDateCreated);
        }
        
        /// assign NULL value
        public void SetDateCreatedNull()
        {
            this.SetNull(this.myTable.ColumnDateCreated);
        }
        
        /// test for NULL value
        public bool IsCreatedByNull()
        {
            return this.IsNull(this.myTable.ColumnCreatedBy);
        }
        
        /// assign NULL value
        public void SetCreatedByNull()
        {
            this.SetNull(this.myTable.ColumnCreatedBy);
        }
        
        /// test for NULL value
        public bool IsDateModifiedNull()
        {
            return this.IsNull(this.myTable.ColumnDateModified);
        }
        
        /// assign NULL value
        public void SetDateModifiedNull()
        {
            this.SetNull(this.myTable.ColumnDateModified);
        }
        
        /// test for NULL value
        public bool IsModifiedByNull()
        {
            return this.IsNull(this.myTable.ColumnModifiedBy);
        }
        
        /// assign NULL value
        public void SetModifiedByNull()
        {
            this.SetNull(this.myTable.ColumnModifiedBy);
        }
        
        /// test for NULL value
        public bool IsModificationIdNull()
        {
            return this.IsNull(this.myTable.ColumnModificationId);
        }
        
        /// assign NULL value
        public void SetModificationIdNull()
        {
            this.SetNull(this.myTable.ColumnModificationId);
        }
    }
    
    /// defines which discounts should be applied by default during a certain event or time period to articles from a certain category
    [Serializable()]
    public class AArDefaultDiscountTable : TTypedDataTable
    {
        
        /// refers to a certain category (catering, hospitality, store, fees)
        public DataColumn ColumnArCategoryCode;
        
        /// code that identifies the discount
        public DataColumn ColumnArDiscountCode;
        
        /// this clearly specifies which version of the discount is meant
        public DataColumn ColumnArDiscountDateValidFrom;
        
        /// this default discount is only applied during this time period
        public DataColumn ColumnArDateValidFrom;
        
        /// this default discount is only applied during this time period; can be null for ongoing default discounts
        public DataColumn ColumnArDateValidTo;
        
        /// The date the record was created.
        public DataColumn ColumnDateCreated;
        
        /// User ID of who created this record.
        public DataColumn ColumnCreatedBy;
        
        /// The date the record was modified.
        public DataColumn ColumnDateModified;
        
        /// User ID of who last modified this record.
        public DataColumn ColumnModifiedBy;
        
        /// This identifies the current version of the record.
        public DataColumn ColumnModificationId;
        
        /// constructor
        public AArDefaultDiscountTable() : 
                base("AArDefaultDiscount")
        {
        }
        
        /// constructor
        public AArDefaultDiscountTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public AArDefaultDiscountTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public AArDefaultDiscountRow this[int i]
        {
            get
            {
                return ((AArDefaultDiscountRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetArCategoryCodeDBName()
        {
            return "a_ar_category_code_c";
        }
        
        /// get help text for column
        public static string GetArCategoryCodeHelp()
        {
            return "refers to a certain category (catering, hospitality, store, fees)";
        }
        
        /// get label of column
        public static string GetArCategoryCodeLabel()
        {
            return "a_ar_category_code_c";
        }
        
        /// get character length for column
        public static short GetArCategoryCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDiscountCodeDBName()
        {
            return "a_ar_discount_code_c";
        }
        
        /// get help text for column
        public static string GetArDiscountCodeHelp()
        {
            return "code that identifies the discount";
        }
        
        /// get label of column
        public static string GetArDiscountCodeLabel()
        {
            return "a_ar_discount_code_c";
        }
        
        /// get character length for column
        public static short GetArDiscountCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDiscountDateValidFromDBName()
        {
            return "a_ar_discount_date_valid_from_d";
        }
        
        /// get help text for column
        public static string GetArDiscountDateValidFromHelp()
        {
            return "this clearly specifies which version of the discount is meant";
        }
        
        /// get label of column
        public static string GetArDiscountDateValidFromLabel()
        {
            return "a_ar_discount_date_valid_from_d";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDateValidFromDBName()
        {
            return "a_ar_date_valid_from_d";
        }
        
        /// get help text for column
        public static string GetArDateValidFromHelp()
        {
            return "this default discount is only applied during this time period";
        }
        
        /// get label of column
        public static string GetArDateValidFromLabel()
        {
            return "a_ar_date_valid_from_d";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDateValidToDBName()
        {
            return "a_ar_date_valid_to_d";
        }
        
        /// get help text for column
        public static string GetArDateValidToHelp()
        {
            return "this default discount is only applied during this time period; can be null for on" +
                "going default discounts";
        }
        
        /// get label of column
        public static string GetArDateValidToLabel()
        {
            return "a_ar_date_valid_to_d";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }
        
        /// get help text for column
        public static string GetDateCreatedHelp()
        {
            return "The date the record was created.";
        }
        
        /// get label of column
        public static string GetDateCreatedLabel()
        {
            return "Created Date";
        }
        
        /// get display format for column
        public static short GetDateCreatedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }
        
        /// get help text for column
        public static string GetCreatedByHelp()
        {
            return "User ID of who created this record.";
        }
        
        /// get label of column
        public static string GetCreatedByLabel()
        {
            return "Created By";
        }
        
        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }
        
        /// get help text for column
        public static string GetDateModifiedHelp()
        {
            return "The date the record was modified.";
        }
        
        /// get label of column
        public static string GetDateModifiedLabel()
        {
            return "Modified Date";
        }
        
        /// get display format for column
        public static short GetDateModifiedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }
        
        /// get help text for column
        public static string GetModifiedByHelp()
        {
            return "User ID of who last modified this record.";
        }
        
        /// get label of column
        public static string GetModifiedByLabel()
        {
            return "Modified By";
        }
        
        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }
        
        /// get help text for column
        public static string GetModificationIdHelp()
        {
            return "This identifies the current version of the record.";
        }
        
        /// get label of column
        public static string GetModificationIdLabel()
        {
            return "";
        }
        
        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "AArDefaultDiscount";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ar_default_discount";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Default discounts";
        }
        
        /// get the index number of fields that are part of the primary key
        public static Int32[] GetPrimKeyColumnOrdList()
        {
            return new int[] {
                    0,
                    1,
                    3};
        }
        
        /// get the names of the columns
        public static String[] GetColumnStringList()
        {
            return new string[] {
                    "a_ar_category_code_c",
                    "a_ar_discount_code_c",
                    "a_ar_discount_date_valid_from_d",
                    "a_ar_date_valid_from_d",
                    "a_ar_date_valid_to_d",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnArCategoryCode = this.Columns["a_ar_category_code_c"];
            this.ColumnArDiscountCode = this.Columns["a_ar_discount_code_c"];
            this.ColumnArDiscountDateValidFrom = this.Columns["a_ar_discount_date_valid_from_d"];
            this.ColumnArDateValidFrom = this.Columns["a_ar_date_valid_from_d"];
            this.ColumnArDateValidTo = this.Columns["a_ar_date_valid_to_d"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnArCategoryCode,
                    this.ColumnArDiscountCode,
                    this.ColumnArDateValidFrom};
        }
        
        /// get typed set of changes
        public AArDefaultDiscountTable GetChangesTyped()
        {
            return ((AArDefaultDiscountTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public AArDefaultDiscountRow NewRowTyped(bool AWithDefaultValues)
        {
            AArDefaultDiscountRow ret = ((AArDefaultDiscountRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public AArDefaultDiscountRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AArDefaultDiscountRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ar_category_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_discount_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_discount_date_valid_from_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_date_valid_from_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_date_valid_to_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnArCategoryCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnArDiscountCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnArDiscountDateValidFrom))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnArDateValidFrom))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnArDateValidTo))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnDateCreated))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCreatedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateModified))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnModifiedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnModificationId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 150);
            }
            return null;
        }
    }
    
    /// defines which discounts should be applied by default during a certain event or time period to articles from a certain category
    [Serializable()]
    public class AArDefaultDiscountRow : System.Data.DataRow
    {
        
        private AArDefaultDiscountTable myTable;
        
        /// Constructor
        public AArDefaultDiscountRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((AArDefaultDiscountTable)(this.Table));
        }
        
        /// refers to a certain category (catering, hospitality, store, fees)
        public String ArCategoryCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArCategoryCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArCategoryCode) 
                            || (((String)(this[this.myTable.ColumnArCategoryCode])) != value)))
                {
                    this[this.myTable.ColumnArCategoryCode] = value;
                }
            }
        }
        
        /// code that identifies the discount
        public String ArDiscountCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDiscountCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDiscountCode) 
                            || (((String)(this[this.myTable.ColumnArDiscountCode])) != value)))
                {
                    this[this.myTable.ColumnArDiscountCode] = value;
                }
            }
        }
        
        /// this clearly specifies which version of the discount is meant
        public System.DateTime ArDiscountDateValidFrom
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDiscountDateValidFrom.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDiscountDateValidFrom) 
                            || (((System.DateTime)(this[this.myTable.ColumnArDiscountDateValidFrom])) != value)))
                {
                    this[this.myTable.ColumnArDiscountDateValidFrom] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ArDiscountDateValidFromLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDiscountDateValidFrom], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ArDiscountDateValidFromHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDiscountDateValidFrom.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// this default discount is only applied during this time period
        public System.DateTime ArDateValidFrom
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDateValidFrom.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDateValidFrom) 
                            || (((System.DateTime)(this[this.myTable.ColumnArDateValidFrom])) != value)))
                {
                    this[this.myTable.ColumnArDateValidFrom] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ArDateValidFromLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDateValidFrom], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ArDateValidFromHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDateValidFrom.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// this default discount is only applied during this time period; can be null for ongoing default discounts
        public System.DateTime ArDateValidTo
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDateValidTo.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDateValidTo) 
                            || (((System.DateTime)(this[this.myTable.ColumnArDateValidTo])) != value)))
                {
                    this[this.myTable.ColumnArDateValidTo] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ArDateValidToLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDateValidTo], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ArDateValidToHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDateValidTo.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateCreated])) != value)))
                {
                    this[this.myTable.ColumnDateCreated] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateCreatedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateCreatedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCreatedBy) 
                            || (((String)(this[this.myTable.ColumnCreatedBy])) != value)))
                {
                    this[this.myTable.ColumnCreatedBy] = value;
                }
            }
        }
        
        /// The date the record was modified.
        public System.DateTime DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateModified])) != value)))
                {
                    this[this.myTable.ColumnDateModified] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateModifiedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateModifiedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModifiedBy) 
                            || (((String)(this[this.myTable.ColumnModifiedBy])) != value)))
                {
                    this[this.myTable.ColumnModifiedBy] = value;
                }
            }
        }
        
        /// This identifies the current version of the record.
        public String ModificationId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModificationId.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModificationId) 
                            || (((String)(this[this.myTable.ColumnModificationId])) != value)))
                {
                    this[this.myTable.ColumnModificationId] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this.SetNull(this.myTable.ColumnArCategoryCode);
            this.SetNull(this.myTable.ColumnArDiscountCode);
            this.SetNull(this.myTable.ColumnArDiscountDateValidFrom);
            this.SetNull(this.myTable.ColumnArDateValidFrom);
            this.SetNull(this.myTable.ColumnArDateValidTo);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsArDateValidToNull()
        {
            return this.IsNull(this.myTable.ColumnArDateValidTo);
        }
        
        /// assign NULL value
        public void SetArDateValidToNull()
        {
            this.SetNull(this.myTable.ColumnArDateValidTo);
        }
        
        /// test for NULL value
        public bool IsDateCreatedNull()
        {
            return this.IsNull(this.myTable.ColumnDateCreated);
        }
        
        /// assign NULL value
        public void SetDateCreatedNull()
        {
            this.SetNull(this.myTable.ColumnDateCreated);
        }
        
        /// test for NULL value
        public bool IsCreatedByNull()
        {
            return this.IsNull(this.myTable.ColumnCreatedBy);
        }
        
        /// assign NULL value
        public void SetCreatedByNull()
        {
            this.SetNull(this.myTable.ColumnCreatedBy);
        }
        
        /// test for NULL value
        public bool IsDateModifiedNull()
        {
            return this.IsNull(this.myTable.ColumnDateModified);
        }
        
        /// assign NULL value
        public void SetDateModifiedNull()
        {
            this.SetNull(this.myTable.ColumnDateModified);
        }
        
        /// test for NULL value
        public bool IsModifiedByNull()
        {
            return this.IsNull(this.myTable.ColumnModifiedBy);
        }
        
        /// assign NULL value
        public void SetModifiedByNull()
        {
            this.SetNull(this.myTable.ColumnModifiedBy);
        }
        
        /// test for NULL value
        public bool IsModificationIdNull()
        {
            return this.IsNull(this.myTable.ColumnModificationId);
        }
        
        /// assign NULL value
        public void SetModificationIdNull()
        {
            this.SetNull(this.myTable.ColumnModificationId);
        }
    }
    
    /// the invoice (which is also an offer at a certain stage)
    [Serializable()]
    public class AArInvoiceTable : TTypedDataTable
    {
        
        /// This is used as a key field in most of the accounting system files
        public DataColumn ColumnLedgerNumber;
        
        /// Key to uniquely identify invoice
        public DataColumn ColumnKey;
        
        /// an invoice can have these states: OFFER, CHARGED, PARTIALLYPAID, PAID
        public DataColumn ColumnStatus;
        
        /// This is the partner who has to pay the bill; can be null for cash payments; could also be another field
        public DataColumn ColumnPartnerKey;
        
        /// this is the date when the invoice was charged
        public DataColumn ColumnDateEffective;
        
        /// refers to the offer that was created for this invoice; it is basically an archived copy of the invoice, and the invoice might actually be different from the offer (e.g. hospitality: different number of people, etc.); table ph_booking always refers to the invoice, and the invoice refers to the offer; there is no requirement for an offer to exist, it can be null
        public DataColumn ColumnOffer;
        
        /// this defines whether no tax is applied to this invoice (NONE), or if a SPECIAL tax is applied, or if the DEFAULT tax defined for each article; this should work around issues of selling to businesses or customers abroad
        public DataColumn ColumnTaxing;
        
        /// if a_taxing_c has the value SPECIAL, then this tax applies (defined by tax type code, tax rate code, and date valid from
        public DataColumn ColumnSpecialTaxTypeCode;
        
        /// this describes whether it is e.g. the standard, reduced or zero rate of VAT
        public DataColumn ColumnSpecialTaxRateCode;
        
        /// this describes when this particular percentage rate has become valid by law
        public DataColumn ColumnSpecialTaxValidFrom;
        
        /// The total amount of money that this invoice is worth; this includes all discounts, even the early payment discount; if the early payment discount does not apply anymore at the time of payment, this total amount needs to be updated
        public DataColumn ColumnTotalAmount;
        
        /// the currency of the total amount
        public DataColumn ColumnCurrencyCode;
        
        /// The date the record was created.
        public DataColumn ColumnDateCreated;
        
        /// User ID of who created this record.
        public DataColumn ColumnCreatedBy;
        
        /// The date the record was modified.
        public DataColumn ColumnDateModified;
        
        /// User ID of who last modified this record.
        public DataColumn ColumnModifiedBy;
        
        /// This identifies the current version of the record.
        public DataColumn ColumnModificationId;
        
        /// constructor
        public AArInvoiceTable() : 
                base("AArInvoice")
        {
        }
        
        /// constructor
        public AArInvoiceTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public AArInvoiceTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public AArInvoiceRow this[int i]
        {
            get
            {
                return ((AArInvoiceRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }
        
        /// get help text for column
        public static string GetLedgerNumberHelp()
        {
            return "Enter the ledger number";
        }
        
        /// get label of column
        public static string GetLedgerNumberLabel()
        {
            return "Ledger Number";
        }
        
        /// get display format for column
        public static short GetLedgerNumberLength()
        {
            return 4;
        }
        
        /// get the name of the field in the database for this column
        public static string GetKeyDBName()
        {
            return "a_key_i";
        }
        
        /// get help text for column
        public static string GetKeyHelp()
        {
            return "Key to uniquely identify invoice";
        }
        
        /// get label of column
        public static string GetKeyLabel()
        {
            return "Invoice Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetStatusDBName()
        {
            return "a_status_c";
        }
        
        /// get help text for column
        public static string GetStatusHelp()
        {
            return "an invoice can have these states: OFFER, CHARGED, PARTIALLYPAID, PAID";
        }
        
        /// get label of column
        public static string GetStatusLabel()
        {
            return "a_status_c";
        }
        
        /// get character length for column
        public static short GetStatusLength()
        {
            return 16;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }
        
        /// get help text for column
        public static string GetPartnerKeyHelp()
        {
            return "This is the partner who has to pay the bill; can be null for cash payments; could" +
                " also be another field";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "p_partner_key_n";
        }
        
        /// get display format for column
        public static short GetPartnerKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateEffectiveDBName()
        {
            return "a_date_effective_d";
        }
        
        /// get help text for column
        public static string GetDateEffectiveHelp()
        {
            return "this is the date when the invoice was charged";
        }
        
        /// get label of column
        public static string GetDateEffectiveLabel()
        {
            return "a_date_effective_d";
        }
        
        /// get the name of the field in the database for this column
        public static string GetOfferDBName()
        {
            return "a_offer_i";
        }
        
        /// get help text for column
        public static string GetOfferHelp()
        {
            return @"refers to the offer that was created for this invoice; it is basically an archived copy of the invoice, and the invoice might actually be different from the offer (e.g. hospitality: different number of people, etc.); table ph_booking always refers to the invoice, and the invoice refers to the offer; there is no requirement for an offer to exist, it can be null";
        }
        
        /// get label of column
        public static string GetOfferLabel()
        {
            return "a_offer_i";
        }
        
        /// get display format for column
        public static short GetOfferLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTaxingDBName()
        {
            return "a_taxing_c";
        }
        
        /// get help text for column
        public static string GetTaxingHelp()
        {
            return "this defines whether no tax is applied to this invoice (NONE), or if a SPECIAL ta" +
                "x is applied, or if the DEFAULT tax defined for each article; this should work a" +
                "round issues of selling to businesses or customers abroad";
        }
        
        /// get label of column
        public static string GetTaxingLabel()
        {
            return "a_taxing_c";
        }
        
        /// get character length for column
        public static short GetTaxingLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetSpecialTaxTypeCodeDBName()
        {
            return "a_special_tax_type_code_c";
        }
        
        /// get help text for column
        public static string GetSpecialTaxTypeCodeHelp()
        {
            return "Enter a tax type code";
        }
        
        /// get label of column
        public static string GetSpecialTaxTypeCodeLabel()
        {
            return "Tax Type Code";
        }
        
        /// get character length for column
        public static short GetSpecialTaxTypeCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetSpecialTaxRateCodeDBName()
        {
            return "a_special_tax_rate_code_c";
        }
        
        /// get help text for column
        public static string GetSpecialTaxRateCodeHelp()
        {
            return "Enter a tax rate code";
        }
        
        /// get label of column
        public static string GetSpecialTaxRateCodeLabel()
        {
            return "Tax Rate Code";
        }
        
        /// get character length for column
        public static short GetSpecialTaxRateCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetSpecialTaxValidFromDBName()
        {
            return "a_special_tax_valid_from_d";
        }
        
        /// get help text for column
        public static string GetSpecialTaxValidFromHelp()
        {
            return "Enter a tax rate code";
        }
        
        /// get label of column
        public static string GetSpecialTaxValidFromLabel()
        {
            return "Tax Rate Code";
        }
        
        /// get display format for column
        public static short GetSpecialTaxValidFromLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTotalAmountDBName()
        {
            return "a_total_amount_n";
        }
        
        /// get help text for column
        public static string GetTotalAmountHelp()
        {
            return "The total amount of money that this invoice is worth; this includes all discounts" +
                ", even the early payment discount; if the early payment discount does not apply " +
                "anymore at the time of payment, this total amount needs to be updated";
        }
        
        /// get label of column
        public static string GetTotalAmountLabel()
        {
            return "Total Amount";
        }
        
        /// get display format for column
        public static short GetTotalAmountLength()
        {
            return 19;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCurrencyCodeDBName()
        {
            return "a_currency_code_c";
        }
        
        /// get help text for column
        public static string GetCurrencyCodeHelp()
        {
            return "the currency of the total amount";
        }
        
        /// get label of column
        public static string GetCurrencyCodeLabel()
        {
            return "a_currency_code_c";
        }
        
        /// get character length for column
        public static short GetCurrencyCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }
        
        /// get help text for column
        public static string GetDateCreatedHelp()
        {
            return "The date the record was created.";
        }
        
        /// get label of column
        public static string GetDateCreatedLabel()
        {
            return "Created Date";
        }
        
        /// get display format for column
        public static short GetDateCreatedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }
        
        /// get help text for column
        public static string GetCreatedByHelp()
        {
            return "User ID of who created this record.";
        }
        
        /// get label of column
        public static string GetCreatedByLabel()
        {
            return "Created By";
        }
        
        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }
        
        /// get help text for column
        public static string GetDateModifiedHelp()
        {
            return "The date the record was modified.";
        }
        
        /// get label of column
        public static string GetDateModifiedLabel()
        {
            return "Modified Date";
        }
        
        /// get display format for column
        public static short GetDateModifiedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }
        
        /// get help text for column
        public static string GetModifiedByHelp()
        {
            return "User ID of who last modified this record.";
        }
        
        /// get label of column
        public static string GetModifiedByLabel()
        {
            return "Modified By";
        }
        
        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }
        
        /// get help text for column
        public static string GetModificationIdHelp()
        {
            return "This identifies the current version of the record.";
        }
        
        /// get label of column
        public static string GetModificationIdLabel()
        {
            return "";
        }
        
        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "AArInvoice";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ar_invoice";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Invoice";
        }
        
        /// get the index number of fields that are part of the primary key
        public static Int32[] GetPrimKeyColumnOrdList()
        {
            return new int[] {
                    0,
                    1};
        }
        
        /// get the names of the columns
        public static String[] GetColumnStringList()
        {
            return new string[] {
                    "a_ledger_number_i",
                    "a_key_i",
                    "a_status_c",
                    "p_partner_key_n",
                    "a_date_effective_d",
                    "a_offer_i",
                    "a_taxing_c",
                    "a_special_tax_type_code_c",
                    "a_special_tax_rate_code_c",
                    "a_special_tax_valid_from_d",
                    "a_total_amount_n",
                    "a_currency_code_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnKey = this.Columns["a_key_i"];
            this.ColumnStatus = this.Columns["a_status_c"];
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnDateEffective = this.Columns["a_date_effective_d"];
            this.ColumnOffer = this.Columns["a_offer_i"];
            this.ColumnTaxing = this.Columns["a_taxing_c"];
            this.ColumnSpecialTaxTypeCode = this.Columns["a_special_tax_type_code_c"];
            this.ColumnSpecialTaxRateCode = this.Columns["a_special_tax_rate_code_c"];
            this.ColumnSpecialTaxValidFrom = this.Columns["a_special_tax_valid_from_d"];
            this.ColumnTotalAmount = this.Columns["a_total_amount_n"];
            this.ColumnCurrencyCode = this.Columns["a_currency_code_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLedgerNumber,
                    this.ColumnKey};
        }
        
        /// get typed set of changes
        public AArInvoiceTable GetChangesTyped()
        {
            return ((AArInvoiceTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public AArInvoiceRow NewRowTyped(bool AWithDefaultValues)
        {
            AArInvoiceRow ret = ((AArInvoiceRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public AArInvoiceRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AArInvoiceRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_status_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("a_date_effective_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_offer_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_taxing_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_special_tax_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_special_tax_rate_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_special_tax_valid_from_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_total_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_currency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLedgerNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnStatus))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 32);
            }
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnDateEffective))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnOffer))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnTaxing))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnSpecialTaxTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnSpecialTaxRateCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnSpecialTaxValidFrom))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnTotalAmount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnCurrencyCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnDateCreated))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCreatedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateModified))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnModifiedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnModificationId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 150);
            }
            return null;
        }
    }
    
    /// the invoice (which is also an offer at a certain stage)
    [Serializable()]
    public class AArInvoiceRow : System.Data.DataRow
    {
        
        private AArInvoiceTable myTable;
        
        /// Constructor
        public AArInvoiceRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((AArInvoiceTable)(this.Table));
        }
        
        /// This is used as a key field in most of the accounting system files
        public Int32 LedgerNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLedgerNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLedgerNumber) 
                            || (((Int32)(this[this.myTable.ColumnLedgerNumber])) != value)))
                {
                    this[this.myTable.ColumnLedgerNumber] = value;
                }
            }
        }
        
        /// Key to uniquely identify invoice
        public Int32 Key
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnKey) 
                            || (((Int32)(this[this.myTable.ColumnKey])) != value)))
                {
                    this[this.myTable.ColumnKey] = value;
                }
            }
        }
        
        /// an invoice can have these states: OFFER, CHARGED, PARTIALLYPAID, PAID
        public String Status
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnStatus.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnStatus) 
                            || (((String)(this[this.myTable.ColumnStatus])) != value)))
                {
                    this[this.myTable.ColumnStatus] = value;
                }
            }
        }
        
        /// This is the partner who has to pay the bill; can be null for cash payments; could also be another field
        public Int64 PartnerKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPartnerKey.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Int64)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnPartnerKey) 
                            || (((Int64)(this[this.myTable.ColumnPartnerKey])) != value)))
                {
                    this[this.myTable.ColumnPartnerKey] = value;
                }
            }
        }
        
        /// this is the date when the invoice was charged
        public System.DateTime DateEffective
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateEffective.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateEffective) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateEffective])) != value)))
                {
                    this[this.myTable.ColumnDateEffective] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateEffectiveLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateEffective], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateEffectiveHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateEffective.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// refers to the offer that was created for this invoice; it is basically an archived copy of the invoice, and the invoice might actually be different from the offer (e.g. hospitality: different number of people, etc.); table ph_booking always refers to the invoice, and the invoice refers to the offer; there is no requirement for an offer to exist, it can be null
        public Int32 Offer
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOffer.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnOffer) 
                            || (((Int32)(this[this.myTable.ColumnOffer])) != value)))
                {
                    this[this.myTable.ColumnOffer] = value;
                }
            }
        }
        
        /// this defines whether no tax is applied to this invoice (NONE), or if a SPECIAL tax is applied, or if the DEFAULT tax defined for each article; this should work around issues of selling to businesses or customers abroad
        public String Taxing
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxing.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTaxing) 
                            || (((String)(this[this.myTable.ColumnTaxing])) != value)))
                {
                    this[this.myTable.ColumnTaxing] = value;
                }
            }
        }
        
        /// if a_taxing_c has the value SPECIAL, then this tax applies (defined by tax type code, tax rate code, and date valid from
        public String SpecialTaxTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSpecialTaxTypeCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnSpecialTaxTypeCode) 
                            || (((String)(this[this.myTable.ColumnSpecialTaxTypeCode])) != value)))
                {
                    this[this.myTable.ColumnSpecialTaxTypeCode] = value;
                }
            }
        }
        
        /// this describes whether it is e.g. the standard, reduced or zero rate of VAT
        public String SpecialTaxRateCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSpecialTaxRateCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnSpecialTaxRateCode) 
                            || (((String)(this[this.myTable.ColumnSpecialTaxRateCode])) != value)))
                {
                    this[this.myTable.ColumnSpecialTaxRateCode] = value;
                }
            }
        }
        
        /// this describes when this particular percentage rate has become valid by law
        public System.DateTime SpecialTaxValidFrom
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSpecialTaxValidFrom.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnSpecialTaxValidFrom) 
                            || (((System.DateTime)(this[this.myTable.ColumnSpecialTaxValidFrom])) != value)))
                {
                    this[this.myTable.ColumnSpecialTaxValidFrom] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime SpecialTaxValidFromLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnSpecialTaxValidFrom], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime SpecialTaxValidFromHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnSpecialTaxValidFrom.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// The total amount of money that this invoice is worth; this includes all discounts, even the early payment discount; if the early payment discount does not apply anymore at the time of payment, this total amount needs to be updated
        public Double TotalAmount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTotalAmount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Double)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTotalAmount) 
                            || (((Double)(this[this.myTable.ColumnTotalAmount])) != value)))
                {
                    this[this.myTable.ColumnTotalAmount] = value;
                }
            }
        }
        
        /// the currency of the total amount
        public String CurrencyCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCurrencyCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCurrencyCode) 
                            || (((String)(this[this.myTable.ColumnCurrencyCode])) != value)))
                {
                    this[this.myTable.ColumnCurrencyCode] = value;
                }
            }
        }
        
        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateCreated])) != value)))
                {
                    this[this.myTable.ColumnDateCreated] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateCreatedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateCreatedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCreatedBy) 
                            || (((String)(this[this.myTable.ColumnCreatedBy])) != value)))
                {
                    this[this.myTable.ColumnCreatedBy] = value;
                }
            }
        }
        
        /// The date the record was modified.
        public System.DateTime DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateModified])) != value)))
                {
                    this[this.myTable.ColumnDateModified] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateModifiedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateModifiedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModifiedBy) 
                            || (((String)(this[this.myTable.ColumnModifiedBy])) != value)))
                {
                    this[this.myTable.ColumnModifiedBy] = value;
                }
            }
        }
        
        /// This identifies the current version of the record.
        public String ModificationId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModificationId.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModificationId) 
                            || (((String)(this[this.myTable.ColumnModificationId])) != value)))
                {
                    this[this.myTable.ColumnModificationId] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnKey);
            this.SetNull(this.myTable.ColumnStatus);
            this.SetNull(this.myTable.ColumnPartnerKey);
            this.SetNull(this.myTable.ColumnDateEffective);
            this.SetNull(this.myTable.ColumnOffer);
            this[this.myTable.ColumnTaxing.Ordinal] = "DEFAULT";
            this.SetNull(this.myTable.ColumnSpecialTaxTypeCode);
            this.SetNull(this.myTable.ColumnSpecialTaxRateCode);
            this.SetNull(this.myTable.ColumnSpecialTaxValidFrom);
            this.SetNull(this.myTable.ColumnTotalAmount);
            this.SetNull(this.myTable.ColumnCurrencyCode);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsKeyNull()
        {
            return this.IsNull(this.myTable.ColumnKey);
        }
        
        /// assign NULL value
        public void SetKeyNull()
        {
            this.SetNull(this.myTable.ColumnKey);
        }
        
        /// test for NULL value
        public bool IsPartnerKeyNull()
        {
            return this.IsNull(this.myTable.ColumnPartnerKey);
        }
        
        /// assign NULL value
        public void SetPartnerKeyNull()
        {
            this.SetNull(this.myTable.ColumnPartnerKey);
        }
        
        /// test for NULL value
        public bool IsDateEffectiveNull()
        {
            return this.IsNull(this.myTable.ColumnDateEffective);
        }
        
        /// assign NULL value
        public void SetDateEffectiveNull()
        {
            this.SetNull(this.myTable.ColumnDateEffective);
        }
        
        /// test for NULL value
        public bool IsOfferNull()
        {
            return this.IsNull(this.myTable.ColumnOffer);
        }
        
        /// assign NULL value
        public void SetOfferNull()
        {
            this.SetNull(this.myTable.ColumnOffer);
        }
        
        /// test for NULL value
        public bool IsSpecialTaxTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnSpecialTaxTypeCode);
        }
        
        /// assign NULL value
        public void SetSpecialTaxTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnSpecialTaxTypeCode);
        }
        
        /// test for NULL value
        public bool IsSpecialTaxRateCodeNull()
        {
            return this.IsNull(this.myTable.ColumnSpecialTaxRateCode);
        }
        
        /// assign NULL value
        public void SetSpecialTaxRateCodeNull()
        {
            this.SetNull(this.myTable.ColumnSpecialTaxRateCode);
        }
        
        /// test for NULL value
        public bool IsSpecialTaxValidFromNull()
        {
            return this.IsNull(this.myTable.ColumnSpecialTaxValidFrom);
        }
        
        /// assign NULL value
        public void SetSpecialTaxValidFromNull()
        {
            this.SetNull(this.myTable.ColumnSpecialTaxValidFrom);
        }
        
        /// test for NULL value
        public bool IsTotalAmountNull()
        {
            return this.IsNull(this.myTable.ColumnTotalAmount);
        }
        
        /// assign NULL value
        public void SetTotalAmountNull()
        {
            this.SetNull(this.myTable.ColumnTotalAmount);
        }
        
        /// test for NULL value
        public bool IsDateCreatedNull()
        {
            return this.IsNull(this.myTable.ColumnDateCreated);
        }
        
        /// assign NULL value
        public void SetDateCreatedNull()
        {
            this.SetNull(this.myTable.ColumnDateCreated);
        }
        
        /// test for NULL value
        public bool IsCreatedByNull()
        {
            return this.IsNull(this.myTable.ColumnCreatedBy);
        }
        
        /// assign NULL value
        public void SetCreatedByNull()
        {
            this.SetNull(this.myTable.ColumnCreatedBy);
        }
        
        /// test for NULL value
        public bool IsDateModifiedNull()
        {
            return this.IsNull(this.myTable.ColumnDateModified);
        }
        
        /// assign NULL value
        public void SetDateModifiedNull()
        {
            this.SetNull(this.myTable.ColumnDateModified);
        }
        
        /// test for NULL value
        public bool IsModifiedByNull()
        {
            return this.IsNull(this.myTable.ColumnModifiedBy);
        }
        
        /// assign NULL value
        public void SetModifiedByNull()
        {
            this.SetNull(this.myTable.ColumnModifiedBy);
        }
        
        /// test for NULL value
        public bool IsModificationIdNull()
        {
            return this.IsNull(this.myTable.ColumnModificationId);
        }
        
        /// assign NULL value
        public void SetModificationIdNull()
        {
            this.SetNull(this.myTable.ColumnModificationId);
        }
    }
    
    /// an invoice consists of one or more details
    [Serializable()]
    public class AArInvoiceDetailTable : TTypedDataTable
    {
        
        /// This is used as a key field in most of the accounting system files
        public DataColumn ColumnLedgerNumber;
        
        /// this invoice detail belongs to the invoice with this key
        public DataColumn ColumnInvoiceKey;
        
        /// A unique number for this detail for its invoice.
        public DataColumn ColumnDetailNumber;
        
        /// code that uniquely identifies the item; can also be a code of a group of equally priced items
        public DataColumn ColumnArArticleCode;
        
        /// Reference for this invoice detail; for a non specific article that could give more details (e.g. which book of type small book)
        public DataColumn ColumnArReference;
        
        /// defines the number of the article items that is bought
        public DataColumn ColumnArNumberOfItem;
        
        /// Reference to the price that should be used for this article in this invoice, by date; without discounts, just for single item
        public DataColumn ColumnArArticlePrice;
        
        /// The total amount of money that this invoice detail is worth; includes the discounts
        public DataColumn ColumnCalculatedAmount;
        
        /// the currency of the total amount
        public DataColumn ColumnCurrencyCode;
        
        /// The tax type is always the same, e.g. VAT
        public DataColumn ColumnTaxTypeCode;
        
        /// this describes whether it is e.g. the standard, reduced or zero rate of VAT
        public DataColumn ColumnTaxRateCode;
        
        /// we need to be able to pick a rate that was valid in the past or will be valid in the future
        public DataColumn ColumnTaxValidFrom;
        
        /// The date the record was created.
        public DataColumn ColumnDateCreated;
        
        /// User ID of who created this record.
        public DataColumn ColumnCreatedBy;
        
        /// The date the record was modified.
        public DataColumn ColumnDateModified;
        
        /// User ID of who last modified this record.
        public DataColumn ColumnModifiedBy;
        
        /// This identifies the current version of the record.
        public DataColumn ColumnModificationId;
        
        /// constructor
        public AArInvoiceDetailTable() : 
                base("AArInvoiceDetail")
        {
        }
        
        /// constructor
        public AArInvoiceDetailTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public AArInvoiceDetailTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public AArInvoiceDetailRow this[int i]
        {
            get
            {
                return ((AArInvoiceDetailRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }
        
        /// get help text for column
        public static string GetLedgerNumberHelp()
        {
            return "Enter the ledger number";
        }
        
        /// get label of column
        public static string GetLedgerNumberLabel()
        {
            return "Ledger Number";
        }
        
        /// get display format for column
        public static short GetLedgerNumberLength()
        {
            return 4;
        }
        
        /// get the name of the field in the database for this column
        public static string GetInvoiceKeyDBName()
        {
            return "a_invoice_key_i";
        }
        
        /// get help text for column
        public static string GetInvoiceKeyHelp()
        {
            return "this invoice detail belongs to the invoice with this key";
        }
        
        /// get label of column
        public static string GetInvoiceKeyLabel()
        {
            return "Invoice Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDetailNumberDBName()
        {
            return "a_detail_number_i";
        }
        
        /// get help text for column
        public static string GetDetailNumberHelp()
        {
            return "A unique number for this detail for its invoice.";
        }
        
        /// get label of column
        public static string GetDetailNumberLabel()
        {
            return "Invoice Key";
        }
        
        /// get display format for column
        public static short GetDetailNumberLength()
        {
            return 9;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArArticleCodeDBName()
        {
            return "a_ar_article_code_c";
        }
        
        /// get help text for column
        public static string GetArArticleCodeHelp()
        {
            return "code that uniquely identifies the item; can also be a code of a group of equally " +
                "priced items";
        }
        
        /// get label of column
        public static string GetArArticleCodeLabel()
        {
            return "a_ar_article_code_c";
        }
        
        /// get character length for column
        public static short GetArArticleCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArReferenceDBName()
        {
            return "a_ar_reference_c";
        }
        
        /// get help text for column
        public static string GetArReferenceHelp()
        {
            return "Reference for this invoice detail; for a non specific article that could give mor" +
                "e details (e.g. which book of type small book)";
        }
        
        /// get label of column
        public static string GetArReferenceLabel()
        {
            return "a_ar_reference_c";
        }
        
        /// get character length for column
        public static short GetArReferenceLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArNumberOfItemDBName()
        {
            return "a_ar_number_of_item_i";
        }
        
        /// get help text for column
        public static string GetArNumberOfItemHelp()
        {
            return "defines the number of the article items that is bought";
        }
        
        /// get label of column
        public static string GetArNumberOfItemLabel()
        {
            return "a_ar_number_of_item_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArArticlePriceDBName()
        {
            return "a_ar_article_price_d";
        }
        
        /// get help text for column
        public static string GetArArticlePriceHelp()
        {
            return "Reference to the price that should be used for this article in this invoice, by d" +
                "ate; without discounts, just for single item";
        }
        
        /// get label of column
        public static string GetArArticlePriceLabel()
        {
            return "a_ar_article_price_d";
        }
        
        /// get the name of the field in the database for this column
        public static string GetCalculatedAmountDBName()
        {
            return "a_calculated_amount_n";
        }
        
        /// get help text for column
        public static string GetCalculatedAmountHelp()
        {
            return "The total amount of money that this invoice detail is worth; includes the discoun" +
                "ts";
        }
        
        /// get label of column
        public static string GetCalculatedAmountLabel()
        {
            return "a_calculated_amount_n";
        }
        
        /// get display format for column
        public static short GetCalculatedAmountLength()
        {
            return 19;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCurrencyCodeDBName()
        {
            return "a_currency_code_c";
        }
        
        /// get help text for column
        public static string GetCurrencyCodeHelp()
        {
            return "the currency of the total amount";
        }
        
        /// get label of column
        public static string GetCurrencyCodeLabel()
        {
            return "a_currency_code_c";
        }
        
        /// get character length for column
        public static short GetCurrencyCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTaxTypeCodeDBName()
        {
            return "a_tax_type_code_c";
        }
        
        /// get help text for column
        public static string GetTaxTypeCodeHelp()
        {
            return "Enter a tax type code";
        }
        
        /// get label of column
        public static string GetTaxTypeCodeLabel()
        {
            return "Tax Type Code";
        }
        
        /// get character length for column
        public static short GetTaxTypeCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTaxRateCodeDBName()
        {
            return "a_tax_rate_code_c";
        }
        
        /// get help text for column
        public static string GetTaxRateCodeHelp()
        {
            return "Enter a tax rate code";
        }
        
        /// get label of column
        public static string GetTaxRateCodeLabel()
        {
            return "Tax Rate Code";
        }
        
        /// get character length for column
        public static short GetTaxRateCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTaxValidFromDBName()
        {
            return "a_tax_valid_from_d";
        }
        
        /// get help text for column
        public static string GetTaxValidFromHelp()
        {
            return "we need to be able to pick a rate that was valid in the past or will be valid in " +
                "the future";
        }
        
        /// get label of column
        public static string GetTaxValidFromLabel()
        {
            return "a_tax_valid_from_d";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }
        
        /// get help text for column
        public static string GetDateCreatedHelp()
        {
            return "The date the record was created.";
        }
        
        /// get label of column
        public static string GetDateCreatedLabel()
        {
            return "Created Date";
        }
        
        /// get display format for column
        public static short GetDateCreatedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }
        
        /// get help text for column
        public static string GetCreatedByHelp()
        {
            return "User ID of who created this record.";
        }
        
        /// get label of column
        public static string GetCreatedByLabel()
        {
            return "Created By";
        }
        
        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }
        
        /// get help text for column
        public static string GetDateModifiedHelp()
        {
            return "The date the record was modified.";
        }
        
        /// get label of column
        public static string GetDateModifiedLabel()
        {
            return "Modified Date";
        }
        
        /// get display format for column
        public static short GetDateModifiedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }
        
        /// get help text for column
        public static string GetModifiedByHelp()
        {
            return "User ID of who last modified this record.";
        }
        
        /// get label of column
        public static string GetModifiedByLabel()
        {
            return "Modified By";
        }
        
        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }
        
        /// get help text for column
        public static string GetModificationIdHelp()
        {
            return "This identifies the current version of the record.";
        }
        
        /// get label of column
        public static string GetModificationIdLabel()
        {
            return "";
        }
        
        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "AArInvoiceDetail";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ar_invoice_detail";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Invoice Detail";
        }
        
        /// get the index number of fields that are part of the primary key
        public static Int32[] GetPrimKeyColumnOrdList()
        {
            return new int[] {
                    0,
                    1,
                    2};
        }
        
        /// get the names of the columns
        public static String[] GetColumnStringList()
        {
            return new string[] {
                    "a_ledger_number_i",
                    "a_invoice_key_i",
                    "a_detail_number_i",
                    "a_ar_article_code_c",
                    "a_ar_reference_c",
                    "a_ar_number_of_item_i",
                    "a_ar_article_price_d",
                    "a_calculated_amount_n",
                    "a_currency_code_c",
                    "a_tax_type_code_c",
                    "a_tax_rate_code_c",
                    "a_tax_valid_from_d",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnInvoiceKey = this.Columns["a_invoice_key_i"];
            this.ColumnDetailNumber = this.Columns["a_detail_number_i"];
            this.ColumnArArticleCode = this.Columns["a_ar_article_code_c"];
            this.ColumnArReference = this.Columns["a_ar_reference_c"];
            this.ColumnArNumberOfItem = this.Columns["a_ar_number_of_item_i"];
            this.ColumnArArticlePrice = this.Columns["a_ar_article_price_d"];
            this.ColumnCalculatedAmount = this.Columns["a_calculated_amount_n"];
            this.ColumnCurrencyCode = this.Columns["a_currency_code_c"];
            this.ColumnTaxTypeCode = this.Columns["a_tax_type_code_c"];
            this.ColumnTaxRateCode = this.Columns["a_tax_rate_code_c"];
            this.ColumnTaxValidFrom = this.Columns["a_tax_valid_from_d"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLedgerNumber,
                    this.ColumnInvoiceKey,
                    this.ColumnDetailNumber};
        }
        
        /// get typed set of changes
        public AArInvoiceDetailTable GetChangesTyped()
        {
            return ((AArInvoiceDetailTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public AArInvoiceDetailRow NewRowTyped(bool AWithDefaultValues)
        {
            AArInvoiceDetailRow ret = ((AArInvoiceDetailRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public AArInvoiceDetailRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AArInvoiceDetailRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_invoice_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_article_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_reference_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_number_of_item_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_article_price_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_calculated_amount_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("a_currency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_rate_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_tax_valid_from_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLedgerNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnInvoiceKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnDetailNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnArArticleCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnArReference))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnArNumberOfItem))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnArArticlePrice))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCalculatedAmount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnCurrencyCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnTaxTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnTaxRateCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnTaxValidFrom))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnDateCreated))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCreatedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateModified))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnModifiedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnModificationId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 150);
            }
            return null;
        }
    }
    
    /// an invoice consists of one or more details
    [Serializable()]
    public class AArInvoiceDetailRow : System.Data.DataRow
    {
        
        private AArInvoiceDetailTable myTable;
        
        /// Constructor
        public AArInvoiceDetailRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((AArInvoiceDetailTable)(this.Table));
        }
        
        /// This is used as a key field in most of the accounting system files
        public Int32 LedgerNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLedgerNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLedgerNumber) 
                            || (((Int32)(this[this.myTable.ColumnLedgerNumber])) != value)))
                {
                    this[this.myTable.ColumnLedgerNumber] = value;
                }
            }
        }
        
        /// this invoice detail belongs to the invoice with this key
        public Int32 InvoiceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnInvoiceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnInvoiceKey) 
                            || (((Int32)(this[this.myTable.ColumnInvoiceKey])) != value)))
                {
                    this[this.myTable.ColumnInvoiceKey] = value;
                }
            }
        }
        
        /// A unique number for this detail for its invoice.
        public Int32 DetailNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDetailNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDetailNumber) 
                            || (((Int32)(this[this.myTable.ColumnDetailNumber])) != value)))
                {
                    this[this.myTable.ColumnDetailNumber] = value;
                }
            }
        }
        
        /// code that uniquely identifies the item; can also be a code of a group of equally priced items
        public String ArArticleCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArArticleCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArArticleCode) 
                            || (((String)(this[this.myTable.ColumnArArticleCode])) != value)))
                {
                    this[this.myTable.ColumnArArticleCode] = value;
                }
            }
        }
        
        /// Reference for this invoice detail; for a non specific article that could give more details (e.g. which book of type small book)
        public String ArReference
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArReference.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArReference) 
                            || (((String)(this[this.myTable.ColumnArReference])) != value)))
                {
                    this[this.myTable.ColumnArReference] = value;
                }
            }
        }
        
        /// defines the number of the article items that is bought
        public Int32 ArNumberOfItem
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArNumberOfItem.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnArNumberOfItem) 
                            || (((Int32)(this[this.myTable.ColumnArNumberOfItem])) != value)))
                {
                    this[this.myTable.ColumnArNumberOfItem] = value;
                }
            }
        }
        
        /// Reference to the price that should be used for this article in this invoice, by date; without discounts, just for single item
        public System.DateTime ArArticlePrice
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArArticlePrice.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArArticlePrice) 
                            || (((System.DateTime)(this[this.myTable.ColumnArArticlePrice])) != value)))
                {
                    this[this.myTable.ColumnArArticlePrice] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ArArticlePriceLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArArticlePrice], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ArArticlePriceHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArArticlePrice.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// The total amount of money that this invoice detail is worth; includes the discounts
        public Double CalculatedAmount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCalculatedAmount.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((Double)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCalculatedAmount) 
                            || (((Double)(this[this.myTable.ColumnCalculatedAmount])) != value)))
                {
                    this[this.myTable.ColumnCalculatedAmount] = value;
                }
            }
        }
        
        /// the currency of the total amount
        public String CurrencyCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCurrencyCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCurrencyCode) 
                            || (((String)(this[this.myTable.ColumnCurrencyCode])) != value)))
                {
                    this[this.myTable.ColumnCurrencyCode] = value;
                }
            }
        }
        
        /// The tax type is always the same, e.g. VAT
        public String TaxTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxTypeCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTaxTypeCode) 
                            || (((String)(this[this.myTable.ColumnTaxTypeCode])) != value)))
                {
                    this[this.myTable.ColumnTaxTypeCode] = value;
                }
            }
        }
        
        /// this describes whether it is e.g. the standard, reduced or zero rate of VAT
        public String TaxRateCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxRateCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTaxRateCode) 
                            || (((String)(this[this.myTable.ColumnTaxRateCode])) != value)))
                {
                    this[this.myTable.ColumnTaxRateCode] = value;
                }
            }
        }
        
        /// we need to be able to pick a rate that was valid in the past or will be valid in the future
        public System.DateTime TaxValidFrom
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTaxValidFrom.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnTaxValidFrom) 
                            || (((System.DateTime)(this[this.myTable.ColumnTaxValidFrom])) != value)))
                {
                    this[this.myTable.ColumnTaxValidFrom] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime TaxValidFromLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnTaxValidFrom], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime TaxValidFromHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnTaxValidFrom.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateCreated])) != value)))
                {
                    this[this.myTable.ColumnDateCreated] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateCreatedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateCreatedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCreatedBy) 
                            || (((String)(this[this.myTable.ColumnCreatedBy])) != value)))
                {
                    this[this.myTable.ColumnCreatedBy] = value;
                }
            }
        }
        
        /// The date the record was modified.
        public System.DateTime DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateModified])) != value)))
                {
                    this[this.myTable.ColumnDateModified] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateModifiedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateModifiedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModifiedBy) 
                            || (((String)(this[this.myTable.ColumnModifiedBy])) != value)))
                {
                    this[this.myTable.ColumnModifiedBy] = value;
                }
            }
        }
        
        /// This identifies the current version of the record.
        public String ModificationId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModificationId.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModificationId) 
                            || (((String)(this[this.myTable.ColumnModificationId])) != value)))
                {
                    this[this.myTable.ColumnModificationId] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnInvoiceKey);
            this.SetNull(this.myTable.ColumnDetailNumber);
            this.SetNull(this.myTable.ColumnArArticleCode);
            this.SetNull(this.myTable.ColumnArReference);
            this.SetNull(this.myTable.ColumnArNumberOfItem);
            this.SetNull(this.myTable.ColumnArArticlePrice);
            this.SetNull(this.myTable.ColumnCalculatedAmount);
            this.SetNull(this.myTable.ColumnCurrencyCode);
            this.SetNull(this.myTable.ColumnTaxTypeCode);
            this.SetNull(this.myTable.ColumnTaxRateCode);
            this.SetNull(this.myTable.ColumnTaxValidFrom);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsInvoiceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnInvoiceKey);
        }
        
        /// assign NULL value
        public void SetInvoiceKeyNull()
        {
            this.SetNull(this.myTable.ColumnInvoiceKey);
        }
        
        /// test for NULL value
        public bool IsDetailNumberNull()
        {
            return this.IsNull(this.myTable.ColumnDetailNumber);
        }
        
        /// assign NULL value
        public void SetDetailNumberNull()
        {
            this.SetNull(this.myTable.ColumnDetailNumber);
        }
        
        /// test for NULL value
        public bool IsArReferenceNull()
        {
            return this.IsNull(this.myTable.ColumnArReference);
        }
        
        /// assign NULL value
        public void SetArReferenceNull()
        {
            this.SetNull(this.myTable.ColumnArReference);
        }
        
        /// test for NULL value
        public bool IsCalculatedAmountNull()
        {
            return this.IsNull(this.myTable.ColumnCalculatedAmount);
        }
        
        /// assign NULL value
        public void SetCalculatedAmountNull()
        {
            this.SetNull(this.myTable.ColumnCalculatedAmount);
        }
        
        /// test for NULL value
        public bool IsDateCreatedNull()
        {
            return this.IsNull(this.myTable.ColumnDateCreated);
        }
        
        /// assign NULL value
        public void SetDateCreatedNull()
        {
            this.SetNull(this.myTable.ColumnDateCreated);
        }
        
        /// test for NULL value
        public bool IsCreatedByNull()
        {
            return this.IsNull(this.myTable.ColumnCreatedBy);
        }
        
        /// assign NULL value
        public void SetCreatedByNull()
        {
            this.SetNull(this.myTable.ColumnCreatedBy);
        }
        
        /// test for NULL value
        public bool IsDateModifiedNull()
        {
            return this.IsNull(this.myTable.ColumnDateModified);
        }
        
        /// assign NULL value
        public void SetDateModifiedNull()
        {
            this.SetNull(this.myTable.ColumnDateModified);
        }
        
        /// test for NULL value
        public bool IsModifiedByNull()
        {
            return this.IsNull(this.myTable.ColumnModifiedBy);
        }
        
        /// assign NULL value
        public void SetModifiedByNull()
        {
            this.SetNull(this.myTable.ColumnModifiedBy);
        }
        
        /// test for NULL value
        public bool IsModificationIdNull()
        {
            return this.IsNull(this.myTable.ColumnModificationId);
        }
        
        /// assign NULL value
        public void SetModificationIdNull()
        {
            this.SetNull(this.myTable.ColumnModificationId);
        }
    }
    
    /// defines which discounts apply directly to the invoice rather than the invoice items; this can depend on the customer etc
    [Serializable()]
    public class AArInvoiceDiscountTable : TTypedDataTable
    {
        
        /// This is used as a key field in most of the accounting system files
        public DataColumn ColumnLedgerNumber;
        
        /// Key to uniquely identify invoice
        public DataColumn ColumnInvoiceKey;
        
        /// code that identifies the discount
        public DataColumn ColumnArDiscountCode;
        
        /// this clearly specifies which version of the discount is meant
        public DataColumn ColumnArDiscountDateValidFrom;
        
        /// The date the record was created.
        public DataColumn ColumnDateCreated;
        
        /// User ID of who created this record.
        public DataColumn ColumnCreatedBy;
        
        /// The date the record was modified.
        public DataColumn ColumnDateModified;
        
        /// User ID of who last modified this record.
        public DataColumn ColumnModifiedBy;
        
        /// This identifies the current version of the record.
        public DataColumn ColumnModificationId;
        
        /// constructor
        public AArInvoiceDiscountTable() : 
                base("AArInvoiceDiscount")
        {
        }
        
        /// constructor
        public AArInvoiceDiscountTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public AArInvoiceDiscountTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public AArInvoiceDiscountRow this[int i]
        {
            get
            {
                return ((AArInvoiceDiscountRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }
        
        /// get help text for column
        public static string GetLedgerNumberHelp()
        {
            return "Enter the ledger number";
        }
        
        /// get label of column
        public static string GetLedgerNumberLabel()
        {
            return "Ledger Number";
        }
        
        /// get display format for column
        public static short GetLedgerNumberLength()
        {
            return 4;
        }
        
        /// get the name of the field in the database for this column
        public static string GetInvoiceKeyDBName()
        {
            return "a_invoice_key_i";
        }
        
        /// get help text for column
        public static string GetInvoiceKeyHelp()
        {
            return "Key to uniquely identify invoice";
        }
        
        /// get label of column
        public static string GetInvoiceKeyLabel()
        {
            return "Invoice Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDiscountCodeDBName()
        {
            return "a_ar_discount_code_c";
        }
        
        /// get help text for column
        public static string GetArDiscountCodeHelp()
        {
            return "code that identifies the discount";
        }
        
        /// get label of column
        public static string GetArDiscountCodeLabel()
        {
            return "a_ar_discount_code_c";
        }
        
        /// get character length for column
        public static short GetArDiscountCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDiscountDateValidFromDBName()
        {
            return "a_ar_discount_date_valid_from_d";
        }
        
        /// get help text for column
        public static string GetArDiscountDateValidFromHelp()
        {
            return "this clearly specifies which version of the discount is meant";
        }
        
        /// get label of column
        public static string GetArDiscountDateValidFromLabel()
        {
            return "a_ar_discount_date_valid_from_d";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }
        
        /// get help text for column
        public static string GetDateCreatedHelp()
        {
            return "The date the record was created.";
        }
        
        /// get label of column
        public static string GetDateCreatedLabel()
        {
            return "Created Date";
        }
        
        /// get display format for column
        public static short GetDateCreatedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }
        
        /// get help text for column
        public static string GetCreatedByHelp()
        {
            return "User ID of who created this record.";
        }
        
        /// get label of column
        public static string GetCreatedByLabel()
        {
            return "Created By";
        }
        
        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }
        
        /// get help text for column
        public static string GetDateModifiedHelp()
        {
            return "The date the record was modified.";
        }
        
        /// get label of column
        public static string GetDateModifiedLabel()
        {
            return "Modified Date";
        }
        
        /// get display format for column
        public static short GetDateModifiedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }
        
        /// get help text for column
        public static string GetModifiedByHelp()
        {
            return "User ID of who last modified this record.";
        }
        
        /// get label of column
        public static string GetModifiedByLabel()
        {
            return "Modified By";
        }
        
        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }
        
        /// get help text for column
        public static string GetModificationIdHelp()
        {
            return "This identifies the current version of the record.";
        }
        
        /// get label of column
        public static string GetModificationIdLabel()
        {
            return "";
        }
        
        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "AArInvoiceDiscount";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ar_invoice_discount";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Discount per invoice";
        }
        
        /// get the index number of fields that are part of the primary key
        public static Int32[] GetPrimKeyColumnOrdList()
        {
            return new int[] {
                    0,
                    1,
                    2,
                    3};
        }
        
        /// get the names of the columns
        public static String[] GetColumnStringList()
        {
            return new string[] {
                    "a_ledger_number_i",
                    "a_invoice_key_i",
                    "a_ar_discount_code_c",
                    "a_ar_discount_date_valid_from_d",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnInvoiceKey = this.Columns["a_invoice_key_i"];
            this.ColumnArDiscountCode = this.Columns["a_ar_discount_code_c"];
            this.ColumnArDiscountDateValidFrom = this.Columns["a_ar_discount_date_valid_from_d"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLedgerNumber,
                    this.ColumnInvoiceKey,
                    this.ColumnArDiscountCode,
                    this.ColumnArDiscountDateValidFrom};
        }
        
        /// get typed set of changes
        public AArInvoiceDiscountTable GetChangesTyped()
        {
            return ((AArInvoiceDiscountTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public AArInvoiceDiscountRow NewRowTyped(bool AWithDefaultValues)
        {
            AArInvoiceDiscountRow ret = ((AArInvoiceDiscountRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public AArInvoiceDiscountRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AArInvoiceDiscountRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_invoice_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_discount_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_discount_date_valid_from_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLedgerNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnInvoiceKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnArDiscountCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnArDiscountDateValidFrom))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnDateCreated))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCreatedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateModified))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnModifiedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnModificationId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 150);
            }
            return null;
        }
    }
    
    /// defines which discounts apply directly to the invoice rather than the invoice items; this can depend on the customer etc
    [Serializable()]
    public class AArInvoiceDiscountRow : System.Data.DataRow
    {
        
        private AArInvoiceDiscountTable myTable;
        
        /// Constructor
        public AArInvoiceDiscountRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((AArInvoiceDiscountTable)(this.Table));
        }
        
        /// This is used as a key field in most of the accounting system files
        public Int32 LedgerNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLedgerNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLedgerNumber) 
                            || (((Int32)(this[this.myTable.ColumnLedgerNumber])) != value)))
                {
                    this[this.myTable.ColumnLedgerNumber] = value;
                }
            }
        }
        
        /// Key to uniquely identify invoice
        public Int32 InvoiceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnInvoiceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnInvoiceKey) 
                            || (((Int32)(this[this.myTable.ColumnInvoiceKey])) != value)))
                {
                    this[this.myTable.ColumnInvoiceKey] = value;
                }
            }
        }
        
        /// code that identifies the discount
        public String ArDiscountCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDiscountCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDiscountCode) 
                            || (((String)(this[this.myTable.ColumnArDiscountCode])) != value)))
                {
                    this[this.myTable.ColumnArDiscountCode] = value;
                }
            }
        }
        
        /// this clearly specifies which version of the discount is meant
        public System.DateTime ArDiscountDateValidFrom
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDiscountDateValidFrom.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDiscountDateValidFrom) 
                            || (((System.DateTime)(this[this.myTable.ColumnArDiscountDateValidFrom])) != value)))
                {
                    this[this.myTable.ColumnArDiscountDateValidFrom] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ArDiscountDateValidFromLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDiscountDateValidFrom], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ArDiscountDateValidFromHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDiscountDateValidFrom.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateCreated])) != value)))
                {
                    this[this.myTable.ColumnDateCreated] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateCreatedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateCreatedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCreatedBy) 
                            || (((String)(this[this.myTable.ColumnCreatedBy])) != value)))
                {
                    this[this.myTable.ColumnCreatedBy] = value;
                }
            }
        }
        
        /// The date the record was modified.
        public System.DateTime DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateModified])) != value)))
                {
                    this[this.myTable.ColumnDateModified] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateModifiedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateModifiedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModifiedBy) 
                            || (((String)(this[this.myTable.ColumnModifiedBy])) != value)))
                {
                    this[this.myTable.ColumnModifiedBy] = value;
                }
            }
        }
        
        /// This identifies the current version of the record.
        public String ModificationId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModificationId.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModificationId) 
                            || (((String)(this[this.myTable.ColumnModificationId])) != value)))
                {
                    this[this.myTable.ColumnModificationId] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnInvoiceKey);
            this.SetNull(this.myTable.ColumnArDiscountCode);
            this.SetNull(this.myTable.ColumnArDiscountDateValidFrom);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsInvoiceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnInvoiceKey);
        }
        
        /// assign NULL value
        public void SetInvoiceKeyNull()
        {
            this.SetNull(this.myTable.ColumnInvoiceKey);
        }
        
        /// test for NULL value
        public bool IsDateCreatedNull()
        {
            return this.IsNull(this.myTable.ColumnDateCreated);
        }
        
        /// assign NULL value
        public void SetDateCreatedNull()
        {
            this.SetNull(this.myTable.ColumnDateCreated);
        }
        
        /// test for NULL value
        public bool IsCreatedByNull()
        {
            return this.IsNull(this.myTable.ColumnCreatedBy);
        }
        
        /// assign NULL value
        public void SetCreatedByNull()
        {
            this.SetNull(this.myTable.ColumnCreatedBy);
        }
        
        /// test for NULL value
        public bool IsDateModifiedNull()
        {
            return this.IsNull(this.myTable.ColumnDateModified);
        }
        
        /// assign NULL value
        public void SetDateModifiedNull()
        {
            this.SetNull(this.myTable.ColumnDateModified);
        }
        
        /// test for NULL value
        public bool IsModifiedByNull()
        {
            return this.IsNull(this.myTable.ColumnModifiedBy);
        }
        
        /// assign NULL value
        public void SetModifiedByNull()
        {
            this.SetNull(this.myTable.ColumnModifiedBy);
        }
        
        /// test for NULL value
        public bool IsModificationIdNull()
        {
            return this.IsNull(this.myTable.ColumnModificationId);
        }
        
        /// assign NULL value
        public void SetModificationIdNull()
        {
            this.SetNull(this.myTable.ColumnModificationId);
        }
    }
    
    /// defines which discounts apply one invoice item
    [Serializable()]
    public class AArInvoiceDetailDiscountTable : TTypedDataTable
    {
        
        /// This is used as a key field in most of the accounting system files
        public DataColumn ColumnLedgerNumber;
        
        /// Key to uniquely identify invoice
        public DataColumn ColumnInvoiceKey;
        
        /// A unique number for this detail for its invoice.
        public DataColumn ColumnDetailNumber;
        
        /// code that identifies the discount
        public DataColumn ColumnArDiscountCode;
        
        /// this clearly specifies which version of the discount is meant
        public DataColumn ColumnArDiscountDateValidFrom;
        
        /// The date the record was created.
        public DataColumn ColumnDateCreated;
        
        /// User ID of who created this record.
        public DataColumn ColumnCreatedBy;
        
        /// The date the record was modified.
        public DataColumn ColumnDateModified;
        
        /// User ID of who last modified this record.
        public DataColumn ColumnModifiedBy;
        
        /// This identifies the current version of the record.
        public DataColumn ColumnModificationId;
        
        /// constructor
        public AArInvoiceDetailDiscountTable() : 
                base("AArInvoiceDetailDiscount")
        {
        }
        
        /// constructor
        public AArInvoiceDetailDiscountTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public AArInvoiceDetailDiscountTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public AArInvoiceDetailDiscountRow this[int i]
        {
            get
            {
                return ((AArInvoiceDetailDiscountRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetLedgerNumberDBName()
        {
            return "a_ledger_number_i";
        }
        
        /// get help text for column
        public static string GetLedgerNumberHelp()
        {
            return "Enter the ledger number";
        }
        
        /// get label of column
        public static string GetLedgerNumberLabel()
        {
            return "Ledger Number";
        }
        
        /// get display format for column
        public static short GetLedgerNumberLength()
        {
            return 4;
        }
        
        /// get the name of the field in the database for this column
        public static string GetInvoiceKeyDBName()
        {
            return "a_invoice_key_i";
        }
        
        /// get help text for column
        public static string GetInvoiceKeyHelp()
        {
            return "Key to uniquely identify invoice";
        }
        
        /// get label of column
        public static string GetInvoiceKeyLabel()
        {
            return "Invoice Key";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDetailNumberDBName()
        {
            return "a_detail_number_i";
        }
        
        /// get help text for column
        public static string GetDetailNumberHelp()
        {
            return "A unique number for this detail for its invoice.";
        }
        
        /// get label of column
        public static string GetDetailNumberLabel()
        {
            return "Invoice Key";
        }
        
        /// get display format for column
        public static short GetDetailNumberLength()
        {
            return 9;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDiscountCodeDBName()
        {
            return "a_ar_discount_code_c";
        }
        
        /// get help text for column
        public static string GetArDiscountCodeHelp()
        {
            return "code that identifies the discount";
        }
        
        /// get label of column
        public static string GetArDiscountCodeLabel()
        {
            return "a_ar_discount_code_c";
        }
        
        /// get character length for column
        public static short GetArDiscountCodeLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetArDiscountDateValidFromDBName()
        {
            return "a_ar_discount_date_valid_from_d";
        }
        
        /// get help text for column
        public static string GetArDiscountDateValidFromHelp()
        {
            return "this clearly specifies which version of the discount is meant";
        }
        
        /// get label of column
        public static string GetArDiscountDateValidFromLabel()
        {
            return "a_ar_discount_date_valid_from_d";
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }
        
        /// get help text for column
        public static string GetDateCreatedHelp()
        {
            return "The date the record was created.";
        }
        
        /// get label of column
        public static string GetDateCreatedLabel()
        {
            return "Created Date";
        }
        
        /// get display format for column
        public static short GetDateCreatedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }
        
        /// get help text for column
        public static string GetCreatedByHelp()
        {
            return "User ID of who created this record.";
        }
        
        /// get label of column
        public static string GetCreatedByLabel()
        {
            return "Created By";
        }
        
        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }
        
        /// get help text for column
        public static string GetDateModifiedHelp()
        {
            return "The date the record was modified.";
        }
        
        /// get label of column
        public static string GetDateModifiedLabel()
        {
            return "Modified Date";
        }
        
        /// get display format for column
        public static short GetDateModifiedLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }
        
        /// get help text for column
        public static string GetModifiedByHelp()
        {
            return "User ID of who last modified this record.";
        }
        
        /// get label of column
        public static string GetModifiedByLabel()
        {
            return "Modified By";
        }
        
        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }
        
        /// get help text for column
        public static string GetModificationIdHelp()
        {
            return "This identifies the current version of the record.";
        }
        
        /// get label of column
        public static string GetModificationIdLabel()
        {
            return "";
        }
        
        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
        }
        
        /// CamelCase version of the tablename
        public static string GetTableName()
        {
            return "AArInvoiceDetailDiscount";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_ar_invoice_detail_discount";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Discount per invoice detail";
        }
        
        /// get the index number of fields that are part of the primary key
        public static Int32[] GetPrimKeyColumnOrdList()
        {
            return new int[] {
                    0,
                    1,
                    2,
                    3,
                    4};
        }
        
        /// get the names of the columns
        public static String[] GetColumnStringList()
        {
            return new string[] {
                    "a_ledger_number_i",
                    "a_invoice_key_i",
                    "a_detail_number_i",
                    "a_ar_discount_code_c",
                    "a_ar_discount_date_valid_from_d",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLedgerNumber = this.Columns["a_ledger_number_i"];
            this.ColumnInvoiceKey = this.Columns["a_invoice_key_i"];
            this.ColumnDetailNumber = this.Columns["a_detail_number_i"];
            this.ColumnArDiscountCode = this.Columns["a_ar_discount_code_c"];
            this.ColumnArDiscountDateValidFrom = this.Columns["a_ar_discount_date_valid_from_d"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLedgerNumber,
                    this.ColumnInvoiceKey,
                    this.ColumnDetailNumber,
                    this.ColumnArDiscountCode,
                    this.ColumnArDiscountDateValidFrom};
        }
        
        /// get typed set of changes
        public AArInvoiceDetailDiscountTable GetChangesTyped()
        {
            return ((AArInvoiceDetailDiscountTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public AArInvoiceDetailDiscountRow NewRowTyped(bool AWithDefaultValues)
        {
            AArInvoiceDetailDiscountRow ret = ((AArInvoiceDetailDiscountRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public AArInvoiceDetailDiscountRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AArInvoiceDetailDiscountRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_ledger_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_invoice_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_detail_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_discount_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_ar_discount_date_valid_from_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLedgerNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnInvoiceKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnDetailNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnArDiscountCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnArDiscountDateValidFrom))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnDateCreated))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnCreatedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateModified))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnModifiedBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnModificationId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 150);
            }
            return null;
        }
    }
    
    /// defines which discounts apply one invoice item
    [Serializable()]
    public class AArInvoiceDetailDiscountRow : System.Data.DataRow
    {
        
        private AArInvoiceDetailDiscountTable myTable;
        
        /// Constructor
        public AArInvoiceDetailDiscountRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((AArInvoiceDetailDiscountTable)(this.Table));
        }
        
        /// This is used as a key field in most of the accounting system files
        public Int32 LedgerNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLedgerNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLedgerNumber) 
                            || (((Int32)(this[this.myTable.ColumnLedgerNumber])) != value)))
                {
                    this[this.myTable.ColumnLedgerNumber] = value;
                }
            }
        }
        
        /// Key to uniquely identify invoice
        public Int32 InvoiceKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnInvoiceKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnInvoiceKey) 
                            || (((Int32)(this[this.myTable.ColumnInvoiceKey])) != value)))
                {
                    this[this.myTable.ColumnInvoiceKey] = value;
                }
            }
        }
        
        /// A unique number for this detail for its invoice.
        public Int32 DetailNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDetailNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDetailNumber) 
                            || (((Int32)(this[this.myTable.ColumnDetailNumber])) != value)))
                {
                    this[this.myTable.ColumnDetailNumber] = value;
                }
            }
        }
        
        /// code that identifies the discount
        public String ArDiscountCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDiscountCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDiscountCode) 
                            || (((String)(this[this.myTable.ColumnArDiscountCode])) != value)))
                {
                    this[this.myTable.ColumnArDiscountCode] = value;
                }
            }
        }
        
        /// this clearly specifies which version of the discount is meant
        public System.DateTime ArDiscountDateValidFrom
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnArDiscountDateValidFrom.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnArDiscountDateValidFrom) 
                            || (((System.DateTime)(this[this.myTable.ColumnArDiscountDateValidFrom])) != value)))
                {
                    this[this.myTable.ColumnArDiscountDateValidFrom] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ArDiscountDateValidFromLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDiscountDateValidFrom], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ArDiscountDateValidFromHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnArDiscountDateValidFrom.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// The date the record was created.
        public System.DateTime DateCreated
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCreated.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateCreated) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateCreated])) != value)))
                {
                    this[this.myTable.ColumnDateCreated] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateCreatedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateCreatedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCreated.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCreatedBy) 
                            || (((String)(this[this.myTable.ColumnCreatedBy])) != value)))
                {
                    this[this.myTable.ColumnCreatedBy] = value;
                }
            }
        }
        
        /// The date the record was modified.
        public System.DateTime DateModified
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateModified.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    throw new System.Data.StrongTypingException("Error: DB null", null);
                }
                else
                {
                    return ((System.DateTime)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDateModified) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateModified])) != value)))
                {
                    this[this.myTable.ColumnDateModified] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateModifiedLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateModifiedHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateModified.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModifiedBy) 
                            || (((String)(this[this.myTable.ColumnModifiedBy])) != value)))
                {
                    this[this.myTable.ColumnModifiedBy] = value;
                }
            }
        }
        
        /// This identifies the current version of the record.
        public String ModificationId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModificationId.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnModificationId) 
                            || (((String)(this[this.myTable.ColumnModificationId])) != value)))
                {
                    this[this.myTable.ColumnModificationId] = value;
                }
            }
        }
        
        /// set default values
        public virtual void InitValues()
        {
            this[this.myTable.ColumnLedgerNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnInvoiceKey);
            this.SetNull(this.myTable.ColumnDetailNumber);
            this.SetNull(this.myTable.ColumnArDiscountCode);
            this.SetNull(this.myTable.ColumnArDiscountDateValidFrom);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsInvoiceKeyNull()
        {
            return this.IsNull(this.myTable.ColumnInvoiceKey);
        }
        
        /// assign NULL value
        public void SetInvoiceKeyNull()
        {
            this.SetNull(this.myTable.ColumnInvoiceKey);
        }
        
        /// test for NULL value
        public bool IsDetailNumberNull()
        {
            return this.IsNull(this.myTable.ColumnDetailNumber);
        }
        
        /// assign NULL value
        public void SetDetailNumberNull()
        {
            this.SetNull(this.myTable.ColumnDetailNumber);
        }
        
        /// test for NULL value
        public bool IsDateCreatedNull()
        {
            return this.IsNull(this.myTable.ColumnDateCreated);
        }
        
        /// assign NULL value
        public void SetDateCreatedNull()
        {
            this.SetNull(this.myTable.ColumnDateCreated);
        }
        
        /// test for NULL value
        public bool IsCreatedByNull()
        {
            return this.IsNull(this.myTable.ColumnCreatedBy);
        }
        
        /// assign NULL value
        public void SetCreatedByNull()
        {
            this.SetNull(this.myTable.ColumnCreatedBy);
        }
        
        /// test for NULL value
        public bool IsDateModifiedNull()
        {
            return this.IsNull(this.myTable.ColumnDateModified);
        }
        
        /// assign NULL value
        public void SetDateModifiedNull()
        {
            this.SetNull(this.myTable.ColumnDateModified);
        }
        
        /// test for NULL value
        public bool IsModifiedByNull()
        {
            return this.IsNull(this.myTable.ColumnModifiedBy);
        }
        
        /// assign NULL value
        public void SetModifiedByNull()
        {
            this.SetNull(this.myTable.ColumnModifiedBy);
        }
        
        /// test for NULL value
        public bool IsModificationIdNull()
        {
            return this.IsNull(this.myTable.ColumnModificationId);
        }
        
        /// assign NULL value
        public void SetModificationIdNull()
        {
            this.SetNull(this.myTable.ColumnModificationId);
        }
    }
}
