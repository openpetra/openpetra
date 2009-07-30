/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
namespace Ict.Petra.Shared.MPartner.Mailroom.Data
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
    
    
    /// Master file for extracts.  Contains names for the extract id
    [Serializable()]
    public class MExtractMasterTable : TTypedDataTable
    {
        
        /// Identifier for the extract
        public DataColumn ColumnExtractId;
        
        /// Short name for the extract to be used in filenames
        public DataColumn ColumnExtractName;
        
        /// This is a long description for the extract
        public DataColumn ColumnExtractDesc;
        
        /// 
        public DataColumn ColumnLastRef;
        
        /// 
        public DataColumn ColumnDeletable;
        
        /// The user can set the frozen field when the extract should not be updated.
        public DataColumn ColumnFrozen;
        
        /// 
        public DataColumn ColumnKeyCount;
        
        /// 
        public DataColumn ColumnPublic;
        
        /// Indicates that the extract has been edited by a user
        public DataColumn ColumnManualModication;
        
        /// Date the extract was manually modified
        public DataColumn ColumnManualModicationDate;
        
        /// Who made the last manual modification ?
        public DataColumn ColumnManualModBy;
        
        /// Indicate the extract type. Which function was the extract created through?
        public DataColumn ColumnExtractTypeCode;
        
        /// Is this extract just a template that has not yet been run?
        public DataColumn ColumnTemplate;
        
        /// Indicates whether or not the extract has restricted access. If it does then the access will be controlled by s_group_extract
        public DataColumn ColumnRestricted;
        
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
        public MExtractMasterTable() : 
                base("MExtractMaster")
        {
        }
        
        /// constructor
        public MExtractMasterTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public MExtractMasterTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public MExtractMasterRow this[int i]
        {
            get
            {
                return ((MExtractMasterRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetExtractIdDBName()
        {
            return "m_extract_id_i";
        }
        
        /// get help text for column
        public static string GetExtractIdHelp()
        {
            return "Identifier for the extract";
        }
        
        /// get label of column
        public static string GetExtractIdLabel()
        {
            return "Extract Id";
        }
        
        /// get display format for column
        public static short GetExtractIdLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetExtractNameDBName()
        {
            return "m_extract_name_c";
        }
        
        /// get help text for column
        public static string GetExtractNameHelp()
        {
            return "Short name for the extract to be used in filenames";
        }
        
        /// get label of column
        public static string GetExtractNameLabel()
        {
            return "Extract Name";
        }
        
        /// get character length for column
        public static short GetExtractNameLength()
        {
            return 25;
        }
        
        /// get the name of the field in the database for this column
        public static string GetExtractDescDBName()
        {
            return "m_extract_desc_c";
        }
        
        /// get help text for column
        public static string GetExtractDescHelp()
        {
            return "Enter a description";
        }
        
        /// get label of column
        public static string GetExtractDescLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetExtractDescLength()
        {
            return 80;
        }
        
        /// get the name of the field in the database for this column
        public static string GetLastRefDBName()
        {
            return "m_last_ref_d";
        }
        
        /// get help text for column
        public static string GetLastRefHelp()
        {
            return "Field automatically maintained only";
        }
        
        /// get label of column
        public static string GetLastRefLabel()
        {
            return "Last Referenced";
        }
        
        /// get display format for column
        public static short GetLastRefLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDeletableDBName()
        {
            return "m_deletable_l";
        }
        
        /// get help text for column
        public static string GetDeletableHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetDeletableLabel()
        {
            return "Deletable";
        }
        
        /// get display format for column
        public static short GetDeletableLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetFrozenDBName()
        {
            return "m_frozen_l";
        }
        
        /// get help text for column
        public static string GetFrozenHelp()
        {
            return "The user can set the frozen field when the extract should not be updated.";
        }
        
        /// get label of column
        public static string GetFrozenLabel()
        {
            return "Frozen";
        }
        
        /// get display format for column
        public static short GetFrozenLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetKeyCountDBName()
        {
            return "m_key_count_i";
        }
        
        /// get help text for column
        public static string GetKeyCountHelp()
        {
            return "Number of keys in this extract";
        }
        
        /// get label of column
        public static string GetKeyCountLabel()
        {
            return "Key Count";
        }
        
        /// get display format for column
        public static short GetKeyCountLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPublicDBName()
        {
            return "m_public_l";
        }
        
        /// get help text for column
        public static string GetPublicHelp()
        {
            return "Allow all users to see this extract?";
        }
        
        /// get label of column
        public static string GetPublicLabel()
        {
            return "Public";
        }
        
        /// get display format for column
        public static short GetPublicLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetManualModicationDBName()
        {
            return "m_manual_mod_l";
        }
        
        /// get help text for column
        public static string GetManualModicationHelp()
        {
            return "Enter \"\"Yes\"\" or \"\"No\"\"";
        }
        
        /// get label of column
        public static string GetManualModicationLabel()
        {
            return "m_manual_mod_l";
        }
        
        /// get display format for column
        public static short GetManualModicationLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetManualModicationDateDBName()
        {
            return "m_manual_mod_d";
        }
        
        /// get help text for column
        public static string GetManualModicationDateHelp()
        {
            return "Date the extract was manually modified";
        }
        
        /// get label of column
        public static string GetManualModicationDateLabel()
        {
            return "Date Edited";
        }
        
        /// get display format for column
        public static short GetManualModicationDateLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetManualModByDBName()
        {
            return "m_manual_mod_by_c";
        }
        
        /// get help text for column
        public static string GetManualModByHelp()
        {
            return "Who made the last manual modification ?";
        }
        
        /// get label of column
        public static string GetManualModByLabel()
        {
            return "Edited By";
        }
        
        /// get character length for column
        public static short GetManualModByLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetExtractTypeCodeDBName()
        {
            return "m_extract_type_code_c";
        }
        
        /// get help text for column
        public static string GetExtractTypeCodeHelp()
        {
            return "Indicate the extract type. Which function was the extract created through?";
        }
        
        /// get label of column
        public static string GetExtractTypeCodeLabel()
        {
            return "Extract Type";
        }
        
        /// get character length for column
        public static short GetExtractTypeCodeLength()
        {
            return 25;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTemplateDBName()
        {
            return "m_template_l";
        }
        
        /// get help text for column
        public static string GetTemplateHelp()
        {
            return "Has this extract not been run yet and is therefore just a template?";
        }
        
        /// get label of column
        public static string GetTemplateLabel()
        {
            return "Template";
        }
        
        /// get display format for column
        public static short GetTemplateLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetRestrictedDBName()
        {
            return "m_restricted_l";
        }
        
        /// get help text for column
        public static string GetRestrictedHelp()
        {
            return "Should access to this extract be restricted to some people?";
        }
        
        /// get label of column
        public static string GetRestrictedLabel()
        {
            return "Extract Restricted";
        }
        
        /// get display format for column
        public static short GetRestrictedLength()
        {
            return 6;
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
            return "MExtractMaster";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "m_extract_master";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Extract Master";
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
                    "m_extract_id_i",
                    "m_extract_name_c",
                    "m_extract_desc_c",
                    "m_last_ref_d",
                    "m_deletable_l",
                    "m_frozen_l",
                    "m_key_count_i",
                    "m_public_l",
                    "m_manual_mod_l",
                    "m_manual_mod_d",
                    "m_manual_mod_by_c",
                    "m_extract_type_code_c",
                    "m_template_l",
                    "m_restricted_l",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnExtractId = this.Columns["m_extract_id_i"];
            this.ColumnExtractName = this.Columns["m_extract_name_c"];
            this.ColumnExtractDesc = this.Columns["m_extract_desc_c"];
            this.ColumnLastRef = this.Columns["m_last_ref_d"];
            this.ColumnDeletable = this.Columns["m_deletable_l"];
            this.ColumnFrozen = this.Columns["m_frozen_l"];
            this.ColumnKeyCount = this.Columns["m_key_count_i"];
            this.ColumnPublic = this.Columns["m_public_l"];
            this.ColumnManualModication = this.Columns["m_manual_mod_l"];
            this.ColumnManualModicationDate = this.Columns["m_manual_mod_d"];
            this.ColumnManualModBy = this.Columns["m_manual_mod_by_c"];
            this.ColumnExtractTypeCode = this.Columns["m_extract_type_code_c"];
            this.ColumnTemplate = this.Columns["m_template_l"];
            this.ColumnRestricted = this.Columns["m_restricted_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnExtractId};
        }
        
        /// get typed set of changes
        public MExtractMasterTable GetChangesTyped()
        {
            return ((MExtractMasterTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public MExtractMasterRow NewRowTyped(bool AWithDefaultValues)
        {
            MExtractMasterRow ret = ((MExtractMasterRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public MExtractMasterRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new MExtractMasterRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("m_extract_id_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("m_extract_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("m_extract_desc_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("m_last_ref_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("m_deletable_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("m_frozen_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("m_key_count_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("m_public_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("m_manual_mod_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("m_manual_mod_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("m_manual_mod_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("m_extract_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("m_template_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("m_restricted_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnExtractId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnExtractName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 50);
            }
            if ((ACol == ColumnExtractDesc))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
            }
            if ((ACol == ColumnLastRef))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnDeletable))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnFrozen))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnKeyCount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnPublic))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnManualModication))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnManualModicationDate))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnManualModBy))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnExtractTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 50);
            }
            if ((ACol == ColumnTemplate))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnRestricted))
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
    
    /// Master file for extracts.  Contains names for the extract id
    [Serializable()]
    public class MExtractMasterRow : System.Data.DataRow
    {
        
        private MExtractMasterTable myTable;
        
        /// Constructor
        public MExtractMasterRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((MExtractMasterTable)(this.Table));
        }
        
        /// Identifier for the extract
        public Int32 ExtractId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnExtractId.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnExtractId) 
                            || (((Int32)(this[this.myTable.ColumnExtractId])) != value)))
                {
                    this[this.myTable.ColumnExtractId] = value;
                }
            }
        }
        
        /// Short name for the extract to be used in filenames
        public String ExtractName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnExtractName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnExtractName) 
                            || (((String)(this[this.myTable.ColumnExtractName])) != value)))
                {
                    this[this.myTable.ColumnExtractName] = value;
                }
            }
        }
        
        /// This is a long description for the extract
        public String ExtractDesc
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnExtractDesc.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnExtractDesc) 
                            || (((String)(this[this.myTable.ColumnExtractDesc])) != value)))
                {
                    this[this.myTable.ColumnExtractDesc] = value;
                }
            }
        }
        
        /// 
        public System.DateTime LastRef
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastRef.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLastRef) 
                            || (((System.DateTime)(this[this.myTable.ColumnLastRef])) != value)))
                {
                    this[this.myTable.ColumnLastRef] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime LastRefLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnLastRef], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime LastRefHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnLastRef.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// 
        public Boolean Deletable
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDeletable.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDeletable) 
                            || (((Boolean)(this[this.myTable.ColumnDeletable])) != value)))
                {
                    this[this.myTable.ColumnDeletable] = value;
                }
            }
        }
        
        /// The user can set the frozen field when the extract should not be updated.
        public Boolean Frozen
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFrozen.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFrozen) 
                            || (((Boolean)(this[this.myTable.ColumnFrozen])) != value)))
                {
                    this[this.myTable.ColumnFrozen] = value;
                }
            }
        }
        
        /// 
        public Int32 KeyCount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnKeyCount.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnKeyCount) 
                            || (((Int32)(this[this.myTable.ColumnKeyCount])) != value)))
                {
                    this[this.myTable.ColumnKeyCount] = value;
                }
            }
        }
        
        /// 
        public Boolean Public
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPublic.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPublic) 
                            || (((Boolean)(this[this.myTable.ColumnPublic])) != value)))
                {
                    this[this.myTable.ColumnPublic] = value;
                }
            }
        }
        
        /// Indicates that the extract has been edited by a user
        public Boolean ManualModication
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnManualModication.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnManualModication) 
                            || (((Boolean)(this[this.myTable.ColumnManualModication])) != value)))
                {
                    this[this.myTable.ColumnManualModication] = value;
                }
            }
        }
        
        /// Date the extract was manually modified
        public System.DateTime ManualModicationDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnManualModicationDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnManualModicationDate) 
                            || (((System.DateTime)(this[this.myTable.ColumnManualModicationDate])) != value)))
                {
                    this[this.myTable.ColumnManualModicationDate] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ManualModicationDateLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnManualModicationDate], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ManualModicationDateHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnManualModicationDate.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// Who made the last manual modification ?
        public String ManualModBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnManualModBy.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnManualModBy) 
                            || (((String)(this[this.myTable.ColumnManualModBy])) != value)))
                {
                    this[this.myTable.ColumnManualModBy] = value;
                }
            }
        }
        
        /// Indicate the extract type. Which function was the extract created through?
        public String ExtractTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnExtractTypeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnExtractTypeCode) 
                            || (((String)(this[this.myTable.ColumnExtractTypeCode])) != value)))
                {
                    this[this.myTable.ColumnExtractTypeCode] = value;
                }
            }
        }
        
        /// Is this extract just a template that has not yet been run?
        public Boolean Template
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTemplate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTemplate) 
                            || (((Boolean)(this[this.myTable.ColumnTemplate])) != value)))
                {
                    this[this.myTable.ColumnTemplate] = value;
                }
            }
        }
        
        /// Indicates whether or not the extract has restricted access. If it does then the access will be controlled by s_group_extract
        public Boolean Restricted
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRestricted.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRestricted) 
                            || (((Boolean)(this[this.myTable.ColumnRestricted])) != value)))
                {
                    this[this.myTable.ColumnRestricted] = value;
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
            this[this.myTable.ColumnExtractId.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnExtractName);
            this.SetNull(this.myTable.ColumnExtractDesc);
            this.SetNull(this.myTable.ColumnLastRef);
            this[this.myTable.ColumnDeletable.Ordinal] = true;
            this[this.myTable.ColumnFrozen.Ordinal] = false;
            this[this.myTable.ColumnKeyCount.Ordinal] = 0;
            this[this.myTable.ColumnPublic.Ordinal] = true;
            this[this.myTable.ColumnManualModication.Ordinal] = false;
            this.SetNull(this.myTable.ColumnManualModicationDate);
            this.SetNull(this.myTable.ColumnManualModBy);
            this.SetNull(this.myTable.ColumnExtractTypeCode);
            this[this.myTable.ColumnTemplate.Ordinal] = false;
            this[this.myTable.ColumnRestricted.Ordinal] = false;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsExtractNameNull()
        {
            return this.IsNull(this.myTable.ColumnExtractName);
        }
        
        /// assign NULL value
        public void SetExtractNameNull()
        {
            this.SetNull(this.myTable.ColumnExtractName);
        }
        
        /// test for NULL value
        public bool IsExtractDescNull()
        {
            return this.IsNull(this.myTable.ColumnExtractDesc);
        }
        
        /// assign NULL value
        public void SetExtractDescNull()
        {
            this.SetNull(this.myTable.ColumnExtractDesc);
        }
        
        /// test for NULL value
        public bool IsLastRefNull()
        {
            return this.IsNull(this.myTable.ColumnLastRef);
        }
        
        /// assign NULL value
        public void SetLastRefNull()
        {
            this.SetNull(this.myTable.ColumnLastRef);
        }
        
        /// test for NULL value
        public bool IsKeyCountNull()
        {
            return this.IsNull(this.myTable.ColumnKeyCount);
        }
        
        /// assign NULL value
        public void SetKeyCountNull()
        {
            this.SetNull(this.myTable.ColumnKeyCount);
        }
        
        /// test for NULL value
        public bool IsManualModicationDateNull()
        {
            return this.IsNull(this.myTable.ColumnManualModicationDate);
        }
        
        /// assign NULL value
        public void SetManualModicationDateNull()
        {
            this.SetNull(this.myTable.ColumnManualModicationDate);
        }
        
        /// test for NULL value
        public bool IsManualModByNull()
        {
            return this.IsNull(this.myTable.ColumnManualModBy);
        }
        
        /// assign NULL value
        public void SetManualModByNull()
        {
            this.SetNull(this.myTable.ColumnManualModBy);
        }
        
        /// test for NULL value
        public bool IsExtractTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnExtractTypeCode);
        }
        
        /// assign NULL value
        public void SetExtractTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnExtractTypeCode);
        }
        
        /// test for NULL value
        public bool IsTemplateNull()
        {
            return this.IsNull(this.myTable.ColumnTemplate);
        }
        
        /// assign NULL value
        public void SetTemplateNull()
        {
            this.SetNull(this.myTable.ColumnTemplate);
        }
        
        /// test for NULL value
        public bool IsRestrictedNull()
        {
            return this.IsNull(this.myTable.ColumnRestricted);
        }
        
        /// assign NULL value
        public void SetRestrictedNull()
        {
            this.SetNull(this.myTable.ColumnRestricted);
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
    
    /// Contains the list of partners in each mailing extract
    [Serializable()]
    public class MExtractTable : TTypedDataTable
    {
        
        /// Identifier for the extract
        public DataColumn ColumnExtractId;
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        
        /// This is the key that tell what site created the linked location
        public DataColumn ColumnSiteKey;
        
        /// 
        public DataColumn ColumnLocationKey;
        
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
        public MExtractTable() : 
                base("MExtract")
        {
        }
        
        /// constructor
        public MExtractTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public MExtractTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public MExtractRow this[int i]
        {
            get
            {
                return ((MExtractRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetExtractIdDBName()
        {
            return "m_extract_id_i";
        }
        
        /// get help text for column
        public static string GetExtractIdHelp()
        {
            return "Identifier for the extract";
        }
        
        /// get label of column
        public static string GetExtractIdLabel()
        {
            return "Extract Id";
        }
        
        /// get display format for column
        public static short GetExtractIdLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }
        
        /// get help text for column
        public static string GetPartnerKeyHelp()
        {
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get display format for column
        public static short GetPartnerKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetSiteKeyDBName()
        {
            return "p_site_key_n";
        }
        
        /// get help text for column
        public static string GetSiteKeyHelp()
        {
            return "Enter the site key";
        }
        
        /// get label of column
        public static string GetSiteKeyLabel()
        {
            return "Site Key";
        }
        
        /// get display format for column
        public static short GetSiteKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetLocationKeyDBName()
        {
            return "p_location_key_i";
        }
        
        /// get help text for column
        public static string GetLocationKeyHelp()
        {
            return "Selected Location Key for this Partner";
        }
        
        /// get label of column
        public static string GetLocationKeyLabel()
        {
            return "Location Key";
        }
        
        /// get display format for column
        public static short GetLocationKeyLength()
        {
            return 7;
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
            return "MExtract";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "m_extract";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Extract";
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
                    "m_extract_id_i",
                    "p_partner_key_n",
                    "p_site_key_n",
                    "p_location_key_i",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnExtractId = this.Columns["m_extract_id_i"];
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnSiteKey = this.Columns["p_site_key_n"];
            this.ColumnLocationKey = this.Columns["p_location_key_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnExtractId,
                    this.ColumnPartnerKey,
                    this.ColumnSiteKey};
        }
        
        /// get typed set of changes
        public MExtractTable GetChangesTyped()
        {
            return ((MExtractTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public MExtractRow NewRowTyped(bool AWithDefaultValues)
        {
            MExtractRow ret = ((MExtractRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public MExtractRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new MExtractRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("m_extract_id_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_site_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_location_key_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnExtractId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnSiteKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnLocationKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
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
    
    /// Contains the list of partners in each mailing extract
    [Serializable()]
    public class MExtractRow : System.Data.DataRow
    {
        
        private MExtractTable myTable;
        
        /// Constructor
        public MExtractRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((MExtractTable)(this.Table));
        }
        
        /// Identifier for the extract
        public Int32 ExtractId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnExtractId.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnExtractId) 
                            || (((Int32)(this[this.myTable.ColumnExtractId])) != value)))
                {
                    this[this.myTable.ColumnExtractId] = value;
                }
            }
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
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
        
        /// This is the key that tell what site created the linked location
        public Int64 SiteKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSiteKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSiteKey) 
                            || (((Int64)(this[this.myTable.ColumnSiteKey])) != value)))
                {
                    this[this.myTable.ColumnSiteKey] = value;
                }
            }
        }
        
        /// 
        public Int32 LocationKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLocationKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLocationKey) 
                            || (((Int32)(this[this.myTable.ColumnLocationKey])) != value)))
                {
                    this[this.myTable.ColumnLocationKey] = value;
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
            this[this.myTable.ColumnExtractId.Ordinal] = 0;
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this[this.myTable.ColumnSiteKey.Ordinal] = 0;
            this[this.myTable.ColumnLocationKey.Ordinal] = 0;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsLocationKeyNull()
        {
            return this.IsNull(this.myTable.ColumnLocationKey);
        }
        
        /// assign NULL value
        public void SetLocationKeyNull()
        {
            this.SetNull(this.myTable.ColumnLocationKey);
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
    
    /// Contains a list of extract type which is needed when extracts need to be rerun
    [Serializable()]
    public class MExtractTypeTable : TTypedDataTable
    {
        
        /// Extract Type Code
        public DataColumn ColumnCode;
        
        /// Function that is run to create the extract (4GL function, Delphi, etc.)
        public DataColumn ColumnFunction;
        
        /// Description of Extract Type
        public DataColumn ColumnDescription;
        
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
        public MExtractTypeTable() : 
                base("MExtractType")
        {
        }
        
        /// constructor
        public MExtractTypeTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public MExtractTypeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public MExtractTypeRow this[int i]
        {
            get
            {
                return ((MExtractTypeRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetCodeDBName()
        {
            return "m_code_c";
        }
        
        /// get help text for column
        public static string GetCodeHelp()
        {
            return "Enter the code for the extract type";
        }
        
        /// get label of column
        public static string GetCodeLabel()
        {
            return "Code";
        }
        
        /// get character length for column
        public static short GetCodeLength()
        {
            return 25;
        }
        
        /// get the name of the field in the database for this column
        public static string GetFunctionDBName()
        {
            return "m_function_c";
        }
        
        /// get help text for column
        public static string GetFunctionHelp()
        {
            return "Enter the code for the extract type";
        }
        
        /// get label of column
        public static string GetFunctionLabel()
        {
            return "Function";
        }
        
        /// get character length for column
        public static short GetFunctionLength()
        {
            return 250;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "m_description_c";
        }
        
        /// get help text for column
        public static string GetDescriptionHelp()
        {
            return "Enter a description for the extract type";
        }
        
        /// get label of column
        public static string GetDescriptionLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 100;
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
            return "MExtractType";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "m_extract_type";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Extract Type";
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
                    "m_code_c",
                    "m_function_c",
                    "m_description_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnCode = this.Columns["m_code_c"];
            this.ColumnFunction = this.Columns["m_function_c"];
            this.ColumnDescription = this.Columns["m_description_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnCode};
        }
        
        /// get typed set of changes
        public MExtractTypeTable GetChangesTyped()
        {
            return ((MExtractTypeTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public MExtractTypeRow NewRowTyped(bool AWithDefaultValues)
        {
            MExtractTypeRow ret = ((MExtractTypeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public MExtractTypeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new MExtractTypeRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("m_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("m_function_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("m_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 50);
            }
            if ((ACol == ColumnFunction))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 500);
            }
            if ((ACol == ColumnDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 200);
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
    
    /// Contains a list of extract type which is needed when extracts need to be rerun
    [Serializable()]
    public class MExtractTypeRow : System.Data.DataRow
    {
        
        private MExtractTypeTable myTable;
        
        /// Constructor
        public MExtractTypeRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((MExtractTypeTable)(this.Table));
        }
        
        /// Extract Type Code
        public String Code
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCode) 
                            || (((String)(this[this.myTable.ColumnCode])) != value)))
                {
                    this[this.myTable.ColumnCode] = value;
                }
            }
        }
        
        /// Function that is run to create the extract (4GL function, Delphi, etc.)
        public String Function
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFunction.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFunction) 
                            || (((String)(this[this.myTable.ColumnFunction])) != value)))
                {
                    this[this.myTable.ColumnFunction] = value;
                }
            }
        }
        
        /// Description of Extract Type
        public String Description
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDescription) 
                            || (((String)(this[this.myTable.ColumnDescription])) != value)))
                {
                    this[this.myTable.ColumnDescription] = value;
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
            this.SetNull(this.myTable.ColumnCode);
            this.SetNull(this.myTable.ColumnFunction);
            this.SetNull(this.myTable.ColumnDescription);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsFunctionNull()
        {
            return this.IsNull(this.myTable.ColumnFunction);
        }
        
        /// assign NULL value
        public void SetFunctionNull()
        {
            this.SetNull(this.myTable.ColumnFunction);
        }
        
        /// test for NULL value
        public bool IsDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnDescription);
        }
        
        /// assign NULL value
        public void SetDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnDescription);
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
    
    /// Contains a list of parameters that an extract was run with (so it can be rerun)
    [Serializable()]
    public class MExtractParameterTable : TTypedDataTable
    {
        
        /// Identifier for the extract
        public DataColumn ColumnExtractId;
        
        /// Extract Parameter Code
        public DataColumn ColumnParameterCode;
        
        /// Index for Parameter Value. Only relevant if a parameter is a list of values in which case a new index is used for each list item.
        public DataColumn ColumnValueIndex;
        
        /// Extract Parameter Value
        public DataColumn ColumnParameterValue;
        
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
        public MExtractParameterTable() : 
                base("MExtractParameter")
        {
        }
        
        /// constructor
        public MExtractParameterTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public MExtractParameterTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public MExtractParameterRow this[int i]
        {
            get
            {
                return ((MExtractParameterRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetExtractIdDBName()
        {
            return "m_extract_id_i";
        }
        
        /// get help text for column
        public static string GetExtractIdHelp()
        {
            return "Identifier for the extract";
        }
        
        /// get label of column
        public static string GetExtractIdLabel()
        {
            return "Extract Id";
        }
        
        /// get display format for column
        public static short GetExtractIdLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetParameterCodeDBName()
        {
            return "m_parameter_code_c";
        }
        
        /// get help text for column
        public static string GetParameterCodeHelp()
        {
            return "Extract Parameter Code";
        }
        
        /// get label of column
        public static string GetParameterCodeLabel()
        {
            return "Code";
        }
        
        /// get character length for column
        public static short GetParameterCodeLength()
        {
            return 25;
        }
        
        /// get the name of the field in the database for this column
        public static string GetValueIndexDBName()
        {
            return "m_value_index_i";
        }
        
        /// get help text for column
        public static string GetValueIndexHelp()
        {
            return "Index for Parameter Value. Only relevant if a parameter is a list of values in wh" +
                "ich case a new index is used for each list item.";
        }
        
        /// get label of column
        public static string GetValueIndexLabel()
        {
            return "Value Index";
        }
        
        /// get display format for column
        public static short GetValueIndexLength()
        {
            return 5;
        }
        
        /// get the name of the field in the database for this column
        public static string GetParameterValueDBName()
        {
            return "m_parameter_value_c";
        }
        
        /// get help text for column
        public static string GetParameterValueHelp()
        {
            return "Extract Parameter Value";
        }
        
        /// get label of column
        public static string GetParameterValueLabel()
        {
            return "Value";
        }
        
        /// get character length for column
        public static short GetParameterValueLength()
        {
            return 100;
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
            return "MExtractParameter";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "m_extract_parameter";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Extract Parameter";
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
                    "m_extract_id_i",
                    "m_parameter_code_c",
                    "m_value_index_i",
                    "m_parameter_value_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnExtractId = this.Columns["m_extract_id_i"];
            this.ColumnParameterCode = this.Columns["m_parameter_code_c"];
            this.ColumnValueIndex = this.Columns["m_value_index_i"];
            this.ColumnParameterValue = this.Columns["m_parameter_value_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnExtractId,
                    this.ColumnParameterCode,
                    this.ColumnValueIndex};
        }
        
        /// get typed set of changes
        public MExtractParameterTable GetChangesTyped()
        {
            return ((MExtractParameterTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public MExtractParameterRow NewRowTyped(bool AWithDefaultValues)
        {
            MExtractParameterRow ret = ((MExtractParameterRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public MExtractParameterRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new MExtractParameterRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("m_extract_id_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("m_parameter_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("m_value_index_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("m_parameter_value_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnExtractId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnParameterCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 50);
            }
            if ((ACol == ColumnValueIndex))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnParameterValue))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 200);
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
    
    /// Contains a list of parameters that an extract was run with (so it can be rerun)
    [Serializable()]
    public class MExtractParameterRow : System.Data.DataRow
    {
        
        private MExtractParameterTable myTable;
        
        /// Constructor
        public MExtractParameterRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((MExtractParameterTable)(this.Table));
        }
        
        /// Identifier for the extract
        public Int32 ExtractId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnExtractId.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnExtractId) 
                            || (((Int32)(this[this.myTable.ColumnExtractId])) != value)))
                {
                    this[this.myTable.ColumnExtractId] = value;
                }
            }
        }
        
        /// Extract Parameter Code
        public String ParameterCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnParameterCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnParameterCode) 
                            || (((String)(this[this.myTable.ColumnParameterCode])) != value)))
                {
                    this[this.myTable.ColumnParameterCode] = value;
                }
            }
        }
        
        /// Index for Parameter Value. Only relevant if a parameter is a list of values in which case a new index is used for each list item.
        public Int32 ValueIndex
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnValueIndex.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnValueIndex) 
                            || (((Int32)(this[this.myTable.ColumnValueIndex])) != value)))
                {
                    this[this.myTable.ColumnValueIndex] = value;
                }
            }
        }
        
        /// Extract Parameter Value
        public String ParameterValue
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnParameterValue.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnParameterValue) 
                            || (((String)(this[this.myTable.ColumnParameterValue])) != value)))
                {
                    this[this.myTable.ColumnParameterValue] = value;
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
            this[this.myTable.ColumnExtractId.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnParameterCode);
            this[this.myTable.ColumnValueIndex.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnParameterValue);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsParameterValueNull()
        {
            return this.IsNull(this.myTable.ColumnParameterValue);
        }
        
        /// assign NULL value
        public void SetParameterValueNull()
        {
            this.SetNull(this.myTable.ColumnParameterValue);
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
    
    /// Lists mailings that are being tracked.   When entering gifts, the mailing that motivated the gift can be indicated.
    [Serializable()]
    public class PMailingTable : TTypedDataTable
    {
        
        /// Mailing Code
        public DataColumn ColumnMailingCode;
        
        /// Mailing Description
        public DataColumn ColumnMailingDescription;
        
        /// Date Of Mailing
        public DataColumn ColumnMailingDate;
        
        /// This defines a motivation group.
        public DataColumn ColumnMotivationGroupCode;
        
        /// This defines the motivation detail within a motivation group.
        public DataColumn ColumnMotivationDetailCode;
        
        /// Cost of Mailing
        public DataColumn ColumnMailingCost;
        
        /// Gift amount attributed to this mailing
        public DataColumn ColumnMailingAttributedAmount;
        
        /// Indicates if the mailing is viewable in comboboxes where the user can select it
        public DataColumn ColumnViewable;
        
        /// Date until this mailing is viewable for users (if p_viewable_l is set)
        public DataColumn ColumnViewableUntil;
        
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
        public PMailingTable() : 
                base("PMailing")
        {
        }
        
        /// constructor
        public PMailingTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PMailingTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PMailingRow this[int i]
        {
            get
            {
                return ((PMailingRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetMailingCodeDBName()
        {
            return "p_mailing_code_c";
        }
        
        /// get help text for column
        public static string GetMailingCodeHelp()
        {
            return "Enter the mailing code";
        }
        
        /// get label of column
        public static string GetMailingCodeLabel()
        {
            return "Mailing Code";
        }
        
        /// get character length for column
        public static short GetMailingCodeLength()
        {
            return 25;
        }
        
        /// get the name of the field in the database for this column
        public static string GetMailingDescriptionDBName()
        {
            return "p_mailing_description_c";
        }
        
        /// get help text for column
        public static string GetMailingDescriptionHelp()
        {
            return "Mailing Description";
        }
        
        /// get label of column
        public static string GetMailingDescriptionLabel()
        {
            return "Mailing Description";
        }
        
        /// get character length for column
        public static short GetMailingDescriptionLength()
        {
            return 80;
        }
        
        /// get the name of the field in the database for this column
        public static string GetMailingDateDBName()
        {
            return "p_mailing_date_d";
        }
        
        /// get help text for column
        public static string GetMailingDateHelp()
        {
            return "Date Of Mailing";
        }
        
        /// get label of column
        public static string GetMailingDateLabel()
        {
            return "Mailing Date";
        }
        
        /// get display format for column
        public static short GetMailingDateLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetMotivationGroupCodeDBName()
        {
            return "a_motivation_group_code_c";
        }
        
        /// get help text for column
        public static string GetMotivationGroupCodeHelp()
        {
            return "Enter a motivation group code";
        }
        
        /// get label of column
        public static string GetMotivationGroupCodeLabel()
        {
            return "Motivation Group Code";
        }
        
        /// get character length for column
        public static short GetMotivationGroupCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetMotivationDetailCodeDBName()
        {
            return "a_motivation_detail_code_c";
        }
        
        /// get help text for column
        public static string GetMotivationDetailCodeHelp()
        {
            return "Enter a motivation detail code";
        }
        
        /// get label of column
        public static string GetMotivationDetailCodeLabel()
        {
            return "Motivation Detail Code";
        }
        
        /// get character length for column
        public static short GetMotivationDetailCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetMailingCostDBName()
        {
            return "p_mailing_cost_n";
        }
        
        /// get help text for column
        public static string GetMailingCostHelp()
        {
            return "Cost of Mailing";
        }
        
        /// get label of column
        public static string GetMailingCostLabel()
        {
            return "Mailing Cost";
        }
        
        /// get display format for column
        public static short GetMailingCostLength()
        {
            return 19;
        }
        
        /// get the name of the field in the database for this column
        public static string GetMailingAttributedAmountDBName()
        {
            return "p_mailing_attributed_amount_n";
        }
        
        /// get help text for column
        public static string GetMailingAttributedAmountHelp()
        {
            return "Gift amount attributed to this mailing";
        }
        
        /// get label of column
        public static string GetMailingAttributedAmountLabel()
        {
            return "Attributed Amount";
        }
        
        /// get display format for column
        public static short GetMailingAttributedAmountLength()
        {
            return 19;
        }
        
        /// get the name of the field in the database for this column
        public static string GetViewableDBName()
        {
            return "p_viewable_l";
        }
        
        /// get help text for column
        public static string GetViewableHelp()
        {
            return "Should this mailing be viewable to the users?";
        }
        
        /// get label of column
        public static string GetViewableLabel()
        {
            return "Viewable";
        }
        
        /// get display format for column
        public static short GetViewableLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetViewableUntilDBName()
        {
            return "p_viewable_until_d";
        }
        
        /// get help text for column
        public static string GetViewableUntilHelp()
        {
            return "Date until this mailing is viewable";
        }
        
        /// get label of column
        public static string GetViewableUntilLabel()
        {
            return "Viewable until Date";
        }
        
        /// get display format for column
        public static short GetViewableUntilLength()
        {
            return 11;
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
            return "PMailing";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_mailing";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Mailing";
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
                    "p_mailing_code_c",
                    "p_mailing_description_c",
                    "p_mailing_date_d",
                    "a_motivation_group_code_c",
                    "a_motivation_detail_code_c",
                    "p_mailing_cost_n",
                    "p_mailing_attributed_amount_n",
                    "p_viewable_l",
                    "p_viewable_until_d",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnMailingCode = this.Columns["p_mailing_code_c"];
            this.ColumnMailingDescription = this.Columns["p_mailing_description_c"];
            this.ColumnMailingDate = this.Columns["p_mailing_date_d"];
            this.ColumnMotivationGroupCode = this.Columns["a_motivation_group_code_c"];
            this.ColumnMotivationDetailCode = this.Columns["a_motivation_detail_code_c"];
            this.ColumnMailingCost = this.Columns["p_mailing_cost_n"];
            this.ColumnMailingAttributedAmount = this.Columns["p_mailing_attributed_amount_n"];
            this.ColumnViewable = this.Columns["p_viewable_l"];
            this.ColumnViewableUntil = this.Columns["p_viewable_until_d"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnMailingCode};
        }
        
        /// get typed set of changes
        public PMailingTable GetChangesTyped()
        {
            return ((PMailingTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PMailingRow NewRowTyped(bool AWithDefaultValues)
        {
            PMailingRow ret = ((PMailingRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PMailingRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PMailingRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_mailing_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_mailing_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_mailing_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_group_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_motivation_detail_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_mailing_cost_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("p_mailing_attributed_amount_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("p_viewable_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_viewable_until_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnMailingCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 50);
            }
            if ((ACol == ColumnMailingDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
            }
            if ((ACol == ColumnMailingDate))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnMotivationGroupCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnMotivationDetailCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnMailingCost))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 19);
            }
            if ((ACol == ColumnMailingAttributedAmount))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 19);
            }
            if ((ACol == ColumnViewable))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnViewableUntil))
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
    
    /// Lists mailings that are being tracked.   When entering gifts, the mailing that motivated the gift can be indicated.
    [Serializable()]
    public class PMailingRow : System.Data.DataRow
    {
        
        private PMailingTable myTable;
        
        /// Constructor
        public PMailingRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PMailingTable)(this.Table));
        }
        
        /// Mailing Code
        public String MailingCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMailingCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMailingCode) 
                            || (((String)(this[this.myTable.ColumnMailingCode])) != value)))
                {
                    this[this.myTable.ColumnMailingCode] = value;
                }
            }
        }
        
        /// Mailing Description
        public String MailingDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMailingDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMailingDescription) 
                            || (((String)(this[this.myTable.ColumnMailingDescription])) != value)))
                {
                    this[this.myTable.ColumnMailingDescription] = value;
                }
            }
        }
        
        /// Date Of Mailing
        public System.DateTime MailingDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMailingDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMailingDate) 
                            || (((System.DateTime)(this[this.myTable.ColumnMailingDate])) != value)))
                {
                    this[this.myTable.ColumnMailingDate] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime MailingDateLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnMailingDate], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime MailingDateHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnMailingDate.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// This defines a motivation group.
        public String MotivationGroupCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMotivationGroupCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMotivationGroupCode) 
                            || (((String)(this[this.myTable.ColumnMotivationGroupCode])) != value)))
                {
                    this[this.myTable.ColumnMotivationGroupCode] = value;
                }
            }
        }
        
        /// This defines the motivation detail within a motivation group.
        public String MotivationDetailCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMotivationDetailCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMotivationDetailCode) 
                            || (((String)(this[this.myTable.ColumnMotivationDetailCode])) != value)))
                {
                    this[this.myTable.ColumnMotivationDetailCode] = value;
                }
            }
        }
        
        /// Cost of Mailing
        public Decimal MailingCost
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMailingCost.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMailingCost) 
                            || (((Decimal)(this[this.myTable.ColumnMailingCost])) != value)))
                {
                    this[this.myTable.ColumnMailingCost] = value;
                }
            }
        }
        
        /// Gift amount attributed to this mailing
        public Decimal MailingAttributedAmount
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMailingAttributedAmount.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMailingAttributedAmount) 
                            || (((Decimal)(this[this.myTable.ColumnMailingAttributedAmount])) != value)))
                {
                    this[this.myTable.ColumnMailingAttributedAmount] = value;
                }
            }
        }
        
        /// Indicates if the mailing is viewable in comboboxes where the user can select it
        public Boolean Viewable
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnViewable.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnViewable) 
                            || (((Boolean)(this[this.myTable.ColumnViewable])) != value)))
                {
                    this[this.myTable.ColumnViewable] = value;
                }
            }
        }
        
        /// Date until this mailing is viewable for users (if p_viewable_l is set)
        public System.DateTime ViewableUntil
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnViewableUntil.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnViewableUntil) 
                            || (((System.DateTime)(this[this.myTable.ColumnViewableUntil])) != value)))
                {
                    this[this.myTable.ColumnViewableUntil] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ViewableUntilLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnViewableUntil], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ViewableUntilHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnViewableUntil.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
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
            this.SetNull(this.myTable.ColumnMailingCode);
            this.SetNull(this.myTable.ColumnMailingDescription);
            this.SetNull(this.myTable.ColumnMailingDate);
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
            this.SetNull(this.myTable.ColumnMotivationDetailCode);
            this[this.myTable.ColumnMailingCost.Ordinal] = 0;
            this[this.myTable.ColumnMailingAttributedAmount.Ordinal] = 0;
            this[this.myTable.ColumnViewable.Ordinal] = true;
            this.SetNull(this.myTable.ColumnViewableUntil);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsMailingDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnMailingDescription);
        }
        
        /// assign NULL value
        public void SetMailingDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnMailingDescription);
        }
        
        /// test for NULL value
        public bool IsMailingDateNull()
        {
            return this.IsNull(this.myTable.ColumnMailingDate);
        }
        
        /// assign NULL value
        public void SetMailingDateNull()
        {
            this.SetNull(this.myTable.ColumnMailingDate);
        }
        
        /// test for NULL value
        public bool IsMotivationGroupCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMotivationGroupCode);
        }
        
        /// assign NULL value
        public void SetMotivationGroupCodeNull()
        {
            this.SetNull(this.myTable.ColumnMotivationGroupCode);
        }
        
        /// test for NULL value
        public bool IsMotivationDetailCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMotivationDetailCode);
        }
        
        /// assign NULL value
        public void SetMotivationDetailCodeNull()
        {
            this.SetNull(this.myTable.ColumnMotivationDetailCode);
        }
        
        /// test for NULL value
        public bool IsMailingCostNull()
        {
            return this.IsNull(this.myTable.ColumnMailingCost);
        }
        
        /// assign NULL value
        public void SetMailingCostNull()
        {
            this.SetNull(this.myTable.ColumnMailingCost);
        }
        
        /// test for NULL value
        public bool IsMailingAttributedAmountNull()
        {
            return this.IsNull(this.myTable.ColumnMailingAttributedAmount);
        }
        
        /// assign NULL value
        public void SetMailingAttributedAmountNull()
        {
            this.SetNull(this.myTable.ColumnMailingAttributedAmount);
        }
        
        /// test for NULL value
        public bool IsViewableNull()
        {
            return this.IsNull(this.myTable.ColumnViewable);
        }
        
        /// assign NULL value
        public void SetViewableNull()
        {
            this.SetNull(this.myTable.ColumnViewable);
        }
        
        /// test for NULL value
        public bool IsViewableUntilNull()
        {
            return this.IsNull(this.myTable.ColumnViewableUntil);
        }
        
        /// assign NULL value
        public void SetViewableUntilNull()
        {
            this.SetNull(this.myTable.ColumnViewableUntil);
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
    
    /// This table contains the address layouts generally available for the user.
    [Serializable()]
    public class PAddressLayoutCodeTable : TTypedDataTable
    {
        
        /// Address Layout Code
        public DataColumn ColumnCode;
        
        /// Description for Address Layout Code
        public DataColumn ColumnDescription;
        
        /// Index for Display Order (to determine the display position of the layout in a list)
        public DataColumn ColumnDisplayIndex;
        
        /// Comment for Address Layout Code
        public DataColumn ColumnComment;
        
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
        public PAddressLayoutCodeTable() : 
                base("PAddressLayoutCode")
        {
        }
        
        /// constructor
        public PAddressLayoutCodeTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PAddressLayoutCodeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PAddressLayoutCodeRow this[int i]
        {
            get
            {
                return ((PAddressLayoutCodeRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetCodeDBName()
        {
            return "p_code_c";
        }
        
        /// get help text for column
        public static string GetCodeHelp()
        {
            return "Address Layout Code";
        }
        
        /// get label of column
        public static string GetCodeLabel()
        {
            return "Address Layout Code";
        }
        
        /// get character length for column
        public static short GetCodeLength()
        {
            return 16;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "p_description_c";
        }
        
        /// get help text for column
        public static string GetDescriptionHelp()
        {
            return "Enter Description for Address Layout Code";
        }
        
        /// get label of column
        public static string GetDescriptionLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 50;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDisplayIndexDBName()
        {
            return "p_display_index_i";
        }
        
        /// get help text for column
        public static string GetDisplayIndexHelp()
        {
            return "Enter Index for Display Order of Address Layout Codes";
        }
        
        /// get label of column
        public static string GetDisplayIndexLabel()
        {
            return "Display Order Index";
        }
        
        /// get display format for column
        public static short GetDisplayIndexLength()
        {
            return 3;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCommentDBName()
        {
            return "p_comment_c";
        }
        
        /// get help text for column
        public static string GetCommentHelp()
        {
            return "Enter Comment for Address Layout Code";
        }
        
        /// get label of column
        public static string GetCommentLabel()
        {
            return "Comment";
        }
        
        /// get character length for column
        public static short GetCommentLength()
        {
            return 300;
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
            return "PAddressLayoutCode";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_address_layout_code";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Address Layout Code";
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
                    "p_code_c",
                    "p_description_c",
                    "p_display_index_i",
                    "p_comment_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnCode = this.Columns["p_code_c"];
            this.ColumnDescription = this.Columns["p_description_c"];
            this.ColumnDisplayIndex = this.Columns["p_display_index_i"];
            this.ColumnComment = this.Columns["p_comment_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnCode};
        }
        
        /// get typed set of changes
        public PAddressLayoutCodeTable GetChangesTyped()
        {
            return ((PAddressLayoutCodeTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PAddressLayoutCodeRow NewRowTyped(bool AWithDefaultValues)
        {
            PAddressLayoutCodeRow ret = ((PAddressLayoutCodeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PAddressLayoutCodeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PAddressLayoutCodeRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_display_index_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_comment_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 32);
            }
            if ((ACol == ColumnDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 100);
            }
            if ((ACol == ColumnDisplayIndex))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnComment))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 600);
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
    
    /// This table contains the address layouts generally available for the user.
    [Serializable()]
    public class PAddressLayoutCodeRow : System.Data.DataRow
    {
        
        private PAddressLayoutCodeTable myTable;
        
        /// Constructor
        public PAddressLayoutCodeRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PAddressLayoutCodeTable)(this.Table));
        }
        
        /// Address Layout Code
        public String Code
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCode) 
                            || (((String)(this[this.myTable.ColumnCode])) != value)))
                {
                    this[this.myTable.ColumnCode] = value;
                }
            }
        }
        
        /// Description for Address Layout Code
        public String Description
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDescription) 
                            || (((String)(this[this.myTable.ColumnDescription])) != value)))
                {
                    this[this.myTable.ColumnDescription] = value;
                }
            }
        }
        
        /// Index for Display Order (to determine the display position of the layout in a list)
        public Int32 DisplayIndex
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDisplayIndex.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDisplayIndex) 
                            || (((Int32)(this[this.myTable.ColumnDisplayIndex])) != value)))
                {
                    this[this.myTable.ColumnDisplayIndex] = value;
                }
            }
        }
        
        /// Comment for Address Layout Code
        public String Comment
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnComment.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnComment) 
                            || (((String)(this[this.myTable.ColumnComment])) != value)))
                {
                    this[this.myTable.ColumnComment] = value;
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
            this.SetNull(this.myTable.ColumnCode);
            this.SetNull(this.myTable.ColumnDescription);
            this.SetNull(this.myTable.ColumnDisplayIndex);
            this.SetNull(this.myTable.ColumnComment);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnDescription);
        }
        
        /// assign NULL value
        public void SetDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnDescription);
        }
        
        /// test for NULL value
        public bool IsCommentNull()
        {
            return this.IsNull(this.myTable.ColumnComment);
        }
        
        /// assign NULL value
        public void SetCommentNull()
        {
            this.SetNull(this.myTable.ColumnComment);
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
    
    /// This table contains the address lines used in laying out an address. Eg a form letter address layout
    [Serializable()]
    public class PAddressLayoutTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnCountryCode;
        
        /// 
        public DataColumn ColumnAddressLayoutCode;
        
        /// 
        public DataColumn ColumnAddressLineNumber;
        
        /// 
        public DataColumn ColumnAddressLineCode;
        
        /// This field is a short description of the Address Line Code record
        public DataColumn ColumnAddressPrompt;
        
        /// System flag indicates a lock is on the record
        public DataColumn ColumnLocked;
        
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
        public PAddressLayoutTable() : 
                base("PAddressLayout")
        {
        }
        
        /// constructor
        public PAddressLayoutTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PAddressLayoutTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PAddressLayoutRow this[int i]
        {
            get
            {
                return ((PAddressLayoutRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetCountryCodeDBName()
        {
            return "p_country_code_c";
        }
        
        /// get help text for column
        public static string GetCountryCodeHelp()
        {
            return "Enter the Country Code";
        }
        
        /// get label of column
        public static string GetCountryCodeLabel()
        {
            return "Country Code";
        }
        
        /// get character length for column
        public static short GetCountryCodeLength()
        {
            return 4;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddressLayoutCodeDBName()
        {
            return "p_address_layout_code_c";
        }
        
        /// get help text for column
        public static string GetAddressLayoutCodeHelp()
        {
            return "Address Layout Code";
        }
        
        /// get label of column
        public static string GetAddressLayoutCodeLabel()
        {
            return "Address Layout Code";
        }
        
        /// get character length for column
        public static short GetAddressLayoutCodeLength()
        {
            return 16;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddressLineNumberDBName()
        {
            return "p_address_line_number_i";
        }
        
        /// get help text for column
        public static string GetAddressLineNumberHelp()
        {
            return "Line Number";
        }
        
        /// get label of column
        public static string GetAddressLineNumberLabel()
        {
            return "Line Number";
        }
        
        /// get display format for column
        public static short GetAddressLineNumberLength()
        {
            return 2;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddressLineCodeDBName()
        {
            return "p_address_line_code_c";
        }
        
        /// get help text for column
        public static string GetAddressLineCodeHelp()
        {
            return "Address Line Code";
        }
        
        /// get label of column
        public static string GetAddressLineCodeLabel()
        {
            return "Address Line Code";
        }
        
        /// get character length for column
        public static short GetAddressLineCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddressPromptDBName()
        {
            return "p_address_prompt_c";
        }
        
        /// get help text for column
        public static string GetAddressPromptHelp()
        {
            return "Prompt text for Address Line Code";
        }
        
        /// get label of column
        public static string GetAddressPromptLabel()
        {
            return "Address Prompt";
        }
        
        /// get character length for column
        public static short GetAddressPromptLength()
        {
            return 15;
        }
        
        /// get the name of the field in the database for this column
        public static string GetLockedDBName()
        {
            return "p_locked_l";
        }
        
        /// get help text for column
        public static string GetLockedHelp()
        {
            return "System flag indicates a lock is on the record";
        }
        
        /// get label of column
        public static string GetLockedLabel()
        {
            return "p_locked_l";
        }
        
        /// get display format for column
        public static short GetLockedLength()
        {
            return 6;
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
            return "PAddressLayout";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_address_layout";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Address Layout";
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
                    "p_country_code_c",
                    "p_address_layout_code_c",
                    "p_address_line_number_i",
                    "p_address_line_code_c",
                    "p_address_prompt_c",
                    "p_locked_l",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnCountryCode = this.Columns["p_country_code_c"];
            this.ColumnAddressLayoutCode = this.Columns["p_address_layout_code_c"];
            this.ColumnAddressLineNumber = this.Columns["p_address_line_number_i"];
            this.ColumnAddressLineCode = this.Columns["p_address_line_code_c"];
            this.ColumnAddressPrompt = this.Columns["p_address_prompt_c"];
            this.ColumnLocked = this.Columns["p_locked_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnCountryCode,
                    this.ColumnAddressLayoutCode,
                    this.ColumnAddressLineNumber,
                    this.ColumnAddressLineCode};
        }
        
        /// get typed set of changes
        public PAddressLayoutTable GetChangesTyped()
        {
            return ((PAddressLayoutTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PAddressLayoutRow NewRowTyped(bool AWithDefaultValues)
        {
            PAddressLayoutRow ret = ((PAddressLayoutRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PAddressLayoutRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PAddressLayoutRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_country_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_address_layout_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_address_line_number_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_address_line_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_address_prompt_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_locked_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnCountryCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 8);
            }
            if ((ACol == ColumnAddressLayoutCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 32);
            }
            if ((ACol == ColumnAddressLineNumber))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAddressLineCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnAddressPrompt))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 30);
            }
            if ((ACol == ColumnLocked))
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
    
    /// This table contains the address lines used in laying out an address. Eg a form letter address layout
    [Serializable()]
    public class PAddressLayoutRow : System.Data.DataRow
    {
        
        private PAddressLayoutTable myTable;
        
        /// Constructor
        public PAddressLayoutRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PAddressLayoutTable)(this.Table));
        }
        
        /// 
        public String CountryCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCountryCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCountryCode) 
                            || (((String)(this[this.myTable.ColumnCountryCode])) != value)))
                {
                    this[this.myTable.ColumnCountryCode] = value;
                }
            }
        }
        
        /// 
        public String AddressLayoutCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressLayoutCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddressLayoutCode) 
                            || (((String)(this[this.myTable.ColumnAddressLayoutCode])) != value)))
                {
                    this[this.myTable.ColumnAddressLayoutCode] = value;
                }
            }
        }
        
        /// 
        public Int32 AddressLineNumber
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressLineNumber.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddressLineNumber) 
                            || (((Int32)(this[this.myTable.ColumnAddressLineNumber])) != value)))
                {
                    this[this.myTable.ColumnAddressLineNumber] = value;
                }
            }
        }
        
        /// 
        public String AddressLineCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressLineCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddressLineCode) 
                            || (((String)(this[this.myTable.ColumnAddressLineCode])) != value)))
                {
                    this[this.myTable.ColumnAddressLineCode] = value;
                }
            }
        }
        
        /// This field is a short description of the Address Line Code record
        public String AddressPrompt
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressPrompt.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddressPrompt) 
                            || (((String)(this[this.myTable.ColumnAddressPrompt])) != value)))
                {
                    this[this.myTable.ColumnAddressPrompt] = value;
                }
            }
        }
        
        /// System flag indicates a lock is on the record
        public Boolean Locked
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLocked.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLocked) 
                            || (((Boolean)(this[this.myTable.ColumnLocked])) != value)))
                {
                    this[this.myTable.ColumnLocked] = value;
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
            this.SetNull(this.myTable.ColumnCountryCode);
            this[this.myTable.ColumnAddressLayoutCode.Ordinal] = "SmlLabel";
            this[this.myTable.ColumnAddressLineNumber.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnAddressLineCode);
            this.SetNull(this.myTable.ColumnAddressPrompt);
            this[this.myTable.ColumnLocked.Ordinal] = false;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsAddressPromptNull()
        {
            return this.IsNull(this.myTable.ColumnAddressPrompt);
        }
        
        /// assign NULL value
        public void SetAddressPromptNull()
        {
            this.SetNull(this.myTable.ColumnAddressPrompt);
        }
        
        /// test for NULL value
        public bool IsLockedNull()
        {
            return this.IsNull(this.myTable.ColumnLocked);
        }
        
        /// assign NULL value
        public void SetLockedNull()
        {
            this.SetNull(this.myTable.ColumnLocked);
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
    
    /// This contains the elements which make up an address. Eg Name etc
    [Serializable()]
    public class PAddressElementTable : TTypedDataTable
    {
        
        /// This Code is used to identify the address element.
        public DataColumn ColumnAddressElementCode;
        
        /// 
        public DataColumn ColumnAddressElementDescription;
        
        /// Address element field name
        public DataColumn ColumnAddressElementFieldName;
        
        /// This is usually a &quot;&quot;.&quot;&quot; or a &quot;&quot;;&quot;&quot; or a &quot;&quot;,&quot;&quot; etc
        public DataColumn ColumnAddressElementText;
        
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
        public PAddressElementTable() : 
                base("PAddressElement")
        {
        }
        
        /// constructor
        public PAddressElementTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PAddressElementTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PAddressElementRow this[int i]
        {
            get
            {
                return ((PAddressElementRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddressElementCodeDBName()
        {
            return "p_address_element_code_c";
        }
        
        /// get help text for column
        public static string GetAddressElementCodeHelp()
        {
            return "Code used to identify the address element";
        }
        
        /// get label of column
        public static string GetAddressElementCodeLabel()
        {
            return "Element Code";
        }
        
        /// get character length for column
        public static short GetAddressElementCodeLength()
        {
            return 12;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddressElementDescriptionDBName()
        {
            return "p_address_element_description_c";
        }
        
        /// get help text for column
        public static string GetAddressElementDescriptionHelp()
        {
            return "Description for address element code";
        }
        
        /// get label of column
        public static string GetAddressElementDescriptionLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetAddressElementDescriptionLength()
        {
            return 80;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddressElementFieldNameDBName()
        {
            return "p_address_element_field_name_c";
        }
        
        /// get help text for column
        public static string GetAddressElementFieldNameHelp()
        {
            return "Enter the address element field name";
        }
        
        /// get label of column
        public static string GetAddressElementFieldNameLabel()
        {
            return "Field Name";
        }
        
        /// get character length for column
        public static short GetAddressElementFieldNameLength()
        {
            return 30;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddressElementTextDBName()
        {
            return "p_address_element_text_c";
        }
        
        /// get help text for column
        public static string GetAddressElementTextHelp()
        {
            return "Enter the address element text, usually a \"\".\"\" or a \"\";\"\" or a \"\",\"\"";
        }
        
        /// get label of column
        public static string GetAddressElementTextLabel()
        {
            return "Element Text";
        }
        
        /// get character length for column
        public static short GetAddressElementTextLength()
        {
            return 1;
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
            return "PAddressElement";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_address_element";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Address Element";
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
                    "p_address_element_code_c",
                    "p_address_element_description_c",
                    "p_address_element_field_name_c",
                    "p_address_element_text_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnAddressElementCode = this.Columns["p_address_element_code_c"];
            this.ColumnAddressElementDescription = this.Columns["p_address_element_description_c"];
            this.ColumnAddressElementFieldName = this.Columns["p_address_element_field_name_c"];
            this.ColumnAddressElementText = this.Columns["p_address_element_text_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnAddressElementCode};
        }
        
        /// get typed set of changes
        public PAddressElementTable GetChangesTyped()
        {
            return ((PAddressElementTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PAddressElementRow NewRowTyped(bool AWithDefaultValues)
        {
            PAddressElementRow ret = ((PAddressElementRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PAddressElementRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PAddressElementRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_address_element_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_address_element_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_address_element_field_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_address_element_text_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnAddressElementCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnAddressElementDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
            }
            if ((ACol == ColumnAddressElementFieldName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 60);
            }
            if ((ACol == ColumnAddressElementText))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 2);
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
    
    /// This contains the elements which make up an address. Eg Name etc
    [Serializable()]
    public class PAddressElementRow : System.Data.DataRow
    {
        
        private PAddressElementTable myTable;
        
        /// Constructor
        public PAddressElementRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PAddressElementTable)(this.Table));
        }
        
        /// This Code is used to identify the address element.
        public String AddressElementCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressElementCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddressElementCode) 
                            || (((String)(this[this.myTable.ColumnAddressElementCode])) != value)))
                {
                    this[this.myTable.ColumnAddressElementCode] = value;
                }
            }
        }
        
        /// 
        public String AddressElementDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressElementDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddressElementDescription) 
                            || (((String)(this[this.myTable.ColumnAddressElementDescription])) != value)))
                {
                    this[this.myTable.ColumnAddressElementDescription] = value;
                }
            }
        }
        
        /// Address element field name
        public String AddressElementFieldName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressElementFieldName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddressElementFieldName) 
                            || (((String)(this[this.myTable.ColumnAddressElementFieldName])) != value)))
                {
                    this[this.myTable.ColumnAddressElementFieldName] = value;
                }
            }
        }
        
        /// This is usually a &quot;&quot;.&quot;&quot; or a &quot;&quot;;&quot;&quot; or a &quot;&quot;,&quot;&quot; etc
        public String AddressElementText
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressElementText.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddressElementText) 
                            || (((String)(this[this.myTable.ColumnAddressElementText])) != value)))
                {
                    this[this.myTable.ColumnAddressElementText] = value;
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
            this.SetNull(this.myTable.ColumnAddressElementCode);
            this.SetNull(this.myTable.ColumnAddressElementDescription);
            this.SetNull(this.myTable.ColumnAddressElementFieldName);
            this.SetNull(this.myTable.ColumnAddressElementText);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsAddressElementDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnAddressElementDescription);
        }
        
        /// assign NULL value
        public void SetAddressElementDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnAddressElementDescription);
        }
        
        /// test for NULL value
        public bool IsAddressElementFieldNameNull()
        {
            return this.IsNull(this.myTable.ColumnAddressElementFieldName);
        }
        
        /// assign NULL value
        public void SetAddressElementFieldNameNull()
        {
            this.SetNull(this.myTable.ColumnAddressElementFieldName);
        }
        
        /// test for NULL value
        public bool IsAddressElementTextNull()
        {
            return this.IsNull(this.myTable.ColumnAddressElementText);
        }
        
        /// assign NULL value
        public void SetAddressElementTextNull()
        {
            this.SetNull(this.myTable.ColumnAddressElementText);
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
    
    /// This is an address line which consists of address elements.  Used along with p_address_layout and p_address_element to define layout of an address for different countries.
    [Serializable()]
    public class PAddressLineTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnAddressLineCode;
        
        /// This is the column number where the element field or text should be placed.
        public DataColumn ColumnAddressElementPosition;
        
        /// This Code is used to identify the address element.
        public DataColumn ColumnAddressElementCode;
        
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
        public PAddressLineTable() : 
                base("PAddressLine")
        {
        }
        
        /// constructor
        public PAddressLineTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PAddressLineTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PAddressLineRow this[int i]
        {
            get
            {
                return ((PAddressLineRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddressLineCodeDBName()
        {
            return "p_address_line_code_c";
        }
        
        /// get help text for column
        public static string GetAddressLineCodeHelp()
        {
            return "Address Line Code";
        }
        
        /// get label of column
        public static string GetAddressLineCodeLabel()
        {
            return "Address Line Code";
        }
        
        /// get character length for column
        public static short GetAddressLineCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddressElementPositionDBName()
        {
            return "p_address_element_position_i";
        }
        
        /// get help text for column
        public static string GetAddressElementPositionHelp()
        {
            return "Column number where address element should be placed";
        }
        
        /// get label of column
        public static string GetAddressElementPositionLabel()
        {
            return "Element Postition";
        }
        
        /// get display format for column
        public static short GetAddressElementPositionLength()
        {
            return 3;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddressElementCodeDBName()
        {
            return "p_address_element_code_c";
        }
        
        /// get help text for column
        public static string GetAddressElementCodeHelp()
        {
            return "Enter an element code";
        }
        
        /// get label of column
        public static string GetAddressElementCodeLabel()
        {
            return "Element Code";
        }
        
        /// get character length for column
        public static short GetAddressElementCodeLength()
        {
            return 12;
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
            return "PAddressLine";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_address_line";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Address Line";
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
                    "p_address_line_code_c",
                    "p_address_element_position_i",
                    "p_address_element_code_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnAddressLineCode = this.Columns["p_address_line_code_c"];
            this.ColumnAddressElementPosition = this.Columns["p_address_element_position_i"];
            this.ColumnAddressElementCode = this.Columns["p_address_element_code_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnAddressLineCode,
                    this.ColumnAddressElementPosition};
        }
        
        /// get typed set of changes
        public PAddressLineTable GetChangesTyped()
        {
            return ((PAddressLineTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PAddressLineRow NewRowTyped(bool AWithDefaultValues)
        {
            PAddressLineRow ret = ((PAddressLineRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PAddressLineRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PAddressLineRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_address_line_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_address_element_position_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_address_element_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnAddressLineCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnAddressElementPosition))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnAddressElementCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
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
    
    /// This is an address line which consists of address elements.  Used along with p_address_layout and p_address_element to define layout of an address for different countries.
    [Serializable()]
    public class PAddressLineRow : System.Data.DataRow
    {
        
        private PAddressLineTable myTable;
        
        /// Constructor
        public PAddressLineRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PAddressLineTable)(this.Table));
        }
        
        /// 
        public String AddressLineCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressLineCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddressLineCode) 
                            || (((String)(this[this.myTable.ColumnAddressLineCode])) != value)))
                {
                    this[this.myTable.ColumnAddressLineCode] = value;
                }
            }
        }
        
        /// This is the column number where the element field or text should be placed.
        public Int32 AddressElementPosition
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressElementPosition.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddressElementPosition) 
                            || (((Int32)(this[this.myTable.ColumnAddressElementPosition])) != value)))
                {
                    this[this.myTable.ColumnAddressElementPosition] = value;
                }
            }
        }
        
        /// This Code is used to identify the address element.
        public String AddressElementCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressElementCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddressElementCode) 
                            || (((String)(this[this.myTable.ColumnAddressElementCode])) != value)))
                {
                    this[this.myTable.ColumnAddressElementCode] = value;
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
            this.SetNull(this.myTable.ColumnAddressLineCode);
            this[this.myTable.ColumnAddressElementPosition.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnAddressElementCode);
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
    
    /// This is used to override titles that might be different in the address than that in the letter.
    ///Eg      German     Herr   Herrn
    ///&quot;&quot;Sehr geehrter Herr Starling&quot;&quot; in the letter and &quot;&quot;Herrn Starling&quot;&quot; in the address.
    [Serializable()]
    public class PAddresseeTitleOverrideTable : TTypedDataTable
    {
        
        /// This is the code used to identify a language.
        public DataColumn ColumnLanguageCode;
        
        /// The partner's title
        public DataColumn ColumnTitle;
        
        /// The title to override the partner
        public DataColumn ColumnTitleOverride;
        
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
        public PAddresseeTitleOverrideTable() : 
                base("PAddresseeTitleOverride")
        {
        }
        
        /// constructor
        public PAddresseeTitleOverrideTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PAddresseeTitleOverrideTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PAddresseeTitleOverrideRow this[int i]
        {
            get
            {
                return ((PAddresseeTitleOverrideRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetLanguageCodeDBName()
        {
            return "p_language_code_c";
        }
        
        /// get help text for column
        public static string GetLanguageCodeHelp()
        {
            return "Enter the language code";
        }
        
        /// get label of column
        public static string GetLanguageCodeLabel()
        {
            return "Language Code";
        }
        
        /// get character length for column
        public static short GetLanguageCodeLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTitleDBName()
        {
            return "p_title_c";
        }
        
        /// get help text for column
        public static string GetTitleHelp()
        {
            return "Enter the partner\'s title";
        }
        
        /// get label of column
        public static string GetTitleLabel()
        {
            return "Title";
        }
        
        /// get character length for column
        public static short GetTitleLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTitleOverrideDBName()
        {
            return "p_title_override_c";
        }
        
        /// get help text for column
        public static string GetTitleOverrideHelp()
        {
            return "Enter the override title";
        }
        
        /// get label of column
        public static string GetTitleOverrideLabel()
        {
            return "Title Override";
        }
        
        /// get character length for column
        public static short GetTitleOverrideLength()
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
            return "PAddresseeTitleOverride";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_addressee_title_override";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Address Title Override";
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
                    "p_language_code_c",
                    "p_title_c",
                    "p_title_override_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLanguageCode = this.Columns["p_language_code_c"];
            this.ColumnTitle = this.Columns["p_title_c"];
            this.ColumnTitleOverride = this.Columns["p_title_override_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLanguageCode,
                    this.ColumnTitle};
        }
        
        /// get typed set of changes
        public PAddresseeTitleOverrideTable GetChangesTyped()
        {
            return ((PAddresseeTitleOverrideTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PAddresseeTitleOverrideRow NewRowTyped(bool AWithDefaultValues)
        {
            PAddresseeTitleOverrideRow ret = ((PAddresseeTitleOverrideRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PAddresseeTitleOverrideRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PAddresseeTitleOverrideRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_language_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_title_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_title_override_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLanguageCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnTitle))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
            }
            if ((ACol == ColumnTitleOverride))
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
    
    /// This is used to override titles that might be different in the address than that in the letter.
    ///Eg      German     Herr   Herrn
    ///&quot;&quot;Sehr geehrter Herr Starling&quot;&quot; in the letter and &quot;&quot;Herrn Starling&quot;&quot; in the address.
    [Serializable()]
    public class PAddresseeTitleOverrideRow : System.Data.DataRow
    {
        
        private PAddresseeTitleOverrideTable myTable;
        
        /// Constructor
        public PAddresseeTitleOverrideRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PAddresseeTitleOverrideTable)(this.Table));
        }
        
        /// This is the code used to identify a language.
        public String LanguageCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLanguageCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLanguageCode) 
                            || (((String)(this[this.myTable.ColumnLanguageCode])) != value)))
                {
                    this[this.myTable.ColumnLanguageCode] = value;
                }
            }
        }
        
        /// The partner's title
        public String Title
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTitle.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTitle) 
                            || (((String)(this[this.myTable.ColumnTitle])) != value)))
                {
                    this[this.myTable.ColumnTitle] = value;
                }
            }
        }
        
        /// The title to override the partner
        public String TitleOverride
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTitleOverride.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTitleOverride) 
                            || (((String)(this[this.myTable.ColumnTitleOverride])) != value)))
                {
                    this[this.myTable.ColumnTitleOverride] = value;
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
            this[this.myTable.ColumnLanguageCode.Ordinal] = "99";
            this.SetNull(this.myTable.ColumnTitle);
            this.SetNull(this.myTable.ColumnTitleOverride);
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
    
    /// Specific greetings from a user to a partner
    [Serializable()]
    public class PCustomisedGreetingTable : TTypedDataTable
    {
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        
        /// This is the system user id. Each user of the system is allocated one
        public DataColumn ColumnUserId;
        
        /// 
        public DataColumn ColumnCustomisedGreetingText;
        
        /// 
        public DataColumn ColumnCustomisedClosingText;
        
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
        public PCustomisedGreetingTable() : 
                base("PCustomisedGreeting")
        {
        }
        
        /// constructor
        public PCustomisedGreetingTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PCustomisedGreetingTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PCustomisedGreetingRow this[int i]
        {
            get
            {
                return ((PCustomisedGreetingRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }
        
        /// get help text for column
        public static string GetPartnerKeyHelp()
        {
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get display format for column
        public static short GetPartnerKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetUserIdDBName()
        {
            return "s_user_id_c";
        }
        
        /// get help text for column
        public static string GetUserIdHelp()
        {
            return "This is the system user id. Each user of the system is allocated one";
        }
        
        /// get label of column
        public static string GetUserIdLabel()
        {
            return "User ID";
        }
        
        /// get character length for column
        public static short GetUserIdLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCustomisedGreetingTextDBName()
        {
            return "p_customised_greeting_text_c";
        }
        
        /// get help text for column
        public static string GetCustomisedGreetingTextHelp()
        {
            return "Enter a customised greeting";
        }
        
        /// get label of column
        public static string GetCustomisedGreetingTextLabel()
        {
            return "Customised Greeting";
        }
        
        /// get character length for column
        public static short GetCustomisedGreetingTextLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCustomisedClosingTextDBName()
        {
            return "p_customised_closing_text_c";
        }
        
        /// get help text for column
        public static string GetCustomisedClosingTextHelp()
        {
            return "Enter a customised greeting";
        }
        
        /// get label of column
        public static string GetCustomisedClosingTextLabel()
        {
            return "Customised Closing";
        }
        
        /// get character length for column
        public static short GetCustomisedClosingTextLength()
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
            return "PCustomisedGreeting";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_customised_greeting";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Customised Greeting";
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
                    "p_partner_key_n",
                    "s_user_id_c",
                    "p_customised_greeting_text_c",
                    "p_customised_closing_text_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnUserId = this.Columns["s_user_id_c"];
            this.ColumnCustomisedGreetingText = this.Columns["p_customised_greeting_text_c"];
            this.ColumnCustomisedClosingText = this.Columns["p_customised_closing_text_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnPartnerKey,
                    this.ColumnUserId};
        }
        
        /// get typed set of changes
        public PCustomisedGreetingTable GetChangesTyped()
        {
            return ((PCustomisedGreetingTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PCustomisedGreetingRow NewRowTyped(bool AWithDefaultValues)
        {
            PCustomisedGreetingRow ret = ((PCustomisedGreetingRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PCustomisedGreetingRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PCustomisedGreetingRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("s_user_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_customised_greeting_text_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_customised_closing_text_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnUserId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnCustomisedGreetingText))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
            }
            if ((ACol == ColumnCustomisedClosingText))
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
    
    /// Specific greetings from a user to a partner
    [Serializable()]
    public class PCustomisedGreetingRow : System.Data.DataRow
    {
        
        private PCustomisedGreetingTable myTable;
        
        /// Constructor
        public PCustomisedGreetingRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PCustomisedGreetingTable)(this.Table));
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
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
        
        /// This is the system user id. Each user of the system is allocated one
        public String UserId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUserId.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUserId) 
                            || (((String)(this[this.myTable.ColumnUserId])) != value)))
                {
                    this[this.myTable.ColumnUserId] = value;
                }
            }
        }
        
        /// 
        public String CustomisedGreetingText
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCustomisedGreetingText.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCustomisedGreetingText) 
                            || (((String)(this[this.myTable.ColumnCustomisedGreetingText])) != value)))
                {
                    this[this.myTable.ColumnCustomisedGreetingText] = value;
                }
            }
        }
        
        /// 
        public String CustomisedClosingText
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCustomisedClosingText.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCustomisedClosingText) 
                            || (((String)(this[this.myTable.ColumnCustomisedClosingText])) != value)))
                {
                    this[this.myTable.ColumnCustomisedClosingText] = value;
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
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnUserId);
            this.SetNull(this.myTable.ColumnCustomisedGreetingText);
            this.SetNull(this.myTable.ColumnCustomisedClosingText);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsCustomisedGreetingTextNull()
        {
            return this.IsNull(this.myTable.ColumnCustomisedGreetingText);
        }
        
        /// assign NULL value
        public void SetCustomisedGreetingTextNull()
        {
            this.SetNull(this.myTable.ColumnCustomisedGreetingText);
        }
        
        /// test for NULL value
        public bool IsCustomisedClosingTextNull()
        {
            return this.IsNull(this.myTable.ColumnCustomisedClosingText);
        }
        
        /// assign NULL value
        public void SetCustomisedClosingTextNull()
        {
            this.SetNull(this.myTable.ColumnCustomisedClosingText);
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
    
    /// Contains the text used in letters
    [Serializable()]
    public class PFormalityTable : TTypedDataTable
    {
        
        /// This is the code used to identify a language.
        public DataColumn ColumnLanguageCode;
        
        /// This is a code which identifies a country.
        ///It is taken from the ISO 3166-1-alpha-2 code elements.
        public DataColumn ColumnCountryCode;
        
        /// 
        public DataColumn ColumnAddresseeTypeCode;
        
        /// 
        public DataColumn ColumnFormalityLevel;
        
        /// 
        public DataColumn ColumnSalutationText;
        
        /// 
        public DataColumn ColumnTitle;
        
        /// 
        public DataColumn ColumnComplimentaryClosingText;
        
        /// 
        public DataColumn ColumnPersonalPronoun;
        
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
        public PFormalityTable() : 
                base("PFormality")
        {
        }
        
        /// constructor
        public PFormalityTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PFormalityTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PFormalityRow this[int i]
        {
            get
            {
                return ((PFormalityRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetLanguageCodeDBName()
        {
            return "p_language_code_c";
        }
        
        /// get help text for column
        public static string GetLanguageCodeHelp()
        {
            return "Enter the language code";
        }
        
        /// get label of column
        public static string GetLanguageCodeLabel()
        {
            return "Language Code";
        }
        
        /// get character length for column
        public static short GetLanguageCodeLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCountryCodeDBName()
        {
            return "p_country_code_c";
        }
        
        /// get help text for column
        public static string GetCountryCodeHelp()
        {
            return "Select a country code";
        }
        
        /// get label of column
        public static string GetCountryCodeLabel()
        {
            return "Country Code";
        }
        
        /// get character length for column
        public static short GetCountryCodeLength()
        {
            return 4;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddresseeTypeCodeDBName()
        {
            return "p_addressee_type_code_c";
        }
        
        /// get help text for column
        public static string GetAddresseeTypeCodeHelp()
        {
            return "Enter an addressee type code";
        }
        
        /// get label of column
        public static string GetAddresseeTypeCodeLabel()
        {
            return "Addressee Type Code";
        }
        
        /// get character length for column
        public static short GetAddresseeTypeCodeLength()
        {
            return 12;
        }
        
        /// get the name of the field in the database for this column
        public static string GetFormalityLevelDBName()
        {
            return "p_formality_level_i";
        }
        
        /// get help text for column
        public static string GetFormalityLevelHelp()
        {
            return "Very Informal (1) to Very Formal (5)";
        }
        
        /// get label of column
        public static string GetFormalityLevelLabel()
        {
            return "Formality Level";
        }
        
        /// get display format for column
        public static short GetFormalityLevelLength()
        {
            return 1;
        }
        
        /// get the name of the field in the database for this column
        public static string GetSalutationTextDBName()
        {
            return "p_salutation_text_c";
        }
        
        /// get help text for column
        public static string GetSalutationTextHelp()
        {
            return "Enter the salutation text";
        }
        
        /// get label of column
        public static string GetSalutationTextLabel()
        {
            return "Salutation Text";
        }
        
        /// get character length for column
        public static short GetSalutationTextLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTitleDBName()
        {
            return "p_title_c";
        }
        
        /// get help text for column
        public static string GetTitleHelp()
        {
            return "Enter a formality title";
        }
        
        /// get label of column
        public static string GetTitleLabel()
        {
            return "Title";
        }
        
        /// get character length for column
        public static short GetTitleLength()
        {
            return 12;
        }
        
        /// get the name of the field in the database for this column
        public static string GetComplimentaryClosingTextDBName()
        {
            return "p_complimentary_closing_text_c";
        }
        
        /// get help text for column
        public static string GetComplimentaryClosingTextHelp()
        {
            return "Enter the closing text";
        }
        
        /// get label of column
        public static string GetComplimentaryClosingTextLabel()
        {
            return "Closing Text";
        }
        
        /// get character length for column
        public static short GetComplimentaryClosingTextLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPersonalPronounDBName()
        {
            return "p_personal_pronoun_c";
        }
        
        /// get help text for column
        public static string GetPersonalPronounHelp()
        {
            return "Enter a formality personal pronoun";
        }
        
        /// get label of column
        public static string GetPersonalPronounLabel()
        {
            return "Personal Pronoun";
        }
        
        /// get character length for column
        public static short GetPersonalPronounLength()
        {
            return 12;
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
            return "PFormality";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_formality";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Formality";
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
                    "p_language_code_c",
                    "p_country_code_c",
                    "p_addressee_type_code_c",
                    "p_formality_level_i",
                    "p_salutation_text_c",
                    "p_title_c",
                    "p_complimentary_closing_text_c",
                    "p_personal_pronoun_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnLanguageCode = this.Columns["p_language_code_c"];
            this.ColumnCountryCode = this.Columns["p_country_code_c"];
            this.ColumnAddresseeTypeCode = this.Columns["p_addressee_type_code_c"];
            this.ColumnFormalityLevel = this.Columns["p_formality_level_i"];
            this.ColumnSalutationText = this.Columns["p_salutation_text_c"];
            this.ColumnTitle = this.Columns["p_title_c"];
            this.ColumnComplimentaryClosingText = this.Columns["p_complimentary_closing_text_c"];
            this.ColumnPersonalPronoun = this.Columns["p_personal_pronoun_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLanguageCode,
                    this.ColumnCountryCode,
                    this.ColumnAddresseeTypeCode,
                    this.ColumnFormalityLevel};
        }
        
        /// get typed set of changes
        public PFormalityTable GetChangesTyped()
        {
            return ((PFormalityTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PFormalityRow NewRowTyped(bool AWithDefaultValues)
        {
            PFormalityRow ret = ((PFormalityRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PFormalityRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PFormalityRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_language_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_country_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_addressee_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_formality_level_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_salutation_text_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_title_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_complimentary_closing_text_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_personal_pronoun_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnLanguageCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnCountryCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 8);
            }
            if ((ACol == ColumnAddresseeTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnFormalityLevel))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnSalutationText))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
            }
            if ((ACol == ColumnTitle))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnComplimentaryClosingText))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
            }
            if ((ACol == ColumnPersonalPronoun))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
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
    
    /// Contains the text used in letters
    [Serializable()]
    public class PFormalityRow : System.Data.DataRow
    {
        
        private PFormalityTable myTable;
        
        /// Constructor
        public PFormalityRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PFormalityTable)(this.Table));
        }
        
        /// This is the code used to identify a language.
        public String LanguageCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLanguageCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLanguageCode) 
                            || (((String)(this[this.myTable.ColumnLanguageCode])) != value)))
                {
                    this[this.myTable.ColumnLanguageCode] = value;
                }
            }
        }
        
        /// This is a code which identifies a country.
        ///It is taken from the ISO 3166-1-alpha-2 code elements.
        public String CountryCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCountryCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCountryCode) 
                            || (((String)(this[this.myTable.ColumnCountryCode])) != value)))
                {
                    this[this.myTable.ColumnCountryCode] = value;
                }
            }
        }
        
        /// 
        public String AddresseeTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddresseeTypeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddresseeTypeCode) 
                            || (((String)(this[this.myTable.ColumnAddresseeTypeCode])) != value)))
                {
                    this[this.myTable.ColumnAddresseeTypeCode] = value;
                }
            }
        }
        
        /// 
        public Int32 FormalityLevel
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFormalityLevel.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFormalityLevel) 
                            || (((Int32)(this[this.myTable.ColumnFormalityLevel])) != value)))
                {
                    this[this.myTable.ColumnFormalityLevel] = value;
                }
            }
        }
        
        /// 
        public String SalutationText
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSalutationText.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSalutationText) 
                            || (((String)(this[this.myTable.ColumnSalutationText])) != value)))
                {
                    this[this.myTable.ColumnSalutationText] = value;
                }
            }
        }
        
        /// 
        public String Title
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTitle.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTitle) 
                            || (((String)(this[this.myTable.ColumnTitle])) != value)))
                {
                    this[this.myTable.ColumnTitle] = value;
                }
            }
        }
        
        /// 
        public String ComplimentaryClosingText
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnComplimentaryClosingText.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnComplimentaryClosingText) 
                            || (((String)(this[this.myTable.ColumnComplimentaryClosingText])) != value)))
                {
                    this[this.myTable.ColumnComplimentaryClosingText] = value;
                }
            }
        }
        
        /// 
        public String PersonalPronoun
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPersonalPronoun.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPersonalPronoun) 
                            || (((String)(this[this.myTable.ColumnPersonalPronoun])) != value)))
                {
                    this[this.myTable.ColumnPersonalPronoun] = value;
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
            this[this.myTable.ColumnLanguageCode.Ordinal] = "99";
            this.SetNull(this.myTable.ColumnCountryCode);
            this.SetNull(this.myTable.ColumnAddresseeTypeCode);
            this[this.myTable.ColumnFormalityLevel.Ordinal] = 1;
            this.SetNull(this.myTable.ColumnSalutationText);
            this.SetNull(this.myTable.ColumnTitle);
            this.SetNull(this.myTable.ColumnComplimentaryClosingText);
            this.SetNull(this.myTable.ColumnPersonalPronoun);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsSalutationTextNull()
        {
            return this.IsNull(this.myTable.ColumnSalutationText);
        }
        
        /// assign NULL value
        public void SetSalutationTextNull()
        {
            this.SetNull(this.myTable.ColumnSalutationText);
        }
        
        /// test for NULL value
        public bool IsTitleNull()
        {
            return this.IsNull(this.myTable.ColumnTitle);
        }
        
        /// assign NULL value
        public void SetTitleNull()
        {
            this.SetNull(this.myTable.ColumnTitle);
        }
        
        /// test for NULL value
        public bool IsComplimentaryClosingTextNull()
        {
            return this.IsNull(this.myTable.ColumnComplimentaryClosingText);
        }
        
        /// assign NULL value
        public void SetComplimentaryClosingTextNull()
        {
            this.SetNull(this.myTable.ColumnComplimentaryClosingText);
        }
        
        /// test for NULL value
        public bool IsPersonalPronounNull()
        {
            return this.IsNull(this.myTable.ColumnPersonalPronoun);
        }
        
        /// assign NULL value
        public void SetPersonalPronounNull()
        {
            this.SetNull(this.myTable.ColumnPersonalPronoun);
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
    
    /// Text for form letters
    [Serializable()]
    public class PFormLetterBodyTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnBodyName;
        
        /// 
        public DataColumn ColumnBodyText;
        
        /// 
        public DataColumn ColumnDescription;
        
        /// 
        public DataColumn ColumnPhysicalFile;
        
        /// 
        public DataColumn ColumnOwner;
        
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
        public PFormLetterBodyTable() : 
                base("PFormLetterBody")
        {
        }
        
        /// constructor
        public PFormLetterBodyTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PFormLetterBodyTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PFormLetterBodyRow this[int i]
        {
            get
            {
                return ((PFormLetterBodyRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetBodyNameDBName()
        {
            return "p_body_name_c";
        }
        
        /// get help text for column
        public static string GetBodyNameHelp()
        {
            return "Enter a form letter body Body Name";
        }
        
        /// get label of column
        public static string GetBodyNameLabel()
        {
            return "Body Name";
        }
        
        /// get character length for column
        public static short GetBodyNameLength()
        {
            return 20;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBodyTextDBName()
        {
            return "p_body_text_c";
        }
        
        /// get help text for column
        public static string GetBodyTextHelp()
        {
            return "Enter the form letter body Body Text";
        }
        
        /// get label of column
        public static string GetBodyTextLabel()
        {
            return "Body Text";
        }
        
        /// get character length for column
        public static short GetBodyTextLength()
        {
            return 15000;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "p_description_c";
        }
        
        /// get help text for column
        public static string GetDescriptionHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetDescriptionLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 80;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPhysicalFileDBName()
        {
            return "p_physical_file_c";
        }
        
        /// get help text for column
        public static string GetPhysicalFileHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetPhysicalFileLabel()
        {
            return "Physical File";
        }
        
        /// get character length for column
        public static short GetPhysicalFileLength()
        {
            return 24;
        }
        
        /// get the name of the field in the database for this column
        public static string GetOwnerDBName()
        {
            return "p_owner_c";
        }
        
        /// get help text for column
        public static string GetOwnerHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetOwnerLabel()
        {
            return "Owner";
        }
        
        /// get character length for column
        public static short GetOwnerLength()
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
            return "PFormLetterBody";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_form_letter_body";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Form Letter Body";
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
                    "p_body_name_c",
                    "p_body_text_c",
                    "p_description_c",
                    "p_physical_file_c",
                    "p_owner_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnBodyName = this.Columns["p_body_name_c"];
            this.ColumnBodyText = this.Columns["p_body_text_c"];
            this.ColumnDescription = this.Columns["p_description_c"];
            this.ColumnPhysicalFile = this.Columns["p_physical_file_c"];
            this.ColumnOwner = this.Columns["p_owner_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnBodyName};
        }
        
        /// get typed set of changes
        public PFormLetterBodyTable GetChangesTyped()
        {
            return ((PFormLetterBodyTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PFormLetterBodyRow NewRowTyped(bool AWithDefaultValues)
        {
            PFormLetterBodyRow ret = ((PFormLetterBodyRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PFormLetterBodyRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PFormLetterBodyRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_body_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_body_text_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_physical_file_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_owner_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnBodyName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 40);
            }
            if ((ACol == ColumnBodyText))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 30000);
            }
            if ((ACol == ColumnDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
            }
            if ((ACol == ColumnPhysicalFile))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 48);
            }
            if ((ACol == ColumnOwner))
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
    
    /// Text for form letters
    [Serializable()]
    public class PFormLetterBodyRow : System.Data.DataRow
    {
        
        private PFormLetterBodyTable myTable;
        
        /// Constructor
        public PFormLetterBodyRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PFormLetterBodyTable)(this.Table));
        }
        
        /// 
        public String BodyName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBodyName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBodyName) 
                            || (((String)(this[this.myTable.ColumnBodyName])) != value)))
                {
                    this[this.myTable.ColumnBodyName] = value;
                }
            }
        }
        
        /// 
        public String BodyText
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBodyText.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBodyText) 
                            || (((String)(this[this.myTable.ColumnBodyText])) != value)))
                {
                    this[this.myTable.ColumnBodyText] = value;
                }
            }
        }
        
        /// 
        public String Description
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDescription) 
                            || (((String)(this[this.myTable.ColumnDescription])) != value)))
                {
                    this[this.myTable.ColumnDescription] = value;
                }
            }
        }
        
        /// 
        public String PhysicalFile
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPhysicalFile.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPhysicalFile) 
                            || (((String)(this[this.myTable.ColumnPhysicalFile])) != value)))
                {
                    this[this.myTable.ColumnPhysicalFile] = value;
                }
            }
        }
        
        /// 
        public String Owner
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnOwner.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnOwner) 
                            || (((String)(this[this.myTable.ColumnOwner])) != value)))
                {
                    this[this.myTable.ColumnOwner] = value;
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
            this.SetNull(this.myTable.ColumnBodyName);
            this.SetNull(this.myTable.ColumnBodyText);
            this.SetNull(this.myTable.ColumnDescription);
            this.SetNull(this.myTable.ColumnPhysicalFile);
            this.SetNull(this.myTable.ColumnOwner);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsBodyTextNull()
        {
            return this.IsNull(this.myTable.ColumnBodyText);
        }
        
        /// assign NULL value
        public void SetBodyTextNull()
        {
            this.SetNull(this.myTable.ColumnBodyText);
        }
        
        /// test for NULL value
        public bool IsDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnDescription);
        }
        
        /// assign NULL value
        public void SetDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnDescription);
        }
        
        /// test for NULL value
        public bool IsPhysicalFileNull()
        {
            return this.IsNull(this.myTable.ColumnPhysicalFile);
        }
        
        /// assign NULL value
        public void SetPhysicalFileNull()
        {
            this.SetNull(this.myTable.ColumnPhysicalFile);
        }
        
        /// test for NULL value
        public bool IsOwnerNull()
        {
            return this.IsNull(this.myTable.ColumnOwner);
        }
        
        /// assign NULL value
        public void SetOwnerNull()
        {
            this.SetNull(this.myTable.ColumnOwner);
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
    
    /// Configuration info for form letters
    [Serializable()]
    public class PFormLetterDesignTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnDesignName;
        
        /// The description of this form letter design.
        public DataColumn ColumnDescription;
        
        /// 
        public DataColumn ColumnAddressLayoutCode;
        
        /// 
        public DataColumn ColumnFormalityLevel;
        
        /// 
        public DataColumn ColumnBodyName;
        
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
        public PFormLetterDesignTable() : 
                base("PFormLetterDesign")
        {
        }
        
        /// constructor
        public PFormLetterDesignTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PFormLetterDesignTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PFormLetterDesignRow this[int i]
        {
            get
            {
                return ((PFormLetterDesignRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetDesignNameDBName()
        {
            return "p_design_name_c";
        }
        
        /// get help text for column
        public static string GetDesignNameHelp()
        {
            return "Enter a form letter design Design Name";
        }
        
        /// get label of column
        public static string GetDesignNameLabel()
        {
            return "Design Name";
        }
        
        /// get character length for column
        public static short GetDesignNameLength()
        {
            return 20;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "p_description_c";
        }
        
        /// get help text for column
        public static string GetDescriptionHelp()
        {
            return "Enter a description for this form letter design";
        }
        
        /// get label of column
        public static string GetDescriptionLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetAddressLayoutCodeDBName()
        {
            return "p_address_layout_code_c";
        }
        
        /// get help text for column
        public static string GetAddressLayoutCodeHelp()
        {
            return "Enter a form letter design Address Layout Code";
        }
        
        /// get label of column
        public static string GetAddressLayoutCodeLabel()
        {
            return "Address Layout Code";
        }
        
        /// get character length for column
        public static short GetAddressLayoutCodeLength()
        {
            return 16;
        }
        
        /// get the name of the field in the database for this column
        public static string GetFormalityLevelDBName()
        {
            return "p_formality_level_i";
        }
        
        /// get help text for column
        public static string GetFormalityLevelHelp()
        {
            return "Very Informal (1) to Very Formal (5)";
        }
        
        /// get label of column
        public static string GetFormalityLevelLabel()
        {
            return "Formality Level";
        }
        
        /// get display format for column
        public static short GetFormalityLevelLength()
        {
            return 1;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBodyNameDBName()
        {
            return "p_body_name_c";
        }
        
        /// get help text for column
        public static string GetBodyNameHelp()
        {
            return "Enter a form letter design Body Name";
        }
        
        /// get label of column
        public static string GetBodyNameLabel()
        {
            return "Body Name";
        }
        
        /// get character length for column
        public static short GetBodyNameLength()
        {
            return 20;
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
            return "PFormLetterDesign";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_form_letter_design";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Form Letter Design";
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
                    "p_design_name_c",
                    "p_description_c",
                    "p_address_layout_code_c",
                    "p_formality_level_i",
                    "p_body_name_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnDesignName = this.Columns["p_design_name_c"];
            this.ColumnDescription = this.Columns["p_description_c"];
            this.ColumnAddressLayoutCode = this.Columns["p_address_layout_code_c"];
            this.ColumnFormalityLevel = this.Columns["p_formality_level_i"];
            this.ColumnBodyName = this.Columns["p_body_name_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnDesignName};
        }
        
        /// get typed set of changes
        public PFormLetterDesignTable GetChangesTyped()
        {
            return ((PFormLetterDesignTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PFormLetterDesignRow NewRowTyped(bool AWithDefaultValues)
        {
            PFormLetterDesignRow ret = ((PFormLetterDesignRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PFormLetterDesignRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PFormLetterDesignRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_design_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_address_layout_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_formality_level_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_body_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnDesignName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 40);
            }
            if ((ACol == ColumnDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
            }
            if ((ACol == ColumnAddressLayoutCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 32);
            }
            if ((ACol == ColumnFormalityLevel))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnBodyName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 40);
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
    
    /// Configuration info for form letters
    [Serializable()]
    public class PFormLetterDesignRow : System.Data.DataRow
    {
        
        private PFormLetterDesignTable myTable;
        
        /// Constructor
        public PFormLetterDesignRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PFormLetterDesignTable)(this.Table));
        }
        
        /// 
        public String DesignName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDesignName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDesignName) 
                            || (((String)(this[this.myTable.ColumnDesignName])) != value)))
                {
                    this[this.myTable.ColumnDesignName] = value;
                }
            }
        }
        
        /// The description of this form letter design.
        public String Description
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDescription) 
                            || (((String)(this[this.myTable.ColumnDescription])) != value)))
                {
                    this[this.myTable.ColumnDescription] = value;
                }
            }
        }
        
        /// 
        public String AddressLayoutCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressLayoutCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddressLayoutCode) 
                            || (((String)(this[this.myTable.ColumnAddressLayoutCode])) != value)))
                {
                    this[this.myTable.ColumnAddressLayoutCode] = value;
                }
            }
        }
        
        /// 
        public Int32 FormalityLevel
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFormalityLevel.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFormalityLevel) 
                            || (((Int32)(this[this.myTable.ColumnFormalityLevel])) != value)))
                {
                    this[this.myTable.ColumnFormalityLevel] = value;
                }
            }
        }
        
        /// 
        public String BodyName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBodyName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBodyName) 
                            || (((String)(this[this.myTable.ColumnBodyName])) != value)))
                {
                    this[this.myTable.ColumnBodyName] = value;
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
            this.SetNull(this.myTable.ColumnDesignName);
            this.SetNull(this.myTable.ColumnDescription);
            this.SetNull(this.myTable.ColumnAddressLayoutCode);
            this[this.myTable.ColumnFormalityLevel.Ordinal] = 1;
            this.SetNull(this.myTable.ColumnBodyName);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnDescription);
        }
        
        /// assign NULL value
        public void SetDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnDescription);
        }
        
        /// test for NULL value
        public bool IsBodyNameNull()
        {
            return this.IsNull(this.myTable.ColumnBodyName);
        }
        
        /// assign NULL value
        public void SetBodyNameNull()
        {
            this.SetNull(this.myTable.ColumnBodyName);
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
    
    /// Insertions for a body of text for a given extract and partner
    [Serializable()]
    public class PFormLetterInsertTable : TTypedDataTable
    {
        
        /// Surrogate key
        public DataColumn ColumnSequence;
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        
        /// Identifier for the extract
        public DataColumn ColumnExtractId;
        
        /// 
        public DataColumn ColumnBodyName;
        
        /// This field should never be displaywed using the default format statement.
        public DataColumn ColumnInsert;
        
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
        public PFormLetterInsertTable() : 
                base("PFormLetterInsert")
        {
        }
        
        /// constructor
        public PFormLetterInsertTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PFormLetterInsertTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PFormLetterInsertRow this[int i]
        {
            get
            {
                return ((PFormLetterInsertRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetSequenceDBName()
        {
            return "p_sequence_i";
        }
        
        /// get help text for column
        public static string GetSequenceHelp()
        {
            return "Surrogate key";
        }
        
        /// get label of column
        public static string GetSequenceLabel()
        {
            return "p_sequence_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }
        
        /// get help text for column
        public static string GetPartnerKeyHelp()
        {
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get display format for column
        public static short GetPartnerKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetExtractIdDBName()
        {
            return "m_extract_id_i";
        }
        
        /// get help text for column
        public static string GetExtractIdHelp()
        {
            return "Identifier for the extract";
        }
        
        /// get label of column
        public static string GetExtractIdLabel()
        {
            return "Extract Id";
        }
        
        /// get display format for column
        public static short GetExtractIdLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetBodyNameDBName()
        {
            return "p_body_name_c";
        }
        
        /// get help text for column
        public static string GetBodyNameHelp()
        {
            return "Enter a form letter body Body Name";
        }
        
        /// get label of column
        public static string GetBodyNameLabel()
        {
            return "Body Name";
        }
        
        /// get character length for column
        public static short GetBodyNameLength()
        {
            return 20;
        }
        
        /// get the name of the field in the database for this column
        public static string GetInsertDBName()
        {
            return "p_insert_c";
        }
        
        /// get help text for column
        public static string GetInsertHelp()
        {
            return "This field should never be displaywed using the default format statement.";
        }
        
        /// get label of column
        public static string GetInsertLabel()
        {
            return "Inserts";
        }
        
        /// get character length for column
        public static short GetInsertLength()
        {
            return 256;
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
            return "PFormLetterInsert";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_form_letter_insert";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Form Letter Insert";
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
                    "p_sequence_i",
                    "p_partner_key_n",
                    "m_extract_id_i",
                    "p_body_name_c",
                    "p_insert_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnSequence = this.Columns["p_sequence_i"];
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnExtractId = this.Columns["m_extract_id_i"];
            this.ColumnBodyName = this.Columns["p_body_name_c"];
            this.ColumnInsert = this.Columns["p_insert_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnSequence};
        }
        
        /// get typed set of changes
        public PFormLetterInsertTable GetChangesTyped()
        {
            return ((PFormLetterInsertTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PFormLetterInsertRow NewRowTyped(bool AWithDefaultValues)
        {
            PFormLetterInsertRow ret = ((PFormLetterInsertRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PFormLetterInsertRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PFormLetterInsertRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_sequence_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("m_extract_id_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_body_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_insert_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnSequence))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnExtractId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnBodyName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 40);
            }
            if ((ACol == ColumnInsert))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 512);
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
    
    /// Insertions for a body of text for a given extract and partner
    [Serializable()]
    public class PFormLetterInsertRow : System.Data.DataRow
    {
        
        private PFormLetterInsertTable myTable;
        
        /// Constructor
        public PFormLetterInsertRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PFormLetterInsertTable)(this.Table));
        }
        
        /// Surrogate key
        public Int32 Sequence
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSequence.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSequence) 
                            || (((Int32)(this[this.myTable.ColumnSequence])) != value)))
                {
                    this[this.myTable.ColumnSequence] = value;
                }
            }
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
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
        
        /// Identifier for the extract
        public Int32 ExtractId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnExtractId.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnExtractId) 
                            || (((Int32)(this[this.myTable.ColumnExtractId])) != value)))
                {
                    this[this.myTable.ColumnExtractId] = value;
                }
            }
        }
        
        /// 
        public String BodyName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnBodyName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnBodyName) 
                            || (((String)(this[this.myTable.ColumnBodyName])) != value)))
                {
                    this[this.myTable.ColumnBodyName] = value;
                }
            }
        }
        
        /// This field should never be displaywed using the default format statement.
        public String Insert
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnInsert.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnInsert) 
                            || (((String)(this[this.myTable.ColumnInsert])) != value)))
                {
                    this[this.myTable.ColumnInsert] = value;
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
            this[this.myTable.ColumnSequence.Ordinal] = 0;
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this[this.myTable.ColumnExtractId.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnBodyName);
            this.SetNull(this.myTable.ColumnInsert);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsInsertNull()
        {
            return this.IsNull(this.myTable.ColumnInsert);
        }
        
        /// assign NULL value
        public void SetInsertNull()
        {
            this.SetNull(this.myTable.ColumnInsert);
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
    
    /// Defines the attributes of different label types.  Eg: for address labels.
    [Serializable()]
    public class PLabelTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnCode;
        
        /// This identifies the form
        public DataColumn ColumnFormName;
        
        /// 
        public DataColumn ColumnGapLines;
        
        /// 
        public DataColumn ColumnHeight;
        
        /// 
        public DataColumn ColumnWidth;
        
        /// 
        public DataColumn ColumnGapColumns;
        
        /// 
        public DataColumn ColumnLabelsAcross;
        
        /// 
        public DataColumn ColumnLabelsDown;
        
        /// 
        public DataColumn ColumnDescription;
        
        /// 
        public DataColumn ColumnStartColumn;
        
        /// 
        public DataColumn ColumnStartLine;
        
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
        public PLabelTable() : 
                base("PLabel")
        {
        }
        
        /// constructor
        public PLabelTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PLabelTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PLabelRow this[int i]
        {
            get
            {
                return ((PLabelRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetCodeDBName()
        {
            return "p_code_c";
        }
        
        /// get help text for column
        public static string GetCodeHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetCodeLabel()
        {
            return "Label Code";
        }
        
        /// get character length for column
        public static short GetCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetFormNameDBName()
        {
            return "s_form_name_c";
        }
        
        /// get help text for column
        public static string GetFormNameHelp()
        {
            return "Enter the form name";
        }
        
        /// get label of column
        public static string GetFormNameLabel()
        {
            return "Form Name";
        }
        
        /// get character length for column
        public static short GetFormNameLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetGapLinesDBName()
        {
            return "p_gap_lines_i";
        }
        
        /// get help text for column
        public static string GetGapLinesHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetGapLinesLabel()
        {
            return "Lines between labels";
        }
        
        /// get display format for column
        public static short GetGapLinesLength()
        {
            return 2;
        }
        
        /// get the name of the field in the database for this column
        public static string GetHeightDBName()
        {
            return "p_height_i";
        }
        
        /// get help text for column
        public static string GetHeightHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetHeightLabel()
        {
            return "Height in Lines";
        }
        
        /// get display format for column
        public static short GetHeightLength()
        {
            return 2;
        }
        
        /// get the name of the field in the database for this column
        public static string GetWidthDBName()
        {
            return "p_width_i";
        }
        
        /// get help text for column
        public static string GetWidthHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetWidthLabel()
        {
            return "Width in Characters";
        }
        
        /// get display format for column
        public static short GetWidthLength()
        {
            return 3;
        }
        
        /// get the name of the field in the database for this column
        public static string GetGapColumnsDBName()
        {
            return "p_gap_columns_i";
        }
        
        /// get help text for column
        public static string GetGapColumnsHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetGapColumnsLabel()
        {
            return "Columns between Labels";
        }
        
        /// get display format for column
        public static short GetGapColumnsLength()
        {
            return 2;
        }
        
        /// get the name of the field in the database for this column
        public static string GetLabelsAcrossDBName()
        {
            return "p_labels_across_i";
        }
        
        /// get help text for column
        public static string GetLabelsAcrossHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetLabelsAcrossLabel()
        {
            return "Number of labels across";
        }
        
        /// get display format for column
        public static short GetLabelsAcrossLength()
        {
            return 2;
        }
        
        /// get the name of the field in the database for this column
        public static string GetLabelsDownDBName()
        {
            return "p_labels_down_i";
        }
        
        /// get help text for column
        public static string GetLabelsDownHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetLabelsDownLabel()
        {
            return "Number of labels down";
        }
        
        /// get display format for column
        public static short GetLabelsDownLength()
        {
            return 2;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "p_description_c";
        }
        
        /// get help text for column
        public static string GetDescriptionHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetDescriptionLabel()
        {
            return "Label Description";
        }
        
        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 35;
        }
        
        /// get the name of the field in the database for this column
        public static string GetStartColumnDBName()
        {
            return "p_start_column_i";
        }
        
        /// get help text for column
        public static string GetStartColumnHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetStartColumnLabel()
        {
            return "Column to start printing in";
        }
        
        /// get display format for column
        public static short GetStartColumnLength()
        {
            return 2;
        }
        
        /// get the name of the field in the database for this column
        public static string GetStartLineDBName()
        {
            return "p_start_line_i";
        }
        
        /// get help text for column
        public static string GetStartLineHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetStartLineLabel()
        {
            return "Line to start printing on";
        }
        
        /// get display format for column
        public static short GetStartLineLength()
        {
            return 2;
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
            return "PLabel";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_label";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Label";
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
                    "p_code_c",
                    "s_form_name_c",
                    "p_gap_lines_i",
                    "p_height_i",
                    "p_width_i",
                    "p_gap_columns_i",
                    "p_labels_across_i",
                    "p_labels_down_i",
                    "p_description_c",
                    "p_start_column_i",
                    "p_start_line_i",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnCode = this.Columns["p_code_c"];
            this.ColumnFormName = this.Columns["s_form_name_c"];
            this.ColumnGapLines = this.Columns["p_gap_lines_i"];
            this.ColumnHeight = this.Columns["p_height_i"];
            this.ColumnWidth = this.Columns["p_width_i"];
            this.ColumnGapColumns = this.Columns["p_gap_columns_i"];
            this.ColumnLabelsAcross = this.Columns["p_labels_across_i"];
            this.ColumnLabelsDown = this.Columns["p_labels_down_i"];
            this.ColumnDescription = this.Columns["p_description_c"];
            this.ColumnStartColumn = this.Columns["p_start_column_i"];
            this.ColumnStartLine = this.Columns["p_start_line_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnCode};
        }
        
        /// get typed set of changes
        public PLabelTable GetChangesTyped()
        {
            return ((PLabelTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PLabelRow NewRowTyped(bool AWithDefaultValues)
        {
            PLabelRow ret = ((PLabelRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PLabelRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PLabelRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_form_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_gap_lines_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_height_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_width_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_gap_columns_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_labels_across_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_labels_down_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_start_column_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_start_line_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnFormName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnGapLines))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnHeight))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnWidth))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnGapColumns))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnLabelsAcross))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnLabelsDown))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 70);
            }
            if ((ACol == ColumnStartColumn))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnStartLine))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
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
    
    /// Defines the attributes of different label types.  Eg: for address labels.
    [Serializable()]
    public class PLabelRow : System.Data.DataRow
    {
        
        private PLabelTable myTable;
        
        /// Constructor
        public PLabelRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PLabelTable)(this.Table));
        }
        
        /// 
        public String Code
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCode) 
                            || (((String)(this[this.myTable.ColumnCode])) != value)))
                {
                    this[this.myTable.ColumnCode] = value;
                }
            }
        }
        
        /// This identifies the form
        public String FormName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFormName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFormName) 
                            || (((String)(this[this.myTable.ColumnFormName])) != value)))
                {
                    this[this.myTable.ColumnFormName] = value;
                }
            }
        }
        
        /// 
        public Int32 GapLines
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGapLines.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGapLines) 
                            || (((Int32)(this[this.myTable.ColumnGapLines])) != value)))
                {
                    this[this.myTable.ColumnGapLines] = value;
                }
            }
        }
        
        /// 
        public Int32 Height
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnHeight.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnHeight) 
                            || (((Int32)(this[this.myTable.ColumnHeight])) != value)))
                {
                    this[this.myTable.ColumnHeight] = value;
                }
            }
        }
        
        /// 
        public Int32 Width
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnWidth.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnWidth) 
                            || (((Int32)(this[this.myTable.ColumnWidth])) != value)))
                {
                    this[this.myTable.ColumnWidth] = value;
                }
            }
        }
        
        /// 
        public Int32 GapColumns
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGapColumns.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGapColumns) 
                            || (((Int32)(this[this.myTable.ColumnGapColumns])) != value)))
                {
                    this[this.myTable.ColumnGapColumns] = value;
                }
            }
        }
        
        /// 
        public Int32 LabelsAcross
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLabelsAcross.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLabelsAcross) 
                            || (((Int32)(this[this.myTable.ColumnLabelsAcross])) != value)))
                {
                    this[this.myTable.ColumnLabelsAcross] = value;
                }
            }
        }
        
        /// 
        public Int32 LabelsDown
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLabelsDown.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLabelsDown) 
                            || (((Int32)(this[this.myTable.ColumnLabelsDown])) != value)))
                {
                    this[this.myTable.ColumnLabelsDown] = value;
                }
            }
        }
        
        /// 
        public String Description
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDescription) 
                            || (((String)(this[this.myTable.ColumnDescription])) != value)))
                {
                    this[this.myTable.ColumnDescription] = value;
                }
            }
        }
        
        /// 
        public Int32 StartColumn
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnStartColumn.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnStartColumn) 
                            || (((Int32)(this[this.myTable.ColumnStartColumn])) != value)))
                {
                    this[this.myTable.ColumnStartColumn] = value;
                }
            }
        }
        
        /// 
        public Int32 StartLine
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnStartLine.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnStartLine) 
                            || (((Int32)(this[this.myTable.ColumnStartLine])) != value)))
                {
                    this[this.myTable.ColumnStartLine] = value;
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
            this.SetNull(this.myTable.ColumnCode);
            this.SetNull(this.myTable.ColumnFormName);
            this[this.myTable.ColumnGapLines.Ordinal] = 0;
            this[this.myTable.ColumnHeight.Ordinal] = 0;
            this[this.myTable.ColumnWidth.Ordinal] = 0;
            this[this.myTable.ColumnGapColumns.Ordinal] = 0;
            this[this.myTable.ColumnLabelsAcross.Ordinal] = 0;
            this[this.myTable.ColumnLabelsDown.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnDescription);
            this[this.myTable.ColumnStartColumn.Ordinal] = 0;
            this[this.myTable.ColumnStartLine.Ordinal] = 0;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsGapLinesNull()
        {
            return this.IsNull(this.myTable.ColumnGapLines);
        }
        
        /// assign NULL value
        public void SetGapLinesNull()
        {
            this.SetNull(this.myTable.ColumnGapLines);
        }
        
        /// test for NULL value
        public bool IsGapColumnsNull()
        {
            return this.IsNull(this.myTable.ColumnGapColumns);
        }
        
        /// assign NULL value
        public void SetGapColumnsNull()
        {
            this.SetNull(this.myTable.ColumnGapColumns);
        }
        
        /// test for NULL value
        public bool IsStartColumnNull()
        {
            return this.IsNull(this.myTable.ColumnStartColumn);
        }
        
        /// assign NULL value
        public void SetStartColumnNull()
        {
            this.SetNull(this.myTable.ColumnStartColumn);
        }
        
        /// test for NULL value
        public bool IsStartLineNull()
        {
            return this.IsNull(this.myTable.ColumnStartLine);
        }
        
        /// assign NULL value
        public void SetStartLineNull()
        {
            this.SetNull(this.myTable.ColumnStartLine);
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
    
    /// Master record for Mail Merge output creation
    [Serializable()]
    public class PMergeFormTable : TTypedDataTable
    {
        
        /// Name of Merge Form
        public DataColumn ColumnMergeFormName;
        
        /// Form description
        public DataColumn ColumnMergeFormDescription;
        
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
        public PMergeFormTable() : 
                base("PMergeForm")
        {
        }
        
        /// constructor
        public PMergeFormTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PMergeFormTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PMergeFormRow this[int i]
        {
            get
            {
                return ((PMergeFormRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetMergeFormNameDBName()
        {
            return "p_merge_form_name_c";
        }
        
        /// get help text for column
        public static string GetMergeFormNameHelp()
        {
            return "Name of Merge Form";
        }
        
        /// get label of column
        public static string GetMergeFormNameLabel()
        {
            return "Merge Form";
        }
        
        /// get character length for column
        public static short GetMergeFormNameLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetMergeFormDescriptionDBName()
        {
            return "p_merge_form_description_c";
        }
        
        /// get help text for column
        public static string GetMergeFormDescriptionHelp()
        {
            return "Enter a description";
        }
        
        /// get label of column
        public static string GetMergeFormDescriptionLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetMergeFormDescriptionLength()
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
            return "PMergeForm";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_merge_form";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "P_Merge_Form";
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
                    "p_merge_form_name_c",
                    "p_merge_form_description_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnMergeFormName = this.Columns["p_merge_form_name_c"];
            this.ColumnMergeFormDescription = this.Columns["p_merge_form_description_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnMergeFormName};
        }
        
        /// get typed set of changes
        public PMergeFormTable GetChangesTyped()
        {
            return ((PMergeFormTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PMergeFormRow NewRowTyped(bool AWithDefaultValues)
        {
            PMergeFormRow ret = ((PMergeFormRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PMergeFormRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PMergeFormRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_merge_form_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_merge_form_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnMergeFormName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnMergeFormDescription))
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
    
    /// Master record for Mail Merge output creation
    [Serializable()]
    public class PMergeFormRow : System.Data.DataRow
    {
        
        private PMergeFormTable myTable;
        
        /// Constructor
        public PMergeFormRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PMergeFormTable)(this.Table));
        }
        
        /// Name of Merge Form
        public String MergeFormName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMergeFormName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMergeFormName) 
                            || (((String)(this[this.myTable.ColumnMergeFormName])) != value)))
                {
                    this[this.myTable.ColumnMergeFormName] = value;
                }
            }
        }
        
        /// Form description
        public String MergeFormDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMergeFormDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMergeFormDescription) 
                            || (((String)(this[this.myTable.ColumnMergeFormDescription])) != value)))
                {
                    this[this.myTable.ColumnMergeFormDescription] = value;
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
            this.SetNull(this.myTable.ColumnMergeFormName);
            this.SetNull(this.myTable.ColumnMergeFormDescription);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsMergeFormDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnMergeFormDescription);
        }
        
        /// assign NULL value
        public void SetMergeFormDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnMergeFormDescription);
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
    
    /// Fields within a Mail Merge Form
    [Serializable()]
    public class PMergeFieldTable : TTypedDataTable
    {
        
        /// Name of Merge Form
        public DataColumn ColumnMergeFormName;
        
        /// Name of the field in the Word document which will be filled with the data 
        public DataColumn ColumnMergeFieldName;
        
        /// Position to define order of merge fields
        public DataColumn ColumnMergeFieldPosition;
        
        /// Type of this field.  This defines the parameters which are required to generate the insert
        public DataColumn ColumnMergeType;
        
        /// List of parameters required to create the actual insert
        public DataColumn ColumnMergeParameters;
        
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
        public PMergeFieldTable() : 
                base("PMergeField")
        {
        }
        
        /// constructor
        public PMergeFieldTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PMergeFieldTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PMergeFieldRow this[int i]
        {
            get
            {
                return ((PMergeFieldRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetMergeFormNameDBName()
        {
            return "p_merge_form_name_c";
        }
        
        /// get help text for column
        public static string GetMergeFormNameHelp()
        {
            return "Name of Merge Form";
        }
        
        /// get label of column
        public static string GetMergeFormNameLabel()
        {
            return "Merge Form";
        }
        
        /// get character length for column
        public static short GetMergeFormNameLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetMergeFieldNameDBName()
        {
            return "p_merge_field_name_c";
        }
        
        /// get help text for column
        public static string GetMergeFieldNameHelp()
        {
            return "Name of the field in the Word document";
        }
        
        /// get label of column
        public static string GetMergeFieldNameLabel()
        {
            return "Field Name";
        }
        
        /// get character length for column
        public static short GetMergeFieldNameLength()
        {
            return 16;
        }
        
        /// get the name of the field in the database for this column
        public static string GetMergeFieldPositionDBName()
        {
            return "p_merge_field_position_i";
        }
        
        /// get help text for column
        public static string GetMergeFieldPositionHelp()
        {
            return "Position to define order of merge fields";
        }
        
        /// get label of column
        public static string GetMergeFieldPositionLabel()
        {
            return "p_merge_field_position_i";
        }
        
        /// get the name of the field in the database for this column
        public static string GetMergeTypeDBName()
        {
            return "p_merge_type_c";
        }
        
        /// get help text for column
        public static string GetMergeTypeHelp()
        {
            return "Type of this field";
        }
        
        /// get label of column
        public static string GetMergeTypeLabel()
        {
            return "Merge Type";
        }
        
        /// get character length for column
        public static short GetMergeTypeLength()
        {
            return 16;
        }
        
        /// get the name of the field in the database for this column
        public static string GetMergeParametersDBName()
        {
            return "p_merge_parameters_c";
        }
        
        /// get help text for column
        public static string GetMergeParametersHelp()
        {
            return "Field parameters";
        }
        
        /// get label of column
        public static string GetMergeParametersLabel()
        {
            return "Parameters";
        }
        
        /// get character length for column
        public static short GetMergeParametersLength()
        {
            return 256;
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
            return "PMergeField";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_merge_field";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "P_Merge_Field";
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
                    "p_merge_form_name_c",
                    "p_merge_field_name_c",
                    "p_merge_field_position_i",
                    "p_merge_type_c",
                    "p_merge_parameters_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnMergeFormName = this.Columns["p_merge_form_name_c"];
            this.ColumnMergeFieldName = this.Columns["p_merge_field_name_c"];
            this.ColumnMergeFieldPosition = this.Columns["p_merge_field_position_i"];
            this.ColumnMergeType = this.Columns["p_merge_type_c"];
            this.ColumnMergeParameters = this.Columns["p_merge_parameters_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnMergeFormName,
                    this.ColumnMergeFieldName};
        }
        
        /// get typed set of changes
        public PMergeFieldTable GetChangesTyped()
        {
            return ((PMergeFieldTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PMergeFieldRow NewRowTyped(bool AWithDefaultValues)
        {
            PMergeFieldRow ret = ((PMergeFieldRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PMergeFieldRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PMergeFieldRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_merge_form_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_merge_field_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_merge_field_position_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_merge_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_merge_parameters_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnMergeFormName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnMergeFieldName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 32);
            }
            if ((ACol == ColumnMergeFieldPosition))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnMergeType))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 32);
            }
            if ((ACol == ColumnMergeParameters))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 512);
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
    
    /// Fields within a Mail Merge Form
    [Serializable()]
    public class PMergeFieldRow : System.Data.DataRow
    {
        
        private PMergeFieldTable myTable;
        
        /// Constructor
        public PMergeFieldRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PMergeFieldTable)(this.Table));
        }
        
        /// Name of Merge Form
        public String MergeFormName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMergeFormName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMergeFormName) 
                            || (((String)(this[this.myTable.ColumnMergeFormName])) != value)))
                {
                    this[this.myTable.ColumnMergeFormName] = value;
                }
            }
        }
        
        /// Name of the field in the Word document which will be filled with the data 
        public String MergeFieldName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMergeFieldName.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMergeFieldName) 
                            || (((String)(this[this.myTable.ColumnMergeFieldName])) != value)))
                {
                    this[this.myTable.ColumnMergeFieldName] = value;
                }
            }
        }
        
        /// Position to define order of merge fields
        public Int32 MergeFieldPosition
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMergeFieldPosition.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMergeFieldPosition) 
                            || (((Int32)(this[this.myTable.ColumnMergeFieldPosition])) != value)))
                {
                    this[this.myTable.ColumnMergeFieldPosition] = value;
                }
            }
        }
        
        /// Type of this field.  This defines the parameters which are required to generate the insert
        public String MergeType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMergeType.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMergeType) 
                            || (((String)(this[this.myTable.ColumnMergeType])) != value)))
                {
                    this[this.myTable.ColumnMergeType] = value;
                }
            }
        }
        
        /// List of parameters required to create the actual insert
        public String MergeParameters
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMergeParameters.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMergeParameters) 
                            || (((String)(this[this.myTable.ColumnMergeParameters])) != value)))
                {
                    this[this.myTable.ColumnMergeParameters] = value;
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
            this.SetNull(this.myTable.ColumnMergeFormName);
            this.SetNull(this.myTable.ColumnMergeFieldName);
            this[this.myTable.ColumnMergeFieldPosition.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnMergeType);
            this.SetNull(this.myTable.ColumnMergeParameters);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsMergeFieldPositionNull()
        {
            return this.IsNull(this.myTable.ColumnMergeFieldPosition);
        }
        
        /// assign NULL value
        public void SetMergeFieldPositionNull()
        {
            this.SetNull(this.myTable.ColumnMergeFieldPosition);
        }
        
        /// test for NULL value
        public bool IsMergeTypeNull()
        {
            return this.IsNull(this.myTable.ColumnMergeType);
        }
        
        /// assign NULL value
        public void SetMergeTypeNull()
        {
            this.SetNull(this.myTable.ColumnMergeType);
        }
        
        /// test for NULL value
        public bool IsMergeParametersNull()
        {
            return this.IsNull(this.myTable.ColumnMergeParameters);
        }
        
        /// assign NULL value
        public void SetMergeParametersNull()
        {
            this.SetNull(this.myTable.ColumnMergeParameters);
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
    
    /// Postcode ranges for each region
    [Serializable()]
    public class PPostcodeRangeTable : TTypedDataTable
    {
        
        /// Name of the postcode range
        public DataColumn ColumnRange;
        
        /// Start of postcode range
        public DataColumn ColumnFrom;
        
        /// End of postcode range
        public DataColumn ColumnTo;
        
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
        public PPostcodeRangeTable() : 
                base("PPostcodeRange")
        {
        }
        
        /// constructor
        public PPostcodeRangeTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PPostcodeRangeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PPostcodeRangeRow this[int i]
        {
            get
            {
                return ((PPostcodeRangeRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetRangeDBName()
        {
            return "p_range_c";
        }
        
        /// get help text for column
        public static string GetRangeHelp()
        {
            return "Name of the postcode range";
        }
        
        /// get label of column
        public static string GetRangeLabel()
        {
            return "Range Name";
        }
        
        /// get character length for column
        public static short GetRangeLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetFromDBName()
        {
            return "p_from_c";
        }
        
        /// get help text for column
        public static string GetFromHelp()
        {
            return "Start of postcode range";
        }
        
        /// get label of column
        public static string GetFromLabel()
        {
            return "From";
        }
        
        /// get character length for column
        public static short GetFromLength()
        {
            return 20;
        }
        
        /// get the name of the field in the database for this column
        public static string GetToDBName()
        {
            return "p_to_c";
        }
        
        /// get help text for column
        public static string GetToHelp()
        {
            return "End of postcode range";
        }
        
        /// get label of column
        public static string GetToLabel()
        {
            return "To";
        }
        
        /// get character length for column
        public static short GetToLength()
        {
            return 20;
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
            return "PPostcodeRange";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_postcode_range";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Postcode Range";
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
                    "p_range_c",
                    "p_from_c",
                    "p_to_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnRange = this.Columns["p_range_c"];
            this.ColumnFrom = this.Columns["p_from_c"];
            this.ColumnTo = this.Columns["p_to_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnRange};
        }
        
        /// get typed set of changes
        public PPostcodeRangeTable GetChangesTyped()
        {
            return ((PPostcodeRangeTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PPostcodeRangeRow NewRowTyped(bool AWithDefaultValues)
        {
            PPostcodeRangeRow ret = ((PPostcodeRangeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PPostcodeRangeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PPostcodeRangeRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_range_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_from_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_to_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnRange))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
            }
            if ((ACol == ColumnFrom))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 40);
            }
            if ((ACol == ColumnTo))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 40);
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
    
    /// Postcode ranges for each region
    [Serializable()]
    public class PPostcodeRangeRow : System.Data.DataRow
    {
        
        private PPostcodeRangeTable myTable;
        
        /// Constructor
        public PPostcodeRangeRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PPostcodeRangeTable)(this.Table));
        }
        
        /// Name of the postcode range
        public String Range
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRange.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRange) 
                            || (((String)(this[this.myTable.ColumnRange])) != value)))
                {
                    this[this.myTable.ColumnRange] = value;
                }
            }
        }
        
        /// Start of postcode range
        public String From
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFrom.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFrom) 
                            || (((String)(this[this.myTable.ColumnFrom])) != value)))
                {
                    this[this.myTable.ColumnFrom] = value;
                }
            }
        }
        
        /// End of postcode range
        public String To
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTo.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTo) 
                            || (((String)(this[this.myTable.ColumnTo])) != value)))
                {
                    this[this.myTable.ColumnTo] = value;
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
            this.SetNull(this.myTable.ColumnRange);
            this.SetNull(this.myTable.ColumnFrom);
            this.SetNull(this.myTable.ColumnTo);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsFromNull()
        {
            return this.IsNull(this.myTable.ColumnFrom);
        }
        
        /// assign NULL value
        public void SetFromNull()
        {
            this.SetNull(this.myTable.ColumnFrom);
        }
        
        /// test for NULL value
        public bool IsToNull()
        {
            return this.IsNull(this.myTable.ColumnTo);
        }
        
        /// assign NULL value
        public void SetToNull()
        {
            this.SetNull(this.myTable.ColumnTo);
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
    
    /// Postcode regions and the ranges they contain.
    [Serializable()]
    public class PPostcodeRegionTable : TTypedDataTable
    {
        
        /// Name of a postcode region
        public DataColumn ColumnRegion;
        
        /// A range for a postcode region
        public DataColumn ColumnRange;
        
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
        public PPostcodeRegionTable() : 
                base("PPostcodeRegion")
        {
        }
        
        /// constructor
        public PPostcodeRegionTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PPostcodeRegionTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PPostcodeRegionRow this[int i]
        {
            get
            {
                return ((PPostcodeRegionRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetRegionDBName()
        {
            return "p_region_c";
        }
        
        /// get help text for column
        public static string GetRegionHelp()
        {
            return "Name of a postcode region";
        }
        
        /// get label of column
        public static string GetRegionLabel()
        {
            return "Region Name";
        }
        
        /// get character length for column
        public static short GetRegionLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetRangeDBName()
        {
            return "p_range_c";
        }
        
        /// get help text for column
        public static string GetRangeHelp()
        {
            return "A range for a postcode region";
        }
        
        /// get label of column
        public static string GetRangeLabel()
        {
            return "Range Name";
        }
        
        /// get character length for column
        public static short GetRangeLength()
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
            return "PPostcodeRegion";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_postcode_region";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Postcode Region";
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
                    "p_region_c",
                    "p_range_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnRegion = this.Columns["p_region_c"];
            this.ColumnRange = this.Columns["p_range_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnRegion,
                    this.ColumnRange};
        }
        
        /// get typed set of changes
        public PPostcodeRegionTable GetChangesTyped()
        {
            return ((PPostcodeRegionTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PPostcodeRegionRow NewRowTyped(bool AWithDefaultValues)
        {
            PPostcodeRegionRow ret = ((PPostcodeRegionRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PPostcodeRegionRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PPostcodeRegionRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_region_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_range_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnRegion))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
            }
            if ((ACol == ColumnRange))
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
    
    /// Postcode regions and the ranges they contain.
    [Serializable()]
    public class PPostcodeRegionRow : System.Data.DataRow
    {
        
        private PPostcodeRegionTable myTable;
        
        /// Constructor
        public PPostcodeRegionRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PPostcodeRegionTable)(this.Table));
        }
        
        /// Name of a postcode region
        public String Region
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRegion.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRegion) 
                            || (((String)(this[this.myTable.ColumnRegion])) != value)))
                {
                    this[this.myTable.ColumnRegion] = value;
                }
            }
        }
        
        /// A range for a postcode region
        public String Range
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRange.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRange) 
                            || (((String)(this[this.myTable.ColumnRange])) != value)))
                {
                    this[this.myTable.ColumnRange] = value;
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
            this.SetNull(this.myTable.ColumnRegion);
            this.SetNull(this.myTable.ColumnRange);
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
    
    /// Details of a publication
    [Serializable()]
    public class PPublicationTable : TTypedDataTable
    {
        
        /// This is the key to the publication table
        public DataColumn ColumnPublicationCode;
        
        /// 
        public DataColumn ColumnNumberOfIssues;
        
        /// The number of free issues and reminders to send out.
        public DataColumn ColumnNumberOfReminders;
        
        /// A short description of the publication
        public DataColumn ColumnPublicationDescription;
        
        /// 
        public DataColumn ColumnValidPublication;
        
        /// 
        public DataColumn ColumnFrequencyCode;
        
        /// The publication short code that is used on an address label
        public DataColumn ColumnPublicationLabelCode;
        
        /// 
        public DataColumn ColumnPublicationLanguage;
        
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
        public PPublicationTable() : 
                base("PPublication")
        {
        }
        
        /// constructor
        public PPublicationTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PPublicationTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PPublicationRow this[int i]
        {
            get
            {
                return ((PPublicationRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetPublicationCodeDBName()
        {
            return "p_publication_code_c";
        }
        
        /// get help text for column
        public static string GetPublicationCodeHelp()
        {
            return "Enter a publication code";
        }
        
        /// get label of column
        public static string GetPublicationCodeLabel()
        {
            return "Publication Code";
        }
        
        /// get character length for column
        public static short GetPublicationCodeLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfIssuesDBName()
        {
            return "p_number_of_issues_i";
        }
        
        /// get help text for column
        public static string GetNumberOfIssuesHelp()
        {
            return "Enter the number of issues per subscription";
        }
        
        /// get label of column
        public static string GetNumberOfIssuesLabel()
        {
            return "Number of Issues";
        }
        
        /// get display format for column
        public static short GetNumberOfIssuesLength()
        {
            return 3;
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfRemindersDBName()
        {
            return "p_number_of_reminders_i";
        }
        
        /// get help text for column
        public static string GetNumberOfRemindersHelp()
        {
            return "Enter the number of issues and reminders to send out";
        }
        
        /// get label of column
        public static string GetNumberOfRemindersLabel()
        {
            return "Reminders";
        }
        
        /// get display format for column
        public static short GetNumberOfRemindersLength()
        {
            return 2;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPublicationDescriptionDBName()
        {
            return "p_publication_description_c";
        }
        
        /// get help text for column
        public static string GetPublicationDescriptionHelp()
        {
            return "Enter a full description";
        }
        
        /// get label of column
        public static string GetPublicationDescriptionLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetPublicationDescriptionLength()
        {
            return 40;
        }
        
        /// get the name of the field in the database for this column
        public static string GetValidPublicationDBName()
        {
            return "p_valid_publication_l";
        }
        
        /// get help text for column
        public static string GetValidPublicationHelp()
        {
            return "Select if publication can be selected by users";
        }
        
        /// get label of column
        public static string GetValidPublicationLabel()
        {
            return "Valid Publication";
        }
        
        /// get display format for column
        public static short GetValidPublicationLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetFrequencyCodeDBName()
        {
            return "a_frequency_code_c";
        }
        
        /// get help text for column
        public static string GetFrequencyCodeHelp()
        {
            return "Select how often this publication will be produced";
        }
        
        /// get label of column
        public static string GetFrequencyCodeLabel()
        {
            return "Frequency";
        }
        
        /// get character length for column
        public static short GetFrequencyCodeLength()
        {
            return 12;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPublicationLabelCodeDBName()
        {
            return "p_publication_label_code_c";
        }
        
        /// get help text for column
        public static string GetPublicationLabelCodeHelp()
        {
            return "Enter a short code (max. 3 characters) for the publication.";
        }
        
        /// get label of column
        public static string GetPublicationLabelCodeLabel()
        {
            return "Publication Label Code";
        }
        
        /// get character length for column
        public static short GetPublicationLabelCodeLength()
        {
            return 3;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPublicationLanguageDBName()
        {
            return "p_publication_language_c";
        }
        
        /// get help text for column
        public static string GetPublicationLanguageHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetPublicationLanguageLabel()
        {
            return "Language";
        }
        
        /// get character length for column
        public static short GetPublicationLanguageLength()
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
            return "PPublication";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_publication";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Publication";
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
                    "p_publication_code_c",
                    "p_number_of_issues_i",
                    "p_number_of_reminders_i",
                    "p_publication_description_c",
                    "p_valid_publication_l",
                    "a_frequency_code_c",
                    "p_publication_label_code_c",
                    "p_publication_language_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPublicationCode = this.Columns["p_publication_code_c"];
            this.ColumnNumberOfIssues = this.Columns["p_number_of_issues_i"];
            this.ColumnNumberOfReminders = this.Columns["p_number_of_reminders_i"];
            this.ColumnPublicationDescription = this.Columns["p_publication_description_c"];
            this.ColumnValidPublication = this.Columns["p_valid_publication_l"];
            this.ColumnFrequencyCode = this.Columns["a_frequency_code_c"];
            this.ColumnPublicationLabelCode = this.Columns["p_publication_label_code_c"];
            this.ColumnPublicationLanguage = this.Columns["p_publication_language_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnPublicationCode};
        }
        
        /// get typed set of changes
        public PPublicationTable GetChangesTyped()
        {
            return ((PPublicationTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PPublicationRow NewRowTyped(bool AWithDefaultValues)
        {
            PPublicationRow ret = ((PPublicationRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PPublicationRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PPublicationRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_publication_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_number_of_issues_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_number_of_reminders_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_publication_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_valid_publication_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("a_frequency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_publication_label_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_publication_language_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnPublicationCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnNumberOfIssues))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNumberOfReminders))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnPublicationDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 80);
            }
            if ((ACol == ColumnValidPublication))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnFrequencyCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnPublicationLabelCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 6);
            }
            if ((ACol == ColumnPublicationLanguage))
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
    
    /// Details of a publication
    [Serializable()]
    public class PPublicationRow : System.Data.DataRow
    {
        
        private PPublicationTable myTable;
        
        /// Constructor
        public PPublicationRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PPublicationTable)(this.Table));
        }
        
        /// This is the key to the publication table
        public String PublicationCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPublicationCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPublicationCode) 
                            || (((String)(this[this.myTable.ColumnPublicationCode])) != value)))
                {
                    this[this.myTable.ColumnPublicationCode] = value;
                }
            }
        }
        
        /// 
        public Int32 NumberOfIssues
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfIssues.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfIssues) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfIssues])) != value)))
                {
                    this[this.myTable.ColumnNumberOfIssues] = value;
                }
            }
        }
        
        /// The number of free issues and reminders to send out.
        public Int32 NumberOfReminders
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfReminders.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfReminders) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfReminders])) != value)))
                {
                    this[this.myTable.ColumnNumberOfReminders] = value;
                }
            }
        }
        
        /// A short description of the publication
        public String PublicationDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPublicationDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPublicationDescription) 
                            || (((String)(this[this.myTable.ColumnPublicationDescription])) != value)))
                {
                    this[this.myTable.ColumnPublicationDescription] = value;
                }
            }
        }
        
        /// 
        public Boolean ValidPublication
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnValidPublication.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnValidPublication) 
                            || (((Boolean)(this[this.myTable.ColumnValidPublication])) != value)))
                {
                    this[this.myTable.ColumnValidPublication] = value;
                }
            }
        }
        
        /// 
        public String FrequencyCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFrequencyCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFrequencyCode) 
                            || (((String)(this[this.myTable.ColumnFrequencyCode])) != value)))
                {
                    this[this.myTable.ColumnFrequencyCode] = value;
                }
            }
        }
        
        /// The publication short code that is used on an address label
        public String PublicationLabelCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPublicationLabelCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPublicationLabelCode) 
                            || (((String)(this[this.myTable.ColumnPublicationLabelCode])) != value)))
                {
                    this[this.myTable.ColumnPublicationLabelCode] = value;
                }
            }
        }
        
        /// 
        public String PublicationLanguage
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPublicationLanguage.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPublicationLanguage) 
                            || (((String)(this[this.myTable.ColumnPublicationLanguage])) != value)))
                {
                    this[this.myTable.ColumnPublicationLanguage] = value;
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
            this.SetNull(this.myTable.ColumnPublicationCode);
            this[this.myTable.ColumnNumberOfIssues.Ordinal] = 0;
            this[this.myTable.ColumnNumberOfReminders.Ordinal] = 1;
            this.SetNull(this.myTable.ColumnPublicationDescription);
            this[this.myTable.ColumnValidPublication.Ordinal] = true;
            this.SetNull(this.myTable.ColumnFrequencyCode);
            this.SetNull(this.myTable.ColumnPublicationLabelCode);
            this.SetNull(this.myTable.ColumnPublicationLanguage);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsNumberOfIssuesNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfIssues);
        }
        
        /// assign NULL value
        public void SetNumberOfIssuesNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfIssues);
        }
        
        /// test for NULL value
        public bool IsNumberOfRemindersNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfReminders);
        }
        
        /// assign NULL value
        public void SetNumberOfRemindersNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfReminders);
        }
        
        /// test for NULL value
        public bool IsPublicationDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnPublicationDescription);
        }
        
        /// assign NULL value
        public void SetPublicationDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnPublicationDescription);
        }
        
        /// test for NULL value
        public bool IsValidPublicationNull()
        {
            return this.IsNull(this.myTable.ColumnValidPublication);
        }
        
        /// assign NULL value
        public void SetValidPublicationNull()
        {
            this.SetNull(this.myTable.ColumnValidPublication);
        }
        
        /// test for NULL value
        public bool IsPublicationLabelCodeNull()
        {
            return this.IsNull(this.myTable.ColumnPublicationLabelCode);
        }
        
        /// assign NULL value
        public void SetPublicationLabelCodeNull()
        {
            this.SetNull(this.myTable.ColumnPublicationLabelCode);
        }
        
        /// test for NULL value
        public bool IsPublicationLanguageNull()
        {
            return this.IsNull(this.myTable.ColumnPublicationLanguage);
        }
        
        /// assign NULL value
        public void SetPublicationLanguageNull()
        {
            this.SetNull(this.myTable.ColumnPublicationLanguage);
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
    
    /// Cost of a publication
    [Serializable()]
    public class PPublicationCostTable : TTypedDataTable
    {
        
        /// The is the key to the publication table
        public DataColumn ColumnPublicationCode;
        
        /// 
        public DataColumn ColumnDateEffective;
        
        /// This is a number of currency units
        public DataColumn ColumnPublicationCost;
        
        /// The cost of posting each item
        public DataColumn ColumnPostageCost;
        
        /// This defines which currency is being used
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
        public PPublicationCostTable() : 
                base("PPublicationCost")
        {
        }
        
        /// constructor
        public PPublicationCostTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PPublicationCostTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PPublicationCostRow this[int i]
        {
            get
            {
                return ((PPublicationCostRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetPublicationCodeDBName()
        {
            return "p_publication_code_c";
        }
        
        /// get help text for column
        public static string GetPublicationCodeHelp()
        {
            return "Enter a publication code";
        }
        
        /// get label of column
        public static string GetPublicationCodeLabel()
        {
            return "Publication Code";
        }
        
        /// get character length for column
        public static short GetPublicationCodeLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateEffectiveDBName()
        {
            return "p_date_effective_d";
        }
        
        /// get help text for column
        public static string GetDateEffectiveHelp()
        {
            return "The effective date of the publication cost";
        }
        
        /// get label of column
        public static string GetDateEffectiveLabel()
        {
            return "Date Effective";
        }
        
        /// get display format for column
        public static short GetDateEffectiveLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPublicationCostDBName()
        {
            return "p_publication_cost_n";
        }
        
        /// get help text for column
        public static string GetPublicationCostHelp()
        {
            return "Enter the cost of this publication";
        }
        
        /// get label of column
        public static string GetPublicationCostLabel()
        {
            return "Publication Cost";
        }
        
        /// get display format for column
        public static short GetPublicationCostLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPostageCostDBName()
        {
            return "p_postage_cost_n";
        }
        
        /// get help text for column
        public static string GetPostageCostHelp()
        {
            return "Enter the postage cost";
        }
        
        /// get label of column
        public static string GetPostageCostLabel()
        {
            return "Postage";
        }
        
        /// get display format for column
        public static short GetPostageCostLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCurrencyCodeDBName()
        {
            return "p_currency_code_c";
        }
        
        /// get help text for column
        public static string GetCurrencyCodeHelp()
        {
            return "Enter a currency code";
        }
        
        /// get label of column
        public static string GetCurrencyCodeLabel()
        {
            return "Currency Code";
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
            return "PPublicationCost";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_publication_cost";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Publication Cost";
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
                    "p_publication_code_c",
                    "p_date_effective_d",
                    "p_publication_cost_n",
                    "p_postage_cost_n",
                    "p_currency_code_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPublicationCode = this.Columns["p_publication_code_c"];
            this.ColumnDateEffective = this.Columns["p_date_effective_d"];
            this.ColumnPublicationCost = this.Columns["p_publication_cost_n"];
            this.ColumnPostageCost = this.Columns["p_postage_cost_n"];
            this.ColumnCurrencyCode = this.Columns["p_currency_code_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnPublicationCode,
                    this.ColumnDateEffective};
        }
        
        /// get typed set of changes
        public PPublicationCostTable GetChangesTyped()
        {
            return ((PPublicationCostTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PPublicationCostRow NewRowTyped(bool AWithDefaultValues)
        {
            PPublicationCostRow ret = ((PPublicationCostRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PPublicationCostRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PPublicationCostRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_publication_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_date_effective_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_publication_cost_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("p_postage_cost_n", typeof(Double)));
            this.Columns.Add(new System.Data.DataColumn("p_currency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnPublicationCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnDateEffective))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnPublicationCost))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 24);
            }
            if ((ACol == ColumnPostageCost))
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
    
    /// Cost of a publication
    [Serializable()]
    public class PPublicationCostRow : System.Data.DataRow
    {
        
        private PPublicationCostTable myTable;
        
        /// Constructor
        public PPublicationCostRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PPublicationCostTable)(this.Table));
        }
        
        /// The is the key to the publication table
        public String PublicationCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPublicationCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPublicationCode) 
                            || (((String)(this[this.myTable.ColumnPublicationCode])) != value)))
                {
                    this[this.myTable.ColumnPublicationCode] = value;
                }
            }
        }
        
        /// 
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
        
        /// This is a number of currency units
        public Double PublicationCost
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPublicationCost.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPublicationCost) 
                            || (((Double)(this[this.myTable.ColumnPublicationCost])) != value)))
                {
                    this[this.myTable.ColumnPublicationCost] = value;
                }
            }
        }
        
        /// The cost of posting each item
        public Double PostageCost
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPostageCost.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPostageCost) 
                            || (((Double)(this[this.myTable.ColumnPostageCost])) != value)))
                {
                    this[this.myTable.ColumnPostageCost] = value;
                }
            }
        }
        
        /// This defines which currency is being used
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
            this.SetNull(this.myTable.ColumnPublicationCode);
            this[this.myTable.ColumnDateEffective.Ordinal] = DateTime.Today;
            this[this.myTable.ColumnPublicationCost.Ordinal] = 0;
            this[this.myTable.ColumnPostageCost.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnCurrencyCode);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsPostageCostNull()
        {
            return this.IsNull(this.myTable.ColumnPostageCost);
        }
        
        /// assign NULL value
        public void SetPostageCostNull()
        {
            this.SetNull(this.myTable.ColumnPostageCost);
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
    
    /// List of reasons for giving a subscription
    [Serializable()]
    public class PReasonSubscriptionGivenTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnCode;
        
        /// 
        public DataColumn ColumnDescription;
        
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
        public PReasonSubscriptionGivenTable() : 
                base("PReasonSubscriptionGiven")
        {
        }
        
        /// constructor
        public PReasonSubscriptionGivenTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PReasonSubscriptionGivenTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PReasonSubscriptionGivenRow this[int i]
        {
            get
            {
                return ((PReasonSubscriptionGivenRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetCodeDBName()
        {
            return "p_code_c";
        }
        
        /// get help text for column
        public static string GetCodeHelp()
        {
            return "Enter code for reason given";
        }
        
        /// get label of column
        public static string GetCodeLabel()
        {
            return "Reason Given Code";
        }
        
        /// get character length for column
        public static short GetCodeLength()
        {
            return 12;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "p_description_c";
        }
        
        /// get help text for column
        public static string GetDescriptionHelp()
        {
            return "Enter a full description for reason Subscription is given";
        }
        
        /// get label of column
        public static string GetDescriptionLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 80;
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
            return "PReasonSubscriptionGiven";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_reason_subscription_given";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Reason Subscription Given";
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
                    "p_code_c",
                    "p_description_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnCode = this.Columns["p_code_c"];
            this.ColumnDescription = this.Columns["p_description_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnCode};
        }
        
        /// get typed set of changes
        public PReasonSubscriptionGivenTable GetChangesTyped()
        {
            return ((PReasonSubscriptionGivenTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PReasonSubscriptionGivenRow NewRowTyped(bool AWithDefaultValues)
        {
            PReasonSubscriptionGivenRow ret = ((PReasonSubscriptionGivenRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PReasonSubscriptionGivenRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PReasonSubscriptionGivenRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
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
    
    /// List of reasons for giving a subscription
    [Serializable()]
    public class PReasonSubscriptionGivenRow : System.Data.DataRow
    {
        
        private PReasonSubscriptionGivenTable myTable;
        
        /// Constructor
        public PReasonSubscriptionGivenRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PReasonSubscriptionGivenTable)(this.Table));
        }
        
        /// 
        public String Code
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCode) 
                            || (((String)(this[this.myTable.ColumnCode])) != value)))
                {
                    this[this.myTable.ColumnCode] = value;
                }
            }
        }
        
        /// 
        public String Description
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDescription) 
                            || (((String)(this[this.myTable.ColumnDescription])) != value)))
                {
                    this[this.myTable.ColumnDescription] = value;
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
            this.SetNull(this.myTable.ColumnCode);
            this.SetNull(this.myTable.ColumnDescription);
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
    
    /// List of reasons for cancelling a subscription
    [Serializable()]
    public class PReasonSubscriptionCancelledTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnCode;
        
        /// 
        public DataColumn ColumnDescription;
        
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
        public PReasonSubscriptionCancelledTable() : 
                base("PReasonSubscriptionCancelled")
        {
        }
        
        /// constructor
        public PReasonSubscriptionCancelledTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PReasonSubscriptionCancelledTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PReasonSubscriptionCancelledRow this[int i]
        {
            get
            {
                return ((PReasonSubscriptionCancelledRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetCodeDBName()
        {
            return "p_code_c";
        }
        
        /// get help text for column
        public static string GetCodeHelp()
        {
            return "Enter a code for reason cancelled";
        }
        
        /// get label of column
        public static string GetCodeLabel()
        {
            return "Reason Cancelled Code";
        }
        
        /// get character length for column
        public static short GetCodeLength()
        {
            return 12;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "p_description_c";
        }
        
        /// get help text for column
        public static string GetDescriptionHelp()
        {
            return "Enter a full description of reason cancelled";
        }
        
        /// get label of column
        public static string GetDescriptionLabel()
        {
            return "Reason";
        }
        
        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 80;
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
            return "PReasonSubscriptionCancelled";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_reason_subscription_cancelled";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Reason Subscription Cancelled";
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
                    "p_code_c",
                    "p_description_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnCode = this.Columns["p_code_c"];
            this.ColumnDescription = this.Columns["p_description_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnCode};
        }
        
        /// get typed set of changes
        public PReasonSubscriptionCancelledTable GetChangesTyped()
        {
            return ((PReasonSubscriptionCancelledTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PReasonSubscriptionCancelledRow NewRowTyped(bool AWithDefaultValues)
        {
            PReasonSubscriptionCancelledRow ret = ((PReasonSubscriptionCancelledRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PReasonSubscriptionCancelledRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PReasonSubscriptionCancelledRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 160);
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
    
    /// List of reasons for cancelling a subscription
    [Serializable()]
    public class PReasonSubscriptionCancelledRow : System.Data.DataRow
    {
        
        private PReasonSubscriptionCancelledTable myTable;
        
        /// Constructor
        public PReasonSubscriptionCancelledRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PReasonSubscriptionCancelledTable)(this.Table));
        }
        
        /// 
        public String Code
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCode) 
                            || (((String)(this[this.myTable.ColumnCode])) != value)))
                {
                    this[this.myTable.ColumnCode] = value;
                }
            }
        }
        
        /// 
        public String Description
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDescription) 
                            || (((String)(this[this.myTable.ColumnDescription])) != value)))
                {
                    this[this.myTable.ColumnDescription] = value;
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
            this.SetNull(this.myTable.ColumnCode);
            this.SetNull(this.myTable.ColumnDescription);
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
    
    /// Details of which partners receive which publications.
    [Serializable()]
    public class PSubscriptionTable : TTypedDataTable
    {
        
        /// The is the key to the publication table
        public DataColumn ColumnPublicationCode;
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        
        /// 
        public DataColumn ColumnPublicationCopies;
        
        /// 
        public DataColumn ColumnReasonSubsGivenCode;
        
        /// 
        public DataColumn ColumnReasonSubsCancelledCode;
        
        /// Date the subscription expires
        public DataColumn ColumnExpiryDate;
        
        /// Provisional date on which the subscription may expire
        public DataColumn ColumnProvisionalExpiryDate;
        
        /// 
        public DataColumn ColumnGratisSubscription;
        
        /// 
        public DataColumn ColumnDateNoticeSent;
        
        /// 
        public DataColumn ColumnDateCancelled;
        
        /// 
        public DataColumn ColumnStartDate;
        
        /// 
        public DataColumn ColumnNumberIssuesReceived;
        
        /// The number of issues sent after a subscription has ceased
        public DataColumn ColumnNumberComplimentary;
        
        /// 
        public DataColumn ColumnSubscriptionRenewalDate;
        
        /// 
        public DataColumn ColumnSubscriptionStatus;
        
        /// 
        public DataColumn ColumnFirstIssue;
        
        /// 
        public DataColumn ColumnLastIssue;
        
        /// 
        public DataColumn ColumnGiftFromKey;
        
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
        public PSubscriptionTable() : 
                base("PSubscription")
        {
        }
        
        /// constructor
        public PSubscriptionTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PSubscriptionTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PSubscriptionRow this[int i]
        {
            get
            {
                return ((PSubscriptionRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetPublicationCodeDBName()
        {
            return "p_publication_code_c";
        }
        
        /// get help text for column
        public static string GetPublicationCodeHelp()
        {
            return "Enter a publication code";
        }
        
        /// get label of column
        public static string GetPublicationCodeLabel()
        {
            return "Publication Code";
        }
        
        /// get character length for column
        public static short GetPublicationCodeLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }
        
        /// get help text for column
        public static string GetPartnerKeyHelp()
        {
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get display format for column
        public static short GetPartnerKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPublicationCopiesDBName()
        {
            return "p_publication_copies_i";
        }
        
        /// get help text for column
        public static string GetPublicationCopiesHelp()
        {
            return "Enter the number of subscription issues";
        }
        
        /// get label of column
        public static string GetPublicationCopiesLabel()
        {
            return "Copies";
        }
        
        /// get display format for column
        public static short GetPublicationCopiesLength()
        {
            return 4;
        }
        
        /// get the name of the field in the database for this column
        public static string GetReasonSubsGivenCodeDBName()
        {
            return "p_reason_subs_given_code_c";
        }
        
        /// get help text for column
        public static string GetReasonSubsGivenCodeHelp()
        {
            return "Enter a reason subscription was given";
        }
        
        /// get label of column
        public static string GetReasonSubsGivenCodeLabel()
        {
            return "Reason Given";
        }
        
        /// get character length for column
        public static short GetReasonSubsGivenCodeLength()
        {
            return 12;
        }
        
        /// get the name of the field in the database for this column
        public static string GetReasonSubsCancelledCodeDBName()
        {
            return "p_reason_subs_cancelled_code_c";
        }
        
        /// get help text for column
        public static string GetReasonSubsCancelledCodeHelp()
        {
            return "Enter a reason subscription was cancelled";
        }
        
        /// get label of column
        public static string GetReasonSubsCancelledCodeLabel()
        {
            return "Reason Cancelled";
        }
        
        /// get character length for column
        public static short GetReasonSubsCancelledCodeLength()
        {
            return 12;
        }
        
        /// get the name of the field in the database for this column
        public static string GetExpiryDateDBName()
        {
            return "p_expiry_date_d";
        }
        
        /// get help text for column
        public static string GetExpiryDateHelp()
        {
            return "Enter the date this subscription discontinues (if necessary)";
        }
        
        /// get label of column
        public static string GetExpiryDateLabel()
        {
            return "Expiry Date";
        }
        
        /// get display format for column
        public static short GetExpiryDateLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetProvisionalExpiryDateDBName()
        {
            return "p_provisional_expiry_date_d";
        }
        
        /// get help text for column
        public static string GetProvisionalExpiryDateHelp()
        {
            return "Provisional date on which the subscription may expire";
        }
        
        /// get label of column
        public static string GetProvisionalExpiryDateLabel()
        {
            return "Provisional Expiry Date";
        }
        
        /// get display format for column
        public static short GetProvisionalExpiryDateLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetGratisSubscriptionDBName()
        {
            return "p_gratis_subscription_l";
        }
        
        /// get help text for column
        public static string GetGratisSubscriptionHelp()
        {
            return "Select if this a free subscription (no dates updated)";
        }
        
        /// get label of column
        public static string GetGratisSubscriptionLabel()
        {
            return "Free Subscription";
        }
        
        /// get display format for column
        public static short GetGratisSubscriptionLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateNoticeSentDBName()
        {
            return "p_date_notice_sent_d";
        }
        
        /// get help text for column
        public static string GetDateNoticeSentHelp()
        {
            return "Enter the date a notice was sent (if necessary)";
        }
        
        /// get label of column
        public static string GetDateNoticeSentLabel()
        {
            return "Date Notice Sent";
        }
        
        /// get display format for column
        public static short GetDateNoticeSentLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDateCancelledDBName()
        {
            return "p_date_cancelled_d";
        }
        
        /// get help text for column
        public static string GetDateCancelledHelp()
        {
            return "Enter the date subscription was cancelled (if necessary)";
        }
        
        /// get label of column
        public static string GetDateCancelledLabel()
        {
            return "Date Cancelled";
        }
        
        /// get display format for column
        public static short GetDateCancelledLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetStartDateDBName()
        {
            return "p_start_date_d";
        }
        
        /// get help text for column
        public static string GetStartDateHelp()
        {
            return "Enter the subscription start date";
        }
        
        /// get label of column
        public static string GetStartDateLabel()
        {
            return "Start Date";
        }
        
        /// get display format for column
        public static short GetStartDateLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberIssuesReceivedDBName()
        {
            return "p_number_issues_received_i";
        }
        
        /// get help text for column
        public static string GetNumberIssuesReceivedHelp()
        {
            return "Enter the number of Issues Received";
        }
        
        /// get label of column
        public static string GetNumberIssuesReceivedLabel()
        {
            return "Issues Received";
        }
        
        /// get display format for column
        public static short GetNumberIssuesReceivedLength()
        {
            return 4;
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberComplimentaryDBName()
        {
            return "p_number_complimentary_i";
        }
        
        /// get help text for column
        public static string GetNumberComplimentaryHelp()
        {
            return "Enter the number of Complimentary issues to be given";
        }
        
        /// get label of column
        public static string GetNumberComplimentaryLabel()
        {
            return "Complimentary";
        }
        
        /// get display format for column
        public static short GetNumberComplimentaryLength()
        {
            return 4;
        }
        
        /// get the name of the field in the database for this column
        public static string GetSubscriptionRenewalDateDBName()
        {
            return "p_subscription_renewal_date_d";
        }
        
        /// get help text for column
        public static string GetSubscriptionRenewalDateHelp()
        {
            return "Enter the projected renewal date (if necessary)";
        }
        
        /// get label of column
        public static string GetSubscriptionRenewalDateLabel()
        {
            return "Renewal Date";
        }
        
        /// get display format for column
        public static short GetSubscriptionRenewalDateLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetSubscriptionStatusDBName()
        {
            return "p_subscription_status_c";
        }
        
        /// get help text for column
        public static string GetSubscriptionStatusHelp()
        {
            return "Select the current status of the subscription";
        }
        
        /// get label of column
        public static string GetSubscriptionStatusLabel()
        {
            return "Status";
        }
        
        /// get character length for column
        public static short GetSubscriptionStatusLength()
        {
            return 12;
        }
        
        /// get the name of the field in the database for this column
        public static string GetFirstIssueDBName()
        {
            return "p_first_issue_d";
        }
        
        /// get help text for column
        public static string GetFirstIssueHelp()
        {
            return "Date the first issue was sent to the Partner";
        }
        
        /// get label of column
        public static string GetFirstIssueLabel()
        {
            return "First Issue";
        }
        
        /// get display format for column
        public static short GetFirstIssueLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetLastIssueDBName()
        {
            return "p_last_issue_d";
        }
        
        /// get help text for column
        public static string GetLastIssueHelp()
        {
            return "Date the most recent issue was sent to this Partner";
        }
        
        /// get label of column
        public static string GetLastIssueLabel()
        {
            return "Last Issue";
        }
        
        /// get display format for column
        public static short GetLastIssueLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetGiftFromKeyDBName()
        {
            return "p_gift_from_key_n";
        }
        
        /// get help text for column
        public static string GetGiftFromKeyHelp()
        {
            return "Who gave this subscription as a gift";
        }
        
        /// get label of column
        public static string GetGiftFromKeyLabel()
        {
            return "Gift Given By";
        }
        
        /// get display format for column
        public static short GetGiftFromKeyLength()
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
            return "PSubscription";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_subscription";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Subscription";
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
                    "p_publication_code_c",
                    "p_partner_key_n",
                    "p_publication_copies_i",
                    "p_reason_subs_given_code_c",
                    "p_reason_subs_cancelled_code_c",
                    "p_expiry_date_d",
                    "p_provisional_expiry_date_d",
                    "p_gratis_subscription_l",
                    "p_date_notice_sent_d",
                    "p_date_cancelled_d",
                    "p_start_date_d",
                    "p_number_issues_received_i",
                    "p_number_complimentary_i",
                    "p_subscription_renewal_date_d",
                    "p_subscription_status_c",
                    "p_first_issue_d",
                    "p_last_issue_d",
                    "p_gift_from_key_n",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnPublicationCode = this.Columns["p_publication_code_c"];
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnPublicationCopies = this.Columns["p_publication_copies_i"];
            this.ColumnReasonSubsGivenCode = this.Columns["p_reason_subs_given_code_c"];
            this.ColumnReasonSubsCancelledCode = this.Columns["p_reason_subs_cancelled_code_c"];
            this.ColumnExpiryDate = this.Columns["p_expiry_date_d"];
            this.ColumnProvisionalExpiryDate = this.Columns["p_provisional_expiry_date_d"];
            this.ColumnGratisSubscription = this.Columns["p_gratis_subscription_l"];
            this.ColumnDateNoticeSent = this.Columns["p_date_notice_sent_d"];
            this.ColumnDateCancelled = this.Columns["p_date_cancelled_d"];
            this.ColumnStartDate = this.Columns["p_start_date_d"];
            this.ColumnNumberIssuesReceived = this.Columns["p_number_issues_received_i"];
            this.ColumnNumberComplimentary = this.Columns["p_number_complimentary_i"];
            this.ColumnSubscriptionRenewalDate = this.Columns["p_subscription_renewal_date_d"];
            this.ColumnSubscriptionStatus = this.Columns["p_subscription_status_c"];
            this.ColumnFirstIssue = this.Columns["p_first_issue_d"];
            this.ColumnLastIssue = this.Columns["p_last_issue_d"];
            this.ColumnGiftFromKey = this.Columns["p_gift_from_key_n"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnPublicationCode,
                    this.ColumnPartnerKey};
        }
        
        /// get typed set of changes
        public PSubscriptionTable GetChangesTyped()
        {
            return ((PSubscriptionTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PSubscriptionRow NewRowTyped(bool AWithDefaultValues)
        {
            PSubscriptionRow ret = ((PSubscriptionRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PSubscriptionRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PSubscriptionRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_publication_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("p_publication_copies_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_reason_subs_given_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_reason_subs_cancelled_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_expiry_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_provisional_expiry_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_gratis_subscription_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_date_notice_sent_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_date_cancelled_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_start_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_number_issues_received_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_number_complimentary_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_subscription_renewal_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_subscription_status_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_first_issue_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_last_issue_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("p_gift_from_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnPublicationCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnPublicationCopies))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnReasonSubsGivenCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnReasonSubsCancelledCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnExpiryDate))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnProvisionalExpiryDate))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnGratisSubscription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnDateNoticeSent))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnDateCancelled))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnStartDate))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnNumberIssuesReceived))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNumberComplimentary))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnSubscriptionRenewalDate))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnSubscriptionStatus))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnFirstIssue))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnLastIssue))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnGiftFromKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
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
    
    /// Details of which partners receive which publications.
    [Serializable()]
    public class PSubscriptionRow : System.Data.DataRow
    {
        
        private PSubscriptionTable myTable;
        
        /// Constructor
        public PSubscriptionRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PSubscriptionTable)(this.Table));
        }
        
        /// The is the key to the publication table
        public String PublicationCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPublicationCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPublicationCode) 
                            || (((String)(this[this.myTable.ColumnPublicationCode])) != value)))
                {
                    this[this.myTable.ColumnPublicationCode] = value;
                }
            }
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
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
        
        /// 
        public Int32 PublicationCopies
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnPublicationCopies.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnPublicationCopies) 
                            || (((Int32)(this[this.myTable.ColumnPublicationCopies])) != value)))
                {
                    this[this.myTable.ColumnPublicationCopies] = value;
                }
            }
        }
        
        /// 
        public String ReasonSubsGivenCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReasonSubsGivenCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnReasonSubsGivenCode) 
                            || (((String)(this[this.myTable.ColumnReasonSubsGivenCode])) != value)))
                {
                    this[this.myTable.ColumnReasonSubsGivenCode] = value;
                }
            }
        }
        
        /// 
        public String ReasonSubsCancelledCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnReasonSubsCancelledCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnReasonSubsCancelledCode) 
                            || (((String)(this[this.myTable.ColumnReasonSubsCancelledCode])) != value)))
                {
                    this[this.myTable.ColumnReasonSubsCancelledCode] = value;
                }
            }
        }
        
        /// Date the subscription expires
        public System.DateTime ExpiryDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnExpiryDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnExpiryDate) 
                            || (((System.DateTime)(this[this.myTable.ColumnExpiryDate])) != value)))
                {
                    this[this.myTable.ColumnExpiryDate] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ExpiryDateLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnExpiryDate], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ExpiryDateHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnExpiryDate.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// Provisional date on which the subscription may expire
        public System.DateTime ProvisionalExpiryDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnProvisionalExpiryDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnProvisionalExpiryDate) 
                            || (((System.DateTime)(this[this.myTable.ColumnProvisionalExpiryDate])) != value)))
                {
                    this[this.myTable.ColumnProvisionalExpiryDate] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ProvisionalExpiryDateLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnProvisionalExpiryDate], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ProvisionalExpiryDateHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnProvisionalExpiryDate.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// 
        public Boolean GratisSubscription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGratisSubscription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGratisSubscription) 
                            || (((Boolean)(this[this.myTable.ColumnGratisSubscription])) != value)))
                {
                    this[this.myTable.ColumnGratisSubscription] = value;
                }
            }
        }
        
        /// 
        public System.DateTime DateNoticeSent
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateNoticeSent.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDateNoticeSent) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateNoticeSent])) != value)))
                {
                    this[this.myTable.ColumnDateNoticeSent] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateNoticeSentLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateNoticeSent], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateNoticeSentHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateNoticeSent.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// 
        public System.DateTime DateCancelled
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDateCancelled.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDateCancelled) 
                            || (((System.DateTime)(this[this.myTable.ColumnDateCancelled])) != value)))
                {
                    this[this.myTable.ColumnDateCancelled] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime DateCancelledLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCancelled], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime DateCancelledHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnDateCancelled.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// 
        public System.DateTime StartDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnStartDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnStartDate) 
                            || (((System.DateTime)(this[this.myTable.ColumnStartDate])) != value)))
                {
                    this[this.myTable.ColumnStartDate] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime StartDateLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnStartDate], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime StartDateHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnStartDate.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// 
        public Int32 NumberIssuesReceived
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberIssuesReceived.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberIssuesReceived) 
                            || (((Int32)(this[this.myTable.ColumnNumberIssuesReceived])) != value)))
                {
                    this[this.myTable.ColumnNumberIssuesReceived] = value;
                }
            }
        }
        
        /// The number of issues sent after a subscription has ceased
        public Int32 NumberComplimentary
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberComplimentary.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberComplimentary) 
                            || (((Int32)(this[this.myTable.ColumnNumberComplimentary])) != value)))
                {
                    this[this.myTable.ColumnNumberComplimentary] = value;
                }
            }
        }
        
        /// 
        public System.DateTime SubscriptionRenewalDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSubscriptionRenewalDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSubscriptionRenewalDate) 
                            || (((System.DateTime)(this[this.myTable.ColumnSubscriptionRenewalDate])) != value)))
                {
                    this[this.myTable.ColumnSubscriptionRenewalDate] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime SubscriptionRenewalDateLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnSubscriptionRenewalDate], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime SubscriptionRenewalDateHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnSubscriptionRenewalDate.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// 
        public String SubscriptionStatus
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnSubscriptionStatus.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnSubscriptionStatus) 
                            || (((String)(this[this.myTable.ColumnSubscriptionStatus])) != value)))
                {
                    this[this.myTable.ColumnSubscriptionStatus] = value;
                }
            }
        }
        
        /// 
        public System.DateTime FirstIssue
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFirstIssue.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnFirstIssue) 
                            || (((System.DateTime)(this[this.myTable.ColumnFirstIssue])) != value)))
                {
                    this[this.myTable.ColumnFirstIssue] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime FirstIssueLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnFirstIssue], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime FirstIssueHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnFirstIssue.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// 
        public System.DateTime LastIssue
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLastIssue.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnLastIssue) 
                            || (((System.DateTime)(this[this.myTable.ColumnLastIssue])) != value)))
                {
                    this[this.myTable.ColumnLastIssue] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime LastIssueLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnLastIssue], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime LastIssueHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnLastIssue.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// 
        public Int64 GiftFromKey
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnGiftFromKey.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnGiftFromKey) 
                            || (((Int64)(this[this.myTable.ColumnGiftFromKey])) != value)))
                {
                    this[this.myTable.ColumnGiftFromKey] = value;
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
            this.SetNull(this.myTable.ColumnPublicationCode);
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this[this.myTable.ColumnPublicationCopies.Ordinal] = 1;
            this.SetNull(this.myTable.ColumnReasonSubsGivenCode);
            this.SetNull(this.myTable.ColumnReasonSubsCancelledCode);
            this.SetNull(this.myTable.ColumnExpiryDate);
            this.SetNull(this.myTable.ColumnProvisionalExpiryDate);
            this[this.myTable.ColumnGratisSubscription.Ordinal] = true;
            this.SetNull(this.myTable.ColumnDateNoticeSent);
            this.SetNull(this.myTable.ColumnDateCancelled);
            this[this.myTable.ColumnStartDate.Ordinal] = DateTime.Today;
            this[this.myTable.ColumnNumberIssuesReceived.Ordinal] = 0;
            this[this.myTable.ColumnNumberComplimentary.Ordinal] = 1;
            this.SetNull(this.myTable.ColumnSubscriptionRenewalDate);
            this[this.myTable.ColumnSubscriptionStatus.Ordinal] = "PERMANENT";
            this.SetNull(this.myTable.ColumnFirstIssue);
            this.SetNull(this.myTable.ColumnLastIssue);
            this[this.myTable.ColumnGiftFromKey.Ordinal] = 0;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsPublicationCopiesNull()
        {
            return this.IsNull(this.myTable.ColumnPublicationCopies);
        }
        
        /// assign NULL value
        public void SetPublicationCopiesNull()
        {
            this.SetNull(this.myTable.ColumnPublicationCopies);
        }
        
        /// test for NULL value
        public bool IsReasonSubsCancelledCodeNull()
        {
            return this.IsNull(this.myTable.ColumnReasonSubsCancelledCode);
        }
        
        /// assign NULL value
        public void SetReasonSubsCancelledCodeNull()
        {
            this.SetNull(this.myTable.ColumnReasonSubsCancelledCode);
        }
        
        /// test for NULL value
        public bool IsExpiryDateNull()
        {
            return this.IsNull(this.myTable.ColumnExpiryDate);
        }
        
        /// assign NULL value
        public void SetExpiryDateNull()
        {
            this.SetNull(this.myTable.ColumnExpiryDate);
        }
        
        /// test for NULL value
        public bool IsProvisionalExpiryDateNull()
        {
            return this.IsNull(this.myTable.ColumnProvisionalExpiryDate);
        }
        
        /// assign NULL value
        public void SetProvisionalExpiryDateNull()
        {
            this.SetNull(this.myTable.ColumnProvisionalExpiryDate);
        }
        
        /// test for NULL value
        public bool IsDateNoticeSentNull()
        {
            return this.IsNull(this.myTable.ColumnDateNoticeSent);
        }
        
        /// assign NULL value
        public void SetDateNoticeSentNull()
        {
            this.SetNull(this.myTable.ColumnDateNoticeSent);
        }
        
        /// test for NULL value
        public bool IsDateCancelledNull()
        {
            return this.IsNull(this.myTable.ColumnDateCancelled);
        }
        
        /// assign NULL value
        public void SetDateCancelledNull()
        {
            this.SetNull(this.myTable.ColumnDateCancelled);
        }
        
        /// test for NULL value
        public bool IsSubscriptionRenewalDateNull()
        {
            return this.IsNull(this.myTable.ColumnSubscriptionRenewalDate);
        }
        
        /// assign NULL value
        public void SetSubscriptionRenewalDateNull()
        {
            this.SetNull(this.myTable.ColumnSubscriptionRenewalDate);
        }
        
        /// test for NULL value
        public bool IsSubscriptionStatusNull()
        {
            return this.IsNull(this.myTable.ColumnSubscriptionStatus);
        }
        
        /// assign NULL value
        public void SetSubscriptionStatusNull()
        {
            this.SetNull(this.myTable.ColumnSubscriptionStatus);
        }
        
        /// test for NULL value
        public bool IsFirstIssueNull()
        {
            return this.IsNull(this.myTable.ColumnFirstIssue);
        }
        
        /// assign NULL value
        public void SetFirstIssueNull()
        {
            this.SetNull(this.myTable.ColumnFirstIssue);
        }
        
        /// test for NULL value
        public bool IsLastIssueNull()
        {
            return this.IsNull(this.myTable.ColumnLastIssue);
        }
        
        /// assign NULL value
        public void SetLastIssueNull()
        {
            this.SetNull(this.myTable.ColumnLastIssue);
        }
        
        /// test for NULL value
        public bool IsGiftFromKeyNull()
        {
            return this.IsNull(this.myTable.ColumnGiftFromKey);
        }
        
        /// assign NULL value
        public void SetGiftFromKeyNull()
        {
            this.SetNull(this.myTable.ColumnGiftFromKey);
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
    
    /// Possible attributes for partner contacts.  Gives the description of each attribute code.  An attribute is a type of contact that was made or which occurred with a partner.
    [Serializable()]
    public class PContactAttributeTable : TTypedDataTable
    {
        
        /// Contact Attribute Code
        public DataColumn ColumnContactAttributeCode;
        
        /// This is a contact attribute description.
        public DataColumn ColumnContactAttributeDescr;
        
        /// allowed to use this attribute for new contacts?
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
        public PContactAttributeTable() : 
                base("PContactAttribute")
        {
        }
        
        /// constructor
        public PContactAttributeTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PContactAttributeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PContactAttributeRow this[int i]
        {
            get
            {
                return ((PContactAttributeRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactAttributeCodeDBName()
        {
            return "p_contact_attribute_code_c";
        }
        
        /// get help text for column
        public static string GetContactAttributeCodeHelp()
        {
            return "Enter a contact attribute code";
        }
        
        /// get label of column
        public static string GetContactAttributeCodeLabel()
        {
            return "Contact Attribute Code";
        }
        
        /// get character length for column
        public static short GetContactAttributeCodeLength()
        {
            return 16;
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactAttributeDescrDBName()
        {
            return "p_contact_attribute_descr_c";
        }
        
        /// get help text for column
        public static string GetContactAttributeDescrHelp()
        {
            return "Enter a description";
        }
        
        /// get label of column
        public static string GetContactAttributeDescrLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetContactAttributeDescrLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetActiveDBName()
        {
            return "p_active_l";
        }
        
        /// get help text for column
        public static string GetActiveHelp()
        {
            return "allowed to use this attribute for new contacts?";
        }
        
        /// get label of column
        public static string GetActiveLabel()
        {
            return "Active";
        }
        
        /// get display format for column
        public static short GetActiveLength()
        {
            return 6;
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
            return "PContactAttribute";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_contact_attribute";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "p_contact_attribute";
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
                    "p_contact_attribute_code_c",
                    "p_contact_attribute_descr_c",
                    "p_active_l",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnContactAttributeCode = this.Columns["p_contact_attribute_code_c"];
            this.ColumnContactAttributeDescr = this.Columns["p_contact_attribute_descr_c"];
            this.ColumnActive = this.Columns["p_active_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnContactAttributeCode};
        }
        
        /// get typed set of changes
        public PContactAttributeTable GetChangesTyped()
        {
            return ((PContactAttributeTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PContactAttributeRow NewRowTyped(bool AWithDefaultValues)
        {
            PContactAttributeRow ret = ((PContactAttributeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PContactAttributeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PContactAttributeRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_contact_attribute_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_contact_attribute_descr_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_active_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnContactAttributeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 32);
            }
            if ((ACol == ColumnContactAttributeDescr))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
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
    
    /// Possible attributes for partner contacts.  Gives the description of each attribute code.  An attribute is a type of contact that was made or which occurred with a partner.
    [Serializable()]
    public class PContactAttributeRow : System.Data.DataRow
    {
        
        private PContactAttributeTable myTable;
        
        /// Constructor
        public PContactAttributeRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PContactAttributeTable)(this.Table));
        }
        
        /// Contact Attribute Code
        public String ContactAttributeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactAttributeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactAttributeCode) 
                            || (((String)(this[this.myTable.ColumnContactAttributeCode])) != value)))
                {
                    this[this.myTable.ColumnContactAttributeCode] = value;
                }
            }
        }
        
        /// This is a contact attribute description.
        public String ContactAttributeDescr
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactAttributeDescr.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactAttributeDescr) 
                            || (((String)(this[this.myTable.ColumnContactAttributeDescr])) != value)))
                {
                    this[this.myTable.ColumnContactAttributeDescr] = value;
                }
            }
        }
        
        /// allowed to use this attribute for new contacts?
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
            this.SetNull(this.myTable.ColumnContactAttributeCode);
            this.SetNull(this.myTable.ColumnContactAttributeDescr);
            this[this.myTable.ColumnActive.Ordinal] = true;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsContactAttributeDescrNull()
        {
            return this.IsNull(this.myTable.ColumnContactAttributeDescr);
        }
        
        /// assign NULL value
        public void SetContactAttributeDescrNull()
        {
            this.SetNull(this.myTable.ColumnContactAttributeDescr);
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
    
    /// Possible attribute details for each contact attribute.  Breaks down the attribute into more specifice information that applies to a contact with a partner.
    [Serializable()]
    public class PContactAttributeDetailTable : TTypedDataTable
    {
        
        /// Contact Attribute Code
        public DataColumn ColumnContactAttributeCode;
        
        /// code for attribute detail
        public DataColumn ColumnContactAttrDetailCode;
        
        /// This is a contact attribute detail description.
        public DataColumn ColumnContactAttrDetailDescr;
        
        /// allowed to use this attribute detail for new contacts?
        public DataColumn ColumnActive;
        
        /// Contact attribute detail comment
        public DataColumn ColumnComment;
        
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
        public PContactAttributeDetailTable() : 
                base("PContactAttributeDetail")
        {
        }
        
        /// constructor
        public PContactAttributeDetailTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PContactAttributeDetailTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PContactAttributeDetailRow this[int i]
        {
            get
            {
                return ((PContactAttributeDetailRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactAttributeCodeDBName()
        {
            return "p_contact_attribute_code_c";
        }
        
        /// get help text for column
        public static string GetContactAttributeCodeHelp()
        {
            return "Enter a contact attribute code";
        }
        
        /// get label of column
        public static string GetContactAttributeCodeLabel()
        {
            return "Contact Attribute Code";
        }
        
        /// get character length for column
        public static short GetContactAttributeCodeLength()
        {
            return 16;
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactAttrDetailCodeDBName()
        {
            return "p_contact_attr_detail_code_c";
        }
        
        /// get help text for column
        public static string GetContactAttrDetailCodeHelp()
        {
            return "code for attribute detail";
        }
        
        /// get label of column
        public static string GetContactAttrDetailCodeLabel()
        {
            return "Attribute Detail Code";
        }
        
        /// get character length for column
        public static short GetContactAttrDetailCodeLength()
        {
            return 16;
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactAttrDetailDescrDBName()
        {
            return "p_contact_attr_detail_descr_c";
        }
        
        /// get help text for column
        public static string GetContactAttrDetailDescrHelp()
        {
            return "Enter a description";
        }
        
        /// get label of column
        public static string GetContactAttrDetailDescrLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetContactAttrDetailDescrLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetActiveDBName()
        {
            return "p_active_l";
        }
        
        /// get help text for column
        public static string GetActiveHelp()
        {
            return "allowed to use this attribute detail for new contacts?";
        }
        
        /// get label of column
        public static string GetActiveLabel()
        {
            return "Active";
        }
        
        /// get display format for column
        public static short GetActiveLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCommentDBName()
        {
            return "p_comment_c";
        }
        
        /// get help text for column
        public static string GetCommentHelp()
        {
            return "Enter a comment.";
        }
        
        /// get label of column
        public static string GetCommentLabel()
        {
            return "Comment";
        }
        
        /// get character length for column
        public static short GetCommentLength()
        {
            return 4000;
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
            return "PContactAttributeDetail";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_contact_attribute_detail";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "p_contact_attribute_detail";
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
                    "p_contact_attribute_code_c",
                    "p_contact_attr_detail_code_c",
                    "p_contact_attr_detail_descr_c",
                    "p_active_l",
                    "p_comment_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnContactAttributeCode = this.Columns["p_contact_attribute_code_c"];
            this.ColumnContactAttrDetailCode = this.Columns["p_contact_attr_detail_code_c"];
            this.ColumnContactAttrDetailDescr = this.Columns["p_contact_attr_detail_descr_c"];
            this.ColumnActive = this.Columns["p_active_l"];
            this.ColumnComment = this.Columns["p_comment_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnContactAttributeCode,
                    this.ColumnContactAttrDetailCode};
        }
        
        /// get typed set of changes
        public PContactAttributeDetailTable GetChangesTyped()
        {
            return ((PContactAttributeDetailTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PContactAttributeDetailRow NewRowTyped(bool AWithDefaultValues)
        {
            PContactAttributeDetailRow ret = ((PContactAttributeDetailRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PContactAttributeDetailRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PContactAttributeDetailRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_contact_attribute_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_contact_attr_detail_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_contact_attr_detail_descr_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_active_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_comment_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnContactAttributeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 32);
            }
            if ((ACol == ColumnContactAttrDetailCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 32);
            }
            if ((ACol == ColumnContactAttrDetailDescr))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
            }
            if ((ACol == ColumnActive))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnComment))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 8000);
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
    
    /// Possible attribute details for each contact attribute.  Breaks down the attribute into more specifice information that applies to a contact with a partner.
    [Serializable()]
    public class PContactAttributeDetailRow : System.Data.DataRow
    {
        
        private PContactAttributeDetailTable myTable;
        
        /// Constructor
        public PContactAttributeDetailRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PContactAttributeDetailTable)(this.Table));
        }
        
        /// Contact Attribute Code
        public String ContactAttributeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactAttributeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactAttributeCode) 
                            || (((String)(this[this.myTable.ColumnContactAttributeCode])) != value)))
                {
                    this[this.myTable.ColumnContactAttributeCode] = value;
                }
            }
        }
        
        /// code for attribute detail
        public String ContactAttrDetailCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactAttrDetailCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactAttrDetailCode) 
                            || (((String)(this[this.myTable.ColumnContactAttrDetailCode])) != value)))
                {
                    this[this.myTable.ColumnContactAttrDetailCode] = value;
                }
            }
        }
        
        /// This is a contact attribute detail description.
        public String ContactAttrDetailDescr
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactAttrDetailDescr.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactAttrDetailDescr) 
                            || (((String)(this[this.myTable.ColumnContactAttrDetailDescr])) != value)))
                {
                    this[this.myTable.ColumnContactAttrDetailDescr] = value;
                }
            }
        }
        
        /// allowed to use this attribute detail for new contacts?
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
        
        /// Contact attribute detail comment
        public String Comment
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnComment.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnComment) 
                            || (((String)(this[this.myTable.ColumnComment])) != value)))
                {
                    this[this.myTable.ColumnComment] = value;
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
            this.SetNull(this.myTable.ColumnContactAttributeCode);
            this.SetNull(this.myTable.ColumnContactAttrDetailCode);
            this.SetNull(this.myTable.ColumnContactAttrDetailDescr);
            this[this.myTable.ColumnActive.Ordinal] = true;
            this.SetNull(this.myTable.ColumnComment);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsContactAttrDetailDescrNull()
        {
            return this.IsNull(this.myTable.ColumnContactAttrDetailDescr);
        }
        
        /// assign NULL value
        public void SetContactAttrDetailDescrNull()
        {
            this.SetNull(this.myTable.ColumnContactAttrDetailDescr);
        }
        
        /// test for NULL value
        public bool IsCommentNull()
        {
            return this.IsNull(this.myTable.ColumnComment);
        }
        
        /// assign NULL value
        public void SetCommentNull()
        {
            this.SetNull(this.myTable.ColumnComment);
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
    
    /// How contacts are made
    [Serializable()]
    public class PMethodOfContactTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnMethodOfContactCode;
        
        /// 
        public DataColumn ColumnDescription;
        
        /// 
        public DataColumn ColumnContactType;
        
        /// 
        public DataColumn ColumnValidMethod;
        
        /// This defines if the method of contact code can be deleted.
        ///This can only be updated by the system manager.
        ///At the risk of serious operational integrity.
        ///Default to Yes
        public DataColumn ColumnDeletable;
        
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
        public PMethodOfContactTable() : 
                base("PMethodOfContact")
        {
        }
        
        /// constructor
        public PMethodOfContactTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PMethodOfContactTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PMethodOfContactRow this[int i]
        {
            get
            {
                return ((PMethodOfContactRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetMethodOfContactCodeDBName()
        {
            return "p_method_of_contact_code_c";
        }
        
        /// get help text for column
        public static string GetMethodOfContactCodeHelp()
        {
            return "Enter a method of contact code";
        }
        
        /// get label of column
        public static string GetMethodOfContactCodeLabel()
        {
            return "Method of Contact";
        }
        
        /// get character length for column
        public static short GetMethodOfContactCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "p_description_c";
        }
        
        /// get help text for column
        public static string GetDescriptionHelp()
        {
            return "Enter the full description";
        }
        
        /// get label of column
        public static string GetDescriptionLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactTypeDBName()
        {
            return "p_contact_type_c";
        }
        
        /// get help text for column
        public static string GetContactTypeHelp()
        {
            return "Select a contact type";
        }
        
        /// get label of column
        public static string GetContactTypeLabel()
        {
            return "Contact Type";
        }
        
        /// get character length for column
        public static short GetContactTypeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetValidMethodDBName()
        {
            return "p_valid_method_l";
        }
        
        /// get help text for column
        public static string GetValidMethodHelp()
        {
            return "Should this option be selectable by users";
        }
        
        /// get label of column
        public static string GetValidMethodLabel()
        {
            return "Valid Method";
        }
        
        /// get display format for column
        public static short GetValidMethodLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetDeletableDBName()
        {
            return "p_deletable_l";
        }
        
        /// get help text for column
        public static string GetDeletableHelp()
        {
            return "This code is Required for System operation by other code";
        }
        
        /// get label of column
        public static string GetDeletableLabel()
        {
            return "Deletable";
        }
        
        /// get display format for column
        public static short GetDeletableLength()
        {
            return 6;
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
            return "PMethodOfContact";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_method_of_contact";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Method of Contact";
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
                    "p_method_of_contact_code_c",
                    "p_description_c",
                    "p_contact_type_c",
                    "p_valid_method_l",
                    "p_deletable_l",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnMethodOfContactCode = this.Columns["p_method_of_contact_code_c"];
            this.ColumnDescription = this.Columns["p_description_c"];
            this.ColumnContactType = this.Columns["p_contact_type_c"];
            this.ColumnValidMethod = this.Columns["p_valid_method_l"];
            this.ColumnDeletable = this.Columns["p_deletable_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnMethodOfContactCode};
        }
        
        /// get typed set of changes
        public PMethodOfContactTable GetChangesTyped()
        {
            return ((PMethodOfContactTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PMethodOfContactRow NewRowTyped(bool AWithDefaultValues)
        {
            PMethodOfContactRow ret = ((PMethodOfContactRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PMethodOfContactRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PMethodOfContactRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_method_of_contact_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_contact_type_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_valid_method_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_deletable_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnMethodOfContactCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
            }
            if ((ACol == ColumnContactType))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnValidMethod))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnDeletable))
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
    
    /// How contacts are made
    [Serializable()]
    public class PMethodOfContactRow : System.Data.DataRow
    {
        
        private PMethodOfContactTable myTable;
        
        /// Constructor
        public PMethodOfContactRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PMethodOfContactTable)(this.Table));
        }
        
        /// 
        public String MethodOfContactCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMethodOfContactCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMethodOfContactCode) 
                            || (((String)(this[this.myTable.ColumnMethodOfContactCode])) != value)))
                {
                    this[this.myTable.ColumnMethodOfContactCode] = value;
                }
            }
        }
        
        /// 
        public String Description
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDescription.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDescription) 
                            || (((String)(this[this.myTable.ColumnDescription])) != value)))
                {
                    this[this.myTable.ColumnDescription] = value;
                }
            }
        }
        
        /// 
        public String ContactType
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactType.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactType) 
                            || (((String)(this[this.myTable.ColumnContactType])) != value)))
                {
                    this[this.myTable.ColumnContactType] = value;
                }
            }
        }
        
        /// 
        public Boolean ValidMethod
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnValidMethod.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnValidMethod) 
                            || (((Boolean)(this[this.myTable.ColumnValidMethod])) != value)))
                {
                    this[this.myTable.ColumnValidMethod] = value;
                }
            }
        }
        
        /// This defines if the method of contact code can be deleted.
        ///This can only be updated by the system manager.
        ///At the risk of serious operational integrity.
        ///Default to Yes
        public Boolean Deletable
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDeletable.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnDeletable) 
                            || (((Boolean)(this[this.myTable.ColumnDeletable])) != value)))
                {
                    this[this.myTable.ColumnDeletable] = value;
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
            this.SetNull(this.myTable.ColumnMethodOfContactCode);
            this.SetNull(this.myTable.ColumnDescription);
            this.SetNull(this.myTable.ColumnContactType);
            this[this.myTable.ColumnValidMethod.Ordinal] = true;
            this[this.myTable.ColumnDeletable.Ordinal] = true;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnDescription);
        }
        
        /// assign NULL value
        public void SetDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnDescription);
        }
        
        /// test for NULL value
        public bool IsContactTypeNull()
        {
            return this.IsNull(this.myTable.ColumnContactType);
        }
        
        /// assign NULL value
        public void SetContactTypeNull()
        {
            this.SetNull(this.myTable.ColumnContactType);
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
    
    /// Details of contacts with partners
    [Serializable()]
    public class PPartnerContactTable : TTypedDataTable
    {
        
        /// identifying key for p_partner_contact
        public DataColumn ColumnContactId;
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
        public DataColumn ColumnPartnerKey;
        
        /// Date of contact
        public DataColumn ColumnContactDate;
        
        /// Time of contact
        public DataColumn ColumnContactTime;
        
        /// Contact code
        public DataColumn ColumnContactCode;
        
        /// User who made the contact
        public DataColumn ColumnContactor;
        
        /// The Message ID (only applies if the type of contact is an email); this helps to identify the email and to interface with the EMail application
        public DataColumn ColumnContactMessageId;
        
        /// Contact Comment (also used to hold contents of emails)
        public DataColumn ColumnContactComment;
        
        /// Identifies a module. A module is any part of aprogram which is related to each menu entry or to the sub-system. Eg, partner administration, AP, AR etc.
        public DataColumn ColumnModuleId;
        
        /// If set, this contact is restricted to one user.
        public DataColumn ColumnUserId;
        
        /// The mailing code associated with the contact
        public DataColumn ColumnMailingCode;
        
        /// Indicates whether or not the contact has restricted access. If it does then the access will be controlled by s_group_partner_contact
        public DataColumn ColumnRestricted;
        
        /// Location of contact
        public DataColumn ColumnContactLocation;
        
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
        public PPartnerContactTable() : 
                base("PPartnerContact")
        {
        }
        
        /// constructor
        public PPartnerContactTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PPartnerContactTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PPartnerContactRow this[int i]
        {
            get
            {
                return ((PPartnerContactRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactIdDBName()
        {
            return "p_contact_id_i";
        }
        
        /// get help text for column
        public static string GetContactIdHelp()
        {
            return "identifying key for p_partner_contact";
        }
        
        /// get label of column
        public static string GetContactIdLabel()
        {
            return "Number";
        }
        
        /// get display format for column
        public static short GetContactIdLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetPartnerKeyDBName()
        {
            return "p_partner_key_n";
        }
        
        /// get help text for column
        public static string GetPartnerKeyHelp()
        {
            return "Enter the partner key (SiteID + Number)";
        }
        
        /// get label of column
        public static string GetPartnerKeyLabel()
        {
            return "Partner Key";
        }
        
        /// get display format for column
        public static short GetPartnerKeyLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactDateDBName()
        {
            return "s_contact_date_d";
        }
        
        /// get help text for column
        public static string GetContactDateHelp()
        {
            return "Enter the date the contact was made";
        }
        
        /// get label of column
        public static string GetContactDateLabel()
        {
            return "Contact Date";
        }
        
        /// get display format for column
        public static short GetContactDateLength()
        {
            return 11;
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactTimeDBName()
        {
            return "s_contact_time_i";
        }
        
        /// get help text for column
        public static string GetContactTimeHelp()
        {
            return "Time of contact";
        }
        
        /// get label of column
        public static string GetContactTimeLabel()
        {
            return "s_contact_time_i";
        }
        
        /// get display format for column
        public static short GetContactTimeLength()
        {
            return 5;
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactCodeDBName()
        {
            return "p_contact_code_c";
        }
        
        /// get help text for column
        public static string GetContactCodeHelp()
        {
            return "Select a contact code";
        }
        
        /// get label of column
        public static string GetContactCodeLabel()
        {
            return "Contact Code";
        }
        
        /// get character length for column
        public static short GetContactCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactorDBName()
        {
            return "p_contactor_c";
        }
        
        /// get help text for column
        public static string GetContactorHelp()
        {
            return "Enter the User ID";
        }
        
        /// get label of column
        public static string GetContactorLabel()
        {
            return "User ID";
        }
        
        /// get character length for column
        public static short GetContactorLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactMessageIdDBName()
        {
            return "p_contact_message_id_c";
        }
        
        /// get help text for column
        public static string GetContactMessageIdHelp()
        {
            return "The Message ID (only applies if the type of contact is an email); this helps to i" +
                "dentify the email and to interface with the EMail application";
        }
        
        /// get label of column
        public static string GetContactMessageIdLabel()
        {
            return "Message ID";
        }
        
        /// get character length for column
        public static short GetContactMessageIdLength()
        {
            return 100;
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactCommentDBName()
        {
            return "p_contact_comment_c";
        }
        
        /// get help text for column
        public static string GetContactCommentHelp()
        {
            return "Enter any additional information regarding this contact";
        }
        
        /// get label of column
        public static string GetContactCommentLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetContactCommentLength()
        {
            return 15000;
        }
        
        /// get the name of the field in the database for this column
        public static string GetModuleIdDBName()
        {
            return "s_module_id_c";
        }
        
        /// get help text for column
        public static string GetModuleIdHelp()
        {
            return "Identifies a module. A module is any part of aprogram which is related to each me" +
                "nu entry or to the sub-system. Eg, partner administration, AP, AR etc.";
        }
        
        /// get label of column
        public static string GetModuleIdLabel()
        {
            return "Module ID";
        }
        
        /// get character length for column
        public static short GetModuleIdLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetUserIdDBName()
        {
            return "s_user_id_c";
        }
        
        /// get help text for column
        public static string GetUserIdHelp()
        {
            return "If set, this contact is restricted to one user.";
        }
        
        /// get label of column
        public static string GetUserIdLabel()
        {
            return "";
        }
        
        /// get character length for column
        public static short GetUserIdLength()
        {
            return 10;
        }
        
        /// get the name of the field in the database for this column
        public static string GetMailingCodeDBName()
        {
            return "p_mailing_code_c";
        }
        
        /// get help text for column
        public static string GetMailingCodeHelp()
        {
            return "Enter the mailing code associated with the contact";
        }
        
        /// get label of column
        public static string GetMailingCodeLabel()
        {
            return "Mailing Code";
        }
        
        /// get character length for column
        public static short GetMailingCodeLength()
        {
            return 25;
        }
        
        /// get the name of the field in the database for this column
        public static string GetRestrictedDBName()
        {
            return "p_restricted_l";
        }
        
        /// get help text for column
        public static string GetRestrictedHelp()
        {
            return "Should access to this contact be restricted to some people?";
        }
        
        /// get label of column
        public static string GetRestrictedLabel()
        {
            return "Contact Restricted";
        }
        
        /// get display format for column
        public static short GetRestrictedLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactLocationDBName()
        {
            return "p_contact_location_c";
        }
        
        /// get help text for column
        public static string GetContactLocationHelp()
        {
            return "Enter a location for this contact.";
        }
        
        /// get label of column
        public static string GetContactLocationLabel()
        {
            return "Location";
        }
        
        /// get character length for column
        public static short GetContactLocationLength()
        {
            return 4000;
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
            return "PPartnerContact";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_partner_contact";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Partner Contact";
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
                    "p_contact_id_i",
                    "p_partner_key_n",
                    "s_contact_date_d",
                    "s_contact_time_i",
                    "p_contact_code_c",
                    "p_contactor_c",
                    "p_contact_message_id_c",
                    "p_contact_comment_c",
                    "s_module_id_c",
                    "s_user_id_c",
                    "p_mailing_code_c",
                    "p_restricted_l",
                    "p_contact_location_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnContactId = this.Columns["p_contact_id_i"];
            this.ColumnPartnerKey = this.Columns["p_partner_key_n"];
            this.ColumnContactDate = this.Columns["s_contact_date_d"];
            this.ColumnContactTime = this.Columns["s_contact_time_i"];
            this.ColumnContactCode = this.Columns["p_contact_code_c"];
            this.ColumnContactor = this.Columns["p_contactor_c"];
            this.ColumnContactMessageId = this.Columns["p_contact_message_id_c"];
            this.ColumnContactComment = this.Columns["p_contact_comment_c"];
            this.ColumnModuleId = this.Columns["s_module_id_c"];
            this.ColumnUserId = this.Columns["s_user_id_c"];
            this.ColumnMailingCode = this.Columns["p_mailing_code_c"];
            this.ColumnRestricted = this.Columns["p_restricted_l"];
            this.ColumnContactLocation = this.Columns["p_contact_location_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnContactId};
        }
        
        /// get typed set of changes
        public PPartnerContactTable GetChangesTyped()
        {
            return ((PPartnerContactTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PPartnerContactRow NewRowTyped(bool AWithDefaultValues)
        {
            PPartnerContactRow ret = ((PPartnerContactRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PPartnerContactRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PPartnerContactRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_contact_id_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_partner_key_n", typeof(Int64)));
            this.Columns.Add(new System.Data.DataColumn("s_contact_date_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_contact_time_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_contact_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_contactor_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_contact_message_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_contact_comment_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_module_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_user_id_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_mailing_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_restricted_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_contact_location_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnContactId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnPartnerKey))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 10);
            }
            if ((ACol == ColumnContactDate))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Date);
            }
            if ((ACol == ColumnContactTime))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnContactCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnContactor))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnContactMessageId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 200);
            }
            if ((ACol == ColumnContactComment))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 30000);
            }
            if ((ACol == ColumnModuleId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnUserId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnMailingCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 50);
            }
            if ((ACol == ColumnRestricted))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnContactLocation))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 8000);
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
    
    /// Details of contacts with partners
    [Serializable()]
    public class PPartnerContactRow : System.Data.DataRow
    {
        
        private PPartnerContactTable myTable;
        
        /// Constructor
        public PPartnerContactRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PPartnerContactTable)(this.Table));
        }
        
        /// identifying key for p_partner_contact
        public Int32 ContactId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactId.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactId) 
                            || (((Int32)(this[this.myTable.ColumnContactId])) != value)))
                {
                    this[this.myTable.ColumnContactId] = value;
                }
            }
        }
        
        /// This is the partner key assigned to each partner. It consists of the fund id followed by a computer generated six digit number.
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
        
        /// Date of contact
        public System.DateTime ContactDate
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactDate.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactDate) 
                            || (((System.DateTime)(this[this.myTable.ColumnContactDate])) != value)))
                {
                    this[this.myTable.ColumnContactDate] = value;
                }
            }
        }
        
        /// Returns the date value or the minimum date if the date is NULL
        public System.DateTime ContactDateLowNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnContactDate], TNullHandlingEnum.nhReturnLowestDate);
            }
        }
        
        /// Returns the date value or the maximum date if the date is NULL
        public System.DateTime ContactDateHighNull
        {
            get
            {
                return TSaveConvert.ObjectToDate(this[this.myTable.ColumnContactDate.Ordinal], TNullHandlingEnum.nhReturnHighestDate);
            }
        }
        
        /// Time of contact
        public Int32 ContactTime
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactTime.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactTime) 
                            || (((Int32)(this[this.myTable.ColumnContactTime])) != value)))
                {
                    this[this.myTable.ColumnContactTime] = value;
                }
            }
        }
        
        /// Contact code
        public String ContactCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactCode) 
                            || (((String)(this[this.myTable.ColumnContactCode])) != value)))
                {
                    this[this.myTable.ColumnContactCode] = value;
                }
            }
        }
        
        /// User who made the contact
        public String Contactor
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactor.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactor) 
                            || (((String)(this[this.myTable.ColumnContactor])) != value)))
                {
                    this[this.myTable.ColumnContactor] = value;
                }
            }
        }
        
        /// The Message ID (only applies if the type of contact is an email); this helps to identify the email and to interface with the EMail application
        public String ContactMessageId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactMessageId.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactMessageId) 
                            || (((String)(this[this.myTable.ColumnContactMessageId])) != value)))
                {
                    this[this.myTable.ColumnContactMessageId] = value;
                }
            }
        }
        
        /// Contact Comment (also used to hold contents of emails)
        public String ContactComment
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactComment.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactComment) 
                            || (((String)(this[this.myTable.ColumnContactComment])) != value)))
                {
                    this[this.myTable.ColumnContactComment] = value;
                }
            }
        }
        
        /// Identifies a module. A module is any part of aprogram which is related to each menu entry or to the sub-system. Eg, partner administration, AP, AR etc.
        public String ModuleId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModuleId.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnModuleId) 
                            || (((String)(this[this.myTable.ColumnModuleId])) != value)))
                {
                    this[this.myTable.ColumnModuleId] = value;
                }
            }
        }
        
        /// If set, this contact is restricted to one user.
        public String UserId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUserId.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUserId) 
                            || (((String)(this[this.myTable.ColumnUserId])) != value)))
                {
                    this[this.myTable.ColumnUserId] = value;
                }
            }
        }
        
        /// The mailing code associated with the contact
        public String MailingCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnMailingCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnMailingCode) 
                            || (((String)(this[this.myTable.ColumnMailingCode])) != value)))
                {
                    this[this.myTable.ColumnMailingCode] = value;
                }
            }
        }
        
        /// Indicates whether or not the contact has restricted access. If it does then the access will be controlled by s_group_partner_contact
        public Boolean Restricted
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnRestricted.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnRestricted) 
                            || (((Boolean)(this[this.myTable.ColumnRestricted])) != value)))
                {
                    this[this.myTable.ColumnRestricted] = value;
                }
            }
        }
        
        /// Location of contact
        public String ContactLocation
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactLocation.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactLocation) 
                            || (((String)(this[this.myTable.ColumnContactLocation])) != value)))
                {
                    this[this.myTable.ColumnContactLocation] = value;
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
            this[this.myTable.ColumnContactId.Ordinal] = 0;
            this[this.myTable.ColumnPartnerKey.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnContactDate);
            this[this.myTable.ColumnContactTime.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnContactCode);
            this.SetNull(this.myTable.ColumnContactor);
            this.SetNull(this.myTable.ColumnContactMessageId);
            this.SetNull(this.myTable.ColumnContactComment);
            this.SetNull(this.myTable.ColumnModuleId);
            this.SetNull(this.myTable.ColumnUserId);
            this.SetNull(this.myTable.ColumnMailingCode);
            this[this.myTable.ColumnRestricted.Ordinal] = false;
            this.SetNull(this.myTable.ColumnContactLocation);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsContactorNull()
        {
            return this.IsNull(this.myTable.ColumnContactor);
        }
        
        /// assign NULL value
        public void SetContactorNull()
        {
            this.SetNull(this.myTable.ColumnContactor);
        }
        
        /// test for NULL value
        public bool IsContactMessageIdNull()
        {
            return this.IsNull(this.myTable.ColumnContactMessageId);
        }
        
        /// assign NULL value
        public void SetContactMessageIdNull()
        {
            this.SetNull(this.myTable.ColumnContactMessageId);
        }
        
        /// test for NULL value
        public bool IsContactCommentNull()
        {
            return this.IsNull(this.myTable.ColumnContactComment);
        }
        
        /// assign NULL value
        public void SetContactCommentNull()
        {
            this.SetNull(this.myTable.ColumnContactComment);
        }
        
        /// test for NULL value
        public bool IsModuleIdNull()
        {
            return this.IsNull(this.myTable.ColumnModuleId);
        }
        
        /// assign NULL value
        public void SetModuleIdNull()
        {
            this.SetNull(this.myTable.ColumnModuleId);
        }
        
        /// test for NULL value
        public bool IsUserIdNull()
        {
            return this.IsNull(this.myTable.ColumnUserId);
        }
        
        /// assign NULL value
        public void SetUserIdNull()
        {
            this.SetNull(this.myTable.ColumnUserId);
        }
        
        /// test for NULL value
        public bool IsMailingCodeNull()
        {
            return this.IsNull(this.myTable.ColumnMailingCode);
        }
        
        /// assign NULL value
        public void SetMailingCodeNull()
        {
            this.SetNull(this.myTable.ColumnMailingCode);
        }
        
        /// test for NULL value
        public bool IsRestrictedNull()
        {
            return this.IsNull(this.myTable.ColumnRestricted);
        }
        
        /// assign NULL value
        public void SetRestrictedNull()
        {
            this.SetNull(this.myTable.ColumnRestricted);
        }
        
        /// test for NULL value
        public bool IsContactLocationNull()
        {
            return this.IsNull(this.myTable.ColumnContactLocation);
        }
        
        /// assign NULL value
        public void SetContactLocationNull()
        {
            this.SetNull(this.myTable.ColumnContactLocation);
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
    
    /// Associates a p_contact_attribute_detail with a p_partner_contact.  A contact with a partner may have more than one p_contact_attribute_detail associated with it.
    [Serializable()]
    public class PPartnerContactAttributeTable : TTypedDataTable
    {
        
        /// identifying key for p_partner_contact
        public DataColumn ColumnContactId;
        
        /// Contact Attribute Code
        public DataColumn ColumnContactAttributeCode;
        
        /// code for attribute detail
        public DataColumn ColumnContactAttrDetailCode;
        
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
        public PPartnerContactAttributeTable() : 
                base("PPartnerContactAttribute")
        {
        }
        
        /// constructor
        public PPartnerContactAttributeTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PPartnerContactAttributeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PPartnerContactAttributeRow this[int i]
        {
            get
            {
                return ((PPartnerContactAttributeRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactIdDBName()
        {
            return "p_contact_id_i";
        }
        
        /// get help text for column
        public static string GetContactIdHelp()
        {
            return "identifying key for p_partner_contact";
        }
        
        /// get label of column
        public static string GetContactIdLabel()
        {
            return "Number";
        }
        
        /// get display format for column
        public static short GetContactIdLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactAttributeCodeDBName()
        {
            return "p_contact_attribute_code_c";
        }
        
        /// get help text for column
        public static string GetContactAttributeCodeHelp()
        {
            return "Enter a contact attribute code";
        }
        
        /// get label of column
        public static string GetContactAttributeCodeLabel()
        {
            return "Contact Attribute Code";
        }
        
        /// get character length for column
        public static short GetContactAttributeCodeLength()
        {
            return 16;
        }
        
        /// get the name of the field in the database for this column
        public static string GetContactAttrDetailCodeDBName()
        {
            return "p_contact_attr_detail_code_c";
        }
        
        /// get help text for column
        public static string GetContactAttrDetailCodeHelp()
        {
            return "code for attribute detail";
        }
        
        /// get label of column
        public static string GetContactAttrDetailCodeLabel()
        {
            return "Attribute Detail Code";
        }
        
        /// get character length for column
        public static short GetContactAttrDetailCodeLength()
        {
            return 16;
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
            return "PPartnerContactAttribute";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_partner_contact_attribute";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "p_partner_contact_attribute";
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
                    "p_contact_id_i",
                    "p_contact_attribute_code_c",
                    "p_contact_attr_detail_code_c",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnContactId = this.Columns["p_contact_id_i"];
            this.ColumnContactAttributeCode = this.Columns["p_contact_attribute_code_c"];
            this.ColumnContactAttrDetailCode = this.Columns["p_contact_attr_detail_code_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnContactId,
                    this.ColumnContactAttributeCode,
                    this.ColumnContactAttrDetailCode};
        }
        
        /// get typed set of changes
        public PPartnerContactAttributeTable GetChangesTyped()
        {
            return ((PPartnerContactAttributeTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PPartnerContactAttributeRow NewRowTyped(bool AWithDefaultValues)
        {
            PPartnerContactAttributeRow ret = ((PPartnerContactAttributeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PPartnerContactAttributeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PPartnerContactAttributeRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_contact_id_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_contact_attribute_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_contact_attr_detail_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnContactId))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnContactAttributeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 32);
            }
            if ((ACol == ColumnContactAttrDetailCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 32);
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
    
    /// Associates a p_contact_attribute_detail with a p_partner_contact.  A contact with a partner may have more than one p_contact_attribute_detail associated with it.
    [Serializable()]
    public class PPartnerContactAttributeRow : System.Data.DataRow
    {
        
        private PPartnerContactAttributeTable myTable;
        
        /// Constructor
        public PPartnerContactAttributeRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PPartnerContactAttributeTable)(this.Table));
        }
        
        /// identifying key for p_partner_contact
        public Int32 ContactId
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactId.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactId) 
                            || (((Int32)(this[this.myTable.ColumnContactId])) != value)))
                {
                    this[this.myTable.ColumnContactId] = value;
                }
            }
        }
        
        /// Contact Attribute Code
        public String ContactAttributeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactAttributeCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactAttributeCode) 
                            || (((String)(this[this.myTable.ColumnContactAttributeCode])) != value)))
                {
                    this[this.myTable.ColumnContactAttributeCode] = value;
                }
            }
        }
        
        /// code for attribute detail
        public String ContactAttrDetailCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnContactAttrDetailCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnContactAttrDetailCode) 
                            || (((String)(this[this.myTable.ColumnContactAttrDetailCode])) != value)))
                {
                    this[this.myTable.ColumnContactAttrDetailCode] = value;
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
            this[this.myTable.ColumnContactId.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnContactAttributeCode);
            this.SetNull(this.myTable.ColumnContactAttrDetailCode);
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
}
