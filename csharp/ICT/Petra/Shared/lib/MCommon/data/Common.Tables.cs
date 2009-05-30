/* Auto generated with nant generateORM
 * Do not modify this file manually!
 */
namespace Ict.Petra.Shared.MCommon.Data
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
    
    
    /// List of language codes
    [Serializable()]
    public class PLanguageTable : TTypedDataTable
    {
        
        /// This is the code used to identify a language.
        public DataColumn ColumnLanguageCode;
        
        /// 
        public DataColumn ColumnLanguageDescription;
        
        /// This field indicates whether or not the language is one that is 'officially' used at conferences. These are the languages for which translation could be provided. 
        public DataColumn ColumnCongressLanguage;
        
        /// This defines if the language code can be deleted.
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
        public PLanguageTable() : 
                base("PLanguage")
        {
        }
        
        /// constructor
        public PLanguageTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PLanguageTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PLanguageRow this[int i]
        {
            get
            {
                return ((PLanguageRow)(this.Rows[i]));
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
            return "Enter an internationally accepted language code";
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
        public static string GetLanguageDescriptionDBName()
        {
            return "p_language_description_c";
        }
        
        /// get help text for column
        public static string GetLanguageDescriptionHelp()
        {
            return "Enter a full description or name";
        }
        
        /// get label of column
        public static string GetLanguageDescriptionLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetLanguageDescriptionLength()
        {
            return 40;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCongressLanguageDBName()
        {
            return "p_congress_language_l";
        }
        
        /// get help text for column
        public static string GetCongressLanguageHelp()
        {
            return "Indicates if the language is an \'official\' Congress Language.";
        }
        
        /// get label of column
        public static string GetCongressLanguageLabel()
        {
            return "Congress Language";
        }
        
        /// get display format for column
        public static short GetCongressLanguageLength()
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
            return "PLanguage";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_language";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Language";
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
                    "p_language_code_c",
                    "p_language_description_c",
                    "p_congress_language_l",
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
            this.ColumnLanguageCode = this.Columns["p_language_code_c"];
            this.ColumnLanguageDescription = this.Columns["p_language_description_c"];
            this.ColumnCongressLanguage = this.Columns["p_congress_language_l"];
            this.ColumnDeletable = this.Columns["p_deletable_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnLanguageCode};
        }
        
        /// get typed set of changes
        public PLanguageTable GetChangesTyped()
        {
            return ((PLanguageTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PLanguageRow NewRowTyped(bool AWithDefaultValues)
        {
            PLanguageRow ret = ((PLanguageRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PLanguageRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PLanguageRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_language_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_language_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_congress_language_l", typeof(Boolean)));
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
            if ((ACol == ColumnLanguageCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 20);
            }
            if ((ACol == ColumnLanguageDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 80);
            }
            if ((ACol == ColumnCongressLanguage))
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
    
    /// List of language codes
    [Serializable()]
    public class PLanguageRow : System.Data.DataRow
    {
        
        private PLanguageTable myTable;
        
        /// Constructor
        public PLanguageRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PLanguageTable)(this.Table));
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
        
        /// 
        public String LanguageDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnLanguageDescription.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnLanguageDescription) 
                            || (((String)(this[this.myTable.ColumnLanguageDescription])) != value)))
                {
                    this[this.myTable.ColumnLanguageDescription] = value;
                }
            }
        }
        
        /// This field indicates whether or not the language is one that is 'officially' used at conferences. These are the languages for which translation could be provided. 
        public Boolean CongressLanguage
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCongressLanguage.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnCongressLanguage) 
                            || (((Boolean)(this[this.myTable.ColumnCongressLanguage])) != value)))
                {
                    this[this.myTable.ColumnCongressLanguage] = value;
                }
            }
        }
        
        /// This defines if the language code can be deleted.
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
            this.SetNull(this.myTable.ColumnLanguageCode);
            this.SetNull(this.myTable.ColumnLanguageDescription);
            this[this.myTable.ColumnCongressLanguage.Ordinal] = false;
            this[this.myTable.ColumnDeletable.Ordinal] = true;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsCongressLanguageNull()
        {
            return this.IsNull(this.myTable.ColumnCongressLanguage);
        }
        
        /// assign NULL value
        public void SetCongressLanguageNull()
        {
            this.SetNull(this.myTable.ColumnCongressLanguage);
        }
        
        /// test for NULL value
        public bool IsDeletableNull()
        {
            return this.IsNull(this.myTable.ColumnDeletable);
        }
        
        /// assign NULL value
        public void SetDeletableNull()
        {
            this.SetNull(this.myTable.ColumnDeletable);
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
    
    /// Units of time. Used in partner letters.  Also used to indicate how often a publication is produced or a receipt is sent to a donor.
    [Serializable()]
    public class AFrequencyTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnFrequencyCode;
        
        /// 
        public DataColumn ColumnFrequencyDescription;
        
        /// 
        public DataColumn ColumnNumberOfYears;
        
        /// 
        public DataColumn ColumnNumberOfMonths;
        
        /// 
        public DataColumn ColumnNumberOfDays;
        
        /// 
        public DataColumn ColumnNumberOfHours;
        
        /// 
        public DataColumn ColumnNumberOfMinutes;
        
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
        public AFrequencyTable() : 
                base("AFrequency")
        {
        }
        
        /// constructor
        public AFrequencyTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public AFrequencyTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public AFrequencyRow this[int i]
        {
            get
            {
                return ((AFrequencyRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetFrequencyCodeDBName()
        {
            return "a_frequency_code_c";
        }
        
        /// get help text for column
        public static string GetFrequencyCodeHelp()
        {
            return "";
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
        public static string GetFrequencyDescriptionDBName()
        {
            return "a_frequency_description_c";
        }
        
        /// get help text for column
        public static string GetFrequencyDescriptionHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetFrequencyDescriptionLabel()
        {
            return "Description";
        }
        
        /// get character length for column
        public static short GetFrequencyDescriptionLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfYearsDBName()
        {
            return "a_number_of_years_i";
        }
        
        /// get help text for column
        public static string GetNumberOfYearsHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetNumberOfYearsLabel()
        {
            return "Number of Years";
        }
        
        /// get display format for column
        public static short GetNumberOfYearsLength()
        {
            return 2;
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfMonthsDBName()
        {
            return "a_number_of_months_i";
        }
        
        /// get help text for column
        public static string GetNumberOfMonthsHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetNumberOfMonthsLabel()
        {
            return "Number of Months";
        }
        
        /// get display format for column
        public static short GetNumberOfMonthsLength()
        {
            return 2;
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfDaysDBName()
        {
            return "a_number_of_days_i";
        }
        
        /// get help text for column
        public static string GetNumberOfDaysHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetNumberOfDaysLabel()
        {
            return "Number of Days";
        }
        
        /// get display format for column
        public static short GetNumberOfDaysLength()
        {
            return 3;
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfHoursDBName()
        {
            return "a_number_of_hours_i";
        }
        
        /// get help text for column
        public static string GetNumberOfHoursHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetNumberOfHoursLabel()
        {
            return "Number of Hours";
        }
        
        /// get display format for column
        public static short GetNumberOfHoursLength()
        {
            return 2;
        }
        
        /// get the name of the field in the database for this column
        public static string GetNumberOfMinutesDBName()
        {
            return "a_number_of_minutes_i";
        }
        
        /// get help text for column
        public static string GetNumberOfMinutesHelp()
        {
            return "";
        }
        
        /// get label of column
        public static string GetNumberOfMinutesLabel()
        {
            return "Number of Minutes";
        }
        
        /// get display format for column
        public static short GetNumberOfMinutesLength()
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
            return "AFrequency";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_frequency";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Frequency";
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
                    "a_frequency_code_c",
                    "a_frequency_description_c",
                    "a_number_of_years_i",
                    "a_number_of_months_i",
                    "a_number_of_days_i",
                    "a_number_of_hours_i",
                    "a_number_of_minutes_i",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnFrequencyCode = this.Columns["a_frequency_code_c"];
            this.ColumnFrequencyDescription = this.Columns["a_frequency_description_c"];
            this.ColumnNumberOfYears = this.Columns["a_number_of_years_i"];
            this.ColumnNumberOfMonths = this.Columns["a_number_of_months_i"];
            this.ColumnNumberOfDays = this.Columns["a_number_of_days_i"];
            this.ColumnNumberOfHours = this.Columns["a_number_of_hours_i"];
            this.ColumnNumberOfMinutes = this.Columns["a_number_of_minutes_i"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnFrequencyCode};
        }
        
        /// get typed set of changes
        public AFrequencyTable GetChangesTyped()
        {
            return ((AFrequencyTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public AFrequencyRow NewRowTyped(bool AWithDefaultValues)
        {
            AFrequencyRow ret = ((AFrequencyRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public AFrequencyRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new AFrequencyRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_frequency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_frequency_description_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_number_of_years_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_number_of_months_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_number_of_days_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_number_of_hours_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("a_number_of_minutes_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnFrequencyCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 24);
            }
            if ((ACol == ColumnFrequencyDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
            }
            if ((ACol == ColumnNumberOfYears))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNumberOfMonths))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNumberOfDays))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNumberOfHours))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnNumberOfMinutes))
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
    
    /// Units of time. Used in partner letters.  Also used to indicate how often a publication is produced or a receipt is sent to a donor.
    [Serializable()]
    public class AFrequencyRow : System.Data.DataRow
    {
        
        private AFrequencyTable myTable;
        
        /// Constructor
        public AFrequencyRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((AFrequencyTable)(this.Table));
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
        
        /// 
        public String FrequencyDescription
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnFrequencyDescription.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnFrequencyDescription) 
                            || (((String)(this[this.myTable.ColumnFrequencyDescription])) != value)))
                {
                    this[this.myTable.ColumnFrequencyDescription] = value;
                }
            }
        }
        
        /// 
        public Int32 NumberOfYears
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfYears.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfYears) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfYears])) != value)))
                {
                    this[this.myTable.ColumnNumberOfYears] = value;
                }
            }
        }
        
        /// 
        public Int32 NumberOfMonths
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfMonths.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfMonths) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfMonths])) != value)))
                {
                    this[this.myTable.ColumnNumberOfMonths] = value;
                }
            }
        }
        
        /// 
        public Int32 NumberOfDays
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfDays.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfDays) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfDays])) != value)))
                {
                    this[this.myTable.ColumnNumberOfDays] = value;
                }
            }
        }
        
        /// 
        public Int32 NumberOfHours
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfHours.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfHours) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfHours])) != value)))
                {
                    this[this.myTable.ColumnNumberOfHours] = value;
                }
            }
        }
        
        /// 
        public Int32 NumberOfMinutes
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNumberOfMinutes.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnNumberOfMinutes) 
                            || (((Int32)(this[this.myTable.ColumnNumberOfMinutes])) != value)))
                {
                    this[this.myTable.ColumnNumberOfMinutes] = value;
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
            this.SetNull(this.myTable.ColumnFrequencyCode);
            this.SetNull(this.myTable.ColumnFrequencyDescription);
            this[this.myTable.ColumnNumberOfYears.Ordinal] = 0;
            this[this.myTable.ColumnNumberOfMonths.Ordinal] = 0;
            this[this.myTable.ColumnNumberOfDays.Ordinal] = 0;
            this[this.myTable.ColumnNumberOfHours.Ordinal] = 0;
            this[this.myTable.ColumnNumberOfMinutes.Ordinal] = 0;
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsNumberOfMonthsNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfMonths);
        }
        
        /// assign NULL value
        public void SetNumberOfMonthsNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfMonths);
        }
        
        /// test for NULL value
        public bool IsNumberOfDaysNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfDays);
        }
        
        /// assign NULL value
        public void SetNumberOfDaysNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfDays);
        }
        
        /// test for NULL value
        public bool IsNumberOfHoursNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfHours);
        }
        
        /// assign NULL value
        public void SetNumberOfHoursNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfHours);
        }
        
        /// test for NULL value
        public bool IsNumberOfMinutesNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfMinutes);
        }
        
        /// assign NULL value
        public void SetNumberOfMinutesNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfMinutes);
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
    
    /// Post office mailing zone classification
    [Serializable()]
    public class PInternationalPostalTypeTable : TTypedDataTable
    {
        
        /// 
        public DataColumn ColumnInternatPostalTypeCode;
        
        /// 
        public DataColumn ColumnDescription;
        
        /// This defines if the international postal type code can be deleted.
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
        public PInternationalPostalTypeTable() : 
                base("PInternationalPostalType")
        {
        }
        
        /// constructor
        public PInternationalPostalTypeTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PInternationalPostalTypeTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PInternationalPostalTypeRow this[int i]
        {
            get
            {
                return ((PInternationalPostalTypeRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetInternatPostalTypeCodeDBName()
        {
            return "p_internat_postal_type_code_c";
        }
        
        /// get help text for column
        public static string GetInternatPostalTypeCodeHelp()
        {
            return "Enter an international postal type code";
        }
        
        /// get label of column
        public static string GetInternatPostalTypeCodeLabel()
        {
            return "International Postal Type Code";
        }
        
        /// get character length for column
        public static short GetInternatPostalTypeCodeLength()
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
            return "Enter a description";
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
            return "PInternationalPostalType";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_international_postal_type";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "International Postal Type";
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
                    "p_internat_postal_type_code_c",
                    "p_description_c",
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
            this.ColumnInternatPostalTypeCode = this.Columns["p_internat_postal_type_code_c"];
            this.ColumnDescription = this.Columns["p_description_c"];
            this.ColumnDeletable = this.Columns["p_deletable_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnInternatPostalTypeCode};
        }
        
        /// get typed set of changes
        public PInternationalPostalTypeTable GetChangesTyped()
        {
            return ((PInternationalPostalTypeTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PInternationalPostalTypeRow NewRowTyped(bool AWithDefaultValues)
        {
            PInternationalPostalTypeRow ret = ((PInternationalPostalTypeRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PInternationalPostalTypeRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PInternationalPostalTypeRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_internat_postal_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_description_c", typeof(String)));
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
            if ((ACol == ColumnInternatPostalTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnDescription))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
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
    
    /// Post office mailing zone classification
    [Serializable()]
    public class PInternationalPostalTypeRow : System.Data.DataRow
    {
        
        private PInternationalPostalTypeTable myTable;
        
        /// Constructor
        public PInternationalPostalTypeRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PInternationalPostalTypeTable)(this.Table));
        }
        
        /// 
        public String InternatPostalTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnInternatPostalTypeCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnInternatPostalTypeCode) 
                            || (((String)(this[this.myTable.ColumnInternatPostalTypeCode])) != value)))
                {
                    this[this.myTable.ColumnInternatPostalTypeCode] = value;
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
        
        /// This defines if the international postal type code can be deleted.
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
            this.SetNull(this.myTable.ColumnInternatPostalTypeCode);
            this.SetNull(this.myTable.ColumnDescription);
            this[this.myTable.ColumnDeletable.Ordinal] = true;
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
    
    /// List of countries with their codes
    [Serializable()]
    public class PCountryTable : TTypedDataTable
    {
        
        /// This is a code which identifies a country.
        ///It is the ISO code (ISO 3166)
        public DataColumn ColumnCountryCode;
        
        /// The name of the country
        public DataColumn ColumnCountryName;
        
        /// The nationality of people in this country
        public DataColumn ColumnNationalityName;
        
        /// Describes if the country is politically sensitive.
        public DataColumn ColumnUndercover;
        
        /// The telephone code needed to dial into a country
        public DataColumn ColumnInternatTelephoneCode;
        
        /// 
        public DataColumn ColumnInternatPostalTypeCode;
        
        /// The code needed to dial out of a country.
        public DataColumn ColumnInternatAccessCode;
        
        /// Number of hours +/- GMT
        public DataColumn ColumnTimeZoneMinimum;
        
        /// Number of hours +/- GMT
        public DataColumn ColumnTimeZoneMaximum;
        
        /// This defines if the country code can be deleted.
        ///This can only be updated by the system manager.
        ///At the risk of serious operational integrity.
        ///Default to Yes
        public DataColumn ColumnDeletable;
        
        /// Tab order of the city, county, and post code fields on the Partner Edit screen
        public DataColumn ColumnAddressOrder;
        
        /// The name of the country in the Local language
        public DataColumn ColumnCountryNameLocal;
        
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
        public PCountryTable() : 
                base("PCountry")
        {
        }
        
        /// constructor
        public PCountryTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public PCountryTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public PCountryRow this[int i]
        {
            get
            {
                return ((PCountryRow)(this.Rows[i]));
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
            return "Enter an internationally accepted (ISO) country code";
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
        public static string GetCountryNameDBName()
        {
            return "p_country_name_c";
        }
        
        /// get help text for column
        public static string GetCountryNameHelp()
        {
            return "Enter the full name of the country";
        }
        
        /// get label of column
        public static string GetCountryNameLabel()
        {
            return "Country Name";
        }
        
        /// get character length for column
        public static short GetCountryNameLength()
        {
            return 40;
        }
        
        /// get the name of the field in the database for this column
        public static string GetNationalityNameDBName()
        {
            return "p_nationality_name_c";
        }
        
        /// get help text for column
        public static string GetNationalityNameHelp()
        {
            return "Enter the nationality of people from this country";
        }
        
        /// get label of column
        public static string GetNationalityNameLabel()
        {
            return "Nationality Name";
        }
        
        /// get character length for column
        public static short GetNationalityNameLength()
        {
            return 40;
        }
        
        /// get the name of the field in the database for this column
        public static string GetUndercoverDBName()
        {
            return "p_undercover_l";
        }
        
        /// get help text for column
        public static string GetUndercoverHelp()
        {
            return "Select if the country is politically sensitive";
        }
        
        /// get label of column
        public static string GetUndercoverLabel()
        {
            return "Undercover";
        }
        
        /// get display format for column
        public static short GetUndercoverLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetInternatTelephoneCodeDBName()
        {
            return "p_internat_telephone_code_i";
        }
        
        /// get help text for column
        public static string GetInternatTelephoneCodeHelp()
        {
            return "Enter the International Code needed to dial INTO the country";
        }
        
        /// get label of column
        public static string GetInternatTelephoneCodeLabel()
        {
            return "International Dialing Code";
        }
        
        /// get display format for column
        public static short GetInternatTelephoneCodeLength()
        {
            return 5;
        }
        
        /// get the name of the field in the database for this column
        public static string GetInternatPostalTypeCodeDBName()
        {
            return "p_internat_postal_type_code_c";
        }
        
        /// get help text for column
        public static string GetInternatPostalTypeCodeHelp()
        {
            return "Enter an international postal type code";
        }
        
        /// get label of column
        public static string GetInternatPostalTypeCodeLabel()
        {
            return "International Postal Type Code";
        }
        
        /// get character length for column
        public static short GetInternatPostalTypeCodeLength()
        {
            return 8;
        }
        
        /// get the name of the field in the database for this column
        public static string GetInternatAccessCodeDBName()
        {
            return "p_internat_access_code_c";
        }
        
        /// get help text for column
        public static string GetInternatAccessCodeHelp()
        {
            return "International telephone access code needed to dial OUT";
        }
        
        /// get label of column
        public static string GetInternatAccessCodeLabel()
        {
            return "International Access Dialing Code";
        }
        
        /// get character length for column
        public static short GetInternatAccessCodeLength()
        {
            return 4;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTimeZoneMinimumDBName()
        {
            return "p_time_zone_minimum_n";
        }
        
        /// get help text for column
        public static string GetTimeZoneMinimumHelp()
        {
            return "Enter the minimum time zone +/- relative to GMT";
        }
        
        /// get label of column
        public static string GetTimeZoneMinimumLabel()
        {
            return "Time Zone Range Minimum";
        }
        
        /// get display format for column
        public static short GetTimeZoneMinimumLength()
        {
            return 6;
        }
        
        /// get the name of the field in the database for this column
        public static string GetTimeZoneMaximumDBName()
        {
            return "p_time_zone_maximum_n";
        }
        
        /// get help text for column
        public static string GetTimeZoneMaximumHelp()
        {
            return "Enter the maximum time zone +/- relative to GMT";
        }
        
        /// get label of column
        public static string GetTimeZoneMaximumLabel()
        {
            return "Time Zone Range Maximum";
        }
        
        /// get display format for column
        public static short GetTimeZoneMaximumLength()
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
        public static string GetAddressOrderDBName()
        {
            return "p_address_order_i";
        }
        
        /// get help text for column
        public static string GetAddressOrderHelp()
        {
            return "0 = International; 1 = European; 2 = American";
        }
        
        /// get label of column
        public static string GetAddressOrderLabel()
        {
            return "Address Display Order";
        }
        
        /// get display format for column
        public static short GetAddressOrderLength()
        {
            return 1;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCountryNameLocalDBName()
        {
            return "p_country_name_local_c";
        }
        
        /// get help text for column
        public static string GetCountryNameLocalHelp()
        {
            return "Enter the full name of the country in your local language";
        }
        
        /// get label of column
        public static string GetCountryNameLocalLabel()
        {
            return "Country Name In Local Language";
        }
        
        /// get character length for column
        public static short GetCountryNameLocalLength()
        {
            return 40;
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
            return "PCountry";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "p_country";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Country";
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
                    "p_country_code_c",
                    "p_country_name_c",
                    "p_nationality_name_c",
                    "p_undercover_l",
                    "p_internat_telephone_code_i",
                    "p_internat_postal_type_code_c",
                    "p_internat_access_code_c",
                    "p_time_zone_minimum_n",
                    "p_time_zone_maximum_n",
                    "p_deletable_l",
                    "p_address_order_i",
                    "p_country_name_local_c",
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
            this.ColumnCountryName = this.Columns["p_country_name_c"];
            this.ColumnNationalityName = this.Columns["p_nationality_name_c"];
            this.ColumnUndercover = this.Columns["p_undercover_l"];
            this.ColumnInternatTelephoneCode = this.Columns["p_internat_telephone_code_i"];
            this.ColumnInternatPostalTypeCode = this.Columns["p_internat_postal_type_code_c"];
            this.ColumnInternatAccessCode = this.Columns["p_internat_access_code_c"];
            this.ColumnTimeZoneMinimum = this.Columns["p_time_zone_minimum_n"];
            this.ColumnTimeZoneMaximum = this.Columns["p_time_zone_maximum_n"];
            this.ColumnDeletable = this.Columns["p_deletable_l"];
            this.ColumnAddressOrder = this.Columns["p_address_order_i"];
            this.ColumnCountryNameLocal = this.Columns["p_country_name_local_c"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnCountryCode};
        }
        
        /// get typed set of changes
        public PCountryTable GetChangesTyped()
        {
            return ((PCountryTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public PCountryRow NewRowTyped(bool AWithDefaultValues)
        {
            PCountryRow ret = ((PCountryRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public PCountryRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new PCountryRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("p_country_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_country_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_nationality_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_undercover_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_internat_telephone_code_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_internat_postal_type_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_internat_access_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_time_zone_minimum_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("p_time_zone_maximum_n", typeof(Decimal)));
            this.Columns.Add(new System.Data.DataColumn("p_deletable_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("p_address_order_i", typeof(Int32)));
            this.Columns.Add(new System.Data.DataColumn("p_country_name_local_c", typeof(String)));
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
            if ((ACol == ColumnCountryName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 80);
            }
            if ((ACol == ColumnNationalityName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 80);
            }
            if ((ACol == ColumnUndercover))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnInternatTelephoneCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnInternatPostalTypeCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnInternatAccessCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 8);
            }
            if ((ACol == ColumnTimeZoneMinimum))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 6);
            }
            if ((ACol == ColumnTimeZoneMaximum))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Decimal, 6);
            }
            if ((ACol == ColumnDeletable))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Bit);
            }
            if ((ACol == ColumnAddressOrder))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.Int);
            }
            if ((ACol == ColumnCountryNameLocal))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 80);
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
    
    /// List of countries with their codes
    [Serializable()]
    public class PCountryRow : System.Data.DataRow
    {
        
        private PCountryTable myTable;
        
        /// Constructor
        public PCountryRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((PCountryTable)(this.Table));
        }
        
        /// This is a code which identifies a country.
        ///It is the ISO code (ISO 3166)
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
        
        /// The name of the country
        public String CountryName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCountryName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCountryName) 
                            || (((String)(this[this.myTable.ColumnCountryName])) != value)))
                {
                    this[this.myTable.ColumnCountryName] = value;
                }
            }
        }
        
        /// The nationality of people in this country
        public String NationalityName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnNationalityName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnNationalityName) 
                            || (((String)(this[this.myTable.ColumnNationalityName])) != value)))
                {
                    this[this.myTable.ColumnNationalityName] = value;
                }
            }
        }
        
        /// Describes if the country is politically sensitive.
        public Boolean Undercover
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnUndercover.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnUndercover) 
                            || (((Boolean)(this[this.myTable.ColumnUndercover])) != value)))
                {
                    this[this.myTable.ColumnUndercover] = value;
                }
            }
        }
        
        /// The telephone code needed to dial into a country
        public Int32 InternatTelephoneCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnInternatTelephoneCode.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnInternatTelephoneCode) 
                            || (((Int32)(this[this.myTable.ColumnInternatTelephoneCode])) != value)))
                {
                    this[this.myTable.ColumnInternatTelephoneCode] = value;
                }
            }
        }
        
        /// 
        public String InternatPostalTypeCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnInternatPostalTypeCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnInternatPostalTypeCode) 
                            || (((String)(this[this.myTable.ColumnInternatPostalTypeCode])) != value)))
                {
                    this[this.myTable.ColumnInternatPostalTypeCode] = value;
                }
            }
        }
        
        /// The code needed to dial out of a country.
        public String InternatAccessCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnInternatAccessCode.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnInternatAccessCode) 
                            || (((String)(this[this.myTable.ColumnInternatAccessCode])) != value)))
                {
                    this[this.myTable.ColumnInternatAccessCode] = value;
                }
            }
        }
        
        /// Number of hours +/- GMT
        public Decimal TimeZoneMinimum
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTimeZoneMinimum.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTimeZoneMinimum) 
                            || (((Decimal)(this[this.myTable.ColumnTimeZoneMinimum])) != value)))
                {
                    this[this.myTable.ColumnTimeZoneMinimum] = value;
                }
            }
        }
        
        /// Number of hours +/- GMT
        public Decimal TimeZoneMaximum
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnTimeZoneMaximum.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnTimeZoneMaximum) 
                            || (((Decimal)(this[this.myTable.ColumnTimeZoneMaximum])) != value)))
                {
                    this[this.myTable.ColumnTimeZoneMaximum] = value;
                }
            }
        }
        
        /// This defines if the country code can be deleted.
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
        
        /// Tab order of the city, county, and post code fields on the Partner Edit screen
        public Int32 AddressOrder
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnAddressOrder.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnAddressOrder) 
                            || (((Int32)(this[this.myTable.ColumnAddressOrder])) != value)))
                {
                    this[this.myTable.ColumnAddressOrder] = value;
                }
            }
        }
        
        /// The name of the country in the Local language
        public String CountryNameLocal
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCountryNameLocal.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCountryNameLocal) 
                            || (((String)(this[this.myTable.ColumnCountryNameLocal])) != value)))
                {
                    this[this.myTable.ColumnCountryNameLocal] = value;
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
            this.SetNull(this.myTable.ColumnCountryName);
            this.SetNull(this.myTable.ColumnNationalityName);
            this[this.myTable.ColumnUndercover.Ordinal] = false;
            this[this.myTable.ColumnInternatTelephoneCode.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnInternatPostalTypeCode);
            this.SetNull(this.myTable.ColumnInternatAccessCode);
            this[this.myTable.ColumnTimeZoneMinimum.Ordinal] = 0;
            this[this.myTable.ColumnTimeZoneMaximum.Ordinal] = 0;
            this[this.myTable.ColumnDeletable.Ordinal] = true;
            this[this.myTable.ColumnAddressOrder.Ordinal] = 0;
            this.SetNull(this.myTable.ColumnCountryNameLocal);
            this[this.myTable.ColumnDateCreated.Ordinal] = DateTime.Today;
            this.SetNull(this.myTable.ColumnCreatedBy);
            this.SetNull(this.myTable.ColumnDateModified);
            this.SetNull(this.myTable.ColumnModifiedBy);
            this.SetNull(this.myTable.ColumnModificationId);
        }
        
        /// test for NULL value
        public bool IsUndercoverNull()
        {
            return this.IsNull(this.myTable.ColumnUndercover);
        }
        
        /// assign NULL value
        public void SetUndercoverNull()
        {
            this.SetNull(this.myTable.ColumnUndercover);
        }
        
        /// test for NULL value
        public bool IsInternatTelephoneCodeNull()
        {
            return this.IsNull(this.myTable.ColumnInternatTelephoneCode);
        }
        
        /// assign NULL value
        public void SetInternatTelephoneCodeNull()
        {
            this.SetNull(this.myTable.ColumnInternatTelephoneCode);
        }
        
        /// test for NULL value
        public bool IsInternatPostalTypeCodeNull()
        {
            return this.IsNull(this.myTable.ColumnInternatPostalTypeCode);
        }
        
        /// assign NULL value
        public void SetInternatPostalTypeCodeNull()
        {
            this.SetNull(this.myTable.ColumnInternatPostalTypeCode);
        }
        
        /// test for NULL value
        public bool IsInternatAccessCodeNull()
        {
            return this.IsNull(this.myTable.ColumnInternatAccessCode);
        }
        
        /// assign NULL value
        public void SetInternatAccessCodeNull()
        {
            this.SetNull(this.myTable.ColumnInternatAccessCode);
        }
        
        /// test for NULL value
        public bool IsTimeZoneMinimumNull()
        {
            return this.IsNull(this.myTable.ColumnTimeZoneMinimum);
        }
        
        /// assign NULL value
        public void SetTimeZoneMinimumNull()
        {
            this.SetNull(this.myTable.ColumnTimeZoneMinimum);
        }
        
        /// test for NULL value
        public bool IsTimeZoneMaximumNull()
        {
            return this.IsNull(this.myTable.ColumnTimeZoneMaximum);
        }
        
        /// assign NULL value
        public void SetTimeZoneMaximumNull()
        {
            this.SetNull(this.myTable.ColumnTimeZoneMaximum);
        }
        
        /// test for NULL value
        public bool IsAddressOrderNull()
        {
            return this.IsNull(this.myTable.ColumnAddressOrder);
        }
        
        /// assign NULL value
        public void SetAddressOrderNull()
        {
            this.SetNull(this.myTable.ColumnAddressOrder);
        }
        
        /// test for NULL value
        public bool IsCountryNameLocalNull()
        {
            return this.IsNull(this.myTable.ColumnCountryNameLocal);
        }
        
        /// assign NULL value
        public void SetCountryNameLocalNull()
        {
            this.SetNull(this.myTable.ColumnCountryNameLocal);
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
    
    /// Unit of money for various countries.
    [Serializable()]
    public class ACurrencyTable : TTypedDataTable
    {
        
        /// This defines which currency is being used
        public DataColumn ColumnCurrencyCode;
        
        /// This is the name of the currency
        public DataColumn ColumnCurrencyName;
        
        /// This is the symbol used to show a currency. Eg $US or &#163;
        public DataColumn ColumnCurrencySymbol;
        
        /// Country code
        public DataColumn ColumnCountryCode;
        
        /// The format in which to display and accept input on a currency (decimal values)
        public DataColumn ColumnDisplayFormat;
        
        /// Indicates whether currency is part of the european exchange rate mechanism/ European Monetary Union
        public DataColumn ColumnInEmu;
        
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
        public ACurrencyTable() : 
                base("ACurrency")
        {
        }
        
        /// constructor
        public ACurrencyTable(string ATablename) : 
                base(ATablename)
        {
        }
        
        /// constructor for serialization
        public ACurrencyTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context)
        {
        }
        
        /// Access a typed row by index
        public ACurrencyRow this[int i]
        {
            get
            {
                return ((ACurrencyRow)(this.Rows[i]));
            }
        }
        
        /// get the name of the field in the database for this column
        public static string GetCurrencyCodeDBName()
        {
            return "a_currency_code_c";
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
        public static string GetCurrencyNameDBName()
        {
            return "a_currency_name_c";
        }
        
        /// get help text for column
        public static string GetCurrencyNameHelp()
        {
            return "Enter the currency name";
        }
        
        /// get label of column
        public static string GetCurrencyNameLabel()
        {
            return "Currency Name";
        }
        
        /// get character length for column
        public static short GetCurrencyNameLength()
        {
            return 32;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCurrencySymbolDBName()
        {
            return "a_currency_symbol_c";
        }
        
        /// get help text for column
        public static string GetCurrencySymbolHelp()
        {
            return "Enter the symbol which represents this currency";
        }
        
        /// get label of column
        public static string GetCurrencySymbolLabel()
        {
            return "Currency Symbol";
        }
        
        /// get character length for column
        public static short GetCurrencySymbolLength()
        {
            return 4;
        }
        
        /// get the name of the field in the database for this column
        public static string GetCountryCodeDBName()
        {
            return "p_country_code_c";
        }
        
        /// get help text for column
        public static string GetCountryCodeHelp()
        {
            return "Enter a valid country code.";
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
        public static string GetDisplayFormatDBName()
        {
            return "a_display_format_c";
        }
        
        /// get help text for column
        public static string GetDisplayFormatHelp()
        {
            return "The format in which to display and accept input on a currency.";
        }
        
        /// get label of column
        public static string GetDisplayFormatLabel()
        {
            return "Display Format";
        }
        
        /// get character length for column
        public static short GetDisplayFormatLength()
        {
            return 20;
        }
        
        /// get the name of the field in the database for this column
        public static string GetInEmuDBName()
        {
            return "a_in_emu_l";
        }
        
        /// get help text for column
        public static string GetInEmuHelp()
        {
            return "Is this currency one of EMU currencies?";
        }
        
        /// get label of column
        public static string GetInEmuLabel()
        {
            return "In EMU";
        }
        
        /// get display format for column
        public static short GetInEmuLength()
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
            return "ACurrency";
        }
        
        /// original name of table in the database
        public static string GetTableDBName()
        {
            return "a_currency";
        }
        
        /// get table label for messages etc
        public static string GetTableLabel()
        {
            return "Currency";
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
                    "a_currency_code_c",
                    "a_currency_name_c",
                    "a_currency_symbol_c",
                    "p_country_code_c",
                    "a_display_format_c",
                    "a_in_emu_l",
                    "s_date_created_d",
                    "s_created_by_c",
                    "s_date_modified_d",
                    "s_modified_by_c",
                    "s_modification_id_c"};
        }
        
        /// assign columns to properties, set primary key
        public override void InitVars()
        {
            this.ColumnCurrencyCode = this.Columns["a_currency_code_c"];
            this.ColumnCurrencyName = this.Columns["a_currency_name_c"];
            this.ColumnCurrencySymbol = this.Columns["a_currency_symbol_c"];
            this.ColumnCountryCode = this.Columns["p_country_code_c"];
            this.ColumnDisplayFormat = this.Columns["a_display_format_c"];
            this.ColumnInEmu = this.Columns["a_in_emu_l"];
            this.ColumnDateCreated = this.Columns["s_date_created_d"];
            this.ColumnCreatedBy = this.Columns["s_created_by_c"];
            this.ColumnDateModified = this.Columns["s_date_modified_d"];
            this.ColumnModifiedBy = this.Columns["s_modified_by_c"];
            this.ColumnModificationId = this.Columns["s_modification_id_c"];
            this.PrimaryKey = new System.Data.DataColumn[] {
                    this.ColumnCurrencyCode};
        }
        
        /// get typed set of changes
        public ACurrencyTable GetChangesTyped()
        {
            return ((ACurrencyTable)(base.GetChangesTypedInternal()));
        }
        
        /// create a new typed row
        public ACurrencyRow NewRowTyped(bool AWithDefaultValues)
        {
            ACurrencyRow ret = ((ACurrencyRow)(this.NewRow()));
            if ((AWithDefaultValues == true))
            {
                ret.InitValues();
            }
            return ret;
        }
        
        /// create a new typed row, always with default values
        public ACurrencyRow NewRowTyped()
        {
            return this.NewRowTyped(true);
        }
        
        /// new typed row using DataRowBuilder
        protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder)
        {
            return new ACurrencyRow(builder);
        }
        
        /// create the columns
        protected override void InitClass()
        {
            this.Columns.Add(new System.Data.DataColumn("a_currency_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_currency_name_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_currency_symbol_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("p_country_code_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_display_format_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("a_in_emu_l", typeof(Boolean)));
            this.Columns.Add(new System.Data.DataColumn("s_date_created_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_created_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_date_modified_d", typeof(System.DateTime)));
            this.Columns.Add(new System.Data.DataColumn("s_modified_by_c", typeof(String)));
            this.Columns.Add(new System.Data.DataColumn("s_modification_id_c", typeof(String)));
        }
        
        /// prepare odbc parameters for given column
        public override OdbcParameter CreateOdbcParameter(DataColumn ACol)
        {
            if ((ACol == ColumnCurrencyCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 16);
            }
            if ((ACol == ColumnCurrencyName))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 64);
            }
            if ((ACol == ColumnCurrencySymbol))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 8);
            }
            if ((ACol == ColumnCountryCode))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 8);
            }
            if ((ACol == ColumnDisplayFormat))
            {
                return new System.Data.Odbc.OdbcParameter("", OdbcType.VarChar, 40);
            }
            if ((ACol == ColumnInEmu))
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
    
    /// Unit of money for various countries.
    [Serializable()]
    public class ACurrencyRow : System.Data.DataRow
    {
        
        private ACurrencyTable myTable;
        
        /// Constructor
        public ACurrencyRow(System.Data.DataRowBuilder rb) : 
                base(rb)
        {
            this.myTable = ((ACurrencyTable)(this.Table));
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
        
        /// This is the name of the currency
        public String CurrencyName
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCurrencyName.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCurrencyName) 
                            || (((String)(this[this.myTable.ColumnCurrencyName])) != value)))
                {
                    this[this.myTable.ColumnCurrencyName] = value;
                }
            }
        }
        
        /// This is the symbol used to show a currency. Eg $US or &#163;
        public String CurrencySymbol
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCurrencySymbol.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnCurrencySymbol) 
                            || (((String)(this[this.myTable.ColumnCurrencySymbol])) != value)))
                {
                    this[this.myTable.ColumnCurrencySymbol] = value;
                }
            }
        }
        
        /// Country code
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
        
        /// The format in which to display and accept input on a currency (decimal values)
        public String DisplayFormat
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnDisplayFormat.Ordinal];
                if ((ret == System.DBNull.Value))
                {
                    return String.Empty;
                }
                else
                {
                    return ((String)(ret));
                }
            }
            set
            {
                if ((this.IsNull(this.myTable.ColumnDisplayFormat) 
                            || (((String)(this[this.myTable.ColumnDisplayFormat])) != value)))
                {
                    this[this.myTable.ColumnDisplayFormat] = value;
                }
            }
        }
        
        /// Indicates whether currency is part of the european exchange rate mechanism/ European Monetary Union
        public Boolean InEmu
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnInEmu.Ordinal];
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
                if ((this.IsNull(this.myTable.ColumnInEmu) 
                            || (((Boolean)(this[this.myTable.ColumnInEmu])) != value)))
                {
                    this[this.myTable.ColumnInEmu] = value;
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
            this.SetNull(this.myTable.ColumnCurrencyCode);
            this.SetNull(this.myTable.ColumnCurrencyName);
            this.SetNull(this.myTable.ColumnCurrencySymbol);
            this.SetNull(this.myTable.ColumnCountryCode);
            this[this.myTable.ColumnDisplayFormat.Ordinal] = "->>>,>>>,>>>,>>9.99";
            this[this.myTable.ColumnInEmu.Ordinal] = false;
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
