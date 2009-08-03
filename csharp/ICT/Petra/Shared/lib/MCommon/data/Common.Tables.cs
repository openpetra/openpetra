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
        /// This can only be updated by the system manager.
        /// At the risk of serious operational integrity.
        /// Default to Yes
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 0;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PLanguage", "p_language",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "LanguageCode", "p_language_code_c", OdbcType.VarChar, 20, true),
                    new TTypedColumnInfo(1, "LanguageDescription", "p_language_description_c", OdbcType.VarChar, 80, true),
                    new TTypedColumnInfo(2, "CongressLanguage", "p_congress_language_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(3, "Deletable", "p_deletable_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(4, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(5, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(6, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PLanguageRow this[int i]
        {
            get
            {
                return ((PLanguageRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PLanguageTable GetChangesTyped()
        {
            return ((PLanguageTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetLanguageCodeDBName()
        {
            return "p_language_code_c";
        }

        /// get character length for column
        public static short GetLanguageCodeLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetLanguageDescriptionDBName()
        {
            return "p_language_description_c";
        }

        /// get character length for column
        public static short GetLanguageDescriptionLength()
        {
            return 80;
        }

        /// get the name of the field in the database for this column
        public static string GetCongressLanguageDBName()
        {
            return "p_congress_language_l";
        }

        /// get character length for column
        public static short GetCongressLanguageLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDeletableDBName()
        {
            return "p_deletable_l";
        }

        /// get character length for column
        public static short GetDeletableLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }

        /// get character length for column
        public static short GetDateCreatedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }

        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }

        /// get character length for column
        public static short GetDateModifiedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }

        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }

        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        /// This can only be updated by the system manager.
        /// At the risk of serious operational integrity.
        /// Default to Yes
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

        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
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

        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsLanguageCodeNull()
        {
            return this.IsNull(this.myTable.ColumnLanguageCode);
        }

        /// assign NULL value
        public void SetLanguageCodeNull()
        {
            this.SetNull(this.myTable.ColumnLanguageCode);
        }

        /// test for NULL value
        public bool IsLanguageDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnLanguageDescription);
        }

        /// assign NULL value
        public void SetLanguageDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnLanguageDescription);
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 1;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "AFrequency", "a_frequency",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "FrequencyCode", "a_frequency_code_c", OdbcType.VarChar, 24, true),
                    new TTypedColumnInfo(1, "FrequencyDescription", "a_frequency_description_c", OdbcType.VarChar, 64, true),
                    new TTypedColumnInfo(2, "NumberOfYears", "a_number_of_years_i", OdbcType.Int, -1, true),
                    new TTypedColumnInfo(3, "NumberOfMonths", "a_number_of_months_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(4, "NumberOfDays", "a_number_of_days_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(5, "NumberOfHours", "a_number_of_hours_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(6, "NumberOfMinutes", "a_number_of_minutes_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(7, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(8, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(9, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(10, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(11, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public AFrequencyRow this[int i]
        {
            get
            {
                return ((AFrequencyRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public AFrequencyTable GetChangesTyped()
        {
            return ((AFrequencyTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetFrequencyCodeDBName()
        {
            return "a_frequency_code_c";
        }

        /// get character length for column
        public static short GetFrequencyCodeLength()
        {
            return 24;
        }

        /// get the name of the field in the database for this column
        public static string GetFrequencyDescriptionDBName()
        {
            return "a_frequency_description_c";
        }

        /// get character length for column
        public static short GetFrequencyDescriptionLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfYearsDBName()
        {
            return "a_number_of_years_i";
        }

        /// get character length for column
        public static short GetNumberOfYearsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfMonthsDBName()
        {
            return "a_number_of_months_i";
        }

        /// get character length for column
        public static short GetNumberOfMonthsLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfDaysDBName()
        {
            return "a_number_of_days_i";
        }

        /// get character length for column
        public static short GetNumberOfDaysLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfHoursDBName()
        {
            return "a_number_of_hours_i";
        }

        /// get character length for column
        public static short GetNumberOfHoursLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetNumberOfMinutesDBName()
        {
            return "a_number_of_minutes_i";
        }

        /// get character length for column
        public static short GetNumberOfMinutesLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }

        /// get character length for column
        public static short GetDateCreatedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }

        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }

        /// get character length for column
        public static short GetDateModifiedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }

        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }

        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
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

        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsFrequencyCodeNull()
        {
            return this.IsNull(this.myTable.ColumnFrequencyCode);
        }

        /// assign NULL value
        public void SetFrequencyCodeNull()
        {
            this.SetNull(this.myTable.ColumnFrequencyCode);
        }

        /// test for NULL value
        public bool IsFrequencyDescriptionNull()
        {
            return this.IsNull(this.myTable.ColumnFrequencyDescription);
        }

        /// assign NULL value
        public void SetFrequencyDescriptionNull()
        {
            this.SetNull(this.myTable.ColumnFrequencyDescription);
        }

        /// test for NULL value
        public bool IsNumberOfYearsNull()
        {
            return this.IsNull(this.myTable.ColumnNumberOfYears);
        }

        /// assign NULL value
        public void SetNumberOfYearsNull()
        {
            this.SetNull(this.myTable.ColumnNumberOfYears);
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
        /// This can only be updated by the system manager.
        /// At the risk of serious operational integrity.
        /// Default to Yes
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 2;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PInternationalPostalType", "p_international_postal_type",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "InternatPostalTypeCode", "p_internat_postal_type_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(1, "Description", "p_description_c", OdbcType.VarChar, 64, true),
                    new TTypedColumnInfo(2, "Deletable", "p_deletable_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(3, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(4, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(5, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(6, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(7, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PInternationalPostalTypeRow this[int i]
        {
            get
            {
                return ((PInternationalPostalTypeRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PInternationalPostalTypeTable GetChangesTyped()
        {
            return ((PInternationalPostalTypeTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetInternatPostalTypeCodeDBName()
        {
            return "p_internat_postal_type_code_c";
        }

        /// get character length for column
        public static short GetInternatPostalTypeCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetDescriptionDBName()
        {
            return "p_description_c";
        }

        /// get character length for column
        public static short GetDescriptionLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetDeletableDBName()
        {
            return "p_deletable_l";
        }

        /// get character length for column
        public static short GetDeletableLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }

        /// get character length for column
        public static short GetDateCreatedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }

        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }

        /// get character length for column
        public static short GetDateModifiedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }

        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }

        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        /// This can only be updated by the system manager.
        /// At the risk of serious operational integrity.
        /// Default to Yes
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

        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
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

        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

    /// List of countries with their codes
    [Serializable()]
    public class PCountryTable : TTypedDataTable
    {
        /// This is a code which identifies a country.
        /// It is the ISO code (ISO 3166)
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
        /// This can only be updated by the system manager.
        /// At the risk of serious operational integrity.
        /// Default to Yes
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 3;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "PCountry", "p_country",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "CountryCode", "p_country_code_c", OdbcType.VarChar, 8, true),
                    new TTypedColumnInfo(1, "CountryName", "p_country_name_c", OdbcType.VarChar, 80, true),
                    new TTypedColumnInfo(2, "NationalityName", "p_nationality_name_c", OdbcType.VarChar, 80, true),
                    new TTypedColumnInfo(3, "Undercover", "p_undercover_l", OdbcType.Bit, -1, false),
                    new TTypedColumnInfo(4, "InternatTelephoneCode", "p_internat_telephone_code_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(5, "InternatPostalTypeCode", "p_internat_postal_type_code_c", OdbcType.VarChar, 16, false),
                    new TTypedColumnInfo(6, "InternatAccessCode", "p_internat_access_code_c", OdbcType.VarChar, 8, false),
                    new TTypedColumnInfo(7, "TimeZoneMinimum", "p_time_zone_minimum_n", OdbcType.Decimal, 6, false),
                    new TTypedColumnInfo(8, "TimeZoneMaximum", "p_time_zone_maximum_n", OdbcType.Decimal, 6, false),
                    new TTypedColumnInfo(9, "Deletable", "p_deletable_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(10, "AddressOrder", "p_address_order_i", OdbcType.Int, -1, false),
                    new TTypedColumnInfo(11, "CountryNameLocal", "p_country_name_local_c", OdbcType.VarChar, 80, false),
                    new TTypedColumnInfo(12, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(13, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(14, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(15, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(16, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public PCountryRow this[int i]
        {
            get
            {
                return ((PCountryRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public PCountryTable GetChangesTyped()
        {
            return ((PCountryTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetCountryCodeDBName()
        {
            return "p_country_code_c";
        }

        /// get character length for column
        public static short GetCountryCodeLength()
        {
            return 8;
        }

        /// get the name of the field in the database for this column
        public static string GetCountryNameDBName()
        {
            return "p_country_name_c";
        }

        /// get character length for column
        public static short GetCountryNameLength()
        {
            return 80;
        }

        /// get the name of the field in the database for this column
        public static string GetNationalityNameDBName()
        {
            return "p_nationality_name_c";
        }

        /// get character length for column
        public static short GetNationalityNameLength()
        {
            return 80;
        }

        /// get the name of the field in the database for this column
        public static string GetUndercoverDBName()
        {
            return "p_undercover_l";
        }

        /// get character length for column
        public static short GetUndercoverLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetInternatTelephoneCodeDBName()
        {
            return "p_internat_telephone_code_i";
        }

        /// get character length for column
        public static short GetInternatTelephoneCodeLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetInternatPostalTypeCodeDBName()
        {
            return "p_internat_postal_type_code_c";
        }

        /// get character length for column
        public static short GetInternatPostalTypeCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetInternatAccessCodeDBName()
        {
            return "p_internat_access_code_c";
        }

        /// get character length for column
        public static short GetInternatAccessCodeLength()
        {
            return 8;
        }

        /// get the name of the field in the database for this column
        public static string GetTimeZoneMinimumDBName()
        {
            return "p_time_zone_minimum_n";
        }

        /// get character length for column
        public static short GetTimeZoneMinimumLength()
        {
            return 6;
        }

        /// get the name of the field in the database for this column
        public static string GetTimeZoneMaximumDBName()
        {
            return "p_time_zone_maximum_n";
        }

        /// get character length for column
        public static short GetTimeZoneMaximumLength()
        {
            return 6;
        }

        /// get the name of the field in the database for this column
        public static string GetDeletableDBName()
        {
            return "p_deletable_l";
        }

        /// get character length for column
        public static short GetDeletableLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetAddressOrderDBName()
        {
            return "p_address_order_i";
        }

        /// get character length for column
        public static short GetAddressOrderLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCountryNameLocalDBName()
        {
            return "p_country_name_local_c";
        }

        /// get character length for column
        public static short GetCountryNameLocalLength()
        {
            return 80;
        }

        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }

        /// get character length for column
        public static short GetDateCreatedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }

        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }

        /// get character length for column
        public static short GetDateModifiedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }

        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }

        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
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
        /// It is the ISO code (ISO 3166)
        public String CountryCode
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCountryCode.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        /// This can only be updated by the system manager.
        /// At the risk of serious operational integrity.
        /// Default to Yes
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
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

        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsCountryCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCountryCode);
        }

        /// assign NULL value
        public void SetCountryCodeNull()
        {
            this.SetNull(this.myTable.ColumnCountryCode);
        }

        /// test for NULL value
        public bool IsCountryNameNull()
        {
            return this.IsNull(this.myTable.ColumnCountryName);
        }

        /// assign NULL value
        public void SetCountryNameNull()
        {
            this.SetNull(this.myTable.ColumnCountryName);
        }

        /// test for NULL value
        public bool IsNationalityNameNull()
        {
            return this.IsNull(this.myTable.ColumnNationalityName);
        }

        /// assign NULL value
        public void SetNationalityNameNull()
        {
            this.SetNull(this.myTable.ColumnNationalityName);
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
        /// This is the symbol used to show a currency. Eg $US or £
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

        /// TableId for Ict.Common.Data generic functions
        public static short TableId = 4;

        private static bool FInitInfoValues = InitInfoValues();
        private static bool InitInfoValues()
        {
            TableInfo.Add(TableId, new TTypedTableInfo(TableId, "ACurrency", "a_currency",
                new TTypedColumnInfo[] {
                    new TTypedColumnInfo(0, "CurrencyCode", "a_currency_code_c", OdbcType.VarChar, 16, true),
                    new TTypedColumnInfo(1, "CurrencyName", "a_currency_name_c", OdbcType.VarChar, 64, true),
                    new TTypedColumnInfo(2, "CurrencySymbol", "a_currency_symbol_c", OdbcType.VarChar, 8, true),
                    new TTypedColumnInfo(3, "CountryCode", "p_country_code_c", OdbcType.VarChar, 8, true),
                    new TTypedColumnInfo(4, "DisplayFormat", "a_display_format_c", OdbcType.VarChar, 40, true),
                    new TTypedColumnInfo(5, "InEmu", "a_in_emu_l", OdbcType.Bit, -1, true),
                    new TTypedColumnInfo(6, "DateCreated", "s_date_created_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(7, "CreatedBy", "s_created_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(8, "DateModified", "s_date_modified_d", OdbcType.Date, -1, false),
                    new TTypedColumnInfo(9, "ModifiedBy", "s_modified_by_c", OdbcType.VarChar, 20, false),
                    new TTypedColumnInfo(10, "ModificationId", "s_modification_id_c", OdbcType.VarChar, 150, false)
                },
                new int[] {
                    0
                }));
            return true;
        }

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
        }

        /// Access a typed row by index
        public ACurrencyRow this[int i]
        {
            get
            {
                return ((ACurrencyRow)(this.Rows[i]));
            }
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

        /// get typed set of changes
        public ACurrencyTable GetChangesTyped()
        {
            return ((ACurrencyTable)(base.GetChangesTypedInternal()));
        }

        /// get an odbc parameter for the given column
        public override OdbcParameter CreateOdbcParameter(Int32 AColumnNr)
        {
            return CreateOdbcParameter(TableId, AColumnNr);
        }

        /// get the name of the field in the database for this column
        public static string GetCurrencyCodeDBName()
        {
            return "a_currency_code_c";
        }

        /// get character length for column
        public static short GetCurrencyCodeLength()
        {
            return 16;
        }

        /// get the name of the field in the database for this column
        public static string GetCurrencyNameDBName()
        {
            return "a_currency_name_c";
        }

        /// get character length for column
        public static short GetCurrencyNameLength()
        {
            return 64;
        }

        /// get the name of the field in the database for this column
        public static string GetCurrencySymbolDBName()
        {
            return "a_currency_symbol_c";
        }

        /// get character length for column
        public static short GetCurrencySymbolLength()
        {
            return 8;
        }

        /// get the name of the field in the database for this column
        public static string GetCountryCodeDBName()
        {
            return "p_country_code_c";
        }

        /// get character length for column
        public static short GetCountryCodeLength()
        {
            return 8;
        }

        /// get the name of the field in the database for this column
        public static string GetDisplayFormatDBName()
        {
            return "a_display_format_c";
        }

        /// get character length for column
        public static short GetDisplayFormatLength()
        {
            return 40;
        }

        /// get the name of the field in the database for this column
        public static string GetInEmuDBName()
        {
            return "a_in_emu_l";
        }

        /// get character length for column
        public static short GetInEmuLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetDateCreatedDBName()
        {
            return "s_date_created_d";
        }

        /// get character length for column
        public static short GetDateCreatedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetCreatedByDBName()
        {
            return "s_created_by_c";
        }

        /// get character length for column
        public static short GetCreatedByLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetDateModifiedDBName()
        {
            return "s_date_modified_d";
        }

        /// get character length for column
        public static short GetDateModifiedLength()
        {
            return -1;
        }

        /// get the name of the field in the database for this column
        public static string GetModifiedByDBName()
        {
            return "s_modified_by_c";
        }

        /// get character length for column
        public static short GetModifiedByLength()
        {
            return 20;
        }

        /// get the name of the field in the database for this column
        public static string GetModificationIdDBName()
        {
            return "s_modification_id_c";
        }

        /// get character length for column
        public static short GetModificationIdLength()
        {
            return 150;
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// This is the symbol used to show a currency. Eg $US or £
        public String CurrencySymbol
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCurrencySymbol.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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

        /// User ID of who created this record.
        public String CreatedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnCreatedBy.Ordinal];
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

        /// User ID of who last modified this record.
        public String ModifiedBy
        {
            get
            {
                object ret;
                ret = this[this.myTable.ColumnModifiedBy.Ordinal];
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
                    throw new System.Data.StrongTypingException("Error: DB null", null);
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
        public bool IsCurrencyCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCurrencyCode);
        }

        /// assign NULL value
        public void SetCurrencyCodeNull()
        {
            this.SetNull(this.myTable.ColumnCurrencyCode);
        }

        /// test for NULL value
        public bool IsCurrencyNameNull()
        {
            return this.IsNull(this.myTable.ColumnCurrencyName);
        }

        /// assign NULL value
        public void SetCurrencyNameNull()
        {
            this.SetNull(this.myTable.ColumnCurrencyName);
        }

        /// test for NULL value
        public bool IsCurrencySymbolNull()
        {
            return this.IsNull(this.myTable.ColumnCurrencySymbol);
        }

        /// assign NULL value
        public void SetCurrencySymbolNull()
        {
            this.SetNull(this.myTable.ColumnCurrencySymbol);
        }

        /// test for NULL value
        public bool IsCountryCodeNull()
        {
            return this.IsNull(this.myTable.ColumnCountryCode);
        }

        /// assign NULL value
        public void SetCountryCodeNull()
        {
            this.SetNull(this.myTable.ColumnCountryCode);
        }

        /// test for NULL value
        public bool IsDisplayFormatNull()
        {
            return this.IsNull(this.myTable.ColumnDisplayFormat);
        }

        /// assign NULL value
        public void SetDisplayFormatNull()
        {
            this.SetNull(this.myTable.ColumnDisplayFormat);
        }

        /// test for NULL value
        public bool IsInEmuNull()
        {
            return this.IsNull(this.myTable.ColumnInEmu);
        }

        /// assign NULL value
        public void SetInEmuNull()
        {
            this.SetNull(this.myTable.ColumnInEmu);
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